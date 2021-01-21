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
        /// <summary>
        /// Lexes and parses a JSON Number as a nullable System.SByte in the specified string starting from the specified index.
        /// </summary>
        /// <param name="str">The string to lex and parse a Number from.</param>
        /// <param name="len">The length of the string.</param>
        /// <param name="i">The index in the string to start lexing from. This value gets updated to the first index position after the Number, if found.</param>
        /// <param name="context">The parser context.</param>
        /// <returns>A System.SByte value or null, if found; otherwise, a ParseException is thrown.</returns>
        /// <remarks>
        /// This method does not consume whitespace; the character at the specified start index in the string is expected to be the start of a valid JSON Number or a JSON null literal.
        /// JSON null literals are case-sensitive.
        /// </remarks>
        internal static SByte? ParseNullableSByte(string str, int len, ref int i, ParserContext context)
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
                return ParseSByte(str, len, ref i, context);
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
                    return ParseSByte(str, len, ref i, context);
                }
            }
#endif
            throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected {0} or null at position '{1}'.", typeof(SByte).Name, b), b, ParseError.InvalidToken);
        }

#if !NO_IO
        /// <summary>
        /// Lexes and parses a JSON Number as a nullable System.SByte in the specified text reader.
        /// </summary>
        /// <param name="reader">The text reader to lex and parse a Number from.</param>
        /// <param name="context">The parser context.</param>
        /// <returns>A System.SByte value or null, if found; otherwise, a ParseException is thrown.</returns>
        /// <remarks>
        /// This method does not consume whitespace; the character at the specified start index in the string is expected to be the start of a valid JSON Number or a JSON null literal.
        /// JSON null literals are case-sensitive.
        /// </remarks>
        internal static SByte? ParseNullableSByte(System.IO.TextReader reader, ParserContext context)
        {
            var c = reader.Peek();

            if (c == 'n')
            {
#if ALLOW_UNSAFE
                if (reader.Read(context.char4, 0, 4) == 4)
                {
                    unsafe
                    {
                        const long NULL = ((long)'n' << 0) | ((long)'u' << 16) | ((long)'l' << 32) | ((long)'l' << 48);

                        fixed (char* s = context.char4) // COVERAGE: Code coverage is partial on the fixed statement due to emitted null checks that can never be hit.
                        {
                            var val = *((long*)s);

                            if (val == NULL)
                            {
                                return null;
                            }
                        }
                    }
                }
#else
                if (TryParseNullSkipN(reader, context))
                {
                    return null;
                }
#endif

                throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected {0} or null.", typeof(SByte).Name), -1, ParseError.InvalidToken);
            }
            else
            {
                return ParseSByte(reader, context);
            }
        }
#endif

        /// <summary>
        /// Lexes and parses a JSON Number as a nullable System.Int16 in the specified string starting from the specified index.
        /// </summary>
        /// <param name="str">The string to lex and parse a Number from.</param>
        /// <param name="len">The length of the string.</param>
        /// <param name="i">The index in the string to start lexing from. This value gets updated to the first index position after the Number, if found.</param>
        /// <param name="context">The parser context.</param>
        /// <returns>A System.Int16 value or null, if found; otherwise, a ParseException is thrown.</returns>
        /// <remarks>
        /// This method does not consume whitespace; the character at the specified start index in the string is expected to be the start of a valid JSON Number or a JSON null literal.
        /// JSON null literals are case-sensitive.
        /// </remarks>
        internal static Int16? ParseNullableInt16(string str, int len, ref int i, ParserContext context)
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
                return ParseInt16(str, len, ref i, context);
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
                    return ParseInt16(str, len, ref i, context);
                }
            }
#endif
            throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected {0} or null at position '{1}'.", typeof(Int16).Name, b), b, ParseError.InvalidToken);
        }

#if !NO_IO
        /// <summary>
        /// Lexes and parses a JSON Number as a nullable System.Int16 in the specified text reader.
        /// </summary>
        /// <param name="reader">The text reader to lex and parse a Number from.</param>
        /// <param name="context">The parser context.</param>
        /// <returns>A System.Int16 value or null, if found; otherwise, a ParseException is thrown.</returns>
        /// <remarks>
        /// This method does not consume whitespace; the character at the specified start index in the string is expected to be the start of a valid JSON Number or a JSON null literal.
        /// JSON null literals are case-sensitive.
        /// </remarks>
        internal static Int16? ParseNullableInt16(System.IO.TextReader reader, ParserContext context)
        {
            var c = reader.Peek();

            if (c == 'n')
            {
#if ALLOW_UNSAFE
                if (reader.Read(context.char4, 0, 4) == 4)
                {
                    unsafe
                    {
                        const long NULL = ((long)'n' << 0) | ((long)'u' << 16) | ((long)'l' << 32) | ((long)'l' << 48);

                        fixed (char* s = context.char4) // COVERAGE: Code coverage is partial on the fixed statement due to emitted null checks that can never be hit.
                        {
                            var val = *((long*)s);

                            if (val == NULL)
                            {
                                return null;
                            }
                        }
                    }
                }
#else
                if (TryParseNullSkipN(reader, context))
                {
                    return null;
                }
#endif

                throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected {0} or null.", typeof(Int16).Name), -1, ParseError.InvalidToken);
            }
            else
            {
                return ParseInt16(reader, context);
            }
        }
