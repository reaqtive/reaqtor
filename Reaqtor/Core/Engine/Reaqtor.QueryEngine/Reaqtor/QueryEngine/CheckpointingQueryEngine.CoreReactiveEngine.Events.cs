// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

using Reaqtor.Metadata;
using Reaqtor.QueryEngine.Events;

namespace Reaqtor.QueryEngine
{
    public partial class CheckpointingQueryEngine
    {
        //
        // This file centralizes all the event raising logic for events exposed on the engine.
        //

        private partial class CoreReactiveEngine
        {
            private void OnEntityCreated(ReactiveEntityEventArgs e)
            {
                Parent.EntityCreated?.InvokeSafe(Parent, e);
            }

            private void OnEntityDeleted(ReactiveEntityEventArgs e)
            {
                Parent.EntityDeleted?.InvokeSafe(Parent, e);
            }

            private void OnEntityDefined(ReactiveEntityEventArgs e)
            {
                Parent.EntityDefined?.InvokeSafe(Parent, e);
            }

            private void OnEntityUndefined(ReactiveEntityEventArgs e)
            {
                Parent.EntityUndefined?.InvokeSafe(Parent, e);
            }

            private bool OnEntityLoadFailed(Uri uri, IReactiveResource entity, ReactiveEntityKind kind, Exception error, out ReactiveEntityRecoveryFailureMitigation mitigation)
            {
                mitigation = ReactiveEntityRecoveryFailureMitigation.Ignore;

                if (error is MitigationBailOutException)
                {
                    return true;
                }

                var entityLoadFailed = Parent.EntityLoadFailed;
                if (entityLoadFailed != null)
                {
                    var e = new ReactiveEntityLoadFailedEventArgs(uri, entity, kind, error);

                    entityLoadFailed.InvokeSafe(Parent, e);

                    mitigation = e.Mitigation;
                    return e.Handled;
                }

                return false;
            }

            private bool OnEntitySaveFailed(Uri uri, IReactiveResource entity, ReactiveEntityKind kind, Exception error)
            {
                var entitySaveFailed = Parent.EntitySaveFailed;
                if (entitySaveFailed != null)
                {
                    var e = new ReactiveEntitySaveFailedEventArgs(uri, entity, kind, error);

                    entitySaveFailed.InvokeSafe(Parent, e);

                    return e.Handled;
                }

                return false;
            }

            private bool OnEntityReplayFailed(Uri uri, Exception error)
            {
                var entityReplayFailed = Parent.EntityReplayFailed;
                if (entityReplayFailed != null)
                {
                    var e = new ReactiveEntityReplayFailedEventArgs(uri, error);

                    entityReplayFailed.InvokeSafe(Parent, e);

                    return e.Handled;
                }

                return false;
            }

            private void OnSchedulerPausing()
            {
                Parent.SchedulerPausing?.InvokeSafe(Parent, new SchedulerPausingEventArgs());
            }

            private void OnSchedulerPaused()
            {
                Parent.SchedulerPaused?.InvokeSafe(Parent, new SchedulerPausedEventArgs());
            }

            private void OnSchedulerContinuing()
            {
                Parent.SchedulerContinuing?.InvokeSafe(Parent, new SchedulerContinuingEventArgs());
            }

            private void OnSchedulerContinued()
            {
                Parent.SchedulerContinued?.InvokeSafe(Parent, new SchedulerContinuedEventArgs());
            }
        }
    }
}
