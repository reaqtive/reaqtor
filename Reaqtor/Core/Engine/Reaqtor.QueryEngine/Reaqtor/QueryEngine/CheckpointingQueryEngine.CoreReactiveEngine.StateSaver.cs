// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ControlFlow;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using Reaqtive;

using Reaqtor.Metadata;
using Reaqtor.QueryEngine.Metrics;

namespace Reaqtor.QueryEngine
{
    public partial class CheckpointingQueryEngine
    {
        private partial class CoreReactiveEngine
        {
            /// <summary>
            /// Base class for state savers to save artifacts from an engine to a <see cref="IStateWriter"/>.
            /// </summary>
            private abstract class StateSaver
            {
                private readonly IStateWriter _writer;
                private readonly CoreReactiveEngine _engine;
                private readonly QueryEngineRegistry _registry;
                private readonly IQueryEngineRegistrySnapshot _snapshot;

                /// <summary>
                /// Initializes a new instance of the <see cref="StateSaver" /> class.
                /// </summary>
                /// <param name="writer">The writer.</param>
                /// <param name="engine">The engine.</param>
                protected StateSaver(IStateWriter writer, CoreReactiveEngine engine)
                {
                    Debug.Assert(writer != null, "Reader should not be null.");
                    Debug.Assert(engine != null, "Engine should not be null.");

                    _writer = writer;
                    _engine = engine;
                    _registry = engine.Parent._registry;
                    _snapshot = _registry.TakeSnapshot();
                }

                /// <summary>
                /// Saves the state.
                /// </summary>
                public void Save(CancellationToken token)
                {
                    Debug.Assert(_registry != null, "Registry should not be null.");
                    Debug.Assert(_writer != null, "Writer should not be null.");

                    ClearRemovedEntities(token);
                    SaveExistingEntities(token);
                }

                private void ClearRemovedEntities(CancellationToken token)
                {
                    token.ThrowIfCancellationRequested();

                    var oth = _snapshot.RemovedOther;
                    var tmp = _snapshot.RemovedTemplates;
                    var obv = _snapshot.RemovedObservers;
                    var obs = _snapshot.RemovedObservables;
                    var suf = _snapshot.RemovedSubjectFactories;
                    var sbf = _snapshot.RemovedSubscriptionFactories;
                    var rsb = _snapshot.RemovedReliableSubscriptions;
                    var sbj = _snapshot.RemovedSubjects;
                    var sub = _snapshot.RemovedSubscriptions;

                    DeleteItems(oth, Category.Other, token);
                    DeleteItems(tmp, Category.Templates, token);
                    DeleteItems(obv, Category.Observers, token);
                    DeleteItems(obs, Category.Observables, token);
                    DeleteItems(suf, Category.SubjectFactories, token);
                    DeleteItems(sbf, Category.SubscriptionFactories, token);
                    DeleteItems(rsb, Category.ReliableSubscriptions, token);
                    DeleteItems(sbj, Category.Subjects, token);
                    DeleteItems(sbj, Category.SubjectsRuntimeState, token);
                    DeleteItems(sub, Category.Subscriptions, token);
                    DeleteItems(sub, Category.SubscriptionsRuntimeState, token);
                }

