// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - July 2013 - Created this file.
//

using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Nuqleon.DataModel;

namespace Tests.Nuqleon.DataModel
{
    /// <summary>
    /// Unit tests for the unit type carried out in unity so we stand united on unitary test principles.
    /// </summary>
    [TestClass]
    public class UnitTests
    {
        [TestMethod]
        public void Unit_Equality()
        {
            var u1 = new Unit();
            var u2 = new Unit();

            Assert.AreEqual(u1, u2);
            Assert.IsTrue(u1 == u2);
            Assert.IsTrue(u1.Equals(u2));
            Assert.IsTrue(u1.Equals((object)u2));
            Assert.IsTrue(object.Equals(u1, u2));
            Assert.IsFalse(u1 != u2);

            var d = new HashSet<Unit>
            {
                u1,
                u2
            };

            Assert.AreEqual(1, d.Count);
        }

        [TestMethod]
        public void Unit_ToString()
        {
            Assert.AreEqual("()", new Unit().ToString());
        }
    }
}
