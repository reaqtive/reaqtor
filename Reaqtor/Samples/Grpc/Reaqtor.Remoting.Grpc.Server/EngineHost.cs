// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Reaqtor.Remoting.Deployable;
using Reaqtor.Remoting.Platform;
using Reaqtor.Remoting.Protocol;

namespace Reaqtor.Remoting.Grpc.Server;

//
// Milestone 1 (MVP) engine bring-up. The gRPC host runs the SAME in-process engine the InMemory oracle runs
// (InMemoryReactivePlatform: a started QueryCoordinator + QueryEvaluator over in-host stores), and the gRPC
// service adapters put a transport facade in front of it. This reuses the proven oracle wiring verbatim — the
// only difference from the in-proc oracle is that commands arrive over the gRPC Execute RPC instead of an
// in-process InProcessReactiveServiceConnection. (Separate per-role QC/QE/store hosts are Milestones 4-6; the
// MVP topology is one host running the full in-proc engine behind gRPC — plan §7/§9.)
//
/// <summary>
/// Owns the in-host reactive engine for a gRPC host: starts an <see cref="InMemoryReactivePlatform"/>, deploys
/// the standard operator/observer surface (<see cref="CoreDeployable"/>), and exposes the query-coordinator
/// connection that the command-channel adapter executes against.
/// </summary>
public sealed class EngineHost : IDisposable
{
    private readonly Lock _gate = new();
    private InMemoryReactivePlatform _platform;
    private IRemotingReactiveServiceConnection _queryCoordinator;
    private IReactiveQueryEvaluatorConnection _queryEvaluator;
    private bool _started;
    private bool _disposed;

    /// <summary>The started query-coordinator connection (entry point for client commands).</summary>
    public IRemotingReactiveServiceConnection QueryCoordinatorConnection
    {
        get
        {
            EnsureStarted();
            return _queryCoordinator;
        }
    }

    /// <summary>
    /// The query-evaluator connection — the control surface for checkpoint/unload/recover (§11.6 / Milestone 7).
    /// On the MVP single-host topology this is the same in-host engine the coordinator drives.
    /// </summary>
    public IReactiveQueryEvaluatorConnection QueryEvaluatorConnection
    {
        get
        {
            EnsureStarted();
            return _queryEvaluator;
        }
    }

    /// <summary>
    /// The single in-host broker the engine publishes results to and subscribes inputs from (§3.6). The
    /// messaging gRPC adapter fans this same instance out to/from remote clients.
    /// </summary>
    public IReactiveMessagingConnection MessagingConnection
    {
        get
        {
            EnsureStarted();
            return field;
        }

        private set;
    }

    /// <summary>
    /// The engine's checkpoint state store (§4.4). This is the SAME instance the engine reads as
    /// <c>config.StateStoreConnection</c> for checkpoint/recover (<c>ReactivePlatformBase</c> populates the
    /// config from <c>Environment.StateStoreService.GetInstance&lt;…&gt;()</c>), so the gRPC StateStore adapter
    /// exposes the real engine store, not a side copy.
    /// </summary>
    public IReactiveStateStoreConnection StateStoreConnection
    {
        get
        {
            EnsureStarted();
            return field;
        }

        private set;
    }

    /// <summary>
    /// The engine's transactional key-value store (§4.4) — the SAME instance the engine reads as
    /// <c>config.KeyValueStoreConnection</c> (populated by <c>ReactivePlatformBase</c> from
    /// <c>Environment.KeyValueStoreService.GetInstance&lt;…&gt;()</c>), so the gRPC KeyValueStore adapter exposes
    /// the engine's store, not a side copy.
    /// </summary>
    public ITransactionalKeyValueStoreConnection KeyValueStoreConnection
    {
        get
        {
            EnsureStarted();
            return field;
        }

        private set;
    }

    /// <summary>
    /// The engine's metadata storage connection (§4.4.2) — the flat-CRUD <see cref="IReactiveStorageConnection"/>
    /// the engine reads as <c>config.StorageConnection</c> on the <c>Remoting</c> metadata path
    /// (<c>MetadataService.GetInstance&lt;…&gt;()</c>). The metadata IQueryable layer runs engine-side over this
    /// flat connection (Cosmos-free, §2.6); the gRPC Storage adapter exposes only the flat CRUD surface.
    /// </summary>
    public IReactiveStorageConnection StorageConnection
    {
        get
        {
            EnsureStarted();
            return field;
        }

        private set;
    }

    /// <summary>Starts the in-host platform and deploys the standard surface (idempotent, thread-safe).</summary>
    public void EnsureStarted()
    {
        if (Volatile.Read(ref _started))
        {
            return;
        }

        lock (_gate)
        {
            ObjectDisposedException.ThrowIf(_disposed, this);

            if (_started)
            {
                return;
            }

#pragma warning disable CA2000 // The platform is owned by this EngineHost and disposed in Dispose().
            var platform = new InMemoryReactivePlatform();
#pragma warning restore CA2000
            platform.StartAsync(CancellationToken.None).GetAwaiter().GetResult();

            var messaging = platform.Environment.MessagingService.GetInstance<IReactiveMessagingConnection>();
            var stateStore = platform.Environment.StateStoreService.GetInstance<IReactiveStateStoreConnection>();
            var keyValueStore = platform.Environment.KeyValueStoreService.GetInstance<ITransactionalKeyValueStoreConnection>();
            var storage = platform.Environment.MetadataService.GetInstance<IReactiveStorageConnection>();

            // §3.6: point the process-wide firehose router at this host's single broker BEFORE deploying/serving,
            // so the engine-side FirehoseObserver(uri) publishes results to the same MessagingConnection the gRPC
            // Messaging service exposes to clients. CoreDeployable only *defines* the firehose (a stored lambda);
            // instantiation happens later at Subscribe time, by which point Instance is initialized.
            MessageRouter.Initialize(messaging);

            // Deploy the standard operator/observer surface in-process (exactly as the oracle does), so client
            // commands that reference those operators resolve server-side.
            new ReactivePlatformDeployer(platform, new CoreDeployable()).Deploy();

            _platform = platform;
            _queryCoordinator = platform.QueryCoordinator.GetInstance<IRemotingReactiveServiceConnection>();
            _queryEvaluator = platform.QueryEvaluators.First().GetInstance<IReactiveQueryEvaluatorConnection>();
            MessagingConnection = messaging;
            StateStoreConnection = stateStore;
            KeyValueStoreConnection = keyValueStore;
            StorageConnection = storage;
            Volatile.Write(ref _started, true);
        }
    }

    public void Dispose()
    {
        lock (_gate)
        {
            if (_disposed)
            {
                return;
            }

            _disposed = true;

            // The connection instances are owned by the platform's runnables (which only null their Instance on
            // dispose, never disposing it), so we dispose them here. The QC's DisposeCore is empty; the QE's
            // flushes its trace and disposes its scheduler.
            _queryCoordinator?.Dispose();
            _queryEvaluator?.Dispose();
            _platform?.Dispose();
            _queryCoordinator = null;
            _queryEvaluator = null;
            MessagingConnection = null;
            StateStoreConnection = null;
            KeyValueStoreConnection = null;
            StorageConnection = null;
            _platform = null;
            _started = false;
        }
    }
}