                private void SaveExistingEntities(CancellationToken token)
                {
                    token.ThrowIfCancellationRequested();

                    var trace = _engine.Parent.TraceSource;

                    var unhandled = new ConcurrentBag<Exception>();

                    using (var unhandledErrorSource = new CancellationTokenSource())
                    using (token.Register(unhandledErrorSource.Cancel))
                    {
                        var onError = new Func<ReactiveEntityKind, Action<string, IReactiveResource, Exception>>(kind => (key, entity, ex) =>
                        {
                            HandleError(kind, key, entity, ex, e =>
                            {
                                unhandled.Add(e);
                                unhandledErrorSource.Cancel();
                            });
                        });

                        try
                        {
                            //
                            //  CONSIDER: The current implementation is sequential. Consider having concurrent tasks for the entities.
                            //

                            SaveDefinitions(Category.Other, _snapshot.Other, ReactiveEntitySaverFactory<DefinitionEntity>(trace, TraceNoun.Other | TraceNoun.Definition, SaveDefinition), onError(ReactiveEntityKind.Other), unhandledErrorSource.Token);
                            SaveDefinitions(Category.Templates, _snapshot.Templates, ReactiveEntitySaverFactory<DefinitionEntity>(trace, TraceNoun.Template | TraceNoun.Definition, SaveDefinition), onError(ReactiveEntityKind.Template), unhandledErrorSource.Token);
                            SaveDefinitions(Category.Observers, _snapshot.Observers, ReactiveEntitySaverFactory<ObserverDefinitionEntity>(trace, TraceNoun.Observer | TraceNoun.Definition, SaveDefinition), onError(ReactiveEntityKind.Observer), unhandledErrorSource.Token);
                            SaveDefinitions(Category.Observables, _snapshot.Observables, ReactiveEntitySaverFactory<ObservableDefinitionEntity>(trace, TraceNoun.Observable | TraceNoun.Definition, SaveDefinition), onError(ReactiveEntityKind.Observable), unhandledErrorSource.Token);
                            SaveDefinitions(Category.SubjectFactories, _snapshot.SubjectFactories, ReactiveEntitySaverFactory<DefinitionEntity>(trace, TraceNoun.SubjectFactory | TraceNoun.Definition, SaveDefinition), onError(ReactiveEntityKind.StreamFactory), unhandledErrorSource.Token);
                            SaveDefinitions(Category.SubscriptionFactories, _snapshot.SubscriptionFactories, ReactiveEntitySaverFactory<DefinitionEntity>(trace, TraceNoun.SubscriptionFactory | TraceNoun.Definition, SaveDefinition), onError(ReactiveEntityKind.SubscriptionFactory), unhandledErrorSource.Token);
                            SaveDefinitions(Category.ReliableSubscriptions, _snapshot.ReliableSubscriptions, ReactiveEntitySaverFactory<ReliableSubscriptionEntity>(trace, TraceNoun.ReliableSubscription | TraceNoun.Definition, SaveDefinition), onError(ReactiveEntityKind.ReliableSubscription), unhandledErrorSource.Token);
                            SaveDefinitions(Category.Subjects, _snapshot.Subjects, ReactiveEntitySaverFactory<SubjectEntity>(trace, TraceNoun.Subject | TraceNoun.Definition, SaveDefinition), onError(ReactiveEntityKind.Stream), unhandledErrorSource.Token);
                            SaveDefinitions(Category.Subscriptions, _snapshot.Subscriptions, ReactiveEntitySaverFactory<SubscriptionEntity>(trace, TraceNoun.Subscription | TraceNoun.Definition, SaveDefinition), onError(ReactiveEntityKind.Subscription), unhandledErrorSource.Token);
                            SaveRuntimeState<SubjectEntity, IDisposable>(Category.SubjectsRuntimeState, _snapshot.Subjects, ShouldSaveSubjectState, ReactiveEntitySaverFactory<SubjectEntity>(trace, TraceNoun.Subject | TraceNoun.State, SaveSubjectState), onError(ReactiveEntityKind.Stream), unhandledErrorSource.Token);
                            SaveRuntimeState<SubscriptionEntity, ISubscription>(Category.SubscriptionsRuntimeState, _snapshot.Subscriptions, ShouldSaveSubscriptionState, ReactiveEntitySaverFactory<SubscriptionEntity>(trace, TraceNoun.Subscription | TraceNoun.State, SaveSubscriptionState), onError(ReactiveEntityKind.Subscription), unhandledErrorSource.Token);
                        }
                        catch (OperationCanceledException ex)
                        {
                            if (ex.CancellationToken != unhandledErrorSource.Token)
                            {
                                throw;
                            }

                            token.ThrowIfCancellationRequested();
                        }
                    }

                    if (!unhandled.IsEmpty)
                    {
                        throw new AggregateException(unhandled);
                    }
                }

                private static Action<TEntity, Stream> ReactiveEntitySaverFactory<TEntity>(TraceSource trace, TraceNoun noun, Action<TEntity, Stream> saveFunc)
                    where TEntity : ReactiveEntity
                {
                    return (entity, stream) =>
                    {
                        StateOperationTracer.Trace(trace, TraceVerb.Save, noun, entity.Uri, () =>
                        {
                            saveFunc(entity, stream);
                        });
                    };
                }

                private void DeleteItems(IReadOnlyList<string> uris, string category, CancellationToken token)
                {
                    var trace = _engine.Parent.TraceSource;

                    trace.Checkpoint_DeleteStarted(_engine.Parent.Uri, category);

                    uris.ForEach((uri, ct) => _writer.DeleteItem(category, uri), token);

                    var total = uris.Count;
                    trace.Checkpoint_DeleteCompleted(_engine.Parent.Uri, category, total);
                }

