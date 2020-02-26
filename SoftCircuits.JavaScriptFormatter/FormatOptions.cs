// Copyright (c) 2020 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//

namespace SoftCircuits.JavaScriptFormatter
{
    /// <summary>
    /// Specifies options used by <see cref="JavaScriptFormatter"></see>.
    /// </summary>
    public class FormatOptions
    {
        /// <summary>
        /// Specifies the string uses for each indentation. Set to 4 spaces
        /// by default.
        /// </summary>
        public string Tab { get; set; }

        /// <summary>
        /// Gets or sets if an empty line is inserted between functions.
        /// Set to true by default.
        /// </summary>
        public bool NewLineBetweenFunctions { get; set; }

        /// <summary>
        /// Gets or sets if opening braces go on a new line. Set to false
        /// by default.
        /// </summary>
        public bool OpenBraceOnNewLine { get; set; }

        /// <summary>
        /// Gets or sets if line comments go on new line. Set to true by
        /// default.
        /// </summary>
        public bool NewLineBeforeLineComment { get; set; }

        /// <summary>
        /// Gets or sets if inline comments go on a new line. Set to true
        /// by default.
        /// </summary>
        public bool NewLineBeforeInlineComment { get; set; }

        /// <summary>
        /// Gets or sets if a new line should follow inline comments. Set
        /// to true by default
        /// </summary>
        public bool NewLineAfterInlineComment { get; set; }

        public FormatOptions()
        {
            Tab = "    ";
            NewLineBetweenFunctions = true;
            OpenBraceOnNewLine = false;
            NewLineBeforeLineComment = true;
            NewLineBeforeInlineComment = true;
            NewLineAfterInlineComment = true;
        }
    }
}
