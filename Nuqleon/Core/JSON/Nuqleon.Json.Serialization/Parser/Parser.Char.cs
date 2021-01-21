// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 04/05/2016 - Created fast JSON deserializer functionality.
//   BD - 05/08/2016 - Added support for deserialization from text readers.
//

using Nuqleon.Json.Parser;
using System.Globalization;

namespace Nuqleon.Json.Serialization
{
    internal partial class Parser
    {
        /// <summary>
        /// Lexes and parses a JSON String as a System.Char in the specified string starting from the specified index.
        /// </summary>
        /// <param name="str">The string to lex and parse a Char from.</param>
        /// <param name="len">The length of the string.</param>
        /// <param name="i">The index in the string to start lexing from. This value gets updated to the first index position after the Char, if found.</param>
        /// <param name="_">The parser context.</param>
        /// <returns>A System.String value, if found; otherwise, a ParseException is thrown.</returns>
        /// <remarks>
        /// This method does not consume whitespace; the character at the specified start index in the string is expected to be the start of a valid JSON String.
        /// Valid inputs for Char values are considered to be single-character JSON Strings. Note that JSON does not support a ' token for the start of a character.
        /// </remarks>
        internal static char ParseChar(string str, int len, ref int i, ParserContext _ = null)
        {
            var b = i;

            if (i < len)
            {
                var c = str[i];

                if (c != '"')
                    throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected Char at position '{0}'. Unexpected character '{1}' found when '\"' was expected.", b, c), i, ParseError.UnexpectedToken);

                i++;

                if (i < len)
                {
                    c = str[i];

                    char res;

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
                                    break;
                                case 'b':
                                    res = '\b';
                                    break;
                                case 'f':
                                    res = '\f';
                                    break;
                                case 'n':
                                    res = '\n';
                                    break;
                                case 'r':
                                    res = '\r';
                                    break;
                                case 't':
                                    res = '\t';
                                    break;
                                case 'u':
                                    if (i + 3 >= len)
                                        throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected Char at position '{0}'. Unexpected string termination found after '\\u' escape character.", b), i, ParseError.PrematureEndOfInput);

                                    int u0, u1, u2, u3;

                                    if (!TryParseHex(str[i], out u0) || !TryParseHex(str[i + 1], out u1) || !TryParseHex(str[i + 2], out u2) || !TryParseHex(str[i + 3], out u3))
                                        throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected Char at position '{0}'. Expected hexadecimal character escape sequence after '\\u' escape character.", b), i, ParseError.UnexpectedToken);

                                    //
                                    // NB: We allow a surrogate character to be returned.
                                    //
                                    // CONSIDER: Introduce a configuration option in the parser context to disallow surrogates?
                                    //

                                    res = (char)((u0 << 12) + (u1 << 8) + (u2 << 4) + u3);
                                    i += 4;
                                    break;
                                default:
                                    throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected Char at position '{0}'. Unexpected character '{1}' found after '\\' escape character.", b, c), i, ParseError.UnexpectedToken);
                            }
                            break;
                        case '"':
                            //
                            // CONSIDER: Introduce a configuration option in the parser context to allow empty strings? Would it be null and/or some configurable character a la '\0'?
                            //

                            throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected Char at position '{0}'. Unexpected empty string literal found.", b), i, ParseError.UnexpectedToken);
                        default:
                            if (char.IsControl(c))
                                throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected Char at position '{0}'. Unexpected control character '{1}' found in string literal.", b, c), i, ParseError.UnexpectedToken);

                            res = c;
                            i++;
                            break;
                    }

                    if (i < len)
                    {
                        c = str[i];

                        if (c == '\"')
                        {
                            i++;
                            return res;
                        }
                    }
                }
            }

            throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected Char at position '{0}'. Unexpected string termination found.", b), i, ParseError.PrematureEndOfInput);
        }

#if !NO_IO
        /// <summary>
        /// Lexes and parses a JSON String as a System.Char in the specified text reader.
        /// </summary>
        /// <param name="reader">The text reader to lex and parse a Char from.</param>
        /// <param name="_">The parser context.</param>
        /// <returns>A System.String value, if found; otherwise, a ParseException is thrown.</returns>
        /// <remarks>
        /// This method does not consume whitespace; the character at the specified start index in the string is expected to be the start of a valid JSON String.
        /// Valid inputs for Char values are considered to be single-character JSON Strings. Note that JSON does not support a ' token for the start of a character.
        /// </remarks>
        internal static char ParseChar(System.IO.TextReader reader, ParserContext _ = null)
        {
            var c = reader.Read();

            if (c != '"')
                throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected Char. Unexpected character '{0}' found when '\"' was expected.", c), -1, ParseError.UnexpectedToken);

            c = reader.Read();

            char res;

            switch (c)
            {
                case '\\':
                    c = reader.Read();

                    switch (c)
                    {
                        case '"':
                        case '\\':
                        case '/':
                            res = (char)c;
                            break;
                        case 'b':
                            res = '\b';
                            break;
                        case 'f':
                            res = '\f';
                            break;
                        case 'n':
                            res = '\n';
                            break;
                        case 'r':
                            res = '\r';
                            break;
                        case 't':
                            res = '\t';
                            break;
                        case 'u':
                            var u0 = reader.Read();
                            var u1 = reader.Read();
                            var u2 = reader.Read();
                            var u3 = reader.Read();

                            if (!ToHex(ref u0) || !ToHex(ref u1) || !ToHex(ref u2) || !ToHex(ref u3))
                                throw new ParseException("Expected Char. Expected hexadecimal character escape sequence after '\\u' escape character.", -1, ParseError.UnexpectedToken);

                            res = (char)((u0 << 12) + (u1 << 8) + (u2 << 4) + u3);
                            break;
                        default:
                            throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected Char. Unexpected character '{0}' found after '\\' escape character.", c), -1, ParseError.UnexpectedToken);
                    }
                    break;
                case '"':
                    //
                    // CONSIDER: Introduce a configuration option in the parser context to allow empty strings? Would it be null and/or some configurable character a la '\0'?
                    //

                    throw new ParseException("Expected Char. Unexpected empty string literal found.", -1, ParseError.UnexpectedToken);
                default:
                    if (char.IsControl((char)c))
                        throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected Char. Unexpected control character '{0}' found in string literal.", c), -1, ParseError.UnexpectedToken);

                    res = (char)c;
                    break;
            }

            c = reader.Read();

            if (c == '\"')
            {
                return res;
            }

            throw new ParseException("Expected Char. Unexpected string termination found.", -1, ParseError.PrematureEndOfInput);
        }
#endif
    }
}
