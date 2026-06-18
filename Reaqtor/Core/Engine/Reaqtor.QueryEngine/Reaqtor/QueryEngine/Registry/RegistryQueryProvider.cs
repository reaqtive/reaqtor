// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.CompilerServices;
using System.Linq.CompilerServices.TypeSystem;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

using Reaqtive;

using Reaqtor.Metadata;

namespace Reaqtor.QueryEngine
{
    /// <summary>
    /// Query provider for the reactive entity collections in the registry.
    /// </summary>
    internal class RegistryQueryProvider(ILoggedQueryEngineRegistry registry) : RegistryQueryProviderBase(new Dictionary<string, ITypeErasedDictionary<Uri, object>>
            {
                // Keep in sync with ReactiveMetadataBase. Loose coupling of names; engine should not know about service APIs.
                { "rx://metadata/streamFactories",       TypeErasedDictionary.ToReadOnly(new WrappedEntityCollection<StreamFactoryDefinitionEntity, IReactiveStreamFactoryDefinition>(registry.SubjectFactories, x => x)) },
                { "rx://metadata/subscriptionFactories", TypeErasedDictionary.ToReadOnly(new WrappedEntityCollection<SubscriptionFactoryDefinitionEntity, IReactiveSubscriptionFactoryDefinition>(registry.SubscriptionFactories, x => x)) },
                { "rx://metadata/observables",           TypeErasedDictionary.ToReadOnly(new WrappedEntityCollection<ObservableDefinitionEntity, IReactiveObservableDefinition>(registry.Observables, x => x)) },
                { "rx://metadata/observers",             TypeErasedDictionary.ToReadOnly(new WrappedEntityCollection<ObserverDefinitionEntity, IReactiveObserverDefinition>(registry.Observers, x => x)) },
                { "rx://metadata/streams",               TypeErasedDictionary.ToReadOnly(new WrappedEntityCollection<SubjectEntity, IReactiveStreamProcess>(registry.Subjects, x => x)) },
                { "rx://metadata/subscriptions",         TypeErasedDictionary.ToReadOnly(new WrappedEntityCollection<SubscriptionEntity, IReactiveSubscriptionProcess>(registry.Subscriptions, x => x)) },
            })
    {
    }

