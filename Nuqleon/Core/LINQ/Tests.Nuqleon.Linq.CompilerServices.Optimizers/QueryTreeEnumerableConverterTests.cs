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
using System.Linq;
using System.Linq.CompilerServices;
using System.Linq.CompilerServices.Optimizers;
using System.Linq.Expressions;
using System.Reflection;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.System.Linq.CompilerServices.Optimizers
{
    [TestClass]
    public class QueryTreeEnumerableConverterTests
    {
#pragma warning disable CA1825 // Avoid unnecessary zero-length array allocations. (Used in expression tree)
        [TestMethod]
        public void QueryTreeConverter_Simple()
        {
            {
                RoundTripBothDomains(() => new int[0].AsQueryable().First());
            }

            {
                RoundTripBothDomains(() => new int[0].AsQueryable().First(_ => true));
            }

            {
                RoundTripBothDomains(() => new int[0].AsQueryable().Select(x => true));
            }

            {
                RoundTripBothDomains(() => new int[0].AsQueryable().Take(1));
            }

            {
                RoundTripBothDomains(() => new int[0].AsQueryable().Where(x => x == 1));
            }
        }

        [TestMethod]
        public void QueryTreeConverter_Touch()
        {
            {
                RoundTripAndTouchBothDomains(() => new int[0].AsQueryable().First());
            }

            {
                RoundTripAndTouchBothDomains(() => new int[0].AsQueryable().First(_ => true));
            }

            {
                RoundTripAndTouchBothDomains(() => new int[0].AsQueryable().Select(x => true));
            }

            {
                RoundTripAndTouchBothDomains(() => new int[0].AsQueryable().Take(1));
            }

            {
                RoundTripAndTouchBothDomains(() => new int[0].AsQueryable().Where(x => x == 1));
            }
        }
#pragma warning restore CA1825

        public static void RoundTripAndTouchBothDomains<T>(Expression<Func<T>> q)
        {
            var cmp = new ExpressionEqualityComparator();
            var qr = RoundTripAndTouchQueryable(q.Body);
            Assert.IsTrue(cmp.Equals(q.Body, qr));
            var e = ConvertToEnumerable(q);
            var er = RoundTripAndTouchEnumerable(e);
            Assert.IsTrue(cmp.Equals(e, er));
        }

        public static Expression RoundTripAndTouchQueryable(Expression query)
        {
            var c = new QueryableToQueryTreeConverter();
            var t = c.Convert(query);
            var i = new TouchVisitor().Visit(t);
            var r = i.Reduce();
            return r;
        }

        public static Expression RoundTripAndTouchEnumerable(Expression query)
        {
            var c = new EnumerableToQueryTreeConverter();
            var t = c.Convert(query);
            var i = new TouchVisitor().Visit(t);
            var r = i.Reduce();
            return r;
        }

        public static void RoundTripBothDomains<T>(Expression<Func<T>> q)
        {
            var cmp = new ExpressionEqualityComparator();
            var qr = RoundTripQueryable(q.Body);
            Assert.IsTrue(cmp.Equals(q.Body, qr));
            var e = ConvertToEnumerable(q);
            var er = RoundTripEnumerable(e);
            Assert.IsTrue(cmp.Equals(e, er));
        }

        public static Expression RoundTripQueryable(Expression query)
        {
            var c = new QueryableToQueryTreeConverter();
            var t = c.Convert(query);
            var r = t.Reduce();
            return r;
        }

        public static Expression RoundTripEnumerable(Expression query)
        {
            var c = new EnumerableToQueryTreeConverter();
            var t = c.Convert(query);
            var r = t.Reduce();
            return r;
        }

        public static Expression ConvertToEnumerable(Expression query)
        {
            var c = new QueryableToEnumerableConverter();
            var q = c.Visit(query);
            return q;
        }

        private class TouchVisitor : QueryVisitor, IOptimizer
        {
            public QueryTree Optimize(QueryTree queryTree)
            {
                return Visit(queryTree);
            }

            protected internal override QueryOperator VisitFirst(FirstOperator op)
            {
                var o = (FirstOperator)base.VisitFirst(op);
                var m = op.QueryExpressionFactory.First(o.ElementType, o.Source);
                return m;
            }

            protected internal override QueryOperator VisitFirstPredicate(FirstPredicateOperator op)
            {
                var o = (FirstPredicateOperator)base.VisitFirstPredicate(op);
                var m = op.QueryExpressionFactory.First(o.ElementType, o.Source, o.Predicate);
                return m;
            }

            protected internal override QueryOperator VisitSelect(SelectOperator op)
            {
                var o = (SelectOperator)base.VisitSelect(op);
                var m = op.QueryExpressionFactory.Select(o.ElementType, o.InputElementType, o.Source, o.Selector);
                return m;
            }

            protected internal override QueryOperator VisitTake(TakeOperator op)
            {
                var o = (TakeOperator)base.VisitTake(op);
                var m = op.QueryExpressionFactory.Take(o.ElementType, o.Source, o.Count);
                return m;
            }

            protected internal override QueryOperator VisitWhere(WhereOperator op)
            {
                var o = (WhereOperator)base.VisitWhere(op);
                var m = op.QueryExpressionFactory.Where(o.ElementType, o.Source, o.Predicate);
                return m;
            }
        }

        private class QueryableToEnumerableConverter : TypeSubstitutionExpressionVisitor
        {
            private static readonly IDictionary<Type, Type> s_map = new Dictionary<Type, Type>
            {
                { typeof(Queryable), typeof(Enumerable) },
                { typeof(IQueryable<>), typeof(IEnumerable<>) },
            };

            public QueryableToEnumerableConverter()
                : base(new Impl(s_map))
            {
            }

            protected override Expression VisitUnary(UnaryExpression node)
            {
                if (node.NodeType == ExpressionType.Quote)
                {
                    return Visit(node.Operand);
                }
                else
                {
                    return base.VisitUnary(node);
                }
            }

            private static readonly MethodInfo s_asQueryable = ((MethodInfo)ReflectionHelpers.InfoOf((IEnumerable<int> q) => q.AsQueryable())).GetGenericMethodDefinition();

            protected override Expression VisitMethodCall(MethodCallExpression node)
            {
                if (node.Method.IsGenericMethod && node.Method.GetGenericMethodDefinition() == s_asQueryable)
                    return Visit(node.Arguments[0]);

                return base.VisitMethodCall(node);
            }

            private sealed class Impl : TypeSubstitutor
            {
                public Impl(IDictionary<Type, Type> map)
                    : base(map)
                {
                }

                protected override Type VisitGenericClosed(Type type)
                {
                    if (type.GetGenericTypeDefinition() == typeof(Expression<>))
                    {
                        return Visit(type.GetGenericArguments().Single());
                    }
                    else
                    {
                        return base.VisitGenericClosed(type);
                    }
                }
            }
        }
    }
}
