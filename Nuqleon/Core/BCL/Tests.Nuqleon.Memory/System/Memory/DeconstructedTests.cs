// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   ER - 10/27/2014 - Wrote these tests.
//

using System.Memory;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.System.Memory
{
    [TestClass]
    public partial class DeconstructedTests
    {
        [TestMethod]
        public void Deconstructed_1Cached_Equality()
        {
            var d1 = Deconstructed.Create(1, 2);
            var d2 = Deconstructed.Create(1, 2);
            var d3 = Deconstructed.Create(int.MaxValue, 2);
            var d4 = Deconstructed.Create(1, int.MaxValue);

            Assert.IsTrue(d1 == d2);
            Assert.IsTrue(d1.Equals((object)d2));
            Assert.IsFalse(d1.Equals(new object()));
            Assert.IsTrue(d1 != d3);
            Assert.IsFalse(d1 == d3);
            Assert.IsFalse(d1 == d4);

            Assert.AreEqual(d1.GetHashCode(), d2.GetHashCode());
        }
    }
}
