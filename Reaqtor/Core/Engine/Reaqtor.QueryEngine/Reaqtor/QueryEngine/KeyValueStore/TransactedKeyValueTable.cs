// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;

namespace Reaqtor.QueryEngine
{
    /// <summary>
    /// Helper methods for <see cref="ITransactedKeyValueTable{TKey,TValue}" />
    /// </summary>
    public static class TransactedKeyValueTable
    {
        /// <summary>
        /// Gets the value if the the key exists.
        /// </summary>
        /// <typeparam name="TKey">The type of the key in the key value table.</typeparam>
        /// <typeparam name="TValue">The type of the value in the key value table.</typeparam>
        /// <param name="table">The key value table.</param>
        /// <param name="key">The key to lookup.</param>
        /// <param name="value">The value corresponding to the key.</param>
        /// <returns>True if found, false otherwise.</returns>
        public static bool TryGet<TKey, TValue>(this ITransactedKeyValueTable<TKey, TValue> table, TKey key, out TValue value)
        {
            if (table == null)
                throw new ArgumentNullException(nameof(table));

            if (table.Contains(key))
            {
                value = table[key];
                return true;
            }

            value = default;
            return false;
        }

        /// <summary>
        /// Removes the value if the the key exists.
        /// </summary>
        /// <typeparam name="TKey">The type of the key in the key value table.</typeparam>
        /// <typeparam name="TValue">The type of the value in the key value table.</typeparam>
        /// <param name="table">The key value table.</param>
        /// <param name="key">The key to remove.</param>
        /// <returns>True if found, false otherwise.</returns>
        public static bool TryRemove<TKey, TValue>(this ITransactedKeyValueTable<TKey, TValue> table, TKey key)
        {
            if (table == null)
                throw new ArgumentNullException(nameof(table));

            if (table.Contains(key))
            {
                table.Remove(key);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Clears the key value table.
        /// </summary>
        /// <typeparam name="TKey">The type of the key in the key value table.</typeparam>
        /// <typeparam name="TValue">The type of the value in the key value table.</typeparam>
        /// <param name="table">The key value table.</param>
        public static void Clear<TKey, TValue>(this ITransactedKeyValueTable<TKey, TValue> table)
        {
            if (table == null)
                throw new ArgumentNullException(nameof(table));

            var lst = table.ToList();

            foreach (var item in lst)
            {
                table.Remove(item.Key);
            }
        }
    }
}
