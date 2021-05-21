// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Reaqtive;
using Reaqtive.Testing;
using Reaqtive.TestingFramework;

namespace Test.Reaqtive.Operators
{
    [TestClass]
    public partial class DefaultIfEmpty : OperatorTestBase
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
        public void DefaultIfEmpty_SaveAndReload()
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
                OnCompleted<int>(310)
            );

            var res = Scheduler.Start(() =>
                xs.DefaultIfEmpty().Apply(Scheduler, checkpoints)
            );

            res.Messages.AssertEqual(
                OnNext(210, 9),
                OnNext(230, 13),
                OnNext(270, 7),
                OnNext(280, 1),
                // state saved @290
                OnNext(300, -1),
                // state reloaded @305
                OnCompleted<int>(310)
            );
        }
    }
}
