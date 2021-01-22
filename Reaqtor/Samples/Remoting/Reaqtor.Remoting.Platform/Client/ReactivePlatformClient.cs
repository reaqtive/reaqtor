// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - October 2013 - Created this file.
//

using Reaqtor.Remoting.Client;
using Reaqtor.Remoting.Protocol;

namespace Reaqtor.Remoting.Platform
{
    public class ReactivePlatformClient : ReactivePlatformClientBase
    {
        public ReactivePlatformClient(IReactivePlatform platform)
            : base(platform)
        {
        }

        public ReactivePlatformClient(IRemotingReactiveServiceConnection queryCoordinator, IReactiveMessagingConnection messaging)
            : base(queryCoordinator, messaging)
        {
        }

        public override ReactiveClientContext Context => new RemotingClientContext(ExpressionServices, ServiceProvider);
    }
}
