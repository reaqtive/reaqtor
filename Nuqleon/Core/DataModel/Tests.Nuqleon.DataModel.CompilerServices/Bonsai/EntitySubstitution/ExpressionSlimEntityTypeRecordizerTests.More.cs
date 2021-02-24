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
using System.Linq.CompilerServices;
using System.Linq.Expressions;
using System.Reflection;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Nuqleon.DataModel;
using Nuqleon.DataModel.CompilerServices;

#if NETFRAMEWORK
using System.IO;
using System.Reflection.Emit;
using Nuqleon.DataModel.CompilerServices.Bonsai;
#endif

namespace Tests.Nuqleon.DataModel.CompilerServices
{
    public partial class ExpressionSlimEntityTypeRecordizerTests
    {
        #region Helper Classes

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

        private class Qux3
        {
            [Mapping("contoso://x")]
            public double X = 0;
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

        public class EnumContainer
        {
            [Mapping("enum")]
            public TestEnum Bar { get; set; }
        }

        public enum TestEnum
        {
            [Mapping("foo")]
            Foo
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

        #endregion

        #region Test Methods

        #region Success Tests

        [TestMethod]
        public void RecordizeSlim_FromNewExpression_Success()
        {
            var exp = (Expression<Func<SimplePropertyTest>>)(() => new SimplePropertyTest(1));

            AssertRecordizationFromExpression(exp, expr => ((LambdaExpression)expr).ReturnType);
        }

        [TestMethod]
        public void RecordizeSlim_FromMemberInitExpressionWithDefaultConstructor_Success()
        {
            var exp = (Expression<Func<SimplePropertyTest>>)(() => new SimplePropertyTest { Foo = 1 });

            AssertRecordizationFromExpression(exp, expr => ((LambdaExpression)expr).ReturnType);
        }

        [TestMethod]
        public void RecordizeSlim_FromMemberInitExpressionWithNonDefaultConstructor_Success()
        {
            var exp = (Expression<Func<SimplePropertyTest>>)(() => new SimplePropertyTest(1) { Bar = 1.0 });

            AssertRecordizationFromExpression(exp, expr => ((LambdaExpression)expr).ReturnType);
        }

        [TestMethod]
        public void RecordizeSlim_FromMemberInitExpressionWithMemberMemberBinding_Success()
        {
            var exp = (Expression<Func<FooSlim>>)(() => new FooSlim { Qux = { X = 1.0 } });

            AssertRecordizationFromExpression(exp, expr => ((LambdaExpression)expr).ReturnType);
        }

        [TestMethod]
        public void RecordizeSlim_UsingGetPropertyMethod_Success()
        {
            var exp = (Expression<Func<Ocean, Planet>>)(ocean => new Planet { OceanDepth = ocean.Depth });

            static void assert(Expression expr) => Assert.IsTrue(expr is LambdaExpression lambda && lambda.ReturnType.IsRecordType() && lambda.Parameters[0].Type.IsRecordType());
            AssertRecordizationFromExpression(exp, assert, new Ocean { Depth = 10 });
        }

        [TestMethod]
        public void RecordizeSlim_ListBindingUsed_Success()
        {
            var exp = (Expression<Func<ListPropertyTest>>)(() => new ListPropertyTest { List = { 1, 2, 3 } });

            AssertRecordizationFromExpression(exp, expr => ((LambdaExpression)expr).ReturnType);
        }

        [TestMethod]
        public void RecordizeSlim_ListBindingOfRecordType_Success()
        {
            var exp = (Expression<Func<Star>>)(() => new Star(new List<int> { 1, 2, 3 }) { Planets = { new Planet(10) } });

            AssertRecordizationFromExpression(exp, expr => ((LambdaExpression)expr).ReturnType);
        }

        [TestMethod]
        public void RecordizeSlim_WithConstantRecordType_Success()
        {
            var obj = new SimplePropertyTest(1);

            AssertRecordizationFromConstant(obj, expr => expr.Type);
        }

