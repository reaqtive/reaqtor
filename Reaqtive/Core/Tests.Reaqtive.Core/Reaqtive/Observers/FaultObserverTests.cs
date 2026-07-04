// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Reaqtive;

namespace Test.Reaqtive;

[TestClass]
public class FaultObserverTests
{
    [TestMethod]
    public void FaultObserver_ArgumentChecking()
    {
        Assert.ThrowsExactly<ArgumentNullException>(() => new FaultObserver<int>(null));
    }

    [TestMethod]
    public void FaultObserver_Disposed()
    {
        var d = FaultObserver<int>.Disposed;

        Assert.ThrowsExactly<ObjectDisposedException>(() => d.OnNext(42));
        Assert.ThrowsExactly<ObjectDisposedException>(() => d.OnError(new Exception()));
        Assert.ThrowsExactly<ObjectDisposedException>(() => d.OnCompleted());
    }

    [TestMethod]
    public void FaultObserver_Basics()
    {
        var ex = new Exception();

        var d = new FaultObserver<int>(() => ex);

        var err = Assert.ThrowsExactly<Exception>(() => d.OnNext(42));
        Assert.AreSame(ex, err);
        var err2 = Assert.ThrowsExactly<Exception>(() => d.OnError(new Exception()));
        Assert.AreSame(ex, err2);
        var err3 = Assert.ThrowsExactly<Exception>(() => d.OnCompleted());
        Assert.AreSame(ex, err3);
    }
}
