// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - July 2013 - Created this file.
//

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.CompilerServices;
using System.Linq.CompilerServices.Bonsai;
using System.Linq.Expressions;
using System.Numerics;
using System.Reflection;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.System.Linq.Expressions.Bonsai
{
    [TestClass]
    public class TypeSlimDerivationVisitorTests : TestBase
    {
        #region Constant

        [TestMethod]
        public void TypeSlimDerivationVisitor_Constant_BuiltIn()
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
                Expression.Constant((decimal)42),
                Expression.Constant('c'),
                Expression.Constant(true),
                Expression.Constant("Hello"),
            };

            foreach (var c in cs)
            {
                RoundtripAndAssert(c);
            }
        }

        [TestMethod]
        public void TypeSlimDerivationVisitor_Constant_Null()
        {
            var cs = new[]
            {
                Expression.Constant(value: null),
            };

            foreach (var c in cs)
            {
                RoundtripAndAssert(c);
            }
        }

        [TestMethod]
        public void TypeSlimDerivationVisitor_Constant_Collections()
        {
            var xs = new List<int> { 1, 2, 3 };

            var ce = Expression.Constant(xs);

            RoundtripAndAssert(ce);
        }


        #endregion

        #region Unary

        [TestMethod]
        public void TypeSlimDerivationVisitor_Unary_Arith()
        {
            var p = Expression.Parameter(typeof(int), "p");
            var q = Expression.Parameter(typeof(Logic), "q");
            var n = Expression.Parameter(typeof(Logic?), "n");

            var us = new[]
            {
                Expression.Negate(p),
                Expression.NegateChecked(p),
                Expression.UnaryPlus(p),
                Expression.OnesComplement(p),
                Expression.OnesComplement(q),
                Expression.OnesComplement(n),
                Expression.Decrement(p),
                Expression.Increment(p),
            };

            foreach (var a in us)
            {
                RoundtripAndAssert(a);
            }
        }

        [TestMethod]
        public void TypeSlimDerivationVisitor_Unary_Logical()
        {
            var p = Expression.Parameter(typeof(bool), "p");
            var q = Expression.Parameter(typeof(Logic), "q");
            var n = Expression.Parameter(typeof(Logic?), "n");

            var us = new[]
            {
                Expression.Not(p),
                Expression.Not(q),
                Expression.Not(n),
            };

            foreach (var a in us)
            {
                RoundtripAndAssert(a);
            }
        }

        [TestMethod]
        public void TypeSlimDerivationVisitor_Unary_ArrayLength()
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
        public void TypeSlimDerivationVisitor_Unary_Type()
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
        public void TypeSlimDerivationVisitor_Unary_WithMethod()
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

        #endregion

        #region Binary

        [TestMethod]
        public void TypeSlimDerivationVisitor_Binary_Arith_BuiltIn()
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
        public void TypeSlimDerivationVisitor_Binary_Arith_Power()
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
        public void TypeSlimDerivationVisitor_Binary_Arith_Lifted()
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
        public void TypeSlimDerivationVisitor_Binary_OperatorOverloads()
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
        public void TypeSlimDerivationVisitor_Binary_Relational()
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
        public void TypeSlimDerivationVisitor_Binary_Logical()
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
        public void TypeSlimDerivationVisitor_Binary_LiftedToNull()
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
        public void TypeSlimDerivationVisitor_Binary_Coalesce()
        {
            var e = (Expression<Func<string, string>>)(s => s ?? "null");
            var f = e.Compile();

            RoundtripAndAssert((BinaryExpression)e.Body);
        }

        [TestMethod]
        public void TypeSlimDerivationVisitor_Binary_Coalesce_Nullable()
        {
            var e = (Expression<Func<int?, int>>)(x => x ?? 42);
            var f = e.Compile();

            RoundtripAndAssert((BinaryExpression)e.Body);
        }

        [TestMethod]
        public void TypeSlimDerivationVisitor_Binary_Coalesce_WithConversion()
        {
            var f = (Expression<Func<string, int>>)(s => int.Parse(s));
            var p = Expression.Parameter(typeof(string));
            var c = Expression.Coalesce(p, Expression.Constant(0), f);

            var e = (Expression<Func<string, int>>)Expression.Lambda(c, new[] { p });

            RoundtripAndAssert(e.Body);
        }

        [TestMethod]
        public void TypeSlimDerivationVisitor_Binary_Coalesce_ImplicitlyConvertible()
        {
            var p = Expression.Parameter(typeof(int?));
            var c = Expression.Coalesce(p, Expression.Constant(0L));

            var e = Expression.Lambda<Func<int?, long>>(c, new[] { p });

            RoundtripAndAssertNull(e.Body);
        }

        [TestMethod]
        public void TypeSlimDerivationVisitor_Binary_Type()
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
        public void TypeSlimDerivationVisitor_Binary_Comparison()
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
                    ExpressionType.LessThan,
                    p3,
                    p4,
                    false,
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
        public void TypeSlimDerivationVisitor_Binary_Comparison_Nullable()
        {
            Expression<Func<DateTime?, DateTime?, bool?>> f = (dt1, dt2) => dt1 < dt2;

            RoundtripAndAssert(f.Body);
        }

        [TestMethod]
        public void TypeSlimDerivationVisitor_Comparison_OverloadedOperator()
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

        #endregion

        #region Conversion

        [TestMethod]
        public void TypeSlimDerivationVisitor_Convert_Implicit()
        {
            var e = (Expression<Func<int, BigInteger>>)(i => i);
            RoundtripAndAssert(e.Body);
        }

        [TestMethod]
        public void TypeSlimDerivationVisitor_Convert_Explicit()
        {
            var e = (Expression<Func<BigInteger, int>>)(i => (int)i);
            RoundtripAndAssert(e.Body);
        }

        [TestMethod]
        public void TypeSlimDerivationVisitor_Convert_Checked()
        {
            var p = Expression.Parameter(typeof(BigInteger));
            var e = Expression.ConvertChecked(p, typeof(int));
            RoundtripAndAssert(e);
        }

        #endregion

        #region Lambda and parameters

        [TestMethod]
        public void TypeSlimDerivationVisitor_Lambda_IdentityFunc()
        {
            var p = Expression.Parameter(typeof(int));
            var l = Expression.Lambda<Func<int, int>>(p, p);
            RoundtripAndAssert(l);
        }

        [TestMethod]
        public void TypeSlimDerivationVisitor_Lambda_Action()
        {
            var l = (Expression<Action<Nopper>>)(n => n.Nop());
            RoundtripAndAssert(l);
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
        public void TypeSlimDerivationVisitor_Lambda_CustomDelegate()
        {
            var p = Expression.Parameter(typeof(int), "p");
            var l = Expression.Lambda<MyFunc>(p, p);
            RoundtripAndAssert(l);
        }

        private delegate int MyFunc(int x);

        [TestMethod]
        public void TypeSlimDerivationVisitor_Lambda_HigherOrderWithConflictingNames()
        {
            var a = Expression.Parameter(typeof(int), "x");
            var b = Expression.Parameter(typeof(int), "x");
            var i = Expression.Lambda<Func<int, int>>(a, b);
            var l = Expression.Lambda<Func<int, Func<int, int>>>(i, a);
            RoundtripAndAssert(l);
        }

        [TestMethod]
        public void TypeSlimDerivationVisitor_Lambda_HigherOrder()
        {
            var a = Expression.Parameter(typeof(int), "a");
            var b = Expression.Parameter(typeof(int), "b");
            var i = Expression.Lambda<Func<int, int>>(a, b);
            var l = Expression.Lambda<Func<int, Func<int, int>>>(i, a);
            RoundtripAndAssert(l);
        }

        [TestMethod]
        public void TypeSlimDerivationVisitor_Lambda_UnnamedParameter()
        {
            var p = Expression.Parameter(typeof(int), "p");
            var l = Expression.Lambda<Func<int, int>>(p, p);
            RoundtripAndAssert(l);
        }

        [TestMethod]
        public void TypeSlimDerivationVisitor_Lambda_Y()
        {
            Expression<Rec<int, int>> f = fib => n => n > 1 ? fib(fib)(n - 1) + fib(fib)(n - 2) : n;
            RoundtripAndAssert(f);
        }

        private delegate Func<A, R> Rec<A, R>(Rec<A, R> r);

        [TestMethod]
        public void TypeSlimDerivationVisitor_Lambda_ImplicitCovariance()
        {
            Expression<Func<int>> f = () => Q.Foo(() => new Foo2());
            RoundtripAndAssert(f);
        }

        [TestMethod]
        public void TypeSlimDerivationVisitor_Parameter_Unbound()
        {
            var ps = new[]
            {
                Expression.Parameter(typeof(string), "p"),
                Expression.Parameter(typeof(int), "p"),
            };

            foreach (var p in ps)
            {
                RoundtripAndAssert(p);
            }
        }

        #endregion

        #region Calls

        [TestMethod]
        public void TypeSlimDerivationVisitor_Call_StaticWithNoParameters()
        {
            var c = Expression.Call(instance: null, typeof(X).GetMethod("A"));
            RoundtripAndAssert(c);
        }

        [TestMethod]
        public void TypeSlimDerivationVisitor_Call_StaticWithParameters()
        {
            var c = Expression.Call(instance: null, typeof(X).GetMethod("B"), Expression.Constant(2));
            RoundtripAndAssert(c);
        }

        [TestMethod]
        public void TypeSlimDerivationVisitor_Call_StaticVoidWithNoParameters()
        {
            var c = Expression.Call(instance: null, typeof(X).GetMethod("C"));
            RoundtripAndAssert(c);
        }

        [TestMethod]
        public void TypeSlimDerivationVisitor_Call_StaticVoidWithParameters()
        {
            var c = Expression.Call(instance: null, typeof(X).GetMethod("D"), Expression.Constant(42));
            RoundtripAndAssert(c);
        }

        [TestMethod]
        public void TypeSlimDerivationVisitor_Call_StaticGeneric()
        {
            var c = Expression.Call(instance: null, typeof(X).GetMethod("I").MakeGenericMethod(typeof(int)), Expression.Constant(42));
            RoundtripAndAssert(c);
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
        public void TypeSlimDerivationVisitor_Call_InstanceVoid()
        {
            Expression<Action<Adder, int>> f = (a, x) => a.Add(x);
            RoundtripAndAssert(f.Body);
        }

        [TestMethod]
        public void TypeSlimDerivationVisitor_Call_InstanceVoidOverload()
        {
            Expression<Action<Adder, string>> f = (a, x) => a.Add(x);
            RoundtripAndAssert(f.Body);
        }

        [TestMethod]
        public void TypeSlimDerivationVisitor_Call_InstanceVoidGeneric()
        {
            Expression<Action<Adder, Answer>> f = (a, x) => a.Add(x);
            RoundtripAndAssert(f.Body);
        }

        [TestMethod]
        public void TypeSlimDerivationVisitor_Call_InstanceReturn()
        {
            Expression<Func<Adder, int, int>> f = (a, x) => a.AddAndRet(x);
            RoundtripAndAssert(f.Body);
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

        #region Invocation

        [TestMethod]
        public void TypeSlimDerivationVisitor_Invocation()
        {
            var e = (Expression<Func<Func<int, int>, int, int>>)((f, x) => f(x));
            RoundtripAndAssert(e.Body);
        }

        [TestMethod]
        public void TypeSlimDerivationVisitor_Invocation_Action()
        {
            var e = (Expression<Action<Action<int>, int>>)((f, x) => f(x));
            RoundtripAndAssert(e.Body);
        }

        #endregion

        #region Members

        [TestMethod]
        public void TypeSlimDerivationVisitor_Members_Instance_Property()
        {
            Expression<Func<Instancy, string>> f = i => i.Value;
            RoundtripAndAssert(f.Body);
        }

        [TestMethod]
        public void TypeSlimDerivationVisitor_Members_Instance_Field()
        {
            Expression<Func<Instancy, int>> f = i => i._field;
            RoundtripAndAssert(f.Body);
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
        public void TypeSlimDerivationVisitor_Members_Static_Property()
        {
            Expression<Func<string>> f = () => Staticy.Value;
            RoundtripAndAssert(f.Body);
        }

        [TestMethod]
        public void TypeSlimDerivationVisitor_Members_Static_Field()
        {
            Expression<Func<int>> f = () => Staticy._field;
            RoundtripAndAssert(f.Body);
        }

        [TestMethod]
        public void TypeSlimDerivationVisitor_Members_Indexed_Property()
        {
            var p = Expression.Parameter(typeof(Instancy));
            var idx1 = Expression.MakeIndex(p, typeof(Instancy).GetProperty("Item", new[] { typeof(int) }), new[] { Expression.Constant(0) });
            var f1 = Expression.Lambda<Func<Instancy, int>>(idx1, p);

            var idx2 = Expression.MakeIndex(p, typeof(Instancy).GetProperty("Item", new[] { typeof(string), typeof(int) }), new[] { Expression.Constant("foo"), Expression.Constant(0) });
            var f2 = Expression.Lambda<Func<Instancy, string>>(idx2, p);

            RoundtripAndAssert(f1.Body);
            RoundtripAndAssert(f2.Body);
        }

        private static class Staticy
        {
            public static int _field = 42;

            public static string Value => "Hello";
        }

        #endregion

        #region Indexers

        [TestMethod]
        public void TypeSlimDerivationVisitor_ArrayIndex_RankOne()
        {
            var e = (Expression<Func<string[], int, string>>)((ss, i) => /* ArrayIndex */ ss[i]);
            RoundtripAndAssert(e.Body);
        }

        [TestMethod]
        public void TypeSlimDerivationVisitor_ArrayIndex_HigherRank()
        {
            var e = (Expression<Func<string[,], int, int, string>>)((ss, i, j) => ss[i, j]);
            RoundtripAndAssert(e.Body);
        }

        [TestMethod]
        public void TypeSlimDerivationVisitor_Indexer()
        {
            var e = (Expression<Func<Indexy, string, int>>)((i, s) => i[s]);
            RoundtripAndAssert(e.Body);
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
        public void TypeSlimDerivationVisitor_Default()
        {
            var e = Expression.Default(typeof(string));
            RoundtripAndAssert(e);
        }

        #endregion

        #region Anonymous types

#pragma warning disable IDE0050 // Convert to tuple. (Tests for anonymous types.)

        [TestMethod]
        public void TypeSlimDerivationVisitor_AnonymousType_OriginalTypeReused()
        {
            Expression<Func<object>> x = () => new { x = 1, y = 2 };

            RoundtripAndAssertStructural(x.Body);
        }

        [TestMethod]
        public void TypeSlimDerivationVisitor_AnonymousType_KeysReconstructed()
        {
            var anon = RuntimeCompiler.CreateAnonymousType(new[]
            {
                new KeyValuePair<string, Type>("foo", typeof(int)),
                new KeyValuePair<string, Type>("bar", typeof(int))
            }, new string[] { "bar" });

            Assert.IsNotNull(anon.GetProperty("bar"));
            Assert.IsNotNull(anon.GetProperty("foo"));

            var m = Expression.New(anon.GetConstructors().Single(), Expression.Constant(1), Expression.Constant(2));

            RoundtripAndAssertStructural(m);
        }

        [TestMethod]
        public void TypeSlimDerivationVisitor_AnonymousType_TypeReconstructed()
        {
            Expression<Func<object>> x = () => new { x = 1, y = 2 };

            RoundtripAndAssertStructural(x.Body);
        }

#pragma warning restore IDE0050

        #endregion

        #region Record types

        [TestMethod]
        public void TypeSlimDerivationVisitor_RecordType_WithValueEqualitySemantics()
        {
            var rec = RuntimeCompiler.CreateRecordType(new[]
            {
                new KeyValuePair<string, Type>("bar", typeof(int))
            }, valueEquality: true);

            var bar = rec.GetProperty("bar");

            var m = Expression.MemberInit(Expression.New(rec), Expression.Bind(bar, Expression.Constant(42)));

            RoundtripAndAssertStructural(m);
        }

        [TestMethod]
        public void TypeSlimDerivationVisitor_RecordType_WithoutValueEqualitySemantics()
        {
            var rec = RuntimeCompiler.CreateRecordType(new[]
            {
                new KeyValuePair<string, Type>("bar", typeof(int))
            }, valueEquality: false);

            var bar = rec.GetProperty("bar");

            var m = Expression.MemberInit(Expression.New(rec), Expression.Bind(bar, Expression.Constant(42)));

            RoundtripAndAssertStructural(m);
        }

        [TestMethod]
        public void TypeSlimDerivationVisitor_RecordType_TypeReconstructed()
        {
            var rec = RuntimeCompiler.CreateRecordType(new[]
            {
                new KeyValuePair<string, Type>("bar", typeof(int))
            }, valueEquality: true);

            var bar = rec.GetProperty("bar");

            var m = Expression.MemberInit(Expression.New(rec), Expression.Bind(bar, Expression.Constant(42)));

            RoundtripAndAssertStructural(m);
        }

        #endregion

        #region Conditional

        [TestMethod]
        public void TypeSlimDerivationVisitor_Conditional()
        {
            Expression<Func<bool, int>> f = b => b ? 3 : 4;
            RoundtripAndAssert(f.Body);
        }

        [TestMethod]
        public void TypeSlimDerivationVisitor_Conditional_WithType()
        {
            var c = Expression.Condition(Expression.Constant(true, typeof(bool)), Expression.New(typeof(B)), Expression.New(typeof(C)), typeof(A));
            RoundtripAndAssert(c);
        }

        private class A { }

        private class B : A { }

        private sealed class C : B { }

        #endregion

        #region Object creation and initialization

        [TestMethod]
        public void TypeSlimDerivationVisitor_Init_Arrays()
        {
            Expression<Func<int[]>> f = () => new[] { 1, 2, 3 };
            RoundtripAndAssert(f.Body);
        }

        [TestMethod]
        public void TypeSlimDerivationVisitor_Init_List()
        {
            Expression<Func<List<int>>> f = () => new List<int> { 1, 2, 3 };
            RoundtripAndAssert(f.Body);
        }

        [TestMethod]
        public void TypeSlimDerivationVisitor_Init_Object()
        {
            var e = (Expression<Func<ThisQux>>)(() => new ThisQux(42) { Baz = "Hello", Bar = { Zuz = 24 }, Foos = { 1, 2, 3 } });
            RoundtripAndAssert(e.Body);
        }

        [TestMethod]
        public void TypeSlimDerivationVisitor_New_ValueType_DefaultConstructor()
        {
            var f = Expression.Lambda(Expression.New(typeof(int)));
            RoundtripAndAssert(f.Body);
        }

        [TestMethod]
        public void TypeSlimDerivationVisitor_New_Array_Bounds()
        {
            var e = Expression.NewArrayBounds(typeof(int), Expression.Constant(1));
            RoundtripAndAssert(e);
        }

        private sealed class ThisQux
        {
            public ThisQux(int x)
            {
                X = x;
                Foos = new List<int>();
                Bar = new ThisBar();
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

            public ThisBar Bar
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

        private sealed class ThisBar
        {
            public int Zuz = 1;
        }

        #endregion

        #region Loop

        [TestMethod]
        public void TypeSlimDerivationVisitor_Loop()
        {
            {
                var e = Expression.Loop(Expression.Constant(42));
                RoundtripAndAssert(e);
            }

            {
                var b = Expression.Label(typeof(void));
                var c = Expression.Label(typeof(void));
                var e = Expression.Loop(Expression.Break(b), b, c);
                RoundtripAndAssert(e);
            }

            {
                var b = Expression.Label(typeof(int));
                var c = Expression.Label(typeof(void));
                var e = Expression.Loop(Expression.Break(b, Expression.Constant(42)), b, c);
                RoundtripAndAssert(e);
            }

            {
                var b = Expression.Label(typeof(int));
                var c = Expression.Label(typeof(void));
                var e = Expression.Loop(Expression.Continue(c), b, c);
                RoundtripAndAssert(e);
            }
        }

        [TestMethod]
        public void TypeSlimDerivationVisitor_Loop_Multiple_Break()
        {
            var b = Expression.Label(typeof(int));
            var c = Expression.Label(typeof(void));
            var e =
                Expression.Loop(
                    Expression.Block(
                        Expression.Break(b, Expression.Constant(42)),
                        Expression.Break(b, Expression.Constant(42)),
                        Expression.Constant("42")
                    ),
                    b,
                    c
                );
            RoundtripAndAssert(e);
        }

        #endregion

        #region Block

        [TestMethod]
        public void TypeSlimDerivationVisitor_Block_Simple()
        {
            {
                var e = Expression.Block(Expression.Constant(42));
                RoundtripAndAssert(e);
            }

            {
                var p = Expression.Variable(typeof(int));
                var e =
                    Expression.Block(
                        new[] { p },
                        p
                    );
                RoundtripAndAssert(e);
            }

            {
                var p = Expression.Variable(typeof(int));
                var e =
                    Expression.Block(
                        new[] { p },
                        Expression.Constant("42"),
                        p
                    );
                RoundtripAndAssert(e);
            }

            {
                var p = Expression.Variable(typeof(int));
                var e =
                    Expression.Block(
                        new[] { p },
                        p,
                        Expression.Constant("42")
                    );
                RoundtripAndAssert(e);
            }

            {
                var e =
                    Expression.Block(
                        typeof(void),
                        Expression.Constant(42)
                    );
                RoundtripAndAssert(e);
            }

            {
                var e = new BlockExpressionSlim(variables: null, new[] { Expression.Constant(42).ToExpressionSlim() }.ToReadOnly(), typeof(string).ToTypeSlim());
                Assert.AreSame(Derive(e).ToType(), typeof(string));
            }

            {
                var e = new BlockExpressionSlim(variables: null, new[] { Expression.Constant(42).ToExpressionSlim() }.ToReadOnly(), type: null);
                Assert.AreSame(Derive(e).ToType(), typeof(int));
            }
        }

        #endregion

        #region Try

        [TestMethod]
        public void TypeSlimDerivationVisitor_Try()
        {
            {
                var ex = (Expression<Func<Exception>>)(() => new Exception());
                var p = Expression.Parameter(typeof(Exception), "a");
                var expr =
                    Expression.TryCatch(
                        Expression.Throw(ex.Body, typeof(int)),
                        Expression.Catch(
                            p,
                            Expression.Constant(43)
                        )
                    );
                RoundtripAndAssert(expr);
            }

            {
                var e = Expression.TryFault(Expression.Constant(42), Expression.Constant(42));
                RoundtripAndAssert(e);
            }

            {
                var e = new TryExpressionSlim(type: null, Expression.Constant(42).ToExpressionSlim(), Expression.Constant("42").ToExpressionSlim(), Expression.Constant("42").ToExpressionSlim(), Array.Empty<CatchBlockSlim>().ToReadOnly());
                Assert.AreSame(Derive(e).ToType(), typeof(int));
            }
        }

        #endregion

        #region Label

        [TestMethod]
        public void TypeSlimDerivationVisitor_Label()
        {
            var e = Expression.Label(Expression.Label());
            RoundtripAndAssert(e);
        }

        #endregion

        #region Switch

        [TestMethod]
        public void TypeSlimDerivationVisitor_Switch()
        {
            {
                var e = Expression.Switch(Expression.Constant(42), Expression.Constant("42"), Expression.SwitchCase(Expression.Constant("42"), Expression.Constant(42)));
                RoundtripAndAssert(e);
            }
        }

        #endregion

        #region End-to-end tests

#pragma warning disable IDE0050 // Convert to tuple. (Tests for anonymous types.)

        [TestMethod]
        public void TypeSlimDerivationVisitor_E2E_QueryWithTransparentIdentifiers()
        {
            Expression<Func<IEnumerable>> f = () => from x in Enumerable.Range(0, 10)
                                                    let y = x * 2
                                                    let z = x + y
                                                    where z > 5
                                                    select new { x, y, z };

            RoundtripAndAssertStructural(f.Body);
        }

#pragma warning restore IDE0050

        [TestMethod]
        public void TypeSlimDerivationVisitor_E2E_Meta()
        {
            Expression<Func<Expression>> f = () => Expression.Lambda<Func<int>>(Expression.Constant(42));

            RoundtripAndAssert(f.Body);
        }

#if NETFRAMEWORK
        [TestMethod]
        public void TypeSlimDerivationVisitor_E2E_Jacquard()
        {
            Expression<Func<IQueryable<string>, IQueryable<string>>> f = urlsToSearch =>
                from url in urlsToSearch
                let client = new global::System.Net.WebClient
                {
                    Headers =
                    {
                        { "user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)" }
                    }
                }
                let data = client.OpenRead(url)
                let reader = new StreamReader(data)
                let s = reader.ReadToEnd()
                where s.Substring(0, 64).Dump(url)
                where data.Close()
                where reader.Close()
                select s;

            RoundtripAndAssert(f);
        }
#endif

        [TestMethod]
        public void TypeSlimDerivationVisitor_E2E_GenericParameterBindings()
        {
            Expression<Func<List<int>, List<int>, List<int>, List<int>>> f = (x, y, z) => Operators.Concat(Operators.Concat(x, y, z), x, 0);
            RoundtripAndAssert(f.Body);
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

#endregion

#region Man or boy tests

        [TestMethod]
        public void TypeSlimDerivationVisitor_ManOrBoy1()
        {
#pragma warning disable IDE0004 // Remove Unnecessary Cast. (Keeping conversion in expression tree.)
            var e = (Expression<Func<string, bool>>)(s => !(~(long)(-(s ?? "qux").ToLower().Substring(1, 2).Length * 2 + 1) > 5L) && s.StartsWith("bar") || s.EndsWith("foo"));
            ManOrBoy(e);
#pragma warning restore IDE0004
        }

        [TestMethod]
        public void TypeSlimDerivationVisitor_ManOrBoy2()
        {
            var e = (Expression<Func<DateTime?, DateTime?, bool?>>)((dt1, dt2) => dt1 - dt2 < TimeSpan.Zero);
            ManOrBoy(e);
        }

        [TestMethod]
        public void TypeSlimDerivationVisitor_ManOrBoy3()
        {
            var e = (Expression<Func<IEnumerable<int>>>)(() => Enumerable.Range(0, 10).Where(x => x > 0).Select(x => x * x));
            ManOrBoy(e);
        }

        private static void ManOrBoy(Expression e)
        {
            var v = new ManOrBoyVisitor();
            v.Visit(e);

            foreach (var kv in v.Entries)
            {
                Assert.IsNotNull(kv.Value);
                Assert.AreEqual(kv.Key.Type, kv.Value.ToType());
            }
        }

        private sealed class ManOrBoyVisitor : ExpressionVisitor
        {
            public readonly List<KeyValuePair<Expression, TypeSlim>> Entries = new();

            public override Expression Visit(Expression node)
            {
                if (node != null)
                {
                    var s = node.ToExpressionSlim();

                    var type = Derive(s);
                    Entries.Add(new KeyValuePair<Expression, TypeSlim>(node, type));
                }

                return base.Visit(node);
            }
        }

#endregion

#region Unsupported stuff

        [TestMethod]
        public void TypeSlimDerivationVisitor_Unsupported()
        {
            new MyDeriver().Test();
        }

#endregion

#region Helpers

        private static TypeSlim Derive(ExpressionSlim e)
        {
            var derivationVisitor = new TypeSlimDerivationVisitor();
            return derivationVisitor.Visit(e);
        }

        private static void RoundtripAndAssert(Expression e)
        {
            Assert.AreSame(Derive(e.ToExpressionSlim()).ToType(), e.Type);
        }

        private static void RoundtripAndAssertNull(Expression e)
        {
            Assert.IsNull(Derive(e.ToExpressionSlim()));
        }

        private static void RoundtripAndAssertStructural(Expression e)
        {
            var originalSlim = e.Type.ToTypeSlim();
            var roundtripSlim = Derive(e.ToExpressionSlim());
            Assert.IsTrue(TypeSlimExtensions.Equals(originalSlim, roundtripSlim));
            Assert.IsTrue(StructuralTypeEqualityComparer.Default.Equals(roundtripSlim.ToType(), e.Type));
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

    public class Foo2 : IFoo
    {
        public int Qux()
        {
            return 42;
        }
    }

    public struct Logic
    {
        public static Logic operator !(Logic l) => l;

        public static Logic operator ~(Logic l) => l;
    }

    internal sealed class MyDeriver : TypeSlimDerivationVisitor
    {
        public void Test()
        {
            Assert.ThrowsException<InvalidOperationException>(() => base.MakeConditional(node: null, test: null, ifTrue: null, ifFalse: null));
            Assert.ThrowsException<InvalidOperationException>(() => base.MakeConstant(node: null));
            Assert.ThrowsException<InvalidOperationException>(() => base.MakeDefault(node: null));
            Assert.ThrowsException<InvalidOperationException>(() => base.MakeElementInit(node: null, arguments: null));
            Assert.ThrowsException<InvalidOperationException>(() => base.MakeIndex(node: null, @object: null, arguments: null));
            Assert.ThrowsException<InvalidOperationException>(() => base.MakeInvocation(node: null, expression: null, arguments: null));
            Assert.ThrowsException<InvalidOperationException>(() => base.MakeListInit(node: null, newExpression: null, initializers: null));
            Assert.ThrowsException<InvalidOperationException>(() => base.MakeMember(node: null, expression: null));
            Assert.ThrowsException<InvalidOperationException>(() => base.MakeMemberAssignment(node: null, expression: null));
            Assert.ThrowsException<InvalidOperationException>(() => base.MakeMemberInit(node: null, newExpression: null, bindings: null));
            Assert.ThrowsException<InvalidOperationException>(() => base.MakeMemberListBinding(node: null, initializers: null));
            Assert.ThrowsException<InvalidOperationException>(() => base.MakeMemberMemberBinding(node: null, bindings: null));
            Assert.ThrowsException<InvalidOperationException>(() => base.MakeMethodCall(node: null, @object: null, arguments: null));
            Assert.ThrowsException<InvalidOperationException>(() => base.MakeNew(node: null, arguments: null));
            Assert.ThrowsException<InvalidOperationException>(() => base.MakeNewArray(node: null, expressions: null));
            Assert.ThrowsException<InvalidOperationException>(() => base.MakeTypeBinary(node: null, expression: null));
        }
    }

#endregion
}
