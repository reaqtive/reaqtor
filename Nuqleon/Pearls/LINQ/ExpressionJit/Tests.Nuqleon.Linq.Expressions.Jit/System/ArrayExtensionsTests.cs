// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Tests
{
    [TestClass]
    public class ArrayExtensionsTests
    {
        [TestMethod]
        public void RemoveFirst1()
        {
            var xs = new[] { 42 };
            var res = xs.RemoveFirst();
            Assert.AreEqual(0, res.Length);
            Assert.AreSame(Array.Empty<int>(), res);
        }

        [TestMethod]
        public void RemoveFirst2()
        {
            var xs = new[] { 42, 43 };
            var res = xs.RemoveFirst();
            Assert.AreEqual(1, res.Length);
            Assert.AreEqual(43, res[0]);
        }

        [TestMethod]
        public void RemoveFirst3()
        {
            var xs = new[] { 2, 3, 5, 7 };
            var res = xs.RemoveFirst();
            Assert.AreEqual(3, res.Length);
            Assert.IsTrue(new[] { 3, 5, 7 }.SequenceEqual(res));
        }

        [TestMethod]
        public void Map1()
        {
            var xs = Array.Empty<int>();
            var res = xs.Map(x => x * 2);
            Assert.AreEqual(0, xs.Length);
            Assert.AreSame(Array.Empty<int>(), res);
        }

        [TestMethod]
        public void Map2()
        {
            var xs = new[] { 1, 2, 3, 4 };
            var res = xs.Map(x => x * 2);
            Assert.AreEqual(4, xs.Length);
            Assert.IsTrue(new[] { 2, 4, 6, 8 }.SequenceEqual(res));
        }
    }
}
