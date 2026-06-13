// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.CompilerServices;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Reaqtor.Remoting.Reactor.Client
{
    /// <summary>
    /// Class providing ReactiveProcessing-specific actions that can be used to
    /// observe the results of a query. Those actions are declared as extension
    /// methods on IAsyncReactiveQbservable, allowing for fluent syntax. These
    /// actions are for backwards compatibility with the legacy RIPP API and
    /// should be considered deprecated/obsolete.
    /// </summary>
    public static class ReactiveProcessingActions
    {
        /// <summary>
        /// Performs an HTTP POST for each message received on the specified stream.
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the elements in the source sequence.
        /// </typeparam>
        /// <param name="source">Source stream whose elements to send via HTTP post.</param>
        /// <param name="getUri">Function to get the HTTP POST's target URI.</param>
        /// <param name="getBody">Function to get the HTTP POST's body field.</param>
        /// <param name="getHeaders">Function to get the HTTP POST's headers.</param>
        /// <returns>
        /// The source sequence with the Http Posted.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// <c>source</c> or <c>getUri</c> or <c>getBody</c> is null</exception>
        /// <remarks>
        /// Using KnownResource here means that the QE will need to have it
        /// defined too. Really the DoHttpPost is like a macro for Do. The right
        /// thing would be to add a VistorAttribute to its declaration and add
        /// a cooperative rewrite to weave in the macro expansion.
        /// </remarks>
        [KnownResource(Constants.Observer.Action.HttpPost.String)]
        public static IAsyncReactiveQbservable<TSource> DoHttpPost<TSource>(
            this IAsyncReactiveQbservable<TSource> source,
            Expression<Func<TSource, Uri>> getUri,
            Expression<Func<TSource, string>> getBody,
            Expression<Func<TSource, Tuple<string, string>[]>> getHeaders = null)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (getUri == null)
            {
                throw new ArgumentNullException(nameof(getUri));
            }

            if (getBody == null)
            {
                throw new ArgumentNullException(nameof(getBody));
            }

            var item = Expression.Parameter(typeof(TSource));

            var bindings = new List<MemberBinding>
                {
                    Expression.Bind(typeof(HttpPost).GetProperty("Uri"), getUri.Apply(item)),
                    Expression.Bind(typeof(HttpPost).GetProperty("Body"), getBody.Apply(item))
                };

            if (getHeaders != null)
            {
                bindings.Add(Expression.Bind(typeof(HttpPost).GetProperty("Headers"), getHeaders.Apply(item)));
            }

            var newHttpPost = Expression.MemberInit(
                Expression.New(typeof(HttpPost)),
                bindings);

            var poster = source.Provider.CreateQbserver<HttpPost>(Expression.Parameter(
                typeof(IAsyncReactiveQbserver<HttpPost>),
                Constants.Observer.Action.HttpPost.String));

#pragma warning disable IDE0034 // Simplify 'default' expression (documents the signature)
            var callOnNext = Expression.Call(
                poster.Expression,
                (MethodInfo)ReflectionHelpers.InfoOf((IAsyncReactiveQbserver<HttpPost> iv) => iv.OnNextAsync(default(HttpPost), default(CancellationToken))),
                newHttpPost,
                Expression.Default(typeof(CancellationToken)));
#pragma warning restore IDE0034 // Simplify 'default' expression

            var onNext = Expression.Lambda<Action<TSource>>(callOnNext, item);

            return source.Do(onNext);
        }

        /// <summary>
        /// Publishes each element received on the specified observable sequence using HTTP POST.
        /// <remarks>Copy-pasted directly from Reactor.</remarks>
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose elements to send via HTTP POST.</param>
        /// <param name="getMethod">Function to get the HTTP request's method.</param>
        /// <param name="getOnNext">Function to get the HTTP request's OnNext URI.</param>
        /// <param name="getBody">Function to get the HTTP request's body content.</param>
        /// <param name="getHeaders">Function to get the HTTP request's headers.</param>
        /// <param name="getRetryData">Function to get data that represents the retry policy.</param>
        /// <param name="getTimeout">Function to get the Http request's timeout.</param>
        /// <returns>The source sequence with each element published using HTTP POST.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <c>source</c> or <c>getMethod</c> or <c>getOnNext</c> or <c>getBody</c>
        /// or <c>getHeaders</c> or <c>getRetryPolicy</c> or <c>getTimeout</c> is null
        /// </exception>
        [Visitor(typeof(DoRewriter))]
        public static IAsyncReactiveQbservable<TSource> DoHttp<TSource>(
            this IAsyncReactiveQbservable<TSource> source,
            Expression<Func<TSource, string>> getMethod,
            Expression<Func<TSource, Uri>> getOnNext,
            Expression<Func<TSource, string>> getBody,
            Expression<Func<TSource, Tuple<string, string>[]>> getHeaders,
            Expression<Func<TSource, RetryData>> getRetryData,
            Expression<Func<TSource, TimeSpan>> getTimeout)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (getMethod == null)
            {
                throw new ArgumentNullException(nameof(getMethod));
            }

            if (getOnNext == null)
            {
                throw new ArgumentNullException(nameof(getOnNext));
            }

            if (getBody == null)
            {
                throw new ArgumentNullException(nameof(getBody));
            }

            if (getHeaders == null)
            {
                throw new ArgumentNullException(nameof(getHeaders));
            }

            if (getRetryData == null)
            {
                throw new ArgumentNullException(nameof(getRetryData));
            }

            if (getTimeout == null)
            {
                throw new ArgumentNullException(nameof(getTimeout));
            }

            Expression<Func<string, string, Tuple<string, string>[], Uri, RetryData, TimeSpan, HttpData>> newHttpDataTemplate = (body, method, headers, onNext, retryData, timeout) => new HttpData
            {
                Body = body,
                Method = method,
                Headers = headers,
                OnNext = onNext,
                RetryData = retryData,
                Timeout = timeout,
            };

            var item = Expression.Parameter(typeof(TSource));

            var newHttpData = BetaReducer.Reduce(Expression.Invoke(newHttpDataTemplate,
                getBody.Apply(item),
                getMethod.Apply(item),
                getHeaders.Apply(item),
                getOnNext.Apply(item),
                getRetryData.Apply(item),
                getTimeout.Apply(item)), BetaReductionNodeTypes.Unrestricted, BetaReductionRestrictions.ExactlyOnce);

            var getHttpData = Expression.Lambda<Func<TSource, HttpData>>(newHttpData, item);

            var poster = source.Provider.CreateQbserver<HttpData>(Expression.Parameter(
                typeof(IAsyncReactiveQbserver<HttpData>),
                Constants.Observer.Action.Http.String));

            return source.Do(getHttpData, poster);
        }

        /// <summary>
        /// Applies the specified expression.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <param name="arguments">The arguments.</param>
        /// <returns>the reduced expression</returns>
        public static Expression Apply(this LambdaExpression expression, params Expression[] arguments)
        {
            return BetaReducer.Reduce(Expression.Invoke(expression, arguments));
        }

        /// <summary>
        /// Cooperative expression rewriter for call sites to Do[Http|Zmq] methods when
        /// used in a nested position, e.g. within a selector function of SelectMany.
        /// </summary>
        private class DoRewriter : IRecursiveExpressionVisitor
        {
            /// <summary>
            /// Rewrites a Do[Http|Zmq] method call to its underlying representation using the Do operator.
            /// </summary>
            /// <param name="expression">Method call expression of the Do[Http|Zmq] call site.</param>
            /// <param name="visit">Recursive visit function.</param>
            /// <param name="result">Rewritten expression with a call to the underlying Do method.</param>
            /// <returns>Always returns true.</returns>
            public bool TryVisit(Expression expression, Func<Expression, Expression> visit, out Expression result)
            {
                var call = (MethodCallExpression)expression;

                var sourceType = call.Method.GetGenericArguments().Single();
                var arguments = call.Arguments.Select(e => visit(e).StripQuotes()).ToArray();

                var source = Activator.CreateInstance(typeof(CapturedQbservable<>).MakeGenericType(sourceType), new CapturedQueryProvider(), arguments[0]);
                var parameters = new[] { source }.Concat(arguments.Skip(1)).ToArray();

                var expandedToDo = (IExpressible)call.Method.Invoke(null, parameters);

                result = visit(expandedToDo.Expression);
                return true;
            }

            /// <summary>
            /// Represents an instance of a qbservable sequence with a captured expression representation.
            /// </summary>
            /// <typeparam name="T">Element type of the observable sequence.</typeparam>
            private class CapturedQbservable<T> : AsyncReactiveQbservableBase<T>
            {
                /// <summary>
                /// Creates a new captured qbservable representation using the specified expression.
                /// </summary>
                /// <param name="provider">Query provider with factory operations used to create reactive artifact representations.</param>
                /// <param name="expression">Expression representing the observable sequence.</param>
                public CapturedQbservable(IAsyncReactiveQueryProvider provider, Expression expression)
                    : base(provider)
                {
                    Expression = expression;
                }

                /// <summary>
                /// Gets the expression representing the observable sequence.
                /// </summary>
                public override Expression Expression { get; }

                /// <summary>
                /// This method is not supported because an instance of this type is only created to hold an
                /// expression tree used to dispatch to the underlying method.
                /// </summary>
                /// <param name="observer">Observer used for the subscription.</param>
                /// <param name="subscriptionUri">URI representing the subscription.</param>
                /// <param name="state">State passed to the subscription creation operation.</param>
                /// <param name="token">Token to observe cancellation requests.</param>
                /// <returns>This method always throws an exception.</returns>
                protected override Task<IAsyncReactiveQubscription> SubscribeAsyncCore(IAsyncReactiveQbserver<T> observer, Uri subscriptionUri, object state, CancellationToken token)
                {
                    throw new NotImplementedException();
                }
            }

            /// <summary>
            /// Represents an instance of a qbserver with a captured expression representation.
            /// </summary>
            /// <typeparam name="T">Element type of the observer.</typeparam>
            private class CapturedQbserver<T> : AsyncReactiveQbserverBase<T>
            {
                /// <summary>
                /// Creates a new captured qbserver representation using the specified expression.
                /// </summary>
                /// <param name="provider">Query provider with factory operations used to create reactive artifact representations.</param>
                /// <param name="expression">Expression representing the observer.</param>
                public CapturedQbserver(IAsyncReactiveQueryProvider provider, Expression expression)
                    : base(provider)
                {
                    Expression = expression;
                }

                /// <summary>
                /// Gets the expression representing the observer.
                /// </summary>
                public override Expression Expression { get; }

                /// <summary>
                /// This method is not supported because an instance of this type is only created to hold an
                /// expression tree used to dispatch to the underlying method.
                /// </summary>
                /// <param name="token">Token to observe cancellation requests.</param>
                /// <returns>This method always throws an exception.</returns>
                protected override Task OnCompletedAsyncCore(CancellationToken token)
                {
                    throw new NotImplementedException();
                }

                /// <summary>
                /// This method is not supported because an instance of this type is only created to hold an
                /// expression tree used to dispatch to the underlying method.
                /// </summary>
                /// <param name="error">Error to send to the observer.</param>
                /// <param name="token">Token to observe cancellation requests.</param>
                /// <returns>This method always throws an exception.</returns>
                protected override Task OnErrorAsyncCore(Exception error, CancellationToken token)
                {
                    throw new NotImplementedException();
                }

                /// <summary>
                /// This method is not supported because an instance of this type is only created to hold an
                /// expression tree used to dispatch to the underlying method.
                /// </summary>
                /// <param name="value">Element to send to the observer.</param>
                /// <param name="token">Token to observe cancellation requests.</param>
                /// <returns>This method always throws an exception.</returns>
                protected override Task OnNextAsyncCore(T value, CancellationToken token)
                {
                    throw new NotImplementedException();
                }
            }

            /// <summary>
            /// Query provider to created captured instances used during dynamic dispatch operations.
            /// </summary>
            private class CapturedQueryProvider : IAsyncReactiveQueryProvider
            {
                /// <summary>
                /// Creates an observable based on the given expression.
                /// </summary>
                /// <typeparam name="T">Type of the data produced by the observable.</typeparam>
                /// <param name="expression">Expression representing the observable.</param>
                /// <returns>Observable represented by the given expression.</returns>
                public IAsyncReactiveQbservable<T> CreateQbservable<T>(Expression expression)
                {
                    return new CapturedQbservable<T>(this, expression);
                }

                /// <summary>
                /// Creates a parameterized observable based on the given expression.
                /// </summary>
                /// <typeparam name="TArgs">Type of the parameter passed to the observable.</typeparam>
                /// <typeparam name="TResult">Type of the data produced by the observable.</typeparam>
                /// <param name="expression">Expression representing the observable.</param>
                /// <returns>Parameterized observable represented by the given expression.</returns>
                public Func<TArgs, IAsyncReactiveQbservable<TResult>> CreateQbservable<TArgs, TResult>(Expression<Func<TArgs, IAsyncReactiveQbservable<TResult>>> expression)
                {
                    throw new NotImplementedException();
                }

                /// <summary>
                /// Creates an observer based on the given expression.
                /// </summary>
                /// <typeparam name="T">Type of the data received by the observer.</typeparam>
                /// <param name="expression">Expression representing the observer.</param>
                /// <returns>Observer represented by the given expression.</returns>
                public IAsyncReactiveQbserver<T> CreateQbserver<T>(Expression expression)
                {
                    return new CapturedQbserver<T>(this, expression);
                }

                /// <summary>
                /// Creates a parameterized observer based on the given expression.
                /// </summary>
                /// <typeparam name="TArgs">Type of the parameter passed to the observer.</typeparam>
                /// <typeparam name="TResult">Type of the data received by the observer.</typeparam>
                /// <param name="expression">Expression representing the observer.</param>
                /// <returns>Parameterized observer represented by the given expression.</returns>
                public Func<TArgs, IAsyncReactiveQbserver<TResult>> CreateQbserver<TArgs, TResult>(Expression<Func<TArgs, IAsyncReactiveQbserver<TResult>>> expression)
                {
                    throw new NotImplementedException();
                }

                /// <summary>
                /// Creates a subject factory based on the given expression.
                /// </summary>
                /// <typeparam name="TInput">Type of the data received by the subject created by the factory.</typeparam>
                /// <typeparam name="TOutput">Type of the data produced by the subject created by the factory.</typeparam>
                /// <param name="expression">Expression representing the subject factory.</param>
                /// <returns>Subject factory represented by the given expression.</returns>
                public IAsyncReactiveQubjectFactory<TInput, TOutput> CreateQubjectFactory<TInput, TOutput>(Expression expression)
                {
                    throw new NotImplementedException();
                }

                /// <summary>
                /// Creates a parameterized subject factory based on the given expression.
                /// </summary>
                /// <typeparam name="TArgs">Type of the parameter passed to the subject factory.</typeparam>
                /// <typeparam name="TInput">Type of the data received by the subject created by the factory.</typeparam>
                /// <typeparam name="TOutput">Type of the data produced by the subject created by the factory.</typeparam>
                /// <param name="expression">Expression representing the subject factory.</param>
                /// <returns>Parameterized subject factory represented by the given expression.</returns>
                public IAsyncReactiveQubjectFactory<TInput, TOutput, TArgs> CreateQubjectFactory<TArgs, TInput, TOutput>(Expression expression)
                {
                    throw new NotImplementedException();
                }

                /// <summary>
                /// Creates a subject based on the given expression.
                /// </summary>
                /// <typeparam name="TInput">Type of the data received by the subject.</typeparam>
                /// <typeparam name="TOutput">Type of the data produced by the subject.</typeparam>
                /// <param name="expression">Expression representing the subject.</param>
                /// <returns>Subject represented by the given expression.</returns>
                public IAsyncReactiveQubject<TInput, TOutput> CreateQubject<TInput, TOutput>(Expression expression)
                {
                    throw new NotImplementedException();
                }

                /// <summary>
                /// Creates a subscription based on the given expression.
                /// </summary>
                /// <param name="expression">Expression representing the subscription.</param>
                /// <returns>Subscription represented by the given expression.</returns>
                public IAsyncReactiveQubscription CreateQubscription(Expression expression)
                {
                    throw new NotImplementedException();
                }

                /// <summary>
                /// Creates a subscription factory based on the given expression.
                /// </summary>
                /// <param name="expression">Expression representing the subscription factory.</param>
                /// <returns>Subscription factory represented by the given expression.</returns>
                public IAsyncReactiveQubscriptionFactory CreateQubscriptionFactory(Expression expression)
                {
                    throw new NotImplementedException();
                }

                /// <summary>
                /// Creates a parameterized subscription factory based on the given expression.
                /// </summary>
                /// <typeparam name="TArgs">Type of the parameter passed to the subscription factory.</typeparam>
                /// <param name="expression">Expression representing the subscription factory.</param>
                /// <returns>Parameterized subscription factory represented by the given expression.</returns>
                public IAsyncReactiveQubscriptionFactory<TArgs> CreateQubscriptionFactory<TArgs>(Expression expression)
                {
                    throw new NotImplementedException();
                }
            }
        }
    }
}
