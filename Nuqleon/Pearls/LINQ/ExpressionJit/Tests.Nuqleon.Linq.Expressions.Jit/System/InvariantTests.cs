// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using MsTest = Microsoft.VisualStudio.TestTools.UnitTesting;

namespace System
{
    [TestClass]
    public class InvariantTests
    {
        [TestMethod]
        public void Assert()
        {
            MsTest.Assert.ThrowsException<InvalidOperationException>(() => Invariant.Assert(false));
        }

        [TestMethod]
        public void Unreachable()
        {
            MsTest.Assert.ThrowsException<InvalidOperationException>(() => { throw Invariant.Unreachable; });
        }
    }
}
