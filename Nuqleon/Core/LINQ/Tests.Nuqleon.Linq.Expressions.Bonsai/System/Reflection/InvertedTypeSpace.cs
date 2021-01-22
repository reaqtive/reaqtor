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
using System.Linq;
using System.Reflection;

namespace Tests.System.Linq.Expressions.Bonsai
{
    [TestClass]
    public class InvertedTypeSpaceTests : TestBase
    {
        [TestMethod]
        public void InvertedTypeSpace_ArgumentChecks()
        {
            var ts = new InvertedTypeSpace();
            AssertEx.ThrowsException<ArgumentNullException>(() => ts.GetMember(memberSlim: null), ex => Assert.AreEqual(ex.ParamName, "memberSlim"));
            AssertEx.ThrowsException<ArgumentNullException>(() => ts.GetConstructor(constructorSlim: null), ex => Assert.AreEqual(ex.ParamName, "constructorSlim"));
            AssertEx.ThrowsException<ArgumentNullException>(() => ts.GetField(fieldSlim: null), ex => Assert.AreEqual(ex.ParamName, "fieldSlim"));
            AssertEx.ThrowsException<ArgumentNullException>(() => ts.GetMethod(methodSlim: null), ex => Assert.AreEqual(ex.ParamName, "methodSlim"));
            AssertEx.ThrowsException<ArgumentNullException>(() => ts.GetProperty(propertySlim: null), ex => Assert.AreEqual(ex.ParamName, "propertySlim"));
            AssertEx.ThrowsException<ArgumentNullException>(() => ts.ConvertType(type: null), ex => Assert.AreEqual(ex.ParamName, "type"));
            AssertEx.ThrowsException<ArgumentNullException>(() => ts.MapType(typeSlim: null, type: null), ex => Assert.AreEqual(ex.ParamName, "typeSlim"));
            AssertEx.ThrowsException<ArgumentNullException>(() => ts.MapType(SlimType, type: null), ex => Assert.AreEqual(ex.ParamName, "type"));
        }

        [TestMethod]
        public void InvertedTypeSpace_MapType_Success()
        {
            var ts = new TypeSpace();
            var its = new InvertedTypeSpace();
            var fooSlim = ts.ConvertType(typeof(Foo));
            its.MapType(fooSlim, typeof(Qux));

            var fooCtor = typeof(Foo).GetConstructors().Single();
            var quxCtor = typeof(Qux).GetConstructors().Single();
            Assert.AreEqual(quxCtor, MemberInfoRoundtrip(fooCtor, ts, its));

            var fooField = typeof(Foo).GetField("baz");
            var quxField = typeof(Qux).GetField("baz");
            Assert.AreEqual(quxField, MemberInfoRoundtrip(fooField, ts, its));

            var fooProp = typeof(Foo).GetProperty("Bar");
            var quxProp = typeof(Qux).GetProperty("Bar");
            Assert.AreEqual(quxProp, MemberInfoRoundtrip(fooProp, ts, its));

            var fooIdxProp = typeof(Foo).GetProperty("Item");
            var quxIdxProp = typeof(Qux).GetProperty("Item");
            Assert.AreEqual(quxIdxProp, MemberInfoRoundtrip(fooIdxProp, ts, its));

            var fooSimple = typeof(Foo).GetMethod("Qux1");
            var quxSimple = typeof(Qux).GetMethod("Qux1");
            Assert.AreEqual(quxSimple, MemberInfoRoundtrip(fooSimple, ts, its));

            var fooGenericDef = typeof(Foo).GetMethod("Qux2");
            var quxGenericDef = typeof(Qux).GetMethod("Qux2");
            Assert.AreEqual(quxGenericDef, MemberInfoRoundtrip(fooGenericDef, ts, its));

            var fooGeneric = fooGenericDef.MakeGenericMethod(new[] { typeof(int) });
            var quxGeneric = quxGenericDef.MakeGenericMethod(new[] { typeof(int) });
            Assert.AreEqual(quxGeneric, MemberInfoRoundtrip(fooGeneric, ts, its));
        }

        [TestMethod]
        public void InvertedTypeSpace_MemberRoundtrip_BadMapping_ThrowsInvalidOperation()
        {
            var ts = new TypeSpace();
            var its = new InvertedTypeSpace();
            var fooSlim = ts.ConvertType(typeof(Foo));
            its.MapType(fooSlim, typeof(Bar));

            var ctor = typeof(Foo).GetConstructors().Single();
            Assert.ThrowsException<InvalidOperationException>(() => MemberInfoRoundtrip(ctor, ts, its));

            var field = typeof(Foo).GetField("baz");
            Assert.ThrowsException<InvalidOperationException>(() => MemberInfoRoundtrip(field, ts, its));

            var prop = typeof(Foo).GetProperty("Bar");
            Assert.ThrowsException<InvalidOperationException>(() => MemberInfoRoundtrip(prop, ts, its));

            var idxProp = typeof(Foo).GetProperty("Item");
            Assert.ThrowsException<InvalidOperationException>(() => MemberInfoRoundtrip(idxProp, ts, its));

            var simple = typeof(Foo).GetMethod("Qux1");
            Assert.ThrowsException<InvalidOperationException>(() => MemberInfoRoundtrip(simple, ts, its));

            var genericDef = typeof(Foo).GetMethod("Qux2");
            Assert.ThrowsException<InvalidOperationException>(() => MemberInfoRoundtrip(genericDef, ts, its));

            var generic = genericDef.MakeGenericMethod(new[] { typeof(int) });
            Assert.ThrowsException<InvalidOperationException>(() => MemberInfoRoundtrip(generic, ts, its));
        }
    }
}
