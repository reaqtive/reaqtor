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
    public class FieldInfoSlimTests : TestBase
    {
        [TestMethod]
        public void FieldInfoSlim_ArgumentChecks()
        {
            AssertEx.ThrowsException<ArgumentNullException>(() => TypeSlimExtensions.GetField(type: null, "bar", fieldType: null), ex => Assert.AreEqual("type", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => SlimType.GetField(name: null, fieldType: null), ex => Assert.AreEqual("name", ex.ParamName));
            AssertEx.ThrowsException<ArgumentException>(() => SlimType.GetField("", fieldType: null), ex => Assert.AreEqual("name", ex.ParamName));

            var f = SlimType.GetField("value", fieldType: null);
            Assert.IsNull(f.FieldType);
        }
    }
}
