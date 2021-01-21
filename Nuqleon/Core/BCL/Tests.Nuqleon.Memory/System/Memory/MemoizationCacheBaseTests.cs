// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 07/29/2015 - Initial work on memoization support.
//

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Memory;

namespace Tests
{
    [TestClass]
    public class MemoizationCacheBaseTests
    {
        [TestMethod]
        public void MemoizationCacheBase_Simple()
        {
            var cleared = false;
            var disposing = false;

            var c = new MyCache
            {
                CountF = () => 42,
                DebugViewF = () => "foo",
                ClearA = b => { Assert.AreEqual(disposing, b); cleared = true; },
                GetOrAddCoreF = x => x + 1,
            };

            Assert.AreEqual(42, c.Count);
            Assert.AreEqual("foo", c.DebugView);
            Assert.AreEqual(2, c.GetOrAdd(1));

            cleared = false;
            disposing = false;
            c.Clear();
            Assert.IsTrue(cleared);

            cleared = false;
            disposing = true;
            c.Dispose();
            Assert.IsTrue(cleared);

            Assert.AreEqual(0, c.Count);
            Assert.AreNotEqual("foo", c.DebugView);

            Assert.ThrowsException<ObjectDisposedException>(() => c.GetOrAdd(2));
        }

        [TestMethod]
        public void SynchronizedMemoizationCacheBase_Simple()
        {
            var cleared = false;
            var disposing = false;

            var c = new MySynchronizedCache
            {
                CountF = () => 42,
                DebugViewF = () => "foo",
                ClearA = b => { Assert.AreEqual(disposing, b); cleared = true; },
                GetOrAddCoreF = x => x + 1,
            };

            Assert.AreEqual(42, c.Count);
            Assert.AreEqual("foo", c.DebugView);
            Assert.AreEqual(2, c.GetOrAdd(1));

            cleared = false;
            disposing = false;
            c.Clear();
            Assert.IsTrue(cleared);

            cleared = false;
            disposing = true;
            c.Dispose();
            Assert.IsTrue(cleared);

            Assert.AreEqual(0, c.Count);
            Assert.AreNotEqual("foo", c.DebugView);

            Assert.ThrowsException<ObjectDisposedException>(() => c.GetOrAdd(2));
        }

        private sealed class MyCache : MemoizationCacheBase<int, int>
        {
            public Func<int> CountF;
            public Func<string> DebugViewF;
            public Action<bool> ClearA;
            public Func<int, int> GetOrAddCoreF;

            protected override int CountCore => CountF();

            protected override string DebugViewCore => DebugViewF();

            protected override void ClearCore(bool disposing) => ClearA(disposing);

            protected override int GetOrAddCore(int argument) => GetOrAddCoreF(argument);
        }

        private sealed class MySynchronizedCache : SynchronizedMemoizationCacheBase<int, int>
        {
            public Func<int> CountF;
            public Func<string> DebugViewF;
            public Action<bool> ClearA;
            public Func<int, int> GetOrAddCoreF;

            protected override int CountCore => CountF();

            protected override string DebugViewCore => DebugViewF();

            protected override object SyncRoot => this;

            protected override void ClearCore(bool disposing) => ClearA(disposing);

            protected override int GetOrAddCore(int argument) => GetOrAddCoreF(argument);
        }
    }
}
