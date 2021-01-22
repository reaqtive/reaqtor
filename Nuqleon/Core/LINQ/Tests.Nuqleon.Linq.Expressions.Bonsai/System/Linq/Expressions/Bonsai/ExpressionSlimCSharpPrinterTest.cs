// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - May 2014 - Created this file.
//

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Tests.System.Linq.Expressions.Bonsai
{
    [TestClass]
    public class ExpressionSlimCSharpPrinterTest : TestBase
    {
        [TestMethod]
        public void ExpressionSlimCSharpPrinter_NotImplemented()
        {
            var e = Expression.Default(typeof(int));
            var tests = new Expression[]
            {
                Expression.Block(e, e),
                Expression.Continue(Expression.Label(), typeof(void)),
                Expression.Label(Expression.Label()),
                Expression.Loop(e),
                Expression.Switch(e, e, Expression.SwitchCase(e, e)),
                Expression.TryFinally(e, e),
            };

            foreach (var t in tests)
            {
                Assert.ThrowsException<NotImplementedException>(() => t.ToExpressionSlim().ToCSharpString());
            }
        }

        [TestMethod]
        public void ExpressionSlimCSharpPrinter_Constant1()
        {
            var ex = Expression.Constant(42).ToExpressionSlim();
            var cs = ex.ToCSharpString();
            Assert.AreEqual("42", cs);
        }

        [TestMethod]
        public void ExpressionSlimCSharpPrinter_Binary1()
        {
            var ex = Expression.Add(Expression.Multiply(Expression.Constant(1), Expression.Constant(2)), Expression.Constant(3)).ToExpressionSlim();
            var cs = ex.ToCSharpString();
            Assert.AreEqual("1 * 2 + 3", cs);
        }

        [TestMethod]
        public void ExpressionSlimCSharpPrinter_Binary2()
        {
            var types = new Dictionary<ExpressionType, string>
            {
                { ExpressionType.Add, "+" },
                { ExpressionType.Subtract, "-" },
                { ExpressionType.Multiply, "*" },
                { ExpressionType.Divide, "/" },
                { ExpressionType.Modulo, "%" },
                { ExpressionType.And, "&" },
                { ExpressionType.Or, "|" },
                { ExpressionType.ExclusiveOr, "^" },
                { ExpressionType.LeftShift, "<<" },
                { ExpressionType.RightShift, ">>" },
            };

            var c1 = Expression.Constant(1);
            var c2 = Expression.Constant(2);

            foreach (var t in types)
            {
                var ex = Expression.MakeBinary(t.Key, c1, c2).ToExpressionSlim();
                var cs = ex.ToCSharpString();
                Assert.AreEqual(string.Format("1 {0} 2", t.Value), cs);
            }
        }

        [TestMethod]
        public void ExpressionSlimCSharpPrinter_Binary()
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
                { Expression.ArrayIndex(Expression.NewArrayInit(typeof(int), Expression.Constant(1)), Expression.Constant(7)), "new int[] { 1 }[7]" },
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
                { Expression.Power(Expression.Constant(1.0), Expression.Constant(2.0)), "System.Math.Pow(1D, 2D)" },
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
                { Expression.PowerAssign(y, Expression.Constant(2.0)), "y = System.Math.Pow(y, 2D)" },
                { Expression.RightShiftAssign(x, Expression.Constant(2)), "x >>= 2" },
                { Expression.SubtractAssign(x, Expression.Constant(2)), "x -= 2" },
                { Expression.SubtractAssignChecked(x, Expression.Constant(2)), "checked(x -= 2)" },
            });
        }

        [TestMethod]
        public void ExpressionSlimCSharpPrinter_Binary_Associativity()
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
                { ((Expression<Func<Indexable, string>>)(s => s.Foo[0])).Body, "s.Foo[0]" },
            });
        }

#pragma warning disable CA1822 // Mark static.
        private sealed class Indexable
        {
            public string[] Foo => null;
        }
