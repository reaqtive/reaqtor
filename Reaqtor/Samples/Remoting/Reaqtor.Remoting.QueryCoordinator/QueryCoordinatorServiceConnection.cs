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

namespace Reaqtor.Remoting.QueryCoordinator
{
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
            var providers = _configuration.QueryEvaluatorConnections.Select(qe =>
                new ReactiveServiceProvider<ExpressionSlim>(new LocalReactiveServiceConnection(qe), _commandTextFactory, _commandTextFactory));
            var serviceProvider = new QueryCoordinatorServiceProvider(GetMetadata(_configuration), providers);
            Connection = new ReactiveServiceConnection<ExpressionSlim>(serviceProvider, _commandTextParser, _commandTextParser);
        }

        protected override void DisposeCore()
        {
        }

        private static AzureReactiveMetadataProxy GetMetadata(IReactivePlatformConfiguration configuration)
        {
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
}
