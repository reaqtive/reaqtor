// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;

using MagicOnion;

namespace Reaqtor.Remoting.Rpc.MagicOnionDemo
{
    //
    // MagicOnion contracts. MagicOnion is a gRPC-native RPC framework (Grpc.AspNetCore server + Grpc.Net.Client),
    // so these run over real HTTP/2 — unlike the StreamJsonRpc prototype, this stays on the SAME transport stack the
    // spike already adopted for gRPC. Two shapes:
    //
    //   * IService<T>      — unary request/response (the command-plane analog; maps to a gRPC unary method).
    //   * IStreamingHub<,> — a bidirectional "hub": hub methods are client->server (and may return a value), while the
    //                        receiver interface is server->client (push). This is the gRPC-native equivalent of the
    //                        StreamJsonRpc Reply<T>/ClientAction demos — server->client calls WITHOUT hand-rolling a
    //                        duplex-stream message protocol.
    //

    /// <summary>Unary service (request/response).</summary>
    public interface IGreeterService : IService<IGreeterService>
    {
        UnaryResult<int> SumAsync(int a, int b);
    }

    /// <summary>
    /// Bidirectional StreamingHub. Hub methods (this interface) are invoked client→server; the server pushes back to
    /// the client through <see cref="IComputeHubReceiver"/>.
    /// </summary>
    public interface IComputeHub : IStreamingHub<IComputeHub, IComputeHubReceiver>
    {
        /// <summary>client → server, returns a value.</summary>
        Task<int> SumAsync(int[] values);

        /// <summary>client → server; the server then pushes <paramref name="count"/> values back via the receiver.</summary>
        Task StartFeedAsync(int count, int factor);
    }

    /// <summary>Server → client callbacks (the firehose-push / <c>Reply&lt;T&gt;</c> analog).</summary>
    public interface IComputeHubReceiver
    {
        void OnValue(int value);

        void OnCompleted();
    }
}
