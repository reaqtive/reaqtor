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
using System.Threading;
using System.Threading.Tasks;

namespace Reaqtor
{
    /// <summary>
    /// Base class for the implementation of subscription factories represented by an expression tree.
    /// </summary>
    public abstract class AsyncReactiveQubscriptionFactoryBase : IAsyncReactiveQubscriptionFactory
    {
        /// <summary>
        /// Creates a new subscription factory represented by an expression tree, using the specified associated query provider.
        /// </summary>
        /// <param name="provider">Query provider associated with the subscription factory.</param>
        protected AsyncReactiveQubscriptionFactoryBase(IAsyncReactiveQueryProvider provider) => Provider = provider;

        /// <summary>
        /// Creates a new subscription with the specified subscription URI.
        /// </summary>
        /// <param name="subscriptionUri">URI identifying the subscription.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <param name="token">Token to observe for cancellation of the request.</param>
        /// <returns>Task returning a subscription object, or an exception if the creation request was unsuccessful.</returns>
        public Task<IAsyncReactiveQubscription> CreateAsync(Uri subscriptionUri, object state = null, CancellationToken token = default)
        {
            if (subscriptionUri == null)
                throw new ArgumentNullException(nameof(subscriptionUri));

            return CreateAsyncCore(subscriptionUri, state, token);
        }

        /// <summary>
        /// Creates a new subscription with the specified subscription URI.
        /// </summary>
        /// <param name="subscriptionUri">URI identifying the subscription.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <param name="token">Token to observe for cancellation of the request.</param>
        /// <returns>Task returning a subscription object, or an exception if the creation request was unsuccessful.</returns>
        Task<IAsyncReactiveSubscription> IAsyncReactiveSubscriptionFactory.CreateAsync(Uri subscriptionUri, object state, CancellationToken token)
        {
            if (subscriptionUri == null)
                throw new ArgumentNullException(nameof(subscriptionUri));

            return CreateAsyncCore(subscriptionUri, state, token).ContinueWith(t => (IAsyncReactiveSubscription)t.Result, token, TaskContinuationOptions.OnlyOnRanToCompletion, TaskScheduler.Default);
        }

        /// <summary>
        /// Creates a new subscription with the specified subscription URI.
        /// </summary>
        /// <param name="subscriptionUri">URI identifying the subscription.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <param name="token">Token to observe for cancellation of the request.</param>
        /// <returns>Task returning a subscription object, or an exception if the creation request was unsuccessful.</returns>
        protected abstract Task<IAsyncReactiveQubscription> CreateAsyncCore(Uri subscriptionUri, object state, CancellationToken token);

        /// <summary>
        /// Gets the query provider that is associated with the subscription factory.
        /// </summary>
        public IAsyncReactiveQueryProvider Provider { get; }

        /// <summary>
        /// Gets the expression tree representing the subscription factory.
        /// </summary>
        public abstract Expression Expression { get; }
    }

    /// <summary>
    /// Base class for the implementation of parameterized subscription factories represented by an expression tree.
    /// </summary>
    /// <typeparam name="TArgs">Type of the parameter passed to the subscription factory.</typeparam>
    public abstract class AsyncReactiveQubscriptionFactoryBase<TArgs> : IAsyncReactiveQubscriptionFactory<TArgs>
    {
        /// <summary>
        /// Creates a new subscription factory represented by an expression tree, using the specified associated query provider.
        /// </summary>
        /// <param name="provider">Query provider associated with the subscription factory.</param>
        protected AsyncReactiveQubscriptionFactoryBase(IAsyncReactiveQueryProvider provider) => Provider = provider;

        /// <summary>
        /// Creates a new subscription with the specified subscription URI.
        /// </summary>
        /// <param name="subscriptionUri">URI identifying the subscription.</param>
        /// <param name="argument">Parameter to pass to the subscription factory.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <param name="token">Token to observe for cancellation of the request.</param>
        /// <returns>Task returning a subscription object, or an exception if the creation request was unsuccessful.</returns>
        public Task<IAsyncReactiveQubscription> CreateAsync(Uri subscriptionUri, TArgs argument, object state = null, CancellationToken token = default)
        {
            if (subscriptionUri == null)
                throw new ArgumentNullException(nameof(subscriptionUri));

            return CreateAsyncCore(subscriptionUri, argument, state, token);
        }

        /// <summary>
        /// Creates a new subscription with the specified subscription URI.
        /// </summary>
        /// <param name="subscriptionUri">URI identifying the subscription.</param>
        /// <param name="argument">Parameter to pass to the subscription factory.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <param name="token">Token to observe for cancellation of the request.</param>
        /// <returns>Task returning a subscription object, or an exception if the creation request was unsuccessful.</returns>
        Task<IAsyncReactiveSubscription> IAsyncReactiveSubscriptionFactory<TArgs>.CreateAsync(Uri subscriptionUri, TArgs argument, object state, CancellationToken token)
        {
            if (subscriptionUri == null)
                throw new ArgumentNullException(nameof(subscriptionUri));

            return CreateAsyncCore(subscriptionUri, argument, state, token).ContinueWith(t => (IAsyncReactiveSubscription)t.Result, token, TaskContinuationOptions.OnlyOnRanToCompletion, TaskScheduler.Default);
        }

        /// <summary>
        /// Creates a new subscription with the specified subscription URI.
        /// </summary>
        /// <param name="subscriptionUri">URI identifying the subscription.</param>
        /// <param name="argument">Parameter to pass to the subscription factory.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <param name="token">Token to observe for cancellation of the request.</param>
        /// <returns>Task returning a subscription object, or an exception if the creation request was unsuccessful.</returns>
        protected abstract Task<IAsyncReactiveQubscription> CreateAsyncCore(Uri subscriptionUri, TArgs argument, object state, CancellationToken token);

        /// <summary>
        /// Gets the query provider that is associated with the subscription factory.
        /// </summary>
        public IAsyncReactiveQueryProvider Provider { get; }

        /// <summary>
        /// Gets the expression tree representing the subscription factory.
        /// </summary>
        public abstract Expression Expression { get; }
    }
}
