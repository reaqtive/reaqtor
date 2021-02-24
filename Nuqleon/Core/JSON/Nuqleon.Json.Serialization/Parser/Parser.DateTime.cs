// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 04/05/2016 - Created fast JSON deserializer functionality.
//   BD - 05/08/2016 - Added support for deserialization from text readers.
//

using System;
using System.Globalization;

using Nuqleon.Json.Parser;

namespace Nuqleon.Json.Serialization
{
    internal partial class Parser
    {
        //
        // NB: These functions don't decode escape sequences in a JSON string.
        //

        /// <summary>
        /// Lexes and parses a JSON String as a System.DateTime using ISO-8601 notation in the specified string starting from the specified index.
        /// </summary>
        /// <param name="str">The string to lex and parse a DateTime from.</param>
        /// <param name="len">The length of the string.</param>
        /// <param name="i">The index in the string to start lexing from. This value gets updated to the first index position after the DateTime, if found.</param>
        /// <param name="context">The parser context.</param>
        /// <returns>A System.DateTime value in UTC time, if found; otherwise, a ParseException is thrown.</returns>
        /// <remarks>
        /// This method does not consume whitespace; the character at the specified start index in the string is expected to be the start of a valid JSON String.
        /// This method expects a JSON String containing an ISO-8601 DateTime notation conform RFC 3339. Other representations of DateTime values are not supported.
        /// </remarks>
        internal static DateTime ParseDateTime(string str, int len, ref int i, ParserContext context)
        {
            var dto = ParseDateTimeOffset(str, len, ref i, context);
            return dto.UtcDateTime;
        }

#if !NO_IO
        /// <summary>
        /// Lexes and parses a JSON String as a System.DateTime using ISO-8601 notation in the specified text reader.
        /// </summary>
        /// <param name="reader">The text reader to lex and parse a DateTime from.</param>
        /// <param name="context">The parser context.</param>
        /// <returns>A System.DateTime value in UTC time, if found; otherwise, a ParseException is thrown.</returns>
        /// <remarks>
        /// This method does not consume whitespace; the character at the specified start index in the string is expected to be the start of a valid JSON String.
        /// This method expects a JSON String containing an ISO-8601 DateTime notation conform RFC 3339. Other representations of DateTime values are not supported.
        /// </remarks>
        internal static DateTime ParseDateTime(System.IO.TextReader reader, ParserContext context)
        {
            var dto = ParseDateTimeOffset(reader, context);
            return dto.UtcDateTime;
        }
#endif

