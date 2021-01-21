// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 07/29/2015 - Initial work on memoization support.
//   BD - 08/09/2015 - Adding nop memoization support.
//

using System.Time;

namespace System.Memory
{
    /// <summary>
    /// Memoization cache factory for memoization caches that are not thread-safe.
    /// </summary>
    public static partial class MemoizationCacheFactory
    {
        /// <summary>
        /// Gets a memoization cache factory for memoization caches with unbounded storage.
        /// </summary>
        /// <remarks>Memoization caches can be cleared explicitly through the <see cref="MemoizedDelegate{TDelegate}"/> returned from Memoize methods.</remarks>
        public static IMemoizationCacheFactory Unbounded { get; } = new UnboundedImpl();

        /// <summary>
        /// Gets a memoization cache factory for memoization caches without any storage.
        /// This allows to disable memoization while retaining a code structure that relies on <see cref="IMemoizationCacheFactory"/> instances.
        /// </summary>
        public static IMemoizationCacheFactory Nop { get; } = new NopImpl();

        /// <summary>
        /// Creates a memoization cache factory for memoization caches that use an LRU cache eviction strategy.
        /// </summary>
        /// <param name="maxCapacity">The maximum capacity of memoization caches returned by the factory.</param>
        /// <returns>Memoization cache factory for memoization caches that use an LRU cache eviction strategy.</returns>
        public static IMemoizationCacheFactory CreateLru(int maxCapacity)
        {
            if (maxCapacity < 1)
                throw new ArgumentOutOfRangeException(nameof(maxCapacity), "A cache should have at a capacity of at least one.");

            return new LruImpl(maxCapacity);
        }

        /// <summary>
        /// Creates a memoization cache factory for memoization caches that use an eviction strategy based on a function to rank cache entries based on metrics.
        /// Entries that meet the age threshold and have the lowest score as computed by the ranker get evicted.
        /// </summary>
        /// <typeparam name="TMetric">Type of the metric to rank cache entries by.</typeparam>
        /// <param name="ranker">The ranker function used to obtain the metric for each entry upon evicting entries from the cache.</param>
        /// <param name="maxCapacity">The maximum capacity of memoization caches returned by the factory.</param>
        /// <param name="ageThreshold">The threshold used to decide whether an entry has aged sufficiently in order to be considered for eviction. E.g. a value of 0.9 means that the youngest 10% of entries cannot get evicted.</param>
        /// <param name="stopwatchFactory">The stopwatch factory used to create stopwatches that measure access times and function invocation times. If omitted, the default stopwatch is used.</param>
        /// <returns>Memoization cache factory for memoization caches that use a ranking-based cache eviction strategy.</returns>
        public static IMemoizationCacheFactory CreateEvictedByLowest<TMetric>(Func<IMemoizationCacheEntryMetrics, TMetric> ranker, int maxCapacity, double ageThreshold = 0.9, IStopwatchFactory stopwatchFactory = null)
        {
            if (ranker == null)
                throw new ArgumentNullException(nameof(ranker));
            if (maxCapacity < 1)
                throw new ArgumentOutOfRangeException(nameof(maxCapacity), "A cache should have at a capacity of at least one.");
            if (ageThreshold is <= 0 or > 1)
                throw new ArgumentOutOfRangeException(nameof(ageThreshold), "The age threshold should be a number between 0 (inclusive) and 1 (exclusive).");

            return new EvictImpl<TMetric>(ranker, maxCapacity, ageThreshold, stopwatchFactory, descending: false);
        }

        /// <summary>
        /// Creates a memoization cache factory for memoization caches that use an eviction strategy based on a function to rank cache entries based on metrics.
        /// Entries that meet the age threshold and have the highest score as computed by the ranker get evicted.
        /// </summary>
        /// <typeparam name="TMetric">Type of the metric to rank cache entries by.</typeparam>
        /// <param name="ranker">The ranker function used to obtain the metric for each entry upon evicting entries from the cache.</param>
        /// <param name="maxCapacity">The maximum capacity of memoization caches returned by the factory.</param>
        /// <param name="ageThreshold">The threshold used to decide whether an entry has aged sufficiently in order to be considered for eviction. E.g. a value of 0.9 means that the youngest 10% of entries cannot get evicted.</param>
        /// <param name="stopwatchFactory">The stopwatch factory used to create stopwatches that measure access times and function invocation times. If omitted, the default stopwatch is used.</param>
        /// <returns>Memoization cache factory for memoization caches that use a ranking-based cache eviction strategy.</returns>
        public static IMemoizationCacheFactory CreateEvictedByHighest<TMetric>(Func<IMemoizationCacheEntryMetrics, TMetric> ranker, int maxCapacity, double ageThreshold = 0.9, IStopwatchFactory stopwatchFactory = null)
        {
            if (ranker == null)
                throw new ArgumentNullException(nameof(ranker));
            if (maxCapacity < 1)
                throw new ArgumentOutOfRangeException(nameof(maxCapacity), "A cache should have at a capacity of at least one.");
            if (ageThreshold is <= 0 or > 1)
                throw new ArgumentOutOfRangeException(nameof(ageThreshold), "The age threshold should be a number between 0 (inclusive) and 1 (exclusive).");

            return new EvictImpl<TMetric>(ranker, maxCapacity, ageThreshold, stopwatchFactory, descending: true);
        }
    }
}
