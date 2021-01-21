// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
//

using System;
using System.Linq.Expressions;

namespace Reaqtor
{
    /// <summary>
    /// Base class for the implementation of observables represented by an expression tree.
    /// </summary>
    /// <typeparam name="T">Type of the data produced by the observable.</typeparam>
    public abstract class ReactiveQbservableBase<T> : ReactiveObservableBase<T>, IReactiveQbservable<T>
    {
        /// <summary>
        /// Creates a new observable represented by an expression tree, using the specified associated query provider.
        /// </summary>
        /// <param name="provider">Query provider associated with the observable.</param>
        protected ReactiveQbservableBase(IReactiveQueryProvider provider)
        {
            Provider = provider;
        }

        /// <summary>
        /// Gets the type of the data produced by the observable.
        /// </summary>
        public Type ElementType => typeof(T);

        /// <summary>
        /// Gets the query provider that is associated with the observable.
        /// </summary>
        public IReactiveQueryProvider Provider { get; }

        /// <summary>
        /// Gets the expression tree representing the observable.
        /// </summary>
        public abstract Expression Expression { get; }

        /// <summary>
        /// Subscribes to the observable using the given observer.
        /// </summary>
        /// <param name="observer">Observer to send the observable's data to.</param>
        /// <param name="subscriptionUri">URI to identify the subscription.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <returns>A subscription object that can be used to cancel the subscription, or an exception if the submission was unsuccessful.</returns>
        public IReactiveQubscription Subscribe(IReactiveQbserver<T> observer, Uri subscriptionUri, object state = null)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (subscriptionUri == null)
                throw new ArgumentNullException(nameof(subscriptionUri));

            return SubscribeCore(observer, subscriptionUri, state);
        }

        /// <summary>
        /// Subscribes to the observable using the given observer.
        /// </summary>
        /// <param name="observer">Observer to send the observable's data to.</param>
        /// <param name="subscriptionUri">URI to identify the subscription.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <returns>A subscription object that can be used to cancel the subscription, or an exception if the submission was unsuccessful.</returns>
        protected abstract IReactiveQubscription SubscribeCore(IReactiveQbserver<T> observer, Uri subscriptionUri, object state);

        /// <summary>
        /// Subscribes to the observable using the given observer.
        /// </summary>
        /// <param name="observer">Observer to send the observable's data to.</param>
        /// <param name="subscriptionUri">URI to identify the subscription.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <returns>A subscription object that can be used to cancel the subscription, or an exception if the submission was unsuccessful.</returns>
        protected override IReactiveSubscription SubscribeCore(IReactiveObserver<T> observer, Uri subscriptionUri, object state)
        {
            if (observer is IExpressible expressible)
            {
                var qbserver = Provider.CreateQbserver<T>(expressible.Expression);
                var qubscription = SubscribeCore(qbserver, subscriptionUri, state);
                return qubscription;
            }

            //
            // NB: A sophisticated client could support this by creating a "reverse proxy" stream in the service and
            //     create a channel to ship events from the stream to the local observer instance. However, it does
            //     require more than this in order to deal with client disconnects and cleanup of service-side resources
            //     or support some form of client-side recovery to reassociate an observer instance to an existing
            //     query (given its subscription URI).
            //

            throw new NotSupportedException("Local observer cannot be subscribed to a remote observable.");
        }
    }
}
