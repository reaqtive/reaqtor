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
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Tests.System.Linq.Expressions.Optimizers
{
    public partial class ExpressionOptimizerTests
    {
        [TestMethod]
        public void Index_Instance_Throw1()
        {
            var i = typeof(PureIndexers).GetProperty("Item", new[] { typeof(int), typeof(int), typeof(int) });

            var x = Expression.Parameter(typeof(int));
            var ex = Expression.Parameter(typeof(Exception));
            var t = Expression.Throw(ex, typeof(PureIndexers));

            var e = Expression.MakeIndex(t, i, new Expression[] { x, Expression.Constant(1), Expression.Constant(2) });

            var r = Expression.Throw(ex, typeof(long));

            AssertOptimized(
                GetPureIndexerOptimizer(),
                e,
                r
            );
        }

        [TestMethod]
        public void Index_Instance_Throw2()
        {
            var i = typeof(PureIndexers).GetProperty("Item", new[] { typeof(int), typeof(int), typeof(int) });

            var o = Expression.Constant(new PureIndexers(1L));
            var x = Expression.Parameter(typeof(int));
            var ex = Expression.Parameter(typeof(Exception));
            var t = Expression.Throw(ex, typeof(int));

            var e = Expression.MakeIndex(o, i, new Expression[] { x, Expression.Constant(1), t });

            var r = Expression.Throw(ex, typeof(long));

            AssertOptimized(
                GetPureIndexerOptimizer(),
                e,
                r
            );
        }

        [TestMethod]
        public void Index_Instance_Eval()
        {
            var i = typeof(PureIndexers).GetProperty("Item", new[] { typeof(int), typeof(int), typeof(int) });

            var o = Expression.Constant(new PureIndexers(1L));
            var e = Expression.MakeIndex(o, i, new Expression[] { Expression.Constant(7), Expression.Constant(8), Expression.Constant(15) });

            var r = Expression.Constant(42L, typeof(long));

            AssertOptimized(
                GetPureIndexerOptimizer(),
                e,
                r
            );
        }

        [TestMethod]
        public void Index_Instance_NoEval()
        {
            var i = typeof(PureIndexers).GetProperty("Item", new[] { typeof(int) });

            var o = Expression.Constant(new PureIndexers(1L));
            var e = Expression.MakeIndex(o, i, new Expression[] { Expression.Constant(7) });

            var r = e;

            AssertOptimized(
                GetPureIndexerOptimizer(),
                e,
                r
            );
        }

        [TestMethod]
        public void Index_Instance_Null1()
        {
            var e =
                Expression.MakeIndex(
                    Expression.Constant(value: null, typeof(List<int>)),
                    typeof(List<int>).GetProperty("Item", new[] { typeof(int) }),
                    new Expression[]
                    {
                        Expression.Constant(1)
                    }
                );

            AssertThrows(e, typeof(NullReferenceException));
        }

        [TestMethod]
        public void Index_Instance_Null2()
        {
            var e =
                Expression.MakeIndex(
                    Expression.Constant(value: null, typeof(List<int>)),
                    typeof(List<int>).GetProperty("Item", new[] { typeof(int) }),
                    new Expression[]
                    {
                        Expression.Parameter(typeof(int))
                    }
                );

            AssertThrows(e, typeof(NullReferenceException));
        }

        [TestMethod]
        public void Index_Instance_Null3()
        {
            var e =
                Expression.MakeIndex(
                    Expression.Constant(value: null, typeof(List<int>)),
                    typeof(List<int>).GetProperty("Item", new[] { typeof(int) }),
                    new Expression[]
                    {
                        F
                    }
                );

            AssertOptimized(e, e);
        }

        [TestMethod]
        public void Index_Array1()
        {
            var e =
                Expression.ArrayAccess(
                    Expression.Constant(new[] { 1, 2, 3 }),
                    Expression.Constant(1)
                );

            var r =
                Expression.Constant(2);

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Index_Array2()
        {
            var e =
                Expression.ArrayAccess(
                    Expression.Constant(new int[3, 3] { { 1, 2, 3 }, { 4, 5, 6 }, { 7, 8, 9 } }),
                    Expression.Constant(1),
                    Expression.Constant(2)
                );

            var r =
                Expression.Constant(6);

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Index_Array_OutOfRange1()
        {
            var e =
                Expression.ArrayAccess(
                    Expression.Constant(new[] { 1, 2, 3 }),
                    Expression.Constant(3)
                );

            AssertThrows(e, typeof(IndexOutOfRangeException));
        }

        [TestMethod]
        public void Index_Array_OutOfRange2()
        {
            var e =
                Expression.ArrayAccess(
                    Expression.Constant(new int[3, 3] { { 1, 2, 3 }, { 4, 5, 6 }, { 7, 8, 9 } }),
                    Expression.Constant(3),
                    Expression.Constant(2)
                );

            AssertThrows(e, typeof(IndexOutOfRangeException));
        }

        private static ExpressionOptimizer GetPureIndexerOptimizer() => new(new PureIndexerSemanticProvider(), GetEvaluatorFactory());

        private sealed class PureIndexerSemanticProvider : DefaultSemanticProvider
        {
            public override bool IsPure(MemberInfo member)
            {
                return member == typeof(PureIndexers).GetProperty("Item", new[] { typeof(int), typeof(int), typeof(int) });
            }
        }

        private sealed class PureIndexers
        {
            private readonly long _seed;

            public PureIndexers(long seed) => _seed = seed;

            public long this[int a] => _seed + a;
            public long this[int a, int b, int c] => _seed + a * b - c;
        }
    }
}
