// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// PS - February 2015 - Created this file.
//

using System;
using System.Collections.Generic;
using System.Linq.CompilerServices;
using System.Linq.CompilerServices.Optimizers;
using System.Linq.Expressions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.System.Linq.CompilerServices.Optimizers
{
    [TestClass]
    public class QueryExpressionToStringTests
    {
        [TestMethod]
        public void QueryExpressionToStringTests_Simple()
        {
            var visitor = new QueryDebugger();

            var type = typeof(int);
            var lambdaAbstraction = DefaultQueryExpressionFactory.Instance.LambdaAbstraction(Expression.Lambda(Expression.Default(typeof(IEnumerable<int>))));
            var lambdaAbstractionPredicate = DefaultQueryExpressionFactory.Instance.LambdaAbstraction((Expression<Func<Func<int, bool>>>)(() => _ => true));
            var lambdaAbstractionSelector = DefaultQueryExpressionFactory.Instance.LambdaAbstraction((Expression<Func<Func<int, int>>>)(() => _ => _));
#pragma warning disable IDE0004 // Remove Unnecessary Cast. (Only unnecessary on C# 10 or later.)
            var lambdaAbstractionCount = DefaultQueryExpressionFactory.Instance.LambdaAbstraction((Expression<Func<int>>)(() => 42));
#pragma warning restore IDE0004 // Remove Unnecessary Cast

            var monadMember = (MonadMember)new MonadAbstraction(type, lambdaAbstraction);

            {
                var op = new FirstOperator(type, monadMember);
                Assert.AreEqual("@First(@MonadAbstraction(@LambdaAbstraction(() => default(IEnumerable`1))))", visitor.Visit(op));
            }

            {
                var op = new FirstPredicateOperator(type, monadMember, lambdaAbstractionPredicate);
                Assert.AreEqual("@First(@MonadAbstraction(@LambdaAbstraction(() => default(IEnumerable`1))), @LambdaAbstraction(() => _ => True))", visitor.Visit(op));
            }

            {
                var op = new SelectOperator(type, type, monadMember, lambdaAbstractionSelector);
                Assert.AreEqual("@Select(@MonadAbstraction(@LambdaAbstraction(() => default(IEnumerable`1))), @LambdaAbstraction(() => _ => _))", visitor.Visit(op));
            }

            {
                var op = new TakeOperator(type, monadMember, lambdaAbstractionCount);
                Assert.AreEqual("@Take(@MonadAbstraction(@LambdaAbstraction(() => default(IEnumerable`1))), @LambdaAbstraction(() => 42))", visitor.Visit(op));
            }

            {
                var op = new WhereOperator(type, monadMember, lambdaAbstractionPredicate);
                Assert.AreEqual("@Where(@MonadAbstraction(@LambdaAbstraction(() => default(IEnumerable`1))), @LambdaAbstraction(() => _ => True))", visitor.Visit(op));
            }

            {
                var ma = new MonadAbstraction(type, lambdaAbstraction);
                Assert.AreEqual("@MonadAbstraction(@LambdaAbstraction(() => default(IEnumerable`1)))", visitor.Visit(ma));
            }

            {
                var lambda = Expression.Lambda(Expression.Default(typeof(int)), Expression.Parameter(typeof(IEnumerable<int>), "p"));
                var parameters = new QueryTree[] { monadMember }.ToReadOnly();

                var la = new LambdaAbstraction(lambda, parameters);
                Assert.AreEqual("@LambdaAbstraction(p => default(Int32), @MonadAbstraction(@LambdaAbstraction(() => default(IEnumerable`1))))", visitor.Visit(la));
            }
        }
    }
}
