// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.CompilerServices;
using System.Linq.Expressions;
using System.Reflection;

using Reaqtor.Metadata;

namespace Reaqtor.QueryEngine.Mocks
{
    public class MockLazyReactiveEngineProvider : IReactiveEngineProvider
    {
        private readonly MetaDataQueryProvider _metadataProvider = new();
        private readonly SubjectManager _subjectManager;

        public MockLazyReactiveEngineProvider()
            : this(new SubjectManager())
        {
        }

        public MockLazyReactiveEngineProvider(SubjectManager subjectManager)
        {
            _subjectManager = subjectManager ?? throw new ArgumentNullException(nameof(subjectManager));
        }

        public void CreateSubscription(Uri subscriptionUri, Expression subscription, object state)
        {
            // Other stuff

            _metadataProvider.Subscriptions[subscriptionUri] = new SubscriptionMetadata(subscriptionUri, subscription, state);
        }

        public void DeleteSubscription(Uri subscriptionUri)
        {
            // Other stuff

            _metadataProvider.Subscriptions.Remove(subscriptionUri);
        }

        public void CreateStream(Uri streamUri, Expression stream, object state)
        {
            // Other stuff

            _metadataProvider.Streams[streamUri] = new StreamMetadata(streamUri, stream, state);
        }

        public void DeleteStream(Uri streamUri)
        {
            // Other stuff

            _metadataProvider.Streams.Remove(streamUri);
        }

        public IReactiveObserver<T> GetObserver<T>(Uri observerUri)
        {
            return new Observer<T>(_subjectManager.GetExternalObserver<T>(observerUri));
        }

        public void DefineObservable(Uri observableUri, Expression observable, object state)
        {
            // Other stuff

            _metadataProvider.Observables[observableUri] = new ObservableMetadata(observableUri, observable, state);
        }

        public void UndefineObservable(Uri observableUri)
        {
            // Other stuff

            _metadataProvider.Observables.Remove(observableUri);
        }

        public void DefineObserver(Uri observerUri, Expression observer, object state)
        {
            // Other stuff

            _metadataProvider.Observers[observerUri] = new ObserverMetadata(observerUri, observer, state);
        }

        public void UndefineObserver(Uri observerUri)
        {
            // Other stuff

            _metadataProvider.Observers.Remove(observerUri);
        }

        public void DefineStreamFactory(Uri streamFactoryUri, Expression streamFactory, object state)
        {
            // Other stuff

            _metadataProvider.StreamFactories[streamFactoryUri] = new StreamFactoryMetadata(streamFactoryUri, streamFactory, state);
        }

        public void UndefineStreamFactory(Uri streamFactoryUri)
        {
            // Other stuff

            _metadataProvider.StreamFactories.Remove(streamFactoryUri);
        }

        public void DefineSubscriptionFactory(Uri subscriptionFactoryUri, Expression subscriptionFactory, object state)
        {
            // Other stuff

            _metadataProvider.SubscriptionFactories[subscriptionFactoryUri] = new SubscriptionFactoryMetadata(subscriptionFactoryUri, subscriptionFactory, state);
        }

        public void UndefineSubscriptionFactory(Uri subscriptionFactoryUri)
        {
            // Other stuff

            _metadataProvider.SubscriptionFactories.Remove(subscriptionFactoryUri);
        }

        public IQueryProvider Provider => _metadataProvider;

        private sealed class MetaDataQueryProvider : IQueryProvider
        {
            private static readonly MethodInfo _singleOrDefaultObservable = (MethodInfo)ReflectionHelpers.InfoOf((IQueryable<KeyValuePair<Uri, IReactiveObservableDefinition>> q) => q.SingleOrDefault(i => true));
            private static readonly MethodInfo _singleOrDefaultObserver = (MethodInfo)ReflectionHelpers.InfoOf((IQueryable<KeyValuePair<Uri, IReactiveObserverDefinition>> q) => q.SingleOrDefault(i => true));
            private static readonly MethodInfo _singleOrDefaultStream = (MethodInfo)ReflectionHelpers.InfoOf((IQueryable<KeyValuePair<Uri, IReactiveStreamProcess>> q) => q.SingleOrDefault(i => true));

