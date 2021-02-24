// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 04/05/2016 - Created fast JSON deserializer functionality.
//   BD - 05/08/2016 - Added support for deserialization from text readers.
//

using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Nuqleon.Json.Serialization;

namespace Tests
{
    [TestClass]
    public partial class ParserTests
    {
        [TestMethod]
        public void FastParser_StartsWith()
        {
            AssertStartsWith("", "", 0);
            AssertStartsWith("b", "", 0);
            AssertStartsWith("ba", "", 0);
            AssertStartsWith("bar", "", 0);
            AssertStartsWith("bar", "b", 1);
            AssertStartsWith("bar", "ba", 2);
            AssertStartsWith("bar", "bar", 3);
            AssertStartsWith("barfoo", "bar", 3);

            AssertStartsWith(@"bar\r", "bar", 3);
            AssertStartsWith(@"bar\r", "bar\r", 5);
            AssertStartsWith(@"bar\n", "bar", 3);
            AssertStartsWith(@"bar\n", "bar\n", 5);
            AssertStartsWith(@"bar\t", "bar", 3);
            AssertStartsWith(@"bar\t", "bar\t", 5);
            AssertStartsWith(@"bar\b", "bar", 3);
            AssertStartsWith(@"bar\b", "bar\b", 5);
            AssertStartsWith(@"bar\f", "bar", 3);
            AssertStartsWith(@"bar\f", "bar\f", 5);
            AssertStartsWith(@"bar\\", "bar", 3);
            AssertStartsWith(@"bar\\", "bar\\", 5);
            AssertStartsWith(@"bar\""", "bar", 3);
            AssertStartsWith(@"bar\""", "bar\"", 5);
            AssertStartsWith(@"bar\/", "bar", 3);
            AssertStartsWith(@"bar\/", "bar/", 5);

            AssertStartsWith(@"b\u0061r", "bar", 8);
            AssertStartsWith(@"f\u006Fo", "foo", 8);
            AssertStartsWith(@"f\u006fobar", "foobar", 11);

            AssertStartsWith(@"c:\\temp\\foo.txt", @"c:\temp", 8);
            AssertStartsWith(@"c:\\temp\\foo.txt", @"c:\temp\", 10);

            AssertStartsWith(@"bar\r\nfoo\tqux", "bar\r\nfoo\tqux", 15);
            AssertStartsWith(@"bar\r\nfoo\tqux\bbaz\fbar", "bar\r\nfoo\tqux\bbaz\fbar", 25);

            AssertNotStartsWith("", "b");
            AssertNotStartsWith("bar", "foo");
            AssertNotStartsWith("bar", "boo");
            AssertNotStartsWith("bar", "bao");
            AssertNotStartsWith("bar", "barfoo");
            AssertNotStartsWith("bar\"", "barfoo");

            AssertStartsWithFail("\t", "bar");
            AssertStartsWithFail("\\", "bar");
            AssertStartsWithFail("\\z", "bar");
            AssertStartsWithFail("\\u", "bar");
            AssertStartsWithFail("\\u1", "bar");
            AssertStartsWithFail("\\u12", "bar");
            AssertStartsWithFail("\\u123", "bar");
            AssertStartsWithFail("\\uX234", "bar");
            AssertStartsWithFail("\\u1X34", "bar");
            AssertStartsWithFail("\\u12X4", "bar");
            AssertStartsWithFail("\\u123X", "bar");
        }

        private static void AssertStartsWith(string json, string value, int afterMatch)
        {
            foreach (var suffix in new[] { "", "*", "**" })
            {
                var inp = json + suffix;

                var i = 0;
                Assert.IsTrue(Parser.StartsWith(inp, inp.Length, 0, ref i, value));
                Assert.AreEqual(afterMatch, i);

#if !NO_IO
                var reader = new Reader(inp);
                Assert.IsTrue(Parser.StartsWithReader(reader, value));
                Assert.AreEqual(afterMatch, reader.Position);
#endif
            }
        }

        private static void AssertNotStartsWith(string json, string value)
        {
            foreach (var suffix in new[] { "", "*", "**" })
            {
                var inp = json + suffix;

                var i = 0;
                Assert.IsFalse(Parser.StartsWith(inp, inp.Length, 0, ref i, value));
                Assert.AreEqual(0, i);

#if !NO_IO
                var reader = new Reader(inp);
                Assert.IsFalse(Parser.StartsWithReader(reader, value));

                //
                // NB: By design for the StartsWithReader variant of StartsWith. Due to limitations
                //     of only being able to peek ahead one character at a time, this helper function
                //     cannot check without consuming characters from the reader. This does not cause
                //     any problems for compiled syntax tries in today's implementation though.
                //
                // Assert.AreEqual(0, reader.Position);
#endif
            }
        }

        private static void AssertStartsWithFail(string json, string value)
        {
            foreach (var suffix in new[] { "", "*", "**" })
            {
                var inp = json + suffix;

                var i = 0;
                Assert.ThrowsException<Nuqleon.Json.Parser.ParseException>(() => Parser.StartsWith(inp, inp.Length, 0, ref i, value));

#if !NO_IO
                var reader = new Reader(inp);
                Assert.ThrowsException<Nuqleon.Json.Parser.ParseException>(() => Parser.StartsWithReader(reader, value));
#endif
            }
        }

        private static void AssertParse<T>(ParseString<T> parser, string s, T expected)
        {
            AssertParseCore(parser, s, expected);

#if !NO_IO
            var parseReaderMtd = parser.Method.DeclaringType.GetMethod(parser.Method.Name, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static, binder: null, new[] { typeof(System.IO.TextReader), typeof(ParserContext) }, modifiers: null);
            var parseReaderFnc = (ParseReader<T>)Delegate.CreateDelegate(typeof(ParseReader<T>), parseReaderMtd);
            var parserWithReader = ParseWithReader(parseReaderFnc);
            AssertParseCore(parserWithReader, s, expected);
#endif
        }

        private static void AssertParseCore<T>(ParseString<T> parser, string s, T expected)
        {
            foreach (var prefix in new[] { "", " ", ",", "[" })
            {
                foreach (var suffix in new[] { "", " ", ",", "]" })
                {
                    var str = prefix + s + suffix;

                    var i = prefix.Length;
                    var b = i;
                    var ctx = GetParserContext();

                    var res = parser(str, str.Length, ref i, ctx);

                    Assert.AreEqual(expected, res);
                    Assert.AreEqual(b + s.Length, i);
                }
            }
        }

        private static void AssertParseFail<T>(ParseString<T> parser, string s)
        {
            AssertParseFailCore(parser, s);

#if !NO_IO
            var parseReaderMtd = parser.Method.DeclaringType.GetMethod(parser.Method.Name, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static, binder: null, new[] { typeof(System.IO.TextReader), typeof(ParserContext) }, modifiers: null);
            var parseReaderFnc = (ParseReader<T>)Delegate.CreateDelegate(typeof(ParseReader<T>), parseReaderMtd);
            var parserWithReader = ParseWithReader(parseReaderFnc);
            AssertParseFailCore(parserWithReader, s);
#endif
        }

        private static void AssertParseFailCore<T>(ParseString<T> parser, string s)
        {
            foreach (var prefix in new[] { "", " ", ",", "[" })
            {
                foreach (var suffix in new[] { "", " ", ",", "]" })
                {
                    var str = prefix + s + suffix;

                    var i = prefix.Length;
                    var b = i;
                    var ctx = GetParserContext();

                    Assert.ThrowsException<Nuqleon.Json.Parser.ParseException>(() => parser(str, str.Length, ref i, ctx));
                }
            }
        }

        private static ParserContext GetParserContext()
        {
            return new ParserContext();
        }

        private delegate T ParseString<T>(string str, int len, ref int i, ParserContext context);

#if !NO_IO
        private delegate T ParseReader<T>(System.IO.TextReader reader, ParserContext context);

        private static ParseString<T> ParseWithReader<T>(ParseReader<T> parse)
        {
            return (string str, int len, ref int i, ParserContext ctx) =>
            {
                var reader = new Reader(str);

                for (var j = 0; j < i; j++)
                    reader.Read();

                var res = parse(reader, ctx);

                i = reader.Position;

                return res;
            };
        }
#endif
    }

#if !NO_IO
    internal sealed class Reader : System.IO.StringReader
    {
        public Reader(string s)
            : base(s)
        {
        }

        public int Position { get; private set; }

        public override int Read()
        {
            Position++;
            return base.Read();
        }

        public override int Read(char[] buffer, int index, int count)
        {
            var res = base.Read(buffer, index, count);
            Position += res;
            return res;
        }
    }
#endif
}
