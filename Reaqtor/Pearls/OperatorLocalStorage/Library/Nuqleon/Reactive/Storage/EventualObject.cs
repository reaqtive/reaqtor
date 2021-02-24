// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2018
//

using System.IO;

using Reaqtive.Serialization;

namespace Reaqtive.Storage
{
    /// <summary>
    /// Representation of an object obtained from persisted state with deferred deserialization support.
    /// </summary>
    internal sealed class EventualObject
    {
        /// <summary>
        /// The byte array containing the serialized state representing the object.
        /// </summary>
        private readonly byte[] _bytes;

        /// <summary>
        /// Creates a new instance of <see cref="EventualObject"/>.
        /// </summary>
        /// <param name="bytes">The byte array containing the serialized state representing the object.</param>
        private EventualObject(byte[] bytes)
        {
            _bytes = bytes;
        }

        /// <summary>
        /// Creates a new instance of <see cref="EventualObject"/>.
        /// </summary>
        /// <param name="stream">The stream containing the serialized state representing the object.</param>
        /// <returns>An eventual object supporting deferred serialization of its representation in <paramref name="stream"/>.</returns>
        public static EventualObject FromState(Stream stream)
        {
            if (stream is not MemoryStream ms)
            {
                stream.Position = 0;

                ms = new MemoryStream();
                stream.CopyTo(ms);
            }

            var bytes = ms.ToArray();

            return new EventualObject(bytes);
        }

        /// <summary>
        /// Deserializes the eventual object into an instance of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type to deserialize to.</typeparam>
        /// <param name="deserializerFactory">The deserialization factory to use to create a deserializer.</param>
        /// <returns>An instance of type <typeparamref name="T"/> obtained from deserializing the state.</returns>
        public T Deserialize<T>(IDeserializerFactory deserializerFactory) => Deserialize(deserializerFactory.GetDeserializer<T>());

        /// <summary>
        /// Deserializes the eventual object into an instance of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type to deserialize to.</typeparam>
        /// <param name="deserializer">The deserializer to us.</param>
        /// <returns>An instance of type <typeparamref name="T"/> obtained from deserializing the state.</returns>
        public T Deserialize<T>(IDeserializer<T> deserializer)
        {
            using var ms = new MemoryStream(_bytes);

            return deserializer.Deserialize(ms);
        }
    }
}