#endif

        /// <summary>
        /// Lexes and parses a JSON Number as a nullable System.Int32 in the specified string starting from the specified index.
        /// </summary>
        /// <param name="str">The string to lex and parse a Number from.</param>
        /// <param name="len">The length of the string.</param>
        /// <param name="i">The index in the string to start lexing from. This value gets updated to the first index position after the Number, if found.</param>
        /// <param name="context">The parser context.</param>
        /// <returns>A System.Int32 value or null, if found; otherwise, a ParseException is thrown.</returns>
        /// <remarks>
        /// This method does not consume whitespace; the character at the specified start index in the string is expected to be the start of a valid JSON Number or a JSON null literal.
        /// JSON null literals are case-sensitive.
        /// </remarks>
        internal static Int32? ParseNullableInt32(string str, int len, ref int i, ParserContext context)
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
                return ParseInt32(str, len, ref i, context);
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
                    return ParseInt32(str, len, ref i, context);
                }
            }
#endif
            throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected {0} or null at position '{1}'.", typeof(Int32).Name, b), b, ParseError.InvalidToken);
        }

#if !NO_IO
        /// <summary>
        /// Lexes and parses a JSON Number as a nullable System.Int32 in the specified text reader.
        /// </summary>
        /// <param name="reader">The text reader to lex and parse a Number from.</param>
        /// <param name="context">The parser context.</param>
        /// <returns>A System.Int32 value or null, if found; otherwise, a ParseException is thrown.</returns>
        /// <remarks>
        /// This method does not consume whitespace; the character at the specified start index in the string is expected to be the start of a valid JSON Number or a JSON null literal.
        /// JSON null literals are case-sensitive.
        /// </remarks>
        internal static Int32? ParseNullableInt32(System.IO.TextReader reader, ParserContext context)
        {
            var c = reader.Peek();

            if (c == 'n')
            {
#if ALLOW_UNSAFE
                if (reader.Read(context.char4, 0, 4) == 4)
                {
                    unsafe
                    {
                        const long NULL = ((long)'n' << 0) | ((long)'u' << 16) | ((long)'l' << 32) | ((long)'l' << 48);

                        fixed (char* s = context.char4) // COVERAGE: Code coverage is partial on the fixed statement due to emitted null checks that can never be hit.
                        {
                            var val = *((long*)s);

                            if (val == NULL)
                            {
                                return null;
                            }
                        }
                    }
                }
#else
                if (TryParseNullSkipN(reader, context))
                {
                    return null;
                }
#endif

                throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected {0} or null.", typeof(Int32).Name), -1, ParseError.InvalidToken);
            }
            else
            {
                return ParseInt32(reader, context);
            }
        }
#endif

        /// <summary>
        /// Lexes and parses a JSON Number as a nullable System.Int64 in the specified string starting from the specified index.
        /// </summary>
        /// <param name="str">The string to lex and parse a Number from.</param>
        /// <param name="len">The length of the string.</param>
        /// <param name="i">The index in the string to start lexing from. This value gets updated to the first index position after the Number, if found.</param>
        /// <param name="context">The parser context.</param>
        /// <returns>A System.Int64 value or null, if found; otherwise, a ParseException is thrown.</returns>
        /// <remarks>
        /// This method does not consume whitespace; the character at the specified start index in the string is expected to be the start of a valid JSON Number or a JSON null literal.
        /// JSON null literals are case-sensitive.
        /// </remarks>
        internal static Int64? ParseNullableInt64(string str, int len, ref int i, ParserContext context)
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
                return ParseInt64(str, len, ref i, context);
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
                    return ParseInt64(str, len, ref i, context);
                }
            }
#endif
            throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected {0} or null at position '{1}'.", typeof(Int64).Name, b), b, ParseError.InvalidToken);
        }

#if !NO_IO
        /// <summary>
        /// Lexes and parses a JSON Number as a nullable System.Int64 in the specified text reader.
        /// </summary>
        /// <param name="reader">The text reader to lex and parse a Number from.</param>
        /// <param name="context">The parser context.</param>
        /// <returns>A System.Int64 value or null, if found; otherwise, a ParseException is thrown.</returns>
        /// <remarks>
        /// This method does not consume whitespace; the character at the specified start index in the string is expected to be the start of a valid JSON Number or a JSON null literal.
        /// JSON null literals are case-sensitive.
        /// </remarks>
        internal static Int64? ParseNullableInt64(System.IO.TextReader reader, ParserContext context)
        {
            var c = reader.Peek();

            if (c == 'n')
            {
#if ALLOW_UNSAFE
                if (reader.Read(context.char4, 0, 4) == 4)
                {
                    unsafe
                    {
                        const long NULL = ((long)'n' << 0) | ((long)'u' << 16) | ((long)'l' << 32) | ((long)'l' << 48);

                        fixed (char* s = context.char4) // COVERAGE: Code coverage is partial on the fixed statement due to emitted null checks that can never be hit.
                        {
                            var val = *((long*)s);

                            if (val == NULL)
                            {
                                return null;
                            }
                        }
                    }
                }
#else
                if (TryParseNullSkipN(reader, context))
                {
                    return null;
                }
#endif

                throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected {0} or null.", typeof(Int64).Name), -1, ParseError.InvalidToken);
            }
            else
            {
                return ParseInt64(reader, context);
            }
        }
