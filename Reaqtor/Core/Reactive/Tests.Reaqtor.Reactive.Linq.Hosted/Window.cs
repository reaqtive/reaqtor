// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Reaqtive;
using Reaqtive.Testing;
using Reaqtive.TestingFramework;

using Reaqtor.TestingFramework;

namespace Test.Reaqtor.Operators
{
    [TestClass]
    public partial class Window : TestBase
    {
        [TestMethod]
        public void Window_Environment_Count_Simple1()
        {
            Run(client =>
            {
                var env = new TestExecutionEnvironment();

                var ctx = client.CreateContext(env);

                var xs = client.CreateHotObservable<int>(
                    OnNext(210, 1),
                    OnNext(220, 2),
                    OnNext(230, 3),
                    OnNext(240, 4),
                    OnNext(250, 5),
                    OnNext(260, 6),
                    OnNext(270, 7),
                    OnNext(280, 8),
                    OnNext(290, 9),
                    OnCompleted<int>(300)
                );

                var res = client.Start(ctx, () =>
                    xs.Window(3).Skip(1).Take(1).Merge(),
                    100, 200, 1000);

                res.Messages.AssertEqual(
                    OnNext(240, 4),
                    OnNext(250, 5),
                    OnNext(260, 6),
                    OnCompleted<int>(260)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(201, 260)
                );

                Assert.AreEqual(0, env.ArtifactCount);
            });
        }

        [TestMethod]
        public void Window_Environment_Count_Simple2()
        {
            Run(client =>
            {
                var env = new TestExecutionEnvironment();

                var ctx = client.CreateContext(env);

                var xs = client.CreateHotObservable<int>(
                    OnNext(210, 1),
                    OnNext(220, 2),
                    OnNext(230, 3),
                    OnNext(240, 4),
                    OnNext(250, 5),
                    OnNext(260, 6),
                    OnNext(270, 7),
                    OnNext(280, 8),
                    OnNext(290, 9),
                    OnCompleted<int>(300)
                );

                var res = client.Start(ctx, () =>
                    xs.Window(3).Skip(2).Merge(),
                    100, 200, 1000);

                res.Messages.AssertEqual(
                    OnNext(270, 7),
                    OnNext(280, 8),
                    OnNext(290, 9),
                    OnCompleted<int>(300)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(201, 300)
                );

                Assert.AreEqual(0, env.ArtifactCount);
            });
        }

        [TestMethod]
        public void Window_Environment_Count_Dependencies()
        {
            Run(client =>
            {
                var env = new TestExecutionEnvironment();

                var ctx = client.CreateContext(env);

                var xs = client.CreateHotObservable<int>(
                    OnNext(210, 1),
                    OnNext(220, 2),
                    OnNext(230, 3),
                    OnNext(240, 4),
                    OnNext(250, 5),
                    OnNext(260, 6),
                    OnNext(270, 7),
                    OnNext(280, 8),
                    OnNext(290, 9),
                    OnCompleted<int>(300)
                );

                var s = default(ISubscription);

                client.ScheduleAbsolute(200, () =>
                {
                    s = xs.Window(3).Subscribe(Observer.Nop<ISubscribable<int>>());
                    new SubscriptionInitializeVisitor(s).Initialize(ctx);
                });

                foreach (var t in new[] { 215, 245, 275 })
                {
                    client.ScheduleAbsolute(t, () =>
                    {
                        var ds = default(IEnumerable<Uri>);

                        SubscriptionVisitor.Do<IDependencyOperator>(op =>
                        {
                            ds = op.Dependencies;
                        }).Apply(s);

                        Assert.IsNotNull(ds);
                        Assert.AreEqual(1, ds.Count(d => d.OriginalString.StartsWith("rx://tollbooth")));
                        Assert.AreEqual(1, ds.Count(d => d.OriginalString.StartsWith("rx://collector")));
                        Assert.AreEqual(1, ds.Count(d => d.OriginalString.StartsWith("rx://tunnel/window")));
                    });
                }

                client.Start();
            });
        }

        [TestMethod]
        public void Window_Environment_Count_Skip_Dependencies()
        {
            Run(client =>
            {
                var env = new TestExecutionEnvironment();

                var ctx = client.CreateContext(env);

                var xs = client.CreateHotObservable<int>(
                    OnNext(210, 1),
                    OnNext(220, 2),
                    OnNext(230, 3),
                    OnNext(240, 4),
                    OnNext(250, 5),
                    OnNext(260, 6),
                    OnNext(270, 7),
                    OnNext(280, 8),
                    OnNext(290, 9),
                    OnCompleted<int>(300)
                );

                var s = default(ISubscription);

                client.ScheduleAbsolute(200, () =>
                {
                    s = xs.Window(5, 2).Subscribe(Observer.Nop<ISubscribable<int>>());
                    new SubscriptionInitializeVisitor(s).Initialize(ctx);
                });

                client.ScheduleAbsolute(215, () =>
                {
                    var ds = default(IEnumerable<Uri>);

                    SubscriptionVisitor.Do<IDependencyOperator>(op =>
                    {
                        ds = op.Dependencies;
                    }).Apply(s);

                    Assert.IsNotNull(ds);
                    Assert.AreEqual(1, ds.Count(d => d.OriginalString.StartsWith("rx://tollbooth")));
                    Assert.AreEqual(1, ds.Count(d => d.OriginalString.StartsWith("rx://collector")));
                    Assert.AreEqual(1, ds.Count(d => d.OriginalString.StartsWith("rx://tunnel/window")));
                });

                foreach (var t in new[] { 235, 255, 275 })
                {
                    client.ScheduleAbsolute(t, () =>
                    {
                        var ds = default(IEnumerable<Uri>);

                        SubscriptionVisitor.Do<IDependencyOperator>(op =>
                        {
                            ds = op.Dependencies;
                        }).Apply(s);

                        Assert.IsNotNull(ds);
                        Assert.AreEqual(1, ds.Count(d => d.OriginalString.StartsWith("rx://tollbooth")));
                        Assert.AreEqual(1, ds.Count(d => d.OriginalString.StartsWith("rx://collector")));
                        Assert.AreEqual(2, ds.Count(d => d.OriginalString.StartsWith("rx://tunnel/window")));
                    });
                }

                client.Start();
            });
        }

