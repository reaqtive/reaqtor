// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Globalization;
using System.Linq.CompilerServices;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Reaqtor.Remoting.Protocol
{
    /// <summary>
    /// Base class for binding reactive service connections to reactive processing service providers.
    /// </summary>
    /// <typeparam name="TExpression">Type used for expression tree representation.</typeparam>
    /// <remarks>This class is intended to be used for a Reactive service.</remarks>
    public class ReactiveServiceConnection<TExpression> : IReactiveServiceConnection
    {
#pragma warning disable IDE0034 // Simplify 'default' expression (documents the signature)
        private static readonly MethodInfo s_executeGetObserverAsync = ((MethodInfo)ReflectionHelpers.InfoOf(
            (ReactiveServiceConnection<TExpression> connection) =>
                connection.ExecuteGetObserverAsync<object>(default(IReactiveServiceCommand), default(Uri), default(CancellationToken)))).GetGenericMethodDefinition();
#pragma warning restore IDE0034 // Simplify 'default' expression

        private readonly IReactiveServiceProvider<TExpression> _serviceProvider;
        private readonly ICommandTextParser<TExpression> _commandTextParser;
        private readonly ICommandResponseFactory _commandResponseFactory;
        private readonly CommandExecutor _executor;

        /// <summary>
        /// Base class for implementing <see cref="IReactiveServiceConnection"/>.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="commandTextParser">The command text parser.</param>
        /// <param name="commandResponseFactory">The command response factory.</param>
        public ReactiveServiceConnection(
            IReactiveServiceProvider<TExpression> serviceProvider,
            ICommandTextParser<TExpression> commandTextParser,
            ICommandResponseFactory commandResponseFactory)
        {
            _serviceProvider = serviceProvider;
            _commandTextParser = commandTextParser;
            _commandResponseFactory = commandResponseFactory;
            _executor = new CommandExecutor(this);
        }

        /// <summary>
        /// Creates and returns an IReactiveServiceCommand object associated with the connection.
        /// </summary>
        /// <param name="verb">Command verb.</param>
        /// <param name="noun">Command noun.</param>
        /// <param name="commandText">Text command to run against the reactive service.</param>
        /// <returns>Command object associated with the connection.</returns>
        public IReactiveServiceCommand CreateCommand(CommandVerb verb, CommandNoun noun, string commandText)
        {
            return new Command(this, verb, noun, commandText);
        }

        protected virtual Task<string> ExecuteMetadataQueryAsync(IReactiveServiceCommand command, Expression expression, CancellationToken token)
        {
            var result = _serviceProvider.Provider.Execute(expression);
            return _commandResponseFactory.CreateResponseAsync(command, Task.FromResult(result), token);
        }

        protected virtual Task<string> ExecuteGetObserverAsync(IReactiveServiceCommand command, Expression expression, CancellationToken token)
        {
            var genericType = typeof(IAsyncReactiveObserver<>);
            var observerType = expression.Type.FindGenericType(genericType);
            if (expression is not ParameterExpression parameterExpression || observerType == null)
            {
                throw new InvalidOperationException(
                    string.Format(CultureInfo.InvariantCulture, "Expected observer parameter expression of type '{0}'. Expression = '{1}'", genericType, expression.ToString()));
            }

            var executeGetObserverAsync = s_executeGetObserverAsync.MakeGenericMethod(observerType.GenericTypeArguments);
            return (Task<string>)executeGetObserverAsync.Invoke(this, new object[] { command, new Uri(parameterExpression.Name), token });
        }

        private Task<string> ExecuteGetObserverAsync<T>(IReactiveServiceCommand command, Uri uri, CancellationToken token)
        {
            var result = _serviceProvider.GetObserverAsync<T>(uri, token);
            return _commandResponseFactory.CreateResponseAsync(command, result, token);
        }

        private sealed class CommandExecutor
        {
            private readonly ReactiveServiceConnection<TExpression> _parent;

            public CommandExecutor(ReactiveServiceConnection<TExpression> parent)
            {
                _parent = parent;
            }

            public Task<string> ExecuteAsync(IReactiveServiceCommand command, CancellationToken token)
            {
                if (command == null)
                {
                    throw new ArgumentNullException(nameof(command));
                }

                return command.Verb switch
                {
                    CommandVerb.New => ExecuteNewAsync(command, token),
                    CommandVerb.Remove => ExecuteRemoveAsync(command, token),
                    CommandVerb.Get => ExecuteGetAsync(command, token),
                    _ => ExecuteExtensionAsync(command, token),
                };
            }

            #region New Commands

            private Task<string> ExecuteNewAsync(IReactiveServiceCommand command, CancellationToken token)
            {
                return command.Noun switch
                {
                    CommandNoun.Observable => ExecuteNewObservableAsync(command, token),
                    CommandNoun.Observer => ExecuteNewObserverAsync(command, token),
                    CommandNoun.StreamFactory => ExecuteNewStreamFactoryAsync(command, token),
                    CommandNoun.SubscriptionFactory => ExecuteNewSubscriptionFactoryAsync(command, token),
                    CommandNoun.Stream => ExecuteNewStreamAsync(command, token),
                    CommandNoun.Subscription => ExecuteNewSubscriptionAsync(command, token),
                    CommandNoun.Metadata => ExecuteNewMetadataAsync(command, token),
                    _ => ExecuteNewExtensionAsync(command, token),
                };
            }

            private Task<string> ExecuteNewObservableAsync(IReactiveServiceCommand command, CancellationToken token)
            {
                var data = _parent._commandTextParser.ParseNewText(command.CommandText);
                var task = _parent._serviceProvider.DefineObservableAsync(data.Uri, data.Expression, data.State, token);
                return _parent._commandResponseFactory.CreateResponseAsync(command, task, token);
            }

            private Task<string> ExecuteNewObserverAsync(IReactiveServiceCommand command, CancellationToken token)
            {
                var data = _parent._commandTextParser.ParseNewText(command.CommandText);
                var task = _parent._serviceProvider.DefineObserverAsync(data.Uri, data.Expression, data.State, token);
                return _parent._commandResponseFactory.CreateResponseAsync(command, task, token);
            }

            private Task<string> ExecuteNewStreamFactoryAsync(IReactiveServiceCommand command, CancellationToken token)
            {
                var data = _parent._commandTextParser.ParseNewText(command.CommandText);
                var task = _parent._serviceProvider.DefineStreamFactoryAsync(data.Uri, data.Expression, data.State, token);
                return _parent._commandResponseFactory.CreateResponseAsync(command, task, token);
            }

            private Task<string> ExecuteNewSubscriptionFactoryAsync(IReactiveServiceCommand command, CancellationToken token)
            {
                var data = _parent._commandTextParser.ParseNewText(command.CommandText);
                var task = _parent._serviceProvider.DefineSubscriptionFactoryAsync(data.Uri, data.Expression, data.State, token);
                return _parent._commandResponseFactory.CreateResponseAsync(command, task, token);
            }

            private Task<string> ExecuteNewStreamAsync(IReactiveServiceCommand command, CancellationToken token)
            {
                var data = _parent._commandTextParser.ParseNewText(command.CommandText);
                var task = _parent._serviceProvider.CreateStreamAsync(data.Uri, data.Expression, data.State, token);
                return _parent._commandResponseFactory.CreateResponseAsync(command, task, token);
            }

            private Task<string> ExecuteNewSubscriptionAsync(IReactiveServiceCommand command, CancellationToken token)
            {
                var data = _parent._commandTextParser.ParseNewText(command.CommandText);
                var task = _parent._serviceProvider.CreateSubscriptionAsync(data.Uri, data.Expression, data.State, token);
                return _parent._commandResponseFactory.CreateResponseAsync(command, task, token);
            }

            private Task<string> ExecuteNewMetadataAsync(IReactiveServiceCommand command, CancellationToken token)
            {
                throw GetNotImplementedCommandException(command, token);
            }

            private Task<string> ExecuteNewExtensionAsync(IReactiveServiceCommand command, CancellationToken token)
            {
                throw GetNotImplementedCommandException(command, token);
            }

            #endregion

            #region Remove Commands

            private Task<string> ExecuteRemoveAsync(IReactiveServiceCommand command, CancellationToken token)
            {
                return command.Noun switch
                {
                    CommandNoun.Observable => ExecuteRemoveObservableAsync(command, token),
                    CommandNoun.Observer => ExecuteRemoveObserverAsync(command, token),
                    CommandNoun.StreamFactory => ExecuteRemoveStreamFactoryAsync(command, token),
                    CommandNoun.SubscriptionFactory => ExecuteRemoveSubscriptionFactoryAsync(command, token),
                    CommandNoun.Stream => ExecuteRemoveStreamAsync(command, token),
                    CommandNoun.Subscription => ExecuteRemoveSubscriptionAsync(command, token),
                    CommandNoun.Metadata => ExecuteRemoveMetadataAsync(command, token),
                    _ => ExecuteRemoveExtensionAsync(command, token),
                };
            }

            private Task<string> ExecuteRemoveObservableAsync(IReactiveServiceCommand command, CancellationToken token)
            {
                var uri = _parent._commandTextParser.ParseRemoveText(command.CommandText);
                var task = _parent._serviceProvider.UndefineObservableAsync(uri, token);
                return _parent._commandResponseFactory.CreateResponseAsync(command, task, token);
            }

            private Task<string> ExecuteRemoveObserverAsync(IReactiveServiceCommand command, CancellationToken token)
            {
                var uri = _parent._commandTextParser.ParseRemoveText(command.CommandText);
                var task = _parent._serviceProvider.UndefineObserverAsync(uri, token);
                return _parent._commandResponseFactory.CreateResponseAsync(command, task, token);
            }

            private Task<string> ExecuteRemoveStreamFactoryAsync(IReactiveServiceCommand command, CancellationToken token)
            {
                var uri = _parent._commandTextParser.ParseRemoveText(command.CommandText);
                var task = _parent._serviceProvider.UndefineStreamFactoryAsync(uri, token);
                return _parent._commandResponseFactory.CreateResponseAsync(command, task, token);
            }

            private Task<string> ExecuteRemoveSubscriptionFactoryAsync(IReactiveServiceCommand command, CancellationToken token)
            {
                var uri = _parent._commandTextParser.ParseRemoveText(command.CommandText);
                var task = _parent._serviceProvider.UndefineSubscriptionFactoryAsync(uri, token);
                return _parent._commandResponseFactory.CreateResponseAsync(command, task, token);
            }

            private Task<string> ExecuteRemoveStreamAsync(IReactiveServiceCommand command, CancellationToken token)
            {
                var uri = _parent._commandTextParser.ParseRemoveText(command.CommandText);
                var task = _parent._serviceProvider.DeleteStreamAsync(uri, token);
                return _parent._commandResponseFactory.CreateResponseAsync(command, task, token);
            }

            private Task<string> ExecuteRemoveSubscriptionAsync(IReactiveServiceCommand command, CancellationToken token)
            {
                var uri = _parent._commandTextParser.ParseRemoveText(command.CommandText);
                var task = _parent._serviceProvider.DeleteSubscriptionAsync(uri, token);
                return _parent._commandResponseFactory.CreateResponseAsync(command, task, token);
            }

            private Task<string> ExecuteRemoveMetadataAsync(IReactiveServiceCommand command, CancellationToken token)
            {
                throw GetNotImplementedCommandException(command, token);
            }

            private Task<string> ExecuteRemoveExtensionAsync(IReactiveServiceCommand command, CancellationToken token)
            {
                throw GetNotImplementedCommandException(command, token);
            }

            #endregion

            #region Get Commands

            private Task<string> ExecuteGetAsync(IReactiveServiceCommand command, CancellationToken token)
            {
                return command.Noun switch
                {
                    CommandNoun.Observable => ExecuteGetObservableAsync(command, token),
                    CommandNoun.Observer => ExecuteGetObserverAsync(command, token),
                    CommandNoun.StreamFactory => ExecuteGetStreamFactoryAsync(command, token),
                    CommandNoun.SubscriptionFactory => ExecuteGetSubscriptionFactoryAsync(command, token),
                    CommandNoun.Stream => ExecuteGetStreamAsync(command, token),
                    CommandNoun.Subscription => ExecuteGetSubscriptionAsync(command, token),
                    CommandNoun.Metadata => ExecuteGetMetadataAsync(command, token),
                    _ => ExecuteGetExtensionAsync(command, token),
                };
            }

            private Task<string> ExecuteGetObservableAsync(IReactiveServiceCommand command, CancellationToken token)
            {
                throw GetNotImplementedCommandException(command, token);
            }

            private Task<string> ExecuteGetObserverAsync(IReactiveServiceCommand command, CancellationToken token)
            {
                var expression = _parent._commandTextParser.ParseGetText(command.CommandText);
                return _parent.ExecuteGetObserverAsync(command, expression, token);
            }

            private Task<string> ExecuteGetStreamFactoryAsync(IReactiveServiceCommand command, CancellationToken token)
            {
                throw GetNotImplementedCommandException(command, token);
            }

            private Task<string> ExecuteGetSubscriptionFactoryAsync(IReactiveServiceCommand command, CancellationToken token)
            {
                throw GetNotImplementedCommandException(command, token);
            }

            private Task<string> ExecuteGetStreamAsync(IReactiveServiceCommand command, CancellationToken token)
            {
                throw GetNotImplementedCommandException(command, token);
            }

            private Task<string> ExecuteGetSubscriptionAsync(IReactiveServiceCommand command, CancellationToken token)
            {
                throw GetNotImplementedCommandException(command, token);
            }

            private Task<string> ExecuteGetMetadataAsync(IReactiveServiceCommand command, CancellationToken token)
            {
                var expression = _parent._commandTextParser.ParseGetText(command.CommandText);
                return _parent.ExecuteMetadataQueryAsync(command, expression, token);
            }

            private Task<string> ExecuteGetExtensionAsync(IReactiveServiceCommand command, CancellationToken token)
            {
                throw GetNotImplementedCommandException(command, token);
            }

            #endregion

            private Task<string> ExecuteExtensionAsync(IReactiveServiceCommand command, CancellationToken token)
            {
                throw GetNotImplementedCommandException(command, token);
            }

            private Exception GetNotImplementedCommandException(IReactiveServiceCommand command, CancellationToken token)
            {
                _ = this; // NB: Suppress CA1822.
                _ = token;

                return new NotImplementedException(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "Command uses noun '{0}' which is not implemented for verb '{1}'.",
                        command.Noun,
                        command.Verb));
            }
        }

        private sealed class Command : IReactiveServiceCommand
        {
            private readonly ReactiveServiceConnection<TExpression> _parent;

            public Command(ReactiveServiceConnection<TExpression> parent, CommandVerb verb, CommandNoun noun, string commandText)
            {
                _parent = parent;
                Verb = verb;
                Noun = noun;
                CommandText = commandText;
            }

            public IReactiveServiceConnection Connection => _parent;

            public CommandVerb Verb { get; }

            public CommandNoun Noun { get; }

            public string CommandText { get; }

            public Task<string> ExecuteAsync(CancellationToken token)
            {
                return _parent._executor.ExecuteAsync(this, token);
            }
        }
    }
}