#endif

        /// <summary>
        /// Lexes and parses a JSON Number as a nullable System.Byte in the specified string starting from the specified index.
        /// </summary>
        /// <param name="str">The string to lex and parse a Number from.</param>
        /// <param name="len">The length of the string.</param>
        /// <param name="i">The index in the string to start lexing from. This value gets updated to the first index position after the Number, if found.</param>
        /// <param name="context">The parser context.</param>
        /// <returns>A System.Byte value or null, if found; otherwise, a ParseException is thrown.</returns>
        /// <remarks>
        /// This method does not consume whitespace; the character at the specified start index in the string is expected to be the start of a valid JSON Number or a JSON null literal.
        /// JSON null literals are case-sensitive.
        /// </remarks>
        internal static Byte? ParseNullableByte(string str, int len, ref int i, ParserContext context)
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
                return ParseByte(str, len, ref i, context);
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
                    return ParseByte(str, len, ref i, context);
                }
            }
#endif
            throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected {0} or null at position '{1}'.", typeof(Byte).Name, b), b, ParseError.InvalidToken);
        }

#if !NO_IO
        /// <summary>
        /// Lexes and parses a JSON Number as a nullable System.Byte in the specified text reader.
        /// </summary>
        /// <param name="reader">The text reader to lex and parse a Number from.</param>
        /// <param name="context">The parser context.</param>
        /// <returns>A System.Byte value or null, if found; otherwise, a ParseException is thrown.</returns>
        /// <remarks>
        /// This method does not consume whitespace; the character at the specified start index in the string is expected to be the start of a valid JSON Number or a JSON null literal.
        /// JSON null literals are case-sensitive.
        /// </remarks>
        internal static Byte? ParseNullableByte(System.IO.TextReader reader, ParserContext context)
        {
            var c = reader.Peek();

            if (c == 'n')
            {
#if ALLOW_UNSAFE
                if (reader.Read(context.char4, 0, 4) == 4)
                {
                    unsafe
                    {
                        const long NULL = ((long)'n' << 0) | ((long)'u' << 16) | ((long)'l' << 32) | ((long)'l' << 48);

                        fixed (char* s = context.char4) // COVERAGE: Code coverage is partial on the fixed statement due to emitted null checks that can never be hit.
                        {
                            var val = *((long*)s);

                            if (val == NULL)
                            {
                                return null;
                            }
                        }
                    }
                }
#else
                if (TryParseNullSkipN(reader, context))
                {
                    return null;
                }
#endif

                throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected {0} or null.", typeof(Byte).Name), -1, ParseError.InvalidToken);
            }
            else
            {
                return ParseByte(reader, context);
            }
        }
#endif

        /// <summary>
        /// Lexes and parses a JSON Number as a nullable System.UInt16 in the specified string starting from the specified index.
        /// </summary>
        /// <param name="str">The string to lex and parse a Number from.</param>
        /// <param name="len">The length of the string.</param>
        /// <param name="i">The index in the string to start lexing from. This value gets updated to the first index position after the Number, if found.</param>
        /// <param name="context">The parser context.</param>
        /// <returns>A System.UInt16 value or null, if found; otherwise, a ParseException is thrown.</returns>
        /// <remarks>
        /// This method does not consume whitespace; the character at the specified start index in the string is expected to be the start of a valid JSON Number or a JSON null literal.
        /// JSON null literals are case-sensitive.
        /// </remarks>
        internal static UInt16? ParseNullableUInt16(string str, int len, ref int i, ParserContext context)
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
                return ParseUInt16(str, len, ref i, context);
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
                    return ParseUInt16(str, len, ref i, context);
                }
            }
#endif
            throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected {0} or null at position '{1}'.", typeof(UInt16).Name, b), b, ParseError.InvalidToken);
        }

