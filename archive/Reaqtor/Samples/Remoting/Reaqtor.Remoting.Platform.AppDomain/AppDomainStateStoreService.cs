// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using Reaqtor.Remoting.StateStore;

namespace Reaqtor.Remoting.Platform
{
    public sealed class AppDomainStateStoreService : ReactiveServiceBase, IReactiveStateStoreService
    {
        public AppDomainStateStoreService()
#pragma warning disable CA2000 // Dispose objects before losing scope. (Ownership transfer.)
            : base(new AppDomainRunnable("StateStoreService", typeof(StateStoreConnection)), ReactiveServiceType.StateStoreService)
#pragma warning restore CA2000
        {
        }
    }
}
