// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 07/29/2015 - Wrote these tests.
//

using System;
using System.Collections;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public partial class TupletTests
    {
        [TestMethod]
        public void Tuplet_TRest_NotValid()
        {
            Assert.ThrowsException<ArgumentException>(() => _ = new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0));
        }

        [TestMethod]
        public void Tuplet_Nested_ToString()
        {
            var args =
                new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int>>>(
                    1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16,
                    new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int>>(
                        17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32,
                        new Tuplet<int>(
                            33
                        )
                    )
                );

            Assert.AreEqual("(" + string.Join(", ", Enumerable.Range(1, 33)) + ")", args.ToString());
        }
    }

    internal sealed class ConstantHashEqualityComparer : IEqualityComparer
    {
        private readonly int _hashCode;

        public ConstantHashEqualityComparer(int hashCode) => _hashCode = hashCode;

        bool IEqualityComparer.Equals(object x, object y) => true;

        int IEqualityComparer.GetHashCode(object obj) => _hashCode;
    }
}
