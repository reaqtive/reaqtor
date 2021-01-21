// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - May 2014 - Created this file.
//

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Tests.System.Linq.Expressions.Bonsai;

namespace Tests.System.Reflection
{
    [TestClass]
    public class MemberInfoSlimEqualityComparerTests : TestBase
    {
        [TestMethod]
        public void MemberInfoSlimEqualityComparer_ArgumentChecking()
        {
#pragma warning disable IDE0034 // Simplify 'default' expression (illustrative of method signature)
            Assert.ThrowsException<ArgumentNullException>(() => _ = new MemberInfoSlimEqualityComparer(default(TypeSlimEqualityComparer)));
            Assert.ThrowsException<ArgumentNullException>(() => _ = new MemberInfoSlimEqualityComparer(default(Func<MemberInfoSlimEqualityComparator>)));
            Assert.ThrowsException<ArgumentException>(() => _ = new MemberInfoSlimEqualityComparer(() => default(MemberInfoSlimEqualityComparator)).GetComparator());
#pragma warning restore IDE0034 // Simplify 'default' expression
        }

        [TestMethod]
        public void MemberInfoSlimEqualityComparer_Trivialities()
        {
            var tq = new TypeSlimEqualityComparer();
            var eq = new MemberInfoSlimEqualityComparer(tq);

            var i4 = typeof(int).ToTypeSlim();
            var fl = new FieldInfoSlim(i4, "foo", i4);
            var pr = PropertyInfoSlim.Make(i4, "bar", i4, new List<TypeSlim>().AsReadOnly(), canWrite: true);

            Assert.IsTrue(eq.Equals(null, null));
            Assert.AreEqual(eq.GetHashCode(null), eq.GetHashCode(null));

            Assert.IsFalse(eq.Equals(fl, null));
            Assert.IsFalse(eq.Equals(null, fl));

            Assert.IsFalse(eq.Equals(fl, pr));
            Assert.IsFalse(eq.Equals(pr, fl));
        }

        [TestMethod]
        public void MemberInfoSlimEqualityComparer_Constructor()
        {
            var tq = new TypeSlimEqualityComparer();
            var eq = new MemberInfoSlimEqualityComparer(tq);

            var dt = typeof(DateTime).ToTypeSlim();
            var i4 = typeof(int).ToTypeSlim();
            var i8 = typeof(long).ToTypeSlim();
            var ts = typeof(TimeSpan).ToTypeSlim();

            var dtct3_0 = new ConstructorInfoSlim(dt, new List<TypeSlim> { i4, i4, i4 }.AsReadOnly());
            var dtct6_0 = new ConstructorInfoSlim(dt, new List<TypeSlim> { i4, i4, i4, i4, i4, i4 }.AsReadOnly());
            var tsct3_0 = new ConstructorInfoSlim(ts, new List<TypeSlim> { i4, i4, i4 }.AsReadOnly());

            var dtct3_1 = new ConstructorInfoSlim(dt, new List<TypeSlim> { i4, i4, i4 }.AsReadOnly());
            var dtct6_1 = new ConstructorInfoSlim(dt, new List<TypeSlim> { i4, i4, i4, i4, i4, i4 }.AsReadOnly());
            var tsct3_1 = new ConstructorInfoSlim(ts, new List<TypeSlim> { i4, i4, i4 }.AsReadOnly());

            var dtct3_2 = new ConstructorInfoSlim(dt, new List<TypeSlim> { i4, i8, i4 }.AsReadOnly());

            Assert.IsTrue(eq.Equals(dtct3_0, dtct3_0));
            Assert.IsTrue(eq.Equals(dtct6_0, dtct6_0));
            Assert.IsTrue(eq.Equals(tsct3_0, tsct3_0));

            Assert.IsTrue(eq.Equals(dtct3_0, dtct3_1));
            Assert.IsTrue(eq.Equals(dtct6_0, dtct6_1));
            Assert.IsTrue(eq.Equals(tsct3_0, tsct3_1));

            Assert.IsTrue(eq.Equals(dtct3_1, dtct3_0));
            Assert.IsTrue(eq.Equals(dtct6_1, dtct6_0));
            Assert.IsTrue(eq.Equals(tsct3_1, tsct3_0));

            Assert.IsFalse(eq.Equals(dtct3_0, dtct6_0));
            Assert.IsFalse(eq.Equals(dtct6_0, dtct3_0));
            Assert.IsFalse(eq.Equals(dtct3_0, tsct3_0));
            Assert.IsFalse(eq.Equals(tsct3_0, dtct3_0));

            Assert.IsFalse(eq.Equals(dtct3_0, dtct3_2));
            Assert.IsFalse(eq.Equals(dtct3_2, dtct3_0));

            Assert.AreEqual(eq.GetHashCode(dtct3_0), eq.GetHashCode(dtct3_1));
            Assert.AreEqual(eq.GetHashCode(dtct6_0), eq.GetHashCode(dtct6_1));
            Assert.AreEqual(eq.GetHashCode(tsct3_0), eq.GetHashCode(tsct3_1));
        }

