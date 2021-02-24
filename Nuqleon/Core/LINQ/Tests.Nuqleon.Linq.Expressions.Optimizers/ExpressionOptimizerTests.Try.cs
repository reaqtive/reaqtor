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

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.System.Linq.Expressions.Optimizers
{
    public partial class ExpressionOptimizerTests
    {
        [TestMethod]
        public void TryCatch_Pure()
        {
            var x = Expression.Parameter(typeof(int));

            var e =
                Expression.TryCatch(
                    x,
                    Expression.Catch(typeof(Exception), F)
                );

            var r =
                x;

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void TryCatch_Constant1()
        {
            var e =
                Expression.TryCatch(
                    Expression.Constant(1),
                    Expression.Catch(typeof(Exception), F)
                );

            var r =
                Expression.Constant(1);

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void TryCatch_Constant2()
        {
            var e =
                Expression.MakeTry(
                    typeof(void),
                    Expression.Constant(1),
                    null,
                    null,
                    new[]
                    {
                        Expression.Catch(typeof(Exception), F)
                    }
                );

            var r =
                Expression.Empty();

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void TryFault_Constant()
        {
            var e =
                Expression.TryFault(
                    Expression.Constant(1),
                    V
                );

            var r =
                Expression.Constant(1);

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void TryFinally_Constant()
        {
            var e =
                Expression.TryFinally(
                    Expression.Constant(1),
                    V
                );

            var r =
                e;

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void TryCatchFinally_Constant()
        {
            var e =
                Expression.TryCatchFinally(
                    Expression.Constant(1),
                    V,
                    Expression.Catch(typeof(Exception), F)
                );

            var r =
                Expression.TryFinally(
                    Expression.Constant(1),
                    V
                );

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Try_NestedBehavior()
        {
            var e =
                Expression.TryCatch(
                    Expression.TryCatch(
                        Expression.Call(
                            typeof(Throwers).GetMethod(nameof(Throwers.ThrowInvalidOperationException))
                        ),
                        Expression.Catch(
                            typeof(InvalidOperationException),
                            Expression.Call(
                                typeof(Throwers).GetMethod(nameof(Throwers.ThrowArithmeticException))
                            )
                        )
                    ),
                    Expression.Catch(typeof(ArithmeticException), V)
                );

            AssertEvalEquals(e);
        }

        [TestMethod]
        public void Try_Simplify_CatchTrue()
        {
            var e =
                Expression.TryCatch(
                    V,
                    Expression.Catch(typeof(InvalidOperationException), V1, Expression.Constant(true))
                );

            var r =
                Expression.TryCatch(
                    V,
                    Expression.Catch(typeof(InvalidOperationException), V1)
                );

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Try_Simplify_CatchFalse1()
        {
            var e =
                Expression.TryCatch(
                    V,
                    Expression.Catch(typeof(FormatException), V1),
                    Expression.Catch(typeof(InvalidOperationException), V2, Expression.Constant(false)),
                    Expression.Catch(typeof(ArithmeticException), V3, B)
                );

            var r =
                Expression.TryCatch(
                    V,
                    Expression.Catch(typeof(FormatException), V1),
                    Expression.Catch(typeof(ArithmeticException), V3, B)
                );

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Try_Simplify_CatchFalse2()
        {
            var e =
                Expression.TryCatch(
                    V,
                    Expression.Catch(typeof(InvalidOperationException), V2, Expression.Constant(false))
                );

            var r =
                V;

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Try_Simplify_CatchAndRethrow1()
        {
            var e =
                Expression.TryCatch(
                    V,
                    Expression.Catch(typeof(FormatException), V1),
                    Expression.Catch(typeof(InvalidOperationException), Expression.Rethrow()), // REVIEW: Stack trace behavior.
                    Expression.Catch(typeof(ArithmeticException), V3, B)
                );

            var r =
                Expression.TryCatch(
                    V,
                    Expression.Catch(typeof(FormatException), V1),
                    Expression.Catch(typeof(ArithmeticException), V3, B)
                );

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Try_Simplify_CatchAndRethrow2()
        {
            var e =
                Expression.TryCatch(
                    V,
                    Expression.Catch(typeof(InvalidOperationException), Expression.Rethrow()) // REVIEW: Stack trace behavior.
                );

            var r =
                V;

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Try_Simplify_CatchAndRethrow3()
        {
            var ex = Expression.Parameter(typeof(InvalidOperationException));

            var e =
                Expression.TryCatch(
                    V,
                    Expression.Catch(ex, Expression.Throw(ex)) // NB: Resets stack traces etc, so keeping it.
                );

            var r =
                e;

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Try_Simplify_CatchVariable1()
        {
            var ex1 = Expression.Parameter(typeof(FormatException));

            var e =
                Expression.TryCatch(
                    V,
                    Expression.Catch(ex1, V1)
                );

            var r =
                Expression.TryCatch(
                    V,
                    Expression.Catch(typeof(FormatException), V1)
                );

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Try_Simplify_CatchVariable2()
        {
            var ex1 = Expression.Parameter(typeof(FormatException));

            var e =
                Expression.TryCatch(
                    V,
                    Expression.Catch(ex1, WriteLine(ex1, typeof(object)))
                );

            var r =
                e;

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Try_Simplify_EmptyFault()
        {
            var e =
                Expression.TryFault(
                    V,
                    Expression.Empty()
                );

            var r =
                V;

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Try_Simplify_EmptyFinally()
        {
            var e =
                Expression.TryFinally(
                    V,
                    Expression.Empty()
                );

            var r =
                V;

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Try_Simplify_DontTouchEmptyTry()
        {
            var e =
                Expression.TryFinally(
                    Expression.Empty(),
                    V
                );

            var r =
                e;

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Try_Throw1()
        {
            var t = Expression.Throw(Expression.New(typeof(E1).GetConstructor(Type.EmptyTypes)));

            var e =
                Expression.TryCatch(
                    t,
                    Expression.Catch(typeof(E1), V1),
                    Expression.Catch(typeof(FormatException), V2),
                    Expression.Catch(typeof(Exception), V3)
                );

            var r =
                e;

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Try_Throw2()
        {
            var ex1 = Expression.Parameter(typeof(E1));
            var t = Expression.Throw(Expression.New(typeof(E1).GetConstructor(Type.EmptyTypes)));

            var e =
                Expression.TryCatch(
                    t,
                    Expression.Catch(ex1, WriteLine(ex1, typeof(object))),
                    Expression.Catch(typeof(FormatException), V2),
                    Expression.Catch(typeof(Exception), V3)
                );

            var r =
                e;

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Try_Throw3()
        {
            var t = Expression.Throw(Expression.New(typeof(E1).GetConstructor(Type.EmptyTypes)));

            var e =
                Expression.TryCatch(
                    t,
                    Expression.Catch(typeof(E1), V1),
                    Expression.Catch(typeof(FormatException), V2),
                    Expression.Catch(typeof(Exception), V3)
                );

            var r =
                Expression.TryCatch(
                    V1,
                    Expression.Catch(typeof(FormatException), V2),
                    Expression.Catch(typeof(Exception), V3)
                );

            AssertOptimized(GetThrowOptimizer(), e, r);
        }

        [TestMethod]
        public void Try_Throw4()
        {
            var ex1 = Expression.Parameter(typeof(E1));
            var t = Expression.Throw(Expression.New(typeof(E1).GetConstructor(Type.EmptyTypes)));

            var e =
                Expression.TryCatch(
                    t,
                    Expression.Catch(ex1, WriteLine(ex1, typeof(object))),
                    Expression.Catch(typeof(FormatException), V2),
                    Expression.Catch(typeof(Exception), V3)
                );

            var r =
                Expression.TryCatch(
                    Expression.Block(
                        new[] { ex1 },
                        Expression.Assign(ex1, t.Operand),
                        WriteLine(ex1, typeof(object))
                    ),
                    Expression.Catch(typeof(FormatException), V2),
                    Expression.Catch(typeof(Exception), V3)
                );

            AssertOptimized(GetThrowOptimizer(), e, r);
        }

        [TestMethod]
        public void Try_Throw5()
        {
            var t = Expression.Throw(Expression.New(typeof(E1).GetConstructor(Type.EmptyTypes)));

            var e =
                Expression.TryFinally(
                    t,
                    V
                );

            var r =
                e;

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Try_Throw6()
        {
            var ex1 = Expression.Parameter(typeof(E1));
            var t = Expression.Throw(Expression.New(typeof(E1).GetConstructor(Type.EmptyTypes)));

            var e =
                Expression.TryCatch(
                    t,
                    Expression.Catch(ex1, WriteLine(ex1, typeof(object)), B),
                    Expression.Catch(typeof(FormatException), V2),
                    Expression.Catch(typeof(Exception), V3)
                );

            var r =
                e;

            AssertOptimized(GetThrowOptimizer(), e, r);
        }

        [TestMethod]
        public void Try_Throw7()
        {
            var ex1 = Expression.Parameter(typeof(E1));
            var t = Expression.Throw(Expression.New(typeof(E1).GetConstructor(Type.EmptyTypes)));

            var e =
                Expression.TryCatch(
                    t,
                    Expression.Catch(typeof(FormatException), V2),
                    Expression.Catch(ex1, WriteLine(ex1, typeof(object))),
                    Expression.Catch(typeof(Exception), V3)
                );

            var r =
                Expression.TryCatch(
                    Expression.Block(
                        new[] { ex1 },
                        Expression.Assign(ex1, t.Operand),
                        WriteLine(ex1, typeof(object))
                    ),
                    Expression.Catch(typeof(Exception), V3)
                );

            AssertOptimized(GetThrowOptimizer(), e, r);
        }

        [TestMethod]
        public void Try_Throw8()
        {
            var t = Expression.Throw(Expression.New(typeof(E1).GetConstructor(Type.EmptyTypes)));

            var e =
                Expression.TryCatch(
                    t,
                    Expression.Catch(typeof(DivideByZeroException), V1),
                    Expression.Catch(typeof(FormatException), V2),
                    Expression.Catch(typeof(InvalidOperationException), V3)
                );

            var r =
                e;

            AssertOptimized(GetThrowOptimizer(), e, r);
        }

        [TestMethod]
        public void Try_Throw9()
        {
            var t1 = Expression.Throw(Expression.New(typeof(E1).GetConstructor(Type.EmptyTypes)));
            var t2 = Expression.Throw(Expression.New(typeof(E2).GetConstructor(Type.EmptyTypes)));

            var e =
                Expression.TryCatch(
                    t1,
                    Expression.Catch(typeof(E1), t2),
                    Expression.Catch(typeof(FormatException), V2),
                    Expression.Catch(typeof(E2), V3)
                );

            var r =
                V3;

            AssertOptimized(GetThrowOptimizer(), e, r);
        }

        [TestMethod]
        public void Try_Throw10()
        {
            var t = Expression.Throw(Expression.Constant(new E1()));

            var e =
                Expression.TryCatch(
                    t,
                    Expression.Catch(typeof(E1), V)
                );

            var r =
                V;

            AssertOptimized(e, r);
        }

        private static ExpressionOptimizer GetThrowOptimizer() => new(new ThrowSemanticProvider(), GetEvaluatorFactory());

        private sealed class ThrowSemanticProvider : DefaultSemanticProvider
        {
            public override bool NeverThrows(Expression expression)
            {
                return expression is NewExpression @new
                    && (@new.Constructor == typeof(E1).GetConstructor(Type.EmptyTypes)
                        || @new.Constructor == typeof(E2).GetConstructor(Type.EmptyTypes));
            }
        }

        private sealed class E1 : Exception
        {
        }

        private sealed class E2 : Exception
        {
        }

        private static class Throwers
        {
            public static void ThrowInvalidOperationException() => throw new InvalidOperationException();

            public static void ThrowArithmeticException() => throw new ArithmeticException();
        }
    }
}
