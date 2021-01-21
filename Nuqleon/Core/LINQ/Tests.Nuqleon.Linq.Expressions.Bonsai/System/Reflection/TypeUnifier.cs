// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - July 2013 - Created this file.
//

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Linq.CompilerServices;
using System.Linq.CompilerServices.TypeSystem;
using Tests.System.Linq.Expressions.Bonsai;

namespace System.Reflection
{
    [TestClass]
    public class TypeUnifierTests : TestBase
    {
        #region Unify

        #region Pass

        [TestMethod]
        public void TryUnify_AlreadyMapped_Pass()
        {
            var unifier = new TypeUnifier();
            var typeToSlim = new TypeToTypeSlimConverter();
            var intSlim = typeToSlim.Visit(typeof(int));
            unifier.Unify(typeof(int), intSlim);
            Assert.IsTrue(unifier.Unify(typeof(int), intSlim));
        }

        #endregion

        #region Fail

        [TestMethod]
        public void Unify_NullArguments()
        {
            var unifier = new TypeUnifier();
            AssertEx.ThrowsException<ArgumentNullException>(() => unifier.Unify(typeRich: null, typeSlim: null), ex => Assert.AreEqual("typeRich", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => unifier.Unify(typeof(int), typeSlim: null), ex => Assert.AreEqual("typeSlim", ex.ParamName));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Unify_AlreadyMapped()
        {
            var unifier = new TypeUnifier();
            var typeToSlim = new TypeToTypeSlimConverter();
            var intSlim = typeToSlim.Visit(typeof(int));
            unifier.Unify(typeof(int), intSlim);
            unifier.Unify(typeof(double), intSlim);
        }

        [TestMethod]
        public void TryUnify_AlreadyMapped()
        {
            var unifier = new TypeUnifier(safe: true);
            var typeToSlim = new TypeToTypeSlimConverter();
            var intSlim = typeToSlim.Visit(typeof(int));
            unifier.Unify(typeof(int), intSlim);
            Assert.IsFalse(unifier.Unify(typeof(double), intSlim));
        }

        [TestMethod]
        public void Unify_Wildcard()
        {
            var unifier = new TypeUnifier(safe: true);
            var typeToSlim = new TypeToTypeSlimConverter();
            var slim = typeToSlim.Visit(typeof(Func<int, int>));
            Assert.IsTrue(unifier.Unify(typeof(Func<T, T>), slim));
            Assert.AreEqual(0, unifier.Entries.Count);
        }

        [TestMethod]
        public void Unify_Wildcard_Fail()
        {
            var unifier = new TypeUnifier(safe: true);
            var typeToSlim = new TypeToTypeSlimConverter();
            var slim = typeToSlim.Visit(typeof(Func<int, bool>));
            Assert.IsFalse(unifier.Unify(typeof(Func<T, T>), slim));
            Assert.AreEqual(0, unifier.Entries.Count);
        }

        [TestMethod]
        public void Unify_UnknownType()
        {
            var unifier = new TypeUnifier();
            var ts = new FakeType();
            Assert.ThrowsException<NotSupportedException>(() => unifier.Unify(typeof(int), ts));
        }

        private sealed class FakeType : TypeSlim
        {
            public override TypeSlimKind Kind => (TypeSlimKind)99;
        }

        #endregion

        #endregion

        #region UnifySimple

        #region Pass

        [TestMethod]
        public void UnifySimple_Pass()
        {
            var unifier = new TypeUnifier();
            var typeToSlim = new TypeToTypeSlimConverter();
            var intSlim = typeToSlim.Visit(typeof(int));

            Assert.IsTrue(unifier.Unify(typeof(int), intSlim));
            Assert.AreEqual(0, unifier.Entries.Count);
        }

        #endregion

        #region Fail

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void UnifySimple_DifferentTypeName_Fail()
        {
            var unifier = new TypeUnifier();
            var typeToSlim = new TypeToTypeSlimConverter();
            var doubleSlim = typeToSlim.Visit(typeof(double));
            unifier.Unify(typeof(int), doubleSlim);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void UnifySimple_DifferentAssemblyName_Fail()
        {
            var unifier = new TypeUnifier();
            unifier.Unify(typeof(int), TypeSlim.Simple(new AssemblySlim("Foo"), "System.Int32"));
        }

        #endregion

        #endregion

        #region UnifyArray

        #region Pass

        [TestMethod]
        public void UnifyArray_Vector_Pass()
        {
            var unifier = new TypeUnifier();
            var typeToSlim = new TypeToTypeSlimConverter();
            var intArraySlim = typeToSlim.Visit(typeof(int[]));

            Assert.IsTrue(unifier.Unify(typeof(int[]), intArraySlim));
            Assert.AreEqual(0, unifier.Entries.Count);
        }

        #endregion

        #region Fail

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void UnifyArray_DifferentElementType_Fail()
        {
            var unifier = new TypeUnifier();
            var typeToSlim = new TypeToTypeSlimConverter();
            var doubleArraySlim = typeToSlim.Visit(typeof(double[]));
            unifier.Unify(typeof(int[]), doubleArraySlim);
        }

        [TestMethod]
        public void TryUnifyArray_DifferentElementType_Fail()
        {
            var unifier = new TypeUnifier(safe: true);
            var typeToSlim = new TypeToTypeSlimConverter();
            var doubleArraySlim = typeToSlim.Visit(typeof(double[]));
            Assert.IsFalse(unifier.Unify(typeof(int[]), doubleArraySlim));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void UnifyArray_DifferentRank_Fail()
        {
            var unifier = new TypeUnifier();
            var typeToSlim = new TypeToTypeSlimConverter();
            var intArraySlim = typeToSlim.Visit(typeof(int[]));
            unifier.Unify(typeof(int[,]), intArraySlim);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void UnifyArray_DifferentRankMultidimensionalSlim_Fail()
        {
            var unifier = new TypeUnifier();
            var typeToSlim = new TypeToTypeSlimConverter();
            var intArraySlim = typeToSlim.Visit(typeof(int[,]));
            unifier.Unify(typeof(int[]), intArraySlim);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void UnifyArray_WithNonArrayClrType_Fail()
        {
            var unifier = new TypeUnifier();
            var typeToSlim = new TypeToTypeSlimConverter();
            var intArraySlim = typeToSlim.Visit(typeof(int[]));
            unifier.Unify(typeof(int), intArraySlim);
        }

        #endregion

        #endregion

        #region UnifyStructural

        #region Pass

        [TestMethod]
        public void UnifyStructural_DifferentPropertyTypes_Pass()
        {
            var unifier = new TypeUnifier();
            var typeToSlim = new TypeToTypeSlimConverter();

            var rtc = new RuntimeCompiler();
            var typeBuilder = rtc.GetNewRecordTypeBuilder();

            rtc.DefineRecordType(
                typeBuilder,
                (new Dictionary<string, Type> { { "foo", typeof(int) } }).AsEnumerable(),
                true
            );

            var structuralType = typeBuilder.CreateType();
            var slimStructuralType = typeToSlim.Visit(structuralType);

            Assert.IsTrue(unifier.Unify(structuralType, slimStructuralType));

            Assert.AreEqual(1, unifier.Entries.Count);

            Assert.IsTrue(unifier.Entries.ContainsKey(slimStructuralType));
            Assert.AreEqual(unifier.Entries[slimStructuralType], structuralType);

            Assert.IsTrue(slimStructuralType is StructuralTypeSlim);
        }

        #endregion

        #region Fail

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void UnifyStructural_DifferentPropertyTypes_Fail()
        {
            var unifier = new TypeUnifier();
            var typeToSlim = new TypeToTypeSlimConverter();

            var rtc = new RuntimeCompiler();
            var typeBuilder1 = rtc.GetNewRecordTypeBuilder();
            var typeBuilder2 = rtc.GetNewRecordTypeBuilder();

            rtc.DefineRecordType(
                typeBuilder1,
                (new Dictionary<string, Type> { { "foo", typeof(int) } }).AsEnumerable(),
                true
            );

            rtc.DefineRecordType(
                typeBuilder2,
                (new Dictionary<string, Type> { { "foo", typeof(double) } }).AsEnumerable(),
                true
            );

            var structuralType1 = typeBuilder1.CreateType();
            var structuralType2 = typeBuilder2.CreateType();
            var slimStructuralType = typeToSlim.Visit(structuralType2);

            unifier.Unify(structuralType1, slimStructuralType);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void UnifyStructural_MissingProperty_Fail()
        {
            var unifier = new TypeUnifier();
            var typeToSlim = new TypeToTypeSlimConverter();

            var rtc = new RuntimeCompiler();
            var typeBuilder1 = rtc.GetNewRecordTypeBuilder();
            var typeBuilder2 = rtc.GetNewRecordTypeBuilder();

            rtc.DefineRecordType(
                typeBuilder1,
                (new Dictionary<string, Type> { { "foo", typeof(int) } }).AsEnumerable(),
                true
            );

            rtc.DefineRecordType(
                typeBuilder2,
                (new Dictionary<string, Type> { { "bar", typeof(double) } }).AsEnumerable(),
                true
            );

            var structuralType1 = typeBuilder1.CreateType();
            var structuralType2 = typeBuilder2.CreateType();
            var slimStructuralType = typeToSlim.Visit(structuralType2);

            unifier.Unify(structuralType1, slimStructuralType);
        }

        #endregion

        #endregion

        #region UnifyGenericDefinition

        #region Pass

        [TestMethod]
        public void UnifyGenericDefinition_Pass()
        {
            var unifier = new TypeUnifier();
            var typeToSlim = new TypeToTypeSlimConverter();
            var definition = typeof(List<>);
            var slimDefinition = typeToSlim.Visit(definition);

            Assert.IsTrue(unifier.Unify(definition, slimDefinition));
            Assert.AreEqual(0, unifier.Entries.Count);
        }

        #endregion

        #region Fail

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void UnifyGenericDefinition_WithNonGenericDefClrType_Fail()
        {
            var unifier = new TypeUnifier();
            var typeToSlim = new TypeToTypeSlimConverter();
            var definition = typeof(List<>);
            var slimDefinition = typeToSlim.Visit(definition);
            unifier.Unify(typeof(int), slimDefinition);
        }

        #endregion

        #endregion

        #region UnifyGeneric

        #region Pass

        [TestMethod]
        public void UnifyGeneric_Pass()
        {
            var unifier = new TypeUnifier();
            var typeToSlim = new TypeToTypeSlimConverter();
            var generic = typeof(List<int>);
            var slimGeneric = typeToSlim.Visit(generic);

            Assert.IsTrue(unifier.Unify(generic, slimGeneric));
            Assert.AreEqual(0, unifier.Entries.Count);
        }

        #endregion

        #region Fail

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void UnifyGeneric_WithNonGenericClrType_Fail()
        {
            var unifier = new TypeUnifier();
            var typeToSlim = new TypeToTypeSlimConverter();
            var generic = typeof(List<int>);
            var slimGeneric = typeToSlim.Visit(generic);
            unifier.Unify(typeof(int), slimGeneric);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void UnifyGeneric_WithGenericDefClrType_Fail()
        {
            var unifier = new TypeUnifier();
            var typeToSlim = new TypeToTypeSlimConverter();
            var generic = typeof(List<int>);
            var slimGeneric = typeToSlim.Visit(generic);
            unifier.Unify(typeof(List<>), slimGeneric);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void UnifyGeneric_DifferentDefinition_Fail()
        {
            var unifier = new TypeUnifier();
            var typeToSlim = new TypeToTypeSlimConverter();
            var generic = typeof(List<int>);
            var slimGeneric = typeToSlim.Visit(generic);
            unifier.Unify(typeof(Nullable<int>), slimGeneric);
        }

        [TestMethod]
        public void TryUnifyGeneric_DifferentDefinition_Fail()
        {
            var unifier = new TypeUnifier(safe: true);
            var typeToSlim = new TypeToTypeSlimConverter();
            var generic = typeof(List<int>);
            var slimGeneric = typeToSlim.Visit(generic);
            Assert.IsFalse(unifier.Unify(typeof(Nullable<int>), slimGeneric));
        }


        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void UnifyGeneric_DifferentArguments_Fail()
        {
            var unifier = new TypeUnifier();
            var typeToSlim = new TypeToTypeSlimConverter();
            var generic = typeof(List<int>);
            var slimGeneric = typeToSlim.Visit(generic);
            unifier.Unify(typeof(List<double>), slimGeneric);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void UnifyGenericParameter_Fail()
        {
            var unifier = new TypeUnifier();
            var openGenericParameterType = typeof(List<>).GetGenericArguments()[0];
            var genericParameterTypeSlim = TypeSlim.GenericParameter("T");
            unifier.Unify(openGenericParameterType, genericParameterTypeSlim);
        }

        #endregion

        #endregion

        #region Unify Man Or Boy

        [TestMethod]
        public void UnifyManOrBoy_Pass()
        {
            var record1 = RuntimeCompiler.CreateRecordType(
                new Dictionary<string, Type> {
                    { "foo", typeof(int) }
                },
                true);
            var record2 = RuntimeCompiler.CreateRecordType(
                new Dictionary<string, Type> {
                    { "bar", record1 },
                    { "qux", typeof(string) }
                },
                true);
            var list = typeof(List<>).MakeGenericType(new[] { record2 });
            var unifier = new TypeUnifier();
            var typeToSlim = new TypeToTypeSlimConverter();
            var slimList = typeToSlim.Visit(list);

            Assert.IsTrue(unifier.Unify(list, slimList));
            Assert.AreEqual(2, unifier.Entries.Count);
        }

        [TestMethod]
        public void UnifyStructuralEquivalence_Pass()
        {
            var rt1 = RuntimeCompiler.CreateRecordType(
                new Dictionary<string, Type>
                {
                    { "Foo", typeof(int) },
                    { "Bar", typeof(string) }
                },
                true);

            var rt2 = RuntimeCompiler.CreateRecordType(
                new Dictionary<string, Type>
                {
                    { "Foo", typeof(int) },
                    { "Bar", typeof(string) }
                },
                true);

            var typeToSlim = new TypeToTypeSlimConverter();
            var slim = typeToSlim.Visit(rt1);
            var unifier = new TypeUnifier();

            Assert.IsTrue(unifier.Unify(rt1, slim));
            Assert.IsTrue(unifier.Unify(rt2, slim));
            Assert.AreEqual(1, unifier.Entries.Count);
            Assert.AreSame(rt1, unifier.Entries[slim]);
        }

        #endregion

        #region White box

        [TestMethod]
        public void Unify_Collection()
        {
            new MyUnifier().TestCollections();
        }

        [TestMethod]
        public void Unify_GenericParameterOverride()
        {
            var gt = typeof(List<>).GetGenericArguments()[0];
            var gs = TypeSlim.GenericParameter("T");

            Assert.IsTrue(new MyUnifier().Unify(gt, gs));
        }

        private sealed class MyUnifier : TypeUnifier
        {
            public MyUnifier()
                : base(safe: true)
            {
            }

            public void TestCollections()
            {
                var r1t = new[] { typeof(int) }.ToReadOnly();
                var r2t = new[] { typeof(string) }.ToReadOnly();
                var r3t = new[] { typeof(int), typeof(long) }.ToReadOnly();

                var r1s = r1t.Select(t => t.ToTypeSlim()).ToReadOnly();
                var r2s = r2t.Select(t => t.ToTypeSlim()).ToReadOnly();
                var r3s = r3t.Select(t => t.ToTypeSlim()).ToReadOnly();

                Assert.IsTrue(base.Unify(r1t, r1s));
                Assert.IsTrue(base.Unify(r2t, r2s));
                Assert.IsTrue(base.Unify(r3t, r3s));

                Assert.IsFalse(base.Unify(r1t, r2s));
                Assert.IsFalse(base.Unify(r1t, r3s));
                Assert.IsFalse(base.Unify(r2t, r1s));
                Assert.IsFalse(base.Unify(r2t, r3s));
                Assert.IsFalse(base.Unify(r3t, r1s));
                Assert.IsFalse(base.Unify(r3t, r2s));
            }

            protected override bool UnifyGenericParameter(Type typeRich, GenericParameterTypeSlim typeSlim)
            {
                return true;
            }
        }

        #endregion
    }
}