        [TestMethod]
        public void MemberInfoSlimEqualityComparer_Field()
        {
            var tq = new TypeSlimEqualityComparer();
            var eq = new MemberInfoSlimEqualityComparer(tq);

            var asm = new AssemblySlim("foo");

            var tp1 = TypeSlim.Simple(asm, "bar");
            var tp2 = TypeSlim.Simple(asm, "baz");

            var i4 = typeof(int).ToTypeSlim();
            var i8 = typeof(long).ToTypeSlim();

            var fld11_0 = new FieldInfoSlim(tp1, "qux1", i4);
            var fld12_0 = new FieldInfoSlim(tp1, "qux2", i4);
            var fld13_0 = new FieldInfoSlim(tp1, "qux3", i8);

            var fld11_1 = new FieldInfoSlim(tp1, "qux1", i4);
            var fld12_1 = new FieldInfoSlim(tp1, "qux2", i4);
            var fld13_1 = new FieldInfoSlim(tp1, "qux3", i8);

            var fld21_0 = new FieldInfoSlim(tp2, "qux1", i4);

            Assert.IsTrue(eq.Equals(fld11_0, fld11_0));
            Assert.IsTrue(eq.Equals(fld11_0, fld11_1));
            Assert.IsTrue(eq.Equals(fld11_1, fld11_0));
            Assert.IsTrue(eq.Equals(fld11_1, fld11_1));

            Assert.IsTrue(eq.Equals(fld12_0, fld12_0));
            Assert.IsTrue(eq.Equals(fld12_0, fld12_1));
            Assert.IsTrue(eq.Equals(fld12_1, fld12_0));
            Assert.IsTrue(eq.Equals(fld12_1, fld12_1));

            Assert.IsTrue(eq.Equals(fld13_0, fld13_0));
            Assert.IsTrue(eq.Equals(fld13_0, fld13_1));
            Assert.IsTrue(eq.Equals(fld13_1, fld13_0));
            Assert.IsTrue(eq.Equals(fld13_1, fld13_1));

            Assert.IsFalse(eq.Equals(fld11_0, fld12_0));
            Assert.IsFalse(eq.Equals(fld12_0, fld11_0));
            Assert.IsFalse(eq.Equals(fld11_0, fld21_0));
            Assert.IsFalse(eq.Equals(fld21_0, fld11_0));

            Assert.AreEqual(eq.GetHashCode(fld11_0), eq.GetHashCode(fld11_1));
            Assert.AreEqual(eq.GetHashCode(fld12_0), eq.GetHashCode(fld12_1));
            Assert.AreEqual(eq.GetHashCode(fld13_0), eq.GetHashCode(fld13_1));
        }

