// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Reaqtor;

namespace Reaqtor.Remoting.TestingFramework;

//
// Milestone 0a "done" criterion (see GRPC-REMOTING-SPIKE-PLAN.md §10): prove that the ported net10.0
// stack — Reaqtor.Remoting.Core / Platform.Core / Client.Core / Deployable.Core / Engine /
// Platform.InMemory.Core / TestingFramework.Core — runs a reactive query end-to-end entirely in-process,
// with no .NET Remoting / AppDomain / Cosmos / MarshalByRefObject anywhere on the path. This is the live
// oracle the gRPC transport (Milestone 0b+) will be measured against.
//
// RemotingTestBase boots an InMemoryReactivePlatform (InMemoryTestPlatform) over the ported
// QueryCoordinatorServiceConnection + TestQueryEvaluatorServiceConnection, deploys TestDeployable (which
// extends CoreDeployable, so the full standard operator/observer surface is defined), and drives the test
// body over a RemotingClientContext whose client -> QC link is the in-process InProcessReactiveServiceConnection.
//
[TestClass]
public class OracleSmokeTests : RemotingTestBase
{
    //
    // The canonical virtual-time vertical slice: define a finite source, subscribe it to the TestObserver
    // (the egress that records timestamped notifications into the QE's DI-scoped observer store), advance the
    // virtual clock, and assert the recorded timeline. Exercises client command path (Define/Subscribe), the
    // QC -> QE in-process command link, engine subscription instantiation + scheduling, and the egress readout.
    //
    [TestMethod]
    public async Task Oracle_Subscribe_VirtualTime_ObserverState()
    {
        var subscriptionUri = new Uri("reactor://test/oracle/subscription");
        var testObserverName = new Uri("reactor://test/oracle/observer");

        await AssertVirtual<ReactiveClientContext, int>(
            (ctx, scheduler) =>
            {
                var observer = ctx.GetObserver<Uri, int>(Constants.Test.TestObserver.Uri)(testObserverName);

                scheduler.ScheduleAbsolute(200, () =>
                    ctx.Empty<int>().StartWith(0, 1, 2, 3, 4).SubscribeAsync(observer, subscriptionUri, null, CancellationToken.None));

                scheduler.ScheduleAbsolute(500, () =>
                    ctx.GetSubscription(subscriptionUri).DisposeAsync(CancellationToken.None));

                return Task.FromResult(true);
            },
            new ObserverState<int>(testObserverName)
            {
                ObserverMessage.OnNext(201, 0),
                ObserverMessage.OnNext(202, 1),
                ObserverMessage.OnNext(203, 2),
                ObserverMessage.OnNext(204, 3),
                ObserverMessage.OnNext(205, 4),
                ObserverMessage.OnCompleted<int>(206),
            });
    }
}
