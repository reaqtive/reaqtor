// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

using Reaqtive;
using Reaqtive.Testing;
using Reaqtive.TestingFramework;

#if GLITCHING
using Reaqtor;
using Reaqtor.TestingFramework;
#endif

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.Reaqtive.Operators
{
    public partial class Regressions : OperatorTestBase
    {
        [TestMethod]
        public void Regressions_StartWith_Timer()
        {
            Run(client =>
            {
                var context = GetContext(client);

                var res = client.Start(() =>
                    context.Timer(TimeSpan.FromTicks(100)).Take(1).StartWith(1, 2, 3).Take(4)
                );

                res.Messages.AssertEqual(
                    OnNext(Increment(200, 1), 1L),
                    OnNext(Increment(200, 2), 2L),
                    OnNext(Increment(200, 3), 3L),
                    OnNext(Increment(200, 3) + 100, 0L),
                    OnCompleted<long>(Increment(200, 3) + 100)
                );
            });
        }

        [TestMethod]
        public void Regressions_StartWith_Take_1()
        {
            Run(client =>
            {
                var context = GetContext(client);

                var res = client.Start(() =>
                    context.Empty<int>()
                        .StartWith(0)
                        .Take(1)
                );

                res.Messages.AssertEqual(
                    OnNext(Increment(200, 1), 0),
                    OnCompleted<int>(Increment(200, 1))
                );
            });
        }

        [TestMethod]
        public void Regressions_Traffic_StartWithFires()
        {
            Run(client =>
            {
                var extraBuffer = TimeSpan.Zero;
                var notificationThreshold = TimeSpan.FromTicks(20);
                var travelTimeWithoutTraffic = TimeSpan.FromTicks(100);
                var initialTraceTimeWithTraffic = travelTimeWithoutTraffic + TimeSpan.FromTicks(50);
                var startMonitoring = new DateTimeOffset(Subscribed, TimeSpan.Zero);
                var eventStartTime = new DateTimeOffset(600, TimeSpan.Zero);
                var overhead = notificationThreshold + extraBuffer;
                var noTrafficNotifyTime = eventStartTime - travelTimeWithoutTraffic - overhead;
                var stopMonitoring = noTrafficNotifyTime + TimeSpan.Zero;
                var customTrafficDelay = travelTimeWithoutTraffic + TimeSpan.FromTicks(75);

                var context = GetContext(client);

                var noTrafficValue = 42;
                var someTrafficValue = 49;

                var res = client.Start(() =>
                    context.Timer(TimeSpan.FromTicks(1000), TimeSpan.FromTicks(1000))
                        .DelaySubscription(startMonitoring)
                        .TakeUntil(stopMonitoring)
                        .Where(x => x == 0)
                        .Select(x => context.Timer(customTrafficDelay).Select(_ => someTrafficValue))
                        .StartWith(context.Timer(noTrafficNotifyTime).Select(_ => noTrafficValue))
                        .Switch()
                        .Take(int.MaxValue)
                );

                res.Messages.AssertEqual(
                    OnNext(noTrafficNotifyTime.Ticks, noTrafficValue),
                    OnCompleted<int>(noTrafficNotifyTime.Ticks)
                );
            });
        }

        [TestMethod]
        public void Regressions_Traffic_StreamFires()
        {
            Run(client =>
            {
                var extraBuffer = TimeSpan.Zero;
                var notificationThreshold = TimeSpan.FromTicks(20);
                var travelTimeWithoutTraffic = TimeSpan.FromTicks(100);
                var startMonitoring = new DateTimeOffset(Subscribed + 50, TimeSpan.Zero);
                var eventStartTime = new DateTimeOffset(600, TimeSpan.Zero);
                var overhead = notificationThreshold + extraBuffer;
                var noTrafficNotifyTime = eventStartTime - travelTimeWithoutTraffic - overhead;
                var stopMonitoring = noTrafficNotifyTime + TimeSpan.Zero;
                var customTrafficDelay = travelTimeWithoutTraffic + TimeSpan.FromTicks(75);

                var context = GetContext(client);

                var noTrafficValue = 42;
                var someTrafficValue = 49;

                var res = client.Start(() =>
                    context.Timer(TimeSpan.Zero, TimeSpan.FromTicks(1000))
                        .DelaySubscription(startMonitoring)
                        .TakeUntil(stopMonitoring)
                        .Where(x => x == 0)
                        .Select(x => context.Timer(eventStartTime - customTrafficDelay - overhead).Select(_ => someTrafficValue))
                        .StartWith(context.Timer(noTrafficNotifyTime).Select(_ => noTrafficValue))
                        .Switch()
                        .Take(1)
                );

                res.Messages.AssertEqual(
                    OnNext((eventStartTime - customTrafficDelay - overhead).Ticks, someTrafficValue),
                    OnCompleted<int>((eventStartTime - customTrafficDelay - overhead).Ticks)
                );
            });
        }

        [TestMethod]
        public void Regressions_Traffic_StartWithFires_TakeUntilPast()
        {
            Run(client =>
            {
                var extraBuffer = TimeSpan.Zero;
                var notificationThreshold = TimeSpan.FromTicks(20);
                var travelTimeWithoutTraffic = TimeSpan.FromTicks(100);
                var initialTraceTimeWithTraffic = travelTimeWithoutTraffic + TimeSpan.FromTicks(50);
                var startMonitoring = new DateTimeOffset(Subscribed, TimeSpan.Zero);
                var eventStartTime = new DateTimeOffset(600, TimeSpan.Zero);
                var overhead = notificationThreshold + extraBuffer;
                var noTrafficNotifyTime = eventStartTime - travelTimeWithoutTraffic - overhead;
                var stopMonitoring = noTrafficNotifyTime + TimeSpan.Zero;
                var customTrafficDelay = travelTimeWithoutTraffic + TimeSpan.FromTicks(75);

                var context = GetContext(client);

                var noTrafficValue = 42;
                var someTrafficValue = 49;

                var res = client.Start(() =>
                    context.Timer(TimeSpan.FromTicks(1000), TimeSpan.FromTicks(1000))
                        .DelaySubscription(startMonitoring)
                        .TakeUntil(stopMonitoring)
                        .Where(x => x == 0)
                        .Select(x => context.Timer(customTrafficDelay).Select(_ => someTrafficValue))
                        .StartWith(context.Timer(noTrafficNotifyTime).Select(_ => noTrafficValue))
                        .Switch()
                        .Take(int.MaxValue)
                , 100, 700, 1000);

                res.Messages.AssertEqual(
                    OnNext(Increment(700, 1), noTrafficValue),
                    OnCompleted<int>(Increment(700, 1))
                );
            });
        }

        [TestMethod]
        public void Regressions_Traffic_StreamFires_TakeUntilPast()
        {
            Run(client =>
            {
                var extraBuffer = TimeSpan.Zero;
                var notificationThreshold = TimeSpan.FromTicks(20);
                var travelTimeWithoutTraffic = TimeSpan.FromTicks(100);
                var startMonitoring = new DateTimeOffset(Subscribed + 50, TimeSpan.Zero);
                var eventStartTime = new DateTimeOffset(600, TimeSpan.Zero);
                var overhead = notificationThreshold + extraBuffer;
                var noTrafficNotifyTime = eventStartTime - travelTimeWithoutTraffic - overhead;
                var stopMonitoring = noTrafficNotifyTime + TimeSpan.Zero;
                var customTrafficDelay = travelTimeWithoutTraffic + TimeSpan.FromTicks(75);

                var context = GetContext(client);

                var noTrafficValue = 42;
                var someTrafficValue = 49;

                var res = client.Start(() =>
                    context.Timer(TimeSpan.Zero, TimeSpan.FromTicks(1000))
                        .DelaySubscription(startMonitoring) // not req'd
                        .TakeUntil(stopMonitoring)
                        .Where(x => x == 0) // not req'd
                        .Select(x => context.Timer(eventStartTime - customTrafficDelay - overhead).Select(_ => someTrafficValue))
                        .StartWith(context.Timer(noTrafficNotifyTime).Select(_ => noTrafficValue))
                        .Switch()
                        .Take(1)
                , 100, 700, 1000);

                res.Messages.AssertEqual(
                    OnNext(Increment(700, 1), 42),
                    OnCompleted<int>(Increment(700, 1))
                );
            });
        }
    }
}
