// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER, BD - July 2013 - Created this file.
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Nuqleon.DataModel;
using Nuqleon.DataModel.CompilerServices;

namespace Tests.Nuqleon.DataModel.CompilerServices
{
    public partial class ExpressionEntityTypeRecordizerTests
    {
        #region Test Classes

#pragma warning disable IDE0052 // Remove unread private members (test code)
#pragma warning disable IDE0060 // Remove unused parameter (test code)

        private struct SimpleStruct
        {
            [Mapping("contoso://i")]
            public int i { get; set; }
            [Mapping("contoso://s")]
            public string s { get; set; }
        }

        private class SimplePropertyTest
        {
            public SimplePropertyTest() { }

            public SimplePropertyTest([Mapping("contoso://foo")] int foo)
            {
                Foo = foo;
            }

            [Mapping("contoso://foo")]
            public int Foo { get; set; }

            [Mapping("contoso://bar")]
            public double Bar { get; set; }
        }

        private class ListPropertyTest
        {
            public ListPropertyTest()
            {
                List = new List<int>();
            }

            [Mapping("contoso://list")]
            public List<int> List { get; set; }
        }

        private class RecursivePropertyTest
        {
            [Mapping("contoso://self")]
            public RecursivePropertyTest Self { get; set; }
        }

        private class RecordListPropertyTest
        {
            public RecordListPropertyTest()
            {
                List = new List<SimplePropertyTest>();
            }

            [Mapping("contoso://list")]
            public List<SimplePropertyTest> List { get; set; }
        }

        private class MemberMemberListPropertyTest
        {
            public MemberMemberListPropertyTest()
            {
                Container = new RecordListPropertyTest();
            }

            [Mapping("contoso://listcontainer")]
            public RecordListPropertyTest Container { get; set; }
        }

        private class UnchangedNewPropertyTest
        {
            [Mapping("contoso://foo")]
            public string Foo { get; set; }
        }

        private class Foo2
        {
            public Foo2()
            {
                Qux = new Qux2();
            }

            public Foo2([Mapping("contoso://bar")] string bar)
            {
                Bar = bar;
                Qux = new Qux2();
            }

            public Foo2([Mapping("contoso://bar")] string bar, int baz)
            {
                Bar = bar;
                //Baz = baz;
                Qux = new Qux2();
            }

            public Foo2([Mapping("contoso://myfoo")] Foo2 myFoo)
            {
                myFoo.MyFoo = myFoo;
                MyFoo = myFoo;
                Qux = new Qux2();
            }

            [Mapping("contoso://bar")]
            public string Bar { get; set; }

            //public int Baz { get; set; }

            [Mapping("contoso://qux")]
            public Qux2 Qux { get; set; }

            [Mapping("contoso://myfoo")]
            public Foo2 MyFoo { get; set; }
        }

        private class FooSlim
        {
            public FooSlim()
            {
                Qux = new Qux2();
            }

            [Mapping("contoso://qux")]
            public Qux2 Qux { get; set; }
        }

        private class Qux2
        {
            [Mapping("contoso://x")]
            public double X { get; set; }
            [Mapping("contoso://y")]
            public double Y { get; set; }
        }

        private class Star
        {
            public Star()
            {
                List = new List<int>();
                Planets = new List<Planet>();
            }

            public Star([Mapping("contoso://list")] List<int> list)
            {
                List = list;
                Planets = new List<Planet>();
            }

            [Mapping("contoso://list")]
            public List<int> List { get; set; }

            [Mapping("contoso://planets")]
            public List<Planet> Planets { get; set; }
        }

        private class Planet
        {
            public Planet() { }

            public Planet([Mapping("contoso://oceandepth")] int depth)
            {
                OceanDepth = depth;
            }

            [Mapping("contoso://oceandepth")]
            public int OceanDepth { get; set; }
        }

        private class Ocean
        {
            [Mapping("contoso://depth")]
            public int Depth { get; set; }

            public int GetDepth()
            {
                return Depth;
            }
        }

        private class Moon
        {
            private readonly double _size;

            public Moon([Mapping("contoso://size")] double size)
            {
                _size = size;
            }

            [Mapping("contoso://glow")]
            public double Glow { get; set; }

            public Moon([Mapping("contoso://glow")] double glow1, [Mapping("contoso://glow")] double glow2)
            {
                Glow = glow1;
            }
        }

