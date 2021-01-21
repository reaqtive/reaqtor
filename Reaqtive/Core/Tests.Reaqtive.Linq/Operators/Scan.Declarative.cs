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
    public partial class Scan : OperatorTestBase
    {
        [TestMethod]
        public void Scan_Simple1()
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
                    xs.Scan((l, r) => l * r)
                );

                res.Messages.AssertEqual(
                    OnNext<int>(220, 2 * 3),
                    OnNext<int>(230, 2 * 3 * 5),
                    OnNext<int>(240, 2 * 3 * 5 * 7),
                    OnCompleted<int>(250)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void Scan_Simple2()
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
                    xs.Scan(1, (l, r) => l * r)
                );

                res.Messages.AssertEqual(
                    OnNext<int>(210, 1 * 2),
                    OnNext<int>(220, 1 * 2 * 3),
                    OnNext<int>(230, 1 * 2 * 3 * 5),
                    OnNext<int>(240, 1 * 2 * 3 * 5 * 7),
                    OnCompleted<int>(250)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void Scan_Throw1()
        {
            Run(client =>
            {
                var ex = new Exception();

                var xs = client.CreateColdObservable(
                    OnNext<int>(10, 2),
                    OnNext<int>(20, 3),
                    OnNext<int>(30, 5),
                    OnNext<int>(40, 7),
                    OnError<int>(50, ex)
                );

                var res = client.Start(() =>
                    xs.Scan((l, r) => l * r)
                );

                res.Messages.AssertEqual(
                    OnNext<int>(220, 2 * 3),
                    OnNext<int>(230, 2 * 3 * 5),
                    OnNext<int>(240, 2 * 3 * 5 * 7),
                    OnError<int>(250, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void Scan_Throw2()
        {
            Run(client =>
            {
                var ex = new Exception();

                var xs = client.CreateColdObservable(
                    OnNext<int>(10, 2),
                    OnNext<int>(20, 3),
                    OnNext<int>(30, 5),
                    OnNext<int>(40, 7),
                    OnError<int>(50, ex)
                );

                var res = client.Start(() =>
                    xs.Scan(1, (l, r) => l * r)
                );

                res.Messages.AssertEqual(
                    OnNext<int>(210, 1 * 2),
                    OnNext<int>(220, 1 * 2 * 3),
                    OnNext<int>(230, 1 * 2 * 3 * 5),
                    OnNext<int>(240, 1 * 2 * 3 * 5 * 7),
                    OnError<int>(250, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void Scan_Empty1()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnCompleted<int>(50)
                );

                var res = client.Start(() =>
                    xs.Scan((l, r) => l * r)
                );

                res.Messages.AssertEqual(
                    OnCompleted<int>(250)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void Scan_Empty2()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnCompleted<int>(50)
                );

                var res = client.Start(() =>
                    xs.Scan(1, (l, r) => l * r)
                );

                res.Messages.AssertEqual(
                    OnCompleted<int>(250)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void Scan_AggregatorThrows()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnNext<int>(10, 2),
                    OnNext<int>(20, 0),
                    OnNext<int>(30, 1),
                    OnCompleted<int>(40)
                );

                var res = client.Start(() =>
                    xs.Scan((l, r) => l / r)
                );

                res.Messages.AssertEqual(
                    OnError<int>(220, ex => ex is DivideByZeroException)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 220)
                );
            });
        }

        [TestMethod]
        public void Scan_AccumulatorThrows1()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnNext<int>(10, 2),
                    OnNext<int>(20, 0),
                    OnNext<int>(30, 1),
                    OnCompleted<int>(40)
                );

                var res = client.Start(() =>
                    xs.Scan(42, (l, r) => l / r)
                );

                res.Messages.AssertEqual(
                    OnNext(210, 42 / 2),
                    OnError<int>(220, ex => ex is DivideByZeroException)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 220)
                );
            });
        }
    }
}
