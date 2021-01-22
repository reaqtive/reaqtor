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
    public partial class Sum : OperatorTestBase
    {
        [TestMethod]
        public void SumInt32_Overflow1()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnNext<Int32>(10, 42),
                    OnNext<Int32>(20, Int32.MaxValue - 100),
                    OnNext<Int32>(30, 100 - 42 + 1),
                    OnNext<Int32>(40, -17),
                    OnCompleted<Int32>(50)
                );

                var res = client.Start(() =>
                    xs.Sum()
                );

                res.Messages.AssertEqual(
                    OnError<Int32>(230, e => e is OverflowException)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 230)
                );
            });
        }

        [TestMethod]
        public void SumInt32_Overflow2()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnNext<Int32>(10, -42),
                    OnNext<Int32>(20, Int32.MinValue + 100),
                    OnNext<Int32>(30, -(100 - 42 + 1)),
                    OnNext<Int32>(40, 17),
                    OnCompleted<Int32>(50)
                );

                var res = client.Start(() =>
                    xs.Sum()
                );

                res.Messages.AssertEqual(
                    OnError<Int32>(230, e => e is OverflowException)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 230)
                );
            });
        }

        [TestMethod]
        public void SumNullableInt32_Overflow1()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnNext<Int32?>(10, 42),
                    OnNext<Int32?>(20, Int32.MaxValue - 100),
                    OnNext<Int32?>(25, default(Int32?)),
                    OnNext<Int32?>(30, 100 - 42 + 1),
                    OnNext<Int32?>(40, -17),
                    OnCompleted<Int32?>(50)
                );

                var res = client.Start(() =>
                    xs.Sum()
                );

                res.Messages.AssertEqual(
                    OnError<Int32?>(230, e => e is OverflowException)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 230)
                );
            });
        }

        [TestMethod]
        public void SumNullableInt32_Overflow2()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnNext<Int32?>(10, -42),
                    OnNext<Int32?>(20, Int32.MinValue + 100),
                    OnNext<Int32?>(25, default(Int32?)),
                    OnNext<Int32?>(30, -(100 - 42 + 1)),
                    OnNext<Int32?>(40, 17),
                    OnCompleted<Int32?>(50)
                );

                var res = client.Start(() =>
                    xs.Sum()
                );

                res.Messages.AssertEqual(
                    OnError<Int32?>(230, e => e is OverflowException)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 230)
                );
            });
        }

        [TestMethod]
        public void SumInt64_Overflow1()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnNext<Int64>(10, 42L),
                    OnNext<Int64>(20, Int64.MaxValue - 100L),
                    OnNext<Int64>(30, 100L - 42L + 1L),
                    OnNext<Int64>(40, -17L),
                    OnCompleted<Int64>(50)
                );

                var res = client.Start(() =>
                    xs.Sum()
                );

                res.Messages.AssertEqual(
                    OnError<Int64>(230, e => e is OverflowException)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 230)
                );
            });
        }

        [TestMethod]
        public void SumInt64_Overflow2()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnNext<Int64>(10, -42L),
                    OnNext<Int64>(20, Int64.MinValue + 100L),
                    OnNext<Int64>(30, -(100L - 42L + 1L)),
                    OnNext<Int64>(40, 17L),
                    OnCompleted<Int64>(50)
                );

                var res = client.Start(() =>
                    xs.Sum()
                );

                res.Messages.AssertEqual(
                    OnError<Int64>(230, e => e is OverflowException)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 230)
                );
            });
        }

        [TestMethod]
        public void SumNullableInt64_Overflow1()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnNext<Int64?>(10, 42L),
                    OnNext<Int64?>(20, Int64.MaxValue - 100L),
                    OnNext<Int64?>(25, default(Int64?)),
                    OnNext<Int64?>(30, 100L - 42L + 1L),
                    OnNext<Int64?>(40, -17L),
                    OnCompleted<Int64?>(50)
                );

                var res = client.Start(() =>
                    xs.Sum()
                );

                res.Messages.AssertEqual(
                    OnError<Int64?>(230, e => e is OverflowException)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 230)
                );
            });
        }

        [TestMethod]
        public void SumNullableInt64_Overflow2()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnNext<Int64?>(10, -42L),
                    OnNext<Int64?>(20, Int64.MinValue + 100L),
                    OnNext<Int64?>(25, default(Int64?)),
                    OnNext<Int64?>(30, -(100L - 42L + 1L)),
                    OnNext<Int64?>(40, 17L),
                    OnCompleted<Int64?>(50L)
                );

                var res = client.Start(() =>
                    xs.Sum()
                );

                res.Messages.AssertEqual(
                    OnError<Int64?>(230, e => e is OverflowException)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 230)
                );
            });
        }

        [TestMethod]
        public void SumDecimal_Overflow1()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnNext<Decimal>(10, 42m),
                    OnNext<Decimal>(20, Decimal.MaxValue - 100m),
                    OnNext<Decimal>(30, 100m - 42m + 1m),
                    OnNext<Decimal>(40, -17m),
                    OnCompleted<Decimal>(50)
                );

                var res = client.Start(() =>
                    xs.Sum()
                );

                res.Messages.AssertEqual(
                    OnError<Decimal>(230, e => e is OverflowException)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 230)
                );
            });
        }

        [TestMethod]
        public void SumDecimal_Overflow2()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnNext<Decimal>(10, -42m),
                    OnNext<Decimal>(20, Decimal.MinValue + 100m),
                    OnNext<Decimal>(30, -(100m - 42m + 1m)),
                    OnNext<Decimal>(40, 17m),
                    OnCompleted<Decimal>(50)
                );

                var res = client.Start(() =>
                    xs.Sum()
                );

                res.Messages.AssertEqual(
                    OnError<Decimal>(230, e => e is OverflowException)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 230)
                );
            });
        }

        [TestMethod]
        public void SumNullableDecimal_Overflow1()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnNext<Decimal?>(10, 42m),
                    OnNext<Decimal?>(20, Decimal.MaxValue - 100m),
                    OnNext<Decimal?>(25, default(Decimal?)),
                    OnNext<Decimal?>(30, 100m - 42m + 1m),
                    OnNext<Decimal?>(40, -17m),
                    OnCompleted<Decimal?>(50)
                );

                var res = client.Start(() =>
                    xs.Sum()
                );

                res.Messages.AssertEqual(
                    OnError<Decimal?>(230, e => e is OverflowException)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 230)
                );
            });
        }

        [TestMethod]
        public void SumNullableDecimal_Overflow2()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnNext<Decimal?>(10, -42m),
                    OnNext<Decimal?>(20, Decimal.MinValue + 100m),
                    OnNext<Decimal?>(25, default(Decimal?)),
                    OnNext<Decimal?>(30, -(100m - 42m + 1m)),
                    OnNext<Decimal?>(40, 17m),
                    OnCompleted<Decimal?>(50)
                );

                var res = client.Start(() =>
                    xs.Sum()
                );

                res.Messages.AssertEqual(
                    OnError<Decimal?>(230, e => e is OverflowException)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 230)
                );
            });
        }
    }
}
