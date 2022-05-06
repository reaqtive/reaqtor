// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 04/05/2016 - Created fast JSON deserializer functionality.
//   BD - 05/08/2016 - Added support for deserialization from text readers.
//

using System.Globalization;
using System.Text;

using Nuqleon.Json.Parser;

namespace Nuqleon.Json.Serialization
{
    internal partial class Parser
    {
        /// <summary>
        /// Lexes and parses a JSON String as a System.String in the specified string starting from the specified index.
        /// </summary>
        /// <param name="str">The string to lex and parse a String from.</param>
        /// <param name="len">The length of the string.</param>
        /// <param name="i">The index in the string to start lexing from. This value gets updated to the first index position after the String, if found.</param>
        /// <param name="_">The parser context.</param>
        /// <returns>A System.String value, if found; otherwise, a ParseException is thrown.</returns>
        /// <remarks>
        /// This method does not consume whitespace; the character at the specified start index in the string is expected to be the start of a valid JSON String.
        /// </remarks>
        internal static string ParseString(string str, int len, ref int i, ParserContext _ = null)
        {
            var b = i;

#if ALLOW_UNSAFE
            if (i + 4 <= len)
            {
                unsafe
                {
                    const long NULL = ((long)'n' << 0) | ((long)'u' << 16) | ((long)'l' << 32) | ((long)'l' << 48);

                    fixed (char* s = str)
                    {
                        var c = s + i;

                        if (*((long*)c) == NULL)
                        {
                            i += 4;
                            return null;
                        }
                    }
                }
            }

            if (i < len)
            {
                return ParseStringNonNull(str, len, ref i);
            }
#else
            if (i < len)
            {
                if (str[i] == 'n')
                {
                    if (TryParseNullSkipN(str, len, ref i))
                    {
                        return null;
                    }
                }
                else
                {
                    return ParseStringNonNull(str, len, ref i);
                }
            }
#endif
            throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected String or null at position '{0}'.", b), b, ParseError.InvalidToken);
        }

        private static string ParseStringNonNull(string str, int len, ref int i)
        {
            var b = i;

            var c = str[i];

            if (c != '"')
                throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected String at position '{0}'. Unexpected character '{1}' found when '\"' was expected.", b, c), i, ParseError.UnexpectedToken);

            i++;

            var start = i;

            while (i < len)
            {
                c = str[i];

                switch (c)
                {
                    case '\\':
                        using (var psb = PooledStringBuilder.New())
                        {
                            var sb = psb.StringBuilder;

                            sb.Append(str, start, i - start);

                            return ParseStringNonNull(str, len, ref i, b, sb);
                        }
                    case '"':
                        var end = i;
                        i++;
#if NET6_0 || NETSTANDARD2_1
                        return str[start..end];
#else
                        return str.Substring(start, end - start);
#endif
                    default:
                        if (char.IsControl(c))
                            throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected String at position '{0}'. Unexpected control character '{1}' found in string literal.", b, c), i, ParseError.UnexpectedToken);

                        i++;
                        break;
                }
            }

            throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected String at position '{0}'.", b), b, ParseError.PrematureEndOfInput);
        }

        private static string ParseStringNonNull(string str, int len, ref int i, int b, StringBuilder sb)
        {
            while (i < len)
            {
                var c = str[i];

                switch (c)
                {
                    case '\\':
                        if (++i >= len)
                            throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected String at position '{0}'. Unexpected string termination found after '\\' escape character.", b), i, ParseError.PrematureEndOfInput);

                        c = str[i];
                        i++;

                        switch (c)
                        {
                            case '"':
                            case '\\':
                            case '/':
                                sb.Append(c);
                                break;
                            case 'b':
                                sb.Append('\b');
                                break;
                            case 'f':
                                sb.Append('\f');
                                break;
                            case 'n':
                                sb.Append('\n');
                                break;
                            case 'r':
                                sb.Append('\r');
                                break;
                            case 't':
                                sb.Append('\t');
                                break;
                            case 'u':
                                if (i + 3 >= len)
                                    throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected String at position '{0}'. Unexpected string termination found after '\\u' escape character.", b), i, ParseError.PrematureEndOfInput);

                                if (!TryParseHexChar(str, i, out var val))
                                    throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected String at position '{0}'. Expected hexadecimal character escape sequence after '\\u' escape character.", b), i, ParseError.UnexpectedToken);

                                i += 4;

                                if (char.IsSurrogate(val))
                                {
                                    if (!char.IsHighSurrogate(val))
                                        throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected String at position '{0}'. Unexpected character '{1}' representing a low surrogate that's not preceded by a high surrogate.", b, val), i, ParseError.UnexpectedToken);

                                    if (i + 6 >= len)
                                        throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected String at position '{0}'. Unexpected string termination found after '\\u' escape character representing a high surrogate.", b), i, ParseError.PrematureEndOfInput);

                                    if (str[i] != '\\' || str[i + 1] != 'u')
                                        throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected String at position '{0}'. Expected '\\u' character escape sequence for the low surrogate after a '\\u' escape sequence representing a high surrogate.", b), i, ParseError.UnexpectedToken);

                                    i += 2;

                                    if (!TryParseHexChar(str, i, out char low))
                                        throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected String at position '{0}'. Expected hexadecimal character escape sequence for the low surrogate after a '\\u' escape sequence representing a high surrogate.", b), i, ParseError.UnexpectedToken);

                                    if (!char.IsLowSurrogate(low))
                                        throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected String at position '{0}'. Unexpected character '{1}' representing a high surrogate following a high surrogate.", b, val), i, ParseError.UnexpectedToken);

                                    i += 4;

                                    sb.Append(val);
                                    sb.Append(low);
                                }
                                else
                                {
                                    sb.Append(val);
                                }
                                break;
                            default:
                                throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected String at position '{0}'. Unexpected character '{1}' found after '\\' escape character.", b, c), i, ParseError.UnexpectedToken);
                        }
                        break;
                    case '"':
                        i++;
                        return sb.ToString();
                    default:
                        if (char.IsControl(c))
                            throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected String at position '{0}'. Unexpected control character '{1}' found in string literal.", b, c), i, ParseError.UnexpectedToken);

                        sb.Append(c);
                        i++;
                        break;
                }
            }

            throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected String at position '{0}'.", b), b, ParseError.PrematureEndOfInput);
        }

#if !NO_IO
        /// <summary>
        /// Lexes and parses a JSON String as a System.String in the specified text reader.
        /// </summary>
        /// <param name="reader">The text reader to lex and parse a String from.</param>
        /// <param name="context">The parser context.</param>
        /// <returns>A System.String value, if found; otherwise, a ParseException is thrown.</returns>
        /// <remarks>
        /// This method does not consume whitespace; the character at the specified start index in the string is expected to be the start of a valid JSON String.
        /// </remarks>
        internal static string ParseString(System.IO.TextReader reader, ParserContext context)
        {
            var c = reader.Peek();

            if (c == 'n')
            {
                if (!TryParseNullSkipN(reader, context))
                    throw new ParseException("Expected String or null.", -1, ParseError.InvalidToken);

                return null;
            }

            return ParseStringNonNull(reader);
        }

