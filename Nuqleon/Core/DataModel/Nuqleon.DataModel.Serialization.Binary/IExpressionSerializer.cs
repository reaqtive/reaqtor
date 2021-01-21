// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.IO;
using System.Linq.Expressions;

namespace Nuqleon.DataModel.Serialization.Binary
{
    /// <summary>
    /// Interface for serialization and deserialization of expressions.
    /// </summary>
    public interface IExpressionSerializer
    {
        /// <summary>
        /// Deserialize a stream containing serialized expression.
        /// </summary>
        /// <param name="stream">Stream containing serialized expression.</param>
        /// <returns>Deserialized expression.</returns>
        Expression Deserialize(Stream stream);

        /// <summary>
        /// Serialize an expression to a stream.
        /// </summary>
        /// <param name="stream">Stream to contain the serialized expression.</param>
        /// <param name="expression">Expression to serialize.</param>
        void Serialize(Stream stream, Expression expression);
    }
}
