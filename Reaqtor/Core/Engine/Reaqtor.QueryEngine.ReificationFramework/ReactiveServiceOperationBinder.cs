// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1062 // Validate arguments of public methods. (Omitting null checks for protected methods.)

using System;
using System.Collections.Generic;
using System.Linq.CompilerServices;
using System.Linq.CompilerServices.TypeSystem;
using System.Linq.Expressions;
using System.Reflection;

using Reaqtor.Service.Core;
using Reaqtor.TestingFramework;

namespace Reaqtor.QueryEngine.ReificationFramework
{
    public class ReactiveServiceOperationBinder : ServiceOperationVisitor<Expression<Action<IReactive>>>
    {
        private static readonly Expression<Action<Uri, IReactive>> s_undefineObservableExpr = (uri, ctx) => ctx.UndefineObservable(uri);
        private static readonly Expression<Action<Uri, IReactive>> s_undefineObserverExpr = (uri, ctx) => ctx.UndefineObserver(uri);
        private static readonly Expression<Action<Uri, IReactive>> s_undefineStreamFactoryExpr = (uri, ctx) => ctx.UndefineStreamFactory(uri);
        private static readonly Expression<Action<Uri, IReactive>> s_undefineSubscriptionFactoryExpr = (uri, ctx) => ctx.UndefineSubscriptionFactory(uri);
        private static readonly Expression<Action<Uri, IReactive>> s_disposeStreamExpr = (uri, ctx) => ctx.GetStream<T1, T2>(uri).Dispose();
        private static readonly Expression<Action<Uri, IReactive>> s_disposeSubscriptionExpr = (uri, ctx) => ctx.GetSubscription(uri).Dispose();

        private static readonly ParameterExpression s_this = Expression.Parameter(typeof(IReactive), Constants.CurrentInstanceUri);

        //private static readonly MethodInfo s_getObserverGenericMethod = ((MethodInfo)ReflectionHelpers.InfoOf((IReactiveProxy ctx) => ctx.GetObserver<object>(default(Uri)))).GetGenericMethodDefinition();

        private static readonly AsyncToSyncRewriter _asyncToSync = new(new Dictionary<Type, Type>
        {
            { typeof(IReactiveClientProxy), typeof(IReactiveClient) },
            { typeof(IReactiveDefinitionProxy), typeof(IReactiveDefinition) },
            { typeof(IReactiveMetadataProxy), typeof(IReactiveMetadata) },
            { typeof(IReactiveProxy), typeof(IReactive) },
        });

        protected override Expression<Action<IReactive>> VisitCreateSubscription(CreateSubscription operation)
        {
            var binder = new IdentityAwareBinder();
            var boundSubscription = (LambdaExpression)binder.BindSubscription(Rewrite(operation.Expression), operation.TargetObjectUri, operation.State);
            var parameter = boundSubscription.Parameters[0];
            return Reduce(
                Expression.Lambda<Action<IReactive>>(
                    boundSubscription.Body,
                    parameter
                )
            );
        }

        protected override Expression<Action<IReactive>> VisitCreateStream(CreateStream operation)
        {
            var binder = new IdentityAwareBinder();
            var boundSubject = (LambdaExpression)binder.BindSubject(Rewrite(operation.Expression), operation.TargetObjectUri, operation.State);
            var parameter = boundSubject.Parameters[0];
            return Reduce(
                Expression.Lambda<Action<IReactive>>(
                    boundSubject.Body,
                    parameter
                )
            );
        }

        protected override Expression<Action<IReactive>> VisitDefineObservable(DefineObservable operation)
        {
            var binder = new IdentityAwareBinder();
            return Reduce((Expression<Action<IReactive>>)binder.BindObservable(Rewrite(operation.Expression), operation.TargetObjectUri, operation.State));
        }

        protected override Expression<Action<IReactive>> VisitDefineObserver(DefineObserver operation)
        {
            var binder = new IdentityAwareBinder();
            return Reduce((Expression<Action<IReactive>>)binder.BindObserver(Rewrite(operation.Expression), operation.TargetObjectUri, operation.State));

        }

