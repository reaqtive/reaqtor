// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

namespace Reaqtor.QueryEngine.Mocks
{
    internal class MockQueryEngineRegistry : ILoggedQueryEngineRegistry
    {
        private readonly ReactiveEntityCollection<string, ObservableDefinitionEntity> _observables = new(StringComparer.Ordinal);
        private readonly ReactiveEntityCollection<string, ObserverDefinitionEntity> _observers = new(StringComparer.Ordinal);

        public MockQueryEngineRegistry()
        {
            Subjects = new ReactiveEntityCollection<string, SubjectEntity>(StringComparer.Ordinal);
            Observables = new ChainedLookupReactiveEntityCollection<string, ObservableDefinitionEntity, SubjectEntity>(_observables, Subjects, s => ObservableDefinitionEntity.FromSubject(s));
            Observers = new ChainedLookupReactiveEntityCollection<string, ObserverDefinitionEntity, SubjectEntity>(_observers, Subjects, s => ObserverDefinitionEntity.FromSubject(s));
            SubjectFactories = new ReactiveEntityCollection<string, StreamFactoryDefinitionEntity>(StringComparer.Ordinal);
            SubscriptionFactories = new ReactiveEntityCollection<string, SubscriptionFactoryDefinitionEntity>(StringComparer.Ordinal);
            Subscriptions = new ReactiveEntityCollection<string, SubscriptionEntity>(StringComparer.Ordinal);
            Other = new ReactiveEntityCollection<string, DefinitionEntity>(StringComparer.Ordinal);
            ReliableSubscriptions = new ReactiveEntityCollection<string, ReliableSubscriptionEntity>(StringComparer.Ordinal);
            Templates = new InvertedLookupReactiveEntityCollection<string, DefinitionEntity>(StringComparer.Ordinal, InvertedDefinitionEntityComparer.Default);
        }

        public IReactiveEntityCollection<string, ObservableDefinitionEntity> Observables { get; }

        public IReactiveEntityCollection<string, ObserverDefinitionEntity> Observers { get; }

        public IReactiveEntityCollection<string, StreamFactoryDefinitionEntity> SubjectFactories { get; }

        public IReactiveEntityCollection<string, SubscriptionFactoryDefinitionEntity> SubscriptionFactories { get; }

        public IReactiveEntityCollection<string, DefinitionEntity> Other { get; }

        public IInvertedLookupReactiveEntityCollection<string, DefinitionEntity> Templates { get; }

        public IReactiveEntityCollection<string, SubscriptionEntity> Subscriptions { get; }

        public IReactiveEntityCollection<string, SubjectEntity> Subjects { get; }

        public IReactiveEntityCollection<string, ReliableSubscriptionEntity> ReliableSubscriptions { get; }

        public void Clear()
        {
            Observables.Clear();
            Observers.Clear();
            Other.Clear();
            ReliableSubscriptions.Clear();
            SubjectFactories.Clear();
            Subjects.Clear();
            SubscriptionFactories.Clear();
            Subscriptions.Clear();
            Templates.Clear();
        }

        public void Dispose()
        {
            _observables.Dispose();
            _observers.Dispose();

            Observables.Dispose();
            Observers.Dispose();
            Other.Dispose();
            ReliableSubscriptions.Dispose();
            SubjectFactories.Dispose();
            Subjects.Dispose();
            SubscriptionFactories.Dispose();
            Subscriptions.Dispose();
            Templates.Dispose();
        }

        public IQueryEngineRegistrySnapshot TakeSnapshot()
        {
            throw new NotImplementedException();
        }

        public void TruncateLoggedEntities(IQueryEngineRegistrySnapshot snapshot)
        {
            throw new NotImplementedException();
        }
    }
}
