// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;

using Reaqtive;
using Reaqtive.Scheduler;

using Reaqtor.Expressions.Core;
using Reaqtor.QueryEngine;
using Reaqtor.Reactive;
using Reaqtor.Reliable;

namespace Reaqtor.Remoting.QueryEvaluator
{
    /// <summary>
    /// Query engine supplying host-aware contexts to subscriptions.
    /// </summary>
    internal class QueryEvaluatorQueryEngine : CheckpointingQueryEngine
    {
        private static readonly IQuotedTypeConversionTargets s_map = QuotedTypeConversionTargets.From(new Dictionary<Type, Type>
        {
            { typeof(ReactiveQbservable),            typeof(Subscribable)               },
            { typeof(ReactiveQbserver),              typeof(Observer)                   },
            { typeof(ReactiveQubjectFactory),        typeof(ReliableSubjectFactory)     },
        });

        private readonly IReadOnlyDictionary<string, object> _context;
        private readonly TraceSource _traceSource;

        /// <summary>
        /// Creates a new query engine that's aware of host services.
        /// </summary>
        /// <param name="uri">URI identifying the query engine.</param>
        /// <param name="serviceResolver">Resolver used by the query engine to
        /// resolve artifacts to reactive services for delegation of requests or
        /// other operations.</param>
        /// <param name="scheduler">Scheduler used by the query engine to
        /// schedule work, also allowing for pausing during checkpointing
        /// operations.</param>
        /// <param name="metadataRegistry">Metadata consulted by the query
        /// engine to lookup artifact definitions.</param>
        /// <param name="traceSource">Tracer to log to.</param>
        public QueryEvaluatorQueryEngine(
            Uri uri,
            IReactiveServiceResolver serviceResolver,
            IScheduler scheduler,
            IReactiveMetadata metadataRegistry,
            IKeyValueStore keyValueStore,
            TraceSource traceSource,
            IReadOnlyDictionary<string, object> context)
            : base(uri, serviceResolver, scheduler, metadataRegistry, keyValueStore, SerializationPolicy.Default, s_map, traceSource)
        {
            _context = context;
            _traceSource = traceSource;
        }

        /// <summary>
        /// Wraps operator contexts with information about the query evaluator
        /// host in order for specialized operators to call back into the
        /// hosting environment.
        /// </summary>
        /// <param name="instanceId">URI of the artifact instance the context is
        /// associated with.</param>
        /// <returns>Operator context that gets flown through the subscription
        /// artifact created by the query evaluator.</returns>
        protected override IHostedOperatorContext CreateOperatorContext(Uri instanceId)
        {
            var context = base.CreateOperatorContext(instanceId);

            return new HostOperatorContext(context, _context, _traceSource);
        }
    }
}
