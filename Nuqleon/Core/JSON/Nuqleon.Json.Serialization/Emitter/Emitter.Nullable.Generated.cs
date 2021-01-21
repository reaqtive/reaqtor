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
        /// <summary>
        /// Emits a nullable System.SByte to the specified string builder.
        /// </summary>
        /// <param name="builder">The string builder to append the nullable System.SByte to.</param>
        /// <param name="value">The nullable System.SByte value to emit.</param>
        /// <param name="context">The emitter context.</param>
        internal static void EmitNullableSByte(StringBuilder builder, SByte? value, EmitterContext context)
        {
            if (value == null)
            {
                builder.Append("null");
            }
            else
            {
                EmitSByte(builder, value.Value, context);
            }
        }

#if !NO_IO
        /// <summary>
        /// Emits a nullable System.SByte to the specified text writer.
        /// </summary>
        /// <param name="writer">The text writer to append the nullable System.SByte to.</param>
        /// <param name="value">The nullable System.SByte value to emit.</param>
        /// <param name="context">The emitter context.</param>
        internal static void EmitNullableSByte(System.IO.TextWriter writer, SByte? value, EmitterContext context)
        {
            if (value == null)
            {
                //
                // CONSIDER: Add a flag to EmitterContext to reveal whether the TextWriter is one of the known writers
                //           which has an efficient implementation of Write(string) that doesn't call ToCharArray on
                //           the given string (see base class behavior of Write).
                //

                writer.Write("null");
            }
            else
            {
                EmitSByte(writer, value.Value, context);
            }
        }
#endif

        /// <summary>
        /// Emits a nullable System.Int16 to the specified string builder.
        /// </summary>
        /// <param name="builder">The string builder to append the nullable System.Int16 to.</param>
        /// <param name="value">The nullable System.Int16 value to emit.</param>
        /// <param name="context">The emitter context.</param>
        internal static void EmitNullableInt16(StringBuilder builder, Int16? value, EmitterContext context)
        {
            if (value == null)
            {
                builder.Append("null");
            }
            else
            {
                EmitInt16(builder, value.Value, context);
            }
        }

#if !NO_IO
        /// <summary>
        /// Emits a nullable System.Int16 to the specified text writer.
        /// </summary>
        /// <param name="writer">The text writer to append the nullable System.Int16 to.</param>
        /// <param name="value">The nullable System.Int16 value to emit.</param>
        /// <param name="context">The emitter context.</param>
        internal static void EmitNullableInt16(System.IO.TextWriter writer, Int16? value, EmitterContext context)
        {
            if (value == null)
            {
                //
                // CONSIDER: Add a flag to EmitterContext to reveal whether the TextWriter is one of the known writers
                //           which has an efficient implementation of Write(string) that doesn't call ToCharArray on
                //           the given string (see base class behavior of Write).
                //

                writer.Write("null");
            }
            else
            {
                EmitInt16(writer, value.Value, context);
            }
        }
#endif

        /// <summary>
        /// Emits a nullable System.Int32 to the specified string builder.
        /// </summary>
        /// <param name="builder">The string builder to append the nullable System.Int32 to.</param>
        /// <param name="value">The nullable System.Int32 value to emit.</param>
        /// <param name="context">The emitter context.</param>
        internal static void EmitNullableInt32(StringBuilder builder, Int32? value, EmitterContext context)
        {
            if (value == null)
            {
                builder.Append("null");
            }
            else
            {
                EmitInt32(builder, value.Value, context);
            }
        }

#if !NO_IO
        /// <summary>
        /// Emits a nullable System.Int32 to the specified text writer.
        /// </summary>
        /// <param name="writer">The text writer to append the nullable System.Int32 to.</param>
        /// <param name="value">The nullable System.Int32 value to emit.</param>
        /// <param name="context">The emitter context.</param>
        internal static void EmitNullableInt32(System.IO.TextWriter writer, Int32? value, EmitterContext context)
        {
            if (value == null)
            {
                //
                // CONSIDER: Add a flag to EmitterContext to reveal whether the TextWriter is one of the known writers
                //           which has an efficient implementation of Write(string) that doesn't call ToCharArray on
                //           the given string (see base class behavior of Write).
                //

                writer.Write("null");
            }
            else
            {
                EmitInt32(writer, value.Value, context);
            }
        }
