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
    public class ExpressionVisitorTests
    {
        [TestMethod]
        public void ExpressionVisitorOfR_Exceptions()
        {
            var v = new ExpressionVisitor<int>();

            Assert.ThrowsException<ArgumentNullException>(() => v.Visit(node: null));

            Assert.ThrowsException<NotImplementedException>(() => v.VisitObject(node: null));
            Assert.ThrowsException<NotImplementedException>(() => v.VisitArray(node: null));
            Assert.ThrowsException<NotImplementedException>(() => v.VisitConstant(node: null));
        }

        [TestMethod]
        public void ExpressionVisitor_Exceptions()
        {
            var v = new ExpressionVisitor();

            Assert.ThrowsException<ArgumentNullException>(() => v.Visit(node: null));

            Assert.ThrowsException<ArgumentNullException>(() => v.VisitObject(node: null));
            Assert.ThrowsException<ArgumentNullException>(() => v.VisitArray(node: null));
            Assert.ThrowsException<ArgumentNullException>(() => v.VisitConstant(node: null));
        }

        [TestMethod]
        public void ExpressionVisitor_Unchanged()
        {
            var e1 = Expression.Null();
            var e2 = Expression.Number("42");
            var e3 = Expression.String("foo");
            var e4 = Expression.Array(e2, e3);
            var e5 = Expression.Object(new Dictionary<string, Expression> { { "bar", e2 }, { "foo", e3 } });

            var v = new ExpressionVisitor();

            var es = new Expression[]
            {
                e1, e2, e3, e4, e5,
            };

            foreach (var e in es)
            {
                Assert.AreSame(e, v.Visit(e));
            }
        }
    }
}