    /// <summary>
    /// Query provider for the reactive entity collections in the registry, handing out read-only entities.
    /// </summary>
    internal class OperatorContextRegistryQueryProvider(ILoggedQueryEngineRegistry registry) : RegistryQueryProviderBase(new Dictionary<string, ITypeErasedDictionary<Uri, object>>
            {
                // Keep in sync with ReactiveMetadataBase. Loose coupling of names; engine should not know about service APIs.
                { "rx://metadata/streamFactories",       TypeErasedDictionary.ToReadOnly(new WrappedEntityCollection<StreamFactoryDefinitionEntity, IReactiveStreamFactoryDefinition>(registry.SubjectFactories, x => new ReadonlyReactiveStreamFactoryDefinition(x), new RegistryOptions { ShowUninitialized = true })) },
                { "rx://metadata/subscriptionFactories", TypeErasedDictionary.ToReadOnly(new WrappedEntityCollection<SubscriptionFactoryDefinitionEntity, IReactiveSubscriptionFactoryDefinition>(registry.SubscriptionFactories, x => new ReadonlyReactiveSubscriptionFactoryDefinition(x), new RegistryOptions { ShowUninitialized = true })) },
                { "rx://metadata/observables",           TypeErasedDictionary.ToReadOnly(new WrappedEntityCollection<ObservableDefinitionEntity, IReactiveObservableDefinition>(registry.Observables, x => new ReadonlyReactiveObservableDefinition(x), new RegistryOptions { ShowUninitialized = true })) },
                { "rx://metadata/observers",             TypeErasedDictionary.ToReadOnly(new WrappedEntityCollection<ObserverDefinitionEntity, IReactiveObserverDefinition>(registry.Observers, x => new ReadonlyReactiveObserverDefinition(x), new RegistryOptions { ShowUninitialized = true })) },
                { "rx://metadata/streams",               TypeErasedDictionary.ToReadOnly(new WrappedEntityCollection<SubjectEntity, IReactiveStreamProcess>(registry.Subjects, x => new ReadonlyReactiveStreamProcess(x), new RegistryOptions { ShowUninitialized = true })) },
                { "rx://metadata/subscriptions",         TypeErasedDictionary.ToReadOnly(new WrappedEntityCollection<SubscriptionEntity, IReactiveSubscriptionProcess>(registry.Subscriptions, x => new ReadonlyReactiveSubscriptionProcess(x), new RegistryOptions { ShowUninitialized = true })) },
            })
    {
        private abstract class ReadonlyReactiveResource<TWrapped>(TWrapped entity) : IReactiveResource
            where TWrapped : ReactiveEntity
        {
            public abstract ReactiveEntityKind Kind { get; }

            public Uri Uri => Entity.Uri;

            public Expression Expression => Entity.Expression;

            public object State => Entity.State;

            protected TWrapped Entity { get; } = entity;

            protected void CheckInitialized()
            {
                if (!Entity.IsInitialized)
                    throw new InvalidOperationException("Entity has not been initialized yet.");
            }
        }

        private abstract class ReadonlyReactiveDefinedResource<TWrapped>(TWrapped entity) : ReadonlyReactiveResource<TWrapped>(entity), IReactiveDefinedResource
            where TWrapped : DefinitionEntity
        {
            public bool IsParameterized => Entity.IsParameterized;

            public DateTimeOffset DefinitionTime => Entity.DefinitionTime;
        }

        private abstract class ReadonlyReactiveProcessResource<TWrapped, TDisposable>(TWrapped entity) : ReadonlyReactiveResource<TWrapped>(entity), IReactiveProcessResource
            where TWrapped : RuntimeEntity<TDisposable>
            where TDisposable : IDisposable
        {
            public DateTimeOffset CreationTime => Entity.CreationTime;

            public void Dispose()
            {
                CheckInitialized();

                Entity.Dispose();
            }
        }

        private sealed class ReadonlyReactiveSubscriptionProcess(SubscriptionEntity entity) : ReadonlyReactiveProcessResource<SubscriptionEntity, ISubscription>(entity), IReactiveSubscriptionProcess
        {
            public IReactiveQubscription ToSubscription()
            {
                CheckInitialized();

                return Entity.ToSubscription();
            }

            public override ReactiveEntityKind Kind => ReactiveEntityKind.Subscription;
        }

        private sealed class ReadonlyReactiveStreamProcess(SubjectEntity entity) : ReadonlyReactiveProcessResource<SubjectEntity, IDisposable>(entity), IReactiveStreamProcess
        {
            public IReactiveQubject<TInput, TOutput> ToStream<TInput, TOutput>()
            {
                CheckInitialized();

                return Entity.ToStream<TInput, TOutput>();
            }

            public override ReactiveEntityKind Kind => ReactiveEntityKind.Stream;
        }

        private sealed class ReadonlyReactiveObserverDefinition(ObserverDefinitionEntity entity) : ReadonlyReactiveDefinedResource<ObserverDefinitionEntity>(entity), IReactiveObserverDefinition
        {
            public IReactiveQbserver<T> ToObserver<T>()
            {
                CheckInitialized();

                return Entity.ToObserver<T>();
            }

            public Func<TArgs, IReactiveQbserver<TResult>> ToObserver<TArgs, TResult>()
            {
                CheckInitialized();

                return Entity.ToObserver<TArgs, TResult>();
            }

            public override ReactiveEntityKind Kind => ReactiveEntityKind.Observer;
        }

        private sealed class ReadonlyReactiveObservableDefinition(ObservableDefinitionEntity entity) : ReadonlyReactiveDefinedResource<ObservableDefinitionEntity>(entity), IReactiveObservableDefinition
        {
            public IReactiveQbservable<T> ToObservable<T>()
            {
                CheckInitialized();

                return Entity.ToObservable<T>();
            }

            public Func<TArgs, IReactiveQbservable<TResult>> ToObservable<TArgs, TResult>()
            {
                CheckInitialized();

                return Entity.ToObservable<TArgs, TResult>();
            }

            public override ReactiveEntityKind Kind => ReactiveEntityKind.Observable;
        }

        private sealed class ReadonlyReactiveStreamFactoryDefinition(StreamFactoryDefinitionEntity entity) : ReadonlyReactiveDefinedResource<StreamFactoryDefinitionEntity>(entity), IReactiveStreamFactoryDefinition
        {
            public IReactiveQubjectFactory<TInput, TOutput> ToStreamFactory<TInput, TOutput>()
            {
                CheckInitialized();

                return Entity.ToStreamFactory<TInput, TOutput>();
            }

            public IReactiveQubjectFactory<TInput, TOutput, TArgs> ToStreamFactory<TArgs, TInput, TOutput>()
            {
                CheckInitialized();

                return Entity.ToStreamFactory<TArgs, TInput, TOutput>();
            }

            public override ReactiveEntityKind Kind => ReactiveEntityKind.StreamFactory;
        }

        private sealed class ReadonlyReactiveSubscriptionFactoryDefinition(SubscriptionFactoryDefinitionEntity entity) : ReadonlyReactiveDefinedResource<SubscriptionFactoryDefinitionEntity>(entity), IReactiveSubscriptionFactoryDefinition
        {
            public IReactiveQubscriptionFactory ToSubscriptionFactory()
            {
                CheckInitialized();

                return Entity.ToSubscriptionFactory();
            }

            public IReactiveQubscriptionFactory<TArgs> ToSubscriptionFactory<TArgs>()
            {
                CheckInitialized();

                return Entity.ToSubscriptionFactory<TArgs>();
            }

            public override ReactiveEntityKind Kind => ReactiveEntityKind.SubscriptionFactory;
        }
    }