        internal static string ParseStringNonNull(System.IO.TextReader reader)
        {
            var c = reader.Read();

            if (c != '"')
                throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected String. Unexpected character '{0}' found when '\"' was expected.", c), -1, ParseError.UnexpectedToken);

            c = reader.Read();

            if (c == '\"')
            {
                return "";
            }

            using var psb = PooledStringBuilder.New();

            var sb = psb.StringBuilder;

            while (c is >= 0 and not '\"')
            {
                switch (c)
                {
                    case '\\':
                        c = reader.Read();

                        switch (c)
                        {
                            case '"':
                            case '\\':
                            case '/':
                                sb.Append((char)c);
                                break;
                            case 'b':
                                sb.Append('\b');
                                break;
                            case 'f':
                                sb.Append('\f');
                                break;
                            case 'n':
                                sb.Append('\n');
                                break;
                            case 'r':
                                sb.Append('\r');
                                break;
                            case 't':
                                sb.Append('\t');
                                break;
                            case 'u':
                                var val = default(char);
                                if (!TryParseHexChar(reader, out val))
                                    throw new ParseException("Expected String. Expected hexadecimal character escape sequence after '\\u' escape character.", -1, ParseError.UnexpectedToken);

                                if (char.IsSurrogate(val))
                                {
                                    if (!char.IsHighSurrogate(val))
                                        throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected String. Unexpected character '{0}' representing a low surrogate that's not preceded by a high surrogate.", val), -1, ParseError.UnexpectedToken);

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable IDE0078 // Use pattern matching. (`reader.Read()` has a side-effect and should not get removed; see https://github.com/dotnet/roslyn-analyzers/issues/4690.)
                                    if (reader.Read() != '\\' || reader.Read() != 'u')
                                        throw new ParseException("Expected String. Expected '\\u' character escape sequence for the low surrogate after a '\\u' escape sequence representing a high surrogate.", -1, ParseError.UnexpectedToken);
#pragma warning restore IDE0078
#pragma warning disable IDE0079

                                    if (!TryParseHexChar(reader, out char low))
                                        throw new ParseException("Expected String. Expected hexadecimal character escape sequence for the low surrogate after a '\\u' escape sequence representing a high surrogate.", -1, ParseError.UnexpectedToken);

                                    if (!char.IsLowSurrogate(low))
                                        throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected String. Unexpected character '{0}' representing a high surrogate following a high surrogate.", val), -1, ParseError.UnexpectedToken);

                                    sb.Append(val);
                                    sb.Append(low);
                                }
                                else
                                {
                                    sb.Append(val);
                                }
                                break;
                            default:
                                throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected String. Unexpected character '{0}' found after '\\' escape character.", c), -1, ParseError.UnexpectedToken);
                        }
                        break;
                    default:
                        if (char.IsControl((char)c))
                            throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected String. Unexpected control character '{0}' found in string literal.", c), -1, ParseError.UnexpectedToken);

                        sb.Append((char)c);
                        break;
                }

                c = reader.Read();
            }

            if (c == '\"')
            {
                return sb.ToString();
            }

            throw new ParseException("Expected String.", -1, ParseError.PrematureEndOfInput);
        }
#endif
    }
}
