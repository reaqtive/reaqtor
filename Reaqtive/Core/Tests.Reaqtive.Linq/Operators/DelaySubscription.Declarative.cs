// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
#if GLITCHING
using System.Linq.Expressions;
using System.Reflection;
#endif

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
    public partial class DelaySubscription : OperatorTestBase
    {
        [TestMethod]
        public void DelaySubscription_TimeSpan_Simple_Cold()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnNext(50, 42),
                    OnNext(60, 43),
                    OnCompleted<int>(70)
                );

                var res = client.Start(() =>
                    xs.DelaySubscription(TimeSpan.FromTicks(30))
                );

                res.Messages.AssertEqual(
                    OnNext(280, 42),
                    OnNext(290, 43),
                    OnCompleted<int>(300)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(230, 300)
                );
            });
        }

        [TestMethod]
        public void DelaySubscription_TimeSpan_Error()
        {
            Run(client =>
            {
                var ex = new Exception();

                var xs = client.CreateColdObservable(
                    OnNext(50, 42),
                    OnNext(60, 43),
                    OnError<int>(70, ex)
                );

                var res = client.Start(() =>
                    xs.DelaySubscription(TimeSpan.FromTicks(30))
                );

                res.Messages.AssertEqual(
                    OnNext(280, 42),
                    OnNext(290, 43),
                    OnError<int>(300, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(230, 300)
                );
            });
        }

        [TestMethod]
        public void DelaySubscription_DateTimeOffset_Simple_Cold()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnNext(50, 42),
                    OnNext(60, 43),
                    OnCompleted<int>(70)
                );

                var res = client.Start(() =>
                    xs.DelaySubscription(new DateTimeOffset(30, TimeSpan.Zero))
                );

                res.Messages.AssertEqual(
                    OnNext(250, 42),
                    OnNext(260, 43),
                    OnCompleted<int>(270)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 270)
                );
            });
        }

        [TestMethod]
        public void DelaySubscription_DateTimeOffset_Simple_Hot()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(190, 40),
                    OnNext(220, 41),
                    OnNext(280, 42),
                    OnNext(290, 43),
                    OnCompleted<int>(300)
                );

                var res = client.Start(() =>
                    xs.DelaySubscription(new DateTimeOffset(230, TimeSpan.Zero))
                );

                res.Messages.AssertEqual(
                    OnNext(280, 42),
                    OnNext(290, 43),
                    OnCompleted<int>(300)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(230, 300)
                );
            });
        }

        [TestMethod]
        public void DelaySubscription_DateTimeOffset_Error()
        {
            Run(client =>
            {
                var ex = new Exception();

                var xs = client.CreateColdObservable(
                    OnNext(50, 42),
                    OnNext(60, 43),
                    OnError<int>(70, ex)
                );

                var res = client.Start(() =>
                    xs.DelaySubscription(new DateTimeOffset(30, TimeSpan.Zero))
                );

                res.Messages.AssertEqual(
                    OnNext(250, 42),
                    OnNext(260, 43),
                    OnError<int>(270, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 270)
                );
            });
        }

#if !GLITCHING
        [TestMethod]
        public void DelaySubscription_ExceptionAfterSubscribe()
        {
            Run(client =>
            {
                var ex = new InvalidOperationException();

                var xs = client.Exceptional<int>(() => { throw ex; }, false);

                var res = client.Start(() =>
                    xs.DelaySubscription(TimeSpan.FromTicks(30))
                );

                res.Messages.AssertEqual(
                    OnError<int>(Increment(200, 31), ex)
                );
            });
        }

        [TestMethod]
        public void DelaySubscription_ExceptionOnSubscribe()
        {
            Run(client =>
            {
                var ex = new InvalidOperationException();

                var xs = client.Exceptional<int>(() => { throw ex; }, true);

                var res = client.Start(() =>
                    xs.DelaySubscription(TimeSpan.FromTicks(30))
                );

                res.Messages.AssertEqual(
                    OnError<int>(Increment(200, 31), ex)
                );
            });
        }