        /// <summary>
        /// Lexes and parses a JSON String as a System.DateTimeOffset using ISO-8601 notation in the specified string starting from the specified index.
        /// </summary>
        /// <param name="str">The string to lex and parse a DateTimeOffset from.</param>
        /// <param name="len">The length of the string.</param>
        /// <param name="i">The index in the string to start lexing from. This value gets updated to the first index position after the DateTimeOffset, if found.</param>
        /// <param name="_">The parser context.</param>
        /// <returns>A System.DateTimeOffset value, if found; otherwise, a ParseException is thrown.</returns>
        /// <remarks>
        /// This method does not consume whitespace; the character at the specified start index in the string is expected to be the start of a valid JSON String.
        /// This method expects a JSON String containing an ISO-8601 DateTime notation conform RFC 3339. Other representations of DateTimeOffset values are not supported.
        /// </remarks>
        internal static DateTimeOffset ParseDateTimeOffset(string str, int len, ref int i, ParserContext _ = null)
        {
            //
            // NB: Only supporting ISO 8601 subset as defined below; no support for the "Microsoft format" (e.g. "\/Date(1234567890)\/")
            //

            /*
                RFC 3339       Date and Time on the Internet: Timestamps       July 2002


                5.6. Internet Date/Time Format


                The following profile of ISO 8601 [ISO8601] dates SHOULD be used in
                new protocols on the Internet.  This is specified using the syntax
                description notation defined in [ABNF].

                date-fullyear   = 4DIGIT
                date-month      = 2DIGIT  ; 01-12Test
                date-mday       = 2DIGIT  ; 01-28, 01-29, 01-30, 01-31 based on
                                          ; month/year
                time-hour       = 2DIGIT  ; 00-23
                time-minute     = 2DIGIT  ; 00-59
                time-second     = 2DIGIT  ; 00-58, 00-59, 00-60 based on leap second
                                          ; rules
                time-secfrac    = "." 1*DIGIT
                time-numoffset  = ("+" / "-") time-hour ":" time-minute
                time-offset     = "Z" / time-numoffset

                partial-time    = time-hour ":" time-minute ":" time-second
                                  [time-secfrac]
                full-date       = date-fullyear "-" date-month "-" date-mday
                full-time       = partial-time time-offset

                date-time       = full-date "T" full-time
            */

            var b = i;

            if (i >= len)
                throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected {0} at position '{1}' conform the ISO 8601 subset defined in RFC 3339. Unexpected string termination found at start of value.", nameof(DateTimeOffset), b), i, ParseError.PrematureEndOfInput);

            var c = str[i++];

            if (c != '\"')
                throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected {0} at position '{1}' conform the ISO 8601 subset defined in RFC 3339. Unexpected character '{2}' found when an RFC 3339 string starting with '\"' was expected.", nameof(DateTimeOffset), b, c), i, ParseError.UnexpectedToken);

            if (i + (4 /*YYYY*/ + 1 /*-*/ + 2 /*MM*/ + 1 /*-*/ + 2 /*DD*/ + 1 /*T*/ + 2 /*HH*/ + 1 /*:*/ + 2 /*MM*/ + 1 /*:*/ + 2 /*SS*/) > len)
                throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected {0} at position '{1}' conform the ISO 8601 subset defined in RFC 3339. The RFC 3339 string literal is too short to contain the mandatory 'yyyy-MM-ddTHH:mm:ss' characters.", nameof(DateTimeOffset), b), i, ParseError.PrematureEndOfInput);

            //
            // NB: Validation of ranges happens through the DateTimeOffset constructor.
            //

            ParseYearMonthDay(str, b, ref i, out int year, out int month, out int day);

            c = str[i++];

            if (c is not 'T' and not 't')
                throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected {0} at position '{1}' conform the ISO 8601 subset defined in RFC 3339. Unexpected character '{2}' found when 'T' or 't' date/time separator was expected.", nameof(DateTimeOffset), b, c), i, ParseError.UnexpectedToken);

            //
            // NB: Validation of ranges happens through the DateTimeOffset constructor.
            //

            ParseHourMinuteSecond(str, b, ref i, out int hour, out int minute, out int second);

            //
            // NB: Leap seconds are valid in ISO 8601 but not supported in .NET. The same restriction exists in other JSON frameworks. We could omit the check altogether here and rely on
            //     the DateTime[Offset] constructor throwing but choose to provide a more descriptive message instead.
            //
            // CONSIDER: Use the ParserContext to provide settings to deal with such anomalies, e.g. to specify a "rounding" behavior for leap seconds.
            //

            if (second == 60)
            {
                throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected {0} at position '{1}' conform the ISO 8601 subset defined in RFC 3339. A second value of 60 was found, which is not supported by {2} and {0}.", nameof(DateTimeOffset), b, nameof(DateTime)), i, ParseError.UnexpectedToken);
            }

            if (i >= len)
                throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected {0} at position '{1}' conform the ISO 8601 subset defined in RFC 3339. Unexpected string termination found when a second fraction or time offset was expected.", nameof(DateTimeOffset), b), i, ParseError.PrematureEndOfInput);

            double fraction = 0.0;

            c = str[i++];

            if (c == '.')
            {
                fraction = ParseFraction(str, len, b, ref i);

                if (i >= len)
                    throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected {0} at position '{1}' conform the ISO 8601 subset defined in RFC 3339. Unexpected string termination found after second fraction where a time offset was expected.", nameof(DateTimeOffset), b), i, ParseError.PrematureEndOfInput);

                c = str[i++];
            }

            if (c == 'Z')
            {
                if (i >= len)
                    throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected {0} at position '{1}' conform the ISO 8601 subset defined in RFC 3339. Unexpected string termination found after 'Z' time offset when '\"' was expected.", nameof(DateTimeOffset), b), i, ParseError.PrematureEndOfInput);

                if (str[i++] != '\"')
                    throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected {0} at position '{1}' conform the ISO 8601 subset defined in RFC 3339. Unexpected character '{2}' found after 'Z' time offset when '\"' was expected.", nameof(DateTimeOffset), b, c), i, ParseError.UnexpectedToken);

                var res = new DateTimeOffset(year, month, day, hour, minute, second, TimeSpan.Zero);
                res = res.AddTicks((long)Math.Round(fraction * 10000000.0 /*100ns*/));
                return res;
            }
            else if (c is '+' or '-')
            {
                var neg = c == '-';

                if (i + (2 /*HH*/ + 1 /*:*/ + 2 /*MM*/) > len)
                    throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected {0} at position '{1}' conform the ISO 8601 subset defined in RFC 3339. The remaining string literal is too short to contain the 'HH:mm' characters specifying a time offset.", nameof(DateTimeOffset), b), i, ParseError.PrematureEndOfInput);

                var offset = ParseOffset(str, b, ref i);

                if (neg)
                {
                    offset = -offset;
                }

                if (i >= len)
                    throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected {0} at position '{1}' conform the ISO 8601 subset defined in RFC 3339. Unexpected string termination found after time offset when '\"' was expected.", nameof(DateTimeOffset), b), i, ParseError.PrematureEndOfInput);

                if (str[i++] != '\"')
                    throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected {0} at position '{1}' conform the ISO 8601 subset defined in RFC 3339. Unexpected character '{2}' found after time offset when '\"' was expected.", nameof(DateTimeOffset), b, c), i, ParseError.UnexpectedToken);

                var res = new DateTimeOffset(year, month, day, hour, minute, second, offset);
                res = res.AddTicks((long)Math.Round(fraction * 10000000.0));
                return res;
            }
            else
            {
                throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected {0} at position '{1}' conform the ISO 8601 subset defined in RFC 3339. Unexpected character '{2}' found when a time offset was expected.", nameof(DateTimeOffset), b, c), i, ParseError.UnexpectedToken);
            }
        }

#if !NO_IO
        /// <summary>
        /// Lexes and parses a JSON String as a System.DateTimeOffset using ISO-8601 notation in the specified text reader.
        /// </summary>
        /// <param name="reader">The text reader to lex and parse a DateTimeOffset from.</param>
        /// <param name="_">The parser context.</param>
        /// <returns>A System.DateTimeOffset value, if found; otherwise, a ParseException is thrown.</returns>
        /// <remarks>
        /// This method does not consume whitespace; the character at the specified start index in the string is expected to be the start of a valid JSON String.
        /// This method expects a JSON String containing an ISO-8601 DateTime notation conform RFC 3339. Other representations of DateTimeOffset values are not supported.
        /// </remarks>
        internal static DateTimeOffset ParseDateTimeOffset(System.IO.TextReader reader, ParserContext _ = null)
        {
            //
            // NB: Only supporting ISO 8601 subset as defined below; no support for the "Microsoft format" (e.g. "\/Date(1234567890)\/")
            //

            /*
                RFC 3339       Date and Time on the Internet: Timestamps       July 2002


                5.6. Internet Date/Time Format


                The following profile of ISO 8601 [ISO8601] dates SHOULD be used in
                new protocols on the Internet.  This is specified using the syntax
                description notation defined in [ABNF].

                date-fullyear   = 4DIGIT
                date-month      = 2DIGIT  ; 01-12Test
                date-mday       = 2DIGIT  ; 01-28, 01-29, 01-30, 01-31 based on
                                          ; month/year
                time-hour       = 2DIGIT  ; 00-23
                time-minute     = 2DIGIT  ; 00-59
                time-second     = 2DIGIT  ; 00-58, 00-59, 00-60 based on leap second
                                          ; rules
                time-secfrac    = "." 1*DIGIT
                time-numoffset  = ("+" / "-") time-hour ":" time-minute
                time-offset     = "Z" / time-numoffset

                partial-time    = time-hour ":" time-minute ":" time-second
                                  [time-secfrac]
                full-date       = date-fullyear "-" date-month "-" date-mday
                full-time       = partial-time time-offset

                date-time       = full-date "T" full-time
            */

            var c = reader.Read();

            if (c != '\"')
                throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected {0} conform the ISO 8601 subset defined in RFC 3339. Unexpected character '{1}' found when an RFC 3339 string starting with '\"' was expected.", nameof(DateTimeOffset), c), -1, ParseError.UnexpectedToken);

            //
            // NB: Validation of ranges happens through the DateTimeOffset constructor.
            //

            ParseYearMonthDay(reader, out int year, out int month, out int day);

            c = reader.Read();

            if (c is not 'T' and not 't')
                throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected {0} conform the ISO 8601 subset defined in RFC 3339. Unexpected character '{1}' found when 'T' or 't' date/time separator was expected.", nameof(DateTimeOffset), c), -1, ParseError.UnexpectedToken);

            //
            // NB: Validation of ranges happens through the DateTimeOffset constructor.
            //

            ParseHourMinuteSecond(reader, out int hour, out int minute, out int second);

            //
            // NB: Leap seconds are valid in ISO 8601 but not supported in .NET. The same restriction exists in other JSON frameworks. We could omit the check altogether here and rely on
            //     the DateTime[Offset] constructor throwing but choose to provide a more descriptive message instead.
            //
            // CONSIDER: Use the ParserContext to provide settings to deal with such anomalies, e.g. to specify a "rounding" behavior for leap seconds.
            //

            if (second == 60)
            {
                throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected {0} conform the ISO 8601 subset defined in RFC 3339. A second value of 60 was found, which is not supported by {1} and {0}.", nameof(DateTimeOffset), nameof(DateTime)), -1, ParseError.UnexpectedToken);
            }

            double fraction = 0.0;

            c = reader.Read();

            if (c == '.')
            {
                fraction = ParseFraction(reader);

                c = reader.Read();
            }

            if (c == 'Z')
            {
                if (reader.Read() != '\"')
                    throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected {0} conform the ISO 8601 subset defined in RFC 3339. Unexpected character '{1}' found after 'Z' time offset when '\"' was expected.", nameof(DateTimeOffset), c), -1, ParseError.UnexpectedToken);

                var res = new DateTimeOffset(year, month, day, hour, minute, second, TimeSpan.Zero);
                res = res.AddTicks((long)Math.Round(fraction * 10000000.0 /*100ns*/));
                return res;
            }
            else if (c is '+' or '-')
            {
                var neg = c == '-';

                var offset = ParseOffset(reader);

                if (neg)
                {
                    offset = -offset;
                }

                if (reader.Read() != '\"')
                    throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected {0} conform the ISO 8601 subset defined in RFC 3339. Unexpected character '{1}' found after time offset when '\"' was expected.", nameof(DateTimeOffset), c), -1, ParseError.UnexpectedToken);

                var res = new DateTimeOffset(year, month, day, hour, minute, second, offset);
                res = res.AddTicks((long)Math.Round(fraction * 10000000.0));
                return res;
            }
            else
            {
                throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected {0} conform the ISO 8601 subset defined in RFC 3339. Unexpected character '{1}' found when a time offset was expected.", nameof(DateTimeOffset), c), -1, ParseError.UnexpectedToken);
            }
        }
#endif