    internal class AsyncQueryProvider(ILoggedQueryEngineRegistry registry) : RegistryQueryProviderBase(new Dictionary<string, ITypeErasedDictionary<Uri, object>>
            {
                { "rx://metadata/streamFactories",       TypeErasedDictionary.ToReadOnly(new WrappedEntityCollection<StreamFactoryDefinitionEntity, IAsyncReactiveStreamFactoryDefinition>(registry.SubjectFactories, x => new AsyncReactiveStreamFactoryDefinition(x.Uri, x.Expression, x.State))) },
                { "rx://metadata/subscriptionFactories", TypeErasedDictionary.ToReadOnly(new WrappedEntityCollection<SubscriptionFactoryDefinitionEntity, IAsyncReactiveSubscriptionFactoryDefinition>(registry.SubscriptionFactories, x => new AsyncReactiveSubscriptionFactoryDefinition(x.Uri, x.Expression, x.State))) },
                { "rx://metadata/observables",           TypeErasedDictionary.ToReadOnly(new WrappedEntityCollection<ObservableDefinitionEntity, IAsyncReactiveObservableDefinition>(registry.Observables, x => new AsyncReactiveObservableDefinition(x.Uri, x.Expression, x.State))) },
                { "rx://metadata/observers",             TypeErasedDictionary.ToReadOnly(new WrappedEntityCollection<ObserverDefinitionEntity, IAsyncReactiveObserverDefinition>(registry.Observers, x => new AsyncReactiveObserverDefinition(x.Uri, x.Expression, x.State))) },
                { "rx://metadata/streams",               TypeErasedDictionary.ToReadOnly(new WrappedEntityCollection<SubjectEntity, IAsyncReactiveStreamProcess>(registry.Subjects, x => new AsyncReactiveStreamProcess(x.Uri, x.Expression, x.State))) },
                { "rx://metadata/subscriptions",         TypeErasedDictionary.ToReadOnly(new WrappedEntityCollection<SubscriptionEntity, IAsyncReactiveSubscriptionProcess>(registry.Subscriptions, x => new AsyncReactiveSubscriptionProcess(x.Uri, x.Expression, x.State))) },
            })
    {
        private abstract class AsyncReactiveResource(Uri uri, Expression expression, object state) : IAsyncReactiveResource
        {
            public abstract ReactiveEntityKind Kind { get; }

            public Uri Uri { get; } = uri ?? throw new ArgumentNullException(nameof(uri));

            public Expression Expression { get; } = expression;

            public object State { get; } = state;
        }

        private abstract class AsyncReactiveDefinedResource(Uri uri, Expression expression, object state) : AsyncReactiveResource(uri, expression, state), IAsyncReactiveDefinedResource
        {
            public bool IsParameterized => throw new NotImplementedException();

            public DateTimeOffset DefinitionTime => throw new NotImplementedException();
        }

        private abstract class AsyncReactiveProcessResource(Uri uri, Expression expression, object state) : AsyncReactiveResource(uri, expression, state), IAsyncReactiveProcessResource
        {
            public DateTimeOffset CreationTime => throw new NotImplementedException();

#if NET6_0_OR_GREATER || NETSTANDARD2_1
            public ValueTask DisposeAsync() => throw new NotImplementedException();
#else
            public Task DisposeAsync(System.Threading.CancellationToken token) => throw new NotImplementedException();
#endif
        }

        private sealed class AsyncReactiveSubscriptionProcess(Uri uri, Expression expression, object state) : AsyncReactiveProcessResource(uri, expression, state), IAsyncReactiveSubscriptionProcess
        {
            public IAsyncReactiveQubscription ToSubscription() => throw new NotImplementedException();

            public override ReactiveEntityKind Kind => ReactiveEntityKind.Subscription;
        }

        private sealed class AsyncReactiveStreamProcess(Uri uri, Expression expression, object state) : AsyncReactiveProcessResource(uri, expression, state), IAsyncReactiveStreamProcess
        {
            public IAsyncReactiveQubject<TInput, TOutput> ToStream<TInput, TOutput>() => throw new NotImplementedException();

            public override ReactiveEntityKind Kind => ReactiveEntityKind.Stream;
        }

        private sealed class AsyncReactiveObserverDefinition(Uri uri, Expression expression, object state) : AsyncReactiveDefinedResource(uri, expression, state), IAsyncReactiveObserverDefinition
        {
            public IAsyncReactiveQbserver<T> ToObserver<T>() => throw new NotImplementedException();

            public Func<TArgs, IAsyncReactiveQbserver<TResult>> ToObserver<TArgs, TResult>() => throw new NotImplementedException();

            public override ReactiveEntityKind Kind => ReactiveEntityKind.Observer;
        }

        private sealed class AsyncReactiveObservableDefinition(Uri uri, Expression expression, object state) : AsyncReactiveDefinedResource(uri, expression, state), IAsyncReactiveObservableDefinition
        {
            public IAsyncReactiveQbservable<T> ToObservable<T>() => throw new NotImplementedException();

            public Func<TArgs, IAsyncReactiveQbservable<TResult>> ToObservable<TArgs, TResult>() => throw new NotImplementedException();

            public override ReactiveEntityKind Kind => ReactiveEntityKind.Observable;
        }

        private sealed class AsyncReactiveStreamFactoryDefinition(Uri uri, Expression expression, object state) : AsyncReactiveDefinedResource(uri, expression, state), IAsyncReactiveStreamFactoryDefinition
        {
            public IAsyncReactiveQubjectFactory<TInput, TOutput> ToStreamFactory<TInput, TOutput>() => throw new NotImplementedException();

            public IAsyncReactiveQubjectFactory<TInput, TOutput, TArgs> ToStreamFactory<TArgs, TInput, TOutput>() => throw new NotImplementedException();

            public override ReactiveEntityKind Kind => ReactiveEntityKind.StreamFactory;
        }

        private sealed class AsyncReactiveSubscriptionFactoryDefinition(Uri uri, Expression expression, object state) : AsyncReactiveDefinedResource(uri, expression, state), IAsyncReactiveSubscriptionFactoryDefinition
        {
            public IAsyncReactiveQubscriptionFactory ToSubscriptionFactory() => throw new NotImplementedException();

            public IAsyncReactiveQubscriptionFactory<TArgs> ToSubscriptionFactory<TArgs>() => throw new NotImplementedException();

            public override ReactiveEntityKind Kind => ReactiveEntityKind.SubscriptionFactory;
        }
    }

