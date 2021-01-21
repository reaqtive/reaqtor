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
    /// Exposes factory methods to create function memoizers.
    /// </summary>
    public static class Memoizer
    {
        /// <summary>
        /// Creates a function memoizer that uses the specified <paramref name="factory"/> to create memoization caches.
        /// </summary>
        /// <param name="factory">Memoization cache factory to use when memoizing functions using the resulting memoizer.</param>
        /// <returns>Function memoizer to speed up function invocations by caching their result.</returns>
        public static IMemoizer Create(IMemoizationCacheFactory factory)
        {
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));

            return new Impl(factory);
        }

        /// <summary>
        /// Creates a function memoizer that uses the specified <paramref name="factory"/> to create memoization caches that don't keep the function arguments alive.
        /// </summary>
        /// <param name="factory">Memoization cache factory to use when memoizing functions using the resulting memoizer.</param>
        /// <returns>Function memoizer to speed up function invocations by caching their result as long as the memoized function arguments are alive.</returns>
        public static IWeakMemoizer CreateWeak(IWeakMemoizationCacheFactory factory)
        {
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));

            return new WeakImpl(factory);
        }

        private sealed class Impl : IMemoizer
        {
            private readonly IMemoizationCacheFactory _factory;

            public Impl(IMemoizationCacheFactory factory) => _factory = factory;

            /// <summary>
            /// Memoizes the specified <paramref name="function"/>.
            /// </summary>
            /// <typeparam name="T">Type of the function argument.</typeparam>
            /// <typeparam name="TResult">Type of the function result.</typeparam>
            /// <param name="function">The function to memoize.</param>
            /// <param name="options">Flags to influence the memoization behavior.</param>
            /// <param name="comparer">Comparer to compare the function argument during lookup in the memoization cache.</param>
            /// <returns>A memoized delegate containing the memoized function and providing access to the memoization cache.</returns>
            public IMemoizedDelegate<Func<T, TResult>> Memoize<T, TResult>(Func<T, TResult> function, MemoizationOptions options = MemoizationOptions.None, IEqualityComparer<T> comparer = null)
            {
                if (function == null)
                    throw new ArgumentNullException(nameof(function));

                var cache = _factory.Create(function, options, comparer);
                return new MemoizedDelegate<Func<T, TResult>>(cache.GetOrAdd, cache);
            }
        }

        private sealed class WeakImpl : IWeakMemoizer
        {
            private readonly IWeakMemoizationCacheFactory _factory;

            public WeakImpl(IWeakMemoizationCacheFactory factory) => _factory = factory;

            /// <summary>
            /// Memoizes the specified <paramref name="function"/> without keeping the function argument alive by the memoization cache.
            /// </summary>
            /// <typeparam name="T">Type of the function argument. This type has to be a reference type.</typeparam>
            /// <typeparam name="TResult">Type of the function result.</typeparam>
            /// <param name="function">The function to memoize.</param>
            /// <param name="options">Flags to influence the memoization behavior.</param>
            /// <returns>A memoized delegate containing the memoized function and providing access to the memoization cache.</returns>
            public IMemoizedDelegate<Func<T, TResult>> MemoizeWeak<T, TResult>(Func<T, TResult> function, MemoizationOptions options = MemoizationOptions.None)
                where T : class
            {
                if (function == null)
                    throw new ArgumentNullException(nameof(function));

                var cache = _factory.Create(function, options);
                return new MemoizedDelegate<Func<T, TResult>>(cache.GetOrAdd, cache);
            }
        }
    }
}
