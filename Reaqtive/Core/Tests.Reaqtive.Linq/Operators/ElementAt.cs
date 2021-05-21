// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Reaqtive;
using Reaqtive.Testing;
using Reaqtive.TestingFramework;

namespace Test.Reaqtive.Operators
{
    [TestClass]
    public partial class ElementAt : OperatorTestBase
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

        #region ElementAt

        [TestMethod]
        public void ElementAt_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.ElementAt(default(ISubscribable<int>), 0));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Subscribable.ElementAt(DummySubscribable<int>.Instance, -1));
        }

        [TestMethod]
        public void ElementAt_SaveAndReload()
        {
            var state = Scheduler.CreateStateContainer();

            var checkpoints = new[] {
                OnSave(240, state),
                OnLoad(320, state),
            };

            var xs = Scheduler.CreateHotObservable(
                OnNext(230, 1),
                // OnSave
                OnNext(280, 0),
                OnNext(290, 1),
                OnNext(310, 0),
                // OnLoad
                OnNext(330, 1),
                OnNext(340, 0),
                OnNext(350, 1),
                OnNext(360, 0),
                OnNext(370, 1),
                OnNext(380, 0),
                OnNext(390, 1),
                OnCompleted<int>(400)
            );

            var res = Scheduler.Start(() =>
                xs.ElementAt(4).Apply(Scheduler, checkpoints)
            );

            res.Messages.AssertEqual(
                OnNext(360, 0),
                OnCompleted<int>(360)
            );
        }

        #endregion

        #region ElementAtOrDefault

        [TestMethod]
        public void ElementAtOrDefault_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.ElementAtOrDefault(default(ISubscribable<int>), 0));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Subscribable.ElementAtOrDefault(DummySubscribable<int>.Instance, -1));
        }

        [TestMethod]
        public void ElementAtOrDefault_SaveAndReload()
        {
            var state = Scheduler.CreateStateContainer();

            var checkpoints = new[] {
                OnSave(240, state),
                OnLoad(320, state),
            };

            var xs = Scheduler.CreateHotObservable(
                OnNext(230, 1),
                // OnSave
                OnNext(280, 0),
                OnNext(290, 1),
                OnNext(310, 0),
                // OnLoad
                OnNext(330, 1),
                OnNext(340, 0),
                OnNext(350, 1),
                OnNext(360, 0),
                OnNext(370, 1),
                OnNext(380, 0),
                OnNext(390, 1),
                OnCompleted<int>(400)
            );

            var res = Scheduler.Start(() =>
                xs.ElementAtOrDefault(4).Apply(Scheduler, checkpoints)
            );

            res.Messages.AssertEqual(
                OnNext(360, 0),
                OnCompleted<int>(360)
            );
        }

        #endregion
    }
}
