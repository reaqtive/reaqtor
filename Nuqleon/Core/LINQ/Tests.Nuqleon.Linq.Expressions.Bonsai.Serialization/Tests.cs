// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD, ER - July 2013 - Created this file.
//

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Linq.CompilerServices;
using System.Linq.Expressions;
using System.Linq.Expressions.Bonsai.Serialization;
using System.Memory;
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Json = Nuqleon.Json.Expressions;

namespace Tests
{
    [TestClass]
    public partial class Tests
    {
        #region Constant

        [TestMethod]
        public void Bonsai_Constant_BuiltIn()
        {
            var cs = new[]
            {
                Expression.Constant((sbyte)42),
                Expression.Constant((byte)42),
                Expression.Constant((short)42),
                Expression.Constant((ushort)42),
                Expression.Constant(42),
                Expression.Constant((uint)42),
                Expression.Constant((long)42),
                Expression.Constant((ulong)42),
                Expression.Constant((float)42),
                Expression.Constant((double)42),
                //Not supported: typeof(Decimal)
                //Issue: inadequate constant converter
                //Expression.Constant((decimal)42),
                Expression.Constant(true),
                Expression.Constant("Hello"),
                //Not supported: typeof(Char)
                //Issue: inadequate constant converter
                //Expression.Constant('c'),
            };

            foreach (var c in cs)
            {
                RoundtripAndAssert(c);
            }
        }

        [TestMethod]
        public void Bonsai_Constant_Null()
        {
            var cs = new[]
            {
                Expression.Constant(value: null),
                Expression.Constant(value: null, typeof(string)),
            };

            foreach (var c in cs)
            {
                RoundtripAndAssert(c);
            }
        }

        //Not supported: null typeof(List<int>)
        //Issue: inadequate constant converter
        /*[TestMethod]
        public void Bonsai_Constant_Collections()
        {
            var xs = new List<int> { 1, 2, 3 };

            var ce = Expression.Constant(xs);

            var r = Roundtrip(ce);

            Assert.IsNotNull(r);
            Assert.AreEqual(ExpressionType.Constant, r.NodeType);

            var rc = (ConstantExpression)r;
            Assert.IsTrue(rc.Value != null && rc.Value is List<int>);
            Assert.IsTrue(Enumerable.SequenceEqual(xs, (List<int>)rc.Value));
        }*/

        private static void RoundtripAndAssert(ConstantExpression c)
        {
            var r = Roundtrip(c) as ConstantExpression;
            Assert.IsNotNull(r);

            Assert.AreEqual(c.Type, r.Type);
            Assert.AreEqual(c.NodeType, r.NodeType);
            Assert.AreEqual(c.Value, r.Value);

            var r08 = Roundtrip(c, V08) as ConstantExpression;
            Assert.IsNotNull(r08);

            Assert.AreEqual(c.Type, r08.Type);
            Assert.AreEqual(c.NodeType, r08.NodeType);
            Assert.AreEqual(c.Value, r08.Value);
        }

        #endregion

        #region Unary

        [TestMethod]
        public void Bonsai_Unary_Arith()
        {
            var p = Expression.Parameter(typeof(int), "p");

            var us = new[]
            {
                Expression.Negate(p),
                Expression.NegateChecked(p),
                Expression.UnaryPlus(p),
                Expression.OnesComplement(p),
                Expression.Decrement(p),
                Expression.Increment(p),
            };

            foreach (var a in us)
            {
                RoundtripAndAssert(a);
            }
        }

        [TestMethod]
        public void Bonsai_Unary_Logical()
        {
            var p = Expression.Parameter(typeof(bool), "p");

            var us = new[]
            {
                Expression.Not(p),
                Expression.IsFalse(p),
                Expression.IsTrue(p),
            };

            foreach (var a in us)
            {
                RoundtripAndAssert(a);
            }
        }

        [TestMethod]
        public void Bonsai_Unary_Compound()
        {
            var p = Expression.Parameter(typeof(int));

            var us = new[]
            {
                Expression.PostDecrementAssign(p),
                Expression.PreDecrementAssign(p),
                Expression.PostIncrementAssign(p),
                Expression.PreIncrementAssign(p),
            };

            foreach (var a in us)
            {
                RoundtripAndAssert(a);
            }
        }

        [TestMethod]
        public void Bonsai_Unary_ArrayLength()
        {
            var p = Expression.Parameter(typeof(int[]), "p");

            var us = new[]
            {
                Expression.ArrayLength(p),
            };

            foreach (var a in us)
            {
                RoundtripAndAssert(a);
            }
        }

        [TestMethod]
        public void Bonsai_Unary_Type()
        {
            var p = Expression.Parameter(typeof(string), "p");

            var us = new[]
            {
                Expression.TypeAs(p, typeof(string)),
            };

            foreach (var a in us)
            {
                RoundtripAndAssert(a);
            }
        }

        [TestMethod]
        public void Bonsai_Unary_WithMethod()
        {
            var p = Expression.Parameter(typeof(int));

            var us = new[] {
                Expression.MakeUnary(ExpressionType.Increment, p, type: null, typeof(CustomUnary).GetMethod("Increment")),
                Expression.MakeUnary(ExpressionType.UnaryPlus, p, type: null, typeof(CustomUnary).GetMethod("Plus")),
                Expression.MakeUnary(ExpressionType.Negate, p, type: null, typeof(CustomUnary).GetMethod("Negate")),
                Expression.MakeUnary(ExpressionType.NegateChecked, p, type: null, typeof(CustomUnary).GetMethod("Negate"))
            };

            foreach (var a in us)
            {
                RoundtripAndAssert(a);
            }
        }

        [TestMethod]
        public void Bonsai_Unary_Throw()
        {
            var p = Expression.Parameter(typeof(Exception));

            var us = new[] {
                Expression.Throw(p),
                Expression.Throw(p, typeof(int)),
                Expression.Rethrow(),
                Expression.Rethrow(typeof(int))
            };

            foreach (var a in us)
            {
                RoundtripAndAssert(a);
            }
        }

        [TestMethod]
        public void Bonsai_Unary_Unbox()
        {
            var p = Expression.Parameter(typeof(object));

            var us = new[] {
                Expression.Unbox(p, typeof(int))
            };

            foreach (var a in us)
            {
                RoundtripAndAssert(a);
            }
        }

        private static class CustomUnary
        {
            public static int Increment(int op)
            {
                return ++op;
            }

            public static int Plus(int i)
            {
                return i;
            }

            public static int Negate(int i)
            {
                return -i;
            }
        }

        private static void RoundtripAndAssert(UnaryExpression a)
        {
            var r = Roundtrip(a) as UnaryExpression;
            Assert.IsNotNull(r);

            Assert.AreEqual(a.Type, r.Type);
            Assert.AreEqual(a.NodeType, r.NodeType);
            Assert.AreEqual(a.Method, r.Method);
            Assert.AreEqual(a.IsLifted, r.IsLifted);
            Assert.AreEqual(a.IsLiftedToNull, r.IsLiftedToNull);

            if (a.Operand != null)
            {
                Assert.AreEqual(a.Operand.NodeType, r.Operand.NodeType);
            }
            else
            {
                Assert.IsNull(r.Operand);
            }

            var r08 = Roundtrip(a, V08) as UnaryExpression;
            Assert.IsNotNull(r08);

            Assert.AreEqual(a.Type, r08.Type);
            Assert.AreEqual(a.NodeType, r08.NodeType);
            Assert.AreEqual(a.Method, r08.Method);
            Assert.AreEqual(a.IsLifted, r08.IsLifted);
            Assert.AreEqual(a.IsLiftedToNull, r08.IsLiftedToNull);

            if (a.Operand != null)
            {
                Assert.AreEqual(a.Operand.NodeType, r08.Operand.NodeType);
            }
            else
            {
                Assert.IsNull(r08.Operand);
            }
        }

        #endregion

        #region Binary

        [TestMethod]
        public void Bonsai_Binary_Arith_BuiltIn()
        {
            var p1 = Expression.Parameter(typeof(int), "p1");
            var p2 = Expression.Parameter(typeof(int), "p2");

            var bs = new[]
            {
                Expression.Add(p1, p2),
                Expression.AddChecked(p1, p2),
                Expression.Subtract(p1, p2),
                Expression.SubtractChecked(p1, p2),
                Expression.Multiply(p1, p2),
                Expression.MultiplyChecked(p1, p2),
                Expression.Divide(p1, p2),
                Expression.Modulo(p1, p2),

                Expression.LeftShift(p1, p2),
                Expression.RightShift(p1, p2),

                Expression.And(p1, p2),
                Expression.Or(p1, p2),
                Expression.ExclusiveOr(p1, p2),
            };

            foreach (var a in bs)
            {
                RoundtripAndAssert(a);
            }
        }

        [TestMethod]
        public void Bonsai_Binary_Arith_Power()
        {
            var p1 = Expression.Parameter(typeof(double), "p1");
            var p2 = Expression.Parameter(typeof(double), "p2");

            var bs = new[]
            {
                Expression.Power(p1, p2),
            };

            foreach (var a in bs)
            {
                RoundtripAndAssert(a);
            }
        }

        [TestMethod]
        public void Bonsai_Binary_Arith_Lifted()
        {
            var p1 = Expression.Parameter(typeof(MyNumeric?), "p1");
            var p2 = Expression.Parameter(typeof(MyNumeric?), "p2");

            var bs = new[]
            {
                Expression.Add(p1, p2),
                Expression.AddChecked(p1, p2),
                Expression.Subtract(p1, p2),
                Expression.SubtractChecked(p1, p2),
                Expression.Multiply(p1, p2),
                Expression.MultiplyChecked(p1, p2),
                Expression.Divide(p1, p2),
                Expression.Modulo(p1, p2),

                Expression.And(p1, p2),
                Expression.Or(p1, p2),
                Expression.ExclusiveOr(p1, p2),
            };

            foreach (var a in bs)
            {
                RoundtripAndAssert(a);
            }
        }

        private struct MyNumeric
        {
            public static MyNumeric operator +(MyNumeric _1, MyNumeric _2) => throw new NotImplementedException();
            public static MyNumeric operator -(MyNumeric _1, MyNumeric _2) => throw new NotImplementedException();
            public static MyNumeric operator *(MyNumeric _1, MyNumeric _2) => throw new NotImplementedException();
            public static MyNumeric operator /(MyNumeric _1, MyNumeric _2) => throw new NotImplementedException();
            public static MyNumeric operator %(MyNumeric _1, MyNumeric _2) => throw new NotImplementedException();
            public static MyNumeric operator |(MyNumeric _1, MyNumeric _2) => throw new NotImplementedException();
            public static MyNumeric operator &(MyNumeric _1, MyNumeric _2) => throw new NotImplementedException();
            public static MyNumeric operator ^(MyNumeric _1, MyNumeric _2) => throw new NotImplementedException();
        }

        [TestMethod]
        public void Bonsai_Binary_OperatorOverloads()
        {
            var c1 = Expression.Constant(DateTime.Now);
            var c2 = Expression.Constant(TimeSpan.FromSeconds(1));

            var bs = new[]
            {
                Expression.Add(c1, c2),
                Expression.AddChecked(c1, c2),
                Expression.Subtract(c1, c2),
                Expression.SubtractChecked(c1, c2),
            };

            foreach (var a in bs)
            {
                RoundtripAndAssert(a);
            }
        }

        [TestMethod]
        public void Bonsai_Binary_Relational()
        {
            var p1 = Expression.Parameter(typeof(int), "p1");
            var p2 = Expression.Parameter(typeof(int), "p2");

            var bs = new[]
            {
                Expression.LessThan(p1, p2),
                Expression.LessThanOrEqual(p1, p2),
                Expression.GreaterThan(p1, p2),
                Expression.GreaterThanOrEqual(p1, p2),
                Expression.Equal(p1, p2),
                Expression.NotEqual(p1, p2),
            };

            foreach (var a in bs)
            {
                RoundtripAndAssert(a);
            }
        }

        [TestMethod]
        public void Bonsai_Binary_Logical()
        {
            var p1 = Expression.Parameter(typeof(bool), "p1");
            var p2 = Expression.Parameter(typeof(bool), "p2");

            var bs = new[]
            {
                Expression.And(p1, p2),
                Expression.Or(p1, p2),
                Expression.ExclusiveOr(p1, p2),
                Expression.AndAlso(p1, p2),
                Expression.OrElse(p1, p2),
            };

            foreach (var a in bs)
            {
                RoundtripAndAssert(a);
            }
        }

        [TestMethod]
        public void Bonsai_Binary_Assignment()
        {
            var p0 = Expression.Parameter(typeof(int), "p0");
            var p1 = Expression.Parameter(typeof(bool), "p1");
            var p2 = Expression.Parameter(typeof(string), "p2");
            var p3 = Expression.Parameter(typeof(Func<string, string>), "p3");

            var c0 = Expression.Constant(42);
            var c1 = Expression.Constant(true);
            var c2 = Expression.Constant("foo");
            var c3 = Expression.Default(typeof(Func<string, string>));

            var ba = new[]
            {
                Expression.Assign(p0, c0),
                Expression.Assign(p1, c1),
                Expression.Assign(p2, c2),
                Expression.Assign(p3, (Expression<Func<string, string>>)(s => s)),
                Expression.Assign(p3, c3),
            };

            foreach (var a in ba)
            {
                RoundtripAndAssert(a);
            }
        }


        [TestMethod]
        public void Bonsai_Binary_OpAssignment()
        {
            var p1 = Expression.Parameter(typeof(int));
            var p2 = Expression.Parameter(typeof(int));

            var bs = new[]
            {
                Expression.AddAssign(p1, p2),
                Expression.AddAssignChecked(p1, p2),
                Expression.SubtractAssign(p1, p2),
                Expression.SubtractAssignChecked(p1, p2),
                Expression.MultiplyAssign(p1, p2),
                Expression.MultiplyAssignChecked(p1, p2),
                Expression.DivideAssign(p1, p2),
                Expression.ModuloAssign(p1, p2),

                Expression.LeftShiftAssign(p1, p2),
                Expression.RightShiftAssign(p1, p2),

                Expression.AndAssign(p1, p2),
                Expression.OrAssign(p1, p2),
                Expression.ExclusiveOrAssign(p1, p2),
            };

            foreach (var a in bs)
            {
                RoundtripAndAssert(a);
            }
        }

        [TestMethod]
        public void Bonsai_Binary_PowerAssignment()
        {
            var p1 = Expression.Parameter(typeof(double));
            var p2 = Expression.Parameter(typeof(double));

            var bs = new[]
            {
                Expression.PowerAssign(p1, p2),
            };

            foreach (var a in bs)
            {
                RoundtripAndAssert(a);
            }
        }

        [TestMethod]
        public void Bonsai_Binary_OpAssignment_WithConversion()
        {
            var p1 = Expression.Parameter(typeof(string));
            var p2 = Expression.Parameter(typeof(string));

            var m = ((Func<string, string, string>)OpMethod).Method;
            var f = (Expression<Func<string, string>>)(s => s);

            var bs = new[]
            {
                Expression.AddAssign(p1, p2, m, f),
                Expression.AddAssignChecked(p1, p2, m, f),
                Expression.SubtractAssign(p1, p2, m, f),
                Expression.SubtractAssignChecked(p1, p2, m, f),
                Expression.MultiplyAssign(p1, p2, m, f),
                Expression.MultiplyAssignChecked(p1, p2, m, f),
                Expression.DivideAssign(p1, p2, m, f),
                Expression.ModuloAssign(p1, p2, m, f),

                Expression.LeftShiftAssign(p1, p2, m, f),
                Expression.RightShiftAssign(p1, p2, m, f),

                Expression.AndAssign(p1, p2, m, f),
                Expression.OrAssign(p1, p2, m, f),
                Expression.ExclusiveOrAssign(p1, p2, m, f),
            };

            foreach (var a in bs)
            {
                RoundtripAndAssert(a);
            }
        }

