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
using System.Linq.CompilerServices;
using System.Reflection;

namespace Tests.System.Linq.Expressions.Optimizers
{
    [TestClass]
    public partial class ExpressionOptimizerTests
    {
        private static readonly ISemanticProvider s_defaultSemanticProvider = new DefaultSemanticProvider();
        private static readonly IEvaluatorFactory s_defaultEvaluatorFactory = new DefaultEvaluatorFactory();

        private static readonly Expression B = Expression.Call(typeof(ExpressionOptimizerTests).GetMethod(nameof(MB)));
        private static readonly Expression B1 = Expression.Call(typeof(ExpressionOptimizerTests).GetMethod(nameof(MB1)));
        private static readonly Expression B2 = Expression.Call(typeof(ExpressionOptimizerTests).GetMethod(nameof(MB2)));
        private static readonly Expression NB = Expression.Call(typeof(ExpressionOptimizerTests).GetMethod(nameof(MNB)));
        private static readonly Expression NB1 = Expression.Call(typeof(ExpressionOptimizerTests).GetMethod(nameof(MNB1)));
        private static readonly Expression NB2 = Expression.Call(typeof(ExpressionOptimizerTests).GetMethod(nameof(MNB2)));
        private static readonly Expression D = Expression.Call(typeof(ExpressionOptimizerTests).GetMethod(nameof(MD)));
        private static readonly Expression D1 = Expression.Call(typeof(ExpressionOptimizerTests).GetMethod(nameof(MD1)));
        private static readonly Expression D2 = Expression.Call(typeof(ExpressionOptimizerTests).GetMethod(nameof(MD2)));
        private static readonly Expression ND = Expression.Call(typeof(ExpressionOptimizerTests).GetMethod(nameof(NMD)));
        private static readonly Expression ND1 = Expression.Call(typeof(ExpressionOptimizerTests).GetMethod(nameof(NMD1)));
        private static readonly Expression ND2 = Expression.Call(typeof(ExpressionOptimizerTests).GetMethod(nameof(NMD2)));
        private static readonly Expression F = Expression.Call(typeof(ExpressionOptimizerTests).GetMethod(nameof(MF)));
        private static readonly Expression G = Expression.Call(typeof(ExpressionOptimizerTests).GetMethod(nameof(MG)));
        private static readonly Expression S = Expression.Call(typeof(ExpressionOptimizerTests).GetMethod(nameof(MS)));
        private static readonly Expression V = Expression.Call(typeof(ExpressionOptimizerTests).GetMethod(nameof(MV)));
        private static readonly Expression V1 = Expression.Call(typeof(ExpressionOptimizerTests).GetMethod(nameof(MV1)));
        private static readonly Expression V2 = Expression.Call(typeof(ExpressionOptimizerTests).GetMethod(nameof(MV2)));
        private static readonly Expression V3 = Expression.Call(typeof(ExpressionOptimizerTests).GetMethod(nameof(MV3)));

        private static Expression WriteLine(Expression e) => WriteLine(e, e.Type);
        private static Expression WriteLine(Expression e, Type type) => Expression.Call(typeof(Console).GetMethod(nameof(WriteLine), new[] { type }), e);

        internal static void AssertOptimized(Expression original, Expression expected)
        {
            AssertOptimized(GetOptimizer(), original, expected);
        }

        internal static void AssertOptimized(ExpressionOptimizer optimizer, Expression original, Expression expected)
        {
            var res = optimizer.Visit(original);
            Assert.IsTrue(new ExpressionEqualityComparer(() => new ExpressionComparator()).Equals(res, expected), $"{res} != ${expected}");
        }

        internal static void AssertThrows(Expression original, Type exceptionType)
        {
            AssertThrows(GetOptimizer(), original, exceptionType);
        }

        internal static void AssertThrows(ExpressionOptimizer optimizer, Expression original, Type exceptionType)
        {
            var res = optimizer.Visit(original);
            Assert.AreEqual(ExpressionType.Throw, res.NodeType);
            Assert.AreEqual(exceptionType, ((UnaryExpression)res).Operand.Type);
        }

        internal static void AssertEval(Expression original)
        {
            AssertEval(GetOptimizer(), original);
        }

        internal static void AssertEvalEquals(Expression original)
        {
            AssertEvalEquals(GetOptimizer(), original);
        }