        /// <summary>
        /// Parses a year, month, and day in ISO-8601 notation from the specified string at the specified index.
        /// </summary>
        /// <param name="str">The string to lex and parse a year, month, and day from.</param>
        /// <param name="b">The start index of the DateTime literal being parsed; used for error reporting.</param>
        /// <param name="i">The index in the string to start lexing from. This value gets updated to the first index position after the day, if found.</param>
        /// <param name="year">The year, if found.</param>
        /// <param name="month">The month, if found.</param>
        /// <param name="day">The day, if found.</param>
        /// <remarks>
        /// This method does not validate the year, month, and day to fall within expected ranges (besides disallowing negative values). It's the caller's responsibility to perform such validations.
        /// This method assumes at least 10 characters can be read from the specified string starting at the specified index. It's the caller's responsibility to ensure this is the case.
        /// </remarks>
        private static void ParseYearMonthDay(string str, int b, ref int i, out int year, out int month, out int day)
        {
            if (TryParseInt32(str, 4, ref i, out year) && str[i++] == '-' &&
                TryParseInt32(str, 2, ref i, out month) && str[i++] == '-' &&
                TryParseInt32(str, 2, ref i, out day))
            {
                return;
            }

            throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected {0} at position '{1}' conform the ISO 8601 subset defined in RFC 3339. Expected date specification of the form 'yyyy-MM-dd'.", nameof(DateTimeOffset), b), i, ParseError.UnexpectedToken);
        }

#if !NO_IO
        /// <summary>
        /// Parses a year, month, and day in ISO-8601 notation from the specified text reader.
        /// </summary>
        /// <param name="reader">The text reader to lex and parse a year, month, and day from.</param>
        /// <param name="year">The year, if found.</param>
        /// <param name="month">The month, if found.</param>
        /// <param name="day">The day, if found.</param>
        /// <remarks>
        /// This method does not validate the year, month, and day to fall within expected ranges (besides disallowing negative values). It's the caller's responsibility to perform such validations.
        /// This method assumes at least 10 characters can be read from the specified string starting at the specified index. It's the caller's responsibility to ensure this is the case.
        /// </remarks>
        private static void ParseYearMonthDay(System.IO.TextReader reader, out int year, out int month, out int day)
        {
            if (TryParseInt32(reader, 4, out year) && reader.Read() == '-' &&
                TryParseInt32(reader, 2, out month) && reader.Read() == '-' &&
                TryParseInt32(reader, 2, out day))
            {
                return;
            }

            throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected {0} conform the ISO 8601 subset defined in RFC 3339. Expected date specification of the form 'yyyy-MM-dd'.", nameof(DateTimeOffset)), -1, ParseError.UnexpectedToken);
        }
#endif

