// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD, ER - October 2013 - Created this file.
//

using System;
using System.Linq;
using System.Linq.Expressions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Reaqtor.Reliable.Client;
using Reaqtor.Reliable.Expressions;
using Reaqtor.Reliable.Service;
using Reaqtor.TestingFramework;

namespace Tests.Reaqtor.Reliable
{
    [TestClass]
    public partial class ReliableReactiveClientContextTests : ReliableReactiveServiceContextTestBase
    {
        [TestMethod]
        public void ReliableReactiveServiceContext_ReliableQbservable_ArgumentChecking()
        {
            Apply(
                ctx =>
                {
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.GetObservable<int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.GetObservable<int, int>(null));

                    var o = ctx.GetObservable<int>(new Uri(Constants.Observable.XS));
                    var provider = o.Provider;

#pragma warning disable IDE0004 // Remove Unnecessary Cast. (Makes the intent clear.)
                    Assert.ThrowsException<ArgumentNullException>(() => o.Subscribe(null, new Uri(Constants.Subscription.SUB), null));
                    Assert.ThrowsException<ArgumentNullException>(() => o.Subscribe(((IReliableReactiveObserver<int>)ctx.GetObserver<int>(new Uri("bing://obs"))), null, null));
                    Assert.ThrowsException<ArgumentNullException>(() => o.Subscribe(ctx.GetObserver<int>(new Uri("bing://obs")), null, null));
                    Assert.ThrowsException<ArgumentNullException>(() => ((IReliableReactiveObservable<int>)o).Subscribe(null, new Uri(Constants.Subscription.SUB), null));
                    Assert.ThrowsException<ArgumentNullException>(() => ((IReliableReactiveObservable<int>)o).Subscribe(((IReliableReactiveObserver<int>)ctx.GetObserver<int>(new Uri("bing://obs"))), null, null));
#pragma warning restore IDE0004

                    Assert.ThrowsException<ArgumentNullException>(() => provider.CreateQbservable<int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => provider.CreateQbservable<int, int>(null));
                }
            );
        }

