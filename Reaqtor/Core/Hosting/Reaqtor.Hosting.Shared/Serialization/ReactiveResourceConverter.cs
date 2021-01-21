// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using Nuqleon.DataModel;
using Nuqleon.DataModel.Serialization.Json;
using Reaqtor.Metadata;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Reaqtor.Hosting.Shared.Serialization
{
    /// <summary>
    /// A data converter for entities implementing <see cref="IAsyncReactiveResource"/>.
    /// </summary>
    /// <typeparam name="TState">The type of state parameter.</typeparam>
    public class ReactiveResourceConverter<TState> : DataConverter
    {
        private readonly IReactiveExpressionServices _expressionServices;

        /// <summary>
        /// Instantiates the data converter.
        /// </summary>
        public ReactiveResourceConverter()
            : this(expressionServices: null)
        {
        }

        /// <summary>
        /// Instantiates the data converter with an expression services implementation
        /// to apply when serializing or deserializing expressions.
        /// </summary>
        /// <param name="expressionServices">The expression services.</param>
        public ReactiveResourceConverter(IReactiveExpressionServices expressionServices)
        {
            _expressionServices = expressionServices;
        }

        /// <summary>
        /// Converts from the underlying representation to an instance of
        /// <see cref="IAsyncReactiveResource"/>.
        /// </summary>
        /// <param name="value">The underlying value.</param>
        /// <param name="sourceType">The source type.</param>
        /// <param name="targetType">The target type.</param>
        /// <returns>The converted instance.</returns>
        public override object ConvertFrom(object value, Type sourceType, Type targetType)
        {
            if (value == null)
            {
                return null;
            }

            var container = (ReactiveResourceContainer)value;
            if (targetType == typeof(IAsyncReactiveObservableDefinition))
            {
                return new Observable(container, _expressionServices);
            }
            else if (targetType == typeof(IAsyncReactiveObserverDefinition))
            {
                return new Observer(container, _expressionServices);
            }
            else if (targetType == typeof(IAsyncReactiveStreamFactoryDefinition))
            {
                return new StreamFactory(container, _expressionServices);
            }
            else if (targetType == typeof(IAsyncReactiveStreamProcess))
            {
                return new Stream(container, _expressionServices);
            }
            else if (targetType == typeof(IAsyncReactiveSubscriptionFactoryDefinition))
            {
                return new SubscriptionFactory(container, _expressionServices);
            }
            else
            {
                // `TryCanConvert` only accepts these five types
                Debug.Assert(targetType == typeof(IAsyncReactiveSubscriptionProcess));

                return new Subscription(container, _expressionServices);
            }
        }

        /// <summary>
        /// Converts an instance of <see cref="IAsyncReactiveResource"/>
        /// to its underlying representation for serialization.
        /// </summary>
        /// <param name="value">The original value.</param>
        /// <param name="sourceType">The source type.</param>
        /// <param name="targetType">The target type.</param>
        /// <returns>The converted instance.</returns>
        public override object ConvertTo(object value, Type sourceType, Type targetType)
        {
            // Base class knows how to write null values
            Debug.Assert(value != null);

            var resource = (IAsyncReactiveResource)value;

            var container = new ReactiveResourceContainer
            {
                Uri = resource.Uri,
                Expression = _expressionServices != null
                    ? _expressionServices.Normalize(resource.Expression).ToExpressionSlim()
                    : resource.Expression.ToExpressionSlim(),
            };

            if (value is IAsyncReactiveDefinedResource definedResource)
            {
                container.State = (TState)definedResource.State;
            }
            else if (value is IAsyncReactiveProcessResource processResource)
            {
                container.State = (TState)processResource.State;
            }

            return container;
        }

        /// <summary>
        /// Checks if a type should be converted.
        /// </summary>
        /// <param name="fromType">The type to check.</param>
        /// <param name="targetType">The target type.</param>
        /// <returns>
        /// <b>true</b> if the type can be converted, <b>false</b> otherwise.
        /// </returns>
        public override bool TryCanConvert(Type fromType, out Type targetType)
        {
            if (typeof(IAsyncReactiveResource).IsAssignableFrom(fromType)
                && (typeof(IAsyncReactiveObservableDefinition).IsAssignableFrom(fromType)
                || typeof(IAsyncReactiveObserverDefinition).IsAssignableFrom(fromType)
                || typeof(IAsyncReactiveStreamFactoryDefinition).IsAssignableFrom(fromType)
                || typeof(IAsyncReactiveStreamProcess).IsAssignableFrom(fromType)
                || typeof(IAsyncReactiveSubscriptionProcess).IsAssignableFrom(fromType)
                || typeof(IAsyncReactiveSubscriptionFactoryDefinition).IsAssignableFrom(fromType)))
            {
                targetType = typeof(ReactiveResourceContainer);
                return true;
            }

            targetType = null;
            return false;
        }

        private sealed class ReactiveResourceContainer
        {
            [Mapping("Uri")]
            public Uri Uri { get; set; }

            [Mapping("Expression")]
            public ExpressionSlim Expression { get; set; }

            [Mapping("State")]
            public TState State { get; set; }
        }

        private abstract class Resource : IAsyncReactiveResource
        {
            private readonly Lazy<Expression> _expression;

            protected Resource(ReactiveResourceContainer container, IReactiveExpressionServices expressionServices)
            {
                Debug.Assert(container != null);

                Uri = container.Uri;
                _expression = new Lazy<Expression>(() => expressionServices != null
                    ? expressionServices.Normalize(container.Expression.ToExpression())
                    : container.Expression.ToExpression());
            }

            public Uri Uri { get; }

            public Expression Expression => _expression.Value;
        }

        private abstract class DefinedResource : Resource, IAsyncReactiveDefinedResource
        {
            private static readonly HashSet<Type> s_funcTypes = new()
            {
                typeof(Func<,>),
                typeof(Func<,,>),
                typeof(Func<,,,>),
                typeof(Func<,,,,>),
                typeof(Func<,,,,,>),
                typeof(Func<,,,,,,>),
                typeof(Func<,,,,,,,>),
                typeof(Func<,,,,,,,,>),
                typeof(Func<,,,,,,,,,>),
                typeof(Func<,,,,,,,,,,>),
                typeof(Func<,,,,,,,,,,,>),
                typeof(Func<,,,,,,,,,,,,>),
                typeof(Func<,,,,,,,,,,,,,>),
                typeof(Func<,,,,,,,,,,,,,,>),
                typeof(Func<,,,,,,,,,,,,,,,>),
                typeof(Func<,,,,,,,,,,,,,,,,>),
            };

            private bool? _isParameterized;

            public DefinedResource(ReactiveResourceContainer container, IReactiveExpressionServices expressionServices)
                : base(container, expressionServices)
            {
                State = container.State;
            }

            public virtual bool IsParameterized
            {
                get
                {
                    if (!_isParameterized.HasValue)
                    {
                        _isParameterized = Expression.Type.IsGenericType && s_funcTypes.Contains(Expression.Type.GetGenericTypeDefinition());
                    }

                    return _isParameterized.Value;
                }
            }

            public object State { get; }

            public DateTimeOffset DefinitionTime => throw new NotImplementedException();
        }

        private abstract class ProcessResource : Resource, IAsyncReactiveProcessResource
        {
            public ProcessResource(ReactiveResourceContainer container, IReactiveExpressionServices expressionServices)
                : base(container, expressionServices)
            {
                State = container.State;
            }

            public object State { get; }

            public DateTimeOffset CreationTime => throw new NotImplementedException();

#if NET5_0 || NETSTANDARD2_1
            public ValueTask DisposeAsync() => throw new NotImplementedException();
#else
            public Task DisposeAsync(System.Threading.CancellationToken token) => throw new NotImplementedException();
#endif
        }

        private sealed class Observable : DefinedResource, IAsyncReactiveObservableDefinition
        {
            public Observable(ReactiveResourceContainer container, IReactiveExpressionServices expressionServices)
                : base(container, expressionServices)
            {
            }

            public IAsyncReactiveQbservable<T> ToObservable<T>()
            {
                throw new NotImplementedException();
            }

            public Func<TArgs, IAsyncReactiveQbservable<TResult>> ToObservable<TArgs, TResult>()
            {
                throw new NotImplementedException();
            }
        }

        private sealed class Observer : DefinedResource, IAsyncReactiveObserverDefinition
        {
            public Observer(ReactiveResourceContainer container, IReactiveExpressionServices expressionServices)
                : base(container, expressionServices)
            {
            }

            public IAsyncReactiveQbserver<T> ToObserver<T>()
            {
                throw new NotImplementedException();
            }

            public Func<TArgs, IAsyncReactiveQbserver<TResult>> ToObserver<TArgs, TResult>()
            {
                throw new NotImplementedException();
            }
        }

        private sealed class StreamFactory : DefinedResource, IAsyncReactiveStreamFactoryDefinition
        {
            private static readonly HashSet<Type> s_factoryTypes = new()
            {
                typeof(IAsyncReactiveQubjectFactory<,,>),
                typeof(IAsyncReactiveQubjectFactory<,,,>),
                typeof(IAsyncReactiveQubjectFactory<,,,,>),
                typeof(IAsyncReactiveQubjectFactory<,,,,,>),
                typeof(IAsyncReactiveQubjectFactory<,,,,,,>),
                typeof(IAsyncReactiveQubjectFactory<,,,,,,,>),
                typeof(IAsyncReactiveQubjectFactory<,,,,,,,,>),
                typeof(IAsyncReactiveQubjectFactory<,,,,,,,,,>),
                typeof(IAsyncReactiveQubjectFactory<,,,,,,,,,,>),
                typeof(IAsyncReactiveQubjectFactory<,,,,,,,,,,,>),
                typeof(IAsyncReactiveQubjectFactory<,,,,,,,,,,,,>),
                typeof(IAsyncReactiveQubjectFactory<,,,,,,,,,,,,,>),
                typeof(IAsyncReactiveQubjectFactory<,,,,,,,,,,,,,,>),
                typeof(IAsyncReactiveQubjectFactory<,,,,,,,,,,,,,,,>),
                typeof(IAsyncReactiveQubjectFactory<,,,,,,,,,,,,,,,,>),
            };

            private bool? _isParameterized;

            public StreamFactory(ReactiveResourceContainer container, IReactiveExpressionServices expressionServices)
                : base(container, expressionServices)
            {
            }

            public override bool IsParameterized
            {
                get
                {
                    if (!base.IsParameterized)
                    {
                        if (!_isParameterized.HasValue)
                        {
                            _isParameterized = Expression.Type.IsGenericType && s_factoryTypes.Contains(Expression.Type.GetGenericTypeDefinition());
                        }

                        return _isParameterized.Value;
                    }

                    return true;
                }
            }

            public IAsyncReactiveQubjectFactory<TInput, TOutput> ToStreamFactory<TInput, TOutput>()
            {
                throw new NotImplementedException();
            }

            public IAsyncReactiveQubjectFactory<TInput, TOutput, TArgs> ToStreamFactory<TArgs, TInput, TOutput>()
            {
                throw new NotImplementedException();
            }
        }

        private sealed class Stream : ProcessResource, IAsyncReactiveStreamProcess
        {
            public Stream(ReactiveResourceContainer container, IReactiveExpressionServices expressionServices)
                : base(container, expressionServices)
            {
            }

            public IAsyncReactiveQubject<TInput, TOutput> ToStream<TInput, TOutput>()
            {
                throw new NotImplementedException();
            }
        }

        private sealed class SubscriptionFactory : DefinedResource, IAsyncReactiveSubscriptionFactoryDefinition
        {
            private static readonly HashSet<Type> s_factoryTypes = new()
            {
                typeof(IAsyncReactiveQubscriptionFactory<>),
                typeof(IAsyncReactiveQubscriptionFactory<,>),
                typeof(IAsyncReactiveQubscriptionFactory<,,>),
                typeof(IAsyncReactiveQubscriptionFactory<,,,>),
                typeof(IAsyncReactiveQubscriptionFactory<,,,,>),
                typeof(IAsyncReactiveQubscriptionFactory<,,,,,>),
                typeof(IAsyncReactiveQubscriptionFactory<,,,,,,>),
                typeof(IAsyncReactiveQubscriptionFactory<,,,,,,,>),
                typeof(IAsyncReactiveQubscriptionFactory<,,,,,,,,>),
                typeof(IAsyncReactiveQubscriptionFactory<,,,,,,,,,>),
                typeof(IAsyncReactiveQubscriptionFactory<,,,,,,,,,,>),
                typeof(IAsyncReactiveQubscriptionFactory<,,,,,,,,,,,>),
                typeof(IAsyncReactiveQubscriptionFactory<,,,,,,,,,,,,>),
                typeof(IAsyncReactiveQubscriptionFactory<,,,,,,,,,,,,,>),
                typeof(IAsyncReactiveQubscriptionFactory<,,,,,,,,,,,,,,>),
            };

            private bool? _isParameterized;

            public SubscriptionFactory(ReactiveResourceContainer container, IReactiveExpressionServices expressionServices)
                : base(container, expressionServices)
            {
            }

            public override bool IsParameterized
            {
                get
                {
                    if (!base.IsParameterized)
                    {
                        if (!_isParameterized.HasValue)
                        {
                            _isParameterized = Expression.Type.IsGenericType && s_factoryTypes.Contains(Expression.Type.GetGenericTypeDefinition());
                        }

                        return _isParameterized.Value;
                    }

                    return true;
                }
            }

            public IAsyncReactiveQubscriptionFactory ToSubscriptionFactory()
            {
                throw new NotImplementedException();
            }

            public IAsyncReactiveQubscriptionFactory<TArgs> ToSubscriptionFactory<TArgs>()
            {
                throw new NotImplementedException();
            }
        }

        private sealed class Subscription : ProcessResource, IAsyncReactiveSubscriptionProcess
        {
            public Subscription(ReactiveResourceContainer container, IReactiveExpressionServices expressionServices)
                : base(container, expressionServices)
            {
            }

            public IAsyncReactiveQubscription ToSubscription()
            {
                throw new NotImplementedException();
            }
        }
    }

    /// <summary>
    /// A data converter for entities implementing <see cref="IAsyncReactiveResource"/>.
    /// </summary>
    public class ReactiveResourceConverter : ReactiveResourceConverter<object>
    {
        /// <summary>
        /// Instantiates the data converter.
        /// </summary>
        public ReactiveResourceConverter() { }

        /// <summary>
        /// Instantiates the data converter with an expression services implementation
        /// to apply when serializing or deserializing expressions.
        /// </summary>
        /// <param name="expressionServices">The expression services.</param>
        public ReactiveResourceConverter(IReactiveExpressionServices expressionServices)
            : base(expressionServices)
        {
        }
    }
}
