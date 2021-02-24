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
        /// Emits a System.Boolean as a JSON Boolean to the specified string builder.
        /// </summary>
        /// <param name="builder">The string builder to append the Boolean to.</param>
        /// <param name="value">The Boolean value to emit.</param>
        /// <param name="_">The emitter context.</param>
        internal static void EmitBoolean(StringBuilder builder, bool value, EmitterContext _ = null)
        {
            //
            // NB: Experiments show that Append(string) is faster than Append(char[]) with a precomputed array.
            //

            if (value)
            {
                builder.Append("true");
            }
            else
            {
                builder.Append("false");
            }
        }

#if !NO_IO
        /// <summary>
        /// Emits a System.Boolean as a JSON Boolean to the specified text writer.
        /// </summary>
        /// <param name="writer">The text writer to append the Boolean to.</param>
        /// <param name="value">The Boolean value to emit.</param>
        /// <param name="_">The emitter context.</param>
        internal static void EmitBoolean(System.IO.TextWriter writer, bool value, EmitterContext _ = null)
        {
            //
            // NB: Experiments show that Write(string) is faster than using Write(char[]) or Write(char) when used
            //     against StringWriter and StreamWriter.
            //
            // CONSIDER: Add a flag to EmitterContext to reveal whether the TextWriter is one of the known writers
            //           which has an efficient implementation of Write(string) that doesn't call ToCharArray on
            //           the given string (see base class behavior of Write).
            //

            if (value)
            {
                writer.Write("true");
            }
            else
            {
                writer.Write("false");
            }
        }
#endif
    }
}
