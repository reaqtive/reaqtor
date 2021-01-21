// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2017 - Created this file.
//

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Reflection;

namespace Tests.System.Reflection.Virtualization
{
    [TestClass]
    public class VirtualizationTests
    {
        [TestMethod]
        public void TypeLoadingProviderExtensions_Assembly()
        {
            Type type = typeof(TypeLoadingProviderExtensions);
            string assembly = type.Assembly.GetName().Name;
            Assert.AreEqual("Nuqleon.Reflection.Virtualization", assembly);
        }
    }
}
