// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace Reaqtor.QueryEngine
{
    /// <summary>
    /// Thread-safe collection for reactive entities.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys.</typeparam>
    /// <typeparam name="TValue">The type of the values.</typeparam>
    internal sealed class ReactiveEntityCollection<TKey, TValue> : IReactiveEntityCollection<TKey, TValue>
    {
        private readonly IEqualityComparer<TKey> _comparer;
        private readonly ConcurrentDictionary<TKey, TValue> _collection;

        // No concurrent hash set, representing with a dictionary of key->().
        // Keep an eye on https://github.com/dotnet/runtime/issues/39919.
        private readonly ConcurrentDictionary<TKey, Empty> _removedKeys;

        /// <summary>
        /// A reader-writer lock to be held during clone operations.
        /// </summary>
        private readonly ReaderWriterLockSlim _lock;

        private bool _disposed;

        /// <summary>
        /// Creates a new reactive entity collection using the specified <paramref name="comparer"/> for keys.
        /// </summary>
        /// <param name="comparer">The comparer to use to check for equality between keys.</param>
        public ReactiveEntityCollection(IEqualityComparer<TKey> comparer)
        {
            _collection = new ConcurrentDictionary<TKey, TValue>(comparer);
            _removedKeys = new ConcurrentDictionary<TKey, Empty>(comparer);
            _comparer = comparer;
            _lock = new ReaderWriterLockSlim();
        }

        public bool ContainsKey(TKey key) => _collection.ContainsKey(key); // No lock required, read-only

        public bool TryAdd(TKey key, TValue value)
        {
            // No lock required, it does not matter whether or not the entry
            // ends up in the cloned collection. If it is not in the cloned
            // collection for an active operation, it will be next time.
            return _collection.TryAdd(key, value);
        }

        public bool TryGetValue(TKey key, out TValue value) => _collection.TryGetValue(key, out value); // No lock required, read-only

        public bool TryRemove(TKey key, out TValue value)
        {
            // Lock required, to prevent the case of an entry appearing in both
            // the cloned collection of entries and the list of removed keys.
            // If the cloning occurs while an entry is being removed, there is
            // some chance that the entries will be cloned before the entry is
            // removed, and then the removed keys will be cloned after the
            // entry key is added to the list of removed keys.
            _lock.EnterReadLock();

            try
            {
                if (_collection.TryRemove(key, out value))
                {
                    _removedKeys.TryAdd(key, value: default);
                    return true;
                }

                return false;
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => _collection.GetEnumerator(); // No lock required, read-only

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator(); // No lock required, read-only

        public ICollection<TValue> Values => _collection.Values; // No lock required, read-only

        public IEnumerable<TKey> RemovedKeys => _removedKeys.Keys; // No lock required, read-only

        public void ClearRemovedKeys(IEnumerable<TKey> keys)
        {
            if (keys == null)
            {
                throw new ArgumentNullException(nameof(keys));
            }

            // Lock technically required, however, the lock is only for issues
            // arising from concurrency between cloning and removal. Generally,
            // this operation (`ClearRemovedKeys`) will not be called
            // concurrently with the clone operation.

            foreach (var key in keys)
            {
                _removedKeys.TryRemove(key, out _);
            }
        }

        public ReadOnlyReactiveEntityCollection<TKey, TValue> Clone()
        {
            // Lock required, prevent any remove operations from occurring
            // while the collection is being cloned.
            _lock.EnterWriteLock();

            try
            {
                return new ReadOnlyReactiveEntityCollection<TKey, TValue>(_collection, _removedKeys, _comparer);
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        public void Clear()
        {
            // Lock technically required, however, the lock is only for issues
            // arising from concurrency between cloning and removal. Generally,
            // this operation (`Clear`) will not be called
            // concurrently with the clone operation.
            _collection.Clear();
            _removedKeys.Clear();
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _lock.Dispose();

                _disposed = true;
            }
        }
    }

    internal struct Empty { }
}
