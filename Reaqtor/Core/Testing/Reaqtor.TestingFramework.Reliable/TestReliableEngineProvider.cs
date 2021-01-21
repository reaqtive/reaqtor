// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - July 2013 - Created this file.
// ER - October 2013 - Minor tweaks.
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

using Reaqtor.Reliable.Engine;
using Reaqtor.Reliable.Client;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Reaqtor.TestingFramework
{
    public class TestReliableEngineProvider : IReliableReactiveEngineProvider
    {
        public void CreateSubscription(Uri subscriptionUri, Expression subscription, object state)
        {
            AssertCreateSubscription(subscriptionUri, subscription, state);
        }

        protected virtual void AssertCreateSubscription(Uri subscriptionUri, Expression subscription, object state)
        {
            Assert.Fail("Did not expect CreateSubscription call.");
        }

        public void DeleteSubscription(Uri subscriptionUri)
        {
            AssertDeleteSubscription(subscriptionUri);
        }

        protected virtual void AssertDeleteSubscription(Uri subscriptionUri)
        {
            Assert.Fail("Did not expect DeleteSubscription call.");
        }

        public void CreateStream(Uri streamUri, Expression stream, object state)
        {
            AssertCreateStream(streamUri, stream, state);
        }

        protected virtual void AssertCreateStream(Uri streamUri, Expression stream, object state)
        {
            Assert.Fail("Did not expect CreateStream call.");
        }

        public void DeleteStream(Uri streamUri)
        {
            AssertDeleteStream(streamUri);
        }

        protected virtual void AssertDeleteStream(Uri streamUri)
        {
            Assert.Fail("Did not expect DeleteStream call.");
        }

        public IReliableReactiveObserver<T> GetObserver<T>(Uri observerUri) => new ReliableObserver<T>(this, observerUri);

        private sealed class ReliableObserver<T> : IReliableReactiveObserver<T>
        {
            private readonly TestReliableEngineProvider _parent;
            private readonly Uri _observerUri;

            public ReliableObserver(TestReliableEngineProvider parent, Uri observerUri)
            {
                _parent = parent;
                _observerUri = observerUri;
            }

            public Uri ResubscribeUri => throw new NotImplementedException();

            public void OnNext(T item, long sequenceId) => _parent.OnNext<T>(_observerUri, item, sequenceId);

            public void OnError(Exception error) => _parent.OnError(_observerUri, error);

            public void OnCompleted() => _parent.OnCompleted(_observerUri);

            public void OnStarted() => _parent.OnStarted(_observerUri);
        }

        private void OnNext<T>(Uri observerUri, T value, long sequenceId)
        {
            _ = sequenceId; // TODO: Support annother Assert method.
            AssertOnNext<T>(observerUri, value);
        }

        protected virtual void AssertOnNext<T>(Uri observerUri, T value)
        {
            Assert.Fail("Did not expect OnNext call.");
        }

#pragma warning disable CA1716 // Identifiers should not match keywords. (Using error from `IObserver<T>.OnError(Exception error)`.)

        private void OnError(Uri observerUri, Exception error)
        {
            AssertOnError(observerUri, error);
        }

        protected virtual void AssertOnError(Uri observerUri, Exception error)
        {
            Assert.Fail("Did not expect OnError call.");
        }

#pragma warning restore CA1716

        private void OnCompleted(Uri observerUri)
        {
            AssertOnCompleted(observerUri);
        }

        protected virtual void AssertOnCompleted(Uri observerUri)
        {
            Assert.Fail("Did not expect OnCompleted call.");
        }

        private void OnStarted(Uri observerUri)
        {
            AssertOnStarted(observerUri);
        }

        protected virtual void AssertOnStarted(Uri observerUri)
        {
            Assert.Fail("Did not expect OnStarted call.");
        }

        public void DefineObservable(Uri observableUri, Expression observable, object state)
        {
            AssertDefineObservable(observableUri, observable, state);
        }

        protected virtual void AssertDefineObservable(Uri observableUri, Expression observable, object state)
        {
            Assert.Fail("Did not expect DefineObservable call.");
        }

        public void UndefineObservable(Uri observableUri)
        {
            AssertUndefineObservable(observableUri);
        }

        protected virtual void AssertUndefineObservable(Uri observableUri)
        {
            Assert.Fail("Did not expect UndefineObservable call.");
        }

        public void DefineObserver(Uri observerUri, Expression observer, object state)
        {
            AssertDefineObserver(observerUri, observer, state);
        }

        protected virtual void AssertDefineObserver(Uri observerUri, Expression observer, object state)
        {
            Assert.Fail("Did not expect DefineObserver call.");
        }

        public void UndefineObserver(Uri observerUri)
        {
            AssertUndefineObserver(observerUri);
        }

        protected virtual void AssertUndefineObserver(Uri observerUri)
        {
            Assert.Fail("Did not expect UndefineObserver call.");
        }

        public void DefineStreamFactory(Uri streamFactoryUri, Expression streamFactory, object state)
        {
            AssertDefineStreamFactory(streamFactoryUri, streamFactory, state);
        }

        protected virtual void AssertDefineStreamFactory(Uri streamFactoryUri, Expression streamFactory, object state)
        {
            Assert.Fail("Did not expect DefineStreamFactory call.");
        }

        public void UndefineStreamFactory(Uri streamFactoryUri)
        {
            AssertUndefineStreamFactory(streamFactoryUri);
        }

        protected virtual void AssertUndefineStreamFactory(Uri streamFactoryUri)
        {
            Assert.Fail("Did not expect UndefineStreamFactory call.");
        }

        public void DefineSubscriptionFactory(Uri subscriptionFactoryUri, Expression subscriptionFactory, object state)
        {
            AssertDefineSubscriptionFactory(subscriptionFactoryUri, subscriptionFactory, state);
        }

        protected virtual void AssertDefineSubscriptionFactory(Uri subscriptionFactoryUri, Expression subscriptionFactory, object state)
        {
            Assert.Fail("Did not expect DefineSubscriptionFactory call.");
        }

        public void UndefineSubscriptionFactory(Uri subscriptionFactoryUri)
        {
            AssertUndefineSubscriptionFactory(subscriptionFactoryUri);
        }

        protected virtual void AssertUndefineSubscriptionFactory(Uri subscriptionFactoryUri)
        {
            Assert.Fail("Did not expect UndefineSubscriptionFactory call.");
        }

        public IQueryProvider Provider => new MetadataQueryProvider(this);

        private sealed class MetadataQueryProvider : IQueryProvider
        {
            private readonly TestReliableEngineProvider _parent;

            public MetadataQueryProvider(TestReliableEngineProvider parent)
            {
                _parent = parent;
            }

            public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
            {
                return new MetadataQuery<TElement>(this, expression);
            }

            private sealed class MetadataQuery<T> : IQueryable<T>
            {
                public MetadataQuery(IQueryProvider provider, Expression expression)
                {
                    Provider = provider;
                    Expression = expression;
                }

                public IEnumerator<T> GetEnumerator()
                {
                    return Provider.Execute<IEnumerable<T>>(Expression).GetEnumerator();
                }

                System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
                {
                    return GetEnumerator();
                }

                public Type ElementType => typeof(T);

                public Expression Expression { get; }

                public IQueryProvider Provider { get; }
            }

            public IQueryable CreateQuery(Expression expression)
            {
                throw new NotImplementedException();
            }

            public TResult Execute<TResult>(Expression expression)
            {
                _parent.AssertMetadataQuery(expression);

                var ie = typeof(TResult).FindGenericType(typeof(IEnumerable<>));
                if (ie != null)
                {
                    var elementType = ie.GetGenericArguments()[0];
                    return (TResult)(object)Array.CreateInstance(elementType, 0);
                }

                return default;
            }

            public object Execute(Expression expression)
            {
                throw new NotImplementedException();
            }
        }

        public void StartSubscription(Uri subscriptionUri, long sequenceId)
        {
            throw new NotImplementedException();
        }

        public void AcknowledgeRange(Uri subscriptionUri, long sequenceId)
        {
            throw new NotImplementedException();
        }

        public Uri GetSubscriptionResubscribeUri(Uri subscriptionUri)
        {
            throw new NotImplementedException();
        }

        public void CreateObserver(Uri streamUri)
        {
            AssertCreateObserver(streamUri);
        }

        public virtual void AssertCreateObserver(Uri streamUri)
        {
            Assert.Fail("Did not expect CreateObserver call.");
        }

        public virtual void AssertMetadataQuery(Expression expression)
        {
            Assert.Fail("Did not expect metadata query execution.");
        }
    }
}
