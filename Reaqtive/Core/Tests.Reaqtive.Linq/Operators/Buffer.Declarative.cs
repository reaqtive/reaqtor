// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;

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
    public partial class Buffer : OperatorTestBase
    {
#pragma warning disable IDE0079 // Remove unnecessary suppression (only on .NET 5.0)
#pragma warning disable CA1829 // Use Length/Count property instead of Count() when available. (Count does not pass the allow list in remoting.)

        [TestMethod]
        public void Buffer_Count_Simple1()
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
                    xs.Buffer(3).Select(w => w.Sum())
                );

                res.Messages.AssertEqual(
                    OnNext(230, 1 + 2 + 3),
                    OnNext(260, 4 + 5 + 6),
                    OnNext(290, 7 + 8 + 9),
                    OnCompleted<int>(300)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 300));
            });
        }

        [TestMethod]
        public void Buffer_Count_Simple2()
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
                    xs.Buffer(3).Select(w => w.Sum())
                );

                res.Messages.AssertEqual(
                    OnNext(230, 1 + 2 + 3),
                    OnNext(260, 4 + 5 + 6),
                    OnNext(300, 7),
                    OnCompleted<int>(300)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 300));
            });
        }

        [TestMethod]
        public void Buffer_Count_Error()
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
                    xs.Buffer(3).Select(w => w.Sum())
                );

                res.Messages.AssertEqual(
                    OnNext(230, 1 + 2 + 3),
                    OnNext(260, 4 + 5 + 6),
                    OnNext(290, 7 + 8 + 9),
                    OnError<int>(300, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 300));
            });
        }

        [TestMethod]
        public void Buffer_Count_ManOrBoy()
        {
            Run(client =>
            {
                var msgs = new[]
                {
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
                };

                var xs = client.CreateHotObservable(msgs);
                var ys = client.CreateHotObservable(msgs);

                var res = client.Start(() =>

                    xs.Buffer(3).Select(w => w.Sum()).SequenceEqual(ys.Window(3).SelectMany(w => w.ToList()).Where(l => l.Count(/* .Count doesn't pass allow list in remoting */) > 0).Select(l => l.Sum()))
                );

                res.Messages.AssertEqual(
                    OnNext(300, true),
                    OnCompleted<bool>(300)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 300));

                ys.Subscriptions.AssertEqual(
                    Subscribe(Increment(200, 1), 300));
            });
        }

        [TestMethod]
        public void Buffer_Count_Skip_Simple1()
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
                    xs.Buffer(3, 2).Select(w => w.Sum())
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
                    Subscribe(200, 300));
            });
        }

        [TestMethod]
        public void Buffer_Count_Skip_Simple2()
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
                    xs.Buffer(3, 5).Select(w => w.Sum())
                );

                res.Messages.AssertEqual(
                    OnNext(230, 1 + 2 + 3),
                    OnNext(280, 6 + 7 + 8),
                    OnCompleted<int>(300)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 300));
            });
        }

        [TestMethod]
        public void Buffer_Count_Skip_Simple3()
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
                    xs.Buffer(3, 3).Select(w => w.Sum())
                );

                res.Messages.AssertEqual(
                    OnNext(230, 1 + 2 + 3),
                    OnNext(260, 4 + 5 + 6),
                    OnNext(290, 7 + 8 + 9),
                    OnCompleted<int>(300)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 300));
            });
        }

        [TestMethod]
        public void Buffer_Count_Skip_Error()
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
                    xs.Buffer(3, 3).Select(w => w.Sum())
                );

                res.Messages.AssertEqual(
                    OnNext(230, 1 + 2 + 3),
                    OnNext(260, 4 + 5 + 6),
                    OnNext(290, 7 + 8 + 9),
                    OnError<int>(300, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 300));
            });
        }

        [TestMethod]
        public void Buffer_Count_Skip_ManOrBoy()
        {
            Run(client =>
            {
                var msgs = new[]
                {
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
                };

                var xs = client.CreateHotObservable(msgs);
                var ys = client.CreateHotObservable(msgs);

                var res = client.Start(() =>
                    xs.Buffer(3, 2).Select(w => w.Sum()).SequenceEqual(ys.Window(3, 2).SelectMany(w => w.ToList()).Where(l => l.Count(/* .Count doesn't pass allow list in remoting */) > 0).Select(l => l.Sum()))
                );

                res.Messages.AssertEqual(
                    OnNext(300, true),
                    OnCompleted<bool>(300)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 300));

                ys.Subscriptions.AssertEqual(
                    Subscribe(Increment(200, 1), 300));
            });
        }

        [TestMethod]
        public void Buffer_Time_Simple1()
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
                    xs.Buffer(TimeSpan.FromTicks(31)).Select(w => w.Sum())
                );

                res.Messages.AssertEqual(
                    OnNext(231, 1 + 2 + 3),
                    OnNext(262, 4 + 5 + 6),
                    OnNext(293, 7 + 8 + 9),
                    OnCompleted<int>(300)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 300)
                );
            });
        }

        [TestMethod]
        public void Buffer_Time_Simple2()
        {
            Run(client =>
            {
                var xs = client.CreateHotObservable<int>(
                    OnNext(210, 1),
                    OnNext(240, 4),
                    OnNext(250, 5),
                    OnNext(260, 6),
                    OnCompleted<int>(300)
                );

                var res = client.Start(() =>
                    xs.Buffer(TimeSpan.FromTicks(31)).Select(w => w.Sum())
                );

                res.Messages.AssertEqual(
                    OnNext(231, 1),
                    OnNext(262, 4 + 5 + 6),
                    OnNext(293, 0),
                    OnCompleted<int>(300)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 300)
                );
            });
        }

        [TestMethod]
        public void Buffer_Time_Error()
        {
            Run(client =>
            {
                var ex = new Exception();

                var xs = client.CreateHotObservable<int>(
                    OnNext(210, 1),
                    OnNext(240, 4),
                    OnNext(250, 5),
                    OnNext(260, 6),
                    OnError<int>(300, ex)
                );

                var res = client.Start(() =>
                    xs.Buffer(TimeSpan.FromTicks(31)).Select(w => w.Sum())
                );

                res.Messages.AssertEqual(
                    OnNext(231, 1),
                    OnNext(262, 4 + 5 + 6),
                    OnNext(293, 0),
                    OnError<int>(300, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 300)
                );
            });
        }

        [TestMethod]
        public void Buffer_Time_ManOrBoy()
        {
            Run(client =>
            {
                var msgs = new[]
                {
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
                };

                var xs = client.CreateHotObservable(msgs);
                var ys = client.CreateHotObservable(msgs);

                var res = client.Start(() =>
                    xs.Buffer(TimeSpan.FromTicks(31)).Select(w => w.Sum()).SequenceEqual(ys.Window(TimeSpan.FromTicks(31)).SelectMany(w => w.ToList()).Where(l => l.Count(/* .Count doesn't pass allow list in remoting */) > 0).Select(l => l.Sum()))
                );

                res.Messages.AssertEqual(
                    OnNext(300, true),
                    OnCompleted<bool>(300)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 300));

                ys.Subscriptions.AssertEqual(
                    Subscribe(Increment(200, 1), 300));
            });
        }

        [TestMethod]
        public void Buffer_Time_Shift_Simple1()
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
                    xs.Buffer(TimeSpan.FromTicks(31), TimeSpan.FromTicks(21)).Select(w => w.Sum())
                );

                res.Messages.AssertEqual(
                    OnNext(231, 1 + 2 + 3),
                    OnNext(252, 3 + 4 + 5),
                    OnNext(273, 5 + 6 + 7),
                    OnNext(294, 7 + 8 + 9),
                    OnNext(300, 9),
                    OnCompleted<int>(300)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 300)
                );
            });
        }

        [TestMethod]
        public void Buffer_Time_Shift_Simple2()
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
                    xs.Buffer(TimeSpan.FromTicks(21), TimeSpan.FromTicks(31)).Select(w => w.Sum())
                );

                res.Messages.AssertEqual(
                    OnNext(221, 1 + 2),
                    OnNext(252, 4 + 5),
                    OnNext(283, 7 + 8),
                    OnCompleted<int>(300)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 300)
                );
            });
        }

        [TestMethod]
        public void Buffer_Time_Shift_Error()
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

                var res = client.Start(() =>
                    xs.Buffer(TimeSpan.FromTicks(31), TimeSpan.FromTicks(21)).Select(w => w.Sum())
                );

                res.Messages.AssertEqual(
                    OnNext(231, 1 + 2 + 3),
                    OnNext(252, 3 + 4 + 5),
                    OnNext(273, 5 + 6 + 7),
                    OnNext(294, 7 + 8 + 9),
                    OnError<int>(300, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 300)
                );
            });
        }

        [TestMethod]
        public void Buffer_Time_Shift_ManOrBoy()
        {
            Run(client =>
            {
                var msgs = new[]
                {
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
                };

                var xs = client.CreateHotObservable(msgs);
                var ys = client.CreateHotObservable(msgs);

                var res = client.Start(() =>
                    xs.Buffer(TimeSpan.FromTicks(31), TimeSpan.FromTicks(21)).Select(w => w.Sum()).SequenceEqual(ys.Window(TimeSpan.FromTicks(31), TimeSpan.FromTicks(21)).SelectMany(w => w.ToList()).Where(l => l.Count(/* .Count doesn't pass allow list in remoting */) > 0).Select(l => l.Sum()))
                );

                res.Messages.AssertEqual(
                    OnNext(300, true),
                    OnCompleted<bool>(300)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 300));

                ys.Subscriptions.AssertEqual(
                    Subscribe(Increment(200, 1), 300));
            });
        }

        [TestMethod]
        public void Buffer_Ferry_Simple1()
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
                    xs.Buffer(TimeSpan.FromTicks(50), 3).Select(w => w.Sum())
                );

                res.Messages.AssertEqual(
                    OnNext(230, 1 + 2 + 3),
                    OnNext(260, 4 + 5 + 6),
                    OnNext(290, 7 + 8 + 9),
                    OnCompleted<int>(300)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 300));
            });
        }

        [TestMethod]
        public void Buffer_Ferry_Simple2()
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
                    xs.Buffer(TimeSpan.FromTicks(30), 5).Select(w => w.Sum())
                );

                res.Messages.AssertEqual(
                    OnNext(230, 1 + 2 + 3),
                    OnNext(260, 4 + 5 + 6),
                    OnNext(290, 7 + 8 + 9),
                    OnCompleted<int>(300)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 300));
            });
        }

        [TestMethod]
        public void Buffer_Ferry_Simple3()
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
                    xs.Buffer(TimeSpan.FromTicks(30), 3).Select(w => w.Sum())
                );

                res.Messages.AssertEqual(
                    OnNext(220, 1 + 2 + 3),
                    OnNext(250, 4 + 5),
                    OnNext(275, 6 + 7 + 8),
                    OnNext(300, 9),
                    OnCompleted<int>(300)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 300));
            });
        }

        [TestMethod]
        public void Buffer_Ferry_Error()
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
                    xs.Buffer(TimeSpan.FromTicks(30), 3).Select(w => w.Sum())
                );

                res.Messages.AssertEqual(
                    OnNext(220, 1 + 2 + 3),
                    OnNext(250, 4 + 5),
                    OnNext(275, 6 + 7 + 8),
                    OnError<int>(300, ex)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 300));
            });
        }

        [TestMethod]
        public void Buffer_Ferry_ManOrBoy()
        {
            Run(client =>
            {
                var msgs = new[]
                {
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
                };

                var xs = client.CreateHotObservable(msgs);
                var ys = client.CreateHotObservable(msgs);

                var res = client.Start(() =>
                    xs.Buffer(TimeSpan.FromTicks(50), 3).Select(w => w.Sum()).SequenceEqual(ys.Window(TimeSpan.FromTicks(50), 3).SelectMany(w => w.ToList()).Where(l => l.Count(/* .Count doesn't pass allow list in remoting */) > 0).Select(l => l.Sum()))
                );

                res.Messages.AssertEqual(
                    OnNext(300, true),
                    OnCompleted<bool>(300)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 300));

                ys.Subscriptions.AssertEqual(
                    Subscribe(Increment(200, 1), 300));
            });
        }

#pragma warning restore CA1829 // Use Length/Count property instead of Count() when available
#pragma warning restore IDE0079 // Remove unnecessary suppression
    }
}
