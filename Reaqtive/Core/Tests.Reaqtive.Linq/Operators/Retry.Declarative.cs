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
    public partial class Retry : OperatorTestBase
    {
        [TestMethod]
        public void Retry_Observable_Basic()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnNext(100, 1),
                    OnNext(150, 2),
                    OnNext(200, 3),
                    OnCompleted<int>(250)
                );

                var res = client.Start(() =>
                    xs.Retry()
                );

                res.Messages.AssertEqual(
                    OnNext(300, 1),
                    OnNext(350, 2),
                    OnNext(400, 3),
                    OnCompleted<int>(450)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 450)
                );
            });
        }

        [TestMethod]
        public void Retry_Observable_Infinite()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnNext(100, 1),
                    OnNext(150, 2),
                    OnNext(200, 3)
                );

                var res = client.Start(() =>
                    xs.Retry()
                );

                res.Messages.AssertEqual(
                    OnNext(300, 1),
                    OnNext(350, 2),
                    OnNext(400, 3)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 1000)
                );
            });
        }

        [TestMethod]
        public void Retry_Observable_Error()
        {
            Run(client =>
            {
                var ex = new Exception();

                var xs = client.CreateColdObservable(
                    OnNext(100, 1),
                    OnNext(150, 2),
                    OnNext(200, 3),
                    OnError<int>(250, ex)
                );

                var res = client.Start(() =>
                    xs.Retry(), 1100
                );

                res.Messages.AssertEqual(
                    OnNext(300, 1),
                    OnNext(350, 2),
                    OnNext(400, 3),
                    OnNext(550, 1),
                    OnNext(600, 2),
                    OnNext(650, 3),
                    OnNext(800, 1),
                    OnNext(850, 2),
                    OnNext(900, 3),
                    OnNext(1050, 1)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 450),
                    Subscribe(450, 700),
                    Subscribe(700, 950),
                    Subscribe(950, 1100)
                );
            });
        }

        [TestMethod]
        public void Retry_Observable_RetryCount_Basic()
        {
            Run(client =>
            {
                var ex = new Exception();

                var xs = client.CreateColdObservable(
                    OnNext(5, 1),
                    OnNext(10, 2),
                    OnNext(15, 3),
                    OnError<int>(20, ex)
                );

                var res = client.Start(() =>
                    xs.Retry(3)
                );

                res.Messages.AssertEqual(
                    OnNext(205, 1),
                    OnNext(210, 2),
                    OnNext(215, 3),
                    OnNext(225, 1),
                    OnNext(230, 2),
                    OnNext(235, 3),
                    OnNext(245, 1),
                    OnNext(250, 2),
                    OnNext(255, 3),
                    OnError<int>(260, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 220),
                    Subscribe(220, 240),
                    Subscribe(240, 260)
                );
            });
        }

        [TestMethod]
        public void Retry_Observable_RetryCount_Infinite()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnNext(100, 1),
                    OnNext(150, 2),
                    OnNext(200, 3)
                );

                var res = client.Start(() =>
                    xs.Retry(3)
                );

                res.Messages.AssertEqual(
                    OnNext(300, 1),
                    OnNext(350, 2),
                    OnNext(400, 3)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 1000)
                );
            });
        }

        [TestMethod]
        public void Retry_Observable_RetryCount_Completed()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnNext(100, 1),
                    OnNext(150, 2),
                    OnNext(200, 3),
                    OnCompleted<int>(250)
                );

                var res = client.Start(() =>
                    xs.Retry(3)
                );

                res.Messages.AssertEqual(
                    OnNext(300, 1),
                    OnNext(350, 2),
                    OnNext(400, 3),
                    OnCompleted<int>(450)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 450)
                );
            });
        }

        [TestMethod]
        public void Retry_Observable_RetryCount_Dispose()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnNext(5, 1),
                    OnNext(10, 2),
                    OnNext(15, 3),
                    OnError<int>(20, new Exception())
                );

                var res = client.Start(() =>
                    xs.Retry(3), 231
                );

                res.Messages.AssertEqual(
                    OnNext(205, 1),
                    OnNext(210, 2),
                    OnNext(215, 3),
                    OnNext(225, 1),
                    OnNext(230, 2)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 220),
                    Subscribe(220, 231)
                );
            });
        }

        [TestMethod]
        public void Retry_NoCount_Cold()
        {
            Run(client =>
            {
                var xs = client.CreateColdObservable(
                    OnNext(10, 42),
                    OnNext(20, 43),
                    OnError<int>(30, new Exception())
                );

                var res = client.Start(() =>
                    xs.Retry().Take(5)
                );

                res.Messages.AssertEqual(
                    OnNext(210, 42),
                    OnNext(220, 43),
                    OnNext(240, 42),
                    OnNext(250, 43),
                    OnNext(270, 42),
                    OnCompleted<int>(270)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 230),
                    Subscribe(230, 260),
                    Subscribe(260, 270)
                );
            });
        }

        [TestMethod]
        public void Retry_NoCount_Hot()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(210, 42),
                    OnNext(220, 43),
                    OnNext(230, 0),
                    OnNext(240, 42),
                    OnNext(250, 43),
                    OnNext(260, 0),
                    OnNext(270, 42),
                    OnNext(280, 43),
                    OnNext(290, 0),
                    OnCompleted<int>(300)
                );

                var res = client.Start(() =>
                    xs.Select(x => 42 * 43 / x).Retry()
                );

                res.Messages.AssertEqual(
                    OnNext(210, 43),
                    OnNext(220, 42),
                    OnNext(240, 43),
                    OnNext(250, 42),
                    OnNext(270, 43),
                    OnNext(280, 42),
                    OnCompleted<int>(300)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 230),
                    Subscribe(230, 260),
                    Subscribe(260, 290),
                    Subscribe(290, 300)
                );
            });
        }

        [TestMethod]
        public void Retry_Count_Cold()
        {
            Run(client =>
            {
                var ex = new Exception();

                var xs = client.CreateColdObservable(
                    OnNext(10, 42),
                    OnNext(20, 43),
                    OnError<int>(30, ex)
                );

                var res = client.Start(() =>
                    xs.Retry(2)
                );

                res.Messages.AssertEqual(
                    OnNext(210, 42),
                    OnNext(220, 43),
                    OnNext(240, 42),
                    OnNext(250, 43),
                    OnError<int>(260, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 230),
                    Subscribe(230, 260)
                );
            });
        }

        [TestMethod]
        public void Retry_Count_Hot()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(210, 42),
                    OnNext(220, 43),
                    OnNext(230, 0),
                    OnNext(240, 42),
                    OnNext(250, 43),
                    OnNext(260, 0),
                    OnNext(270, 42),
                    OnNext(280, 43),
                    OnNext(290, 0),
                    OnCompleted<int>(300)
                );

                var res = client.Start(() =>
                    xs.Select(x => 42 * 43 / x).Retry(2)
                );

                res.Messages.AssertEqual(
                    OnNext(210, 43),
                    OnNext(220, 42),
                    OnNext(240, 43),
                    OnNext(250, 42),
                    OnError<int>(260, ex => ex is DivideByZeroException)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 230),
                    Subscribe(230, 260)
                );
            });
        }

