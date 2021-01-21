// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;
using System.Linq;
using System.Linq.CompilerServices;
using System.Linq.Expressions;
using System.Reflection;

namespace Reaqtor.Service.Core
{
    /// <summary>
    /// Binder to bind URI-based artifacts to an IReactive context.
    /// </summary>
    public class UriToReactiveBinder : UriToReactiveBinderBase
    {
        private static readonly MethodInfo _getStreamFactory = ((MethodInfo)ReflectionHelpers.InfoOf((IReactive r) => r.GetStreamFactory<object, object>(null))).GetGenericMethodDefinition();
        private static readonly MethodInfo _getStreamFactoryParam = ((MethodInfo)ReflectionHelpers.InfoOf((IReactive r) => r.GetStreamFactory<object, object, object>(null))).GetGenericMethodDefinition();

        private static readonly MethodInfo _getSubscriptionFactory = (MethodInfo)ReflectionHelpers.InfoOf((IReactive r) => r.GetSubscriptionFactory(null));
        private static readonly MethodInfo _getSubscriptionFactoryParam = ((MethodInfo)ReflectionHelpers.InfoOf((IReactive r) => r.GetSubscriptionFactory<object>(null))).GetGenericMethodDefinition();

        private static readonly MethodInfo _getStream = ((MethodInfo)ReflectionHelpers.InfoOf((IReactive r) => r.GetStream<object, object>(null))).GetGenericMethodDefinition();

        private static readonly MethodInfo _getObservable = ((MethodInfo)ReflectionHelpers.InfoOf((IReactive r) => r.GetObservable<object>(null))).GetGenericMethodDefinition();
        private static readonly MethodInfo _getObservableParam = ((MethodInfo)ReflectionHelpers.InfoOf((IReactive r) => r.GetObservable<object, object>(null))).GetGenericMethodDefinition();

        private static readonly MethodInfo _getObserver = ((MethodInfo)ReflectionHelpers.InfoOf((IReactive r) => r.GetObserver<object>(null))).GetGenericMethodDefinition();
        private static readonly MethodInfo _getObserverParam = ((MethodInfo)ReflectionHelpers.InfoOf((IReactive r) => r.GetObserver<object, object>(null))).GetGenericMethodDefinition();

        private static readonly MethodInfo _getSubscription = ((MethodInfo)ReflectionHelpers.InfoOf((IReactive r) => r.GetSubscription(null)));

        private static readonly MethodInfo _defineObservable = ((MethodInfo)ReflectionHelpers.InfoOf((IReactive r) => r.DefineObservable<object>(null, null, null))).GetGenericMethodDefinition();
        private static readonly MethodInfo _defineObservableParam = ((MethodInfo)ReflectionHelpers.InfoOf((IReactive r) => r.DefineObservable<object, object>(null, null, null))).GetGenericMethodDefinition();

        private static readonly MethodInfo _defineObserver = ((MethodInfo)ReflectionHelpers.InfoOf((IReactive r) => r.DefineObserver<object>(null, null, null))).GetGenericMethodDefinition();
        private static readonly MethodInfo _defineObserverParam = ((MethodInfo)ReflectionHelpers.InfoOf((IReactive r) => r.DefineObserver<object, object>(null, null, null))).GetGenericMethodDefinition();

        /// <summary>
        /// Gets the type of this 'this' reference, i.e. IReactive.
        /// </summary>
        protected override Type ThisType => typeof(IReactive);

        /// <summary>
        /// Binds a subscription creation expression to the IReactive context.
        /// </summary>
        /// <param name="expr">Expression representing the subscription creation expression.</param>
        /// <param name="subscriptionUri">URI passed to the Subscribe call.</param>
        /// <param name="state">State passed to the Subscribe call</param>
        /// <returns>Rewritten expression with the subscription creation call bound to the IReactive context.</returns>
        public Expression BindSubscription(Expression expr, Uri subscriptionUri, object state)
        {
            if (expr == null)
                throw new ArgumentNullException(nameof(expr));
            if (subscriptionUri == null)
                throw new ArgumentNullException(nameof(subscriptionUri));

            if (expr is not InvocationExpression invoke || invoke.Type != typeof(IReactiveQubscription) ||
                invoke.Expression is not ParameterExpression parameter || parameter.Name == null)
            {
                throw new InvalidOperationException("Expected invocation of subscription factory.");
            }

            if (parameter.Name == Constants.SubscribeUri)
            {
                return BindObservableSubscription(invoke, subscriptionUri, state);
            }

            return BindFactorySubscription(expr, subscriptionUri, state);
        }

