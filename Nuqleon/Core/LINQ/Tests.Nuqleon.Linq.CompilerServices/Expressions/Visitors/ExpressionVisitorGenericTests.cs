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
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.CompilerServices;
using System.Linq.Expressions;

namespace Tests.System.Linq.CompilerServices
{
    [TestClass]
    public class ExpressionVisitorGenericTests
    {
        [TestMethod]
        public void ExpressionVisitorGeneric_ArgumentChecking()
        {
            new MyVisitor().TestArgumentChecks();
        }

        [TestMethod]
        public void ExpressionVisitorGeneric_Extension1()
        {
            Assert.ThrowsException<NotImplementedException>(() => _ = new MyVisitor().Visit(new MyExt()));
        }

        [TestMethod]
        public void ExpressionVisitorGeneric_Extension2()
        {
            var e = new MyExt();
            var f = new MyVisitor2().Visit(e);
            Assert.AreSame(e, f);
        }

        private sealed class MyExt : Expression
        {
            public override ExpressionType NodeType => ExpressionType.Extension;
        }

        [TestMethod]
        public void ExpressionVisitorGeneric_InvalidExpression()
        {
            Assert.ThrowsException<NotSupportedException>(() => _ = new MyVisitor().Visit(new BrokenExpression()));
        }

        private sealed class BrokenExpression : Expression
        {
            public override ExpressionType NodeType => (ExpressionType)65432;
        }

        [TestMethod]
        public void ExpressionVisitorGeneric_Null()
        {
            Assert.IsNull(new MyVisitor().Visit(node: null));
        }

        [TestMethod]
        public void ExpressionVisitorGeneric_Simple1()
        {
            var v = new MyVisitor();

            var r = v.Visit(Expression.Add(Expression.Constant(2), Expression.Constant(3)));

            var e = new ExpressionEqualityComparer();
            Assert.IsTrue(e.Equals(r, Expression.Subtract(Expression.Constant(4), Expression.Constant(6))));
        }

        [TestMethod]
        public void ExpressionVisitorGeneric_Simple2()
        {
            var v = new MyVisitor();

            var r = v.Visit(Expression.UnaryPlus(Expression.Constant(2)));

            var e = new ExpressionEqualityComparer();
            Assert.IsTrue(e.Equals(r, Expression.Negate(Expression.Constant(4))));
        }

        [TestMethod]
        public void ExpressionVisitorGeneric_Simple3()
        {
            var v = new MyVisitor();

            var r = v.Visit(Expression.Default(typeof(int)));

            var e = new ExpressionEqualityComparer();
            Assert.IsTrue(e.Equals(r, Expression.Default(typeof(string))));
        }

        [TestMethod]
        public void ExpressionVisitorGeneric_Simple4()
        {
            var v = new MyVisitor();

            var r = v.Visit(Expression.Block(Expression.Constant(2), Expression.Constant(3)));

            var e = new ExpressionEqualityComparer();
            Assert.IsTrue(e.Equals(r, Expression.Block(Expression.Constant(6), Expression.Constant(4))));
        }

        [TestMethod]
        public void ExpressionVisitorGeneric_Simple5()
        {
            var v = new MyVisitor();

            var r = v.Visit(Expression.TypeIs(Expression.Constant(2), typeof(int)));

            var e = new ExpressionEqualityComparer();
            Assert.IsTrue(e.Equals(r, Expression.TypeIs(Expression.Constant(4), typeof(string))));
        }

        [TestMethod]
        public void ExpressionVisitorGeneric_Simple6()
        {
            var v = new MyVisitor();

            var r = v.Visit(Expression.Condition(Expression.Constant(true), Expression.Constant(2), Expression.Constant(3)));

            var e = new ExpressionEqualityComparer();
            Assert.IsTrue(e.Equals(r, Expression.Condition(Expression.Constant(false), Expression.Constant(6), Expression.Constant(4))));
        }

        [TestMethod]
        public void ExpressionVisitorGeneric_Simple7()
        {
            var v = new MyVisitor();

            var o = (Expression<Func<List<int>>>)(() => new List<int> { 2, 3 });
            var r = v.Visit(o.Body);
            var n = (Expression<Func<HashSet<int>>>)(() => new HashSet<int> { 6, 4 });

            var e = new ExpressionEqualityComparer();
            Assert.IsTrue(e.Equals(r, n.Body));
        }

