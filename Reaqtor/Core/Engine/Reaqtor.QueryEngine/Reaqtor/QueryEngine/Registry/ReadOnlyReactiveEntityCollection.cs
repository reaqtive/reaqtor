// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace Reaqtor.QueryEngine
{
    /// <summary>
    /// Read-only snapshot of a reactive entity collection.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys.</typeparam>
    /// <typeparam name="TValue">The type of the values.</typeparam>
    internal sealed class ReadOnlyReactiveEntityCollection<TKey, TValue>
    {
        /// <summary>
        /// Creates a snapshot of the specified <paramref name="collection"/>, except for the entries with keys in <paramref name="removedKeys"/>.
        /// </summary>
        /// <param name="collection">The collection to clone.</param>
        /// <param name="removedKeys">The keys to exclude.</param>
        /// <param name="comparer">The compare to use for equality of keys.</param>
        public ReadOnlyReactiveEntityCollection(ConcurrentDictionary<TKey, TValue> collection, ConcurrentDictionary<TKey, Empty> removedKeys, IEqualityComparer<TKey> comparer)
        {
            //
            // First, clone the collection using a regular non-concurrent dictionary, which uses enumeration
            // underneath. Note that we're being called under a reader/writer lock ensuring that no concurrent
            // modifications are made. Therefore, the copy of the dictionary will be consistent.
            //

            var entriesClone = new Dictionary<TKey, TValue>(collection, comparer);

            //
            // Next, clone the removed keys by simply calling the Keys property, which takes a snapshot. This
            // acquires all locks, but no concurrentcy is expected because we're called under an exclusive
            // write lock. The result is a consistent copy of the keys.
            //

            var removedKeysClone = removedKeys.Keys;

            //
            // Wrap the cloned dictionary of the entries in a ReadOnlyDictionary<,> which has the nice property
            // of storing the result returned from calling the underlying Keys and Values properties, thus
            // avoiding multiple allocations of these collections upon repeated access to these properties on
            // the read-only wrapper.
            //

            Entries = new ReadOnlyDictionary<TKey, TValue>(entriesClone);

            //
            // Finally, convert the clone of the removed keys collection to a read-only collection. Given that
            // the ConcurrentDictionary<,> returns a ReadOnlyCollection<> instance already, this will become
            // a simple type conversion rather than a clone.
            //

            RemovedKeys = AsReadOnlyCollection(removedKeysClone);
        }

        private static ReadOnlyCollection<T> AsReadOnlyCollection<T>(IEnumerable<T> source)
        {
            if (source is ReadOnlyCollection<T> c)
            {
                return c;
            }

            Debug.Assert(false, "Potential performance hazard.");

            return new ReadOnlyCollection<T>(source.ToList());
        }

        public ReadOnlyDictionary<TKey, TValue> Entries { get; }

        public ReadOnlyCollection<TKey> RemovedKeys { get; }
    }
}
