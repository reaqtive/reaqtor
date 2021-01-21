// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - August 2014 - Created this file.
//

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Reaqtor.Expressions.Core
{
    [TestClass]
    public class AsyncReactiveQbserverTests
    {
        [TestMethod]
        public void AsyncReactiveQbserver_ElementType()
        {
            var q = new MyAsyncReactiveQbserver<int>(null);
            Assert.AreEqual(typeof(int), q.ElementType);
        }
    }
}
