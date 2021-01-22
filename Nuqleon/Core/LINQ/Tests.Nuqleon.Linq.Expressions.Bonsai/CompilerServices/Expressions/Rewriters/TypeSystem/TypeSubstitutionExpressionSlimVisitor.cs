// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - October 2013 - Created this file.
//

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.CompilerServices;
using System.Linq.CompilerServices.Bonsai;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Tests.System.Linq.Expressions.Bonsai.CompilerServices
{
    [TestClass]
    public class TypeSubstitutionExpressionSlimVisitorTests : TestBase
    {
        [TestMethod]
        public void TypeSubstitutionExpressionSlimVisitor_ArgumentChecking()
        {
            AssertEx.ThrowsException<ArgumentNullException>(() => new TypeSubstitutionExpressionSlimVisitor(default(TypeSlimSubstitutor)), ex => Assert.AreEqual("typeSubstitutor", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => new TypeSubstitutionExpressionSlimVisitor(default(IDictionary<TypeSlim, TypeSlim>)), ex => Assert.AreEqual("typeMap", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => GetVisitorSimple().Apply(expression: null), ex => Assert.AreEqual("expression", ex.ParamName));

            new MySubst().Do();
        }

        [TestMethod]
        public void TypeSlimSubstitutor_Assembly()
        {
            Type type = typeof(TypeSlimSubstitutor);
            string assembly = type.Assembly.GetName().Name;
            Assert.AreEqual("Nuqleon.Linq.Expressions.Bonsai", assembly);
        }

        private sealed class MySubst : TypeSubstitutionExpressionSlimVisitor
        {
            public MySubst()
                : base(new Dictionary<TypeSlim, TypeSlim>())
            {
            }

            public void Do()
            {
                var t = typeof(int).ToTypeSlim();
                var ts = new[] { t };

                var ctor = typeof(MySubst).ToTypeSlim().GetConstructor(EmptyReadOnlyCollection<TypeSlim>.Instance);

                AssertEx.ThrowsException<ArgumentNullException>(() => base.ResolveConstructor(originalConstructor: null, t, ts), ex => Assert.AreEqual("originalConstructor", ex.ParamName));
                AssertEx.ThrowsException<ArgumentNullException>(() => base.ResolveConstructor(ctor, declaringType: null, ts), ex => Assert.AreEqual("declaringType", ex.ParamName));
                AssertEx.ThrowsException<ArgumentNullException>(() => base.ResolveConstructor(ctor, t, parameters: null), ex => Assert.AreEqual("parameters", ex.ParamName));

                var prop = typeof(string).ToTypeSlim().GetProperty("Length", typeof(int).ToTypeSlim(), EmptyReadOnlyCollection<TypeSlim>.Instance, canWrite: false);

                AssertEx.ThrowsException<ArgumentNullException>(() => base.ResolveProperty(originalProperty: null, t, t, ts), ex => Assert.AreEqual("originalProperty", ex.ParamName));
                AssertEx.ThrowsException<ArgumentNullException>(() => base.ResolveProperty(prop, declaringType: null, t, ts), ex => Assert.AreEqual("declaringType", ex.ParamName));
                //AssertEx.Throws<ArgumentNullException>(() => base.ResolveProperty(prop, t, propertyType: null, ts), ex => Assert.AreEqual("propertyType", ex.ParamName));
                AssertEx.ThrowsException<ArgumentNullException>(() => base.ResolveProperty(prop, t, t, indexerParameters: null), ex => Assert.AreEqual("indexerParameters", ex.ParamName));

                var fld = typeof(string).ToTypeSlim().GetField("Empty", typeof(string).ToTypeSlim());

                AssertEx.ThrowsException<ArgumentNullException>(() => base.ResolveField(originalField: null, t, t), ex => Assert.AreEqual("originalField", ex.ParamName));
                AssertEx.ThrowsException<ArgumentNullException>(() => base.ResolveField(fld, declaringType: null, t), ex => Assert.AreEqual("declaringType", ex.ParamName));
                //AssertEx.Throws<ArgumentNullException>(() => base.ResolveField(fld, t, fieldType: null), ex => Assert.AreEqual("fieldType", ex.ParamName));

                var mtd = typeof(string).ToTypeSlim().GetSimpleMethod("ToUpper", EmptyReadOnlyCollection<TypeSlim>.Instance, typeof(string).ToTypeSlim());

                AssertEx.ThrowsException<ArgumentNullException>(() => base.ResolveMethod(originalMethod: null, t, ts, ts, t), ex => Assert.AreEqual("originalMethod", ex.ParamName));
                AssertEx.ThrowsException<ArgumentNullException>(() => base.ResolveMethod(mtd, declaringType: null, ts, ts, t), ex => Assert.AreEqual("declaringType", ex.ParamName));
                AssertEx.ThrowsException<ArgumentNullException>(() => base.ResolveMethod(mtd, t, genericArguments: null, ts, t), ex => Assert.AreEqual("genericArguments", ex.ParamName));
                AssertEx.ThrowsException<ArgumentNullException>(() => base.ResolveMethod(mtd, t, ts, parameters: null, t), ex => Assert.AreEqual("parameters", ex.ParamName));
                AssertEx.ThrowsException<ArgumentNullException>(() => base.ResolveMethod(mtd, t, ts, ts, returnType: null), ex => Assert.AreEqual("returnType", ex.ParamName));
            }
        }

        #region Binary

        [TestMethod]
        public void TypeSubstitutionExpressionSlimVisitor_Binary_Simple()
        {
            var subst = GetVisitorSimple();

            var expA = Expression.Add(Expression.Constant(1), Expression.Constant(2));
            var expB = Expression.Add(Expression.Constant(1.0), Expression.Constant(2.0));
            var expBslim = expB.ToExpressionSlim();

            var res1 = subst.Apply(expA.ToExpressionSlim());
            var res2 = subst.Apply(expBslim);

            ReduceAndAssertEqual(res1, Expression.Add(Expression.Constant(1L), Expression.Constant(2L)));
            Assert.AreSame(expBslim, res2);
        }

        [TestMethod]
        public void TypeSubstitutionExpressionSlimVisitor_Binary_CustomOperator()
        {
            var subst = GetVisitorSimple();

            var expA = Expression.Add(Expression.Parameter(typeof(TestBar)), Expression.Constant(2));
            var expB = Expression.Add(Expression.Parameter(typeof(TestBar)), Expression.Constant("foo"));

            var res1 = subst.Apply(expA.ToExpressionSlim());
            var res2 = subst.Apply(expB.ToExpressionSlim());

            ReduceAndAssertEqual(res1, Expression.Add(Expression.Parameter(typeof(TestFoo)), Expression.Constant(2L)));
            ReduceAndAssertEqual(res2, Expression.Add(Expression.Parameter(typeof(TestFoo)), Expression.Constant("foo")));
        }

        [TestMethod]
        public void TypeSubstitutionExpressionSlimVisitor_Binary_Coalesce()
        {
            var subst = GetVisitorSimple();

            var p = Expression.Parameter(typeof(TestBar));
            var i = Expression.Lambda(p, p);
            var exp = Expression.Coalesce(Expression.Parameter(typeof(TestBar)), Expression.Constant(value: null, typeof(TestBar)), i);

            var res = subst.Apply(exp.ToExpressionSlim());

            var q = Expression.Parameter(typeof(TestFoo));
            var j = Expression.Lambda(q, q);
            ReduceAndAssertEqual(res, Expression.Coalesce(Expression.Parameter(typeof(TestFoo)), Expression.Constant(value: null, typeof(TestFoo)), j));
        }

        #endregion

        #region Block

        [TestMethod]
        public void TypeSubstitutionExpressionSlimVisitor_Block_Simple()
        {
            {
                var subst = GetVisitorSimple();

                var expA = Expression.Block(Expression.Constant("a"), Expression.Constant(2));
                var expB = Expression.Block(Expression.Constant("a"), Expression.Constant(2.0));
                var expBslim = expB.ToExpressionSlim();

                var res1 = subst.Apply(expA.ToExpressionSlim());
                var res2 = subst.Apply(expBslim);

                ReduceAndAssertEqual(res1, Expression.Block(Expression.Constant("a"), Expression.Constant(2L)));
                Assert.AreSame(expBslim, res2);
            }

            {
                var subst = GetVisitorSimple();

                var expA = Expression.Block(Expression.Constant(2), Expression.Constant("a"));
                var expB = Expression.Block(Expression.Constant(2.0), Expression.Constant("a"));
                var expBslim = expB.ToExpressionSlim();

                var res1 = subst.Apply(expA.ToExpressionSlim());
                var res2 = subst.Apply(expBslim);

                ReduceAndAssertEqual(res1, Expression.Block(Expression.Constant(2L), Expression.Constant("a")));
                Assert.AreSame(expBslim, res2);
            }

            {
                var subst = GetVisitorSimple();

                var pInt = Expression.Parameter(typeof(int));
                var pFloat = Expression.Parameter(typeof(float));
                var pLong = Expression.Parameter(typeof(long));
                var expA = Expression.Block(new[] { pInt }, pInt);
                var expB = Expression.Block(new[] { pFloat }, pFloat);
                var expBslim = expB.ToExpressionSlim();

                var res1 = subst.Apply(expA.ToExpressionSlim());
                var res2 = subst.Apply(expBslim);

                ReduceAndAssertEqual(res1, Expression.Block(new[] { pLong }, pLong));
                Assert.AreSame(expBslim, res2);
            }
        }

        #endregion

        #region Condition

        [TestMethod]
        public void TypeSubstitutionExpressionSlimVisitor_Condition_Simple()
        {
            var subst = GetVisitorSimple();

            var expA = Expression.Condition(Expression.Constant(true), Expression.Constant(2), Expression.Constant(3));
            var expB = Expression.Condition(Expression.Constant(true), Expression.Constant(2.0), Expression.Constant(3.0));
            var expBslim = expB.ToExpressionSlim();

            var res1 = subst.Apply(expA.ToExpressionSlim());
            var res2 = subst.Apply(expBslim);

            ReduceAndAssertEqual(res1, Expression.Condition(Expression.Constant(true), Expression.Constant(2L), Expression.Constant(3L)));
            Assert.AreSame(expBslim, res2);
        }

        #endregion

        #region Constant

        [TestMethod]
        public void TypeSubstitutionExpressionSlimVisitor_Constant()
        {
            var subst = GetVisitorSimple();
            var banana = new Banana();

            var expA = Expression.Constant(42, typeof(int));
            var expB = Expression.Constant("foo");
            var expBslim = expB.ToExpressionSlim();
            var expC = Expression.Constant(value: null, typeof(TestBar));
            var expD = Expression.Constant(value: null, typeof(int?));
            var expE = Expression.Constant(banana, typeof(Banana));

            var res1 = subst.Apply(expA.ToExpressionSlim());
            var res2 = subst.Apply(expBslim);
            var res3 = subst.Apply(expC.ToExpressionSlim());
            var res4 = subst.Apply(expD.ToExpressionSlim());
            var res5 = subst.Apply(expE.ToExpressionSlim());

            ReduceAndAssertEqual(res1, Expression.Constant(42L, typeof(long)));

            Assert.AreSame(expBslim, res2);

            ReduceAndAssertEqual(res3, Expression.Constant(value: null, typeof(TestFoo)));
            ReduceAndAssertEqual(res4, Expression.Constant(value: null, typeof(long?)));
            ReduceAndAssertEqual(res5, Expression.Constant(banana, typeof(Fruit)));
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
        public void TypeSubstitutionExpressionSlimVisitor_Default()
        {
            var subst = GetVisitorSimple();

            var expA = Expression.Default(typeof(int));
            var expB = Expression.Default(typeof(string));
            var expBslim = expB.ToExpressionSlim();

            var res1 = subst.Apply(expA.ToExpressionSlim());
            var res2 = subst.Apply(expBslim);

            ReduceAndAssertEqual(res1, Expression.Default(typeof(long)));

            Assert.AreSame(expBslim, res2);
        }

        #endregion

        #region Goto

        [TestMethod]
        public void TypeSubstitutionExpressionSlimVisitor_Goto()
        {
            var subst = GetVisitorSimple();

            var labelInt = Expression.Label(typeof(int));
            var labelDouble = Expression.Label(typeof(double));
            var labelLong = Expression.Label(typeof(long));
            var expA =
                Expression.Block(
                    Expression.Goto(labelInt, Expression.Constant(1)),
                    Expression.Label(labelInt, Expression.Constant(1))
                );
            var expB =
                Expression.Block(
                    Expression.Goto(labelDouble, Expression.Constant(1.0)),
                    Expression.Label(labelDouble, Expression.Constant(1.0))
                );
            var expBslim = expB.ToExpressionSlim();

            var res1 = subst.Apply(expA.ToExpressionSlim());
            var res2 = subst.Apply(expBslim);

            ReduceAndAssertEqual(
                res1,
                Expression.Block(
                    Expression.Goto(labelLong, Expression.Constant(1L)),
                    Expression.Label(labelLong, Expression.Constant(1L))
                )
            );
            Assert.AreSame(expBslim, res2);
        }

        #endregion

        #region Index

        [TestMethod]
        public void TypeSubstitutionExpressionSlimVisitor_Index()
        {
            var subst = GetVisitorSimple();

            var expA = Expression.MakeIndex(Expression.Parameter(typeof(Dictionary<int, string>)), typeof(Dictionary<int, string>).GetProperty("Item"), new[] { Expression.Constant(1) });
            var expB = Expression.MakeIndex(Expression.Parameter(typeof(Dictionary<double, string>)), typeof(Dictionary<double, string>).GetProperty("Item"), new[] { Expression.Constant(1.0) });
            var expBslim = expB.ToExpressionSlim();

            var res1 = subst.Apply(expA.ToExpressionSlim());
            var res2 = subst.Apply(expBslim);

            ReduceAndAssertEqual(res1, Expression.MakeIndex(Expression.Parameter(typeof(Dictionary<long, string>)), typeof(Dictionary<long, string>).GetProperty("Item"), new[] { Expression.Constant(1L) }));
            Assert.AreSame(expBslim, res2);
        }

        [TestMethod]
        public void TypeSubstitutionExpressionSlimVisitor_Index_PropertyMissing()
        {
            var subst = GetVisitorSimple();

            var exp = Expression.MakeIndex(Expression.Parameter(typeof(TestBar)), typeof(TestBar).GetProperty("Item"), new[] { Expression.Constant(1) });

            Assert.ThrowsException<InvalidOperationException>(() => subst.Apply(exp.ToExpressionSlim()).ToExpression());
        }

        #endregion

        #region Invoke

        [TestMethod]
        public void TypeSubstitutionExpressionSlimVisitor_Invoke()
        {
            var subst = GetVisitorSimple();

            var expA = Expression.Invoke(Expression.Parameter(typeof(Func<int, int>)), Expression.Constant(42));
            var expB = Expression.Invoke(Expression.Parameter(typeof(Func<string, string>)), Expression.Constant("foo"));
            var expBslim = expB.ToExpressionSlim();

            var res1 = subst.Apply(expA.ToExpressionSlim());
            var res2 = subst.Apply(expBslim);

            ReduceAndAssertEqual(res1, Expression.Invoke(Expression.Parameter(typeof(Func<long, long>)), Expression.Constant(42L)));

            Assert.AreSame(expBslim, res2);
        }

        #endregion

        #region Lambda

        [TestMethod]
        public void TypeSubstitutionExpressionSlimVisitor_Lambda()
        {
            var subst = GetVisitorSimple();

            var expA = (Expression<Func<int, int>>)(x => x);
            var expB = (Expression<Func<double, double>>)(x => x);
            var expBslim = expB.ToExpressionSlim();
            var expC = (Expression<Func<int, Func<string, int>>>)(x => s => x);

            var res1 = subst.Apply(expA.ToExpressionSlim());
            var res2 = subst.Apply(expBslim);
            var res3 = subst.Apply(expC.ToExpressionSlim());

            ReduceAndAssertEqual(res1, (Expression<Func<long, long>>)(x => x));
            Assert.AreSame(expBslim, res2);
            ReduceAndAssertEqual(res3, (Expression<Func<long, Func<string, long>>>)(x => s => x));
        }

        #endregion

        #region Loop

        [TestMethod]
        public void TypeSubstitutionExpressionSlimVisitor_Loop()
        {
            var subst = GetVisitorSimple();

            var labelInt = Expression.Label(typeof(int));
            var labelDouble = Expression.Label(typeof(double));
            var labelLong = Expression.Label(typeof(long));
            var expA =
                Expression.Loop(
                    Expression.Break(labelInt, Expression.Constant(1))
                );
            var expB =
                Expression.Loop(
                    Expression.Break(labelDouble, Expression.Constant(1.0))
                );
            var expBslim = expB.ToExpressionSlim();

            var res1 = subst.Apply(expA.ToExpressionSlim());
            var res2 = subst.Apply(expBslim);

            ReduceAndAssertEqual(
                res1,
                Expression.Loop(
                    Expression.Break(labelLong, Expression.Constant(1L))
                )
            );
            Assert.AreSame(expBslim, res2);
        }

        #endregion

        #region ListInit

        [TestMethod]
        public void TypeSubstitutionExpressionSlimVisitor_ListInit()
        {
            var subst = GetVisitorSimple();

            var expA = ((Expression<Func<List<int>>>)(() => new List<int> { 2, 3, 5 })).Body;
            var expB = ((Expression<Func<List<double>>>)(() => new List<double> { 2.0, 3.0, 5.0 })).Body;
            var expBslim = expB.ToExpressionSlim();

            var res1 = subst.Apply(expA.ToExpressionSlim());
            var res2 = subst.Apply(expBslim);

            ReduceAndAssertEqual(res1, ((Expression<Func<List<long>>>)(() => new List<long> { 2L, 3L, 5L })).Body);
            Assert.AreSame(expBslim, res2);
        }

        #endregion

        #region Member

        [TestMethod]
        public void TypeSubstitutionExpressionSlimVisitor_Member_Instance()
        {
            var subst = GetVisitorSimple();

            var barQux = (PropertyInfo)ReflectionHelpers.InfoOf((TestBar b) => b.Qux);
            var fooQux = (PropertyInfo)ReflectionHelpers.InfoOf((TestFoo f) => f.Qux);

            var expA = Expression.Property(Expression.Parameter(typeof(TestBar)), barQux);
            var expB = Expression.Property(Expression.Parameter(typeof(AppDomain)), "BaseDirectory");
            var expBslim = expB.ToExpressionSlim();

            var res1 = subst.Apply(expA.ToExpressionSlim());
            var res2 = subst.Apply(expBslim);

            ReduceAndAssertEqual(res1, Expression.Property(Expression.Parameter(typeof(TestFoo)), fooQux));
            Assert.AreSame(expBslim, res2);
        }

        [TestMethod]
        public void TypeSubstitutionExpressionSlimVisitor_Member_Instance_ResultTypeIncompatible()
        {
            var subst = GetVisitorSimple();

            var exp = Expression.Property(Expression.Parameter(typeof(string)), "Length");

            Assert.ThrowsException<InvalidOperationException>(() => subst.Apply(exp.ToExpressionSlim()).ToExpression());
        }

        [TestMethod]
        public void TypeSubstitutionExpressionSlimVisitor_Member_Static()
        {
            var subst = GetVisitorSimple();

            var barBaz = (FieldInfo)ReflectionHelpers.InfoOf(() => TestBar.Baz);
            var fooBaz = (FieldInfo)ReflectionHelpers.InfoOf(() => TestFoo.Baz);

            var expA = Expression.Field(expression: null, barBaz);
            var expB = Expression.Property(expression: null, (PropertyInfo)ReflectionHelpers.InfoOf(() => DateTime.Now));
            var expBslim = expB.ToExpressionSlim();
            var expC = Expression.Field(expression: null, (FieldInfo)ReflectionHelpers.InfoOf(() => DateTime.MaxValue));
            var expCslim = expC.ToExpressionSlim();

            var res1 = subst.Apply(expA.ToExpressionSlim());
            var res2 = subst.Apply(expBslim);
            var res3 = subst.Apply(expCslim);

            ReduceAndAssertEqual(res1, Expression.Field(expression: null, fooBaz));
            Assert.AreSame(expBslim, res2);
            Assert.AreSame(expCslim, res3);
        }

        [TestMethod]
        public void TypeSubstitutionExpressionSlimVisitor_Member_FieldMissing()
        {
            var subst = GetVisitorSimple();

            var exp1 = (Expression<Func<TestBaz, int>>)((TestBaz b) => b.x);
            var exp2 = (Expression<Func<TestBar, int>>)((TestBar b) => b.Wrong2);
            var exp3 = (Expression<Func<TestBar, int>>)((TestBar b) => b.Wrong3);

            Assert.ThrowsException<ArgumentException>(() => subst.Apply(exp1.ToExpressionSlim()).ToExpression());
            Assert.ThrowsException<ArgumentException>(() => subst.Apply(exp2.ToExpressionSlim()).ToExpression());
            Assert.ThrowsException<InvalidOperationException>(() => subst.Apply(exp3.ToExpressionSlim()).ToExpression());
        }

        [TestMethod]
        public void TypeSubstitutionExpressionSlimVisitor_Member_PropertyMissing()
        {
            var subst = GetVisitorSimple();

            var exp = (Expression<Func<TestBaz, int>>)((TestBaz b) => b.Y);

            Assert.ThrowsException<InvalidOperationException>(() => subst.Apply(exp.ToExpressionSlim()).ToExpression());
        }

        #endregion

        #region MemberInit

        [TestMethod]
        public void TypeSubstitutionExpressionSlimVisitor_MemberInit()
        {
            var subst = GetVisitorSimple();

            var expA = ((Expression<Func<TestBar>>)(() => new TestBar(1) { Qux = 2, Xs = { 3, 4 }, Joey = { Qux = 5 }, Stubborn = "foo", Ys = { "qux" } })).Body;
            var expB = ((Expression<Func<TestFoo>>)(() => new TestFoo(1) { Qux = 2, Xs = { 3, 4 }, Joey = { Qux = 5 }, Stubborn = "foo", Ys = { "qux" } })).Body;
            var expBslim = expB.ToExpressionSlim();
            var res1 = subst.Apply(expA.ToExpressionSlim());
            var res2 = subst.Apply(expBslim);

            ReduceAndAssertEqual(res1, ((Expression<Func<TestFoo>>)(() => new TestFoo(1L) { Qux = 2L, Xs = { 3L, 4L }, Joey = { Qux = 5L }, Stubborn = "foo", Ys = { "qux" } })).Body);
            Assert.AreSame(expBslim, res2);
        }

        #endregion

        #region MethodCall

        [TestMethod]
        public void TypeSubstitutionExpressionSlimVisitor_MethodCall_Static()
        {
            var subst = GetVisitorSimple();

            var cwInt32 = (MethodInfo)ReflectionHelpers.InfoOf(() => Console.WriteLine(42));
            var cwInt64 = (MethodInfo)ReflectionHelpers.InfoOf(() => Console.WriteLine(1L));
            var cwStrng = (MethodInfo)ReflectionHelpers.InfoOf(() => Console.WriteLine(""));

            var expA = Expression.Call(cwInt32, Expression.Constant(12345));
            var expB = Expression.Call(cwStrng, Expression.Constant("foo"));
            var expBslim = expB.ToExpressionSlim();

            var res1 = subst.Apply(expA.ToExpressionSlim());
            var res2 = subst.Apply(expBslim);

            ReduceAndAssertEqual(res1, Expression.Call(cwInt64, Expression.Constant(12345L)));

            Assert.AreSame(expBslim, res2);
        }

        [TestMethod]
        public void TypeSubstitutionExpressionSlimVisitor_MethodCall_GenericType()
        {
            var subst = GetVisitorSimple();

            var lstInt32Add = (MethodInfo)ReflectionHelpers.InfoOf((List<int> l) => l.Add(42));
            var lstInt64Add = (MethodInfo)ReflectionHelpers.InfoOf((List<long> l) => l.Add(1L));
            var lstStrngAdd = (MethodInfo)ReflectionHelpers.InfoOf((List<string> l) => l.Add(""));

            var expA = Expression.Call(Expression.Parameter(typeof(List<int>)), lstInt32Add, Expression.Constant(12345));
            var expB = Expression.Call(Expression.Parameter(typeof(List<string>)), lstStrngAdd, Expression.Constant("foo"));
            var expBslim = expB.ToExpressionSlim();

            var res1 = subst.Apply(expA.ToExpressionSlim());
            var res2 = subst.Apply(expBslim);

            ReduceAndAssertEqual(res1, Expression.Call(Expression.Parameter(typeof(List<long>)), lstInt64Add, Expression.Constant(12345L)));

            Assert.AreSame(expBslim, res2);
        }

        [TestMethod]
        public void TypeSubstitutionExpressionSlimVisitor_MethodCall_GenericMethod()
        {
            var subst = GetVisitorSimple();

            var aciInt32 = (MethodInfo)ReflectionHelpers.InfoOf(() => Activator.CreateInstance<int>());
            var aciInt64 = (MethodInfo)ReflectionHelpers.InfoOf(() => Activator.CreateInstance<long>());
            var aciStrng = (MethodInfo)ReflectionHelpers.InfoOf(() => Activator.CreateInstance<string>());

            var expA = Expression.Call(aciInt32);
            var expB = Expression.Call(aciStrng);
            var expBslim = expB.ToExpressionSlim();

            var res1 = subst.Apply(expA.ToExpressionSlim());
            var res2 = subst.Apply(expBslim);

            ReduceAndAssertEqual(res1, Expression.Call(aciInt64));

            Assert.AreSame(expBslim, res2);
        }

        [TestMethod]
        public void TypeSubstitutionExpressionSlimVisitor_Method_MethodMissing()
        {
            var subst = GetVisitorSimple();

            var exp1 = (Expression<Func<TestBaz, int>>)((TestBaz b) => b.Z(5));
            var exp2 = (Expression<Func<TestBaz, int>>)((TestBaz b) => b.A<bool>(5));

            Assert.ThrowsException<InvalidOperationException>(() => subst.Apply(exp1.ToExpressionSlim()).ToExpression());
            Assert.ThrowsException<InvalidOperationException>(() => subst.Apply(exp2.ToExpressionSlim()).ToExpression());
        }

        #endregion

        #region New

        [TestMethod]
        public void TypeSubstitutionExpressionSlimVisitor_New_Simple()
        {
            var subst = GetVisitorSimple();

            var barCtor = (ConstructorInfo)ReflectionHelpers.InfoOf(() => new TestBar(1));
            var fooCtor = (ConstructorInfo)ReflectionHelpers.InfoOf(() => new TestFoo(1L));

            var expA = Expression.New(barCtor, Expression.Constant(42));
            var expB = Expression.New((ConstructorInfo)ReflectionHelpers.InfoOf(() => new TimeSpan(123L)), Expression.Constant(123L));
            var expBslim = expB.ToExpressionSlim();

            var res1 = subst.Apply(expA.ToExpressionSlim());
            var res2 = subst.Apply(expBslim);

            ReduceAndAssertEqual(res1, Expression.New(fooCtor, Expression.Constant(42L)));
            Assert.AreSame(expBslim, res2);
        }

        [TestMethod]
        public void TypeSubstitutionExpressionSlimVisitor_New_Generic()
        {
            var subst = GetVisitorSimple();

            var listInt32Ctor = (ConstructorInfo)ReflectionHelpers.InfoOf(() => new List<int>());
            var listInt64Ctor = (ConstructorInfo)ReflectionHelpers.InfoOf(() => new List<long>());

            var exp = Expression.New(listInt32Ctor);

            var res = subst.Apply(exp.ToExpressionSlim());

            ReduceAndAssertEqual(res, Expression.New(listInt64Ctor));
        }

#pragma warning disable IDE0050 // Convert to tuple. (Tests for anonymous types.)

        [TestMethod]
        public void TypeSubstitutionExpressionSlimVisitor_New_Anonymous()
        {
            var subst = GetVisitorSimple();

            var expA = ((Expression<Func<object>>)(() => new { x = 42, y = "foo" })).Body;
            var expB = ((Expression<Func<object>>)(() => new { x = 42L, y = "foo" })).Body;

            var res = subst.Apply(expA.ToExpressionSlim());

            ReduceAndAssertEqual(res, expB);
        }

#pragma warning restore IDE0050

        [TestMethod]
        public void TypeSubstitutionExpressionSlimVisitor_New_ConstructorMissing()
        {
            var subst = GetVisitorSimple();

            var exp = (Expression<Func<TestBaz>>)(() => new TestBaz(5));

            Assert.ThrowsException<InvalidOperationException>(() => subst.Apply(exp.ToExpressionSlim()).ToExpression());
        }

        [TestMethod]
        public void TypeSubstitutionExpressionSlimVisitor_New_ValueType_DefaultConstructor()
        {
            var subst = GetVisitorSimple();
            var exp = Expression.Lambda(Expression.New(typeof(int)));
            var res = subst.Apply(exp.ToExpressionSlim());
            ReduceAndAssertEqual(res, Expression.Lambda(Expression.New(typeof(long))));
        }

        #endregion

        #region NewArray

        [TestMethod]
        public void TypeSubstitutionExpressionSlimVisitor_NewArrayBounds_Simple()
        {
            var subst = new TypeSubstitutionExpressionSlimVisitor(new Dictionary<TypeSlim, TypeSlim> { { typeof(string).ToTypeSlim(), typeof(bool).ToTypeSlim() } });

            var expA = Expression.NewArrayBounds(typeof(string), Expression.Constant(1));
            var expB = Expression.NewArrayBounds(typeof(int), Expression.Constant(1));
            var expBslim = expB.ToExpressionSlim();

            var res1 = subst.Apply(expA.ToExpressionSlim());
            var res2 = subst.Apply(expBslim);

            ReduceAndAssertEqual(res1, Expression.NewArrayBounds(typeof(bool), Expression.Constant(1)));
            Assert.AreSame(expBslim, res2);
        }

        [TestMethod]
        public void TypeSubstitutionExpressionSlimVisitor_NewArrayInit_Simple()
        {
            var subst = new TypeSubstitutionExpressionSlimVisitor(new Dictionary<TypeSlim, TypeSlim> { { typeof(string).ToTypeSlim(), typeof(bool).ToTypeSlim() } });

            var expA = Expression.NewArrayInit(typeof(string), Expression.Parameter(typeof(string), "e1"), Expression.Parameter(typeof(string), "e2"));
            var expB = Expression.NewArrayInit(typeof(int), Expression.Parameter(typeof(int), "e1"), Expression.Parameter(typeof(int), "e2"));
            var expBslim = expB.ToExpressionSlim();

            var res1 = subst.Apply(expA.ToExpressionSlim());
            var res2 = subst.Apply(expBslim);

            ReduceAndAssertEqual(res1, Expression.NewArrayInit(typeof(bool), Expression.Parameter(typeof(bool), "e1"), Expression.Parameter(typeof(bool), "e2")));
            Assert.AreSame(expBslim, res2);
        }

        #endregion

        #region Parameter

        [TestMethod]
        public void TypeSubstitutionExpressionSlimVisitor_Parameter_Global1()
        {
            var subst = GetVisitorSimple();

            var parA = Expression.Parameter(typeof(int), "a");
            var parB = Expression.Parameter(typeof(string), "b");
            var parBslim = parB.ToExpressionSlim();

            var res1 = subst.Apply(parA.ToExpressionSlim());
            //var res2 = subst.Apply(parA); // TODO: Sharing of globals across substitutions?
            var res3 = subst.Apply(parBslim);
            //var res4 = subst.Apply(parB);

            ReduceAndAssertEqual(res1, Expression.Parameter(typeof(long), "a"));
            //Assert.AreSame(res2, res1);

            Assert.AreSame(parBslim, res3);
            //Assert.AreSame(res4, res3);
        }

        [TestMethod]
        public void TypeSubstitutionExpressionSlimVisitor_Parameter_Global2()
        {
            var subst = GetVisitorSimple();

            var parOld = Expression.Parameter(typeof(int), "a");
            var expOld = Expression.Add(parOld, parOld);

            var res = subst.Apply(expOld.ToExpressionSlim());

            var parNew = Expression.Parameter(typeof(long), "a"); ;
            var expNew = Expression.Add(parNew, parNew);

            ReduceAndAssertEqual(res, expNew);
        }

        #endregion

        #region Switch

        [TestMethod]
        public void TypeSubstitutionExpressionSlimVisitor_Switch()
        {
            var subst = GetVisitorSimple();

            var expA =
                Expression.Switch(
                    Expression.Constant(1),
                    Expression.Constant(1),
                    Expression.SwitchCase(
                        Expression.Constant(1),
                        Expression.Constant(1)
                    )
                );
            var expB =
                Expression.Switch(
                    Expression.Constant(1.0),
                    Expression.Constant(1.0),
                    Expression.SwitchCase(
                        Expression.Constant(1.0),
                        Expression.Constant(1.0)
                    )
                );
            var expBslim = expB.ToExpressionSlim();

            var res1 = subst.Apply(expA.ToExpressionSlim());
            var res2 = subst.Apply(expBslim);

            ReduceAndAssertEqual(
                res1,
                Expression.Switch(
                    Expression.Constant(1L),
                    Expression.Constant(1L),
                    Expression.SwitchCase(
                        Expression.Constant(1L),
                        Expression.Constant(1L)
                    )
                )
            );
            Assert.AreSame(expBslim, res2);
        }

        #endregion

        #region TryCatch

        [TestMethod]
        public void TypeSubstitutionExpressionSlimVisitor_TryCatch_Simple()
        {
            var subst = GetVisitorSimple();

            var expA =
                Expression.TryCatchFinally(
                    Expression.Constant("e1"),
                    Expression.Constant("e2"),
                    Expression.Catch(
                        Expression.Parameter(typeof(string)),
                        Expression.Constant("e3")
                    )
                );
            var expB =
                Expression.TryCatchFinally(
                    Expression.Constant(1),
                    Expression.Constant(2),
                    Expression.Catch(
                        Expression.Parameter(typeof(int)),
                        Expression.Constant(3)
                    )
                );
            var expAslim = expA.ToExpressionSlim();

            var res1 = subst.Apply(expAslim);
            var res2 = subst.Apply(expB.ToExpressionSlim());

            ReduceAndAssertEqual(
                res2,
                Expression.TryCatchFinally(
                    Expression.Constant(1L),
                    Expression.Constant(2L),
                    Expression.Catch(
                        Expression.Parameter(typeof(long)),
                        Expression.Constant(3L)
                    )
                )
            );
            Assert.AreSame(expAslim, res1);
        }

        #endregion

        #region TypeBinary

        [TestMethod]
        public void TypeSubstitutionExpressionSlimVisitor_TypeBinary_Simple()
        {
            var subst = GetVisitorSimple();

            var expA = Expression.TypeIs(Expression.Constant("foo"), typeof(int));
            var expB = Expression.TypeIs(Expression.Constant("foo"), typeof(string));
            var expBslim = expB.ToExpressionSlim();
            var expC = Expression.TypeIs(Expression.Constant(12345), typeof(int));
            var expD = Expression.TypeIs(Expression.Constant(12345), typeof(string));

            var res1 = subst.Apply(expA.ToExpressionSlim());
            var res2 = subst.Apply(expBslim);
            var res3 = subst.Apply(expC.ToExpressionSlim());
            var res4 = subst.Apply(expD.ToExpressionSlim());

            ReduceAndAssertEqual(res1, Expression.TypeIs(Expression.Constant("foo"), typeof(long)));
            Assert.AreSame(expBslim, res2);
            ReduceAndAssertEqual(res3, Expression.TypeIs(Expression.Constant(12345L), typeof(long)));
            ReduceAndAssertEqual(res4, Expression.TypeIs(Expression.Constant(12345L), typeof(string)));
        }

        [TestMethod]
        public void TypeSubstitutionExpressionSlimVisitor_TypeBinary_TypeEqual()
        {
            var subst = GetVisitorSimple();

            var expA = Expression.TypeEqual(Expression.Parameter(typeof(Type)), typeof(int));
            var expB = Expression.TypeEqual(Expression.Constant(typeof(Type)), typeof(string));
            var expBslim = expB.ToExpressionSlim();

            var res1 = subst.Apply(expA.ToExpressionSlim());
            var res2 = subst.Apply(expBslim);

            ReduceAndAssertEqual(res1, Expression.TypeEqual(Expression.Parameter(typeof(Type)), typeof(long)));
            Assert.AreSame(expBslim, res2);
        }

        #endregion

        #region Unary

        [TestMethod]
        public void TypeSubstitutionExpressionSlimVisitor_Unary_Simple()
        {
            var subst = GetVisitorSimple();

            var expA = Expression.Negate(Expression.Constant(1));
            var expB = Expression.Not(Expression.Constant(false));
            var expBslim = expB.ToExpressionSlim();

            var res1 = subst.Apply(expA.ToExpressionSlim());
            var res2 = subst.Apply(expBslim);

            ReduceAndAssertEqual(res1, Expression.Negate(Expression.Constant(1L)));
            Assert.AreSame(expBslim, res2);
        }

        [TestMethod]
        public void TypeSubstitutionExpressionSlimVisitor_Unary_CustomOperator()
        {
            var subst = GetVisitorSimple();

            var expA = Expression.Negate(Expression.Parameter(typeof(TestBar)));

            var res1 = subst.Apply(expA.ToExpressionSlim());

            ReduceAndAssertEqual(res1, Expression.Negate(Expression.Parameter(typeof(TestFoo))));
        }

        [TestMethod]
        public void TypeSubstitutionExpressionSlimVisitor_Unary_Convert()
        {
            var subst = GetVisitorSimple();

            var expA = Expression.Convert(Expression.Parameter(typeof(object)), typeof(int));
            var expB = Expression.Convert(Expression.Parameter(typeof(object)), typeof(string));
            var expBslim = expB.ToExpressionSlim();

            var res1 = subst.Apply(expA.ToExpressionSlim());
            var res2 = subst.Apply(expBslim);

            ReduceAndAssertEqual(res1, Expression.Convert(Expression.Parameter(typeof(object)), typeof(long)));
            Assert.AreSame(expBslim, res2);
        }

        #endregion

        #region E2E

        [TestMethod]
        public void TypeSubstitutionExpressionSlimVisitor_Anonymize()
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
        public void TypeSubstitutionExpressionSlimVisitor_ChangeInterface()
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
        public void TypeSubstitutionExpressionSlimVisitor_StubbornTypeChange_Fail()
        {
            Expression<Func<string, int>> f = s => Math.Abs(s.Length);

            var subst = new MyTypeSubstitutionExpressionVisitor();

            Assert.ThrowsException<InvalidOperationException>(() => subst.Apply(f.ToExpressionSlim()).ToExpression());
        }

        [TestMethod]
        public void TypeSubstitutionExpressionSlimVisitor_StubbornTypeChange_NotQuite()
        {
            Expression<Func<string, int>> f = s => Math.Abs(s.Length);

            var subst = new NotQuiteForgivingTypeSubstitutionExpressionVisitor();

            Assert.ThrowsException<InvalidOperationException>(() => subst.Apply(f.ToExpressionSlim()));
        }

        private sealed class NotQuiteForgivingTypeSubstitutionExpressionVisitor : MyTypeSubstitutionExpressionVisitor
        {
            protected override MemberInfoSlim ResolveProperty(PropertyInfoSlim originalProperty, TypeSlim declaringType, TypeSlim propertyType, TypeSlim[] indexerParameters)
            {
                if (TypeSlimExtensions.Equals(originalProperty.DeclaringType, typeof(string).ToTypeSlim()) && originalProperty.Name == "Length")
                {
                    return originalProperty;
                }

                return base.ResolveProperty(originalProperty, declaringType, propertyType, indexerParameters);
            }
        }

        [TestMethod]
        public void TypeSubstitutionExpressionSlimVisitor_StubbornTypeChange_Resolve()
        {
            Expression<Func<string, int>> f = s => Math.Abs(s.Length);

            var subst = new ForgivingTypeSubstitutionExpressionVisitor();

            var res = subst.Apply(f.ToExpressionSlim());

            Expression<Func<string, long>> g = s => Math.Abs((long)s.Length);

            ReduceAndAssertEqual(res, g);
        }

        private sealed class ForgivingTypeSubstitutionExpressionVisitor : MyTypeSubstitutionExpressionVisitor
        {
            protected override MemberInfoSlim ResolveProperty(PropertyInfoSlim originalProperty, TypeSlim declaringType, TypeSlim propertyType, TypeSlim[] indexerParameters)
            {
                if (TypeSlimExtensions.Equals(originalProperty.DeclaringType, typeof(string).ToTypeSlim()) && originalProperty.Name == "Length")
                {
                    return originalProperty;
                }

                return base.ResolveProperty(originalProperty, declaringType, propertyType, indexerParameters);
            }

            protected override ExpressionSlim ConvertExpression(ExpressionSlim originalExpression, ExpressionSlim resultExpression, TypeSlim newType)
            {
                return ExpressionSlim.Convert(resultExpression, newType);
            }
        }

        [TestMethod]
        public void TypeSubstitutionExpressionSlimVisitor_PrivateReflection()
        {
            Expression<Func<int, int>> f = x => _Bar(x);

            var subst = new MyTypeSubstitutionExpressionVisitor();

            var res = subst.Apply(f.ToExpressionSlim());

            Expression<Func<long, long>> g = x => _Bar(x);

            ReduceAndAssertEqual(res, g);
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
        public void TypeSubstitutionExpressionSlimVisitor_ConstantChange_Fail()
        {
            Expression<Func<int>> f = () => 42;

            var subst = new TypeSubstitutionExpressionVisitor(new Dictionary<Type, Type>
            {
                { typeof(int), typeof(long) }
            });

            Assert.ThrowsException<InvalidOperationException>(() => subst.Apply(f));
        }

        [TestMethod]
        public void TypeSubstitutionExpressionSlimVisitor_ConstantChange_Resolve()
        {
            Expression<Func<int>> f = () => 42;

            var subst = new ConstantChangingTypeSubstitutionExpressionVisitor();

            var res = subst.Apply(f.ToExpressionSlim());

            Expression<Func<long>> g = () => 42L;

            ReduceAndAssertEqual(res, g);
        }

        private sealed class ConstantChangingTypeSubstitutionExpressionVisitor : MyTypeSubstitutionExpressionVisitor
        {
        }

        #endregion

        #region Private implementation

        private static void ReduceAndAssertEqual(ExpressionSlim slimA, Expression b)
        {
            var a = slimA.ToExpression();

            var fva = FreeVariableScanner.Scan(a);
            var fvb = FreeVariableScanner.Scan(b);

            var la = Expression.Lambda(a, fva);
            var lb = Expression.Lambda(b, fvb);

            Assert.IsTrue(new ExpressionEqualityComparer(() => new Comparator(new StructuralTypeEqualityComparator())).Equals(la, lb));
        }

        private sealed class Comparator : ExpressionEqualityComparator
        {
            public Comparator(StructuralTypeEqualityComparator typeComparer)
                : base(typeComparer, typeComparer.MemberComparer, EqualityComparer<object>.Default, EqualityComparer<CallSiteBinder>.Default)
            {
            }

            public override bool Equals(Expression x, Expression y)
            {
                return base.Equals(x, y);
            }
        }

        private static TypeSubstitutionExpressionSlimVisitor GetVisitorSimple()
        {
            return new MyTypeSubstitutionExpressionVisitor();
        }

        private class MyTypeSubstitutionExpressionVisitor : TypeSubstitutionExpressionSlimVisitor
        {
            public MyTypeSubstitutionExpressionVisitor()
                : base(new Dictionary<TypeSlim, TypeSlim>
                {
                    { typeof(int).ToTypeSlim(), typeof(long).ToTypeSlim() },
                    { typeof(TestBar).ToTypeSlim(), typeof(TestFoo).ToTypeSlim() },
                    { typeof(Banana).ToTypeSlim(), typeof(Fruit).ToTypeSlim() }
                })
            {
            }

            protected override ObjectSlim ConvertConstant(ObjectSlim originalValue, TypeSlim newType)
            {
                if (TypeSlimExtensions.Equals(originalValue.TypeSlim, TypeSlimExtensions.IntegerType) && TypeSlimExtensions.Equals(newType, typeof(long).ToTypeSlim()))
                {
                    return ObjectSlim.Create((long)(int)originalValue.Value, typeof(long).ToTypeSlim(), typeof(long));
                }

                return base.ConvertConstant(originalValue, newType);
            }
        }

#pragma warning disable 0649
#pragma warning disable IDE0060 // Remove unused parameter
        private sealed class TestBar
        {
            public TestBar(int x)
            {
            }

            public int this[int x] => throw new NotImplementedException();

            public static string Baz;
            public int Qux { get; set; }
            public TestBar Joey { get; private set; }
            public List<int> Xs { get; private set; }
            public string Stubborn { get; set; }
            public List<string> Ys { get; private set; }

            public int Good1;
            public int Wrong2;
            public int Wrong3;

            public static TestBar operator +(TestBar b1, int x) => throw new NotImplementedException();

            public static TestBar operator +(TestBar b1, string x) => throw new NotImplementedException();

            public static TestBar operator -(TestBar b) => throw new NotImplementedException();
        }

        private sealed class TestFoo
        {
            public TestFoo(long x)
            {
            }

            public static string Baz;
            public long Qux { get; set; }
            public TestFoo Joey { get; private set; }
            public List<long> Xs { get; private set; }
            public string Stubborn { get; set; }
            public List<string> Ys { get; private set; }

            public long Good1;
            public string Wrong2;
            public long Verkeerd3;

            public static TestFoo operator +(TestFoo f1, long x) => throw new NotImplementedException();

            public static TestFoo operator +(TestFoo f1, string x) => throw new NotImplementedException();

            public static TestFoo operator -(TestFoo b) => throw new NotImplementedException();
        }

#pragma warning disable CA1822 // Mark static
        private sealed class TestBaz
        {
            public TestBaz(int x)
            {
            }

            public int x;
            public int Y { get; private set; }
            public int Z(int x) { return 42; }
            public int A<T>(int x) { return x; }
        }
#pragma warning restore CA1822

        private sealed class Person
        {
            public string Name { get; set; }
            public int Age { get; set; }
        }
#pragma warning restore 0649
#pragma warning restore IDE0060 // Remove unused parameter

        #endregion
    }
}