            private static readonly Type _observableDictionaryType = typeof(IQueryableDictionary<Uri, IReactiveObservableDefinition>);
            private static readonly Type _observerDictionaryType = typeof(IQueryableDictionary<Uri, IReactiveObserverDefinition>);
            private static readonly Type _streamDictionaryType = typeof(IQueryableDictionary<Uri, IReactiveStreamProcess>);

            public MetaDataQueryProvider()
            {
                StreamFactories = new Dictionary<Uri, IReactiveStreamFactoryDefinition>();
                SubscriptionFactories = new Dictionary<Uri, IReactiveSubscriptionFactoryDefinition>();
                Streams = new Dictionary<Uri, IReactiveStreamProcess>();
                Observables = new Dictionary<Uri, IReactiveObservableDefinition>();
                Observers = new Dictionary<Uri, IReactiveObserverDefinition>();
                Subscriptions = new Dictionary<Uri, IReactiveSubscriptionProcess>();
            }

            public IDictionary<Uri, IReactiveStreamFactoryDefinition> StreamFactories { get; }

            public IDictionary<Uri, IReactiveSubscriptionFactoryDefinition> SubscriptionFactories { get; }

            public IDictionary<Uri, IReactiveStreamProcess> Streams { get; }

            public IDictionary<Uri, IReactiveObservableDefinition> Observables { get; }

            public IDictionary<Uri, IReactiveObserverDefinition> Observers { get; }

            public IDictionary<Uri, IReactiveSubscriptionProcess> Subscriptions { get; }

            public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
            {
                var expr = new Binder(this).Visit(expression);

                var res = Array.Empty<object>().AsQueryable().Provider.CreateQuery<TElement>(expr);

                return res;
            }

            public IQueryable CreateQuery(Expression expression)
            {
                throw new NotImplementedException();
            }

            public TResult Execute<TResult>(Expression expression)
            {
                var expr = new Binder(this).Visit(expression);

                if (!TryFastExecute(expression, out TResult res))
                {
                    res = Array.Empty<object>().AsQueryable().Provider.Execute<TResult>(expr);
                }

                return res;
            }

            private bool GetValue(string name, Uri uri, out object res)
            {
                switch (name)
                {
                    case Constants.MetadataObservablesUri:
                        {
                            if (Observables.TryGetValue(uri, out var def))
                            {
                                res = new KeyValuePair<Uri, IReactiveObservableDefinition>(uri, def);
                                return true;
                            }
                            break;
                        }
                    case Constants.MetadataObserversUri:
                        {
                            if (Observers.TryGetValue(uri, out var def))
                            {
                                res = new KeyValuePair<Uri, IReactiveObserverDefinition>(uri, def);
                                return true;
                            }
                            break;
                        }
                    case Constants.MetadataStreamsUri:
                        {
                            if (Streams.TryGetValue(uri, out var def))
                            {
                                res = new KeyValuePair<Uri, IReactiveStreamProcess>(uri, def);
                                return true;
                            }
                            break;
                        }
                }

                res = null;
                return false;
            }

            private static bool IsValid(string name, Type type)
            {
                return
                    (name == Constants.MetadataObservablesUri && type == _observableDictionaryType) ||
                    (name == Constants.MetadataObserversUri && type == _observerDictionaryType) ||
                    (name == Constants.MetadataStreamsUri && type == _streamDictionaryType);
            }

            private static LambdaExpression GetLambda(string name)
            {
                switch (name)
                {
                    case Constants.MetadataObservablesUri:
                        {
                            Expression<Func<KeyValuePair<Uri, IReactiveObservableDefinition>, Uri>> def = item => item.Key;
                            return def;
                        }
                    case Constants.MetadataObserversUri:
                        {
                            Expression<Func<KeyValuePair<Uri, IReactiveObserverDefinition>, Uri>> def = item => item.Key;
                            return def;
                        }
                    case Constants.MetadataStreamsUri:
                        {
                            Expression<Func<KeyValuePair<Uri, IReactiveStreamProcess>, Uri>> def = item => item.Key;
                            return def;
                        }
                }

                return null;
            }

