// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

namespace Reaqtor.QueryEngine
{
    /// <summary>
    /// Interface for collections containing reactive entities, e.g. used for registries.
    /// </summary>
    /// <typeparam name="TKey">Type of the keys used to locate entities, e.g. strings, GUIDs, URIs, etc.</typeparam>
    /// <typeparam name="TValue">Type of the values used to represent entities.</typeparam>
    internal interface IReadOnlyReactiveEntityCollection<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
    {
        /// <summary>
        /// Checks if an entry with the specified <paramref name="key"/> exists.
        /// </summary>
        /// <param name="key">The key to check for.</param>
        /// <returns>true if an entry with the specified <paramref name="key"/> exists; otherwise, false.</returns>
        bool ContainsKey(TKey key);

        /// <summary>
        /// Tries to get the value of an entry with the specified <paramref name="key"/> if it exists.
        /// </summary>
        /// <param name="key">The key to look up.</param>
        /// <param name="value">The value returned, if an entry was found.</param>
        /// <returns>true if an entry with the specified <paramref name="key"/> exists and its <paramref name="value"/> was returned; otherwise, false.</returns>
        bool TryGetValue(TKey key, out TValue value);

        /// <summary>
        /// Gets a collection containing all the values in the collection.
        /// </summary>
        ICollection<TValue> Values { get; }
    }
}