        [TestMethod]
        public void RecordizeSlim_WithConstantArrayType_Success()
        {
            var obj = new SimplePropertyTest[] { new SimplePropertyTest(1) };

            static Type getRecord(Expression expr) => expr.Evaluate().GetType().GetElementType();
            AssertRecordizationFromConstant(obj, getRecord);
        }

        [TestMethod]
        public void RecordizeSlim_WithConstantEnumerableType_Success()
        {
            var obj = new List<SimplePropertyTest> { new SimplePropertyTest(1) };

            static Type getRecord(Expression expr) => expr.Evaluate().GetType().GetGenericArguments()[0];
            AssertRecordizationFromConstant(obj, getRecord);
        }

        [TestMethod]
        [Ignore, Description("TODO: enable dictionaries in DataType, EqualityChecker, etc.")]
        public void RecordizeSlim_WithConstantDictionaryType_Success()
        {
            var obj = new Dictionary<string, Foo2>
            {
                { "bar", new Foo2("bar") }
            };

            AssertRecordizationFromConstant(obj, expr => expr.Type);
        }

        [TestMethod]
        [Ignore, Description("Serialization layer does not support recursion, these tests require serialization to work.")]
        public void RecordizeSlim_WithConstantRecordTypeRecursiveReference_Success()
        {
            var obj = new RecursivePropertyTest();
            obj.Self = obj;

            AssertRecordizationFromConstant(obj, expr => expr.Type);
        }

        [TestMethod]
        public void RecordizeSlim_WithNewExpressionAndUnchangedType_Success()
        {
            var exp = (Expression<Func<UnchangedNewPropertyTest>>)(() => new UnchangedNewPropertyTest { Foo = new string("bar".ToArray()) });

            AssertRecordizationFromExpression(exp, expr => ((LambdaExpression)expr).ReturnType);
        }

        [TestMethod]
        public void RecordizeSlim_MemberMemberBindingWithNewList_Success()
        {
            var exp = (Expression<Func<MemberMemberListPropertyTest>>)(() => new MemberMemberListPropertyTest { Container = { List = new List<SimplePropertyTest> { new SimplePropertyTest(1) } } });

            AssertRecordizationFromExpression(exp, expr => ((LambdaExpression)expr).ReturnType);
        }

        [TestMethod]
        public void RecordizeSlim_MemberMemberBindingWithExistingList_Success()
        {
            var exp = (Expression<Func<List<SimplePropertyTest>, MemberMemberListPropertyTest>>)(list => new MemberMemberListPropertyTest { Container = { List = list } });

            AssertRecordizationFromExpression(exp, expr => ((LambdaExpression)expr).ReturnType, new List<SimplePropertyTest> { new SimplePropertyTest(1) });
        }

        [TestMethod]
        public void RecordizeSlim_MemberAccessEnum()
        {
            Expression<Func<EnumContainer, TestEnum>> exp = box => box.Bar;

            AssertRecordizationFromExpression(exp, expr => expr.Type.GenericTypeArguments[0], new EnumContainer { Bar = TestEnum.Foo });
        }

        [TestMethod]
        public void RecordizeSlim_NewExpressionWithMemberMappedArguments()
        {
            Expression exp = Expression.Lambda(Expression.New(
                (ConstructorInfo)ReflectionHelpers.InfoOf(() => new SimplePropertyTest(0)),
                new Expression[] { Expression.Constant(42) },
                ReflectionHelpers.InfoOf((SimplePropertyTest test) => test.Foo)));

            AssertRecordizationFromExpression(exp, expr => ((LambdaExpression)expr).ReturnType);
        }

        [TestMethod]
        public void RecordizeSlim_KnownTypesIgnored()
        {
            Expression<Func<KnownDMType>> exp = () => new KnownDMType();
            var result = Roundtrip(exp);
            Assert.AreEqual(typeof(Func<KnownDMType>), result.Type);
        }

