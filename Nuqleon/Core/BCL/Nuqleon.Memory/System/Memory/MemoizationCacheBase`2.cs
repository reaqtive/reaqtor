﻿// Licensed to the .NET Foundation under one or more agreements.
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
    /// Base class for a cache to store memoization entries for a memoized function.
    /// </summary>
    /// <typeparam name="T">Type of the memoized function argument.</typeparam>
    /// <typeparam name="TResult">Type of the memoized function result.</typeparam>
    public abstract class MemoizationCacheBase<T, TResult> : MemoizationCacheBase, IMemoizationCache<T, TResult>
    {
        /// <summary>
        /// Gets the result of invoking the memoized function with the specified <paramref name="argument"/>.
        /// If the memoization cache does not have the result of the function invocation stored yet, it will call the function.
        /// </summary>
        /// <param name="argument">The argument to get the function invocation result for.</param>
        /// <returns>The function invocation result.</returns>
        public virtual TResult GetOrAdd(T argument)
        {
            CheckDisposed();

            return GetOrAddCore(argument);
        }

        /// <summary>
        /// Gets the result of invoking the memoized function with the specified <paramref name="argument"/>.
        /// If the memoization cache does not have the result of the function invocation stored yet, it will call the function.
        /// </summary>
        /// <param name="argument">The argument to get the function invocation result for.</param>
        /// <returns>The function invocation result.</returns>
        protected abstract TResult GetOrAddCore(T argument);
    }
}
