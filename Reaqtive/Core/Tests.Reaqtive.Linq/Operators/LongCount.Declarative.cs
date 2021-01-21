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
    public partial class LongCount : OperatorTestBase
    {
        [TestMethod]
        public void LongCount_NoPredicate_Empty()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnCompleted<int>(50)
                );

                var res = client.Start(() =>
                    xs.LongCount()
                );

                res.Messages.AssertEqual(
                    OnNext<long>(250, 0),
                    OnCompleted<long>(250)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void LongCount_NoPredicate_NonEmpty()
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
                    xs.LongCount()
                );

                res.Messages.AssertEqual(
                    OnNext<long>(250, 4),
                    OnCompleted<long>(250)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void LongCount_NoPredicate_Throw()
        {
            Run(client =>
            {
                var ex = new Exception();

                var xs = client.CreateColdObservable(
                    OnError<int>(50, ex)
                );

                var res = client.Start(() =>
                    xs.LongCount()
                );

                res.Messages.AssertEqual(
                    OnError<long>(250, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void LongCount_Predicate_Some()
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
                    xs.LongCount(x => x % 3 == 2)
                );

                res.Messages.AssertEqual(
                    OnNext<long>(250, 2),
                    OnCompleted<long>(250)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void LongCount_Predicate_None()
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
                    xs.LongCount(x => x % 3 == 3)
                );

                res.Messages.AssertEqual(
                    OnNext<long>(250, 0),
                    OnCompleted<long>(250)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void LongCount_PredicateThrows()
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
                    xs.LongCount(x => 100 / x < 0)
                );

                res.Messages.AssertEqual(
                    OnError<long>(230, ex => ex is DivideByZeroException)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 230)
                );
            });
        }
    }
}
