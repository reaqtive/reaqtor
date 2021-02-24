// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq.CompilerServices;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.System.Linq.CompilerServices
{
    [TestClass]
    public class InvariantTests
    {
        [TestMethod]
        public void Invariant_Assert()
        {
            Invariant.Assert(true, "Okay!");

#if !DEBUG
            Assert.ThrowsException<InvalidOperationException>(() => Invariant.Assert(false, "Oops!"));
#endif
        }

        [TestMethod]
        public void Invariant_Unreachable()
        {
            var ex1 = Invariant.Unreachable;
            var ex2 = Invariant.Unreachable;

            Assert.IsInstanceOfType(ex1, typeof(InvalidOperationException));
            Assert.IsInstanceOfType(ex2, typeof(InvalidOperationException));

            Assert.AreNotSame(ex1, ex2);
        }
    }
}
