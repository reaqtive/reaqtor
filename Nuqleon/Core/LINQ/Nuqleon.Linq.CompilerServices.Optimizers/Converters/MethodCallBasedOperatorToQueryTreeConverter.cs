// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// PS - February 2015 - Created this file.
//

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1062 // Omitted null checks. (Protected methods are supposed to be called with non-null arguments.)

using System.Linq.Expressions;
using System.Reflection;

namespace System.Linq.CompilerServices.Optimizers
{
    /// <summary>
    /// Converter for expressions which use <see cref="MethodCallExpression"/> for the operator.
    /// </summary>
    public abstract class MethodCallBasedOperatorToQueryTreeConverter : ToQueryTreeConverter
    {
        /// <summary>
        /// Factory to create domain specific derived operators.
        /// </summary>
        protected abstract IQueryExpressionFactory QueryExpressionFactory { get; }

        /// <summary>
        /// Visits method and returns a handle to a query tree if the current node is a known operator.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        /// <returns>The visited expression.</returns>
        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if (!TryGetOperatorType(node.Method, out OperatorType opType))
            {
                return base.VisitMethodCall(node);
            }

            switch (opType)
            {
                case OperatorType.Where:
                    {
                        var source = VisitFromKnownOperator(node.Arguments[0]);
                        var predicate = VisitFromKnownOperator(node.Arguments[1].StripQuotes());

                        var genericArgs = node.Method.GetGenericArguments();
                        var ret = QueryExpressionFactory.Where(
                            genericArgs[0],
                            ConvertToMonadMember(genericArgs[0], source),
                            predicate);

                        var handle = CreateKnownOperator(ret, node.Method.ReturnType);
                        return handle;
                    }
                case OperatorType.Select:
                    {
                        var source = VisitFromKnownOperator(node.Arguments[0]);
                        var selector = VisitFromKnownOperator(node.Arguments[1].StripQuotes());

                        var genericArgs = node.Method.GetGenericArguments();
                        var ret = QueryExpressionFactory.Select(
                            genericArgs[1],
                            genericArgs[0],
                            ConvertToMonadMember(genericArgs[0], source),
                            selector);

                        var handle = CreateKnownOperator(ret, node.Method.ReturnType);
                        return handle;
                    }
                case OperatorType.Take:
                    {
                        var source = VisitFromKnownOperator(node.Arguments[0]);
                        var count = VisitFromKnownOperator(node.Arguments[1]);

                        var genericArgs = node.Method.GetGenericArguments();
                        var ret = QueryExpressionFactory.Take(
                            genericArgs[0],
                            ConvertToMonadMember(genericArgs[0], source),
                            count);

                        var handle = CreateKnownOperator(ret, node.Method.ReturnType);
                        return handle;
                    }
                case OperatorType.First:
                    {
                        var source = VisitFromKnownOperator(node.Arguments[0]);

                        var genericArgs = node.Method.GetGenericArguments();
                        var ret = QueryExpressionFactory.First(
                            genericArgs[0],
                            ConvertToMonadMember(genericArgs[0], source));

                        var handle = CreateKnownOperator(ret, node.Method.ReturnType);
                        return handle;
                    }
                case OperatorType.FirstPredicate:
                    {
                        var source = VisitFromKnownOperator(node.Arguments[0]);
                        var predicate = VisitFromKnownOperator(node.Arguments[1].StripQuotes());

                        var genericArgs = node.Method.GetGenericArguments();
                        var ret = QueryExpressionFactory.First(
                            genericArgs[0],
                            ConvertToMonadMember(genericArgs[0], source),
                            predicate);

                        var handle = CreateKnownOperator(ret, node.Method.ReturnType);
                        return handle;
                    }
                default:
                    return base.VisitMethodCall(node);
            }
        }

        /// <summary>
        /// Tries to get the operator from the method if an operator corresponds to the method.
        /// </summary>
        /// <param name="method">The method to look up.</param>
        /// <param name="operatorType">The operator the method represents.</param>
        /// <returns>Whether matching of the method to an operator was successful.</returns>
        protected abstract bool TryGetOperatorType(MethodInfo method, out OperatorType operatorType);
    }
}
