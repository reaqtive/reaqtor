// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Concurrent;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Reaqtor;
using Reaqtor.Hosting.Shared.Serialization;
using Reaqtor.Remoting.Client;
using Reaqtor.Remoting.Grpc.Client;
using Reaqtor.Remoting.Platform;
using Reaqtor.Remoting.Platform.Grpc;
using Reaqtor.Remoting.Protocol;

namespace Reaqtor.Remoting.Grpc.Tests
{
    //
    // Milestone 8: richer end-to-end workloads (plan §10, the Reactor.*/ReificationFramework row). Beyond the simple
    // operators of M1.d, these are multi-stage compositions — filtering on a running aggregate, distinct-until-changed
    // feeding a scan, early completion via Take, four-operator pipelines — run end-to-end over gRPC AND against the
    // in-proc oracle, asserting identical results. They exercise the full command + firehose path on realistic query
    // shapes. (Porting the archived net472 Reactor.* / ReificationFramework sample *applications* wholesale is a larger
    // follow-on; this delivers equivalently rich workloads on the gRPC stack.)
    //
    [TestClass]
    public class GrpcRicherWorkloadTests
    {
        private static readonly TimeSpan ReadyTimeout = TimeSpan.FromSeconds(60);
        private static readonly Uri FireHoseObserverUri = Reaqtor.Remoting.Platform.Constants.Identifiers.Observer.FireHose.Uri;

        private static string HostAssembly =>
            typeof(GrpcRicherWorkloadTests).Assembly
                .GetCustomAttributes<AssemblyMetadataAttribute>()
                .Single(a => a.Key == "GrpcHostAssembly")
                .Value;

        private sealed record Scenario(string Name, Func<ReactiveClientContext, IAsyncReactiveQbservable<int>> BuildQuery, int[] Expected);

        private static readonly Scenario[] Scenarios =
        [
            // Filter -> project -> early completion (Take cuts the stream short).
            new("Where->Select->Take",
                ctx => ctx.Empty<int>().StartWith(1, 2, 3, 4, 5, 6, 7, 8, 9, 10).Where(x => x % 2 == 0).Select(x => x * 10).Take(3),
                [20, 40, 60]),

            // Dedup consecutive duplicates, then a running sum over the distinct values.
            new("DistinctUntilChanged->Scan",
                ctx => ctx.Empty<int>().StartWith(1, 1, 2, 2, 2, 3, 3).DistinctUntilChanged().Scan(0, (acc, x) => acc + x),
                [1, 3, 6]),

            // Skip a prefix, accumulate, then filter ON the running aggregate (stateful + post-filter).
            new("Skip->Scan->Where(on aggregate)",
                ctx => ctx.Empty<int>().StartWith(1, 2, 3, 4, 5).Skip(1).Scan(0, (acc, x) => acc + x).Where(x => x >= 5),
                [5, 9, 14]),

            // Four-stage pipeline: filter -> running sum -> scale.
            new("Where->Scan->Select (4-stage)",
                ctx => ctx.Empty<int>().StartWith(1, 2, 3, 4, 5, 6, 7, 8, 9, 10).Where(x => x % 2 == 0).Scan(0, (acc, x) => acc + x).Select(x => x * 100),
                [200, 600, 1200, 2000, 3000]),
        ];

        [Timeout(120000)] // backstop: host-launching test must not hang the run (review #13)
        [TestMethod]
        public async Task Grpc_Richer_Workloads_Parity_With_InProc_Oracle()
        {
            // Over gRPC (engine out-of-process)...
            var grpc = new int[Scenarios.Length][];
            using (var host = GrpcProcessRunnable.Launch(HostAssembly))
            {
                await host.WaitForReadyAsync(ReadyTimeout);
                using var command = new GrpcReactiveServiceConnection(host.Address);
                using var messaging = new GrpcMessagingConnection(host.Address);
                var context = new RemotingClientContext(command);

                for (var i = 0; i < Scenarios.Length; i++)
                {
                    grpc[i] = await RunScenarioAsync(context, messaging, Scenarios[i]);
                }
            }

            // ...and against the in-proc oracle.
            var oracle = new int[Scenarios.Length][];
            using (var platform = new InMemoryReactivePlatform())
            {
                await platform.StartAsync(CancellationToken.None);
                var oracleMessaging = platform.Environment.MessagingService.GetInstance<IReactiveMessagingConnection>();
                MessageRouter.Initialize(oracleMessaging);
                new ReactivePlatformDeployer(platform, new Deployable.CoreDeployable()).Deploy();

                var oracleQc = platform.QueryCoordinator.GetInstance<IRemotingReactiveServiceConnection>();
                var oracleContext = new RemotingClientContext(new InProcessReactiveServiceConnection(oracleQc));

                for (var i = 0; i < Scenarios.Length; i++)
                {
                    oracle[i] = await RunScenarioAsync(oracleContext, oracleMessaging, Scenarios[i]);
                }
            }

            for (var i = 0; i < Scenarios.Length; i++)
            {
                var name = Scenarios[i].Name;
                CollectionAssert.AreEqual(Scenarios[i].Expected, grpc[i], $"[{name}] gRPC values");
                CollectionAssert.AreEqual(oracle[i], grpc[i], $"[{name}] value parity gRPC vs oracle");
            }
        }

        private static async Task<int[]> RunScenarioAsync(ReactiveClientContext context, IReactiveMessagingConnection messaging, Scenario scenario)
        {
            var serializer = new SerializationHelpers();
            var runId = Guid.NewGuid().ToString("N");
            var topic = "reactor://m8/results/" + runId;
            var subscriptionUri = new Uri("reactor://m8/sub/" + runId);
            const int Sentinel = int.MinValue;

            var values = new ConcurrentQueue<int>();
            using var done = new SemaphoreSlim(0);
            using var probeSeen = new SemaphoreSlim(0);

            using var egress = messaging.Subscribe(topic, notification =>
            {
                switch (notification.Kind)
                {
                    case NotificationKind.OnNext:
                        var value = serializer.Deserialize<int>(notification.Value);
                        if (value == Sentinel)
                        {
                            probeSeen.Release();
                        }
                        else
                        {
                            values.Enqueue(value);
                        }
                        break;
                    case NotificationKind.OnError:
                    case NotificationKind.OnCompleted:
                        done.Release();
                        break;
                }
            });

            // The query's StartWith fires on subscribe, so confirm the egress is live on this topic first.
            var deadline = DateTime.UtcNow + TimeSpan.FromSeconds(30);
            var established = false;
            while (!established && DateTime.UtcNow < deadline)
            {
                messaging.Publish(topic, ObserverNotification.CreateOnNext(serializer.ToBytes(Sentinel)));
                established = await probeSeen.WaitAsync(TimeSpan.FromMilliseconds(250));
            }

            Assert.IsTrue(established, string.Create(CultureInfo.InvariantCulture, $"[{scenario.Name}] egress did not establish."));

            var query = scenario.BuildQuery(context);
            var firehose = context.GetObserver<Uri, int>(FireHoseObserverUri)(new Uri(topic));
            await query.SubscribeAsync(firehose, subscriptionUri, state: null, CancellationToken.None);

            Assert.IsTrue(await done.WaitAsync(TimeSpan.FromSeconds(30)),
                string.Create(CultureInfo.InvariantCulture, $"[{scenario.Name}] did not terminate (OnCompleted/OnError)."));

            await context.GetSubscription(subscriptionUri).DisposeAsync(CancellationToken.None);

            return [.. values];
        }
    }
}
