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
    public partial class Any : OperatorTestBase
    {
        [TestMethod]
        public void Any_NoPredicate_Empty()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnCompleted<int>(50)
                );

                var res = client.Start(() =>
                    xs.Any()
                );

                res.Messages.AssertEqual(
                    OnNext<bool>(250, false),
                    OnCompleted<bool>(250)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void Any_NoPredicate_NonEmpty()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnNext<int>(10, 2),
                    OnNext<int>(20, 3),
                    OnNext<int>(30, 5),
                    OnNext<int>(40, 7),
                    OnCompleted<int>(50)
                );

                var res = client.Start(() =>
                    xs.Any()
                );

                res.Messages.AssertEqual(
                    OnNext<bool>(210, true),
                    OnCompleted<bool>(210)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 210)
                );
            });
        }

        [TestMethod]
        public void Any_NoPredicate_Throw()
        {
            Run(client =>
            {
                var ex = new Exception();

                var xs = client.CreateColdObservable(
                    OnError<int>(50, ex)
                );

                var res = client.Start(() =>
                    xs.Any()
                );

                res.Messages.AssertEqual(
                    OnError<bool>(250, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void Any_Simple_True()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnNext<int>(10, 2),
                    OnNext<int>(20, 3),
                    OnNext<int>(30, 5),
                    OnNext<int>(40, 7),
                    OnCompleted<int>(50)
                );

                var res = client.Start(() =>
                    xs.Any(x => x > 3)
                );

                res.Messages.AssertEqual(
                    OnNext<bool>(230, true),
                    OnCompleted<bool>(230)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 230)
                );
            });
        }

        [TestMethod]
        public void Any_Simple_False()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnNext<int>(10, 2),
                    OnNext<int>(20, 3),
                    OnNext<int>(30, 5),
                    OnNext<int>(40, 7),
                    OnCompleted<int>(50)
                );

                var res = client.Start(() =>
                    xs.Any(x => x > 10)
                );

                res.Messages.AssertEqual(
                    OnNext<bool>(250, false),
                    OnCompleted<bool>(250)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void Any_Empty()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnCompleted<int>(50)
                );

                var res = client.Start(() =>
                    xs.Any(x => x < 10)
                );

                res.Messages.AssertEqual(
                    OnNext<bool>(250, false),
                    OnCompleted<bool>(250)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void Any_Throw()
        {
            Run(client =>
            {
                var ex = new Exception();

                var xs = client.CreateColdObservable(
                    OnError<int>(50, ex)
                );

                var res = client.Start(() =>
                    xs.Any(x => x < 10)
                );

                res.Messages.AssertEqual(
                    OnError<bool>(250, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void Any_PredicateThrows()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnNext<int>(10, 2),
                    OnNext<int>(20, 3),
                    OnNext<int>(30, 0),
                    OnNext<int>(40, 7),
                    OnCompleted<int>(50)
                );

                var res = client.Start(() =>
                    xs.Any(x => 100 / x < 0)
                );

                res.Messages.AssertEqual(
                    OnError<bool>(230, ex => ex is DivideByZeroException)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 230)
                );
            });
        }
    }
}
