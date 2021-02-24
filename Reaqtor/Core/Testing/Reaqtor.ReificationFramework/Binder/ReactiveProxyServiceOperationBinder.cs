// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using System.Linq.CompilerServices;
using System.Linq.CompilerServices.TypeSystem;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using Reaqtor.TestingFramework;

namespace Reaqtor.ReificationFramework
{
    internal class ReactiveProxyServiceOperationBinder : ServiceOperationVisitor<Expression<Func<IReactiveProxy, Task>>>
    {
        private static readonly Expression<Func<Uri, IReactiveProxy, Task>> s_undefineObservableExpr = (uri, ctx) => ctx.UndefineObservableAsync(uri, CancellationToken.None);
        private static readonly Expression<Func<Uri, IReactiveProxy, Task>> s_undefineObserverExpr = (uri, ctx) => ctx.UndefineObserverAsync(uri, CancellationToken.None);
        private static readonly Expression<Func<Uri, IReactiveProxy, Task>> s_undefineStreamFactoryExpr = (uri, ctx) => ctx.UndefineStreamFactoryAsync(uri, CancellationToken.None);
        private static readonly Expression<Func<Uri, IReactiveProxy, Task>> s_undefineSubscriptionFactoryExpr = (uri, ctx) => ctx.UndefineSubscriptionFactoryAsync(uri, CancellationToken.None);

#if NET5_0 || NETSTANDARD2_1
        private static readonly Expression<Func<Uri, IReactiveProxy, Task>> s_disposeStreamExpr = (uri, ctx) => ctx.GetStream<T1, T2>(uri).DisposeAsync().AsTask();
        private static readonly Expression<Func<Uri, IReactiveProxy, Task>> s_disposeSubscriptionExpr = (uri, ctx) => ctx.GetSubscription(uri).DisposeAsync().AsTask();
#else
        private static readonly Expression<Func<Uri, IReactiveProxy, Task>> s_disposeStreamExpr = (uri, ctx) => ctx.GetStream<T1, T2>(uri).DisposeAsync(CancellationToken.None);
        private static readonly Expression<Func<Uri, IReactiveProxy, Task>> s_disposeSubscriptionExpr = (uri, ctx) => ctx.GetSubscription(uri).DisposeAsync(CancellationToken.None);
#endif

        private static readonly ParameterExpression s_this = Expression.Parameter(typeof(IReactiveProxy), Constants.CurrentInstanceUri);

        //private static readonly MethodInfo s_getObserverGenericMethod = ((MethodInfo)ReflectionHelpers.InfoOf((IReactiveProxy ctx) => ctx.GetObserver<object>(default(Uri)))).GetGenericMethodDefinition();

        protected override Expression<Func<IReactiveProxy, Task>> VisitCreateSubscription(CreateSubscription operation)
        {
            var binder = new IdentityAwareBinder();
            var boundSubscription = (LambdaExpression)binder.BindSubscription(operation.Expression, operation.TargetObjectUri, operation.State, CancellationToken.None);
            var parameter = boundSubscription.Parameters[0];
            return Reduce(
                Expression.Lambda<Func<IReactiveProxy, Task>>(
                    Expression.Convert(
                        boundSubscription.Body,
                        typeof(Task)
                    ),
                    parameter
                )
            );
        }

        protected override Expression<Func<IReactiveProxy, Task>> VisitCreateStream(CreateStream operation)
        {
            var binder = new IdentityAwareBinder();
            var boundStream = (LambdaExpression)binder.BindStream(operation.Expression, operation.TargetObjectUri, operation.State, CancellationToken.None);
            var parameter = boundStream.Parameters[0];
            return Reduce(
                Expression.Lambda<Func<IReactiveProxy, Task>>(
                    Expression.Convert(
                        boundStream.Body,
                        typeof(Task)
                    ),
                    parameter
                )
            );
        }

        protected override Expression<Func<IReactiveProxy, Task>> VisitDefineObservable(DefineObservable operation)
        {
            var binder = new IdentityAwareBinder();
            return Reduce((Expression<Func<IReactiveProxy, Task>>)binder.BindObservable(operation.Expression, operation.TargetObjectUri, operation.State, CancellationToken.None));
        }

        protected override Expression<Func<IReactiveProxy, Task>> VisitDefineObserver(DefineObserver operation)
        {
            var binder = new IdentityAwareBinder();
            return Reduce((Expression<Func<IReactiveProxy, Task>>)binder.BindObserver(operation.Expression, operation.TargetObjectUri, operation.State, CancellationToken.None));
        }

