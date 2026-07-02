// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - July 2013 - Created this file.
//

using System;
using System.Linq;
using System.Reflection;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.System.Linq.Expressions.Bonsai
{
    [TestClass]
    public class InvertedTypeSpaceTests : TestBase
    {
        [TestMethod]
        public void InvertedTypeSpace_ArgumentChecks()
        {
            var ts = new InvertedTypeSpace();
            var ex = Assert.ThrowsExactly<ArgumentNullException>(() => ts.GetMember(memberSlim: null));
            Assert.AreEqual(ex.ParamName, "memberSlim");
            var ex2 = Assert.ThrowsExactly<ArgumentNullException>(() => ts.GetConstructor(constructorSlim: null));
            Assert.AreEqual(ex2.ParamName, "constructorSlim");
            var ex3 = Assert.ThrowsExactly<ArgumentNullException>(() => ts.GetField(fieldSlim: null));
            Assert.AreEqual(ex3.ParamName, "fieldSlim");
            var ex4 = Assert.ThrowsExactly<ArgumentNullException>(() => ts.GetMethod(methodSlim: null));
            Assert.AreEqual(ex4.ParamName, "methodSlim");
            var ex5 = Assert.ThrowsExactly<ArgumentNullException>(() => ts.GetProperty(propertySlim: null));
            Assert.AreEqual(ex5.ParamName, "propertySlim");
            var ex6 = Assert.ThrowsExactly<ArgumentNullException>(() => ts.ConvertType(type: null));
            Assert.AreEqual(ex6.ParamName, "type");
            var ex7 = Assert.ThrowsExactly<ArgumentNullException>(() => ts.MapType(typeSlim: null, type: null));
            Assert.AreEqual(ex7.ParamName, "typeSlim");
            var ex8 = Assert.ThrowsExactly<ArgumentNullException>(() => ts.MapType(SlimType, type: null));
            Assert.AreEqual(ex8.ParamName, "type");
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

            var fooGeneric = fooGenericDef.MakeGenericMethod([typeof(int)]);
            var quxGeneric = quxGenericDef.MakeGenericMethod([typeof(int)]);
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
            Assert.ThrowsExactly<InvalidOperationException>(() => MemberInfoRoundtrip(ctor, ts, its));

            var field = typeof(Foo).GetField("baz");
            Assert.ThrowsExactly<InvalidOperationException>(() => MemberInfoRoundtrip(field, ts, its));

            var prop = typeof(Foo).GetProperty("Bar");
            Assert.ThrowsExactly<InvalidOperationException>(() => MemberInfoRoundtrip(prop, ts, its));

            var idxProp = typeof(Foo).GetProperty("Item");
            Assert.ThrowsExactly<InvalidOperationException>(() => MemberInfoRoundtrip(idxProp, ts, its));

            var simple = typeof(Foo).GetMethod("Qux1");
            Assert.ThrowsExactly<InvalidOperationException>(() => MemberInfoRoundtrip(simple, ts, its));

            var genericDef = typeof(Foo).GetMethod("Qux2");
            Assert.ThrowsExactly<InvalidOperationException>(() => MemberInfoRoundtrip(genericDef, ts, its));

            var generic = genericDef.MakeGenericMethod([typeof(int)]);
            Assert.ThrowsExactly<InvalidOperationException>(() => MemberInfoRoundtrip(generic, ts, its));
        }
    }
}
