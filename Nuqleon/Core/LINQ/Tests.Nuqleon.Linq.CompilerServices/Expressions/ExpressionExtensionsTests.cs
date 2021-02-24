// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - May 2013 - Created this file.
//

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.CompilerServices;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace Tests.System.Linq.CompilerServices
{
    [TestClass]
    public class ExpressionExtensionsTests
    {
        #region Funcletize

        [TestMethod]
        public void ExpressionExtensions_Funcletize_ArgumentChecking()
        {
#pragma warning disable IDE0034 // Simplify 'default' expression (illustrative of method signature)
            AssertEx.ThrowsException<ArgumentNullException>(() => ExpressionExtensions.Funcletize(default(Expression)), ex => Assert.AreEqual("expression", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => ExpressionExtensions.Funcletize<int>(default(Expression)), ex => Assert.AreEqual("expression", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => ExpressionExtensions.Funcletize<int>(default(Expression<Func<int>>)), ex => Assert.AreEqual("expression", ex.ParamName));
#pragma warning restore IDE0034 // Simplify 'default' expression
        }

        [TestMethod]
        public void ExpressionExtensions_Funcletize_NonLambda_Typed_ExactType()
        {
            var f = Expression.Constant(42).Funcletize<int>();
            var g = f.Compile();
            Assert.AreEqual(42, g());
        }

        [TestMethod]
        public void ExpressionExtensions_Funcletize_NonLambda_Typed_BoxingConversion()
        {
            var f = Expression.Constant(42).Funcletize<object>();
            var g = f.Compile();
            Assert.AreEqual(42, g());
        }

        [TestMethod]
        public void ExpressionExtensions_Funcletize_NonLambda_Typed_CovariantConversion()
        {
            var f = Expression.Constant("bar").Funcletize<object>();
            var g = f.Compile();
            Assert.AreEqual("bar", g());
        }

        [TestMethod]
        public void ExpressionExtensions_Funcletize_NonLambda_Untyped_BoxingConversion()
        {
            var f = Expression.Constant(42).Funcletize();
            var g = f.Compile();
            Assert.AreEqual(42, g());
        }

        [TestMethod]
        public void ExpressionExtensions_Funcletize_NonLambda_Untyped_CovariantConversion()
        {
            var f = Expression.Constant("bar").Funcletize();
            var g = f.Compile();
            Assert.AreEqual("bar", g());
        }

        [TestMethod]
        public void ExpressionExtensions_Funcletize_Lambda_Direct()
        {
            var f = Expression.Lambda<Func<int>>(Expression.Constant(42)).Funcletize();
            var g = f.Compile();
            Assert.AreEqual(42, g());
        }

        [TestMethod]
        public void ExpressionExtensions_Funcletize_Lambda_TypedFunction()
        {
            var f = Expression.Lambda<Func<int>>(Expression.Constant(42)).Funcletize<Func<int>>();
            var g = f.Compile();
            Assert.AreEqual(42, g()());
        }

        [TestMethod]
        public void ExpressionExtensions_Funcletize_Lambda_TypedFunction_CovariantConversion()
        {
            var f = Expression.Lambda<Func<string>>(Expression.Constant("bar")).Funcletize<Func<object>>();
            var g = f.Compile();
            Assert.AreEqual("bar", g()());
        }

        [TestMethod]
        public void ExpressionExtensions_Funcletize_Open()
        {
            var f = (Expression<Func<int, int>>)(x => x + 1);

            AssertEx.ThrowsException<UnboundParameterException>(() => f.Body.Funcletize(), ex =>
            {
                Assert.AreSame(f.Body, ex.Expression);
                Assert.IsTrue(f.Parameters.SequenceEqual(ex.Parameters));
            });
        }

        #endregion

        #region Evaluate

        [TestMethod]
        public void ExpressionExtensions_Evaluate_ArgumentChecking()
        {
#pragma warning disable IDE0034 // Simplify 'default' expression (illustrative of method signature)
            AssertEx.ThrowsException<ArgumentNullException>(() => ExpressionExtensions.Evaluate(default(Expression)), ex => Assert.AreEqual("expression", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => ExpressionExtensions.Evaluate<int>(default(Expression)), ex => Assert.AreEqual("expression", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => ExpressionExtensions.Evaluate<int>(default(Expression<Func<int>>)), ex => Assert.AreEqual("expression", ex.ParamName));

            var cache = new SimpleCompiledDelegateCache();
            AssertEx.ThrowsException<ArgumentNullException>(() => ExpressionExtensions.Evaluate(default(Expression), cache), ex => Assert.AreEqual("expression", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => ExpressionExtensions.Evaluate<int>(default(Expression), cache), ex => Assert.AreEqual("expression", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => ExpressionExtensions.Evaluate<int>(default(Expression<Func<int>>), cache), ex => Assert.AreEqual("expression", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => ExpressionExtensions.Evaluate(Expression.Empty(), cache: null), ex => Assert.AreEqual("cache", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => ExpressionExtensions.Evaluate<int>(Expression.Default(typeof(int)), cache: null), ex => Assert.AreEqual("cache", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => ExpressionExtensions.Evaluate<int>(() => 42, cache: null), ex => Assert.AreEqual("cache", ex.ParamName));
#pragma warning restore IDE0034 // Simplify 'default' expression
        }

        [TestMethod]
        public void ExpressionExtensions_Evaluate_NonLambda_Typed_ExactType()
        {
            var x = Expression.Constant(42).Evaluate<int>();
            Assert.AreEqual(42, x);
        }

        [TestMethod]
        public void ExpressionExtensions_Evaluate_NonLambda_Typed_BoxingConversion()
        {
            var x = Expression.Constant(42).Evaluate<object>();
            Assert.AreEqual(42, x);
        }

        [TestMethod]
        public void ExpressionExtensions_Evaluate_NonLambda_Typed_CovariantConversion()
        {
            var x = Expression.Constant("bar").Evaluate<object>();
            Assert.AreEqual("bar", x);
        }

        [TestMethod]
        public void ExpressionExtensions_Evaluate_NonLambda_Untyped_BoxingConversion()
        {
            var x = Expression.Constant(42).Evaluate();
            Assert.AreEqual(42, x);
        }

        [TestMethod]
        public void ExpressionExtensions_Evaluate_NonLambda_Untyped_CovariantConversion()
        {
            var x = Expression.Constant("bar").Evaluate();
            Assert.AreEqual("bar", x);
        }

        [TestMethod]
        public void ExpressionExtensions_Evaluate_Lambda_Direct()
        {
            var x = Expression.Lambda<Func<int>>(Expression.Constant(42)).Evaluate();
            Assert.AreEqual(42, x);
        }

        [TestMethod]
        public void ExpressionExtensions_Evaluate_Lambda_TypedFunction()
        {
            var f = Expression.Lambda<Func<int>>(Expression.Constant(42)).Evaluate<Func<int>>();
            Assert.AreEqual(42, f());
        }

        [TestMethod]
        public void ExpressionExtensions_Evaluate_Lambda_TypedFunction_CovariantConversion()
        {
            var f = Expression.Lambda<Func<string>>(Expression.Constant("bar")).Evaluate<Func<object>>();
            Assert.AreEqual("bar", f());
        }

        [TestMethod]
        public void ExpressionExtensions_Evaluate_Open()
        {
            var f = (Expression<Func<int, int>>)(x => x + 1);

            AssertEx.ThrowsException<UnboundParameterException>(() => f.Body.Evaluate(), ex =>
            {
                Assert.AreSame(f.Body, ex.Expression);
                Assert.IsTrue(f.Parameters.SequenceEqual(ex.Parameters));
            });
        }

        [TestMethod]
        public void ExpressionExtensions_Evaluate_Misc1()
        {
            var res = new Dictionary<Expression, int>
            {
                { Expression.Constant(42), 42 },
                { Expression.Add(Expression.Constant(1), Expression.Constant(2)), 3 },
                { Expression.Default(typeof(int)), 0 },
            };

            foreach (var kv in res)
            {
                Assert.AreEqual((int)kv.Key.Evaluate(), kv.Value);
                Assert.AreEqual(kv.Key.Evaluate<int>(), kv.Value);
            }
        }

        [TestMethod]
        public void ExpressionExtensions_Evaluate_Misc2()
        {
            var ex1 = new ArgumentException();

            var res = new Dictionary<Expression, Exception>
            {
                { Expression.Constant(ex1), ex1 },
                { Expression.Default(typeof(Exception)), null },
                { Expression.Default(typeof(ArgumentException)), null },
            };

            foreach (var kv in res)
            {
                Assert.AreSame((Exception)kv.Key.Evaluate(), kv.Value);
                Assert.AreSame(kv.Key.Evaluate<Exception>(), kv.Value);
            }
        }

        [TestMethod]
        public void ExpressionExtensions_Evaluate_Misc3()
        {
            var res = new Dictionary<Expression, string>
            {
                { Expression.Constant("bar"), "bar" },
                { Expression.Call(Expression.Constant("bar"), typeof(string).GetMethod("ToUpper", Type.EmptyTypes)), "BAR" },
                { Expression.Default(typeof(string)), null },
            };

            foreach (var kv in res)
            {
                Assert.AreEqual((string)kv.Key.Evaluate(), kv.Value);
                Assert.AreEqual(kv.Key.Evaluate<string>(), kv.Value);
            }
        }

        [TestMethod]
        public void ExpressionExtensions_Evaluate_Misc4()
        {
            Assert.ThrowsException<InvalidOperationException>(() => Expression.Constant(42).Evaluate<string>());
            Assert.ThrowsException<InvalidOperationException>(() => Expression.Default(typeof(int)).Evaluate<string>());

            Assert.ThrowsException<InvalidOperationException>(() => Expression.Constant("bar").Evaluate<int>());
            Assert.ThrowsException<InvalidOperationException>(() => Expression.Default(typeof(string)).Evaluate<int>());
        }

        [TestMethod]
        public void ExpressionExtensions_Evaluate_Misc5()
        {
            var o1 = (int)Expression.Constant(42).Evaluate<object>();
            Assert.AreEqual(42, o1);

            var o2 = (int)Expression.Default(typeof(int)).Evaluate<object>();
            Assert.AreEqual(0, o2);
        }

        #region Cached Evaluation

        [TestMethod]
        public void ExpressionExtensions_Evaluate_NonLambda_Cached()
        {
            var cache = new AssertionCache();
            var e = Expression.Add(Expression.Constant(41), Expression.Constant(1));
            var result = e.Evaluate(cache);
            Assert.AreEqual(42, result);
            Assert.AreEqual(1, cache.GetOrAddCount);
        }

        [TestMethod]
        public void ExpressionExtensions_Evaluate_NonLambda_Typed_Cached()
        {
            var cache = new AssertionCache();
            var e = Expression.Add(Expression.Constant(41), Expression.Constant(1));
            var result = e.Evaluate<int>(cache);
            Assert.AreEqual(42, result);
            Assert.AreEqual(1, cache.GetOrAddCount);
        }

        [TestMethod]
        public void ExpressionExtensions_Evaluate_Lambda_Cached()
        {
            var cache = new AssertionCache();
            Expression<Func<int>> f = () => 42;
            var result = f.Evaluate<int>(cache);
            Assert.AreEqual(42, result);
            Assert.AreEqual(1, cache.GetOrAddCount);
        }

        private class AssertionCache : ICompiledDelegateCache
        {
            public int GetOrAddCount
            {
                get;
                set;
            }

            public int Count => 0;

            public void Clear()
            {
            }

            public virtual Delegate GetOrAdd(LambdaExpression expression)
            {
                GetOrAddCount++;
                return expression.Compile();
            }
        }

        #endregion

        #endregion

        #region ToCSharpString

        [TestMethod]
        public void ToCSharpString_ArgumentChecking()
        {
            Assert.ThrowsException<ArgumentNullException>(() => ExpressionExtensions.ToCSharpString(expression: null));
            Assert.ThrowsException<ArgumentNullException>(() => ExpressionExtensions.ToCSharp(expression: null));
            Assert.ThrowsException<ArgumentNullException>(() => ExpressionExtensions.ToCSharpString(expression: null, allowCompilerGeneratedNames: false));
            Assert.ThrowsException<ArgumentNullException>(() => ExpressionExtensions.ToCSharp(expression: null, allowCompilerGeneratedNames: false));
        }

        [TestMethod]
        public void ToCSharpString_Binary()
        {
            var x = Expression.Parameter(typeof(int), "x");
            var y = Expression.Parameter(typeof(double), "y");

            AssertPrint(new Dictionary<Expression, string>
            {
                { Expression.Add(Expression.Constant(1), Expression.Constant(2)), "1 + 2" },
                { Expression.AddChecked(Expression.Constant(1), Expression.Constant(2)), "checked(1 + 2)" },
                { Expression.And(Expression.Constant(1), Expression.Constant(2)), "1 & 2" },
                { Expression.And(Expression.Constant(true), Expression.Constant(false)), "true & false" },
                { Expression.AndAlso(Expression.Constant(true), Expression.Constant(false)), "true && false" },
                { Expression.ArrayIndex(Expression.Constant(new[] { 1, 2, 3 }), Expression.Constant(7)), "__c0[7]" },
                { Expression.Coalesce(Expression.Constant("foo"), Expression.Constant("bar")), "\"foo\" ?? \"bar\"" },
                { Expression.Divide(Expression.Constant(1), Expression.Constant(2)), "1 / 2" },
                { Expression.Equal(Expression.Constant(1), Expression.Constant(2)), "1 == 2" },
                { Expression.ExclusiveOr(Expression.Constant(1), Expression.Constant(2)), "1 ^ 2" },
                { Expression.GreaterThan(Expression.Constant(1), Expression.Constant(2)), "1 > 2" },
                { Expression.GreaterThanOrEqual(Expression.Constant(1), Expression.Constant(2)), "1 >= 2" },
                { Expression.LeftShift(Expression.Constant(1), Expression.Constant(2)), "1 << 2" },
                { Expression.LessThan(Expression.Constant(1), Expression.Constant(2)), "1 < 2" },
                { Expression.LessThanOrEqual(Expression.Constant(1), Expression.Constant(2)), "1 <= 2" },
                { Expression.Modulo(Expression.Constant(1), Expression.Constant(2)), "1 % 2" },
                { Expression.Multiply(Expression.Constant(1), Expression.Constant(2)), "1 * 2" },
                { Expression.MultiplyChecked(Expression.Constant(1), Expression.Constant(2)), "checked(1 * 2)" },
                { Expression.NotEqual(Expression.Constant(1), Expression.Constant(2)), "1 != 2" },
                { Expression.Or(Expression.Constant(1), Expression.Constant(2)), "1 | 2" },
                { Expression.Or(Expression.Constant(true), Expression.Constant(false)), "true | false" },
                { Expression.OrElse(Expression.Constant(true), Expression.Constant(false)), "true || false" },
                { Expression.Power(Expression.Constant(1.0), Expression.Constant(2.0)), "Math.Pow(1D, 2D)" },
                { Expression.RightShift(Expression.Constant(1), Expression.Constant(2)), "1 >> 2" },
                { Expression.Subtract(Expression.Constant(1), Expression.Constant(2)), "1 - 2" },
                { Expression.SubtractChecked(Expression.Constant(1), Expression.Constant(2)), "checked(1 - 2)" },
                { Expression.AddAssign(x, Expression.Constant(2)), "x += 2" },
                { Expression.AddAssignChecked(x, Expression.Constant(2)), "checked(x += 2)" },
                { Expression.AndAssign(x, Expression.Constant(2)), "x &= 2" },
                { Expression.Assign(x, Expression.Constant(2)), "x = 2" },
                { Expression.DivideAssign(x, Expression.Constant(2)), "x /= 2" },
                { Expression.ExclusiveOrAssign(x, Expression.Constant(2)), "x ^= 2" },
                { Expression.LeftShiftAssign(x, Expression.Constant(2)), "x <<= 2" },
                { Expression.ModuloAssign(x, Expression.Constant(2)), "x %= 2" },
                { Expression.MultiplyAssign(x, Expression.Constant(2)), "x *= 2" },
                { Expression.MultiplyAssignChecked(x, Expression.Constant(2)), "checked(x *= 2)" },
                { Expression.OrAssign(x, Expression.Constant(2)), "x |= 2" },
                { Expression.PowerAssign(y, Expression.Constant(2.0)), "y = Math.Pow(y, 2D)" },
                { Expression.RightShiftAssign(x, Expression.Constant(2)), "x >>= 2" },
                { Expression.SubtractAssign(x, Expression.Constant(2)), "x -= 2" },
                { Expression.SubtractAssignChecked(x, Expression.Constant(2)), "checked(x -= 2)" },
            });
        }

        [TestMethod]
        public void ToCSharpString_Binary_Associativity()
        {
            AssertPrint(new Dictionary<Expression, string>
            {
                { Expression.Add(Expression.Constant(1), Expression.Add(Expression.Constant(2), Expression.Constant(3))), "1 + (2 + 3)" },
                { Expression.Add(Expression.Add(Expression.Constant(1), Expression.Constant(2)), Expression.Constant(3)), "1 + 2 + 3" },

                { Expression.Subtract(Expression.Constant(1), Expression.Subtract(Expression.Constant(2), Expression.Constant(3))), "1 - (2 - 3)" },
                { Expression.Subtract(Expression.Subtract(Expression.Constant(1), Expression.Constant(2)), Expression.Constant(3)), "1 - 2 - 3" },

                { Expression.Add(Expression.Constant(1), Expression.Subtract(Expression.Constant(2), Expression.Constant(3))), "1 + (2 - 3)" },
                { Expression.Subtract(Expression.Add(Expression.Constant(1), Expression.Constant(2)), Expression.Constant(3)), "1 + 2 - 3" },

                { Expression.Subtract(Expression.Constant(1), Expression.Add(Expression.Constant(2), Expression.Constant(3))), "1 - (2 + 3)" },
                { Expression.Add(Expression.Subtract(Expression.Constant(1), Expression.Constant(2)), Expression.Constant(3)), "1 - 2 + 3" },

                { Expression.Coalesce(Expression.Coalesce(Expression.Constant("bar"), Expression.Constant("foo")), Expression.Constant("qux")), "(\"bar\" ?? \"foo\") ?? \"qux\"" },
                { Expression.Coalesce(Expression.Constant("bar"), Expression.Coalesce(Expression.Constant("foo"), Expression.Constant("qux"))), "\"bar\" ?? \"foo\" ?? \"qux\"" },

                { ((Expression<Func<DateTime, int>>)(dt => dt.Year + dt.Month)).Body, "dt.Year + dt.Month" },
                { ((Expression<Func<DateTime[], int>>)(dt => dt[0].Year)).Body, "dt[0].Year" },
                { ((Expression<Func<ADS, string>>)(s => s.AppDomainInitializerArguments[0])).Body, "s.AppDomainInitializerArguments[0]" },
            });
        }

        private sealed class ADS
        {
            public string[] AppDomainInitializerArguments { get; set; }
        }

        [TestMethod]
        public void ToCSharpString_Binary_Precedence()
        {
            AssertPrint(new Dictionary<Expression, string>
            {
                { Expression.Add(Expression.Constant(1), Expression.Multiply(Expression.Constant(2), Expression.Constant(3))), "1 + 2 * 3" },
                { Expression.Add(Expression.Multiply(Expression.Constant(1), Expression.Constant(2)), Expression.Constant(3)), "1 * 2 + 3" },

                { Expression.Divide(Expression.Constant(1), Expression.Subtract(Expression.Constant(2), Expression.Constant(3))), "1 / (2 - 3)" },
                { Expression.Divide(Expression.Subtract(Expression.Constant(1), Expression.Constant(2)), Expression.Constant(3)), "(1 - 2) / 3" },

                { Expression.ArrayIndex(Expression.Constant(new[] { 1, 2, 3 }), Expression.Add(Expression.Constant(1), Expression.Constant(2))), "__c0[1 + 2]" },
            });
        }

        [TestMethod]
        public void ToCSharpString_Conditional()
        {
            var b = Expression.Parameter(typeof(bool), "b");

            AssertPrint(new Dictionary<Expression, string>
            {
                { Expression.Condition(Expression.Constant(true), Expression.Constant(1), Expression.Constant(2)), "true ? 1 : 2" },
                { Expression.Condition(Expression.Equal(Expression.Constant(3), Expression.Constant(4)), Expression.Constant(1), Expression.Constant(2)), "3 == 4 ? 1 : 2" },
                { Expression.Add(Expression.Constant(3), Expression.Condition(Expression.Constant(true), Expression.Constant(1), Expression.Constant(2))), "3 + (true ? 1 : 2)" },
                { Expression.Add(Expression.Condition(Expression.Constant(true), Expression.Constant(1), Expression.Constant(2)), Expression.Constant(3)), "(true ? 1 : 2) + 3" },
                { Expression.Condition(Expression.Equal(Expression.Condition(Expression.Constant(true), Expression.Constant(1), Expression.Constant(2)), Expression.Constant(3)), Expression.Constant(4), Expression.Constant(5)), "(true ? 1 : 2) == 3 ? 4 : 5" },
                { Expression.Condition(Expression.Constant(true), Expression.Constant(1), Expression.Condition(Expression.Constant(true), Expression.Constant(2), Expression.Constant(3))), "true ? 1 : true ? 2 : 3" },
                { Expression.Condition(Expression.Assign(b, Expression.Constant(true)), Expression.Constant(1), Expression.Constant(2)), "(b = true) ? 1 : 2" },
            });
        }

        [TestMethod]
        public void ToCSharpString_Constant()
        {
            AssertPrint(new Dictionary<Expression, string>
            {
                // byte
                { Expression.Constant((byte)1), "(byte)1" },
                { Expression.Constant((byte)123), "(byte)123" },
                { Expression.Constant(byte.MaxValue), "(byte)255" },

                // sbyte
                { Expression.Constant((sbyte)1), "(sbyte)1" },
                { Expression.Constant((sbyte)-1), "(sbyte)-1" },
                { Expression.Constant(sbyte.MaxValue), "(sbyte)127" },
                { Expression.Constant(sbyte.MinValue), "(sbyte)-128" },

                // short
                { Expression.Constant((short)1), "(short)1" },
                { Expression.Constant((short)-1), "(short)-1" },
                { Expression.Constant(short.MaxValue), "(short)32767" },
                { Expression.Constant(short.MinValue), "(short)-32768" },

                // ushort
                { Expression.Constant((ushort)1), "(ushort)1" },
                { Expression.Constant(ushort.MaxValue), "(ushort)65535" },
                { Expression.Constant(ushort.MinValue), "(ushort)0" },

                // int
                { Expression.Constant(1), "1" },
                { Expression.Constant(-1), "-1" },
                { Expression.Constant(123), "123" },
                { Expression.Constant(-123), "-123" },
                { Expression.Constant(int.MaxValue), "2147483647" },
                { Expression.Constant(int.MinValue), "-2147483648" },

                // uint
                { Expression.Constant(1u), "1U" },
                { Expression.Constant(123u), "123U" },
                { Expression.Constant(uint.MaxValue), "4294967295U" },
                { Expression.Constant(uint.MinValue), "0U" },

                // long
                { Expression.Constant(1L), "1L" },
                { Expression.Constant(-1L), "-1L" },
                { Expression.Constant(123L), "123L" },
                { Expression.Constant(-123L), "-123L" },
                { Expression.Constant(long.MaxValue), "9223372036854775807L" },
                { Expression.Constant(long.MinValue), "-9223372036854775808L" },

                // ulong
                { Expression.Constant(1UL), "1UL" },
                { Expression.Constant(123UL), "123UL" },
                { Expression.Constant(ulong.MaxValue), "18446744073709551615UL" },
                { Expression.Constant(ulong.MinValue), "0UL" },

                // float
                { Expression.Constant(1.0f), "1F" },
                { Expression.Constant(-1.0f), "-1F" },
                { Expression.Constant(12.34f), "12.34F" },
                { Expression.Constant(-12.34f), "-12.34F" },
                { Expression.Constant(float.NaN), "float.NaN" },
                { Expression.Constant(float.NegativeInfinity), "float.NegativeInfinity" },
                { Expression.Constant(float.PositiveInfinity), "float.PositiveInfinity" },

                // double
                { Expression.Constant(1.0), "1D" },
                { Expression.Constant(-1.0), "-1D" },
                { Expression.Constant(12.34), "12.34D" },
                { Expression.Constant(-12.34), "-12.34D" },
                { Expression.Constant(double.NaN), "double.NaN" },
                { Expression.Constant(double.NegativeInfinity), "double.NegativeInfinity" },
                { Expression.Constant(double.PositiveInfinity), "double.PositiveInfinity" },

                // decimal
                { Expression.Constant(1.0m), "1.0M" },
                { Expression.Constant(-1.0m), "-1.0M" },
                { Expression.Constant(12.34m), "12.34M" },
                { Expression.Constant(-12.34m), "-12.34M" },

                // bool
                { Expression.Constant(false), "false" },
                { Expression.Constant(true), "true" },

                // char
                { Expression.Constant('a'), "'a'" },
                { Expression.Constant('9'), "'9'" },
                { Expression.Constant('"'), "'\"'" },
                { Expression.Constant('\t'), @"'\t'" },
                { Expression.Constant('\r'), @"'\r'" },
                { Expression.Constant('\n'), @"'\n'" },
                { Expression.Constant('\b'), @"'\b'" },
                { Expression.Constant('\a'), @"'\a'" },
                { Expression.Constant('\v'), @"'\v'" },
                { Expression.Constant('\f'), @"'\f'" },
                { Expression.Constant('\\'), @"'\\'" },
                { Expression.Constant('\''), @"'\''" },
                { Expression.Constant('\0'), @"'\0'" },

                // string
                { Expression.Constant(""), @"""""" },
                { Expression.Constant("Hello, World!"), @"""Hello, World!""" },
                { Expression.Constant("I said \"Wow\" because this is cool.\r\n"), @"""I said \""Wow\"" because this is cool.\r\n""" },

                // null literal
                { Expression.Constant(value: null, typeof(object)), "(object)null" },
                { Expression.Constant(default(string), typeof(string)), "(string)null" },
                { Expression.Constant(default(int?), typeof(int?)), "(int?)null" },

                // Type
                { Expression.Constant(typeof(string)), "typeof(string)" },

                // Enum
                { Expression.Constant(ConsoleColor.Red), "ConsoleColor.Red" },
                { Expression.Constant((ConsoleColor)123), "(ConsoleColor)123" },
                { Expression.Constant(FileShare.Read), "FileShare.Read" },
                { Expression.Constant(FileShare.ReadWrite), "FileShare.ReadWrite" },
                { Expression.Constant(FileShare.Read | FileShare.Delete), "FileShare.Read | FileShare.Delete" },
                { Expression.Constant((FileShare)123), "(FileShare)123" },
            });
        }

        [TestMethod]
        public void ToCSharpString_Constant_Unprintable()
        {
            var d = AppDomain.CurrentDomain;
            var n = DateTime.Now;
            var e =
                Expression.AndAlso(
                    Expression.Equal(
                        Expression.Constant(d),
                        Expression.Constant(d)
                    ),
                    Expression.Equal(
                        Expression.Constant(n),
                        Expression.Constant(n)
                    )
                );

            var res = e.ToCSharp();

            Assert.AreEqual(3, res.Constants.Count);
            Assert.AreSame(d, res.Constants["__c0"].Value);
            Assert.AreEqual(n, res.Constants["__c1"].Value);
            Assert.AreEqual(n, res.Constants["__c2"].Value);

            Assert.AreEqual("__c0 == __c0 && __c1 == __c2", res.Code);
        }

        [TestMethod]
        public void ToCSharpString_Default()
        {
            AssertPrint(new Dictionary<Expression, string>
            {
                { Expression.Default(typeof(int)), "default(int)" },
            });
        }

        [TestMethod]
        public void ToCSharpString_Invocation()
        {
            var x = Expression.Parameter(typeof(int), "x");

            AssertPrint(new Dictionary<Expression, string>
            {
                { Expression.Invoke(Expression.Constant(new Func<int, int>(a => a)), Expression.Constant(1)), "__c0(1)" },
                { Expression.Invoke(Expression.Lambda(Expression.Multiply(x, Expression.Constant(2)), x), Expression.Constant(1)), "((Func<int, int>)((int x) => x * 2))(1)" },
            });
        }

        [TestMethod]
        public void ToCSharpString_Lambda()
        {
            var y = Expression.Parameter(typeof(int));
            var z = Expression.Parameter(typeof(int));

            var l1 = (Expression<Func<int>>)(() => 42);
            var l2 = (Expression<Func<int, int>>)(x => x * 2);
            var l3 = (Expression<Func<int, string, int>>)((x, s) => x + s.Length);
            var l4 = Expression.Lambda(Expression.Add(y, z), y, z);

            AssertPrint(new Dictionary<Expression, string>
            {
                { l1, "() => 42" },
                { l2, "(int x) => x * 2" },
                { l3, "(int x, string s) => x + s.Length" },
                { l4, "(int t, int t1) => t + t1" },
            });
        }

        [TestMethod]
        public void ToCSharpString_ListInit()
        {
            var l1 = (Expression<Func<List<int>>>)(() => new List<int> { 2, 3, 5 });
            var l2 = (Expression<Func<List<int>>>)(() => new List<int>(7) { 2, 3, 5 });
            var l3 = (Expression<Func<List<int>>>)(() => new List<int>(2) { });
            var l4 = (Expression<Func<Dictionary<int, int>>>)(() => new Dictionary<int, int> { { 2, 3 }, { 5, 7 } });
            var l5 = (Expression<Func<int>>)(() => new List<int> { 2, 3, 5 }.Count);

            AssertPrint(new Dictionary<Expression, string>
            {
                { l1.Body, "new List<int> { 2, 3, 5 }" },
                { l2.Body, "new List<int>(7) { 2, 3, 5 }" },
                { l3.Body, "new List<int>(2) { }" },
                { l4.Body, "new Dictionary<int, int> { { 2, 3 }, { 5, 7 } }" },
                { l5.Body, "new List<int> { 2, 3, 5 }.Count" },
            });
        }

#pragma warning disable IDE0060 // Remove unused parameter
#pragma warning disable 0649
        private sealed class Bar
        {
            public Bar()
            {
            }

            public Bar(int x)
            {
            }

            public int Qux { get; set; }
            public Foo Foo;
            public List<int> Ys { get; set; }
        }

        private sealed class Foo
        {
            public int Baz;
        }
#pragma warning restore 0649
#pragma warning restore IDE0060 // Remove unused parameter

        [TestMethod]
        public void ToCSharpString_MemberInit()
        {
            var m1 = (Expression<Func<Bar>>)(() => new Bar { });
            var m2 = (Expression<Func<Bar>>)(() => new Bar(1) { });
            var m3 = (Expression<Func<Bar>>)(() => new Bar { Qux = 2 });
            var m4 = (Expression<Func<Bar>>)(() => new Bar(1) { Qux = 2 });
            var m5 = (Expression<Func<Bar>>)(() => new Bar(1) { Qux = 2, Foo = { Baz = 3 } });
            var m6 = (Expression<Func<Bar>>)(() => new Bar(1) { Qux = 2, Ys = { 3, 5 } });

            AssertPrint(new Dictionary<Expression, string>
            {
                { m1.Body, "new Bar { }" },
                { m2.Body, "new Bar(1) { }" },
                { m3.Body, "new Bar { Qux = 2 }" },
                { m4.Body, "new Bar(1) { Qux = 2 }" },
                { m5.Body, "new Bar(1) { Qux = 2, Foo = { Baz = 3 } }" },
                { m6.Body, "new Bar(1) { Qux = 2, Ys = { 3, 5 } }" },
            });
        }

        [TestMethod]
        public void ToCSharpString_MethodCall()
        {
            var m1 = (Expression<Func<double>>)(() => Math.Sin(12.34));
            var m2 = (Expression<Func<string>>)(() => "foo".Substring(1, 2));
            var m3 = (Expression<Func<string>>)(() => "foo".ToUpper());
            var m4 = (Expression<Func<string>>)(() => Activator.CreateInstance<string>());
            var m5 = (Expression<Func<IEnumerable<char>>>)(() => "foo".Reverse());
            var m6 = Expression.ArrayIndex(Expression.Constant(new int[2, 2]), Expression.Constant(1), Expression.Constant(2));

            AssertPrint(new Dictionary<Expression, string>
            {
                { m1.Body, "Math.Sin(12.34D)" },
                { m2.Body, "\"foo\".Substring(1, 2)" },
                { m3.Body, "\"foo\".ToUpper()" },
                { m4.Body, "Activator.CreateInstance<string>()" },
                { m5.Body, "\"foo\".Reverse<char>()" }, // TODO: generic parameter trimming
                { m6, "__c0[1, 2]" },
            });
        }

        [TestMethod]
        public void ToCSharpString_Member()
        {
            var m1 = (Expression<Func<AppDomain>>)(() => AppDomain.CurrentDomain);
            var m2 = (Expression<Func<int>>)(() => "foo".Length);
            var m3 = (Expression<Func<int>>)(() => (DateTime.Now + TimeSpan.Zero).Day);
            var m4 = (Expression<Func<string>>)(() => AppDomain.CurrentDomain.BaseDirectory);

            AssertPrint(new Dictionary<Expression, string>
            {
                { m1.Body, "AppDomain.CurrentDomain" },
                { m2.Body, "\"foo\".Length" },
                { m3.Body, "(DateTime.Now + TimeSpan.Zero).Day" },
                { m4.Body, "AppDomain.CurrentDomain.BaseDirectory" },
            });
        }

#pragma warning disable IDE0050 // Convert to tuple. (Test for anonymous types.)

        [TestMethod]
        public void ToCSharpString_New()
        {
            var n1 = (Expression<Func<TimeSpan>>)(() => new TimeSpan());
            var n2 = (Expression<Func<TimeSpan>>)(() => new TimeSpan(1, 2, 3));
            var n3 = (Expression<Func<object>>)(() => new { });
            var n4 = (Expression<Func<object>>)(() => new { a = 2, b = 3 });
            var n5 = (Expression<Func<IEnumerable<object>>>)(() => new[] { 1, 2 }.Select(x => new { x, y = x * 2 }).Select(t => new { t.y, z = t.x }));
            var n6 = (Expression<Func<IEnumerable<int>>>)(() => from x in new[] { 1, 2 } let y = x * 2 select x + y);
            var n7 = (Expression<Func<IEnumerable<int>>>)(() => from x in new[] { 1, 2 } let y = x * 2 let z = x + y select x * z + 1);

            AssertPrint(new Dictionary<Expression, string>
            {
                { n1.Body, "new TimeSpan()" },
                { n2.Body, "new TimeSpan(1, 2, 3)" },
                { n3.Body, "new { }" },
                { n4.Body, "new { a = 2, b = 3 }" },
                { n5.Body, "new int[] { 1, 2 }.Select((int x) => new { x, y = x * 2 }).Select(t => new { t.y, z = t.x })" }, // TODO: parameter type inference; best type of array
                { n6.Body, "new int[] { 1, 2 }.Select((int x) => new { x, y = x * 2 }).Select(t => t.x + t.y)" },
                { n7.Body, "new int[] { 1, 2 }.Select((int x) => new { x, y = x * 2 }).Select(t => new { t, z = t.x + t.y }).Select(t => t.t.x * t.z + 1)" },
            });
        }

#pragma warning restore IDE0050

        [TestMethod]
        public void ToCSharpString_NewArray()
        {
            var n1 = (Expression<Func<int[]>>)(() => new int[1]);
            var n2 = (Expression<Func<int[]>>)(() => new int[] { 1, 2 });
            var n3 = (Expression<Func<int[,]>>)(() => new int[1, 2]);
            var n4 = (Expression<Func<object[]>>)(() => new[] { new { x = 1 } });

            AssertPrint(new Dictionary<Expression, string>
            {
                { n1.Body, "new int[1]" },
                { n2.Body, "new int[] { 1, 2 }" }, // TODO: implicitly typed arrays
                { n3.Body, "new int[1, 2]" },
                { n4.Body, "new[] { new { x = 1 } }" },
            });
        }

        [TestMethod]
        public void ToCSharpString_Parameter()
        {
            var p1 = Expression.Parameter(typeof(int), "x");
            var p2 = Expression.Parameter(typeof(int));
            var p3 = Expression.Parameter(typeof(int));

            AssertPrint(new Dictionary<Expression, string>
            {
                { p1, "x" },
                { p2, "g1" },
                { Expression.Add(p2, p3), "g1 + g2" },
            });
        }

        [TestMethod]
        public void ToCSharpString_Parameter_Globals()
        {
            var e = Expression.Parameter(typeof(int), "x");

            var res = e.ToCSharp();

            Assert.AreEqual(1, res.GlobalVariables.Count);
            Assert.AreSame(e, res.GlobalVariables[0]);
        }

        [TestMethod]
        public void ToCSharpString_Quote()
        {
            var e1 = new[] { 1, 2, 3 }.AsQueryable().Where(x => x > 0).Expression;
            var e2 = new[] { 1, 2, 3 }.AsQueryable().Select(x => new { x }).Where((x, i) => x.x > i).Expression;

            AssertPrint(new Dictionary<Expression, string>
            {
                { e1, "__c0.Where<int>((int x) => x > 0)" }, // TODO: infer parameter types and generic type parameters
                { e2, "__c0.Select((int x) => new { x }).Where((x, i) => x.x > i)" },
            });
        }

        [TestMethod]
        public void ToCSharpString_TypeBinary()
        {
            var n1 = (Expression<Func<object, bool>>)(o => o is string);
            var n2 = (Expression<Func<object, bool>>)(o => true || o is string);
            var n3 = Expression.TypeIs(Expression.Add(Expression.Constant(1), Expression.Constant(2)), typeof(int));
            var n4 = Expression.TypeIs(Expression.Equal(Expression.Constant(1), Expression.Constant(2)), typeof(bool));

            AssertPrint(new Dictionary<Expression, string>
            {
                { n1.Body, "o is string" },
                { n2.Body, "true || o is string" },
                { n3, "1 + 2 is int" },
                { n4, "(1 == 2) is bool" },
            });
        }

        private sealed class WithCustomOperator
        {
            public static int operator ~(WithCustomOperator _) => throw new NotImplementedException();

            public static int operator !(WithCustomOperator _) => throw new NotImplementedException();
        }

        [TestMethod]
        public void ToCSharpString_Unary()
        {
            var f1 = (Expression<Func<ConsoleColor, ConsoleColor>>)(c => ~c);
#pragma warning disable IDE0004 // Remove Unnecessary Cast. (Keeping inside expression tree.)
            var f2 = (Expression<Func<ConsoleColor, ConsoleColor>>)(c => (ConsoleColor)~(int)c);
#pragma warning restore IDE0004

            var o = Expression.Parameter(typeof(object), "o");
            var x = Expression.Parameter(typeof(int), "x");
            var b = Expression.Parameter(typeof(bool), "b");
            var e = Expression.Parameter(typeof(Exception), "e");

            AssertPrint(new Dictionary<Expression, string>
            {
                { Expression.ArrayLength(Expression.Constant(new[] { 1, 2, 3 })), "__c0.Length" },
                { Expression.Convert(Expression.Constant(1), typeof(double)), "(double)1" },
                { Expression.ConvertChecked(Expression.Constant(1), typeof(double)), "checked((double)1)" },
                { Expression.OnesComplement(Expression.Constant(1)), "~1" },
                { f1.Body, "(ConsoleColor)~(int)c" },
                { f2.Body, "(ConsoleColor)~(int)c" },
                { Expression.OnesComplement(Expression.Constant(new WithCustomOperator())), "~__c0" },
                { Expression.Negate(Expression.Constant(1)), "-1" },
                { Expression.Negate(Expression.Negate(Expression.Constant(1))), "-(-1)" },
                { Expression.Negate(Expression.UnaryPlus(Expression.Constant(1))), "-+1" },
                { Expression.NegateChecked(Expression.Constant(1)), "checked(-1)" },
                { Expression.Not(Expression.Constant(true)), "!true" },
                { Expression.Not(Expression.Constant(new WithCustomOperator())), "!__c0" },
                { Expression.TypeAs(Expression.Constant("foo"), typeof(string)), "\"foo\" as string" },
                { Expression.UnaryPlus(Expression.Constant(1)), "+1" },
                { Expression.UnaryPlus(Expression.UnaryPlus(Expression.Constant(1))), "+(+1)" },
                { Expression.UnaryPlus(Expression.Negate(Expression.Constant(1))), "+-1" },
                { Expression.PostIncrementAssign(x), "x++" },
                { Expression.PostDecrementAssign(x), "x--" },
                { Expression.PreIncrementAssign(x), "++x" },
                { Expression.PreDecrementAssign(x), "--x" },
            });

            Assert.ThrowsException<NotSupportedException>(() => Print(Expression.Decrement(x)));
            Assert.ThrowsException<NotSupportedException>(() => Print(Expression.Increment(x)));
            Assert.ThrowsException<NotSupportedException>(() => Print(Expression.IsTrue(b)));
            Assert.ThrowsException<NotSupportedException>(() => Print(Expression.IsFalse(b)));
            Assert.ThrowsException<NotSupportedException>(() => Print(Expression.Throw(e)));
            Assert.ThrowsException<NotSupportedException>(() => Print(Expression.Unbox(o, typeof(int))));
        }

        [TestMethod]
        public void ToCSharpString_CompilerGenerated()
        {
            var rec = RuntimeCompiler.CreateRecordType(new[] { new KeyValuePair<string, Type>("bar", typeof(int)) }, valueEquality: true);

            var exp = Expression.New(rec);

            Assert.ThrowsException<InvalidOperationException>(() => exp.ToCSharp());
            Assert.ThrowsException<InvalidOperationException>(() => exp.ToCSharpString());
            Assert.ThrowsException<InvalidOperationException>(() => exp.ToCSharp(allowCompilerGeneratedNames: false));
            Assert.ThrowsException<InvalidOperationException>(() => exp.ToCSharpString(allowCompilerGeneratedNames: false));

            var cs1 = exp.ToCSharp(allowCompilerGeneratedNames: true);
            var cs2 = exp.ToCSharpString(allowCompilerGeneratedNames: true);

            Assert.AreEqual(cs1.Code, cs2);
            Assert.AreEqual("new " + rec.Name + "()", cs2);
        }

        [TestMethod]
        public void ToCSharpString_Indexer()
        {
            AssertPrint(new Dictionary<Expression, string>
            {
                { ((Expression<Func<List<int>, int>>)(xs => xs[0])).Body, "xs[0]" },
                { ((Expression<Func<Dictionary<bool, int>, int>>)(xs => xs[true])).Body, "xs[true]" },
                { ((Expression<Func<MyDict, int>>)(xs => xs[7])).Body, "xs[7]" },
            });
        }

        private sealed class MyDict
        {
#pragma warning disable CA1822 // Mark static. (https://github.com/dotnet/roslyn/issues/50197)
            [IndexerName("Foo")]
            public int this[int x] => 42;
#pragma warning restore CA1822

            public int Item
            {
                get;
                set;
            }
        }

        private static void AssertPrint(Dictionary<Expression, string> tests)
        {
            foreach (var kv in tests)
            {
                Assert.AreEqual(kv.Value, Print(kv.Key));
            }
        }

        private static string Print(Expression expression) => expression.ToCSharpString();

        #endregion
    }
}
