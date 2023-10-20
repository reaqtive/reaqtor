// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq.CompilerServices;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Nuqleon.DataModel;
using Nuqleon.DataModel.Serialization.Json;

using Reaqtor;
using Reaqtor.Hosting.Shared.Serialization;
using Reaqtor.Metadata;

namespace Tests.Microsoft.Hosting.Shared.Serialization
{
    [TestClass]
    public class ReactiveResourceConverterTests
    {
        [TestMethod]
        public void ReactiveResourceConverter_Observable_Roundtrip()
        {
            var observable = new Observable(new Uri("test://uri"), Expression.Default(typeof(object)), null);
            var serializer = new SerializationHelpers(new DataConverter[] { new ReactiveResourceConverter() });
            var serialized = serializer.Serialize(observable);
            var roundtripped = serializer.Deserialize<IAsyncReactiveObservableDefinition>(serialized);

            AssertEqual(observable, roundtripped);

            var unused = default(DateTimeOffset);
            Assert.ThrowsException<NotImplementedException>(() => unused = roundtripped.DefinitionTime);
            Assert.ThrowsException<NotImplementedException>(() => roundtripped.ToObservable<object>());
            Assert.ThrowsException<NotImplementedException>(() => roundtripped.ToObservable<object, object>());
        }

        [TestMethod]
        public void ReactiveResourceConverter_Observer_Roundtrip()
        {
            var observer = new Observer(new Uri("test://uri"), Expression.Default(typeof(object)), null);
            var serializer = new SerializationHelpers(new DataConverter[] { new ReactiveResourceConverter() });
            var serialized = serializer.Serialize(observer);
            var roundtripped = serializer.Deserialize<IAsyncReactiveObserverDefinition>(serialized);

            AssertEqual(observer, roundtripped);

            var unused = default(DateTimeOffset);
            Assert.ThrowsException<NotImplementedException>(() => unused = roundtripped.DefinitionTime);
            Assert.ThrowsException<NotImplementedException>(() => roundtripped.ToObserver<object>());
            Assert.ThrowsException<NotImplementedException>(() => roundtripped.ToObserver<object, object>());
        }

        [TestMethod]
        public void ReactiveResourceConverter_StreamFactory_Roundtrip()
        {
            var streamFactory = new StreamFactory(new Uri("test://uri"), Expression.Default(typeof(object)), null);
            var serializer = new SerializationHelpers(new DataConverter[] { new ReactiveResourceConverter() });
            var serialized = serializer.Serialize(streamFactory);
            var roundtripped = serializer.Deserialize<IAsyncReactiveStreamFactoryDefinition>(serialized);

            AssertEqual(streamFactory, roundtripped);
            Assert.IsFalse(roundtripped.IsParameterized);

            var unused = default(DateTimeOffset);
            Assert.ThrowsException<NotImplementedException>(() => unused = roundtripped.DefinitionTime);
            Assert.ThrowsException<NotImplementedException>(() => roundtripped.ToStreamFactory<object, object>());
            Assert.ThrowsException<NotImplementedException>(() => roundtripped.ToStreamFactory<object, object, object>());
        }

        [TestMethod]
        public void ReactiveResourceConverter_SubscriptionFactory_Roundtrip()
        {
            var subscriptionFactory = new SubscriptionFactory(new Uri("test://uri"), Expression.Default(typeof(object)), null);
            var serializer = new SerializationHelpers(new DataConverter[] { new ReactiveResourceConverter() });
            var serialized = serializer.Serialize(subscriptionFactory);
            var roundtripped = serializer.Deserialize<IAsyncReactiveSubscriptionFactoryDefinition>(serialized);

            AssertEqual(subscriptionFactory, roundtripped);
            Assert.IsFalse(roundtripped.IsParameterized);

            var unused = default(DateTimeOffset);
            Assert.ThrowsException<NotImplementedException>(() => unused = roundtripped.DefinitionTime);
            Assert.ThrowsException<NotImplementedException>(() => roundtripped.ToSubscriptionFactory());
            Assert.ThrowsException<NotImplementedException>(() => roundtripped.ToSubscriptionFactory<object>());
        }

