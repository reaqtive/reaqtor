﻿// Licensed to the .NET Foundation under one or more agreements.
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
    public class ConstructorInfoSlimTests : TestBase
    {
        [TestMethod]
        public void ConstructorInfoSlim_ArgumentChecks()
        {
            AssertEx.ThrowsException<ArgumentNullException>(() => TypeSlimExtensions.GetConstructor(type: null, Empty), ex => Assert.AreEqual("type", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => SlimType.GetConstructor(parameterTypes: null), ex => Assert.AreEqual("parameterTypes", ex.ParamName));
        }
    }
}
