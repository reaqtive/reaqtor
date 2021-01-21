// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.CompilerServices;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using Reaqtor.Remoting.Protocol;

namespace Reaqtor.Remoting.Client
{
    /// <summary>
    /// Base class for binding reactive processing service providers to reactive service connections.
    /// </summary>
    /// <typeparam name="TExpression">Type used for expression tree representation.</typeparam>
    public class ReactiveServiceProvider<TExpression> : IReactiveServiceProvider<TExpression>
    {
        private readonly ICommandTextFactory<TExpression> _commandTextFactory;
        private readonly ICommandResponseParser _commandResponseParser;

        /// <summary>
        /// Creates a new reactive processing service provider using the specified connection object.
        /// </summary>
        /// <param name="connection">The reactive processing service connection.</param>
        /// <param name="commandTextFactory">The command text factory.</param>
        /// <param name="commandResponseParser">The command response parser.</param>
        public ReactiveServiceProvider(
            IReactiveServiceConnection connection,
            ICommandTextFactory<TExpression> commandTextFactory,
            ICommandResponseParser commandResponseParser)
        {
            Connection = connection;
            _commandTextFactory = commandTextFactory;
            _commandResponseParser = commandResponseParser;
            Provider = new MetadataQueryProvider(this);
        }

        /// <summary>
        /// Gets the query provider that will be used to execute metadata queries.
        /// </summary>
        public virtual IQueryProvider Provider { get; }

        /// <summary>
        /// Gets the connection object underlying the service provide.
        /// </summary>
        public IReactiveServiceConnection Connection { get; }

        /// <summary>
        /// Creates a new subscription using the specified expression tree representation.
        /// </summary>
        /// <param name="subscriptionUri">URI to identify the new subscription.</param>
        /// <param name="subscription">Expression representing the subscription creation. (E.g. an invocation of the subscription operation on an observable.)</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <param name="token">Token to observe for cancellation of the request.</param>
        /// <returns>Task to await the acknowledgement of the creation of the subscription, or an exception.</returns>
        public virtual Task CreateSubscriptionAsync(Uri subscriptionUri, TExpression subscription, object state, CancellationToken token)
        {
            var commandText = _commandTextFactory.CreateNewText(new NewCommandData<TExpression>(subscriptionUri, subscription, state));
            var command = Connection.CreateCommand(CommandVerb.New, CommandNoun.Subscription, commandText);
            return _commandResponseParser.ParseResponseAsync(command, command.ExecuteAsync(token), token);
        }

        /// <summary>
        /// Deletes the subscription identified by the specified URI.
        /// </summary>
        /// <param name="subscriptionUri">URI of the subscription to delete.</param>
        /// <param name="token">Token to observe for cancellation of the request.</param>
        /// <returns>Task to await the acknowledgement of the deletion of the subscription, or an exception.</returns>
        public virtual Task DeleteSubscriptionAsync(Uri subscriptionUri, CancellationToken token)
        {
            var commandText = _commandTextFactory.CreateRemoveText(subscriptionUri);
            var command = Connection.CreateCommand(CommandVerb.Remove, CommandNoun.Subscription, commandText);
            return _commandResponseParser.ParseResponseAsync(command, command.ExecuteAsync(token), token);
        }

        /// <summary>
        /// Creates a new stream using the specified expression tree representation.
        /// </summary>
        /// <param name="streamUri">URI to identify the new stream.</param>
        /// <param name="stream">Expression representing the stream creation. (E.g. an invocation of a known stream factory.)</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <param name="token">Token to observe for cancellation of the request.</param>
        /// <returns>Task to await the acknowledgement of the creation of the stream, or an exception.</returns>
        public virtual Task CreateStreamAsync(Uri streamUri, TExpression stream, object state, CancellationToken token)
        {
            var commandText = _commandTextFactory.CreateNewText(new NewCommandData<TExpression>(streamUri, stream, state));
            var command = Connection.CreateCommand(CommandVerb.New, CommandNoun.Stream, commandText);
            return _commandResponseParser.ParseResponseAsync(command, command.ExecuteAsync(token), token);
        }

        /// <summary>
        /// Deletes the stream identified by the specified URI.
        /// </summary>
        /// <param name="streamUri">URI of the stream to delete.</param>
        /// <param name="token">Token to observe for cancellation of the request.</param>
        /// <returns>Task to await the acknowledgement of the deletion of the stream, or an exception.</returns>
        public virtual Task DeleteStreamAsync(Uri streamUri, CancellationToken token)
        {
            var commandText = _commandTextFactory.CreateRemoveText(streamUri);
            var command = Connection.CreateCommand(CommandVerb.Remove, CommandNoun.Stream, commandText);
            return _commandResponseParser.ParseResponseAsync(command, command.ExecuteAsync(token), token);
        }

