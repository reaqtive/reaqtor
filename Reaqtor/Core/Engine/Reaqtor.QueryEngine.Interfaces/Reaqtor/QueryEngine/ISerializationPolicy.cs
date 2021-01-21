// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

namespace Reaqtor.QueryEngine
{
    /// <summary>
    /// Serialization policy, providing a central location to obtain <see cref="ISerializer"/> instances, for example
    /// when serializing state during a checkpoint, or when deserializing state from a checkpoint where the persisted
    /// state contains a name and version for the serializer that was used to write the state.
    /// </summary>
    public interface ISerializationPolicy
    {
        /// <summary>
        /// Gets a serializer with the specified <paramref name="name"/> and <paramref name="version"/>. This is used
        /// when recovering from a checkpoint, where serializer names and versions are stored in order to locate the
        /// right (de)serializer to use for reading the state.
        /// </summary>
        /// <param name="name">The name of the serializer.</param>
        /// <param name="version">The version of the serializer.</param>
        /// <returns>The serializer instance.</returns>
        ISerializer GetSerializer(string name, Version version);

        /// <summary>
        /// Gets the default serializer. Useful for serialization of new state using the "latest known good" serializer.
        /// </summary>
        /// <returns>The serializer instance.</returns>
        ISerializer GetSerializer();
    }
}
