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
using Tests.System.Linq.Expressions.Bonsai;

namespace Tests.System.Reflection
{
    [TestClass]
    public class TypeSpaceTests : TestBase
    {
        [TestMethod]
        public void TypeSpace_ArgumentChecks()
        {
            var ts = new TypeSpace();
            AssertEx.ThrowsException<ArgumentNullException>(() => ts.GetMember(member: null), ex => Assert.AreEqual(ex.ParamName, "member"));
            AssertEx.ThrowsException<ArgumentNullException>(() => ts.GetConstructor(constructor: null), ex => Assert.AreEqual(ex.ParamName, "constructor"));
            AssertEx.ThrowsException<ArgumentNullException>(() => ts.GetField(field: null), ex => Assert.AreEqual(ex.ParamName, "field"));
            AssertEx.ThrowsException<ArgumentNullException>(() => ts.GetMethod(method: null), ex => Assert.AreEqual(ex.ParamName, "method"));
            AssertEx.ThrowsException<ArgumentNullException>(() => ts.GetProperty(property: null), ex => Assert.AreEqual(ex.ParamName, "property"));
            AssertEx.ThrowsException<ArgumentNullException>(() => ts.ConvertType(type: null), ex => Assert.AreEqual(ex.ParamName, "type"));
            AssertEx.ThrowsException<ArgumentNullException>(() => ts.MapType(type: null, typeSlim: null), ex => Assert.AreEqual(ex.ParamName, "type"));
            AssertEx.ThrowsException<ArgumentNullException>(() => ts.MapType(typeof(int), typeSlim: null), ex => Assert.AreEqual(ex.ParamName, "typeSlim"));
        }

        [TestMethod]
        public void TypeSpace_Resolve()
        {
            var ts = new TypeSpace();
            var longSlim = ts.ConvertType(typeof(long));
            ts.MapType(typeof(int), longSlim);
            Assert.AreEqual(ts.ConvertType(typeof(int)), longSlim);
        }

        [TestMethod]
        public void TypeSpace_MemberRoundtrip()
        {
            var ctor = typeof(Foo).GetConstructors().Single();
            var ctorRt = MemberInfoRoundtrip(ctor);
            Assert.AreEqual(ctor, ctorRt);

            var field = typeof(Foo).GetField("baz");
            var fieldRt = MemberInfoRoundtrip(field);
            Assert.AreEqual(field, fieldRt);

            var prop = typeof(Foo).GetProperty("Bar");
            var propRt = MemberInfoRoundtrip(prop);
            Assert.AreEqual(prop, propRt);

            var idxProp = typeof(Foo).GetProperty("Item");
            var idxPropRt = MemberInfoRoundtrip(idxProp);
            Assert.AreEqual(idxProp, idxPropRt);

            var simple = typeof(Foo).GetMethod("Qux1");
            var simpleRt = MemberInfoRoundtrip(simple);
            Assert.AreEqual(simple, simpleRt);

            var genericDef = typeof(Foo).GetMethod("Qux2");
            var genericDefRt = MemberInfoRoundtrip(genericDef);
            Assert.IsTrue(genericDef.IsGenericMethodDefinition);
            Assert.AreEqual(genericDef, genericDefRt);

            var generic = genericDef.MakeGenericMethod(new[] { typeof(int) });
            var genericRt = MemberInfoRoundtrip(generic);
            Assert.AreEqual(generic, genericRt);
        }
    }
}
