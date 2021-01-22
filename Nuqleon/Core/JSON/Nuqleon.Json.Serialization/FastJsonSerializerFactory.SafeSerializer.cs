// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 04/16/2016 - Created fast JSON serializer functionality.
//   BD - 05/08/2016 - Added support for serialization to text writers.
//

using System.Memory;
using System.Text;

namespace Nuqleon.Json.Serialization
{
    public partial class FastJsonSerializerFactory
    {
        /// <summary>
        /// JSON serializer for objects of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of the objects to serialize.</typeparam>
        /// <remarks>This type is thread-safe.</remarks>
        private sealed class SafeSerializer<T> : SerializerBase<T>
        {
            private readonly ObjectPool<EmitterContext> _contextPool;

#if !NO_IO
            /// <summary>
            /// Creates a new serializer given the specified emitter implementation.
            /// </summary>
            /// <param name="emitterString">The emitter to use to serialize objects to string outputs.</param>
            /// <param name="builderString">The builder to use to create emitters for objects based on their runtime type.</param>
            /// <param name="emitterWriter">The emitter to use to serialize objects to text writers.</param>
            /// <param name="builderWriter">The builder to use to create emitters for objects based on their runtime type.</param>
            public SafeSerializer(EmitStringAction<T> emitterString, EmitterStringBuilder builderString, EmitWriterAction<T> emitterWriter, EmitterWriterBuilder builderWriter)
                : base(emitterString, emitterWriter)
            {
                _contextPool = new ObjectPool<EmitterContext>(() => new EmitterContext(builderString, builderWriter));
            }
#else
            /// <summary>
            /// Creates a new serializer given the specified emitter implementation.
            /// </summary>
            /// <param name="emitter">The emitter to use to serialize objects.</param>
            /// <param name="builder">The builder to use to create emitters for objects based on their runtime type.</param>
            public SafeSerializer(EmitStringAction<T> emitter, EmitterStringBuilder builder)
                : base(emitter)
            {
                _contextPool = new ObjectPool<EmitterContext>(() => new EmitterContext(builder));
            }
#endif

            /// <summary>
            /// Serializes the specified object to JSON in the specified string builder.
            /// </summary>
            /// <param name="builder">The string builder to append the JSON to.</param>
            /// <param name="value">The object to serialize.</param>
            protected override void SerializeCore(StringBuilder builder, T value)
            {
                using var context = _contextPool.New();

                _emitterString(builder, value, context.Object);
            }

            /// <summary>
            /// Serializes the specified object to JSON in the specified text writer.
            /// </summary>
            /// <param name="writer">The text writer to append the JSON to.</param>
            /// <param name="value">The object to serialize.</param>
            protected override void SerializeCore(System.IO.TextWriter writer, T value)
            {
                using var context = _contextPool.New();

                _emitterWriter(writer, value, context.Object);
            }
        }
    }
}
