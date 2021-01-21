// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD, ER - January 2014 - Created this file.
//

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.CompilerServices;
using System.Linq.Expressions;
using System.Numerics;
using System.Reflection;
using Tests.VisualBasic;

namespace Tests.System.Linq.CompilerServices
{
    [TestClass]
    public class ExpressionInterningTests
    {
        #region Test Cleanup

        [TestCleanup]
        public void ClearCache()
        {
            ExpressionInterning.ClearInternCache();
        }

        #endregion

        #region Argument Checks

        [TestMethod]
        public void Intern_ArgumentChecks()
        {
            AssertEx.ThrowsException<ArgumentNullException>(() => Expression.Constant(1).Intern(cache: null), ex => Assert.AreEqual(ex.ParamName, "cache"));
            AssertEx.ThrowsException<ArgumentNullException>(() => ((Expression)null).Intern(new ExpressionInterningCache()), ex => Assert.AreEqual(ex.ParamName, "expression"));
            AssertEx.ThrowsException<ArgumentNullException>(() => new ExpressionInterningCache().GetOrAdd(expression: null), ex => Assert.AreEqual(ex.ParamName, "expression"));
        }

        #endregion

        #region Cache Count

        [TestMethod]
        public void Intern_CacheCount()
        {
            var cache = new ExpressionInterningCache();
            Assert.AreEqual(cache.Count, 0);
            Expression.Constant(1).Intern(cache);
            Assert.AreEqual(cache.Count, 1);
        }

        #endregion

        #region Constant

        [TestMethod]
        public void Intern_Constant()
        {
            var e1 = Expression.Constant(1);
            var e2 = Expression.Constant(2);
            var e3 = Expression.Constant(1);

            var i1 = Intern(e1);
            var i2 = Intern(e2);
            var i3 = Intern(e3);

            Assert.AreSame(e1, i1);
            Assert.AreSame(i3, i1);
            Assert.AreNotSame(i1, i2);
            Assert.AreNotSame(i2, i3);
        }

        [TestMethod]
        public void Intern_Constant_BuiltIn()
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
                Expression.Constant(true),
                Expression.Constant("Hello"),
                Expression.Constant('c'),
            };

