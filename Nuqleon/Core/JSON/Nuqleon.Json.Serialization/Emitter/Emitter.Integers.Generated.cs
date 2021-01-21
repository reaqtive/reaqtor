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
using System.Text;

namespace Nuqleon.Json.Serialization
{
    internal partial class Emitter
    {
        //
        // NB: The use of unsafe code for `itoa` type functionality here pays off mostly on x64 targets where
        //     printing long.MaxValue is about 40% faster compared to the naive approach of using Append(long)
        //     which causes a ToString and allocations. When using x86, we see gains for numbers with up to 11
        //     digits. This is enough to make all 32-bit numbers faster to print, but has up to 60% regression
        //     for 64-bit numbers of 12 digits and more. For the time being, we won't worry about the x86 case
        //     with big numbers (which could be deemed rare) and focus on server workloads.
        //
        //     For the safe code variants we see up to 40% improvement for x64 with a similar regression on x86
        //     with numbers containing 12 or more digits.
        //

        /// <summary>
        /// Emits a System.SByte as a JSON Number to the specified string builder.
        /// </summary>
        /// <param name="builder">The string builder to append the System.SByte to.</param>
        /// <param name="value">The System.SByte value to emit.</param>
        /// <param name="context">The emitter context.</param>
        internal static void EmitSByte(StringBuilder builder, SByte value, EmitterContext context)
        {
            switch (value)
            {
                case SByte.MinValue:
                    builder.Append("-128");
                    return;
                case -1:
                    builder.Append("-1");
                    return;
                case 0:
                    builder.Append('0');
                    return;
                case 1:
                    builder.Append('1');
                    return;
                case 2:
                    builder.Append('2');
                    return;
                case 3:
                    builder.Append('3');
                    return;
                case 4:
                    builder.Append('4');
                    return;
                case 5:
                    builder.Append('5');
                    return;
                case 6:
                    builder.Append('6');
                    return;
                case 7:
                    builder.Append('7');
                    return;
            }

            if (value < 0)
            {
                builder.Append('-');
                value = (SByte)(-value);
            }

#if ALLOW_UNSAFE && HAS_APPEND_CHARSTAR
            unsafe
            {
                const int len = 3; // "127".Length

                var str = stackalloc char[len];

                var i = 0;
                while (value != 0)
                {
                    //
                    // NB: Keeping the modulo and division operation adjacent to each other is similar to Math.DivRem which allows for
                    //     the JIT to emit a single idiv instruction rather than two.
                    //

                    var rem = value % 10;
                    value /= 10;

                    str[i] = (char)('0' + rem);
                    i++;
                }

                var n = i;

                for (int b = 0, e = n - 1; b < e; b++, e--)
                {
                    var t = str[b];
                    str[b] = str[e];
                    str[e] = t;
                }

                builder.Append(str, n);
            }
#else
            var str = context.IntegerDigitBuffer;

            var i = 0;
            while (value != 0)
            {
                //
                // NB: Keeping the modulo and division operation adjacent to each other is similar to Math.DivRem which allows for
                //     the JIT to emit a single idiv instruction rather than two.
                //

                var rem = value % 10;
                value /= 10;

                str[i] = (char)('0' + rem);
                i++;
            }

            var n = i;

            for (int b = 0, e = n - 1; b < e; b++, e--)
            {
                var t = str[b];
                str[b] = str[e];
                str[e] = t;
            }

            builder.Append(str, 0, n);
#endif
        }

#if !NO_IO
        private static readonly char[] s_minValueSByte = "-128".ToCharArray();

