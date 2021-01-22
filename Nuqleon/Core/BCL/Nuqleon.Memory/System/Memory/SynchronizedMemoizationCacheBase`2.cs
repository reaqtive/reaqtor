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
    /// Base class for a cache to store memoization entries for a memoized function. Access to the cache gets synchronized through a lock.
    /// </summary>
    /// <typeparam name="T">Type of the memoized function argument.</typeparam>
    /// <typeparam name="TResult">Type of the memoized function result.</typeparam>
    public abstract class SynchronizedMemoizationCacheBase<T, TResult> : MemoizationCacheBase<T, TResult>
    {
        //
        // NB: Recursive functions can cause reentrant behavior. We don't protect against this with this implementation.
        //

        /// <summary>
        /// Gets an object to synchronize access to the cache on.
        /// </summary>
        protected abstract object SyncRoot
        {
            get;
        }

        /// <summary>
        /// Gets the number of entries in the cache.
        /// </summary>
        public override int Count
        {
            get
            {
                lock (SyncRoot)
                {
                    return base.Count;
                }
            }
        }

        /// <summary>
        /// Gets a debug view on the memoization cache.
        /// </summary>
        public override string DebugView
        {
            get
            {
                lock (SyncRoot)
                {
                    return base.DebugView;
                }
            }
        }

        /// <summary>
        /// Gets the result of invoking the memoized function with the specified <paramref name="argument"/>.
        /// If the memoization cache does not have the result of the function invocation stored yet, it will call the function.
        /// </summary>
        /// <param name="argument">The argument to get the function invocation result for.</param>
        /// <returns>The function invocation result.</returns>
        public override TResult GetOrAdd(T argument)
        {
            lock (SyncRoot)
            {
                return base.GetOrAdd(argument);
            }
        }

        /// <summary>
        /// Clears the entries in the cache.
        /// </summary>
        public override void Clear()
        {
            lock (SyncRoot)
            {
                base.Clear();
            }
        }

        /// <summary>
        /// Disposes the cache.
        /// </summary>
        /// <param name="disposing">Indicates whether the dispose operation is triggered by a call to Dispose, or the finalizer.</param>
        protected override void Dispose(bool disposing)
        {
            lock (SyncRoot)
            {
                base.Dispose(disposing);
            }
        }
    }
}
