// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

namespace Reaqtor.Remoting.Protocol
{
    public interface ITransactionalKeyValueStoreConnection
    {
        long CreateTransaction();

        /// <summary>
        /// Gets the value for the given key transactionally.
        /// </summary>
        /// <param name="tableName">The name of the table to access.</param>
        /// <param name="key">The key whose value is to be got.</param>
        /// <returns>The value for the key.</returns>
        /// <throws>An <see cref="System.ArgumentException" /> if the key does not exist.</throws>
        byte[] this[long transactionId, string tableName, string key] { get; }

        /// <summary>
        /// Add an item into a key value store transactionally.
        /// </summary>
        /// <param name="tableName">The name of the table to insert into.</param>
        /// <param name="key">The key to insert.</param>
        /// <param name="value">The value associated with the key.</param>
        /// <throws>An <see cref="System.ArgumentException" /> if the key already exists.</throws>
        void Add(long transactionId, string tableName, string key, byte[] value);

        /// <summary>
        /// Queries the table transactionally to determine whether a key exists.
        /// </summary>
        /// <param name="tableName">The name of the table to query.</param>
        /// <param name="key">The key for which to query.</param>
        /// <returns>True if the key is found; false if it is not.</returns>
        bool Contains(long transactionId, string tableName, string key);

        /// <summary>
        /// Updates the value for the given key transactionally.
        /// </summary>
        /// <param name="tableName">The name of the table to update.</param>
        /// <param name="key">The key to update.</param>
        /// <param name="value">The updated value.</param>
        /// <throws>An <see cref="System.ArgumentException" /> if the key does not exist.</throws>
        void Update(long transactionId, string tableName, string key, byte[] value);

        /// <summary>
        /// Removes a key-value pair transactionally.
        /// </summary>
        /// <param name="tableName">The name of the table from which to remove.</param>
        /// <param name="key">The key to remove.</param>
        /// <throws>An <see cref="System.ArgumentException" /> if the key does not exist.</throws>
        void Remove(long transactionId, string tableName, string key);

        /// <summary>
        /// Gets an enumerator for the table.
        /// </summary>
        /// <param name="tableName">The table which is to be enumerated.</param>
        /// <returns>An enumerator that will enumerate the table.</returns>
        List<KeyValuePair<string, byte[]>> GetEnumerator(long transactionId, string tableName);

        /// <summary>
        /// Commits the transaction.
        /// </summary>
        /// <param name="token">The token to commit operation.</param>
        /// <returns>A task representing the eventual completion of the commit,</returns>
        void Commit(long transactionId);

        /// <summary>
        /// Cleans up intermediate state and partially committed data.
        /// </summary>
        void Rollback(long transactionId);

        void Dispose(long transactionId);

        byte[] SerializeStore();

        void DeserializeStore(byte[] bytes);

        void Clear();
    }
}