        [KnownType]
        public class KnownDMType
        {
            [Mapping("foo")]
            public int Foo { get; set; }
        }

        public class PartialKnownType
        {
            [Mapping("KnownProp")]
            public KnownDMType KnownProp
            {
                get;
                set;
            }
        }

#pragma warning restore IDE0060 // Remove unused parameter
#pragma warning restore IDE0052 // Remove unread private members

        #endregion

        #region Test Methods

        #region Success Tests

        [TestMethod]
        public void Recordize_FromNewExpression_Success()
        {
            var exp = (Expression<Func<SimplePropertyTest>>)(() => new SimplePropertyTest(1));

            static Type getRecord(Expression expr) => (expr as LambdaExpression).ReturnType;
            AssertRecordizationFromExpression(exp, getRecord);
        }

        [TestMethod]
        public void Recordize_FromMemberInitExpressionWithDefaultConstructor_Success()
        {
            var exp = (Expression<Func<SimplePropertyTest>>)(() => new SimplePropertyTest { Foo = 1 });

            static Type getRecord(Expression expr) => (expr as LambdaExpression).ReturnType;
            AssertRecordizationFromExpression(exp, getRecord);
        }

        [TestMethod]
        public void Recordize_FromMemberInitExpressionWithNonDefaultConstructor_Success()
        {
            var exp = (Expression<Func<SimplePropertyTest>>)(() => new SimplePropertyTest(1) { Bar = 1.0 });

            static Type getRecord(Expression expr) => (expr as LambdaExpression).ReturnType;
            AssertRecordizationFromExpression(exp, getRecord);
        }

        [TestMethod]
        public void Recordize_FromMemberInitExpressionWithMemberMemberBinding_Success()
        {
            var exp = (Expression<Func<FooSlim>>)(() => new FooSlim { Qux = { X = 1.0 } });

            static Type getRecord(Expression expr) => (expr as LambdaExpression).ReturnType;
            AssertRecordizationFromExpression(exp, getRecord);
        }

        [TestMethod]
        public void Recordize_UsingGetPropertyMethod_Success()
        {
            var exp = (Expression<Func<Ocean, Planet>>)(ocean => new Planet { OceanDepth = ocean.Depth });

            static void assert(Expression expr) => Assert.IsTrue((expr as LambdaExpression).ReturnType.IsRecordType() && (expr as LambdaExpression).Parameters[0].Type.IsRecordType());
            AssertRecordizationFromExpression(exp, assert, new Ocean { Depth = 10 });
        }

        [TestMethod]
        public void Recordize_ListBindingUsed_Success()
        {
            var exp = (Expression<Func<ListPropertyTest>>)(() => new ListPropertyTest { List = { 1, 2, 3 } });

            static Type getRecord(Expression expr) => (expr as LambdaExpression).ReturnType;
            AssertRecordizationFromExpression(exp, getRecord);
        }

        [TestMethod]
        public void Recordize_ListBindingOfRecordType_Success()
        {
            var exp = (Expression<Func<Star>>)(() => new Star(new List<int> { 1, 2, 3 }) { Planets = { new Planet(10) } });

            static Type getRecord(Expression expr) => (expr as LambdaExpression).ReturnType;
            AssertRecordizationFromExpression(exp, getRecord);
        }

        [TestMethod]
        public void Recordize_WithConstantRecordType_Success()
        {
            var obj = new SimplePropertyTest(1);

            static Type getRecord(Expression expr) => expr.Type;
            AssertRecordizationFromConstant(obj, getRecord);
        }

        [TestMethod]
        public void Recordize_WithConstantArrayType_Success()
        {
            var obj = new SimplePropertyTest[] { new(1) };

            static Type getRecord(Expression expr) => expr.Evaluate().GetType().GetElementType();
            AssertRecordizationFromConstant(obj, getRecord);
        }

        [TestMethod]
        public void Recordize_WithConstantEnumerableType_Success()
        {
            var obj = new List<SimplePropertyTest> { new(1) };

            static Type getRecord(Expression expr) => expr.Evaluate().GetType().GetGenericArguments()[0];
            AssertRecordizationFromConstant(obj, getRecord);
        }

