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
            var ex = Assert.ThrowsExactly<ArgumentNullException>(() => new CacheStorage<string>(comparer: null));
            Assert.AreEqual("comparer", ex.ParamName);
            var ex2 = Assert.ThrowsExactly<ArgumentNullException>(() => cacheStorage.GetEntry(value: null));
            Assert.AreEqual("value", ex2.ParamName);
            var ex3 = Assert.ThrowsExactly<ArgumentNullException>(() => cacheStorage.ReleaseEntry(entry: null));
            Assert.AreEqual("entry", ex3.ParamName);
            var ex4 = Assert.ThrowsExactly<ArgumentException>(() => cacheStorage.ReleaseEntry(new NullEntry()));
            Assert.AreEqual("entry", ex4.ParamName);
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