        protected override Expression<Func<IReactiveProxy, Task>> VisitDeleteStream(DeleteStream operation)
        {
            return Reduce(s_this.Let(@this =>
                Expression.Lambda<Func<IReactiveProxy, Task>>(
                    Expression.Invoke(
                        s_disposeStreamExpr,
                        Expression.Constant(operation.TargetObjectUri),
                        @this),
                    @this)));
        }

        protected override Expression<Func<IReactiveProxy, Task>> VisitDeleteSubscription(DeleteSubscription operation)
        {
            return Reduce(s_this.Let(@this =>
                Expression.Lambda<Func<IReactiveProxy, Task>>(
                    Expression.Invoke(
                        s_disposeSubscriptionExpr,
                        Expression.Constant(operation.TargetObjectUri),
                        @this),
                    @this)));
        }

        protected override Expression<Func<IReactiveProxy, Task>> VisitObserverOnNextCore<T>(ObserverOnNext<T> operation)
        {
            var binder = new IdentityAwareBinder();
            var boundParameter = binder.Bind(Expression.Parameter(typeof(IAsyncReactiveQbserver<T>), operation.TargetObjectUri.ToCanonicalString()));
            var thisParameter = FreeVariableScanner.Scan(boundParameter).Single();
            var onNextMethod = boundParameter.Type.GetMethod("OnNextAsync");
            return Expression.Lambda<Func<IReactiveProxy, Task>>(
                Expression.Call(
                    boundParameter,
                    onNextMethod,
                    Expression.Constant(operation.Value, typeof(T)),
                    Expression.Constant(CancellationToken.None)),
                thisParameter);
        }

        protected override Expression<Func<IReactiveProxy, Task>> VisitObserverOnErrorCore<T>(ObserverOnError<T> operation)
        {
            var binder = new IdentityAwareBinder();
            var boundParameter = binder.Bind(Expression.Parameter(typeof(IAsyncReactiveQbserver<T>), operation.TargetObjectUri.ToCanonicalString()));
            var thisParameter = FreeVariableScanner.Scan(boundParameter).Single();
            var onErrorMethod = boundParameter.Type.GetMethod("OnErrorAsync");
            return Expression.Lambda<Func<IReactiveProxy, Task>>(
                Expression.Call(
                    boundParameter,
                    onErrorMethod,
                    Expression.Constant(operation.Error, typeof(Exception)),
                    Expression.Constant(CancellationToken.None)),
                thisParameter);
        }

        protected override Expression<Func<IReactiveProxy, Task>> VisitObserverOnCompletedCore<T>(ObserverOnCompleted<T> operation)
        {
            var binder = new IdentityAwareBinder();
            var boundParameter = binder.Bind(Expression.Parameter(typeof(IAsyncReactiveQbserver<T>), operation.TargetObjectUri.ToCanonicalString()));
            var thisParameter = FreeVariableScanner.Scan(boundParameter).Single();
            var onCompletedMethod = boundParameter.Type.GetMethod("OnCompletedAsync");
            return Expression.Lambda<Func<IReactiveProxy, Task>>(
                Expression.Call(
                    boundParameter,
                    onCompletedMethod,
                    Expression.Constant(CancellationToken.None)),
                thisParameter);
        }

        protected override Expression<Func<IReactiveProxy, Task>> VisitUndefineObservable(UndefineObservable operation)
        {
            return Reduce(s_this.Let(@this =>
                Expression.Lambda<Func<IReactiveProxy, Task>>(
                    Expression.Invoke(
                        s_undefineObservableExpr,
                        Expression.Constant(operation.TargetObjectUri),
                        @this),
                    @this)));
        }

        protected override Expression<Func<IReactiveProxy, Task>> VisitUndefineObserver(UndefineObserver operation)
        {
            return Reduce(s_this.Let(@this =>
                Expression.Lambda<Func<IReactiveProxy, Task>>(
                    Expression.Invoke(
                        s_undefineObserverExpr,
                        Expression.Constant(operation.TargetObjectUri),
                        @this),
                    @this)));
        }

        protected override Expression<Func<IReactiveProxy, Task>> VisitUndefineStreamFactory(UndefineStreamFactory operation)
        {
            return Reduce(s_this.Let(@this =>
                Expression.Lambda<Func<IReactiveProxy, Task>>(
                    Expression.Invoke(
                        s_undefineStreamFactoryExpr,
                        Expression.Constant(operation.TargetObjectUri),
                        @this),
                    @this)));
        }

        protected override Expression<Func<IReactiveProxy, Task>> VisitUndefineSubscriptionFactory(UndefineSubscriptionFactory operation)
        {
            return Reduce(s_this.Let(@this =>
                Expression.Lambda<Func<IReactiveProxy, Task>>(
                    Expression.Invoke(
                        s_undefineSubscriptionFactoryExpr,
                        Expression.Constant(operation.TargetObjectUri),
                        @this),
                    @this)));
        }

