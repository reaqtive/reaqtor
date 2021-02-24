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

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Reaqtor;

namespace Tests.Reaqtor
{
    [TestClass]
    public class UriToReactiveProxyBinderTests
    {
        private static readonly MethodInfo _getStreamFactory = ((MethodInfo)ReflectionHelpers.InfoOf((IReactiveProxy r) => r.GetStreamFactory<object, object>(null))).GetGenericMethodDefinition();
        private static readonly MethodInfo _getStreamFactoryParam = ((MethodInfo)ReflectionHelpers.InfoOf((IReactiveProxy r) => r.GetStreamFactory<object, object, object>(null))).GetGenericMethodDefinition();

        private static readonly MethodInfo _getSubscriptionFactory = (MethodInfo)ReflectionHelpers.InfoOf((IReactiveProxy r) => r.GetSubscriptionFactory(null));
        private static readonly MethodInfo _getSubscriptionFactoryParam = ((MethodInfo)ReflectionHelpers.InfoOf((IReactiveProxy r) => r.GetSubscriptionFactory<object>(null))).GetGenericMethodDefinition();

        private static readonly MethodInfo _getStream = ((MethodInfo)ReflectionHelpers.InfoOf((IReactiveProxy r) => r.GetStream<object, object>(null))).GetGenericMethodDefinition();

        private static readonly MethodInfo _getObservable = ((MethodInfo)ReflectionHelpers.InfoOf((IReactiveProxy r) => r.GetObservable<object>(null))).GetGenericMethodDefinition();
        private static readonly MethodInfo _getObservableParam = ((MethodInfo)ReflectionHelpers.InfoOf((IReactiveProxy r) => r.GetObservable<object, object>(null))).GetGenericMethodDefinition();

        private static readonly MethodInfo _getObserver = ((MethodInfo)ReflectionHelpers.InfoOf((IReactiveProxy r) => r.GetObserver<object>(null))).GetGenericMethodDefinition();
        private static readonly MethodInfo _getObserverParam = ((MethodInfo)ReflectionHelpers.InfoOf((IReactiveProxy r) => r.GetObserver<object, object>(null))).GetGenericMethodDefinition();

        private static readonly MethodInfo _getSubscription = ((MethodInfo)ReflectionHelpers.InfoOf((IReactiveProxy r) => r.GetSubscription(null)));

        private static readonly MethodInfo _defineObservable = ((MethodInfo)ReflectionHelpers.InfoOf((IReactiveProxy r) => r.DefineObservableAsync<object>(null, null, null, CancellationToken.None))).GetGenericMethodDefinition();
        private static readonly MethodInfo _defineObservableParam = ((MethodInfo)ReflectionHelpers.InfoOf((IReactiveProxy r) => r.DefineObservableAsync<object, object>(null, null, null, CancellationToken.None))).GetGenericMethodDefinition();

        private static readonly MethodInfo _defineObserver = ((MethodInfo)ReflectionHelpers.InfoOf((IReactiveProxy r) => r.DefineObserverAsync<object>(null, null, null, CancellationToken.None))).GetGenericMethodDefinition();
        private static readonly MethodInfo _defineObserverParam = ((MethodInfo)ReflectionHelpers.InfoOf((IReactiveProxy r) => r.DefineObserverAsync<object, object>(null, null, null, CancellationToken.None))).GetGenericMethodDefinition();

