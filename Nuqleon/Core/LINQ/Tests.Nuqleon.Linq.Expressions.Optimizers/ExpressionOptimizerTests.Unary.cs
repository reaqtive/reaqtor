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
        public void Unary_ArrayLength_Throw()
        {
            var ex = Expression.Parameter(typeof(Exception));
            var t = Expression.Throw(ex, typeof(int[]));

            var e =
                Expression.ArrayLength(
                    t
                );

            var r =
                Expression.Throw(ex, typeof(int));

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Unary_ArrayLength_Constant()
        {
            var e =
                Expression.ArrayLength(
                    Expression.Constant(new[] { 1, 2, 3 })
                );

            var r =
                Expression.Constant(3);

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Unary_ArrayLength_Null()
        {
            var e =
                Expression.ArrayLength(
                    Expression.Constant(value: null, typeof(int[]))
                );

            AssertThrows(e, typeof(NullReferenceException));
        }

        [TestMethod]
        public void Unary_ArrayLength_NewArrayBounds()
        {
            var e =
                Expression.ArrayLength(
                    Expression.NewArrayBounds(
                        typeof(int),
                        Expression.Constant(42)
                    )
                );

            var r =
                Expression.Constant(42);

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Unary_ArrayLength_NewArrayBounds_Nop()
        {
            var x = Expression.Parameter(typeof(int));

            var e =
                Expression.ArrayLength(
                    Expression.NewArrayBounds(
                        typeof(int),
                        x
                    )
                );

            var r =
                e;

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Unary_ArrayLength_NewArrayInit()
        {
            var c = Expression.Constant(1);
            var x = Expression.Parameter(typeof(int));

            var e =
                Expression.ArrayLength(
                    Expression.NewArrayInit(
                        typeof(int),
                        c,
                        x
                    )
                );

            var r =
                Expression.Constant(2);

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Unary_ArrayLength_NewArrayInit_Nop()
        {
            var c = Expression.Constant(1);
            var x = Expression.Parameter(typeof(int));

            var e =
                Expression.ArrayLength(
                    Expression.NewArrayInit(
                        typeof(int),
                        c,
                        F,
                        x
                    )
                );

            var r =
                e;

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Unary_ArrayLength_Nop()
        {
            var xs = Expression.Parameter(typeof(int[]));

            var e =
                Expression.ArrayLength(
                    xs
                );

            var r =
                e;

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Unary_Not_NoEval()
        {
            var e =
                Expression.Not(
                    Expression.Parameter(typeof(bool))
                );

            AssertOptimized(e, e);
        }

        [TestMethod]
        public void Unary_Negate_NoEval()
        {
            var e =
                Expression.Negate(
                    F
                );

            AssertOptimized(e, e);
        }

        [TestMethod]
        public void Unary_Convert_NoEval1()
        {
            var e =
                Expression.Convert(
                    Expression.Parameter(typeof(int)),
                    typeof(long)
                );

            AssertOptimized(e, e);
        }

        [TestMethod]
        public void Unary_Convert_NoEval2()
        {
            var e =
                Expression.Convert(
                    Expression.Constant(0),
                    typeof(IComparable)
                );

            AssertOptimized(e, e);
        }

        [TestMethod]
        public void Unary_Convert_Equivalent()
        {
            var e =
                Expression.Convert(
                    Expression.Constant(1),
                    typeof(int)
                );

            var r =
                Expression.Constant(1);

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Unary_Convert_Enum1()
        {
            var e =
                Expression.Convert(
                    Expression.Constant(ConsoleColor.Red),
                    typeof(int)
                );

            var r =
                Expression.Constant((int)ConsoleColor.Red);

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Unary_Convert_Enum2()
        {
            var e =
                Expression.Convert(
                    Expression.Constant((int)ConsoleColor.Red),
                    typeof(ConsoleColor)
                );

            var r =
                Expression.Constant(ConsoleColor.Red);

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Unary_TypeAs_Equivalent()
        {
            var e =
                Expression.TypeAs(
                    Expression.Constant("bar"),
                    typeof(string)
                );

            var r =
                Expression.Constant("bar");

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Unary_TypeAs_Null()
        {
            var e =
                Expression.TypeAs(
                    Expression.Constant(value: null, typeof(string)),
                    typeof(IComparable)
                );

            var r =
                Expression.Default(typeof(IComparable));

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Unary_TypeAs_NoMatch1()
        {
            var e =
                Expression.TypeAs(
                    Expression.Parameter(typeof(string)),
                    typeof(Uri)
                );

            var r =
                Expression.Default(typeof(Uri));

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Unary_TypeAs_NoMatch2()
        {
            var e =
                Expression.TypeAs(
                    Expression.Parameter(typeof(string)),
                    typeof(IAsyncResult)
                );

            var r =
                Expression.Default(typeof(IAsyncResult));

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Unary_TypeAs_Constant1()
        {
            var e =
                Expression.TypeAs(
                    Expression.Constant("bar", typeof(string)),
                    typeof(IComparable)
                );

            var r =
                Expression.Constant("bar", typeof(IComparable));

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Unary_TypeAs_Constant2()
        {
            var u = new Uri("http://bing.com");

            var e =
                Expression.TypeAs(
                    Expression.Constant(u, typeof(Uri)),
                    typeof(object)
                );

            var r =
                Expression.Constant(u, typeof(object));

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Unary_TypeAs_Constant3()
        {
            var u = new Uri("http://bing.com");

            var e =
                Expression.TypeAs(
                    Expression.Constant(u, typeof(Uri)),
                    typeof(IAsyncResult)
                );

            var r =
                Expression.Constant(value: null, typeof(IAsyncResult));

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Unary_TypeAs_Nop()
        {
            var e =
                Expression.TypeAs(
                    S,
                    typeof(IComparable)
                );

            AssertOptimized(e, e);
        }

        [TestMethod]
        public void Unary_Assignment_Nop()
        {
            var e =
                Expression.PostIncrementAssign(
                    Expression.Parameter(typeof(int))
                );

            AssertOptimized(e, e);
        }

        [TestMethod]
        public void Unary_Negate_Method_Nop()
        {
            var e =
                Expression.Negate(
                    Expression.Parameter(typeof(decimal))
                );

            AssertOptimized(GetPureUnaryOptimizer(), e, e);
        }

        [TestMethod]
        public void Unary_Negate_Method_Eval()
        {
            var e =
                Expression.Negate(
                    Expression.Constant(49.95m)
                );

            var r =
                Expression.Constant(-49.95m);

            AssertOptimized(GetPureUnaryOptimizer(), e, r);
        }

        [TestMethod]
        public void Unary_Negate_Method_Lifted_NonNull_Eval()
        {
            var e =
                Expression.Negate(
                    Expression.Constant(49.95m, typeof(decimal?))
                );

            var r =
                Expression.Constant(-49.95m, typeof(decimal?));

            AssertOptimized(GetPureUnaryOptimizer(), e, r);
        }

        [TestMethod]
        public void Unary_Negate_Method_Lifted_Null_Eval()
        {
            var e =
                Expression.Negate(
                    Expression.Constant(value: null, typeof(decimal?))
                );

            var r =
                Expression.Constant(value: null, typeof(decimal?));

            AssertOptimized(GetPureUnaryOptimizer(), e, r);
        }

        [TestMethod]
        public void Unary_Negate_Method_Custom_Eval()
        {
            var e =
                Expression.Negate(
                    Expression.Constant(new UnaryOps(), typeof(UnaryOps))
                );

            var r =
                Expression.Constant(42, typeof(int));

            AssertOptimized(GetPureUnaryOptimizer(), e, r);
        }

        [TestMethod]
        public void Unary_Negate_Method_Custom_Lifted_NonNull_Eval()
        {
            var e =
                Expression.Negate(
                    Expression.Constant(new UnaryOps(), typeof(UnaryOps?))
                );

            var r =
                Expression.Constant(42, typeof(int?));

            AssertOptimized(GetPureUnaryOptimizer(), e, r);
        }

        [TestMethod]
        public void Unary_Negate_Method_Custom_Lifted_Null_Eval()
        {
            var e =
                Expression.Negate(
                    Expression.Constant(value: null, typeof(UnaryOps?))
                );

            var r =
                Expression.Constant(value: null, typeof(int?));

            AssertOptimized(GetPureUnaryOptimizer(), e, r);
        }

        [TestMethod]
        public void Unary_UnaryPlus_Method_Custom_NonLifted_NonNull_Eval()
        {
            var e =
                Expression.UnaryPlus(
                    Expression.Constant(new UnaryOps(), typeof(UnaryOps?))
                );

            var r =
                Expression.Constant(42, typeof(int));

            AssertOptimized(GetPureUnaryOptimizer(), e, r);
        }

        [TestMethod]
        public void Unary_UnaryPlus_Method_Custom_NonLifted_Null_Eval()
        {
            var e =
                Expression.UnaryPlus(
                    Expression.Constant(value: null, typeof(UnaryOps?))
                );

            var r =
                Expression.Constant(-42, typeof(int));

            AssertOptimized(GetPureUnaryOptimizer(), e, r);
        }

        [TestMethod]
        public void Unary_Convert_Method_Eval()
        {
            var dt = new DateTime(1983, 2, 11);
            var dto = (DateTimeOffset)dt;

            var e =
                Expression.Convert(
                    Expression.Constant(dt),
                    typeof(DateTimeOffset)
                );

            var r =
                Expression.Constant(dto);

            AssertOptimized(GetPureUnaryOptimizer(), e, r);
        }

        [TestMethod]
        public void Unary_Convert_Method_Lifted_NonNull_Eval()
        {
            var dt = new DateTime(1983, 2, 11);
            var dto = (DateTimeOffset)dt;

            var e =
                Expression.Convert(
                    Expression.Constant(dt, typeof(DateTime?)),
                    typeof(DateTimeOffset?)
                );

            var r =
                Expression.Constant(dto, typeof(DateTimeOffset?));

            AssertOptimized(GetPureUnaryOptimizer(), e, r);
        }

        [TestMethod]
        public void Unary_Convert_Method_Lifted_Null_Eval()
        {
            var e =
                Expression.Convert(
                    Expression.Constant(value: null, typeof(DateTime?)),
                    typeof(DateTimeOffset?)
                );

            var r =
                Expression.Constant(value: null, typeof(DateTimeOffset?));

            AssertOptimized(GetPureUnaryOptimizer(), e, r);
        }

        [TestMethod]
        public void Unary_Convert_Method_Lifted_NullReference()
        {
            var e =
                Expression.Convert(
                    Expression.Constant(value: null, typeof(DateTime?)),
                    typeof(DateTimeOffset)
                );

            // NB: We could possibly support evaluating to InvalidOperationException at compile time.

            AssertOptimized(GetPureUnaryOptimizer(), e, e);
        }

        [TestMethod]
        public void Unary_Convert_Method_Lifted_ToNullable()
        {
            var dt = new DateTime(1983, 2, 11);
            var dto = (DateTimeOffset)dt;

            var e =
                Expression.Convert(
                    Expression.Constant(dt, typeof(DateTime)),
                    typeof(DateTimeOffset?)
                );

            var r =
                Expression.Constant(dto, typeof(DateTimeOffset?));

            AssertOptimized(GetPureUnaryOptimizer(), e, r);
        }

        [TestMethod]
        public void Unary_Convert_Method_Custom1_NonLifted()
        {
            var e =
                Expression.Convert(
                    Expression.Constant(new UnaryOps(), typeof(UnaryOps)),
                    typeof(string)
                );

            var r =
                Expression.Constant("42", typeof(string));

            AssertOptimized(GetPureUnaryOptimizer(), e, r);
        }

        [TestMethod]
        public void Unary_Convert_Method_Custom1_Lifted_NonNull()
        {
            var e =
                Expression.Convert(
                    Expression.Constant(new UnaryOps(), typeof(UnaryOps?)),
                    typeof(string)
                );

            var r =
                Expression.Constant("42", typeof(string));

            AssertOptimized(GetPureUnaryOptimizer(), e, r);
        }

        [TestMethod]
        public void Unary_Convert_Method_Custom1_Lifted_Null()
        {
            var e =
                Expression.Convert(
                    Expression.Constant(value: null, typeof(UnaryOps?)),
                    typeof(string)
                );

            // NB: This is a tricky case which we choose not to optimize to an exception.

            AssertOptimized(GetPureUnaryOptimizer(), e, e);
        }

        [TestMethod]
        public void Unary_Convert_Method_Custom2_NonLifted()
        {
            var e =
                Expression.Convert(
                    Expression.Constant(new UnaryOps(), typeof(UnaryOps)),
                    typeof(int)
                );

            var r =
                Expression.Constant(42, typeof(int));

            AssertOptimized(GetPureUnaryOptimizer(), e, r);
        }

        [TestMethod]
        public void Unary_Convert_Method_Custom2_Lifted_NonNull()
        {
            var e =
                Expression.Convert(
                    Expression.Constant(new UnaryOps(), typeof(UnaryOps?)),
                    typeof(int?)
                );

            var r =
                Expression.Constant(42, typeof(int?));

            AssertOptimized(GetPureUnaryOptimizer(), e, r);
        }

        [TestMethod]
        public void Unary_Convert_Method_Custom2_Lifted_Null()
        {
            var e =
                Expression.Convert(
                    Expression.Constant(value: null, typeof(UnaryOps?)),
                    typeof(int?)
                );

            var r =
                Expression.Constant(value: null, typeof(int?));

            AssertOptimized(GetPureUnaryOptimizer(), e, r);
        }

        [TestMethod]
        public void Unary_Convert_Method_Custom3_Null()
        {
            var e =
                Expression.Convert(
                    Expression.Constant(value: null, typeof(UnaryOps?)),
                    typeof(Uri)
                );

            var r =
                Expression.Constant(new Uri("http://bar.com"), typeof(Uri));

            AssertOptimized(GetPureUnaryOptimizer(), e, r);
        }

        [TestMethod]
        public void Unary_Convert_Method_Custom3_NonNull()
        {
            var e =
                Expression.Convert(
                    Expression.Constant(new UnaryOps(), typeof(UnaryOps?)),
                    typeof(Uri)
                );

            var r =
                Expression.Constant(new Uri("http://foo.com"), typeof(Uri));

            AssertOptimized(GetPureUnaryOptimizer(), e, r);
        }

        [TestMethod]
        public void Unary_Convert_Method_Custom4_Null()
        {
            var e =
                Expression.Convert(
                    Expression.Constant(value: null, typeof(UnaryOps?)),
                    typeof(long)
                );

            var r =
                Expression.Constant(0L, typeof(long));

            AssertOptimized(GetPureUnaryOptimizer(), e, r);
        }

        [TestMethod]
        public void Unary_Convert_Method_Custom4_NonNull()
        {
            var e =
                Expression.Convert(
                    Expression.Constant(new UnaryOps(), typeof(UnaryOps?)),
                    typeof(long)
                );

            var r =
                Expression.Constant(42L, typeof(long));

            AssertOptimized(GetPureUnaryOptimizer(), e, r);
        }

        [TestMethod]
        public void Unary_Convert_Method_Custom5_Null()
        {
            var e =
                Expression.Convert(
                    Expression.Constant(value: null, typeof(UnaryOps?)),
                    typeof(byte?)
                );

            var r =
                Expression.Constant((byte)0, typeof(byte?));

            AssertOptimized(GetPureUnaryOptimizer(), e, r);
        }

        [TestMethod]
        public void Unary_Convert_Method_Custom5_NonNull()
        {
            var e =
                Expression.Convert(
                    Expression.Constant(new UnaryOps(), typeof(UnaryOps?)),
                    typeof(byte?)
                );

            var r =
                Expression.Constant((byte)42, typeof(byte?));

            AssertOptimized(GetPureUnaryOptimizer(), e, r);
        }

        private static ExpressionOptimizer GetPureUnaryOptimizer() => new(new PureUnarySemanticProvider(), GetEvaluatorFactory());

        private sealed class PureUnarySemanticProvider : DefaultSemanticProvider
        {
            public override bool IsPure(MemberInfo member)
            {
                return member.DeclaringType == typeof(decimal)
                    || member.DeclaringType == typeof(DateTimeOffset)
                    || member.DeclaringType == typeof(UnaryOps);
            }

            public override bool IsImmutable(Type type)
            {
                return base.IsImmutable(type)
                    || type == typeof(Uri)
                    || type == typeof(decimal)
                    || type == typeof(DateTimeOffset)
                    || type == typeof(UnaryOps);
            }
        }

#pragma warning disable IDE0060 // Remove unused parameter
        private struct UnaryOps
        {
            public static int operator -(UnaryOps b) => 42;
            public static int operator +(UnaryOps? b) => b == null ? -42 : 42;

            public static explicit operator string(UnaryOps b) => "42";
            public static explicit operator int(UnaryOps b) => 42;

            public static explicit operator Uri(UnaryOps? b) => b == null ? new Uri("http://bar.com") : new Uri("http://foo.com");
            public static explicit operator long(UnaryOps? b) => b == null ? 0L : 42L;
            public static explicit operator byte?(UnaryOps? b) => b == null ? (byte?)0 : (byte?)42;
        }
#pragma warning restore IDE0060 // Remove unused parameter
    }
}
