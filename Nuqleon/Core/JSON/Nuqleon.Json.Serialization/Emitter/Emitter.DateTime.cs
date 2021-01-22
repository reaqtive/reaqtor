// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 04/16/2016 - Created fast JSON serializer functionality.
//   BD - 05/08/2016 - Added support for serialization to text writers.
//

using System;
using System.Diagnostics;
using System.Text;

namespace Nuqleon.Json.Serialization
{
    internal partial class Emitter
    {
        /// <summary>
        /// Emits a System.DateTime as a JSON String to the specified string builder using ISO-8601 notation.
        /// </summary>
        /// <param name="builder">The string builder to append the DateTime to.</param>
        /// <param name="value">The DateTime value to emit.</param>
        /// <param name="_">The emitter context.</param>
        internal static void EmitDateTime(StringBuilder builder, DateTime value, EmitterContext _ = null) => EmitDateTimeOffset(builder, value, _);

#if !NO_IO
        /// <summary>
        /// Emits a System.DateTime as a JSON String to the specified text writer using ISO-8601 notation.
        /// </summary>
        /// <param name="writer">The text writer to append the DateTime to.</param>
        /// <param name="value">The DateTime value to emit.</param>
        /// <param name="_">The emitter context.</param>
        internal static void EmitDateTime(System.IO.TextWriter writer, DateTime value, EmitterContext _ = null) => EmitDateTimeOffset(writer, value, _);
#endif

        /// <summary>
        /// Emits a System.DateTimeOffset as a JSON String to the specified string builder using ISO-8601 notation.
        /// </summary>
        /// <param name="builder">The string builder to append the DateTimeOffset to.</param>
        /// <param name="value">The DateTimeOffset value to emit.</param>
        /// <param name="_">The emitter context.</param>
        internal static void EmitDateTimeOffset(StringBuilder builder, DateTimeOffset value, EmitterContext _ = null)
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

            builder.Append('\"');

            EmitInt32PadFour(builder, value.Year);
            builder.Append('-');
            EmitInt32PadTwo(builder, value.Month);
            builder.Append('-');
            EmitInt32PadTwo(builder, value.Day);

            builder.Append('T');

            EmitInt32PadTwo(builder, value.Hour);
            builder.Append(':');
            EmitInt32PadTwo(builder, value.Minute);
            builder.Append(':');
            EmitInt32PadTwo(builder, value.Second);

            var ticks = value.Ticks;
            var fraction = (int)(ticks % 10000000 /*100ns*/);

            if (fraction != 0)
            {
                builder.Append('.');

                for (var f = 1000000; fraction > 0; f /= 10)
                {
                    var div = Math.DivRem(fraction, f, out fraction);
                    builder.Append((char)('0' + div));
                }
            }

            var offset = value.Offset;

            if (offset == TimeSpan.Zero)
            {
                builder.Append('Z');
            }
            else
            {
                if (offset < TimeSpan.Zero)
                {
                    builder.Append('-');
                    offset = -offset;
                }
                else
                {
                    builder.Append('+');
                }

                EmitInt32PadTwo(builder, offset.Hours);
                builder.Append(':');
                EmitInt32PadTwo(builder, offset.Minutes);
            }

            builder.Append('\"');
        }

#if !NO_IO
        /// <summary>
        /// Emits a System.DateTimeOffset as a JSON String to the specified text writer using ISO-8601 notation.
        /// </summary>
        /// <param name="writer">The text writer to append the DateTimeOffset to.</param>
        /// <param name="value">The DateTimeOffset value to emit.</param>
        /// <param name="_">The emitter context.</param>
        internal static void EmitDateTimeOffset(System.IO.TextWriter writer, DateTimeOffset value, EmitterContext _ = null)
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

            writer.Write('\"');

            EmitInt32PadFour(writer, value.Year);
            writer.Write('-');
            EmitInt32PadTwo(writer, value.Month);
            writer.Write('-');
            EmitInt32PadTwo(writer, value.Day);

            writer.Write('T');

            EmitInt32PadTwo(writer, value.Hour);
            writer.Write(':');
            EmitInt32PadTwo(writer, value.Minute);
            writer.Write(':');
            EmitInt32PadTwo(writer, value.Second);

            var ticks = value.Ticks;
            var fraction = (int)(ticks % 10000000 /*100ns*/);

