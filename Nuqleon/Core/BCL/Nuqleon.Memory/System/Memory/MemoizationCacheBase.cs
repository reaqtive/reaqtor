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
    /// Base class for a cache to store memoization entries for a memoized function.
    /// </summary>
    public abstract class MemoizationCacheBase : IMemoizationCache
    {
        private volatile bool _disposed;

        /// <summary>
        /// Gets the number of entries in the cache.
        /// </summary>
        public virtual int Count
        {
            get
            {
                if (_disposed)
                {
                    return 0;
                }

                return CountCore;
            }
        }

        /// <summary>
        /// Gets the number of entries in the cache.
        /// </summary>
        protected abstract int CountCore
        {
            get;
        }

        /// <summary>
        /// Gets a debug view on the memoization cache.
        /// </summary>
        public virtual string DebugView
        {
            get
            {
                if (_disposed)
                {
                    return "The cache has been disposed.";
                }

                return DebugViewCore;
            }
        }

        /// <summary>
        /// Gets a debug view on the memoization cache.
        /// </summary>
        protected abstract string DebugViewCore
        {
            get;
        }

        /// <summary>
        /// Clears the entries in the cache.
        /// </summary>
        public virtual void Clear() => ClearCore(disposing: false);

        /// <summary>
        /// Clears the entries in the cache.
        /// </summary>
        /// <param name="disposing">Indicates whether the clear operation is triggered by a cache disposal.</param>
        protected abstract void ClearCore(bool disposing);

        /// <summary>
        /// Disposes the cache.
        /// </summary>
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes the cache.
        /// </summary>
        /// <param name="disposing">Indicates whether the dispose operation is triggered by a call to Dispose, or the finalizer.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!_disposed)
                {
                    //
                    // NB: This is done to conserve memory.
                    //
                    ClearCore(disposing: true);

                    DisposeCore();
                    _disposed = true;
                }
            }
        }

        /// <summary>
        /// Disposes the cache.
        /// </summary>
        protected virtual void DisposeCore()
        {
        }

        /// <summary>
        /// Checks whether the current instance has been disposed.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Thrown if the instance has been disposed.</exception>
        protected void CheckDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().FullName);
        }
    }
}
