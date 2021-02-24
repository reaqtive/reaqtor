// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - May 2013 - Created this file.
//

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq.CompilerServices;

namespace Tests.System.Linq.CompilerServices
{
    [TestClass]
    public class SyntaxTrieTests
    {
        [TestMethod]
        public void SyntaxTrie_ArgumentChecking()
        {
            var st = new SyntaxTrie();

            AssertEx.ThrowsException<ArgumentNullException>(() => st.Add(identifier: null), ex => Assert.AreEqual("identifier", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => st.Contains(identifier: null), ex => Assert.AreEqual("identifier", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => st.Remove(identifier: null), ex => Assert.AreEqual("identifier", ex.ParamName));
        }

        [TestMethod]
        public void SyntaxTrie_Basics()
        {
            var st = new SyntaxTrie();

            // { }
            {
                Assert.IsFalse(st.Contains("foo"));
                Assert.IsFalse(st.Contains("bar"));
                Assert.IsFalse(st.Contains("baz"));

                Assert.IsFalse(st.Contains("fo"));
                Assert.IsFalse(st.Contains("f"));
                Assert.IsFalse(st.Contains("ba"));
                Assert.IsFalse(st.Contains("b"));
            }

            // { foo }
            {
                st.Add("foo");
                Assert.IsTrue(st.Contains("foo"));
                Assert.IsFalse(st.Contains("bar"));
                Assert.IsFalse(st.Contains("baz"));

                Assert.IsFalse(st.Contains("fo"));
                Assert.IsFalse(st.Contains("f"));
                Assert.IsFalse(st.Contains("ba"));
                Assert.IsFalse(st.Contains("b"));
            }

            // { }
            {
                Assert.IsFalse(st.Remove("f"));
                Assert.IsFalse(st.Remove("fo"));
                Assert.IsFalse(st.Remove("b"));
                Assert.IsFalse(st.Remove("ba"));

                Assert.IsTrue(st.Remove("foo"));
                Assert.IsFalse(st.Contains("foo"));
                Assert.IsFalse(st.Remove("foo"));
                Assert.IsFalse(st.Contains("bar"));
                Assert.IsFalse(st.Contains("baz"));

                Assert.IsFalse(st.Contains("fo"));
                Assert.IsFalse(st.Contains("f"));
                Assert.IsFalse(st.Contains("ba"));
                Assert.IsFalse(st.Contains("b"));
            }

            // { foo }
            {
                Assert.IsFalse(st.Contains("fo"));
                Assert.IsFalse(st.Contains("f"));
                Assert.IsFalse(st.Contains("ba"));
                Assert.IsFalse(st.Contains("b"));

                st.Add("foo");
                Assert.IsTrue(st.Contains("foo"));
                Assert.IsFalse(st.Contains("bar"));
                Assert.IsFalse(st.Contains("baz"));

                Assert.IsFalse(st.Remove("f"));
                Assert.IsFalse(st.Remove("fo"));
                Assert.IsFalse(st.Remove("b"));
                Assert.IsFalse(st.Remove("ba"));
            }

            // { foo, bar }
            {
                Assert.IsFalse(st.Remove("f"));
                Assert.IsFalse(st.Remove("fo"));
                Assert.IsFalse(st.Remove("b"));
                Assert.IsFalse(st.Remove("ba"));

                st.Add("bar");
                Assert.IsTrue(st.Contains("foo"));
                Assert.IsTrue(st.Contains("bar"));
                Assert.IsFalse(st.Contains("baz"));

                Assert.IsFalse(st.Contains("fo"));
                Assert.IsFalse(st.Contains("f"));
                Assert.IsFalse(st.Contains("ba"));
                Assert.IsFalse(st.Contains("b"));
            }

            // { foo, bar, baz }
            {
                Assert.IsFalse(st.Remove("f"));
                Assert.IsFalse(st.Remove("fo"));
                Assert.IsFalse(st.Remove("b"));
                Assert.IsFalse(st.Remove("ba"));

                st.Add("baz");
                Assert.IsTrue(st.Contains("foo"));
                Assert.IsTrue(st.Contains("bar"));
                Assert.IsTrue(st.Contains("baz"));

                Assert.IsFalse(st.Contains("fo"));
                Assert.IsFalse(st.Contains("f"));
                Assert.IsFalse(st.Contains("ba"));
                Assert.IsFalse(st.Contains("b"));
            }

            // { foo, baz }
            {
                Assert.IsFalse(st.Remove("f"));
                Assert.IsFalse(st.Remove("fo"));
                Assert.IsFalse(st.Remove("b"));
                Assert.IsFalse(st.Remove("ba"));

                Assert.IsTrue(st.Remove("bar"));
                Assert.IsTrue(st.Contains("foo"));
                Assert.IsFalse(st.Contains("bar"));
                Assert.IsTrue(st.Contains("baz"));
                Assert.IsFalse(st.Remove("bar"));

                Assert.IsFalse(st.Contains("fo"));
                Assert.IsFalse(st.Contains("f"));
                Assert.IsFalse(st.Contains("ba"));
                Assert.IsFalse(st.Contains("b"));
            }

            // { baz }
            {
                Assert.IsFalse(st.Contains("fo"));
                Assert.IsFalse(st.Contains("f"));
                Assert.IsFalse(st.Contains("ba"));
                Assert.IsFalse(st.Contains("b"));

                Assert.IsTrue(st.Remove("foo"));
                Assert.IsFalse(st.Contains("foo"));
                Assert.IsFalse(st.Contains("bar"));
                Assert.IsTrue(st.Contains("baz"));
                Assert.IsFalse(st.Remove("foo"));

                Assert.IsFalse(st.Remove("f"));
                Assert.IsFalse(st.Remove("fo"));
                Assert.IsFalse(st.Remove("b"));
                Assert.IsFalse(st.Remove("ba"));
            }

            // { }
            {
                Assert.IsTrue(st.Remove("baz"));
                Assert.IsFalse(st.Contains("foo"));
                Assert.IsFalse(st.Contains("bar"));
                Assert.IsFalse(st.Contains("baz"));
                Assert.IsFalse(st.Remove("baz"));

                Assert.IsFalse(st.Contains("fo"));
                Assert.IsFalse(st.Contains("f"));
                Assert.IsFalse(st.Contains("ba"));
                Assert.IsFalse(st.Contains("b"));

                Assert.IsFalse(st.Remove("f"));
                Assert.IsFalse(st.Remove("fo"));
                Assert.IsFalse(st.Remove("b"));
                Assert.IsFalse(st.Remove("ba"));
            }
        }
    }
}
