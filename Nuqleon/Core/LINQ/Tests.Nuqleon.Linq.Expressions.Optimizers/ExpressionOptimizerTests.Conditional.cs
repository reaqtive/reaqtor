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

namespace Tests.System.Linq.Expressions.Optimizers
{
    public partial class ExpressionOptimizerTests
    {
        [TestMethod]
        public void Conditional_Throw1()
        {
            var ex = Expression.Parameter(typeof(Exception));

            var e =
                Expression.Condition(
                    Expression.Throw(ex, typeof(bool)),
                    Expression.Constant(1),
                    Expression.Constant(2)
                );

            var r =
                Expression.Throw(ex, typeof(int));

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Conditional_Throw2()
        {
            var ex = Expression.Parameter(typeof(Exception));

            var e =
                Expression.IfThenElse(
                    Expression.Throw(ex, typeof(bool)),
                    Expression.Constant(1),
                    Expression.Constant(2)
                );

            var r =
                Expression.Throw(ex, typeof(void));

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Conditional_Throw3()
        {
            var ex = Expression.Parameter(typeof(Exception));

            var e =
                Expression.IfThen(
                    Expression.Throw(ex, typeof(bool)),
                    Expression.Constant(1)
                );

            var r =
                Expression.Throw(ex, typeof(void));

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Conditional_True()
        {
            var e =
                Expression.Condition(
                    Expression.Constant(true),
                    Expression.Constant(1),
                    Expression.Constant(2)
                );

            var r =
                Expression.Constant(1);

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Conditional_False()
        {
            var e =
                Expression.Condition(
                    Expression.Constant(false),
                    Expression.Constant(1),
                    Expression.Constant(2)
                );

            var r =
                Expression.Constant(2);

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Conditional_Flatten1()
        {
            var b1 = Expression.Parameter(typeof(bool));

            var e =
                Expression.Condition(
                    b1,
                    Expression.Constant(1),
                    Expression.Default(typeof(int))
                );

            var r =
                Expression.Condition(
                    b1,
                    Expression.Constant(1),
                    Expression.Default(typeof(int))
                );

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Conditional_Flatten2()
        {
            var b1 = Expression.Parameter(typeof(bool));
            var b2 = Expression.Parameter(typeof(bool));

            var e =
                Expression.Condition(
                    b1,
                    Expression.Condition(
                        b2,
                        Expression.Constant(1),
                        Expression.Default(typeof(int))
                    ),
                    Expression.Default(typeof(int))
                );

            var r =
                Expression.Condition(
                    Expression.AndAlso(b1, b2),
                    Expression.Constant(1),
                    Expression.Default(typeof(int))
                );

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Conditional_Flatten3()
        {
            var b1 = Expression.Parameter(typeof(bool));
            var b2 = Expression.Parameter(typeof(bool));
            var b3 = Expression.Parameter(typeof(bool));

            var e =
                Expression.Condition(
                    b1,
                    Expression.Condition(
                        b2,
                        Expression.Condition(
                            b3,
                            Expression.Constant(1),
                            Expression.Default(typeof(int))
                        ),
                        Expression.Default(typeof(int))
                    ),
                    Expression.Default(typeof(int))
                );

            var r =
                Expression.Condition(
                    Expression.AndAlso(b1, Expression.AndAlso(b2, b3)),
                    Expression.Constant(1),
                    Expression.Default(typeof(int))
                );

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Conditional_Flatten4()
        {
            var b1 = Expression.Parameter(typeof(bool));
            var b2 = Expression.Parameter(typeof(bool));
            var b3 = Expression.Parameter(typeof(bool));

            var e =
                Expression.Condition(
                    b1,
                    Expression.Condition(
                        b2,
                        Expression.Condition(
                            b3,
                            Expression.Constant(1),
                            Expression.Constant(-1)
                        ),
                        Expression.Constant(-2)
                    ),
                    Expression.Constant(-3)
                );

            var r =
                e;

            AssertOptimized(e, r);
        }
    }
}