        /// <summary>
        /// Gets an observer to send notifications to.
        /// </summary>
        /// <typeparam name="T">Type of the value to send to the observer.</typeparam>
        /// <param name="observerUri">URI of the observer to send the notification to.</param>
        /// <param name="token">Token used to observe cancellation requests.</param>
        /// <returns>Observer that can be used to send notifcations to.</returns>
        public virtual Task<IAsyncReactiveObserver<T>> GetObserverAsync<T>(Uri observerUri, CancellationToken token)
        {
            var body = _commandTextFactory.CreateGetText(Expression.Parameter(typeof(IAsyncReactiveObserver<T>), observerUri.ToCanonicalString()));
            var command = Connection.CreateCommand(CommandVerb.Get, CommandNoun.Observer, body);
            return _commandResponseParser.ParseResponseAsync<IAsyncReactiveObserver<T>>(command, command.ExecuteAsync(token), token);
        }

        /// <summary>
        /// Defines an observable using the specified expression tree representation.
        /// The observable can be parameterized by specifying a lambda expression.
        /// </summary>
        /// <param name="observableUri">URI to identify the new observable.</param>
        /// <param name="observable">Expression representing the observable creation. (E.g. using composition of LINQ query operators.)</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <param name="token">Token to observe for cancellation of the request.</param>
        /// <returns>Task to await the completion of the observable definition.</returns>
        public virtual Task DefineObservableAsync(Uri observableUri, TExpression observable, object state, CancellationToken token)
        {
            var commandText = _commandTextFactory.CreateNewText(new NewCommandData<TExpression>(observableUri, observable, state));
            var command = Connection.CreateCommand(CommandVerb.New, CommandNoun.Observable, commandText);
            return _commandResponseParser.ParseResponseAsync(command, command.ExecuteAsync(token), token);
        }

        /// <summary>
        /// Undefines the observable identified by the specified URI.
        /// </summary>
        /// <param name="observableUri">URI identifying the observable.</param>
        /// <param name="token">Token to observe for cancellation of the request.</param>
        /// <returns>Task to await the completion of the observable undefinition.</returns>
        public virtual Task UndefineObservableAsync(Uri observableUri, CancellationToken token)
        {
            var commandText = _commandTextFactory.CreateRemoveText(observableUri);
            var command = Connection.CreateCommand(CommandVerb.Remove, CommandNoun.Observable, commandText);
            return _commandResponseParser.ParseResponseAsync(command, command.ExecuteAsync(token), token);
        }

        /// <summary>
        /// Defines an observer using the specified expression tree representation.
        /// The observer can be parameterized by specifying a lambda expression.
        /// </summary>
        /// <param name="observerUri">URI to identify the new observer.</param>
        /// <param name="observer">Expression representing the observer creation.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <param name="token">Token to observe for cancellation of the request.</param>
        /// <returns>Task to await the completion of the observer definition.</returns>
        public virtual Task DefineObserverAsync(Uri observerUri, TExpression observer, object state, CancellationToken token)
        {
            var commandText = _commandTextFactory.CreateNewText(new NewCommandData<TExpression>(observerUri, observer, state));
            var command = Connection.CreateCommand(CommandVerb.New, CommandNoun.Observer, commandText);
            return _commandResponseParser.ParseResponseAsync(command, command.ExecuteAsync(token), token);
        }

        /// <summary>
        /// Undefines the observer identified by the specified URI.
        /// </summary>
        /// <param name="observerUri">URI identifying the observer.</param>
        /// <param name="token">Token to observe for cancellation of the request.</param>
        /// <returns>Task to await the completion of the observer undefinition.</returns>
        public virtual Task UndefineObserverAsync(Uri observerUri, CancellationToken token)
        {
            var commandText = _commandTextFactory.CreateRemoveText(observerUri);
            var command = Connection.CreateCommand(CommandVerb.Remove, CommandNoun.Observer, commandText);
            return _commandResponseParser.ParseResponseAsync(command, command.ExecuteAsync(token), token);
        }

        /// <summary>
        /// Defines a stream factory using the specified expression tree representation.
        /// The stream factory can be parameterized by specifying a lambda expression.
        /// </summary>
        /// <param name="streamFactoryUri">URI to identify the new stream factory.</param>
        /// <param name="streamFactory">Expression representing the stream factory creation.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <param name="token">Token to observe for cancellation of the request.</param>
        /// <returns>Task to await the completion of the stream factory definition.</returns>
        public virtual Task DefineStreamFactoryAsync(Uri streamFactoryUri, TExpression streamFactory, object state, CancellationToken token)
        {
            var commandText = _commandTextFactory.CreateNewText(new NewCommandData<TExpression>(streamFactoryUri, streamFactory, state));
            var command = Connection.CreateCommand(CommandVerb.New, CommandNoun.StreamFactory, commandText);
            return _commandResponseParser.ParseResponseAsync(command, command.ExecuteAsync(token), token);
        }

