// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Reaqtive;

namespace Test.Reaqtive
{
    [TestClass]
    public class TinyObserverTests
    {
        [TestMethod]
        public void TinyObserver_OnNext()
        {
            var o = new O();
            o.OnNext(42);
            Assert.AreEqual(42, o.Value);
        }

        [TestMethod]
        public void TinyObserver_OnErrorThrows()
        {
            var o = new O();
            Assert.ThrowsException<InvalidOperationException>(() => o.OnError(new Exception()));
        }

        [TestMethod]
        public void TinyObserver_OnCompletedThrows()
        {
            var o = new O();
            Assert.ThrowsException<InvalidOperationException>(() => o.OnCompleted());
        }

        private sealed class O : TinyObserver<int>
        {
            public int Value;

            protected override void OnNextCore(int value)
            {
                Value = value;
            }
        }
    }
}