        /// <summary>
        /// Emits a System.SByte as a JSON Number to the specified text writer.
        /// </summary>
        /// <param name="writer">The text writer to append the System.SByte to.</param>
        /// <param name="value">The System.SByte value to emit.</param>
        /// <param name="context">The emitter context.</param>
        internal static void EmitSByte(System.IO.TextWriter writer, SByte value, EmitterContext context)
        {
            switch (value)
            {
                case SByte.MinValue:
                    writer.Write(s_minValueSByte);
                    return;
                case -1:
                    writer.Write('-');
                    writer.Write('1');
                    return;
                case 0:
                    writer.Write('0');
                    return;
                case 1:
                    writer.Write('1');
                    return;
                case 2:
                    writer.Write('2');
                    return;
                case 3:
                    writer.Write('3');
                    return;
                case 4:
                    writer.Write('4');
                    return;
                case 5:
                    writer.Write('5');
                    return;
                case 6:
                    writer.Write('6');
                    return;
                case 7:
                    writer.Write('7');
                    return;
            }

            if (value < 0)
            {
                writer.Write('-');
                value = (SByte)(-value);
            }

            var str = context.IntegerDigitBuffer;

            var i = 0;
            while (value != 0)
            {
                //
                // NB: Keeping the modulo and division operation adjacent to each other is similar to Math.DivRem which allows for
                //     the JIT to emit a single idiv instruction rather than two.
                //

                var rem = value % 10;
                value /= 10;

                str[i] = (char)('0' + rem);
                i++;
            }

            var n = i;

            for (int b = 0, e = n - 1; b < e; b++, e--)
            {
                var t = str[b];
                str[b] = str[e];
                str[e] = t;
            }

            writer.Write(str, 0, n);
        }
#endif

        /// <summary>
        /// Emits a System.Int16 as a JSON Number to the specified string builder.
        /// </summary>
        /// <param name="builder">The string builder to append the System.Int16 to.</param>
        /// <param name="value">The System.Int16 value to emit.</param>
        /// <param name="context">The emitter context.</param>
        internal static void EmitInt16(StringBuilder builder, Int16 value, EmitterContext context)
        {
            switch (value)
            {
                case Int16.MinValue:
                    builder.Append("-32768");
                    return;
                case -1:
                    builder.Append("-1");
                    return;
                case 0:
                    builder.Append('0');
                    return;
                case 1:
                    builder.Append('1');
                    return;
                case 2:
                    builder.Append('2');
                    return;
                case 3:
                    builder.Append('3');
                    return;
                case 4:
                    builder.Append('4');
                    return;
                case 5:
                    builder.Append('5');
                    return;
                case 6:
                    builder.Append('6');
                    return;
                case 7:
                    builder.Append('7');
                    return;
            }

            if (value < 0)
            {
                builder.Append('-');
                value = (Int16)(-value);
            }

#if ALLOW_UNSAFE && HAS_APPEND_CHARSTAR
            unsafe
            {
                const int len = 5; // "32767".Length

                var str = stackalloc char[len];

                var i = 0;
                while (value != 0)
                {
                    //
                    // NB: Keeping the modulo and division operation adjacent to each other is similar to Math.DivRem which allows for
                    //     the JIT to emit a single idiv instruction rather than two.
                    //

                    var rem = value % 10;
                    value /= 10;

                    str[i] = (char)('0' + rem);
                    i++;
                }

                var n = i;

                for (int b = 0, e = n - 1; b < e; b++, e--)
                {
                    var t = str[b];
                    str[b] = str[e];
                    str[e] = t;
                }

                builder.Append(str, n);
            }
#else
            var str = context.IntegerDigitBuffer;

            var i = 0;
            while (value != 0)
            {
                //
                // NB: Keeping the modulo and division operation adjacent to each other is similar to Math.DivRem which allows for
                //     the JIT to emit a single idiv instruction rather than two.
                //

                var rem = value % 10;
                value /= 10;

                str[i] = (char)('0' + rem);
                i++;
            }

            var n = i;

            for (int b = 0, e = n - 1; b < e; b++, e--)
            {
                var t = str[b];
                str[b] = str[e];
                str[e] = t;
            }

            builder.Append(str, 0, n);
#endif
        }

#if !NO_IO
        private static readonly char[] s_minValueInt16 = "-32768".ToCharArray();

