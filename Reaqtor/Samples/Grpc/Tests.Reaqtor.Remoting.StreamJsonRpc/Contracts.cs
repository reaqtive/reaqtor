// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using PolyType;

using StreamJsonRpc;

namespace Reaqtor.Remoting.Rpc.Demo
{
    //
    // RPC contracts for the StreamJsonRpc prototype. Unlike a gRPC [ServiceContract], a StreamJsonRpc method may
    // accept LIVE objects that are marshaled by reference (the receiver gets a proxy and calls back across the same
    // connection): a [RpcMarshalable] callback (the modern Reply<T>), IProgress<T>, IAsyncEnumerable<T>, and a
    // CancellationToken — and the connection is full-duplex, so the server can invoke methods on the client.
    //
    // Each member is annotated with the archived .NET Remoting primitive it is the modern, MBR-free equivalent of.
    //

    /// <summary>The server contract (the client calls these). The client end attaches a proxy via JsonRpc.Attach.</summary>
    [JsonRpcContract]
    [GenerateShape(IncludeMethods = MethodShapeFlags.PublicInstance)]
    public partial interface IComputeService
    {
        /// <summary>
        /// <b>Reply&lt;T&gt; analog.</b> Push results into a CLIENT-supplied marshaled observer, then complete it.
        /// The server holds a proxy and calls back across the SAME connection — the modern equivalent of the archived
        /// <c>Reply&lt;T&gt; : MarshalByRefObject, IObserver&lt;T&gt;</c>, with no <c>MarshalByRefObject</c>.
        /// </summary>
        Task RunIntoObserverAsync(int[] inputs, IResultObserver observer, CancellationToken cancellationToken);

        /// <summary><b>IProgress&lt;T&gt;</b> (built-in marshaling): report progress to the client during a long op.</summary>
        Task<int> SumWithProgressAsync(int[] inputs, IProgress<int> progress, CancellationToken cancellationToken);

        /// <summary><b>IAsyncEnumerable&lt;T&gt;</b> (built-in): server-streamed results, pulled lazily by the client.</summary>
        IAsyncEnumerable<int> CountAsync(int count, CancellationToken cancellationToken);

        /// <summary>
        /// <b>ClientAction / §3.4 analog.</b> The server calls BACK into the client (full-duplex) to resolve each key,
        /// then aggregates — the modern equivalent of the archived <c>ClientAction : MarshalByRefObject</c> that the
        /// server's scheduler invoked back on the client. gRPC unary has no built-in server→client call.
        /// </summary>
        Task<int> AggregateViaClientAsync(int[] keys, CancellationToken cancellationToken);

        /// <summary>
        /// <b>RemoteCancellationDisposable / ICancellationProvider analog.</b> A long op that honours cancellation —
        /// the client cancels its <see cref="CancellationToken"/> and the in-flight server op is aborted, replacing the
        /// archived GUID-keyed MBR cancellation callback with a first-class token.
        /// </summary>
        Task<int> SlowEchoAsync(int value, TimeSpan delay, CancellationToken cancellationToken);
    }

    /// <summary>
    /// A callback the SERVER invokes on the client (full-duplex). The client end registers an implementation as its
    /// local RPC target; the server end holds a proxy. This is the modern realization of the archived
    /// <c>ClientAction</c> (an MBR object the server invoked back on the client).
    /// </summary>
    [JsonRpcContract]
    [GenerateShape(IncludeMethods = MethodShapeFlags.PublicInstance)]
    public partial interface IClientCallback
    {
        Task<int> ResolveAsync(int key);
    }

    /// <summary>
    /// A marshaled observer — the direct analog of <c>Reply&lt;T&gt; : MarshalByRefObject, IObserver&lt;T&gt;</c>.
    /// <see cref="RpcMarshalableAttribute"/> makes instances pass-by-reference over JSON-RPC (the receiver gets a
    /// proxy); deriving from <see cref="IDisposable"/> bounds the marshaled object's lifetime.
    /// </summary>
    [RpcMarshalable]
    [TypeShape(IncludeMethods = MethodShapeFlags.PublicInstance)]
    public partial interface IResultObserver : IDisposable
    {
        Task OnNextAsync(int value);
        Task OnErrorAsync(string error);
        Task OnCompletedAsync();
    }
}
