// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Threading;
using System.Threading.Tasks;

using Reaqtor.Remoting.Platform;
using Reaqtor.Remoting.QueryCoordinator;
using Reaqtor.Remoting.Reactor.DomainFeeds;

namespace Reaqtor.Remoting.TestingFramework
{
    public class InMemoryTestPlatform : InMemoryReactivePlatform<QueryCoordinatorServiceConnection, TestQueryEvaluatorServiceConnection>
    {
        private readonly bool _selfContainedEnvironment;

        public InMemoryTestPlatform()
        {
            _selfContainedEnvironment = true;
        }

        public InMemoryTestPlatform(IReactiveEnvironment environment)
            : base(environment)
        {
        }

        public override async Task StartAsync(CancellationToken token)
        {
            await base.StartAsync(token);
            if (_selfContainedEnvironment)
            {
                new ReactivePlatformDeployer(this, new TestDeployable(), new Reactor.Deployable(), new DomainFeedsDeployable()).Deploy();
            }
        }
    }
}
