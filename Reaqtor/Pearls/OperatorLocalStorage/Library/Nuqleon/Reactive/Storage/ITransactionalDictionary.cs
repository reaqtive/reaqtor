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
    /// Interface representing a transactional dictionary supporting the creation of snapshots to apply edits to a persistent key/value store.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys stored in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of the values stored in the dictionary.</typeparam>
    internal interface ITransactionalDictionary<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
    {
        /// <summary>
        /// Gets or sets the value of the entry in the dictionary with the specified <paramref name="key"/>.
        /// </summary>
        /// <param name="key">The key whose value to get or set.</param>
        /// <returns>The value of the entry in the dictionary with the specified <paramref name="key"/>.</returns>
        /// <exception cref="KeyNotFoundException">The specified <paramref name="key"/> does not exist when attempting to get the corresponding value.</exception>
        TValue this[TKey key] { get; set; }

        /// <summary>
        /// Adds an entry to the dictionary with the specified <paramref name="key"/> and <paramref name="value"/>.
        /// </summary>
        /// <param name="key">The key of the entry to add.</param>
        /// <param name="value">The value of the entry to add.</param>
        /// <exception cref="InvalidOperationException">A entry with the specified <paramref name="key"/> already exists.</exception>
        void Add(TKey key, TValue value);

        /// <summary>
        /// Checks whether the dictionary contains an entry with the specified <paramref name="key"/>.
        /// </summary>
        /// <param name="key">The key to look up in the dictionary.</param>
        /// <returns><c>true</c> if the dictionary contains an entry with the specified <paramref name="key"/>; otherwise, <c>false</c>.</returns>
        bool ContainsKey(TKey key);

        /// <summary>
        /// Removes the entry with the specified <paramref name="key"/> from the dictionary, if found.
        /// </summary>
        /// <param name="key">The key of the entry to remove from the dictionary.</param>
        /// <returns><c>true</c> if an entry with the specified <paramref name="key"/> was found and removed; otherwise, <c>false</c>.</returns>
        bool Remove(TKey key);

        /// <summary>
        /// Tries to retrieve the value of the entry with the specified <paramref name="key"/>.
        /// </summary>
        /// <param name="key">The key to look up in the dictionary.</param>
        /// <param name="value">The value of the entry, if found.</param>
        /// <returns><c>true</c> if the dictionary contains an entry with the specified <paramref name="key"/>; otherwise, <c>false</c>.</returns>
        bool TryGetValue(TKey key, out TValue value);

        /// <summary>
        /// Creates a snapshot of the dictionary which can be used to apply addition, edit, and deletion operations to a persistent key/value store.
        /// </summary>
        /// <param name="differential">A value indicating whether to return a differential snapshot (<c>true</c>), i.e. one that only contains the edits since the last successful snapshot persistence. Otherwise, a full snapshot is created.</param>
        /// <returns>The snapshot containing the addition, edit, and deletion operations. For more information, see <see cref="ISnapshot{TKey, TValue}"/>.</returns>
        ISnapshot<TKey, TValue> CreateSnapshot(bool differential);
    }
}
