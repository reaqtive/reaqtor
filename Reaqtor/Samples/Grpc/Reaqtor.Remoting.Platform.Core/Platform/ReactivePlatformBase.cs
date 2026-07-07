// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - October 2013 - Created this file.
//

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Reaqtor.Remoting.Protocol;

namespace Reaqtor.Remoting.Platform;

//
// ADAPTATION (plan §3.2 / §3.3): the archived ReactivePlatformBase.StartAsync opened with
//
//     ChannelServices.RegisterChannel(new TcpChannel(props, clientProvider,
//         new BinaryServerFormatterSinkProvider { TypeFilterLevel = TypeFilterLevel.Full }), ...);
//
// i.e. it registered a .NET Remoting TcpChannel with a BinaryFormatter sink at TypeFilterLevel.Full. None of
// System.Runtime.Remoting exists on net10.0, and the BinaryFormatter path is removed entirely. This
// transport-neutral base therefore registers NO channel: StartAsync only composes the service graph and
// starts the services (evaluators first, then the coordinator - the same ordering as the archived Tcp
// platform). A concrete transport (e.g. the gRPC platform, a later milestone) supplies the actual wire
// behaviour by overriding StartAsync/CreateClient and by materializing the config's connection getters from
// addresses (plan §3.2). The [Serializable] nested configuration is replaced by the plain
// ReactivePlatformConfiguration class (plan §3.3).
//
// CreateClient() is abstract: the archived base returned `new ReactivePlatformClient(this)`, but
// ReactivePlatformClient/ReactivePlatformClientBase are NOT ported here - they depend on client-only types
// (RemotingServiceProvider / LocalReactiveServiceConnection / RemotingClientContext /
// TupletizingExpressionServices) that are ported later in Reaqtor.Remoting.Client.Core (step 0a.4) and on the
// gRPC client connection. The concrete platform supplies CreateClient().
//
public abstract class ReactivePlatformBase : IReactivePlatform
{
    private readonly IReactiveEnvironment _environment;
    private readonly ReactivePlatformConfiguration _configuration;

    protected ReactivePlatformBase(IReactiveEnvironment environment)
    {
        _environment = environment;
        _configuration = new ReactivePlatformConfiguration();
    }

    public abstract IReactiveQueryCoordinator QueryCoordinator { get; }

    public abstract IEnumerable<IReactiveQueryEvaluator> QueryEvaluators { get; }

    public virtual IReactiveEnvironment Environment => _environment;

    public virtual IReactivePlatformConfiguration Configuration => _configuration;

    public virtual async Task StartAsync(CancellationToken token)
    {
        _configuration.MessagingConnection = _environment.MessagingService.GetInstance<IReactiveMessagingConnection>();
        _configuration.StateStoreConnection = _environment.StateStoreService.GetInstance<IReactiveStateStoreConnection>();
        _configuration.KeyValueStoreConnection = _environment.KeyValueStoreService.GetInstance<ITransactionalKeyValueStoreConnection>();

        _configuration.StorageType = _environment.StorageType;
        if (_configuration.StorageType == MetadataStorageType.Remoting)
        {
            _configuration.StorageConnection = _environment.MetadataService.GetInstance<IReactiveStorageConnection>();
        }
        else if (_configuration.StorageType == MetadataStorageType.Azure)
        {
            _configuration.AzureConnectionString = _environment.AzureConnectionString;
        }

        foreach (var queryEvaluator in QueryEvaluators)
        {
            if (_configuration.StorageType == MetadataStorageType.Remoting)
            {
                queryEvaluator.Register(_environment.MetadataService);
            }

            queryEvaluator.Register(_environment.StateStoreService);
            queryEvaluator.Register(_environment.MessagingService);
            queryEvaluator.Register(_environment.KeyValueStoreService);
            await queryEvaluator.StartAsync(token).ConfigureAwait(false);
            _configuration.QueryEvaluatorConnections.Add(queryEvaluator.GetInstance<IReactiveQueryEvaluatorConnection>());
            QueryCoordinator.Register(queryEvaluator);
        }

        if (_configuration.StorageType == MetadataStorageType.Remoting)
        {
            QueryCoordinator.Register(_environment.MetadataService);
        }
        await QueryCoordinator.StartAsync(token).ConfigureAwait(false);
    }

    public virtual async Task StopAsync(CancellationToken token)
    {
        await QueryCoordinator.StopAsync(token).ConfigureAwait(false);

        foreach (var queryEvaluator in QueryEvaluators)
        {
            await queryEvaluator.StopAsync(token).ConfigureAwait(false);
        }
    }

    public abstract IReactivePlatformClient CreateClient();

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            StopAsync(CancellationToken.None).Wait();
        }
    }
}