        [TestMethod]
        public void MemberInfoSlimEqualityComparer_Property_NoIndex()
        {
            var tq = new TypeSlimEqualityComparer();
            var eq = new MemberInfoSlimEqualityComparer(tq);

            var asm = new AssemblySlim("foo");

            var tp1 = TypeSlim.Simple(asm, "bar");
            var tp2 = TypeSlim.Simple(asm, "baz");

            var i4 = typeof(int).ToTypeSlim();
            var i8 = typeof(long).ToTypeSlim();

            var indxs = Array.Empty<TypeSlim>().ToList().AsReadOnly();

            var prp11_0 = PropertyInfoSlim.Make(tp1, "qux1", i4, indxs, canWrite: true);
            var prp12_0 = PropertyInfoSlim.Make(tp1, "qux2", i4, indxs, canWrite: true);
            var prp13_0 = PropertyInfoSlim.Make(tp1, "qux3", i8, indxs, canWrite: true);

            var prp11_1 = PropertyInfoSlim.Make(tp1, "qux1", i4, indxs, canWrite: true);
            var prp12_1 = PropertyInfoSlim.Make(tp1, "qux2", i4, indxs, canWrite: true);
            var prp13_1 = PropertyInfoSlim.Make(tp1, "qux3", i8, indxs, canWrite: true);

            var prp21_0 = PropertyInfoSlim.Make(tp2, "qux1", i4, indxs, canWrite: true);

            Assert.IsTrue(eq.Equals(prp11_0, prp11_0));
            Assert.IsTrue(eq.Equals(prp11_0, prp11_1));
            Assert.IsTrue(eq.Equals(prp11_1, prp11_0));
            Assert.IsTrue(eq.Equals(prp11_1, prp11_1));

            Assert.IsTrue(eq.Equals(prp12_0, prp12_0));
            Assert.IsTrue(eq.Equals(prp12_0, prp12_1));
            Assert.IsTrue(eq.Equals(prp12_1, prp12_0));
            Assert.IsTrue(eq.Equals(prp12_1, prp12_1));

            Assert.IsTrue(eq.Equals(prp13_0, prp13_0));
            Assert.IsTrue(eq.Equals(prp13_0, prp13_1));
            Assert.IsTrue(eq.Equals(prp13_1, prp13_0));
            Assert.IsTrue(eq.Equals(prp13_1, prp13_1));

            Assert.IsFalse(eq.Equals(prp11_0, prp12_0));
            Assert.IsFalse(eq.Equals(prp12_0, prp11_0));
            Assert.IsFalse(eq.Equals(prp11_0, prp21_0));
            Assert.IsFalse(eq.Equals(prp21_0, prp11_0));

            Assert.AreEqual(eq.GetHashCode(prp11_0), eq.GetHashCode(prp11_1));
            Assert.AreEqual(eq.GetHashCode(prp12_0), eq.GetHashCode(prp12_1));
            Assert.AreEqual(eq.GetHashCode(prp13_0), eq.GetHashCode(prp13_1));
        }

        [TestMethod]
        public void MemberInfoSlimEqualityComparer_Property_Index()
        {
            var tq = new TypeSlimEqualityComparer();
            var eq = new MemberInfoSlimEqualityComparer(tq);

            var asm = new AssemblySlim("foo");

            var tp1 = TypeSlim.Simple(asm, "bar");

            var i4 = typeof(int).ToTypeSlim();
            var i8 = typeof(long).ToTypeSlim();

            var indxs0 = new List<TypeSlim> { i4 }.AsReadOnly();
            var indxs1 = new List<TypeSlim> { i8 }.AsReadOnly();
            var indxs2 = new List<TypeSlim> { i4, i8 }.AsReadOnly();
            var indxs3 = new List<TypeSlim> { i8, i4 }.AsReadOnly();

            var prp_i4_0_0 = PropertyInfoSlim.Make(tp1, "Item", i4, indxs0, canWrite: true);
            var prp_i4_0_1 = PropertyInfoSlim.Make(tp1, "Item", i4, indxs0, canWrite: true);
            var prp_i4_1_0 = PropertyInfoSlim.Make(tp1, "Item", i4, indxs1, canWrite: true);
            var prp_i4_2_0 = PropertyInfoSlim.Make(tp1, "Item", i4, indxs2, canWrite: true);
            var prp_i4_3_0 = PropertyInfoSlim.Make(tp1, "Item", i4, indxs3, canWrite: true);
            var prp_i4_3_1 = PropertyInfoSlim.Make(tp1, "Item", i4, indxs3, canWrite: true);

            var prp_i8_0_0 = PropertyInfoSlim.Make(tp1, "Item", i8, indxs0, canWrite: true);
            var prp_i8_1_0 = PropertyInfoSlim.Make(tp1, "Item", i8, indxs1, canWrite: true);
            var prp_i8_2_0 = PropertyInfoSlim.Make(tp1, "Item", i8, indxs2, canWrite: true);
            var prp_i8_3_0 = PropertyInfoSlim.Make(tp1, "Item", i8, indxs3, canWrite: true);

            Assert.IsTrue(eq.Equals(prp_i4_0_0, prp_i4_0_0));
            Assert.IsTrue(eq.Equals(prp_i4_0_0, prp_i4_0_1));
            Assert.IsTrue(eq.Equals(prp_i4_0_1, prp_i4_0_0));
            Assert.IsTrue(eq.Equals(prp_i4_3_0, prp_i4_3_1));
            Assert.IsTrue(eq.Equals(prp_i4_3_1, prp_i4_3_0));

            Assert.IsFalse(eq.Equals(prp_i4_0_0, prp_i4_1_0));
            Assert.IsFalse(eq.Equals(prp_i4_1_0, prp_i4_0_0));
            Assert.IsFalse(eq.Equals(prp_i4_2_0, prp_i4_3_0));
            Assert.IsFalse(eq.Equals(prp_i4_3_0, prp_i4_2_0));

            Assert.IsFalse(eq.Equals(prp_i4_0_0, prp_i8_0_0));
            Assert.IsFalse(eq.Equals(prp_i4_1_0, prp_i8_1_0));
            Assert.IsFalse(eq.Equals(prp_i4_2_0, prp_i8_2_0));
            Assert.IsFalse(eq.Equals(prp_i4_3_0, prp_i8_3_0));

            Assert.AreEqual(eq.GetHashCode(prp_i4_0_0), eq.GetHashCode(prp_i4_0_1));
            Assert.AreEqual(eq.GetHashCode(prp_i4_3_0), eq.GetHashCode(prp_i4_3_1));
        }

