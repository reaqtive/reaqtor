// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;

namespace Reaqtor.QueryEngine
{
    /// <summary>
    /// Interface used to abstract over serializers used for persistence of state (checkpoints, transaction logs, etc.).
    /// </summary>
    public interface ISerializer
    {
        /// <summary>
        /// Gets the name tag of the serializer.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the version of the serializer.
        /// </summary>
        Version Version { get; }

        /// <summary>
        /// Serializes a value of the specified type to the specified stream.
        /// </summary>
        /// <typeparam name="T">The type of the value to serialize.</typeparam>
        /// <param name="value">The value to serialize.</param>
        /// <param name="stream">The stream to serialize to.</param>
        void Serialize<T>(T value, Stream stream);

        /// <summary>
        /// Deserializes a value of the specified type from the specified stream.
        /// </summary>
        /// <typeparam name="T">The type of the value to deserialize.</typeparam>
        /// <param name="stream">The stream to deserialize from.</param>
        /// <returns>The deserialized value.</returns>
        T Deserialize<T>(Stream stream);

        /// <summary>
        /// Serializes a value of the specified type to the specified stream.
        /// </summary>
        /// <param name="value">The value to serialize.</param>
        /// <param name="type">The type of the value to serialize.</param>
        /// <param name="stream">The stream to serialize to.</param>
        void Serialize(object value, Type type, Stream stream);

        /// <summary>
        /// Deserializes a value of the specified type from the specified stream.
        /// </summary>
        /// <param name="stream">The stream to deserialize from.</param>
        /// <param name="type">The type of the value to deserialize.</param>
        /// <returns>The deserialized value.</returns>
        object Deserialize(Type type, Stream stream);
    }
}
