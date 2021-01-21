// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

using Reaqtive;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.Reaqtive
{
    [TestClass]
    public class FaultObserverTests
    {
        [TestMethod]
        public void FaultObserver_ArgumentChecking()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new FaultObserver<int>(null));
        }

        [TestMethod]
        public void FaultObserver_Disposed()
        {
            var d = FaultObserver<int>.Disposed;

            Assert.ThrowsException<ObjectDisposedException>(() => d.OnNext(42));
            Assert.ThrowsException<ObjectDisposedException>(() => d.OnError(new Exception()));
            Assert.ThrowsException<ObjectDisposedException>(() => d.OnCompleted());
        }

        [TestMethod]
        public void FaultObserver_Basics()
        {
            var ex = new Exception();

            var d = new FaultObserver<int>(() => ex);

            AssertEx.ThrowsException<Exception>(() => d.OnNext(42), err => Assert.AreSame(ex, err));
            AssertEx.ThrowsException<Exception>(() => d.OnError(new Exception()), err => Assert.AreSame(ex, err));
            AssertEx.ThrowsException<Exception>(() => d.OnCompleted(), err => Assert.AreSame(ex, err));
        }
    }
}