        /// <summary>
        /// Parses an hour, minute, and second in ISO-8601 notation from the specified string at the specified index.
        /// </summary>
        /// <param name="str">The string to lex and parse an hour, minute, and second from.</param>
        /// <param name="b">The start index of the DateTime literal being parsed; used for error reporting.</param>
        /// <param name="i">The index in the string to start lexing from. This value gets updated to the first index position after the second, if found.</param>
        /// <param name="hour">The hour, if found.</param>
        /// <param name="minute">The minute, if found.</param>
        /// <param name="second">The second, if found.</param>
        /// <remarks>
        /// This method does not validate the hour, minute, and second to fall within expected ranges (besides disallowing negative values). It's the caller's responsibility to perform such validations.
        /// This method assumes at least 8 characters can be read from the specified string starting at the specified index. It's the caller's responsibility to ensure this is the case.
        /// </remarks>
        private static void ParseHourMinuteSecond(string str, int b, ref int i, out int hour, out int minute, out int second)
        {
            if (TryParseInt32(str, 2, ref i, out hour) && str[i++] == ':' &&
                TryParseInt32(str, 2, ref i, out minute) && str[i++] == ':' &&
                TryParseInt32(str, 2, ref i, out second))
            {
                return;
            }

            throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected {0} at position '{1}' conform the ISO 8601 subset defined in RFC 3339. Expected time specification of the form 'HH:mm:ss'.", nameof(DateTimeOffset), b), i, ParseError.UnexpectedToken);
        }

#if !NO_IO
        /// <summary>
        /// Parses an hour, minute, and second in ISO-8601 notation from the specified text reader.
        /// </summary>
        /// <param name="reader">The text reader to lex and parse an hour, minute, and second from.</param>
        /// <param name="hour">The hour, if found.</param>
        /// <param name="minute">The minute, if found.</param>
        /// <param name="second">The second, if found.</param>
        /// <remarks>
        /// This method does not validate the hour, minute, and second to fall within expected ranges (besides disallowing negative values). It's the caller's responsibility to perform such validations.
        /// This method assumes at least 8 characters can be read from the specified string starting at the specified index. It's the caller's responsibility to ensure this is the case.
        /// </remarks>
        private static void ParseHourMinuteSecond(System.IO.TextReader reader, out int hour, out int minute, out int second)
        {
            if (TryParseInt32(reader, 2, out hour) && reader.Read() == ':' &&
                TryParseInt32(reader, 2, out minute) && reader.Read() == ':' &&
                TryParseInt32(reader, 2, out second))
            {
                return;
            }

            throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected {0} conform the ISO 8601 subset defined in RFC 3339. Expected time specification of the form 'HH:mm:ss'.", nameof(DateTimeOffset)), -1, ParseError.UnexpectedToken);
        }
#endif

