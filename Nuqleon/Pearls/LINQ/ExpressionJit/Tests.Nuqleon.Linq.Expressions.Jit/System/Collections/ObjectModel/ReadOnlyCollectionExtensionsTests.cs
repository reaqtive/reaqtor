// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

namespace System.Collections.ObjectModel;

[TestClass]
public class ReadOnlyCollectionExtensionsTests
{
    [TestMethod]
    public void AddFirst()
    {
        var xs = new ReadOnlyCollection<int>([3, 5, 7]);
        var res = xs.AddFirst(2);
        Assert.HasCount(4, res);
        Assert.IsTrue(new[] { 2, 3, 5, 7 }.SequenceEqual(res));
    }
}
