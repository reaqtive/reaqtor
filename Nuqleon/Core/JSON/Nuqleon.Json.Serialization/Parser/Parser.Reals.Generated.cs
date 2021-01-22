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
        // TODO: Add support for NaN, -Infinity, Infinity

        /// <summary>
        /// Lexes and parses a JSON Number as a System.Single in the specified string starting from the specified index.
        /// </summary>
        /// <param name="str">The string to lex and parse a Number from.</param>
        /// <param name="len">The length of the string.</param>
        /// <param name="i">The index in the string to start lexing from. This value gets updated to the first index position after the Number, if found.</param>
        /// <param name="context">The parser context.</param>
        /// <returns>A System.Single value, if found; otherwise, a ParseException is thrown.</returns>
        /// <remarks>
        /// This method does not consume whitespace; the character at the specified start index in the string is expected to be the start of a valid JSON Number.
        /// </remarks>
        internal static Single ParseSingle(string str, int len, ref int i, ParserContext context)
        {
            //
            // NB: We resort to simply using the built-in BCL functions. Those are reasonable efficient and are also used by many JSON frameworks (compatibility) but suffer from
            //     string allocations and minor imprecisions (cf. RealParser in Roslyn addresses this, but is about 5x slower). An "atof" approach was tried as well but leads to
            //     major imprecisions compared to the built-in BCL functions.
            //
            // CONSIDER: Implement these TryParse functions in an approach similar to the BCL but using a StringSegment instead, thus eliminating the substring allocations. The
            //           tricky part is in the NumberBufferToDouble extern call these parse functions depend on. We'd still need to lex the string to determine StringSegment span
            //           but we'd be able to avoid the String allocations.
            //

            var b = i;

#if USE_STRINGPOOL && ALLOW_UNSAFE && HAS_MEMCPY
            if (TryLexNumber(str, len, i, out var num))
            {
                var buffer = default(string);
                try
                {
                    var numStr = default(string);

                    buffer = context.StringPool.Allocate(num);
                    if (buffer != null)
                    {
                        unsafe
                        {
                            fixed (char* dest = buffer)
                            {
                                fixed (char* input = str)
                                {
                                    var src = input + i;
                                    var byteCount = num * sizeof(char);
                                    Buffer.MemoryCopy(src, dest, byteCount, byteCount);
                                }
                            }
                        }

                        numStr = buffer;
                    }
                    else
                    {
                        numStr = str.Substring(i, num);
                    }

                    i += num;

                    return Single.Parse(numStr, NumberStyles.AllowDecimalPoint | NumberStyles.AllowExponent | NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture);
                }
                finally
                {
                    context.StringPool.Free(buffer);
                }
            }
#else
            if (TryLexNumber(str, len, ref i, out var numStr))
            {
                return Single.Parse(numStr, NumberStyles.AllowDecimalPoint | NumberStyles.AllowExponent | NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture);
            }
#endif
            throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected Single at position '{0}'.", b), i, ParseError.UnexpectedToken);
        }

#if !NO_IO
        /// <summary>
        /// Lexes and parses a JSON Number as a System.Single in the specified text reader.
        /// </summary>
        /// <param name="reader">The text reader to lex and parse a Number from.</param>
        /// <param name="context">The parser context.</param>
        /// <returns>A System.Single value, if found; otherwise, a ParseException is thrown.</returns>
        /// <remarks>
        /// This method does not consume whitespace; the character at the specified start index in the string is expected to be the start of a valid JSON Number.
        /// </remarks>
        internal static Single ParseSingle(System.IO.TextReader reader, ParserContext context)
        {
            var res = default(Single);

            if (TryLexNumber(reader, out var numStr) && Single.TryParse(numStr, NumberStyles.AllowDecimalPoint | NumberStyles.AllowExponent | NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out res))
            {
                return res;
            }

            throw new ParseException("Expected Single.", -1, ParseError.UnexpectedToken);
        }
#endif

        /// <summary>
        /// Lexes and parses a JSON Number as a System.Double in the specified string starting from the specified index.
        /// </summary>
        /// <param name="str">The string to lex and parse a Number from.</param>
        /// <param name="len">The length of the string.</param>
        /// <param name="i">The index in the string to start lexing from. This value gets updated to the first index position after the Number, if found.</param>
        /// <param name="context">The parser context.</param>
        /// <returns>A System.Double value, if found; otherwise, a ParseException is thrown.</returns>
        /// <remarks>
        /// This method does not consume whitespace; the character at the specified start index in the string is expected to be the start of a valid JSON Number.
        /// </remarks>
        internal static Double ParseDouble(string str, int len, ref int i, ParserContext context)
        {
            //
            // NB: We resort to simply using the built-in BCL functions. Those are reasonable efficient and are also used by many JSON frameworks (compatibility) but suffer from
            //     string allocations and minor imprecisions (cf. RealParser in Roslyn addresses this, but is about 5x slower). An "atof" approach was tried as well but leads to
            //     major imprecisions compared to the built-in BCL functions.
            //
            // CONSIDER: Implement these TryParse functions in an approach similar to the BCL but using a StringSegment instead, thus eliminating the substring allocations. The
            //           tricky part is in the NumberBufferToDouble extern call these parse functions depend on. We'd still need to lex the string to determine StringSegment span
            //           but we'd be able to avoid the String allocations.
            //

            var b = i;

#if USE_STRINGPOOL && ALLOW_UNSAFE && HAS_MEMCPY
            if (TryLexNumber(str, len, i, out var num))
            {
                var buffer = default(string);
                try
                {
                    var numStr = default(string);

                    buffer = context.StringPool.Allocate(num);
                    if (buffer != null)
                    {
                        unsafe
                        {
                            fixed (char* dest = buffer)
                            {
                                fixed (char* input = str)
                                {
                                    var src = input + i;
                                    var byteCount = num * sizeof(char);
                                    Buffer.MemoryCopy(src, dest, byteCount, byteCount);
                                }
                            }
                        }

                        numStr = buffer;
                    }
                    else
                    {
                        numStr = str.Substring(i, num);
                    }

                    i += num;

                    return Double.Parse(numStr, NumberStyles.AllowDecimalPoint | NumberStyles.AllowExponent | NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture);
                }
                finally
                {
                    context.StringPool.Free(buffer);
                }
            }
#else
            if (TryLexNumber(str, len, ref i, out var numStr))
            {
                return Double.Parse(numStr, NumberStyles.AllowDecimalPoint | NumberStyles.AllowExponent | NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture);
            }
#endif
            throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected Double at position '{0}'.", b), i, ParseError.UnexpectedToken);
        }