#if !NO_IO
        /// <summary>
        /// Lexes and parses a JSON Number as a nullable System.UInt16 in the specified text reader.
        /// </summary>
        /// <param name="reader">The text reader to lex and parse a Number from.</param>
        /// <param name="context">The parser context.</param>
        /// <returns>A System.UInt16 value or null, if found; otherwise, a ParseException is thrown.</returns>
        /// <remarks>
        /// This method does not consume whitespace; the character at the specified start index in the string is expected to be the start of a valid JSON Number or a JSON null literal.
        /// JSON null literals are case-sensitive.
        /// </remarks>
        internal static UInt16? ParseNullableUInt16(System.IO.TextReader reader, ParserContext context)
        {
            var c = reader.Peek();

            if (c == 'n')
            {
#if ALLOW_UNSAFE
                if (reader.Read(context.char4, 0, 4) == 4)
                {
                    unsafe
                    {
                        const long NULL = ((long)'n' << 0) | ((long)'u' << 16) | ((long)'l' << 32) | ((long)'l' << 48);

                        fixed (char* s = context.char4) // COVERAGE: Code coverage is partial on the fixed statement due to emitted null checks that can never be hit.
                        {
                            var val = *((long*)s);

                            if (val == NULL)
                            {
                                return null;
                            }
                        }
                    }
                }
#else
                if (TryParseNullSkipN(reader, context))
                {
                    return null;
                }
#endif

                throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected {0} or null.", typeof(UInt16).Name), -1, ParseError.InvalidToken);
            }
            else
            {
                return ParseUInt16(reader, context);
            }
        }
#endif

        /// <summary>
        /// Lexes and parses a JSON Number as a nullable System.UInt32 in the specified string starting from the specified index.
        /// </summary>
        /// <param name="str">The string to lex and parse a Number from.</param>
        /// <param name="len">The length of the string.</param>
        /// <param name="i">The index in the string to start lexing from. This value gets updated to the first index position after the Number, if found.</param>
        /// <param name="context">The parser context.</param>
        /// <returns>A System.UInt32 value or null, if found; otherwise, a ParseException is thrown.</returns>
        /// <remarks>
        /// This method does not consume whitespace; the character at the specified start index in the string is expected to be the start of a valid JSON Number or a JSON null literal.
        /// JSON null literals are case-sensitive.
        /// </remarks>
        internal static UInt32? ParseNullableUInt32(string str, int len, ref int i, ParserContext context)
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
                return ParseUInt32(str, len, ref i, context);
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
                    return ParseUInt32(str, len, ref i, context);
                }
            }
#endif
            throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected {0} or null at position '{1}'.", typeof(UInt32).Name, b), b, ParseError.InvalidToken);
        }

#if !NO_IO
        /// <summary>
        /// Lexes and parses a JSON Number as a nullable System.UInt32 in the specified text reader.
        /// </summary>
        /// <param name="reader">The text reader to lex and parse a Number from.</param>
        /// <param name="context">The parser context.</param>
        /// <returns>A System.UInt32 value or null, if found; otherwise, a ParseException is thrown.</returns>
        /// <remarks>
        /// This method does not consume whitespace; the character at the specified start index in the string is expected to be the start of a valid JSON Number or a JSON null literal.
        /// JSON null literals are case-sensitive.
        /// </remarks>
        internal static UInt32? ParseNullableUInt32(System.IO.TextReader reader, ParserContext context)
        {
            var c = reader.Peek();

            if (c == 'n')
            {
#if ALLOW_UNSAFE
                if (reader.Read(context.char4, 0, 4) == 4)
                {
                    unsafe
                    {
                        const long NULL = ((long)'n' << 0) | ((long)'u' << 16) | ((long)'l' << 32) | ((long)'l' << 48);

                        fixed (char* s = context.char4) // COVERAGE: Code coverage is partial on the fixed statement due to emitted null checks that can never be hit.
                        {
                            var val = *((long*)s);

                            if (val == NULL)
                            {
                                return null;
                            }
                        }
                    }
                }
#else
                if (TryParseNullSkipN(reader, context))
                {
                    return null;
                }
#endif

                throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected {0} or null.", typeof(UInt32).Name), -1, ParseError.InvalidToken);
            }
            else
            {
                return ParseUInt32(reader, context);
            }
        }
#endif

        /// <summary>
        /// Lexes and parses a JSON Number as a nullable System.UInt64 in the specified string starting from the specified index.
        /// </summary>
        /// <param name="str">The string to lex and parse a Number from.</param>
        /// <param name="len">The length of the string.</param>
        /// <param name="i">The index in the string to start lexing from. This value gets updated to the first index position after the Number, if found.</param>
        /// <param name="context">The parser context.</param>
        /// <returns>A System.UInt64 value or null, if found; otherwise, a ParseException is thrown.</returns>
        /// <remarks>
        /// This method does not consume whitespace; the character at the specified start index in the string is expected to be the start of a valid JSON Number or a JSON null literal.
        /// JSON null literals are case-sensitive.
        /// </remarks>
        internal static UInt64? ParseNullableUInt64(string str, int len, ref int i, ParserContext context)
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
                return ParseUInt64(str, len, ref i, context);
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
                    return ParseUInt64(str, len, ref i, context);
                }
            }
#endif
            throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected {0} or null at position '{1}'.", typeof(UInt64).Name, b), b, ParseError.InvalidToken);
        }

