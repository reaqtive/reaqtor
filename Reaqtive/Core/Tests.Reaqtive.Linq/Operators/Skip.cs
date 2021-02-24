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
    public partial class Skip : OperatorTestBase
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
        public void Skip_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ((ISubscribable<int>)null).Skip(0));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => DummySubscribable<int>.Instance.Skip(-1));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummySubscribable<int>.Instance.Skip(0).Subscribe(null));

            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.Skip(default(ISubscribable<int>), TimeSpan.FromSeconds(1)));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Subscribable.Skip(DummySubscribable<int>.Instance, TimeSpan.FromSeconds(-1)));
        }

        [TestMethod]
        public void Skip_TimeSpan_Simple()
        {
            var xs = Scheduler.CreateHotObservable(
                OnNext(100, 1),
                OnNext(200, 2),
                OnNext(300, 3),
                OnNext(400, 4),
                OnNext(500, 5),
                OnCompleted<int>(600)
            );

            var res = Scheduler.Start(() => xs.Skip(new TimeSpan(150)));

            res.Messages.AssertEqual(
                OnNext(400, 4),
                OnNext(500, 5),
                OnCompleted<int>(600)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 600)
            );
        }

        [TestMethod]
        public void Skip_TimeSpan_AfterEnd()
        {
            var xs = Scheduler.CreateHotObservable(
                OnNext(100, 1),
                OnNext(200, 2),
                OnNext(300, 3),
                OnNext(400, 4),
                OnNext(500, 5),
                OnCompleted<int>(600)
            );

            var res = Scheduler.Start(() => xs.Skip(new TimeSpan(450)));

            res.Messages.AssertEqual(
                OnCompleted<int>(600)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 600)
            );
        }

        [TestMethod]
        public void Skip_MoreToSkip_SaveAndReload()
        {
            var state = Scheduler.CreateStateContainer();

            var checkpoints = new[] {
                OnSave(275, state),
                OnLoad(305, state),
            };

            var xs = Scheduler.CreateHotObservable(
                OnNext(230, 1),
                OnNext(270, 1),
                // state saved @275
                OnNext(280, 2),
                // state loaded @305
                OnNext(310, 1),
                OnNext(320, 3),
                OnCompleted<int>(400)
            );

            var res = Scheduler.Start(
                () =>
                    xs.Skip(3).Apply(Scheduler, checkpoints)
            );

            res.Messages.AssertEqual(
                OnNext(320, 3),
                OnCompleted<int>(400)
            );
        }

        [TestMethod]
        public void Skip_AlreadySkipped_SaveAndReload()
        {
            var state = Scheduler.CreateStateContainer();

            var checkpoints = new[] {
                OnSave(275, state),
                OnLoad(305, state),
            };

            var xs = Scheduler.CreateHotObservable(
                OnNext(230, 1),
                // state saved @275
                OnNext(280, 1),
                OnNext(290, 2),
                // state loaded @305
                OnNext(310, 1),
                OnNext(320, 3),
                OnCompleted<int>(400)
            );

            var res = Scheduler.Start(
                () =>
                    xs.Skip(2).Apply(Scheduler, checkpoints)
            );

            res.Messages.AssertEqual(
                OnNext(290, 2),
                OnNext(320, 3),
                OnCompleted<int>(400)
            );
        }
    }
}
