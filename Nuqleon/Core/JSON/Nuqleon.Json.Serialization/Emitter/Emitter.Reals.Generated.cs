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
using System.Globalization;
using System.Text;

namespace Nuqleon.Json.Serialization
{
    internal partial class Emitter
    {
        // TODO: Add support for NaN, -Infinity, Infinity

        /// <summary>
        /// Emits a System.Single as a JSON Number to the specified string builder.
        /// </summary>
        /// <param name="builder">The string builder to append the System.Single to.</param>
        /// <param name="value">The System.Single value to emit.</param>
        /// <param name="_">The emitter context.</param>
        internal static void EmitSingle(StringBuilder builder, Single value, EmitterContext _ = null) => builder.Append(value.ToString(CultureInfo.InvariantCulture));

#if !NO_IO
        /// <summary>
        /// Emits a System.Single as a JSON Number to the specified text writer.
        /// </summary>
        /// <param name="writer">The text writer to append the System.Single to.</param>
        /// <param name="value">The System.Single value to emit.</param>
        /// <param name="_">The emitter context.</param>
        internal static void EmitSingle(System.IO.TextWriter writer, Single value, EmitterContext _ = null) => writer.Write(value.ToString(CultureInfo.InvariantCulture));
#endif

        /// <summary>
        /// Emits a System.Double as a JSON Number to the specified string builder.
        /// </summary>
        /// <param name="builder">The string builder to append the System.Double to.</param>
        /// <param name="value">The System.Double value to emit.</param>
        /// <param name="_">The emitter context.</param>
        internal static void EmitDouble(StringBuilder builder, Double value, EmitterContext _ = null) => builder.Append(value.ToString(CultureInfo.InvariantCulture));

#if !NO_IO
        /// <summary>
        /// Emits a System.Double as a JSON Number to the specified text writer.
        /// </summary>
        /// <param name="writer">The text writer to append the System.Double to.</param>
        /// <param name="value">The System.Double value to emit.</param>
        /// <param name="_">The emitter context.</param>
        internal static void EmitDouble(System.IO.TextWriter writer, Double value, EmitterContext _ = null) => writer.Write(value.ToString(CultureInfo.InvariantCulture));
#endif

        /// <summary>
        /// Emits a System.Decimal as a JSON Number to the specified string builder.
        /// </summary>
        /// <param name="builder">The string builder to append the System.Decimal to.</param>
        /// <param name="value">The System.Decimal value to emit.</param>
        /// <param name="_">The emitter context.</param>
        internal static void EmitDecimal(StringBuilder builder, Decimal value, EmitterContext _ = null) => builder.Append(value.ToString(CultureInfo.InvariantCulture));

#if !NO_IO
        /// <summary>
        /// Emits a System.Decimal as a JSON Number to the specified text writer.
        /// </summary>
        /// <param name="writer">The text writer to append the System.Decimal to.</param>
        /// <param name="value">The System.Decimal value to emit.</param>
        /// <param name="_">The emitter context.</param>
        internal static void EmitDecimal(System.IO.TextWriter writer, Decimal value, EmitterContext _ = null) => writer.Write(value.ToString(CultureInfo.InvariantCulture));
#endif

    }
}
