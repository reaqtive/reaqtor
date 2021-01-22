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
    /// A converter to create query expression trees from expressions with <see cref="IEnumerable{T}" /> types.
    /// </summary>
    public class EnumerableToQueryTreeConverter
    {
#pragma warning disable IDE0034 // Simplify 'default' expression (illustrative of the method signatures)
        private static readonly MethodInfo s_first = ((MethodInfo)ReflectionHelpers.InfoOf((IEnumerable<int> xs) => xs.First())).GetGenericMethodDefinition();
        private static readonly MethodInfo s_firstPredicate = ((MethodInfo)ReflectionHelpers.InfoOf((IEnumerable<int> xs) => xs.First(default(Func<int, bool>)))).GetGenericMethodDefinition();
        private static readonly MethodInfo s_select = ((MethodInfo)ReflectionHelpers.InfoOf((IEnumerable<int> xs) => xs.Select(default(Func<int, int>)))).GetGenericMethodDefinition();
        private static readonly MethodInfo s_take = ((MethodInfo)ReflectionHelpers.InfoOf((IEnumerable<int> xs) => xs.Take(default(int)))).GetGenericMethodDefinition();
        private static readonly MethodInfo s_where = ((MethodInfo)ReflectionHelpers.InfoOf((IEnumerable<int> xs) => xs.Where(default(Func<int, bool>)))).GetGenericMethodDefinition();
#pragma warning restore IDE0034 // Simplify 'default' expression

        private readonly Impl _converter;

        /// <summary>
        /// Creates a converter to create query expression trees from expressions with <see cref="IEnumerable{T}" /> types.
        /// </summary>
        public EnumerableToQueryTreeConverter()
        {
            _converter = new Impl();
        }

        /// <summary>
        /// Converts an expression with <see cref="IEnumerable{T}" /> types into a query expression tree.
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

            protected override IQueryExpressionFactory QueryExpressionFactory => EnumerableQueryExpressionFactory.Instance;

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

        private sealed class EnumerableQueryExpressionFactory : DefaultQueryExpressionFactory, IQueryExpressionFactory
        {
            public static new EnumerableQueryExpressionFactory Instance { get; } = new EnumerableQueryExpressionFactory();

            protected override SelectOperator MakeSelect(Type elementType, Type inputElementType, MonadMember source, QueryTree selector)
            {
                return new EnumerableSelectOperator(elementType, inputElementType, source, selector);
            }

            protected override WhereOperator MakeWhere(Type elementType, MonadMember source, QueryTree predicate)
            {
                return new EnumerableWhereOperator(elementType, source, predicate);
            }

            protected override FirstOperator MakeFirst(Type elementType, MonadMember source)
            {
                return new EnumerableFirstOperator(elementType, source);
            }

            protected override FirstPredicateOperator MakeFirstPredicate(Type elementType, MonadMember source, QueryTree predicate)
            {
                return new EnumerableFirstPredicateOperator(elementType, source, predicate);
            }

            protected override TakeOperator MakeTake(Type elementType, MonadMember source, QueryTree count)
            {
                return new EnumerableTakeOperator(elementType, source, count);
            }
        }

        private sealed class EnumerableFirstOperator : FirstOperator
        {
            internal EnumerableFirstOperator(Type elementType, MonadMember source)
                : base(elementType, source)
            {
            }

            public override IQueryExpressionFactory QueryExpressionFactory => EnumerableQueryExpressionFactory.Instance;

            public override Expression Reduce()
            {
                return Expression.Call(s_first.MakeGenericMethod(ElementType), Source.Reduce());
            }
        }

        private sealed class EnumerableFirstPredicateOperator : FirstPredicateOperator
        {
            internal EnumerableFirstPredicateOperator(Type elementType, MonadMember source, QueryTree predicate)
                : base(elementType, source, predicate)
            {
            }

            public override IQueryExpressionFactory QueryExpressionFactory => EnumerableQueryExpressionFactory.Instance;

            public override Expression Reduce()
            {
                return Expression.Call(s_firstPredicate.MakeGenericMethod(ElementType), Source.Reduce(), Predicate.Reduce());
            }
        }

        private sealed class EnumerableSelectOperator : SelectOperator
        {
            // TODO make this take a MethodInfo instead?
            public EnumerableSelectOperator(Type elementType, Type inputElementType, MonadMember source, QueryTree selector)
                : base(elementType, inputElementType, source, selector)
            {
            }

            public override IQueryExpressionFactory QueryExpressionFactory => EnumerableQueryExpressionFactory.Instance;

            public override Expression Reduce()
            {
                return Expression.Call(s_select.MakeGenericMethod(InputElementType, ElementType), Source.Reduce(), Selector.Reduce());
            }
        }

        private sealed class EnumerableTakeOperator : TakeOperator
        {
            internal EnumerableTakeOperator(Type elementType, MonadMember source, QueryTree count)
                : base(elementType, source, count)
            {
            }

            public override IQueryExpressionFactory QueryExpressionFactory => EnumerableQueryExpressionFactory.Instance;

            public override Expression Reduce()
            {
                return Expression.Call(s_take.MakeGenericMethod(ElementType), Source.Reduce(), Count.Reduce());
            }
        }

        private sealed class EnumerableWhereOperator : WhereOperator
        {
            internal EnumerableWhereOperator(Type elementType, MonadMember source, QueryTree predicate)
                : base(elementType, source, predicate)
            {
            }

            public override IQueryExpressionFactory QueryExpressionFactory => EnumerableQueryExpressionFactory.Instance;

            public override Expression Reduce()
            {
                return Expression.Call(s_where.MakeGenericMethod(ElementType), Source.Reduce(), Predicate.Reduce());
            }
        }
    }
}
