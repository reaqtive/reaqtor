// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Linq.Expressions;

using Reaqtive;
using Reaqtive.Testing;
using Reaqtive.TestingFramework;

using Reaqtor.Reactive;
using Reaqtor.Reactive.Expressions;
using Reaqtor.TestingFramework;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.Reaqtor.Operators
{
    [TestClass]
    public partial class Throttle : TestBase
    {
        [TestMethod]
        public void Throttle_Environment_Simple()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(200, 2),
                    OnNext(300, 3),
                    OnNext(400, 5),
                    OnCompleted<int>(500)
                );

                var ys = client.CreateColdObservable(
                    OnNext(50, "bar"),
                    OnCompleted<string>(70)
                );

                var qys = new QuotedSubscribable<string>(ys, Expression.Constant(ys, typeof(ISubscribable<string>)));

                var env = new TestExecutionEnvironment();

                var ctx = client.CreateContext(env);

                var res = client.Start(ctx, () =>
                    xs.Throttle(_ => qys),
                    100, 150, 1000
                );

                res.Messages.AssertEqual(
                    OnNext<int>(250, 2),
                    OnNext<int>(350, 3),
                    OnNext<int>(450, 5),
                    OnCompleted<int>(500)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(150, 500)
                );
            });
        }

        [TestMethod]
        public void Throttle_Environment_NonQuoted()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(200, 2)
                );

                var ys = client.CreateColdObservable(
                    OnNext(50, "bar")
                );

                var env = new TestExecutionEnvironment();

                var ctx = client.CreateContext(env);

                var res = client.Start(ctx, () =>
                    xs.Throttle(_ => ys),
                    100, 150, 1000
                );

                res.Messages.AssertEqual(
                    OnError<int>(200, ex => ex is HigherOrderSubscriptionFailedException)
                );
            });
        }

        [TestMethod]
        public void Throttle_Environment_SaveAndReload1()
        {
            Run(client =>
            {
                var state = client.CreateStateContainer();

                var checkpoints = new[] {
                    OnSave(260, state),
                    OnLoad(290, state),
                };

                var xs = client.CreateHotObservable(
                    OnNext(200, 2),
                    OnNext(300, 3),
                    OnNext(400, 5),
                    OnCompleted<int>(500)
                );

                var ys = client.CreateColdObservable(
                    OnNext(50, "bar"),
                    OnCompleted<string>(70)
                );

                var qys = new QuotedSubscribable<string>(ys, Expression.Constant(ys, typeof(ISubscribable<string>)));

                var env = new TestExecutionEnvironment();

                var ctx = client.CreateContext(env);

                var res = client.Start(ctx, () =>
                    xs.Throttle(_ => qys).Apply(client, checkpoints),
                    100, 150, 1000
                );

                res.Messages.AssertEqual(
                    OnNext<int>(250, 2),
                    OnNext<int>(350, 3),
                    OnNext<int>(450, 5),
                    OnCompleted<int>(500)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(150, 500)
                );
            });
        }
    }
}
