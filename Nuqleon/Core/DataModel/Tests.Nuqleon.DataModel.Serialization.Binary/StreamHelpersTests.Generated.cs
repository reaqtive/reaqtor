// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;

using Nuqleon.DataModel.Serialization.Binary;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Nuqleon.DataModel.Serialization.Binary
{
    public partial class StreamHelpersTests
    {
        [TestMethod]
        public void StreamHelpers_WriteAndRead_Byte()
        {
            var testValues = new byte [] {
                Byte.MinValue,
                Byte.MaxValue,
                (Byte)0,
            };

            foreach (var value in testValues)
            {
                using (var stream = new MemoryStream())
                {
                    stream.WriteByte(value);
                    stream.Position = 0;
                    Assert.AreEqual(value, stream.ReadByte());
                }
            }
        }

        [TestMethod]
        public void StreamHelpers_WriteAndRead_Boolean()
        {
            var testValues = new bool [] {
                true,
                false,
            };

            foreach (var value in testValues)
            {
                using (var stream = new MemoryStream())
                {
                    stream.WriteBoolean(value);
                    stream.Position = 0;
                    Assert.AreEqual(value, stream.ReadBoolean());
                }
            }
        }

        [TestMethod]
        public void StreamHelpers_WriteAndRead_Char()
        {
            var testValues = new char [] {
                Char.MinValue,
                Char.MaxValue,
                (Char)0,
            };

            foreach (var value in testValues)
            {
                using (var stream = new MemoryStream())
                {
                    stream.WriteChar(value);
                    stream.Position = 0;
                    Assert.AreEqual(value, stream.ReadChar());
                }
            }
        }

        [TestMethod]
        public void StreamHelpers_WriteAndRead_Double()
        {
            var testValues = new double [] {
                Double.MinValue,
                Double.MaxValue,
                (Double)0,
            };

            foreach (var value in testValues)
            {
                using (var stream = new MemoryStream())
                {
                    stream.WriteDouble(value);
                    stream.Position = 0;
                    Assert.AreEqual(value, stream.ReadDouble());
                }
            }
        }

        [TestMethod]
        public void StreamHelpers_WriteAndRead_Decimal()
        {
            var testValues = new decimal [] {
                Decimal.MinValue,
                Decimal.MaxValue,
                (Decimal)0,
            };

            foreach (var value in testValues)
            {
                using (var stream = new MemoryStream())
                {
                    stream.WriteDecimal(value);
                    stream.Position = 0;
                    Assert.AreEqual(value, stream.ReadDecimal());
                }
            }
        }

        [TestMethod]
        public void StreamHelpers_WriteAndRead_Guid()
        {
            var testValues = new Guid [] {
                Guid.NewGuid(),
            };

            foreach (var value in testValues)
            {
                using (var stream = new MemoryStream())
                {
                    stream.WriteGuid(value);
                    stream.Position = 0;
                    Assert.AreEqual(value, stream.ReadGuid());
                }
            }
        }

        [TestMethod]
        public void StreamHelpers_WriteAndRead_Int16()
        {
            var testValues = new Int16 [] {
                Int16.MinValue,
                Int16.MaxValue,
                (Int16)0,
            };

            foreach (var value in testValues)
            {
                using (var stream = new MemoryStream())
                {
                    stream.WriteInt16(value);
                    stream.Position = 0;
                    Assert.AreEqual(value, stream.ReadInt16());
                }
            }
        }

        [TestMethod]
        public void StreamHelpers_WriteAndRead_Int32()
        {
            var testValues = new Int32 [] {
                Int32.MinValue,
                Int32.MaxValue,
                (Int32)0,
            };

            foreach (var value in testValues)
            {
                using (var stream = new MemoryStream())
                {
                    stream.WriteInt32(value);
                    stream.Position = 0;
                    Assert.AreEqual(value, stream.ReadInt32());
                }
            }
        }

        [TestMethod]
        public void StreamHelpers_WriteAndRead_Int64()
        {
            var testValues = new Int64 [] {
                Int64.MinValue,
                Int64.MaxValue,
                (Int64)0,
            };

            foreach (var value in testValues)
            {
                using (var stream = new MemoryStream())
                {
                    stream.WriteInt64(value);
                    stream.Position = 0;
                    Assert.AreEqual(value, stream.ReadInt64());
                }
            }
        }

        [TestMethod]
        public void StreamHelpers_WriteAndRead_SByte()
        {
            var testValues = new SByte [] {
                SByte.MinValue,
                SByte.MaxValue,
                (SByte)0,
            };

            foreach (var value in testValues)
            {
                using (var stream = new MemoryStream())
                {
                    stream.WriteSByte(value);
                    stream.Position = 0;
                    Assert.AreEqual(value, stream.ReadSByte());
                }
            }
        }

        [TestMethod]
        public void StreamHelpers_WriteAndRead_Single()
        {
            var testValues = new Single [] {
                Single.MinValue,
                Single.MaxValue,
                (Single)0,
            };

            foreach (var value in testValues)
            {
                using (var stream = new MemoryStream())
                {
                    stream.WriteSingle(value);
                    stream.Position = 0;
                    Assert.AreEqual(value, stream.ReadSingle());
                }
            }
        }

        [TestMethod]
        public void StreamHelpers_WriteAndRead_String()
        {
            var testValues = new String [] {
                String.Empty,
                "foobar",
            };

            foreach (var value in testValues)
            {
                using (var stream = new MemoryStream())
                {
                    stream.WriteString(value);
                    stream.Position = 0;
                    Assert.AreEqual(value, stream.ReadString());
                }
            }
        }

        [TestMethod]
        public void StreamHelpers_WriteAndRead_UInt16()
        {
            var testValues = new UInt16 [] {
                UInt16.MinValue,
                UInt16.MaxValue,
                (UInt16)0,
            };

            foreach (var value in testValues)
            {
                using (var stream = new MemoryStream())
                {
                    stream.WriteUInt16(value);
                    stream.Position = 0;
                    Assert.AreEqual(value, stream.ReadUInt16());
                }
            }
        }

        [TestMethod]
        public void StreamHelpers_WriteAndRead_UInt32()
        {
            var testValues = new UInt32 [] {
                UInt32.MinValue,
                UInt32.MaxValue,
                (UInt32)0,
            };

            foreach (var value in testValues)
            {
                using (var stream = new MemoryStream())
                {
                    stream.WriteUInt32(value);
                    stream.Position = 0;
                    Assert.AreEqual(value, stream.ReadUInt32());
                }
            }
        }

        [TestMethod]
        public void StreamHelpers_WriteAndRead_UInt32Compact()
        {
            var testValues = new UInt32 [] {
                UInt32.MinValue,
                UInt32.MaxValue,
                (UInt32)0,
            };

            foreach (var value in testValues)
            {
                using (var stream = new MemoryStream())
                {
                    stream.WriteUInt32Compact(value);
                    stream.Position = 0;
                    Assert.AreEqual(value, stream.ReadUInt32Compact());
                }
            }
        }

        [TestMethod]
        public void StreamHelpers_WriteAndRead_UInt64()
        {
            var testValues = new UInt64 [] {
                UInt64.MinValue,
                UInt64.MaxValue,
                (UInt64)0,
            };

            foreach (var value in testValues)
            {
                using (var stream = new MemoryStream())
                {
                    stream.WriteUInt64(value);
                    stream.Position = 0;
                    Assert.AreEqual(value, stream.ReadUInt64());
                }
            }
        }

    }
}