        private Expression BindFactorySubscription(Expression expr, Uri subscriptionUri, object state)
        {
            var invoke = (InvocationExpression)expr;
            var parameter = (ParameterExpression)invoke.Expression;

            if (invoke.Arguments.Count is not (0 or 1))
            {
                throw new InvalidOperationException("Expected invocation of subscription factory.");
            }

            SetThisParameter(expr);

            BindingVisitor binder = new BindingVisitor(this);
            Expression subscriptionFactoryExpr = binder.Visit(parameter);

            var subscriptionUriExpr = Expression.Constant(subscriptionUri, typeof(Uri));
            var stateExpr = Expression.Constant(state, typeof(object));

            var create = subscriptionFactoryExpr.Type.GetMethod("Create");
            Debug.Assert(create != null);

            Expression createExpr;
            if (invoke.Arguments.Count == 0)
            {
                createExpr = Expression.Call(subscriptionFactoryExpr, create, subscriptionUriExpr, stateExpr);
            }
            else
            {
                Debug.Assert(invoke.Arguments.Count == 1);
                createExpr = Expression.Call(subscriptionFactoryExpr, create, subscriptionUriExpr, invoke.Arguments[0], stateExpr);
            }

            var res = (LambdaExpression)base.Bind(createExpr);

            if (res.Parameters.Count == 0)
            {
                res = Expression.Lambda(res.Body, ThisParameter);
            }

            return res;
        }

        private Expression BindObservableSubscription(InvocationExpression invoke, Uri subscriptionUri, object state)
        {
            if (invoke.Arguments.Count is not (1 or 2))
            {
                throw new InvalidOperationException("Expected invocation for subscribe.");
            }

            Expression observableExpr, observerExpr;

            if (invoke.Arguments.Count == 1)
            {
                if (invoke.Arguments[0] is not NewExpression tupleArg || tupleArg.Arguments.Count != 2)
                {
                    throw new InvalidOperationException("Expected subscription operation.");
                }

                observableExpr = tupleArg.Arguments[0];
                observerExpr = tupleArg.Arguments[1];
            }
            else
            {
                observableExpr = invoke.Arguments[0];
                observerExpr = invoke.Arguments[1];
            }

            if (observableExpr.Type.FindGenericType(typeof(IReactiveQbservable<>)) == null)
            {
                throw new InvalidOperationException("Expected observable argument passed to subscription operation.");
            }

            if (observerExpr.Type.FindGenericType(typeof(IReactiveQbserver<>)) == null)
            {
                throw new InvalidOperationException("Expected observer argument passed to subscription operation.");
            }

            var subscriptionUriExpr = Expression.Constant(subscriptionUri, typeof(Uri));
            var stateExpr = Expression.Constant(state, typeof(object));

            var subscribe = observableExpr.Type.GetMethod("Subscribe");
            Debug.Assert(subscribe != null);

            var subscribeExpr = Expression.Call(observableExpr, subscribe, observerExpr, subscriptionUriExpr, stateExpr);

            var res = base.Bind(subscribeExpr);

            return res;
        }

        /// <summary>
        /// Binds an observable definition expression to the IReactive context.
        /// </summary>
        /// <param name="expr">Expression representing the observable definition expression.</param>
        /// <param name="observableUri">URI passed to the Define call.</param>
        /// <param name="state">State passed to the Define call</param>
        /// <returns>Rewritten expression with the observable definition call bound to the IReactive context.</returns>
        public Expression BindObservable(Expression expr, Uri observableUri, object state)
        {
            if (expr == null)
                throw new ArgumentNullException(nameof(expr));
            if (observableUri == null)
                throw new ArgumentNullException(nameof(observableUri));

            Type type = expr.Type;
            if (!type.IsGenericType)
            {
                throw new InvalidOperationException("Expected observable or parameterized observable.");
            }

            MethodInfo defineObservableMethod;

            if (TryFindQbservableType(type, out Type qbservableType))
            {
                Type elementType = qbservableType.GetGenericArguments()[0];
                defineObservableMethod = _defineObservable.MakeGenericMethod(elementType);
            }
            else
            {
                Type genericType = type.GetGenericTypeDefinition();

                if (genericType == typeof(Func<,>))
                {
                    var genArgs = type.GetGenericArguments();

                    Type argType = genArgs[0];
                    Type observableType = genArgs[1];

                    if (!TryFindQbservableType(observableType, out qbservableType))
                    {
                        throw new InvalidOperationException("Unexpected parameterized observable type.");
                    }

                    Type elementType = qbservableType.GetGenericArguments()[0];

                    defineObservableMethod = _defineObservableParam.MakeGenericMethod(argType, elementType);
                }
                else
                {
                    throw new InvalidOperationException("Expected observable or parameterized observable.");
                }
            }

            var observableUriExpr = Expression.Constant(observableUri, typeof(Uri));
            var stateExpr = Expression.Constant(state, typeof(object));

            SetThisParameter(expr);

            var observableExpr = Expression.Call(ThisParameter, defineObservableMethod, observableUriExpr, expr, stateExpr);

            var res = (LambdaExpression)base.Bind(observableExpr);

            return res.Parameters.Count == 0 ? Expression.Lambda(res.Body, ThisParameter) : res;
        }

