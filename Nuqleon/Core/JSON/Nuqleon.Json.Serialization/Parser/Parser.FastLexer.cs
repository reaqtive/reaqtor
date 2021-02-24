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

using Nuqleon.Json.Parser;

namespace Nuqleon.Json.Serialization
{
    internal partial class Parser
    {
        /// <summary>
        /// Skips one JSON value from the specified string starting at the specified index.
        /// </summary>
        /// <param name="str">The string to lex one JSON value from.</param>
        /// <param name="len">The length of the string.</param>
        /// <param name="i">The index in the string to start lexing from. This value gets updated to the first index position after the JSON value, if found.</param>
        /// <remarks>
        /// JSON values include the Null literal, Boolean literals, Number literals, String literals, JSON Arrays, and JSON Objects.
        /// An exception of type <see cref="ParseException"/> is thrown if no JSON value is found at the specified index in the string.
        /// This method performs no validation of the input other than the lexical grammar defined by JSON.
        /// This method consumes leading and trailing whitespace.
        /// </remarks>
        internal static void SkipOne(string str, int len, ref int i)
        {
            SkipWhiteSpace(str, len, ref i);

            try
            {
                var b = i;

                SkipCore(str, len, b, ref i);

                if (i == b)
                    throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected a JSON value at position '{0}'.", b), i, ParseError.UnexpectedToken);
            }
            finally
            {
                SkipWhiteSpace(str, len, ref i);
            }
        }

#if !NO_IO
        /// <summary>
        /// Skips one JSON value from the specified text reader.
        /// </summary>
        /// <param name="reader">The text reader to lex one JSON value from.</param>
        /// <remarks>
        /// JSON values include the Null literal, Boolean literals, Number literals, String literals, JSON Arrays, and JSON Objects.
        /// An exception of type <see cref="ParseException"/> is thrown if no JSON value is found at the specified index in the string.
        /// This method performs no validation of the input other than the lexical grammar defined by JSON.
        /// This method consumes leading and trailing whitespace.
        /// </remarks>
        internal static void SkipOne(System.IO.TextReader reader)
        {
            SkipWhiteSpace(reader);

            try
            {
                SkipCore(reader);
            }
            finally
            {
                SkipWhiteSpace(reader);
            }
        }
#endif

