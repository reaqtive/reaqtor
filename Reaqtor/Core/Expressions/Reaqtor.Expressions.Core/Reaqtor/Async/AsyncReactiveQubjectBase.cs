// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
//

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1033 // Explicit implementation of ElementType. (Alternative properties are provided.)

using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Reaqtor
{
    /// <summary>
    /// Base class for the implementation of subjects represented by an expression tree.
    /// </summary>
    /// <typeparam name="TInput">Type of the data received by the subject.</typeparam>
    /// <typeparam name="TOutput">Type of the data produced by the subject.</typeparam>
    public abstract class AsyncReactiveQubjectBase<TInput, TOutput> : AsyncReactiveSubjectBase<TInput, TOutput>, IAsyncReactiveQubject<TInput, TOutput>
    {
        /// <summary>
        /// Creates a new subject represented by an expression tree, using the specified associated query provider.
        /// </summary>
        /// <param name="provider">Query provider associated with the subject.</param>
        protected AsyncReactiveQubjectBase(IAsyncReactiveQueryProvider provider) => Provider = provider;

        /// <summary>
        /// Gets the type of the data received by the subject.
        /// </summary>
        protected Type InputType => typeof(TInput);

        /// <summary>
        /// Gets the type of the data received by the subject.
        /// </summary>
        Type IAsyncReactiveQbserver.ElementType => InputType;

        /// <summary>
        /// Gets the type of the data produced by the subject.
        /// </summary>
        protected Type OutputType => typeof(TOutput);

        /// <summary>
        /// Gets the type of the data produced by the subject.
        /// </summary>
        Type IAsyncReactiveQbservable.ElementType => OutputType;

        /// <summary>
        /// Gets the query provider that is associated with the subject.
        /// </summary>
        public IAsyncReactiveQueryProvider Provider { get; }

        /// <summary>
        /// Gets the expression tree representing the subject.
        /// </summary>
        public abstract Expression Expression { get; }

        /// <summary>
        /// Subscribes to the subject using the given observer.
        /// </summary>
        /// <param name="observer">Observer to send the subject's data to.</param>
        /// <param name="subscriptionUri">URI to identify the subscription.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <param name="token">Token to observe for cancellation of the request.</param>
        /// <returns>Task returning a subscription object that can be used to cancel the subscription, or an exception if the submission was unsuccessful.</returns>
        public Task<IAsyncReactiveQubscription> SubscribeAsync(IAsyncReactiveQbserver<TOutput> observer, Uri subscriptionUri, object state = null, CancellationToken token = default)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (subscriptionUri == null)
                throw new ArgumentNullException(nameof(subscriptionUri));

            return SubscribeAsyncCore(observer, subscriptionUri, state, token);
        }

        /// <summary>
        /// Subscribes to the subject using the given observer.
        /// </summary>
        /// <param name="observer">Observer to send the subject's data to.</param>
        /// <param name="subscriptionUri">URI to identify the subscription.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <param name="token">Token to observe for cancellation of the request.</param>
        /// <returns>Task returning a subscription object that can be used to cancel the subscription, or an exception if the submission was unsuccessful.</returns>
        protected abstract Task<IAsyncReactiveQubscription> SubscribeAsyncCore(IAsyncReactiveQbserver<TOutput> observer, Uri subscriptionUri, object state, CancellationToken token);

        /// <summary>
        /// Subscribes to the subject using the given observer.
        /// </summary>
        /// <param name="observer">Observer to send the subject's data to.</param>
        /// <param name="subscriptionUri">URI to identify the subscription.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <param name="token">Token to observe for cancellation of the request.</param>
        /// <returns>Task returning a subscription object that can be used to cancel the subscription, or an exception if the submission was unsuccessful.</returns>
        protected override Task<IAsyncReactiveSubscription> SubscribeAsyncCore(IAsyncReactiveObserver<TOutput> observer, Uri subscriptionUri, object state, CancellationToken token)
        {
            if (observer is IExpressible expressible)
            {
                var qbserver = Provider.CreateQbserver<TOutput>(expressible.Expression);
                return SubscribeAsyncCore(qbserver, subscriptionUri, state, token).ContinueWith(t => (IAsyncReactiveSubscription)t.Result, token, TaskContinuationOptions.OnlyOnRanToCompletion, TaskScheduler.Default);
            }

            //
            // NB: A sophisticated client could support this by creating a "reverse proxy" stream in the service and
            //     create a channel to ship events from the stream to the local observer instance. However, it does
            //     require more than this in order to deal with client disconnects and cleanup of service-side resources
            //     or support some form of client-side recovery to reassociate an observer instance to an existing
            //     query (given its subscription URI).
            //

            throw new NotSupportedException("Local observer cannot be subscribed to a remote subject.");
        }
    }
}
