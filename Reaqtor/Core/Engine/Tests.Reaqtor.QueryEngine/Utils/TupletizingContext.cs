// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.CompilerServices;
using System.Linq.Expressions;
using System.Reflection;

using Reaqtor;
using Reaqtor.Service.Core;

namespace Tests.Reaqtor.QueryEngine
{
    internal class TupletizingClientContext : ReactiveClientContext
    {
        public TupletizingClientContext(IReactiveServiceProvider provider)
            : base(new ExpressionService(), provider)
        {
        }

        private class ExpressionService : ReactiveExpressionServices
        {
            public ExpressionService()
                : base(typeof(IReactiveClientProxy))
            {
            }

            public override Expression Normalize(Expression expression)
            {
                var normalized = base.Normalize(expression);
                var tupletized = new InvocationTupletizer().Visit(normalized);
                if (tupletized is LambdaExpression lambda)
                {
                    tupletized = ExpressionTupletizer.Pack(lambda);
                }

                var sync = new AsyncToSyncRewriter(new Dictionary<Type, Type>()).Rewrite(tupletized);

                return sync;
            }
        }

        // TODO: Move InvocationTupletizer to a common location.

        /// <summary>
        /// Tupletizer for unbound parameter invocation sites.
        /// </summary>
        private class InvocationTupletizer : ScopedExpressionVisitor<ParameterExpression>
        {
            /// <summary>
            /// Visits the children of the <see cref="T:System.Linq.Expressions.InvocationExpression" />.
            /// </summary>
            /// <param name="node">The expression to visit.</param>
            /// <returns>
            /// The modified expression, if it or any subexpression was modified; otherwise, returns the original expression.
            /// </returns>
            protected override Expression VisitInvocation(InvocationExpression node)
            {
                var expr = Visit(node.Expression);
                var args = Visit(node.Arguments);

                if (expr.NodeType == ExpressionType.Parameter)
                {
                    var parameter = (ParameterExpression)expr;

                    // Turns f(x, y, z) into f((x, y, z)) when f is an unbound parameter, i.e. representing a known resource.
                    if (IsUnboundParameter(parameter))
                    {
                        if (args.Count > 0)
                        {
                            var tuple = ExpressionTupletizer.Pack(args);
                            var funcType = Expression.GetDelegateType(tuple.Type, node.Type);
                            var function = Expression.Parameter(funcType, parameter.Name);
                            return Expression.Invoke(function, tuple);
                        }
                    }
                }

                return node.Update(expr, args);
            }

            /// <summary>
            /// Gets the state associated with the specified parameter declaration.
            /// </summary>
            /// <param name="parameter">The parameter to obtain associated state for.</param>
            /// <returns>State associated with the specified parameter declaration.</returns>
            protected override ParameterExpression GetState(ParameterExpression parameter) => parameter;

            /// <summary>
            /// Determines whether the specified parameter is unbound.
            /// </summary>
            /// <param name="parameter">The parameter to check.</param>
            /// <returns>
            ///   <c>true</c> if the specified parameter is unbound; otherwise, <c>false</c>.
            /// </returns>
            private bool IsUnboundParameter(ParameterExpression parameter) => !TryLookup(parameter, out _);
        }
    }

    public class TupletizingContext : ReactiveServiceContext
    {
        public TupletizingContext(IReactive innerService)
            : base(new ExpressionService(), new Engine(innerService))
        {
        }

        private class Engine : IReactiveEngineProvider
        {
            private static readonly MethodInfo _invokeTypedExpressionHelper1 = ((MethodInfo)ReflectionHelpers.InfoOf(() => InvokeTypedExpressionHelper<object, object>(null, null))).GetGenericMethodDefinition();

            private readonly IReactive _innerService;

            public Engine(IReactive innerService)
            {
                _innerService = innerService;
            }

            public void CreateSubscription(Uri subscriptionUri, Expression subscription, object state)
            {
                UriToReactiveBinder binder = new UriToReactiveBinder();
                var bindingExpr = binder.BindSubscription(subscription, subscriptionUri, state);
                ((Expression<Func<IReactive, IReactiveQubscription>>)bindingExpr).Compile()(_innerService);
            }

            public void DeleteSubscription(Uri subscriptionUri)
            {
                _innerService.GetSubscription(subscriptionUri).Dispose();
            }

            public void CreateStream(Uri streamUri, Expression stream, object state)
            {
                UriToReactiveBinder binder = new UriToReactiveBinder();
                var bindingExpr = binder.BindSubject(stream, streamUri, state);

                Type exprType = bindingExpr.Type;

                Debug.Assert(exprType.IsGenericType);
                Debug.Assert(!exprType.IsGenericTypeDefinition);
                Debug.Assert(exprType.GetGenericTypeDefinition() == typeof(Func<,>));

                Type[] args = exprType.GetGenericArguments();

                _invokeTypedExpressionHelper1.MakeGenericMethod(args[0], args[1]).Invoke(null, new object[] { bindingExpr, _innerService });
            }

