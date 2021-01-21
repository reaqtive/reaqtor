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
    public partial class Sample : OperatorTestBase
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
        public void Sample_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.Sample(default(ISubscribable<int>), TimeSpan.Zero));

            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Subscribable.Sample(DummySubscribable<int>.Instance, TimeSpan.FromSeconds(-1)));

            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.Sample(default(ISubscribable<int>), DummySubscribable<int>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.Sample(DummySubscribable<int>.Instance, default(ISubscribable<int>)));
        }

        [TestMethod]
        public void Sample_SaveAndReload()
        {
            var state = Scheduler.CreateStateContainer();

            var checkpoints = new[] {
                OnSave(275, state),
                OnLoad(290, state),
            };

            var xs = Scheduler.CreateHotObservable(
                OnNext(230, 1),
                OnNext(270, 2),
                // state saved @275
                OnNext(280, 3),
                // state loaded @290
                OnNext(310, 4),
                OnNext(320, 5),
                OnCompleted<int>(400)
            );

            var res = Scheduler.Start(
                () =>
                    xs.Sample(TimeSpan.FromTicks(50)).Apply(Scheduler, checkpoints)
            );

            res.Messages.AssertEqual(
                OnNext(250, 1),
                OnNext(300, 2),
                OnNext(350, 5),
                OnCompleted<int>(400)
            );
        }
    }
}
