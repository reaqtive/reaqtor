// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq.CompilerServices;
using System.Linq.Expressions;

using Reaqtive;
using Reaqtive.Expressions;

using Reaqtor.Expressions;
using Reaqtor.Reactive.Expressions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Reaqtor.QueryEngine
{
    [TestClass]
    public class QuoterTests
    {
        [TestMethod]
        public void Quoter_Nop()
        {
            var quoter = new Quoter(DefaultExpressionPolicy.Instance);

            var expr = Expression.Constant(1);

            var res = quoter.Visit(expr);

            Assert.AreSame(expr, res);
        }

        [TestMethod]
        public void Quoter_NoEnviroment()
        {
            var quoter = new Quoter(DefaultExpressionPolicy.Instance);

            var expr = Expression.Constant(Subscribable.Return(42), typeof(ISubscribable<int>));

            var res = quoter.Visit(expr);

            var obj = res.Evaluate();

            var quote = ((IExpressible)obj).Expression;

            Assert.AreSame(expr, quote);
        }

        [TestMethod]
        public void Quoter_WithEnvironment()
        {
            var quoter = new Quoter(DefaultExpressionPolicy.Instance);

            var expr = (Expression<Func<int, ISubscribable<int>>>)(x => Subscribable.Return(x));

            var res = quoter.Visit(expr);

            var obj = (Func<int, ISubscribable<int>>)res.Evaluate();
            var value = obj(42);

            var quote = ((IExpressible)value).Expression;

            var eq = new ExpressionEqualityComparer();

            Assert.IsTrue(
                eq.Equals(
                    Expression.Invoke(
                        expr,
                        Expression.Constant(42)
                    ),
                    quote
                )
            );
        }

        [TestMethod]
        public void Quoter_Expressible1()
        {
            var quoter = new Quoter(DefaultExpressionPolicy.Instance);

            var expr = (Expression<Func<Quotable>>)(() => new Quotable());

            var res = quoter.Visit(expr);

            var obj = (Func<Quotable>)res.Evaluate();
            var value = obj();

            var quote = value.Expression;

            var eq = new ExpressionEqualityComparer();

            Assert.IsTrue(
                eq.Equals(
                    expr.Body,
                    quote
                )
            );
        }

        [TestMethod]
        public void Quoter_Expressible2()
        {
            var quoter = new Quoter(DefaultExpressionPolicy.Instance);

            var expr = (Expression<Func<Quotable>>)(() => new Quotable() { X = 1 });

            var res = quoter.Visit(expr);

            var obj = (Func<Quotable>)res.Evaluate();
            var value = obj();

            var quote = value.Expression;

            var eq = new ExpressionEqualityComparer();

            Assert.IsTrue(
                eq.Equals(
                    expr.Body,
                    quote
                )
            );
        }

        [TestMethod]
        public void Quoter_Expressible3()
        {
            var quoter = new Quoter(DefaultExpressionPolicy.Instance);

            var expr = (Expression<Func<Quotable>>)(() => Quotable.Create());

            var res = quoter.Visit(expr);

            var obj = (Func<Quotable>)res.Evaluate();
            var value = obj();

            var quote = value.Expression;

            var eq = new ExpressionEqualityComparer();

            Assert.IsTrue(
                eq.Equals(
                    expr.Body,
                    quote
                )
            );
        }

        [TestMethod]
        public void Quoter_Environment_Expressible1()
        {
            var quoter = new Quoter(DefaultExpressionPolicy.Instance);

            var expr = (Expression<Func<int, Quotable>>)(x => new Quotable(x));

            var res = quoter.Visit(expr);

            var obj = (Func<int, Quotable>)res.Evaluate();
            var value = obj(42);

            var quote = value.Expression;

            var eq = new ExpressionEqualityComparer();

            Assert.IsTrue(
                eq.Equals(
                    ((NewExpression)expr.Body).Update(new[] { Expression.Constant(42) }),
                    quote
                )
            );
        }

        [TestMethod]
        public void Quoter_Environment_Expressible2()
        {
            var quoter = new Quoter(DefaultExpressionPolicy.Instance);

            var expr = (Expression<Func<int, Quotable>>)(x => new Quotable() { X = x });

            var res = quoter.Visit(expr);

            var obj = (Func<int, Quotable>)res.Evaluate();
            var value = obj(42);

            var quote = value.Expression;

            var eq = new ExpressionEqualityComparer();

            var mi = (MemberInitExpression)expr.Body;
            var b = (MemberAssignment)mi.Bindings[0];

            Assert.IsTrue(
                eq.Equals(
                    mi.Update(
                        mi.NewExpression,
                        new[]
                        {
                            b.Update(Expression.Constant(42))
                        }
                    ),
                    quote
                )
            );
        }

        [TestMethod]
        public void Quoter_Environment_Expressible3()
        {
            var quoter = new Quoter(DefaultExpressionPolicy.Instance);

            var expr = (Expression<Func<int, Quotable>>)(x => Quotable.Create(x));

            var res = quoter.Visit(expr);

            var obj = (Func<int, Quotable>)res.Evaluate();
            var value = obj(42);

            var quote = value.Expression;

            var eq = new ExpressionEqualityComparer();

            Assert.IsTrue(
                eq.Equals(
                    ((MethodCallExpression)expr.Body).Update(null, new[] { Expression.Constant(42) }),
                    quote
                )
            );
        }

        [TestMethod]
        public void QuoteFactory_Simple()
        {
            const int N = 16; // NB: Ensure this value is higher than Creator<T, R>.CompilationInvocationThreshold.

            for (var i = 0; i < N; i++)
            {
                var foo = new Foo();
                var expr = Expression.Constant(foo);
                var args = new object[] { i };

                var q1 = (QuotedFoo)QuoteFactory.Create<IFoo, QuotedFoo>(foo, expr, args);

                Assert.AreSame(foo, q1.Value);
                Assert.AreSame(expr, q1.Expression);
                Assert.AreEqual(args[0], q1.Argument);
            }
        }

        private interface IFoo
        {
        }

        private sealed class Foo : IFoo
        {
        }

        private sealed class QuotedFoo : IQuoted<IFoo>, IFoo
        {
            public QuotedFoo(IFoo value, Expression expression, int argument)
            {
                Value = value;
                Expression = expression;
                Argument = argument;
            }

            public IFoo Value { get; }

            public Expression Expression { get; }

            public int Argument { get; }
        }

        private sealed class Quotable : IExpressible
        {
            public Quotable()
            {
            }

            public Quotable(int x)
            {
                X = x;
            }

            public Expression Expression { get; set; }

            public int X { get; set; }

            public static Quotable Create() => new();
            public static Quotable Create(int x) => new(x);
        }
    }
}