        private static string OpMethod(string a, string b)
        {
            return a + b;
        }

        [TestMethod]
        public void Bonsai_Binary_LiftedToNull()
        {
            var e = (Expression<Func<Persoon2, bool>>)(p => p.Geslacht == Sex.Male);

            RoundtripAndAssert((BinaryExpression)e.Body);
        }

        public class Persoon2
        {
            public string Naam { get; set; }

            public int Leeftijd { get; set; }

            public Sex? Geslacht { get; set; }
        }

        public enum Sex
        {
            Male = 1,
            Female = 2,
        }

        [TestMethod]
        public void Bonsai_Binary_Coalesce()
        {
            var e = (Expression<Func<string, string>>)(s => s ?? "null");
            var f = e.Compile();

            var g = (Expression<Func<string, string>>)Roundtrip(e);
            var h = g.Compile();

            Assert.AreEqual(f(null), h(null));
            Assert.AreEqual(f("Bar"), h("Bar"));

            var g08 = (Expression<Func<string, string>>)Roundtrip(e, V08);
            var h08 = g.Compile();

            Assert.AreEqual(f(null), h08(null));
            Assert.AreEqual(f("Bar"), h08("Bar"));
        }

        [TestMethod]
        public void Bonsai_Binary_Coalesce_WithConversion()
        {
            var f = (Expression<Func<string, int>>)(s => int.Parse(s));
            var p = Expression.Parameter(typeof(string));
            var c = Expression.Coalesce(p, Expression.Constant(0), f);

            var e = (Expression<Func<string, int>>)Expression.Lambda(c, new[] { p });
            var r = (Expression<Func<string, int>>)Roundtrip(e);

            var ef = e.Compile();
            var rf = r.Compile();

            Assert.AreEqual(ef(null), rf(null));
            Assert.AreEqual(ef("0"), rf("0"));
            Assert.AreEqual(e.Type, r.Type);
        }

        [TestMethod]
        public void Bonsai_Binary_Type()
        {
            var p1 = Expression.Parameter(typeof(string), "p1");

            var bs = new TypeBinaryExpression[]
            {
                Expression.TypeIs(p1, typeof(string)),
                Expression.TypeEqual(p1, typeof(string))
            };

            foreach (var a in bs)
            {
                RoundtripAndAssert(a);
            }
        }

        [TestMethod]
        public void Bonsai_Binary_Comparison()
        {
            var p1 = Expression.Parameter(typeof(int), "p1");
            var p2 = Expression.Parameter(typeof(int), "p2");
            var p3 = Expression.Parameter(typeof(int?), "p3");
            var p4 = Expression.Parameter(typeof(int?), "p4");
            var bs = new[] {
                Expression.MakeBinary(
                    ExpressionType.LessThan,
                    p1,
                    p2,
                    false,
                    typeof(MyRelationalOperation).GetMethod("LessThan")
                ),
                Expression.MakeBinary(
                    ExpressionType.LessThan,
                    p3,
                    p4,
                    true,
                    typeof(MyRelationalOperation).GetMethod("LessThan")
                ),
                Expression.MakeBinary(
                    ExpressionType.Equal,
                    p3,
                    p4,
                    false,
                    null)
            };

            foreach (var a in bs)
            {
                RoundtripAndAssert(a);
            }
        }

        [TestMethod]
        public void Bonsai_Comparison_OverloadedOperator()
        {
            var e = Expression.Equal(Expression.Parameter(typeof(OEq)), Expression.Parameter(typeof(int)), liftToNull: false, method: null);
            RoundtripAndAssert(e);
        }

        private static class MyRelationalOperation
        {
            public static bool LessThan(int x, int y)
            {
                return x < y;
            }
        }

        private sealed class OEq
        {
#pragma warning disable IDE0060 // Remove unused parameter (https://github.com/dotnet/roslyn/issues/32852)
            public static string operator ==(OEq x, int y) => y.ToString();

            public static string operator !=(OEq x, int y) => y.ToString();
#pragma warning restore IDE0060 // Remove unused parameter

            public override bool Equals(object obj)
            {
                return object.ReferenceEquals(this, obj);
            }

            public override int GetHashCode()
            {
                return 0;
            }
        }

        private static void RoundtripAndAssert(BinaryExpression a)
        {
            var r = Roundtrip(a) as BinaryExpression;
            Assert.IsNotNull(r);

            Assert.AreEqual(a.Type, r.Type);
            Assert.AreEqual(a.NodeType, r.NodeType);
            Assert.AreEqual(a.Method, r.Method);
            Assert.AreEqual(a.IsLifted, r.IsLifted);
            Assert.AreEqual(a.IsLiftedToNull, r.IsLiftedToNull);
            Assert.AreEqual(a.Left.NodeType, r.Left.NodeType);
            Assert.AreEqual(a.Right.NodeType, r.Right.NodeType);

            var r08 = Roundtrip(a, V08) as BinaryExpression;
            Assert.IsNotNull(r08);

            Assert.AreEqual(a.Type, r08.Type);
            Assert.AreEqual(a.NodeType, r08.NodeType);
            Assert.AreEqual(a.Method, r08.Method);
            Assert.AreEqual(a.IsLifted, r08.IsLifted);
            Assert.AreEqual(a.IsLiftedToNull, r08.IsLiftedToNull);
            Assert.AreEqual(a.Left.NodeType, r08.Left.NodeType);
            Assert.AreEqual(a.Right.NodeType, r08.Right.NodeType);
        }

        private static void RoundtripAndAssert(TypeBinaryExpression a)
        {
            var r = Roundtrip(a) as TypeBinaryExpression;
            Assert.IsNotNull(r);

            Assert.AreEqual(a.NodeType, r.NodeType);
            Assert.AreEqual(a.TypeOperand, r.TypeOperand);
            Assert.AreEqual(a.Type, r.Type);
            Assert.AreEqual(a.Expression.NodeType, r.Expression.NodeType);

            var r08 = Roundtrip(a, V08) as TypeBinaryExpression;
            Assert.IsNotNull(r08);

            Assert.AreEqual(a.NodeType, r08.NodeType);
            Assert.AreEqual(a.TypeOperand, r08.TypeOperand);
            Assert.AreEqual(a.Type, r08.Type);
            Assert.AreEqual(a.Expression.NodeType, r08.Expression.NodeType);
        }

        #endregion

        #region Block
        [TestMethod]
        public void Bonsai_Block()
        {
            var v1 = Expression.Parameter(typeof(object));
            var v2 = Expression.Parameter(typeof(int), "x");
            var v3 = Expression.Parameter(typeof(bool), "b");

            var e1 = Expression.Empty();
            var e2 = Expression.Constant(42);
            var e3 = Expression.Add(e2, v2);

            var bs = new[] {
                Expression.Block(e1),
                Expression.Block(e1, e1),
                Expression.Block(e1, e1, e1),
                Expression.Block(e2),
                Expression.Block(typeof(void), e2),
                Expression.Block(new[] { v1 }, e1),
                Expression.Block(new[] { v1 }, v1),
                Expression.Block(new[] { v1, v3 }, e1, e2, e3),
                Expression.Block(new[] { v1, v2, v3 }, e1, e2, e3),
                Expression.Block(typeof(void), new[] { v1, v2, v3 }, e1, e2, e3),
            };

            foreach (var a in bs)
            {
                RoundtripAndAssert(a);
            }
        }

        private static void RoundtripAndAssert(BlockExpression b)
        {
            var r = Roundtrip(b);
            Assert.IsTrue(new ExpressionEqualityComparer(() => new GlobalParameterSafeComparator()).Equals(b, r));
        }

        private sealed class GlobalParameterSafeComparator : ExpressionEqualityComparator
        {
            protected override bool EqualsGlobalParameter(ParameterExpression x, ParameterExpression y)
            {
                return x.Name == y.Name && Equals(x.Type, y.Type);
            }
        }
        #endregion

        #region Conversion

        [TestMethod]
        public void Bonsai_Convert_Implicit()
        {
            var e = (Expression<Func<int, BigInteger>>)(i => i);
            var d = Roundtrip(e) as Expression<Func<int, BigInteger>>;

            Assert.IsNotNull(d);
            Assert.AreEqual(new BigInteger(42), d.Compile()(42));

            var d08 = Roundtrip(e, V08) as Expression<Func<int, BigInteger>>;

            Assert.IsNotNull(d08);
            Assert.AreEqual(new BigInteger(42), d08.Compile()(42));
        }

        [TestMethod]
        public void Bonsai_Convert_Explicit()
        {
            var e = (Expression<Func<BigInteger, int>>)(i => (int)i);
            var d = Roundtrip(e) as Expression<Func<BigInteger, int>>;

            Assert.IsNotNull(d);
            Assert.AreEqual(42, d.Compile()(new BigInteger(42)));

            var d08 = Roundtrip(e, V08) as Expression<Func<BigInteger, int>>;

            Assert.IsNotNull(d08);
            Assert.AreEqual(42, d08.Compile()(new BigInteger(42)));
        }

        [TestMethod]
        public void Bonsai_Convert_Checked1()
        {
            var p = Expression.Parameter(typeof(BigInteger));
            var e = Expression.ConvertChecked(p, typeof(int));

            var d = Roundtrip(e) as UnaryExpression;

            Assert.IsNotNull(d);
            Assert.AreEqual(d.ToCSharpString(), e.ToCSharpString());

            var d08 = Roundtrip(e, V08) as UnaryExpression;

            Assert.IsNotNull(d08);
            Assert.AreEqual(d08.ToCSharpString(), e.ToCSharpString());
        }

        [TestMethod]
        public void Bonsai_Convert_Checked2()
        {
            var p = Expression.Parameter(typeof(long));
            var e = Expression.ConvertChecked(p, typeof(int));

            var d = Roundtrip(e) as UnaryExpression;

            Assert.IsNotNull(d);
            Assert.AreEqual(d.ToCSharpString(), e.ToCSharpString());

            var d08 = Roundtrip(e, V08) as UnaryExpression;

            Assert.IsNotNull(d08);
            Assert.AreEqual(d08.ToCSharpString(), e.ToCSharpString());
        }

        #endregion

        #region Lambda and parameters

        [TestMethod]
        public void Bonsai_Lambda_IdentityFunc()
        {
            var p = Expression.Parameter(typeof(int));
            var l = Expression.Lambda<Func<int, int>>(p, p);

            var r = Roundtrip(l) as LambdaExpression;
            Assert.IsNotNull(r);

            var f = (Func<int, int>)r.Compile();
            Assert.AreEqual(42, f(42));

            var r08 = Roundtrip(l, V08) as LambdaExpression;
            Assert.IsNotNull(r08);

            var f08 = (Func<int, int>)r08.Compile();
            Assert.AreEqual(42, f08(42));
        }

        [TestMethod]
        public void Bonsai_Lambda_LargeFunc()
        {
            var ps = Enumerable.Range(0, 15).Select(i => Expression.Parameter(typeof(int))).ToArray();
            var sum = ps.Cast<Expression>().Aggregate((a, b) => Expression.Add(a, b));
            var l = Expression.Lambda<Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(sum, ps);

            var r = Roundtrip(l) as LambdaExpression;
            Assert.IsNotNull(r);

            var f = (Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>)r.Compile();
            Assert.AreEqual(15 * 16 / 2, f(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15));

            var r08 = Roundtrip(l, V08) as LambdaExpression;
            Assert.IsNotNull(r08);

            var f08 = (Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>)r08.Compile();
            Assert.AreEqual(15 * 16 / 2, f08(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15));
        }

        [TestMethod]
        public void Bonsai_Lambda_Action()
        {
            var l = (Expression<Action<Nopper>>)(n => n.Nop());

            var r = Roundtrip(l) as LambdaExpression;
            Assert.IsNotNull(r);

            var f = (Action<Nopper>)r.Compile();

            var nopper = new Nopper();
            f(nopper);
            Assert.IsTrue(nopper.HasNopped);

            var r08 = Roundtrip(l, V08) as LambdaExpression;
            Assert.IsNotNull(r08);

            var f08 = (Action<Nopper>)r08.Compile();

            var nopper08 = new Nopper();
            f08(nopper08);
            Assert.IsTrue(nopper08.HasNopped);
        }

        private sealed class Nopper
        {
            public void Nop()
            {
                HasNopped = true;
            }

            public bool HasNopped;
        }

        [TestMethod]
        public void Bonsai_Lambda_CustomDelegate()
        {
            var p = Expression.Parameter(typeof(int), "p");
            var l = Expression.Lambda<MyFunc>(p, p);

            var r = Roundtrip(l) as LambdaExpression;
            Assert.IsNotNull(r);

            var f = (MyFunc)r.Compile();
            Assert.AreEqual(42, f(42));

            var r08 = Roundtrip(l, V08) as LambdaExpression;
            Assert.IsNotNull(r08);

            var f08 = (MyFunc)r08.Compile();
            Assert.AreEqual(42, f08(42));
        }

        private delegate int MyFunc(int x);

        [TestMethod]
        public void Bonsai_Lambda_HigherOrderWithConflictingNames()
        {
            var a = Expression.Parameter(typeof(int), "x");
            var b = Expression.Parameter(typeof(int), "x");
            var i = Expression.Lambda<Func<int, int>>(a, b);
            var l = Expression.Lambda<Func<int, Func<int, int>>>(i, a);

            var r = Roundtrip(l) as LambdaExpression;
            Assert.IsNotNull(r);

            var f = (Func<int, Func<int, int>>)r.Compile();
            Assert.AreEqual(2, f(2)(3));

            var r08 = Roundtrip(l, V08) as LambdaExpression;
            Assert.IsNotNull(r08);

            var f08 = (Func<int, Func<int, int>>)r08.Compile();
            Assert.AreEqual(2, f08(2)(3));
        }

        [TestMethod]
        public void Bonsai_Lambda_HigherOrder()
        {
            var a = Expression.Parameter(typeof(int), "a");
            var b = Expression.Parameter(typeof(int), "b");
            var i = Expression.Lambda<Func<int, int>>(a, b);
            var l = Expression.Lambda<Func<int, Func<int, int>>>(i, a);

            var r = Roundtrip(l) as LambdaExpression;
            Assert.IsNotNull(r);

            var f = (Func<int, Func<int, int>>)r.Compile();
            Assert.AreEqual(2, f(2)(3));

            var r08 = Roundtrip(l, V08) as LambdaExpression;
            Assert.IsNotNull(r08);

            var f08 = (Func<int, Func<int, int>>)r08.Compile();
            Assert.AreEqual(2, f08(2)(3));
        }

        [TestMethod]
        public void Bonsai_Lambda_UnnamedParameter()
        {
            var p = Expression.Parameter(typeof(int));
            var l = Expression.Lambda<Func<int, int>>(p, p);

            var r = Roundtrip(l) as LambdaExpression;
            Assert.IsNotNull(r);

            var f = (Func<int, int>)r.Compile();
            Assert.AreEqual(42, f(42));

            var r08 = Roundtrip(l, V08) as LambdaExpression;
            Assert.IsNotNull(r08);

            var f08 = (Func<int, int>)r08.Compile();
            Assert.AreEqual(42, f08(42));
        }

        [TestMethod]
        public void Bonsai_Lambda_Y()
        {
            Expression<Rec<int, int>> f = fib => n => n > 1 ? fib(fib)(n - 1) + fib(fib)(n - 2) : n;

            var r = Roundtrip(f) as LambdaExpression;
            Assert.IsNotNull(r);

            var fibRec = (Rec<int, int>)r.Compile();
            var g = fibRec(fibRec);
            Assert.AreEqual(8, g(6));

            var r08 = Roundtrip(f, V08) as LambdaExpression;
            Assert.IsNotNull(r08);

            var fibRec08 = (Rec<int, int>)r08.Compile();
            var g08 = fibRec08(fibRec08);
            Assert.AreEqual(8, g08(6));
        }

        private delegate Func<A, R> Rec<A, R>(Rec<A, R> r);

