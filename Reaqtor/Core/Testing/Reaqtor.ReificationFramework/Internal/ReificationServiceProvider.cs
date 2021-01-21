// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

using Reaqtor.TestingFramework;

namespace Reaqtor.ReificationFramework
{
    internal class ReificationServiceProvider : IReactiveServiceProvider
    {
        private readonly List<ServiceOperation> _operations = new();

        public ServiceOperation[] Operations => _operations.ToArray();

        public async Task CreateSubscriptionAsync(Uri subscriptionUri, Expression subscription, object state, CancellationToken token)
        {
            await AddAsync(new CreateSubscription(subscriptionUri, subscription, state)).ConfigureAwait(false);
        }

        public async Task DeleteSubscriptionAsync(Uri subscriptionUri, CancellationToken token)
        {
            await AddAsync(new DeleteSubscription(subscriptionUri)).ConfigureAwait(false);
        }

        public async Task CreateStreamAsync(Uri streamUri, Expression stream, object state, CancellationToken token)
        {
            await AddAsync(new CreateStream(streamUri, stream, state)).ConfigureAwait(false);
        }

        public async Task DeleteStreamAsync(Uri streamUri, CancellationToken token)
        {
            await AddAsync(new DeleteStream(streamUri)).ConfigureAwait(false);
        }

        public Task<IAsyncReactiveObserver<T>> GetObserverAsync<T>(Uri observerUri, CancellationToken token)
        {
            return Task.FromResult<IAsyncReactiveObserver<T>>(new Observer<T>(observerUri, this));
        }

        public async Task DefineObservableAsync(Uri observableUri, Expression observable, object state, CancellationToken token)
        {
            await AddAsync(new DefineObservable(observableUri, observable, state)).ConfigureAwait(false);
        }

        public async Task UndefineObservableAsync(Uri observableUri, CancellationToken token)
        {
            await AddAsync(new UndefineObservable(observableUri)).ConfigureAwait(false);
        }

        public async Task DefineObserverAsync(Uri observerUri, Expression observer, object state, CancellationToken token)
        {
            await AddAsync(new DefineObserver(observerUri, observer, state)).ConfigureAwait(false);
        }

        public async Task UndefineObserverAsync(Uri observerUri, CancellationToken token)
        {
            await AddAsync(new UndefineObserver(observerUri)).ConfigureAwait(false);
        }

        public async Task DefineStreamFactoryAsync(Uri streamFactoryUri, Expression streamFactory, object state, CancellationToken token)
        {
            await AddAsync(new DefineStreamFactory(streamFactoryUri, streamFactory, state)).ConfigureAwait(false);
        }

        public async Task UndefineStreamFactoryAsync(Uri streamFactoryUri, CancellationToken token)
        {
            await AddAsync(new UndefineStreamFactory(streamFactoryUri)).ConfigureAwait(false);
        }

        public async Task DefineSubscriptionFactoryAsync(Uri subscriptionFactoryUri, Expression subscriptionFactory, object state, CancellationToken token)
        {
            await AddAsync(new DefineSubscriptionFactory(subscriptionFactoryUri, subscriptionFactory, state)).ConfigureAwait(false);
        }

        public async Task UndefineSubscriptionFactoryAsync(Uri subscriptionFactoryUri, CancellationToken token)
        {
            await AddAsync(new UndefineSubscriptionFactory(subscriptionFactoryUri)).ConfigureAwait(false);
        }

        private Task AddAsync(ServiceOperation operation)
        {
            _operations.Add(operation);
            return Task.FromResult(true);
        }

        public IQueryProvider Provider => new QueryProvider(this);

        private sealed class Observer<T> : IAsyncReactiveObserver<T>
        {
            private readonly Uri _observerUri;
            private readonly ReificationServiceProvider _parent;

            public Observer(Uri observerUri, ReificationServiceProvider parent)
            {
                _observerUri = observerUri;
                _parent = parent;
            }

            public async Task OnNextAsync(T value, CancellationToken token)
            {
                await _parent.AddAsync(new ObserverOnNext<T>(_observerUri, value)).ConfigureAwait(false);
            }

            public async Task OnErrorAsync(Exception error, CancellationToken token)
            {
                await _parent.AddAsync(new ObserverOnError<T>(_observerUri, error)).ConfigureAwait(false);
            }

            public async Task OnCompletedAsync(CancellationToken token)
            {
                await _parent.AddAsync(new ObserverOnCompleted<T>(_observerUri)).ConfigureAwait(false);
            }
        }

        private sealed class QueryProvider : IQueryProvider
        {
            private readonly ReificationServiceProvider _parent;

            public QueryProvider(ReificationServiceProvider parent) => _parent = parent;

            public IQueryable<TElement> CreateQuery<TElement>(Expression expression) => new Queryable<TElement>(this, expression);

            public IQueryable CreateQuery(Expression expression) => throw new NotImplementedException();

            public TResult Execute<TResult>(Expression expression)
            {
                _parent._operations.Add(new MetadataQuery(expression));

                var ie = typeof(TResult).FindGenericType(typeof(IEnumerable<>));
                if (ie != null)
                {
                    var elementType = ie.GetGenericArguments()[0];
                    return (TResult)(object)Array.CreateInstance(elementType, 0);
                }

                return default;
            }

            public object Execute(Expression expression) => throw new NotImplementedException();

            private sealed class Queryable<T> : IQueryable<T>
            {
                public Queryable(IQueryProvider provider, Expression expression)
                {
                    Provider = provider;
                    Expression = expression;
                }

                public Type ElementType => typeof(T);

                public Expression Expression { get; }

                public IQueryProvider Provider { get; }

                public IEnumerator<T> GetEnumerator()
                {
                    return Provider.Execute<IEnumerable<T>>(Expression).GetEnumerator();
                }

                System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
            }
        }
    }
}
