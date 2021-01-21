// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2018
//

using System.Collections.Generic;

namespace Reaqtive.Storage
{
    /// <summary>
    /// Snapshot visitor that applies the edits in a snapshot to a dictionary instance.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys stored in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of the values stored in the dictionary.</typeparam>
    internal sealed class DictionarySnapshotVisitor<TKey, TValue> : ISnapshotVisitor<TKey, TValue>
    {
        /// <summary>
        /// The dictionary to apply the edits to.
        /// </summary>
        private readonly Dictionary<TKey, TValue> _dictionary;

        /// <summary>
        /// Creates a new instance of <see cref="DictionarySnapshotVisitor{TKey, TValue}"/> which will apply edits to the specified <paramref name="dictionary"/>.
        /// </summary>
        /// <param name="dictionary">The dictionary to apply the edits to.</param>
        public DictionarySnapshotVisitor(Dictionary<TKey, TValue> dictionary)
        {
            _dictionary = dictionary;
        }

        /// <summary>
        /// Adds or updates the dictionary entry with the specified <paramref name="key"/> and <paramref name="value"/>.
        /// </summary>
        /// <param name="key">The key of the dictionary entry to add or edit.</param>
        /// <param name="value">The value of the dictionary entry to add or edit.</param>
        public void AddOrUpdate(TKey key, TValue value) => _dictionary[key] = value;

        /// <summary>
        /// Deletes the dictionary entry with the specified <paramref name="key"/>.
        /// </summary>
        /// <param name="key">The key of the dictionary entry to delete.</param>
        public void Delete(TKey key) => _dictionary.Remove(key);
    }
}
