// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2018
//

using System; // NB: Used for XML doc comments.
using System.Collections.Generic;

namespace Reaqtive.Storage
{
    /// <summary>
    /// Interface representing a persisted dictionary.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in the persisted dictionary.</typeparam>
    /// <typeparam name="TValue">The type of the values in the persisted dictionary.</typeparam>
    public interface IPersistedDictionary<TKey, TValue> : IDictionary<TKey, TValue>, IReadOnlyDictionary<TKey, TValue>, IPersisted
    {
        /// <summary>
        /// Gets or sets the element with the specified key.
        /// </summary>
        /// <param name="key">The key of the element to get or set.</param>
        /// <returns>The element with the specified <paramref name="key"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="key"/> is <c>null</c>.</exception>
        /// <exception cref="KeyNotFoundException">The property is retrieved and <paramref name="key"/> is not found.</exception>
        new TValue this[TKey key] { get; set; }

        /// <summary>
        /// Gets the number of elements in the dictionary.
        /// </summary>
        new int Count { get; }

        /// <summary>
        /// Gets a collection containing the keys in the dictionary.
        /// </summary>
        new IReadOnlyCollection<TKey> Keys { get; }

        /// <summary>
        /// Gets a collection containing the values in the dictionary.
        /// </summary>
        new IReadOnlyCollection<TValue> Values { get; }

        /// <summary>
        /// Determines whether the dictionary contains an element that has the specified <paramref name="key"/>.
        /// </summary>
        /// <param name="key">The key to locate.</param>
        /// <returns><c>true</c> if the dictionary contains an element that has the specified <paramref name="key"/>; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="key"/> is <c>null</c>.</exception>
        new bool ContainsKey(TKey key);

        /// <summary>
        /// Gets the value that is associated with the specified <paramref name="key"/>.
        /// </summary>
        /// <param name="key">The key to locate.</param>
        /// <param name="value">When this method returns, the value associated with the specified <paramref name="key"/>, if the key is found; otherwise, the default value for the type of the value parameter.</param>
        /// <returns><c>true</c> if the dictionary contains an element that has the specified <paramref name="key"/>; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="key"/> is <c>null</c>.</exception>
        new bool TryGetValue(TKey key, out TValue value);
    }
}
