// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;

using Reaqtive;

using Reaqtor.Reliable;

namespace Reaqtor.QueryEngine
{
    /// <summary>
    /// Implementation of the query engine registry, with support for:
    /// 
    /// * chained lookups in parent registries (for use in binding);
    /// * snapshotting and truncation (for use in checkpointing);
    /// * access to reactive entity instances through <see cref="IExecutionEnvironment"/> (for use by operators).
    /// </summary>
    internal sealed class QueryEngineRegistry : ILoggedQueryEngineRegistry, IReliableExecutionEnvironment
    {
        private readonly IQueryEngineRegistry _parent;

        private readonly ReactiveEntityCollection<string, SubjectEntity> _localSubjects;
        private readonly ReactiveEntityCollection<string, ObservableDefinitionEntity> _localObservables;
        private readonly ReactiveEntityCollection<string, ObserverDefinitionEntity> _localObservers;
        private readonly ReactiveEntityCollection<string, StreamFactoryDefinitionEntity> _localSubjectFactories;
        private readonly ReactiveEntityCollection<string, SubscriptionFactoryDefinitionEntity> _localSubscriptionFactories;
        private readonly ReactiveEntityCollection<string, SubscriptionEntity> _localSubscriptions;
        private readonly ReactiveEntityCollection<string, DefinitionEntity> _localOther;
        private readonly ReactiveEntityCollection<string, ReliableSubscriptionEntity> _localReliableSubscriptions;
        private readonly ReactiveEntityCollection<string, DefinitionEntity> _localTemplates;

        private bool _disposed;

        public QueryEngineRegistry(IQueryEngineRegistry parent)
        {
            Debug.Assert(parent != null);

            _parent = parent;

            _localSubjects = new ReactiveEntityCollection<string, SubjectEntity>(StringComparer.Ordinal);
            Subjects = new ChainedLookupReactiveEntityCollection<string, SubjectEntity>(_localSubjects, _parent.Subjects);

            _localObservables = new ReactiveEntityCollection<string, ObservableDefinitionEntity>(StringComparer.Ordinal);
            var localObservablesAndSubjects = new ChainedLookupReactiveEntityCollection<string, ObservableDefinitionEntity, SubjectEntity>(_localObservables, _localSubjects, s => ObservableDefinitionEntity.FromSubject(s));
            Observables = new ChainedLookupReactiveEntityCollection<string, ObservableDefinitionEntity>(localObservablesAndSubjects, _parent.Observables);

            _localObservers = new ReactiveEntityCollection<string, ObserverDefinitionEntity>(StringComparer.Ordinal);
            var localObserversAndSubjects = new ChainedLookupReactiveEntityCollection<string, ObserverDefinitionEntity, SubjectEntity>(_localObservers, _localSubjects, s => ObserverDefinitionEntity.FromSubject(s));
            Observers = new ChainedLookupReactiveEntityCollection<string, ObserverDefinitionEntity>(localObserversAndSubjects, _parent.Observers);

            _localSubjectFactories = new ReactiveEntityCollection<string, StreamFactoryDefinitionEntity>(StringComparer.Ordinal);
            SubjectFactories = new ChainedLookupReactiveEntityCollection<string, StreamFactoryDefinitionEntity>(_localSubjectFactories, _parent.SubjectFactories);

            _localSubscriptionFactories = new ReactiveEntityCollection<string, SubscriptionFactoryDefinitionEntity>(StringComparer.Ordinal);
            SubscriptionFactories = new ChainedLookupReactiveEntityCollection<string, SubscriptionFactoryDefinitionEntity>(_localSubscriptionFactories, _parent.SubscriptionFactories);

            _localSubscriptions = new ReactiveEntityCollection<string, SubscriptionEntity>(StringComparer.Ordinal);
            Subscriptions = new ChainedLookupReactiveEntityCollection<string, SubscriptionEntity>(_localSubscriptions, _parent.Subscriptions);

            _localOther = new ReactiveEntityCollection<string, DefinitionEntity>(StringComparer.Ordinal);
            Other = new ChainedLookupReactiveEntityCollection<string, DefinitionEntity>(_localOther, _parent.Other);

            _localReliableSubscriptions = new ReactiveEntityCollection<string, ReliableSubscriptionEntity>(StringComparer.Ordinal);
            ReliableSubscriptions = new ChainedLookupReactiveEntityCollection<string, ReliableSubscriptionEntity>(_localReliableSubscriptions, _parent.ReliableSubscriptions);

            _localTemplates = new ReactiveEntityCollection<string, DefinitionEntity>(StringComparer.Ordinal);
            var chainedTemplates = new ChainedLookupReactiveEntityCollection<string, DefinitionEntity>(_localTemplates, _parent.Templates);
            Templates = new InvertedLookupReactiveEntityCollection<string, DefinitionEntity>(chainedTemplates, InvertedDefinitionEntityComparer.Default);
        }

        public IReactiveEntityCollection<string, ObservableDefinitionEntity> Observables { get; }

