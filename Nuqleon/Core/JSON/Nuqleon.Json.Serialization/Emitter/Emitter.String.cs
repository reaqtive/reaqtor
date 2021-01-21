// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 04/16/2016 - Created fast JSON serializer functionality.
//   BD - 05/08/2016 - Added support for serialization to text writers.
//

using System.Text;

namespace Nuqleon.Json.Serialization
{
    internal partial class Emitter
    {
        /// <summary>
        /// Emits a System.String as a JSON String to the specified string builder.
        /// </summary>
        /// <param name="builder">The string builder to append the String to.</param>
        /// <param name="value">The String value to emit.</param>
        /// <param name="_">The emitter context.</param>
        internal static void EmitString(StringBuilder builder, string value, EmitterContext _ = null)
        {
            if (value == null)
            {
                builder.Append("null");
                return;
            }

            var len = value.Length;

            if (len == 0)
            {
                builder.Append("\"\"");
                return;
            }

            builder.Append('\"');

            var i = 0;

            while (i < len)
            {
                var c = value[i];

                var needsEscape = c switch
                {
                    '\"' or '\\' or '\b' or '\f' or '\n' or '\r' or '\t' => true,
                    _ => char.IsControl(c),
                };

                if (needsEscape)
                {
                    break;
                }

                i++;
            }

            builder.Append(value, 0, i);

            while (i < len)
            {
                EmitCharCore(builder, value[i++]);
            }

            builder.Append('\"');
        }

#if !NO_IO
        /// <summary>
        /// Emits a System.String as a JSON String to the specified text writer.
        /// </summary>
        /// <param name="writer">The text writer to append the String to.</param>
        /// <param name="value">The String value to emit.</param>
        /// <param name="_">The emitter context.</param>
        internal static void EmitString(System.IO.TextWriter writer, string value, EmitterContext _ = null)
        {
            if (value == null)
            {
                writer.Write("null");
                return;
            }

            var len = value.Length;

            if (len == 0)
            {
                writer.Write("\"\"");
                return;
            }

            writer.Write('\"');

            var i = 0;

            while (i < len)
            {
                var c = value[i];

                var needsEscape = c switch
                {
                    '\"' or '\\' or '\b' or '\f' or '\n' or '\r' or '\t' => true,
                    _ => char.IsControl(c),
                };

                if (needsEscape)
                {
                    break;
                }

                i++;
            }

            if (i == len)
            {
                writer.Write(value);
            }
            else
            {
                for (var j = 0; j < i; j++)
                {
                    writer.Write(value[j]);
                }

                while (i < len)
                {
                    EmitCharCore(writer, value[i++]);
                }
            }

            writer.Write('\"');
        }
#endif
    }
}
