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
using System.Linq;
using System.Text;
using System.Time;

namespace System.Memory
{
    public partial class MemoizationCacheFactory
    {
        /// <summary>
        /// Implementation of a factory for memoization caches with a ranking-based eviction strategy.
        /// </summary>
        /// <typeparam name="TMetric">Type of the metric to rank cache entries by.</typeparam>
        private sealed class EvictImpl<TMetric> : IMemoizationCacheFactory
        {
            private readonly Func<IMemoizationCacheEntryMetrics, TMetric> _ranker;
            private readonly int _maxCapacity;
            private readonly bool _descending;
            private readonly double _ageThreshold;
            private readonly IStopwatchFactory _stopwatchFactory;

            /// <summary>
            /// Creates a memoization cache factory for memoization caches that use an eviction strategy based on a function to rank cache entries based on metrics.
            /// </summary>
            /// <param name="ranker">The ranker function used to obtain the metric for each entry upon evicting entries from the cache.</param>
            /// <param name="maxCapacity">The maximum capacity of memoization caches returned by the factory.</param>
            /// <param name="ageThreshold">The threshold used to decide whether an entry has aged sufficiently in order to be considered for eviction. E.g. a value of 0.9 means that the youngest 10% of entries cannot get evicted.</param>
            /// <param name="stopwatchFactory">The stopwatch factory used to create stopwatches that measure access times and function invocation times. If omitted, the default stopwatch is used.</param>
            /// <param name="descending">Indicates whether the ranker should evict entries with the highest or lowest score.</param>
            /// <returns>Memoization cache factory for memoization caches that use a ranking-based cache eviction strategy.</returns>
            public EvictImpl(Func<IMemoizationCacheEntryMetrics, TMetric> ranker, int maxCapacity, double ageThreshold, IStopwatchFactory stopwatchFactory, bool descending)
            {
                _ranker = ranker;
                _maxCapacity = maxCapacity;
                _descending = descending;
                _ageThreshold = ageThreshold;
                _stopwatchFactory = stopwatchFactory ?? StopwatchFactory.Diagnostics;
            }

            /// <summary>
            /// Creates a memoization cache for the specified <paramref name="function"/> that doesn't keep cache entry keys alive.
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
                return new Cache<T, TResult>(function, _ranker, _maxCapacity, _descending, _ageThreshold, comparer, cacheError, _stopwatchFactory);
            }