                private void HandleError(ReactiveEntityKind kind, string key, IReactiveResource entity, Exception ex, Action<Exception> unhandled)
                {
                    if (ex is EngineUnloadedException)
                        return;

                    var uri = new Uri(key);

                    var error = new EntitySaveFailedException(uri, kind, ex);

                    if (!_engine.OnEntitySaveFailed(uri, entity, kind, error))
                    {
                        unhandled(error);
                    }
                }

                /// <summary>
                /// Called when the state was persisted.
                /// </summary>
                public void OnSaved()
                {
                    _registry.TruncateLoggedEntities(_snapshot);

                    foreach (var entity in _snapshot.Entities)
                    {
                        entity.OnPersisted();
                    }

                    foreach (var subject in _snapshot.Subjects.Values)
                    {
                        if (subject.Instance is IStatefulOperator op)
                        {
                            op.OnStateSaved();
                        }
                    }

                    foreach (var subscription in _snapshot.Subscriptions.Values)
                    {
                        var instance = subscription.Instance;
                        if (instance != null)
                        {
                            SubscriptionStateVisitor.OnStateSaved(instance);
                        }
                    }
                }

                /// <summary>
                /// Saves reactive definitions.
                /// </summary>
                /// <typeparam name="TEntity">The type of the entity.</typeparam>
                /// <param name="category">The category.</param>
                /// <param name="entities">The entities.</param>
                /// <param name="save">Function to save the entity to a stream.</param>
                /// <param name="onError">Function to report an error.</param>
                /// <param name="token">Cancellation token.</param>
                private void SaveDefinitions<TEntity>(
                    string category,
                    IReadOnlyDictionary<string, TEntity> entities,
                    Action<TEntity, Stream> save,
                    Action<string, TEntity, Exception> onError,
                    CancellationToken token)
                    where TEntity : ReactiveEntity
                {
                    Debug.Assert(entities != null, "Entities should not be null.");
                    Debug.Assert(!string.IsNullOrEmpty(category), "Category should not be null or empty.");

                    var trace = _engine.Parent.TraceSource;

                    trace.Checkpoint_SavingDefinitionsStarted(_engine.Parent.Uri, category);

                    var total = 0;
                    var skipped = 0;
                    var failed = 0;

                    Parallel.ForEach(
                        entities.Values,
                        new ParallelOptions
                        {
                            MaxDegreeOfParallelism = _engine.Parent.Options.CheckpointDegreeOfParallelism,
                            CancellationToken = token,
                        },
                        entity =>
                        {
                            var key = entity.Uri.ToCanonicalString();

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1031 // Do not catch general exception types. (Exception handled by mitigation.)

                            try
                            {
                                if (entity.IsInitialized && ShouldSaveDefinition(entity))
                                {
                                    using var stream = _writer.GetItemWriter(category, key);

                                    save(entity, stream);
                                }
                                else
                                {
                                    Interlocked.Increment(ref skipped);
                                }
                            }
                            catch (Exception ex)
                            {
                                Interlocked.Increment(ref failed);

                                trace.Checkpoint_SavingDefinitionsFailure(_engine.Parent.Uri, category, key, ex.Message);

                                onError(key, entity, ex);
                            }

#pragma warning restore CA1031
#pragma warning restore IDE0079

                            Interlocked.Increment(ref total);
                        });

                    trace.Checkpoint_SavingDefinitionsCompleted(_engine.Parent.Uri, category, total, skipped, failed);
                }

