// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

//
// Normalizer for query operator applications based on properties of these query operators.
//
// BD - September 2014
//
//
// Design notes:
//
//   This rewriter combines the predicate simplifier and normalizers into a query-level
//   rewrite capability. It leverages knowledge of the known resource identifier for
//   filters in order to detect filters whose predicates need to be normalized.
//
//   In order to increase the likelihood of sharing subexpressions, it also employs some
//   simplified strategy to order predicates such that the ones with higher chance of
//   reuse move closer to the source. For example:
//
//     xs.Where(x => f(x) && g(x))
//
//   will first get split into consecutive Where clauses such that we can simplify and
//   normalize the predicates, i.e.
//
//     xs.Where(x => f'(x)).Where(x => g'(x))
//
//   where f' and g' are the results of simplifying and normalizing f and g, respectively.
//   A subsequent reordering step will sort the predicates, e.g. if g' is deemed more
//   likely to result in reuse, the expression will become:
//
//     xs.Where(x => g'(x)).Where(x => f'(x))
//

using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Pearls.Reaqtor.CSE
{
    /// <summary>
    /// Normalizer for query operator occurrences in the query expression, used to improve the likelihood of sharing of subexpressions.
    /// </summary>
    internal class QueryOperatorNormalizer : ExpressionVisitor
    {
        // TODO: omitted non-algebraic approach to sorting of e.g. predicates by cardinality estimation; this requires a much deeper integration that can be considered separately
        //       this only applies one possible strategy; others may be more effective depending on domain properties

        /// <summary>
        /// Analyzes invocation expressions for the typical pattern of invoking a known resource in order to normalize query operator applications.
        /// </summary>
        /// <param name="node">Expression to analyze.</param>
        /// <returns>Expression after applying query operator normalizations, e.g. resulting in the sorting of predicates by filtering likelihood and cardinality reduction.</returns>
        protected override Expression VisitInvocation(InvocationExpression node)
        {
            if (node.Expression is ParameterExpression p) // omitted unbound parameter check
            {
                switch (p.Name)
                {
                    case "rx://operators/filter":
                        {
                            if (node.Arguments[1] is LambdaExpression f)
                            {
                                var x = f.Parameters[0];

                                var s = new PredicateSimplifier();
                                var n = new PredicateNormalizer(x);

                                // TODO: sorting of predicates needs to be done with more care; e.g. null checks need to precede expressions using the possibly-null subexpression

                                var filters = GetFilters(f.Body).Select(s.Simplify).ToArray();
                                var allFilters = filters.SelectMany(GetFilters).ToArray(); // De Morgan's rule could cause expansion

                                var normalized = allFilters.Select(n.Normalize).ToArray();
                                var ordered = normalized.OrderBy(Primary).ThenBy(e => e.NodeType).ThenBy(e => e, new ExpressionSorter(x)).ToArray();

                                var res = node.Arguments[0];

                                foreach (var filter in ordered)
                                {
                                    res = Expression.Invoke(p, res, Expression.Lambda(filter, x));
                                }

                                return res;
                            }
                        }
                        break;
                }
            }

            return base.VisitInvocation(node);
        }

        /// <summary>
        /// Mapping function for primary ordering of filter expressions.
        /// </summary>
        /// <param name="e">Expression to returning an ordering key for.</param>
        /// <returns>Ordering key for the specified expression.</returns>
        private static int Primary(Expression e)
        {
            // e.g. xs.Where(x => x < 0 && x == 0 && x != 0) --> xs.Where(x => x == 0).Where(x => x < 0).Where(x => x != 0)
            //                                                   ^^^^^^^^^^^^^^^^^^^^^
            //
            //                                         high cardinality reduction & chance of reuse

            // TODO: below is some poor man's avoidance of reordering of common patterns of argument checking; give this more thought (could even use Z3 or SF if we want to go fancy)
            //       one option is to have a very conservative rewrite that only bubbles up subexpressions that have no common prefix with others

            if (e is BinaryExpression b)
            {
                if (b.NodeType == ExpressionType.NotEqual)
                {
                    if (b.Right is ConstantExpression c && c.Value == null)
                    {
                        return -1;
                    }
                }

                if (b.Left.NodeType == ExpressionType.ArrayLength)
                {
                    return -1;
                }
            }

            if (e is MemberExpression m && (m.Member.Name == "HasValue" || m.Member.Name == "Length")) // TODO: omitted checks for Nullable<T>, List<T>, string, etc.
            {
                return -1;
            }

            if (e is UnaryExpression u)
            {
                if (u.Operand is MethodCallExpression c && c.Method.DeclaringType == typeof(string) && c.Method.Name.StartsWith("IsNullOr"))
                {
                    return -1;
                }
            }

            return e.NodeType switch
            {
                ExpressionType.Equal => 0, // exact match, high filter rate
                ExpressionType.LessThan or ExpressionType.LessThanOrEqual or ExpressionType.GreaterThan or ExpressionType.GreaterThanOrEqual => 1, // assume domain properties that have degree of filtering
                _ => 2, // opaque Boolean-valued methods and properties, NotEqual, etc.
            };
        }

        /// <summary>
        /// Comparer used to sort expressions in a canonical order, used to improve the likelihood fo sharing of subexpressions.
        /// </summary>
        private class ExpressionSorter : IComparer<Expression>
        {
            /// <summary>
            /// Range variable of the filter predicates.
            /// </summary>
            private readonly ParameterExpression _p;

            /// <summary>
            /// Creates a new expression comparer using the specified range variable used in filter predicates.
            /// </summary>
            /// <param name="p">Range variable of the filter predicates.</param>
            public ExpressionSorter(ParameterExpression p)
            {
                _p = p;
            }

            /// <summary>
            /// Compares the specified expressions to establish an order.
            /// </summary>
            /// <param name="x">First expression to compare.</param>
            /// <param name="y">Second expression to compare.</param>
            /// <returns>Integer number representing the relative order of the specified expressions.</returns>
            public int Compare(Expression x, Expression y)
            {
                if (TryFindMember(x, out var m1) && TryFindMember(y, out var m2))
                {
                    // e.g. reorders xs.Where(x => x.Name == "Bart" && x.Age == 21) to xs.Where(x => x.Age == 21).Where(x => x.Name == "Bart")
                    return m1.CompareTo(m2);
                }

                // TODO: this is a bit shallow; would be better to classify first by "sortability"
                return 0;
            }

            /// <summary>
            /// Tries to find a single member access on the range variable in a given expression.
            /// </summary>
            /// <param name="e">Expression to scan for a single member access on the range variable.</param>
            /// <param name="m">The member name, if a single member access was found; otherwise, <c>null</c>.</param>
            /// <returns><c>true</c> if a single member access was found; otherwise, <c>false</c>.</returns>
            private bool TryFindMember(Expression e, out string m)
            {
                // This strategy

                var f = new MemberFinder(_p);
                f.Visit(e);

                if (f.Members.Count == 1)
                {
                    m = f.Members[0].Name;
                    return true;
                }

                m = null;
                return false;
            }

            // e.g. reorders xs.Where(x => x.Name == "Bart" && x.Age == 21) to xs.Where(x => x.Age == 21).Where(x => x.Name == "Bart")
            // alternatively, domains could be ranked based on range (string equality has higher likelihood of filtering compared to such comparisons in a small domain like Int32)

            /// <summary>
            /// Expression visitor to find member accesses on the range variable.
            /// </summary>
            private class MemberFinder : ExpressionVisitor
            {
                /// <summary>
                /// Range variable of the filter predicates.
                /// </summary>
                private readonly ParameterExpression _p;

                /// <summary>
                /// Creates a new member finder using the specified range variable used in filter predicates.
                /// </summary>
                /// <param name="p">Range variable of the filter predicates.</param>
                public MemberFinder(ParameterExpression p)
                {
                    _p = p;
                }

                /// <summary>
                /// Gets the member accesses on the range variable that were found in the analyzed expression.
                /// </summary>
                public List<MemberInfo> Members { get; } = new(); // not a set; want to be conservative

                /// <summary>
                /// Visits member expressions to detect member access on the range variable.
                /// </summary>
                /// <param name="node">Expression node to analyze.</param>
                /// <returns>The original expression.</returns>
                protected override Expression VisitMember(MemberExpression node)
                {
                    if (node.Expression == _p)
                    {
                        Members.Add(node.Member);
                    }

                    return base.VisitMember(node);
                }
            }
        }

        /// <summary>
        /// Gets all the filters in a predicate that are combined using AND.
        /// </summary>
        /// <param name="expression">Predicate expression to get all the filters for.</param>
        /// <returns>All the filters that occur in the given predicate expression.</returns>
        private IEnumerable<Expression> GetFilters(Expression expression)
        {
            var q = new Queue<Expression>();
            q.Enqueue(expression);

            while (q.Count > 0)
            {
                var e = q.Dequeue();

                if (e.NodeType is ExpressionType.And or ExpressionType.AndAlso) // assume pure functions so short-circuiting effect is irrelevant
                {
                    var b = (BinaryExpression)e;
                    q.Enqueue(b.Left);
                    q.Enqueue(b.Right);
                }
                else
                {
                    yield return e;
                }
            }
        }
    }
}
