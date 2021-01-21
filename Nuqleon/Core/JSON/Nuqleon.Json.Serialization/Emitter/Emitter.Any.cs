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
using System.Collections.Generic;
using System.Text;

namespace Nuqleon.Json.Serialization
{
    internal partial class Emitter
    {
        /// <summary>
        /// Emits an object as a JSON value by using its runtime type, or as a JSON null literal if it's null.
        /// </summary>
        /// <param name="builder">The string builder to append the object to.</param>
        /// <param name="value">The value to emit.</param>
        /// <param name="context">The emitter context.</param>
        internal static void EmitAny(StringBuilder builder, object value, EmitterContext context)
        {
            //
            // CONSIDER: Expose a serialization option to disallow late bound serialization.
            //

            if (value == null)
            {
                builder.Append("null");
                return;
            }

            var type = value.GetType();

            //
            // NB: We first try to dispatch quickly to an emitter based on a check for well-known types.
            //

            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Boolean:
                    EmitBoolean(builder, (bool)value, context);
                    return;
                case TypeCode.Char:
                    EmitChar(builder, (char)value, context);
                    return;
                case TypeCode.SByte:
                    EmitSByte(builder, (sbyte)value, context);
                    return;
                case TypeCode.Byte:
                    EmitByte(builder, (byte)value, context);
                    return;
                case TypeCode.Int16:
                    EmitInt16(builder, (short)value, context);
                    return;
                case TypeCode.UInt16:
                    EmitUInt16(builder, (ushort)value, context);
                    return;
                case TypeCode.Int32:
                    EmitInt32(builder, (int)value, context);
                    return;
                case TypeCode.UInt32:
                    EmitUInt32(builder, (uint)value, context);
                    return;
                case TypeCode.Int64:
                    EmitInt64(builder, (long)value, context);
                    return;
                case TypeCode.UInt64:
                    EmitUInt64(builder, (ulong)value, context);
                    return;
                case TypeCode.Single:
                    EmitSingle(builder, (float)value, context);
                    return;
                case TypeCode.Double:
                    EmitDouble(builder, (double)value, context);
                    return;
                case TypeCode.Decimal:
                    EmitDecimal(builder, (decimal)value, context);
                    return;
                case TypeCode.DateTime:
                    EmitDateTime(builder, (DateTime)value, context);
                    return;
                case TypeCode.String:
                    EmitString(builder, (string)value, context);
                    return;
            }

            if (type == typeof(DateTimeOffset))
            {
                EmitDateTimeOffset(builder, (DateTimeOffset)value, context);
                return;
            }

            //
            // NB: Serialization of the same types is hopefully common, so we build a polymorphic inline cache and store it in
            //     the emitter context.
            //
            // CONSIDER: What should be the scope for the polymorphic inline cache? Per serializer or per invocation?
            //
            // TODO: Add object reference cycle detection.
            //

            context.EmitAnyString(builder, value, context);
        }

#if !NO_IO
        /// <summary>
        /// Emits an object as a JSON value by using its runtime type, or as a JSON null literal if it's null.
        /// </summary>
        /// <param name="writer">The text writer to append the object to.</param>
        /// <param name="value">The value to emit.</param>
        /// <param name="context">The emitter context.</param>
        internal static void EmitAny(System.IO.TextWriter writer, object value, EmitterContext context)
        {
            //
            // CONSIDER: Expose a serialization option to disallow late bound serialization.
            //

            if (value == null)
            {
                writer.Write("null");
                return;
            }

            var type = value.GetType();

            //
            // NB: We first try to dispatch quickly to an emitter based on a check for well-known types.
            //

            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Boolean:
                    EmitBoolean(writer, (bool)value, context);
                    return;
                case TypeCode.Char:
                    EmitChar(writer, (char)value, context);
                    return;
                case TypeCode.SByte:
                    EmitSByte(writer, (sbyte)value, context);
                    return;
                case TypeCode.Byte:
                    EmitByte(writer, (byte)value, context);
                    return;
                case TypeCode.Int16:
                    EmitInt16(writer, (short)value, context);
                    return;
                case TypeCode.UInt16:
                    EmitUInt16(writer, (ushort)value, context);
                    return;
                case TypeCode.Int32:
                    EmitInt32(writer, (int)value, context);
                    return;
                case TypeCode.UInt32:
                    EmitUInt32(writer, (uint)value, context);
                    return;
                case TypeCode.Int64:
                    EmitInt64(writer, (long)value, context);
                    return;
                case TypeCode.UInt64:
                    EmitUInt64(writer, (ulong)value, context);
                    return;
                case TypeCode.Single:
                    EmitSingle(writer, (float)value, context);
                    return;
                case TypeCode.Double:
                    EmitDouble(writer, (double)value, context);
                    return;
                case TypeCode.Decimal:
                    EmitDecimal(writer, (decimal)value, context);
                    return;
                case TypeCode.DateTime:
                    EmitDateTime(writer, (DateTime)value, context);
                    return;
                case TypeCode.String:
                    EmitString(writer, (string)value, context);
                    return;
            }

