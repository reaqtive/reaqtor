// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2014 - Created this file.
//

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.CompilerServices;
using CollectionExtensions = System.Linq.CompilerServices.CollectionExtensions;

namespace Tests.System.Linq.CompilerServices
{
    [TestClass]
    public class CollectionExtensionsTests
    {
        [TestMethod]
        public void ToReadOnly_Empty()
        {
            var xs = new List<int>();

            var e1 = CollectionExtensions.ToReadOnly<int>(enumerable: null);
            AssertEmpty(e1);

            var e2 = Array.Empty<int>().ToReadOnly();
            AssertEmpty(e2);

            var e3 = xs.ToReadOnly();
            AssertEmpty(e3);

            xs.Add(1);
            AssertEmpty(e3);

            Assert.AreSame(e1, e2);
            Assert.AreSame(e2, e3);

            var e4 = CollectionExtensions.ToReadOnly<int>(enumerable: null);
            var e5 = Array.Empty<int>().ToReadOnly();
            var e6 = new List<int>().ToReadOnly();

            Assert.AreSame(e3, e4);
            Assert.AreSame(e4, e5);
            Assert.AreSame(e5, e6);
        }

        [TestMethod]
        public void ToReadOnly_NoAliasing1()
        {
            var xs = new List<int> { 2, 3, 5 };

            var ro = xs.ToReadOnly();
            Assert.AreEqual(3, ro.Count);
            Assert.IsTrue(new[] { 2, 3, 5 }.SequenceEqual(ro));

            xs.Add(7);
            Assert.AreEqual(3, ro.Count);
            Assert.IsTrue(new[] { 2, 3, 5 }.SequenceEqual(ro));

            xs.Clear();
            Assert.AreEqual(3, ro.Count);
            Assert.IsTrue(new[] { 2, 3, 5 }.SequenceEqual(ro));
        }

        [TestMethod]
        public void ToReadOnly_NoAliasing2()
        {
            var xs = new List<int> { 2, 3, 5 };
            var ys = xs.Select(x => x);

            var ro = ys.ToReadOnly();
            Assert.AreEqual(3, ro.Count);
            Assert.IsTrue(new[] { 2, 3, 5 }.SequenceEqual(ro));

            xs.Add(7);
            Assert.AreEqual(3, ro.Count);
            Assert.IsTrue(new[] { 2, 3, 5 }.SequenceEqual(ro));

            xs.Clear();
            Assert.AreEqual(3, ro.Count);
            Assert.IsTrue(new[] { 2, 3, 5 }.SequenceEqual(ro));
        }

        [TestMethod]
        public void ToReadOnly_Reuse()
        {
            var xs = new List<int> { 2, 3, 5 };

            var r1 = xs.ToReadOnly();
            var r2 = r1.ToReadOnly();
            var r3 = r1.ToReadOnly();
            var r4 = r2.ToReadOnly();

            Assert.AreSame(r1, r2);
            Assert.AreSame(r2, r3);
            Assert.AreSame(r3, r4);
        }

        [TestMethod]
        public void AsArray_Array()
        {
            var xs = new[] { 2, 3, 5 };

            var ys = CollectionExtensions.AsArray(xs);

            Assert.AreSame(xs, ys);
        }

        [TestMethod]
        public void AsArray_Enumerable()
        {
            var xs = new[] { 2, 3, 5 }.Select(x => x);

            var ys = CollectionExtensions.AsArray(xs);

            Assert.IsTrue(xs.SequenceEqual(ys));
        }

        [TestMethod]
        public void AsArray_Null()
        {
            var xs = default(int[]);

            var ys = CollectionExtensions.AsArray(xs);

            Assert.AreEqual(0, ys.Length);
        }

        [TestMethod]
        public void AsCollection_Array()
        {
            var xs = new[] { 2, 3, 5 };

            var ys = CollectionExtensions.AsCollection(xs);

            Assert.AreSame(xs, ys);
        }

        [TestMethod]
        public void AsCollection_Enumerable()
        {
            var xs = new[] { 2, 3, 5 }.Select(x => x);

            var ys = CollectionExtensions.AsCollection(xs);

            Assert.IsTrue(xs.SequenceEqual(ys));
        }

        [TestMethod]
        public void AsCollection_Null()
        {
            var xs = default(int[]);

            var ys = CollectionExtensions.AsCollection(xs);

            Assert.AreEqual(0, ys.Count);
        }

        [TestMethod]
        public void ToIListUnsafe()
        {
            var xs = new List<int> { 2, 3, 5 };
            Assert.AreSame(xs, xs.ToIListUnsafe());

            CollectionAssert.AreEqual(xs, xs.Select(x => x).ToIListUnsafe().ToArray());
        }

        [TestMethod]
        public void ToIReadOnlyListUnsafe()
        {
            var xs = new List<int> { 2, 3, 5 };
            Assert.AreSame(xs, xs.ToIReadOnlyListUnsafe());

            CollectionAssert.AreEqual(xs, xs.Select(x => x).ToIReadOnlyListUnsafe().ToArray());
        }

        [TestMethod]
        public void ToReadOnlyUnsafe()
        {
            CollectionAssert.AreEqual(Array.Empty<int>(), Enumerable.Empty<int>().ToReadOnlyUnsafe());
            CollectionAssert.AreEqual(Array.Empty<int>(), Array.Empty<int>().ToReadOnlyUnsafe());
            CollectionAssert.AreEqual(Array.Empty<int>(), new List<int>().ToReadOnlyUnsafe());

            var xs = new List<int> { 2, 3, 5 };
            CollectionAssert.AreEqual(xs, xs.ToReadOnlyUnsafe());

            var ys = new ReadOnlyCollection<int>(xs);
            Assert.AreSame(ys, ys.ToReadOnlyUnsafe());

            CollectionAssert.AreEqual(xs, xs.Select(x => x).ToReadOnlyUnsafe());
        }

        [TestMethod]
        public void ToArrayUnsafe()
        {
            var xs = new List<int> { 2, 3, 5 };
            CollectionAssert.AreEqual(xs, new ReadOnlyCollection<int>(xs).ToArrayUnsafe());

            var ys = new int[] { 2, 3, 5 };
            CollectionAssert.AreEqual(ys, new ReadOnlyCollection<int>(ys).ToArrayUnsafe());
            CollectionAssert.AreEqual(ys, ys.ToReadOnly().ToArrayUnsafe());
        }

        private static void AssertEmpty<T>(ReadOnlyCollection<T> c)
        {
            Assert.AreEqual(0, c.Count);
        }
    }
}