#endif

        /// <summary>
        /// Emits a nullable System.Int64 to the specified string builder.
        /// </summary>
        /// <param name="builder">The string builder to append the nullable System.Int64 to.</param>
        /// <param name="value">The nullable System.Int64 value to emit.</param>
        /// <param name="context">The emitter context.</param>
        internal static void EmitNullableInt64(StringBuilder builder, Int64? value, EmitterContext context)
        {
            if (value == null)
            {
                builder.Append("null");
            }
            else
            {
                EmitInt64(builder, value.Value, context);
            }
        }

#if !NO_IO
        /// <summary>
        /// Emits a nullable System.Int64 to the specified text writer.
        /// </summary>
        /// <param name="writer">The text writer to append the nullable System.Int64 to.</param>
        /// <param name="value">The nullable System.Int64 value to emit.</param>
        /// <param name="context">The emitter context.</param>
        internal static void EmitNullableInt64(System.IO.TextWriter writer, Int64? value, EmitterContext context)
        {
            if (value == null)
            {
                //
                // CONSIDER: Add a flag to EmitterContext to reveal whether the TextWriter is one of the known writers
                //           which has an efficient implementation of Write(string) that doesn't call ToCharArray on
                //           the given string (see base class behavior of Write).
                //

                writer.Write("null");
            }
            else
            {
                EmitInt64(writer, value.Value, context);
            }
        }
#endif

        /// <summary>
        /// Emits a nullable System.Byte to the specified string builder.
        /// </summary>
        /// <param name="builder">The string builder to append the nullable System.Byte to.</param>
        /// <param name="value">The nullable System.Byte value to emit.</param>
        /// <param name="context">The emitter context.</param>
        internal static void EmitNullableByte(StringBuilder builder, Byte? value, EmitterContext context)
        {
            if (value == null)
            {
                builder.Append("null");
            }
            else
            {
                EmitByte(builder, value.Value, context);
            }
        }

#if !NO_IO
        /// <summary>
        /// Emits a nullable System.Byte to the specified text writer.
        /// </summary>
        /// <param name="writer">The text writer to append the nullable System.Byte to.</param>
        /// <param name="value">The nullable System.Byte value to emit.</param>
        /// <param name="context">The emitter context.</param>
        internal static void EmitNullableByte(System.IO.TextWriter writer, Byte? value, EmitterContext context)
        {
            if (value == null)
            {
                //
                // CONSIDER: Add a flag to EmitterContext to reveal whether the TextWriter is one of the known writers
                //           which has an efficient implementation of Write(string) that doesn't call ToCharArray on
                //           the given string (see base class behavior of Write).
                //

                writer.Write("null");
            }
            else
            {
                EmitByte(writer, value.Value, context);
            }
        }
#endif

        /// <summary>
        /// Emits a nullable System.UInt16 to the specified string builder.
        /// </summary>
        /// <param name="builder">The string builder to append the nullable System.UInt16 to.</param>
        /// <param name="value">The nullable System.UInt16 value to emit.</param>
        /// <param name="context">The emitter context.</param>
        internal static void EmitNullableUInt16(StringBuilder builder, UInt16? value, EmitterContext context)
        {
            if (value == null)
            {
                builder.Append("null");
            }
            else
            {
                EmitUInt16(builder, value.Value, context);
            }
        }

#if !NO_IO
        /// <summary>
        /// Emits a nullable System.UInt16 to the specified text writer.
        /// </summary>
        /// <param name="writer">The text writer to append the nullable System.UInt16 to.</param>
        /// <param name="value">The nullable System.UInt16 value to emit.</param>
        /// <param name="context">The emitter context.</param>
        internal static void EmitNullableUInt16(System.IO.TextWriter writer, UInt16? value, EmitterContext context)
        {
            if (value == null)
            {
                //
                // CONSIDER: Add a flag to EmitterContext to reveal whether the TextWriter is one of the known writers
                //           which has an efficient implementation of Write(string) that doesn't call ToCharArray on
                //           the given string (see base class behavior of Write).
                //

                writer.Write("null");
            }
            else
            {
                EmitUInt16(writer, value.Value, context);
            }
        }
