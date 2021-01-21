// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - May 2014 - Created this file.
//

using Nuqleon.Json.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Tests
{
    [TestClass]
    public class PrettyPrinterTests
    {
        [TestMethod]
        public void PrettyPrinter_ArgumentChecks()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new PrettyPrinter().Visit(node: null));
            Assert.ThrowsException<ArgumentNullException>(() => new PrettyPrinter().VisitObject(node: null));
            Assert.ThrowsException<ArgumentNullException>(() => new PrettyPrinter().VisitArray(node: null));
            Assert.ThrowsException<ArgumentNullException>(() => new PrettyPrinter().VisitConstant(node: null));
        }

        [TestMethod]
        public void PrettyPrinter_Constant_Null()
        {
            AssertPretty(Expression.Null(), "null");
        }

        [TestMethod]
        public void PrettyPrinter_Constant_Int()
        {
            AssertPretty(Expression.Number("42"), "42");
        }

        [TestMethod]
        public void PrettyPrinter_Constant_String()
        {
            AssertPretty(Expression.String("foo"), "\"foo\"");
        }

        [TestMethod]
        public void PrettyPrinter_Array()
        {
            AssertPretty(Expression.Array(Expression.Number("1"), Expression.Number("2")), "[\r\n  1,\r\n  2\r\n]");
        }

        [TestMethod]
        public void PrettyPrinter_Object()
        {
            var d = new Dictionary<string, Expression>
            {
                { "bar", Expression.Number("1") },
                { "foo", Expression.Number("2") },
            };

            AssertPretty(Expression.Object(d), "{\r\n  \"bar\": 1,\r\n  \"foo\": 2\r\n}");
        }

        private static void AssertPretty(Expression e, string s)
        {
            var pp = new PrettyPrinter();
            var js = pp.Visit(e);

            Assert.AreEqual(s, js);
        }
    }
}