#if !NO_IO
        /// <summary>
        /// Lexes and parses a JSON Number as a nullable System.UInt64 in the specified text reader.
        /// </summary>
        /// <param name="reader">The text reader to lex and parse a Number from.</param>
        /// <param name="context">The parser context.</param>
        /// <returns>A System.UInt64 value or null, if found; otherwise, a ParseException is thrown.</returns>
        /// <remarks>
        /// This method does not consume whitespace; the character at the specified start index in the string is expected to be the start of a valid JSON Number or a JSON null literal.
        /// JSON null literals are case-sensitive.
        /// </remarks>
        internal static UInt64? ParseNullableUInt64(System.IO.TextReader reader, ParserContext context)
        {
            var c = reader.Peek();

            if (c == 'n')
            {
#if ALLOW_UNSAFE
                if (reader.Read(context.char4, 0, 4) == 4)
                {
                    unsafe
                    {
                        const long NULL = ((long)'n' << 0) | ((long)'u' << 16) | ((long)'l' << 32) | ((long)'l' << 48);

                        fixed (char* s = context.char4) // COVERAGE: Code coverage is partial on the fixed statement due to emitted null checks that can never be hit.
                        {
                            var val = *((long*)s);

                            if (val == NULL)
                            {
                                return null;
                            }
                        }
                    }
                }
#else
                if (TryParseNullSkipN(reader, context))
                {
                    return null;
                }
#endif

                throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected {0} or null.", typeof(UInt64).Name), -1, ParseError.InvalidToken);
            }
            else
            {
                return ParseUInt64(reader, context);
            }
        }
#endif

        /// <summary>
        /// Lexes and parses a JSON Boolean as a nullable System.Boolean in the specified string starting from the specified index.
        /// </summary>
        /// <param name="str">The string to lex and parse a Boolean from.</param>
        /// <param name="len">The length of the string.</param>
        /// <param name="i">The index in the string to start lexing from. This value gets updated to the first index position after the Boolean, if found.</param>
        /// <param name="context">The parser context.</param>
        /// <returns>A System.Boolean value or null, if found; otherwise, a ParseException is thrown.</returns>
        /// <remarks>
        /// This method does not consume whitespace; the character at the specified start index in the string is expected to be the start of a valid JSON Boolean or a JSON null literal.
        /// JSON null literals are case-sensitive.
        /// </remarks>
        internal static Boolean? ParseNullableBoolean(string str, int len, ref int i, ParserContext context)
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
                return ParseBoolean(str, len, ref i, context);
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
                    return ParseBoolean(str, len, ref i, context);
                }
            }
#endif
            throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected {0} or null at position '{1}'.", typeof(Boolean).Name, b), b, ParseError.InvalidToken);
        }

#if !NO_IO
        /// <summary>
        /// Lexes and parses a JSON Boolean as a nullable System.Boolean in the specified text reader.
        /// </summary>
        /// <param name="reader">The text reader to lex and parse a Boolean from.</param>
        /// <param name="context">The parser context.</param>
        /// <returns>A System.Boolean value or null, if found; otherwise, a ParseException is thrown.</returns>
        /// <remarks>
        /// This method does not consume whitespace; the character at the specified start index in the string is expected to be the start of a valid JSON Boolean or a JSON null literal.
        /// JSON null literals are case-sensitive.
        /// </remarks>
        internal static Boolean? ParseNullableBoolean(System.IO.TextReader reader, ParserContext context)
        {
            var c = reader.Peek();

            if (c == 'n')
            {
#if ALLOW_UNSAFE
                if (reader.Read(context.char4, 0, 4) == 4)
                {
                    unsafe
                    {
                        const long NULL = ((long)'n' << 0) | ((long)'u' << 16) | ((long)'l' << 32) | ((long)'l' << 48);

                        fixed (char* s = context.char4) // COVERAGE: Code coverage is partial on the fixed statement due to emitted null checks that can never be hit.
                        {
                            var val = *((long*)s);

                            if (val == NULL)
                            {
                                return null;
                            }
                        }
                    }
                }
#else
                if (TryParseNullSkipN(reader, context))
                {
                    return null;
                }
#endif

                throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected {0} or null.", typeof(Boolean).Name), -1, ParseError.InvalidToken);
            }
            else
            {
                return ParseBoolean(reader, context);
            }
        }
#endif

        /// <summary>
        /// Lexes and parses a JSON String as a nullable System.Char in the specified string starting from the specified index.
        /// </summary>
        /// <param name="str">The string to lex and parse a String from.</param>
        /// <param name="len">The length of the string.</param>
        /// <param name="i">The index in the string to start lexing from. This value gets updated to the first index position after the String, if found.</param>
        /// <param name="context">The parser context.</param>
        /// <returns>A System.Char value or null, if found; otherwise, a ParseException is thrown.</returns>
        /// <remarks>
        /// This method does not consume whitespace; the character at the specified start index in the string is expected to be the start of a valid JSON String or a JSON null literal.
        /// JSON null literals are case-sensitive.
        /// </remarks>
        internal static Char? ParseNullableChar(string str, int len, ref int i, ParserContext context)
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
                return ParseChar(str, len, ref i, context);
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
                    return ParseChar(str, len, ref i, context);
                }
            }
