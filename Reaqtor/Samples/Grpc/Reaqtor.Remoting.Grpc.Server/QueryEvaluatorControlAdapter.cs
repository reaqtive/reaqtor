// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;

using ProtoBuf.Grpc;

using Reaqtor.Remoting.Grpc.Contracts;

namespace Reaqtor.Remoting.Grpc.Server
{
    //
    // Server-side adapter for the QE-only control surface (plan §4.2 / §11.6 / Milestone 7): Checkpoint / Unload /
    // Recover drive the in-host CheckpointingQueryEngine via the ported IReactiveQueryEvaluatorConnection. These are
    // the real engine operations (CheckpointAsync persists active subscriptions + operator state to the state store;
    // UnloadAsync tears the engine down — including in-engine firehose broker subscriptions; Recover re-instantiates
    // the engine from the checkpoint, re-subscribing the firehoses). The (potentially slow, blocking) engine calls
    // are offloaded so the gRPC dispatch thread is not held.
    //
    /// <summary>Code-first implementation of <see cref="IQueryEvaluatorControl"/> over the in-host engine.</summary>
    public sealed class QueryEvaluatorControlAdapter : IQueryEvaluatorControl
    {
        private readonly EngineHost _engine;

        public QueryEvaluatorControlAdapter(EngineHost engine)
        {
            _engine = engine ?? throw new ArgumentNullException(nameof(engine));
        }

        public async Task<Empty> CheckpointAsync(Empty request, CallContext context = default)
        {
            await Task.Run(() => _engine.QueryEvaluatorConnection.Checkpoint()).ConfigureAwait(false);
            return Empty.Instance;
        }

        public async Task<Empty> UnloadAsync(Empty request, CallContext context = default)
        {
            await Task.Run(() => _engine.QueryEvaluatorConnection.Unload()).ConfigureAwait(false);
            return Empty.Instance;
        }

        public async Task<Empty> RecoverAsync(Empty request, CallContext context = default)
        {
            await Task.Run(() => _engine.QueryEvaluatorConnection.Recover()).ConfigureAwait(false);
            return Empty.Instance;
        }
    }
}
