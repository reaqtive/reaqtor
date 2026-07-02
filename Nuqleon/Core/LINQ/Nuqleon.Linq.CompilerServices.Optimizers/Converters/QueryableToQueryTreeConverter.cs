// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// PS - February 2015 - Created this file.
//

using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace System.Linq.CompilerServices.Optimizers
{
    /// <summary>
    /// A converter to create query expression trees from expressions with <see cref="IQueryable{T}" /> types.
    /// </summary>
    public class QueryableToQueryTreeConverter
    {
#if !NET6_0_OR_GREATER  // The latest analyzer doesn't apply IDE0034 in expression trees, but we get this warning on older targets.
#pragma warning disable IDE0034 // Simplify 'default' expression (illustrative of the method signatures)
#endif
        private static readonly MethodInfo s_first = ((MethodInfo)ReflectionHelpers.InfoOf((IQueryable<int> xs) => xs.First())).GetGenericMethodDefinition();
        private static readonly MethodInfo s_firstPredicate = ((MethodInfo)ReflectionHelpers.InfoOf((IQueryable<int> xs) => xs.First(default(Expression<Func<int, bool>>)))).GetGenericMethodDefinition();
        private static readonly MethodInfo s_select = ((MethodInfo)ReflectionHelpers.InfoOf((IQueryable<int> xs) => xs.Select(default(Expression<Func<int, int>>)))).GetGenericMethodDefinition();
        private static readonly MethodInfo s_take = ((MethodInfo)ReflectionHelpers.InfoOf((IQueryable<int> xs) => xs.Take(default(int)))).GetGenericMethodDefinition();
        private static readonly MethodInfo s_where = ((MethodInfo)ReflectionHelpers.InfoOf((IQueryable<int> xs) => xs.Where(default(Expression<Func<int, bool>>)))).GetGenericMethodDefinition();
#if !NET6_0_OR_GREATER
#pragma warning restore IDE0034 // Simplify 'default' expression
#endif

        private readonly Impl _converter;

        /// <summary>
        /// Creates a converter to create query expression trees from expressions with <see cref="IQueryable{T}" /> types.
        /// </summary>
        public QueryableToQueryTreeConverter()
        {
            _converter = new Impl();
        }

        /// <summary>
        /// Converts an expression with <see cref="IQueryable{T}" /> types into a query expression tree.
        /// </summary>
        /// <param name="expression">The expression to convert.</param>
        /// <returns>A query expression tree representation of the query.</returns>
        public QueryTree Convert(Expression expression) => _converter.Convert(expression);

        private sealed class Impl : MethodCallBasedOperatorToQueryTreeConverter
        {
#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable format // (Formatted as a table.)
            private static readonly Dictionary<MethodInfo, OperatorType> operatorMap = new()
            {
                { s_first,          OperatorType.First          },
                { s_firstPredicate, OperatorType.FirstPredicate },
                { s_select,         OperatorType.Select         },
                { s_take,           OperatorType.Take           },
                { s_where,          OperatorType.Where          },
            };
#pragma warning restore format
#pragma warning restore IDE0079

            protected override IQueryExpressionFactory QueryExpressionFactory => QueryableQueryExpressionFactory.Instance;

            protected override bool TryGetOperatorType(MethodInfo method, out OperatorType operatorType)
            {
                if (method == null)
                    throw new ArgumentNullException(nameof(method));

                if (!method.IsGenericMethod)
                {
                    operatorType = default;
                    return false;
                }

                return operatorMap.TryGetValue(method.GetGenericMethodDefinition(), out operatorType);
            }
        }

        private sealed class QueryableQueryExpressionFactory : DefaultQueryExpressionFactory, IQueryExpressionFactory
        {
            public static new QueryableQueryExpressionFactory Instance { get; } = new QueryableQueryExpressionFactory();

            protected override SelectOperator MakeSelect(Type elementType, Type inputElementType, MonadMember source, QueryTree selector)
            {
                return new QueryableSelectOperator(elementType, inputElementType, source, selector);
            }

            protected override WhereOperator MakeWhere(Type elementType, MonadMember source, QueryTree predicate)
            {
                return new QueryableWhereOperator(elementType, source, predicate);
            }

            protected override FirstOperator MakeFirst(Type elementType, MonadMember source)
            {
                return new QueryableFirstOperator(elementType, source);
            }

            protected override FirstPredicateOperator MakeFirstPredicate(Type elementType, MonadMember source, QueryTree predicate)
            {
                return new QueryableFirstPredicateOperator(elementType, source, predicate);
            }

            protected override TakeOperator MakeTake(Type elementType, MonadMember source, QueryTree count)
            {
                return new QueryableTakeOperator(elementType, source, count);
            }
        }

        private sealed class QueryableFirstOperator : FirstOperator
        {
            internal QueryableFirstOperator(Type elementType, MonadMember source)
                : base(elementType, source)
            {
            }

            public override IQueryExpressionFactory QueryExpressionFactory => QueryableQueryExpressionFactory.Instance;

            public override Expression Reduce()
            {
                return Expression.Call(s_first.MakeGenericMethod(ElementType), Source.Reduce());
            }
        }

        private sealed class QueryableFirstPredicateOperator : FirstPredicateOperator
        {
            internal QueryableFirstPredicateOperator(Type elementType, MonadMember source, QueryTree predicate)
                : base(elementType, source, predicate)
            {
            }

            public override IQueryExpressionFactory QueryExpressionFactory => QueryableQueryExpressionFactory.Instance;

            public override Expression Reduce()
            {
                return Expression.Call(s_firstPredicate.MakeGenericMethod(ElementType), Source.Reduce(), Predicate.Reduce());
            }
        }

        private sealed class QueryableSelectOperator : SelectOperator
        {
            // TODO make this take a MethodInfo instead?
            public QueryableSelectOperator(Type elementType, Type inputElementType, MonadMember source, QueryTree selector)
                : base(elementType, inputElementType, source, selector)
            {
            }

            public override IQueryExpressionFactory QueryExpressionFactory => QueryableQueryExpressionFactory.Instance;

            public override Expression Reduce()
            {
                return Expression.Call(s_select.MakeGenericMethod(InputElementType, ElementType), Source.Reduce(), Selector.Reduce());
            }
        }

        private sealed class QueryableTakeOperator : TakeOperator
        {
            internal QueryableTakeOperator(Type elementType, MonadMember source, QueryTree count)
                : base(elementType, source, count)
            {
            }

            public override IQueryExpressionFactory QueryExpressionFactory => QueryableQueryExpressionFactory.Instance;

            public override Expression Reduce()
            {
                return Expression.Call(s_take.MakeGenericMethod(ElementType), Source.Reduce(), Count.Reduce());
            }
        }

        private sealed class QueryableWhereOperator : WhereOperator
        {
            internal QueryableWhereOperator(Type elementType, MonadMember source, QueryTree predicate)
                : base(elementType, source, predicate)
            {
            }

            public override IQueryExpressionFactory QueryExpressionFactory => QueryableQueryExpressionFactory.Instance;

            public override Expression Reduce()
            {
                return Expression.Call(s_where.MakeGenericMethod(ElementType), Source.Reduce(), Predicate.Reduce());
            }
        }
    }
}
