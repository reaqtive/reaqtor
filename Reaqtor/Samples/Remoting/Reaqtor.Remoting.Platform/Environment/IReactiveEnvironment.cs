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
    public interface IReactiveEnvironment : IDisposable
    {
        MetadataStorageType StorageType { get; }
        IReactiveMetadataService MetadataService { get; }
        string AzureConnectionString { get; }
        IReactiveMessagingService MessagingService { get; }
        IReactiveStateStoreService StateStoreService { get; }
        IKeyValueStoreService KeyValueStoreService { get; }
        Task StartAsync(CancellationToken token);
        Task StopAsync(CancellationToken token);
    }
}
