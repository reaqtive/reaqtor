// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

using Reaqtor.QueryEngine;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Reaqtor.QueryEngine
{
    [TestClass]
    public class ReliableSentinelsTests
    {
        [TestMethod]
        public void ReliableSentinels_Disposed_Throws()
        {
            AssertEx.ThrowsException<ObjectDisposedException>(() => ReliableSentinels<int>.Disposed.OnNext(0, 0L), ex => Assert.AreEqual("this", ex.ObjectName));
            AssertEx.ThrowsException<ObjectDisposedException>(() => ReliableSentinels<int>.Disposed.OnError(new Exception()), ex => Assert.AreEqual("this", ex.ObjectName));
            AssertEx.ThrowsException<ObjectDisposedException>(() => ReliableSentinels<int>.Disposed.OnCompleted(), ex => Assert.AreEqual("this", ex.ObjectName));
            AssertEx.ThrowsException<ObjectDisposedException>(() => ReliableSentinels<int>.Disposed.OnStarted(), ex => Assert.AreEqual("this", ex.ObjectName));
            AssertEx.ThrowsException<ObjectDisposedException>(() => _ = ReliableSentinels<int>.Disposed.ResubscribeUri, ex => Assert.AreEqual("this", ex.ObjectName));
        }
    }
}
