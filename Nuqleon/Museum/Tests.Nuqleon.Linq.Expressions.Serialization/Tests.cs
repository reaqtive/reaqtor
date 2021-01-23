// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - December 2012
//

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Linq.CompilerServices;
using System.Linq.Expressions;
using System.Numerics;
using System.Reflection;
using System.Threading.Tasks;

using Nuqleon.Linq.Expressions.Serialization;

using CSharpDynamic = Microsoft.CSharp.RuntimeBinder;

namespace Tests
{
    [TestClass]
    public class Tests
    {
        #region Test initialization and cleanup

        [TestInitialize]
        public void Initialize()
        {
            //
            // Trigger rule table load.
            //
            _ = ExpressionJsonSerializer.Instance;
        }

        #endregion

        #region Constant

        [TestMethod]
        public void Constant_BuiltIn()
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
                RoundtripAndAssert(c);
            }
        }

        [TestMethod]
        public void Constant_Null()
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
        public void Constant_Tuple()
        {
            var cs = new[]
            {
                Expression.Constant(Tuple.Create(42)),
                Expression.Constant(Tuple.Create(42, "bar")),
                Expression.Constant(Tuple.Create(42, "bar", false)),
                Expression.Constant(Tuple.Create(42, "bar", false, 12.34)),
                Expression.Constant(Tuple.Create(42, "bar", false, 12.34, 'c')),
                Expression.Constant(Tuple.Create(42, "bar", false, 12.34, 'c', DateTime.Now)),
                Expression.Constant(Tuple.Create(42, "bar", false, 12.34, 'c', DateTime.Now, TimeSpan.Zero)),
                
                // TODO BUG: this doesn't roundtrip
                //Expression.Constant(Tuple.Create(42, "bar", false, 12.34, 'c', DateTime.Now, TimeSpan.Zero, 49.95m)),
            };

            foreach (var c in cs)
            {
                RoundtripAndAssert(c);
            }
        }

        [TestMethod]
        public void Constant_Collections()
        {
            var xs = new List<int> { 1, 2, 3 };

            var ce = Expression.Constant(xs);

            var r = Roundtrip(ce);

            Assert.IsNotNull(r);
            Assert.AreEqual(ExpressionType.Constant, r.NodeType);

            var rc = (ConstantExpression)r;
            Assert.IsTrue(rc.Value is not null and List<int>);
            Assert.IsTrue(Enumerable.SequenceEqual(xs, (List<int>)rc.Value));
        }

        private static void RoundtripAndAssert(ConstantExpression c)
        {
            var r = Roundtrip(c) as ConstantExpression;
            Assert.IsNotNull(r);

            Assert.AreEqual(c.Type, r.Type);
            Assert.AreEqual(c.NodeType, r.NodeType);
            Assert.AreEqual(c.Value, r.Value);
        }

        #endregion

        #region Unary

        [TestMethod]
        public void Unary_Arith()
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
        public void Unary_Logical()
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
        public void Unary_Compound()
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

        private static void RoundtripAndAssert(UnaryExpression a)
        {
            var r = Roundtrip(a) as UnaryExpression;
            Assert.IsNotNull(r);

            Assert.AreEqual(a.Type, r.Type);
            Assert.AreEqual(a.NodeType, r.NodeType);
            Assert.AreEqual(a.Method, r.Method);
            Assert.AreEqual(a.IsLifted, r.IsLifted);
            Assert.AreEqual(a.IsLiftedToNull, r.IsLiftedToNull);
            Assert.AreEqual(a.Operand.NodeType, r.Operand.NodeType);
        }

        #endregion

        #region Binary

        [TestMethod]
        public void Binary_Arith_BuiltIn()
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
        public void Binary_Arith_Power()
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
                RoundtripAndAssert(a);
            }
        }

        [TestMethod]
        public void Binary_Arith_Lifted()
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
        public void Binary_OperatorOverloads()
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
        public void Binary_Relational()
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
        public void Binary_Logical()
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
        public void Binary_Coalesce()
        {
            var e = (Expression<Func<string, string>>)(s => s ?? "null");
            var f = e.Compile();

            var g = (Expression<Func<string, string>>)Roundtrip(e);
            var h = g.Compile();

            Assert.AreEqual(f(null), h(null));
            Assert.AreEqual(f("Bar"), h("Bar"));
        }

        [TestMethod]
        public void Binary_CompoundAssignment()
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
        }

        #endregion

        #region Conversion

        [TestMethod]
        public void Convert_Implicit()
        {
            var e = (Expression<Func<int, BigInteger>>)(i => i);
            var d = Roundtrip(e) as Expression<Func<int, BigInteger>>;

            Assert.IsNotNull(d);
            Assert.AreEqual(new BigInteger(42), d.Compile()(42));
        }

        [TestMethod]
        public void Convert_Explicit()
        {
            var e = (Expression<Func<BigInteger, int>>)(i => (int)i);
            var d = Roundtrip(e) as Expression<Func<BigInteger, int>>;

            Assert.IsNotNull(d);
            Assert.AreEqual(42, d.Compile()(new BigInteger(42)));
        }

        #endregion

        #region Lambda and parameters

        [TestMethod]
        public void Lambda_IdentityFunc()
        {
            var p = Expression.Parameter(typeof(int), "_");
            var l = Expression.Lambda<Func<int, int>>(p, p);

            var r = Roundtrip(l) as LambdaExpression;
            Assert.IsNotNull(r);

            var f = (Func<int, int>)r.Compile();
            Assert.AreEqual(42, f(42));
        }

        [TestMethod]
        public void Lambda_Action()
        {
            var l = (Expression<Action<Nopper>>)(n => n.Nop());

            var r = Roundtrip(l) as LambdaExpression;
            Assert.IsNotNull(r);

            var f = (Action<Nopper>)r.Compile();

            var nopper = new Nopper();
            f(nopper);
            Assert.IsTrue(nopper.HasNopped);
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
        public void Lambda_CustomDelegate()
        {
            var p = Expression.Parameter(typeof(int), "p");
            var l = Expression.Lambda<MyFunc>(p, p);

            var r = Roundtrip(l) as LambdaExpression;
            Assert.IsNotNull(r);

            var f = (MyFunc)r.Compile();
            Assert.AreEqual(42, f(42));
        }

        private delegate int MyFunc(int x);

        [TestMethod]
        public void Lambda_HigherOrderWithConflictingNames()
        {
            var a = Expression.Parameter(typeof(int), "x");
            var b = Expression.Parameter(typeof(int), "x");
            var i = Expression.Lambda<Func<int, int>>(a, b);
            var l = Expression.Lambda<Func<int, Func<int, int>>>(i, a);

            var r = Roundtrip(l) as LambdaExpression;
            Assert.IsNotNull(r);

            var f = (Func<int, Func<int, int>>)r.Compile();
            Assert.AreEqual(2, f(2)(3));
        }

        [TestMethod]
        public void Lambda_HigherOrder()
        {
            var a = Expression.Parameter(typeof(int), "a");
            var b = Expression.Parameter(typeof(int), "b");
            var i = Expression.Lambda<Func<int, int>>(a, b);
            var l = Expression.Lambda<Func<int, Func<int, int>>>(i, a);

            var r = Roundtrip(l) as LambdaExpression;
            Assert.IsNotNull(r);

            var f = (Func<int, Func<int, int>>)r.Compile();
            Assert.AreEqual(2, f(2)(3));
        }

        [TestMethod]
        public void Lambda_UnnamedParameter()
        {
            var p = Expression.Parameter(typeof(int), "p");
            var l = Expression.Lambda<Func<int, int>>(p, p);

            var r = Roundtrip(l) as LambdaExpression;
            Assert.IsNotNull(r);

            var f = (Func<int, int>)r.Compile();
            Assert.AreEqual(42, f(42));
        }

        [TestMethod]
        public void Lambda_Y()
        {
            Expression<Rec<int, int>> f = fib => n => n > 1 ? fib(fib)(n - 1) + fib(fib)(n - 2) : n;

            var r = Roundtrip(f) as LambdaExpression;
            Assert.IsNotNull(r);

            var fibRec = (Rec<int, int>)r.Compile();
            var g = fibRec(fibRec);
            Assert.AreEqual(8, g(6));
        }

        private delegate Func<A, R> Rec<A, R>(Rec<A, R> r);

        [TestMethod]
        public void Lambda_ImplicitCovariance()
        {
            Expression<Func<int>> f = () => Q.Foo(() => new Foo());

            var e = ((Expression<Func<int>>)Roundtrip(f)).Compile();

            Assert.AreEqual(f.Compile()(), e());
        }

        [TestMethod]
        public void Parameter_Unbound()
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
            }
        }

        #endregion

        #region Calls

        [TestMethod]
        public void Call_StaticWithNoParameters()
        {
            var c = Expression.Call(instance: null, typeof(X).GetMethod("A"));

            var r = Roundtrip(c) as MethodCallExpression;
            Assert.IsNotNull(r);

            var f = Expression.Lambda<Func<int>>(r).Compile();
            Assert.AreEqual(42, f());
        }

        [TestMethod]
        public void Call_StaticWithParameters()
        {
            var c = Expression.Call(instance: null, typeof(X).GetMethod("B"), Expression.Constant(2));

            var r = Roundtrip(c) as MethodCallExpression;
            Assert.IsNotNull(r);

            var f = Expression.Lambda<Func<int>>(r).Compile();
            Assert.AreEqual(42 * 2, f());
        }

        [TestMethod]
        public void Call_StaticVoidWithNoParameters()
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
        }

        [TestMethod]
        public void Call_StaticVoidWithParameters()
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
        }

        [TestMethod]
        public void Call_StaticGeneric()
        {
            var c = Expression.Call(instance: null, typeof(X).GetMethod("I").MakeGenericMethod(typeof(int)), Expression.Constant(42));

            var r = Roundtrip(c) as MethodCallExpression;
            Assert.IsNotNull(r);

            var f = Expression.Lambda<Func<int>>(r).Compile();
            Assert.AreEqual(42, f());
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
        public void Call_InstanceVoid()
        {
            Expression<Action<Adder, int>> f = (a, x) => a.Add(x);

            var g = ((Expression<Action<Adder, int>>)Roundtrip(f)).Compile();

            var adder = new Adder();
            g(adder, 3);
            g(adder, 1);
            g(adder, 2);

            Assert.AreEqual(6, adder.Count);
        }

        [TestMethod]
        public void Call_InstanceVoidOverload()
        {
            Expression<Action<Adder, string>> f = (a, x) => a.Add(x);

            var g = ((Expression<Action<Adder, string>>)Roundtrip(f)).Compile();

            var adder = new Adder();
            g(adder, "***");
            g(adder, "*");
            g(adder, "**");

            Assert.AreEqual(6, adder.Count);
        }

        [TestMethod]
        public void Call_InstanceVoidGeneric()
        {
            Expression<Action<Adder, Answer>> f = (a, x) => a.Add(x);

            var g = ((Expression<Action<Adder, Answer>>)Roundtrip(f)).Compile();

            var adder = new Adder();
            g(adder, new Answer());
            g(adder, new Answer());
            g(adder, new Answer());

            Assert.AreEqual(42 * 3, adder.Count);
        }

        [TestMethod]
        public void Call_InstanceReturn()
        {
            Expression<Func<Adder, int, int>> f = (a, x) => a.AddAndRet(x);

            var g = ((Expression<Func<Adder, int, int>>)Roundtrip(f)).Compile();

            var adder = new Adder();
            g(adder, 3);
            g(adder, 1);
            g(adder, 2);

            Assert.AreEqual(10, g(adder, 4));
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
        public void Invocation()
        {
            var e = (Expression<Func<Func<int, int>, int, int>>)((f, x) => f(x));

            var i = Roundtrip(e) as Expression<Func<Func<int, int>, int, int>>;
            Assert.IsNotNull(i);
            Assert.IsTrue(i.Body is InvocationExpression);

            Assert.AreEqual(7 * 2, i.Compile()(x => x * 2, 7));
        }

        #endregion

        #region Members

        [TestMethod]
        public void Members_Instance_Property()
        {
            Expression<Func<Instancy, string>> f = i => i.Value;

            var e = ((Expression<Func<Instancy, string>>)Roundtrip(f)).Compile();

            var o = new Instancy();
            Assert.AreEqual(e(o), f.Compile()(o));
        }

        [TestMethod]
        public void Members_Instance_Field()
        {
            Expression<Func<Instancy, int>> f = i => i._field;

            var e = ((Expression<Func<Instancy, int>>)Roundtrip(f)).Compile();

            var o = new Instancy();
            Assert.AreEqual(e(o), f.Compile()(o));
        }

        private sealed class Instancy
        {
            public int _field = 42;

            public string Value => _field.ToString();
        }

        [TestMethod]
        public void Members_Static_Property()
        {
            Expression<Func<string>> f = () => Staticy.Value;

            var e = ((Expression<Func<string>>)Roundtrip(f)).Compile();

            Assert.AreEqual(e(), f.Compile()());
        }

        [TestMethod]
        public void Members_Static_Field()
        {
            Expression<Func<int>> f = () => Staticy._field;

            var e = ((Expression<Func<int>>)Roundtrip(f)).Compile();

            Assert.AreEqual(e(), f.Compile()());
        }

        private static class Staticy
        {
            public static int _field = 42;

            public static string Value => "Hello";
        }

        #endregion

        #region Indexers

        [TestMethod]
        public void ArrayIndex_RankOne()
        {
            var e = (Expression<Func<string[], int, string>>)((ss, i) => /* ArrayIndex */ ss[i]);

            var f = Roundtrip(e) as Expression<Func<string[], int, string>>;
            Assert.IsNotNull(f);

            var o = new[] { "foo", "bar", "qux", "baz" };
            Assert.AreEqual(o[2], f.Compile()(o, 2));
        }

        [TestMethod]
        public void ArrayIndex_HigherRank()
        {
            var e = (Expression<Func<string[,], int, int, string>>)((ss, i, j) => /* ArrayIndex */ ss[i, j]);

            var f = Roundtrip(e) as Expression<Func<string[,], int, int, string>>;
            Assert.IsNotNull(f);

            var o = new[,] { { "foo", "bar" }, { "qux", "baz" } };
            Assert.AreEqual(o[1, 1], f.Compile()(o, 1, 1));
        }

        [TestMethod]
        public void Indexer()
        {
            var e = (Expression<Func<Indexy, string, int>>)((i, s) => i[s]);

            var f = Roundtrip(e) as Expression<Func<Indexy, string, int>>;
            Assert.IsNotNull(f);

            var o = new Indexy();
            Assert.AreEqual(o["Hello"], f.Compile()(o, "Hello"));
        }

        private sealed class Indexy
        {
#pragma warning disable CA1822 // Mark members as static (https://github.com/dotnet/roslyn-analyzers/issues/4651)
            public int this[string s] => s.Length;
#pragma warning restore CA1822
        }

        #endregion

        #region Anonymous types

        [TestMethod]
        public void AnonymousType_OriginalTypeReused()
        {
#pragma warning disable IDE0050 // Convert to tuple. (Test for anonymous types.)
            Expression<Func<object>> x = () => new { x = 1, y = 2 };
#pragma warning restore IDE0050

            var e = Roundtrip(x.Body);
            Assert.IsNotNull(e);

            var f = Expression.Lambda<Func<object>>(e).Compile();

            var o = f();
            Assert.AreEqual(1, (int)o.GetType().GetProperty("x").GetValue(o, index: null));
            Assert.AreEqual(2, (int)o.GetType().GetProperty("y").GetValue(o, index: null));

            Assert.AreEqual(x.Compile()().GetType(), o.GetType());
        }

        [TestMethod]
        public void AnonymousType_TypeReconstructed()
        {
#pragma warning disable IDE0050 // Convert to tuple. (Test for anonymous types.)
            Expression<Func<object>> x = () => new { x = 1, y = 2 };
#pragma warning restore IDE0050

            var ser = new ExpressionJsonSerializer();
            var s = ser.Serialize(x.Body);

            var y = s.ToString().Replace("OriginalName", "_OriginalName");
            var t = Nuqleon.Json.Expressions.Expression.Parse(y);

            var e = ser.Deserialize(t);
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
        }

        [TestMethod]
        public void AnonymousType_TypeReconstructed_VisualBasic()
        {
            var x = ((LambdaExpression)VisualBasicModule.GetAnonymousTypeWithKeysExpression()).Body;

            var ser = new ExpressionJsonSerializer();
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
        }

        #endregion

        #region Record types

        [TestMethod]
        public void RecordType_TypeReconstructed()
        {
            var rec = RuntimeCompiler.CreateRecordType(new[]
            {
                new KeyValuePair<string, Type>("bar", typeof(int))
            }, true);

            var bar = rec.GetProperty("bar");

            var m = Expression.MemberInit(Expression.New(rec), Expression.Bind(bar, Expression.Constant(42)));

            var ser = new ExpressionJsonSerializer();
            var s = ser.Serialize(m);

            var y = s.ToString().Replace("OriginalName", "_OriginalName");
            var t = Nuqleon.Json.Expressions.Expression.Parse(y);

            var e = ser.Deserialize(t);
            Assert.IsNotNull(e);

            var o = e.Evaluate();

            Assert.AreEqual(42, (int)o.GetType().GetProperty("bar").GetValue(o, index: null));

            var p = m.Evaluate();
            Assert.AreEqual(o.GetHashCode(), p.GetHashCode());
            Assert.AreEqual(o.ToString(), p.ToString());
            Assert.AreNotEqual(o.GetType(), p.GetType());
        }

        #endregion

        #region Exceptions

        [TestMethod]
        public void Exceptions_Handling()
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
            var f = Expression.Lambda<Action<Action, Action<int>, Action<Exception>>>(             /*
                Expression.TryFault(                                                                */
                    Expression.TryFinally(
                        Expression.TryCatch(
                            Expression.TryFinally(                                                 /*
                                Expression.TryFault(                                                */
                                    Expression.Block(
                                        Expression.Invoke(body),
                                        Expression.Invoke(logi, Expression.Constant(0))
                                    ),                                                             /*
                                    Expression.Invoke(logi, Expression.Constant(1))                 *
                                ),                                                                  */
                                Expression.Invoke(logi, Expression.Constant(2))
                            ),
                            Expression.Catch(ioe,
                                Expression.Block(
                                    Expression.Invoke(loge, ioe),
                                    Expression.Invoke(logi, Expression.Constant(3))
                                )
                            ),                                                                     /*
                            Expression.Catch(ae,                                                    *
                                Expression.Block(                                                   *
                                    Expression.Invoke(loge, ae),                                    *
                                    Expression.Invoke(logi, Expression.Constant(4))                 *
                                ),                                                                  *
                                Expression.Equal(                                                   *
                                    Expression.Property(ae, "ParamName"),                           *
                                    Expression.Constant("bar")                                      *
                                )                                                                   *
                            ),                                                                      */
                            Expression.Catch(e,
                                Expression.Block(
                                    Expression.Invoke(loge, e),
                                    Expression.Invoke(logi, Expression.Constant(5))
                                )
                            )
                        ),
                        Expression.Invoke(logi, Expression.Constant(6))
                    ),                                                                             /*
                  Expression.Invoke(logi, Expression.Constant(7))                                   *
                ),                                                                                  */
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

        #endregion

        #region Conditional

        [TestMethod]
        public void Conditional()
        {
            Expression<Func<bool, int>> f = b => b ? 3 : 4;

            var e = ((Expression<Func<bool, int>>)Roundtrip(f)).Compile();

            Assert.AreEqual(4, e(false));
            Assert.AreEqual(3, e(true));
        }

        #endregion

        #region Object creation and initialization

        [TestMethod]
        public void Init_Arrays()
        {
            Expression<Func<int[]>> f = () => new[] { 1, 2, 3 };

            var e = ((Expression<Func<int[]>>)Roundtrip(f)).Compile();

            Assert.IsTrue(e().SequenceEqual(f.Compile()()));
        }

        [TestMethod]
        public void Init_List()
        {
            Expression<Func<List<int>>> f = () => new List<int> { 1, 2, 3 };

            var e = ((Expression<Func<List<int>>>)Roundtrip(f)).Compile();

            Assert.IsTrue(e().SequenceEqual(f.Compile()()));
        }

        [TestMethod]
        public void Init_Object()
        {
            var e = (Expression<Func<Qux>>)(() => new Qux(42) { Baz = "Hello", Bar = { Zuz = 24 }, Foos = { 1, 2, 3 } });

            var f = (Expression<Func<Qux>>)Roundtrip(e);

            var q = f.Compile()();
            Assert.AreEqual(42, q.X);
            Assert.AreEqual("Hello", q.Baz);
            Assert.AreEqual(24, q.Bar.Zuz);
            Assert.IsTrue(q.Foos.SequenceEqual(new[] { 1, 2, 3 }));
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

        [TestMethod]
        public void Closures()
        {
            var x = 42;
            var y = 43;
            Expression<Func<int>> f = () => x + y;

            var e = ((Expression<Func<int>>)Roundtrip(f)).Compile();

            Assert.AreEqual(f.Compile()(), e());
        }

        #endregion

        #region Dynamic

        //
        // NB: Support for C# binder objects relies on private reflection which differs in Mono's Microsoft.CSharp implementation,
        //     so we skip these tests. Note that this is a Museum project, so we don't really care about fixing this on Mono. The
        //     real fix would be for binder objects to be serializable or expose more of their properties publicly.
        //

        private static bool IsRunningOnMono => Type.GetType("Mono.Runtime") != null;

        [TestMethod]
        public void Dynamic_BinaryOperation()
        {
            if (IsRunningOnMono)
            {
                return;
            }

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
            if (IsRunningOnMono)
            {
                return;
            }

            var op = CSharpDynamic.Binder.BinaryOperation(CSharpDynamic.CSharpBinderFlags.BinaryOperationLogical, ExpressionType.And, typeof(Tests) /* Convertible visibility */, new[]
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
            public static bool operator true(Logi _) => true;
            public static bool operator false(Logi _) => true;
            public static Logi operator &(Logi _1, Logi _2) => new Logi();
            public static Logi operator |(Logi _1, Logi _2) => new Logi();
        }

        [TestMethod]
        public void Dynamic_BinaryOperation_Checked()
        {
            if (IsRunningOnMono)
            {
                return;
            }

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
            var g = f.Compile();

            Assert.ThrowsException<OverflowException>(() => _ = g());
        }

        [TestMethod]
        public void Dynamic_Convert_Explicit_BuiltIn()
        {
            if (IsRunningOnMono)
            {
                return;
            }

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
            if (IsRunningOnMono)
            {
                return;
            }

            var op = CSharpDynamic.Binder.Convert(CSharpDynamic.CSharpBinderFlags.ConvertExplicit, typeof(string), typeof(Tests) /* Convertible visibility */);

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
            if (IsRunningOnMono)
            {
                return;
            }

            var op = CSharpDynamic.Binder.Convert(CSharpDynamic.CSharpBinderFlags.None, typeof(long), typeof(object));

            var value = 42;

            var e = Expression.Dynamic(op, typeof(long), Expression.Constant(value, typeof(object)));
            var d = (DynamicExpression)Roundtrip(e);

            var f = Expression.Lambda<Func<long>>(d);
            Assert.AreEqual(value, f.Compile()());
        }

        [TestMethod]
        public void Dynamic_Convert_Implicit_Custom()
        {
            if (IsRunningOnMono)
            {
                return;
            }

            var op = CSharpDynamic.Binder.Convert(CSharpDynamic.CSharpBinderFlags.None, typeof(int), typeof(Tests) /* Convertible visibility */);

            var p = Expression.Parameter(typeof(Convertible));
            var e = Expression.Lambda<Func<Convertible, int>>(Expression.Dynamic(op, typeof(int), Expression.Convert(p, typeof(object))), p);
            var d = Roundtrip(e);

            var f = (Expression<Func<Convertible, int>>)d;
            var c = new Convertible();
            Assert.AreEqual(c, f.Compile()(c));
        }

        private sealed class Convertible
        {
            public static explicit operator string(Convertible _) => "Boo!";
            public static implicit operator int(Convertible _) => 42;
        }

        [TestMethod]
        public void Dynamic_GetIndex()
        {
            if (IsRunningOnMono)
            {
                return;
            }

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
            if (IsRunningOnMono)
            {
                return;
            }

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
            if (IsRunningOnMono)
            {
                return;
            }

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
            if (IsRunningOnMono)
            {
                return;
            }

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
            if (IsRunningOnMono)
            {
                return;
            }

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
            if (IsRunningOnMono)
            {
                return;
            }

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
            if (IsRunningOnMono)
            {
                return;
            }

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
            if (IsRunningOnMono)
            {
                return;
            }

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
            if (IsRunningOnMono)
            {
                return;
            }

            var op = CSharpDynamic.Binder.SetMember(CSharpDynamic.CSharpBinderFlags.None, "Foo", typeof(Tests) /* MyMemberish visibility */, new[]
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
            if (IsRunningOnMono)
            {
                return;
            }

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

        [TestMethod]
        public void Dynamic_UnaryOperation_Checked()
        {
            if (IsRunningOnMono)
            {
                return;
            }

            var op = CSharpDynamic.Binder.UnaryOperation(CSharpDynamic.CSharpBinderFlags.CheckedContext, ExpressionType.Negate, typeof(object), new[]
            {
                CSharpDynamic.CSharpArgumentInfo.Create(CSharpDynamic.CSharpArgumentInfoFlags.None, name: null),
            });

            var min = int.MinValue;

            var e = Expression.Dynamic(op, typeof(object), Expression.Constant(min));
            var d = (DynamicExpression)Roundtrip(e);

            var f = Expression.Lambda<Func<int>>(Expression.Convert(d, typeof(int)));
            var g = f.Compile();

            Assert.ThrowsException<OverflowException>(() => _ = g());
        }

        #endregion

        #region Type resolution

        [TestMethod]
        public void TypeResolutionOverride()
        {
            var ser = new ExpressionJsonSerializer(RuleOptions.Default, new MyTypeResolver());

            var e = (Expression<Func<Bary, int>>)(b => b.Qux);
            var j = ser.Serialize(e);
            var d = ser.Deserialize(j) as Expression<Func<Fooy, int>>;

            Assert.IsNotNull(d);
            Assert.AreEqual(42, d.Compile()(new Fooy(42)));
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

        #region End-to-end tests

#pragma warning disable IDE0050 // Convert to tuple. (Test for anonymous types.)

        [TestMethod]
        public void E2E_QueryWithTransparentIdentifiers()
        {
            Expression<Func<IEnumerable>> f = () => from x in Enumerable.Range(0, 10)
                                                    let y = x * 2
                                                    let z = x + y
                                                    where z > 5
                                                    select new { x, y, z };

            var e = Expression.Lambda<Func<IEnumerable>>(Roundtrip(f.Body));

            var xs = e.Compile()().Cast<object>();
            var ys = f.Compile()().Cast<object>();

            Assert.IsTrue(xs.SequenceEqual(ys));
        }

#pragma warning restore IDE0050 // Convert to tuple

        [TestMethod]
        public void E2E_Meta()
        {
            Expression<Func<Expression>> f = () => Expression.Lambda<Func<int>>(Expression.Constant(42));

            var e = Roundtrip(f.Body);

            Assert.AreEqual(f.Body.ToString(), e.ToString());
        }

        [TestMethod]
        public void E2E_Jacquard()
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

            Assert.IsTrue(new ExpressionEquality().Equals(f, e));
        }

        [TestMethod]
        public void E2E_Statements()
        {
            var ser = ExpressionJsonSerializer.Instance;

            var to = Expression.Parameter(typeof(int), "to");
            var res = Expression.Variable(typeof(List<int>), "res");
            var n = Expression.Variable(typeof(int), "n");
            var found = Expression.Variable(typeof(bool), "found");
            var d = Expression.Variable(typeof(int), "d");
            var breakOuter = Expression.Label("break_outer");
            var breakInner = Expression.Label("break_inner");
            var getPrimes =
                // Func<int, List<int>> getPrimes =
                Expression.Lambda<Func<int, List<int>>>(
                    // {
                    Expression.Block(
                        // List<int> res;
                        new[] { res },
                        // res = new List<int>();
                        Expression.Assign(
                            res,
                            Expression.New(typeof(List<int>))
                        ),
                        // {
                        Expression.Block(
                            // int n;
                            new[] { n },
                            // n = 2;
                            Expression.Assign(
                                n,
                                Expression.Constant(2)
                            ),
                            // while (true)
                            Expression.Loop(
                                // {
                                Expression.Block(
                                    // if
                                    Expression.IfThen(
                                        // (!
                                        Expression.Not(
                                            // (n <= to)
                                            Expression.LessThanOrEqual(
                                                n,
                                                to
                                            )
                                        // )
                                        ),
                                        // break;
                                        Expression.Break(breakOuter)
                                    ),
                                    // {
                                    Expression.Block(
                                        // bool found;
                                        new[] { found },
                                        // found = false;
                                        Expression.Assign(
                                            found,
                                            Expression.Constant(false)
                                        ),
                                        // {
                                        Expression.Block(
                                            // int d;
                                            new[] { d },
                                            // d = 2;
                                            Expression.Assign(
                                                d,
                                                Expression.Constant(2)
                                            ),
                                            // while (true)
                                            Expression.Loop(
                                                // {
                                                Expression.Block(
                                                    // if
                                                    Expression.IfThen(
                                                        // (!
                                                        Expression.Not(
                                                            // d <= Math.Sqrt(n)
                                                            Expression.LessThanOrEqual(
                                                                d,
                                                                Expression.Convert(
                                                                    Expression.Call(
                                                                        null,
                                                                        typeof(Math).GetMethod("Sqrt"),
                                                                        Expression.Convert(
                                                                            n,
                                                                            typeof(double)
                                                                        )
                                                                    ),
                                                                    typeof(int)
                                                                )
                                                            )
                                                        // )
                                                        ),
                                                        // break;
                                                        Expression.Break(breakInner)
                                                    ),
                                                    // {
                                                    Expression.Block(
                                                        // if (n % d == 0)
                                                        Expression.IfThen(
                                                            Expression.Equal(
                                                                Expression.Modulo(
                                                                    n,
                                                                    d
                                                                ),
                                                                Expression.Constant(0)
                                                            ),
                                                            // {
                                                            Expression.Block(
                                                                // found = true;
                                                                Expression.Assign(
                                                                    found,
                                                                    Expression.Constant(true)
                                                                ),
                                                                // break;
                                                                Expression.Break(breakInner)
                                                            // }
                                                            )
                                                        )
                                                    // }
                                                    ),
                                                    // d++;
                                                    Expression.PostIncrementAssign(d)
                                                // }
                                                ),
                                                breakInner
                                            )
                                        ),
                                        // if
                                        Expression.IfThen(
                                            // (!found)
                                            Expression.Not(found),
                                            //    res.Add(n);
                                            Expression.Call(
                                                res,
                                                typeof(List<int>).GetMethod("Add"),
                                                n
                                            )
                                        )
                                    ),
                                    // n++;
                                    Expression.PostIncrementAssign(n)
                                // }
                                ),
                                breakOuter
                            )
                        ),
                        res
                    ),
                    to
                // }
                );

            var getPrime2 = (Expression<Func<int, List<int>>>)ser.Deserialize(ser.Serialize(getPrimes));

            var xs = getPrime2.Compile()(100);
            var ys = getPrimes.Compile()(100);

            Assert.IsTrue(xs.SequenceEqual(ys));
        }

        #endregion

        #region Regression tests

        [TestMethod]
        public void Regression_Task()
        {
            Expression<Func<Task>> f = () => new Task<int>(() => 42).ContinueWith(t => Console.WriteLine(t.Result));

            var e = Roundtrip(f) as Expression<Func<Task>>;
            var u = e.Compile()();

            // Just the mere fact of getting here is what's of interest to us.
            Assert.IsTrue(true);
        }

        #endregion

        #region Helpers

        private static Expression Roundtrip(Expression e)
        {
            var ser = ExpressionJsonSerializer.Instance;
            var s = ser.Serialize(e);
            return ser.Deserialize(s);
        }

        private sealed class ExpressionEquality : IEqualityComparer<Expression>, IEqualityComparer<MemberBinding>, IEqualityComparer<ElementInit>
        {
            private readonly Stack<Dictionary<ParameterExpression, ParameterExpression>> _environment;

            public ExpressionEquality()
            {
                _environment = new Stack<Dictionary<ParameterExpression, ParameterExpression>>();
            }

            public bool Equals(Expression a, Expression b)
            {
                if (a == null && b == null)
                    return true;
                if (a == null || b == null)
                    return false;

                if (a.NodeType != b.NodeType)
                    return false;

                switch (a.NodeType)
                {
                    case ExpressionType.Lambda:
                        {
                            var la = (LambdaExpression)a;
                            var lb = (LambdaExpression)b;

                            if (!la.Parameters.Select(p => p.Type).SequenceEqual(lb.Parameters.Select(p => p.Type)))
                                return false;

                            try
                            {
                                _environment.Push(la.Parameters.Zip(lb.Parameters, Tuple.Create).ToDictionary(t => t.Item1, t => t.Item2));
                                return Equals(la.Body, lb.Body);
                            }
                            finally
                            {
                                _environment.Pop();
                            }
                        }
                    case ExpressionType.Call:
                        {
                            var ca = (MethodCallExpression)a;
                            var cb = (MethodCallExpression)b;
                            return ca.Method == cb.Method && Equals(ca.Object, cb.Object) && ca.Arguments.SequenceEqual(cb.Arguments, this);
                        }
                    case ExpressionType.Parameter:
                        {
                            var pa = (ParameterExpression)a;
                            var pb = (ParameterExpression)b;

                            if (pa.Type != pb.Type)
                                return false;

                            foreach (var frame in _environment)
                            {
                                if (frame.TryGetValue(pa, out var res))
                                    return object.ReferenceEquals(pb, res);
                            }

                            return true;
                        }
                    case ExpressionType.Quote:
                    //case ExpressionType.OnesComplement:
                    case ExpressionType.Not:
                    case ExpressionType.UnaryPlus:
                    case ExpressionType.Negate:
                    case ExpressionType.NegateChecked:
                        {
                            var ua = (UnaryExpression)a;
                            var ub = (UnaryExpression)b;
                            return ua.Method == ub.Method && Equals(ua.Operand, ub.Operand);
                        }
                    case ExpressionType.Add:
                    case ExpressionType.AddChecked:
                    case ExpressionType.Subtract:
                    case ExpressionType.SubtractChecked:
                    case ExpressionType.Multiply:
                    case ExpressionType.MultiplyChecked:
                    case ExpressionType.Divide:
                    case ExpressionType.Modulo:
                    case ExpressionType.And:
                    case ExpressionType.AndAlso:
                    case ExpressionType.Or:
                    case ExpressionType.OrElse:
                        {
                            var ba = (BinaryExpression)a;
                            var bb = (BinaryExpression)b;
                            return ba.Method == bb.Method && Equals(ba.Left, bb.Left) && Equals(ba.Right, bb.Right);
                        }
                    case ExpressionType.New:
                        {
                            var na = (NewExpression)a;
                            var nb = (NewExpression)b;
                            return na.Constructor == nb.Constructor && na.Arguments.SequenceEqual(nb.Arguments, this) &&
                                (
                                    (na.Members == null && nb.Members == null)
                                ||
                                    (
                                        na.Members != null && nb.Members != null
                                    &&
                                        na.Members.SequenceEqual(nb.Members)
                                    )
                                );
                        }
                    case ExpressionType.MemberInit:
                        {
                            var ma = (MemberInitExpression)a;
                            var mb = (MemberInitExpression)b;
                            return Equals(ma.NewExpression, mb.NewExpression) && ma.Bindings.SequenceEqual(mb.Bindings, this);
                        }
                    case ExpressionType.MemberAccess:
                        {
                            var ma = (MemberExpression)a;
                            var mb = (MemberExpression)b;
                            return ma.Member == mb.Member && Equals(ma.Expression, mb.Expression);
                        }
                    case ExpressionType.Constant:
                        {
                            var ca = (ConstantExpression)a;
                            var cb = (ConstantExpression)b;
                            return object.Equals(ca.Value, cb.Value);
                        }
                }

                throw new InvalidOperationException("Unknown tree node type: " + a.NodeType);
            }

            public int GetHashCode(Expression obj)
            {
                throw new NotImplementedException();
            }

            public bool Equals(MemberBinding x, MemberBinding y)
            {
                if (x.BindingType != y.BindingType)
                    return false;

                switch (x.BindingType)
                {
                    case MemberBindingType.MemberBinding:
                        {
                            var mx = (MemberMemberBinding)x;
                            var my = (MemberMemberBinding)y;
                            return mx.Member == my.Member && mx.Bindings.SequenceEqual(my.Bindings, this);
                        }
                    case MemberBindingType.ListBinding:
                        {
                            var lx = (MemberListBinding)x;
                            var ly = (MemberListBinding)y;
                            return lx.Member == ly.Member && lx.Initializers.SequenceEqual(ly.Initializers, this);
                        }
                }

                throw new InvalidOperationException("Unknown binding type: " + x.BindingType);
            }

            public int GetHashCode(MemberBinding obj)
            {
                throw new NotImplementedException();
            }

            public bool Equals(ElementInit x, ElementInit y)
            {
                if (x.AddMethod != y.AddMethod)
                    return false;

                return x.Arguments.SequenceEqual(y.Arguments, this);
            }

            public int GetHashCode(ElementInit obj)
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region Boneyard

        //[TestMethod]
        //public void Tuples()
        //{
        //    var t = Tuple.Create("Bart", 29);
        //    var c = Expression.Constant(t);

        //    var e = (Tuple<string, int>)((ConstantExpression)Roundtrip(c)).Value;

        //    Assert.AreEqual(t, e);
        //}

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