        [TestMethod]
        public void ReactiveResourceConverter_Stream_Roundtrip()
        {
            var stream = new Stream(new Uri("test://uri"), Expression.Default(typeof(object)), null);
            var serializer = new SerializationHelpers(new DataConverter[] { new ReactiveResourceConverter() });
            var serialized = serializer.Serialize(stream);
            var roundtripped = serializer.Deserialize<IAsyncReactiveStreamProcess>(serialized);

            AssertEqual(stream, roundtripped);

            var unused = default(DateTimeOffset);
            Assert.ThrowsException<NotImplementedException>(() => unused = roundtripped.CreationTime);
            Assert.ThrowsException<NotImplementedException>(() => roundtripped.ToStream<object, object>());
        }

        [TestMethod]
        public void ReactiveResourceConverter_Subscription_Roundtrip()
        {
            var subscription = new Subscription(new Uri("test://uri"), Expression.Default(typeof(object)), null);
            var serializer = new SerializationHelpers(new DataConverter[] { new ReactiveResourceConverter() });
            var serialized = serializer.Serialize(subscription);
            var roundtripped = serializer.Deserialize<IAsyncReactiveSubscriptionProcess>(serialized);

            AssertEqual(subscription, roundtripped);

            var unused = default(DateTimeOffset);
            Assert.ThrowsException<NotImplementedException>(() => unused = roundtripped.CreationTime);
            Assert.ThrowsException<NotImplementedException>(() => roundtripped.ToSubscription());
        }

        [TestMethod]
        public void ReactiveResourceConverter_Null_Roundtrip()
        {
            var serializer = new SerializationHelpers(new DataConverter[] { new ReactiveResourceConverter() });
            var serialized = serializer.Serialize<IAsyncReactiveSubscriptionProcess>(null);
            var roundtripped = serializer.Deserialize<IAsyncReactiveSubscriptionProcess>(serialized);

            Assert.IsNull(roundtripped);
        }

        [TestMethod]
        public void ReactiveResourceConverter_Roundtrip_UsesExpressionServices()
        {
            var observable = new Observable(new Uri("test://uri"), Expression.Default(typeof(object)), null);
            var expressionServices = new ExpressionServices();
            var serializer = new SerializationHelpers(new DataConverter[] { new ReactiveResourceConverter(expressionServices) });
            var serialized = serializer.Serialize(observable);
            var roundtripped = serializer.Deserialize<IAsyncReactiveObservableDefinition>(serialized);

            Assert.AreNotEqual(observable, roundtripped);
            Assert.AreEqual(observable.Uri, roundtripped.Uri);
            Assert.AreEqual(observable.State, roundtripped.State);
            Assert.IsTrue(new ExpressionEqualityComparer().Equals(Expression.Default(typeof(int)), roundtripped.Expression));
        }

        [TestMethod]
        public void ReactiveResourceConverter_DefinedResource_Roundtrip_Parameterized()
        {
            var observable = new Observable(new Uri("test://uri"), Expression.Lambda<Func<int, IAsyncReactiveQbservable<int>>>(Expression.Default(typeof(IAsyncReactiveQbservable<int>)), Expression.Parameter(typeof(int))), null);
            var serializer = new SerializationHelpers(new DataConverter[] { new ReactiveResourceConverter() });
            var serialized = serializer.Serialize(observable);
            var roundtripped = serializer.Deserialize<IAsyncReactiveObservableDefinition>(serialized);

            AssertEqual(observable, roundtripped);
            Assert.IsTrue(observable.IsParameterized);
            Assert.IsTrue(roundtripped.IsParameterized);
        }


        [TestMethod]
        public void ReactiveResourceConverter_StreamFactory_Roundtrip_Parameterized_Func()
        {
            var streamFactory = new StreamFactory(new Uri("test://uri"), Expression.Lambda<Func<int, IAsyncReactiveQubject<int, int>>>(Expression.Default(typeof(IAsyncReactiveQubject<int, int>)), Expression.Parameter(typeof(int))), null);
            var serializer = new SerializationHelpers(new DataConverter[] { new ReactiveResourceConverter() });
            var serialized = serializer.Serialize(streamFactory);
            var roundtripped = serializer.Deserialize<IAsyncReactiveStreamFactoryDefinition>(serialized);

            AssertEqual(streamFactory, roundtripped);
            Assert.IsTrue(roundtripped.IsParameterized);
        }

