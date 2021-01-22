// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.CompilerServices;
using System.Linq.Expressions;

using Reaqtive;

using Reaqtor;
using Reaqtor.Metadata;
using Reaqtor.QueryEngine.Mocks;
using Reaqtor.Reliable;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Reaqtor.QueryEngine
{
    [TestClass]
    public class RegistryQueryProviderTests
    {
        private static readonly string[] Keys = new[]
        {
            "test://key1",
            "test://key2",
            "test://key3",
            "test://key4",
            "test://key5",
        };

        private static readonly Expression[] Expressions = new Expression[]
        {
            Expression.New(typeof(DummySubject)),
            Expression.Default(typeof(ISubscribable<int>)),
            Expression.Default(typeof(IObserver<int>)),
            Expression.Default(typeof(Func<IReliableMultiSubject<int>>)),
            Expression.New(typeof(DummySubscription)),
        };

        [TestMethod]
        public void RegistryQueryProvider_NotImplemented()
        {
            var provider = CreateEngineProvider();
            var tryGetValue = CreateQuery("foo", Keys[0], (IQueryable<KeyValuePair<Uri, IReactiveResource>> q, Uri u) => q.SingleOrDefault(r => r.Key == u));
            var keys = CreateQuery("foo", (IQueryable<KeyValuePair<Uri, IReactiveResource>> q) => q.Select(r => r.Key));
            var values = CreateQuery("foo", (IQueryable<KeyValuePair<Uri, IReactiveResource>> q) => q.Select(r => r.Value));
            var enums = CreateQuery("foo", (IQueryable<KeyValuePair<Uri, IReactiveResource>> q) => q);

            Assert.ThrowsException<NotImplementedException>(() => provider.Provider.Execute<IReactiveResource>(tryGetValue));
            Assert.ThrowsException<NotImplementedException>(() => provider.Provider.Execute<IReactiveResource>(keys));
            Assert.ThrowsException<NotImplementedException>(() => provider.Provider.Execute<IReactiveResource>(values));
            Assert.ThrowsException<NotImplementedException>(() => provider.Provider.Execute<IReactiveResource>(enums));
            Assert.ThrowsException<NotImplementedException>(() => provider.Provider.CreateQuery(null));
            Assert.ThrowsException<NotImplementedException>(() => provider.Provider.CreateQuery<object>(null));
        }

        [TestMethod]
        public void RegistryQueryProvider_NotSupported()
        {
            var provider = CreateEngineProvider();

            var multipleMemberAccess = CreateQuery("foo", Keys[0], (IQueryable<KeyValuePair<Uri, IReactiveResource>> q, Uri u) => q.Select(r => r.Value.Expression));
            var nonMemberAccess = CreateQuery("foo", Keys[0], (IQueryable<KeyValuePair<Uri, IReactiveResource>> q, Uri u) => q.Select(r => r.Value == null));
            var notByKey = CreateQuery("foo", Keys[0], (IQueryable<KeyValuePair<Uri, IReactiveResource>> q, Uri u) => q.SingleOrDefault(r => r.Value == null));

            Assert.ThrowsException<NotSupportedException>(() => provider.Provider.Execute<IReactiveResource>(multipleMemberAccess));
            Assert.ThrowsException<NotSupportedException>(() => provider.Provider.Execute<IReactiveResource>(nonMemberAccess));
            Assert.ThrowsException<NotSupportedException>(() => provider.Provider.Execute<IReactiveResource>(notByKey));
        }

        [TestMethod]
        public void RegistryQueryProvider_NonGenericExecute()
        {
            var provider = CreateEngineProvider();
            var metadata = CreateMetadata(provider);
            var streamsTable = ((ParameterExpression)metadata.Streams.Expression).Name;

            var lookup = CreateQuery(streamsTable, Keys[0], (IQueryable<KeyValuePair<Uri, IReactiveResource>> q, Uri u) => q.SingleOrDefault(r => r.Key == u));
            var nullResult = provider.Provider.Execute(lookup);
            Assert.IsNull(nullResult);
        }

        [TestMethod]
        public void RegistryQueryProvider_Keys()
        {
            var provider = CreateEngineProvider();
            var metadata = CreateMetadata(provider);

            provider.CreateStream(new Uri(Keys[0]), Expressions[0], null);
            provider.DefineObservable(new Uri(Keys[1]), Expressions[1], null);
            provider.DefineObserver(new Uri(Keys[2]), Expressions[2], null);
            provider.DefineStreamFactory(new Uri(Keys[3]), Expressions[3], null);
            provider.CreateSubscription(new Uri(Keys[4]), Expressions[4], null);

            Assert.IsTrue(metadata.Streams.Keys.ToList().SequenceEqual(new[] { new Uri(Keys[0]) }));
            Assert.IsTrue(metadata.Observables.Keys.ToList().SequenceEqual(new[] { new Uri(Keys[1]) }));
            Assert.IsTrue(metadata.Observers.Keys.ToList().SequenceEqual(new[] { new Uri(Keys[2]) }));
            Assert.IsTrue(metadata.StreamFactories.Keys.ToList().SequenceEqual(new[] { new Uri(Keys[3]) }));
            Assert.IsTrue(metadata.Subscriptions.Keys.ToList().SequenceEqual(new[] { new Uri(Keys[4]) }));
        }

        [TestMethod]
        public void RegistryQueryProvider_Values()
        {
            var provider = CreateEngineProvider();
            var metadata = CreateMetadata(provider);

            provider.CreateStream(new Uri(Keys[0]), Expressions[0], null);
            provider.DefineObservable(new Uri(Keys[1]), Expressions[1], null);
            provider.DefineObserver(new Uri(Keys[2]), Expressions[2], null);
            provider.DefineStreamFactory(new Uri(Keys[3]), Expressions[3], null);
            provider.CreateSubscription(new Uri(Keys[4]), Expressions[4], null);

            Assert.IsTrue(metadata.Streams.Values.ToList().Select(s => s.Expression).SequenceEqual(new[] { Expressions[0] }));
            Assert.IsTrue(metadata.Observables.Values.ToList().Select(s => s.Expression).SequenceEqual(new[] { Expressions[1] }));
            Assert.IsTrue(metadata.Observers.Values.ToList().Select(s => s.Expression).SequenceEqual(new[] { Expressions[2] }));
            Assert.IsTrue(metadata.StreamFactories.Values.ToList().Select(s => s.Expression).SequenceEqual(new[] { Expressions[3] }));
            Assert.IsTrue(metadata.Subscriptions.Values.ToList().Select(s => s.Expression).SequenceEqual(new[] { Expressions[4] }));
        }

        [TestMethod]
        public void RegistryQueryProvider_Enumerations()
        {
            var provider = CreateEngineProvider();
            var metadata = CreateMetadata(provider);

            provider.CreateStream(new Uri(Keys[0]), Expressions[0], null);
            provider.DefineObservable(new Uri(Keys[1]), Expressions[1], null);
            provider.DefineObserver(new Uri(Keys[2]), Expressions[2], null);
            provider.DefineStreamFactory(new Uri(Keys[3]), Expressions[3], null);
            provider.CreateSubscription(new Uri(Keys[4]), Expressions[4], null);

            Assert.IsTrue(metadata.Streams.ToList().Select(s => s.Value.Expression).SequenceEqual(new[] { Expressions[0] }));
            Assert.IsTrue(metadata.Observables.ToList().Select(s => s.Value.Expression).SequenceEqual(new[] { Expressions[1] }));
            Assert.IsTrue(metadata.Observers.ToList().Select(s => s.Value.Expression).SequenceEqual(new[] { Expressions[2] }));
            Assert.IsTrue(metadata.StreamFactories.ToList().Select(s => s.Value.Expression).SequenceEqual(new[] { Expressions[3] }));
            Assert.IsTrue(metadata.Subscriptions.ToList().Select(s => s.Value.Expression).SequenceEqual(new[] { Expressions[4] }));
        }

        [TestMethod]
        public void RegistryQueryProvider_ContainsKey()
        {
            var provider = CreateEngineProvider();
            var metadata = CreateMetadata(provider);

            provider.CreateStream(new Uri(Keys[0]), Expressions[0], null);
            provider.DefineObservable(new Uri(Keys[1]), Expressions[1], null);
            provider.DefineObserver(new Uri(Keys[2]), Expressions[2], null);
            provider.DefineStreamFactory(new Uri(Keys[3]), Expressions[3], null);
            provider.CreateSubscription(new Uri(Keys[4]), Expressions[4], null);

            Assert.IsTrue(metadata.Streams.ContainsKey(new Uri(Keys[0])));
            Assert.IsTrue(metadata.Observables.ContainsKey(new Uri(Keys[1])));
            Assert.IsTrue(metadata.Observers.ContainsKey(new Uri(Keys[2])));
            Assert.IsTrue(metadata.StreamFactories.ContainsKey(new Uri(Keys[3])));
            Assert.IsTrue(metadata.Subscriptions.ContainsKey(new Uri(Keys[4])));

            Assert.IsFalse(metadata.Streams.ContainsKey(new Uri(Keys[1])));
        }

        [TestMethod]
        public void RegistryQueryProvider_TryGetValue()
        {
            var provider = CreateEngineProvider();
            var metadata = CreateMetadata(provider);

            provider.CreateStream(new Uri(Keys[0]), Expressions[0], null);
            provider.DefineObservable(new Uri(Keys[1]), Expressions[1], null);
            provider.DefineObserver(new Uri(Keys[2]), Expressions[2], null);
            provider.DefineStreamFactory(new Uri(Keys[3]), Expressions[3], null);
            provider.CreateSubscription(new Uri(Keys[4]), Expressions[4], null);

            Assert.IsTrue(metadata.Streams.TryGetValue(new Uri(Keys[0]), out var stream));
            Assert.IsTrue(metadata.Observables.TryGetValue(new Uri(Keys[1]), out var observable));
            Assert.IsTrue(metadata.Observers.TryGetValue(new Uri(Keys[2]), out var observer));
            Assert.IsTrue(metadata.StreamFactories.TryGetValue(new Uri(Keys[3]), out var streamFactory));
            Assert.IsTrue(metadata.Subscriptions.TryGetValue(new Uri(Keys[4]), out var subscription));

            Assert.AreSame(metadata.Streams[new Uri(Keys[0])], stream);
            Assert.AreSame(metadata.Observables[new Uri(Keys[1])], observable);
            Assert.AreSame(metadata.Observers[new Uri(Keys[2])], observer);
            Assert.AreSame(metadata.StreamFactories[new Uri(Keys[3])], streamFactory);
            Assert.AreSame(metadata.Subscriptions[new Uri(Keys[4])], subscription);

            Assert.IsFalse(metadata.Streams.TryGetValue(new Uri(Keys[1]), out _));
        }

        private static IReactiveEngineProvider CreateEngineProvider()
        {
            var registry = new MockQueryEngineRegistry();
            return new MockReactiveEngineProvider(registry);
        }

        private static ReactiveMetadata CreateMetadata(IReactiveEngineProvider provider)
        {
            return new ReactiveMetadata(provider, new ReactiveExpressionServices(typeof(IReactive)));
        }

        private static Expression CreateQuery<T, R>(string collection, string key, Expression<Func<IQueryable<KeyValuePair<Uri, T>>, Uri, R>> expression)
        {
            return BetaReducer.Reduce(Expression.Invoke(expression, Expression.Parameter(typeof(IQueryable<KeyValuePair<Uri, T>>), collection), Expression.Constant(new Uri(key))));
        }

        private static Expression CreateQuery<T, R>(string collection, Expression<Func<IQueryable<KeyValuePair<Uri, T>>, R>> expression)
        {
            return BetaReducer.Reduce(Expression.Invoke(expression, Expression.Parameter(typeof(IQueryable<KeyValuePair<Uri, T>>), collection)));
        }

        private sealed class DummySubject : IReliableMultiSubject<int, int>
        {
            public IReliableObserver<int> CreateObserver()
            {
                throw new NotImplementedException();
            }

            public void Dispose()
            {
                throw new NotImplementedException();
            }

            public IReliableSubscription Subscribe(IReliableObserver<int> observer)
            {
                throw new NotImplementedException();
            }
        }

        private sealed class DummySubscription : ISubscription
        {
            public void Accept(ISubscriptionVisitor visitor)
            {
                throw new NotImplementedException();
            }

            public void Dispose()
            {
                throw new NotImplementedException();
            }
        }
    }
}