        /// <summary>
        /// Skips one JSON value from the specified string starting at the specified index.
        /// </summary>
        /// <param name="str">The string to lex one JSON value from.</param>
        /// <param name="len">The length of the string.</param>
        /// <param name="b">The start index of the JSON value lexed; used for error reporting.</param>
        /// <param name="i">The index in the string to start lexing from. This value gets updated to the first index position after the JSON value, if found.</param>
        /// <remarks>
        /// This method does not consume leading and trailing whitespace.
        /// </remarks>
        private static void SkipCore(string str, int len, int b, ref int i)
        {
            if (i >= len)
                throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected a JSON value at position '{0}'. Unexpected string termination found at start of value.", b), i, ParseError.PrematureEndOfInput);

            var c = str[i];

            switch (c)
            {
                case '{':
                    // object
                    {
                        if (++i >= len)
                            throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected a JSON Object at position '{0}'. Unexpected string termination found after '{{' token denoting that start of a JSON Object.", b), i, ParseError.PrematureEndOfInput);

                        SkipWhiteSpace(str, len, ref i);

                        c = i < len ? str[i] : '\0';

                        if (c == '}')
                        {
                            i++;
                        }
                        else
                        {
                            while (true)
                            {
                                if (i == len)
                                    throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected a JSON Object at position '{0}'. Unexpected string termination found when a JSON String denoting a key was expected.", b), i, ParseError.PrematureEndOfInput);

                                c = str[i];

                                if (c != '\"')
                                    throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected a JSON Object at position '{0}'. Unexpected character '{1}' found when a string starting with '\"' was expected.", b, c), i, ParseError.UnexpectedToken);

                                SkipString(str, len, i++, ref i);

                                SkipWhiteSpace(str, len, ref i);

                                c = i < len ? str[i] : '\0';

                                if (c != ':')
                                    throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected a JSON Object at position '{0}'. Unexpected character '{1}' found when a ':' was expected.", b, c), i, ParseError.UnexpectedToken);

                                i++;

                                SkipOne(str, len, ref i);

                                if (i >= len)
                                    throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected a JSON Object at position '{0}'. Unexpected string termination found when a ',' or '}}' was expected.", b), i, ParseError.PrematureEndOfInput);

                                c = str[i];

                                if (c == ',')
                                {
                                    i++;

                                    SkipWhiteSpace(str, len, ref i);
                                }
                                else
                                {
                                    break;
                                }
                            }

                            if (c != '}')
                                throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected a JSON Object at position '{0}'. Unexpected character '{1}' found when a '}}' was expected.", b, c), i, ParseError.UnexpectedToken);

                            i++;
                        }
                    }
                    break;
                case '[':
                    // array
                    {
                        if (++i >= len)
                            throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected a JSON Object at position '{0}'. Unexpected string termination found after '[' token denoting that start of a JSON Array.", b), i, ParseError.PrematureEndOfInput);

                        SkipWhiteSpace(str, len, ref i);

                        c = i < len ? str[i] : '\0';

                        if (c == ']')
                        {
                            i++;
                        }
                        else
                        {
                            while (true)
                            {
                                SkipOne(str, len, ref i);

                                if (i >= len)
                                    throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected a JSON Object at position '{0}'. Unexpected string termination found when an array element JSON value was expected.", b), i, ParseError.PrematureEndOfInput);

                                c = str[i];

                                if (c == ',')
                                {
                                    i++;
                                }
                                else
                                {
                                    break;
                                }
                            }

                            if (c != ']')
                                throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected a JSON Object at position '{0}'. Unexpected character '{1}' found when a ']' was expected.", b, c), i, ParseError.UnexpectedToken);

                            i++;
                        }
                    }
                    break;
                case 'n':
                    // null
                    {
                        if (i + 3 >= len)
                            throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected a JSON Null at position '{0}'. Unexpected string termination found.", b), i, ParseError.PrematureEndOfInput);
                        if (str[i + 1] != 'u' || str[i + 2] != 'l' || str[i + 3] != 'l')
                            throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected a JSON Null at position '{0}'. Unexpected character encountered while lexing 'null'.", b), i, ParseError.UnexpectedToken);
                        i += 4;
                    }
                    break;
                case 't':
                    //true
                    {
                        if (i + 3 >= len)
                            throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected a JSON Boolean at position '{0}'. Unexpected string termination found.", b), i, ParseError.PrematureEndOfInput);
                        if (str[i + 1] != 'r' || str[i + 2] != 'u' || str[i + 3] != 'e')
                            throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected a JSON Boolean at position '{0}'. Unexpected character encountered while lexing 'true'.", b), i, ParseError.UnexpectedToken);
                        i += 4;
                    }
                    break;
                case 'f':
                    // false
                    {
                        if (i + 4 >= len)
                            throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected a JSON Boolean at position '{0}'. Unexpected string termination found.", b), i, ParseError.PrematureEndOfInput);
                        if (str[i + 1] != 'a' || str[i + 2] != 'l' || str[i + 3] != 's' || str[i + 4] != 'e')
                            throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected a JSON Boolean at position '{0}'. Unexpected character encountered while lexing 'false'.", b), i, ParseError.UnexpectedToken);
                        i += 5;
                    }
                    break;
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
                        // number
                        if (c == '-')
                        {
                            if (++i >= len)
                                throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected a JSON Number at position '{0}'. Unexpected string termination found after '-' sign.", b), i, ParseError.PrematureEndOfInput);

                            c = str[i];
                        }

                        if (c == '0')
                        {
                            if (++i < len)
                                c = str[i];
                            else
                                c = '\0';
                        }
                        else if (c is >= '1' and <= '9')
                        {
                            do
                            {
                                if (++i < len)
                                    c = str[i];
                                else
                                    c = '\0';
                            } while (c is >= '0' and <= '9');
                        }
                        else
                        {
                            throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected a JSON Number at position '{0}'. Unexpected character '{1}' found after '-' sign.", b, c), i, ParseError.UnexpectedToken);
                        }

                        if (c == '.')
                        {
                            if (++i >= len)
                                throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected a JSON Number at position '{0}'. Unexpected string termination found after '.' decimal point.", b), i, ParseError.PrematureEndOfInput);

                            c = str[i];

                            if (c is < '0' or > '9')
                                throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected a JSON Number at position '{0}'. Unexpected character '{1}' found when a decimal digit was expected after the decimal point.", b, c), i, ParseError.UnexpectedToken);

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
                                throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected a JSON Number at position '{0}'. Unexpected string termination found after '{1}' exponent.", b, c), i, ParseError.PrematureEndOfInput);

                            c = str[i];

                            if (c is '+' or '-')
                            {
                                if (++i >= len)
                                    throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected a JSON Number at position '{0}'. Unexpected string termination found after '{1}' exponent sign.", b, c), i, ParseError.PrematureEndOfInput);

                                c = str[i];
                            }

                            if (c is < '0' or > '9')
                                throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected a JSON Number at position '{0}'. Unexpected character '{1}' found when a decimal digit was expected for the exponent.", b, c), i, ParseError.UnexpectedToken);

                            do
                            {
                                if (++i < len)
                                    c = str[i];
                                else
                                    c = '\0';
                            } while (c is >= '0' and <= '9');
                        }
                    }
                    break;
                case '"':
                    i++;
                    SkipString(str, len, b, ref i);
                    break;
            }
        }

#if !NO_IO
        /// <summary>
        /// Skips one JSON value from the specified text reader.
        /// </summary>
        /// <param name="reader">The text reader to lex one JSON value from.</param>
        /// <remarks>
        /// This method does not consume leading and trailing whitespace.
        /// </remarks>
        private static void SkipCore(System.IO.TextReader reader)
        {
            var c = reader.Peek();

            if (c < 0)
                throw new ParseException("Expected a JSON value. Unexpected string termination found at start of value.", 1, ParseError.PrematureEndOfInput);

            switch (c)
            {
                case '{':
                    // object
                    {
                        reader.Read();

                        c = reader.Peek();

                        if (c < 0)
                            throw new ParseException("Expected a JSON Object. Unexpected string termination found after '{{' token denoting that start of a JSON Object.", -1, ParseError.PrematureEndOfInput);

                        SkipWhiteSpace(reader);

                        c = reader.Peek();

                        if (c == '}')
                        {
                            reader.Read();
                        }
                        else
                        {
                            while (true)
                            {
                                c = reader.Read();

                                if (c < 0)
                                    throw new ParseException("Expected a JSON Object. Unexpected string termination found when a JSON String denoting a key was expected.", -1, ParseError.PrematureEndOfInput);

                                if (c != '\"')
                                    throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected a JSON Object. Unexpected character '{0}' found when a string starting with '\"' was expected.", (char)c), -1, ParseError.UnexpectedToken);

                                SkipString(reader);

                                SkipWhiteSpace(reader);

                                c = reader.Peek();

                                if (c != ':')
                                    throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected a JSON Object. Unexpected character '{0}' found when a ':' was expected.", (char)c), -1, ParseError.UnexpectedToken);

                                reader.Read();

                                SkipOne(reader);

                                c = reader.Peek();

                                if (c < 0)
                                    throw new ParseException("Expected a JSON Object. Unexpected string termination found when a ',' or '}}' was expected.", -1, ParseError.PrematureEndOfInput);

                                if (c == ',')
                                {
                                    reader.Read();

                                    SkipWhiteSpace(reader);
                                }
                                else
                                {
                                    break;
                                }
                            }

                            if (c != '}')
                                throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected a JSON Object. Unexpected character '{0}' found when a '}}' was expected.", (char)c), -1, ParseError.UnexpectedToken);

                            reader.Read();
                        }
                    }
                    break;
                case '[':
                    // array
                    {
                        reader.Read();

                        c = reader.Peek();

                        if (c < 0)
                            throw new ParseException("Expected a JSON Object. Unexpected string termination found after '[' token denoting that start of a JSON Array.", -1, ParseError.PrematureEndOfInput);

                        SkipWhiteSpace(reader);

                        c = reader.Peek();

                        if (c == ']')
                        {
                            reader.Read();
                        }
                        else
                        {
                            while (true)
                            {
                                SkipOne(reader);

                                c = reader.Peek();

                                if (c < 0)
                                    throw new ParseException("Expected a JSON Object. Unexpected string termination found when an array element JSON value was expected.", -1, ParseError.PrematureEndOfInput);

                                if (c == ',')
                                {
                                    reader.Read();
                                }
                                else
                                {
                                    break;
                                }
                            }

                            if (c != ']')
                                throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected a JSON Object. Unexpected character '{0}' found when a ']' was expected.", (char)c), -1, ParseError.UnexpectedToken);

                            reader.Read();
                        }
                    }
                    break;
#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable IDE0078 // Use pattern matching. (`reader.Read()` has a side-effect and should not get removed; see https://github.com/dotnet/roslyn-analyzers/issues/4690.)
                case 'n':
                    // null
                    {
                        reader.Read();
                        if (reader.Read() != 'u' || reader.Read() != 'l' || reader.Read() != 'l')
                            throw new ParseException("Expected a JSON Null. Unexpected character encountered while lexing 'null'.", -1, ParseError.UnexpectedToken);
                    }
                    break;
                case 't':
                    //true
                    {
                        reader.Read();
                        if (reader.Read() != 'r' || reader.Read() != 'u' || reader.Read() != 'e')
                            throw new ParseException("Expected a JSON Boolean. Unexpected character encountered while lexing 'true'.", -1, ParseError.UnexpectedToken);
                    }
                    break;
                case 'f':
                    // false
                    {
                        reader.Read();
                        if (reader.Read() != 'a' || reader.Read() != 'l' || reader.Read() != 's' || reader.Read() != 'e')
                            throw new ParseException("Expected a JSON Boolean. Unexpected character encountered while lexing 'false'.", -1, ParseError.UnexpectedToken);
                    }
                    break;
#pragma warning restore IDE0078
#pragma warning disable IDE0079
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
                        // number
                        if (c == '-')
                        {
                            reader.Read();
                            c = reader.Peek();

                            if (c < 0)
                                throw new ParseException("Expected a JSON Number. Unexpected string termination found after '-' sign.", -1, ParseError.PrematureEndOfInput);
                        }

                        if (c == '0')
                        {
                            reader.Read();
                            c = reader.Peek();
                        }
                        else if (c is >= '1' and <= '9')
                        {
                            do
                            {
                                reader.Read();
                                c = reader.Peek();
                            } while (c is >= '0' and <= '9');
                        }
                        else
                        {
                            throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected a JSON Number. Unexpected character '{0}' found after '-' sign.", (char)c), -1, ParseError.UnexpectedToken);
                        }

                        if (c == '.')
                        {
                            reader.Read();
                            c = reader.Peek();

                            if (c < 0)
                                throw new ParseException("Expected a JSON Number. Unexpected string termination found after '.' decimal point.", -1, ParseError.PrematureEndOfInput);

                            if (c is < '0' or > '9')
                                throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected a JSON Number. Unexpected character '{0}' found when a decimal digit was expected after the decimal point.", (char)c), -1, ParseError.UnexpectedToken);

                            do
                            {
                                reader.Read();
                                c = reader.Peek();
                            } while (c is >= '0' and <= '9');
                        }

                        if (c is 'e' or 'E')
                        {
                            reader.Read();
                            c = reader.Peek();

                            if (c < 0)
                                throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected a JSON Number. Unexpected string termination found after '{0}' exponent.", (char)c), -1, ParseError.PrematureEndOfInput);

                            if (c is '+' or '-')
                            {
                                reader.Read();
                                c = reader.Peek();

                                if (c < 0)
                                    throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected a JSON Number. Unexpected string termination found after '{0}' exponent sign.", (char)c), -1, ParseError.PrematureEndOfInput);
                            }

                            if (c is < '0' or > '9')
                                throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected a JSON Number. Unexpected character '{0}' found when a decimal digit was expected for the exponent.", (char)c), -1, ParseError.UnexpectedToken);

                            do
                            {
                                reader.Read();
                                c = reader.Peek();
                            } while (c is >= '0' and <= '9');
                        }
                    }
                    break;
                case '"':
                    reader.Read();
                    SkipString(reader);
                    break;
                default:
                    throw new ParseException("Expected a JSON value.", -1, ParseError.UnexpectedToken);
            }
        }
