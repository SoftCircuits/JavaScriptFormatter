// Copyright (c) 2020-2021 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace SoftCircuits.JavaScriptFormatter
{
    /// <summary>
    /// Low-level text parsing helper class.
    /// </summary>
    public class ParsingHelper
    {
        private static readonly char[] NewLineChars = { '\r', '\n' };

        /// <summary>
        /// Represents a invalid character. This character is returned when a valid character
        /// is not available, such as when attempting to access a character that is
        /// out-of-bounds. The character is represented as <c>'\0'</c>.
        /// </summary>
        public const char NullChar = '\0';

        /// <summary>
        /// Returns the current text being parsed.
        /// </summary>
        public string Text { get; private set; }

        /// <summary>
        /// Returns the current position within the text being parsed.
        /// </summary>
        public int Index { get; private set; }


        /// <summary>
        /// Constructs a ParsingHelper instance.
        /// </summary>
        /// <param name="text">The text to be parsed.</param>
        public ParsingHelper(string? text)
        {
            Reset(text);
        }

        /// <summary>
        /// Sets the text to be parsed and resets the current position to the start of that text.
        /// </summary>
        /// <param name="text">The text to be parsed.</param>
#if NET5_0
        [MemberNotNull(nameof(Text))]
#endif
        public void Reset(string? text)
        {
            Text = text ?? string.Empty;
            Index = 0;
        }

        /// <summary>
        /// Indicates if the current position is at the end of the text being parsed.
        /// </summary>
        public bool EndOfText => (Index >= Text.Length);

        /// <summary>
        /// Returns the number of characters not yet parsed. This is equal to the length of the
        /// text being parsed minus the current position within that text.
        /// </summary>
        public int Remaining => (Text.Length - Index);

        /// <summary>
        /// Returns the character at the current position, or <see cref="NullChar"></see> if
        /// we're at the end of the text being parsed.
        /// </summary>
        /// <returns>The character at the current position.</returns>
        public char Peek()
        {
            Debug.Assert(Index >= 0);
            return (Index < Text.Length) ? Text[Index] : NullChar;
        }

        /// <summary>
        /// Returns the character at the specified number of characters beyond the current
        /// position, or <see cref="NullChar"></see> if the specified position is out of
        /// bounds of the text being parsed.
        /// </summary>
        /// <param name="count">The number of characters beyond the current position.</param>
        /// <returns>The character at the specified position.</returns>
        public char Peek(int count)
        {
            int index = (Index + count);
            return (index >= 0 && index < Text.Length) ? Text[index] : NullChar;
        }

        /// <summary>
        /// Moves the current position ahead one character.
        /// </summary>
        public void Next()
        {
            Debug.Assert(Index >= 0);
            if (Index < Text.Length)
                Index++;
        }

        /// <summary>
        /// Moves the current position ahead the specified number of characters.
        /// </summary>
        /// <param name="count">The number of characters to move ahead. Use negative numbers
        /// to move back.</param>
        public void Next(int count)
        {
            Index += count;
            if (Index < 0)
                Index = 0;
            else if (Index > Text.Length)
                Index = Text.Length;
        }

        /// <summary>
        /// Moves the current position to the next occurrence of the specified string and returns
        /// <c>true</c> if successful. If the specified string is not found, this method moves the
        /// current position to the end of the input text and returns <c>false</c>.
        /// </summary>
        /// <param name="s">String to find.</param>
        /// <param name="comparison">One of the enumeration values that specifies the rules for
        /// search.</param>
        /// <returns>Returns a Boolean value that indicates if any of the specified characters
        /// were found.</returns>
        public bool SkipTo(string s, StringComparison comparison = StringComparison.Ordinal)
        {
            Index = Text.IndexOf(s, Index, comparison);
            if (Index == -1)
            {
                Index = Text.Length;
                return false;
            }
            return true;
        }

        /// <summary>
        /// Moves to the next occurrence of any one of the specified characters and returns
        /// <c>true</c> if successful. If none of the specified characters are found, this method
        /// moves the current position to the end of the input text and returns <c>false</c>.
        /// </summary>
        /// <param name="chars">Characters to skip to.</param>
        /// <returns>Returns a Boolean value that indicates if any of the specified characters
        /// were found.</returns>
        public bool SkipTo(params char[] chars)
        {
            Index = Text.IndexOfAny(chars, Index);
            if (Index == -1)
            {
                Index = Text.Length;
                return false;
            }
            return true;
        }

        /// <summary>
        /// Moves the current position forward to the next newline character.
        /// </summary>
        /// <returns>Returns a Boolean value that indicates if any newline characters
        /// were found.</returns>
        public bool SkipToEndOfLine() => SkipTo(NewLineChars);

        /// <summary>
        /// Moves the current position to the next character that is not whitespace.
        /// </summary>
        public void SkipWhiteSpace()
        {
            SkipWhile(char.IsWhiteSpace);
        }

        /// <summary>
        /// Moves the current position to the next character that is not one of the specified
        /// characters.
        /// </summary>
        /// <param name="chars">Characters to skip over.</param>
        public void Skip(params char[] chars)
        {
            SkipWhile(chars.Contains);
        }

        /// <summary>
        /// Moves the current position to the next character that causes <paramref name="predicate"/>
        /// to return false.
        /// </summary>
        /// <param name="predicate">Function to test each character.</param>
        public void SkipWhile(Func<char, bool> predicate)
        {
            while (!EndOfText && predicate(Peek()))
                Next();
        }

        /// <summary>
        /// Parses characters until the next occurrence of any one of the specified characters and
        /// returns a string with the parsed characters. If none of the specified characters are found,
        /// this method parses all character to the end of the input text.
        /// </summary>
        /// <param name="chars">Characters to parse until.</param>
        /// <returns>A string with the characters parsed.</returns>
        public string ParseTo(params char[] chars)
        {
            int start = Index;
            Index = Text.IndexOfAny(chars, Index);
            if (Index == -1)
                Index = Text.Length;
            return Text.Substring(start, Index - start);
        }

        /// <summary>
        /// Parses characters until the next character that causes <paramref name="predicate"/> to
        /// return false, and then returns the characters spanned. Can return an empty string.
        /// </summary>
        /// <param name="predicate">Function to test each character.</param>
        /// <returns>A string with the characters parsed.</returns>
        public string ParseWhile(Func<char, bool> predicate)
        {
            int start = Index;
            while (!EndOfText && predicate(Peek()))
                Next();
            return Text.Substring(start, Index - start);
        }

        /// <summary>
        /// Compares the given string to text at the current position using a case-sensitive comparison.
        /// </summary>
        /// <remarks>Testing showed this is the fastest way to compare part of a string. Faster even
        /// than using Span&lt;T&gt;.</remarks>
        /// <param name="s">String to compare.</param>
        /// <returns>Returns <c>true</c> if the given string matches the text at the current position.
        /// Returns false otherwise.</returns>
        public bool MatchesCurrentPosition(string s)
        {
            if (s == null || s.Length == 0 || Remaining < s.Length)
                return false;

            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] != Text[Index + i])
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Extracts a substring from the specified range of the text being parsed.
        /// </summary>
        /// <param name="start">0-based position of first character to be extracted.</param>
        /// <param name="end">0-based position of the character that follows the last
        /// character to be extracted.</param>
        /// <returns>Returns the extracted string.</returns>
        public string Extract(int start, int end) => Text.Substring(start, end - start);

        #region Operator overloads

        public static implicit operator int(ParsingHelper helper) => helper.Index;

        /// <summary>
        /// Move the current position ahead one character.
        /// </summary>
        public static ParsingHelper operator ++(ParsingHelper helper)
        {
            helper.Next(1);
            return helper;
        }

        /// <summary>
        /// Move the current position back one character.
        /// </summary>
        public static ParsingHelper operator --(ParsingHelper helper)
        {
            helper.Next(-1);
            return helper;
        }

        /// <summary>
        /// Moves the current position ahead (or back) by the specified
        /// number of characters.
        /// </summary>
        public static ParsingHelper operator +(ParsingHelper helper, int count)
        {
            helper.Next(count);
            return helper;
        }

        /// <summary>
        /// Moves the current position back (or ahead) by the specified
        /// number of characters.
        /// </summary>
        public static ParsingHelper operator -(ParsingHelper helper, int count)
        {
            helper.Next(-count);
            return helper;
        }

        #endregion

    }
}
