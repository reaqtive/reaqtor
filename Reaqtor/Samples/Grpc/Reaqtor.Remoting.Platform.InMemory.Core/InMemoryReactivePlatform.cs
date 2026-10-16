// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - October 2013 - Created this file.
//

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Reaqtor.Remoting.Protocol;
using Reaqtor.Remoting.QueryCoordinator;
using Reaqtor.Remoting.QueryEvaluator;

namespace Reaqtor.Remoting.Platform;

public class InMemoryReactivePlatform<TQueryCoordinator, TQueryEvaluator> : ReactivePlatformBase
    where TQueryCoordinator : IRemotingReactiveServiceConnection, new()
    where TQueryEvaluator : IReactiveQueryEvaluatorConnection, new()
{
    private readonly InMemoryQueryEvaluator<TQueryEvaluator> _queryEvaluator;
    private readonly InMemoryQueryCoordinator<TQueryCoordinator> _queryCoordinator;
    private readonly bool _selfContainedEnvironment;

    public InMemoryReactivePlatform()
#pragma warning disable CA2000 // Dispose objects before losing scope. (Ownership transfer.)
        : this(new InMemoryReactiveEnvironment())
#pragma warning restore CA2000
    {
        _selfContainedEnvironment = true;
    }

    public InMemoryReactivePlatform(IReactiveEnvironment environment)
        : base(environment)
    {
        _queryEvaluator = new InMemoryQueryEvaluator<TQueryEvaluator>(this);
        _queryCoordinator = new InMemoryQueryCoordinator<TQueryCoordinator>(this);
    }

    public override IReactiveQueryCoordinator QueryCoordinator => _queryCoordinator;

    public override IEnumerable<IReactiveQueryEvaluator> QueryEvaluators
    {
        get
        {
            yield return _queryEvaluator;
        }
    }

    // NB (plan §3.2): the archived ReactivePlatformBase supplied a non-abstract
    //     CreateClient() => new ReactivePlatformClient(this). The ported transport-neutral base
    //     (Reaqtor.Remoting.Platform.Core) made it abstract because the client driver depends on
    //     client-only types; the in-process platform supplies it here.
    public override IReactivePlatformClient CreateClient() => new ReactivePlatformClient(this);

    public override async Task StartAsync(CancellationToken token)
    {
        if (_selfContainedEnvironment)
        {
            await Environment.StartAsync(CancellationToken.None).ConfigureAwait(false);
        }

        await base.StartAsync(token).ConfigureAwait(false);
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        if (disposing && _selfContainedEnvironment)
        {
            Environment.Dispose();
        }
    }
}

public class InMemoryReactivePlatform : InMemoryReactivePlatform<QueryCoordinatorServiceConnection, QueryEvaluatorServiceConnection>
{
    public InMemoryReactivePlatform()
    {
    }

    public InMemoryReactivePlatform(IReactiveEnvironment environment)
        : base(environment)
    {
    }
}
