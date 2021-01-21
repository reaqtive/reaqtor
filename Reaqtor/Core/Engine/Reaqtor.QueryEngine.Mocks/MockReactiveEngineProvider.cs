// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA2000 // Dispose objects before losing scope. (Entities are owned by the registry.)

using System;
using System.Linq;
using System.Linq.Expressions;

using Reaqtive;

namespace Reaqtor.QueryEngine.Mocks
{
    internal class MockReactiveEngineProvider : IReactiveEngineProvider
    {
        private readonly MockQueryEngineRegistry _registry;
        private readonly QueryEngineBinder _binder;
        private readonly RegistryQueryProviderBase _queryProvider;

        public MockReactiveEngineProvider(MockQueryEngineRegistry registry)
            : this(registry, new RegistryQueryProvider(registry))
        {
        }

        public MockReactiveEngineProvider(MockQueryEngineRegistry registry, RegistryQueryProviderBase queryProvider)
        {
            _registry = registry;
            _binder = new FullBinder(registry);
            _queryProvider = queryProvider;
        }

        public void CreateSubscription(Uri subscriptionUri, Expression subscription, object state)
        {
            _registry.Subscriptions.Add(
                subscriptionUri.ToCanonicalString(),
                new SubscriptionEntity(
                    subscriptionUri,
                    subscription,
                    state,
                    Evaluate<ISubscription>(subscription)));
        }

        public void DeleteSubscription(Uri subscriptionUri)
        {
            _registry.Subscriptions.TryRemove(subscriptionUri.ToCanonicalString(), out _);
        }

        public void CreateStream(Uri streamUri, Expression stream, object state)
        {
            _registry.Subjects.Add(
                streamUri.ToCanonicalString(),
                new SubjectEntity(
                    streamUri,
                    stream,
                    state,
                    Evaluate<IDisposable>(stream)));
        }

        public void DeleteStream(Uri streamUri)
        {
            _registry.Subjects.TryRemove(streamUri.ToCanonicalString(), out _);
        }

        public IReactiveObserver<T> GetObserver<T>(Uri observerUri)
        {
            throw new NotImplementedException();
        }

        public void DefineObservable(Uri observableUri, Expression observable, object state)
        {
            _registry.Observables.Add(observableUri.ToCanonicalString(), new ObservableDefinitionEntity(observableUri, observable, state));
        }

        public void UndefineObservable(Uri observableUri)
        {
            _registry.Observables.TryRemove(observableUri.ToCanonicalString(), out _);
        }

        public void DefineObserver(Uri observerUri, Expression observer, object state)
        {
            _registry.Observers.Add(observerUri.ToCanonicalString(), new ObserverDefinitionEntity(observerUri, observer, state));
        }

        public void UndefineObserver(Uri observerUri)
        {
            _registry.Observers.TryRemove(observerUri.ToCanonicalString(), out _);
        }

        public void DefineStreamFactory(Uri streamFactoryUri, Expression streamFactory, object state)
        {
            _registry.SubjectFactories.Add(streamFactoryUri.ToCanonicalString(), new StreamFactoryDefinitionEntity(streamFactoryUri, streamFactory, state));
        }

        public void UndefineStreamFactory(Uri streamFactoryUri)
        {
            _registry.SubjectFactories.TryRemove(streamFactoryUri.ToCanonicalString(), out _);
        }

        public void DefineSubscriptionFactory(Uri subscriptionFactoryUri, Expression subscriptionFactory, object state)
        {
            _registry.SubscriptionFactories.Add(subscriptionFactoryUri.ToCanonicalString(), new SubscriptionFactoryDefinitionEntity(subscriptionFactoryUri, subscriptionFactory, state));
        }

        public void UndefineSubscriptionFactory(Uri subscriptionFactoryUri)
        {
            _registry.SubscriptionFactories.TryRemove(subscriptionFactoryUri.ToCanonicalString(), out _);
        }

        public IQueryProvider Provider => _queryProvider;

        private T Evaluate<T>(Expression expression)
        {
#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1031 // Do not catch general exception types.

            try
            {
                return _binder.Bind(expression).Evaluate<T>();
            }
            catch
            {
                return default; // REVIEW
            }

#pragma warning restore CA1031
#pragma warning restore IDE0079
        }
    }
}
