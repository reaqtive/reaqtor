// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;

using Reaqtor.Remoting.Client;
using Reaqtor.Remoting.Metadata;
using Reaqtor.Remoting.Protocol;

namespace Reaqtor.Remoting.QueryCoordinator;

public class QueryCoordinatorServiceConnection : RemotingReactiveServiceConnectionBase
{
    private readonly CommandTextFactory<ExpressionSlim> _commandTextFactory = new();
    private readonly CommandTextParser<ExpressionSlim> _commandTextParser = new();

    private IReactivePlatformConfiguration _configuration;

    public override void Configure(IReactivePlatformConfiguration configuration)
    {
        _configuration = configuration;
    }

    public override void Start()
    {
        // ADAPTATION (plan §2.5, adaptation #1): the archived QC wrapped each query-evaluator connection in a
        // .NET Remoting LocalReactiveServiceConnection (which new'd up a ReactiveServiceCommandProxy : RemoteProxyBase).
        // That pair is not ported (no System.Runtime.Remoting on net10.0). For the in-proc QC->QE leg we use
        // InProcessReactiveServiceConnection (Reaqtor.Remoting.Protocol, in Reaqtor.Remoting.Core), which connects
        // directly to the engine's IRemotingReactiveServiceConnection with no transport in between. 'qe' is an
        // IReactiveQueryEvaluatorConnection : IRemotingReactiveServiceConnection, which satisfies the ctor. The
        // cross-process transport substitutes its own IReactiveServiceConnection (Reaqtor.Remoting.Grpc.*).
        var providers = _configuration.QueryEvaluatorConnections.Select(qe =>
            new ReactiveServiceProvider<ExpressionSlim>(new InProcessReactiveServiceConnection(qe), _commandTextFactory, _commandTextFactory)).ToList();

        // QC -> QE placement (plan §4.6 / Milestone 6): with a single evaluator, preserve the archived
        // first-evaluator behaviour exactly; with ≥2 evaluators, distribute by a deterministic hash of the entity
        // URI so the load spreads and each subscription's create/delete co-locate on the same evaluator.
        var selector = providers.Count > 1
            ? (IQueryEvaluatorSelector)ConsistentHashQueryEvaluatorSelector.Instance
            : FirstQueryEvaluatorSelector.Instance;

        var serviceProvider = new QueryCoordinatorServiceProvider(GetMetadata(_configuration), providers, selector);
        Connection = new ReactiveServiceConnection<ExpressionSlim>(serviceProvider, _commandTextParser, _commandTextParser);
    }

    protected override void DisposeCore()
    {
    }

    private static AzureReactiveMetadataProxy GetMetadata(IReactivePlatformConfiguration configuration)
    {
        // NB (plan §2.6, adaptation #2): functionally unchanged. AzureReactiveMetadataProxy, StorageConnectionTableClient
        // and AzureStorageResolver now come from the Cosmos-free metadata layer in Reaqtor.Remoting.Platform.Core
        // (namespace Reaqtor.Remoting.Metadata; already imported above) instead of the archived net472 Platform. The
        // commented-out MetadataStorageType.Azure (Cosmos.Table) branch is left exactly as the archive has it; it is
        // not supported on net10.0 and the default case still throws.
        switch (configuration.StorageType)
        {
            case MetadataStorageType.Remoting:
                var tableClient = new StorageConnectionTableClient(configuration.StorageConnection);
                var resolver = new AzureStorageResolver();
                return new AzureReactiveMetadataProxy(tableClient, resolver);
            // case MetadataStorageType.Azure:
            //     var storageAccount = Microsoft.WindowsAzure.Storage.CloudStorageAccount.Parse(configuration.AzureConnectionString);
            //     return new AzureReactiveMetadataProxy(
            //         new Microsoft.WindowsAzure.Storage.AzureTableClient(storageAccount.CreateCloudTableClient()),
            //         new AzureStorageResolver());
            default:
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Unexpected metadata storage type '{0}'.", configuration.StorageType));
        }
    }
}
