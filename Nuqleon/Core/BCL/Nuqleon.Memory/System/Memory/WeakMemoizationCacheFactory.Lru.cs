// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 07/29/2015 - Initial work on memoization support.
//

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

#if DEBUG
using System.Diagnostics;
#endif

namespace System.Memory
{
    public partial class WeakMemoizationCacheFactory
    {
        /// <summary>
        /// Implementation of a factory for weak memoization caches with an LRU cache eviction strategy.
        /// </summary>
        private sealed class LruImpl : IWeakMemoizationCacheFactory
        {
            private readonly int _maxCapacity;

            /// <summary>
            /// Creates a new LRU cache with the specified maximum capacity.
            /// </summary>
            /// <param name="maxCapacity">The maximum capacity of the cache.</param>
            public LruImpl(int maxCapacity) => _maxCapacity = maxCapacity;

            /// <summary>
            /// Creates a memoization cache for the specified <paramref name="function"/> that doesn't keep cache entry keys alive.
            /// </summary>
            /// <typeparam name="T">Type of the memoization cache entry keys. This type has to be a reference type.</typeparam>
            /// <typeparam name="R">Type of the memoization cache entry values.</typeparam>
            /// <param name="function">The function to memoize.</param>
            /// <param name="options">Flags to influence the memoization behavior.</param>
            /// <returns>An empty memoization cache instance.</returns>
            public IMemoizationCache<T, R> Create<T, R>(Func<T, R> function, MemoizationOptions options) where T : class
            {
                if (function == null)
                    throw new ArgumentNullException(nameof(function));

                var cacheError = (options & MemoizationOptions.CacheException) > MemoizationOptions.None;
                return new Cache<T, R>(function, _maxCapacity, cacheError);
            }

            private sealed class Cache<T, R> : MemoizationCacheBase<T, R>, IServiceProvider
                where T : class
            {
                //
                // The cache consists of a ConditionalWeakTable<T, Entry> where T contains the
                // function argument and is not kept alive by the cache. Each entry is a node
                // in a double-linked list that acts as the LRU list. The first element of the
                // list represents the most recently accessed entry. Cache eviction starts from
                // the tail of the list. Each list entry contains the result of evaluation the
                // function with the corresponding argument, as well as a weak reference to the
                // argument. This allows the cache eviction logic using the LRU list traversal
                // to remove the corresponding cache entry, without keeping the argument alive.
                //

                private readonly ConditionalWeakTable<T, ILruCacheEntry<WeakReference<T>, R>>.CreateValueCallback _function;
                private readonly IWeakCacheDictionary<T, ILruCacheEntry<WeakReference<T>, R>> _cache;
                private readonly ReaderWriterLockSlim _lock;
                private readonly int _maxCapacity;
                private readonly bool _cacheError;

                private volatile int _count;

                //
                // We embed a linked list to save on the costs of additional pointers:
                //
                //  - 1 pointer to the linked list
                //  - N pointers to the containing list in each list node
                //
                private LruLinkedList<ILruCacheEntry<WeakReference<T>, R>> _lruList;

#if DEBUG
                private int _invocationCount;
                private int _accessCount;
                private int _evictionCount;
                private ILruCacheEntry<WeakReference<T>, R> _lastEvicted;
#endif

                public Cache(Func<T, R> function, int maxCapacity, bool cacheError)
                {
                    _function = args =>
                    {
                        var weakArgs = WeakReferenceExtensions.Create(args);

#if DEBUG
                        var invokeDuration = default(TimeSpan);
                        Interlocked.Increment(ref _invocationCount);
#endif
                        Trim();

                        var res = default(ILruCacheEntry<WeakReference<T>, R>);
                        try
                        {
#if DEBUG
                            var swInvoke = Stopwatch.StartNew();
#endif
                            var value = function(args);
#if DEBUG
                            invokeDuration = swInvoke.Elapsed;
#endif
                            res = new LruValueCacheEntry<WeakReference<T>, R>(weakArgs, value);
                        }
                        catch (Exception ex) when (_cacheError)
                        {
                            res = new LruErrorCacheEntry<WeakReference<T>, R>(weakArgs, ex);
                        }

                        Interlocked.Increment(ref _count);
#if DEBUG
                        res.GetMetrics().InvokeDuration = invokeDuration;
#endif
                        return res;
                    };

                    _cache = new WeakCacheDictionary<T, ILruCacheEntry<WeakReference<T>, R>>();
                    _lock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);
                    _maxCapacity = maxCapacity;
                    _cacheError = cacheError;
                }

