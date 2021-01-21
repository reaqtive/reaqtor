// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 08/09/2015 - Adding nop memoization support.
//

namespace System.Memory
{
    public partial class WeakMemoizationCacheFactory
    {
        /// <summary>
        /// Implementation of a factory for weak memoization caches without storage.
        /// </summary>
        private sealed class NopImpl : IWeakMemoizationCacheFactory
        {
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

                return new MemoizationCacheFactory.NopImpl.Cache<T, R>(function);
            }
        }
    }
}
