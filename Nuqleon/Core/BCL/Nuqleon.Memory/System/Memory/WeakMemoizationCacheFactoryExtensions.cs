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
    /// Extension methods for IWeakMemoizationCacheFactory.
    /// </summary>
    public static class WeakMemoizationCacheFactoryExtensions
    {
        /// <summary>
        /// Creates a memoization cache factory that keeps function memoization caches for each thread on which the memoized function gets invoked.
        /// This is useful to reduce cache access lock contention and can be used to make a memoization cache safe for concurrent access, at the expense of keeping a cache per thread.
        /// </summary>
        /// <param name="factory">The memoization cache factory to wrap with thread-local caching behavior.</param>
        /// <returns>A memoization cache factory that wraps the specified <paramref name="factory"/> and adds thread-local isolation to it.</returns>
        public static IWeakMemoizationCacheFactory WithThreadLocal(this IWeakMemoizationCacheFactory factory)
        {
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));

            return new ThreadLocalFactory(factory);
        }

        /// <summary>
        /// Creates a memoization cache factory that keeps function memoization caches for each thread on which the memoized function gets invoked.
        /// This is useful to reduce cache access lock contention and can be used to make a memoization cache safe for concurrent access, at the expense of keeping a cache per thread.
        /// </summary>
        /// <param name="factory">The memoization cache factory to wrap with thread-local caching behavior.</param>
        /// <param name="exposeThreadLocalView">Indicates whether the caches returned from the resulting factory provide a thread-local view on the cache, for properties like Count and methods like Clear.</param>
        /// <returns>A memoization cache factory that wraps the specified <paramref name="factory"/> and adds thread-local isolation to it.</returns>
        public static IWeakMemoizationCacheFactory WithThreadLocal(this IWeakMemoizationCacheFactory factory, bool exposeThreadLocalView)
        {
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));

            return new ThreadLocalFactory(factory, exposeThreadLocalView);
        }

        //
        // NB: No Synchronized implementation, because weak cache factories are assumed to be thread-safe
        //     due to their use of CWT. Use of WithThreadLocal allows to reduce the locking caused by the
        //     underlying weak cache's synchronization. If we ever decide to support non-thread-safe weak
        //     caches, we can revisit this API restriction.
        //

        private sealed class ThreadLocalFactory : IWeakMemoizationCacheFactory
        {
            private readonly IWeakMemoizationCacheFactory _factory;
            private readonly bool _exposeGlobalView;

            public ThreadLocalFactory(IWeakMemoizationCacheFactory factory) => _factory = factory;

            public ThreadLocalFactory(IWeakMemoizationCacheFactory factory, bool exposeThreadLocalView)
            {
                _factory = factory;
                _exposeGlobalView = !exposeThreadLocalView;
            }

            public IMemoizationCache<T, R> Create<T, R>(Func<T, R> function, MemoizationOptions options) where T : class
            {
                if (function == null)
                    throw new ArgumentNullException(nameof(function));

                if (_exposeGlobalView)
                {
                    return new MemoizationCacheFactoryExtensions.ThreadLocalFactory.ImplUnion<T, R>(() => _factory.Create(function, options));
                }

                return new MemoizationCacheFactoryExtensions.ThreadLocalFactory.Impl<T, R>(() => _factory.Create(function, options));
            }
        }
    }
}
