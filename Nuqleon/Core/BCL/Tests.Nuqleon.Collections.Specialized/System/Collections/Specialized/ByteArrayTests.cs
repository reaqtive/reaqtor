// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   PS - 10/17/2014 - Wrote these tests.
//

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Specialized;

namespace Tests.System.Collections.Specialized
{
    [TestClass]
    public class ByteArrayTests
    {
        [TestMethod]
        public void ByteArray_ManOrBoy()
        {
            for (var i = 0; i < 1; i++)
            {
                var arr = default(ByteArray1);
                Assert.AreEqual(arr.Length, 1);
                arr[i] = 0xff;

                for (var j = 0; j < 1; j++)
                {
                    Assert.AreEqual(i == j ? 0xff : 0x00, arr[j]);
                }
            }

            for (var i = 0; i < 2; i++)
            {
                var arr = default(ByteArray2);
                Assert.AreEqual(arr.Length, 2);
                arr[i] = 0xff;

                for (var j = 0; j < 2; j++)
                {
                    Assert.AreEqual(i == j ? 0xff : 0x00, arr[j]);
                }
            }

            for (var i = 0; i < 3; i++)
            {
                var arr = default(ByteArray3);
                Assert.AreEqual(arr.Length, 3);
                arr[i] = 0xff;

                for (var j = 0; j < 3; j++)
                {
                    Assert.AreEqual(i == j ? 0xff : 0x00, arr[j]);
                }
            }

            for (var i = 0; i < 4; i++)
            {
                var arr = default(ByteArray4);
                Assert.AreEqual(arr.Length, 4);
                arr[i] = 0xff;

                for (var j = 0; j < 4; j++)
                {
                    Assert.AreEqual(i == j ? 0xff : 0x00, arr[j]);
                }
            }

            for (var i = 0; i < 5; i++)
            {
                var arr = default(ByteArray5);
                Assert.AreEqual(arr.Length, 5);
                arr[i] = 0xff;

                for (var j = 0; j < 5; j++)
                {
                    Assert.AreEqual(i == j ? 0xff : 0x00, arr[j]);
                }
            }

            for (var i = 0; i < 6; i++)
            {
                var arr = default(ByteArray6);
                Assert.AreEqual(arr.Length, 6);
                arr[i] = 0xff;

                for (var j = 0; j < 6; j++)
                {
                    Assert.AreEqual(i == j ? 0xff : 0x00, arr[j]);
                }
            }

            for (var i = 0; i < 7; i++)
            {
                var arr = default(ByteArray7);
                Assert.AreEqual(arr.Length, 7);
                arr[i] = 0xff;

                for (var j = 0; j < 7; j++)
                {
                    Assert.AreEqual(i == j ? 0xff : 0x00, arr[j]);
                }
            }

            for (var i = 0; i < 8; i++)
            {
                var arr = default(ByteArray8);
                Assert.AreEqual(arr.Length, 8);
                arr[i] = 0xff;

                for (var j = 0; j < 8; j++)
                {
                    Assert.AreEqual(i == j ? 0xff : 0x00, arr[j]);
                }
            }

            for (var i = 0; i < 9; i++)
            {
                var arr = default(ByteArray9);
                Assert.AreEqual(arr.Length, 9);
                arr[i] = 0xff;

                for (var j = 0; j < 9; j++)
                {
                    Assert.AreEqual(i == j ? 0xff : 0x00, arr[j]);
                }
            }

            for (var i = 0; i < 10; i++)
            {
                var arr = default(ByteArray10);
                Assert.AreEqual(arr.Length, 10);
                arr[i] = 0xff;

                for (var j = 0; j < 10; j++)
                {
                    Assert.AreEqual(i == j ? 0xff : 0x00, arr[j]);
                }
            }

            for (var i = 0; i < 11; i++)
            {
                var arr = default(ByteArray11);
                Assert.AreEqual(arr.Length, 11);
                arr[i] = 0xff;

                for (var j = 0; j < 11; j++)
                {
                    Assert.AreEqual(i == j ? 0xff : 0x00, arr[j]);
                }
            }

        }

        [TestMethod]
        public void ByteArray_EdgeCases()
        {
            var arr1 = default(ByteArray1);
            try
            {
                var dummy = arr1[1];
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
            }

            try
            {
                var dummy = arr1[-1];
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
            }

            try
            {
                arr1[1] = 1;
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
            }

            try
            {
                arr1[-1] = 1;
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
            }

            var arr2 = default(ByteArray2);
            try
            {
                var dummy = arr2[2];
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
            }

            try
            {
                var dummy = arr2[-1];
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
            }

            try
            {
                arr2[2] = 1;
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
            }

            try
            {
                arr2[-1] = 1;
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
            }

            var arr3 = default(ByteArray3);
            try
            {
                var dummy = arr3[3];
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
            }

            try
            {
                var dummy = arr3[-1];
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
            }

            try
            {
                arr3[3] = 1;
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
            }

            try
            {
                arr3[-1] = 1;
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
            }

            var arr4 = default(ByteArray4);
            try
            {
                var dummy = arr4[4];
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
            }

            try
            {
                var dummy = arr4[-1];
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
            }

            try
            {
                arr4[4] = 1;
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
            }

            try
            {
                arr4[-1] = 1;
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
            }

            var arr5 = default(ByteArray5);
            try
            {
                var dummy = arr5[5];
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
            }

            try
            {
                var dummy = arr5[-1];
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
            }

            try
            {
                arr5[5] = 1;
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
            }

            try
            {
                arr5[-1] = 1;
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
            }

            var arr6 = default(ByteArray6);
            try
            {
                var dummy = arr6[6];
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
            }

            try
            {
                var dummy = arr6[-1];
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
            }

            try
            {
                arr6[6] = 1;
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
            }

            try
            {
                arr6[-1] = 1;
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
            }

            var arr7 = default(ByteArray7);
            try
            {
                var dummy = arr7[7];
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
            }

            try
            {
                var dummy = arr7[-1];
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
            }

            try
            {
                arr7[7] = 1;
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
            }

            try
            {
                arr7[-1] = 1;
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
            }

            var arr8 = default(ByteArray8);
            try
            {
                var dummy = arr8[8];
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
            }

            try
            {
                var dummy = arr8[-1];
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
            }

            try
            {
                arr8[8] = 1;
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
            }

            try
            {
                arr8[-1] = 1;
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
            }

            var arr9 = default(ByteArray9);
            try
            {
                var dummy = arr9[9];
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
            }

            try
            {
                var dummy = arr9[-1];
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
            }

            try
            {
                arr9[9] = 1;
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
            }

            try
            {
                arr9[-1] = 1;
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
            }

            var arr10 = default(ByteArray10);
            try
            {
                var dummy = arr10[10];
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
            }

            try
            {
                var dummy = arr10[-1];
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
            }

            try
            {
                arr10[10] = 1;
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
            }

            try
            {
                arr10[-1] = 1;
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
            }

            var arr11 = default(ByteArray11);
            try
            {
                var dummy = arr11[11];
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
            }

            try
            {
                var dummy = arr11[-1];
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
            }

            try
            {
                arr11[11] = 1;
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
            }

            try
            {
                arr11[-1] = 1;
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
            }

        }
    }
}
