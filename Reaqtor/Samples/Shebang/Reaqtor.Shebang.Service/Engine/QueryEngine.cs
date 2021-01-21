// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

using Reaqtive.Scheduler;

using Reaqtor.QueryEngine;
using Reaqtor.Reactive;
using Reaqtor.Shebang.Client;

namespace Reaqtor.Shebang.Service
{
    //
    // Query engine implementation, specializing the Reaqtor query engine on a few facilities.
    //

    public sealed class SimplerCheckpointingQueryEngine : CheckpointingQueryEngine
    {
        private readonly IQueryEngineStateStore _store;
        private readonly IReadOnlyDictionary<string, object> _context;
        private readonly IReadOnlyDictionary<string, object> _environmentServices;

        public SimplerCheckpointingQueryEngine(Uri uri, IScheduler scheduler, IQueryEngineStateStore store, IReadOnlyDictionary<string, object> context = null, IIngressEgressManager ingressEgressManager = null, TraceSource traceSource = null)
             : base(uri, new NopReactiveServiceResolver(), scheduler, new EmptyReactiveMetadata(), store, SerializationPolicy.Default, new DefaultQuotedTypeConversionTargets(), traceSource)
        {
            _store = store;
            _context = context ?? new Dictionary<string, object>();
            _environmentServices = new Dictionary<string, object> { { "IngressEgressManager", ingressEgressManager } };
        }

        public ClientContext Client => new(new LocalReactiveServiceProvider(ServiceProvider));

        public Task CheckpointAsync(CancellationToken token = default) => CheckpointAsync(_store.GetWriter(), token, progress: null);

        public Task RecoverAsync(CancellationToken token = default) => RecoverAsync(_store.GetReader(), token, progress: null);

        protected override IHostedOperatorContext CreateOperatorContext(Uri instanceId) =>
            new CustomOperatorContext(
                new CustomOperatorContext(
                    base.CreateOperatorContext(instanceId),
                    _context
                ),
                _environmentServices
            );
    }
}