        protected override Expression<Action<IReactive>> VisitDeleteStream(DeleteStream operation)
        {
            return Reduce(s_this.Let(@this =>
                Expression.Lambda<Action<IReactive>>(
                    Expression.Invoke(
                        s_disposeStreamExpr,
                        Expression.Constant(operation.TargetObjectUri),
                        @this),
                    @this)));
        }

        protected override Expression<Action<IReactive>> VisitDeleteSubscription(DeleteSubscription operation)
        {
            return Reduce(s_this.Let(@this =>
                Expression.Lambda<Action<IReactive>>(
                    Expression.Invoke(
                        s_disposeSubscriptionExpr,
                        Expression.Constant(operation.TargetObjectUri),
                        @this),
                    @this)));
        }

        protected override Expression<Action<IReactive>> VisitObserverOnNextCore<T>(ObserverOnNext<T> operation)
        {
            var binder = new IdentityAwareBinder();
            var boundParameter = (LambdaExpression)binder.Bind(Expression.Parameter(typeof(IReactiveQbserver<T>), operation.TargetObjectUri.ToCanonicalString()));
            var thisParameter = boundParameter.Parameters[0];
            var onNextMethod = (MethodInfo)ReflectionHelpers.InfoOf((IReactiveObserver<T> iv) => iv.OnNext(default));
            return Expression.Lambda<Action<IReactive>>(
                Expression.Call(
                    boundParameter.Body,
                    onNextMethod,
                    Expression.Constant(operation.Value, typeof(T))
                ),
                thisParameter);
        }

        protected override Expression<Action<IReactive>> VisitObserverOnErrorCore<T>(ObserverOnError<T> operation)
        {
            var binder = new IdentityAwareBinder();
            var boundParameter = (LambdaExpression)binder.Bind(Expression.Parameter(typeof(IReactiveQbserver<T>), operation.TargetObjectUri.ToCanonicalString()));
            var thisParameter = boundParameter.Parameters[0];
            var onErrorMethod = (MethodInfo)ReflectionHelpers.InfoOf((IReactiveObserver<T> iv) => iv.OnError(default));
            return Expression.Lambda<Action<IReactive>>(
                Expression.Call(
                    boundParameter.Body,
                    onErrorMethod,
                    Expression.Constant(operation.Error, typeof(Exception))
                ),
                thisParameter);
        }

        protected override Expression<Action<IReactive>> VisitObserverOnCompletedCore<T>(ObserverOnCompleted<T> operation)
        {
            var binder = new IdentityAwareBinder();
            var boundParameter = (LambdaExpression)binder.Bind(Expression.Parameter(typeof(IReactiveQbserver<T>), operation.TargetObjectUri.ToCanonicalString()));
            var thisParameter = boundParameter.Parameters[0];
            var onCompletedMethod = (MethodInfo)ReflectionHelpers.InfoOf((IReactiveObserver<T> iv) => iv.OnCompleted());
            return Expression.Lambda<Action<IReactive>>(
                Expression.Call(
                    boundParameter.Body,
                    onCompletedMethod
                ),
                thisParameter);
        }

        protected override Expression<Action<IReactive>> VisitUndefineObservable(UndefineObservable operation)
        {
            return Reduce(s_this.Let(@this =>
                Expression.Lambda<Action<IReactive>>(
                    Expression.Invoke(
                        s_undefineObservableExpr,
                        Expression.Constant(operation.TargetObjectUri),
                        @this),
                    @this)));
        }

        protected override Expression<Action<IReactive>> VisitUndefineObserver(UndefineObserver operation)
        {
            return Reduce(s_this.Let(@this =>
                Expression.Lambda<Action<IReactive>>(
                    Expression.Invoke(
                        s_undefineObserverExpr,
                        Expression.Constant(operation.TargetObjectUri),
                        @this),
                    @this)));
        }

