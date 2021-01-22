// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Memory;

namespace Tests.System.Memory
{
    public partial class DeconstructedTests
    {
        [TestMethod]
        public void Deconstructed_2Cached_Equality()
        {
            var d1 = Deconstructed.Create(1, 2, 3);
            var d2 = Deconstructed.Create(1, 2, 3);
            var d3 = Deconstructed.Create(int.MaxValue, 2, 3);
            var d4 = Deconstructed.Create(1, int.MaxValue, 3);
            var d5 = Deconstructed.Create(1, 2, int.MaxValue);

            Assert.IsTrue(d1 == d2);
            Assert.IsTrue(d1.Equals((object)d2));
            Assert.IsFalse(d1.Equals(new object()));
            Assert.IsTrue(d1 != d3);
            Assert.IsFalse(d1 == d3);
            Assert.IsFalse(d1 == d4);
            Assert.IsFalse(d1 == d5);

            Assert.AreEqual(d1.GetHashCode(), d2.GetHashCode());
        }

        [TestMethod]
        public void Deconstructed_3Cached_Equality()
        {
            var d1 = Deconstructed.Create(1, 2, 3, 4);
            var d2 = Deconstructed.Create(1, 2, 3, 4);
            var d3 = Deconstructed.Create(int.MaxValue, 2, 3, 4);
            var d4 = Deconstructed.Create(1, int.MaxValue, 3, 4);
            var d5 = Deconstructed.Create(1, 2, int.MaxValue, 4);
            var d6 = Deconstructed.Create(1, 2, 3, int.MaxValue);

            Assert.IsTrue(d1 == d2);
            Assert.IsTrue(d1.Equals((object)d2));
            Assert.IsFalse(d1.Equals(new object()));
            Assert.IsTrue(d1 != d3);
            Assert.IsFalse(d1 == d3);
            Assert.IsFalse(d1 == d4);
            Assert.IsFalse(d1 == d5);
            Assert.IsFalse(d1 == d6);

            Assert.AreEqual(d1.GetHashCode(), d2.GetHashCode());
        }

        [TestMethod]
        public void Deconstructed_4Cached_Equality()
        {
            var d1 = Deconstructed.Create(1, 2, 3, 4, 5);
            var d2 = Deconstructed.Create(1, 2, 3, 4, 5);
            var d3 = Deconstructed.Create(int.MaxValue, 2, 3, 4, 5);
            var d4 = Deconstructed.Create(1, int.MaxValue, 3, 4, 5);
            var d5 = Deconstructed.Create(1, 2, int.MaxValue, 4, 5);
            var d6 = Deconstructed.Create(1, 2, 3, int.MaxValue, 5);
            var d7 = Deconstructed.Create(1, 2, 3, 4, int.MaxValue);

            Assert.IsTrue(d1 == d2);
            Assert.IsTrue(d1.Equals((object)d2));
            Assert.IsFalse(d1.Equals(new object()));
            Assert.IsTrue(d1 != d3);
            Assert.IsFalse(d1 == d3);
            Assert.IsFalse(d1 == d4);
            Assert.IsFalse(d1 == d5);
            Assert.IsFalse(d1 == d6);
            Assert.IsFalse(d1 == d7);

            Assert.AreEqual(d1.GetHashCode(), d2.GetHashCode());
        }

    }
}
