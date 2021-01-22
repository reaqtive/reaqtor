// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - October 2013 - Created this file.
//

using Reaqtor.Remoting.Platform;

namespace Reaqtor.Remoting.Reactor.Client
{
    public class AgentsClient : ReactivePlatformClientBase
    {
        public AgentsClient(IReactivePlatform platform)
            : base(platform)
        {
        }

        public override ReactiveClientContext Context => new AgentsTestContext(ExpressionServices, ServiceProvider);
    }
}
