// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - July 2013 - Created this file.
//

using System;
using System.Reflection;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Tests.System.Linq.Expressions.Bonsai;

namespace Tests.System.Reflection;

[TestClass]
public class FieldInfoSlimTests : TestBase
{
    [TestMethod]
    public void FieldInfoSlim_ArgumentChecks()
    {
        var ex = Assert.ThrowsExactly<ArgumentNullException>(() => TypeSlimExtensions.GetField(type: null, "bar", fieldType: null));
        Assert.AreEqual("type", ex.ParamName);
        var ex2 = Assert.ThrowsExactly<ArgumentNullException>(() => SlimType.GetField(name: null, fieldType: null));
        Assert.AreEqual("name", ex2.ParamName);
        var ex3 = Assert.ThrowsExactly<ArgumentException>(() => SlimType.GetField("", fieldType: null));
        Assert.AreEqual("name", ex3.ParamName);

        var f = SlimType.GetField("value", fieldType: null);
        Assert.IsNull(f.FieldType);
    }
}