        /// <summary>
        /// Parses a time zone offset in ISO-8601 notation from the specified string at the specified index.
        /// </summary>
        /// <param name="str">The string to lex and parse a time zone offset from.</param>
        /// <param name="b">The start index of the DateTime literal being parsed; used for error reporting.</param>
        /// <param name="i">The index in the string to start lexing from. This value gets updated to the first index position after the offset, if found.</param>
        /// <returns>The time zone offset expressed as a TimeSpan value.</returns>
        /// <remarks>
        /// This method validates the offset to be a valid TimeSpan.
        /// This method assumes at least 5 characters can be read from the specified string starting at the specified index. It's the caller's responsibility to ensure this is the case.
        /// </remarks>
        private static TimeSpan ParseOffset(string str, int b, ref int i)
        {
            if (TryParseInt32(str, 2, ref i, out int offHours) && str[i++] == ':' &&
                TryParseInt32(str, 2, ref i, out int offMinutes))
            {
                // TODO: validate ranges

                return new TimeSpan(offHours, offMinutes, 0);
            }

            throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected {0} at position '{1}' conform the ISO 8601 subset defined in RFC 3339. Expected time offset specification of the form 'HH:mm'.", nameof(DateTimeOffset), b), i, ParseError.UnexpectedToken);
        }

#if !NO_IO
        /// <summary>
        /// Parses a time zone offset in ISO-8601 notation from the specified text reader.
        /// </summary>
        /// <param name="reader">The text reader to lex and parse a time zone offset from.</param>
        /// <returns>The time zone offset expressed as a TimeSpan value.</returns>
        /// <remarks>
        /// This method validates the offset to be a valid TimeSpan.
        /// This method assumes at least 5 characters can be read from the specified string starting at the specified index. It's the caller's responsibility to ensure this is the case.
        /// </remarks>
        private static TimeSpan ParseOffset(System.IO.TextReader reader)
        {
            if (TryParseInt32(reader, 2, out int offHours) && reader.Read() == ':' &&
                TryParseInt32(reader, 2, out int offMinutes))
            {
                // TODO: validate ranges

                return new TimeSpan(offHours, offMinutes, 0);
            }

            throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected {0} conform the ISO 8601 subset defined in RFC 3339. Expected time offset specification of the form 'HH:mm'.", nameof(DateTimeOffset)), -1, ParseError.UnexpectedToken);
        }
#endif

