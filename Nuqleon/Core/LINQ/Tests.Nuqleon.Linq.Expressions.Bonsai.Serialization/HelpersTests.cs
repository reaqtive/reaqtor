// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - December 2016 - Created this file.
//

using System;
using System.Linq.Expressions.Bonsai.Serialization;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class HelpersTests
    {
        [TestMethod]
        public void ParseInt32()
        {
            var xs = new int[]
            {
                int.MinValue, int.MinValue + 1, int.MinValue + 2,
                -1234567890,
                -234567890,
                -23456789,
                -3456789,
                -345678,
                -45678,
                -4567,
                -567,
                -56,
                -9, -8, -7, -6, -5, -4, -3, -2, -1,
                0,
                1, 2, 3, 4, 5, 6, 7, 8, 9,
                54,
                654,
                6543,
                76543,
                765432,
                8765432,
                87654321,
                987654321,
                int.MaxValue - 2, int.MaxValue - 1, int.MaxValue,
            };

            foreach (var x in xs)
            {
                Assert.AreEqual(x, Helpers.ParseInt32(x.ToString()));
            }
        }

        [TestMethod]
        public void ParseInt32_Fail()
        {
            var ss = new[]
            {
                "",
                "-",
                "--1",
                " 12",
                "12 ",
                "1 2",
                ((char)('0' - 1)).ToString(),
                "0" + ((char)('0' - 1)).ToString(),
                "1" + ((char)('0' - 1)).ToString(),
                ((char)('9' + 1)).ToString(),
                "8" + ((char)('9' + 1)).ToString(),
                "9" + ((char)('9' + 1)).ToString(),
                ((long)int.MinValue - 1).ToString(),
                ((long)int.MaxValue + 1).ToString(),
                "2147483651",
                "-2147483651",
            };

            foreach (var s in ss)
            {
                Assert.ThrowsException<FormatException>(() => Helpers.ParseInt32(s));
            }
        }
    }
}