        [TestMethod]
        public void RecordizeSlim_PartialKnownTypes()
        {
            Expression<Func<KnownDMType>> exp = () => new PartialKnownType { KnownProp = new KnownDMType() }.KnownProp;
            var result = Roundtrip(exp);
            Assert.AreEqual(typeof(Func<KnownDMType>), result.Type);
            Assert.IsTrue(((MemberExpression)((LambdaExpression)result).Body).Expression.Type.IsRecordType());
        }

        [TestMethod]
        public void RecordizeSlim_NonInferrableType()
        {
#pragma warning disable IDE0004 // Remove Unnecessary Cast. (Keeping in expression tree.)

            Expression<Func<SimplePropertyTest, string>> expr = spt => "Hi" + (object)spt.Foo;
            var result = Roundtrip(expr);
            Assert.IsTrue(((LambdaExpression)result).Parameters[0].Type.IsRecordType());

#pragma warning restore IDE0004
        }

        #endregion

        #region Exception Tests

        [TestMethod]
        public void RecordizeSlim_UnmappedPropertyUsed_ThrowsInvalidOperation()
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
        public void RecordizeSlim_UnmappedConstructorArgumentUsed_ThrowsInvalidOperation()
        {
            var exp = (Expression<Func<string, int, Foo2>>)((bar, baz) => new Foo2(bar, baz));
            AssertRecordizationException<InvalidOperationException>(exp);
        }

        [TestMethod]
        public void RecordizeSlim_ConstructorAndPropertyUsedSameMapping_ThrowsInvalidOperation()
        {
            var exp = (Expression<Func<string, Foo2>>)(bar => new Foo2(bar) { Bar = bar });
            AssertRecordizationException<InvalidOperationException>(exp);
        }

        [TestMethod]
        public void RecordizeSlim_ConstructorAndPropertyUsedSameMappingList_ThrowsInvalidOperation()
        {
            var exp = (Expression<Func<Star>>)(() => new Star(new List<int> { 1, 2, 3 }) { List = { 1, 2, 3 } });
            AssertRecordizationException<InvalidOperationException>(exp);
        }

        [TestMethod]
        public void RecordizeSlim_ConstructorParameterWithoutMatchingPropertyMapping_ThrowsInvalidOperation()
        {
            var exp = (Expression<Func<Moon>>)(() => new Moon(0.1));
            AssertRecordizationException<InvalidOperationException>(exp);
        }

        [TestMethod]
        public void RecordizeSlim_ConstructorParameterMappingUsedMoreThanOnce_ThrowsInvalidOperation()
        {
            var exp = (Expression<Func<Moon>>)(() => new Moon(0.1, 0.1));
            AssertRecordizationException<InvalidOperationException>(exp);
        }

        [TestMethod]
        public void RecordizeSlim_Field()
        {
            var exp = (Expression<Func<Qux3>>)(() => new Qux3 { X = 42, Y = 43 });
            AssertRecordizationFromExpression(exp, e => ((LambdaExpression)e).Body.Type);
        }

        [TestMethod]
        public void RecordizeSlim_UsingMethodCall_ThrowsInvalidOperation()
        {
            var exp = (Expression<Func<Ocean, Planet>>)(ocean => new Planet { OceanDepth = ocean.GetDepth() });
            AssertRecordizationException<InvalidOperationException>(exp);
        }

        #endregion

