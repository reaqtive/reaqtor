// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq.Expressions;
using System.Linq.Expressions.Jit;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using static System.Linq.Expressions.Expression;

namespace Tests
{
    [TestClass]
    public class ReducerTests
    {
        [TestMethod]
        public void Reducer_CompoundAssignment()
        {
            var x = Parameter(typeof(DateTimeOffset));
            var y = Parameter(typeof(DateTimeOffset));

            var d = new DateTimeOffset(1983, 2, 11, 12, 0, 0, TimeSpan.Zero);
            var t = TimeSpan.FromDays(365);
            var e = TimeSpan.FromDays(1);

            var o = typeof(DateTimeOffset).GetMethod("op_Addition", new[] { typeof(DateTimeOffset), typeof(TimeSpan) });

            var f =
                Lambda<Func<DateTimeOffset>>(
                    Block(
                        new[] { x },
                        Assign(
                            x,
                            Constant(d)
                        ),
                        AddAssign(
                            x,
                            Constant(t),
                            o,
                            Lambda(
                                Add(
                                    y,
                                    Constant(e)
                                ),
                                y
                            )
                        ),
                        x
                    )
                );

            AssertReduceAndEval(f, d + t + e);
        }

        [TestMethod]
        public void Reducer_CoalesceNullable1()
        {
            var x = Parameter(typeof(int?));
            var y = Parameter(typeof(int));

            var f =
                Lambda<Func<long>>(
                    Block(
                        new[] { x },
                        Assign(
                            x,
                            Default(typeof(int?))
                        ),
                        Coalesce(
                            x,
                            Constant(42L, typeof(long)),
                            Lambda(
                                Convert(y, typeof(long)),
                                y
                            )
                        )
                    )
                );

            AssertReduceAndEval(f, 42L);
        }

        [TestMethod]
        public void Reducer_CoalesceNullable2()
        {
            var x = Parameter(typeof(int?));
            var y = Parameter(typeof(int));

            var f =
                Lambda<Func<long>>(
                    Block(
                        new[] { x },
                        Assign(
                            x,
                            Constant(41, typeof(int?))
                        ),
                        Coalesce(
                            x,
                            Constant(42L, typeof(long)),
                            Lambda(
                                Convert(y, typeof(long)),
                                y
                            )
                        )
                    )
                );

            AssertReduceAndEval(f, 41L);
        }

        [TestMethod]
        public void Reducer_CoalesceNullable3()
        {
            var x = Parameter(typeof(int?));
            var y = Parameter(typeof(int?));

            var f =
                Lambda<Func<long?>>(
                    Block(
                        new[] { x },
                        Assign(
                            x,
                            Default(typeof(int?))
                        ),
                        Coalesce(
                            x,
                            Constant(42L, typeof(long?)),
                            Lambda(
                                Convert(y, typeof(long?)),
                                y
                            )
                        )
                    )
                );

            AssertReduceAndEval(f, 42L);
        }

        [TestMethod]
        public void Reducer_CoalesceNullable4()
        {
            var x = Parameter(typeof(int?));
            var y = Parameter(typeof(int?));

            var f =
                Lambda<Func<long?>>(
                    Block(
                        new[] { x },
                        Assign(
                            x,
                            Constant(41, typeof(int?))
                        ),
                        Coalesce(
                            x,
                            Constant(42L, typeof(long?)),
                            Lambda(
                                Convert(y, typeof(long?)),
                                y
                            )
                        )
                    )
                );

            AssertReduceAndEval(f, 41L);
        }

        [TestMethod]
        public void Reducer_CoalesceReference1()
        {
            var x = Parameter(typeof(string));
            var y = Parameter(typeof(string));

            var m = typeof(string).GetMethod(nameof(string.ToUpper), Type.EmptyTypes);

            var f =
                Lambda<Func<string>>(
                    Block(
                        new[] { x },
                        Assign(
                            x,
                            Default(typeof(string))
                        ),
                        Coalesce(
                            x,
                            Constant("foo", typeof(string)),
                            Lambda(
                                Call(y, m),
                                y
                            )
                        )
                    )
                );

            AssertReduceAndEval(f, "foo");
        }

        [TestMethod]
        public void Reducer_CoalesceReference2()
        {
            var x = Parameter(typeof(string));
            var y = Parameter(typeof(string));

            var m = typeof(string).GetMethod(nameof(string.ToUpper), Type.EmptyTypes);

            var f =
                Lambda<Func<string>>(
                    Block(
                        new[] { x },
                        Assign(
                            x,
                            Constant("bar", typeof(string))
                        ),
                        Coalesce(
                            x,
                            Constant("foo", typeof(string)),
                            Lambda(
                                Call(y, m),
                                y
                            )
                        )
                    )
                );

            AssertReduceAndEval(f, "BAR");
        }

        private static void AssertReduceAndEval<T>(Expression<Func<T>> f, T expected)
        {
            Assert.AreEqual(expected, f.Compile()());

            var r = Reducer.Reduce(f);

            AssertVisitor.Instance.Visit(r);

            Assert.AreEqual(expected, r.Compile()());
        }

        private sealed class AssertVisitor : ExpressionVisitor
        {
            public static readonly AssertVisitor Instance = new();

            private AssertVisitor() { }

            protected override Expression VisitBinary(BinaryExpression node)
            {
                Assert.IsNull(node.Conversion);
                return base.VisitBinary(node);
            }

            protected override Expression VisitExtension(Expression node)
            {
                Assert.Fail();
                return base.VisitExtension(node);
            }
        }
    }
}