#if !NO_IO
        /// <summary>
        /// Lexes and parses a JSON Number as a System.Double in the specified text reader.
        /// </summary>
        /// <param name="reader">The text reader to lex and parse a Number from.</param>
        /// <param name="context">The parser context.</param>
        /// <returns>A System.Double value, if found; otherwise, a ParseException is thrown.</returns>
        /// <remarks>
        /// This method does not consume whitespace; the character at the specified start index in the string is expected to be the start of a valid JSON Number.
        /// </remarks>
        internal static Double ParseDouble(System.IO.TextReader reader, ParserContext context)
        {
            var res = default(Double);

            if (TryLexNumber(reader, out var numStr) && Double.TryParse(numStr, NumberStyles.AllowDecimalPoint | NumberStyles.AllowExponent | NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out res))
            {
                return res;
            }

            throw new ParseException("Expected Double.", -1, ParseError.UnexpectedToken);
        }
#endif

        /// <summary>
        /// Lexes and parses a JSON Number as a System.Decimal in the specified string starting from the specified index.
        /// </summary>
        /// <param name="str">The string to lex and parse a Number from.</param>
        /// <param name="len">The length of the string.</param>
        /// <param name="i">The index in the string to start lexing from. This value gets updated to the first index position after the Number, if found.</param>
        /// <param name="context">The parser context.</param>
        /// <returns>A System.Decimal value, if found; otherwise, a ParseException is thrown.</returns>
        /// <remarks>
        /// This method does not consume whitespace; the character at the specified start index in the string is expected to be the start of a valid JSON Number.
        /// </remarks>
        internal static Decimal ParseDecimal(string str, int len, ref int i, ParserContext context)
        {
            //
            // NB: We resort to simply using the built-in BCL functions. Those are reasonable efficient and are also used by many JSON frameworks (compatibility) but suffer from
            //     string allocations and minor imprecisions (cf. RealParser in Roslyn addresses this, but is about 5x slower). An "atof" approach was tried as well but leads to
            //     major imprecisions compared to the built-in BCL functions.
            //
            // CONSIDER: Implement these TryParse functions in an approach similar to the BCL but using a StringSegment instead, thus eliminating the substring allocations. The
            //           tricky part is in the NumberBufferToDouble extern call these parse functions depend on. We'd still need to lex the string to determine StringSegment span
            //           but we'd be able to avoid the String allocations.
            //

            var b = i;

#if USE_STRINGPOOL && ALLOW_UNSAFE && HAS_MEMCPY
            if (TryLexNumber(str, len, i, out var num))
            {
                var buffer = default(string);
                try
                {
                    var numStr = default(string);

                    buffer = context.StringPool.Allocate(num);
                    if (buffer != null)
                    {
                        unsafe
                        {
                            fixed (char* dest = buffer)
                            {
                                fixed (char* input = str)
                                {
                                    var src = input + i;
                                    var byteCount = num * sizeof(char);
                                    Buffer.MemoryCopy(src, dest, byteCount, byteCount);
                                }
                            }
                        }

                        numStr = buffer;
                    }
                    else
                    {
                        numStr = str.Substring(i, num);
                    }

                    i += num;

                    return Decimal.Parse(numStr, NumberStyles.AllowDecimalPoint | NumberStyles.AllowExponent | NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture);
                }
                finally
                {
                    context.StringPool.Free(buffer);
                }
            }
#else
            if (TryLexNumber(str, len, ref i, out var numStr))
            {
                return Decimal.Parse(numStr, NumberStyles.AllowDecimalPoint | NumberStyles.AllowExponent | NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture);
            }
#endif
            throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected Decimal at position '{0}'.", b), i, ParseError.UnexpectedToken);
        }

#if !NO_IO
        /// <summary>
        /// Lexes and parses a JSON Number as a System.Decimal in the specified text reader.
        /// </summary>
        /// <param name="reader">The text reader to lex and parse a Number from.</param>
        /// <param name="context">The parser context.</param>
        /// <returns>A System.Decimal value, if found; otherwise, a ParseException is thrown.</returns>
        /// <remarks>
        /// This method does not consume whitespace; the character at the specified start index in the string is expected to be the start of a valid JSON Number.
        /// </remarks>
        internal static Decimal ParseDecimal(System.IO.TextReader reader, ParserContext context)
        {
            var res = default(Decimal);

            if (TryLexNumber(reader, out var numStr) && Decimal.TryParse(numStr, NumberStyles.AllowDecimalPoint | NumberStyles.AllowExponent | NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out res))
            {
                return res;
            }

            throw new ParseException("Expected Decimal.", -1, ParseError.UnexpectedToken);
        }
#endif

    }
}
