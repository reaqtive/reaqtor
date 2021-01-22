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
    public class TcpReactiveEnvironment : ReactiveEnvironmentBase
    {
        public TcpReactiveEnvironment()
            : this(new TcpReactivePlatformSettings())
        {
        }

        public TcpReactiveEnvironment(ITcpReactivePlatformSettings settings)
        {
            StorageType = MetadataStorageType.Remoting;
            MetadataService = new TcpMetadataService(settings.GetExecutablePath("MetadataHost"), settings.MetadataPort, settings.MetadataUri);
            MessagingService = new TcpMessagingService(settings.GetExecutablePath("MessagingHost"), settings.MessagingPort, settings.MessagingUri);
            StateStoreService = new TcpStateStoreService(settings.GetExecutablePath("StateStoreHost"), settings.StateStorePort, settings.StateStoreUri);
            KeyValueStoreService = new TcpKeyValueStoreService(settings.GetExecutablePath("KeyValueStoreHost"), settings.KeyValueStorePort, settings.KeyValueStoreUri);
        }

        public TcpReactiveEnvironment(string azureConnectionString)
            : this(azureConnectionString, new TcpReactivePlatformSettings())
        {
        }

        public TcpReactiveEnvironment(string azureConnectionString, ITcpReactivePlatformSettings settings)
        {
            StorageType = MetadataStorageType.Azure;
            AzureConnectionString = azureConnectionString;
            MessagingService = new TcpMessagingService(settings.GetExecutablePath("MessagingHost"), settings.MessagingPort, settings.MessagingUri);
            StateStoreService = new TcpStateStoreService(settings.GetExecutablePath("StateStoreHost"), settings.StateStorePort, settings.StateStoreUri);
            KeyValueStoreService = new TcpKeyValueStoreService(settings.GetExecutablePath("KeyValueStoreHost"), settings.KeyValueStorePort, settings.KeyValueStoreUri);
        }

        public override IReactiveMetadataService MetadataService { get; }

        public override IReactiveMessagingService MessagingService { get; }

        public override IReactiveStateStoreService StateStoreService { get; }

        public override IKeyValueStoreService KeyValueStoreService { get; }

        public override MetadataStorageType StorageType { get; }

        public override string AzureConnectionString { get; }
    }
}
