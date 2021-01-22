// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2018
//

using System;
using System.IO;

namespace Reaqtive.Serialization
{
    /// <summary>
    /// Interface representing a deserializer for objects of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of the objects to deserialize.</typeparam>
    public interface IDeserializer<out T>
    {
        /// <summary>
        /// Deserializes an object from the specified <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The stream to deserialize the value from.</param>
        /// <exception cref="ArgumentNullException"><paramref name="stream"/> is <c>null</c>.</exception>
        T Deserialize(Stream stream);
    }
}
