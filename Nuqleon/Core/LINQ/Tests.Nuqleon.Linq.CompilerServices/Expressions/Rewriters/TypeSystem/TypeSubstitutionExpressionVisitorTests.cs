// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
//

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.CompilerServices;
using System.Linq.Expressions;
using System.Reflection;

namespace Tests.System.Linq.CompilerServices
{
    [TestClass]
    public class TypeSubstitutionExpressionVisitorTests
    {
        [TestMethod]
        public void TypeSubstitutionExpressionVisitor_ArgumentChecking()
        {
            AssertEx.ThrowsException<ArgumentNullException>(() => new TypeSubstitutionExpressionVisitor(default(TypeSubstitutor)), ex => Assert.AreEqual("typeSubstitutor", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => new TypeSubstitutionExpressionVisitor(default(IDictionary<Type, Type>)), ex => Assert.AreEqual("typeMap", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => GetVisitorSimple().Apply(expression: null), ex => Assert.AreEqual("expression", ex.ParamName));

            new MySubst().Do();
        }

        private sealed class MySubst : TypeSubstitutionExpressionVisitor
        {
            public MySubst()
                : base(new Dictionary<Type, Type>())
            {
            }

            public void Do()
            {
                var t = typeof(int);
                var ts = new[] { t };

                var ctor = typeof(MySubst).GetConstructor(Type.EmptyTypes);

                AssertEx.ThrowsException<ArgumentNullException>(() => base.ResolveConstructor(originalConstructor: null, t, ts), ex => Assert.AreEqual("originalConstructor", ex.ParamName));
                AssertEx.ThrowsException<ArgumentNullException>(() => base.ResolveConstructor(ctor, declaringType: null, ts), ex => Assert.AreEqual("declaringType", ex.ParamName));
                AssertEx.ThrowsException<ArgumentNullException>(() => base.ResolveConstructor(ctor, t, parameters: null), ex => Assert.AreEqual("parameters", ex.ParamName));

                var prop = typeof(string).GetProperty("Length");

                AssertEx.ThrowsException<ArgumentNullException>(() => base.ResolveProperty(originalProperty: null, t, t, ts), ex => Assert.AreEqual("originalProperty", ex.ParamName));
                AssertEx.ThrowsException<ArgumentNullException>(() => base.ResolveProperty(prop, declaringType: null, t, ts), ex => Assert.AreEqual("declaringType", ex.ParamName));
                AssertEx.ThrowsException<ArgumentNullException>(() => base.ResolveProperty(prop, t, propertyType: null, ts), ex => Assert.AreEqual("propertyType", ex.ParamName));
                AssertEx.ThrowsException<ArgumentNullException>(() => base.ResolveProperty(prop, t, t, indexerParameters: null), ex => Assert.AreEqual("indexerParameters", ex.ParamName));

                var fld = typeof(string).GetField("Empty");

                AssertEx.ThrowsException<ArgumentNullException>(() => base.ResolveField(originalField: null, t, t), ex => Assert.AreEqual("originalField", ex.ParamName));
                AssertEx.ThrowsException<ArgumentNullException>(() => base.ResolveField(fld, declaringType: null, t), ex => Assert.AreEqual("declaringType", ex.ParamName));
                AssertEx.ThrowsException<ArgumentNullException>(() => base.ResolveField(fld, t, fieldType: null), ex => Assert.AreEqual("fieldType", ex.ParamName));

                var mtd = typeof(string).GetMethod("ToUpper", Type.EmptyTypes);

                AssertEx.ThrowsException<ArgumentNullException>(() => base.ResolveMethod(originalMethod: null, t, ts, ts, t), ex => Assert.AreEqual("originalMethod", ex.ParamName));
                AssertEx.ThrowsException<ArgumentNullException>(() => base.ResolveMethod(mtd, declaringType: null, ts, ts, t), ex => Assert.AreEqual("declaringType", ex.ParamName));
                AssertEx.ThrowsException<ArgumentNullException>(() => base.ResolveMethod(mtd, t, genericArguments: null, ts, t), ex => Assert.AreEqual("genericArguments", ex.ParamName));
                AssertEx.ThrowsException<ArgumentNullException>(() => base.ResolveMethod(mtd, t, ts, parameters: null, t), ex => Assert.AreEqual("parameters", ex.ParamName));
                AssertEx.ThrowsException<ArgumentNullException>(() => base.ResolveMethod(mtd, t, ts, ts, returnType: null), ex => Assert.AreEqual("returnType", ex.ParamName));
            }
        }

        #region Binary

        [TestMethod]
        public void TypeSubstitutionExpressionVisitor_Binary_Simple()
        {
            var subst = GetVisitorSimple();

            var expA = Expression.Add(Expression.Constant(1), Expression.Constant(2));
            var expB = Expression.Add(Expression.Constant(1.0), Expression.Constant(2.0));

            var res1 = subst.Apply(expA);
            var res2 = subst.Apply(expB);

            AssertEqual(res1, Expression.Add(Expression.Constant(1L), Expression.Constant(2L)));
            Assert.AreSame(expB, res2);
        }

        [TestMethod]
        public void TypeSubstitutionExpressionVisitor_Binary_CustomOperator()
        {
            var subst = GetVisitorSimple();

            var expA = Expression.Add(Expression.Parameter(typeof(Bar)), Expression.Constant(2));
            var expB = Expression.Add(Expression.Parameter(typeof(Bar)), Expression.Constant("foo"));

            var res1 = subst.Apply(expA);
            var res2 = subst.Apply(expB);

            AssertEqual(res1, Expression.Add(Expression.Parameter(typeof(Foo)), Expression.Constant(2L)));
            AssertEqual(res2, Expression.Add(Expression.Parameter(typeof(Foo)), Expression.Constant("foo")));
        }

        [TestMethod]
        public void TypeSubstitutionExpressionVisitor_Binary_Coalesce()
        {
            var subst = GetVisitorSimple();

            var p = Expression.Parameter(typeof(Bar));
            var i = Expression.Lambda(p, p);
            var exp = Expression.Coalesce(Expression.Parameter(typeof(Bar)), Expression.Constant(value: null, typeof(Bar)), i);

            var res = subst.Apply(exp);

            var q = Expression.Parameter(typeof(Foo));
            var j = Expression.Lambda(q, q);
            AssertEqual(res, Expression.Coalesce(Expression.Parameter(typeof(Foo)), Expression.Constant(value: null, typeof(Foo)), j));
        }

        #endregion

        #region Block

        [TestMethod]
        public void TypeSubstitutionExpressionVisitor_Block()
        {
            var subst = GetVisitorSimple();

            var p = Expression.Parameter(typeof(int));
            var q = Expression.Parameter(typeof(long));

            var expA = Expression.Block(Expression.Constant(1));
            var expB = Expression.Block(Expression.Constant(1.0));
            var expC = Expression.Block(new[] { p }, p);

            var res1 = subst.Apply(expA);
            var res2 = subst.Apply(expB);
            var res3 = subst.Apply(expC);

            AssertEqual(res1, Expression.Block(Expression.Constant(1L)));
            Assert.AreSame(expB, res2);
            AssertEqual(res3, Expression.Block(new[] { q }, q));
        }

        #endregion

        #region Condition

        [TestMethod]
        public void TypeSubstitutionExpressionVisitor_Condition_Simple()
        {
            var subst = GetVisitorSimple();

            var expA = Expression.Condition(Expression.Constant(true), Expression.Constant(2), Expression.Constant(3));
            var expB = Expression.Condition(Expression.Constant(true), Expression.Constant(2.0), Expression.Constant(3.0));

            var res1 = subst.Apply(expA);
            var res2 = subst.Apply(expB);

            AssertEqual(res1, Expression.Condition(Expression.Constant(true), Expression.Constant(2L), Expression.Constant(3L)));
            Assert.AreSame(expB, res2);
        }

        #endregion

        #region Constant

        [TestMethod]
        public void TypeSubstitutionExpressionVisitor_Constant()
        {
            var subst = GetVisitorSimple();
            var banana = new Banana();

            var expA = Expression.Constant(42, typeof(int));
            var expB = Expression.Constant("foo");
            var expC = Expression.Constant(value: null, typeof(Bar));
            var expD = Expression.Constant(value: null, typeof(int?));
            var expE = Expression.Constant(banana, typeof(Banana));

            var res1 = subst.Apply(expA);
            var res2 = subst.Apply(expB);
            var res3 = subst.Apply(expC);
            var res4 = subst.Apply(expD);
            var res5 = subst.Apply(expE);

            AssertEqual(res1, Expression.Constant(42L, typeof(long)));

            Assert.AreSame(expB, res2);

            AssertEqual(res3, Expression.Constant(value: null, typeof(Foo)));
            AssertEqual(res4, Expression.Constant(value: null, typeof(long?)));
            AssertEqual(res5, Expression.Constant(banana, typeof(Fruit)));
        }

        public class Fruit
        {
            public ConsoleColor Color { get; set; }
        }

        public class Banana : Fruit
        {
            public Banana()
            {
                Color = ConsoleColor.Yellow;
            }
        }

        #endregion

        #region Default

        [TestMethod]
        public void TypeSubstitutionExpressionVisitor_Default()
        {
            var subst = GetVisitorSimple();

            var expA = Expression.Default(typeof(int));
            var expB = Expression.Default(typeof(string));

            var res1 = subst.Apply(expA);
            var res2 = subst.Apply(expB);

            AssertEqual(res1, Expression.Default(typeof(long)));

            Assert.AreSame(expB, res2);
        }

        #endregion

        #region Dynamic

        [TestMethod]
        public void TypeSubstitutionExpressionVisitor_Dynamic()
        {
            var subst = GetVisitorSimple();

            var add = Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(
                Microsoft.CSharp.RuntimeBinder.CSharpBinderFlags.None,
                ExpressionType.Add,
                typeof(ExpressionEqualityComparerTests),
                new[]
                {
                    Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfo.Create(Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfoFlags.None, name: null),
                    Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfo.Create(Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfoFlags.None, name: null)
                }
            );

            var expA = Expression.Dynamic(
                add,
                typeof(object),
                Expression.Constant(1),
                Expression.Constant(2)
            );

            var expB = Expression.Dynamic(
                add,
                typeof(object),
                Expression.Constant(1.0),
                Expression.Constant(2.0)
            );

            var expC = Expression.Dynamic(
                add,
                typeof(int),
                Expression.Constant(1),
                Expression.Constant(2)
            );

            var res1 = subst.Apply(expA);
            var res2 = subst.Apply(expB);
            var res3 = subst.Apply(expC);

            var expD = Expression.Dynamic(
                add,
                typeof(object),
                Expression.Constant(1L),
                Expression.Constant(2L)
            );

            var expE = Expression.Dynamic(
                add,
                typeof(long),
                Expression.Constant(1L),
                Expression.Constant(2L)
            );

            AssertEqual(expD, res1);
            AssertEqual(expE, res3);

            Assert.AreSame(expB, res2);
        }

        #endregion

        #region Goto

        [TestMethod]
        public void TypeSubstitutionExpressionVisitor_Goto()
        {
            var subst = GetVisitorSimple();

            var tgtA = Expression.Label(typeof(int), "foo");
            var tgtB = Expression.Label(typeof(double), "foo");

            var expA = Expression.Return(tgtA, Expression.Constant(1));
            var expB = Expression.Return(tgtB, Expression.Constant(1.0));

            var res1 = subst.Apply(expA);
            var res2 = subst.Apply(expB);

            var tgt1 = Expression.Label(typeof(long), "foo");
            AssertEqual(res1, Expression.Return(tgt1, Expression.Constant(1L)));
            Assert.AreSame(expB, res2);
        }

        #endregion

        #region Index

        [TestMethod]
        public void TypeSubstitutionExpressionVisitor_Index()
        {
            var subst = GetVisitorSimple();

            var expA = Expression.MakeIndex(Expression.Parameter(typeof(Dictionary<int, string>)), typeof(Dictionary<int, string>).GetProperty("Item"), new[] { Expression.Constant(1) });
            var expB = Expression.MakeIndex(Expression.Parameter(typeof(Dictionary<double, string>)), typeof(Dictionary<double, string>).GetProperty("Item"), new[] { Expression.Constant(1.0) });

            var res1 = subst.Apply(expA);
            var res2 = subst.Apply(expB);

            AssertEqual(res1, Expression.MakeIndex(Expression.Parameter(typeof(Dictionary<long, string>)), typeof(Dictionary<long, string>).GetProperty("Item"), new[] { Expression.Constant(1L) }));
            Assert.AreSame(expB, res2);
        }

        [TestMethod]
        public void TypeSubstitutionExpressionVisitor_Index_PropertyMissing()
        {
            var subst = GetVisitorSimple();

            var exp = Expression.MakeIndex(Expression.Parameter(typeof(Bar)), typeof(Bar).GetProperty("Item"), new[] { Expression.Constant(1) });

            Assert.ThrowsException<InvalidOperationException>(() => subst.Apply(exp));
        }

        #endregion

        #region Invoke

        [TestMethod]
        public void TypeSubstitutionExpressionVisitor_Invoke()
        {
            var subst = GetVisitorSimple();

            var expA = Expression.Invoke(Expression.Parameter(typeof(Func<int, int>)), Expression.Constant(42));
            var expB = Expression.Invoke(Expression.Parameter(typeof(Func<string, string>)), Expression.Constant("foo"));

            var res1 = subst.Apply(expA);
            var res2 = subst.Apply(expB);

            AssertEqual(res1, Expression.Invoke(Expression.Parameter(typeof(Func<long, long>)), Expression.Constant(42L)));

            Assert.AreSame(expB, res2);
        }

        #endregion

        #region Label

        [TestMethod]
        public void TypeSubstitutionExpressionVisitor_Label()
        {
            var subst = GetVisitorSimple();

            var tgtA = Expression.Label(typeof(int), "foo");
            var tgtB = Expression.Label(typeof(double), "foo");

            var expA = Expression.Label(tgtA, Expression.Constant(1));
            var expB = Expression.Label(tgtB, Expression.Constant(1.0));

            var res1 = subst.Apply(expA);
            var res2 = subst.Apply(expB);

            var tgt1 = Expression.Label(typeof(long), "foo");
            AssertEqual(res1, Expression.Label(tgt1, Expression.Constant(1L)));
            Assert.AreSame(expB, res2);
        }

        #endregion

        #region Lambda

        [TestMethod]
        public void TypeSubstitutionExpressionVisitor_Lambda()
        {
            var subst = GetVisitorSimple();

            var expA = (Expression<Func<int, int>>)(x => x);
            var expB = (Expression<Func<double, double>>)(x => x);
            var expC = (Expression<Func<int, Func<string, int>>>)(x => s => x);

            var res1 = subst.Apply(expA);
            var res2 = subst.Apply(expB);
            var res3 = subst.Apply(expC);

            AssertEqual(res1, (Expression<Func<long, long>>)(x => x));
            Assert.AreSame(expB, res2);
            AssertEqual(res3, (Expression<Func<long, Func<string, long>>>)(x => s => x));
        }

        #endregion

        #region ListInit

        [TestMethod]
        public void TypeSubstitutionExpressionVisitor_ListInit()
        {
            var subst = GetVisitorSimple();

            var expA = ((Expression<Func<List<int>>>)(() => new List<int> { 2, 3, 5 })).Body;
            var expB = ((Expression<Func<List<double>>>)(() => new List<double> { 2.0, 3.0, 5.0 })).Body;

            var res1 = subst.Apply(expA);
            var res2 = subst.Apply(expB);

            AssertEqual(res1, ((Expression<Func<List<long>>>)(() => new List<long> { 2L, 3L, 5L })).Body);
            Assert.AreSame(expB, res2);
        }

        #endregion

        #region Loop

        [TestMethod]
        public void TypeSubstitutionExpressionVisitor_Loop()
        {
            var subst = GetVisitorSimple();

            var barQux = (PropertyInfo)ReflectionHelpers.InfoOf((Bar b) => b.Qux);
            var fooQux = (PropertyInfo)ReflectionHelpers.InfoOf((Foo f) => f.Qux);

            var brkA = Expression.Label("brk");
            var cntA = Expression.Label("cnt");

            var brkB = Expression.Label("brk");
            var cntB = Expression.Label("cnt");

            var expA = Expression.Loop(Expression.Constant(1), brkA, cntA);
            var expB = Expression.Loop(Expression.Constant(1.0), brkB, cntB);

            var res1 = subst.Apply(expA);
            var res2 = subst.Apply(expB);

            var brk1 = Expression.Label("brk");
            var cnt1 = Expression.Label("cnt");

            AssertEqual(res1, Expression.Loop(Expression.Constant(1L), brk1, cnt1));
            Assert.AreSame(expB, res2);
        }

        #endregion

        #region Member

        [TestMethod]
        public void TypeSubstitutionExpressionVisitor_Member_Instance()
        {
            var subst = GetVisitorSimple();

            var barQux = (PropertyInfo)ReflectionHelpers.InfoOf((Bar b) => b.Qux);
            var fooQux = (PropertyInfo)ReflectionHelpers.InfoOf((Foo f) => f.Qux);

            var expA = Expression.Property(Expression.Parameter(typeof(Bar)), barQux);
            var expB = Expression.Property(Expression.Parameter(typeof(AppDomain)), "BaseDirectory");

            var res1 = subst.Apply(expA);
            var res2 = subst.Apply(expB);

            AssertEqual(res1, Expression.Property(Expression.Parameter(typeof(Foo)), fooQux));
            Assert.AreSame(expB, res2);
        }

        [TestMethod]
        public void TypeSubstitutionExpressionVisitor_Member_Instance_ResultTypeIncompatible()
        {
            var subst = GetVisitorSimple();

            var exp = Expression.Property(Expression.Parameter(typeof(string)), "Length");

            Assert.ThrowsException<InvalidOperationException>(() => subst.Apply(exp));
        }

        [TestMethod]
        public void TypeSubstitutionExpressionVisitor_Member_Static()
        {
            var subst = GetVisitorSimple();

            var barBaz = (FieldInfo)ReflectionHelpers.InfoOf(() => Bar.Baz);
            var fooBaz = (FieldInfo)ReflectionHelpers.InfoOf(() => Foo.Baz);

            var expA = Expression.Field(expression: null, barBaz);
            var expB = Expression.Property(expression: null, (PropertyInfo)ReflectionHelpers.InfoOf(() => DateTime.Now));
            var expC = Expression.Field(expression: null, (FieldInfo)ReflectionHelpers.InfoOf(() => DateTime.MaxValue));

            var res1 = subst.Apply(expA);
            var res2 = subst.Apply(expB);
            var res3 = subst.Apply(expC);

            AssertEqual(res1, Expression.Field(expression: null, fooBaz));
            Assert.AreSame(expB, res2);
            Assert.AreSame(expC, res3);
        }

        [TestMethod]
        public void TypeSubstitutionExpressionVisitor_Member_FieldMissing()
        {
            var subst = GetVisitorSimple();

            var exp1 = (Expression<Func<Baz, int>>)((Baz b) => b.x);
            var exp2 = (Expression<Func<Bar, int>>)((Bar b) => b.Wrong2);
            var exp3 = (Expression<Func<Bar, int>>)((Bar b) => b.Wrong3);

            Assert.ThrowsException<InvalidOperationException>(() => subst.Apply(exp1));
            Assert.ThrowsException<InvalidOperationException>(() => subst.Apply(exp2));
            Assert.ThrowsException<InvalidOperationException>(() => subst.Apply(exp3));
        }

        [TestMethod]
        public void TypeSubstitutionExpressionVisitor_Member_PropertyMissing()
        {
            var subst = GetVisitorSimple();

            var exp = (Expression<Func<Baz, int>>)((Baz b) => b.Y);

            Assert.ThrowsException<InvalidOperationException>(() => subst.Apply(exp));
        }

        #endregion

        #region MemberInit

        [TestMethod]
        public void TypeSubstitutionExpressionVisitor_MemberInit()
        {
            var subst = GetVisitorSimple();

            var expA = ((Expression<Func<Bar>>)(() => new Bar(1) { Qux = 2, Xs = { 3, 4 }, Joey = { Qux = 5 }, Stubborn = "foo", Ys = { "qux" } })).Body;
            var expB = ((Expression<Func<Foo>>)(() => new Foo(1) { Qux = 2, Xs = { 3, 4 }, Joey = { Qux = 5 }, Stubborn = "foo", Ys = { "qux" } })).Body;

            var res1 = subst.Apply(expA);
            var res2 = subst.Apply(expB);

            AssertEqual(res1, ((Expression<Func<Foo>>)(() => new Foo(1L) { Qux = 2L, Xs = { 3L, 4L }, Joey = { Qux = 5L }, Stubborn = "foo", Ys = { "qux" } })).Body);
            Assert.AreSame(expB, res2);
        }

        #endregion

        #region MethodCall

        [TestMethod]
        public void TypeSubstitutionExpressionVisitor_MethodCall_Static()
        {
            var subst = GetVisitorSimple();

            var cwInt32 = (MethodInfo)ReflectionHelpers.InfoOf(() => Console.WriteLine(42));
            var cwInt64 = (MethodInfo)ReflectionHelpers.InfoOf(() => Console.WriteLine(1L));
            var cwStrng = (MethodInfo)ReflectionHelpers.InfoOf(() => Console.WriteLine(""));

            var expA = Expression.Call(cwInt32, Expression.Constant(12345));
            var expB = Expression.Call(cwStrng, Expression.Constant("foo"));

            var res1 = subst.Apply(expA);
            var res2 = subst.Apply(expB);

            AssertEqual(res1, Expression.Call(cwInt64, Expression.Constant(12345L)));

            Assert.AreSame(expB, res2);
        }

        [TestMethod]
        public void TypeSubstitutionExpressionVisitor_MethodCall_GenericType()
        {
            var subst = GetVisitorSimple();

            var lstInt32Add = (MethodInfo)ReflectionHelpers.InfoOf((List<int> l) => l.Add(42));
            var lstInt64Add = (MethodInfo)ReflectionHelpers.InfoOf((List<long> l) => l.Add(1L));
            var lstStrngAdd = (MethodInfo)ReflectionHelpers.InfoOf((List<string> l) => l.Add(""));

            var expA = Expression.Call(Expression.Parameter(typeof(List<int>)), lstInt32Add, Expression.Constant(12345));
            var expB = Expression.Call(Expression.Parameter(typeof(List<string>)), lstStrngAdd, Expression.Constant("foo"));

            var res1 = subst.Apply(expA);
            var res2 = subst.Apply(expB);

            AssertEqual(res1, Expression.Call(Expression.Parameter(typeof(List<long>)), lstInt64Add, Expression.Constant(12345L)));

            Assert.AreSame(expB, res2);
        }

        [TestMethod]
        public void TypeSubstitutionExpressionVisitor_MethodCall_GenericMethod()
        {
            var subst = GetVisitorSimple();

            var aciInt32 = (MethodInfo)ReflectionHelpers.InfoOf(() => Activator.CreateInstance<int>());
            var aciInt64 = (MethodInfo)ReflectionHelpers.InfoOf(() => Activator.CreateInstance<long>());
            var aciStrng = (MethodInfo)ReflectionHelpers.InfoOf(() => Activator.CreateInstance<string>());

            var expA = Expression.Call(aciInt32);
            var expB = Expression.Call(aciStrng);

            var res1 = subst.Apply(expA);
            var res2 = subst.Apply(expB);

            AssertEqual(res1, Expression.Call(aciInt64));

            Assert.AreSame(expB, res2);
        }

        [TestMethod]
        public void TypeSubstitutionExpressionVisitor_Method_MethodMissing()
        {
            var subst = GetVisitorSimple();

            var exp1 = (Expression<Func<Baz, int>>)((Baz b) => b.Z(5));
            var exp2 = (Expression<Func<Baz, int>>)((Baz b) => b.A<bool>(5));

            Assert.ThrowsException<InvalidOperationException>(() => subst.Apply(exp1));
            Assert.ThrowsException<InvalidOperationException>(() => subst.Apply(exp2));
        }

        #endregion

        #region New

        [TestMethod]
        public void TypeSubstitutionExpressionVisitor_New_Simple()
        {
            var subst = GetVisitorSimple();

            var barCtor = (ConstructorInfo)ReflectionHelpers.InfoOf(() => new Bar(1));
            var fooCtor = (ConstructorInfo)ReflectionHelpers.InfoOf(() => new Foo(1L));

            var expA = Expression.New(barCtor, Expression.Constant(42));
            var expB = Expression.New((ConstructorInfo)ReflectionHelpers.InfoOf(() => new TimeSpan(123L)), Expression.Constant(123L));

            var res1 = subst.Apply(expA);
            var res2 = subst.Apply(expB);

            AssertEqual(res1, Expression.New(fooCtor, Expression.Constant(42L)));
            Assert.AreSame(expB, res2);
        }

        [TestMethod]
        public void TypeSubstitutionExpressionVisitor_New_Generic()
        {
            var subst = GetVisitorSimple();

            var listInt32Ctor = (ConstructorInfo)ReflectionHelpers.InfoOf(() => new List<int>());
            var listInt64Ctor = (ConstructorInfo)ReflectionHelpers.InfoOf(() => new List<long>());

            var exp = Expression.New(listInt32Ctor);

            var res = subst.Apply(exp);

            AssertEqual(res, Expression.New(listInt64Ctor));
        }

#pragma warning disable IDE0050 // Convert to tuple. (Test for anonymous types.)

        [TestMethod]
        public void TypeSubstitutionExpressionVisitor_New_Anonymous()
        {
            var subst = GetVisitorSimple();

            var expA = ((Expression<Func<object>>)(() => new { x = 42, y = "foo" })).Body;
            var expB = ((Expression<Func<object>>)(() => new { x = 42L, y = "foo" })).Body;

            var res = subst.Apply(expA);

            AssertEqual(res, expB);
        }

#pragma warning restore IDE0050

        [TestMethod]
        public void TypeSubstitutionExpressionVisitor_New_ConstructorMissing()
        {
            var subst = GetVisitorSimple();

            var exp = (Expression<Func<Baz>>)(() => new Baz(5));

            Assert.ThrowsException<InvalidOperationException>(() => subst.Apply(exp));
        }

        [TestMethod]
        public void TypeSubstitutionExpressionVisitor_New_ValueType_DefaultConstructor()
        {
            var subst = GetVisitorSimple();
            var exp = Expression.Lambda(Expression.New(typeof(int)));
            var res = subst.Apply(exp);
            AssertEqual(Expression.Lambda(Expression.New(typeof(long))), res);
        }

        #endregion

        #region NewArray

        [TestMethod]
        public void TypeSubstitutionExpressionVisitor_NewArrayBounds_Simple()
        {
            var subst = new TypeSubstitutionExpressionVisitor(new Dictionary<Type, Type> { { typeof(string), typeof(bool) } });

            var expA = Expression.NewArrayBounds(typeof(string), Expression.Constant(1));
            var expB = Expression.NewArrayBounds(typeof(int), Expression.Constant(1));

            var res1 = subst.Apply(expA);
            var res2 = subst.Apply(expB);

            AssertEqual(res1, Expression.NewArrayBounds(typeof(bool), Expression.Constant(1)));
            Assert.AreSame(expB, res2);
        }

        [TestMethod]
        public void TypeSubstitutionExpressionVisitor_NewArrayInit_Simple()
        {
            var subst = new TypeSubstitutionExpressionVisitor(new Dictionary<Type, Type> { { typeof(string), typeof(bool) } });

            var expA = Expression.NewArrayInit(typeof(string), Expression.Parameter(typeof(string), "e1"), Expression.Parameter(typeof(string), "e2"));
            var expB = Expression.NewArrayInit(typeof(int), Expression.Parameter(typeof(int), "e1"), Expression.Parameter(typeof(int), "e2"));

            var res1 = subst.Apply(expA);
            var res2 = subst.Apply(expB);

            AssertEqual(res1, Expression.NewArrayInit(typeof(bool), Expression.Parameter(typeof(bool), "e1"), Expression.Parameter(typeof(bool), "e2")));
            Assert.AreSame(expB, res2);
        }

        #endregion

        #region Parameter

        [TestMethod]
        public void TypeSubstitutionExpressionVisitor_Parameter_Global1()
        {
            var subst = GetVisitorSimple();

            var parA = Expression.Parameter(typeof(int), "a");
            var parB = Expression.Parameter(typeof(string), "b");

            var res1 = subst.Apply(parA);
            //var res2 = subst.Apply(parA); // TODO: Sharing of globals across substitutions?
            var res3 = subst.Apply(parB);
            //var res4 = subst.Apply(parB);

            AssertEqual(res1, Expression.Parameter(typeof(long), "a"));
            //Assert.AreSame(res2, res1);

            Assert.AreSame(parB, res3);
            //Assert.AreSame(res4, res3);
        }

        [TestMethod]
        public void TypeSubstitutionExpressionVisitor_Parameter_Global2()
        {
            var subst = GetVisitorSimple();

            var parOld = Expression.Parameter(typeof(int), "a");
            var expOld = Expression.Add(parOld, parOld);

            var res = subst.Apply(expOld);

            var parNew = Expression.Parameter(typeof(long), "a"); ;
            var expNew = Expression.Add(parNew, parNew);

            AssertEqual(expNew, res);
        }

        #endregion

        #region RuntimeVariables

        [TestMethod]
        public void TypeSubstitutionExpressionVisitor_RuntimeVariables()
        {
            var subst = GetVisitorSimple();

            var expA = Expression.RuntimeVariables(Expression.Parameter(typeof(int), "a"), Expression.Parameter(typeof(int), "b"));
            var expB = Expression.RuntimeVariables(Expression.Parameter(typeof(double), "a"), Expression.Parameter(typeof(double), "b"));

            var res1 = subst.Apply(expA);
            var res2 = subst.Apply(expB);

            AssertEqual(res1, Expression.RuntimeVariables(Expression.Parameter(typeof(long), "a"), Expression.Parameter(typeof(long), "b")));
            Assert.AreSame(expB, res2);
        }

        #endregion

        #region Switch

        [TestMethod]
        public void TypeSubstitutionExpressionVisitor_Switch()
        {
            var subst = GetVisitorSimple();

            var expA = Expression.Switch(Expression.Constant(1), Expression.Constant("foo"), Expression.SwitchCase(Expression.Constant("bar"), Expression.Constant(2), Expression.Constant(3)));
            var expB = Expression.Switch(Expression.Constant(1.0), Expression.Constant("foo"), Expression.SwitchCase(Expression.Constant("bar"), Expression.Constant(2.0), Expression.Constant(3.0)));
            var expC = Expression.Switch(Expression.Constant("foo"), Expression.Constant(1), Expression.SwitchCase(Expression.Constant(2), Expression.Constant("bar"), Expression.Constant("qux")));

            var res1 = subst.Apply(expA);
            var res2 = subst.Apply(expB);
            var res3 = subst.Apply(expC);

            AssertEqual(res1, Expression.Switch(Expression.Constant(1L), Expression.Constant("foo"), Expression.SwitchCase(Expression.Constant("bar"), Expression.Constant(2L), Expression.Constant(3L))));
            Assert.AreSame(expB, res2);
            AssertEqual(res3, Expression.Switch(Expression.Constant("foo"), Expression.Constant(1L), Expression.SwitchCase(Expression.Constant(2L), Expression.Constant("bar"), Expression.Constant("qux"))));
        }

        #endregion

        #region Try

        [TestMethod]
        public void TypeSubstitutionExpressionVisitor_Try()
        {
            var subst = GetVisitorSimple();

            var expA = Expression.TryCatch(Expression.Constant(1), Expression.Catch(Expression.Parameter(typeof(Exception)), Expression.Constant(2)));
            var expB = Expression.TryCatch(Expression.Constant(1.0), Expression.Catch(Expression.Parameter(typeof(Exception)), Expression.Constant(2.0)));
            var expC = Expression.TryFinally(Expression.Constant(1), Expression.Constant(2));
            var expD = Expression.TryFault(Expression.Constant(1), Expression.Constant(2));

            var res1 = subst.Apply(expA);
            var res2 = subst.Apply(expB);
            var res3 = subst.Apply(expC);
            var res4 = subst.Apply(expD);

            AssertEqual(res1, Expression.TryCatch(Expression.Constant(1L), Expression.Catch(Expression.Parameter(typeof(Exception)), Expression.Constant(2L))));
            Assert.AreSame(expB, res2);
            AssertEqual(res3, Expression.TryFinally(Expression.Constant(1L), Expression.Constant(2L)));
            AssertEqual(res4, Expression.TryFault(Expression.Constant(1L), Expression.Constant(2L)));
        }

        #endregion

        #region TypeBinary

        [TestMethod]
        public void TypeSubstitutionExpressionVisitor_TypeBinary_Simple()
        {
            var subst = GetVisitorSimple();

            var expA = Expression.TypeIs(Expression.Constant("foo"), typeof(int));
            var expB = Expression.TypeIs(Expression.Constant("foo"), typeof(string));
            var expC = Expression.TypeIs(Expression.Constant(12345), typeof(int));
            var expD = Expression.TypeIs(Expression.Constant(12345), typeof(string));

            var res1 = subst.Apply(expA);
            var res2 = subst.Apply(expB);
            var res3 = subst.Apply(expC);
            var res4 = subst.Apply(expD);

            AssertEqual(res1, Expression.TypeIs(Expression.Constant("foo"), typeof(long)));
            Assert.AreSame(expB, res2);
            AssertEqual(res3, Expression.TypeIs(Expression.Constant(12345L), typeof(long)));
            AssertEqual(res4, Expression.TypeIs(Expression.Constant(12345L), typeof(string)));
        }

        [TestMethod]
        public void TypeSubstitutionExpressionVisitor_TypeBinary_TypeEqual()
        {
            var subst = GetVisitorSimple();

            var expA = Expression.TypeEqual(Expression.Parameter(typeof(Type)), typeof(int));
            var expB = Expression.TypeEqual(Expression.Constant(typeof(Type)), typeof(string));

            var res1 = subst.Apply(expA);
            var res2 = subst.Apply(expB);

            AssertEqual(res1, Expression.TypeEqual(Expression.Parameter(typeof(Type)), typeof(long)));
            Assert.AreSame(expB, res2);
        }

        #endregion

        #region Unary

        [TestMethod]
        public void TypeSubstitutionExpressionVisitor_Unary_Simple()
        {
            var subst = GetVisitorSimple();

            var expA = Expression.Negate(Expression.Constant(1));
            var expB = Expression.Not(Expression.Constant(false));

            var res1 = subst.Apply(expA);
            var res2 = subst.Apply(expB);

            AssertEqual(res1, Expression.Negate(Expression.Constant(1L)));
            Assert.AreSame(expB, res2);
        }

        [TestMethod]
        public void TypeSubstitutionExpressionVisitor_Unary_CustomOperator()
        {
            var subst = GetVisitorSimple();

            var expA = Expression.Negate(Expression.Parameter(typeof(Bar)));

            var res1 = subst.Apply(expA);

            AssertEqual(res1, Expression.Negate(Expression.Parameter(typeof(Foo))));
        }

        [TestMethod]
        public void TypeSubstitutionExpressionVisitor_Unary_Convert()
        {
            var subst = GetVisitorSimple();

            var expA = Expression.Convert(Expression.Parameter(typeof(object)), typeof(int));
            var expB = Expression.Convert(Expression.Parameter(typeof(object)), typeof(string));

            var res1 = subst.Apply(expA);
            var res2 = subst.Apply(expB);

            AssertEqual(res1, Expression.Convert(Expression.Parameter(typeof(object)), typeof(long)));
            Assert.AreSame(expB, res2);
        }

        #endregion

        #region E2E

        [TestMethod]
        public void TypeSubstitutionExpressionVisitor_Anonymize()
        {
            var pers = typeof(Person);
            var query = (Expression<Func<IEnumerable<Person>, IEnumerable<string>>>)(xs => from x in xs where x.Age > 10 let name = x.Name where name.StartsWith("B") select name.ToUpper() + " is " + x.Age);

            var check1 = new TypeErasureChecker(new[] { typeof(Person) });
            Assert.ThrowsException<InvalidOperationException>(() => check1.Visit(query));


            var anon = RuntimeCompiler.CreateAnonymousType(new[]
            {
                new KeyValuePair<string, Type>("Name", typeof(string)),
                new KeyValuePair<string, Type>("Age", typeof(int)),
            });

            var subst1 = new TypeSubstitutionExpressionVisitor(new Dictionary<Type, Type>
            {
                { pers, anon }
            });

            var res1 = subst1.Apply(query);

            check1.Visit(res1);

            var check2 = new TypeErasureChecker(new[] { anon });
            Assert.ThrowsException<InvalidOperationException>(() => check2.Visit(res1));


            var f = ((LambdaExpression)res1).Compile();

            var cast = ((MethodInfo)ReflectionHelpers.InfoOf(() => Enumerable.Cast<int>(null))).GetGenericMethodDefinition().MakeGenericMethod(anon);
            var peopleObj = new[] { Activator.CreateInstance(anon, new object[] { "Bart", 10 }), Activator.CreateInstance(anon, new object[] { "Lisa", 8 }), Activator.CreateInstance(anon, new object[] { "Bart", 21 }) };
            var peopleAnon = cast.Invoke(obj: null, new object[] { peopleObj });
            var qres = (IEnumerable<string>)f.DynamicInvoke(peopleAnon);
            Assert.IsTrue(new[] { "BART is 21" }.SequenceEqual(qres));

            var subst2 = new TypeSubstitutionExpressionVisitor(new Dictionary<Type, Type>
            {
                { anon, pers }
            });

            var res2 = subst2.Apply(res1);

            check2.Visit(res2);

            Assert.ThrowsException<InvalidOperationException>(() => check1.Visit(res2));

            var eq = new ExpressionEqualityComparer();

            Assert.IsTrue(eq.Equals(query, res2));
        }

        [TestMethod]
        public void TypeSubstitutionExpressionVisitor_ChangeInterface()
        {
            var query = new[] { 2, 3, 5 }.AsQueryable().Where(x => x > 0).Select(x => x.ToString()).Expression;

            var subst = new TypeSubstitutionExpressionVisitor(new Dictionary<Type, Type>
            {
                { typeof(IQueryable<>), typeof(IEnumerable<>) }
            });

        }

        private sealed class TypeErasureChecker : ExpressionVisitor
        {
            private readonly Checker _checker;

            public TypeErasureChecker(Type[] disallow)
            {
                _checker = new Checker(disallow);
            }

            public override Expression Visit(Expression node)
            {
                if (node != null)
                {
                    _checker.Visit(node.Type);
                }

                return base.Visit(node);
            }

            private sealed class Checker : TypeVisitor
            {
                private readonly Type[] _disallow;

                public Checker(Type[] disallow)
                {
                    _disallow = disallow;
                }

                public override Type Visit(Type type)
                {
                    if (_disallow.Contains(type))
                    {
                        throw new InvalidOperationException();
                    }

                    return base.Visit(type);
                }
            }
        }

        #endregion

        #region Advanced

        [TestMethod]
        public void TypeSubstitutionExpressionVisitor_StubbornTypeChange_Fail()
        {
            Expression<Func<string, int>> f = s => Math.Abs(s.Length);

            var subst = new MyTypeSubstitutionExpressionVisitor();

            Assert.ThrowsException<InvalidOperationException>(() => subst.Apply(f));
        }

        [TestMethod]
        public void TypeSubstitutionExpressionVisitor_StubbornTypeChange_NotQuite()
        {
            Expression<Func<string, int>> f = s => Math.Abs(s.Length);

            var subst = new NotQuiteForgivingTypeSubstitutionExpressionVisitor();

            Assert.ThrowsException<InvalidOperationException>(() => subst.Apply(f));
        }

        private sealed class NotQuiteForgivingTypeSubstitutionExpressionVisitor : MyTypeSubstitutionExpressionVisitor
        {
            protected override MemberInfo ResolveProperty(PropertyInfo originalProperty, Type declaringType, Type propertyType, Type[] indexerParameters)
            {
                if (originalProperty == (PropertyInfo)ReflectionHelpers.InfoOf(() => "".Length))
                {
                    return originalProperty;
                }

                return base.ResolveProperty(originalProperty, declaringType, propertyType, indexerParameters);
            }
        }

        [TestMethod]
        public void TypeSubstitutionExpressionVisitor_StubbornTypeChange_Resolve()
        {
            Expression<Func<string, int>> f = s => Math.Abs(s.Length);

            var subst = new ForgivingTypeSubstitutionExpressionVisitor();

            var res = subst.Apply(f);

            Expression<Func<string, long>> g = s => Math.Abs((long)s.Length);

            AssertEqual(g, res);
        }

        private sealed class ForgivingTypeSubstitutionExpressionVisitor : MyTypeSubstitutionExpressionVisitor
        {
            protected override MemberInfo ResolveProperty(PropertyInfo originalProperty, Type declaringType, Type propertyType, Type[] indexerParameters)
            {
                if (originalProperty == (PropertyInfo)ReflectionHelpers.InfoOf(() => "".Length))
                {
                    return originalProperty;
                }

                return base.ResolveProperty(originalProperty, declaringType, propertyType, indexerParameters);
            }

            protected override Expression ConvertExpression(Expression originalExpression, Expression resultExpression, Type newType)
            {
                return Expression.Convert(resultExpression, newType);
            }
        }

        [TestMethod]
        public void TypeSubstitutionExpressionVisitor_PrivateReflection()
        {
            Expression<Func<int, int>> f = x => _Bar(x);

            var subst = new MyTypeSubstitutionExpressionVisitor();

            var res = subst.Apply(f);

            Expression<Func<long, long>> g = x => _Bar(x);

            AssertEqual(g, res);
        }

        private static int _Bar(int x)
        {
            throw new NotImplementedException();
        }

        private static long _Bar(long x)
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void TypeSubstitutionExpressionVisitor_ConstantChange_Fail()
        {
            Expression<Func<int>> f = () => 42;

            var subst = new TypeSubstitutionExpressionVisitor(new Dictionary<Type, Type>
            {
                { typeof(int), typeof(long) }
            });

            Assert.ThrowsException<InvalidOperationException>(() => subst.Apply(f));
        }

        [TestMethod]
        public void TypeSubstitutionExpressionVisitor_ConstantChange_Resolve()
        {
            Expression<Func<int>> f = () => 42;

            var subst = new ConstantChangingTypeSubstitutionExpressionVisitor();

            var res = subst.Apply(f);

            Expression<Func<long>> g = () => 42L;

            AssertEqual(g, res);
        }

        private sealed class ConstantChangingTypeSubstitutionExpressionVisitor : MyTypeSubstitutionExpressionVisitor
        {
            protected override object ConvertConstant(object oldValue, Type newType)
            {
                return newType == typeof(long) && oldValue is int i ? (long)i : base.ConvertConstant(oldValue, newType);
            }
        }

        #endregion

        #region Private implementation

        private static void AssertEqual(Expression a, Expression b)
        {
            var fva = FreeVariableScanner.Scan(a);
            var fvb = FreeVariableScanner.Scan(b);

            var la = Expression.Lambda(a, fva);
            var lb = Expression.Lambda(b, fvb);

            Assert.IsTrue(new ExpressionEqualityComparer().Equals(la, lb));
        }

        private static TypeSubstitutionExpressionVisitor GetVisitorSimple()
        {
            return new MyTypeSubstitutionExpressionVisitor();
        }

        private class MyTypeSubstitutionExpressionVisitor : TypeSubstitutionExpressionVisitor
        {
            public MyTypeSubstitutionExpressionVisitor()
                : base(new Dictionary<Type, Type>
                {
                    { typeof(int), typeof(long) },
                    { typeof(Bar), typeof(Foo) },
                    { typeof(Banana), typeof(Fruit) }
                })
            {
            }

            protected override object ConvertConstant(object oldValue, Type newType)
            {
                return oldValue is int i && newType == typeof(long) ? (long)i : base.ConvertConstant(oldValue, newType);
            }
        }

#pragma warning disable 0649
#pragma warning disable IDE0060 // Remove unused parameter
#pragma warning disable CA1822 // Mark static
        private sealed class Bar
        {
            public Bar(int x)
            {
            }

            public int this[int x] => throw new NotImplementedException();

            public static string Baz;
            public int Qux { get; set; }
            public Bar Joey { get; private set; }
            public List<int> Xs { get; private set; }
            public string Stubborn { get; set; }
            public List<string> Ys { get; private set; }

            public int Good1;
            public int Wrong2;
            public int Wrong3;

            public static Bar operator +(Bar b1, int x) => throw new NotImplementedException();

            public static Bar operator +(Bar b1, string x) => throw new NotImplementedException();

            public static Bar operator -(Bar b) => throw new NotImplementedException();
        }

        private sealed class Foo
        {
            public Foo(long x)
            {
            }

            public static string Baz;
            public long Qux { get; set; }
            public Foo Joey { get; private set; }
            public List<long> Xs { get; private set; }
            public string Stubborn { get; set; }
            public List<string> Ys { get; private set; }

            public long Good1;
            public string Wrong2;
            public long Verkeerd3;

            public static Foo operator +(Foo f1, long x) => throw new NotImplementedException();

            public static Foo operator +(Foo f1, string x) => throw new NotImplementedException();

            public static Foo operator -(Foo b) => throw new NotImplementedException();
        }

        private sealed class Baz
        {
            public Baz(int x)
            {
            }

            public int x;
            public int Y { get; private set; }
            public int Z(int x) { return 42; }
            public int A<T>(int x) { return x; }
        }

        private sealed class Person
        {
            public string Name { get; set; }
            public int Age { get; set; }
        }
#pragma warning restore CA1822
#pragma warning restore IDE0060 // Remove unused parameter
#pragma warning restore 0649

        #endregion
    }
}
