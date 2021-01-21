// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - July 2013 - Created this file.
//

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Reflection;
using System.Collections.ObjectModel;
using System.Linq.CompilerServices;
using Tests.System.Linq.Expressions.Bonsai;

namespace Tests.System.Reflection
{
    [TestClass]
    public class MemberInfoSlimVisitorTests : TestBase
    {
        [TestMethod]
        public void MemberInfoSlimVisitor_Visit_NullArguments()
        {
            var visitor = new MemberInfoSlimVisitor();
            AssertEx.ThrowsException<ArgumentNullException>(() => visitor.Visit(member: null), ex => Assert.AreEqual("member", ex.ParamName));
        }

        [TestMethod]
        public void MemberInfoSlimVisitor_Generic_Visit_NullArguments()
        {
            var visitor = new MyVisitor();
            AssertEx.ThrowsException<ArgumentNullException>(() => visitor.Visit(member: null), ex => Assert.AreEqual("member", ex.ParamName));
        }

        [TestMethod]
        public void MemberInfoSlimVisitor_VisitField_AreSame()
        {
            var visitor = new MemberInfoSlimVisitor();
            var field = SlimType.GetField("name", fieldType: null);
            Assert.AreSame(field, visitor.Visit(field));
        }

        [TestMethod]
        public void MemberInfoSlimVisitor_VisitProperty_AreSame()
        {
            var visitor = new MemberInfoSlimVisitor();
            var property = SlimType.GetProperty("Foo", propertyType: null, EmptyReadOnlyCollection<TypeSlim>.Instance, canWrite: true);
            Assert.AreSame(property, visitor.Visit(property));
        }

        [TestMethod]
        public void MemberInfoSlimVisitor_VisitConstructor_AreSame()
        {
            var visitor = new MemberInfoSlimVisitor();
            var constructor = SlimType.GetConstructor(EmptyReadOnlyCollection<TypeSlim>.Instance);
            Assert.AreSame(constructor, visitor.Visit(constructor));
        }

        [TestMethod]
        public void MemberInfoSlimVisitor_VisitSimpleMethod_AreSame()
        {
            var visitor = new MemberInfoSlimVisitor();
            var simple = SlimType.GetSimpleMethod("Foo", EmptyReadOnlyCollection<TypeSlim>.Instance, returnType: null);
            Assert.AreSame(simple, visitor.Visit(simple));
        }

        [TestMethod]
        public void MemberInfoSlimVisitor_VisitGenericMethod_AreSame()
        {
            var visitor = new MemberInfoSlimVisitor();
            var genDef = SlimType.GetGenericDefinitionMethod("Foo", new TypeSlim[] { TypeSlim.GenericParameter("T") }.ToReadOnly(), EmptyReadOnlyCollection<TypeSlim>.Instance, returnType: null);
            var generic = SlimType.GetGenericMethod(genDef, new TypeSlim[] { SlimType }.ToReadOnly());
            Assert.AreSame(generic, visitor.Visit(generic));
        }

        [TestMethod]
        public void MemberInfoSlimVisitor_VisitGenericMethod_AreNotSame()
        {
            var visitor = new A();
            var genDef = SlimType.GetGenericDefinitionMethod("Foo", new TypeSlim[] { TypeSlim.GenericParameter("T") }.ToReadOnly(), EmptyReadOnlyCollection<TypeSlim>.Instance, returnType: null);
            var generic = SlimType.GetGenericMethod(genDef, new TypeSlim[] { SlimType }.ToReadOnly());
            Assert.AreNotSame(generic, visitor.Visit(generic));
        }

        private sealed class A : MemberInfoSlimVisitor
        {
            protected override MemberInfoSlim VisitGenericDefinitionMethod(GenericDefinitionMethodInfoSlim method)
            {
                return SlimType.GetGenericDefinitionMethod("Foo", new TypeSlim[] { TypeSlim.GenericParameter("T") }.ToReadOnly(), EmptyReadOnlyCollection<TypeSlim>.Instance, returnType: null);
            }

            protected override MemberInfoSlim VisitField(FieldInfoSlim field)
            {
                return SlimType.GetProperty("Foo", propertyType: null, EmptyReadOnlyCollection<TypeSlim>.Instance, canWrite: true);
            }
        }

        [TestMethod]
        public void MemberInfoSlimVisitor_VisitAndConvert_Fail()
        {
            var visitor = new A();
            var field = SlimType.GetField("name", fieldType: null);
            Assert.ThrowsException<InvalidOperationException>(() => visitor.VisitAndConvert<FieldInfoSlim>(field));
        }

        [TestMethod]
        public void MemberInfoSlimVisitor_UnknownNodeType()
        {
            var visitor = new MemberInfoSlimVisitor();
            Assert.ThrowsException<NotSupportedException>(() => visitor.Visit(new MyMember()));
        }

        [TestMethod]
        public void MemberInfoSlimVisitor_Generic_UnknownNodeType()
        {
            var visitor = new MyVisitor();
            Assert.ThrowsException<NotSupportedException>(() => visitor.Visit(new MyMember()));
        }

        [TestMethod]
        public void MemberInfoSlimVisitor_UnknownMethodType()
        {
            var visitor = new MemberInfoSlimVisitor();
            Assert.ThrowsException<NotSupportedException>(() => visitor.Visit(new MyMethod()));
        }

        [TestMethod]
        public void MemberInfoSlimVisitor_Generic_UnknownMethodType()
        {
            var visitor = new MyVisitor();
            Assert.ThrowsException<NotSupportedException>(() => visitor.Visit(new MyMethod()));
        }

        [TestMethod]
        public void MemberInfoSlimVisitor_Generic_GenericMethod()
        {
            var visitor = new MyVisitor();

            var genDef = SlimType.GetGenericDefinitionMethod("Foo", new TypeSlim[] { TypeSlim.GenericParameter("T") }.ToReadOnly(), EmptyReadOnlyCollection<TypeSlim>.Instance, returnType: null);
            var generic = SlimType.GetGenericMethod(genDef, new TypeSlim[] { SlimType }.ToReadOnly());

            var res = visitor.Visit(generic);

            Assert.AreEqual(42, res);
        }

        private sealed class MyMember : MemberInfoSlim
        {
            public MyMember()
                : base(typeof(int).ToTypeSlim())
            {
            }

            public override MemberTypes MemberType => (MemberTypes)99;
        }

        private sealed class MyMethod : MethodInfoSlim
        {
            public MyMethod()
                : base(typeof(int).ToTypeSlim())
            {
            }

            public override bool IsGenericMethod => false;

            public override MethodInfoSlimKind Kind => (MethodInfoSlimKind)99;

            public override ReadOnlyCollection<TypeSlim> ParameterTypes => throw new NotImplementedException();

            public override TypeSlim ReturnType => throw new NotImplementedException();
        }

        private sealed class MyVisitor : MemberInfoSlimVisitor<int, int, int, int, int, int, int, int>
        {
            protected override int VisitConstructor(ConstructorInfoSlim constructor) => throw new NotImplementedException();

            protected override int VisitField(FieldInfoSlim field) => throw new NotImplementedException();

            protected override int VisitSimpleMethod(SimpleMethodInfoSlim method) => throw new NotImplementedException();

            protected override int VisitGenericDefinitionMethod(GenericDefinitionMethodInfoSlim method) => 40;

            protected override int MakeGenericMethod(GenericMethodInfoSlim method, int methodDefinition) => methodDefinition + 2;

            protected override int VisitProperty(PropertyInfoSlim property) => throw new NotImplementedException();
        }
    }
}
