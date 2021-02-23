// Copyright (c) 2020-2021 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//

using System;
using System.Collections.Generic;
using System.Text;

namespace SoftCircuits.JavaScriptFormatter
{
    /// <summary>
    /// Class to format JavaScript.
    /// </summary>
    public class JavaScriptFormatter
    {
        private readonly FormatOptions FormatOptions;
        private StringBuilder Builder;
        private Indents Indents;
        private int ParenCount;         // Line parentheses depth
        private int BracketCount;       // Line bracket depth
        private LineFlags LineFlags;    // Line flags
        private LineFlags NextLineFlags;

        // Keywords that cause indent
        // Note: 'case' and 'default' have special handling
        private static readonly HashSet<string> BlockKeywords = new HashSet<string>
        {
            "catch",
            "do",
            "finally",
            "for",
            "if",
            "switch",
            "try",
            "while",
            "with"
        };

        /// <summary>
        /// Constructs a new JavaScriptFormatter instance.
        /// </summary>
        /// <param name="formatOptions">Optional format options instance. Specify to
        /// override default options.</param>
        public JavaScriptFormatter(FormatOptions? formatOptions = null)
        {
            // Create default options if none specified
            FormatOptions = formatOptions ?? new FormatOptions();

            Builder = new StringBuilder();
            Indents = new Indents(FormatOptions.Tab);
        }