        [TestMethod]
        public void Bonsai_Lambda_ImplicitCovariance()
        {
            Expression<Func<int>> f = () => Q.Foo(() => new Foo());

            var e = ((Expression<Func<int>>)Roundtrip(f)).Compile();

            Assert.AreEqual(f.Compile()(), e());

            var e08 = ((Expression<Func<int>>)Roundtrip(f, V08)).Compile();

            Assert.AreEqual(f.Compile()(), e08());
        }

        [TestMethod]
        public void Bonsai_Parameter_Unbound()
        {
            var ps = new[]
            {
                Expression.Parameter(typeof(string), "p"),
                Expression.Parameter(typeof(int), "p"),
            };

            foreach (var p in ps)
            {
                var r = Roundtrip(p) as ParameterExpression;
                Assert.IsNotNull(r);

                Assert.AreEqual(p.Name, r.Name);
                Assert.AreEqual(p.Type, r.Type);

                var r08 = Roundtrip(p, V08) as ParameterExpression;
                Assert.IsNotNull(r08);

                Assert.AreEqual(p.Name, r08.Name);
                Assert.AreEqual(p.Type, r08.Type);
            }
        }

        [TestMethod]
        public void Bonsai_Lambda_DynamicDelegateType()
        {
            var ps = Enumerable.Range(0, 20).Select(i => Expression.Parameter(typeof(int))).ToArray();
            var sum = ps.Cast<Expression>().Aggregate((a, b) => Expression.Add(a, b));
            var l = Expression.Lambda(sum, ps);

            var r = Roundtrip(l) as LambdaExpression;
            Assert.IsNotNull(r);

            var f = r.Compile();
            Assert.AreEqual(20 * 21 / 2, f.DynamicInvoke(Enumerable.Range(1, 20).Cast<object>().ToArray()));
        }

        #endregion

        #region Calls

        [TestMethod]
        public void Bonsai_Call_StaticWithNoParameters()
        {
            var c = Expression.Call(instance: null, typeof(X).GetMethod("A"));

            var r = Roundtrip(c) as MethodCallExpression;
            Assert.IsNotNull(r);

            var f = Expression.Lambda<Func<int>>(r).Compile();
            Assert.AreEqual(42, f());

            var r08 = Roundtrip(c, V08) as MethodCallExpression;
            Assert.IsNotNull(r08);

            var f08 = Expression.Lambda<Func<int>>(r08).Compile();
            Assert.AreEqual(42, f08());
        }

        [TestMethod]
        public void Bonsai_Call_StaticWithParameters()
        {
            var c = Expression.Call(instance: null, typeof(X).GetMethod("B"), Expression.Constant(2));

            var r = Roundtrip(c) as MethodCallExpression;
            Assert.IsNotNull(r);

            var f = Expression.Lambda<Func<int>>(r).Compile();
            Assert.AreEqual(42 * 2, f());

            var r08 = Roundtrip(c, V08) as MethodCallExpression;
            Assert.IsNotNull(r08);

            var f08 = Expression.Lambda<Func<int>>(r08).Compile();
            Assert.AreEqual(42 * 2, f08());
        }

        [TestMethod]
        public void Bonsai_Call_StaticVoidWithNoParameters()
        {
            var c = Expression.Call(instance: null, typeof(X).GetMethod("C"));

            var r = Roundtrip(c) as MethodCallExpression;
            Assert.IsNotNull(r);

            var f = Expression.Lambda<Action>(r).Compile();

            var p = false;
            var h = new Action(() => p = true);
            X.C_ += h;

            f();

            Assert.IsTrue(p);

            X.C_ -= h;

            var r08 = Roundtrip(c, V08) as MethodCallExpression;
            Assert.IsNotNull(r08);

            var f08 = Expression.Lambda<Action>(r08).Compile();

            var p08 = false;
            var h08 = new Action(() => p08 = true);
            X.C_ += h08;

            f08();

            Assert.IsTrue(p08);

            X.C_ -= h08;
        }

        [TestMethod]
        public void Bonsai_Call_StaticVoidWithParameters()
        {
            var c = Expression.Call(instance: null, typeof(X).GetMethod("D"), Expression.Constant(42));

            var r = Roundtrip(c) as MethodCallExpression;
            Assert.IsNotNull(r);

            var f = Expression.Lambda<Action>(r).Compile();

            var p = default(int);
            var h = new Action<int>(x => p = x);
            X.D_ += h;

            f();

            Assert.AreEqual(42, p);

            X.D_ -= h;

            var r08 = Roundtrip(c, V08) as MethodCallExpression;
            Assert.IsNotNull(r08);

            var f08 = Expression.Lambda<Action>(r08).Compile();

            var p08 = default(int);
            var h08 = new Action<int>(x => p08 = x);
            X.D_ += h08;

            f08();

            Assert.AreEqual(42, p08);

            X.D_ -= h08;
        }

        [TestMethod]
        public void Bonsai_Call_StaticGeneric()
        {
            var c = Expression.Call(instance: null, typeof(X).GetMethod("I").MakeGenericMethod(typeof(int)), Expression.Constant(42));

            var r = Roundtrip(c) as MethodCallExpression;
            Assert.IsNotNull(r);

            var f = Expression.Lambda<Func<int>>(r).Compile();
            Assert.AreEqual(42, f());

            var r08 = Roundtrip(c, V08) as MethodCallExpression;
            Assert.IsNotNull(r08);

            var f08 = Expression.Lambda<Func<int>>(r08).Compile();
            Assert.AreEqual(42, f08());
        }

        private sealed class X
        {
            public static int A()
            {
                return 42;
            }

            public static int B(int x)
            {
                return x * 42;
            }

            public static event Action C_;

            public static void C()
            {
                C_();
            }

            public static event Action<int> D_;

            public static void D(int x)
            {
                D_(x);
            }

            public static T I<T>(T x)
            {
                return x;
            }
        }

        [TestMethod]
        public void Bonsai_Call_InstanceVoid()
        {
            Expression<Action<Adder, int>> f = (a, x) => a.Add(x);

            var g = ((Expression<Action<Adder, int>>)Roundtrip(f)).Compile();

            var adder = new Adder();
            g(adder, 3);
            g(adder, 1);
            g(adder, 2);

            Assert.AreEqual(6, adder.Count);

            var g08 = ((Expression<Action<Adder, int>>)Roundtrip(f, V08)).Compile();

            var adder08 = new Adder();
            g08(adder08, 3);
            g08(adder08, 1);
            g08(adder08, 2);

            Assert.AreEqual(6, adder08.Count);
        }

        [TestMethod]
        public void Bonsai_Call_InstanceVoidOverload()
        {
            Expression<Action<Adder, string>> f = (a, x) => a.Add(x);

            var g = ((Expression<Action<Adder, string>>)Roundtrip(f)).Compile();

            var adder = new Adder();
            g(adder, "***");
            g(adder, "*");
            g(adder, "**");

            Assert.AreEqual(6, adder.Count);

            var g08 = ((Expression<Action<Adder, string>>)Roundtrip(f, V08)).Compile();

            var adder08 = new Adder();
            g08(adder08, "***");
            g08(adder08, "*");
            g08(adder08, "**");

            Assert.AreEqual(6, adder08.Count);
        }

        [TestMethod]
        public void Bonsai_Call_InstanceVoidGeneric()
        {
            Expression<Action<Adder, Answer>> f = (a, x) => a.Add(x);

            var g = ((Expression<Action<Adder, Answer>>)Roundtrip(f)).Compile();

            var adder = new Adder();
            g(adder, new Answer());
            g(adder, new Answer());
            g(adder, new Answer());

            Assert.AreEqual(42 * 3, adder.Count);

            var g08 = ((Expression<Action<Adder, Answer>>)Roundtrip(f, V08)).Compile();

            var adder08 = new Adder();
            g08(adder08, new Answer());
            g08(adder08, new Answer());
            g08(adder08, new Answer());

            Assert.AreEqual(42 * 3, adder08.Count);
        }

        [TestMethod]
        public void Bonsai_Call_InstanceReturn()
        {
            Expression<Func<Adder, int, int>> f = (a, x) => a.AddAndRet(x);

            var g = ((Expression<Func<Adder, int, int>>)Roundtrip(f)).Compile();

            var adder = new Adder();
            g(adder, 3);
            g(adder, 1);
            g(adder, 2);

            Assert.AreEqual(10, g(adder, 4));

            var g08 = ((Expression<Func<Adder, int, int>>)Roundtrip(f, V08)).Compile();

            var adder08 = new Adder();
            g08(adder08, 3);
            g08(adder08, 1);
            g08(adder08, 2);

            Assert.AreEqual(10, g08(adder08, 4));
        }

        private sealed class Adder
        {
            public int Count;

            public void Add(int a)
            {
                Count += a;
            }

            public void Add(string s)
            {
                Count += s.Length;
            }

            public void Add<T>(T t)
                where T : IIntifiable
            {
                Count += t.Intify();
            }

            public int AddAndRet(int a)
            {
                Add(a);
                return Count;
            }
        }

        private interface IIntifiable
        {
            int Intify();
        }

        private sealed class Answer : IIntifiable
        {
            public int Intify()
            {
                return 42;
            }
        }

        #endregion

        #region Labels, Loops & Gotos
        [TestMethod]
        public void Bonsai_Goto()
        {
            var labelTarget1 = Expression.Label();
            var labelTarget2 = Expression.Label(typeof(int));
            var labelTarget3 = Expression.Label("foo");
            var labelTarget4 = Expression.Label(typeof(int), "bar");

            var def = Expression.Parameter(typeof(int), "default");
            var pas = Expression.Parameter(typeof(int), "passed");

            var label1 = Expression.Label(labelTarget1);
            var label2 = Expression.Label(labelTarget2, def);
            var label3 = Expression.Label(labelTarget3);
            var label4 = Expression.Label(labelTarget4, def);

            var goto1 = Expression.Goto(labelTarget1);
            var goto2 = Expression.Goto(labelTarget2, pas);
            var goto3 = Expression.Goto(labelTarget3);
            var goto4 = Expression.Goto(labelTarget4, pas);

            var return1 = Expression.Return(labelTarget1);
            var return2 = Expression.Return(labelTarget2, pas);
            var return3 = Expression.Return(labelTarget3);
            var return4 = Expression.Return(labelTarget4, pas);

            var break1 = Expression.Break(labelTarget1);
            var break2 = Expression.Break(labelTarget2, pas);
            var break3 = Expression.Break(labelTarget3);
            var break4 = Expression.Break(labelTarget4, pas);

            var continue1 = Expression.Continue(labelTarget1);
            var continue3 = Expression.Continue(labelTarget3);

            var bs = new[] {
                Expression.Block(label1),
                Expression.Block(label2),
                Expression.Block(label3),
                Expression.Block(label4),
                Expression.Block(goto1, label1),
                Expression.Block(goto2, label2),
                Expression.Block(goto3, label3),
                Expression.Block(goto4, label4),
                Expression.Block(return1, label1),
                Expression.Block(return2, label2),
                Expression.Block(return3, label3),
                Expression.Block(return4, label4),
                Expression.Block(break1, label1),
                Expression.Block(break2, label2),
                Expression.Block(break3, label3),
                Expression.Block(break4, label4),
                Expression.Block(continue1, label1),
                Expression.Block(continue3, label3),
            };

            foreach (var a in bs)
            {
                RoundtripAndAssert(a);
            }
        }

        [TestMethod]
        public void Bonsai_Loop()
        {
            var labelTarget1 = Expression.Label();
            var labelTarget2 = Expression.Label(typeof(int));
            var labelTarget3 = Expression.Label("foo");
            var labelTarget4 = Expression.Label(typeof(int), "bar");

            var simpleBody = Expression.Constant(42);

            var loop1 = Expression.Loop(simpleBody);
            var loop2 = Expression.Loop(simpleBody, labelTarget1);
            var loop3 = Expression.Loop(simpleBody, @break: null, labelTarget1);
            var loop4 = Expression.Loop(simpleBody, labelTarget2, labelTarget3);
            var loop5 = Expression.Loop(simpleBody, labelTarget4, labelTarget3);

            var bs = new[] {
                loop1,
                loop2,
                loop3,
                loop4,
                loop5,
            };

            foreach (var a in bs)
            {
                RoundtripAndAssert(a);
            }
        }

        private static void RoundtripAndAssert(LoopExpression a)
        {
            var r = Roundtrip(a) as LoopExpression;
            Assert.IsNotNull(r);

            Assert.AreEqual(a.Type, r.Type);
            Assert.AreEqual(a.NodeType, r.NodeType);

            Assert.IsTrue(new ExpressionEqualityComparer(() => new GlobalParameterSafeComparator()).Equals(a.Body, r.Body));

            if (a.BreakLabel != null)
            {
                Assert.AreEqual(a.BreakLabel.Name, r.BreakLabel.Name);
            }
            else
            {
                Assert.IsNull(r.BreakLabel);
            }

            if (a.ContinueLabel != null)
            {
                Assert.AreEqual(a.ContinueLabel.Name, r.ContinueLabel.Name);
            }
            else
            {
                Assert.IsNull(r.ContinueLabel);
            }
        }
        #endregion

        #region Switch
        [TestMethod]
        public void Bonsai_Switch()
        {
            var switchValue1 = Expression.Parameter(typeof(int), "i");
            var switchValue2 = Expression.Add(switchValue1, Expression.Constant(1));

            var switchCases1 = new[]
            {
                Expression.SwitchCase(Expression.Empty(), Expression.Constant(1)),
                Expression.SwitchCase(Expression.Empty(), Expression.Constant(2)),
                Expression.SwitchCase(Expression.Empty(), Expression.Constant(3)),
            };

            var switchCases2 = new[]
            {
                Expression.SwitchCase(Expression.Default(typeof(string)), new[] { Expression.Constant(1), Expression.Constant(2), Expression.Constant(3) }),
                Expression.SwitchCase(Expression.Default(typeof(string)), Expression.Constant(4)),
                Expression.SwitchCase(Expression.Default(typeof(string)), new[] { Expression.Constant(5), Expression.Constant(6), Expression.Constant(7) }),
            };

            var bs = new[] {
                Expression.Switch(switchValue1, switchCases1),
                Expression.Switch(switchValue1, Expression.Empty(), switchCases1),
                Expression.Switch(switchValue2, Expression.Default(typeof(string)), switchCases2),
            };

            foreach (var a in bs)
            {
                RoundtripAndAssert(a);
            }
        }

        private static void RoundtripAndAssert(SwitchExpression a)
        {
            var r = Roundtrip(a) as SwitchExpression;
            Assert.IsNotNull(r);

            Assert.AreEqual(a.Type, r.Type);
            Assert.AreEqual(a.NodeType, r.NodeType);
            Assert.AreEqual(a.Comparison, r.Comparison);
            Assert.AreEqual(a.Cases.Count, r.Cases.Count);

            for (int i = 0; i < a.Cases.Count; ++i)
            {
                Assert.IsTrue(new ExpressionEqualityComparer(() => new GlobalParameterSafeComparator()).Equals(a.Cases[i].Body, r.Cases[i].Body));
            }
        }
        #endregion

        #region Invocation

        // See auto-generated file.

        #endregion

        #region Members

        [TestMethod]
        public void Bonsai_Members_Instance_Property()
        {
            Expression<Func<Instancy, string>> f = i => i.Value;

            var e = ((Expression<Func<Instancy, string>>)Roundtrip(f)).Compile();

            var o = new Instancy();
            Assert.AreEqual(e(o), f.Compile()(o));

            var e08 = ((Expression<Func<Instancy, string>>)Roundtrip(f, V08)).Compile();

            var o08 = new Instancy();
            Assert.AreEqual(e08(o08), f.Compile()(o08));
        }