        [TestMethod]
        public void MemberInfoSlimEqualityComparer_Method_Simple()
        {
            var tq = new TypeSlimEqualityComparer();
            var eq = new MemberInfoSlimEqualityComparer(tq);

            var asm = new AssemblySlim("foo");

            var tp1 = TypeSlim.Simple(asm, "bar");

            var i4 = typeof(int).ToTypeSlim();
            var i8 = typeof(long).ToTypeSlim();

            var args0 = new List<TypeSlim> { i4, i8 }.AsReadOnly();
            var args1 = new List<TypeSlim> { i4 }.AsReadOnly();
            var args2 = new List<TypeSlim> { i8, i4 }.AsReadOnly();
            var args3 = new List<TypeSlim> { i4, i8, i4 }.AsReadOnly();

            var mtd_0_i4i8_i4_0 = new SimpleMethodInfoSlim(tp1, "Qux0", args0, i4);
            var mtd_0_i4i8_i4_1 = new SimpleMethodInfoSlim(tp1, "Qux0", args0, i4);

            var mtd_0_i4i8_i8_0 = new SimpleMethodInfoSlim(tp1, "Qux0", args0, i8);
            var mtd_0_i4i8_i8_1 = new SimpleMethodInfoSlim(tp1, "Qux0", args0, i8);

            var mtd_0_i4_i4_0 = new SimpleMethodInfoSlim(tp1, "Qux0", args1, i4);
            var mtd_0_i8i4_i4_0 = new SimpleMethodInfoSlim(tp1, "Qux0", args2, i4);
            var mtd_0_i4i8i4_i4_0 = new SimpleMethodInfoSlim(tp1, "Qux0", args3, i4);

            Assert.IsTrue(eq.Equals(mtd_0_i4i8_i4_0, mtd_0_i4i8_i4_0));
            Assert.IsTrue(eq.Equals(mtd_0_i4i8_i4_0, mtd_0_i4i8_i4_1));
            Assert.IsTrue(eq.Equals(mtd_0_i4i8_i4_1, mtd_0_i4i8_i4_0));

            Assert.IsTrue(eq.Equals(mtd_0_i4i8_i8_0, mtd_0_i4i8_i8_0));
            Assert.IsTrue(eq.Equals(mtd_0_i4i8_i8_0, mtd_0_i4i8_i8_1));
            Assert.IsTrue(eq.Equals(mtd_0_i4i8_i8_1, mtd_0_i4i8_i8_0));

            Assert.IsFalse(eq.Equals(mtd_0_i4i8_i4_0, mtd_0_i4i8_i8_0));
            Assert.IsFalse(eq.Equals(mtd_0_i4i8_i8_0, mtd_0_i4i8_i4_0));

            Assert.IsFalse(eq.Equals(mtd_0_i4_i4_0, mtd_0_i4i8_i8_0));
            Assert.IsFalse(eq.Equals(mtd_0_i4i8_i8_0, mtd_0_i4_i4_0));

            Assert.IsFalse(eq.Equals(mtd_0_i8i4_i4_0, mtd_0_i4i8_i8_0));
            Assert.IsFalse(eq.Equals(mtd_0_i4i8_i8_0, mtd_0_i8i4_i4_0));

            Assert.IsFalse(eq.Equals(mtd_0_i4i8i4_i4_0, mtd_0_i4i8_i8_0));
            Assert.IsFalse(eq.Equals(mtd_0_i4i8_i8_0, mtd_0_i4i8i4_i4_0));

            Assert.AreEqual(eq.GetHashCode(mtd_0_i4i8_i4_0), eq.GetHashCode(mtd_0_i4i8_i4_0));
            Assert.AreEqual(eq.GetHashCode(mtd_0_i4i8_i4_0), eq.GetHashCode(mtd_0_i4i8_i4_1));
            Assert.AreEqual(eq.GetHashCode(mtd_0_i4i8_i4_1), eq.GetHashCode(mtd_0_i4i8_i4_1));
        }

