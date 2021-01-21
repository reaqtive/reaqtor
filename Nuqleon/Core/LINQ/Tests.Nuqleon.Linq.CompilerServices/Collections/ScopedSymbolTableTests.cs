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
using System.Linq;
using System.Linq.CompilerServices;

namespace Tests.System.Linq.CompilerServices
{
    [TestClass]
    public class ScopedSymbolTableTests
    {
        [TestMethod]
        public void ScopedSymbolTable_ArgumentChecking()
        {
            AssertEx.ThrowsException<ArgumentNullException>(() => new ScopedSymbolTable<string, string>(symbolComparer: null), ex => Assert.AreEqual("symbolComparer", ex.ParamName));
        }

        [TestMethod]
        public void ScopedSymbolTable_Basics()
        {
            var res = default(Indexed<Indexed<string>>);

            var sst = new ScopedSymbolTable<string, string>
            {
                { "glb", "val" }
            };

            var assertGlobal = new Action(() =>
            {
                Assert.IsTrue(sst.TryLookup("glb", out res));
                Assert.AreEqual(-1, res.Index);
                Assert.AreEqual(0, res.Value.Index);
                Assert.AreEqual("val", res.Value.Value);

                Assert.IsFalse(sst.TryLookup("blg", out res));
            });

            assertGlobal();

            AssertPushPop(sst,
                () =>
                {
                    assertGlobal();

                    Assert.IsFalse(sst.TryLookup("bar", out res));
                },
                () =>
                {
                    sst.Add("bar", "foo");
                    sst.Add("baz", "qux");

                    AssertPushPop(sst,
                        () =>
                        {
                            assertGlobal();

                            Assert.IsTrue(sst.TryLookup("bar", out res));
                            Assert.AreEqual(0, res.Index);
                            Assert.AreEqual(0, res.Value.Index);
                            Assert.AreEqual("foo", res.Value.Value);

                            Assert.IsTrue(sst.TryLookup("baz", out res));
                            Assert.AreEqual(0, res.Index);
                            Assert.AreEqual(1, res.Value.Index);
                            Assert.AreEqual("qux", res.Value.Value);
                        },
                        () =>
                        {
                            sst.Add("foo", "bar");
                            sst.Add("bar", "qux");

                            AssertPushPop(sst,
                                () =>
                                {
                                    assertGlobal();

                                    Assert.IsTrue(sst.TryLookup("foo", out res));
                                    Assert.AreEqual(0, res.Index);
                                    Assert.AreEqual(0, res.Value.Index);
                                    Assert.AreEqual("bar", res.Value.Value);

                                    Assert.IsTrue(sst.TryLookup("bar", out res));
                                    Assert.AreEqual(0, res.Index);
                                    Assert.AreEqual(1, res.Value.Index);
                                    Assert.AreEqual("qux", res.Value.Value);

                                    Assert.IsTrue(sst.TryLookup("baz", out res));
                                    Assert.AreEqual(1, res.Index);
                                    Assert.AreEqual(1, res.Value.Index);
                                    Assert.AreEqual("qux", res.Value.Value);
                                },
                                () =>
                                {
                                    sst.Add("qux", "baz");

                                    AssertPushPop(sst,
                                        () =>
                                        {
                                            assertGlobal();

                                            Assert.IsTrue(sst.TryLookup("qux", out res));
                                            Assert.AreEqual(0, res.Index);
                                            Assert.AreEqual(0, res.Value.Index);
                                            Assert.AreEqual("baz", res.Value.Value);

                                            Assert.IsTrue(sst.TryLookup("foo", out res));
                                            Assert.AreEqual(1, res.Index);
                                            Assert.AreEqual(0, res.Value.Index);
                                            Assert.AreEqual("bar", res.Value.Value);

                                            Assert.IsTrue(sst.TryLookup("bar", out res));
                                            Assert.AreEqual(1, res.Index);
                                            Assert.AreEqual(1, res.Value.Index);
                                            Assert.AreEqual("qux", res.Value.Value);

                                            Assert.IsTrue(sst.TryLookup("baz", out res));
                                            Assert.AreEqual(2, res.Index);
                                            Assert.AreEqual(1, res.Value.Index);
                                            Assert.AreEqual("qux", res.Value.Value);
                                        },
                                        () =>
                                        {
                                            // NOTE: The test method pushes a frame before entering, so we're off by 1 for the outer indices.

                                            Assert.IsNotNull(((IEnumerable)sst).GetEnumerator());

                                            var symbols = sst.SelectMany(ist => ist.Value, (o, i) => (Outer: o.Index, Inner: i.Index, Symbol: i.Value.Key, i.Value.Value)).ToArray();

                                            Assert.IsTrue(symbols.SequenceEqual(new[]
                                            {
                                                ( Outer : 1, Inner : 0, Symbol : "qux", Value : "baz" ),
                                                ( Outer : 2, Inner : 0, Symbol : "foo", Value : "bar" ),
                                                ( Outer : 2, Inner : 1, Symbol : "bar", Value : "qux" ),
                                                ( Outer : 3, Inner : 0, Symbol : "bar", Value : "foo" ),
                                                ( Outer : 3, Inner : 1, Symbol : "baz", Value : "qux" ),
                                                ( Outer : -1, Inner : 0, Symbol : "glb", Value : "val" ),
                                            }));
                                        }
                                    );
                                }
                            );
                        }
                    );
                }
            );
        }

        private static void AssertPushPop(ScopedSymbolTable<string, string> sst, Action assertBeforeAfter, Action inner)
        {
            assertBeforeAfter();

            sst.Push();
            inner();
            sst.Pop();

            assertBeforeAfter();
        }
    }
}