    internal sealed class RegistryOptions
    {
        public bool ShowUninitialized { get; set; }
    }

    internal class RegistryQueryProviderBase : IQueryProvider
    {
#pragma warning disable IDE0034 // Simplify 'default' expression

        private static readonly Lazy<MethodInfo> s_singleOrDefault = new(() => ((MethodInfo)ReflectionHelpers.InfoOf(() => Queryable.SingleOrDefault<T>(default(IQueryable<T>), default(Expression<Func<T, bool>>)))).GetGenericMethodDefinition());
        private static readonly Lazy<MethodInfo> s_select = new(() => ((MethodInfo)ReflectionHelpers.InfoOf(() => Queryable.Select<T, R>(default(IQueryable<T>), default(Expression<Func<T, R>>)))).GetGenericMethodDefinition());
        private static readonly Lazy<MethodInfo> s_any = new(() => ((MethodInfo)ReflectionHelpers.InfoOf(() => Queryable.Any<T>(default(IQueryable<T>), default(Expression<Func<T, bool>>)))).GetGenericMethodDefinition());

#pragma warning restore IDE0034 // Simplify 'default' expression

        private readonly IDictionary<string, ITypeErasedDictionary<Uri, object>> _bindings;

        protected RegistryQueryProviderBase(IDictionary<string, ITypeErasedDictionary<Uri, object>> bindings) => _bindings = bindings;

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression) => throw new NotImplementedException();

