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

using Reaqtor;
using Reaqtor.Service.Core;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Reaqtor
{
    [TestClass]
    public class UriToReactiveBinderTests
    {
        private static readonly MethodInfo _getStreamFactory = ((MethodInfo)ReflectionHelpers.InfoOf((IReactive r) => r.GetStreamFactory<object, object>(null))).GetGenericMethodDefinition();
        private static readonly MethodInfo _getStreamFactoryParam = ((MethodInfo)ReflectionHelpers.InfoOf((IReactive r) => r.GetStreamFactory<object, object, object>(null))).GetGenericMethodDefinition();

        private static readonly MethodInfo _getSubscriptionFactory = (MethodInfo)ReflectionHelpers.InfoOf((IReactive r) => r.GetSubscriptionFactory(null));
        private static readonly MethodInfo _getSubscriptionFactoryParam = ((MethodInfo)ReflectionHelpers.InfoOf((IReactive r) => r.GetSubscriptionFactory<object>(null))).GetGenericMethodDefinition();

        private static readonly MethodInfo _getStream = ((MethodInfo)ReflectionHelpers.InfoOf((IReactive r) => r.GetStream<object, object>(null))).GetGenericMethodDefinition();

        private static readonly MethodInfo _getObservable = ((MethodInfo)ReflectionHelpers.InfoOf((IReactive r) => r.GetObservable<object>(null))).GetGenericMethodDefinition();
        private static readonly MethodInfo _getObservableParam = ((MethodInfo)ReflectionHelpers.InfoOf((IReactive r) => r.GetObservable<object, object>(null))).GetGenericMethodDefinition();

        private static readonly MethodInfo _getObserver = ((MethodInfo)ReflectionHelpers.InfoOf((IReactive r) => r.GetObserver<object>(null))).GetGenericMethodDefinition();
        private static readonly MethodInfo _getObserverParam = ((MethodInfo)ReflectionHelpers.InfoOf((IReactive r) => r.GetObserver<object, object>(null))).GetGenericMethodDefinition();

        private static readonly MethodInfo _getSubscription = ((MethodInfo)ReflectionHelpers.InfoOf((IReactive r) => r.GetSubscription(null)));

        private static readonly MethodInfo _defineObservable = ((MethodInfo)ReflectionHelpers.InfoOf((IReactive r) => r.DefineObservable<object>(null, null, null))).GetGenericMethodDefinition();
        private static readonly MethodInfo _defineObservableParam = ((MethodInfo)ReflectionHelpers.InfoOf((IReactive r) => r.DefineObservable<object, object>(null, null, null))).GetGenericMethodDefinition();

        private static readonly MethodInfo _defineObserver = ((MethodInfo)ReflectionHelpers.InfoOf((IReactive r) => r.DefineObserver<object>(null, null, null))).GetGenericMethodDefinition();
        private static readonly MethodInfo _defineObserverParam = ((MethodInfo)ReflectionHelpers.InfoOf((IReactive r) => r.DefineObserver<object, object>(null, null, null))).GetGenericMethodDefinition();

        private static readonly Type _elementType = typeof(int);
        private static readonly Type _argumentType = typeof(string);
        private static readonly Type _observableType = typeof(IReactiveQbservable<>).MakeGenericType(_elementType);
        private static readonly Type _observableParamType = typeof(Func<,>).MakeGenericType(_elementType, _observableType);
        private static readonly Type _observerType = typeof(IReactiveQbserver<>).MakeGenericType(_elementType);
        private static readonly Type _observerParamType = typeof(Func<,>).MakeGenericType(_elementType, _observerType);
        private static readonly Type _subjectType = typeof(IReactiveQubject<,>).MakeGenericType(_elementType, _elementType);
        private static readonly Type _subscriptionType = typeof(IReactiveQubscription);
        private static readonly Type _subjectFactoryType = typeof(IReactiveQubjectFactory<,>).MakeGenericType(_elementType, _elementType);
        private static readonly Type _parameterizedSubjectFactoryType = typeof(IReactiveQubjectFactory<,,>).MakeGenericType(_elementType, _elementType, _argumentType);
        private static readonly Type _subscriptionFactoryType = typeof(IReactiveQubscriptionFactory);
        private static readonly Type _parameterizedSubscriptionFactoryType = typeof(IReactiveQubscriptionFactory<>).MakeGenericType(_argumentType);
        private static readonly Type _subscribeTupleType = typeof(Tuple<,>).MakeGenericType(_observableType, _observerType);
        private static readonly Type _subscribeDelegateType = typeof(Func<,>).MakeGenericType(_subscribeTupleType, _subscriptionType);
        private static readonly Type _createStreamDelegateType = typeof(Func<>).MakeGenericType(_subjectType);
        private static readonly Type _createStreamParameterizedDelegateType = typeof(Func<,>).MakeGenericType(_argumentType, _subjectType);
        private static readonly Type _createSubscriptionDelegateType = typeof(Func<>).MakeGenericType(_subscriptionType);
        private static readonly Type _createSubscriptionParameterizedDelegateType = typeof(Func<,>).MakeGenericType(_argumentType, _subscriptionType);
        private static readonly Type _otherType = typeof(Func<object>);
        private static readonly Type _observableSubjectParamType = typeof(Func<,>).MakeGenericType(_subjectType, _observableType);
        private static readonly Type _observableSubscriptionParamType = typeof(Func<,>).MakeGenericType(_subscriptionType, _observableType);
        private static readonly Type _observableOtherParamType = typeof(Func<,>).MakeGenericType(_otherType, _observableType);

        private static readonly string _id = "bing://id";
        private static readonly string _fooId = "bing://foo";
        private static readonly string _barId = "bing://bar";
        private static readonly string _thisId = "rx://builtin/this";
        private static readonly Uri _uri = new(_id);
        private static readonly Uri _fooUri = new(_fooId);
        private static readonly Uri _barUri = new(_barId);

        private static readonly Uri _subscriptionUri = new("bing://sub");
        private static readonly Uri _observableUri = new("bing://obs");
        private static readonly Uri _observerUri = new("bing://obv");
        private static readonly Uri _subjectUri = new("bing://str");
        private static readonly object _state = null;

        [TestMethod]
        public void UriToReactiveBinder_BindSubscription_InvalidSignaturesFail()
        {
            var rebinder = new UriToReactiveBinder();

            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                rebinder.BindSubscription(
                    Expression.Parameter(typeof(int)),
                    _subscriptionUri,
                    _state
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
                    _state
                );
            });

            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                rebinder.BindSubscription(
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<int, int, int, IReactiveQubscription>)),
                        Expression.Default(typeof(int)),
                        Expression.Default(typeof(int)),
                        Expression.Default(typeof(int))
                    ),
                    _subscriptionUri,
                    _state
                );
            });

            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                rebinder.BindSubscription(
                    Expression.Invoke(
                        Expression.Lambda<Func<int, int, IReactiveQubscription>>(
                            Expression.Default(typeof(IReactiveQubscription)),
                            Expression.Parameter(typeof(int)),
                            Expression.Parameter(typeof(int))
                        ),
                        Expression.Default(typeof(int)),
                        Expression.Default(typeof(int))
                    ),
                    _subscriptionUri,
                    _state
                );
            });

            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                rebinder.BindSubscription(
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<int, int, IReactiveQubscription>)),
                        Expression.Parameter(typeof(int)),
                        Expression.Parameter(typeof(int))
                    ),
                    _subscriptionUri,
                    _state
                );
            });

            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                rebinder.BindSubscription(
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<int, IReactiveQubscription>), "rx://observable/subscribe"),
                        Expression.Default(typeof(int))
                    ),
                    _subscriptionUri,
                    _state
                );
            });

            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                rebinder.BindSubscription(
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<List<int>, IReactiveQubscription>), "rx://observable/subscribe"),
                        Expression.Default(typeof(List<int>))
                    ),
                    _subscriptionUri,
                    _state
                );
            });

            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                rebinder.BindSubscription(
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<List<int>, IReactiveQubscription>), "rx://observable/subscribe"),
                        Expression.New(typeof(List<int>).GetConstructor(Type.EmptyTypes))
                    ),
                    _subscriptionUri,
                    _state
                );
            });

            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                rebinder.BindSubscription(
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<Tuple<int, int>, IReactiveQubscription>), "rx://observable/subscribe"),
                        Expression.New(
                            typeof(Tuple<int, int>).GetConstructor(new[] { typeof(int), typeof(int) }),
                            Expression.Default(typeof(int)),
                            Expression.Default(typeof(int))
                        )
                    ),
                    _subscriptionUri,
                    _state
                );
            });

            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                rebinder.BindSubscription(
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<Tuple<List<int>, IReactiveQbserver<int>>, IReactiveQubscription>), "rx://observable/subscribe"),
                        Expression.New(
                            typeof(Tuple<List<int>, IReactiveQbserver<int>>).GetConstructor(new[] { typeof(List<int>), typeof(IReactiveQbserver<int>) }),
                            Expression.Default(typeof(List<int>)),
                            Expression.Default(typeof(IReactiveQbserver<int>))
                        )
                    ),
                    _subscriptionUri,
                    _state
                );
            });

            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                rebinder.BindSubscription(
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<Tuple<IReactiveQbservable<int>, List<int>>, IReactiveQubscription>), "rx://observable/subscribe"),
                        Expression.New(
                            typeof(Tuple<IReactiveQbservable<int>, List<int>>).GetConstructor(new[] { typeof(IReactiveQbservable<int>), typeof(List<int>) }),
                            Expression.Default(typeof(IReactiveQbservable<int>)),
                            Expression.Default(typeof(List<int>))
                        )
                    ),
                    _subscriptionUri,
                    _state
                );
            });

            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                rebinder.BindSubscription(
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<Tuple<IReactiveQbservable<int>, int>, IReactiveQubscription>), "rx://observable/subscribe"),
                        Expression.New(
                            typeof(Tuple<IReactiveQbservable<int>, int>).GetConstructor(new[] { typeof(IReactiveQbservable<int>), typeof(int) }),
                            Expression.Default(typeof(IReactiveQbservable<int>)),
                            Expression.Default(typeof(int))
                        )
                    ),
                    _subscriptionUri,
                    _state
                );
            });

            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                rebinder.BindSubscription(
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<int, int, IReactiveQubscription>), "rx://observable/subscribe"),
                        Expression.Default(typeof(int)),
                        Expression.Default(typeof(int))
                    ),
                    _subscriptionUri,
                    _state
                );
            });

            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                rebinder.BindSubscription(
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<List<int>, List<int>, IReactiveQubscription>), "rx://observable/subscribe"),
                        Expression.Default(typeof(List<int>)),
                        Expression.Default(typeof(List<int>))
                    ),
                    _subscriptionUri,
                    _state
                );
            });
        }

        [TestMethod]
        public void UriToReactiveBinder_BindObservable_InvalidSignaturesFail()
        {
            var rebinder = new UriToReactiveBinder();

            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                rebinder.BindObservable(
                    Expression.Parameter(typeof(int)),
                    _observableUri,
                    _state
                );
            });

            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                rebinder.BindObservable(
                    Expression.Parameter(typeof(Func<int, int>)),
                    _observableUri,
                    _state
                );
            });

            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                rebinder.BindObservable(
                    Expression.Parameter(typeof(List<int>)),
                    _observableUri,
                    _state
                );
            });

            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                rebinder.BindObservable(
                    Expression.Parameter(typeof(Func<int, List<int>>)),
                    _observableUri,
                    _state
                );
            });
        }

        [TestMethod]
        public void UriToReactiveBinder_BindObserver_InvalidSignaturesFail()
        {
            var rebinder = new UriToReactiveBinder();

            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                rebinder.BindObserver(
                    Expression.Parameter(typeof(int)),
                    _observerUri,
                    _state
                );
            });

            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                rebinder.BindObserver(
                    Expression.Parameter(typeof(Func<int, int>)),
                    _observerUri,
                    _state
                );
            });

            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                rebinder.BindObserver(
                    Expression.Parameter(typeof(List<int>)),
                    _observerUri,
                    _state
                );
            });


            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                rebinder.BindObserver(
                    Expression.Parameter(typeof(Func<int, List<int>>)),
                    _observerUri,
                    _state
                );
            });
        }

        [TestMethod]
        public void UriToReactiveBinder_BindSubject_InvalidSignaturesFail()
        {
            var rebinder = new UriToReactiveBinder();

            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                rebinder.BindSubject(
                    Expression.Parameter(typeof(int)),
                    _subjectUri,
                    _state
                );
            });

            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                rebinder.BindSubject(
                    Expression.Invoke(
                        Expression.Default(typeof(Func<int, int>)),
                        Expression.Default(typeof(int))
                    ),
                    _subjectUri,
                    _state
                );
            });

            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                rebinder.BindSubject(
                    Expression.Invoke(
                        Expression.Default(typeof(Func<int, List<int>>)),
                        Expression.Default(typeof(int))
                    ),
                    _subjectUri,
                    _state
                );
            });

            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                rebinder.BindSubject(
                    Expression.Invoke(
                        Expression.Default(typeof(Func<int, int, int, IReactiveQubject<int, int>>)),
                        Expression.Default(typeof(int)),
                        Expression.Default(typeof(int)),
                        Expression.Default(typeof(int))
                    ),
                    _subjectUri,
                    _state
                );
            });

            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                rebinder.BindSubject(
                    Expression.Invoke(
                        Expression.Lambda<Func<int, int, IReactiveQubject<int, int>>>(
                            Expression.Default(typeof(IReactiveQubject<int, int>)),
                            Expression.Parameter(typeof(int)),
                            Expression.Parameter(typeof(int))
                        ),
                        Expression.Default(typeof(int)),
                        Expression.Default(typeof(int))
                    ),
                    _subjectUri,
                    _state
                );
            });

            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                rebinder.BindSubject(
                    Expression.Invoke(
                        Expression.Lambda<Func<int, IReactiveQubject<int, int>>>(
                            Expression.Default(typeof(IReactiveQubject<int, int>)),
                            Expression.Parameter(typeof(int))
                        ),
                        Expression.Default(typeof(int))
                    ),
                    _subjectUri,
                    _state
                );
            });

            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                rebinder.BindSubject(
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<int, IReactiveQubject<int, int>>)),
                        Expression.Default(typeof(int))
                    ),
                    _subjectUri,
                    _state
                );
            });
        }

        [TestMethod]
        public void UriToReactiveBinder_Subscription()
        {
            AssertSubscriptionRewrite(
                Expression.Invoke(
                    Expression.Parameter(_subscribeDelegateType, Constants.SubscribeUri),
                    Expression.New(
                        _subscribeTupleType.GetConstructor(new[] { _observableType, _observerType }),
                        Expression.Parameter(_observableType, "bing://xs"),
                        Expression.Parameter(_observerType, "bing://observer")
                    )
                ),
                Expression.Parameter(typeof(IReactive), _thisId).Let(@this =>
                    Expression.Lambda(
                        Expression.Call(
                            Expression.Call(
                                @this,
                                _getObservable.MakeGenericMethod(_elementType),
                                Expression.Constant(new Uri("bing://xs"), typeof(Uri))
                            ),
                            _observableType.GetMethod("Subscribe"),
                            Expression.Call(
                                @this,
                                _getObserver.MakeGenericMethod(_elementType),
                                Expression.Constant(new Uri("bing://observer"), typeof(Uri))
                            ),
                            Expression.Constant(_uri, typeof(Uri)),
                            Expression.Constant(_state, typeof(object))
                        ),
                        @this
                    )
                )
            );
        }

        [TestMethod]
        public void UriToReactiveBinder_ParameterizedSubscription()
        {
            AssertSubscriptionRewrite(
                Expression.Invoke(
                    Expression.Parameter(_subscribeDelegateType, Constants.SubscribeUri),
                    Expression.New(
                        _subscribeTupleType.GetConstructor(new[] { _observableType, _observerType }),
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
                Expression.Parameter(typeof(IReactive), _thisId).Let(@this =>
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
                            _observableType.GetMethod("Subscribe"),
                            Expression.Invoke(
                                Expression.Call(
                                    @this,
                                    _getObserverParam.MakeGenericMethod(_elementType, _elementType),
                                    Expression.Constant(new Uri("bing://observer"), typeof(Uri))
                                ),
                                Expression.Default(_elementType)
                            ),
                            Expression.Constant(_uri, typeof(Uri)),
                            Expression.Constant(_state, typeof(object))
                        ),
                        @this
                    )
                )
            );
        }

        [TestMethod]
        public void UriToReactiveBinder_Stream()
        {
            AssertStreamRewrite(
                Expression.Invoke(
                    Expression.Parameter(_createStreamDelegateType, "bing://sf")
                ),
                Expression.Parameter(typeof(IReactive), _thisId).Let(@this =>
                    Expression.Lambda(
                        Expression.Call(
                            Expression.Call(
                                @this,
                                _getStreamFactory.MakeGenericMethod(_elementType, _elementType),
                                Expression.Constant(new Uri("bing://sf"), typeof(Uri))
                            ),
                            _subjectFactoryType.GetMethod("Create"),
                            Expression.Constant(_uri, typeof(Uri)),
                            Expression.Constant(_state, typeof(object))
                        ),
                        @this
                    )
                )
            );
        }

        [TestMethod]
        public void UriToReactiveBinder_StreamParameterized()
        {
            var method = _getStreamFactoryParam.MakeGenericMethod(_argumentType, _elementType, _elementType);
            AssertStreamRewrite(
                Expression.Invoke(
                    Expression.Parameter(_createStreamParameterizedDelegateType, "bing://sf"),
                    Expression.Default(_argumentType)
                ),
                Expression.Parameter(typeof(IReactive), _thisId).Let(@this =>
                    Expression.Lambda(
                        Expression.Call(
                            Expression.Call(
                                @this,
                                _getStreamFactoryParam.MakeGenericMethod(_argumentType, _elementType, _elementType),
                                Expression.Constant(new Uri("bing://sf"), typeof(Uri))
                            ),
                            _parameterizedSubjectFactoryType.GetMethod("Create"),
                            Expression.Constant(_uri, typeof(Uri)),
                            Expression.Default(_argumentType),
                            Expression.Constant(_state, typeof(object))
                        ),
                        @this
                    )
                )
            );
        }

        [TestMethod]
        public void UriToReactiveBinder_Subscription_Factory()
        {
            AssertSubscriptionRewrite(
                Expression.Invoke(
                    Expression.Parameter(_createSubscriptionDelegateType, "bing://sf")
                ),
                Expression.Parameter(typeof(IReactive), _thisId).Let(@this =>
                    Expression.Lambda(
                        Expression.Call(
                            Expression.Call(
                                @this,
                                _getSubscriptionFactory,
                                Expression.Constant(new Uri("bing://sf"), typeof(Uri))
                            ),
                            _subscriptionFactoryType.GetMethod("Create"),
                            Expression.Constant(_uri, typeof(Uri)),
                            Expression.Constant(_state, typeof(object))
                        ),
                        @this
                    )
                )
            );
        }

        [TestMethod]
        public void UriToReactiveBinder_SubscriptionParameterized_Factory()
        {
            var method = _getSubscriptionFactoryParam.MakeGenericMethod(_argumentType);
            AssertSubscriptionRewrite(
                Expression.Invoke(
                    Expression.Parameter(_createSubscriptionParameterizedDelegateType, "bing://sf"),
                    Expression.Default(_argumentType)
                ),
                Expression.Parameter(typeof(IReactive), _thisId).Let(@this =>
                    Expression.Lambda(
                        Expression.Call(
                            Expression.Call(
                                @this,
                                _getSubscriptionFactoryParam.MakeGenericMethod(_argumentType),
                                Expression.Constant(new Uri("bing://sf"), typeof(Uri))
                            ),
                            _parameterizedSubscriptionFactoryType.GetMethod("Create"),
                            Expression.Constant(_uri, typeof(Uri)),
                            Expression.Default(_argumentType),
                            Expression.Constant(_state, typeof(object))
                        ),
                        @this
                    )
                )
            );
        }

        [TestMethod]
        public void UriToReactiveBinder_Observable_NoUnbound()
        {
            AssertObservableRewrite(
                Expression.Default(_observableType),
                Expression.Parameter(typeof(IReactive), _thisId).Let(@this =>
                    Expression.Lambda(
                        Expression.Call(
                            @this,
                            _defineObservable.MakeGenericMethod(_elementType),
                            Expression.Constant(_uri, typeof(Uri)),
                            Expression.Default(_observableType),
                            Expression.Constant(_state)
                        ),
                        @this
                    )
                )
            );
        }

        [TestMethod]
        public void UriToReactiveBinder_Observable()
        {
            AssertObservableRewrite(
                Expression.Parameter(_observableType, _fooId),
                Expression.Parameter(typeof(IReactive), _thisId).Let(@this =>
                    Expression.Lambda(
                        Expression.Call(
                            @this,
                            _defineObservable.MakeGenericMethod(_elementType),
                            Expression.Constant(_uri, typeof(Uri)),
                            Expression.Call(
                                @this,
                                _getObservable.MakeGenericMethod(_elementType),
                                Expression.Constant(_fooUri)
                            ),
                            Expression.Constant(_state)
                        ),
                        @this
                    )
                )
            );
        }

        [TestMethod]
        public void UriToReactiveBinder_ObservableParameterized()
        {
            AssertObservableRewrite(
                Expression.Parameter(_elementType).Let(x =>
                    Expression.Lambda(
                        Expression.Invoke(
                            Expression.Parameter(_observableParamType, _fooId),
                            x
                        ),
                        x
                    )
                ),
                Expression.Parameter(typeof(IReactive), _thisId).Let(@this =>
                    Expression.Lambda(
                        Expression.Call(
                            @this,
                            _defineObservableParam.MakeGenericMethod(_elementType, _elementType),
                            Expression.Constant(_uri, typeof(Uri)),
                            Expression.Parameter(_elementType).Let(x =>
                                Expression.Lambda(
                                    Expression.Invoke(
                                        Expression.Call(
                                            @this,
                                            _getObservableParam.MakeGenericMethod(_elementType, _elementType),
                                            Expression.Constant(_fooUri)
                                        ),
                                        x
                                    ),
                                    x
                                )
                            ),
                            Expression.Constant(_state)
                        ),
                        @this
                    )
                )
            );
        }

        [TestMethod]
        public void UriToReactiveBinder_Observer()
        {
            AssertObserverRewrite(
                Expression.Parameter(_observerType, _fooId),
                Expression.Parameter(typeof(IReactive), _thisId).Let(@this =>
                    Expression.Lambda(
                        Expression.Call(
                            @this,
                            _defineObserver.MakeGenericMethod(_elementType),
                            Expression.Constant(_uri, typeof(Uri)),
                            Expression.Call(
                                @this,
                                _getObserver.MakeGenericMethod(_elementType),
                                Expression.Constant(_fooUri)
                            ),
                            Expression.Constant(_state)
                        ),
                        @this
                    )
                )
            );
        }

        [TestMethod]
        public void UriToReactiveBinder_ObserverParameterized()
        {
            AssertObserverRewrite(
                Expression.Parameter(_elementType).Let(x =>
                    Expression.Lambda(
                        Expression.Invoke(
                            Expression.Parameter(_observerParamType, _fooId),
                            x
                        ),
                        x
                    )
                ),
                Expression.Parameter(typeof(IReactive), _thisId).Let(@this =>
                    Expression.Lambda(
                        Expression.Call(
                            @this,
                            _defineObserverParam.MakeGenericMethod(_elementType, _elementType),
                            Expression.Constant(_uri, typeof(Uri)),
                            Expression.Parameter(_elementType).Let(x =>
                                Expression.Lambda(
                                    Expression.Invoke(
                                        Expression.Call(
                                            @this,
                                            _getObserverParam.MakeGenericMethod(_elementType, _elementType),
                                            Expression.Constant(_fooUri)
                                        ),
                                        x
                                    ),
                                    x
                                )
                            ),
                            Expression.Constant(_state)
                        ),
                        @this
                    )
                )
            );
        }

        [TestMethod]
        public void UriToReactiveBinder_Observer_NoUnbound()
        {
            AssertObserverRewrite(
                Expression.Default(_observerType),
                Expression.Parameter(typeof(IReactive), _thisId).Let(@this =>
                    Expression.Lambda(
                        Expression.Call(
                            @this,
                            _defineObserver.MakeGenericMethod(_elementType),
                            Expression.Constant(_uri, typeof(Uri)),
                            Expression.Default(_observerType),
                            Expression.Constant(_state)
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
                Expression.Parameter(typeof(IReactive), _thisId).Let(@this =>
                    Expression.Lambda(
                        Expression.Call(
                            @this,
                            _defineObservable.MakeGenericMethod(_elementType),
                            Expression.Constant(_uri, typeof(Uri)),
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
                            Expression.Constant(_state)
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
                Expression.Parameter(typeof(IReactive), _thisId).Let(@this =>
                    Expression.Lambda(
                        Expression.Call(
                            @this,
                            _defineObservable.MakeGenericMethod(_elementType),
                            Expression.Constant(_uri, typeof(Uri)),
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
                            Expression.Constant(_state)
                        ),
                        @this
                    )
                )
            );
        }

        [TestMethod]
        public void UriToReactiveBinder_LookupOther_ThrowsInvalidOperation()
        {
            var binder = new UriToReactiveBinder();
            Assert.ThrowsException<InvalidOperationException>(() =>
                binder.BindObservable(
                    Expression.Invoke(
                        Expression.Parameter(_observableOtherParamType, _fooId),
                        Expression.Parameter(_otherType, _barId)
                    ),
                    _uri,
                    _state
                )
            );
        }

        private static void AssertSubscriptionRewrite(Expression before, Expression after)
        {
            var rebinder = new UriToReactiveBinder();
            var comparer = new ExpressionEqualityComparer(() => new Comparator());
            var rewritten = rebinder.BindSubscription(before, _uri, _state);
            Assert.IsTrue(comparer.Equals(rewritten, after), string.Format(CultureInfo.InvariantCulture, "Expected: {0}, Actual: {1}", after, rewritten));
        }

        private static void AssertStreamRewrite(Expression before, Expression after)
        {
            var rebinder = new UriToReactiveBinder();
            var comparer = new ExpressionEqualityComparer(() => new Comparator());
            var rewritten = rebinder.BindSubject(before, _uri, _state);
            Assert.IsTrue(comparer.Equals(rewritten, after), string.Format(CultureInfo.InvariantCulture, "Expected: {0}, Actual: {1}", after, rewritten));
        }

        private static void AssertObservableRewrite(Expression before, Expression after)
        {
            var rebinder = new UriToReactiveBinder();
            var comparer = new ExpressionEqualityComparer(() => new Comparator());
            var rewritten = rebinder.BindObservable(before, _uri, _state);
            Assert.IsTrue(comparer.Equals(rewritten, after), string.Format(CultureInfo.InvariantCulture, "Expected: {0}, Actual: {1}", after, rewritten));
        }

        private static void AssertObserverRewrite(Expression before, Expression after)
        {
            var rebinder = new UriToReactiveBinder();
            var comparer = new ExpressionEqualityComparer(() => new Comparator());
            var rewritten = rebinder.BindObserver(before, _uri, _state);
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