        [TestMethod]
        public void MemberInfoSlimEqualityComparer_Method_Generic()
        {
            var e1 = (Expression<Func<IEnumerable<int>, IEnumerable<int>>>)(xs => xs.Where(x => true));
            var e2 = (Expression<Func<IEnumerable<int>, IEnumerable<int>>>)(xs => xs.Where(x => true));
            var e3 = (Expression<Func<IEnumerable<int>, IEnumerable<int>>>)(xs => xs.Select(x => x));
            var e4 = (Expression<Func<IEnumerable<int>, IEnumerable<int>>>)(xs => xs.Where((x, i) => true));
            var e5 = (Expression<Func<IEnumerable<long>, IEnumerable<long>>>)(xs => xs.Where(x => true));

            var c1 = (MethodCallExpressionSlim)e1.Body.ToExpressionSlim();
            var c2 = (MethodCallExpressionSlim)e2.Body.ToExpressionSlim();
            var c3 = (MethodCallExpressionSlim)e3.Body.ToExpressionSlim();
            var c4 = (MethodCallExpressionSlim)e4.Body.ToExpressionSlim();
            var c5 = (MethodCallExpressionSlim)e5.Body.ToExpressionSlim();

            var m1 = c1.Method;
            var m2 = c2.Method;
            var m3 = c3.Method;
            var m4 = c4.Method;
            var m5 = c5.Method;

            var tq = new TypeSlimEqualityComparer();
            var eq = new MemberInfoSlimEqualityComparer(tq);

            Assert.IsTrue(eq.Equals(m1, m1));
            Assert.IsTrue(eq.Equals(m1, m2));
            Assert.IsTrue(eq.Equals(m2, m1));

            Assert.IsFalse(eq.Equals(m1, m3));
            Assert.IsFalse(eq.Equals(m3, m1));

            Assert.IsFalse(eq.Equals(m1, m4));
            Assert.IsFalse(eq.Equals(m4, m1));

            Assert.IsFalse(eq.Equals(m1, m5));
            Assert.IsFalse(eq.Equals(m5, m1));

            Assert.AreEqual(eq.GetHashCode(m1), eq.GetHashCode(m2));

            var d1 = ((GenericMethodInfoSlim)c1.Method).GenericMethodDefinition;
            var d2 = ((GenericMethodInfoSlim)c2.Method).GenericMethodDefinition;
            var d3 = ((GenericMethodInfoSlim)c3.Method).GenericMethodDefinition;
            var d4 = ((GenericMethodInfoSlim)c4.Method).GenericMethodDefinition;
            var d5 = ((GenericMethodInfoSlim)c5.Method).GenericMethodDefinition;

            Assert.IsTrue(eq.Equals(d1, d1));
            Assert.IsTrue(eq.Equals(d1, d2));
            Assert.IsTrue(eq.Equals(d2, d1));

            Assert.IsFalse(eq.Equals(d1, d3));
            Assert.IsFalse(eq.Equals(d3, d1));

            Assert.IsFalse(eq.Equals(d1, d4));
            Assert.IsFalse(eq.Equals(d4, d1));

            Assert.IsTrue(eq.Equals(d1, d5));
            Assert.IsTrue(eq.Equals(d5, d1));

            Assert.AreEqual(eq.GetHashCode(d1), eq.GetHashCode(d2));
        }

