// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 07/29/2015 - Initial work on memoization support.
//

using System.Collections.Generic;

namespace System.Memory
{
    /// <summary>
    /// Provides a set of extension methods for memoization caches.
    /// </summary>
    public static class MemoizationCacheExtensions
    {
        /// <summary>
        /// Gets a trimmable view on the specified memoization <paramref name="cache"/>, exposing the memoized function's pairs of arguments and results.
        /// </summary>
        /// <typeparam name="T">The type of the memoized function's arguments.</typeparam>
        /// <typeparam name="TResult">The type of the memoized function's result.</typeparam>
        /// <param name="cache">The memoization cache to obtain a trimmable view for.</param>
        /// <returns>A trimmable view on the specified memoization <paramref name="cache"/> if the cache supports trimming; otherwise, null.</returns>
        public static ITrimmable<KeyValuePair<T, TResult>> AsTrimmableByArgumentAndResult<T, TResult>(this IMemoizationCache cache)
        {
            if (cache == null)
                throw new ArgumentNullException(nameof(cache));

            return cache.GetService<ITrimmable<KeyValuePair<T, TResult>>>();
        }

        /// <summary>
        /// Gets a trimmable view on the specified memoization <paramref name="cache"/>, exposing the memoized function's pairs of arguments and results.
        /// </summary>
        /// <typeparam name="T">The type of the memoized function's arguments.</typeparam>
        /// <typeparam name="TResult">The type of the memoized function's result.</typeparam>
        /// <param name="cache">The memoization cache to obtain a trimmable view for.</param>
        /// <returns>A trimmable view on the specified memoization <paramref name="cache"/> if the cache supports trimming; otherwise, null.</returns>
        public static ITrimmable<KeyValuePair<T, TResult>> AsTrimmableByArgumentAndResult<T, TResult>(this IMemoizationCache<T, TResult> cache)
        {
            if (cache == null)
                throw new ArgumentNullException(nameof(cache));

            return cache.GetService<ITrimmable<KeyValuePair<T, TResult>>>();
        }

        /// <summary>
        /// Gets a trimmable view on the specified memoization <paramref name="cache"/>, exposing the memoized function's pairs of arguments and results or errors.
        /// </summary>
        /// <typeparam name="T">The type of the memoized function's arguments.</typeparam>
        /// <typeparam name="TResult">The type of the memoized function's result.</typeparam>
        /// <param name="cache">The memoization cache to obtain a trimmable view for.</param>
        /// <returns>A trimmable view on the specified memoization <paramref name="cache"/> if the cache supports trimming; otherwise, null.</returns>
        public static ITrimmable<KeyValuePair<T, IValueOrError<TResult>>> AsTrimmableByArgumentAndResultOrError<T, TResult>(this IMemoizationCache cache)
        {
            if (cache == null)
                throw new ArgumentNullException(nameof(cache));

            return cache.GetService<ITrimmable<KeyValuePair<T, IValueOrError<TResult>>>>();
        }

        /// <summary>
        /// Gets a trimmable view on the specified memoization <paramref name="cache"/>, exposing the memoized function's pairs of arguments and results or errors.
        /// </summary>
        /// <typeparam name="T">The type of the memoized function's arguments.</typeparam>
        /// <typeparam name="TResult">The type of the memoized function's result.</typeparam>
        /// <param name="cache">The memoization cache to obtain a trimmable view for.</param>
        /// <returns>A trimmable view on the specified memoization <paramref name="cache"/> if the cache supports trimming; otherwise, null.</returns>
        public static ITrimmable<KeyValuePair<T, IValueOrError<TResult>>> AsTrimmableByArgumentAndResultOrError<T, TResult>(this IMemoizationCache<T, TResult> cache)
        {
            if (cache == null)
                throw new ArgumentNullException(nameof(cache));

            return cache.GetService<ITrimmable<KeyValuePair<T, IValueOrError<TResult>>>>();
        }

        /// <summary>
        /// Gets a trimmable view on the specified memoization <paramref name="cache"/>, exposing the memoized function's cache entry metrics.
        /// </summary>
        /// <param name="cache">The memoization cache to obtain a trimmable view for.</param>
        /// <returns>A trimmable view on the specified memoization <paramref name="cache"/> if the cache supports trimming; otherwise, null.</returns>
        public static ITrimmable<IMemoizationCacheEntryMetrics> AsTrimmableByMetrics(this IMemoizationCache cache)
        {
            if (cache == null)
                throw new ArgumentNullException(nameof(cache));

            return cache.GetService<ITrimmable<IMemoizationCacheEntryMetrics>>();
        }