        /// <summary>
        /// Emits a System.Int16 as a JSON Number to the specified text writer.
        /// </summary>
        /// <param name="writer">The text writer to append the System.Int16 to.</param>
        /// <param name="value">The System.Int16 value to emit.</param>
        /// <param name="context">The emitter context.</param>
        internal static void EmitInt16(System.IO.TextWriter writer, Int16 value, EmitterContext context)
        {
            switch (value)
            {
                case Int16.MinValue:
                    writer.Write(s_minValueInt16);
                    return;
                case -1:
                    writer.Write('-');
                    writer.Write('1');
                    return;
                case 0:
                    writer.Write('0');
                    return;
                case 1:
                    writer.Write('1');
                    return;
                case 2:
                    writer.Write('2');
                    return;
                case 3:
                    writer.Write('3');
                    return;
                case 4:
                    writer.Write('4');
                    return;
                case 5:
                    writer.Write('5');
                    return;
                case 6:
                    writer.Write('6');
                    return;
                case 7:
                    writer.Write('7');
                    return;
            }

            if (value < 0)
            {
                writer.Write('-');
                value = (Int16)(-value);
            }

            var str = context.IntegerDigitBuffer;

            var i = 0;
            while (value != 0)
            {
                //
                // NB: Keeping the modulo and division operation adjacent to each other is similar to Math.DivRem which allows for
                //     the JIT to emit a single idiv instruction rather than two.
                //

                var rem = value % 10;
                value /= 10;

                str[i] = (char)('0' + rem);
                i++;
            }

            var n = i;

            for (int b = 0, e = n - 1; b < e; b++, e--)
            {
                var t = str[b];
                str[b] = str[e];
                str[e] = t;
            }

            writer.Write(str, 0, n);
        }
#endif

        /// <summary>
        /// Emits a System.Int32 as a JSON Number to the specified string builder.
        /// </summary>
        /// <param name="builder">The string builder to append the System.Int32 to.</param>
        /// <param name="value">The System.Int32 value to emit.</param>
        /// <param name="context">The emitter context.</param>
        internal static void EmitInt32(StringBuilder builder, Int32 value, EmitterContext context)
        {
            switch (value)
            {
                case Int32.MinValue:
                    builder.Append("-2147483648");
                    return;
                case -1:
                    builder.Append("-1");
                    return;
                case 0:
                    builder.Append('0');
                    return;
                case 1:
                    builder.Append('1');
                    return;
                case 2:
                    builder.Append('2');
                    return;
                case 3:
                    builder.Append('3');
                    return;
                case 4:
                    builder.Append('4');
                    return;
                case 5:
                    builder.Append('5');
                    return;
                case 6:
                    builder.Append('6');
                    return;
                case 7:
                    builder.Append('7');
                    return;
            }

            if (value < 0)
            {
                builder.Append('-');
                value = (Int32)(-value);
            }

#if ALLOW_UNSAFE && HAS_APPEND_CHARSTAR
            unsafe
            {
                const int len = 10; // "2147483647".Length

                var str = stackalloc char[len];

                var i = 0;
                while (value != 0)
                {
                    //
                    // NB: Use of Math.DivRem allows for the JIT to emit a single idiv instruction rather than two.
                    //

                    var rem = default(System.Int32);
                    value = Math.DivRem(value, 10, out rem);

                    str[i] = (char)('0' + rem);
                    i++;
                }

                var n = i;

                for (int b = 0, e = n - 1; b < e; b++, e--)
                {
                    var t = str[b];
                    str[b] = str[e];
                    str[e] = t;
                }

                builder.Append(str, n);
            }
#else
            var str = context.IntegerDigitBuffer;

            var i = 0;
            while (value != 0)
            {
                //
                // NB: Use of Math.DivRem allows for the JIT to emit a single idiv instruction rather than two.
                //

                var rem = default(System.Int32);
                value = Math.DivRem(value, 10, out rem);

                str[i] = (char)('0' + rem);
                i++;
            }

            var n = i;

            for (int b = 0, e = n - 1; b < e; b++, e--)
            {
                var t = str[b];
                str[b] = str[e];
                str[e] = t;
            }

            builder.Append(str, 0, n);
#endif
        }

#if !NO_IO
        private static readonly char[] s_minValueInt32 = "-2147483648".ToCharArray();