#endif
            throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected {0} or null at position '{1}'.", typeof(Char).Name, b), b, ParseError.InvalidToken);
        }

#if !NO_IO
        /// <summary>
        /// Lexes and parses a JSON String as a nullable System.Char in the specified text reader.
        /// </summary>
        /// <param name="reader">The text reader to lex and parse a String from.</param>
        /// <param name="context">The parser context.</param>
        /// <returns>A System.Char value or null, if found; otherwise, a ParseException is thrown.</returns>
        /// <remarks>
        /// This method does not consume whitespace; the character at the specified start index in the string is expected to be the start of a valid JSON String or a JSON null literal.
        /// JSON null literals are case-sensitive.
        /// </remarks>
        internal static Char? ParseNullableChar(System.IO.TextReader reader, ParserContext context)
        {
            var c = reader.Peek();

            if (c == 'n')
            {
#if ALLOW_UNSAFE
                if (reader.Read(context.char4, 0, 4) == 4)
                {
                    unsafe
                    {
                        const long NULL = ((long)'n' << 0) | ((long)'u' << 16) | ((long)'l' << 32) | ((long)'l' << 48);

                        fixed (char* s = context.char4) // COVERAGE: Code coverage is partial on the fixed statement due to emitted null checks that can never be hit.
                        {
                            var val = *((long*)s);

                            if (val == NULL)
                            {
                                return null;
                            }
                        }
                    }
                }
#else
                if (TryParseNullSkipN(reader, context))
                {
                    return null;
                }
#endif

                throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected {0} or null.", typeof(Char).Name), -1, ParseError.InvalidToken);
            }
            else
            {
                return ParseChar(reader, context);
            }
        }
#endif

        /// <summary>
        /// Lexes and parses a JSON String as a nullable System.DateTime in the specified string starting from the specified index.
        /// </summary>
        /// <param name="str">The string to lex and parse a String from.</param>
        /// <param name="len">The length of the string.</param>
        /// <param name="i">The index in the string to start lexing from. This value gets updated to the first index position after the String, if found.</param>
        /// <param name="context">The parser context.</param>
        /// <returns>A System.DateTime value or null, if found; otherwise, a ParseException is thrown.</returns>
        /// <remarks>
        /// This method does not consume whitespace; the character at the specified start index in the string is expected to be the start of a valid JSON String or a JSON null literal.
        /// JSON null literals are case-sensitive.
        /// </remarks>
        internal static DateTime? ParseNullableDateTime(string str, int len, ref int i, ParserContext context)
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
                return ParseDateTime(str, len, ref i, context);
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
                    return ParseDateTime(str, len, ref i, context);
                }
            }
#endif
            throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected {0} or null at position '{1}'.", typeof(DateTime).Name, b), b, ParseError.InvalidToken);
        }

#if !NO_IO
        /// <summary>
        /// Lexes and parses a JSON String as a nullable System.DateTime in the specified text reader.
        /// </summary>
        /// <param name="reader">The text reader to lex and parse a String from.</param>
        /// <param name="context">The parser context.</param>
        /// <returns>A System.DateTime value or null, if found; otherwise, a ParseException is thrown.</returns>
        /// <remarks>
        /// This method does not consume whitespace; the character at the specified start index in the string is expected to be the start of a valid JSON String or a JSON null literal.
        /// JSON null literals are case-sensitive.
        /// </remarks>
        internal static DateTime? ParseNullableDateTime(System.IO.TextReader reader, ParserContext context)
        {
            var c = reader.Peek();

            if (c == 'n')
            {
#if ALLOW_UNSAFE
                if (reader.Read(context.char4, 0, 4) == 4)
                {
                    unsafe
                    {
                        const long NULL = ((long)'n' << 0) | ((long)'u' << 16) | ((long)'l' << 32) | ((long)'l' << 48);

                        fixed (char* s = context.char4) // COVERAGE: Code coverage is partial on the fixed statement due to emitted null checks that can never be hit.
                        {
                            var val = *((long*)s);

                            if (val == NULL)
                            {
                                return null;
                            }
                        }
                    }
                }
#else
                if (TryParseNullSkipN(reader, context))
                {
                    return null;
                }
#endif

                throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected {0} or null.", typeof(DateTime).Name), -1, ParseError.InvalidToken);
            }
            else
            {
                return ParseDateTime(reader, context);
            }
        }
#endif

        /// <summary>
        /// Lexes and parses a JSON String as a nullable System.DateTimeOffset in the specified string starting from the specified index.
        /// </summary>
        /// <param name="str">The string to lex and parse a String from.</param>
        /// <param name="len">The length of the string.</param>
        /// <param name="i">The index in the string to start lexing from. This value gets updated to the first index position after the String, if found.</param>
        /// <param name="context">The parser context.</param>
        /// <returns>A System.DateTimeOffset value or null, if found; otherwise, a ParseException is thrown.</returns>
        /// <remarks>
        /// This method does not consume whitespace; the character at the specified start index in the string is expected to be the start of a valid JSON String or a JSON null literal.
        /// JSON null literals are case-sensitive.
        /// </remarks>
        internal static DateTimeOffset? ParseNullableDateTimeOffset(string str, int len, ref int i, ParserContext context)
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
                return ParseDateTimeOffset(str, len, ref i, context);
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
                    return ParseDateTimeOffset(str, len, ref i, context);
                }
            }
