// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 03/30/2016 - Created StringSegment functionality.
//

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;

namespace Tests
{
    [TestClass]
    public class StringBuilderStringSegmentExtensionsTests
    {
        [TestMethod]
        public void StringSegment_StringBuilder_Append_StringSegment_ArgumentChecking()
        {
            var bar = new StringSegment("bar");

            Assert.ThrowsException<ArgumentNullException>(() => StringBuilderStringSegmentExtensions.Append(builder: null, bar));
        }

        [TestMethod]
        public void StringSegment_StringBuilder_Append_StringSegment1()
        {
            var sb = new StringBuilder();

            Assert.AreSame(sb, sb.Append("foo"));
            Assert.AreSame(sb, StringBuilderStringSegmentExtensions.Append(sb, new StringSegment("zzbarzz", 2, 3)));
            Assert.AreSame(sb, sb.Append("qux"));

            Assert.AreEqual("foobarqux", sb.ToString());
        }

        [TestMethod]
        public void StringSegment_StringBuilder_Append_StringSegment2()
        {
            var sb = new StringBuilder();

            Assert.AreSame(sb, sb.Append("foo"));
            Assert.AreSame(sb, StringBuilderStringSegmentExtensions.Append(sb, default));
            Assert.AreSame(sb, sb.Append("qux"));

            Assert.AreEqual("fooqux", sb.ToString());
        }

        [TestMethod]
        public void StringSegment_StringBuilder_Append_StringSegment3()
        {
            var sb = new StringBuilder();

            Assert.AreSame(sb, sb.Append("foo"));
            Assert.AreSame(sb, StringBuilderStringSegmentExtensions.Append(sb, new StringSegment("zzz", 1, 0)));
            Assert.AreSame(sb, sb.Append("qux"));

            Assert.AreEqual("fooqux", sb.ToString());
        }

        [TestMethod]
        public void StringSegment_StringBuilder_Append_StringSegment_Int32_Int32_ArgumentChecking()
        {
            var sb = new StringBuilder();
            var bar = new StringSegment("bar");

            Assert.ThrowsException<ArgumentNullException>(() => StringBuilderStringSegmentExtensions.Append(builder: null, bar, 0, 1));

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => StringBuilderStringSegmentExtensions.Append(sb, bar, -1, 1));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => StringBuilderStringSegmentExtensions.Append(sb, bar, 0, -1));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => StringBuilderStringSegmentExtensions.Append(sb, bar, 3, 1));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => StringBuilderStringSegmentExtensions.Append(sb, bar, 0, 4));

            Assert.ThrowsException<ArgumentNullException>(() => StringBuilderStringSegmentExtensions.Append(sb, default, 0, 1));
            Assert.ThrowsException<ArgumentNullException>(() => StringBuilderStringSegmentExtensions.Append(sb, default, 1, 0));
        }

        [TestMethod]
        public void StringSegment_StringBuilder_Append_StringSegment_Int32_Int32_1()
        {
            var sb = new StringBuilder();

            Assert.AreSame(sb, sb.Append("foo"));
            Assert.AreSame(sb, sb.Append(new StringSegment("zzbarzz", 2, 3), 0, 3));
            Assert.AreSame(sb, sb.Append("qux"));

            Assert.AreEqual("foobarqux", sb.ToString());
        }

        [TestMethod]
        public void StringSegment_StringBuilder_Append_StringSegment_Int32_Int32_2()
        {
            var sb = new StringBuilder();

            Assert.AreSame(sb, sb.Append("foo"));
            Assert.AreSame(sb, sb.Append(new StringSegment("zzbarzz", 2, 3), 0, 2));
            Assert.AreSame(sb, sb.Append("qux"));

            Assert.AreEqual("foobaqux", sb.ToString());
        }

        [TestMethod]
        public void StringSegment_StringBuilder_Append_StringSegment_Int32_Int32_3()
        {
            var sb = new StringBuilder();

            Assert.AreSame(sb, sb.Append("foo"));
            Assert.AreSame(sb, sb.Append(new StringSegment("zzbarzz", 2, 3), 1, 2));
            Assert.AreSame(sb, sb.Append("qux"));

            Assert.AreEqual("fooarqux", sb.ToString());
        }

        [TestMethod]
        public void StringSegment_StringBuilder_Append_StringSegment_Int32_Int32_4()
        {
            var sb = new StringBuilder();

            Assert.AreSame(sb, sb.Append("foo"));
            Assert.AreSame(sb, sb.Append(new StringSegment("zzbarzz", 2, 3), 2, 0));
            Assert.AreSame(sb, sb.Append("qux"));

            Assert.AreEqual("fooqux", sb.ToString());
        }

        [TestMethod]
        public void StringSegment_StringBuilder_Append_StringSegment_Int32_Int32_5()
        {
            var sb = new StringBuilder();

            Assert.AreSame(sb, sb.Append("foo"));
            Assert.AreSame(sb, sb.Append(StringSegment.Empty, 0, 0));
            Assert.AreSame(sb, sb.Append("qux"));

            Assert.AreEqual("fooqux", sb.ToString());
        }

        [TestMethod]
        public void StringSegment_StringBuilder_Append_StringSegment_Int32_Int32_6()
        {
            var sb = new StringBuilder();

            Assert.AreSame(sb, sb.Append("foo"));
            Assert.AreSame(sb, sb.Append(default(StringSegment), 0, 0));
            Assert.AreSame(sb, sb.Append("qux"));

            Assert.AreEqual("fooqux", sb.ToString());
        }

        [TestMethod]
        public void StringSegment_StringBuilder_Append_StringSegment_Int32_Int32_7()
        {
            var sb = new StringBuilder();

            Assert.AreSame(sb, sb.Append("foo"));
            Assert.AreSame(sb, sb.Append(new StringSegment("bar"), 1, 0));
            Assert.AreSame(sb, sb.Append("qux"));

            Assert.AreEqual("fooqux", sb.ToString());
        }

        [TestMethod]
        public void StringSegment_StringBuilder_AppendLine_StringSegment_ArgumentChecking()
        {
            var bar = new StringSegment("bar");

            Assert.ThrowsException<ArgumentNullException>(() => StringBuilderStringSegmentExtensions.AppendLine(builder: null, bar));
        }

        [TestMethod]
        public void StringSegment_StringBuilder_AppendLine_StringSegment()
        {
            var sb = new StringBuilder();

            Assert.AreSame(sb, sb.AppendLine("foo"));
            Assert.AreSame(sb, sb.AppendLine(new StringSegment("zzbarzz", 2, 3)));
            Assert.AreSame(sb, sb.AppendLine("qux"));

            Assert.AreEqual("foo\r\nbar\r\nqux\r\n", sb.ToString());
        }
    }
}