                /// <summary>
                /// Saves runtime state of the entity.
                /// </summary>
                /// <typeparam name="TEntity">The type of the entity.</typeparam>
                /// <typeparam name="TInstance">Type of the stateful entity's runtime artifact.</typeparam>
                /// <param name="category">The category.</param>
                /// <param name="entities">The entities.</param>
                /// <param name="shouldSaveRuntimeState">Predicate identifying whether the state should be saved.</param>
                /// <param name="saveStateAction">The save state action.</param>
                /// <param name="onError">Function to report an error.</param>
                /// <param name="token">Cancellation token.</param>
                private void SaveRuntimeState<TEntity, TInstance>(
                    string category,
                    IReadOnlyDictionary<string, TEntity> entities,
                    Func<TEntity, bool> shouldSaveRuntimeState,
                    Action<TEntity, Stream> saveStateAction,
                    Action<string, TEntity, Exception> onError,
                    CancellationToken token)
                    where TEntity : RuntimeEntity<TInstance>
                    where TInstance : IDisposable
                {
                    Debug.Assert(entities != null, "Entities should not be null.");
                    Debug.Assert(!string.IsNullOrEmpty(category), "Category should not be null or empty.");
                    Debug.Assert(shouldSaveRuntimeState != null, "shouldSaveRuntimeState is not allowed to be null.");
                    Debug.Assert(saveStateAction != null, "saveStateAction is not allowed to be null.");

                    var trace = _engine.Parent.TraceSource;

                    trace.Checkpoint_SavingStateStarted(_engine.Parent.Uri, category);

                    var total = 0;
                    var skipped = 0;
                    var failed = 0;

                    Parallel.ForEach(
                        entities.Values,
                        new ParallelOptions
                        {
                            MaxDegreeOfParallelism = _engine.Parent.Options.CheckpointDegreeOfParallelism,
                            CancellationToken = token,
                        },
                        entity =>
                        {
                            var key = entity.Uri.ToCanonicalString();

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1031 // Do not catch general exception types. (Exception handled by mitigation.)

                            try
                            {
                                // Note: We may not have stored the definition but the entity may have gotten initialized in
                                // the meantime. This is fine because loading will first get the definition and only attempt
                                // to load state if the definition was restored properly.
                                if (entity.IsInitialized && shouldSaveRuntimeState(entity))
                                {
                                    using Stream stream = _writer.GetItemWriter(category, key);

                                    saveStateAction(entity, stream);
                                }
                                else
                                {
                                    Interlocked.Increment(ref skipped);
                                }
                            }
                            catch (Exception ex)
                            {
                                Interlocked.Increment(ref failed);

                                trace.Checkpoint_SavingStateFailure(_engine.Parent.Uri, category, key, ex.Message);

                                onError(key, entity, ex);
                            }

#pragma warning restore CA1031
#pragma warning restore IDE0079

                            Interlocked.Increment(ref total);
                        });

                    token.ThrowIfCancellationRequested();

                    trace.Checkpoint_SavingStateCompleted(_engine.Parent.Uri, category, total, skipped, failed);
                }

                /// <summary>
                /// Predicate defining whether the definition should be included in the checkpoint.
                /// </summary>
                /// <param name="entity">The entity.</param>
                /// <returns>True if should be included.</returns>
                protected abstract bool ShouldSaveDefinition(ReactiveEntity entity);

                /// <summary>
                /// Predicate defining whether the subject state should be included in the checkpoint.
                /// </summary>
                /// <param name="entity">The entity.</param>
                /// <returns>True if should be included.</returns>
                protected abstract bool ShouldSaveSubjectState(SubjectEntity entity);

                /// <summary>
                /// Predicate defining whether the subscription state should be included in the checkpoint.
                /// </summary>
                /// <param name="entity">The entity.</param>
                /// <returns>True if should be included.</returns>
                protected abstract bool ShouldSaveSubscriptionState(SubscriptionEntity entity);

                /// <summary>
                /// Saves subscription runtime state.
                /// </summary>
                /// <param name="entity">The entity to save.</param>
                /// <param name="stream">The stream to save to.</param>
                protected virtual void SaveSubscriptionState(SubscriptionEntity entity, Stream stream)
                {
                    Debug.Assert(entity != null, "Entity should not be null.");
                    Debug.Assert(stream != null, "Stream should not be null.");

                    var instance = entity.Instance;
                    if (instance != null)
                    {
                        var policy = _engine.Parent._serializationPolicy;

                        using (entity.Measure(EntityMetric.SaveState))
                        {
                            using var operatorStateWriter = new OperatorStateWriterFactory(stream, policy);

                            operatorStateWriter.WriteHeader();

                            SubscriptionStateVisitor.SaveState(instance, operatorStateWriter);
                        }
                    }
                }

                /// <summary>
                /// Saves subject runtime state.
                /// </summary>
                /// <param name="entity">The entity to save.</param>
                /// <param name="stream">The stream to save to.</param>
                protected virtual void SaveSubjectState(SubjectEntity entity, Stream stream)
                {
                    Debug.Assert(entity != null, "Entity should not be null.");
                    Debug.Assert(stream != null, "Stream should not be null.");

                    var policy = _engine.Parent._serializationPolicy;

                    using (entity.Measure(EntityMetric.SaveState))
                    {
                        using var operatorStateWriter = new OperatorStateWriterFactory(stream, policy);

                        operatorStateWriter.WriteHeader();

                        if (entity.Instance is IStatefulOperator op)
                        {
                            operatorStateWriter.SaveState(op);
                        }
                    }
                }

