// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 03/30/2016 - Created StringSegment functionality.
//

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class StringSegmentTests
    {
        //=================================================================\\
        //    _____                _                   _                   \\
        //   / ____|              | |                 | |                  \\
        //  | |     ___  _ __  ___| |_ _ __ _   _  ___| |_ ___  _ __ ___   \\
        //  | |    / _ \| '_ \/ __| __| '__| | | |/ __| __/ _ \| '__/ __|  \\
        //  | |___| (_) | | | \__ \ |_| |  | |_| | (__| || (_) | |  \__ \  \\
        //   \_____\___/|_| |_|___/\__|_|   \__,_|\___|\__\___/|_|  |___/  \\
        //                                                                 \\
        //=================================================================\\

        [TestMethod]
        public void StringSegment_Ctor()
        {
            var s = new StringSegment();

            Assert.IsNull(s.String);
            Assert.AreEqual(0, s.StartIndex);
            Assert.AreEqual(0, s.Length);

            Assert.AreEqual(string.Empty, (string)s);
        }

        [TestMethod]
        public void StringSegment_Ctor_String_ArgumentChecking()
        {
            Assert.ThrowsException<ArgumentNullException>(() => _ = new StringSegment(default(string)));
        }

        [TestMethod]
        public void StringSegment_Ctor_String()
        {
            var value = "Bar";
            var s = new StringSegment(value);

            Assert.AreSame(value, s.String);
            Assert.AreEqual(0, s.StartIndex);
            Assert.AreEqual(value.Length, s.Length);

            Assert.AreSame(value, (string)s);
        }

        [TestMethod]
        public void StringSegment_Ctor_CharArray()
        {
            var value = "Bar";
            var s = new StringSegment(value.ToCharArray());

            Assert.AreEqual(value, s.String);
            Assert.AreEqual(0, s.StartIndex);
            Assert.AreEqual(value.Length, s.Length);

            Assert.AreEqual(value, (string)s);
        }

        [TestMethod]
        public void StringSegment_Ctor_CharArraySubstring_ArgumentChecking()
        {
            var c = new char[5];

            Assert.ThrowsException<ArgumentNullException>(() => _ = new StringSegment(default(char[]), 0, 0));

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => _ = new StringSegment(c, -1, 0));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => _ = new StringSegment(c, 0, -1));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => _ = new StringSegment(c, 5, 1));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => _ = new StringSegment(c, 4, 2));
        }

        [TestMethod]
        public void StringSegment_Ctor_CharArraySubstring()
        {
            var c = new[] { 'q', 'B', 'a', 'r', 'f', 'o' };
            var s = new StringSegment(c, 1, 3);

            Assert.AreEqual("Bar", s.String);
            Assert.AreEqual(0, s.StartIndex);
            Assert.AreEqual(3, s.Length);

            Assert.AreEqual("Bar", (string)s);
        }

        [TestMethod]
        public void StringSegment_Ctor_CharCount_ArgumentChecking()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => _ = new StringSegment('a', -1));
        }

        [TestMethod]
        public void StringSegment_Ctor_CharCount()
        {
            var s = new StringSegment('b', 3);

            Assert.AreEqual("bbb", s.String);
            Assert.AreEqual(0, s.StartIndex);
            Assert.AreEqual(3, s.Length);

            Assert.AreEqual("bbb", (string)s);
        }

        [TestMethod]
        public void StringSegment_Ctor_Substring_ArgumentChecking()
        {
            var s = "seven";

            Assert.ThrowsException<ArgumentNullException>(() => _ = new StringSegment(default(string), 0, 0));

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => _ = new StringSegment(s, -1, 0));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => _ = new StringSegment(s, 0, -1));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => _ = new StringSegment(s, 5, 1));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => _ = new StringSegment(s, 4, 2));
        }

        [TestMethod]
        public void StringSegment_Ctor_Substring()
        {
            var v = "qBarfo";
            var s = new StringSegment(v, 1, 3);

            Assert.AreSame(v, s.String);
            Assert.AreEqual(1, s.StartIndex);
            Assert.AreEqual(3, s.Length);

            Assert.AreEqual("Bar", (string)s);
        }


        //=====================================================\\
        //   _____                           _   _             \\
        //  |  __ \                         | | (_)            \\
        //  | |__) | __ ___  _ __   ___ _ __| |_ _  ___  ___   \\
        //  |  ___/ '__/ _ \| '_ \ / _ \ '__| __| |/ _ \/ __|  \\
        //  | |   | | | (_) | |_) |  __/ |  | |_| |  __/\__ \  \\
        //  |_|   |_|  \___/| .__/ \___|_|   \__|_|\___||___/  \\
        //                  | |                                \\
        //                  |_|                                \\
        //=====================================================\\

        [TestMethod]
        public void StringSegment_Indexer_ArgumentChecking()
        {
            WithVariations("bar", s =>
            {
                Assert.ThrowsException<IndexOutOfRangeException>(() => s[-1]);
                Assert.ThrowsException<IndexOutOfRangeException>(() => s[4]);
            });
        }

        [TestMethod]
        public void StringSegment_Indexer()
        {
            WithVariations("Bar", s =>
            {
                Assert.AreEqual('B', s[0]);
                Assert.AreEqual('a', s[1]);
                Assert.AreEqual('r', s[2]);
            });
        }


        //==================================================\\
        //   ____                       _                   \\
        //  / __ \                     | |                  \\
        // | |  | |_ __   ___ _ __ __ _| |_ ___  _ __ ___   \\
        // | |  | | '_ \ / _ \ '__/ _` | __/ _ \| '__/ __|  \\
        // | |__| | |_) |  __/ | | (_| | || (_) | |  \__ \  \\
        //  \____/| .__/ \___|_|  \__,_|\__\___/|_|  |___/  \\
        //        | |                                       \\
        //        |_|                                       \\
        //==================================================\\

        [TestMethod]
        public void StringSegment_StringToStringSegment()
        {
            var v = "Bar";
            var s = (StringSegment)v;

            Assert.AreSame(v, s.String);
            Assert.AreEqual(0, s.StartIndex);
            Assert.AreEqual(3, s.Length);
        }

        [TestMethod]
        public void StringSegment_StringToStringSegment_Null()
        {
            var v = default(string);
            var s = (StringSegment)v;

            Assert.AreSame(v, s.String);
            Assert.AreEqual(0, s.StartIndex);
            Assert.AreEqual(0, s.Length);
        }

        [TestMethod]
        public void StringSegment_StringSegmentToString1()
        {
            var v = "Bar";
            var s = new StringSegment(v);

            Assert.AreSame(v, (string)s);
        }

        [TestMethod]
        public void StringSegment_StringSegmentToString2()
        {
            var v = "xybarqux";
            var s = new StringSegment(v, 2, 3);

            Assert.AreEqual("bar", (string)s);
        }

        [TestMethod]
        public void StringSegment_Equality1()
        {
            WithVariations("bar", "bar", (l, r) =>
            {
                Assert.IsTrue(l == r);
            });

            WithVariations("bar", "foo", (l, r) =>
            {
                Assert.IsFalse(l == r);
            });
        }

        [TestMethod]
        public void StringSegment_Equality2()
        {
            var s1 = default(StringSegment);
            var s2 = default(StringSegment);
            var s3 = new StringSegment("bar");
            var s4 = new StringSegment("bar");
            var s5 = new StringSegment("foo");
            var s6 = new StringSegment("barfoo");

            Assert.IsTrue(s1 == s2);
            Assert.IsFalse(s1 == s3);
            Assert.IsFalse(s3 == s1);
            Assert.IsTrue(s3 == s4);
            Assert.IsFalse(s3 == s5);
            Assert.IsFalse(s3 == s6);
        }

        [TestMethod]
        public void StringSegment_Inequality1()
        {
            WithVariations("bar", "bar", (l, r) =>
            {
                Assert.IsFalse(l != r);
            });

            WithVariations("bar", "foo", (l, r) =>
            {
                Assert.IsTrue(l != r);
            });
        }

        [TestMethod]
        public void StringSegment_Inequality2()
        {
            var s1 = default(StringSegment);
            var s2 = default(StringSegment);
            var s3 = new StringSegment("bar");
            var s4 = new StringSegment("bar");
            var s5 = new StringSegment("foo");
            var s6 = new StringSegment("barfoo");

            Assert.IsFalse(s1 != s2);
            Assert.IsTrue(s1 != s3);
            Assert.IsTrue(s3 != s1);
            Assert.IsFalse(s3 != s4);
            Assert.IsTrue(s3 != s5);
            Assert.IsTrue(s3 != s6);
        }


        //===========================================\\
        //   __  __      _   _               _       \\
        //  |  \/  |    | | | |             | |      \\
        //  | \  / | ___| |_| |__   ___   __| |___   \\
        //  | |\/| |/ _ \ __| '_ \ / _ \ / _` / __|  \\
        //  | |  | |  __/ |_| | | | (_) | (_| \__ \  \\
        //  |_|  |_|\___|\__|_| |_|\___/ \__,_|___/  \\
        //                                           \\
        //===========================================\\

        [TestMethod]
        public void StringSegment_Compare_StringSegment_StringSegment_ArgumentChecking()
        {
            // NB: No known invalid inputs.

            Assert.IsTrue(true);
        }

        [TestMethod]
        public void StringSegment_Compare_StringSegment_StringSegment()
        {
            var n = default(StringSegment);
            var b = new StringSegment("bar");

            Assert.AreEqual(0, StringSegment.Compare(n, n));
            Assert.AreEqual(0, StringSegment.Compare(b, b));
            Assert.IsTrue(StringSegment.Compare(n, b) < 0);
            Assert.IsTrue(StringSegment.Compare(b, n) > 0);

            AssertWithVariations(
                new[] { "", "bar", "baz", "foo", "qux", "foobar", "BAR" },
                new[] { "", "bar", "baz", "foo", "qux", "foobar", "BAR" },
                (l, r) => string.Compare(l, r),
                (l, r) => StringSegment.Compare(l, r)
            );
        }

        [TestMethod]
        public void StringSegment_Compare_StringSegment_StringSegment_Boolean_ArgumentChecking()
        {
            // NB: No known invalid inputs.

            Assert.IsTrue(true);
        }

        [TestMethod]
        public void StringSegment_Compare_StringSegment_StringSegment_Boolean()
        {
            var n = default(StringSegment);
            var b = new StringSegment("bar");

            foreach (var ignoreCase in new[] { false, true })
            {
                Assert.AreEqual(0, StringSegment.Compare(n, n, ignoreCase));
                Assert.AreEqual(0, StringSegment.Compare(b, b, ignoreCase));
                Assert.IsTrue(StringSegment.Compare(n, b, ignoreCase) < 0);
                Assert.IsTrue(StringSegment.Compare(b, n, ignoreCase) > 0);

                AssertWithVariations(
                    new[] { "", "bar", "baz", "foo", "foobar", "BAR", "FOO" },
                    new[] { "", "bar", "baz", "foo", "foobar", "BAR", "FOO" },
                    (l, r) => string.Compare(l, r, ignoreCase),
                    (l, r) => StringSegment.Compare(l, r, ignoreCase)
                );
            }
        }

        [TestMethod]
        public void StringSegment_Compare_StringSegment_StringSegment_Boolean_CultureInfo_ArgumentChecking()
        {
            WithVariations("bar", bar =>
            {
                Assert.ThrowsException<ArgumentNullException>(() => StringSegment.Compare(bar, bar, true, default));
            });
        }

        [TestMethod]
        public void StringSegment_Compare_StringSegment_StringSegment_Boolean_CultureInfo()
        {
            var n = default(StringSegment);
            var b = new StringSegment("bar");

            foreach (var ignoreCase in new[] { false, true })
            {
                foreach (var culture in new[] { CultureInfo.InvariantCulture, CultureInfo.CurrentCulture })
                {
                    Assert.AreEqual(0, StringSegment.Compare(n, n, ignoreCase, culture));
                    Assert.AreEqual(0, StringSegment.Compare(b, b, ignoreCase, culture));
                    Assert.IsTrue(StringSegment.Compare(n, b, ignoreCase, culture) < 0);
                    Assert.IsTrue(StringSegment.Compare(b, n, ignoreCase, culture) > 0);

                    AssertWithVariations(
                        new[] { "", "bar", "baz", "foo", "foobar", "BaR", "fOo" },
                        new[] { "", "bar", "baz", "foo", "foobar", "BaR", "fOo" },
                        (l, r) => string.Compare(l, r, ignoreCase, culture),
                        (l, r) => StringSegment.Compare(l, r, ignoreCase, culture)
                    );
                }
            }
        }

        [TestMethod]
        public void StringSegment_Compare_StringSegment_StringSegment_StringComparison_ArgumentChecking()
        {
            WithVariations("bar", bar =>
            {
                Assert.ThrowsException<ArgumentException>(() => StringSegment.Compare(bar, bar, (StringComparison)(-1)));
            });
        }

        [TestMethod]
        public void StringSegment_Compare_StringSegment_StringSegment_StringComparison()
        {
            var n = default(StringSegment);
            var b = new StringSegment("bar");

            foreach (var comparison in new[] { StringComparison.CurrentCulture, StringComparison.CurrentCultureIgnoreCase, StringComparison.InvariantCulture, StringComparison.InvariantCultureIgnoreCase, StringComparison.Ordinal, StringComparison.OrdinalIgnoreCase })
            {
                Assert.AreEqual(0, StringSegment.Compare(n, n, comparison));
                Assert.AreEqual(0, StringSegment.Compare(b, b, comparison));
                Assert.IsTrue(StringSegment.Compare(n, b, comparison) < 0);
                Assert.IsTrue(StringSegment.Compare(b, n, comparison) > 0);

                AssertWithVariations(
                    new[] { "", "bar", "baz", "foo", "foobar", "BaR", "fOo" },
                    new[] { "", "bar", "baz", "foo", "foobar", "BaR", "fOo" },
                    (l, r) => string.Compare(l, r, comparison),
                    (l, r) => StringSegment.Compare(l, r, comparison),
                    (e, a) => Math.Sign(e) == Math.Sign(a) // TODO: Do we need to guarantee an exact output, e.g. ("", "bar") -> -98 due to use of '\0'.
                );
            }
        }

        [TestMethod]
        public void StringSegment_Compare_StringSegment_StringSegment_CultureInfo_CompareOptions_ArgumentChecking()
        {
            WithVariations("bar", bar =>
            {
                Assert.ThrowsException<ArgumentNullException>(() => StringSegment.Compare(bar, bar, default, CompareOptions.None));
            });
        }

        [TestMethod]
        public void StringSegment_Compare_StringSegment_StringSegment_CultureInfo_CompareOptions()
        {
            foreach (var culture in new[] { CultureInfo.InvariantCulture, CultureInfo.CurrentCulture })
            {
                foreach (var options in new[] { CompareOptions.None, CompareOptions.IgnoreCase, CompareOptions.Ordinal, CompareOptions.OrdinalIgnoreCase })
                {
                    AssertWithVariations(
                        new[] { "", "bar", "baz", "foo", "foobar", "BaR", "fOo" },
                        new[] { "", "bar", "baz", "foo", "foobar", "BaR", "fOo" },
                        (l, r) => string.Compare(l, r, culture, options),
                        (l, r) => StringSegment.Compare(l, r, culture, options),
                        (e, a) => Math.Sign(e) == Math.Sign(a) // TODO: Do we need to guarantee an exact output, e.g. ("", "bar") -> -98 due to use of '\0'.
                    );
                }
            }
        }

        [TestMethod]
        public void StringSegment_Compare_StringSegment_Int32_StringSegment_Int32_Int32_ArgumentChecking()
        {
            var n = default(StringSegment);

            WithVariations("bar", "xybar", (bar, xybar) =>
            {
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => StringSegment.Compare(bar, -1, xybar, 0, 1));
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => StringSegment.Compare(bar, 4, xybar, -1, 1));
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => StringSegment.Compare(bar, 0, xybar, -1, 1));
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => StringSegment.Compare(bar, 0, xybar, 6, 1));
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => StringSegment.Compare(bar, 0, xybar, 0, -1));
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => StringSegment.Compare(n, 0, xybar, 0, 1));
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => StringSegment.Compare(bar, 0, n, 0, 1));
            });
        }

        [TestMethod]
        public void StringSegment_Compare_StringSegment_Int32_StringSegment_Int32_Int32()
        {
            Parallel.ForEach(new[] { 0, 1, 3 }, startIndexA =>
            {
                Parallel.ForEach(new[] { 0, 1, 3 }, startIndexB =>
                {
                    foreach (var length in new[] { 0, 1, 3, 4, 6, 7 })
                    {
                        AssertWithVariations(
                            new[] { "", "bar", "baz", "foo", "foobar", "BaR" },
                            new[] { "", "bar", "baz", "foo", "foobar", "BaR" },
                            (l, r) => string.Compare(l, startIndexA, r, startIndexB, length),
                            (l, r) => StringSegment.Compare(l, startIndexA, r, startIndexB, length),
                            (e, a) => Math.Sign(e) == Math.Sign(a) // TODO: Do we need to guarantee an exact output, e.g. ("", "bar") -> -98 due to use of '\0'.
                        );
                    }
                });
            });
        }

        [TestMethod]
        public void StringSegment_Compare_StringSegment_Int32_StringSegment_Int32_Int32_Boolean_ArgumentChecking()
        {
            var n = default(StringSegment);

            WithVariations("bar", "xybar", (bar, xybar) =>
            {
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => StringSegment.Compare(bar, -1, xybar, 0, 1, ignoreCase: false));
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => StringSegment.Compare(bar, 4, xybar, -1, 1, ignoreCase: false));
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => StringSegment.Compare(bar, 0, xybar, -1, 1, ignoreCase: false));
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => StringSegment.Compare(bar, 0, xybar, 6, 1, ignoreCase: false));
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => StringSegment.Compare(bar, 0, xybar, 0, -1, ignoreCase: false));
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => StringSegment.Compare(n, 0, xybar, 0, 1, ignoreCase: false));
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => StringSegment.Compare(bar, 0, n, 0, 1, ignoreCase: false));
            });
        }

        [TestMethod]
        public void StringSegment_Compare_StringSegment_Int32_StringSegment_Int32_Int32_Boolean()
        {
            Parallel.ForEach(new[] { 0, 1, 3 }, startIndexA =>
            {
                Parallel.ForEach(new[] { 0, 1, 3 }, startIndexB =>
                {
                    foreach (var length in new[] { 0, 1, 3, 4, 6, 7 })
                    {
                        foreach (var ignoreCase in new[] { false, true })
                        {
                            AssertWithVariations(
                                new[] { "", "bar", "baz", "foo", "foobar", "BaR" },
                                new[] { "", "bar", "baz", "foo", "foobar", "BaR" },
                                (l, r) => string.Compare(l, startIndexA, r, startIndexB, length, ignoreCase),
                                (l, r) => StringSegment.Compare(l, startIndexA, r, startIndexB, length, ignoreCase),
                                (e, a) => Math.Sign(e) == Math.Sign(a) // TODO: Do we need to guarantee an exact output, e.g. ("", "bar") -> -98 due to use of '\0'.
                            );
                        }
                    }
                });
            });
        }

        [TestMethod]
        public void StringSegment_Compare_StringSegment_Int32_StringSegment_Int32_Int32_StringComparison_ArgumentChecking()
        {
            var n = default(StringSegment);

            WithVariations("bar", "xybar", (bar, xybar) =>
            {
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => StringSegment.Compare(bar, -1, xybar, 0, 1, StringComparison.Ordinal));
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => StringSegment.Compare(bar, 4, xybar, -1, 1, StringComparison.Ordinal));
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => StringSegment.Compare(bar, 0, xybar, -1, 1, StringComparison.Ordinal));
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => StringSegment.Compare(bar, 0, xybar, 6, 1, StringComparison.Ordinal));
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => StringSegment.Compare(bar, 0, xybar, 0, -1, StringComparison.Ordinal));
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => StringSegment.Compare(n, 0, xybar, 0, 1, StringComparison.Ordinal));
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => StringSegment.Compare(bar, 0, n, 0, 1, StringComparison.Ordinal));
            });
        }

        [TestMethod]
        public void StringSegment_Compare_StringSegment_Int32_StringSegment_Int32_Int32_StringComparison()
        {
            Parallel.ForEach(new[] { 0, 1, 3 }, startIndexA =>
            {
                Parallel.ForEach(new[] { 0, 1, 3 }, startIndexB =>
                {
                    foreach (var length in new[] { 0, 1, 3, 4, 6, 7 })
                    {
                        foreach (var comparison in new[] { StringComparison.InvariantCulture, StringComparison.Ordinal, StringComparison.OrdinalIgnoreCase })
                        {
                            AssertWithVariations(
                                new[] { "", "bar", "baz", "foo", "foobar", "BaR" },
                                new[] { "", "bar", "baz", "foo", "foobar", "BaR" },
                                (l, r) => string.Compare(l, startIndexA, r, startIndexB, length, comparison),
                                (l, r) => StringSegment.Compare(l, startIndexA, r, startIndexB, length, comparison),
                                (e, a) => Math.Sign(e) == Math.Sign(a) // TODO: Do we need to guarantee an exact output, e.g. ("", "bar") -> -98 due to use of '\0'.
                            );
                        }
                    }
                });
            });
        }

        [TestMethod]
        public void StringSegment_Compare_StringSegment_Int32_StringSegment_Int32_Int32_Boolean_CultureInfo_ArgumentChecking()
        {
            var n = default(StringSegment);

            WithVariations("bar", "xybar", (bar, xybar) =>
            {
                Assert.ThrowsException<ArgumentNullException>(() => StringSegment.Compare(bar, 0, xybar, 0, 1, ignoreCase: false, culture: null));

                Assert.ThrowsException<ArgumentOutOfRangeException>(() => StringSegment.Compare(bar, -1, xybar, 0, 1, ignoreCase: false, CultureInfo.InvariantCulture));
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => StringSegment.Compare(bar, 4, xybar, -1, 1, ignoreCase: false, CultureInfo.InvariantCulture));
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => StringSegment.Compare(bar, 0, xybar, -1, 1, ignoreCase: false, CultureInfo.InvariantCulture));
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => StringSegment.Compare(bar, 0, xybar, 6, 1, ignoreCase: false, CultureInfo.InvariantCulture));
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => StringSegment.Compare(bar, 0, xybar, 0, -1, ignoreCase: false, CultureInfo.InvariantCulture));
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => StringSegment.Compare(n, 0, xybar, 0, 1, ignoreCase: false, CultureInfo.InvariantCulture));
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => StringSegment.Compare(bar, 0, n, 0, 1, ignoreCase: false, CultureInfo.InvariantCulture));
            });
        }

        [TestMethod]
        public void StringSegment_Compare_StringSegment_Int32_StringSegment_Int32_Int32_Boolean_CultureInfo()
        {
            Parallel.ForEach(new[] { 0, 1, 3 }, startIndexA =>
            {
                Parallel.ForEach(new[] { 0, 1, 3 }, startIndexB =>
                {
                    foreach (var length in new[] { 0, 1, 3, 4, 6, 7 })
                    {
                        foreach (var ignoreCase in new[] { false, true })
                        {
                            AssertWithVariations(
                                new[] { "", "bar", "baz", "foo", "foobar", "BaR" },
                                new[] { "", "bar", "baz", "foo", "foobar", "BaR" },
                                (l, r) => string.Compare(l, startIndexA, r, startIndexB, length, ignoreCase, CultureInfo.InvariantCulture),
                                (l, r) => StringSegment.Compare(l, startIndexA, r, startIndexB, length, ignoreCase, CultureInfo.InvariantCulture),
                                (e, a) => Math.Sign(e) == Math.Sign(a) // TODO: Do we need to guarantee an exact output, e.g. ("", "bar") -> -98 due to use of '\0'.
                            );
                        }
                    }
                });
            });
        }

        [TestMethod]
        public void StringSegment_Compare_StringSegment_Int32_StringSegment_Int32_Int32_CultureInfo_CompareOptions_ArgumentChecking()
        {
            var n = default(StringSegment);

            WithVariations("bar", "xybar", (bar, xybar) =>
            {
                Assert.ThrowsException<ArgumentNullException>(() => StringSegment.Compare(bar, 0, xybar, 0, 1, culture: null, CompareOptions.None));

                Assert.ThrowsException<ArgumentOutOfRangeException>(() => StringSegment.Compare(bar, -1, xybar, 0, 1, CultureInfo.InvariantCulture, CompareOptions.None));
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => StringSegment.Compare(bar, 4, xybar, -1, 1, CultureInfo.InvariantCulture, CompareOptions.None));
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => StringSegment.Compare(bar, 0, xybar, -1, 1, CultureInfo.InvariantCulture, CompareOptions.None));
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => StringSegment.Compare(bar, 0, xybar, 6, 1, CultureInfo.InvariantCulture, CompareOptions.None));
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => StringSegment.Compare(bar, 0, xybar, 0, -1, CultureInfo.InvariantCulture, CompareOptions.None));
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => StringSegment.Compare(n, 0, xybar, 0, 1, CultureInfo.InvariantCulture, CompareOptions.None));
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => StringSegment.Compare(bar, 0, n, 0, 1, CultureInfo.InvariantCulture, CompareOptions.None));
            });
        }

        [TestMethod]
        public void StringSegment_Compare_StringSegment_Int32_StringSegment_Int32_Int32_CultureInfo_CompareOptions()
        {
            Parallel.ForEach(new[] { 0, 1, 3 }, startIndexA =>
            {
                Parallel.ForEach(new[] { 0, 1, 3 }, startIndexB =>
                {
                    foreach (var length in new[] { 0, 1, 3, 4, 6, 7 })
                    {
                        foreach (var options in new[] { CompareOptions.None, CompareOptions.IgnoreCase, CompareOptions.Ordinal })
                        {
                            AssertWithVariations(
                                new[] { "", "bar", "baz", "foo", "foobar", "BaR" },
                                new[] { "", "bar", "baz", "foo", "foobar", "BaR" },
                                (l, r) => string.Compare(l, startIndexA, r, startIndexB, length, CultureInfo.InvariantCulture, options),
                                (l, r) => StringSegment.Compare(l, startIndexA, r, startIndexB, length, CultureInfo.InvariantCulture, options),
                                (e, a) => Math.Sign(e) == Math.Sign(a) // TODO: Do we need to guarantee an exact output, e.g. ("", "bar") -> -98 due to use of '\0'.
                            );
                        }
                    }
                });
            });
        }

        [TestMethod]
        public void StringSegment_CompareOrdinal_StringSegment_StringSegment_ArgumentChecking()
        {
            // NB: No known invalid inputs.

            Assert.IsTrue(true);
        }

        [TestMethod]
        public void StringSegment_CompareOrdinal_StringSegment_StringSegment()
        {
            // TODO: Consider adding tests for default(StringSegment) inputs.

            AssertWithVariations(
                new[] { "", "bar", "baz", "foo", "qux", "foobar", "BAR" },
                new[] { "", "bar", "baz", "foo", "qux", "foobar", "BAR" },
                (l, r) => string.CompareOrdinal(l, r),
                (l, r) => StringSegment.CompareOrdinal(l, r),
                (e, a) => Math.Sign(e) == Math.Sign(a) // TODO: Do we need to guarantee an exact output, e.g. ("", "bar") -> -98 due to use of '\0'.
            );
        }

        [TestMethod]
        public void StringSegment_CompareOrdinal_StringSegment_Int32_StringSegment_Int32_Int32_ArgumentChecking()
        {
            var n = default(StringSegment);

            WithVariations("bar", "xybar", (bar, xybar) =>
            {
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => StringSegment.CompareOrdinal(bar, -1, xybar, 0, 1));
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => StringSegment.CompareOrdinal(bar, 4, xybar, -1, 1));
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => StringSegment.CompareOrdinal(bar, 0, xybar, -1, 1));
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => StringSegment.CompareOrdinal(bar, 0, xybar, 6, 1));
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => StringSegment.CompareOrdinal(bar, 0, xybar, 0, -1));
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => StringSegment.CompareOrdinal(n, 0, xybar, 0, 1));
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => StringSegment.CompareOrdinal(bar, 0, n, 0, 1));
            });
        }

        [TestMethod]
        public void StringSegment_CompareOrdinal_StringSegment_Int32_StringSegment_Int32_Int32()
        {
            Parallel.ForEach(new[] { 0, 1, 3 }, startIndexA =>
            {
                Parallel.ForEach(new[] { 0, 1, 3 }, startIndexB =>
                {
                    foreach (var length in new[] { 0, 1, 3, 4, 6, 7 })
                    {
                        AssertWithVariations(
                            new[] { "", "bar", "baz", "foo", "foobar", "BaR" },
                            new[] { "", "bar", "baz", "foo", "foobar", "BaR" },
                            (l, r) => string.CompareOrdinal(l, startIndexA, r, startIndexB, length),
                            (l, r) => StringSegment.CompareOrdinal(l, startIndexA, r, startIndexB, length),
                            (e, a) => Math.Sign(e) == Math.Sign(a) // TODO: Do we need to guarantee an exact output, e.g. ("", "bar") -> -98 due to use of '\0'.
                        );
                    }
                });
            });
        }

        [TestMethod]
        public void StringSegment_CompareTo_StringSegment_ArgumentChecking()
        {
            // NB: No known invalid inputs.

            Assert.IsTrue(true);
        }

        [TestMethod]
        public void StringSegment_CompareTo_StringSegment()
        {
            AssertWithVariations(
                new[] { "", "b", "ba", "bar", "foo", "foobar", "qux" },
                new[] { "", "b", "ba", "bar", "foo", "foobar", "qux" },
                (l, r) => l.CompareTo(r),
                (l, r) => l.CompareTo(r)
            );

            var n = default(StringSegment);
            var s = new StringSegment("bar");

            Assert.AreEqual(0, n.CompareTo(n));
            Assert.AreEqual(0, s.CompareTo(s));
            Assert.IsTrue(s.CompareTo(n) > 0);
            Assert.IsTrue(n.CompareTo(s) < 0);
        }

        [TestMethod]
        public void StringSegment_CompareTo_Object_ArgumentChecking()
        {
            WithVariations("bar", bar =>
            {
                Assert.ThrowsException<ArgumentException>(() => ((IComparable)bar).CompareTo(1));
                Assert.ThrowsException<ArgumentException>(() => ((IComparable)bar).CompareTo("bar"));
            });
        }

        [TestMethod]
        public void StringSegment_CompareTo_Object()
        {
            AssertWithVariations(
                new[] { "", "b", "ba", "bar", "foo", "foobar", "qux" },
                new[] { "", "b", "ba", "bar", "foo", "foobar", "qux" },
                (l, r) => ((IComparable)l).CompareTo(r),
                (l, r) => ((IComparable)l).CompareTo(r)
            );

            var n = (IComparable)default(StringSegment);
            var s = (IComparable)new StringSegment("bar");

            Assert.AreEqual(0, n.CompareTo(n));
            Assert.AreEqual(0, n.CompareTo(obj: null));
            Assert.AreEqual(0, s.CompareTo(s));
            Assert.IsTrue(s.CompareTo(n) > 0);
            Assert.IsTrue(s.CompareTo(obj: null) > 0);
            Assert.IsTrue(n.CompareTo(s) < 0);
        }

        [TestMethod]
        public void StringSegment_Concat_StringSegment_StringSegment_ArgumentChecking()
        {
            // NB: No known invalid inputs.

            Assert.IsTrue(true);
        }

        [TestMethod]
        public void StringSegment_Concat_StringSegment_StringSegment()
        {
            AssertWithVariations(
                new[] { null, "", "bar", "foo", "bazqux" },
                new[] { null, "", "bar", "foo", "bazqux" },
                (s1, s2) => string.Concat(s1, s2),
                (s1, s2) => StringSegment.Concat(s1, s2)
            );
        }

        [TestMethod]
        public void StringSegment_Concat_StringSegment_StringSegment_Optimized()
        {
            var s = "foobarquxbaz";

            var s0 = new StringSegment(s, 0, 3);
            var s1 = new StringSegment(s, 3, 4);
            var s2 = new StringSegment(s, 7, 2);
            var s3 = new StringSegment(s, 9, 3);

            var r1 = StringSegment.Concat(s0, s1);
            var r2 = StringSegment.Concat(s1, s2);
            var r3 = StringSegment.Concat(s2, s3);

            Assert.AreEqual(s0.ToString() + s1.ToString(), r1.ToString());
            Assert.AreEqual(s1.ToString() + s2.ToString(), r2.ToString());
            Assert.AreEqual(s2.ToString() + s3.ToString(), r3.ToString());

            Assert.AreSame(s, r1.String);
            Assert.AreSame(s, r2.String);
            Assert.AreSame(s, r3.String);
        }

        [TestMethod]
        public void StringSegment_Concat_StringSegment_StringSegment_StringSegment_ArgumentChecking()
        {
            // NB: No known invalid inputs.

            Assert.IsTrue(true);
        }

        [TestMethod]
        public void StringSegment_Concat_StringSegment_StringSegment_StringSegment()
        {
            AssertWithVariations(
                new[] { null, "", "bar" },
                new[] { null, "", "qux" },
                new[] { null, "", "fubaz" },
                (s1, s2, s3) => string.Concat(s1, s2, s3),
                (s1, s2, s3) => StringSegment.Concat(s1, s2, s3)
            );
        }

        [TestMethod]
        public void StringSegment_Concat_StringSegment_StringSegment_StringSegment_Optimized()
        {
            var s = "foobarquxbaz";

            var s0 = new StringSegment(s, 0, 3);
            var s1 = new StringSegment(s, 3, 4);
            var s2 = new StringSegment(s, 7, 2);
            var s3 = new StringSegment(s, 9, 3);

            var r1 = StringSegment.Concat(s0, s1, s2);
            var r2 = StringSegment.Concat(s1, s2, s3);

            Assert.AreEqual(s0.ToString() + s1.ToString() + s2.ToString(), r1.ToString());
            Assert.AreEqual(s1.ToString() + s2.ToString() + s3.ToString(), r2.ToString());

            Assert.AreSame(s, r1.String);
            Assert.AreSame(s, r2.String);
        }

        [TestMethod]
        public void StringSegment_Concat_StringSegment_StringSegment_StringSegment_StringSegment_ArgumentChecking()
        {
            // NB: No known invalid inputs.

            Assert.IsTrue(true);
        }

        [TestMethod]
        public void StringSegment_Concat_StringSegment_StringSegment_StringSegment_StringSegment()
        {
            AssertWithVariations(
                new[] { null, "", "bar" },
                new[] { null, "", "foo" },
                new[] { null, "", "qux" },
                new[] { null, "", "fubaz" },
                (s1, s2, s3, s4) => string.Concat(s1, s2, s3, s4),
                (s1, s2, s3, s4) => StringSegment.Concat(s1, s2, s3, s4)
            );
        }

        [TestMethod]
        public void StringSegment_Concat_StringSegment_StringSegment_StringSegment_StringSegment_Optimized()
        {
            var s = "foobarquxbaz";

            var s0 = new StringSegment(s, 0, 3);
            var s1 = new StringSegment(s, 3, 4);
            var s2 = new StringSegment(s, 7, 2);
            var s3 = new StringSegment(s, 9, 3);

            var r1 = StringSegment.Concat(s0, s1, s2, s3);

            Assert.AreEqual(s0.ToString() + s1.ToString() + s2.ToString() + s3.ToString(), r1.ToString());

            Assert.AreSame(s, r1.String);
        }

        [TestMethod]
        public void StringSegment_Concat_StringSegmentArray_ArgumentChecking()
        {
            Assert.ThrowsException<ArgumentNullException>(() => StringSegment.Concat(default(StringSegment[])));
        }

        [TestMethod]
        public void StringSegment_Concat_StringSegmentArray0()
        {
            Assert.AreEqual("", (string)StringSegment.Concat(Array.Empty<StringSegment>()));
        }

        [TestMethod]
        public void StringSegment_Concat_StringSegmentArray1()
        {
            AssertWithVariations(
                new[] { null, "", "bar", "foo", "bazqux" },
                s => string.Concat(new[] { s }),
                s => StringSegment.Concat(new[] { s })
            );
        }

        [TestMethod]
        public void StringSegment_Concat_StringSegmentArray2()
        {
            AssertWithVariations(
                new[] { null, "", "bar", "foo", "bazqux" },
                new[] { null, "", "bar", "foo", "bazqux" },
                (s1, s2) => string.Concat(new[] { s1, s2 }),
                (s1, s2) => StringSegment.Concat(new[] { s1, s2 })
            );
        }

        [TestMethod]
        public void StringSegment_Concat_StringSegmentArrayN()
        {
            var xs = new[] { "bar", null, "foo", "baz", "", "qux", "foobar" };

            Assert.AreEqual(string.Concat(xs), (string)StringSegment.Concat(xs.Select(x => (StringSegment)x).ToArray()));
        }

        [TestMethod]
        public void StringSegment_Concat_StringSegmentArray_Optimized()
        {
            var s = "foobarquxbaz";

            var s0 = new StringSegment(s, 0, 3);
            var s1 = new StringSegment(s, 3, 4);
            var s2 = new StringSegment(s, 7, 2);
            var s3 = new StringSegment(s, 9, 3);

            var r1 = StringSegment.Concat(new StringSegment[] { s0, s1, s2, s3 });

            Assert.AreEqual(s0.ToString() + s1.ToString() + s2.ToString() + s3.ToString(), r1.ToString());

            Assert.AreSame(s, r1.String);
        }

        [TestMethod]
        public void StringSegment_Concat_IEnumerableOfT_ArgumentChecking()
        {
            Assert.ThrowsException<ArgumentNullException>(() => StringSegment.Concat<object>(default));
            Assert.ThrowsException<ArgumentNullException>(() => StringSegment.Concat<StringSegment>(default));
        }

        [TestMethod]
        public void StringSegment_Concat_IEnumerableOfT()
        {
            var xs = new object[] { 1, true, null, "bar" }.AsEnumerable();
            var ys = new int[] { 1, 2, 3 }.AsEnumerable();
            var zs = new StringSegment[] { "bar", "foo", "qux" }.AsEnumerable();

            for (var i = 0; i <= 4; i++)
            {
                Assert.AreEqual(string.Concat<object>(xs.Take(i)), (string)StringSegment.Concat<object>(xs.Take(i)));
                Assert.AreEqual(string.Concat<int>(ys.Take(i)), (string)StringSegment.Concat<int>(ys.Take(i)));
                Assert.AreEqual(string.Concat<StringSegment>(zs.Take(i)), (string)StringSegment.Concat<StringSegment>(zs.Take(i)));
            }
        }

        [TestMethod]
        public void StringSegment_Concat_Object_ArgumentChecking()
        {
            // NB: No known invalid inputs.

            Assert.IsTrue(true);
        }

        [TestMethod]
        public void StringSegment_Concat_Object()
        {
            var os = new object[] { null, 1, "foo", new StringSegment("bar") };

            foreach (var o in os)
            {
                Assert.AreEqual(string.Concat(o), (string)StringSegment.Concat(o));
            }
        }

        [TestMethod]
        public void StringSegment_Concat_Object_Object_ArgumentChecking()
        {
            // NB: No known invalid inputs.

            Assert.IsTrue(true);
        }

        [TestMethod]
        public void StringSegment_Concat_Object_Object()
        {
            var os = new object[] { null, 1, "foo", new StringSegment("bar") };

            foreach (var o1 in os)
            {
                foreach (var o2 in os)
                {
                    Assert.AreEqual(string.Concat(o1, o2), (string)StringSegment.Concat(o1, o2));
                }
            }
        }

        [TestMethod]
        public void StringSegment_Concat_Object_Object_Object_ArgumentChecking()
        {
            // NB: No known invalid inputs.

            Assert.IsTrue(true);
        }

        [TestMethod]
        public void StringSegment_Concat_Object_Object_Object()
        {
            var os = new object[] { null, 1, "foo", new StringSegment("bar") };

            foreach (var o1 in os)
            {
                foreach (var o2 in os)
                {
                    foreach (var o3 in os)
                    {
                        Assert.AreEqual(string.Concat(o1, o2, o3), (string)StringSegment.Concat(o1, o2, o3));
                    }
                }
            }
        }

        [TestMethod]
        public void StringSegment_Concat_Object_Object_Object_Object_ArgumentChecking()
        {
            // NB: No known invalid inputs.

            Assert.IsTrue(true);
        }

        [TestMethod]
        public void StringSegment_Concat_Object_Object_Object_Object()
        {
            var os = new object[] { null, 1, "foo", new StringSegment("bar") };

            foreach (var o1 in os)
            {
                foreach (var o2 in os)
                {
                    foreach (var o3 in os)
                    {
                        foreach (var o4 in os)
                        {
                            Assert.AreEqual(string.Concat(o1, o2, o3, o4), (string)StringSegment.Concat(o1, o2, o3, o4));
                        }
                    }
                }
            }
        }

        [TestMethod]
        public void StringSegment_Concat_Object_Object_Object_Object_ArgIterator_ArgumentChecking()
        {
            // NB: No known invalid inputs.

            Assert.IsTrue(true);
        }

