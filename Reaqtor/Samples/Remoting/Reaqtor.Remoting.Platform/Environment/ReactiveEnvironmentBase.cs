// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - December 2013 - Created this file.
//

using System;
using System.Threading;
using System.Threading.Tasks;

using Reaqtor.Remoting.Protocol;

namespace Reaqtor.Remoting.Platform
{
    public abstract class ReactiveEnvironmentBase : IReactiveEnvironment
    {
        public abstract MetadataStorageType StorageType
        {
            get;
        }

        public abstract IReactiveMetadataService MetadataService
        {
            get;
        }

        public abstract string AzureConnectionString
        {
            get;
        }

        public abstract IReactiveMessagingService MessagingService
        {
            get;
        }

        public abstract IReactiveStateStoreService StateStoreService
        {
            get;
        }

        public abstract IKeyValueStoreService KeyValueStoreService
        {
            get;
        }

        public virtual async Task StartAsync(CancellationToken token)
        {
            if (StorageType == MetadataStorageType.Remoting)
            {
                await MetadataService.StartAsync(token);
            }

            await MessagingService.StartAsync(token);
            ServiceInstanceHelpers.SetMessagingServiceInstance(AppDomain.CurrentDomain, MessagingService.GetInstance<IReactiveMessagingConnection>());
            await StateStoreService.StartAsync(token);
            await KeyValueStoreService.StartAsync(token);
        }


        public virtual async Task StopAsync(CancellationToken token)
        {
            await StateStoreService.StopAsync(token);
            await MessagingService.StopAsync(token);
            await KeyValueStoreService.StopAsync(token);

            if (StorageType == MetadataStorageType.Remoting)
            {
                await MetadataService.StopAsync(token);
            }
        }

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
}
