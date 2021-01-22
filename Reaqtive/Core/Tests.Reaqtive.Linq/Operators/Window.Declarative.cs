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
    public partial class Window : OperatorTestBase
    {
        [TestMethod]
        public void Window_Count_Simple1()
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
                    xs.Window(3).SelectMany(w => w.Sum())
                );

                res.Messages.AssertEqual(
                    OnNext(230, 1 + 2 + 3),
                    OnNext(260, 4 + 5 + 6),
                    OnNext(290, 7 + 8 + 9),
                    OnNext(300, 0),
                    OnCompleted<int>(300)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(Increment(200, 1), 300));
            });
        }

        [TestMethod]
        public void Window_Count_Simple2()
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
                    OnCompleted<int>(300)
                );

                var res = client.Start(() =>
                    xs.Window(3).SelectMany(w => w.Sum())
                );

                res.Messages.AssertEqual(
                    OnNext(230, 1 + 2 + 3),
                    OnNext(260, 4 + 5 + 6),
                    OnNext(300, 7),
                    OnCompleted<int>(300)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(Increment(200, 1), 300));
            });
        }

        [TestMethod]
        public void Window_Count_ManOrBoy1()
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
                    OnCompleted<int>(300)
                );

                var res = client.Start(() =>
                    xs.Window(3).SelectMany(w => w.Aggregate("", (l, x) => l + "," + x, s => s.TrimStart(',')).CombineLatest(w.Sum(), (c, s) => "Sum(" + c + ") = " + s))
                );

                res.Messages.AssertEqual(
                    OnNext(230, "Sum(1,2,3) = 6"),
                    OnNext(260, "Sum(4,5,6) = 15"),
                    OnNext(300, "Sum(7) = 7"),
                    OnCompleted<string>(300)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(Increment(200, 1), 300));
            });
        }

        [TestMethod]
        public void Window_Count_ManOrBoy2()
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
                    OnCompleted<int>(300)
                );

                var res = client.Start(() =>
                    xs.Window(3).SelectMany(w => w.Take(1).CombineLatest(w.Take(2).Sum(), (l, r) => l + "," + r))
                );

                res.Messages.AssertEqual(
                    OnNext(220, "1,3"),
                    OnNext(250, "4,9"),
                    OnNext(300, "7,7"),
                    OnCompleted<string>(300)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(Increment(200, 1), 300));
            });
        }

        [TestMethod]
        public void Window_Count_ManOrBoy3()
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
                    OnCompleted<int>(300)
                );

                var res = client.Start(() =>
                    xs.Window(3).Skip(1).Take(1).SelectMany(w => w.Skip(1).Take(1).Sum())
                );

                res.Messages.AssertEqual(
                    OnNext(250, 5),
                    OnCompleted<int>(250)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(Increment(200, 1), 250));
            });
        }

        [TestMethod]
        public void Window_Count_Skip_Simple1()
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
                    xs.Window(3, 2).SelectMany(w => w.Sum())
                );

                res.Messages.AssertEqual(
                    OnNext(230, 1 + 2 + 3),
                    OnNext(250, 3 + 4 + 5),
                    OnNext(270, 5 + 6 + 7),
                    OnNext(290, 7 + 8 + 9),
                    OnNext(300, 9),
                    OnCompleted<int>(300)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(Increment(200, 1), 300));
            });
        }

        [TestMethod]
        public void Window_Count_Skip_Simple2()
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
                    xs.Window(3, 5).SelectMany(w => w.Sum())
                );

                res.Messages.AssertEqual(
                    OnNext(230, 1 + 2 + 3),
                    OnNext(280, 6 + 7 + 8),
                    OnCompleted<int>(300)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(Increment(200, 1), 300));
            });
        }

        [TestMethod]
        public void Window_Count_Skip_Simple3()
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
                    xs.Window(3, 3).SelectMany(w => w.Sum())
                );

                res.Messages.AssertEqual(
                    OnNext(230, 1 + 2 + 3),
                    OnNext(260, 4 + 5 + 6),
                    OnNext(290, 7 + 8 + 9),
                    OnNext(300, 0),
                    OnCompleted<int>(300)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(Increment(200, 1), 300));
            });
        }

        [TestMethod]
        public void Window_Duration_Simple1()
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
                    xs.Window(TimeSpan.FromTicks(30)).SelectMany(w => w.Sum())
                );

                res.Messages.AssertEqual(
                    OnNext(230, 1 + 2 + 3),
                    OnNext(260, 4 + 5 + 6),
                    OnNext(290, 7 + 8 + 9),
                    OnNext(300, 0),
                    OnCompleted<int>(300)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(Increment(200, 1), 300));
            });
        }

        [TestMethod]
        public void Window_Duration_Shift_Simple1()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable<int>(
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
                    xs.Window(TimeSpan.FromTicks(30), TimeSpan.FromTicks(20)).SelectMany(w => w.Sum())
                );

                res.Messages.AssertEqual(
                    OnNext(230, 1 + 2 + 3),
                    OnNext(250, 3 + 4 + 5),
                    OnNext(270, 5 + 6 + 7),
                    OnNext(290, 7 + 8 + 9),
                    OnNext(300, 9),
                    OnCompleted<int>(300)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(Increment(200, 1), 300)
                );
            });
        }

        [TestMethod]
        public void Window_Duration_Shift_Simple2()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable<int>(
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
                    xs.Window(TimeSpan.FromTicks(20), TimeSpan.FromTicks(30)).SelectMany(w => w.Sum())
                );

                res.Messages.AssertEqual(
                    OnNext(220, 1 + 2),
                    OnNext(250, 4 + 5),
                    OnNext(280, 7 + 8),
                    OnNext(300, 0),
                    OnCompleted<int>(300)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(Increment(200, 1), 300)
                );
            });
        }

        [TestMethod]
        public void Window_Count_SelectMany_Aggregate1()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable<int>(
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

                var res = client.Start(() => xs.Window(3).SelectMany(w => w.Aggregate("", (s, x) => s == "" ? x.ToString() : s + "," + x)));

                res.Messages.AssertEqual(
                    OnNext(230, "1,2,3"),
                    OnNext(260, "4,5,6"),
                    OnNext(290, "7,8,9"),
                    OnNext(300, ""),
                    OnCompleted<string>(300)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(Increment(200, 1), 300)
                );
            });
        }

        [TestMethod]
        public void Window_Count_SelectMany_Aggregate2()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable<int>(
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

                var res = client.Start(() => xs.Window(4).SelectMany(w => w.Aggregate("", (s, x) => s == "" ? x.ToString() : s + "," + x)));

                res.Messages.AssertEqual(
                    OnNext(240, "1,2,3,4"),
                    OnNext(280, "5,6,7,8"),
                    OnNext(300, "9"),
                    OnCompleted<string>(300)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(Increment(200, 1), 300)
                );
            });
        }

        [TestMethod]
        public void Window_Count_SelectMany_Error()
        {
            Run(client =>
            {
                var ex = new Exception();

                var xs = client.CreateHotObservable<int>(
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

                var res = client.Start(() => xs.Window(4).SelectMany(w => w.Aggregate("", (s, x) => s == "" ? x.ToString() : s + "," + x)));

                res.Messages.AssertEqual(
                    OnNext(240, "1,2,3,4"),
                    OnNext(280, "5,6,7,8"),
                    OnError<string>(300, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(Increment(200, 1), 300)
                );
            });
        }

        [TestMethod]
        public void Window_Count_Skip_SelectMany_Aggregate1()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable<int>(
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

                var res = client.Start(() => xs.Window(3, 3).SelectMany(w => w.Aggregate("", (s, x) => s == "" ? x.ToString() : s + "," + x)));

                res.Messages.AssertEqual(
                    OnNext(230, "1,2,3"),
                    OnNext(260, "4,5,6"),
                    OnNext(290, "7,8,9"),
                    OnNext(300, ""),
                    OnCompleted<string>(300)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(Increment(200, 1), 300)
                );
            });
        }

        [TestMethod]
        public void Window_Count_Skip_SelectMany_Aggregate2()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable<int>(
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

                var res = client.Start(() => xs.Window(3, 2).SelectMany(w => w.Aggregate("", (s, x) => s == "" ? x.ToString() : s + "," + x)));

                res.Messages.AssertEqual(
                    OnNext(230, "1,2,3"),
                    OnNext(250, "3,4,5"),
                    OnNext(270, "5,6,7"),
                    OnNext(290, "7,8,9"),
                    OnNext(300, "9"),
                    OnCompleted<string>(300)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(Increment(200, 1), 300)
                );
            });
        }

        [TestMethod]
        public void Window_Count_Skip_SelectMany_Aggregate3()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable<int>(
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

                var res = client.Start(() => xs.Window(3, 4).SelectMany(w => w.Aggregate("", (s, x) => s == "" ? x.ToString() : s + "," + x)));

                res.Messages.AssertEqual(
                    OnNext(230, "1,2,3"),
                    OnNext(270, "5,6,7"),
                    OnNext(300, "9"),
                    OnCompleted<string>(300)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(Increment(200, 1), 300)
                );
            });
        }

        [TestMethod]
        public void Window_Count_Skip_SelectMany_Error()
        {
            Run(client =>
            {
                var ex = new Exception();

                var xs = client.CreateHotObservable<int>(
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

                var res = client.Start(() => xs.Window(3, 4).SelectMany(w => w.Aggregate("", (s, x) => s == "" ? x.ToString() : s + "," + x)));

                res.Messages.AssertEqual(
                    OnNext(230, "1,2,3"),
                    OnNext(270, "5,6,7"),
                    OnError<string>(300, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(Increment(200, 1), 300)
                );
            });
        }

        [TestMethod]
        public void Window_Time_SelectMany_Aggregate1()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable<int>(
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

                var res = client.Start(() => xs.Window(TimeSpan.FromTicks(30)).SelectMany(w => w.Aggregate("", (s, x) => s == "" ? x.ToString() : s + "," + x)));

                res.Messages.AssertEqual(
                    OnNext(230, "1,2,3"),
                    OnNext(260, "4,5,6"),
                    OnNext(290, "7,8,9"),
                    OnNext(300, ""),
                    OnCompleted<string>(300)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(Increment(200, 1), 300)
                );
            });
        }

        [TestMethod]
        public void Window_Time_SelectMany_Error()
        {
            Run(client =>
            {
                var ex = new Exception();

                var xs = client.CreateHotObservable<int>(
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

                var res = client.Start(() => xs.Window(TimeSpan.FromTicks(30)).SelectMany(w => w.Aggregate("", (s, x) => s == "" ? x.ToString() : s + "," + x)));

                res.Messages.AssertEqual(
                    OnNext(230, "1,2,3"),
                    OnNext(260, "4,5,6"),
                    OnNext(290, "7,8,9"),
                    OnError<string>(300, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(Increment(200, 1), 300)
                );
            });
        }

        [TestMethod]
        public void Window_Time_Shift_SelectMany_Aggregate1()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable<int>(
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

                var res = client.Start(() => xs.Window(TimeSpan.FromTicks(30), TimeSpan.FromTicks(20)).SelectMany(w => w.Aggregate("", (s, x) => s == "" ? x.ToString() : s + "," + x)));

                res.Messages.AssertEqual(
                    OnNext(230, "1,2,3"),
                    OnNext(250, "3,4,5"),
                    OnNext(270, "5,6,7"),
                    OnNext(290, "7,8,9"),
                    OnNext(300, "9"),
                    OnCompleted<string>(300)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(Increment(200, 1), 300)
                );
            });
        }

        [TestMethod]
        public void Window_Time_Shift_SelectMany_Aggregate2()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable<int>(
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

                var res = client.Start(() => xs.Window(TimeSpan.FromTicks(20), TimeSpan.FromTicks(30)).SelectMany(w => w.Aggregate("", (s, x) => s == "" ? x.ToString() : s + "," + x)));

                res.Messages.AssertEqual(
                    OnNext(220, "1,2"),
                    OnNext(250, "4,5"),
                    OnNext(280, "7,8"),
                    OnNext(300, ""),
                    OnCompleted<string>(300)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(Increment(200, 1), 300)
                );
            });
        }

        [TestMethod]
        public void Window_Time_Shift_SelectMany_Error()
        {
            Run(client =>
            {
                var ex = new Exception();

                var xs = client.CreateHotObservable<int>(
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

                var res = client.Start(() => xs.Window(TimeSpan.FromTicks(30), TimeSpan.FromTicks(20)).SelectMany(w => w.Aggregate("", (s, x) => s == "" ? x.ToString() : s + "," + x)));

                res.Messages.AssertEqual(
                    OnNext(230, "1,2,3"),
                    OnNext(250, "3,4,5"),
                    OnNext(270, "5,6,7"),
                    OnNext(290, "7,8,9"),
                    OnError<string>(300, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(Increment(200, 1), 300)
                );
            });
        }

        [TestMethod]
        public void Window_Ferry_Simple1()
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
                    xs.Window(TimeSpan.FromTicks(50), 3).SelectMany(w => w.Sum())
                );

                res.Messages.AssertEqual(
                    OnNext(230, 1 + 2 + 3),
                    OnNext(260, 4 + 5 + 6),
                    OnNext(290, 7 + 8 + 9),
                    OnNext(300, 0),
                    OnCompleted<int>(300)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(Increment(200, 1), 300));
            });
        }

        [TestMethod]
        public void Window_Ferry_Simple2()
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
                    xs.Window(TimeSpan.FromTicks(30), 5).SelectMany(w => w.Sum())
                );

                res.Messages.AssertEqual(
                    OnNext(Increment(230, 1), 1 + 2 + 3),
                    OnNext(Increment(260, 1), 4 + 5 + 6),
                    OnNext(Increment(290, 1), 7 + 8 + 9),
                    OnNext(300, 0),
                    OnCompleted<int>(300)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(Increment(200, 1), 300));
            });
        }

        [TestMethod]
        public void Window_Ferry_Simple3()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable(
                    OnNext(210, 1),
                    OnNext(215, 2),
                    OnNext(220, 3),
                    OnNext(225, 4),
                    OnNext(240, 5),
                    OnNext(260, 6),
                    OnNext(265, 7),
                    OnNext(275, 8),
                    OnNext(290, 9),
                    OnCompleted<int>(300)
                );

                var res = client.Start(() =>
                    xs.Window(TimeSpan.FromTicks(30), 3).SelectMany(w => w.Sum())
                );

                res.Messages.AssertEqual(
                    OnNext(220, 1 + 2 + 3),
                    OnNext(250, 4 + 5),
                    OnNext(275, 6 + 7 + 8),
                    OnNext(300, 9),
                    OnCompleted<int>(300)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(Increment(200, 1), 300));
            });
        }

        [TestMethod]
        public void Window_Ferry_Error()
        {
            Run(client =>
            {
                var ex = new Exception();

                var xs = client.CreateHotObservable(
                    OnNext(210, 1),
                    OnNext(215, 2),
                    OnNext(220, 3),
                    OnNext(225, 4),
                    OnNext(240, 5),
                    OnNext(260, 6),
                    OnNext(265, 7),
                    OnNext(275, 8),
                    OnNext(290, 9),
                    OnError<int>(300, ex)
                );

                var res = client.Start(() =>
                    xs.Window(TimeSpan.FromTicks(30), 3).SelectMany(w => w.Sum())
                );

                res.Messages.AssertEqual(
                    OnNext(220, 1 + 2 + 3),
                    OnNext(250, 4 + 5),
                    OnNext(275, 6 + 7 + 8),
                    OnError<int>(300, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(Increment(200, 1), 300));
            });
        }
    }
}
