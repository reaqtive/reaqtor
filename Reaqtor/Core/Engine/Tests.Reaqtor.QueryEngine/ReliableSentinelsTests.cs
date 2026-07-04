// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using Reaqtor.QueryEngine;

namespace Tests.Reaqtor.QueryEngine;

[TestClass]
public class ReliableSentinelsTests
{
    [TestMethod]
    public void ReliableSentinels_Disposed_Throws()
    {
        var ex = Assert.ThrowsExactly<ObjectDisposedException>(() => ReliableSentinels<int>.Disposed.OnNext(0, 0L));
        Assert.AreEqual("this", ex.ObjectName);
        var ex2 = Assert.ThrowsExactly<ObjectDisposedException>(() => ReliableSentinels<int>.Disposed.OnError(new Exception()));
        Assert.AreEqual("this", ex2.ObjectName);
        var ex3 = Assert.ThrowsExactly<ObjectDisposedException>(() => ReliableSentinels<int>.Disposed.OnCompleted());
        Assert.AreEqual("this", ex3.ObjectName);
        var ex4 = Assert.ThrowsExactly<ObjectDisposedException>(() => ReliableSentinels<int>.Disposed.OnStarted());
        Assert.AreEqual("this", ex4.ObjectName);
        var ex5 = Assert.ThrowsExactly<ObjectDisposedException>(() => _ = ReliableSentinels<int>.Disposed.ResubscribeUri);
        Assert.AreEqual("this", ex5.ObjectName);
    }
}
