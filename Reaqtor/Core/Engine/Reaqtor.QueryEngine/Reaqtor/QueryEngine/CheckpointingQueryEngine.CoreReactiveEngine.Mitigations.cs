// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Globalization;

using Reaqtor.QueryEngine.Events;

namespace Reaqtor.QueryEngine
{
    public partial class CheckpointingQueryEngine
    {
        private partial class CoreReactiveEngine
        {
            /// <summary>
            /// Try to invoke the specified <paramref name="action"/> and handle any exception that occurs.
            /// If an exception occurs and <paramref name="shouldMitigate"/> is true, the specified <paramref name="mitigator"/> is used to apply a mitigation.
            /// </summary>
            /// <param name="action">The action to try.</param>
            /// <param name="entity">The entity being loaded/mitigated.</param>
            /// <param name="shouldMitigate">true if errors should be mitigated; otherwise, false.</param>
            /// <param name="mitigator">The mitigator to use if the failed action should be mitigated.</param>
            private void TryMitigate(Action action, ReactiveEntity entity, bool shouldMitigate, ReactiveEntityRecoveryFailureMitigator mitigator)
            {
                try
                {
                    action();
                }
                catch (Exception ex)
                {
                    var error = new EntityLoadFailedException(entity.Uri, entity.Kind, ex);

                    if (shouldMitigate && Parent._engine.OnEntityLoadFailed(entity.Uri, entity, entity.Kind, error, out ReactiveEntityRecoveryFailureMitigation mitigation))
                    {
                        var trace = Parent.TraceSource;

                        trace.Mitigation_Execute(entity.Uri, mitigation);

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1031 // Do not catch general exception types. (Protecting against engine unavailability; last resort mitigation with tracing of errors.)

                        try
                        {
                            if (!mitigator.DoMitigate(entity, mitigation))
                            {
                                trace.Mitigation_NotAccepted(mitigation, entity.Uri, ex);
                            }
                        }
                        catch (Exception mx)
                        {
                            trace.Mitigation_Failure(mitigation, entity.Uri, mx);
                        }

#pragma warning restore CA1031
#pragma warning restore IDE0079

                        // For this implementation, whenever a mitigation occurs, we will throw a 
                        // bail out exception. This is to prevent further processing of the 
                        // 'normal' recovery process from throwing additional errors that will
                        // trigger additional mitigations. An unfortunate consequence is that the
                        // mitigations must ensure all steps that would have occurred in the
                        // 'normal' recovery process occur in the mitigation (esp. for things
                        // mitigations like `Regenerate`. Right now, this is not a problem, as the
                        // steps being mitigated are late in the recovery process.
                        throw new MitigationBailOutException();
                    }

                    // This may result in a second call to the `EntityLoadFailed` event.
                    throw;
                }
            }

            /// <summary>
            /// Mitigation that removes an entity that failed to load.
            /// </summary>
            private class RemoveMitigator : ReactiveEntityRecoveryFailureMitigator
            {
                private readonly CoreReactiveEngine _parent;

                public RemoveMitigator(CoreReactiveEngine parent)
                {
                    _parent = parent;
                }

                protected override bool OnRemove(ReactiveEntity entity)
                {
                    if (_parent.Parent.Options.GarbageCollectionEnabled && _parent.Parent.Options.GarbageCollectionSweepEnabled)
                    {
                        _parent._garbageCollector.Enqueue(entity);
                    }
                    else
                    {
                        _parent.RemoveEntity(entity);
                    }

                    return true;
                }
            }

            /// <summary>
            /// Mitigation that supports to regenerate an entity that failed to load, e.g. if its state is corrupt.
            /// </summary>
            private sealed class FullMitigator : RemoveMitigator
            {
                private readonly Action<ReactiveEntity> _onRegenerate;

                public FullMitigator(Action<ReactiveEntity> onRegenerate, CoreReactiveEngine engine)
                    : base(engine)
                {
                    _onRegenerate = onRegenerate;
                }

                protected override bool OnRegenerate(ReactiveEntity entity)
                {
                    _onRegenerate(entity);
                    return true;
                }
            }

            /// <summary>
            /// Mitigation that replaces the entity that failed to load by a placeholder, causing subsequent metadata queries
            /// to indicate the presence of an entity, and avoiding creation operation to override the entity. The placeholder
            /// is volatile (not persisted upon checkpointing) making this mitigation useful for (manual) fixing of failed
            /// entities in the underlying store.
            /// </summary>
            private sealed class PlaceholderMitigator : RemoveMitigator
            {
                private readonly CoreReactiveEngine _parent;

