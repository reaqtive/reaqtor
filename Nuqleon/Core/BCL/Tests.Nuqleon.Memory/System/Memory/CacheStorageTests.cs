// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   ER - 10/27/2014 - Wrote these tests.
//

using System;
using System.Memory;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.System.Memory
{
    [TestClass]
    public class CacheStorageTests
    {
        [TestMethod]
        public void CacheStorage_ArgumentChecks()
        {
            var cacheStorage = new CacheStorage<string>();
            AssertEx.ThrowsException<ArgumentNullException>(() => new CacheStorage<string>(comparer: null), ex => Assert.AreEqual("comparer", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => cacheStorage.GetEntry(value: null), ex => Assert.AreEqual("value", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => cacheStorage.ReleaseEntry(entry: null), ex => Assert.AreEqual("entry", ex.ParamName));
            AssertEx.ThrowsException<ArgumentException>(() => cacheStorage.ReleaseEntry(new NullEntry()), ex => Assert.AreEqual("entry", ex.ParamName));
        }

        [TestMethod]
        public void CacheStorage_Count()
        {
            var cacheStorage = new CacheStorage<string>();
            Assert.AreEqual(0, cacheStorage.Count);

            var e1 = cacheStorage.GetEntry("foo");
            var e2 = cacheStorage.GetEntry("bar");
            Assert.AreEqual(2, cacheStorage.Count);

            cacheStorage.ReleaseEntry(e1);
            cacheStorage.ReleaseEntry(e2);
            Assert.AreEqual(0, cacheStorage.Count);
        }

        private sealed class NullEntry : IReference<string>
        {
            public string Value => null;
        }
    }
}
