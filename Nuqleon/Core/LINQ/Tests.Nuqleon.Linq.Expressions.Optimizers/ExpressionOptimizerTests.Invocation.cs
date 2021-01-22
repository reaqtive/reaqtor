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
using System.Reflection;

namespace Tests.System.Linq.Expressions.Optimizers
{
    public partial class ExpressionOptimizerTests
    {
        [TestMethod]
        public void Invocation_Throw1()
        {
            var ex = Expression.Parameter(typeof(Exception));
            var t = Expression.Throw(ex, typeof(Func<int, int>));

            var e = Expression.Invoke(t, Expression.Constant(1));

            var r = Expression.Throw(ex, typeof(int));

            AssertOptimized(
                e,
                r
            );
        }

        [TestMethod]
        public void Invocation_Throw2()
        {
            var ex = Expression.Parameter(typeof(Exception));
            var t = Expression.Throw(ex, typeof(int));

            var f = Expression.Constant(new Func<int, int>(x => x * 2));
            var e = Expression.Invoke(f, t);

            var r = Expression.Throw(ex, typeof(int));

            AssertOptimized(
                e,
                r
            );
        }

        [TestMethod]
        public void Invocation_Identity()
        {
            var x = Expression.Parameter(typeof(int));
            var f = Expression.Lambda(x, x);
            var e = Expression.Invoke(f, Expression.Constant(1));

            var r = Expression.Constant(1);

            AssertOptimized(
                e,
                r
            );
        }

        [TestMethod]
        public void Invocation_NoIdentity()
        {
            var x = Expression.Parameter(typeof(int));
            var f = Expression.Lambda(Expression.Multiply(x, Expression.Constant(2)), x);
            var e = Expression.Invoke(f, Expression.Constant(1));

            var r = Expression.Constant(2);

            AssertOptimized(
                e,
                r
            );
        }

        [TestMethod]
        public void Invocation_NoOptimization()
        {
            var x = Expression.Parameter(typeof(int));
            var y = Expression.Parameter(typeof(int));
            var f = Expression.Lambda(Expression.Add(x, y), x, y);
            var e = Expression.Invoke(f, F, G);

            var r = e;

            AssertOptimized(
                e,
                r
            );
        }

        [TestMethod]
        public void Invocation_InvokeLambda1()
        {
            var x = Expression.Parameter(typeof(int));
            var y = Expression.Parameter(typeof(int));
            var f = Expression.Lambda(x, x, y);
            var e = Expression.Invoke(f, Expression.Constant(1), Expression.Constant(2));

            var r = Expression.Constant(1);

            AssertOptimized(
                e,
                r
            );
        }

        [TestMethod]
        public void Invocation_InvokeLambda2()
        {
            var x = Expression.Parameter(typeof(int));
            var y = Expression.Parameter(typeof(int));
            var f = Expression.Lambda(y, x, y);
            var e = Expression.Invoke(f, Expression.Constant(1), Expression.Constant(2));

            var r = Expression.Constant(2);

            AssertOptimized(
                e,
                r
            );
        }

        [TestMethod]
        public void Invocation_InvokeLambda3()
        {
            var x = Expression.Parameter(typeof(int));
            var y = Expression.Parameter(typeof(int));
            var z = Expression.Parameter(typeof(int));
            var f = Expression.Lambda(z, x, y);
            var e = Expression.Invoke(f, Expression.Constant(1), Expression.Constant(2));

            var r = z;

            AssertOptimized(
                e,
                r
            );
        }

        [TestMethod]
        public void Invocation_InvokeLambda4()
        {
            var x = Expression.Parameter(typeof(int));
            var y = Expression.Parameter(typeof(int));
            var z = Expression.Parameter(typeof(int));
            var f = Expression.Lambda(z, x, y);
            var e = Expression.Invoke(f, F, G);

            AssertOptimized(
                e,
                e
            );
        }

