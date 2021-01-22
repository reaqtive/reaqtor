// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.CompilerServices;
using System.Linq.Expressions;
using System.Reflection;

using Nuqleon.DataModel;
using Reaqtor;
using Reaqtor.Metadata;
using Reaqtor.Hosting.Shared.Serialization;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Microsoft.Hosting.Shared.Serialization
{
    [TestClass]
    public class UnifyingSerializationHelpersTests
    {
        [TestMethod]
        public void UnifyingSerializationHelpers_FindAndUnify_Observable()
        {
            var metadata = new MetadataMock();
            var uri = new Uri("test://id");
            metadata.Observables.Add(uri, new MockObservable<int>(uri));

            var parameter = ExpressionSlim.Parameter(typeof(IAsyncReactiveQbservable<int>).ToTypeSlim(), uri.ToCanonicalString());
            var mappings = UnifyingSerializationHelpers.FindAndUnify(parameter, metadata);
            Assert.AreEqual(0, mappings.Count());
        }

        [TestMethod]
        public void UnifyingSerializationHelpers_FindAndUnify_Observer()
        {
            var metadata = new MetadataMock();
            var uri = new Uri("test://id");
            metadata.Observers.Add(uri, new MockObserver<int>(uri));

            var parameter = ExpressionSlim.Parameter(typeof(IAsyncReactiveQbserver<int>).ToTypeSlim(), uri.ToCanonicalString());
            var mappings = UnifyingSerializationHelpers.FindAndUnify(parameter, metadata);
            Assert.AreEqual(0, mappings.Count());
        }

        [TestMethod]
        public void UnifyingSerializationHelpers_FindAndUnify_StreamFactory()
        {
            var metadata = new MetadataMock();
            var uri = new Uri("test://id");
            metadata.StreamFactories.Add(uri, new MockStreamFactory<int>(uri));

            var parameter = ExpressionSlim.Parameter(typeof(IAsyncReactiveQubjectFactory<int, int>).ToTypeSlim(), uri.ToCanonicalString());
            var mappings = UnifyingSerializationHelpers.FindAndUnify(parameter, metadata);
            Assert.AreEqual(0, mappings.Count());
        }

        [TestMethod]
        public void UnifyingSerializationHelpers_FindAndUnify_SubscriptionFactory()
        {
            var metadata = new MetadataMock();
            var uri = new Uri("test://id");
            metadata.SubscriptionFactories.Add(uri, new MockSubscriptionFactory(uri));

            var parameter = ExpressionSlim.Parameter(typeof(IAsyncReactiveQubscriptionFactory).ToTypeSlim(), uri.ToCanonicalString());
            var mappings = UnifyingSerializationHelpers.FindAndUnify(parameter, metadata);
            Assert.AreEqual(0, mappings.Count());
        }

        [TestMethod]
        public void UnifyingSerializationHelpers_FindAndUnify_Stream()
        {
            var metadata = new MetadataMock();
            var uri = new Uri("test://id");
            metadata.Streams.Add(uri, new MockStream<int>(uri));

            var parameter = ExpressionSlim.Parameter(typeof(IAsyncReactiveQubject<int, int>).ToTypeSlim(), uri.ToCanonicalString());
            var mappings = UnifyingSerializationHelpers.FindAndUnify(parameter, metadata);
            Assert.AreEqual(0, mappings.Count());
        }

        [TestMethod]
        public void UnifyingSerializationHelpers_FindAndUnify_Subscription()
        {
            var metadata = new MetadataMock();
            var uri = new Uri("test://id");
            metadata.Subscriptions.Add(uri, new MockSubscription(uri));

            var parameter = ExpressionSlim.Parameter(typeof(IAsyncReactiveQubscription).ToTypeSlim(), uri.ToCanonicalString());
            var mappings = UnifyingSerializationHelpers.FindAndUnify(parameter, metadata);
            Assert.AreEqual(0, mappings.Count());
        }

        [TestMethod]
        public void UnifyingSerializationHelpers_FindAndUnify_Parameterized()
        {
            var metadata = new MetadataMock();
            var uri = new Uri("test://id");
            metadata.Observables.Add(uri, new MockParameterizedObservable<int, int>(uri));

            var parameter = ExpressionSlim.Parameter(typeof(Func<int, IAsyncReactiveQbservable<int>>).ToTypeSlim(), uri.ToCanonicalString());
            var mappings = UnifyingSerializationHelpers.FindAndUnify(parameter, metadata);
            Assert.AreEqual(0, mappings.Count());
        }

        [TestMethod]
        public void UnifyingSerializationHelpers_FindAndUnify_Enums()
        {
            var metadata = new MetadataMock();
            var uri = new Uri("test://id");
            metadata.Observables.Add(uri, new MockObservable<TypeWithEnum>(uri));

            var structuralType = StructuralTypeSlimReference.Create(hasValueEqualitySemantics: true, StructuralTypeSlimKind.Record);
            structuralType.AddProperty(structuralType.GetProperty("test://enum", typeof(int).ToTypeSlim(), EmptyReadOnlyCollection<TypeSlim>.Instance, canWrite: true));
            var observerableType = TypeSlim.Generic(((GenericDefinitionTypeSlim)typeof(IAsyncReactiveQbservable<>).ToTypeSlim()), new TypeSlim[] { structuralType }.ToReadOnly());
            var parameter = ExpressionSlim.Parameter(observerableType, uri.ToCanonicalString());
            var mappings = UnifyingSerializationHelpers.FindAndUnify(parameter, metadata);
            Assert.AreEqual(1, mappings.Count());
        }

        [TestMethod]
        public void UnifyingSerializationHelpers_FindAndUnify_MismatchProperty()
        {
            var metadata = new MetadataMock();
            var uri = new Uri("test://id");
            metadata.Observables.Add(uri, new MockObservable<TypeWithEnum>(uri));

            var structuralType = StructuralTypeSlimReference.Create(hasValueEqualitySemantics: true, StructuralTypeSlimKind.Record);
            structuralType.AddProperty(structuralType.GetProperty("eg://enum", typeof(int).ToTypeSlim(), EmptyReadOnlyCollection<TypeSlim>.Instance, canWrite: true));
            var observerableType = TypeSlim.Generic(((GenericDefinitionTypeSlim)typeof(IAsyncReactiveQbservable<>).ToTypeSlim()), new TypeSlim[] { structuralType }.ToReadOnly());
            var parameter = ExpressionSlim.Parameter(observerableType, uri.ToCanonicalString());
            Assert.ThrowsException<InvalidOperationException>(() => UnifyingSerializationHelpers.FindAndUnify(parameter, metadata));
        }

        [TestMethod]
        public void UnifyingSerializationHelpers_Roundtrip_WithEnums()
        {
            var metadata = new MetadataMock();
            var uri1 = new Uri("test://id/1");
            var uri2 = new Uri("test://id/2");
            metadata.Observables.Add(uri1, new MockObservable<EntityEnum>(uri1));
            metadata.Observables.Add(uri2, new MockObservable<TypeWithEnum>(uri2));

            Expression<Func<IAsyncReactiveQbservable<EntityEnum>, IAsyncReactiveQbservable<TypeWithEnum>, IAsyncReactiveQbservable<TypeWithEnum>>> f = (io1, io2) => Concat(Select(io1, x => new TypeWithEnum { Enum = x }), io2);
            var invoked = BetaReducer.Reduce(
                Expression.Invoke(
                    f,
                    Expression.Parameter(typeof(IAsyncReactiveQbservable<EntityEnum>), uri1.ToCanonicalString()),
                    Expression.Parameter(typeof(IAsyncReactiveQbservable<TypeWithEnum>), uri2.ToCanonicalString())));
            var normalized = new ReactiveExpressionServices(typeof(object)).Normalize(invoked);

            var clientSerializer = new ClientSerializationHelpers();
            var serviceSerializer = new UnifyingSerializationHelpers(metadata);
            var bonsai = clientSerializer.Serialize(normalized);
            var roundtrip = serviceSerializer.Deserialize<Expression>(bonsai);
            Assert.AreEqual(typeof(IAsyncReactiveQbservable<TypeWithEnum>), roundtrip.Type);
        }

        private enum EntityEnum
        {
            [Mapping("eg://x")]
            X,
        }

        private sealed class TypeWithEnum
        {
            [Mapping("test://enum")]
            public EntityEnum Enum { get; set; }
        }

        [KnownResource("test://select")]
        public static IAsyncReactiveQbservable<R> Select<T, R>(IAsyncReactiveQbservable<T> source, Func<T, R> selector)
        {
            throw new NotImplementedException();
        }

        [KnownResource("test://concat")]
        public static IAsyncReactiveQbservable<T> Concat<T>(IAsyncReactiveQbservable<T> first, IAsyncReactiveQbservable<T> second)
        {
            throw new NotImplementedException();
        }

        #region Helper Classes

        private sealed class MetadataMock : IReactiveMetadata
        {
            public MockDictionary<IReactiveStreamFactoryDefinition> StreamFactories { get; } = new();

            public MockDictionary<IReactiveSubscriptionFactoryDefinition> SubscriptionFactories { get; } = new();

            public MockDictionary<IReactiveStreamProcess> Streams { get; } = new();

            public MockDictionary<IReactiveObservableDefinition> Observables { get; } = new();

            public MockDictionary<IReactiveObserverDefinition> Observers { get; } = new();

            public MockDictionary<IReactiveSubscriptionProcess> Subscriptions { get; } = new();

            IQueryableDictionary<Uri, IReactiveStreamFactoryDefinition> IReactiveMetadata.StreamFactories => StreamFactories;

            IQueryableDictionary<Uri, IReactiveSubscriptionFactoryDefinition> IReactiveMetadata.SubscriptionFactories => SubscriptionFactories;

            IQueryableDictionary<Uri, IReactiveStreamProcess> IReactiveMetadata.Streams => Streams;

            IQueryableDictionary<Uri, IReactiveObservableDefinition> IReactiveMetadata.Observables => Observables;

            IQueryableDictionary<Uri, IReactiveObserverDefinition> IReactiveMetadata.Observers => Observers;

            IQueryableDictionary<Uri, IReactiveSubscriptionProcess> IReactiveMetadata.Subscriptions => Subscriptions;
        }

        private sealed class MockDictionary<T> : QueryableDictionaryBase<Uri, T>
        {
            private readonly IDictionary<Uri, T> _dictionary;
            private readonly IQueryable<KeyValuePair<Uri, T>> _queryable;

            public MockDictionary()
            {
                _dictionary = new Dictionary<Uri, T>();
                _queryable = _dictionary.AsQueryable();
            }

            public void Add(Uri key, T value)
            {
                _dictionary.Add(key, value);
            }

            public override IEnumerator<KeyValuePair<Uri, T>> GetEnumerator()
            {
                return _dictionary.GetEnumerator();
            }

            public override Expression Expression => Expression.Constant(_queryable);

            public override IQueryProvider Provider => _queryable.Provider;
        }

        private class MockResource<T> : IReactiveResource
        {
            public MockResource(Uri uri)
            {
                Uri = uri;
            }

            public Uri Uri { get; }

            public Expression Expression => Expression.Default(typeof(T));
        }

        private class MockDefinedResource<T> : MockResource<T>, IReactiveDefinedResource
        {
            public MockDefinedResource(Uri uri)
                : base(uri)
            {
            }

            public bool IsParameterized => typeof(T).FindGenericType(typeof(Func<,>)) != null;

            public object State => null;

            public DateTimeOffset DefinitionTime => throw new NotImplementedException();

            public void Undefine()
            {
                throw new NotImplementedException();
            }

            public void Dispose()
            {
                throw new NotImplementedException();
            }
        }

        private class MockProcessResource<T> : MockResource<T>, IReactiveProcessResource
        {
            public MockProcessResource(Uri uri)
                : base(uri)
            {
            }

            public object State => null;

            public DateTimeOffset CreationTime => throw new NotImplementedException();

            public void Dispose()
            {
                throw new NotImplementedException();
            }
        }

        private sealed class MockObservable<T> : MockDefinedResource<IReactiveQbservable<T>>, IReactiveObservableDefinition
        {
            public MockObservable(Uri uri)
                : base(uri)
            {
            }

            public IReactiveQbservable<TResult> ToObservable<TResult>()
            {
                throw new NotImplementedException();
            }

            public Func<TArgs, IReactiveQbservable<TResult>> ToObservable<TArgs, TResult>()
            {
                throw new NotImplementedException();
            }
        }

        private sealed class MockParameterizedObservable<TParam, T> : MockDefinedResource<Func<TParam, IReactiveQbservable<T>>>, IReactiveObservableDefinition
        {
            public MockParameterizedObservable(Uri uri)
                : base(uri)
            {
            }

            public IReactiveQbservable<TResult> ToObservable<TResult>()
            {
                throw new NotImplementedException();
            }

            public Func<TArgs, IReactiveQbservable<TResult>> ToObservable<TArgs, TResult>()
            {
                throw new NotImplementedException();
            }
        }

        private sealed class MockObserver<T> : MockDefinedResource<IReactiveQbserver<T>>, IReactiveObserverDefinition
        {
            public MockObserver(Uri uri)
                : base(uri)
            {
            }

            public IReactiveQbserver<TResult> ToObserver<TResult>()
            {
                throw new NotImplementedException();
            }

            public Func<TArgs, IReactiveQbserver<TResult>> ToObserver<TArgs, TResult>()
            {
                throw new NotImplementedException();
            }
        }

        private sealed class MockStreamFactory<T> : MockDefinedResource<IReactiveQubjectFactory<T, T>>, IReactiveStreamFactoryDefinition
        {
            public MockStreamFactory(Uri uri)
                : base(uri)
            {
            }

            public IReactiveQubjectFactory<TInput, TOutput> ToStreamFactory<TInput, TOutput>()
            {
                throw new NotImplementedException();
            }

            public IReactiveQubjectFactory<TInput, TOutput, TArgs> ToStreamFactory<TArgs, TInput, TOutput>()
            {
                throw new NotImplementedException();
            }
        }

        private sealed class MockSubscriptionFactory : MockDefinedResource<IReactiveQubscriptionFactory>, IReactiveSubscriptionFactoryDefinition
        {
            public MockSubscriptionFactory(Uri uri)
                : base(uri)
            {
            }

            public IReactiveQubscriptionFactory ToSubscriptionFactory()
            {
                throw new NotImplementedException();
            }

            public IReactiveQubscriptionFactory<TArgs> ToSubscriptionFactory<TArgs>()
            {
                throw new NotImplementedException();
            }
        }

        private sealed class MockStream<T> : MockProcessResource<IReactiveQubject<T>>, IReactiveStreamProcess
        {
            public MockStream(Uri uri)
                : base(uri)
            {
            }

            public IReactiveQubject<TInput, TOutput> ToStream<TInput, TOutput>()
            {
                throw new NotImplementedException();
            }
        }

        private sealed class MockSubscription : MockProcessResource<IReactiveQubscription>, IReactiveSubscriptionProcess
        {
            public MockSubscription(Uri uri)
                : base(uri)
            {
            }

            public IReactiveQubscription ToSubscription()
            {
                throw new NotImplementedException();
            }
        }

        #endregion
    }
}
