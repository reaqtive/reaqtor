// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - July 2013 - Created this file.
//

using System;
using System.Collections.Generic;
using System.Linq.CompilerServices;
using System.Reflection;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Tests.System.Linq.Expressions.Bonsai;

namespace Tests.System.Reflection
{
    [TestClass]
    public class GenericMethodInfoSlimTests : TestBase
    {
        [TestMethod]
        public void GenericMethodInfoSlim_ArgumentChecks()
        {
            var ex = Assert.ThrowsExactly<ArgumentNullException>(() => TypeSlimExtensions.GetGenericMethod(type: null, methodDefinition: null, arguments: null));
            Assert.AreEqual("type", ex.ParamName);
            var ex2 = Assert.ThrowsExactly<ArgumentNullException>(() => SlimType.GetGenericMethod(methodDefinition: null, arguments: null));
            Assert.AreEqual("methodDefinition", ex2.ParamName);

            var def = SlimType.GetGenericDefinitionMethod("Foo", new List<TypeSlim> { SlimType }.AsReadOnly(), EmptyReadOnlyCollection<TypeSlim>.Instance, returnType: null);
            var ex3 = Assert.ThrowsExactly<ArgumentNullException>(() => def.DeclaringType.GetGenericMethod(def, arguments: null));
            Assert.AreEqual("arguments", ex3.ParamName);
        }
    }
}
