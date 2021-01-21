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
        public void FastParser_FastLexer()
        {
            AssertSkipAll("null");

            AssertSkipAll("true");
            AssertSkipAll("false");

            AssertSkipAll("0");
            AssertSkipAll("1");
            AssertSkipAll("42");
            AssertSkipAll("-42");
            AssertSkipAll("0.1");
            AssertSkipAll("-42.95");

            foreach (var e in new[] { 'e', 'E' })
            {
                AssertSkipAll("1" + e + "1");
                AssertSkipAll("23" + e + "67");
                AssertSkipAll("-789" + e + "0");
                AssertSkipAll("-789.65" + e + "4");
                AssertSkipAll("-789.65" + e + "+4");
                AssertSkipAll("-789.65" + e + "+41");
                AssertSkipAll("-789.65" + e + "-4");
                AssertSkipAll("-789.65" + e + "-41");
            }

            AssertSkipAll(Literal(""));
            AssertSkipAll(Literal(" "));
            AssertSkipAll(Literal("bar"));
            AssertSkipAll(Literal(" bar foo "));
            AssertSkipAll(Literal(@"bar\tfoo\rqux\nbaz\fbar\\foo\""qux\/baz\u1234bar\uabcf\uDEAD"));

            AssertSkipAll("[]");
            AssertSkipAll("[ ]");
            AssertSkipAll("[1]");
            AssertSkipAll("[ 1]");
            AssertSkipAll("[1 ]");
            AssertSkipAll("[ 1 ]");
            AssertSkipAll("[ \t 1\t\r \n ]");

            foreach (var c in new[] { ",", ", ", ",  ", " ,", "  ,", " , ", "  ,  " })
            {
                var s = string.Join(c, new[] { "42", "true", Literal("bar") });
                AssertSkipAll("[" + s + "]");
            }

            AssertSkipAll("{}");
            AssertSkipAll("{ }");
            AssertSkipAll("{\"bar\":42}");
            AssertSkipAll("{\"bar\":42 }");
            AssertSkipAll("{\"bar\":42  }");
            AssertSkipAll("{ \"bar\":42}");
            AssertSkipAll("{  \"bar\":42}");
            AssertSkipAll("{ \"bar\":42 }");
            AssertSkipAll("{  \t\r \n \"bar\":42 \t\n\r}");
            AssertSkipAll("{\"bar\" :42}");
            AssertSkipAll("{\"bar\": 42}");
            AssertSkipAll("{\"bar\" : 42}");
            AssertSkipAll("{ \"bar\": 42, \"foo\": true, \"qux\": \"baz\" }");
            AssertSkipAll("{ \"\": \"\", \"a\": \"a\", \"ab\": \"ab\" }");

            AssertSkipAllFail("");

            AssertSkipAllFail("a");
            AssertSkipAllFail("ra");
            AssertSkipAllFail("bar");

            AssertSkipAllFail("n");
            AssertSkipAllFail("nu");
            AssertSkipAllFail("nul");
            AssertSkipAllFail("nula");

            AssertSkipAllFail("Null");
            AssertSkipAllFail("nUll");
            AssertSkipAllFail("nuLl");
            AssertSkipAllFail("nulL");

            AssertSkipAllFail("t");
            AssertSkipAllFail("tr");
            AssertSkipAllFail("tru");
            AssertSkipAllFail("trua");

            AssertSkipAllFail("True");
            AssertSkipAllFail("tRue");
            AssertSkipAllFail("trUe");
            AssertSkipAllFail("truE");

            AssertSkipAllFail("f");
            AssertSkipAllFail("fa");
            AssertSkipAllFail("fal");
            AssertSkipAllFail("fals");
            AssertSkipAllFail("falsa");

            AssertSkipAllFail("False");
            AssertSkipAllFail("fAlse");
            AssertSkipAllFail("faLse");
            AssertSkipAllFail("falSe");
            AssertSkipAllFail("falsE");

            AssertSkipAllFail("-");

            AssertSkipAllFail("1.");
            AssertSkipAllFail("1.X");

            AssertSkipAllFail("42e");
            AssertSkipAllFail("987E");
            AssertSkipAllFail("42e*");
            AssertSkipAllFail("987E*");

            AssertSkipAllFail("1234e+");
            AssertSkipAllFail("12.4e-");
            AssertSkipAllFail("1234E+");
            AssertSkipAllFail("12.4E-");
            AssertSkipAllFail("1234e+Z");
            AssertSkipAllFail("12.4e-Z");
            AssertSkipAllFail("1234E+Z");
            AssertSkipAllFail("12.4E-Z");

            AssertSkipAllFail("\"");
            AssertSkipAllFail("\"b");
            AssertSkipAllFail("\"ba");
            AssertSkipAllFail("\"bar");
            AssertSkipAllFail("\"bar\\");
            AssertSkipAllFail("\"bar\\z");
            AssertSkipAllFail("\"bar\\u");
            AssertSkipAllFail("\"bar\\u1");
            AssertSkipAllFail("\"bar\\u12");
            AssertSkipAllFail("\"bar\\u123");
            AssertSkipAllFail("\"bar\\uX234");
            AssertSkipAllFail("\"bar\\u1X34");
            AssertSkipAllFail("\"bar\\u12X4");
            AssertSkipAllFail("\"bar\\u123X");

            AssertSkipAllFail("{");
            AssertSkipAllFail("{X");
            AssertSkipAllFail("{\"");
            AssertSkipAllFail("{\"b");
            AssertSkipAllFail("{\"ba");
            AssertSkipAllFail("{\"bar");
            AssertSkipAllFail("{\"bar\"");
            AssertSkipAllFail("{\"bar\":");
            AssertSkipAllFail("{\"bar\"X");
            AssertSkipAllFail("{\"bar\":42");
            AssertSkipAllFail("{\"bar\":42X");
            AssertSkipAllFail("{\"bar\":42,}");
            AssertSkipAllFail("{\"bar\":42,\"foo\":trueX");

            AssertSkipAllFail("[");
            AssertSkipAllFail("[X");
            AssertSkipAllFail("[42");
            AssertSkipAllFail("[42,");
            AssertSkipAllFail("[42X");
            AssertSkipAllFail("[42,]");
        }

        private static void AssertSkipAll(string s)
        {
            foreach (var prefix in new[] { "", " ", "\t" })
            {
                foreach (var suffix in new[] { "", " ", "\t" })
                {
                    var str = prefix + s + suffix;

                    var i = 0;

                    Parser.SkipOne(str, str.Length, ref i);

                    Assert.AreEqual(str.Length, i);
                }
            }

#if !NO_IO
            foreach (var prefix in new[] { "", " ", "\t" })
            {
                foreach (var suffix in new[] { "", " ", "\t" })
                {
                    var str = prefix + s + suffix;

                    var reader = new Reader(str);

                    Parser.SkipOne(reader);

                    Assert.AreEqual(str.Length, reader.Position);
                }
            }
#endif
        }

        private static void AssertSkipAllFail(string s)
        {
            foreach (var prefix in new[] { "", " ", "\t" })
            {
                foreach (var suffix in new[] { "", " ", "\t" })
                {
                    var str = prefix + s + suffix;

                    var i = 0;

                    Assert.ThrowsException<Nuqleon.Json.Parser.ParseException>(() => Parser.SkipOne(str, str.Length, ref i));
                }
            }

#if !NO_IO
            foreach (var prefix in new[] { "", " ", "\t" })
            {
                foreach (var suffix in new[] { "", " ", "\t" })
                {
                    var str = prefix + s + suffix;

                    Assert.ThrowsException<Nuqleon.Json.Parser.ParseException>(() => Parser.SkipOne(new Reader(str)));
                }
            }
#endif
        }
    }
}
