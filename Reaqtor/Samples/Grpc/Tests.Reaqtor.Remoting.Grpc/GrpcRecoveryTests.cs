// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ProtoBuf.Grpc.Client;

using Reaqtor;
using Reaqtor.Hosting.Shared.Serialization;
using Reaqtor.Remoting.Client;
using Reaqtor.Remoting.Grpc.Client;
using Reaqtor.Remoting.Grpc.Contracts;
using Reaqtor.Remoting.Platform.Grpc;
using Reaqtor.Remoting.Protocol;

namespace Reaqtor.Remoting.Grpc.Tests;

//
// Milestone 7: recovery / fault hardening (plan §4.2, §11.6). The QueryEvaluatorControl Checkpoint/Unload/Recover
// RPCs drive the REAL in-host CheckpointingQueryEngine over gRPC (CheckpointAsync persists active subscriptions +
// operator state to the state store; UnloadAsync tears the engine down; Recover re-instantiates it from the
// checkpoint). This test exercises that control plane end-to-end and asserts the engine survives a full
// checkpoint -> unload -> recover cycle and remains functional, and that the checkpoint persisted real state.
//
// NB (plan §3.4): driving an *active firehose subscription's* automatic resubscribe with preserved operator state
// across recovery is exercised in the in-process virtual-time axis (the engine + TestScheduler co-located), because
// recovery's source re-subscription is scheduler-driven and the scheduler does not cross gRPC. Over the wire we
// assert the control plane, checkpoint persistence, and post-recovery health.
//
[TestClass]
public class GrpcRecoveryTests
{
    private static readonly TimeSpan ReadyTimeout = TimeSpan.FromSeconds(60);
    private static readonly Uri FireHoseObservableUri = Reaqtor.Remoting.Platform.Constants.Identifiers.Observable.FireHose.Uri;
    private static readonly Uri FireHoseObserverUri = Reaqtor.Remoting.Platform.Constants.Identifiers.Observer.FireHose.Uri;

    private static string HostAssembly =>
        typeof(GrpcRecoveryTests).Assembly
            .GetCustomAttributes<AssemblyMetadataAttribute>()
            .Single(a => a.Key == "GrpcHostAssembly")
            .Value;

    [Timeout(120000)] // backstop: host-launching test must not hang the run (review #13)
    [TestMethod]
    public async Task Grpc_Checkpoint_Unload_Recover_Cycle_Keeps_Engine_Functional()
    {
        using var host = GrpcProcessRunnable.Launch(HostAssembly);
        await host.WaitForReadyAsync(ReadyTimeout);

        var serializer = new SerializationHelpers();

        using var command = new GrpcReactiveServiceConnection(host.Address);
        using var messaging = new GrpcMessagingConnection(host.Address);
        using var stateStore = new GrpcStateStoreConnection(host.Address);
        using var controlChannel = GrpcConnectionFactory.CreateChannel(host.Address);
        var control = controlChannel.CreateGrpcService<IQueryEvaluatorControl>();
        var context = new RemotingClientContext(command);

        // Phase 1: a stateful running sum reaches 6 — the engine is processing events before the checkpoint.
        var run1 = await RunStatefulSumAsync(context, messaging, serializer, payload: [1, 2, 3], expectedSum: 6);
        Assert.IsTrue(run1, "the pre-checkpoint stateful subscription did not reach the expected running sum");

        // Phase 2: checkpoint persists real engine state to the state store (observed via the M2 state-store adapter).
        await control.CheckpointAsync(Empty.Instance);
        Assert.IsTrue(stateStore.GetCategories().Any(), "Checkpoint should persist engine state to the state store (no categories were written)");

        // Phase 3: tear the engine down and rebuild it from the checkpoint — the real recovery control plane over gRPC.
        await control.UnloadAsync(Empty.Instance);
        await control.RecoverAsync(Empty.Instance);

        // Phase 4: the engine is fully functional after recovery — a fresh stateful subscription runs end-to-end
        // (command channel + firehose in/out + stateful operator), proving Unload->Recover left a healthy engine.
        var run2 = await RunStatefulSumAsync(context, messaging, serializer, payload: [4, 5], expectedSum: 9);
        Assert.IsTrue(run2, "after Unload->Recover the engine did not serve a new stateful subscription end-to-end");
    }

    //
    // Deploys firehose(topicIn) -> Scan(0, +) -> firehose(topicOut) on fresh topics, establishes the pipeline with
    // a 0-probe (0 is the additive identity, so it does not perturb the running sum), feeds the payload, and waits
    // for the cumulative running sum to reach expectedSum. Returns whether it did within the timeout.
    //
    private static async Task<bool> RunStatefulSumAsync(ReactiveClientContext context, GrpcMessagingConnection messaging, SerializationHelpers serializer, int[] payload, int expectedSum)
    {
        var runId = Guid.NewGuid().ToString("N");
        var topicIn = "reactor://m7/in/" + runId;
        var topicOut = "reactor://m7/out/" + runId;
        var subscriptionUri = new Uri("reactor://m7/sub/" + runId);

        var outputs = new ConcurrentQueue<int>();
        using var egress = messaging.Subscribe(topicOut, n =>
        {
            if (n.Kind == NotificationKind.OnNext)
            {
                outputs.Enqueue(serializer.Deserialize<int>(n.Value));
            }
        });

        var query = context.GetObservable<Uri, int>(FireHoseObservableUri)(new Uri(topicIn)).Scan(0, (acc, x) => acc + x);
        var observer = context.GetObserver<Uri, int>(FireHoseObserverUri)(new Uri(topicOut));
        await query.SubscribeAsync(observer, subscriptionUri, state: null, CancellationToken.None);

        // Establish: publish the identity (0) until an output appears (confirms the firehose subscribed and the
        // egress is receiving).
        var live = false;
        var deadline = DateTime.UtcNow + TimeSpan.FromSeconds(45);
        while (DateTime.UtcNow < deadline && !live)
        {
            messaging.Publish(topicIn, ObserverNotification.CreateOnNext<byte[]>(serializer.ToBytes(0)));
            await Task.Delay(200);
            live = !outputs.IsEmpty;
        }

        if (!live)
        {
            return false;
        }

        foreach (var value in payload)
        {
            messaging.Publish(topicIn, ObserverNotification.CreateOnNext<byte[]>(serializer.ToBytes(value)));
        }

        var reached = false;
        var collectDeadline = DateTime.UtcNow + TimeSpan.FromSeconds(30);
        while (DateTime.UtcNow < collectDeadline && !reached)
        {
            reached = outputs.Contains(expectedSum);
            if (!reached)
            {
                await Task.Delay(100);
            }
        }

        await context.GetSubscription(subscriptionUri).DisposeAsync(CancellationToken.None);
        return reached;
    }
}
