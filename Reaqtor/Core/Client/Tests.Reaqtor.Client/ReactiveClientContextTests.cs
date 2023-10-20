// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - July 2013 - Created this file.
// ER - October 2013 - Created this file.
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.CompilerServices;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Reaqtor;
using Reaqtor.Metadata;
using Reaqtor.TestingFramework;

namespace Tests.Reaqtor.Client
{
    [TestClass]
    public partial class ReactiveClientContextTests : ReactiveClientContextTestBase
    {
        [TestMethod]
        public void ReactiveClientContext_Qbservable_ArgumentChecking()
        {
            Apply(
                ctx =>
                {
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.GetObservable<int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.GetObservable<int, int>(null));

                    var o = ctx.GetObservable<int>(new Uri(Constants.Observable.XS));
                    var provider = o.Provider;

                    Assert.ThrowsException<ArgumentNullException>(() => ((IAsyncReactiveObservable<int>)o).SubscribeAsync(null, new Uri(Constants.Subscription.SUB), null));
                    Assert.ThrowsException<ArgumentNullException>(() => ((IAsyncReactiveObservable<int>)o).SubscribeAsync(null, new Uri(Constants.Subscription.SUB), null, CancellationToken.None));

#pragma warning disable IDE0004 // Remove Unnecessary Cast. (Makes the intent clear.)
                    Assert.ThrowsException<ArgumentNullException>(() => ((IAsyncReactiveObservable<int>)o).SubscribeAsync(((IAsyncReactiveObserver<int>)ctx.GetObserver<int>(new Uri(Constants.Observer.OB))), null, null));
                    Assert.ThrowsException<ArgumentNullException>(() => ((IAsyncReactiveObservable<int>)o).SubscribeAsync(((IAsyncReactiveObserver<int>)ctx.GetObserver<int>(new Uri(Constants.Observer.OB))), null, null, CancellationToken.None));
#pragma warning restore IDE0004

                    Assert.ThrowsException<ArgumentNullException>(() => provider.CreateQbservable<int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => provider.CreateQbservable<int, int>(null));
                }
            );
        }