        [TestMethod]
        public void Bonsai_Members_Instance_Field()
        {
            Expression<Func<Instancy, int>> f = i => i._field;

            var e = ((Expression<Func<Instancy, int>>)Roundtrip(f)).Compile();

            var o = new Instancy();
            Assert.AreEqual(e(o), f.Compile()(o));

            var e08 = ((Expression<Func<Instancy, int>>)Roundtrip(f, V08)).Compile();

            var o08 = new Instancy();
            Assert.AreEqual(e08(o08), f.Compile()(o08));
        }

#pragma warning disable CA1822 // Mark static
        private sealed class Instancy
        {
            public int _field = 42;

            public string Value => "Hello";

            public int this[int i] => i;

            public string this[string i, int j] => i + j;
        }
#pragma warning restore CA1822

        [TestMethod]
        public void Bonsai_Members_Static_Property()
        {
            Expression<Func<string>> f = () => Staticy.Value;

            var e = ((Expression<Func<string>>)Roundtrip(f)).Compile();

            Assert.AreEqual(e(), f.Compile()());

            var e08 = ((Expression<Func<string>>)Roundtrip(f, V08)).Compile();

            Assert.AreEqual(e08(), f.Compile()());
        }

        [TestMethod]
        public void Bonsai_Members_Static_Field()
        {
            Expression<Func<int>> f = () => Staticy._field;

            var e = ((Expression<Func<int>>)Roundtrip(f)).Compile();

            Assert.AreEqual(e(), f.Compile()());

            var e08 = ((Expression<Func<int>>)Roundtrip(f, V08)).Compile();

            Assert.AreEqual(e08(), f.Compile()());
        }

        [TestMethod]
        public void Bonsai_Members_Indexed_Property()
        {
            var p = Expression.Parameter(typeof(Instancy));
            var idx1 = Expression.MakeIndex(p, typeof(Instancy).GetProperty("Item", new[] { typeof(int) }), new[] { Expression.Constant(0) });
            var f1 = Expression.Lambda<Func<Instancy, int>>(idx1, p);

            var idx2 = Expression.MakeIndex(p, typeof(Instancy).GetProperty("Item", new[] { typeof(string), typeof(int) }), new[] { Expression.Constant("foo"), Expression.Constant(0) });
            var f2 = Expression.Lambda<Func<Instancy, string>>(idx2, p);

            var e1 = ((Expression<Func<Instancy, int>>)Roundtrip(f1)).Compile();

            var o = new Instancy();
            Assert.AreEqual(e1(o), f1.Compile()(o));

            var e2 = ((Expression<Func<Instancy, string>>)Roundtrip(f2)).Compile();

            Assert.AreEqual(e2(o), f2.Compile()(o));
        }

        [TestMethod]
        public void Bonsai_V08_IndexedProperty_ThrowsNotSupported()
        {
            var p = Expression.Parameter(typeof(Instancy));
            var idx1 = Expression.MakeIndex(p, typeof(Instancy).GetProperty("Item", new[] { typeof(int) }), new[] { Expression.Constant(0) });
            var f1 = Expression.Lambda<Func<Instancy, int>>(idx1, p);

            var idx2 = Expression.MakeIndex(p, typeof(Instancy).GetProperty("Item", new[] { typeof(string), typeof(int) }), new[] { Expression.Constant("foo"), Expression.Constant(0) });
            var f2 = Expression.Lambda<Func<Instancy, string>>(idx2, p);

            Assert.ThrowsException<NotSupportedException>(() => Roundtrip(f1, V08));

            Assert.ThrowsException<NotSupportedException>(() => Roundtrip(f2, V08));
        }

        private static class Staticy
        {
            public static int _field = 42;

            public static string Value => "Hello";
        }

        #endregion

        #region Indexers

        [TestMethod]
        public void Bonsai_ArrayIndex_RankOne()
        {
            var e = (Expression<Func<string[], int, string>>)((ss, i) => /* ArrayIndex */ ss[i]);

            var f = Roundtrip(e) as Expression<Func<string[], int, string>>;
            Assert.IsNotNull(f);

            var o = new[] { "foo", "bar", "qux", "baz" };
            Assert.AreEqual(o[2], f.Compile()(o, 2));
        }

        [TestMethod]
        public void Bonsai_ArrayIndex_HigherRank()
        {
            var e = (Expression<Func<string[,], int, int, string>>)((ss, i, j) => ss[i, j]);

            var f = Roundtrip(e) as Expression<Func<string[,], int, int, string>>;
            Assert.IsNotNull(f);

            var o = new[,] { { "foo", "bar" }, { "qux", "baz" } };
            Assert.AreEqual(o[1, 1], f.Compile()(o, 1, 1));
        }

        [TestMethod]
        public void Bonsai_Indexer()
        {
            var e = (Expression<Func<Indexy, string, int>>)((i, s) => i[s]);

            var f = Roundtrip(e) as Expression<Func<Indexy, string, int>>;
            Assert.IsNotNull(f);

            var o = new Indexy();
            Assert.AreEqual(o["Hello"], f.Compile()(o, "Hello"));
        }

        private sealed class Indexy
        {
#pragma warning disable CA1822 // Mark static. (https://github.com/dotnet/roslyn/issues/50197)
            public int this[string s] => s.Length;
#pragma warning restore CA1822
        }

        #endregion

        #region Default

        [TestMethod]
        public void Bonsai_Default()
        {
            var e = Expression.Default(typeof(string));

            var f = Roundtrip(e) as DefaultExpression;
            Assert.IsNotNull(f);

            Assert.AreEqual(e.Type, f.Type);
        }

        #endregion

        #region Anonymous types

        //Not supported: original type reuse
        /*[TestMethod]
        public void Bonsai_AnonymousType_OriginalTypeReused()
        {
            Expression<Func<object>> x = () => new { x = 1, y = 2 };

            var e = Roundtrip(x.Body);
            Assert.IsNotNull(e);

            var f = Expression.Lambda<Func<object>>(e).Compile();

            var o = f();
            Assert.AreEqual(1, (int)o.GetType().GetProperty("x").GetValue(o, index: null));
            Assert.AreEqual(2, (int)o.GetType().GetProperty("y").GetValue(o, index: null));

            Assert.AreEqual(x.Compile()().GetType(), o.GetType());
        }*/

        [TestMethod]
        public void Bonsai_AnonymousType_KeysReconstructed()
        {
            var anon = RuntimeCompiler.CreateAnonymousType(new[]
            {
                new KeyValuePair<string, Type>("foo", typeof(int)),
                new KeyValuePair<string, Type>("bar", typeof(int))
            }, new string[] { "bar" });

            Assert.IsNotNull(anon.GetProperty("bar"));
            Assert.IsNotNull(anon.GetProperty("foo"));

            var m = Expression.New(anon.GetConstructors().Single(), Expression.Constant(1), Expression.Constant(2));

            var e = Roundtrip(m) as NewExpression;

            var newBar = e.Type.GetProperty("bar");
            var newFoo = e.Type.GetProperty("foo");

            Assert.IsFalse(newBar.CanWrite);
            Assert.IsTrue(newFoo.CanWrite);

            var e08 = Roundtrip(m, V08) as NewExpression;

            var newBar08 = e08.Type.GetProperty("bar");
            var newFoo08 = e08.Type.GetProperty("foo");

            Assert.IsFalse(newBar08.CanWrite);
            Assert.IsTrue(newFoo08.CanWrite);
        }

#pragma warning disable IDE0050 // Convert to tuple. (Tests for anonymous types.)

        [TestMethod]
        public void Bonsai_AnonymousType_TypeReconstructed()
        {
            Expression<Func<object>> x = () => new { x = 1, y = 2 };

            var e = Roundtrip(x.Body);

            Assert.IsNotNull(e);

            var f = Expression.Lambda<Func<object>>(e).Compile();

            var o = f();
            Assert.AreEqual(1, (int)o.GetType().GetProperty("x").GetValue(o, index: null));
            Assert.AreEqual(2, (int)o.GetType().GetProperty("y").GetValue(o, index: null));

            var p = f();
            Assert.AreEqual(o.GetHashCode(), p.GetHashCode());
            Assert.IsTrue(o.Equals(p));

            var q = x.Compile()();
            Assert.AreEqual(q.ToString(), o.ToString());

            Assert.AreNotEqual(q.GetType(), o.GetType());

            Assert.IsFalse(o.GetType().GetProperty("x").CanWrite);
            Assert.IsFalse(o.GetType().GetProperty("y").CanWrite);

            var e08 = Roundtrip(x.Body, V08);

            Assert.IsNotNull(e08);

            var f08 = Expression.Lambda<Func<object>>(e08).Compile();

            var o08 = f08();
            Assert.AreEqual(1, (int)o08.GetType().GetProperty("x").GetValue(o08, index: null));
            Assert.AreEqual(2, (int)o08.GetType().GetProperty("y").GetValue(o08, index: null));

            var p08 = f08();
            Assert.AreEqual(o08.GetHashCode(), p08.GetHashCode());
            Assert.IsTrue(o08.Equals(p08));

            var q08 = x.Compile()();
            Assert.AreEqual(q08.ToString(), o08.ToString());

            Assert.AreNotEqual(q08.GetType(), o08.GetType());

            Assert.IsFalse(o08.GetType().GetProperty("x").CanWrite);
            Assert.IsFalse(o08.GetType().GetProperty("y").CanWrite);
        }

#pragma warning restore IDE0050

        // TODO: implement visual basic tests
        /*[TestMethod]
        public void Bonsai_AnonymousType_TypeReconstructed_VisualBasic()
        {
            var x = ((LambdaExpression)VisualBasicModule.GetAnonymousTypeWithKeysExpression()).Body;

            var s = ser.Serialize(x);

            var y = s.ToString().Replace("OriginalName", "_OriginalName");
            var t = Nuqleon.Json.Expressions.Expression.Parse(y);

            var e = ser.Deserialize(t);
            Assert.IsNotNull(e);

            var o = e.Evaluate();
            Assert.AreEqual(42, (int)o.GetType().GetProperty("Bar").GetValue(o, index: null));
            Assert.AreEqual(43, (int)o.GetType().GetProperty("Foo").GetValue(o, index: null));

            var p = e.Evaluate();
            Assert.AreEqual(o.GetHashCode(), p.GetHashCode());
            Assert.IsTrue(o.Equals(p));

            var q = x.Evaluate();
            Assert.AreEqual(q.ToString(), o.ToString());
            Assert.AreNotEqual(q.GetType(), o.GetType());

            Assert.IsFalse(o.GetType().GetProperty("Bar").CanWrite);
            Assert.IsTrue(o.GetType().GetProperty("Foo").CanWrite);
        }*/

        #endregion

        #region Record types

        [TestMethod]
        public void Bonsai_RecordType_WithValueEqualitySemantics()
        {
            var rec = RuntimeCompiler.CreateRecordType(new[]
            {
                new KeyValuePair<string, Type>("bar", typeof(int))
            }, valueEquality: true);

            var bar = rec.GetProperty("bar");

            var m = Expression.MemberInit(Expression.New(rec), Expression.Bind(bar, Expression.Constant(42)));

            var e = Roundtrip(m) as MemberInitExpression;

            Assert.IsNotNull(e);

            Assert.IsNotNull(e.NewExpression.Type.GetMethod("GetHashCode", BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance));
        }

        [TestMethod]
        public void Bonsai_RecordType_WithoutValueEqualitySemantics()
        {
            var rec = RuntimeCompiler.CreateRecordType(new[]
            {
                new KeyValuePair<string, Type>("bar", typeof(int))
            }, valueEquality: false);

            var bar = rec.GetProperty("bar");

            var m = Expression.MemberInit(Expression.New(rec), Expression.Bind(bar, Expression.Constant(42)));

            var e = Roundtrip(m) as MemberInitExpression;

            Assert.IsNotNull(e);

            Assert.IsNull(e.NewExpression.Type.GetMethod("GetHashCode", BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance));
        }

        [TestMethod]
        public void Bonsai_RecordType_TypeReconstructed()
        {
            var rec = RuntimeCompiler.CreateRecordType(new[]
            {
                new KeyValuePair<string, Type>("bar", typeof(int))
            }, valueEquality: true);

            var bar = rec.GetProperty("bar");

            var m = Expression.MemberInit(Expression.New(rec), Expression.Bind(bar, Expression.Constant(42)));

            var objectSerializer = new ObjectSerializer();
            var serializer = new CustomBonsaiExpressionSerializer(objectSerializer.GetSerializer, objectSerializer.GetDeserializer, new Version(0, 9));

            var slimIn = serializer.Lift(m);
            var s = serializer.Serialize(slimIn);
            var slimOut = serializer.Deserialize(s);
            var e = serializer.Reduce(slimOut);
            Assert.AreEqual(slimIn.ToString(), slimOut.ToString());

            Assert.IsNotNull(e);

            var o = e.Evaluate();

            Assert.AreEqual(42, (int)o.GetType().GetProperty("bar").GetValue(o, index: null));

            var p = m.Evaluate();
            Assert.AreEqual(o.GetHashCode(), p.GetHashCode());
            Assert.AreEqual(o.ToString(), p.ToString());
            Assert.AreNotEqual(o.GetType(), p.GetType());
        }

        [TestMethod]
        public void Bonsai_V08_RecordType_ThrowsNotSupported()
        {
            var rec = RuntimeCompiler.CreateRecordType(new[]
            {
                new KeyValuePair<string, Type>("bar", typeof(int))
            }, valueEquality: true);

            var bar = rec.GetProperty("bar");

            var m = Expression.MemberInit(Expression.New(rec), Expression.Bind(bar, Expression.Constant(42)));

            Assert.ThrowsException<NotSupportedException>(() => Roundtrip(m, V08));
        }

        #endregion

        #region Exceptions

        [TestMethod]
        public void Bonsai_ExceptionHandling()
        {
            var ex1 = Expression.Parameter(typeof(Exception), "ex1");
            var ex2 = Expression.Parameter(typeof(ArgumentException), "ex2");
            var ex3 = Expression.Parameter(typeof(InvalidOperationException), "ex3");
            var act = Expression.Parameter(typeof(Action), "act");
            var nop = Expression.Invoke(act);

            var bs = new[] {
                Expression.TryCatch(Expression.Empty(), Expression.Catch(typeof(Exception), Expression.Empty())),
                Expression.TryCatch(Expression.Empty(), Expression.Catch(ex1, Expression.Empty())),
                Expression.TryCatchFinally(Expression.Empty(), nop, Expression.Catch(ex3, Expression.Empty()), Expression.Catch(ex2, Expression.Empty()), Expression.Catch(ex1, Expression.Empty())),
                Expression.TryFault(nop, Expression.Empty()),
                Expression.TryCatch(Expression.Empty(), Expression.MakeCatchBlock(typeof(Exception), ex1, Expression.Empty(), Expression.Default(typeof(bool)))),
            };

            foreach (var a in bs)
            {
                RoundtripAndAssert(a);
            }
        }

        private static void RoundtripAndAssert(TryExpression a)
        {
            var r = Roundtrip(a) as TryExpression;
            Assert.IsNotNull(r);

            Assert.AreEqual(a.Type, r.Type);
            Assert.AreEqual(a.NodeType, r.NodeType);

            Assert.IsTrue(new ExpressionEqualityComparer(() => new GlobalParameterSafeComparator()).Equals(a.Body, r.Body));

            Assert.AreEqual(a.Handlers.Count, r.Handlers.Count);
            for (int i = 0; i < a.Handlers.Count; ++i)
            {
                var handlerA = a.Handlers[i];
                var handlerR = r.Handlers[i];

                Assert.AreEqual(handlerA.Test, handlerR.Test);
                Assert.IsTrue(new ExpressionEqualityComparer(() => new GlobalParameterSafeComparator()).Equals(handlerA.Body, handlerR.Body));
                Assert.IsTrue(new ExpressionEqualityComparer(() => new GlobalParameterSafeComparator()).Equals(handlerA.Variable, handlerR.Variable));
            }
        }

