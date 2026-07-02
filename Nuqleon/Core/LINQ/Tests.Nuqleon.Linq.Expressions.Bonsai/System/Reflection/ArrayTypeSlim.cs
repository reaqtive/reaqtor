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
    public class ArrayTypeSlimTests : TestBase
    {
        [TestMethod]
        public void ArrayTypeSlim_ArgumentChecks()
        {
            var ex = Assert.ThrowsExactly<ArgumentNullException>(() => TypeSlim.Array(elementType: null));
            Assert.AreEqual("elementType", ex.ParamName);
            var ex2 = Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => TypeSlim.Array(SlimType, -1));
            Assert.AreEqual("rank", ex2.ParamName);
        }
    }
}
