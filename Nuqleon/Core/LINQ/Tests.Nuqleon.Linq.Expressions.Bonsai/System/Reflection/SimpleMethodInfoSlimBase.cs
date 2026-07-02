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
    public class SimpleMethodInfoSlimBaseTests : TestBase
    {
        [TestMethod]
        public void SimpleMethodInfoSlimBase_ArgumentChecks()
        {
            var ex = Assert.ThrowsExactly<ArgumentNullException>(() => TypeSlimExtensions.GetSimpleMethod(type: null, "bar", parameterTypes: null, returnType: null));
            Assert.AreEqual("type", ex.ParamName);
            var ex2 = Assert.ThrowsExactly<ArgumentNullException>(() => SlimType.GetSimpleMethod(name: null, parameterTypes: null, returnType: null));
            Assert.AreEqual("name", ex2.ParamName);
            var ex3 = Assert.ThrowsExactly<ArgumentException>(() => SlimType.GetSimpleMethod("", parameterTypes: null, returnType: null));
            Assert.AreEqual("name", ex3.ParamName);
            var ex4 = Assert.ThrowsExactly<ArgumentNullException>(() => SlimType.GetSimpleMethod("foo", parameterTypes: null, returnType: null));
            Assert.AreEqual("parameterTypes", ex4.ParamName);

            var m = SlimType.GetSimpleMethod("foo", Empty, returnType: null);
        }
    }
}
