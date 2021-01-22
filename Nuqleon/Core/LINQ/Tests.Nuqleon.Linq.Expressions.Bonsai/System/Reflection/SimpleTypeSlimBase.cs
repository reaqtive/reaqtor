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
using System.Reflection;
using Tests.System.Linq.Expressions.Bonsai;

namespace Tests.System.Reflection
{
    [TestClass]
    public class SimpleTypeSlimBaseTests : TestBase
    {
        [TestMethod]
        public void SimpleTypeSlimBase_ArgumentChecks()
        {
            AssertEx.ThrowsException<ArgumentException>(() => TypeSlim.Simple(assembly: null, name: null), ex => Assert.AreEqual("name", ex.ParamName));
            AssertEx.ThrowsException<ArgumentException>(() => TypeSlim.Simple(assembly: null, ""), ex => Assert.AreEqual("name", ex.ParamName));
        }
    }
}