#endif

        /// <summary>
        /// Emits a nullable System.UInt32 to the specified string builder.
        /// </summary>
        /// <param name="builder">The string builder to append the nullable System.UInt32 to.</param>
        /// <param name="value">The nullable System.UInt32 value to emit.</param>
        /// <param name="context">The emitter context.</param>
        internal static void EmitNullableUInt32(StringBuilder builder, UInt32? value, EmitterContext context)
        {
            if (value == null)
            {
                builder.Append("null");
            }
            else
            {
                EmitUInt32(builder, value.Value, context);
            }
        }

#if !NO_IO
        /// <summary>
        /// Emits a nullable System.UInt32 to the specified text writer.
        /// </summary>
        /// <param name="writer">The text writer to append the nullable System.UInt32 to.</param>
        /// <param name="value">The nullable System.UInt32 value to emit.</param>
        /// <param name="context">The emitter context.</param>
        internal static void EmitNullableUInt32(System.IO.TextWriter writer, UInt32? value, EmitterContext context)
        {
            if (value == null)
            {
                //
                // CONSIDER: Add a flag to EmitterContext to reveal whether the TextWriter is one of the known writers
                //           which has an efficient implementation of Write(string) that doesn't call ToCharArray on
                //           the given string (see base class behavior of Write).
                //

                writer.Write("null");
            }
            else
            {
                EmitUInt32(writer, value.Value, context);
            }
        }
#endif

        /// <summary>
        /// Emits a nullable System.UInt64 to the specified string builder.
        /// </summary>
        /// <param name="builder">The string builder to append the nullable System.UInt64 to.</param>
        /// <param name="value">The nullable System.UInt64 value to emit.</param>
        /// <param name="context">The emitter context.</param>
        internal static void EmitNullableUInt64(StringBuilder builder, UInt64? value, EmitterContext context)
        {
            if (value == null)
            {
                builder.Append("null");
            }
            else
            {
                EmitUInt64(builder, value.Value, context);
            }
        }

#if !NO_IO
        /// <summary>
        /// Emits a nullable System.UInt64 to the specified text writer.
        /// </summary>
        /// <param name="writer">The text writer to append the nullable System.UInt64 to.</param>
        /// <param name="value">The nullable System.UInt64 value to emit.</param>
        /// <param name="context">The emitter context.</param>
        internal static void EmitNullableUInt64(System.IO.TextWriter writer, UInt64? value, EmitterContext context)
        {
            if (value == null)
            {
                //
                // CONSIDER: Add a flag to EmitterContext to reveal whether the TextWriter is one of the known writers
                //           which has an efficient implementation of Write(string) that doesn't call ToCharArray on
                //           the given string (see base class behavior of Write).
                //

                writer.Write("null");
            }
            else
            {
                EmitUInt64(writer, value.Value, context);
            }
        }
#endif

        /// <summary>
        /// Emits a nullable System.Boolean to the specified string builder.
        /// </summary>
        /// <param name="builder">The string builder to append the nullable System.Boolean to.</param>
        /// <param name="value">The nullable System.Boolean value to emit.</param>
        /// <param name="context">The emitter context.</param>
        internal static void EmitNullableBoolean(StringBuilder builder, Boolean? value, EmitterContext context)
        {
            if (value == null)
            {
                builder.Append("null");
            }
            else
            {
                EmitBoolean(builder, value.Value, context);
            }
        }

#if !NO_IO
        /// <summary>
        /// Emits a nullable System.Boolean to the specified text writer.
        /// </summary>
        /// <param name="writer">The text writer to append the nullable System.Boolean to.</param>
        /// <param name="value">The nullable System.Boolean value to emit.</param>
        /// <param name="context">The emitter context.</param>
        internal static void EmitNullableBoolean(System.IO.TextWriter writer, Boolean? value, EmitterContext context)
        {
            if (value == null)
            {
                //
                // CONSIDER: Add a flag to EmitterContext to reveal whether the TextWriter is one of the known writers
                //           which has an efficient implementation of Write(string) that doesn't call ToCharArray on
                //           the given string (see base class behavior of Write).
                //

                writer.Write("null");
            }
            else
            {
                EmitBoolean(writer, value.Value, context);
            }
        }