        [TestMethod]
        public void Window_Environment_Time_Dependencies()
        {
            Run(client =>
            {
                var env = new TestExecutionEnvironment();

                var ctx = client.CreateContext(env);

                var xs = client.CreateHotObservable<int>(
                    OnNext(210, 1),
                    OnNext(220, 2),
                    OnNext(230, 3),
                    OnNext(240, 4),
                    OnNext(250, 5),
                    OnNext(260, 6),
                    OnNext(270, 7),
                    OnNext(280, 8),
                    OnNext(290, 9),
                    OnCompleted<int>(300)
                );

                var s = default(ISubscription);

                client.ScheduleAbsolute(200, () =>
                {
                    s = xs.Window(TimeSpan.FromTicks(30)).Subscribe(Observer.Nop<ISubscribable<int>>());
                    new SubscriptionInitializeVisitor(s).Initialize(ctx);
                });

                foreach (var t in new[] { 215, 245, 275 })
                {
                    client.ScheduleAbsolute(t, () =>
                    {
                        var ds = default(IEnumerable<Uri>);

                        SubscriptionVisitor.Do<IDependencyOperator>(op =>
                        {
                            ds = op.Dependencies;
                        }).Apply(s);

                        Assert.IsNotNull(ds);
                        Assert.AreEqual(1, ds.Count(d => d.OriginalString.StartsWith("rx://tollbooth")));
                        Assert.AreEqual(1, ds.Count(d => d.OriginalString.StartsWith("rx://collector")));
                        Assert.AreEqual(1, ds.Count(d => d.OriginalString.StartsWith("rx://tunnel/window")));
                    });
                }

                client.Start();
            });
        }

        [TestMethod]
        public void Window_Environment_Time_Shift_Dependencies()
        {
            Run(client =>
            {
                var env = new TestExecutionEnvironment();

                var ctx = client.CreateContext(env);

                var xs = client.CreateHotObservable<int>(
                    OnNext(210, 1),
                    OnNext(220, 2),
                    OnNext(230, 3),
                    OnNext(240, 4),
                    OnNext(250, 5),
                    OnNext(260, 6),
                    OnNext(270, 7),
                    OnNext(280, 8),
                    OnNext(290, 9),
                    OnCompleted<int>(300)
                );

                var s = default(ISubscription);

                client.ScheduleAbsolute(200, () =>
                {
                    s = xs.Window(TimeSpan.FromTicks(50), TimeSpan.FromTicks(20)).Subscribe(Observer.Nop<ISubscribable<int>>());
                    new SubscriptionInitializeVisitor(s).Initialize(ctx);
                });

                client.ScheduleAbsolute(215, () =>
                {
                    var ds = default(IEnumerable<Uri>);

                    SubscriptionVisitor.Do<IDependencyOperator>(op =>
                    {
                        ds = op.Dependencies;
                    }).Apply(s);

                    Assert.IsNotNull(ds);
                    Assert.AreEqual(1, ds.Count(d => d.OriginalString.StartsWith("rx://tollbooth")));
                    Assert.AreEqual(1, ds.Count(d => d.OriginalString.StartsWith("rx://collector")));
                    Assert.AreEqual(1, ds.Count(d => d.OriginalString.StartsWith("rx://tunnel/window")));
                });

                foreach (var t in new[] { 235, 255, 275 })
                {
                    client.ScheduleAbsolute(t, () =>
                    {
                        var ds = default(IEnumerable<Uri>);

                        SubscriptionVisitor.Do<IDependencyOperator>(op =>
                        {
                            ds = op.Dependencies;
                        }).Apply(s);

                        Assert.IsNotNull(ds);
                        Assert.AreEqual(1, ds.Count(d => d.OriginalString.StartsWith("rx://tollbooth")));
                        Assert.AreEqual(1, ds.Count(d => d.OriginalString.StartsWith("rx://collector")));
                        Assert.AreEqual(2, ds.Count(d => d.OriginalString.StartsWith("rx://tunnel/window")));
                    });
                }

                client.Start();
            });
        }

        [TestMethod]
        public void Window_Settings_MaxWindowCount()
        {
            Run(client =>
            {
                var env = new TestExecutionEnvironment();

                var ctx = client.CreateContext(env, settings: new Dictionary<string, object>
                {
                    { "rx://operators/window/settings/maxWindowCount", 5 },
                });

                var xs = client.CreateHotObservable<int>(
                );

                var res = client.Start(ctx, () =>
                    xs.Window(TimeSpan.FromTicks(10), TimeSpan.FromTicks(1)).Merge(),
                    100, 200, 1000);

                res.Messages.AssertEqual(
                    OnError<int>(205, ex => ex is InvalidOperationException)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(201, 205)
                );
            });
        }
    }
}