#pragma warning restore CA1822

        [TestMethod]
        public void ExpressionSlimCSharpPrinter_Binary_Precedence()
        {
            AssertPrint(new Dictionary<Expression, string>
            {
                { Expression.Add(Expression.Constant(1), Expression.Multiply(Expression.Constant(2), Expression.Constant(3))), "1 + 2 * 3" },
                { Expression.Add(Expression.Multiply(Expression.Constant(1), Expression.Constant(2)), Expression.Constant(3)), "1 * 2 + 3" },

                { Expression.Divide(Expression.Constant(1), Expression.Subtract(Expression.Constant(2), Expression.Constant(3))), "1 / (2 - 3)" },
                { Expression.Divide(Expression.Subtract(Expression.Constant(1), Expression.Constant(2)), Expression.Constant(3)), "(1 - 2) / 3" },

                { Expression.ArrayIndex(Expression.NewArrayInit(typeof(int), Expression.Constant(1)), Expression.Add(Expression.Constant(1), Expression.Constant(2))), "new int[] { 1 }[1 + 2]" },
            });
        }

        [TestMethod]
        public void ExpressionSlimCSharpPrinter_Conditional()
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
        public void ExpressionSlimCSharpPrinter_Constant()
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

                // default
                { Expression.Default(typeof(string)), "default(string)" },
                
                // Type
                { Expression.Constant(typeof(string)), "typeof(string)" },

                //// Enum
                //{ Expression.Constant(ConsoleColor.Red), "ConsoleColor.Red" },
                //{ Expression.Constant((ConsoleColor)123), "(ConsoleColor)123" },
                //{ Expression.Constant(FileShare.Read), "FileShare.Read" },
                //{ Expression.Constant(FileShare.ReadWrite), "FileShare.ReadWrite" },
                //{ Expression.Constant(FileShare.Read | FileShare.Delete), "FileShare.Read | FileShare.Delete" },
                //{ Expression.Constant((FileShare)123), "(FileShare)123" },

                // Nullable
                { Expression.Constant(42, typeof(int?)), "(int?)42" },
                { Expression.Constant(true, typeof(bool?)), "(bool?)true" },

                // Unknown
                { Expression.Constant(TimeSpan.Zero, typeof(TimeSpan)), "default(System.TimeSpan) /* 00:00:00 */" },
            });
        }

        [TestMethod]
        public void ExpressionSlimCSharpPrinter_Invocation()
        {
            var x = Expression.Parameter(typeof(int), "x");

            AssertPrint(new Dictionary<Expression, string>
            {
                //{ Expression.Invoke(Expression.Constant(new Func<int, int>(a => a)), Expression.Constant(1)), "__c0(1)" },
                { Expression.Invoke(Expression.Lambda(Expression.Multiply(x, Expression.Constant(2)), x), Expression.Constant(1)), "(x => x * 2)(1)" },
            });
        }

        [TestMethod]
        public void ExpressionSlimCSharpPrinter_Lambda()
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
                { l2, "x => x * 2" },
                { l3, "(x, s) => x + s.Length" },
                { l4, "(t, t1) => t + t1" },
            });
        }

        [TestMethod]
        public void ExpressionSlimCSharpPrinter_ListInit()
        {
            var l1 = (Expression<Func<List<int>>>)(() => new List<int> { 2, 3, 5 });
            var l2 = (Expression<Func<List<int>>>)(() => new List<int>(7) { 2, 3, 5 });
            var l3 = (Expression<Func<List<int>>>)(() => new List<int>(2) { });
            var l4 = (Expression<Func<Dictionary<int, int>>>)(() => new Dictionary<int, int> { { 2, 3 }, { 5, 7 } });
            var l5 = (Expression<Func<int>>)(() => new List<int> { 2, 3, 5 }.Count);

            AssertPrint(new Dictionary<Expression, string>
            {
                { l1.Body, "new System.Collections.Generic.List<int> { 2, 3, 5 }" },
                { l2.Body, "new System.Collections.Generic.List<int>(7) { 2, 3, 5 }" },
                { l3.Body, "new System.Collections.Generic.List<int>(2) { }" },
                { l4.Body, "new System.Collections.Generic.Dictionary<int, int> { { 2, 3 }, { 5, 7 } }" },
                { l5.Body, "new System.Collections.Generic.List<int> { 2, 3, 5 }.Count" },
            });
        }

        [TestMethod]
        public void ExpressionSlimCSharpPrinter_MemberInit()
        {
            var m1 = (Expression<Func<Types.Bar>>)(() => new Types.Bar { });
            var m2 = (Expression<Func<Types.Bar>>)(() => new Types.Bar(1) { });
            var m3 = (Expression<Func<Types.Bar>>)(() => new Types.Bar { Qux = 2 });
            var m4 = (Expression<Func<Types.Bar>>)(() => new Types.Bar(1) { Qux = 2 });
            var m5 = (Expression<Func<Types.Bar>>)(() => new Types.Bar(1) { Qux = 2, Foo = { Baz = 3 } });
            var m6 = (Expression<Func<Types.Bar>>)(() => new Types.Bar(1) { Qux = 2, Ys = { 3, 5 } });

            AssertPrint(new Dictionary<Expression, string>
            {
                { m1.Body, "new Types.Bar { }" },
                { m2.Body, "new Types.Bar(1) { }" },
                { m3.Body, "new Types.Bar { Qux = 2 }" },
                { m4.Body, "new Types.Bar(1) { Qux = 2 }" },
                { m5.Body, "new Types.Bar(1) { Qux = 2, Foo = { Baz = 3 } }" },
                { m6.Body, "new Types.Bar(1) { Qux = 2, Ys = { 3, 5 } }" },
            });
        }

        [TestMethod]
        public void ExpressionSlimCSharpPrinter_Member()
        {
            var m1 = (Expression<Func<AppDomain>>)(() => AppDomain.CurrentDomain);
            var m2 = (Expression<Func<int>>)(() => "foo".Length);
            var m3 = (Expression<Func<int>>)(() => (DateTime.Now + TimeSpan.Zero).Day);
            var m4 = (Expression<Func<string>>)(() => AppDomain.CurrentDomain.BaseDirectory);

            AssertPrint(new Dictionary<Expression, string>
            {
                { m1.Body, "System.AppDomain.CurrentDomain" },
                { m2.Body, "\"foo\".Length" },
                { m3.Body, "(System.DateTime.Now + System.TimeSpan.Zero).Day" },
                { m4.Body, "System.AppDomain.CurrentDomain.BaseDirectory" },
            });
        }

        [TestMethod]
        public void ExpressionSlimCSharpPrinter_MethodCall()
        {
            var m1 = (Expression<Func<double>>)(() => Math.Sin(12.34));
            var m2 = (Expression<Func<string>>)(() => "foo".Substring(1, 2));
            var m3 = (Expression<Func<string>>)(() => "foo".ToUpper());
            var m4 = (Expression<Func<string>>)(() => Activator.CreateInstance<string>());
            var m5 = (Expression<Func<IEnumerable<char>>>)(() => "foo".Reverse());
            var m6 = Expression.ArrayIndex(Expression.NewArrayBounds(typeof(int), Expression.Constant(2), Expression.Constant(3)), Expression.Constant(1), Expression.Constant(2));

            AssertPrint(new Dictionary<Expression, string>
            {
                { m1.Body, "System.Math.Sin(12.34D)" },
                { m2.Body, "\"foo\".Substring(1, 2)" },
                { m3.Body, "\"foo\".ToUpper()" },
                { m4.Body, "System.Activator.CreateInstance<string>()" },
                { m5.Body, "System.Linq.Enumerable.Reverse<char>(\"foo\")" }, // TODO: generic parameter trimming
                { m6, "new int[2, 3][1, 2]" },
            });
        }

