// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

namespace Reaqtor.QueryEngine
{
    /// <summary>
    /// A persistent transactional key value store. Transactions span all tables.
    /// </summary>
    public interface IKeyValueStore
    {
        /// <summary>
        /// Creates a transaction over the whole store that can be used to scope table in the store.
        /// </summary>
        /// <returns>A transaction for the store.</returns>
        IKeyValueStoreTransaction CreateTransaction();

        /// <summary>
        /// Gets a table by name.
        /// </summary>
        /// <param name="name">The name of the table to get.</param>
        /// <returns>The key value table.</returns>
        IKeyValueTable<string, byte[]> GetTable(string name);
    }
}
