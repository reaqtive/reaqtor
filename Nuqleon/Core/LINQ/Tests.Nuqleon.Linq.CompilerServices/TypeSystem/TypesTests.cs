// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - May 2013 - Created this file.
//

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq.CompilerServices;

namespace Tests.System.Linq.CompilerServices
{
    [TestClass]
    public class TypesTests
    {
        [TestMethod]
        public void Types_TopBot_Laws()
        {
            Assert.AreEqual("Top", Types.Top.ToString());
            Assert.AreEqual("Bot", Types.Bottom.ToString());

            Assert.AreEqual(Types.Top, Types.Top);
            Assert.AreEqual(Types.Bottom, Types.Bottom);
            Assert.AreNotEqual(Types.Top, Types.Bottom);
            Assert.AreNotEqual(Types.Bottom, Types.Top);

            Assert.IsTrue(Types.Top.Equals(Types.Top));
            Assert.IsTrue(Types.Bottom.Equals(Types.Bottom));
            Assert.IsFalse(Types.Top.Equals(Types.Bottom));
            Assert.IsFalse(Types.Bottom.Equals(Types.Top));

            Assert.IsTrue(Types.Top.IsAssignableTo(Types.Top));
            Assert.IsTrue(Types.Bottom.IsAssignableTo(Types.Bottom));

            Assert.IsTrue(Types.Bottom.IsAssignableTo(Types.Top));
            Assert.IsFalse(Types.Top.IsAssignableTo(Types.Bottom));
        }
    }
}