        public IReactiveEntityCollection<string, ObserverDefinitionEntity> Observers { get; }

        public IReactiveEntityCollection<string, StreamFactoryDefinitionEntity> SubjectFactories { get; }

        public IReactiveEntityCollection<string, SubscriptionFactoryDefinitionEntity> SubscriptionFactories { get; }

        public IReactiveEntityCollection<string, DefinitionEntity> Other { get; }

        public IReactiveEntityCollection<string, SubscriptionEntity> Subscriptions { get; }

        public IReactiveEntityCollection<string, SubjectEntity> Subjects { get; }

        public IReactiveEntityCollection<string, ReliableSubscriptionEntity> ReliableSubscriptions { get; }

        public IInvertedLookupReactiveEntityCollection<string, DefinitionEntity> Templates { get; }

        #region IReliableExecutionEnvironment

        public bool TryGetSubject<TInput, TOutput>(Uri uri, out IMultiSubject<TInput, TOutput> subject)
        {
            if (!Subjects.TryGetValue(uri.ToCanonicalString(), out SubjectEntity entity))
            {
                subject = null;
                return false;
            }

            switch (entity.Instance)
            {
                case IMultiSubject<TInput, TOutput> result:
                    subject = result;
                    return true;
                case IDynamicMultiSubject dyn:
                    subject = dyn.ToTyped<TInput, TOutput>();
                    return true;
                case IMultiSubject untyped:
                    subject = untyped.ToTyped<TInput, TOutput>();
                    return true;
            }

            throw InvalidInstance("Subject", uri, typeof(IMultiSubject<TInput, TOutput>), nameof(uri));
        }

        public IMultiSubject<TInput, TOutput> GetSubject<TInput, TOutput>(Uri uri)
        {
            if (!TryGetSubject(uri, out IMultiSubject<TInput, TOutput> result))
            {
                throw MissingArtifact("Subject", uri, nameof(uri));
            }

            return result;
        }

        public bool TryGetReliableSubject<TInput, TOutput>(Uri uri, out IReliableMultiSubject<TInput, TOutput> subject)
        {
            if (!Subjects.TryGetValue(uri.ToCanonicalString(), out SubjectEntity entity))
            {
                subject = null;
                return false;
            }

            if (entity.Instance is IReliableMultiSubject<TInput, TOutput> result)
            {
                subject = result;
                return true;
            }

            throw InvalidInstance("ReliableSubject", uri, typeof(IReliableMultiSubject<TInput, TOutput>), nameof(uri));
        }

        public IReliableMultiSubject<TInput, TOutput> GetReliableSubject<TInput, TOutput>(Uri uri)
        {
            if (!TryGetReliableSubject(uri, out IReliableMultiSubject<TInput, TOutput> result))
            {
                throw MissingArtifact("Subject", uri, nameof(uri));
            }

            return result;
        }

        public bool TryGetSubscription(Uri uri, out ISubscription subscription)
        {
            if (!Subscriptions.TryGetValue(uri.ToCanonicalString(), out SubscriptionEntity entity))
            {
                subscription = null;
                return false;
            }

            var result = entity.Instance;
            if (result != null)
            {
                subscription = result;
                return true;
            }

            throw InvalidInstance("Subscription", uri, typeof(ISubscription), nameof(uri));
        }

        public ISubscription GetSubscription(Uri uri)
        {
            if (!TryGetSubscription(uri, out ISubscription result))
            {
                throw MissingArtifact("Subscription", uri, nameof(uri));
            }

            return result;
        }

        private static Exception MissingArtifact(string artifactType, Uri uri, string paramName)
        {
            return new ArgumentException(string.Format(CultureInfo.InvariantCulture, "{0} with URI '{1}' does not exist in the registry.", artifactType, uri), paramName);
        }

        private static Exception InvalidInstance(string artifactType, Uri uri, Type type, string paramName)
        {
            return new ArgumentException(string.Format(CultureInfo.InvariantCulture, "{0} with URI '{1}' does not have a valid instance of type '{2}'.", artifactType, uri, type), paramName);
        }

        #endregion

        public IQueryEngineRegistrySnapshot TakeSnapshot() => new Snapshot(this);

        public void TruncateLoggedEntities(IQueryEngineRegistrySnapshot snapshot)
        {
            if (snapshot == null)
            {
                throw new ArgumentNullException(nameof(snapshot));
            }

            Observers.ClearRemovedKeys(snapshot.RemovedObservers);
            Observables.ClearRemovedKeys(snapshot.RemovedObservables);
            SubjectFactories.ClearRemovedKeys(snapshot.RemovedSubjectFactories);
            Subjects.ClearRemovedKeys(snapshot.RemovedSubjects);
            ReliableSubscriptions.ClearRemovedKeys(snapshot.RemovedReliableSubscriptions);
            SubscriptionFactories.ClearRemovedKeys(snapshot.RemovedSubscriptionFactories);
            Subscriptions.ClearRemovedKeys(snapshot.RemovedSubscriptions);
            Other.ClearRemovedKeys(snapshot.RemovedOther);
            Templates.ClearRemovedKeys(snapshot.RemovedTemplates);
        }

