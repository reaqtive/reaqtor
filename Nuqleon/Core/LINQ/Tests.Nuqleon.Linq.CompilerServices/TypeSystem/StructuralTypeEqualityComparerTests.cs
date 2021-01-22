// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - September 2013 - Created this file.
//

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq.CompilerServices;

namespace Tests.System.Linq.CompilerServices.TypeSystem
{
    [TestClass]
    public class StructuralTypeEqualityComparerTests
    {
        [TestMethod]
        public void StructuralTypeEqualityComparer_NullEquals()
        {
            var eq = new StructuralTypeEqualityComparer();

            Assert.IsTrue(eq.Equals(null, null));
            Assert.IsFalse(eq.Equals(null, typeof(int)));
            Assert.IsFalse(eq.Equals(typeof(int), null));
        }

        [TestMethod]
        public void StructuralTypeEqualityComparer_StructuralEquals()
        {
            var rt1 = RuntimeCompiler.CreateRecordType(new Dictionary<string, Type>
            {
                { "Foo", typeof(int) },
                { "Bar", typeof(string) }
            },
            true);

            var rt2 = RuntimeCompiler.CreateRecordType(new Dictionary<string, Type>
            {
                { "Foo", typeof(int) },
                { "Bar", typeof(string) },
            },
            true);

            var at1 = RuntimeCompiler.CreateAnonymousType(new Dictionary<string, Type>
            {
                { "Foo", typeof(int) },
                { "Bar", typeof(string) }
            },
            null);

            var at2 = RuntimeCompiler.CreateAnonymousType(new Dictionary<string, Type>
            {
                { "Foo", typeof(int) },
                { "Bar", typeof(string) }
            },
            null);

            var eq = new StructuralTypeEqualityComparer();
            var orig = new TypeEqualityComparer();

            Assert.IsFalse(orig.Equals(rt1, rt2));
            Assert.IsTrue(eq.Equals(rt1, rt2));
            Assert.IsFalse(orig.Equals(at1, at2));
            Assert.IsTrue(eq.Equals(at1, at2));
        }

        [TestMethod]
        public void StructuralTypeEqualityComparer_StructuralNotEquals()
        {
            var rt1 = RuntimeCompiler.CreateRecordType(new Dictionary<string, Type>
            {
                { "Foo", typeof(int) },
            },
            true);

            var rt2 = RuntimeCompiler.CreateRecordType(new Dictionary<string, Type>
            {
                { "Foo", typeof(int) },
                { "Bar", typeof(string) },
            },
            true);

            var rt3 = RuntimeCompiler.CreateRecordType(new Dictionary<string, Type>
            {
                { "Foo", typeof(int) },
                { "Bar", typeof(int) }
            },
            true);

            var at1 = RuntimeCompiler.CreateAnonymousType(new Dictionary<string, Type>
            {
                { "Foo", typeof(int) },
            },
            null);

            var at2 = RuntimeCompiler.CreateAnonymousType(new Dictionary<string, Type>
            {
                { "Foo", typeof(int) },
                { "Bar", typeof(string) }
            },
            null);

            var at3 = RuntimeCompiler.CreateAnonymousType(new Dictionary<string, Type>
            {
                { "Foo", typeof(int) },
                { "Bar", typeof(int) }
            },
            null);

            var eq = new StructuralTypeEqualityComparer();

            Assert.IsFalse(eq.Equals(rt1, rt2));
            Assert.IsFalse(eq.Equals(rt2, rt3));
            Assert.IsFalse(eq.Equals(at1, at2));
            Assert.IsFalse(eq.Equals(at2, at3));
        }