        /// <summary>
        /// Gets a trimmable view on the specified memoization <paramref name="cache"/>, exposing the memoized function's pairs of arguments and results.
        /// </summary>
        /// <typeparam name="T">The type of the memoized function's arguments.</typeparam>
        /// <typeparam name="TResult">The type of the memoized function's result.</typeparam>
        /// <param name="cache">The memoization cache to obtain a trimmable view for.</param>
        /// <returns>A trimmable view on the specified memoization <paramref name="cache"/> if the cache supports trimming; otherwise, an exception is thrown.</returns>
        public static ITrimmable<KeyValuePair<T, TResult>> ToTrimmableByArgumentAndResult<T, TResult>(this IMemoizationCache cache)
        {
            if (cache == null)
                throw new ArgumentNullException(nameof(cache));

            return NotNull(cache.GetService<ITrimmable<KeyValuePair<T, TResult>>>());
        }

        /// <summary>
        /// Gets a trimmable view on the specified memoization <paramref name="cache"/>, exposing the memoized function's pairs of arguments and results.
        /// </summary>
        /// <typeparam name="T">The type of the memoized function's arguments.</typeparam>
        /// <typeparam name="TResult">The type of the memoized function's result.</typeparam>
        /// <param name="cache">The memoization cache to obtain a trimmable view for.</param>
        /// <returns>A trimmable view on the specified memoization <paramref name="cache"/> if the cache supports trimming; otherwise, an exception is thrown.</returns>
        public static ITrimmable<KeyValuePair<T, TResult>> ToTrimmableByArgumentAndResult<T, TResult>(this IMemoizationCache<T, TResult> cache)
        {
            if (cache == null)
                throw new ArgumentNullException(nameof(cache));

            return NotNull(cache.GetService<ITrimmable<KeyValuePair<T, TResult>>>());
        }

        /// <summary>
        /// Gets a trimmable view on the specified memoization <paramref name="cache"/>, exposing the memoized function's pairs of arguments and results or errors.
        /// </summary>
        /// <typeparam name="T">The type of the memoized function's arguments.</typeparam>
        /// <typeparam name="TResult">The type of the memoized function's result.</typeparam>
        /// <param name="cache">The memoization cache to obtain a trimmable view for.</param>
        /// <returns>A trimmable view on the specified memoization <paramref name="cache"/> if the cache supports trimming; otherwise, an exception is thrown.</returns>
        public static ITrimmable<KeyValuePair<T, IValueOrError<TResult>>> ToTrimmableByArgumentAndResultOrError<T, TResult>(this IMemoizationCache cache)
        {
            if (cache == null)
                throw new ArgumentNullException(nameof(cache));

            return NotNull(cache.GetService<ITrimmable<KeyValuePair<T, IValueOrError<TResult>>>>());
        }

        /// <summary>
        /// Gets a trimmable view on the specified memoization <paramref name="cache"/>, exposing the memoized function's pairs of arguments and results or errors.
        /// </summary>
        /// <typeparam name="T">The type of the memoized function's arguments.</typeparam>
        /// <typeparam name="TResult">The type of the memoized function's result.</typeparam>
        /// <param name="cache">The memoization cache to obtain a trimmable view for.</param>
        /// <returns>A trimmable view on the specified memoization <paramref name="cache"/> if the cache supports trimming; otherwise, an exception is thrown.</returns>
        public static ITrimmable<KeyValuePair<T, IValueOrError<TResult>>> ToTrimmableByArgumentAndResultOrError<T, TResult>(this IMemoizationCache<T, TResult> cache)
        {
            if (cache == null)
                throw new ArgumentNullException(nameof(cache));

            return NotNull(cache.GetService<ITrimmable<KeyValuePair<T, IValueOrError<TResult>>>>());
        }

        /// <summary>
        /// Gets a trimmable view on the specified memoization <paramref name="cache"/>, exposing the memoized function's cache entry metrics.
        /// </summary>
        /// <param name="cache">The memoization cache to obtain a trimmable view for.</param>
        /// <returns>A trimmable view on the specified memoization <paramref name="cache"/> if the cache supports trimming; otherwise, an exception is thrown.</returns>
        public static ITrimmable<IMemoizationCacheEntryMetrics> ToTrimmableByMetrics(this IMemoizationCache cache)
        {
            if (cache == null)
                throw new ArgumentNullException(nameof(cache));

            return NotNull(cache.GetService<ITrimmable<IMemoizationCacheEntryMetrics>>());
        }

        private static T NotNull<T>(T result)
        {
            if (result == null)
                throw new InvalidOperationException("The specified cache does not support trimming.");

            return result;
        }
    }
}