            foreach (var c in cs)
            {
                CloneAndAssert(c);
            }
        }

        [TestMethod]
        public void Intern_Constant_Null()
        {
            var cs = new[]
            {
                Expression.Constant(value: null),
            };

            foreach (var c in cs)
            {
                CloneAndAssert(c);
            }
        }

        [TestMethod]
        public void Intern_Constant_Collections()
        {
            var xs = new List<int> { 1, 2, 3 };

            var ce = Expression.Constant(xs);

            CloneAndAssert(ce);
        }

        #endregion

        #region Unary

        [TestMethod]
        public void Intern_Unary_Arith()
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
                CloneAndAssert(a);
            }
        }

        [TestMethod]
        public void Intern_Unary_Logical()
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
                CloneAndAssert(a);
            }
        }

        [TestMethod]
        public void Intern_Unary_Compound()
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
                CloneAndAssert(a);
            }
        }

        [TestMethod]
        public void Intern_Unary_ArrayLength()
        {
            var p = Expression.Parameter(typeof(int[]), "p");

            var us = new[]
            {
                Expression.ArrayLength(p),
            };

            foreach (var a in us)
            {
                CloneAndAssert(a);
            }
        }

        [TestMethod]
        public void Intern_Unary_Type()
        {
            var p = Expression.Parameter(typeof(string), "p");

            var us = new[]
            {
                Expression.TypeAs(p, typeof(string)),
            };

            foreach (var a in us)
            {
                CloneAndAssert(a);
            }
        }

        [TestMethod]
        public void Intern_Unary_WithMethod()
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
                CloneAndAssert(a);
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
        public void Intern_Binary_Arith_BuiltIn()
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
                CloneAndAssert(a);
            }
        }

        [TestMethod]
        public void Intern_Binary_Arith_Power()
        {
            var p1 = Expression.Parameter(typeof(double), "p1");
            var p2 = Expression.Parameter(typeof(double), "p2");

            var bs = new[]
            {
                Expression.Power(p1, p2),
                Expression.PowerAssign(p1, p2),
            };

            foreach (var a in bs)
            {
                CloneAndAssert(a);
            }
        }

        [TestMethod]
        public void Intern_Binary_Arith_Lifted()
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
                CloneAndAssert(a);
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
        public void Intern_Binary_OperatorOverloads()
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
                CloneAndAssert(a);
            }
        }

        [TestMethod]
        public void Intern_Binary_Relational()
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
                CloneAndAssert(a);
            }
        }

        [TestMethod]
        public void Intern_Binary_Logical()
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
                CloneAndAssert(a);
            }
        }

        [TestMethod]
        public void Intern_Binary_CompoundAssignment()
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
                CloneAndAssert(a);
            }
        }

        [TestMethod]
        public void Intern_Binary_LiftedToNull()
        {
            var e = (Expression<Func<Persoon2, bool>>)(p => p.Geslacht == Sex.Male);

            CloneAndAssert((BinaryExpression)e.Body);
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
        public void Intern_Binary_Coalesce()
        {
            var e = (Expression<Func<string, string>>)(s => s ?? "null");
            CloneAndAssert(e);
        }

        [TestMethod]
        public void Intern_Binary_Coalesce_WithConversion()
        {
            var f = (Expression<Func<string, int>>)(s => int.Parse(s));
            var p = Expression.Parameter(typeof(string));
            var c = Expression.Coalesce(p, Expression.Constant(0), f);

            var e = (Expression<Func<string, int>>)Expression.Lambda(c, new[] { p });
            CloneAndAssert(e);
        }

        [TestMethod]
        public void Intern_Binary_Type()
        {
            var p1 = Expression.Parameter(typeof(string), "p1");

            var bs = new TypeBinaryExpression[]
            {
                Expression.TypeIs(p1, typeof(string)),
                Expression.TypeEqual(p1, typeof(string))
            };

            foreach (var a in bs)
            {
                CloneAndAssert(a);
            }
        }

        [TestMethod]
        public void Intern_Binary_Comparison()
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
                CloneAndAssert(a);
            }
        }

        private static class MyRelationalOperation
        {
            public static bool LessThan(int x, int y)
            {
                return x < y;
            }
        }

        #endregion

        #region Conversion

        [TestMethod]
        public void Intern_Convert_Implicit()
        {
            var e = (Expression<Func<int, BigInteger>>)(i => i);
            CloneAndAssert(e);
        }

        [TestMethod]
        public void Intern_Convert_Explicit()
        {
            var e = (Expression<Func<BigInteger, int>>)(i => (int)i);
            CloneAndAssert(e);
        }

        [TestMethod]
        public void Intern_Convert_Checked()
        {
            var p = Expression.Parameter(typeof(BigInteger));
            var e = Expression.ConvertChecked(p, typeof(int));
            CloneAndAssert(e);
        }

        #endregion

        #region Lambda and parameters

        [TestMethod]
        public void Intern_Lambda_IdentityFunc()
        {
            var p = Expression.Parameter(typeof(int));
            var l = Expression.Lambda<Func<int, int>>(p, p);

            CloneAndAssert(l);
        }

        [TestMethod]
        public void Intern_Lambda_Action()
        {
            var l = (Expression<Action<Nopper>>)(n => n.Nop());

            CloneAndAssert(l);
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
        public void Intern_Lambda_CustomDelegate()
        {
            var p = Expression.Parameter(typeof(int), "p");
            var l = Expression.Lambda<MyFunc>(p, p);

            CloneAndAssert(l);
        }

        private delegate int MyFunc(int x);

        [TestMethod]
        public void Intern_Lambda_HigherOrderWithConflictingNames()
        {
            var a = Expression.Parameter(typeof(int), "x");
            var b = Expression.Parameter(typeof(int), "x");
            var i = Expression.Lambda<Func<int, int>>(a, b);
            var l = Expression.Lambda<Func<int, Func<int, int>>>(i, a);

            CloneAndAssert(l);
        }

        [TestMethod]
        public void Intern_Lambda_HigherOrder()
        {
            var a = Expression.Parameter(typeof(int), "a");
            var b = Expression.Parameter(typeof(int), "b");
            var i = Expression.Lambda<Func<int, int>>(a, b);
            var l = Expression.Lambda<Func<int, Func<int, int>>>(i, a);

            CloneAndAssert(l);
        }

        [TestMethod]
        public void Intern_Lambda_UnnamedParameter()
        {
            var p = Expression.Parameter(typeof(int), "p");
            var l = Expression.Lambda<Func<int, int>>(p, p);

            CloneAndAssert(l);
        }

        [TestMethod]
        public void Intern_Lambda_Y()
        {
            Expression<Rec<int, int>> f = fib => n => n > 1 ? fib(fib)(n - 1) + fib(fib)(n - 2) : n;

            CloneAndAssert(f);
        }

        private delegate Func<A, R> Rec<A, R>(Rec<A, R> r);

        [TestMethod]
        public void Intern_Lambda_ImplicitCovariance()
        {
            Expression<Func<int>> f = () => Q.Foo(() => new QFoo());

            CloneAndAssert(f);
        }

        public interface IQBar
        {
            int Qux();
        }

        public interface IQFoo : IQBar
        {
        }

        public class QFoo : IQFoo
        {
            public int Qux()
            {
                return 42;
            }
        }

        public class Q
        {
            public static int Foo(Expression<Func<IQBar>> e)
            {
                return e.Compile()().Qux();
            }
        }

        [TestMethod]
        public void Intern_Parameter_Unbound()
        {
            var ps = new[]
            {
                Expression.Parameter(typeof(string), "p"),
                Expression.Parameter(typeof(int), "p"),
            };

            foreach (var p in ps)
            {
                CloneAndAssert(p);
            }
        }

        #endregion

        #region Calls

        [TestMethod]
        public void Intern_Call_StaticWithNoParameters()
        {
            var c = Expression.Call(instance: null, typeof(X).GetMethod("A"));

            CloneAndAssert(c);
        }

        [TestMethod]
        public void Intern_Call_StaticWithParameters()
        {
            var c = Expression.Call(instance: null, typeof(X).GetMethod("B"), Expression.Constant(2));

            CloneAndAssert(c);
        }

        [TestMethod]
        public void Intern_Call_StaticVoidWithNoParameters()
        {
            var c = Expression.Call(instance: null, typeof(X).GetMethod("C"));

            CloneAndAssert(c);
        }

        [TestMethod]
        public void Intern_Call_StaticVoidWithParameters()
        {
            var c = Expression.Call(instance: null, typeof(X).GetMethod("D"), Expression.Constant(42));

            CloneAndAssert(c);
        }

        [TestMethod]
        public void Intern_Call_StaticGeneric()
        {
            var c = Expression.Call(instance: null, typeof(X).GetMethod("I").MakeGenericMethod(typeof(int)), Expression.Constant(42));

            CloneAndAssert(c);
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
        public void Intern_Call_InstanceVoid()
        {
            Expression<Action<Adder, int>> f = (a, x) => a.Add(x);

            CloneAndAssert(f);
        }

        [TestMethod]
        public void Intern_Call_InstanceVoidOverload()
        {
            Expression<Action<Adder, string>> f = (a, x) => a.Add(x);

            CloneAndAssert(f);
        }

        [TestMethod]
        public void Intern_Call_InstanceVoidGeneric()
        {
            Expression<Action<Adder, Answer>> f = (a, x) => a.Add(x);

            CloneAndAssert(f);
        }

        [TestMethod]
        public void Intern_Call_InstanceReturn()
        {
            Expression<Func<Adder, int, int>> f = (a, x) => a.AddAndRet(x);

            CloneAndAssert(f);
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


        [TestMethod]
        public void Intern_Call_Static1()
        {
            var e1 = Expression.Call(instance: null, typeof(Console).GetMethod("OpenStandardInput", Type.EmptyTypes));
            var e2 = Expression.Call(instance: null, typeof(Console).GetMethod("OpenStandardOutput", Type.EmptyTypes));
            var e3 = Expression.Call(instance: null, typeof(Console).GetMethod("OpenStandardInput", Type.EmptyTypes));

            var i1 = Intern(e1);
            var i2 = Intern(e2);
            var i3 = Intern(e3);

            Assert.AreSame(e1, i1);
            Assert.AreSame(i3, i1);
            Assert.AreNotSame(i1, i2);
            Assert.AreNotSame(i2, i3);
        }

        [TestMethod]
        public void Intern_Call_Static2()
        {
            var e1 = Expression.Call(instance: null, typeof(Console).GetMethod("ReadKey", new[] { typeof(bool) }), Expression.Constant(true));
            var e2 = Expression.Call(instance: null, typeof(Console).GetMethod("ReadKey", new[] { typeof(bool) }), Expression.Constant(false));
            var e3 = Expression.Call(instance: null, typeof(Console).GetMethod("ReadKey", new[] { typeof(bool) }), Expression.Constant(true));

            var i1 = Intern(e1);
            var i2 = Intern(e2);
            var i3 = Intern(e3);

            Assert.AreSame(e1, i1);
            Assert.AreSame(i3, i1);
            Assert.AreNotSame(i1, i2);
            Assert.AreNotSame(i2, i3);
        }

        [TestMethod]
        public void Intern_Call_Instance1()
        {
            var c = Expression.Constant("bar");

            var e1 = Expression.Call(c, typeof(string).GetMethod("ToUpper", Type.EmptyTypes));
            var e2 = Expression.Call(c, typeof(string).GetMethod("ToLower", Type.EmptyTypes));
            var e3 = Expression.Call(c, typeof(string).GetMethod("ToUpper", Type.EmptyTypes));

            var i1 = Intern(e1);
            var i2 = Intern(e2);
            var i3 = Intern(e3);

            Assert.AreSame(e1, i1);
            Assert.AreSame(i3, i1);
            Assert.AreNotSame(i1, i2);
            Assert.AreNotSame(i2, i3);
        }

        [TestMethod]
        public void Intern_Call_Instance2()
        {
            var c = Expression.Constant("bar");

            var e1 = Expression.Call(c, typeof(string).GetMethod("Substring", new[] { typeof(int) }), Expression.Constant(1));
            var e2 = Expression.Call(c, typeof(string).GetMethod("Substring", new[] { typeof(int) }), Expression.Constant(2));
            var e3 = Expression.Call(c, typeof(string).GetMethod("Substring", new[] { typeof(int) }), Expression.Constant(1));

            var i1 = Intern(e1);
            var i2 = Intern(e2);
            var i3 = Intern(e3);

            Assert.AreSame(e1, i1);
            Assert.AreSame(i3, i1);
            Assert.AreNotSame(i1, i2);
            Assert.AreNotSame(i2, i3);
        }

        #endregion

        #region Invocation

        [TestMethod]
        public void Intern_Invocation()
        {
            var e = (Expression<Func<Func<int, int>, int, int>>)((f, x) => f(x));

            CloneAndAssert(e);
        }

        [TestMethod]
        public void Intern_Invocation2()
        {
            var d1 = Expression.Constant(new Func<int, int>(x => x));
            var d2 = Expression.Constant(new Func<int, int>(x => x));

            var e1 = Expression.Invoke(d1, Expression.Constant(1));
            var e2 = Expression.Invoke(d2, Expression.Constant(1));
            var e3 = Expression.Invoke(d1, Expression.Constant(1));

            var i1 = Intern(e1);
            var i2 = Intern(e2);
            var i3 = Intern(e3);

            Assert.AreSame(e1, i1);
            Assert.AreSame(i3, i1);
        }

        #endregion

        #region Members

        [TestMethod]
        public void Intern_Members_Instance_Property()
        {
            Expression<Func<Instancy, string>> f = i => i.Value;

            CloneAndAssert(f);
        }

        [TestMethod]
        public void Intern_Members_Instance_Field()
        {
            Expression<Func<Instancy, int>> f = i => i._field;

            CloneAndAssert(f);
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
        public void Intern_Members_Static_Property()
        {
            Expression<Func<string>> f = () => Staticy.Value;

            CloneAndAssert(f);
        }

        [TestMethod]
        public void Intern_Members_Static_Field()
        {
            Expression<Func<int>> f = () => Staticy._field;

            CloneAndAssert(f);
        }

        [TestMethod]
        public void Intern_Members_Indexed_Property()
        {
            var p = Expression.Parameter(typeof(Instancy));
            var idx1 = Expression.MakeIndex(p, typeof(Instancy).GetProperty("Item", new[] { typeof(int) }), new[] { Expression.Constant(0) });
            var f1 = Expression.Lambda<Func<Instancy, int>>(idx1, p);

            CloneAndAssert(f1);

            var idx2 = Expression.MakeIndex(p, typeof(Instancy).GetProperty("Item", new[] { typeof(string), typeof(int) }), new[] { Expression.Constant("foo"), Expression.Constant(0) });
            var f2 = Expression.Lambda<Func<Instancy, string>>(idx2, p);

            CloneAndAssert(f2);
        }

        private static class Staticy
        {
            public static int _field = 42;

            public static string Value => "Hello";
        }

        [TestMethod]
        public void Intern_Member_Static()
        {
            var e1 = Expression.MakeMemberAccess(expression: null, typeof(Environment).GetProperty("MachineName"));
            var e2 = Expression.MakeMemberAccess(expression: null, typeof(Environment).GetProperty("UserName"));
            var e3 = Expression.MakeMemberAccess(expression: null, typeof(Environment).GetProperty("MachineName"));

            var i1 = Intern(e1);
            var i2 = Intern(e2);
            var i3 = Intern(e3);

            Assert.AreSame(e1, i1);
            Assert.AreSame(i3, i1);
            Assert.AreNotSame(i1, i2);
            Assert.AreNotSame(i2, i3);
        }

        [TestMethod]
        public void Intern_Member_Instance()
        {
            var ad = Expression.Constant(AppDomain.CurrentDomain);

            var e1 = Expression.MakeMemberAccess(ad, typeof(AppDomain).GetProperty("BaseDirectory"));
            var e2 = Expression.MakeMemberAccess(ad, typeof(AppDomain).GetProperty("DynamicDirectory"));
            var e3 = Expression.MakeMemberAccess(ad, typeof(AppDomain).GetProperty("BaseDirectory"));

            var i1 = Intern(e1);
            var i2 = Intern(e2);
            var i3 = Intern(e3);

            Assert.AreSame(e1, i1);
            Assert.AreSame(i3, i1);
            Assert.AreNotSame(i1, i2);
            Assert.AreNotSame(i2, i3);
        }

        #endregion

        #region Indexers

        [TestMethod]
        public void Intern_ArrayIndex_RankOne()
        {
            var e = (Expression<Func<string[], int, string>>)((ss, i) => /* ArrayIndex */ ss[i]);

            CloneAndAssert(e);
        }

        [TestMethod]
        public void Intern_ArrayIndex_HigherRank()
        {
            var e = (Expression<Func<string[,], int, int, string>>)((ss, i, j) => ss[i, j]);

            CloneAndAssert(e);
        }

        [TestMethod]
        public void Intern_Indexer()
        {
            var e = (Expression<Func<Indexy, string, int>>)((i, s) => i[s]);

            CloneAndAssert(e);
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
        public void Intern_Default()
        {
            var e = Expression.Default(typeof(string));

            CloneAndAssert(e);
        }

        [TestMethod]
        public void Intern_Default2()
        {
            var e1 = Expression.Default(typeof(int));
            var e2 = Expression.Default(typeof(string));
            var e3 = Expression.Default(typeof(int));

            var i1 = Intern(e1);
            var i2 = Intern(e2);
            var i3 = Intern(e3);

            Assert.AreSame(e1, i1);
            Assert.AreSame(i3, i1);
            Assert.AreNotSame(i1, i2);
            Assert.AreNotSame(i2, i3);
        }

        #endregion

        #region Anonymous types

#pragma warning disable IDE0050 // Convert to tuple. (Test for anonymous types.)

        [TestMethod]
        public void Intern_AnonymousType_OriginalTypeReused()
        {
            Expression<Func<object>> x = () => new { x = 1, y = 2 };

            CloneAndAssert(x);
        }

#pragma warning restore IDE0050

        [TestMethod]
        public void Intern_AnonymousType_KeysReconstructed()
        {
            var anon = RuntimeCompiler.CreateAnonymousType(new[]
            {
                new KeyValuePair<string, Type>("foo", typeof(int)),
                new KeyValuePair<string, Type>("bar", typeof(int))
            }, new string[] { "bar" });

            Assert.IsNotNull(anon.GetProperty("bar"));
            Assert.IsNotNull(anon.GetProperty("foo"));

            var m = Expression.New(anon.GetConstructors().Single(), Expression.Constant(1), Expression.Constant(2));

            CloneAndAssert(m);
        }

#pragma warning disable IDE0050 // Convert to tuple. (Test for anonymous types.)

        [TestMethod]
        public void Intern_AnonymousType_TypeReconstructed()
        {
            Expression<Func<object>> x = () => new { x = 1, y = 2 };

            CloneAndAssert(x.Body);
        }

#pragma warning restore IDE0050

        [TestMethod]
        public void Intern_AnonymousType_TypeReconstructed_VisualBasic()
        {
            var x = ((LambdaExpression)VisualBasicModule.GetAnonymousTypeWithKeysExpression()).Body;

            CloneAndAssert(x);
        }

        #endregion

        #region Record types

        [TestMethod]
        public void Intern_RecordType_WithValueEqualitySemantics()
        {
            var rec = RuntimeCompiler.CreateRecordType(new[]
            {
                new KeyValuePair<string, Type>("bar", typeof(int))
            }, valueEquality: true);

            var bar = rec.GetProperty("bar");

            var m = Expression.MemberInit(Expression.New(rec), Expression.Bind(bar, Expression.Constant(42)));

            CloneAndAssert(m);
        }

        [TestMethod]
        public void Intern_RecordType_WithoutValueEqualitySemantics()
        {
            var rec = RuntimeCompiler.CreateRecordType(new[]
            {
                new KeyValuePair<string, Type>("bar", typeof(int))
            }, valueEquality: false);

            var bar = rec.GetProperty("bar");

            var m = Expression.MemberInit(Expression.New(rec), Expression.Bind(bar, Expression.Constant(42)));

            CloneAndAssert(m);
        }

        [TestMethod]
        public void Intern_RecordType_TypeReconstructed()
        {
            var rec = RuntimeCompiler.CreateRecordType(new[]
            {
                new KeyValuePair<string, Type>("bar", typeof(int))
            }, valueEquality: true);

            var bar = rec.GetProperty("bar");

            var m = Expression.MemberInit(Expression.New(rec), Expression.Bind(bar, Expression.Constant(42)));

            CloneAndAssert(m);
        }

        #endregion

        #region Conditional

        [TestMethod]
        public void Intern_Conditional()
        {
            Expression<Func<bool, int>> f = b => b ? 3 : 4;

            CloneAndAssert(f);
        }

        [TestMethod]
        public void Intern_Conditional_WithType()
        {
            var c = Expression.Condition(Expression.Constant(true, typeof(bool)), Expression.New(typeof(B)), Expression.New(typeof(C)), typeof(A));

            CloneAndAssert(c);
        }

        private class A { }

        private class B : A { }

        private sealed class C : B { }

        #endregion

        #region Object creation and initialization

        [TestMethod]
        public void Intern_Init_Arrays()
        {
            Expression<Func<int[]>> f = () => new[] { 1, 2, 3 };

            CloneAndAssert(f);
        }

        [TestMethod]
        public void Intern_Init_List()
        {
            Expression<Func<List<int>>> f = () => new List<int> { 1, 2, 3 };

            CloneAndAssert(f);
        }

        [TestMethod]
        public void Intern_Init_Object()
        {
            var e = (Expression<Func<Qux>>)(() => new Qux(42) { Baz = "Hello", Bar = { Zuz = 24 }, Foos = { 1, 2, 3 } });

            CloneAndAssert(e);
        }

        [TestMethod]
        public void Intern_New_Array_Bounds()
        {
            var e = Expression.NewArrayBounds(typeof(int), Expression.Constant(1));

            CloneAndAssert(e);
        }

        [TestMethod]
        public void Intern_New_ValueType_DefaultConstructor()
        {
            var e = Expression.Lambda(Expression.New(typeof(int)));

            CloneAndAssert(e);
        }

        private sealed class Qux
        {
            public Qux(int x)
            {
                X = x;
                Foos = new List<int>();
                Bar = new Bar2();
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

            public Bar2 Bar
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

        private sealed class Bar2
        {
            public int Zuz = 1;
        }

        #endregion

        #region Closure support

        [TestMethod]
        public void Intern_Closures()
        {
            var x = 42;
            var y = 43;
            Expression<Func<int>> f = () => x + y;

            CloneAndAssert(f);
        }

        #endregion

        #region End-to-end tests

#pragma warning disable IDE0050 // Convert to tuple. (Test for anonymous types.)

        [TestMethod]
        public void Intern_E2E_QueryWithTransparentIdentifiers()
        {
            Expression<Func<IEnumerable>> f = () => from x in Enumerable.Range(0, 10)
                                                    let y = x * 2
                                                    let z = x + y
                                                    where z > 5
                                                    select new { x, y, z };

            CloneAndAssert(f);
        }

#pragma warning restore IDE0050

        [TestMethod]
        public void Intern_E2E_Meta()
        {
            Expression<Func<Expression>> f = () => Expression.Lambda<Func<int>>(Expression.Constant(42));

            CloneAndAssert(f);
        }

        [TestMethod]
        public void Intern_E2E_Jacquard()
        {
            Expression<Func<IQueryable<string>, IQueryable<string>>> f = urlsToSearch =>
                from url in urlsToSearch
                let data = File.OpenRead(url)
                let reader = new StreamReader(data)
                let s = reader.ReadToEnd()
                where s.Substring(0, 64).Dump(url)
                where data.Close()
                where reader.Close()
                select s;

            CloneAndAssert(f);
        }

        [TestMethod]
        public void Intern_E2E_GenericParameterBindings()
        {
            Expression<Func<List<int>, List<int>, List<int>, List<int>>> f = (x, y, z) => Operators.Concat(Operators.Concat(x, y, z), x, 0);

            CloneAndAssert(f);
        }

        [TestMethod]
        public void Intern_Reuse_Simple1()
        {
            var e1 = Expression.Constant(1);
            var e2 = Expression.Add(Expression.Constant(1), Expression.Constant(2));
            var e3 = Expression.Constant(2);

            var i1 = Intern(e1);
            var i2 = Intern(e2);
            var i3 = Intern(e3);

            Assert.AreSame(i1, i2.Left);
            Assert.AreSame(i2.Right, i3);
        }

        [TestMethod]
        public void Intern_ManOrBoy1()
        {
            var xs = new[] { 2, 3, 5 }.AsQueryable();

#pragma warning disable IDE0075 // Simplify conditional expression (in expression tree to test different types of expressions)
            var ys = from x in xs
                     where x > 42 && !(-x < 17)
                     where x.ToString().StartsWith("a")
                     where Console.ReadLine().StartsWith("foo")
                     where Environment.MachineName.Length > 1
                     where new int[2].Length <= 3
                     where new int[] { 5, 7 }.Length >= 13 ? true : false
                     where new List<int> { 19, 23 }.Count != 29
                     where new object[] { "qux" }[0] is string
                     where new string('*', 31).Contains("**")
                     where new Foo { Bar = { Baz = { 37, 41 }, Qux = 43 } }.Bar.Baz.Sum() > 47
                     select ~(x * x);

            var zs = from y in xs
                     where y > 42 && !(-y < 17)
                     where y.ToString().StartsWith("a")
                     where Console.ReadLine().StartsWith("foo")
                     where Environment.MachineName.Length > 1
                     where new int[2].Length <= 3
                     where new int[] { 5, 7 }.Length >= 13 ? true : false
                     where new List<int> { 19, 23 }.Count != 29
                     where new object[] { "qux" }[0] is string
                     where new string('*', 31).Contains("**")
                     where new Foo { Bar = { Baz = { 37, 41 }, Qux = 43 } }.Bar.Baz.Sum() > 47
                     select ~(y * y);
#pragma warning restore IDE0075 // Simplify conditional expression

            var e1 = ys.Expression;
            var e2 = xs.Expression;
            var e3 = zs.Expression;

            var i1 = Intern(e1);
            var i2 = Intern(e2);
            var i3 = Intern(e3);

            Assert.AreSame(e1, i1);
            Assert.AreSame(i3, i1);
        }

        [TestMethod]
        public void Intern_Repro()
        {
            var e1 = (Expression<Func<bool>>)(() => new List<int> { 17, 19 }.Count != 55);
            var e2 = (Expression<Func<bool>>)(() => new List<int> { 17, 19 }.Count != 55);

            var i1 = Intern(e1);
            var i2 = Intern(e2);

            Assert.AreSame(i1, i2);
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

        #region Sub-expression tests

        [TestMethod]
        public void Intern_ElementInit()
        {
            var a = (Expression<Func<WithMemberList>>)(() => new WithMemberList { Inner = { 1, 2, 3 } });
            var b = (Expression<Func<List<int>>>)(() => new List<int> { 1, 2, 3 });

            var i1 = Intern(a).Body as MemberInitExpression;
            var mlBinding = i1.Bindings[0] as MemberListBinding;
            var i2 = Intern(b).Body as ListInitExpression;

            for (var i = 0; i < mlBinding.Initializers.Count; ++i)
            {
                Assert.AreSame(mlBinding.Initializers[i], i2.Initializers[i]);
            }
        }

        [TestMethod]
        public void Intern_MemberBind()
        {
            var a = (Expression<Func<WithMember>>)(() => new WithMember { Value = 1 });
            var b = (Expression<Func<WithMemberMember>>)(() => new WithMemberMember { Member = { Value = 1 } });

            var i1 = Intern(a).Body as MemberInitExpression;
            var i2 = Intern(b).Body as MemberInitExpression;

            var mmb = i2.Bindings[0] as MemberMemberBinding;

            for (var i = 0; i < i1.Bindings.Count; ++i)
            {
                Assert.AreSame(i1.Bindings[i], mmb.Bindings[i]);
            }
        }

        [TestMethod]
        public void Intern_MemberMemberBind()
        {
            var a = (Expression<Func<WithMemberMember>>)(() => new WithMemberMember { Member = { Value = 1 } });
            var b = (Expression<Func<WithMemberMemberMember>>)(() => new WithMemberMemberMember { MemberMember = { Member = { Value = 1 } } });

            var i1 = Intern(a).Body as MemberInitExpression;
            var i2 = Intern(b).Body as MemberInitExpression;

            var mmb = i2.Bindings[0] as MemberMemberBinding;

            for (var i = 0; i < i1.Bindings.Count; ++i)
            {
                Assert.AreSame(i1.Bindings[i], mmb.Bindings[i]);
            }
        }

        [TestMethod]
        public void Intern_MemberListBind()
        {
            var a = (Expression<Func<WithMemberList>>)(() => new WithMemberList { Inner = { 1, 2, 3 } });
            var b = (Expression<Func<WithMemberMemberList>>)(() => new WithMemberMemberList { MemberList = { Inner = { 1, 2, 3 } } });

            var i1 = Intern(a).Body as MemberInitExpression;
            var i2 = Intern(b).Body as MemberInitExpression;

            var mmb = i2.Bindings[0] as MemberMemberBinding;

            for (var i = 0; i < i1.Bindings.Count; ++i)
            {
                Assert.AreSame(i1.Bindings[i], mmb.Bindings[i]);
            }
        }

        [TestMethod]
        public void Intern_ReplaceElementInitChild()
        {
            var a = Expression.Constant(1, typeof(int));
            var b = (Expression<Func<List<int>>>)(() => new List<int> { 1, 2, 3 });

            var i1 = Intern(a);
            var i2 = Intern(b.Body) as ListInitExpression;

            Assert.AreSame(i1, i2.Initializers[0].Arguments[0]);
        }

        [TestMethod]
        public void Intern_ReplaceMemberListChild()
        {
            var a = Expression.Constant(1, typeof(int));
            var b = (Expression<Func<WithMemberList>>)(() => new WithMemberList { Inner = { 1, 2, 3 } });

            var i1 = Intern(a);
            var i2 = Intern(b.Body) as MemberInitExpression;
            var mlb = i2.Bindings[0] as MemberListBinding;

            Assert.AreSame(i1, mlb.Initializers[0].Arguments[0]);
        }

        [TestMethod]
        public void Intern_ReplaceMemberAssignChild()
        {
            var a = Expression.Constant(1, typeof(int));
            var b = (Expression<Func<WithMember>>)(() => new WithMember { Value = 1 });

            var i1 = Intern(a);
            var i2 = Intern(b.Body) as MemberInitExpression;
            var ma = i2.Bindings[0] as MemberAssignment;

            Assert.AreSame(i1, ma.Expression);
        }

        [TestMethod]
        public void Intern_ReplaceNewArrayChild()
        {
            var a = Expression.Constant(1, typeof(int));
            var b = (Expression<Func<int[]>>)(() => new int[] { 1, 2, 3 });

            var i1 = Intern(a);
            var i2 = Intern(b.Body) as NewArrayExpression;

            Assert.AreSame(i1, i2.Expressions[0]);
        }

        [TestMethod]
        public void Intern_ReplaceTypeBinaryChild()
        {
            var a = Expression.Constant(1, typeof(int));
            var b = Expression.TypeIs(Expression.Constant(1, typeof(int)), typeof(int));

            var i1 = Intern(a);
            var i2 = Intern(b);

            Assert.AreSame(i1, i2.Expression);
        }

        [TestMethod]
        public void Intern_ReplaceUnaryChild()
        {
            var a = Expression.Constant(1, typeof(int));
            var b = Expression.Negate(Expression.Constant(1, typeof(int)));

            var i1 = Intern(a);
            var i2 = Intern(b);

            Assert.AreSame(i1, i2.Operand);
        }

        [TestMethod]
        public void Intern_ReplaceBinaryConversionChild()
        {
            var a = Expression.Constant("1", typeof(string));
            var f = (Expression<Func<string, int>>)(s => int.Parse(s));
            var b = Expression.Coalesce(Expression.Constant("1", typeof(string)), Expression.Constant(1), f);

            var i1 = Intern(a);
            var i2 = Intern(b);

            Assert.AreSame(i1, i2.Left);
        }

        [TestMethod]
        public void Intern_ReplaceStaticMethodCall()
        {
            var a = Expression.Constant(1, typeof(int));
            var b = (Expression<Func<int>>)(() => WithStatics.Ret(1));

            var i1 = Intern(a);
            var i2 = Intern(b.Body) as MethodCallExpression;

            Assert.AreSame(i1, i2.Arguments[0]);
        }

        [TestMethod]
        public void Intern_ReplaceInstanceMethodCall()
        {
            var a = Expression.Constant(1, typeof(int));
            var b = (Expression<Func<WithMethod, int>>)(x => x.Ret(1));

            var i1 = Intern(a);
            var i2 = Intern(b.Body) as MethodCallExpression;

            Assert.AreSame(i1, i2.Arguments[0]);
        }

        private sealed class WithMemberList
        {
            public WithMemberList()
            {
                Inner = new List<int>();
            }

            public List<int> Inner
            {
                get;
                set;
            }
        }

        private sealed class WithMemberMemberList
        {
            public WithMemberList MemberList
            {
                get;
                set;
            }
        }

        private sealed class WithMemberMemberMember
        {
            public WithMemberMember MemberMember
            {
                get;
                set;
            }
        }

        private sealed class WithMemberMember
        {
            public WithMember Member
            {
                get;
                set;
            }
        }

        private sealed class WithMember
        {
            public int Value
            {
                get;
                set;
            }
        }

        private sealed class WithStatics
        {
            public static int Ret(int x)
            {
                return x;
            }
        }

#pragma warning disable CA1822 // Mark static
        private sealed class WithMethod
        {
            public int Ret(int x)
            {
                return x;
            }
        }
#pragma warning restore CA1822

        #endregion

        #region Unbound Parameters Support

        [TestMethod]
        public void Intern_ExpressionWithUnboundParameters()
        {
            var p1 = Expression.Parameter(typeof(int), "myGlobal");
            var p2 = Expression.Parameter(typeof(int), "myGlobal");
            var a = Expression.Negate(p1);
            var b = Expression.Negate(p2);

            var i1 = a.Intern();
            var i2 = b.Intern();

            Assert.AreNotSame(i1, i2);

            var cache = new ExpressionInterningCache(() => new NamedGlobalParameterExpressionEqualityComparator());
            var i3 = a.Intern(cache);
            var i4 = b.Intern(cache);

            Assert.AreSame(i3, i4);
        }

        private sealed class NamedGlobalParameterExpressionEqualityComparator : ExpressionEqualityComparator
        {
            protected override bool EqualsGlobalParameter(ParameterExpression x, ParameterExpression y)
            {
                return Equals(x.Type, y.Type) && x.Name == y.Name;
            }
        }

        #endregion

        #region Test Helpers

        private static void CloneAndAssert(Expression c)
        {
            var ex = c;
            var free = FreeVariableScanner.Scan(c).ToList();
            if (free.Count > 0)
            {
                ex = Expression.Lambda(c, free);
            }
            var copy = ex.Clone();
            var e1 = Intern(ex);
            var e2 = Intern(copy);
            Assert.IsTrue(new ClonedExpressionEqualityComparator().Equals(ex, copy));
            Assert.AreSame(e1, e2);
        }

        private static T Intern<T>(T e)
            where T : Expression
        {
            var res = e.Intern();

            Assert.IsTrue(new ExpressionEqualityComparer().Equals(e, res));

            return res;
        }

        private sealed class Foo
        {
            public Bar Bar { get; set; }
        }

        private sealed class Bar
        {
            public int Qux { get; set; }
            public List<int> Baz { get; set; }
        }

        #endregion
    }

    #region Test Helper Types

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

    #endregion
}