        /// <summary>
        /// Binds an observer definition expression to the IReactive context.
        /// </summary>
        /// <param name="expr">Expression representing the observer definition expression.</param>
        /// <param name="observerUri">URI passed to the Define call.</param>
        /// <param name="state">State passed to the Define call</param>
        /// <returns>Rewritten expression with the observer definition call bound to the IReactive context.</returns>
        public Expression BindObserver(Expression expr, Uri observerUri, object state)
        {
            if (expr == null)
                throw new ArgumentNullException(nameof(expr));
            if (observerUri == null)
                throw new ArgumentNullException(nameof(observerUri));

            Type type = expr.Type;
            if (!type.IsGenericType)
            {
                throw new InvalidOperationException("Expected observer or parameterized observer.");
            }

            MethodInfo defineObserverMethod;

            if (TryFindQbserverType(type, out Type qbserverType))
            {
                Type elementType = qbserverType.GetGenericArguments()[0];
                defineObserverMethod = _defineObserver.MakeGenericMethod(elementType);
            }
            else
            {
                Type genericType = type.GetGenericTypeDefinition();

                if (genericType == typeof(Func<,>))
                {
                    var genArgs = type.GetGenericArguments();

                    Type argType = genArgs[0];
                    Type observerType = genArgs[1];

                    if (!TryFindQbserverType(observerType, out qbserverType))
                    {
                        throw new InvalidOperationException("Unexpected parameterized observer type.");
                    }

                    Type elementType = qbserverType.GetGenericArguments()[0];

                    defineObserverMethod = _defineObserverParam.MakeGenericMethod(argType, elementType);
                }
                else
                {
                    throw new InvalidOperationException("Expected observer or parameterized observer.");
                }
            }

            var observerUriExpr = Expression.Constant(observerUri, typeof(Uri));
            var stateExpr = Expression.Constant(state, typeof(object));

            SetThisParameter(expr);

            var observerExpr = Expression.Call(ThisParameter, defineObserverMethod, observerUriExpr, expr, stateExpr);

            var res = (LambdaExpression)base.Bind(observerExpr);

            return res.Parameters.Count == 0 ? Expression.Lambda(res.Body, ThisParameter) : res;
        }

        /// <summary>
        /// Binds a stream creation expression to the IReactive context.
        /// </summary>
        /// <param name="expr">Expression representing the subscription creation expression.</param>
        /// <param name="streamUri">URI passed to the Create call.</param>
        /// <param name="state">State passed to the Create call</param>
        /// <returns>Rewritten expression with the subscription creation call bound to the IReactive context.</returns>
        public Expression BindSubject(Expression expr, Uri streamUri, object state)
        {
            if (expr == null)
                throw new ArgumentNullException(nameof(expr));
            if (streamUri == null)
                throw new ArgumentNullException(nameof(streamUri));

            if (expr is not InvocationExpression invoke ||
                !invoke.Type.IsGenericType ||
                invoke.Type.GetGenericTypeDefinition() != typeof(IReactiveQubject<,>) ||
                !(invoke.Arguments.Count == 0 || invoke.Arguments.Count == 1) ||
                invoke.Expression is not ParameterExpression parameter ||
                parameter.Name == null)
            {
                throw new InvalidOperationException("Expected invocation of subject factory.");
            }

            SetThisParameter(expr);

            BindingVisitor binder = new BindingVisitor(this);
            Expression subjectFactoryExpr = binder.Visit(parameter);

            var streamUriExpr = Expression.Constant(streamUri, typeof(Uri));
            var stateExpr = Expression.Constant(state, typeof(object));

            var create = subjectFactoryExpr.Type.GetMethod("Create");
            Debug.Assert(create != null);

            Expression createExpr;
            if (invoke.Arguments.Count == 0)
            {
                createExpr = Expression.Call(subjectFactoryExpr, create, streamUriExpr, stateExpr);
            }
            else
            {
                Debug.Assert(invoke.Arguments.Count == 1);
                createExpr = Expression.Call(subjectFactoryExpr, create, streamUriExpr, invoke.Arguments[0], stateExpr);
            }

            var res = (LambdaExpression)base.Bind(createExpr);

            if (res.Parameters.Count == 0)
            {
                res = Expression.Lambda(res.Body, ThisParameter);
            }

            return res;
        }

