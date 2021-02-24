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
    public class ArrayTypeSlimTests : TestBase
    {
        [TestMethod]
        public void ArrayTypeSlim_ArgumentChecks()
        {
            AssertEx.ThrowsException<ArgumentNullException>(() => TypeSlim.Array(elementType: null), ex => Assert.AreEqual("elementType", ex.ParamName));
            AssertEx.ThrowsException<ArgumentOutOfRangeException>(() => TypeSlim.Array(SlimType, -1), ex => Assert.AreEqual("rank", ex.ParamName));
        }
    }
}