        #region Not Implemented

        protected override Expression<Func<IReactiveProxy, Task>> VisitCreateObserver(CreateObserver operation)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Func<IReactiveProxy, Task>> VisitDefineStreamFactory(DefineStreamFactory operation)
        {
            throw new InvalidOperationException();
        }

        protected override Expression<Func<IReactiveProxy, Task>> VisitDefineSubscriptionFactory(DefineSubscriptionFactory operation)
        {
            throw new InvalidOperationException();
        }

        protected override Expression<Func<IReactiveProxy, Task>> VisitDeleteObservableMetadata(DeleteObservableMetadata operation)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Func<IReactiveProxy, Task>> VisitDeleteObserverMetadata(DeleteObserverMetadata operation)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Func<IReactiveProxy, Task>> VisitDeleteStreamFactoryMetadata(DeleteStreamFactoryMetadata operation)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Func<IReactiveProxy, Task>> VisitDeleteSubscriptionFactoryMetadata(DeleteSubscriptionFactoryMetadata operation)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Func<IReactiveProxy, Task>> VisitDeleteStreamMetadata(DeleteStreamMetadata operation)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Func<IReactiveProxy, Task>> VisitDeleteSubscriptionMetadata(DeleteSubscriptionMetadata operation)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Func<IReactiveProxy, Task>> VisitInsertObservableMetadata(InsertObservableMetadata operation)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Func<IReactiveProxy, Task>> VisitInsertObserverMetadata(InsertObserverMetadata operation)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Func<IReactiveProxy, Task>> VisitInsertStreamFactoryMetadata(InsertStreamFactoryMetadata operation)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Func<IReactiveProxy, Task>> VisitInsertSubscriptionFactoryMetadata(InsertSubscriptionFactoryMetadata operation)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Func<IReactiveProxy, Task>> VisitInsertStreamMetadata(InsertStreamMetadata operation)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Func<IReactiveProxy, Task>> VisitInsertSubscriptionMetadata(InsertSubscriptionMetadata operation)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Func<IReactiveProxy, Task>> VisitLookupObservableMetadata(LookupObservableMetadata operation)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Func<IReactiveProxy, Task>> VisitLookupObserverMetadata(LookupObserverMetadata operation)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Func<IReactiveProxy, Task>> VisitLookupStreamFactoryMetadata(LookupStreamFactoryMetadata operation)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Func<IReactiveProxy, Task>> VisitLookupSubscriptionFactoryMetadata(LookupSubscriptionFactoryMetadata operation)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Func<IReactiveProxy, Task>> VisitLookupStreamMetadata(LookupStreamMetadata operation)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Func<IReactiveProxy, Task>> VisitLookupSubscriptionMetadata(LookupSubscriptionMetadata operation)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Func<IReactiveProxy, Task>> VisitMetadataQuery(MetadataQuery operation)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Func<IReactiveProxy, Task>> VisitObserverOnNextCore(ObserverOnNext operation)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Func<IReactiveProxy, Task>> VisitObserverOnErrorCore(ObserverOnError operation)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Func<IReactiveProxy, Task>> VisitObserverOnCompletedCore(ObserverOnCompleted operation)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Func<IReactiveProxy, Task>> VisitExtensions(ServiceOperation operation)
        {
            throw new NotImplementedException();
        }

        #endregion

        private static Expression Reduce(Expression expression)
        {
            return BetaReducer.ReduceEager(
                expression,
                BetaReductionNodeTypes.Unrestricted,
                BetaReductionRestrictions.None,
                true);
        }

        private static Expression<T> Reduce<T>(Expression<T> expression)
        {
            return (Expression<T>)Reduce((Expression)expression);
        }

        private sealed class IdentityAwareBinder : UriToReactiveProxyBinder
        {
            private static readonly MethodInfo s_toMethod = ((MethodInfo)ReflectionHelpers.InfoOf((object obj) => obj.To<object, object>())).GetGenericMethodDefinition();

            protected override Expression Lookup(string id, Type type, Type funcType)
            {
                if (id == Constants.IdentityFunctionUri)
                {
                    var method = s_toMethod.MakeGenericMethod(funcType.GenericTypeArguments);
                    var parameter = Expression.Parameter(funcType.GenericTypeArguments[0]);
                    return Expression.Lambda(Expression.Call(method, parameter), parameter);
                }

                return base.Lookup(id, type, funcType);
            }
        }
    }

    internal static class Extensions
    {
        [KnownResource(Constants.IdentityFunctionUri)]
        public static TOutput To<TInput, TOutput>(this TInput input)
        {
            return (TOutput)(object)input;
        }
    }
}
