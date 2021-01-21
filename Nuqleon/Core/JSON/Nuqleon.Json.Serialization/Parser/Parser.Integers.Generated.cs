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
using System;
using System.Globalization;

namespace Nuqleon.Json.Serialization
{
    internal partial class Parser
    {
        //
        // CONSIDER: Allow integral values to be specified using exponents, e.g. 123.4e1 can be turned into an integer with value 1234.
        // CONSIDER: Add support for BigInteger.
        //

        /// <summary>
        /// Lexes and parses a JSON Number as a System.SByte in the specified string starting from the specified index.
        /// </summary>
        /// <param name="str">The string to lex and parse a Number from.</param>
        /// <param name="len">The length of the string.</param>
        /// <param name="i">The index in the string to start lexing from. This value gets updated to the first index position after the Number, if found.</param>
        /// <param name="context">The parser context.</param>
        /// <returns>A System.SByte value, if found; otherwise, a ParseException is thrown.</returns>
        /// <remarks>
        /// This method does not consume whitespace; the character at the specified start index in the string is expected to be the start of a valid JSON Number.
        /// </remarks>
        internal static SByte ParseSByte(string str, int len, ref int i, ParserContext context)
        {
            var res = default(SByte);

            var b = i;

            if (i >= len)
                throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected {0} at position '{1}'. Unexpected string termination found.", typeof(SByte).Name, b), i, ParseError.PrematureEndOfInput);

            var c = str[i];

            var neg = false;

            if (c == '-')
            {
                if (++i >= len)
                    throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected {0} at position '{1}'. Unexpected string termination found after '-' sign.", typeof(SByte).Name, b), i, ParseError.PrematureEndOfInput);

                neg = true;
                c = str[i];
            }

            if (c == '0')
            {
                i++;
                return res;
            }
            else if (c >= '1' && c <= '9')
            {
                res = (SByte)(c - '0');
                i++;
            }
            else
            {
                throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected {0} at position '{1}'. Unexpected character '{2}' found at start of number.", typeof(SByte).Name, b, c), i, ParseError.UnexpectedToken);
            }

            while (i < len && ((c = str[i]) >= '0') && c <= '9')
            {
                var next = (SByte)(res * 10 + (SByte)(c - '0'));

                if (next < res)
                {
                    if (next == SByte.MinValue && neg)
                    {
                        res = next;
                        i++;
                        continue;
                    }

                    throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected {0} at position '{1}'. The JSON number does not fit in a value of type {0}.", typeof(SByte).Name, b), i, ParseError.UnexpectedToken);
                }
                else
                {
                    res = next;
                }

                i++;
            }

            return neg ? (SByte)(-res) : res;
        }

#if !NO_IO
        /// <summary>
        /// Lexes and parses a JSON Number as a System.SByte in the specified text reader.
        /// </summary>
        /// <param name="reader">The text reader to lex and parse a System.SByte from.</param>
        /// <param name="context">The parser context.</param>
        /// <returns>A System.SByte value, if found; otherwise, a ParseException is thrown.</returns>
        /// <remarks>
        /// This method does not consume whitespace; the character at the specified start index in the string is expected to be the start of a valid JSON Number.
        /// </remarks>
        internal static SByte ParseSByte(System.IO.TextReader reader, ParserContext context)
        {
            var res = default(SByte);

            var c = reader.Read();

            if (c < 0)
                throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected {0}. Unexpected string termination found.", typeof(SByte).Name), -1, ParseError.PrematureEndOfInput);

            var neg = false;

            if (c == '-')
            {
                neg = true;
                c = reader.Read();

                if (c < 0)
                    throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected {0}. Unexpected string termination found after '-' sign.", typeof(SByte).Name), -1, ParseError.PrematureEndOfInput);
            }

            if (c == '0')
            {
                return res;
            }
            else if (c >= '1' && c <= '9')
            {
                res = (SByte)(c - '0');
            }
            else
            {
                throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected {0}. Unexpected character '{1}' found at start of number.", typeof(SByte).Name, c), -1, ParseError.UnexpectedToken);
            }

            while (((c = reader.Peek()) >= '0') && c <= '9')
            {
                reader.Read();

                var next = (SByte)(res * 10 + (SByte)(c - '0'));

                if (next < res)
                {
                    if (next == SByte.MinValue && neg)
                    {
                        res = next;
                        continue;
                    }

                    throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected {0}. The JSON number does not fit in a value of type {0}.", typeof(SByte).Name), -1, ParseError.UnexpectedToken);
                }
                else
                {
                    res = next;
                }
            }

            return neg ? (SByte)(-res) : res;
        }
#endif