        private sealed class Snapshot : IQueryEngineRegistrySnapshot
        {
            private readonly ReadOnlyReactiveEntityCollection<string, SubscriptionEntity> _subscriptions;
            private readonly ReadOnlyReactiveEntityCollection<string, SubjectEntity> _subjects;
            private readonly ReadOnlyReactiveEntityCollection<string, ReliableSubscriptionEntity> _reliableSubscriptions;
            private readonly ReadOnlyReactiveEntityCollection<string, ObservableDefinitionEntity> _observables;
            private readonly ReadOnlyReactiveEntityCollection<string, ObserverDefinitionEntity> _observers;
            private readonly ReadOnlyReactiveEntityCollection<string, StreamFactoryDefinitionEntity> _subjectFactories;
            private readonly ReadOnlyReactiveEntityCollection<string, SubscriptionFactoryDefinitionEntity> _subscriptionFactories;
            private readonly ReadOnlyReactiveEntityCollection<string, DefinitionEntity> _other;
            private readonly ReadOnlyReactiveEntityCollection<string, DefinitionEntity> _templates;

            public Snapshot(QueryEngineRegistry registry)
            {
                Debug.Assert(registry != null, "Registry should not be null.");

                _subscriptions = registry._localSubscriptions.Clone();
                _subjects = registry._localSubjects.Clone();
                _reliableSubscriptions = registry._localReliableSubscriptions.Clone();
                _observables = registry._localObservables.Clone();
                _observers = registry._localObservers.Clone();
                _subjectFactories = registry._localSubjectFactories.Clone();
                _subscriptionFactories = registry._localSubscriptionFactories.Clone();
                _other = registry._localOther.Clone();
                _templates = registry._localTemplates.Clone();
            }

            public IReadOnlyDictionary<string, SubscriptionEntity> Subscriptions => _subscriptions.Entries;

            public IReadOnlyList<string> RemovedSubscriptions => _subscriptions.RemovedKeys;

            public IReadOnlyDictionary<string, SubjectEntity> Subjects => _subjects.Entries;

            public IReadOnlyList<string> RemovedSubjects => _subjects.RemovedKeys;

            public IReadOnlyDictionary<string, ReliableSubscriptionEntity> ReliableSubscriptions => _reliableSubscriptions.Entries;

            public IReadOnlyList<string> RemovedReliableSubscriptions => _reliableSubscriptions.RemovedKeys;

            public IReadOnlyDictionary<string, ObservableDefinitionEntity> Observables => _observables.Entries;

            public IReadOnlyList<string> RemovedObservables => _observables.RemovedKeys;

            public IReadOnlyDictionary<string, ObserverDefinitionEntity> Observers => _observers.Entries;

            public IReadOnlyList<string> RemovedObservers => _observers.RemovedKeys;

            public IReadOnlyDictionary<string, StreamFactoryDefinitionEntity> SubjectFactories => _subjectFactories.Entries;

            public IReadOnlyList<string> RemovedSubjectFactories => _subjectFactories.RemovedKeys;

            public IReadOnlyDictionary<string, SubscriptionFactoryDefinitionEntity> SubscriptionFactories => _subscriptionFactories.Entries;

            public IReadOnlyList<string> RemovedSubscriptionFactories => _subscriptionFactories.RemovedKeys;

            public IReadOnlyDictionary<string, DefinitionEntity> Other => _other.Entries;

            public IReadOnlyList<string> RemovedOther => _other.RemovedKeys;

            public IReadOnlyDictionary<string, DefinitionEntity> Templates => _templates.Entries;

            public IReadOnlyList<string> RemovedTemplates => _templates.RemovedKeys;

            public IEnumerable<ReactiveEntity> Entities
            {
                get
                {
                    return Concat<ReactiveEntity>(
                        _observers.Entries.Values,
                        _observables.Entries.Values,
                        _subjectFactories.Entries.Values,
                        _subjects.Entries.Values,
                        _reliableSubscriptions.Entries.Values,
                        _subscriptionFactories.Entries.Values,
                        _subscriptions.Entries.Values,
                        _other.Entries.Values,
                        _templates.Entries.Values
                    );

                    static IEnumerable<T> Concat<T>(params IEnumerable<T>[] sources)
                    {
                        foreach (var source in sources)
                        {
                            foreach (var element in source)
                            {
                                yield return element;
                            }
                        }
                    }
                }
            }
        }

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
            if (!_disposed)
            {
                _localSubjects.Dispose();
                _localObservables.Dispose();
                _localObservers.Dispose();
                _localSubjectFactories.Dispose();
                _localSubscriptionFactories.Dispose();
                _localSubscriptions.Dispose();
                _localOther.Dispose();
                _localReliableSubscriptions.Dispose();
                _localTemplates.Dispose();

                _disposed = true;
            }
        }
    }
}