        [TestMethod]
        public void ReactiveResourceConverter_StreamFactory_Roundtrip_Parameterized_StreamFactory()
        {
            var streamFactory = new StreamFactory(new Uri("test://uri"), Expression.Default(typeof(IAsyncReactiveQubjectFactory<int, int, int>)), null);
            var serializer = new SerializationHelpers(new DataConverter[] { new ReactiveResourceConverter() });
            var serialized = serializer.Serialize(streamFactory);
            var roundtripped = serializer.Deserialize<IAsyncReactiveStreamFactoryDefinition>(serialized);

            AssertEqual(streamFactory, roundtripped);
            Assert.IsTrue(roundtripped.IsParameterized);
        }

        [TestMethod]
        public void ReactiveResourceConverter_SubscriptionFactory_Roundtrip_Parameterized_Func()
        {
            var subscriptionFactory = new SubscriptionFactory(new Uri("test://uri"), Expression.Lambda<Func<int, IAsyncReactiveQubscription>>(Expression.Default(typeof(IAsyncReactiveQubscription)), Expression.Parameter(typeof(int))), null);
            var serializer = new SerializationHelpers(new DataConverter[] { new ReactiveResourceConverter() });
            var serialized = serializer.Serialize(subscriptionFactory);
            var roundtripped = serializer.Deserialize<IAsyncReactiveSubscriptionFactoryDefinition>(serialized);

            AssertEqual(subscriptionFactory, roundtripped);
            Assert.IsTrue(roundtripped.IsParameterized);
        }

        [TestMethod]
        public void ReactiveResourceConverter_SubscriptionFactory_Roundtrip_Parameterized_SubscriptionFactory()
        {
            var subscriptionFactory = new SubscriptionFactory(new Uri("test://uri"), Expression.Default(typeof(IAsyncReactiveQubscriptionFactory<int>)), null);
            var serializer = new SerializationHelpers(new DataConverter[] { new ReactiveResourceConverter() });
            var serialized = serializer.Serialize(subscriptionFactory);
            var roundtripped = serializer.Deserialize<IAsyncReactiveSubscriptionFactoryDefinition>(serialized);

            AssertEqual(subscriptionFactory, roundtripped);
            Assert.IsTrue(roundtripped.IsParameterized);
        }

        [TestMethod]
        public void ReactiveResourceConverter_DefinedResource_Roundtrip_State()
        {
            var observable = new Observable(new Uri("test://uri"), Expression.Default(typeof(object)), 42);
            var serializer = new SerializationHelpers(new DataConverter[] { new ReactiveResourceConverter<int>() });
            var serialized = serializer.Serialize(observable);
            var roundtripped = serializer.Deserialize<IAsyncReactiveObservableDefinition>(serialized);

            AssertEqual(observable, roundtripped);
        }

        [TestMethod]
        public void ReactiveResourceConverter_ProcessResource_Roundtrip_State()
        {
            var subscription = new Subscription(new Uri("test://uri"), Expression.Default(typeof(object)), 42);
            var serializer = new SerializationHelpers(new DataConverter[] { new ReactiveResourceConverter<int>() });
            var serialized = serializer.Serialize(subscription);
            var roundtripped = serializer.Deserialize<IAsyncReactiveSubscriptionProcess>(serialized);

            AssertEqual(subscription, roundtripped);
        }

        [TestMethod]
        public void ReactiveResourceConverter_Generic_State()
        {
            var state = new State { Foo = "foo", Bar = 42 };
            var observable = new Observable(new Uri("test://uri"), Expression.Default(typeof(object)), state);
            var serializer = new SerializationHelpers(new DataConverter[] { new ReactiveResourceConverter<State>() });
            var serialized = serializer.Serialize(observable);
            var roundtripped = serializer.Deserialize<IAsyncReactiveObservableDefinition>(serialized);

            AssertEqualCore(observable, roundtripped);
            Assert.IsTrue(roundtripped.State is State);
            Assert.AreEqual(((State)observable.State).Foo, ((State)roundtripped.State).Foo);
            Assert.AreEqual(((State)observable.State).Bar, ((State)roundtripped.State).Bar);

            var unused = default(DateTimeOffset);
            Assert.ThrowsException<NotImplementedException>(() => unused = roundtripped.DefinitionTime);
            Assert.ThrowsException<NotImplementedException>(() => roundtripped.ToObservable<object>());
            Assert.ThrowsException<NotImplementedException>(() => roundtripped.ToObservable<object, object>());
        }

