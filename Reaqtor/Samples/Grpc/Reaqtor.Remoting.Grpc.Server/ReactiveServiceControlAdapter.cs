// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Threading;
using System.Threading.Tasks;

using global::Grpc.Core;

using ProtoBuf.Grpc;

using Reaqtor.Remoting.Grpc.Contracts;
using Reaqtor.Remoting.Protocol;

namespace Reaqtor.Remoting.Grpc.Server;

//
// Server-side adapter for the lifecycle/control surface (plan §4.2). Hosted as a singleton by the QC/QE
// Kestrel hosts (§7). For Milestone 0b (the scaffold) this delivers a working readiness probe (Ping) and the
// real storage-type validation (§3.3); the engine bring-up that Start drives is completed in Milestone 1, once
// the four synchronous gRPC store adapters (§4.4) exist for the ported QueryEvaluatorServiceConnection.Start to
// read from config (config.MessagingConnection/StateStoreConnection/KeyValueStoreConnection/StorageConnection).
//
/// <summary>Code-first implementation of <see cref="IReactiveServiceControl"/> (Configure/Start/Ping).</summary>
public sealed class ReactiveServiceControlAdapter : IReactiveServiceControl
{
    private int _started;

    /// <summary>Gets the configuration accepted by the most recent <see cref="ConfigureAsync"/> call, if any.</summary>
    public PlatformConfiguration Configuration { get; private set; }

    /// <summary>Gets a value indicating whether <see cref="StartAsync"/> has been called.</summary>
    public bool IsStarted => Volatile.Read(ref _started) != 0;

    /// <summary>
    /// Validates and retains the platform configuration. Only <see cref="MetadataStorageType.Remoting"/> is
    /// supported on net10.0 (plan §2.6/§4.2); anything else fails fast with <c>InvalidArgument</c> at Configure
    /// rather than throwing lazily at Start.
    /// </summary>
    public Task<Empty> ConfigureAsync(PlatformConfiguration request, CallContext context = default)
    {
        if (request == null)
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "A platform configuration is required."));
        }

        if (request.StorageType != MetadataStorageType.Remoting)
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument,
                $"Unsupported metadata storage type '{request.StorageType}'. net10.0 supports only '{MetadataStorageType.Remoting}' (see plan §2.6)."));
        }

        Configuration = request;
        return Task.FromResult(Empty.Instance);
    }

    /// <summary>
    /// Acknowledges Start. NB (Milestone 0b): this is the control-plane acknowledgement only — the ported
    /// QueryEvaluatorServiceConnection.Start engine bring-up reads the four typed store connections from config
    /// and is wired in Milestone 1 once the gRPC store adapters (§4.4) exist. The 0b scaffold asserts Ping, not
    /// engine execution, so no engine work is faked here.
    /// </summary>
    public Task<Empty> StartAsync(Empty request, CallContext context = default)
    {
        if (Configuration == null)
        {
            throw new RpcException(new Status(StatusCode.FailedPrecondition, "Configure must be called before Start."));
        }

        Interlocked.Exchange(ref _started, 1);
        return Task.FromResult(Empty.Instance);
    }

    /// <summary>Readiness probe — returns immediately once the host is accepting calls.</summary>
    public Task<Empty> PingAsync(Empty request, CallContext context = default) => Task.FromResult(Empty.Instance);
}