        [TestMethod]
        public void ExpressionVisitorGeneric_Simple8()
        {
            var v = new MyVisitor();

            var o = (Expression<Func<double, double>>)(x => Math.Sin(x));
            var r = v.Visit(o);
            var n = (Expression<Func<double, double>>)(x => Math.Cos(x));

            var e = new ExpressionEqualityComparer();
            Assert.IsTrue(e.Equals(r, n));
        }

        [TestMethod]
        public void ExpressionVisitorGeneric_Simple9()
        {
            var v = new MyVisitor();

            var r = v.Visit(Expression.NewArrayInit(typeof(int), Expression.Constant(2), Expression.Constant(3)));

            var e = new ExpressionEqualityComparer();
            Assert.IsTrue(e.Equals(r, Expression.NewArrayInit(typeof(int), Expression.Constant(6), Expression.Constant(4))));
        }

        [TestMethod]
        public void ExpressionVisitorGeneric_Simple10()
        {
            var v = new MyVisitor();

            var o = (Expression<Func<Bar>>)(() => new Bar(2, "bar") { Foo = 1, Xs = { 3, 4 }, Qux = { Baz = 2 } });
            var r = v.Visit(o.Body);
            var n = (Expression<Func<Foo>>)(() => new Foo("BAR", 4) { Baz = { Qux = 4 }, Ys = { 8, 6 }, Bar = 2 });

            var e = new ExpressionEqualityComparer();
            Assert.IsTrue(e.Equals(r, n.Body));
        }

        [TestMethod]
        public void ExpressionVisitorGeneric_Simple11()
        {
            var v = new MyVisitor();

            var r = v.Visit(
                Expression.Switch(
                    Expression.Constant(1),
                    Expression.Constant(3),
                    Expression.SwitchCase(
                        Expression.Constant(2),
                        Expression.Constant(3), Expression.Constant(4)
                    ),
                    Expression.SwitchCase(
                        Expression.Constant(4),
                        Expression.Constant(1)
                    )
                )
            );

            var e = new ExpressionEqualityComparer();
            Assert.IsTrue(e.Equals(r,
                Expression.Switch(
                    Expression.Constant(2),
                    Expression.Constant(6),
                    Expression.SwitchCase(
                        Expression.Constant(8),
                        Expression.Constant(2)
                    ),
                    Expression.SwitchCase(
                        Expression.Constant(4),
                        Expression.Constant(8), Expression.Constant(6)
                    )
                )
            ));
        }

        [TestMethod]
        public void ExpressionVisitorGeneric_Simple12()
        {
            var v = new MyVisitor();

            var o = Expression.DebugInfo(Expression.SymbolDocument("foo.txt"), 1, 2, 3, 4);
            var r = (DebugInfoExpression)v.Visit(o);
            var n = Expression.DebugInfo(Expression.SymbolDocument("bar.txt"), 2, 4, 6, 8);

            // TODO
            //var e = new ExpressionEqualityComparer();
            //Assert.IsTrue(e.Equals(r, n));

            Assert.AreEqual(n.StartLine, r.StartLine);
            Assert.AreEqual(n.EndLine, r.EndLine);
            Assert.AreEqual(n.StartColumn, r.StartColumn);
            Assert.AreEqual(n.EndColumn, r.EndColumn);
            Assert.AreEqual(n.Document.FileName, r.Document.FileName);
        }

        [TestMethod]
        public void ExpressionVisitorGeneric_Simple13()
        {
            var v = new MyVisitor();

            var add = Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(
                Microsoft.CSharp.RuntimeBinder.CSharpBinderFlags.None,
                ExpressionType.Add,
                typeof(ExpressionEqualityComparerTests),
                new[]
                {
                    Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfo.Create(Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfoFlags.None, name: null),
                    Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfo.Create(Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfoFlags.None, name: null)
                }
            );

            var o = Expression.Dynamic(
                add,
                typeof(object),
                Expression.Constant(1),
                Expression.Constant(2)
            );

            var r = v.Visit(o);

            var n = Expression.Dynamic(
                add,
                typeof(object),
                Expression.Constant(2),
                Expression.Constant(4)
            );

            var e = new ExpressionEqualityComparer();
            Assert.IsTrue(e.Equals(r, n));
        }