        /// <summary>
        /// Emits a System.Int32 as a JSON Number to the specified text writer.
        /// </summary>
        /// <param name="writer">The text writer to append the System.Int32 to.</param>
        /// <param name="value">The System.Int32 value to emit.</param>
        /// <param name="context">The emitter context.</param>
        internal static void EmitInt32(System.IO.TextWriter writer, Int32 value, EmitterContext context)
        {
            switch (value)
            {
                case Int32.MinValue:
                    writer.Write(s_minValueInt32);
                    return;
                case -1:
                    writer.Write('-');
                    writer.Write('1');
                    return;
                case 0:
                    writer.Write('0');
                    return;
                case 1:
                    writer.Write('1');
                    return;
                case 2:
                    writer.Write('2');
                    return;
                case 3:
                    writer.Write('3');
                    return;
                case 4:
                    writer.Write('4');
                    return;
                case 5:
                    writer.Write('5');
                    return;
                case 6:
                    writer.Write('6');
                    return;
                case 7:
                    writer.Write('7');
                    return;
            }

            if (value < 0)
            {
                writer.Write('-');
                value = (Int32)(-value);
            }

            var str = context.IntegerDigitBuffer;

            var i = 0;
            while (value != 0)
            {
                //
                // NB: Use of Math.DivRem allows for the JIT to emit a single idiv instruction rather than two.
                //

                var rem = default(System.Int32);
                value = Math.DivRem(value, 10, out rem);

                str[i] = (char)('0' + rem);
                i++;
            }

            var n = i;

            for (int b = 0, e = n - 1; b < e; b++, e--)
            {
                var t = str[b];
                str[b] = str[e];
                str[e] = t;
            }

            writer.Write(str, 0, n);
        }
#endif

        /// <summary>
        /// Emits a System.Int64 as a JSON Number to the specified string builder.
        /// </summary>
        /// <param name="builder">The string builder to append the System.Int64 to.</param>
        /// <param name="value">The System.Int64 value to emit.</param>
        /// <param name="context">The emitter context.</param>
        internal static void EmitInt64(StringBuilder builder, Int64 value, EmitterContext context)
        {
            switch (value)
            {
                case Int64.MinValue:
                    builder.Append("-9223372036854775808");
                    return;
                case -1:
                    builder.Append("-1");
                    return;
                case 0:
                    builder.Append('0');
                    return;
                case 1:
                    builder.Append('1');
                    return;
                case 2:
                    builder.Append('2');
                    return;
                case 3:
                    builder.Append('3');
                    return;
                case 4:
                    builder.Append('4');
                    return;
                case 5:
                    builder.Append('5');
                    return;
                case 6:
                    builder.Append('6');
                    return;
                case 7:
                    builder.Append('7');
                    return;
            }

            if (value < 0)
            {
                builder.Append('-');
                value = (Int64)(-value);
            }

#if ALLOW_UNSAFE && HAS_APPEND_CHARSTAR
            unsafe
            {
                const int len = 19; // "9223372036854775807".Length

                var str = stackalloc char[len];

                var i = 0;
                while (value != 0)
                {
                    //
                    // NB: Use of Math.DivRem allows for the JIT to emit a single idiv instruction rather than two.
                    //

                    var rem = default(System.Int64);
                    value = Math.DivRem(value, 10, out rem);

                    str[i] = (char)('0' + rem);
                    i++;
                }

                var n = i;

                for (int b = 0, e = n - 1; b < e; b++, e--)
                {
                    var t = str[b];
                    str[b] = str[e];
                    str[e] = t;
                }

                builder.Append(str, n);
            }
#else
            var str = context.IntegerDigitBuffer;

            var i = 0;
            while (value != 0)
            {
                //
                // NB: Use of Math.DivRem allows for the JIT to emit a single idiv instruction rather than two.
                //

                var rem = default(System.Int64);
                value = Math.DivRem(value, 10, out rem);

                str[i] = (char)('0' + rem);
                i++;
            }

            var n = i;

            for (int b = 0, e = n - 1; b < e; b++, e--)
            {
                var t = str[b];
                str[b] = str[e];
                str[e] = t;
            }

            builder.Append(str, 0, n);
#endif
        }

#if !NO_IO
        private static readonly char[] s_minValueInt64 = "-9223372036854775808".ToCharArray();