                protected override int CountCore => _count;

                [ExcludeFromCodeCoverage]
                protected override string DebugViewCore
                {
                    get
                    {
                        var sb = new StringBuilder();

                        sb.AppendLine("Entries");
                        sb.AppendLine("-------");
                        sb.AppendLine();

                        if (_count == 0)
                        {
                            sb.AppendLine("  (empty)");
                        }
                        else
                        {
                            var node = _lruList.First;
                            while (node != null)
                            {
                                if (node.Key.TryGetTarget(out T key))
                                {
                                    var value = node is ErrorCacheEntry<WeakReference<T>, R> err ? err.Exception : (object)node.Value;
                                    sb.AppendFormat(CultureInfo.InvariantCulture, "  {0} -> {1}", key, value);
                                    sb.AppendLine();

#if DEBUG
                                    node.GetMetrics().ToDebugView(sb, "    ");
                                    sb.AppendLine();
#endif
                                }
                                else
                                {
                                    sb.AppendLine("  (empty slot)");
                                }

                                node = node.Next;
                            }

#if DEBUG
                            //
                            // CONSIDER: add statistical information about invoke / hit / access / speedup
                            //
                            sb.AppendLine("Summary");
                            sb.AppendLine("-------");
                            sb.AppendLine();
                            sb.AppendLine("  Invocation count = " + _invocationCount);
                            sb.AppendLine("  Access count     = " + _accessCount);
                            sb.AppendLine("  Eviction count   = " + _evictionCount);

                            if (_lastEvicted != null)
                            {
                                sb.AppendLine();
                                sb.AppendLine("  Last eviction");
                                _lastEvicted.GetMetrics().ToDebugView(sb, "    ");
                            }

                            sb.AppendLine();
#endif
                        }

                        return sb.ToString();
                    }
                }

                protected override R GetOrAddCore(T args)
                {
                    var entry = default(ILruCacheEntry<WeakReference<T>, R>);

                    //
                    // NB: CWT already has a fat lock inside of it. We're putting the extra lock here to perform Trim activities, which
                    //     need to maintain the LRU cache.
                    //
                    _lock.EnterUpgradeableReadLock();
                    try
                    {
#if DEBUG
                        var swTotal = Stopwatch.StartNew();
                        Interlocked.Increment(ref _accessCount);
#endif
                        //
                        // NB: CWT does not call the function under its internal lock.
                        //
                        entry = _cache.GetOrAdd(args, _function);
#if DEBUG
                        var keys = _cache.Keys;
                        Debug.Assert(_count == keys.Count);
#endif
                        MostRecent(entry);
#if DEBUG
                        var metrics = entry.GetMetrics();
                        lock (metrics)
                        {
                            metrics.HitCount++;
                            metrics.TotalDuration += swTotal.Elapsed;
                        }
#endif
                    }
                    finally
                    {
                        _lock.ExitUpgradeableReadLock();
                    }

                    return entry.Value;
                }

                /// <summary>
                /// Makes the specified node the most recently accessed one by moving it to the front of the LRU list.
                /// </summary>
                /// <param name="node">The node that was most recently accessed.</param>
                private void MostRecent(ILruCacheEntry<WeakReference<T>, R> node)
                {
                    //
                    // NB: We opted for a linked list approach for the LRU cache to have constant overhead for memoized function
                    //     invocation, with a minor increment upon cache pruning. Alternatively, we could sort entries by their
                    //     last access time, having less overhead during a lookup (just store the new access time) but at the
                    //     expensive of having to sort the entries in the Trim procedure. That'd cause a hiccup in lookup times.
                    //
                    // NB: If a ranking based eviction is desirable, resulting in higher lookup speeds but lower pruning speeds,
                    //     one can use the CreateEvictedBy* methods on WeakMemoizationCacheFactory.
                    //
                    _lock.EnterWriteLock();
                    try
                    {
                        while (_lruList.Last != null && !_lruList.Last.Key.TryGetTarget(out _))
                        {
                            LruLinkedList.RemoveLast(ref _lruList);
                            Interlocked.Decrement(ref _count);
                        }

                        LruLinkedList.MoveFirst(ref _lruList, node);
                    }
                    finally
                    {
                        _lock.ExitWriteLock();
                    }
                }