        [TestMethod]
        [Ignore, Description("TODO: enable dictionaries in DataType, EqualityChecker, etc.")]
        public void Recordize_WithConstantDictionaryType_Success()
        {
            var obj = new Dictionary<string, Foo2>
            {
                { "bar", new Foo2("bar") }
            };

            static Type getRecord(Expression expr) => expr.Type;
            AssertRecordizationFromConstant(obj, getRecord);
        }

        [TestMethod]
        public void Recordize_WithConstantRecordTypeRecursiveReference_Success()
        {
            var obj = new RecursivePropertyTest();
            obj.Self = obj;

            static Type getRecord(Expression expr) => expr.Type;
            AssertRecordizationFromConstant(obj, getRecord);
        }

        [TestMethod]
        public void Recordize_WithNewExpressionAndUnchangedType_Success()
        {
            var exp = (Expression<Func<UnchangedNewPropertyTest>>)(() => new UnchangedNewPropertyTest { Foo = new string("bar".ToArray()) });

            static Type getRecord(Expression expr) => (expr as LambdaExpression).ReturnType;
            AssertRecordizationFromExpression(exp, getRecord);
        }

        [TestMethod]
        public void Recordize_MemberMemberBindingWithNewList_Success()
        {
            var exp = (Expression<Func<MemberMemberListPropertyTest>>)(() => new MemberMemberListPropertyTest { Container = { List = new List<SimplePropertyTest> { new(1) } } });

            static Type getRecord(Expression expr) => (expr as LambdaExpression).ReturnType;
            AssertRecordizationFromExpression(exp, getRecord);
        }

        [TestMethod]
        public void Recordize_MemberMemberBindingWithExistingList_Success()
        {
            var exp = (Expression<Func<List<SimplePropertyTest>, MemberMemberListPropertyTest>>)(list => new MemberMemberListPropertyTest { Container = { List = list } });

            static Type getRecord(Expression expr) => (expr as LambdaExpression).ReturnType;
            AssertRecordizationFromExpression(exp, getRecord, new List<SimplePropertyTest> { new(1) });
        }

        [TestMethod]
        public void Recordize_PartialKnownTypes()
        {
            Expression<Func<KnownDMType>> exp = () => new PartialKnownType { KnownProp = new KnownDMType() }.KnownProp;
            AssertRecordizationFromExpression(exp);
        }

        /* Currently not rejected
        [TestMethod]
        public void Recordize_Struct_ThrowsInvalidOperation()
        {
            var obj = new SimpleStruct { i = 1, s = "foo" };
            AssertRecordizationException<InvalidOperationException>(obj);
        }
         */

        #endregion

        #region Exception Tests

        [TestMethod]
        public void Recordize_NullExpression_ThrowsArgumentNull()
        {
            var etr = new ExpressionEntityTypeRecordizer();
            AssertRecordizationException<ArgumentNullException>(exp: null, ex => ex.ParamName.Equals("expression"));
        }

        [TestMethod]
        public void Recordize_UnmappedPropertyUsed_ThrowsInvalidOperation()
        {
            var exp = (Expression<Func<string, int, Foo3>>)((bar, baz) => new Foo3(bar) { Baz = baz });
            AssertRecordizationException<InvalidOperationException>(exp);
        }

        private class Foo3
        {
            public Foo3([Mapping("bar")] string bar)
            {
                Bar = bar;
            }

            [Mapping("bar")]
            public string Bar { get; set; }

            public int Baz { get; set; }
        }

        [TestMethod]
        public void Recordize_UnmappedConstructorArgumentUsed_ThrowsInvalidOperation()
        {
            var exp = (Expression<Func<string, int, Foo2>>)((bar, baz) => new Foo2(bar, baz));
            AssertRecordizationException<InvalidOperationException>(exp);
        }

        [TestMethod]
        public void Recordize_ConstructorAndPropertyUsedSameMapping_ThrowsInvalidOperation()
        {
            var exp = (Expression<Func<string, Foo2>>)(bar => new Foo2(bar) { Bar = bar });
            AssertRecordizationException<InvalidOperationException>(exp);
        }

        [TestMethod]
        public void Recordize_ConstructorAndPropertyUsedSameMappingList_ThrowsInvalidOperation()
        {
            var exp = (Expression<Func<Star>>)(() => new Star(new List<int> { 1, 2, 3 }) { List = { 1, 2, 3 } });
            AssertRecordizationException<InvalidOperationException>(exp);
        }

