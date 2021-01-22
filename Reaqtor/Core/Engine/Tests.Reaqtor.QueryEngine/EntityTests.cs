// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq.CompilerServices;
using System.Linq.Expressions;

using Reaqtor.QueryEngine;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Reaqtor.QueryEngine
{
    [TestClass]
    public class EntityTests
    {
        private static readonly Uri _entityUri = new("bar://foo");
        private static readonly Expression _expression = Expression.Lambda(Expression.Constant(new object(), typeof(object)));
        private static readonly object _state = new();

        [TestMethod]
        public void Entities_Properties()
        {
            Entities_Properties_Assert((id, e, s) => new ObservableDefinitionEntity(id, e, s), ReactiveEntityKind.Observable, isInitialized: true);
            Entities_Properties_Assert((id, e, s) => new ObserverDefinitionEntity(id, e, s), ReactiveEntityKind.Observer, isInitialized: true);
            Entities_Properties_Assert((id, e, s) => new StreamFactoryDefinitionEntity(id, e, s), ReactiveEntityKind.StreamFactory, isInitialized: true);
            Entities_Properties_Assert((id, e, s) => new OtherDefinitionEntity(id, e, s), ReactiveEntityKind.Other, isInitialized: true);

            Entities_Properties_Assert((id, e, s) => new SubscriptionEntity(id, e, s), ReactiveEntityKind.Subscription, isInitialized: false);
            Entities_Properties_Assert((id, e, s) => new ReliableSubscriptionEntity(id, e, s), ReactiveEntityKind.ReliableSubscription, isInitialized: false);
            Entities_Properties_Assert((id, e, s) => new SubjectEntity(id, e, s), ReactiveEntityKind.Stream, isInitialized: false);
        }

        [TestMethod]
        public void Entities_Invalid()
        {
            Entities_Invalid_Assert((id) => ObservableDefinitionEntity.CreateInvalidInstance(id), ReactiveEntityKind.Observable, isInitialized: false);
            Entities_Invalid_Assert((id) => ObserverDefinitionEntity.CreateInvalidInstance(id), ReactiveEntityKind.Observer, isInitialized: false);
            Entities_Invalid_Assert((id) => StreamFactoryDefinitionEntity.CreateInvalidInstance(id), ReactiveEntityKind.StreamFactory, isInitialized: false);
            Entities_Invalid_Assert((id) => OtherDefinitionEntity.CreateInvalidInstance(id), ReactiveEntityKind.Other, isInitialized: false);

            Entities_Invalid_Assert((id) => SubscriptionEntity.CreateInvalidInstance(id), ReactiveEntityKind.Subscription, isInitialized: false);
            Entities_Invalid_Assert((id) => ReliableSubscriptionEntity.CreateInvalidInstance(id), ReactiveEntityKind.ReliableSubscription, isInitialized: false);
            Entities_Invalid_Assert((id) => SubjectEntity.CreateInvalidInstance(id), ReactiveEntityKind.Stream, isInitialized: false);
        }

        private static void Entities_Properties_Assert(Func<Uri, Expression, object, ReactiveEntity> create, ReactiveEntityKind kind, bool isInitialized)
        {
            var res = create(_entityUri, _expression, _state);
            Assert.AreSame(_entityUri, res.Uri);
            Assert.AreSame(_expression, res.Expression);
            Assert.AreSame(_state, res.State);
            Assert.AreEqual(kind, res.Kind);
            Assert.AreEqual(isInitialized, res.IsInitialized);

            var cache = new ExpressionHeap();
            res.Cache(cache);
            Assert.AreNotSame(_expression, res.Expression);
            Assert.IsTrue(new ExpressionEqualityComparer().Equals(_expression, res.Expression));
        }

        private static void Entities_Invalid_Assert(Func<Uri, ReactiveEntity> create, ReactiveEntityKind kind, bool isInitialized)
        {
            var res = create(_entityUri);
            Assert.AreSame(_entityUri, res.Uri);
            Assert.IsNotNull(res.Expression);
            Assert.IsNull(res.State);
            Assert.AreEqual(kind, res.Kind);
            Assert.AreEqual(isInitialized, res.IsInitialized);
        }
    }
}
