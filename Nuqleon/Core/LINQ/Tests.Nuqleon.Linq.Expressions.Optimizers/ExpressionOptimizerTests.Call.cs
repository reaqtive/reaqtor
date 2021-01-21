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
        public void Call_Static_Throw1()
        {
            var m = typeof(PureCalls).GetMethod(nameof(PureCalls.StaticMethod));

            var x = Expression.Parameter(typeof(int));
            var ex = Expression.Parameter(typeof(Exception));
            var t = Expression.Throw(ex, typeof(int));

            var e = Expression.Call(m, t, x, Expression.Constant(1));

            var r = Expression.Throw(ex, typeof(long));

            AssertOptimized(
                GetPureCallOptimizer(),
                e,
                r
            );
        }

        [TestMethod]
        public void Call_Static_Throw2()
        {
            var m = typeof(PureCalls).GetMethod(nameof(PureCalls.StaticMethod));

            var x = Expression.Parameter(typeof(int));
            var ex = Expression.Parameter(typeof(Exception));
            var t = Expression.Throw(ex, typeof(int));

            var e = Expression.Call(m, x, Expression.Constant(1), t);

            var r = Expression.Throw(ex, typeof(long));

            AssertOptimized(
                GetPureCallOptimizer(),
                e,
                r
            );
        }

        [TestMethod]
        public void Call_Static_Eval()
        {
            var m = typeof(PureCalls).GetMethod(nameof(PureCalls.StaticMethod));

            var e = Expression.Call(m, Expression.Constant(7), Expression.Constant(8), Expression.Constant(14));

            var r = Expression.Constant(42L, typeof(long));

            AssertOptimized(
                GetPureCallOptimizer(),
                e,
                r
            );
        }

        [TestMethod]
        public void Call_Static_EvalVoid()
        {
            var m = typeof(PureCalls).GetMethod(nameof(PureCalls.StaticMethodVoid));

            var e = Expression.Call(m, Expression.Constant(1));

            var r = Expression.Empty();

            AssertOptimized(
                GetPureCallOptimizer(),
                e,
                r
            );
        }

        [TestMethod]
        public void Call_Static_NoEval()
        {
            var m = typeof(PureCalls).GetMethod(nameof(PureCalls.StaticMethodNotPure));

            var e = Expression.Call(m, Expression.Constant(7), Expression.Constant(8), Expression.Constant(14));

            var r = e;

            AssertOptimized(
                GetPureCallOptimizer(),
                e,
                r
            );
        }

        [TestMethod]
        public void Call_Instance_Throw1()
        {
            var m = typeof(PureCalls).GetMethod(nameof(PureCalls.InstanceMethod));

            var x = Expression.Parameter(typeof(int));
            var ex = Expression.Parameter(typeof(Exception));
            var t = Expression.Throw(ex, typeof(PureCalls));

            var e = Expression.Call(t, m, x, Expression.Constant(1), Expression.Constant(2));

            var r = Expression.Throw(ex, typeof(long));

            AssertOptimized(
                GetPureCallOptimizer(),
                e,
                r
            );
        }

        [TestMethod]
        public void Call_Instance_Throw2()
        {
            var m = typeof(PureCalls).GetMethod(nameof(PureCalls.InstanceMethod));

            var o = Expression.Constant(new PureCalls(1L));
            var x = Expression.Parameter(typeof(int));
            var ex = Expression.Parameter(typeof(Exception));
            var t = Expression.Throw(ex, typeof(int));

            var e = Expression.Call(o, m, x, Expression.Constant(1), t);

            var r = Expression.Throw(ex, typeof(long));

            AssertOptimized(
                GetPureCallOptimizer(),
                e,
                r
            );
        }

        [TestMethod]
        public void Call_Instance_Eval()
        {
            var m = typeof(PureCalls).GetMethod(nameof(PureCalls.InstanceMethod));

            var o = Expression.Constant(new PureCalls(1L));
            var e = Expression.Call(o, m, Expression.Constant(7), Expression.Constant(8), Expression.Constant(15));

            var r = Expression.Constant(42L, typeof(long));

            AssertOptimized(
                GetPureCallOptimizer(),
                e,
                r
            );
        }

        [TestMethod]
        public void Call_Instance_NoEval()
        {
            var m = typeof(PureCalls).GetMethod(nameof(PureCalls.InstanceMethodNotPure));

            var o = Expression.Constant(new PureCalls(1L));
            var e = Expression.Call(o, m, Expression.Constant(7), Expression.Constant(8), Expression.Constant(15));

            var r = e;

            AssertOptimized(
                GetPureCallOptimizer(),
                e,
                r
            );
        }

        [TestMethod]
        public void Call_Instance_Eval_Throw()
        {
            var m = typeof(PureCalls).GetMethod(nameof(PureCalls.StaticMethodThrow));

            var e = Expression.Call(m, Expression.Constant(1));

            var opt = GetPureCallOptimizer().Visit(e);

            Assert.AreEqual(ExpressionType.Throw, opt.NodeType);

            var ex = ((UnaryExpression)opt).Operand as ConstantExpression;

            Assert.IsNotNull(ex);

            var exVal = ex.Value as Exception;

            Assert.IsNotNull(exVal);
            Assert.AreEqual("Oops!", exVal.Message);
        }

        [TestMethod]
        public void Call_Instance_Null1()
        {
            var e =
                Expression.Call(
                    Expression.Constant(value: null, typeof(string)),
                    typeof(string).GetMethod(nameof(string.ToUpper), Type.EmptyTypes)
                );

            AssertThrows(e, typeof(NullReferenceException));
        }

        [TestMethod]
        public void Call_Instance_Null2()
        {
            var e =
                Expression.Call(
                    Expression.Constant(value: null, typeof(string)),
                    typeof(string).GetMethod(nameof(string.Substring), new[] { typeof(int), typeof(int) }),
                    Expression.Constant(1),
                    Expression.Parameter(typeof(int))
                );

            AssertThrows(e, typeof(NullReferenceException));
        }

        [TestMethod]
        public void Call_Instance_Null3()
        {
            var e =
                Expression.Call(
                    Expression.Constant(value: null, typeof(string)),
                    typeof(string).GetMethod(nameof(string.Substring), new[] { typeof(int), typeof(int) }),
                    F,
                    Expression.Constant(1)
                );

            AssertOptimized(e, e);
        }

        private static ExpressionOptimizer GetPureCallOptimizer() => new(new PureCallSemanticProvider(), GetEvaluatorFactory());

        private sealed class PureCallSemanticProvider : DefaultSemanticProvider
        {
            public override bool IsPure(MemberInfo member)
            {
                return member == typeof(PureCalls).GetMethod(nameof(PureCalls.StaticMethod))
                    || member == typeof(PureCalls).GetMethod(nameof(PureCalls.StaticMethodVoid))
                    || member == typeof(PureCalls).GetMethod(nameof(PureCalls.StaticMethodThrow))
                    || member == typeof(PureCalls).GetMethod(nameof(PureCalls.InstanceMethod));
            }
        }

#pragma warning disable IDE0060 // Remove unused parameter
        private sealed class PureCalls
        {
            private readonly long _seed;

            public PureCalls(long seed) => _seed = seed;

            public static long StaticMethod(int a, int b, int c) => a * b - c;
            public static void StaticMethodVoid(int a) { }
            public static int StaticMethodThrow(int a) { throw new Exception("Oops!"); }
            public static long StaticMethodNotPure(int a, int b, int c) => a * b - c;
            public long InstanceMethod(int a, int b, int c) => _seed + a * b - c;
            public long InstanceMethodNotPure(int a, int b, int c) => _seed + a * b - c;
        }
#pragma warning restore IDE0060 // Remove unused parameter
    }
}