        [TestMethod]
        public void Recordize_ConstructorParameterWithoutMatchingPropertyMapping_ThrowsInvalidOperation()
        {
            var exp = (Expression<Func<Moon>>)(() => new Moon(0.1));
            AssertRecordizationException<InvalidOperationException>(exp);
        }

        [TestMethod]
        public void Recordize_ConstructorParameterMappingUsedMoreThanOnce_ThrowsInvalidOperation()
        {
            var exp = (Expression<Func<Moon>>)(() => new Moon(0.1, 0.1));
            AssertRecordizationException<InvalidOperationException>(exp);
        }

        [TestMethod]
        public void Recordize_UsingMethodCall_ThrowsInvalidOperation()
        {
            var exp = (Expression<Func<Ocean, Planet>>)(ocean => new Planet { OceanDepth = ocean.GetDepth() });
            AssertRecordizationException<InvalidOperationException>(exp);
        }

        #endregion

        #endregion

        #region Test Helpers

        private static void AssertRecordizationException<T>(Expression exp)
            where T : Exception
        {
            var etr = new ExpressionEntityTypeRecordizer();
            Assert.ThrowsException<T>(() => etr.Apply(exp));
        }

        private static void AssertRecordizationException<T>(object obj)
            where T : Exception
        {
            var exp = Expression.Constant(obj);
            AssertRecordizationException<T>(exp);
        }

        private static void AssertRecordizationException<T>(Expression exp, Action<T> assert)
            where T : Exception
        {
            var etr = new ExpressionEntityTypeRecordizer();
            AssertEx.ThrowsException<T>(() => etr.Apply(exp), assert);
        }

        private static void AssertRecordizationException<T>(object obj, Action<T> assert)
            where T : Exception
        {
            var exp = Expression.Constant(obj);
            AssertRecordizationException<T>(exp, assert);
        }

        private static void AssertRecordizationFromExpression(Expression exp, params object[] args)
        {
            AssertRecordizationFromExpressionCore(exp, args);
        }

        private static void AssertRecordizationFromExpression(Expression exp, Func<Expression, Type> getRecordType, params object[] args)
        {
            var res = AssertRecordizationFromExpressionCore(exp, args);
            Assert.IsTrue(getRecordType(res).IsRecordType());
        }

        private static void AssertRecordizationFromExpression(Expression exp, Action<Expression> assert, params object[] args)
        {
            var res = AssertRecordizationFromExpressionCore(exp, args);
            assert(res);
        }

        private static Expression AssertRecordizationFromExpressionCore(Expression exp, params object[] args)
        {
            var etr = new ExpressionEntityTypeRecordizer();
            var res = etr.Apply(exp);
            Assert.IsTrue(new RecordTreeComparator(new TypeComparator()).Equals(exp, res));
            var xFunc = exp.Evaluate();
            var x = xFunc.GetType().GetMethod("Invoke").Invoke(xFunc, args);
            var yFunc = res.Evaluate();
            var yArgs = args.Select(arg => etr.Apply(Expression.Constant(arg)).Evaluate()).ToArray();
            var y = yFunc.GetType().GetMethod("Invoke").Invoke(yFunc, yArgs);
            Assert.IsTrue(ObjectComparator.CreateInstance().Equals(x, y));
            return res;
        }

        private static void AssertRecordizationFromConstant(object obj)
        {
            AssertRecordizationFromConstantCore(obj);
        }

        private static void AssertRecordizationFromConstant(object obj, Func<Expression, Type> getRecordType)
        {
            var res = AssertRecordizationFromConstantCore(obj);
            Assert.IsTrue(getRecordType(res).IsRecordType());
        }

        private static void AssertRecordizationFromConstant(object obj, Action<Expression> assert)
        {
            var res = AssertRecordizationFromConstantCore(obj);
            assert(res);
        }

        private static Expression AssertRecordizationFromConstantCore(object obj)
        {
            var etr = new ExpressionEntityTypeRecordizer();
            var exp = Expression.Constant(obj);
            var res = etr.Apply(exp);
            Assert.IsTrue(ObjectComparator.CreateInstance().Equals(obj, res.Evaluate()));
            return res;
        }

        #endregion
    }
}