#if NETFRAMEWORK
        [TestMethod]
        public void StringSegment_Concat_Object_Object_Object_Object_ArgIterator()
        {
            // NB: Throws NotSupportedException on Mono.
            if (Type.GetType("Mono.Runtime") == null)
            {
                Assert.AreEqual(
                    string.Concat(null, 1, "foo", new StringSegment("bar"), __arglist()),
                    (string)StringSegment.Concat(null, 1, "foo", new StringSegment("bar"), __arglist())
                );

                Assert.AreEqual(
                    string.Concat(null, 1, "foo", new StringSegment("bar"), __arglist(42)),
                    (string)StringSegment.Concat(null, 1, "foo", new StringSegment("bar"), __arglist(42))
                );

                Assert.AreEqual(
                    string.Concat(null, 1, "foo", new StringSegment("bar"), __arglist(42, "qux")),
                    (string)StringSegment.Concat(null, 1, "foo", new StringSegment("bar"), __arglist(42, "qux"))
                );
            }
        }
#endif

        [TestMethod]
        public void StringSegment_Concat_ObjectArray_ArgumentChecking()
        {
            // NB: No known invalid inputs.

            Assert.IsTrue(true);
        }

        [TestMethod]
        public void StringSegment_Concat_ObjectArray()
        {
            Assert.AreEqual(string.Concat(new object[] { 1, true, "bar" }), StringSegment.Concat(new object[] { 1, true, "bar" }));
        }

        [TestMethod]
        public void StringSegment_Concat_IEnumerableOfStringSegment_ArgumentChecking()
        {
            Assert.ThrowsException<ArgumentNullException>(() => StringSegment.Concat(default(IEnumerable<StringSegment>)));
        }

        [TestMethod]
        public void StringSegment_Concat_IEnumerableOfStringSegment0()
        {
            Assert.AreEqual("", StringSegment.Concat(Enumerable.Empty<StringSegment>()).ToString());
        }

        [TestMethod]
        public void StringSegment_Concat_IEnumerableOfStringSegment1()
        {
            AssertWithVariations(
                new[] { null, "", "bar", "foo", "bazqux" },
                s => string.Concat(new[] { s }.AsEnumerable()),
                s => StringSegment.Concat(new List<StringSegment> { s }.AsEnumerable())
            );
        }

        [TestMethod]
        public void StringSegment_Concat_IEnumerableOfStringSegment2()
        {
            AssertWithVariations(
                new[] { null, "", "bar", "foo", "bazqux" },
                new[] { null, "", "bar", "foo", "bazqux" },
                (s1, s2) => string.Concat(new[] { s1, s2 }.AsEnumerable()),
                (s1, s2) => StringSegment.Concat(new List<StringSegment> { s1, s2 }.AsEnumerable())
            );
        }

        [TestMethod]
        public void StringSegment_Concat_IEnumerableOfStringSegmentN()
        {
            var xs = new[] { "bar", null, "foo", "baz", "", "qux", "foobar" };

            Assert.AreEqual(string.Concat(xs.AsEnumerable()), StringSegment.Concat(xs.Select(x => (StringSegment)x)));
        }

        [TestMethod]
        public void StringSegment_Contains_StringSegment_ArgumentChecking()
        {
            Assert.ThrowsException<NullReferenceException>(() => default(StringSegment).Contains("bar"));

            WithVariations("bar", bar =>
            {
                Assert.ThrowsException<ArgumentNullException>(() => bar.Contains(value: null));
            });
        }

        [TestMethod]
        public void StringSegment_Contains_StringSegment()
        {
            AssertWithVariations(
                new[] { "", "bar", "foobar", "foobarqux", "barfoo" },
                new[] { "", "bar", "foo", "qux", "baz", "a", "B", "ar", "ba" },
                (l, r) => l.Contains(r),
                (l, r) => l.Contains(r)
            );
        }

