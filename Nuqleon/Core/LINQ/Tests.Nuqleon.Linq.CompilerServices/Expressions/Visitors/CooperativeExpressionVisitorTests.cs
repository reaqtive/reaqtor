// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - May 2013 - Created this file.
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.CompilerServices;
using System.Linq.Expressions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.System.Linq.CompilerServices
{
    [TestClass]
    public class CooperativeExpressionVisitorTests
    {
        [TestMethod]
        public void CooperativeExpressionVisitor_ArgumentChecking()
        {
            new MyCooperativeVisitor1().Test();
        }

        private sealed class MyCooperativeVisitor1 : CooperativeExpressionVisitor
        {
            public void Test()
            {
                AssertEx.ThrowsException<ArgumentNullException>(() => base.VisitBinary(node: null), ex => Assert.AreEqual("node", ex.ParamName));
                AssertEx.ThrowsException<ArgumentNullException>(() => base.VisitIndex(node: null), ex => Assert.AreEqual("node", ex.ParamName));
                AssertEx.ThrowsException<ArgumentNullException>(() => base.VisitMember(node: null), ex => Assert.AreEqual("node", ex.ParamName));
                AssertEx.ThrowsException<ArgumentNullException>(() => base.VisitMethodCall(node: null), ex => Assert.AreEqual("node", ex.ParamName));
                AssertEx.ThrowsException<ArgumentNullException>(() => base.VisitNew(node: null), ex => Assert.AreEqual("node", ex.ParamName));
                AssertEx.ThrowsException<ArgumentNullException>(() => base.VisitUnary(node: null), ex => Assert.AreEqual("node", ex.ParamName));
            }
        }

        [TestMethod]
        public void CooperativeExpressionVisitor_Binary_NoRewrite()
        {
            var cv = new CooperativeExpressionVisitor();

            var b1 = (Expression<Func<int, int, int>>)((a, b) => a + b);
            Assert.AreSame(b1, cv.Visit(b1));

            var b2 = (Expression<Func<DateTime, TimeSpan, DateTime>>)((a, b) => a + b);
            Assert.AreSame(b2, cv.Visit(b2));
        }

        [TestMethod]
        public void CooperativeExpressionVisitor_Binary_Rewrite1()
        {
            var cv = new CooperativeExpressionVisitor();

            var b1 = (Expression<Func<BinOp, BinOp, BinOp>>)((a, b) => a + b);

            var res = cv.Visit(b1.Body);
            var br = res as BinaryExpression;

            Assert.IsNotNull(br);
            Assert.AreEqual(ExpressionType.Subtract, br.NodeType);
            Assert.AreSame(b1.Parameters[0], br.Left);
            Assert.AreSame(b1.Parameters[1], br.Right);
        }

        [TestMethod]
        public void CooperativeExpressionVisitor_Binary_Rewrite2()
        {
            var cv = new CooperativeExpressionVisitor();

            var b1 = (Expression<Func<BinOp, BinOp, BinOp, BinOp>>)((a, b, c) => a * b + c);

            var res = cv.Visit(b1.Body);
            var br = res as BinaryExpression;

            Assert.IsNotNull(br);
            Assert.AreEqual(ExpressionType.Subtract, br.NodeType);

            var bi = br.Left as BinaryExpression;
            Assert.IsNotNull(bi);
            Assert.AreEqual(ExpressionType.Divide, bi.NodeType);

            Assert.AreSame(b1.Parameters[0], bi.Left);
            Assert.AreSame(b1.Parameters[1], bi.Right);
            Assert.AreSame(b1.Parameters[2], br.Right);
        }

        [TestMethod]
        public void CooperativeExpressionVisitor_Binary_Rewrite3()
        {
            var cv = new CooperativeExpressionVisitor();

            var b1 = (Expression<Func<BinOp, BinOp, BinOp, BinOp>>)((a, b, c) => a % (b + c));

            var res = cv.Visit(b1.Body);
            var br = res as BinaryExpression;

            Assert.IsNotNull(br);
            Assert.AreEqual(ExpressionType.Modulo, br.NodeType);

            var bi = br.Right as BinaryExpression;
            Assert.IsNotNull(bi);
            Assert.AreEqual(ExpressionType.Subtract, bi.NodeType);

            Assert.AreSame(b1.Parameters[0], br.Left);
            Assert.AreSame(b1.Parameters[1], bi.Left);
            Assert.AreSame(b1.Parameters[2], bi.Right);
        }

        private sealed class BinOp
        {
            [Visitor(typeof(BinOpRewriter))]
            public static BinOp operator +(BinOp _1, BinOp _2) => throw new NotImplementedException();

            public static BinOp operator -(BinOp _1, BinOp _2) => throw new NotImplementedException();

            [Visitor(typeof(BinOpRewriter))]
            public static BinOp operator *(BinOp _1, BinOp _2) => throw new NotImplementedException();

            public static BinOp operator /(BinOp _1, BinOp _2) => throw new NotImplementedException();

            public static BinOp operator %(BinOp _1, BinOp _2) => throw new NotImplementedException();

            private sealed class BinOpRewriter : IRecursiveExpressionVisitor
            {
                public bool TryVisit(Expression expression, Func<Expression, Expression> visit, out Expression result)
                {
                    var be = expression as BinaryExpression;

                    Assert.IsNotNull(be);
                    Assert.IsNotNull(be.Method);

                    var l = visit(be.Left);
                    var r = visit(be.Right);

                    if (be.NodeType == ExpressionType.Add)
                    {
                        result = Expression.Subtract(l, r);
                    }
                    else if (be.NodeType == ExpressionType.Multiply)
                    {
                        result = Expression.Divide(l, r);
                    }
                    else
                    {
                        throw new NotSupportedException();
                    }

                    return true;
                }
            }
        }

        [TestMethod]
        public void CooperativeExpressionVisitor_Index_NoRewrite()
        {
            var cv = new CooperativeExpressionVisitor();

            var p1 = Expression.Parameter(typeof(List<string>));
            var i1 = Expression.Lambda(Expression.MakeIndex(p1, typeof(List<string>).GetProperty("Item"), new[] { Expression.Constant(42) }), p1);
            Assert.AreSame(i1, cv.Visit(i1));
        }

        [TestMethod]
        public void CooperativeExpressionVisitor_Index_Rewrite1()
        {
            var cv = new CooperativeExpressionVisitor();

            var p1 = Expression.Parameter(typeof(IndOp));
            var i1 = Expression.Lambda(Expression.MakeIndex(p1, typeof(IndOp).GetProperty("Item"), new[] { Expression.Constant(42) }), p1);

            var res = cv.Visit(i1.Body);
            var ir = res as MethodCallExpression;

            Assert.IsNotNull(ir);
            Assert.AreEqual("Get", ir.Method.Name);

            Assert.AreSame(i1.Parameters[0], ir.Object);
        }

        private sealed class IndOp
        {
            [Visitor(typeof(IndOpRewriter))]
            public string this[int _] => throw new NotImplementedException();

            public string Get(int _) => throw new NotImplementedException();

            private sealed class IndOpRewriter : IRecursiveExpressionVisitor
            {
                public bool TryVisit(Expression expression, Func<Expression, Expression> visit, out Expression result)
                {
                    var ie = expression as IndexExpression;

                    Assert.IsNotNull(ie);

                    var o = visit(ie.Object);
                    var a = visit(ie.Arguments[0]);

                    result = Expression.Call(o, typeof(IndOp).GetMethod("Get"), a);

                    return true;
                }
            }
        }

#pragma warning disable IDE0050 // Convert to tuple. (Tests for anonymous types.)

        [TestMethod]
        public void CooperativeExpressionVisitor_Member_NoRewrite()
        {
            var cv = new CooperativeExpressionVisitor();

            var m1 = (Expression<Func<Exception, string>>)(ex => ex.Message);
            Assert.AreSame(m1, cv.Visit(m1));

            var m2 = (Expression<Func<int>>)(() => new { a = 42, b = "bar" }.a);
            Assert.AreSame(m2, cv.Visit(m2));
        }

#pragma warning restore IDE0050

        [TestMethod]
        public void CooperativeExpressionVisitor_Member_Rewrite1()
        {
            var cv = new CooperativeExpressionVisitor();

            var m1 = (Expression<Func<MemOp, int>>)(m => m.Bar);

            var res = cv.Visit(m1.Body);
            var mr = res as MemberExpression;

            Assert.IsNotNull(mr);
            Assert.AreSame(m1.Parameters[0], mr.Expression);
            Assert.AreEqual("Baz", mr.Member.Name);
        }

        [TestMethod]
        public void CooperativeExpressionVisitor_Member_Rewrite2()
        {
            var cv = new CooperativeExpressionVisitor();

            var m1 = (Expression<Func<int>>)(() => MemOp.Foo);

            var res = cv.Visit(m1.Body);
            var mr = res as MemberExpression;

            Assert.IsNotNull(mr);
            Assert.IsNull(mr.Expression);
            Assert.AreEqual("Foz", mr.Member.Name);
        }

        private sealed class MemOp
        {
            [Visitor(typeof(MemOpRewriter))]
            public static int Foo { get; private set; }

            public static int Foz { get; private set; }

            [Visitor(typeof(MemOpRewriter))]
            public int Bar { get; private set; }

            public int Baz { get; private set; }

            private sealed class MemOpRewriter : IRecursiveExpressionVisitor
            {
                public bool TryVisit(Expression expression, Func<Expression, Expression> visit, out Expression result)
                {
                    var me = expression as MemberExpression;

                    Assert.IsNotNull(me);

                    if (me.Member.Name == "Foo")
                    {
                        result = Expression.MakeMemberAccess(expression: null, typeof(MemOp).GetProperty("Foz"));
                    }
                    else if (me.Member.Name == "Bar")
                    {
                        result = Expression.MakeMemberAccess(visit(me.Expression), typeof(MemOp).GetProperty("Baz"));
                    }
                    else
                    {
                        throw new NotSupportedException();
                    }

                    return true;
                }
            }
        }

        [TestMethod]
        public void CooperativeExpressionVisitor_MethodCall_NoRewrite()
        {
            var cv = new CooperativeExpressionVisitor();

            var m1 = (Expression<Action<string>>)(s => Console.WriteLine(s));
            Assert.AreSame(m1, cv.Visit(m1));

            var m2 = (Expression<Func<TimeSpan, TimeSpan, TimeSpan>>)((t1, t2) => t1.Add(t2));
            Assert.AreSame(m2, cv.Visit(m2));
        }

        [TestMethod]
        public void CooperativeExpressionVisitor_MethodCall_Rewrite1()
        {
            var cv = new CooperativeExpressionVisitor();

            var m1 = (Expression<Func<MethOp, int, int>>)((m, x) => m.Bar(x));

            var res = cv.Visit(m1.Body);
            var mr = res as MethodCallExpression;

            Assert.IsNotNull(mr);
            Assert.AreSame(m1.Parameters[0], mr.Object);
            Assert.AreEqual("Baz", mr.Method.Name);
            Assert.AreSame(m1.Parameters[1], mr.Arguments[0]);
        }

        [TestMethod]
        public void CooperativeExpressionVisitor_MethodCall_Rewrite2()
        {
            var cv = new CooperativeExpressionVisitor();

            var m1 = (Expression<Func<int, int>>)(x => MethOp.Foo(x));

            var res = cv.Visit(m1.Body);
            var mr = res as MethodCallExpression;

            Assert.IsNotNull(mr);
            Assert.IsNull(mr.Object);
            Assert.AreEqual("Foz", mr.Method.Name);
            Assert.AreSame(m1.Parameters[0], mr.Arguments[0]);
        }

        [TestMethod]
        public void CooperativeExpressionVisitor_MethodCall_Rewrite3()
        {
            var cv = new CooperativeExpressionVisitor();

            var m1 = (Expression<Func<MethOp, int, int>>)((m, x) => MethOp.Foo(MethOp.Qux(m.Bar(x))));

            var res = cv.Visit(m1.Body);
            var mr = res as MethodCallExpression;

            Assert.IsNotNull(mr);
            Assert.IsNull(mr.Object);
            Assert.AreEqual("Foz", mr.Method.Name);

            var mi = mr.Arguments[0] as MethodCallExpression;

            Assert.IsNotNull(mi);
            Assert.IsNull(mi.Object);
            Assert.AreEqual("Qux", mi.Method.Name);

            var mj = mi.Arguments[0] as MethodCallExpression;

            Assert.IsNotNull(mj);
            Assert.AreSame(m1.Parameters[0], mj.Object);
            Assert.AreSame(m1.Parameters[1], mj.Arguments[0]);
        }

        private sealed class MethOp
        {
            [Visitor(typeof(MethOpRewriter))]
            public static int Foo(int x)
            {
                throw new NotImplementedException();
            }

            public static int Foz(int x)
            {
                throw new NotImplementedException();
            }

            [Visitor(typeof(MethOpRewriter))]
            public int Bar(int x)
            {
                throw new NotImplementedException();
            }

            public int Baz(int x)
            {
                throw new NotImplementedException();
            }

            public static int Qux(int x)
            {
                throw new NotImplementedException();
            }

            private sealed class MethOpRewriter : IRecursiveExpressionVisitor
            {
                public bool TryVisit(Expression expression, Func<Expression, Expression> visit, out Expression result)
                {
                    var me = expression as MethodCallExpression;

                    Assert.IsNotNull(me);

                    if (me.Method.Name == "Foo")
                    {
                        result = Expression.Call(instance: null, typeof(MethOp).GetMethod("Foz"), visit(me.Arguments[0]));
                    }
                    else if (me.Method.Name == "Bar")
                    {
                        result = Expression.Call(visit(me.Object), typeof(MethOp).GetMethod("Baz"), visit(me.Arguments[0]));
                    }
                    else
                    {
                        throw new NotSupportedException();
                    }

                    return true;
                }
            }
        }

#pragma warning disable IDE0050 // Convert to tuple. (Tests for anonymous types.)

        [TestMethod]
        public void CooperativeExpressionVisitor_New_NoRewrite()
        {
            var cv = new CooperativeExpressionVisitor();

            var n1 = (Expression<Func<Exception>>)(() => new Exception("foo"));
            Assert.AreSame(n1, cv.Visit(n1));

            var n2 = (Expression<Func<object>>)(() => new { a = 42, b = "bar" });
            Assert.AreSame(n2, cv.Visit(n2));

            var n3 = Expression.Lambda(Expression.New(typeof(int)));
            Assert.AreSame(n3, cv.Visit(n3));
        }

#pragma warning restore IDE0050

        [TestMethod]
        public void CooperativeExpressionVisitor_New_Rewrite1()
        {
            var cv = new CooperativeExpressionVisitor();

            var n1 = (Expression<Func<int, string, NewOp>>)((x, s) => new NewOp(x, s));

            var res = cv.Visit(n1.Body);
            var nr = res as NewExpression;

            Assert.IsNotNull(nr);
            Assert.IsTrue(new[] { typeof(string), typeof(int) }.SequenceEqual(nr.Constructor.GetParameters().Select(p => p.ParameterType)));
            Assert.AreSame(n1.Parameters[1], nr.Arguments[0]);
            Assert.AreSame(n1.Parameters[0], nr.Arguments[1]);
        }

        private sealed class NewOp
        {
            [Visitor(typeof(NewOpRewriter))]
            public NewOp(int x, string y)
            {
                throw new NotImplementedException();
            }

            public NewOp(string y, int x)
            {
                throw new NotImplementedException();
            }

            private sealed class NewOpRewriter : IRecursiveExpressionVisitor
            {
                public bool TryVisit(Expression expression, Func<Expression, Expression> visit, out Expression result)
                {
                    var ne = expression as NewExpression;

                    Assert.IsNotNull(ne);

                    if (ne.Constructor.GetParameters().Select(p => p.ParameterType).SequenceEqual(new[] { typeof(int), typeof(string) }))
                    {
                        result = Expression.New(typeof(NewOp).GetConstructor(new[] { typeof(string), typeof(int) }), visit(ne.Arguments[1]), visit(ne.Arguments[0]));
                    }
                    else
                    {
                        throw new NotSupportedException();
                    }

                    return true;
                }
            }
        }

        [TestMethod]
        public void CooperativeExpressionVisitor_Unary_NoRewrite()
        {
            var cv = new CooperativeExpressionVisitor();

            var u1 = (Expression<Func<int, int>>)(x => -x);
            Assert.AreSame(u1, cv.Visit(u1));

            var u2 = (Expression<Func<TimeSpan, TimeSpan>>)(x => -x);
            Assert.AreSame(u2, cv.Visit(u2));
        }

        [TestMethod]
        public void CooperativeExpressionVisitor_Unary_Rewrite1()
        {
            var cv = new CooperativeExpressionVisitor();

            var u1 = (Expression<Func<UnOp, UnOp>>)(x => -x);

            var res = cv.Visit(u1.Body);
            var br = res as UnaryExpression;

            Assert.IsNotNull(br);
            Assert.AreEqual(ExpressionType.UnaryPlus, br.NodeType);
            Assert.AreSame(u1.Parameters[0], br.Operand);
        }

        [TestMethod]
        public void CooperativeExpressionVisitor_Unary_Rewrite2()
        {
            var cv = new CooperativeExpressionVisitor();

            var u1 = (Expression<Func<UnOp, UnOp>>)(x => !-x);

            var res = cv.Visit(u1.Body);
            var br = res as UnaryExpression;

            Assert.IsNotNull(br);
            Assert.AreEqual(ExpressionType.Not, br.NodeType);

            var bi = br.Operand as UnaryExpression;
            Assert.IsNotNull(bi);
            Assert.AreEqual(ExpressionType.UnaryPlus, bi.NodeType);

            Assert.AreSame(u1.Parameters[0], bi.Operand);
        }

        [TestMethod]
        public void CooperativeExpressionVisitor_Unary_Rewrite3()
        {
            var cv = new CooperativeExpressionVisitor();

            var u1 = (Expression<Func<UnOp, UnOp>>)(x => -!x);

            var res = cv.Visit(u1.Body);
            var br = res as UnaryExpression;

            Assert.IsNotNull(br);
            Assert.AreEqual(ExpressionType.UnaryPlus, br.NodeType);

            var bi = br.Operand as UnaryExpression;
            Assert.IsNotNull(bi);
            Assert.AreEqual(ExpressionType.Not, bi.NodeType);

            Assert.AreSame(u1.Parameters[0], bi.Operand);
        }

        private sealed class UnOp
        {
#pragma warning disable IDE0060 // Remove unused parameter (https://github.com/dotnet/roslyn/issues/32852)
            [Visitor(typeof(UnOpRewriter))]
            public static UnOp operator -(UnOp a) => throw new NotImplementedException();

            public static UnOp operator +(UnOp a) => throw new NotImplementedException();

            public static UnOp operator !(UnOp a) => throw new NotImplementedException();
#pragma warning restore IDE0060 // Remove unused parameter

            private sealed class UnOpRewriter : IRecursiveExpressionVisitor
            {
                public bool TryVisit(Expression expression, Func<Expression, Expression> visit, out Expression result)
                {
                    var be = expression as UnaryExpression;

                    Assert.IsNotNull(be);
                    Assert.IsNotNull(be.Method);

                    var o = visit(be.Operand);

                    if (be.NodeType == ExpressionType.Negate)
                    {
                        result = Expression.UnaryPlus(o);
                    }
                    else
                    {
                        throw new NotSupportedException();
                    }

                    return true;
                }
            }
        }
    }
}
