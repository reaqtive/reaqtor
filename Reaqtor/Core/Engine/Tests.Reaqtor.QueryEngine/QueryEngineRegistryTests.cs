// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq.Expressions;

using Reaqtive;

using Reaqtor.QueryEngine;
using Reaqtor.QueryEngine.Mocks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Reaqtor.QueryEngine
{
    [TestClass]
    public class QueryEngineRegistryTests
    {
        private static readonly string[] Keys = new[]
        {
            "test://key0",
            "test://key1",
            "test://key2",
            "test://key3",
            "test://key4",
            "test://key5",
            "test://key6",
            "test://key7",
        };

        private static readonly Expression[] Expressions = new Expression[]
        {
            Expression.New(typeof(DummySubject)),
            Expression.Default(typeof(ISubscribable<int>)),
            Expression.Default(typeof(IObserver<int>)),
            Expression.Lambda(Expression.New(typeof(DummySubject))),
            Expression.New(typeof(DummySubscription)),
            Expression.Default(typeof(DummySubscription)),
            Expression.New(typeof(DummyTypedSubject)),
            Expression.Default(typeof(DummySubject)),
        };

        [TestMethod]
        public void QueryEngineRegistry_ExecutionEnvironment_GetSubscription()
        {
            var registry = CreateRegistry();

            // Entity does not exist
            Assert.ThrowsException<ArgumentException>(() => registry.GetSubscription(new Uri(Keys[0])));

            // Entity is not initialized
            Assert.ThrowsException<ArgumentException>(() => registry.GetSubscription(new Uri(Keys[5])));

            // Entity exists
            var sub = registry.GetSubscription(new Uri(Keys[4]));
            Assert.IsTrue(sub is DummySubscription);
        }

        [TestMethod]
        public void QueryEngineRegistry_ExecutionEnvironment_GetSubject()
        {
            var registry = CreateRegistry();

            // Entity does not exist
            Assert.ThrowsException<ArgumentException>(() => registry.GetSubject<int, int>(new Uri(Keys[1])));

            // Entity is not initialized
            Assert.ThrowsException<ArgumentException>(() => registry.GetSubject<int, int>(new Uri(Keys[7])));

            // Entity exists, casted to IMultiSubject<,>
            var sub = registry.GetSubject<int, int>(new Uri(Keys[0]));
            Assert.IsFalse(sub is DummySubject);

            // Entity exists, not casted
            sub = registry.GetSubject<int, int>(new Uri(Keys[6]));
            Assert.IsTrue(sub is DummyTypedSubject);
        }

        private static QueryEngineRegistry CreateRegistry()
        {
            var parent = new MockQueryEngineRegistry();
            var provider = new MockReactiveEngineProvider(parent);

            provider.CreateStream(new Uri(Keys[0]), Expressions[0], null);
            provider.DefineObservable(new Uri(Keys[1]), Expressions[1], null);
            provider.DefineObserver(new Uri(Keys[2]), Expressions[2], null);
            provider.DefineStreamFactory(new Uri(Keys[3]), Expressions[3], null);
            provider.CreateSubscription(new Uri(Keys[4]), Expressions[4], null);
            provider.CreateSubscription(new Uri(Keys[5]), Expressions[5], null);
            provider.CreateStream(new Uri(Keys[6]), Expressions[6], null);
            provider.CreateStream(new Uri(Keys[7]), Expressions[7], null);

            return new QueryEngineRegistry(parent);
        }

        private class DummySubject : IMultiSubject
        {
            public IObserver<T> GetObserver<T>()
            {
                throw new NotImplementedException();
            }

            public ISubscribable<T> GetObservable<T>()
            {
                throw new NotImplementedException();
            }

            public void Dispose()
            {
                throw new NotImplementedException();
            }
        }

        private sealed class DummyTypedSubject : IMultiSubject<int>
        {
            public IObserver<int> CreateObserver()
            {
                throw new NotImplementedException();
            }

            public ISubscription Subscribe(IObserver<int> observer)
            {
                throw new NotImplementedException();
            }

            IDisposable IObservable<int>.Subscribe(IObserver<int> observer)
            {
                throw new NotImplementedException();
            }

            public void Dispose()
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
