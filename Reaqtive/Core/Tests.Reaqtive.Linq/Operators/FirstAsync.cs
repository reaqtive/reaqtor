// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

using Reaqtive;
using Reaqtive.Testing;
using Reaqtive.TestingFramework;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.Reaqtive.Operators
{
    [TestClass]
    public partial class FirstAsync : OperatorTestBase
    {
        #region FirstAsync

        [TestMethod]
        public void FirstAsync_ArgumentChecking()
        {
#pragma warning disable IDE0034 // Simplify 'default' expression (illustrative of method signature)
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.FirstAsync(default(ISubscribable<int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.FirstAsync(default(ISubscribable<int>), _ => true));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.FirstAsync(DummySubscribable<int>.Instance, default(Func<int, bool>)));
#pragma warning restore IDE0034 // Simplify 'default' expression
        }

        [TestMethod]
        public void FirstAsync_PredicateThrows()
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
                    xs.FirstAsync(x => { if (x < 4) return false; throw ex; })
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

        #region FirstOrDefaultAsync

        [TestMethod]
        public void FirstOrDefaultAsync_ArgumentChecking()
        {
#pragma warning disable IDE0034 // Simplify 'default' expression (illustrative of method signature)
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.FirstOrDefaultAsync(default(ISubscribable<int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.FirstOrDefaultAsync(default(ISubscribable<int>), _ => true));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.FirstOrDefaultAsync(DummySubscribable<int>.Instance, default(Func<int, bool>)));
#pragma warning restore IDE0034 // Simplify 'default' expression
        }

        [TestMethod]
        public void FirstOrDefaultAsync_Empty()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnCompleted<int>(250)
                );

                var res = client.Start(() =>
                    xs.FirstOrDefaultAsync()
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
        public void FirstOrDefaultAsync_One()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnNext(210, 2),
                    OnCompleted<int>(250)
                );

                var res = client.Start(() =>
                    xs.FirstOrDefaultAsync()
                );

                res.Messages.AssertEqual(
                    OnNext(210, 2),
                    OnCompleted<int>(210)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 210)
                );
            });
        }

        [TestMethod]
        public void FirstOrDefaultAsync_Many()
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
                    xs.FirstOrDefaultAsync()
                );

                res.Messages.AssertEqual(
                    OnNext(210, 2),
                    OnCompleted<int>(210)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 210)
                );
            });
        }

        [TestMethod]
        public void FirstOrDefaultAsync_Error()
        {
            Run(client =>
            {
                var ex = new Exception();

                var xs = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnError<int>(210, ex)
                );

                var res = client.Start(() =>
                    xs.FirstOrDefaultAsync()
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
        public void FirstOrDefaultAsync_Predicate()
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
                    xs.FirstOrDefaultAsync(x => x % 2 == 1)
                );

                res.Messages.AssertEqual(
                    OnNext(220, 3),
                    OnCompleted<int>(220)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 220)
                );
            });
        }

        [TestMethod]
        public void FirstOrDefaultAsync_Predicate_None()
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
                    xs.FirstOrDefaultAsync(x => x > 10)
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
        public void FirstOrDefaultAsync_Predicate_Throw()
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
                    xs.FirstOrDefaultAsync(x => x % 2 == 1)
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
        public void FirstOrDefaultAsync_PredicateThrows()
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
                    xs.FirstOrDefaultAsync(x => { if (x < 4) return false; throw ex; })
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
