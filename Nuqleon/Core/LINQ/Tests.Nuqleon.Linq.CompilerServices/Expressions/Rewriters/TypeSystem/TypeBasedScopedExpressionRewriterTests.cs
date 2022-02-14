// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - November 2013 - Created this file.
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
    public class TypeBasedExpressionRewriterTests
    {
        [TestMethod]
        public void TypeBasedExpressionRewriter_ArgumentChecking()
        {
            var rewriter = new TypeBasedExpressionRewriter();
            Assert.ThrowsException<ArgumentNullException>(() => rewriter.Add(type: null, e => e));
            Assert.ThrowsException<ArgumentNullException>(() => rewriter.AddDefinition(type: null, e => e));
            Assert.ThrowsException<ArgumentNullException>(() => rewriter.Add(typeof(int), rewriter: null));
            Assert.ThrowsException<ArgumentNullException>(() => rewriter.AddDefinition(typeof(List<>), rewriter: null));
            Assert.ThrowsException<ArgumentException>(() => rewriter.AddDefinition(typeof(int), e => e));
        }

        [TestMethod]
        public void TypeBasedExpressionRewriter_NoChange()
        {
            foreach (var e in new Expression[]
            {
                Expression.Constant(1L, typeof(long)),
#pragma warning disable IDE0004 // Remove Unnecessary Cast. (Only unnecessary on C# 10 or later.)
                (Expression<Func<DateTimeOffset>>)(() => DateTimeOffset.Now),
#pragma warning restore IDE0004
                (Expression<Func<IEnumerable<long>, IEnumerable<string>>>)(xs => from x in xs where x % 2 == 0 select x.ToString())
            })
            {
                var rewriter = new TestRewriter();
                Assert.AreSame(e, rewriter.Visit(e));
            }
        }

        [TestMethod]
        public void TypeBasedExpressionRewriter_Simple()
        {
            var f = new Foo { Value = 42 };

            foreach (var e in new Dictionary<Expression, Expression>
            {
                { Expression.Constant(f, typeof(Foo)), Expression.Convert(Expression.Constant(f, typeof(Foo)), typeof(Bar)) },
                { (Expression<Func<Foo, Foo>>)(x => x.Other), (Expression<Func<Foo, Foo>>)(x => (Bar)((Bar)x).Other) },
                { (Expression<Func<Foo, Foo>>)(x => x), (Expression<Func<Foo, Foo>>)(x => (Bar)x) },
#pragma warning disable IDE0004 // Remove Unnecessary Cast. (Keeping inside expression tree.)
                { (Expression<Func<List<int>, IEnumerable<int>>>)(x => (IEnumerable<int>)x),  (Expression<Func<List<int>, IEnumerable<int>>>)(x => (IEnumerable<int>)(IEnumerable<int>)x) },
#pragma warning restore IDE0004
            })
            {
                var comparer = new ExpressionEqualityComparer();
                var rewriter = new TestRewriter();
                Assert.IsTrue(comparer.Equals(rewriter.Visit(e.Key), e.Value));
            }
        }

        private class TestRewriter : TypeBasedExpressionRewriter
        {
            public TestRewriter()
            {
                Add(typeof(Foo), RewriteFoo);
                AddDefinition(typeof(List<>), RewriteList);
            }

            protected override Expression VisitLambdaCore<T>(Expression<T> node)
            {
                var body = Visit(node.Body);
                return node.Update(body, node.Parameters);
            }

            private Expression RewriteFoo(Expression e)
            {
                return Expression.Convert(e, typeof(Bar));
            }

            private Expression RewriteList(Expression e)
            {
                var oldElemType = e.Type.GenericTypeArguments[0];
                var newType = typeof(IEnumerable<>).MakeGenericType(oldElemType);
                return Expression.Convert(e, newType);
            }
        }

        private class Foo
        {
            public int Value { get; set; }
            public Foo Other { get; set; }
        }

        private class Bar : Foo
        {
        }
    }
}