        [TestMethod]
        public void Exceptions_Handling()
        {
            var body = Expression.Parameter(typeof(Action));
            var logi = Expression.Parameter(typeof(Action<int>));
            var loge = Expression.Parameter(typeof(Action<Exception>));

            var ioe = Expression.Parameter(typeof(InvalidOperationException));
            var ae = Expression.Parameter(typeof(ArgumentException));
            var e = Expression.Parameter(typeof(Exception));

            var f = Expression.Lambda<Action<Action, Action<int>, Action<Exception>>>(
                Expression.TryCatchFinally(
                    Expression.TryCatch(
                        Expression.Block(
                            Expression.Invoke(body),
                            Expression.Invoke(logi, Expression.Constant(0))
                        ),
                        Expression.Catch(ioe,
                            Expression.Block(
                                Expression.Invoke(loge, ioe),
                                Expression.Invoke(logi, Expression.Constant(1))
                            )
                        )
                    ),
                    Expression.Block(
                        Expression.Invoke(logi, Expression.Constant(2))
                    ),
                    Expression.Catch(ae,
                        Expression.Block(
                            Expression.Invoke(loge, ae),
                            Expression.Invoke(logi, Expression.Constant(3))
                        )
                    ),
                    Expression.Catch(e,
                        Expression.Block(
                            Expression.Invoke(loge, e),
                            Expression.Invoke(logi, Expression.Constant(4))
                        )
                    )
                ),
                body, logi, loge
            );

            var fo = f.Compile();

            var g = (Expression<Action<Action, Action<int>, Action<Exception>>>)Roundtrip(f);

            var fr = g.Compile();

            var exs = new Exception[]
            {
                new InvalidOperationException(),
                new ArgumentException("Booh!", "bar"),
                new ArgumentException("Booh!", "foo"),
                new DivideByZeroException()
            };

            var actions = new Action[]
            {
                () => { },
                () => { throw exs[0]; },
                () => { throw exs[1]; },
                () => { throw exs[2]; },
                () => { throw exs[3]; },
            };

            foreach (var action in actions)
            {
                var seqo = new List<int>();
                var logo = new List<Exception>();
                fo(action, seqo.Add, logo.Add);

                var seqr = new List<int>();
                var logr = new List<Exception>();
                fr(action, seqr.Add, logr.Add);

                Assert.IsTrue(Enumerable.SequenceEqual(seqo, seqr));
                Assert.IsTrue(Enumerable.SequenceEqual(logo, logr));
            }
        }

        [TestMethod]
        public void Exceptions_Handling_FilterAndFault()
        {
            var body = Expression.Parameter(typeof(Action));
            var logi = Expression.Parameter(typeof(Action<int>));
            var loge = Expression.Parameter(typeof(Action<Exception>));

            var ioe = Expression.Parameter(typeof(InvalidOperationException));
            var ae = Expression.Parameter(typeof(ArgumentException));
            var e = Expression.Parameter(typeof(Exception));

            //
            // Fault handlers and filtered exceptions are not supported by .Compile().
            //
            var f = Expression.Lambda<Action<Action, Action<int>, Action<Exception>>>(
                Expression.TryFault(
                    Expression.TryFinally(
                        Expression.TryCatch(
                            Expression.TryFinally(
                                Expression.TryFault(
                                    Expression.Block(
                                        Expression.Invoke(body),
                                        Expression.Invoke(logi, Expression.Constant(0))
                                    ),
                                    Expression.Invoke(logi, Expression.Constant(1))
                                ),
                                Expression.Invoke(logi, Expression.Constant(2))
                            ),
                            Expression.Catch(ioe,
                                Expression.Block(
                                    Expression.Invoke(loge, ioe),
                                    Expression.Invoke(logi, Expression.Constant(3))
                                )
                            ),
                            Expression.Catch(ae,
                                Expression.Block(
                                    Expression.Invoke(loge, ae),
                                    Expression.Invoke(logi, Expression.Constant(4))
                                ),
                                Expression.Equal(
                                    Expression.Property(ae, "ParamName"),
                                    Expression.Constant("bar")
                                )
                            ),
                            Expression.Catch(e,
                                Expression.Block(
                                    Expression.Invoke(loge, e),
                                    Expression.Invoke(logi, Expression.Constant(5))
                                )
                            )
                        ),
                        Expression.Invoke(logi, Expression.Constant(6))
                    ),
                  Expression.Invoke(logi, Expression.Constant(7))
                ),
                body, logi, loge
            );

            var g = (Expression<Action<Action, Action<int>, Action<Exception>>>)Roundtrip(f);

            var res = new ExpressionEqualityComparer().Equals(f, g);

            Assert.IsTrue(res);

#if CANCOMPILEFILTERFAULT
            var fo = f.Compile();
            var fr = g.Compile();

            var exs = new Exception[]
            {
                new InvalidOperationException(),
                new ArgumentException("Booh!", "bar"),
                new ArgumentException("Booh!", "foo"),
                new DivideByZeroException()
            };

            var actions = new Action[]
            {
                () => { },
                () => { throw exs[0]; },
                () => { throw exs[1]; },
                () => { throw exs[2]; },
                () => { throw exs[3]; },
            };

            foreach (var action in actions)
            {
                var seqo = new List<int>();
                var logo = new List<Exception>();
                fo(action, seqo.Add, logo.Add);

                var seqr = new List<int>();
                var logr = new List<Exception>();
                fr(action, seqr.Add, logr.Add);

                Assert.IsTrue(Enumerable.SequenceEqual(seqo, seqr));
                Assert.IsTrue(Enumerable.SequenceEqual(logo, logr));
            }
#endif
        }

        #endregion

        #region Dynamic

        //Not supported
        /*[TestMethod]
        public void Dynamic_BinaryOperation()
        {
            var op = CSharpDynamic.Binder.BinaryOperation(CSharpDynamic.CSharpBinderFlags.None, ExpressionType.Add, typeof(object), new[]
            {
                CSharpDynamic.CSharpArgumentInfo.Create(CSharpDynamic.CSharpArgumentInfoFlags.None, name: null),
                CSharpDynamic.CSharpArgumentInfo.Create(CSharpDynamic.CSharpArgumentInfoFlags.None, name: null),
            });

            var today = new DateTime(2013, 1, 2);
            var twelf = TimeSpan.FromHours(12);

            var e = Expression.Dynamic(op, typeof(object), Expression.Constant(today), Expression.Constant(twelf));
            var d = (DynamicExpression)Roundtrip(e);

            var f = Expression.Lambda<Func<DateTime>>(Expression.Convert(d, typeof(DateTime)));
            Assert.AreEqual(today + twelf, f.Compile()());
        }

        [TestMethod]
        public void Dynamic_BinaryOperation_Logical()
        {
            var op = CSharpDynamic.Binder.BinaryOperation(CSharpDynamic.CSharpBinderFlags.BinaryOperationLogical, ExpressionType.And, typeof(Tests) /* Convertible visibility * /, new[]
            {
                CSharpDynamic.CSharpArgumentInfo.Create(CSharpDynamic.CSharpArgumentInfoFlags.None, name: null),
                CSharpDynamic.CSharpArgumentInfo.Create(CSharpDynamic.CSharpArgumentInfoFlags.None, name: null),
            });

            var a = Expression.Parameter(typeof(object));
            var b = Expression.Parameter(typeof(object));
            var e = Expression.Lambda<Func<object, object, object>>(Expression.Dynamic(op, typeof(object), a, b), a, b);
            var d = (Expression<Func<object, object, object>>)Roundtrip(e);

            var f = d.Compile();

            var x = new Logi();
            var y = new Logi();
            var r = f(x, y);

            Assert.AreSame(x, x && y);
            Assert.AreSame(x, r);
        }

        private sealed class Logi
        {
            public static bool operator true(Logi l)
            {
                return true;
            }

            public static bool operator false(Logi l)
            {
                return true;
            }

            public static Logi operator &(Logi a, Logi b)
            {
                return new Logi();
            }

            public static Logi operator |(Logi a, Logi b)
            {
                return new Logi();
            }
        }

        [TestMethod, ExpectedException(typeof(OverflowException))]
        public void Dynamic_BinaryOperation_Checked()
        {
            var op = CSharpDynamic.Binder.BinaryOperation(CSharpDynamic.CSharpBinderFlags.CheckedContext, ExpressionType.Add, typeof(object), new[]
            {
                CSharpDynamic.CSharpArgumentInfo.Create(CSharpDynamic.CSharpArgumentInfoFlags.None, name: null),
                CSharpDynamic.CSharpArgumentInfo.Create(CSharpDynamic.CSharpArgumentInfoFlags.None, name: null),
            });

            var max = int.MaxValue;
            var one = 1;

            var e = Expression.Dynamic(op, typeof(object), Expression.Constant(max), Expression.Constant(one));
            var d = (DynamicExpression)Roundtrip(e);

            var f = Expression.Lambda<Func<int>>(Expression.Convert(d, typeof(int)));
            var overflow = f.Compile()();
        }

        [TestMethod]
        public void Dynamic_Convert_Explicit_BuiltIn()
        {
            var op = CSharpDynamic.Binder.Convert(CSharpDynamic.CSharpBinderFlags.ConvertExplicit, typeof(Guid), typeof(object));

            var guid = Guid.NewGuid();

            var e = Expression.Dynamic(op, typeof(Guid), Expression.Constant(guid, typeof(object)));
            var d = (DynamicExpression)Roundtrip(e);

            var f = Expression.Lambda<Func<Guid>>(d);
            Assert.AreEqual(guid, f.Compile()());
        }

        [TestMethod]
        public void Dynamic_Convert_Explicit_Custom()
        {
            var op = CSharpDynamic.Binder.Convert(CSharpDynamic.CSharpBinderFlags.ConvertExplicit, typeof(string), typeof(Tests) /* Convertible visibility * /);

            var p = Expression.Parameter(typeof(Convertible));
            var e = Expression.Lambda<Func<Convertible, string>>(Expression.Dynamic(op, typeof(string), Expression.Convert(p, typeof(object))), p);
            var d = Roundtrip(e);

            var f = (Expression<Func<Convertible, string>>)d;
            var c = new Convertible();
            Assert.AreEqual((string)c, f.Compile()(c));
        }

        [TestMethod]
        public void Dynamic_Convert_Implicit_BuiltIn()
        {
            var op = CSharpDynamic.Binder.Convert(CSharpDynamic.CSharpBinderFlags.None, typeof(long), typeof(object));

            var value = 42;

            var e = Expression.Dynamic(op, typeof(long), Expression.Constant(value, typeof(object)));
            var d = (DynamicExpression)Roundtrip(e);

            var f = Expression.Lambda<Func<long>>(d);
            Assert.AreEqual((long)value, f.Compile()());
        }

        [TestMethod]
        public void Dynamic_Convert_Implicit_Custom()
        {
            var op = CSharpDynamic.Binder.Convert(CSharpDynamic.CSharpBinderFlags.None, typeof(int), typeof(Tests) /* Convertible visibility * /);

            var p = Expression.Parameter(typeof(Convertible));
            var e = Expression.Lambda<Func<Convertible, int>>(Expression.Dynamic(op, typeof(int), Expression.Convert(p, typeof(object))), p);
            var d = Roundtrip(e);

            var f = (Expression<Func<Convertible, int>>)d;
            var c = new Convertible();
            Assert.AreEqual((int)c, f.Compile()(c));
        }

        private sealed class Convertible
        {
            public static explicit operator string(Convertible c)
            {
                return "Boo!";
            }

            public static implicit operator int(Convertible c)
            {
                return 42;
            }
        }

        [TestMethod]
        public void Dynamic_GetIndex()
        {
            var op = CSharpDynamic.Binder.GetIndex(CSharpDynamic.CSharpBinderFlags.None, typeof(object), new[]
            {
                CSharpDynamic.CSharpArgumentInfo.Create(CSharpDynamic.CSharpArgumentInfoFlags.None, name: null),
                CSharpDynamic.CSharpArgumentInfo.Create(CSharpDynamic.CSharpArgumentInfoFlags.None, name: null),
            });

            var dict = new Dictionary<string, int>
            {
                { "Bart", 10 },
                { "Homer", 36 },
            };

            var o = Expression.Parameter(typeof(object));
            var k = Expression.Parameter(typeof(string));
            var e = Expression.Lambda<Func<object, string, int>>(Expression.Convert(Expression.Dynamic(op, typeof(object), o, k), typeof(int)), o, k);
            var d = Roundtrip(e);

            var f = ((Expression<Func<object, string, int>>)d).Compile();
            Assert.AreEqual(dict["Bart"], f(dict, "Bart"));
            Assert.AreEqual(dict["Homer"], f(dict, "Homer"));
        }

        [TestMethod]
        public void Dynamic_GetMember()
        {
            var op = CSharpDynamic.Binder.GetMember(CSharpDynamic.CSharpBinderFlags.None, "FullName", typeof(object), new[]
            {
                CSharpDynamic.CSharpArgumentInfo.Create(CSharpDynamic.CSharpArgumentInfoFlags.None, name: null),
            });

            var type = typeof(AppDomain);

            var e = Expression.Dynamic(op, typeof(object), Expression.Constant(type));
            var d = (DynamicExpression)Roundtrip(e);

            var f = Expression.Lambda<Func<string>>(Expression.Convert(d, typeof(string)));
            Assert.AreEqual(type.FullName, f.Compile()());
        }

        [TestMethod]
        public void Dynamic_Invoke()
        {
            var op = CSharpDynamic.Binder.Invoke(CSharpDynamic.CSharpBinderFlags.None, typeof(object), new[]
            {
                CSharpDynamic.CSharpArgumentInfo.Create(CSharpDynamic.CSharpArgumentInfoFlags.None, name: null),
                CSharpDynamic.CSharpArgumentInfo.Create(CSharpDynamic.CSharpArgumentInfoFlags.None, name: null),
            });

            var func = new Func<string, int>(str => str.Length);
            var arg = "Hello";

            var g = Expression.Parameter(typeof(object));
            var s = Expression.Parameter(typeof(string));
            var e = Expression.Lambda<Func<object, string, object>>(Expression.Dynamic(op, typeof(object), g, s), g, s);
            var d = Roundtrip(e);

            var f = (Expression<Func<object, string, object>>)d;
            Assert.AreEqual(func(arg), (int)f.Compile()(func, arg));
        }

        [TestMethod]
        public void Dynamic_InvokeConstructor()
        {
            var op = CSharpDynamic.Binder.InvokeConstructor(CSharpDynamic.CSharpBinderFlags.None, typeof(object), new[]
            {
                CSharpDynamic.CSharpArgumentInfo.Create(CSharpDynamic.CSharpArgumentInfoFlags.None, name: null),
                CSharpDynamic.CSharpArgumentInfo.Create(CSharpDynamic.CSharpArgumentInfoFlags.None, name: null),
            });

            var msg = "Oops!";

            var t = Expression.Parameter(typeof(Type));
            var s = Expression.Parameter(typeof(string));
            var e = Expression.Lambda<Func<Type, string, object>>(Expression.Dynamic(op, typeof(object), t, s), t, s);
            var d = Roundtrip(e);

            var f = (Expression<Func<Type, string, object>>)d;
            Assert.AreEqual(msg, ((Exception)f.Compile()(typeof(InvalidOperationException), msg)).Message);
        }

        [TestMethod]
        public void Dynamic_InvokeMember()
        {
            var op = CSharpDynamic.Binder.InvokeMember(CSharpDynamic.CSharpBinderFlags.None, "Substring", Type.EmptyTypes, typeof(object), new[]
            {
                CSharpDynamic.CSharpArgumentInfo.Create(CSharpDynamic.CSharpArgumentInfoFlags.None, name: null),
                CSharpDynamic.CSharpArgumentInfo.Create(CSharpDynamic.CSharpArgumentInfoFlags.None, name: null),
                CSharpDynamic.CSharpArgumentInfo.Create(CSharpDynamic.CSharpArgumentInfoFlags.None, name: null),
            });

            var data = "Hello, world!";
            var startIndex = 2;
            var length = 5;

            var e = Expression.Dynamic(op, typeof(object), Expression.Constant(data), Expression.Constant(startIndex), Expression.Constant(length));
            var d = (DynamicExpression)Roundtrip(e);

            var f = Expression.Lambda<Func<string>>(Expression.Convert(d, typeof(string)));
            Assert.AreEqual(data.Substring(startIndex, length), f.Compile()());
        }

        [TestMethod]
        public void Dynamic_InvokeMember_NamedArguments()
        {
            var op = CSharpDynamic.Binder.InvokeMember(CSharpDynamic.CSharpBinderFlags.None, "Substring", Type.EmptyTypes, typeof(object), new[]
            {
                CSharpDynamic.CSharpArgumentInfo.Create(CSharpDynamic.CSharpArgumentInfoFlags.None, name: null),
                CSharpDynamic.CSharpArgumentInfo.Create(CSharpDynamic.CSharpArgumentInfoFlags.NamedArgument, "length"),
                CSharpDynamic.CSharpArgumentInfo.Create(CSharpDynamic.CSharpArgumentInfoFlags.NamedArgument, "startIndex"),
            });

            var data = "Hello, world!";
            var startIndex = 2;
            var length = 5;

            var e = Expression.Dynamic(op, typeof(object), Expression.Constant(data), Expression.Constant(length), Expression.Constant(startIndex));
            var d = (DynamicExpression)Roundtrip(e);

            var f = Expression.Lambda<Func<string>>(Expression.Convert(d, typeof(string)));
            Assert.AreEqual(data.Substring(startIndex, length), f.Compile()());
        }

        [TestMethod]
        public void Dynamic_IsEvent()
        {
            var op = CSharpDynamic.Binder.IsEvent(CSharpDynamic.CSharpBinderFlags.None, "TypeResolve", typeof(object));

            var o = Expression.Parameter(typeof(object));
            var e = Expression.Lambda<Func<object, bool>>(Expression.Dynamic(op, typeof(bool), o), o);
            var d = Roundtrip(e);

            var f = ((Expression<Func<object, bool>>)d).Compile();
            Assert.IsTrue(f(AppDomain.CurrentDomain));
            Assert.IsFalse(f(new NadaTypeResolve()));
        }

        private sealed class NadaTypeResolve
        {
            public int TypeResolve { get; set; }
        }

        [TestMethod]
        public void Dynamic_SetIndex()
        {
            var op = CSharpDynamic.Binder.SetIndex(CSharpDynamic.CSharpBinderFlags.None, typeof(object), new[]
            {
                CSharpDynamic.CSharpArgumentInfo.Create(CSharpDynamic.CSharpArgumentInfoFlags.None, name: null),
                CSharpDynamic.CSharpArgumentInfo.Create(CSharpDynamic.CSharpArgumentInfoFlags.None, name: null),
                CSharpDynamic.CSharpArgumentInfo.Create(CSharpDynamic.CSharpArgumentInfoFlags.None, name: null),
            });

            var dict = new Dictionary<string, int>
            {
                { "Bart", 10 },
                { "Homer", 36 },
            };

            var o = Expression.Parameter(typeof(object));
            var k = Expression.Parameter(typeof(string));
            var v = Expression.Parameter(typeof(int));
            var e = Expression.Lambda<Action<object, string, int>>(Expression.Dynamic(op, typeof(object), o, k, v), o, k, v);
            var d = Roundtrip(e);

            var f = ((Expression<Action<object, string, int>>)d).Compile();
            f(dict, "Bart", 12);
            Assert.AreEqual(dict["Bart"], 12);
            f(dict, "Homer", 34);
            Assert.AreEqual(dict["Homer"], 34);
        }

        [TestMethod]
        public void Dynamic_SetMember()
        {
            var op = CSharpDynamic.Binder.SetMember(CSharpDynamic.CSharpBinderFlags.None, "Foo", typeof(Tests) /* MyMemberish visibility * /, new[]
            {
                CSharpDynamic.CSharpArgumentInfo.Create(CSharpDynamic.CSharpArgumentInfoFlags.None, name: null),
                CSharpDynamic.CSharpArgumentInfo.Create(CSharpDynamic.CSharpArgumentInfoFlags.None, name: null),
            });

            var obj = new MyMemberish();

            var o = Expression.Parameter(typeof(object));
            var e = Expression.Lambda<Action<object>>(Expression.Dynamic(op, typeof(object), o, Expression.Constant(42)), o);
            var d = Roundtrip(e);

            var f = ((Expression<Action<object>>)d).Compile();
            f(obj);
            Assert.AreEqual(42, obj.Foo);
        }

        private sealed class MyMemberish
        {
            public int Foo { get; set; }
        }

        [TestMethod]
        public void Dynamic_UnaryOperation()
        {
            var op = CSharpDynamic.Binder.UnaryOperation(CSharpDynamic.CSharpBinderFlags.None, ExpressionType.Negate, typeof(object), new[]
            {
                CSharpDynamic.CSharpArgumentInfo.Create(CSharpDynamic.CSharpArgumentInfoFlags.None, name: null),
            });

            var bigOne = new BigInteger(1234567890);

            var p = Expression.Parameter(typeof(BigInteger));
            var e = Expression.Lambda<Func<BigInteger, object>>(Expression.Dynamic(op, typeof(object), p), p);
            var d = (Expression<Func<BigInteger, object>>)Roundtrip(e);

            var f = Expression.Lambda<Func<BigInteger>>(Expression.Convert(Expression.Invoke(d, Expression.Constant(bigOne)), typeof(BigInteger)));
            Assert.AreEqual(-bigOne, f.Compile()());
        }

        [TestMethod, ExpectedException(typeof(OverflowException))]
        public void Dynamic_UnaryOperation_Checked()
        {
            var op = CSharpDynamic.Binder.UnaryOperation(CSharpDynamic.CSharpBinderFlags.CheckedContext, ExpressionType.Negate, typeof(object), new[]
            {
                CSharpDynamic.CSharpArgumentInfo.Create(CSharpDynamic.CSharpArgumentInfoFlags.None, name: null),
            });

            var min = int.MinValue;

            var e = Expression.Dynamic(op, typeof(object), Expression.Constant(min));
            var d = (DynamicExpression)Roundtrip(e);

            var f = Expression.Lambda<Func<int>>(Expression.Convert(d, typeof(int)));
            var overflow = f.Compile()();
        }*/

