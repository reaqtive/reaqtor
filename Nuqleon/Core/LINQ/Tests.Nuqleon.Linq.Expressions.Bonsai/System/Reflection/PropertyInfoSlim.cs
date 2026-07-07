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
public class PropertyInfoSlimTests : TestBase
{
    [TestMethod]
    public void PropertyInfoSlim_ArgumentChecks()
    {
        var ex = Assert.ThrowsExactly<ArgumentNullException>(() => TypeSlimExtensions.GetProperty(type: null, "bar", propertyType: null, indexParameterTypes: null, canWrite: false));
        Assert.AreEqual("type", ex.ParamName);
        var ex2 = Assert.ThrowsExactly<ArgumentNullException>(() => SlimType.GetProperty(name: null, propertyType: null, indexParameterTypes: null, canWrite: false));
        Assert.AreEqual("name", ex2.ParamName);
        var ex3 = Assert.ThrowsExactly<ArgumentException>(() => SlimType.GetProperty("", propertyType: null, indexParameterTypes: null, canWrite: false));
        Assert.AreEqual("name", ex3.ParamName);
        var ex4 = Assert.ThrowsExactly<ArgumentNullException>(() => SlimType.GetProperty("foo", propertyType: null, indexParameterTypes: null, canWrite: false));
        Assert.AreEqual("indexParameterTypes", ex4.ParamName);
    }
}
