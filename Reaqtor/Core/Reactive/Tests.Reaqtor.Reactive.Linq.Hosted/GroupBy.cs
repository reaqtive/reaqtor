// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;

using Reaqtive;
using Reaqtive.Testing;
using Reaqtive.TestingFramework;

using Reaqtor.TestingFramework;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.Reaqtor.Operators
{
    [TestClass]
    public partial class GroupBy : TestBase
    {
        [TestMethod]
        public void GroupBy_Environment_Simple1()
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
                    xs.GroupBy(x => x % 2).Merge(),
                    100, 200, 1000);

                res.Messages.AssertEqual(
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

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 300)
                );

                Assert.AreEqual(0, env.ArtifactCount);
            });
        }

        [TestMethod]
        public void GroupBy_Environment_Simple2()
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
                    xs.GroupBy(x => x % 2).Take(1).Merge(),
                    100, 200, 1000);

                res.Messages.AssertEqual(
                    OnNext(210, 1),
                    OnNext(230, 3),
                    OnNext(250, 5),
                    OnNext(270, 7),
                    OnNext(290, 9),
                    OnCompleted<int>(300)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 300)
                );

                Assert.AreEqual(0, env.ArtifactCount);
            });
        }

        [TestMethod]
        public void GroupBy_Environment_Simple3()
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
                    xs.GroupBy(x => x % 2).Skip(1).Take(1).Merge(),
                    100, 200, 1000);

                res.Messages.AssertEqual(
                    OnNext(220, 2),
                    OnNext(240, 4),
                    OnNext(260, 6),
                    OnNext(280, 8),
                    OnCompleted<int>(300)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 300)
                );

                Assert.AreEqual(0, env.ArtifactCount);
            });
        }

        [TestMethod]
        public void GroupBy_Environment_Dependencies()
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
                    s = xs.GroupBy(x => x % 2).Subscribe(Observer.Nop<ISubscribable<int>>());
                    new SubscriptionInitializeVisitor(s).Initialize(ctx);
                });

                client.ScheduleAbsolute(205, () =>
                {
                    var ds = default(IEnumerable<Uri>);

                    SubscriptionVisitor.Do<IDependencyOperator>(op =>
                    {
                        ds = op.Dependencies;
                    }).Apply(s);

                    Assert.IsNotNull(ds);
                    Assert.AreEqual(1, ds.Count(d => d.OriginalString.StartsWith("rx://tollbooth")));
                    Assert.AreEqual(1, ds.Count(d => d.OriginalString.StartsWith("rx://collector")));
                    Assert.AreEqual(0, ds.Count(d => d.OriginalString.StartsWith("rx://tunnel/group")));
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
                    Assert.AreEqual(1, ds.Count(d => d.OriginalString.StartsWith("rx://tunnel/group")));
                });

                for (var t = 225; t < 300; t += 10)
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
                        Assert.AreEqual(2, ds.Count(d => d.OriginalString.StartsWith("rx://tunnel/group")));
                    });
                }

                client.Start();
            });
        }

        [TestMethod]
        public void GroupBy_Settings_MaxWindowCount()
        {
            Run(client =>
            {
                var env = new TestExecutionEnvironment();

                var ctx = client.CreateContext(env, settings: new Dictionary<string, object>
                {
                    { "rx://operators/groupBy/settings/maxGroupCount", 5 },
                });

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
                    xs.GroupBy(x => x).Merge(),
                    100, 200, 1000);

                res.Messages.AssertEqual(
                    OnNext(210, 1),
                    OnNext(220, 2),
                    OnNext(230, 3),
                    OnNext(240, 4),
                    OnNext(250, 5),
                    OnError<int>(260, ex => ex is InvalidOperationException)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 260)
                );
            });
        }
    }
}
