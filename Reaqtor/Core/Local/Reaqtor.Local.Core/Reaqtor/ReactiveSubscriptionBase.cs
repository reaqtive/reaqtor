// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
//

using System;

namespace Reaqtor
{
    /// <summary>
    /// Base class for subscriptions.
    /// </summary>
    public abstract class ReactiveSubscriptionBase : IReactiveSubscription
    {
        #region Dispose

        /// <summary>
        /// Disposes the subscription.
        /// </summary>
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes the object.
        /// </summary>
        /// <param name="disposing">true if called from <see cref="IDisposable.Dispose"/>; otherwise, false.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                DisposeCore();
            }
        }

        /// <summary>
        /// Disposes the subscription.
        /// </summary>
        protected abstract void DisposeCore();

        #endregion
    }
}
