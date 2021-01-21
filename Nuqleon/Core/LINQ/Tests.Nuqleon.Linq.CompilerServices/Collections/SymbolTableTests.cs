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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.CompilerServices;

namespace Tests.System.Linq.CompilerServices
{
    [TestClass]
    public class SymbolTableTests
    {
        [TestMethod]
        public void SymbolTable_ArgumentChecking()
        {
            AssertEx.ThrowsException<ArgumentNullException>(() => new SymbolTable<string, string>(parent: null, symbolComparer: null), ex => Assert.AreEqual("symbolComparer", ex.ParamName));
        }

        [TestMethod]
        public void SymbolTable_Basics()
        {
            var st = new SymbolTable<string, string>(parent: null)
            {
                { "bar", "foo" },
                { "baz", "qux" },
            };

            Assert.IsNull(st.Parent);

            Assert.ThrowsException<InvalidOperationException>(() => st.Add("bar", "qux"));

            Assert.IsTrue(st.TryGetValue("bar", out Indexed<string> foo));
            Assert.AreEqual("foo", foo.Value);
            Assert.AreEqual(0, foo.Index);
            Assert.AreEqual(foo, st["bar"]);

            Assert.IsTrue(st.TryGetValue("baz", out Indexed<string> qux));
            Assert.AreEqual("qux", qux.Value);
            Assert.AreEqual(1, qux.Index);

            Assert.IsFalse(st.TryGetValue("foo", out Indexed<string> unk));

            Assert.IsTrue(st.ToArray().SequenceEqual(new[] {
                new Indexed<KeyValuePair<string, string>>(new KeyValuePair<string, string>("bar", "foo"), 0),
                new Indexed<KeyValuePair<string, string>>(new KeyValuePair<string, string>("baz", "qux"), 1)
            }));

            Assert.IsNotNull(((IEnumerable)st).GetEnumerator());
        }

        [TestMethod]
        public void SymbolTable_Nested()
        {
            var st1 = new SymbolTable<string, string>(parent: null)
            {
                { "bar", "foo" },
                { "baz", "xuq" },
            };

            var st2 = new SymbolTable<string, string>(st1)
            {
                { "foo", "bar" },
                { "bar", "qux" },
            };

            var st3 = new SymbolTable<string, string>(st2)
            {
                { "qux", "baz" },
            };

            Assert.IsFalse(st3.TryLookup("nop", out _));

            Assert.IsTrue(st3.TryLookup("qux", out Indexed<Indexed<string>> res2));
            Assert.AreEqual(0, res2.Index);
            Assert.AreEqual(0, res2.Value.Index);
            Assert.AreEqual("baz", res2.Value.Value);

            Assert.IsTrue(st3.TryLookup("bar", out Indexed<Indexed<string>> res3));
            Assert.AreEqual(1, res3.Index);
            Assert.AreEqual(1, res3.Value.Index);
            Assert.AreEqual("qux", res3.Value.Value);

            Assert.IsTrue(st3.TryLookup("baz", out Indexed<Indexed<string>> res4));
            Assert.AreEqual(2, res4.Index);
            Assert.AreEqual(1, res4.Value.Index);
            Assert.AreEqual("xuq", res4.Value.Value);
        }
    }
}
