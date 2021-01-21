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
    public class MemoizedDelegateTests
    {
        [TestMethod]
        public void MemoizedDelegate_Equality()
        {
            var f1 = new Func<int, int>(x => x);
            var f2 = new Func<int, int>(x => x);

            var cache1 = new MyCache();
            var cache2 = new MyCache();

            var m11 = new MemoizedDelegate<Func<int, int>>(f1, cache1);
            var m12 = new MemoizedDelegate<Func<int, int>>(f1, cache2);
            var m21 = new MemoizedDelegate<Func<int, int>>(f2, cache1);
            var m22 = new MemoizedDelegate<Func<int, int>>(f2, cache2);

            var c11 = m11;

            Assert.IsTrue(m11.Equals(m11));

            Assert.IsFalse(m11.Equals(m12));
            Assert.IsFalse(m11.Equals(m21));
            Assert.IsFalse(m11.Equals(m22));

            Assert.IsFalse(m21.Equals(m11));
            Assert.IsFalse(m21.Equals(m12));
            Assert.IsFalse(m21.Equals(m22));

            Assert.IsFalse(m22.Equals(m11));
            Assert.IsFalse(m22.Equals(m12));
            Assert.IsFalse(m22.Equals(m21));

            Assert.IsTrue(m11 == c11);
            Assert.IsFalse(m11 != c11);

            Assert.IsTrue(m11 != m12);
            Assert.IsFalse(m11 == m12);

            Assert.AreEqual(m11.GetHashCode(), c11.GetHashCode());

            Assert.IsFalse(m11.Equals(null));
            Assert.IsFalse(m11.Equals("foo"));

            Assert.IsTrue(m11.Equals(m11));
            Assert.IsFalse(m11.Equals(m12));
        }

        private sealed class MyCache : IMemoizationCache
        {
            public string DebugView => throw new NotImplementedException();

            public int Count => throw new NotImplementedException();

            public void Clear() => throw new NotImplementedException();

            public void Dispose() => throw new NotImplementedException();
        }
    }
}