#endif

        /// <summary>
        /// Skips the remainder of a JSON string literal from the specified string starting at the specified index.
        /// </summary>
        /// <param name="str">The string to lex one JSON value from.</param>
        /// <param name="len">The length of the string.</param>
        /// <param name="b">The start index of the String literal being lexed; used for error reporting.</param>
        /// <param name="i">The index in the string to start lexing from. This value is expected to point to a character within the string literal (i.e. not the initial '"' double quote). This value gets updated to the first index position after the JSON value, if found.</param>
        /// <remarks>
        /// This method enables skipping the remainder of a JSON string literal and is assumed to receive a start index that's beyond the initial '"' double quote character that starts the string literal.
        /// The start index is not allowed to be inside an escape sequence.
        /// </remarks>
        internal static void SkipString(string str, int len, int b, ref int i)
        {
            if (i >= len)
                throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected a JSON String at position '{0}'. Unexpected string termination found after '\"' token denoting that start of a JSON String.", b), i, ParseError.PrematureEndOfInput);

            var c = str[i];

            while (c != '\"')
            {
                if (char.IsControl(c))
                {
                    throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected a JSON String at position '{0}'. Unexpected control character '{1}' found.", b, c), i, ParseError.UnexpectedToken);
                }
                else if (c == '\\')
                {
                    if (++i >= len)
                        throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected a JSON String at position '{0}'. Unexpected string termination found after '\\' escape character.", b), i, ParseError.PrematureEndOfInput);

                    c = str[i];

                    switch (c)
                    {
                        case '"':
                        case '\\':
                        case '/':
                        case 'b':
                        case 'f':
                        case 'n':
                        case 'r':
                        case 't':
                            break;
                        case 'u':
                            if (i + 4 >= len)
                                throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected a JSON String at position '{0}'. Unexpected string termination found in '\\u' escape sequence.", b), i, ParseError.PrematureEndOfInput);
                            if (!IsHexChar(str[i + 1]) || !IsHexChar(str[i + 2]) || !IsHexChar(str[i + 3]) || !IsHexChar(str[i + 4]))
                                throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected a JSON String at position '{0}'. Unexpected character found in '\\u' escape sequence.", b), i, ParseError.UnexpectedToken);
                            i += 4;
                            break;
                        default:
                            throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected a JSON String at position '{0}'. Unexpected character '{1}' found after '\\' escape character.", b, c), i, ParseError.UnexpectedToken);
                    }
                }

                if (++i >= len)
                    throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected a JSON String at position '{0}'. Unexpected string termination found when expecting a character or a '\"' string terminator.", b), i, ParseError.PrematureEndOfInput);

                c = str[i];
            }

            i++;
        }

