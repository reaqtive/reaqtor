// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Memory.Diagnostics;

namespace Tests
{
    [TestClass]
    public class FastHeapReferenceWalkerTests
    {
        [TestMethod]
        public void FastHeapReferenceWalker_Null()
        {
            var w = new FastHeapReferenceWalker();

            w.Walk(obj: null, _ =>
            {
                Assert.Fail();
                return true;
            });
        }

        [TestMethod]
        public void FastHeapReferenceWalker_BoxedValue()
        {
            var w = new FastHeapReferenceWalker();

            var x = (object)42;

            var set = new HashSet<object>(ReferenceEqualityComparer<object>.Instance);

            w.Walk(x, set.Add);

            AssertSetEquals(set, x);
        }

        [TestMethod]
        public void FastHeapReferenceWalker_Object()
        {
            var w = new FastHeapReferenceWalker();

            var x = new object();

            var set = new HashSet<object>(ReferenceEqualityComparer<object>.Instance);

            w.Walk(x, set.Add);

            AssertSetEquals(set, x);
        }

        [TestMethod]
        public void FastHeapReferenceWalker_Vector_Ref1()
        {
            var w = new FastHeapReferenceWalker();

            var xs = new[] { new object(), new object(), new object() };

            var set = new HashSet<object>(ReferenceEqualityComparer<object>.Instance);

            w.Walk(xs, set.Add);

            AssertSetEquals(set, new object[] { xs }.Concat(xs));
        }

        [TestMethod]
        public void FastHeapReferenceWalker_Vector_Ref2()
        {
            var w = new FastHeapReferenceWalker();

            var xs = new object[] { new object(), "bar", 42 };

            var set = new HashSet<object>(ReferenceEqualityComparer<object>.Instance);

            w.Walk(xs, set.Add);

            AssertSetEquals(set, new object[] { xs }.Concat(xs));
        }

        [TestMethod]
        public void FastHeapReferenceWalker_Vector_Value1()
        {
            var w = new FastHeapReferenceWalker();

            var xs = new[] { new KeyValuePair<int, int>(1, 2), new KeyValuePair<int, int>(2, 3), new KeyValuePair<int, int>(3, 4) };

            var set = new HashSet<object>(ReferenceEqualityComparer<object>.Instance);

            w.Walk(xs, set.Add);

            AssertSetEquals(set, new object[] { xs });
        }

        [TestMethod]
        public void FastHeapReferenceWalker_Vector_Value2()
        {
            var w = new FastHeapReferenceWalker();

            var xs = new[] { new KeyValuePair<int, object>(1, "bar"), new KeyValuePair<int, object>(2, new object()), new KeyValuePair<int, object>(3, 42) };

            var set = new HashSet<object>(ReferenceEqualityComparer<object>.Instance);

            w.Walk(xs, set.Add);

            AssertSetEquals(set, new object[] { xs }.Concat(xs.Select(x => x.Value)));
        }

        [TestMethod]
        public void FastHeapReferenceWalker_Multidimensional_Ref1()
        {
            var w = new FastHeapReferenceWalker();

            var xs = new object[2, 3] { { new object(), "bar", 42 }, { new object(), "qux", 43 } };

            var set = new HashSet<object>(ReferenceEqualityComparer<object>.Instance);

            w.Walk(xs, set.Add);

            AssertSetEquals(set, new object[] { xs }.Concat(xs.Cast<object>()));
        }

        [TestMethod]
        public void FastHeapReferenceWalker_Multidimensional_Value1()
        {
            var w = new FastHeapReferenceWalker();

            var xs = new KeyValuePair<int, object>[2, 3]
            {
                { new KeyValuePair<int, object>(1, "bar"), new KeyValuePair<int, object>(2, new object()), new KeyValuePair<int, object>(3, 42) },
                { new KeyValuePair<int, object>(4, "baz"), new KeyValuePair<int, object>(5, new object()), new KeyValuePair<int, object>(6, 43) },
            };

            var set = new HashSet<object>(ReferenceEqualityComparer<object>.Instance);

            w.Walk(xs, set.Add);

            AssertSetEquals(set, new object[] { xs }.Concat(xs.Cast<KeyValuePair<int, object>>().Select(kv => kv.Value)));
        }

        [TestMethod]
        public void FastHeapReferenceWalker_Deep()
        {
            var w = new FastHeapReferenceWalker();

            var bar =
                new Bar
                {
                    X = 1,
                    Y = "bar",
                    Z = new Foo
                    {
                        A = 2,
                        B = "qux",
                        C = new[]
                        {
                            new Qux
                            {
                                I = 3,
                                J = "foo",
                                K = new Baz
                                {
                                    N = 4,
                                    O = "baz",
                                    P = new[]
                                    {
                                        "abc",
                                        new object()
                                    }
                                }
                            },
                            new Qux
                            {
                                I = 5,
                                J = "FOO",
                                K = new Baz
                                {
                                    N = 6,
                                    O = "BAZ",
                                    P = new[]
                                    {
                                        "xyz",
                                        new object()
                                    }
                                }
                            }
                        }
                    }
                };

            var set = new HashSet<object>(ReferenceEqualityComparer<object>.Instance);

            w.Walk(bar, set.Add);

            AssertSetEquals(set, new object[]
            {
                bar,
                bar.Y,
                bar.Z.B,
                bar.Z.C,
                bar.Z.C[0].J,
                bar.Z.C[0].K.O,
                bar.Z.C[0].K.P,
                bar.Z.C[0].K.P[0],
                bar.Z.C[0].K.P[1],
                bar.Z.C[1].J,
                bar.Z.C[1].K.O,
                bar.Z.C[1].K.P,
                bar.Z.C[1].K.P[0],
                bar.Z.C[1].K.P[1],
            });
        }

        private static void AssertSetEquals(HashSet<object> set, IEnumerable<object> objects) => AssertSetEquals(set, objects.ToArray());

        private static void AssertSetEquals(HashSet<object> set, params object[] objects)
        {
            var other = new HashSet<object>(objects, ReferenceEqualityComparer<object>.Instance);
            Assert.IsTrue(set.SetEquals(other));
        }

        private sealed class Bar
        {
            public int X;
            public string Y;
            public Foo Z;
        }

        private struct Foo
        {
            public int A;
            public string B;
            public Qux[] C;
        }

        private struct Qux
        {
            public int I;
            public string J;
            public Baz K;
        }

        private struct Baz
        {
            public int N;
            public string O;
            public object[] P;
        }
    }
}
