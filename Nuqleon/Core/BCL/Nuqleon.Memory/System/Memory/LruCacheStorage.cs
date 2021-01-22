// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   ER - 11/14/2014 - Created this type.
//

using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace System.Memory
{
    /// <summary>
    /// Basic cache storage implementation.
    /// </summary>
    /// <typeparam name="T">Type of the objects kept in the cache.</typeparam>
    public sealed class LruCacheStorage<T> : ICacheStorage<T>, IDisposable
    {
        private readonly int _size;
        private readonly ConcurrentDictionary<T, Entry> _cache;
        private readonly ReaderWriterLockSlim _lock;

        private int _disposed;

        /// <summary>
        /// Initializes the cache storage with a default equality comparer.
        /// </summary>
        public LruCacheStorage(int size)
            : this(size, FastEqualityComparer<T>.Default)
        {
        }

        /// <summary>
        /// Initializes the cache storage with the provided equality comparer.
        /// </summary>
        /// <param name="size">The maximum size of the cache.</param>
        /// <param name="comparer">The equality comparer used to compare cached objects.</param>
        public LruCacheStorage(int size, IEqualityComparer<T> comparer)
        {
            if (size < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(size));
            }
            if (comparer == null)
            {
                throw new ArgumentNullException(nameof(comparer));
            }

            _size = size;
            _cache = new ConcurrentDictionary<T, Entry>(comparer);
            _lock = new ReaderWriterLockSlim();
        }


        /// <summary>
        /// Gets the current number of entries stored in the cache.
        /// </summary>
        public int Count => _cache.Count;

        /// <summary>
        /// Gets a reference to a cache entry.
        /// </summary>
        /// <param name="value">A value to get a reference for.</param>
        /// <returns>A reference to a cache entry.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the provided value is null.</exception>
        public IReference<T> GetEntry(T value)
        {
            CheckDisposed();

            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            var entry = default(Entry);

            _lock.EnterReadLock();

            try
            {
                while (true)
                {
                    if (!_cache.TryGetValue(value, out entry))
                    {
                        entry = new Entry(value);
                        if (_cache.TryAdd(value, entry) && entry.TryIncrement())
                        {
                            break;
                        }
                    }
                    else if (entry.TryIncrement())
                    {
                        break;
                    }
                }
            }
            finally
            {
                _lock.ExitReadLock();
            }

            Prune();

            return entry;
        }

        /// <summary>
        /// Releases a reference to a cache entry.
        /// </summary>
        /// <param name="entry">The reference to release.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if the provided entry is null.
        /// </exception>
        public void ReleaseEntry(IReference<T> entry)
        {
            CheckDisposed();

            if (entry == null)
            {
                throw new ArgumentNullException(nameof(entry));
            }
            if (entry.Value == null)
            {
                throw new ArgumentException("Value contained in the entry cannot be null.", nameof(entry));
            }

            var cached = default(Entry);

            _lock.EnterReadLock();

            try
            {
                if (_cache.TryGetValue(entry.Value, out cached) && ReferenceEquals(entry, cached))
                {
                    try
                    {
                        // Purposefully empty; we need to make sure there is no interruption
                        // between decrementing the reference count and removing the entry.
                    }
                    finally
                    {
                        if (cached.Decrement() == 0)
                        {
#if DEBUG
                            var removed = _cache.TryRemove(entry.Value, out cached);
                            Debug.Assert(removed);
#else
                            _cache.TryRemove(entry.Value, out cached);
#endif
                        }
                    }
                }
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

        private void Prune()
        {
            if (_cache.Count > _size)
            {
                _lock.EnterWriteLock();
                try
                {
                    if (_cache.Count > _size)
                    {
                        foreach (var value in _cache.Values.OrderBy(entry => entry.LastAccess).Take(_cache.Count - _size))
                        {
#if DEBUG
                            var removed = _cache.TryRemove(value.Value, out _);
                            Debug.Assert(removed);
#else
                            _cache.TryRemove(value.Value, out _);
#endif
                        }
                    }
                }
                finally
                {
                    _lock.ExitWriteLock();
                }
            }
        }

        private sealed class Entry : IReference<T>
        {
            private static readonly Stopwatch s_stopwatch = Stopwatch.StartNew();

            private readonly object _gate = new();
            private int _count;

#if DEBUG
#pragma warning disable IDE0052 // Remove unread private members
            private readonly StackTrace _stackTrace;
#pragma warning restore IDE0052 // Remove unread private members
#endif

            public Entry(T value)
            {
                Value = value;
                _count = -1;

#if DEBUG
                _stackTrace = new StackTrace(1, fNeedFileInfo: false);
#endif
            }

            public T Value { get; private set; }

            public long LastAccess { get; private set; }

            public bool TryIncrement()
            {
                lock (_gate)
                {
                    if (_count is -1 or not 0)
                    {
                        _count = _count > 0 ? _count + 1 : 1;
                        LastAccess = s_stopwatch.ElapsedTicks;
                        return true;
                    }
                }

                return false;
            }

            public int Decrement()
            {
                lock (_gate)
                {
                    Debug.Assert(_count >= 1);
                    return --_count;
                }
            }
        }

        /// <summary>
        /// Disposes the cache storage instance.
        /// </summary>
        public void Dispose()
        {
            if (Interlocked.Exchange(ref _disposed, 1) == 0)
            {
                _lock.Dispose();
            }
        }

        private void CheckDisposed()
        {
            if (Volatile.Read(ref _disposed) == 1)
            {
                throw new ObjectDisposedException("this");
            }
        }
    }
}