        #region Regression Tests

#if NETFRAMEWORK
        [TestMethod]
        public void RecordizeSlim_UsingTypesFromLoadedAssembly()
        {
            var assemblyName = "Test.dll";
            var assemblyBase = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            var assemblyPath = Path.Combine(assemblyBase, assemblyName);

            try
            {
                Directory.CreateDirectory(assemblyBase);

                // Emit dynamic assembly.
                var dynamicAssembly = AppDomain.CurrentDomain.DefineDynamicAssembly(new AssemblyName(Path.GetFileNameWithoutExtension(assemblyName)), AssemblyBuilderAccess.RunAndSave, assemblyBase);
                var module = dynamicAssembly.DefineDynamicModule("Test", assemblyName);
                var type = module.DefineType("Test", TypeAttributes.Class | TypeAttributes.Public);
                var prop = type.DefineProperty("Test", PropertyAttributes.None, typeof(int), Type.EmptyTypes);
                var getter = type.DefineMethod("get_Test", MethodAttributes.Public);
                var ilgen = getter.GetILGenerator();
                ilgen.Emit(OpCodes.Ldc_I4_0);
                ilgen.Emit(OpCodes.Ret);
                prop.SetGetMethod(getter);
                prop.SetCustomAttribute(new CustomAttributeBuilder((ConstructorInfo)ReflectionHelpers.InfoOf(() => new MappingAttribute(null)), new object[] { "foo" }));
                var res = type.CreateType();
                dynamicAssembly.Save("Test.dll");

                // Execute test logic in separate AppDomain so binary can be cleaned up.
                var domain = AppDomain.CreateDomain(MethodBase.GetCurrentMethod().Name, null, new AppDomainSetup { ApplicationBase = AppDomain.CurrentDomain.BaseDirectory });
                domain.SetData("AssemblyPath", assemblyPath);
                domain.DoCallBack(() =>
                {
                    var remotedAssemblyPath = (string)AppDomain.CurrentDomain.GetData("AssemblyPath");
                    var assembly = Assembly.LoadFrom(remotedAssemblyPath);
                    var types = assembly.GetTypes();

                    foreach (var t in types)
                    {
                        var expr = Expression.New(t);
                        var lifter = new ExpressionToExpressionSlimConverter(new DataModelTypeSpace());
                        var slimExpr = lifter.Visit(expr);
                        var recordizer = new ExpressionSlimEntityTypeRecordizer();

                        // Test fails if `Apply` throws.
                        recordizer.Apply(slimExpr);
                    }
                });
                AppDomain.Unload(domain);
            }
            finally
            {
                try { if (File.Exists(assemblyPath)) File.Delete(assemblyPath); }
                catch { }
                try { if (Directory.Exists(assemblyBase)) Directory.Delete(assemblyBase); }
                catch { }
            }
        }
#endif

        #endregion

        #endregion

        #region Test Helpers

        private static void AssertRecordizationException<T>(Expression exp)
            where T : Exception
        {
            var etr = new ExpressionEntityTypeRecordizer();
            Assert.ThrowsException<T>(() => Roundtrip(exp));
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
            var res = Roundtrip(exp);
            Assert.IsTrue(new RecordTreeComparator(new TypeComparator()).Equals(exp, res));
            var xFunc = exp.Evaluate();
            var x = xFunc.GetType().GetMethod("Invoke").Invoke(xFunc, args);
            var yFunc = res.Evaluate();
            var yArgs = args.Select(arg => Roundtrip(Expression.Constant(arg)).Evaluate()).ToArray();
            var y = yFunc.GetType().GetMethod("Invoke").Invoke(yFunc, yArgs);
            Assert.IsTrue(ObjectComparator.CreateInstance().Equals(x, y));
            return res;
        }

        private static void AssertRecordizationFromConstant(object obj, Func<Expression, Type> getRecordType)
        {
            var res = AssertRecordizationFromConstantCore(obj);
            Assert.IsTrue(getRecordType(res).IsRecordType());
        }

        private static Expression AssertRecordizationFromConstantCore(object obj)
        {
            var exp = Expression.Constant(obj);
            var res = Roundtrip(exp);
            Assert.IsTrue(ObjectComparator.CreateInstance().Equals(obj, res.Evaluate()));
            return res;
        }

        #endregion
    }
}
