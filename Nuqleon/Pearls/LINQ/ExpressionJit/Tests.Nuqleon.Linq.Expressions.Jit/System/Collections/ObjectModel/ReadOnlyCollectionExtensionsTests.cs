// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace System.Collections.ObjectModel
{
    [TestClass]
    public class ReadOnlyCollectionExtensionsTests
    {
        [TestMethod]
        public void AddFirst()
        {
            var xs = new ReadOnlyCollection<int>(new[] { 3, 5, 7 });
            var res = xs.AddFirst(2);
            Assert.AreEqual(4, res.Count);
            Assert.IsTrue(new[] { 2, 3, 5, 7 }.SequenceEqual(res));
        }
    }
}