        /// <summary>
        /// Emits a System.Int64 as a JSON Number to the specified text writer.
        /// </summary>
        /// <param name="writer">The text writer to append the System.Int64 to.</param>
        /// <param name="value">The System.Int64 value to emit.</param>
        /// <param name="context">The emitter context.</param>
        internal static void EmitInt64(System.IO.TextWriter writer, Int64 value, EmitterContext context)
        {
            switch (value)
            {
                case Int64.MinValue:
                    writer.Write(s_minValueInt64);
                    return;
                case -1:
                    writer.Write('-');
                    writer.Write('1');
                    return;
                case 0:
                    writer.Write('0');
                    return;
                case 1:
                    writer.Write('1');
                    return;
                case 2:
                    writer.Write('2');
                    return;
                case 3:
                    writer.Write('3');
                    return;
                case 4:
                    writer.Write('4');
                    return;
                case 5:
                    writer.Write('5');
                    return;
                case 6:
                    writer.Write('6');
                    return;
                case 7:
                    writer.Write('7');
                    return;
            }

            if (value < 0)
            {
                writer.Write('-');
                value = (Int64)(-value);
            }

            var str = context.IntegerDigitBuffer;

            var i = 0;
            while (value != 0)
            {
                //
                // NB: Use of Math.DivRem allows for the JIT to emit a single idiv instruction rather than two.
                //

                var rem = default(System.Int64);
                value = Math.DivRem(value, 10, out rem);

                str[i] = (char)('0' + rem);
                i++;
            }

            var n = i;

            for (int b = 0, e = n - 1; b < e; b++, e--)
            {
                var t = str[b];
                str[b] = str[e];
                str[e] = t;
            }

            writer.Write(str, 0, n);
        }
#endif

        /// <summary>
        /// Emits a System.Byte as a JSON Number to the specified string builder.
        /// </summary>
        /// <param name="builder">The string builder to append the System.Byte to.</param>
        /// <param name="value">The System.Byte value to emit.</param>
        /// <param name="context">The emitter context.</param>
        internal static void EmitByte(StringBuilder builder, Byte value, EmitterContext context)
        {
            switch (value)
            {
                case 0:
                    builder.Append('0');
                    return;
                case 1:
                    builder.Append('1');
                    return;
                case 2:
                    builder.Append('2');
                    return;
                case 3:
                    builder.Append('3');
                    return;
                case 4:
                    builder.Append('4');
                    return;
                case 5:
                    builder.Append('5');
                    return;
                case 6:
                    builder.Append('6');
                    return;
                case 7:
                    builder.Append('7');
                    return;
            }


#if ALLOW_UNSAFE && HAS_APPEND_CHARSTAR
            unsafe
            {
                const int len = 3; // "255".Length

                var str = stackalloc char[len];

                var i = 0;
                while (value != 0)
                {
                    //
                    // NB: Keeping the modulo and division operation adjacent to each other is similar to Math.DivRem which allows for
                    //     the JIT to emit a single idiv instruction rather than two.
                    //

                    var rem = value % 10;
                    value /= 10;

                    str[i] = (char)('0' + rem);
                    i++;
                }

                var n = i;

                for (int b = 0, e = n - 1; b < e; b++, e--)
                {
                    var t = str[b];
                    str[b] = str[e];
                    str[e] = t;
                }

                builder.Append(str, n);
            }
#else
            var str = context.IntegerDigitBuffer;

            var i = 0;
            while (value != 0)
            {
                //
                // NB: Keeping the modulo and division operation adjacent to each other is similar to Math.DivRem which allows for
                //     the JIT to emit a single idiv instruction rather than two.
                //

                var rem = value % 10;
                value /= 10;

                str[i] = (char)('0' + rem);
                i++;
            }

            var n = i;

            for (int b = 0, e = n - 1; b < e; b++, e--)
            {
                var t = str[b];
                str[b] = str[e];
                str[e] = t;
            }

            builder.Append(str, 0, n);
#endif
        }

#if !NO_IO
        /// <summary>
        /// Emits a System.Byte as a JSON Number to the specified text writer.
        /// </summary>
        /// <param name="writer">The text writer to append the System.Byte to.</param>
        /// <param name="value">The System.Byte value to emit.</param>
        /// <param name="context">The emitter context.</param>
        internal static void EmitByte(System.IO.TextWriter writer, Byte value, EmitterContext context)
        {
            switch (value)
            {
                case 0:
                    writer.Write('0');
                    return;
                case 1:
                    writer.Write('1');
                    return;
                case 2:
                    writer.Write('2');
                    return;
                case 3:
                    writer.Write('3');
                    return;
                case 4:
                    writer.Write('4');
                    return;
                case 5:
                    writer.Write('5');
                    return;
                case 6:
                    writer.Write('6');
                    return;
                case 7:
                    writer.Write('7');
                    return;
            }

            var str = context.IntegerDigitBuffer;

            var i = 0;
            while (value != 0)
            {
                //
                // NB: Keeping the modulo and division operation adjacent to each other is similar to Math.DivRem which allows for
                //     the JIT to emit a single idiv instruction rather than two.
                //

                var rem = value % 10;
                value /= 10;

                str[i] = (char)('0' + rem);
                i++;
            }

            var n = i;

            for (int b = 0, e = n - 1; b < e; b++, e--)
            {
                var t = str[b];
                str[b] = str[e];
                str[e] = t;
            }

            writer.Write(str, 0, n);
        }
#endif