        [TestMethod]
        public void ExpressionVisitorGeneric_Simple14()
        {
            var v = new MyVisitor();

            var l = Expression.Label("ret");

            var o = Expression.Return(l, Expression.Constant(2));
            var r = v.Visit(o);
            var n = Expression.Return(l, Expression.Constant(4));

            var e = new ExpressionEqualityComparer();
            Assert.IsTrue(e.Equals(r, n));
        }

        [TestMethod]
        public void ExpressionVisitorGeneric_Simple15()
        {
            var v = new MyVisitor();

            var l = Expression.Label("lbl_o");
            var m = Expression.Label("lbl_n");

            var o = Expression.Block(Expression.Constant(2), Expression.Goto(l), Expression.Constant(3), Expression.Label(l), Expression.Constant(4));
            var r = v.Visit(o);
            var n = Expression.Block(Expression.Constant(8), Expression.Label(m), Expression.Constant(6), Expression.Goto(m), Expression.Constant(4));

            var e = new ExpressionEqualityComparer();
            Assert.IsTrue(e.Equals(r, n));
        }

        [TestMethod]
        public void ExpressionVisitorGeneric_Simple16()
        {
            var v = new MyVisitor();

            var p = Expression.Parameter(typeof(Func<int, int, int>));

            var o = Expression.Invoke(p, Expression.Constant(1), Expression.Constant(2));
            var r = v.Visit(o);
            var n = Expression.Invoke(p, Expression.Constant(4), Expression.Constant(2));

            var e = new ExpressionEqualityComparer();
            Assert.IsTrue(e.Equals(r, n));
        }

        [TestMethod]
        public void ExpressionVisitorGeneric_Simple17()
        {
            var v = new MyVisitor();

            var o = (Expression<Func<Bar, int>>)(b => b.Foo);
            var r = v.Visit(o);
            var n = (Expression<Func<Bar, int>>)(f => f.ooF);

            var e = new ExpressionEqualityComparer();
            Assert.IsTrue(e.Equals(r, n));
        }

        [TestMethod]
        public void ExpressionVisitorGeneric_Simple18()
        {
            var v = new MyVisitor();

            var o = (Expression<Func<int[], int>>)(xs => xs[1]);
            var r = v.Visit(o);
            var n = (Expression<Func<int[], int>>)(xs => xs[2]);

            var e = new ExpressionEqualityComparer();
            Assert.IsTrue(e.Equals(r, n));
        }

        [TestMethod]
        public void ExpressionVisitorGeneric_Simple19()
        {
            var v = new MyVisitor();

            var o = (Expression<Func<int[,], int>>)(xs => xs[1, 2]);
            var r = v.Visit(o);
            var n = (Expression<Func<int[,], int>>)(xs => xs[4, 2]);

            var e = new ExpressionEqualityComparer();
            Assert.IsTrue(e.Equals(r, n));
        }

        [TestMethod]
        public void ExpressionVisitorGeneric_Simple20()
        {
            var v = new MyVisitor();

            var p = Expression.Parameter(typeof(int[,]));

            var o = Expression.ArrayIndex(p, Expression.Constant(1), Expression.Constant(2));
            var r = v.Visit(o);
            var n = Expression.ArrayIndex(p, Expression.Constant(4), Expression.Constant(2));

            var e = new ExpressionEqualityComparer();
            Assert.IsTrue(e.Equals(r, n));
        }

        [TestMethod]
        public void ExpressionVisitorGeneric_Simple21()
        {
            var v = new MyVisitor();

            var p = Expression.Parameter(typeof(int[,]));

            var o = Expression.ArrayAccess(p, Expression.Constant(1), Expression.Constant(2));
            var r = v.Visit(o);
            var n = Expression.ArrayAccess(p, Expression.Constant(4), Expression.Constant(2));

            var e = new ExpressionEqualityComparer();
            Assert.IsTrue(e.Equals(r, n));
        }

        [TestMethod]
        public void ExpressionVisitorGeneric_Simple22()
        {
            var v = new MyVisitor();

            var p = Expression.Parameter(typeof(int));
            var q = Expression.Parameter(typeof(int));

            var o = Expression.RuntimeVariables(p, q);
            var r = v.Visit(o);
            var n = Expression.RuntimeVariables(q, p);

            var e = new ExpressionEqualityComparer();
            Assert.IsTrue(e.Equals(r, n));
        }

