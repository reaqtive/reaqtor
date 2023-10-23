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
                    OnNext<int>(10, 42),
                    OnNext<int>(20, int.MaxValue - 100),
                    OnNext<int>(30, 100 - 42 + 1),
                    OnNext<int>(40, -17),
                    OnCompleted<int>(50)
                );

                var res = client.Start(() =>
                    xs.Sum()
                );

                res.Messages.AssertEqual(
                    OnError<int>(230, e => e is OverflowException)
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
                    OnNext<int>(10, -42),
                    OnNext<int>(20, int.MinValue + 100),
                    OnNext<int>(30, -(100 - 42 + 1)),
                    OnNext<int>(40, 17),
                    OnCompleted<int>(50)
                );

                var res = client.Start(() =>
                    xs.Sum()
                );

                res.Messages.AssertEqual(
                    OnError<int>(230, e => e is OverflowException)
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
                    OnNext<int?>(10, 42),
                    OnNext<int?>(20, int.MaxValue - 100),
                    OnNext<int?>(25, default(int?)),
                    OnNext<int?>(30, 100 - 42 + 1),
                    OnNext<int?>(40, -17),
                    OnCompleted<int?>(50)
                );

                var res = client.Start(() =>
                    xs.Sum()
                );

                res.Messages.AssertEqual(
                    OnError<int?>(230, e => e is OverflowException)
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
                    OnNext<int?>(10, -42),
                    OnNext<int?>(20, int.MinValue + 100),
                    OnNext<int?>(25, default(int?)),
                    OnNext<int?>(30, -(100 - 42 + 1)),
                    OnNext<int?>(40, 17),
                    OnCompleted<int?>(50)
                );

                var res = client.Start(() =>
                    xs.Sum()
                );

                res.Messages.AssertEqual(
                    OnError<int?>(230, e => e is OverflowException)
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
                    OnNext<long>(10, 42L),
                    OnNext<long>(20, long.MaxValue - 100L),
                    OnNext<long>(30, 100L - 42L + 1L),
                    OnNext<long>(40, -17L),
                    OnCompleted<long>(50)
                );

                var res = client.Start(() =>
                    xs.Sum()
                );

                res.Messages.AssertEqual(
                    OnError<long>(230, e => e is OverflowException)
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
                    OnNext<long>(10, -42L),
                    OnNext<long>(20, long.MinValue + 100L),
                    OnNext<long>(30, -(100L - 42L + 1L)),
                    OnNext<long>(40, 17L),
                    OnCompleted<long>(50)
                );

                var res = client.Start(() =>
                    xs.Sum()
                );

                res.Messages.AssertEqual(
                    OnError<long>(230, e => e is OverflowException)
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
                    OnNext<long?>(10, 42L),
                    OnNext<long?>(20, long.MaxValue - 100L),
                    OnNext<long?>(25, default(long?)),
                    OnNext<long?>(30, 100L - 42L + 1L),
                    OnNext<long?>(40, -17L),
                    OnCompleted<long?>(50)
                );

                var res = client.Start(() =>
                    xs.Sum()
                );

                res.Messages.AssertEqual(
                    OnError<long?>(230, e => e is OverflowException)
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
                    OnNext<long?>(10, -42L),
                    OnNext<long?>(20, long.MinValue + 100L),
                    OnNext<long?>(25, default(long?)),
                    OnNext<long?>(30, -(100L - 42L + 1L)),
                    OnNext<long?>(40, 17L),
                    OnCompleted<long?>(50L)
                );

                var res = client.Start(() =>
                    xs.Sum()
                );

                res.Messages.AssertEqual(
                    OnError<long?>(230, e => e is OverflowException)
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
                    OnNext<decimal>(10, 42m),
                    OnNext<decimal>(20, decimal.MaxValue - 100m),
                    OnNext<decimal>(30, 100m - 42m + 1m),
                    OnNext<decimal>(40, -17m),
                    OnCompleted<decimal>(50)
                );

                var res = client.Start(() =>
                    xs.Sum()
                );

                res.Messages.AssertEqual(
                    OnError<decimal>(230, e => e is OverflowException)
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
                    OnNext<decimal>(10, -42m),
                    OnNext<decimal>(20, decimal.MinValue + 100m),
                    OnNext<decimal>(30, -(100m - 42m + 1m)),
                    OnNext<decimal>(40, 17m),
                    OnCompleted<decimal>(50)
                );

                var res = client.Start(() =>
                    xs.Sum()
                );

                res.Messages.AssertEqual(
                    OnError<decimal>(230, e => e is OverflowException)
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
                    OnNext<decimal?>(10, 42m),
                    OnNext<decimal?>(20, decimal.MaxValue - 100m),
                    OnNext<decimal?>(25, default(decimal?)),
                    OnNext<decimal?>(30, 100m - 42m + 1m),
                    OnNext<decimal?>(40, -17m),
                    OnCompleted<decimal?>(50)
                );

                var res = client.Start(() =>
                    xs.Sum()
                );

                res.Messages.AssertEqual(
                    OnError<decimal?>(230, e => e is OverflowException)
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
                    OnNext<decimal?>(10, -42m),
                    OnNext<decimal?>(20, decimal.MinValue + 100m),
                    OnNext<decimal?>(25, default(decimal?)),
                    OnNext<decimal?>(30, -(100m - 42m + 1m)),
                    OnNext<decimal?>(40, 17m),
                    OnCompleted<decimal?>(50)
                );

                var res = client.Start(() =>
                    xs.Sum()
                );

                res.Messages.AssertEqual(
                    OnError<decimal?>(230, e => e is OverflowException)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 230)
                );
            });
        }
    }
}