        #endregion

        #region Conditional

        [TestMethod]
        public void Bonsai_Conditional()
        {
            Expression<Func<bool, int>> f = b => b ? 3 : 4;

            var e = ((Expression<Func<bool, int>>)Roundtrip(f)).Compile();

            Assert.AreEqual(4, e(false));
            Assert.AreEqual(3, e(true));

            var e08 = ((Expression<Func<bool, int>>)Roundtrip(f, V08)).Compile();

            Assert.AreEqual(4, e08(false));
            Assert.AreEqual(3, e08(true));
        }

        [TestMethod]
        public void Bonsai_Conditional_WithType()
        {
            var c = Expression.Condition(Expression.Constant(true, typeof(bool)), Expression.New(typeof(B)), Expression.New(typeof(C)), typeof(A));
            var r = (ConditionalExpression)Roundtrip(c);

            var ce = c.Evaluate();
            var re = r.Evaluate();
            Assert.AreEqual(ce.GetType(), re.GetType());
            Assert.AreEqual(c.Type, r.Type);
        }

        private class A { }

        private class B : A { }

        private sealed class C : B { }

        #endregion

        #region Object creation and initialization

        [TestMethod]
        public void Bonsai_Init_Arrays()
        {
            Expression<Func<int[]>> f = () => new[] { 1, 2, 3 };

            var e = ((Expression<Func<int[]>>)Roundtrip(f)).Compile();

            Assert.IsTrue(e().SequenceEqual(f.Compile()()));

            var e08 = ((Expression<Func<int[]>>)Roundtrip(f, V08)).Compile();

            Assert.IsTrue(e08().SequenceEqual(f.Compile()()));
        }

        [TestMethod]
        public void Bonsai_Init_List()
        {
            Expression<Func<List<int>>> f = () => new List<int> { 1, 2, 3 };

            var e = ((Expression<Func<List<int>>>)Roundtrip(f)).Compile();

            Assert.IsTrue(e().SequenceEqual(f.Compile()()));

            var e08 = ((Expression<Func<List<int>>>)Roundtrip(f, V08)).Compile();

            Assert.IsTrue(e08().SequenceEqual(f.Compile()()));
        }

        [TestMethod]
        public void Bonsai_Init_Object()
        {
            var e = (Expression<Func<Qux>>)(() => new Qux(42) { Baz = "Hello", Bar = { Zuz = 24 }, Foos = { 1, 2, 3 } });

            var f = (Expression<Func<Qux>>)Roundtrip(e);

            var q = f.Compile()();
            Assert.AreEqual(42, q.X);
            Assert.AreEqual("Hello", q.Baz);
            Assert.AreEqual(24, q.Bar.Zuz);
            Assert.IsTrue(q.Foos.SequenceEqual(new[] { 1, 2, 3 }));

            var f08 = (Expression<Func<Qux>>)Roundtrip(e, V08);

            var q08 = f08.Compile()();
            Assert.AreEqual(42, q08.X);
            Assert.AreEqual("Hello", q08.Baz);
            Assert.AreEqual(24, q08.Bar.Zuz);
            Assert.IsTrue(q08.Foos.SequenceEqual(new[] { 1, 2, 3 }));
        }

        [TestMethod]
        public void Bonsai_New_ValueType_DefaultConstructor()
        {
            var f = Expression.Lambda(Expression.New(typeof(int)));

            var e = (Expression<Func<int>>)Roundtrip(f);

            Assert.IsTrue(new ExpressionEqualityComparer().Equals(f, e));

            var e08 = (Expression<Func<int>>)Roundtrip(f, new Version(0, 8));

            Assert.IsTrue(new ExpressionEqualityComparer().Equals(f, e08));
        }

        [TestMethod]
        public void Bonsai_New_Array_Bounds()
        {
            var e = Expression.NewArrayBounds(typeof(int), Expression.Constant(1));

            var d = Roundtrip(e) as NewArrayExpression;

            Assert.IsNotNull(d);
            Assert.AreEqual(d.ToCSharpString(), e.ToCSharpString());

            var d08 = Roundtrip(e, V08) as NewArrayExpression;

            Assert.IsNotNull(d08);
            Assert.AreEqual(d08.ToCSharpString(), e.ToCSharpString());
        }

        private sealed class Qux
        {
            public Qux(int x)
            {
                X = x;
                Foos = new List<int>();
                Bar = new Bar();
            }

            public int X
            {
                get;
                private set;
            }

            public string Baz
            {
                get;
                set;
            }

            public Bar Bar
            {
                get;
                private set;
            }

            public List<int> Foos
            {
                get;
                private set;
            }
        }

        private sealed class Bar
        {
            public int Zuz = 1;
        }

        #endregion

        #region Closure support

        //Not supported: closures
        /*[TestMethod]
        public void Bonsai_Closures()
        {
            var x = 42;
            var y = 43;
            Expression<Func<int>> f = () => x + y;

            var e = ((Expression<Func<int>>)Roundtrip(f)).Compile();

            Assert.AreEqual(f.Compile()(), e());
        }*/

        #endregion

        #region Type resolution

        [TestMethod]
        public void Bonsai_TypeResolutionOverride()
        {
            var e = (Expression<Func<Bary, int>>)(b => b.Qux);

            var typeSpace = new TypeSpace();
            var invertedTypeSpace = new InvertedTypeSpace();

            var barSlim = typeSpace.ConvertType(typeof(Bary));
            invertedTypeSpace.MapType(barSlim, typeof(Fooy));
            var r = Roundtrip(e, typeSpace, invertedTypeSpace) as Expression<Func<Fooy, int>>;

            Assert.IsNotNull(r);
            Assert.AreEqual(42, r.Compile()(new Fooy(42)));

            var typeSpace08 = new TypeSpace();
            var invertedTypeSpace08 = new InvertedTypeSpace();

            var barSlim08 = typeSpace08.ConvertType(typeof(Bary));
            invertedTypeSpace08.MapType(barSlim08, typeof(Fooy));
            var r08 = Roundtrip(e, new Version(0, 8), typeSpace08, invertedTypeSpace08) as Expression<Func<Fooy, int>>;

            Assert.IsNotNull(r08);
            Assert.AreEqual(42, r08.Compile()(new Fooy(42)));
        }

        private sealed class Bary
        {
            public Bary(int qux)
            {
                Qux = qux;
            }

            public int Qux { get; set; }
        }

        private sealed class Fooy
        {
            public Fooy(int qux)
            {
                Qux = qux;
            }

            public int Qux { get; set; }
        }

        private sealed class MyTypeResolver : ITypeResolutionService
        {
            public System.Reflection.Assembly GetAssembly(System.Reflection.AssemblyName name, bool throwOnError)
            {
                return null;
            }

            public System.Reflection.Assembly GetAssembly(System.Reflection.AssemblyName name)
            {
                return GetAssembly(name, throwOnError: false);
            }

            public string GetPathOfAssembly(System.Reflection.AssemblyName name)
            {
                throw new NotImplementedException();
            }

            public Type GetType(string name, bool throwOnError, bool ignoreCase)
            {
                if (name.StartsWith(typeof(Bary).FullName))
                {
                    return typeof(Fooy);
                }

                return null;
            }

            public Type GetType(string name, bool throwOnError)
            {
                return GetType(name, throwOnError, ignoreCase: false);
            }

            public Type GetType(string name)
            {
                return GetType(name, throwOnError: false, ignoreCase: false);
            }

            public void ReferenceAssembly(System.Reflection.AssemblyName name)
            {
            }
        }

        #endregion

        #region Recursion Tests

        [TestMethod]
        public void Bonsai_RecursiveStructuralType()
        {
            var rc = new RuntimeCompiler();
            var tb = rc.GetNewRecordTypeBuilder();
            rc.DefineRecordType(tb, new Dictionary<string, Type> { { "foo", tb } }, valueEquality: true);
            var rt = tb.CreateType();

            var instance = Activator.CreateInstance(rt);

            var c = Expression.Constant(instance);

            var res = (ConstantExpression)Roundtrip(c, new CustomBonsaiExpressionSerializer(_ => __ => Json.Expression.String("constant"), type => __ => Activator.CreateInstance(type)));

            Assert.AreNotSame(c.Type, res.Type);
            Assert.AreSame(res.Type, res.Type.GetProperty("foo").PropertyType);
        }

        [TestMethod]
        public void Bonsai_RecursiveStructuralType_Nested()
        {
            var rc = new RuntimeCompiler();
            var tb1 = rc.GetNewRecordTypeBuilder();
            var tb2 = rc.GetNewRecordTypeBuilder();
            rc.DefineRecordType(tb1, new Dictionary<string, Type> { { "foo", tb2 } }, valueEquality: true);
            rc.DefineRecordType(tb2, new Dictionary<string, Type> { { "bar", tb1 } }, valueEquality: true);
            var rt2 = tb2.CreateType();
            var rt1 = tb1.CreateType();

            var instance = Activator.CreateInstance(rt1);

            var c = Expression.Constant(instance);

            var res = (ConstantExpression)Roundtrip(c, new CustomBonsaiExpressionSerializer(_ => __ => Json.Expression.String("constant"), type => __ => Activator.CreateInstance(type)));

            Assert.AreNotSame(c.Type, res.Type);
            Assert.AreSame(res.Type, res.Type.GetProperty("foo").PropertyType.GetProperty("bar").PropertyType);
        }

        #endregion

        #region End-to-end tests

#pragma warning disable IDE0050 // Convert to tuple. (Tests for anonymous types.)