        /// <summary>
        /// Lexes and parses a JSON Number as a System.Int16 in the specified string starting from the specified index.
        /// </summary>
        /// <param name="str">The string to lex and parse a Number from.</param>
        /// <param name="len">The length of the string.</param>
        /// <param name="i">The index in the string to start lexing from. This value gets updated to the first index position after the Number, if found.</param>
        /// <param name="context">The parser context.</param>
        /// <returns>A System.Int16 value, if found; otherwise, a ParseException is thrown.</returns>
        /// <remarks>
        /// This method does not consume whitespace; the character at the specified start index in the string is expected to be the start of a valid JSON Number.
        /// </remarks>
        internal static Int16 ParseInt16(string str, int len, ref int i, ParserContext context)
        {
            var res = default(Int16);

            var b = i;

            if (i >= len)
                throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected {0} at position '{1}'. Unexpected string termination found.", typeof(Int16).Name, b), i, ParseError.PrematureEndOfInput);

            var c = str[i];

            var neg = false;

            if (c == '-')
            {
                if (++i >= len)
                    throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected {0} at position '{1}'. Unexpected string termination found after '-' sign.", typeof(Int16).Name, b), i, ParseError.PrematureEndOfInput);

                neg = true;
                c = str[i];
            }

            if (c == '0')
            {
                i++;
                return res;
            }
            else if (c >= '1' && c <= '9')
            {
                res = (Int16)(c - '0');
                i++;
            }
            else
            {
                throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected {0} at position '{1}'. Unexpected character '{2}' found at start of number.", typeof(Int16).Name, b, c), i, ParseError.UnexpectedToken);
            }

            while (i < len && ((c = str[i]) >= '0') && c <= '9')
            {
                var next = (Int16)(res * 10 + (Int16)(c - '0'));

                if (next < res)
                {
                    if (next == Int16.MinValue && neg)
                    {
                        res = next;
                        i++;
                        continue;
                    }

                    throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected {0} at position '{1}'. The JSON number does not fit in a value of type {0}.", typeof(Int16).Name, b), i, ParseError.UnexpectedToken);
                }
                else
                {
                    res = next;
                }

                i++;
            }

            return neg ? (Int16)(-res) : res;
        }

#if !NO_IO
        /// <summary>
        /// Lexes and parses a JSON Number as a System.Int16 in the specified text reader.
        /// </summary>
        /// <param name="reader">The text reader to lex and parse a System.Int16 from.</param>
        /// <param name="context">The parser context.</param>
        /// <returns>A System.Int16 value, if found; otherwise, a ParseException is thrown.</returns>
        /// <remarks>
        /// This method does not consume whitespace; the character at the specified start index in the string is expected to be the start of a valid JSON Number.
        /// </remarks>
        internal static Int16 ParseInt16(System.IO.TextReader reader, ParserContext context)
        {
            var res = default(Int16);

            var c = reader.Read();

            if (c < 0)
                throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected {0}. Unexpected string termination found.", typeof(Int16).Name), -1, ParseError.PrematureEndOfInput);

            var neg = false;

            if (c == '-')
            {
                neg = true;
                c = reader.Read();

                if (c < 0)
                    throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected {0}. Unexpected string termination found after '-' sign.", typeof(Int16).Name), -1, ParseError.PrematureEndOfInput);
            }

            if (c == '0')
            {
                return res;
            }
            else if (c >= '1' && c <= '9')
            {
                res = (Int16)(c - '0');
            }
            else
            {
                throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected {0}. Unexpected character '{1}' found at start of number.", typeof(Int16).Name, c), -1, ParseError.UnexpectedToken);
            }

            while (((c = reader.Peek()) >= '0') && c <= '9')
            {
                reader.Read();

                var next = (Int16)(res * 10 + (Int16)(c - '0'));

                if (next < res)
                {
                    if (next == Int16.MinValue && neg)
                    {
                        res = next;
                        continue;
                    }

                    throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected {0}. The JSON number does not fit in a value of type {0}.", typeof(Int16).Name), -1, ParseError.UnexpectedToken);
                }
                else
                {
                    res = next;
                }
            }

            return neg ? (Int16)(-res) : res;
        }
#endif

