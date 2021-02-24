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
using System.Collections.Generic;
using System.Linq.CompilerServices;
using System.Reflection;
using Tests.System.Linq.Expressions.Bonsai;

namespace Tests.System.Reflection
{
    [TestClass]
    public class GenericMethodInfoSlimTests : TestBase
    {
        [TestMethod]
        public void GenericMethodInfoSlim_ArgumentChecks()
        {
            AssertEx.ThrowsException<ArgumentNullException>(() => TypeSlimExtensions.GetGenericMethod(type: null, methodDefinition: null, arguments: null), ex => Assert.AreEqual("type", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => SlimType.GetGenericMethod(methodDefinition: null, arguments: null), ex => Assert.AreEqual("methodDefinition", ex.ParamName));

            var def = SlimType.GetGenericDefinitionMethod("Foo", new List<TypeSlim> { SlimType }.AsReadOnly(), EmptyReadOnlyCollection<TypeSlim>.Instance, returnType: null);
            AssertEx.ThrowsException<ArgumentNullException>(() => def.DeclaringType.GetGenericMethod(def, arguments: null), ex => Assert.AreEqual("arguments", ex.ParamName));
        }
    }
}
