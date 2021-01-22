// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - December 2014 - Created this file.
//

using System.IO;

namespace System.Linq.Expressions.Bonsai.Serialization.Binary
{
#if USE_SLIM
    using Object = System.ObjectSlim;
#endif

    /// <summary>
    /// Interface for object serializers.
    /// </summary>
    public interface IObjectSerializer
    {
        /// <summary>
        /// Serializes the specified object of the given type to the stream.
        /// </summary>
        /// <param name="stream">Stream to serialize the object to.</param>
        /// <param name="type">Type of the object to serialize.</param>
        /// <param name="value">Object to serialize.</param>
        void Serialize(Stream stream, Type type, Object value);

        /// <summary>
        /// Deserializes an object of the specified type from the stream.
        /// </summary>
        /// <param name="stream">Stream to deserialize the object from.</param>
        /// <param name="type">Type of the object to deserialize.</param>
        /// <returns>Deserialized object.</returns>
        Object Deserialize(Stream stream, Type type);
    }
}
