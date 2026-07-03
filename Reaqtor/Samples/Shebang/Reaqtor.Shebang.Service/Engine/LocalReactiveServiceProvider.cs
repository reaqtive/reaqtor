// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.CompilerServices;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Reaqtor.Shebang.Service
{
    public class LocalReactiveServiceProvider : IReactiveServiceProvider
    {
        private readonly IReactiveServiceProvider _provider;

        public LocalReactiveServiceProvider(IReactiveServiceProvider provider)
        {
            _provider = provider;
        }

        public IQueryProvider Provider => _provider.Provider;

        public Task CreateStreamAsync(Uri streamUri, Expression stream, object state, CancellationToken token)
        {
            return _provider.CreateStreamAsync(streamUri, Rewrite(stream), state, token);
        }

        public Task CreateSubscriptionAsync(Uri subscriptionUri, Expression subscription, object state, CancellationToken token)
        {
            return _provider.CreateSubscriptionAsync(subscriptionUri, Rewrite(subscription), state, token);
        }

        public Task DefineObservableAsync(Uri observableUri, Expression observable, object state, CancellationToken token)
        {
            return _provider.DefineObservableAsync(observableUri, Rewrite(observable), state, token);
        }

        public Task DefineObserverAsync(Uri observerUri, Expression observer, object state, CancellationToken token)
        {
            return _provider.DefineObserverAsync(observerUri, Rewrite(observer), state, token);
        }

        public Task DefineStreamFactoryAsync(Uri streamFactoryUri, Expression streamFactory, object state, CancellationToken token)
        {
            return _provider.DefineStreamFactoryAsync(streamFactoryUri, Rewrite(streamFactory), state, token);
        }

        public Task DefineSubscriptionFactoryAsync(Uri subscriptionFactoryUri, Expression subscriptionFactory, object state, CancellationToken token)
        {
            return _provider.DefineSubscriptionFactoryAsync(subscriptionFactoryUri, Rewrite(subscriptionFactory), state, token);
        }

        public Task DeleteStreamAsync(Uri streamUri, CancellationToken token)
        {
            return _provider.DeleteStreamAsync(streamUri, token);
        }

        public Task DeleteSubscriptionAsync(Uri subscriptionUri, CancellationToken token)
        {
            return _provider.DeleteSubscriptionAsync(subscriptionUri, token);
        }

        public Task<IAsyncReactiveObserver<T>> GetObserverAsync<T>(Uri observerUri, CancellationToken token)
        {
            return _provider.GetObserverAsync<T>(observerUri, token);
        }

        public Task UndefineObservableAsync(Uri observableUri, CancellationToken token)
        {
            return _provider.UndefineObservableAsync(observableUri, token);
        }

        public Task UndefineObserverAsync(Uri observerUri, CancellationToken token)
        {
            return _provider.UndefineObserverAsync(observerUri, token);
        }

        public Task UndefineStreamFactoryAsync(Uri streamFactoryUri, CancellationToken token)
        {
            return _provider.UndefineStreamFactoryAsync(streamFactoryUri, token);
        }

        public Task UndefineSubscriptionFactoryAsync(Uri subscriptionFactoryUri, CancellationToken token)
        {
            return _provider.UndefineSubscriptionFactoryAsync(subscriptionFactoryUri, token);
        }

        private static readonly Dictionary<Type, Type> s_map = new()
        {
            { typeof(IReactiveClientProxy), typeof(IReactiveClient) },
            { typeof(IReactiveDefinitionProxy), typeof(IReactiveDefinition) },
            { typeof(IReactiveMetadataProxy), typeof(IReactiveMetadata) },
            { typeof(IReactiveProxy), typeof(IReactive) },
        };


        private static Expression Rewrite(Expression expression)
        {
            var tuplitized = Tupletize(expression);

            var rw = new AsyncToSyncRewriter(s_map);

            return rw.Rewrite(tuplitized);
        }

        private static Expression Tupletize(Expression expression)
        {
            var inv = new InvocationTupletizer();
            var result = inv.Visit(expression);
            if (result is LambdaExpression lambda)
            {
                result = ExpressionTupletizer.Pack(lambda);
            }

            return result;
        }

        private sealed class InvocationTupletizer : ScopedExpressionVisitor<ParameterExpression>
        {
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

            protected override ParameterExpression GetState(ParameterExpression parameter) => parameter;

            private bool IsUnboundParameter(ParameterExpression parameter) => !TryLookup(parameter, out _);
        }

        private sealed class QueryProvider : IQueryProvider
        {
            private readonly IQueryProvider _provider;

            public QueryProvider(IQueryProvider provider) => _provider = provider;

            public IQueryable CreateQuery(Expression expression)
            {
                return _provider.CreateQuery(Rewrite(expression));
            }

            public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
            {
                return _provider.CreateQuery<TElement>(Rewrite(expression));
            }

            public object Execute(Expression expression)
            {
                return _provider.Execute(Rewrite(expression));
            }

            public TResult Execute<TResult>(Expression expression)
            {
                return _provider.Execute<TResult>(Rewrite(expression));
            }
        }
    }
}
