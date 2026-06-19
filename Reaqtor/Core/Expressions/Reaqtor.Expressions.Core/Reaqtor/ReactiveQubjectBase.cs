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

namespace Reaqtor
{
    /// <summary>
    /// Base class for the implementation of subjects represented by an expression tree.
    /// </summary>
    /// <typeparam name="TInput">Type of the data received by the subject.</typeparam>
    /// <typeparam name="TOutput">Type of the data produced by the subject.</typeparam>
    public abstract class ReactiveQubjectBase<TInput, TOutput> : ReactiveSubjectBase<TInput, TOutput>, IReactiveQubject<TInput, TOutput>
    {
        /// <summary>
        /// Creates a new subject represented by an expression tree, using the specified associated query provider.
        /// </summary>
        /// <param name="provider">Query provider associated with the subject.</param>
        protected ReactiveQubjectBase(IReactiveQueryProvider provider) => Provider = provider;

        /// <summary>
        /// Gets the type of the data received by the subject.
        /// </summary>
        protected Type InputType => typeof(TInput);

        /// <summary>
        /// Gets the type of the data received by the subject.
        /// </summary>
        Type IReactiveQbserver.ElementType => InputType;

        /// <summary>
        /// Gets the type of the data produced by the subject.
        /// </summary>
        protected Type OutputType => typeof(TOutput);

        /// <summary>
        /// Gets the type of the data produced by the subject.
        /// </summary>
        Type IReactiveQbservable.ElementType => OutputType;

        /// <summary>
        /// Gets the query provider that is associated with the subject.
        /// </summary>
        public IReactiveQueryProvider Provider { get; }

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
        /// <returns>A subscription object that can be used to cancel the subscription, or an exception if the submission was unsuccessful.</returns>
        public IReactiveQubscription Subscribe(IReactiveQbserver<TOutput> observer, Uri subscriptionUri, object state = null)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (subscriptionUri == null)
                throw new ArgumentNullException(nameof(subscriptionUri));

            return SubscribeCore(observer, subscriptionUri, state);
        }

        /// <summary>
        /// Subscribes to the subject using the given observer.
        /// </summary>
        /// <param name="observer">Observer to send the subject's data to.</param>
        /// <param name="subscriptionUri">URI to identify the subscription.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <returns>A subscription object that can be used to cancel the subscription, or an exception if the submission was unsuccessful.</returns>
        protected abstract IReactiveQubscription SubscribeCore(IReactiveQbserver<TOutput> observer, Uri subscriptionUri, object state);

        /// <summary>
        /// Subscribes to the subject using the given observer.
        /// </summary>
        /// <param name="observer">Observer to send the subject's data to.</param>
        /// <param name="subscriptionUri">URI to identify the subscription.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <returns>A subscription object that can be used to cancel the subscription, or an exception if the submission was unsuccessful.</returns>
        protected override IReactiveSubscription SubscribeCore(IReactiveObserver<TOutput> observer, Uri subscriptionUri, object state)
        {
            if (observer is IExpressible expressible)
            {
                var qbserver = Provider.CreateQbserver<TOutput>(expressible.Expression);
                var qubscription = SubscribeCore(qbserver, subscriptionUri, state);
                return qubscription;
            }

            throw new NotSupportedException("Local observer cannot be subscribed to a remote subject."); // TODO: Provide fallback plan?
        }
    }
}
