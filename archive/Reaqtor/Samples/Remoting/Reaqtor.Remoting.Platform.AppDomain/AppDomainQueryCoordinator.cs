// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - October 2013 - Created this file.
//

using Reaqtor.Remoting.Protocol;

namespace Reaqtor.Remoting.Platform
{
    internal sealed class AppDomainQueryCoordinator<TQueryCoordinator> : ReactivePlatformServiceBase, IReactiveQueryCoordinator
        where TQueryCoordinator : IRemotingReactiveServiceConnection
    {
        public AppDomainQueryCoordinator(IReactivePlatform platform)
#pragma warning disable CA2000 // Dispose objects before losing scope. (Ownership transfer.)
            : base(platform, new AppDomainRunnable("QueryCoordinator", typeof(TQueryCoordinator)), ReactiveServiceType.QueryCoordinator)
#pragma warning restore CA2000
        {
        }

        protected override void RegisterQueryEvaluator(IReactiveQueryEvaluator service)
        {
            Helpers.MarshalServiceInstance(this, service);
        }

        protected override void RegisterMetadataService(IReactiveMetadataService service)
        {
            Helpers.MarshalServiceInstance(this, service);
        }
    }
}
