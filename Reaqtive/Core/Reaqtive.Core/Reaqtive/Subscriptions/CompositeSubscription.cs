// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace Reaqtive
{
    /// <summary>
    /// Composite subscription that can hold a dynamic number of inner subscriptions.
    /// </summary>
    public sealed class CompositeSubscription : ICompositeSubscription
    {
        private const int MinimumCapacityToShrinkList = 64;

        private readonly object _syncLock = new();
        private List<ISubscription> _subscriptions;
        private bool _disposed;
        private int _activeSubscriptionsCount;

        /// <summary>
        /// Creates a new composite subscription with the specified inner subscriptions.
        /// </summary>
        /// <param name="subscriptions">Inner subscriptions.</param>
        public CompositeSubscription(params ISubscription[] subscriptions)
            : this((IEnumerable<ISubscription>)subscriptions)
        {
        }

        /// <summary>
        /// Creates a new composite subscription with the specified inner subscriptions.
        /// </summary>
        /// <param name="subscriptions">Inner subscriptions.</param>
        public CompositeSubscription(IEnumerable<ISubscription> subscriptions)
        {
            if (subscriptions == null)
            {
                throw new ArgumentNullException(nameof(subscriptions));
            }

            _subscriptions = new List<ISubscription>(subscriptions);
            _activeSubscriptionsCount = _subscriptions.Count;
        }

        /// <summary>
        /// Creates a new composite subscription with the specified initial capacity.
        /// </summary>
        /// <param name="capacity">Initial capacity of the number of inner subscriptions the composite subscription can hold.</param>
        public CompositeSubscription(int capacity)
        {
            if (capacity <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(capacity));
            }

            _subscriptions = new List<ISubscription>(capacity);
        }

        /// <summary>
        /// Gets the number of inner subscriptions.
        /// </summary>
        /// <remarks>This number can change immediate after being read in the presence of multi-threaded usage.</remarks>
        public int Count => Volatile.Read(ref _activeSubscriptionsCount);

        private bool ShouldShrinkSubscriptionList =>
                // shrink the list if the capacity is over the threshold and at the number of active subscriptions is less than its half
                _subscriptions.Capacity > MinimumCapacityToShrinkList && _activeSubscriptionsCount < _subscriptions.Capacity / 2;

        /// <summary>
        /// Adds the specified subscription to the list of subscriptions that this instance manages.
        /// If the <see cref="CompositeSubscription"/> is already disposed, then the passed in subscription also gets disposed
        /// </summary>
        /// <param name="subscription">Subscription to add.</param>
        public void Add(ISubscription subscription)
        {
            if (subscription == null)
            {
                throw new ArgumentNullException(nameof(subscription));
            }

            bool shouldDispose = false;

            lock (_syncLock)
            {
                shouldDispose = _disposed;

                if (!shouldDispose)
                {
                    _subscriptions.Add(subscription);
                    _activeSubscriptionsCount++;
                }
            }

            if (shouldDispose)
            {
                subscription.Dispose();
            }
        }

        /// <summary>
        /// Removes the specified subscription from the list of subscriptions that this instance manages and disposes the removed subscription, if found.
        /// </summary>
        /// <param name="subscription">Subscription to remove.</param>
        public void Remove(ISubscription subscription)
        {
            if (subscription == null)
            {
                throw new ArgumentNullException(nameof(subscription));
            }

            bool subscriptionFound = false;

            lock (_syncLock)
            {
                var position = _subscriptions.IndexOf(subscription);

                if (position >= 0)
                {
                    subscriptionFound = true;
                    _subscriptions[position] = null;
                    _activeSubscriptionsCount--;

                    if (ShouldShrinkSubscriptionList)
                    {
                        ShrinkSubscriptionList();
                    }
                }
            }

            if (subscriptionFound)
            {
                // Also dispose the subscription here
                subscription.Dispose();
            }
        }

        /// <summary>
        /// Accepts a visitor that will be dispatched through the subscription, causing all inner subscriptions to be visited.
        /// </summary>
        /// <param name="visitor">Visitor to accept.</param>
        public void Accept(ISubscriptionVisitor visitor)
        {
            var snapshot = TakeSnapshot();

            foreach (var subscription in snapshot)
            {
                subscription.Accept(visitor);
            }
        }

        /// <summary>
        /// Gets an enumerator to iterate over the inner subscriptions.
        /// </summary>
        /// <returns>Enumerator to iterate over a snapshot of the inner subscriptions.</returns>
        /// <remarks>This method causes a snapshot of inner subscriptions to be created. While iteration is in progress, subscriptions may be added or removed due to multi-threaded accesses.</remarks>
        public IEnumerator<ISubscription> GetEnumerator() => ((IEnumerable<ISubscription>)TakeSnapshot()).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// Disposes the composite subscription, causing all inner subscriptions to be disposed.
        /// </summary>
        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            ISubscription[] snapshot = null;

            lock (_syncLock)
            {
                if (!_disposed)
                {
                    // take a snapshot of all existing subscriptions
                    snapshot = TakeSnapshot();

                    // clear the internal list so that we can't alter its references through Remove calls
                    _subscriptions.Clear();
                    _activeSubscriptionsCount = 0;

                    // mark the instance as disposed right now so that any new Add will dispose the subscription to add
                    _disposed = true;
                }
            }

            if (snapshot != null)
            {
                foreach (var subscription in snapshot)
                {
                    subscription.Dispose();
                }
            }
        }

        private ISubscription[] TakeSnapshot()
        {
            lock (_syncLock)
            {
                if (_activeSubscriptionsCount == 0)
                {
                    return Array.Empty<ISubscription>();
                }

                var snapshot = new ISubscription[_activeSubscriptionsCount];

                var i = 0;

                foreach (var sub in _subscriptions)
                {
                    if (sub != null)
                    {
                        snapshot[i++] = sub;
                    }
                }

                Debug.Assert(i == snapshot.Length);

                return snapshot;
            }
        }

        /// <summary>
        /// Keep only the active subscriptions inside this instance.
        /// </summary>
        private void ShrinkSubscriptionList()
        {
            Debug.Assert(Monitor.IsEntered(_syncLock));

            var newSubscriptionList = new List<ISubscription>(_subscriptions.Capacity / 2);

            foreach (var sub in _subscriptions)
            {
                if (sub != null)
                {
                    newSubscriptionList.Add(sub);
                }
            }

            // discard the previous list reference
            _subscriptions = newSubscriptionList;
        }
    }
}
