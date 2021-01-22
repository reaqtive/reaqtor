// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 07/29/2015 - Initial work on memoization support.
//

namespace System.Memory
{
    /// <summary>
    /// Interface used to access metrics on memoization cache entries for ranking purposes.
    /// </summary>
    public interface IMemoizationCacheEntryMetrics
    {
        /// <summary>
        /// Gets the creation time for the cache entry. This value is relative to the creation time of the cache.
        /// </summary>
        TimeSpan CreationTime { get; }

        /// <summary>
        /// Gets the time it took to invoke the function whose result is memoized in the cache entry.
        /// </summary>
        TimeSpan InvokeDuration { get; }

        /// <summary>
        /// Gets the average time it takes to obtain the memoization result from the cache.
        /// </summary>
        TimeSpan AverageAccessTime { get; }

        /// <summary>
        /// Gets the number of cache hits for the cache entry upon invoking the memoized function.
        /// </summary>
        int HitCount { get; }

        /// <summary>
        /// Gets the last access time for the cache entry. This value is relative to the creation time of the cache.
        /// </summary>
        TimeSpan LastAccessTime { get; }

        /// <summary>
        /// Gets the speedup factor for the cache entry, i.e. the division of function invocation time by average access time.
        /// A value less than 1.0 indicates a slowdown; a value higher than 1.0 indicates a speedup.
        /// </summary>
        double SpeedupFactor { get; }
    }
}
