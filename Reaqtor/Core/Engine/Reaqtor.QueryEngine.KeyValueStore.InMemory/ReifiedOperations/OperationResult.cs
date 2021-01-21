// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

namespace Reaqtor.QueryEngine.KeyValueStore.InMemory
{
    /// <summary>
    /// Base class to represent the result of applying a reified operation to a key/value store instance.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    public abstract class OperationResult<TKey, TValue>
    {
        /// <summary>
        /// Gets the exception thrown by the operation, if any.
        /// </summary>
        public abstract Exception Exception { get; }

        /// <summary>
        /// Gets the object returned by the operation, if successful.
        /// </summary>
        /// <remarks>
        /// The return type of this property is weakly typed; the type of the result depends on the operation.
        /// </remarks>
        public abstract object Result { get; }

        /// <summary>
        /// Checks if the result is equal to the given object.
        /// </summary>
        /// <param name="obj">The object to compare with.</param>
        /// <returns>true if equal; otherwise, false.</returns>
        public abstract override bool Equals(object obj);

        /// <summary>
        /// Gets a hash code for the current instance.
        /// </summary>
        /// <returns>A hash code.</returns>
        public abstract override int GetHashCode();
    }
}
