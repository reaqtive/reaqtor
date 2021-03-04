// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Reaqtor.Remoting.QueryEvaluator
{
    /// <summary>
    /// Provides helper methods to rewrite expression trees.
    /// </summary>
    internal static class ExpressionRewriteHelpers
    {
        /// <summary>
        /// Rewrites the expression from asynchronous reactive proxy interfaces to their synchronous counterparts.
        /// </summary>
        /// <param name="expression">The expression to rewrite to a synchronous form.</param>
        /// <returns>The rewritten expression using synchronous operations.</returns>
        public static Expression RewriteAsyncToSync(Expression expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            var rw =
                new AsyncToSyncRewriter(
                    new Dictionary<Type, Type>
                    {
                        { typeof(IReactiveClientProxy), typeof(IReactiveClient) },
                        { typeof(IReactiveDefinitionProxy), typeof(IReactiveDefinition) },
                        { typeof(IReactiveMetadataProxy), typeof(IReactiveMetadata) },
                        { typeof(IReactiveProxy), typeof(IReactive) },
                    });

            return rw.Rewrite(expression);
        }

        /// <summary>
        /// Rewrites the expression to include a host-aware hook to perform cleanup operators after the observable's subscription is terminated.
        /// For more information about this hook, <see cref="SubscriptionCleanup&lt;T&gt;"/>.
        /// </summary>
        /// <param name="expression">The subscription expression to instrument with a cleanup hook.</param>
        /// <returns>The rewritten subscription expression that includes the cleanup hook.</returns>
        public static Expression GetExpressionWithSubscriptionCleanupHook(this Expression expression)
        {
            return GetExpressionWithSubscriptionCleanupHook(expression, throwWhenNotAttached: true);
        }

        /// <summary>
        /// Rewrites the expression to include a host-aware hook to perform cleanup operators after the observable's subscription is terminated.
        /// For more information about this hook, <see cref="SubscriptionCleanup&lt;T&gt;"/>.
        /// </summary>
        /// <param name="expression">The subscription expression to instrument with a cleanup hook.</param>
        /// <param name="throwWhenNotAttached">Indicates whether the failure to add a cleanup hook will result in an exception.</param>
        /// <returns>The rewritten subscription expression that includes the cleanup hook.</returns>
        public static Expression GetExpressionWithSubscriptionCleanupHook(this Expression expression, bool throwWhenNotAttached)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            if (expression is not InvocationExpression invoke ||
                invoke.Expression.NodeType != ExpressionType.Parameter ||
                invoke.Arguments.Count != 1 ||
                invoke.Arguments[0].NodeType != ExpressionType.New ||
                invoke.Arguments[0].Type.FindGenericType(typeof(Tuple<,>)) == null)
            {
                if (!throwWhenNotAttached)
                {
                    return expression;
                }

                throw new InvalidOperationException(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "Expected a subscription invocation with a tuple parameter of generic type IAsyncReactiveQbservable and IAsyncReactiveQbserver, respectively, of type IAsyncReactiveQubscription, got '{0}'.",
                        expression.ToTraceString()));
            }

            var tupleArgs = invoke.Arguments[0].Type.GetGenericArguments();
            var target = (ParameterExpression)invoke.Expression;
            var tupleExpr = (NewExpression)invoke.Arguments[0];
            if (tupleArgs[0].FindGenericType(typeof(IAsyncReactiveQbservable<>)) == null ||
                tupleArgs[1].FindGenericType(typeof(IAsyncReactiveQbserver<>)) == null ||
                !typeof(Task<IAsyncReactiveQubscription>).IsAssignableFrom(invoke.Type))
            {
                if (!throwWhenNotAttached)
                {
                    return expression;
                }

                throw new InvalidOperationException(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "Expected a subscription invocation with a tuple parameter of generic type IAsyncReactiveQbservable and IAsyncReactiveQbserver, respectively, of type IAsyncReactiveQubscription, got '{0}'.",
                        expression.ToTraceString()));
            }

            var observableExpression = tupleExpr.Arguments[0];
            var observerExpression = tupleExpr.Arguments[1];

            var cleanupFunctionType = typeof(Func<,>).MakeGenericType(observableExpression.Type, observableExpression.Type);
            var cleanupOperator = Expression.Parameter(cleanupFunctionType, HostOperatorConstants.CleanupSubscriptionUri);
            var observableWithCleanupOperator = Expression.Invoke(cleanupOperator, observableExpression);

            var rewrittenExpression = Expression.Invoke(
                target,
                Expression.New(
                    tupleExpr.Constructor,
                    observableWithCleanupOperator,
                    observerExpression
                )
            );

            return rewrittenExpression;
        }

#if UNUSED // NB: Leftover from other calling conventions; kept for illustration purposes.
        /// <summary>
        /// Rewrites a subscription expression which is an invocation of a packed tuple containing the
        /// observable and observer into an expression with these arguments unpacked.  If the expression
        /// does not contain a packed tuple argument, the original expression is returned.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns>The expression with unpacked arguments, or original</returns>
        private static Expression RewriteSubscriptionInvocationWithoutTuples(this Expression expression)
        {
            if (expression is InvocationExpression invoke &&
                invoke.Expression is ParameterExpression parameter &&
                invoke.Arguments.Count == 1 &&
                invoke.Arguments[0] is NewExpression newExpr &&
                invoke.Arguments[0].Type.FindGenericType(typeof(Tuple<,>)) != null)
            {
                var tupleArgs = invoke.Arguments[0].Type.GetGenericArguments();
                var target = parameter;
                var tupleExpr = newExpr;
                if (tupleArgs[0].FindGenericType(typeof(IAsyncReactiveQbservable<>)) != null &&
                    tupleArgs[1].FindGenericType(typeof(IAsyncReactiveQbserver<>)) != null &&
                    typeof(Task<IAsyncReactiveQubscription>).IsAssignableFrom(invoke.Type))
                {
                    Debug.Assert(tupleExpr.Arguments.Count == 2);

                    var newTargetType = typeof(Func<,,>).MakeGenericType(tupleArgs[0], tupleArgs[1], invoke.Type);
                    var newTarget = Expression.Parameter(newTargetType, target.Name);
                    var observableExpression = tupleExpr.Arguments[0];
                    var observerExpression = tupleExpr.Arguments[1];
                    var newInvoke = Expression.Invoke(newTarget, observableExpression, observerExpression);
                    return newInvoke;
                }
            }

            return expression;
        }
#endif
    }
}
