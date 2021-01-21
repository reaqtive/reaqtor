// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 07/29/2015 - Initial work on memoization support.
//

using System.Collections.Generic;

namespace System.Memory
{
    /// <summary>
    /// Interface for dictionaries used in caches.
    /// </summary>
    /// <typeparam name="TKey">Type of the keys.</typeparam>
    /// <typeparam name="TValue">Type of the values.</typeparam>
    internal interface ICacheDictionary<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
    {
        /// <summary>
        /// Gets the number of entries in the dictionary.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Gets the values of the entries in the dictionary.
        /// </summary>
        IEnumerable<TValue> Values { get; }

        /// <summary>
        /// Tries to retrieve an entry with the specified <paramref name="key"/> from the dictionary.
        /// If no entry is found, the specified <paramref name="valueFactory"/> is invoked to add a new entry.
        /// </summary>
        /// <param name="key">The key of the entry to look up in the dictionary.</param>
        /// <param name="valueFactory">Value factory to invoke when no entry with the specified <paramref name="key"/> is found.</param>
        /// <returns>The value of the entry with the specified <paramref name="key"/>.</returns>
        TValue GetOrAdd(TKey key, Func<TKey, TValue> valueFactory);

        /// <summary>
        /// Clears the entries in the dictionary.
        /// </summary>
        void Clear();

        /// <summary>
        /// Removes the entry with the specified <paramref name="key"/>.
        /// </summary>
        /// <param name="key">The key of the entry to remove.</param>
        /// <returns>true if the element was found; otherwise, false.</returns>
        bool Remove(TKey key);
    }
}
