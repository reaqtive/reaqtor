// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Reaqtive;
using Reaqtive.Testing;
using Reaqtive.TestingFramework;

namespace Test.Reaqtive.Operators
{
    [TestClass]
    public partial class SingleAsync : OperatorTestBase
    {
        #region SingleAsync

        [TestMethod]
        public void SingleAsync_ArgumentChecking()
        {
#pragma warning disable IDE0034 // Simplify 'default' expression (illustrative of method signature)
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.SingleAsync(default(ISubscribable<int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.SingleAsync(default(ISubscribable<int>), _ => true));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.SingleAsync(DummySubscribable<int>.Instance, default(Func<int, bool>)));
#pragma warning restore IDE0034 // Simplify 'default' expression
        }

        [TestMethod]
        public void SingleAsync_PredicateThrows()
        {
            Run(client =>
            {
                var ex = new Exception();

                var xs = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnNext(210, 2),
                    OnNext(220, 3),
                    OnNext(230, 4),
                    OnNext(240, 5),
                    OnCompleted<int>(250)
                );

                var res = client.Start(() =>
                    xs.SingleAsync(x => { if (x < 4) return false; throw ex; })
                );

                res.Messages.AssertEqual(
                    OnError<int>(230, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 230)
                );
            });
        }

        #endregion

        #region SingleOrDefaultAsync

        [TestMethod]
        public void SingleOrDefaultAsync_ArgumentChecking()
        {
#pragma warning disable IDE0034 // Simplify 'default' expression (illustrative of method signature)
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.SingleOrDefaultAsync(default(ISubscribable<int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.SingleOrDefaultAsync(default(ISubscribable<int>), _ => true));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.SingleOrDefaultAsync(DummySubscribable<int>.Instance, default(Func<int, bool>)));
#pragma warning restore IDE0034 // Simplify 'default' expression
        }

        [TestMethod]
        public void SingleOrDefaultAsync_Empty()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnCompleted<int>(250)
                );

                var res = client.Start(() =>
                    xs.SingleOrDefaultAsync()
                );

                res.Messages.AssertEqual(
                    OnNext(250, 0),
                    OnCompleted<int>(250)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void SingleOrDefaultAsync_One()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnNext(210, 2),
                    OnCompleted<int>(250)
                );

                var res = client.Start(() =>
                    xs.SingleOrDefaultAsync()
                );

                res.Messages.AssertEqual(
                    OnNext(250, 2),
                    OnCompleted<int>(250)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void SingleOrDefaultAsync_Many()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnNext(210, 2),
                    OnNext(220, 3),
                    OnCompleted<int>(250)
                );

                var res = client.Start(() =>
                    xs.SingleOrDefaultAsync()
                );

                res.Messages.AssertEqual(
                    OnError<int>(220, e => e is InvalidOperationException)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 220)
                );
            });
        }

        [TestMethod]
        public void SingleOrDefaultAsync_Error()
        {
            Run(client =>
            {
                var ex = new Exception();

                var xs = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnError<int>(210, ex)
                );

                var res = client.Start(() =>
                    xs.SingleOrDefaultAsync()
                );

                res.Messages.AssertEqual(
                    OnError<int>(210, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 210)
                );
            });
        }

        [TestMethod]
        public void SingleOrDefaultAsync_Predicate_One()
        {
            Run(client =>
            {
                var ex = new Exception();

                var xs = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnNext(210, 2),
                    OnNext(220, 3),
                    OnNext(230, 4),
                    OnNext(240, 5),
                    OnCompleted<int>(250)
                );

                var res = client.Start(() =>
                    xs.SingleOrDefaultAsync(x => x == 4)
                );

                res.Messages.AssertEqual(
                    OnNext(250, 4),
                    OnCompleted<int>(250)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void SingleOrDefaultAsync_Predicate_Many()
        {
            Run(client =>
            {
                var ex = new Exception();

                var xs = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnNext(210, 2),
                    OnNext(220, 3),
                    OnNext(230, 4),
                    OnNext(240, 5),
                    OnCompleted<int>(250)
                );

                var res = client.Start(() =>
                    xs.SingleOrDefaultAsync(x => x % 2 == 1)
                );

                res.Messages.AssertEqual(
                    OnError<int>(240, e => e is InvalidOperationException)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 240)
                );
            });
        }

        [TestMethod]
        public void SingleOrDefaultAsync_Predicate_None()
        {
            Run(client =>
            {
                var ex = new Exception();

                var xs = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnNext(210, 2),
                    OnNext(220, 3),
                    OnNext(230, 4),
                    OnNext(240, 5),
                    OnCompleted<int>(250)
                );

                var res = client.Start(() =>
                    xs.SingleOrDefaultAsync(x => x > 10)
                );

                res.Messages.AssertEqual(
                    OnNext(250, 0),
                    OnCompleted<int>(250)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void SingleOrDefaultAsync_Predicate_Throw()
        {
            Run(client =>
            {
                var ex = new Exception();

                var xs = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnNext(210, 2),
                    OnError<int>(220, ex)
                );

                var res = client.Start(() =>
                    xs.SingleOrDefaultAsync(x => x % 2 == 1)
                );

                res.Messages.AssertEqual(
                    OnError<int>(220, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 220)
                );
            });
        }

        [TestMethod]
        public void SingleOrDefaultAsync_PredicateThrows()
        {
            Run(client =>
            {
                var ex = new Exception();

                var xs = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnNext(210, 2),
                    OnNext(220, 3),
                    OnNext(230, 4),
                    OnNext(240, 5),
                    OnCompleted<int>(250)
                );

                var res = client.Start(() =>
                    xs.SingleOrDefaultAsync(x => { if (x < 4) return false; throw ex; })
                );

                res.Messages.AssertEqual(
                    OnError<int>(230, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 230)
                );
            });
        }

        #endregion
    }
}