        /// <summary>
        /// Lexes and parses a JSON Number as a System.Int32 in the specified string starting from the specified index.
        /// </summary>
        /// <param name="str">The string to lex and parse a Number from.</param>
        /// <param name="len">The length of the string.</param>
        /// <param name="i">The index in the string to start lexing from. This value gets updated to the first index position after the Number, if found.</param>
        /// <param name="context">The parser context.</param>
        /// <returns>A System.Int32 value, if found; otherwise, a ParseException is thrown.</returns>
        /// <remarks>
        /// This method does not consume whitespace; the character at the specified start index in the string is expected to be the start of a valid JSON Number.
        /// </remarks>
        internal static Int32 ParseInt32(string str, int len, ref int i, ParserContext context)
        {
            var res = default(Int32);

            var b = i;

            if (i >= len)
                throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected {0} at position '{1}'. Unexpected string termination found.", typeof(Int32).Name, b), i, ParseError.PrematureEndOfInput);

            var c = str[i];

            var neg = false;

            if (c == '-')
            {
                if (++i >= len)
                    throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected {0} at position '{1}'. Unexpected string termination found after '-' sign.", typeof(Int32).Name, b), i, ParseError.PrematureEndOfInput);

                neg = true;
                c = str[i];
            }

            if (c == '0')
            {
                i++;
                return res;
            }
            else if (c >= '1' && c <= '9')
            {
                res = (Int32)(c - '0');
                i++;
            }
            else
            {
                throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected {0} at position '{1}'. Unexpected character '{2}' found at start of number.", typeof(Int32).Name, b, c), i, ParseError.UnexpectedToken);
            }

            while (i < len && ((c = str[i]) >= '0') && c <= '9')
            {
                var next = (Int32)(res * 10 + (Int32)(c - '0'));

                if (next < res)
                {
                    if (next == Int32.MinValue && neg)
                    {
                        res = next;
                        i++;
                        continue;
                    }

                    throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected {0} at position '{1}'. The JSON number does not fit in a value of type {0}.", typeof(Int32).Name, b), i, ParseError.UnexpectedToken);
                }
                else
                {
                    res = next;
                }

                i++;
            }

            return neg ? (Int32)(-res) : res;
        }

#if !NO_IO
        /// <summary>
        /// Lexes and parses a JSON Number as a System.Int32 in the specified text reader.
        /// </summary>
        /// <param name="reader">The text reader to lex and parse a System.Int32 from.</param>
        /// <param name="context">The parser context.</param>
        /// <returns>A System.Int32 value, if found; otherwise, a ParseException is thrown.</returns>
        /// <remarks>
        /// This method does not consume whitespace; the character at the specified start index in the string is expected to be the start of a valid JSON Number.
        /// </remarks>
        internal static Int32 ParseInt32(System.IO.TextReader reader, ParserContext context)
        {
            var res = default(Int32);

            var c = reader.Read();

            if (c < 0)
                throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected {0}. Unexpected string termination found.", typeof(Int32).Name), -1, ParseError.PrematureEndOfInput);

            var neg = false;

            if (c == '-')
            {
                neg = true;
                c = reader.Read();

                if (c < 0)
                    throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected {0}. Unexpected string termination found after '-' sign.", typeof(Int32).Name), -1, ParseError.PrematureEndOfInput);
            }

            if (c == '0')
            {
                return res;
            }
            else if (c >= '1' && c <= '9')
            {
                res = (Int32)(c - '0');
            }
            else
            {
                throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected {0}. Unexpected character '{1}' found at start of number.", typeof(Int32).Name, c), -1, ParseError.UnexpectedToken);
            }

            while (((c = reader.Peek()) >= '0') && c <= '9')
            {
                reader.Read();

                var next = (Int32)(res * 10 + (Int32)(c - '0'));

                if (next < res)
                {
                    if (next == Int32.MinValue && neg)
                    {
                        res = next;
                        continue;
                    }

                    throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected {0}. The JSON number does not fit in a value of type {0}.", typeof(Int32).Name), -1, ParseError.UnexpectedToken);
                }
                else
                {
                    res = next;
                }
            }

            return neg ? (Int32)(-res) : res;
        }
#endif

