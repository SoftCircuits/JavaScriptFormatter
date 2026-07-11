// Copyright (c) 2020-2026 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace SoftCircuits.JavaScriptFormatter.Enum
#pragma warning restore IDE0130 // Namespace does not match folder structure
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
