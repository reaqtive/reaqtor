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

namespace System.Memory
{
    /// <summary>
    /// Basic cache storage implementation.
    /// </summary>
    /// <typeparam name="T">Type of the objects kept in the cache.</typeparam>
    public sealed class CacheStorage<T> : ICacheStorage<T>
    {
        private readonly ConcurrentDictionary<T, Entry> _cache;

        /// <summary>
        /// Initializes the cache storage with a default equality comparer.
        /// </summary>
        public CacheStorage()
            : this(EqualityComparer<T>.Default)
        {
        }

        /// <summary>
        /// Initializes the cache storage with the provided equality comparer.
        /// </summary>
        /// <param name="comparer">The equality comparer used to compare cached objects.</param>
        public CacheStorage(IEqualityComparer<T> comparer)
        {
            if (comparer == null)
            {
                throw new ArgumentNullException(nameof(comparer));
            }

            _cache = new ConcurrentDictionary<T, Entry>(comparer);
        }

        /// <summary>
        /// Gets the current number of entries stored in the cache.
        /// </summary>
        public int Count => _cache.Count;

        /// <summary>
        /// Gets the cache entry for the provided value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The cache entry.</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if the provided value is null.
        /// </exception>
        public IReference<T> GetEntry(T value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            Entry entry;

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

            return entry;
        }

        /// <summary>
        /// Releases a cache entry.
        /// </summary>
        /// <param name="entry">The entry to release.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if the provided entry is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown if the value contained in the entry is null.
        /// </exception>
        public void ReleaseEntry(IReference<T> entry)
        {
            if (entry == null)
            {
                throw new ArgumentNullException(nameof(entry));
            }
            if (entry.Value == null)
            {
                throw new ArgumentException("Value contained in the entry cannot be null.", nameof(entry));
            }

            if (_cache.TryGetValue(entry.Value, out Entry cached))
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

        private sealed class Entry : IReference<T>
        {
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
                _count = int.MinValue;

#if DEBUG
                _stackTrace = new StackTrace(1, fNeedFileInfo: false);
#endif
            }

            public T Value { get; }

            public bool TryIncrement()
            {
                lock (_gate)
                {
                    if (_count != 0)
                    {
                        Debug.Assert(_count is int.MinValue or > 0);
                        _count = _count != int.MinValue ? _count + 1 : 1;
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
    }
}
