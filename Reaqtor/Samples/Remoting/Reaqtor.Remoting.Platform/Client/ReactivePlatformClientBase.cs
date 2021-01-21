// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - October 2013 - Created this file.
//

using System;

using Reaqtor.Remoting.Client;
using Reaqtor.Remoting.Metadata;
using Reaqtor.Remoting.Platform.Firehose;
using Reaqtor.Remoting.Protocol;

namespace Reaqtor.Remoting.Platform
{
    public abstract class ReactivePlatformClientBase : IReactivePlatformClient
    {
        private readonly IReactivePlatform _platform;
        private readonly ITableClient _tableClient;
        private readonly IRemotingReactiveServiceConnection _queryCoordinator;

        private readonly MessageRouter _messageRouter;

        protected ReactivePlatformClientBase(IReactivePlatform platform)
        {
            _platform = platform;
            _queryCoordinator = platform.QueryCoordinator.GetInstance<IRemotingReactiveServiceConnection>();
            _messageRouter = GetMessageRouter(platform);
            _tableClient = GetTableClient(platform);
        }

        protected ReactivePlatformClientBase(IRemotingReactiveServiceConnection queryCoordinator, IReactiveMessagingConnection messaging)
        {
            _queryCoordinator = queryCoordinator;
            _messageRouter = GetMessageRouter(messaging);
        }

        public virtual IReactivePlatform Platform => _platform ?? throw new NotSupportedException();

        public abstract ReactiveClientContext Context
        {
            get;
        }

        public virtual IReactiveMetadataProxy MetadataProxy
        {
            get
            {
                if (_tableClient == null)
                {
                    throw new NotSupportedException();
                }

                return new AzureReactiveMetadataProxy(_tableClient, new AzureStorageResolver());
            }
        }

        protected virtual IReactiveServiceProvider ServiceProvider => new RemotingServiceProvider(new LocalReactiveServiceConnection(_queryCoordinator), GetRemoteObserver);

        protected virtual IReactiveExpressionServices ExpressionServices => new TupletizingExpressionServices(typeof(IReactiveClientProxy));

        public virtual object GetRemoteObserver(Type elementType, Uri uri)
        {
            var observerType = typeof(FirehoseObserver<>).MakeGenericType(elementType);
            return _messageRouter != null
                ? Activator.CreateInstance(observerType, uri, _messageRouter)
                : Activator.CreateInstance(observerType, uri);
        }

        private static MessageRouter GetMessageRouter(IReactivePlatform platform)
        {
            var messaging = platform.Environment.MessagingService.GetInstance<IReactiveMessagingConnection>();
            return GetMessageRouter(messaging);
        }

        private static MessageRouter GetMessageRouter(IReactiveMessagingConnection messaging)
        {
            return new MessageRouter(messaging);
        }

        private static ITableClient GetTableClient(IReactivePlatform platform)
        {
            switch (platform.Environment.StorageType)
            {
                case MetadataStorageType.Remoting:
                    var storageConnection = platform.Environment.MetadataService.GetInstance<IReactiveStorageConnection>();
                    return new StorageConnectionTableClient(storageConnection);
                // case MetadataStorageType.Azure:
                //     var storageAccount = CloudStorageAccount.Parse(platform.Environment.AzureConnectionString);
                //     return new AzureTableClient(storageAccount.CreateCloudTableClient());
                default:
                    throw new NotSupportedException();
            }

        }
    }
}
