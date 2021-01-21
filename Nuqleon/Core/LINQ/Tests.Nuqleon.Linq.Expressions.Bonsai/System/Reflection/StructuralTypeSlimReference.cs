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
using System.Linq.CompilerServices;
using System.Reflection;
using Tests.System.Linq.Expressions.Bonsai;

namespace Tests.System.Reflection
{
    [TestClass]
    public class StructuralTypeSlimReferenceTests : TestBase
    {
        [TestMethod]
        public void StructuralTypeSlimReference_AddProperty_NullArgument()
        {
            var t = StructuralTypeSlimReference.Create(hasValueEqualitySemantics: true, StructuralTypeSlimKind.Record);
            AssertEx.ThrowsException<ArgumentNullException>(() => t.AddProperty(property: null), ex => Assert.AreEqual("property", ex.ParamName));
        }

        [TestMethod]
        public void StructuralTypeSlimReference_AddPropertyAfterFreeze_Fail()
        {
            var t = StructuralTypeSlimReference.Create(hasValueEqualitySemantics: true, StructuralTypeSlimKind.Record);
            t.Freeze();
            var p = t.GetProperty("Foo", propertyType: null, EmptyReadOnlyCollection<TypeSlim>.Instance, canWrite: true);
            Assert.ThrowsException<InvalidOperationException>(() => t.AddProperty(p));
        }
    }
}