        [TestMethod]
        public void Invocation_InvokeLambda5()
        {
            var x = Expression.Parameter(typeof(int));
            var y = Expression.Parameter(typeof(int));
            var f = Expression.Lambda(Expression.Add(x, y), x, y);
            var e = Expression.Invoke(f, Expression.Constant(1), Expression.Constant(2));

            var r = Expression.Constant(3);

            AssertOptimized(
                e,
                r
            );
        }

        [TestMethod]
        public void Invocation_InvokeLambda_Throw1()
        {
            var ex = Expression.Parameter(typeof(Exception));
            var t = Expression.Throw(ex, typeof(int));

            var x = Expression.Parameter(typeof(int));
            var y = Expression.Parameter(typeof(int));
            var f = Expression.Lambda(y, x, y);
            var e = Expression.Invoke(f, Expression.Constant(1), t);

            var r = Expression.Throw(ex, typeof(int));

            AssertOptimized(
                e,
                r
            );
        }

        [TestMethod]
        public void Invocation_InvokeLambda_Throw2()
        {
            var ex = Expression.Parameter(typeof(Exception));
            var t = Expression.Throw(ex, typeof(int));

            var x = Expression.Parameter(typeof(int));
            var y = Expression.Parameter(typeof(int));
            var f = Expression.Lambda(t, x, y);
            var e = Expression.Invoke(f, Expression.Constant(1), Expression.Constant(2));

            var r = Expression.Throw(ex, typeof(int));

            AssertOptimized(
                e,
                r
            );
        }

        [TestMethod]
        public void Invocation_InvokeDelegate_Eval()
        {
            var f = Expression.Constant(new Func<int, int, int, long>(PureInvocations.StaticMethod));
            var e = Expression.Invoke(f, Expression.Constant(1), Expression.Constant(2), Expression.Constant(3));

            var r = Expression.Constant(-1L);

            AssertOptimized(
                GetPureInvocationOptimizer(),
                e,
                r
            );
        }

        [TestMethod]
        public void Invocation_InvokeDelegate_NoEval()
        {
            var f = Expression.Constant(new Func<int, int, int, long>(PureInvocations.StaticMethodNotPure));
            var e = Expression.Invoke(f, Expression.Constant(1), Expression.Constant(2), Expression.Constant(3));

            var r = e;

            AssertOptimized(
                GetPureInvocationOptimizer(),
                e,
                r
            );
        }

        [TestMethod]
        public void Invocation_Null1()
        {
            var e =
                Expression.Invoke(
                    Expression.Constant(value: null, typeof(Func<int>))
                );

            AssertThrows(e, typeof(NullReferenceException));
        }

        [TestMethod]
        public void Invocation_Null2()
        {
            var e =
                Expression.Invoke(
                    Expression.Constant(value: null, typeof(Func<int, int>)),
                    Expression.Constant(1)
                );

            AssertThrows(e, typeof(NullReferenceException));
        }

        [TestMethod]
        public void Invocation_Null3()
        {
            var e =
                Expression.Invoke(
                    Expression.Constant(value: null, typeof(Func<int, int>)),
                    Expression.Parameter(typeof(int))
                );

            AssertThrows(e, typeof(NullReferenceException));
        }

        [TestMethod]
        public void Invocation_Null4()
        {
            var e =
                Expression.Invoke(
                    Expression.Constant(value: null, typeof(Func<int, int>)),
                    F
                );

            AssertOptimized(e, e);
        }

        private static ExpressionOptimizer GetPureInvocationOptimizer() => new(new PureInvocationSemanticProvider(), GetEvaluatorFactory());

        private sealed class PureInvocationSemanticProvider : DefaultSemanticProvider
        {
            public override bool IsPure(MemberInfo member)
            {
                return member == typeof(PureInvocations).GetMethod(nameof(PureInvocations.StaticMethod));
            }
        }

        private sealed class PureInvocations
        {
            public static long StaticMethod(int a, int b, int c) => a * b - c;
            public static long StaticMethodNotPure(int a, int b, int c) => a * b - c;
        }
    }
}
