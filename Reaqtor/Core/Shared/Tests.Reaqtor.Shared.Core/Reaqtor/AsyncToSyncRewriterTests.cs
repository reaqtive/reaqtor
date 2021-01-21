// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - October 2013 - Created this file.
//

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq.CompilerServices;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using Reaqtor;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Reaqtor
{
    [TestClass]
    public class AsyncToSyncRewriterTests
    {
        private static readonly Type AsyncObservableType = typeof(IAsyncReactiveQbservable<int>);
        private static readonly Type SyncObservableType = typeof(IReactiveQbservable<int>);
        private static readonly Type AsyncObserverType = typeof(IAsyncReactiveQbserver<int>);
        private static readonly Type SyncObserverType = typeof(IReactiveQbserver<int>);
        private static readonly Type AsyncSubscriptionType = typeof(Task<IAsyncReactiveQubscription>);
        private static readonly Type SyncSubscriptionType = typeof(IReactiveQubscription);
        private static readonly Type SubscribeAsyncTupleType = typeof(Tuple<,>).MakeGenericType(AsyncObservableType, AsyncObserverType);
        private static readonly Type SubscribeSyncTupleType = typeof(Tuple<,>).MakeGenericType(SyncObservableType, SyncObserverType);
        private static readonly Type SubscribeAsyncDelegateType = typeof(Func<,>).MakeGenericType(SubscribeAsyncTupleType, AsyncSubscriptionType);
        private static readonly Type SubscribeSyncDelegateType = typeof(Func<,>).MakeGenericType(SubscribeSyncTupleType, SyncSubscriptionType);

        [TestMethod]
        public void AsyncToSyncRewriter_InvalidSubscribeSignatureFails()
        {
            var rewriter = new AsyncToSyncRewriter(new Dictionary<Type, Type>());

            Assert.ThrowsException<InvalidOperationException>(
                () => rewriter.Rewrite(
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<,,>).MakeGenericType(AsyncObservableType, AsyncObserverType, AsyncSubscriptionType), Constants.SubscribeUri),
                        Expression.Parameter(AsyncObservableType, "bing://xs"),
                        Expression.Parameter(AsyncObserverType, "bing://observer")
                    )
                )
            );

            Assert.ThrowsException<InvalidOperationException>(
                () => rewriter.Rewrite(
                    Expression.Parameter(typeof(int), Constants.SubscribeUri)
                )
            );
        }

        [TestMethod]
        public void AsyncToSyncRewriter_SimpleSubscribe()
        {
            AssertRewrite(
                Expression.Invoke(
                    Expression.Parameter(SubscribeAsyncDelegateType, Constants.SubscribeUri),
                    Expression.New(
                        SubscribeAsyncTupleType.GetConstructor(new[] { AsyncObservableType, AsyncObserverType }),
                        Expression.Parameter(AsyncObservableType, "bing://xs"),
                        Expression.Parameter(AsyncObserverType, "bing://observer")
                    )
                ),
                Expression.Invoke(
                    Expression.Parameter(SubscribeSyncDelegateType, Constants.SubscribeUri),
                    Expression.New(
                        SubscribeSyncTupleType.GetConstructor(new[] { SyncObservableType, SyncObserverType }),
                        Expression.Parameter(SyncObservableType, "bing://xs"),
                        Expression.Parameter(SyncObserverType, "bing://observer")
                    )
                )
            );
        }

        [TestMethod]
        public void AsyncToSyncRewriter_ObserverMethodCallRewrites()
        {
            AssertRewrite(
                Expression.Invoke(
                    Expression.Parameter(SubscribeAsyncDelegateType, "rx://observable/subscribe"),
                    Expression.New(
                        SubscribeAsyncTupleType.GetConstructor(new[] { AsyncObservableType, AsyncObserverType }),
                        Expression.Invoke(
                            Expression.Parameter(typeof(Func<,,>).MakeGenericType(AsyncObservableType, typeof(Expression<Action<int>>), AsyncObservableType), "rx://do"),
                            Expression.Parameter(AsyncObservableType, "bing://xs"),
                            Expression.Parameter(typeof(int), "x").Let(x =>
                                Expression.Lambda<Action<int>>(
                                    Expression.Call(
                                        Expression.Parameter(AsyncObserverType, "bing://passiveObserver"),
                                        (MethodInfo)ReflectionHelpers.InfoOf((IAsyncReactiveQbserver<int> observer) => observer.OnNextAsync(0, CancellationToken.None)),
                                        x,
                                        Expression.Constant(CancellationToken.None)
                                    ),
                                    x
                                )
                            )
                        ),
                        Expression.Parameter(AsyncObserverType, "bing://observer")
                    )
                ),
                Expression.Invoke(
                    Expression.Parameter(SubscribeSyncDelegateType, "rx://observable/subscribe"),
                    Expression.New(
                        SubscribeSyncTupleType.GetConstructor(new[] { SyncObservableType, SyncObserverType }),
                        Expression.Invoke(
                            Expression.Parameter(typeof(Func<,,>).MakeGenericType(SyncObservableType, typeof(Expression<Action<int>>), SyncObservableType), "rx://do"),
                            Expression.Parameter(SyncObservableType, "bing://xs"),
                            Expression.Parameter(typeof(int), "x").Let(x =>
                                Expression.Lambda<Action<int>>(
                                    Expression.Call(
                                        Expression.Parameter(SyncObserverType, "bing://passiveObserver"),
                                        (MethodInfo)ReflectionHelpers.InfoOf((IReactiveQbserver<int> observer) => observer.OnNext(0)),
                                        x
                                    ),
                                    x
                                )
                            )
                        ),
                        Expression.Parameter(SyncObserverType, "bing://observer")
                    )
                )
            );
        }

        [TestMethod]
        public void AsyncToSyncRewriter_ObservableMethodCallRewrites()
        {
            AssertRewrite(
                Expression.Invoke(
                    Expression.Parameter(SubscribeAsyncDelegateType, "rx://observable/subscribe"),
                    Expression.New(
                        SubscribeAsyncTupleType.GetConstructor(new[] { AsyncObservableType, AsyncObserverType }),
                        Expression.Invoke(
                            Expression.Parameter(typeof(Func<,,>).MakeGenericType(AsyncObservableType, typeof(Expression<Action<int>>), AsyncObservableType), "rx://do"),
                            Expression.Parameter(AsyncObservableType, "bing://xs"),
                            Expression.Parameter(typeof(int), "x").Let(x =>
                                Expression.Lambda<Action<int>>(
                                    Expression.Call(
                                        Expression.Parameter(AsyncObservableType, "bing://dummyObservable"),
                                        (MethodInfo)ReflectionHelpers.InfoOf((IAsyncReactiveQbservable<int> observable) => observable.SubscribeAsync(null, null, null, CancellationToken.None)),
                                        Expression.Parameter(AsyncObserverType, "bing://dummyObserver"),
                                        Expression.Constant(new Uri("bing://dummySubscription"), typeof(Uri)),
                                        Expression.Constant(null, typeof(object)),
                                        Expression.Constant(CancellationToken.None)
                                    ),
                                    x
                                )
                            )
                        ),
                        Expression.Parameter(AsyncObserverType, "bing://observer")
                    )
                ),
                Expression.Invoke(
                    Expression.Parameter(SubscribeSyncDelegateType, "rx://observable/subscribe"),
                    Expression.New(
                        SubscribeSyncTupleType.GetConstructor(new[] { SyncObservableType, SyncObserverType }),
                        Expression.Invoke(
                            Expression.Parameter(typeof(Func<,,>).MakeGenericType(SyncObservableType, typeof(Expression<Action<int>>), SyncObservableType), "rx://do"),
                            Expression.Parameter(SyncObservableType, "bing://xs"),
                            Expression.Parameter(typeof(int), "x").Let(x =>
                                Expression.Lambda<Action<int>>(
                                    Expression.Call(
                                        Expression.Parameter(SyncObservableType, "bing://dummyObservable"),
                                        (MethodInfo)ReflectionHelpers.InfoOf((IReactiveQbservable<int> observable) => observable.Subscribe(null, null, null)),
                                        Expression.Parameter(SyncObserverType, "bing://dummyObserver"),
                                        Expression.Constant(new Uri("bing://dummySubscription"), typeof(Uri)),
                                        Expression.Constant(null, typeof(object))
                                    ),
                                    x
                                )
                            )
                        ),
                        Expression.Parameter(SyncObserverType, "bing://observer")
                    )
                )
            );
        }

        private static void AssertRewrite(Expression before, Expression after)
        {
            var rewriter = new AsyncToSyncRewriter(new Dictionary<Type, Type>());
            var comparer = new ExpressionEqualityComparer(() => new Comparator());
            var rewritten = rewriter.Rewrite(before);
            Assert.IsTrue(comparer.Equals(rewritten, after), string.Format(CultureInfo.InvariantCulture, "Expected: {0}, Actual: {1}", after, rewritten));
        }

        private sealed class Comparator : ExpressionEqualityComparator
        {
            protected override bool EqualsGlobalParameter(ParameterExpression x, ParameterExpression y)
            {
                return x.Name == y.Name && Equals(x.Type, y.Type);
            }
        }
    }
}
