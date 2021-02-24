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
    public partial class GroupBy : OperatorTestBase
    {
        [TestMethod]
        public void GroupBy_Simple1()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(210, 1),
                    OnNext(220, 2),
                    OnNext(230, 3),
                    OnNext(240, 4),
                    OnNext(250, 5),
                    OnNext(260, 6),
                    OnNext(270, 7),
                    OnNext(280, 8),
                    OnNext(290, 9),
                    OnCompleted<int>(300)
                );

                var res = client.Start(() =>
                    xs.GroupBy(x => x % 2).SelectMany(w => w.Sum())
                );

                res.Messages.AssertEqual(
                    OnNext(300, 1 + 3 + 5 + 7 + 9),
                    OnNext(300, 2 + 4 + 6 + 8),
                    OnCompleted<int>(300)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 300)
                );
            });
        }

        [TestMethod]
        public void GroupBy_Simple2()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(210, 1),
                    OnNext(220, 2),
                    OnNext(230, 3),
                    OnNext(240, 4),
                    OnNext(250, 5),
                    OnNext(260, 6),
                    OnNext(270, 7),
                    OnNext(280, 8),
                    OnNext(290, 9),
                    OnCompleted<int>(300)
                );

                var res = client.Start(() =>
                    xs.GroupBy(x => x % 2, x => x.ToString()).SelectMany(w => w.Aggregate("", (s, x) => s + x))
                );

                res.Messages.AssertEqual(
                    OnNext(300, "13579"),
                    OnNext(300, "2468"),
                    OnCompleted<string>(300)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 300)
                );
            });
        }

        [TestMethod]
        public void GroupBy_Simple3()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(210, "bar"),
                    OnNext(220, "foo"),
                    OnNext(230, "bar"),
                    OnNext(240, "qux"),
                    OnNext(250, default(string)),
                    OnNext(260, "qux"),
                    OnNext(270, "qux"),
                    OnNext(280, "foo"),
                    OnNext(290, default(string)),
                    OnCompleted<string>(300)
                );

                var res = client.Start(() =>
                    xs.GroupBy(x => x).SelectMany(w => w.Count(), (w, c) => (w.Key ?? "<null>") + " - " + c)
                );

                res.Messages.AssertEqual(
                    OnNext(300, "<null> - 2"),  // Note: the null group will always see the terminal message first
                    OnNext(300, "bar - 2"),
                    OnNext(300, "foo - 2"),
                    OnNext(300, "qux - 3"),
                    OnCompleted<string>(300)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 300)
                );
            });
        }

        [TestMethod]
        public void GroupBy_Error1()
        {
            Run(client =>
            {
                var ex = new Exception();

                var xs = client.CreateHotObservable(
                    OnNext(210, 1),
                    OnNext(220, 2),
                    OnNext(230, 3),
                    OnNext(240, 4),
                    OnNext(250, 5),
                    OnNext(260, 6),
                    OnNext(270, 7),
                    OnNext(280, 8),
                    OnNext(290, 9),
                    OnError<int>(300, ex)
                );

                var res = client.Start(() =>
                    xs.GroupBy(x => x % 2).SelectMany(w => w.Sum())
                );

                res.Messages.AssertEqual(
                    OnError<int>(300, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 300)
                );
            });
        }

        [TestMethod]
        public void GroupBy_Error2()
        {
            Run(client =>
            {
                var ex = new Exception();

                var xs = client.CreateHotObservable(
                    OnNext(210, "bar"),
                    OnNext(220, "foo"),
                    OnNext(230, "bar"),
                    OnNext(240, "qux"),
                    OnNext(250, default(string)),
                    OnNext(260, "qux"),
                    OnNext(270, "qux"),
                    OnNext(280, "foo"),
                    OnNext(290, default(string)),
                    OnError<string>(300, ex)
                );

                var res = client.Start(() =>
                    xs.GroupBy(x => x).SelectMany(w => w.Count(), (w, c) => (w.Key ?? "<null>") + " - " + c)
                );

                res.Messages.AssertEqual(
                    OnError<string>(300, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 300)
                );
            });
        }

        [TestMethod]
        public void GroupBy_KeySelectorThrows()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(210, 2),
                    OnNext(220, 4),
                    OnNext(230, 1),
                    OnNext(240, 0),
                    OnNext(250, 3),
                    OnNext(260, 7),
                    OnCompleted<int>(300)
                );

                var res = client.Start(() =>
                    xs.GroupBy(x => 100 / x).SelectMany(w => w.Sum())
                );

                res.Messages.AssertEqual(
                    OnError<int>(240, ex => ex is DivideByZeroException)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 240)
                );
            });
        }

        [TestMethod]
        public void GroupBy_ElementSelectorThrows()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(210, 2),
                    OnNext(220, 4),
                    OnNext(230, 1),
                    OnNext(240, 0),
                    OnNext(250, 3),
                    OnNext(260, 7),
                    OnCompleted<int>(300)
                );

                var res = client.Start(() =>
                    xs.GroupBy(x => x % 2, x => 100 / x).SelectMany(w => w.Sum())
                );

                res.Messages.AssertEqual(
                    OnError<int>(240, ex => ex is DivideByZeroException)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 240)
                );
            });
        }
    }
}
