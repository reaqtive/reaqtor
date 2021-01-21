// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.CompilerServices;
using System.Linq.Expressions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.System.Linq.CompilerServices
{
    [TestClass]
    public class ExpressionVisitorWithReflectionTests
    {
        [TestMethod]
        public void ExpressionVisitorWithReflection_Checks()
        {
            new MyVisitor().Do();
        }

        private sealed class MyVisitor : ExpressionVisitorWithReflection
        {
            public void Do()
            {
                Assert.ThrowsException<ArgumentException>(() => base.MakeNewArray(ExpressionType.Add, typeof(int), expressions: null));
                Assert.ThrowsException<ArgumentException>(() => base.MakeTypeBinary(ExpressionType.Add, expression: null, typeof(int)));
            }
        }

        [TestMethod]
        public void ExpressionVisitorWithReflection_Invariant()
        {
            var visitor = new ExpressionVisitorWithReflection();

            foreach (var e in new Expression[]
            {
                (Expression<Func<int, int>>)(x => Math.Abs(x)),
                (Expression<Func<string, int>>)(s => s.Length),
                (Expression<Func<int, string>>)(x => new string('x', x)),
                (Expression<Func<object, bool>>)(o => o is string),
                (from x in new[] { 2, 3, 5 }.AsQueryable() where x > 0 let y = x * x where y > 0 select x + y).Expression,
                Expression.MakeIndex(Expression.Parameter(typeof(Dictionary<int, int>)), typeof(Dictionary<int, int>).GetProperty("Item"), new[] { Expression.Constant(1) }),
            })
            {
                Assert.AreSame(e, visitor.Visit(e));
            }
        }
    }
}