#if !GLITCHING
        [TestMethod]
        public void Retry_ExceptionAfterSubscribe()
        {
            var first = true;
            Run(client =>
            {
                var ex = new InvalidOperationException();

                var res = client.Start(() =>
                    client.Exceptional<int>(() => { if (!first) throw ex; first = false; }, false).Retry()
                );

                res.Messages.AssertEqual(
                    OnError<int>(Increment(200, 1), ex)
                );
            });
        }

        [TestMethod]
        public void Retry_ExceptionOnSubscribe()
        {
            var first = true;
            Run(client =>
            {
                var ex = new InvalidOperationException();

                var res = client.Start(() =>
                    client.Exceptional<int>(() => { if (!first) throw ex; first = false; }, true).Retry()
                );

                res.Messages.AssertEqual(
                    OnError<int>(Increment(200, 1), ex)
                );
            });
        }

        [TestMethod]
        public void RetryCount_ExceptionAfterSubscribe()
        {
            var first = true;
            Run(client =>
            {
                var ex = new InvalidOperationException();

                var res = client.Start(() =>
                    client.Exceptional<int>(() => { if (!first) throw ex; first = false; }, false).Retry(2)
                );

                res.Messages.AssertEqual(
                    OnError<int>(Increment(200, 1), ex)
                );
            });
        }

        [TestMethod]
        public void RetryCount_ExceptionOnSubscribe()
        {
            var first = true;
            Run(client =>
            {
                var ex = new InvalidOperationException();

                var res = client.Start(() =>
                    client.Exceptional<int>(() => { if (!first) throw ex; first = false; }, true).Retry(2)
                );

                res.Messages.AssertEqual(
                    OnError<int>(Increment(200, 1), ex)
                );
            });
        }
#endif
    }
}
