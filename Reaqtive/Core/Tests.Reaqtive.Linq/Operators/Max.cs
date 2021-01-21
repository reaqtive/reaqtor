// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;

using Reaqtive;
using Reaqtive.Testing;
using Reaqtive.TestingFramework;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.Reaqtive.Operators
{
    [TestClass]
    public partial class Max : OperatorTestBase
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
        public void MaxComparer_ArgumentChecking()
        {
#pragma warning disable IDE0034 // Simplify 'default' expression (illustrative of method signature)
            Assert.ThrowsException<ArgumentNullException>(() => Subscribable.Max<string>(default(ISubscribable<string>)));

            Assert.ThrowsException<ArgumentNullException>(() => Subscribable.Max<string>(default(ISubscribable<string>), StringComparer.Ordinal));
            Assert.ThrowsException<ArgumentNullException>(() => Subscribable.Max<string>(DummySubscribable<string>.Instance, default(IComparer<string>)));
#pragma warning restore IDE0034 // Simplify 'default' expression
        }

        [TestMethod]
        public void Max_SaveAndReload_Null()
        {
            var state = Scheduler.CreateStateContainer();

            var checkpoints = new[] {
                OnSave(290, state),
                OnLoad(305, state),
            };

            var xs = Scheduler.CreateHotObservable(
                OnNext(210, "bar"),
                OnNext(230, "foo"),
                OnNext(270, "qux"),
                OnNext(280, "baz"),
                // state saved @290
                OnNext(300, "wobble"),
                // state loaded @305
                OnNext(310, "truc"),
                OnNext(340, "corge"),
                OnNext(370, "grault"),
                OnCompleted<string>(400)
            );

            var res = Scheduler.Start(() =>
                xs.Max().Apply(Scheduler, checkpoints)
            );

            res.Messages.AssertEqual(
                // state saved @290
                // state reloaded @305
                OnNext(400, "truc"),
                OnCompleted<string>(400)
            );
        }

        [TestMethod]
        public void Max_SaveAndReload()
        {
            var state = Scheduler.CreateStateContainer();

            var checkpoints = new[] {
                OnSave(290, state),
                OnLoad(305, state),
            };

            var xs = Scheduler.CreateHotObservable(
                OnNext(210, TimeSpan.FromTicks(80)),
                OnNext(230, TimeSpan.FromTicks(120)),
                OnNext(270, TimeSpan.FromTicks(110)),
                OnNext(280, TimeSpan.FromTicks(130)),
                // state saved @290
                OnNext(300, TimeSpan.FromTicks(210)),
                // state loaded @305
                OnNext(310, TimeSpan.FromTicks(70)),
                OnNext(340, TimeSpan.FromTicks(150)),
                OnNext(370, TimeSpan.FromTicks(90)),
                OnCompleted<TimeSpan>(400)
            );

            var res = Scheduler.Start(() =>
                xs.Max().Apply(Scheduler, checkpoints)
            );

            res.Messages.AssertEqual(
                // state saved @290
                // state reloaded @305
                OnNext(400, TimeSpan.FromTicks(150)),
                OnCompleted<TimeSpan>(400)
            );
        }
    }
}
