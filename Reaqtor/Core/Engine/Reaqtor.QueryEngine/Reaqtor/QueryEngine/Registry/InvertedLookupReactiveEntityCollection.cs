// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace Reaqtor.QueryEngine
{
    /// <summary>
    /// Reactive entity collection supporting an inverted lookup by value.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys.</typeparam>
    /// <typeparam name="TValue">The type of the values.</typeparam>
    /// <remarks>
    /// This functionality is used to look up templates from a given (normalized) expression.
    /// </remarks>
    internal class InvertedLookupReactiveEntityCollection<TKey, TValue> : IInvertedLookupReactiveEntityCollection<TKey, TValue>, IDisposable
    {
        private readonly IReactiveEntityCollection<TKey, TValue> _collection;
        private readonly ConcurrentDictionary<TValue, TKey> _invertedCollection;
        private readonly ReaderWriterLockSlim _lock = new();
        private bool _disposed;

        public InvertedLookupReactiveEntityCollection(IEqualityComparer<TKey> keyComparer, IEqualityComparer<TValue> valueComparer)
        {
            if (valueComparer == null)
                throw new ArgumentNullException(nameof(valueComparer));

            _collection = new ReactiveEntityCollection<TKey, TValue>(keyComparer);
            _invertedCollection = new ConcurrentDictionary<TValue, TKey>(valueComparer);
        }

        public InvertedLookupReactiveEntityCollection(IReactiveEntityCollection<TKey, TValue> collection, IEqualityComparer<TValue> valueComparer)
        {
            if (valueComparer == null)
                throw new ArgumentNullException(nameof(valueComparer));

            _collection = collection ?? throw new ArgumentNullException(nameof(collection));
            _invertedCollection = new ConcurrentDictionary<TValue, TKey>(valueComparer);
        }

        public bool ContainsKey(TKey key) => _collection.ContainsKey(key);

        public bool TryAdd(TKey key, TValue value)
        {
            var invertedAdded = false;
            var added = false;

            // The lock is used to ensure that calls to `TryGetKey` do not
            // occur after a value was added to `_invertedCollection`, but
            // before finding we discover that the key already exists.
            using (new WriteLock(_lock))
            {
                try
                {
                    invertedAdded = _invertedCollection.TryAdd(value, key);
                    if (invertedAdded)
                    {
                        added = _collection.TryAdd(key, value);
                    }
                }
                finally
                {
                    if (invertedAdded && !added)
                    {
                        var removed = _invertedCollection.TryRemove(value, out _);
                        Debug.Assert(removed);
                    }
                }
            }

            return added;
        }

        public bool TryGetValue(TKey key, out TValue value) => _collection.TryGetValue(key, out value);

        public bool TryRemove(TKey key, out TValue value)
        {
            var removed = _collection.TryRemove(key, out value);
            if (removed)
            {
                var valueRemoved = _invertedCollection.TryRemove(value, out _);
                Debug.Assert(valueRemoved);
            }

            return removed;
        }

        public ICollection<TValue> Values => _collection.Values;

        public IEnumerable<TKey> RemovedKeys => _collection.RemovedKeys;

        public void ClearRemovedKeys(IEnumerable<TKey> keys) => _collection.ClearRemovedKeys(keys);

        public void Clear() => _collection.Clear();

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => _collection.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public bool TryGetKey(TValue value, out TKey key)
        {
            // The lock is used to ensure that inverted lookups do not occur
            // while still adding key value pairs to the regular collection.
            using (new ReadLock(_lock))
            {
                return _invertedCollection.TryGetValue(value, out key);
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _lock.Dispose();
                    _collection.Dispose();
                }

                _disposed = true;
            }
        }

        private readonly struct ReadLock : IDisposable
        {
            private readonly ReaderWriterLockSlim _lock;

            public ReadLock(ReaderWriterLockSlim @lock)
            {
                _lock = @lock;
                _lock.EnterReadLock();
            }

            public void Dispose() => _lock.ExitReadLock();
        }

        private readonly struct WriteLock : IDisposable
        {
            private readonly ReaderWriterLockSlim _lock;

            public WriteLock(ReaderWriterLockSlim @lock)
            {
                _lock = @lock;
                _lock.EnterWriteLock();
            }

            public void Dispose() => _lock.ExitWriteLock();
        }
    }
}
