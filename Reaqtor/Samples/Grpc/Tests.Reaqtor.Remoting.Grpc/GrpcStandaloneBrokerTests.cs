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

using Reaqtor;
using Reaqtor.Hosting.Shared.Serialization;
using Reaqtor.Remoting.Client;
using Reaqtor.Remoting.Grpc.Client;
using Reaqtor.Remoting.Platform.Grpc;
using Reaqtor.Remoting.Protocol;

namespace Reaqtor.Remoting.Grpc.Tests;

//
// Milestone 5: the standalone Messaging broker host (plan §4.3 / §9). The broker is promoted out of the QE host
// into its OWN process (MessagingHost). Two cross-process properties are asserted:
//   1. Server-side fan-out across connections: one Publish fans out to ≥2 Subscribe streams on different client
//      connections to the standalone broker.
//   2. Both firehose legs across processes: a query input-firehose(topicIn) -> Select -> output-firehose(topicOut)
//      runs in the QE host, whose MessageRouter is pointed at the standalone broker, so the engine SUBSCRIBES to
//      topicIn on the broker (QE-as-subscriber, input leg) and PUBLISHES to topicOut on the broker
//      (QE-as-publisher, output leg). A client publishes inputs to the broker and reads transformed outputs from
//      the broker — every hop crossing a process boundary through the single shared broker.
//
[TestClass]
public class GrpcStandaloneBrokerTests
{
    private static readonly TimeSpan ReadyTimeout = TimeSpan.FromSeconds(120);
    private static readonly Uri FireHoseObservableUri = Reaqtor.Remoting.Platform.Constants.Identifiers.Observable.FireHose.Uri;
    private static readonly Uri FireHoseObserverUri = Reaqtor.Remoting.Platform.Constants.Identifiers.Observer.FireHose.Uri;

    private static string HostAssembly => Metadata("GrpcHostAssembly");
    private static string MessagingHostAssembly => Metadata("GrpcMessagingHostAssembly");

    private static string Metadata(string key) =>
        typeof(GrpcStandaloneBrokerTests).Assembly
            .GetCustomAttributes<AssemblyMetadataAttribute>()
            .Single(a => a.Key == key)
            .Value;

    [Timeout(120000)] // backstop: host-launching test must not hang the run (review #13)
    [TestMethod]
    public async Task Grpc_StandaloneBroker_FanOut_Across_Connections()
    {
        using var broker = GrpcProcessRunnable.Launch(MessagingHostAssembly);
        await broker.WaitForReadyAsync(ReadyTimeout);

        const string topic = "reactor://grpc/m5/fanout";
        var payload = new byte[] { 7, 7, 7 };

        using var publisher = new GrpcMessagingConnection(broker.Address);
        using var subscriberA = new GrpcMessagingConnection(broker.Address);
        using var subscriberB = new GrpcMessagingConnection(broker.Address);

        using var gotA = new SemaphoreSlim(0);
        using var gotB = new SemaphoreSlim(0);
        var receivedA = new ConcurrentQueue<byte[]>();
        var receivedB = new ConcurrentQueue<byte[]>();

        using var subA = subscriberA.Subscribe(topic, n => { if (n.Kind == NotificationKind.OnNext) { receivedA.Enqueue(n.Value); gotA.Release(); } });
        using var subB = subscriberB.Subscribe(topic, n => { if (n.Kind == NotificationKind.OnNext) { receivedB.Enqueue(n.Value); gotB.Release(); } });

        // Re-publish until BOTH subscribers (each on its own connection) have received, riding out the async
        // establishment of the two server-streaming subscriptions.
        var deadline = DateTime.UtcNow + TimeSpan.FromSeconds(30);
        var a = false;
        var b = false;
        while (DateTime.UtcNow < deadline && !(a && b))
        {
            publisher.Publish(topic, ObserverNotification.CreateOnNext<byte[]>(payload));
            if (!a)
            {
                a = await gotA.WaitAsync(TimeSpan.FromMilliseconds(200));
            }
            if (!b)
            {
                b = await gotB.WaitAsync(TimeSpan.FromMilliseconds(200));
            }
        }

        Assert.IsTrue(a, "subscriber A (its own connection) did not receive the fan-out publish");
        Assert.IsTrue(b, "subscriber B (its own connection) did not receive the fan-out publish");
        Assert.IsTrue(receivedA.TryDequeue(out var valueA));
        CollectionAssert.AreEqual(payload, valueA);
        Assert.IsTrue(receivedB.TryDequeue(out var valueB));
        CollectionAssert.AreEqual(payload, valueB);
    }

