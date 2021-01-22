// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   ER - 11/14/2014 - Generated the code in this file.
//

namespace System.Memory
{
    /// <summary>
    /// Interface for reference counting storage underlying a cache.
    /// </summary>
    /// <typeparam name="T">Type of objects in the cache.</typeparam>
    public interface ICacheStorage<T>
    {
        /// <summary>
        /// Gets the current number of entries stored in the cache.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Gets the cache entry for the provided value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The cache entry.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// Thrown if the provided value is null.
        /// </exception>
        IReference<T> GetEntry(T value);

        /// <summary>
        /// Releases a cache entry.
        /// </summary>
        /// <param name="entry">The entry to release.</param>
        /// <exception cref="System.ArgumentNullException">
        /// Thrown if the provided entry is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// Thrown if the value contained in the entry is null.
        /// </exception>
        void ReleaseEntry(IReference<T> entry);
    }
}
