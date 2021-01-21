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

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Reaqtor.TestingFramework
{
    public class TestServiceProvider : IReactiveServiceProvider
    {
        private readonly Task _done = Task.FromResult(true);

        public Task CreateSubscriptionAsync(Uri subscriptionUri, Expression subscription, object state, CancellationToken token)
        {
            AssertCreateSubscription(subscriptionUri, subscription, state);
            return _done;
        }

        protected virtual void AssertCreateSubscription(Uri subscriptionUri, Expression subscription, object state)
        {
            Assert.Fail("Did not expect CreateSubscriptionAsync call.");
        }

        public Task DeleteSubscriptionAsync(Uri subscriptionUri, CancellationToken token)
        {
            AssertDeleteSubscription(subscriptionUri);
            return _done;
        }

        protected virtual void AssertDeleteSubscription(Uri subscriptionUri)
        {
            Assert.Fail("Did not expect DeleteSubscriptionAsync call.");
        }

        public Task CreateStreamAsync(Uri streamUri, Expression stream, object state, CancellationToken token)
        {
            AssertCreateStream(streamUri, stream, state);
            return _done;
        }

        protected virtual void AssertCreateStream(Uri streamUri, Expression stream, object state)
        {
            Assert.Fail("Did not expect CreateStreamAsync call.");
        }

        public Task DeleteStreamAsync(Uri streamUri, CancellationToken token)
        {
            AssertDeleteStream(streamUri);
            return _done;
        }

        protected virtual void AssertDeleteStream(Uri streamUri)
        {
            Assert.Fail("Did not expect DeleteStreamAsync call.");
        }

        public Task<IAsyncReactiveObserver<T>> GetObserverAsync<T>(Uri observerUri, CancellationToken token)
        {
            var observer = new AsyncObserver<T>(this, observerUri);
            return Task.FromResult<IAsyncReactiveObserver<T>>(observer);
        }

        private sealed class AsyncObserver<T> : IAsyncReactiveObserver<T>
        {
            private readonly TestServiceProvider _parent;
            private readonly Uri _observerUri;

            public AsyncObserver(TestServiceProvider parent, Uri observerUri)
            {
                _parent = parent;
                _observerUri = observerUri;
            }

            public Task OnNextAsync(T value, CancellationToken token) => _parent.OnNextAsync<T>(_observerUri, value);

            public Task OnErrorAsync(Exception error, CancellationToken token) => _parent.OnErrorAsync(_observerUri, error);

            public Task OnCompletedAsync(CancellationToken token) => _parent.OnCompletedAsync(_observerUri);
        }

        private Task OnNextAsync<T>(Uri observerUri, T value)
        {
            AssertOnNext<T>(observerUri, value);
            return _done;
        }

        protected virtual void AssertOnNext<T>(Uri observerUri, T value)
        {
            Assert.Fail("Did not expect OnNextAsync call.");
        }

#pragma warning disable CA1716 // Identifiers should not match keywords. (Using error from `IObserver<T>.OnError(Exception error)`.)

        private Task OnErrorAsync(Uri observerUri, Exception error)
        {
            AssertOnError(observerUri, error);
            return _done;
        }

        protected virtual void AssertOnError(Uri observerUri, Exception error)
        {
            Assert.Fail("Did not expect OnErrorAsync call.");
        }

#pragma warning restore CA1716

        private Task OnCompletedAsync(Uri observerUri)
        {
            AssertOnCompleted(observerUri);
            return _done;
        }

        protected virtual void AssertOnCompleted(Uri observerUri)
        {
            Assert.Fail("Did not expect OnCompletedAsync call.");
        }

        public Task DefineObservableAsync(Uri observableUri, Expression observable, object state, CancellationToken token)
        {
            AssertDefineObservable(observableUri, observable, state);
            return _done;
        }

        protected virtual void AssertDefineObservable(Uri observableUri, Expression observable, object state)
        {
            Assert.Fail("Did not expect DefineObservableAsync call.");
        }

        public Task UndefineObservableAsync(Uri observableUri, CancellationToken token)
        {
            AssertUndefineObservable(observableUri);
            return _done;
        }

        protected virtual void AssertUndefineObservable(Uri observableUri)
        {
            Assert.Fail("Did not expect UndefineObservableAsync call.");
        }

        public Task DefineObserverAsync(Uri observerUri, Expression observer, object state, CancellationToken token)
        {
            AssertDefineObserver(observerUri, observer, state);
            return _done;
        }

        protected virtual void AssertDefineObserver(Uri observerUri, Expression observer, object state)
        {
            Assert.Fail("Did not expect DefineObserverAsync call.");
        }

        public Task UndefineObserverAsync(Uri observerUri, CancellationToken token)
        {
            AssertUndefineObserver(observerUri);
            return _done;
        }

        protected virtual void AssertUndefineObserver(Uri observerUri)
        {
            Assert.Fail("Did not expect UndefineObserverAsync call.");
        }

        public Task DefineStreamFactoryAsync(Uri streamFactoryUri, Expression streamFactory, object state, CancellationToken token)
        {
            AssertDefineStreamFactory(streamFactoryUri, streamFactory, state);
            return _done;
        }

        protected virtual void AssertDefineStreamFactory(Uri streamFactoryUri, Expression streamFactory, object state)
        {
            Assert.Fail("Did not expect DefineStreamFactoryAsync call.");
        }

        public Task UndefineStreamFactoryAsync(Uri streamFactoryUri, CancellationToken token)
        {
            AssertUndefineStreamFactory(streamFactoryUri);
            return _done;
        }

        protected virtual void AssertUndefineStreamFactory(Uri streamFactoryUri)
        {
            Assert.Fail("Did not expect UndefineStreamFactoryAsync call.");
        }

        public Task DefineSubscriptionFactoryAsync(Uri subscriptionFactoryUri, Expression subscriptionFactory, object state, CancellationToken token)
        {
            AssertDefineSubscriptionFactory(subscriptionFactoryUri, subscriptionFactory, state);
            return _done;
        }

        protected virtual void AssertDefineSubscriptionFactory(Uri subscriptionFactoryUri, Expression subscriptionFactory, object state)
        {
            Assert.Fail("Did not expect DefineSubscriptionFactory call.");
        }

        public Task UndefineSubscriptionFactoryAsync(Uri subscriptionFactoryUri, CancellationToken token)
        {
            AssertUndefineSubscriptionFactory(subscriptionFactoryUri);
            return _done;
        }

        protected virtual void AssertUndefineSubscriptionFactory(Uri subscriptionFactoryUri)
        {
            Assert.Fail("Did not expect UndefineSubscriptionFactory call.");
        }

        public IQueryProvider Provider => new MetadataQueryProvider(this);

        private sealed class MetadataQueryProvider : IQueryProvider
        {
            private readonly TestServiceProvider _parent;

            public MetadataQueryProvider(TestServiceProvider parent)
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

        public virtual void AssertMetadataQuery(Expression expression)
        {
            Assert.Fail("Did not expect metadata query execution.");
        }
    }
}
