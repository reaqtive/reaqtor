// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2017 - Created this file.
//

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;

namespace Tests.System.Reflection.Virtualization
{
    [TestClass]
    public class Tests
    {
        [TestMethod]
        public void ManOrBoy()
        {
            var res = DefaultReflectionProvider.Instance.GetBaseType(typeof(string));
            Assert.AreEqual(typeof(object), res);
        }
    }
}
