// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - August 2014 - Created this file.
//

using System;
using System.Threading;
using System.Threading.Tasks;

using Reaqtor.Remoting.Protocol;

#pragma warning disable CA1303 // Do not pass literals as localized parameters. (No localization in sample code.)

namespace Reaqtor.Remoting.Platform
{
    internal sealed class TcpMultiRoleReactiveEnvironment : ReactiveEnvironment
    {
        private const int RetryCount = 1000;

        public TcpMultiRoleReactiveEnvironment(IReactiveMetadataService metadataService, IReactiveMessagingService messagingService, IReactiveStateStoreService stateStoreService, IKeyValueStoreService _keyValueStoreService)
            : base(metadataService, messagingService, stateStoreService, _keyValueStoreService)
        {
        }

        public override async Task StartAsync(CancellationToken token)
        {
            await base.StartAsync(token).ConfigureAwait(false);

            var count = 0;
            while (count < RetryCount)
            {
                try
                {
                    MessagingService.GetInstance<IReactiveConnection>().Ping();
                    MetadataService.GetInstance<IReactiveConnection>().Ping();
                    StateStoreService.GetInstance<IReactiveConnection>().Ping();
                    KeyValueStoreService.GetInstance<IReactiveConnection>().Ping();
                }
#pragma warning disable CA1031 // REVIEW: Do not catch general exception types.
                catch
                {
                    Console.WriteLine("Waiting for services to start (retry attempt {0})...", ++count);
                }
#pragma warning restore CA1031
            }

            if (count == RetryCount)
            {
                throw new InvalidOperationException("Error starting the multi-role environment.");
            }
        }
    }
}
