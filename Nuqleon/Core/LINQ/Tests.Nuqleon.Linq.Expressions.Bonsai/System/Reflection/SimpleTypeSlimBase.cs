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
public class SimpleTypeSlimBaseTests : TestBase
{
    [TestMethod]
    public void SimpleTypeSlimBase_ArgumentChecks()
    {
        var ex = Assert.ThrowsExactly<ArgumentException>(() => TypeSlim.Simple(assembly: null, name: null));
        Assert.AreEqual("name", ex.ParamName);
        var ex2 = Assert.ThrowsExactly<ArgumentException>(() => TypeSlim.Simple(assembly: null, ""));
        Assert.AreEqual("name", ex2.ParamName);
    }
}
