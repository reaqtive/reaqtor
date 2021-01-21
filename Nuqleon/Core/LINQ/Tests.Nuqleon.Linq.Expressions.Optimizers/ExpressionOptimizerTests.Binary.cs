// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2017 - Created this file.
//

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq.Expressions;
using System.Numerics;
using System.Reflection;

namespace Tests.System.Linq.Expressions.Optimizers
{
    public partial class ExpressionOptimizerTests
    {
        [TestMethod]
        public void Binary_ArrayIndex_ThrowLeft()
        {
            var ex = Expression.Parameter(typeof(Exception));
            var t = Expression.Throw(ex, typeof(int[]));

            var e =
                Expression.ArrayIndex(
                    t,
                    Expression.Constant(1)
                );

            var r =
                Expression.Throw(ex, typeof(int));

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Binary_ArrayIndex_ThrowRight()
        {
            var ex = Expression.Parameter(typeof(Exception));
            var t = Expression.Throw(ex, typeof(int));

            var e =
                Expression.ArrayIndex(
                    Expression.Constant(new[] { 42 }),
                    t
                );

            var r =
                Expression.Throw(ex, typeof(int));

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Binary_ArrayIndex_NullReference()
        {
            var e =
                Expression.ArrayIndex(
                    Expression.Constant(value: null, typeof(int[])),
                    Expression.Constant(1)
                );

            AssertThrows(e, typeof(NullReferenceException));
        }

        [TestMethod]
        public void Binary_ArrayIndex_Eval1()
        {
            var e =
                Expression.ArrayIndex(
                    Expression.Constant(new[] { 2, 3, 5 }),
                    Expression.Constant(1)
                );

            var r =
                Expression.Constant(3);

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Binary_ArrayIndex_Eval2()
        {
            var e =
                Expression.ArrayIndex(
                    Expression.Constant(new[] { 2, 3, 5 }),
                    Expression.Constant(3)
                );

            AssertThrows(e, typeof(IndexOutOfRangeException));
        }

        [TestMethod]
        public void Binary_ArrayIndex_Nop1()
        {
            var i = Expression.Parameter(typeof(int));

            var e =
                Expression.ArrayIndex(
                    Expression.Constant(new[] { 2, 3, 5 }),
                    i
                );

            AssertOptimized(e, e);
        }

        [TestMethod]
        public void Binary_ArrayIndex_Nop2()
        {
            var xs = Expression.Parameter(typeof(int[]));

            var e =
                Expression.ArrayIndex(
                    xs,
                    Expression.Constant(1)
                );

            AssertOptimized(e, e);
        }

        [TestMethod]
        public void Binary_ArrayIndex_Nop3()
        {
            var xs = Expression.Parameter(typeof(int[]));
            var i = Expression.Parameter(typeof(int));

            var e =
                Expression.ArrayIndex(
                    xs,
                    i
                );

            AssertOptimized(e, e);
        }

        [TestMethod]
        public void Binary_Coalesce_ThrowLeft()
        {
            var ex = Expression.Parameter(typeof(Exception));
            var t = Expression.Throw(ex, typeof(string));

            var e =
                Expression.Coalesce(
                    t,
                    Expression.Constant("bar")
                );

            var r =
                Expression.Throw(ex, typeof(string));

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Binary_Coalesce_NeverNull()
        {
            var s = Expression.Parameter(typeof(string));

            var e =
                Expression.Coalesce(
                    Expression.Constant("bar"),
                    s
                );

            var r =
                Expression.Constant("bar");

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Binary_Coalesce_NeverNull_Conversion()
        {
            var c = Expression.Parameter(typeof(DateTime));
            var d = new DateTime(1983, 2, 11);
            var s = new DateTimeOffset(new DateTime(1958, 10, 28), TimeSpan.Zero);

            var e =
                Expression.Coalesce(
                    Expression.Constant(d, typeof(DateTime?)),
                    Expression.Constant(s),
                    Expression.Lambda(
                        Expression.Convert(c, typeof(DateTimeOffset)),
                        c
                    )
                );

            var r =
                Expression.Convert(
                    Expression.Constant(d),
                    typeof(DateTimeOffset)  // NB: By default, conversion of DateTimeOffset is not treated as pure.
                );

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Binary_Coalesce_NeverNull_Conversion_Pure()
        {
            var c = Expression.Parameter(typeof(DateTime));
            var d = new DateTime(1983, 2, 11);
            var s = new DateTimeOffset(new DateTime(1958, 10, 28), TimeSpan.Zero);

            var e =
                Expression.Coalesce(
                    Expression.Constant(d, typeof(DateTime?)),
                    Expression.Constant(s),
                    Expression.Lambda(
                        Expression.Convert(c, typeof(DateTimeOffset)),
                        c
                    )
                );

            var r =
                Expression.Constant((DateTimeOffset)d);

            AssertOptimized(GetPureBinaryOptimizer(), e, r);
        }

        [TestMethod]
        public void Binary_Coalesce_AlwaysNull()
        {
            var e =
                Expression.Coalesce(
                    Expression.Default(typeof(string)),
                    Expression.Constant("bar")
                );

            var r =
                Expression.Constant("bar");

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Binary_Coalesce_Nop1()
        {
            var s = Expression.Parameter(typeof(string));
            var t = Expression.Parameter(typeof(string));

            var e =
                Expression.Coalesce(
                    s,
                    t
                );

            AssertOptimized(e, e);
        }

        [TestMethod]
        public void Binary_Coalesce_Nop2()
        {
            var t = Expression.Parameter(typeof(string));

            var e =
                Expression.Coalesce(
                    S,
                    t
                );

            AssertOptimized(e, e);
        }

        [TestMethod]
        public void Binary_CompoundAssign_Nop()
        {
            var d = Expression.Parameter(typeof(DateTimeOffset));
            var c = Expression.Parameter(typeof(DateTimeOffset));

            var e =
                Expression.AddAssign(
                    d,
                    Expression.Constant(TimeSpan.Zero),
                    null,
                    Expression.Lambda(c, c)
                );

            AssertOptimized(e, e);
        }

        [TestMethod]
        public void Binary_AndAlso_Unlifted_FalseShortCircuit()
        {
            var e =
                Expression.AndAlso(
                    Expression.Constant(false),
                    B
                );

            var r =
                Expression.Constant(false);

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Binary_AndAlso_Unlifted_TrueShortCircuit()
        {
            var e =
                Expression.AndAlso(
                    Expression.Constant(true),
                    B
                );

            var r =
                B;

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Binary_AndAlso_Unlifted_Nop()
        {
            var e =
                Expression.AndAlso(
                    B1,
                    B2
                );

            AssertOptimized(e, e);
        }

        [TestMethod]
        public void Binary_AndAlso_Unlifted_AssertAllConstant()
        {
            foreach (var l in new[] { false, true })
            {
                foreach (var r in new[] { false, true })
                {
                    var e = Expression.AndAlso(Expression.Constant(l), Expression.Constant(r));
                    var o = GetOptimizer().Visit(e);

                    var evalE = Expression.Lambda<Func<bool>>(e).Compile();
                    var evalO = Expression.Lambda<Func<bool>>(o).Compile();

                    Assert.AreEqual(evalE(), evalO(), $"{l} && {r}");
                }
            }
        }

        [TestMethod]
        public void Binary_AndAlso_Lifted_FalseShortCircuit()
        {
            var e =
                Expression.AndAlso(
                    Expression.Constant(false, typeof(bool?)),
                    NB
                );

            var r =
                Expression.Constant(false, typeof(bool?));

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Binary_AndAlso_Lifted_TrueShortCircuit()
        {
            var e =
                Expression.AndAlso(
                    Expression.Constant(true, typeof(bool?)),
                    NB
                );

            var r =
                NB;

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Binary_AndAlso_Lifted_NullAndFalse()
        {
            var e =
                Expression.AndAlso(
                    Expression.Constant(value: null, typeof(bool?)),
                    Expression.Constant(false, typeof(bool?))
                );

            var r =
                Expression.Constant(false, typeof(bool?));

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Binary_AndAlso_Lifted_NullAndTrue()
        {
            var e =
                Expression.AndAlso(
                    Expression.Constant(value: null, typeof(bool?)),
                    Expression.Constant(true, typeof(bool?))
                );

            var r =
                Expression.Constant(value: null, typeof(bool?));

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Binary_AndAlso_Lifted_NullAndPure()
        {
            var b = Expression.Parameter(typeof(bool?));

            var e =
                Expression.AndAlso(
                    Expression.Constant(value: null, typeof(bool?)),
                    b
                );

            AssertOptimized(e, e);
        }

        [TestMethod]
        public void Binary_AndAlso_Lifted_NullAndNonPure()
        {
            var e =
                Expression.AndAlso(
                    Expression.Constant(value: null, typeof(bool?)),
                    NB
                );

            AssertOptimized(e, e);
        }

        [TestMethod]
        public void Binary_AndAlso_Lifted_NullOrThrow()
        {
            var ex = Expression.Parameter(typeof(Exception));
            var t = Expression.Throw(ex, typeof(bool?));

            var e =
                Expression.AndAlso(
                    Expression.Constant(value: null, typeof(bool?)),
                    t
                );

            var r =
                t;

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Binary_AndAlso_Lifted_Nop()
        {
            var e =
                Expression.AndAlso(
                    NB1,
                    NB2
                );

            AssertOptimized(e, e);
        }

        [TestMethod]
        public void Binary_AndAlso_Lifted_AssertAllConstant()
        {
            foreach (var l in new bool?[] { null, false, true })
            {
                foreach (var r in new bool?[] { null, false, true })
                {
                    var e = Expression.AndAlso(Expression.Constant(l, typeof(bool?)), Expression.Constant(r, typeof(bool?)));
                    var o = GetOptimizer().Visit(e);

                    var evalE = Expression.Lambda<Func<bool?>>(e).Compile();
                    var evalO = Expression.Lambda<Func<bool?>>(o).Compile();

                    Assert.AreEqual(evalE(), evalO(), $"{l} && {r}");
                }
            }
        }

        [TestMethod]
        public void Binary_AndAlso_Method_Nop()
        {
            var b1 = Expression.Parameter(typeof(BinaryLogicalOps));
            var b2 = Expression.Parameter(typeof(BinaryLogicalOps));

            var e =
                Expression.AndAlso(
                    b1,
                    b2
                );

            AssertOptimized(e, e);
        }

        [TestMethod]
        public void Binary_AndAlso_Method_LeftIsFalse()
        {
            lock (BinaryLogicalOps.Lock)
            {
                BinaryLogicalOps.False = () => true;
                BinaryLogicalOps.True = null;
                BinaryLogicalOps.And = null;
                BinaryLogicalOps.Or = null;

                var b1 = Expression.Constant(new BinaryLogicalOps());
                var b2 = Expression.Parameter(typeof(BinaryLogicalOps));

                var e =
                    Expression.AndAlso(
                        b1,
                        b2
                    );

                var r =
                    b1;

                AssertOptimized(GetPureBinaryOptimizer(), e, r);
            }
        }

        [TestMethod]
        public void Binary_AndAlso_Method_LeftNotIsFalse()
        {
            lock (BinaryLogicalOps.Lock)
            {
                BinaryLogicalOps.False = () => false;
                BinaryLogicalOps.True = null;
                BinaryLogicalOps.And = null;
                BinaryLogicalOps.Or = null;

                var b1 = Expression.Constant(new BinaryLogicalOps());
                var b2 = Expression.Parameter(typeof(BinaryLogicalOps));

                var e =
                    Expression.AndAlso(
                        b1,
                        b2
                    );

                var r =
                    Expression.And(
                        b1,
                        b2
                    );

                AssertOptimized(GetPureBinaryOptimizer(), e, r);
            }
        }

        [TestMethod]
        public void Binary_AndAlso_Method_LeftIsFalseThrows()
        {
            lock (BinaryLogicalOps.Lock)
            {
                var ex = new Exception("Oops!");

                BinaryLogicalOps.False = () => { throw ex; };
                BinaryLogicalOps.True = null;
                BinaryLogicalOps.And = null;
                BinaryLogicalOps.Or = null;

                var b1 = Expression.Constant(new BinaryLogicalOps());
                var b2 = Expression.Parameter(typeof(BinaryLogicalOps));

                var e =
                    Expression.AndAlso(
                        b1,
                        b2
                    );

                var r =
                    Expression.Throw(
                        Expression.Constant(ex),
                        typeof(BinaryLogicalOps)
                    );

                AssertOptimized(GetPureBinaryOptimizer(), e, r);
            }
        }

        [TestMethod]
        public void Binary_AndAlso_Method_LeftNotIsFalse_RightThrows()
        {
            lock (BinaryLogicalOps.Lock)
            {
                BinaryLogicalOps.False = () => false;
                BinaryLogicalOps.True = null;
                BinaryLogicalOps.And = null;
                BinaryLogicalOps.Or = null;

                var ex = Expression.Parameter(typeof(Exception));

                var b1 = Expression.Constant(new BinaryLogicalOps());
                var b2 = Expression.Throw(ex, typeof(BinaryLogicalOps));

                var e =
                    Expression.AndAlso(
                        b1,
                        b2
                    );

                var r =
                    b2;

                AssertOptimized(GetPureBinaryOptimizer(), e, r);
            }
        }

        [TestMethod]
        public void Binary_AndAlso_Method_LeftNotIsFalse_RightConstant()
        {
            lock (BinaryLogicalOps.Lock)
            {
                var res = new BinaryLogicalOps();

                BinaryLogicalOps.False = () => false;
                BinaryLogicalOps.True = null;
                BinaryLogicalOps.And = () => res;
                BinaryLogicalOps.Or = null;

                var ex = Expression.Parameter(typeof(Exception));

                var b1 = Expression.Constant(new BinaryLogicalOps());
                var b2 = Expression.Constant(new BinaryLogicalOps());

                var e =
                    Expression.AndAlso(
                        b1,
                        b2
                    );

                var r =
                    Expression.Constant(res);

                AssertOptimized(GetPureBinaryOptimizer(), e, r);
            }
        }

        [TestMethod]
        public void Binary_AndAlso_Method_Lifted_Nop()
        {
            var b1 = Expression.Parameter(typeof(BinaryLogicalOpsStruct?));
            var b2 = Expression.Parameter(typeof(BinaryLogicalOpsStruct?));

            var e =
                Expression.AndAlso(
                    b1,
                    b2
                );

            AssertOptimized(e, e);
        }

        [TestMethod]
        public void Binary_AndAlso_Method_Lifted_LeftIsNull()
        {
            lock (BinaryLogicalOpsStruct.Lock)
            {
                BinaryLogicalOpsStruct.False = null;
                BinaryLogicalOpsStruct.True = null;
                BinaryLogicalOpsStruct.And = null;
                BinaryLogicalOpsStruct.Or = null;

                var b1 = Expression.Constant(value: null, typeof(BinaryLogicalOpsStruct?));
                var b2 = Expression.Parameter(typeof(BinaryLogicalOpsStruct?));

                var e =
                    Expression.AndAlso(
                        b1,
                        b2
                    );

                var r =
                    b1;

                AssertOptimized(GetPureBinaryOptimizer(), e, r);
            }
        }

        [TestMethod]
        public void Binary_AndAlso_Method_Lifted_LeftIsFalse()
        {
            lock (BinaryLogicalOpsStruct.Lock)
            {
                BinaryLogicalOpsStruct.False = () => true;
                BinaryLogicalOpsStruct.True = null;
                BinaryLogicalOpsStruct.And = null;
                BinaryLogicalOpsStruct.Or = null;

                var b1 = Expression.Constant(new BinaryLogicalOpsStruct(), typeof(BinaryLogicalOpsStruct?));
                var b2 = Expression.Parameter(typeof(BinaryLogicalOpsStruct?));

                var e =
                    Expression.AndAlso(
                        b1,
                        b2
                    );

                var r =
                    b1;

                AssertOptimized(GetPureBinaryOptimizer(), e, r);
            }
        }

        [TestMethod]
        public void Binary_AndAlso_Method_Lifted_LeftNotIsFalse()
        {
            lock (BinaryLogicalOpsStruct.Lock)
            {
                BinaryLogicalOpsStruct.False = () => false;
                BinaryLogicalOpsStruct.True = null;
                BinaryLogicalOpsStruct.And = null;
                BinaryLogicalOpsStruct.Or = null;

                var b1 = Expression.Constant(new BinaryLogicalOpsStruct(), typeof(BinaryLogicalOpsStruct?));
                var b2 = Expression.Parameter(typeof(BinaryLogicalOpsStruct?));

                var e =
                    Expression.AndAlso(
                        b1,
                        b2
                    );

                var r =
                    Expression.And(
                        b1,
                        b2
                    );

                AssertOptimized(GetPureBinaryOptimizer(), e, r);
            }
        }

        [TestMethod]
        public void Binary_AndAlso_Method_Lifted_LeftIsFalseThrows()
        {
            lock (BinaryLogicalOpsStruct.Lock)
            {
                var ex = new Exception("Oops!");

                BinaryLogicalOpsStruct.False = () => { throw ex; };
                BinaryLogicalOpsStruct.True = null;
                BinaryLogicalOpsStruct.And = null;
                BinaryLogicalOpsStruct.Or = null;

                var b1 = Expression.Constant(new BinaryLogicalOpsStruct(), typeof(BinaryLogicalOpsStruct?));
                var b2 = Expression.Parameter(typeof(BinaryLogicalOpsStruct?));

                var e =
                    Expression.AndAlso(
                        b1,
                        b2
                    );

                var r =
                    Expression.Throw(
                        Expression.Constant(ex),
                        typeof(BinaryLogicalOpsStruct?)
                    );

                AssertOptimized(GetPureBinaryOptimizer(), e, r);
            }
        }

        [TestMethod]
        public void Binary_AndAlso_Method_Lifted_LeftNotIsFalse_RightThrows()
        {
            lock (BinaryLogicalOpsStruct.Lock)
            {
                BinaryLogicalOpsStruct.False = () => false;
                BinaryLogicalOpsStruct.True = null;
                BinaryLogicalOpsStruct.And = null;
                BinaryLogicalOpsStruct.Or = null;

                var ex = Expression.Parameter(typeof(Exception));

                var b1 = Expression.Constant(new BinaryLogicalOpsStruct(), typeof(BinaryLogicalOpsStruct?));
                var b2 = Expression.Throw(ex, typeof(BinaryLogicalOpsStruct?));

                var e =
                    Expression.AndAlso(
                        b1,
                        b2
                    );

                var r =
                    b2;

                AssertOptimized(GetPureBinaryOptimizer(), e, r);
            }
        }

        [TestMethod]
        public void Binary_AndAlso_Method_Lifted_LeftNotIsFalse_RightIsNull()
        {
            lock (BinaryLogicalOpsStruct.Lock)
            {
                BinaryLogicalOpsStruct.False = () => false;
                BinaryLogicalOpsStruct.True = null;
                BinaryLogicalOpsStruct.And = null;
                BinaryLogicalOpsStruct.Or = null;

                var ex = Expression.Parameter(typeof(Exception));

                var b1 = Expression.Constant(new BinaryLogicalOpsStruct(), typeof(BinaryLogicalOpsStruct?));
                var b2 = Expression.Constant(value: null, typeof(BinaryLogicalOpsStruct?));

                var e =
                    Expression.AndAlso(
                        b1,
                        b2
                    );

                var r =
                    b2;

                AssertOptimized(GetPureBinaryOptimizer(), e, r);
            }
        }

        [TestMethod]
        public void Binary_AndAlso_Method_Lifted_LeftNotIsFalse_RightConstant()
        {
            lock (BinaryLogicalOpsStruct.Lock)
            {
                var res = new BinaryLogicalOpsStruct();

                BinaryLogicalOpsStruct.False = () => false;
                BinaryLogicalOpsStruct.True = null;
                BinaryLogicalOpsStruct.And = () => res;
                BinaryLogicalOpsStruct.Or = null;

                var ex = Expression.Parameter(typeof(Exception));

                var b1 = Expression.Constant(new BinaryLogicalOpsStruct(), typeof(BinaryLogicalOpsStruct?));
                var b2 = Expression.Constant(new BinaryLogicalOpsStruct(), typeof(BinaryLogicalOpsStruct?));

                var e =
                    Expression.AndAlso(
                        b1,
                        b2
                    );

                var r =
                    Expression.Constant(res, typeof(BinaryLogicalOpsStruct?));

                AssertOptimized(GetPureBinaryOptimizer(), e, r);
            }
        }

        [TestMethod]
        public void Binary_OrElse_Unlifted_FalseShortCircuit()
        {
            var e =
                Expression.OrElse(
                    Expression.Constant(false),
                    B
                );

            var r =
                B;

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Binary_OrElse_Unlifted_TrueShortCircuit()
        {
            var e =
                Expression.OrElse(
                    Expression.Constant(true),
                    B
                );

            var r =
                Expression.Constant(true);

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Binary_OrElse_Unlifted_Nop()
        {
            var e =
                Expression.OrElse(
                    B1,
                    B2
                );

            AssertOptimized(e, e);
        }

        [TestMethod]
        public void Binary_OrElse_Unlifted_AssertAllConstant()
        {
            foreach (var l in new[] { false, true })
            {
                foreach (var r in new[] { false, true })
                {
                    var e = Expression.OrElse(Expression.Constant(l), Expression.Constant(r));
                    var o = GetOptimizer().Visit(e);

                    var evalE = Expression.Lambda<Func<bool>>(e).Compile();
                    var evalO = Expression.Lambda<Func<bool>>(o).Compile();

                    Assert.AreEqual(evalE(), evalO(), $"{l} || {r}");
                }
            }
        }

        [TestMethod]
        public void Binary_OrElse_Lifted_FalseShortCircuit()
        {
            var e =
                Expression.OrElse(
                    Expression.Constant(false, typeof(bool?)),
                    NB
                );

            var r =
                NB;

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Binary_OrElse_Lifted_TrueShortCircuit()
        {
            var e =
                Expression.OrElse(
                    Expression.Constant(true, typeof(bool?)),
                    NB
                );

            var r =
                Expression.Constant(true, typeof(bool?));

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Binary_OrElse_Lifted_NullOrFalse()
        {
            var e =
                Expression.OrElse(
                    Expression.Constant(value: null, typeof(bool?)),
                    Expression.Constant(false, typeof(bool?))
                );

            var r =
                Expression.Constant(value: null, typeof(bool?));

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Binary_OrElse_Lifted_NullOrTrue()
        {
            var e =
                Expression.OrElse(
                    Expression.Constant(value: null, typeof(bool?)),
                    Expression.Constant(true, typeof(bool?))
                );

            var r =
                Expression.Constant(true, typeof(bool?));

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Binary_OrElse_Lifted_NullOrPure()
        {
            var b = Expression.Parameter(typeof(bool?));

            var e =
                Expression.OrElse(
                    Expression.Constant(value: null, typeof(bool?)),
                    b
                );

            AssertOptimized(e, e);
        }

        [TestMethod]
        public void Binary_OrElse_Lifted_NullOrNonPure()
        {
            var e =
                Expression.OrElse(
                    Expression.Constant(value: null, typeof(bool?)),
                    NB
                );

            AssertOptimized(e, e);
        }

        [TestMethod]
        public void Binary_OrElse_Lifted_NullOrThrow()
        {
            var ex = Expression.Parameter(typeof(Exception));
            var t = Expression.Throw(ex, typeof(bool?));

            var e =
                Expression.OrElse(
                    Expression.Constant(value: null, typeof(bool?)),
                    t
                );

            var r =
                t;

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Binary_OrElse_Lifted_Nop()
        {
            var e =
                Expression.OrElse(
                    NB1,
                    NB2
                );

            AssertOptimized(e, e);
        }

        [TestMethod]
        public void Binary_OrElse_Lifted_AssertAllConstant()
        {
            foreach (var l in new bool?[] { null, false, true })
            {
                foreach (var r in new bool?[] { null, false, true })
                {
                    var e = Expression.OrElse(Expression.Constant(l, typeof(bool?)), Expression.Constant(r, typeof(bool?)));
                    var o = GetOptimizer().Visit(e);

                    var evalE = Expression.Lambda<Func<bool?>>(e).Compile();
                    var evalO = Expression.Lambda<Func<bool?>>(o).Compile();

                    Assert.AreEqual(evalE(), evalO(), $"{l} || {r}");
                }
            }
        }

        [TestMethod]
        public void Binary_OrElse_Method_Nop()
        {
            var b1 = Expression.Parameter(typeof(BinaryLogicalOps));
            var b2 = Expression.Parameter(typeof(BinaryLogicalOps));

            var e =
                Expression.OrElse(
                    b1,
                    b2
                );

            AssertOptimized(e, e);
        }

        [TestMethod]
        public void Binary_OrElse_Method_LeftIsTrue()
        {
            lock (BinaryLogicalOps.Lock)
            {
                BinaryLogicalOps.True = () => true;
                BinaryLogicalOps.False = null;
                BinaryLogicalOps.And = null;
                BinaryLogicalOps.Or = null;

                var b1 = Expression.Constant(new BinaryLogicalOps());
                var b2 = Expression.Parameter(typeof(BinaryLogicalOps));

                var e =
                    Expression.OrElse(
                        b1,
                        b2
                    );

                var r =
                    b1;

                AssertOptimized(GetPureBinaryOptimizer(), e, r);
            }
        }

        [TestMethod]
        public void Binary_OrElse_Method_LeftNotIsTrue()
        {
            lock (BinaryLogicalOps.Lock)
            {
                BinaryLogicalOps.True = () => false;
                BinaryLogicalOps.False = null;
                BinaryLogicalOps.And = null;
                BinaryLogicalOps.Or = null;

                var b1 = Expression.Constant(new BinaryLogicalOps());
                var b2 = Expression.Parameter(typeof(BinaryLogicalOps));

                var e =
                    Expression.OrElse(
                        b1,
                        b2
                    );

                var r =
                    Expression.Or(
                        b1,
                        b2
                    );

                AssertOptimized(GetPureBinaryOptimizer(), e, r);
            }
        }

        [TestMethod]
        public void Binary_OrElse_Method_LeftIsTrueThrows()
        {
            lock (BinaryLogicalOps.Lock)
            {
                var ex = new Exception("Oops!");

                BinaryLogicalOps.True = () => { throw ex; };
                BinaryLogicalOps.False = null;
                BinaryLogicalOps.And = null;
                BinaryLogicalOps.Or = null;

                var b1 = Expression.Constant(new BinaryLogicalOps());
                var b2 = Expression.Parameter(typeof(BinaryLogicalOps));

                var e =
                    Expression.OrElse(
                        b1,
                        b2
                    );

                var r =
                    Expression.Throw(
                        Expression.Constant(ex),
                        typeof(BinaryLogicalOps)
                    );

                AssertOptimized(GetPureBinaryOptimizer(), e, r);
            }
        }

        [TestMethod]
        public void Binary_OrElse_Method_LeftNotIsTrue_RightThrows()
        {
            lock (BinaryLogicalOps.Lock)
            {
                BinaryLogicalOps.True = () => false;
                BinaryLogicalOps.False = null;
                BinaryLogicalOps.And = null;
                BinaryLogicalOps.Or = null;

                var ex = Expression.Parameter(typeof(Exception));

                var b1 = Expression.Constant(new BinaryLogicalOps());
                var b2 = Expression.Throw(ex, typeof(BinaryLogicalOps));

                var e =
                    Expression.OrElse(
                        b1,
                        b2
                    );

                var r =
                    b2;

                AssertOptimized(GetPureBinaryOptimizer(), e, r);
            }
        }

        [TestMethod]
        public void Binary_OrElse_Method_LeftNotIsTrue_RightConstant()
        {
            lock (BinaryLogicalOps.Lock)
            {
                var res = new BinaryLogicalOps();

                BinaryLogicalOps.True = () => false;
                BinaryLogicalOps.False = null;
                BinaryLogicalOps.And = null;
                BinaryLogicalOps.Or = () => res;

                var ex = Expression.Parameter(typeof(Exception));

                var b1 = Expression.Constant(new BinaryLogicalOps());
                var b2 = Expression.Constant(new BinaryLogicalOps());

                var e =
                    Expression.OrElse(
                        b1,
                        b2
                    );

                var r =
                    Expression.Constant(res);

                AssertOptimized(GetPureBinaryOptimizer(), e, r);
            }
        }

        [TestMethod]
        public void Binary_OrElse_Method_Lifted_Nop()
        {
            var b1 = Expression.Parameter(typeof(BinaryLogicalOpsStruct?));
            var b2 = Expression.Parameter(typeof(BinaryLogicalOpsStruct?));

            var e =
                Expression.OrElse(
                    b1,
                    b2
                );

            AssertOptimized(e, e);
        }

        [TestMethod]
        public void Binary_OrElse_Method_Lifted_LeftIsNull()
        {
            lock (BinaryLogicalOpsStruct.Lock)
            {
                BinaryLogicalOpsStruct.True = null;
                BinaryLogicalOpsStruct.False = null;
                BinaryLogicalOpsStruct.And = null;
                BinaryLogicalOpsStruct.Or = null;

                var b1 = Expression.Constant(value: null, typeof(BinaryLogicalOpsStruct?));
                var b2 = Expression.Parameter(typeof(BinaryLogicalOpsStruct?));

                var e =
                    Expression.OrElse(
                        b1,
                        b2
                    );

                var r =
                    b1;

                AssertOptimized(GetPureBinaryOptimizer(), e, r);
            }
        }

        [TestMethod]
        public void Binary_OrElse_Method_Lifted_LeftIsTrue()
        {
            lock (BinaryLogicalOpsStruct.Lock)
            {
                BinaryLogicalOpsStruct.True = () => true;
                BinaryLogicalOpsStruct.False = null;
                BinaryLogicalOpsStruct.And = null;
                BinaryLogicalOpsStruct.Or = null;

                var b1 = Expression.Constant(new BinaryLogicalOpsStruct(), typeof(BinaryLogicalOpsStruct?));
                var b2 = Expression.Parameter(typeof(BinaryLogicalOpsStruct?));

                var e =
                    Expression.OrElse(
                        b1,
                        b2
                    );

                var r =
                    b1;

                AssertOptimized(GetPureBinaryOptimizer(), e, r);
            }
        }

        [TestMethod]
        public void Binary_OrElse_Method_Lifted_LeftNotIsTrue()
        {
            lock (BinaryLogicalOpsStruct.Lock)
            {
                BinaryLogicalOpsStruct.True = () => false;
                BinaryLogicalOpsStruct.False = null;
                BinaryLogicalOpsStruct.And = null;
                BinaryLogicalOpsStruct.Or = null;

                var b1 = Expression.Constant(new BinaryLogicalOpsStruct(), typeof(BinaryLogicalOpsStruct?));
                var b2 = Expression.Parameter(typeof(BinaryLogicalOpsStruct?));

                var e =
                    Expression.OrElse(
                        b1,
                        b2
                    );

                var r =
                    Expression.Or(
                        b1,
                        b2
                    );

                AssertOptimized(GetPureBinaryOptimizer(), e, r);
            }
        }

        [TestMethod]
        public void Binary_OrElse_Method_Lifted_LeftIsTrueThrows()
        {
            lock (BinaryLogicalOpsStruct.Lock)
            {
                var ex = new Exception("Oops!");

                BinaryLogicalOpsStruct.True = () => { throw ex; };
                BinaryLogicalOpsStruct.False = null;
                BinaryLogicalOpsStruct.And = null;
                BinaryLogicalOpsStruct.Or = null;

                var b1 = Expression.Constant(new BinaryLogicalOpsStruct(), typeof(BinaryLogicalOpsStruct?));
                var b2 = Expression.Parameter(typeof(BinaryLogicalOpsStruct?));

                var e =
                    Expression.OrElse(
                        b1,
                        b2
                    );

                var r =
                    Expression.Throw(
                        Expression.Constant(ex),
                        typeof(BinaryLogicalOpsStruct)
                    );

                AssertOptimized(GetPureBinaryOptimizer(), e, r);
            }
        }

        [TestMethod]
        public void Binary_OrElse_Method_Lifted_LeftNotIsTrue_RightThrows()
        {
            lock (BinaryLogicalOpsStruct.Lock)
            {
                BinaryLogicalOpsStruct.True = () => false;
                BinaryLogicalOpsStruct.False = null;
                BinaryLogicalOpsStruct.And = null;
                BinaryLogicalOpsStruct.Or = null;

                var ex = Expression.Parameter(typeof(Exception));

                var b1 = Expression.Constant(new BinaryLogicalOpsStruct(), typeof(BinaryLogicalOpsStruct?));
                var b2 = Expression.Throw(ex, typeof(BinaryLogicalOpsStruct?));

                var e =
                    Expression.OrElse(
                        b1,
                        b2
                    );

                var r =
                    b2;

                AssertOptimized(GetPureBinaryOptimizer(), e, r);
            }
        }

        [TestMethod]
        public void Binary_OrElse_Method_Lifted_LeftNotIsTrue_RightIsNull()
        {
            lock (BinaryLogicalOpsStruct.Lock)
            {
                BinaryLogicalOpsStruct.True = () => false;
                BinaryLogicalOpsStruct.False = null;
                BinaryLogicalOpsStruct.And = null;
                BinaryLogicalOpsStruct.Or = null;

                var ex = Expression.Parameter(typeof(Exception));

                var b1 = Expression.Constant(new BinaryLogicalOpsStruct(), typeof(BinaryLogicalOpsStruct?));
                var b2 = Expression.Constant(value: null, typeof(BinaryLogicalOpsStruct?));

                var e =
                    Expression.OrElse(
                        b1,
                        b2
                    );

                var r =
                    b2;

                AssertOptimized(GetPureBinaryOptimizer(), e, r);
            }
        }

        [TestMethod]
        public void Binary_OrElse_Method_Lifted_LeftNotIsTrue_RightConstant()
        {
            lock (BinaryLogicalOpsStruct.Lock)
            {
                var res = new BinaryLogicalOpsStruct();

                BinaryLogicalOpsStruct.True = () => false;
                BinaryLogicalOpsStruct.False = null;
                BinaryLogicalOpsStruct.And = null;
                BinaryLogicalOpsStruct.Or = () => res;

                var ex = Expression.Parameter(typeof(Exception));

                var b1 = Expression.Constant(new BinaryLogicalOpsStruct(), typeof(BinaryLogicalOpsStruct?));
                var b2 = Expression.Constant(new BinaryLogicalOpsStruct(), typeof(BinaryLogicalOpsStruct?));

                var e =
                    Expression.OrElse(
                        b1,
                        b2
                    );

                var r =
                    Expression.Constant(res, typeof(BinaryLogicalOpsStruct?));

                AssertOptimized(GetPureBinaryOptimizer(), e, r);
            }
        }

        [TestMethod]
        public void Binary_Arithmetic_Method_NonLifted_ThrowLeft()
        {
            var ex = Expression.Constant(new Exception());
            var t = Expression.Throw(ex, typeof(decimal));

            var e =
                Expression.Add(
                    t,
                    Expression.Parameter(typeof(decimal))
                );

            var r =
                Expression.Throw(ex, typeof(decimal));

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Binary_Arithmetic_Method_NonLifted_PureLeft_ThrowRight()
        {
            var ex = Expression.Constant(new Exception());
            var t = Expression.Throw(ex, typeof(decimal));

            var e =
                Expression.Add(
                    Expression.Parameter(typeof(decimal)),
                    t
                );

            var r =
                Expression.Throw(ex, typeof(decimal));

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Binary_Arithmetic_Method_NonLifted_NonPureLeft_ThrowRight()
        {
            var ex = Expression.Constant(new Exception());
            var t = Expression.Throw(ex, typeof(decimal));

            var e =
                Expression.Add(
                    D,
                    t
                );

            AssertOptimized(e, e);
        }

        [TestMethod]
        public void Binary_Arithmetic_Method_NonLifted_Eval_NonPureMethod()
        {
            var e =
                Expression.Add(
                    Expression.Constant(2.0m),
                    Expression.Constant(3.0m)
                );

            AssertOptimized(e, e);
        }

        [TestMethod]
        public void Binary_Arithmetic_Method_NonLifted_Eval_PureMethod()
        {
            var e =
                Expression.Add(
                    Expression.Constant(2.0m),
                    Expression.Constant(3.0m)
                );

            var r =
                Expression.Constant(5.0m);

            AssertOptimized(GetPureBinaryOptimizer(), e, r);
        }

        [TestMethod]
        public void Binary_Arithmetic_Method_NonLifted_Nop()
        {
            var e =
                Expression.Add(
                    D1,
                    D2
                );

            AssertOptimized(e, e);
        }

        [TestMethod]
        public void Binary_Arithmetic_Method_Lifted_ThrowLeft()
        {
            var ex = Expression.Constant(new Exception());
            var t = Expression.Throw(ex, typeof(decimal?));

            var e =
                Expression.Add(
                    t,
                    Expression.Parameter(typeof(decimal?))
                );

            var r =
                Expression.Throw(ex, typeof(decimal?));

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Binary_Arithmetic_Method_Lifted_PureLeft_ThrowRight()
        {
            var ex = Expression.Constant(new Exception());
            var t = Expression.Throw(ex, typeof(decimal?));

            var e =
                Expression.Add(
                    Expression.Parameter(typeof(decimal?)),
                    t
                );

            var r =
                Expression.Throw(ex, typeof(decimal?));

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Binary_Arithmetic_Method_Lifted_NonPureLeft_ThrowRight()
        {
            var ex = Expression.Constant(new Exception());
            var t = Expression.Throw(ex, typeof(decimal?));

            var e =
                Expression.Add(
                    ND,
                    t
                );

            AssertOptimized(e, e);
        }

        [TestMethod]
        public void Binary_Arithmetic_Method_Lifted_Nop()
        {
            var e =
                Expression.Add(
                    ND1,
                    ND2
                );

            AssertOptimized(e, e);
        }

        [TestMethod]
        public void Binary_Arithmetic_Method_Lifted_LeftNull_RightPure()
        {
            var e =
                Expression.Add(
                    Expression.Constant(value: null, typeof(decimal?)),
                    Expression.Parameter(typeof(decimal?))
                );

            var r =
                Expression.Constant(value: null, typeof(decimal?));

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Binary_Arithmetic_Method_Lifted_LeftNull_RightNonPure()
        {
            var e =
                Expression.Add(
                    Expression.Constant(value: null, typeof(decimal?)),
                    ND
                );

            AssertOptimized(e, e);
        }

        [TestMethod]
        public void Binary_Arithmetic_Method_Lifted_LeftPure_RightNull()
        {
            var e =
                Expression.Add(
                    Expression.Parameter(typeof(decimal?)),
                    Expression.Constant(value: null, typeof(decimal?))
                );

            var r =
                Expression.Constant(value: null, typeof(decimal?));

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Binary_Arithmetic_Method_Lifted_LeftNonPure_RightNull()
        {
            var e =
                Expression.Add(
                    ND,
                    Expression.Constant(value: null, typeof(decimal?))
                );

            AssertOptimized(e, e);
        }

        [TestMethod]
        public void Binary_Arithmetic_Method_Lifted_LeftNull_RightNull()
        {
            var e =
                Expression.Add(
                    Expression.Constant(value: null, typeof(decimal?)),
                    Expression.Constant(value: null, typeof(decimal?))
                );

            var r =
                Expression.Constant(value: null, typeof(decimal?));

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Binary_Arithmetic_Method_Lifted_Eval_NonPureMethod()
        {
            var e =
                Expression.Add(
                    Expression.Constant(2.0m, typeof(decimal?)),
                    Expression.Constant(3.0m, typeof(decimal?))
                );

            AssertOptimized(e, e);
        }

        [TestMethod]
        public void Binary_Arithmetic_Method_Lifted_Eval_PureMethod()
        {
            var e =
                Expression.Add(
                    Expression.Constant(2.0m, typeof(decimal?)),
                    Expression.Constant(3.0m, typeof(decimal?))
                );

            var r =
                Expression.Constant(5.0m, typeof(decimal?));

            AssertOptimized(GetPureBinaryOptimizer(), e, r);
        }

        [TestMethod]
        public void Binary_Comparison_Method_NonLifted_ThrowLeft()
        {
            var ex = Expression.Constant(new Exception());
            var t = Expression.Throw(ex, typeof(decimal));

            var e =
                Expression.LessThan(
                    t,
                    Expression.Parameter(typeof(decimal))
                );

            var r =
                Expression.Throw(ex, typeof(bool));

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Binary_Comparison_Method_NonLifted_PureLeft_ThrowRight()
        {
            var ex = Expression.Constant(new Exception());
            var t = Expression.Throw(ex, typeof(decimal));

            var e =
                Expression.LessThan(
                    Expression.Parameter(typeof(decimal)),
                    t
                );

            var r =
                Expression.Throw(ex, typeof(bool));

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Binary_Comparison_Method_NonLifted_NonPureLeft_ThrowRight()
        {
            var ex = Expression.Constant(new Exception());
            var t = Expression.Throw(ex, typeof(decimal));

            var e =
                Expression.LessThan(
                    D,
                    t
                );

            AssertOptimized(e, e);
        }

        [TestMethod]
        public void Binary_Comparison_Method_NonLifted_Eval_NonPureMethod()
        {
            var e =
                Expression.LessThan(
                    Expression.Constant(2.0m),
                    Expression.Constant(3.0m)
                );

            AssertOptimized(e, e);
        }

        [TestMethod]
        public void Binary_Comparison_Method_NonLifted_Eval_PureMethod()
        {
            var e =
                Expression.LessThan(
                    Expression.Constant(2.0m),
                    Expression.Constant(3.0m)
                );

            var r =
                Expression.Constant(true);

            AssertOptimized(GetPureBinaryOptimizer(), e, r);
        }

        [TestMethod]
        public void Binary_Comparison_Method_NonLifted_Nop()
        {
            var e =
                Expression.LessThan(
                    D1,
                    D2
                );

            AssertOptimized(e, e);
        }

        [TestMethod]
        public void Binary_Comparison_Method_Lifted_ThrowLeft()
        {
            var ex = Expression.Constant(new Exception());
            var t = Expression.Throw(ex, typeof(decimal?));

            var e =
                Expression.LessThan(
                    t,
                    Expression.Parameter(typeof(decimal?))
                );

            var r =
                Expression.Throw(ex, typeof(bool));

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Binary_Comparison_Method_Lifted_PureLeft_ThrowRight()
        {
            var ex = Expression.Constant(new Exception());
            var t = Expression.Throw(ex, typeof(decimal?));

            var e =
                Expression.LessThan(
                    Expression.Parameter(typeof(decimal?)),
                    t
                );

            var r =
                Expression.Throw(ex, typeof(bool));

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Binary_Comparison_Method_Lifted_NonPureLeft_ThrowRight()
        {
            var ex = Expression.Constant(new Exception());
            var t = Expression.Throw(ex, typeof(decimal?));

            var e =
                Expression.LessThan(
                    ND,
                    t
                );

            AssertOptimized(e, e);
        }

        [TestMethod]
        public void Binary_Comparison_Method_Lifted_Nop()
        {
            var e =
                Expression.LessThan(
                    ND1,
                    ND2
                );

            AssertOptimized(e, e);
        }

        [TestMethod]
        public void Binary_Comparison_Method_Lifted_LeftNull_RightPure()
        {
            var e =
                Expression.LessThan(
                    Expression.Constant(value: null, typeof(decimal?)),
                    Expression.Parameter(typeof(decimal?))
                );

            var r =
                Expression.Constant(false, typeof(bool));

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Binary_Comparison_Method_Lifted_LeftNull_RightNonPure()
        {
            var e =
                Expression.LessThan(
                    Expression.Constant(value: null, typeof(decimal?)),
                    ND
                );

            AssertOptimized(e, e);
        }

        [TestMethod]
        public void Binary_Comparison_Method_Lifted_LeftPure_RightNull()
        {
            var e =
                Expression.LessThan(
                    Expression.Parameter(typeof(decimal?)),
                    Expression.Constant(value: null, typeof(decimal?))
                );

            var r =
                Expression.Constant(false, typeof(bool));

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Binary_Comparison_Method_Lifted_LeftNonPure_RightNull()
        {
            var e =
                Expression.LessThan(
                    ND,
                    Expression.Constant(value: null, typeof(decimal?))
                );

            AssertOptimized(e, e);
        }

        [TestMethod]
        public void Binary_Comparison_Method_Lifted_LeftNull_RightNull()
        {
            var e =
                Expression.LessThan(
                    Expression.Constant(value: null, typeof(decimal?)),
                    Expression.Constant(value: null, typeof(decimal?))
                );

            var r =
                Expression.Constant(false, typeof(bool));

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Binary_Comparison_Method_Lifted_Eval_NonPureMethod()
        {
            var e =
                Expression.LessThan(
                    Expression.Constant(2.0m, typeof(decimal?)),
                    Expression.Constant(3.0m, typeof(decimal?))
                );

            AssertOptimized(e, e);
        }

        [TestMethod]
        public void Binary_Comparison_Method_Lifted_Eval_PureMethod()
        {
            var e =
                Expression.LessThan(
                    Expression.Constant(2.0m, typeof(decimal?)),
                    Expression.Constant(3.0m, typeof(decimal?))
                );

            var r =
                Expression.Constant(true, typeof(bool));

            AssertOptimized(GetPureBinaryOptimizer(), e, r);
        }

        [TestMethod]
        public void Binary_Comparison_Method_Lifted_LiftToNull_ThrowLeft()
        {
            var ex = Expression.Constant(new Exception());
            var t = Expression.Throw(ex, typeof(decimal?));

            var e =
                Expression.LessThan(
                    t,
                    Expression.Parameter(typeof(decimal?)),
                    true,
                    null
                );

            var r =
                Expression.Throw(ex, typeof(bool?));

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Binary_Comparison_Method_Lifted_LiftToNull_PureLeft_ThrowRight()
        {
            var ex = Expression.Constant(new Exception());
            var t = Expression.Throw(ex, typeof(decimal?));

            var e =
                Expression.LessThan(
                    Expression.Parameter(typeof(decimal?)),
                    t,
                    true,
                    null
                );

            var r =
                Expression.Throw(ex, typeof(bool?));

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Binary_Comparison_Method_Lifted_LiftToNull_NonPureLeft_ThrowRight()
        {
            var ex = Expression.Constant(new Exception());
            var t = Expression.Throw(ex, typeof(decimal?));

            var e =
                Expression.LessThan(
                    ND,
                    t,
                    true,
                    null
                );

            AssertOptimized(e, e);
        }

        [TestMethod]
        public void Binary_Comparison_Method_Lifted_LiftToNull_Nop()
        {
            var e =
                Expression.LessThan(
                    ND1,
                    ND2,
                    true,
                    null
                );

            AssertOptimized(e, e);
        }

        [TestMethod]
        public void Binary_Comparison_Method_Lifted_LiftToNull_LeftNull_RightPure()
        {
            var e =
                Expression.LessThan(
                    Expression.Constant(value: null, typeof(decimal?)),
                    Expression.Parameter(typeof(decimal?)),
                    true,
                    null
                );

            var r =
                Expression.Constant(value: null, typeof(bool?));

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Binary_Comparison_Method_Lifted_LiftToNull_LeftNull_RightNonPure()
        {
            var e =
                Expression.LessThan(
                    Expression.Constant(value: null, typeof(decimal?)),
                    ND,
                    true,
                    null
                );

            AssertOptimized(e, e);
        }

        [TestMethod]
        public void Binary_Comparison_Method_Lifted_LiftToNull_LeftPure_RightNull()
        {
            var e =
                Expression.LessThan(
                    Expression.Parameter(typeof(decimal?)),
                    Expression.Constant(value: null, typeof(decimal?)),
                    true,
                    null
                );

            var r =
                Expression.Constant(value: null, typeof(bool?));

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Binary_Comparison_Method_Lifted_LiftToNull_LeftNonPure_RightNull()
        {
            var e =
                Expression.LessThan(
                    ND,
                    Expression.Constant(value: null, typeof(decimal?)),
                    true,
                    null
                );

            AssertOptimized(e, e);
        }

        [TestMethod]
        public void Binary_Comparison_Method_Lifted_LiftToNull_LeftNull_RightNull()
        {
            var e =
                Expression.LessThan(
                    Expression.Constant(value: null, typeof(decimal?)),
                    Expression.Constant(value: null, typeof(decimal?)),
                    true,
                    null
                );

            var r =
                Expression.Constant(value: null, typeof(bool?));

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Binary_Comparison_Method_Lifted_LiftToNull_Eval_NonPureMethod()
        {
            var e =
                Expression.LessThan(
                    Expression.Constant(2.0m, typeof(decimal?)),
                    Expression.Constant(3.0m, typeof(decimal?)),
                    true,
                    null
                );

            AssertOptimized(e, e);
        }

        [TestMethod]
        public void Binary_Comparison_Method_Lifted_LiftToNull_Eval_PureMethod()
        {
            var e =
                Expression.LessThan(
                    Expression.Constant(2.0m, typeof(decimal?)),
                    Expression.Constant(3.0m, typeof(decimal?)),
                    true,
                    null
                );

            var r =
                Expression.Constant(true, typeof(bool?));

            AssertOptimized(GetPureBinaryOptimizer(), e, r);
        }

        [TestMethod]
        public void Binary_Equality_Method_NonLifted_ThrowLeft()
        {
            var ex = Expression.Constant(new Exception());
            var t = Expression.Throw(ex, typeof(decimal));

            var e =
                Expression.Equal(
                    t,
                    Expression.Parameter(typeof(decimal))
                );

            var r =
                Expression.Throw(ex, typeof(bool));

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Binary_Equality_Method_NonLifted_PureLeft_ThrowRight()
        {
            var ex = Expression.Constant(new Exception());
            var t = Expression.Throw(ex, typeof(decimal));

            var e =
                Expression.Equal(
                    Expression.Parameter(typeof(decimal)),
                    t
                );

            var r =
                Expression.Throw(ex, typeof(bool));

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Binary_Equality_Method_NonLifted_NonPureLeft_ThrowRight()
        {
            var ex = Expression.Constant(new Exception());
            var t = Expression.Throw(ex, typeof(decimal));

            var e =
                Expression.Equal(
                    D,
                    t
                );

            AssertOptimized(e, e);
        }

        [TestMethod]
        public void Binary_Equality_Method_NonLifted_Eval_NonPureMethod()
        {
            var e =
                Expression.Equal(
                    Expression.Constant(2.0m),
                    Expression.Constant(3.0m)
                );

            AssertOptimized(e, e);
        }

        [TestMethod]
        public void Binary_Equality_Method_NonLifted_Eval_PureMethod()
        {
            var e =
                Expression.Equal(
                    Expression.Constant(2.0m),
                    Expression.Constant(3.0m)
                );

            var r =
                Expression.Constant(false);

            AssertOptimized(GetPureBinaryOptimizer(), e, r);
        }

        [TestMethod]
        public void Binary_Inequality_Method_NonLifted_Eval_PureMethod()
        {
            var e =
                Expression.NotEqual(
                    Expression.Constant(2.0m),
                    Expression.Constant(3.0m)
                );

            var r =
                Expression.Constant(true);

            AssertOptimized(GetPureBinaryOptimizer(), e, r);
        }

        [TestMethod]
        public void Binary_Equality_Method_NonLifted_Nop()
        {
            var e =
                Expression.Equal(
                    D1,
                    D2
                );

            AssertOptimized(e, e);
        }

        [TestMethod]
        public void Binary_Equality_Method_Lifted_ThrowLeft()
        {
            var ex = Expression.Constant(new Exception());
            var t = Expression.Throw(ex, typeof(decimal?));

            var e =
                Expression.Equal(
                    t,
                    Expression.Parameter(typeof(decimal?))
                );

            var r =
                Expression.Throw(ex, typeof(bool));

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Binary_Equality_Method_Lifted_PureLeft_ThrowRight()
        {
            var ex = Expression.Constant(new Exception());
            var t = Expression.Throw(ex, typeof(decimal?));

            var e =
                Expression.Equal(
                    Expression.Parameter(typeof(decimal?)),
                    t
                );

            var r =
                Expression.Throw(ex, typeof(bool));

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Binary_Equality_Method_Lifted_NonPureLeft_ThrowRight()
        {
            var ex = Expression.Constant(new Exception());
            var t = Expression.Throw(ex, typeof(decimal?));

            var e =
                Expression.Equal(
                    ND,
                    t
                );

            AssertOptimized(e, e);
        }

        [TestMethod]
        public void Binary_Equality_Method_Lifted_Nop()
        {
            var e =
                Expression.Equal(
                    ND1,
                    ND2
                );

            AssertOptimized(e, e);
        }

        [TestMethod]
        public void Binary_Equality_Method_Lifted_LeftNull_RightPure()
        {
            var e =
                Expression.Equal(
                    Expression.Constant(value: null, typeof(decimal?)),
                    Expression.Parameter(typeof(decimal?))
                );

            AssertOptimized(e, e);
        }

        [TestMethod]
        public void Binary_Inequality_Method_Lifted_LeftNull_RightPure()
        {
            var e =
                Expression.NotEqual(
                    Expression.Constant(value: null, typeof(decimal?)),
                    Expression.Parameter(typeof(decimal?))
                );

            AssertOptimized(e, e);
        }

        [TestMethod]
        public void Binary_Equality_Method_Lifted_LeftNull_RightNonPure()
        {
            var e =
                Expression.Equal(
                    Expression.Constant(value: null, typeof(decimal?)),
                    ND
                );

            AssertOptimized(e, e);
        }

        [TestMethod]
        public void Binary_Equality_Method_Lifted_LeftPure_RightNull()
        {
            var e =
                Expression.Equal(
                    Expression.Parameter(typeof(decimal?)),
                    Expression.Constant(value: null, typeof(decimal?))
                );

            AssertOptimized(e, e);
        }

        [TestMethod]
        public void Binary_Inequality_Method_Lifted_LeftPure_RightNull()
        {
            var e =
                Expression.NotEqual(
                    Expression.Parameter(typeof(decimal?)),
                    Expression.Constant(value: null, typeof(decimal?))
                );

            AssertOptimized(e, e);
        }

        [TestMethod]
        public void Binary_Equality_Method_Lifted_LeftNonPure_RightNull()
        {
            var e =
                Expression.Equal(
                    ND,
                    Expression.Constant(value: null, typeof(decimal?))
                );

            AssertOptimized(e, e);
        }

        [TestMethod]
        public void Binary_Equality_Method_Lifted_LeftNull_RightNull()
        {
            var e =
                Expression.Equal(
                    Expression.Constant(value: null, typeof(decimal?)),
                    Expression.Constant(value: null, typeof(decimal?))
                );

            var r =
                Expression.Constant(true, typeof(bool));

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Binary_Inequality_Method_Lifted_LeftNull_RightNull()
        {
            var e =
                Expression.NotEqual(
                    Expression.Constant(value: null, typeof(decimal?)),
                    Expression.Constant(value: null, typeof(decimal?))
                );

            var r =
                Expression.Constant(false, typeof(bool));

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Binary_Equality_Method_Lifted_Eval_NonPureMethod()
        {
            var e =
                Expression.Equal(
                    Expression.Constant(2.0m, typeof(decimal?)),
                    Expression.Constant(3.0m, typeof(decimal?))
                );

            AssertOptimized(e, e);
        }

        [TestMethod]
        public void Binary_Equality_Method_Lifted_Eval_PureMethod()
        {
            var e =
                Expression.Equal(
                    Expression.Constant(2.0m, typeof(decimal?)),
                    Expression.Constant(3.0m, typeof(decimal?))
                );

            var r =
                Expression.Constant(false, typeof(bool));

            AssertOptimized(GetPureBinaryOptimizer(), e, r);
        }

        [TestMethod]
        public void Binary_Inequality_Method_Lifted_Eval_PureMethod()
        {
            var e =
                Expression.NotEqual(
                    Expression.Constant(2.0m, typeof(decimal?)),
                    Expression.Constant(3.0m, typeof(decimal?))
                );

            var r =
                Expression.Constant(true, typeof(bool));

            AssertOptimized(GetPureBinaryOptimizer(), e, r);
        }

        [TestMethod]
        public void Binary_Equality_Method_Lifted_LiftToNull_ThrowLeft()
        {
            var ex = Expression.Constant(new Exception());
            var t = Expression.Throw(ex, typeof(decimal?));

            var e =
                Expression.Equal(
                    t,
                    Expression.Parameter(typeof(decimal?)),
                    true,
                    null
                );

            var r =
                Expression.Throw(ex, typeof(bool?));

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Binary_Equality_Method_Lifted_LiftToNull_PureLeft_ThrowRight()
        {
            var ex = Expression.Constant(new Exception());
            var t = Expression.Throw(ex, typeof(decimal?));

            var e =
                Expression.Equal(
                    Expression.Parameter(typeof(decimal?)),
                    t,
                    true,
                    null
                );

            var r =
                Expression.Throw(ex, typeof(bool?));

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Binary_Equality_Method_Lifted_LiftToNull_NonPureLeft_ThrowRight()
        {
            var ex = Expression.Constant(new Exception());
            var t = Expression.Throw(ex, typeof(decimal?));

            var e =
                Expression.Equal(
                    ND,
                    t,
                    true,
                    null
                );

            AssertOptimized(e, e);
        }

        [TestMethod]
        public void Binary_Equality_Method_Lifted_LiftToNull_Nop()
        {
            var e =
                Expression.Equal(
                    ND1,
                    ND2,
                    true,
                    null
                );

            AssertOptimized(e, e);
        }

        [TestMethod]
        public void Binary_Equality_Method_Lifted_LiftToNull_LeftNull_RightPure()
        {
            var e =
                Expression.Equal(
                    Expression.Constant(value: null, typeof(decimal?)),
                    Expression.Parameter(typeof(decimal?)),
                    true,
                    null
                );


            var r =
                Expression.Constant(value: null, typeof(bool?));

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Binary_Inequality_Method_Lifted_LiftToNull_LeftNull_RightPure()
        {
            var e =
                Expression.NotEqual(
                    Expression.Constant(value: null, typeof(decimal?)),
                    Expression.Parameter(typeof(decimal?)),
                    true,
                    null
                );

            var r =
                Expression.Constant(value: null, typeof(bool?));

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Binary_Equality_Method_Lifted_LiftToNull_LeftNull_RightNonPure()
        {
            var e =
                Expression.Equal(
                    Expression.Constant(value: null, typeof(decimal?)),
                    ND,
                    true,
                    null
                );

            AssertOptimized(e, e);
        }

        [TestMethod]
        public void Binary_Equality_Method_Lifted_LiftToNull_LeftPure_RightNull()
        {
            var e =
                Expression.Equal(
                    Expression.Parameter(typeof(decimal?)),
                    Expression.Constant(value: null, typeof(decimal?)),
                    true,
                    null
                );

            var r =
                Expression.Constant(value: null, typeof(bool?));

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Binary_Equality_Method_Lifted_LiftToNull_LeftNonPure_RightNull()
        {
            var e =
                Expression.Equal(
                    ND,
                    Expression.Constant(value: null, typeof(decimal?)),
                    true,
                    null
                );

            AssertOptimized(e, e);
        }

        [TestMethod]
        public void Binary_Equality_Method_Lifted_LiftToNull_LeftNull_RightNull()
        {
            var e =
                Expression.Equal(
                    Expression.Constant(value: null, typeof(decimal?)),
                    Expression.Constant(value: null, typeof(decimal?)),
                    true,
                    null
                );

            var r =
                Expression.Constant(value: null, typeof(bool?));

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Binary_Inequality_Method_Lifted_LiftToNull_LeftNull_RightNull()
        {
            var e =
                Expression.NotEqual(
                    Expression.Constant(value: null, typeof(decimal?)),
                    Expression.Constant(value: null, typeof(decimal?)),
                    true,
                    null
                );

            var r =
                Expression.Constant(value: null, typeof(bool?));

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Binary_Equality_Method_Lifted_LiftToNull_Eval_NonPureMethod()
        {
            var e =
                Expression.Equal(
                    Expression.Constant(2.0m, typeof(decimal?)),
                    Expression.Constant(3.0m, typeof(decimal?)),
                    true,
                    null
                );

            AssertOptimized(e, e);
        }

        [TestMethod]
        public void Binary_Equality_Method_Lifted_LiftToNull_Eval_PureMethod()
        {
            var e =
                Expression.Equal(
                    Expression.Constant(2.0m, typeof(decimal?)),
                    Expression.Constant(3.0m, typeof(decimal?)),
                    true,
                    null
                );

            var r =
                Expression.Constant(false, typeof(bool?));

            AssertOptimized(GetPureBinaryOptimizer(), e, r);
        }

        [TestMethod]
        public void Binary_Arithmetic_Method_NonLifted()
        {
            var xs = new[] { new BigInteger(0), new BigInteger(1), new BigInteger(2), new BigInteger(3), new BigInteger(42) };
            var ops = new[] { ExpressionType.Add, ExpressionType.Subtract, ExpressionType.Multiply, ExpressionType.Divide, ExpressionType.Modulo };

            foreach (var op in ops)
            {
                AssertBinary<BigInteger, BigInteger>((l, r) => Expression.MakeBinary(op, l, r), xs);
            }
        }

        [TestMethod]
        public void Binary_Bitwise_Method_NonLifted()
        {
            var xs = new[] { new BigInteger(0), new BigInteger(1), new BigInteger(2), new BigInteger(3), new BigInteger(42) };
            var ops = new[] { ExpressionType.And, ExpressionType.Or, ExpressionType.ExclusiveOr };

            foreach (var op in ops)
            {
                AssertBinary<BigInteger, BigInteger>((l, r) => Expression.MakeBinary(op, l, r), xs);
            }
        }

        [TestMethod]
        public void Binary_Comparison_Method_NonLifted()
        {
            var xs = new[] { new BigInteger(0), new BigInteger(1), new BigInteger(2), new BigInteger(3), new BigInteger(42) };
            var ops = new[] { ExpressionType.LessThan, ExpressionType.LessThanOrEqual, ExpressionType.GreaterThan, ExpressionType.GreaterThanOrEqual };

            foreach (var op in ops)
            {
                AssertBinary<BigInteger, bool>((l, r) => Expression.MakeBinary(op, l, r), xs);
            }
        }

        [TestMethod]
        public void Binary_Equality_Method_NonLifted()
        {
            var xs = new[] { new BigInteger(0), new BigInteger(1), new BigInteger(2), new BigInteger(3), new BigInteger(42) };
            var ops = new[] { ExpressionType.Equal, ExpressionType.NotEqual };

            foreach (var op in ops)
            {
                AssertBinary<BigInteger, bool>((l, r) => Expression.MakeBinary(op, l, r), xs);
            }
        }

        [TestMethod]
        public void Binary_Arithmetic_Method_Lifted()
        {
            var xs = new BigInteger?[] { null, new BigInteger(0), new BigInteger(1), new BigInteger(2), new BigInteger(3), new BigInteger(42) };
            var ops = new[] { ExpressionType.Add, ExpressionType.Subtract, ExpressionType.Multiply, ExpressionType.Divide, ExpressionType.Modulo };

            foreach (var op in ops)
            {
                AssertBinary<BigInteger?, BigInteger?>((l, r) => Expression.MakeBinary(op, l, r), xs);
            }
        }

        [TestMethod]
        public void Binary_Bitwise_Method_Lifted()
        {
            var xs = new BigInteger?[] { null, new BigInteger(0), new BigInteger(1), new BigInteger(2), new BigInteger(3), new BigInteger(42) };
            var ops = new[] { ExpressionType.And, ExpressionType.Or, ExpressionType.ExclusiveOr };

            foreach (var op in ops)
            {
                AssertBinary<BigInteger?, BigInteger?>((l, r) => Expression.MakeBinary(op, l, r), xs);
            }
        }

        [TestMethod]
        public void Binary_Comparison_Method_Lifted()
        {
            var xs = new BigInteger?[] { null, new BigInteger(0), new BigInteger(1), new BigInteger(2), new BigInteger(3), new BigInteger(42) };
            var ops = new[] { ExpressionType.LessThan, ExpressionType.LessThanOrEqual, ExpressionType.GreaterThan, ExpressionType.GreaterThanOrEqual };

            foreach (var op in ops)
            {
                AssertBinary<BigInteger?, bool>((l, r) => Expression.MakeBinary(op, l, r), xs);
            }
        }

        [TestMethod]
        public void Binary_Equality_Method_Lifted()
        {
            var xs = new BigInteger?[] { null, new BigInteger(0), new BigInteger(1), new BigInteger(2), new BigInteger(3), new BigInteger(42) };
            var ops = new[] { ExpressionType.Equal, ExpressionType.NotEqual };

            foreach (var op in ops)
            {
                AssertBinary<BigInteger?, bool>((l, r) => Expression.MakeBinary(op, l, r), xs);
            }
        }

        [TestMethod]
        public void Binary_Comparison_Method_LiftedToNull()
        {
            var xs = new BigInteger?[] { null, new BigInteger(0), new BigInteger(1), new BigInteger(2), new BigInteger(3), new BigInteger(42) };
            var ops = new[] { ExpressionType.LessThan, ExpressionType.LessThanOrEqual, ExpressionType.GreaterThan, ExpressionType.GreaterThanOrEqual };

            foreach (var op in ops)
            {
                AssertBinary<BigInteger?, bool?>((l, r) => Expression.MakeBinary(op, l, r, liftToNull: true, method: null), xs);
            }
        }

        [TestMethod]
        public void Binary_Equality_Method_LiftedToNull()
        {
            var xs = new BigInteger?[] { null, new BigInteger(0), new BigInteger(1), new BigInteger(2), new BigInteger(3), new BigInteger(42) };
            var ops = new[] { ExpressionType.Equal, ExpressionType.NotEqual };

            foreach (var op in ops)
            {
                AssertBinary<BigInteger?, bool?>((l, r) => Expression.MakeBinary(op, l, r, liftToNull: true, method: null), xs);
            }
        }

        private static void AssertBinary<T, R>(Func<Expression, Expression, Expression> original, T[] values)
        {
            foreach (var opt in new[] { GetOptimizer(), GetPureBinaryOptimizer() })
            {
                var l = Expression.Parameter(typeof(T), "x");
                var r = Expression.Parameter(typeof(T), "y");

                var o = Expression.Lambda<Func<T, T, R>>(original(l, r), l, r);
                var z = opt.VisitAndConvert(o, "Test");

                AssertBinary(o, z, values);

                foreach (var x in values)
                {
                    foreach (var y in values)
                    {
                        var e = Expression.Lambda<Func<R>>(original(Expression.Constant(x, typeof(T)), Expression.Constant(y, typeof(T))));
                        var f = opt.VisitAndConvert(e, "Test");

                        AssertBinary(e, f);
                    }
                }
            }
        }

        private static void AssertBinary<T, R>(Expression<Func<T, T, R>> original, Expression<Func<T, T, R>> alternative, T[] values)
        {
            var o = original.Compile();
            var a = alternative.Compile();

            foreach (var x in values)
            {
                foreach (var y in values)
                {
                    var oRes = default(object);
                    var aRes = default(object);

                    var oEx = default(Exception);
                    var aEx = default(Exception);

                    try
                    {
                        oRes = o(x, y);
                    }
                    catch (Exception ex)
                    {
                        oEx = ex;
                    }

                    try
                    {
                        aRes = a(x, y);
                    }
                    catch (Exception ex)
                    {
                        aEx = ex;
                    }

                    Assert.AreEqual(oEx != null, aEx != null, $"Failed for ({x},{y}) - {original.Body} != {alternative.Body}");

                    if (oEx != null)
                    {
                        Assert.AreEqual(oEx.GetType(), aEx.GetType(), $"Failed for ({x},{y}) - {original.Body} != {alternative.Body}");
                    }
                    else
                    {
                        Assert.AreEqual(oRes, aRes, $"Failed for ({x},{y}) - {original.Body} != {alternative.Body}");
                    }
                }
            }
        }

        private static void AssertBinary<R>(Expression<Func<R>> original, Expression<Func<R>> alternative)
        {
            var o = original.Compile();
            var a = alternative.Compile();

            var oRes = default(object);
            var aRes = default(object);

            var oEx = default(Exception);
            var aEx = default(Exception);

            try
            {
                oRes = o();
            }
            catch (Exception ex)
            {
                oEx = ex;
            }

            try
            {
                aRes = a();
            }
            catch (Exception ex)
            {
                aEx = ex;
            }

            Assert.AreEqual(oEx != null, aEx != null, $"{original.Body} != {alternative.Body}");

            if (oEx != null)
            {
                Assert.AreEqual(oEx.GetType(), aEx.GetType(), $"{original.Body} != {alternative.Body}");
            }
            else
            {
                Assert.AreEqual(oRes, aRes, $"{original.Body} != {alternative.Body}");
            }
        }

        private static ExpressionOptimizer GetPureBinaryOptimizer() => new(new PureBinarySemanticProvider(), GetEvaluatorFactory());

        private sealed class PureBinarySemanticProvider : DefaultSemanticProvider
        {
            public override bool IsPure(MemberInfo member)
            {
                return member.DeclaringType == typeof(decimal)
                    || member.DeclaringType == typeof(DateTimeOffset)
                    || member.DeclaringType == typeof(BigInteger)
                    || member.DeclaringType == typeof(BinaryOps)
                    || member.DeclaringType == typeof(BinaryLogicalOps)
                    || member.DeclaringType == typeof(BinaryLogicalOpsStruct);
            }

            public override bool IsImmutable(Type type)
            {
                return base.IsImmutable(type)
                    || type.GetNonNullableType() == typeof(DateTimeOffset)
                    || type.GetNonNullableType() == typeof(BigInteger)
                    || type.GetNonNullableType() == typeof(BinaryOps)
                    || type.GetNonNullableType() == typeof(BinaryLogicalOps)
                    || type.GetNonNullableType() == typeof(BinaryLogicalOpsStruct);
            }
        }

#pragma warning disable IDE0060 // Remove unused parameter (https://github.com/dotnet/roslyn/issues/32852)
        private struct BinaryOps
        {
            public static int operator +(BinaryOps b1, BinaryOps b2) => 42;
            public static int operator +(BinaryOps? b1, BinaryOps? b2) => b1 == null ? 1 : (b2 == null ? 2 : 42);
        }

        private class BinaryLogicalOps
        {
            public static readonly object Lock = new();
            public static Func<bool> True;
            public static Func<bool> False;
            public static Func<BinaryLogicalOps> And;
            public static Func<BinaryLogicalOps> Or;

            public static bool operator true(BinaryLogicalOps b) => True();
            public static bool operator false(BinaryLogicalOps b) => False();

            public static BinaryLogicalOps operator &(BinaryLogicalOps b1, BinaryLogicalOps b2) => And();
            public static BinaryLogicalOps operator |(BinaryLogicalOps b1, BinaryLogicalOps b2) => Or();
        }

        private struct BinaryLogicalOpsStruct
        {
            public static readonly object Lock = new();
            public static Func<bool> True;
            public static Func<bool> False;
            public static Func<BinaryLogicalOpsStruct> And;
            public static Func<BinaryLogicalOpsStruct> Or;

            public static bool operator true(BinaryLogicalOpsStruct b) => True();
            public static bool operator false(BinaryLogicalOpsStruct b) => False();

            public static BinaryLogicalOpsStruct operator &(BinaryLogicalOpsStruct b1, BinaryLogicalOpsStruct b2) => And();
            public static BinaryLogicalOpsStruct operator |(BinaryLogicalOpsStruct b1, BinaryLogicalOpsStruct b2) => Or();
        }
#pragma warning restore IDE0060 // Remove unused parameter
    }
}
