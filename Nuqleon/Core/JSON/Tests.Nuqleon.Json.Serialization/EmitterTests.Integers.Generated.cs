// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 04/16/2016 - Created fast JSON serializer functionality.
//

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nuqleon.Json.Serialization;
using System;

namespace Tests
{
    partial class EmitterTests
    {
        [TestMethod]
        public void FastEmitter_SByte()
        {
            AssertEmit<SByte>(Emitter.EmitSByte, SByte.MinValue, SByte.MinValue.ToString());
            AssertEmit<SByte>(Emitter.EmitSByte, SByte.MaxValue, SByte.MaxValue.ToString());

            for (var i = -128; i <= 127; i++)
            {
                AssertEmit<SByte>(Emitter.EmitSByte, (SByte)i, i.ToString());
            }
        }

        [TestMethod]
        public void FastEmitter_NullableSByte()
        {
            AssertEmit<SByte?>(Emitter.EmitNullableSByte, null, "null");

            AssertEmit<SByte?>(Emitter.EmitNullableSByte, SByte.MinValue, SByte.MinValue.ToString());
            AssertEmit<SByte?>(Emitter.EmitNullableSByte, SByte.MaxValue, SByte.MaxValue.ToString());

            for (var i = -128; i <= 127; i++)
            {
                AssertEmit<SByte?>(Emitter.EmitNullableSByte, (SByte)i, i.ToString());
            }
        }

        [TestMethod]
        public void FastEmitter_Int16()
        {
            AssertEmit<Int16>(Emitter.EmitInt16, Int16.MinValue, Int16.MinValue.ToString());
            AssertEmit<Int16>(Emitter.EmitInt16, Int16.MaxValue, Int16.MaxValue.ToString());

            for (var i = -128; i <= 127; i++)
            {
                AssertEmit<Int16>(Emitter.EmitInt16, (Int16)i, i.ToString());
            }
        }

        [TestMethod]
        public void FastEmitter_NullableInt16()
        {
            AssertEmit<Int16?>(Emitter.EmitNullableInt16, null, "null");

            AssertEmit<Int16?>(Emitter.EmitNullableInt16, Int16.MinValue, Int16.MinValue.ToString());
            AssertEmit<Int16?>(Emitter.EmitNullableInt16, Int16.MaxValue, Int16.MaxValue.ToString());

            for (var i = -128; i <= 127; i++)
            {
                AssertEmit<Int16?>(Emitter.EmitNullableInt16, (Int16)i, i.ToString());
            }
        }

        [TestMethod]
        public void FastEmitter_Int32()
        {
            AssertEmit<Int32>(Emitter.EmitInt32, Int32.MinValue, Int32.MinValue.ToString());
            AssertEmit<Int32>(Emitter.EmitInt32, Int32.MaxValue, Int32.MaxValue.ToString());

            for (var i = -128; i <= 127; i++)
            {
                AssertEmit<Int32>(Emitter.EmitInt32, (Int32)i, i.ToString());
            }
        }

        [TestMethod]
        public void FastEmitter_NullableInt32()
        {
            AssertEmit<Int32?>(Emitter.EmitNullableInt32, null, "null");

            AssertEmit<Int32?>(Emitter.EmitNullableInt32, Int32.MinValue, Int32.MinValue.ToString());
            AssertEmit<Int32?>(Emitter.EmitNullableInt32, Int32.MaxValue, Int32.MaxValue.ToString());

            for (var i = -128; i <= 127; i++)
            {
                AssertEmit<Int32?>(Emitter.EmitNullableInt32, (Int32)i, i.ToString());
            }
        }

        [TestMethod]
        public void FastEmitter_Int64()
        {
            AssertEmit<Int64>(Emitter.EmitInt64, Int64.MinValue, Int64.MinValue.ToString());
            AssertEmit<Int64>(Emitter.EmitInt64, Int64.MaxValue, Int64.MaxValue.ToString());

            for (var i = -128; i <= 127; i++)
            {
                AssertEmit<Int64>(Emitter.EmitInt64, (Int64)i, i.ToString());
            }
        }

        [TestMethod]
        public void FastEmitter_NullableInt64()
        {
            AssertEmit<Int64?>(Emitter.EmitNullableInt64, null, "null");

            AssertEmit<Int64?>(Emitter.EmitNullableInt64, Int64.MinValue, Int64.MinValue.ToString());
            AssertEmit<Int64?>(Emitter.EmitNullableInt64, Int64.MaxValue, Int64.MaxValue.ToString());

            for (var i = -128; i <= 127; i++)
            {
                AssertEmit<Int64?>(Emitter.EmitNullableInt64, (Int64)i, i.ToString());
            }
        }