            if (type == typeof(DateTimeOffset))
            {
                EmitDateTimeOffset(writer, (DateTimeOffset)value, context);
                return;
            }

            //
            // NB: Serialization of the same types is hopefully common, so we build a polymorphic inline cache and store it in
            //     the emitter context.
            //
            // CONSIDER: What should be the scope for the polymorphic inline cache? Per serializer or per invocation?
            //
            // TODO: Add object reference cycle detection.
            //

            context.EmitAnyWriter(writer, value, context);
        }
#endif

        /// <summary>
        /// Emits a homogeneously typed dictionary as a JSON Object to the specified string builder.
        /// </summary>
        /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
        /// <typeparam name="TDictionary">The type of the dictionary to serialize.</typeparam>
        /// <param name="builder">The string builder to append the dictionary to.</param>
        /// <param name="value">The homogeneously typed dictionary to emit.</param>
        /// <param name="context">The emitter context.</param>
        /// <param name="emitValue">The emitter to use for values.</param>
        internal static void EmitAnyObject<TValue, TDictionary>(StringBuilder builder, TDictionary value, EmitterContext context, EmitStringAction<TValue> emitValue)
            where TDictionary : IEnumerable<KeyValuePair<string, TValue>>
        {
            if (value == null)
            {
                builder.Append("null");
                return;
            }

            builder.Append('{');

            var first = true;

            foreach (var kv in value)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    builder.Append(',');
                }

                var key = kv.Key;
                var val = kv.Value;

                if (key == null)
                    throw new InvalidOperationException("Encountered a key value of 'null' which can not be used for a property in a JSON Object.");

                EmitString(builder, key, context);

                builder.Append(':');

                emitValue(builder, val, context);
            }

            builder.Append('}');
        }

#if !NO_IO
        /// <summary>
        /// Emits a homogeneously typed dictionary as a JSON Object to the specified text writer.
        /// </summary>
        /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
        /// <typeparam name="TDictionary">The type of the dictionary to serialize.</typeparam>
        /// <param name="writer">The text writer to write the dictionary to.</param>
        /// <param name="value">The homogeneously typed dictionary to emit.</param>
        /// <param name="context">The emitter context.</param>
        /// <param name="emitValue">The emitter to use for values.</param>
        internal static void EmitAnyObject<TValue, TDictionary>(System.IO.TextWriter writer, TDictionary value, EmitterContext context, EmitWriterAction<TValue> emitValue)
            where TDictionary : IEnumerable<KeyValuePair<string, TValue>>
        {
            if (value == null)
            {
                writer.Write("null");
                return;
            }

            writer.Write('{');

            var first = true;

            foreach (var kv in value)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    writer.Write(',');
                }

                var key = kv.Key;
                var val = kv.Value;

                if (key == null)
                    throw new InvalidOperationException("Encountered a key value of 'null' which can not be used for a property in a JSON Object.");

                EmitString(writer, key, context);

                writer.Write(':');

                emitValue(writer, val, context);
            }

            writer.Write('}');
        }
#endif
    }
}
