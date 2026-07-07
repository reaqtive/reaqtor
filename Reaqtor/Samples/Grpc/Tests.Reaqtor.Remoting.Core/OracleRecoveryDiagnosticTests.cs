// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Concurrent;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Reaqtor;
using Reaqtor.Hosting.Shared.Serialization;
using Reaqtor.Remoting.Client;
using Reaqtor.Remoting.Deployable;
using Reaqtor.Remoting.Platform;
using Reaqtor.Remoting.Protocol;

namespace Reaqtor.Remoting.TestingFramework
{
    //
    // CHARACTERIZATION (root-causes the M7 over-the-wire observation). Question: after Checkpoint -> Unload -> Recover,
    // does a subscription sourced from the BROKER firehose (FirehoseSubscribable via MessageRouter) resume receiving
    // broker events? This runs ENTIRELY IN-PROCESS on the ported InMemoryReactivePlatform — no gRPC, no process
    // boundary, the in-host MessagingConnection's publish/subscribe is synchronous — so it removes every transport and
    // ordering variable the gRPC M7 test had.
    //
    // Result (locked in below): it does NOT resume — identical to the gRPC observation. So the behaviour is a property
    // of the ported engine's recovery semantics for externally-sourced (broker) subscriptions, NOT a gRPC limitation:
    // the broker subscription is not part of checkpointed engine state (plan §11.6) and recovery does not re-run the
    // firehose operator's OnStart to re-subscribe. The engine itself recovers to a fully healthy state — a fresh
    // subscription created after recovery runs end-to-end — so the gap is specifically the auto-resubscribe of a
    // previously-checkpointed broker-firehose source.
    //
    // The query is firehose(topicIn) -> Scan(0,+) -> firehose(topicOut). 0 is the additive identity, so a 0-probe
    // re-emits the current running sum without perturbing it (a safe liveness/state probe for the stateful operator).
    //
    [TestClass]
    public class OracleRecoveryDiagnosticTests
    {
        [TestMethod]
        public async Task Oracle_Recovery_BrokerFirehose_DoesNotAutoResubscribe_But_Engine_Is_Healthy()
        {
            using var platform = new InMemoryReactivePlatform();
            await platform.StartAsync(CancellationToken.None);

            var messaging = platform.Environment.MessagingService.GetInstance<IReactiveMessagingConnection>();
            MessageRouter.Initialize(messaging);
            new ReactivePlatformDeployer(platform, new CoreDeployable()).Deploy();

            var qc = platform.QueryCoordinator.GetInstance<IRemotingReactiveServiceConnection>();
            var qe = platform.QueryEvaluators.First().GetInstance<IReactiveQueryEvaluatorConnection>();
            var context = new RemotingClientContext(new InProcessReactiveServiceConnection(qc));
            var serializer = new SerializationHelpers();

            // A stateful running sum sourced from the broker firehose; drive it to 6.
            var topicIn = "reactor://diag/in";
            var outputs = SubscribeEgress(messaging, serializer, "reactor://diag/out");
            await SubscribeSumAsync(context, "reactor://diag/in", "reactor://diag/out", new Uri("reactor://diag/sub"));
            Assert.IsTrue(await PublishUntilAsync(messaging, serializer, topicIn, 0, () => !outputs.IsEmpty, TimeSpan.FromSeconds(20)), "pipeline did not become live");
            foreach (var v in new[] { 1, 2, 3 })
            {
                messaging.Publish(topicIn, ObserverNotification.CreateOnNext<byte[]>(serializer.ToBytes(v)));
            }
            Assert.IsTrue(await WaitUntilAsync(() => outputs.Contains(6), TimeSpan.FromSeconds(20)), "running sum did not reach 6");

            var sixesBeforeRecover = outputs.Count(x => x == 6);

            // Checkpoint persists real engine state, then a full in-process Unload -> Recover cycle.
            qe.Checkpoint();
            Assert.IsTrue(platform.Environment.StateStoreService.GetInstance<IReactiveStateStoreConnection>().GetCategories().Any(),
                "Checkpoint should persist engine state to the state store");
            qe.Unload();
            qe.Recover();

            // CHARACTERIZATION: the recovered broker-firehose subscription does NOT resume. A 0-probe (which would
            // re-emit the preserved sum 6 if it had resubscribed) produces nothing new, in-process, within a generous
            // window. (Empirically also confirmed unchanged at 30s; if recovery is ever taught to re-subscribe broker
            // sources this assertion flips and the docs/§13 must be revisited.)
            var recoveredResumed = await PublishUntilAsync(messaging, serializer, topicIn, 0, () => outputs.Count(x => x == 6) > sixesBeforeRecover, TimeSpan.FromSeconds(12));
            Assert.IsFalse(recoveredResumed,
                string.Create(CultureInfo.InvariantCulture, $"EXPECTED current behaviour: a recovered broker-firehose subscription does not auto-resubscribe. If this now resumes, update plan §13. outputs=[{string.Join(",", outputs)}]"));

            // But the ENGINE is healthy after recovery: a fresh subscription on new topics runs end-to-end.
            var fresh = SubscribeEgress(messaging, serializer, "reactor://diag/out2");
            await SubscribeSumAsync(context, "reactor://diag/in2", "reactor://diag/out2", new Uri("reactor://diag/sub2"));
            const string freshIn = "reactor://diag/in2";
            Assert.IsTrue(await PublishUntilAsync(messaging, serializer, freshIn, 0, () => !fresh.IsEmpty, TimeSpan.FromSeconds(20)), "post-recovery fresh pipeline did not become live");
            foreach (var v in new[] { 4, 5 })
            {
                messaging.Publish(freshIn, ObserverNotification.CreateOnNext<byte[]>(serializer.ToBytes(v)));
            }
            Assert.IsTrue(await WaitUntilAsync(() => fresh.Contains(9), TimeSpan.FromSeconds(20)),
                "after recovery the engine should serve a fresh stateful subscription end-to-end (4,5 -> 9)");
        }

