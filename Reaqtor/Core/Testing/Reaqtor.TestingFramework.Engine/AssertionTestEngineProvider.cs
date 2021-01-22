// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - July 2013 - Created this file.
//

using System;
using System.Linq.Expressions;

namespace Reaqtor.TestingFramework
{
    public abstract class AssertionTestEngineProvider : TestEngineProvider
    {
        protected override void AssertCreateStream(Uri streamUri, Expression stream, object state)
        {
            AssertCore(new CreateStream(streamUri, stream, state));
        }

        protected override void AssertCreateSubscription(Uri subscriptionUri, Expression subscription, object state)
        {
            AssertCore(new CreateSubscription(subscriptionUri, subscription, state));
        }

        protected override void AssertDefineObservable(Uri observableUri, Expression observable, object state)
        {
            AssertCore(new DefineObservable(observableUri, observable, state));
        }

        protected override void AssertDefineObserver(Uri observerUri, Expression observer, object state)
        {
            AssertCore(new DefineObserver(observerUri, observer, state));
        }

        protected override void AssertDefineStreamFactory(Uri streamFactoryUri, Expression streamFactory, object state)
        {
            AssertCore(new DefineStreamFactory(streamFactoryUri, streamFactory, state));
        }

        protected override void AssertDefineSubscriptionFactory(Uri subscriptionFactoryUri, Expression subscriptionFactory, object state)
        {
            AssertCore(new DefineSubscriptionFactory(subscriptionFactoryUri, subscriptionFactory, state));
        }

        protected override void AssertDeleteStream(Uri streamUri)
        {
            AssertCore(new DeleteStream(streamUri));
        }

        protected override void AssertDeleteSubscription(Uri subscriptionUri)
        {
            AssertCore(new DeleteSubscription(subscriptionUri));
        }

        protected override void AssertOnCompleted(Uri observerUri)
        {
            AssertCore(new ObserverOnCompleted(observerUri));
        }

        protected override void AssertOnError(Uri observerUri, Exception error)
        {
            AssertCore(new ObserverOnError(observerUri, error));
        }

        protected override void AssertOnNext<T>(Uri observerUri, T value)
        {
            AssertCore(new ObserverOnNext<T>(observerUri, value));
        }

        protected override void AssertUndefineObservable(Uri observableUri)
        {
            AssertCore(new UndefineObservable(observableUri));
        }

        protected override void AssertUndefineObserver(Uri observerUri)
        {
            AssertCore(new UndefineObserver(observerUri));
        }

        protected override void AssertUndefineStreamFactory(Uri streamFactoryUri)
        {
            AssertCore(new UndefineStreamFactory(streamFactoryUri));
        }

        protected override void AssertUndefineSubscriptionFactory(Uri subscriptionFactoryUri)
        {
            AssertCore(new UndefineSubscriptionFactory(subscriptionFactoryUri));
        }

        public override void AssertMetadataQuery(Expression expression)
        {
            AssertCore(new MetadataQuery(expression));
        }

        protected abstract void AssertCore(ServiceOperation operation);
    }
}