        /// <summary>
        /// Lexes and parses a JSON Number as a System.Int64 in the specified string starting from the specified index.
        /// </summary>
        /// <param name="str">The string to lex and parse a Number from.</param>
        /// <param name="len">The length of the string.</param>
        /// <param name="i">The index in the string to start lexing from. This value gets updated to the first index position after the Number, if found.</param>
        /// <param name="context">The parser context.</param>
        /// <returns>A System.Int64 value, if found; otherwise, a ParseException is thrown.</returns>
        /// <remarks>
        /// This method does not consume whitespace; the character at the specified start index in the string is expected to be the start of a valid JSON Number.
        /// </remarks>
        internal static Int64 ParseInt64(string str, int len, ref int i, ParserContext context)
        {
            var res = default(Int64);

            var b = i;

            if (i >= len)
                throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected {0} at position '{1}'. Unexpected string termination found.", typeof(Int64).Name, b), i, ParseError.PrematureEndOfInput);

            var c = str[i];

            var neg = false;

            if (c == '-')
            {
                if (++i >= len)
                    throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected {0} at position '{1}'. Unexpected string termination found after '-' sign.", typeof(Int64).Name, b), i, ParseError.PrematureEndOfInput);

                neg = true;
                c = str[i];
            }

            if (c == '0')
            {
                i++;
                return res;
            }
            else if (c >= '1' && c <= '9')
            {
                res = (Int64)(c - '0');
                i++;
            }
            else
            {
                throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected {0} at position '{1}'. Unexpected character '{2}' found at start of number.", typeof(Int64).Name, b, c), i, ParseError.UnexpectedToken);
            }

            while (i < len && ((c = str[i]) >= '0') && c <= '9')
            {
                var next = (Int64)(res * 10 + (Int64)(c - '0'));

                if (next < res)
                {
                    if (next == Int64.MinValue && neg)
                    {
                        res = next;
                        i++;
                        continue;
                    }

                    throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected {0} at position '{1}'. The JSON number does not fit in a value of type {0}.", typeof(Int64).Name, b), i, ParseError.UnexpectedToken);
                }
                else
                {
                    res = next;
                }

                i++;
            }

            return neg ? (Int64)(-res) : res;
        }

#if !NO_IO
        /// <summary>
        /// Lexes and parses a JSON Number as a System.Int64 in the specified text reader.
        /// </summary>
        /// <param name="reader">The text reader to lex and parse a System.Int64 from.</param>
        /// <param name="context">The parser context.</param>
        /// <returns>A System.Int64 value, if found; otherwise, a ParseException is thrown.</returns>
        /// <remarks>
        /// This method does not consume whitespace; the character at the specified start index in the string is expected to be the start of a valid JSON Number.
        /// </remarks>
        internal static Int64 ParseInt64(System.IO.TextReader reader, ParserContext context)
        {
            var res = default(Int64);

            var c = reader.Read();

            if (c < 0)
                throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected {0}. Unexpected string termination found.", typeof(Int64).Name), -1, ParseError.PrematureEndOfInput);

            var neg = false;

            if (c == '-')
            {
                neg = true;
                c = reader.Read();

                if (c < 0)
                    throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected {0}. Unexpected string termination found after '-' sign.", typeof(Int64).Name), -1, ParseError.PrematureEndOfInput);
            }

            if (c == '0')
            {
                return res;
            }
            else if (c >= '1' && c <= '9')
            {
                res = (Int64)(c - '0');
            }
            else
            {
                throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected {0}. Unexpected character '{1}' found at start of number.", typeof(Int64).Name, c), -1, ParseError.UnexpectedToken);
            }

            while (((c = reader.Peek()) >= '0') && c <= '9')
            {
                reader.Read();

                var next = (Int64)(res * 10 + (Int64)(c - '0'));

                if (next < res)
                {
                    if (next == Int64.MinValue && neg)
                    {
                        res = next;
                        continue;
                    }

                    throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected {0}. The JSON number does not fit in a value of type {0}.", typeof(Int64).Name), -1, ParseError.UnexpectedToken);
                }
                else
                {
                    res = next;
                }
            }

            return neg ? (Int64)(-res) : res;
        }
#endif