        [TestMethod]
        public void Bonsai_E2E_QueryWithTransparentIdentifiers()
        {
            Expression<Func<IEnumerable>> f = () => from x in Enumerable.Range(0, 10)
                                                    let y = x * 2
                                                    let z = x + y
                                                    where z > 5
                                                    select new { x, y, z };

            var e = Expression.Lambda<Func<IEnumerable>>(Roundtrip(f.Body));

            var xs = e.Compile()().Cast<object>().Select(o => o.ToString());
            var ys = f.Compile()().Cast<object>().Select(o => o.ToString());

            Assert.IsTrue(xs.SequenceEqual(ys));

            var e08 = Expression.Lambda<Func<IEnumerable>>(Roundtrip(f.Body, V08));

            var xs08 = e08.Compile()().Cast<object>().Select(o => o.ToString());
            var ys08 = f.Compile()().Cast<object>().Select(o => o.ToString());

            Assert.IsTrue(xs08.SequenceEqual(ys08));
        }

        [TestMethod]
        public void Bonsai_AnonymousTypesInequality()
        {
            Expression<Func<int>> f = () => new { y = 2, new { x = 1, y = 2 }.x }.x;

            var e = Expression.Lambda<Func<int>>(Roundtrip(f.Body));

            var x = e.Compile()();
            var y = e.Compile()();

            Assert.AreEqual(x, y);
        }

#pragma warning restore IDE0050

        [TestMethod]
        public void Bonsai_E2E_Meta()
        {
            Expression<Func<Expression>> f = () => Expression.Lambda<Func<int>>(Expression.Constant(42));

            var e = Roundtrip(f.Body);

            Assert.AreEqual(f.Body.ToString(), e.ToString());

            var e08 = Roundtrip(f.Body, V08);

            Assert.AreEqual(f.Body.ToString(), e08.ToString());
        }

        [TestMethod]
        public void Bonsai_E2E_Jacquard()
        {
            Expression<Func<IQueryable<string>, IQueryable<string>>> f = urlsToSearch =>
                from url in urlsToSearch
                let client = new System.Net.WebClient
                {
                    Headers =
                    {
                        { "user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)" }
                    }
                }
                let data = client.OpenRead(url)
                let reader = new System.IO.StreamReader(data)
                let s = reader.ReadToEnd()
                where s.Substring(0, 64).Dump(url)
                where data.Close()
                where reader.Close()
                select s;

            var e = (Expression<Func<IQueryable<string>, IQueryable<string>>>)Roundtrip(f);

            static ExpressionEqualityComparator factory() => new Comparator(new StructuralTypeEqualityComparator());
            Assert.IsTrue(new ExpressionEqualityComparer(factory).Equals(f, e));

            var e08 = (Expression<Func<IQueryable<string>, IQueryable<string>>>)Roundtrip(f, V08);

            Assert.IsTrue(new ExpressionEqualityComparer(factory).Equals(f, e08));
        }

        [TestMethod]
        public void Bonsai_E2E_GenericParameterBindings()
        {
            Expression<Func<List<int>, List<int>, List<int>, List<int>>> f = (x, y, z) => Operators.Concat(Operators.Concat(x, y, z), x, 0);

            try
            {
                var e = (Expression<Func<List<int>, List<int>, List<int>, List<int>>>)Roundtrip(f);
            }
            catch (InvalidOperationException)
            {
                Assert.Fail("Regressed to caching types containing open generic parameters.");
            }
        }

        private static class Operators
        {
            public static List<T> Concat<T>(List<T> x, List<T> y, int take)
            {
                return x.Concat(y.Take(take)).ToList();
            }

            public static List<T> Concat<T>(List<T> x, List<T> y, List<T> z)
            {
                return x.Concat(y).Concat(z).ToList();
            }
        }

        [TestMethod]
        public void Bonsai_E2E_FibonacciWithLoops()
        {
            var n = Expression.Parameter(typeof(int), "n");

            var i = Expression.Variable(typeof(int), "i");

            var fib = Expression.Variable(typeof(long), "fib");
            var prev1 = Expression.Variable(typeof(long), "prev2");
            var prev2 = Expression.Variable(typeof(long), "prev2");

            var loopBreakTarget = Expression.Label();
            var endTarget = Expression.Label(typeof(long), "end");

            var zero = Expression.Constant(0);
            var one = Expression.Constant(1);
            var zeroL = Expression.Constant(0L);
            var oneL = Expression.Constant(1L);

            var baseCase = Expression.Condition(
                Expression.Equal(n, zero),
                Expression.Return(endTarget, zeroL),
                Expression.Empty());

            var assign = Expression.Assign(i, one);
            var assignFib = Expression.Assign(fib, oneL);
            var assign1 = Expression.Assign(prev1, oneL);
            var assign2 = Expression.Assign(prev2, zeroL);

            var loopCondition = Expression.IfThen(
                Expression.GreaterThanOrEqual(i, n),
                Expression.Break(loopBreakTarget));

            var loopInc = Expression.PreIncrementAssign(i);

            var loopFibAssign = Expression.Assign(
                fib,
                Expression.Add(prev1, prev2));

            var loopPrev2Assign = Expression.Assign(prev2, prev1);

            var loopPrev1Assign = Expression.Assign(prev1, fib);

            Expression<Action<long>> printFibExp = s => Console.WriteLine(s);
            var loopPrint = Expression.Invoke(printFibExp, new[] { fib });

            var loop = Expression.Loop(
                Expression.Block(
                    new Expression[]
                    {
                        loopCondition,
                        loopInc,
                        loopFibAssign,
                        loopPrev2Assign,
                        loopPrev1Assign,
                        loopPrint
                    }),
                loopBreakTarget);

            var returnFib = Expression.Return(endTarget, fib, typeof(long));

            var end = Expression.Label(endTarget, fib);

            var block = Expression.Block(
                new ParameterExpression[]
                {
                    i,
                    fib,
                    prev1,
                    prev2

                },
                new Expression[]
                {
                    baseCase,
                    assign,
                    assignFib,
                    assign1,
                    assign2,
                    loopPrint,
                    loop,
                    returnFib,
                    end
                });

            var f = (Expression<Func<int, long>>)Expression.Lambda(block, new[] { n });
            var e = (Expression<Func<int, long>>)Roundtrip(f);

            var fc = f.Compile();
            var ec = e.Compile();

            for (int iter = 0; iter < 10; ++iter)
                Assert.AreEqual(fc(iter), ec(iter));
        }

        #endregion

        #region Null references

        [TestMethod]
        public void Bonsai_Serialize_Expression_Null1()
        {
            WithSerializers((serializer, backCompatSerializer) =>
            {
                var slim = serializer.Lift(expression: null);
                var s = serializer.Serialize(slim);

                var d = serializer.Deserialize(s);
                var e = serializer.Reduce(d);

                Assert.IsNull(d);
                Assert.IsNull(e);
            });
        }

        [TestMethod]
        public void Bonsai_Serialize_Expression_Null2()
        {
            WithSerializers((serializer, backCompatSerializer) =>
            {
                var s = serializer.Serialize(expression: null);
                var d = serializer.Deserialize(s);

                Assert.IsNull(d);
            });
        }

        [TestMethod]
        public void Bonsai_Serialize_Expression_Null3()
        {
            WithSerializers((serializer, backCompatSerializer) =>
            {
                var d = serializer.Deserialize("null");

                Assert.IsNull(d);
            });
        }

        [TestMethod]
        public void Bonsai_Serialize_Expression_Null4()
        {
            WithSerializers((serializer, backCompatSerializer) =>
            {
                var d = serializer.Deserialize("null");
                var e = serializer.Reduce(d);

                Assert.IsNull(d);
                Assert.IsNull(e);
            });
        }

        [TestMethod]
        public void Bonsai_Serialize_Expression_Null5()
        {
            WithSerializers((serializer, backCompatSerializer) =>
            {
                var j = backCompatSerializer.Serialize(expression: null);
                var d = backCompatSerializer.Deserialize(j);

                Assert.IsNull(d);
            });
        }

        [TestMethod]
        public void Bonsai_Serialize_Expression_Null6()
        {
            WithSerializers((serializer, backCompatSerializer) =>
            {
                var d = backCompatSerializer.Deserialize(Json.Expression.Null());

                Assert.IsNull(d);
            });
        }

        private static void WithSerializers(Action<BonsaiExpressionSerializer, ExpressionSlimBonsaiSerializer> action)
        {
            var v = new Version(0, 9);
            var serializer = new SimpleExpressionSerializer(v);

            var objectSerializer = new ObjectSerializer();
            var backCompatSerializer = new ExpressionSlimBonsaiSerializer(objectSerializer.GetJsonSerializer, objectSerializer.GetJsonDeserializer, v);

            action(serializer, backCompatSerializer);
        }

        #endregion

        #region Malformed inputs

        [TestMethod]
        public void Bonsai_Malformed_Parameter()
        {
            var bonsai = @"{""Context"": {""Types"": [[""::"", ""System.Int32"", 0], [""::"", ""System.Func`2"", 0], [""<>"", 1, [0, 0]]], ""Assemblies"": [""mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089""], ""Version"": ""0.9.0.0""}, ""Expression"": [""=>"", 2, ##BODY##, [[0, ""x""]]]}";

            foreach (var param in new[]
            {
                @"[""$""]", // too little

                @"[""$"", 0, 0, 0]", // too much

                @"[""$"", true]", // not an int
                @"[""$"", []]", // not an int

                @"[""$"", true, 0]", // not an int
                @"[""$"", [], 0]", // not an int

                @"[""$"", 0, true]", // not an int
                @"[""$"", 0, []]", // not an int
            })
            {
                var b = bonsai.Replace("##BODY##", param);

                AssertInvalid(b);
            }
        }

        [TestMethod]
        public void Bonsai_Malformed_Block()
        {
            var bonsai = @"{""Context"": {""Types"": [[""::"", ""System.Int32"", 0], [""::"", ""System.Func`2"", 0], [""<>"", 1, [0, 0]]], ""Assemblies"": [""mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089""], ""Version"": ""0.9.0.0""}, ""Expression"": [""=>"", 2, ##BODY##, [[0, ""x""]]]}";

            foreach (var param in new[]
            {
                @"[""{...}""]", // too little
                @"[""{...}"", 0]", // too little
                @"[""{...}"", 0, 1]", // too little

                @"[""{...}"", 0, 1, 2, 3, 4]", // too much

                @"[""{...}"", 0, true, []]", // not an array (variables)
                @"[""{...}"", 0, {}, []]", // not an array (variables)

                @"[""{...}"", 0, [], true]", // not an array (expressions)
                @"[""{...}"", 0, [], {}]", // not an array (expressions)
            })
            {
                var b = bonsai.Replace("##BODY##", param);

                AssertInvalid(b);
            }
        }

        [TestMethod]
        public void Bonsai_Malformed_Throw()
        {
            var bonsai = @"{""Context"": {""Types"": [[""::"", ""System.Int32"", 0], [""::"", ""System.Func`2"", 0], [""<>"", 1, [0, 0]]], ""Assemblies"": [""mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089""], ""Version"": ""0.9.0.0""}, ""Expression"": [""=>"", 2, ##BODY##, [[0, ""x""]]]}";

            foreach (var param in new[]
            {
                @"[""throw""]", // too little

                @"[""throw"", 0, 1, 2]", // too much
                @"[""throw"", 0, 1, 2, 3]", // too much
            })
            {
                var b = bonsai.Replace("##BODY##", param);

                AssertInvalid(b);
            }
        }

        [TestMethod]
        public void Bonsai_Malformed_Unbox()
        {
            var bonsai = @"{""Context"": {""Types"": [[""::"", ""System.Int32"", 0], [""::"", ""System.Func`2"", 0], [""<>"", 1, [0, 0]]], ""Assemblies"": [""mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089""], ""Version"": ""0.9.0.0""}, ""Expression"": [""=>"", 2, ##BODY##, [[0, ""x""]]]}";

            foreach (var param in new[]
            {
                @"[""unbox""]", // too little
                @"[""unbox"", 0]", // too little

                @"[""unbox"", 0, 1, 2]", // too much
                @"[""unbox"", 0, 1, 2, 3]", // too much
            })
            {
                var b = bonsai.Replace("##BODY##", param);

                AssertInvalid(b);
            }
        }

        [TestMethod]
        public void Bonsai_Malformed_Goto()
        {
            var bonsai = @"{""Context"": {""Types"": [[""::"", ""System.Int32"", 0], [""::"", ""System.Func`2"", 0], [""<>"", 1, [0, 0]]], ""Assemblies"": [""mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089""], ""Version"": ""0.9.0.0""}, ""Expression"": [""=>"", 2, ##BODY##, [[0, ""x""]]]}";

            foreach (var param in new[]
            {
                @"[""goto""]", // too little
                @"[""goto"", 0]", // too little

                @"[""goto"", 0, 1, 2, 4, 5]", // too much
                @"[""goto"", 0, 1, 2, 4, 5, 6]", // too much

                @"[""goto"", true, 1]", // not a valid kind
                @"[""goto"", [], 1]", // not a valid kind
                @"[""goto"", {}, 1]", // not a valid kind
                @"[""goto"", 99, 1]", // not a valid kind

                @"[""goto"", 0, true]", // not a valid label target
                @"[""goto"", 0, []]", // not a valid label target
                @"[""goto"", 0, {}]", // not a valid label target
            })
            {
                var b = bonsai.Replace("##BODY##", param);

                AssertInvalid(b);
            }
        }

        [TestMethod]
        public void Bonsai_Malformed_Label()
        {
            var bonsai = @"{""Context"": {""Types"": [[""::"", ""System.Int32"", 0], [""::"", ""System.Func`2"", 0], [""<>"", 1, [0, 0]]], ""Assemblies"": [""mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089""], ""Version"": ""0.9.0.0""}, ""Expression"": [""=>"", 2, ##BODY##, [[0, ""x""]]]}";

            foreach (var param in new[]
            {
                @"[""label""]", // too little

                @"[""label"", 0, 1, 2]", // too much
                @"[""label"", 0, 1, 2, 3]", // too much
            })
            {
                var b = bonsai.Replace("##BODY##", param);

                AssertInvalid(b);
            }
        }

        [TestMethod]
        public void Bonsai_Malformed_Lambda()
        {
            // REVIEW: Add support for tail call and name.

            var bonsai = @"{""Context"": {""Types"": [[""::"", ""System.Int32"", 0], [""::"", ""System.Func`2"", 0], [""<>"", 1, [0, 0]]], ""Assemblies"": [""mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089""], ""Version"": ""0.9.0.0""}, ""Expression"": [""=>"", 2, ##BODY##, [[0, ""x""]]]}";

            foreach (var param in new[]
            {
                @"[""=>""]", // too little
                @"[""=>"", 1]", // too little
                @"[""=>"", 1, 2]", // too little

                @"[""=>"", 1, 2, 3, 4]", // too much
                @"[""=>"", 1, 2, 3, 4, 5]", // too much
            })
            {
                var b = bonsai.Replace("##BODY##", param);

                AssertInvalid(b);
            }
        }

        [TestMethod]
        public void Bonsai_Malformed_ListInit()
        {
            var bonsai = @"{""Context"": {""Types"": [[""::"", ""System.Int32"", 0], [""::"", ""System.Func`2"", 0], [""<>"", 1, [0, 0]]], ""Assemblies"": [""mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089""], ""Version"": ""0.9.0.0""}, ""Expression"": [""=>"", 2, ##BODY##, [[0, ""x""]]]}";

            foreach (var param in new[]
            {
                @"[""{+}""]", // too little
                @"[""{+}"", 1]", // too little

                @"[""{+}"", 1, 2, 3]", // too much
                @"[""{+}"", 1, 2, 3, 4]", // too much

                @"[""{+}"", [], 2]", // not a NewExpression (NB: other invalid nodes may throw cast exceptions, which is fine)

                @"[""{+}"", [""new"", 0], 2]", // not an array
                @"[""{+}"", [""new"", 0], true]", // not an array
                @"[""{+}"", [""new"", 0], {}]", // not an array
            })
            {
                var b = bonsai.Replace("##BODY##", param);

                AssertInvalid(b);
            }
        }

