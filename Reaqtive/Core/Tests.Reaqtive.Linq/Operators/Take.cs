// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

using Reaqtive;
using Reaqtive.Testing;
using Reaqtive.TestingFramework;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.Reaqtive.Operators
{
    [TestClass]
    public partial class Take : OperatorTestBase
    {
        [TestInitialize]
        public void Initialize()
        {
            base.TestInitialize();
        }

        [TestCleanup]
        public void Cleanup()
        {
            base.TestCleanup();
        }

        [TestMethod]
        public void Take_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ((ISubscribable<int>)null).Take(0));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => DummySubscribable<int>.Instance.Take(-1));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummySubscribable<int>.Instance.Take(1).Subscribe(null));

            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.Take(default(ISubscribable<int>), TimeSpan.FromSeconds(1)));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Subscribable.Take(DummySubscribable<int>.Instance, TimeSpan.FromSeconds(-1)));
        }

        [TestMethod]
        public void Take_SaveAndReload1()
        {
            var state = Scheduler.CreateStateContainer();

            var checkpoints = new[] {
                OnSave(290, state),
                OnLoad(305, state),
            };

            var xs = Scheduler.CreateHotObservable(
                OnNext(210, 9),
                OnNext(230, 13),
                OnNext(270, 7),
                OnNext(280, 1),
                // state saved @290
                OnNext(300, -1),
                // state loaded @305
                OnNext(310, 3),
                OnNext(340, 8),
                OnNext(370, 11),
                OnCompleted<int>(400)
            );

            var res = Scheduler.Start(() =>
                xs.Take(6).Apply(Scheduler, checkpoints)
            );

            res.Messages.AssertEqual(
                OnNext(210, 9),
                OnNext(230, 13),
                OnNext(270, 7),
                OnNext(280, 1),
                // state saved @290
                OnNext(300, -1),
                // state reloaded @305
                OnNext(310, 3),
                OnNext(340, 8),
                OnCompleted<int>(340)
            );
        }

        [TestMethod]
        public void Take_SaveAndReload2()
        {
            var state = Scheduler.CreateStateContainer();

            var xs = Scheduler.CreateHotObservable(
                OnNext(210, 2),
                OnNext(230, 3),
                OnNext(270, 5),
                OnNext(280, 7),
                // state saved @290
                OnNext(300, 11),
                // state loaded @310
                OnNext(320, 13)
            );

            //
            // ------------------------------------- Save -------------------------------------
            //

            var cp1 = new[] {
                OnSave(290, state),
            };

            var res1 = Scheduler.Start(() =>
                xs.Take(3).Apply(Scheduler, cp1), 100, 200, 1000
            );

            res1.Messages.AssertEqual(
                OnNext(210, 2),
                OnNext(230, 3),
                OnNext(270, 5),
                OnCompleted<int>(270) //
                                      // state saved @290
            );

            //
            // ------------------------------------- Load -------------------------------------
            //

            var res2 = Scheduler.Start(() =>
                xs.Take(3), 300, 310, 1000, state
            );

            res2.Messages.AssertEqual(
            // state reloaded @310 (including disposed flag)
            );
        }
    }
}
