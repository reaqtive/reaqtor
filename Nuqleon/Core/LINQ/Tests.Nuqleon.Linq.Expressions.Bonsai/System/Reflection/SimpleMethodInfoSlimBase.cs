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
    public class SimpleMethodInfoSlimBaseTests : TestBase
    {
        [TestMethod]
        public void SimpleMethodInfoSlimBase_ArgumentChecks()
        {
            AssertEx.ThrowsException<ArgumentNullException>(() => TypeSlimExtensions.GetSimpleMethod(type: null, "bar", parameterTypes: null, returnType: null), ex => Assert.AreEqual("type", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => SlimType.GetSimpleMethod(name: null, parameterTypes: null, returnType: null), ex => Assert.AreEqual("name", ex.ParamName));
            AssertEx.ThrowsException<ArgumentException>(() => SlimType.GetSimpleMethod("", parameterTypes: null, returnType: null), ex => Assert.AreEqual("name", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => SlimType.GetSimpleMethod("foo", parameterTypes: null, returnType: null), ex => Assert.AreEqual("parameterTypes", ex.ParamName));

            var m = SlimType.GetSimpleMethod("foo", Empty, returnType: null);
        }
    }
}
