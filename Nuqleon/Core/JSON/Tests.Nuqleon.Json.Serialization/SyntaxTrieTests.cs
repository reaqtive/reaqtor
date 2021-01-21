// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 04/05/2016 - Created fast JSON deserializer functionality.
//   BD - 05/08/2016 - Added support for deserialization from text readers.
//

using Nuqleon.Json.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Tests
{
    [TestClass]
    public class SyntaxTrieTests
    {
        [TestMethod]
        public void SyntaxTrie_Basics()
        {
            var trie = new SyntaxTrie<string>();

            trie.Add("bar", "@bar");
            trie.Add("baz", "@baz");
            trie.Add("foo", "@foo");
            trie.Add("qux", "@qux");
            trie.Add("foobar", "@foobar");

            Assert.ThrowsException<InvalidOperationException>(() => trie.Add("foo", "@XXX"));

            Assert.AreEqual(3, trie.Root.Children.Count);
            Assert.AreEqual(1, trie.Root.Children['b'].Children.Count);
            Assert.AreEqual(2, trie.Root.Children['b'].Children['a'].Children.Count);
            Assert.AreEqual(0, trie.Root.Children['b'].Children['a'].Children['r'].Children.Count);
            Assert.AreEqual(0, trie.Root.Children['b'].Children['a'].Children['z'].Children.Count);
            Assert.AreEqual(1, trie.Root.Children['f'].Children.Count);
            Assert.AreEqual(1, trie.Root.Children['f'].Children['o'].Children.Count);
            Assert.AreEqual(1, trie.Root.Children['f'].Children['o'].Children['o'].Children.Count);
            Assert.AreEqual(1, trie.Root.Children['f'].Children['o'].Children['o'].Children['b'].Children.Count);
            Assert.AreEqual(1, trie.Root.Children['f'].Children['o'].Children['o'].Children['b'].Children['a'].Children.Count);
            Assert.AreEqual(0, trie.Root.Children['f'].Children['o'].Children['o'].Children['b'].Children['a'].Children['r'].Children.Count);
            Assert.AreEqual(1, trie.Root.Children['q'].Children.Count);
            Assert.AreEqual(1, trie.Root.Children['q'].Children['u'].Children.Count);
            Assert.AreEqual(0, trie.Root.Children['q'].Children['u'].Children['x'].Children.Count);

            Assert.IsNull(trie.Root.Terminal);
            Assert.IsNull(trie.Root.Children['b'].Terminal);
            Assert.IsNull(trie.Root.Children['b'].Children['a'].Terminal);
            Assert.IsNull(trie.Root.Children['f'].Terminal);
            Assert.IsNull(trie.Root.Children['f'].Children['o'].Terminal);
            Assert.IsNull(trie.Root.Children['f'].Children['o'].Children['o'].Children['b'].Terminal);
            Assert.IsNull(trie.Root.Children['f'].Children['o'].Children['o'].Children['b'].Children['a'].Terminal);
            Assert.IsNull(trie.Root.Children['q'].Terminal);
            Assert.IsNull(trie.Root.Children['q'].Children['u'].Terminal);
            Assert.AreEqual("@bar", trie.Root.Children['b'].Children['a'].Children['r'].Terminal);
            Assert.AreEqual("@baz", trie.Root.Children['b'].Children['a'].Children['z'].Terminal);
            Assert.AreEqual("@foo", trie.Root.Children['f'].Children['o'].Children['o'].Terminal);
            Assert.AreEqual("@foobar", trie.Root.Children['f'].Children['o'].Children['o'].Children['b'].Children['a'].Children['r'].Terminal);
            Assert.AreEqual("@qux", trie.Root.Children['q'].Children['u'].Children['x'].Terminal);

            Assert.AreEqual('b', trie.Root.Children['b'].Value); // NB: The Value property is used for diagnostic purposes only; we test it nonetheless.

            AssertTrieMatch(trie, new Dictionary<string, string>
            {
                { "bar", "@bar" },
                { "baz", "@baz" },
                { "foo", "@foo" },
                { "qux", "@qux" },
                { "foobar", "@foobar" },
            });

            AssertTrieNotMatch(trie,
                "b",
                "ba",
                "fo",
                "foob"
            );
        }

        [TestMethod]
        public void SyntaxTrie_Escapes()
        {
            // CHECK: When a prefix search resorts to CompareOrdinal, it accepts control characters like \t in the
            //        input string. Should we prevent this?

            var trie = new SyntaxTrie<string>();

            trie.Add("bar\tfoo", "TAB");
            trie.Add("bar\rfoo", "CRT");
            trie.Add("bar\nfoo", "NWL");
            trie.Add("bar\bfoo", "BCK");
            trie.Add("bar\ffoo", "FRM");
            trie.Add("bar/foo", "SLH");
            trie.Add("bar\\foo", "BSH");
            trie.Add("bar\"foo", "QTE");

            AssertTrieMatch(trie, new Dictionary<string, string>
            {
                { @"bar\tfoo", "TAB" },
                { @"bar\rfoo", "CRT" },
                { @"bar\nfoo", "NWL" },
                { @"bar\bfoo", "BCK" },
                { @"bar\ffoo", "FRM" },
                { @"bar\/foo", "SLH" },
                { @"bar\\foo", "BSH" },
                { @"bar\""foo", "QTE" },
            });
        }

        [TestMethod]
        public void SyntaxTrie_UTF16()
        {
            var trie = new SyntaxTrie<string>();

            trie.Add("far", "far");
            trie.Add("foo", "foo");

            AssertTrieMatch(trie, new Dictionary<string, string>
            {
                { @"f\u0061r", "far" },
                { @"f\u006Fo", "foo" },
            });
        }

        [TestMethod]
        public void SyntaxTrie_InvalidCharacter()
        {
            var trie = new SyntaxTrie<string>();

            trie.Add("barfoo", "foo");
            trie.Add("barqux", "qux");

            _ = trie.CompileString();

            AssertTrieMatchFail(trie,
                @"bar\",
                @"bar\z",
                "bar\tfoo",
                @"bar\u",
                @"bar\u1",
                @"bar\u12",
                @"bar\u123",
                @"bar\uX234",
                @"bar\uXX34",
                @"bar\uX2X4",
                @"bar\uX23X"
            );
        }

        [TestMethod]
        public void SyntaxTrie_ManOrBoy()
        {
            // NB: Building a syntax trie based on full type names of BCL types tests prefix searches extensively
            //     due to the repetition of namespace names.

            var trie = new SyntaxTrie<Type>();

            foreach (var t in typeof(int).Assembly.GetTypes())
            {
                trie.Add(t.FullName, t);
            }

            var inputs = typeof(int).Assembly.GetTypes().ToDictionary(t => t.FullName, t => t);

            AssertTrieMatch(trie, inputs);
        }

        private static void AssertTrieMatch<T>(SyntaxTrie<T> syntaxTrie, Dictionary<string, T> tests)
           where T : class
        {
            {
                var trie = syntaxTrie.CompileString();

                foreach (var test in tests)
                {
                    var input = test.Key;
                    var expected = test.Value;

                    var str = input + "\"";
                    var i = 0;

                    Assert.IsTrue(trie.Eval(str, str.Length, i, ref i, out var res));

                    Assert.AreEqual(expected, trie.Terminals[res]);

                    Assert.AreEqual(str.Length - 1, i);
                }
            }

#if !NO_IO
            {
                var trie = syntaxTrie.CompileReader();

                foreach (var test in tests)
                {
                    var input = test.Key;
                    var expected = test.Value;

                    var str = input + "\"";
                    var reader = new Reader(str);

                    Assert.IsTrue(trie.Eval(reader, out var res));

                    Assert.AreEqual(expected, trie.Terminals[res]);

                    Assert.AreEqual(str.Length - 1, reader.Position);
                }
            }
#endif
        }

        private static void AssertTrieNotMatch<T>(SyntaxTrie<T> syntaxTrie, params string[] inputs)
            where T : class
        {
            {
                var trie = syntaxTrie.CompileString();

                foreach (var input in inputs)
                {
                    var str = input + "\"";
                    var i = 0;

                    Assert.IsFalse(trie.Eval(str, str.Length, i, ref i, out var res));
                }
            }

#if !NO_IO
            {
                var trie = syntaxTrie.CompileReader();

                foreach (var input in inputs)
                {
                    var str = input + "\"";
                    var reader = new Reader(str);

                    Assert.IsFalse(trie.Eval(reader, out var res));
                }
            }
#endif
        }

        private static void AssertTrieMatchFail<T>(SyntaxTrie<T> syntaxTrie, params string[] inputs)
            where T : class
        {
            {
                var trie = syntaxTrie.CompileString();

                foreach (var input in inputs)
                {
                    var str = input;
                    var i = 0;

                    var res = default(int);
                    Assert.ThrowsException<Nuqleon.Json.Parser.ParseException>(() => trie.Eval(str, str.Length, i, ref i, out res));
                }
            }

#if !NO_IO
            {
                var trie = syntaxTrie.CompileReader();

                foreach (var input in inputs)
                {
                    var str = input;
                    var reader = new Reader(str);

                    var res = default(int);
                    Assert.ThrowsException<Nuqleon.Json.Parser.ParseException>(() => trie.Eval(reader, out res));
                }
            }
#endif
        }
    }
}
