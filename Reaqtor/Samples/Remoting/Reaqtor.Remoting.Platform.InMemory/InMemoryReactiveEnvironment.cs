// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - December 2013 - Created this file.
//

using System.Threading;
using System.Threading.Tasks;

using Reaqtor.Remoting.Protocol;

namespace Reaqtor.Remoting.Platform
{
    public class InMemoryReactiveEnvironment : ReactiveEnvironmentBase
    {
        private readonly bool _stateStoreOnly;

        public InMemoryReactiveEnvironment()
        {
            StorageType = MetadataStorageType.Remoting;
            MetadataService = new InMemoryMetadataService();
            MessagingService = new InMemoryMessagingService();
            StateStoreService = new InMemoryStateStoreService();
            KeyValueStoreService = new InMemoryKeyValueStoreService();
        }

        public InMemoryReactiveEnvironment(string connectionString)
        {
            StorageType = MetadataStorageType.Azure;
            AzureConnectionString = connectionString;
            MessagingService = new InMemoryMessagingService();
            StateStoreService = new InMemoryStateStoreService();
            KeyValueStoreService = new InMemoryKeyValueStoreService();
        }

        public InMemoryReactiveEnvironment(IReactiveMetadataService metadataService, IReactiveMessagingService messagingService)
        {
            StorageType = MetadataStorageType.Remoting;
            MetadataService = metadataService;
            MessagingService = messagingService;
            StateStoreService = new InMemoryStateStoreService();
            KeyValueStoreService = new InMemoryKeyValueStoreService();
            _stateStoreOnly = true;
        }

        public override IReactiveMetadataService MetadataService { get; }

        public override IReactiveMessagingService MessagingService { get; }

        public override IReactiveStateStoreService StateStoreService { get; }

        public override IKeyValueStoreService KeyValueStoreService { get; }

        public override MetadataStorageType StorageType { get; }

        public override string AzureConnectionString { get; }

        public override Task StartAsync(CancellationToken token)
        {
            if (_stateStoreOnly)
            {
                return StateStoreService.StartAsync(token);
            }
            else
            {
                return base.StartAsync(token);
            }
        }
    }
}
