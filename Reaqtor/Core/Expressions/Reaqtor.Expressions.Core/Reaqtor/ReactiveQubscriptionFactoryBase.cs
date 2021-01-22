// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - February 2016 - Created this file.
//

using System;
using System.Linq.Expressions;

namespace Reaqtor
{
    /// <summary>
    /// Base class for the implementation of subscription factories represented by an expression tree.
    /// </summary>
    public abstract class ReactiveQubscriptionFactoryBase : IReactiveQubscriptionFactory
    {
        /// <summary>
        /// Creates a new subscription factory represented by an expression tree, using the specified associated query provider.
        /// </summary>
        /// <param name="provider">Query provider associated with the subscription factory.</param>
        protected ReactiveQubscriptionFactoryBase(IReactiveQueryProvider provider) => Provider = provider;

        /// <summary>
        /// Creates a new subscription with the specified subscription URI.
        /// </summary>
        /// <param name="subscriptionUri">URI identifying the subscription.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <returns>A subscription object, or an exception if the creation request was unsuccessful.</returns>
        public IReactiveQubscription Create(Uri subscriptionUri, object state = null)
        {
            if (subscriptionUri == null)
                throw new ArgumentNullException(nameof(subscriptionUri));

            return CreateCore(subscriptionUri, state);
        }

        /// <summary>
        /// Creates a new subscription with the specified subscription URI.
        /// </summary>
        /// <param name="subscriptionUri">URI identifying the subscription.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <returns>A subscription object, or an exception if the creation request was unsuccessful.</returns>
        IReactiveSubscription IReactiveSubscriptionFactory.Create(Uri subscriptionUri, object state)
        {
            if (subscriptionUri == null)
                throw new ArgumentNullException(nameof(subscriptionUri));

            return CreateCore(subscriptionUri, state);
        }

        /// <summary>
        /// Creates a new subscription with the specified subscription URI.
        /// </summary>
        /// <param name="subscriptionUri">URI identifying the subscription.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <returns>A subscription object, or an exception if the creation request was unsuccessful.</returns>
        protected abstract IReactiveQubscription CreateCore(Uri subscriptionUri, object state);

        /// <summary>
        /// Gets the query provider that is associated with the subscription factory.
        /// </summary>
        public IReactiveQueryProvider Provider { get; }

        /// <summary>
        /// Gets the expression tree representing the subscription factory.
        /// </summary>
        public abstract Expression Expression { get; }
    }

    /// <summary>
    /// Base class for the implementation of parameterized subscription factories represented by an expression tree.
    /// </summary>
    /// <typeparam name="TArgs">Type of the parameter passed to the subscription factory.</typeparam>
    public abstract class ReactiveQubscriptionFactoryBase<TArgs> : IReactiveQubscriptionFactory<TArgs>
    {
        /// <summary>
        /// Creates a new subscription factory represented by an expression tree, using the specified associated query provider.
        /// </summary>
        /// <param name="provider">Query provider associated with the subscription factory.</param>
        protected ReactiveQubscriptionFactoryBase(IReactiveQueryProvider provider) => Provider = provider;

        /// <summary>
        /// Creates a new subscription with the specified subscription URI.
        /// </summary>
        /// <param name="subscriptionUri">URI identifying the subscription.</param>
        /// <param name="argument">Parameter to pass to the subscription factory.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <returns>A subscription object, or an exception if the creation request was unsuccessful.</returns>
        public IReactiveQubscription Create(Uri subscriptionUri, TArgs argument, object state = null)
        {
            if (subscriptionUri == null)
                throw new ArgumentNullException(nameof(subscriptionUri));

            return CreateCore(subscriptionUri, argument, state);
        }

        /// <summary>
        /// Creates a new subscription with the specified subscription URI.
        /// </summary>
        /// <param name="subscriptionUri">URI identifying the subscription.</param>
        /// <param name="argument">Parameter to pass to the subscription factory.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <returns>A subscription object, or an exception if the creation request was unsuccessful.</returns>
        IReactiveSubscription IReactiveSubscriptionFactory<TArgs>.Create(Uri subscriptionUri, TArgs argument, object state)
        {
            if (subscriptionUri == null)
                throw new ArgumentNullException(nameof(subscriptionUri));

            return CreateCore(subscriptionUri, argument, state);
        }

        /// <summary>
        /// Creates a new subscription with the specified subscription URI.
        /// </summary>
        /// <param name="subscriptionUri">URI identifying the subscription.</param>
        /// <param name="argument">Parameter to pass to the subscription factory.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <returns>A subscription object, or an exception if the creation request was unsuccessful.</returns>
        protected abstract IReactiveQubscription CreateCore(Uri subscriptionUri, TArgs argument, object state);

        /// <summary>
        /// Gets the query provider that is associated with the subscription factory.
        /// </summary>
        public IReactiveQueryProvider Provider { get; }

        /// <summary>
        /// Gets the expression tree representing the subscription factory.
        /// </summary>
        public abstract Expression Expression { get; }
    }
}
