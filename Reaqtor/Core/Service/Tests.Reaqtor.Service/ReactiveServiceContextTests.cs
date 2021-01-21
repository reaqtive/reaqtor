// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD, ER - October 2013 - Created this file.
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.CompilerServices;
using System.Linq.Expressions;
using System.Reflection;

using Reaqtor;
using Reaqtor.Metadata;
using Reaqtor.TestingFramework;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Reaqtor.Service
{
    [TestClass]
    public partial class ReactiveServiceContextTests : ReactiveServiceContextTestBase
    {
        #region Argument Checking

        [TestMethod]
        public void ReactiveServiceContext_Qbserver_ArgumentChecking()
        {
            Apply(
                ctx =>
                {
                    var observer = ctx.GetObserver<int>(new Uri(Constants.Observer.OB));

                    Assert.ThrowsException<ArgumentNullException>(() => observer.OnError(null));
                }
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_Qbservable_ArgumentChecking()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<int>(new Uri(Constants.Observable.XS));
                    var ob = ctx.GetObserver<int>(new Uri(Constants.Observer.OB));

                    Assert.ThrowsException<ArgumentNullException>(() => ((IReactiveObservable<int>)xs).Subscribe(null, new Uri(Constants.Subscription.SUB), null));
                    Assert.ThrowsException<ArgumentNullException>(() => ((IReactiveObservable<int>)xs).Subscribe(ob, null, null));
                }
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_QubjectFactory_ArgumentChecking()
        {
            Apply(
                ctx =>
                {
                    var factory = ctx.GetStreamFactory<int, int>(new Uri(Constants.StreamFactory.SF));
                    var pfactory = ctx.GetStreamFactory<string, int, int>(new Uri(Constants.StreamFactory.SG));

                    Assert.ThrowsException<ArgumentNullException>(() => factory.Create(null, null));
                    Assert.ThrowsException<ArgumentNullException>(() => ((IReactiveSubjectFactory<int, int>)factory).Create(null, null));

                    Assert.ThrowsException<ArgumentNullException>(() => pfactory.Create(null, "factory_parameter_1", null));
                    Assert.ThrowsException<ArgumentNullException>(() => ((IReactiveSubjectFactory<int, int, string>)pfactory).Create(null, "factory_parameter_1", null));
                }
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_ReactiveQueryProvider_ArgumentChecking_CreateQbservable()
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
        public void ReactiveServiceContext_ReactiveQueryProvider_ArgumentChecking_CreateQbserver()
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
        public void ReactiveServiceContext_ReactiveQueryProvider_ArgumentChecking_CreateQubject()
        {
            Apply(
                ctx =>
                {
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.Provider.CreateQubject<int, int>(null));
                }
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_ReactiveQueryProvider_ArgumentChecking_CreateQubjectFactory()
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
        public void ReactiveServiceContext_ReactiveQueryProvider_ArgumentChecking_CreateQubscription()
        {
            Apply(
                ctx =>
                {
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.Provider.CreateQubscription(null));
                }
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_ReactiveClient_ArgumentChecking_GetObservable()
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
        public void ReactiveServiceContext_ReactiveClient_ArgumentChecking_GetObserver()
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
        public void ReactiveServiceContext_ReactiveClient_ArgumentChecking_GetSubscription()
        {
            Apply(
                ctx =>
                {
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.GetSubscription(null));
                }
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_ReactiveClient_ArgumentChecking_GetStream()
        {
            Apply(
                ctx =>
                {
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.GetStream<int, int>(null));
                }
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_ReactiveClient_ArgumentChecking_GetStreamFactory()
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
        public void ReactiveServiceContext_ReactiveDefinition_ArgumentChecking_DefineObservable()
        {
            Apply(
                ctx =>
                {
                    var dummyUri = new Uri("io:/dummy");
                    var dummyQbservable = ctx.Provider.CreateQbservable<int>(Expression.Default(typeof(IReactiveQbservable<int>)));
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.DefineObservable<int>(null, dummyQbservable, null));
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.DefineObservable<int>(dummyUri, null, null));
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.DefineObservable<int, int>(null, _ => dummyQbservable, null));
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.DefineObservable<int, int>(dummyUri, null, null));
                }
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_ReactiveDefinition_ArgumentChecking_DefineObserver()
        {
            Apply(
                ctx =>
                {
                    var dummyUri = new Uri("iv:/dummy");
                    var dummyQbserver = ctx.Provider.CreateQbserver<int>(Expression.Default(typeof(IReactiveQbserver<int>)));
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.DefineObserver<int>(null, dummyQbserver, null));
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.DefineObserver<int>(dummyUri, null, null));
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.DefineObserver<int, int>(null, _ => dummyQbserver, null));
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.DefineObserver<int, int>(dummyUri, null, null));
                }
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_ReactiveDefinition_ArgumentChecking_DefineStreamFactory()
        {
            Apply(
                ctx =>
                {
                    var dummyUri = new Uri("iv:/dummy");
                    var dummyStreamFactory = ctx.Provider.CreateQubjectFactory<int, int>(Expression.Default(typeof(IReactiveQubjectFactory<int, int>)));
                    var paramDummyStreamFactory = ctx.Provider.CreateQubjectFactory<int, int, int>(Expression.Default(typeof(IReactiveQubjectFactory<int, int, int>)));
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.DefineStreamFactory<int, int>(null, dummyStreamFactory, null));
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.DefineStreamFactory<int, int>(dummyUri, null, null));
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.DefineStreamFactory<int, int, int>(null, paramDummyStreamFactory, null));
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.DefineStreamFactory<int, int, int>(dummyUri, null, null));
                }
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_ReactiveDefinition_ArgumentChecking_UndefineObservable()
        {
            Apply(
                ctx =>
                {
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.UndefineObservable(null));
                }
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_ReactiveDefinition_ArgumentChecking_UndefineObserver()
        {
            Apply(
                ctx =>
                {
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.UndefineObserver(null));
                }
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_ReactiveDefinition_ArgumentChecking_UndefineStreamFactory()
        {
            Apply(
                ctx =>
                {
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.UndefineStreamFactory(null));
                }
            );
        }

        #endregion

        #region Subscribe

        [TestMethod]
        public void ReactiveServiceContext_Subscribe_Simple1()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<int>(new Uri(Constants.Observable.XS));
                    var ob = ctx.GetObserver<int>(new Uri(Constants.Observer.OB));
                    xs.Subscribe(ob, new Uri(Constants.Subscription.SUB), null);
                    ((IReactiveObservable<int>)xs).Subscribe(ob, new Uri(Constants.Subscription.SUB), null);
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<IReactiveQbservable<int>, IReactiveQbserver<int>, IReactiveQubscription>), Constants.SubscribeUri),
                        Expression.Parameter(typeof(IReactiveQbservable<int>), Constants.Observable.XS),
                        Expression.Parameter(typeof(IReactiveQbserver<int>), Constants.Observer.OB)
                    ),
                    null
                ),
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<IReactiveQbservable<int>, IReactiveQbserver<int>, IReactiveQubscription>), Constants.SubscribeUri),
                        Expression.Parameter(typeof(IReactiveQbservable<int>), Constants.Observable.XS),
                        Expression.Parameter(typeof(IReactiveQbserver<int>), Constants.Observer.OB)
                    ),
                    null
                )
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_Subscribe_Simple2()
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
                            Expression.Parameter(typeof(Func<IReactiveQbservable<int>, IReactiveQbserver<int>, IReactiveQubscription>), Constants.SubscribeUri),
                            Expression.Invoke(
                                Expression.Parameter(typeof(Func<IReactiveQbservable<int>, Expression<Func<int, int>>, IReactiveQbservable<int>>), Constants.Observable.Select),
                                Expression.Invoke(
                                    Expression.Parameter(typeof(Func<IReactiveQbservable<int>, Expression<Func<int, bool>>, IReactiveQbservable<int>>), Constants.Observable.Where),
                                    Expression.Parameter(typeof(IReactiveQbservable<int>), Constants.Observable.XS),
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
                            Expression.Parameter(typeof(IReactiveQbserver<int>), Constants.Observer.OB)
                        )
                    ),
                    null
                )
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_Subscribe_Simple4()
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
                        Expression.Parameter(typeof(Func<IReactiveQbservable<long>, IReactiveQbserver<long>, IReactiveQubscription>), Constants.SubscribeUri),
                        Expression.Invoke(
                            Expression.Parameter(typeof(Func<IReactiveQbservable<long>, Expression<Func<long, IReactiveQbservable<long>>>, IReactiveQbservable<long>>), Constants.Observable.Bind),
                            Expression.Parameter(typeof(IReactiveQbservable<long>), Constants.Observable.XS),
                            Expression.Lambda<Func<long, IReactiveQbservable<long>>>(
                                Expression.Invoke(
                                    Expression.Parameter(typeof(Func<TimeSpan, IReactiveQbservable<long>>), Constants.Observable.Timer),
                                    Expression.New(typeof(TimeSpan).GetConstructor(new[] { typeof(long) }), longParam)
                                ),
                                longParam
                            )
                        ),
                        Expression.Parameter(typeof(IReactiveQbserver<long>), Constants.Observer.OB)
                    ),
                    null
                )
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_Subscribe_Simple3()
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
                        Expression.Parameter(typeof(Func<IReactiveQbservable<int>, IReactiveQbserver<int>, IReactiveQubscription>), Constants.SubscribeUri),
                        Expression.Parameter(typeof(IReactiveQbservable<int>), Constants.Observable.XS),
                        Expression.Parameter(typeof(IReactiveQbserver<int>), Constants.Observer.OB)
                    ),
                    null
                )
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_Subscribe_NAry_1()
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
                        Expression.Parameter(typeof(Func<IReactiveQbservable<string>, IReactiveQbserver<string>, IReactiveQubscription>), Constants.SubscribeUri),
                        Expression.Invoke(
                            Expression.Parameter(typeof(Func<int, IReactiveQbservable<string>>), Constants.Observable.XS),
                            Expression.Constant(1)
                        ),
                        Expression.Invoke(
                            Expression.Parameter(typeof(Func<int, IReactiveQbserver<string>>), Constants.Observer.OB),
                            Expression.Constant(-1)
                        )
                    ),
                    null
                )
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_Subscribe_NAry_2()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<int, bool, string>(new Uri(Constants.Observable.XS));
                    var ob = ctx.GetObserver<bool, int, string>(new Uri(Constants.Observer.OB));
                    xs(1, true).Subscribe(ob(false, -1), new Uri(Constants.Subscription.SUB), null);
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<IReactiveQbservable<string>, IReactiveQbserver<string>, IReactiveQubscription>), Constants.SubscribeUri),
                        Expression.Invoke(
                            Expression.Parameter(typeof(Func<int, bool, IReactiveQbservable<string>>), Constants.Observable.XS),
                            Expression.Constant(1),
                            Expression.Constant(true)
                        ),
                        Expression.Invoke(
                            Expression.Parameter(typeof(Func<bool, int, IReactiveQbserver<string>>), Constants.Observer.OB),
                            Expression.Constant(false),
                            Expression.Constant(-1)
                        )
                    ),
                    null
                )
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_Subscribe_Closure1()
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
                                Expression.Parameter(typeof(Func<IReactiveQbservable<int>, IReactiveQbserver<int>, IReactiveQubscription>), Constants.SubscribeUri),
                                Expression.Invoke(
                                    Expression.Parameter(typeof(Func<IReactiveQbservable<int>, Expression<Func<int, int>>, IReactiveQbservable<int>>), Constants.Observable.Select),
                                    Expression.Invoke(
                                        Expression.Parameter(typeof(Func<IReactiveQbservable<int>, Expression<Func<int, bool>>, IReactiveQbservable<int>>), Constants.Observable.Where),
                                        Expression.Parameter(typeof(IReactiveQbservable<int>), Constants.Observable.XS),
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
                                Expression.Parameter(typeof(IReactiveQbserver<int>), Constants.Observer.OB)
                            )
                        ),
                        null
                    )
                ).ToArray()
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_Subscribe_Parameterized()
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
                            Expression.Parameter(typeof(Func<IReactiveQbservable<int>, IReactiveQbserver<int>, IReactiveQubscription>), Constants.SubscribeUri),
                            Expression.Invoke(
                                Expression.Parameter(typeof(Func<IReactiveQbservable<int>, Expression<Func<int, int>>, IReactiveQbservable<int>>), Constants.Observable.Select),
                                Expression.Invoke(
                                    Expression.Parameter(typeof(Func<IReactiveQbservable<int>, Expression<Func<int, bool>>, IReactiveQbservable<int>>), Constants.Observable.Where),
                                    Expression.Invoke(
                                        Expression.Parameter(typeof(Func<string, IReactiveQbservable<int>>), Constants.Observable.XS),
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
                            Expression.Parameter(typeof(IReactiveQbserver<int>), Constants.Observer.OB)
                        )
                    ),
                    null
                )
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_Subscribe_SelectMany1_Simple()
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
                                Expression.Parameter(typeof(Func<IReactiveQbservable<int>, IReactiveQbserver<int>, IReactiveQubscription>), Constants.SubscribeUri),
                                Expression.Invoke(
                                    Expression.Parameter(typeof(Func<IReactiveQbservable<int>, Expression<Func<int, IReactiveQbservable<string>>>, Expression<Func<int, string, int>>, IReactiveQbservable<int>>), Constants.Observable.SelectMany),
                                    Expression.Parameter(typeof(IReactiveQbservable<int>), Constants.Observable.XS),
                                    Expression.Lambda(
                                        Expression.Invoke(
                                            Expression.Parameter(typeof(Func<int, IReactiveQbservable<string>>), Constants.Observable.YS),
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
                                Expression.Parameter(typeof(IReactiveQbserver<int>), Constants.Observer.OB)
                            )
                        )
                    ),
                    null
                )
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_Subscribe_SelectMany2_Closure()
        {
            var xs = default(IReactiveQbservable<int>);
            var ys = default(Func<int, IReactiveQbservable<string>>);

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
                                Expression.Parameter(typeof(Func<IReactiveQbservable<int>, IReactiveQbserver<int>, IReactiveQubscription>), Constants.SubscribeUri),
                                Expression.Invoke(
                                    Expression.Parameter(typeof(Func<IReactiveQbservable<int>, Expression<Func<int, IReactiveQbservable<string>>>, Expression<Func<int, string, int>>, IReactiveQbservable<int>>), Constants.Observable.SelectMany),
                                    Expression.Parameter(typeof(IReactiveQbservable<int>), Constants.Observable.XS),
                                    Expression.Lambda(
                                        Expression.Invoke(
                                            Expression.Parameter(typeof(Func<int, IReactiveQbservable<string>>), Constants.Observable.YS),
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
                                Expression.Parameter(typeof(IReactiveQbserver<int>), Constants.Observer.OB)
                            )
                        )
                    ),
                    null
                )
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_Subscribe_SelectMany3_CustomContext()
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
                                Expression.Parameter(typeof(Func<IReactiveQbservable<int>, IReactiveQbserver<int>, IReactiveQubscription>), Constants.SubscribeUri),
                                Expression.Invoke(
                                    Expression.Parameter(typeof(Func<IReactiveQbservable<int>, Expression<Func<int, IReactiveQbservable<int>>>, Expression<Func<int, int, int>>, IReactiveQbservable<int>>), Constants.Observable.SelectMany),
                                    Expression.Parameter(typeof(IReactiveQbservable<int>), Constants.Observable.XS),
                                    Expression.Lambda(
                                        Expression.Parameter(typeof(IReactiveQbservable<int>), Constants.Observable.XS),
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
                                Expression.Parameter(typeof(IReactiveQbserver<int>), Constants.Observer.OB)
                            )
                        )
                    ),
                    null
                )
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_Subscribe_SelectMany2_Inlined()
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
                                Expression.Parameter(typeof(Func<IReactiveQbservable<int>, IReactiveQbserver<int>, IReactiveQubscription>), Constants.SubscribeUri),
                                Expression.Invoke(
                                    Expression.Parameter(typeof(Func<IReactiveQbservable<int>, Expression<Func<int, IReactiveQbservable<string>>>, Expression<Func<int, string, int>>, IReactiveQbservable<int>>), Constants.Observable.SelectMany),
                                    Expression.Parameter(typeof(IReactiveQbservable<int>), Constants.Observable.XS),
                                    Expression.Lambda(
                                        Expression.Invoke(
                                            Expression.Parameter(typeof(Func<int, IReactiveQbservable<string>>), Constants.Observable.YS),
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
                                Expression.Parameter(typeof(IReactiveQbserver<int>), Constants.Observer.OB)
                            )
                        )
                    ),
                    null
                )
            );
        }

        #endregion

        #region GetStreamFactory

        [TestMethod]
        public void ReactiveServiceContext_GetStreamFactory()
        {
            Apply(
                ctx =>
                {
                    var factory = ctx.GetStreamFactory<int, int>(new Uri(Constants.StreamFactory.SF));
                    ctx.DefineStreamFactory<int, int>(new Uri(Constants.StreamFactory.SG), factory, null);
                    factory.Create(new Uri(Constants.Stream.FOO), null);
                },
                new DefineStreamFactory(
                    new Uri(Constants.StreamFactory.SG),
                    Expression.Parameter(typeof(Func<IReactiveQubject<int, int>>), Constants.StreamFactory.SF),
                    null
                ),
                new CreateStream(
                    new Uri(Constants.Stream.FOO),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<IReactiveQubject<int, int>>), Constants.StreamFactory.SF)
                    ),
                    null
                )
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_GetStreamFactory_Parameterized()
        {
            var ex = new Exception();

            Apply(
                ctx =>
                {
                    var factory = ctx.GetStreamFactory<string, int, int>(new Uri(Constants.StreamFactory.SF));
                    ctx.DefineStreamFactory<string, int, int>(new Uri(Constants.StreamFactory.SG), factory, null);
                    var qubject = factory.Create(new Uri(Constants.Stream.FOO), "factory_parameter_1", null);
                    qubject.OnError(ex);
                    qubject = factory.Create(new Uri(Constants.Stream.BAR), "factory_parameter_1", null);
                    qubject.OnCompleted();
                },
                new DefineStreamFactory(
                    new Uri(Constants.StreamFactory.SG),
                    Expression.Parameter(typeof(Func<string, IReactiveQubject<int, int>>), Constants.StreamFactory.SF),
                    null
                ),
                new CreateStream(
                    new Uri(Constants.Stream.FOO),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<string, IReactiveQubject<int, int>>), Constants.StreamFactory.SF),
                        Expression.Constant("factory_parameter_1")
                    ),
                    null
                ),
                new ObserverOnError(new Uri(Constants.Stream.FOO), ex),
                new CreateStream(
                    new Uri(Constants.Stream.BAR),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<string, IReactiveQubject<int, int>>), Constants.StreamFactory.SF),
                        Expression.Constant("factory_parameter_1")
                    ),
                    null
                ),
                new ObserverOnCompleted(new Uri(Constants.Stream.BAR))
            );
        }

        #endregion

        #region GetSubscriptionFactory

        [TestMethod]
        public void ReactiveServiceContext_GetSubscriptionFactory()
        {
            Apply(
                ctx =>
                {
                    var factory = ctx.GetSubscriptionFactory(new Uri(Constants.SubscriptionFactory.SF));
                    ctx.DefineSubscriptionFactory(new Uri(Constants.SubscriptionFactory.SG), factory, null);
                    factory.Create(new Uri(Constants.Subscription.SUB), null);
                },
                new DefineSubscriptionFactory(
                    new Uri(Constants.SubscriptionFactory.SG),
                    Expression.Parameter(typeof(Func<IReactiveQubscription>), Constants.SubscriptionFactory.SF),
                    null
                ),
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<IReactiveQubscription>), Constants.SubscriptionFactory.SF)
                    ),
                    null
                )
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_GetSubscriptionFactory_Parameterized()
        {
            var ex = new Exception();

            Apply(
                ctx =>
                {
                    var factory = ctx.GetSubscriptionFactory<string>(new Uri(Constants.SubscriptionFactory.SF));
                    ctx.DefineSubscriptionFactory<string>(new Uri(Constants.SubscriptionFactory.SG), factory, null);
                    factory.Create(new Uri(Constants.Subscription.SUB1), "factory_parameter_1", null);
                    factory.Create(new Uri(Constants.Subscription.SUB2), "factory_parameter_1", null);
                },
                new DefineSubscriptionFactory(
                    new Uri(Constants.SubscriptionFactory.SG),
                    Expression.Parameter(typeof(Func<string, IReactiveQubscription>), Constants.SubscriptionFactory.SF),
                    null
                ),
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB1),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<string, IReactiveQubscription>), Constants.SubscriptionFactory.SF),
                        Expression.Constant("factory_parameter_1")
                    ),
                    null
                ),
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB2),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<string, IReactiveQubscription>), Constants.SubscriptionFactory.SF),
                        Expression.Constant("factory_parameter_1")
                    ),
                    null
                )
            );
        }

        #endregion

        #region GetObserver

        [TestMethod]
        public void ReactiveServiceContext_GetObserver()
        {
            var ex = new Exception();

            Apply(
                ctx =>
                {
                    var observer = ctx.GetObserver<int>(new Uri(Constants.Observer.OB1));
                    observer.OnNext(42);
                    observer.OnError(ex);
                    observer = ctx.GetObserver<int>(new Uri(Constants.Observer.OB2));
                    observer.OnCompleted();
                },
                new ObserverOnNext(new Uri(Constants.Observer.OB1), 42),
                new ObserverOnError(new Uri(Constants.Observer.OB1), ex),
                new ObserverOnCompleted(new Uri(Constants.Observer.OB2))
            );
        }

        #endregion

        #region Metadata

        [TestMethod]
        public void ReactiveServiceContext_Metadata_Observables()
        {
            Apply(
                ctx =>
                {
                    var all = ctx.Observables.ToList();
                },
                new MetadataQuery(
                    Expression.Parameter(typeof(IQueryableDictionary<Uri, IReactiveObservableDefinition>), "rx://metadata/observables")
                )
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_Metadata_Observers()
        {
            Apply(
                ctx =>
                {
                    var all = ctx.Observers.ToList();
                },
                new MetadataQuery(
                    Expression.Parameter(typeof(IQueryableDictionary<Uri, IReactiveObserverDefinition>), "rx://metadata/observers")
                )
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_Metadata_StreamFactories()
        {
            Apply(
                ctx =>
                {
                    var all = ctx.StreamFactories.ToList();
                },
                new MetadataQuery(
                    Expression.Parameter(typeof(IQueryableDictionary<Uri, IReactiveStreamFactoryDefinition>), "rx://metadata/streamFactories")
                )
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_Metadata_SubscriptionFactories()
        {
            Apply(
                ctx =>
                {
                    var all = ctx.SubscriptionFactories.ToList();
                },
                new MetadataQuery(
                    Expression.Parameter(typeof(IQueryableDictionary<Uri, IReactiveSubscriptionFactoryDefinition>), "rx://metadata/subscriptionFactories")
                )
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_Metadata_Subscriptions()
        {
            Apply(
                ctx =>
                {
                    var all = ctx.Subscriptions.ToList();
                },
                new MetadataQuery(
                    Expression.Parameter(typeof(IQueryableDictionary<Uri, IReactiveSubscriptionProcess>), "rx://metadata/subscriptions")
                )
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_Metadata_Streams()
        {
            Apply(
                ctx =>
                {
                    var all = ctx.Streams.ToList();
                },
                new MetadataQuery(
                    Expression.Parameter(typeof(IQueryableDictionary<Uri, IReactiveStreamProcess>), "rx://metadata/streams")
                )
            );
        }

#pragma warning disable IDE0034 // Simplify 'default' expression (documents the signature)
        private static readonly MethodInfo s_where = ((MethodInfo)ReflectionHelpers.InfoOf(() => Queryable.Where<object>(default(IQueryable<object>), default(Expression<Func<object, bool>>)))).GetGenericMethodDefinition();
        private static readonly MethodInfo s_contains = ((MethodInfo)ReflectionHelpers.InfoOf(() => Queryable.Contains<object>(default(IQueryable<object>), default(object)))).GetGenericMethodDefinition();
#pragma warning restore IDE0034 // Simplify 'default' expression

        [TestMethod]
        public void ReactiveServiceContext_Metadata_Observables_Query()
        {
            Apply(
                ctx =>
                {
                    var all = ctx.Observables.Where(kv => kv.Key == new Uri("bar://foo")).ToList();
                },
                new MetadataQuery(
                    Expression.Call(
                        s_where.MakeGenericMethod(typeof(KeyValuePair<Uri, IReactiveObservableDefinition>)),
                        Expression.Parameter(typeof(IQueryableDictionary<Uri, IReactiveObservableDefinition>), "rx://metadata/observables"),
                        Expression.Parameter(typeof(KeyValuePair<Uri, IReactiveObservableDefinition>), "kv").Let(kv =>
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
        public void ReactiveServiceContext_Metadata_Observables_Query_NonGeneric()
        {
            Apply(
                ctx =>
                {
                    var obs = ctx.Observables;
                    var qry = obs.Where(kv => kv.Key == new Uri("bar://foo"));
                    var res = (IQueryable<KeyValuePair<Uri, IReactiveObservableDefinition>>)obs.Provider.CreateQuery(qry.Expression);
                    var all = res.ToList();
                },
                new MetadataQuery(
                    Expression.Call(
                        s_where.MakeGenericMethod(typeof(KeyValuePair<Uri, IReactiveObservableDefinition>)),
                        Expression.Parameter(typeof(IQueryableDictionary<Uri, IReactiveObservableDefinition>), "rx://metadata/observables"),
                        Expression.Parameter(typeof(KeyValuePair<Uri, IReactiveObservableDefinition>), "kv").Let(kv =>
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

        private static readonly MethodInfo s_count = ((MethodInfo)ReflectionHelpers.InfoOf(() => Queryable.Count<object>(default))).GetGenericMethodDefinition();

        [TestMethod]
        public void ReactiveServiceContext_Metadata_Observables_Query_Aggregate()
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
                        s_count.MakeGenericMethod(typeof(KeyValuePair<Uri, IReactiveObservableDefinition>)),
                        Expression.Parameter(typeof(IQueryableDictionary<Uri, IReactiveObservableDefinition>), "rx://metadata/observables")
                    )
                )
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_Metadata_Observables_Query_Aggregate_NonGeneric()
        {
            Apply(
                ctx =>
                {
                    var exp = Expression.Call(
                        s_count.MakeGenericMethod(typeof(KeyValuePair<Uri, IReactiveObservableDefinition>)),
                        ctx.Observables.Expression
                    );

                    var n = (int)ctx.Observables.Provider.Execute(exp);
                },
                new MetadataQuery(
                    Expression.Call(
                        s_count.MakeGenericMethod(typeof(KeyValuePair<Uri, IReactiveObservableDefinition>)),
                        Expression.Parameter(typeof(IQueryableDictionary<Uri, IReactiveObservableDefinition>), "rx://metadata/observables")
                    )
                )
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_Metadata_Provider_ArgumentChecking()
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
        public void ReactiveServiceContext_Metadata_Join_Query()
        {
            Apply(
                ctx =>
                {
                    var all = ctx.Observables.Where(kv => ctx.Observers.Keys.Contains(kv.Key)).ToList();
                },
                new MetadataQuery(
                    Expression.Call(
                        s_where.MakeGenericMethod(typeof(KeyValuePair<Uri, IReactiveObservableDefinition>)),
                        Expression.Parameter(typeof(IQueryableDictionary<Uri, IReactiveObservableDefinition>), "rx://metadata/observables"),
                        Expression.Parameter(typeof(IQueryableDictionary<Uri, IReactiveObserverDefinition>), "rx://metadata/observers").Let(observers =>
                            Expression.Parameter(typeof(KeyValuePair<Uri, IReactiveObservableDefinition>), "kv").Let(kv =>
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
        public void ReactiveServiceContext_Metadata_Join_Query_WithUnboundContext()
        {
            // This test simply asserts that we do not rewrite the collections to their expression form if the expression contains unbound parameters.
            Expression<Func<IQueryableDictionary<Uri, IReactiveObservableDefinition>, IQueryable<KeyValuePair<Uri, IReactiveObservableDefinition>>>> f =
                observables => observables.Where(kv => Foo(kv.Key.AbsoluteUri).Observers.Keys.Contains(kv.Key));

            var expression = BetaReducer.Reduce(
                Expression.Invoke(
                    f,
                    Expression.Parameter(
                        typeof(IQueryableDictionary<Uri, IReactiveObservableDefinition>),
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
        public void ReactiveServiceContext_Metadata_Join_Query_WithNonInterfaceProperty()
        {
            // This test simply asserts that we do not rewrite the collections to their expression form if the expression contains unbound parameters.
            Expression<Func<ReactiveServiceContext, IQueryableDictionary<Uri, IReactiveObservableDefinition>, IQueryable<KeyValuePair<Uri, IReactiveObservableDefinition>>>> f =
                (ctx, observables) => observables.Where(kv => ctx.Provider.ToString() == kv.Key.AbsoluteUri);

            var expression = BetaReducer.Reduce(
                Expression.Invoke(
                    f,
                    Expression.Parameter(
                        typeof(TestServiceContext),
                        "rx://builtin/this"
                    ),
                    Expression.Parameter(
                        typeof(IQueryableDictionary<Uri, IReactiveObservableDefinition>),
                        "rx://metadata/observables"
                    )
                )
            );

            Apply(
                ctx =>
                {
                    // This query is gibberish, but `Provider` is just some property that
                    // doesn't exist on the `IReactiveMetadata` interface.
                    _ = ctx.Observables.Where(kv => ctx.Provider.ToString() == kv.Key.AbsoluteUri).ToList();
                },
                new MetadataQuery(expression)
            );
        }

        private static ReactiveServiceContext Foo(string arg)
        {
            _ = arg;
            return null;
        }

        #endregion
    }
}
