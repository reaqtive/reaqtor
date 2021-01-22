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
    public partial class SkipUntil : OperatorTestBase
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
        public void SkipUntil_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.SkipUntil<int, int>(null, DummySubscribable<int>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.SkipUntil<int, int>(DummySubscribable<int>.Instance, null));

            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.SkipUntil(default(ISubscribable<int>), DateTimeOffset.Now));
        }

        [TestMethod]
        public void SkipUntil_CheckpointStateWrittenCorrectlyAfterDispose()
        {
            // Check that state is written correctly after we dispose of the other
            // subscription we are given.
            var state = Scheduler.CreateStateContainer();

            var checkpoints = new[] {
                OnSave(340, state),
            };

            var xs = Scheduler.CreateHotObservable(
                OnNext(310, 1),
                OnNext(350, 4),
                OnNext(370, 5),
                OnCompleted<int>(500)
                );

            var ys = Scheduler.CreateHotObservable(
                OnNext(330, 2),
                OnCompleted<int>(425)
            );

            var res = Scheduler.Start(() =>
                xs.SkipUntil(ys).Apply(Scheduler, checkpoints));

            Assert.AreEqual(xs.Subscriptions[0].Unsubscribe, 500);
            Assert.AreEqual(ys.Subscriptions[0].Unsubscribe, 330);

            res.Messages.AssertEqual(
                OnNext(350, 4),
                OnNext(370, 5),
                OnCompleted<int>(500));

            var reader = state.CreateReader().Create(null);

            ReadOperatorHeader(reader);

            var skipUntilIsDisposed = reader.Read<bool>();
            var otherObserverHasSignaled = reader.Read<bool>();
            var firstSubscriptionIsDisposed = reader.Read<bool>();

            ReadOperatorHeader(reader);

            var takeIsDisposed = reader.Read<bool>();
            var remainingItemsToTake = reader.Read<int>();

            Assert.IsFalse(skipUntilIsDisposed);
            Assert.IsTrue(otherObserverHasSignaled);
            Assert.IsFalse(firstSubscriptionIsDisposed);
            Assert.IsTrue(takeIsDisposed);
            Assert.AreEqual(remainingItemsToTake, 0);
        }

        private static void ReadOperatorHeader(IOperatorStateReader reader)
        {
            var operatorName = reader.Read<string>();
            Assert.IsNotNull(operatorName);

            var operatorVersionMajor = reader.Read<int>();
            Assert.IsTrue(operatorVersionMajor > 0);

            var operatorVersionMinor = reader.Read<int>();
            Assert.IsTrue(operatorVersionMinor >= 0);

            var operatorVersionBuild = reader.Read<int>();
            Assert.IsTrue(operatorVersionBuild >= 0);

            var operatorVersionRevision = reader.Read<int>();
            Assert.IsTrue(operatorVersionRevision >= 0);
        }
    }
}