    [Timeout(120000)] // backstop: host-launching test must not hang the run (review #13)
    [TestMethod]
    public async Task Grpc_StandaloneBroker_BothLegs_Across_Processes()
    {
        using var broker = GrpcProcessRunnable.Launch(MessagingHostAssembly);
        await broker.WaitForReadyAsync(ReadyTimeout);

        // The QE host routes its firehose legs to the standalone broker (arg[1] = broker address).
        using var qe = GrpcProcessRunnable.Launch(HostAssembly, null, broker.Address);
        await qe.WaitForReadyAsync(ReadyTimeout);

        var serializer = new SerializationHelpers();
        var runId = Guid.NewGuid().ToString("N");
        var topicIn = "reactor://m5/in/" + runId;
        var topicOut = "reactor://m5/out/" + runId;
        var subscriptionUri = new Uri("reactor://m5/sub/" + runId);

        using var command = new GrpcReactiveServiceConnection(qe.Address);
        using var messaging = new GrpcMessagingConnection(broker.Address); // client talks to the BROKER, not the QE
        var context = new RemotingClientContext(command);

        var outputs = new ConcurrentQueue<int>();
        using var egress = messaging.Subscribe(topicOut, n =>
        {
            if (n.Kind == NotificationKind.OnNext)
            {
                outputs.Enqueue(serializer.Deserialize<int>(n.Value));
            }
        });

        // input firehose(topicIn) -> Select(x*10) -> output firehose(topicOut). Both firehoses resolve via the
        // QE host's MessageRouter, which points at the standalone broker — so both legs cross the process boundary.
        var input = context.GetObservable<Uri, int>(FireHoseObservableUri)(new Uri(topicIn)).Select(x => x * 10);
        var output = context.GetObserver<Uri, int>(FireHoseObserverUri)(new Uri(topicOut));
        await input.SubscribeAsync(output, subscriptionUri, state: null, CancellationToken.None);

        const int Probe = 7; // 7 -> 70, distinct from the payload outputs (10/20/30)

        // Liveness: publish the probe to topicIn until its transform (70) appears at topicOut. Because both legs
        // run over the standalone broker, seeing 70 confirms the WHOLE cross-process pipeline is live (engine
        // subscribed to topicIn on the broker, transforming, and publishing to topicOut; client egress receiving).
        var deadline = DateTime.UtcNow + TimeSpan.FromSeconds(45);
        var live = false;
        while (DateTime.UtcNow < deadline && !live)
        {
            messaging.Publish(topicIn, ObserverNotification.CreateOnNext<byte[]>(serializer.ToBytes(Probe)));
            await Task.Delay(200);
            live = outputs.Contains(70);
        }

        Assert.IsTrue(live, "the cross-process firehose pipeline did not become live within the timeout");

        // Push the real payload through the input leg; expect the transformed values out the output leg.
        foreach (var value in new[] { 1, 2, 3 })
        {
            messaging.Publish(topicIn, ObserverNotification.CreateOnNext<byte[]>(serializer.ToBytes(value)));
        }

        var expected = new[] { 10, 20, 30 };
        var collectDeadline = DateTime.UtcNow + TimeSpan.FromSeconds(20);
        while (DateTime.UtcNow < collectDeadline && PayloadOutputs(outputs).Length < expected.Length)
        {
            await Task.Delay(100);
        }

        CollectionAssert.AreEqual(expected, PayloadOutputs(outputs), "both legs across processes: payload did not transform end-to-end through the standalone broker");

        await context.GetSubscription(subscriptionUri).DisposeAsync(CancellationToken.None);
    }

    // The transformed payload values, excluding the liveness probe's transform (70).
    private static int[] PayloadOutputs(ConcurrentQueue<int> outputs)
        => [.. outputs.Where(x => x != 70).Distinct().OrderBy(x => x)];
}