            private static bool IsSingleOrDefault(MethodInfo mi)
            {
                return mi == _singleOrDefaultObservable || mi == _singleOrDefaultObserver || mi == _singleOrDefaultStream;
            }

            private bool TryFastExecute<TResult>(Expression expression, out TResult result)
            {
                result = default;

                if (expression.NodeType == ExpressionType.Call)
                {
                    var methodCall = (MethodCallExpression)expression;
                    if (IsSingleOrDefault(methodCall.Method) &&
                        methodCall.Arguments != null &&
                        methodCall.Arguments.Count == 2 &&
                        methodCall.Arguments[0].NodeType == ExpressionType.Parameter)
                    {
                        var firstArg = (ParameterExpression)methodCall.Arguments[0];
                        if (IsValid(firstArg.Name, firstArg.Type) &&
                            methodCall.Arguments[1].NodeType == ExpressionType.Quote)
                        {
                            var secondArg = ((UnaryExpression)methodCall.Arguments[1]).Unquote();
                            if (secondArg.Parameters.Count == 1 &&
                                secondArg.Parameters[0].Name == "item" &&
                                // secondArg.Parameters[0].Type == typeof(KeyValuePair<Uri, IReactiveObservableDefinition>) &&
                                secondArg.Body.NodeType == ExpressionType.Equal)
                            {
                                var body = (BinaryExpression)secondArg.Body;
                                LambdaExpression def = GetLambda(firstArg.Name);
                                if (new GlobalParameterComparator().Equals(def.Body, body.Left) &&
                                    body.Right.NodeType == ExpressionType.Constant)
                                {
                                    var search = (ConstantExpression)body.Right;
                                    if (search.Type == typeof(Uri))
                                    {
                                        var key = (Uri)search.Value;
                                        var found = GetValue(firstArg.Name, key, out var temp);
                                        if (found)
                                        {
                                            result = (TResult)temp;
                                            return true;
                                        }
                                        else
                                        {
                                            result = default;
                                            return true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                return false;
            }

            private sealed class GlobalParameterComparator : ExpressionEqualityComparator
            {
                protected override bool EqualsGlobalParameter(ParameterExpression x, ParameterExpression y)
                {
                    return x.Name == y.Name && Equals(x.Type, y.Type);
                }
            }

            public object Execute(Expression expression)
            {
                throw new NotImplementedException();
            }

            private sealed class Binder : ScopedExpressionVisitor<ParameterExpression>
            {
                private readonly MetaDataQueryProvider _parent;

                public Binder(MetaDataQueryProvider parent)
                {
                    _parent = parent;
                }

                protected override ParameterExpression GetState(ParameterExpression parameter)
                {
                    return parameter;
                }

                protected override Expression VisitParameter(ParameterExpression node)
                {
                    if (IsUnboundParameter(node))
                    {
                        switch (node.Name)
                        {
                            case Constants.MetadataStreamFactoriesUri:
                                return Expression.Constant(_parent.StreamFactories.AsQueryable());
                            case Constants.MetadataSubscriptionFactoriesUri:
                                return Expression.Constant(_parent.SubscriptionFactories.AsQueryable());
                            case Constants.MetadataStreamsUri:
                                return Expression.Constant(_parent.Streams.AsQueryable());
                            case Constants.MetadataObservablesUri:
                                return Expression.Constant(_parent.Observables.AsQueryable());
                            case Constants.MetadataObserversUri:
                                return Expression.Constant(_parent.Observers.AsQueryable());
                            case Constants.MetadataSubscriptionsUri:
                                return Expression.Constant(_parent.Subscriptions.AsQueryable());
                        }
                    }

                    return base.VisitParameter(node);
                }

                private bool IsUnboundParameter(ParameterExpression parameter) => !TryLookup(parameter, out _);
            }
        }

        private class ResourceMetadata : IReactiveResource
        {
            public ResourceMetadata(Uri uri, Expression expression)
            {
                Uri = uri;
                Expression = expression;
            }

            public Uri Uri { get; }

            public Expression Expression { get; }
        }

        private class DefinedMetadata : ResourceMetadata, IReactiveDefinedResource
        {
            public DefinedMetadata(Uri uri, Expression expression, object state)
                : base(uri, expression)
            {
                State = state;
                DefinitionTime = DateTimeOffset.UtcNow;
            }

            public bool IsParameterized => Expression.NodeType == ExpressionType.Lambda;

            public object State { get; }

            public DateTimeOffset DefinitionTime { get; }

            public void Undefine()
            {
                throw new NotImplementedException();
            }

            public void Dispose()
            {
                throw new NotImplementedException();
            }
        }

        private class ProcessMetadata : ResourceMetadata, IReactiveProcessResource
        {
            public ProcessMetadata(Uri uri, Expression expression, object state)
                : base(uri, expression)
            {
                State = state;
                CreationTime = DateTimeOffset.UtcNow;
            }

            public object State { get; }

            public DateTimeOffset CreationTime { get; }

            public void Dispose()
            {
                throw new NotImplementedException();
            }
        }

        private class StreamFactoryMetadata : DefinedMetadata, IReactiveStreamFactoryDefinition
        {
            public StreamFactoryMetadata(Uri uri, Expression expression, object state)
                : base(uri, expression, state)
            {
            }

            public IReactiveQubjectFactory<TInput, TOutput> ToStreamFactory<TInput, TOutput>()
            {
                throw new InvalidOperationException();
            }

            public IReactiveQubjectFactory<TInput, TOutput, TArgs> ToStreamFactory<TArgs, TInput, TOutput>()
            {
                throw new InvalidOperationException();
            }
        }

        private class SubscriptionFactoryMetadata : DefinedMetadata, IReactiveSubscriptionFactoryDefinition
        {
            public SubscriptionFactoryMetadata(Uri uri, Expression expression, object state)
                : base(uri, expression, state)
            {
            }

            public IReactiveQubscriptionFactory ToSubscriptionFactory()
            {
                throw new InvalidOperationException();
            }

            public IReactiveQubscriptionFactory<TArgs> ToSubscriptionFactory<TArgs>()
            {
                throw new InvalidOperationException();
            }
        }

        private sealed class StreamMetadata : ProcessMetadata, IReactiveStreamProcess
        {
            public StreamMetadata(Uri uri, Expression expression, object state)
                : base(uri, expression, state)
            {
            }

            public IReactiveQubject<TInput, TOutput> ToStream<TInput, TOutput>()
            {
                throw new NotImplementedException();
            }
        }

        private sealed class ObservableMetadata : DefinedMetadata, IReactiveObservableDefinition
        {
            public ObservableMetadata(Uri uri, Expression expression, object state)
                : base(uri, expression, state)
            {
            }

            public IReactiveQbservable<T> ToObservable<T>()
            {
                throw new NotImplementedException();
            }

            public Func<TArgs, IReactiveQbservable<TResult>> ToObservable<TArgs, TResult>()
            {
                throw new NotImplementedException();
            }
        }

        private sealed class ObserverMetadata : DefinedMetadata, IReactiveObserverDefinition
        {
            public ObserverMetadata(Uri uri, Expression expression, object state)
                : base(uri, expression, state)
            {
            }

            public IReactiveQbserver<T> ToObserver<T>()
            {
                throw new NotImplementedException();
            }

            public Func<TArgs, IReactiveQbserver<TResult>> ToObserver<TArgs, TResult>()
            {
                throw new NotImplementedException();
            }
        }

        private sealed class SubscriptionMetadata : ProcessMetadata, IReactiveSubscriptionProcess
        {
            public SubscriptionMetadata(Uri uri, Expression expression, object state)
                : base(uri, expression, state)
            {
            }

            public IReactiveQubscription ToSubscription()
            {
                throw new NotImplementedException();
            }
        }

        private sealed class Observer<T> : IReactiveObserver<T>
        {
            private readonly IObserver<T> _innerObserver;

            public Observer(IObserver<T> innerObserver) => _innerObserver = innerObserver;

            public void OnNext(T value) => _innerObserver.OnNext(value);

            public void OnError(Exception error) => _innerObserver.OnError(error);

            public void OnCompleted() => _innerObserver.OnCompleted();
        }
    }
}