        /// <summary>
        /// Lexes and parses a JSON Number as a System.Byte in the specified string starting from the specified index.
        /// </summary>
        /// <param name="str">The string to lex and parse a Number from.</param>
        /// <param name="len">The length of the string.</param>
        /// <param name="i">The index in the string to start lexing from. This value gets updated to the first index position after the Number, if found.</param>
        /// <param name="context">The parser context.</param>
        /// <returns>A System.Byte value, if found; otherwise, a ParseException is thrown.</returns>
        /// <remarks>
        /// This method does not consume whitespace; the character at the specified start index in the string is expected to be the start of a valid JSON Number.
        /// </remarks>
        internal static Byte ParseByte(string str, int len, ref int i, ParserContext context)
        {
            var res = default(Byte);

            var b = i;

            if (i >= len)
                throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected {0} at position '{1}'. Unexpected string termination found.", typeof(Byte).Name, b), i, ParseError.PrematureEndOfInput);

            var c = str[i];

            if (c == '0')
            {
                i++;
                return res;
            }
            else if (c >= '1' && c <= '9')
            {
                res = (Byte)(c - '0');
                i++;
            }
            else
            {
                throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected {0} at position '{1}'. Unexpected character '{2}' found at start of number.", typeof(Byte).Name, b, c), i, ParseError.UnexpectedToken);
            }

            while (i < len && ((c = str[i]) >= '0') && c <= '9')
            {
                var next = (Byte)(res * 10 + (Byte)(c - '0'));

                if (next < res)
                {
                    throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected {0} at position '{1}'. The JSON number does not fit in a value of type {0}.", typeof(Byte).Name, b), i, ParseError.UnexpectedToken);
                }
                else
                {
                    res = next;
                }

                i++;
            }

            return res;
        }

#if !NO_IO
        /// <summary>
        /// Lexes and parses a JSON Number as a System.Byte in the specified text reader.
        /// </summary>
        /// <param name="reader">The text reader to lex and parse a System.Byte from.</param>
        /// <param name="context">The parser context.</param>
        /// <returns>A System.Byte value, if found; otherwise, a ParseException is thrown.</returns>
        /// <remarks>
        /// This method does not consume whitespace; the character at the specified start index in the string is expected to be the start of a valid JSON Number.
        /// </remarks>
        internal static Byte ParseByte(System.IO.TextReader reader, ParserContext context)
        {
            var res = default(Byte);

            var c = reader.Read();

            if (c < 0)
                throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected {0}. Unexpected string termination found.", typeof(Byte).Name), -1, ParseError.PrematureEndOfInput);

            if (c == '0')
            {
                return res;
            }
            else if (c >= '1' && c <= '9')
            {
                res = (Byte)(c - '0');
            }
            else
            {
                throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected {0}. Unexpected character '{1}' found at start of number.", typeof(Byte).Name, c), -1, ParseError.UnexpectedToken);
            }

            while (((c = reader.Peek()) >= '0') && c <= '9')
            {
                reader.Read();

                var next = (Byte)(res * 10 + (Byte)(c - '0'));

                if (next < res)
                {
                    throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected {0}. The JSON number does not fit in a value of type {0}.", typeof(Byte).Name), -1, ParseError.UnexpectedToken);
                }
                else
                {
                    res = next;
                }
            }

            return res;
        }
#endif

