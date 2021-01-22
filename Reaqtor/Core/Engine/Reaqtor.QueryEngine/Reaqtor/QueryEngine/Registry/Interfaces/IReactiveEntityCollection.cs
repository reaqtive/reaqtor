// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Globalization;

namespace Reaqtor.QueryEngine
{
    /// <summary>
    /// Interface for collections containing reactive entities, e.g. used for registries.
    /// </summary>
    /// <typeparam name="TKey">Type of the keys used to locate entities, e.g. strings, GUIDs, URIs, etc.</typeparam>
    /// <typeparam name="TValue">Type of the values used to represent entities.</typeparam>
    internal interface IReactiveEntityCollection<TKey, TValue> : IReadOnlyReactiveEntityCollection<TKey, TValue>, IDisposable
    {
        /// <summary>
        /// Tries to add an entry with the specified <paramref name="key"/> and <paramref name="value"/>.
        /// </summary>
        /// <param name="key">The key of the entry to add.</param>
        /// <param name="value">The value to add.</param>
        /// <returns>true if the entry was added because no existing entry with the specified <paramref name="key"/> exists; otherwise, false.</returns>
        bool TryAdd(TKey key, TValue value);

        /// <summary>
        /// Tries to remove an entry with the specified <paramref name="key"/> if it exists, and returns it values.
        /// </summary>
        /// <param name="key">The key to look up.</param>
        /// <param name="value">The value returned, if an entry was found and removed.</param>
        /// <returns>true if an entry with the specified <paramref name="key"/> was found and removed, and its <paramref name="value"/> was returned; otherwise, false.</returns>
        bool TryRemove(TKey key, out TValue value);

        /// <summary>
        /// Gets a sequence of removed keys.
        /// </summary>
        /// <remarks>
        /// This functionality is used to assist with creating snapshots for purposes of checkpointing.
        /// </remarks>
        IEnumerable<TKey> RemovedKeys { get; }

        /// <summary>
        /// Clears the <see cref="RemovedKeys"/> sequence using the specified <paramref name="keys"/> to remove.
        /// </summary>
        /// <param name="keys">The keys to remove from <see cref="RemovedKeys"/>.</param>
        /// <remarks>
        /// This functionality is used to assist with creating snapshots for purposes of checkpointing. After creating a snapshot of
        /// the <see cref="RemovedKeys"/> and processing these (e.g. by removing entries from a persistent store), the sequence of
        /// removed keys can be pruned (set difference semantics).
        /// </remarks>
        void ClearRemovedKeys(IEnumerable<TKey> keys);

        /// <summary>
        /// Clears the collection.
        /// </summary>
        void Clear();
    }

    /// <summary>
    /// Provides extension methods for <see cref="IReactiveEntityCollection{TKey, TValue}"/>.
    /// </summary>
    internal static class ReactiveEntityCollectionExtensions
    {
        /// <summary>
        /// Adds an entry with the specified <paramref name="key"/> and <paramref name="value"/>. An exception is thrown in an entry already exists.
        /// </summary>
        /// <param name="collection">The collection to add the entry to.</param>
        /// <param name="key">The key of the entry to add.</param>
        /// <param name="value">The value to add.</param>
        /// <exception cref="InvalidOperationException">Thrown if an entry with the specified <paramref name="key"/> already exists.</exception>
        public static void Add<TKey, TValue>(this IReactiveEntityCollection<TKey, TValue> collection, TKey key, TValue value)
        {
            if (!collection.TryAdd(key, value))
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "An entry with key '{0}' already exists.", key));
            }
        }
    }
}