        [TestMethod]
        public void ReliableReactiveServiceContext_ReliableQbserver_ArgumentChecking()
        {
            Apply(
                ctx =>
                {
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.GetObserver<int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.GetObserver<int, int>(null));

                    var observer = ctx.GetObserver<int>(new Uri(Constants.Observer.OB));
                    Assert.AreEqual(typeof(int), observer.ElementType);

                    Assert.ThrowsException<ArgumentNullException>(() => observer.OnError(null));
                }
            );
        }

        [TestMethod]
        public void ReliableReactiveServiceContext_QubjectFactory_ArgumentChecking()
        {
            Apply(
                ctx =>
                {
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.GetStreamFactory<int, int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.GetStreamFactory<int, int, int>(null));

                    var factory = ctx.GetStreamFactory<int, int>(new Uri("bing://foo"));
                    var pfactory = ctx.GetStreamFactory<string, int, int>(new Uri("bing://pfoo"));

                    Assert.ThrowsException<ArgumentNullException>(() => factory.Create(null, null));
                    Assert.ThrowsException<ArgumentNullException>(() => pfactory.Create(null, "factory_parameter_1", null));
                    Assert.ThrowsException<ArgumentNullException>(() => ((IReliableReactiveSubjectFactory<int, int>)factory).Create(null, null));
                    Assert.ThrowsException<ArgumentNullException>(() => ((IReliableReactiveSubjectFactory<int, int, string>)pfactory).Create(null, "factory_parameter_1", null));
                }
            );
        }

        [TestMethod]
        public void ReliableReactiveServiceContext_QubscriptionFactory_ArgumentChecking()
        {
            Apply(
                ctx =>
                {
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.GetSubscriptionFactory(null));
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.GetSubscriptionFactory<int>(null));

                    var factory = ctx.GetSubscriptionFactory(new Uri("bing://foo"));
                    var pfactory = ctx.GetSubscriptionFactory<string>(new Uri("bing://pfoo"));

                    Assert.ThrowsException<ArgumentNullException>(() => factory.Create(null, null));
                    Assert.ThrowsException<ArgumentNullException>(() => pfactory.Create(null, "factory_parameter_1", null));
                    Assert.ThrowsException<ArgumentNullException>(() => ((IReliableReactiveSubscriptionFactory)factory).Create(null, null));
                    Assert.ThrowsException<ArgumentNullException>(() => ((IReliableReactiveSubscriptionFactory<string>)pfactory).Create(null, "factory_parameter_1", null));
                }
            );
        }

        [TestMethod]
        public void ReliableReactiveServiceContext_ReliableQueryProviderBase_ArgumentChecking()
        {
            Apply(
                ctx =>
                {
                    var provider = (ReliableQueryProvider)ctx.Provider;

                    Assert.ThrowsException<ArgumentNullException>(() => provider.CreateQbserver<int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => provider.CreateQbserver<int, int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => provider.CreateQubject<int, int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => provider.CreateQubjectFactory<int, int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => provider.CreateQubjectFactory<int, int, int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => provider.CreateQubscriptionFactory(null));
                    Assert.ThrowsException<ArgumentNullException>(() => provider.CreateQubscriptionFactory<int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => provider.CreateQubscription(null));

                    Assert.ThrowsException<ArgumentNullException>(() => provider.AcknowledgeRange(null, 0));
                    Assert.ThrowsException<ArgumentNullException>(() => provider.StartSubscription(null, 0));
                    Assert.ThrowsException<ArgumentNullException>(() => provider.GetSubscriptionResubscribeUri(null));
                    Assert.ThrowsException<ArgumentNullException>(() => provider.CreateObserver<int, int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => provider.CreateSubscription(null, null));
                    Assert.ThrowsException<ArgumentNullException>(() => provider.DeleteStream<int, int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => provider.DeleteSubscription(null));
                    Assert.ThrowsException<ArgumentNullException>(() => provider.GetObserver<int>(null));
                }
            );
        }

        [TestMethod]
        public void ReliableReactiveServiceContext_ReliableQubject_ArgumentChecking()
        {
            Apply(
                ctx =>
                {
                    var s = ctx.GetStream<int, int>(new Uri(Constants.Stream.FOO));

                    var provider = s.Provider;

#pragma warning disable IDE0004 // Remove Unnecessary Cast. (Makes the intent clear.)
                    Assert.ThrowsException<ArgumentNullException>(() => s.Subscribe(null, new Uri(Constants.Subscription.SUB), null));
                    Assert.ThrowsException<ArgumentNullException>(() => s.Subscribe(ctx.GetObserver<int>(new Uri("bing://obs")), null, null));
                    Assert.ThrowsException<ArgumentNullException>(() => s.Subscribe(((IReliableReactiveObserver<int>)ctx.GetObserver<int>(new Uri("bing://obs"))), null, null));
                    Assert.ThrowsException<ArgumentNullException>(() => ((IReliableReactiveMultiSubject<int, int>)s).Subscribe(null, new Uri(Constants.Subscription.SUB), null));
                    Assert.ThrowsException<ArgumentNullException>(() => ((IReliableReactiveMultiSubject<int, int>)s).Subscribe(ctx.GetObserver<int>(new Uri("bing://obs")), null, null));
                    Assert.ThrowsException<ArgumentNullException>(() => ((IReliableReactiveMultiSubject<int, int>)s).Subscribe(((IReliableReactiveObserver<int>)ctx.GetObserver<int>(new Uri("bing://obs"))), null, null));
#pragma warning restore IDE0004 // Remove Unnecessary Cast
                }
            );
        }

        [TestMethod]
        public void ReliableReactiveServiceContext_ReliableQbserver_Unknown()
        {
            Apply(
                ctx =>
                {
                    var qbserver = ctx.Provider.CreateQbserver<int>(Expression.Default(typeof(IReliableQbserver<int>)));

                    var uri = default(Uri);
                    Assert.ThrowsException<InvalidOperationException>(() => uri = qbserver.ResubscribeUri);
                }
            );
        }

        [TestMethod]
        public void ReliableReactiveServiceContext_ReliableQubject_Unknown()
        {
            Apply(
                ctx =>
                {
                    var qubject = ctx.Provider.CreateQubject<int, int>(Expression.Default(typeof(IReliableMultiQubject<int, int>)));
                    Assert.ThrowsException<InvalidOperationException>(() => qubject.CreateObserver());
                    Assert.ThrowsException<InvalidOperationException>(() => qubject.Dispose());
                }
            );
        }

        [TestMethod]
        public void ReliableReactiveServiceContext_ReliableQubscription_Unknown()
        {
            Apply(
                ctx =>
                {
                    var qubscription = ctx.Provider.CreateQubscription(Expression.Default(typeof(IReliableQubscription)));

                    var uri = default(Uri);
                    Assert.ThrowsException<InvalidOperationException>(() => uri = qubscription.ResubscribeUri);

                    Assert.ThrowsException<InvalidOperationException>(() => qubscription.Start(0));
                    Assert.ThrowsException<InvalidOperationException>(() => qubscription.AcknowledgeRange(0));
                    Assert.ThrowsException<InvalidOperationException>(() => qubscription.Dispose());
                }
            );
        }

        [TestMethod]
        public void ReliableReactiveServiceContext_Subscribe_Simple1()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<int>(new Uri(Constants.Observable.XS));
                    Assert.AreEqual(typeof(int), xs.ElementType);

                    var ob = ctx.GetObserver<int>(new Uri(Constants.Observer.OB));
                    Assert.AreEqual(typeof(int), xs.ElementType);

                    xs.Subscribe(ob, new Uri(Constants.Subscription.SUB1), null);
                    ((IReliableReactiveObservable<int>)xs).Subscribe(ob, new Uri(Constants.Subscription.SUB2), null);
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB1),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<IReliableQbservable<int>, IReliableQbserver<int>, IReliableQubscription>), Constants.SubscribeUri),
                        Expression.Parameter(typeof(IReliableQbservable<int>), Constants.Observable.XS),
                        Expression.Parameter(typeof(IReliableQbserver<int>), Constants.Observer.OB)
                    ),
                    null
                ),
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB2),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<IReliableQbservable<int>, IReliableQbserver<int>, IReliableQubscription>), Constants.SubscribeUri),
                        Expression.Parameter(typeof(IReliableQbservable<int>), Constants.Observable.XS),
                        Expression.Parameter(typeof(IReliableQbserver<int>), Constants.Observer.OB)
                    ),
                    null
                )
            );
        }

        [TestMethod]
        public void ReliableReactiveServiceContext_Subscribe_Simple2()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<int>(new Uri(Constants.Observable.XS));

                    var ys = from x in xs
                             where x > 0
                             select x * x;

                    var ob = ctx.GetObserver<int>(new Uri(Constants.Observer.OB));
                    ys.Subscribe(ob, new Uri(Constants.Subscription.SUB), null);
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Parameter(typeof(int), "x").Let(x =>
                        Expression.Invoke(
                            Expression.Parameter(typeof(Func<IReliableQbservable<int>, IReliableQbserver<int>, IReliableQubscription>), Constants.SubscribeUri),
                            Expression.Invoke(
                                Expression.Parameter(typeof(Func<IReliableQbservable<int>, Expression<Func<int, int>>, IReliableQbservable<int>>), Constants.Observable.Select),
                                Expression.Invoke(
                                    Expression.Parameter(typeof(Func<IReliableQbservable<int>, Expression<Func<int, bool>>, IReliableQbservable<int>>), Constants.Observable.Where),
                                    Expression.Parameter(typeof(IReliableQbservable<int>), Constants.Observable.XS),
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
                            Expression.Parameter(typeof(IReliableQbserver<int>), Constants.Observer.OB)
                        )
                    ),
                    null
                )
            );
        }

        [TestMethod]
        public void ReliableReactiveServiceContext_Subscribe_Simple4()
        {
            var longParam = Expression.Parameter(typeof(long), "dueTime");

            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<long>(new Uri(Constants.Observable.XS));

                    var ys = xs.SelectMany(dueTime => ctx.Timer(new TimeSpan(dueTime)));

                    var ob = ctx.GetObserver<long>(new Uri(Constants.Observer.OB));
                    ys.Subscribe(ob, new Uri(Constants.Subscription.SUB), null);
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<IReliableQbservable<long>, IReliableQbserver<long>, IReliableQubscription>), Constants.SubscribeUri),
                        Expression.Invoke(
                            Expression.Parameter(typeof(Func<IReliableQbservable<long>, Expression<Func<long, IReliableQbservable<long>>>, IReliableQbservable<long>>), Constants.Observable.Bind),
                            Expression.Parameter(typeof(IReliableQbservable<long>), Constants.Observable.XS),
                            Expression.Lambda<Func<long, IReliableQbservable<long>>>(
                                Expression.Invoke(
                                    Expression.Parameter(typeof(Func<TimeSpan, IReliableQbservable<long>>), Constants.Observable.Timer),
                                    Expression.New(typeof(TimeSpan).GetConstructor(new[] { typeof(long) }), longParam)
                                ),
                                longParam
                            )
                        ),
                        Expression.Parameter(typeof(IReliableQbserver<long>), Constants.Observer.OB)
                    ),
                    null
                )
            );
        }

        [TestMethod]
        public void ReliableReactiveServiceContext_Subscribe_Simple3()
        {
            Apply(
                provider => new MyContext(provider),
                ctx =>
                {
                    var xs = ctx.Xs;
                    var ob = ctx.GetObserver<int>(new Uri(Constants.Observer.OB));
                    xs.Subscribe(ob, new Uri(Constants.Subscription.SUB), null);
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<IReliableQbservable<int>, IReliableQbserver<int>, IReliableQubscription>), Constants.SubscribeUri),
                        Expression.Parameter(typeof(IReliableQbservable<int>), Constants.Observable.XS),
                        Expression.Parameter(typeof(IReliableQbserver<int>), Constants.Observer.OB)
                    ),
                    null
                )
            );
        }

        [TestMethod]
        public void ReliableReactiveServiceContext_Subscribe_NAry_1()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<int, string>(new Uri(Constants.Observable.XS));
                    var ob = ctx.GetObserver<int, string>(new Uri(Constants.Observer.OB));
                    xs(1).Subscribe(ob(-1), new Uri(Constants.Subscription.SUB), null);
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<IReliableQbservable<string>, IReliableQbserver<string>, IReliableQubscription>), Constants.SubscribeUri),
                        Expression.Invoke(
                            Expression.Parameter(typeof(Func<int, IReliableQbservable<string>>), Constants.Observable.XS),
                            Expression.Constant(1)
                        ),
                        Expression.Invoke(
                            Expression.Parameter(typeof(Func<int, IReliableQbserver<string>>), Constants.Observer.OB),
                            Expression.Constant(-1)
                        )
                    ),
                    null
                )
            );
        }

        [TestMethod]
        public void ReliableReactiveServiceContext_Subscribe_Closure1()
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
                        ys.Subscribe(ob, new Uri(Constants.Subscription.SUB), null);
                    }
                },
                Enumerable.Range(0, 3).Select(i =>
                    new CreateSubscription(
                        new Uri(Constants.Subscription.SUB),
                        Expression.Parameter(typeof(int), "x").Let(x =>
                            Expression.Invoke(
                                Expression.Parameter(typeof(Func<IReliableQbservable<int>, IReliableQbserver<int>, IReliableQubscription>), Constants.SubscribeUri),
                                Expression.Invoke(
                                    Expression.Parameter(typeof(Func<IReliableQbservable<int>, Expression<Func<int, int>>, IReliableQbservable<int>>), Constants.Observable.Select),
                                    Expression.Invoke(
                                        Expression.Parameter(typeof(Func<IReliableQbservable<int>, Expression<Func<int, bool>>, IReliableQbservable<int>>), Constants.Observable.Where),
                                        Expression.Parameter(typeof(IReliableQbservable<int>), Constants.Observable.XS),
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
                                Expression.Parameter(typeof(IReliableQbserver<int>), Constants.Observer.OB)
                            )
                        ),
                        null
                    )
                ).ToArray()
            );
        }

        [TestMethod]
        public void ReliableReactiveServiceContext_Subscribe_Parameterized()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<string, int>(new Uri(Constants.Observable.XS));

                    var ys = from x in xs("bar")
                             where x > 0
                             select x * x;

                    var ob = ctx.GetObserver<int>(new Uri(Constants.Observer.OB));
                    ys.Subscribe(ob, new Uri(Constants.Subscription.SUB), null);
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Parameter(typeof(int), "x").Let(x =>
                        Expression.Invoke(
                            Expression.Parameter(typeof(Func<IReliableQbservable<int>, IReliableQbserver<int>, IReliableQubscription>), Constants.SubscribeUri),
                            Expression.Invoke(
                                Expression.Parameter(typeof(Func<IReliableQbservable<int>, Expression<Func<int, int>>, IReliableQbservable<int>>), Constants.Observable.Select),
                                Expression.Invoke(
                                    Expression.Parameter(typeof(Func<IReliableQbservable<int>, Expression<Func<int, bool>>, IReliableQbservable<int>>), Constants.Observable.Where),
                                    Expression.Invoke(
                                        Expression.Parameter(typeof(Func<string, IReliableQbservable<int>>), Constants.Observable.XS),
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
                            Expression.Parameter(typeof(IReliableQbserver<int>), Constants.Observer.OB)
                        )
                    ),
                    null
                )
            );
        }

        [TestMethod]
        public void ReliableReactiveServiceContext_Subscribe_SelectMany1_Simple()
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
                    zs.Subscribe(ob, new Uri(Constants.Subscription.SUB), null);
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Parameter(typeof(int), "x").Let(x =>
                        Expression.Parameter(typeof(string), "y").Let(y =>
                            Expression.Invoke(
                                Expression.Parameter(typeof(Func<IReliableQbservable<int>, IReliableQbserver<int>, IReliableQubscription>), Constants.SubscribeUri),
                                Expression.Invoke(
                                    Expression.Parameter(typeof(Func<IReliableQbservable<int>, Expression<Func<int, IReliableQbservable<string>>>, Expression<Func<int, string, int>>, IReliableQbservable<int>>), Constants.Observable.SelectMany),
                                    Expression.Parameter(typeof(IReliableQbservable<int>), Constants.Observable.XS),
                                    Expression.Lambda(
                                        Expression.Invoke(
                                            Expression.Parameter(typeof(Func<int, IReliableQbservable<string>>), Constants.Observable.YS),
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
                                Expression.Parameter(typeof(IReliableQbserver<int>), Constants.Observer.OB)
                            )
                        )
                    ),
                    null
                )
            );
        }

        [TestMethod]
        public void ReliableReactiveServiceContext_Subscribe_SelectMany2_Closure()
        {
            var xs = default(IReliableQbservable<int>);
            var ys = default(Func<int, IReliableQbservable<string>>);

            Apply(
                ctx =>
                {
                    xs = ctx.GetObservable<int>(new Uri(Constants.Observable.XS));
                    ys = ctx.GetObservable<int, string>(new Uri(Constants.Observable.YS));

                    var zs = from x in xs
                             from y in ys(x)
                             select x + y.Length;

                    var ob = ctx.GetObserver<int>(new Uri(Constants.Observer.OB));
                    zs.Subscribe(ob, new Uri(Constants.Subscription.SUB), null);
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Parameter(typeof(int), "x").Let(x =>
                        Expression.Parameter(typeof(string), "y").Let(y =>
                            Expression.Invoke(
                                Expression.Parameter(typeof(Func<IReliableQbservable<int>, IReliableQbserver<int>, IReliableQubscription>), Constants.SubscribeUri),
                                Expression.Invoke(
                                    Expression.Parameter(typeof(Func<IReliableQbservable<int>, Expression<Func<int, IReliableQbservable<string>>>, Expression<Func<int, string, int>>, IReliableQbservable<int>>), Constants.Observable.SelectMany),
                                    Expression.Parameter(typeof(IReliableQbservable<int>), Constants.Observable.XS),
                                    Expression.Lambda(
                                        Expression.Invoke(
                                            Expression.Parameter(typeof(Func<int, IReliableQbservable<string>>), Constants.Observable.YS),
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
                                Expression.Parameter(typeof(IReliableQbserver<int>), Constants.Observer.OB)
                            )
                        )
                    ),
                    null
                )
            );
        }

        [TestMethod]
        public void ReliableReactiveServiceContext_Subscribe_SelectMany3_CustomContext()
        {
            Apply(
                provider => new MyContext(provider),
                ctx =>
                {
                    var zs = from x1 in ctx.Xs
                             from x2 in ctx.Xs
                             select x1 + x2;

                    var ob = ctx.GetObserver<int>(new Uri(Constants.Observer.OB));
                    zs.Subscribe(ob, new Uri(Constants.Subscription.SUB), null);
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Parameter(typeof(int), "x1").Let(x1 =>
                        Expression.Parameter(typeof(int), "x2").Let(x2 =>
                            Expression.Invoke(
                                Expression.Parameter(typeof(Func<IReliableQbservable<int>, IReliableQbserver<int>, IReliableQubscription>), Constants.SubscribeUri),
                                Expression.Invoke(
                                    Expression.Parameter(typeof(Func<IReliableQbservable<int>, Expression<Func<int, IReliableQbservable<int>>>, Expression<Func<int, int, int>>, IReliableQbservable<int>>), Constants.Observable.SelectMany),
                                    Expression.Parameter(typeof(IReliableQbservable<int>), Constants.Observable.XS),
                                    Expression.Lambda(
                                        Expression.Parameter(typeof(IReliableQbservable<int>), Constants.Observable.XS),
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
                                Expression.Parameter(typeof(IReliableQbserver<int>), Constants.Observer.OB)
                            )
                        )
                    ),
                    null
                )
            );
        }

        [TestMethod]
        public void ReliableReactiveServiceContext_Subscribe_SelectMany2_Inlined()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<int>(new Uri(Constants.Observable.XS));

                    var zs = from x in xs
                             from y in ctx.GetObservable<int, string>(new Uri(Constants.Observable.YS))(x)
                             select x + y.Length;

                    var ob = ctx.GetObserver<int>(new Uri(Constants.Observer.OB));
                    zs.Subscribe(ob, new Uri(Constants.Subscription.SUB), null);
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Parameter(typeof(int), "x").Let(x =>
                        Expression.Parameter(typeof(string), "y").Let(y =>
                            Expression.Invoke(
                                Expression.Parameter(typeof(Func<IReliableQbservable<int>, IReliableQbserver<int>, IReliableQubscription>), Constants.SubscribeUri),
                                Expression.Invoke(
                                    Expression.Parameter(typeof(Func<IReliableQbservable<int>, Expression<Func<int, IReliableQbservable<string>>>, Expression<Func<int, string, int>>, IReliableQbservable<int>>), Constants.Observable.SelectMany),
                                    Expression.Parameter(typeof(IReliableQbservable<int>), Constants.Observable.XS),
                                    Expression.Lambda(
                                        Expression.Invoke(
                                            Expression.Parameter(typeof(Func<int, IReliableQbservable<string>>), Constants.Observable.YS),
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
                                Expression.Parameter(typeof(IReliableQbserver<int>), Constants.Observer.OB)
                            )
                        )
                    ),
                    null
                )
            );
        }

        [TestMethod]
        public void ReliableReactiveServiceContext_GetSubscription()
        {
            Apply(
                ctx =>
                {
                    var s = ctx.GetSubscription(new Uri(Constants.Subscription.SUB));

                    s.Dispose();
                },
                new DeleteSubscription(new Uri(Constants.Subscription.SUB))
            );
        }

        [TestMethod]
        public void ReliableReactiveServiceContext_GetStreamFactory()
        {
            var createStreamExpression =
                Expression.Invoke(
                    Expression.Parameter(typeof(Func<IReliableMultiQubject<int, int>>), Constants.Observable.ZS)
                );

            Apply(
                ctx =>
                {
                    var factory = ctx.GetStreamFactory<int, int>(new Uri(Constants.Observable.ZS));

                    var qubject = factory.Create(new Uri(Constants.Stream.BAR), null);
                    Assert.AreEqual(typeof(int), qubject.ElementType);

                    ((IReliableReactiveSubjectFactory<int, int>)factory).Create(new Uri(Constants.Stream.FOO), null);
                    qubject.Subscribe(ctx.GetObserver<int>(new Uri(Constants.Observer.OB)), new Uri(Constants.Subscription.SUB), null);
                },
                new CreateStream(new Uri(Constants.Stream.BAR), createStreamExpression, null),
                new CreateStream(new Uri(Constants.Stream.FOO), createStreamExpression, null),
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<IReliableQbservable<int>, IReliableQbserver<int>, IReliableQubscription>), Constants.SubscribeUri),
                        Expression.Parameter(typeof(IReliableMultiQubject<int, int>), Constants.Stream.BAR),
                        Expression.Parameter(typeof(IReliableQbserver<int>), Constants.Observer.OB)
                    ),
                    null
                )
            );
        }

        [TestMethod]
        public void ReliableReactiveServiceContext_GetStreamFactory_Parameterized()
        {
            var createStreamExpression =
                Expression.Invoke(
                    Expression.Parameter(typeof(Func<string, IReliableMultiQubject<int, int>>), Constants.Observable.ZS),
                    Expression.Constant("factory_parameter_1")
                );

            Apply(
                ctx =>
                {
                    var factory = ctx.GetStreamFactory<string, int, int>(new Uri(Constants.Observable.ZS));

                    var qubject = factory.Create(new Uri(Constants.Stream.BAR), "factory_parameter_1", null);
                    ((IReliableReactiveSubjectFactory<int, int, string>)factory).Create(new Uri(Constants.Stream.FOO), "factory_parameter_1", null);

                    ((IReliableReactiveMultiSubject<int, int>)qubject).Subscribe(ctx.GetObserver<int>(new Uri(Constants.Observer.OB)), new Uri(Constants.Subscription.SUB), null);
                },
                new CreateStream(new Uri(Constants.Stream.BAR), createStreamExpression, null),
                new CreateStream(new Uri(Constants.Stream.FOO), createStreamExpression, null),
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<IReliableQbservable<int>, IReliableQbserver<int>, IReliableQubscription>), Constants.SubscribeUri),
                        createStreamExpression, // BUG
                        Expression.Parameter(typeof(IReliableQbserver<int>), Constants.Observer.OB)
                    ),
                    null
                )
            );
        }

        [TestMethod]
        public void ReliableReactiveServiceContext_GetSubscriptionFactory()
        {
            var createSubscriptionExpression =
                Expression.Invoke(
                    Expression.Parameter(typeof(Func<IReliableQubscription>), Constants.Observable.ZS)
                );

            Apply(
                ctx =>
                {
                    var factory = ctx.GetSubscriptionFactory(new Uri(Constants.Observable.ZS));

                    var qubscription = factory.Create(new Uri(Constants.Subscription.SUB1), null);

                    ((IReliableReactiveSubscriptionFactory)factory).Create(new Uri(Constants.Subscription.SUB2), null);
                },
                new CreateSubscription(new Uri(Constants.Subscription.SUB1), createSubscriptionExpression, null),
                new CreateSubscription(new Uri(Constants.Subscription.SUB2), createSubscriptionExpression, null)
            );
        }

        [TestMethod]
        public void ReliableReactiveServiceContext_GetSubscriptionFactory_Parameterized()
        {
            var createSubscriptionExpression =
                Expression.Invoke(
                    Expression.Parameter(typeof(Func<string, IReliableQubscription>), Constants.Observable.ZS),
                    Expression.Constant("factory_parameter_1")
                );

            Apply(
                ctx =>
                {
                    var factory = ctx.GetSubscriptionFactory<string>(new Uri(Constants.Observable.ZS));

                    var qubscription = factory.Create(new Uri(Constants.Subscription.SUB1), "factory_parameter_1", null);
                    ((IReliableReactiveSubscriptionFactory<string>)factory).Create(new Uri(Constants.Subscription.SUB2), "factory_parameter_1", null);
                },
                new CreateSubscription(new Uri(Constants.Subscription.SUB1), createSubscriptionExpression, null),
                new CreateSubscription(new Uri(Constants.Subscription.SUB2), createSubscriptionExpression, null)
            );
        }

        [TestMethod]
        public void ReliableReactiveServiceContext_GetObserver()
        {
            var ex = new Exception();

            Apply(
                ctx =>
                {
                    var observer = ctx.GetObserver<int>(new Uri(Constants.Observer.OB));
                    observer.OnNext(76, 0L);
                    observer.OnError(ex);
                    observer = ctx.GetObserver<int>(new Uri(Constants.Observer.OC));
                    observer.OnCompleted();
                },
                new ObserverOnNext(new Uri(Constants.Observer.OB), 76),
                new ObserverOnError(new Uri(Constants.Observer.OB), ex),
                new ObserverOnCompleted(new Uri(Constants.Observer.OC))
            );
        }

        [TestMethod]
        public void ReliableReactiveServiceContext_ReliableQueryProvider_CreateQubject()
        {
            Apply(
                ctx =>
                {
                    var s = ctx.GetStream<int, int>(new Uri(Constants.Stream.BAR));
                    var q = ctx.Provider.CreateQubject<int, int>(s.Expression);
                    q.Dispose();
                },
                new DeleteStream(new Uri(Constants.Stream.BAR))
            );
        }

        [TestMethod]
        public void ReliableReactiveServiceContext_ReliableMultiQubject_CreateObserver()
        {
            Apply(
                ctx =>
                {
                    var qubject = ctx.GetStream<int, int>(new Uri(Constants.Stream.FOO));
                    var o1 = qubject.CreateObserver();
                    var o2 = ((IReliableReactiveMultiSubject<int, int>)qubject).CreateObserver();
                },
                new CreateObserver(new Uri(Constants.Stream.FOO)),
                new CreateObserver(new Uri(Constants.Stream.FOO))
            );
        }
    }
}