        [TestMethod]
        public void ReactiveClientContext_Qbserver_ArgumentChecking()
        {
            Apply(
                ctx =>
                {
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.GetObserver<int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.GetObserver<int, int>(null));

                    var observer = ctx.GetObserver<int>(new Uri(Constants.Observer.OB));

                    Assert.ThrowsException<ArgumentNullException>(() => observer.OnErrorAsync(null));
                    Assert.ThrowsException<ArgumentNullException>(() => observer.OnErrorAsync(null, CancellationToken.None));
                }
            );
        }

        [TestMethod]
        public void ReactiveClientContext_Qubscription_ArgumentChecking()
        {
#if !NET6_0
            Assert.ThrowsException<ArgumentNullException>(() => ((IAsyncReactiveQubscription)null).DisposeAsync());
#endif
            Assert.ThrowsException<ArgumentNullException>(() => ((IAsyncReactiveQubscription)null).AsDisposable());
        }

        [TestMethod]
        public void ReactiveClientContext_QubjectFactory_ArgumentChecking()
        {
            Apply(
                ctx =>
                {
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.GetStreamFactory<int, int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.GetStreamFactory<int, int, int>(null));

                    var factory = ctx.GetStreamFactory<int, int>(new Uri(Constants.StreamFactory.SF));
                    var pfactory = ctx.GetStreamFactory<string, int, int>(new Uri(Constants.StreamFactory.SG));

                    Assert.ThrowsException<ArgumentNullException>(() => factory.CreateAsync(null, null));
                    Assert.ThrowsException<ArgumentNullException>(() => ((IAsyncReactiveSubjectFactory<int, int>)factory).CreateAsync(null, null));
                    Assert.ThrowsException<ArgumentNullException>(() => ((IAsyncReactiveSubjectFactory<int, int>)factory).CreateAsync(null, null, CancellationToken.None));

                    Assert.ThrowsException<ArgumentNullException>(() => pfactory.CreateAsync(null, "factory_parameter_1", null));
                    Assert.ThrowsException<ArgumentNullException>(() => ((IAsyncReactiveSubjectFactory<int, int, string>)pfactory).CreateAsync(null, "factory_parameter_1", null));
                    Assert.ThrowsException<ArgumentNullException>(() => ((IAsyncReactiveSubjectFactory<int, int, string>)pfactory).CreateAsync(null, "factory_parameter_1", null, CancellationToken.None));
                }
            );
        }

        [TestMethod]
        public void ReactiveClientContext_QubscriptionFactory_ArgumentChecking()
        {
            Apply(
                ctx =>
                {
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.GetSubscriptionFactory<int, int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.GetSubscriptionFactory<int, int, int>(null));

                    var factory = ctx.GetSubscriptionFactory(new Uri(Constants.SubscriptionFactory.SF));
                    var pfactory = ctx.GetSubscriptionFactory<string>(new Uri(Constants.SubscriptionFactory.SG));

                    Assert.ThrowsException<ArgumentNullException>(() => factory.CreateAsync(null, null));
                    Assert.ThrowsException<ArgumentNullException>(() => ((IAsyncReactiveSubscriptionFactory)factory).CreateAsync(null, null));
                    Assert.ThrowsException<ArgumentNullException>(() => ((IAsyncReactiveSubscriptionFactory)factory).CreateAsync(null, null, CancellationToken.None));

                    Assert.ThrowsException<ArgumentNullException>(() => pfactory.CreateAsync(null, "factory_parameter_1", null));
                    Assert.ThrowsException<ArgumentNullException>(() => ((IAsyncReactiveSubscriptionFactory<string>)pfactory).CreateAsync(null, "factory_parameter_1", null));
                    Assert.ThrowsException<ArgumentNullException>(() => ((IAsyncReactiveSubscriptionFactory<string>)pfactory).CreateAsync(null, "factory_parameter_1", null, CancellationToken.None));
                }
            );
        }

        [TestMethod]
        public void ReactiveClientContext_AsyncReactiveQueryProvider_ArgumentChecking_CreateQbservable()
        {
            Apply(
                ctx =>
                {
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.Provider.CreateQbservable<int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.Provider.CreateQbservable<int, int>(null));
                }
            );
        }

        [TestMethod]
        public void ReactiveClientContext_AsyncReactiveQueryProvider_ArgumentChecking_CreateQbserver()
        {
            Apply(
                ctx =>
                {
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.Provider.CreateQbserver<int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.Provider.CreateQbserver<int, int>(null));
                }
            );
        }

        [TestMethod]
        public void ReactiveClientContext_AsyncReactiveQueryProvider_ArgumentChecking_CreateQubject()
        {
            Apply(
                ctx =>
                {
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.Provider.CreateQubject<int, int>(null));
                }
            );
        }

        [TestMethod]
        public void ReactiveClientContext_AsyncReactiveQueryProvider_ArgumentChecking_CreateQubjectFactory()
        {
            Apply(
                ctx =>
                {
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.Provider.CreateQubjectFactory<int, int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.Provider.CreateQubjectFactory<int, int, int>(null));
                }
            );
        }

        [TestMethod]
        public void ReactiveClientContext_AsyncReactiveQueryProvider_ArgumentChecking_CreateQubscriptionFactory()
        {
            Apply(
                ctx =>
                {
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.Provider.CreateQubscriptionFactory(null));
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.Provider.CreateQubscriptionFactory<int>(null));
                }
            );
        }

        [TestMethod]
        public void ReactiveClientContext_AsyncReactiveQueryProvider_ArgumentChecking_CreateQubscription()
        {
            Apply(
                ctx =>
                {
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.Provider.CreateQubscription(null));
                }
            );
        }

        [TestMethod]
        public void ReactiveClientContext_ReactiveClientProxy_ArgumentChecking_GetObservable()
        {
            Apply(
                ctx =>
                {
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.GetObservable<int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.GetObservable<int, int>(null));
                }
            );
        }

        [TestMethod]
        public void ReactiveClientContext_ReactiveClientProxy_ArgumentChecking_GetObserver()
        {
            Apply(
                ctx =>
                {
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.GetObserver<int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.GetObserver<int, int>(null));
                }
            );
        }

        [TestMethod]
        public void ReactiveClientContext_ReactiveClientProxy_ArgumentChecking_GetSubscription()
        {
            Apply(
                ctx =>
                {
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.GetSubscription(null));
                }
            );
        }

        [TestMethod]
        public void ReactiveClientContext_ReactiveClientProxy_ArgumentChecking_GetStream()
        {
            Apply(
                ctx =>
                {
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.GetStream<int, int>(null));
                }
            );
        }

        [TestMethod]
        public void ReactiveClientContext_ReactiveClientProxy_ArgumentChecking_GetStreamFactory()
        {
            Apply(
                ctx =>
                {
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.GetStreamFactory<int, int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.GetStreamFactory<int, int, int>(null));
                }
            );
        }

        [TestMethod]
        public void ReactiveClientContext_ReactiveClientProxy_ArgumentChecking_GetSubscriptionFactory()
        {
            Apply(
                ctx =>
                {
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.GetSubscriptionFactory(null));
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.GetSubscriptionFactory<int>(null));
                }
            );
        }

        [TestMethod]
        public void ReactiveClientContext_ReactiveDefinitionProxy_ArgumentChecking_DefineObservable()
        {
            Apply(
                ctx =>
                {
                    var dummyUri = new Uri("io:/dummy");
                    var dummyQbservable = ctx.Provider.CreateQbservable<int>(Expression.Default(typeof(IAsyncReactiveQbservable<int>)));
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.DefineObservableAsync<int>(null, dummyQbservable, null, CancellationToken.None));
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.DefineObservableAsync<int>(dummyUri, null, null, CancellationToken.None));
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.DefineObservableAsync<int, int>(null, _ => dummyQbservable, null, CancellationToken.None));
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.DefineObservableAsync<int, int>(dummyUri, null, null, CancellationToken.None));
                }
            );
        }

        [TestMethod]
        public void ReactiveClientContext_ReactiveDefinitionProxy_ArgumentChecking_DefineObserver()
        {
            Apply(
                ctx =>
                {
                    var dummyUri = new Uri("iv:/dummy");
                    var dummyQbserver = ctx.Provider.CreateQbserver<int>(Expression.Default(typeof(IAsyncReactiveQbserver<int>)));
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.DefineObserverAsync<int>(null, dummyQbserver, null, CancellationToken.None));
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.DefineObserverAsync<int>(dummyUri, null, null, CancellationToken.None));
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.DefineObserverAsync<int, int>(null, _ => dummyQbserver, null, CancellationToken.None));
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.DefineObserverAsync<int, int>(dummyUri, null, null, CancellationToken.None));
                }
            );
        }

        [TestMethod]
        public void ReactiveClientContext_ReactiveDefinitionProxy_ArgumentChecking_DefineStreamFactory()
        {
            Apply(
                ctx =>
                {
                    var dummyUri = new Uri("sf:/dummy");
                    var dummyStreamFactory = ctx.Provider.CreateQubjectFactory<int, int>(Expression.Default(typeof(IAsyncReactiveQubjectFactory<int, int>)));
                    var paramDummyStreamFactory = ctx.Provider.CreateQubjectFactory<int, int, int>(Expression.Default(typeof(IAsyncReactiveQubjectFactory<int, int, int>)));
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.DefineStreamFactoryAsync<int, int>(null, dummyStreamFactory, null, CancellationToken.None));
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.DefineStreamFactoryAsync<int, int>(dummyUri, null, null, CancellationToken.None));
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.DefineStreamFactoryAsync<int, int, int>(null, paramDummyStreamFactory, null, CancellationToken.None));
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.DefineStreamFactoryAsync<int, int, int>(dummyUri, null, null, CancellationToken.None));
                }
            );
        }

        [TestMethod]
        public void ReactiveClientContext_ReactiveDefinitionProxy_ArgumentChecking_DefineSubscriptionFactory()
        {
            Apply(
                ctx =>
                {
                    var dummyUri = new Uri("sf:/dummy");
                    var dummyStreamFactory = ctx.Provider.CreateQubscriptionFactory(Expression.Default(typeof(IAsyncReactiveQubscriptionFactory)));
                    var paramDummyStreamFactory = ctx.Provider.CreateQubscriptionFactory<int>(Expression.Default(typeof(IAsyncReactiveQubscriptionFactory<int>)));
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.DefineSubscriptionFactoryAsync(null, dummyStreamFactory, null, CancellationToken.None));
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.DefineSubscriptionFactoryAsync(dummyUri, null, null, CancellationToken.None));
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.DefineSubscriptionFactoryAsync<int>(null, paramDummyStreamFactory, null, CancellationToken.None));
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.DefineSubscriptionFactoryAsync<int>(dummyUri, default(IAsyncReactiveQubscriptionFactory<int>), null, CancellationToken.None));
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.DefineSubscriptionFactoryAsync<int>(dummyUri, default(Expression<Func<int, IAsyncReactiveQubscription>>), null, CancellationToken.None));
                }
            );
        }

        [TestMethod]
        public void ReactiveClientContext_ReactiveDefinitionProxy_ArgumentChecking_UndefineObservable()
        {
            Apply(
                ctx =>
                {
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.UndefineObservableAsync(null, CancellationToken.None));
                }
            );
        }

        [TestMethod]
        public void ReactiveClientContext_ReactiveDefinitionProxy_ArgumentChecking_UndefineObserver()
        {
            Apply(
                ctx =>
                {
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.UndefineObserverAsync(null, CancellationToken.None));
                }
            );
        }

        [TestMethod]
        public void ReactiveClientContext_ReactiveDefinitionProxy_ArgumentChecking_UndefineStreamFactory()
        {
            Apply(
                ctx =>
                {
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.UndefineStreamFactoryAsync(null, CancellationToken.None));
                }
            );
        }

        [TestMethod]
        public void ReactiveClientContext_ReactiveDefinitionProxy_ArgumentChecking_UndefineSubscriptionFactory()
        {
            Apply(
                ctx =>
                {
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.UndefineSubscriptionFactoryAsync(null, CancellationToken.None));
                }
            );
        }

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_Simple1()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<int>(new Uri(Constants.Observable.XS));
                    Assert.AreEqual(typeof(int), xs.ElementType);

                    var ob = ctx.GetObserver<int>(new Uri(Constants.Observer.OB));
                    Assert.AreEqual(typeof(int), xs.ElementType);

                    xs.SubscribeAsync(ob, new Uri(Constants.Subscription.SUB1), null).Wait();
                    ((IAsyncReactiveObservable<int>)xs).SubscribeAsync(ob, new Uri(Constants.Subscription.SUB2), null).Wait();
                    ((IAsyncReactiveObservable<int>)xs).SubscribeAsync(ob, new Uri(Constants.Subscription.SUB3), null, CancellationToken.None).Wait();
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB1),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, IAsyncReactiveQbserver<int>, Task<IAsyncReactiveQubscription>>), Constants.SubscribeUri),
                        Expression.Parameter(typeof(IAsyncReactiveQbservable<int>), Constants.Observable.XS),
                        Expression.Parameter(typeof(IAsyncReactiveQbserver<int>), Constants.Observer.OB)
                    ),
                    null
                ),
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB2),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, IAsyncReactiveQbserver<int>, Task<IAsyncReactiveQubscription>>), Constants.SubscribeUri),
                        Expression.Parameter(typeof(IAsyncReactiveQbservable<int>), Constants.Observable.XS),
                        Expression.Parameter(typeof(IAsyncReactiveQbserver<int>), Constants.Observer.OB)
                    ),
                    null
                ),
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB3),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, IAsyncReactiveQbserver<int>, Task<IAsyncReactiveQubscription>>), Constants.SubscribeUri),
                        Expression.Parameter(typeof(IAsyncReactiveQbservable<int>), Constants.Observable.XS),
                        Expression.Parameter(typeof(IAsyncReactiveQbserver<int>), Constants.Observer.OB)
                    ),
                    null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_Simple2()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<int>(new Uri(Constants.Observable.XS));

                    var ys = from x in xs
                             where x > 0
                             select x * x;

                    var ob = ctx.GetObserver<int>(new Uri(Constants.Observer.OB));
                    ys.SubscribeAsync(ob, new Uri(Constants.Subscription.SUB), null).Wait();
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Parameter(typeof(int), "x").Let(x =>
                        Expression.Invoke(
                            Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, IAsyncReactiveQbserver<int>, Task<IAsyncReactiveQubscription>>), Constants.SubscribeUri),
                            Expression.Invoke(
                                Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, Expression<Func<int, int>>, IAsyncReactiveQbservable<int>>), Constants.Observable.Select),
                                Expression.Invoke(
                                    Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, Expression<Func<int, bool>>, IAsyncReactiveQbservable<int>>), Constants.Observable.Where),
                                    Expression.Parameter(typeof(IAsyncReactiveQbservable<int>), Constants.Observable.XS),
                                    Expression.Lambda<Func<int, bool>>(
                                        Expression.GreaterThan(x, Expression.Constant(0)),
                                        x
                                    )
                                ),
                                Expression.Lambda<Func<int, int>>(
                                    Expression.Multiply(x, x),
                                    x
                                )
                            ),
                            Expression.Parameter(typeof(IAsyncReactiveQbserver<int>), Constants.Observer.OB)
                        )
                    ),
                    null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_Simple4()
        {
            var longParam = Expression.Parameter(typeof(long), "dueTime");

            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<long>(new Uri(Constants.Observable.XS));

                    var ys = xs.SelectMany(dueTime => ctx.Timer(new TimeSpan(dueTime)));

                    var ob = ctx.GetObserver<long>(new Uri(Constants.Observer.OB));
                    ys.SubscribeAsync(ob, new Uri(Constants.Subscription.SUB), null).Wait();
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<long>, IAsyncReactiveQbserver<long>, Task<IAsyncReactiveQubscription>>), Constants.SubscribeUri),
                        Expression.Invoke(
                            Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<long>, Expression<Func<long, IAsyncReactiveQbservable<long>>>, IAsyncReactiveQbservable<long>>), Constants.Observable.Bind),
                            Expression.Parameter(typeof(IAsyncReactiveQbservable<long>), Constants.Observable.XS),
                            Expression.Lambda<Func<long, IAsyncReactiveQbservable<long>>>(
                                Expression.Invoke(
                                    Expression.Parameter(typeof(Func<TimeSpan, IAsyncReactiveQbservable<long>>), Constants.Observable.Timer),
                                    Expression.New(typeof(TimeSpan).GetConstructor(new[] { typeof(long) }), longParam)
                                ),
                                longParam
                            )
                        ),
                        Expression.Parameter(typeof(IAsyncReactiveQbserver<long>), Constants.Observer.OB)
                    ),
                    null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_Simple3()
        {
            Apply(
                provider => new MyContext(provider),
                ctx =>
                {
                    var xs = ctx.Xs;
                    var ob = ctx.GetObserver<int>(new Uri(Constants.Observer.OB));
                    xs.SubscribeAsync(ob, new Uri(Constants.Subscription.SUB), null).Wait();
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, IAsyncReactiveQbserver<int>, Task<IAsyncReactiveQubscription>>), Constants.SubscribeUri),
                        Expression.Parameter(typeof(IAsyncReactiveQbservable<int>), Constants.Observable.XS),
                        Expression.Parameter(typeof(IAsyncReactiveQbserver<int>), Constants.Observer.OB)
                    ),
                    null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_NAry_1()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<int, string>(new Uri(Constants.Observable.XS));
                    var ob = ctx.GetObserver<int, string>(new Uri(Constants.Observer.OB));
                    xs(1).SubscribeAsync(ob(-1), new Uri(Constants.Subscription.SUB), null).Wait();
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<string>, IAsyncReactiveQbserver<string>, Task<IAsyncReactiveQubscription>>), Constants.SubscribeUri),
                        Expression.Invoke(
                            Expression.Parameter(typeof(Func<int, IAsyncReactiveQbservable<string>>), Constants.Observable.XS),
                            Expression.Constant(1)
                        ),
                        Expression.Invoke(
                            Expression.Parameter(typeof(Func<int, IAsyncReactiveQbserver<string>>), Constants.Observer.OB),
                            Expression.Constant(-1)
                        )
                    ),
                    null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_NAry_2()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<int, bool, string>(new Uri(Constants.Observable.XS));
                    var ob = ctx.GetObserver<bool, int, string>(new Uri(Constants.Observer.OB));
                    xs(1, true).SubscribeAsync(ob(false, -1), new Uri(Constants.Subscription.SUB), null).Wait();
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<string>, IAsyncReactiveQbserver<string>, Task<IAsyncReactiveQubscription>>), Constants.SubscribeUri),
                        Expression.Invoke(
                            Expression.Parameter(typeof(Func<int, bool, IAsyncReactiveQbservable<string>>), Constants.Observable.XS),
                            Expression.Constant(1),
                            Expression.Constant(true)
                        ),
                        Expression.Invoke(
                            Expression.Parameter(typeof(Func<bool, int, IAsyncReactiveQbserver<string>>), Constants.Observer.OB),
                            Expression.Constant(false),
                            Expression.Constant(-1)
                        )
                    ),
                    null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_Closure1()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<int>(new Uri(Constants.Observable.XS));

                    var a = -1;

                    var ys = from x in xs
                             where x > a
                             select x * x;

                    var ob = ctx.GetObserver<int>(new Uri(Constants.Observer.OB));

                    for (var i = 0; i < 3; i++)
                    {
                        a = i;
                        ys.SubscribeAsync(ob, new Uri(Constants.Subscription.SUB), null).Wait();
                    }
                },
                Enumerable.Range(0, 3).Select(i =>
                    new CreateSubscription(
                        new Uri(Constants.Subscription.SUB),
                        Expression.Parameter(typeof(int), "x").Let(x =>
                            Expression.Invoke(
                                Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, IAsyncReactiveQbserver<int>, Task<IAsyncReactiveQubscription>>), Constants.SubscribeUri),
                                Expression.Invoke(
                                    Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, Expression<Func<int, int>>, IAsyncReactiveQbservable<int>>), Constants.Observable.Select),
                                    Expression.Invoke(
                                        Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, Expression<Func<int, bool>>, IAsyncReactiveQbservable<int>>), Constants.Observable.Where),
                                        Expression.Parameter(typeof(IAsyncReactiveQbservable<int>), Constants.Observable.XS),
                                        Expression.Lambda<Func<int, bool>>(
                                            Expression.GreaterThan(x, Expression.Constant(i)),
                                            x
                                        )
                                    ),
                                    Expression.Lambda<Func<int, int>>(
                                        Expression.Multiply(x, x),
                                        x
                                    )
                                ),
                                Expression.Parameter(typeof(IAsyncReactiveQbserver<int>), Constants.Observer.OB)
                            )
                        ),
                        null
                    )
                ).ToArray()
            );
        }

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_Parameterized()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<string, int>(new Uri(Constants.Observable.XS));

                    var ys = from x in xs("bar")
                             where x > 0
                             select x * x;

                    var ob = ctx.GetObserver<int>(new Uri(Constants.Observer.OB));
                    ys.SubscribeAsync(ob, new Uri(Constants.Subscription.SUB), null).Wait();
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Parameter(typeof(int), "x").Let(x =>
                        Expression.Invoke(
                            Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, IAsyncReactiveQbserver<int>, Task<IAsyncReactiveQubscription>>), Constants.SubscribeUri),
                            Expression.Invoke(
                                Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, Expression<Func<int, int>>, IAsyncReactiveQbservable<int>>), Constants.Observable.Select),
                                Expression.Invoke(
                                    Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, Expression<Func<int, bool>>, IAsyncReactiveQbservable<int>>), Constants.Observable.Where),
                                    Expression.Invoke(
                                        Expression.Parameter(typeof(Func<string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS),
                                        Expression.Constant("bar")
                                    ),
                                    Expression.Lambda<Func<int, bool>>(
                                        Expression.GreaterThan(x, Expression.Constant(0)),
                                        x
                                    )
                                ),
                                Expression.Lambda<Func<int, int>>(
                                    Expression.Multiply(x, x),
                                    x
                                )
                            ),
                            Expression.Parameter(typeof(IAsyncReactiveQbserver<int>), Constants.Observer.OB)
                        )
                    ),
                    null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_SelectMany1_Simple()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<int>(new Uri(Constants.Observable.XS));
                    var ys = ctx.GetObservable<int, string>(new Uri(Constants.Observable.YS));

                    var zs = from x in xs
                             from y in ys(x)
                             select x + y.Length;

                    var ob = ctx.GetObserver<int>(new Uri(Constants.Observer.OB));
                    zs.SubscribeAsync(ob, new Uri(Constants.Subscription.SUB), null).Wait();
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Parameter(typeof(int), "x").Let(x =>
                        Expression.Parameter(typeof(string), "y").Let(y =>
                            Expression.Invoke(
                                Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, IAsyncReactiveQbserver<int>, Task<IAsyncReactiveQubscription>>), Constants.SubscribeUri),
                                Expression.Invoke(
                                    Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, Expression<Func<int, IAsyncReactiveQbservable<string>>>, Expression<Func<int, string, int>>, IAsyncReactiveQbservable<int>>), Constants.Observable.SelectMany),
                                    Expression.Parameter(typeof(IAsyncReactiveQbservable<int>), Constants.Observable.XS),
                                    Expression.Lambda(
                                        Expression.Invoke(
                                            Expression.Parameter(typeof(Func<int, IAsyncReactiveQbservable<string>>), Constants.Observable.YS),
                                            x
                                        ),
                                        x
                                    ),
                                    Expression.Lambda(
                                        Expression.Add(
                                            x,
                                            Expression.Property(y, "Length")
                                        ),
                                        x, y
                                    )
                                ),
                                Expression.Parameter(typeof(IAsyncReactiveQbserver<int>), Constants.Observer.OB)
                            )
                        )
                    ),
                    null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_SelectMany2_Closure()
        {
            var xs = default(IAsyncReactiveQbservable<int>);
            var ys = default(Func<int, IAsyncReactiveQbservable<string>>);

            Apply(
                ctx =>
                {
                    xs = ctx.GetObservable<int>(new Uri(Constants.Observable.XS));
                    ys = ctx.GetObservable<int, string>(new Uri(Constants.Observable.YS));

                    var zs = from x in xs
                             from y in ys(x)
                             select x + y.Length;

                    var ob = ctx.GetObserver<int>(new Uri(Constants.Observer.OB));
                    zs.SubscribeAsync(ob, new Uri(Constants.Subscription.SUB), null).Wait();
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Parameter(typeof(int), "x").Let(x =>
                        Expression.Parameter(typeof(string), "y").Let(y =>
                            Expression.Invoke(
                                Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, IAsyncReactiveQbserver<int>, Task<IAsyncReactiveQubscription>>), Constants.SubscribeUri),
                                Expression.Invoke(
                                    Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, Expression<Func<int, IAsyncReactiveQbservable<string>>>, Expression<Func<int, string, int>>, IAsyncReactiveQbservable<int>>), Constants.Observable.SelectMany),
                                    Expression.Parameter(typeof(IAsyncReactiveQbservable<int>), Constants.Observable.XS),
                                    Expression.Lambda(
                                        Expression.Invoke(
                                            Expression.Parameter(typeof(Func<int, IAsyncReactiveQbservable<string>>), Constants.Observable.YS),
                                            x
                                        ),
                                        x
                                    ),
                                    Expression.Lambda(
                                        Expression.Add(
                                            x,
                                            Expression.Property(y, "Length")
                                        ),
                                        x, y
                                    )
                                ),
                                Expression.Parameter(typeof(IAsyncReactiveQbserver<int>), Constants.Observer.OB)
                            )
                        )
                    ),
                    null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_SelectMany3_CustomContext()
        {
            Apply(
                provider => new MyContext(provider),
                ctx =>
                {
                    var zs = from x1 in ctx.Xs
                             from x2 in ctx.Xs
                             select x1 + x2;

                    var ob = ctx.GetObserver<int>(new Uri(Constants.Observer.OB));
                    zs.SubscribeAsync(ob, new Uri(Constants.Subscription.SUB), null).Wait();
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Parameter(typeof(int), "x1").Let(x1 =>
                        Expression.Parameter(typeof(int), "x2").Let(x2 =>
                            Expression.Invoke(
                                Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, IAsyncReactiveQbserver<int>, Task<IAsyncReactiveQubscription>>), Constants.SubscribeUri),
                                Expression.Invoke(
                                    Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, Expression<Func<int, IAsyncReactiveQbservable<int>>>, Expression<Func<int, int, int>>, IAsyncReactiveQbservable<int>>), Constants.Observable.SelectMany),
                                    Expression.Parameter(typeof(IAsyncReactiveQbservable<int>), Constants.Observable.XS),
                                    Expression.Lambda(
                                        Expression.Parameter(typeof(IAsyncReactiveQbservable<int>), Constants.Observable.XS),
                                        x1
                                    ),
                                    Expression.Lambda(
                                        Expression.Add(
                                            x1,
                                            x2
                                        ),
                                        x1, x2
                                    )
                                ),
                                Expression.Parameter(typeof(IAsyncReactiveQbserver<int>), Constants.Observer.OB)
                            )
                        )
                    ),
                    null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_SelectMany2_Inlined()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<int>(new Uri(Constants.Observable.XS));

                    var zs = from x in xs
                             from y in ctx.GetObservable<int, string>(new Uri(Constants.Observable.YS))(x)
                             select x + y.Length;

                    var ob = ctx.GetObserver<int>(new Uri(Constants.Observer.OB));
                    zs.SubscribeAsync(ob, new Uri(Constants.Subscription.SUB), null).Wait();
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Parameter(typeof(int), "x").Let(x =>
                        Expression.Parameter(typeof(string), "y").Let(y =>
                            Expression.Invoke(
                                Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, IAsyncReactiveQbserver<int>, Task<IAsyncReactiveQubscription>>), Constants.SubscribeUri),
                                Expression.Invoke(
                                    Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, Expression<Func<int, IAsyncReactiveQbservable<string>>>, Expression<Func<int, string, int>>, IAsyncReactiveQbservable<int>>), Constants.Observable.SelectMany),
                                    Expression.Parameter(typeof(IAsyncReactiveQbservable<int>), Constants.Observable.XS),
                                    Expression.Lambda(
                                        Expression.Invoke(
                                            Expression.Parameter(typeof(Func<int, IAsyncReactiveQbservable<string>>), Constants.Observable.YS),
                                            x
                                        ),
                                        x
                                    ),
                                    Expression.Lambda(
                                        Expression.Add(
                                            x,
                                            Expression.Property(y, "Length")
                                        ),
                                        x, y
                                    )
                                ),
                                Expression.Parameter(typeof(IAsyncReactiveQbserver<int>), Constants.Observer.OB)
                            )
                        )
                    ),
                    null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_GetSubscription()
        {
            Apply(
                ctx =>
                {
                    var s = ctx.GetSubscription(new Uri(Constants.Subscription.SUB1));
                    var s1 = ctx.GetSubscription(new Uri(Constants.Subscription.SUB2));
                    var s2 = ctx.GetSubscription(new Uri(Constants.Subscription.SUB3));

#if NET6_0 // Suppresses CA2012
                    s.DisposeAsync(CancellationToken.None).AsTask();
                    s1.DisposeAsync().AsTask();
#else
                    s.DisposeAsync(CancellationToken.None);
                    s1.DisposeAsync();
#endif
                    s2.AsDisposable().Dispose();
                },
                new DeleteSubscription(new Uri(Constants.Subscription.SUB1)),
                new DeleteSubscription(new Uri(Constants.Subscription.SUB2)),
                new DeleteSubscription(new Uri(Constants.Subscription.SUB3))
            );
        }

        [TestMethod]
        public void ReactiveClientContext_GetStreamFactory()
        {
            var createStreamExpression =
                Expression.Invoke(
                    Expression.Parameter(typeof(Func<IAsyncReactiveQubject<int, int>>), Constants.StreamFactory.SF)
                );

            Apply(
                ctx =>
                {
                    var factory = ctx.GetStreamFactory<int, int>(new Uri(Constants.StreamFactory.SF));

                    var qubject = factory.CreateAsync(new Uri(Constants.Stream.FOO), null).Result;
                    Assert.AreEqual(typeof(int), ((IAsyncReactiveQbservable<int>)qubject).ElementType);
                    Assert.AreEqual(typeof(int), ((IAsyncReactiveQbserver<int>)qubject).ElementType);

                    factory.CreateAsync(new Uri(Constants.Stream.BAR), null, CancellationToken.None).Wait();
                    ((IAsyncReactiveSubjectFactory<int, int>)factory).CreateAsync(new Uri(Constants.Stream.QUX), null).Wait();
                    qubject.SubscribeAsync(ctx.GetObserver<int>(new Uri(Constants.Observer.OB)), new Uri(Constants.Subscription.SUB), null, CancellationToken.None);

                    ctx.GetStream<int, int>(new Uri(Constants.Stream.FOO)).DisposeAsync()
#if NET6_0
                        .AsTask()
#endif
                        .Wait();
                },
                new CreateStream(new Uri(Constants.Stream.FOO), createStreamExpression, null),
                new CreateStream(new Uri(Constants.Stream.BAR), createStreamExpression, null),
                new CreateStream(new Uri(Constants.Stream.QUX), createStreamExpression, null),
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, IAsyncReactiveQbserver<int>, Task<IAsyncReactiveQubscription>>), Constants.SubscribeUri),
                        createStreamExpression, // BUG
                        Expression.Parameter(typeof(IAsyncReactiveQbserver<int>), Constants.Observer.OB)
                    ),
                    null
                ),
                new DeleteStream(new Uri(Constants.Stream.FOO))
            );
        }

        [TestMethod]
        public void ReactiveClientContext_GetStreamFactory_Parameterized()
        {
            var createStreamExpression =
                Expression.Invoke(
                    Expression.Parameter(typeof(Func<string, IAsyncReactiveQubject<int, int>>), Constants.StreamFactory.SF),
                    Expression.Constant("factory_parameter_1")
                );

            Apply(
                ctx =>
                {
                    var factory = ctx.GetStreamFactory<string, int, int>(new Uri(Constants.StreamFactory.SF));

                    var qubject = factory.CreateAsync(new Uri(Constants.Stream.FOO), "factory_parameter_1", null).Result;
                    factory.CreateAsync(new Uri(Constants.Stream.BAR), "factory_parameter_1", null, CancellationToken.None).Wait();
                    ((IAsyncReactiveSubjectFactory<int, int, string>)factory).CreateAsync(new Uri(Constants.Stream.QUX), "factory_parameter_1", null).Wait();

                    ((IAsyncReactiveSubject<int, int>)qubject).SubscribeAsync(ctx.GetObserver<int>(new Uri(Constants.Observer.OB)), new Uri(Constants.Subscription.SUB), null);
                },
                new CreateStream(new Uri(Constants.Stream.FOO), createStreamExpression, null),
                new CreateStream(new Uri(Constants.Stream.BAR), createStreamExpression, null),
                new CreateStream(new Uri(Constants.Stream.QUX), createStreamExpression, null),
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, IAsyncReactiveQbserver<int>, Task<IAsyncReactiveQubscription>>), Constants.SubscribeUri),
                        createStreamExpression, // BUG
                        Expression.Parameter(typeof(IAsyncReactiveQbserver<int>), Constants.Observer.OB)
                    ),
                    null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_GetSubscriptionFactory()
        {
            var createSubscriptionExpression =
                Expression.Invoke(
                    Expression.Parameter(typeof(Func<IAsyncReactiveQubscription>), Constants.SubscriptionFactory.SF)
                );

            Apply(
                ctx =>
                {
                    var factory = ctx.GetSubscriptionFactory(new Uri(Constants.SubscriptionFactory.SF));

                    factory.CreateAsync(new Uri(Constants.Subscription.SUB1), null).Wait();

                    ctx.GetSubscription(new Uri(Constants.Subscription.SUB1)).DisposeAsync()
#if NET6_0
                        .AsTask()
#endif
                        .Wait();
                },
                new CreateSubscription(new Uri(Constants.Subscription.SUB1), createSubscriptionExpression, null),
                new DeleteSubscription(new Uri(Constants.Subscription.SUB1))
            );
        }

        [TestMethod]
        public void ReactiveClientContext_GetSubscriptionFactory_Parameterized()
        {
            var createSubscriptionExpression =
                Expression.Invoke(
                    Expression.Parameter(typeof(Func<string, IAsyncReactiveQubscription>), Constants.SubscriptionFactory.SF),
                    Expression.Constant("factory_parameter_1")
                );

            Apply(
                ctx =>
                {
                    var factory = ctx.GetSubscriptionFactory<string>(new Uri(Constants.SubscriptionFactory.SF));

                    factory.CreateAsync(new Uri(Constants.Subscription.SUB1), "factory_parameter_1", null).Wait();
                },
                new CreateSubscription(new Uri(Constants.Subscription.SUB1), createSubscriptionExpression, null)
            );
        }

        [TestMethod]
        public void ReactiveClientContext_GetObserver()
        {
            var ex = new Exception();

            Apply(
                ctx =>
                {
                    var observer = ctx.GetObserver<int>(new Uri(Constants.Observer.OB));
                    observer.OnNextAsync(42, CancellationToken.None);
                    observer.OnNextAsync(76);
                    observer.OnErrorAsync(ex, CancellationToken.None);
                    observer = ctx.GetObserver<int>(new Uri(Constants.Observer.OB1));
                    observer.OnErrorAsync(ex);
                    observer = ctx.GetObserver<int>(new Uri(Constants.Observer.OB2));
                    observer.OnCompletedAsync(CancellationToken.None);
                    observer = ctx.GetObserver<int>(new Uri(Constants.Observer.OB3));
                    observer.OnCompletedAsync();
                },
                new ObserverOnNext(new Uri(Constants.Observer.OB), 42),
                new ObserverOnNext(new Uri(Constants.Observer.OB), 76),
                new ObserverOnError(new Uri(Constants.Observer.OB), ex),
                new ObserverOnError(new Uri(Constants.Observer.OB1), ex),
                new ObserverOnCompleted(new Uri(Constants.Observer.OB2)),
                new ObserverOnCompleted(new Uri(Constants.Observer.OB3))
            );
        }

        [TestMethod]
        public void ReactiveClientContext_DefineObservable()
        {
            Apply(
                ctx =>
                {
                    var ys = ctx.GetObservable<int>(new Uri(Constants.Observable.YS));
                    var pys = ctx.GetObservable<int, int>(new Uri(Constants.Observable.PYS));
                    ctx.DefineObservableAsync<int>(new Uri(Constants.Observable.XS), ys, null, CancellationToken.None);
                    ctx.DefineObservableAsync<int, int>(new Uri(Constants.Observable.PXS), x => pys(x), null, CancellationToken.None);
                    ctx.UndefineObservableAsync(new Uri(Constants.Observable.XS), CancellationToken.None);
                },
                new DefineObservable(new Uri(Constants.Observable.XS), Expression.Parameter(typeof(IAsyncReactiveQbservable<int>), Constants.Observable.YS), null),
                new DefineObservable(
                    new Uri(Constants.Observable.PXS),
                    Expression.Parameter(typeof(int), "x").Let(x =>
                        Expression.Lambda<Func<int, IAsyncReactiveQbservable<int>>>(
                            Expression.Invoke(
                                Expression.Parameter(typeof(Func<int, IAsyncReactiveQbservable<int>>), Constants.Observable.PYS),
                                x
                            ),
                            x
                        )
                    ),
                    null
                ),
                new UndefineObservable(new Uri(Constants.Observable.XS))
            );
        }

        [TestMethod]
        public void ReactiveClientContext_DefineObserver()
        {
            Apply(
                ctx =>
                {
                    var observer = ctx.GetObserver<int>(new Uri(Constants.Observer.OB));
                    var pobserver = ctx.GetObserver<int, int>(new Uri(Constants.Observer.POB));
                    ctx.DefineObserverAsync<int>(new Uri(Constants.Observer.OB2), observer, null, CancellationToken.None);
                    ctx.DefineObserverAsync<int, int>(new Uri(Constants.Observer.POB2), x => pobserver(x), null, CancellationToken.None);
                    ctx.UndefineObserverAsync(new Uri(Constants.Observer.POB2), CancellationToken.None);
                },
                new DefineObserver(new Uri(Constants.Observer.OB2), Expression.Parameter(typeof(IAsyncReactiveQbserver<int>), Constants.Observer.OB), null),
                new DefineObserver(
                    new Uri(Constants.Observer.POB2),
                    Expression.Parameter(typeof(int), "x").Let(x =>
                        Expression.Lambda<Func<int, IAsyncReactiveQbserver<int>>>(
                            Expression.Invoke(
                                Expression.Parameter(typeof(Func<int, IAsyncReactiveQbserver<int>>), Constants.Observer.POB),
                                x
                            ),
                            x
                        )
                    ),
                    null
                ),
                new UndefineObserver(new Uri(Constants.Observer.POB2))
            );
        }

        [TestMethod]
        public void ReactiveClientContext_DefineStreamFactory()
        {
            Apply(
                ctx =>
                {
                    var sf = ctx.GetStreamFactory<int, int>(new Uri(Constants.StreamFactory.SF));
                    var psf = ctx.GetStreamFactory<int, int, int>(new Uri(Constants.StreamFactory.PSF));
                    ctx.DefineStreamFactoryAsync<int, int>(new Uri(Constants.StreamFactory.SG), sf, null, CancellationToken.None);
                    ctx.DefineStreamFactoryAsync<int, int, int>(new Uri(Constants.StreamFactory.PSG), psf, null, CancellationToken.None);
                    ctx.UndefineStreamFactoryAsync(new Uri(Constants.StreamFactory.SG), CancellationToken.None);
                },
                new DefineStreamFactory(new Uri(Constants.StreamFactory.SG), Expression.Parameter(typeof(Func<IAsyncReactiveQubject<int, int>>), Constants.StreamFactory.SF), null),
                new DefineStreamFactory(new Uri(Constants.StreamFactory.PSG), Expression.Parameter(typeof(Func<int, IAsyncReactiveQubject<int, int>>), Constants.StreamFactory.PSF), null),
                new UndefineStreamFactory(new Uri(Constants.StreamFactory.SG))
            );
        }

        [TestMethod]
        public void ReactiveClientContext_DefineSubscriptionFactory()
        {
            Apply(
                ctx =>
                {
                    var sf = ctx.GetSubscriptionFactory(new Uri(Constants.SubscriptionFactory.SF));
                    var psf = ctx.GetSubscriptionFactory<int>(new Uri(Constants.SubscriptionFactory.PSF));
                    ctx.DefineSubscriptionFactoryAsync(new Uri(Constants.SubscriptionFactory.SG), sf, null, CancellationToken.None);
                    ctx.DefineSubscriptionFactoryAsync<int>(new Uri(Constants.SubscriptionFactory.PSG), psf, null, CancellationToken.None);
                    ctx.UndefineSubscriptionFactoryAsync(new Uri(Constants.SubscriptionFactory.SG), CancellationToken.None);
                },
                new DefineSubscriptionFactory(new Uri(Constants.SubscriptionFactory.SG), Expression.Parameter(typeof(Func<IAsyncReactiveQubscription>), Constants.SubscriptionFactory.SF), null),
                new DefineSubscriptionFactory(new Uri(Constants.SubscriptionFactory.PSG), Expression.Parameter(typeof(Func<int, IAsyncReactiveQubscription>), Constants.SubscriptionFactory.PSF), null),
                new UndefineSubscriptionFactory(new Uri(Constants.SubscriptionFactory.SG))
            );
        }

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_GetStream()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetStream<int, int>(new Uri(Constants.Stream.FOO));
                    var ys = ctx.GetStream<int, int>(new Uri(Constants.Stream.BAR));
                    var zs = ctx.GetStream<int, int>(new Uri(Constants.Stream.QUX));

                    xs.Do(ys).SubscribeAsync(zs, new Uri(Constants.Subscription.SUB), null).Wait();
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, IAsyncReactiveQbserver<int>, Task<IAsyncReactiveQubscription>>), Constants.SubscribeUri),
                        Expression.Invoke(
                            Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, IAsyncReactiveQbserver<int>, IAsyncReactiveQbservable<int>>), Constants.Observable.Do),
                            Expression.Parameter(typeof(IAsyncReactiveQbservable<int>), Constants.Stream.FOO),
                            Expression.Parameter(typeof(IAsyncReactiveQbserver<int>), Constants.Stream.BAR)
                        ),
                        Expression.Parameter(typeof(IAsyncReactiveQbserver<int>), Constants.Stream.QUX)
                    ),
                    null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_CreateStreamAsync()
        {
            Apply(
                ctx =>
                {
                    var sf = ctx.GetStreamFactory<int, int>(new Uri(Constants.StreamFactory.SF));

                    var xs = sf.CreateAsync(new Uri(Constants.Stream.FOO), null).Result;
                    var ys = sf.CreateAsync(new Uri(Constants.Stream.BAR), null).Result;
                    var zs = sf.CreateAsync(new Uri(Constants.Stream.QUX), null).Result;

                    xs.Do(ys).SubscribeAsync(zs, new Uri(Constants.Subscription.SUB), null).Wait();
                },
                new CreateStream(
                    new Uri(Constants.Stream.FOO),
                    Expression.Invoke(Expression.Parameter(typeof(Func<IAsyncReactiveQubject<int, int>>), Constants.StreamFactory.SF)),
                    null
                ),
                new CreateStream(
                    new Uri(Constants.Stream.BAR),
                    Expression.Invoke(Expression.Parameter(typeof(Func<IAsyncReactiveQubject<int, int>>), Constants.StreamFactory.SF)),
                    null
                ),
                new CreateStream(
                    new Uri(Constants.Stream.QUX),
                    Expression.Invoke(Expression.Parameter(typeof(Func<IAsyncReactiveQubject<int, int>>), Constants.StreamFactory.SF)),
                    null
                ),
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, IAsyncReactiveQbserver<int>, Task<IAsyncReactiveQubscription>>), Constants.SubscribeUri),
                        Expression.Invoke(
                            Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, IAsyncReactiveQbserver<int>, IAsyncReactiveQbservable<int>>), Constants.Observable.Do),
                            Expression.Parameter(typeof(IAsyncReactiveQbservable<int>), Constants.Stream.FOO),
                            Expression.Parameter(typeof(IAsyncReactiveQbserver<int>), Constants.Stream.BAR)
                        ),
                        Expression.Parameter(typeof(IAsyncReactiveQbserver<int>), Constants.Stream.QUX)
                    ),
                    null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_WithInnerNonParameterizedObservable()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<int>(new Uri(Constants.Observable.XS));
                    var ob = ctx.GetObserver<IAsyncReactiveQbservable<int>>(new Uri(Constants.Observer.OB));
                    xs.Select(x => ctx.Empty<int>()).SubscribeAsync(ob, new Uri(Constants.Subscription.SUB), null, CancellationToken.None).Wait();
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<IAsyncReactiveQbservable<int>>, IAsyncReactiveQbserver<IAsyncReactiveQbservable<int>>, Task<IAsyncReactiveQubscription>>), Constants.SubscribeUri),
                        Expression.Parameter(typeof(int), "x").Let(x =>
                            Expression.Invoke(
                                Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, Expression<Func<int, IAsyncReactiveQbservable<int>>>, IAsyncReactiveQbservable<IAsyncReactiveQbservable<int>>>), Constants.Observable.Select),
                                Expression.Parameter(typeof(IAsyncReactiveQbservable<int>), Constants.Observable.XS),
                                Expression.Lambda<Func<int, IAsyncReactiveQbservable<int>>>(
                                    Expression.Parameter(typeof(IAsyncReactiveQbservable<int>), Constants.Observable.Empty),
                                    x
                                )
                            )
                        ),
                        Expression.Parameter(typeof(IAsyncReactiveQbserver<IAsyncReactiveQbservable<int>>), Constants.Observer.OB)
                    ),
                    null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_Metadata_Observables()
        {
            Apply(
                ctx =>
                {
                    var all = ctx.Observables.ToList();
                },
                new MetadataQuery(
                    Expression.Parameter(typeof(IQueryableDictionary<Uri, IAsyncReactiveObservableDefinition>), "rx://metadata/observables")
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_Metadata_Observers()
        {
            Apply(
                ctx =>
                {
                    var all = ctx.Observers.ToList();
                },
                new MetadataQuery(
                    Expression.Parameter(typeof(IQueryableDictionary<Uri, IAsyncReactiveObserverDefinition>), "rx://metadata/observers")
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_Metadata_StreamFactories()
        {
            Apply(
                ctx =>
                {
                    var all = ctx.StreamFactories.ToList();
                },
                new MetadataQuery(
                    Expression.Parameter(typeof(IQueryableDictionary<Uri, IAsyncReactiveStreamFactoryDefinition>), "rx://metadata/streamFactories")
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_Metadata_SubscriptionFactories()
        {
            Apply(
                ctx =>
                {
                    var all = ctx.SubscriptionFactories.ToList();
                },
                new MetadataQuery(
                    Expression.Parameter(typeof(IQueryableDictionary<Uri, IAsyncReactiveSubscriptionFactoryDefinition>), "rx://metadata/subscriptionFactories")
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_Metadata_Subscriptions()
        {
            Apply(
                ctx =>
                {
                    var all = ctx.Subscriptions.ToList();
                },
                new MetadataQuery(
                    Expression.Parameter(typeof(IQueryableDictionary<Uri, IAsyncReactiveSubscriptionProcess>), "rx://metadata/subscriptions")
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_Metadata_Streams()
        {
            Apply(
                ctx =>
                {
                    var all = ctx.Streams.ToList();
                },
                new MetadataQuery(
                    Expression.Parameter(typeof(IQueryableDictionary<Uri, IAsyncReactiveStreamProcess>), "rx://metadata/streams")
                )
            );
        }

#pragma warning disable IDE0034 // Simplify 'default' expression (documents the signature)
        private static readonly MethodInfo s_where = ((MethodInfo)ReflectionHelpers.InfoOf(() => Queryable.Where<object>(default(IQueryable<object>), default(Expression<Func<object, bool>>)))).GetGenericMethodDefinition();
        private static readonly MethodInfo s_contains = ((MethodInfo)ReflectionHelpers.InfoOf(() => Queryable.Contains<object>(default(IQueryable<object>), default(object)))).GetGenericMethodDefinition();
#pragma warning restore IDE0034 // Simplify 'default' expression

        [TestMethod]
        public void ReactiveClientContext_Metadata_Observables_Query()
        {
            Apply(
                ctx =>
                {
                    var all = ctx.Observables.Where(kv => kv.Key == new Uri("bar://foo")).ToList();
                },
                new MetadataQuery(
                    Expression.Call(
                        s_where.MakeGenericMethod(typeof(KeyValuePair<Uri, IAsyncReactiveObservableDefinition>)),
                        Expression.Parameter(typeof(IQueryableDictionary<Uri, IAsyncReactiveObservableDefinition>), "rx://metadata/observables"),
                        Expression.Parameter(typeof(KeyValuePair<Uri, IAsyncReactiveObservableDefinition>), "kv").Let(kv =>
                            Expression.Lambda(
                                Expression.Equal(
                                    Expression.Property(kv, "Key"),
                                    Expression.New(
                                        typeof(Uri).GetConstructor(new[] { typeof(string) }),
                                        Expression.Constant("bar://foo")
                                    )
                                ),
                                kv
                            )
                        )
                    )
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_Metadata_Observables_Query_NonGeneric()
        {
            Apply(
                ctx =>
                {
                    var obs = ctx.Observables;
                    var qry = obs.Where(kv => kv.Key == new Uri("bar://foo"));
                    var res = (IQueryable<KeyValuePair<Uri, IAsyncReactiveObservableDefinition>>)obs.Provider.CreateQuery(qry.Expression);
                    var all = res.ToList();
                },
                new MetadataQuery(
                    Expression.Call(
                        s_where.MakeGenericMethod(typeof(KeyValuePair<Uri, IAsyncReactiveObservableDefinition>)),
                        Expression.Parameter(typeof(IQueryableDictionary<Uri, IAsyncReactiveObservableDefinition>), "rx://metadata/observables"),
                        Expression.Parameter(typeof(KeyValuePair<Uri, IAsyncReactiveObservableDefinition>), "kv").Let(kv =>
                            Expression.Lambda(
                                Expression.Equal(
                                    Expression.Property(kv, "Key"),
                                    Expression.New(
                                        typeof(Uri).GetConstructor(new[] { typeof(string) }),
                                        Expression.Constant("bar://foo")
                                    )
                                ),
                                kv
                            )
                        )
                    )
                )
            );
        }

#pragma warning disable IDE0034 // Simplify 'default' expression (documents the signature)
        private static readonly MethodInfo s_count = ((MethodInfo)ReflectionHelpers.InfoOf(() => Queryable.Count<object>(default(IQueryable<object>)))).GetGenericMethodDefinition();
#pragma warning restore IDE0034 // Simplify 'default' expression

        [TestMethod]
        public void ReactiveClientContext_Metadata_Observables_Query_Aggregate()
        {
            Apply(
                ctx =>
                {
#pragma warning disable IDE0079 // Remove unnecessary suppression (only in .NET 5.0)
#pragma warning disable CA1829 // Use Length/Count property instead of Count() when available (testing query provider)
                    var n = ctx.Observables.Count();
#pragma warning restore CA1829
#pragma warning restore IDE0079
                },
                new MetadataQuery(
                    Expression.Call(
                        s_count.MakeGenericMethod(typeof(KeyValuePair<Uri, IAsyncReactiveObservableDefinition>)),
                        Expression.Parameter(typeof(IQueryableDictionary<Uri, IAsyncReactiveObservableDefinition>), "rx://metadata/observables")
                    )
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_Metadata_Observables_Query_Aggregate_NonGeneric()
        {
            Apply(
                ctx =>
                {
                    var exp = Expression.Call(
                        s_count.MakeGenericMethod(typeof(KeyValuePair<Uri, IAsyncReactiveObservableDefinition>)),
                        ctx.Observables.Expression
                    );

                    var n = (int)ctx.Observables.Provider.Execute(exp);
                },
                new MetadataQuery(
                    Expression.Call(
                        s_count.MakeGenericMethod(typeof(KeyValuePair<Uri, IAsyncReactiveObservableDefinition>)),
                        Expression.Parameter(typeof(IQueryableDictionary<Uri, IAsyncReactiveObservableDefinition>), "rx://metadata/observables")
                    )
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_Metadata_Provider_ArgumentChecking()
        {
            Apply(
                ctx =>
                {
                    var prv = ctx.Observables.Provider;

                    Assert.ThrowsException<ArgumentNullException>(() => prv.Execute(null));
                    Assert.ThrowsException<ArgumentNullException>(() => prv.Execute<int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => prv.CreateQuery(null));
                    Assert.ThrowsException<ArgumentNullException>(() => prv.CreateQuery<int>(null));

                    Assert.ThrowsException<InvalidOperationException>(() => prv.Execute<int>(Expression.Constant("foo")));
                    Assert.ThrowsException<InvalidOperationException>(() => prv.CreateQuery(Expression.Constant("foo")));
                    Assert.ThrowsException<InvalidOperationException>(() => prv.CreateQuery(Expression.Default(typeof(IQueryable))));
                }
            );
        }

        [TestMethod]
        public void ReactiveClientContext_Metadata_Join_Query()
        {
            Apply(
                ctx =>
                {
                    var all = ctx.Observables.Where(kv => ctx.Observers.Keys.Contains(kv.Key)).ToList();
                },
                new MetadataQuery(
                    Expression.Call(
                        s_where.MakeGenericMethod(typeof(KeyValuePair<Uri, IAsyncReactiveObservableDefinition>)),
                        Expression.Parameter(typeof(IQueryableDictionary<Uri, IAsyncReactiveObservableDefinition>), "rx://metadata/observables"),
                        Expression.Parameter(typeof(IQueryableDictionary<Uri, IAsyncReactiveObserverDefinition>), "rx://metadata/observers").Let(observers =>
                            Expression.Parameter(typeof(KeyValuePair<Uri, IAsyncReactiveObservableDefinition>), "kv").Let(kv =>
                                Expression.Lambda(
                                    Expression.Call(
                                        s_contains.MakeGenericMethod(typeof(Uri)),
                                        Expression.Property(
                                            observers,
                                            "Keys"
                                        ),
                                        Expression.Property(
                                            kv,
                                            "Key"
                                        )
                                    ),
                                    kv
                                )
                            )
                        )
                    )
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_Metadata_Join_Query_WithUnboundContext()
        {
            // This test simply asserts that we do not rewrite the collections to their expression form if the expression contains unbound parameters.
            Expression<Func<IQueryableDictionary<Uri, IAsyncReactiveObservableDefinition>, IQueryable<KeyValuePair<Uri, IAsyncReactiveObservableDefinition>>>> f =
                observables => observables.Where(kv => Foo(kv.Key.AbsoluteUri).Observers.Keys.Contains(kv.Key));

            var expression = BetaReducer.Reduce(
                Expression.Invoke(
                    f,
                    Expression.Parameter(
                        typeof(IQueryableDictionary<Uri, IAsyncReactiveObservableDefinition>),
                        "rx://metadata/observables"
                    )
                )
            );

            Apply(
                ctx =>
                {
                    _ = ctx.Observables.Where(kv => Foo(kv.Key.AbsoluteUri).Observers.Keys.Contains(kv.Key)).ToList();
                },
                new MetadataQuery(expression)
            );
        }

        [TestMethod]
        public void ReactiveClientContext_Metadata_Join_Query_WithNonInterfaceProperty()
        {
            // This test simply asserts that we do not rewrite the collections to their expression form if the expression contains unbound parameters.
            Expression<Func<ReactiveClientContext, IQueryableDictionary<Uri, IAsyncReactiveObservableDefinition>, IQueryable<KeyValuePair<Uri, IAsyncReactiveObservableDefinition>>>> f =
                (ctx, observables) => observables.Where(kv => ctx.Provider.ToString() == kv.Key.AbsoluteUri);

            var expression = BetaReducer.Reduce(
                Expression.Invoke(
                    f,
                    Expression.Parameter(
                        typeof(TestClientContext),
                        "rx://builtin/this"
                    ),
                    Expression.Parameter(
                        typeof(IQueryableDictionary<Uri, IAsyncReactiveObservableDefinition>),
                        "rx://metadata/observables"
                    )
                )
            );

            Apply(
                ctx =>
                {
                    // This query is gibberish, but `Provider` is just some property that
                    // doesn't exist on the `IReactiveMetadataProxy` interface.
                    _ = ctx.Observables.Where(kv => ctx.Provider.ToString() == kv.Key.AbsoluteUri).ToList();
                },
                new MetadataQuery(expression)
            );
        }

        private static ReactiveClientContext Foo(string arg)
        {
            _ = arg;
            return null;
        }
    }
}
