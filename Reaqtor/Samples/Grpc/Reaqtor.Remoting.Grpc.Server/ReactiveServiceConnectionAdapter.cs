// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Threading;
using System.Threading.Tasks;

using ProtoBuf.Grpc;

using Reaqtor.Remoting.Grpc.Contracts;
using Reaqtor.Remoting.Protocol;

namespace Reaqtor.Remoting.Grpc.Server
{
    //
    // Server side of the command channel (plan §4.1): turns each Execute RPC into a command against the in-host
    // engine's query-coordinator connection. It reuses the in-process command bridge (InProcessReactiveService
    // connection / ReactiveServiceCommandService, Reaqtor.Remoting.Core) — the engine sees an ordinary in-proc
    // command; only the verb/noun/command_text (Bonsai/DataModel JSON) crossed the wire. This is the server mirror
    // of the client-side GrpcReactiveServiceConnection.
    //
    /// <summary>Code-first implementation of <see cref="IReactiveServiceConnectionService"/> over the in-host engine.</summary>
    public sealed class ReactiveServiceConnectionAdapter : IReactiveServiceConnectionService
    {
        private readonly EngineHost _engine;

        public ReactiveServiceConnectionAdapter(EngineHost engine)
        {
            _engine = engine ?? throw new System.ArgumentNullException(nameof(engine));
        }

        public async Task<ExecuteResponse> ExecuteAsync(ExecuteRequest request, CallContext context = default)
        {
            System.ArgumentNullException.ThrowIfNull(request);

            var token = context.ServerCallContext?.CancellationToken ?? CancellationToken.None;

            var connection = new InProcessReactiveServiceConnection(_engine.QueryCoordinatorConnection);
            var command = connection.CreateCommand(request.Verb, request.Noun, request.CommandText);
            var result = await command.ExecuteAsync(token).ConfigureAwait(false);

            return new ExecuteResponse { Result = result };
        }
    }
}
