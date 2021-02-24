// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - May 2014 - Created this file.
//

using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Nuqleon.Json.Parser;

namespace Tests.Nuqleon.Json
{
    [TestClass]
    public class TokenizerTests
    {
        [TestMethod]
        public void Tokenizer_Potpourri()
        {
            var tests = new Dictionary<string, string>
            {
                { "true", "TRUE" },
                { "false", "FALSE" },

                { "null", "NULL" },

                { "\"foo\"", "STRING(foo)" },

                { "42", "NUM(42)" },
                { "-42", "NUM(-42)" },
                { "12.34", "NUM(12.34)" },

                { ":", "COLON" },
                { ",", "COMMA" },

                { "{", "LEFTCURLY" },
                { "}", "RIGHTCURLY" },

                { "[", "LEFTBRACKET" },
                { "]", "RIGHTBRACKET" },

                { " \t\r\n ", "WHITE" },

                { "\0", "EOF" },
            };

            foreach (var kv in tests)
            {
                AssertTokens(kv.Key, kv.Value);
            }
        }

        [TestMethod]
        public void Tokenizer_Numbers()
        {
            var tests = new List<string>
            {
                "0",
                "-0",

                "1",
                "-1",

                "42",
                "-42",

                "123",
                "-123",

                "0.1",
                "-0.1",

                "1.2",
                "-1.2",

                "12.34",
                "-12.34",

                "1e1",
                "-1e1",
                "1e+1",
                "-1e+1",
                "1e-1",
                "-1e-1",

                "1E1",
                "-1E1",
                "1E+1",
                "-1E+1",
                "1E-1",
                "-1E-1",

                "12.34e56",
                "-12.34e56",
                "12.34e+56",
                "-12.34e+56",
                "12.34e-56",
                "-12.34e-56",

                "12.34E56",
                "-12.34E56",
                "12.34E+56",
                "-12.34E+56",
                "12.34E-56",
                "-12.34E-56",
            };

            foreach (var test in tests)
            {
                AssertTokens(test, "NUM(" + test + ")");
            }
        }

        [TestMethod]
        public void Tokenizer_IsControl()
        {
            for (char c = '\x0000'; c <= '\x0100'; c++)
            {
                Assert.AreEqual(char.IsControl(c), Tokenizer.IsControl(c));
            }
        }

        private static void AssertTokens(string text, string tokens)
        {
            var res = Tokenize(text);
            Assert.AreEqual(tokens, res, "Unexpected tokenization: " + text);
        }

        private static string Tokenize(string text)
        {
            var tokens = new Tokenizer(text).Tokenize();
            return string.Join(";", AsEnumerable(tokens).Select(t => t.ToString()).ToArray());
        }

        private static IEnumerable<T> AsEnumerable<T>(IEnumerator<T> enumerator)
        {
            using (enumerator)
            {
                while (enumerator.MoveNext())
                {
                    yield return enumerator.Current;
                }
            }
        }
    }
}
