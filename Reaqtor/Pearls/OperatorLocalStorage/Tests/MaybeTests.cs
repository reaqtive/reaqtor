// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2018
//

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Reaqtive.Storage;

namespace Tests
{
    [TestClass]
    public class MaybeTests
    {
        [TestMethod]
        public void WithValue()
        {
            var value = new Maybe<int>(42);

            Assert.IsTrue(value.HasValue);
            Assert.AreEqual(42, value.Value);
        }

        [TestMethod]
        public void WithoutValue()
        {
            var value = new Maybe<int>();

            Assert.IsFalse(value.HasValue);
            Assert.AreEqual(0, value.Value);
        }
    }
}
