// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

using Reaqtive.Scheduler;

using Reaqtor.Expressions;

namespace Reaqtor.QueryEngine
{
    public partial class CheckpointingQueryEngine
    {
        private partial class CoreReactiveEngine
        {
            /// <summary>
            /// Scheduler task that can be made to run in the background to carry out templatization of artifacts in the registry.
            /// </summary>
            /// <remarks>
            /// Templates have been introduced to compact stores for query engines that have a lot of expressions (typically subscriptions)
            /// that are similar modulo constants. For example:
            /// 
            /// <c>xs.Where(x => x > a).Subscribe(o("foo"))</c>
            /// <c>xs.Where(x => x > b).Subscribe(o("bar"))</c>
            /// 
            /// where `a`, `b`, `"foo"`, and `"bar"` are constants that differ across the two. A template hoists out these constants and
            /// saves the common piece only once in a special table for templates:
            /// 
            /// <c>t1 = xs.Where(x => x > @p0).Subscribe(o(@p2))</c>
            /// 
            /// The original artifacts are then rewritten to be instantiations of the template:
            /// 
            /// <c>t1(a, "foo")</c>
            /// <c>t1(b, "bar")</c>
            /// 
            /// This template migration task scans the registry looking for expressions that can be templatized. A regex filter is supported
            /// to match on the identifiers of artifacts, which is used in production to only templatize known common queries (though it can
            /// of course be configured as `.*` to templatize everything).
            /// 
            /// Templatization has been useful in resource constrained or high density environments where a lot of queries are really
            /// instantiations of a template on the client side. Later, we introduced subscription factories to allow users to define such
            /// templates directly as first-class citizens. However, that doesn't render templates useless, because templatization operates
            /// on artifacts in the registry which are often the result of inlining of definitions during binding steps (so duplication can
            /// still occur).
            /// 
            /// The limited templatization support has been shown to be effective in these environments (at the expense of extra CPU usage
            /// to carry out the post-facto templatization on existing stores). More sophisticated approaches have been prototyped, e.g.
            /// matching on subexpressions, but didn't move the needle all that much, so we have refrained from putting these in the product.
            /// 
            /// Finally note that templatization is often just carried out after an engine's recovery, subject to a certain quota, and
            /// running periodically (up to a certain time-based quantum threshold) until the quota is met. This is done to reduce the
            /// impact on event processing. However, this is merely a policy at the engine level, and can easily be overridden by setting
            /// the quota to int.MaxValue. Only the quantum is hardcoded to 10ms at this point.
            /// </remarks>
            private sealed class TemplateMigrationTask : ISchedulerTask
            {
                private static readonly TimeSpan _quantum = TimeSpan.FromMilliseconds(10);

                private readonly CheckpointingQueryEngine _queryEngine;
                private readonly QueryEngineRegistryTemplatizer _templatizer;
                private readonly int _quota;
                private readonly TraceSource _traceSource;
                private readonly IEnumerator<ReactiveEntity> _enumerator;

                private int _remainingQuota;

                public TemplateMigrationTask(CheckpointingQueryEngine queryEngine, QueryEngineRegistryTemplatizer templatizer, Regex keyRegex, int quota, TraceSource traceSource)
                {
                    Debug.Assert(quota > 0);

                    _queryEngine = queryEngine;
                    _templatizer = templatizer;
                    _quota = quota;
                    _remainingQuota = quota;
                    _traceSource = traceSource;
                    _enumerator = new RegistryKeyEnumerable(queryEngine._registry).Where(e => keyRegex.IsMatch(e.Uri.ToCanonicalString())).GetEnumerator();
                }

                public long Priority => 1;

                public bool IsRunnable => Volatile.Read(ref _remainingQuota) > 0;

                public bool Execute(IScheduler scheduler)
                {
                    if (TryGetCandidate(out ReactiveEntity entity, out bool complete))
                    {
                        var templatized = _templatizer.Templatize(entity.Expression);
                        if (templatized != entity.Expression)
                        {
                            entity.Update(templatized);
                            Interlocked.Decrement(ref _remainingQuota);

                            _traceSource.TemplateMigration_Execute(_queryEngine.Uri, entity.Uri);
                        }
                    }

                    return complete;
                }

                public void RecalculatePriority()
                {
                }

                public void ResetQuota()
                {
                    _traceSource.TemplateMigration_ResetQuota(_queryEngine.Uri, _quota, _quota - Volatile.Read(ref _remainingQuota));

                    Interlocked.Exchange(ref _remainingQuota, _quota);
                }

                private bool TryGetCandidate(out ReactiveEntity entity, out bool complete)
                {
                    var stopwatch = Stopwatch.StartNew();

                    while (stopwatch.Elapsed < _quantum)
                    {
                        if (!_enumerator.MoveNext())
                        {
                            _traceSource.TemplateMigration_Completed(_queryEngine.Uri);

                            _enumerator.Dispose();

                            entity = null;
                            complete = true;
                            return false;
                        }

                        if (!_enumerator.Current.Expression.IsTemplatized())
                        {
                            entity = _enumerator.Current;
                            complete = false;
                            return true;
                        }
                    }

                    entity = null;
                    complete = false;
                    return false;
                }

                private sealed class RegistryKeyEnumerable : IEnumerable<ReactiveEntity>
                {
                    private readonly IQueryEngineRegistry _registry;

                    public RegistryKeyEnumerable(IQueryEngineRegistry registry)
                    {
                        _registry = registry;
                    }

                    public IEnumerator<ReactiveEntity> GetEnumerator()
                    {
                        foreach (var observable in _registry.Observables.Values)
                        {
                            yield return observable;
                        }

                        foreach (var observer in _registry.Observers.Values)
                        {
                            yield return observer;
                        }

                        foreach (var streamFactory in _registry.SubjectFactories.Values)
                        {
                            yield return streamFactory;
                        }

                        foreach (var stream in _registry.Subjects.Values)
                        {
                            yield return stream;
                        }

                        foreach (var subscription in _registry.Subscriptions.Values)
                        {
                            yield return subscription;
                        }

                        foreach (var reliableSubscription in _registry.ReliableSubscriptions.Values)
                        {
                            yield return reliableSubscription;
                        }
                    }

                    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
                }
            }
        }
    }
}