        private static ConcurrentQueue<int> SubscribeEgress(IReactiveMessagingConnection messaging, SerializationHelpers serializer, string topicOut)
        {
            var outputs = new ConcurrentQueue<int>();
            messaging.Subscribe(topicOut, n =>
            {
                if (n.Kind == NotificationKind.OnNext)
                {
                    outputs.Enqueue(serializer.Deserialize<int>(n.Value));
                }
            });
            return outputs;
        }

        private static async Task SubscribeSumAsync(ReactiveClientContext context, string topicIn, string topicOut, Uri subscriptionUri)
        {
            var observable = context.GetObservable<Uri, int>(Reaqtor.Remoting.Platform.Constants.Identifiers.Observable.FireHose.Uri)(new Uri(topicIn)).Scan(0, (acc, x) => acc + x);
            var observer = context.GetObserver<Uri, int>(Reaqtor.Remoting.Platform.Constants.Identifiers.Observer.FireHose.Uri)(new Uri(topicOut));
            await observable.SubscribeAsync(observer, subscriptionUri, state: null, CancellationToken.None);
        }

        private static async Task<bool> PublishUntilAsync(IReactiveMessagingConnection messaging, SerializationHelpers serializer, string topic, int value, Func<bool> predicate, TimeSpan timeout)
        {
            var deadline = DateTime.UtcNow + timeout;
            while (DateTime.UtcNow < deadline)
            {
                messaging.Publish(topic, ObserverNotification.CreateOnNext<byte[]>(serializer.ToBytes(value)));
                if (predicate())
                {
                    return true;
                }
                await Task.Delay(200);
            }
            return predicate();
        }

        private static async Task<bool> WaitUntilAsync(Func<bool> predicate, TimeSpan timeout)
        {
            var deadline = DateTime.UtcNow + timeout;
            while (DateTime.UtcNow < deadline)
            {
                if (predicate())
                {
                    return true;
                }
                await Task.Delay(100);
            }
            return predicate();
        }
    }
}
