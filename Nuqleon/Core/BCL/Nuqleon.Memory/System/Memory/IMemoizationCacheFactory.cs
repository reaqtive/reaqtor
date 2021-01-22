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
    /// Interface for memoization cache factories.
    /// </summary>
    public interface IMemoizationCacheFactory
    {
#pragma warning disable CA1716 // Identifiers should not match keywords. (Use of function is appropriate here.)

        /// <summary>
        /// Creates a memoization cache for the specified <paramref name="function"/> using the specified <paramref name="comparer"/> to compare cache entries.
        /// </summary>
        /// <typeparam name="T">Type of the memoization cache entry keys.</typeparam>
        /// <typeparam name="TResult">Type of the memoization cache entry values.</typeparam>
        /// <param name="function">The function to memoize.</param>
        /// <param name="options">Flags to influence the memoization behavior.</param>
        /// <param name="comparer">Comparer to compare the key during lookup in the memoization cache.</param>
        /// <returns>An empty memoization cache instance.</returns>
        IMemoizationCache<T, TResult> Create<T, TResult>(Func<T, TResult> function, MemoizationOptions options, IEqualityComparer<T> comparer);

#pragma warning restore CA1716
    }
}