        /// <summary>
        /// Parses a second fraction from the specified specified string at the specified index.
        /// </summary>
        /// <param name="str">The string to lex and parse a second fraction from.</param>
        /// <param name="len">The length of the string.</param>
        /// <param name="b">The start index of the DateTime literal being parsed; used for error reporting.</param>
        /// <param name="i">The index in the string to start lexing from, after the decimal point. This value gets updated to the first index position after the offset, if found.</param>
        /// <returns>The second fraction expressed as a Double value.</returns>
        private static double ParseFraction(string str, int len, int b, ref int i)
        {
            var res = 0.0;

            var factor = 0.1;
            var n = 0;

            while (i < len)
            {
                var c = str[i];

                if (c is < '0' or > '9')
                    break;

                i++;

                res += (c - '0') * factor;

                factor *= 0.1;
                n++;
            }

            if (n == 0)
                throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected {0} at position '{1}' conform the ISO 8601 subset defined in RFC 3339. Expected at least one digit for the second fraction after the '.' decimal point.", nameof(DateTimeOffset), b), i, ParseError.PrematureEndOfInput);

            return res;
        }

#if !NO_IO
        /// <summary>
        /// Parses a second fraction from the specified text reader.
        /// </summary>
        /// <param name="reader">The text reader to lex and parse a second fraction from.</param>
        /// <returns>The second fraction expressed as a Double value.</returns>
        private static double ParseFraction(System.IO.TextReader reader)
        {
            var res = 0.0;

            var factor = 0.1;
            var n = 0;

            int c;

            while ((c = reader.Peek()) >= 0)
            {
                if (c is < '0' or > '9')
                    break;

                reader.Read();

                res += (c - '0') * factor;

                factor *= 0.1;
                n++;
            }

            if (n == 0)
                throw new ParseException(string.Format(CultureInfo.InvariantCulture, "Expected {0} conform the ISO 8601 subset defined in RFC 3339. Expected at least one digit for the second fraction after the '.' decimal point.", nameof(DateTimeOffset)), -1, ParseError.PrematureEndOfInput);

            return res;
        }
#endif
    }
}