        /// <summary>
        /// Looks up the artifact with the specified identifier and type, returning its bound expression representation.
        /// </summary>
        /// <param name="id">Identifier representing the artifact.</param>
        /// <param name="type">Type of the artifact.</param>
        /// <param name="funcType">Function type for parameterized artifacts.</param>
        /// <returns>Bound expression representation of the artifact.</returns>
        protected override Expression Lookup(string id, Type type, Type funcType)
        {
            Debug.Assert(type != null);

            Expression result = null;

            if (type.IsGenericType)
            {
                if (TryFindQubjectType(type, out var qubjectType))
                {
                    if (funcType == null)
                    {
                        result = LookupStream(id, qubjectType);
                    }
                    else
                    {
                        result = LookupStreamFactory(id, qubjectType, funcType);
                    }
                }
                else if (TryFindQbservableType(type, out var qbservableType))
                {
                    result = LookupObservable(id, qbservableType, funcType);
                }
                else if (TryFindQbserverType(type, out var qbserverType))
                {
                    result = LookupObserver(id, qbserverType, funcType);
                }
            }
            else if (type == typeof(IReactiveQubscription))
            {
                if (funcType == null)
                {
                    result = LookupSubscription(id);
                }
                else
                {
                    result = LookupSubscriptionFactory(id, funcType);
                }
            }

            if (result == null)
            {
                throw new InvalidOperationException("Unexpected expression type.");
            }

            return result;
        }

        private Expression LookupStreamFactory(string id, Type type, Type funcType)
        {
            var args = type.GetGenericArguments();
            Debug.Assert(args.Length == 2);

            var funcArgs = funcType.GetGenericArguments();
            Debug.Assert(funcArgs.Length is 1 or 2);

            if (funcArgs.Length == 1)
            {
                return Expression.Call(
                    ThisParameter,
                    _getStreamFactory.MakeGenericMethod(args),
                    Expression.Constant(new Uri(id)));
            }
            else
            {
                return Expression.Call(
                    ThisParameter,
                    _getStreamFactoryParam.MakeGenericMethod(funcArgs[0], args[0], args[1]),
                    Expression.Constant(new Uri(id)));
            }
        }

        private Expression LookupStream(string id, Type type)
        {
            var args = type.GetGenericArguments();
            Debug.Assert(args.Length == 2);

            return Expression.Call(
                ThisParameter,
                _getStream.MakeGenericMethod(args),
                Expression.Constant(new Uri(id)));
        }

        private Expression LookupObservable(string id, Type type, Type funcType)
        {
            Debug.Assert(type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IReactiveQbservable<>));

            if (funcType == null)
            {
                return Expression.Call(
                    ThisParameter,
                    _getObservable.MakeGenericMethod(type.GetGenericArguments()),
                    Expression.Constant(new Uri(id)));
            }
            else
            {
                var argType = funcType.GetGenericArguments().First();

                return Expression.Call(
                    ThisParameter,
                    _getObservableParam.MakeGenericMethod(argType, type.GetGenericArguments().First()),
                    Expression.Constant(new Uri(id)));
            }
        }

        private Expression LookupObserver(string id, Type type, Type funcType)
        {
            Debug.Assert(type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IReactiveQbserver<>));

            if (funcType == null)
            {
                return Expression.Call(
                    ThisParameter,
                    _getObserver.MakeGenericMethod(type.GetGenericArguments()),
                    Expression.Constant(new Uri(id)));
            }
            else
            {
                var argType = funcType.GetGenericArguments().First();

                return Expression.Call(
                    ThisParameter,
                    _getObserverParam.MakeGenericMethod(argType, type.GetGenericArguments().First()),
                    Expression.Constant(new Uri(id)));
            }
        }

        private Expression LookupSubscriptionFactory(string id, Type funcType)
        {
            var funcArgs = funcType.GetGenericArguments();
            Debug.Assert(funcArgs.Length is 1 or 2);

            if (funcArgs.Length == 1)
            {
                return Expression.Call(
                    ThisParameter,
                    _getSubscriptionFactory,
                    Expression.Constant(new Uri(id)));
            }
            else
            {
                return Expression.Call(
                    ThisParameter,
                    _getSubscriptionFactoryParam.MakeGenericMethod(funcArgs[0]),
                    Expression.Constant(new Uri(id)));
            }
        }

        private Expression LookupSubscription(string id)
        {
            return Expression.Call(
                ThisParameter,
                _getSubscription,
                Expression.Constant(new Uri(id)));
        }

        private static bool TryFindQbservableType(Type type, out Type qbservableType)
        {
            qbservableType = type.FindGenericType(typeof(IReactiveQbservable<>));
            return qbservableType != null;
        }

        private static bool TryFindQbserverType(Type type, out Type qbserverType)
        {
            qbserverType = type.FindGenericType(typeof(IReactiveQbserver<>));
            return qbserverType != null;
        }

        private static bool TryFindQubjectType(Type type, out Type qubjectType)
        {
            qubjectType = type.FindGenericType(typeof(IReactiveQubject<,>));
            return qubjectType != null;
        }
    }
}
