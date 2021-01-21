// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq.Expressions;
using System.Linq.Expressions.Jit;
using System.Runtime.CompilerServices;

namespace Tests
{
    [TestClass]
    public class AnalyzerTests
    {
        [TestMethod]
        public void Analyze_Unbound()
        {
            var e = Expression.Lambda<Func<int>>(Expression.Parameter(typeof(int)));
            Assert.ThrowsException<InvalidOperationException>(() => Analyzer.Analyze(e, methodTable: null));
        }

        [TestMethod]
        public void Analyze_Extension()
        {
            var e = Expression.Lambda<Func<int>>(new Extension());
            Assert.ThrowsException<InvalidOperationException>(() => Analyzer.Analyze(e, methodTable: null));
        }

        [TestMethod]
        public void Analyze_Lambda_NoLocals()
        {
            var e = Expression.Lambda<Action>(Expression.Empty());

            var a = Analyzer.Analyze(e, methodTable: null);

            Assert.AreEqual(1, a.Count);

            var l = a[e];

            Assert.AreSame(e, l.Node);
            Assert.IsNull(l.Parent);
            Assert.IsFalse(l.NeedsClosure);
            Assert.IsFalse(l.HasHoistedLocals);
            Assert.AreEqual(0, l.Locals.Count);
        }

        [TestMethod]
        public void Analyze_Lambda_OnlyLocals1()
        {
            var x = Expression.Parameter(typeof(int));
            var e = Expression.Lambda<Action<int>>(Expression.Empty(), x);

            var a = Analyzer.Analyze(e, methodTable: null);

            Assert.AreEqual(1, a.Count);

            var l = a[e];

            Assert.AreSame(e, l.Node);
            Assert.IsNull(l.Parent);
            Assert.IsFalse(l.NeedsClosure);
            Assert.IsFalse(l.HasHoistedLocals);
            Assert.AreEqual(1, l.Locals.Count);

            Assert.AreEqual(StorageKind.Local, l.Locals[x]);
        }

        [TestMethod]
        public void Analyze_Lambda_OnlyLocals2()
        {
            var x = Expression.Parameter(typeof(int));
            var e = Expression.Lambda<Func<int, int>>(Expression.Negate(x), x);

            var a = Analyzer.Analyze(e, methodTable: null);

            Assert.AreEqual(1, a.Count);

            var l = a[e];

            Assert.AreSame(e, l.Node);
            Assert.IsNull(l.Parent);
            Assert.IsFalse(l.NeedsClosure);
            Assert.IsFalse(l.HasHoistedLocals);
            Assert.AreEqual(1, l.Locals.Count);

            Assert.AreEqual(StorageKind.Local, l.Locals[x]);
        }

        [TestMethod]
        public void Analyze_Lambda_Nested1()
        {
            var x = Expression.Parameter(typeof(int));
            var y = Expression.Parameter(typeof(int));
            var i = Expression.Lambda<Func<int, int>>(y, y);
            var e = Expression.Lambda<Func<int, Func<int, int>>>(i, x);

            var a = Analyzer.Analyze(e, methodTable: null);

            Assert.AreEqual(2, a.Count);

            var l1 = a[e];

            Assert.AreSame(e, l1.Node);
            Assert.IsNull(l1.Parent);
            Assert.IsFalse(l1.NeedsClosure);
            Assert.IsFalse(l1.HasHoistedLocals);
            Assert.AreEqual(1, l1.Locals.Count);

            Assert.AreEqual(StorageKind.Local, l1.Locals[x]);

            var l2 = a[i];

            Assert.AreSame(i, l2.Node);
            Assert.AreSame(l1, l2.Parent);
            Assert.IsFalse(l2.NeedsClosure);
            Assert.IsFalse(l2.HasHoistedLocals);
            Assert.AreEqual(1, l2.Locals.Count);

            Assert.AreEqual(StorageKind.Local, l2.Locals[y]);
        }

        [TestMethod]
        public void Analyze_Lambda_Nested2()
        {
            var x = Expression.Parameter(typeof(int));
            var y = Expression.Parameter(typeof(int));
            var i = Expression.Lambda<Func<int, int>>(Expression.Add(x, y), y);
            var e = Expression.Lambda<Func<int, Func<int, int>>>(i, x);

            var a = Analyzer.Analyze(e, methodTable: null);

            Assert.AreEqual(2, a.Count);

            var l1 = a[e];

            Assert.AreSame(e, l1.Node);
            Assert.IsNull(l1.Parent);
            Assert.IsFalse(l1.NeedsClosure);
            Assert.IsTrue(l1.HasHoistedLocals);
            Assert.AreEqual(1, l1.Locals.Count);

            Assert.AreEqual(StorageKind.Hoisted, l1.Locals[x]);

            var l2 = a[i];

            Assert.AreSame(i, l2.Node);
            Assert.AreSame(l1, l2.Parent);
            Assert.IsTrue(l2.NeedsClosure);
            Assert.IsFalse(l2.HasHoistedLocals);
            Assert.AreEqual(1, l2.Locals.Count);

            Assert.AreEqual(StorageKind.Local, l2.Locals[y]);
        }