        [TestMethod]
        public void FastEmitter_Byte()
        {
            AssertEmit<Byte>(Emitter.EmitByte, Byte.MinValue, Byte.MinValue.ToString());
            AssertEmit<Byte>(Emitter.EmitByte, Byte.MaxValue, Byte.MaxValue.ToString());

            for (var i = 0; i <= 127; i++)
            {
                AssertEmit<Byte>(Emitter.EmitByte, (Byte)i, i.ToString());
            }
        }

        [TestMethod]
        public void FastEmitter_NullableByte()
        {
            AssertEmit<Byte?>(Emitter.EmitNullableByte, null, "null");

            AssertEmit<Byte?>(Emitter.EmitNullableByte, Byte.MinValue, Byte.MinValue.ToString());
            AssertEmit<Byte?>(Emitter.EmitNullableByte, Byte.MaxValue, Byte.MaxValue.ToString());

            for (var i = 0; i <= 127; i++)
            {
                AssertEmit<Byte?>(Emitter.EmitNullableByte, (Byte)i, i.ToString());
            }
        }

        [TestMethod]
        public void FastEmitter_UInt16()
        {
            AssertEmit<UInt16>(Emitter.EmitUInt16, UInt16.MinValue, UInt16.MinValue.ToString());
            AssertEmit<UInt16>(Emitter.EmitUInt16, UInt16.MaxValue, UInt16.MaxValue.ToString());

            for (var i = 0; i <= 127; i++)
            {
                AssertEmit<UInt16>(Emitter.EmitUInt16, (UInt16)i, i.ToString());
            }
        }

        [TestMethod]
        public void FastEmitter_NullableUInt16()
        {
            AssertEmit<UInt16?>(Emitter.EmitNullableUInt16, null, "null");

            AssertEmit<UInt16?>(Emitter.EmitNullableUInt16, UInt16.MinValue, UInt16.MinValue.ToString());
            AssertEmit<UInt16?>(Emitter.EmitNullableUInt16, UInt16.MaxValue, UInt16.MaxValue.ToString());

            for (var i = 0; i <= 127; i++)
            {
                AssertEmit<UInt16?>(Emitter.EmitNullableUInt16, (UInt16)i, i.ToString());
            }
        }

        [TestMethod]
        public void FastEmitter_UInt32()
        {
            AssertEmit<UInt32>(Emitter.EmitUInt32, UInt32.MinValue, UInt32.MinValue.ToString());
            AssertEmit<UInt32>(Emitter.EmitUInt32, UInt32.MaxValue, UInt32.MaxValue.ToString());

            for (var i = 0; i <= 127; i++)
            {
                AssertEmit<UInt32>(Emitter.EmitUInt32, (UInt32)i, i.ToString());
            }
        }

        [TestMethod]
        public void FastEmitter_NullableUInt32()
        {
            AssertEmit<UInt32?>(Emitter.EmitNullableUInt32, null, "null");

            AssertEmit<UInt32?>(Emitter.EmitNullableUInt32, UInt32.MinValue, UInt32.MinValue.ToString());
            AssertEmit<UInt32?>(Emitter.EmitNullableUInt32, UInt32.MaxValue, UInt32.MaxValue.ToString());

            for (var i = 0; i <= 127; i++)
            {
                AssertEmit<UInt32?>(Emitter.EmitNullableUInt32, (UInt32)i, i.ToString());
            }
        }

        [TestMethod]
        public void FastEmitter_UInt64()
        {
            AssertEmit<UInt64>(Emitter.EmitUInt64, UInt64.MinValue, UInt64.MinValue.ToString());
            AssertEmit<UInt64>(Emitter.EmitUInt64, UInt64.MaxValue, UInt64.MaxValue.ToString());

            for (var i = 0; i <= 127; i++)
            {
                AssertEmit<UInt64>(Emitter.EmitUInt64, (UInt64)i, i.ToString());
            }
        }

        [TestMethod]
        public void FastEmitter_NullableUInt64()
        {
            AssertEmit<UInt64?>(Emitter.EmitNullableUInt64, null, "null");

            AssertEmit<UInt64?>(Emitter.EmitNullableUInt64, UInt64.MinValue, UInt64.MinValue.ToString());
            AssertEmit<UInt64?>(Emitter.EmitNullableUInt64, UInt64.MaxValue, UInt64.MaxValue.ToString());

            for (var i = 0; i <= 127; i++)
            {
                AssertEmit<UInt64?>(Emitter.EmitNullableUInt64, (UInt64)i, i.ToString());
            }
        }

    }
}
