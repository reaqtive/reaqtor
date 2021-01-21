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
        public void FastParser_String()
        {
            AssertParse(Parser.ParseString, "null", null);

            AssertParse(Parser.ParseString, Literal(""), "");

            AssertParse(Parser.ParseString, Literal("b"), "b");
            AssertParse(Parser.ParseString, Literal("ba"), "ba");
            AssertParse(Parser.ParseString, Literal("bar"), "bar");
            AssertParse(Parser.ParseString, Literal(" bar"), " bar");
            AssertParse(Parser.ParseString, Literal(" bar "), " bar ");

            AssertParse(Parser.ParseString, Literal(@"\t\r\n\b\f"), "\t\r\n\b\f");
            AssertParse(Parser.ParseString, Literal(@"foo\tbar\tqux"), "foo\tbar\tqux");
            AssertParse(Parser.ParseString, Literal(@"c:\\temp\\foo.txt"), "c:\\temp\\foo.txt");
            AssertParse(Parser.ParseString, Literal(@"http:\/\/bing.com"), "http://bing.com");
            AssertParse(Parser.ParseString, Literal(@"Bart says \""Hello, World!\""."), "Bart says \"Hello, World!\".");

            AssertParse(Parser.ParseString, Literal(@"\u1234"), "\u1234");
            AssertParse(Parser.ParseString, Literal(@"\u12ab"), "\u12ab");
            AssertParse(Parser.ParseString, Literal(@"\u12EF"), "\u12EF");
            AssertParse(Parser.ParseString, Literal(@"\uD834\uDD1E"), "\uD834\uDD1E");

            // empty
            AssertParseFail(Parser.ParseString, "");

            // unterminated literal
            AssertParseFail(Parser.ParseString, "\"");
            AssertParseFail(Parser.ParseString, "\"b");
            AssertParseFail(Parser.ParseString, "\"ba");
            AssertParseFail(Parser.ParseString, "\"bar");
            AssertParseFail(Parser.ParseString, "\"bar\\");
            AssertParseFail(Parser.ParseString, "\"bar\\t");
            AssertParseFail(Parser.ParseString, "\"bar\\tfoo");
            AssertParseFail(Parser.ParseString, "\"bar\\tfoo\\");

            // control character
            AssertParseFail(Parser.ParseString, "\"bar\\tfoo\t");

            // control character
            AssertParseFail(Parser.ParseString, Literal("foo\tbar"));
            AssertParseFail(Parser.ParseString, Literal("foo\tbar\tqux"));

            // invalid escape sequence
            AssertParseFail(Parser.ParseString, Literal(@"bar\"));
            AssertParseFail(Parser.ParseString, Literal(@"bar\z"));
            AssertParseFail(Parser.ParseString, Literal(@"bar\tfoo\"));
            AssertParseFail(Parser.ParseString, Literal(@"bar\tfoo\z"));
            AssertParseFail(Parser.ParseString, Literal(@"bar\u"));
            AssertParseFail(Parser.ParseString, Literal(@"bar\u1"));
            AssertParseFail(Parser.ParseString, Literal(@"bar\u12"));
            AssertParseFail(Parser.ParseString, Literal(@"bar\u123"));
            AssertParseFail(Parser.ParseString, Literal(@"bar\u12g4"));
            AssertParseFail(Parser.ParseString, Literal(@"bar\u12G4"));
            AssertParseFail(Parser.ParseString, Literal(@"bar\u12-4"));
            AssertParseFail(Parser.ParseString, Literal(@"bar\u12-4"));

            // invalid surrogate
            AssertParseFail(Parser.ParseString, Literal(@"\uD834"));       // hi
            AssertParseFail(Parser.ParseString, Literal(@"\uD834b"));      // hi
            AssertParseFail(Parser.ParseString, Literal(@"\uD834ba"));     // hi
            AssertParseFail(Parser.ParseString, Literal(@"\uD834bar"));    // hi
            AssertParseFail(Parser.ParseString, Literal(@"\uD834barfoo")); // hi
            AssertParseFail(Parser.ParseString, Literal(@"\uD834\"));      // hi
            AssertParseFail(Parser.ParseString, Literal(@"\uD834\t"));     // hi
            AssertParseFail(Parser.ParseString, Literal(@"\uD834\tz"));    // hi
            AssertParseFail(Parser.ParseString, Literal(@"\uD834\tzz"));   // hi
            AssertParseFail(Parser.ParseString, Literal(@"\uD834\tzzz"));  // hi
            AssertParseFail(Parser.ParseString, Literal(@"\uD834\tzzzz")); // hi
            AssertParseFail(Parser.ParseString, Literal(@"\uD834\u12-4"));
            AssertParseFail(Parser.ParseString, Literal(@"\uDD1E\uD834")); // lohi
            AssertParseFail(Parser.ParseString, Literal(@"\uD834\uD834")); // hihi

            AssertParseFail(Parser.ParseString, "n");
            AssertParseFail(Parser.ParseString, "nu");
            AssertParseFail(Parser.ParseString, "nul");

            AssertParseFail(Parser.ParseString, "Null");
            AssertParseFail(Parser.ParseString, "nUll");
            AssertParseFail(Parser.ParseString, "nuLl");
            AssertParseFail(Parser.ParseString, "nulL");

            AssertParseFail(Parser.ParseString, "42");
            AssertParseFail(Parser.ParseString, "-42");

            AssertParseFail(Parser.ParseString, "[");
            AssertParseFail(Parser.ParseString, "]");
            AssertParseFail(Parser.ParseString, "{");
            AssertParseFail(Parser.ParseString, "}");
            AssertParseFail(Parser.ParseString, ",");
            AssertParseFail(Parser.ParseString, ":");
        }

        private static string Literal(string s)
        {
            return "\"" + s + "\"";
        }
    }
}
