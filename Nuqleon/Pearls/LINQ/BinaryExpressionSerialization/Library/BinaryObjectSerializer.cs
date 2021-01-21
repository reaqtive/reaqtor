// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - December 2014 - Created this file.
//

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

#if USE_SLIM
using System.Reflection;
#endif

namespace System.Linq.Expressions.Bonsai.Serialization.Binary
{
#if USE_SLIM
    using Object = System.ObjectSlim;
#endif

    /// <summary>
    /// Object serializer using the default binary formatter.
    /// </summary>
    public sealed class BinaryObjectSerializer : IObjectSerializer
    {
        private readonly BinaryFormatter _formatter;

        /// <summary>
        /// Creates a new object serializers using the default binary formatter.
        /// </summary>
        public BinaryObjectSerializer()
        {
            _formatter = new BinaryFormatter();
        }

        /// <summary>
        /// Serializes the specified object to the stream.
        /// </summary>
        /// <param name="stream">Stream to serialize the object to.</param>
        /// <param name="type">This parameter is ignored.</param>
        /// <param name="value">Object to serialize.</param>
        public void Serialize(Stream stream, Type type, Object value)
        {
#if USE_SLIM
            _formatter.Serialize(stream, value.Value);
#else
            _formatter.Serialize(stream, value);
#endif
        }

        /// <summary>
        /// Deserializes an object of the specified type from the stream.
        /// </summary>
        /// <param name="stream">Stream to deserialize the object from.</param>
        /// <param name="type">This parameter is ignored.</param>
        /// <returns>Deserialized object.</returns>
        public Object Deserialize(Stream stream, Type type)
        {
#if USE_SLIM
            var res = _formatter.Deserialize(stream);
            var clrType = res?.GetType() ?? typeof(object); // REVIEW
            var slimType = clrType.ToTypeSlim(); // REVIEW
            return Object.Create(res, slimType, clrType);
#else
            return _formatter.Deserialize(stream);
#endif
        }
    }
}
