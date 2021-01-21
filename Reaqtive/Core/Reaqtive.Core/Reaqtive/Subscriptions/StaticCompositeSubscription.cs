// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Reaqtive
{
    /// <summary>
    /// Composite subscription with a static set of inner subscriptions.
    /// </summary>
    public sealed class StaticCompositeSubscription : StaticCompositeSubscriptionBase
    {
        private readonly ISubscription[] _subscriptions;

        /// <summary>
        /// Creates a new composite subscription with the specified inner subscriptions.
        /// </summary>
        /// <param name="subscriptions">Inner subscriptions.</param>
        /// <remarks>This constructor overload does not clone the <paramref name="subscriptions"/> collection.</remarks>
        public StaticCompositeSubscription(params ISubscription[] subscriptions)
        {
            _subscriptions = subscriptions ?? throw new ArgumentNullException(nameof(subscriptions));
        }

        /// <summary>
        /// Creates a new composite subscription with the specified inner subscriptions.
        /// </summary>
        /// <param name="subscriptions">Inner subscriptions.</param>
        /// <remarks>This constructor overload clones the <paramref name="subscriptions"/> collection.</remarks>
        public StaticCompositeSubscription(IEnumerable<ISubscription> subscriptions)
        {
            if (subscriptions == null)
                throw new ArgumentNullException(nameof(subscriptions));

            _subscriptions = subscriptions.ToArray();
        }

        /// <summary>
        /// Creates a new composite subscription with the specified inner subscriptions.
        /// </summary>
        /// <param name="subscription1">The first inner subscription.</param>
        /// <param name="subscription2">The second inner subscription.</param>
        public static ICompositeSubscription Create(ISubscription subscription1, ISubscription subscription2)
        {
            if (subscription1 == null)
                throw new ArgumentNullException(nameof(subscription1));
            if (subscription2 == null)
                throw new ArgumentNullException(nameof(subscription2));

            return new Binary(subscription1, subscription2);
        }

        /// <summary>
        /// Gets the number of inner subscriptions.
        /// </summary>
        public int Count => _subscriptions.Length;

        /// <summary>
        /// Accepts a visitor that will be dispatched through the subscription, causing all inner subscriptions to be visited.
        /// </summary>
        /// <param name="visitor">Visitor to accept.</param>
        public override void Accept(ISubscriptionVisitor visitor)
        {
            foreach (var subscription in _subscriptions)
            {
                subscription.Accept(visitor);
            }
        }

        /// <summary>
        /// Gets an enumerator to iterate over the inner subscriptions.
        /// </summary>
        /// <returns>Enumerator to iterate over the inner subscriptions.</returns>
        public override IEnumerator<ISubscription> GetEnumerator() => ((IEnumerable<ISubscription>)_subscriptions).GetEnumerator();

        /// <summary>
        /// Disposes the composite subscription, causing all inner subscriptions to be disposed.
        /// </summary>
        protected override void DisposeCore()
        {
            foreach (var subscription in _subscriptions)
            {
                subscription.Dispose();
            }
        }

        private sealed class Binary : StaticCompositeSubscriptionBase
        {
            private readonly ISubscription _subscription1;
            private readonly ISubscription _subscription2;

            public Binary(ISubscription subscription1, ISubscription subscription2)
            {
                _subscription1 = subscription1;
                _subscription2 = subscription2;
            }

            public override void Accept(ISubscriptionVisitor visitor)
            {
                _subscription1.Accept(visitor);
                _subscription2.Accept(visitor);
            }

            protected override void DisposeCore()
            {
                _subscription1.Dispose();
                _subscription2.Dispose();
            }

            public override IEnumerator<ISubscription> GetEnumerator()
            {
                yield return _subscription1;
                yield return _subscription2;
            }
        }
    }
}
