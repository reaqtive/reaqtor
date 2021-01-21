// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class TypeUtilsTests
    {
        [TestMethod]
        public void IsNullableType()
        {
            Assert.IsFalse(typeof(int).IsNullableType());
            Assert.IsFalse(typeof(Nullable<>).IsNullableType());
            Assert.IsTrue(typeof(int?).IsNullableType());
        }

        [TestMethod]
        public void GetNonNullableType()
        {
            Assert.AreEqual(typeof(int), typeof(int).GetNonNullableType());
            Assert.AreEqual(typeof(int), typeof(int?).GetNonNullableType());
            Assert.AreEqual(typeof(string), typeof(string).GetNonNullableType());
        }

        [TestMethod]
        public void AreEquivalent()
        {
            Assert.IsTrue(TypeUtils.AreEquivalent(typeof(int), typeof(int)));
            Assert.IsFalse(TypeUtils.AreEquivalent(typeof(int), typeof(string)));
        }
    }
}
