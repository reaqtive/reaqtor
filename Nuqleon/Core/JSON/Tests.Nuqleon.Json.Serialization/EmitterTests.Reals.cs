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

namespace Tests
{
    public partial class EmitterTests
    {
        [TestMethod]
        public void FastEmitter_Single()
        {
            AssertEmit<float>(Emitter.EmitSingle, 0.65f, "0.65");
            AssertEmit<float>(Emitter.EmitSingle, 1.23f, "1.23");
            AssertEmit<float>(Emitter.EmitSingle, -98f, "-98");
        }

        [TestMethod]
        public void FastEmitter_NullableSingle()
        {
            AssertEmit<float?>(Emitter.EmitNullableSingle, null, "null");

            AssertEmit<float?>(Emitter.EmitNullableSingle, 0.65f, "0.65");
            AssertEmit<float?>(Emitter.EmitNullableSingle, 1.23f, "1.23");
            AssertEmit<float?>(Emitter.EmitNullableSingle, -98f, "-98");
        }

        [TestMethod]
        public void FastEmitter_Double()
        {
            AssertEmit<double>(Emitter.EmitDouble, 0.65d, "0.65");
            AssertEmit<double>(Emitter.EmitDouble, 1.23d, "1.23");
            AssertEmit<double>(Emitter.EmitDouble, -98d, "-98");
        }

        [TestMethod]
        public void FastEmitter_NullableDouble()
        {
            AssertEmit<double?>(Emitter.EmitNullableDouble, null, "null");

            AssertEmit<double?>(Emitter.EmitNullableDouble, 0.65d, "0.65");
            AssertEmit<double?>(Emitter.EmitNullableDouble, 1.23d, "1.23");
            AssertEmit<double?>(Emitter.EmitNullableDouble, -98d, "-98");
        }

        [TestMethod]
        public void FastEmitter_Decimal()
        {
            AssertEmit<decimal>(Emitter.EmitDecimal, 0.65m, "0.65");
            AssertEmit<decimal>(Emitter.EmitDecimal, 1.23m, "1.23");
            AssertEmit<decimal>(Emitter.EmitDecimal, -98m, "-98");
        }

        [TestMethod]
        public void FastEmitter_NullableDecimal()
        {
            AssertEmit<decimal?>(Emitter.EmitNullableDecimal, null, "null");

            AssertEmit<decimal?>(Emitter.EmitNullableDecimal, 0.65m, "0.65");
            AssertEmit<decimal?>(Emitter.EmitNullableDecimal, 1.23m, "1.23");
            AssertEmit<decimal?>(Emitter.EmitNullableDecimal, -98m, "-98");
        }
    }
}
