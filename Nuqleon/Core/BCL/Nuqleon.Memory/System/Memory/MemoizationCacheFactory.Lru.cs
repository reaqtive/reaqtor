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
using System.Text;

#if DEBUG
using System.Diagnostics;
#endif

namespace System.Memory
{
    public partial class MemoizationCacheFactory
    {
        /// <summary>
        /// Implementation of a factory for memoization caches with an LRU cache eviction strategy.
        /// </summary>
        private sealed class LruImpl : IMemoizationCacheFactory
        {
            private readonly int _maxCapacity;

            /// <summary>
            /// Creates a new LRU cache with the specified maximum capacity.
            /// </summary>
            /// <param name="maxCapacity">The maximum capacity of the cache.</param>
            public LruImpl(int maxCapacity)
            {
                _maxCapacity = maxCapacity;
            }

            /// <summary>
            /// Creates a memoization cache for the specified <paramref name="function"/> using the specified <paramref name="comparer"/> to compare cache entries.
            /// </summary>
            /// <typeparam name="T">Type of the memoization cache entry keys.</typeparam>
            /// <typeparam name="TResult">Type of the memoization cache entry values.</typeparam>
            /// <param name="function">The function to memoize.</param>
            /// <param name="options">Flags to influence the memoization behavior.</param>
            /// <param name="comparer">Comparer to compare the key during lookup in the memoization cache.</param>
            /// <returns>An empty memoization cache instance.</returns>
            public IMemoizationCache<T, TResult> Create<T, TResult>(Func<T, TResult> function, MemoizationOptions options, IEqualityComparer<T> comparer)
            {
                if (function == null)
                    throw new ArgumentNullException(nameof(function));

                var cacheError = (options & MemoizationOptions.CacheException) > MemoizationOptions.None;
                return new Cache<T, TResult>(function, _maxCapacity, comparer, cacheError);
            }

            private sealed class Cache<T, R> : MemoizationCacheBase<T, R>, IServiceProvider
            {
                //
                // The cache consists of a regular Dictionary<T, Entry> where T contains the
                // function argument and is not kept alive by the cache. Each entry is a node
                // in a double-linked list that acts as the LRU list. The first element of the
                // list represents the most recently accessed entry. Cache eviction starts from
                // the tail of the list. Each list entry contains the result of evaluation the
                // function with the corresponding argument, as well as a weak reference to the
                // argument. This allows the cache eviction logic using the LRU list traversal
                // to remove the corresponding cache entry, without keeping the argument alive.
                //

                private readonly Func<T, ILruCacheEntry<T, R>> _function;
                private readonly ICacheDictionary<T, ILruCacheEntry<T, R>> _cache;
                private readonly int _maxCapacity;
                private readonly bool _cacheError;

                //
                // We embed a linked list to save on the costs of additional pointers:
                //
                //  - 1 pointer to the linked list
                //  - N pointers to the containing list in each list node
                //
                private LruLinkedList<ILruCacheEntry<T, R>> _lruList;

#if DEBUG
                private int _invocationCount;
                private int _accessCount;
                private int _evictionCount;
                private ILruCacheEntry<T, R> _lastEvicted;
#endif

                public Cache(Func<T, R> function, int maxCapacity, IEqualityComparer<T> comparer, bool cacheError)
                {
                    _function = args =>
                    {
#if DEBUG
                        var invokeDuration = default(TimeSpan);
                        _invocationCount++;
#endif
                        Trim();

                        var res = default(ILruCacheEntry<T, R>);
                        try
                        {
#if DEBUG
                            var swInvoke = Stopwatch.StartNew();
#endif
                            var value = function(args);
#if DEBUG
                            invokeDuration = swInvoke.Elapsed;
#endif
                            res = new LruValueCacheEntry<T, R>(args, value);
                        }
                        catch (Exception ex) when (_cacheError)
                        {
                            res = new LruErrorCacheEntry<T, R>(args, ex);
                        }
#if DEBUG
                        res.GetMetrics().InvokeDuration = invokeDuration;
#endif
                        return res;
                    };

                    _cache = new CacheDictionary<T, ILruCacheEntry<T, R>>(comparer);
                    _maxCapacity = maxCapacity;
                    _cacheError = cacheError;
                }

