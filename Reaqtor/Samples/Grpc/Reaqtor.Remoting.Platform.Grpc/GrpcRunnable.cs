// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

using global::Grpc.Net.Client;

using ProtoBuf.Grpc.Client;

namespace Reaqtor.Remoting.Platform.Grpc;

/// <summary>
/// The gRPC analogue of the archived <c>TcpRunnable&lt;T&gt;</c> (§3.1): <see cref="Instance"/> returns a typed
/// code-first client bound to the service's channel — what the archived <c>Activator.GetObject</c> returned as a
/// transparent MBR proxy. The engine consumes one of the sync engine-facing interfaces via a client adapter
/// (§4.4); for plain control/command contracts <typeparamref name="TContract"/> is the gRPC service interface.
/// </summary>
public sealed class GrpcRunnable<TContract>
    where TContract : class
{
    private readonly GrpcChannel _channel;

    /// <summary>Creates a runnable over the given channel.</summary>
    public GrpcRunnable(GrpcChannel channel)
    {
        _channel = channel ?? throw new ArgumentNullException(nameof(channel));
    }

    /// <summary>The typed code-first client for <typeparamref name="TContract"/>.</summary>
    public object Instance => _channel.CreateGrpcService<TContract>();
}
