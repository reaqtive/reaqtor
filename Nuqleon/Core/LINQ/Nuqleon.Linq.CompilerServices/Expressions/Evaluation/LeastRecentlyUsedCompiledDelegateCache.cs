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
using System.Memory;

namespace System.Linq.Expressions
{
    /// <summary>
    /// Compiled delegate cache with a least recently used (LRU) eviction policy.
    /// </summary>
    public class LeastRecentlyUsedCompiledDelegateCache : ICompiledDelegateCache, IClearable
    {
        private readonly int _capacity;
        private readonly Dictionary<ExpressionWithHashCode, CacheEntry> _cache;
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
            _cache = new Dictionary<ExpressionWithHashCode, CacheEntry>(Math.Max(capacity, 16));
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

            //
            // PERF: We short-circuit a few commonly hit paths over here to avoid running too much code under the
            //       lock. There are two parts to this:
            //
            //       1. Computing the hash code outside the lock and saving it in ExpressionWithHashCode. The
            //          default equality comparer will dispatch to ExpressionWithHashCode.GetHashCode() and just
            //          get the precomputed value back. This hash code is used multiple times below due to the
            //          double-checked locking pattern:
            //
            //            - TryGetValue during the first "fast path" check.
            //            - TryAdd during the "slow path" to add to the cache, or, on downlevel platforms:
            //              - ContainsKey to perform the second check, and,
            //              - indexer assignment to insert the entry.
            //
            //       2. If there's a hit during TryGetValue, we still need to run Equals, which short-circuits
            //          for reference equality. If the slow path is taken, we still run the recursive equality
            //          check under the lock.
            //

            var key = new ExpressionWithHashCode(expression);

            var cacheEntry = default(CacheEntry);

            // PERF: consider a lock-free implementation
            lock (_cache)
            {
                if (_cache.TryGetValue(key, out cacheEntry)) // PERF: expensive comparer (1)
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
                Expression = key,
                Delegate = expression.Compile(),
            };

            lock (_cache)
            {
#if NET6_0 || NETSTANDARD3_1
                if (_cache.TryAdd(key, cacheEntry)) // PERF: expensive comparer (2)
                {
#else
                if (!_cache.ContainsKey(key)) // PERF: expensive comparer (2)
                {
                    _cache[key] = cacheEntry; // PERF: expensive comparer (3)
#endif

                    OnAdded(expression, cacheEntry.Delegate);

                    var entry = _lru.AddFirst(cacheEntry);
                    cacheEntry.Entry = entry;

                    if (_cache.Count > _capacity)
                    {
                        var victim = _lru.Last.Value;
                        _cache.Remove(victim.Expression); // PERF: expensive comparer (3 or 4)
                        _lru.RemoveLast();

                        OnEvicted(victim.Expression.Expression, victim.Delegate);
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
            public ExpressionWithHashCode Expression;
            public Delegate Delegate;
            public LinkedListNode<CacheEntry> Entry;
        }

        private sealed class FastExpressionEqualityComparer : IEqualityComparer<Expression>
        {
            public static readonly FastExpressionEqualityComparer Instance = new();

            private readonly ExpressionEqualityComparer _comparer = new();

            private FastExpressionEqualityComparer() { }

            public bool Equals(Expression x, Expression y) => ReferenceEquals(x, y) || _comparer.Equals(x, y);

            public int GetHashCode(Expression obj) => _comparer.GetHashCode(obj);
        }

        private readonly struct ExpressionWithHashCode : IEquatable<ExpressionWithHashCode>
        {
            private readonly int _hashCode;

            public ExpressionWithHashCode(LambdaExpression expression)
            {
                Expression = expression;
                _hashCode = FastExpressionEqualityComparer.Instance.GetHashCode(expression);
            }

            public LambdaExpression Expression { get; }

            public override bool Equals(object obj) => obj is ExpressionWithHashCode e && Equals(e);

            public bool Equals(ExpressionWithHashCode other) => FastExpressionEqualityComparer.Instance.Equals(Expression, other.Expression);

            public override int GetHashCode() => _hashCode;
        }
    }
}