            public void DeleteStream(Uri streamUri)
            {
                _innerService.GetStream<object, object>(streamUri).Dispose();
            }

            public IReactiveObserver<T> GetObserver<T>(Uri observerUri)
            {
                return _innerService.GetObserver<T>(observerUri);
            }

            public void DefineObservable(Uri observableUri, Expression observable, object state)
            {
                UriToReactiveBinder binder = new UriToReactiveBinder();
                var bindingExpr = binder.BindObservable(observable, observableUri, state);
                ((Expression<Action<IReactive>>)bindingExpr).Compile()(_innerService);
            }

            public void UndefineObservable(Uri observableUri)
            {
                _innerService.UndefineObservable(observableUri);
            }

            public void DefineObserver(Uri observerUri, Expression observer, object state)
            {
                UriToReactiveBinder binder = new UriToReactiveBinder();
                var bindingExpr = binder.BindObserver(observer, observerUri, state);
                ((Expression<Action<IReactive>>)bindingExpr).Compile()(_innerService);
            }

            public void UndefineObserver(Uri observerUri)
            {
                _innerService.UndefineObserver(observerUri);
            }

            public void DefineStreamFactory(Uri streamFactoryUri, Expression streamFactory, object state)
            {
                throw new NotImplementedException();
            }

            public void UndefineStreamFactory(Uri streamFactoryUri)
            {
                _innerService.UndefineStreamFactory(streamFactoryUri);
            }

            public void DefineSubscriptionFactory(Uri subscriptionFactoryUri, Expression subscriptionFactory, object state)
            {
                throw new NotImplementedException();
            }

            public void UndefineSubscriptionFactory(Uri subscriptionFactoryUri)
            {
                _innerService.UndefineSubscriptionFactory(subscriptionFactoryUri);
            }

            public IQueryProvider Provider => _innerService.Subscriptions.Provider;

            private static TReturn InvokeTypedExpressionHelper<TParam1, TReturn>(Expression expr, TParam1 param1)
            {
                return ((Expression<Func<TParam1, TReturn>>)expr).Compile()(param1);
            }
        }

        private class ExpressionService : ReactiveExpressionServices
        {
            public ExpressionService()
                : base(typeof(IReactiveClient))
            {
            }

            public override Expression Normalize(Expression expression)
            {
                var normalized = base.Normalize(expression);
                var tupletized = new InvocationTupletizer().Visit(normalized);
                if (tupletized is LambdaExpression lambda)
                {
                    tupletized = ExpressionTupletizer.Pack(lambda);
                }

                return tupletized;
            }
        }

        // TODO: Move InvocationTupletizer to a common location.

        /// <summary>
        /// Tupletizer for unbound parameter invocation sites.
        /// </summary>
        private class InvocationTupletizer : ScopedExpressionVisitor<ParameterExpression>
        {
            /// <summary>
            /// Visits the children of the <see cref="T:System.Linq.Expressions.InvocationExpression" />.
            /// </summary>
            /// <param name="node">The expression to visit.</param>
            /// <returns>
            /// The modified expression, if it or any subexpression was modified; otherwise, returns the original expression.
            /// </returns>
            protected override Expression VisitInvocation(InvocationExpression node)
            {
                var expr = Visit(node.Expression);
                var args = Visit(node.Arguments);

                if (expr.NodeType == ExpressionType.Parameter)
                {
                    var parameter = (ParameterExpression)expr;

                    // Turns f(x, y, z) into f((x, y, z)) when f is an unbound parameter, i.e. representing a known resource.
                    if (IsUnboundParameter(parameter))
                    {
                        if (args.Count > 0)
                        {
                            var tuple = ExpressionTupletizer.Pack(args);
                            var funcType = Expression.GetDelegateType(tuple.Type, node.Type);
                            var function = Expression.Parameter(funcType, parameter.Name);
                            return Expression.Invoke(function, tuple);
                        }
                    }
                }

                return node.Update(expr, args);
            }

            /// <summary>
            /// Gets the state associated with the specified parameter declaration.
            /// </summary>
            /// <param name="parameter">The parameter to obtain associated state for.</param>
            /// <returns>State associated with the specified parameter declaration.</returns>
            protected override ParameterExpression GetState(ParameterExpression parameter) => parameter;

            /// <summary>
            /// Determines whether the specified parameter is unbound.
            /// </summary>
            /// <param name="parameter">The parameter to check.</param>
            /// <returns>
            ///   <c>true</c> if the specified parameter is unbound; otherwise, <c>false</c>.
            /// </returns>
            private bool IsUnboundParameter(ParameterExpression parameter) => !TryLookup(parameter, out _);
        }
    }
}