        private static readonly Type _parameterType = typeof(string);
        private static readonly Type _elementType = typeof(int);
        private static readonly Type _observableType = typeof(IAsyncReactiveQbservable<>).MakeGenericType(_elementType);
        private static readonly Type _observableParamType = typeof(Func<,>).MakeGenericType(_parameterType, _observableType);
        private static readonly Type _observerType = typeof(IAsyncReactiveQbserver<>).MakeGenericType(_elementType);
        private static readonly Type _observerParamType = typeof(Func<,>).MakeGenericType(_parameterType, _observerType);
        private static readonly Type _subjectType = typeof(IAsyncReactiveQubject<,>).MakeGenericType(_elementType, _elementType);
        private static readonly Type _subscriptionType = typeof(IAsyncReactiveQubscription);
        private static readonly Type _subscriptionTaskType = typeof(Task<IAsyncReactiveQubscription>);
        private static readonly Type _subjectFactoryType = typeof(IAsyncReactiveQubjectFactory<,>).MakeGenericType(_elementType, _elementType);
        private static readonly Type _subjectFactoryParamType = typeof(IAsyncReactiveQubjectFactory<,,>).MakeGenericType(_elementType, _elementType, _parameterType);
        private static readonly Type _subscriptionFactoryType = typeof(IAsyncReactiveQubscriptionFactory);
        private static readonly Type _subscriptionFactoryParamType = typeof(IAsyncReactiveQubscriptionFactory<>).MakeGenericType(_parameterType);
        private static readonly Type _subscribeArgumentType = typeof(Tuple<,>).MakeGenericType(_observableType, _observerType);
        private static readonly Type _subscribeType = typeof(Func<,>).MakeGenericType(_subscribeArgumentType, _subscriptionTaskType);
        private static readonly Type _createStreamArgumentType = typeof(Func<>).MakeGenericType(_subjectType);
        private static readonly Type _createStreamType = typeof(Func<,>).MakeGenericType(_parameterType, _subjectType);
        private static readonly Type _createSubscriptionArgumentType = typeof(Func<>).MakeGenericType(_subscriptionType);
        private static readonly Type _createSubscriptionType = typeof(Func<,>).MakeGenericType(_parameterType, _subscriptionType);
        private static readonly Type _otherType = typeof(Func<object>);
        private static readonly Type _observableSubjectParamType = typeof(Func<,>).MakeGenericType(_subjectType, _observableType);
        private static readonly Type _observableSubscriptionParamType = typeof(Func<,>).MakeGenericType(_subscriptionType, _observableType);
        private static readonly Type _observableOtherParamType = typeof(Func<,>).MakeGenericType(_otherType, _observableType);

        private static readonly string _fooId = "bing://foo";
        private static readonly string _barId = "bing://bar";
        private static readonly string _thisId = "rx://builtin/this";

        private static readonly Uri _subscriptionUri = new("bing://sub");
        private static readonly Uri _streamUri = new("bing://stream");
        private static readonly Uri _observableUri = new("bing://observable");
        private static readonly Uri _observerUri = new("bing://observer");
        private static readonly Uri _fooUri = new(_fooId);
        private static readonly Uri _barUri = new(_barId);

        private static readonly object _state = null;
        private static readonly CancellationToken _token = CancellationToken.None;

