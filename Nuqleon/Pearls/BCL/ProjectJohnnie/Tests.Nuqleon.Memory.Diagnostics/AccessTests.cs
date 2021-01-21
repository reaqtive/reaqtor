// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Memory.Diagnostics;
using System.Reflection;

namespace Tests
{
    [TestClass]
    public class AccessTests
    {
        [TestMethod]
        public void Access_Field_Private()
        {
            var f = typeof(Bar).GetField("Qux", BindingFlags.NonPublic | BindingFlags.Instance);

            var qux = Access.Field(f);

            Assert.AreEqual(AccessType.Field, qux.AccessType);
            Assert.AreSame(f, qux.Field);
            Assert.AreEqual(".Qux", qux.ToString());

            Assert.AreEqual(41, qux.Apply(new Bar()));

            var p = Expression.Parameter(typeof(Bar));
            Assert.AreEqual(41, Expression.Lambda<Func<Bar, int>>(qux.ToExpression(p), p).Compile()(new Bar()));
        }

        [TestMethod]
        public void Access_Field_Public()
        {
            var f = typeof(Bar).GetField("Foo", BindingFlags.Public | BindingFlags.Instance);

            var foo = Access.Field(f);

            Assert.AreEqual(AccessType.Field, foo.AccessType);
            Assert.AreSame(f, foo.Field);
            Assert.AreEqual(".Foo", foo.ToString());

            Assert.AreEqual(42, foo.Apply(new Bar()));

            var p = Expression.Parameter(typeof(Bar));
            Assert.AreEqual(42, Expression.Lambda<Func<Bar, int>>(foo.ToExpression(p), p).Compile()(new Bar()));
        }

        [TestMethod]
        public void Access_VectorElement()
        {
            var x0 = Access.VectorElement(0);
            var x1 = Access.VectorElement(1);

            Assert.AreEqual(AccessType.VectorElement, x0.AccessType);
            Assert.AreEqual(AccessType.VectorElement, x1.AccessType);

            Assert.AreEqual(0, x0.Index);
            Assert.AreEqual(1, x1.Index);

            Assert.AreEqual("[0]", x0.ToString());
            Assert.AreEqual("[1]", x1.ToString());

            Assert.AreEqual(41, x0.Apply(new[] { 41, 42 }));
            Assert.AreEqual(42, x1.Apply(new[] { 41, 42 }));

            var p = Expression.Parameter(typeof(int[]));
            Assert.AreEqual(41, Expression.Lambda<Func<int[], int>>(x0.ToExpression(p), p).Compile()(new[] { 41, 42 }));
            Assert.AreEqual(42, Expression.Lambda<Func<int[], int>>(x1.ToExpression(p), p).Compile()(new[] { 41, 42 }));
        }

        [TestMethod]
        public void Access_MultidimensionalArrayElement()
        {
            var x00 = Access.MultidimensionalArrayElement(0, 0);
            var x12 = Access.MultidimensionalArrayElement(1, 2);

            Assert.AreEqual(AccessType.MultidimensionalArrayElement, x00.AccessType);
            Assert.AreEqual(AccessType.MultidimensionalArrayElement, x12.AccessType);

            Assert.IsTrue(new[] { 0, 0 }.SequenceEqual(x00.Indexes));
            Assert.IsTrue(new[] { 1, 2 }.SequenceEqual(x12.Indexes));

            Assert.AreEqual("[0, 0]", x00.ToString());
            Assert.AreEqual("[1, 2]", x12.ToString());

            Assert.AreEqual(1, x00.Apply(new int[2, 3] { { 1, 2, 3 }, { 4, 5, 6 } }));
            Assert.AreEqual(6, x12.Apply(new int[2, 3] { { 1, 2, 3 }, { 4, 5, 6 } }));

            var p = Expression.Parameter(typeof(int[,]));
            Assert.AreEqual(1, Expression.Lambda<Func<int[,], int>>(x00.ToExpression(p), p).Compile()(new int[2, 3] { { 1, 2, 3 }, { 4, 5, 6 } }));
            Assert.AreEqual(6, Expression.Lambda<Func<int[,], int>>(x12.ToExpression(p), p).Compile()(new int[2, 3] { { 1, 2, 3 }, { 4, 5, 6 } }));
        }

        [TestMethod]
        public void Access_Composite()
        {
            var a0 = Access.VectorElement(1);
            var a1 = Access.Field(typeof(Baz).GetField(nameof(Baz.Bar)));
            var a2 = Access.Field(typeof(Bar).GetField(nameof(Bar.Foo)));

            var a = Access.Composite(a0, a1, a2);

            Assert.AreEqual(AccessType.Composite, a.AccessType);

            Assert.AreEqual(3, a.Accesses.Count);
            Assert.AreSame(a0, a.Accesses[0]);
            Assert.AreSame(a1, a.Accesses[1]);
            Assert.AreSame(a2, a.Accesses[2]);

            Assert.AreEqual("[1].Bar.Foo", a.ToString());

            var o = new[] { new Baz { Bar = new Bar { Foo = -1 } }, new Baz { Bar = new Bar { Foo = 42 } } };

            Assert.AreEqual(42, a.Apply(o));

            var p = Expression.Parameter(typeof(Baz[]));
            Assert.AreEqual(42, Expression.Lambda<Func<Baz[], int>>(a.ToExpression(p), p).Compile()(o));
        }

        private sealed class Bar
        {
#pragma warning disable
            private int Qux = 41;
#pragma warning restore
            public int Foo = 42;
        }

        private sealed class Baz
        {
            public Bar Bar;
        }
    }
}
