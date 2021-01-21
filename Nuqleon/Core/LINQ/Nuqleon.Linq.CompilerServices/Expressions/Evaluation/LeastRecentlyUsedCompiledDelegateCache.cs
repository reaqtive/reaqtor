// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - December 2013 - Created this file.
//

using System.Collections.Generic;
using System.Linq.CompilerServices;

namespace System.Linq.Expressions
{
    /// <summary>
    /// Compiled delegate cache with a least recently used (LRU) eviction policy.
    /// </summary>
    public class LeastRecentlyUsedCompiledDelegateCache : ICompiledDelegateCache
    {
        private readonly int _capacity;
        private readonly Dictionary<LambdaExpression, CacheEntry> _cache;
        private readonly LinkedList<CacheEntry> _lru;

        /// <summary>
        /// Creates a new compiled delegate cache with a least recently used (LRU) eviction policy.
        /// </summary>
        /// <param name="capacity">Maximum capacity of the cache.</param>
        public LeastRecentlyUsedCompiledDelegateCache(int capacity)
        {
            if (capacity < 1)
                throw new ArgumentOutOfRangeException(nameof(capacity), "The specified capacity should be >= 1.");

            _capacity = capacity;
            _cache = new Dictionary<LambdaExpression, CacheEntry>(Math.Max(capacity, 16), new ExpressionEqualityComparer());
            _lru = new LinkedList<CacheEntry>();
        }

        /// <summary>
        /// Gets the number of entries in the cache.
        /// </summary>
        public int Count
        {
            get
            {
                lock (_cache)
                {
                    return _cache.Count;
                }
            }
        }

        /// <summary>
        /// Event raised when a cache hit occurs.
        /// </summary>
        public event EventHandler<CacheEventArgs> Hit;

        /// <summary>
        /// Event raised when a cache addition occurs.
        /// </summary>
        public event EventHandler<CacheEventArgs> Added;

        /// <summary>
        /// Event raised when a cache eviction occurs.
        /// </summary>
        public event EventHandler<CacheEventArgs> Evicted;

        /// <summary>
        /// Clears the cache.
        /// </summary>
        public void Clear()
        {
            lock (_cache)
            {
                _cache.Clear();
                _lru.Clear();
            }
        }

        /// <summary>
        /// Gets a compiled delegate from the cache if the specified lambda expression already has been compiled.
        /// Otherwise, compiles the lambda expression to a delegate and stores the result.
        /// </summary>
        /// <param name="expression">Lambda expression to look up in the cache.</param>
        /// <returns>Compiled delegate to execute the lambda expression.</returns>
        public Delegate GetOrAdd(LambdaExpression expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            var cacheEntry = default(CacheEntry);

            // PERF: consider a lock-free implementation
            lock (_cache)
            {
                if (_cache.TryGetValue(expression, out cacheEntry)) // PERF: expensive comparer (1)
                {
                    OnHit(expression, cacheEntry.Delegate);

                    if (_lru.First != cacheEntry.Entry)
                    {
                        _lru.Remove(cacheEntry.Entry);
                        _lru.AddFirst(cacheEntry.Entry);
                    }

                    return cacheEntry.Delegate;
                }
            }

            cacheEntry = new CacheEntry()
            {
                Lambda = expression,
                Delegate = expression.Compile(),
            };

            lock (_cache)
            {
                if (!_cache.ContainsKey(expression)) // PERF: expensive comparer (2)
                {
                    OnAdded(expression, cacheEntry.Delegate);

                    _cache[expression] = cacheEntry; // PERF: expensive comparer (3)

                    var entry = _lru.AddFirst(cacheEntry);
                    cacheEntry.Entry = entry;

                    if (_cache.Count > _capacity)
                    {
                        var victim = _lru.Last;
                        _cache.Remove(victim.Value.Lambda); // PERF: expensive comparer (4)
                        _lru.RemoveLast();

                        OnEvicted(victim.Value.Lambda, victim.Value.Delegate);
                    }
                }
            }

            return cacheEntry.Delegate;
        }

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1716 // Conflict with reserved language keyword. (Delegate is appropriate here.)

        /// <summary>
        /// Raises the Hit event with the specified arguments.
        /// </summary>
        /// <param name="expression">Expression found in the cache.</param>
        /// <param name="delegate">Compiled delegate of the expression.</param>
        protected virtual void OnHit(LambdaExpression expression, Delegate @delegate) => Hit?.Invoke(this, new CacheEventArgs(expression, @delegate));

        /// <summary>
        /// Raises the Added event with the specified arguments.
        /// </summary>
        /// <param name="expression">Expression found in the cache.</param>
        /// <param name="delegate">Compiled delegate of the expression.</param>
        protected virtual void OnAdded(LambdaExpression expression, Delegate @delegate) => Added?.Invoke(this, new CacheEventArgs(expression, @delegate));

        /// <summary>
        /// Raises the Evicted event with the specified arguments.
        /// </summary>
        /// <param name="expression">Expression found in the cache.</param>
        /// <param name="delegate">Compiled delegate of the expression.</param>
        protected virtual void OnEvicted(LambdaExpression expression, Delegate @delegate) => Evicted?.Invoke(this, new CacheEventArgs(expression, @delegate));

#pragma warning restore CA1716
#pragma warning restore IDE0079

        private sealed class CacheEntry
        {
            public LambdaExpression Lambda;
            public Delegate Delegate;
            public LinkedListNode<CacheEntry> Entry;
        }
    }
}
