// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2017 - Created this file.
//

using System;
using System.Linq.Expressions;
using System.Reflection;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.System.Linq.Expressions.Optimizers
{
    public partial class ExpressionOptimizerTests
    {
        [TestMethod]
        public void New_Instance_Throw1()
        {
            var c = typeof(PureConstructors).GetConstructor(new[] { typeof(int), typeof(int), typeof(int) });

            var ex = Expression.Parameter(typeof(Exception));
            var t = Expression.Throw(ex, typeof(int));

            var e = Expression.New(c, t, Expression.Constant(1), Expression.Constant(2));

            var r = Expression.Throw(ex, typeof(PureConstructors));

            AssertOptimized(
                GetPureConstructorOptimizer(),
                e,
                r
            );
        }

        [TestMethod]
        public void New_Instance_Throw2()
        {
            var c = typeof(PureConstructors).GetConstructor(new[] { typeof(int), typeof(int), typeof(int) });

            var x = Expression.Parameter(typeof(int));
            var ex = Expression.Parameter(typeof(Exception));
            var t = Expression.Throw(ex, typeof(int));

            var e = Expression.New(c, x, Expression.Constant(1), t);

            var r = Expression.Throw(ex, typeof(PureConstructors));

            AssertOptimized(
                GetPureConstructorOptimizer(),
                e,
                r
            );
        }

        [TestMethod]
        public void New_Instance_Eval1()
        {
            var c = typeof(PureConstructors).GetConstructor(new[] { typeof(int), typeof(int), typeof(int) });

            var e = Expression.New(c, Expression.Constant(7), Expression.Constant(8), Expression.Constant(15));

            var r = Expression.Constant(new PureConstructors(7, 8, 15));

            AssertOptimized(
                GetPureConstructorOptimizer(),
                e,
                r
            );
        }

        [TestMethod]
        public void New_Instance_Eval2()
        {
            var e = Expression.New(typeof(TimeSpan));

            var r = Expression.Constant(TimeSpan.Zero);

            AssertOptimized(
                GetPureConstructorOptimizer(),
                e,
                r
            );
        }

        [TestMethod]
        public void New_Instance_NoEval1()
        {
            var c = typeof(PureConstructors).GetConstructor(new[] { typeof(int) });

            var e = Expression.New(c, Expression.Constant(7));

            var r = e;

            AssertOptimized(
                GetPureConstructorOptimizer(),
                e,
                r
            );
        }

        [TestMethod]
        public void New_Instance_NoEval2()
        {
            var e = Expression.New(typeof(DateTime));

            var r = e;

            AssertOptimized(
                GetPureConstructorOptimizer(),
                e,
                r
            );
        }

        private static ExpressionOptimizer GetPureConstructorOptimizer() => new(new PureConstructorSemanticProvider(), GetEvaluatorFactory());

        private sealed class PureConstructorSemanticProvider : DefaultSemanticProvider
        {
            public override bool IsPure(MemberInfo member)
            {
                return member == typeof(PureConstructors).GetConstructor(new[] { typeof(int), typeof(int), typeof(int) })
                    || member == typeof(TimeSpan);
            }

            public override bool IsImmutable(Type type)
            {
                return base.IsImmutable(type)
                    || type == typeof(PureConstructors)
                    || type == typeof(TimeSpan);
            }
        }

        private sealed class PureConstructors
        {
            private readonly int _x;
            private readonly int _y;
            private readonly int _z;

            public PureConstructors(int x)
            {
                _x = x;
            }

            public PureConstructors(int x, int y, int z)
            {
                _x = x;
                _y = y;
                _z = z;
            }

            public override bool Equals(object obj)
            {
                return obj is PureConstructors c && c._x == _x && c._y == _y && c._z == _z;
            }

            public override int GetHashCode() => 0;
        }
    }
}
