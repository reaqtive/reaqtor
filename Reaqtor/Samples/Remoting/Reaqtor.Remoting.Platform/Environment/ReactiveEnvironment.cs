// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - December 2013 - Created this file.
//

using Reaqtor.Remoting.Protocol;

namespace Reaqtor.Remoting.Platform
{
    public class ReactiveEnvironment : ReactiveEnvironmentBase
    {
        public ReactiveEnvironment(IReactiveMetadataService metadataService, IReactiveMessagingService messagingService, IReactiveStateStoreService stateStoreService, IKeyValueStoreService keyValueStoreService)
        {
            StorageType = MetadataStorageType.Remoting;
            MetadataService = metadataService;
            MessagingService = messagingService;
            StateStoreService = stateStoreService;
            KeyValueStoreService = keyValueStoreService;
        }

        public ReactiveEnvironment(string azureConnectionString, IReactiveMessagingService messagingService, IReactiveStateStoreService stateStoreService, IKeyValueStoreService keyValueStoreService)
        {
            StorageType = MetadataStorageType.Azure;
            AzureConnectionString = azureConnectionString;
            MessagingService = messagingService;
            StateStoreService = stateStoreService;
            KeyValueStoreService = keyValueStoreService;
        }

        public override IReactiveMetadataService MetadataService { get; }

        public override IReactiveMessagingService MessagingService { get; }

        public override IReactiveStateStoreService StateStoreService { get; }

        public override IKeyValueStoreService KeyValueStoreService { get; }

        public override MetadataStorageType StorageType { get; }

        public override string AzureConnectionString { get; }
    }
}