#endif

        /// <summary>
        /// Emits a nullable System.Char to the specified string builder.
        /// </summary>
        /// <param name="builder">The string builder to append the nullable System.Char to.</param>
        /// <param name="value">The nullable System.Char value to emit.</param>
        /// <param name="context">The emitter context.</param>
        internal static void EmitNullableChar(StringBuilder builder, Char? value, EmitterContext context)
        {
            if (value == null)
            {
                builder.Append("null");
            }
            else
            {
                EmitChar(builder, value.Value, context);
            }
        }

#if !NO_IO
        /// <summary>
        /// Emits a nullable System.Char to the specified text writer.
        /// </summary>
        /// <param name="writer">The text writer to append the nullable System.Char to.</param>
        /// <param name="value">The nullable System.Char value to emit.</param>
        /// <param name="context">The emitter context.</param>
        internal static void EmitNullableChar(System.IO.TextWriter writer, Char? value, EmitterContext context)
        {
            if (value == null)
            {
                //
                // CONSIDER: Add a flag to EmitterContext to reveal whether the TextWriter is one of the known writers
                //           which has an efficient implementation of Write(string) that doesn't call ToCharArray on
                //           the given string (see base class behavior of Write).
                //

                writer.Write("null");
            }
            else
            {
                EmitChar(writer, value.Value, context);
            }
        }
#endif

        /// <summary>
        /// Emits a nullable System.DateTime to the specified string builder.
        /// </summary>
        /// <param name="builder">The string builder to append the nullable System.DateTime to.</param>
        /// <param name="value">The nullable System.DateTime value to emit.</param>
        /// <param name="context">The emitter context.</param>
        internal static void EmitNullableDateTime(StringBuilder builder, DateTime? value, EmitterContext context)
        {
            if (value == null)
            {
                builder.Append("null");
            }
            else
            {
                EmitDateTime(builder, value.Value, context);
            }
        }

#if !NO_IO
        /// <summary>
        /// Emits a nullable System.DateTime to the specified text writer.
        /// </summary>
        /// <param name="writer">The text writer to append the nullable System.DateTime to.</param>
        /// <param name="value">The nullable System.DateTime value to emit.</param>
        /// <param name="context">The emitter context.</param>
        internal static void EmitNullableDateTime(System.IO.TextWriter writer, DateTime? value, EmitterContext context)
        {
            if (value == null)
            {
                //
                // CONSIDER: Add a flag to EmitterContext to reveal whether the TextWriter is one of the known writers
                //           which has an efficient implementation of Write(string) that doesn't call ToCharArray on
                //           the given string (see base class behavior of Write).
                //

                writer.Write("null");
            }
            else
            {
                EmitDateTime(writer, value.Value, context);
            }
        }
#endif

        /// <summary>
        /// Emits a nullable System.DateTimeOffset to the specified string builder.
        /// </summary>
        /// <param name="builder">The string builder to append the nullable System.DateTimeOffset to.</param>
        /// <param name="value">The nullable System.DateTimeOffset value to emit.</param>
        /// <param name="context">The emitter context.</param>
        internal static void EmitNullableDateTimeOffset(StringBuilder builder, DateTimeOffset? value, EmitterContext context)
        {
            if (value == null)
            {
                builder.Append("null");
            }
            else
            {
                EmitDateTimeOffset(builder, value.Value, context);
            }
        }

#if !NO_IO
        /// <summary>
        /// Emits a nullable System.DateTimeOffset to the specified text writer.
        /// </summary>
        /// <param name="writer">The text writer to append the nullable System.DateTimeOffset to.</param>
        /// <param name="value">The nullable System.DateTimeOffset value to emit.</param>
        /// <param name="context">The emitter context.</param>
        internal static void EmitNullableDateTimeOffset(System.IO.TextWriter writer, DateTimeOffset? value, EmitterContext context)
        {
            if (value == null)
            {
                //
                // CONSIDER: Add a flag to EmitterContext to reveal whether the TextWriter is one of the known writers
                //           which has an efficient implementation of Write(string) that doesn't call ToCharArray on
                //           the given string (see base class behavior of Write).
                //

                writer.Write("null");
            }
            else
            {
                EmitDateTimeOffset(writer, value.Value, context);
            }
        }
