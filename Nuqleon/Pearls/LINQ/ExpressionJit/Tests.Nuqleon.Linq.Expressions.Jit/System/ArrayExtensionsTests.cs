// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

namespace Tests;

[TestClass]
public class ArrayExtensionsTests
{
    [TestMethod]
    public void RemoveFirst1()
    {
        var xs = new[] { 42 };
        var res = xs.RemoveFirst();
        Assert.IsEmpty(res);
        Assert.AreSame([], res);
    }

    [TestMethod]
    public void RemoveFirst2()
    {
        var xs = new[] { 42, 43 };
        var res = xs.RemoveFirst();
        Assert.HasCount(1, res);
        Assert.AreEqual(43, res[0]);
    }

    [TestMethod]
    public void RemoveFirst3()
    {
        var xs = new[] { 2, 3, 5, 7 };
        var res = xs.RemoveFirst();
        Assert.HasCount(3, res);
        Assert.IsTrue(new[] { 3, 5, 7 }.SequenceEqual(res));
    }

    [TestMethod]
    public void Map1()
    {
        var xs = Array.Empty<int>();
        var res = xs.Map(x => x * 2);
        Assert.IsEmpty(xs);
        Assert.AreSame([], res);
    }

    [TestMethod]
    public void Map2()
    {
        var xs = new[] { 1, 2, 3, 4 };
        var res = xs.Map(x => x * 2);
        Assert.HasCount(4, xs);
        Assert.IsTrue(new[] { 2, 4, 6, 8 }.SequenceEqual(res));
    }
}
