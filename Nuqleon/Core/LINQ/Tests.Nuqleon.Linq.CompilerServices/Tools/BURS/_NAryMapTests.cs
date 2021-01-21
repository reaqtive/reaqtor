// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - May 2013 - Created this file.
//

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.CompilerServices;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.System.Linq.CompilerServices
{
    [TestClass]
    public class _NAryMapTests
    {
        [TestMethod]
        public void NAryMap_Degenerate()
        {
            var map = new NAryMap<int, string>(1);

            map[42] = "Answer";
            Assert.AreEqual("Answer", map[42]);

            Assert.IsTrue(map.TryGetValue(new[] { 42 }, out string answer));
            Assert.AreEqual("Answer", answer);

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => map[Array.Empty<int>()] = "foo");
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => map[42, 43] = "foo");

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => map[Array.Empty<int>()]);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => map[42, 43]);

            Assert.IsFalse(map.TryGetValue(new[] { 43 }, out _));
            Assert.ThrowsException<KeyNotFoundException>(() => map[43]);
        }

        [TestMethod]
        public void NAryMap_NonTrivial()
        {
            var map = new NAryMap<int, string>(2);

            map[19, 23] = "Answer 1";
            Assert.AreEqual("Answer 1", map[19, 23]);

            map[19, 24] = "Answer 2";
            Assert.AreEqual("Answer 2", map[19, 24]);

            Assert.IsTrue(map.TryGetValue(new[] { 19, 23 }, out string answer1));
            Assert.AreEqual("Answer 1", answer1);

            Assert.IsTrue(map.TryGetValue(new[] { 19, 24 }, out string answer2));
            Assert.AreEqual("Answer 2", answer2);

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => map[19] = "foo");
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => map[19, 21, 2] = "foo");

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => map[19]);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => map[19, 21, 2]);

            Assert.IsFalse(map.TryGetValue(new[] { 43 }, out _));
            Assert.ThrowsException<KeyNotFoundException>(() => map[43]);

            Assert.IsTrue(map.TryGetValue(19, out NAryMapOrLeaf<int, string> partial));
            Assert.IsNotNull(partial.Map);
            Assert.IsNull(partial.Leaf);

            Assert.IsTrue(partial.Map.TryGetValue(23, out NAryMapOrLeaf<int, string> leaf1));
            Assert.IsNull(leaf1.Map);
            Assert.IsNotNull(leaf1.Leaf);
            Assert.AreEqual("Answer 1", leaf1.Leaf);

            Assert.IsTrue(partial.Map.TryGetValue(24, out NAryMapOrLeaf<int, string> leaf2));
            Assert.IsNull(leaf2.Map);
            Assert.IsNotNull(leaf2.Leaf);
            Assert.AreEqual("Answer 2", leaf2.Leaf);

            var entries = new List<KeyValuePair<IEnumerable<int>, string>>();
            foreach (KeyValuePair<IEnumerable<int>, string> kv in ((IEnumerable)map))
            {
                entries.Add(kv);
            }

            Assert.AreEqual(2, entries.Count);
            Assert.IsTrue(entries.Single(kv => kv.Key.SequenceEqual(new[] { 19, 23 })).Value == "Answer 1");
            Assert.IsTrue(entries.Single(kv => kv.Key.SequenceEqual(new[] { 19, 24 })).Value == "Answer 2");
        }
    }
}
