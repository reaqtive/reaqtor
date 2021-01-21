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
    public class AppDomainReactiveEnvironment : ReactiveEnvironmentBase
    {
        public AppDomainReactiveEnvironment()
        {
            StorageType = MetadataStorageType.Remoting;
            MetadataService = new AppDomainMetadataService();
            MessagingService = new AppDomainMessagingService();
            StateStoreService = new AppDomainStateStoreService();
            KeyValueStoreService = new AppDomainKeyValueStoreService();
        }

        public AppDomainReactiveEnvironment(string azureConnectionString)
        {
            StorageType = MetadataStorageType.Azure;
            AzureConnectionString = azureConnectionString;
            MessagingService = new AppDomainMessagingService();
            StateStoreService = new AppDomainStateStoreService();
            KeyValueStoreService = new AppDomainKeyValueStoreService();
        }

        public override IReactiveMetadataService MetadataService { get; }

        public override IReactiveMessagingService MessagingService { get; }

        public override IReactiveStateStoreService StateStoreService { get; }

        public override IKeyValueStoreService KeyValueStoreService { get; }

        public override MetadataStorageType StorageType { get; }

        public override string AzureConnectionString { get; }
    }
}
