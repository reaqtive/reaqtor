// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Reaqtive
{
    /// <summary>
    /// Composite subscription that can hold a dynamic number of inner subscriptions.
    /// </summary>
    /// <remarks>
    /// As the name suggests, it is expected that the subscription set is relatively
    /// stable. Adding and removing subscriptions results in the allocation of a new
    /// array, followed by a linear copy over. For a composite subscription with a
    /// more dynamic scenario, use <see cref="Reaqtive.CompositeSubscription"/>.
    /// </remarks>
    public sealed class StableCompositeSubscription : ICompositeSubscription
    {
        private readonly object _syncLock = new();
        private ISubscription[] _subscriptions;
        private bool _disposed;

        /// <summary>
        /// Creates a new composite subscription with the specified inner subscriptions.
        /// </summary>
        /// <param name="subscriptions">Inner subscriptions.</param>
        public StableCompositeSubscription(params ISubscription[] subscriptions)
            : this((IEnumerable<ISubscription>)subscriptions)
        {
        }

        /// <summary>
        /// Creates a new composite subscription with the specified inner subscriptions. 
        /// </summary>
        /// <param name="subscriptions">Inner subscriptions.</param>
        public StableCompositeSubscription(IEnumerable<ISubscription> subscriptions)
        {
            if (subscriptions == null)
            {
                throw new ArgumentNullException(nameof(subscriptions));
            }

            _subscriptions = subscriptions.ToArray();
        }

        /// <summary>
        /// Gets the number of inner subscriptions.
        /// </summary>
        /// <remarks>This number can change immediate after being read in the presence of multi-threaded usage.</remarks>
        public int Count => _subscriptions.Length;

        /// <summary>
        /// Adds the specified subscription to the list of subscriptions that this instance manages. 
        /// If the <see cref="CompositeSubscription"/> is already disposed, then the passed in subscription also gets disposed.
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
                    var newSubscriptions = new ISubscription[_subscriptions.Length + 1];
                    Array.Copy(_subscriptions, newSubscriptions, _subscriptions.Length);
                    newSubscriptions[_subscriptions.Length] = subscription;
                    _subscriptions = newSubscriptions;
                }
            }

            if (shouldDispose)
            {
                subscription.Dispose();
            }
        }

        /// <summary>
        /// Adds the specified subscriptions to the list of subscriptions that this instance manages. 
        /// If the <see cref="CompositeSubscription"/> is already disposed, then the passed in subscriptions also get disposed.
        /// </summary>
        /// <param name="subscriptions">Subscriptions to add.</param>
        public void AddRange(IEnumerable<ISubscription> subscriptions)
        {
            if (subscriptions == null)
            {
                throw new ArgumentNullException(nameof(subscriptions));
            }

            bool shouldDispose = false;
            lock (_syncLock)
            {
                shouldDispose = _disposed;

                if (!shouldDispose)
                {
                    var collection = subscriptions.AsCollection();
                    var count = collection.Count;
                    if (count > 0)
                    {
                        var length = _subscriptions.Length;
                        var newSubscriptions = new ISubscription[length + count];
                        Array.Copy(_subscriptions, newSubscriptions, length);
                        var idx = 0;
                        foreach (var subscription in collection)
                        {
                            newSubscriptions[length + idx++] = subscription;
                        }
                        _subscriptions = newSubscriptions;
                    }
                }
            }

            if (shouldDispose)
            {
                foreach (var subscription in subscriptions)
                {
                    subscription.Dispose();
                }
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
                var position = Array.IndexOf(_subscriptions, subscription);

                if (position >= 0)
                {
                    subscriptionFound = true;

                    var length = _subscriptions.Length;
                    if (length > 1)
                    {
                        var newSubscriptions = new ISubscription[length - 1];
                        Array.Copy(_subscriptions, newSubscriptions, position);
                        Array.Copy(_subscriptions, position + 1, newSubscriptions, position, length - position - 1);
                        _subscriptions = newSubscriptions;
                    }
                    else
                    {
                        _subscriptions = Array.Empty<ISubscription>();
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
            var snapshot = _subscriptions;
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
        public IEnumerator<ISubscription> GetEnumerator()
        {
            return ((IEnumerable<ISubscription>)_subscriptions).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

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
                    snapshot = _subscriptions;

                    // clear the internal list so that we can't alter its references through Remove calls 
                    _subscriptions = Array.Empty<ISubscription>();

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
    }
}
