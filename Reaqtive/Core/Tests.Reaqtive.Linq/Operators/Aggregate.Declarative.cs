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
    public partial class Aggregate : OperatorTestBase
    {
        [TestMethod]
        public void Aggregate_Simple1()
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
                    xs.Aggregate((l, r) => l * r)
                );

                res.Messages.AssertEqual(
                    OnNext<int>(250, 2 * 3 * 5 * 7),
                    OnCompleted<int>(250)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void Aggregate_Simple2()
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
                    xs.Aggregate(1, (l, r) => l * r)
                );

                res.Messages.AssertEqual(
                    OnNext<int>(250, 2 * 3 * 5 * 7),
                    OnCompleted<int>(250)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void Aggregate_Simple3()
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
                    xs.Aggregate(1, (l, r) => l * r, p => p.ToString())
                );

                res.Messages.AssertEqual(
                    OnNext<string>(250, (2 * 3 * 5 * 7).ToString()),
                    OnCompleted<string>(250)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void Aggregate_Throw1()
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
                    xs.Aggregate((l, r) => l * r)
                );

                res.Messages.AssertEqual(
                    OnError<int>(250, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void Aggregate_Throw2()
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
                    xs.Aggregate(1, (l, r) => l * r)
                );

                res.Messages.AssertEqual(
                    OnError<int>(250, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void Aggregate_Throw3()
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
                    xs.Aggregate(1, (l, r) => l * r, p => p.ToString())
                );

                res.Messages.AssertEqual(
                    OnError<string>(250, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void Aggregate_Empty1()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnCompleted<int>(50)
                );

                var res = client.Start(() =>
                    xs.Aggregate((l, r) => l * r)
                );

                res.Messages.AssertEqual(
                    OnError<int>(250, ex => ex is InvalidOperationException)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void Aggregate_Empty2()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnCompleted<int>(50)
                );

                var res = client.Start(() =>
                    xs.Aggregate(1, (l, r) => l * r)
                );

                res.Messages.AssertEqual(
                    OnNext<int>(250, 1),
                    OnCompleted<int>(250)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void Aggregate_Empty3()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnCompleted<int>(50)
                );

                var res = client.Start(() =>
                    xs.Aggregate(1, (l, r) => l * r, p => p.ToString())
                );

                res.Messages.AssertEqual(
                    OnNext<string>(250, "1"),
                    OnCompleted<string>(250)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void Aggregate_AggregatorThrows()
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
                    xs.Aggregate((l, r) => l / r)
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
        public void Aggregate_AccumulatorThrows1()
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
                    xs.Aggregate(42, (l, r) => l / r)
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
        public void Aggregate_ResultSelectorThrows()
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
#pragma warning disable IDE0079 // Remove unnecessary suppression (only on .NET 5.0)
#pragma warning disable IDE0057 // Use range operator (declarative tests are also used in remoting and captured as expression trees)
                    xs.Aggregate(1, (l, r) => l * r, p => p.ToString().Substring(99))
#pragma warning restore IDE0057
#pragma warning restore IDE0079
                );

                res.Messages.AssertEqual(
                    OnError<string>(250, ex => ex is ArgumentOutOfRangeException)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }
    }
}
