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
    /// Interface representing a serializer for objects of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of the objects to serialize.</typeparam>
    public interface ISerializer<in T>
    {
        /// <summary>
        /// Serializes the specified <paramref name="value"/> into the specified <paramref name="stream"/>.
        /// </summary>
        /// <param name="value">The value to serialize.</param>
        /// <param name="stream">The stream to serialize the value to.</param>
        /// <exception cref="ArgumentNullException"><paramref name="stream"/> is <c>null</c>.</exception>
        void Serialize(T value, Stream stream);
    }
}