#if !NO_IO
        /// <summary>
        /// Skips the remainder of a JSON string literal from the specified text reader.
        /// </summary>
        /// <param name="reader">The text reader to lex one JSON value from.</param>
        /// <remarks>
        /// This method enables skipping the remainder of a JSON string literal and is assumed to receive a start index that's beyond the initial '"' double quote character that starts the string literal.
        /// The start index is not allowed to be inside an escape sequence.
        /// </remarks>
        internal static void SkipString(System.IO.TextReader reader)
        {
            var c = reader.Peek();

            if (c < 0)
                throw new ParseException("Expected a JSON String. Unexpected string termination found after '\"' token denoting that start of a JSON String.", -1, ParseError.PrematureEndOfInput);

            while (c != '\"')
            {
                if (char.IsControl((char)c))
                {
                    throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected a JSON String. Unexpected control character '{0}' found.", (char)c), -1, ParseError.UnexpectedToken);
                }
                else if (c == '\\')
                {
                    reader.Read();
                    c = reader.Peek();

                    if (c < 0)
                        throw new ParseException("Expected a JSON String. Unexpected string termination found after '\\' escape character.", -1, ParseError.PrematureEndOfInput);

                    switch (c)
                    {
                        case '"':
                        case '\\':
                        case '/':
                        case 'b':
                        case 'f':
                        case 'n':
                        case 'r':
                        case 't':
                            reader.Read();
                            break;
                        case 'u':
                            reader.Read();
                            if (!IsHexChar((char)reader.Read()) || !IsHexChar((char)reader.Read()) || !IsHexChar((char)reader.Read()) || !IsHexChar((char)reader.Read()))
                                throw new ParseException("Expected a JSON String'. Unexpected character found in '\\u' escape sequence.", -1, ParseError.UnexpectedToken);
                            break;
                        default:
                            throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected a JSON String. Unexpected character '{0}' found after '\\' escape character.", (char)c), -1, ParseError.UnexpectedToken);
                    }
                }
                else
                {
                    reader.Read();
                }

                c = reader.Peek();

                if (c < 0)
                    throw new ParseException("Expected a JSON String. Unexpected string termination found when expecting a character or a '\"' string terminator.", -1, ParseError.PrematureEndOfInput);
            }

            reader.Read();
        }
#endif
    }
}
