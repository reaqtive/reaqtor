// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq.Expressions;

using Reaqtive;

using Reaqtor.Reliable;
using Reaqtor.Reliable.Client;
using Reaqtor.Reliable.Engine;
using Reaqtor.Reliable.Expressions;
using Reaqtor.Reliable.Service;

namespace Reaqtor.QueryEngine
{
    public partial class CheckpointingQueryEngine
    {
        /// <summary>
        /// Implementation of <see cref="IReliableReactive"/> exposed on the engine, to access IReliable* artifacts.
        /// </summary>
        private sealed class ReliableServiceContext : ReliableReactiveServiceContext
        {
            public ReliableServiceContext(CoreReactiveEngine engine)
                : base(engine.ExpressionService, new ReactiveEngine(engine))
            {
            }

            private sealed class ReactiveEngine : IReliableReactiveEngineProvider
            {
#pragma warning disable format // Formatted as tables.

                private static readonly Dictionary<Type, Type> s_subst = new()
                {
                    { typeof(IReliableMultiQubject<,>), typeof(IReliableMultiSubject<,>) },
                    { typeof(IReliableQbservable<>),    typeof(ISubscribable<>)          },
                    { typeof(IReliableQbserver<>),      typeof(IObserver<>)              },
                    { typeof(IReliableQubscription),    typeof(ISubscription)            },
                };

#pragma warning restore format

                private readonly CoreReactiveEngine _engine;

                public ReactiveEngine(CoreReactiveEngine engine)
                {
                    _engine = engine;
                }

                public IReliableReactiveObserver<T> GetObserver<T>(Uri observerUri)
                {
                    return new ReliableReactiveObserverToReliableObserver<T>(_engine.GetObserver<T>(observerUri));
                }

                public void CreateSubscription(Uri subscriptionUri, Expression subscription, object state)
                {
                    Expression expr = RewriteQuotedReliableToSubscribable(subscription);
                    _engine.CreateSubscription(subscriptionUri, expr, state);
                }

                public void DeleteSubscription(Uri subscriptionUri)
                {
                    _engine.DeleteReliableSubscription(subscriptionUri);
                }

                public void StartSubscription(Uri subscriptionUri, long sequenceId)
                {
                    // NB: Causes Start on the entity, which maintains the top-level lifecycle of the artifact.
                    _engine.StartSubscription(subscriptionUri, sequenceId);
                }

                public void AcknowledgeRange(Uri subscriptionUri, long sequenceId)
                {
                    _engine.AcknowledgeRange(subscriptionUri, sequenceId);
                }

                public Uri GetSubscriptionResubscribeUri(Uri subscriptionUri)
                {
                    return _engine.GetSubscriptionResubscribeUri(subscriptionUri);
                }

                public void CreateStream(Uri streamUri, Expression stream, object state)
                {
                    throw new NotImplementedException();
                }

                public void DeleteStream(Uri streamUri)
                {
                    throw new NotImplementedException();
                }

                public void CreateObserver(Uri streamUri)
                {
                    throw new NotImplementedException();
                }

                private static Expression RewriteQuotedReliableToSubscribable(Expression expression)
                {
                    var rewrite = new SubstituteAndUnquoteRewriter(s_subst);

                    Expression result = rewrite.Apply(expression);
                    Expression detupletized = ExpressionHelpers.Detupletize(result);
                    return detupletized;
                }

                private sealed class ReliableReactiveObserverToReliableObserver<T> : IReliableReactiveObserver<T>
                {
                    private readonly IReliableObserver<T> _reliableObserver;

                    public ReliableReactiveObserverToReliableObserver(IReliableObserver<T> reliableObserver)
                    {
                        _reliableObserver = reliableObserver;
                    }

                    public Uri ResubscribeUri => _reliableObserver.ResubscribeUri;

                    public void OnNext(T item, long sequenceId) => _reliableObserver.OnNext(item, sequenceId);

                    public void OnStarted() => _reliableObserver.OnStarted();

                    public void OnError(Exception error) => _reliableObserver.OnError(error);

                    public void OnCompleted() => _reliableObserver.OnCompleted();
                }
            }
        }
    }
}
