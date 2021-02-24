// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - May 2013 - Created this file.
//

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq.CompilerServices;
using System.Linq.Expressions;

namespace Tests.System.Linq.CompilerServices
{
    [TestClass]
    public class ExpressionTreeTests
    {
        [TestMethod]
        public void ExpressionTree_ArgumentChecking()
        {
            AssertEx.ThrowsException<ArgumentNullException>(() => ExpressionExtensions.ToExpressionTree(default(Expression)), ex => Assert.AreEqual("expression", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => ExpressionExtensions.ToExpressionTree(default(ElementInit)), ex => Assert.AreEqual("elementInitializer", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => ExpressionExtensions.ToExpressionTree(default(MemberBinding)), ex => Assert.AreEqual("memberBinding", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => new ExpressionExpressionTreeNode(expression: null), ex => Assert.AreEqual("expression", ex.ParamName));
        }

        [TestMethod]
        public void ExpressionTree_ToString()
        {
            var ce = Expression.Add(Expression.Constant(1), Expression.Constant(2));
            var et = ce.ToExpressionTree();
            Assert.AreEqual("Add(Constant[(int)1](), Constant[(int)2]())", et.ToString());
        }

        [TestMethod]
        public void ExpressionTree_Update_ArgumentChecks()
        {
            var et = Expression.Constant(42).ToExpressionTree();

#pragma warning disable IDE0034 // Simplify 'default' expression (illustrative of method signature)
            AssertEx.ThrowsException<ArgumentNullException>(() => et.Update(default(IEnumerable<ExpressionTree>)), ex => Assert.AreEqual("children", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => et.Update(default(ExpressionTree[])), ex => Assert.AreEqual("children", ex.ParamName));
#pragma warning restore IDE0034 // Simplify 'default' expression
        }

        [TestMethod]
        public void ExpressionTree_Update_Checks()
        {
            // This code is not directly reachable right now because the Tree.Update method ignores nodes with no children.

            var args = Array.Empty<ITree<ExpressionTreeNode>>();
            var update = new ExpressionTreeUpdate(args);

            var ce = Expression.Constant(42);
            Assert.AreSame(ce, update.Visit(ce).Expression);

            var de = Expression.Default(typeof(int));
            Assert.AreSame(de, update.Visit(de).Expression);

            var pe = Expression.Parameter(typeof(int));
            Assert.AreSame(pe, update.Visit(pe).Expression);

            var me = Expression.Property(expression: null, typeof(DateTime).GetProperty("Now"));
            Assert.AreSame(me, update.Visit(me).Expression);
        }

        [TestMethod]
        public void ExpressionTreeNode_Equality1()
        {
            var e1 = Expression.Constant(42);
            var e2 = Expression.Parameter(typeof(int));
            var e3 = Expression.ElementInit(typeof(List<int>).GetMethod("Add"), Expression.Constant(1));

            var t1 = e1.ToExpressionTree().Value;
            var t2 = e2.ToExpressionTree().Value;
            var t3 = new ElementInitExpressionTreeNode(e3);
            var u1 = e1.ToExpressionTree().Value;
            var u2 = e2.ToExpressionTree().Value;
            var u3 = new ElementInitExpressionTreeNode(e3);

            Assert.IsTrue(t1.Equals(t1));
            Assert.IsTrue(t2.Equals(t2));
            Assert.IsTrue(t3.Equals(t3));
            Assert.IsFalse(t1.Equals(t2));
            Assert.IsFalse(t2.Equals(t1));
            Assert.IsFalse(t1.Equals(t3));
            Assert.IsFalse(t3.Equals(t1));
            Assert.IsFalse(t2.Equals(t3));
            Assert.IsFalse(t3.Equals(t2));
            Assert.IsFalse(t1.Equals(null));
            Assert.IsFalse(t2.Equals(null));
            Assert.IsFalse(t3.Equals(null));
            Assert.IsFalse(t1.Equals("foo"));
            Assert.IsFalse(t2.Equals("foo"));
            Assert.IsFalse(t3.Equals("foo"));

            Assert.AreEqual(t1.GetHashCode(), u1.GetHashCode());
            Assert.AreEqual(t2.GetHashCode(), u2.GetHashCode());
            Assert.AreEqual(t3.GetHashCode(), u3.GetHashCode());

            var t4 = (IEquatable<ExpressionExpressionTreeNode>)t1;
            var t5 = (ExpressionExpressionTreeNode)t1;

            Assert.IsTrue(t4.Equals(t5));
            Assert.IsFalse(t4.Equals(default));
            Assert.IsFalse(t5.Equals(42));
            Assert.IsTrue(t1.Equals(t5));
            Assert.IsTrue(t5.Equals(t1));

            var t6 = t1;
            var t7 = (ExpressionTreeNode)t3;

            Assert.IsFalse(t6.Equals(t7));
            Assert.IsTrue(t6.Equals(t6));
            Assert.IsTrue(t7.Equals(t7));
        }

        [TestMethod]
        public void ExpressionTreeNode_Equality2()
        {
            var e2 = Expression.ElementInit(typeof(List<int>).GetMethod("Add"), Expression.Constant(1));
            var e3 = Expression.Bind(typeof(ADS).GetProperty("ApplicationBase"), Expression.Constant("foo"));

            var t2 = new ElementInitExpressionTreeNode(e2);
            var t3 = new MemberAssignmentExpressionTreeNode(e3);

            var u3 = new MemberAssignmentExpressionTreeNode(e3);

            Assert.IsTrue(t3.Equals(t3));
            Assert.IsFalse(t2.Equals(t3));
            Assert.IsFalse(t3.Equals(t2));
            Assert.IsFalse(t3.Equals(null));
            Assert.IsTrue(t3.Equals(u3));
            Assert.IsTrue(u3.Equals(t3));

            var t4 = (ExpressionTreeNode)t3;

            Assert.IsTrue(t4.Equals(t4));

            var e5 = Expression.MemberBind(typeof(Bar).GetProperty("Foo"), Expression.Bind(typeof(Foo).GetProperty("Baz"), Expression.Constant(1)));
            var e6 = Expression.ListBind(typeof(Foo).GetProperty("Xs"), e2);

            var t5 = new MemberMemberBindingExpressionTreeNode(e5);
            var t6 = new MemberListBindingExpressionTreeNode(e6);

            Assert.IsFalse(t3.Equals(t5));
            Assert.IsFalse(t3.Equals(t6));
            Assert.IsFalse(t5.Equals(t6));
            Assert.IsFalse(t5.Equals(t3));
            Assert.IsFalse(t6.Equals(t3));
            Assert.IsFalse(t6.Equals(t5));
            Assert.IsFalse(t5.Equals(null));
            Assert.IsFalse(t6.Equals(null));

            Assert.IsTrue(t5.Equals(t5));
            Assert.IsTrue(t6.Equals(t6));

            var t7 = (MemberBindingExpressionTreeNode)t4;
            var t8 = (MemberBindingExpressionTreeNode)t5;
            var t9 = (MemberBindingExpressionTreeNode)t6;

            Assert.IsFalse(t7.Equals(t8));
            Assert.IsFalse(t7.Equals(t9));
            Assert.IsFalse(t8.Equals(t9));

            Assert.IsFalse(t4.Equals("bar"));
            Assert.IsFalse(t5.Equals("bar"));
            Assert.IsFalse(t6.Equals("bar"));
            Assert.IsFalse(t7.Equals("bar"));
            Assert.IsFalse(t8.Equals("bar"));
            Assert.IsFalse(t9.Equals("bar"));

            var u5 = new MemberMemberBindingExpressionTreeNode(e5);
            var u6 = new MemberListBindingExpressionTreeNode(e6);

            Assert.IsTrue(t5.Equals(u5));
            Assert.IsTrue(u6.Equals(t6));

            Assert.AreEqual(t5.GetHashCode(), u5.GetHashCode());
            Assert.AreEqual(t6.GetHashCode(), u6.GetHashCode());
        }

        private sealed class ADS
        {
            public string ApplicationBase { get; set; }
        }

        [TestMethod]
        public void ExpressionTree_Constant()
        {
            var ce1 = Expression.Constant(42);
            var ce2 = Expression.Constant(42);

            var et1 = (ExpressionTree<ConstantExpression>)ce1.ToExpressionTree();
            var et2 = (ExpressionTree<ConstantExpression>)ce2.ToExpressionTree();

            Assert.AreSame(ce1, et1.Expression);

            Assert.IsFalse(et1.Equals(null));
            Assert.IsFalse(et1.Equals(42));
            Assert.IsTrue(et1.Equals(et1));
            Assert.IsTrue(et1.Equals(et2));
            Assert.IsTrue(et2.Equals(et1));

            Assert.AreEqual(et1.GetHashCode(), et2.GetHashCode());

            Assert.AreEqual("Constant[(int)42]()", et1.ToString());
        }

        [TestMethod]
        public void ExpressionTree_Constant_Null()
        {
            var ce1 = Expression.Constant(value: null, typeof(string));
            var ce2 = Expression.Constant(value: null, typeof(string));

            var et1 = (ExpressionTree<ConstantExpression>)ce1.ToExpressionTree();
            var et2 = (ExpressionTree<ConstantExpression>)ce2.ToExpressionTree();

            Assert.AreSame(ce1, et1.Expression);

            Assert.IsFalse(et1.Equals(null));
            Assert.IsFalse(et1.Equals(""));
            Assert.IsTrue(et1.Equals(et1));
            Assert.IsTrue(et1.Equals(et2));
            Assert.IsTrue(et2.Equals(et1));

            Assert.AreEqual(et1.GetHashCode(), et2.GetHashCode());

            Assert.AreEqual("Constant[(string)null]()", et1.ToString());
        }

        [TestMethod]
        public void ExpressionTree_Constant_Update()
        {
            var ce = Expression.Constant(42);

            var et = (ExpressionTree<ConstantExpression>)ce.ToExpressionTree();

            var cu = et.Update();

            Assert.AreSame(et, cu);
        }

        [TestMethod]
        public void ExpressionTree_Default()
        {
            var de1 = Expression.Default(typeof(int));
            var de2 = Expression.Default(typeof(int));

            var et1 = (ExpressionTree<DefaultExpression>)de1.ToExpressionTree();
            var et2 = (ExpressionTree<DefaultExpression>)de2.ToExpressionTree();

            Assert.AreSame(de1, et1.Expression);

            Assert.IsFalse(et1.Equals(null));
            Assert.IsFalse(et1.Equals(42));
            Assert.IsTrue(et1.Equals(et1));
            Assert.IsTrue(et1.Equals(et2));
            Assert.IsTrue(et2.Equals(et1));

            Assert.AreEqual(et1.GetHashCode(), et2.GetHashCode());

            Assert.AreEqual("Default[int]()", et1.ToString());
        }

        [TestMethod]
        public void ExpressionTree_Default_Update()
        {
            var de = Expression.Default(typeof(int));

            var et = (ExpressionTree<DefaultExpression>)de.ToExpressionTree();

            var du = et.Update();

            Assert.AreSame(et, du);
        }

        [TestMethod]
        public void ExpressionTree_Binary_Standard()
        {
            var be1 = Expression.Add(Expression.Constant(2), Expression.Constant(3));
            var be2 = Expression.Add(Expression.Constant(2), Expression.Constant(3));

            var et1 = (ExpressionTree<BinaryExpression>)be1.ToExpressionTree();
            var et2 = (ExpressionTree<BinaryExpression>)be2.ToExpressionTree();

            Assert.AreSame(be1, et1.Expression);

            Assert.IsFalse(et1.Equals(null));
            Assert.IsFalse(et1.Equals(42));
            Assert.IsTrue(et1.Equals(et1));
            Assert.IsTrue(et1.Equals(et2));
            Assert.IsTrue(et2.Equals(et1));

            Assert.AreEqual(et1.GetHashCode(), et2.GetHashCode());

            Assert.AreEqual("Add(Constant[(int)2](), Constant[(int)3]())", et1.ToString());
        }

        [TestMethod]
        public void ExpressionTree_Binary_CustomOperator()
        {
            var be1 = Expression.Add(Expression.Constant(TimeSpan.FromSeconds(1)), Expression.Constant(TimeSpan.FromSeconds(3)));
            var be2 = Expression.Add(Expression.Constant(TimeSpan.FromSeconds(1)), Expression.Constant(TimeSpan.FromSeconds(3)));

            var et1 = (ExpressionTree<BinaryExpression>)be1.ToExpressionTree();
            var et2 = (ExpressionTree<BinaryExpression>)be2.ToExpressionTree();

            Assert.AreSame(be1, et1.Expression);

            Assert.IsFalse(et1.Equals(null));
            Assert.IsFalse(et1.Equals(42));
            Assert.IsTrue(et1.Equals(et1));
            Assert.IsTrue(et1.Equals(et2));
            Assert.IsTrue(et2.Equals(et1));

            Assert.AreEqual(et1.GetHashCode(), et2.GetHashCode());

            Assert.AreEqual("Add[TimeSpan TimeSpan::op_Addition(TimeSpan t1, TimeSpan t2)](Constant[(TimeSpan)00:00:01](), Constant[(TimeSpan)00:00:03]())", et1.ToString());
        }

        [TestMethod]
        public void ExpressionTree_Binary_Conversion()
        {
            var p = Expression.Parameter(typeof(string), "p");
            var q = Expression.Parameter(typeof(string), "q");

            var be1 = Expression.Coalesce(Expression.Constant("bar"), Expression.Constant("foo"), Expression.Lambda<Func<string, string>>(p, p));
            var be2 = Expression.Coalesce(Expression.Constant("bar"), Expression.Constant("foo"), Expression.Lambda<Func<string, string>>(q, q));

            var et1 = (ExpressionTree<BinaryExpression>)be1.ToExpressionTree();
            var et2 = (ExpressionTree<BinaryExpression>)be2.ToExpressionTree();

            Assert.AreSame(be1, et1.Expression);

            Assert.IsFalse(et1.Equals(null));
            Assert.IsFalse(et1.Equals(42));
            Assert.IsTrue(et1.Equals(et1));
            Assert.IsTrue(et1.Equals(et2));
            Assert.IsTrue(et2.Equals(et1));

            Assert.AreEqual(et1.GetHashCode(), et2.GetHashCode());

            Assert.AreEqual("Coalesce(Constant[(string)bar](), Constant[(string)foo](), Lambda[Func<string, string>](Parameter[string p](), Parameter[string p]()))", et1.ToString());
        }

        [TestMethod]
        public void ExpressionTree_Binary_Standard_Update()
        {
            var be = Expression.Add(Expression.Constant(2), Expression.Constant(3));

            var et = (ExpressionTree<BinaryExpression>)be.ToExpressionTree();

            var a1 = Expression.Constant(4).ToExpressionTree();
            var a2 = et.Children[1];

            var bu = et.Update(a1, a2);
            var bv = et.Update((IEnumerable<ExpressionTree>)new[] { a1, (ExpressionTree)a2 });

            Assert.AreSame(a1, bu.Children[0]);
            Assert.AreSame(a2, bu.Children[1]);

            Assert.AreSame(a1, bv.Children[0]);
            Assert.AreSame(a2, bv.Children[1]);

            var b2 = Expression.Add(Expression.Constant(4), Expression.Constant(3));

            var eq = new ExpressionEqualityComparer();
            Assert.IsTrue(eq.Equals(b2, ((ExpressionTree<BinaryExpression>)bu).Expression));
            Assert.IsTrue(eq.Equals(b2, ((ExpressionTree<BinaryExpression>)bv).Expression));
        }

        [TestMethod]
        public void ExpressionTree_Binary_Conversion_Update()
        {
            var s = Expression.Parameter(typeof(string), "s");
            var i = Expression.Lambda<Func<string, string>>(s, s);
            var j = Expression.Lambda<Func<string, string>>(Expression.Constant("baz"), s);

            var be = Expression.Coalesce(Expression.Constant("bar"), Expression.Constant("foo"), i);

            var et = (ExpressionTree<BinaryExpression>)be.ToExpressionTree();

            var a1 = Expression.Constant("qux").ToExpressionTree();
            var a2 = et.Children[1];
            var a3 = j.ToExpressionTree();

            var bu = et.Update(a1, a2, a3);

            Assert.AreSame(a1, bu.Children[0]);
            Assert.AreSame(a2, bu.Children[1]);
            Assert.AreSame(a3, bu.Children[2]);

            var b2 = Expression.Coalesce(Expression.Constant("qux"), Expression.Constant("foo"), j);

            var eq = new ExpressionEqualityComparer();
            Assert.IsTrue(eq.Equals(b2, ((ExpressionTree<BinaryExpression>)bu).Expression));
        }

        [TestMethod]
        public void ExpressionTree_Conditional_Both()
        {
            var ce1 = Expression.Condition(Expression.Constant(true), Expression.Constant(2), Expression.Constant(3));
            var ce2 = Expression.Condition(Expression.Constant(true), Expression.Constant(2), Expression.Constant(3));

            var et1 = (ExpressionTree<ConditionalExpression>)ce1.ToExpressionTree();
            var et2 = (ExpressionTree<ConditionalExpression>)ce2.ToExpressionTree();

            Assert.AreSame(ce1, et1.Expression);

            Assert.IsFalse(et1.Equals(null));
            Assert.IsFalse(et1.Equals(42));
            Assert.IsTrue(et1.Equals(et1));
            Assert.IsTrue(et1.Equals(et2));
            Assert.IsTrue(et2.Equals(et1));

            Assert.AreEqual(et1.GetHashCode(), et2.GetHashCode());

            Assert.AreEqual("Conditional(Constant[(bool)True](), Constant[(int)2](), Constant[(int)3]())", et1.ToString());
        }

        [TestMethod]
        public void ExpressionTree_Conditional_Update()
        {
            var ce = Expression.Condition(Expression.Constant(true), Expression.Constant(2), Expression.Constant(3));

            var et = (ExpressionTree<ConditionalExpression>)ce.ToExpressionTree();

            var e1 = Expression.Constant(false);
            var e2 = ce.IfTrue;
            var e3 = Expression.Constant(4);

            var a1 = e1.ToExpressionTree();
            var a2 = e2.ToExpressionTree();
            var a3 = e3.ToExpressionTree();

            var cu = et.Update(a1, a2, a3);

            Assert.AreSame(a1, cu.Children[0]);
            Assert.AreSame(a2, cu.Children[1]);
            Assert.AreSame(a3, cu.Children[2]);

            var c2 = Expression.Condition(Expression.Constant(false), Expression.Constant(2), Expression.Constant(4));

            var eq = new ExpressionEqualityComparer();
            Assert.IsTrue(eq.Equals(c2, ((ExpressionTree<ConditionalExpression>)cu).Expression));
        }

        [TestMethod]
        public void ExpressionTree_Invocation_Update()
        {
            var f = new Func<int, int>(x => x);

            var ie = Expression.Invoke(Expression.Constant(f), Expression.Constant(2));

            var et = (ExpressionTree<InvocationExpression>)ie.ToExpressionTree();

            var e1 = et.Children[0];
            var e2 = Expression.Constant(3);

            var a1 = e1;
            var a2 = e2.ToExpressionTree();

            var iu = et.Update(a1, a2);

            Assert.AreSame(a1, iu.Children[0]);
            Assert.AreSame(a2, iu.Children[1]);

            var i2 = Expression.Invoke(Expression.Constant(f), Expression.Constant(3));

            var eq = new ExpressionEqualityComparer();
            Assert.IsTrue(eq.Equals(i2, ((ExpressionTree<InvocationExpression>)iu).Expression));

            Assert.ThrowsException<InvalidOperationException>(() => et.Update(a1, a2, a2));
        }

        [TestMethod]
        public void ExpressionTree_ListInit()
        {
            var le1 = ((Expression<Func<List<int>>>)(() => new List<int> { 2, 3, 5 })).Body;
            var le2 = ((Expression<Func<List<int>>>)(() => new List<int> { 2, 3, 5 })).Body;

            var et1 = (ExpressionTree<ListInitExpression>)le1.ToExpressionTree();
            var et2 = (ExpressionTree<ListInitExpression>)le2.ToExpressionTree();

            Assert.AreSame(le1, et1.Expression);

            Assert.IsFalse(et1.Equals(null));
            Assert.IsFalse(et1.Equals(42));
            Assert.IsTrue(et1.Equals(et1));
            Assert.IsTrue(et1.Equals(et2));
            Assert.IsTrue(et2.Equals(et1));

            Assert.AreEqual(et1.GetHashCode(), et2.GetHashCode());

            Assert.AreEqual("ListInit(New[new List<int>()](), ElementInit[void List<int>.Add(int item)](Constant[(int)2]()), ElementInit[void List<int>.Add(int item)](Constant[(int)3]()), ElementInit[void List<int>.Add(int item)](Constant[(int)5]()))", et1.ToString());
        }

        [TestMethod]
        public void ExpressionTree_ListInit_Update()
        {
            var le = ((Expression<Func<List<int>>>)(() => new List<int> { 2, 3, 5 })).Body;

            var et = (ExpressionTree<ListInitExpression>)le.ToExpressionTree();

            var ei = Expression.ElementInit(typeof(List<int>).GetMethod("Add"), Expression.Constant(4));

            var a0 = et.Children[0];
            var a1 = et.Children[1];
            var a2 = ei.ToExpressionTree();
            var a3 = et.Children[3];

            var lu = et.Update(a0, a1, a2, a3);

            Assert.AreSame(a0, lu.Children[0]);
            Assert.AreSame(a1, lu.Children[1]);
            Assert.AreSame(a2, lu.Children[2]);
            Assert.AreSame(a3, lu.Children[3]);

            var l2 = ((Expression<Func<List<int>>>)(() => new List<int> { 2, 4, 5 })).Body;

            var eq = new ExpressionEqualityComparer();
            Assert.IsTrue(eq.Equals(l2, ((ExpressionTree<ListInitExpression>)lu).Expression));

            Assert.ThrowsException<InvalidOperationException>(() => et.Update(a0));
            Assert.ThrowsException<InvalidOperationException>(() => et.Update(a0, a1));
            Assert.ThrowsException<InvalidOperationException>(() => et.Update(a0, a1, a2, a3, a2));
        }

        [TestMethod]
        public void ExpressionTree_MethodCall_Instance()
        {
            var me1 = ((Expression<Func<string>>)(() => "Hello".Substring(1, 2))).Body;
            var me2 = ((Expression<Func<string>>)(() => "Hello".Substring(1, 2))).Body;

            var et1 = (ExpressionTree<MethodCallExpression>)me1.ToExpressionTree();
            var et2 = (ExpressionTree<MethodCallExpression>)me2.ToExpressionTree();

            Assert.AreSame(me1, et1.Expression);

            Assert.IsFalse(et1.Equals(null));
            Assert.IsFalse(et1.Equals(42));
            Assert.IsTrue(et1.Equals(et1));
            Assert.IsTrue(et1.Equals(et2));
            Assert.IsTrue(et2.Equals(et1));

            Assert.AreEqual(et1.GetHashCode(), et2.GetHashCode());

            Assert.AreEqual("Call[string string.Substring(int startIndex, int length)](Constant[(string)Hello](), Constant[(int)1](), Constant[(int)2]())", et1.ToString());
        }

        [TestMethod]
        public void ExpressionTree_MethodCall_Instance_Update()
        {
#pragma warning disable IDE0057 // Substring can be simplified. (Not in expression trees)
            var me = ((Expression<Func<string>>)(() => "bar".Substring(1))).Body;

            var et = (ExpressionTree<MethodCallExpression>)me.ToExpressionTree();

            var a1 = Expression.Constant("foo").ToExpressionTree();
            var a2 = Expression.Constant(2).ToExpressionTree();

            var mu = et.Update(a1, a2);

            Assert.AreSame(a1, mu.Children[0]);
            Assert.AreSame(a2, mu.Children[1]);

            var m2 = ((Expression<Func<string>>)(() => "foo".Substring(2))).Body;

            var eq = new ExpressionEqualityComparer();
            Assert.IsTrue(eq.Equals(m2, ((ExpressionTree<MethodCallExpression>)mu).Expression));

            Assert.ThrowsException<InvalidOperationException>(() => et.Update(a1));
            Assert.ThrowsException<InvalidOperationException>(() => et.Update(a1, a2, a2));
#pragma warning restore IDE0057
        }

        [TestMethod]
        public void ExpressionTree_MethodCall_Static()
        {
            var me1 = ((Expression<Action>)(() => Console.WriteLine("Hello"))).Body;
            var me2 = ((Expression<Action>)(() => Console.WriteLine("Hello"))).Body;

            var et1 = (ExpressionTree<MethodCallExpression>)me1.ToExpressionTree();
            var et2 = (ExpressionTree<MethodCallExpression>)me2.ToExpressionTree();

            Assert.AreSame(me1, et1.Expression);

            Assert.IsFalse(et1.Equals(null));
            Assert.IsFalse(et1.Equals(42));
            Assert.IsTrue(et1.Equals(et1));
            Assert.IsTrue(et1.Equals(et2));
            Assert.IsTrue(et2.Equals(et1));

            Assert.AreEqual(et1.GetHashCode(), et2.GetHashCode());

            Assert.AreEqual("Call[void Console::WriteLine(string value)](Constant[(string)Hello]())", et1.ToString());
        }

        [TestMethod]
        public void ExpressionTree_MethodCall_Static_Update()
        {
            var me = ((Expression<Action>)(() => Console.WriteLine("bar"))).Body;

            var et = (ExpressionTree<MethodCallExpression>)me.ToExpressionTree();

            var a1 = Expression.Constant("qux").ToExpressionTree();

            var mu = et.Update(a1);

            Assert.AreSame(a1, mu.Children[0]);

            var m2 = ((Expression<Action>)(() => Console.WriteLine("qux"))).Body;

            var eq = new ExpressionEqualityComparer();
            Assert.IsTrue(eq.Equals(m2, ((ExpressionTree<MethodCallExpression>)mu).Expression));

            Assert.ThrowsException<InvalidOperationException>(() => et.Update(a1, a1));
            Assert.ThrowsException<InvalidOperationException>(() => et.Update());
        }

        [TestMethod]
        public void ExpressionTree_Member_Instance()
        {
            var me1 = ((Expression<Func<int>>)(() => "Hello".Length)).Body;
            var me2 = ((Expression<Func<int>>)(() => "Hello".Length)).Body;

            var et1 = (ExpressionTree<MemberExpression>)me1.ToExpressionTree();
            var et2 = (ExpressionTree<MemberExpression>)me2.ToExpressionTree();

            Assert.AreSame(me1, et1.Expression);

            Assert.IsFalse(et1.Equals(null));
            Assert.IsFalse(et1.Equals(42));
            Assert.IsTrue(et1.Equals(et1));
            Assert.IsTrue(et1.Equals(et2));
            Assert.IsTrue(et2.Equals(et1));

            Assert.AreEqual(et1.GetHashCode(), et2.GetHashCode());

            Assert.AreEqual("MemberAccess[int string.Length { get; }](Constant[(string)Hello]())", et1.ToString());
        }

        [TestMethod]
        public void ExpressionTree_Member_Instance_Update()
        {
            var me = ((Expression<Func<int>>)(() => "bar".Length)).Body;

            var et = (ExpressionTree<MemberExpression>)me.ToExpressionTree();

            var a1 = Expression.Constant("qux").ToExpressionTree();

            var mu = et.Update(a1);

            Assert.AreSame(a1, mu.Children[0]);

            var m2 = ((Expression<Func<int>>)(() => "qux".Length)).Body;

            var eq = new ExpressionEqualityComparer();
            Assert.IsTrue(eq.Equals(m2, ((ExpressionTree<MemberExpression>)mu).Expression));

            Assert.ThrowsException<InvalidOperationException>(() => et.Update(a1, a1));
            Assert.ThrowsException<InvalidOperationException>(() => et.Update());
        }

        [TestMethod]
        public void ExpressionTree_Member_Static()
        {
            var me1 = ((Expression<Func<DateTime>>)(() => DateTime.Now)).Body;
            var me2 = ((Expression<Func<DateTime>>)(() => DateTime.Now)).Body;

            var et1 = (ExpressionTree<MemberExpression>)me1.ToExpressionTree();
            var et2 = (ExpressionTree<MemberExpression>)me2.ToExpressionTree();

            Assert.AreSame(me1, et1.Expression);

            Assert.IsFalse(et1.Equals(null));
            Assert.IsFalse(et1.Equals(42));
            Assert.IsTrue(et1.Equals(et1));
            Assert.IsTrue(et1.Equals(et2));
            Assert.IsTrue(et2.Equals(et1));

            Assert.AreEqual(et1.GetHashCode(), et2.GetHashCode());

            Assert.AreEqual("MemberAccess[DateTime DateTime::Now { get; }]()", et1.ToString());
        }

        [TestMethod]
        public void ExpressionTree_Member_Static_Update()
        {
            var me = ((Expression<Func<DateTime>>)(() => DateTime.Now)).Body;

            var et = (ExpressionTree<MemberExpression>)me.ToExpressionTree();

            var mu = et.Update();

            Assert.AreSame(et, mu);

            Assert.ThrowsException<InvalidOperationException>(() => et.Update(et));
        }

        [TestMethod]
        public void ExpressionTree_MemberInit()
        {
            var be1 = ((Expression<Func<Bar>>)(() => new Bar { Qux = "baz", Foo = { Xs = { 1 } } })).Body;
            var be2 = ((Expression<Func<Bar>>)(() => new Bar { Qux = "baz", Foo = { Xs = { 1 } } })).Body;

            var et1 = (ExpressionTree<MemberInitExpression>)be1.ToExpressionTree();
            var et2 = (ExpressionTree<MemberInitExpression>)be2.ToExpressionTree();

            Assert.AreSame(be1, et1.Expression);

            Assert.IsFalse(et1.Equals(null));
            Assert.IsFalse(et1.Equals(42));
            Assert.IsTrue(et1.Equals(et1));
            Assert.IsTrue(et1.Equals(et2));
            Assert.IsTrue(et2.Equals(et1));

            Assert.AreEqual(et1.GetHashCode(), et2.GetHashCode());

            Assert.AreEqual("MemberInit(New[new Bar()](), MemberAssignment[string Bar.Qux](Constant[(string)baz]()), MemberMemberBinding[Foo Bar.Foo { get; set; }](MemberListBinding[List<int> Foo.Xs { get; }](ElementInit[void List<int>.Add(int item)](Constant[(int)1]()))))", et1.ToString());
        }

        [TestMethod]
        public void ExpressionTree_MemberInit_Update()
        {
            var me = ((Expression<Func<Bar>>)(() => new Bar { Qux = "baz", Foo = { Xs = { 1 } } })).Body;

            var et = (ExpressionTree<MemberInitExpression>)me.ToExpressionTree();

            var ma = Expression.Bind(typeof(Bar).GetField("Qux"), Expression.Constant("foo"));

            var a0 = et.Children[0];
            var a1 = ma.ToExpressionTree();
            var a2 = et.Children[2];

            var mu = et.Update(a0, a1, a2);

            Assert.AreSame(a0, mu.Children[0]);
            Assert.AreSame(a1, mu.Children[1]);
            Assert.AreSame(a2, mu.Children[2]);

            var m2 = ((Expression<Func<Bar>>)(() => new Bar { Qux = "foo", Foo = { Xs = { 1 } } })).Body;

            var eq = new ExpressionEqualityComparer();
            Assert.IsTrue(eq.Equals(m2, ((ExpressionTree<MemberInitExpression>)mu).Expression));

            Assert.ThrowsException<InvalidOperationException>(() => et.Update(a0));
            Assert.ThrowsException<InvalidOperationException>(() => et.Update(a0, a1));
            Assert.ThrowsException<InvalidOperationException>(() => et.Update(a0, a1, a2, a2));
        }

        [TestMethod]
        public void ExpressionTree_New()
        {
            var ne1 = ((Expression<Func<TimeSpan>>)(() => new TimeSpan(1, 2, 3))).Body;
            var ne2 = ((Expression<Func<TimeSpan>>)(() => new TimeSpan(1, 2, 3))).Body;

            var et1 = (ExpressionTree<NewExpression>)ne1.ToExpressionTree();
            var et2 = (ExpressionTree<NewExpression>)ne2.ToExpressionTree();

            Assert.AreSame(ne1, et1.Expression);

            Assert.IsFalse(et1.Equals(null));
            Assert.IsFalse(et1.Equals(42));
            Assert.IsTrue(et1.Equals(et1));
            Assert.IsTrue(et1.Equals(et2));
            Assert.IsTrue(et2.Equals(et1));

            Assert.AreEqual(et1.GetHashCode(), et2.GetHashCode());

            Assert.AreEqual("New[new TimeSpan(int hours, int minutes, int seconds)](Constant[(int)1](), Constant[(int)2](), Constant[(int)3]())", et1.ToString());
        }

        [TestMethod]
        public void ExpressionTree_New_Update()
        {
            var ne = ((Expression<Func<TimeSpan>>)(() => new TimeSpan(1, 2, 3))).Body;

            var et = (ExpressionTree<NewExpression>)ne.ToExpressionTree();

            var a1 = et.Children[0];
            var a2 = Expression.Constant(4).ToExpressionTree();
            var a3 = et.Children[2];

            var nu = et.Update(a1, a2, a3);

            Assert.AreSame(a1, nu.Children[0]);
            Assert.AreSame(a2, nu.Children[1]);
            Assert.AreSame(a3, nu.Children[2]);

            var n2 = ((Expression<Func<TimeSpan>>)(() => new TimeSpan(1, 4, 3))).Body;

            var eq = new ExpressionEqualityComparer();
            Assert.IsTrue(eq.Equals(n2, ((ExpressionTree<NewExpression>)nu).Expression));

            Assert.ThrowsException<InvalidOperationException>(() => et.Update(a1, a2));
            Assert.ThrowsException<InvalidOperationException>(() => et.Update(a1, a2, a3, a3));
        }

        [TestMethod]
        public void ExpressionTree_NewArrayInit()
        {
            var be1 = ((Expression<Func<string[]>>)(() => new[] { "bar", "foo" })).Body;
            var be2 = ((Expression<Func<string[]>>)(() => new[] { "bar", "foo" })).Body;

            var et1 = (ExpressionTree<NewArrayExpression>)be1.ToExpressionTree();
            var et2 = (ExpressionTree<NewArrayExpression>)be2.ToExpressionTree();

            Assert.AreSame(be1, et1.Expression);

            Assert.IsFalse(et1.Equals(null));
            Assert.IsFalse(et1.Equals(42));
            Assert.IsTrue(et1.Equals(et1));
            Assert.IsTrue(et1.Equals(et2));
            Assert.IsTrue(et2.Equals(et1));

            Assert.AreEqual(et1.GetHashCode(), et2.GetHashCode());

            Assert.AreEqual("NewArrayInit[string[]](Constant[(string)bar](), Constant[(string)foo]())", et1.ToString());
        }

        [TestMethod]
        public void ExpressionTree_NewArrayBounds()
        {
            var be1 = ((Expression<Func<string[,]>>)(() => new string[1, 2])).Body;
            var be2 = ((Expression<Func<string[,]>>)(() => new string[1, 2])).Body;

            var et1 = (ExpressionTree<NewArrayExpression>)be1.ToExpressionTree();
            var et2 = (ExpressionTree<NewArrayExpression>)be2.ToExpressionTree();

            Assert.AreSame(be1, et1.Expression);

            Assert.IsFalse(et1.Equals(null));
            Assert.IsFalse(et1.Equals(42));
            Assert.IsTrue(et1.Equals(et1));
            Assert.IsTrue(et1.Equals(et2));
            Assert.IsTrue(et2.Equals(et1));

            Assert.AreEqual(et1.GetHashCode(), et2.GetHashCode());

            Assert.AreEqual("NewArrayBounds[string[,]](Constant[(int)1](), Constant[(int)2]())", et1.ToString());
        }

        [TestMethod]
        public void ExpressionTree_NewArray_Update()
        {
            var ne = ((Expression<Func<int[]>>)(() => new int[] { 1, 2, 3 })).Body;

            var et = (ExpressionTree<NewArrayExpression>)ne.ToExpressionTree();

            var a1 = et.Children[0];
            var a2 = Expression.Constant(4).ToExpressionTree();
            var a3 = et.Children[2];

            var nu = et.Update(a1, a2, a3);

            Assert.AreSame(a1, nu.Children[0]);
            Assert.AreSame(a2, nu.Children[1]);
            Assert.AreSame(a3, nu.Children[2]);

            var n2 = ((Expression<Func<int[]>>)(() => new int[] { 1, 4, 3 })).Body;

            var eq = new ExpressionEqualityComparer();
            Assert.IsTrue(eq.Equals(n2, ((ExpressionTree<NewArrayExpression>)nu).Expression));

            Assert.ThrowsException<InvalidOperationException>(() => et.Update(a1, a2));
            Assert.ThrowsException<InvalidOperationException>(() => et.Update(a1, a2, a3, a3));
        }

        [TestMethod]
        public void ExpressionTree_LambdaParameterInvocation()
        {
            var be1 = ((Expression<Func<Func<int, int>, int, int>>)((f, x) => f(x)));
            var be2 = ((Expression<Func<Func<int, int>, int, int>>)((g, y) => g(y)));

            var et1 = (ExpressionTree<LambdaExpression>)be1.ToExpressionTree();
            var et2 = (ExpressionTree<LambdaExpression>)be2.ToExpressionTree();

            Assert.AreSame(be1, et1.Expression);

            Assert.IsFalse(et1.Equals(null));
            Assert.IsFalse(et1.Equals(42));
            Assert.IsTrue(et1.Equals(et1));
            Assert.IsTrue(et1.Equals(et2));
            Assert.IsTrue(et2.Equals(et1));

            Assert.AreEqual(et1.GetHashCode(), et2.GetHashCode());

            Assert.AreEqual("Lambda[Func<Func<int, int>, int, int>](Invoke(Parameter[Func<int, int> f](), Parameter[int x]()), Parameter[Func<int, int> f](), Parameter[int x]())", et1.ToString());
        }

        [TestMethod]
        public void ExpressionTree_Lambda1_Update()
        {
            var le = (Expression<Func<int>>)(() => 42);

            var et = le.ToExpressionTree();

            var nt = (Expression<Func<int>>)(() => 43);
            var nb = nt.Body.ToExpressionTree();

            var lu = et.Update(nb);

            Assert.AreSame(nb, lu.Children[0]);

            var eq = new ExpressionEqualityComparer();
            Assert.IsTrue(eq.Equals(nt, ((ExpressionTree<Expression<Func<int>>>)lu).Expression));

            Assert.ThrowsException<InvalidOperationException>(() => et.Update(nb, Expression.Parameter(typeof(int)).ToExpressionTree()));
        }

        [TestMethod]
        public void ExpressionTree_Lambda2_Update()
        {
            var le = (Expression<Func<int, int, int>>)((x, y) => x);

            var et = le.ToExpressionTree();

            var nt = (Expression<Func<int, int, int>>)((x, y) => y);
            var nb = et.Children[2];

            var lu = et.Update(nb, et.Children[1], et.Children[2]);

            Assert.AreSame(nb, lu.Children[0]);
            Assert.AreSame(et.Children[1], lu.Children[1]);
            Assert.AreSame(et.Children[2], lu.Children[2]);

            var eq = new ExpressionEqualityComparer();
            Assert.IsTrue(eq.Equals(nt, ((ExpressionTree<Expression<Func<int, int, int>>>)lu).Expression));

            Assert.ThrowsException<InvalidOperationException>(() => et.Update(nb));
            Assert.ThrowsException<InvalidOperationException>(() => et.Update(nb, et.Children[1]));
        }

        [TestMethod]
        public void ExpressionTree_TypeBinary()
        {
            var te1 = Expression.TypeIs(Expression.Constant(2), typeof(int));
            var te2 = Expression.TypeIs(Expression.Constant(2), typeof(int));

            var et1 = (ExpressionTree<TypeBinaryExpression>)te1.ToExpressionTree();
            var et2 = (ExpressionTree<TypeBinaryExpression>)te2.ToExpressionTree();

            Assert.AreSame(te1, et1.Expression);

            Assert.IsFalse(et1.Equals(null));
            Assert.IsFalse(et1.Equals(42));
            Assert.IsTrue(et1.Equals(et1));
            Assert.IsTrue(et1.Equals(et2));
            Assert.IsTrue(et2.Equals(et1));

            Assert.AreEqual(et1.GetHashCode(), et2.GetHashCode());

            Assert.AreEqual("TypeIs[int](Constant[(int)2]())", et1.ToString());
        }

        [TestMethod]
        public void ExpressionTree_TypeBinary_Update()
        {
            var te = Expression.TypeIs(Expression.Constant(2), typeof(int));

            var et = (ExpressionTree<TypeBinaryExpression>)te.ToExpressionTree();

            var on = Expression.Constant(3).ToExpressionTree();

            var tu = et.Update(on);

            Assert.AreSame(on, tu.Children[0]);

            var t2 = Expression.TypeIs(Expression.Constant(3), typeof(int));

            var eq = new ExpressionEqualityComparer();
            Assert.IsTrue(eq.Equals(t2, ((ExpressionTree<TypeBinaryExpression>)tu).Expression));
        }

        [TestMethod]
        public void ExpressionTree_Unary_Standard()
        {
            var ue1 = Expression.Negate(Expression.Constant(2));
            var ue2 = Expression.Negate(Expression.Constant(2));

            var et1 = (ExpressionTree<UnaryExpression>)ue1.ToExpressionTree();
            var et2 = (ExpressionTree<UnaryExpression>)ue2.ToExpressionTree();

            Assert.AreSame(ue1, et1.Expression);

            Assert.IsFalse(et1.Equals(null));
            Assert.IsFalse(et1.Equals(42));
            Assert.IsTrue(et1.Equals(et1));
            Assert.IsTrue(et1.Equals(et2));
            Assert.IsTrue(et2.Equals(et1));

            Assert.AreEqual(et1.GetHashCode(), et2.GetHashCode());

            Assert.AreEqual("Negate(Constant[(int)2]())", et1.ToString());
        }

        [TestMethod]
        public void ExpressionTree_Unary_CustomOperator()
        {
            var ue1 = Expression.Negate(Expression.Constant(TimeSpan.FromSeconds(1)));
            var ue2 = Expression.Negate(Expression.Constant(TimeSpan.FromSeconds(1)));

            var et1 = (ExpressionTree<UnaryExpression>)ue1.ToExpressionTree();
            var et2 = (ExpressionTree<UnaryExpression>)ue2.ToExpressionTree();

            Assert.AreSame(ue1, et1.Expression);

            Assert.IsFalse(et1.Equals(null));
            Assert.IsFalse(et1.Equals(42));
            Assert.IsTrue(et1.Equals(et1));
            Assert.IsTrue(et1.Equals(et2));
            Assert.IsTrue(et2.Equals(et1));

            Assert.AreEqual(et1.GetHashCode(), et2.GetHashCode());

            Assert.AreEqual("Negate[TimeSpan TimeSpan::op_UnaryNegation(TimeSpan t)](Constant[(TimeSpan)00:00:01]())", et1.ToString());
        }

        [TestMethod]
        public void ExpressionTree_Unary_Convert_Standard()
        {
            var ue1 = Expression.Convert(Expression.Constant(2), typeof(long));
            var ue2 = Expression.Convert(Expression.Constant(2), typeof(long));

            var et1 = (ExpressionTree<UnaryExpression>)ue1.ToExpressionTree();
            var et2 = (ExpressionTree<UnaryExpression>)ue2.ToExpressionTree();

            Assert.AreSame(ue1, et1.Expression);

            Assert.IsFalse(et1.Equals(null));
            Assert.IsFalse(et1.Equals(42));
            Assert.IsTrue(et1.Equals(et1));
            Assert.IsTrue(et1.Equals(et2));
            Assert.IsTrue(et2.Equals(et1));

            Assert.AreEqual(et1.GetHashCode(), et2.GetHashCode());

            Assert.AreEqual("Convert[long](Constant[(int)2]())", et1.ToString());
        }

        [TestMethod]
        public void ExpressionTree_Unary_Convert_CustomOperator()
        {
            var dt = new DateTime(2013, 5, 29);

            var ue1 = Expression.Convert(Expression.Constant(dt), typeof(DateTimeOffset));
            var ue2 = Expression.Convert(Expression.Constant(dt), typeof(DateTimeOffset));

            var et1 = (ExpressionTree<UnaryExpression>)ue1.ToExpressionTree();
            var et2 = (ExpressionTree<UnaryExpression>)ue2.ToExpressionTree();

            Assert.AreSame(ue1, et1.Expression);

            Assert.IsFalse(et1.Equals(null));
            Assert.IsFalse(et1.Equals(42));
            Assert.IsTrue(et1.Equals(et1));
            Assert.IsTrue(et1.Equals(et2));
            Assert.IsTrue(et2.Equals(et1));

            Assert.AreEqual(et1.GetHashCode(), et2.GetHashCode());

            Assert.AreEqual("Convert[DateTimeOffset DateTimeOffset::op_Implicit(DateTime dateTime)](Constant[(DateTime)" + dt.ToString() + "]())", et1.ToString());
        }

        [TestMethod]
        public void ExpressionTree_Unary_Update()
        {
            var ue = Expression.Negate(Expression.Constant(2));

            var et = (ExpressionTree<UnaryExpression>)ue.ToExpressionTree();

            var on = Expression.Constant(3).ToExpressionTree();

            var uu = et.Update(on);

            Assert.AreSame(on, uu.Children[0]);

            var u2 = Expression.Negate(Expression.Constant(3));

            var eq = new ExpressionEqualityComparer();
            Assert.IsTrue(eq.Equals(u2, ((ExpressionTree<UnaryExpression>)uu).Expression));
        }

        [TestMethod]
        public void ExpressionTree_ElementInit_ArgumentChecking()
        {
            AssertEx.ThrowsException<ArgumentNullException>(() => new ElementInitExpressionTreeNode(elementInit: null), ex => Assert.AreEqual("elementInit", ex.ParamName));
        }

        [TestMethod]
        public void ExpressionTree_ElementInit()
        {
            var ei = Expression.ElementInit(typeof(List<int>).GetMethod("Add"), Expression.Constant(2));

            var tr = ei.ToExpressionTree();

            Assert.AreSame(ei, tr.ElementInit);
            Assert.AreEqual(1, tr.Children.Count);

            var ce = tr.Children[0] as ExpressionTree<ConstantExpression>;
            Assert.IsNotNull(ce);

            Assert.AreSame(ei.Arguments[0], ce.Expression);

            Assert.AreEqual("ElementInit[void List<int>.Add(int item)]", tr.Value.ToString());
        }

        [TestMethod]
        public void ExpressionTree_ElementInit_Update()
        {
            var ei = Expression.ElementInit(typeof(List<int>).GetMethod("Add"), Expression.Constant(2));

            var et = ei.ToExpressionTree();

            var ce = Expression.Constant(3);
            var a1 = ce.ToExpressionTree();

            var eu = et.Update(a1);
            var ev = et.Update((IEnumerable<ExpressionTree>)new[] { a1 });

            Assert.AreSame(a1, eu.Children[0]);
            Assert.AreSame(a1, ev.Children[0]);

            var e2 = Expression.ElementInit(typeof(List<int>).GetMethod("Add"), Expression.Constant(3));

            var eq = new ExpressionEqualityComparator();
            Assert.IsTrue(eq.Equals(e2, eu.ElementInit));
            Assert.IsTrue(eq.Equals(e2, ev.ElementInit));

            Assert.ThrowsException<InvalidOperationException>(() => et.Update());
            Assert.ThrowsException<InvalidOperationException>(() => et.Update(a1, a1));
        }

        [TestMethod]
        public void ExpressionTree_ElementInit_Update_ArgumentChecks()
        {
            var et = Expression.ElementInit(typeof(List<int>).GetMethod("Add"), Expression.Constant(2)).ToExpressionTree();

#pragma warning disable IDE0034 // Simplify 'default' expression (illustrative of method signature)
            AssertEx.ThrowsException<ArgumentNullException>(() => et.Update(default(IEnumerable<ExpressionTree>)), ex => Assert.AreEqual("children", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => et.Update(default(ExpressionTree[])), ex => Assert.AreEqual("children", ex.ParamName));
#pragma warning restore IDE0034 // Simplify 'default' expression
        }

        [TestMethod]
        public void ExpressionTree_MemberAssignment_ArgumentChecking()
        {
            AssertEx.ThrowsException<ArgumentNullException>(() => new MemberAssignmentExpressionTreeNode(memberAssignment: null), ex => Assert.AreEqual("memberAssignment", ex.ParamName));
        }

        [TestMethod]
        public void ExpressionTree_MemberAssignment()
        {
            var ma = Expression.Bind(typeof(Foo).GetProperty("Baz"), Expression.Constant(2));

            var et = ma.ToExpressionTree();

            var tr = et as MemberAssignmentExpressionTree;
            Assert.IsNotNull(tr);

            Assert.AreSame(ma, tr.MemberAssignment);
            Assert.AreEqual(1, tr.Children.Count);

            var ce = tr.Children[0] as ExpressionTree<ConstantExpression>;
            Assert.IsNotNull(ce);

            Assert.AreSame(ma.Expression, ce.Expression);

            Assert.AreEqual("MemberAssignment[int Foo.Baz { get; set; }]", et.Value.ToString());
        }

        [TestMethod]
        public void ExpressionTree_MemberAssignment_Update1()
        {
            var ma = Expression.Bind(typeof(Foo).GetProperty("Baz"), Expression.Constant(2));

            var et = ma.ToExpressionTree();

            var ce = Expression.Constant(3);
            var a1 = ce.ToExpressionTree();

            var eu = et.Update(a1);
            var ev = et.Update((IEnumerable<ExpressionTree>)new[] { a1 });

            Assert.AreSame(a1, eu.Children[0]);
            Assert.AreSame(a1, ev.Children[0]);

            var e2 = Expression.Bind(typeof(Foo).GetProperty("Baz"), Expression.Constant(3));

            var eq = new ExpressionEqualityComparator();
            Assert.IsTrue(eq.Equals(e2, ((MemberAssignmentExpressionTree)eu).MemberAssignment));
            Assert.IsTrue(eq.Equals(e2, ((MemberAssignmentExpressionTree)ev).MemberAssignment));

            Assert.ThrowsException<InvalidOperationException>(() => et.Update());
            Assert.ThrowsException<InvalidOperationException>(() => et.Update(a1, a1));
        }

        [TestMethod]
        public void ExpressionTree_MemberAssignment_Update2()
        {
            var ma = Expression.Bind(typeof(Foo).GetProperty("Baz"), Expression.Constant(2));

            var et = (MemberAssignmentExpressionTree)ma.ToExpressionTree();

            var ce = Expression.Constant(3);
            var a1 = ce.ToExpressionTree();

            var eu = et.Update(a1);
            var ev = et.Update((IEnumerable<ExpressionTree>)new[] { a1 });

            Assert.AreSame(a1, eu.Children[0]);
            Assert.AreSame(a1, ev.Children[0]);

            var e2 = Expression.Bind(typeof(Foo).GetProperty("Baz"), Expression.Constant(3));

            var eq = new ExpressionEqualityComparator();
            Assert.IsTrue(eq.Equals(e2, eu.MemberAssignment));
            Assert.IsTrue(eq.Equals(e2, ev.MemberAssignment));

            Assert.ThrowsException<InvalidOperationException>(() => et.Update());
            Assert.ThrowsException<InvalidOperationException>(() => et.Update(a1, a1));
        }

        [TestMethod]
        public void ExpressionTree_MemberAssignment_Update_ArgumentChecks()
        {
            var et = (MemberAssignmentExpressionTree)Expression.Bind(typeof(Foo).GetProperty("Baz"), Expression.Constant(2)).ToExpressionTree();

#pragma warning disable IDE0034 // Simplify 'default' expression (illustrative of method signature)
            AssertEx.ThrowsException<ArgumentNullException>(() => et.Update(default(IEnumerable<ExpressionTree>)), ex => Assert.AreEqual("children", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => et.Update(default(ExpressionTree[])), ex => Assert.AreEqual("children", ex.ParamName));
#pragma warning restore IDE0034 // Simplify 'default' expression
        }

        [TestMethod]
        public void ExpressionTree_MemberMemberBinding_ArgumentChecking()
        {
            AssertEx.ThrowsException<ArgumentNullException>(() => new MemberMemberBindingExpressionTreeNode(memberMemberBinding: null), ex => Assert.AreEqual("memberMemberBinding", ex.ParamName));
        }

        [TestMethod]
        public void ExpressionTree_MemberMemberBinding()
        {
            var mb = Expression.MemberBind(typeof(Bar).GetProperty("Foo"), Expression.Bind(typeof(Foo).GetProperty("Baz"), Expression.Constant(2)));

            var et = mb.ToExpressionTree();

            var tr = et as MemberMemberBindingExpressionTree;
            Assert.IsNotNull(tr);

            Assert.AreSame(mb, tr.MemberMemberBinding);
            Assert.AreEqual(1, tr.Children.Count);

            var ma = tr.Children[0] as MemberAssignmentExpressionTree;
            Assert.IsNotNull(ma);

            Assert.AreSame(mb.Bindings[0], ma.MemberAssignment);

            Assert.AreEqual("MemberMemberBinding[Foo Bar.Foo { get; set; }]", et.Value.ToString());
        }

        [TestMethod]
        public void ExpressionTree_MemberMemberBinding_Update1()
        {
            var baz = Expression.Bind(typeof(Foo).GetProperty("Baz"), Expression.Constant(2));
            var bar = Expression.Bind(typeof(Foo).GetProperty("Bar"), Expression.Constant("foo"));

            var mb = Expression.MemberBind(typeof(Bar).GetProperty("Foo"), baz);

            var et = mb.ToExpressionTree();

            var a1 = bar.ToExpressionTree();

            var eu = et.Update(a1);
            var ev = et.Update((IEnumerable<MemberBindingExpressionTree>)new[] { a1 });

            Assert.AreSame(a1, eu.Children[0]);
            Assert.AreSame(a1, ev.Children[0]);

            var m2 = Expression.MemberBind(typeof(Bar).GetProperty("Foo"), bar);

            var eq = new ExpressionEqualityComparator();
            Assert.IsTrue(eq.Equals(m2, ((MemberMemberBindingExpressionTree)eu).MemberMemberBinding));
            Assert.IsTrue(eq.Equals(m2, ((MemberMemberBindingExpressionTree)ev).MemberMemberBinding));

            Assert.ThrowsException<InvalidOperationException>(() => et.Update());
            Assert.ThrowsException<InvalidOperationException>(() => et.Update(a1, a1));
        }

        [TestMethod]
        public void ExpressionTree_MemberMemberBinding_Update2()
        {
            var baz = Expression.Bind(typeof(Foo).GetProperty("Baz"), Expression.Constant(2));
            var bar = Expression.Bind(typeof(Foo).GetProperty("Bar"), Expression.Constant("foo"));

            var mb = Expression.MemberBind(typeof(Bar).GetProperty("Foo"), baz);

            var et = (MemberMemberBindingExpressionTree)mb.ToExpressionTree();

            var a1 = bar.ToExpressionTree();

            var eu = et.Update(a1);
            var ev = et.Update((IEnumerable<MemberBindingExpressionTree>)new[] { a1 });

            Assert.AreSame(a1, eu.Children[0]);
            Assert.AreSame(a1, ev.Children[0]);

            var m2 = Expression.MemberBind(typeof(Bar).GetProperty("Foo"), bar);

            var eq = new ExpressionEqualityComparator();
            Assert.IsTrue(eq.Equals(m2, eu.MemberMemberBinding));
            Assert.IsTrue(eq.Equals(m2, ev.MemberMemberBinding));

            Assert.ThrowsException<InvalidOperationException>(() => et.Update());
            Assert.ThrowsException<InvalidOperationException>(() => et.Update(a1, a1));
        }

        [TestMethod]
        public void ExpressionTree_MemberMemberBinding_Update_ArgumentChecks()
        {
            var et = (MemberMemberBindingExpressionTree)Expression.MemberBind(typeof(Bar).GetProperty("Foo"), Expression.Bind(typeof(Foo).GetProperty("Baz"), Expression.Constant(2))).ToExpressionTree();

#pragma warning disable IDE0034 // Simplify 'default' expression (illustrative of method signature)
            AssertEx.ThrowsException<ArgumentNullException>(() => et.Update(default(IEnumerable<MemberBindingExpressionTree>)), ex => Assert.AreEqual("children", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => et.Update(default(MemberBindingExpressionTree[])), ex => Assert.AreEqual("children", ex.ParamName));
#pragma warning restore IDE0034 // Simplify 'default' expression
        }

        [TestMethod]
        public void ExpressionTree_MemberListBinding_ArgumentChecking()
        {
            AssertEx.ThrowsException<ArgumentNullException>(() => new MemberListBindingExpressionTreeNode(memberListBinding: null), ex => Assert.AreEqual("memberListBinding", ex.ParamName));
        }

        [TestMethod]
        public void ExpressionTree_MemberListBinding()
        {
            var lb = Expression.ListBind(typeof(Foo).GetProperty("Xs"), Expression.ElementInit(typeof(List<int>).GetMethod("Add"), Expression.Constant(2)));

            var et = lb.ToExpressionTree();

            var tr = et as MemberListBindingExpressionTree;
            Assert.IsNotNull(tr);

            Assert.AreSame(lb, tr.MemberListBinding);
            Assert.AreEqual(1, tr.Children.Count);

            var ei = tr.Children[0] as ElementInitExpressionTree;
            Assert.IsNotNull(ei);

            Assert.AreSame(lb.Initializers[0], ei.ElementInit);

            Assert.AreEqual("MemberListBinding[List<int> Foo.Xs { get; }]", et.Value.ToString());
        }

        [TestMethod]
        public void ExpressionTree_MemberListBinding_Update1()
        {
            var ei1 = Expression.ElementInit(typeof(List<int>).GetMethod("Add"), Expression.Constant(2));
            var ei2 = Expression.ElementInit(typeof(List<int>).GetMethod("Add"), Expression.Constant(3));

            var lb = Expression.ListBind(typeof(Foo).GetProperty("Xs"), ei1);

            var et = lb.ToExpressionTree();

            var a1 = ei2.ToExpressionTree();

            var eu = et.Update(a1);
            var ev = et.Update((IEnumerable<ElementInitExpressionTree>)new[] { a1 });

            Assert.AreSame(a1, eu.Children[0]);
            Assert.AreSame(a1, ev.Children[0]);

            var l2 = Expression.ListBind(typeof(Foo).GetProperty("Xs"), ei2);

            var eq = new ExpressionEqualityComparator();
            Assert.IsTrue(eq.Equals(l2, ((MemberListBindingExpressionTree)eu).MemberListBinding));
            Assert.IsTrue(eq.Equals(l2, ((MemberListBindingExpressionTree)ev).MemberListBinding));

            Assert.ThrowsException<InvalidOperationException>(() => et.Update());
            Assert.ThrowsException<InvalidOperationException>(() => et.Update(a1, a1));
        }

        [TestMethod]
        public void ExpressionTree_MemberListBinding_Update2()
        {
            var ei1 = Expression.ElementInit(typeof(List<int>).GetMethod("Add"), Expression.Constant(2));
            var ei2 = Expression.ElementInit(typeof(List<int>).GetMethod("Add"), Expression.Constant(3));

            var lb = Expression.ListBind(typeof(Foo).GetProperty("Xs"), ei1);

            var et = (MemberListBindingExpressionTree)lb.ToExpressionTree();

            var a1 = ei2.ToExpressionTree();

            var eu = et.Update(a1);
            var ev = et.Update((IEnumerable<ElementInitExpressionTree>)new[] { a1 });

            Assert.AreSame(a1, eu.Children[0]);
            Assert.AreSame(a1, ev.Children[0]);

            var l2 = Expression.ListBind(typeof(Foo).GetProperty("Xs"), ei2);

            var eq = new ExpressionEqualityComparator();
            Assert.IsTrue(eq.Equals(l2, eu.MemberListBinding));
            Assert.IsTrue(eq.Equals(l2, ev.MemberListBinding));

            Assert.ThrowsException<InvalidOperationException>(() => et.Update());
            Assert.ThrowsException<InvalidOperationException>(() => et.Update(a1, a1));
        }

        [TestMethod]
        public void ExpressionTree_MemberListBinding_Update_ArgumentChecks()
        {
            var et = (MemberListBindingExpressionTree)Expression.ListBind(typeof(Foo).GetProperty("Xs"), Expression.ElementInit(typeof(List<int>).GetMethod("Add"), Expression.Constant(2))).ToExpressionTree();

#pragma warning disable IDE0034 // Simplify 'default' expression (illustrative of method signature)
            AssertEx.ThrowsException<ArgumentNullException>(() => et.Update(default(IEnumerable<ElementInitExpressionTree>)), ex => Assert.AreEqual("children", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => et.Update(default(ElementInitExpressionTree[])), ex => Assert.AreEqual("children", ex.ParamName));
#pragma warning restore IDE0034 // Simplify 'default' expression
        }

        [TestMethod]
        public void ExpressionTree_TypeSystem_Expression()
        {
            var et = Expression.Constant(42).ToExpressionTree();
            var typed = (ITyped)et;
            var type = typed.GetType();
            Assert.IsTrue(type.Equals(new ClrType(typeof(int))));
        }

        [TestMethod]
        public void ExpressionTree_TypeSystem_MemberBinding()
        {
            var et = Expression.Bind(typeof(Foo).GetProperty("Baz"), Expression.Constant(2)).ToExpressionTree();
            var typed = (ITyped)et;
            var type = typed.GetType();
            Assert.IsTrue(type.Equals(ClrType.Void));
        }

        [TestMethod]
        public void ExpressionTree_TypeSystem_ElementInit()
        {
            var et = Expression.ElementInit(typeof(List<int>).GetMethod("Add"), Expression.Constant(2)).ToExpressionTree();
            var typed = (ITyped)et;
            var type = typed.GetType();
            Assert.IsTrue(type.Equals(ClrType.Void));
        }

        private sealed class Bar
        {
#pragma warning disable 0649
            public string Qux;
#pragma warning restore

            public Foo Foo
            {
                get;
                set;
            }
        }

        private sealed class Foo
        {
            public List<int> Xs => throw new NotImplementedException();

            public int Baz
            {
                get;
                set;
            }

            public string Bar
            {
                get;
                set;
            }
        }
    }
}
