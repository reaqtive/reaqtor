// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2017 - Created this file.
//

using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.System.Linq.Expressions.Optimizers
{
    [TestClass]
    public partial class LvalExpressionVisitorTests
    {
        [TestMethod]
        public void LvalExpressionVisitor_Basics()
        {
            var visitor = new MyVisitor();

            var x = Expression.Parameter(typeof(int), "x");
            var xs = Expression.Parameter(typeof(int[]), "xs");
            var sb = Expression.Parameter(typeof(StrongBox<int>), "sb");
            var m = typeof(Interlocked).GetMethod(nameof(Interlocked.Increment), new[] { typeof(int).MakeByRefType() });

            var xs0 = Expression.ArrayAccess(xs, Expression.Constant(0));
            var val = Expression.Field(sb, nameof(StrongBox<int>.Value));

            var e =
                Expression.Block(
                    Expression.Assign(x, Expression.Constant(1)),
                    Expression.Assign(xs0, Expression.Constant(1)),
                    Expression.Call(m, val)
                );

            var r = visitor.Visit(e);

            Assert.AreSame(e, r);

            CollectionAssert.AreEqual(
                new Expression[] { x, xs0, xs, val, sb },
                visitor.Lvals
            );
        }

        private sealed class MyVisitor : LvalExpressionVisitor
        {
            public readonly List<Expression> Lvals = new();

            protected override Expression VisitLval(Expression node)
            {
                Lvals.Add(node);

                return base.VisitLval(node);
            }
        }
    }
}
