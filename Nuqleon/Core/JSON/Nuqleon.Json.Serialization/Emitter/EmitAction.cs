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
    /// <summary>
    /// Delegate to emit string output for an instance of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type to emit output for.</typeparam>
    /// <param name="builder">The builder to append output to.</param>
    /// <param name="value">The value to emit.</param>
    /// <param name="ctx">The emitter context used to thread state through the entire serialization.</param>
    internal delegate void EmitStringAction<in T>(StringBuilder builder, T value, EmitterContext ctx);

#if !NO_IO
    /// <summary>
    /// Delegate to emit string output for an instance of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type to emit output for.</typeparam>
    /// <param name="writer">The text writer to append output to.</param>
    /// <param name="value">The value to emit.</param>
    /// <param name="ctx">The emitter context used to thread state through the entire serialization.</param>
    internal delegate void EmitWriterAction<in T>(System.IO.TextWriter writer, T value, EmitterContext ctx);
#endif
}
