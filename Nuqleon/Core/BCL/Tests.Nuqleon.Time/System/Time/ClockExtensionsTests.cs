// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 07/29/2015 - Wrote these tests.
//

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Time;

namespace Tests
{
    [TestClass]
    public class ClockExtensionsTests
    {
        [TestMethod]
        public void ClockExtensions_ArgumentChecking()
        {
            Assert.ThrowsException<ArgumentNullException>(() => ClockExtensions.AssertMonotonic(clock: null));
            Assert.ThrowsException<ArgumentNullException>(() => ClockExtensions.EnsureMonotonic(clock: null));
        }

        [TestMethod]
        public void ClockExtensions_AssertMonotonic()
        {
            var c = new VirtualTimeClock();

            var m = c.AssertMonotonic();

            Assert.AreEqual(c.Now, m.Now);
            Assert.AreEqual(c.Now, m.Now);

            c.Now += 10;

            Assert.AreEqual(c.Now, m.Now);
            Assert.AreEqual(c.Now, m.Now);

            c.Now += 20;

            Assert.AreEqual(c.Now, m.Now);
            Assert.AreEqual(c.Now, m.Now);

            c.Now -= 5;

            Assert.ThrowsException<InvalidOperationException>(() => m.Now);

            c.Now += 5;

            Assert.AreEqual(c.Now, m.Now);
            Assert.AreEqual(c.Now, m.Now);
        }

        [TestMethod]
        public void ClockExtensions_EnsureMonotonic()
        {
            var c = new VirtualTimeClock();

            var m = c.EnsureMonotonic();

            Assert.AreEqual(c.Now, m.Now);
            Assert.AreEqual(c.Now, m.Now);

            c.Now += 10;

            Assert.AreEqual(c.Now, m.Now);
            Assert.AreEqual(c.Now, m.Now);

            c.Now += 20;

            Assert.AreEqual(c.Now, m.Now);
            Assert.AreEqual(c.Now, m.Now);

            c.Now -= 5;

            Assert.AreEqual(30, m.Now);
            Assert.AreEqual(30, m.Now);

            c.Now += 5;

            Assert.AreEqual(c.Now, m.Now);
            Assert.AreEqual(c.Now, m.Now);
        }
    }
}
