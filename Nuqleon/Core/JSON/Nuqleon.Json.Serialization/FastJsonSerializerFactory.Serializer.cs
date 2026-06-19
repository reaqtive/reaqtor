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
    public partial class FastJsonSerializerFactory
    {
        /// <summary>
        /// JSON serializer for objects of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of the objects to serialize.</typeparam>
        /// <remarks>This type is not thread-safe.</remarks>
        private sealed class Serializer<T> : SerializerBase<T>
        {
            private readonly EmitterContext _context;

#if !NO_IO
            /// <summary>
            /// Creates a new serializer given the specified emitter implementation.
            /// </summary>
            /// <param name="emitterString">The emitter to use to serialize objects to string outputs.</param>
            /// <param name="builderString">The builder to use to create emitters for objects based on their runtime type.</param>
            /// <param name="emitterWriter">The emitter to use to serialize objects to text writers.</param>
            /// <param name="builderWriter">The builder to use to create emitters for objects based on their runtime type.</param>
            public Serializer(EmitStringAction<T> emitterString, EmitterStringBuilder builderString, EmitWriterAction<T> emitterWriter, EmitterWriterBuilder builderWriter)
                : base(emitterString, emitterWriter)
            {
                _context = new EmitterContext(builderString, builderWriter);
            }
#else
            /// <summary>
            /// Creates a new serializer given the specified emitter implementation.
            /// </summary>
            /// <param name="emitterString">The emitter to use to serialize objects.</param>
            /// <param name="builderString">The builder to use to create emitters for objects based on their runtime type.</param>
            public Serializer(EmitStringAction<T> emitterString, EmitterBuilder builderString)
                : base(emitterString)
            {
                _context = new EmitterContext(builderString);
            }
#endif

            /// <summary>
            /// Serializes the specified object to JSON in the specified string builder.
            /// </summary>
            /// <param name="builder">The string builder to append the JSON to.</param>
            /// <param name="value">The object to serialize.</param>
            protected override void SerializeCore(StringBuilder builder, T value)
            {
                //
                // NB: Sharing the same context can boost performance significantly because we don't have to obtain
                //     a context in a thread-safe manner from a pool, or allocate a new one each time. This in turn
                //     allows to reuse (unsafe) character buffers etc.
                //

                _emitterString(builder, value, _context);
            }

#if !NO_IO
            /// <summary>
            /// Serializes the specified object to JSON in the specified text writer.
            /// </summary>
            /// <param name="writer">The text writer to append the JSON to.</param>
            /// <param name="value">The object to serialize.</param>
            protected override void SerializeCore(System.IO.TextWriter writer, T value)
            {
                //
                // NB: Sharing the same context can boost performance significantly because we don't have to obtain
                //     a context in a thread-safe manner from a pool, or allocate a new one each time. This in turn
                //     allows to reuse (unsafe) character buffers etc.
                //

                _emitterWriter(writer, value, _context);
            }
#endif
        }
    }
}
