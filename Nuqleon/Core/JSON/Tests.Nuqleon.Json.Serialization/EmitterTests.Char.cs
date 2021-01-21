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
using System.Globalization;
using System;
using System.Text;

namespace Tests
{
    public partial class EmitterTests
    {
        [TestMethod]
        public void FastEmitter_Char()
        {
            AssertEmit(Emitter.EmitChar, 'a', "\"a\"");
            AssertEmit(Emitter.EmitChar, '0', "\"0\"");

            AssertEmit(Emitter.EmitChar, '\"', "\"\\\"\"");  // JSON `"\""`
            AssertEmit(Emitter.EmitChar, '\\', "\"\\\\\"");  // JSON `"\\"`

            AssertEmit(Emitter.EmitChar, '\b', "\"\\b\"");   // JSON `"\b"`
            AssertEmit(Emitter.EmitChar, '\f', "\"\\f\"");   // JSON `"\f"`
            AssertEmit(Emitter.EmitChar, '\n', "\"\\n\"");   // JSON `"\n"`
            AssertEmit(Emitter.EmitChar, '\r', "\"\\r\"");   // JSON `"\r"`
            AssertEmit(Emitter.EmitChar, '\t', "\"\\t\"");   // JSON `"\t"`

            var special = new[] { '\b', '\f', '\n', '\r', '\t' };

            for (var i = 0x0000; i <= 0xFFFF; i++)
            {
                var c = (char)i;

                if (char.IsControl(c) && Array.IndexOf(special, c) < 0)
                {
                    AssertEmit(Emitter.EmitChar, c, string.Format(CultureInfo.InvariantCulture, "\"\\u{0:X4}\"", i));
                }
            }

            AssertEmitCharHexPadFour((char)0x0000, "0000");
            AssertEmitCharHexPadFour((char)0x1234, "1234");
            AssertEmitCharHexPadFour((char)0x6789, "6789");
            AssertEmitCharHexPadFour((char)0xABCD, "ABCD");
            AssertEmitCharHexPadFour((char)0xCDEF, "CDEF");
        }

        [TestMethod]
        public void FastEmitter_NullableChar()
        {
            AssertEmit<char?>(Emitter.EmitNullableChar, null, "null");

            AssertEmit<char?>(Emitter.EmitNullableChar, 'a', "\"a\"");
            AssertEmit<char?>(Emitter.EmitNullableChar, '0', "\"0\"");
        }

        private static void AssertEmitCharHexPadFour(char c, string expected)
        {
            var sb = new StringBuilder();
            Emitter.EmitCharHexPadFour(sb, c);
            Assert.AreEqual(expected, sb.ToString());
        }
    }
}
