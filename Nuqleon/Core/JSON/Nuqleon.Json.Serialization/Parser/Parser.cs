// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 04/05/2016 - Created fast JSON deserializer functionality.
//   BD - 05/08/2016 - Added support for deserialization from text readers.
//   BD - 01/25/2019 - Leverage ReadOnlySpan<char> based parsing.
//

using Nuqleon.Json.Parser;
#if USE_SPAN
using System;
#endif
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace Nuqleon.Json.Serialization
{
    internal partial class Parser
    {
        //
        // CONSIDER: Add support for TimeSpan using ISO-8601 duration notation.
        // CONSIDER: Add support for deserialization of any type that has a Parse or .ctor taking in a String[Segment].
        //

        /// <summary>
        /// Skips whitespace from the specified string starting from the specified index.
        /// </summary>
        /// <param name="str">The string to consume whitespace from.</param>
        /// <param name="len">The length of the string.</param>
        /// <param name="i">The index in the string to start lexing from. This value gets updated to the first index position after the whitespace, if found.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void SkipWhiteSpace(string str, int len, ref int i)
        {
            //
            // CONSIDER: Evaluate the use of more sophisticated whitespace skipping for consecutive whitespace, which is common for
            //           formatted JSON documents with indentation. One possible strategy is to use unsafe code and check the next
            //           four characters (if available) against a few commonly occurring long values:
            //
            //              "    "     = ((long)(' ' ) << 0) || ((long)(' ' ) << 16) || ((long)(' ' ) << 32) || ((long)(' ' ) << 48)
            //              "\t\t\t\t" = ((long)('\t') << 0) || ((long)('\t') << 16) || ((long)('\t') << 32) || ((long)('\t') << 48)
            //              "\r\n  "   = ((long)('\r') << 0) || ((long)('\n') << 16) || ((long)(' ' ) << 32) || ((long)(' ' ) << 48)
            //              "\n   "    = ((long)('\n') << 0) || ((long)(' ' ) << 16) || ((long)(' ' ) << 32) || ((long)(' ' ) << 48)
            //              "\n\t\t\t" = ((long)('\n') << 0) || ((long)('\t') << 16) || ((long)('\t') << 32) || ((long)('\t') << 48)
            //
            //           If there's no match, we can mask off the last two characters, and check the remaining 4 bytes against the
            //           result of masking all the constants above (i.e. checking their first two characters). If there's still no
            //           match, we resort to the traditional character-by-character loop.
            //
            //           The concern is that we'd waste quite a few cycles on an "optimized" JSON document where whitespace has been
            //           trimmed away, so we have to measure whether the gains outweigh the losses, or come up with a strategy to
            //           minimize the losses. Some options are to allow for hints when creating a serializer, or threading the context
            //           all the way to this helper function and check/set a Boolean flag that indicates whether the advanced whitespace
            //           skipping strategy is working for the current input (i.e. self-learning optimization).
            //
            //           Another concern is that there may be quite a few places where single spaces are commonly used, e.g. after :
            //           and , tokens or around { } [ ] tokens. The additional checks there will have to outweigh the cost of a
            //           string indexing operation and the IsWhiteSpace call, which may be hard to beat.
            //

            while (i < len && char.IsWhiteSpace(str[i]))
                i++;
        }

#if !NO_IO
        /// <summary>
        /// Skips whitespace from the specified text reader.
        /// </summary>
        /// <param name="reader">The text reader to consume whitespace from.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void SkipWhiteSpace(System.IO.TextReader reader)
        {
            //
            // NB: TextReader.Peek is well-defined to return -1 (and not any negative value) when no input is available.
            //     The char representation of -1 is \uffff which is not a whitespace character, so the test below is easy.
            //

            while (char.IsWhiteSpace((char)reader.Peek()))
                reader.Read();
        }
#endif

        /// <summary>
        /// Tries to lex a JSON null literal in the specified string starting from the specified index.
        /// </summary>
        /// <param name="str">The string to lex and parse a null literal from.</param>
        /// <param name="len">The length of the string.</param>
        /// <param name="i">The index in the string to start lexing from. This value gets updated to the first index position after the null literal, if found.</param>
        /// <returns>true if a null literal was found; otherwise, false.</returns>
        /// <remarks>
        /// This method does not consume whitespace; the character at the specified start index in the string is expected to be the start of a valid JSON null literal.
        /// JSON null iterals are case-sensitive.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static bool TryParseNullSkipN(string str, int len, ref int i)
        {
            if (i + 4 <= len)
            {
#if ALLOW_UNSAFE
                unsafe
                {
                    const long NULL = ((long)'n' << 0) | ((long)'u' << 16) | ((long)'l' << 32) | ((long)'l' << 48);

                    fixed (char* s = str)
                    {
                        var c = s + i;
                
                        if (*((long*)c) == NULL)
                        {
                            i += 4;
                            return true;
                        }
                    }
                }
#else
                if (str[i + 1] == 'u' && str[i + 2] == 'l' && str[i + 3] == 'l')
                {
                    i += 4;
                    return true;
                }
#endif
            }

            return false;
        }

#if !NO_IO
        /// <summary>
        /// Tries to lex a JSON null literal in the specified text reader.
        /// </summary>
        /// <param name="reader">The text reader to lex and parse a null literal from.</param>
        /// <param name="context">The parser context.</param>
        /// <returns>true if a null literal was found; otherwise, false.</returns>
        /// <remarks>
        /// This method does not consume whitespace; the character at the specified start index in the string is expected to be the start of a valid JSON null literal.
        /// JSON null iterals are case-sensitive.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static bool TryParseNullSkipN(System.IO.TextReader reader, ParserContext context)
        {
#if ALLOW_UNSAFE
            if (reader.Read(context.char4, 0, 4) == 4)
            {
                unsafe
                {
                    const long NULL = ((long)'n' << 0) | ((long)'u' << 16) | ((long)'l' << 32) | ((long)'l' << 48);

                    fixed (char* s = context.char4)
                    {
                        var val = *((long*)s);

                        if (val == NULL)
                        {
                            return true;
                        }
                    }
                }
            }
#else
            _ = context; // NB: Suppress unused parameter warning; gets optimized out.

            reader.Read();

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable IDE0078 // Use pattern matching. (`reader.Read()` has a side-effect and should not get removed; see https://github.com/dotnet/roslyn-analyzers/issues/4690.)
            if (reader.Read() == 'u' && reader.Read() == 'l' && reader.Read() == 'l')
            {
                return true;
            }
#pragma warning restore IDE0078
#pragma warning disable IDE0079
#endif

            return false;
        }
#endif

        /// <summary>
        /// Tries to parse an Int32 value in the specified string starting from the specified index and spanning the specified number of characters.
        /// </summary>
        /// <param name="str">The string to lex and parse an Int32 value from.</param>
        /// <param name="count">The number of characters to read.</param>
        /// <param name="i">The index in the string to start lexing from. This value gets updated to the first index position after the number, if found.</param>
        /// <param name="res">The number that was parsed, if found.</param>
        /// <returns>true if all <paramref name="count"/> characters starting from <paramref name="i"/> are digits; otherwise, false.</returns>
        /// <remarks>
        /// This method assumes that all indexes in the range [i, i + count - 1] fall within the string. It's the caller's responsibility to ensure that's the case.
        /// </remarks>
        private static bool TryParseInt32(string str, int count, ref int i, out int res)
        {
            Debug.Assert(i + count <= str.Length);

            res = 0;

            for (var n = 0; n < count; n++)
            {
                var c = str[i++];

                if (c is < '0' or > '9')
                {
                    return false;
                }

                res = res * 10 + (c - '0');
            }

            return true;
        }

#if !NO_IO
        /// <summary>
        /// Tries to parse an Int32 value in the specified string text reader and spanning the specified number of characters.
        /// </summary>
        /// <param name="reader">The text reader to lex and parse an Int32 value from.</param>
        /// <param name="count">The number of characters to read.</param>
        /// <param name="res">The number that was parsed, if found.</param>
        /// <returns>true if all <paramref name="count"/> characters read from the text reader are digits; otherwise, false.</returns>
        private static bool TryParseInt32(System.IO.TextReader reader, int count, out int res)
        {
            res = 0;

            for (var n = 0; n < count; n++)
            {
                var c = reader.Read();

                if (c is < '0' or > '9')
                {
                    return false;
                }

                res = res * 10 + (c - '0');
            }

            return true;
        }
#endif

        /// <summary>
        /// Checks whether the given character specifies a hexadecimal digit [0-9A-Fa-f].
        /// </summary>
        /// <param name="c">The character to check</param>
        /// <returns>true if the specified character specifies a hexadecimal digit; otherwise, false.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool IsHexChar(char c)
        {
            return c is >= '0' and <= '9' or >= 'a' and <= 'f' or >= 'A' and <= 'F';
        }

        /// <summary>
        /// Tries to parse a 4-digit hexadecimal representation of a character in the specified string from the specified index.
        /// </summary>
        /// <param name="str">The string to lex and parse a 4-digit hexadecimal representation of a character from.</param>
        /// <param name="i">The index in the string to start lexing from.</param>
        /// <param name="c">The character corresponding to the 4-digit hexadecimal representation in the specified string at the specified index; otherwise, false.</param>
        /// <returns>true if a 4-digit hexadecimal representation of a character was found at the specified index; otherwise, false.</returns>
        private static bool TryParseHexChar(string str, int i, out char c)
        {
            if (!TryParseHex(str[i], out int u0) || !TryParseHex(str[i + 1], out int u1) || !TryParseHex(str[i + 2], out int u2) || !TryParseHex(str[i + 3], out int u3))
            {
                c = default;
                return false;
            }

            c = (char)((u0 << 12) + (u1 << 8) + (u2 << 4) + u3);
            return true;
        }

#if !NO_IO
        /// <summary>
        /// Tries to parse a 4-digit hexadecimal representation of a character in the specified text reader.
        /// </summary>
        /// <param name="reader">The text reader to lex and parse a 4-digit hexadecimal representation of a character from.</param>
        /// <param name="c">The character corresponding to the 4-digit hexadecimal representation in the specified string at the specified index; otherwise, false.</param>
        /// <returns>true if a 4-digit hexadecimal representation of a character was found at the specified index; otherwise, false.</returns>
        private static bool TryParseHexChar(System.IO.TextReader reader, out char c)
        {
            if (!TryParseHex((char)reader.Read(), out int u0) || !TryParseHex((char)reader.Read(), out int u1) || !TryParseHex((char)reader.Read(), out int u2) || !TryParseHex((char)reader.Read(), out int u3))
            {
                c = default;
                return false;
            }

            c = (char)((u0 << 12) + (u1 << 8) + (u2 << 4) + u3);
            return true;
        }
#endif

        /// <summary>
        /// Tries to convert the specified character matching a hexadecimal digit [0-9A-Fa-f] to an integer representation.
        /// </summary>
        /// <param name="c">The character to convert to an integer representation, if it represents a hexadecimal digit.</param>
        /// <param name="x">The integer value of the character interpreted as a hexadecimal digit.</param>
        /// <returns>true if the specified character represents a hexadecimal digit; otherwise, false.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool TryParseHex(char c, out int x)
        {
            x = 0;

            if (c is >= '0' and <= '9')
            {
                x = c - '0';
                return true;
            }

            if (c is >= 'a' and <= 'f')
            {
                x = c - 'a' + 10;
                return true;
            }

            if (c is >= 'A' and <= 'F')
            {
                x = c - 'A' + 10;
                return true;
            }

            return false;
        }

#if !NO_IO
        /// <summary>
        /// Tries to convert the specified character, represented as an Int32 value representing a Char, matching a hexadecimal digit [0-9A-Fa-f] to an integer representation.
        /// </summary>
        /// <param name="i">The character to convert to an integer representation, if it represents a hexadecimal digit.</param>
        /// <returns>true if the specified character represents a hexadecimal digit; otherwise, false.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool ToHex(ref int i)
        {
            if (i is >= '0' and <= '9')
            {
                i = (char)i - '0';
                return true;
            }
            else if (i is >= 'a' and <= 'f')
            {
                i = (char)i - 'a' + 10;
                return true;
            }
            else if (i is >= 'A' and <= 'F')
            {
                i = (char)i - 'A' + 10;
                return true;
            }

            return false;
        }
#endif

#if USE_STRINGPOOL
        /// <summary>
        /// Tries to lex a JSON Number in the specified string starting from the specified index.
        /// </summary>
        /// <param name="str">The string to lex a JSON Number literal from.</param>
        /// <param name="len">The length of the string.</param>
        /// <param name="i">The index in the string to start lexing from. This value gets updated to the first index position after the Number literal, if found.</param>
        /// <param name="length">The number of characters in the lexed JSON Number. The caller can use this number to allocate a substring containing the lexed number.</param>
        /// <returns>true if a Number literal was found; otherwise, false.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        private static bool TryLexNumber(string str, int len, int i, out int length)
        {
            length = 0;

            var b = i;

            if (i < len)
            {
                var c = str[i];

                switch (c)
                {
                    case '-':
                    case '0':
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                        {
                            if (c == '-')
                            {
                                if (++i >= len)
                                    return false;

                                c = str[i];

                                if (c < '0' || c > '9')
                                    return false;
                            }

                            if (c == '0')
                            {
                                if (++i < len)
                                    c = str[i];
                                else
                                    c = '\0';
                            }
                            else
                            {
                                do
                                {
                                    if (++i < len)
                                        c = str[i];
                                    else
                                        c = '\0';
                                } while (c >= '0' && c <= '9');
                            }

                            if (c == '.')
                            {
                                if (++i >= len)
                                    return false;

                                c = str[i];

                                if (c < '0' || c > '9')
                                    return false;

                                do
                                {
                                    if (++i < len)
                                        c = str[i];
                                    else
                                        c = '\0';
                                } while (c >= '0' && c <= '9');
                            }

                            if (c == 'e' || c == 'E')
                            {
                                if (++i >= len)
                                    return false;

                                c = str[i];

                                if (c == '+' || c == '-')
                                {
                                    if (++i >= len)
                                        return false;

                                    c = str[i];
                                }

                                if (c < '0' || c > '9')
                                    return false;

                                do
                                {
                                    if (++i < len)
                                        c = str[i];
                                    else
                                        c = '\0';
                                } while (c >= '0' && c <= '9');
                            }

                            length = i - b;
                            return true;
                        }
                }
            }

            return false;
        }
#else
        /// <summary>
        /// Tries to lex a JSON Number in the specified string starting from the specified index.
        /// </summary>
        /// <param name="str">The string to lex a JSON Number literal from.</param>
        /// <param name="len">The length of the string.</param>
        /// <param name="i">The index in the string to start lexing from. This value gets updated to the first index position after the Number literal, if found.</param>
        /// <param name="res">A string containing the lexed number.</param>
        /// <returns>true if a Number literal was found; otherwise, false.</returns>
#if USE_SPAN
        private static bool TryLexNumber(string str, int len, ref int i, out ReadOnlySpan<char> res)
#else
        private static bool TryLexNumber(string str, int len, ref int i, out string res)
#endif
        {
            res = null;

            var b = i;

            if (i < len)
            {
                var c = str[i];

                switch (c)
                {
                    case '-':
                    case '0':
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                        {
                            if (c == '-')
                            {
                                if (++i >= len)
                                    return false;

                                c = str[i];

                                if (c is < '0' or > '9')
                                    return false;
                            }

                            if (c == '0')
                            {
                                if (++i < len)
                                    c = str[i];
                                else
                                    c = '\0';
                            }
                            else
                            {
                                do
                                {
                                    if (++i < len)
                                        c = str[i];
                                    else
                                        c = '\0';
                                } while (c is >= '0' and <= '9');
                            }

                            if (c == '.')
                            {
                                if (++i >= len)
                                    return false;

                                c = str[i];

                                if (c is < '0' or > '9')
                                    return false;

                                do
                                {
                                    if (++i < len)
                                        c = str[i];
                                    else
                                        c = '\0';
                                } while (c is >= '0' and <= '9');
                            }

                            if (c is 'e' or 'E')
                            {
                                if (++i >= len)
                                    return false;

                                c = str[i];

                                if (c is '+' or '-')
                                {
                                    if (++i >= len)
                                        return false;

                                    c = str[i];
                                }

                                if (c is < '0' or > '9')
                                    return false;

                                do
                                {
                                    if (++i < len)
                                        c = str[i];
                                    else
                                        c = '\0';
                                } while (c is >= '0' and <= '9');
                            }

#if USE_SPAN
                            res = str.AsSpan(b, i - b);
#elif NET5_0 || NETSTANDARD2_1
                            res = str[b..i];
#else
                            res = str.Substring(b, i - b);
#endif
                            return true;
                        }
                }
            }

            return false;
        }
#endif

#if !NO_IO
        /// <summary>
        /// Tries to lex a JSON Number in the specified text reader.
        /// </summary>
        /// <param name="reader">The text reader to lex a JSON Number literal from.</param>
        /// <param name="res">A string containing the lexed number.</param>
        /// <returns>true if a Number literal was found; otherwise, false.</returns>
        private static bool TryLexNumber(System.IO.TextReader reader, out string res)
        {
            res = null;

            var c = reader.Peek();

            switch (c)
            {
                case '-':
                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                    {
                        using var psb = System.Text.PooledStringBuilder.New();

                        var sb = psb.StringBuilder;

                        if (c == '-')
                        {
                            sb.Append((char)reader.Read());

                            c = reader.Peek();

                            if (c is < '0' or > '9')
                                return false;
                        }

                        if (c == '0')
                        {
                            sb.Append((char)reader.Read());

                            c = reader.Peek();
                        }
                        else
                        {
                            do
                            {
                                sb.Append((char)reader.Read());
                            } while ((c = reader.Peek()) >= '0' && c <= '9');
                        }

                        if (c == '.')
                        {
                            sb.Append((char)reader.Read());

                            c = reader.Peek();

                            if (c is < '0' or > '9')
                                return false;

                            do
                            {
                                sb.Append((char)reader.Read());
                            } while ((c = reader.Peek()) >= '0' && c <= '9');
                        }

                        if (c is 'e' or 'E')
                        {
                            sb.Append((char)reader.Read());

                            c = reader.Peek();

                            if (c is '+' or '-')
                            {
                                sb.Append((char)reader.Read());

                                c = reader.Peek();
                            }

                            if (c is < '0' or > '9')
                                return false;

                            do
                            {
                                sb.Append((char)reader.Read());
                            } while ((c = reader.Peek()) >= '0' && c <= '9');
                        }

                        res = sb.ToString();
                        return true;
                    }
            }

            return false;
        }
#endif

        /// <summary>
        /// Checks whether a string has no '\' or '"' characters in it, which have special meaning when scanning a JSON string literal.
        /// </summary>
        /// <param name="value">The string to check for specialcharacters.</param>
        /// <returns>true if the string has special characters; otherwise, false.</returns>
        internal static bool StringHasNoEscapesOrTerminator(string value)
        {
            //
            // NB: When performing an optimized StartsWith from a compiled SyntaxTrie evaluator expression, we need to check whether we can safely make
            //     a call to String.CompareOrdinal. If the string to check for contains a '"' character, it could cause us to read beyond the end of the
            //     string literal being matched, e.g.
            //
            //         json_raw  = `..."bar": 42...`       json_csharp  = "...\"bar\": 42..."
            //         check_raw =     `bar": 4`           check_csharp =      "bar\": 4"
            //         match     =      [.....]
            //         remainder =             2...
            //
            //     Also, if the string to check for contains a '\' character, it could cause us to read part of a JSON escape sequence, leaving us in the
            //     middle of an escape sequence, e.g.
            //
            //         json_raw  = `..."bar\t": 42...`     json_csharp  = "...\"bar\\t\": 42..."
            //         check_raw =     `bar\`              check_csharp =      "bar\\"
            //         match     =      [..]
            //         remainder =          t": 42
            //

            return value.IndexOfAny(new char[] { '\\', '"' }) < 0;
        }

        /// <summary>
        /// Checks whether the specified JSON string at the specified index starts with the specified value, without taking JSON escape sequences into account.
        /// </summary>
        /// <param name="str">The JSON string to match in. This string can use JSON encodings.</param>
        /// <param name="i">The index in the string to start matching from. This value gets updated to the first index position after the match, if found.</param>
        /// <param name="value">The string to match. This string is not supposed to be JSON encoded and should not contain a '"' or '\' character.</param>
        /// <returns>true if the specified string starts with the specified value at the specified index; otherwise, false.</returns>
        internal static bool StartsWithFast(string str, ref int i, string value)
        {
#if NET5_0
            Debug.Assert(value.IndexOf('\\', System.StringComparison.Ordinal) < 0 && value.IndexOf('"', System.StringComparison.Ordinal) < 0);
#else
            Debug.Assert(value.IndexOf('\\') < 0 && value.IndexOf('"') < 0);
#endif

            if (string.CompareOrdinal(str, i, value, 0, value.Length) == 0)
            {
                i += value.Length;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Checks whether the specified JSON string at the specified index starts with the specified value, taking JSON escape sequences into account.
        /// </summary>
        /// <param name="str">The JSON string to match in. This string can use JSON encodings.</param>
        /// <param name="len">The length of the string.</param>
        /// <param name="b">The start index of the String literal being lexed; used for error reporting.</param>
        /// <param name="i">The index in the string to start matching from. This value gets updated to the first index position after the match, if found.</param>
        /// <param name="value">The string to match. This string is not supposed to be JSON encoded.</param>
        /// <returns>true if the specified string starts with the specified value at the specified index; otherwise, false.</returns>
        internal static bool StartsWith(string str, int len, int b, ref int i, string value)
        {
            var jsonIndex = i;
            var valueIndex = 0;
            var valueLength = value.Length;

            while (jsonIndex < len && valueIndex < valueLength)
            {
                var c = str[jsonIndex];

                switch (c)
                {
                    case '\\':
                        if (++jsonIndex >= len)
                            throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected a JSON String at position '{0}'. Unexpected string termination found after '\\' escape character.", b), jsonIndex, ParseError.PrematureEndOfInput);

                        c = str[jsonIndex];
                        jsonIndex++;

                        switch (c)
                        {
                            case '"':
                            case '\\':
                            case '/':
                                break;
                            case 'b':
                                c = '\b';
                                break;
                            case 'f':
                                c = '\f';
                                break;
                            case 'n':
                                c = '\n';
                                break;
                            case 'r':
                                c = '\r';
                                break;
                            case 't':
                                c = '\t';
                                break;
                            case 'u':
                                if (jsonIndex + 3 >= len)
                                    throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected a JSON String at position '{0}'. Unexpected string termination found in '\\u' escape sequence.", b), jsonIndex, ParseError.PrematureEndOfInput);

                                int u0, u1, u2, u3;

                                if (!TryParseHex(str[jsonIndex], out u0) || !TryParseHex(str[jsonIndex + 1], out u1) || !TryParseHex(str[jsonIndex + 2], out u2) || !TryParseHex(str[jsonIndex + 3], out u3))
                                    throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected a JSON String at position '{0}'. Unexpected character found in '\\u' escape sequence.", b), jsonIndex, ParseError.UnexpectedToken);

                                c = (char)((u0 << 12) + (u1 << 8) + (u2 << 4) + u3);
                                jsonIndex += 4;
                                break;
                            default:
                                throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected a JSON String at position '{0}'. Unexpected character '{1}' found after '\\' escape character.", b, c), jsonIndex, ParseError.UnexpectedToken);
                        }
                        break;
                    case '"':
                        return false;
                    default:
                        if (char.IsControl(c))
                        {
                            throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected a JSON String at position '{0}'. Unexpected control character '{1}' found.", b, c), jsonIndex, ParseError.UnexpectedToken);
                        }

                        jsonIndex++;
                        break;
                }

                if (value[valueIndex] != c)
                    return false;

                valueIndex++;
            }

            if (valueIndex == valueLength)
            {
                i = jsonIndex;
                return true;
            }

            return false;
        }

#if !NO_IO
        /// <summary>
        /// Checks whether the JSON payload in the specified text reader starts with the specified value, taking JSON escape sequences into account.
        /// </summary>
        /// <param name="reader">The text reader containing the JSON string to match in. This string can use JSON encodings.</param>
        /// <param name="value">The string to match. This string is not supposed to be JSON encoded.</param>
        /// <returns>true if the specified string starts with the specified value; otherwise, false.</returns>
        internal static bool StartsWithReader(System.IO.TextReader reader, string value)
        {
            //
            // NB: The reader-based version of StartsWith differs from the string-based version in its
            //     consumption of characters. Even if no match is found, the reader will consume input
            //     because we can only peek one character ahead. This is not a problem for the current
            //     use sites of StartsWithReader in the compiled SyntaxTrie expression where it's only
            //     called when there's only one child to a node (i.e. the "if" statement has no "else"
            //     case to check other prefixes).
            //

            var valueIndex = 0;
            var valueLength = value.Length;

            while (valueIndex < valueLength)
            {
                var c = reader.Peek();

                switch (c)
                {
                    case '\\':
                        reader.Read();
                        c = reader.Read();

                        switch (c)
                        {
                            case '"':
                            case '\\':
                            case '/':
                                break;
                            case 'b':
                                c = '\b';
                                break;
                            case 'f':
                                c = '\f';
                                break;
                            case 'n':
                                c = '\n';
                                break;
                            case 'r':
                                c = '\r';
                                break;
                            case 't':
                                c = '\t';
                                break;
                            case 'u':
                                int u0 = reader.Read();
                                int u1 = reader.Read();
                                int u2 = reader.Read();
                                int u3 = reader.Read();

                                if (!ToHex(ref u0) || !ToHex(ref u1) || !ToHex(ref u2) || !ToHex(ref u3))
                                    throw new ParseException("Expected a JSON String. Unexpected character found in '\\u' escape sequence.", -1, ParseError.UnexpectedToken);

                                c = (char)((u0 << 12) + (u1 << 8) + (u2 << 4) + u3);
                                break;
                            default:
                                throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected a JSON String. Unexpected character '{0}' found after '\\' escape character.", (char)c), -1, ParseError.UnexpectedToken);
                        }
                        break;
                    case '"':
                        return false;
                    default:
                        if (char.IsControl((char)c))
                        {
                            throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected a JSON String. Unexpected control character '{0}' found.", (char)c), -1, ParseError.UnexpectedToken);
                        }

                        reader.Read();
                        break;
                }

                if (value[valueIndex] != c)
                    return false;

                valueIndex++;
            }

            return valueIndex == valueLength;
        }
#endif

        /// <summary>
        /// Gets the next string character from the specified JSON string at the specified index, taking JSON escape sequences into account.
        /// </summary>
        /// <param name="str">The JSON string to retrieve the next string character from. This string can use JSON encodings.</param>
        /// <param name="len">The length of the string.</param>
        /// <param name="i">The index in the string to start reading a character from. This value gets updated to the first index position after the character read.</param>
        /// <param name="res">The next decoded character read from the JSON string, if found.</param>
        /// <returns></returns>
        internal static bool TryGetNextChar(string str, int len, ref int i, out char res)
        {
            //
            // CONSIDER: Feed in the begin index rather than computing it here.
            //

            var b = i;

            var c = str[i];

            switch (c)
            {
                case '\\':
                    if (++i >= len)
                        throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected Char at position '{0}'. Unexpected string termination found after '\\' escape character.", b), i, ParseError.PrematureEndOfInput);

                    c = str[i];
                    i++;

                    switch (c)
                    {
                        case '"':
                        case '\\':
                        case '/':
                            res = c;
                            return true;
                        case 'b':
                            res = '\b';
                            return true;
                        case 'f':
                            res = '\f';
                            return true;
                        case 'n':
                            res = '\n';
                            return true;
                        case 'r':
                            res = '\r';
                            return true;
                        case 't':
                            res = '\t';
                            return true;
                        case 'u':
                            if (i + 3 >= len)
                                throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected Char at position '{0}'. Unexpected string termination found after '\\u' escape character.", b), i, ParseError.PrematureEndOfInput);

                            int u0, u1, u2, u3;

                            if (!TryParseHex(str[i], out u0) || !TryParseHex(str[i + 1], out u1) || !TryParseHex(str[i + 2], out u2) || !TryParseHex(str[i + 3], out u3))
                                throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected Char at position '{0}'. Expected hexadecimal character escape sequence after '\\u' escape character.", b), i, ParseError.UnexpectedToken);

                            //
                            // NB: We allow a surrogate character to be returned.
                            //

                            i += 4;
                            res = (char)((u0 << 12) + (u1 << 8) + (u2 << 4) + u3);
                            return true;
                        default:
                            throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected Char at position '{0}'. Unexpected character '{1}' found after '\\' escape character.", b, c), i, ParseError.UnexpectedToken);
                    }
                case '\"':
                    res = '\0';
                    return false;
                default:
                    if (char.IsControl(c))
                        throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected Char at position '{0}'. Unexpected control character '{1}' found in string literal.", b, c), i, ParseError.UnexpectedToken);

                    i++;
                    res = c;
                    return true;
            }
        }

#if !NO_IO
        /// <summary>
        /// Gets the next string character from the JSON payload in the specified text reader, taking JSON escape sequences into account.
        /// </summary>
        /// <param name="reader">The text reader to retrieve the next string character from. This string can use JSON encodings.</param>
        /// <param name="res">The next decoded character read from the JSON string, if found.</param>
        /// <returns></returns>
        internal static bool TryGetNextCharReader(System.IO.TextReader reader, out int res)
        {
            //
            // CONSIDER: Feed in the begin index rather than computing it here.
            //

            var c = reader.Peek();

            switch (c)
            {
                case '\\':
                    reader.Read();

                    c = reader.Read();

                    if (c < 0)
                        throw new ParseException("Expected Char. Unexpected string termination found after '\\' escape character.", -1, ParseError.PrematureEndOfInput);

                    switch (c)
                    {
                        case '"':
                        case '\\':
                        case '/':
                            res = c;
                            return true;
                        case 'b':
                            res = '\b';
                            return true;
                        case 'f':
                            res = '\f';
                            return true;
                        case 'n':
                            res = '\n';
                            return true;
                        case 'r':
                            res = '\r';
                            return true;
                        case 't':
                            res = '\t';
                            return true;
                        case 'u':
                            var u0 = reader.Read();
                            var u1 = reader.Read();
                            var u2 = reader.Read();
                            var u3 = reader.Read();

                            if (!ToHex(ref u0) || !ToHex(ref u1) || !ToHex(ref u2) || !ToHex(ref u3))
                                throw new ParseException("Expected Char. Expected hexadecimal character escape sequence after '\\u' escape character.", -1, ParseError.UnexpectedToken);

                            //
                            // NB: We allow a surrogate character to be returned.
                            //

                            res = (u0 << 12) + (u1 << 8) + (u2 << 4) + u3;
                            return true;
                        default:
                            throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected Char. Unexpected character '{0}' found after '\\' escape character.", (char)c), -1, ParseError.UnexpectedToken);
                    }
                case '\"':
                    res = '\0';
                    return false;
                default:
                    if (char.IsControl((char)c))
                        throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected Char. Unexpected control character '{0}' found in string literal.", (char)c), -1, ParseError.UnexpectedToken);

                    res = reader.Read();
                    return true;
            }
        }
#endif
    }
}