#pragma warning disable IDE0050 // Convert to tuple. (Tests for anonymous types.)

        [TestMethod]
        public void ExpressionSlimCSharpPrinter_New()
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
                { n1.Body, "new System.TimeSpan()" },
                { n2.Body, "new System.TimeSpan(1, 2, 3)" },
                { n3.Body, "new { }" },
                { n4.Body, "new { a = 2, b = 3 }" },
                { n5.Body, "System.Linq.Enumerable.Select(System.Linq.Enumerable.Select(new int[] { 1, 2 }, x => new { x, y = x * 2 }), t => new { t.y, z = t.x })" }, // TODO: parameter type inference; best type of array
                { n6.Body, "System.Linq.Enumerable.Select(System.Linq.Enumerable.Select(new int[] { 1, 2 }, x => new { x, y = x * 2 }), t => t.x + t.y)" },
                { n7.Body, "System.Linq.Enumerable.Select(System.Linq.Enumerable.Select(System.Linq.Enumerable.Select(new int[] { 1, 2 }, x => new { x, y = x * 2 }), t => new { t, z = t.x + t.y }), t => t.t.x * t.z + 1)" },
            });
        }

#pragma warning restore IDE0050

        [TestMethod]
        public void ExpressionSlimCSharpPrinter_NewArray()
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
        public void ExpressionSlimCSharpPrinter_TypeBinary()
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

        [TestMethod]
        public void ExpressionSlimCSharpPrinter_Unary()
        {
            var f1 = (Expression<Func<ConsoleColor, ConsoleColor>>)(c => ~c);
#pragma warning disable IDE0004 // Remove Unnecessary Cast. (Keeping conversion in expression tree.)
            var f2 = (Expression<Func<ConsoleColor, ConsoleColor>>)(c => (ConsoleColor)~(int)c);
#pragma warning restore IDE0004

            var o = Expression.Parameter(typeof(object), "o");
            var x = Expression.Parameter(typeof(int), "x");
            var b = Expression.Parameter(typeof(bool), "b");
            var e = Expression.Parameter(typeof(Exception), "e");

            AssertPrint(new Dictionary<Expression, string>
            {
                { Expression.ArrayLength(Expression.NewArrayInit(typeof(int), Expression.Constant(1))), "new int[] { 1 }.Length" },
                { Expression.Convert(Expression.Constant(1), typeof(double)), "(double)1" },
                { Expression.ConvertChecked(Expression.Constant(1), typeof(double)), "checked((double)1)" },
                { Expression.OnesComplement(Expression.Constant(1)), "~1" },
                { f1.Body, "(System.ConsoleColor)~(int)c" },
                { f2.Body, "(System.ConsoleColor)~(int)c" },
                //{ Expression.OnesComplement(Expression.Constant(new WithCustomOperator())), "~__c0" },
                { Expression.Negate(Expression.Constant(1)), "-1" },
                { Expression.Negate(Expression.Negate(Expression.Constant(1))), "-(-1)" },
                { Expression.Negate(Expression.UnaryPlus(Expression.Constant(1))), "-+1" },
                { Expression.NegateChecked(Expression.Constant(1)), "checked(-1)" },
                { Expression.Not(Expression.Constant(true)), "!true" },
                //{ Expression.Not(Expression.Constant(new WithCustomOperator())), "!__c0" },
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

        private static void AssertPrint(Dictionary<Expression, string> tests)
        {
            foreach (var kv in tests)
            {
                Assert.AreEqual(kv.Value, Print(kv.Key), kv.Key.ToString());
            }
        }

        private static string Print(Expression expression)
        {
            return expression.ToExpressionSlim().ToCSharpString();
        }
    }
}

namespace Types
{
#pragma warning disable IDE0060 // Remove unused parameter
#pragma warning disable 0649
    internal sealed class Bar
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

    internal sealed class Foo
    {
        public int Baz;
    }
#pragma warning restore 0649
#pragma warning restore IDE0060 // Remove unused parameter
}
