// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Threading;
using System.Threading.Tasks;

using Reaqtor.Remoting.Platform;
using Reaqtor.Remoting.QueryCoordinator;

namespace Reaqtor.Remoting.TestingFramework;

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
        await base.StartAsync(token).ConfigureAwait(false);
        if (_selfContainedEnvironment)
        {
            // NB: the archived deployer also deployed new Reactor.Deployable() and new DomainFeedsDeployable();
            //     those Reactor.*/DomainFeeds deployables are out of 0a scope (plan §10, Milestone 2+) and their
            //     projects are not ported. TestDeployable : CoreDeployable, so deploying it alone defines the core
            //     operator/observer surface the oracle needs.
            new ReactivePlatformDeployer(this, new TestDeployable()).Deploy();
        }
    }
}
