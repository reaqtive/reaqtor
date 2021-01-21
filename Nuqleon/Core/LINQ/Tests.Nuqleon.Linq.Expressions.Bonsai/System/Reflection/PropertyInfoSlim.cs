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
    public class PropertyInfoSlimTests : TestBase
    {
        [TestMethod]
        public void PropertyInfoSlim_ArgumentChecks()
        {
            AssertEx.ThrowsException<ArgumentNullException>(() => TypeSlimExtensions.GetProperty(type: null, "bar", propertyType: null, indexParameterTypes: null, canWrite: false), ex => Assert.AreEqual("type", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => SlimType.GetProperty(name: null, propertyType: null, indexParameterTypes: null, canWrite: false), ex => Assert.AreEqual("name", ex.ParamName));
            AssertEx.ThrowsException<ArgumentException>(() => SlimType.GetProperty("", propertyType: null, indexParameterTypes: null, canWrite: false), ex => Assert.AreEqual("name", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => SlimType.GetProperty("foo", propertyType: null, indexParameterTypes: null, canWrite: false), ex => Assert.AreEqual("indexParameterTypes", ex.ParamName));
        }
    }
}
