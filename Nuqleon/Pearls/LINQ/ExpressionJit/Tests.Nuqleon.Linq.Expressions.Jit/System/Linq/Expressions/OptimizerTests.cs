// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq.CompilerServices;
using System.Linq.Expressions;

namespace Tests
{
    [TestClass]
    public class OptimizerTests
    {
        [TestMethod]
        public void BlockFlattening1()
        {
            var eq = new ExpressionEqualityComparer();

            for (var i = 1; i <= 3; i++)
            {
                var e = (Expression)Expression.Empty();

                for (var j = 1; j <= i; j++)
                {
                    e = Expression.Block(e);
                }

                foreach (var f in new[] { Optimizations.BlockFlattening, Optimizations.All })
                {
                    var o = Optimizer.Optimize(e, f);

                    Assert.IsTrue(eq.Equals(o, Expression.Block(Expression.Empty())));
                }
            }
        }

        [TestMethod]
        public void BlockFlattening2()
        {
            var eq = new ExpressionEqualityComparer();

            var x = Expression.Parameter(typeof(int));
            var y = Expression.Parameter(typeof(int));
            var z = Expression.Parameter(typeof(int));

            var e =
                Expression.Block(new[] { x },
                    Expression.Block(new[] { y },
                        Expression.Block(new[] { z },
                            Expression.Add(x, Expression.Multiply(y, z))
                        )
                    )
                );

            foreach (var f in new[] { Optimizations.BlockFlattening, Optimizations.All })
            {
                var o = Optimizer.Optimize(e, f);

                Assert.IsTrue(
                    eq.Equals(o,
                        Expression.Block(new[] { x, y, z },
                            Expression.Add(x, Expression.Multiply(y, z))
                        )
                    )
                );
            }
        }

        [TestMethod]
        public void BlockFlattening3()
        {
            var eq = new ExpressionEqualityComparer();

            var x = Expression.Parameter(typeof(int));
            var y = Expression.Parameter(typeof(int));
            var z = Expression.Parameter(typeof(int));

            var e =
                Expression.Block(new[] { x },
                    Expression.Block(new[] { y },
                        Expression.Block(new[] { z },
                            Expression.Add(x, Expression.Multiply(y, z))
                        ),
                        Expression.Empty()
                    )
                );

            foreach (var f in new[] { Optimizations.BlockFlattening, Optimizations.All })
            {
                var o = Optimizer.Optimize(e, f);

                Assert.IsTrue(
                    eq.Equals(o,
                        Expression.Block(new[] { x, y },
                            Expression.Block(new[] { z },
                                Expression.Add(x, Expression.Multiply(y, z))
                            ),
                            Expression.Empty() // NB: Flattening would be valid here, but we don't analyze this case.
                        )
                    )
                );
            }
        }

        [TestMethod]
        public void InvocationInlining1()
        {
            var eq = new ExpressionEqualityComparer();

            var x = Expression.Parameter(typeof(int));

            var e =
                Expression.Invoke(
                    Expression.Lambda(x, x),
                    Expression.Constant(1)
                );

            foreach (var f in new[] { Optimizations.InvocationInlining, Optimizations.All })
            {
                var o = Optimizer.Optimize(e, f);

                Assert.IsTrue(
                    eq.Equals(o,
                        Expression.Block(new[] { x },
                            Expression.Assign(x, Expression.Constant(1)),
                            x
                        )
                    )
                );
            }
        }

        [TestMethod]
        public void InvocationInlining2()
        {
            var eq = new ExpressionEqualityComparer();

            var x = Expression.Parameter(typeof(int));
            var y = Expression.Parameter(typeof(int));

            var e =
                Expression.Invoke(
                    Expression.Lambda(Expression.Add(x, y), x, y),
                    Expression.Constant(1),
                    Expression.Constant(2)
                );

            foreach (var f in new[] { Optimizations.InvocationInlining, Optimizations.All })
            {
                var o = Optimizer.Optimize(e, f);

                Assert.IsTrue(
                    eq.Equals(o,
                        Expression.Block(new[] { x, y },
                            Expression.Assign(x, Expression.Constant(1)),
                            Expression.Assign(y, Expression.Constant(2)),
                            Expression.Add(x, y)
                        )
                    )
                );
            }
        }

        [TestMethod]
        public void InvocationInlining3()
        {
            var eq = new ExpressionEqualityComparer();

            var a = Expression.Parameter(typeof(int));
            var b = Expression.Parameter(typeof(int));
            var c = Expression.Parameter(typeof(int));
            var x = Expression.Parameter(typeof(int));

            var e =
                Expression.Block(
                    new[] { a, b, c, x },
                    Expression.Invoke(
                        Expression.Lambda(Expression.Add(a, Expression.Multiply(b, c)), a, b, c),
                        Expression.Add(Expression.Constant(1), a),
                        Expression.Multiply(Expression.Constant(2), b),
                        Expression.Subtract(x, Expression.Constant(3))
                    )
                );

            foreach (var f in new[] { Optimizations.InvocationInlining, Optimizations.All })
            {
                var o = Optimizer.Optimize(e, f);

                var t1 = Expression.Parameter(typeof(int));
                var t2 = Expression.Parameter(typeof(int));

                Assert.IsTrue(
                    eq.Equals(o,
                        Expression.Block(
                            new[] { a, b, c, x },
                            Expression.Block(
                                new[] { t1, t2, c },
                                Expression.Assign(t1, Expression.Add(Expression.Constant(1), a)),
                                Expression.Assign(t2, Expression.Multiply(Expression.Constant(2), b)),
                                Expression.Assign(c, Expression.Subtract(x, Expression.Constant(3))),
                                Expression.Block(
                                    new[] { a, b },
                                    Expression.Assign(a, t1),
                                    Expression.Assign(b, t2),
                                    Expression.Add(a, Expression.Multiply(b, c))
                                )
                            )
                        )
                    )
                );
            }
        }

        [TestMethod]
        public void InvocationInlining_Nop()
        {
            var f = Expression.Parameter(typeof(Action));
            var e = Expression.Invoke(f);
            var o = Optimizer.Optimize(e, Optimizations.InvocationInlining);
            Assert.AreSame(e, o);
        }

        [TestMethod]
        public void InvocationInlining_HasByRef1()
        {
            var f = Expression.Parameter(typeof(ByRef));
            var x = Expression.Parameter(typeof(int));
            var e = Expression.Invoke(f, x);
            var o = Optimizer.Optimize(e, Optimizations.InvocationInlining);
            Assert.AreSame(e, o);
        }

        [TestMethod]
        public void InvocationInlining_HasByRef2()
        {
            var f = Expression.Lambda<ByRef>(Expression.Empty(), Expression.Parameter(typeof(int).MakeByRefType()));
            var x = Expression.Parameter(typeof(int));
            var e = Expression.Invoke(f, x);
            var o = Optimizer.Optimize(e, Optimizations.InvocationInlining);
            Assert.AreSame(e, o);
        }

        private delegate void ByRef(ref int i);
    }
}