        [TestMethod]
        public void ExpressionVisitorGeneric_Simple23()
        {
            var v = new MyVisitor();

            var c = Expression.Label("cont_o");
            var d = Expression.Label("cont_n");
            var e = Expression.Label("break_o");
            var f = Expression.Label("break_n");

            var o = Expression.Loop(Expression.Block(Expression.Constant(1), Expression.Goto(c), Expression.Constant(2), Expression.Goto(e)), e, c);
            var r = v.Visit(o);
            var n = Expression.Loop(Expression.Block(Expression.Goto(f), Expression.Constant(4), Expression.Goto(d), Expression.Constant(2)), f, d);

            var q = new ExpressionEqualityComparer();
            Assert.IsTrue(q.Equals(r, n));
        }

        [TestMethod]
        public void ExpressionVisitorGeneric_Simple24()
        {
            var v = new MyVisitor();

            var f = (Expression<Action>)(() => Console.WriteLine("finally"));
            var g = (Expression<Action>)(() => Console.WriteLine("FINALLY"));

            var p = Expression.Parameter(typeof(Exception));
            var q = Expression.Parameter(typeof(Exception));

            var o = Expression.MakeTry(typeof(int), Expression.Constant(1), f.Body, fault: null, new[] { Expression.Catch(p, Expression.Constant(2)), Expression.Catch(p, Expression.Constant(3)) });
            var r = v.Visit(o);
            var n = Expression.MakeTry(typeof(int), Expression.Constant(2), g.Body, fault: null, new[] { Expression.Catch(q, Expression.Constant(6)), Expression.Catch(q, Expression.Constant(4)) });

            var e = new ExpressionEqualityComparer();
            Assert.IsTrue(e.Equals(r, n));
        }