#endif
            throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected {0} or null at position '{1}'.", typeof(DateTimeOffset).Name, b), b, ParseError.InvalidToken);
        }

#if !NO_IO
        /// <summary>
        /// Lexes and parses a JSON String as a nullable System.DateTimeOffset in the specified text reader.
        /// </summary>
        /// <param name="reader">The text reader to lex and parse a String from.</param>
        /// <param name="context">The parser context.</param>
        /// <returns>A System.DateTimeOffset value or null, if found; otherwise, a ParseException is thrown.</returns>
        /// <remarks>
        /// This method does not consume whitespace; the character at the specified start index in the string is expected to be the start of a valid JSON String or a JSON null literal.
        /// JSON null literals are case-sensitive.
        /// </remarks>
        internal static DateTimeOffset? ParseNullableDateTimeOffset(System.IO.TextReader reader, ParserContext context)
        {
            var c = reader.Peek();

            if (c == 'n')
            {
#if ALLOW_UNSAFE
                if (reader.Read(context.char4, 0, 4) == 4)
                {
                    unsafe
                    {
                        const long NULL = ((long)'n' << 0) | ((long)'u' << 16) | ((long)'l' << 32) | ((long)'l' << 48);

                        fixed (char* s = context.char4) // COVERAGE: Code coverage is partial on the fixed statement due to emitted null checks that can never be hit.
                        {
                            var val = *((long*)s);

                            if (val == NULL)
                            {
                                return null;
                            }
                        }
                    }
                }
#else
                if (TryParseNullSkipN(reader, context))
                {
                    return null;
                }
#endif

                throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected {0} or null.", typeof(DateTimeOffset).Name), -1, ParseError.InvalidToken);
            }
            else
            {
                return ParseDateTimeOffset(reader, context);
            }
        }
#endif

        /// <summary>
        /// Lexes and parses a JSON Number as a nullable System.Single in the specified string starting from the specified index.
        /// </summary>
        /// <param name="str">The string to lex and parse a Number from.</param>
        /// <param name="len">The length of the string.</param>
        /// <param name="i">The index in the string to start lexing from. This value gets updated to the first index position after the Number, if found.</param>
        /// <param name="context">The parser context.</param>
        /// <returns>A System.Single value or null, if found; otherwise, a ParseException is thrown.</returns>
        /// <remarks>
        /// This method does not consume whitespace; the character at the specified start index in the string is expected to be the start of a valid JSON Number or a JSON null literal.
        /// JSON null literals are case-sensitive.
        /// </remarks>
        internal static Single? ParseNullableSingle(string str, int len, ref int i, ParserContext context)
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
                return ParseSingle(str, len, ref i, context);
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
                    return ParseSingle(str, len, ref i, context);
                }
            }
#endif
            throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected {0} or null at position '{1}'.", typeof(Single).Name, b), b, ParseError.InvalidToken);
        }

#if !NO_IO
        /// <summary>
        /// Lexes and parses a JSON Number as a nullable System.Single in the specified text reader.
        /// </summary>
        /// <param name="reader">The text reader to lex and parse a Number from.</param>
        /// <param name="context">The parser context.</param>
        /// <returns>A System.Single value or null, if found; otherwise, a ParseException is thrown.</returns>
        /// <remarks>
        /// This method does not consume whitespace; the character at the specified start index in the string is expected to be the start of a valid JSON Number or a JSON null literal.
        /// JSON null literals are case-sensitive.
        /// </remarks>
        internal static Single? ParseNullableSingle(System.IO.TextReader reader, ParserContext context)
        {
            var c = reader.Peek();

            if (c == 'n')
            {
#if ALLOW_UNSAFE
                if (reader.Read(context.char4, 0, 4) == 4)
                {
                    unsafe
                    {
                        const long NULL = ((long)'n' << 0) | ((long)'u' << 16) | ((long)'l' << 32) | ((long)'l' << 48);

                        fixed (char* s = context.char4) // COVERAGE: Code coverage is partial on the fixed statement due to emitted null checks that can never be hit.
                        {
                            var val = *((long*)s);

                            if (val == NULL)
                            {
                                return null;
                            }
                        }
                    }
                }
#else
                if (TryParseNullSkipN(reader, context))
                {
                    return null;
                }
#endif

                throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected {0} or null.", typeof(Single).Name), -1, ParseError.InvalidToken);
            }
            else
            {
                return ParseSingle(reader, context);
            }
        }