#endif
    }

#if GLITCHING
    public partial class DelaySubscription
    {
        [TestMethod]
        public void DelaySubscriptionV2_TimeSpan_Simple_Cold()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnNext(50, 42),
                    OnNext(60, 43),
                    OnCompleted<int>(70)
                );

                var res = client.Start(() =>
                    xs.DelaySubscriptionV2(TimeSpan.FromTicks(30))
                );

                res.Messages.AssertEqual(
                    OnNext(280, 42),
                    OnNext(290, 43),
                    OnCompleted<int>(300)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(230, 300)
                );
            });
        }

        [TestMethod]
        public void DelaySubscriptionV2_TimeSpan_Error()
        {
            Run(client =>
            {
                var ex = new Exception();

                var xs = client.CreateColdObservable(
                    OnNext(50, 42),
                    OnNext(60, 43),
                    OnError<int>(70, ex)
                );

                var res = client.Start(() =>
                    xs.DelaySubscriptionV2(TimeSpan.FromTicks(30))
                );

                res.Messages.AssertEqual(
                    OnNext(280, 42),
                    OnNext(290, 43),
                    OnError<int>(300, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(230, 300)
                );
            });
        }

        [TestMethod]
        public void DelaySubscriptionV2_DateTimeOffset_Simple_Cold()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnNext(50, 42),
                    OnNext(60, 43),
                    OnCompleted<int>(70)
                );

                var res = client.Start(() =>
                    xs.DelaySubscriptionV2(new DateTimeOffset(30, TimeSpan.Zero))
                );

                res.Messages.AssertEqual(
                    OnNext(250, 42),
                    OnNext(260, 43),
                    OnCompleted<int>(270)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 270)
                );
            });
        }

        [TestMethod]
        public void DelaySubscriptionV2_DateTimeOffset_Simple_Hot()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(190, 40),
                    OnNext(220, 41),
                    OnNext(280, 42),
                    OnNext(290, 43),
                    OnCompleted<int>(300)
                );

                var res = client.Start(() =>
                    xs.DelaySubscriptionV2(new DateTimeOffset(230, TimeSpan.Zero))
                );

                res.Messages.AssertEqual(
                    OnNext(280, 42),
                    OnNext(290, 43),
                    OnCompleted<int>(300)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(230, 300)
                );
            });
        }

        [TestMethod]
        public void DelaySubscriptionV2_DateTimeOffset_Error()
        {
            Run(client =>
            {
                var ex = new Exception();

                var xs = client.CreateColdObservable(
                    OnNext(50, 42),
                    OnNext(60, 43),
                    OnError<int>(70, ex)
                );

                var res = client.Start(() =>
                    xs.DelaySubscriptionV2(new DateTimeOffset(30, TimeSpan.Zero))
                );

                res.Messages.AssertEqual(
                    OnNext(250, 42),
                    OnNext(260, 43),
                    OnError<int>(270, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 270)
                );
            });
        }
    }

    internal static class OperatorsV2
    {
        [KnownResource("rx://operators/delaySubscription/absoluteTime/v2")]
        public static IAsyncReactiveQbservable<TSource> DelaySubscriptionV2<TSource>(
            this IAsyncReactiveQbservable<TSource> source,
            DateTimeOffset dueTime)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.Provider.CreateQbservable<TSource>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
                    source.Expression,
                    Expression.Constant(dueTime)));
        }

        [KnownResource("rx://operators/delaySubscription/relativeTime/v2")]
        public static IAsyncReactiveQbservable<TSource> DelaySubscriptionV2<TSource>(
            this IAsyncReactiveQbservable<TSource> source,
            TimeSpan dueTime)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.Provider.CreateQbservable<TSource>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
                    source.Expression,
                    Expression.Constant(dueTime)));
        }
    }
#endif
}
