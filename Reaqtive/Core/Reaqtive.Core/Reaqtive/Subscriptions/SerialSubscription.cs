// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

namespace Reaqtive
{
    /// <summary>
    /// Serial subscription that disposes an existing subscriptions upon reassigment.
    /// </summary>
    public class SerialSubscription : IFutureSubscription
    {
        private readonly object _gate = new();
        private ISubscription _current;
        private bool _disposed;

        /// <summary>
        /// Gets or sets the underlying subscription.
        /// </summary>
        /// <remarks>If the SerialDisposable has already been disposed, assignment to this property causes immediate disposal of the given disposable object. Assigning this property disposes the previous disposable object.</remarks>
        public ISubscription Subscription
        {
            get => _current;

            set
            {
                var shouldDispose = false;

                var old = default(ISubscription);
                lock (_gate)
                {
                    shouldDispose = _disposed;
                    if (!shouldDispose)
                    {
                        old = _current;
                        _current = value;
                    }
                }

                old?.Dispose();

                if (shouldDispose && value != null)
                    value.Dispose();
            }
        }

        /// <summary>
        /// Accepts a visitor that will be dispatched through the subscription, causing the inner subscription to be visited.
        /// </summary>
        /// <param name="visitor">Visitor to accept.</param>
        public void Accept(ISubscriptionVisitor visitor)
        {
            _current?.Accept(visitor);
        }

        /// <summary>
        /// Disposes the underlying disposable as well as all future replacements.
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
                var old = default(ISubscription);

                lock (_gate)
                {
                    if (!_disposed)
                    {
                        _disposed = true;
                        old = _current;
                        _current = null;
                    }
                }

                old?.Dispose();
            }
        }
    }
}