        /// <summary>
        /// Emits a System.UInt16 as a JSON Number to the specified string builder.
        /// </summary>
        /// <param name="builder">The string builder to append the System.UInt16 to.</param>
        /// <param name="value">The System.UInt16 value to emit.</param>
        /// <param name="context">The emitter context.</param>
        internal static void EmitUInt16(StringBuilder builder, UInt16 value, EmitterContext context)
        {
            switch (value)
            {
                case 0:
                    builder.Append('0');
                    return;
                case 1:
                    builder.Append('1');
                    return;
                case 2:
                    builder.Append('2');
                    return;
                case 3:
                    builder.Append('3');
                    return;
                case 4:
                    builder.Append('4');
                    return;
                case 5:
                    builder.Append('5');
                    return;
                case 6:
                    builder.Append('6');
                    return;
                case 7:
                    builder.Append('7');
                    return;
            }


#if ALLOW_UNSAFE && HAS_APPEND_CHARSTAR
            unsafe
            {
                const int len = 5; // "65535".Length

                var str = stackalloc char[len];

                var i = 0;
                while (value != 0)
                {
                    //
                    // NB: Keeping the modulo and division operation adjacent to each other is similar to Math.DivRem which allows for
                    //     the JIT to emit a single idiv instruction rather than two.
                    //

                    var rem = value % 10;
                    value /= 10;

                    str[i] = (char)('0' + rem);
                    i++;
                }

                var n = i;

                for (int b = 0, e = n - 1; b < e; b++, e--)
                {
                    var t = str[b];
                    str[b] = str[e];
                    str[e] = t;
                }

                builder.Append(str, n);
            }
#else
            var str = context.IntegerDigitBuffer;

            var i = 0;
            while (value != 0)
            {
                //
                // NB: Keeping the modulo and division operation adjacent to each other is similar to Math.DivRem which allows for
                //     the JIT to emit a single idiv instruction rather than two.
                //

                var rem = value % 10;
                value /= 10;

                str[i] = (char)('0' + rem);
                i++;
            }

            var n = i;

            for (int b = 0, e = n - 1; b < e; b++, e--)
            {
                var t = str[b];
                str[b] = str[e];
                str[e] = t;
            }

            builder.Append(str, 0, n);
#endif
        }

#if !NO_IO
        /// <summary>
        /// Emits a System.UInt16 as a JSON Number to the specified text writer.
        /// </summary>
        /// <param name="writer">The text writer to append the System.UInt16 to.</param>
        /// <param name="value">The System.UInt16 value to emit.</param>
        /// <param name="context">The emitter context.</param>
        internal static void EmitUInt16(System.IO.TextWriter writer, UInt16 value, EmitterContext context)
        {
            switch (value)
            {
                case 0:
                    writer.Write('0');
                    return;
                case 1:
                    writer.Write('1');
                    return;
                case 2:
                    writer.Write('2');
                    return;
                case 3:
                    writer.Write('3');
                    return;
                case 4:
                    writer.Write('4');
                    return;
                case 5:
                    writer.Write('5');
                    return;
                case 6:
                    writer.Write('6');
                    return;
                case 7:
                    writer.Write('7');
                    return;
            }

            var str = context.IntegerDigitBuffer;

            var i = 0;
            while (value != 0)
            {
                //
                // NB: Keeping the modulo and division operation adjacent to each other is similar to Math.DivRem which allows for
                //     the JIT to emit a single idiv instruction rather than two.
                //

                var rem = value % 10;
                value /= 10;

                str[i] = (char)('0' + rem);
                i++;
            }

            var n = i;

            for (int b = 0, e = n - 1; b < e; b++, e--)
            {
                var t = str[b];
                str[b] = str[e];
                str[e] = t;
            }

            writer.Write(str, 0, n);
        }
#endif

