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
        public void Switch_Throw1()
        {
            var ex = Expression.Parameter(typeof(Exception));
            var t = Expression.Throw(ex, typeof(int));

            var e =
                Expression.Switch(
                    t,
                    F,
                    Expression.SwitchCase(G, Expression.Constant(1))
                );

            var r =
                Expression.Throw(ex, typeof(int));

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Switch_Throw2()
        {
            var ex = Expression.Parameter(typeof(Exception));
            var t = Expression.Throw(ex, typeof(int));

            var e =
                Expression.Switch(
                    typeof(void),
                    t,
                    F,
                    comparison: null,
                    Expression.SwitchCase(G, Expression.Constant(1))
                );

            var r =
                Expression.Throw(ex, typeof(void));

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Switch_Throw3()
        {
            var ex = Expression.Parameter(typeof(Exception));
            var t = Expression.Throw(ex, typeof(int));

            var e =
                Expression.Switch(
                    typeof(void),
                    Expression.Constant(1),
                    F,
                    comparison: null,
                    Expression.SwitchCase(G, t)
                );

            var r =
                Expression.Throw(ex, typeof(void));

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Switch_Eval1()
        {
            var e =
                Expression.Switch(
                    Expression.Constant(1),
                    Expression.Constant(-1),
                    Expression.SwitchCase(F, Expression.Constant(1), Expression.Constant(3)),
                    Expression.SwitchCase(G, Expression.Constant(2))
                );

            var r =
                F;

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Switch_Eval2()
        {
            var e =
                Expression.Switch(
                    Expression.Constant(3),
                    Expression.Constant(-1),
                    Expression.SwitchCase(F, Expression.Constant(1), Expression.Constant(3)),
                    Expression.SwitchCase(G, Expression.Constant(2))
                );

            var r =
                F;

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Switch_Eval3()
        {
            var e =
                Expression.Switch(
                    Expression.Constant(2),
                    Expression.Constant(-1),
                    Expression.SwitchCase(F, Expression.Constant(1), Expression.Constant(3)),
                    Expression.SwitchCase(G, Expression.Constant(2))
                );

            var r =
                G;

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Switch_Eval4()
        {
            var e =
                Expression.Switch(
                    Expression.Constant(7),
                    Expression.Constant(-1),
                    Expression.SwitchCase(F, Expression.Constant(1), Expression.Constant(3)),
                    Expression.SwitchCase(G, Expression.Constant(2))
                );

            var r =
                Expression.Constant(-1);

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Switch_Eval5()
        {
            var e =
                Expression.Switch(
                    Expression.Constant(3),
                    Expression.Constant(-1),
                    Expression.SwitchCase(F, Expression.Constant(1), Expression.Constant(3)),
                    Expression.SwitchCase(Expression.Constant(2), G) // NB: This is fine due to sequential evaluation.
                );

            var r =
                F;

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Switch_Eval_Pure1()
        {
            var e =
                Expression.Switch(
                    Expression.Constant(1),
                    Expression.Constant(-1),
                    typeof(ExpressionOptimizerTests).GetMethod(nameof(ComparisonPure)),
                    Expression.SwitchCase(F, Expression.Constant(2), Expression.Constant(4)),
                    Expression.SwitchCase(G, Expression.Constant(7))
                );

            var r =
                G;

            AssertOptimized(
                GetSwitchOptimizer(),
                e,
                r
            );
        }

        [TestMethod]
        public void Switch_NoEval1()
        {
            var e =
                Expression.Switch(
                    Expression.Constant(7),
                    Expression.Constant(-1),
                    Expression.SwitchCase(F, Expression.Constant(1), Expression.Constant(3)),
                    Expression.SwitchCase(Expression.Constant(2), G)
                );

            var r =
                e;

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Switch_NoEval2()
        {
            var e =
                Expression.Switch(
                    Expression.Parameter(typeof(int)),
                    Expression.Constant(-1),
                    Expression.SwitchCase(F, Expression.Constant(1)),
                    Expression.SwitchCase(G, Expression.Constant(2))
                );

            var r =
                e;

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Switch_NoEval3()
        {
            var e =
                Expression.Switch(
                    Expression.Constant(1),
                    Expression.Constant(-1),
                    typeof(ExpressionOptimizerTests).GetMethod(nameof(ComparisonNonPure)),
                    Expression.SwitchCase(F, Expression.Constant(1)),
                    Expression.SwitchCase(G, Expression.Constant(2))
                );

            var r =
                e;

            AssertOptimized(e, r);
        }

        private static ExpressionOptimizer GetSwitchOptimizer() => new(new SwitchSemanticProvider(), GetEvaluatorFactory());

        private sealed class SwitchSemanticProvider : DefaultSemanticProvider
        {
            public override bool IsPure(MemberInfo member)
            {
                return member == typeof(ExpressionOptimizerTests).GetMethod(nameof(ComparisonPure));
            }
        }

#pragma warning disable IDE0060 // Remove unused parameter
        public static bool ComparisonNonPure(int x, int y) => false;
        public static bool ComparisonPure(int x, int y) => x % 2 == y % 2;
#pragma warning restore IDE0060 // Remove unused parameter
    }
}
