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
    //
    // ADAPTATION (plan §3.6): the archived StartAsync called
    // ServiceInstanceHelpers.SetMessagingServiceInstance(AppDomain.CurrentDomain, MessagingService.GetInstance<...>())
    // to publish the messaging connection into an AppDomain-keyed data slot that MessageRouter later read back.
    // There is no AppDomain (nor cross-domain data slot) on net10.0, so that call is removed. The resolved
    // IReactiveMessagingConnection is instead held in a plain in-process property (MessagingConnection) populated
    // during StartAsync; in-process consumers (e.g. a MessageRouter, FirehoseObserver) obtain it by injection
    // rather than via a static cross-domain slot. This preserves single-process semantics with no remoting.
    //
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

        /// <summary>
        /// Gets the messaging connection resolved when the environment was started, or <c>null</c> if the
        /// environment has not been started. This replaces the AppDomain-keyed messaging service instance that
        /// the archived code stashed via <c>ServiceInstanceHelpers.SetMessagingServiceInstance</c> (plan §3.6).
        /// </summary>
        public IReactiveMessagingConnection MessagingConnection { get; private set; }

        public virtual async Task StartAsync(CancellationToken token)
        {
            if (StorageType == MetadataStorageType.Remoting)
            {
                await MetadataService.StartAsync(token).ConfigureAwait(false);
            }

            await MessagingService.StartAsync(token).ConfigureAwait(false);
            MessagingConnection = MessagingService.GetInstance<IReactiveMessagingConnection>();
            await StateStoreService.StartAsync(token).ConfigureAwait(false);
            await KeyValueStoreService.StartAsync(token).ConfigureAwait(false);
        }


        public virtual async Task StopAsync(CancellationToken token)
        {
            await StateStoreService.StopAsync(token).ConfigureAwait(false);
            await MessagingService.StopAsync(token).ConfigureAwait(false);
            await KeyValueStoreService.StopAsync(token).ConfigureAwait(false);

            if (StorageType == MetadataStorageType.Remoting)
            {
                await MetadataService.StopAsync(token).ConfigureAwait(false);
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
