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

namespace Tests.System.Reflection
{
    [TestClass]
    public class GenericParameterTypeSlimTests : TestBase
    {
        [TestMethod]
        public void GenericParameterTypeSlim_ArgumentChecks()
        {
            AssertEx.ThrowsException<ArgumentException>(() => TypeSlim.GenericParameter(name: null), ex => Assert.AreEqual("name", ex.ParamName));
            AssertEx.ThrowsException<ArgumentException>(() => TypeSlim.GenericParameter(name: ""), ex => Assert.AreEqual("name", ex.ParamName));
        }
    }
}
