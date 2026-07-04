// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - May 2013 - Created this file.
//

using System.Collections;
using System.Linq.CompilerServices;

namespace Tests.System.Linq.CompilerServices;

[TestClass]
public class _NAryMapTests
{
    [TestMethod]
    public void NAryMap_Degenerate()
    {
        var map = new NAryMap<int, string>(1);

        map[42] = "Answer";
        Assert.AreEqual("Answer", map[42]);

        Assert.IsTrue(map.TryGetValue([42], out string answer));
        Assert.AreEqual("Answer", answer);

        Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => map[[]] = "foo");
        Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => map[42, 43] = "foo");

        Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => map[[]]);
        Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => map[42, 43]);

        Assert.IsFalse(map.TryGetValue([43], out _));
        Assert.ThrowsExactly<KeyNotFoundException>(() => map[43]);
    }

    [TestMethod]
    public void NAryMap_NonTrivial()
    {
        var map = new NAryMap<int, string>(2);

        map[19, 23] = "Answer 1";
        Assert.AreEqual("Answer 1", map[19, 23]);

        map[19, 24] = "Answer 2";
        Assert.AreEqual("Answer 2", map[19, 24]);

        Assert.IsTrue(map.TryGetValue([19, 23], out string answer1));
        Assert.AreEqual("Answer 1", answer1);

        Assert.IsTrue(map.TryGetValue([19, 24], out string answer2));
        Assert.AreEqual("Answer 2", answer2);

        Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => map[19] = "foo");
        Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => map[19, 21, 2] = "foo");

        Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => map[19]);
        Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => map[19, 21, 2]);

        Assert.IsFalse(map.TryGetValue([43], out _));
        Assert.ThrowsExactly<KeyNotFoundException>(() => map[43]);

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
        Assert.IsTrue(entries.Single(kv => kv.Key.SequenceEqual([19, 23])).Value == "Answer 1");
        Assert.IsTrue(entries.Single(kv => kv.Key.SequenceEqual([19, 24])).Value == "Answer 2");
    }
}
