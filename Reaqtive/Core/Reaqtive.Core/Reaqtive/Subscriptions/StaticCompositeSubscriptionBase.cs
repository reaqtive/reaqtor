// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace Reaqtive
{
    /// <summary>
    /// Base class for composite subscription with a static set of inner subscriptions.
    /// </summary>
    public abstract class StaticCompositeSubscriptionBase : ICompositeSubscription
    {
        private int _disposed;

        /// <summary>
        /// Subscriptions cannot be added to this composite subscription.
        /// </summary>
        /// <param name="subscription">Irrelevant.</param>
        /// <exception cref="System.InvalidOperationException">Always thrown.</exception>
        public void Add(ISubscription subscription)
        {
            throw new InvalidOperationException("This composite subscription instance is read-only.");
        }

        /// <summary>
        /// Subscriptions cannot be removed from this composite subscription.
        /// </summary>
        /// <param name="subscription">Irrelevant.</param>
        /// <exception cref="System.InvalidOperationException">Always thrown.</exception>
        public void Remove(ISubscription subscription)
        {
            throw new InvalidOperationException("This composite subscription instance is read-only.");
        }

        /// <summary>
        /// Gets an enumerator to iterate over the inner subscriptions.
        /// </summary>
        /// <returns>Enumerator to iterate over the inner subscriptions.</returns>
        public abstract IEnumerator<ISubscription> GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// Accepts a visitor that will be dispatched through the subscription, causing all inner subscriptions to be visited.
        /// </summary>
        /// <param name="visitor">Visitor to accept.</param>
        public abstract void Accept(ISubscriptionVisitor visitor);

        /// <summary>
        /// Disposes the composite subscription, causing all inner subscriptions to be disposed.
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
                if (Interlocked.Exchange(ref _disposed, 1) != 0)
                {
                    return;
                }

                DisposeCore();
            }
        }

        /// <summary>
        /// Disposes the composite subscription, causing all inner subscriptions to be disposed.
        /// </summary>
        protected abstract void DisposeCore();
    }
}