        /// <summary>
        /// Lexes and parses a JSON Number as a System.UInt16 in the specified string starting from the specified index.
        /// </summary>
        /// <param name="str">The string to lex and parse a Number from.</param>
        /// <param name="len">The length of the string.</param>
        /// <param name="i">The index in the string to start lexing from. This value gets updated to the first index position after the Number, if found.</param>
        /// <param name="context">The parser context.</param>
        /// <returns>A System.UInt16 value, if found; otherwise, a ParseException is thrown.</returns>
        /// <remarks>
        /// This method does not consume whitespace; the character at the specified start index in the string is expected to be the start of a valid JSON Number.
        /// </remarks>
        internal static UInt16 ParseUInt16(string str, int len, ref int i, ParserContext context)
        {
            var res = default(UInt16);

            var b = i;

            if (i >= len)
                throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected {0} at position '{1}'. Unexpected string termination found.", typeof(UInt16).Name, b), i, ParseError.PrematureEndOfInput);

            var c = str[i];

            if (c == '0')
            {
                i++;
                return res;
            }
            else if (c >= '1' && c <= '9')
            {
                res = (UInt16)(c - '0');
                i++;
            }
            else
            {
                throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected {0} at position '{1}'. Unexpected character '{2}' found at start of number.", typeof(UInt16).Name, b, c), i, ParseError.UnexpectedToken);
            }

            while (i < len && ((c = str[i]) >= '0') && c <= '9')
            {
                var next = (UInt16)(res * 10 + (UInt16)(c - '0'));

                if (next < res)
                {
                    throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected {0} at position '{1}'. The JSON number does not fit in a value of type {0}.", typeof(UInt16).Name, b), i, ParseError.UnexpectedToken);
                }
                else
                {
                    res = next;
                }

                i++;
            }

            return res;
        }

#if !NO_IO
        /// <summary>
        /// Lexes and parses a JSON Number as a System.UInt16 in the specified text reader.
        /// </summary>
        /// <param name="reader">The text reader to lex and parse a System.UInt16 from.</param>
        /// <param name="context">The parser context.</param>
        /// <returns>A System.UInt16 value, if found; otherwise, a ParseException is thrown.</returns>
        /// <remarks>
        /// This method does not consume whitespace; the character at the specified start index in the string is expected to be the start of a valid JSON Number.
        /// </remarks>
        internal static UInt16 ParseUInt16(System.IO.TextReader reader, ParserContext context)
        {
            var res = default(UInt16);

            var c = reader.Read();

            if (c < 0)
                throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected {0}. Unexpected string termination found.", typeof(UInt16).Name), -1, ParseError.PrematureEndOfInput);

            if (c == '0')
            {
                return res;
            }
            else if (c >= '1' && c <= '9')
            {
                res = (UInt16)(c - '0');
            }
            else
            {
                throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected {0}. Unexpected character '{1}' found at start of number.", typeof(UInt16).Name, c), -1, ParseError.UnexpectedToken);
            }

            while (((c = reader.Peek()) >= '0') && c <= '9')
            {
                reader.Read();

                var next = (UInt16)(res * 10 + (UInt16)(c - '0'));

                if (next < res)
                {
                    throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected {0}. The JSON number does not fit in a value of type {0}.", typeof(UInt16).Name), -1, ParseError.UnexpectedToken);
                }
                else
                {
                    res = next;
                }
            }

            return res;
        }
#endif

        /// <summary>
        /// Lexes and parses a JSON Number as a System.UInt32 in the specified string starting from the specified index.
        /// </summary>
        /// <param name="str">The string to lex and parse a Number from.</param>
        /// <param name="len">The length of the string.</param>
        /// <param name="i">The index in the string to start lexing from. This value gets updated to the first index position after the Number, if found.</param>
        /// <param name="context">The parser context.</param>
        /// <returns>A System.UInt32 value, if found; otherwise, a ParseException is thrown.</returns>
        /// <remarks>
        /// This method does not consume whitespace; the character at the specified start index in the string is expected to be the start of a valid JSON Number.
        /// </remarks>
        internal static UInt32 ParseUInt32(string str, int len, ref int i, ParserContext context)
        {
            var res = default(UInt32);

            var b = i;

            if (i >= len)
                throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected {0} at position '{1}'. Unexpected string termination found.", typeof(UInt32).Name, b), i, ParseError.PrematureEndOfInput);

            var c = str[i];

            if (c == '0')
            {
                i++;
                return res;
            }
            else if (c >= '1' && c <= '9')
            {
                res = (UInt32)(c - '0');
                i++;
            }
            else
            {
                throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected {0} at position '{1}'. Unexpected character '{2}' found at start of number.", typeof(UInt32).Name, b, c), i, ParseError.UnexpectedToken);
            }

            while (i < len && ((c = str[i]) >= '0') && c <= '9')
            {
                var next = (UInt32)(res * 10 + (UInt32)(c - '0'));

                if (next < res)
                {
                    throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected {0} at position '{1}'. The JSON number does not fit in a value of type {0}.", typeof(UInt32).Name, b), i, ParseError.UnexpectedToken);
                }
                else
                {
                    res = next;
                }

                i++;
            }

            return res;
        }

#if !NO_IO
        /// <summary>
        /// Lexes and parses a JSON Number as a System.UInt32 in the specified text reader.
        /// </summary>
        /// <param name="reader">The text reader to lex and parse a System.UInt32 from.</param>
        /// <param name="context">The parser context.</param>
        /// <returns>A System.UInt32 value, if found; otherwise, a ParseException is thrown.</returns>
        /// <remarks>
        /// This method does not consume whitespace; the character at the specified start index in the string is expected to be the start of a valid JSON Number.
        /// </remarks>
        internal static UInt32 ParseUInt32(System.IO.TextReader reader, ParserContext context)
        {
            var res = default(UInt32);

            var c = reader.Read();

            if (c < 0)
                throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected {0}. Unexpected string termination found.", typeof(UInt32).Name), -1, ParseError.PrematureEndOfInput);

            if (c == '0')
            {
                return res;
            }
            else if (c >= '1' && c <= '9')
            {
                res = (UInt32)(c - '0');
            }
            else
            {
                throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected {0}. Unexpected character '{1}' found at start of number.", typeof(UInt32).Name, c), -1, ParseError.UnexpectedToken);
            }

            while (((c = reader.Peek()) >= '0') && c <= '9')
            {
                reader.Read();

                var next = (UInt32)(res * 10 + (UInt32)(c - '0'));

                if (next < res)
                {
                    throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected {0}. The JSON number does not fit in a value of type {0}.", typeof(UInt32).Name), -1, ParseError.UnexpectedToken);
                }
                else
                {
                    res = next;
                }
            }

            return res;
        }
#endif

