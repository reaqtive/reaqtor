// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

namespace Reaqtor.QueryEngine
{
    /// <summary>
    /// A transaction for a key value store.
    /// </summary>
    public interface IKeyValueStoreTransaction : ITransaction
    {
#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1819 // Properties should not return arrays. (By design for store.)

        /// <summary>
        /// Gets the value for the given key transactionally.
        /// </summary>
        /// <param name="tableName">The name of the table to access.</param>
        /// <param name="key">The key whose value is to be got.</param>
        /// <returns>The value for the key.</returns>
        /// <throws>An <see cref="System.ArgumentException" /> if the key does not exist.</throws>
        byte[] this[string tableName, string key] { get; }

#pragma warning restore CA1819
#pragma warning restore IDE0079

        /// <summary>
        /// Add an item into a key value store transactionally.
        /// </summary>
        /// <param name="tableName">The name of the table to insert into.</param>
        /// <param name="key">The key to insert.</param>
        /// <param name="value">The value associated with the key.</param>
        /// <throws>An <see cref="System.ArgumentException" /> if the key already exists.</throws>
        void Add(string tableName, string key, byte[] value);

        /// <summary>
        /// Queries the table transactionally to determine whether a key exists.
        /// </summary>
        /// <param name="tableName">The name of the table to query.</param>
        /// <param name="key">The key for which to query.</param>
        /// <returns>True if the key is found; false if it is not.</returns>
        bool Contains(string tableName, string key);

        /// <summary>
        /// Updates the value for the given key transactionally.
        /// </summary>
        /// <param name="tableName">The name of the table to update.</param>
        /// <param name="key">The key to update.</param>
        /// <param name="value">The updated value.</param>
        /// <throws>An <see cref="System.ArgumentException" /> if the key does not exist.</throws>
        void Update(string tableName, string key, byte[] value);

        /// <summary>
        /// Removes a key-value pair transactionally.
        /// </summary>
        /// <param name="tableName">The name of the table from which to remove.</param>
        /// <param name="key">The key to remove.</param>
        /// <throws>An <see cref="System.ArgumentException" /> if the key does not exist.</throws>
        void Remove(string tableName, string key);

        /// <summary>
        /// Gets an enumerator for the table.
        /// </summary>
        /// <param name="tableName">The table which is to be enumerated.</param>
        /// <returns>An enumerator that will enumerate the table.</returns>
        IEnumerator<KeyValuePair<string, byte[]>> GetEnumerator(string tableName);
    }
}