        /// <summary>
        /// Undefines the stream factory identified by the specified URI.
        /// </summary>
        /// <param name="streamFactoryUri">URI identifying the stream factory.</param>
        /// <param name="token">Token to observe for cancellation of the request.</param>
        /// <returns>Task to await the completion of the stream factory undefinition.</returns>
        public virtual Task UndefineStreamFactoryAsync(Uri streamFactoryUri, CancellationToken token)
        {
            var commandText = _commandTextFactory.CreateRemoveText(streamFactoryUri);
            var command = Connection.CreateCommand(CommandVerb.Remove, CommandNoun.StreamFactory, commandText);
            return _commandResponseParser.ParseResponseAsync(command, command.ExecuteAsync(token), token);
        }

        /// <summary>
        /// Defines a subscription factory using the specified expression tree representation.
        /// The subscription factory can be parameterized by specifying a lambda expression.
        /// </summary>
        /// <param name="subscriptionFactoryUri">URI to identify the new subscription factory.</param>
        /// <param name="subscriptionFactory">Expression representing the subscription factory creation.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <param name="token">Token to observe for cancellation of the request.</param>
        /// <returns>Task to await the completion of the subscription factory definition.</returns>
        public virtual Task DefineSubscriptionFactoryAsync(Uri subscriptionFactoryUri, TExpression subscriptionFactory, object state, CancellationToken token)
        {
            var commandText = _commandTextFactory.CreateNewText(new NewCommandData<TExpression>(subscriptionFactoryUri, subscriptionFactory, state));
            var command = Connection.CreateCommand(CommandVerb.New, CommandNoun.SubscriptionFactory, commandText);
            return _commandResponseParser.ParseResponseAsync(command, command.ExecuteAsync(token), token);
        }

        /// <summary>
        /// Undefines the subscription factory identified by the specified URI.
        /// </summary>
        /// <param name="subscriptionFactoryUri">URI identifying the subscription factory.</param>
        /// <param name="token">Token to observe for cancellation of the request.</param>
        /// <returns>Task to await the completion of the subscription factory undefinition.</returns>
        public virtual Task UndefineSubscriptionFactoryAsync(Uri subscriptionFactoryUri, CancellationToken token)
        {
            var commandText = _commandTextFactory.CreateRemoveText(subscriptionFactoryUri);
            var command = Connection.CreateCommand(CommandVerb.Remove, CommandNoun.SubscriptionFactory, commandText);
            return _commandResponseParser.ParseResponseAsync(command, command.ExecuteAsync(token), token);
        }

        private sealed class MetadataQueryProvider : IQueryProvider
        {
            private readonly ReactiveServiceProvider<TExpression> _parent;

            public MetadataQueryProvider(ReactiveServiceProvider<TExpression> parent)
            {
                _parent = parent;
            }

            public IQueryable<TElement> CreateQuery<TElement>(Expression expression) => new MetadataQuery<TElement>(this, expression);

            public IQueryable CreateQuery(Expression expression)
            {
                var queryableType = expression.Type.FindGenericType(typeof(IQueryable<>));
                var elementType = queryableType != null ? queryableType.GenericTypeArguments[0] : typeof(object);
                var queryType = typeof(MetadataQuery<>).MakeGenericType(elementType);
                return (IQueryable)Activator.CreateInstance(queryType, this, expression);
            }

            public TResult Execute<TResult>(Expression expression)
            {
                var commandText = _parent._commandTextFactory.CreateGetText(expression);
                var command = _parent.Connection.CreateCommand(CommandVerb.Get, CommandNoun.Metadata, commandText);
                return _parent._commandResponseParser.ParseResponseAsync<TResult>(command, command.ExecuteAsync(CancellationToken.None), CancellationToken.None).Result;
            }

            public object Execute(Expression expression)
            {
                var execute = ((MethodInfo)ReflectionHelpers.InfoOf((IQueryProvider p) => p.Execute<object>(default))).GetGenericMethodDefinition().MakeGenericMethod(expression.Type);
                return execute.Invoke(this, new object[] { expression });
            }

            private sealed class MetadataQuery<T> : IQueryable<T>
            {
                public MetadataQuery(IQueryProvider provider, Expression expression)
                {
                    Provider = provider;
                    Expression = expression;
                }

                public Type ElementType => typeof(T);

                public Expression Expression { get; }

                public IQueryProvider Provider { get; }

                public IEnumerator<T> GetEnumerator() => Provider.Execute<IEnumerable<T>>(Expression).GetEnumerator();

                IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
            }
        }
    }

    /// <summary>
    /// Base class for reactive processing service providers using BCL expression type.
    /// </summary>
    public class ReactiveServiceProvider : ReactiveServiceProvider<Expression>, IReactiveServiceProvider
    {
        /// <summary>
        /// Creates a new reactive processing service provider using the specified connection object.
        /// </summary>
        /// <param name="connection">The reactive processing service connection.</param>
        /// <param name="commandTextFactory">The command text factory.</param>
        /// <param name="commandResponseParser">The command response parser.</param>
        public ReactiveServiceProvider(IReactiveServiceConnection connection, ICommandTextFactory<Expression> commandTextFactory, ICommandResponseParser commandResponseParser)
            : base(connection, commandTextFactory, commandResponseParser)
        {
        }
    }
}