#endif

        /// <summary>
        /// Emits a nullable System.Single to the specified string builder.
        /// </summary>
        /// <param name="builder">The string builder to append the nullable System.Single to.</param>
        /// <param name="value">The nullable System.Single value to emit.</param>
        /// <param name="context">The emitter context.</param>
        internal static void EmitNullableSingle(StringBuilder builder, Single? value, EmitterContext context)
        {
            if (value == null)
            {
                builder.Append("null");
            }
            else
            {
                EmitSingle(builder, value.Value, context);
            }
        }

#if !NO_IO
        /// <summary>
        /// Emits a nullable System.Single to the specified text writer.
        /// </summary>
        /// <param name="writer">The text writer to append the nullable System.Single to.</param>
        /// <param name="value">The nullable System.Single value to emit.</param>
        /// <param name="context">The emitter context.</param>
        internal static void EmitNullableSingle(System.IO.TextWriter writer, Single? value, EmitterContext context)
        {
            if (value == null)
            {
                //
                // CONSIDER: Add a flag to EmitterContext to reveal whether the TextWriter is one of the known writers
                //           which has an efficient implementation of Write(string) that doesn't call ToCharArray on
                //           the given string (see base class behavior of Write).
                //

                writer.Write("null");
            }
            else
            {
                EmitSingle(writer, value.Value, context);
            }
        }
#endif

        /// <summary>
        /// Emits a nullable System.Double to the specified string builder.
        /// </summary>
        /// <param name="builder">The string builder to append the nullable System.Double to.</param>
        /// <param name="value">The nullable System.Double value to emit.</param>
        /// <param name="context">The emitter context.</param>
        internal static void EmitNullableDouble(StringBuilder builder, Double? value, EmitterContext context)
        {
            if (value == null)
            {
                builder.Append("null");
            }
            else
            {
                EmitDouble(builder, value.Value, context);
            }
        }

#if !NO_IO
        /// <summary>
        /// Emits a nullable System.Double to the specified text writer.
        /// </summary>
        /// <param name="writer">The text writer to append the nullable System.Double to.</param>
        /// <param name="value">The nullable System.Double value to emit.</param>
        /// <param name="context">The emitter context.</param>
        internal static void EmitNullableDouble(System.IO.TextWriter writer, Double? value, EmitterContext context)
        {
            if (value == null)
            {
                //
                // CONSIDER: Add a flag to EmitterContext to reveal whether the TextWriter is one of the known writers
                //           which has an efficient implementation of Write(string) that doesn't call ToCharArray on
                //           the given string (see base class behavior of Write).
                //

                writer.Write("null");
            }
            else
            {
                EmitDouble(writer, value.Value, context);
            }
        }
#endif

        /// <summary>
        /// Emits a nullable System.Decimal to the specified string builder.
        /// </summary>
        /// <param name="builder">The string builder to append the nullable System.Decimal to.</param>
        /// <param name="value">The nullable System.Decimal value to emit.</param>
        /// <param name="context">The emitter context.</param>
        internal static void EmitNullableDecimal(StringBuilder builder, Decimal? value, EmitterContext context)
        {
            if (value == null)
            {
                builder.Append("null");
            }
            else
            {
                EmitDecimal(builder, value.Value, context);
            }
        }

#if !NO_IO
        /// <summary>
        /// Emits a nullable System.Decimal to the specified text writer.
        /// </summary>
        /// <param name="writer">The text writer to append the nullable System.Decimal to.</param>
        /// <param name="value">The nullable System.Decimal value to emit.</param>
        /// <param name="context">The emitter context.</param>
        internal static void EmitNullableDecimal(System.IO.TextWriter writer, Decimal? value, EmitterContext context)
        {
            if (value == null)
            {
                //
                // CONSIDER: Add a flag to EmitterContext to reveal whether the TextWriter is one of the known writers
                //           which has an efficient implementation of Write(string) that doesn't call ToCharArray on
                //           the given string (see base class behavior of Write).
                //

                writer.Write("null");
            }
            else
            {
                EmitDecimal(writer, value.Value, context);
            }
        }
#endif

    }
}
