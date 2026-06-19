// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 08/09/2015 - Adding nop memoization support.
//

using System.Collections.Generic;

namespace System.Memory
{
    public partial class MemoizationCacheFactory
    {
        /// <summary>
        /// Implementation of a factory for memoization caches without storage.
        /// </summary>
        internal sealed class NopImpl : IMemoizationCacheFactory
        {
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

                return new Cache<T, TResult>(function);
            }

            internal sealed class Cache<T, R> : MemoizationCacheBase<T, R>
            {
                private readonly Func<T, R> _function;

                public Cache(Func<T, R> function) => _function = function;

                protected override R GetOrAddCore(T argument) => _function(argument);

                protected override int CountCore => 0;

                protected override string DebugViewCore => "";

                protected override void ClearCore(bool disposing)
                {
                }
            }
        }
    }
}