                /// <summary>
                /// Saves a definition.
                /// </summary>
                /// <param name="entity">The entity to save.</param>
                /// <param name="stream">The stream to save to.</param>
                protected virtual void SaveDefinition(ReactiveEntity entity, Stream stream)
                {
                    Debug.Assert(entity != null, "Entity should not be null.");
                    Debug.Assert(stream != null, "Stream should not be null.");

                    var policy = _engine.Parent._serializationPolicy;

                    using (entity.Measure(EntityMetric.SaveEntity))
                    {
                        using var definitionWriter = new EntityWriter(stream, policy);

                        definitionWriter.WriteHeader();
                        definitionWriter.Save(entity);
                    }
                }
            }

            /// <summary>
            /// Differential checkpoint.
            /// </summary>
            private sealed class DifferentialStateSaver : StateSaver
            {
                /// <summary>
                /// Initializes a new instance of the <see cref="DifferentialStateSaver"/> class.
                /// </summary>
                /// <param name="writer">The writer.</param>
                /// <param name="engine">The engine.</param>
                public DifferentialStateSaver(IStateWriter writer, CoreReactiveEngine engine)
                    : base(writer, engine)
                {
                }

                /// <summary>
                /// Predicate defining whether the definition should be included in the checkpoint.
                /// </summary>
                /// <param name="entity">The entity.</param>
                /// <returns>
                /// True if should be included.
                /// </returns>
                protected override bool ShouldSaveDefinition(ReactiveEntity entity)
                {
                    Debug.Assert(entity != null, "Entity should not be null.");
                    return !entity.IsPersisted;
                }

                /// <summary>
                /// Predicate defining whether the subject state should be included in the checkpoint.
                /// </summary>
                /// <param name="entity">The entity.</param>
                /// <returns>
                /// True if should be included.
                /// </returns>
                protected override bool ShouldSaveSubjectState(SubjectEntity entity)
                {
                    if (entity.Instance is IStatefulOperator op)
                    {
                        return op.StateChanged;
                    }

                    return false;
                }

                /// <summary>
                /// Predicate defining whether the subscription state should be included in the checkpoint.
                /// </summary>
                /// <param name="entity">The entity.</param>
                /// <returns>
                /// True if should be included.
                /// </returns>
                protected override bool ShouldSaveSubscriptionState(SubscriptionEntity entity)
                {
                    Debug.Assert(entity != null, "Entity should not be null.");

                    var instance = entity.Instance;
                    if (instance != null)
                    {
                        return SubscriptionStateVisitor.HasStateChanged(instance);
                    }

                    return false;
                }
            }

            /// <summary>
            /// Full checkpoint.
            /// </summary>
            private sealed class FullStateSaver : StateSaver
            {
                /// <summary>
                /// Initializes a new instance of the <see cref="FullStateSaver"/> class.
                /// </summary>
                /// <param name="writer">The writer.</param>
                /// <param name="engine">The engine.</param>
                public FullStateSaver(IStateWriter writer, CoreReactiveEngine engine)
                    : base(writer, engine)
                {
                }

                /// <summary>
                /// Predicate defining whether the definition should be included in the checkpoint.
                /// </summary>
                /// <param name="entity">The entity.</param>
                /// <returns>
                /// True if should be included.
                /// </returns>
                protected override bool ShouldSaveDefinition(ReactiveEntity entity) => true;

                /// <summary>
                /// Predicate defining whether the subject state should be included in the checkpoint.
                /// </summary>
                /// <param name="entity">The entity.</param>
                /// <returns>
                /// True if should be included.
                /// </returns>
                protected override bool ShouldSaveSubjectState(SubjectEntity entity) => true;

                /// <summary>
                /// Predicate defining whether the subscription state should be included in the checkpoint.
                /// </summary>
                /// <param name="entity">The entity.</param>
                /// <returns>
                /// True if should be included.
                /// </returns>
                protected override bool ShouldSaveSubscriptionState(SubscriptionEntity entity) => true;
            }
        }
    }
}