        [TestMethod]
        public void Analyze_Lambda_Quote()
        {
            var x = Expression.Parameter(typeof(int));
            var y = Expression.Parameter(typeof(int));
            var i = Expression.Lambda<Func<int, int>>(Expression.Add(x, y), y);
            var e = Expression.Lambda<Func<int, Expression<Func<int, int>>>>(Expression.Quote(i), x);

            var a = Analyzer.Analyze(e, methodTable: null);

            Assert.AreEqual(2, a.Count);

            var l1 = a[e];

            Assert.AreSame(e, l1.Node);
            Assert.IsNull(l1.Parent);
            Assert.IsFalse(l1.NeedsClosure);
            Assert.IsTrue(l1.HasHoistedLocals);
            Assert.AreEqual(1, l1.Locals.Count);

            Assert.AreEqual(StorageKind.Hoisted | StorageKind.Boxed, l1.Locals[x]);

            var l2 = a[i];

            Assert.AreSame(i, l2.Node);
            Assert.AreSame(l1, l2.Parent);
            Assert.IsTrue(l2.NeedsClosure);
            Assert.IsFalse(l2.HasHoistedLocals);
            Assert.AreEqual(1, l2.Locals.Count);

            Assert.AreEqual(StorageKind.Local, l2.Locals[y]);
        }

        [TestMethod]
        public void Analyze_Block()
        {
            var x = Expression.Parameter(typeof(int));
            var y = Expression.Parameter(typeof(int));
            var b = Expression.Block(new[] { y }, Expression.Add(x, y));
            var e = Expression.Lambda<Func<int, int>>(b, x);

            var a = Analyzer.Analyze(e, methodTable: null);

            Assert.AreEqual(2, a.Count);

            var l1 = a[e];

            Assert.AreSame(e, l1.Node);
            Assert.IsNull(l1.Parent);
            Assert.IsFalse(l1.NeedsClosure);
            Assert.IsFalse(l1.HasHoistedLocals);
            Assert.AreEqual(1, l1.Locals.Count);

            Assert.AreEqual(StorageKind.Local, l1.Locals[x]);

            var b1 = a[b];

            Assert.AreSame(b, b1.Node);
            Assert.AreSame(l1, b1.Parent);
            Assert.IsTrue(b1.NeedsClosure); // REVIEW
            Assert.IsFalse(b1.HasHoistedLocals);
            Assert.AreEqual(1, b1.Locals.Count);

            Assert.AreEqual(StorageKind.Local, b1.Locals[y]);
        }

        [TestMethod]
        public void Analyze_RuntimeVariables()
        {
            var x = Expression.Parameter(typeof(int));
            var e = Expression.Lambda<Func<int, IRuntimeVariables>>(Expression.RuntimeVariables(x), x);

            var a = Analyzer.Analyze(e, methodTable: null);

            Assert.AreEqual(1, a.Count);

            var l = a[e];

            Assert.AreSame(e, l.Node);
            Assert.IsNull(l.Parent);
            Assert.IsFalse(l.NeedsClosure);
            Assert.IsTrue(l.HasHoistedLocals);
            Assert.AreEqual(1, l.Locals.Count);

            Assert.AreEqual(StorageKind.Boxed | StorageKind.Hoisted, l.Locals[x]);
        }

        [TestMethod]
        public void Analyze_CatchBlock()
        {
            var x = Expression.Parameter(typeof(int));
            var r = Expression.Parameter(typeof(Exception));
            var c = Expression.Catch(r, Expression.Empty());
            var t = Expression.TryCatch(Expression.Empty(), c);
            var e = Expression.Lambda<Action<int>>(t, x);

            var a = Analyzer.Analyze(e, methodTable: null);

            Assert.AreEqual(2, a.Count);

            var l1 = a[e];

            Assert.AreSame(e, l1.Node);
            Assert.IsNull(l1.Parent);
            Assert.IsFalse(l1.NeedsClosure);
            Assert.IsFalse(l1.HasHoistedLocals);
            Assert.AreEqual(1, l1.Locals.Count);

            Assert.AreEqual(StorageKind.Local, l1.Locals[x]);

            var c1 = a[c];

            Assert.AreSame(c, c1.Node);
            Assert.AreSame(l1, c1.Parent);
            Assert.IsFalse(c1.NeedsClosure);
            Assert.IsFalse(c1.HasHoistedLocals);
            Assert.AreEqual(1, c1.Locals.Count);

            Assert.AreEqual(StorageKind.Local, c1.Locals[r]);
        }

        [TestMethod]
        public void Analyze_CatchBlock_NoVariable()
        {
            var x = Expression.Parameter(typeof(int));
            var c = Expression.Catch(typeof(Exception), Expression.Empty());
            var t = Expression.TryCatch(Expression.Empty(), c);
            var e = Expression.Lambda<Action<int>>(t, x);

            var a = Analyzer.Analyze(e, methodTable: null);

            Assert.AreEqual(2, a.Count);

            var l1 = a[e];

            Assert.AreSame(e, l1.Node);
            Assert.IsNull(l1.Parent);
            Assert.IsFalse(l1.NeedsClosure);
            Assert.IsFalse(l1.HasHoistedLocals);
            Assert.AreEqual(1, l1.Locals.Count);

            Assert.AreEqual(StorageKind.Local, l1.Locals[x]);

            var c1 = a[c];

            Assert.AreSame(c, c1.Node);
            Assert.AreSame(l1, c1.Parent);
            Assert.IsFalse(c1.NeedsClosure);
            Assert.IsFalse(c1.HasHoistedLocals);
            Assert.AreEqual(0, c1.Locals.Count);
        }

        private sealed class Extension : Expression
        {
            public override bool CanReduce => false;
            public override ExpressionType NodeType => ExpressionType.Extension;
            public override Type Type => typeof(int);
        }
    }
}