                public PlaceholderMitigator(CoreReactiveEngine parent)
                    : base(parent)
                {
                    _parent = parent;
                }

                protected override void OnAll(ReactiveEntity entity)
                {
                    _parent.AddEntityPlaceholder(entity.Kind, entity.Uri.ToCanonicalString());
                }
            }

            private void AddEntityPlaceholder(ReactiveEntityKind kind, string key)
            {
                var uri = new Uri(key);

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA2000 // Dispose objects before losing scope. (Entities are owned by the registry.)

                var added = kind switch
                {
                    ReactiveEntityKind.Observable => Registry.Observables.TryAdd(key, ObservableDefinitionEntity.CreateInvalidInstance(uri)),
                    ReactiveEntityKind.Observer => Registry.Observers.TryAdd(key, ObserverDefinitionEntity.CreateInvalidInstance(uri)),
                    ReactiveEntityKind.Stream => Registry.Subjects.TryAdd(key, SubjectEntity.CreateInvalidInstance(uri)),
                    ReactiveEntityKind.StreamFactory => Registry.SubjectFactories.TryAdd(key, StreamFactoryDefinitionEntity.CreateInvalidInstance(uri)),
                    ReactiveEntityKind.SubscriptionFactory => Registry.SubscriptionFactories.TryAdd(key, SubscriptionFactoryDefinitionEntity.CreateInvalidInstance(uri)),
                    ReactiveEntityKind.Subscription => Registry.Subscriptions.TryAdd(key, SubscriptionEntity.CreateInvalidInstance(uri)),
                    ReactiveEntityKind.ReliableSubscription => Registry.ReliableSubscriptions.TryAdd(key, ReliableSubscriptionEntity.CreateInvalidInstance(uri)),
                    ReactiveEntityKind.Other => Registry.Other.TryAdd(key, OtherDefinitionEntity.CreateInvalidInstance(uri)),
                    ReactiveEntityKind.Template => Registry.Templates.TryAdd(key, OtherDefinitionEntity.CreateInvalidInstance(uri)),
                    _ => throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Cannot create an invalid entity '{0}' of type '{1}'.", key, kind)),
                };

#pragma warning restore CA2000
#pragma warning restore IDE0079

                if (added)
                {
                    Parent.TraceSource.Registry_AddEntityPlaceholder(Parent.Uri, kind.ToString(), key);
                }
            }

            private void RemoveEntity(ReactiveEntity entity)
            {
                var removed = true;
                var uri = entity.Uri;
                var key = uri.ToCanonicalString();

                switch (entity.Kind)
                {
                    case ReactiveEntityKind.Observable:
                        {
                            UndefineObservable(uri);
                            break;
                        }
                    case ReactiveEntityKind.Observer:
                        {
                            UndefineObserver(uri);
                            break;
                        }
                    case ReactiveEntityKind.Stream:
                        {
                            DeleteStream(uri);
                            break;
                        }
                    case ReactiveEntityKind.StreamFactory:
                        {
                            UndefineSubjectFactory(uri);
                            break;
                        }
                    case ReactiveEntityKind.SubscriptionFactory:
                        {
                            UndefineSubscriptionFactory(uri);
                            break;
                        }
                    case ReactiveEntityKind.Subscription:
                        {
                            DeleteSubscription(uri);
                            break;
                        }
                    case ReactiveEntityKind.ReliableSubscription:
                        {
                            DeleteReliableSubscription(uri);
                            break;
                        }
                    case ReactiveEntityKind.Other:
                        {
                            removed = Registry.Other.TryRemove(key, out _);
                            break;
                        }
                    case ReactiveEntityKind.Template:
                        {
                            removed = Registry.Templates.TryRemove(key, out _);
                            break;
                        }
                    case ReactiveEntityKind.None:
                    default:
                        throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Cannot remove a placeholder ignore '{0}' of type '{1}'.", key, entity.Kind));
                }

                if (removed)
                {
                    Parent.TraceSource.Registry_RemoveEntity(entity.Kind.ToString(), key, Parent.Uri);
                }
            }

            private void RemoveEntitySafe(ReactiveEntity entity)
            {
#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1031 // Do not catch general exception types. (Purpose of Safe variant.)

                try
                {
                    RemoveEntity(entity);
                }
                catch (Exception ex)
                {
                    Parent.TraceSource.FailSafe_Exception(nameof(RemoveEntity), ex);
                }

#pragma warning restore CA1031
#pragma warning restore IDE0079
            }
        }
    }
}
