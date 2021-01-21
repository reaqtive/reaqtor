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
        /// Base class for JSON serializers for objects of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of the objects to serialize.</typeparam>
        private abstract class SerializerBase<T> : IFastJsonSerializer<T>
        {
#if !NO_IO
            protected readonly EmitStringAction<T> _emitterString;
            protected readonly EmitWriterAction<T> _emitterWriter;

            /// <summary>
            /// Creates a new serializer given the specified emitter implementations.
            /// </summary>
            /// <param name="emitterString">The emitter to use to serialize objects to string outputs.</param>
            /// <param name="emitterWriter">The emitter to use to serialize objects to text writers.</param>
            public SerializerBase(EmitStringAction<T> emitterString, EmitWriterAction<T> emitterWriter)
            {
                _emitterString = emitterString;
                _emitterWriter = emitterWriter;
            }
#else
            protected readonly EmitStringAction<T> _emitterString;

            /// <summary>
            /// Creates a new serializer given the specified emitter implementation.
            /// </summary>
            /// <param name="emitterString">The emitter to use to serialize objects.</param>
            public SerializerBase(EmitStringAction<T> emitterString)
            {
                _emitterString = emitterString;
            }
#endif

            /// <summary>
            /// Serializes the specified object to JSON.
            /// </summary>
            /// <param name="value">The object to serialize.</param>
            /// <returns>A JSON string containing the result of serializing the specified object.</returns>
            public string Serialize(T value)
            {
                //
                // CONSIDER: Add an overload or extension method for serialization into a StringBuilder.
                //

                using var psb = PooledStringBuilder.New();

                var sb = psb.StringBuilder;

                SerializeCore(sb, value);

                return sb.ToString();
            }

#if !NO_IO
            /// <summary>
            /// Serializes the specified object to JSON into the specified text writer.
            /// </summary>
            /// <param name="value">The object to serialize.</param>
            /// <param name="writer">The text writer to append the JSON output to.</param>
            public void Serialize(T value, System.IO.TextWriter writer)
            {
                SerializeCore(writer, value);
            }
#endif

            /// <summary>
            /// Serializes the specified object to JSON in the specified string builder.
            /// </summary>
            /// <param name="builder">The string builder to append the JSON to.</param>
            /// <param name="value">The object to serialize.</param>
            protected abstract void SerializeCore(StringBuilder builder, T value);

#if !NO_IO
            /// <summary>
            /// Serializes the specified object to JSON in the specified text writer.
            /// </summary>
            /// <param name="writer">The text writer to append the JSON to.</param>
            /// <param name="value">The object to serialize.</param>
            protected abstract void SerializeCore(System.IO.TextWriter writer, T value);
#endif
        }
    }
}
