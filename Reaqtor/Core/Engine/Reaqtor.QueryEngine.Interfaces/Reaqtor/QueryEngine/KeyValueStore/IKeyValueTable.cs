// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

namespace Reaqtor.QueryEngine
{
    /// <summary>
    /// A table in a key value store.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in the table.</typeparam>
    /// <typeparam name="TValue">The type of the values in the table.</typeparam>
    public interface IKeyValueTable<TKey, TValue>
    {
        /// <summary>
        /// Enters the table into an open transaction. This means any operations on the table will
        /// be done on the transaction.
        /// </summary>
        /// <param name="transaction">The transaction with which to scope the table.</param>
        /// <returns>The transacted key value table.</returns>
        ITransactedKeyValueTable<TKey, TValue> Enter(IKeyValueStoreTransaction transaction);
    }
}
