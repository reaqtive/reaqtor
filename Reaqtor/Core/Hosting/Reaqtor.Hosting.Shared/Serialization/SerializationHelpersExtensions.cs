// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;

namespace Reaqtor.Hosting.Shared.Serialization
{
    /// <summary>
    /// Useful extension methods for serialization helpers.
    /// </summary>
    public static class SerializationHelpersExtensions
    {
        /// <summary>
        /// Serializes the specified object. If the object contains expression 
        /// trees, Bonsai representation is used to serialize those.
        /// </summary>
        /// <typeparam name="T">Type of the object that gets serialized.</typeparam>
        /// <param name="serializationHelpers">The serialization helpers.</param>
        /// <param name="value">Object to serialize.</param>
        /// <returns>Serialized representation of the object.</returns>
        /// <remarks>The use of a generic type parameter allows for "typed nulls".</remarks>
        public static string Serialize<T>(this SerializationHelpers serializationHelpers, T value)
        {
            if (serializationHelpers == null)
                throw new ArgumentNullException(nameof(serializationHelpers));

            var stream = new MemoryStream();

            try
            {
                serializationHelpers.Serialize<T>(value, stream);
                stream.Position = 0;

                using var reader = new StreamReader(stream);

                stream = null;

                return reader.ReadToEnd();
            }
            finally
            {
                stream?.Dispose();
            }
        }

        /// <summary>
        /// Deserializes the specified serialization payload to an instance of 
        /// the specified type.
        /// </summary>
        /// <typeparam name="T">Type of the deserialized object.</typeparam>
        /// <param name="serializationHelpers">The serialization helpers.</param>
        /// <param name="json">Serialized representation of the object.</param>
        /// <returns>
        /// Instance of the specified type resulting from deserializing the 
        /// payload.
        /// </returns>
        /// <remarks>
        /// It is possible to deserialize an object to a different CLR type than
        /// the one that was used during serialization, as long as it has the 
        /// same data model projection.
        /// </remarks>
        /// <exception cref="Nuqleon.DataModel.Serialization.Json.DataSerializerException">
        /// if a JsonException was thrown during deserialization of <c>json</c></exception>
        public static T Deserialize<T>(this SerializationHelpers serializationHelpers, string json)
        {
            if (serializationHelpers == null)
                throw new ArgumentNullException(nameof(serializationHelpers));

            var stream = new MemoryStream();

            try
            {
                using var writer = new StreamWriter(stream);

                stream = (MemoryStream)writer.BaseStream; // CA2202 pattern

                writer.Write(json);
                writer.Flush();

                stream.Position = 0;

                return serializationHelpers.Deserialize<T>(stream);
            }
            finally
            {
                stream?.Dispose();
            }
        }

        /// <summary>
        /// Converts a byte array to its object representation
        /// </summary>
        /// <typeparam name="T">the type to return</typeparam>
        /// <param name="serializationHelpers">The serialization helpers.</param>
        /// <param name="bytes">The bytes.</param>
        /// <returns>
        /// the object deserialized from the stream
        /// </returns>
        public static T Deserialize<T>(this SerializationHelpers serializationHelpers, byte[] bytes)
        {
            if (serializationHelpers == null)
                throw new ArgumentNullException(nameof(serializationHelpers));
            if (bytes == null)
                throw new ArgumentNullException(nameof(bytes));

            using var stream = new MemoryStream(bytes) { Position = 0 };

            return serializationHelpers.Deserialize<T>(stream);
        }

        /// <summary>
        /// Serializes the specified object. If the object contains expression 
        /// trees, Bonsai representation is used to serialize those.
        /// </summary>
        /// <typeparam name="T">Type of the object that gets serialized.</typeparam>
        /// <param name="serializationHelpers">The serialization helpers.</param>
        /// <param name="value">Object to serialize.</param>
        /// <returns>Serialized representation of the object.</returns>
        /// <remarks>The use of a generic type parameter allows for "typed nulls".</remarks>
        public static byte[] ToBytes<T>(this SerializationHelpers serializationHelpers, T value)
        {
            if (serializationHelpers == null)
                throw new ArgumentNullException(nameof(serializationHelpers));

            using var stream = new MemoryStream();

            serializationHelpers.Serialize<T>(value, stream);

            stream.Position = 0;

            return stream.ToArray();
        }
    }
}
