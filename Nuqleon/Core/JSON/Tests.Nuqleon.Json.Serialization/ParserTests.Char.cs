// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 04/05/2016 - Created fast JSON deserializer functionality.
//

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nuqleon.Json.Serialization;

namespace Tests
{
    public partial class ParserTests
    {
        [TestMethod]
        public void FastParser_Char()
        {
            AssertParse(Parser.ParseChar, Literal("b"), 'b');

            AssertParse(Parser.ParseChar, Literal(@"\t"), '\t');
            AssertParse(Parser.ParseChar, Literal(@"\r"), '\r');
            AssertParse(Parser.ParseChar, Literal(@"\n"), '\n');
            AssertParse(Parser.ParseChar, Literal(@"\b"), '\b');
            AssertParse(Parser.ParseChar, Literal(@"\f"), '\f');
            AssertParse(Parser.ParseChar, Literal(@"\\"), '\\');
            AssertParse(Parser.ParseChar, Literal(@"\/"), '/');
            AssertParse(Parser.ParseChar, Literal(@"\"""), '\"');

            AssertParse(Parser.ParseChar, Literal(@"\u1234"), '\u1234');
            AssertParse(Parser.ParseChar, Literal(@"\u12ab"), '\u12ab');
            AssertParse(Parser.ParseChar, Literal(@"\u12EF"), '\u12EF');
            AssertParse(Parser.ParseChar, Literal(@"\uD834"), '\uD834');
            AssertParse(Parser.ParseChar, Literal(@"\uDD1E"), '\uDD1E');

            // empty
            AssertParseFail(Parser.ParseChar, "");
            AssertParseFail(Parser.ParseChar, Literal(""));

            // too long
            AssertParseFail(Parser.ParseChar, "\"ba");
            AssertParseFail(Parser.ParseChar, Literal("ba"));

            // unterminated literal
            AssertParseFail(Parser.ParseChar, "\"");
            AssertParseFail(Parser.ParseChar, "\"b");

            // control character
            AssertParseFail(Parser.ParseChar, Literal("\t"));

            // invalid escape sequence
            AssertParseFail(Parser.ParseChar, "\"\\");
            AssertParseFail(Parser.ParseChar, Literal(@"\"));
            AssertParseFail(Parser.ParseChar, Literal(@"\z"));
            AssertParseFail(Parser.ParseChar, Literal(@"\u"));
            AssertParseFail(Parser.ParseChar, Literal(@"\u1"));
            AssertParseFail(Parser.ParseChar, Literal(@"\u12"));
            AssertParseFail(Parser.ParseChar, Literal(@"\u123"));
            AssertParseFail(Parser.ParseChar, Literal(@"\u12g4"));
            AssertParseFail(Parser.ParseChar, Literal(@"\u12G4"));
            AssertParseFail(Parser.ParseChar, Literal(@"\u12-4"));
            AssertParseFail(Parser.ParseChar, Literal(@"\u12-4"));

            AssertParseFail(Parser.ParseChar, "n");
            AssertParseFail(Parser.ParseChar, "nu");
            AssertParseFail(Parser.ParseChar, "nul");
            AssertParseFail(Parser.ParseChar, "null");

            AssertParseFail(Parser.ParseChar, "Null");
            AssertParseFail(Parser.ParseChar, "nUll");
            AssertParseFail(Parser.ParseChar, "nuLl");
            AssertParseFail(Parser.ParseChar, "nulL");

            AssertParseFail(Parser.ParseChar, "42");
            AssertParseFail(Parser.ParseChar, "-42");

            AssertParseFail(Parser.ParseChar, "[");
            AssertParseFail(Parser.ParseChar, "]");
            AssertParseFail(Parser.ParseChar, "{");
            AssertParseFail(Parser.ParseChar, "}");
            AssertParseFail(Parser.ParseChar, ",");
            AssertParseFail(Parser.ParseChar, ":");
        }

        [TestMethod]
        public void FastParser_NullableChar()
        {
            AssertParse(Parser.ParseNullableChar, "null", null);

            AssertParse(Parser.ParseNullableChar, Literal("b"), 'b');

            AssertParse(Parser.ParseNullableChar, Literal(@"\t"), '\t');
            AssertParse(Parser.ParseNullableChar, Literal(@"\r"), '\r');
            AssertParse(Parser.ParseNullableChar, Literal(@"\n"), '\n');
            AssertParse(Parser.ParseNullableChar, Literal(@"\b"), '\b');
            AssertParse(Parser.ParseNullableChar, Literal(@"\f"), '\f');
            AssertParse(Parser.ParseNullableChar, Literal(@"\\"), '\\');
            AssertParse(Parser.ParseNullableChar, Literal(@"\/"), '/');
            AssertParse(Parser.ParseNullableChar, Literal(@"\"""), '\"');

            AssertParse(Parser.ParseNullableChar, Literal(@"\u1234"), '\u1234');
            AssertParse(Parser.ParseNullableChar, Literal(@"\u12ab"), '\u12ab');
            AssertParse(Parser.ParseNullableChar, Literal(@"\u12EF"), '\u12EF');
            AssertParse(Parser.ParseNullableChar, Literal(@"\uD834"), '\uD834');
            AssertParse(Parser.ParseNullableChar, Literal(@"\uDD1E"), '\uDD1E');

            // empty
            AssertParseFail(Parser.ParseNullableChar, "");
            AssertParseFail(Parser.ParseNullableChar, Literal(""));

            // too long
            AssertParseFail(Parser.ParseNullableChar, "\"ba");
            AssertParseFail(Parser.ParseNullableChar, Literal("ba"));

            // unterminated literal
            AssertParseFail(Parser.ParseNullableChar, "\"");
            AssertParseFail(Parser.ParseNullableChar, "\"b");

            // control character
            AssertParseFail(Parser.ParseNullableChar, Literal("\t"));

            // invalid escape sequence
            AssertParseFail(Parser.ParseNullableChar, "\"\\");
            AssertParseFail(Parser.ParseNullableChar, Literal(@"\"));
            AssertParseFail(Parser.ParseNullableChar, Literal(@"\z"));
            AssertParseFail(Parser.ParseNullableChar, Literal(@"\u"));
            AssertParseFail(Parser.ParseNullableChar, Literal(@"\u1"));
            AssertParseFail(Parser.ParseNullableChar, Literal(@"\u12"));
            AssertParseFail(Parser.ParseNullableChar, Literal(@"\u123"));
            AssertParseFail(Parser.ParseNullableChar, Literal(@"\u12g4"));
            AssertParseFail(Parser.ParseNullableChar, Literal(@"\u12G4"));
            AssertParseFail(Parser.ParseNullableChar, Literal(@"\u12-4"));
            AssertParseFail(Parser.ParseNullableChar, Literal(@"\u12-4"));

            AssertParseFail(Parser.ParseNullableChar, "n");
            AssertParseFail(Parser.ParseNullableChar, "nu");
            AssertParseFail(Parser.ParseNullableChar, "nul");

            AssertParseFail(Parser.ParseNullableChar, "Null");
            AssertParseFail(Parser.ParseNullableChar, "nUll");
            AssertParseFail(Parser.ParseNullableChar, "nuLl");
            AssertParseFail(Parser.ParseNullableChar, "nulL");

            AssertParseFail(Parser.ParseNullableChar, "42");
            AssertParseFail(Parser.ParseNullableChar, "-42");

            AssertParseFail(Parser.ParseNullableChar, "[");
            AssertParseFail(Parser.ParseNullableChar, "]");
            AssertParseFail(Parser.ParseNullableChar, "{");
            AssertParseFail(Parser.ParseNullableChar, "}");
            AssertParseFail(Parser.ParseNullableChar, ",");
            AssertParseFail(Parser.ParseNullableChar, ":");
        }
    }
}
