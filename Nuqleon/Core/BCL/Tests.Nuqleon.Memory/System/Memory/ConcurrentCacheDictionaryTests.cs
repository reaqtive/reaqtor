// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 07/29/2015 - Initial work on memoization support.
//

using System.Collections;
using System.Memory;

namespace Tests;

[TestClass]
public class ConcurrentCacheDictionaryTests
{
    [TestMethod]
    public void ConcurrentCacheDictionary_Simple()
    {
        var cd = new ConcurrentCacheDictionary<int, int>(EqualityComparer<int>.Default);
        Assert.AreEqual(0, cd.Count);

        var values = cd.Values;

        Assert.AreEqual(42, cd.GetOrAdd(21, x => x * 2));
        Assert.AreEqual(1, cd.Count);
        Assert.HasCount(1, values);
        Assert.Contains(42, values);

        Assert.AreEqual(42, cd.GetOrAdd(21, x => x * 2));
        Assert.AreEqual(1, cd.Count);
        Assert.HasCount(1, values);
        Assert.Contains(42, values);

        Assert.AreEqual(44, cd.GetOrAdd(22, x => x * 2));
        Assert.AreEqual(2, cd.Count);
        Assert.HasCount(2, values);
        Assert.IsTrue(values.Contains(42) && values.Contains(44));

        Assert.IsTrue(cd.Remove(21));
        Assert.AreEqual(1, cd.Count);
        Assert.HasCount(1, values);
        Assert.Contains(44, values);

        Assert.IsFalse(cd.Remove(21));
        Assert.AreEqual(1, cd.Count);
        Assert.HasCount(1, values);
        Assert.Contains(44, values);

        Assert.IsFalse(cd.Remove(-1));
        Assert.AreEqual(1, cd.Count);
        Assert.HasCount(1, values);
        Assert.Contains(44, values);

        cd.Clear();
        Assert.AreEqual(0, cd.Count);
        Assert.IsEmpty(values);
    }

    [TestMethod]
    public void ConcurrentCacheDictionary_Null()
    {
        var cd = new ConcurrentCacheDictionary<string, int>(EqualityComparer<string>.Default);
        Assert.AreEqual(0, cd.Count);

        var values = cd.Values;

        Assert.AreEqual(4, cd.GetOrAdd(key: null, x => (x ?? "null").Length));
        Assert.AreEqual(1, cd.Count);
        Assert.HasCount(1, values);
        Assert.Contains(4, values);

        Assert.AreEqual(4, cd.GetOrAdd(key: null, x => (x ?? "null").Length));
        Assert.AreEqual(1, cd.Count);
        Assert.HasCount(1, values);
        Assert.Contains(4, values);

        Assert.IsTrue(cd.Remove(key: null));
        Assert.AreEqual(0, cd.Count);
        Assert.IsEmpty(values);

        cd.Clear();
        Assert.AreEqual(0, cd.Count);
        Assert.IsEmpty(values);
    }

    [TestMethod]
    public void ConcurrentCacheDictionary_Enumerate()
    {
        var cd = new ConcurrentCacheDictionary<string, int>(EqualityComparer<string>.Default);

        cd.GetOrAdd("foo", s => s == null ? 0 : s.Length);
        cd.GetOrAdd(key: null, s => s == null ? 0 : s.Length);

        var res = cd.ToArray();

        Assert.HasCount(2, res);

        Assert.Contains(x => x.Key == null && x.Value == 0, res);
        Assert.Contains(x => x.Key == "foo" && x.Value == 3, res);

        Assert.IsNotNull(((IEnumerable)cd).GetEnumerator());
    }
}