        private class MyVisitor : ExpressionVisitor<Expression, LambdaExpression, ParameterExpression, NewExpression, ElementInit, MemberBinding, MemberAssignment, MemberListBinding, MemberMemberBinding, CatchBlock, SwitchCase, LabelTarget>
        {
            public void TestArgumentChecks()
            {
                AssertEx.ThrowsException<ArgumentNullException>(() => base.VisitBinary(node: null), ex => Assert.AreEqual("node", ex.ParamName));
                AssertEx.ThrowsException<ArgumentNullException>(() => base.VisitBlock(node: null), ex => Assert.AreEqual("node", ex.ParamName));
                AssertEx.ThrowsException<ArgumentNullException>(() => base.VisitCatchBlock(node: null), ex => Assert.AreEqual("node", ex.ParamName));
                AssertEx.ThrowsException<ArgumentNullException>(() => base.VisitConditional(node: null), ex => Assert.AreEqual("node", ex.ParamName));
                AssertEx.ThrowsException<ArgumentNullException>(() => base.VisitConstant(node: null), ex => Assert.AreEqual("node", ex.ParamName));
                AssertEx.ThrowsException<ArgumentNullException>(() => base.VisitDebugInfo(node: null), ex => Assert.AreEqual("node", ex.ParamName));
                AssertEx.ThrowsException<ArgumentNullException>(() => base.VisitDefault(node: null), ex => Assert.AreEqual("node", ex.ParamName));
                AssertEx.ThrowsException<ArgumentNullException>(() => base.VisitDynamic(node: null), ex => Assert.AreEqual("node", ex.ParamName));
                AssertEx.ThrowsException<ArgumentNullException>(() => base.VisitElementInit(node: null), ex => Assert.AreEqual("node", ex.ParamName));
                AssertEx.ThrowsException<ArgumentNullException>(() => base.VisitGoto(node: null), ex => Assert.AreEqual("node", ex.ParamName));
                AssertEx.ThrowsException<ArgumentNullException>(() => base.VisitIndex(node: null), ex => Assert.AreEqual("node", ex.ParamName));
                AssertEx.ThrowsException<ArgumentNullException>(() => base.VisitInvocation(node: null), ex => Assert.AreEqual("node", ex.ParamName));
                AssertEx.ThrowsException<ArgumentNullException>(() => base.VisitLabel(node: null), ex => Assert.AreEqual("node", ex.ParamName));
                AssertEx.ThrowsException<ArgumentNullException>(() => base.VisitLabelTarget(node: null), ex => Assert.AreEqual("node", ex.ParamName));
                AssertEx.ThrowsException<ArgumentNullException>(() => base.VisitLambda<Action>(node: null), ex => Assert.AreEqual("node", ex.ParamName));
                AssertEx.ThrowsException<ArgumentNullException>(() => base.VisitListInit(node: null), ex => Assert.AreEqual("node", ex.ParamName));
                AssertEx.ThrowsException<ArgumentNullException>(() => base.VisitLoop(node: null), ex => Assert.AreEqual("node", ex.ParamName));
                AssertEx.ThrowsException<ArgumentNullException>(() => base.VisitMember(node: null), ex => Assert.AreEqual("node", ex.ParamName));
                AssertEx.ThrowsException<ArgumentNullException>(() => base.VisitMemberAssignment(node: null), ex => Assert.AreEqual("node", ex.ParamName));
                AssertEx.ThrowsException<ArgumentNullException>(() => base.VisitMemberBinding(node: null), ex => Assert.AreEqual("node", ex.ParamName));
                AssertEx.ThrowsException<ArgumentNullException>(() => base.VisitMemberInit(node: null), ex => Assert.AreEqual("node", ex.ParamName));
                AssertEx.ThrowsException<ArgumentNullException>(() => base.VisitMemberListBinding(node: null), ex => Assert.AreEqual("node", ex.ParamName));
                AssertEx.ThrowsException<ArgumentNullException>(() => base.VisitMemberMemberBinding(node: null), ex => Assert.AreEqual("node", ex.ParamName));
                AssertEx.ThrowsException<ArgumentNullException>(() => base.VisitMethodCall(node: null), ex => Assert.AreEqual("node", ex.ParamName));
                AssertEx.ThrowsException<ArgumentNullException>(() => base.VisitNew(node: null), ex => Assert.AreEqual("node", ex.ParamName));
                AssertEx.ThrowsException<ArgumentNullException>(() => base.VisitNewArray(node: null), ex => Assert.AreEqual("node", ex.ParamName));
                AssertEx.ThrowsException<ArgumentNullException>(() => base.VisitParameter(node: null), ex => Assert.AreEqual("node", ex.ParamName));
                AssertEx.ThrowsException<ArgumentNullException>(() => base.VisitRuntimeVariables(node: null), ex => Assert.AreEqual("node", ex.ParamName));
                AssertEx.ThrowsException<ArgumentNullException>(() => base.VisitSwitch(node: null), ex => Assert.AreEqual("node", ex.ParamName));
                AssertEx.ThrowsException<ArgumentNullException>(() => base.VisitSwitchCase(node: null), ex => Assert.AreEqual("node", ex.ParamName));
                AssertEx.ThrowsException<ArgumentNullException>(() => base.VisitTry(node: null), ex => Assert.AreEqual("node", ex.ParamName));
                AssertEx.ThrowsException<ArgumentNullException>(() => base.VisitTypeBinary(node: null), ex => Assert.AreEqual("node", ex.ParamName));
                AssertEx.ThrowsException<ArgumentNullException>(() => base.VisitUnary(node: null), ex => Assert.AreEqual("node", ex.ParamName));
            }

            protected override Expression MakeBinary(BinaryExpression node, Expression left, LambdaExpression conversion, Expression right)
            {
                if (node.NodeType == ExpressionType.Add)
                {
                    return Expression.MakeBinary(ExpressionType.Subtract, left, right);
                }

                if (node.NodeType == ExpressionType.ArrayIndex)
                {
                    return Expression.ArrayIndex(left, right);
                }

                throw new NotImplementedException();
            }

            protected override Expression MakeBlock(BlockExpression node, ReadOnlyCollection<ParameterExpression> variables, ReadOnlyCollection<Expression> expressions)
            {
                return Expression.Block(variables, expressions.Reverse());
            }

            protected override CatchBlock MakeCatchBlock(CatchBlock node, ParameterExpression variable, Expression body, Expression filter)
            {
                return Expression.MakeCatchBlock(node.Test, variable, body, filter);
            }

            protected override Expression MakeConditional(ConditionalExpression node, Expression test, Expression ifTrue, Expression ifFalse)
            {
                return Expression.Condition(test, ifFalse, ifTrue);
            }

            protected override Expression MakeConstant(ConstantExpression node)
            {
                if (node.Type == typeof(int))
                {
                    return Expression.Constant((int)node.Value * 2);
                }

                if (node.Type == typeof(bool))
                {
                    return Expression.Constant(!(bool)node.Value);
                }

                if (node.Type == typeof(string))
                {
                    return Expression.Constant(((string)node.Value).ToUpper());
                }

                throw new NotImplementedException();
            }

