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
    public class BitArrayTests
    {
        [TestMethod]
        public void BitArray_Factory_ReturnsCorrectlySizedType()
        {
            // i = 0 is hoisted out
            var arr = BitArrayFactory.Create(0);
            Assert.AreEqual(arr.Count, 0);

            for (int i = 1; i < 100; i++)
            {
                arr = BitArrayFactory.Create(i);
                Assert.AreEqual(arr.Count, i);
                arr[i - 1] = true;

                try
                {
                    arr[i] = false;
                    Assert.Fail();
                }
                catch (ArgumentOutOfRangeException)
                {
                }
            }
        }

        [TestMethod]
        public void BitArray_Assembly()
        {
            Type type = typeof(BitArrayFactory);
            string assembly = type.Assembly.GetName().Name;
            Assert.AreEqual("Nuqleon.Collections.Specialized", assembly);
        }

        [TestMethod]
        public void BitArray_EdgeCases()
        {
            // i = 0 is hoisted out
            var arr = BitArrayFactory.Create(0);
            try
            {
                var dummy = arr[0];
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
            }

            for (int i = 1; i < 100; i++)
            {
                arr = BitArrayFactory.Create(i);
                try
                {
                    var dummy = arr[-1];
                    Assert.Fail();
                }
                catch (ArgumentOutOfRangeException)
                {
                }

                try
                {
                    var dummy = arr[i];
                    Assert.Fail();
                }
                catch (ArgumentOutOfRangeException)
                {
                }
            }
        }

        [TestMethod]
        public void BitArray_ManOrBoy_Set()
        {
            for (var capacity = 0; capacity < 100; capacity++)
            {
                for (var i = 0; i < capacity; i++)
                {
                    var arr = BitArrayFactory.Create(capacity);
                    arr[i] = true;

                    for (var j = 0; j < capacity; j++)
                    {
                        Assert.AreEqual(arr[j], i == j);
                    }
                }
            }
        }

        [TestMethod]
        public void BitArray_ManOrBoy_UnSet()
        {
            for (var capacity = 0; capacity < 100; capacity++)
            {
                for (var i = 0; i < capacity; i++)
                {
                    var arr = BitArrayFactory.Create(capacity);
                    arr.SetAll(true);

                    arr[i] = false;

                    for (var j = 0; j < capacity; j++)
                    {
                        Assert.AreEqual(arr[j], i != j);
                    }
                }
            }
        }


        [TestMethod]
        public void BitArray_SetAll()
        {
            for (var capacity = 0; capacity < 100; capacity++)
            {
                for (var i = 0; i < capacity; i++)
                {
                    var arr = BitArrayFactory.Create(capacity);
                    arr.SetAll(true);

                    for (var j = 0; j < capacity; j++)
                    {
                        Assert.AreEqual(arr[j], true);
                    }

                    arr.SetAll(false);

                    for (var j = 0; j < capacity; j++)
                    {
                        Assert.AreEqual(arr[j], false);
                    }

                    // The difference from above is that we set all to false first then set to true
                    arr = BitArrayFactory.Create(capacity);
                    arr.SetAll(false);

                    for (var j = 0; j < capacity; j++)
                    {
                        Assert.AreEqual(arr[j], false);
                    }

                    arr.SetAll(true);

                    for (var j = 0; j < capacity; j++)
                    {
                        Assert.AreEqual(arr[j], true);
                    }
                }
            }
        }

        [TestMethod]
        public void BitArrayFactory_InvalidSize()
        {
            try
            {
                BitArrayFactory.Create(-1);
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
            }
        }

        [TestMethod]
        public void BitArrayFactory_Constructor_Validation()
        {
            try
            {
                _ = new BitArraySlim<ByteArray1>(-1);
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
            }

            try
            {
                _ = new BitArraySlim<ByteArray1>(9);
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
            }

            _ = new BitArraySlim<ByteArrayGinormous>(255);

            try
            {
                // Small bit array restricts the max size of the underlying
                // byte array so it can save space by only using a byte for
                // the field it uses to store the size
                _ = new BitArraySlim<ByteArrayGinormous>(256);
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
            }
        }

        private struct ByteArrayGinormous : IByteArray
        {
            public int Length => 300;

            public byte this[int index]
            {
                get => throw new NotImplementedException();
                set => throw new NotImplementedException();
            }
        }
    }
}