        /// <summary>
        /// Emits a System.UInt32 as a JSON Number to the specified string builder.
        /// </summary>
        /// <param name="builder">The string builder to append the System.UInt32 to.</param>
        /// <param name="value">The System.UInt32 value to emit.</param>
        /// <param name="context">The emitter context.</param>
        internal static void EmitUInt32(StringBuilder builder, UInt32 value, EmitterContext context)
        {
            switch (value)
            {
                case 0:
                    builder.Append('0');
                    return;
                case 1:
                    builder.Append('1');
                    return;
                case 2:
                    builder.Append('2');
                    return;
                case 3:
                    builder.Append('3');
                    return;
                case 4:
                    builder.Append('4');
                    return;
                case 5:
                    builder.Append('5');
                    return;
                case 6:
                    builder.Append('6');
                    return;
                case 7:
                    builder.Append('7');
                    return;
            }


#if ALLOW_UNSAFE && HAS_APPEND_CHARSTAR
            unsafe
            {
                const int len = 10; // "4294967295".Length

                var str = stackalloc char[len];

                var i = 0;
                while (value != 0)
                {
                    //
                    // NB: Keeping the modulo and division operation adjacent to each other is similar to Math.DivRem which allows for
                    //     the JIT to emit a single idiv instruction rather than two.
                    //

                    var rem = value % 10;
                    value /= 10;

                    str[i] = (char)('0' + rem);
                    i++;
                }

                var n = i;

                for (int b = 0, e = n - 1; b < e; b++, e--)
                {
                    var t = str[b];
                    str[b] = str[e];
                    str[e] = t;
                }

                builder.Append(str, n);
            }
#else
            var str = context.IntegerDigitBuffer;

            var i = 0;
            while (value != 0)
            {
                //
                // NB: Keeping the modulo and division operation adjacent to each other is similar to Math.DivRem which allows for
                //     the JIT to emit a single idiv instruction rather than two.
                //

                var rem = value % 10;
                value /= 10;

                str[i] = (char)('0' + rem);
                i++;
            }

            var n = i;

            for (int b = 0, e = n - 1; b < e; b++, e--)
            {
                var t = str[b];
                str[b] = str[e];
                str[e] = t;
            }

            builder.Append(str, 0, n);
#endif
        }

#if !NO_IO
        /// <summary>
        /// Emits a System.UInt32 as a JSON Number to the specified text writer.
        /// </summary>
        /// <param name="writer">The text writer to append the System.UInt32 to.</param>
        /// <param name="value">The System.UInt32 value to emit.</param>
        /// <param name="context">The emitter context.</param>
        internal static void EmitUInt32(System.IO.TextWriter writer, UInt32 value, EmitterContext context)
        {
            switch (value)
            {
                case 0:
                    writer.Write('0');
                    return;
                case 1:
                    writer.Write('1');
                    return;
                case 2:
                    writer.Write('2');
                    return;
                case 3:
                    writer.Write('3');
                    return;
                case 4:
                    writer.Write('4');
                    return;
                case 5:
                    writer.Write('5');
                    return;
                case 6:
                    writer.Write('6');
                    return;
                case 7:
                    writer.Write('7');
                    return;
            }

            var str = context.IntegerDigitBuffer;

            var i = 0;
            while (value != 0)
            {
                //
                // NB: Keeping the modulo and division operation adjacent to each other is similar to Math.DivRem which allows for
                //     the JIT to emit a single idiv instruction rather than two.
                //

                var rem = value % 10;
                value /= 10;

                str[i] = (char)('0' + rem);
                i++;
            }

            var n = i;

            for (int b = 0, e = n - 1; b < e; b++, e--)
            {
                var t = str[b];
                str[b] = str[e];
                str[e] = t;
            }

            writer.Write(str, 0, n);
        }
#endif