            protected override Expression MakeDebugInfo(DebugInfoExpression node)
            {
                if (node.Document.FileName == "foo.txt")
                {
                    return Expression.DebugInfo(Expression.SymbolDocument("bar.txt"), node.StartLine * 2, node.StartColumn * 2, node.EndLine * 2, node.EndColumn * 2);
                }

                throw new NotImplementedException();
            }

            protected override Expression MakeDefault(DefaultExpression node)
            {
                if (node.Type == typeof(int))
                {
                    return Expression.Default(typeof(string));
                }

                throw new NotImplementedException();
            }

            protected override Expression MakeDynamic(DynamicExpression node, ReadOnlyCollection<Expression> arguments)
            {
                return Expression.Dynamic(node.Binder, node.Type, arguments);
            }

            protected override ElementInit MakeElementInit(ElementInit node, ReadOnlyCollection<Expression> arguments)
            {
                if (node.AddMethod.DeclaringType == typeof(List<int>))
                {
                    return Expression.ElementInit(typeof(HashSet<int>).GetMethod("Add", new[] { typeof(int) }), arguments);
                }

                throw new NotImplementedException();
            }

            protected override Expression MakeGoto(GotoExpression node, LabelTarget target, Expression value)
            {
                return Expression.MakeGoto(node.Kind, target, value, node.Type);
            }

            protected override Expression MakeIndex(IndexExpression node, Expression @object, ReadOnlyCollection<Expression> arguments)
            {
                return Expression.MakeIndex(@object, node.Indexer, arguments.Reverse());
            }

            protected override Expression MakeInvocation(InvocationExpression node, Expression expression, ReadOnlyCollection<Expression> arguments)
            {
                return Expression.Invoke(expression, arguments.Reverse());
            }

            protected override Expression MakeLabel(LabelExpression node, LabelTarget target, Expression defaultValue)
            {
                return Expression.Label(target, defaultValue);
            }

            protected override LabelTarget MakeLabelTarget(LabelTarget node) => node;

            protected override LambdaExpression MakeLambda<T>(Expression<T> node, Expression body, ReadOnlyCollection<ParameterExpression> parameters)
            {
                return Expression.Lambda(body, parameters);
            }

            protected override Expression MakeListInit(ListInitExpression node, NewExpression newExpression, ReadOnlyCollection<ElementInit> initializers)
            {
                return Expression.ListInit(newExpression, initializers.Reverse().ToArray());
            }

            protected override Expression MakeLoop(LoopExpression node, Expression body, LabelTarget breakLabel, LabelTarget continueLabel)
            {
                return Expression.Loop(body, breakLabel, continueLabel);
            }

            protected override Expression MakeMember(MemberExpression node, Expression expression)
            {
                if (node.Member.DeclaringType == typeof(Bar) && node.Member.Name == "Foo")
                {
                    return Expression.MakeMemberAccess(expression, typeof(Bar).GetProperty("ooF"));
                }

                throw new NotImplementedException();
            }

            protected override MemberAssignment MakeMemberAssignment(MemberAssignment node, Expression expression)
            {
                if (node.Member.DeclaringType == typeof(Bar) && node.Member.Name == "Foo")
                {
                    return Expression.Bind(typeof(Foo).GetProperty("Bar"), expression);
                }

                if (node.Member.DeclaringType == typeof(Qux) && node.Member.Name == "Baz")
                {
                    return Expression.Bind(typeof(Baz).GetProperty("Qux"), expression);
                }

                throw new NotImplementedException();
            }

            protected override Expression MakeMemberInit(MemberInitExpression node, NewExpression newExpression, ReadOnlyCollection<MemberBinding> bindings)
            {
                return Expression.MemberInit(newExpression, bindings.Reverse());
            }

            protected override MemberListBinding MakeMemberListBinding(MemberListBinding node, ReadOnlyCollection<ElementInit> initializers)
            {
                if (node.Member.DeclaringType == typeof(Bar) && node.Member.Name == "Xs")
                {
                    return Expression.ListBind(typeof(Foo).GetProperty("Ys"), initializers.Reverse());
                }

                throw new NotImplementedException();
            }

            protected override MemberMemberBinding MakeMemberMemberBinding(MemberMemberBinding node, ReadOnlyCollection<MemberBinding> bindings)
            {
                if (node.Member.DeclaringType == typeof(Bar) && node.Member.Name == "Qux")
                {
                    return Expression.MemberBind(typeof(Foo).GetProperty("Baz"), bindings.Reverse());
                }

                throw new NotImplementedException();
            }

