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
    public partial class Distinct : TestBase
    {
        [TestMethod]
        public void Distinct_Environment_Never()
        {
            Run(client =>
            {
                var env = new TestExecutionEnvironment();

                var ctx = client.CreateContext(env);

                var xs = client.CreateHotObservable<int>();

                var res = client.Start(ctx, () =>
                    xs.Distinct(),
                    100, 200, 1000);

                res.Messages.AssertEqual(
                );

                Assert.AreEqual(0, env.ArtifactCount);
            });
        }

        [TestMethod]
        public void Distinct_Environment_Empty()
        {
            Run(client =>
            {
                var env = new TestExecutionEnvironment();

                var ctx = client.CreateContext(env);

                var xs = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnCompleted<int>(250)
                );

                var res = client.Start(ctx, () =>
                    xs.Distinct(),
                    100, 200, 1000);

                res.Messages.AssertEqual(
                    OnCompleted<int>(250)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );

                Assert.AreEqual(0, env.ArtifactCount);
            });
        }

        [TestMethod]
        public void Distinct_Environment_Return()
        {
            Run(client =>
            {
                var env = new TestExecutionEnvironment();

                var ctx = client.CreateContext(env);

                var xs = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnNext(220, 2),
                    OnCompleted<int>(250)
                );

                var res = client.Start(ctx, () =>
                    xs.Distinct(),
                    100, 200, 1000);

                res.Messages.AssertEqual(
                    OnNext(220, 2),
                    OnCompleted<int>(250)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );

                Assert.AreEqual(0, env.ArtifactCount);
            });
        }

        [TestMethod]
        public void Distinct_Environment_Throw()
        {
            Run(client =>
            {
                var env = new TestExecutionEnvironment();

                var ctx = client.CreateContext(env);

                var ex = new Exception();
                var xs = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnError<int>(250, ex)
                );

                var res = client.Start(ctx, () =>
                    xs.Distinct(),
                    100, 200, 1000);

                res.Messages.AssertEqual(
                    OnError<int>(250, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );

                Assert.AreEqual(0, env.ArtifactCount);
            });
        }

        [TestMethod]
        public void Distinct_Environment_AllChanges()
        {
            Run(client =>
            {
                var env = new TestExecutionEnvironment();

                var ctx = client.CreateContext(env);

                var xs = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnNext(210, 2),
                    OnNext(220, 3),
                    OnNext(230, 4),
                    OnNext(240, 5),
                    OnCompleted<int>(250)
                );

                var res = client.Start(ctx, () =>
                    xs.Distinct(),
                    100, 200, 1000);

                res.Messages.AssertEqual(
                    OnNext(210, 2),
                    OnNext(220, 3),
                    OnNext(230, 4),
                    OnNext(240, 5),
                    OnCompleted<int>(250)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );

                Assert.AreEqual(0, env.ArtifactCount);
            });
        }

        [TestMethod]
        public void Distinct_Environment_AllSame()
        {
            Run(client =>
            {
                var env = new TestExecutionEnvironment();

                var ctx = client.CreateContext(env);

                var xs = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnNext(210, 2),
                    OnNext(220, 2),
                    OnNext(230, 2),
                    OnNext(240, 2),
                    OnCompleted<int>(250)
                );

                var res = client.Start(ctx, () =>
                    xs.Distinct(),
                    100, 200, 1000);

                res.Messages.AssertEqual(
                    OnNext(210, 2),
                    OnCompleted<int>(250)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );

                Assert.AreEqual(0, env.ArtifactCount);
            });
        }

        [TestMethod]
        public void Distinct_Environment_SomeChanges()
        {
            Run(client =>
            {
                var env = new TestExecutionEnvironment();

                var ctx = client.CreateContext(env);

                var xs = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnNext(210, 2), //*
                    OnNext(215, 3), //*
                    OnNext(220, 3),
                    OnNext(225, 2),
                    OnNext(230, 2),
                    OnNext(235, 1), //*
                    OnNext(240, 2),
                    OnCompleted<int>(250)
                );

                var res = client.Start(ctx, () =>
                    xs.Distinct(),
                    100, 200, 1000);

                res.Messages.AssertEqual(
                    OnNext(210, 2),
                    OnNext(215, 3),
                    OnNext(235, 1),
                    OnCompleted<int>(250)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );

                Assert.AreEqual(0, env.ArtifactCount);
            });
        }

        [TestMethod]
        public void Distinct_Environment_KeySelector_Div2()
        {
            Run(client =>
            {
                var env = new TestExecutionEnvironment();

                var ctx = client.CreateContext(env);

                var xs = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnNext(210, 2), //*
                    OnNext(220, 4),
                    OnNext(230, 3), //*
                    OnNext(240, 5),
                    OnCompleted<int>(250)
                );

                var res = client.Start(ctx, () =>
                    xs.Distinct(x => x % 2),
                    100, 200, 1000);

                res.Messages.AssertEqual(
                    OnNext(210, 2),
                    OnNext(230, 3),
                    OnCompleted<int>(250)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );

                Assert.AreEqual(0, env.ArtifactCount);
            });
        }

        [TestMethod]
        public void Distinct_Settings_MaxDistinctElements()
        {
            Run(client =>
            {
                var env = new TestExecutionEnvironment();

                var ctx = client.CreateContext(env, settings: new Dictionary<string, object>
                {
                    { "rx://operators/distinct/settings/maxDistinctElements", 3 },
                });

                var xs = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnNext(210, 2),
                    OnNext(220, 3),
                    OnNext(230, 4),
                    OnNext(240, 5),
                    OnCompleted<int>(250)
                );

                var res = client.Start(ctx, () =>
                    xs.Distinct(),
                    100, 200, 1000);

                res.Messages.AssertEqual(
                    OnNext(210, 2),
                    OnNext(220, 3),
                    OnNext(230, 4),
                    OnError<int>(240, e => e is InvalidOperationException)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 240)
                );

                Assert.AreEqual(0, env.ArtifactCount);
            });
        }
    }
}
