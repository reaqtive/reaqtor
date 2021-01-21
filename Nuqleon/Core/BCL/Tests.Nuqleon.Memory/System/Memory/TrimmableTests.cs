// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 07/29/2015 - Initial work on memoization support.
//

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Memory;
using System.Linq;

namespace Tests
{
    [TestClass]
    public class TrimmableTests
    {
        [TestMethod]
        public void Trimmable_Create_ArgumentChecking()
        {
            Assert.ThrowsException<ArgumentNullException>(() => Trimmable.Create<int>(trim: null));
            Assert.ThrowsException<ArgumentNullException>(() => Trimmable.Create<int>(_ => 0).Trim(shouldTrim: null));
        }

        [TestMethod]
        public void Trimmable_Create()
        {
            var values = new List<int>();

            var res = Trimmable.Create<int>(shouldTrim =>
            {
                return values.RemoveAll(x => shouldTrim(x));
            });

            values.Add(1);
            values.Add(2);
            values.Add(3);
            values.Add(4);

            Assert.AreEqual(2, res.Trim(x => x % 2 == 0));
            Assert.AreEqual(2, values.Count);
        }
    }
}
