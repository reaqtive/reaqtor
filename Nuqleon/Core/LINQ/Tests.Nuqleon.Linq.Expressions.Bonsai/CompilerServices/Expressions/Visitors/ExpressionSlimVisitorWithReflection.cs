// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD, ER - October 2013 - Created this file.
//

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.CompilerServices.Bonsai;
using System.Linq.Expressions;
using System.Reflection;

namespace Tests.System.Linq.Expressions.Bonsai.CompilerServices
{
    [TestClass]
    public class ExpressionSlimVisitorWithReflectionTests : TestBase
    {
        [TestMethod]
        public void ExpressionVisitorWithReflection_Checks()
        {
            new MyVisitor().Do();
        }

        private sealed class MyVisitor : ExpressionSlimVisitorWithReflection
        {
            public void Do()
            {
                Assert.ThrowsException<ArgumentException>(() => base.MakeNewArray(ExpressionType.Add, typeof(int).ToTypeSlim(), expressions: null));
                Assert.ThrowsException<ArgumentException>(() => base.MakeTypeBinary(ExpressionType.Add, expression: null, typeof(int).ToTypeSlim()));
            }
        }

        [TestMethod]
        public void ExpressionVisitorWithReflection_Invariant()
        {
            var visitor = new ExpressionSlimVisitorWithReflection();

            var b = Expression.Label(typeof(int));
            var c = Expression.Label();

            var p1 = Expression.Parameter(typeof(int));
            var p2 = Expression.Parameter(typeof(int));

            var blk1 =
                Expression.Block(
                    new[] { p1 },
                    p1
                );

            var blk2 =
                Expression.Block(
                    Expression.Constant(true)
                );

            var blk3 =
                Expression.Block(
                    new[] { p1 },
                    p1,
                    Expression.Constant(true)
                );

            var blk4 =
                Expression.Block(
                    new[] { p1, p2 },
                    p1,
                    p2
                );

            var ex = Expression.Parameter(typeof(Exception));
            var tcf =
                Expression.TryCatchFinally(
                    Expression.Constant(1),
                    Expression.Constant(2),
                    Expression.Catch(
                        ex,
                        Expression.Throw(ex, typeof(int))
                    )
                );

            foreach (var e in new Expression[]
            {
                (Expression<Func<int, int>>)(x => Math.Abs(x)),
                (Expression<Func<string, int>>)(s => s.Length),
                (Expression<Func<int, string>>)(x => new string('x', x)),
                (Expression<Func<object, bool>>)(o => o is string),
                (from x in new[] { 2, 3, 5 }.AsQueryable() where x > 0 let y = x * x where y > 0 select x + y).Expression,
                Expression.MakeIndex(Expression.Parameter(typeof(Dictionary<int, int>)), typeof(Dictionary<int, int>).GetProperty("Item"), new[] { Expression.Constant(1) }),
                blk1,
                blk2,
                blk3,
                blk4,
                tcf,
            })
            {
                var slim = e.ToExpressionSlim();
                Assert.AreSame(slim, visitor.Visit(slim));
            }
        }
    }
}