        [TestMethod]
        public void Bonsai_Malformed_Loop()
        {
            var bonsai = @"{""Context"": {""Types"": [[""::"", ""System.Int32"", 0], [""::"", ""System.Func`2"", 0], [""<>"", 1, [0, 0]]], ""Assemblies"": [""mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089""], ""Version"": ""0.9.0.0""}, ""Expression"": [""=>"", 2, ##BODY##, [[0, ""x""]]]}";

            foreach (var param in new[]
            {
                @"[""loop""]", // too little

                @"[""loop"", 0, 1, 2, 3]", // too much
                @"[""loop"", 0, 1, 2, 3, 4]", // too much
            })
            {
                var b = bonsai.Replace("##BODY##", param);

                AssertInvalid(b);
            }
        }

        [TestMethod]
        public void Bonsai_Malformed_MemberInit()
        {
            var bonsai = @"{""Context"": {""Types"": [[""::"", ""System.Int32"", 0], [""::"", ""System.Func`2"", 0], [""<>"", 1, [0, 0]]], ""Assemblies"": [""mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089""], ""Version"": ""0.9.0.0""}, ""Expression"": [""=>"", 2, ##BODY##, [[0, ""x""]]]}";

            foreach (var param in new[]
            {
                @"[""{.}""]", // too little
                @"[""{.}"", 1]", // too little

                @"[""{.}"", 1, 2, 3]", // too much
                @"[""{.}"", 1, 2, 3, 4]", // too much

                @"[""{.}"", [], 2]", // not a NewExpression (NB: other invalid nodes may throw cast exceptions, which is fine)

                @"[""{.}"", [""new"", 0], 2]", // not an array
                @"[""{.}"", [""new"", 0], true]", // not an array
                @"[""{.}"", [""new"", 0], {}]", // not an array
            })
            {
                var b = bonsai.Replace("##BODY##", param);

                AssertInvalid(b);
            }
        }

        [TestMethod]
        public void Bonsai_Malformed_Switch()
        {
            var bonsai = @"{""Context"": {""Types"": [[""::"", ""System.Int32"", 0], [""::"", ""System.Func`2"", 0], [""<>"", 1, [0, 0]]], ""Assemblies"": [""mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089""], ""Version"": ""0.9.0.0""}, ""Expression"": [""=>"", 2, ##BODY##, [[0, ""x""]]]}";

            foreach (var param in new[]
            {
                @"[""switch"", 0, 1, 2]", // too little
                @"[""switch"", 0, 1, 2, 3]", // too little

                @"[""switch"", 0, 1, 2, 3, 4, 5]", // too much
                @"[""switch"", 0, 1, 2, 3, 4, 5, 6]", // too much

                @"[""switch"", 0, 1, 2, 3, 4]", // not a valid value
                @"[""switch"", true, 1, 2, 3, 4]", // not a valid value
                @"[""switch"", [], 1, 2, 3, 4]", // not a valid value
                @"[""switch"", {}, 1, 2, 3, 4]", // not a valid value

                @"[""switch"", ["":"", 0, 0], 1, 2, 3, 4]", // not an array of cases
                @"[""switch"", ["":"", 0, 0], {}, 2, 3, 4]", // not an array of cases

                @"[""switch"", ["":"", 0, 0], [null], 2, 3, 4]", // not a valid case
                @"[""switch"", ["":"", 0, 0], [1], 2, 3, 4]", // not a valid case
                @"[""switch"", ["":"", 0, 0], [true], 2, 3, 4]", // not a valid case
                @"[""switch"", ["":"", 0, 0], [{}], 2, 3, 4]", // not a valid case

                @"[""switch"", ["":"", 0, 0], [[]], 2, 3, 4]", // case too little
                @"[""switch"", ["":"", 0, 0], [[1]], 2, 3, 4]", // case too little

                @"[""switch"", ["":"", 0, 0], [[1, 2, 3]], 2, 3, 4]", // case too much

                @"[""switch"", ["":"", 0, 0], [[1, 2]], 2, 3, 4]", // case no test values array
                @"[""switch"", ["":"", 0, 0], [[false, 2]], 2, 3, 4]", // case no test values array
                @"[""switch"", ["":"", 0, 0], [[{}, 2]], 2, 3, 4]", // case no test values array
            })
            {
                var b = bonsai.Replace("##BODY##", param);

                AssertInvalid(b);
            }
        }

        [TestMethod]
        public void Bonsai_Malformed_Try()
        {
            var bonsai = @"{""Context"": {""Types"": [[""::"", ""System.Int32"", 0], [""::"", ""System.Func`2"", 0], [""<>"", 1, [0, 0]]], ""Assemblies"": [""mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089""], ""Version"": ""0.9.0.0""}, ""Expression"": [""=>"", 2, ##BODY##, [[0, ""x""]]]}";

            foreach (var param in new[]
            {
                @"[""try"", 0, 1, 2]", // too little
                @"[""try"", 0, 1, 2, 3]", // too little

                @"[""try"", 0, 1, 2, 3, 4, 5]", // too much
                @"[""try"", 0, 1, 2, 3, 4, 5, 6]", // too much

                @"[""try"", 0, 1, 2, 3, 4]", // not a valid value
                @"[""try"", true, 1, 2, 3, 4]", // not a valid value
                @"[""try"", [], 1, 2, 3, 4]", // not a valid value
                @"[""try"", {}, 1, 2, 3, 4]", // not a valid value

                @"[""try"", ["":"", 0, 0], 1, 2, 3, 4]", // not an array of catch blocks
                @"[""try"", ["":"", 0, 0], {}, 2, 3, 4]", // not an array of catch blocks

                @"[""try"", ["":"", 0, 0], [null], 2, 3, 4]", // not a valid catch block
                @"[""try"", ["":"", 0, 0], [1], 2, 3, 4]", // not a valid catch block
                @"[""try"", ["":"", 0, 0], [true], 2, 3, 4]", // not a valid catch block
                @"[""try"", ["":"", 0, 0], [{}], 2, 3, 4]", // not a valid catch block

                @"[""try"", ["":"", 0, 0], [[]], 2, 3, 4]", // catch block too little
                @"[""try"", ["":"", 0, 0], [[1]], 2, 3, 4]", // catch block too little
                @"[""try"", ["":"", 0, 0], [[1, 2]], 2, 3, 4]", // catch block too little
                @"[""try"", ["":"", 0, 0], [[1, 2, 3]], 2, 3, 4]", // catch block too little

                @"[""try"", ["":"", 0, 0], [[1, 2, 3, 4, 5]], 2, 3, 4]", // catch block too much
                @"[""try"", ["":"", 0, 0], [[1, 2, 3, 4, 5, 6]], 2, 3, 4]", // catch block too much
            })
            {
                var b = bonsai.Replace("##BODY##", param);

                AssertInvalid(b);
            }
        }

        [TestMethod]
        public void Bonsai_Malformed_TypeEqual()
        {
            var bonsai = @"{""Context"": {""Types"": [[""::"", ""System.Int32"", 0], [""::"", ""System.Func`2"", 0], [""<>"", 1, [0, 0]]], ""Assemblies"": [""mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089""], ""Version"": ""0.9.0.0""}, ""Expression"": [""=>"", 2, ##BODY##, [[0, ""x""]]]}";

            foreach (var param in new[]
            {
                @"[""=:""]", // too little
                @"[""=:"", 0]", // too little

                @"[""=:"", 0, 1, 2]", // too much
                @"[""=:"", 0, 1, 2, 3]", // too much
            })
            {
                var b = bonsai.Replace("##BODY##", param);

                AssertInvalid(b);
            }
        }

        private static void AssertInvalid(string bonsai)
        {
            var serializer = new SimpleExpressionSerializer();
            Assert.ThrowsException<BonsaiParseException>(() => serializer.Deserialize(bonsai));
        }

        #endregion

        #region Memoization

        [TestMethod]
        public void BonsaiSerialization_NoMemoization()
        {
            var mem = default(IMemoizer);
            var ser = new SimpleExpressionSerializer(new Version(0, 9), mem);

            var e = Expression.Add(Expression.Constant(1), Expression.Constant(2)).ToExpressionSlim();

            var b = ser.Serialize(e);
            Assert.AreEqual(2, ser.GetConstantSerializerHitCount[typeof(int)]);

            var r = ser.Deserialize(b);
            Assert.IsFalse(ser.GetConstantDeserializerHitCount.ContainsKey(typeof(int)));

            _ = r.ToExpression(); // NB: Asymmetric by design; the reduction only gets called at this point.
            Assert.AreEqual(2, ser.GetConstantDeserializerHitCount[typeof(int)]);
        }

        [TestMethod]
        public void BonsaiSerialization_Memoization()
        {
            var mem = Memoizer.Create(MemoizationCacheFactory.Unbounded);
            var ser = new SimpleExpressionSerializer(new Version(0, 9), mem);

            var e = Expression.Add(Expression.Constant(1), Expression.Constant(2)).ToExpressionSlim();

            var b = ser.Serialize(e);
            Assert.AreEqual(1, ser.GetConstantSerializerHitCount[typeof(int)]);

            var r = ser.Deserialize(b);
            Assert.IsFalse(ser.GetConstantDeserializerHitCount.ContainsKey(typeof(int)));
            _ = r.ToExpression(); // NB: Asymmetric by design; the reduction only gets called at this point.
            Assert.AreEqual(1, ser.GetConstantDeserializerHitCount[typeof(int)]);
        }

        #endregion

        #region Helpers

        private static readonly Version V08 = new(0, 8);

        private static Expression Roundtrip(Expression e)
        {
            return Roundtrip(e, new Version(0, 9));
        }

        private static Expression Roundtrip(Expression e, Version v)
        {
            var serializer = new SimpleExpressionSerializer(v);
            var slim = serializer.Lift(e);
            var s = serializer.Serialize(slim);
            var d = serializer.Deserialize(s);

            var objectSerializer = new ObjectSerializer();
            var backCompatSerializer = new ExpressionSlimBonsaiSerializer(objectSerializer.GetJsonSerializer, objectSerializer.GetJsonDeserializer, v);

            var sbc = backCompatSerializer.Serialize(d);
            var sbd = backCompatSerializer.Deserialize(sbc);

            var expectedString = Regex.Replace(slim.ToString(), "Constant\\((.*?),(.*?)\\)", "Constant(value,$2)");
            var actualString = Regex.Replace(sbd.ToString(), "Constant\\((.*?),(.*?)\\)", "Constant(value,$2)");

            Assert.AreEqual(expectedString, actualString);

            return serializer.Reduce(sbd);
        }

        private static Expression Roundtrip(Expression e, TypeSpace ts, InvertedTypeSpace its)
        {
            return Roundtrip(e, new Version(0, 9), ts, its);
        }

        private static Expression Roundtrip(Expression e, Version v, TypeSpace ts, InvertedTypeSpace its)
        {
            var objectSerializer = new ObjectSerializer();
            var serializer = new CustomBonsaiExpressionSerializer(ts, its, objectSerializer.GetSerializer, objectSerializer.GetDeserializer, v);

            var slim = serializer.Lift(e);
            var s = serializer.Serialize(slim);
            var d = serializer.Deserialize(s);

            var serializerBackCompat = new ExpressionSlimBonsaiSerializer(objectSerializer.GetJsonSerializer, objectSerializer.GetJsonDeserializer, v);

            var sbc = serializerBackCompat.Serialize(d);
            d = serializerBackCompat.Deserialize(sbc);

            Assert.AreEqual(slim.ToString(), d.ToString());

            return serializer.Reduce(d);
        }

        private sealed class SimpleExpressionSerializer : BonsaiExpressionSerializer
        {
            private static readonly ObjectSerializer s_objectSerializer = new();
            private readonly object _gate = new();

            public SimpleExpressionSerializer() { }

            public SimpleExpressionSerializer(Version version) : base(version) { }

            public SimpleExpressionSerializer(Version version, IMemoizer memoizer) : base(version, memoizer, memoizer) { }

            public Dictionary<Type, int> GetConstantSerializerHitCount { get; } = new Dictionary<Type, int>();
            public Dictionary<Type, int> GetConstantDeserializerHitCount { get; } = new Dictionary<Type, int>();

            protected override Func<object, Json.Expression> GetConstantSerializer(Type type)
            {
                lock (_gate)
                {
                    GetConstantSerializerHitCount[type] = GetConstantSerializerHitCount.TryGetValue(type, out var count) ? count + 1 : 1;
                }

                return s_objectSerializer.GetSerializer(type);
            }

            protected override Func<Json.Expression, object> GetConstantDeserializer(Type type)
            {
                lock (_gate)
                {
                    GetConstantDeserializerHitCount[type] = GetConstantDeserializerHitCount.TryGetValue(type, out var count) ? count + 1 : 1;
                }

                return s_objectSerializer.GetDeserializer(type);
            }
        }

        private sealed class CustomBonsaiExpressionSerializer : BonsaiExpressionSerializer
        {
            private readonly ExpressionToExpressionSlimConverter _lifter;
            private readonly ExpressionSlimToExpressionConverter _reducer;
            private readonly Func<Type, Func<object, Json.Expression>> _liftFactory;
            private readonly Func<Type, Func<Json.Expression, object>> _reduceFactory;

            public CustomBonsaiExpressionSerializer(Func<Type, Func<object, Json.Expression>> liftFactory, Func<Type, Func<Json.Expression, object>> reduceFactory)
                : this(liftFactory, reduceFactory, BonsaiVersion.Default)
            {
            }

            public CustomBonsaiExpressionSerializer(Func<Type, Func<object, Json.Expression>> liftFactory, Func<Type, Func<Json.Expression, object>> reduceFactory, Version version)
                : this(new TypeSpace(), new InvertedTypeSpace(), liftFactory, reduceFactory, version)
            {
            }

            public CustomBonsaiExpressionSerializer(TypeSpace typeSpace, InvertedTypeSpace invertedTypeSpace, Func<Type, Func<object, Json.Expression>> liftFactory, Func<Type, Func<Json.Expression, object>> reduceFactory, Version version)
                : base(version)
            {
                _lifter = new ExpressionToExpressionSlimConverter(typeSpace);
                _reducer = new ExpressionSlimToExpressionConverter(invertedTypeSpace);
                _liftFactory = liftFactory;
                _reduceFactory = reduceFactory;
            }

            public override ExpressionSlim Lift(Expression expression) => _lifter.Visit(expression);

            public override Expression Reduce(ExpressionSlim expression) => _reducer.Visit(expression);

            protected override Func<object, Json.Expression> GetConstantSerializer(Type type) => _liftFactory(type);

            protected override Func<Json.Expression, object> GetConstantDeserializer(Type type) => _reduceFactory(type);
        }

        private sealed class Comparator : ExpressionEqualityComparator
        {
            public Comparator(StructuralTypeEqualityComparator typeComparator)
                : base(typeComparator, typeComparator.MemberComparer, EqualityComparer<object>.Default, EqualityComparer<CallSiteBinder>.Default)
            {
            }
        }

        #endregion
    }

    #region Helper types

    public static class Ext
    {
#pragma warning disable IDE0060 // Remove unused parameter
        public static void Dump(this object o, string s) { }
#pragma warning restore IDE0060 // Remove unused parameter

        public static IQueryable<T> Where<T>(this IQueryable<T> source, Expression<Action<T>> f)
        {
            return source.Provider.CreateQuery<T>(
                Expression.Call(
                    ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(T)),
                    source.Expression,
                    f
                )
            );
        }
    }

    public class Q
    {
        public static int Foo(Expression<Func<IBar>> e)
        {
            return e.Compile()().Qux();
        }
    }

    public interface IBar
    {
        int Qux();
    }

    public interface IFoo : IBar
    {
    }

    public class Foo : IFoo
    {
        public int Qux()
        {
            return 42;
        }
    }

    #endregion
}
