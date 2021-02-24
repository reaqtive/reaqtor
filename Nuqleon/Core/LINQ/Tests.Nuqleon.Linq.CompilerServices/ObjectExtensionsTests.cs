// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - May 2013 - Created this file.
//

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Tests.System.Linq.CompilerServices
{
    [TestClass]
    public class ObjectExtensionsTests
    {
        [TestMethod]
        public void ObjectExtensions_Let()
        {
            AssertEx.ThrowsException<ArgumentNullException>(() => 21.Let(default(Func<int, int>)), ex => Assert.AreEqual("function", ex.ParamName));
            Assert.AreEqual(42, 21.Let(x => x * 2));
        }
    }
}
