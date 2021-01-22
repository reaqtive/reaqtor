// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - December 2013 - Created this file.
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.CompilerServices;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

using Reaqtive.Testing;

using Reaqtor.Hosting.Shared.Tools;
using Reaqtor.Remoting.Protocol;

namespace Reaqtor.Remoting.TestingFramework
{
    public class TestableQbservable<T> : ITestableQbservable<T>
    {
        private readonly IAsyncReactiveQbservable<T> _inner;
        private readonly ITestReactivePlatformClient _testClient;
        private readonly Uri _observableId;

        public TestableQbservable(Uri observableId, IList<Recorded<INotification<T>>> messages, IAsyncReactiveQbservable<T> inner, ITestReactivePlatformClient testClient)
        {
            _observableId = observableId;
            ObserverMessages = messages;
            _inner = inner;
            _testClient = testClient;
        }

        public Task<IAsyncReactiveQubscription> SubscribeAsync(IAsyncReactiveQbserver<T> observer, Uri subscriptionUri, object state, CancellationToken token)
        {
            _testClient.CleanupEntity(subscriptionUri, ReactiveEntityType.Subscription);
            return _inner.SubscribeAsync(observer, subscriptionUri, state, token);
        }

        public Task<IAsyncReactiveSubscription> SubscribeAsync(IAsyncReactiveObserver<T> observer, Uri subscriptionUri, object state, CancellationToken token)
        {
            _testClient.CleanupEntity(subscriptionUri, ReactiveEntityType.Subscription);
            return _inner.SubscribeAsync(observer, subscriptionUri, state, token);
        }

        public Type ElementType => _inner.ElementType;

        public IAsyncReactiveQueryProvider Provider => _inner.Provider;

        public Expression Expression
        {
            get
            {
                var rewriter = new TypeSubstitutionExpressionVisitor(new Dictionary<Type, Type>
                {
                    { typeof(ITestableQbservable<>), typeof(IAsyncReactiveQbservable<>) }
                });

                return rewriter.Visit(_inner.Expression);
            }
        }

        public IList<Subscription> Subscriptions
        {
            get
            {
                var testQE = _testClient.Platform.QueryEvaluators.First().GetInstance<TestQueryEvaluatorServiceConnection>();
                testQE.TestSubscriptions.TryGetValue(_observableId.ToCanonicalString(), out var subscriptions);
                return subscriptions ?? new List<Subscription>();
            }
        }

        public IList<Recorded<INotification<T>>> ObserverMessages { get; }
    }
}