        public IQueryable CreateQuery(Expression expression) => throw new NotImplementedException();

        public TResult Execute<TResult>(Expression expression)
        {
            // Narrow query provider implementation; only to support indexing and full enumeration scenarios right now,
            // but using efficient dictionary lookups.

            var result = default(TResult);

            switch (expression.NodeType)
            {
                case ExpressionType.Call:
                    {
                        var call = (MethodCallExpression)expression;

                        var method = call.Method;

                        if (method.IsGenericMethod)
                        {
                            var genMethod = method.GetGenericMethodDefinition();
                            if (genMethod == s_singleOrDefault.Value)
                            {
                                if (TryFindLookupByKey(call, out TryFunc<TResult> getResult))
                                {
                                    getResult(out result);
                                    return result;
                                }
                            }
                            else if (genMethod == s_select.Value)
                            {
                                if (TryFindProjection(call, out result))
                                {
                                    return result;
                                }
                            }
                            else if (genMethod == s_any.Value)
                            {
                                if (TryFindLookupByKey(call, out TryFunc<object> getResult))
                                {
                                    return (TResult)(object)getResult(out _);
                                }
                            }
                        }

                        break;
                    }
                case ExpressionType.Parameter:
                    {
                        var parameter = (ParameterExpression)expression;

                        if (TryFindEnumeration(parameter, out result))
                        {
                            return result;
                        }

                        break;
                    }
            }

            throw new NotSupportedException("Unrecognized metadata query.");
        }

        private delegate bool TryFunc<T>(out T result);

        private bool TryFindLookupByKey<TResult>(MethodCallExpression call, out TryFunc<TResult> result)
        {
            var method = call.Method;

            var elementType = method.GetGenericArguments()[0];
            if (elementType.IsGenericType && elementType.GetGenericTypeDefinition() == typeof(KeyValuePair<,>) && elementType.GetGenericArguments()[0] == typeof(Uri))
            {
                if (call.Arguments[0] is ParameterExpression source && call.Arguments[1].StripQuotes() is LambdaExpression predicate)
                {
                    var collection = source.Name;

                    if (predicate.Body is BinaryExpression filter && filter.NodeType == ExpressionType.Equal)
                    {
                        if (filter.Left is MemberExpression key && filter.Right is ConstantExpression value)
                        {
                            if (key.Expression == predicate.Parameters[0] && key.Member == elementType.GetProperty("Key"))
                            {
                                var id = (Uri)value.Value;

                                result = delegate (out TResult res)
                                {
                                    if (TryLookup(collection, id, out object obj))
                                    {
                                        res = (TResult)obj;
                                        return true;
                                    }

                                    res = default;
                                    return false;
                                };

                                return true;
                            }
                        }
                    }
                }
            }

            result = null;
            return false;
        }

        private bool TryFindProjection<TResult>(MethodCallExpression call, out TResult result)
        {
            var method = call.Method;

            var args = method.GetGenericArguments();
            var elementType = args[0];
            var resultType = args[1];

            if (elementType.IsGenericType && elementType.GetGenericTypeDefinition() == typeof(KeyValuePair<,>) && elementType.GetGenericArguments()[0] == typeof(Uri))
            {
                if (call.Arguments[0] is ParameterExpression source && call.Arguments[1].StripQuotes() is LambdaExpression selector)
                {
                    var collection = source.Name;

                    if (selector.Body is MemberExpression key && key.Expression == selector.Parameters[0])
                    {
                        if (key.Member == elementType.GetProperty("Key") && resultType == typeof(Uri))
                        {
                            result = (TResult)LookupKeys(collection);
                            return true;
                        }
                        else if (key.Member == elementType.GetProperty("Value"))
                        {
                            result = (TResult)LookupValues(collection);
                            return true;
                        }
                    }
                }
            }

            result = default;
            return false;
        }

        private bool TryFindEnumeration<TResult>(ParameterExpression collection, out TResult result)
        {
            var dict = Bind(collection.Name);
            result = dict.AsEnumerable<TResult>();
            return true;
        }

        private bool TryLookup(string collection, Uri id, out object result)
        {
            var dict = Bind(collection);
            var res = dict.TryLookup(id);
            if (res.HasValue)
            {
                result = res.Value;
                return true;
            }

            result = res.Value;
            return false;
        }