                /// <summary>
                /// Trims the LRU cache if needed.
                /// </summary>
                private void Trim()
                {
                    //
                    // NB: We can have temporary oversubscription during concurrent accesses because we avoid to enter the write lock
                    //     until absolutely necessary, so _cache.Count can be a dirty read.
                    //
                    if (_count >= _maxCapacity)
                    {
                        _lock.EnterWriteLock();
                        try
                        {
                            do
                            {
                                var entry = _lruList.Last;

                                if (entry.Key.TryGetTarget(out T key))
                                {
                                    _cache.Remove(key);
                                }

                                LruLinkedList.RemoveLast(ref _lruList);
                                Interlocked.Decrement(ref _count);
#if DEBUG
                                _lastEvicted = entry;
                                _evictionCount++;
#endif
                            } while (_count >= _maxCapacity);
                        }
                        finally
                        {
                            _lock.ExitWriteLock();
                        }
                    }
                }

                protected override void ClearCore(bool disposing)
                {
                    _lock.EnterWriteLock();
                    try
                    {
                        var node = _lruList.First;
                        while (node != null)
                        {
                            if (node.Key.TryGetTarget(out T key))
                            {
                                _cache.Remove(key);
                            }

                            node = node.Next;
                        }

                        _count = 0;

                        LruLinkedList.Clear(ref _lruList);
#if DEBUG
                        _accessCount = 0;
                        _evictionCount = 0;
#endif
                    }
                    finally
                    {
                        _lock.ExitWriteLock();
                    }
                }

                protected override void DisposeCore()
                {
                    //
                    // NB: This can fail if the cache is in use; notice that the base class does not set
                    //     the disposed flag until DisposeCore has successfully returned, so the caller
                    //     can retry the operation at a later time.
                    //
                    _lock.Dispose();
                }

                public object GetService(Type serviceType)
                {
                    var res = default(object);

                    if (serviceType == typeof(ITrimmable<KeyValuePair<T, R>>))
                    {
                        res = Trimmable.Create<KeyValuePair<T, R>>(shouldTrim => TrimBy(kv => kv.Value.Kind == ValueOrErrorKind.Value, kv => new KeyValuePair<T, R>(kv.Key, kv.Value.Value), shouldTrim));
                    }
                    else if (serviceType == typeof(ITrimmable<KeyValuePair<T, IValueOrError<R>>>))
                    {
                        res = Trimmable.Create<KeyValuePair<T, IValueOrError<R>>>(shouldTrim => TrimBy(kv => true, kv => new KeyValuePair<T, IValueOrError<R>>(kv.Key, kv.Value), shouldTrim));
                    }

                    //
                    // NB: No trim by key or value; those types could unify to the same ITrimmable<>.
                    //     The drawback is that users of N-ary function need to use Args<> types.
                    //

                    return res;
                }

                private int TrimBy<K>(Func<KeyValuePair<T, IValueOrError<R>>, bool> filter, Func<KeyValuePair<T, IValueOrError<R>>, K> selector, Func<K, bool> shouldTrim)
                {
                    var res = 0;

                    _lock.EnterWriteLock();
                    try
                    {
                        for (var node = _lruList.First; node != null; node = node.Next)
                        {
                            var shouldRemove = false;

                            if (node.Key.TryGetTarget(out T key))
                            {
                                var kv = new KeyValuePair<T, IValueOrError<R>>(key, node);
                                if (filter(kv) && shouldTrim(selector(kv)))
                                {
                                    _cache.Remove(key);
                                    shouldRemove = true;
                                }
                            }
                            else
                            {
                                shouldRemove = true;
                            }

                            if (shouldRemove)
                            {
                                LruLinkedList.Remove(ref _lruList, node);
                                Interlocked.Decrement(ref _count);
                                res++;
                            }
                        }
                    }
                    finally
                    {
                        _lock.ExitWriteLock();
                    }

                    return res;
                }
            }
        }
    }
}