        /// <summary>
        /// Formats the given JavaScript string.
        /// </summary>
        /// <param name="javascript">JavaScript script to format.</param>
        /// <returns>The formatted string.</returns>
        public string Format(string javascript)
        {
            Tokenizer tokenizer = new Tokenizer(javascript);
            bool endLine = false;       // Cause new line
            bool isLineStart = true;    // Current token is first on line
            Token peek;

            Builder.Capacity = javascript.Length;
            Indents.Reset();
            ParenCount = 0;
            BracketCount = 0;
            LineFlags = LineFlags.None;
            NextLineFlags = LineFlags.None;

            // Process each token in input string
            while (tokenizer.GetToken())
            {
                // Get current token
                Token token = tokenizer.CurrentToken;

                // Test for new line
                if (Builder.Length > 0)
                {
                    isLineStart = endLine;
                    if (endLine)
                    {
                        NewLine();
                        endLine = false;
                    }
                }

                // Process this token
                switch (token.Type)
                {
                    case TokenType.OpenBrace:
                        if (!isLineStart)
                        {
                            if (FormatOptions.OpenBraceOnNewLine && Builder.Length > 0)
                            {
                                // Put open brace on new line
                                NewLine();
                            }
                            else
                            {
                                // Put open brace on same line
                                if (token.PreviousType != TokenType.OpenParen &&
                                    token.PreviousType != TokenType.OpenBracket)
                                    Builder.Append(' ');
                            }
                        }

                        // Write token
                        Builder.Append(token.Value);

                        // Start new indent block
                        peek = tokenizer.PeekToken();
                        if (peek.Type == TokenType.CloseBrace)
                        {
                            // Special handling for "{}"
                            tokenizer.GetToken();
                            Builder.Append(tokenizer.CurrentToken.Value);
                            peek = tokenizer.PeekToken();
                            if (peek.Type != TokenType.SemiColon &&
                                peek.Type != TokenType.Comma)
                            {
                                // Unindent if in conditional block without braces
                                while (Indents.Current.HasFlag(IndentFlags.NoBraces))
                                    Indents.Unindent();
                                endLine = true;
                            }
                            else if (peek.Type == TokenType.Comma)
                            {
                                // Normally, we insert a new line after
                                // a closing brace and comma but not here
                                tokenizer.GetToken();
                                Builder.Append(tokenizer.CurrentToken.Value);
                            }
                        }
                        else
                        {
                            // Increase indentation
                            IndentFlags flags = IndentFlags.None;
                            if (LineFlags.HasFlag(LineFlags.DoKeyword))
                                flags |= IndentFlags.DoBlock;
                            else if (LineFlags.HasFlag(LineFlags.CaseKeyword))
                                flags |= IndentFlags.CaseBlock;

                            Indents.Indent(flags);
                            endLine = true;
                        }
                        break;

                    case TokenType.CloseBrace:
                        // End indent block
                        if (Indents.Current.HasFlag(IndentFlags.CaseBlock))
                        {
                            // Extra unindent if in case/default block
                            Indents.Unindent();
                            if (isLineStart)
                                Indents.StripTrailingIndent(Builder);
                        }

                        // Unindent if in conditional block without braces
                        while (Indents.Current.HasFlag(IndentFlags.NoBraces))
                            Indents.Unindent();

                        // Regular unindent
                        Indents.Unindent();
                        if (isLineStart)
                            Indents.StripTrailingIndent(Builder);
                        else
                            NewLine();
                        Builder.Append(token.Value);

                        // Don't unindent without braces for catch/finally
                        peek = tokenizer.PeekToken();
                        if (peek.Value != "catch" &&
                            peek.Value != "finally" &&
                            peek.Value != ":")
                        {
                            // Unindent if in conditional block without braces
                            while (Indents.Current.HasFlag(IndentFlags.NoBraces))
                                Indents.Unindent();
                        }

                        if (Indents.LastIndent.HasFlag(IndentFlags.DoBlock))
                            LineFlags |= LineFlags.EndDoBlock;

                        // Insert new line after code block
                        if (peek.Type != TokenType.SemiColon &&
                            peek.Type != TokenType.CloseParen &&
                            peek.Type != TokenType.CloseBracket &&
                            peek.Type != TokenType.Comma &&
                            peek.Type != TokenType.OpenParen &&
                            peek.Type != TokenType.Colon &&
                            !LineFlags.HasFlag(LineFlags.EndDoBlock))
                        {
                            endLine = true;
                        }
                        break;

                    case TokenType.OpenParen:
                        if (!isLineStart &&
                            token.PreviousType != TokenType.OpenParen &&
                            token.PreviousType != TokenType.UnaryPrefix &&
                            token.PreviousType != TokenType.CloseBracket &&
                            token.PreviousType != TokenType.CloseParen &&
                            token.PreviousType != TokenType.CloseBrace &&
                            (token.PreviousType != TokenType.Symbol ||
                            (LineFlags.HasFlag(LineFlags.BlockKeyword) &&
                            ParenCount == 0)))
                            Builder.Append(' ');
                        Builder.Append(token.Value);
                        ParenCount++;
                        break;

                    case TokenType.CloseParen:
                        // Append closing parenthesis
                        Builder.Append(token.Value);
                        ParenCount = Math.Max(ParenCount - 1, 0);

                        // Test for indent block start without braces
                        if (ParenCount == 0 &&
                            LineFlags.HasFlag(LineFlags.BlockKeyword))
                        {
                            // Examine next token
                            peek = tokenizer.PeekToken();
                            if (peek.Type != TokenType.OpenBrace)
                            {
                                // Single line indent with no conditions or braces
                                Indents.Indent(IndentFlags.NoBraces);
                                endLine = true;
                            }
                        }
                        break;

                    case TokenType.OpenBracket:
                        if (!isLineStart &&
                            token.PreviousType != TokenType.Symbol &&
                            token.PreviousType != TokenType.OpenParen &&
                            token.PreviousType != TokenType.CloseParen &&
                            token.PreviousType != TokenType.CloseBracket)
                            Builder.Append(' ');

                        // Special handling for JSON syntax?
                        peek = tokenizer.PeekToken();
                        if (LineFlags.HasFlag(LineFlags.JsonColon) &&
                            peek.Type != TokenType.CloseBracket &&
                            peek.Type == TokenType.OpenBrace &&
                            ParenCount == 0)
                        {
                            if (FormatOptions.OpenBraceOnNewLine)
                                NewLine();
                            Indents.Indent(IndentFlags.BracketBlock);
                            endLine = true;
                        }
                        Builder.Append(token.Value);
                        BracketCount++;
                        break;

                    case TokenType.CloseBracket:
                        BracketCount = Math.Max(BracketCount - 1, 0);
                        if (Indents.Current.HasFlag(IndentFlags.BracketBlock))
                        {
                            Indents.Unindent();
                            if (isLineStart)
                            {
                                Indents.StripTrailingIndent(Builder);
                                Builder.Append(token.Value);
                            }
                            else
                            {
                                NewLine();
                                Builder.Append(token.Value);
                            }
                        }
                        else Builder.Append(token.Value);
                        break;

                    case TokenType.Symbol:

                        bool isBlockKeyword = BlockKeywords.Contains(token.Value);

                        // Special handling for function
                        if (token.Value == "function" && FormatOptions.NewLineBetweenFunctions && token.PreviousType == TokenType.CloseBrace)
                            NewLine();

                        // Special handling for else without if
                        if (token.Value == "else" && tokenizer.PeekToken().Value != "if")
                            isBlockKeyword = true;

                        // Special handling for switch..case..default
                        if (Indents.Current.HasFlag(IndentFlags.CaseBlock) && (token.Value == "case" || token.Value == "default"))
                        {
                            Indents.StripTrailingIndent(Builder);
                            Indents.Unindent();
                        }

                        if (ParenCount == 0 && isBlockKeyword)
                        {
                            // Keyword that starts an indented block
                            if (!isLineStart)
                                Builder.Append(' ');
                            // Append this symbol
                            Builder.Append(token.Value);

                            if (!LineFlags.HasFlag(LineFlags.EndDoBlock) ||
                                token.Value != "while")
                            {
                                // Test for special-case blocks
                                if (token.Value == "do")
                                    LineFlags |= LineFlags.DoKeyword;
                                // Examine next token
                                peek = tokenizer.PeekToken();
                                if (peek.Type == TokenType.OpenBrace ||
                                    peek.Type == TokenType.OpenParen)
                                {
                                    // Handle indentation at ')' or '{'
                                    LineFlags |= LineFlags.BlockKeyword;
                                }
                                else
                                {
                                    // Single line indent with no conditions or braces
                                    IndentFlags flags = IndentFlags.NoBraces;
                                    if (LineFlags.HasFlag(LineFlags.DoKeyword))
                                        flags |= IndentFlags.DoBlock;

                                    Indents.Indent(flags);
                                    endLine = true;
                                }
                            }
                        }
                        else
                        {
                            // All other symbols
                            if (!isLineStart &&
                                token.PreviousType != TokenType.OpenParen &&
                                token.PreviousType != TokenType.OpenBracket &&
                                token.PreviousType != TokenType.UnaryPrefix &&
                                token.PreviousType != TokenType.Dot)
                                Builder.Append(' ');

                            // Flag line for case block
                            if (token.Value == "case" || token.Value == "default")
                                LineFlags |= LineFlags.CaseKeyword;

                            Builder.Append(token.Value);
                        }
                        break;

                    case TokenType.String:
                    case TokenType.Number:
                    case TokenType.RegEx:
                        // Emit constant
                        if (!isLineStart &&
                            token.PreviousType != TokenType.OpenParen &&
                            token.PreviousType != TokenType.OpenBracket &&
                            token.PreviousType != TokenType.UnaryPrefix)
                            Builder.Append(' ');
                        Builder.Append(token.Value);
                        break;

                    case TokenType.SemiColon:
                        Builder.Append(token.Value);
                        if (ParenCount == 0)
                        {
                            // Unindent if in conditional block without braces
                            while (Indents.Current.HasFlag(IndentFlags.NoBraces))
                                Indents.Unindent();
                            if (Indents.LastIndent.HasFlag(IndentFlags.DoBlock))
                                NextLineFlags |= LineFlags.EndDoBlock;

                            // Determine if end of single-line indent block
                            peek = tokenizer.PeekToken();
                            if (peek.Type == TokenType.LineComment ||
                                peek.Type == TokenType.InlineComment)
                            {
                                bool newLine;
                                if (peek.Type == TokenType.LineComment)
                                    newLine = FormatOptions.NewLineBeforeLineComment;
                                else
                                    newLine = FormatOptions.NewLineBeforeInlineComment;

                                tokenizer.GetToken();
                                if (newLine)
                                    NewLine();
                                else
                                    Builder.Append(' ');
                                Builder.Append(tokenizer.CurrentToken.Value);
                            }

                            endLine = true;
                        }
                        break;

                    case TokenType.Comma:
                        Builder.Append(token.Value);
                        // Append newline if it looks like JSON syntax
                        if (token.PreviousType == TokenType.CloseBrace ||
                            (LineFlags.HasFlag(LineFlags.JsonColon) &&
                            ParenCount == 0 &&
                            BracketCount == 0 &&
                            Indents.Count > 0))
                            endLine = true;
                        break;

                    case TokenType.Colon:
                        if (!LineFlags.HasFlag(LineFlags.CaseKeyword))
                        {
                            // Standard colon handling
                            if (!isLineStart &&
                                (LineFlags.HasFlag(LineFlags.QuestionMark) ||
                                token.PreviousType == TokenType.CloseBrace))
                                Builder.Append(' ');
                            Builder.Append(token.Value);
                            // May be JSON syntax
                            if (!LineFlags.HasFlag(LineFlags.QuestionMark))
                                LineFlags |= LineFlags.JsonColon;
                        }
                        else
                        {
                            // Special handling for case and default
                            Builder.Append(token.Value);
                            Indents.Indent(IndentFlags.CaseBlock);
                            endLine = true;
                        }
                        break;

                    case TokenType.QuestionMark:
                        LineFlags |= LineFlags.QuestionMark;
                        if (!isLineStart)
                            Builder.Append(' ');
                        Builder.Append(token.Value);
                        break;

                    case TokenType.BinaryOperator:
                    case TokenType.UnaryPrefix:
                        if (!isLineStart &&
                            token.PreviousType != TokenType.OpenParen &&
                            token.PreviousType != TokenType.OpenBracket &&
                            token.PreviousType != TokenType.UnaryPrefix)
                            Builder.Append(' ');
                        Builder.Append(token.Value);
                        break;

                    case TokenType.LineComment:
                        // Separate line comment from previous token
                        if (!isLineStart)
                        {
                            if (FormatOptions.NewLineBeforeLineComment)
                                NewLine();              // Separate with new line
                            else
                                Builder.Append(' ');   // Separate with space
                        }
                        // Append comment
                        Builder.Append(token.Value);
                        // Line comment always followed by new line
                        endLine = true;
                        break;

                    case TokenType.InlineComment:
                        // Separate line comment from previous token
                        if (!isLineStart)
                        {
                            if (FormatOptions.NewLineBeforeInlineComment)
                                NewLine();              // Separate with new line
                            else
                                Builder.Append(' ');   // Separate with space
                        }
                        // Append comment
                        Builder.Append(token.Value);
                        // New line after comment
                        if (FormatOptions.NewLineAfterInlineComment)
                            endLine = true;
                        break;

                    default:
                        Builder.Append(token.Value);
                        break;
                }
            }

            Builder.AppendLine();

            return Builder.ToString();
        }

        /// <summary>
        /// Emits a new line to the output string.
        /// </summary>
        protected void NewLine()
        {
            Builder.AppendLine();
            Builder.Append(Indents.ToString());

            BracketCount = ParenCount = 0;
            LineFlags = NextLineFlags;
            NextLineFlags = LineFlags.None;
        }
    }
}
