// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.IO;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Nuqleon.DataModel.Serialization.Binary;

namespace Tests.Nuqleon.DataModel.Serialization.Binary
{
    [TestClass]
    public partial class StreamHelpersTests
    {
        [TestMethod]
        public void StreamHelpers_EmptyArray()
        {
            Assert.ThrowsException<EndOfStreamException>(() => StreamHelpers.ReadSByte(new MemoryStream()));
            Assert.ThrowsException<EndOfStreamException>(() => StreamHelpers.ReadByte(new MemoryStream()));
            Assert.ThrowsException<EndOfStreamException>(() => StreamHelpers.ReadInt16(new MemoryStream()));
            Assert.ThrowsException<EndOfStreamException>(() => StreamHelpers.ReadUInt16(new MemoryStream()));
            Assert.ThrowsException<EndOfStreamException>(() => StreamHelpers.ReadInt32(new MemoryStream()));
            Assert.ThrowsException<EndOfStreamException>(() => StreamHelpers.ReadUInt32(new MemoryStream()));
            Assert.ThrowsException<EndOfStreamException>(() => StreamHelpers.ReadInt64(new MemoryStream()));
            Assert.ThrowsException<EndOfStreamException>(() => StreamHelpers.ReadUInt64(new MemoryStream()));
            Assert.ThrowsException<EndOfStreamException>(() => StreamHelpers.ReadSingle(new MemoryStream()));
            Assert.ThrowsException<EndOfStreamException>(() => StreamHelpers.ReadDouble(new MemoryStream()));
            Assert.ThrowsException<EndOfStreamException>(() => StreamHelpers.ReadDecimal(new MemoryStream()));
            Assert.ThrowsException<EndOfStreamException>(() => StreamHelpers.ReadChar(new MemoryStream()));
            Assert.ThrowsException<EndOfStreamException>(() => StreamHelpers.ReadBoolean(new MemoryStream()));
            Assert.ThrowsException<EndOfStreamException>(() => StreamHelpers.ReadGuid(new MemoryStream()));

            using var stream = new MemoryStream();

            stream.WriteUInt32Compact(42u);
            stream.Position = 0;

            Assert.ThrowsException<EndOfStreamException>(() => StreamHelpers.ReadString(stream));
        }

        [TestMethod]
        public void StreamHelpers_EmptyString()
        {
            using var stream = new MemoryStream();

            stream.WriteString(value: null);
            stream.Position = 0;

            Assert.IsNull(stream.ReadString());
        }

        [TestMethod]
        public void StreamHelpers_BigString()
        {
            using var stream = new MemoryStream();

            var bigString = new string(Enumerable.Repeat('a', StreamHelpers.MAX_POOLED_STRING_BYTES * 2).ToArray());

            stream.WriteString(bigString);
            stream.Position = 0;

            Assert.AreEqual(bigString, stream.ReadString());
        }

        [TestMethod]
        public void StreamHelpers_UInt32Compact()
        {
            using (var stream = new MemoryStream())
            {
                var value = 42u;
                stream.WriteUInt32Compact(value);
                stream.Position = 0;
                Assert.AreEqual(value, stream.ReadUInt32Compact());
            }

            using (var stream = new MemoryStream())
            {
                var value = (uint)0x4000;
                stream.WriteUInt32Compact(value);
                stream.Position = 0;
                Assert.AreEqual(value, stream.ReadUInt32Compact());
            }

            using (var stream = new MemoryStream())
            {
                var value = (uint)0x200000;
                stream.WriteUInt32Compact(value);
                stream.Position = 0;
                Assert.AreEqual(value, stream.ReadUInt32Compact());
            }

            using (var stream = new MemoryStream())
            {
                var value = (uint)0x10000000;
                stream.WriteUInt32Compact(value);
                stream.Position = 0;
                Assert.AreEqual(value, stream.ReadUInt32Compact());
            }
        }
    }
}
