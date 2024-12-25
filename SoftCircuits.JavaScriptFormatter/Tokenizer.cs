// Copyright (c) 2020-2024 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//

using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace SoftCircuits.JavaScriptFormatter
{
    /// <summary>
    /// Parses JavaScript into a sequence of tokens.
    /// </summary>
    internal class Tokenizer : ParsingHelper
    {
        protected static readonly HashSet<char> FirstSymbolChars = new("$_abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ");
        protected static readonly HashSet<char> SymbolChars = new("$_abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890");
        protected static readonly HashSet<char> OctalChars = new("01234567");
        protected static readonly HashSet<char> HexadecimalChars = new("0123456789abcdefABCDEF");
        protected static readonly HashSet<char> DecimalChars = new("0123456789.eE");
        protected static readonly HashSet<char> OperatorChars = new("+-*/%=!<>&|?:~{}()[].;,^");

        private static readonly Dictionary<char, TokenType> NamedOperators = new()
        {
            ['{'] = TokenType.OpenBrace,
            ['}'] = TokenType.CloseBrace,
            ['('] = TokenType.OpenParen,
            [')'] = TokenType.CloseParen,
            ['['] = TokenType.OpenBracket,
            [']'] = TokenType.CloseBracket,
            [':'] = TokenType.Colon,
            ['.'] = TokenType.Dot,
            [';'] = TokenType.SemiColon,
            [','] = TokenType.Comma,
            ['?'] = TokenType.QuestionMark,
        };

        // IMPORTANT: Longer operators must come first to avoid partial match with shorter operator
        protected static readonly string[] MultiCharOperators =
        [
            ">>>=",
            "===",
            "!==",
            ">>>",
            "<<=",
            ">>=",
            "==",
            "!=",
            "<=",
            ">=",
            "+=",
            "-=",
            "*=",
            "/=",
            "%=",
            "&&",
            "||",
            "++",
            "--",
            "^=",
            "~=",
            "|=",
            "&=",
            "<<",
            ">>"
        ];

        public Token? CurrentToken { get; private set; }
        private Token? PendingToken;
        private readonly Token EmptyToken = new() { Value = string.Empty };

        /// <summary>
        /// Constructs a new Tokenizer instance.
        /// </summary>
        /// <param name="script">The JavaScript script to be tokenized.</param>
        public Tokenizer(string? script)
            : base(script)
        {
            CurrentToken = PendingToken = null;
        }

        /// <summary>
        /// Ungets the current token so that it will become the current token
        /// after the next call to GetToken().
        /// </summary>
        public void UngetToken()
        {
            // Don't overwrite pending token
            Debug.Assert(PendingToken == null);
            PendingToken = CurrentToken;
        }

        public Token PeekToken()
        {
            if (GetToken())
            {
                Token token = CurrentToken;
                UngetToken();
                return token;
            }
            return EmptyToken;
        }

        /// <summary>
        /// Parses the next token from the input string. Returns false
        /// when no more tokens are available.
        /// </summary>
#if !NETSTANDARD2_0
        [MemberNotNullWhen(true, nameof(CurrentToken))]
#endif
        public bool GetToken()
        {
            // Return pending token, if any
            if (PendingToken != null)
            {
                CurrentToken = PendingToken;
                PendingToken = null;
                return true;
            }

            // Ignore whitespace
            SkipWhiteSpace();

            // Test for end of input
            if (EndOfText)
                return false;

            // Create new token
            Token token = new(CurrentToken);

            // Parse token
            int start = Index;
            if (FirstSymbolChars.Contains(Peek()))
            {
                // Symbol (keywords, variables, function names, etc.)
                token.Type = TokenType.Symbol;
                Next();
                SkipWhile(SymbolChars.Contains);
            }
            else if (Peek() == '/' && Peek(1) == '/')
            {
                // Line comment
                token.Type = TokenType.LineComment;
                SkipToEndOfLine();
            }
            else if (Peek() == '/' && Peek(1) == '*')
            {
                // Inline comment
                token.Type = TokenType.InlineComment;
                SkipTo("*/");
                Next(2);
            }
            else if (Peek() == '\'' || Peek() == '"')
            {
                // String or character constant
                token.Type = TokenType.String;
                char quote = Peek();
                Next();
                while (!EndOfText)
                {
                    if (Peek() == quote)
                    {
                        Next();
                        break;
                    }
                    else if (Peek() == '\\' && (Peek(1) == '\\' || Peek(1) == quote))
                        Next(2);
                    else
                        Next();
                }
            }
            else if (char.IsDigit(Peek()) || (Peek() == '.' && char.IsDigit(Peek(1))))
            {
                // Numeric constant
                token.Type = TokenType.Number;
                HashSet<char> chars = DecimalChars;
                if (Peek() == '0')
                {
                    if (char.IsDigit(Peek(1)))
                        chars = OctalChars;
                    else if (char.ToLower(Peek(1)) == 'x')
                    {
                        chars = HexadecimalChars;
                        Next(2);
                    }
                }

                bool hasDecimal = false;
                bool hasExponential = false;
                while (chars.Contains(Peek()))
                {
                    if (Peek() == '.')
                    {
                        // Ensure no more than one decimal point
                        if (hasDecimal)
                            break;
                        hasDecimal = true;
                    }
                    else if (char.ToLower(Peek()) == 'e' && chars == DecimalChars)
                    {
                        // Ensure no more than one exponential notation
                        if (hasExponential)
                            break;
                        hasExponential = true;
                        // Skip exponential notation sign
                        if (Peek(1) == '+' || Peek(1) == '-')
                            Next();
                    }
                    Next();
                }
            }
            else if (Peek() == '/' &&
                token.PreviousType != TokenType.CloseParen &&
                token.PreviousType != TokenType.CloseBracket &&
                (token.PreviousType != TokenType.Symbol ||
                token.PreviousValue == "return" ||
                token.PreviousValue == "yield") &&
                token.PreviousType != TokenType.String &&
                token.PreviousType != TokenType.Number &&
                token.PreviousType != TokenType.RegEx &&
                token.PreviousType != TokenType.Dot &&
                token.PreviousType != TokenType.UnarySuffix)
            {
                // Regular expression constant
                token.Type = TokenType.RegEx;
                Next();

                // Scan regular expression
                bool inCaptureGroup = false;
                while (!EndOfText && (Peek() != '/' || inCaptureGroup) &&
                    Peek() != '\r' && Peek() != '\n')
                {
                    if (Peek() == '[')
                    {
                        inCaptureGroup = true;
                    }
                    else if (Peek() == ']')
                    {
                        inCaptureGroup = false;
                    }
                    else
                    {
                        // Skip over escaped characters with special meaning
                        char c = Peek(1);
                        if (Peek() == '\\' && (c == '\\' || c == '/' || c == ']' || c == '['))
                            Next();
                    }
                    Next();
                }

                // Scan regular expression flags
                if (Peek() == '/')
                {
                    Next();
                    while (char.IsLetter(Peek()))
                        Next();
                }
            }
            else if (OperatorChars.Contains(Peek()))
            {
                // Operator
                if (NamedOperators.TryGetValue(Peek(), out TokenType namedType))
                {
                    // Matches a specialized operator
                    token.Type = namedType;
                    Next();
                }
                else
                {
                    // Parse multi-character operator
                    string? op = MultiCharOperators.FirstOrDefault(s => MatchesCurrentPosition(s));
                    if (op != null)
                        Next(op.Length);
                    else
                    {
                        op = Peek().ToString();
                        Next();
                    }
                    // Determine operator type
                    if (op == "~" || op == "!")
                    {
                        token.Type = TokenType.UnaryPrefix;
                    }
                    else if (op == "++" || op == "--")
                    {
                        if (token.PreviousType == TokenType.Symbol ||
                            token.PreviousType == TokenType.CloseParen ||
                            token.PreviousType == TokenType.CloseBracket)
                            token.Type = TokenType.UnarySuffix;
                        else
                            token.Type = TokenType.UnaryPrefix;
                    }
                    else if (op == "+" || op == "-")
                    {
                        if (token.PreviousType == TokenType.Symbol ||
                            token.PreviousType == TokenType.Number ||
                            token.PreviousType == TokenType.String ||
                            token.PreviousType == TokenType.RegEx ||
                            token.PreviousType == TokenType.CloseParen ||
                            token.PreviousType == TokenType.CloseBracket)
                            token.Type = TokenType.BinaryOperator;
                        else
                            token.Type = TokenType.UnaryPrefix;
                    }
                    else token.Type = TokenType.BinaryOperator;
                }
            }
            else
            {
                // Unknown token type
                token.Type = TokenType.Unknown;
                Next();
            }

            // Extract token value
            token.Value = Extract(start, Index);

            // Update current token and indicate success
            CurrentToken = token;
            return true;
        }
    }
}
