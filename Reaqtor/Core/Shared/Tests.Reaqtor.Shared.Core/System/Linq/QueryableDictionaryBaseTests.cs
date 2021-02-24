// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.CompilerServices;
using System.Linq.Expressions;

namespace Tests.Reaqtor.Shared.Core.System.Linq
{
    [TestClass]
    public class QueryableDictionaryBaseTests
    {
        [TestMethod]
        public void QueryableDictionaryBase_ElementType()
        {
            var data = new Dictionary<string, string> { { "foo", "bar" } };
            var qd = new TestQueryableDictionary<string, string>(Expression.Parameter(typeof(string)), data, ex => { });
            Assert.AreEqual(typeof(KeyValuePair<string, string>), qd.ElementType);
        }

        [TestMethod]
        public void QueryableDictionaryBase_Index()
        {
            var data = new Dictionary<string, string> { { "foo", "bar" } };
            AssertExpression(
                data,
                qd => Assert.AreEqual("bar", qd["foo"]),
                qd => qd.SingleOrDefault(item => item.Key == "foo"));

            var x = default(string);
            AssertExpression(
                data,
                qd => Assert.ThrowsException<KeyNotFoundException>(() => x = qd["bar"]),
                qd => qd.SingleOrDefault(item => item.Key == "bar"));
        }

        [TestMethod]
        public void QueryableDictionaryBase_Keys()
        {
            var data = new Dictionary<string, string> { { "foo", "bar" } };
            AssertExpression(
                data,
                qd => Assert.IsTrue(qd.Keys.SequenceEqual(new[] { "foo" })),
                qd => qd.Select(kv => kv.Key));
        }

        [TestMethod]
        public void QueryableDictionaryBase_Values()
        {
            var data = new Dictionary<string, string> { { "foo", "bar" } };
            AssertExpression(
                data,
                qd => Assert.IsTrue(qd.Values.SequenceEqual(new[] { "bar" })),
                qd => qd.Select(kv => kv.Value));
        }

        [TestMethod]
        public void QueryableDictionaryBase_ContainsKey()
        {
            var data = new Dictionary<string, string> { { "foo", "bar" } };
            AssertExpression(
                data,
                qd => Assert.IsTrue(qd.ContainsKey("foo")),
                qd => qd.Any(item => item.Key == "foo"));
        }

        [TestMethod]
        public void QueryableDictionaryBase_Count()
        {
#pragma warning disable IDE0079 // Remove unnecessary suppression (only in .NET 5.0)
#pragma warning disable CA1829 // Use Length/Count property instead of Count() when available (testing query provider)
            var data = new Dictionary<string, string> { { "foo", "bar" } };
            AssertExpression(
                data,
                qd => Assert.AreEqual(1, qd.Count),
                qd => qd.Count());
#pragma warning restore CA1829
#pragma warning restore IDE0079
        }

        [TestMethod]
        public void QueryableDictionaryBase_ReadOnlyDictionary_Keys()
        {
            var data = new Dictionary<string, string> { { "foo", "bar" } };
            AssertExpression(
                data,
                qd => Assert.IsTrue(((IReadOnlyDictionary<string, string>)qd).Keys.SequenceEqual(new[] { "foo" })),
                qd => qd.Select(kv => kv.Key));
        }

        [TestMethod]
        public void QueryableDictionaryBase_ReadOnlyDictionary_Values()
        {
            var data = new Dictionary<string, string> { { "foo", "bar" } };
            AssertExpression(
                data,
                qd => Assert.IsTrue(((IReadOnlyDictionary<string, string>)qd).Values.SequenceEqual(new[] { "bar" })),
                qd => qd.Select(kv => kv.Value));
        }

        private void AssertExpression<TKey, TValue, TResult>(IDictionary<TKey, TValue> data, Action<QueryableDictionaryBase<TKey, TValue>> action, Expression<Func<TestQueryableDictionary<TKey, TValue>, TResult>> expected)
        {
            var thisParameter = Expression.Parameter(typeof(TestQueryableDictionary<TKey, TValue>), "this");

            void assert(Expression expr)
            {
                var invoked = BetaReducer.Reduce(Expression.Invoke(expected, thisParameter));
                Assert.IsTrue(new ExpressionEqualityComparer().Equals(invoked, expr));
            }

            var queryableDictionary = new TestQueryableDictionary<TKey, TValue>(thisParameter, data, assert);
            action(queryableDictionary);
        }

        private sealed class TestQueryableDictionary<TKey, TValue> : QueryableDictionaryBase<TKey, TValue>
        {
            private readonly Action<Expression> _assertExpression;
            private readonly IQueryProvider _queryProvider;
            private readonly Expression _expression;
            private readonly IDictionary<TKey, TValue> _inner;

            public TestQueryableDictionary(Expression expression, IDictionary<TKey, TValue> inner, Action<Expression> assertExpression)
            {
                _expression = expression;
                _inner = inner;
                _assertExpression = assertExpression;
                _queryProvider = new QueryProvider(this);
            }

            public override IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
            {
                return _inner.GetEnumerator();
            }

            public override Expression Expression => _expression;

            public override IQueryProvider Provider => _queryProvider;

            private sealed class QueryProvider : IQueryProvider
            {
                private readonly TestQueryableDictionary<TKey, TValue> _parent;
                private readonly ExpressionVisitor _visitor;

                public QueryProvider(TestQueryableDictionary<TKey, TValue> parent)
                {
                    _parent = parent;
                    _visitor = new Visitor(_parent._expression, Expression.Constant(parent._inner.AsQueryable()));
                }

                public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
                {
                    _parent._assertExpression(expression);
                    return _visitor.Visit(expression).Evaluate<IQueryable<TElement>>();
                }

                public IQueryable CreateQuery(Expression expression)
                {
                    return CreateQuery<object>(expression);
                }

                public TResult Execute<TResult>(Expression expression)
                {
                    _parent._assertExpression(expression);
                    return _visitor.Visit(expression).Evaluate<TResult>();
                }

                public object Execute(Expression expression)
                {
                    return Execute<object>(expression);
                }
            }

            private sealed class Queryable<TElement> : IQueryable<TElement>
            {
                public Queryable(Expression expression, IQueryProvider queryProvider)
                {
                    Expression = expression;
                    Provider = queryProvider;
                }

                public IEnumerator<TElement> GetEnumerator()
                {
                    throw new NotImplementedException();
                }

                IEnumerator IEnumerable.GetEnumerator()
                {
                    return GetEnumerator();
                }

                public Type ElementType => typeof(TElement);

                public Expression Expression { get; private set; }

                public IQueryProvider Provider { get; private set; }
            }

            private sealed class Visitor : ExpressionVisitor
            {
                private readonly Expression _target;
                private readonly Expression _replacement;

                public Visitor(Expression target, Expression replacement)
                {
                    _target = target;
                    _replacement = replacement;
                }

                public override Expression Visit(Expression node)
                {
                    if (_target == node)
                    {
                        return _replacement;
                    }

                    return base.Visit(node);
                }
            }
        }
    }
}