#if !NET6_0
        [TestMethod]
        public void StringSegment_Copy_StringSegment_ArgumentChecking()
        {
            Assert.ThrowsException<ArgumentNullException>(() => StringSegment.Copy(default));
        }

        [TestMethod]
        public void StringSegment_Copy_StringSegment()
        {
            var v1 = "Bar";
            var s1 = new StringSegment(v1);
            var s2 = StringSegment.Copy(s1);
            Assert.AreNotSame(s1.String, s2.String);

            var v3 = "qqBarqq";
            var s3 = new StringSegment(v3, 2, 3);
            var s4 = StringSegment.Copy(s3);
            Assert.AreNotSame(s3.String, s4.String);
        }
#endif

        [TestMethod]
        public void StringSegment_CopyTo_Int32_CharArray_Int32_Int32_ArgumentChecking()
        {
            WithVariations("bar", s =>
            {
                Assert.ThrowsException<NullReferenceException>(() => default(StringSegment).CopyTo(0, new char[1], 0, 1));
                Assert.ThrowsException<ArgumentNullException>(() => s.CopyTo(0, default, 0, 1));

                Assert.ThrowsException<ArgumentOutOfRangeException>(() => s.CopyTo(-1, new char[1], 0, 1));
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => s.CopyTo(0, new char[1], -1, 1));
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => s.CopyTo(0, new char[1], 0, -1));

                Assert.ThrowsException<ArgumentOutOfRangeException>(() => s.CopyTo(0, new char[1], 0, 2));
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => s.CopyTo(3, new char[1], 0, 1));
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => s.CopyTo(2, new char[2], 0, 2));
            });
        }

        [TestMethod]
        public void StringSegment_CopyTo_Int32_CharArray_Int32_Int32()
        {
            WithVariations("bar", s =>
            {
                var NUL = '\0';

                var c = new char[8];

                s.CopyTo(0, c, 0, 0);
                Assert.IsTrue(c.SequenceEqual(new[] { NUL, NUL, NUL, NUL, NUL, NUL, NUL, NUL }));
                Array.Clear(c, 0, c.Length);

                s.CopyTo(0, c, 0, 1);
                Assert.IsTrue(c.SequenceEqual(new[] { 'b', NUL, NUL, NUL, NUL, NUL, NUL, NUL }));
                Array.Clear(c, 0, c.Length);

                s.CopyTo(0, c, 0, 2);
                Assert.IsTrue(c.SequenceEqual(new[] { 'b', 'a', NUL, NUL, NUL, NUL, NUL, NUL }));
                Array.Clear(c, 0, c.Length);

                s.CopyTo(0, c, 0, 3);
                Assert.IsTrue(c.SequenceEqual(new[] { 'b', 'a', 'r', NUL, NUL, NUL, NUL, NUL }));
                Array.Clear(c, 0, c.Length);

                s.CopyTo(0, c, 1, 3);
                Assert.IsTrue(c.SequenceEqual(new[] { NUL, 'b', 'a', 'r', NUL, NUL, NUL, NUL }));
                Array.Clear(c, 0, c.Length);

                s.CopyTo(0, c, 2, 3);
                Assert.IsTrue(c.SequenceEqual(new[] { NUL, NUL, 'b', 'a', 'r', NUL, NUL, NUL }));
                Array.Clear(c, 0, c.Length);

                s.CopyTo(0, c, 3, 2);
                Assert.IsTrue(c.SequenceEqual(new[] { NUL, NUL, NUL, 'b', 'a', NUL, NUL, NUL }));
                Array.Clear(c, 0, c.Length);

                s.CopyTo(0, c, 4, 1);
                Assert.IsTrue(c.SequenceEqual(new[] { NUL, NUL, NUL, NUL, 'b', NUL, NUL, NUL }));
                Array.Clear(c, 0, c.Length);

                s.CopyTo(1, c, 5, 1);
                Assert.IsTrue(c.SequenceEqual(new[] { NUL, NUL, NUL, NUL, NUL, 'a', NUL, NUL }));
                Array.Clear(c, 0, c.Length);

                s.CopyTo(1, c, 6, 2);
                Assert.IsTrue(c.SequenceEqual(new[] { NUL, NUL, NUL, NUL, NUL, NUL, 'a', 'r' }));
                Array.Clear(c, 0, c.Length);

                s.CopyTo(2, c, 7, 1);
                Assert.IsTrue(c.SequenceEqual(new[] { NUL, NUL, NUL, NUL, NUL, NUL, NUL, 'r' }));
                Array.Clear(c, 0, c.Length);
            });
        }

        [TestMethod]
        public void StringSegment_EndsWith_StringSegment_ArgumentChecking()
        {
            Assert.ThrowsException<NullReferenceException>(() => default(StringSegment).EndsWith("bar"));

            WithVariations("bar", bar =>
            {
                Assert.ThrowsException<ArgumentNullException>(() => bar.EndsWith(value: null));
            });
        }

        [TestMethod]
        public void StringSegment_EndsWith_StringSegment()
        {
            AssertWithVariations(
                new[] { "", "bar", "foobar", "foobarqux", "barfoo" },
                new[] { "", "bar", "foo", "qux", "baz", "a", "B", "ar", "ba" },
                (l, r) => l.EndsWith(r),
                (l, r) => l.EndsWith(r)
            );
        }

        [TestMethod]
        public void StringSegment_EndsWith_StringSegment_StringComparison_ArgumentChecking()
        {
            Assert.ThrowsException<NullReferenceException>(() => default(StringSegment).EndsWith("bar", StringComparison.CurrentCulture));

            WithVariations("bar", bar =>
            {
                Assert.ThrowsException<ArgumentNullException>(() => bar.EndsWith(value: null, StringComparison.CurrentCulture));
            });
        }

        [TestMethod]
        public void StringSegment_EndsWith_StringSegment_StringComparison()
        {
            AssertWithVariations(
                new[] { "", "bar", "foobar", "foobarqux", "barfoo" },
                new[] { "", "bar", "BAR", "qux", "UX", "B", "baz", "O", "BaR" },
                (l, r) => l.EndsWith(r, StringComparison.OrdinalIgnoreCase),
                (l, r) => l.EndsWith(r, StringComparison.OrdinalIgnoreCase)
            );
        }

        [TestMethod]
        public void StringSegment_EndsWith_StringSegment_Boolean_CultureInfo_ArgumentChecking()
        {
            Assert.ThrowsException<NullReferenceException>(() => default(StringSegment).EndsWith("bar", ignoreCase: true, CultureInfo.InvariantCulture));

            WithVariations("bar", bar =>
            {
                Assert.ThrowsException<ArgumentNullException>(() => bar.EndsWith(value: null, ignoreCase: true, CultureInfo.InvariantCulture));
            });
        }

        [TestMethod]
        public void StringSegment_EndsWith_StringSegment_Boolean_CultureInfo()
        {
            foreach (var culture in new[] { null, CultureInfo.CurrentCulture, CultureInfo.InvariantCulture })
            {
                foreach (var ignoreCase in new[] { false, true })
                {
                    AssertWithVariations(
                        new[] { "", "bar", "foobar", "foobarqux", "barfoo" },
                        new[] { "", "bar", "BAR", "qux", "UX", "B", "baz", "O", "BaR" },
                        (l, r) => l.EndsWith(r, ignoreCase, culture),
                        (l, r) => l.EndsWith(r, ignoreCase, culture)
                    );
                }
            }
        }

        [TestMethod]
        public void StringSegment_Equals_StringSegment_StringSegment_ArgumentChecking()
        {
            // NB: No known invalid inputs.

            Assert.IsTrue(true);
        }

        [TestMethod]
        public void StringSegment_Equals_StringSegment_StringSegment()
        {
            AssertWithVariations(
                new[] { null, "", "bar", "foo", "foobar" },
                new[] { null, "", "bar", "foo", "foobar" },
                (l, r) => string.Equals(l, r),
                (l, r) => StringSegment.Equals(l, r)
            );
        }

        [TestMethod]
        public void StringSegment_Equals_StringSegment_StringSegment_StringComparison_ArgumentChecking()
        {
            // NB: No known invalid inputs.

            Assert.IsTrue(true);
        }

        [TestMethod]
        public void StringSegment_Equals_StringSegment_StringSegment_StringComparison()
        {
            foreach (var c in new[] { StringComparison.Ordinal, StringComparison.OrdinalIgnoreCase, StringComparison.InvariantCulture })
            {
                AssertWithVariations(
                    new[] { null, "", "bar", "foo", "foobar", "bAr", "fOObAR" },
                    new[] { null, "", "bar", "foo", "foobar", "bAr", "fOObAR" },
                    (l, r) => string.Equals(l, r, c),
                    (l, r) => StringSegment.Equals(l, r, c)
                );
            }
        }

        [TestMethod]
        public void StringSegment_Equals_Object_ArgumentChecking()
        {
            // NB: No known invalid inputs.

            Assert.IsTrue(true);
        }

        [TestMethod]
        public void StringSegment_Equals_Object()
        {
            var s1 = default(StringSegment);
            var s2 = default(StringSegment);
            var s3 = new StringSegment("bar");
            var s4 = new StringSegment("bar");
            var s5 = new StringSegment("foo");
            var s6 = new StringSegment("barfoo");

            Assert.IsTrue(s1.Equals((object)s2));
            Assert.IsFalse(s1.Equals((object)s3));
            Assert.IsFalse(s3.Equals((object)s1));
            Assert.IsTrue(s3.Equals((object)s4));
            Assert.IsFalse(s3.Equals((object)s5));
            Assert.IsFalse(s3.Equals((object)s6));

            var o1 = "qux";
            var o2 = (object)42;

            Assert.IsFalse(s3.Equals((object)o1));
            Assert.IsFalse(s3.Equals(o2));
        }

        [TestMethod]
        public void StringSegment_Equals_StringSegment_ArgumentChecking()
        {
            // NB: No known invalid inputs.

            Assert.IsTrue(true);
        }

        [TestMethod]
        public void StringSegment_Equals_StringSegment1()
        {
            WithVariations("bar", "bar", (l, r) =>
            {
                Assert.IsTrue(l.Equals(r));
            });

            WithVariations("bar", "foo", (l, r) =>
            {
                Assert.IsFalse(l.Equals(r));
            });
        }

        [TestMethod]
        public void StringSegment_Equals_StringSegment2()
        {
            var s1 = default(StringSegment);
            var s2 = default(StringSegment);
            var s3 = new StringSegment("bar");
            var s4 = new StringSegment("bar");
            var s5 = new StringSegment("foo");
            var s6 = new StringSegment("barfoo");

            Assert.IsTrue(s1.Equals(s2));
            Assert.IsFalse(s1.Equals(s3));
            Assert.IsFalse(s3.Equals(s1));
            Assert.IsTrue(s3.Equals(s4));
            Assert.IsFalse(s3.Equals(s5));
            Assert.IsFalse(s3.Equals(s6));
        }

        [TestMethod]
        public void StringSegment_Equals_StringSegment_StringComparison_ArgumentChecking()
        {
            // NB: No known invalid inputs.

            Assert.IsTrue(true);
        }

        [TestMethod]
        public void StringSegment_Equals_StringSegment_StringComparison()
        {
            foreach (var c in new[] { StringComparison.CurrentCulture, StringComparison.Ordinal, StringComparison.OrdinalIgnoreCase })
            {
                AssertWithVariations(
                    new[] { "", "b", "bar", "foobar", "BAR", "fOoBaR" },
                    new[] { "", "b", "bar", "foobar", "BAR", "fOoBaR" },
                    (l, r) => l.Equals(r, c),
                    (l, r) => l.Equals(r, c)
                );
            }

            var n = default(StringSegment);
            var s = new StringSegment("bar");

            Assert.IsTrue(n.Equals(n, StringComparison.Ordinal));
            Assert.IsTrue(s.Equals(s, StringComparison.Ordinal));
            Assert.IsFalse(n.Equals(s, StringComparison.Ordinal));
            Assert.IsFalse(s.Equals(n, StringComparison.Ordinal));
        }

        [TestMethod]
        public void StringSegment_Format_StringSegment_ObjectArray_ArgumentChecking()
        {
            Assert.ThrowsException<ArgumentNullException>(() => StringSegment.Format(default, Array.Empty<object>()));
        }

        [TestMethod]
        public void StringSegment_Format_StringSegment_ObjectArray()
        {
            var os = new object[] { 42, "bar", new StringSegment("qfooq", 1, 3) };

            Assert.AreEqual(string.Format("{0} {1} {2}", os), (string)StringSegment.Format("{0} {1} {2}", os));
        }

        [TestMethod]
        public void StringSegment_Format_StringSegment_Object_ArgumentChecking()
        {
            Assert.ThrowsException<ArgumentNullException>(() => StringSegment.Format(default, 0));
        }

        [TestMethod]
        public void StringSegment_Format_StringSegment_Object()
        {
            Assert.AreEqual(string.Format("{0}", 42), (string)StringSegment.Format("{0}", 42));
        }

        [TestMethod]
        public void StringSegment_Format_StringSegment_Object_Object_ArgumentChecking()
        {
            Assert.ThrowsException<ArgumentNullException>(() => StringSegment.Format(default, 0, 1));
        }

        [TestMethod]
        public void StringSegment_Format_StringSegment_Object_Object()
        {
            Assert.AreEqual(string.Format("{0} {1}", 42, "bar"), (string)StringSegment.Format("{0} {1}", 42, "bar"));
        }

        [TestMethod]
        public void StringSegment_Format_StringSegment_Object_Object_Object_ArgumentChecking()
        {
            Assert.ThrowsException<ArgumentNullException>(() => StringSegment.Format(default, 0, 1, 2));
        }

        [TestMethod]
        public void StringSegment_Format_StringSegment_Object_Object_Object()
        {
            Assert.AreEqual(string.Format("{0} {1} {2}", 42, "bar", new StringSegment("qfooq", 1, 3)), (string)StringSegment.Format("{0} {1} {2}", 42, "bar", new StringSegment("qfooq", 1, 3)));
        }

        [TestMethod]
        public void StringSegment_Format_IFormatProvider_StringSegment_ObjectArray_ArgumentChecking()
        {
            Assert.ThrowsException<ArgumentNullException>(() => StringSegment.Format(CultureInfo.InvariantCulture, default, Array.Empty<object>()));
        }

        [TestMethod]
        public void StringSegment_Format_IFormatProvider_StringSegment_ObjectArray()
        {
            var os = new object[] { 42, "bar", new StringSegment("qfooq", 1, 3) };

            Assert.AreEqual(string.Format(CultureInfo.InvariantCulture, "{0} {1} {2}", os), (string)StringSegment.Format(CultureInfo.InvariantCulture, "{0} {1} {2}", os));
        }

        [TestMethod]
        public void StringSegment_GetEnumerator1()
        {
            WithVariations("bar", s =>
            {
                var e = s.GetEnumerator();

                for (var i = 0; i < 2; i++)
                {
                    Assert.ThrowsException<InvalidOperationException>(() => e.Current);

                    Assert.IsTrue(e.MoveNext());
                    Assert.AreEqual('b', e.Current);
                    Assert.AreEqual('b', ((IEnumerator)e).Current);

                    Assert.IsTrue(e.MoveNext());
                    Assert.AreEqual('a', e.Current);
                    Assert.AreEqual('a', ((IEnumerator)e).Current);

                    Assert.IsTrue(e.MoveNext());
                    Assert.AreEqual('r', e.Current);
                    Assert.AreEqual('r', ((IEnumerator)e).Current);

                    Assert.IsFalse(e.MoveNext());

                    Assert.ThrowsException<InvalidOperationException>(() => e.Current);

                    e.Reset();
                }

                e.Dispose();

                Assert.IsFalse(e.MoveNext());

                Assert.ThrowsException<InvalidOperationException>(() => e.Current);
            });
        }

        [TestMethod]
        public void StringSegment_GetEnumerator2()
        {
#pragma warning disable IDE0004 // Cast is added for expressiveness.
            WithVariations("bar", s =>
            {
                var s1 = new string(((IEnumerable)s).Cast<char>().ToArray());
                Assert.AreEqual("bar", s1);

                var s2 = new string(((IEnumerable<char>)s).ToArray());
                Assert.AreEqual("bar", s2);
            });
#pragma warning restore IDE0004
        }

        [TestMethod]
        public void StringSegment_GetEnumerator3()
        {
            WithVariations("bar", s =>
            {
                var e = ((IEnumerable)s).GetEnumerator();

                for (var i = 0; i < 2; i++)
                {
                    Assert.ThrowsException<InvalidOperationException>(() => e.Current);

                    Assert.IsTrue(e.MoveNext());
                    Assert.AreEqual('b', e.Current);

                    Assert.IsTrue(e.MoveNext());
                    Assert.AreEqual('a', e.Current);

                    Assert.IsTrue(e.MoveNext());
                    Assert.AreEqual('r', e.Current);

                    Assert.IsFalse(e.MoveNext());

                    Assert.ThrowsException<InvalidOperationException>(() => e.Current);

                    e.Reset();
                }
            });
        }

        [TestMethod]
        public void StringSegment_GetHashCode()
        {
            Assert.AreEqual(0, default(StringSegment).GetHashCode());

            var s1 = new StringSegment("foobarqux", 3, 3);
            var s2 = new StringSegment("barfoo", 0, 3);
            var s3 = new StringSegment("quxfoobar", 6, 3);

            Assert.AreNotEqual(s1.GetHashCode(), s2.GetHashCode());
            Assert.AreNotEqual(s1.GetHashCode(), s3.GetHashCode());
        }

        [TestMethod]
        public void StringSegment_IndexOf_Char_ArgumentChecking()
        {
            Assert.ThrowsException<NullReferenceException>(() => default(StringSegment).IndexOf('a'));
        }

        [TestMethod]
        public void StringSegment_IndexOf_Char()
        {
            foreach (var c in new[] { 'b', 'a', 'r', 'f', 'o', 'q', 'u', 'x', 'B', '!', '#' })
            {
                AssertWithVariations(
                    new[]
                    {
                        "",
                        "foo",
                        "bar",
                        "foobar",
                        "barfoo",
                        "qux",
                        "foobarqux",
                    },
                    s => s.IndexOf(c),
                    s => s.IndexOf(c)
                );
            }
        }

        [TestMethod]
        public void StringSegment_IndexOf_Char_Int32_ArgumentChecking()
        {
            Assert.ThrowsException<NullReferenceException>(() => default(StringSegment).IndexOf('a', 0));

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => _ = new StringSegment("bar").IndexOf('a', -1));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => _ = new StringSegment("bar").IndexOf('a', 4));
        }

        [TestMethod]
        public void StringSegment_IndexOf_Char_Int32()
        {
            foreach (var c in new[] { 'b', 'a', 'r', 'f', 'o', 'q', 'u', 'x', 'B', '!', '#' })
            {
                foreach (var startIndex in Enumerable.Range(0, 10))
                {
                    AssertWithVariations(
                        new[]
                        {
                            "",
                            "foo",
                            "bar",
                            "foobar",
                            "barfoo",
                            "qux",
                            "foobarqux",
                        },
                        s => s.IndexOf(c, startIndex),
                        s => s.IndexOf(c, startIndex)
                    );
                }
            }
        }

        [TestMethod]
        public void StringSegment_IndexOf_Char_Int32_Int32_ArgumentChecking()
        {
            Assert.ThrowsException<NullReferenceException>(() => default(StringSegment).IndexOf('a', 0, 1));

            WithVariations("bar", bar =>
            {
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => bar.IndexOf('a', -1, 1));
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => bar.IndexOf('a', 4, 1));

                Assert.ThrowsException<ArgumentOutOfRangeException>(() => bar.IndexOf('a', 0, -1));
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => bar.IndexOf('a', 0, 4));
            });
        }

        [TestMethod]
        public void StringSegment_IndexOf_Char_Int32_Int32()
        {
            foreach (var c in new[] { 'b', 'a', 'r', 'f', 'o', 'q', 'u', 'x', 'B', '!', '#' })
            {
                foreach (var startIndex in new[] { 0, 1, 3, 6, 10 })
                {
                    foreach (var count in new[] { 0, 1, 3, 6, 10 })
                    {
                        AssertWithVariations(
                            new[]
                            {
                                "",
                                "foo",
                                "bar",
                                "foobar",
                                "barfoo",
                                "qux",
                                "foobarqux",
                            },
                            s => s.IndexOf(c, startIndex, count),
                            s => s.IndexOf(c, startIndex, count)
                        );
                    }
                }
            }
        }

        [TestMethod]
        public void StringSegment_IndexOf_StringSegment_ArgumentChecking()
        {
            Assert.ThrowsException<NullReferenceException>(() => default(StringSegment).IndexOf("a"));

            WithVariations("bar", bar =>
            {
                Assert.ThrowsException<ArgumentNullException>(() => bar.IndexOf(default(StringSegment)));
            });
        }

        [TestMethod]
        public void StringSegment_IndexOf_StringSegment()
        {
            AssertWithVariations(
                new[] { "", "b", "ba", "bar", "barbar" },
                new[] { "", "b", "a", "r", "ar", "bar", "z", "xy", "!", "#" },
                (l, r) => l.IndexOf(r),
                (l, r) => l.IndexOf(r)
            );

            AssertWithVariations(
                new[] { "", "foobar", "foobarqux", "barfooquxfoo" },
                new[] { "", "b", "a", "r", "ar", "bar", "foo", "z", "xy", "!", "#" },
                (l, r) => l.IndexOf(r),
                (l, r) => l.IndexOf(r)
            );
        }

        [TestMethod]
        public void StringSegment_IndexOf_StringSegment_StringComparison_ArgumentChecking()
        {
            Assert.ThrowsException<NullReferenceException>(() => default(StringSegment).IndexOf("a", StringComparison.Ordinal));

            WithVariations("bar", bar =>
            {
                Assert.ThrowsException<ArgumentNullException>(() => bar.IndexOf(default, StringComparison.Ordinal));
            });
        }

        [TestMethod]
        public void StringSegment_IndexOf_StringSegment_StringComparison()
        {
            AssertWithVariations(
                new[] { "", "b", "ba", "bar", "barbar" },
                new[] { "", "b", "A", "r", "AR", "bar", "z", "xy", "!", "#" },
                (l, r) => l.IndexOf(r, StringComparison.OrdinalIgnoreCase),
                (l, r) => l.IndexOf(r, StringComparison.OrdinalIgnoreCase)
            );

            AssertWithVariations(
                new[] { "", "foobar", "foobarqux", "barfooquxfoo" },
                new[] { "", "b", "A", "r", "AR", "bar", "FOO", "z", "xy", "!", "#" },
                (l, r) => l.IndexOf(r, StringComparison.OrdinalIgnoreCase),
                (l, r) => l.IndexOf(r, StringComparison.OrdinalIgnoreCase)
            );
        }

        [TestMethod]
        public void StringSegment_IndexOf_StringSegment_Int32_ArgumentChecking()
        {
            Assert.ThrowsException<NullReferenceException>(() => default(StringSegment).IndexOf("a", 0));

            WithVariations("bar", bar =>
            {
                Assert.ThrowsException<ArgumentNullException>(() => bar.IndexOf(default(StringSegment), 0));

                Assert.ThrowsException<ArgumentOutOfRangeException>(() => bar.IndexOf("a", -1));
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => bar.IndexOf("a", 4));
            });
        }

        [TestMethod]
        public void StringSegment_IndexOf_StringSegment_Int32()
        {
            var s = "Where do you want to go today?";

            foreach (var startIndex in Enumerable.Range(0, s.Length))
            {
                AssertWithVariations(
                    new[] { s },
                    new[] { "", "Where", " ", "do", "you", "want to go", "today", "day?", "?", "tomorrow", "xyz", "!", "#" },
                    (l, r) => l.IndexOf(r, startIndex),
                    (l, r) => l.IndexOf(r, startIndex)
                );
            }
        }

        [TestMethod]
        public void StringSegment_IndexOf_StringSegment_Int32_Int32_ArgumentChecking()
        {
            Assert.ThrowsException<NullReferenceException>(() => default(StringSegment).IndexOf("a", 0, 1));

            WithVariations("bar", bar =>
            {
                Assert.ThrowsException<ArgumentNullException>(() => bar.IndexOf(default(StringSegment), 0, 1));

                Assert.ThrowsException<ArgumentOutOfRangeException>(() => bar.IndexOf("a", -1, 1));
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => bar.IndexOf("a", 4, 1));

                Assert.ThrowsException<ArgumentOutOfRangeException>(() => bar.IndexOf("a", 0, -1));
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => bar.IndexOf("a", 0, 4));

                Assert.ThrowsException<ArgumentOutOfRangeException>(() => bar.IndexOf("a", 0, 4));
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => bar.IndexOf("a", 1, 3));
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => bar.IndexOf("a", 2, 2));
            });
        }

        [TestMethod]
        public void StringSegment_IndexOf_StringSegment_Int32_Int32()
        {
            var s = "Where do you want to go today?";

            foreach (var startIndex in new[] { 0, 1, 2, 3, 5, 8, 13, 16, 21, 28, 29 })
            {
                foreach (var count in new[] { 0, 1, 2, 4, 8, 16, s.Length })
                {
                    AssertWithVariations(
                        new[] { s },
                        new[] { "", "Where", " ", "do", "you", "want to go", "today", "day?", "?", "tomorrow", "xyz", "!", "#" },
                        (l, r) => l.IndexOf(r, startIndex, count),
                        (l, r) => l.IndexOf(r, startIndex, count)
                    );
                }
            }
        }

        [TestMethod]
        public void StringSegment_IndexOf_StringSegment_Int32_StringComparison_ArgumentChecking()
        {
            Assert.ThrowsException<NullReferenceException>(() => default(StringSegment).IndexOf("a", 0, StringComparison.Ordinal));

            WithVariations("bar", bar =>
            {
                Assert.ThrowsException<ArgumentNullException>(() => bar.IndexOf(default, 0, StringComparison.Ordinal));

                Assert.ThrowsException<ArgumentOutOfRangeException>(() => bar.IndexOf("a", -1, StringComparison.Ordinal));
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => bar.IndexOf("a", 4, StringComparison.Ordinal));
            });
        }

        [TestMethod]
        public void StringSegment_IndexOf_StringSegment_Int32_StringComparison()
        {
            var s = "Where do you want to go today?";

            foreach (var startIndex in Enumerable.Range(0, s.Length))
            {
                foreach (var comparison in new[] { StringComparison.Ordinal, StringComparison.OrdinalIgnoreCase })
                {
                    AssertWithVariations(
                        new[] { s },
                        new[] { "", "Where", "DO", "wanT to Go", "Day?", "!", "#" },
                        (l, r) => l.IndexOf(r, startIndex, comparison),
                        (l, r) => l.IndexOf(r, startIndex, comparison)
                    );
                }
            }
        }

        [TestMethod]
        public void StringSegment_IndexOf_StringSegment_Int32_Int32_StringComparison_ArgumentChecking()
        {
            Assert.ThrowsException<NullReferenceException>(() => default(StringSegment).IndexOf("a", 0, 1, StringComparison.Ordinal));

            WithVariations("bar", bar =>
            {
                Assert.ThrowsException<ArgumentNullException>(() => bar.IndexOf(default, 0, 1, StringComparison.Ordinal));

                Assert.ThrowsException<ArgumentOutOfRangeException>(() => bar.IndexOf("a", -1, 1, StringComparison.Ordinal));
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => bar.IndexOf("a", 4, 1, StringComparison.Ordinal));

                Assert.ThrowsException<ArgumentOutOfRangeException>(() => bar.IndexOf("a", 0, -1, StringComparison.Ordinal));
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => bar.IndexOf("a", 0, 4, StringComparison.Ordinal));
            });
        }

        [TestMethod]
        public void StringSegment_IndexOf_StringSegment_Int32_Int32_StringComparison()
        {
            var s = "Where do you want to go today?";

            foreach (var startIndex in new[] { 0, 1, 2, 3, 5, 8, 13, 16, 21, 28, 29 })
            {
                foreach (var count in new[] { 0, 1, 2, 4, 8, 16, s.Length })
                {
                    foreach (var comparison in new[] { StringComparison.Ordinal, StringComparison.OrdinalIgnoreCase })
                    {
                        AssertWithVariations(
                            new[] { s },
                            new[] { "", "Where", "DO", "wanT to Go", "Day?", "!", "#" },
                            (l, r) => l.IndexOf(r, startIndex, count, comparison),
                            (l, r) => l.IndexOf(r, startIndex, count, comparison)
                        );
                    }
                }
            }
        }

        [TestMethod]
        public void StringSegment_IndexOf_StringSegment_Test()
        {
            var s = "foobarqux";

            foreach (var t in new[] { "", "f", "fo", "foo", "o", "ob", "oba", "bar", "barq", "qux", "ux", "x", "foobarqux" })
            {
                Assert.AreEqual(s.IndexOf(t), ((StringSegment)s).IndexOf(t));

                for (var i = 0; i <= s.Length; i++)
                {
                    Assert.AreEqual(s.IndexOf(t, i), ((StringSegment)s).IndexOf(t, i));

                    for (var c = 0; c < s.Length; c++)
                    {
                        if (i + c > s.Length)
                            continue;

                        Assert.AreEqual(s.IndexOf(t, i, c), ((StringSegment)s).IndexOf(t, i, c));
                    }
                }
            }
        }

        [TestMethod]
        public void StringSegment_IndexOfAny_CharArray_ArgumentChecking()
        {
            Assert.ThrowsException<NullReferenceException>(() => default(StringSegment).IndexOfAny(new[] { 'a' }));

            WithVariations("bar", bar =>
            {
                Assert.ThrowsException<ArgumentNullException>(() => bar.IndexOfAny(default));
            });
        }

        [TestMethod]
        public void StringSegment_IndexOfAny_CharArray()
        {
            foreach (var anyOf in new[] { new[] { 'b', 'a' }, new[] { 'b' }, new[] { 'b', 'q', 'f' }, new[] { 'o' }, new[] { 'a', 'o' }, new[] { 'Z', 'E', 'D' }, new[] { '#', '!' } })
            {
                AssertWithVariations(
                    new[]
                    {
                        "",
                        "foo",
                        "bar",
                        "foobar",
                        "barfoo",
                        "qux",
                        "foobarqux",
                    },
                    s => s.IndexOfAny(anyOf),
                    s => s.IndexOfAny(anyOf)
                );
            }
        }

        [TestMethod]
        public void StringSegment_IndexOfAny_CharArray_Int32_ArgumentChecking()
        {
            Assert.ThrowsException<NullReferenceException>(() => default(StringSegment).IndexOfAny(new[] { 'a' }, 0));

            WithVariations("bar", bar =>
            {
                Assert.ThrowsException<ArgumentNullException>(() => bar.IndexOfAny(default, 0));

                Assert.ThrowsException<ArgumentOutOfRangeException>(() => bar.IndexOfAny(new[] { 'a' }, -1));
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => bar.IndexOfAny(new[] { 'a' }, 4));
            });
        }

        [TestMethod]
        public void StringSegment_IndexOfAny_CharArray_Int32()
        {
            foreach (var anyOf in new[] { new[] { 'b', 'a' }, new[] { 'b' }, new[] { 'b', 'q', 'f' }, new[] { 'o' }, new[] { 'a', 'o' }, new[] { 'Z', 'E', 'D' }, new[] { '#', '!' } })
            {
                foreach (var startIndex in new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 })
                {
                    AssertWithVariations(
                        new[]
                        {
                            "",
                            "foo",
                            "bar",
                            "foobar",
                            "barfoo",
                            "qux",
                            "foobarqux",
                        },
                        s => s.IndexOfAny(anyOf, startIndex),
                        s => s.IndexOfAny(anyOf, startIndex)
                    );
                }
            }
        }

        [TestMethod]
        public void StringSegment_IndexOfAny_CharArray_Int32_Int32_ArgumentChecking()
        {
            Assert.ThrowsException<NullReferenceException>(() => default(StringSegment).IndexOfAny(new[] { 'a' }, 0, 1));

            WithVariations("bar", bar =>
            {
                Assert.ThrowsException<ArgumentNullException>(() => bar.IndexOfAny(default, 0, 1));

                Assert.ThrowsException<ArgumentOutOfRangeException>(() => bar.IndexOfAny(new[] { 'a' }, -1, 1));
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => bar.IndexOfAny(new[] { 'a' }, 4, 1));
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => bar.IndexOfAny(new[] { 'a' }, 0, -1));
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => bar.IndexOfAny(new[] { 'a' }, 1, 3));
            });
        }

        [TestMethod]
        public void StringSegment_IndexOfAny_CharArray_Int32_Int32()
        {
            foreach (var anyOf in new[] { new[] { 'b', 'a' }, new[] { 'b', 'q', 'f' }, new[] { 'o' }, new[] { 'a', 'o' }, new[] { 'Z', 'D' }, new[] { '#', '!' } })
            {
                foreach (var startIndex in new[] { 0, 1, 2, 3 })
                {
                    foreach (var count in new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 })
                    {
                        AssertWithVariations(
                            new[]
                            {
                                "",
                                "foo",
                                "bar",
                                "foobar",
                                "barfoo",
                                "qux",
                                "foobarqux",
                            },
                            s => s.IndexOfAny(anyOf, startIndex, count),
                            s => s.IndexOfAny(anyOf, startIndex, count)
                        );
                    }
                }
            }
        }

        [TestMethod]
        public void StringSegment_Insert_Int32_StringSegment_ArgumentChecking()
        {
            Assert.ThrowsException<NullReferenceException>(() => default(StringSegment).Insert(0, new StringSegment("foobar")));

            WithVariations("bar", bar =>
            {
                Assert.ThrowsException<ArgumentNullException>(() => bar.Insert(0, default));

                Assert.ThrowsException<ArgumentOutOfRangeException>(() => bar.Insert(-1, new StringSegment("foobar")));
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => bar.Insert(4, new StringSegment("foobar")));
            });
        }

        [TestMethod]
        public void StringSegment_Insert_Int32_StringSegment()
        {
            foreach (var startIndex in Enumerable.Range(0, 4))
            {
                AssertWithVariations(
                    new[] { "", "b", "ba", "bar" },
                    new[] { "", "f", "fo", "foo" },
                    (l, r) => l.Insert(startIndex, r),
                    (l, r) => l.Insert(startIndex, r)
                );
            }
        }

        [TestMethod]
        public void StringSegment_IsNullOrEmpty_StringSegment_ArgumentChecking()
        {
            // NB: No known invalid inputs.

            Assert.IsTrue(true);
        }

        [TestMethod]
        public void StringSegment_IsNullOrEmpty_StringSegment()
        {
            Assert.IsTrue(StringSegment.IsNullOrEmpty(value: null));
            Assert.IsTrue(StringSegment.IsNullOrEmpty(value: default));

            AssertWithVariations(
                new[] { "", " ", "bar" },
                s => string.IsNullOrEmpty(s),
                s => StringSegment.IsNullOrEmpty(s)
            );
        }

        [TestMethod]
        public void StringSegment_IsNullOrWhiteSpace_StringSegment_ArgumentChecking()
        {
            // NB: No known invalid inputs.

            Assert.IsTrue(true);
        }

        [TestMethod]
        public void StringSegment_IsNullOrWhiteSpace_StringSegment()
        {
            Assert.IsTrue(StringSegment.IsNullOrWhiteSpace(null));
            Assert.IsTrue(StringSegment.IsNullOrWhiteSpace(default));

            AssertWithVariations(
                new[] { "", " ", "  ", "bar", "\t", " \t\r\n" },
                s => string.IsNullOrWhiteSpace(s),
                s => StringSegment.IsNullOrWhiteSpace(s)
            );
        }

        [TestMethod]
        public void StringSegment_Join_StringSegment_IEnumerableOfT_ArgumentChecking()
        {
            WithVariations(",", comma =>
            {
                Assert.ThrowsException<ArgumentNullException>(() => StringSegment.Join(comma, default(IEnumerable<int>)));
            });
        }

        [TestMethod]
        public void StringSegment_Join_StringSegment_IEnumerableOfT1()
        {
            var xs = new List<int> { 2, 3, 5 };
            var ys = new List<int>();

            foreach (var col in new[] { xs, ys })
            {
                AssertWithVariations(
                    new[] { null, ",", ", ", " ,", " , " },
                    s => string.Join(s, col),
                    s => StringSegment.Join(s, col)
                );
            }
        }

        [TestMethod]
        public void StringSegment_Join_StringSegment_IEnumerableOfT2()
        {
            var xs = new List<string> { "bar", "foo", "", null, "qux" };
            var ys = new List<string> { null, "", "baz" };

            foreach (var col in new[] { xs, ys })
            {
                AssertWithVariations(
                    new[] { null, ",", ", ", " ,", " , " },
                    s => string.Join(s, col),
                    s => StringSegment.Join(s, col)
                );
            }
        }

        [TestMethod]
        public void StringSegment_Join_StringSegment_StringSegmentArray_ArgumentChecking()
        {
            WithVariations(",", comma =>
            {
                Assert.ThrowsException<ArgumentNullException>(() => StringSegment.Join(comma, default(StringSegment[])));
            });
        }

        [TestMethod]
        public void StringSegment_Join_StringSegment_StringSegmentArray()
        {
            var xs = new StringSegment[] { "bar", "foo", "", null, "qux" };
            var ys = Array.Empty<StringSegment>();

            foreach (var col in new[] { xs, ys })
            {
                AssertWithVariations(
                    new[] { null, ",", ", ", " ,", " , " },
                    s => string.Join(s, col),
                    s => StringSegment.Join(s, col)
                );
            }
        }

        [TestMethod]
        public void StringSegment_Join_StringSegment_StringSegmentArray_Int32_Int32_ArgumentChecking()
        {
            WithVariations(",", comma =>
            {
                Assert.ThrowsException<ArgumentNullException>(() => StringSegment.Join(comma, default, 0, 5));

                Assert.ThrowsException<ArgumentOutOfRangeException>(() => StringSegment.Join(comma, Array.Empty<StringSegment>(), -1, 5));
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => StringSegment.Join(comma, Array.Empty<StringSegment>(), 0, -1));
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => StringSegment.Join(comma, new StringSegment[1], 0, 2));
            });
        }

        [TestMethod]
        public void StringSegment_Join_StringSegment_StringSegmentArray_Int32_Int32_1()
        {
            var xs = new string[] { "bar", "foo", "", null, "qux" };
            var ys = xs.Select(x => (StringSegment)x).ToArray();

            AssertWithVariations(
                new[] { null, ",", ", ", " ,", " , " },
                s => string.Join(s, xs, 0, 5),
                s => StringSegment.Join(s, ys, 0, 5)
            );

            AssertWithVariations(
                new[] { null, ",", ", ", " ,", " , " },
                s => string.Join(s, xs, 1, 4),
                s => StringSegment.Join(s, ys, 1, 4)
            );

            AssertWithVariations(
                new[] { null, ",", ", ", " ,", " , " },
                s => string.Join(s, xs, 1, 3),
                s => StringSegment.Join(s, ys, 1, 3)
            );

            AssertWithVariations(
                new[] { null, ",", ", ", " ,", " , " },
                s => string.Join(s, xs, 1, 0),
                s => StringSegment.Join(s, ys, 1, 0)
            );
        }

        [TestMethod]
        public void StringSegment_Join_StringSegment_StringSegmentArray_Int32_Int32_2()
        {
            var xs = new string[] { "", "" };
            var ys = xs.Select(x => (StringSegment)x).ToArray();

            AssertWithVariations(
                new[] { "" },
                s => string.Join(s, xs, 0, 2),
                s => StringSegment.Join(s, ys, 0, 2)
            );
        }

        [TestMethod]
        public void StringSegment_Join_StringSegment_ObjectArray_ArgumentChecking()
        {
            WithVariations(",", comma =>
            {
                Assert.ThrowsException<ArgumentNullException>(() => StringSegment.Join(comma, default(object[])));
            });
        }

        [TestMethod]
        public void StringSegment_Join_StringSegment_ObjectArray1()
        {
            var xs = new object[] { 2, 3, 5 };
            var ys = Array.Empty<object>();

            foreach (var col in new[] { xs, ys })
            {
                AssertWithVariations(
                    new[] { null, ",", ", ", " ,", " , " },
                    s => string.Join(s, col),
                    s => StringSegment.Join(s, col)
                );
            }
        }

        [TestMethod]
        public void StringSegment_Join_StringSegment_ObjectArray2()
        {
            var xs = new object[] { "bar", "foo", "", null, "qux" };

            var ys = new object[] { "", "baz" }; // NB: The quirk on .NET Framework has been removed in .NET Core. Our .NET Standard library implements .NET Core semantics.

#if NETFRAMEWORK
            if (Type.GetType("Mono.Runtime") == null)
            {
                ys = new object[] { null, "", "baz" }; // COMPAT: Interesting quirk in behavior here due to the use of null in the first position. Quirk compatible.
            }
#endif

            foreach (var col in new[] { xs, ys })
            {
                AssertWithVariations(
                    new[] { null, ",", ", ", " ,", " , " },
                    s => string.Join(s, col),
                    s => StringSegment.Join(s, col)
                );
            }
        }

        [TestMethod]
        public void StringSegment_Join_StringSegment_ObjectArray3()
        {
            var xs = new object[] { (StringSegment)"bar", (StringSegment)"foo", (StringSegment)"", (StringSegment)null, (StringSegment)"qux" };
            var ys = new object[] { 1, "bar", true, (StringSegment)"foo" };

            foreach (var col in new[] { xs, ys })
            {
                AssertWithVariations(
                    new[] { null, ",", ", ", " ,", " , " },
                    s => string.Join(s, col),
                    s => StringSegment.Join(s, col)
                );
            }
        }

        [TestMethod]
        public void StringSegment_Join_StringSegment_IEnumerableOfStringSegment_ArgumentChecking()
        {
            WithVariations(",", comma =>
            {
                Assert.ThrowsException<ArgumentNullException>(() => StringSegment.Join(comma, default(IEnumerable<StringSegment>)));
            });
        }

        [TestMethod]
        public void StringSegment_Join_StringSegment_IEnumerableOfStringSegment()
        {
            var xs = new List<StringSegment> { "bar", "foo", "", null, "qux" };
            var ys = new List<StringSegment>();

            foreach (var col in new[] { xs, ys })
            {
                AssertWithVariations(
                    new[] { null, ",", ", ", " ,", " , " },
                    s => string.Join(s, col),
                    s => StringSegment.Join(s, col)
                );
            }
        }

        [TestMethod]
        public void StringSegment_LastIndexOf_Char_ArgumentChecking()
        {
            Assert.ThrowsException<NullReferenceException>(() => default(StringSegment).LastIndexOf('a'));
        }

        [TestMethod]
        public void StringSegment_LastIndexOf_Char()
        {
            foreach (var c in new[] { 'b', 'a', 'r', 'f', 'o', 'q', 'u', 'x', 'B', '!', '#' })
            {
                AssertWithVariations(
                    new[]
                    {
                        "",
                        "foo",
                        "bar",
                        "foobar",
                        "barfoo",
                        "qux",
                        "foobarqux",
                    },
                    s => s.LastIndexOf(c),
                    s => s.LastIndexOf(c)
                );
            }
        }

        [TestMethod]
        public void StringSegment_LastIndexOf_Char_Int32_ArgumentChecking()
        {
            Assert.ThrowsException<NullReferenceException>(() => default(StringSegment).LastIndexOf('a', 0));

            WithVariations("bar", bar =>
            {
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => bar.LastIndexOf('a', -1));
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => bar.LastIndexOf('a', 3));
            });
        }

        [TestMethod]
        public void StringSegment_LastIndexOf_Char_Int32()
        {
            foreach (var c in new[] { 'b', 'a', 'r', 'f', 'o', 'q', 'u', 'x', 'B', '!', '#' })
            {
                foreach (var startIndex in Enumerable.Range(0, 11).Reverse())
                {
                    AssertWithVariations(
                        new[]
                        {
                            "",
                            "foo",
                            "bar",
                            "foobar",
                            "barfoo",
                            "qux",
                            "foobarqux",
                        },
                        s => s.LastIndexOf(c, startIndex),
                        s => s.LastIndexOf(c, startIndex)
                    );
                }
            }
        }

        [TestMethod]
        public void StringSegment_LastIndexOf_Char_Int32_Int32_ArgumentChecking()
        {
            Assert.ThrowsException<NullReferenceException>(() => default(StringSegment).LastIndexOf('a', 0, 1));

            WithVariations("bar", bar =>
            {
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => bar.LastIndexOf('a', -1, 1));
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => bar.LastIndexOf('a', 3, 1));
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => bar.LastIndexOf('a', 0, -1));
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => bar.LastIndexOf('a', 0, 2));
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => bar.LastIndexOf('a', 1, 3));
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => bar.LastIndexOf('a', 2, 4));
            });
        }

        [TestMethod]
        public void StringSegment_LastIndexOf_Char_Int32_Int32()
        {
            foreach (var c in new[] { 'b', 'a', 'r', 'f', 'o', 'q', 'u', 'x', 'B', '!', '#' })
            {
                foreach (var startIndex in Enumerable.Range(0, 11).Reverse())
                {
                    foreach (var count in Enumerable.Range(0, 11))
                    {
                        AssertWithVariations(
                            new[]
                            {
                                "",
                                "foo",
                                "bar",
                                "foobar",
                                "barfoo",
                                "qux",
                                "foobarqux",
                            },
                            s => s.LastIndexOf(c, startIndex, count),
                            s => s.LastIndexOf(c, startIndex, count)
                        );
                    }
                }
            }
        }

        [TestMethod]
        public void StringSegment_LastIndexOf_StringSegment_ArgumentChecking()
        {
            Assert.ThrowsException<NullReferenceException>(() => default(StringSegment).LastIndexOf("a"));

            WithVariations("bar", bar =>
            {
                Assert.ThrowsException<ArgumentNullException>(() => bar.LastIndexOf(default(StringSegment)));
            });
        }

        [TestMethod]
        public void StringSegment_LastIndexOf_StringSegment()
        {
            AssertWithVariations(
                new[] { "", "b", "ba", "bar", "barbar" },
                new[] { "", "b", "a", "r", "ar", "bar", "z", "xy", "#", "!" },
                (l, r) => l.LastIndexOf(r),
                (l, r) => l.LastIndexOf(r)
            );

            AssertWithVariations(
                new[] { "", "foobar", "foobarqux", "barfooquxfoo" },
                new[] { "", "b", "a", "r", "ar", "bar", "foo", "z", "xy", "#", "!" },
                (l, r) => l.LastIndexOf(r),
                (l, r) => l.LastIndexOf(r)
            );
        }

        [TestMethod]
        public void StringSegment_LastIndexOf_StringSegment_Int32_ArgumentChecking()
        {
            Assert.ThrowsException<NullReferenceException>(() => default(StringSegment).LastIndexOf("a", 1));

            WithVariations("bar", bar =>
            {
                Assert.ThrowsException<ArgumentNullException>(() => bar.LastIndexOf(default(StringSegment), 1));

                Assert.ThrowsException<ArgumentOutOfRangeException>(() => bar.LastIndexOf("a", -1));
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => bar.LastIndexOf("a", 4));
            });
        }

        [TestMethod]
        public void StringSegment_LastIndexOf_StringSegment_Int32()
        {
            var s = "Where do you want to go today?";

            foreach (var startIndex in Enumerable.Range(0, s.Length))
            {
                AssertWithVariations(
                    new[] { s },
                    new[] { "", "Where", " ", "do", "you", "want to go", "today", "day?", "?", "tomorrow", "xyz", "!", "#" },
                    (l, r) => l.LastIndexOf(r, startIndex),
                    (l, r) => l.LastIndexOf(r, startIndex)
                );
            }
        }

        [TestMethod]
        public void StringSegment_LastIndexOf_StringSegment_StringComparison_ArgumentChecking()
        {
            Assert.ThrowsException<NullReferenceException>(() => default(StringSegment).LastIndexOf("a", StringComparison.Ordinal));

            WithVariations("bar", bar =>
            {
                Assert.ThrowsException<ArgumentNullException>(() => bar.LastIndexOf(default, StringComparison.Ordinal));
            });
        }

        [TestMethod]
        public void StringSegment_LastIndexOf_StringSegment_StringComparison()
        {
            AssertWithVariations(
                new[] { "", "b", "ba", "bar", "barbar" },
                new[] { "", "b", "A", "r", "AR", "bar", "z", "xy", "!", "#" },
                (l, r) => l.LastIndexOf(r, StringComparison.OrdinalIgnoreCase),
                (l, r) => l.LastIndexOf(r, StringComparison.OrdinalIgnoreCase)
            );

            AssertWithVariations(
                new[] { "", "foobar", "foobarqux", "barfooquxfoo" },
                new[] { "", "b", "A", "r", "AR", "bar", "FOO", "z", "xy", "!", "#" },
                (l, r) => l.LastIndexOf(r, StringComparison.OrdinalIgnoreCase),
                (l, r) => l.LastIndexOf(r, StringComparison.OrdinalIgnoreCase)
            );
        }

        [TestMethod]
        public void StringSegment_LastIndexOf_StringSegment_Int32_Int32_ArgumentChecking()
        {
            Assert.ThrowsException<NullReferenceException>(() => default(StringSegment).LastIndexOf("a", 0, 1));

            WithVariations("bar", bar =>
            {
                Assert.ThrowsException<ArgumentNullException>(() => bar.LastIndexOf(default(StringSegment), 0, 1));

                Assert.ThrowsException<ArgumentOutOfRangeException>(() => bar.LastIndexOf("a", -1, 1));
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => bar.LastIndexOf("a", 4, 1));

                Assert.ThrowsException<ArgumentOutOfRangeException>(() => bar.LastIndexOf("a", 0, -1));
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => bar.LastIndexOf("a", 0, 4));

                Assert.ThrowsException<ArgumentOutOfRangeException>(() => bar.LastIndexOf("a", 0, 2));
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => bar.LastIndexOf("a", 1, 3));
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => bar.LastIndexOf("a", 2, 4));
            });
        }

        [TestMethod]
        public void StringSegment_LastIndexOf_StringSegment_Int32_Int32()
        {
            var s = "Where do you want to go today?";

            foreach (var startIndex in new[] { 0, 1, 2, 3, 5, 8, 13, 16, 21, 28, 29 })
            {
                foreach (var count in new[] { 0, 1, 2, 4, 8, 16, s.Length }.Select(c => s.Length - c))
                {
                    AssertWithVariations(
                        new[] { s },
                        new[] { "", "Where", " ", "do", "you", "want to go", "today", "day?", "?", "tomorrow", "xyz", "!", "#" },
                        (l, r) => l.LastIndexOf(r, startIndex, count),
                        (l, r) => l.LastIndexOf(r, startIndex, count)
                    );
                }
            }
        }

        [TestMethod]
        public void StringSegment_LastIndexOf_StringSegment_Int32_StringComparison_ArgumentChecking()
        {
            Assert.ThrowsException<NullReferenceException>(() => default(StringSegment).LastIndexOf("a", 1, StringComparison.Ordinal));

            WithVariations("bar", bar =>
            {
                Assert.ThrowsException<ArgumentNullException>(() => bar.LastIndexOf(default, 1, StringComparison.Ordinal));

                Assert.ThrowsException<ArgumentOutOfRangeException>(() => bar.LastIndexOf("a", -1, StringComparison.Ordinal));
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => bar.LastIndexOf("a", 4, StringComparison.Ordinal));
            });
        }

        [TestMethod]
        public void StringSegment_LastIndexOf_StringSegment_Int32_StringComparison()
        {
            var s = "Where do you want to go today?";

            foreach (var startIndex in Enumerable.Range(0, s.Length))
            {
                foreach (var comparison in new[] { StringComparison.Ordinal, StringComparison.OrdinalIgnoreCase })
                {
                    AssertWithVariations(
                        new[] { s },
                        new[] { "", "Where", "DO", "wanT to Go", "Day?", "!", "#" },
                        (l, r) => l.LastIndexOf(r, startIndex, comparison),
                        (l, r) => l.LastIndexOf(r, startIndex, comparison)
                    );
                }
            }
        }

        [TestMethod]
        public void StringSegment_LastIndexOf_StringSegment_Int32_Int32_StringComparison_ArgumentChecking()
        {
            Assert.ThrowsException<NullReferenceException>(() => default(StringSegment).LastIndexOf("a", 1, 1, StringComparison.Ordinal));

            WithVariations("bar", bar =>
            {
                Assert.ThrowsException<ArgumentNullException>(() => bar.LastIndexOf(default, 1, 1, StringComparison.Ordinal));

                Assert.ThrowsException<ArgumentOutOfRangeException>(() => bar.LastIndexOf("a", -1, 1, StringComparison.Ordinal));
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => bar.LastIndexOf("a", 4, 1, StringComparison.Ordinal));
            });
        }

        [TestMethod]
        public void StringSegment_LastIndexOf_StringSegment_Int32_Int32_StringComparison()
        {
            var s = "Where do you want to go today?";

            foreach (var startIndex in new[] { 0, 1, 2, 3, 5, 8, 13, 16, 21, 28, 29 })
            {
                foreach (var count in new[] { 0, 1, 2, 4, 8, 16, s.Length }.Select(c => s.Length - c))
                {
                    foreach (var comparison in new[] { StringComparison.Ordinal, StringComparison.OrdinalIgnoreCase })
                    {
                        AssertWithVariations(
                            new[] { s },
                            new[] { "", "Where", "DO", "wanT to Go", "Day?", "!", "#" },
                            (l, r) => l.LastIndexOf(r, startIndex, count, comparison),
                            (l, r) => l.LastIndexOf(r, startIndex, count, comparison)
                        );
                    }
                }
            }
        }

        [TestMethod]
        public void StringSegment_LastIndexOf_StringSegment_Test()
        {
            var s = "foobarqux";

            foreach (var t in new[] { "", "f", "fo", "foo", "o", "ob", "oba", "bar", "barq", "qux", "ux", "x", "foobarqux" })
            {
                Assert.AreEqual(s.LastIndexOf(t), ((StringSegment)s).LastIndexOf(t));

                for (var i = 0; i <= s.Length; i++)
                {
                    Assert.AreEqual(s.LastIndexOf(t, i), ((StringSegment)s).LastIndexOf(t, i));

                    for (var c = 0; c < s.Length; c++)
                    {
                        if (c >= i)
                            continue;

                        Assert.AreEqual(s.LastIndexOf(t, i, c), ((StringSegment)s).LastIndexOf(t, i, c));
                    }
                }
            }
        }

        [TestMethod]
        public void StringSegment_LastIndexOf_StringSegment_EdgeCases()
        {
            AssertWithVariations(
                new[] { "" },
                new[] { "", "bar" },
                (l, r) => l.LastIndexOf(r, -1),
                (l, r) => l.LastIndexOf(r, 0)
            );
        }

        [TestMethod]
        public void StringSegment_LastIndexOfAny_CharArray_ArgumentChecking()
        {
            Assert.ThrowsException<NullReferenceException>(() => default(StringSegment).LastIndexOfAny(new[] { 'a' }));

            WithVariations("bar", bar =>
            {
                Assert.ThrowsException<ArgumentNullException>(() => bar.LastIndexOfAny(default));
            });
        }

        [TestMethod]
        public void StringSegment_LastIndexOfAny_CharArray()
        {
            foreach (var anyOf in new[] { new[] { 'b', 'a' }, new[] { 'b' }, new[] { 'b', 'q', 'f' }, new[] { 'o' }, new[] { 'a', 'o' }, new[] { 'Z', 'E', 'D' }, new[] { '#', '!' } })
            {
                AssertWithVariations(
                    new[]
                    {
                        "",
                        "foo",
                        "bar",
                        "foobar",
                        "barfoo",
                        "qux",
                        "foobarqux",
                    },
                    s => s.LastIndexOfAny(anyOf),
                    s => s.LastIndexOfAny(anyOf)
                );
            }
        }

        [TestMethod]
        public void StringSegment_LastIndexOfAny_CharArray_Int32_ArgumentChecking()
        {
            Assert.ThrowsException<NullReferenceException>(() => default(StringSegment).LastIndexOfAny(new[] { 'a' }, 0));

            WithVariations("bar", bar =>
            {
                Assert.ThrowsException<ArgumentNullException>(() => bar.LastIndexOfAny(default, 0));

                Assert.ThrowsException<ArgumentOutOfRangeException>(() => bar.LastIndexOfAny(new[] { 'a' }, -1));
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => bar.LastIndexOfAny(new[] { 'a' }, 4));
            });
        }

        [TestMethod]
        public void StringSegment_LastIndexOfAny_CharArray_Int32()
        {
            foreach (var anyOf in new[] { new[] { 'b', 'a' }, new[] { 'b', 'q', 'f' }, new[] { 'o' }, new[] { 'a', 'o' }, new[] { 'Z', 'D' }, new[] { '#', '!' } })
            {
                foreach (var startIndex in new[] { 0, 1, 2, 3 })
                {
                    AssertWithVariations(
                        new[]
                        {
                            "",
                            "foo",
                            "bar",
                            "foobar",
                            "barfoo",
                            "qux",
                            "foobarqux",
                        },
                        s => s.LastIndexOfAny(anyOf, startIndex),
                        s => s.LastIndexOfAny(anyOf, startIndex)
                    );
                }
            }
        }

        [TestMethod]
        public void StringSegment_LastIndexOfAny_CharArray_Int32_Int32_ArgumentChecking()
        {
            Assert.ThrowsException<NullReferenceException>(() => default(StringSegment).LastIndexOfAny(new[] { 'a' }, 0, 1));

            WithVariations("bar", bar =>
            {
                Assert.ThrowsException<ArgumentNullException>(() => bar.LastIndexOfAny(default, 0, 1));

                Assert.ThrowsException<ArgumentOutOfRangeException>(() => bar.LastIndexOfAny(new[] { 'a' }, -1, 1));
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => bar.LastIndexOfAny(new[] { 'a' }, 4, 1));
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => bar.LastIndexOfAny(new[] { 'a' }, 0, -1));
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => bar.LastIndexOfAny(new[] { 'a' }, 1, 3));
            });
        }

        [TestMethod]
        public void StringSegment_LastIndexOfAny_CharArray_Int32_Int32()
        {
            foreach (var anyOf in new[] { new[] { 'b', 'a' }, new[] { 'b', 'q', 'f' }, new[] { 'o' }, new[] { 'a', 'o' }, new[] { 'Z', 'D' }, new[] { '#', '!' } })
            {
                foreach (var startIndex in new[] { 0, 1, 2, 3 })
                {
                    foreach (var count in new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 })
                    {
                        AssertWithVariations(
                            new[]
                            {
                                "",
                                "foo",
                                "bar",
                                "foobar",
                                "barfoo",
                                "qux",
                                "foobarqux",
                            },
                            s => s.LastIndexOfAny(anyOf, startIndex, count),
                            s => s.LastIndexOfAny(anyOf, startIndex, count)
                        );
                    }
                }
            }
        }

        [TestMethod]
        public void StringSegment_PadLeft_Int32_ArgumentChecking()
        {
            Assert.ThrowsException<NullReferenceException>(() => default(StringSegment).PadLeft(1));

            WithVariations("bar", bar =>
            {
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => bar.PadLeft(-1));
            });
        }

        [TestMethod]
        public void StringSegment_PadLeft_Int32()
        {
            foreach (var i in new[] { 0, 1, 2, 3, 4, 5, 6 })
            {
                AssertWithVariations(
                    new[] { "", " ", "  ", "b", "ba", "bar", "barf", "barfo", "barfoo", "barfooq", "barfooqu", "barfooqux" },
                    s => s.PadLeft(i),
                    s => s.PadLeft(i)
                );
            }

            var bar = new StringSegment("^    bar$$$$$", 5, 3);

            for (var i = 3; i <= 9; i++)
            {
                var res = bar.PadLeft(i);
                Assert.AreEqual("bar".PadLeft(i), res.ToString());

                if (i <= 7)
                {
                    Assert.AreSame(bar.String, res.String);
                }
            }
        }

        [TestMethod]
        public void StringSegment_PadLeft_Int32_Char_ArgumentChecking()
        {
            Assert.ThrowsException<NullReferenceException>(() => default(StringSegment).PadLeft(1, ' '));

            WithVariations("bar", bar =>
            {
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => bar.PadLeft(-1, ' '));
            });
        }

        [TestMethod]
        public void StringSegment_PadLeft_Int32_Char()
        {
            foreach (var c in new[] { ' ', '*' })
            {
                foreach (var i in new[] { 0, 1, 2, 3, 4, 5, 6 })
                {
                    AssertWithVariations(
                        new[] { "", " ", "  ", "b", "ba", "bar", "barf", "barfo", "barfoo", "barfooq", "barfooqu", "barfooqux" },
                        s => s.PadLeft(i, c),
                        s => s.PadLeft(i, c)
                    );
                }
            }

            var bar = new StringSegment("^****bar$$$$$", 5, 3);

            for (var i = 3; i <= 9; i++)
            {
                var res = bar.PadLeft(i, '*');
                Assert.AreEqual("bar".PadLeft(i, '*'), res.ToString());

                if (i <= 7)
                {
                    Assert.AreSame(bar.String, res.String);
                }
            }
        }

        [TestMethod]
        public void StringSegment_PadRight_Int32_ArgumentChecking()
        {
            Assert.ThrowsException<NullReferenceException>(() => default(StringSegment).PadRight(1));

            WithVariations("bar", bar =>
            {
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => bar.PadRight(-1));
            });
        }

        [TestMethod]
        public void StringSegment_PadRight_Int32()
        {
            foreach (var i in new[] { 0, 1, 2, 3, 4, 5, 6 })
            {
                AssertWithVariations(
                    new[] { "", " ", "  ", "b", "ba", "bar", "barf", "barfo", "barfoo", "barfooq", "barfooqu", "barfooqux" },
                    s => s.PadRight(i),
                    s => s.PadRight(i)
                );
            }

            var bar = new StringSegment("^^^^^bar    $", 5, 3);

            for (var i = 3; i <= 9; i++)
            {
                var res = bar.PadRight(i);
                Assert.AreEqual("bar".PadRight(i), res.ToString());

                if (i <= 7)
                {
                    Assert.AreSame(bar.String, res.String);
                }
            }
        }

        [TestMethod]
        public void StringSegment_PadRight_Int32_Char_ArgumentChecking()
        {
            Assert.ThrowsException<NullReferenceException>(() => default(StringSegment).PadRight(1, ' '));

            WithVariations("bar", bar =>
            {
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => bar.PadRight(-1, ' '));
            });
        }

        [TestMethod]
        public void StringSegment_PadRight_Int32_Char()
        {
            foreach (var c in new[] { ' ', '*' })
            {
                foreach (var i in new[] { 0, 1, 2, 3, 4, 5, 6 })
                {
                    AssertWithVariations(
                        new[] { "", " ", "  ", "b", "ba", "bar", "barf", "barfo", "barfoo", "barfooq", "barfooqu", "barfooqux" },
                        s => s.PadRight(i, c),
                        s => s.PadRight(i, c)
                    );
                }
            }

            var bar = new StringSegment("^^^^^bar****$", 5, 3);

            for (var i = 3; i <= 9; i++)
            {
                var res = bar.PadRight(i, '*');
                Assert.AreEqual("bar".PadRight(i, '*'), res.ToString());

                if (i <= 7)
                {
                    Assert.AreSame(bar.String, res.String);
                }
            }
        }

        [TestMethod]
        public void StringSegment_Remove_Int32_ArgumentChecking()
        {
            Assert.ThrowsException<NullReferenceException>(() => default(StringSegment).Remove(0));

            // NB: Range checks covered by tests below.
        }

        [TestMethod]
        public void StringSegment_Remove_Int32()
        {
            foreach (var startIndex in new[] { -1, 0, 1, 2, 3 })
            {
                AssertWithVariations(
                    new[] { "bar", "foo", "foobar", "quxfoobaz" },
                    s => s.Remove(startIndex),
                    s => s.Remove(startIndex)
                );
            }
        }

        [TestMethod]
        public void StringSegment_Remove_Int32_Int32_ArgumentChecking()
        {
            Assert.ThrowsException<NullReferenceException>(() => default(StringSegment).Remove(0, 0));

            // NB: Range checks covered by tests below.
        }

        [TestMethod]
        public void StringSegment_Remove_Int32_Int32()
        {
            foreach (var startIndex in new[] { -1, 0, 1, 2, 3 })
            {
                foreach (var count in new[] { -1, 0, 1, 2, 3 })
                {
                    AssertWithVariations(
                        new[] { "bar", "foo", "foobar", "quxfoobaz" },
                        s => s.Remove(startIndex, count),
                        s => s.Remove(startIndex, count)
                    );
                }
            }
        }

        [TestMethod]
        public void StringSegment_Replace_StringSegment_StringSegment_ArgumentChecking()
        {
            Assert.ThrowsException<NullReferenceException>(() => default(StringSegment).Replace("a", "b"));

            WithVariations("bar", bar =>
            {
                Assert.ThrowsException<ArgumentNullException>(() => bar.Replace(default, "foo"));
                Assert.ThrowsException<ArgumentException>(() => bar.Replace("", "foo"));
            });
        }

        [TestMethod]
        public void StringSegment_Replace_StringSegment_StringSegment()
        {
            foreach (var o in new[] { "I", "sushi", "bar", "steak", "ut", "." })
            {
                foreach (var n in new[] { null, "", "U", "sushi", "steak", "!" })
                {
                    AssertWithVariations(
                        new[] { "I <3 sushi with peanut butter and soda at the bar." },
                        s => s.Replace(o, n),
                        s => s.Replace(o, n)
                    );
                }
            }
        }

        [TestMethod]
        public void StringSegment_Replace_Char_Char_ArgumentChecking()
        {
            Assert.ThrowsException<NullReferenceException>(() => default(StringSegment).Replace('a', 'b'));
        }

        [TestMethod]
        public void StringSegment_Replace_Char_Char()
        {
            foreach (var o in new[] { 'a', 'b', '<', '3', 'z' })
            {
                foreach (var n in new[] { 'f', 'o', 'q' })
                {
                    AssertWithVariations(
                        new[] { "I <3 sushi with peanut butter and soda at the bar." },
                        s => s.Replace(o, n),
                        s => s.Replace(o, n)
                    );
                }
            }
        }

        [TestMethod]
        public void StringSegment_Split_CharArray_ArgumentChecking()
        {
            Assert.ThrowsException<NullReferenceException>(() => default(StringSegment).Split(new[] { ',' }));
        }

        [TestMethod]
        public void StringSegment_Split_CharArray()
        {
            foreach (var seps in new[] { null, Array.Empty<char>(), new[] { ',' }, new[] { ';' }, new[] { ',', ';' } })
            {
                AssertWithVariations(
                    new[]
                    {
                        "",
                        "1",
                        "bar,foo",
                        "1,bar;true,,qux",
                        ",",
                        ",;",
                        ",,,",
                        "foo\tbar qux",
                    },
                    s => s.Split(seps),
                    s => s.Split(seps),
                    (e, a) => e.SequenceEqual(a.Select(s => (string)s))
                );
            }
        }

        [TestMethod]
        public void StringSegment_Split_CharArray_StringSplitOptions_ArgumentChecking()
        {
            Assert.ThrowsException<NullReferenceException>(() => default(StringSegment).Split(new[] { ',' }, StringSplitOptions.None));

            WithVariations("bar", bar =>
            {
                Assert.ThrowsException<ArgumentException>(() => bar.Split(new[] { ',' }, (StringSplitOptions)(-1)));
                Assert.ThrowsException<ArgumentException>(() => bar.Split(new[] { ',' }, (StringSplitOptions)2));
            });
        }

        [TestMethod]
        public void StringSegment_Split_CharArray_StringSplitOptions()
        {
            foreach (var options in new[] { StringSplitOptions.None, StringSplitOptions.RemoveEmptyEntries })
            {
                foreach (var seps in new[] { null, Array.Empty<char>(), new[] { ',' }, new[] { ';' }, new[] { ',', ';' } })
                {
                    AssertWithVariations(
                        new[]
                        {
                            "",
                            "1",
                            "bar,foo",
                            "1,bar;true,,qux",
                            ",",
                            ",;",
                            ",,,",
                            "foo\tbar qux",
                        },
                        s => s.Split(seps, options),
                        s => s.Split(seps, options),
                        (e, a) => e.SequenceEqual(a.Select(s => (string)s))
                    );
                }
            }
        }

        [TestMethod]
        public void StringSegment_Split_CharArray_Int32_ArgumentChecking()
        {
            Assert.ThrowsException<NullReferenceException>(() => default(StringSegment).Split(new[] { ',' }, 1));

            WithVariations("bar", bar =>
            {
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => bar.Split(new[] { ',' }, -1));
            });
        }

        [TestMethod]
        public void StringSegment_Split_CharArray_Int32()
        {
            foreach (var count in new[] { 0, 1, 2, 3, 4, 127 })
            {
                foreach (var seps in new[] { null, Array.Empty<char>(), new[] { ',' }, new[] { ';' }, new[] { ',', ';' } })
                {
                    AssertWithVariations(
                        new[]
                        {
                            "",
                            "1",
                            "bar,foo",
                            "1,bar;true,,qux",
                            ",",
                            ",;",
                            ",,,",
                            "foo\tbar qux",
                        },
                        s => s.Split(seps, count),
                        s => s.Split(seps, count),
                        (e, a) => e.SequenceEqual(a.Select(s => (string)s))
                    );
                }
            }
        }

        [TestMethod]
        public void StringSegment_Split_CharArray_Int32_StringSplitOptions_ArgumentChecking()
        {
            Assert.ThrowsException<NullReferenceException>(() => default(StringSegment).Split(new[] { ',' }, 1, StringSplitOptions.None));

            WithVariations("bar", bar =>
            {
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => bar.Split(new[] { ',' }, -1, StringSplitOptions.None));

                Assert.ThrowsException<ArgumentException>(() => bar.Split(new[] { ',' }, 1, (StringSplitOptions)(-1)));
                Assert.ThrowsException<ArgumentException>(() => bar.Split(new[] { ',' }, 1, (StringSplitOptions)2));
            });
        }

        [TestMethod]
        public void StringSegment_Split_CharArray_Int32_StringSplitOptions()
        {
            foreach (var count in new[] { 0, 1, 2, 3, 4, 127 })
            {
                foreach (var options in new[] { StringSplitOptions.None, StringSplitOptions.RemoveEmptyEntries })
                {
                    foreach (var seps in new[] { null, Array.Empty<char>(), new[] { ',' }, new[] { ';' }, new[] { ',', ';' } })
                    {
                        AssertWithVariations(
                            new[]
                            {
                                "",
                                "1",
                                "bar,foo",
                                "1,bar;true,,qux",
                                ",",
                                ",;",
                                ",,,",
                                "foo\tbar qux",
                            },
                            s => s.Split(seps, count, options),
                            s => s.Split(seps, count, options),
                            (e, a) => e.SequenceEqual(a.Select(s => (string)s))
                        );
                    }
                }
            }
        }

        [TestMethod]
        public void StringSegment_Split_StringSegmentArray_StringSplitOptions_ArgumentChecking()
        {
            Assert.ThrowsException<NullReferenceException>(() => default(StringSegment).Split(Array.Empty<StringSegment>(), StringSplitOptions.None));

            WithVariations("bar", bar =>
            {
                Assert.ThrowsException<ArgumentException>(() => bar.Split(Array.Empty<StringSegment>(), (StringSplitOptions)(-1)));
                Assert.ThrowsException<ArgumentException>(() => bar.Split(Array.Empty<StringSegment>(), (StringSplitOptions)2));
            });
        }

        [TestMethod]
        public void StringSegment_Split_StringSegmentArray_StringSplitOptions()
        {
            foreach (var options in new[] { StringSplitOptions.None, StringSplitOptions.RemoveEmptyEntries })
            {
                foreach (var seps in new[] { Array.Empty<string>(), new[] { ",", ";" }, new[] { ")*(", "++", "_-_" } })
                {
                    var sept = seps.Select(s => (StringSegment)s).ToArray();

                    AssertWithVariations(
                        new[]
                        {
                            "",
                            "bar",
                            "bar,foo;qux,,baz,",
                            "bar++foo++++qux++baz",
                            "_-_bar)*(++foo_)+)*(++baz",
                        },
                        s => s.Split(seps, options),
                        s => s.Split(sept, options),
                        (e, a) => e.SequenceEqual(a.Select(s => (string)s))
                    );
                }
            }
        }

        [TestMethod]
        public void StringSegment_Split_StringSegmentArray_Int32_StringSplitOptions_ArgumentChecking()
        {
            Assert.ThrowsException<NullReferenceException>(() => default(StringSegment).Split(Array.Empty<StringSegment>(), 1, StringSplitOptions.None));

            WithVariations("bar", bar =>
            {
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => bar.Split(Array.Empty<StringSegment>(), -1, StringSplitOptions.None));
                Assert.ThrowsException<ArgumentException>(() => bar.Split(Array.Empty<StringSegment>(), 1, (StringSplitOptions)(-1)));
                Assert.ThrowsException<ArgumentException>(() => bar.Split(Array.Empty<StringSegment>(), 1, (StringSplitOptions)2));
            });
        }

        [TestMethod]
        public void StringSegment_Split_StringSegmentArray_Int32_StringSplitOptions()
        {
            foreach (var count in new[] { 0, 1, 2, 3, 4, 127 })
            {
                foreach (var options in new[] { StringSplitOptions.None, StringSplitOptions.RemoveEmptyEntries })
                {
                    foreach (var seps in new[] { null, Array.Empty<string>(), new[] { default(string) }, new[] { "" }, new[] { ",", ";" }, new[] { ")*(", "++", "_-_" } })
                    {
                        var sept = seps?.Select(s => (StringSegment)s).ToArray();

                        AssertWithVariations(
                            new[]
                            {
                                "",
                                "bar",
                                "bar,foo;qux,,baz,",
                                "bar++foo++++qux++baz",
                                "_-_bar)*(++foo_)+)*(++baz",
                            },
                            s => s.Split(seps, count, options),
                            s => s.Split(sept, count, options),
                            (e, a) => e.SequenceEqual(a.Select(s => (string)s))
                        );
                    }
                }
            }
        }

        [TestMethod]
        public void StringSegment_StartsWith_StringSegment_ArgumentChecking()
        {
            Assert.ThrowsException<NullReferenceException>(() => default(StringSegment).StartsWith("bar"));

            WithVariations("bar", bar =>
            {
                Assert.ThrowsException<ArgumentNullException>(() => bar.StartsWith(value: null));
            });
        }

        [TestMethod]
        public void StringSegment_StartsWith_StringSegment()
        {
            AssertWithVariations(
                new[] { "", "bar", "foobar", "foobarqux", "barfoo" },
                new[] { "", "bar", "foo", "qux", "baz", "a", "B", "ar", "ba" },
                (l, r) => l.StartsWith(r),
                (l, r) => l.StartsWith(r)
            );
        }

        [TestMethod]
        public void StringSegment_StartsWith_StringSegment_StringComparison_ArgumentChecking()
        {
            Assert.ThrowsException<NullReferenceException>(() => default(StringSegment).StartsWith("bar", StringComparison.CurrentCulture));

            WithVariations("bar", bar =>
            {
                Assert.ThrowsException<ArgumentNullException>(() => bar.StartsWith(value: null, StringComparison.CurrentCulture));
            });
        }

        [TestMethod]
        public void StringSegment_StartsWith_StringSegment_StringComparison()
        {
            AssertWithVariations(
                new[] { "", "bar", "foobar", "foobarqux", "barfoo" },
                new[] { "", "bar", "BAR", "qux", "UX", "B", "baz", "F", "FoO" },
                (l, r) => l.StartsWith(r, StringComparison.OrdinalIgnoreCase),
                (l, r) => l.StartsWith(r, StringComparison.OrdinalIgnoreCase)
            );
        }

        [TestMethod]
        public void StringSegment_StartsWith_StringSegment_Boolean_CultureInfo_ArgumentChecking()
        {
            Assert.ThrowsException<NullReferenceException>(() => default(StringSegment).StartsWith("bar", ignoreCase: true, CultureInfo.InvariantCulture));

            WithVariations("bar", bar =>
            {
                Assert.ThrowsException<ArgumentNullException>(() => bar.StartsWith(value: null, ignoreCase: true, CultureInfo.InvariantCulture));
            });
        }

        [TestMethod]
        public void StringSegment_StartsWith_StringSegment_Boolean_CultureInfo()
        {
            foreach (var culture in new[] { null, CultureInfo.CurrentCulture, CultureInfo.InvariantCulture })
            {
                foreach (var ignoreCase in new[] { false, true })
                {
                    AssertWithVariations(
                        new[] { "", "bar", "foobar", "foobarqux", "barfoo" },
                        new[] { "", "bar", "BAR", "qux", "UX", "B", "baz", "F", "FoO" },
                        (l, r) => l.StartsWith(r, ignoreCase, culture),
                        (l, r) => l.StartsWith(r, ignoreCase, culture)
                    );
                }
            }
        }

        [TestMethod]
        public void StringSegment_Substring_Int32_ArgumentChecking()
        {
            Assert.ThrowsException<NullReferenceException>(() => default(StringSegment).Substring(0));

            WithVariations("bar", bar =>
            {
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => bar.Substring(-1));
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => bar.Substring(4));
            });
        }

        [TestMethod]
        public void StringSegment_Substring_Int32()
        {
#pragma warning disable IDE0079 // Remove unnecessary suppression (only applies to .NET 5)
#pragma warning disable IDE0057 // Substring can be simplified
            foreach (var startIndex in Enumerable.Range(-1, 8))
            {
                AssertWithVariations(
                    new[] { "", "b", "ba", "bar", "foobar" },
                    s => s.Substring(startIndex),
                    s => s.Substring(startIndex)
                );
            }
#pragma warning restore IDE0075
#pragma warning restore IDE0079
        }

        [TestMethod]
        public void StringSegment_Substring_Int32_Int32_ArgumentChecking()
        {
            Assert.ThrowsException<NullReferenceException>(() => default(StringSegment).Substring(0, 0));

            WithVariations("bar", bar =>
            {
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => bar.Substring(-1, 0));
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => bar.Substring(4, 0));
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => bar.Substring(0, -1));
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => bar.Substring(3, 1));
            });
        }

        [TestMethod]
        public void StringSegment_Substring_Int32_Int32()
        {
            foreach (var startIndex in Enumerable.Range(-1, 8))
            {
                foreach (var count in Enumerable.Range(-1, 8))
                {
                    AssertWithVariations(
                        new[] { "", "b", "ba", "bar", "foobar" },
                        s => s.Substring(startIndex, count),
                        s => s.Substring(startIndex, count)
                    );
                }
            }
        }

        [TestMethod]
        public void StringSegment_ToCharArray_ArgumentChecking()
        {
            Assert.ThrowsException<NullReferenceException>(() => default(StringSegment).ToCharArray());
        }

        [TestMethod]
        public void StringSegment_ToCharArray()
        {
            AssertWithVariations(
                new[]
                {
                    "",
                    "bar",
                    "foobar",
                },
                s => s.ToCharArray(),
                s => s.ToCharArray(),
                (e, a) => e.SequenceEqual(a)
            );
        }

        [TestMethod]
        public void StringSegment_ToCharArray_Int32_Int32_ArgumentChecking()
        {
            Assert.ThrowsException<NullReferenceException>(() => default(StringSegment).ToCharArray(0, 1));

            WithVariations("bar", bar =>
            {
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => bar.ToCharArray(-1, 1));
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => bar.ToCharArray(0, -1));
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => bar.ToCharArray(0, 4));
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => bar.ToCharArray(3, 1));
            });
        }

        [TestMethod]
        public void StringSegment_ToCharArray_Int32_Int32()
        {
            foreach (var startIndex in Enumerable.Range(-1, 6))
            {
                foreach (var count in Enumerable.Range(-1, 6))
                {
                    AssertWithVariations(
                        new[]
                        {
                            "",
                            "bar",
                            "foobar",
                        },
                        s => s.ToCharArray(startIndex, count),
                        s => s.ToCharArray(startIndex, count),
                        (e, a) => e.SequenceEqual(a)
                    );
                }
            }
        }

        [TestMethod]
        public void StringSegment_ToLower_ArgumentChecking()
        {
            Assert.ThrowsException<NullReferenceException>(() => default(StringSegment).ToLower());
        }

        [TestMethod]
        public void StringSegment_ToLower()
        {
            AssertWithVariations(
                new[] { "", "bar", "Bar", "BAR", "bAR" },
                s => s.ToLower(),
                v => v.ToLower()
            );
        }

        [TestMethod]
        public void StringSegment_ToLower_CultureInfo_ArgumentChecking()
        {
            Assert.ThrowsException<NullReferenceException>(() => default(StringSegment).ToLower());

            WithVariations("bar", bar =>
            {
                Assert.ThrowsException<ArgumentNullException>(() => bar.ToLower(default));
            });
        }

        [TestMethod]
        public void StringSegment_ToLower_CultureInfo()
        {
            AssertWithVariations(
                new[] { "", "bar", "Bar", "BAR", "bAR" },
                s => s.ToLower(CultureInfo.InvariantCulture),
                v => v.ToLower(CultureInfo.InvariantCulture)
            );
        }

        [TestMethod]
        public void StringSegment_ToLowerInvariant_ArgumentChecking()
        {
            Assert.ThrowsException<NullReferenceException>(() => default(StringSegment).ToLowerInvariant());
        }

        [TestMethod]
        public void StringSegment_ToLowerInvariant()
        {
            AssertWithVariations(
                new[] { "", "bar", "Bar", "BAR", "bAR" },
                s => s.ToLowerInvariant(),
                v => v.ToLowerInvariant()
            );
        }

        [TestMethod]
        public void StringSegment_ToString()
        {
            WithVariations("bar", bar =>
            {
                Assert.AreEqual("bar", bar.ToString());
            });

            Assert.AreEqual("bar", new StringSegment("xybarqux", 2, 3).ToString());
        }

        [TestMethod]
        public void StringSegment_ToString_IFormatProvider_ArgumentChecking()
        {
            // NB: No known invalid inputs.

            Assert.IsTrue(true);
        }

        [TestMethod]
        public void StringSegment_ToString_IFormatProvider()
        {
            WithVariations("bar", bar =>
            {
                Assert.AreEqual("bar", bar.ToString(CultureInfo.InvariantCulture));
            });

            Assert.AreEqual("bar", new StringSegment("xybarqux", 2, 3).ToString(CultureInfo.InvariantCulture));
        }

        [TestMethod]
        public void StringSegment_ToUpper_ArgumentChecking()
        {
            Assert.ThrowsException<NullReferenceException>(() => default(StringSegment).ToUpper());
        }

        [TestMethod]
        public void StringSegment_ToUpper()
        {
            AssertWithVariations(
                new[] { "", "bar", "Bar", "BAR", "bAR" },
                s => s.ToUpper(),
                v => v.ToUpper()
            );
        }

        [TestMethod]
        public void StringSegment_ToUpper_CultureInfo_ArgumentChecking()
        {
            Assert.ThrowsException<NullReferenceException>(() => default(StringSegment).ToUpper());

            WithVariations("bar", bar =>
            {
                Assert.ThrowsException<ArgumentNullException>(() => bar.ToUpper(default));
            });
        }

        [TestMethod]
        public void StringSegment_ToUpper_CultureInfo()
        {
            AssertWithVariations(
                new[] { "", "bar", "Bar", "BAR", "bAR" },
                s => s.ToUpper(CultureInfo.InvariantCulture),
                v => v.ToUpper(CultureInfo.InvariantCulture)
            );
        }

        [TestMethod]
        public void StringSegment_ToUpperInvariant_ArgumentChecking()
        {
            Assert.ThrowsException<NullReferenceException>(() => default(StringSegment).ToUpperInvariant());
        }

        [TestMethod]
        public void StringSegment_ToUpperInvariant()
        {
            AssertWithVariations(
                new[] { "", "bar", "Bar", "BAR", "bAR" },
                s => s.ToUpperInvariant(),
                v => v.ToUpperInvariant()
            );
        }

        [TestMethod]
        public void StringSegment_Trim_ArgumentChecking()
        {
            Assert.ThrowsException<NullReferenceException>(() => default(StringSegment).Trim());
        }

        [TestMethod]
        public void StringSegment_Trim()
        {
            AssertWithVariations(
                new[]
                {
                    "",
                    " ",
                    "  ",
                    "bar",
                    "qubar ",
                    "quxbar  ",
                    " bar",
                    "  qubar",
                    "   quxbar",
                    " foobar ",
                    " foobar  ",
                    " foobar   ",
                    "  foobar ",
                    "   foobar ",
                    "   foobar  ",
                    "   foobar   ",
                },
                s => s.Trim(),
                v => v.Trim()
            );
        }

        [TestMethod]
        public void StringSegment_Trim_CharArray_ArgumentChecking()
        {
            Assert.ThrowsException<NullReferenceException>(() => default(StringSegment).Trim(' '));
        }

        [TestMethod]
        public void StringSegment_Trim_CharArray()
        {
            AssertWithVariations(
                new[]
                {
                    "",
                    " ",
                    " _",
                    "bar",
                    "qubar ",
                    "quxbar_ ",
                    " bar",
                    " _qubar",
                    " _ quxbar",
                    " foobar ",
                    " foobar_ ",
                    " foobar__ ",
                    "  foobar ",
                    "_  foobar ",
                    " _ foobar  ",
                    "   foobar _ ",
                },
                s => s.Trim(' ', '_'),
                v => v.Trim(' ', '_')
            );
        }

        [TestMethod]
        public void StringSegment_Trim_CharArray_Null()
        {
            AssertWithVariations(
                new[]
                {
                    "",
                    " ",
                    " _",
                    "bar",
                    "qubar ",
                    "quxbar_ ",
                    " bar",
                    " _qubar",
                    " _ quxbar",
                    " foobar ",
                    " foobar_ ",
                    " foobar__ ",
                    "  foobar ",
                    "_  foobar ",
                    " _ foobar  ",
                    "   foobar _ ",
                },
                s => s.Trim(trimChars: null),
                v => v.Trim(trimChars: null)
            );
        }

        [TestMethod]
        public void StringSegment_Trim_CharArray_Empty()
        {
            AssertWithVariations(
                new[]
                {
                    "",
                    " ",
                    " _",
                    "bar",
                    "qubar ",
                    "quxbar_ ",
                    " bar",
                    " _qubar",
                    " _ quxbar",
                    " foobar ",
                    " foobar_ ",
                    " foobar__ ",
                    "  foobar ",
                    "_  foobar ",
                    " _ foobar  ",
                    "   foobar _ ",
                },
                s => s.Trim(Array.Empty<char>()),
                v => v.Trim(Array.Empty<char>())
            );
        }

        [TestMethod]
        public void StringSegment_TrimEnd_CharArray_ArgumentChecking()
        {
            Assert.ThrowsException<NullReferenceException>(() => default(StringSegment).TrimEnd(' '));
        }

        [TestMethod]
        public void StringSegment_TrimEnd_CharArray()
        {
            AssertWithVariations(
                new[]
                {
                    "",
                    " ",
                    " _",
                    "bar",
                    "qubar ",
                    "quxbar_ ",
                    " bar",
                    " _qubar",
                    " _ quxbar",
                    " foobar ",
                    " foobar_ ",
                    " foobar__ ",
                    "  foobar ",
                    "_  foobar ",
                    " _ foobar  ",
                    "   foobar _ ",
                },
                s => s.TrimEnd(' ', '_'),
                v => v.TrimEnd(' ', '_')
            );
        }

        [TestMethod]
        public void StringSegment_TrimEnd_CharArray_Null()
        {
            AssertWithVariations(
                new[]
                {
                    "",
                    " ",
                    " _",
                    "bar",
                    "qubar ",
                    "quxbar_ ",
                    " bar",
                    " _qubar",
                    " _ quxbar",
                    " foobar ",
                    " foobar_ ",
                    " foobar__ ",
                    "  foobar ",
                    "_  foobar ",
                    " _ foobar  ",
                    "   foobar _ ",
                },
                s => s.TrimEnd(trimChars: null),
                v => v.TrimEnd(trimChars: null)
            );
        }

        [TestMethod]
        public void StringSegment_TrimEnd_CharArray_Empty()
        {
            AssertWithVariations(
                new[]
                {
                    "",
                    " ",
                    " _",
                    "bar",
                    "qubar ",
                    "quxbar_ ",
                    " bar",
                    " _qubar",
                    " _ quxbar",
                    " foobar ",
                    " foobar_ ",
                    " foobar__ ",
                    "  foobar ",
                    "_  foobar ",
                    " _ foobar  ",
                    "   foobar _ ",
                },
                s => s.TrimEnd(Array.Empty<char>()),
                v => v.TrimEnd(Array.Empty<char>())
            );
        }

        [TestMethod]
        public void StringSegment_TrimStart_CharArray_ArgumentChecking()
        {
            Assert.ThrowsException<NullReferenceException>(() => default(StringSegment).TrimStart(' '));
        }

        [TestMethod]
        public void StringSegment_TrimStart_CharArray()
        {
            AssertWithVariations(
                new[]
                {
                    "",
                    " ",
                    " _",
                    "bar",
                    "qubar ",
                    "quxbar_ ",
                    " bar",
                    " _qubar",
                    " _ quxbar",
                    " foobar ",
                    " foobar_ ",
                    " foobar__ ",
                    "  foobar ",
                    "_  foobar ",
                    " _ foobar  ",
                    "   foobar _ ",
                },
                s => s.TrimStart(' ', '_'),
                v => v.TrimStart(' ', '_')
            );
        }

        [TestMethod]
        public void StringSegment_TrimStart_CharArray_Null()
        {
            AssertWithVariations(
                new[]
                {
                    "",
                    " ",
                    " _",
                    "bar",
                    "qubar ",
                    "quxbar_ ",
                    " bar",
                    " _qubar",
                    " _ quxbar",
                    " foobar ",
                    " foobar_ ",
                    " foobar__ ",
                    "  foobar ",
                    "_  foobar ",
                    " _ foobar  ",
                    "   foobar _ ",
                },
                s => s.TrimStart(trimChars: null),
                v => v.TrimStart(trimChars: null)
            );
        }

        [TestMethod]
        public void StringSegment_TrimStart_CharArray_Empty()
        {
            AssertWithVariations(
                new[]
                {
                    "",
                    " ",
                    " _",
                    "bar",
                    "qubar ",
                    "quxbar_ ",
                    " bar",
                    " _qubar",
                    " _ quxbar",
                    " foobar ",
                    " foobar_ ",
                    " foobar__ ",
                    "  foobar ",
                    "_  foobar ",
                    " _ foobar  ",
                    "   foobar _ ",
                },
                s => s.TrimStart(Array.Empty<char>()),
                v => v.TrimStart(Array.Empty<char>())
            );
        }


        //=======================================\\
        //   _    _      _                       \\
        //  | |  | |    | |                      \\
        //  | |__| | ___| |_ __   ___ _ __ ___   \\
        //  |  __  |/ _ \ | '_ \ / _ \ '__/ __|  \\
        //  | |  | |  __/ | |_) |  __/ |  \__ \  \\
        //  |_|  |_|\___|_| .__/ \___|_|  |___/  \\
        //                | |                    \\
        //                |_|                    \\
        //                                       \\
        //=======================================\\

        private static void WithVariations(string value, Action<StringSegment> test)
        {
            WithVariations(value, (_, s) => test(s));
        }

        private static void WithVariations(string value, Action<string, StringSegment> test)
        {
            WithVariations(new[] { value }, test);
        }

        private static void WithVariations(string[] values, Action<string, StringSegment> test)
        {
            foreach (var value in values)
            {
                if (value == null)
                {
                    test(value, new StringSegment());
                    continue;
                }

                test(value, value);

                var len = value.Length;

                test(value, new StringSegment("!" + value, 1, len));
                test(value, new StringSegment("!@" + value, 2, len));

                test(value, new StringSegment(value + "#", 0, len));
                test(value, new StringSegment(value + "$#", 0, len));

                test(value, new StringSegment("!" + value + "#", 1, len));
                test(value, new StringSegment("!" + value + "$#", 1, len));
                test(value, new StringSegment("!@" + value + "#", 2, len));
                test(value, new StringSegment("!@" + value + "$#", 2, len));
            }
        }

        private static void WithVariations(string value1, string value2, Action<StringSegment, StringSegment> test)
        {
            WithVariations(value1, value2, (_, val1, _, val2) => test(val1, val2));
        }

        private static void WithVariations(string value1, string value2, Action<string, StringSegment, string, StringSegment> test)
        {
            WithVariations(value1, val1 => WithVariations(value2, val2 => test(value1, val1, value2, val2)));
        }

        private static void WithVariations(string[] values1, string[] values2, Action<string, StringSegment, string, StringSegment> test)
        {
            WithVariations(values1, (value1, val1) => WithVariations(values2, (value2, val2) => test(value1, val1, value2, val2)));
        }

        private static void WithVariations(string[] values1, string[] values2, string[] values3, Action<string, StringSegment, string, StringSegment, string, StringSegment> test)
        {
            WithVariations(values1, (value1, val1) => WithVariations(values2, (value2, val2) => WithVariations(values3, (value3, val3) => test(value1, val1, value2, val2, value3, val3))));
        }

        private static void WithVariations(string[] values1, string[] values2, string[] values3, string[] values4, Action<string, StringSegment, string, StringSegment, string, StringSegment, string, StringSegment> test)
        {
            WithVariations(values1, (value1, val1) => WithVariations(values2, (value2, val2) => WithVariations(values3, (value3, val3) => WithVariations(values4, (value4, val4) => test(value1, val1, value2, val2, value3, val3, value4, val4)))));
        }

        private static void AssertWithVariations<T>(string[] values, Func<string, T> expected, Func<StringSegment, T> actual)
        {
            AssertWithVariations<T, T>(values, expected, actual, (l, r) => EqualityComparer<T>.Default.Equals(l, r));
        }

        private static void AssertWithVariations<T1, T2>(string[] values, Func<string, T1> expected, Func<StringSegment, T2> actual, Func<T1, T2, bool> equal)
        {
            WithVariations(values, (v, s) =>
            {
                var e = default(T1);
                var a = default(T2);

                var ee = default(Exception);
                var ae = default(Exception);

                try
                {
                    e = expected(v);
                }
                catch (Exception ex)
                {
                    ee = ex;
                }

                try
                {
                    a = actual(s);
                }
                catch (Exception ex)
                {
                    ae = ex;
                }

                if (!AssertExceptions(ee, ae))
                {
                    Assert.IsTrue(equal(e, a), string.Format("Failed for input '{0}': '{1}' != '{2}'", s, e, a));
                }
            });
        }

        private static void AssertWithVariations<T>(string[] values1, string[] values2, Func<string, string, T> expected, Func<StringSegment, StringSegment, T> actual)
        {
            AssertWithVariations<T, T>(values1, values2, expected, actual, (l, r) => EqualityComparer<T>.Default.Equals(l, r));
        }

        private static void AssertWithVariations<T1, T2>(string[] values1, string[] values2, Func<string, string, T1> expected, Func<StringSegment, StringSegment, T2> actual, Func<T1, T2, bool> equal)
        {
            WithVariations(values1, values2, (v1, s1, v2, s2) =>
            {
                var e = default(T1);
                var a = default(T2);

                var ee = default(Exception);
                var ae = default(Exception);

                try
                {
                    e = expected(v1, v2);
                }
                catch (Exception ex)
                {
                    ee = ex;
                }

                try
                {
                    a = actual(s1, s2);
                }
                catch (Exception ex)
                {
                    ae = ex;
                }

                if (!AssertExceptions(ee, ae))
                {
                    Assert.IsTrue(equal(e, a), string.Format("Failed for input ('{0}', '{1}'): '{2}' != '{3}'", s1, s2, e, a));
                }
            });
        }

        private static void AssertWithVariations<T>(string[] values1, string[] values2, string[] values3, Func<string, string, string, T> expected, Func<StringSegment, StringSegment, StringSegment, T> actual)
        {
            AssertWithVariations<T, T>(values1, values2, values3, expected, actual, (l, r) => EqualityComparer<T>.Default.Equals(l, r));
        }

        private static void AssertWithVariations<T1, T2>(string[] values1, string[] values2, string[] values3, Func<string, string, string, T1> expected, Func<StringSegment, StringSegment, StringSegment, T2> actual, Func<T1, T2, bool> equal)
        {
            WithVariations(values1, values2, values3, (v1, s1, v2, s2, v3, s3) =>
            {
                var e = default(T1);
                var a = default(T2);

                var ee = default(Exception);
                var ae = default(Exception);

                try
                {
                    e = expected(v1, v2, v3);
                }
                catch (Exception ex)
                {
                    ee = ex;
                }

                try
                {
                    a = actual(s1, s2, s3);
                }
                catch (Exception ex)
                {
                    ae = ex;
                }

                if (!AssertExceptions(ee, ae))
                {
                    Assert.IsTrue(equal(e, a), string.Format("Failed for input ('{0}', '{1}', '{2}'): '{3}' != '{4}'", s1, s2, s3, e, a));
                }
            });
        }

        private static void AssertWithVariations<T>(string[] values1, string[] values2, string[] values3, string[] values4, Func<string, string, string, string, T> expected, Func<StringSegment, StringSegment, StringSegment, StringSegment, T> actual)
        {
            AssertWithVariations<T, T>(values1, values2, values3, values4, expected, actual, (l, r) => EqualityComparer<T>.Default.Equals(l, r));
        }

        private static void AssertWithVariations<T1, T2>(string[] values1, string[] values2, string[] values3, string[] values4, Func<string, string, string, string, T1> expected, Func<StringSegment, StringSegment, StringSegment, StringSegment, T2> actual, Func<T1, T2, bool> equal)
        {
            WithVariations(values1, values2, values3, values4, (v1, s1, v2, s2, v3, s3, v4, s4) =>
            {
                var e = default(T1);
                var a = default(T2);

                var ee = default(Exception);
                var ae = default(Exception);

                try
                {
                    e = expected(v1, v2, v3, v4);
                }
                catch (Exception ex)
                {
                    ee = ex;
                }

                try
                {
                    a = actual(s1, s2, s3, s4);
                }
                catch (Exception ex)
                {
                    ae = ex;
                }

                if (!AssertExceptions(ee, ae))
                {
                    Assert.IsTrue(equal(e, a), string.Format("Failed for input ('{0}', '{1}', '{2}', '{3}'): '{4}' != '{5}'", s1, s2, s3, s4, e, a));
                }
            });
        }

        private static bool AssertExceptions(Exception expected, Exception actual)
        {
            Assert.AreEqual(expected?.GetType(), actual?.GetType(), string.Format("Exceptions don't match: '{0}' != '{1}'", expected, actual));

            return expected != null || actual != null;
        }
    }
}