                protected override int CountCore => _cache.Count;

                [ExcludeFromCodeCoverage]
                protected override string DebugViewCore
                {
                    get
                    {
                        var sb = new StringBuilder();

                        sb.AppendLine("Entries");
                        sb.AppendLine("-------");
                        sb.AppendLine();

                        if (_cache.Count == 0)
                        {
                            sb.AppendLine("  (empty)");
                        }
                        else
                        {
                            var node = _lruList.First;
                            while (node != null)
                            {
                                var value = node is ErrorCacheEntry<T, R> err ? err.Exception : (object)node.Value;
                                sb.AppendFormat(CultureInfo.InvariantCulture, "  {0} -> {1}", node.Key, value);
                                sb.AppendLine();

#if DEBUG
                                node.GetMetrics().ToDebugView(sb, "    ");
                                sb.AppendLine();
#endif

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
#if DEBUG
                    var swTotal = Stopwatch.StartNew();
                    _accessCount++;
#endif
                    var entry = _cache.GetOrAdd(args, _function);

                    MostRecent(entry);
#if DEBUG
                    var metrics = entry.GetMetrics();
                    metrics.HitCount++;
                    metrics.TotalDuration += swTotal.Elapsed;
#endif
                    return entry.Value;
                }

                /// <summary>
                /// Makes the specified node the most recently accessed one by moving it to the front of the LRU list.
                /// </summary>
                /// <param name="node">The node that was most recently accessed.</param>
                private void MostRecent(ILruCacheEntry<T, R> node)
                {
                    //
                    // NB: We opted for a linked list approach for the LRU cache to have constant overhead for memoized function
                    //     invocation, with a minor increment upon cache pruning. Alternatively, we could sort entries by their
                    //     last access time, having less overhead during a lookup (just store the new access time) but at the
                    //     expense of having to sort the entries in the Trim procedure. That'd cause a hiccup in lookup times.
                    //
                    // NB: If a ranking based eviction is desirable, resulting in higher lookup speeds but lower pruning speeds,
                    //     one can use the CreateEvictedBy* methods on MemoizationCacheFactory.
                    //
                    LruLinkedList.MoveFirst(ref _lruList, node);
                }

                /// <summary>
                /// Trims the LRU cache if needed.
                /// </summary>
                private void Trim()
                {
                    while (_cache.Count >= _maxCapacity)
                    {
                        var entry = _lruList.Last;
                        _cache.Remove(entry.Key);
                        LruLinkedList.RemoveLast(ref _lruList);
#if DEBUG
                        _lastEvicted = entry;
                        _evictionCount++;
#endif
                    }
                }

                protected override void ClearCore(bool disposing)
                {
                    _cache.Clear();
                    LruLinkedList.Clear(ref _lruList);
#if DEBUG
                    _accessCount = 0;
                    _evictionCount = 0;
                    _invocationCount = 0;
                    _lastEvicted = null;
#endif
                }

                public object GetService(Type serviceType)
                {
                    var res = default(object);

                    if (serviceType == typeof(ITrimmable<KeyValuePair<T, R>>))
                    {
                        res = Trimmable.Create<KeyValuePair<T, R>>(shouldTrim => TrimBy(kv => kv.Value.Kind == ValueOrErrorKind.Value, kv => new KeyValuePair<T, R>(kv.Key, kv.Value.Value), shouldTrim));
                    }
                    else if (serviceType == typeof(ITrimmable<KeyValuePair<T, IValueOrError<R>>>) && _cacheError)
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

                    for (var node = _lruList.First; node != null; node = node.Next)
                    {
                        var kv = new KeyValuePair<T, IValueOrError<R>>(node.Key, node);

                        if (filter(kv) && shouldTrim(selector(kv)))
                        {
                            LruLinkedList.Remove(ref _lruList, node);
                            _cache.Remove(node.Key);

                            res++;
                        }
                    }

                    return res;
                }
            }
        }
    }
}