        protected override Expression<Action<IReactive>> VisitUndefineStreamFactory(UndefineStreamFactory operation)
        {
            return Reduce(s_this.Let(@this =>
                Expression.Lambda<Action<IReactive>>(
                    Expression.Invoke(
                        s_undefineStreamFactoryExpr,
                        Expression.Constant(operation.TargetObjectUri),
                        @this),
                    @this)));
        }

        protected override Expression<Action<IReactive>> VisitUndefineSubscriptionFactory(UndefineSubscriptionFactory operation)
        {
            return Reduce(s_this.Let(@this =>
                Expression.Lambda<Action<IReactive>>(
                    Expression.Invoke(
                        s_undefineSubscriptionFactoryExpr,
                        Expression.Constant(operation.TargetObjectUri),
                        @this),
                    @this)));
        }

        #region Not Implemented

        protected override Expression<Action<IReactive>> VisitCreateObserver(CreateObserver operation)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Action<IReactive>> VisitDefineStreamFactory(DefineStreamFactory operation)
        {
            throw new InvalidOperationException();
        }

        protected override Expression<Action<IReactive>> VisitDefineSubscriptionFactory(DefineSubscriptionFactory operation)
        {
            throw new InvalidOperationException();
        }

        protected override Expression<Action<IReactive>> VisitDeleteObservableMetadata(DeleteObservableMetadata operation)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Action<IReactive>> VisitDeleteObserverMetadata(DeleteObserverMetadata operation)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Action<IReactive>> VisitDeleteStreamFactoryMetadata(DeleteStreamFactoryMetadata operation)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Action<IReactive>> VisitDeleteSubscriptionFactoryMetadata(DeleteSubscriptionFactoryMetadata operation)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Action<IReactive>> VisitDeleteStreamMetadata(DeleteStreamMetadata operation)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Action<IReactive>> VisitDeleteSubscriptionMetadata(DeleteSubscriptionMetadata operation)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Action<IReactive>> VisitInsertObservableMetadata(InsertObservableMetadata operation)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Action<IReactive>> VisitInsertObserverMetadata(InsertObserverMetadata operation)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Action<IReactive>> VisitInsertStreamFactoryMetadata(InsertStreamFactoryMetadata operation)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Action<IReactive>> VisitInsertSubscriptionFactoryMetadata(InsertSubscriptionFactoryMetadata operation)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Action<IReactive>> VisitInsertStreamMetadata(InsertStreamMetadata operation)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Action<IReactive>> VisitInsertSubscriptionMetadata(InsertSubscriptionMetadata operation)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Action<IReactive>> VisitLookupObservableMetadata(LookupObservableMetadata operation)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Action<IReactive>> VisitLookupObserverMetadata(LookupObserverMetadata operation)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Action<IReactive>> VisitLookupStreamFactoryMetadata(LookupStreamFactoryMetadata operation)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Action<IReactive>> VisitLookupSubscriptionFactoryMetadata(LookupSubscriptionFactoryMetadata operation)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Action<IReactive>> VisitLookupStreamMetadata(LookupStreamMetadata operation)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Action<IReactive>> VisitLookupSubscriptionMetadata(LookupSubscriptionMetadata operation)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Action<IReactive>> VisitMetadataQuery(MetadataQuery operation)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Action<IReactive>> VisitObserverOnNextCore(ObserverOnNext operation)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Action<IReactive>> VisitObserverOnErrorCore(ObserverOnError operation)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Action<IReactive>> VisitObserverOnCompletedCore(ObserverOnCompleted operation)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Action<IReactive>> VisitExtensions(ServiceOperation operation)
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

        private static Expression Rewrite(Expression expression)
        {
            return _asyncToSync.Rewrite(expression);
        }

        private sealed class IdentityAwareBinder : UriToReactiveBinder
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

    internal static class ObjectExtensions
    {
        [KnownResource("rx://builtin/id")]
        public static TOutput To<TInput, TOutput>(this TInput input)
        {
            return (TOutput)(object)input;
        }
    }
}
