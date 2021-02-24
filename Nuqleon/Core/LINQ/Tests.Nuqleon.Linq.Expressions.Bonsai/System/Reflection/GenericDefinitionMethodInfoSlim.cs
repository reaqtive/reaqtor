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
    public class GenericDefinitionMethodInfoSlimTests : TestBase
    {
        [TestMethod]
        public void GenericDefinitionMethodInfoSlim_ArgumentChecks()
        {
            AssertEx.ThrowsException<ArgumentNullException>(() => TypeSlimExtensions.GetGenericDefinitionMethod(type: null, "bar", genericParameterTypes: null, parameterTypes: null, returnType: null), ex => Assert.AreEqual("type", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => SlimType.GetGenericDefinitionMethod(name: null, genericParameterTypes: null, parameterTypes: null, returnType: null), ex => Assert.AreEqual("name", ex.ParamName));
            AssertEx.ThrowsException<ArgumentException>(() => SlimType.GetGenericDefinitionMethod("", genericParameterTypes: null, parameterTypes: null, returnType: null), ex => Assert.AreEqual("name", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => SlimType.GetGenericDefinitionMethod("foo", genericParameterTypes: null, parameterTypes: null, returnType: null), ex => Assert.AreEqual("parameterTypes", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => SlimType.GetGenericDefinitionMethod("foo", genericParameterTypes: null, Empty, returnType: null), ex => Assert.AreEqual("genericParameterTypes", ex.ParamName));
            AssertEx.ThrowsException<ArgumentOutOfRangeException>(() => SlimType.GetGenericDefinitionMethod("foo", Empty, Empty, returnType: null), ex => Assert.AreEqual("genericParameterTypes", ex.ParamName));
        }
    }
}
