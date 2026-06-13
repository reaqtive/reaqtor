// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using Reaqtor.Remoting.KeyValueStore;

namespace Reaqtor.Remoting.Platform
{
    public class AppDomainKeyValueStoreService : ReactiveServiceBase, IKeyValueStoreService
    {
        public AppDomainKeyValueStoreService()
#pragma warning disable CA2000 // Dispose objects before losing scope. (Ownership transfer.)
            : base(new AppDomainRunnable("KeyValueStoreService", typeof(KeyValueStoreConnection)), ReactiveServiceType.KeyValueStoreService)
#pragma warning restore CA2000
        {
        }
    }
}
