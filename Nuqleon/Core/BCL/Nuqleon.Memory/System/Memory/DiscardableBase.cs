// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   ER - 11/14/2014 - Created this type.
//

namespace System.Memory
{
    /// <summary>
    /// Base implementation of cached reference including a finalizer.
    /// </summary>
    /// <typeparam name="T">Type of the objects kept in the cache.</typeparam>
    public abstract class DiscardableBase<T> : IDiscardable<T>
    {
        /// <summary>
        /// Gets the cached value.
        /// </summary>
        public abstract T Value
        {
            get;
        }

        /// <summary>
        /// Disposes the cached reference.
        /// </summary>
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes the cached reference.
        /// </summary>
        /// <param name="disposing">
        /// <b>true</b> if the dispose was explicit.
        /// </param>
        protected abstract void Dispose(bool disposing);

        /// <summary>
        /// Finalizes the cached reference.
        /// </summary>
        ~DiscardableBase()
        {
            Dispose(disposing: false);
        }
    }
}
