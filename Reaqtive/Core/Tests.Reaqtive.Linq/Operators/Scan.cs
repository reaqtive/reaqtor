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
    public partial class Scan : OperatorTestBase
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
        public void Scan_ArgumentChecking()
        {
#pragma warning disable IDE0034 // Simplify 'default' expression (illustrative of method signature)
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.Scan<int>(default(ISubscribable<int>), (l, r) => l + r));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.Scan<int>(DummySubscribable<int>.Instance, default(Func<int, int, int>)));

            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.Scan<int, int>(default(ISubscribable<int>), 0, (l, r) => l + r));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.Scan<int, int>(DummySubscribable<int>.Instance, 0, default(Func<int, int, int>)));
            var nullSeedAllowed = Subscribable.Scan<int, string>(DummySubscribable<int>.Instance, default(string), (s, x) => s + x);
#pragma warning restore IDE0034 // Simplify 'default' expression
        }

        [TestMethod]
        public void Scan_SaveAndReload1()
        {
            var state = Scheduler.CreateStateContainer();

            var checkpoints = new[] {
                OnSave(290, state),
                OnLoad(305, state),
            };

            var xs = Scheduler.CreateHotObservable(
                OnNext(210, 2),
                OnNext(230, 3),
                OnNext(270, 5),
                OnNext(280, 7),
                // state saved @290
                OnNext(300, 11),
                // state loaded @305
                OnNext(310, 13),
                OnNext(340, 17),
                OnNext(370, 19),
                OnCompleted<int>(400)
            );

            var res = Scheduler.Start(() =>
                xs.Scan((x, y) => x + y).Apply(Scheduler, checkpoints)
            );

            res.Messages.AssertEqual(
                // state saved @290
                // state reloaded @305
                OnNext(230, 2 + 3),
                OnNext(270, 2 + 3 + 5),
                OnNext(280, 2 + 3 + 5 + 7),
                OnNext(300, 2 + 3 + 5 + 7 + 11),
                OnNext(310, 2 + 3 + 5 + 7 + 13),
                OnNext(340, 2 + 3 + 5 + 7 + 13 + 17),
                OnNext(370, 2 + 3 + 5 + 7 + 13 + 17 + 19),
                OnCompleted<int>(400)
            );
        }

        [TestMethod]
        public void Scan_SaveAndReload2()
        {
            var state = Scheduler.CreateStateContainer();

            var checkpoints = new[] {
                OnSave(290, state),
                OnLoad(305, state),
            };

            var xs = Scheduler.CreateHotObservable(
                OnNext(210, 2),
                OnNext(230, 3),
                OnNext(270, 5),
                OnNext(280, 7),
                // state saved @290
                OnNext(300, 11),
                // state loaded @305
                OnNext(310, 13),
                OnNext(340, 17),
                OnNext(370, 19),
                OnCompleted<int>(400)
            );

            var res = Scheduler.Start(() =>
                xs.Scan(0, (x, y) => x + y).Apply(Scheduler, checkpoints)
            );

            res.Messages.AssertEqual(
                // state saved @290
                // state reloaded @305
                OnNext(210, 0 + 2),
                OnNext(230, 0 + 2 + 3),
                OnNext(270, 0 + 2 + 3 + 5),
                OnNext(280, 0 + 2 + 3 + 5 + 7),
                OnNext(300, 0 + 2 + 3 + 5 + 7 + 11),
                OnNext(310, 0 + 2 + 3 + 5 + 7 + 13),
                OnNext(340, 0 + 2 + 3 + 5 + 7 + 13 + 17),
                OnNext(370, 0 + 2 + 3 + 5 + 7 + 13 + 17 + 19),
                OnCompleted<int>(400)
            );
        }
    }
}
