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
    public partial class StartWith : OperatorTestBase
    {
        [TestMethod]
        public void StartWith_Nothing()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnNext(220, 8),
                    OnCompleted<int>(250)
                );

                var res = client.Start(() =>
                    xs.StartWith()
                );

                res.Messages.AssertEqual(
                    OnNext(220, 8),
                    OnCompleted<int>(250)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(Increment(200, 1), 250));
            });
        }

        [TestMethod]
        public void StartWith_Test()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnNext(220, 8),
                    OnCompleted<int>(250)
                );

                var res = client.Start(() =>
                    xs.StartWith(2, 4, 6)
                );

                res.Messages.AssertEqual(
                    OnNext(Increment(200, 1), 2),
                    OnNext(Increment(200, 2), 4),
                    OnNext(Increment(200, 3), 6),
                    OnNext(220, 8),
                    OnCompleted<int>(250)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(Increment(200, 3), 250));
            });
        }

        [TestMethod]
        public void StartWith_Error()
        {
            Run(client =>
            {
                var ex = new Exception();

                var xs = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnNext(220, 8),
                    OnError<int>(250, ex)
                );

                var res = client.Start(() =>
                    xs.StartWith(2, 4, 6)
                );

                res.Messages.AssertEqual(
                    OnNext(Increment(200, 1), 2),
                    OnNext(Increment(200, 2), 4),
                    OnNext(Increment(200, 3), 6),
                    OnNext(220, 8),
                    OnError<int>(250, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(Increment(200, 3), 250));
            });
        }

#if !GLITCHING
        [TestMethod]
        public void StartWith_ExceptionAfterSubscribe()
        {
            Run(client =>
            {
                var ex = new InvalidOperationException();

                var xs = client.Exceptional<int>(() => { throw ex; }, false);

                var res = client.Start(() =>
                    xs.StartWith(42)
                );

                res.Messages.AssertEqual(
                    OnNext(Increment(200, 1), 42),
                    OnError<int>(Increment(200, 2), ex)
                );
            });
        }

        [TestMethod]
        public void StartWith_ExceptionOnSubscribe()
        {
            Run(client =>
            {
                var ex = new InvalidOperationException();

                var xs = client.Exceptional<int>(() => { throw ex; }, true);

                var res = client.Start(() =>
                    xs.StartWith(42)
                );

                res.Messages.AssertEqual(
                    OnNext(Increment(200, 1), 42),
                    OnError<int>(Increment(200, 2), ex)
                );
            });
        }
#endif

        [TestMethod]
        public void StartWith_PropagationOfContext()
        {
            Run(client =>
            {
                var context = GetContext(client);

                var res = client.Start(() => context.Timer(TimeSpan.FromTicks(300)).StartWith(1, 2, 3, 4, 5));

                res.Messages.AssertEqual(
                    OnNext(Increment(Subscribed, 1), 1L),
                    OnNext(Increment(Subscribed, 2), 2L),
                    OnNext(Increment(Subscribed, 3), 3L),
                    OnNext(Increment(Subscribed, 4), 4L),
                    OnNext(Increment(Subscribed, 5), 5L),
                    OnNext(Increment(Subscribed, 5) + 300, 0L),
                    OnCompleted<long>(Increment(Subscribed, 5) + 300));
            });
        }

        [TestMethod]
        public void StartWith_Nulls()
        {
            Run(client =>
            {
                var context = GetContext(client);

                var res = client.Start(() => context.Empty<string>().StartWith("hello", default));

                res.Messages.AssertEqual(
                    OnNext(Increment(Subscribed, 1), "hello"),
                    OnNext(Increment(Subscribed, 2), default(string)),
                    OnCompleted<string>(Increment(Subscribed, 3)));
            });
        }
    }
}
