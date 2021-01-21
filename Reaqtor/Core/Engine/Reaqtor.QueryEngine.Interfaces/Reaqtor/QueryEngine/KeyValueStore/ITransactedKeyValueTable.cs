// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

namespace Reaqtor.QueryEngine
{
    /// <summary>
    /// A key value table that operates in the scope of a transaction. For example, if the transaction
    /// has snapshot isolation, this acts on a snapshot of the table. Note that the implementation of this data 
    /// structure is not required to be thread safe. Concurrent reads and/or writes may corrupt the invariants of the
    /// implementation.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    public interface ITransactedKeyValueTable<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
    {
        /// <summary>
        /// Gets the value for the given key transactionally.
        /// </summary>
        /// <param name="key">The key whose value is to be got.</param>
        /// <returns>The value for the key.</returns>
        /// <throws>An <see cref="System.ArgumentException" /> if the key does not exist.</throws>
        TValue this[TKey key] { get; }

        /// <summary>
        /// Add an item into a key value store transactionally.
        /// </summary>
        /// <param name="key">The key to insert.</param>
        /// <param name="value">The value associated with the key.</param>
        /// <throws>An <see cref="System.ArgumentException" /> if the key already exists.</throws>
        void Add(TKey key, TValue value);

        /// <summary>
        /// Queries the table transactionally to determine whether a key exists.
        /// </summary>
        /// <param name="key">The key for which to query.</param>
        /// <returns>True if the key is found; false if it is not.</returns>
        bool Contains(TKey key);

        /// <summary>
        /// Updates the value for the given key transactionally.
        /// </summary>
        /// <param name="key">The key to update.</param>
        /// <param name="value">The updated value.</param>
        /// <throws>An <see cref="System.ArgumentException" /> if the key does not exist.</throws>
        void Update(TKey key, TValue value);

        /// <summary>
        /// Removes a key-value pair transactionally.
        /// </summary>
        /// <param name="key">The key to remove.</param>
        /// <throws>An <see cref="System.ArgumentException" /> if the key does not exist.</throws>
        void Remove(TKey key);
    }
}
