// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;

using Reaqtive;
using Reaqtive.Testing;
using Reaqtive.TestingFramework;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.Reaqtive.Operators
{
    [TestClass]
    public partial class CombineLatest : OperatorTestBase
    {
        #region Initialize

        [TestInitialize]
        public void Initialize()
        {
            base.TestInitialize();
        }

        #endregion

        #region Cleanup

        [TestCleanup]
        public void Cleanup()
        {
            base.TestCleanup();
        }

        #endregion

        #region Checkpoint tests

        [TestMethod]
        public void CombineLatest_InterleavedWithTailCheckpointed()
        {
            // Interleaving calls to the `CombineLatest` with a checkpointed load in the middle
            // of all the action.
            var state = Scheduler.CreateStateContainer();

            var checkpoints = new[] {
                OnSave(222, state),
                OnLoad(228, state),
            };

            var o1 = Scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(215, 2),
                OnNext(225, 4),
                OnCompleted<int>(230)
            );

            var o2 = Scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(220, 3),
                OnNext(230, 5),
                OnNext(235, 6),
                OnNext(240, 7),
                OnCompleted<int>(250)
            );

            var res = Scheduler.Start(() =>
                o2.CombineLatest(o1, (x, y) => x + y).Apply(Scheduler, checkpoints)
            );

            res.Messages.AssertEqual(
                OnNext(220, 2 + 3),
                OnNext(225, 3 + 4),
                OnNext(230, 2 + 5),
                OnNext(235, 2 + 6),
                OnNext(240, 2 + 7),
                OnCompleted<int>(250)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );

            o2.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void CombineLatest_ConsecutiveCheckpointed()
        {
            // Consecutive calls to the combining function with a load in the middle of the action.
            var state = Scheduler.CreateStateContainer();

            var checkpoints = new[] {
                OnSave(222, state),
                OnLoad(228, state),
            };

            var o1 = Scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(215, 2),
                OnNext(225, 4),
                OnCompleted<int>(230)
            );

            var o2 = Scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(235, 6),
                OnNext(240, 7),
                OnCompleted<int>(250)
            );

            var res = Scheduler.Start(() =>
                o2.CombineLatest(o1, (x, y) => x + y).Apply(Scheduler, checkpoints)
            );

            res.Messages.AssertEqual(
                OnNext(235, 2 + 6),
                OnNext(240, 2 + 7),
                OnCompleted<int>(250)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );

            o2.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void CombineLatest_CheckpointedNeverLoads()
        {
            // State saved but exception thrown, and never loaded.
            var state = Scheduler.CreateStateContainer();

            var checkpoints = new[] {
                OnSave(228, state),
                OnLoad(232, state),
            };

            var ex = new Exception();

            var o1 = Scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(215, 2),
                OnNext(225, 4),
                OnError<int>(230, ex),
                OnNext(235, 5)
            );

            var o2 = Scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(235, 6),
                OnNext(240, 7),
                OnCompleted<int>(250)
            );

            var res = Scheduler.Start(() =>
                o2.CombineLatest(o1, (x, y) => x + y).Apply(Scheduler, checkpoints)
            );

            res.Messages.AssertEqual(
                OnError<int>(230, ex)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );

            o2.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );
        }

        #endregion

        #region AllEmptyButOne

        [TestMethod]
        public void CombineLatest_WillNeverBeAbleToCombine2()
        {
            Run(client =>
            {
                var e0 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(250) });
                var e1 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(500, 2), OnCompleted<int>(800) });

                var res = client.Start(() =>
                    e0.CombineLatest(e1, (_0, _1) => 42)
                );

                res.Messages.AssertEqual(
                    OnCompleted<int>(500)
                );

                var i = 0;
                var es = new[] { e0, e1 };
                foreach (var e in es.Take(es.Length - 1))
                    e.Subscriptions.AssertEqual(Subscribe(200, 200 + 50 + (i++ * 10)));

                es.Last().Subscriptions.AssertEqual(Subscribe(200, 500));
            });
        }

        /// <summary>
        /// Like `CombineLatest_WillNeverBeAbleToCombine2`, except `e1` completes before `e0`.
        /// </summary>
        [TestMethod]
        public void CombineLatest_WillNeverBeAbleToCombine2Reversed()
        {
            Run(client =>
            {
                var e0 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(500, 2), OnCompleted<int>(800) });
                var e1 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(250) });

                var res = client.Start(() =>
                    e0.CombineLatest(e1, (_0, _1) => 42)
                );

                res.Messages.AssertEqual(
                    OnCompleted<int>(500)
                );

                var i = 0;
                var es = new[] { e0, e1 };
                es.First().Subscriptions.AssertEqual(Subscribe(200, 500));
                foreach (var e in es.Skip(1))
                    e.Subscriptions.AssertEqual(Subscribe(200, 200 + 50 + (i++ * 10)));
            });
        }

        [TestMethod]
        public void CombineLatest_WillNeverBeAbleToCombine3()
        {
            Run(client =>
            {
                var e0 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(250) });
                var e1 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(260) });
                var e2 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(500, 2), OnCompleted<int>(800) });

                var res = client.Start(() =>
                    e0.CombineLatest(e1, e2, (_0, _1, _2) => 42)
                );

                res.Messages.AssertEqual(
                    OnCompleted<int>(500)
                );

                var i = 0;
                var es = new[] { e0, e1, e2 };
                foreach (var e in es.Take(es.Length - 1))
                    e.Subscriptions.AssertEqual(Subscribe(200, 200 + 50 + (i++ * 10)));

                es.Last().Subscriptions.AssertEqual(Subscribe(200, 500));
            });
        }

        [TestMethod]
        public void CombineLatest_WillNeverBeAbleToCombine4()
        {
            Run(client =>
            {
                var e0 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(250) });
                var e1 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(260) });
                var e2 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(270) });
                var e3 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(500, 2), OnCompleted<int>(800) });

                var res = client.Start(() =>
                    e0.CombineLatest(e1, e2, e3, (_0, _1, _2, _3) => 42)
                );

                res.Messages.AssertEqual(
                    OnCompleted<int>(500)
                );

                var i = 0;
                var es = new[] { e0, e1, e2, e3 };
                foreach (var e in es.Take(es.Length - 1))
                    e.Subscriptions.AssertEqual(Subscribe(200, 200 + 50 + (i++ * 10)));

                es.Last().Subscriptions.AssertEqual(Subscribe(200, 500));
            });
        }

        [TestMethod]
        public void CombineLatest_WillNeverBeAbleToCombine5()
        {
            Run(client =>
            {
                var e0 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(250) });
                var e1 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(260) });
                var e2 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(270) });
                var e3 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(280) });
                var e4 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(500, 2), OnCompleted<int>(800) });

                var res = client.Start(() =>
                    e0.CombineLatest(e1, e2, e3, e4, (_0, _1, _2, _3, _4) => 42)
                );

                res.Messages.AssertEqual(
                    OnCompleted<int>(500)
                );

                var i = 0;
                var es = new[] { e0, e1, e2, e3, e4 };
                foreach (var e in es.Take(es.Length - 1))
                    e.Subscriptions.AssertEqual(Subscribe(200, 200 + 50 + (i++ * 10)));

                es.Last().Subscriptions.AssertEqual(Subscribe(200, 500));
            });
        }

        [TestMethod]
        public void CombineLatest_WillNeverBeAbleToCombine6()
        {
            Run(client =>
            {
                var e0 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(250) });
                var e1 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(260) });
                var e2 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(270) });
                var e3 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(280) });
                var e4 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(290) });
                var e5 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(500, 2), OnCompleted<int>(800) });

                var res = client.Start(() =>
                    e0.CombineLatest(e1, e2, e3, e4, e5, (_0, _1, _2, _3, _4, _5) => 42)
                );

                res.Messages.AssertEqual(
                    OnCompleted<int>(500)
                );

                var i = 0;
                var es = new[] { e0, e1, e2, e3, e4, e5 };
                foreach (var e in es.Take(es.Length - 1))
                    e.Subscriptions.AssertEqual(Subscribe(200, 200 + 50 + (i++ * 10)));

                es.Last().Subscriptions.AssertEqual(Subscribe(200, 500));
            });
        }

        [TestMethod]
        public void CombineLatest_WillNeverBeAbleToCombine7()
        {
            Run(client =>
            {
                var e0 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(250) });
                var e1 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(260) });
                var e2 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(270) });
                var e3 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(280) });
                var e4 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(290) });
                var e5 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(300) });
                var e6 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(500, 2), OnCompleted<int>(800) });

                var res = client.Start(() =>
                    e0.CombineLatest(e1, e2, e3, e4, e5, e6, (_0, _1, _2, _3, _4, _5, _6) => 42)
                );

                res.Messages.AssertEqual(
                    OnCompleted<int>(500)
                );

                var i = 0;
                var es = new[] { e0, e1, e2, e3, e4, e5, e6 };
                foreach (var e in es.Take(es.Length - 1))
                    e.Subscriptions.AssertEqual(Subscribe(200, 200 + 50 + (i++ * 10)));

                es.Last().Subscriptions.AssertEqual(Subscribe(200, 500));
            });
        }

        [TestMethod]
        public void CombineLatest_WillNeverBeAbleToCombine8()
        {
            Run(client =>
            {
                var e0 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(250) });
                var e1 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(260) });
                var e2 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(270) });
                var e3 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(280) });
                var e4 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(290) });
                var e5 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(300) });
                var e6 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(310) });
                var e7 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(500, 2), OnCompleted<int>(800) });

                var res = client.Start(() =>
                    e0.CombineLatest(e1, e2, e3, e4, e5, e6, e7, (_0, _1, _2, _3, _4, _5, _6, _7) => 42)
                );

                res.Messages.AssertEqual(
                    OnCompleted<int>(500)
                );

                var i = 0;
                var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7 };
                foreach (var e in es.Take(es.Length - 1))
                    e.Subscriptions.AssertEqual(Subscribe(200, 200 + 50 + (i++ * 10)));

                es.Last().Subscriptions.AssertEqual(Subscribe(200, 500));
            });
        }

        [TestMethod]
        public void CombineLatest_WillNeverBeAbleToCombine9()
        {
            Run(client =>
            {
                var e0 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(250) });
                var e1 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(260) });
                var e2 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(270) });
                var e3 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(280) });
                var e4 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(290) });
                var e5 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(300) });
                var e6 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(310) });
                var e7 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(320) });
                var e8 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(500, 2), OnCompleted<int>(800) });

                var res = client.Start(() =>
                    e0.CombineLatest(e1, e2, e3, e4, e5, e6, e7, e8, (_0, _1, _2, _3, _4, _5, _6, _7, _8) => 42)
                );

                res.Messages.AssertEqual(
                    OnCompleted<int>(500)
                );

                var i = 0;
                var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8 };
                foreach (var e in es.Take(es.Length - 1))
                    e.Subscriptions.AssertEqual(Subscribe(200, 200 + 50 + (i++ * 10)));

                es.Last().Subscriptions.AssertEqual(Subscribe(200, 500));
            });
        }

        [TestMethod]
        public void CombineLatest_WillNeverBeAbleToCombine10()
        {
            Run(client =>
            {
                var e0 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(250) });
                var e1 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(260) });
                var e2 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(270) });
                var e3 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(280) });
                var e4 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(290) });
                var e5 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(300) });
                var e6 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(310) });
                var e7 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(320) });
                var e8 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(330) });
                var e9 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(500, 2), OnCompleted<int>(800) });

                var res = client.Start(() =>
                    e0.CombineLatest(e1, e2, e3, e4, e5, e6, e7, e8, e9, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9) => 42)
                );

                res.Messages.AssertEqual(
                    OnCompleted<int>(500)
                );

                var i = 0;
                var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9 };
                foreach (var e in es.Take(es.Length - 1))
                    e.Subscriptions.AssertEqual(Subscribe(200, 200 + 50 + (i++ * 10)));

                es.Last().Subscriptions.AssertEqual(Subscribe(200, 500));
            });
        }

        [TestMethod]
        public void CombineLatest_WillNeverBeAbleToCombine11()
        {
            Run(client =>
            {
                var e0 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(250) });
                var e1 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(260) });
                var e2 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(270) });
                var e3 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(280) });
                var e4 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(290) });
                var e5 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(300) });
                var e6 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(310) });
                var e7 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(320) });
                var e8 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(330) });
                var e9 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(340) });
                var e10 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(500, 2), OnCompleted<int>(800) });

                var res = client.Start(() =>
                    e0.CombineLatest(e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10) => 42)
                );

                res.Messages.AssertEqual(
                    OnCompleted<int>(500)
                );

                var i = 0;
                var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10 };
                foreach (var e in es.Take(es.Length - 1))
                    e.Subscriptions.AssertEqual(Subscribe(200, 200 + 50 + (i++ * 10)));

                es.Last().Subscriptions.AssertEqual(Subscribe(200, 500));
            });
        }

        [TestMethod]
        public void CombineLatest_WillNeverBeAbleToCombine12()
        {
            Run(client =>
            {
                var e0 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(250) });
                var e1 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(260) });
                var e2 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(270) });
                var e3 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(280) });
                var e4 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(290) });
                var e5 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(300) });
                var e6 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(310) });
                var e7 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(320) });
                var e8 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(330) });
                var e9 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(340) });
                var e10 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(350) });
                var e11 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(500, 2), OnCompleted<int>(800) });

                var res = client.Start(() =>
                    e0.CombineLatest(e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11) => 42)
                );

                res.Messages.AssertEqual(
                    OnCompleted<int>(500)
                );

                var i = 0;
                var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11 };
                foreach (var e in es.Take(es.Length - 1))
                    e.Subscriptions.AssertEqual(Subscribe(200, 200 + 50 + (i++ * 10)));

                es.Last().Subscriptions.AssertEqual(Subscribe(200, 500));
            });
        }

        [TestMethod]
        public void CombineLatest_WillNeverBeAbleToCombine13()
        {
            Run(client =>
            {
                var e0 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(250) });
                var e1 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(260) });
                var e2 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(270) });
                var e3 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(280) });
                var e4 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(290) });
                var e5 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(300) });
                var e6 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(310) });
                var e7 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(320) });
                var e8 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(330) });
                var e9 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(340) });
                var e10 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(350) });
                var e11 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(360) });
                var e12 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(500, 2), OnCompleted<int>(800) });

                var res = client.Start(() =>
                    e0.CombineLatest(e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12) => 42)
                );

                res.Messages.AssertEqual(
                    OnCompleted<int>(500)
                );

                var i = 0;
                var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12 };
                foreach (var e in es.Take(es.Length - 1))
                    e.Subscriptions.AssertEqual(Subscribe(200, 200 + 50 + (i++ * 10)));

                es.Last().Subscriptions.AssertEqual(Subscribe(200, 500));
            });
        }

        [TestMethod]
        public void CombineLatest_WillNeverBeAbleToCombine14()
        {
            Run(client =>
            {
                var e0 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(250) });
                var e1 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(260) });
                var e2 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(270) });
                var e3 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(280) });
                var e4 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(290) });
                var e5 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(300) });
                var e6 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(310) });
                var e7 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(320) });
                var e8 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(330) });
                var e9 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(340) });
                var e10 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(350) });
                var e11 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(360) });
                var e12 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(370) });
                var e13 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(500, 2), OnCompleted<int>(800) });

                var res = client.Start(() =>
                    e0.CombineLatest(e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13) => 42)
                );

                res.Messages.AssertEqual(
                    OnCompleted<int>(500)
                );

                var i = 0;
                var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13 };
                foreach (var e in es.Take(es.Length - 1))
                    e.Subscriptions.AssertEqual(Subscribe(200, 200 + 50 + (i++ * 10)));

                es.Last().Subscriptions.AssertEqual(Subscribe(200, 500));
            });
        }

        [TestMethod]
        public void CombineLatest_WillNeverBeAbleToCombine15()
        {
            Run(client =>
            {
                var e0 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(250) });
                var e1 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(260) });
                var e2 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(270) });
                var e3 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(280) });
                var e4 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(290) });
                var e5 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(300) });
                var e6 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(310) });
                var e7 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(320) });
                var e8 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(330) });
                var e9 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(340) });
                var e10 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(350) });
                var e11 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(360) });
                var e12 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(370) });
                var e13 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(380) });
                var e14 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(500, 2), OnCompleted<int>(800) });

                var res = client.Start(() =>
                    e0.CombineLatest(e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14) => 42)
                );

                res.Messages.AssertEqual(
                    OnCompleted<int>(500)
                );

                var i = 0;
                var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14 };
                foreach (var e in es.Take(es.Length - 1))
                    e.Subscriptions.AssertEqual(Subscribe(200, 200 + 50 + (i++ * 10)));

                es.Last().Subscriptions.AssertEqual(Subscribe(200, 500));
            });
        }

        [TestMethod]
        public void CombineLatest_WillNeverBeAbleToCombine16()
        {
            Run(client =>
            {
                var e0 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(250) });
                var e1 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(260) });
                var e2 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(270) });
                var e3 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(280) });
                var e4 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(290) });
                var e5 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(300) });
                var e6 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(310) });
                var e7 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(320) });
                var e8 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(330) });
                var e9 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(340) });
                var e10 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(350) });
                var e11 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(360) });
                var e12 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(370) });
                var e13 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(380) });
                var e14 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(390) });
                var e15 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(500, 2), OnCompleted<int>(800) });

                var res = client.Start(() =>
                    e0.CombineLatest(e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14, e15, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15) => 42)
                );

                res.Messages.AssertEqual(
                    OnCompleted<int>(500)
                );

                var i = 0;
                var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14, e15 };
                foreach (var e in es.Take(es.Length - 1))
                    e.Subscriptions.AssertEqual(Subscribe(200, 200 + 50 + (i++ * 10)));

                es.Last().Subscriptions.AssertEqual(Subscribe(200, 500));
            });
        }


        #endregion

        #region AtLeastOneThrows

        [TestMethod]
        public void CombineLatest_AtLeastOneThrows4()
        {
            Run(client =>
            {
                var ex = new Exception();

                var e0 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400) });
                var e1 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400) });
                var e2 = client.CreateHotObservable(new[] { OnNext(150, 1), OnError<int>(230, ex) });
                var e3 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400) });

                var res = client.Start(() =>
                    e0.CombineLatest(e1, e2, e3, (_0, _1, _2, _3) => 42)
                );

                res.Messages.AssertEqual(
                    OnError<int>(230, ex)
                );

                e0.Subscriptions.AssertEqual(Subscribe(200, 230));
                e1.Subscriptions.AssertEqual(Subscribe(200, 230));
                e2.Subscriptions.AssertEqual(Subscribe(200, 230));
                e3.Subscriptions.AssertEqual(Subscribe(200, 230));
            });
        }

        #endregion

        #region ArgumentChecking

        /// <summary>
        /// Checking `null` is handled correctly as argument
        /// </summary>
        [TestMethod]
        public void CombineLatest_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ((ISubscribable<int>)null).CombineLatest<int, int, int>(DummySubscribable<int>.Instance, DummyFunc<int, int, int>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummySubscribable<int>.Instance.CombineLatest<int, int, int>(DummySubscribable<int>.Instance, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummySubscribable<int>.Instance.CombineLatest<int, int, int>(null, DummyFunc<int, int, int>.Instance));
        }

        #endregion

        #region Basics

        [TestMethod]
        public void CombineLatest_InterleavedWithTail()
        {
            Run(client =>
            {
                var o1 = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnNext(215, 2),
                    OnNext(225, 4),
                    OnCompleted<int>(230)
                );

                var o2 = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnNext(220, 3),
                    OnNext(230, 5),
                    OnNext(235, 6),
                    OnNext(240, 7),
                    OnCompleted<int>(250)
                );

                var res = client.Start(() =>
                    o2.CombineLatest(o1, (x, y) => x + y)
                );

                res.Messages.AssertEqual(
                    OnNext(220, 2 + 3),
                    OnNext(225, 3 + 4),
                    OnNext(230, 4 + 5),
                    OnNext(235, 4 + 6),
                    OnNext(240, 4 + 7),
                    OnCompleted<int>(250)
                );

                o1.Subscriptions.AssertEqual(
                    Subscribe(200, 230)
                );

                o2.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void CombineLatest_Consecutive()
        {
            Run(client =>
            {
                var o1 = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnNext(215, 2),
                    OnNext(225, 4),
                    OnCompleted<int>(230)
                );

                var o2 = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnNext(235, 6),
                    OnNext(240, 7),
                    OnCompleted<int>(250)
                );

                var res = client.Start(() =>
                    o2.CombineLatest(o1, (x, y) => x + y)
                );

                res.Messages.AssertEqual(
                    OnNext(235, 4 + 6),
                    OnNext(240, 4 + 7),
                    OnCompleted<int>(250)
                );

                o1.Subscriptions.AssertEqual(
                    Subscribe(200, 230)
                );

                o2.Subscriptions.AssertEqual(
                    Subscribe(200, 250)
                );
            });
        }

        [TestMethod]
        public void CombineLatest_ConsecutiveEndWithErrorLeft()
        {
            Run(client =>
            {
                var ex = new Exception();

                var o1 = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnNext(215, 2),
                    OnNext(225, 4),
                    OnError<int>(230, ex)
                );

                var o2 = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnNext(235, 6),
                    OnNext(240, 7),
                    OnCompleted<int>(250)
                );

                var res = client.Start(() =>
                    o2.CombineLatest(o1, (x, y) => x + y)
                );

                res.Messages.AssertEqual(
                    OnError<int>(230, ex)
                );

                o1.Subscriptions.AssertEqual(
                    Subscribe(200, 230)
                );

                o2.Subscriptions.AssertEqual(
                    Subscribe(200, 230)
                );
            });
        }

        [TestMethod]
        public void CombineLatest_ConsecutiveEndWithErrorRight()
        {
            Run(client =>
            {
                var ex = new Exception();

                var o1 = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnNext(215, 2),
                    OnNext(225, 4),
                    OnCompleted<int>(250)
                );

                var o2 = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnNext(235, 6),
                    OnNext(240, 7),
                    OnError<int>(245, ex)
                );

                var res = client.Start(() =>
                    o2.CombineLatest(o1, (x, y) => x + y)
                );

                res.Messages.AssertEqual(
                    OnNext(235, 4 + 6),
                    OnNext(240, 4 + 7),
                    OnError<int>(245, ex)
                );

                o1.Subscriptions.AssertEqual(
                    Subscribe(200, 245)
                );

                o2.Subscriptions.AssertEqual(
                    Subscribe(200, 245)
                );
            });
        }

        #endregion

        #region Empty

        [TestMethod]
        public void CombineLatest_Empty2()
        {
            Run(client =>
            {
                var e0 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(210) });
                var e1 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(220) });

                var res = client.Start(() =>
                    e0.CombineLatest(e1, (_0, _1) => 42)
                );

                res.Messages.AssertEqual(
                    OnCompleted<int>(220)
                );

                var i = 0;
                foreach (var e in new[] { e0, e1 })
                    e.Subscriptions.AssertEqual(Subscribe(200, 200 + (++i * 10)));
            });
        }

        [TestMethod]
        public void CombineLatest_Empty3()
        {
            Run(client =>
            {
                var e0 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(210) });
                var e1 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(220) });
                var e2 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(230) });

                var res = client.Start(() =>
                    e0.CombineLatest(e1, e2, (_0, _1, _2) => 42)
                );

                res.Messages.AssertEqual(
                    OnCompleted<int>(230)
                );

                var i = 0;
                foreach (var e in new[] { e0, e1, e2 })
                    e.Subscriptions.AssertEqual(Subscribe(200, 200 + (++i * 10)));
            });
        }

        [TestMethod]
        public void CombineLatest_Empty4()
        {
            Run(client =>
            {
                var e0 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(210) });
                var e1 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(220) });
                var e2 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(230) });
                var e3 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(240) });

                var res = client.Start(() =>
                    e0.CombineLatest(e1, e2, e3, (_0, _1, _2, _3) => 42)
                );

                res.Messages.AssertEqual(
                    OnCompleted<int>(240)
                );

                var i = 0;
                foreach (var e in new[] { e0, e1, e2, e3 })
                    e.Subscriptions.AssertEqual(Subscribe(200, 200 + (++i * 10)));
            });
        }

        [TestMethod]
        public void CombineLatest_Empty5()
        {
            Run(client =>
            {
                var e0 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(210) });
                var e1 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(220) });
                var e2 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(230) });
                var e3 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(240) });
                var e4 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(250) });

                var res = client.Start(() =>
                    e0.CombineLatest(e1, e2, e3, e4, (_0, _1, _2, _3, _4) => 42)
                );

                res.Messages.AssertEqual(
                    OnCompleted<int>(250)
                );

                var i = 0;
                foreach (var e in new[] { e0, e1, e2, e3, e4 })
                    e.Subscriptions.AssertEqual(Subscribe(200, 200 + (++i * 10)));
            });
        }

        [TestMethod]
        public void CombineLatest_Empty6()
        {
            Run(client =>
            {
                var e0 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(210) });
                var e1 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(220) });
                var e2 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(230) });
                var e3 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(240) });
                var e4 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(250) });
                var e5 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(260) });

                var res = client.Start(() =>
                    e0.CombineLatest(e1, e2, e3, e4, e5, (_0, _1, _2, _3, _4, _5) => 42)
                );

                res.Messages.AssertEqual(
                    OnCompleted<int>(260)
                );

                var i = 0;
                foreach (var e in new[] { e0, e1, e2, e3, e4, e5 })
                    e.Subscriptions.AssertEqual(Subscribe(200, 200 + (++i * 10)));
            });
        }

        [TestMethod]
        public void CombineLatest_Empty7()
        {
            Run(client =>
            {
                var e0 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(210) });
                var e1 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(220) });
                var e2 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(230) });
                var e3 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(240) });
                var e4 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(250) });
                var e5 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(260) });
                var e6 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(270) });

                var res = client.Start(() =>
                    e0.CombineLatest(e1, e2, e3, e4, e5, e6, (_0, _1, _2, _3, _4, _5, _6) => 42)
                );

                res.Messages.AssertEqual(
                    OnCompleted<int>(270)
                );

                var i = 0;
                foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6 })
                    e.Subscriptions.AssertEqual(Subscribe(200, 200 + (++i * 10)));
            });
        }

        [TestMethod]
        public void CombineLatest_Empty8()
        {
            Run(client =>
            {
                var e0 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(210) });
                var e1 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(220) });
                var e2 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(230) });
                var e3 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(240) });
                var e4 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(250) });
                var e5 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(260) });
                var e6 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(270) });
                var e7 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(280) });

                var res = client.Start(() =>
                    e0.CombineLatest(e1, e2, e3, e4, e5, e6, e7, (_0, _1, _2, _3, _4, _5, _6, _7) => 42)
                );

                res.Messages.AssertEqual(
                    OnCompleted<int>(280)
                );

                var i = 0;
                foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7 })
                    e.Subscriptions.AssertEqual(Subscribe(200, 200 + (++i * 10)));
            });
        }

        [TestMethod]
        public void CombineLatest_Empty9()
        {
            Run(client =>
            {
                var e0 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(210) });
                var e1 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(220) });
                var e2 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(230) });
                var e3 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(240) });
                var e4 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(250) });
                var e5 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(260) });
                var e6 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(270) });
                var e7 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(280) });
                var e8 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(290) });

                var res = client.Start(() =>
                    e0.CombineLatest(e1, e2, e3, e4, e5, e6, e7, e8, (_0, _1, _2, _3, _4, _5, _6, _7, _8) => 42)
                );

                res.Messages.AssertEqual(
                    OnCompleted<int>(290)
                );

                var i = 0;
                foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8 })
                    e.Subscriptions.AssertEqual(Subscribe(200, 200 + (++i * 10)));
            });
        }

        [TestMethod]
        public void CombineLatest_Empty10()
        {
            Run(client =>
            {
                var e0 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(210) });
                var e1 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(220) });
                var e2 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(230) });
                var e3 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(240) });
                var e4 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(250) });
                var e5 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(260) });
                var e6 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(270) });
                var e7 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(280) });
                var e8 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(290) });
                var e9 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(300) });

                var res = client.Start(() =>
                    e0.CombineLatest(e1, e2, e3, e4, e5, e6, e7, e8, e9, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9) => 42)
                );

                res.Messages.AssertEqual(
                    OnCompleted<int>(300)
                );

                var i = 0;
                foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9 })
                    e.Subscriptions.AssertEqual(Subscribe(200, 200 + (++i * 10)));
            });
        }

        [TestMethod]
        public void CombineLatest_Empty11()
        {
            Run(client =>
            {
                var e0 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(210) });
                var e1 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(220) });
                var e2 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(230) });
                var e3 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(240) });
                var e4 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(250) });
                var e5 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(260) });
                var e6 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(270) });
                var e7 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(280) });
                var e8 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(290) });
                var e9 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(300) });
                var e10 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(310) });

                var res = client.Start(() =>
                    e0.CombineLatest(e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10) => 42)
                );

                res.Messages.AssertEqual(
                    OnCompleted<int>(310)
                );

                var i = 0;
                foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10 })
                    e.Subscriptions.AssertEqual(Subscribe(200, 200 + (++i * 10)));
            });
        }

        [TestMethod]
        public void CombineLatest_Empty12()
        {
            Run(client =>
            {
                var e0 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(210) });
                var e1 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(220) });
                var e2 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(230) });
                var e3 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(240) });
                var e4 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(250) });
                var e5 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(260) });
                var e6 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(270) });
                var e7 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(280) });
                var e8 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(290) });
                var e9 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(300) });
                var e10 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(310) });
                var e11 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(320) });

                var res = client.Start(() =>
                    e0.CombineLatest(e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11) => 42)
                );

                res.Messages.AssertEqual(
                    OnCompleted<int>(320)
                );

                var i = 0;
                foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11 })
                    e.Subscriptions.AssertEqual(Subscribe(200, 200 + (++i * 10)));
            });
        }

        [TestMethod]
        public void CombineLatest_Empty13()
        {
            Run(client =>
            {
                var e0 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(210) });
                var e1 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(220) });
                var e2 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(230) });
                var e3 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(240) });
                var e4 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(250) });
                var e5 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(260) });
                var e6 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(270) });
                var e7 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(280) });
                var e8 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(290) });
                var e9 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(300) });
                var e10 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(310) });
                var e11 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(320) });
                var e12 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(330) });

                var res = client.Start(() =>
                    e0.CombineLatest(e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12) => 42)
                );

                res.Messages.AssertEqual(
                    OnCompleted<int>(330)
                );

                var i = 0;
                foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12 })
                    e.Subscriptions.AssertEqual(Subscribe(200, 200 + (++i * 10)));
            });
        }

        [TestMethod]
        public void CombineLatest_Empty14()
        {
            Run(client =>
            {
                var e0 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(210) });
                var e1 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(220) });
                var e2 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(230) });
                var e3 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(240) });
                var e4 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(250) });
                var e5 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(260) });
                var e6 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(270) });
                var e7 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(280) });
                var e8 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(290) });
                var e9 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(300) });
                var e10 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(310) });
                var e11 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(320) });
                var e12 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(330) });
                var e13 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(340) });

                var res = client.Start(() =>
                    e0.CombineLatest(e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13) => 42)
                );

                res.Messages.AssertEqual(
                    OnCompleted<int>(340)
                );

                var i = 0;
                foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13 })
                    e.Subscriptions.AssertEqual(Subscribe(200, 200 + (++i * 10)));
            });
        }

        [TestMethod]
        public void CombineLatest_Empty15()
        {
            Run(client =>
            {
                var e0 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(210) });
                var e1 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(220) });
                var e2 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(230) });
                var e3 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(240) });
                var e4 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(250) });
                var e5 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(260) });
                var e6 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(270) });
                var e7 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(280) });
                var e8 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(290) });
                var e9 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(300) });
                var e10 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(310) });
                var e11 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(320) });
                var e12 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(330) });
                var e13 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(340) });
                var e14 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(350) });

                var res = client.Start(() =>
                    e0.CombineLatest(e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14) => 42)
                );

                res.Messages.AssertEqual(
                    OnCompleted<int>(350)
                );

                var i = 0;
                foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14 })
                    e.Subscriptions.AssertEqual(Subscribe(200, 200 + (++i * 10)));
            });
        }

        [TestMethod]
        public void CombineLatest_Empty16()
        {
            Run(client =>
            {
                var e0 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(210) });
                var e1 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(220) });
                var e2 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(230) });
                var e3 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(240) });
                var e4 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(250) });
                var e5 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(260) });
                var e6 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(270) });
                var e7 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(280) });
                var e8 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(290) });
                var e9 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(300) });
                var e10 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(310) });
                var e11 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(320) });
                var e12 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(330) });
                var e13 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(340) });
                var e14 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(350) });
                var e15 = client.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(360) });

                var res = client.Start(() =>
                    e0.CombineLatest(e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14, e15, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15) => 42)
                );

                res.Messages.AssertEqual(
                    OnCompleted<int>(360)
                );

                var i = 0;
                foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14, e15 })
                    e.Subscriptions.AssertEqual(Subscribe(200, 200 + (++i * 10)));
            });
        }

        #endregion

        #region Empty/Error

        [TestMethod]
        public void CombineLatest_EmptyError()
        {
            Run(client =>
            {
                var ex = new Exception();

                var o1 = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnCompleted<int>(230)
                );

                var o2 = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnError<int>(220, ex)
                );

                var res = client.Start(() =>
                    o1.CombineLatest(o2, (x, y) => x + y)
                );

                res.Messages.AssertEqual(
                    OnError<int>(220, ex)
                );

                o1.Subscriptions.AssertEqual(
                    Subscribe(200, 220)
                );

                o2.Subscriptions.AssertEqual(
                    Subscribe(200, 220)
                );
            });
        }

        [TestMethod]
        public void CombineLatest_ErrorEmpty()
        {
            Run(client =>
            {
                var ex = new Exception();

                var e = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnCompleted<int>(230)
                );

                var f = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnError<int>(220, ex)
                );

                var res = client.Start(() =>
                    f.CombineLatest(e, (x, y) => x + y)
                );

                res.Messages.AssertEqual(
                    OnError<int>(220, ex)
                );

                e.Subscriptions.AssertEqual(
                    Subscribe(200, 220)
                );

                f.Subscriptions.AssertEqual(
                    Subscribe(200, 220)
                );
            });
        }

        #endregion

        #region Empty/Return

        [TestMethod]
        public void CombineLatest_EmptyReturn()
        {
            Run(client =>
            {
                var e = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnCompleted<int>(210)
                );

                var o = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnNext(215, 2),
                    OnCompleted<int>(220)
                );

                var res = client.Start(() =>
                    e.CombineLatest(o, (x, y) => x + y)
                );

                res.Messages.AssertEqual(
                    OnCompleted<int>(215)
                );

                e.Subscriptions.AssertEqual(
                    Subscribe(200, 210)
                );

                o.Subscriptions.AssertEqual(
                    Subscribe(200, 215)
                );
            });
        }

        [TestMethod]
        public void CombineLatest_ReturnEmpty()
        {
            Run(client =>
            {
                var e = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnCompleted<int>(210)
                );

                var o = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnNext(215, 2),
                    OnCompleted<int>(220)
                );

                var res = client.Start(() =>
                    o.CombineLatest(e, (x, y) => x + y)
                );

                res.Messages.AssertEqual(
                    OnCompleted<int>(215)
                );

                e.Subscriptions.AssertEqual(
                    Subscribe(200, 210)
                );

                o.Subscriptions.AssertEqual(
                    Subscribe(200, 215)
                );
            });
        }

        #endregion

        #region Never

        /// <summary>
        /// Checking that empty streams never combine
        /// </summary>
        [TestMethod]
        public void CombineLatest_Never2()
        {
            Run(client =>
            {
                var e0 = client.CreateHotObservable(new[] { OnNext(150, 1) });
                var e1 = client.CreateHotObservable(new[] { OnNext(150, 1) });

                var res = client.Start(() =>
                    e0.CombineLatest<int, int, int>(e1, (_0, _1) => 42)
                );

                res.Messages.AssertEqual(
                );

                foreach (var e in new[] { e0, e1 })
                    e.Subscriptions.AssertEqual(Subscribe(200, 1000));
            });
        }

        [TestMethod]
        public void CombineLatest_Never3()
        {
            Run(client =>
            {
                var e0 = client.CreateHotObservable(new[] { OnNext(150, 1) });
                var e1 = client.CreateHotObservable(new[] { OnNext(150, 1) });
                var e2 = client.CreateHotObservable(new[] { OnNext(150, 1) });

                var res = client.Start(() =>
                    e0.CombineLatest(e1, e2, (_0, _1, _2) => 42)
                );

                res.Messages.AssertEqual(
                );

                foreach (var e in new[] { e0, e1, e2 })
                    e.Subscriptions.AssertEqual(Subscribe(200, 1000));
            });
        }

        [TestMethod]
        public void CombineLatest_Never4()
        {
            Run(client =>
            {
                var e0 = client.CreateHotObservable(new[] { OnNext(150, 1) });
                var e1 = client.CreateHotObservable(new[] { OnNext(150, 1) });
                var e2 = client.CreateHotObservable(new[] { OnNext(150, 1) });
                var e3 = client.CreateHotObservable(new[] { OnNext(150, 1) });

                var res = client.Start(() =>
                    e0.CombineLatest(e1, e2, e3, (_0, _1, _2, _3) => 42)
                );

                res.Messages.AssertEqual(
                );

                foreach (var e in new[] { e0, e1, e2, e3 })
                    e.Subscriptions.AssertEqual(Subscribe(200, 1000));
            });
        }

        [TestMethod]
        public void CombineLatest_Never5()
        {
            Run(client =>
            {
                var e0 = client.CreateHotObservable(new[] { OnNext(150, 1) });
                var e1 = client.CreateHotObservable(new[] { OnNext(150, 1) });
                var e2 = client.CreateHotObservable(new[] { OnNext(150, 1) });
                var e3 = client.CreateHotObservable(new[] { OnNext(150, 1) });
                var e4 = client.CreateHotObservable(new[] { OnNext(150, 1) });

                var res = client.Start(() =>
                    e0.CombineLatest(e1, e2, e3, e4, (_0, _1, _2, _3, _4) => 42)
                );

                res.Messages.AssertEqual(
                );

                foreach (var e in new[] { e0, e1, e2, e3, e4 })
                    e.Subscriptions.AssertEqual(Subscribe(200, 1000));
            });
        }

        [TestMethod]
        public void CombineLatest_Never6()
        {
            Run(client =>
            {
                var e0 = client.CreateHotObservable(new[] { OnNext(150, 1) });
                var e1 = client.CreateHotObservable(new[] { OnNext(150, 1) });
                var e2 = client.CreateHotObservable(new[] { OnNext(150, 1) });
                var e3 = client.CreateHotObservable(new[] { OnNext(150, 1) });
                var e4 = client.CreateHotObservable(new[] { OnNext(150, 1) });
                var e5 = client.CreateHotObservable(new[] { OnNext(150, 1) });

                var res = client.Start(() =>
                    e0.CombineLatest(e1, e2, e3, e4, e5, (_0, _1, _2, _3, _4, _5) => 42)
                );

                res.Messages.AssertEqual(
                );

                foreach (var e in new[] { e0, e1, e2, e3, e4, e5 })
                    e.Subscriptions.AssertEqual(Subscribe(200, 1000));
            });
        }

        [TestMethod]
        public void CombineLatest_Never7()
        {
            Run(client =>
            {
                var e0 = client.CreateHotObservable(new[] { OnNext(150, 1) });
                var e1 = client.CreateHotObservable(new[] { OnNext(150, 1) });
                var e2 = client.CreateHotObservable(new[] { OnNext(150, 1) });
                var e3 = client.CreateHotObservable(new[] { OnNext(150, 1) });
                var e4 = client.CreateHotObservable(new[] { OnNext(150, 1) });
                var e5 = client.CreateHotObservable(new[] { OnNext(150, 1) });
                var e6 = client.CreateHotObservable(new[] { OnNext(150, 1) });

                var res = client.Start(() =>
                    e0.CombineLatest(e1, e2, e3, e4, e5, e6, (_0, _1, _2, _3, _4, _5, _6) => 42)
                );

                res.Messages.AssertEqual(
                );

                foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6 })
                    e.Subscriptions.AssertEqual(Subscribe(200, 1000));
            });
        }

        [TestMethod]
        public void CombineLatest_Never8()
        {
            Run(client =>
            {
                var e0 = client.CreateHotObservable(new[] { OnNext(150, 1) });
                var e1 = client.CreateHotObservable(new[] { OnNext(150, 1) });
                var e2 = client.CreateHotObservable(new[] { OnNext(150, 1) });
                var e3 = client.CreateHotObservable(new[] { OnNext(150, 1) });
                var e4 = client.CreateHotObservable(new[] { OnNext(150, 1) });
                var e5 = client.CreateHotObservable(new[] { OnNext(150, 1) });
                var e6 = client.CreateHotObservable(new[] { OnNext(150, 1) });
                var e7 = client.CreateHotObservable(new[] { OnNext(150, 1) });

                var res = client.Start(() =>
                    e0.CombineLatest(e1, e2, e3, e4, e5, e6, e7, (_0, _1, _2, _3, _4, _5, _6, _7) => 42)
                );

                res.Messages.AssertEqual(
                );

                foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7 })
                    e.Subscriptions.AssertEqual(Subscribe(200, 1000));
            });
        }

        [TestMethod]
        public void CombineLatest_Never9()
        {
            Run(client =>
            {
                var e0 = client.CreateHotObservable(new[] { OnNext(150, 1) });
                var e1 = client.CreateHotObservable(new[] { OnNext(150, 1) });
                var e2 = client.CreateHotObservable(new[] { OnNext(150, 1) });
                var e3 = client.CreateHotObservable(new[] { OnNext(150, 1) });
                var e4 = client.CreateHotObservable(new[] { OnNext(150, 1) });
                var e5 = client.CreateHotObservable(new[] { OnNext(150, 1) });
                var e6 = client.CreateHotObservable(new[] { OnNext(150, 1) });
                var e7 = client.CreateHotObservable(new[] { OnNext(150, 1) });
                var e8 = client.CreateHotObservable(new[] { OnNext(150, 1) });

                var res = client.Start(() =>
                    e0.CombineLatest(e1, e2, e3, e4, e5, e6, e7, e8, (_0, _1, _2, _3, _4, _5, _6, _7, _8) => 42)
                );

                res.Messages.AssertEqual(
                );

                foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8 })
                    e.Subscriptions.AssertEqual(Subscribe(200, 1000));
            });
        }

        [TestMethod]
        public void CombineLatest_Never10()
        {
            Run(client =>
            {
                var e0 = client.CreateHotObservable(new[] { OnNext(150, 1) });
                var e1 = client.CreateHotObservable(new[] { OnNext(150, 1) });
                var e2 = client.CreateHotObservable(new[] { OnNext(150, 1) });
                var e3 = client.CreateHotObservable(new[] { OnNext(150, 1) });
                var e4 = client.CreateHotObservable(new[] { OnNext(150, 1) });
                var e5 = client.CreateHotObservable(new[] { OnNext(150, 1) });
                var e6 = client.CreateHotObservable(new[] { OnNext(150, 1) });
                var e7 = client.CreateHotObservable(new[] { OnNext(150, 1) });
                var e8 = client.CreateHotObservable(new[] { OnNext(150, 1) });
                var e9 = client.CreateHotObservable(new[] { OnNext(150, 1) });

                var res = client.Start(() =>
                    e0.CombineLatest(e1, e2, e3, e4, e5, e6, e7, e8, e9, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9) => 42)
                );

                res.Messages.AssertEqual(
                );

                foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9 })
                    e.Subscriptions.AssertEqual(Subscribe(200, 1000));
            });
        }

        [TestMethod]
        public void CombineLatest_Never11()
        {
            Run(client =>
            {
                var e0 = client.CreateHotObservable(new[] { OnNext(150, 1) });
                var e1 = client.CreateHotObservable(new[] { OnNext(150, 1) });
                var e2 = client.CreateHotObservable(new[] { OnNext(150, 1) });
                var e3 = client.CreateHotObservable(new[] { OnNext(150, 1) });
                var e4 = client.CreateHotObservable(new[] { OnNext(150, 1) });
                var e5 = client.CreateHotObservable(new[] { OnNext(150, 1) });
                var e6 = client.CreateHotObservable(new[] { OnNext(150, 1) });
                var e7 = client.CreateHotObservable(new[] { OnNext(150, 1) });
                var e8 = client.CreateHotObservable(new[] { OnNext(150, 1) });
                var e9 = client.CreateHotObservable(new[] { OnNext(150, 1) });
                var e10 = client.CreateHotObservable(new[] { OnNext(150, 1) });

                var res = client.Start(() =>
                    e0.CombineLatest(e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10) => 42)
                );

                res.Messages.AssertEqual(
                );

                foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10 })
                    e.Subscriptions.AssertEqual(Subscribe(200, 1000));
            });
        }

        [TestMethod]
        public void CombineLatest_Never12()
        {
            Run(client =>
            {
                var e0 = client.CreateHotObservable(new[] { OnNext(150, 1) });
                var e1 = client.CreateHotObservable(new[] { OnNext(150, 1) });
                var e2 = client.CreateHotObservable(new[] { OnNext(150, 1) });
                var e3 = client.CreateHotObservable(new[] { OnNext(150, 1) });
                var e4 = client.CreateHotObservable(new[] { OnNext(150, 1) });
                var e5 = client.CreateHotObservable(new[] { OnNext(150, 1) });
                var e6 = client.CreateHotObservable(new[] { OnNext(150, 1) });
                var e7 = client.CreateHotObservable(new[] { OnNext(150, 1) });
                var e8 = client.CreateHotObservable(new[] { OnNext(150, 1) });
                var e9 = client.CreateHotObservable(new[] { OnNext(150, 1) });
                var e10 = client.CreateHotObservable(new[] { OnNext(150, 1) });
                var e11 = client.CreateHotObservable(new[] { OnNext(150, 1) });

                var res = client.Start(() =>
                    e0.CombineLatest(e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11) => 42)
                );

                res.Messages.AssertEqual(
                );

                foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11 })
                    e.Subscriptions.AssertEqual(Subscribe(200, 1000));
            });
        }

        [TestMethod]
        public void CombineLatest_Never13()
        {
            Run(client =>
            {
                var e0 = client.CreateHotObservable(new[] { OnNext(150, 1) });
                var e1 = client.CreateHotObservable(new[] { OnNext(150, 1) });
                var e2 = client.CreateHotObservable(new[] { OnNext(150, 1) });
                var e3 = client.CreateHotObservable(new[] { OnNext(150, 1) });
                var e4 = client.CreateHotObservable(new[] { OnNext(150, 1) });
                var e5 = client.CreateHotObservable(new[] { OnNext(150, 1) });
                var e6 = client.CreateHotObservable(new[] { OnNext(150, 1) });
                var e7 = client.CreateHotObservable(new[] { OnNext(150, 1) });
                var e8 = client.CreateHotObservable(new[] { OnNext(150, 1) });
                var e9 = client.CreateHotObservable(new[] { OnNext(150, 1) });
                var e10 = client.CreateHotObservable(new[] { OnNext(150, 1) });
                var e11 = client.CreateHotObservable(new[] { OnNext(150, 1) });
                var e12 = client.CreateHotObservable(new[] { OnNext(150, 1) });

                var res = client.Start(() =>
                    e0.CombineLatest(e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12) => 42)
                );

                res.Messages.AssertEqual(
                );

                foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12 })
                    e.Subscriptions.AssertEqual(Subscribe(200, 1000));
            });
        }

        [TestMethod]
        public void CombineLatest_Never14()
        {
            Run(client =>
            {
                var e0 = client.CreateHotObservable(new[] { OnNext(150, 1) });
                var e1 = client.CreateHotObservable(new[] { OnNext(150, 1) });
                var e2 = client.CreateHotObservable(new[] { OnNext(150, 1) });
                var e3 = client.CreateHotObservable(new[] { OnNext(150, 1) });
                var e4 = client.CreateHotObservable(new[] { OnNext(150, 1) });
                var e5 = client.CreateHotObservable(new[] { OnNext(150, 1) });
                var e6 = client.CreateHotObservable(new[] { OnNext(150, 1) });
                var e7 = client.CreateHotObservable(new[] { OnNext(150, 1) });
                var e8 = client.CreateHotObservable(new[] { OnNext(150, 1) });
                var e9 = client.CreateHotObservable(new[] { OnNext(150, 1) });
                var e10 = client.CreateHotObservable(new[] { OnNext(150, 1) });
                var e11 = client.CreateHotObservable(new[] { OnNext(150, 1) });
                var e12 = client.CreateHotObservable(new[] { OnNext(150, 1) });
                var e13 = client.CreateHotObservable(new[] { OnNext(150, 1) });

                var res = client.Start(() =>
                    e0.CombineLatest(e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13) => 42)
                );

                res.Messages.AssertEqual(
                );

                foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13 })
                    e.Subscriptions.AssertEqual(Subscribe(200, 1000));
            });
        }

        [TestMethod]
        public void CombineLatest_Never15()
        {
            Run(client =>
            {
                var e0 = client.CreateHotObservable(new[] { OnNext(150, 1) });
                var e1 = client.CreateHotObservable(new[] { OnNext(150, 1) });
                var e2 = client.CreateHotObservable(new[] { OnNext(150, 1) });
                var e3 = client.CreateHotObservable(new[] { OnNext(150, 1) });
                var e4 = client.CreateHotObservable(new[] { OnNext(150, 1) });
                var e5 = client.CreateHotObservable(new[] { OnNext(150, 1) });
                var e6 = client.CreateHotObservable(new[] { OnNext(150, 1) });
                var e7 = client.CreateHotObservable(new[] { OnNext(150, 1) });
                var e8 = client.CreateHotObservable(new[] { OnNext(150, 1) });
                var e9 = client.CreateHotObservable(new[] { OnNext(150, 1) });
                var e10 = client.CreateHotObservable(new[] { OnNext(150, 1) });
                var e11 = client.CreateHotObservable(new[] { OnNext(150, 1) });
                var e12 = client.CreateHotObservable(new[] { OnNext(150, 1) });
                var e13 = client.CreateHotObservable(new[] { OnNext(150, 1) });
                var e14 = client.CreateHotObservable(new[] { OnNext(150, 1) });

                var res = client.Start(() =>
                    e0.CombineLatest(e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14) => 42)
                );

                res.Messages.AssertEqual(
                );

                foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14 })
                    e.Subscriptions.AssertEqual(Subscribe(200, 1000));
            });
        }

        [TestMethod]
        public void CombineLatest_Never16()
        {
            Run(client =>
            {
                var e0 = client.CreateHotObservable(new[] { OnNext(150, 1) });
                var e1 = client.CreateHotObservable(new[] { OnNext(150, 1) });
                var e2 = client.CreateHotObservable(new[] { OnNext(150, 1) });
                var e3 = client.CreateHotObservable(new[] { OnNext(150, 1) });
                var e4 = client.CreateHotObservable(new[] { OnNext(150, 1) });
                var e5 = client.CreateHotObservable(new[] { OnNext(150, 1) });
                var e6 = client.CreateHotObservable(new[] { OnNext(150, 1) });
                var e7 = client.CreateHotObservable(new[] { OnNext(150, 1) });
                var e8 = client.CreateHotObservable(new[] { OnNext(150, 1) });
                var e9 = client.CreateHotObservable(new[] { OnNext(150, 1) });
                var e10 = client.CreateHotObservable(new[] { OnNext(150, 1) });
                var e11 = client.CreateHotObservable(new[] { OnNext(150, 1) });
                var e12 = client.CreateHotObservable(new[] { OnNext(150, 1) });
                var e13 = client.CreateHotObservable(new[] { OnNext(150, 1) });
                var e14 = client.CreateHotObservable(new[] { OnNext(150, 1) });
                var e15 = client.CreateHotObservable(new[] { OnNext(150, 1) });

                var res = client.Start(() =>
                    e0.CombineLatest(e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14, e15, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15) => 42)
                );

                res.Messages.AssertEqual(
                );

                foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14, e15 })
                    e.Subscriptions.AssertEqual(Subscribe(200, 1000));
            });
        }

        #endregion

        #region Never/Empty

        /// <summary>
        /// Nonterminating observer paired with observer that completes should result in nonterminated and an empty
        /// </summary>
        [TestMethod]
        public void CombineLatest_NeverEmpty()
        {
            Run(client =>
            {
                var n = client.CreateHotObservable(
                    OnNext(150, 1)
                );

                var e = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnCompleted<int>(210)
                );

                var res = client.Start(() =>
                    n.CombineLatest(e, (x, y) => x + y)
                );

                res.Messages.AssertEqual(
                );

                n.Subscriptions.AssertEqual(
                    Subscribe(200, 1000)
                );

                e.Subscriptions.AssertEqual(
                    Subscribe(200, 210)
                );
            });
        }

        /// <summary>
        /// Nonterminating observer paired with observer that completes should result in nonterminated and an empty
        /// </summary>
        [TestMethod]
        public void CombineLatest_EmptyNever()
        {
            Run(client =>
            {
                var e = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnCompleted<int>(210)
                );

                var n = client.CreateHotObservable(
                    OnNext(150, 1)
                );

                var res = client.Start(() =>
                    e.CombineLatest(n, (x, y) => x + y)
                );

                res.Messages.AssertEqual(
                );

                n.Subscriptions.AssertEqual(
                    Subscribe(200, 1000)
                );

                e.Subscriptions.AssertEqual(
                    Subscribe(200, 210)
                );
            });
        }

        #endregion

        #region Never/Return

        [TestMethod]
        public void CombineLatest_NeverReturn()
        {
            Run(client =>
            {
                var o = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnNext(215, 2),
                    OnCompleted<int>(220)
                );

                var n = client.CreateHotObservable(
                    OnNext(150, 1)
                );

                var res = client.Start(() =>
                    n.CombineLatest(o, (x, y) => x + y)
                );

                res.Messages.AssertEqual(
                );

                o.Subscriptions.AssertEqual(
                    Subscribe(200, 220)
                );

                n.Subscriptions.AssertEqual(
                    Subscribe(200, 1000)
                );
            });
        }

        [TestMethod]
        public void CombineLatest_ReturnNever()
        {
            Run(client =>
            {
                var o = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnNext(215, 2),
                    OnCompleted<int>(220)
                );

                var n = client.CreateHotObservable(
                    OnNext(150, 1)
                );

                var res = client.Start(() =>
                    o.CombineLatest(n, (x, y) => x + y)
                );

                res.Messages.AssertEqual(
                );

                o.Subscriptions.AssertEqual(
                    Subscribe(200, 220)
                );

                n.Subscriptions.AssertEqual(
                    Subscribe(200, 1000)
                );
            });
        }

        #endregion

        #region Never/Throw

        [TestMethod]
        public void CombineLatest_NeverThrow()
        {
            Run(client =>
            {
                var ex = new Exception();

                var o1 = client.CreateHotObservable(
                    OnNext(150, 1)
                );

                var o2 = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnError<int>(220, ex)
                );

                var res = client.Start(() =>
                    o1.CombineLatest(o2, (x, y) => x + y)
                );

                res.Messages.AssertEqual(
                    OnError<int>(220, ex)
                );

                o1.Subscriptions.AssertEqual(
                    Subscribe(200, 220)
                );

                o2.Subscriptions.AssertEqual(
                    Subscribe(200, 220)
                );
            });
        }

        [TestMethod]
        public void CombineLatest_ThrowNever()
        {
            Run(client =>
            {
                var ex = new Exception();

                var o1 = client.CreateHotObservable(
                    OnNext(150, 1)
                );

                var o2 = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnError<int>(220, ex)
                );

                var res = client.Start(() =>
                    o2.CombineLatest(o1, (x, y) => x + y)
                );

                res.Messages.AssertEqual(
                    OnError<int>(220, ex)
                );

                o1.Subscriptions.AssertEqual(
                    Subscribe(200, 220)
                );

                o2.Subscriptions.AssertEqual(
                    Subscribe(200, 220)
                );
            });
        }

        #endregion

        #region Return/Return

        [TestMethod]
        public void CombineLatest_ReturnReturn()
        {
            Run(client =>
            {
                var o1 = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnNext(215, 2),
                    OnCompleted<int>(230)
                );

                var o2 = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnNext(220, 3),
                    OnCompleted<int>(240)
                );

                var res = client.Start(() =>
                    o1.CombineLatest(o2, (x, y) => x + y)
                );

                res.Messages.AssertEqual(
                    OnNext(220, 2 + 3),
                    OnCompleted<int>(240)
                );

                o1.Subscriptions.AssertEqual(
                    Subscribe(200, 230)
                );

                o2.Subscriptions.AssertEqual(
                    Subscribe(200, 240)
                );
            });
        }

        #endregion

        #region Return/Throw

        [TestMethod]
        public void CombineLatest_ReturnThrow()
        {
            Run(client =>
            {
                var ex = new Exception();

                var o1 = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnNext(210, 2),
                    OnCompleted<int>(230)
                );

                var o2 = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnError<int>(220, ex)
                );

                var res = client.Start(() =>
                    o1.CombineLatest(o2, (x, y) => x + y)
                );

                res.Messages.AssertEqual(
                    OnError<int>(220, ex)
                );

                o1.Subscriptions.AssertEqual(
                    Subscribe(200, 220)
                );

                o2.Subscriptions.AssertEqual(
                    Subscribe(200, 220)
                );
            });
        }

        [TestMethod]
        public void CombineLatest_ThrowReturn()
        {
            Run(client =>
            {
                var ex = new Exception();

                var o1 = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnNext(210, 2),
                    OnCompleted<int>(230)
                );

                var o2 = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnError<int>(220, ex)
                );

                var res = client.Start(() =>
                    o2.CombineLatest(o1, (x, y) => x + y)
                );

                res.Messages.AssertEqual(
                    OnError<int>(220, ex)
                );

                o1.Subscriptions.AssertEqual(
                    Subscribe(200, 220)
                );

                o2.Subscriptions.AssertEqual(
                    Subscribe(200, 220)
                );
            });
        }

        #endregion

        #region SelectorThrows

        [TestMethod]
        public void CombineLatest_SelectorThrows()
        {
            Run(client =>
            {
                var o1 = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnNext(215, 2),
                    OnCompleted<int>(230)
                );

                var o2 = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnNext(220, 3),
                    OnCompleted<int>(240)
                );

                var ex = new Exception();

                var res = client.Start(() =>
                    o2.CombineLatest<int, int, int>(o1, (x, y) => { throw ex; })
                );

                res.Messages.AssertEqual(
                    OnError<int>(220, ex)
                );

                o1.Subscriptions.AssertEqual(
                    Subscribe(200, 220)
                );

                o2.Subscriptions.AssertEqual(
                    Subscribe(200, 220)
                );
            });
        }

        [TestMethod]
        public void CombineLatest_SelectorThrows2()
        {
            Run(client =>
            {
                var e0 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400) });
                var e1 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400) });

                var ex = new Exception();
                int f() { throw ex; }

                var res = client.Start(() =>
                    e0.CombineLatest(e1, (_0, _1) => f())
                );

                res.Messages.AssertEqual(
                    OnError<int>(220, ex)
                );

                var es = new[] { e0, e1 };
                foreach (var e in es)
                    e.Subscriptions.AssertEqual(Subscribe(200, 200 + 10 * es.Length));
            });
        }

        [TestMethod]
        public void CombineLatest_SelectorThrows3()
        {
            Run(client =>
            {
                var e0 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400) });
                var e1 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400) });
                var e2 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400) });

                var ex = new Exception();
                int f() { throw ex; }

                var res = client.Start(() =>
                    e0.CombineLatest(e1, e2, (_0, _1, _2) => f())
                );

                res.Messages.AssertEqual(
                    OnError<int>(230, ex)
                );

                var es = new[] { e0, e1, e2 };
                foreach (var e in es)
                    e.Subscriptions.AssertEqual(Subscribe(200, 200 + 10 * es.Length));
            });
        }

        [TestMethod]
        public void CombineLatest_SelectorThrows4()
        {
            Run(client =>
            {
                var e0 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400) });
                var e1 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400) });
                var e2 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400) });
                var e3 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400) });

                var ex = new Exception();
                int f() { throw ex; }

                var res = client.Start(() =>
                    e0.CombineLatest(e1, e2, e3, (_0, _1, _2, _3) => f())
                );

                res.Messages.AssertEqual(
                    OnError<int>(240, ex)
                );

                var es = new[] { e0, e1, e2, e3 };
                foreach (var e in es)
                    e.Subscriptions.AssertEqual(Subscribe(200, 200 + 10 * es.Length));
            });
        }

        [TestMethod]
        public void CombineLatest_SelectorThrows5()
        {
            Run(client =>
            {
                var e0 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400) });
                var e1 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400) });
                var e2 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400) });
                var e3 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400) });
                var e4 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(250, 5), OnCompleted<int>(400) });

                var ex = new Exception();
                int f() { throw ex; }

                var res = client.Start(() =>
                    e0.CombineLatest(e1, e2, e3, e4, (_0, _1, _2, _3, _4) => f())
                );

                res.Messages.AssertEqual(
                    OnError<int>(250, ex)
                );

                var es = new[] { e0, e1, e2, e3, e4 };
                foreach (var e in es)
                    e.Subscriptions.AssertEqual(Subscribe(200, 200 + 10 * es.Length));
            });
        }

        [TestMethod]
        public void CombineLatest_SelectorThrows6()
        {
            Run(client =>
            {
                var e0 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400) });
                var e1 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400) });
                var e2 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400) });
                var e3 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400) });
                var e4 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(250, 5), OnCompleted<int>(400) });
                var e5 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(260, 6), OnCompleted<int>(400) });

                var ex = new Exception();
                int f() { throw ex; }

                var res = client.Start(() =>
                    e0.CombineLatest(e1, e2, e3, e4, e5, (_0, _1, _2, _3, _4, _5) => f())
                );

                res.Messages.AssertEqual(
                    OnError<int>(260, ex)
                );

                var es = new[] { e0, e1, e2, e3, e4, e5 };
                foreach (var e in es)
                    e.Subscriptions.AssertEqual(Subscribe(200, 200 + 10 * es.Length));
            });
        }

        [TestMethod]
        public void CombineLatest_SelectorThrows7()
        {
            Run(client =>
            {
                var e0 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400) });
                var e1 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400) });
                var e2 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400) });
                var e3 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400) });
                var e4 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(250, 5), OnCompleted<int>(400) });
                var e5 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(260, 6), OnCompleted<int>(400) });
                var e6 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(270, 7), OnCompleted<int>(400) });

                var ex = new Exception();
                int f() { throw ex; }

                var res = client.Start(() =>
                    e0.CombineLatest(e1, e2, e3, e4, e5, e6, (_0, _1, _2, _3, _4, _5, _6) => f())
                );

                res.Messages.AssertEqual(
                    OnError<int>(270, ex)
                );

                var es = new[] { e0, e1, e2, e3, e4, e5, e6 };
                foreach (var e in es)
                    e.Subscriptions.AssertEqual(Subscribe(200, 200 + 10 * es.Length));
            });
        }

        [TestMethod]
        public void CombineLatest_SelectorThrows8()
        {
            Run(client =>
            {
                var e0 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400) });
                var e1 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400) });
                var e2 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400) });
                var e3 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400) });
                var e4 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(250, 5), OnCompleted<int>(400) });
                var e5 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(260, 6), OnCompleted<int>(400) });
                var e6 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(270, 7), OnCompleted<int>(400) });
                var e7 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(280, 8), OnCompleted<int>(400) });

                var ex = new Exception();
                int f() { throw ex; }

                var res = client.Start(() =>
                    e0.CombineLatest(e1, e2, e3, e4, e5, e6, e7, (_0, _1, _2, _3, _4, _5, _6, _7) => f())
                );

                res.Messages.AssertEqual(
                    OnError<int>(280, ex)
                );

                var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7 };
                foreach (var e in es)
                    e.Subscriptions.AssertEqual(Subscribe(200, 200 + 10 * es.Length));
            });
        }

        [TestMethod]
        public void CombineLatest_SelectorThrows9()
        {
            Run(client =>
            {
                var e0 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400) });
                var e1 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400) });
                var e2 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400) });
                var e3 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400) });
                var e4 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(250, 5), OnCompleted<int>(400) });
                var e5 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(260, 6), OnCompleted<int>(400) });
                var e6 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(270, 7), OnCompleted<int>(400) });
                var e7 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(280, 8), OnCompleted<int>(400) });
                var e8 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(290, 9), OnCompleted<int>(400) });

                var ex = new Exception();
                int f() { throw ex; }

                var res = client.Start(() =>
                    e0.CombineLatest(e1, e2, e3, e4, e5, e6, e7, e8, (_0, _1, _2, _3, _4, _5, _6, _7, _8) => f())
                );

                res.Messages.AssertEqual(
                    OnError<int>(290, ex)
                );

                var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8 };
                foreach (var e in es)
                    e.Subscriptions.AssertEqual(Subscribe(200, 200 + 10 * es.Length));
            });
        }

        [TestMethod]
        public void CombineLatest_SelectorThrows10()
        {
            Run(client =>
            {
                var e0 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400) });
                var e1 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400) });
                var e2 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400) });
                var e3 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400) });
                var e4 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(250, 5), OnCompleted<int>(400) });
                var e5 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(260, 6), OnCompleted<int>(400) });
                var e6 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(270, 7), OnCompleted<int>(400) });
                var e7 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(280, 8), OnCompleted<int>(400) });
                var e8 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(290, 9), OnCompleted<int>(400) });
                var e9 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(300, 10), OnCompleted<int>(400) });

                var ex = new Exception();
                int f() { throw ex; }

                var res = client.Start(() =>
                    e0.CombineLatest(e1, e2, e3, e4, e5, e6, e7, e8, e9, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9) => f())
                );

                res.Messages.AssertEqual(
                    OnError<int>(300, ex)
                );

                var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9 };
                foreach (var e in es)
                    e.Subscriptions.AssertEqual(Subscribe(200, 200 + 10 * es.Length));
            });
        }

        [TestMethod]
        public void CombineLatest_SelectorThrows11()
        {
            Run(client =>
            {
                var e0 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400) });
                var e1 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400) });
                var e2 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400) });
                var e3 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400) });
                var e4 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(250, 5), OnCompleted<int>(400) });
                var e5 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(260, 6), OnCompleted<int>(400) });
                var e6 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(270, 7), OnCompleted<int>(400) });
                var e7 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(280, 8), OnCompleted<int>(400) });
                var e8 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(290, 9), OnCompleted<int>(400) });
                var e9 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(300, 10), OnCompleted<int>(400) });
                var e10 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(310, 11), OnCompleted<int>(400) });

                var ex = new Exception();
                int f() { throw ex; }

                var res = client.Start(() =>
                    e0.CombineLatest(e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10) => f())
                );

                res.Messages.AssertEqual(
                    OnError<int>(310, ex)
                );

                var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10 };
                foreach (var e in es)
                    e.Subscriptions.AssertEqual(Subscribe(200, 200 + 10 * es.Length));
            });
        }

        [TestMethod]
        public void CombineLatest_SelectorThrows12()
        {
            Run(client =>
            {
                var e0 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400) });
                var e1 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400) });
                var e2 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400) });
                var e3 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400) });
                var e4 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(250, 5), OnCompleted<int>(400) });
                var e5 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(260, 6), OnCompleted<int>(400) });
                var e6 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(270, 7), OnCompleted<int>(400) });
                var e7 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(280, 8), OnCompleted<int>(400) });
                var e8 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(290, 9), OnCompleted<int>(400) });
                var e9 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(300, 10), OnCompleted<int>(400) });
                var e10 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(310, 11), OnCompleted<int>(400) });
                var e11 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(320, 12), OnCompleted<int>(400) });

                var ex = new Exception();
                int f() { throw ex; }

                var res = client.Start(() =>
                    e0.CombineLatest(e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11) => f())
                );

                res.Messages.AssertEqual(
                    OnError<int>(320, ex)
                );

                var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11 };
                foreach (var e in es)
                    e.Subscriptions.AssertEqual(Subscribe(200, 200 + 10 * es.Length));
            });
        }

        [TestMethod]
        public void CombineLatest_SelectorThrows13()
        {
            Run(client =>
            {
                var e0 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400) });
                var e1 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400) });
                var e2 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400) });
                var e3 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400) });
                var e4 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(250, 5), OnCompleted<int>(400) });
                var e5 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(260, 6), OnCompleted<int>(400) });
                var e6 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(270, 7), OnCompleted<int>(400) });
                var e7 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(280, 8), OnCompleted<int>(400) });
                var e8 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(290, 9), OnCompleted<int>(400) });
                var e9 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(300, 10), OnCompleted<int>(400) });
                var e10 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(310, 11), OnCompleted<int>(400) });
                var e11 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(320, 12), OnCompleted<int>(400) });
                var e12 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(330, 13), OnCompleted<int>(400) });

                var ex = new Exception();
                int f() { throw ex; }

                var res = client.Start(() =>
                    e0.CombineLatest(e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12) => f())
                );

                res.Messages.AssertEqual(
                    OnError<int>(330, ex)
                );

                var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12 };
                foreach (var e in es)
                    e.Subscriptions.AssertEqual(Subscribe(200, 200 + 10 * es.Length));
            });
        }

        [TestMethod]
        public void CombineLatest_SelectorThrows14()
        {
            Run(client =>
            {
                var e0 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400) });
                var e1 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400) });
                var e2 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400) });
                var e3 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400) });
                var e4 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(250, 5), OnCompleted<int>(400) });
                var e5 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(260, 6), OnCompleted<int>(400) });
                var e6 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(270, 7), OnCompleted<int>(400) });
                var e7 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(280, 8), OnCompleted<int>(400) });
                var e8 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(290, 9), OnCompleted<int>(400) });
                var e9 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(300, 10), OnCompleted<int>(400) });
                var e10 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(310, 11), OnCompleted<int>(400) });
                var e11 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(320, 12), OnCompleted<int>(400) });
                var e12 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(330, 13), OnCompleted<int>(400) });
                var e13 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(340, 14), OnCompleted<int>(400) });

                var ex = new Exception();
                int f() { throw ex; }

                var res = client.Start(() =>
                    e0.CombineLatest(e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13) => f())
                );

                res.Messages.AssertEqual(
                    OnError<int>(340, ex)
                );

                var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13 };
                foreach (var e in es)
                    e.Subscriptions.AssertEqual(Subscribe(200, 200 + 10 * es.Length));
            });
        }

        [TestMethod]
        public void CombineLatest_SelectorThrows15()
        {
            Run(client =>
            {
                var e0 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400) });
                var e1 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400) });
                var e2 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400) });
                var e3 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400) });
                var e4 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(250, 5), OnCompleted<int>(400) });
                var e5 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(260, 6), OnCompleted<int>(400) });
                var e6 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(270, 7), OnCompleted<int>(400) });
                var e7 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(280, 8), OnCompleted<int>(400) });
                var e8 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(290, 9), OnCompleted<int>(400) });
                var e9 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(300, 10), OnCompleted<int>(400) });
                var e10 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(310, 11), OnCompleted<int>(400) });
                var e11 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(320, 12), OnCompleted<int>(400) });
                var e12 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(330, 13), OnCompleted<int>(400) });
                var e13 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(340, 14), OnCompleted<int>(400) });
                var e14 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(350, 15), OnCompleted<int>(400) });

                var ex = new Exception();
                int f() { throw ex; }

                var res = client.Start(() =>
                    e0.CombineLatest(e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14) => f())
                );

                res.Messages.AssertEqual(
                    OnError<int>(350, ex)
                );

                var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14 };
                foreach (var e in es)
                    e.Subscriptions.AssertEqual(Subscribe(200, 200 + 10 * es.Length));
            });
        }

        [TestMethod]
        public void CombineLatest_SelectorThrows16()
        {
            Run(client =>
            {
                var e0 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400) });
                var e1 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400) });
                var e2 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400) });
                var e3 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400) });
                var e4 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(250, 5), OnCompleted<int>(400) });
                var e5 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(260, 6), OnCompleted<int>(400) });
                var e6 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(270, 7), OnCompleted<int>(400) });
                var e7 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(280, 8), OnCompleted<int>(400) });
                var e8 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(290, 9), OnCompleted<int>(400) });
                var e9 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(300, 10), OnCompleted<int>(400) });
                var e10 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(310, 11), OnCompleted<int>(400) });
                var e11 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(320, 12), OnCompleted<int>(400) });
                var e12 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(330, 13), OnCompleted<int>(400) });
                var e13 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(340, 14), OnCompleted<int>(400) });
                var e14 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(350, 15), OnCompleted<int>(400) });
                var e15 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(360, 16), OnCompleted<int>(400) });

                var ex = new Exception();
                int f() { throw ex; }

                var res = client.Start(() =>
                    e0.CombineLatest(e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14, e15, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15) => f())
                );

                res.Messages.AssertEqual(
                    OnError<int>(360, ex)
                );

                var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14, e15 };
                foreach (var e in es)
                    e.Subscriptions.AssertEqual(Subscribe(200, 200 + 10 * es.Length));
            });
        }

        #endregion

        #region Some/Throw

        [TestMethod]
        public void CombineLatest_SomeThrow()
        {
            Run(client =>
            {
                var ex = new Exception();

                var o1 = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnNext(215, 2),
                    OnCompleted<int>(230)
                );

                var o2 = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnError<int>(220, ex)
                );

                var res = client.Start(() =>
                    o1.CombineLatest(o2, (x, y) => x + y)
                );

                res.Messages.AssertEqual(
                    OnError<int>(220, ex)
                );

                o1.Subscriptions.AssertEqual(
                    Subscribe(200, 220)
                );

                o2.Subscriptions.AssertEqual(
                    Subscribe(200, 220)
                );
            });
        }

        [TestMethod]
        public void CombineLatest_ThrowSome()
        {
            Run(client =>
            {
                var ex = new Exception();

                var o1 = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnNext(215, 2),
                    OnCompleted<int>(230)
                );

                var o2 = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnError<int>(220, ex)
                );

                var res = client.Start(() =>
                    o2.CombineLatest(o1, (x, y) => x + y)
                );

                res.Messages.AssertEqual(
                    OnError<int>(220, ex)
                );

                o1.Subscriptions.AssertEqual(
                    Subscribe(200, 220)
                );

                o2.Subscriptions.AssertEqual(
                    Subscribe(200, 220)
                );
            });
        }

        #endregion

        #region ThrowAfterCompleted

        [TestMethod]
        public void CombineLatest_ThrowAfterCompleteLeft()
        {
            Run(client =>
            {
                var ex = new Exception();

                var o1 = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnNext(215, 2),
                    OnCompleted<int>(220)
                );

                var o2 = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnError<int>(230, ex)
                );

                var res = client.Start(() =>
                    o2.CombineLatest(o1, (x, y) => x + y)
                );

                res.Messages.AssertEqual(
                    OnError<int>(230, ex)
                );

                o1.Subscriptions.AssertEqual(
                    Subscribe(200, 220)
                );

                o2.Subscriptions.AssertEqual(
                    Subscribe(200, 230)
                );
            });
        }

        [TestMethod]
        public void CombineLatest_ThrowAfterCompleteRight()
        {
            Run(client =>
            {
                var ex = new Exception();

                var o1 = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnNext(215, 2),
                    OnCompleted<int>(220)
                );

                var o2 = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnError<int>(230, ex)
                );

                var res = client.Start(() =>
                    o1.CombineLatest(o2, (x, y) => x + y)
                );

                res.Messages.AssertEqual(
                    OnError<int>(230, ex)
                );

                o1.Subscriptions.AssertEqual(
                    Subscribe(200, 220)
                );

                o2.Subscriptions.AssertEqual(
                    Subscribe(200, 230)
                );
            });
        }

        #endregion

        #region Throw/Throw

        [TestMethod]
        public void CombineLatest_ThrowThrow()
        {
            Run(client =>
            {
                var ex1 = new Exception();
                var ex2 = new Exception();

                var o1 = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnError<int>(220, ex1)
                );

                var o2 = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnError<int>(230, ex2)
                );

                var res = client.Start(() =>
                    o2.CombineLatest(o1, (x, y) => x + y)
                );

                res.Messages.AssertEqual(
                    OnError<int>(220, ex1)
                );

                o1.Subscriptions.AssertEqual(
                    Subscribe(200, 220)
                );

                o2.Subscriptions.AssertEqual(
                    Subscribe(200, 220)
                );
            });
        }

        [TestMethod]
        public void CombineLatest_ErrorThrow()
        {
            Run(client =>
            {
                var ex1 = new Exception();
                var ex2 = new Exception();

                var o1 = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnNext(210, 2),
                    OnError<int>(220, ex1)
                );

                var o2 = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnError<int>(230, ex2)
                );

                var res = client.Start(() =>
                    o2.CombineLatest(o1, (x, y) => x + y)
                );

                res.Messages.AssertEqual(
                    OnError<int>(220, ex1)
                );

                o1.Subscriptions.AssertEqual(
                    Subscribe(200, 220)
                );

                o2.Subscriptions.AssertEqual(
                    Subscribe(200, 220)
                );
            });
        }

        [TestMethod]
        public void CombineLatest_ThrowError()
        {
            Run(client =>
            {
                var ex1 = new Exception();
                var ex2 = new Exception();

                var o1 = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnNext(210, 2),
                    OnError<int>(220, ex1)
                );

                var o2 = client.CreateHotObservable(
                    OnNext(150, 1),
                    OnError<int>(230, ex2)
                );

                var res = client.Start(() =>
                    o1.CombineLatest(o2, (x, y) => x + y)
                );

                res.Messages.AssertEqual(
                    OnError<int>(220, ex1)
                );

                o1.Subscriptions.AssertEqual(
                    Subscribe(200, 220)
                );

                o2.Subscriptions.AssertEqual(
                    Subscribe(200, 220)
                );
            });
        }

        #endregion

        #region Typical

        [TestMethod]
        public void CombineLatest_Typical2()
        {
            Run(client =>
            {
                var e0 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnNext(410, 3), OnCompleted<int>(800) });
                var e1 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnNext(420, 4), OnCompleted<int>(800) });

                var res = client.Start(() =>
                    e0.CombineLatest(e1, (_0, _1) => _0 + _1)
                );

                res.Messages.AssertEqual(
                    OnNext(220, 3),
                    OnNext(410, 5),
                    OnNext(420, 7),
                    OnCompleted<int>(800)
                );

                foreach (var e in new[] { e0, e1 })
                    e.Subscriptions.AssertEqual(Subscribe(200, 800));
            });
        }

        [TestMethod]
        public void CombineLatest_Typical3()
        {
            Run(client =>
            {
                var e0 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnNext(410, 4), OnCompleted<int>(800) });
                var e1 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnNext(420, 5), OnCompleted<int>(800) });
                var e2 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnNext(430, 6), OnCompleted<int>(800) });

                var res = client.Start(() =>
                    e0.CombineLatest(e1, e2, (_0, _1, _2) => _0 + _1 + _2)
                );

                res.Messages.AssertEqual(
                    OnNext(230, 6),
                    OnNext(410, 9),
                    OnNext(420, 12),
                    OnNext(430, 15),
                    OnCompleted<int>(800)
                );

                foreach (var e in new[] { e0, e1, e2 })
                    e.Subscriptions.AssertEqual(Subscribe(200, 800));
            });
        }

        [TestMethod]
        public void CombineLatest_Typical4()
        {
            Run(client =>
            {
                var e0 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnNext(410, 5), OnCompleted<int>(800) });
                var e1 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnNext(420, 6), OnCompleted<int>(800) });
                var e2 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnNext(430, 7), OnCompleted<int>(800) });
                var e3 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(240, 4), OnNext(440, 8), OnCompleted<int>(800) });

                var res = client.Start(() =>
                    e0.CombineLatest(e1, e2, e3, (_0, _1, _2, _3) => _0 + _1 + _2 + _3)
                );

                res.Messages.AssertEqual(
                    OnNext(240, 10),
                    OnNext(410, 14),
                    OnNext(420, 18),
                    OnNext(430, 22),
                    OnNext(440, 26),
                    OnCompleted<int>(800)
                );

                foreach (var e in new[] { e0, e1, e2, e3 })
                    e.Subscriptions.AssertEqual(Subscribe(200, 800));
            });
        }

        [TestMethod]
        public void CombineLatest_Typical5()
        {
            Run(client =>
            {
                var e0 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnNext(410, 6), OnCompleted<int>(800) });
                var e1 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnNext(420, 7), OnCompleted<int>(800) });
                var e2 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnNext(430, 8), OnCompleted<int>(800) });
                var e3 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(240, 4), OnNext(440, 9), OnCompleted<int>(800) });
                var e4 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(250, 5), OnNext(450, 10), OnCompleted<int>(800) });

                var res = client.Start(() =>
                    e0.CombineLatest(e1, e2, e3, e4, (_0, _1, _2, _3, _4) => _0 + _1 + _2 + _3 + _4)
                );

                res.Messages.AssertEqual(
                    OnNext(250, 15),
                    OnNext(410, 20),
                    OnNext(420, 25),
                    OnNext(430, 30),
                    OnNext(440, 35),
                    OnNext(450, 40),
                    OnCompleted<int>(800)
                );

                foreach (var e in new[] { e0, e1, e2, e3, e4 })
                    e.Subscriptions.AssertEqual(Subscribe(200, 800));
            });
        }

        [TestMethod]
        public void CombineLatest_Typical6()
        {
            Run(client =>
            {
                var e0 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnNext(410, 7), OnCompleted<int>(800) });
                var e1 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnNext(420, 8), OnCompleted<int>(800) });
                var e2 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnNext(430, 9), OnCompleted<int>(800) });
                var e3 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(240, 4), OnNext(440, 10), OnCompleted<int>(800) });
                var e4 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(250, 5), OnNext(450, 11), OnCompleted<int>(800) });
                var e5 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(260, 6), OnNext(460, 12), OnCompleted<int>(800) });

                var res = client.Start(() =>
                    e0.CombineLatest(e1, e2, e3, e4, e5, (_0, _1, _2, _3, _4, _5) => _0 + _1 + _2 + _3 + _4 + _5)
                );

                res.Messages.AssertEqual(
                    OnNext(260, 21),
                    OnNext(410, 27),
                    OnNext(420, 33),
                    OnNext(430, 39),
                    OnNext(440, 45),
                    OnNext(450, 51),
                    OnNext(460, 57),
                    OnCompleted<int>(800)
                );

                foreach (var e in new[] { e0, e1, e2, e3, e4, e5 })
                    e.Subscriptions.AssertEqual(Subscribe(200, 800));
            });
        }

        [TestMethod]
        public void CombineLatest_Typical7()
        {
            Run(client =>
            {
                var e0 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnNext(410, 8), OnCompleted<int>(800) });
                var e1 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnNext(420, 9), OnCompleted<int>(800) });
                var e2 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnNext(430, 10), OnCompleted<int>(800) });
                var e3 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(240, 4), OnNext(440, 11), OnCompleted<int>(800) });
                var e4 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(250, 5), OnNext(450, 12), OnCompleted<int>(800) });
                var e5 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(260, 6), OnNext(460, 13), OnCompleted<int>(800) });
                var e6 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(270, 7), OnNext(470, 14), OnCompleted<int>(800) });

                var res = client.Start(() =>
                    e0.CombineLatest(e1, e2, e3, e4, e5, e6, (_0, _1, _2, _3, _4, _5, _6) => _0 + _1 + _2 + _3 + _4 + _5 + _6)
                );

                res.Messages.AssertEqual(
                    OnNext(270, 28),
                    OnNext(410, 35),
                    OnNext(420, 42),
                    OnNext(430, 49),
                    OnNext(440, 56),
                    OnNext(450, 63),
                    OnNext(460, 70),
                    OnNext(470, 77),
                    OnCompleted<int>(800)
                );

                foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6 })
                    e.Subscriptions.AssertEqual(Subscribe(200, 800));
            });
        }

        [TestMethod]
        public void CombineLatest_Typical8()
        {
            Run(client =>
            {
                var e0 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnNext(410, 9), OnCompleted<int>(800) });
                var e1 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnNext(420, 10), OnCompleted<int>(800) });
                var e2 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnNext(430, 11), OnCompleted<int>(800) });
                var e3 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(240, 4), OnNext(440, 12), OnCompleted<int>(800) });
                var e4 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(250, 5), OnNext(450, 13), OnCompleted<int>(800) });
                var e5 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(260, 6), OnNext(460, 14), OnCompleted<int>(800) });
                var e6 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(270, 7), OnNext(470, 15), OnCompleted<int>(800) });
                var e7 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(280, 8), OnNext(480, 16), OnCompleted<int>(800) });

                var res = client.Start(() =>
                    e0.CombineLatest(e1, e2, e3, e4, e5, e6, e7, (_0, _1, _2, _3, _4, _5, _6, _7) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7)
                );

                res.Messages.AssertEqual(
                    OnNext(280, 36),
                    OnNext(410, 44),
                    OnNext(420, 52),
                    OnNext(430, 60),
                    OnNext(440, 68),
                    OnNext(450, 76),
                    OnNext(460, 84),
                    OnNext(470, 92),
                    OnNext(480, 100),
                    OnCompleted<int>(800)
                );

                foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7 })
                    e.Subscriptions.AssertEqual(Subscribe(200, 800));
            });
        }

        [TestMethod]
        public void CombineLatest_Typical9()
        {
            Run(client =>
            {
                var e0 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnNext(410, 10), OnCompleted<int>(800) });
                var e1 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnNext(420, 11), OnCompleted<int>(800) });
                var e2 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnNext(430, 12), OnCompleted<int>(800) });
                var e3 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(240, 4), OnNext(440, 13), OnCompleted<int>(800) });
                var e4 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(250, 5), OnNext(450, 14), OnCompleted<int>(800) });
                var e5 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(260, 6), OnNext(460, 15), OnCompleted<int>(800) });
                var e6 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(270, 7), OnNext(470, 16), OnCompleted<int>(800) });
                var e7 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(280, 8), OnNext(480, 17), OnCompleted<int>(800) });
                var e8 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(290, 9), OnNext(490, 18), OnCompleted<int>(800) });

                var res = client.Start(() =>
                    e0.CombineLatest(e1, e2, e3, e4, e5, e6, e7, e8, (_0, _1, _2, _3, _4, _5, _6, _7, _8) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8)
                );

                res.Messages.AssertEqual(
                    OnNext(290, 45),
                    OnNext(410, 54),
                    OnNext(420, 63),
                    OnNext(430, 72),
                    OnNext(440, 81),
                    OnNext(450, 90),
                    OnNext(460, 99),
                    OnNext(470, 108),
                    OnNext(480, 117),
                    OnNext(490, 126),
                    OnCompleted<int>(800)
                );

                foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8 })
                    e.Subscriptions.AssertEqual(Subscribe(200, 800));
            });
        }

        [TestMethod]
        public void CombineLatest_Typical10()
        {
            Run(client =>
            {
                var e0 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnNext(410, 11), OnCompleted<int>(800) });
                var e1 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnNext(420, 12), OnCompleted<int>(800) });
                var e2 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnNext(430, 13), OnCompleted<int>(800) });
                var e3 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(240, 4), OnNext(440, 14), OnCompleted<int>(800) });
                var e4 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(250, 5), OnNext(450, 15), OnCompleted<int>(800) });
                var e5 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(260, 6), OnNext(460, 16), OnCompleted<int>(800) });
                var e6 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(270, 7), OnNext(470, 17), OnCompleted<int>(800) });
                var e7 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(280, 8), OnNext(480, 18), OnCompleted<int>(800) });
                var e8 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(290, 9), OnNext(490, 19), OnCompleted<int>(800) });
                var e9 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(300, 10), OnNext(500, 20), OnCompleted<int>(800) });

                var res = client.Start(() =>
                    e0.CombineLatest(e1, e2, e3, e4, e5, e6, e7, e8, e9, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9)
                );

                res.Messages.AssertEqual(
                    OnNext(300, 55),
                    OnNext(410, 65),
                    OnNext(420, 75),
                    OnNext(430, 85),
                    OnNext(440, 95),
                    OnNext(450, 105),
                    OnNext(460, 115),
                    OnNext(470, 125),
                    OnNext(480, 135),
                    OnNext(490, 145),
                    OnNext(500, 155),
                    OnCompleted<int>(800)
                );

                foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9 })
                    e.Subscriptions.AssertEqual(Subscribe(200, 800));
            });
        }

        [TestMethod]
        public void CombineLatest_Typical11()
        {
            Run(client =>
            {
                var e0 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnNext(410, 12), OnCompleted<int>(800) });
                var e1 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnNext(420, 13), OnCompleted<int>(800) });
                var e2 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnNext(430, 14), OnCompleted<int>(800) });
                var e3 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(240, 4), OnNext(440, 15), OnCompleted<int>(800) });
                var e4 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(250, 5), OnNext(450, 16), OnCompleted<int>(800) });
                var e5 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(260, 6), OnNext(460, 17), OnCompleted<int>(800) });
                var e6 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(270, 7), OnNext(470, 18), OnCompleted<int>(800) });
                var e7 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(280, 8), OnNext(480, 19), OnCompleted<int>(800) });
                var e8 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(290, 9), OnNext(490, 20), OnCompleted<int>(800) });
                var e9 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(300, 10), OnNext(500, 21), OnCompleted<int>(800) });
                var e10 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(310, 11), OnNext(510, 22), OnCompleted<int>(800) });

                var res = client.Start(() =>
                    e0.CombineLatest(e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10)
                );

                res.Messages.AssertEqual(
                    OnNext(310, 66),
                    OnNext(410, 77),
                    OnNext(420, 88),
                    OnNext(430, 99),
                    OnNext(440, 110),
                    OnNext(450, 121),
                    OnNext(460, 132),
                    OnNext(470, 143),
                    OnNext(480, 154),
                    OnNext(490, 165),
                    OnNext(500, 176),
                    OnNext(510, 187),
                    OnCompleted<int>(800)
                );

                foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10 })
                    e.Subscriptions.AssertEqual(Subscribe(200, 800));
            });
        }

        [TestMethod]
        public void CombineLatest_Typical12()
        {
            Run(client =>
            {
                var e0 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnNext(410, 13), OnCompleted<int>(800) });
                var e1 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnNext(420, 14), OnCompleted<int>(800) });
                var e2 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnNext(430, 15), OnCompleted<int>(800) });
                var e3 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(240, 4), OnNext(440, 16), OnCompleted<int>(800) });
                var e4 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(250, 5), OnNext(450, 17), OnCompleted<int>(800) });
                var e5 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(260, 6), OnNext(460, 18), OnCompleted<int>(800) });
                var e6 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(270, 7), OnNext(470, 19), OnCompleted<int>(800) });
                var e7 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(280, 8), OnNext(480, 20), OnCompleted<int>(800) });
                var e8 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(290, 9), OnNext(490, 21), OnCompleted<int>(800) });
                var e9 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(300, 10), OnNext(500, 22), OnCompleted<int>(800) });
                var e10 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(310, 11), OnNext(510, 23), OnCompleted<int>(800) });
                var e11 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(320, 12), OnNext(520, 24), OnCompleted<int>(800) });

                var res = client.Start(() =>
                    e0.CombineLatest(e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11)
                );

                res.Messages.AssertEqual(
                    OnNext(320, 78),
                    OnNext(410, 90),
                    OnNext(420, 102),
                    OnNext(430, 114),
                    OnNext(440, 126),
                    OnNext(450, 138),
                    OnNext(460, 150),
                    OnNext(470, 162),
                    OnNext(480, 174),
                    OnNext(490, 186),
                    OnNext(500, 198),
                    OnNext(510, 210),
                    OnNext(520, 222),
                    OnCompleted<int>(800)
                );

                foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11 })
                    e.Subscriptions.AssertEqual(Subscribe(200, 800));
            });
        }

        [TestMethod]
        public void CombineLatest_Typical13()
        {
            Run(client =>
            {
                var e0 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnNext(410, 14), OnCompleted<int>(800) });
                var e1 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnNext(420, 15), OnCompleted<int>(800) });
                var e2 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnNext(430, 16), OnCompleted<int>(800) });
                var e3 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(240, 4), OnNext(440, 17), OnCompleted<int>(800) });
                var e4 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(250, 5), OnNext(450, 18), OnCompleted<int>(800) });
                var e5 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(260, 6), OnNext(460, 19), OnCompleted<int>(800) });
                var e6 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(270, 7), OnNext(470, 20), OnCompleted<int>(800) });
                var e7 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(280, 8), OnNext(480, 21), OnCompleted<int>(800) });
                var e8 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(290, 9), OnNext(490, 22), OnCompleted<int>(800) });
                var e9 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(300, 10), OnNext(500, 23), OnCompleted<int>(800) });
                var e10 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(310, 11), OnNext(510, 24), OnCompleted<int>(800) });
                var e11 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(320, 12), OnNext(520, 25), OnCompleted<int>(800) });
                var e12 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(330, 13), OnNext(530, 26), OnCompleted<int>(800) });

                var res = client.Start(() =>
                    e0.CombineLatest(e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12)
                );

                res.Messages.AssertEqual(
                    OnNext(330, 91),
                    OnNext(410, 104),
                    OnNext(420, 117),
                    OnNext(430, 130),
                    OnNext(440, 143),
                    OnNext(450, 156),
                    OnNext(460, 169),
                    OnNext(470, 182),
                    OnNext(480, 195),
                    OnNext(490, 208),
                    OnNext(500, 221),
                    OnNext(510, 234),
                    OnNext(520, 247),
                    OnNext(530, 260),
                    OnCompleted<int>(800)
                );

                foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12 })
                    e.Subscriptions.AssertEqual(Subscribe(200, 800));
            });
        }

        [TestMethod]
        public void CombineLatest_Typical14()
        {
            Run(client =>
            {
                var e0 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnNext(410, 15), OnCompleted<int>(800) });
                var e1 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnNext(420, 16), OnCompleted<int>(800) });
                var e2 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnNext(430, 17), OnCompleted<int>(800) });
                var e3 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(240, 4), OnNext(440, 18), OnCompleted<int>(800) });
                var e4 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(250, 5), OnNext(450, 19), OnCompleted<int>(800) });
                var e5 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(260, 6), OnNext(460, 20), OnCompleted<int>(800) });
                var e6 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(270, 7), OnNext(470, 21), OnCompleted<int>(800) });
                var e7 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(280, 8), OnNext(480, 22), OnCompleted<int>(800) });
                var e8 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(290, 9), OnNext(490, 23), OnCompleted<int>(800) });
                var e9 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(300, 10), OnNext(500, 24), OnCompleted<int>(800) });
                var e10 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(310, 11), OnNext(510, 25), OnCompleted<int>(800) });
                var e11 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(320, 12), OnNext(520, 26), OnCompleted<int>(800) });
                var e12 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(330, 13), OnNext(530, 27), OnCompleted<int>(800) });
                var e13 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(340, 14), OnNext(540, 28), OnCompleted<int>(800) });

                var res = client.Start(() =>
                    e0.CombineLatest(e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13)
                );

                res.Messages.AssertEqual(
                    OnNext(340, 105),
                    OnNext(410, 119),
                    OnNext(420, 133),
                    OnNext(430, 147),
                    OnNext(440, 161),
                    OnNext(450, 175),
                    OnNext(460, 189),
                    OnNext(470, 203),
                    OnNext(480, 217),
                    OnNext(490, 231),
                    OnNext(500, 245),
                    OnNext(510, 259),
                    OnNext(520, 273),
                    OnNext(530, 287),
                    OnNext(540, 301),
                    OnCompleted<int>(800)
                );

                foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13 })
                    e.Subscriptions.AssertEqual(Subscribe(200, 800));
            });
        }

        [TestMethod]
        public void CombineLatest_Typical15()
        {
            Run(client =>
            {
                var e0 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnNext(410, 16), OnCompleted<int>(800) });
                var e1 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnNext(420, 17), OnCompleted<int>(800) });
                var e2 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnNext(430, 18), OnCompleted<int>(800) });
                var e3 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(240, 4), OnNext(440, 19), OnCompleted<int>(800) });
                var e4 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(250, 5), OnNext(450, 20), OnCompleted<int>(800) });
                var e5 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(260, 6), OnNext(460, 21), OnCompleted<int>(800) });
                var e6 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(270, 7), OnNext(470, 22), OnCompleted<int>(800) });
                var e7 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(280, 8), OnNext(480, 23), OnCompleted<int>(800) });
                var e8 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(290, 9), OnNext(490, 24), OnCompleted<int>(800) });
                var e9 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(300, 10), OnNext(500, 25), OnCompleted<int>(800) });
                var e10 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(310, 11), OnNext(510, 26), OnCompleted<int>(800) });
                var e11 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(320, 12), OnNext(520, 27), OnCompleted<int>(800) });
                var e12 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(330, 13), OnNext(530, 28), OnCompleted<int>(800) });
                var e13 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(340, 14), OnNext(540, 29), OnCompleted<int>(800) });
                var e14 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(350, 15), OnNext(550, 30), OnCompleted<int>(800) });

                var res = client.Start(() =>
                    e0.CombineLatest(e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14)
                );

                res.Messages.AssertEqual(
                    OnNext(350, 120),
                    OnNext(410, 135),
                    OnNext(420, 150),
                    OnNext(430, 165),
                    OnNext(440, 180),
                    OnNext(450, 195),
                    OnNext(460, 210),
                    OnNext(470, 225),
                    OnNext(480, 240),
                    OnNext(490, 255),
                    OnNext(500, 270),
                    OnNext(510, 285),
                    OnNext(520, 300),
                    OnNext(530, 315),
                    OnNext(540, 330),
                    OnNext(550, 345),
                    OnCompleted<int>(800)
                );

                foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14 })
                    e.Subscriptions.AssertEqual(Subscribe(200, 800));
            });
        }

        [TestMethod]
        public void CombineLatest_Typical16()
        {
            Run(client =>
            {
                var e0 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnNext(410, 17), OnCompleted<int>(800) });
                var e1 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnNext(420, 18), OnCompleted<int>(800) });
                var e2 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnNext(430, 19), OnCompleted<int>(800) });
                var e3 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(240, 4), OnNext(440, 20), OnCompleted<int>(800) });
                var e4 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(250, 5), OnNext(450, 21), OnCompleted<int>(800) });
                var e5 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(260, 6), OnNext(460, 22), OnCompleted<int>(800) });
                var e6 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(270, 7), OnNext(470, 23), OnCompleted<int>(800) });
                var e7 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(280, 8), OnNext(480, 24), OnCompleted<int>(800) });
                var e8 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(290, 9), OnNext(490, 25), OnCompleted<int>(800) });
                var e9 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(300, 10), OnNext(500, 26), OnCompleted<int>(800) });
                var e10 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(310, 11), OnNext(510, 27), OnCompleted<int>(800) });
                var e11 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(320, 12), OnNext(520, 28), OnCompleted<int>(800) });
                var e12 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(330, 13), OnNext(530, 29), OnCompleted<int>(800) });
                var e13 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(340, 14), OnNext(540, 30), OnCompleted<int>(800) });
                var e14 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(350, 15), OnNext(550, 31), OnCompleted<int>(800) });
                var e15 = client.CreateHotObservable(new[] { OnNext(150, 1), OnNext(360, 16), OnNext(560, 32), OnCompleted<int>(800) });

                var res = client.Start(() =>
                    e0.CombineLatest(e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14, e15, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14 + _15)
                );

                res.Messages.AssertEqual(
                    OnNext(360, 136),
                    OnNext(410, 152),
                    OnNext(420, 168),
                    OnNext(430, 184),
                    OnNext(440, 200),
                    OnNext(450, 216),
                    OnNext(460, 232),
                    OnNext(470, 248),
                    OnNext(480, 264),
                    OnNext(490, 280),
                    OnNext(500, 296),
                    OnNext(510, 312),
                    OnNext(520, 328),
                    OnNext(530, 344),
                    OnNext(540, 360),
                    OnNext(550, 376),
                    OnNext(560, 392),
                    OnCompleted<int>(800)
                );

                foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14, e15 })
                    e.Subscriptions.AssertEqual(Subscribe(200, 800));
            });
        }

        #endregion
    }
}
