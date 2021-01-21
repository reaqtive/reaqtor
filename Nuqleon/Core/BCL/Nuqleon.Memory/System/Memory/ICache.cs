// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   ER - 10/27/2014 - Created this type.
//

namespace System.Memory
{
    /// <summary>
    /// Implementation of an object cache.
    /// </summary>
    /// <typeparam name="T">Type of the objects kept in the cache.</typeparam>
    public interface ICache<T>
    {
        /// <summary>
        /// Caches the item and creates a reference to reconstruct the item with.
        /// </summary>
        /// <param name="value">The instance to cache.</param>
        /// <returns>A handle to reconstruct the original instance.</returns>
        /// <remarks>
        /// The lifetime of the item in the cache is tied to the lifetime of the reference.
        /// </remarks>
        IDiscardable<T> Create(T value);
    }
}