            protected override Expression MakeMethodCall(MethodCallExpression node, Expression @object, ReadOnlyCollection<Expression> arguments)
            {
                if (node.Method.DeclaringType == typeof(Math) && node.Method.Name == "Sin")
                {
                    return Expression.Call(typeof(Math).GetMethod("Cos", new[] { typeof(double) }), arguments);
                }

                if (@object != null && typeof(Array).IsAssignableFrom(@object.Type) && node.Method.Name == "Get")
                {
                    return Expression.Call(@object, node.Method, arguments.Reverse());
                }

                if (node.Method.DeclaringType == typeof(Console) && node.Method.Name == "WriteLine")
                {
                    return Expression.Call(node.Method, arguments);
                }

                throw new NotImplementedException();
            }

            protected override Expression MakeNew(NewExpression node, ReadOnlyCollection<Expression> arguments)
            {
                if (node.Type == typeof(List<int>))
                {
                    return Expression.New(typeof(HashSet<int>).GetConstructor(Type.EmptyTypes), arguments);
                }

                if (node.Type == typeof(Bar))
                {
                    return Expression.New(typeof(Foo).GetConstructor(new[] { typeof(string), typeof(int) }), arguments.Reverse());
                }

                if (node.Type == typeof(Qux))
                {
                    return Expression.New(typeof(Baz).GetConstructor(Type.EmptyTypes), arguments);
                }

                throw new NotImplementedException();
            }

            protected override Expression MakeNewArray(NewArrayExpression node, ReadOnlyCollection<Expression> expressions)
            {
                if (node.NodeType == ExpressionType.NewArrayInit)
                {
                    return Expression.NewArrayInit(node.Type.GetElementType(), expressions.Reverse());
                }

                throw new NotImplementedException();
            }

            protected override ParameterExpression MakeParameter(ParameterExpression node) => node;

            protected override Expression MakeRuntimeVariables(RuntimeVariablesExpression node, ReadOnlyCollection<ParameterExpression> variables)
            {
                return Expression.RuntimeVariables(variables.Reverse());
            }

            protected override Expression MakeSwitch(SwitchExpression node, Expression switchValue, Expression defaultBody, ReadOnlyCollection<SwitchCase> cases)
            {
                return Expression.Switch(switchValue, defaultBody, cases.Reverse().ToArray());
            }

            protected override SwitchCase MakeSwitchCase(SwitchCase node, Expression body, ReadOnlyCollection<Expression> testValues)
            {
                return Expression.SwitchCase(body, testValues.Reverse());
            }

            protected override Expression MakeTry(TryExpression node, Expression body, Expression @finally, Expression fault, ReadOnlyCollection<CatchBlock> handlers)
            {
                return Expression.MakeTry(node.Type, body, @finally, fault, handlers.Reverse());
            }

            protected override Expression MakeTypeBinary(TypeBinaryExpression node, Expression expression)
            {
                if (node.NodeType == ExpressionType.TypeIs && node.TypeOperand == typeof(int))
                {
                    return Expression.TypeIs(expression, typeof(string));
                }

                throw new NotImplementedException();
            }

            protected override Expression MakeUnary(UnaryExpression node, Expression operand)
            {
                if (node.NodeType == ExpressionType.UnaryPlus)
                {
                    return Expression.Negate(operand);
                }

                throw new NotImplementedException();
            }
        }

#pragma warning disable IDE0060 // Remove unused parameter
        private sealed class Bar
        {
            public Bar(int x, string y)
            {
            }

            public int Foo { get; set; }
            public Qux Qux { get; set; }
            public List<int> Xs { get; set; }

            public int ooF { get; set; }
        }

        private sealed class Foo
        {
            public Foo(string y, int x)
            {
            }

            public int Bar { get; set; }
            public Baz Baz { get; set; }
            public HashSet<int> Ys { get; set; }
        }
#pragma warning restore IDE0060 // Remove unused parameter

        private sealed class Qux
        {
            public int Baz { get; set; }
        }

        private sealed class Baz
        {
            public int Qux { get; set; }
        }

        private sealed class MyVisitor2 : MyVisitor
        {
            protected override Expression VisitExtension(Expression node) => node;
        }
    }
}