        [TestMethod]
        public void UriToReactiveProxyBinder_BindSubscription_InvalidSignaturesFail()
        {
            var rebinder = new UriToReactiveProxyBinder();

            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                rebinder.BindSubscription(
                    Expression.Parameter(typeof(int)),
                    _subscriptionUri,
                    _state,
                    _token
                );
            });

            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                rebinder.BindSubscription(
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<int, int>)),
                        Expression.Default(typeof(int))
                    ),
                    _subscriptionUri,
                    _state,
                    _token
                );
            });

            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                rebinder.BindSubscription(
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<int, int, int, Task<IAsyncReactiveQubscription>>)),
                        Expression.Default(typeof(int)),
                        Expression.Default(typeof(int)),
                        Expression.Default(typeof(int))
                    ),
                    _subscriptionUri,
                    _state,
                    _token
                );
            });

            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                rebinder.BindSubscription(
                    Expression.Invoke(
                        Expression.Lambda<Func<int, int, Task<IAsyncReactiveQubscription>>>(
                            Expression.Default(typeof(Task<IAsyncReactiveQubscription>)),
                            Expression.Parameter(typeof(int)),
                            Expression.Parameter(typeof(int))
                        ),
                        Expression.Default(typeof(int)),
                        Expression.Default(typeof(int))
                    ),
                    _subscriptionUri,
                    _state,
                    _token
                );
            });

            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                rebinder.BindSubscription(
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<int, int, Task<IAsyncReactiveQubscription>>)),
                        Expression.Parameter(typeof(int)),
                        Expression.Parameter(typeof(int))
                    ),
                    _subscriptionUri,
                    _state,
                    _token
                );
            });

            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                rebinder.BindSubscription(
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<int, Task<IAsyncReactiveQubscription>>), "rx://observable/subscribe"),
                        Expression.Default(typeof(int))
                    ),
                    _subscriptionUri,
                    _state,
                    _token
                );
            });

            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                rebinder.BindSubscription(
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<List<int>, Task<IAsyncReactiveQubscription>>), "rx://observable/subscribe"),
                        Expression.Default(typeof(List<int>))
                    ),
                    _subscriptionUri,
                    _state,
                    _token
                );
            });

            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                rebinder.BindSubscription(
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<List<int>, Task<IAsyncReactiveQubscription>>), "rx://observable/subscribe"),
                        Expression.New(typeof(List<int>).GetConstructor(Type.EmptyTypes))
                    ),
                    _subscriptionUri,
                    _state,
                    _token
                );
            });

            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                rebinder.BindSubscription(
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<Tuple<int, int>, Task<IAsyncReactiveQubscription>>), "rx://observable/subscribe"),
                        Expression.New(
                            typeof(Tuple<int, int>).GetConstructor(new[] { typeof(int), typeof(int) }),
                            Expression.Default(typeof(int)),
                            Expression.Default(typeof(int))
                        )
                    ),
                    _subscriptionUri,
                    _state,
                    _token
                );
            });

            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                rebinder.BindSubscription(
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<Tuple<List<int>, IAsyncReactiveQbserver<int>>, Task<IAsyncReactiveQubscription>>), "rx://observable/subscribe"),
                        Expression.New(
                            typeof(Tuple<List<int>, IAsyncReactiveQbserver<int>>).GetConstructor(new[] { typeof(List<int>), typeof(IAsyncReactiveQbserver<int>) }),
                            Expression.Default(typeof(List<int>)),
                            Expression.Default(typeof(IAsyncReactiveQbserver<int>))
                        )
                    ),
                    _subscriptionUri,
                    _state,
                    _token
                );
            });

            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                rebinder.BindSubscription(
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<Tuple<IAsyncReactiveQbservable<int>, List<int>>, Task<IAsyncReactiveQubscription>>), "rx://observable/subscribe"),
                        Expression.New(
                            typeof(Tuple<IAsyncReactiveQbservable<int>, List<int>>).GetConstructor(new[] { typeof(IAsyncReactiveQbservable<int>), typeof(List<int>) }),
                            Expression.Default(typeof(IAsyncReactiveQbservable<int>)),
                            Expression.Default(typeof(List<int>))
                        )
                    ),
                    _subscriptionUri,
                    _state,
                    _token
                );
            });

            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                rebinder.BindSubscription(
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<Tuple<IAsyncReactiveQbservable<int>, int>, Task<IAsyncReactiveQubscription>>), "rx://observable/subscribe"),
                        Expression.New(
                            typeof(Tuple<IAsyncReactiveQbservable<int>, int>).GetConstructor(new[] { typeof(IAsyncReactiveQbservable<int>), typeof(int) }),
                            Expression.Default(typeof(IAsyncReactiveQbservable<int>)),
                            Expression.Default(typeof(int))
                        )
                    ),
                    _subscriptionUri,
                    _state,
                    _token
                );
            });

            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                rebinder.BindSubscription(
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<int, int, Task<IAsyncReactiveQubscription>>), "rx://observable/subscribe"),
                        Expression.Default(typeof(int)),
                        Expression.Default(typeof(int))
                    ),
                    _subscriptionUri,
                    _state,
                    _token
                );
            });

            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                rebinder.BindSubscription(
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<List<int>, List<int>, Task<IAsyncReactiveQubscription>>), "rx://observable/subscribe"),
                        Expression.Default(typeof(List<int>)),
                        Expression.Default(typeof(List<int>))
                    ),
                    _subscriptionUri,
                    _state,
                    _token
                );
            });
        }

        [TestMethod]
        public void UriToReactiveProxyBinder_SimpleBindSubscription()
        {
            AssertSubscriptionRewrite(
                Expression.Invoke(
                    Expression.Parameter(_subscribeType, Constants.SubscribeUri),
                    Expression.New(
                        _subscribeArgumentType.GetConstructor(new[] { _observableType, _observerType }),
                        Expression.Parameter(_observableType, "bing://xs"),
                        Expression.Parameter(_observerType, "bing://observer")
                    )
                ),
                Expression.Parameter(typeof(IReactiveProxy), "rx://builtin/this").Let(@this =>
                    Expression.Lambda(
                        Expression.Call(
                            Expression.Call(
                                @this,
                                _getObservable.MakeGenericMethod(_elementType),
                                Expression.Constant(new Uri("bing://xs"), typeof(Uri))
                            ),
                            _observableType.GetMethod("SubscribeAsync"),
                            Expression.Call(
                                @this,
                                _getObserver.MakeGenericMethod(_elementType),
                                Expression.Constant(new Uri("bing://observer"), typeof(Uri))
                            ),
                            Expression.Constant(_subscriptionUri, typeof(Uri)),
                            Expression.Constant(_state, typeof(object)),
                            Expression.Constant(_token, typeof(CancellationToken))
                        ),
                        @this
                    )
                )
            );
        }

        [TestMethod]
        public void UriToReactiveProxyBinder_ParameterizedSubscription()
        {
            AssertSubscriptionRewrite(
                Expression.Invoke(
                    Expression.Parameter(_subscribeType, Constants.SubscribeUri),
                    Expression.New(
                        _subscribeArgumentType.GetConstructor(new[] { _observableType, _observerType }),
                        Expression.Invoke(
                            Expression.Parameter(typeof(Func<,>).MakeGenericType(_elementType, _observableType), "bing://xs"),
                            Expression.Default(_elementType)
                        ),
                        Expression.Invoke(
                            Expression.Parameter(typeof(Func<,>).MakeGenericType(_elementType, _observerType), "bing://observer"),
                            Expression.Default(_elementType)
                        )
                    )
                ),
                Expression.Parameter(typeof(IReactiveProxy), "rx://builtin/this").Let(@this =>
                    Expression.Lambda(
                        Expression.Call(
                            Expression.Invoke(
                                Expression.Call(
                                    @this,
                                    _getObservableParam.MakeGenericMethod(_elementType, _elementType),
                                    Expression.Constant(new Uri("bing://xs"), typeof(Uri))
                                ),
                                Expression.Default(_elementType)
                            ),
                            _observableType.GetMethod("SubscribeAsync"),
                            Expression.Invoke(
                                Expression.Call(
                                    @this,
                                    _getObserverParam.MakeGenericMethod(_elementType, _elementType),
                                    Expression.Constant(new Uri("bing://observer"), typeof(Uri))
                                ),
                                Expression.Default(_elementType)
                            ),
                            Expression.Constant(_subscriptionUri, typeof(Uri)),
                            Expression.Constant(_state, typeof(object)),
                            Expression.Constant(_token, typeof(CancellationToken))
                        ),
                        @this
                    )
                )
            );
        }

        [TestMethod]
        public void UriToReactiveProxyBinder_BindStream_InvalidSignaturesFail()
        {
            var rebinder = new UriToReactiveProxyBinder();

            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                rebinder.BindStream(
                    Expression.Parameter(typeof(int)),
                    _streamUri,
                    _state,
                    _token
                );
            });

            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                rebinder.BindStream(
                    Expression.Invoke(
                        Expression.Default(typeof(Func<int, int>)),
                        Expression.Default(typeof(int))
                    ),
                    _streamUri,
                    _state,
                    _token
                );
            });

            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                rebinder.BindStream(
                    Expression.Invoke(
                        Expression.Default(typeof(Func<int, List<int>>)),
                        Expression.Default(typeof(int))
                    ),
                    _streamUri,
                    _state,
                    _token
                );
            });

            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                rebinder.BindStream(
                    Expression.Invoke(
                        Expression.Default(typeof(Func<int, int, int, IAsyncReactiveQubject<int, int>>)),
                        Expression.Default(typeof(int)),
                        Expression.Default(typeof(int)),
                        Expression.Default(typeof(int))
                    ),
                    _streamUri,
                    _state,
                    _token
                );
            });

            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                rebinder.BindStream(
                    Expression.Invoke(
                        Expression.Lambda<Func<int, int, IAsyncReactiveQubject<int, int>>>(
                            Expression.Default(typeof(IAsyncReactiveQubject<int, int>)),
                            Expression.Parameter(typeof(int)),
                            Expression.Parameter(typeof(int))
                        ),
                        Expression.Default(typeof(int)),
                        Expression.Default(typeof(int))
                    ),
                    _streamUri,
                    _state,
                    _token
                );
            });

            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                rebinder.BindStream(
                    Expression.Invoke(
                        Expression.Lambda<Func<int, IAsyncReactiveQubject<int, int>>>(
                            Expression.Default(typeof(IAsyncReactiveQubject<int, int>)),
                            Expression.Parameter(typeof(int))
                        ),
                        Expression.Default(typeof(int))
                    ),
                    _streamUri,
                    _state,
                    _token
                );
            });

            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                rebinder.BindStream(
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<int, IAsyncReactiveQubject<int, int>>)),
                        Expression.Default(typeof(int))
                    ),
                    _streamUri,
                    _state,
                    _token
                );
            });
        }

        [TestMethod]
        public void UriToReactiveProxyBinder_SimpleBindStream()
        {
            AssertStreamRewrite(
                Expression.Invoke(
                    Expression.Parameter(_createStreamArgumentType, "bing://sf")
                ),
                Expression.Parameter(typeof(IReactiveProxy), "rx://builtin/this").Let(@this =>
                    Expression.Lambda(
                        Expression.Call(
                            Expression.Call(
                                @this,
                                _getStreamFactory.MakeGenericMethod(_elementType, _elementType),
                                Expression.Constant(new Uri("bing://sf"), typeof(Uri))
                            ),
                            _subjectFactoryType.GetMethod("CreateAsync"),
                            Expression.Constant(_streamUri, typeof(Uri)),
                            Expression.Constant(_state, typeof(object)),
                            Expression.Constant(_token, typeof(CancellationToken))
                        ),
                        @this
                    )
                )
            );
        }

        [TestMethod]
        public void UriToReactiveProxyBinder_SimpleBindStreamParameterized()
        {
            AssertStreamRewrite(
                Expression.Invoke(
                    Expression.Parameter(_createStreamType, "bing://sf"),
                    Expression.Default(_parameterType)
                ),
                Expression.Parameter(typeof(IReactiveProxy), "rx://builtin/this").Let(@this =>
                    Expression.Lambda(
                        Expression.Call(
                            Expression.Call(
                                @this,
                                _getStreamFactoryParam.MakeGenericMethod(_parameterType, _elementType, _elementType),
                                Expression.Constant(new Uri("bing://sf"), typeof(Uri))
                            ),
                            _subjectFactoryParamType.GetMethod("CreateAsync"),
                            Expression.Constant(_streamUri, typeof(Uri)),
                            Expression.Default(_parameterType),
                            Expression.Constant(_state, typeof(object)),
                            Expression.Constant(_token, typeof(CancellationToken))
                        ),
                        @this
                    )
                )
            );
        }

        [TestMethod]
        public void UriToReactiveProxyBinder_SimpleBindSubscription_Factory()
        {
            AssertSubscriptionRewrite(
                Expression.Invoke(
                    Expression.Parameter(_createSubscriptionArgumentType, "bing://sf")
                ),
                Expression.Parameter(typeof(IReactiveProxy), "rx://builtin/this").Let(@this =>
                    Expression.Lambda(
                        Expression.Call(
                            Expression.Call(
                                @this,
                                _getSubscriptionFactory,
                                Expression.Constant(new Uri("bing://sf"), typeof(Uri))
                            ),
                            _subscriptionFactoryType.GetMethod("CreateAsync"),
                            Expression.Constant(_subscriptionUri, typeof(Uri)),
                            Expression.Constant(_state, typeof(object)),
                            Expression.Constant(_token, typeof(CancellationToken))
                        ),
                        @this
                    )
                )
            );
        }

        [TestMethod]
        public void UriToReactiveProxyBinder_SimpleBindSubscriptionParameterized_Factory()
        {
            AssertSubscriptionRewrite(
                Expression.Invoke(
                    Expression.Parameter(_createSubscriptionType, "bing://sf"),
                    Expression.Default(_parameterType)
                ),
                Expression.Parameter(typeof(IReactiveProxy), "rx://builtin/this").Let(@this =>
                    Expression.Lambda(
                        Expression.Call(
                            Expression.Call(
                                @this,
                                _getSubscriptionFactoryParam.MakeGenericMethod(_parameterType),
                                Expression.Constant(new Uri("bing://sf"), typeof(Uri))
                            ),
                            _subscriptionFactoryParamType.GetMethod("CreateAsync"),
                            Expression.Constant(_subscriptionUri, typeof(Uri)),
                            Expression.Default(_parameterType),
                            Expression.Constant(_state, typeof(object)),
                            Expression.Constant(_token, typeof(CancellationToken))
                        ),
                        @this
                    )
                )
            );
        }

        [TestMethod]
        public void UriToReactiveProxyBinder_BindObservable_InvalidSignaturesFail()
        {
            var rebinder = new UriToReactiveProxyBinder();

            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                rebinder.BindObservable(
                    Expression.Parameter(typeof(int)),
                    _observableUri,
                    _state,
                    _token
                );
            });

            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                rebinder.BindObservable(
                    Expression.Parameter(typeof(Func<int, int>)),
                    _observableUri,
                    _state,
                    _token
                );
            });

            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                rebinder.BindObservable(
                    Expression.Parameter(typeof(List<int>)),
                    _observableUri,
                    _state,
                    _token
                );
            });

            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                rebinder.BindObservable(
                    Expression.Parameter(typeof(Func<int, List<int>>)),
                    _observableUri,
                    _state,
                    _token
                );
            });
        }

        [TestMethod]
        public void UriToReactiveProxyBinder_Observable()
        {
            AssertObservableRewrite(
                Expression.Parameter(_observableType, _fooId),
                Expression.Parameter(typeof(IReactiveProxy), _thisId).Let(@this =>
                    Expression.Lambda(
                        Expression.Call(
                            @this,
                            _defineObservable.MakeGenericMethod(_elementType),
                            Expression.Constant(_observableUri, typeof(Uri)),
                            Expression.Call(
                                @this,
                                _getObservable.MakeGenericMethod(_elementType),
                                Expression.Constant(_fooUri)
                            ),
                            Expression.Constant(_state),
                            Expression.Constant(_token)
                        ),
                        @this
                    )
                )
            );
        }

        [TestMethod]
        public void UriToReactiveProxyBinder_ObservableParameterized()
        {
            AssertObservableRewrite(
                Expression.Parameter(_parameterType).Let(x =>
                    Expression.Lambda(
                        Expression.Invoke(
                            Expression.Parameter(_observableParamType, _fooId),
                            x
                        ),
                        x
                    )
                ),
                Expression.Parameter(typeof(IReactiveProxy), _thisId).Let(@this =>
                    Expression.Lambda(
                        Expression.Call(
                            @this,
                            _defineObservableParam.MakeGenericMethod(_parameterType, _elementType),
                            Expression.Constant(_observableUri, typeof(Uri)),
                            Expression.Parameter(_parameterType).Let(x =>
                                Expression.Lambda(
                                    Expression.Invoke(
                                        Expression.Call(
                                            @this,
                                            _getObservableParam.MakeGenericMethod(_parameterType, _elementType),
                                            Expression.Constant(_fooUri)
                                        ),
                                        x
                                    ),
                                    x
                                )
                            ),
                            Expression.Constant(_state),
                            Expression.Constant(_token)
                        ),
                        @this
                    )
                )
            );
        }

        [TestMethod]
        public void UriToReactiveProxyBinder_Observable_NoUnbound()
        {
            AssertObservableRewrite(
                Expression.Default(_observableType),
                Expression.Parameter(typeof(IReactiveProxy), _thisId).Let(@this =>
                    Expression.Lambda(
                        Expression.Call(
                            @this,
                            _defineObservable.MakeGenericMethod(_elementType),
                            Expression.Constant(_observableUri, typeof(Uri)),
                            Expression.Default(_observableType),
                            Expression.Constant(_state),
                            Expression.Constant(_token)
                        ),
                        @this
                    )
                )
            );
        }

        [TestMethod]
        public void UriToReactiveProxyBinder_BindObserver_InvalidSignaturesFail()
        {
            var rebinder = new UriToReactiveProxyBinder();

            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                rebinder.BindObserver(
                    Expression.Parameter(typeof(int)),
                    _observerUri,
                    _state,
                    _token
                );
            });

            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                rebinder.BindObserver(
                    Expression.Parameter(typeof(Func<int, int>)),
                    _observerUri,
                    _state,
                    _token
                );
            });

            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                rebinder.BindObserver(
                    Expression.Parameter(typeof(List<int>)),
                    _observerUri,
                    _state,
                    _token
                );
            });


            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                rebinder.BindObserver(
                    Expression.Parameter(typeof(Func<int, List<int>>)),
                    _observerUri,
                    _state,
                    _token
                );
            });
        }

        [TestMethod]
        public void UriToReactiveProxyBinder_Observer()
        {
            AssertObserverRewrite(
                Expression.Parameter(_observerType, _fooId),
                Expression.Parameter(typeof(IReactiveProxy), _thisId).Let(@this =>
                    Expression.Lambda(
                        Expression.Call(
                            @this,
                            _defineObserver.MakeGenericMethod(_elementType),
                            Expression.Constant(_observerUri, typeof(Uri)),
                            Expression.Call(
                                @this,
                                _getObserver.MakeGenericMethod(_elementType),
                                Expression.Constant(_fooUri)
                            ),
                            Expression.Constant(_state),
                            Expression.Constant(_token)
                        ),
                        @this
                    )
                )
            );
        }

        [TestMethod]
        public void UriToReactiveProxyBinder_ObserverParameterized()
        {
            AssertObserverRewrite(
                Expression.Parameter(_parameterType).Let(x =>
                    Expression.Lambda(
                        Expression.Invoke(
                            Expression.Parameter(_observerParamType, _fooId),
                            x
                        ),
                        x
                    )
                ),
                Expression.Parameter(typeof(IReactiveProxy), _thisId).Let(@this =>
                    Expression.Lambda(
                        Expression.Call(
                            @this,
                            _defineObserverParam.MakeGenericMethod(_parameterType, _elementType),
                            Expression.Constant(_observerUri, typeof(Uri)),
                            Expression.Parameter(_parameterType).Let(x =>
                                Expression.Lambda(
                                    Expression.Invoke(
                                        Expression.Call(
                                            @this,
                                            _getObserverParam.MakeGenericMethod(_parameterType, _elementType),
                                            Expression.Constant(_fooUri)
                                        ),
                                        x
                                    ),
                                    x
                                )
                            ),
                            Expression.Constant(_state),
                            Expression.Constant(_token)
                        ),
                        @this
                    )
                )
            );
        }

        [TestMethod]
        public void UriToReactiveProxyBinder_Observer_NoUnbound()
        {
            AssertObserverRewrite(
                Expression.Default(_observerType),
                Expression.Parameter(typeof(IReactiveProxy), _thisId).Let(@this =>
                    Expression.Lambda(
                        Expression.Call(
                            @this,
                            _defineObserver.MakeGenericMethod(_elementType),
                            Expression.Constant(_observerUri, typeof(Uri)),
                            Expression.Default(_observerType),
                            Expression.Constant(_state),
                            Expression.Constant(_token)
                        ),
                        @this
                    )
                )
            );
        }

        [TestMethod]
        public void UriToReactiveBinder_LookupStream()
        {
            AssertObservableRewrite(
                Expression.Invoke(
                    Expression.Parameter(_observableSubjectParamType, _fooId),
                    Expression.Parameter(_subjectType, _barId)
                ),
                Expression.Parameter(typeof(IReactiveProxy), _thisId).Let(@this =>
                    Expression.Lambda(
                        Expression.Call(
                            @this,
                            _defineObservable.MakeGenericMethod(_elementType),
                            Expression.Constant(_observableUri, typeof(Uri)),
                            Expression.Invoke(
                                Expression.Call(
                                    @this,
                                    _getObservableParam.MakeGenericMethod(_subjectType, _elementType),
                                    Expression.Constant(_fooUri)
                                ),
                                Expression.Call(
                                    @this,
                                    _getStream.MakeGenericMethod(_elementType, _elementType),
                                    Expression.Constant(_barUri)
                                )
                            ),
                            Expression.Constant(_state),
                            Expression.Constant(_token)
                        ),
                        @this
                    )
                )
            );
        }

        [TestMethod]
        public void UriToReactiveBinder_LookupSubscription()
        {
            AssertObservableRewrite(
                Expression.Invoke(
                    Expression.Parameter(_observableSubscriptionParamType, _fooId),
                    Expression.Parameter(_subscriptionType, _barId)
                ),
                Expression.Parameter(typeof(IReactiveProxy), _thisId).Let(@this =>
                    Expression.Lambda(
                        Expression.Call(
                            @this,
                            _defineObservable.MakeGenericMethod(_elementType),
                            Expression.Constant(_observableUri, typeof(Uri)),
                            Expression.Invoke(
                                Expression.Call(
                                    @this,
                                    _getObservableParam.MakeGenericMethod(_subscriptionType, _elementType),
                                    Expression.Constant(_fooUri)
                                ),
                                Expression.Call(
                                    @this,
                                    _getSubscription,
                                    Expression.Constant(_barUri)
                                )
                            ),
                            Expression.Constant(_state),
                            Expression.Constant(_token)
                        ),
                        @this
                    )
                )
            );
        }

        [TestMethod]
        public void UriToReactiveBinder_LookupOther_ThrowsInvalidOperation()
        {
            var binder = new UriToReactiveProxyBinder();
            Assert.ThrowsException<InvalidOperationException>(() =>
                binder.BindObservable(
                    Expression.Invoke(
                        Expression.Parameter(_observableOtherParamType, _fooId),
                        Expression.Parameter(_otherType, _barId)
                    ),
                    _observableUri,
                    _state,
                    _token
                )
            );
        }

        private static void AssertSubscriptionRewrite(Expression before, Expression after)
        {
            var rebinder = new UriToReactiveProxyBinder();
            var comparer = new ExpressionEqualityComparer(() => new Comparator());
            var rewritten = rebinder.BindSubscription(before, _subscriptionUri, _state, _token);
            Assert.IsTrue(comparer.Equals(rewritten, after), string.Format(CultureInfo.InvariantCulture, "Expected: {0}, Actual: {1}", after, rewritten));
        }

        private static void AssertStreamRewrite(Expression before, Expression after)
        {
            var rebinder = new UriToReactiveProxyBinder();
            var comparer = new ExpressionEqualityComparer(() => new Comparator());
            var rewritten = rebinder.BindStream(before, _streamUri, _state, _token);
            Assert.IsTrue(comparer.Equals(rewritten, after), string.Format(CultureInfo.InvariantCulture, "Expected: {0}, Actual: {1}", after, rewritten));
        }

        private static void AssertObservableRewrite(Expression before, Expression after)
        {
            var rebinder = new UriToReactiveProxyBinder();
            var comparer = new ExpressionEqualityComparer(() => new Comparator());
            var rewritten = rebinder.BindObservable(before, _observableUri, _state, _token);
            Assert.IsTrue(comparer.Equals(rewritten, after), string.Format(CultureInfo.InvariantCulture, "Expected: {0}, Actual: {1}", after, rewritten));
        }

        private static void AssertObserverRewrite(Expression before, Expression after)
        {
            var rebinder = new UriToReactiveProxyBinder();
            var comparer = new ExpressionEqualityComparer(() => new Comparator());
            var rewritten = rebinder.BindObserver(before, _observerUri, _state, _token);
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
