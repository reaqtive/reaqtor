// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

namespace System.Runtime.CompilerServices
{
    /// <summary>
    /// Provides extension methods for <see cref="ConditionalWeakTable{TKey, TValue}"/>.
    /// </summary>
    public static class ConditionalWeakTableExtensions
    {
        /// <summary>
        /// Checks whether the conditional weak table contains the specified key.
        /// </summary>
        /// <typeparam name="TKey">Type of the keys stored in the conditional weak table.</typeparam>
        /// <typeparam name="TValue">Type of the elements stored in the conditional weak table.</typeparam>
        /// <param name="table">Conditional weak table to perform the lookup on.</param>
        /// <param name="key">Key to look for.</param>
        /// <returns><c>true</c> if the conditional weak table contains an entry with the specified key; otherwise, <c>false</c>.</returns>
        public static bool ContainsKey<TKey, TValue>(this ConditionalWeakTable<TKey, TValue> table, TKey key)
            where TKey : class
            where TValue : class
        {
            if (table == null)
                throw new ArgumentNullException(nameof(table));

            return table.TryGetValue(key, out _);
        }
    }
}
