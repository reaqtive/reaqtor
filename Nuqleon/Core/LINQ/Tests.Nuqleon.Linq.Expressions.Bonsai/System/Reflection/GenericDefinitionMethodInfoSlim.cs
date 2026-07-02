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
    public class GenericDefinitionMethodInfoSlimTests : TestBase
    {
        [TestMethod]
        public void GenericDefinitionMethodInfoSlim_ArgumentChecks()
        {
            var ex = Assert.ThrowsExactly<ArgumentNullException>(() => TypeSlimExtensions.GetGenericDefinitionMethod(type: null, "bar", genericParameterTypes: null, parameterTypes: null, returnType: null));
            Assert.AreEqual("type", ex.ParamName);
            var ex2 = Assert.ThrowsExactly<ArgumentNullException>(() => SlimType.GetGenericDefinitionMethod(name: null, genericParameterTypes: null, parameterTypes: null, returnType: null));
            Assert.AreEqual("name", ex2.ParamName);
            var ex3 = Assert.ThrowsExactly<ArgumentException>(() => SlimType.GetGenericDefinitionMethod("", genericParameterTypes: null, parameterTypes: null, returnType: null));
            Assert.AreEqual("name", ex3.ParamName);
            var ex4 = Assert.ThrowsExactly<ArgumentNullException>(() => SlimType.GetGenericDefinitionMethod("foo", genericParameterTypes: null, parameterTypes: null, returnType: null));
            Assert.AreEqual("parameterTypes", ex4.ParamName);
            var ex5 = Assert.ThrowsExactly<ArgumentNullException>(() => SlimType.GetGenericDefinitionMethod("foo", genericParameterTypes: null, Empty, returnType: null));
            Assert.AreEqual("genericParameterTypes", ex5.ParamName);
            var ex6 = Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => SlimType.GetGenericDefinitionMethod("foo", Empty, Empty, returnType: null));
            Assert.AreEqual("genericParameterTypes", ex6.ParamName);
        }
    }
}