        [TestMethod]
        public void StructuralTypeEqualityComparer_ManOrBoy1_Success()
        {
            var rtc = new RuntimeCompiler();
            var rt1 = rtc.GetNewRecordTypeBuilder();
            var rt2 = rtc.GetNewRecordTypeBuilder();
            var at1 = rtc.GetNewAnonymousTypeBuilder();
            var at2 = rtc.GetNewAnonymousTypeBuilder();

            rtc.DefineRecordType(
                rt1,
                new Dictionary<string, Type>
                {
                    { "Self", rt1 },
                    { "Copy", rt2 },
                    { "Other", at1 }
                },
                true);

            rtc.DefineRecordType(
                rt2,
                new Dictionary<string, Type>
                {
                    { "Self", rt2 },
                    { "Copy", rt1 },
                    { "Other", at2 }
                },
                true);

            rtc.DefineAnonymousType(
                at1,
                new Dictionary<string, Type>
                {
                    { "Self", at1 },
                    { "Copy", at2 },
                    { "Other", rt1 }
                },
                null);

            rtc.DefineAnonymousType(
                at2,
                new Dictionary<string, Type>
                {
                    { "Self", at2 },
                    { "Copy", at1 },
                    { "Other", rt2 }
                },
                null);

            var rt1c = rt1.CreateType();
            var rt2c = rt2.CreateType();
            var at1c = at1.CreateType();
            var at2c = at2.CreateType();

            var eq = new StructuralTypeEqualityComparer();
            var orig = new TypeEqualityComparer();

            Assert.IsFalse(orig.Equals(rt1c, rt2c));
            Assert.IsTrue(eq.Equals(rt1c, rt2c));
            Assert.IsFalse(orig.Equals(at1c, at2c));
            Assert.IsTrue(eq.Equals(at1c, at2c));
        }

        [TestMethod]
        public void StructuralTypeEqualityComparer_ManOrBoy2_Success()
        {
            var rtc = new RuntimeCompiler();
            var rt1 = rtc.GetNewRecordTypeBuilder();
            var rt2 = rtc.GetNewRecordTypeBuilder();
            var at1 = rtc.GetNewAnonymousTypeBuilder();
            var at2 = rtc.GetNewAnonymousTypeBuilder();

            rtc.DefineRecordType(
                rt1,
                new Dictionary<string, Type>
                {
                    { "Self", rt1 },
                    { "Copy", rt2 },
                    { "Other", typeof(int) }
                },
                true);

            rtc.DefineRecordType(
                rt2,
                new Dictionary<string, Type>
                {
                    { "Self", rt2 },
                    { "Copy", rt1 },
                    { "Other", typeof(int) }
                },
                true);

            rtc.DefineAnonymousType(
                at1,
                new Dictionary<string, Type>
                {
                    { "Self", at1 },
                    { "Copy", at2 },
                    { "Other", typeof(int) }
                },
                null);

            rtc.DefineAnonymousType(
                at2,
                new Dictionary<string, Type>
                {
                    { "Self", at2 },
                    { "Copy", at1 },
                    { "Other", typeof(int) }
                },
                null);

            var rt1c = rt1.CreateType();
            var rt2c = rt2.CreateType();
            var at1c = at1.CreateType();
            var at2c = at2.CreateType();

            var eq = new StructuralTypeEqualityComparer();
            var orig = new TypeEqualityComparer();

            Assert.IsFalse(orig.Equals(rt1c, rt2c));
            Assert.IsTrue(eq.Equals(rt1c, rt2c));
            Assert.IsFalse(orig.Equals(at1c, at2c));
            Assert.IsTrue(eq.Equals(at1c, at2c));
        }

        [TestMethod]
        public void StructuralTypeEqualityComparer_ManOrBoy_Failure()
        {
            var rtc = new RuntimeCompiler();
            var rt1 = rtc.GetNewRecordTypeBuilder();
            var rt2 = rtc.GetNewRecordTypeBuilder();
            var at1 = rtc.GetNewAnonymousTypeBuilder();
            var at2 = rtc.GetNewAnonymousTypeBuilder();

            rtc.DefineRecordType(
                rt1,
                new Dictionary<string, Type>
                {
                    { "Self", rt1 },
                    { "Copy", rt2 },
                    { "Other", typeof(int) }
                },
                true);

            rtc.DefineRecordType(
                rt2,
                new Dictionary<string, Type>
                {
                    { "Self", rt2 },
                    { "Copy", rt1 },
                    { "Other", typeof(string) }
                },
                true);

            rtc.DefineAnonymousType(
                at1,
                new Dictionary<string, Type>
                {
                    { "Self", at1 },
                    { "Copy", at2 },
                    { "Other", typeof(int) }
                },
                null);

            rtc.DefineAnonymousType(
                at2,
                new Dictionary<string, Type>
                {
                    { "Self", at2 },
                    { "Copy", at1 },
                    { "Other", typeof(string) }
                },
                null);

            rt1.CreateType();
            rt2.CreateType();
            at1.CreateType();
            at2.CreateType();

            var eq = new StructuralTypeEqualityComparer();

            Assert.IsFalse(eq.Equals(rt1, rt2));
            Assert.IsFalse(eq.Equals(at1, at2));
        }
    }
}
