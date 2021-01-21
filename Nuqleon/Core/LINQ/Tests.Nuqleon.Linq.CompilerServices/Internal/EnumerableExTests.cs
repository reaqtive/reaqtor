// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - December 2014 - Created this file.
//

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace Tests.System.Linq.CompilerServices
{
    [TestClass]
    public class EnumerableExTests
    {
        [TestMethod]
        public void EnumerableEx_AsArray()
        {
            {
                var arr = new[] { 1, 2, 3 };
                var res = EnumerableEx.AsArray(arr);
                Assert.AreSame(arr, res);
            }

            {
                var lst = new List<int> { 1, 2, 3 };
                var res = EnumerableEx.AsArray(lst);
                Assert.IsTrue(res.SequenceEqual(lst));
            }
        }
    }
}