            if (fraction != 0)
            {
                writer.Write('.');

                for (var f = 1000000; fraction > 0; f /= 10)
                {
                    var div = Math.DivRem(fraction, f, out fraction);
                    writer.Write((char)('0' + div));
                }
            }

            var offset = value.Offset;

            if (offset == TimeSpan.Zero)
            {
                writer.Write('Z');
            }
            else
            {
                if (offset < TimeSpan.Zero)
                {
                    writer.Write('-');
                    offset = -offset;
                }
                else
                {
                    writer.Write('+');
                }

                EmitInt32PadTwo(writer, offset.Hours);
                writer.Write(':');
                EmitInt32PadTwo(writer, offset.Minutes);
            }

            writer.Write('\"');
        }
#endif

        /// <summary>
        /// Emits the four digit decimal representation of the specified integer <paramref name="value"/> to the specified string <paramref name="builder"/>.
        /// </summary>
        /// <param name="builder">The string builder to append the four digit decimal representation of the integer value to.</param>
        /// <param name="value">The integer value whose decimal representation to append to the builder.</param>
        /// <remarks>
        /// If the decimal representation of the character is shorter than four decimal digits, leading '0' characters will be added to pad to a total length of four.
        /// </remarks>
        private static void EmitInt32PadFour(StringBuilder builder, int value)
        {
            Debug.Assert(value is >= 0 and <= 9999);

            int div = Math.DivRem(value, 1000, out value);
            builder.Append((char)('0' + div));

            div = Math.DivRem(value, 100, out value);
            builder.Append((char)('0' + div));

            div = Math.DivRem(value, 10, out value);
            builder.Append((char)('0' + div));
            builder.Append((char)('0' + value));
        }

#if !NO_IO
        /// <summary>
        /// Emits the four digit decimal representation of the specified integer <paramref name="value"/> to the specified text <paramref name="writer"/>.
        /// </summary>
        /// <param name="writer">The text writer to append the four digit decimal representation of the integer value to.</param>
        /// <param name="value">The integer value whose decimal representation to append to the text writer.</param>
        /// <remarks>
        /// If the decimal representation of the character is shorter than four decimal digits, leading '0' characters will be added to pad to a total length of four.
        /// </remarks>
        private static void EmitInt32PadFour(System.IO.TextWriter writer, int value)
        {
            Debug.Assert(value is >= 0 and <= 9999);

            int div = Math.DivRem(value, 1000, out value);
            writer.Write((char)('0' + div));

            div = Math.DivRem(value, 100, out value);
            writer.Write((char)('0' + div));

            div = Math.DivRem(value, 10, out value);
            writer.Write((char)('0' + div));
            writer.Write((char)('0' + value));
        }
#endif

        /// <summary>
        /// Emits the two digit decimal representation of the specified integer <paramref name="value"/> to the specified string <paramref name="builder"/>.
        /// </summary>
        /// <param name="builder">The string builder to append the two digit decimal representation of the integer value to.</param>
        /// <param name="value">The integer value whose decimal representation to append to the builder.</param>
        /// <remarks>
        /// If the decimal representation of the character is shorter than two decimal digits, leading '0' characters will be added to pad to a total length of two.
        /// </remarks>
        private static void EmitInt32PadTwo(StringBuilder builder, int value)
        {
            Debug.Assert(value is >= 0 and <= 99);

            var div = Math.DivRem(value, 10, out value);
            builder.Append((char)('0' + div));
            builder.Append((char)('0' + value));
        }

#if !NO_IO
        /// <summary>
        /// Emits the two digit decimal representation of the specified integer <paramref name="value"/> to the specified text <paramref name="writer"/>.
        /// </summary>
        /// <param name="writer">The text writer to append the two digit decimal representation of the integer value to.</param>
        /// <param name="value">The integer value whose decimal representation to append to the text writer.</param>
        /// <remarks>
        /// If the decimal representation of the character is shorter than two decimal digits, leading '0' characters will be added to pad to a total length of two.
        /// </remarks>
        private static void EmitInt32PadTwo(System.IO.TextWriter writer, int value)
        {
            Debug.Assert(value is >= 0 and <= 99);

            var div = Math.DivRem(value, 10, out value);
            writer.Write((char)('0' + div));
            writer.Write((char)('0' + value));
        }
#endif
    }
}