#endif

        /// <summary>
        /// Lexes and parses a JSON Number as a nullable System.Double in the specified string starting from the specified index.
        /// </summary>
        /// <param name="str">The string to lex and parse a Number from.</param>
        /// <param name="len">The length of the string.</param>
        /// <param name="i">The index in the string to start lexing from. This value gets updated to the first index position after the Number, if found.</param>
        /// <param name="context">The parser context.</param>
        /// <returns>A System.Double value or null, if found; otherwise, a ParseException is thrown.</returns>
        /// <remarks>
        /// This method does not consume whitespace; the character at the specified start index in the string is expected to be the start of a valid JSON Number or a JSON null literal.
        /// JSON null literals are case-sensitive.
        /// </remarks>
        internal static Double? ParseNullableDouble(string str, int len, ref int i, ParserContext context)
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
                return ParseDouble(str, len, ref i, context);
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
                    return ParseDouble(str, len, ref i, context);
                }
            }
#endif
            throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected {0} or null at position '{1}'.", typeof(Double).Name, b), b, ParseError.InvalidToken);
        }

#if !NO_IO
        /// <summary>
        /// Lexes and parses a JSON Number as a nullable System.Double in the specified text reader.
        /// </summary>
        /// <param name="reader">The text reader to lex and parse a Number from.</param>
        /// <param name="context">The parser context.</param>
        /// <returns>A System.Double value or null, if found; otherwise, a ParseException is thrown.</returns>
        /// <remarks>
        /// This method does not consume whitespace; the character at the specified start index in the string is expected to be the start of a valid JSON Number or a JSON null literal.
        /// JSON null literals are case-sensitive.
        /// </remarks>
        internal static Double? ParseNullableDouble(System.IO.TextReader reader, ParserContext context)
        {
            var c = reader.Peek();

            if (c == 'n')
            {
#if ALLOW_UNSAFE
                if (reader.Read(context.char4, 0, 4) == 4)
                {
                    unsafe
                    {
                        const long NULL = ((long)'n' << 0) | ((long)'u' << 16) | ((long)'l' << 32) | ((long)'l' << 48);

                        fixed (char* s = context.char4) // COVERAGE: Code coverage is partial on the fixed statement due to emitted null checks that can never be hit.
                        {
                            var val = *((long*)s);

                            if (val == NULL)
                            {
                                return null;
                            }
                        }
                    }
                }
#else
                if (TryParseNullSkipN(reader, context))
                {
                    return null;
                }
#endif

                throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected {0} or null.", typeof(Double).Name), -1, ParseError.InvalidToken);
            }
            else
            {
                return ParseDouble(reader, context);
            }
        }
#endif

        /// <summary>
        /// Lexes and parses a JSON Number as a nullable System.Decimal in the specified string starting from the specified index.
        /// </summary>
        /// <param name="str">The string to lex and parse a Number from.</param>
        /// <param name="len">The length of the string.</param>
        /// <param name="i">The index in the string to start lexing from. This value gets updated to the first index position after the Number, if found.</param>
        /// <param name="context">The parser context.</param>
        /// <returns>A System.Decimal value or null, if found; otherwise, a ParseException is thrown.</returns>
        /// <remarks>
        /// This method does not consume whitespace; the character at the specified start index in the string is expected to be the start of a valid JSON Number or a JSON null literal.
        /// JSON null literals are case-sensitive.
        /// </remarks>
        internal static Decimal? ParseNullableDecimal(string str, int len, ref int i, ParserContext context)
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
                return ParseDecimal(str, len, ref i, context);
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
                    return ParseDecimal(str, len, ref i, context);
                }
            }
#endif
            throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected {0} or null at position '{1}'.", typeof(Decimal).Name, b), b, ParseError.InvalidToken);
        }

#if !NO_IO
        /// <summary>
        /// Lexes and parses a JSON Number as a nullable System.Decimal in the specified text reader.
        /// </summary>
        /// <param name="reader">The text reader to lex and parse a Number from.</param>
        /// <param name="context">The parser context.</param>
        /// <returns>A System.Decimal value or null, if found; otherwise, a ParseException is thrown.</returns>
        /// <remarks>
        /// This method does not consume whitespace; the character at the specified start index in the string is expected to be the start of a valid JSON Number or a JSON null literal.
        /// JSON null literals are case-sensitive.
        /// </remarks>
        internal static Decimal? ParseNullableDecimal(System.IO.TextReader reader, ParserContext context)
        {
            var c = reader.Peek();

            if (c == 'n')
            {
#if ALLOW_UNSAFE
                if (reader.Read(context.char4, 0, 4) == 4)
                {
                    unsafe
                    {
                        const long NULL = ((long)'n' << 0) | ((long)'u' << 16) | ((long)'l' << 32) | ((long)'l' << 48);

                        fixed (char* s = context.char4) // COVERAGE: Code coverage is partial on the fixed statement due to emitted null checks that can never be hit.
                        {
                            var val = *((long*)s);

                            if (val == NULL)
                            {
                                return null;
                            }
                        }
                    }
                }
#else
                if (TryParseNullSkipN(reader, context))
                {
                    return null;
                }
#endif

                throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected {0} or null.", typeof(Decimal).Name), -1, ParseError.InvalidToken);
            }
            else
            {
                return ParseDecimal(reader, context);
            }
        }
#endif

    }
}
