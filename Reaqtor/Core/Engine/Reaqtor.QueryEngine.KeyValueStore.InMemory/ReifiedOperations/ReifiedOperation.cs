// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

namespace Reaqtor.QueryEngine.KeyValueStore.InMemory
{
    /// <summary>
    /// Provides factory methods for reified key/value store operations.
    /// </summary>
    public static class ReifiedOperation
    {
        /// <summary>
        /// Creates a reified operation representing an addition of a key/value pair.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="key">The key to add.</param>
        /// <param name="value">The value to add.</param>
        /// <returns>A reified operation instance representing the addition operation.</returns>
        public static AddOperation<TKey, TValue> Add<TKey, TValue>(TKey key, TValue value) => new(key, value);

        /// <summary>
        /// Creates a reified operation representing the removal of a key/value pair.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="key">The key to remove.</param>
        /// <returns>A reified operation instance representing the removal operation.</returns>
        public static RemoveOperation<TKey, TValue> Remove<TKey, TValue>(TKey key) => new(key);

        /// <summary>
        /// Creates a reified operation representing the retrieval of a key/value pair.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="key">The key to look up.</param>
        /// <returns>A reified operation instance representing the retrieval operation.</returns>
        public static GetOperation<TKey, TValue> Get<TKey, TValue>(TKey key) => new(key);

        /// <summary>
        /// Creates a reified operation representing a check for the existence of a key/value pair.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="key">The key to check for.</param>
        /// <returns>A reified operation instance representing the contains operation.</returns>
        public static ContainsOperation<TKey, TValue> Contains<TKey, TValue>(TKey key) => new(key);

        /// <summary>
        /// Creates a reified operation representing an update to a key/value pair.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="key">The key of the key/value pair to update.</param>
        /// <param name="value">The new value.</param>
        /// <returns>A reified operation instance representing the update operation.</returns>
        public static UpdateOperation<TKey, TValue> Update<TKey, TValue>(TKey key, TValue value) => new(key, value);

        /// <summary>
        /// Creates a reified operation representing an enumeration over the key/value store.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="filter">The filter applied while enumerating over the store.</param>
        /// <returns>A reified operation instance representing the enumeration operation.</returns>
        public static EnumerateOperation<TKey, TValue> GetEnumerator<TKey, TValue>(Func<TKey, bool> filter) => new(filter);
    }
}
