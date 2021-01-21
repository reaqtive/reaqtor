// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2018
//

namespace Reaqtive.Serialization
{
    /// <summary>
    /// Interface representing a factory for strongly typed serializers.
    /// </summary>
    public interface ISerializerFactory
    {
        /// <summary>
        /// Gets a serializer for objects of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of the objects to serialize.</typeparam>
        /// <returns>A serializer for objects of type <typeparamref name="T"/>.</returns>
        ISerializer<T> GetSerializer<T>();
    }
}
