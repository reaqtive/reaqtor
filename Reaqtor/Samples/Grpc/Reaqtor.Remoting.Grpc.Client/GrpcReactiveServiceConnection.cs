// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading;
using System.Threading.Tasks;

using global::Grpc.Core;
using global::Grpc.Net.Client;

using ProtoBuf.Grpc;
using ProtoBuf.Grpc.Client;

using Reaqtor.Remoting.Grpc.Contracts;
using Reaqtor.Remoting.Protocol;

namespace Reaqtor.Remoting.Grpc.Client
{
    //
    // The gRPC peer of the in-process InProcessReactiveServiceConnection (Reaqtor.Remoting.Core, §2.5): an
    // IReactiveServiceConnection that issues each command as a unary Execute RPC over the command channel (§4.1,
    // §4.7). The ported ReactivePlatformClientBase uses this in place of the deleted LocalReactiveServiceConnection.
    // The full command round-trip is exercised in Milestone 1 (the server Execute adapter wraps the ported engine
    // connection); for 0b this is a compiled, dial-able deliverable.
    //
    /// <summary>A gRPC-backed <see cref="IReactiveServiceConnection"/> over the command channel.</summary>
    public sealed class GrpcReactiveServiceConnection : IReactiveServiceConnection, IDisposable
    {
        private readonly GrpcChannel _channel;
        private readonly bool _ownsChannel;
        private readonly IReactiveServiceConnectionService _service;

        /// <summary>Creates a connection to a command-channel host address, owning the underlying channel.</summary>
        public GrpcReactiveServiceConnection(string address)
            : this(GrpcConnectionFactory.CreateChannel(address), ownsChannel: true)
        {
        }

        /// <summary>Creates a connection over an existing channel.</summary>
        public GrpcReactiveServiceConnection(GrpcChannel channel, bool ownsChannel = false)
        {
            _channel = channel ?? throw new ArgumentNullException(nameof(channel));
            _ownsChannel = ownsChannel;
            _service = channel.CreateGrpcService<IReactiveServiceConnectionService>();
        }

        public IReactiveServiceCommand CreateCommand(CommandVerb verb, CommandNoun noun, string commandText)
        {
            return new GrpcReactiveServiceCommand(_service, this, verb, noun, commandText);
        }

        public void Dispose()
        {
            if (_ownsChannel)
            {
                _channel.Dispose();
            }
        }

        private sealed class GrpcReactiveServiceCommand : IReactiveServiceCommand
        {
            private readonly IReactiveServiceConnectionService _service;

            public GrpcReactiveServiceCommand(IReactiveServiceConnectionService service, IReactiveServiceConnection connection, CommandVerb verb, CommandNoun noun, string commandText)
            {
                _service = service;
                Connection = connection;
                Verb = verb;
                Noun = noun;
                CommandText = commandText;
            }

            public IReactiveServiceConnection Connection { get; }

            public CommandVerb Verb { get; }

            public CommandNoun Noun { get; }

            public string CommandText { get; }

            public async Task<string> ExecuteAsync(CancellationToken token)
            {
                var request = new ExecuteRequest { Verb = Verb, Noun = Noun, CommandText = CommandText };
                var response = await _service.ExecuteAsync(request, new CallContext(new CallOptions(cancellationToken: token))).ConfigureAwait(false);
                return response.Result;
            }
        }
    }
}