        private ITypeErasedDictionary<Uri, object> Bind(string identifier)
        {
            if (_bindings.TryGetValue(identifier, out ITypeErasedDictionary<Uri, object> dictionary))
                return dictionary;

            throw new NotImplementedException("Metadata lookup for collection " + identifier + " is not supported yet.");
        }

        private IEnumerable<Uri> LookupKeys(string collection)
        {
            var dict = Bind(collection);
            return dict.Keys;
        }

        private object LookupValues(string collection)
        {
            var dict = Bind(collection);
            return dict.Values;
        }

        public object Execute(Expression expression) => Execute<object>(expression);

        /// <summary>
        /// Projects <see cref="ReactiveEntityCollection{S,T}"/> to a <see cref="ReactiveEntityCollection{U,R}"/>.
        /// </summary>
        /// <typeparam name="T">The concrete type in the entity collection.</typeparam>
        /// <typeparam name="R">The type exposed by the reactive service.</typeparam>
        protected class WrappedEntityCollection<T, R>(IReactiveEntityCollection<string, T> parent, Func<T, R> conversion, RegistryOptions options) : IReactiveEntityCollection<Uri, R>
            where T : ReactiveEntity
        {
            private readonly IReactiveEntityCollection<string, T> _parent = parent;
            private readonly Func<T, R> _conversion = conversion;
            private readonly RegistryOptions _options = options;

            public WrappedEntityCollection(IReactiveEntityCollection<string, T> parent, Func<T, R> conversion)
                : this(parent, conversion, new RegistryOptions())
            {
            }

            public bool ContainsKey(Uri key) => TryGetValue(key, out _);

            public bool TryAdd(Uri key, R value) => throw new NotSupportedException();

            public bool TryGetValue(Uri key, out R value)
            {
                if (_parent.TryGetValue(key.ToCanonicalString(), out T val) && (_options.ShowUninitialized || val.IsInitialized))
                {
                    value = _conversion(val);
                    return true;
                }
                else
                {
                    value = default;
                    return false;
                }
            }

            public bool TryRemove(Uri key, out R value) => throw new NotSupportedException();

            public ICollection<R> Values => throw new NotSupportedException();

            public IEnumerable<Uri> RemovedKeys => throw new NotSupportedException();

            public void ClearRemovedKeys(IEnumerable<Uri> keys) => throw new NotSupportedException();

            public void Clear() => throw new NotSupportedException();

            public void Dispose() { }

            public IEnumerator<KeyValuePair<Uri, R>> GetEnumerator()
            {
                var src = _parent;
                var filtered = _options.ShowUninitialized ? _parent : _parent.Where(item => item.Value.IsInitialized);
                return filtered.Select(item => new KeyValuePair<Uri, R>(new Uri(item.Key), _conversion(item.Value))).GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        protected static class TypeErasedDictionary
        {
            public static ITypeErasedDictionary<TKey, TValue> ToReadOnly<TKey, TValue>(IReactiveEntityCollection<TKey, TValue> collection)
            {
                return new Impl<TKey, TValue>(collection);
            }

            private sealed class Impl<TKey, TValue>(IReactiveEntityCollection<TKey, TValue> collection) : ITypeErasedDictionary<TKey, TValue>
            {
                private readonly IReactiveEntityCollection<TKey, TValue> _collection = collection;

                public IOption<object> TryLookup(TKey key)
                {
                    if (_collection.TryGetValue(key, out TValue value))
                        return new Some<object>(new KeyValuePair<TKey, TValue>(key, value));

                    return new None<object>(default(KeyValuePair<TKey, TValue>));
                }

                public TResult AsEnumerable<TResult>() => (TResult)_collection;

                public IEnumerable<TKey> Keys => _collection.Select(kvp => kvp.Key);

                public object Values => _collection.Select(kvp => kvp.Value);

                private sealed class None<T>(T value) : IOption<T>
                {
                    public bool HasValue => false;

                    public T Value { get; } = value;
                }

                private sealed class Some<T>(T value) : IOption<T>
                {
                    public bool HasValue => true;

                    public T Value { get; } = value;
                }
            }
        }

        protected interface ITypeErasedDictionary<TKey, out TValue>
        {
            IOption<object> TryLookup(TKey key);

            TResult AsEnumerable<TResult>();

            IEnumerable<TKey> Keys { get; }

            object Values { get; }
        }

        protected interface IOption<out T>
        {
            bool HasValue { get; }

            T Value { get; }
        }
    }
}
