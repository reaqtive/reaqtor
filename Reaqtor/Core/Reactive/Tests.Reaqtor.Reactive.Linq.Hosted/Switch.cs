// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq.Expressions;

using Reaqtive;
using Reaqtive.Tasks;
using Reaqtive.Testing;
using Reaqtive.TestingFramework;

using Reaqtor.Reactive;
using Reaqtor.Reactive.Expressions;
using Reaqtor.TestingFramework;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.Reaqtor.Operators
{
    [TestClass]
    public partial class Switch : TestBase
    {
        [TestMethod]
        public void Switch_Environment_Simple()
        {
            Run(client =>
            {
                var ys1 = client.CreateHotObservable(
                    OnNext(200, 2),
                    OnNext(300, 3),
                    OnNext(400, 4),
                    OnCompleted<int>(500)
                );

                var qys1 = new QuotedSubscribable<int>(ys1, Expression.Constant(ys1, typeof(ISubscribable<int>)));

                var ys2 = client.CreateHotObservable(
                    OnNext(400, 5),
                    OnCompleted<int>(500)
                );

                var qys2 = new QuotedSubscribable<int>(ys2, Expression.Constant(ys2, typeof(ISubscribable<int>)));

                var xss = client.CreateHotObservable(
                    OnNext<ISubscribable<int>>(190, qys1),
                    OnNext<ISubscribable<int>>(350, qys2),
                    OnCompleted<ISubscribable<int>>(450)
                );

                var env = new TestExecutionEnvironment();

                var ctx = client.CreateContext(env);

                var res = client.Start(ctx, () =>
                    xss.Switch(),
                    100, 150, 1000
                );

                res.Messages.AssertEqual(
                    OnNext<int>(200, 2),
                    OnNext<int>(300, 3),
                    OnNext<int>(400, 5),
                    OnCompleted<int>(500)
                );

                xss.Subscriptions.AssertEqual(
                    Subscribe(150, 450)
                );
            });
        }

        [TestMethod]
        public void Switch_Environment_NonQuoted()
        {
            Run(client =>
            {
                var ys1 = client.CreateHotObservable(
                    OnNext(200, 2),
                    OnNext(300, 3),
                    OnNext(400, 4),
                    OnCompleted<int>(500)
                );

                var qys1 = new QuotedSubscribable<int>(ys1, Expression.Constant(ys1, typeof(ISubscribable<int>)));

                var ys2 = client.CreateHotObservable(
                    OnNext(400, 5),
                    OnCompleted<int>(500)
                );

                var xss = client.CreateHotObservable(
                    OnNext<ISubscribable<int>>(190, qys1),
                    OnNext<ISubscribable<int>>(350, ys2),
                    OnCompleted<ISubscribable<int>>(450)
                );

                var env = new TestExecutionEnvironment();

                var ctx = client.CreateContext(env);

                var res = client.Start(ctx, () =>
                    xss.Switch(),
                    100, 150, 1000
                );

                res.Messages.AssertEqual(
                    OnNext<int>(200, 2),
                    OnNext<int>(300, 3),
                    OnError<int>(350, ex => ex is HigherOrderSubscriptionFailedException)
                );
            });
        }

        [TestMethod]
        public void Switch_Environment_SaveAndReload1()
        {
            var ys1 = default(ISubscribable<int>);
            var ys2 = default(ISubscribable<int>);

            var qyss = new ISubscribable<int>[]
            {
                new QuotedSubscribable<int>(ys1, Expression.Invoke((Expression<Func<ISubscribable<int>>>)(() => ys1))),
                new QuotedSubscribable<int>(ys2, Expression.Invoke((Expression<Func<ISubscribable<int>>>)(() => ys2))),
            };

            var getQuery = FromContext(client =>
            {
                ys1 = client.CreateHotObservable(
                    OnNext(200, 2),
                    OnNext(300, 3),
                    OnNext(400, 4),
                    OnCompleted<int>(500)
                );

                ys2 = client.CreateHotObservable(
                    OnNext(400, 5),
                    OnCompleted<int>(500)
                );

                var xss = client.CreateHotObservable(
                    OnNext<ISubscribable<int>>(190, qyss[0]),
                    OnNext<ISubscribable<int>>(350, qyss[1]),
                    OnCompleted<ISubscribable<int>>(450)
                );

                return xss.Switch();
            });

            var state = default(IOperatorStateContainer);
            var env = new TestExecutionEnvironment();

            // Before checkpoint
            Run(client =>
            {
                client.Schedule(TimeSpan.FromTicks(259), new ActionTask(env.Freeze));

                state = client.CreateStateContainer();

                var query = getQuery(client);

                var ctx = client.CreateContext(env);

                var res = client.Start(ctx, () =>
                    query.Apply(client, OnSave(250, state)),
                    100, 150, 260
                );

                res.Messages.AssertEqual(
                    OnNext<int>(200, 2)
                );
            });

            // After checkpoint
            Run(client =>
            {
                client.Schedule(TimeSpan.FromTicks(259), new ActionTask(env.Defrost));

                var query = getQuery(client);

                var ctx = client.CreateContext(env);

                var res = client.Start(ctx, () =>
                    query,
                    100, 260, 1000, state
                );

                res.Messages.AssertEqual(
                    OnNext<int>(300, 3),
                    OnNext<int>(400, 5),
                    OnCompleted<int>(500)
                );
            });
        }

        [TestMethod]
        public void Switch_Environment_SaveAndReload2()
        {
            var ys1 = default(ISubscribable<int>);
            var ys2 = default(ISubscribable<int>);

            var qyss = new ISubscribable<int>[]
            {
                new QuotedSubscribable<int>(ys1, Expression.Invoke((Expression<Func<ISubscribable<int>>>)(() => ys1))),
                new QuotedSubscribable<int>(ys2, Expression.Invoke((Expression<Func<ISubscribable<int>>>)(() => ys2))),
            };

            var getQuery = FromContext(client =>
            {
                ys1 = client.CreateHotObservable(
                    OnNext(200, 2),
                    OnNext(300, 3),
                    OnNext(400, 4),
                    OnCompleted<int>(500)
                );

                ys2 = client.CreateHotObservable(
                    OnNext(400, 5),
                    OnCompleted<int>(500)
                );

                var xss = client.CreateHotObservable(
                    OnNext<ISubscribable<int>>(190, qyss[0]),
                    OnNext<ISubscribable<int>>(350, qyss[1]),
                    OnCompleted<ISubscribable<int>>(250)
                );

                return xss.Switch();
            });

            var state = default(IOperatorStateContainer);
            var env = new TestExecutionEnvironment();

            // Before checkpoint
            Run(client =>
            {
                client.Schedule(TimeSpan.FromTicks(259), new ActionTask(env.Freeze));

                state = client.CreateStateContainer();

                var query = getQuery(client);

                var ctx = client.CreateContext(env);

                var res = client.Start(ctx, () =>
                    query.Apply(client, OnSave(255, state)),
                    100, 150, 260
                );

                res.Messages.AssertEqual(
                    OnNext<int>(200, 2)
                );
            });

            // After checkpoint
            Run(client =>
            {
                client.Schedule(TimeSpan.FromTicks(259), new ActionTask(env.Defrost));

                var query = getQuery(client);

                var ctx = client.CreateContext(env);

                var res = client.Start(ctx, () =>
                    query,
                    100, 260, 1000, state
                );

                res.Messages.AssertEqual(
                    OnNext<int>(300, 3),
                    OnNext<int>(400, 5),
                    OnCompleted<int>(500)
                );
            });
        }
    }
}
