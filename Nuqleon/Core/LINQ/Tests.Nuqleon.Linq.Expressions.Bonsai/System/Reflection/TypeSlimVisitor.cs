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
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Linq.CompilerServices;
using System.Reflection;
using Tests.System.Linq.Expressions.Bonsai;

namespace Tests.System.Reflection
{
    [TestClass]
    public class TypeSlimVisitorTests : TestBase
    {
        [TestMethod]
        public void TypeSlimVisitor_Visit_NullArguments()
        {
            var visitor = new TypeSlimVisitor();
            AssertEx.ThrowsException<ArgumentNullException>(() => visitor.Visit((TypeSlim)null), ex => Assert.AreEqual("type", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => visitor.Visit((ReadOnlyCollection<TypeSlim>)null), ex => Assert.AreEqual("types", ex.ParamName));
        }

        [TestMethod]
        public void TypeSlimVisitor_Visit_ResultIsSame()
        {
            var visitor = new TypeSlimVisitor();
            var slimifier = new TypeToTypeSlimConverter();
            var slim = slimifier.Visit(typeof(List<int[]>));
            Assert.AreSame(slim, visitor.Visit(slim));
        }

        [TestMethod]
        [Ignore, Description("Ignoring until we can correctly enable reference equal results.")]
        public void TypeSlimVisitor_VisitStructural_Recursive_ResultIsSame()
        {
            var visitor = new TypeSlimVisitor();
            var empty = EmptyReadOnlyCollection<TypeSlim>.Instance;
            var structural = StructuralTypeSlimReference.Create(hasValueEqualitySemantics: true, StructuralTypeSlimKind.Record);
            var recursiveRef = structural.GetProperty("Foo", structural, empty, canWrite: true);
            structural.AddProperty(recursiveRef);
            structural.Freeze();
            Assert.AreSame(structural, visitor.Visit(structural));
        }

        [TestMethod]
        [Ignore, Description("Ignoring until we can correctly enable reference equal results.")]
        public void TypeSlimVisitor_VisitStructural_DeepRecursive_ResultIsSame()
        {
            var visitor = new TypeSlimVisitor();
            var empty = EmptyReadOnlyCollection<TypeSlim>.Instance;
            var s1 = StructuralTypeSlimReference.Create(hasValueEqualitySemantics: true, StructuralTypeSlimKind.Record);
            var s2 = StructuralTypeSlimReference.Create(hasValueEqualitySemantics: true, StructuralTypeSlimKind.Record);
            var s1Prop = s2.GetProperty("Foo", s1, empty, canWrite: true);
            var s2Prop = s1.GetProperty("Bar", s2, empty, canWrite: true);
            s1.AddProperty(s2Prop);
            s2.AddProperty(s1Prop);
            s1.Freeze();
            s2.Freeze();
            Assert.AreSame(s1, visitor.Visit(s1));
        }

        [TestMethod]
        public void TypeSlimVisitor_VisitStructural_DeepRecursive_ManOrBoy()
        {
            var visitor = new B();
            var empty = EmptyReadOnlyCollection<TypeSlim>.Instance;
            var s1 = StructuralTypeSlimReference.Create(hasValueEqualitySemantics: true, StructuralTypeSlimKind.Record);
            var s2 = StructuralTypeSlimReference.Create(hasValueEqualitySemantics: true, StructuralTypeSlimKind.Record);
            var s1Prop = s2.GetProperty("Foo", s1, empty, canWrite: true);
            var changedProp = s2.GetProperty("Baz", SlimType, empty, canWrite: true);
            var s2Prop = s1.GetProperty("Bar", s2, empty, canWrite: true);
            s1.AddProperty(s2Prop);
            s2.AddProperty(s1Prop);
            s2.AddProperty(changedProp);
            s1.Freeze();
            s2.Freeze();
            Assert.AreSame(s1, s2.Properties.SingleOrDefault(p => p.Name == "Foo").PropertyType);

            var rs2 = (StructuralTypeSlim)visitor.Visit(s2);
            var rs1 = (StructuralTypeSlim)rs2.Properties.SingleOrDefault(p => p.Name == "Foo").PropertyType;
            Assert.AreSame(rs2, rs1.Properties.SingleOrDefault(p => p.Name == "Bar").PropertyType);
        }

        [TestMethod]
        public void TypeSlimVisitor_VisitStructural_NullPropertyType_ResultIsSame()
        {
            var visitor = new TypeSlimVisitor();
            var structural = StructuralTypeSlimReference.Create(hasValueEqualitySemantics: true, StructuralTypeSlimKind.Record);
            var index = new List<TypeSlim> { SlimType }.AsReadOnly();
            var prop = structural.GetProperty("Foo", propertyType: null, index, canWrite: true);
            structural.AddProperty(prop);
            structural.Freeze();
            Assert.AreSame(structural, visitor.Visit(structural));
        }

        [TestMethod]
        public void TypeSlimVisitor_VisitStructural_Simple_ResultIsSame()
        {
            var visitor = new TypeSlimVisitor();
            var structural = StructuralTypeSlimReference.Create(hasValueEqualitySemantics: true, StructuralTypeSlimKind.Record);
            var index = new List<TypeSlim> { SlimType }.AsReadOnly();
            var prop = structural.GetProperty("Foo", SlimType, index, canWrite: true);
            structural.AddProperty(prop);
            structural.Freeze();
            Assert.AreSame(structural, visitor.Visit(structural));
        }

        [TestMethod]
        public void TypeSlimVisitor_VisitStructural_IndexParameterChange_ResultIsNotSame()
        {
            var visitor = new B();
            var structural = StructuralTypeSlimReference.Create(hasValueEqualitySemantics: true, StructuralTypeSlimKind.Record);
            var index = new List<TypeSlim> { SlimType }.AsReadOnly();
            var prop = structural.GetProperty("Foo", structural, index, canWrite: true);
            structural.AddProperty(prop);
            structural.Freeze();
            Assert.AreNotSame(structural, visitor.Visit(structural));
        }

        private sealed class B : TypeSlimVisitor
        {
            protected override TypeSlim VisitSimple(SimpleTypeSlim type)
            {
                if (type == SlimType)
                    return TypeSlim.Simple(new AssemblySlim("Foo"), "Bar");
                else
                    return base.VisitSimple(type);
            }
        }

        [TestMethod]
        public void TypeSlimVisitor_VisitStructural_ResultIsSame()
        {
            var visitor = new C();
            var empty = EmptyReadOnlyCollection<TypeSlim>.Instance;
            var s1 = StructuralTypeSlimReference.Create(hasValueEqualitySemantics: true, StructuralTypeSlimKind.Record);
            var s2 = StructuralTypeSlimReference.Create(hasValueEqualitySemantics: false, StructuralTypeSlimKind.Record);
            var s1Prop = s2.GetProperty("Foo", s1, empty, canWrite: true);
            var s2Prop = s1.GetProperty("Bar", s2, empty, canWrite: true);
            s1.AddProperty(s2Prop);
            s2.AddProperty(s1Prop);
            s1.Freeze();
            s2.Freeze();
            Assert.AreSame(s1, visitor.Visit(s1));
        }

        private sealed class C : TypeSlimVisitor
        {
            protected override TypeSlim VisitStructural(StructuralTypeSlim type)
            {
                if (!type.HasValueEqualitySemantics)
                {
                    var newType = StructuralTypeSlimReference.Create(type.HasValueEqualitySemantics, type.StructuralKind);
                    foreach (var property in type.Properties)
                        newType.AddProperty(property);
                    newType.Freeze();
                    return newType;
                }
                else
                    return base.VisitStructural(type);
            }
        }

        [TestMethod]
        public void TypeSlimVisitor_VisitAndConvert_Success()
        {
            var visitor = new TypeSlimVisitor();
            Assert.AreSame(SlimType, visitor.VisitAndConvert<TypeSlim>(SlimType));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TypeSlimVisitor_VisitAndConvert_Fail()
        {
            var visitor = new D();
            visitor.VisitAndConvert<SimpleTypeSlim>(SlimType);
        }

        private sealed class D : TypeSlimVisitor
        {
            protected override TypeSlim VisitSimple(SimpleTypeSlim type)
            {
                if (type == SlimType)
                    return TypeSlim.Array(type);
                else
                    return base.VisitSimple(type);
            }
        }


        [TestMethod]
        public void TypeSlimVisitor_Visit_Collection_MakeNewCollection()
        {
            var visitor = new B();
            var types = new List<TypeSlim> { TypeSlim.Simple(new AssemblySlim("Foo"), "Bar"), SlimType, SlimType }.AsReadOnly();
            Assert.AreNotSame(types, visitor.Visit(types));
        }

        [TestMethod]
        public void TypeSlimVisitor_Visit_GenericParameter()
        {
            var gp = TypeSlim.GenericParameter("T");

            var visitor = new TypeSlimVisitor();

            Assert.AreSame(gp, visitor.Visit(gp));
        }

        [TestMethod]
        public void TypeSlimVisitor_Visit_Simple()
        {
            var t = typeof(int).ToTypeSlim();

            var visitor = new TypeSlimVisitor();

            Assert.AreSame(t, visitor.Visit(t));
        }

        [TestMethod]
        public void TypeSlimVisitor_Visit_UnknownType()
        {
            var t = new MyTypeSlim();

            var visitor = new TypeSlimVisitor();

            Assert.ThrowsException<NotSupportedException>(() => visitor.Visit(t));
        }

        private sealed class MyTypeSlim : TypeSlim
        {
            public override TypeSlimKind Kind => (TypeSlimKind)99;
        }

        [TestMethod]
        public void TypeSlimVisitor_Visit_GenericChangeDefinition()
        {
            TypeSlimVisitor_Visit_GenericChangeDefinition(typeof(Func<int>), typeof(Action<int>));
            TypeSlimVisitor_Visit_GenericChangeDefinition(typeof(Func<int, string>), typeof(Action<int, string>));
            TypeSlimVisitor_Visit_GenericChangeDefinition(typeof(Func<int, string, bool>), typeof(Action<int, string, bool>));
            TypeSlimVisitor_Visit_GenericChangeDefinition(typeof(Func<int, string, bool, long>), typeof(Action<int, string, bool, long>));
            TypeSlimVisitor_Visit_GenericChangeDefinition(typeof(Func<int, string, bool, long, char>), typeof(Action<int, string, bool, long, char>));
        }

        private static void TypeSlimVisitor_Visit_GenericChangeDefinition(Type input, Type expected)
        {
            var t = (GenericTypeSlim)input.ToTypeSlim();

            for (var i = 0; i < 2; i++)
            {
                var visitor = new GenericDefinitionChange();

                var res = visitor.Visit(t);

                var r = res.ToType();

                Assert.AreEqual(expected, r);

                _ = t.GenericArguments; // Causes allocation of the underlying collection
            }
        }

        [TestMethod]
        public void TypeSlimVisitor_Visit_GenericChangeArguments()
        {
            TypeSlimVisitor_Visit_GenericChangeArguments(typeof(Func<int>), typeof(Func<long>));

            TypeSlimVisitor_Visit_GenericChangeArguments(typeof(Func<int, string>), typeof(Func<long, string>));
            TypeSlimVisitor_Visit_GenericChangeArguments(typeof(Func<string, int>), typeof(Func<string, long>));

            TypeSlimVisitor_Visit_GenericChangeArguments(typeof(Func<int, bool, string>), typeof(Func<long, bool, string>));
            TypeSlimVisitor_Visit_GenericChangeArguments(typeof(Func<bool, int, string>), typeof(Func<bool, long, string>));
            TypeSlimVisitor_Visit_GenericChangeArguments(typeof(Func<bool, string, int>), typeof(Func<bool, string, long>));

            TypeSlimVisitor_Visit_GenericChangeArguments(typeof(Func<int, bool, char, string>), typeof(Func<long, bool, char, string>));
            TypeSlimVisitor_Visit_GenericChangeArguments(typeof(Func<bool, int, char, string>), typeof(Func<bool, long, char, string>));
            TypeSlimVisitor_Visit_GenericChangeArguments(typeof(Func<bool, char, int, string>), typeof(Func<bool, char, long, string>));
            TypeSlimVisitor_Visit_GenericChangeArguments(typeof(Func<bool, char, string, int>), typeof(Func<bool, char, string, long>));

            TypeSlimVisitor_Visit_GenericChangeArguments(typeof(Func<int, bool, char, string, byte>), typeof(Func<long, bool, char, string, byte>));
            TypeSlimVisitor_Visit_GenericChangeArguments(typeof(Func<bool, int, char, string, byte>), typeof(Func<bool, long, char, string, byte>));
            TypeSlimVisitor_Visit_GenericChangeArguments(typeof(Func<bool, char, int, string, byte>), typeof(Func<bool, char, long, string, byte>));
            TypeSlimVisitor_Visit_GenericChangeArguments(typeof(Func<bool, char, string, int, byte>), typeof(Func<bool, char, string, long, byte>));
            TypeSlimVisitor_Visit_GenericChangeArguments(typeof(Func<bool, char, string, byte, int>), typeof(Func<bool, char, string, byte, long>));

            TypeSlimVisitor_Visit_GenericChangeArguments(typeof(Func<int, int>), typeof(Func<long, long>));
            TypeSlimVisitor_Visit_GenericChangeArguments(typeof(Func<int, int, int>), typeof(Func<long, long, long>));
            TypeSlimVisitor_Visit_GenericChangeArguments(typeof(Func<int, int, int, int>), typeof(Func<long, long, long, long>));
            TypeSlimVisitor_Visit_GenericChangeArguments(typeof(Func<int, int, int, int, int>), typeof(Func<long, long, long, long, long>));
        }

        private static void TypeSlimVisitor_Visit_GenericChangeArguments(Type from, Type expected)
        {
            var t = (GenericTypeSlim)from.ToTypeSlim();

            for (var i = 0; i < 2; i++)
            {
                var visitor = new GenericArgumentsChange();

                var res = visitor.Visit(t);

                var r = res.ToType();

                Assert.AreEqual(expected, r);

                _ = t.GenericArguments; // Causes allocation of the underlying collection
            }
        }

        [TestMethod]
        public void TypeSlimVisitor_Visit_Helpers()
        {
            new MySimpleVisitor().Test();
        }

        private sealed class GenericDefinitionChange : TypeSlimVisitor
        {
            protected override TypeSlim VisitGenericDefinition(GenericDefinitionTypeSlim type)
            {
                if (type.Name.StartsWith("System.Func`"))
                {
#if NET5_0 || NETCOREAPP3_1
                    var action = "System.Action`" + type.Name["System.Func`".Length..];
#else
                    var action = "System.Action`" + type.Name.Substring("System.Func`".Length);
#endif
                    return TypeSlim.GenericDefinition(type.Assembly, action);
                }

                return base.VisitGenericDefinition(type);
            }
        }

        private sealed class GenericArgumentsChange : TypeSlimVisitor
        {
            protected override TypeSlim VisitSimple(SimpleTypeSlim type)
            {
                if (type.Name == "System.Int32")
                {
                    return TypeSlim.Simple(type.Assembly, "System.Int64");
                }

                return base.VisitSimple(type);
            }
        }

        private sealed class MySimpleVisitor : TypeSlimVisitor<int, int, int, int, int, int, int>
        {
            public void Test()
            {
                Assert.ThrowsException<ArgumentNullException>(() => base.Visit(default(ReadOnlyCollection<TypeSlim>)));
                Assert.ThrowsException<ArgumentNullException>(() => base.VisitAndConvert<int>(default(ReadOnlyCollection<TypeSlim>)));

                var ts = new TypeSlim[] { typeof(int).ToTypeSlim(), typeof(long).ToTypeSlim() }.ToReadOnly();

                var res1 = base.Visit(ts);
                Assert.IsTrue(res1.SequenceEqual(new[] { 32, 64 }));

                var res2 = base.VisitAndConvert<int>(ts);
                Assert.IsTrue(res2.SequenceEqual(new[] { 32, 64 }));
            }

            protected override int VisitSimple(SimpleTypeSlim type)
            {
                if (type.Equals(typeof(int).ToTypeSlim()))
                    return 32;
                else if (type.Equals(typeof(long).ToTypeSlim()))
                    return 64;
                else
                    throw new InvalidOperationException();
            }

            protected override int MakeStructuralType(StructuralTypeSlim type, IEnumerable<KeyValuePair<PropertyInfoSlim, int>> propertyTypes, IEnumerable<KeyValuePair<PropertyInfoSlim, ReadOnlyCollection<int>>> propertyIndexParameters) => throw new NotImplementedException();

            protected override int MakeArrayType(ArrayTypeSlim type, int elementType, int? rank) => throw new NotImplementedException();

            protected override int MakeGenericDefinition(GenericDefinitionTypeSlim type) => throw new NotImplementedException();

            protected override int MakeGeneric(GenericTypeSlim type, int typeDefinition, ReadOnlyCollection<int> arguments) => throw new NotImplementedException();

            protected override int VisitGenericParameter(GenericParameterTypeSlim type) => throw new NotImplementedException();
        }
    }
}
