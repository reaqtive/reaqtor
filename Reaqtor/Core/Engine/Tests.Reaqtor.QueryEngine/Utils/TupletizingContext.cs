// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Diagnostics;
using System.Linq.CompilerServices;
using System.Linq.Expressions;
using System.Reflection;

using Reaqtor;
using Reaqtor.Service.Core;

namespace Tests.Reaqtor.QueryEngine;

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
            var tupletized = ExpressionTupletization.Tupletize(normalized);

            var sync = new AsyncToSyncRewriter(new Dictionary<Type, Type>()).Rewrite(tupletized);

            return sync;
        }
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

            _invokeTypedExpressionHelper1.MakeGenericMethod(args[0], args[1]).Invoke(null, [bindingExpr, _innerService]);
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
            return ExpressionTupletization.Tupletize(normalized);
        }
    }
}
