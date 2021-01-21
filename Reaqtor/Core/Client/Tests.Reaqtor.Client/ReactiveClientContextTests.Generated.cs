// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - August 2013 - Created this file.
//

using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

using Reaqtor;
using Reaqtor.TestingFramework;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Reaqtor.Client
{
    public partial class ReactiveClientContextTests
    {
        #region Client

        #region Observable

        [TestMethod]
        public void ReactiveClientContext_Client_Observable_NAry_2()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<int, bool, string>(new Uri(Constants.Observable.XS));
                    var ob = ctx.GetObserver<string>(new Uri(Constants.Observer.OB));
                    xs(1, false).SubscribeAsync(ob, new Uri(Constants.Subscription.SUB), null).Wait();
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<string>, IAsyncReactiveQbserver<string>, Task<IAsyncReactiveQubscription>>), Constants.SubscribeUri),
                        Expression.Invoke(
                            Expression.Parameter(typeof(Func<int, bool, IAsyncReactiveQbservable<string>>), Constants.Observable.XS),
                            Expression.Constant(1),
                            Expression.Constant(false)
                        ),
                        Expression.Parameter(typeof(IAsyncReactiveQbserver<string>), Constants.Observer.OB)
                    ),
                    null
                )
            );
        }

        #endregion

        #region Observer

        [TestMethod]
        public void ReactiveClientContext_Client_Observer_NAry_2()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<string>(new Uri(Constants.Observable.XS));
                    var ob = ctx.GetObserver<int, bool, string>(new Uri(Constants.Observer.OB));
                    xs.SubscribeAsync(ob(1, false), new Uri(Constants.Subscription.SUB), null).Wait();
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<string>, IAsyncReactiveQbserver<string>, Task<IAsyncReactiveQubscription>>), Constants.SubscribeUri),
                        Expression.Parameter(typeof(IAsyncReactiveQbservable<string>), Constants.Observable.XS),
                        Expression.Invoke(
                            Expression.Parameter(typeof(Func<int, bool, IAsyncReactiveQbserver<string>>), Constants.Observer.OB),
                            Expression.Constant(1),
                            Expression.Constant(false)
                        )
                    ),
                    null
                )
            );
        }

        #endregion

        #region StreamFactory

        [TestMethod]
        public void ReactiveClientContext_Client_StreamFactory_NAry_2()
        {
            Apply(
                ctx =>
                {
                    var sf = ctx.GetStreamFactory<int, bool, string, string>(new Uri(Constants.StreamFactory.SF));
                    sf.CreateAsync(new Uri(Constants.Stream.BAR), 1, false, null).Wait();
                },
                new CreateStream(
                    new Uri(Constants.Stream.BAR),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<int, bool, IAsyncReactiveQubject<string, string>>), Constants.StreamFactory.SF),
                        Expression.Constant(1),
                        Expression.Constant(false)
                    ),
                    null
                )
            );
        }

        #endregion

        #region SubscriptionFactory

        [TestMethod]
        public void ReactiveClientContext_Client_SubscriptionFactory_NAry_2()
        {
            Apply(
                ctx =>
                {
                    var sf = ctx.GetSubscriptionFactory<int, bool>(new Uri(Constants.SubscriptionFactory.SF));
                    sf.CreateAsync(new Uri(Constants.Subscription.SUB), 1, false, null).Wait();
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<int, bool, IAsyncReactiveQubscription>), Constants.SubscriptionFactory.SF),
                        Expression.Constant(1),
                        Expression.Constant(false)
                    ),
                    null
                )
            );
        }

        #endregion

        #endregion

        #region Definition

        #region Observable

        [TestMethod]
        public void ReactiveClientContext_Definition_Observable_NAry_2()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<int, bool, string>(new Uri(Constants.Observable.XS));
                    ctx.DefineObservableAsync<bool, int, string>(new Uri(Constants.Observable.YS), (a, b) => xs(b, a), null, CancellationToken.None).Wait();
                },
                new DefineObservable(
                    new Uri(Constants.Observable.YS),
                    Expression.Parameter(typeof(bool), "a").Let(a =>
                        Expression.Parameter(typeof(int), "b").Let(b =>
                            Expression.Lambda(
                                Expression.Invoke(
                                    Expression.Parameter(typeof(Func<int, bool, IAsyncReactiveQbservable<string>>), Constants.Observable.XS),
                                    b, a
                                ),
                                a, b
                            )
                        )
                    ),
                    null
                )
            );
        }

        #endregion

        #region Observer

        [TestMethod]
        public void ReactiveClientContext_Definition_Observer_NAry_2()
        {
            Apply(
                ctx =>
                {
                    var ob = ctx.GetObserver<int, bool, string>(new Uri(Constants.Observer.OB1));
                    ctx.DefineObserverAsync<bool, int, string>(new Uri(Constants.Observer.OB2), (a, b) => ob(b, a), null, CancellationToken.None).Wait();
                },
                new DefineObserver(
                    new Uri(Constants.Observer.OB2),
                    Expression.Parameter(typeof(bool), "a").Let(a =>
                        Expression.Parameter(typeof(int), "b").Let(b =>
                            Expression.Lambda(
                                Expression.Invoke(
                                    Expression.Parameter(typeof(Func<int, bool, IAsyncReactiveQbserver<string>>), Constants.Observer.OB1),
                                    b, a
                                ),
                                a, b
                            )
                        )
                    ),
                    null
                )
            );
        }

        #endregion

        #region StreamFactory

        // TODO

        #endregion

        #region SubscriptionFactory

        // TODO

        #endregion

        #endregion
    }
}
