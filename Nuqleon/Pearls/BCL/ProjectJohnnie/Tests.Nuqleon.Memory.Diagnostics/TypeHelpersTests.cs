// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Runtime.CompilerServices;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class TypeHelpersTests
    {
        [TestMethod]
        public void TypeHelpers_SizeOfT()
        {
            Assert.AreEqual(sizeof(bool), TypeHelpers.SizeOf<bool>());
            Assert.AreEqual(sizeof(byte), TypeHelpers.SizeOf<byte>());
            Assert.AreEqual(sizeof(sbyte), TypeHelpers.SizeOf<sbyte>());
            Assert.AreEqual(sizeof(short), TypeHelpers.SizeOf<short>());
            Assert.AreEqual(sizeof(ushort), TypeHelpers.SizeOf<ushort>());
            Assert.AreEqual(sizeof(int), TypeHelpers.SizeOf<int>());
            Assert.AreEqual(sizeof(uint), TypeHelpers.SizeOf<uint>());
            Assert.AreEqual(sizeof(long), TypeHelpers.SizeOf<long>());
            Assert.AreEqual(sizeof(ulong), TypeHelpers.SizeOf<ulong>());
            Assert.AreEqual(sizeof(decimal), TypeHelpers.SizeOf<decimal>());
            Assert.AreEqual(sizeof(char), TypeHelpers.SizeOf<char>());
        }
    }
}
