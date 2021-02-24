// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;

using Reaqtive;

namespace Reaqtor.QueryEngine
{
    public partial class CheckpointingQueryEngine
    {
        private partial class CoreReactiveEngine
        {
            /// <summary>
            /// Garbage collector for orphaned artifacts (due to either lazy or partial cleanup, or due to incidents in production)
            /// which can be run in the background.
            /// </summary>
            /// <remarks>
            /// While the GC was introduced to clean up some mess in production, it has turned out to be a useful construct to move
            /// the deletion of artifacts to a centralized component. We haven't yet doubled down on the GC approach, but this provides
            /// a good basis to start from. The central idea is reachability analysis using <see cref="IDependencyOperator"/> with a
            /// mark and sweep approach.
            /// </remarks>
            private sealed class GarbageCollector
            {
                private readonly CheckpointingQueryEngine _queryEngine;
                private readonly Action<ReactiveEntity> _remove;
                private readonly ConcurrentQueue<ReactiveEntity> _collectibleEntities;

                private int _iterations;
                private int _totalSweeps;
                private TimeSpan _totalSweepTime;

                public GarbageCollector(CheckpointingQueryEngine queryEngine, Action<ReactiveEntity> remove)
                {
                    _queryEngine = queryEngine;
                    _remove = remove;
                    _collectibleEntities = new ConcurrentQueue<ReactiveEntity>();
                }

                public void Enqueue(ReactiveEntity entity)
                {
                    _collectibleEntities.Enqueue(entity);
                }

                public void Collect()
                {
                    var trace = _queryEngine.TraceSource;

                    var enabled = _queryEngine.Options.GarbageCollectionEnabled;
                    if (!enabled)
                    {
                        trace.RegistryGarbageCollection_NotEnabled(_queryEngine.Uri);
                        return;
                    }

                    trace.RegistryGarbageCollection_Started(_queryEngine.Uri);

                    var sw = Stopwatch.StartNew();

                    var allDependenciesList = new List<Uri>();

                    var visitor = SubscriptionVisitor.Do<IDependencyOperator>(op => { allDependenciesList.AddRange(op.Dependencies); });

                    foreach (var sub in _queryEngine._registry.Subscriptions)
                    {
                        var value = sub.Value;
                        if (value != null)
                        {
                            var instance = value.Instance;
                            if (instance != null && value.IsInitialized)
                            {
                                visitor.Apply(instance);
                            }
                        }
                    }

                    foreach (var sub in _queryEngine._registry.Subjects)
                    {
                        var value = sub.Value;
                        if (value != null)
                        {
                            var instance = value.Instance;
                            if (instance != null && value.IsInitialized)
                            {
                                if (instance is IDependencyOperator op)
                                {
                                    allDependenciesList.AddRange(op.Dependencies);
                                }
                            }
                        }
                    }

                    var allDependencies = new HashSet<Uri>(allDependenciesList);

                    var markTime = sw.Elapsed;

                    trace.RegistryGarbageCollection_LiveDependencies(_queryEngine.Uri, allDependencies.Count, (int)markTime.TotalMilliseconds);

                    sw.Restart();

                    var observables = _queryEngine._registry.Observables;
                    var unreachableObservables = 0;

                    foreach (var obs in observables)
                    {
                        var value = obs.Value;
                        if (value != null)
                        {
                            //
                            // NB: Right now, we take a narrow approach for known entities that cannot be top-level nodes (i.e.
                            //     they are always owned by a parent, and such a parent should be found for them to be alive).
                            //
                            //     A more complete implementation should have a mechanism to discover these, because they are
                            //     really dependent on the operator library used (with the exception of e.g. edges), and thus
                            //     this violates the layering map right now.
                            //

                            if (obs.Key.EndsWith("/upstream-observable", StringComparison.Ordinal))
                            {
                                if (!allDependencies.Contains(value.Uri))
                                {
                                    unreachableObservables++;
                                    _collectibleEntities.Enqueue(value);
                                }
                            }
                        }
                    }

                    //
                    // NB: Removed various internal-only identifiers from libraries other than Reaqtor for the OSS release.
                    //

                    var scanTime = sw.Elapsed;

                    trace.RegistryGarbageCollection_UnreachableObservables(_queryEngine.Uri, unreachableObservables, observables.Values.Count, (int)scanTime.TotalMilliseconds);

                    if (!_queryEngine.Options.GarbageCollectionSweepEnabled)
                    {
                        trace.RegistryGarbageCollection_SweepDisabled(_queryEngine.Uri);
                    }
                }

                public void Sweep()
                {
                    if (_collectibleEntities.IsEmpty)
                    {
                        return;
                    }

                    var budget = _queryEngine.Options.GarbageCollectionSweepBudgetPerCheckpoint;

                    var trace = _queryEngine.TraceSource;

                    trace.RegistryGarbageCollection_SweepStarted(_queryEngine.Uri, _collectibleEntities.Count);

                    var sw = Stopwatch.StartNew();

                    var i = 0;

                    while (i < budget && _collectibleEntities.TryDequeue(out ReactiveEntity entity))
                    {
                        _remove(entity);
                        i++;
                    }

                    var sweepTime = sw.Elapsed;

                    _iterations++;
                    _totalSweeps += i;
                    _totalSweepTime += sweepTime;

                    trace.RegistryGarbageCollection_SweepStopped(_queryEngine.Uri, i, _collectibleEntities.Count, (int)sweepTime.TotalMilliseconds);

                    if (_collectibleEntities.IsEmpty)
                    {
                        trace.RegistryGarbageCollection_SweepCompleted(
                            _queryEngine.Uri,
                            _totalSweeps,
                            _iterations,
                            (int)_totalSweepTime.TotalMilliseconds
                        );
                    }
                }
            }
        }
    }
}
