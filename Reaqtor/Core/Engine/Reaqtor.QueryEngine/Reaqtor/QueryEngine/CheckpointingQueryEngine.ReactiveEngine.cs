// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using System.Linq.Expressions;

using Reaqtor.Reliable;

namespace Reaqtor.QueryEngine
{
    public partial class CheckpointingQueryEngine
    {
        /// <summary>
        /// Implementation of <see cref="IReactiveEngineProvider"/>. Calls through this interface bypass the built-in
        /// transaction log, which can be used in environments that have an external transaction log.
        /// </summary>
        /// <remarks>
        /// Historically, the core engine didn't have a transaction log in it. Because a transaction log needs I/O, we
        /// need async interfaces. Rather than having an async variant of the <see cref="IReactiveEngineProvider"/>
        /// interface, we simply piggyback on the <see cref="IReactiveServiceProvider"/>.
        ///
        /// See <see cref="AsyncReactiveEngineProvider"/> for the async variant, with transaction log applied.
        /// </remarks>
        private sealed class ReactiveEngine : IReactiveEngineProvider
        {
            private readonly CoreReactiveEngine _engine;

            public ReactiveEngine(CoreReactiveEngine engine)
            {
                _engine = engine;
            }

            public void CreateSubscription(Uri subscriptionUri, Expression subscription, object state)
            {
                Expression expr = _engine.Parent.RewriteQuotedReactiveToSubscribable(subscription);
                _engine.CreateSubscription(subscriptionUri, expr, state);
            }

            public void DeleteSubscription(Uri subscriptionUri)
            {
                _engine.DeleteSubscription(subscriptionUri);
            }

            public void CreateStream(Uri streamUri, Expression stream, object state)
            {
                Expression expr = _engine.Parent.RewriteQuotedReactiveToSubscribable(stream);
                _engine.CreateStream(streamUri, expr, state);
            }

            public void DeleteStream(Uri streamUri)
            {
                _engine.DeleteStream(streamUri);
            }

            public IReactiveObserver<T> GetObserver<T>(Uri observerUri)
            {
                return new ReactiveObserverToReliableObserver<T>(_engine.GetObserver<T>(observerUri));
            }

            public void DefineObservable(Uri observableUri, Expression observable, object state)
            {
                Expression expr = _engine.Parent.RewriteQuotedReactiveToSubscribable(observable);
                _engine.DefineObservable(observableUri, expr, state);
            }

            public void UndefineObservable(Uri observableUri)
            {
                _engine.UndefineObservable(observableUri);
            }

            public void DefineObserver(Uri observerUri, Expression observer, object state)
            {
                Expression expr = _engine.Parent.RewriteQuotedReactiveToSubscribable(observer);
                _engine.DefineObserver(observerUri, expr, state);
            }

            public void UndefineObserver(Uri observerUri)
            {
                _engine.UndefineObserver(observerUri);
            }

            public void DefineStreamFactory(Uri streamFactoryUri, Expression streamFactory, object state)
            {
                Expression expr = _engine.Parent.RewriteQuotedReactiveToSubscribable(streamFactory);
                _engine.DefineSubjectFactory(streamFactoryUri, expr, state);
            }

            public void UndefineStreamFactory(Uri streamFactoryUri)
            {
                _engine.UndefineSubjectFactory(streamFactoryUri);
            }

            public void DefineSubscriptionFactory(Uri subscriptionFactoryUri, Expression subscriptionFactory, object state)
            {
                Expression expr = _engine.Parent.RewriteQuotedReactiveToSubscribable(subscriptionFactory);
                _engine.DefineSubscriptionFactory(subscriptionFactoryUri, expr, state);
            }

            public void UndefineSubscriptionFactory(Uri subscriptionFactoryUri)
            {
                _engine.UndefineSubscriptionFactory(subscriptionFactoryUri);
            }

            public IQueryProvider Provider => _engine.MetadataQueryProvider;

            private sealed class ReactiveObserverToReliableObserver<T> : IReactiveObserver<T>
            {
                private readonly IReliableObserver<T> _reliableObserver;

                public ReactiveObserverToReliableObserver(IReliableObserver<T> reliableObserver)
                {
                    _reliableObserver = reliableObserver;
                }

                public void OnCompleted() => _reliableObserver.OnCompleted();

                public void OnError(Exception error) => _reliableObserver.OnError(error);

                public void OnNext(T value) => _reliableObserver.OnNext(value, 0);
            }
        }
    }
}
