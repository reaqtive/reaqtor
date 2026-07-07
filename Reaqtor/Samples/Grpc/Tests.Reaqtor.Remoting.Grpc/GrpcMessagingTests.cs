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

using Reaqtor.Remoting.Grpc.Client;
using Reaqtor.Remoting.Platform.Grpc;
using Reaqtor.Remoting.Protocol;

namespace Reaqtor.Remoting.Grpc.Tests;

//
// Milestone 1.b: the messaging broker works over gRPC end-to-end (plan §4.3). A client publishes onto a topic
// and a client subscriber receives it, fanned out through the in-host MessagingConnection behind the gRPC
// Messaging service (unary Publish + server-streaming Subscribe). This is the §3.7 value-readout egress that
// the full vertical slice (M1.c) uses to observe engine-produced results.
//
[TestClass]
public class GrpcMessagingTests
{
    private static readonly TimeSpan ReadyTimeout = TimeSpan.FromSeconds(60);

    private static string HostAssembly =>
        typeof(GrpcMessagingTests).Assembly
            .GetCustomAttributes<AssemblyMetadataAttribute>()
            .Single(a => a.Key == "GrpcHostAssembly")
            .Value;

    [Timeout(120000)] // backstop: host-launching test must not hang the run (review #13)
    [TestMethod]
    public async Task Grpc_Messaging_Broker_RoundTrip_Over_The_Wire()
    {
        using var host = GrpcProcessRunnable.Launch(HostAssembly);
        await host.WaitForReadyAsync(ReadyTimeout);

        const string topic = "reactor://grpc/test/topic";
        var payload = new byte[] { 1, 2, 3, 4 };

        using var messaging = new GrpcMessagingConnection(host.Address);

        var received = new ConcurrentQueue<byte[]>();
        using var gotOne = new SemaphoreSlim(0);

        using var subscription = messaging.Subscribe(topic, notification =>
        {
            if (notification.Kind == NotificationKind.OnNext)
            {
                received.Enqueue(notification.Value);
                gotOne.Release();
            }
        });

        // Re-publish until delivered, to ride out the asynchronous establishment of the server-streaming
        // subscription (the broker fans out only to subscribers registered at publish time).
        var deadline = DateTime.UtcNow + TimeSpan.FromSeconds(30);
        var delivered = false;
        while (DateTime.UtcNow < deadline && !delivered)
        {
            messaging.Publish(topic, ObserverNotification.CreateOnNext<byte[]>(payload));
            delivered = await gotOne.WaitAsync(TimeSpan.FromMilliseconds(250));
        }

        Assert.IsTrue(delivered, "Did not receive a published notification over the gRPC broker within the timeout.");
        Assert.IsTrue(received.TryDequeue(out var got));
        CollectionAssert.AreEqual(payload, got);
    }
}