        /// <summary>
        /// Lexes and parses a JSON Number as a System.UInt64 in the specified string starting from the specified index.
        /// </summary>
        /// <param name="str">The string to lex and parse a Number from.</param>
        /// <param name="len">The length of the string.</param>
        /// <param name="i">The index in the string to start lexing from. This value gets updated to the first index position after the Number, if found.</param>
        /// <param name="context">The parser context.</param>
        /// <returns>A System.UInt64 value, if found; otherwise, a ParseException is thrown.</returns>
        /// <remarks>
        /// This method does not consume whitespace; the character at the specified start index in the string is expected to be the start of a valid JSON Number.
        /// </remarks>
        internal static UInt64 ParseUInt64(string str, int len, ref int i, ParserContext context)
        {
            var res = default(UInt64);

            var b = i;

            if (i >= len)
                throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected {0} at position '{1}'. Unexpected string termination found.", typeof(UInt64).Name, b), i, ParseError.PrematureEndOfInput);

            var c = str[i];

            if (c == '0')
            {
                i++;
                return res;
            }
            else if (c >= '1' && c <= '9')
            {
                res = (UInt64)(c - '0');
                i++;
            }
            else
            {
                throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected {0} at position '{1}'. Unexpected character '{2}' found at start of number.", typeof(UInt64).Name, b, c), i, ParseError.UnexpectedToken);
            }

            while (i < len && ((c = str[i]) >= '0') && c <= '9')
            {
                var next = (UInt64)(res * 10 + (UInt64)(c - '0'));

                if (next < res)
                {
                    throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected {0} at position '{1}'. The JSON number does not fit in a value of type {0}.", typeof(UInt64).Name, b), i, ParseError.UnexpectedToken);
                }
                else
                {
                    res = next;
                }

                i++;
            }

            return res;
        }

#if !NO_IO
        /// <summary>
        /// Lexes and parses a JSON Number as a System.UInt64 in the specified text reader.
        /// </summary>
        /// <param name="reader">The text reader to lex and parse a System.UInt64 from.</param>
        /// <param name="context">The parser context.</param>
        /// <returns>A System.UInt64 value, if found; otherwise, a ParseException is thrown.</returns>
        /// <remarks>
        /// This method does not consume whitespace; the character at the specified start index in the string is expected to be the start of a valid JSON Number.
        /// </remarks>
        internal static UInt64 ParseUInt64(System.IO.TextReader reader, ParserContext context)
        {
            var res = default(UInt64);

            var c = reader.Read();

            if (c < 0)
                throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected {0}. Unexpected string termination found.", typeof(UInt64).Name), -1, ParseError.PrematureEndOfInput);

            if (c == '0')
            {
                return res;
            }
            else if (c >= '1' && c <= '9')
            {
                res = (UInt64)(c - '0');
            }
            else
            {
                throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected {0}. Unexpected character '{1}' found at start of number.", typeof(UInt64).Name, c), -1, ParseError.UnexpectedToken);
            }

            while (((c = reader.Peek()) >= '0') && c <= '9')
            {
                reader.Read();

                var next = (UInt64)(res * 10 + (UInt64)(c - '0'));

                if (next < res)
                {
                    throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected {0}. The JSON number does not fit in a value of type {0}.", typeof(UInt64).Name), -1, ParseError.UnexpectedToken);
                }
                else
                {
                    res = next;
                }
            }

            return res;
        }
#endif

    }
}
