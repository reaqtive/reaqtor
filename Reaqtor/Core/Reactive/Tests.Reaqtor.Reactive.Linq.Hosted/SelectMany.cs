// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq.Expressions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Reaqtive;
using Reaqtive.Tasks;
using Reaqtive.Testing;
using Reaqtive.TestingFramework;

using Reaqtor.Reactive;
using Reaqtor.Reactive.Expressions;
using Reaqtor.TestingFramework;

namespace Test.Reaqtor.Operators
{
    [TestClass]
    public partial class SelectMany : TestBase
    {
        [TestMethod]
        public void SelectMany_Environment_NonQuoted()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(200, 2)
                );

                var ys = client.CreateColdObservable(
                    OnNext(10, "bar")
                );

                var env = new TestExecutionEnvironment();

                var ctx = client.CreateContext(env);

                var res = client.Start(ctx, () =>
                    from x in xs
                    from y in ys
                    select x + " - " + y,
                    100, 150, 1000
                );

                res.Messages.AssertEqual(
                    OnError<string>(200, ex => ex is HigherOrderSubscriptionFailedException)
                );
            });
        }

        [TestMethod]
        public void SelectMany_Environment_BridgeSubscribeFailure()
        {
            Run(client =>
            {
                var err = new Exception();

                var xs = client.CreateHotObservable(
                    OnNext(200, 2)
                );

                var ys = client.CreateColdObservable(
                    OnNext(10, "bar")
                );

                var qys = new QuotedSubscribable<string>(ys, Expression.Constant(ys, typeof(ISubscribable<string>)));

                var env = new TestExecutionEnvironment() { BridgeSubscriptionError = err };

                var ctx = client.CreateContext(env);

                var res = client.Start(ctx, () =>
                    from x in xs
                    from y in qys
                    select x + " - " + y,
                    100, 150, 1000
                );

                res.Messages.AssertEqual(
                    OnError<string>(200, ex => ex is HigherOrderSubscriptionFailedException && ex.InnerException == err)
                );
            });
        }

        [TestMethod]
        public void SelectMany_Environment_SaveAndReload1()
        {
            Run(client =>
            {
                var state = client.CreateStateContainer();

                var checkpoints = new[] {
                    OnSave(250, state),
                    OnLoad(290, state),
                };

                var xs = client.CreateHotObservable(
                    OnNext(200, 2),
                    OnNext(300, 3),
                    OnNext(400, 5),
                    OnCompleted<int>(500)
                );

                var ys = client.CreateColdObservable(
                    OnNext(10, "bar"),
                    OnNext(20, "foo"),
                    OnCompleted<string>(30)
                );

                var qys = new QuotedSubscribable<string>(ys, Expression.Constant(ys, typeof(ISubscribable<string>)));

                var env = new TestExecutionEnvironment();

                var ctx = client.CreateContext(env);

                var res = client.Start(ctx, () =>
                    (from x in xs
                     from y in qys
                     select x + " - " + y)
                    .Apply(client, checkpoints),
                    100, 150, 1000
                );

                res.Messages.AssertEqual(
                    OnNext<string>(210, "2 - bar"),
                    OnNext<string>(220, "2 - foo"),
                    OnNext<string>(310, "3 - bar"),
                    OnNext<string>(320, "3 - foo"),
                    OnNext<string>(410, "5 - bar"),
                    OnNext<string>(420, "5 - foo"),
                    OnCompleted<string>(500)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(150, 500)
                );
            });
        }

        [TestMethod]
        public void SelectMany_Environment_SaveAndReload2()
        {
            var ys2 = default(ISubscribable<string>);
            var ys3 = default(ISubscribable<string>);
            var ys5 = default(ISubscribable<string>);

            var qyss = new Dictionary<int, ISubscribable<string>>
            {
#pragma warning disable IDE0004 // Remove Unnecessary Cast. (Only unnecessary on C# 10 or later.)
                { 2, new QuotedSubscribable<string>(ys2, Expression.Invoke((Expression<Func<ISubscribable<string>>>)(() => ys2))) },
                { 3, new QuotedSubscribable<string>(ys3, Expression.Invoke((Expression<Func<ISubscribable<string>>>)(() => ys3))) },
                { 5, new QuotedSubscribable<string>(ys5, Expression.Invoke((Expression<Func<ISubscribable<string>>>)(() => ys5))) },
#pragma warning restore IDE0004 // Remove Unnecessary Cast
            };

            var getQuery = FromContext(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(200, 2),
                    OnNext(300, 3),
                    OnNext(400, 5),
                    OnCompleted<int>(500)
                );

                ys2 = client.CreateHotObservable(
                    OnNext(210, "bar"),
                    OnNext(220, "foo"),
                    OnCompleted<string>(230)
                );

                ys3 = client.CreateHotObservable(
                    OnNext(310, "qux"),
                    OnNext(320, "baz"),
                    OnCompleted<string>(330)
                );

                ys5 = client.CreateHotObservable(
                    OnNext(410, "corge"),
                    OnNext(420, "grault"),
                    OnCompleted<string>(430)
                );

                var query = from x in xs
                            from y in qyss[x]
                            select x + " - " + y;

                return query;
            });

            var state = default(IOperatorStateContainer);
            var env = new TestExecutionEnvironment();

            // Before checkpoint
            Run(client =>
            {
                client.Schedule(TimeSpan.FromTicks(324), new ActionTask(env.Freeze));

                state = client.CreateStateContainer();

                var query = getQuery(client);

                var ctx = client.CreateContext(env);

                var res = client.Start(ctx, () =>
                    query.Apply(client, OnSave(315, state)),
                    100, 150, 325
                );

                res.Messages.AssertEqual(
                    OnNext<string>(210, "2 - bar"),
                    OnNext<string>(220, "2 - foo"),
                    OnNext<string>(310, "3 - qux"),
                    OnNext<string>(320, "3 - baz")
                );
            });

            // After checkpoint
            Run(client =>
            {
                client.Schedule(TimeSpan.FromTicks(324), new ActionTask(env.Defrost));

                var query = getQuery(client);

                var ctx = client.CreateContext(env);

                var res = client.Start(ctx, () =>
                    query,
                    100, 325, 1000, state
                );

                res.Messages.AssertEqual(
                    OnNext<string>(410, "5 - corge"),
                    OnNext<string>(420, "5 - grault"),
                    OnCompleted<string>(500)
                );
            });
        }

        [TestMethod]
        public void SelectMany_Settings_MaxConcurrentInnerSubscriptionCount()
        {
            Run(client =>
            {
                var env = new TestExecutionEnvironment();

                var ctx = client.CreateContext(env, settings: new Dictionary<string, object>
                {
                    { "rx://operators/bind/settings/maxConcurrentInnerSubscriptionCount", 5 },
                });

                var xs = client.CreateHotObservable<int>(
                    OnNext(210, 1),
                    OnNext(220, 2),
                    OnNext(230, 3),
                    OnNext(240, 4),
                    OnNext(250, 5),
                    OnNext(260, 6)
                );

                var ys = client.CreateColdObservable<int>(
                );

                var qys = new QuotedSubscribable<int>(ys, Expression.Constant(ys, typeof(ISubscribable<int>)));

                var res = client.Start(ctx, () =>
                    xs.SelectMany(x => qys),
                    100, 200, 1000);

                res.Messages.AssertEqual(
                    OnError<int>(260, ex => ex is InvalidOperationException)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 260)
                );
            });
        }
    }
}