        [TestMethod]
        public void MemberInfoSlimEqualityComparer_NotSupported()
        {
            var ts = new[]
            {
                MemberTypes.Event,
                MemberTypes.NestedType,
                MemberTypes.TypeInfo,
            };

            var tq = new TypeSlimEqualityComparer();
            var eq = new MemberInfoSlimEqualityComparer(tq);

            foreach (var t in ts)
            {
                var m1 = new NotSupportedMemberInfoSlim(t);
                var m2 = new NotSupportedMemberInfoSlim(t);

                Assert.ThrowsException<NotSupportedException>(() => eq.GetHashCode(m1));
                Assert.ThrowsException<NotSupportedException>(() => eq.Equals(m1, m2));
            }
        }

        [TestMethod]
        public void MemberInfoSlimEqualityComparer_NotImplemented()
        {
            var ts = new[]
            {
                MemberTypes.Custom,
                (MemberTypes)99,
            };

            var tq = new TypeSlimEqualityComparer();
            var eq = new MemberInfoSlimEqualityComparer(tq);

            foreach (var t in ts)
            {
                var m1 = new NotSupportedMemberInfoSlim(t);
                var m2 = new NotSupportedMemberInfoSlim(t);

                Assert.ThrowsException<NotImplementedException>(() => eq.GetHashCode(m1));
                Assert.ThrowsException<NotImplementedException>(() => eq.Equals(m1, m2));
            }
        }

        [TestMethod]
        public void MemberInfoSlimEqualityComparer_NotSupported_Override()
        {
            var ts = new[]
            {
                MemberTypes.Custom,
                MemberTypes.Event,
                MemberTypes.NestedType,
                MemberTypes.TypeInfo,
                (MemberTypes)99,
            };

            var tq = new TypeSlimEqualityComparer();
            var eq = new MemberInfoSlimEqualityComparer(() => new NotSupportedOverride(tq));

            foreach (var t in ts)
            {
                var m1 = new NotSupportedMemberInfoSlim(t);
                var m2 = new NotSupportedMemberInfoSlim(t);

                Assert.AreEqual(42, eq.GetHashCode(m1));
                Assert.IsTrue(eq.Equals(m1, m2));
            }
        }

        private sealed class NotSupportedMemberInfoSlim : MemberInfoSlim
        {
            public NotSupportedMemberInfoSlim(MemberTypes types)
                : base(typeof(int).ToTypeSlim())
            {
                MemberType = types;
            }

            public override MemberTypes MemberType { get; }
        }

        private sealed class NotSupportedOverride : MemberInfoSlimEqualityComparator
        {
            public NotSupportedOverride(TypeSlimEqualityComparer tq)
                : base(tq)
            {
            }

            protected override bool EqualsCustom(MemberInfoSlim x, MemberInfoSlim y) => true;

            protected override bool EqualsEvent(MemberInfoSlim x, MemberInfoSlim y) => true;

            protected override bool EqualsExtension(MemberInfoSlim x, MemberInfoSlim y) => true;

            protected override bool EqualsNestedType(MemberInfoSlim x, MemberInfoSlim y) => true;

            protected override bool EqualsTypeInfo(MemberInfoSlim x, MemberInfoSlim y) => true;

            protected override int GetHashCodeCustom(MemberInfoSlim obj) => 42;

            protected override int GetHashCodeEvent(MemberInfoSlim obj) => 42;

            protected override int GetHashCodeExtension(MemberInfoSlim obj) => 42;

            protected override int GetHashCodeNestedType(MemberInfoSlim obj) => 42;

            protected override int GetHashCodeTypeInfo(MemberInfoSlim obj) => 42;
        }
    }
}