        public class State
        {
            [Mapping("Foo")]
            public string Foo { get; set; }

            [Mapping("Bar")]
            public int Bar { get; set; }
        }

        private static void AssertEqual(IAsyncReactiveDefinedResource x, IAsyncReactiveDefinedResource y)
        {
            AssertEqualCore(x, y);
            Assert.AreEqual(x.State, y.State);
        }

        private static void AssertEqual(IAsyncReactiveProcessResource x, IAsyncReactiveProcessResource y)
        {
            AssertEqualCore(x, y);
            Assert.AreEqual(x.State, y.State);
        }

        private static void AssertEqualCore(IAsyncReactiveResource x, IAsyncReactiveResource y)
        {
            Assert.AreEqual(x.Uri, y.Uri);
            Assert.IsTrue(new ExpressionEqualityComparer().Equals(x.Expression, y.Expression));
        }

        private class Resource : IAsyncReactiveResource
        {
            public Resource(Uri uri, Expression expression)
            {
                Uri = uri;
                Expression = expression;
            }

            public Uri Uri { get; }

            public Expression Expression { get; }
        }

        private class ProcessResource : Resource, IAsyncReactiveProcessResource
        {
            public ProcessResource(Uri uri, Expression expression, object state)
                : base(uri, expression)
            {
                State = state;
            }

            public object State { get; }

            public DateTimeOffset CreationTime => DateTimeOffset.Now;

#if NET6_0
            public ValueTask DisposeAsync() => throw new NotImplementedException();
#else
            public Task DisposeAsync(System.Threading.CancellationToken token) => throw new NotImplementedException();
#endif
        }

        private class DefinedResource : Resource, IAsyncReactiveDefinedResource
        {
            public DefinedResource(Uri uri, Expression expression, object state)
                : base(uri, expression)
            {
                State = state;
            }

            public bool IsParameterized => Expression is LambdaExpression;

            public object State { get; }

            public DateTimeOffset DefinitionTime => DateTimeOffset.Now;
        }

        private sealed class Observable : DefinedResource, IAsyncReactiveObservableDefinition
        {
            public Observable(Uri uri, Expression expression, object state)
                : base(uri, expression, state)
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
            public Observer(Uri uri, Expression expression, object state)
                : base(uri, expression, state)
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
            public StreamFactory(Uri uri, Expression expression, object state)
                : base(uri, expression, state)
            {
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

        private sealed class SubscriptionFactory : DefinedResource, IAsyncReactiveSubscriptionFactoryDefinition
        {
            public SubscriptionFactory(Uri uri, Expression expression, object state)
                : base(uri, expression, state)
            {
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

        private sealed class Stream : ProcessResource, IAsyncReactiveStreamProcess
        {
            public Stream(Uri uri, Expression expression, object state)
                : base(uri, expression, state)
            {
            }

            public IAsyncReactiveQubject<TInput, TOutput> ToStream<TInput, TOutput>()
            {
                throw new NotImplementedException();
            }
        }

        private sealed class Subscription : ProcessResource, IAsyncReactiveSubscriptionProcess
        {
            public Subscription(Uri uri, Expression expression, object state)
                : base(uri, expression, state)
            {
            }

            public IAsyncReactiveQubscription ToSubscription()
            {
                throw new NotImplementedException();
            }
        }

        private sealed class ExpressionServices : ReactiveExpressionServices
        {
            public ExpressionServices()
                : base(typeof(IReactiveClientProxy))
            {
            }

            public override Expression Normalize(Expression expression)
            {
                var normalized = base.Normalize(expression);

                return new Visitor().Visit(normalized);
            }

            private sealed class Visitor : ExpressionVisitor
            {
                protected override Expression VisitDefault(DefaultExpression node)
                {
                    if (node.Type == typeof(object))
                    {
                        return Expression.Default(typeof(int));
                    }

                    return base.VisitDefault(node);
                }
            }
        }
    }
}
