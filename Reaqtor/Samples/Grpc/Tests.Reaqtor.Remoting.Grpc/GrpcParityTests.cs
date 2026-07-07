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
using Reaqtor.Remoting.Grpc.Protocol;
using Reaqtor.Remoting.Platform;
using Reaqtor.Remoting.Platform.Grpc;
using Reaqtor.Remoting.Protocol;

namespace Reaqtor.Remoting.Grpc.Tests
{
    //
    // Milestone 1 success criterion + M1.d coverage: real reactive queries run end-to-end over gRPC and produce
    // results IDENTICAL to the in-proc oracle. The same scenario set runs against BOTH transports
    // (GrpcReactiveServiceConnection+GrpcMessagingConnection out-of-process, and InMemoryReactivePlatform+
    // InProcessReactiveServiceConnection in-proc) and is asserted equal per scenario. Each scenario:
    //   define+subscribe  query -> firehose(topic)  over the command channel; engine runs it; the FireHose observer
    //   publishes results (and OnError/OnCompleted) to the broker; the client Subscribe(topic)s and records them.
    // Covers operator composition (Select, Where) and exception propagation (the §6.1 ErrorInfo<->Exception path).
    //
    [TestClass]
    public class GrpcParityTests
    {
        private static readonly TimeSpan ReadyTimeout = TimeSpan.FromSeconds(60);
        private static readonly Uri FireHoseObserverUri = Reaqtor.Remoting.Platform.Constants.Identifiers.Observer.FireHose.Uri;

        private static string HostAssembly =>
            typeof(GrpcParityTests).Assembly
                .GetCustomAttributes<AssemblyMetadataAttribute>()
                .Single(a => a.Key == "GrpcHostAssembly")
                .Value;

        private sealed record ScenarioResult(int[] Values, string ErrorTypeName);

        private sealed record Scenario(
            string Name,
            Func<ReactiveClientContext, IAsyncReactiveQbservable<int>> BuildQuery,
            int[] ExpectedValues,
            string ExpectedErrorTypeName);

        private static readonly Scenario[] Scenarios =
        [
            new("StartWith+Select",
                ctx => ctx.Empty<int>().StartWith(0, 1, 2, 3, 4).Select(x => x + 1),
                [1, 2, 3, 4, 5], null),

            new("Where+Select (operator composition)",
                ctx => ctx.Empty<int>().StartWith(1, 2, 3, 4, 5, 6).Where(x => x % 2 == 0).Select(x => x * 10),
                [20, 40, 60], null),

            new("Select divide-by-zero (OnError propagation)",
                ctx => ctx.Empty<int>().StartWith(2, 1, 0).Select(x => 10 / x),
                [5, 10], typeof(DivideByZeroException).FullName),
        ];

        [Timeout(120000)] // backstop: host-launching test must not hang the run (review #13)
        [TestMethod]
        public async Task Grpc_Queries_Parity_With_InProc_Oracle()
        {
            // Run every scenario against the gRPC transport (engine out-of-process)...
            var grpc = new ScenarioResult[Scenarios.Length];
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
            var oracleResults = new ScenarioResult[Scenarios.Length];
            using (var oracle = new InMemoryReactivePlatform())
            {
                await oracle.StartAsync(CancellationToken.None);
                var oracleMessaging = oracle.Environment.MessagingService.GetInstance<IReactiveMessagingConnection>();
                MessageRouter.Initialize(oracleMessaging); // §3.6
                new ReactivePlatformDeployer(oracle, new Deployable.CoreDeployable()).Deploy();

                var oracleQc = oracle.QueryCoordinator.GetInstance<IRemotingReactiveServiceConnection>();
                var oracleContext = new RemotingClientContext(new InProcessReactiveServiceConnection(oracleQc));

                for (var i = 0; i < Scenarios.Length; i++)
                {
                    oracleResults[i] = await RunScenarioAsync(oracleContext, oracleMessaging, Scenarios[i]);
                }
            }

            // Per-scenario: gRPC matches expectation AND matches the oracle (values and error type).
            for (var i = 0; i < Scenarios.Length; i++)
            {
                var name = Scenarios[i].Name;
                CollectionAssert.AreEqual(Scenarios[i].ExpectedValues, grpc[i].Values, $"[{name}] gRPC values");
                Assert.AreEqual(Scenarios[i].ExpectedErrorTypeName, grpc[i].ErrorTypeName, $"[{name}] gRPC error type");
                CollectionAssert.AreEqual(oracleResults[i].Values, grpc[i].Values, $"[{name}] value parity");
                Assert.AreEqual(oracleResults[i].ErrorTypeName, grpc[i].ErrorTypeName, $"[{name}] error-type parity");
            }
        }

        private static async Task<ScenarioResult> RunScenarioAsync(ReactiveClientContext context, IReactiveMessagingConnection messaging, Scenario scenario)
        {
            var serializer = new SerializationHelpers();
            var runId = Guid.NewGuid().ToString("N");
            var topic = "reactor://test/results/" + runId;
            var subscriptionUri = new Uri("reactor://test/sub/" + runId);
            const int Sentinel = int.MinValue;

            var values = new ConcurrentQueue<int>();
            string errorTypeName = null;
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
                        // gRPC rehydrates as GrpcRemoteException carrying the original type name; in-proc carries the
                        // raw exception. Either way capture the original CLR type name for parity (§6.1).
                        errorTypeName = (notification.Exception as GrpcRemoteException)?.RemoteTypeName
                            ?? notification.Exception?.GetType().FullName;
                        done.Release();
                        break;
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

            return new ScenarioResult([.. values], errorTypeName);
        }
    }
}
