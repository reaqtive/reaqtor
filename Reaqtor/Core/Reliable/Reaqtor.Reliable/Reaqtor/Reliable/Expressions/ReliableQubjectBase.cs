// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq.Expressions;

using Reaqtor.Reliable.Client;

namespace Reaqtor.Reliable.Expressions
{
    public abstract class ReliableQubjectBase<TInput, TOutput> : ReliableReactiveMultiSubjectBase<TInput, TOutput>, IReliableMultiQubject<TInput, TOutput>
    {
        protected ReliableQubjectBase(IReliableQueryProvider provider) => Provider = provider;

        protected Type InputType => typeof(TInput);

        protected Type OutputType => typeof(TOutput);

        public Type ElementType => OutputType;

        public IReliableQueryProvider Provider { get; }

        public abstract Expression Expression { get; }

        #region Subscribe

        public IReliableQubscription Subscribe(IReliableQbserver<TOutput> observer, Uri subscriptionUri, object state = null)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (subscriptionUri == null)
                throw new ArgumentNullException(nameof(subscriptionUri));

            return SubscribeCore(observer, subscriptionUri, state);
        }

        protected abstract IReliableQubscription SubscribeCore(IReliableQbserver<TOutput> observer, Uri subscriptionUri, object state);

        protected override IReliableReactiveSubscription SubscribeCore(IReliableReactiveObserver<TOutput> observer, Uri subscriptionUri, object state)
        {
            if (observer is IExpressible expressible)
            {
                var qbserver = Provider.CreateQbserver<TOutput>(expressible.Expression);
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

            throw new NotSupportedException("Local observer cannot be subscribed to a remote subject.");
        }

        #endregion

        #region CreateObserver

        public new IReliableQbserver<TInput> CreateObserver() => CreateQbserverCore();

        protected abstract IReliableQbserver<TInput> CreateQbserverCore();

        protected override IReliableReactiveObserver<TInput> CreateObserverCore() => CreateQbserverCore();

        #endregion
    }
}
