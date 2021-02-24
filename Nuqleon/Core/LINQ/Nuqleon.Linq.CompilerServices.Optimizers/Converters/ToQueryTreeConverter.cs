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

namespace System.Linq.CompilerServices.Optimizers
{
    /// <summary>
    /// Base class for conversions from a source domain to the query expression tree model.
    /// </summary>
    public abstract class ToQueryTreeConverter : ExpressionVisitor
    {
        private readonly Stack<IDictionary<ParameterExpression, QueryTree>> _knownResourcesTable;

        private QueryTree _result;

        /// <summary>
        /// Creates the converter to the query expression tree model.
        /// </summary>
        protected ToQueryTreeConverter()
        {
            _knownResourcesTable = new Stack<IDictionary<ParameterExpression, QueryTree>>();
        }

        /// <summary>
        /// Converts the expression to the query expression tree model.
        /// </summary>
        /// <param name="expression">The expression to convert.</param>
        /// <returns>The converted expression.</returns>
        public QueryTree Convert(Expression expression) => VisitFromKnownOperator(expression);

        /// <summary>
        /// Creates a handle for the query tree node which is to be used to replace the node being visited.
        /// </summary>
        /// <param name="queryTree">The query tree representing the current node.</param>
        /// <param name="type">The type of the current node.</param>
        /// <returns>A handle which is associated with the query tree replacing the current node.</returns>
        protected ParameterExpression CreateKnownOperator(QueryTree queryTree, Type type)
        {
            _result = queryTree;

            var parameter = Expression.Parameter(type);
            _knownResourcesTable.Peek().Add(parameter, _result);

            return parameter;
        }

        /// <summary>
        /// Visits the expression from an expression node which is going to be replaced by a query expression tree node. If visiting the expression doesn't return a handle to a query tree, then it is wrapped in a lambda abstraction.
        /// </summary>
        /// <param name="expression">The expression to visit.</param>
        /// <returns>The result of visiting the expression. If it is a handle to a query tree, then the query tree is returned; else a lambda abstraction is returned.</returns>
        protected QueryTree VisitFromKnownOperator(Expression expression)
        {
            var scope = new Dictionary<ParameterExpression, QueryTree>();

            _knownResourcesTable.Push(scope);

            var ret = Visit(expression);

            if (ret.NodeType == ExpressionType.Quote)
            {
                ret = ret.StripQuotes();
            }

            _knownResourcesTable.Pop();

            if (ret is ParameterExpression srcParam && scope.ContainsKey(srcParam))
            {
                return _result;
            }
            else
            {
                var parameters = new ParameterExpression[scope.Count];
                var abstractions = new QueryTree[scope.Count];

                var i = 0;
                foreach (var kvp in scope)
                {
                    parameters[i] = kvp.Key;
                    abstractions[i] = kvp.Value;

                    i++;
                }

                return DefaultQueryExpressionFactory.Instance.LambdaAbstraction(Expression.Lambda(ret, parameters), abstractions);
            }
        }

        /// <summary>
        /// Converts the query tree to a monad member by wrapping it if needed.
        /// </summary>
        /// <param name="monadElementType">The desired element type of the resulting monad.</param>
        /// <param name="queryTree">The query tree to convert.</param>
        /// <returns>The query tree as a monad member.</returns>
        protected static MonadMember ConvertToMonadMember(Type monadElementType, QueryTree queryTree)
        {
            if (queryTree == null)
                throw new ArgumentNullException(nameof(queryTree));

            return queryTree.QueryNodeType is QueryNodeType.MonadAbstraction or QueryNodeType.Operator
                ? (MonadMember)queryTree
                : DefaultQueryExpressionFactory.Instance.MonadAbstraction(monadElementType, queryTree);
        }
    }
}
