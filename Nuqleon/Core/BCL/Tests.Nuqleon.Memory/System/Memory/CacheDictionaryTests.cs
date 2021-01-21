// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 07/29/2015 - Initial work on memoization support.
//

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Memory;

namespace Tests
{
    [TestClass]
    public class CacheDictionaryTests
    {
        [TestMethod]
        public void CacheDictionary_Simple()
        {
            var cd = new CacheDictionary<int, int>(EqualityComparer<int>.Default);
            Assert.AreEqual(0, cd.Count);

            var values = cd.Values;

            Assert.AreEqual(42, cd.GetOrAdd(21, x => x * 2));
            Assert.AreEqual(1, cd.Count);
            Assert.AreEqual(1, values.Count());
            Assert.IsTrue(values.Contains(42));

            Assert.AreEqual(42, cd.GetOrAdd(21, x => x * 2));
            Assert.AreEqual(1, cd.Count);
            Assert.AreEqual(1, values.Count());
            Assert.IsTrue(values.Contains(42));

            Assert.AreEqual(44, cd.GetOrAdd(22, x => x * 2));
            Assert.AreEqual(2, cd.Count);
            Assert.AreEqual(2, values.Count());
            Assert.IsTrue(values.Contains(42) && values.Contains(44));

            Assert.IsTrue(cd.Remove(21));
            Assert.AreEqual(1, cd.Count);
            Assert.AreEqual(1, values.Count());
            Assert.IsTrue(values.Contains(44));

            Assert.IsFalse(cd.Remove(21));
            Assert.AreEqual(1, cd.Count);
            Assert.AreEqual(1, values.Count());
            Assert.IsTrue(values.Contains(44));

            Assert.IsFalse(cd.Remove(-1));
            Assert.AreEqual(1, cd.Count);
            Assert.AreEqual(1, values.Count());
            Assert.IsTrue(values.Contains(44));

            cd.Clear();
            Assert.AreEqual(0, cd.Count);
            Assert.AreEqual(0, values.Count());
        }

        [TestMethod]
        public void CacheDictionary_Null()
        {
            var cd = new CacheDictionary<string, int>(EqualityComparer<string>.Default);
            Assert.AreEqual(0, cd.Count);

            var values = cd.Values;

            Assert.AreEqual(4, cd.GetOrAdd(key: null, x => (x ?? "null").Length));
            Assert.AreEqual(1, cd.Count);
            Assert.AreEqual(1, values.Count());
            Assert.IsTrue(values.Contains(4));

            Assert.AreEqual(4, cd.GetOrAdd(key: null, x => (x ?? "null").Length));
            Assert.AreEqual(1, cd.Count);
            Assert.AreEqual(1, values.Count());
            Assert.IsTrue(values.Contains(4));

            Assert.IsTrue(cd.Remove(key: null));
            Assert.AreEqual(0, cd.Count);
            Assert.AreEqual(0, values.Count());

            cd.Clear();
            Assert.AreEqual(0, cd.Count);
            Assert.AreEqual(0, values.Count());
        }

        [TestMethod]
        public void CacheDictionary_Enumerate()
        {
            var cd = new CacheDictionary<string, int>(EqualityComparer<string>.Default);

            cd.GetOrAdd("foo", s => s == null ? 0 : s.Length);
            cd.GetOrAdd(key: null, s => s == null ? 0 : s.Length);

            var res = cd.ToArray();

            Assert.AreEqual(2, res.Length);

            Assert.IsTrue(res.Any(x => x.Key == null && x.Value == 0));
            Assert.IsTrue(res.Any(x => x.Key == "foo" && x.Value == 3));

            Assert.IsNotNull(((IEnumerable)cd).GetEnumerator());
        }
    }
}