        /// <summary>
        /// Emits a System.UInt64 as a JSON Number to the specified string builder.
        /// </summary>
        /// <param name="builder">The string builder to append the System.UInt64 to.</param>
        /// <param name="value">The System.UInt64 value to emit.</param>
        /// <param name="context">The emitter context.</param>
        internal static void EmitUInt64(StringBuilder builder, UInt64 value, EmitterContext context)
        {
            switch (value)
            {
                case 0:
                    builder.Append('0');
                    return;
                case 1:
                    builder.Append('1');
                    return;
                case 2:
                    builder.Append('2');
                    return;
                case 3:
                    builder.Append('3');
                    return;
                case 4:
                    builder.Append('4');
                    return;
                case 5:
                    builder.Append('5');
                    return;
                case 6:
                    builder.Append('6');
                    return;
                case 7:
                    builder.Append('7');
                    return;
            }


#if ALLOW_UNSAFE && HAS_APPEND_CHARSTAR
            unsafe
            {
                const int len = 20; // "18446744073709551615".Length

                var str = stackalloc char[len];

                var i = 0;
                while (value != 0)
                {
                    //
                    // NB: Keeping the modulo and division operation adjacent to each other is similar to Math.DivRem which allows for
                    //     the JIT to emit a single idiv instruction rather than two.
                    //

                    var rem = value % 10;
                    value /= 10;

                    str[i] = (char)('0' + rem);
                    i++;
                }

                var n = i;

                for (int b = 0, e = n - 1; b < e; b++, e--)
                {
                    var t = str[b];
                    str[b] = str[e];
                    str[e] = t;
                }

                builder.Append(str, n);
            }
#else
            var str = context.IntegerDigitBuffer;

            var i = 0;
            while (value != 0)
            {
                //
                // NB: Keeping the modulo and division operation adjacent to each other is similar to Math.DivRem which allows for
                //     the JIT to emit a single idiv instruction rather than two.
                //

                var rem = value % 10;
                value /= 10;

                str[i] = (char)('0' + rem);
                i++;
            }

            var n = i;

            for (int b = 0, e = n - 1; b < e; b++, e--)
            {
                var t = str[b];
                str[b] = str[e];
                str[e] = t;
            }

            builder.Append(str, 0, n);
#endif
        }

#if !NO_IO
        /// <summary>
        /// Emits a System.UInt64 as a JSON Number to the specified text writer.
        /// </summary>
        /// <param name="writer">The text writer to append the System.UInt64 to.</param>
        /// <param name="value">The System.UInt64 value to emit.</param>
        /// <param name="context">The emitter context.</param>
        internal static void EmitUInt64(System.IO.TextWriter writer, UInt64 value, EmitterContext context)
        {
            switch (value)
            {
                case 0:
                    writer.Write('0');
                    return;
                case 1:
                    writer.Write('1');
                    return;
                case 2:
                    writer.Write('2');
                    return;
                case 3:
                    writer.Write('3');
                    return;
                case 4:
                    writer.Write('4');
                    return;
                case 5:
                    writer.Write('5');
                    return;
                case 6:
                    writer.Write('6');
                    return;
                case 7:
                    writer.Write('7');
                    return;
            }

            var str = context.IntegerDigitBuffer;

            var i = 0;
            while (value != 0)
            {
                //
                // NB: Keeping the modulo and division operation adjacent to each other is similar to Math.DivRem which allows for
                //     the JIT to emit a single idiv instruction rather than two.
                //

                var rem = value % 10;
                value /= 10;

                str[i] = (char)('0' + rem);
                i++;
            }

            var n = i;

            for (int b = 0, e = n - 1; b < e; b++, e--)
            {
                var t = str[b];
                str[b] = str[e];
                str[e] = t;
            }

            writer.Write(str, 0, n);
        }
#endif

    }
}
