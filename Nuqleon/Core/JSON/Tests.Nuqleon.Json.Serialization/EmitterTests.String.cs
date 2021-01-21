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
        public void FastEmitter_String()
        {
            AssertEmit<string>(Emitter.EmitString, null, "null");

            AssertEmit<string>(Emitter.EmitString, "", "\"\"");

            AssertEmit<string>(Emitter.EmitString, "b", "\"b\"");
            AssertEmit<string>(Emitter.EmitString, "ba", "\"ba\"");
            AssertEmit<string>(Emitter.EmitString, "bar", "\"bar\"");

            AssertEmit<string>(Emitter.EmitString, "Bart says \"Hi!\".", "\"Bart says \\\"Hi!\\\".\"");
            AssertEmit<string>(Emitter.EmitString, "\"Hi!\", he said.", "\"\\\"Hi!\\\", he said.\"");
            AssertEmit<string>(Emitter.EmitString, "Escape\tescape\r\nescape\bescape\f.", "\"Escape\\tescape\\r\\nescape\\bescape\\f.\"");
            AssertEmit<string>(Emitter.EmitString, "Escape\u0000escape\u0001.", "\"Escape\\u0000escape\\u0001.\"");
        }
    }
}
