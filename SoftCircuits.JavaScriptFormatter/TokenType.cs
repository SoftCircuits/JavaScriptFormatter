// Copyright (c) 2020-2024 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//

namespace SoftCircuits.JavaScriptFormatter
{
    /// <summary>
    /// Enumeration of available token types.
    /// </summary>
    internal enum TokenType
    {
        Unknown,
        OpenBrace,
        CloseBrace,
        OpenParen,
        CloseParen,
        OpenBracket,
        CloseBracket,
        Symbol,
        String,
        Number,
        RegEx,
        SemiColon,
        Comma,
        Colon,
        Dot,
        QuestionMark,
        BinaryOperator,
        UnaryPrefix,
        UnarySuffix,
        LineComment,
        InlineComment,
    }
}