        private static ExpressionOptimizer GetOptimizer() => new(s_defaultSemanticProvider, s_defaultEvaluatorFactory);

        internal static void AssertEval(ExpressionOptimizer optimizer, Expression original)
        {
            var res = optimizer.Visit(original);

            var val = default(object);
            try
            {
                val = Expression.Lambda(original).Compile().DynamicInvoke();
            }
            catch (TargetInvocationException tie)
            {
                Assert.AreEqual(ExpressionType.Throw, res.NodeType);

                var @throw = (UnaryExpression)res;
                Assert.IsNotNull(@throw.Operand);

                Assert.AreEqual(tie.InnerException.GetType(), @throw.Operand.Type);

                return;
            }
            catch (Exception ex) when (Type.GetType("Mono.Runtime") != null) // NB: No TargetInvocationException on Mono when using DynamicInvoke.
            {
                Assert.AreEqual(ExpressionType.Throw, res.NodeType);

                var @throw = (UnaryExpression)res;
                Assert.IsNotNull(@throw.Operand);

                Assert.AreEqual(ex.GetType(), @throw.Operand.Type);

                return;
            }

            var expected = Expression.Constant(val, original.Type);

            Assert.IsTrue(new ExpressionEqualityComparer(() => new ExpressionComparator()).Equals(res, expected), $"{res} != ${expected}");
        }

        internal static void AssertEvalEquals(ExpressionOptimizer optimizer, Expression original)
        {
            Eval(original, out var valOriginal, out var excOriginal);

            var optimized = optimizer.Visit(original);

            Eval(optimized, out var valOptimized, out var excOptimized);

            if (excOriginal != null || excOptimized != null)
            {
                Assert.AreEqual(excOriginal?.GetType(), excOptimized?.GetType());
            }
            else if (original.Type != typeof(void))
            {
                Assert.AreEqual(valOriginal, valOptimized);
            }
        }

        private static void Eval(Expression expression, out object value, out Exception exception)
        {
            value = null;
            exception = null;

            try
            {
                value = Expression.Lambda(expression).Compile().DynamicInvoke();
            }
            catch (TargetInvocationException tie)
            {
                exception = tie.InnerException;
            }
        }

        internal static Expression NonConstant<T>()
        {
            return Expression.Parameter(typeof(T));
        }

        private sealed class ExpressionComparator : ExpressionEqualityComparator
        {
            public override bool Equals(Expression x, Expression y)
            {
                // REVIEW: Should we handle the introduction of Default nodes elsewhere?

                if (x != null && y != null && x.Type == y.Type && (x.Type.IsNullableType() || x.Type.IsClass) && IsNull(x) && IsNull(y))
                {
                    return true;
                }

                return base.Equals(x, y);
            }

            private static bool IsNull(Expression e)
            {
                if (e.NodeType == ExpressionType.Default)
                    return true;

                if (e.NodeType == ExpressionType.Constant)
                    return ((ConstantExpression)e).Value == null;

                return false;
            }

            protected override bool EqualsUnary(UnaryExpression x, UnaryExpression y)
            {
                if (x.NodeType == ExpressionType.Throw && y.NodeType == ExpressionType.Throw)
                {
                    if (x.Operand is ConstantExpression xOperand && y.Operand is ConstantExpression yOperand)
                    {
                        return xOperand.Type == yOperand.Type;
                    }
                }

                return base.EqualsUnary(x, y);
            }
        }

        private static IEvaluatorFactory GetEvaluatorFactory() => s_defaultEvaluatorFactory;

        public static bool MB() => false;
        public static bool MB1() => false;
        public static bool MB2() => false;
        public static bool? MNB() => false;
        public static bool? MNB1() => false;
        public static bool? MNB2() => false;
        public static decimal MD() => 0.0m;
        public static decimal MD1() => 0.0m;
        public static decimal MD2() => 0.0m;
        public static decimal? NMD() => 0.0m;
        public static decimal? NMD1() => 0.0m;
        public static decimal? NMD2() => 0.0m;
        public static int MF() => 0;
        public static int MG() => 0;
        public static string MS() => "";
        public static void MV() { }
        public static void MV1() { }
        public static void MV2() { }
        public static void MV3() { }
    }
}