            private sealed class Cache<T, R> : MemoizationCacheBase<T, R>, IServiceProvider
            {
                private readonly Func<T, IMetricsCacheEntry<T, R>> _function;
                private readonly ICacheDictionary<T, IMetricsCacheEntry<T, R>> _cache;
                private readonly IStopwatch _stopwatch;
                private readonly IEnumerable<IMetricsCacheEntry<T, R>> _ranker;
                private readonly int _maxCapacity;
                private readonly bool _cacheError;
#if DEBUG
                private int _invocationCount;
                private int _accessCount;
                private int _evictionCount;
                private int _trimCount;
                private TimeSpan _trimElapsed;
                private IMetricsCacheEntry<T, R> _lastEvicted;
#endif
                public Cache(Func<T, R> function, Func<IMemoizationCacheEntryMetrics, TMetric> ranker, int maxCapacity, bool descending, double ageThreshold, IEqualityComparer<T> comparer, bool cacheError, IStopwatchFactory stopwatchFactory)
                {
                    _function = args =>
                    {
                        var invokeDuration = default(TimeSpan);
#if DEBUG
                        _invocationCount++;
#endif
                        Trim();

                        var res = default(IMetricsCacheEntry<T, R>);
                        try
                        {
                            var swInvokeStart = _stopwatch.ElapsedTicks;

                            var value = function(args);

                            invokeDuration = new TimeSpan(_stopwatch.ElapsedTicks - swInvokeStart);

                            res = new MetricsValueCacheEntry<T, R>(args, value);
                        }
                        catch (Exception ex) when (_cacheError)
                        {
                            res = new MetricsErrorCacheEntry<T, R>(args, ex);
                        }

                        res.CreationTime = new TimeSpan(_stopwatch.ElapsedTicks);
                        res.InvokeDuration = invokeDuration;

                        return res;
                    };

                    _cache = new CacheDictionary<T, IMetricsCacheEntry<T, R>>(comparer);
                    _stopwatch = stopwatchFactory.StartNew();

                    //
                    // Exclude newest items which have statically irrelevant data, so they get a chance to become relevant.
                    //
                    var candidates = _cache.Values.OrderBy(e => _stopwatch.ElapsedTicks - e.CreationTime.Ticks).Take(Math.Max(1, (int)(maxCapacity * ageThreshold)));
                    _ranker = descending ? candidates.OrderByDescending(e => ranker(e)) : candidates.OrderBy(e => ranker(e));

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
                            foreach (var node in _cache.Values)
                            {
                                var value = node is ErrorCacheEntry<T, R> err ? err.Exception : (object)node.Value;
                                sb.AppendFormat(CultureInfo.InvariantCulture, "  {0} -> {1}", node.Key, value);
                                sb.AppendLine();
                                node.ToDebugView(sb, "    ");
                                sb.AppendLine();
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
                            sb.AppendLine("  Trim count       = " + _trimCount);
                            sb.AppendLine("  Trim elapsed     = " + _trimElapsed);
                            sb.AppendLine("  Eviction count   = " + _evictionCount);

                            if (_lastEvicted != null)
                            {
                                sb.AppendLine();
                                sb.AppendLine("  Last eviction");
                                _lastEvicted.ToDebugView(sb, "    ");
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
                    _accessCount++;
#endif
                    var swTotalStart = _stopwatch.ElapsedTicks;

                    var entry = _cache.GetOrAdd(args, _function);

                    var duration = new TimeSpan(_stopwatch.ElapsedTicks - swTotalStart);
                    var accessTime = _stopwatch.Elapsed;

                    entry.HitCount++;
                    entry.TotalDuration += duration;
                    entry.LastAccessTime = accessTime;

                    return entry.Value;
                }

                private void Trim()
                {
                    if (_cache.Count >= _maxCapacity)
                    {
#if DEBUG
                        var trimStart = _stopwatch.ElapsedTicks;
#endif
#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable IDE0063 // Use simple 'using' statement. (Only in RELEASE build.)
                        using (var evictionOrder = _ranker.GetEnumerator())
                        {
                            while (_cache.Count >= _maxCapacity && evictionOrder.MoveNext())
                            {
                                var entry = evictionOrder.Current;
#if DEBUG
                                _lastEvicted = entry;
#endif
                                _cache.Remove(entry.Key);
#if DEBUG
                                _evictionCount++;
#endif
                            }
                        }
#pragma warning restore IDE0063
#pragma warning restore IDE0079
#if DEBUG
                        var trimElapsed = new TimeSpan(_stopwatch.ElapsedTicks - trimStart);
                        _trimCount++;
                        _trimElapsed += trimElapsed;
#endif
                    }
                }

                protected override void ClearCore(bool disposing)
                {
                    _cache.Clear();
#if DEBUG
                    _invocationCount = 0;
                    _accessCount = 0;
                    _evictionCount = 0;
                    _trimCount = 0;
                    _trimElapsed = TimeSpan.Zero;
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
                    else if (serviceType == typeof(ITrimmable<IMemoizationCacheEntryMetrics>))
                    {
                        res = Trimmable.Create<IMemoizationCacheEntryMetrics>(shouldTrim => TrimBy(kv => true, kv => kv.Value, shouldTrim));
                    }

                    //
                    // NB: No trim by key or value; those types could unify to the same ITrimmable<>.
                    //     The drawback is that users of N-ary function need to use Args<> types.
                    //

                    return res;
                }

                private int TrimBy<K>(Func<KeyValuePair<T, IMetricsCacheEntry<T, R>>, bool> filter, Func<KeyValuePair<T, IMetricsCacheEntry<T, R>>, K> selector, Func<K, bool> shouldTrim)
                {
                    var keys = new HashSet<T>();

                    foreach (var entry in _cache)
                    {
                        if (filter(entry) && shouldTrim(selector(entry)))
                        {
                            keys.Add(entry.Key);
                        }
                    }

                    foreach (var key in keys)
                    {
                        _cache.Remove(key);
                    }

                    return keys.Count;
                }
            }
        }
    }
}
