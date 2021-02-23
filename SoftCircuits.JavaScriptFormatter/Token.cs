// Copyright (c) 2020-2021 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//

namespace SoftCircuits.JavaScriptFormatter
{
    /// <summary>
    /// Represents a single JavaScript token. For context, it also contains
    /// information about the previous token.
    /// </summary>
    internal class Token
    {
        public string Value { get; set; }
        public TokenType Type { get; set; }
        public string? PreviousValue { get; set; }
        public TokenType PreviousType { get; set; }

        /// <summary>
        /// Constructs a new Token instance.
        /// </summary>
        /// <param name="previousToken">The previous token, if any.</param>
        public Token(Token? previousToken = null)
        {
            Value = string.Empty;
            if (previousToken != null)
            {
                PreviousValue = previousToken.Value;
                PreviousType = previousToken.Type;
            }
        }
    }
}
