// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq.Expressions;

using Reaqtor.Reliable.Client;

namespace Reaqtor.Reliable.Expressions
{
    public abstract class ReliableQubscriptionFactoryBase : IReliableQubscriptionFactory
    {
        protected ReliableQubscriptionFactoryBase(IReliableQueryProvider provider) => Provider = provider;

        public IReliableQubscription Create(Uri subscriptionUri, object state = null)
        {
            if (subscriptionUri == null)
                throw new ArgumentNullException(nameof(subscriptionUri));

            return CreateCore(subscriptionUri, state);
        }

        IReliableReactiveSubscription IReliableReactiveSubscriptionFactory.Create(Uri subscriptionUri, object state)
        {
            if (subscriptionUri == null)
                throw new ArgumentNullException(nameof(subscriptionUri));

            return CreateCore(subscriptionUri, state);
        }

        protected abstract IReliableQubscription CreateCore(Uri subscriptionUri, object state);

        public IReliableQueryProvider Provider { get; }

        public abstract Expression Expression { get; }
    }

    public abstract class ReliableQubscriptionFactoryBase<TArg> : IReliableQubscriptionFactory<TArg>
    {
        protected ReliableQubscriptionFactoryBase(IReliableQueryProvider provider) => Provider = provider;

        public IReliableQubscription Create(Uri subscriptionUri, TArg argument, object state = null)
        {
            if (subscriptionUri == null)
                throw new ArgumentNullException(nameof(subscriptionUri));

            return CreateCore(subscriptionUri, argument, state);
        }

        IReliableReactiveSubscription IReliableReactiveSubscriptionFactory<TArg>.Create(Uri subscriptionUri, TArg argument, object state)
        {
            if (subscriptionUri == null)
                throw new ArgumentNullException(nameof(subscriptionUri));

            return CreateCore(subscriptionUri, argument, state);
        }

        protected abstract IReliableQubscription CreateCore(Uri subscriptionUri, TArg argument, object state);

        public IReliableQueryProvider Provider { get; }

        public abstract Expression Expression { get; }
    }

}
