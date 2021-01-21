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
        public void FastEmitter_Boolean()
        {
            AssertEmit(Emitter.EmitBoolean, true, "true");
            AssertEmit(Emitter.EmitBoolean, false, "false");
        }

        [TestMethod]
        public void FastEmitter_NullableBoolean()
        {
            AssertEmit<bool?>(Emitter.EmitNullableBoolean, null, "null");
            AssertEmit<bool?>(Emitter.EmitNullableBoolean, true, "true");
            AssertEmit<bool?>(Emitter.EmitNullableBoolean, false, "false");
        }
    }
}
