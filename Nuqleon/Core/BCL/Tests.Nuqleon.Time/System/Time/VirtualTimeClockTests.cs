// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 07/29/2015 - Wrote these tests.
//

using System;
using System.Time;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class VirtualTimeClockTests
    {
        [TestMethod]
        public void VirtualTimeClock_ArgumentChecking()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => _ = new VirtualTimeClock(-1));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => _ = new VirtualTimeClock().Now = -1);
        }

        [TestMethod]
        public void VirtualTimeClock_Assembly()
        {
            Type type = typeof(VirtualTimeClock);
            string assembly = type.Assembly.GetName().Name;
            Assert.AreEqual("Nuqleon.Time", assembly);
        }

        [TestMethod]
        public void VirtualTimeClock_Simple1()
        {
            var c = new VirtualTimeClock();

            Assert.AreEqual(0, c.Now);

            c.Now += 20;

            Assert.AreEqual(20, c.Now);

            c.Now -= 10;

            Assert.AreEqual(10, c.Now);
        }

        [TestMethod]
        public void VirtualTimeClock_Simple2()
        {
            var c = new VirtualTimeClock(10);

            Assert.AreEqual(10, c.Now);

            c.Now += 20;

            Assert.AreEqual(30, c.Now);

            c.Now -= 10;

            Assert.AreEqual(20, c.Now);
        }
    }
}
