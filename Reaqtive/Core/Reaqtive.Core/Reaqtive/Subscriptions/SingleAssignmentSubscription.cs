// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

namespace Reaqtive
{
    /// <summary>
    /// Subscription that supports exactly one assignment of an inner subscription.
    /// </summary>
    public sealed class SingleAssignmentSubscription : IFutureSubscription
    {
        private readonly _ Disposed;
        private volatile ISubscription _current;
        private readonly object _lock;

        /// <summary>
        /// Creates a new single assignment subscription with no initial inner subscription assigned.
        /// </summary>
        public SingleAssignmentSubscription()
        {
            Disposed = new _();
            _lock = new object();
        }

        /// <summary>
        /// Gets a flag indicating whether the subscription has been disposed.
        /// </summary>
        public bool IsDisposed => _current == Disposed;

        /// <summary>
        /// Gets or sets the inner subscription.
        /// When the subscription is already disposed, a <c>null</c> reference will be returned, and the assignment of an inner subscription will result in immediate disposal.
        /// Any attempt to set an inner subscription when an inner subscription has already been set will result in an InvalidOperationException being thrown.
        /// </summary>
        public ISubscription Subscription
        {
            get
            {
                var current = _current;
                return current != Disposed ? current : null;
            }

            set
            {
                var old = default(ISubscription);

                lock (_lock)
                {
                    old = _current;
                    _current ??= value;
                }

                if (old == null)
                {
                    return;
                }

                if (old != Disposed)
                {
                    throw new InvalidOperationException("Subscription is already assigned.");
                }

                if (value != null)
                {
                    value.Dispose();

                    Disposed.DisposedSubscription ??= value;
                }
            }
        }

        /// <summary>
        /// Accepts a subscription visitor that will visit the inner subscription if it has been set already.
        /// </summary>
        /// <param name="visitor">Visitor to visit the subscription.</param>
        public void Accept(ISubscriptionVisitor visitor)
        {
            _current?.Accept(visitor);
        }

        /// <summary>
        /// Disposes the subscription. If an inner subscription has been set already, it will be disposed. Otherwise, the inner subscription will get disposed upon its assignment.
        /// </summary>
        public void Dispose()
        {
            var old = default(ISubscription);

            lock (_lock)
            {
                old = _current;
                _current = Disposed;

                if (old != null && old != Disposed)
                {
                    Disposed.DisposedSubscription = old;
                    Disposed.Dispose();
                }
            }

            old?.Dispose();
        }

        private sealed class _ : ISubscription
        {
            public ISubscription DisposedSubscription
            {
                get;
                set;
            }

            public void Accept(ISubscriptionVisitor visitor)
            {
                DisposedSubscription?.Accept(visitor);
            }

            public void Dispose()
            {
            }
        }
    }
}
