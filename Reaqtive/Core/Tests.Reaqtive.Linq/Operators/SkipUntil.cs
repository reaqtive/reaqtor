// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using Reaqtive;
using Reaqtive.Testing;
using Reaqtive.TestingFramework;

namespace Test.Reaqtive.Operators;

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

        Assert.AreEqual(500, xs.Subscriptions[0].Unsubscribe);
        Assert.AreEqual(330, ys.Subscriptions[0].Unsubscribe);

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
        Assert.AreEqual(0, remainingItemsToTake);
    }

    private static void ReadOperatorHeader(IOperatorStateReader reader)
    {
        var operatorName = reader.Read<string>();
        Assert.IsNotNull(operatorName);

        var operatorVersionMajor = reader.Read<int>();
        Assert.IsGreaterThan(0, operatorVersionMajor);

        var operatorVersionMinor = reader.Read<int>();
        Assert.IsGreaterThanOrEqualTo(0, operatorVersionMinor);

        var operatorVersionBuild = reader.Read<int>();
        Assert.IsGreaterThanOrEqualTo(0, operatorVersionBuild);

        var operatorVersionRevision = reader.Read<int>();
        Assert.IsGreaterThanOrEqualTo(0, operatorVersionRevision);
    }
}
