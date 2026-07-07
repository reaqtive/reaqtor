// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - July 2013 - Created this file.
//

using System.Reflection;

using Tests.System.Linq.Expressions.Bonsai;

namespace Tests.System.Reflection;

[TestClass]
public class TypeSpaceTests : TestBase
{
    [TestMethod]
    public void TypeSpace_ArgumentChecks()
    {
        var ts = new TypeSpace();
        var ex = Assert.ThrowsExactly<ArgumentNullException>(() => ts.GetMember(member: null));
        Assert.AreEqual("member", ex.ParamName);
        var ex2 = Assert.ThrowsExactly<ArgumentNullException>(() => ts.GetConstructor(constructor: null));
        Assert.AreEqual("constructor", ex2.ParamName);
        var ex3 = Assert.ThrowsExactly<ArgumentNullException>(() => ts.GetField(field: null));
        Assert.AreEqual("field", ex3.ParamName);
        var ex4 = Assert.ThrowsExactly<ArgumentNullException>(() => ts.GetMethod(method: null));
        Assert.AreEqual("method", ex4.ParamName);
        var ex5 = Assert.ThrowsExactly<ArgumentNullException>(() => ts.GetProperty(property: null));
        Assert.AreEqual("property", ex5.ParamName);
        var ex6 = Assert.ThrowsExactly<ArgumentNullException>(() => ts.ConvertType(type: null));
        Assert.AreEqual("type", ex6.ParamName);
        var ex7 = Assert.ThrowsExactly<ArgumentNullException>(() => ts.MapType(type: null, typeSlim: null));
        Assert.AreEqual("type", ex7.ParamName);
        var ex8 = Assert.ThrowsExactly<ArgumentNullException>(() => ts.MapType(typeof(int), typeSlim: null));
        Assert.AreEqual("typeSlim", ex8.ParamName);
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

        var generic = genericDef.MakeGenericMethod([typeof(int)]);
        var genericRt = MemberInfoRoundtrip(generic);
        Assert.AreEqual(generic, genericRt);
    }
}
