// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// RB - July 2013
// ER - August 2013 - Created this file.
// Auto-generated file, changes to source may be lost
//

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using Reaqtor;
using Reaqtor.TestingFramework;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Reaqtor.Client
{
    [TestClass]
    public class ReactiveClientContextHigherArityTests : ReactiveClientContextTestBase
    {
        #region "ReactiveClientContext_AsyncReactiveQueryProvider_ArgumentChecking_HigherArity"

        [TestMethod]
        public void ReactiveClientContext_AsyncReactiveQueryProvider_ArgumentChecking_HigherArity_CreateQbservable()
        {
            Apply(
                ctx =>
                {
                    var provider = (AsyncReactiveQueryProvider)ctx.Provider;
                    Assert.ThrowsException<ArgumentNullException>(() => provider.CreateQbservable<int, int, int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => provider.CreateQbservable<int, int, int, int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => provider.CreateQbservable<int, int, int, int, int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => provider.CreateQbservable<int, int, int, int, int, int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => provider.CreateQbservable<int, int, int, int, int, int, int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => provider.CreateQbservable<int, int, int, int, int, int, int, int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => provider.CreateQbservable<int, int, int, int, int, int, int, int, int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => provider.CreateQbservable<int, int, int, int, int, int, int, int, int, int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => provider.CreateQbservable<int, int, int, int, int, int, int, int, int, int, int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => provider.CreateQbservable<int, int, int, int, int, int, int, int, int, int, int, int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => provider.CreateQbservable<int, int, int, int, int, int, int, int, int, int, int, int, int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => provider.CreateQbservable<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => provider.CreateQbservable<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => provider.CreateQbservable<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(null));
                }
            );
        }

        [TestMethod]
        public void ReactiveClientContext_AsyncReactiveQueryProvider_ArgumentChecking_HigherArity_CreateQbserver()
        {
            Apply(
                ctx =>
                {
                    var provider = (AsyncReactiveQueryProvider)ctx.Provider;
                    Assert.ThrowsException<ArgumentNullException>(() => provider.CreateQbserver<int, int, int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => provider.CreateQbserver<int, int, int, int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => provider.CreateQbserver<int, int, int, int, int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => provider.CreateQbserver<int, int, int, int, int, int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => provider.CreateQbserver<int, int, int, int, int, int, int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => provider.CreateQbserver<int, int, int, int, int, int, int, int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => provider.CreateQbserver<int, int, int, int, int, int, int, int, int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => provider.CreateQbserver<int, int, int, int, int, int, int, int, int, int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => provider.CreateQbserver<int, int, int, int, int, int, int, int, int, int, int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => provider.CreateQbserver<int, int, int, int, int, int, int, int, int, int, int, int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => provider.CreateQbserver<int, int, int, int, int, int, int, int, int, int, int, int, int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => provider.CreateQbserver<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => provider.CreateQbserver<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => provider.CreateQbserver<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(null));
                }
            );
        }

        [TestMethod]
        public void ReactiveClientContext_AsyncReactiveQueryProvider_ArgumentChecking_HigherArity_CreateQubjectFactory()
        {
            Apply(
                ctx =>
                {
                    var provider = (AsyncReactiveQueryProvider)ctx.Provider;
                    Assert.ThrowsException<ArgumentNullException>(() => provider.CreateQubjectFactory<int, int, int, int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => provider.CreateQubjectFactory<int, int, int, int, int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => provider.CreateQubjectFactory<int, int, int, int, int, int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => provider.CreateQubjectFactory<int, int, int, int, int, int, int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => provider.CreateQubjectFactory<int, int, int, int, int, int, int, int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => provider.CreateQubjectFactory<int, int, int, int, int, int, int, int, int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => provider.CreateQubjectFactory<int, int, int, int, int, int, int, int, int, int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => provider.CreateQubjectFactory<int, int, int, int, int, int, int, int, int, int, int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => provider.CreateQubjectFactory<int, int, int, int, int, int, int, int, int, int, int, int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => provider.CreateQubjectFactory<int, int, int, int, int, int, int, int, int, int, int, int, int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => provider.CreateQubjectFactory<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => provider.CreateQubjectFactory<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => provider.CreateQubjectFactory<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => provider.CreateQubjectFactory<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(null));
                }
            );
        }

        [TestMethod]
        public void ReactiveClientContext_AsyncReactiveQueryProvider_ArgumentChecking_HigherArity_CreateQubscriptionFactory()
        {
            Apply(
                ctx =>
                {
                    var provider = (AsyncReactiveQueryProvider)ctx.Provider;
                    Assert.ThrowsException<ArgumentNullException>(() => provider.CreateQubscriptionFactory<int, int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => provider.CreateQubscriptionFactory<int, int, int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => provider.CreateQubscriptionFactory<int, int, int, int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => provider.CreateQubscriptionFactory<int, int, int, int, int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => provider.CreateQubscriptionFactory<int, int, int, int, int, int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => provider.CreateQubscriptionFactory<int, int, int, int, int, int, int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => provider.CreateQubscriptionFactory<int, int, int, int, int, int, int, int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => provider.CreateQubscriptionFactory<int, int, int, int, int, int, int, int, int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => provider.CreateQubscriptionFactory<int, int, int, int, int, int, int, int, int, int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => provider.CreateQubscriptionFactory<int, int, int, int, int, int, int, int, int, int, int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => provider.CreateQubscriptionFactory<int, int, int, int, int, int, int, int, int, int, int, int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => provider.CreateQubscriptionFactory<int, int, int, int, int, int, int, int, int, int, int, int, int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => provider.CreateQubscriptionFactory<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => provider.CreateQubscriptionFactory<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(null));
                }
            );
        }

        [TestMethod]
        public void ReactiveClientContext_AsyncReactiveQueryProvider_CreateQubjectFactory_HigherArity_UnknownArtifact()
        {
            Apply(
                ctx =>
                {
                    var provider = (AsyncReactiveQueryProvider)ctx.Provider;
                    Assert.IsFalse(provider.CreateQubscription(Expression.Default(typeof(IReactiveQubscription))) is IKnownResource);
                    Assert.IsFalse(provider.CreateQubjectFactory<int, int>(Expression.Default(typeof(IReactiveQubjectFactory<int, int>))) is IKnownResource);
                    Assert.IsFalse(provider.CreateQubjectFactory<int, int, int>(Expression.Default(typeof(IReactiveQubjectFactory<int, int, int>))) is IKnownResource);
                    Assert.IsFalse(provider.CreateQubjectFactory<int, int, int, int>(Expression.Default(typeof(IReactiveQubjectFactory<int, int, int, int>))) is IKnownResource);
                    Assert.IsFalse(provider.CreateQubjectFactory<int, int, int, int, int>(Expression.Default(typeof(IReactiveQubjectFactory<int, int, int, int, int>))) is IKnownResource);
                    Assert.IsFalse(provider.CreateQubjectFactory<int, int, int, int, int, int>(Expression.Default(typeof(IReactiveQubjectFactory<int, int, int, int, int, int>))) is IKnownResource);
                    Assert.IsFalse(provider.CreateQubjectFactory<int, int, int, int, int, int, int>(Expression.Default(typeof(IReactiveQubjectFactory<int, int, int, int, int, int, int>))) is IKnownResource);
                    Assert.IsFalse(provider.CreateQubjectFactory<int, int, int, int, int, int, int, int>(Expression.Default(typeof(IReactiveQubjectFactory<int, int, int, int, int, int, int, int>))) is IKnownResource);
                    Assert.IsFalse(provider.CreateQubjectFactory<int, int, int, int, int, int, int, int, int>(Expression.Default(typeof(IReactiveQubjectFactory<int, int, int, int, int, int, int, int, int>))) is IKnownResource);
                    Assert.IsFalse(provider.CreateQubjectFactory<int, int, int, int, int, int, int, int, int, int>(Expression.Default(typeof(IReactiveQubjectFactory<int, int, int, int, int, int, int, int, int, int>))) is IKnownResource);
                    Assert.IsFalse(provider.CreateQubjectFactory<int, int, int, int, int, int, int, int, int, int, int>(Expression.Default(typeof(IReactiveQubjectFactory<int, int, int, int, int, int, int, int, int, int, int>))) is IKnownResource);
                    Assert.IsFalse(provider.CreateQubjectFactory<int, int, int, int, int, int, int, int, int, int, int, int>(Expression.Default(typeof(IReactiveQubjectFactory<int, int, int, int, int, int, int, int, int, int, int, int>))) is IKnownResource);
                    Assert.IsFalse(provider.CreateQubjectFactory<int, int, int, int, int, int, int, int, int, int, int, int, int>(Expression.Default(typeof(IReactiveQubjectFactory<int, int, int, int, int, int, int, int, int, int, int, int, int>))) is IKnownResource);
                    Assert.IsFalse(provider.CreateQubjectFactory<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(Expression.Default(typeof(IReactiveQubjectFactory<int, int, int, int, int, int, int, int, int, int, int, int, int, int>))) is IKnownResource);
                    Assert.IsFalse(provider.CreateQubjectFactory<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(Expression.Default(typeof(IReactiveQubjectFactory<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>))) is IKnownResource);
                }
            );
        }

        [TestMethod]
        public void ReactiveClientContext_AsyncReactiveQueryProvider_CreateQubscriptionFactory_HigherArity_UnknownArtifact()
        {
            Apply(
                ctx =>
                {
                    var provider = (AsyncReactiveQueryProvider)ctx.Provider;
                    Assert.IsFalse(provider.CreateQubscription(Expression.Default(typeof(IReactiveQubscription))) is IKnownResource);
                    Assert.IsFalse(provider.CreateQubscriptionFactory<int, int>(Expression.Default(typeof(IReactiveQubscriptionFactory<int, int>))) is IKnownResource);
                    Assert.IsFalse(provider.CreateQubscriptionFactory<int, int, int>(Expression.Default(typeof(IReactiveQubscriptionFactory<int, int, int>))) is IKnownResource);
                    Assert.IsFalse(provider.CreateQubscriptionFactory<int, int, int, int>(Expression.Default(typeof(IReactiveQubscriptionFactory<int, int, int, int>))) is IKnownResource);
                    Assert.IsFalse(provider.CreateQubscriptionFactory<int, int, int, int, int>(Expression.Default(typeof(IReactiveQubscriptionFactory<int, int, int, int, int>))) is IKnownResource);
                    Assert.IsFalse(provider.CreateQubscriptionFactory<int, int, int, int, int, int>(Expression.Default(typeof(IReactiveQubscriptionFactory<int, int, int, int, int, int>))) is IKnownResource);
                    Assert.IsFalse(provider.CreateQubscriptionFactory<int, int, int, int, int, int, int>(Expression.Default(typeof(IReactiveQubscriptionFactory<int, int, int, int, int, int, int>))) is IKnownResource);
                    Assert.IsFalse(provider.CreateQubscriptionFactory<int, int, int, int, int, int, int, int>(Expression.Default(typeof(IReactiveQubscriptionFactory<int, int, int, int, int, int, int, int>))) is IKnownResource);
                    Assert.IsFalse(provider.CreateQubscriptionFactory<int, int, int, int, int, int, int, int, int>(Expression.Default(typeof(IReactiveQubscriptionFactory<int, int, int, int, int, int, int, int, int>))) is IKnownResource);
                    Assert.IsFalse(provider.CreateQubscriptionFactory<int, int, int, int, int, int, int, int, int, int>(Expression.Default(typeof(IReactiveQubscriptionFactory<int, int, int, int, int, int, int, int, int, int>))) is IKnownResource);
                    Assert.IsFalse(provider.CreateQubscriptionFactory<int, int, int, int, int, int, int, int, int, int, int>(Expression.Default(typeof(IReactiveQubscriptionFactory<int, int, int, int, int, int, int, int, int, int, int>))) is IKnownResource);
                    Assert.IsFalse(provider.CreateQubscriptionFactory<int, int, int, int, int, int, int, int, int, int, int, int>(Expression.Default(typeof(IReactiveQubscriptionFactory<int, int, int, int, int, int, int, int, int, int, int, int>))) is IKnownResource);
                    Assert.IsFalse(provider.CreateQubscriptionFactory<int, int, int, int, int, int, int, int, int, int, int, int, int>(Expression.Default(typeof(IReactiveQubscriptionFactory<int, int, int, int, int, int, int, int, int, int, int, int, int>))) is IKnownResource);
                    Assert.IsFalse(provider.CreateQubscriptionFactory<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(Expression.Default(typeof(IReactiveQubscriptionFactory<int, int, int, int, int, int, int, int, int, int, int, int, int, int>))) is IKnownResource);
                    Assert.IsFalse(provider.CreateQubscriptionFactory<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(Expression.Default(typeof(IReactiveQubscriptionFactory<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>))) is IKnownResource);
                }
            );
        }

        #endregion

        #region "ReactiveClientContext_ReactiveClientProxy_ArgumentChecking_HigherArity"

        [TestMethod]
        public void ReactiveClientContext_ReactiveClientProxy_ArgumentChecking_HigherArity_GetObservable()
        {
            Apply(
                ctx =>
                {
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.GetObservable<int, int, int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.GetObservable<int, int, int, int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.GetObservable<int, int, int, int, int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.GetObservable<int, int, int, int, int, int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.GetObservable<int, int, int, int, int, int, int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.GetObservable<int, int, int, int, int, int, int, int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.GetObservable<int, int, int, int, int, int, int, int, int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.GetObservable<int, int, int, int, int, int, int, int, int, int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.GetObservable<int, int, int, int, int, int, int, int, int, int, int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.GetObservable<int, int, int, int, int, int, int, int, int, int, int, int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.GetObservable<int, int, int, int, int, int, int, int, int, int, int, int, int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.GetObservable<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.GetObservable<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.GetObservable<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(null));
                }
            );
        }

        [TestMethod]
        public void ReactiveClientContext_ReactiveClientProxy_ArgumentChecking_HigherArity_GetObserver()
        {
            Apply(
                ctx =>
                {
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.GetObserver<int, int, int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.GetObserver<int, int, int, int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.GetObserver<int, int, int, int, int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.GetObserver<int, int, int, int, int, int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.GetObserver<int, int, int, int, int, int, int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.GetObserver<int, int, int, int, int, int, int, int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.GetObserver<int, int, int, int, int, int, int, int, int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.GetObserver<int, int, int, int, int, int, int, int, int, int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.GetObserver<int, int, int, int, int, int, int, int, int, int, int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.GetObserver<int, int, int, int, int, int, int, int, int, int, int, int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.GetObserver<int, int, int, int, int, int, int, int, int, int, int, int, int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.GetObserver<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.GetObserver<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.GetObserver<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(null));
                }
            );
        }

        [TestMethod]
        public void ReactiveClientContext_ReactiveClientProxy_ArgumentChecking_HigherArity_GetStreamFactory()
        {
            Apply(
                ctx =>
                {
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.GetStreamFactory<int, int, int, int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.GetStreamFactory<int, int, int, int, int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.GetStreamFactory<int, int, int, int, int, int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.GetStreamFactory<int, int, int, int, int, int, int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.GetStreamFactory<int, int, int, int, int, int, int, int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.GetStreamFactory<int, int, int, int, int, int, int, int, int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.GetStreamFactory<int, int, int, int, int, int, int, int, int, int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.GetStreamFactory<int, int, int, int, int, int, int, int, int, int, int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.GetStreamFactory<int, int, int, int, int, int, int, int, int, int, int, int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.GetStreamFactory<int, int, int, int, int, int, int, int, int, int, int, int, int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.GetStreamFactory<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.GetStreamFactory<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.GetStreamFactory<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.GetStreamFactory<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(null));
                }
            );
        }

        [TestMethod]
        public void ReactiveClientContext_ReactiveClientProxy_ArgumentChecking_HigherArity_GetSubscriptionFactory()
        {
            Apply(
                ctx =>
                {
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.GetSubscriptionFactory<int, int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.GetSubscriptionFactory<int, int, int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.GetSubscriptionFactory<int, int, int, int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.GetSubscriptionFactory<int, int, int, int, int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.GetSubscriptionFactory<int, int, int, int, int, int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.GetSubscriptionFactory<int, int, int, int, int, int, int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.GetSubscriptionFactory<int, int, int, int, int, int, int, int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.GetSubscriptionFactory<int, int, int, int, int, int, int, int, int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.GetSubscriptionFactory<int, int, int, int, int, int, int, int, int, int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.GetSubscriptionFactory<int, int, int, int, int, int, int, int, int, int, int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.GetSubscriptionFactory<int, int, int, int, int, int, int, int, int, int, int, int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.GetSubscriptionFactory<int, int, int, int, int, int, int, int, int, int, int, int, int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.GetSubscriptionFactory<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(null));
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.GetSubscriptionFactory<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(null));
                }
            );
        }

        #endregion

        #region "ReactiveClientContext_SubscribeAsync_Parameterized_Simple1"

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_Parameterized_Simple1_2P()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<string, string, int>(new Uri(Constants.Observable.XS));
                    var ob = ctx.GetObserver<string, string, int>(new Uri(Constants.Observer.OB));
                    xs("observable_parameter_1", "observable_parameter_2").SubscribeAsync(ob("observer_parameter_1", "observer_parameter_2"), new Uri(Constants.Subscription.SUB), state: null).Wait();
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, IAsyncReactiveQbserver<int>, Task<IAsyncReactiveQubscription>>), Constants.SubscribeUri),
                        Expression.Invoke(
                            Expression.Parameter(typeof(Func<string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS),
                            Expression.Constant("observable_parameter_1"),
                            Expression.Constant("observable_parameter_2")
                        ),
                        Expression.Invoke(
                            Expression.Parameter(typeof(Func<string, string, IAsyncReactiveQbserver<int>>), Constants.Observer.OB),
                            Expression.Constant("observer_parameter_1"),
                            Expression.Constant("observer_parameter_2")
                        )
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_Parameterized_Simple1_3P()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<string, string, string, int>(new Uri(Constants.Observable.XS));
                    var ob = ctx.GetObserver<string, string, string, int>(new Uri(Constants.Observer.OB));
                    xs("observable_parameter_1", "observable_parameter_2", "observable_parameter_3").SubscribeAsync(ob("observer_parameter_1", "observer_parameter_2", "observer_parameter_3"), new Uri(Constants.Subscription.SUB), state: null).Wait();
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, IAsyncReactiveQbserver<int>, Task<IAsyncReactiveQubscription>>), Constants.SubscribeUri),
                        Expression.Invoke(
                            Expression.Parameter(typeof(Func<string, string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS),
                            Expression.Constant("observable_parameter_1"),
                            Expression.Constant("observable_parameter_2"),
                            Expression.Constant("observable_parameter_3")
                        ),
                        Expression.Invoke(
                            Expression.Parameter(typeof(Func<string, string, string, IAsyncReactiveQbserver<int>>), Constants.Observer.OB),
                            Expression.Constant("observer_parameter_1"),
                            Expression.Constant("observer_parameter_2"),
                            Expression.Constant("observer_parameter_3")
                        )
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_Parameterized_Simple1_4P()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<string, string, string, string, int>(new Uri(Constants.Observable.XS));
                    var ob = ctx.GetObserver<string, string, string, string, int>(new Uri(Constants.Observer.OB));
                    xs("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4").SubscribeAsync(ob("observer_parameter_1", "observer_parameter_2", "observer_parameter_3", "observer_parameter_4"), new Uri(Constants.Subscription.SUB), state: null).Wait();
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, IAsyncReactiveQbserver<int>, Task<IAsyncReactiveQubscription>>), Constants.SubscribeUri),
                        Expression.Invoke(
                            Expression.Parameter(typeof(Func<string, string, string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS),
                            Expression.Constant("observable_parameter_1"),
                            Expression.Constant("observable_parameter_2"),
                            Expression.Constant("observable_parameter_3"),
                            Expression.Constant("observable_parameter_4")
                        ),
                        Expression.Invoke(
                            Expression.Parameter(typeof(Func<string, string, string, string, IAsyncReactiveQbserver<int>>), Constants.Observer.OB),
                            Expression.Constant("observer_parameter_1"),
                            Expression.Constant("observer_parameter_2"),
                            Expression.Constant("observer_parameter_3"),
                            Expression.Constant("observer_parameter_4")
                        )
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_Parameterized_Simple1_5P()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<string, string, string, string, string, int>(new Uri(Constants.Observable.XS));
                    var ob = ctx.GetObserver<string, string, string, string, string, int>(new Uri(Constants.Observer.OB));
                    xs("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5").SubscribeAsync(ob("observer_parameter_1", "observer_parameter_2", "observer_parameter_3", "observer_parameter_4", "observer_parameter_5"), new Uri(Constants.Subscription.SUB), state: null).Wait();
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, IAsyncReactiveQbserver<int>, Task<IAsyncReactiveQubscription>>), Constants.SubscribeUri),
                        Expression.Invoke(
                            Expression.Parameter(typeof(Func<string, string, string, string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS),
                            Expression.Constant("observable_parameter_1"),
                            Expression.Constant("observable_parameter_2"),
                            Expression.Constant("observable_parameter_3"),
                            Expression.Constant("observable_parameter_4"),
                            Expression.Constant("observable_parameter_5")
                        ),
                        Expression.Invoke(
                            Expression.Parameter(typeof(Func<string, string, string, string, string, IAsyncReactiveQbserver<int>>), Constants.Observer.OB),
                            Expression.Constant("observer_parameter_1"),
                            Expression.Constant("observer_parameter_2"),
                            Expression.Constant("observer_parameter_3"),
                            Expression.Constant("observer_parameter_4"),
                            Expression.Constant("observer_parameter_5")
                        )
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_Parameterized_Simple1_6P()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<string, string, string, string, string, string, int>(new Uri(Constants.Observable.XS));
                    var ob = ctx.GetObserver<string, string, string, string, string, string, int>(new Uri(Constants.Observer.OB));
                    xs("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6").SubscribeAsync(ob("observer_parameter_1", "observer_parameter_2", "observer_parameter_3", "observer_parameter_4", "observer_parameter_5", "observer_parameter_6"), new Uri(Constants.Subscription.SUB), state: null).Wait();
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, IAsyncReactiveQbserver<int>, Task<IAsyncReactiveQubscription>>), Constants.SubscribeUri),
                        Expression.Invoke(
                            Expression.Parameter(typeof(Func<string, string, string, string, string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS),
                            Expression.Constant("observable_parameter_1"),
                            Expression.Constant("observable_parameter_2"),
                            Expression.Constant("observable_parameter_3"),
                            Expression.Constant("observable_parameter_4"),
                            Expression.Constant("observable_parameter_5"),
                            Expression.Constant("observable_parameter_6")
                        ),
                        Expression.Invoke(
                            Expression.Parameter(typeof(Func<string, string, string, string, string, string, IAsyncReactiveQbserver<int>>), Constants.Observer.OB),
                            Expression.Constant("observer_parameter_1"),
                            Expression.Constant("observer_parameter_2"),
                            Expression.Constant("observer_parameter_3"),
                            Expression.Constant("observer_parameter_4"),
                            Expression.Constant("observer_parameter_5"),
                            Expression.Constant("observer_parameter_6")
                        )
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_Parameterized_Simple1_7P()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<string, string, string, string, string, string, string, int>(new Uri(Constants.Observable.XS));
                    var ob = ctx.GetObserver<string, string, string, string, string, string, string, int>(new Uri(Constants.Observer.OB));
                    xs("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7").SubscribeAsync(ob("observer_parameter_1", "observer_parameter_2", "observer_parameter_3", "observer_parameter_4", "observer_parameter_5", "observer_parameter_6", "observer_parameter_7"), new Uri(Constants.Subscription.SUB), state: null).Wait();
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, IAsyncReactiveQbserver<int>, Task<IAsyncReactiveQubscription>>), Constants.SubscribeUri),
                        Expression.Invoke(
                            Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS),
                            Expression.Constant("observable_parameter_1"),
                            Expression.Constant("observable_parameter_2"),
                            Expression.Constant("observable_parameter_3"),
                            Expression.Constant("observable_parameter_4"),
                            Expression.Constant("observable_parameter_5"),
                            Expression.Constant("observable_parameter_6"),
                            Expression.Constant("observable_parameter_7")
                        ),
                        Expression.Invoke(
                            Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, IAsyncReactiveQbserver<int>>), Constants.Observer.OB),
                            Expression.Constant("observer_parameter_1"),
                            Expression.Constant("observer_parameter_2"),
                            Expression.Constant("observer_parameter_3"),
                            Expression.Constant("observer_parameter_4"),
                            Expression.Constant("observer_parameter_5"),
                            Expression.Constant("observer_parameter_6"),
                            Expression.Constant("observer_parameter_7")
                        )
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_Parameterized_Simple1_8P()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observable.XS));
                    var ob = ctx.GetObserver<string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observer.OB));
                    xs("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7", "observable_parameter_8").SubscribeAsync(ob("observer_parameter_1", "observer_parameter_2", "observer_parameter_3", "observer_parameter_4", "observer_parameter_5", "observer_parameter_6", "observer_parameter_7", "observer_parameter_8"), new Uri(Constants.Subscription.SUB), state: null).Wait();
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, IAsyncReactiveQbserver<int>, Task<IAsyncReactiveQubscription>>), Constants.SubscribeUri),
                        Expression.Invoke(
                            Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS),
                            Expression.Constant("observable_parameter_1"),
                            Expression.Constant("observable_parameter_2"),
                            Expression.Constant("observable_parameter_3"),
                            Expression.Constant("observable_parameter_4"),
                            Expression.Constant("observable_parameter_5"),
                            Expression.Constant("observable_parameter_6"),
                            Expression.Constant("observable_parameter_7"),
                            Expression.Constant("observable_parameter_8")
                        ),
                        Expression.Invoke(
                            Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, IAsyncReactiveQbserver<int>>), Constants.Observer.OB),
                            Expression.Constant("observer_parameter_1"),
                            Expression.Constant("observer_parameter_2"),
                            Expression.Constant("observer_parameter_3"),
                            Expression.Constant("observer_parameter_4"),
                            Expression.Constant("observer_parameter_5"),
                            Expression.Constant("observer_parameter_6"),
                            Expression.Constant("observer_parameter_7"),
                            Expression.Constant("observer_parameter_8")
                        )
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_Parameterized_Simple1_9P()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observable.XS));
                    var ob = ctx.GetObserver<string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observer.OB));
                    xs("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7", "observable_parameter_8", "observable_parameter_9").SubscribeAsync(ob("observer_parameter_1", "observer_parameter_2", "observer_parameter_3", "observer_parameter_4", "observer_parameter_5", "observer_parameter_6", "observer_parameter_7", "observer_parameter_8", "observer_parameter_9"), new Uri(Constants.Subscription.SUB), state: null).Wait();
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, IAsyncReactiveQbserver<int>, Task<IAsyncReactiveQubscription>>), Constants.SubscribeUri),
                        Expression.Invoke(
                            Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS),
                            Expression.Constant("observable_parameter_1"),
                            Expression.Constant("observable_parameter_2"),
                            Expression.Constant("observable_parameter_3"),
                            Expression.Constant("observable_parameter_4"),
                            Expression.Constant("observable_parameter_5"),
                            Expression.Constant("observable_parameter_6"),
                            Expression.Constant("observable_parameter_7"),
                            Expression.Constant("observable_parameter_8"),
                            Expression.Constant("observable_parameter_9")
                        ),
                        Expression.Invoke(
                            Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, IAsyncReactiveQbserver<int>>), Constants.Observer.OB),
                            Expression.Constant("observer_parameter_1"),
                            Expression.Constant("observer_parameter_2"),
                            Expression.Constant("observer_parameter_3"),
                            Expression.Constant("observer_parameter_4"),
                            Expression.Constant("observer_parameter_5"),
                            Expression.Constant("observer_parameter_6"),
                            Expression.Constant("observer_parameter_7"),
                            Expression.Constant("observer_parameter_8"),
                            Expression.Constant("observer_parameter_9")
                        )
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_Parameterized_Simple1_10P()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observable.XS));
                    var ob = ctx.GetObserver<string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observer.OB));
                    xs("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7", "observable_parameter_8", "observable_parameter_9", "observable_parameter_10").SubscribeAsync(ob("observer_parameter_1", "observer_parameter_2", "observer_parameter_3", "observer_parameter_4", "observer_parameter_5", "observer_parameter_6", "observer_parameter_7", "observer_parameter_8", "observer_parameter_9", "observer_parameter_10"), new Uri(Constants.Subscription.SUB), state: null).Wait();
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, IAsyncReactiveQbserver<int>, Task<IAsyncReactiveQubscription>>), Constants.SubscribeUri),
                        Expression.Invoke(
                            Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS),
                            Expression.Constant("observable_parameter_1"),
                            Expression.Constant("observable_parameter_2"),
                            Expression.Constant("observable_parameter_3"),
                            Expression.Constant("observable_parameter_4"),
                            Expression.Constant("observable_parameter_5"),
                            Expression.Constant("observable_parameter_6"),
                            Expression.Constant("observable_parameter_7"),
                            Expression.Constant("observable_parameter_8"),
                            Expression.Constant("observable_parameter_9"),
                            Expression.Constant("observable_parameter_10")
                        ),
                        Expression.Invoke(
                            Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQbserver<int>>), Constants.Observer.OB),
                            Expression.Constant("observer_parameter_1"),
                            Expression.Constant("observer_parameter_2"),
                            Expression.Constant("observer_parameter_3"),
                            Expression.Constant("observer_parameter_4"),
                            Expression.Constant("observer_parameter_5"),
                            Expression.Constant("observer_parameter_6"),
                            Expression.Constant("observer_parameter_7"),
                            Expression.Constant("observer_parameter_8"),
                            Expression.Constant("observer_parameter_9"),
                            Expression.Constant("observer_parameter_10")
                        )
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_Parameterized_Simple1_11P()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<string, string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observable.XS));
                    var ob = ctx.GetObserver<string, string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observer.OB));
                    xs("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7", "observable_parameter_8", "observable_parameter_9", "observable_parameter_10", "observable_parameter_11").SubscribeAsync(ob("observer_parameter_1", "observer_parameter_2", "observer_parameter_3", "observer_parameter_4", "observer_parameter_5", "observer_parameter_6", "observer_parameter_7", "observer_parameter_8", "observer_parameter_9", "observer_parameter_10", "observer_parameter_11"), new Uri(Constants.Subscription.SUB), state: null).Wait();
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, IAsyncReactiveQbserver<int>, Task<IAsyncReactiveQubscription>>), Constants.SubscribeUri),
                        Expression.Invoke(
                            Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS),
                            Expression.Constant("observable_parameter_1"),
                            Expression.Constant("observable_parameter_2"),
                            Expression.Constant("observable_parameter_3"),
                            Expression.Constant("observable_parameter_4"),
                            Expression.Constant("observable_parameter_5"),
                            Expression.Constant("observable_parameter_6"),
                            Expression.Constant("observable_parameter_7"),
                            Expression.Constant("observable_parameter_8"),
                            Expression.Constant("observable_parameter_9"),
                            Expression.Constant("observable_parameter_10"),
                            Expression.Constant("observable_parameter_11")
                        ),
                        Expression.Invoke(
                            Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQbserver<int>>), Constants.Observer.OB),
                            Expression.Constant("observer_parameter_1"),
                            Expression.Constant("observer_parameter_2"),
                            Expression.Constant("observer_parameter_3"),
                            Expression.Constant("observer_parameter_4"),
                            Expression.Constant("observer_parameter_5"),
                            Expression.Constant("observer_parameter_6"),
                            Expression.Constant("observer_parameter_7"),
                            Expression.Constant("observer_parameter_8"),
                            Expression.Constant("observer_parameter_9"),
                            Expression.Constant("observer_parameter_10"),
                            Expression.Constant("observer_parameter_11")
                        )
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_Parameterized_Simple1_12P()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<string, string, string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observable.XS));
                    var ob = ctx.GetObserver<string, string, string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observer.OB));
                    xs("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7", "observable_parameter_8", "observable_parameter_9", "observable_parameter_10", "observable_parameter_11", "observable_parameter_12").SubscribeAsync(ob("observer_parameter_1", "observer_parameter_2", "observer_parameter_3", "observer_parameter_4", "observer_parameter_5", "observer_parameter_6", "observer_parameter_7", "observer_parameter_8", "observer_parameter_9", "observer_parameter_10", "observer_parameter_11", "observer_parameter_12"), new Uri(Constants.Subscription.SUB), state: null).Wait();
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, IAsyncReactiveQbserver<int>, Task<IAsyncReactiveQubscription>>), Constants.SubscribeUri),
                        Expression.Invoke(
                            Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS),
                            Expression.Constant("observable_parameter_1"),
                            Expression.Constant("observable_parameter_2"),
                            Expression.Constant("observable_parameter_3"),
                            Expression.Constant("observable_parameter_4"),
                            Expression.Constant("observable_parameter_5"),
                            Expression.Constant("observable_parameter_6"),
                            Expression.Constant("observable_parameter_7"),
                            Expression.Constant("observable_parameter_8"),
                            Expression.Constant("observable_parameter_9"),
                            Expression.Constant("observable_parameter_10"),
                            Expression.Constant("observable_parameter_11"),
                            Expression.Constant("observable_parameter_12")
                        ),
                        Expression.Invoke(
                            Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQbserver<int>>), Constants.Observer.OB),
                            Expression.Constant("observer_parameter_1"),
                            Expression.Constant("observer_parameter_2"),
                            Expression.Constant("observer_parameter_3"),
                            Expression.Constant("observer_parameter_4"),
                            Expression.Constant("observer_parameter_5"),
                            Expression.Constant("observer_parameter_6"),
                            Expression.Constant("observer_parameter_7"),
                            Expression.Constant("observer_parameter_8"),
                            Expression.Constant("observer_parameter_9"),
                            Expression.Constant("observer_parameter_10"),
                            Expression.Constant("observer_parameter_11"),
                            Expression.Constant("observer_parameter_12")
                        )
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_Parameterized_Simple1_13P()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<string, string, string, string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observable.XS));
                    var ob = ctx.GetObserver<string, string, string, string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observer.OB));
                    xs("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7", "observable_parameter_8", "observable_parameter_9", "observable_parameter_10", "observable_parameter_11", "observable_parameter_12", "observable_parameter_13").SubscribeAsync(ob("observer_parameter_1", "observer_parameter_2", "observer_parameter_3", "observer_parameter_4", "observer_parameter_5", "observer_parameter_6", "observer_parameter_7", "observer_parameter_8", "observer_parameter_9", "observer_parameter_10", "observer_parameter_11", "observer_parameter_12", "observer_parameter_13"), new Uri(Constants.Subscription.SUB), state: null).Wait();
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, IAsyncReactiveQbserver<int>, Task<IAsyncReactiveQubscription>>), Constants.SubscribeUri),
                        Expression.Invoke(
                            Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS),
                            Expression.Constant("observable_parameter_1"),
                            Expression.Constant("observable_parameter_2"),
                            Expression.Constant("observable_parameter_3"),
                            Expression.Constant("observable_parameter_4"),
                            Expression.Constant("observable_parameter_5"),
                            Expression.Constant("observable_parameter_6"),
                            Expression.Constant("observable_parameter_7"),
                            Expression.Constant("observable_parameter_8"),
                            Expression.Constant("observable_parameter_9"),
                            Expression.Constant("observable_parameter_10"),
                            Expression.Constant("observable_parameter_11"),
                            Expression.Constant("observable_parameter_12"),
                            Expression.Constant("observable_parameter_13")
                        ),
                        Expression.Invoke(
                            Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQbserver<int>>), Constants.Observer.OB),
                            Expression.Constant("observer_parameter_1"),
                            Expression.Constant("observer_parameter_2"),
                            Expression.Constant("observer_parameter_3"),
                            Expression.Constant("observer_parameter_4"),
                            Expression.Constant("observer_parameter_5"),
                            Expression.Constant("observer_parameter_6"),
                            Expression.Constant("observer_parameter_7"),
                            Expression.Constant("observer_parameter_8"),
                            Expression.Constant("observer_parameter_9"),
                            Expression.Constant("observer_parameter_10"),
                            Expression.Constant("observer_parameter_11"),
                            Expression.Constant("observer_parameter_12"),
                            Expression.Constant("observer_parameter_13")
                        )
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_Parameterized_Simple1_14P()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<string, string, string, string, string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observable.XS));
                    var ob = ctx.GetObserver<string, string, string, string, string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observer.OB));
                    xs("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7", "observable_parameter_8", "observable_parameter_9", "observable_parameter_10", "observable_parameter_11", "observable_parameter_12", "observable_parameter_13", "observable_parameter_14").SubscribeAsync(ob("observer_parameter_1", "observer_parameter_2", "observer_parameter_3", "observer_parameter_4", "observer_parameter_5", "observer_parameter_6", "observer_parameter_7", "observer_parameter_8", "observer_parameter_9", "observer_parameter_10", "observer_parameter_11", "observer_parameter_12", "observer_parameter_13", "observer_parameter_14"), new Uri(Constants.Subscription.SUB), state: null).Wait();
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, IAsyncReactiveQbserver<int>, Task<IAsyncReactiveQubscription>>), Constants.SubscribeUri),
                        Expression.Invoke(
                            Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS),
                            Expression.Constant("observable_parameter_1"),
                            Expression.Constant("observable_parameter_2"),
                            Expression.Constant("observable_parameter_3"),
                            Expression.Constant("observable_parameter_4"),
                            Expression.Constant("observable_parameter_5"),
                            Expression.Constant("observable_parameter_6"),
                            Expression.Constant("observable_parameter_7"),
                            Expression.Constant("observable_parameter_8"),
                            Expression.Constant("observable_parameter_9"),
                            Expression.Constant("observable_parameter_10"),
                            Expression.Constant("observable_parameter_11"),
                            Expression.Constant("observable_parameter_12"),
                            Expression.Constant("observable_parameter_13"),
                            Expression.Constant("observable_parameter_14")
                        ),
                        Expression.Invoke(
                            Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQbserver<int>>), Constants.Observer.OB),
                            Expression.Constant("observer_parameter_1"),
                            Expression.Constant("observer_parameter_2"),
                            Expression.Constant("observer_parameter_3"),
                            Expression.Constant("observer_parameter_4"),
                            Expression.Constant("observer_parameter_5"),
                            Expression.Constant("observer_parameter_6"),
                            Expression.Constant("observer_parameter_7"),
                            Expression.Constant("observer_parameter_8"),
                            Expression.Constant("observer_parameter_9"),
                            Expression.Constant("observer_parameter_10"),
                            Expression.Constant("observer_parameter_11"),
                            Expression.Constant("observer_parameter_12"),
                            Expression.Constant("observer_parameter_13"),
                            Expression.Constant("observer_parameter_14")
                        )
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_Parameterized_Simple1_15P()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observable.XS));
                    var ob = ctx.GetObserver<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observer.OB));
                    xs("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7", "observable_parameter_8", "observable_parameter_9", "observable_parameter_10", "observable_parameter_11", "observable_parameter_12", "observable_parameter_13", "observable_parameter_14", "observable_parameter_15").SubscribeAsync(ob("observer_parameter_1", "observer_parameter_2", "observer_parameter_3", "observer_parameter_4", "observer_parameter_5", "observer_parameter_6", "observer_parameter_7", "observer_parameter_8", "observer_parameter_9", "observer_parameter_10", "observer_parameter_11", "observer_parameter_12", "observer_parameter_13", "observer_parameter_14", "observer_parameter_15"), new Uri(Constants.Subscription.SUB), state: null).Wait();
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, IAsyncReactiveQbserver<int>, Task<IAsyncReactiveQubscription>>), Constants.SubscribeUri),
                        Expression.Invoke(
                            Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS),
                            Expression.Constant("observable_parameter_1"),
                            Expression.Constant("observable_parameter_2"),
                            Expression.Constant("observable_parameter_3"),
                            Expression.Constant("observable_parameter_4"),
                            Expression.Constant("observable_parameter_5"),
                            Expression.Constant("observable_parameter_6"),
                            Expression.Constant("observable_parameter_7"),
                            Expression.Constant("observable_parameter_8"),
                            Expression.Constant("observable_parameter_9"),
                            Expression.Constant("observable_parameter_10"),
                            Expression.Constant("observable_parameter_11"),
                            Expression.Constant("observable_parameter_12"),
                            Expression.Constant("observable_parameter_13"),
                            Expression.Constant("observable_parameter_14"),
                            Expression.Constant("observable_parameter_15")
                        ),
                        Expression.Invoke(
                            Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQbserver<int>>), Constants.Observer.OB),
                            Expression.Constant("observer_parameter_1"),
                            Expression.Constant("observer_parameter_2"),
                            Expression.Constant("observer_parameter_3"),
                            Expression.Constant("observer_parameter_4"),
                            Expression.Constant("observer_parameter_5"),
                            Expression.Constant("observer_parameter_6"),
                            Expression.Constant("observer_parameter_7"),
                            Expression.Constant("observer_parameter_8"),
                            Expression.Constant("observer_parameter_9"),
                            Expression.Constant("observer_parameter_10"),
                            Expression.Constant("observer_parameter_11"),
                            Expression.Constant("observer_parameter_12"),
                            Expression.Constant("observer_parameter_13"),
                            Expression.Constant("observer_parameter_14"),
                            Expression.Constant("observer_parameter_15")
                        )
                    ),
                    state: null
                )
            );
        }


        #endregion

        #region "ReactiveClientContext_SubscribeAsync_Parameterized_Simple2"

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_Parameterized_Simple2_2P()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<string, string, int>(new Uri(Constants.Observable.XS));

                    var ys = from x in xs("observable_parameter_1", "observable_parameter_2")
                             where x > 0
                             select x * x;

                    var ob = ctx.GetObserver<string, string, int>(new Uri(Constants.Observer.OB));
                    ys.SubscribeAsync(ob("observer_parameter_1", "observer_parameter_2"), new Uri(Constants.Subscription.SUB), state: null).Wait();
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
                                        Expression.Parameter(typeof(Func<string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS),
                                        Expression.Constant("observable_parameter_1"),
                                        Expression.Constant("observable_parameter_2")
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
                            Expression.Invoke(
                                Expression.Parameter(typeof(Func<string, string, IAsyncReactiveQbserver<int>>), Constants.Observer.OB),
                                Expression.Constant("observer_parameter_1"),
                                Expression.Constant("observer_parameter_2")
                            )
                        )
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_Parameterized_Simple2_3P()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<string, string, string, int>(new Uri(Constants.Observable.XS));

                    var ys = from x in xs("observable_parameter_1", "observable_parameter_2", "observable_parameter_3")
                             where x > 0
                             select x * x;

                    var ob = ctx.GetObserver<string, string, string, int>(new Uri(Constants.Observer.OB));
                    ys.SubscribeAsync(ob("observer_parameter_1", "observer_parameter_2", "observer_parameter_3"), new Uri(Constants.Subscription.SUB), state: null).Wait();
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
                                        Expression.Parameter(typeof(Func<string, string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS),
                                        Expression.Constant("observable_parameter_1"),
                                        Expression.Constant("observable_parameter_2"),
                                        Expression.Constant("observable_parameter_3")
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
                            Expression.Invoke(
                                Expression.Parameter(typeof(Func<string, string, string, IAsyncReactiveQbserver<int>>), Constants.Observer.OB),
                                Expression.Constant("observer_parameter_1"),
                                Expression.Constant("observer_parameter_2"),
                                Expression.Constant("observer_parameter_3")
                            )
                        )
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_Parameterized_Simple2_4P()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<string, string, string, string, int>(new Uri(Constants.Observable.XS));

                    var ys = from x in xs("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4")
                             where x > 0
                             select x * x;

                    var ob = ctx.GetObserver<string, string, string, string, int>(new Uri(Constants.Observer.OB));
                    ys.SubscribeAsync(ob("observer_parameter_1", "observer_parameter_2", "observer_parameter_3", "observer_parameter_4"), new Uri(Constants.Subscription.SUB), state: null).Wait();
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
                                        Expression.Parameter(typeof(Func<string, string, string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS),
                                        Expression.Constant("observable_parameter_1"),
                                        Expression.Constant("observable_parameter_2"),
                                        Expression.Constant("observable_parameter_3"),
                                        Expression.Constant("observable_parameter_4")
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
                            Expression.Invoke(
                                Expression.Parameter(typeof(Func<string, string, string, string, IAsyncReactiveQbserver<int>>), Constants.Observer.OB),
                                Expression.Constant("observer_parameter_1"),
                                Expression.Constant("observer_parameter_2"),
                                Expression.Constant("observer_parameter_3"),
                                Expression.Constant("observer_parameter_4")
                            )
                        )
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_Parameterized_Simple2_5P()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<string, string, string, string, string, int>(new Uri(Constants.Observable.XS));

                    var ys = from x in xs("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5")
                             where x > 0
                             select x * x;

                    var ob = ctx.GetObserver<string, string, string, string, string, int>(new Uri(Constants.Observer.OB));
                    ys.SubscribeAsync(ob("observer_parameter_1", "observer_parameter_2", "observer_parameter_3", "observer_parameter_4", "observer_parameter_5"), new Uri(Constants.Subscription.SUB), state: null).Wait();
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
                                        Expression.Parameter(typeof(Func<string, string, string, string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS),
                                        Expression.Constant("observable_parameter_1"),
                                        Expression.Constant("observable_parameter_2"),
                                        Expression.Constant("observable_parameter_3"),
                                        Expression.Constant("observable_parameter_4"),
                                        Expression.Constant("observable_parameter_5")
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
                            Expression.Invoke(
                                Expression.Parameter(typeof(Func<string, string, string, string, string, IAsyncReactiveQbserver<int>>), Constants.Observer.OB),
                                Expression.Constant("observer_parameter_1"),
                                Expression.Constant("observer_parameter_2"),
                                Expression.Constant("observer_parameter_3"),
                                Expression.Constant("observer_parameter_4"),
                                Expression.Constant("observer_parameter_5")
                            )
                        )
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_Parameterized_Simple2_6P()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<string, string, string, string, string, string, int>(new Uri(Constants.Observable.XS));

                    var ys = from x in xs("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6")
                             where x > 0
                             select x * x;

                    var ob = ctx.GetObserver<string, string, string, string, string, string, int>(new Uri(Constants.Observer.OB));
                    ys.SubscribeAsync(ob("observer_parameter_1", "observer_parameter_2", "observer_parameter_3", "observer_parameter_4", "observer_parameter_5", "observer_parameter_6"), new Uri(Constants.Subscription.SUB), state: null).Wait();
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
                                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS),
                                        Expression.Constant("observable_parameter_1"),
                                        Expression.Constant("observable_parameter_2"),
                                        Expression.Constant("observable_parameter_3"),
                                        Expression.Constant("observable_parameter_4"),
                                        Expression.Constant("observable_parameter_5"),
                                        Expression.Constant("observable_parameter_6")
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
                            Expression.Invoke(
                                Expression.Parameter(typeof(Func<string, string, string, string, string, string, IAsyncReactiveQbserver<int>>), Constants.Observer.OB),
                                Expression.Constant("observer_parameter_1"),
                                Expression.Constant("observer_parameter_2"),
                                Expression.Constant("observer_parameter_3"),
                                Expression.Constant("observer_parameter_4"),
                                Expression.Constant("observer_parameter_5"),
                                Expression.Constant("observer_parameter_6")
                            )
                        )
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_Parameterized_Simple2_7P()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<string, string, string, string, string, string, string, int>(new Uri(Constants.Observable.XS));

                    var ys = from x in xs("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7")
                             where x > 0
                             select x * x;

                    var ob = ctx.GetObserver<string, string, string, string, string, string, string, int>(new Uri(Constants.Observer.OB));
                    ys.SubscribeAsync(ob("observer_parameter_1", "observer_parameter_2", "observer_parameter_3", "observer_parameter_4", "observer_parameter_5", "observer_parameter_6", "observer_parameter_7"), new Uri(Constants.Subscription.SUB), state: null).Wait();
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
                                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS),
                                        Expression.Constant("observable_parameter_1"),
                                        Expression.Constant("observable_parameter_2"),
                                        Expression.Constant("observable_parameter_3"),
                                        Expression.Constant("observable_parameter_4"),
                                        Expression.Constant("observable_parameter_5"),
                                        Expression.Constant("observable_parameter_6"),
                                        Expression.Constant("observable_parameter_7")
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
                            Expression.Invoke(
                                Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, IAsyncReactiveQbserver<int>>), Constants.Observer.OB),
                                Expression.Constant("observer_parameter_1"),
                                Expression.Constant("observer_parameter_2"),
                                Expression.Constant("observer_parameter_3"),
                                Expression.Constant("observer_parameter_4"),
                                Expression.Constant("observer_parameter_5"),
                                Expression.Constant("observer_parameter_6"),
                                Expression.Constant("observer_parameter_7")
                            )
                        )
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_Parameterized_Simple2_8P()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observable.XS));

                    var ys = from x in xs("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7", "observable_parameter_8")
                             where x > 0
                             select x * x;

                    var ob = ctx.GetObserver<string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observer.OB));
                    ys.SubscribeAsync(ob("observer_parameter_1", "observer_parameter_2", "observer_parameter_3", "observer_parameter_4", "observer_parameter_5", "observer_parameter_6", "observer_parameter_7", "observer_parameter_8"), new Uri(Constants.Subscription.SUB), state: null).Wait();
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
                                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS),
                                        Expression.Constant("observable_parameter_1"),
                                        Expression.Constant("observable_parameter_2"),
                                        Expression.Constant("observable_parameter_3"),
                                        Expression.Constant("observable_parameter_4"),
                                        Expression.Constant("observable_parameter_5"),
                                        Expression.Constant("observable_parameter_6"),
                                        Expression.Constant("observable_parameter_7"),
                                        Expression.Constant("observable_parameter_8")
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
                            Expression.Invoke(
                                Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, IAsyncReactiveQbserver<int>>), Constants.Observer.OB),
                                Expression.Constant("observer_parameter_1"),
                                Expression.Constant("observer_parameter_2"),
                                Expression.Constant("observer_parameter_3"),
                                Expression.Constant("observer_parameter_4"),
                                Expression.Constant("observer_parameter_5"),
                                Expression.Constant("observer_parameter_6"),
                                Expression.Constant("observer_parameter_7"),
                                Expression.Constant("observer_parameter_8")
                            )
                        )
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_Parameterized_Simple2_9P()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observable.XS));

                    var ys = from x in xs("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7", "observable_parameter_8", "observable_parameter_9")
                             where x > 0
                             select x * x;

                    var ob = ctx.GetObserver<string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observer.OB));
                    ys.SubscribeAsync(ob("observer_parameter_1", "observer_parameter_2", "observer_parameter_3", "observer_parameter_4", "observer_parameter_5", "observer_parameter_6", "observer_parameter_7", "observer_parameter_8", "observer_parameter_9"), new Uri(Constants.Subscription.SUB), state: null).Wait();
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
                                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS),
                                        Expression.Constant("observable_parameter_1"),
                                        Expression.Constant("observable_parameter_2"),
                                        Expression.Constant("observable_parameter_3"),
                                        Expression.Constant("observable_parameter_4"),
                                        Expression.Constant("observable_parameter_5"),
                                        Expression.Constant("observable_parameter_6"),
                                        Expression.Constant("observable_parameter_7"),
                                        Expression.Constant("observable_parameter_8"),
                                        Expression.Constant("observable_parameter_9")
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
                            Expression.Invoke(
                                Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, IAsyncReactiveQbserver<int>>), Constants.Observer.OB),
                                Expression.Constant("observer_parameter_1"),
                                Expression.Constant("observer_parameter_2"),
                                Expression.Constant("observer_parameter_3"),
                                Expression.Constant("observer_parameter_4"),
                                Expression.Constant("observer_parameter_5"),
                                Expression.Constant("observer_parameter_6"),
                                Expression.Constant("observer_parameter_7"),
                                Expression.Constant("observer_parameter_8"),
                                Expression.Constant("observer_parameter_9")
                            )
                        )
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_Parameterized_Simple2_10P()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observable.XS));

                    var ys = from x in xs("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7", "observable_parameter_8", "observable_parameter_9", "observable_parameter_10")
                             where x > 0
                             select x * x;

                    var ob = ctx.GetObserver<string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observer.OB));
                    ys.SubscribeAsync(ob("observer_parameter_1", "observer_parameter_2", "observer_parameter_3", "observer_parameter_4", "observer_parameter_5", "observer_parameter_6", "observer_parameter_7", "observer_parameter_8", "observer_parameter_9", "observer_parameter_10"), new Uri(Constants.Subscription.SUB), state: null).Wait();
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
                                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS),
                                        Expression.Constant("observable_parameter_1"),
                                        Expression.Constant("observable_parameter_2"),
                                        Expression.Constant("observable_parameter_3"),
                                        Expression.Constant("observable_parameter_4"),
                                        Expression.Constant("observable_parameter_5"),
                                        Expression.Constant("observable_parameter_6"),
                                        Expression.Constant("observable_parameter_7"),
                                        Expression.Constant("observable_parameter_8"),
                                        Expression.Constant("observable_parameter_9"),
                                        Expression.Constant("observable_parameter_10")
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
                            Expression.Invoke(
                                Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQbserver<int>>), Constants.Observer.OB),
                                Expression.Constant("observer_parameter_1"),
                                Expression.Constant("observer_parameter_2"),
                                Expression.Constant("observer_parameter_3"),
                                Expression.Constant("observer_parameter_4"),
                                Expression.Constant("observer_parameter_5"),
                                Expression.Constant("observer_parameter_6"),
                                Expression.Constant("observer_parameter_7"),
                                Expression.Constant("observer_parameter_8"),
                                Expression.Constant("observer_parameter_9"),
                                Expression.Constant("observer_parameter_10")
                            )
                        )
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_Parameterized_Simple2_11P()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<string, string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observable.XS));

                    var ys = from x in xs("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7", "observable_parameter_8", "observable_parameter_9", "observable_parameter_10", "observable_parameter_11")
                             where x > 0
                             select x * x;

                    var ob = ctx.GetObserver<string, string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observer.OB));
                    ys.SubscribeAsync(ob("observer_parameter_1", "observer_parameter_2", "observer_parameter_3", "observer_parameter_4", "observer_parameter_5", "observer_parameter_6", "observer_parameter_7", "observer_parameter_8", "observer_parameter_9", "observer_parameter_10", "observer_parameter_11"), new Uri(Constants.Subscription.SUB), state: null).Wait();
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
                                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS),
                                        Expression.Constant("observable_parameter_1"),
                                        Expression.Constant("observable_parameter_2"),
                                        Expression.Constant("observable_parameter_3"),
                                        Expression.Constant("observable_parameter_4"),
                                        Expression.Constant("observable_parameter_5"),
                                        Expression.Constant("observable_parameter_6"),
                                        Expression.Constant("observable_parameter_7"),
                                        Expression.Constant("observable_parameter_8"),
                                        Expression.Constant("observable_parameter_9"),
                                        Expression.Constant("observable_parameter_10"),
                                        Expression.Constant("observable_parameter_11")
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
                            Expression.Invoke(
                                Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQbserver<int>>), Constants.Observer.OB),
                                Expression.Constant("observer_parameter_1"),
                                Expression.Constant("observer_parameter_2"),
                                Expression.Constant("observer_parameter_3"),
                                Expression.Constant("observer_parameter_4"),
                                Expression.Constant("observer_parameter_5"),
                                Expression.Constant("observer_parameter_6"),
                                Expression.Constant("observer_parameter_7"),
                                Expression.Constant("observer_parameter_8"),
                                Expression.Constant("observer_parameter_9"),
                                Expression.Constant("observer_parameter_10"),
                                Expression.Constant("observer_parameter_11")
                            )
                        )
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_Parameterized_Simple2_12P()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<string, string, string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observable.XS));

                    var ys = from x in xs("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7", "observable_parameter_8", "observable_parameter_9", "observable_parameter_10", "observable_parameter_11", "observable_parameter_12")
                             where x > 0
                             select x * x;

                    var ob = ctx.GetObserver<string, string, string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observer.OB));
                    ys.SubscribeAsync(ob("observer_parameter_1", "observer_parameter_2", "observer_parameter_3", "observer_parameter_4", "observer_parameter_5", "observer_parameter_6", "observer_parameter_7", "observer_parameter_8", "observer_parameter_9", "observer_parameter_10", "observer_parameter_11", "observer_parameter_12"), new Uri(Constants.Subscription.SUB), state: null).Wait();
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
                                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS),
                                        Expression.Constant("observable_parameter_1"),
                                        Expression.Constant("observable_parameter_2"),
                                        Expression.Constant("observable_parameter_3"),
                                        Expression.Constant("observable_parameter_4"),
                                        Expression.Constant("observable_parameter_5"),
                                        Expression.Constant("observable_parameter_6"),
                                        Expression.Constant("observable_parameter_7"),
                                        Expression.Constant("observable_parameter_8"),
                                        Expression.Constant("observable_parameter_9"),
                                        Expression.Constant("observable_parameter_10"),
                                        Expression.Constant("observable_parameter_11"),
                                        Expression.Constant("observable_parameter_12")
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
                            Expression.Invoke(
                                Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQbserver<int>>), Constants.Observer.OB),
                                Expression.Constant("observer_parameter_1"),
                                Expression.Constant("observer_parameter_2"),
                                Expression.Constant("observer_parameter_3"),
                                Expression.Constant("observer_parameter_4"),
                                Expression.Constant("observer_parameter_5"),
                                Expression.Constant("observer_parameter_6"),
                                Expression.Constant("observer_parameter_7"),
                                Expression.Constant("observer_parameter_8"),
                                Expression.Constant("observer_parameter_9"),
                                Expression.Constant("observer_parameter_10"),
                                Expression.Constant("observer_parameter_11"),
                                Expression.Constant("observer_parameter_12")
                            )
                        )
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_Parameterized_Simple2_13P()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<string, string, string, string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observable.XS));

                    var ys = from x in xs("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7", "observable_parameter_8", "observable_parameter_9", "observable_parameter_10", "observable_parameter_11", "observable_parameter_12", "observable_parameter_13")
                             where x > 0
                             select x * x;

                    var ob = ctx.GetObserver<string, string, string, string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observer.OB));
                    ys.SubscribeAsync(ob("observer_parameter_1", "observer_parameter_2", "observer_parameter_3", "observer_parameter_4", "observer_parameter_5", "observer_parameter_6", "observer_parameter_7", "observer_parameter_8", "observer_parameter_9", "observer_parameter_10", "observer_parameter_11", "observer_parameter_12", "observer_parameter_13"), new Uri(Constants.Subscription.SUB), state: null).Wait();
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
                                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS),
                                        Expression.Constant("observable_parameter_1"),
                                        Expression.Constant("observable_parameter_2"),
                                        Expression.Constant("observable_parameter_3"),
                                        Expression.Constant("observable_parameter_4"),
                                        Expression.Constant("observable_parameter_5"),
                                        Expression.Constant("observable_parameter_6"),
                                        Expression.Constant("observable_parameter_7"),
                                        Expression.Constant("observable_parameter_8"),
                                        Expression.Constant("observable_parameter_9"),
                                        Expression.Constant("observable_parameter_10"),
                                        Expression.Constant("observable_parameter_11"),
                                        Expression.Constant("observable_parameter_12"),
                                        Expression.Constant("observable_parameter_13")
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
                            Expression.Invoke(
                                Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQbserver<int>>), Constants.Observer.OB),
                                Expression.Constant("observer_parameter_1"),
                                Expression.Constant("observer_parameter_2"),
                                Expression.Constant("observer_parameter_3"),
                                Expression.Constant("observer_parameter_4"),
                                Expression.Constant("observer_parameter_5"),
                                Expression.Constant("observer_parameter_6"),
                                Expression.Constant("observer_parameter_7"),
                                Expression.Constant("observer_parameter_8"),
                                Expression.Constant("observer_parameter_9"),
                                Expression.Constant("observer_parameter_10"),
                                Expression.Constant("observer_parameter_11"),
                                Expression.Constant("observer_parameter_12"),
                                Expression.Constant("observer_parameter_13")
                            )
                        )
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_Parameterized_Simple2_14P()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<string, string, string, string, string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observable.XS));

                    var ys = from x in xs("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7", "observable_parameter_8", "observable_parameter_9", "observable_parameter_10", "observable_parameter_11", "observable_parameter_12", "observable_parameter_13", "observable_parameter_14")
                             where x > 0
                             select x * x;

                    var ob = ctx.GetObserver<string, string, string, string, string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observer.OB));
                    ys.SubscribeAsync(ob("observer_parameter_1", "observer_parameter_2", "observer_parameter_3", "observer_parameter_4", "observer_parameter_5", "observer_parameter_6", "observer_parameter_7", "observer_parameter_8", "observer_parameter_9", "observer_parameter_10", "observer_parameter_11", "observer_parameter_12", "observer_parameter_13", "observer_parameter_14"), new Uri(Constants.Subscription.SUB), state: null).Wait();
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
                                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS),
                                        Expression.Constant("observable_parameter_1"),
                                        Expression.Constant("observable_parameter_2"),
                                        Expression.Constant("observable_parameter_3"),
                                        Expression.Constant("observable_parameter_4"),
                                        Expression.Constant("observable_parameter_5"),
                                        Expression.Constant("observable_parameter_6"),
                                        Expression.Constant("observable_parameter_7"),
                                        Expression.Constant("observable_parameter_8"),
                                        Expression.Constant("observable_parameter_9"),
                                        Expression.Constant("observable_parameter_10"),
                                        Expression.Constant("observable_parameter_11"),
                                        Expression.Constant("observable_parameter_12"),
                                        Expression.Constant("observable_parameter_13"),
                                        Expression.Constant("observable_parameter_14")
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
                            Expression.Invoke(
                                Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQbserver<int>>), Constants.Observer.OB),
                                Expression.Constant("observer_parameter_1"),
                                Expression.Constant("observer_parameter_2"),
                                Expression.Constant("observer_parameter_3"),
                                Expression.Constant("observer_parameter_4"),
                                Expression.Constant("observer_parameter_5"),
                                Expression.Constant("observer_parameter_6"),
                                Expression.Constant("observer_parameter_7"),
                                Expression.Constant("observer_parameter_8"),
                                Expression.Constant("observer_parameter_9"),
                                Expression.Constant("observer_parameter_10"),
                                Expression.Constant("observer_parameter_11"),
                                Expression.Constant("observer_parameter_12"),
                                Expression.Constant("observer_parameter_13"),
                                Expression.Constant("observer_parameter_14")
                            )
                        )
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_Parameterized_Simple2_15P()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observable.XS));

                    var ys = from x in xs("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7", "observable_parameter_8", "observable_parameter_9", "observable_parameter_10", "observable_parameter_11", "observable_parameter_12", "observable_parameter_13", "observable_parameter_14", "observable_parameter_15")
                             where x > 0
                             select x * x;

                    var ob = ctx.GetObserver<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observer.OB));
                    ys.SubscribeAsync(ob("observer_parameter_1", "observer_parameter_2", "observer_parameter_3", "observer_parameter_4", "observer_parameter_5", "observer_parameter_6", "observer_parameter_7", "observer_parameter_8", "observer_parameter_9", "observer_parameter_10", "observer_parameter_11", "observer_parameter_12", "observer_parameter_13", "observer_parameter_14", "observer_parameter_15"), new Uri(Constants.Subscription.SUB), state: null).Wait();
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
                                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS),
                                        Expression.Constant("observable_parameter_1"),
                                        Expression.Constant("observable_parameter_2"),
                                        Expression.Constant("observable_parameter_3"),
                                        Expression.Constant("observable_parameter_4"),
                                        Expression.Constant("observable_parameter_5"),
                                        Expression.Constant("observable_parameter_6"),
                                        Expression.Constant("observable_parameter_7"),
                                        Expression.Constant("observable_parameter_8"),
                                        Expression.Constant("observable_parameter_9"),
                                        Expression.Constant("observable_parameter_10"),
                                        Expression.Constant("observable_parameter_11"),
                                        Expression.Constant("observable_parameter_12"),
                                        Expression.Constant("observable_parameter_13"),
                                        Expression.Constant("observable_parameter_14"),
                                        Expression.Constant("observable_parameter_15")
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
                            Expression.Invoke(
                                Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQbserver<int>>), Constants.Observer.OB),
                                Expression.Constant("observer_parameter_1"),
                                Expression.Constant("observer_parameter_2"),
                                Expression.Constant("observer_parameter_3"),
                                Expression.Constant("observer_parameter_4"),
                                Expression.Constant("observer_parameter_5"),
                                Expression.Constant("observer_parameter_6"),
                                Expression.Constant("observer_parameter_7"),
                                Expression.Constant("observer_parameter_8"),
                                Expression.Constant("observer_parameter_9"),
                                Expression.Constant("observer_parameter_10"),
                                Expression.Constant("observer_parameter_11"),
                                Expression.Constant("observer_parameter_12"),
                                Expression.Constant("observer_parameter_13"),
                                Expression.Constant("observer_parameter_14"),
                                Expression.Constant("observer_parameter_15")
                            )
                        )
                    ),
                    state: null
                )
            );
        }


        #endregion

        #region "ReactiveClientContext_SubscribeAsync_Parameterized_Simple3"

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_Parameterized_Simple3_2P()
        {
            Apply(
                provider => new MyParameterizedContext(provider),
                ctx =>
                {
                    var xs = ctx.Xs2p("observable_parameter_1", "observable_parameter_2");
                    var ob = ctx.GetObserver<string, string, int>(new Uri(Constants.Observer.OB));
                    xs.SubscribeAsync(ob("observer_parameter_1", "observer_parameter_2"), new Uri(Constants.Subscription.SUB), state: null).Wait();
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, IAsyncReactiveQbserver<int>, Task<IAsyncReactiveQubscription>>), Constants.SubscribeUri),
                        Expression.Invoke(
                            Expression.Parameter(typeof(Func<string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS + 2),
                            Expression.Constant("observable_parameter_1"),
                            Expression.Constant("observable_parameter_2")
                        ),
                        Expression.Invoke(
                            Expression.Parameter(typeof(Func<string, string, IAsyncReactiveQbserver<int>>), Constants.Observer.OB),
                            Expression.Constant("observer_parameter_1"),
                            Expression.Constant("observer_parameter_2")
                        )
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_Parameterized_Simple3_3P()
        {
            Apply(
                provider => new MyParameterizedContext(provider),
                ctx =>
                {
                    var xs = ctx.Xs3p("observable_parameter_1", "observable_parameter_2", "observable_parameter_3");
                    var ob = ctx.GetObserver<string, string, string, int>(new Uri(Constants.Observer.OB));
                    xs.SubscribeAsync(ob("observer_parameter_1", "observer_parameter_2", "observer_parameter_3"), new Uri(Constants.Subscription.SUB), state: null).Wait();
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, IAsyncReactiveQbserver<int>, Task<IAsyncReactiveQubscription>>), Constants.SubscribeUri),
                        Expression.Invoke(
                            Expression.Parameter(typeof(Func<string, string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS + 3),
                            Expression.Constant("observable_parameter_1"),
                            Expression.Constant("observable_parameter_2"),
                            Expression.Constant("observable_parameter_3")
                        ),
                        Expression.Invoke(
                            Expression.Parameter(typeof(Func<string, string, string, IAsyncReactiveQbserver<int>>), Constants.Observer.OB),
                            Expression.Constant("observer_parameter_1"),
                            Expression.Constant("observer_parameter_2"),
                            Expression.Constant("observer_parameter_3")
                        )
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_Parameterized_Simple3_4P()
        {
            Apply(
                provider => new MyParameterizedContext(provider),
                ctx =>
                {
                    var xs = ctx.Xs4p("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4");
                    var ob = ctx.GetObserver<string, string, string, string, int>(new Uri(Constants.Observer.OB));
                    xs.SubscribeAsync(ob("observer_parameter_1", "observer_parameter_2", "observer_parameter_3", "observer_parameter_4"), new Uri(Constants.Subscription.SUB), state: null).Wait();
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, IAsyncReactiveQbserver<int>, Task<IAsyncReactiveQubscription>>), Constants.SubscribeUri),
                        Expression.Invoke(
                            Expression.Parameter(typeof(Func<string, string, string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS + 4),
                            Expression.Constant("observable_parameter_1"),
                            Expression.Constant("observable_parameter_2"),
                            Expression.Constant("observable_parameter_3"),
                            Expression.Constant("observable_parameter_4")
                        ),
                        Expression.Invoke(
                            Expression.Parameter(typeof(Func<string, string, string, string, IAsyncReactiveQbserver<int>>), Constants.Observer.OB),
                            Expression.Constant("observer_parameter_1"),
                            Expression.Constant("observer_parameter_2"),
                            Expression.Constant("observer_parameter_3"),
                            Expression.Constant("observer_parameter_4")
                        )
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_Parameterized_Simple3_5P()
        {
            Apply(
                provider => new MyParameterizedContext(provider),
                ctx =>
                {
                    var xs = ctx.Xs5p("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5");
                    var ob = ctx.GetObserver<string, string, string, string, string, int>(new Uri(Constants.Observer.OB));
                    xs.SubscribeAsync(ob("observer_parameter_1", "observer_parameter_2", "observer_parameter_3", "observer_parameter_4", "observer_parameter_5"), new Uri(Constants.Subscription.SUB), state: null).Wait();
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, IAsyncReactiveQbserver<int>, Task<IAsyncReactiveQubscription>>), Constants.SubscribeUri),
                        Expression.Invoke(
                            Expression.Parameter(typeof(Func<string, string, string, string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS + 5),
                            Expression.Constant("observable_parameter_1"),
                            Expression.Constant("observable_parameter_2"),
                            Expression.Constant("observable_parameter_3"),
                            Expression.Constant("observable_parameter_4"),
                            Expression.Constant("observable_parameter_5")
                        ),
                        Expression.Invoke(
                            Expression.Parameter(typeof(Func<string, string, string, string, string, IAsyncReactiveQbserver<int>>), Constants.Observer.OB),
                            Expression.Constant("observer_parameter_1"),
                            Expression.Constant("observer_parameter_2"),
                            Expression.Constant("observer_parameter_3"),
                            Expression.Constant("observer_parameter_4"),
                            Expression.Constant("observer_parameter_5")
                        )
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_Parameterized_Simple3_6P()
        {
            Apply(
                provider => new MyParameterizedContext(provider),
                ctx =>
                {
                    var xs = ctx.Xs6p("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6");
                    var ob = ctx.GetObserver<string, string, string, string, string, string, int>(new Uri(Constants.Observer.OB));
                    xs.SubscribeAsync(ob("observer_parameter_1", "observer_parameter_2", "observer_parameter_3", "observer_parameter_4", "observer_parameter_5", "observer_parameter_6"), new Uri(Constants.Subscription.SUB), state: null).Wait();
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, IAsyncReactiveQbserver<int>, Task<IAsyncReactiveQubscription>>), Constants.SubscribeUri),
                        Expression.Invoke(
                            Expression.Parameter(typeof(Func<string, string, string, string, string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS + 6),
                            Expression.Constant("observable_parameter_1"),
                            Expression.Constant("observable_parameter_2"),
                            Expression.Constant("observable_parameter_3"),
                            Expression.Constant("observable_parameter_4"),
                            Expression.Constant("observable_parameter_5"),
                            Expression.Constant("observable_parameter_6")
                        ),
                        Expression.Invoke(
                            Expression.Parameter(typeof(Func<string, string, string, string, string, string, IAsyncReactiveQbserver<int>>), Constants.Observer.OB),
                            Expression.Constant("observer_parameter_1"),
                            Expression.Constant("observer_parameter_2"),
                            Expression.Constant("observer_parameter_3"),
                            Expression.Constant("observer_parameter_4"),
                            Expression.Constant("observer_parameter_5"),
                            Expression.Constant("observer_parameter_6")
                        )
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_Parameterized_Simple3_7P()
        {
            Apply(
                provider => new MyParameterizedContext(provider),
                ctx =>
                {
                    var xs = ctx.Xs7p("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7");
                    var ob = ctx.GetObserver<string, string, string, string, string, string, string, int>(new Uri(Constants.Observer.OB));
                    xs.SubscribeAsync(ob("observer_parameter_1", "observer_parameter_2", "observer_parameter_3", "observer_parameter_4", "observer_parameter_5", "observer_parameter_6", "observer_parameter_7"), new Uri(Constants.Subscription.SUB), state: null).Wait();
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, IAsyncReactiveQbserver<int>, Task<IAsyncReactiveQubscription>>), Constants.SubscribeUri),
                        Expression.Invoke(
                            Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS + 7),
                            Expression.Constant("observable_parameter_1"),
                            Expression.Constant("observable_parameter_2"),
                            Expression.Constant("observable_parameter_3"),
                            Expression.Constant("observable_parameter_4"),
                            Expression.Constant("observable_parameter_5"),
                            Expression.Constant("observable_parameter_6"),
                            Expression.Constant("observable_parameter_7")
                        ),
                        Expression.Invoke(
                            Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, IAsyncReactiveQbserver<int>>), Constants.Observer.OB),
                            Expression.Constant("observer_parameter_1"),
                            Expression.Constant("observer_parameter_2"),
                            Expression.Constant("observer_parameter_3"),
                            Expression.Constant("observer_parameter_4"),
                            Expression.Constant("observer_parameter_5"),
                            Expression.Constant("observer_parameter_6"),
                            Expression.Constant("observer_parameter_7")
                        )
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_Parameterized_Simple3_8P()
        {
            Apply(
                provider => new MyParameterizedContext(provider),
                ctx =>
                {
                    var xs = ctx.Xs8p("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7", "observable_parameter_8");
                    var ob = ctx.GetObserver<string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observer.OB));
                    xs.SubscribeAsync(ob("observer_parameter_1", "observer_parameter_2", "observer_parameter_3", "observer_parameter_4", "observer_parameter_5", "observer_parameter_6", "observer_parameter_7", "observer_parameter_8"), new Uri(Constants.Subscription.SUB), state: null).Wait();
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, IAsyncReactiveQbserver<int>, Task<IAsyncReactiveQubscription>>), Constants.SubscribeUri),
                        Expression.Invoke(
                            Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS + 8),
                            Expression.Constant("observable_parameter_1"),
                            Expression.Constant("observable_parameter_2"),
                            Expression.Constant("observable_parameter_3"),
                            Expression.Constant("observable_parameter_4"),
                            Expression.Constant("observable_parameter_5"),
                            Expression.Constant("observable_parameter_6"),
                            Expression.Constant("observable_parameter_7"),
                            Expression.Constant("observable_parameter_8")
                        ),
                        Expression.Invoke(
                            Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, IAsyncReactiveQbserver<int>>), Constants.Observer.OB),
                            Expression.Constant("observer_parameter_1"),
                            Expression.Constant("observer_parameter_2"),
                            Expression.Constant("observer_parameter_3"),
                            Expression.Constant("observer_parameter_4"),
                            Expression.Constant("observer_parameter_5"),
                            Expression.Constant("observer_parameter_6"),
                            Expression.Constant("observer_parameter_7"),
                            Expression.Constant("observer_parameter_8")
                        )
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_Parameterized_Simple3_9P()
        {
            Apply(
                provider => new MyParameterizedContext(provider),
                ctx =>
                {
                    var xs = ctx.Xs9p("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7", "observable_parameter_8", "observable_parameter_9");
                    var ob = ctx.GetObserver<string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observer.OB));
                    xs.SubscribeAsync(ob("observer_parameter_1", "observer_parameter_2", "observer_parameter_3", "observer_parameter_4", "observer_parameter_5", "observer_parameter_6", "observer_parameter_7", "observer_parameter_8", "observer_parameter_9"), new Uri(Constants.Subscription.SUB), state: null).Wait();
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, IAsyncReactiveQbserver<int>, Task<IAsyncReactiveQubscription>>), Constants.SubscribeUri),
                        Expression.Invoke(
                            Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS + 9),
                            Expression.Constant("observable_parameter_1"),
                            Expression.Constant("observable_parameter_2"),
                            Expression.Constant("observable_parameter_3"),
                            Expression.Constant("observable_parameter_4"),
                            Expression.Constant("observable_parameter_5"),
                            Expression.Constant("observable_parameter_6"),
                            Expression.Constant("observable_parameter_7"),
                            Expression.Constant("observable_parameter_8"),
                            Expression.Constant("observable_parameter_9")
                        ),
                        Expression.Invoke(
                            Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, IAsyncReactiveQbserver<int>>), Constants.Observer.OB),
                            Expression.Constant("observer_parameter_1"),
                            Expression.Constant("observer_parameter_2"),
                            Expression.Constant("observer_parameter_3"),
                            Expression.Constant("observer_parameter_4"),
                            Expression.Constant("observer_parameter_5"),
                            Expression.Constant("observer_parameter_6"),
                            Expression.Constant("observer_parameter_7"),
                            Expression.Constant("observer_parameter_8"),
                            Expression.Constant("observer_parameter_9")
                        )
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_Parameterized_Simple3_10P()
        {
            Apply(
                provider => new MyParameterizedContext(provider),
                ctx =>
                {
                    var xs = ctx.Xs10p("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7", "observable_parameter_8", "observable_parameter_9", "observable_parameter_10");
                    var ob = ctx.GetObserver<string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observer.OB));
                    xs.SubscribeAsync(ob("observer_parameter_1", "observer_parameter_2", "observer_parameter_3", "observer_parameter_4", "observer_parameter_5", "observer_parameter_6", "observer_parameter_7", "observer_parameter_8", "observer_parameter_9", "observer_parameter_10"), new Uri(Constants.Subscription.SUB), state: null).Wait();
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, IAsyncReactiveQbserver<int>, Task<IAsyncReactiveQubscription>>), Constants.SubscribeUri),
                        Expression.Invoke(
                            Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS + 10),
                            Expression.Constant("observable_parameter_1"),
                            Expression.Constant("observable_parameter_2"),
                            Expression.Constant("observable_parameter_3"),
                            Expression.Constant("observable_parameter_4"),
                            Expression.Constant("observable_parameter_5"),
                            Expression.Constant("observable_parameter_6"),
                            Expression.Constant("observable_parameter_7"),
                            Expression.Constant("observable_parameter_8"),
                            Expression.Constant("observable_parameter_9"),
                            Expression.Constant("observable_parameter_10")
                        ),
                        Expression.Invoke(
                            Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQbserver<int>>), Constants.Observer.OB),
                            Expression.Constant("observer_parameter_1"),
                            Expression.Constant("observer_parameter_2"),
                            Expression.Constant("observer_parameter_3"),
                            Expression.Constant("observer_parameter_4"),
                            Expression.Constant("observer_parameter_5"),
                            Expression.Constant("observer_parameter_6"),
                            Expression.Constant("observer_parameter_7"),
                            Expression.Constant("observer_parameter_8"),
                            Expression.Constant("observer_parameter_9"),
                            Expression.Constant("observer_parameter_10")
                        )
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_Parameterized_Simple3_11P()
        {
            Apply(
                provider => new MyParameterizedContext(provider),
                ctx =>
                {
                    var xs = ctx.Xs11p("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7", "observable_parameter_8", "observable_parameter_9", "observable_parameter_10", "observable_parameter_11");
                    var ob = ctx.GetObserver<string, string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observer.OB));
                    xs.SubscribeAsync(ob("observer_parameter_1", "observer_parameter_2", "observer_parameter_3", "observer_parameter_4", "observer_parameter_5", "observer_parameter_6", "observer_parameter_7", "observer_parameter_8", "observer_parameter_9", "observer_parameter_10", "observer_parameter_11"), new Uri(Constants.Subscription.SUB), state: null).Wait();
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, IAsyncReactiveQbserver<int>, Task<IAsyncReactiveQubscription>>), Constants.SubscribeUri),
                        Expression.Invoke(
                            Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS + 11),
                            Expression.Constant("observable_parameter_1"),
                            Expression.Constant("observable_parameter_2"),
                            Expression.Constant("observable_parameter_3"),
                            Expression.Constant("observable_parameter_4"),
                            Expression.Constant("observable_parameter_5"),
                            Expression.Constant("observable_parameter_6"),
                            Expression.Constant("observable_parameter_7"),
                            Expression.Constant("observable_parameter_8"),
                            Expression.Constant("observable_parameter_9"),
                            Expression.Constant("observable_parameter_10"),
                            Expression.Constant("observable_parameter_11")
                        ),
                        Expression.Invoke(
                            Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQbserver<int>>), Constants.Observer.OB),
                            Expression.Constant("observer_parameter_1"),
                            Expression.Constant("observer_parameter_2"),
                            Expression.Constant("observer_parameter_3"),
                            Expression.Constant("observer_parameter_4"),
                            Expression.Constant("observer_parameter_5"),
                            Expression.Constant("observer_parameter_6"),
                            Expression.Constant("observer_parameter_7"),
                            Expression.Constant("observer_parameter_8"),
                            Expression.Constant("observer_parameter_9"),
                            Expression.Constant("observer_parameter_10"),
                            Expression.Constant("observer_parameter_11")
                        )
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_Parameterized_Simple3_12P()
        {
            Apply(
                provider => new MyParameterizedContext(provider),
                ctx =>
                {
                    var xs = ctx.Xs12p("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7", "observable_parameter_8", "observable_parameter_9", "observable_parameter_10", "observable_parameter_11", "observable_parameter_12");
                    var ob = ctx.GetObserver<string, string, string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observer.OB));
                    xs.SubscribeAsync(ob("observer_parameter_1", "observer_parameter_2", "observer_parameter_3", "observer_parameter_4", "observer_parameter_5", "observer_parameter_6", "observer_parameter_7", "observer_parameter_8", "observer_parameter_9", "observer_parameter_10", "observer_parameter_11", "observer_parameter_12"), new Uri(Constants.Subscription.SUB), state: null).Wait();
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, IAsyncReactiveQbserver<int>, Task<IAsyncReactiveQubscription>>), Constants.SubscribeUri),
                        Expression.Invoke(
                            Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS + 12),
                            Expression.Constant("observable_parameter_1"),
                            Expression.Constant("observable_parameter_2"),
                            Expression.Constant("observable_parameter_3"),
                            Expression.Constant("observable_parameter_4"),
                            Expression.Constant("observable_parameter_5"),
                            Expression.Constant("observable_parameter_6"),
                            Expression.Constant("observable_parameter_7"),
                            Expression.Constant("observable_parameter_8"),
                            Expression.Constant("observable_parameter_9"),
                            Expression.Constant("observable_parameter_10"),
                            Expression.Constant("observable_parameter_11"),
                            Expression.Constant("observable_parameter_12")
                        ),
                        Expression.Invoke(
                            Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQbserver<int>>), Constants.Observer.OB),
                            Expression.Constant("observer_parameter_1"),
                            Expression.Constant("observer_parameter_2"),
                            Expression.Constant("observer_parameter_3"),
                            Expression.Constant("observer_parameter_4"),
                            Expression.Constant("observer_parameter_5"),
                            Expression.Constant("observer_parameter_6"),
                            Expression.Constant("observer_parameter_7"),
                            Expression.Constant("observer_parameter_8"),
                            Expression.Constant("observer_parameter_9"),
                            Expression.Constant("observer_parameter_10"),
                            Expression.Constant("observer_parameter_11"),
                            Expression.Constant("observer_parameter_12")
                        )
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_Parameterized_Simple3_13P()
        {
            Apply(
                provider => new MyParameterizedContext(provider),
                ctx =>
                {
                    var xs = ctx.Xs13p("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7", "observable_parameter_8", "observable_parameter_9", "observable_parameter_10", "observable_parameter_11", "observable_parameter_12", "observable_parameter_13");
                    var ob = ctx.GetObserver<string, string, string, string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observer.OB));
                    xs.SubscribeAsync(ob("observer_parameter_1", "observer_parameter_2", "observer_parameter_3", "observer_parameter_4", "observer_parameter_5", "observer_parameter_6", "observer_parameter_7", "observer_parameter_8", "observer_parameter_9", "observer_parameter_10", "observer_parameter_11", "observer_parameter_12", "observer_parameter_13"), new Uri(Constants.Subscription.SUB), state: null).Wait();
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, IAsyncReactiveQbserver<int>, Task<IAsyncReactiveQubscription>>), Constants.SubscribeUri),
                        Expression.Invoke(
                            Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS + 13),
                            Expression.Constant("observable_parameter_1"),
                            Expression.Constant("observable_parameter_2"),
                            Expression.Constant("observable_parameter_3"),
                            Expression.Constant("observable_parameter_4"),
                            Expression.Constant("observable_parameter_5"),
                            Expression.Constant("observable_parameter_6"),
                            Expression.Constant("observable_parameter_7"),
                            Expression.Constant("observable_parameter_8"),
                            Expression.Constant("observable_parameter_9"),
                            Expression.Constant("observable_parameter_10"),
                            Expression.Constant("observable_parameter_11"),
                            Expression.Constant("observable_parameter_12"),
                            Expression.Constant("observable_parameter_13")
                        ),
                        Expression.Invoke(
                            Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQbserver<int>>), Constants.Observer.OB),
                            Expression.Constant("observer_parameter_1"),
                            Expression.Constant("observer_parameter_2"),
                            Expression.Constant("observer_parameter_3"),
                            Expression.Constant("observer_parameter_4"),
                            Expression.Constant("observer_parameter_5"),
                            Expression.Constant("observer_parameter_6"),
                            Expression.Constant("observer_parameter_7"),
                            Expression.Constant("observer_parameter_8"),
                            Expression.Constant("observer_parameter_9"),
                            Expression.Constant("observer_parameter_10"),
                            Expression.Constant("observer_parameter_11"),
                            Expression.Constant("observer_parameter_12"),
                            Expression.Constant("observer_parameter_13")
                        )
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_Parameterized_Simple3_14P()
        {
            Apply(
                provider => new MyParameterizedContext(provider),
                ctx =>
                {
                    var xs = ctx.Xs14p("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7", "observable_parameter_8", "observable_parameter_9", "observable_parameter_10", "observable_parameter_11", "observable_parameter_12", "observable_parameter_13", "observable_parameter_14");
                    var ob = ctx.GetObserver<string, string, string, string, string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observer.OB));
                    xs.SubscribeAsync(ob("observer_parameter_1", "observer_parameter_2", "observer_parameter_3", "observer_parameter_4", "observer_parameter_5", "observer_parameter_6", "observer_parameter_7", "observer_parameter_8", "observer_parameter_9", "observer_parameter_10", "observer_parameter_11", "observer_parameter_12", "observer_parameter_13", "observer_parameter_14"), new Uri(Constants.Subscription.SUB), state: null).Wait();
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, IAsyncReactiveQbserver<int>, Task<IAsyncReactiveQubscription>>), Constants.SubscribeUri),
                        Expression.Invoke(
                            Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS + 14),
                            Expression.Constant("observable_parameter_1"),
                            Expression.Constant("observable_parameter_2"),
                            Expression.Constant("observable_parameter_3"),
                            Expression.Constant("observable_parameter_4"),
                            Expression.Constant("observable_parameter_5"),
                            Expression.Constant("observable_parameter_6"),
                            Expression.Constant("observable_parameter_7"),
                            Expression.Constant("observable_parameter_8"),
                            Expression.Constant("observable_parameter_9"),
                            Expression.Constant("observable_parameter_10"),
                            Expression.Constant("observable_parameter_11"),
                            Expression.Constant("observable_parameter_12"),
                            Expression.Constant("observable_parameter_13"),
                            Expression.Constant("observable_parameter_14")
                        ),
                        Expression.Invoke(
                            Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQbserver<int>>), Constants.Observer.OB),
                            Expression.Constant("observer_parameter_1"),
                            Expression.Constant("observer_parameter_2"),
                            Expression.Constant("observer_parameter_3"),
                            Expression.Constant("observer_parameter_4"),
                            Expression.Constant("observer_parameter_5"),
                            Expression.Constant("observer_parameter_6"),
                            Expression.Constant("observer_parameter_7"),
                            Expression.Constant("observer_parameter_8"),
                            Expression.Constant("observer_parameter_9"),
                            Expression.Constant("observer_parameter_10"),
                            Expression.Constant("observer_parameter_11"),
                            Expression.Constant("observer_parameter_12"),
                            Expression.Constant("observer_parameter_13"),
                            Expression.Constant("observer_parameter_14")
                        )
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_Parameterized_Simple3_15P()
        {
            Apply(
                provider => new MyParameterizedContext(provider),
                ctx =>
                {
                    var xs = ctx.Xs15p("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7", "observable_parameter_8", "observable_parameter_9", "observable_parameter_10", "observable_parameter_11", "observable_parameter_12", "observable_parameter_13", "observable_parameter_14", "observable_parameter_15");
                    var ob = ctx.GetObserver<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observer.OB));
                    xs.SubscribeAsync(ob("observer_parameter_1", "observer_parameter_2", "observer_parameter_3", "observer_parameter_4", "observer_parameter_5", "observer_parameter_6", "observer_parameter_7", "observer_parameter_8", "observer_parameter_9", "observer_parameter_10", "observer_parameter_11", "observer_parameter_12", "observer_parameter_13", "observer_parameter_14", "observer_parameter_15"), new Uri(Constants.Subscription.SUB), state: null).Wait();
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, IAsyncReactiveQbserver<int>, Task<IAsyncReactiveQubscription>>), Constants.SubscribeUri),
                        Expression.Invoke(
                            Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS + 15),
                            Expression.Constant("observable_parameter_1"),
                            Expression.Constant("observable_parameter_2"),
                            Expression.Constant("observable_parameter_3"),
                            Expression.Constant("observable_parameter_4"),
                            Expression.Constant("observable_parameter_5"),
                            Expression.Constant("observable_parameter_6"),
                            Expression.Constant("observable_parameter_7"),
                            Expression.Constant("observable_parameter_8"),
                            Expression.Constant("observable_parameter_9"),
                            Expression.Constant("observable_parameter_10"),
                            Expression.Constant("observable_parameter_11"),
                            Expression.Constant("observable_parameter_12"),
                            Expression.Constant("observable_parameter_13"),
                            Expression.Constant("observable_parameter_14"),
                            Expression.Constant("observable_parameter_15")
                        ),
                        Expression.Invoke(
                            Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQbserver<int>>), Constants.Observer.OB),
                            Expression.Constant("observer_parameter_1"),
                            Expression.Constant("observer_parameter_2"),
                            Expression.Constant("observer_parameter_3"),
                            Expression.Constant("observer_parameter_4"),
                            Expression.Constant("observer_parameter_5"),
                            Expression.Constant("observer_parameter_6"),
                            Expression.Constant("observer_parameter_7"),
                            Expression.Constant("observer_parameter_8"),
                            Expression.Constant("observer_parameter_9"),
                            Expression.Constant("observer_parameter_10"),
                            Expression.Constant("observer_parameter_11"),
                            Expression.Constant("observer_parameter_12"),
                            Expression.Constant("observer_parameter_13"),
                            Expression.Constant("observer_parameter_14"),
                            Expression.Constant("observer_parameter_15")
                        )
                    ),
                    state: null
                )
            );
        }


        #endregion

        #region "ReactiveClientContext_SubscribeAsync_Parameterized_Closure1"

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_Parameterized_Closure1_2P()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<string, string, int>(new Uri(Constants.Observable.XS));

                    var a = -1;

                    var ys = from x in xs("observable_parameter_1", "observable_parameter_2")
                             where x > a
                             select x * x;

                    var ob = ctx.GetObserver<string, string, int>(new Uri(Constants.Observer.OB));

                    for (var i = 0; i < 3; i++)
                    {
                        a = i;
                        ys.SubscribeAsync(ob("observer_parameter_1", "observer_parameter_2"), new Uri(Constants.Subscription.SUB), state: null).Wait();
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
                                        Expression.Invoke(
                                            Expression.Parameter(typeof(Func<string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS),
                                            Expression.Constant("observable_parameter_1"),
                                            Expression.Constant("observable_parameter_2")
                                        ),
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
                                Expression.Invoke(
                                    Expression.Parameter(typeof(Func<string, string, IAsyncReactiveQbserver<int>>), Constants.Observer.OB),
                                    Expression.Constant("observer_parameter_1"),
                                    Expression.Constant("observer_parameter_2")

                                )
                            )
                        ),
                        state: null
                    )
                ).ToArray()
            );
        }

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_Parameterized_Closure1_3P()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<string, string, string, int>(new Uri(Constants.Observable.XS));

                    var a = -1;

                    var ys = from x in xs("observable_parameter_1", "observable_parameter_2", "observable_parameter_3")
                             where x > a
                             select x * x;

                    var ob = ctx.GetObserver<string, string, string, int>(new Uri(Constants.Observer.OB));

                    for (var i = 0; i < 3; i++)
                    {
                        a = i;
                        ys.SubscribeAsync(ob("observer_parameter_1", "observer_parameter_2", "observer_parameter_3"), new Uri(Constants.Subscription.SUB), state: null).Wait();
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
                                        Expression.Invoke(
                                            Expression.Parameter(typeof(Func<string, string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS),
                                            Expression.Constant("observable_parameter_1"),
                                            Expression.Constant("observable_parameter_2"),
                                            Expression.Constant("observable_parameter_3")
                                        ),
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
                                Expression.Invoke(
                                    Expression.Parameter(typeof(Func<string, string, string, IAsyncReactiveQbserver<int>>), Constants.Observer.OB),
                                    Expression.Constant("observer_parameter_1"),
                                    Expression.Constant("observer_parameter_2"),
                                    Expression.Constant("observer_parameter_3")

                                )
                            )
                        ),
                        state: null
                    )
                ).ToArray()
            );
        }

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_Parameterized_Closure1_4P()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<string, string, string, string, int>(new Uri(Constants.Observable.XS));

                    var a = -1;

                    var ys = from x in xs("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4")
                             where x > a
                             select x * x;

                    var ob = ctx.GetObserver<string, string, string, string, int>(new Uri(Constants.Observer.OB));

                    for (var i = 0; i < 3; i++)
                    {
                        a = i;
                        ys.SubscribeAsync(ob("observer_parameter_1", "observer_parameter_2", "observer_parameter_3", "observer_parameter_4"), new Uri(Constants.Subscription.SUB), state: null).Wait();
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
                                        Expression.Invoke(
                                            Expression.Parameter(typeof(Func<string, string, string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS),
                                            Expression.Constant("observable_parameter_1"),
                                            Expression.Constant("observable_parameter_2"),
                                            Expression.Constant("observable_parameter_3"),
                                            Expression.Constant("observable_parameter_4")
                                        ),
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
                                Expression.Invoke(
                                    Expression.Parameter(typeof(Func<string, string, string, string, IAsyncReactiveQbserver<int>>), Constants.Observer.OB),
                                    Expression.Constant("observer_parameter_1"),
                                    Expression.Constant("observer_parameter_2"),
                                    Expression.Constant("observer_parameter_3"),
                                    Expression.Constant("observer_parameter_4")

                                )
                            )
                        ),
                        state: null
                    )
                ).ToArray()
            );
        }

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_Parameterized_Closure1_5P()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<string, string, string, string, string, int>(new Uri(Constants.Observable.XS));

                    var a = -1;

                    var ys = from x in xs("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5")
                             where x > a
                             select x * x;

                    var ob = ctx.GetObserver<string, string, string, string, string, int>(new Uri(Constants.Observer.OB));

                    for (var i = 0; i < 3; i++)
                    {
                        a = i;
                        ys.SubscribeAsync(ob("observer_parameter_1", "observer_parameter_2", "observer_parameter_3", "observer_parameter_4", "observer_parameter_5"), new Uri(Constants.Subscription.SUB), state: null).Wait();
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
                                        Expression.Invoke(
                                            Expression.Parameter(typeof(Func<string, string, string, string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS),
                                            Expression.Constant("observable_parameter_1"),
                                            Expression.Constant("observable_parameter_2"),
                                            Expression.Constant("observable_parameter_3"),
                                            Expression.Constant("observable_parameter_4"),
                                            Expression.Constant("observable_parameter_5")
                                        ),
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
                                Expression.Invoke(
                                    Expression.Parameter(typeof(Func<string, string, string, string, string, IAsyncReactiveQbserver<int>>), Constants.Observer.OB),
                                    Expression.Constant("observer_parameter_1"),
                                    Expression.Constant("observer_parameter_2"),
                                    Expression.Constant("observer_parameter_3"),
                                    Expression.Constant("observer_parameter_4"),
                                    Expression.Constant("observer_parameter_5")

                                )
                            )
                        ),
                        state: null
                    )
                ).ToArray()
            );
        }

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_Parameterized_Closure1_6P()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<string, string, string, string, string, string, int>(new Uri(Constants.Observable.XS));

                    var a = -1;

                    var ys = from x in xs("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6")
                             where x > a
                             select x * x;

                    var ob = ctx.GetObserver<string, string, string, string, string, string, int>(new Uri(Constants.Observer.OB));

                    for (var i = 0; i < 3; i++)
                    {
                        a = i;
                        ys.SubscribeAsync(ob("observer_parameter_1", "observer_parameter_2", "observer_parameter_3", "observer_parameter_4", "observer_parameter_5", "observer_parameter_6"), new Uri(Constants.Subscription.SUB), state: null).Wait();
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
                                        Expression.Invoke(
                                            Expression.Parameter(typeof(Func<string, string, string, string, string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS),
                                            Expression.Constant("observable_parameter_1"),
                                            Expression.Constant("observable_parameter_2"),
                                            Expression.Constant("observable_parameter_3"),
                                            Expression.Constant("observable_parameter_4"),
                                            Expression.Constant("observable_parameter_5"),
                                            Expression.Constant("observable_parameter_6")
                                        ),
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
                                Expression.Invoke(
                                    Expression.Parameter(typeof(Func<string, string, string, string, string, string, IAsyncReactiveQbserver<int>>), Constants.Observer.OB),
                                    Expression.Constant("observer_parameter_1"),
                                    Expression.Constant("observer_parameter_2"),
                                    Expression.Constant("observer_parameter_3"),
                                    Expression.Constant("observer_parameter_4"),
                                    Expression.Constant("observer_parameter_5"),
                                    Expression.Constant("observer_parameter_6")

                                )
                            )
                        ),
                        state: null
                    )
                ).ToArray()
            );
        }

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_Parameterized_Closure1_7P()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<string, string, string, string, string, string, string, int>(new Uri(Constants.Observable.XS));

                    var a = -1;

                    var ys = from x in xs("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7")
                             where x > a
                             select x * x;

                    var ob = ctx.GetObserver<string, string, string, string, string, string, string, int>(new Uri(Constants.Observer.OB));

                    for (var i = 0; i < 3; i++)
                    {
                        a = i;
                        ys.SubscribeAsync(ob("observer_parameter_1", "observer_parameter_2", "observer_parameter_3", "observer_parameter_4", "observer_parameter_5", "observer_parameter_6", "observer_parameter_7"), new Uri(Constants.Subscription.SUB), state: null).Wait();
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
                                        Expression.Invoke(
                                            Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS),
                                            Expression.Constant("observable_parameter_1"),
                                            Expression.Constant("observable_parameter_2"),
                                            Expression.Constant("observable_parameter_3"),
                                            Expression.Constant("observable_parameter_4"),
                                            Expression.Constant("observable_parameter_5"),
                                            Expression.Constant("observable_parameter_6"),
                                            Expression.Constant("observable_parameter_7")
                                        ),
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
                                Expression.Invoke(
                                    Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, IAsyncReactiveQbserver<int>>), Constants.Observer.OB),
                                    Expression.Constant("observer_parameter_1"),
                                    Expression.Constant("observer_parameter_2"),
                                    Expression.Constant("observer_parameter_3"),
                                    Expression.Constant("observer_parameter_4"),
                                    Expression.Constant("observer_parameter_5"),
                                    Expression.Constant("observer_parameter_6"),
                                    Expression.Constant("observer_parameter_7")

                                )
                            )
                        ),
                        state: null
                    )
                ).ToArray()
            );
        }

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_Parameterized_Closure1_8P()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observable.XS));

                    var a = -1;

                    var ys = from x in xs("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7", "observable_parameter_8")
                             where x > a
                             select x * x;

                    var ob = ctx.GetObserver<string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observer.OB));

                    for (var i = 0; i < 3; i++)
                    {
                        a = i;
                        ys.SubscribeAsync(ob("observer_parameter_1", "observer_parameter_2", "observer_parameter_3", "observer_parameter_4", "observer_parameter_5", "observer_parameter_6", "observer_parameter_7", "observer_parameter_8"), new Uri(Constants.Subscription.SUB), state: null).Wait();
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
                                        Expression.Invoke(
                                            Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS),
                                            Expression.Constant("observable_parameter_1"),
                                            Expression.Constant("observable_parameter_2"),
                                            Expression.Constant("observable_parameter_3"),
                                            Expression.Constant("observable_parameter_4"),
                                            Expression.Constant("observable_parameter_5"),
                                            Expression.Constant("observable_parameter_6"),
                                            Expression.Constant("observable_parameter_7"),
                                            Expression.Constant("observable_parameter_8")
                                        ),
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
                                Expression.Invoke(
                                    Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, IAsyncReactiveQbserver<int>>), Constants.Observer.OB),
                                    Expression.Constant("observer_parameter_1"),
                                    Expression.Constant("observer_parameter_2"),
                                    Expression.Constant("observer_parameter_3"),
                                    Expression.Constant("observer_parameter_4"),
                                    Expression.Constant("observer_parameter_5"),
                                    Expression.Constant("observer_parameter_6"),
                                    Expression.Constant("observer_parameter_7"),
                                    Expression.Constant("observer_parameter_8")

                                )
                            )
                        ),
                        state: null
                    )
                ).ToArray()
            );
        }

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_Parameterized_Closure1_9P()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observable.XS));

                    var a = -1;

                    var ys = from x in xs("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7", "observable_parameter_8", "observable_parameter_9")
                             where x > a
                             select x * x;

                    var ob = ctx.GetObserver<string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observer.OB));

                    for (var i = 0; i < 3; i++)
                    {
                        a = i;
                        ys.SubscribeAsync(ob("observer_parameter_1", "observer_parameter_2", "observer_parameter_3", "observer_parameter_4", "observer_parameter_5", "observer_parameter_6", "observer_parameter_7", "observer_parameter_8", "observer_parameter_9"), new Uri(Constants.Subscription.SUB), state: null).Wait();
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
                                        Expression.Invoke(
                                            Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS),
                                            Expression.Constant("observable_parameter_1"),
                                            Expression.Constant("observable_parameter_2"),
                                            Expression.Constant("observable_parameter_3"),
                                            Expression.Constant("observable_parameter_4"),
                                            Expression.Constant("observable_parameter_5"),
                                            Expression.Constant("observable_parameter_6"),
                                            Expression.Constant("observable_parameter_7"),
                                            Expression.Constant("observable_parameter_8"),
                                            Expression.Constant("observable_parameter_9")
                                        ),
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
                                Expression.Invoke(
                                    Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, IAsyncReactiveQbserver<int>>), Constants.Observer.OB),
                                    Expression.Constant("observer_parameter_1"),
                                    Expression.Constant("observer_parameter_2"),
                                    Expression.Constant("observer_parameter_3"),
                                    Expression.Constant("observer_parameter_4"),
                                    Expression.Constant("observer_parameter_5"),
                                    Expression.Constant("observer_parameter_6"),
                                    Expression.Constant("observer_parameter_7"),
                                    Expression.Constant("observer_parameter_8"),
                                    Expression.Constant("observer_parameter_9")

                                )
                            )
                        ),
                        state: null
                    )
                ).ToArray()
            );
        }

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_Parameterized_Closure1_10P()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observable.XS));

                    var a = -1;

                    var ys = from x in xs("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7", "observable_parameter_8", "observable_parameter_9", "observable_parameter_10")
                             where x > a
                             select x * x;

                    var ob = ctx.GetObserver<string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observer.OB));

                    for (var i = 0; i < 3; i++)
                    {
                        a = i;
                        ys.SubscribeAsync(ob("observer_parameter_1", "observer_parameter_2", "observer_parameter_3", "observer_parameter_4", "observer_parameter_5", "observer_parameter_6", "observer_parameter_7", "observer_parameter_8", "observer_parameter_9", "observer_parameter_10"), new Uri(Constants.Subscription.SUB), state: null).Wait();
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
                                        Expression.Invoke(
                                            Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS),
                                            Expression.Constant("observable_parameter_1"),
                                            Expression.Constant("observable_parameter_2"),
                                            Expression.Constant("observable_parameter_3"),
                                            Expression.Constant("observable_parameter_4"),
                                            Expression.Constant("observable_parameter_5"),
                                            Expression.Constant("observable_parameter_6"),
                                            Expression.Constant("observable_parameter_7"),
                                            Expression.Constant("observable_parameter_8"),
                                            Expression.Constant("observable_parameter_9"),
                                            Expression.Constant("observable_parameter_10")
                                        ),
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
                                Expression.Invoke(
                                    Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQbserver<int>>), Constants.Observer.OB),
                                    Expression.Constant("observer_parameter_1"),
                                    Expression.Constant("observer_parameter_2"),
                                    Expression.Constant("observer_parameter_3"),
                                    Expression.Constant("observer_parameter_4"),
                                    Expression.Constant("observer_parameter_5"),
                                    Expression.Constant("observer_parameter_6"),
                                    Expression.Constant("observer_parameter_7"),
                                    Expression.Constant("observer_parameter_8"),
                                    Expression.Constant("observer_parameter_9"),
                                    Expression.Constant("observer_parameter_10")

                                )
                            )
                        ),
                        state: null
                    )
                ).ToArray()
            );
        }

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_Parameterized_Closure1_11P()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<string, string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observable.XS));

                    var a = -1;

                    var ys = from x in xs("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7", "observable_parameter_8", "observable_parameter_9", "observable_parameter_10", "observable_parameter_11")
                             where x > a
                             select x * x;

                    var ob = ctx.GetObserver<string, string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observer.OB));

                    for (var i = 0; i < 3; i++)
                    {
                        a = i;
                        ys.SubscribeAsync(ob("observer_parameter_1", "observer_parameter_2", "observer_parameter_3", "observer_parameter_4", "observer_parameter_5", "observer_parameter_6", "observer_parameter_7", "observer_parameter_8", "observer_parameter_9", "observer_parameter_10", "observer_parameter_11"), new Uri(Constants.Subscription.SUB), state: null).Wait();
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
                                        Expression.Invoke(
                                            Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS),
                                            Expression.Constant("observable_parameter_1"),
                                            Expression.Constant("observable_parameter_2"),
                                            Expression.Constant("observable_parameter_3"),
                                            Expression.Constant("observable_parameter_4"),
                                            Expression.Constant("observable_parameter_5"),
                                            Expression.Constant("observable_parameter_6"),
                                            Expression.Constant("observable_parameter_7"),
                                            Expression.Constant("observable_parameter_8"),
                                            Expression.Constant("observable_parameter_9"),
                                            Expression.Constant("observable_parameter_10"),
                                            Expression.Constant("observable_parameter_11")
                                        ),
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
                                Expression.Invoke(
                                    Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQbserver<int>>), Constants.Observer.OB),
                                    Expression.Constant("observer_parameter_1"),
                                    Expression.Constant("observer_parameter_2"),
                                    Expression.Constant("observer_parameter_3"),
                                    Expression.Constant("observer_parameter_4"),
                                    Expression.Constant("observer_parameter_5"),
                                    Expression.Constant("observer_parameter_6"),
                                    Expression.Constant("observer_parameter_7"),
                                    Expression.Constant("observer_parameter_8"),
                                    Expression.Constant("observer_parameter_9"),
                                    Expression.Constant("observer_parameter_10"),
                                    Expression.Constant("observer_parameter_11")

                                )
                            )
                        ),
                        state: null
                    )
                ).ToArray()
            );
        }

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_Parameterized_Closure1_12P()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<string, string, string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observable.XS));

                    var a = -1;

                    var ys = from x in xs("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7", "observable_parameter_8", "observable_parameter_9", "observable_parameter_10", "observable_parameter_11", "observable_parameter_12")
                             where x > a
                             select x * x;

                    var ob = ctx.GetObserver<string, string, string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observer.OB));

                    for (var i = 0; i < 3; i++)
                    {
                        a = i;
                        ys.SubscribeAsync(ob("observer_parameter_1", "observer_parameter_2", "observer_parameter_3", "observer_parameter_4", "observer_parameter_5", "observer_parameter_6", "observer_parameter_7", "observer_parameter_8", "observer_parameter_9", "observer_parameter_10", "observer_parameter_11", "observer_parameter_12"), new Uri(Constants.Subscription.SUB), state: null).Wait();
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
                                        Expression.Invoke(
                                            Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS),
                                            Expression.Constant("observable_parameter_1"),
                                            Expression.Constant("observable_parameter_2"),
                                            Expression.Constant("observable_parameter_3"),
                                            Expression.Constant("observable_parameter_4"),
                                            Expression.Constant("observable_parameter_5"),
                                            Expression.Constant("observable_parameter_6"),
                                            Expression.Constant("observable_parameter_7"),
                                            Expression.Constant("observable_parameter_8"),
                                            Expression.Constant("observable_parameter_9"),
                                            Expression.Constant("observable_parameter_10"),
                                            Expression.Constant("observable_parameter_11"),
                                            Expression.Constant("observable_parameter_12")
                                        ),
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
                                Expression.Invoke(
                                    Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQbserver<int>>), Constants.Observer.OB),
                                    Expression.Constant("observer_parameter_1"),
                                    Expression.Constant("observer_parameter_2"),
                                    Expression.Constant("observer_parameter_3"),
                                    Expression.Constant("observer_parameter_4"),
                                    Expression.Constant("observer_parameter_5"),
                                    Expression.Constant("observer_parameter_6"),
                                    Expression.Constant("observer_parameter_7"),
                                    Expression.Constant("observer_parameter_8"),
                                    Expression.Constant("observer_parameter_9"),
                                    Expression.Constant("observer_parameter_10"),
                                    Expression.Constant("observer_parameter_11"),
                                    Expression.Constant("observer_parameter_12")

                                )
                            )
                        ),
                        state: null
                    )
                ).ToArray()
            );
        }

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_Parameterized_Closure1_13P()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<string, string, string, string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observable.XS));

                    var a = -1;

                    var ys = from x in xs("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7", "observable_parameter_8", "observable_parameter_9", "observable_parameter_10", "observable_parameter_11", "observable_parameter_12", "observable_parameter_13")
                             where x > a
                             select x * x;

                    var ob = ctx.GetObserver<string, string, string, string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observer.OB));

                    for (var i = 0; i < 3; i++)
                    {
                        a = i;
                        ys.SubscribeAsync(ob("observer_parameter_1", "observer_parameter_2", "observer_parameter_3", "observer_parameter_4", "observer_parameter_5", "observer_parameter_6", "observer_parameter_7", "observer_parameter_8", "observer_parameter_9", "observer_parameter_10", "observer_parameter_11", "observer_parameter_12", "observer_parameter_13"), new Uri(Constants.Subscription.SUB), state: null).Wait();
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
                                        Expression.Invoke(
                                            Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS),
                                            Expression.Constant("observable_parameter_1"),
                                            Expression.Constant("observable_parameter_2"),
                                            Expression.Constant("observable_parameter_3"),
                                            Expression.Constant("observable_parameter_4"),
                                            Expression.Constant("observable_parameter_5"),
                                            Expression.Constant("observable_parameter_6"),
                                            Expression.Constant("observable_parameter_7"),
                                            Expression.Constant("observable_parameter_8"),
                                            Expression.Constant("observable_parameter_9"),
                                            Expression.Constant("observable_parameter_10"),
                                            Expression.Constant("observable_parameter_11"),
                                            Expression.Constant("observable_parameter_12"),
                                            Expression.Constant("observable_parameter_13")
                                        ),
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
                                Expression.Invoke(
                                    Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQbserver<int>>), Constants.Observer.OB),
                                    Expression.Constant("observer_parameter_1"),
                                    Expression.Constant("observer_parameter_2"),
                                    Expression.Constant("observer_parameter_3"),
                                    Expression.Constant("observer_parameter_4"),
                                    Expression.Constant("observer_parameter_5"),
                                    Expression.Constant("observer_parameter_6"),
                                    Expression.Constant("observer_parameter_7"),
                                    Expression.Constant("observer_parameter_8"),
                                    Expression.Constant("observer_parameter_9"),
                                    Expression.Constant("observer_parameter_10"),
                                    Expression.Constant("observer_parameter_11"),
                                    Expression.Constant("observer_parameter_12"),
                                    Expression.Constant("observer_parameter_13")

                                )
                            )
                        ),
                        state: null
                    )
                ).ToArray()
            );
        }

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_Parameterized_Closure1_14P()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<string, string, string, string, string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observable.XS));

                    var a = -1;

                    var ys = from x in xs("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7", "observable_parameter_8", "observable_parameter_9", "observable_parameter_10", "observable_parameter_11", "observable_parameter_12", "observable_parameter_13", "observable_parameter_14")
                             where x > a
                             select x * x;

                    var ob = ctx.GetObserver<string, string, string, string, string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observer.OB));

                    for (var i = 0; i < 3; i++)
                    {
                        a = i;
                        ys.SubscribeAsync(ob("observer_parameter_1", "observer_parameter_2", "observer_parameter_3", "observer_parameter_4", "observer_parameter_5", "observer_parameter_6", "observer_parameter_7", "observer_parameter_8", "observer_parameter_9", "observer_parameter_10", "observer_parameter_11", "observer_parameter_12", "observer_parameter_13", "observer_parameter_14"), new Uri(Constants.Subscription.SUB), state: null).Wait();
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
                                        Expression.Invoke(
                                            Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS),
                                            Expression.Constant("observable_parameter_1"),
                                            Expression.Constant("observable_parameter_2"),
                                            Expression.Constant("observable_parameter_3"),
                                            Expression.Constant("observable_parameter_4"),
                                            Expression.Constant("observable_parameter_5"),
                                            Expression.Constant("observable_parameter_6"),
                                            Expression.Constant("observable_parameter_7"),
                                            Expression.Constant("observable_parameter_8"),
                                            Expression.Constant("observable_parameter_9"),
                                            Expression.Constant("observable_parameter_10"),
                                            Expression.Constant("observable_parameter_11"),
                                            Expression.Constant("observable_parameter_12"),
                                            Expression.Constant("observable_parameter_13"),
                                            Expression.Constant("observable_parameter_14")
                                        ),
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
                                Expression.Invoke(
                                    Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQbserver<int>>), Constants.Observer.OB),
                                    Expression.Constant("observer_parameter_1"),
                                    Expression.Constant("observer_parameter_2"),
                                    Expression.Constant("observer_parameter_3"),
                                    Expression.Constant("observer_parameter_4"),
                                    Expression.Constant("observer_parameter_5"),
                                    Expression.Constant("observer_parameter_6"),
                                    Expression.Constant("observer_parameter_7"),
                                    Expression.Constant("observer_parameter_8"),
                                    Expression.Constant("observer_parameter_9"),
                                    Expression.Constant("observer_parameter_10"),
                                    Expression.Constant("observer_parameter_11"),
                                    Expression.Constant("observer_parameter_12"),
                                    Expression.Constant("observer_parameter_13"),
                                    Expression.Constant("observer_parameter_14")

                                )
                            )
                        ),
                        state: null
                    )
                ).ToArray()
            );
        }

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_Parameterized_Closure1_15P()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observable.XS));

                    var a = -1;

                    var ys = from x in xs("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7", "observable_parameter_8", "observable_parameter_9", "observable_parameter_10", "observable_parameter_11", "observable_parameter_12", "observable_parameter_13", "observable_parameter_14", "observable_parameter_15")
                             where x > a
                             select x * x;

                    var ob = ctx.GetObserver<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observer.OB));

                    for (var i = 0; i < 3; i++)
                    {
                        a = i;
                        ys.SubscribeAsync(ob("observer_parameter_1", "observer_parameter_2", "observer_parameter_3", "observer_parameter_4", "observer_parameter_5", "observer_parameter_6", "observer_parameter_7", "observer_parameter_8", "observer_parameter_9", "observer_parameter_10", "observer_parameter_11", "observer_parameter_12", "observer_parameter_13", "observer_parameter_14", "observer_parameter_15"), new Uri(Constants.Subscription.SUB), state: null).Wait();
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
                                        Expression.Invoke(
                                            Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS),
                                            Expression.Constant("observable_parameter_1"),
                                            Expression.Constant("observable_parameter_2"),
                                            Expression.Constant("observable_parameter_3"),
                                            Expression.Constant("observable_parameter_4"),
                                            Expression.Constant("observable_parameter_5"),
                                            Expression.Constant("observable_parameter_6"),
                                            Expression.Constant("observable_parameter_7"),
                                            Expression.Constant("observable_parameter_8"),
                                            Expression.Constant("observable_parameter_9"),
                                            Expression.Constant("observable_parameter_10"),
                                            Expression.Constant("observable_parameter_11"),
                                            Expression.Constant("observable_parameter_12"),
                                            Expression.Constant("observable_parameter_13"),
                                            Expression.Constant("observable_parameter_14"),
                                            Expression.Constant("observable_parameter_15")
                                        ),
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
                                Expression.Invoke(
                                    Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQbserver<int>>), Constants.Observer.OB),
                                    Expression.Constant("observer_parameter_1"),
                                    Expression.Constant("observer_parameter_2"),
                                    Expression.Constant("observer_parameter_3"),
                                    Expression.Constant("observer_parameter_4"),
                                    Expression.Constant("observer_parameter_5"),
                                    Expression.Constant("observer_parameter_6"),
                                    Expression.Constant("observer_parameter_7"),
                                    Expression.Constant("observer_parameter_8"),
                                    Expression.Constant("observer_parameter_9"),
                                    Expression.Constant("observer_parameter_10"),
                                    Expression.Constant("observer_parameter_11"),
                                    Expression.Constant("observer_parameter_12"),
                                    Expression.Constant("observer_parameter_13"),
                                    Expression.Constant("observer_parameter_14"),
                                    Expression.Constant("observer_parameter_15")

                                )
                            )
                        ),
                        state: null
                    )
                ).ToArray()
            );
        }


        #endregion

        #region "ReactiveClientContext_SubscribeAsync_Parameterized_SelectMany1_Simple"

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_Parameterized_SelectMany1_Simple_2P()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<string, string, int>(new Uri(Constants.Observable.XS));
                    var ys = ctx.GetObservable<int, int, string>(new Uri(Constants.Observable.YS));

                    var zs = from x in xs("observable_parameter_1", "observable_parameter_2")
                             from y in ys(x, x)
                             select x + y.Length;

                    var ob = ctx.GetObserver<int>(new Uri(Constants.Observer.OB));
                    zs.SubscribeAsync(ob, new Uri(Constants.Subscription.SUB), state: null).Wait();
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Parameter(typeof(int), "x").Let(x =>
                        Expression.Parameter(typeof(string), "y").Let(y =>
                            Expression.Invoke(
                                Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, IAsyncReactiveQbserver<int>, Task<IAsyncReactiveQubscription>>), Constants.SubscribeUri),
                                Expression.Invoke(
                                    Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, Expression<Func<int, IAsyncReactiveQbservable<string>>>, Expression<Func<int, string, int>>, IAsyncReactiveQbservable<int>>), Constants.Observable.SelectMany),
                                    Expression.Invoke(
                                        Expression.Parameter(typeof(Func<string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS),
                                        Expression.Constant("observable_parameter_1"),
                                        Expression.Constant("observable_parameter_2")
                                    ),
                                    Expression.Lambda(
                                        Expression.Invoke(
                                        Expression.Parameter(typeof(Func<int, int, IAsyncReactiveQbservable<string>>), Constants.Observable.YS),
                                            x, x
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
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_Parameterized_SelectMany1_Simple_3P()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<string, string, string, int>(new Uri(Constants.Observable.XS));
                    var ys = ctx.GetObservable<int, int, int, string>(new Uri(Constants.Observable.YS));

                    var zs = from x in xs("observable_parameter_1", "observable_parameter_2", "observable_parameter_3")
                             from y in ys(x, x, x)
                             select x + y.Length;

                    var ob = ctx.GetObserver<int>(new Uri(Constants.Observer.OB));
                    zs.SubscribeAsync(ob, new Uri(Constants.Subscription.SUB), state: null).Wait();
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Parameter(typeof(int), "x").Let(x =>
                        Expression.Parameter(typeof(string), "y").Let(y =>
                            Expression.Invoke(
                                Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, IAsyncReactiveQbserver<int>, Task<IAsyncReactiveQubscription>>), Constants.SubscribeUri),
                                Expression.Invoke(
                                    Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, Expression<Func<int, IAsyncReactiveQbservable<string>>>, Expression<Func<int, string, int>>, IAsyncReactiveQbservable<int>>), Constants.Observable.SelectMany),
                                    Expression.Invoke(
                                        Expression.Parameter(typeof(Func<string, string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS),
                                        Expression.Constant("observable_parameter_1"),
                                        Expression.Constant("observable_parameter_2"),
                                        Expression.Constant("observable_parameter_3")
                                    ),
                                    Expression.Lambda(
                                        Expression.Invoke(
                                        Expression.Parameter(typeof(Func<int, int, int, IAsyncReactiveQbservable<string>>), Constants.Observable.YS),
                                            x, x, x
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
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_Parameterized_SelectMany1_Simple_4P()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<string, string, string, string, int>(new Uri(Constants.Observable.XS));
                    var ys = ctx.GetObservable<int, int, int, int, string>(new Uri(Constants.Observable.YS));

                    var zs = from x in xs("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4")
                             from y in ys(x, x, x, x)
                             select x + y.Length;

                    var ob = ctx.GetObserver<int>(new Uri(Constants.Observer.OB));
                    zs.SubscribeAsync(ob, new Uri(Constants.Subscription.SUB), state: null).Wait();
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Parameter(typeof(int), "x").Let(x =>
                        Expression.Parameter(typeof(string), "y").Let(y =>
                            Expression.Invoke(
                                Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, IAsyncReactiveQbserver<int>, Task<IAsyncReactiveQubscription>>), Constants.SubscribeUri),
                                Expression.Invoke(
                                    Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, Expression<Func<int, IAsyncReactiveQbservable<string>>>, Expression<Func<int, string, int>>, IAsyncReactiveQbservable<int>>), Constants.Observable.SelectMany),
                                    Expression.Invoke(
                                        Expression.Parameter(typeof(Func<string, string, string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS),
                                        Expression.Constant("observable_parameter_1"),
                                        Expression.Constant("observable_parameter_2"),
                                        Expression.Constant("observable_parameter_3"),
                                        Expression.Constant("observable_parameter_4")
                                    ),
                                    Expression.Lambda(
                                        Expression.Invoke(
                                        Expression.Parameter(typeof(Func<int, int, int, int, IAsyncReactiveQbservable<string>>), Constants.Observable.YS),
                                            x, x, x, x
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
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_Parameterized_SelectMany1_Simple_5P()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<string, string, string, string, string, int>(new Uri(Constants.Observable.XS));
                    var ys = ctx.GetObservable<int, int, int, int, int, string>(new Uri(Constants.Observable.YS));

                    var zs = from x in xs("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5")
                             from y in ys(x, x, x, x, x)
                             select x + y.Length;

                    var ob = ctx.GetObserver<int>(new Uri(Constants.Observer.OB));
                    zs.SubscribeAsync(ob, new Uri(Constants.Subscription.SUB), state: null).Wait();
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Parameter(typeof(int), "x").Let(x =>
                        Expression.Parameter(typeof(string), "y").Let(y =>
                            Expression.Invoke(
                                Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, IAsyncReactiveQbserver<int>, Task<IAsyncReactiveQubscription>>), Constants.SubscribeUri),
                                Expression.Invoke(
                                    Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, Expression<Func<int, IAsyncReactiveQbservable<string>>>, Expression<Func<int, string, int>>, IAsyncReactiveQbservable<int>>), Constants.Observable.SelectMany),
                                    Expression.Invoke(
                                        Expression.Parameter(typeof(Func<string, string, string, string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS),
                                        Expression.Constant("observable_parameter_1"),
                                        Expression.Constant("observable_parameter_2"),
                                        Expression.Constant("observable_parameter_3"),
                                        Expression.Constant("observable_parameter_4"),
                                        Expression.Constant("observable_parameter_5")
                                    ),
                                    Expression.Lambda(
                                        Expression.Invoke(
                                        Expression.Parameter(typeof(Func<int, int, int, int, int, IAsyncReactiveQbservable<string>>), Constants.Observable.YS),
                                            x, x, x, x, x
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
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_Parameterized_SelectMany1_Simple_6P()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<string, string, string, string, string, string, int>(new Uri(Constants.Observable.XS));
                    var ys = ctx.GetObservable<int, int, int, int, int, int, string>(new Uri(Constants.Observable.YS));

                    var zs = from x in xs("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6")
                             from y in ys(x, x, x, x, x, x)
                             select x + y.Length;

                    var ob = ctx.GetObserver<int>(new Uri(Constants.Observer.OB));
                    zs.SubscribeAsync(ob, new Uri(Constants.Subscription.SUB), state: null).Wait();
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Parameter(typeof(int), "x").Let(x =>
                        Expression.Parameter(typeof(string), "y").Let(y =>
                            Expression.Invoke(
                                Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, IAsyncReactiveQbserver<int>, Task<IAsyncReactiveQubscription>>), Constants.SubscribeUri),
                                Expression.Invoke(
                                    Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, Expression<Func<int, IAsyncReactiveQbservable<string>>>, Expression<Func<int, string, int>>, IAsyncReactiveQbservable<int>>), Constants.Observable.SelectMany),
                                    Expression.Invoke(
                                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS),
                                        Expression.Constant("observable_parameter_1"),
                                        Expression.Constant("observable_parameter_2"),
                                        Expression.Constant("observable_parameter_3"),
                                        Expression.Constant("observable_parameter_4"),
                                        Expression.Constant("observable_parameter_5"),
                                        Expression.Constant("observable_parameter_6")
                                    ),
                                    Expression.Lambda(
                                        Expression.Invoke(
                                        Expression.Parameter(typeof(Func<int, int, int, int, int, int, IAsyncReactiveQbservable<string>>), Constants.Observable.YS),
                                            x, x, x, x, x, x
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
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_Parameterized_SelectMany1_Simple_7P()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<string, string, string, string, string, string, string, int>(new Uri(Constants.Observable.XS));
                    var ys = ctx.GetObservable<int, int, int, int, int, int, int, string>(new Uri(Constants.Observable.YS));

                    var zs = from x in xs("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7")
                             from y in ys(x, x, x, x, x, x, x)
                             select x + y.Length;

                    var ob = ctx.GetObserver<int>(new Uri(Constants.Observer.OB));
                    zs.SubscribeAsync(ob, new Uri(Constants.Subscription.SUB), state: null).Wait();
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Parameter(typeof(int), "x").Let(x =>
                        Expression.Parameter(typeof(string), "y").Let(y =>
                            Expression.Invoke(
                                Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, IAsyncReactiveQbserver<int>, Task<IAsyncReactiveQubscription>>), Constants.SubscribeUri),
                                Expression.Invoke(
                                    Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, Expression<Func<int, IAsyncReactiveQbservable<string>>>, Expression<Func<int, string, int>>, IAsyncReactiveQbservable<int>>), Constants.Observable.SelectMany),
                                    Expression.Invoke(
                                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS),
                                        Expression.Constant("observable_parameter_1"),
                                        Expression.Constant("observable_parameter_2"),
                                        Expression.Constant("observable_parameter_3"),
                                        Expression.Constant("observable_parameter_4"),
                                        Expression.Constant("observable_parameter_5"),
                                        Expression.Constant("observable_parameter_6"),
                                        Expression.Constant("observable_parameter_7")
                                    ),
                                    Expression.Lambda(
                                        Expression.Invoke(
                                        Expression.Parameter(typeof(Func<int, int, int, int, int, int, int, IAsyncReactiveQbservable<string>>), Constants.Observable.YS),
                                            x, x, x, x, x, x, x
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
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_Parameterized_SelectMany1_Simple_8P()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observable.XS));
                    var ys = ctx.GetObservable<int, int, int, int, int, int, int, int, string>(new Uri(Constants.Observable.YS));

                    var zs = from x in xs("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7", "observable_parameter_8")
                             from y in ys(x, x, x, x, x, x, x, x)
                             select x + y.Length;

                    var ob = ctx.GetObserver<int>(new Uri(Constants.Observer.OB));
                    zs.SubscribeAsync(ob, new Uri(Constants.Subscription.SUB), state: null).Wait();
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Parameter(typeof(int), "x").Let(x =>
                        Expression.Parameter(typeof(string), "y").Let(y =>
                            Expression.Invoke(
                                Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, IAsyncReactiveQbserver<int>, Task<IAsyncReactiveQubscription>>), Constants.SubscribeUri),
                                Expression.Invoke(
                                    Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, Expression<Func<int, IAsyncReactiveQbservable<string>>>, Expression<Func<int, string, int>>, IAsyncReactiveQbservable<int>>), Constants.Observable.SelectMany),
                                    Expression.Invoke(
                                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS),
                                        Expression.Constant("observable_parameter_1"),
                                        Expression.Constant("observable_parameter_2"),
                                        Expression.Constant("observable_parameter_3"),
                                        Expression.Constant("observable_parameter_4"),
                                        Expression.Constant("observable_parameter_5"),
                                        Expression.Constant("observable_parameter_6"),
                                        Expression.Constant("observable_parameter_7"),
                                        Expression.Constant("observable_parameter_8")
                                    ),
                                    Expression.Lambda(
                                        Expression.Invoke(
                                        Expression.Parameter(typeof(Func<int, int, int, int, int, int, int, int, IAsyncReactiveQbservable<string>>), Constants.Observable.YS),
                                            x, x, x, x, x, x, x, x
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
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_Parameterized_SelectMany1_Simple_9P()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observable.XS));
                    var ys = ctx.GetObservable<int, int, int, int, int, int, int, int, int, string>(new Uri(Constants.Observable.YS));

                    var zs = from x in xs("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7", "observable_parameter_8", "observable_parameter_9")
                             from y in ys(x, x, x, x, x, x, x, x, x)
                             select x + y.Length;

                    var ob = ctx.GetObserver<int>(new Uri(Constants.Observer.OB));
                    zs.SubscribeAsync(ob, new Uri(Constants.Subscription.SUB), state: null).Wait();
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Parameter(typeof(int), "x").Let(x =>
                        Expression.Parameter(typeof(string), "y").Let(y =>
                            Expression.Invoke(
                                Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, IAsyncReactiveQbserver<int>, Task<IAsyncReactiveQubscription>>), Constants.SubscribeUri),
                                Expression.Invoke(
                                    Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, Expression<Func<int, IAsyncReactiveQbservable<string>>>, Expression<Func<int, string, int>>, IAsyncReactiveQbservable<int>>), Constants.Observable.SelectMany),
                                    Expression.Invoke(
                                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS),
                                        Expression.Constant("observable_parameter_1"),
                                        Expression.Constant("observable_parameter_2"),
                                        Expression.Constant("observable_parameter_3"),
                                        Expression.Constant("observable_parameter_4"),
                                        Expression.Constant("observable_parameter_5"),
                                        Expression.Constant("observable_parameter_6"),
                                        Expression.Constant("observable_parameter_7"),
                                        Expression.Constant("observable_parameter_8"),
                                        Expression.Constant("observable_parameter_9")
                                    ),
                                    Expression.Lambda(
                                        Expression.Invoke(
                                        Expression.Parameter(typeof(Func<int, int, int, int, int, int, int, int, int, IAsyncReactiveQbservable<string>>), Constants.Observable.YS),
                                            x, x, x, x, x, x, x, x, x
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
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_Parameterized_SelectMany1_Simple_10P()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observable.XS));
                    var ys = ctx.GetObservable<int, int, int, int, int, int, int, int, int, int, string>(new Uri(Constants.Observable.YS));

                    var zs = from x in xs("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7", "observable_parameter_8", "observable_parameter_9", "observable_parameter_10")
                             from y in ys(x, x, x, x, x, x, x, x, x, x)
                             select x + y.Length;

                    var ob = ctx.GetObserver<int>(new Uri(Constants.Observer.OB));
                    zs.SubscribeAsync(ob, new Uri(Constants.Subscription.SUB), state: null).Wait();
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Parameter(typeof(int), "x").Let(x =>
                        Expression.Parameter(typeof(string), "y").Let(y =>
                            Expression.Invoke(
                                Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, IAsyncReactiveQbserver<int>, Task<IAsyncReactiveQubscription>>), Constants.SubscribeUri),
                                Expression.Invoke(
                                    Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, Expression<Func<int, IAsyncReactiveQbservable<string>>>, Expression<Func<int, string, int>>, IAsyncReactiveQbservable<int>>), Constants.Observable.SelectMany),
                                    Expression.Invoke(
                                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS),
                                        Expression.Constant("observable_parameter_1"),
                                        Expression.Constant("observable_parameter_2"),
                                        Expression.Constant("observable_parameter_3"),
                                        Expression.Constant("observable_parameter_4"),
                                        Expression.Constant("observable_parameter_5"),
                                        Expression.Constant("observable_parameter_6"),
                                        Expression.Constant("observable_parameter_7"),
                                        Expression.Constant("observable_parameter_8"),
                                        Expression.Constant("observable_parameter_9"),
                                        Expression.Constant("observable_parameter_10")
                                    ),
                                    Expression.Lambda(
                                        Expression.Invoke(
                                        Expression.Parameter(typeof(Func<int, int, int, int, int, int, int, int, int, int, IAsyncReactiveQbservable<string>>), Constants.Observable.YS),
                                            x, x, x, x, x, x, x, x, x, x
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
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_Parameterized_SelectMany1_Simple_11P()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<string, string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observable.XS));
                    var ys = ctx.GetObservable<int, int, int, int, int, int, int, int, int, int, int, string>(new Uri(Constants.Observable.YS));

                    var zs = from x in xs("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7", "observable_parameter_8", "observable_parameter_9", "observable_parameter_10", "observable_parameter_11")
                             from y in ys(x, x, x, x, x, x, x, x, x, x, x)
                             select x + y.Length;

                    var ob = ctx.GetObserver<int>(new Uri(Constants.Observer.OB));
                    zs.SubscribeAsync(ob, new Uri(Constants.Subscription.SUB), state: null).Wait();
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Parameter(typeof(int), "x").Let(x =>
                        Expression.Parameter(typeof(string), "y").Let(y =>
                            Expression.Invoke(
                                Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, IAsyncReactiveQbserver<int>, Task<IAsyncReactiveQubscription>>), Constants.SubscribeUri),
                                Expression.Invoke(
                                    Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, Expression<Func<int, IAsyncReactiveQbservable<string>>>, Expression<Func<int, string, int>>, IAsyncReactiveQbservable<int>>), Constants.Observable.SelectMany),
                                    Expression.Invoke(
                                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS),
                                        Expression.Constant("observable_parameter_1"),
                                        Expression.Constant("observable_parameter_2"),
                                        Expression.Constant("observable_parameter_3"),
                                        Expression.Constant("observable_parameter_4"),
                                        Expression.Constant("observable_parameter_5"),
                                        Expression.Constant("observable_parameter_6"),
                                        Expression.Constant("observable_parameter_7"),
                                        Expression.Constant("observable_parameter_8"),
                                        Expression.Constant("observable_parameter_9"),
                                        Expression.Constant("observable_parameter_10"),
                                        Expression.Constant("observable_parameter_11")
                                    ),
                                    Expression.Lambda(
                                        Expression.Invoke(
                                        Expression.Parameter(typeof(Func<int, int, int, int, int, int, int, int, int, int, int, IAsyncReactiveQbservable<string>>), Constants.Observable.YS),
                                            x, x, x, x, x, x, x, x, x, x, x
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
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_Parameterized_SelectMany1_Simple_12P()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<string, string, string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observable.XS));
                    var ys = ctx.GetObservable<int, int, int, int, int, int, int, int, int, int, int, int, string>(new Uri(Constants.Observable.YS));

                    var zs = from x in xs("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7", "observable_parameter_8", "observable_parameter_9", "observable_parameter_10", "observable_parameter_11", "observable_parameter_12")
                             from y in ys(x, x, x, x, x, x, x, x, x, x, x, x)
                             select x + y.Length;

                    var ob = ctx.GetObserver<int>(new Uri(Constants.Observer.OB));
                    zs.SubscribeAsync(ob, new Uri(Constants.Subscription.SUB), state: null).Wait();
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Parameter(typeof(int), "x").Let(x =>
                        Expression.Parameter(typeof(string), "y").Let(y =>
                            Expression.Invoke(
                                Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, IAsyncReactiveQbserver<int>, Task<IAsyncReactiveQubscription>>), Constants.SubscribeUri),
                                Expression.Invoke(
                                    Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, Expression<Func<int, IAsyncReactiveQbservable<string>>>, Expression<Func<int, string, int>>, IAsyncReactiveQbservable<int>>), Constants.Observable.SelectMany),
                                    Expression.Invoke(
                                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS),
                                        Expression.Constant("observable_parameter_1"),
                                        Expression.Constant("observable_parameter_2"),
                                        Expression.Constant("observable_parameter_3"),
                                        Expression.Constant("observable_parameter_4"),
                                        Expression.Constant("observable_parameter_5"),
                                        Expression.Constant("observable_parameter_6"),
                                        Expression.Constant("observable_parameter_7"),
                                        Expression.Constant("observable_parameter_8"),
                                        Expression.Constant("observable_parameter_9"),
                                        Expression.Constant("observable_parameter_10"),
                                        Expression.Constant("observable_parameter_11"),
                                        Expression.Constant("observable_parameter_12")
                                    ),
                                    Expression.Lambda(
                                        Expression.Invoke(
                                        Expression.Parameter(typeof(Func<int, int, int, int, int, int, int, int, int, int, int, int, IAsyncReactiveQbservable<string>>), Constants.Observable.YS),
                                            x, x, x, x, x, x, x, x, x, x, x, x
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
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_Parameterized_SelectMany1_Simple_13P()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<string, string, string, string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observable.XS));
                    var ys = ctx.GetObservable<int, int, int, int, int, int, int, int, int, int, int, int, int, string>(new Uri(Constants.Observable.YS));

                    var zs = from x in xs("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7", "observable_parameter_8", "observable_parameter_9", "observable_parameter_10", "observable_parameter_11", "observable_parameter_12", "observable_parameter_13")
                             from y in ys(x, x, x, x, x, x, x, x, x, x, x, x, x)
                             select x + y.Length;

                    var ob = ctx.GetObserver<int>(new Uri(Constants.Observer.OB));
                    zs.SubscribeAsync(ob, new Uri(Constants.Subscription.SUB), state: null).Wait();
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Parameter(typeof(int), "x").Let(x =>
                        Expression.Parameter(typeof(string), "y").Let(y =>
                            Expression.Invoke(
                                Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, IAsyncReactiveQbserver<int>, Task<IAsyncReactiveQubscription>>), Constants.SubscribeUri),
                                Expression.Invoke(
                                    Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, Expression<Func<int, IAsyncReactiveQbservable<string>>>, Expression<Func<int, string, int>>, IAsyncReactiveQbservable<int>>), Constants.Observable.SelectMany),
                                    Expression.Invoke(
                                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS),
                                        Expression.Constant("observable_parameter_1"),
                                        Expression.Constant("observable_parameter_2"),
                                        Expression.Constant("observable_parameter_3"),
                                        Expression.Constant("observable_parameter_4"),
                                        Expression.Constant("observable_parameter_5"),
                                        Expression.Constant("observable_parameter_6"),
                                        Expression.Constant("observable_parameter_7"),
                                        Expression.Constant("observable_parameter_8"),
                                        Expression.Constant("observable_parameter_9"),
                                        Expression.Constant("observable_parameter_10"),
                                        Expression.Constant("observable_parameter_11"),
                                        Expression.Constant("observable_parameter_12"),
                                        Expression.Constant("observable_parameter_13")
                                    ),
                                    Expression.Lambda(
                                        Expression.Invoke(
                                        Expression.Parameter(typeof(Func<int, int, int, int, int, int, int, int, int, int, int, int, int, IAsyncReactiveQbservable<string>>), Constants.Observable.YS),
                                            x, x, x, x, x, x, x, x, x, x, x, x, x
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
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_Parameterized_SelectMany1_Simple_14P()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<string, string, string, string, string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observable.XS));
                    var ys = ctx.GetObservable<int, int, int, int, int, int, int, int, int, int, int, int, int, int, string>(new Uri(Constants.Observable.YS));

                    var zs = from x in xs("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7", "observable_parameter_8", "observable_parameter_9", "observable_parameter_10", "observable_parameter_11", "observable_parameter_12", "observable_parameter_13", "observable_parameter_14")
                             from y in ys(x, x, x, x, x, x, x, x, x, x, x, x, x, x)
                             select x + y.Length;

                    var ob = ctx.GetObserver<int>(new Uri(Constants.Observer.OB));
                    zs.SubscribeAsync(ob, new Uri(Constants.Subscription.SUB), state: null).Wait();
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Parameter(typeof(int), "x").Let(x =>
                        Expression.Parameter(typeof(string), "y").Let(y =>
                            Expression.Invoke(
                                Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, IAsyncReactiveQbserver<int>, Task<IAsyncReactiveQubscription>>), Constants.SubscribeUri),
                                Expression.Invoke(
                                    Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, Expression<Func<int, IAsyncReactiveQbservable<string>>>, Expression<Func<int, string, int>>, IAsyncReactiveQbservable<int>>), Constants.Observable.SelectMany),
                                    Expression.Invoke(
                                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS),
                                        Expression.Constant("observable_parameter_1"),
                                        Expression.Constant("observable_parameter_2"),
                                        Expression.Constant("observable_parameter_3"),
                                        Expression.Constant("observable_parameter_4"),
                                        Expression.Constant("observable_parameter_5"),
                                        Expression.Constant("observable_parameter_6"),
                                        Expression.Constant("observable_parameter_7"),
                                        Expression.Constant("observable_parameter_8"),
                                        Expression.Constant("observable_parameter_9"),
                                        Expression.Constant("observable_parameter_10"),
                                        Expression.Constant("observable_parameter_11"),
                                        Expression.Constant("observable_parameter_12"),
                                        Expression.Constant("observable_parameter_13"),
                                        Expression.Constant("observable_parameter_14")
                                    ),
                                    Expression.Lambda(
                                        Expression.Invoke(
                                        Expression.Parameter(typeof(Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, IAsyncReactiveQbservable<string>>), Constants.Observable.YS),
                                            x, x, x, x, x, x, x, x, x, x, x, x, x, x
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
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_Parameterized_SelectMany1_Simple_15P()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observable.XS));
                    var ys = ctx.GetObservable<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, string>(new Uri(Constants.Observable.YS));

                    var zs = from x in xs("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7", "observable_parameter_8", "observable_parameter_9", "observable_parameter_10", "observable_parameter_11", "observable_parameter_12", "observable_parameter_13", "observable_parameter_14", "observable_parameter_15")
                             from y in ys(x, x, x, x, x, x, x, x, x, x, x, x, x, x, x)
                             select x + y.Length;

                    var ob = ctx.GetObserver<int>(new Uri(Constants.Observer.OB));
                    zs.SubscribeAsync(ob, new Uri(Constants.Subscription.SUB), state: null).Wait();
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Parameter(typeof(int), "x").Let(x =>
                        Expression.Parameter(typeof(string), "y").Let(y =>
                            Expression.Invoke(
                                Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, IAsyncReactiveQbserver<int>, Task<IAsyncReactiveQubscription>>), Constants.SubscribeUri),
                                Expression.Invoke(
                                    Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, Expression<Func<int, IAsyncReactiveQbservable<string>>>, Expression<Func<int, string, int>>, IAsyncReactiveQbservable<int>>), Constants.Observable.SelectMany),
                                    Expression.Invoke(
                                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS),
                                        Expression.Constant("observable_parameter_1"),
                                        Expression.Constant("observable_parameter_2"),
                                        Expression.Constant("observable_parameter_3"),
                                        Expression.Constant("observable_parameter_4"),
                                        Expression.Constant("observable_parameter_5"),
                                        Expression.Constant("observable_parameter_6"),
                                        Expression.Constant("observable_parameter_7"),
                                        Expression.Constant("observable_parameter_8"),
                                        Expression.Constant("observable_parameter_9"),
                                        Expression.Constant("observable_parameter_10"),
                                        Expression.Constant("observable_parameter_11"),
                                        Expression.Constant("observable_parameter_12"),
                                        Expression.Constant("observable_parameter_13"),
                                        Expression.Constant("observable_parameter_14"),
                                        Expression.Constant("observable_parameter_15")
                                    ),
                                    Expression.Lambda(
                                        Expression.Invoke(
                                        Expression.Parameter(typeof(Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, IAsyncReactiveQbservable<string>>), Constants.Observable.YS),
                                            x, x, x, x, x, x, x, x, x, x, x, x, x, x, x
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
                    state: null
                )
            );
        }


        #endregion

        #region "ReactiveClientContext_SubscribeAsync_Parameterized_SelectMany2_Closure"

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_Parameterized_SelectMany2_Closure_2P()
        {
            var xs = default(IAsyncReactiveQbservable<int>);
            var ys = default(Func<int, int, IAsyncReactiveQbservable<string>>);

            Apply(
                ctx =>
                {
                    xs = ctx.GetObservable<string, string, int>(new Uri(Constants.Observable.XS))("observable_parameter_1", "observable_parameter_2");
                    ys = ctx.GetObservable<int, int, string>(new Uri(Constants.Observable.YS));

                    var zs = from x in xs
                             from y in ys(x, x)
                             select x + y.Length;

                    var ob = ctx.GetObserver<int>(new Uri(Constants.Observer.OB));
                    zs.SubscribeAsync(ob, new Uri(Constants.Subscription.SUB), state: null).Wait();
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Parameter(typeof(int), "x").Let(x =>
                        Expression.Parameter(typeof(string), "y").Let(y =>
                            Expression.Invoke(
                                Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, IAsyncReactiveQbserver<int>, Task<IAsyncReactiveQubscription>>), Constants.SubscribeUri),
                                Expression.Invoke(
                                    Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, Expression<Func<int, IAsyncReactiveQbservable<string>>>, Expression<Func<int, string, int>>, IAsyncReactiveQbservable<int>>), Constants.Observable.SelectMany),
                                    Expression.Invoke(
                                        Expression.Parameter(typeof(Func<string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS),
                                        Expression.Constant("observable_parameter_1"),
                                        Expression.Constant("observable_parameter_2")
                                    ),
                                    Expression.Lambda(
                                        Expression.Invoke(
                                            Expression.Parameter(typeof(Func<int, int, IAsyncReactiveQbservable<string>>), Constants.Observable.YS),
                                            x, x
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
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_Parameterized_SelectMany2_Closure_3P()
        {
            var xs = default(IAsyncReactiveQbservable<int>);
            var ys = default(Func<int, int, int, IAsyncReactiveQbservable<string>>);

            Apply(
                ctx =>
                {
                    xs = ctx.GetObservable<string, string, string, int>(new Uri(Constants.Observable.XS))("observable_parameter_1", "observable_parameter_2", "observable_parameter_3");
                    ys = ctx.GetObservable<int, int, int, string>(new Uri(Constants.Observable.YS));

                    var zs = from x in xs
                             from y in ys(x, x, x)
                             select x + y.Length;

                    var ob = ctx.GetObserver<int>(new Uri(Constants.Observer.OB));
                    zs.SubscribeAsync(ob, new Uri(Constants.Subscription.SUB), state: null).Wait();
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Parameter(typeof(int), "x").Let(x =>
                        Expression.Parameter(typeof(string), "y").Let(y =>
                            Expression.Invoke(
                                Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, IAsyncReactiveQbserver<int>, Task<IAsyncReactiveQubscription>>), Constants.SubscribeUri),
                                Expression.Invoke(
                                    Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, Expression<Func<int, IAsyncReactiveQbservable<string>>>, Expression<Func<int, string, int>>, IAsyncReactiveQbservable<int>>), Constants.Observable.SelectMany),
                                    Expression.Invoke(
                                        Expression.Parameter(typeof(Func<string, string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS),
                                        Expression.Constant("observable_parameter_1"),
                                        Expression.Constant("observable_parameter_2"),
                                        Expression.Constant("observable_parameter_3")
                                    ),
                                    Expression.Lambda(
                                        Expression.Invoke(
                                            Expression.Parameter(typeof(Func<int, int, int, IAsyncReactiveQbservable<string>>), Constants.Observable.YS),
                                            x, x, x
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
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_Parameterized_SelectMany2_Closure_4P()
        {
            var xs = default(IAsyncReactiveQbservable<int>);
            var ys = default(Func<int, int, int, int, IAsyncReactiveQbservable<string>>);

            Apply(
                ctx =>
                {
                    xs = ctx.GetObservable<string, string, string, string, int>(new Uri(Constants.Observable.XS))("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4");
                    ys = ctx.GetObservable<int, int, int, int, string>(new Uri(Constants.Observable.YS));

                    var zs = from x in xs
                             from y in ys(x, x, x, x)
                             select x + y.Length;

                    var ob = ctx.GetObserver<int>(new Uri(Constants.Observer.OB));
                    zs.SubscribeAsync(ob, new Uri(Constants.Subscription.SUB), state: null).Wait();
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Parameter(typeof(int), "x").Let(x =>
                        Expression.Parameter(typeof(string), "y").Let(y =>
                            Expression.Invoke(
                                Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, IAsyncReactiveQbserver<int>, Task<IAsyncReactiveQubscription>>), Constants.SubscribeUri),
                                Expression.Invoke(
                                    Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, Expression<Func<int, IAsyncReactiveQbservable<string>>>, Expression<Func<int, string, int>>, IAsyncReactiveQbservable<int>>), Constants.Observable.SelectMany),
                                    Expression.Invoke(
                                        Expression.Parameter(typeof(Func<string, string, string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS),
                                        Expression.Constant("observable_parameter_1"),
                                        Expression.Constant("observable_parameter_2"),
                                        Expression.Constant("observable_parameter_3"),
                                        Expression.Constant("observable_parameter_4")
                                    ),
                                    Expression.Lambda(
                                        Expression.Invoke(
                                            Expression.Parameter(typeof(Func<int, int, int, int, IAsyncReactiveQbservable<string>>), Constants.Observable.YS),
                                            x, x, x, x
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
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_Parameterized_SelectMany2_Closure_5P()
        {
            var xs = default(IAsyncReactiveQbservable<int>);
            var ys = default(Func<int, int, int, int, int, IAsyncReactiveQbservable<string>>);

            Apply(
                ctx =>
                {
                    xs = ctx.GetObservable<string, string, string, string, string, int>(new Uri(Constants.Observable.XS))("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5");
                    ys = ctx.GetObservable<int, int, int, int, int, string>(new Uri(Constants.Observable.YS));

                    var zs = from x in xs
                             from y in ys(x, x, x, x, x)
                             select x + y.Length;

                    var ob = ctx.GetObserver<int>(new Uri(Constants.Observer.OB));
                    zs.SubscribeAsync(ob, new Uri(Constants.Subscription.SUB), state: null).Wait();
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Parameter(typeof(int), "x").Let(x =>
                        Expression.Parameter(typeof(string), "y").Let(y =>
                            Expression.Invoke(
                                Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, IAsyncReactiveQbserver<int>, Task<IAsyncReactiveQubscription>>), Constants.SubscribeUri),
                                Expression.Invoke(
                                    Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, Expression<Func<int, IAsyncReactiveQbservable<string>>>, Expression<Func<int, string, int>>, IAsyncReactiveQbservable<int>>), Constants.Observable.SelectMany),
                                    Expression.Invoke(
                                        Expression.Parameter(typeof(Func<string, string, string, string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS),
                                        Expression.Constant("observable_parameter_1"),
                                        Expression.Constant("observable_parameter_2"),
                                        Expression.Constant("observable_parameter_3"),
                                        Expression.Constant("observable_parameter_4"),
                                        Expression.Constant("observable_parameter_5")
                                    ),
                                    Expression.Lambda(
                                        Expression.Invoke(
                                            Expression.Parameter(typeof(Func<int, int, int, int, int, IAsyncReactiveQbservable<string>>), Constants.Observable.YS),
                                            x, x, x, x, x
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
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_Parameterized_SelectMany2_Closure_6P()
        {
            var xs = default(IAsyncReactiveQbservable<int>);
            var ys = default(Func<int, int, int, int, int, int, IAsyncReactiveQbservable<string>>);

            Apply(
                ctx =>
                {
                    xs = ctx.GetObservable<string, string, string, string, string, string, int>(new Uri(Constants.Observable.XS))("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6");
                    ys = ctx.GetObservable<int, int, int, int, int, int, string>(new Uri(Constants.Observable.YS));

                    var zs = from x in xs
                             from y in ys(x, x, x, x, x, x)
                             select x + y.Length;

                    var ob = ctx.GetObserver<int>(new Uri(Constants.Observer.OB));
                    zs.SubscribeAsync(ob, new Uri(Constants.Subscription.SUB), state: null).Wait();
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Parameter(typeof(int), "x").Let(x =>
                        Expression.Parameter(typeof(string), "y").Let(y =>
                            Expression.Invoke(
                                Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, IAsyncReactiveQbserver<int>, Task<IAsyncReactiveQubscription>>), Constants.SubscribeUri),
                                Expression.Invoke(
                                    Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, Expression<Func<int, IAsyncReactiveQbservable<string>>>, Expression<Func<int, string, int>>, IAsyncReactiveQbservable<int>>), Constants.Observable.SelectMany),
                                    Expression.Invoke(
                                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS),
                                        Expression.Constant("observable_parameter_1"),
                                        Expression.Constant("observable_parameter_2"),
                                        Expression.Constant("observable_parameter_3"),
                                        Expression.Constant("observable_parameter_4"),
                                        Expression.Constant("observable_parameter_5"),
                                        Expression.Constant("observable_parameter_6")
                                    ),
                                    Expression.Lambda(
                                        Expression.Invoke(
                                            Expression.Parameter(typeof(Func<int, int, int, int, int, int, IAsyncReactiveQbservable<string>>), Constants.Observable.YS),
                                            x, x, x, x, x, x
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
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_Parameterized_SelectMany2_Closure_7P()
        {
            var xs = default(IAsyncReactiveQbservable<int>);
            var ys = default(Func<int, int, int, int, int, int, int, IAsyncReactiveQbservable<string>>);

            Apply(
                ctx =>
                {
                    xs = ctx.GetObservable<string, string, string, string, string, string, string, int>(new Uri(Constants.Observable.XS))("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7");
                    ys = ctx.GetObservable<int, int, int, int, int, int, int, string>(new Uri(Constants.Observable.YS));

                    var zs = from x in xs
                             from y in ys(x, x, x, x, x, x, x)
                             select x + y.Length;

                    var ob = ctx.GetObserver<int>(new Uri(Constants.Observer.OB));
                    zs.SubscribeAsync(ob, new Uri(Constants.Subscription.SUB), state: null).Wait();
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Parameter(typeof(int), "x").Let(x =>
                        Expression.Parameter(typeof(string), "y").Let(y =>
                            Expression.Invoke(
                                Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, IAsyncReactiveQbserver<int>, Task<IAsyncReactiveQubscription>>), Constants.SubscribeUri),
                                Expression.Invoke(
                                    Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, Expression<Func<int, IAsyncReactiveQbservable<string>>>, Expression<Func<int, string, int>>, IAsyncReactiveQbservable<int>>), Constants.Observable.SelectMany),
                                    Expression.Invoke(
                                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS),
                                        Expression.Constant("observable_parameter_1"),
                                        Expression.Constant("observable_parameter_2"),
                                        Expression.Constant("observable_parameter_3"),
                                        Expression.Constant("observable_parameter_4"),
                                        Expression.Constant("observable_parameter_5"),
                                        Expression.Constant("observable_parameter_6"),
                                        Expression.Constant("observable_parameter_7")
                                    ),
                                    Expression.Lambda(
                                        Expression.Invoke(
                                            Expression.Parameter(typeof(Func<int, int, int, int, int, int, int, IAsyncReactiveQbservable<string>>), Constants.Observable.YS),
                                            x, x, x, x, x, x, x
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
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_Parameterized_SelectMany2_Closure_8P()
        {
            var xs = default(IAsyncReactiveQbservable<int>);
            var ys = default(Func<int, int, int, int, int, int, int, int, IAsyncReactiveQbservable<string>>);

            Apply(
                ctx =>
                {
                    xs = ctx.GetObservable<string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observable.XS))("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7", "observable_parameter_8");
                    ys = ctx.GetObservable<int, int, int, int, int, int, int, int, string>(new Uri(Constants.Observable.YS));

                    var zs = from x in xs
                             from y in ys(x, x, x, x, x, x, x, x)
                             select x + y.Length;

                    var ob = ctx.GetObserver<int>(new Uri(Constants.Observer.OB));
                    zs.SubscribeAsync(ob, new Uri(Constants.Subscription.SUB), state: null).Wait();
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Parameter(typeof(int), "x").Let(x =>
                        Expression.Parameter(typeof(string), "y").Let(y =>
                            Expression.Invoke(
                                Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, IAsyncReactiveQbserver<int>, Task<IAsyncReactiveQubscription>>), Constants.SubscribeUri),
                                Expression.Invoke(
                                    Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, Expression<Func<int, IAsyncReactiveQbservable<string>>>, Expression<Func<int, string, int>>, IAsyncReactiveQbservable<int>>), Constants.Observable.SelectMany),
                                    Expression.Invoke(
                                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS),
                                        Expression.Constant("observable_parameter_1"),
                                        Expression.Constant("observable_parameter_2"),
                                        Expression.Constant("observable_parameter_3"),
                                        Expression.Constant("observable_parameter_4"),
                                        Expression.Constant("observable_parameter_5"),
                                        Expression.Constant("observable_parameter_6"),
                                        Expression.Constant("observable_parameter_7"),
                                        Expression.Constant("observable_parameter_8")
                                    ),
                                    Expression.Lambda(
                                        Expression.Invoke(
                                            Expression.Parameter(typeof(Func<int, int, int, int, int, int, int, int, IAsyncReactiveQbservable<string>>), Constants.Observable.YS),
                                            x, x, x, x, x, x, x, x
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
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_Parameterized_SelectMany2_Closure_9P()
        {
            var xs = default(IAsyncReactiveQbservable<int>);
            var ys = default(Func<int, int, int, int, int, int, int, int, int, IAsyncReactiveQbservable<string>>);

            Apply(
                ctx =>
                {
                    xs = ctx.GetObservable<string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observable.XS))("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7", "observable_parameter_8", "observable_parameter_9");
                    ys = ctx.GetObservable<int, int, int, int, int, int, int, int, int, string>(new Uri(Constants.Observable.YS));

                    var zs = from x in xs
                             from y in ys(x, x, x, x, x, x, x, x, x)
                             select x + y.Length;

                    var ob = ctx.GetObserver<int>(new Uri(Constants.Observer.OB));
                    zs.SubscribeAsync(ob, new Uri(Constants.Subscription.SUB), state: null).Wait();
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Parameter(typeof(int), "x").Let(x =>
                        Expression.Parameter(typeof(string), "y").Let(y =>
                            Expression.Invoke(
                                Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, IAsyncReactiveQbserver<int>, Task<IAsyncReactiveQubscription>>), Constants.SubscribeUri),
                                Expression.Invoke(
                                    Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, Expression<Func<int, IAsyncReactiveQbservable<string>>>, Expression<Func<int, string, int>>, IAsyncReactiveQbservable<int>>), Constants.Observable.SelectMany),
                                    Expression.Invoke(
                                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS),
                                        Expression.Constant("observable_parameter_1"),
                                        Expression.Constant("observable_parameter_2"),
                                        Expression.Constant("observable_parameter_3"),
                                        Expression.Constant("observable_parameter_4"),
                                        Expression.Constant("observable_parameter_5"),
                                        Expression.Constant("observable_parameter_6"),
                                        Expression.Constant("observable_parameter_7"),
                                        Expression.Constant("observable_parameter_8"),
                                        Expression.Constant("observable_parameter_9")
                                    ),
                                    Expression.Lambda(
                                        Expression.Invoke(
                                            Expression.Parameter(typeof(Func<int, int, int, int, int, int, int, int, int, IAsyncReactiveQbservable<string>>), Constants.Observable.YS),
                                            x, x, x, x, x, x, x, x, x
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
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_Parameterized_SelectMany2_Closure_10P()
        {
            var xs = default(IAsyncReactiveQbservable<int>);
            var ys = default(Func<int, int, int, int, int, int, int, int, int, int, IAsyncReactiveQbservable<string>>);

            Apply(
                ctx =>
                {
                    xs = ctx.GetObservable<string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observable.XS))("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7", "observable_parameter_8", "observable_parameter_9", "observable_parameter_10");
                    ys = ctx.GetObservable<int, int, int, int, int, int, int, int, int, int, string>(new Uri(Constants.Observable.YS));

                    var zs = from x in xs
                             from y in ys(x, x, x, x, x, x, x, x, x, x)
                             select x + y.Length;

                    var ob = ctx.GetObserver<int>(new Uri(Constants.Observer.OB));
                    zs.SubscribeAsync(ob, new Uri(Constants.Subscription.SUB), state: null).Wait();
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Parameter(typeof(int), "x").Let(x =>
                        Expression.Parameter(typeof(string), "y").Let(y =>
                            Expression.Invoke(
                                Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, IAsyncReactiveQbserver<int>, Task<IAsyncReactiveQubscription>>), Constants.SubscribeUri),
                                Expression.Invoke(
                                    Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, Expression<Func<int, IAsyncReactiveQbservable<string>>>, Expression<Func<int, string, int>>, IAsyncReactiveQbservable<int>>), Constants.Observable.SelectMany),
                                    Expression.Invoke(
                                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS),
                                        Expression.Constant("observable_parameter_1"),
                                        Expression.Constant("observable_parameter_2"),
                                        Expression.Constant("observable_parameter_3"),
                                        Expression.Constant("observable_parameter_4"),
                                        Expression.Constant("observable_parameter_5"),
                                        Expression.Constant("observable_parameter_6"),
                                        Expression.Constant("observable_parameter_7"),
                                        Expression.Constant("observable_parameter_8"),
                                        Expression.Constant("observable_parameter_9"),
                                        Expression.Constant("observable_parameter_10")
                                    ),
                                    Expression.Lambda(
                                        Expression.Invoke(
                                            Expression.Parameter(typeof(Func<int, int, int, int, int, int, int, int, int, int, IAsyncReactiveQbservable<string>>), Constants.Observable.YS),
                                            x, x, x, x, x, x, x, x, x, x
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
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_Parameterized_SelectMany2_Closure_11P()
        {
            var xs = default(IAsyncReactiveQbservable<int>);
            var ys = default(Func<int, int, int, int, int, int, int, int, int, int, int, IAsyncReactiveQbservable<string>>);

            Apply(
                ctx =>
                {
                    xs = ctx.GetObservable<string, string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observable.XS))("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7", "observable_parameter_8", "observable_parameter_9", "observable_parameter_10", "observable_parameter_11");
                    ys = ctx.GetObservable<int, int, int, int, int, int, int, int, int, int, int, string>(new Uri(Constants.Observable.YS));

                    var zs = from x in xs
                             from y in ys(x, x, x, x, x, x, x, x, x, x, x)
                             select x + y.Length;

                    var ob = ctx.GetObserver<int>(new Uri(Constants.Observer.OB));
                    zs.SubscribeAsync(ob, new Uri(Constants.Subscription.SUB), state: null).Wait();
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Parameter(typeof(int), "x").Let(x =>
                        Expression.Parameter(typeof(string), "y").Let(y =>
                            Expression.Invoke(
                                Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, IAsyncReactiveQbserver<int>, Task<IAsyncReactiveQubscription>>), Constants.SubscribeUri),
                                Expression.Invoke(
                                    Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, Expression<Func<int, IAsyncReactiveQbservable<string>>>, Expression<Func<int, string, int>>, IAsyncReactiveQbservable<int>>), Constants.Observable.SelectMany),
                                    Expression.Invoke(
                                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS),
                                        Expression.Constant("observable_parameter_1"),
                                        Expression.Constant("observable_parameter_2"),
                                        Expression.Constant("observable_parameter_3"),
                                        Expression.Constant("observable_parameter_4"),
                                        Expression.Constant("observable_parameter_5"),
                                        Expression.Constant("observable_parameter_6"),
                                        Expression.Constant("observable_parameter_7"),
                                        Expression.Constant("observable_parameter_8"),
                                        Expression.Constant("observable_parameter_9"),
                                        Expression.Constant("observable_parameter_10"),
                                        Expression.Constant("observable_parameter_11")
                                    ),
                                    Expression.Lambda(
                                        Expression.Invoke(
                                            Expression.Parameter(typeof(Func<int, int, int, int, int, int, int, int, int, int, int, IAsyncReactiveQbservable<string>>), Constants.Observable.YS),
                                            x, x, x, x, x, x, x, x, x, x, x
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
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_Parameterized_SelectMany2_Closure_12P()
        {
            var xs = default(IAsyncReactiveQbservable<int>);
            var ys = default(Func<int, int, int, int, int, int, int, int, int, int, int, int, IAsyncReactiveQbservable<string>>);

            Apply(
                ctx =>
                {
                    xs = ctx.GetObservable<string, string, string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observable.XS))("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7", "observable_parameter_8", "observable_parameter_9", "observable_parameter_10", "observable_parameter_11", "observable_parameter_12");
                    ys = ctx.GetObservable<int, int, int, int, int, int, int, int, int, int, int, int, string>(new Uri(Constants.Observable.YS));

                    var zs = from x in xs
                             from y in ys(x, x, x, x, x, x, x, x, x, x, x, x)
                             select x + y.Length;

                    var ob = ctx.GetObserver<int>(new Uri(Constants.Observer.OB));
                    zs.SubscribeAsync(ob, new Uri(Constants.Subscription.SUB), state: null).Wait();
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Parameter(typeof(int), "x").Let(x =>
                        Expression.Parameter(typeof(string), "y").Let(y =>
                            Expression.Invoke(
                                Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, IAsyncReactiveQbserver<int>, Task<IAsyncReactiveQubscription>>), Constants.SubscribeUri),
                                Expression.Invoke(
                                    Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, Expression<Func<int, IAsyncReactiveQbservable<string>>>, Expression<Func<int, string, int>>, IAsyncReactiveQbservable<int>>), Constants.Observable.SelectMany),
                                    Expression.Invoke(
                                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS),
                                        Expression.Constant("observable_parameter_1"),
                                        Expression.Constant("observable_parameter_2"),
                                        Expression.Constant("observable_parameter_3"),
                                        Expression.Constant("observable_parameter_4"),
                                        Expression.Constant("observable_parameter_5"),
                                        Expression.Constant("observable_parameter_6"),
                                        Expression.Constant("observable_parameter_7"),
                                        Expression.Constant("observable_parameter_8"),
                                        Expression.Constant("observable_parameter_9"),
                                        Expression.Constant("observable_parameter_10"),
                                        Expression.Constant("observable_parameter_11"),
                                        Expression.Constant("observable_parameter_12")
                                    ),
                                    Expression.Lambda(
                                        Expression.Invoke(
                                            Expression.Parameter(typeof(Func<int, int, int, int, int, int, int, int, int, int, int, int, IAsyncReactiveQbservable<string>>), Constants.Observable.YS),
                                            x, x, x, x, x, x, x, x, x, x, x, x
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
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_Parameterized_SelectMany2_Closure_13P()
        {
            var xs = default(IAsyncReactiveQbservable<int>);
            var ys = default(Func<int, int, int, int, int, int, int, int, int, int, int, int, int, IAsyncReactiveQbservable<string>>);

            Apply(
                ctx =>
                {
                    xs = ctx.GetObservable<string, string, string, string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observable.XS))("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7", "observable_parameter_8", "observable_parameter_9", "observable_parameter_10", "observable_parameter_11", "observable_parameter_12", "observable_parameter_13");
                    ys = ctx.GetObservable<int, int, int, int, int, int, int, int, int, int, int, int, int, string>(new Uri(Constants.Observable.YS));

                    var zs = from x in xs
                             from y in ys(x, x, x, x, x, x, x, x, x, x, x, x, x)
                             select x + y.Length;

                    var ob = ctx.GetObserver<int>(new Uri(Constants.Observer.OB));
                    zs.SubscribeAsync(ob, new Uri(Constants.Subscription.SUB), state: null).Wait();
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Parameter(typeof(int), "x").Let(x =>
                        Expression.Parameter(typeof(string), "y").Let(y =>
                            Expression.Invoke(
                                Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, IAsyncReactiveQbserver<int>, Task<IAsyncReactiveQubscription>>), Constants.SubscribeUri),
                                Expression.Invoke(
                                    Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, Expression<Func<int, IAsyncReactiveQbservable<string>>>, Expression<Func<int, string, int>>, IAsyncReactiveQbservable<int>>), Constants.Observable.SelectMany),
                                    Expression.Invoke(
                                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS),
                                        Expression.Constant("observable_parameter_1"),
                                        Expression.Constant("observable_parameter_2"),
                                        Expression.Constant("observable_parameter_3"),
                                        Expression.Constant("observable_parameter_4"),
                                        Expression.Constant("observable_parameter_5"),
                                        Expression.Constant("observable_parameter_6"),
                                        Expression.Constant("observable_parameter_7"),
                                        Expression.Constant("observable_parameter_8"),
                                        Expression.Constant("observable_parameter_9"),
                                        Expression.Constant("observable_parameter_10"),
                                        Expression.Constant("observable_parameter_11"),
                                        Expression.Constant("observable_parameter_12"),
                                        Expression.Constant("observable_parameter_13")
                                    ),
                                    Expression.Lambda(
                                        Expression.Invoke(
                                            Expression.Parameter(typeof(Func<int, int, int, int, int, int, int, int, int, int, int, int, int, IAsyncReactiveQbservable<string>>), Constants.Observable.YS),
                                            x, x, x, x, x, x, x, x, x, x, x, x, x
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
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_Parameterized_SelectMany2_Closure_14P()
        {
            var xs = default(IAsyncReactiveQbservable<int>);
            var ys = default(Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, IAsyncReactiveQbservable<string>>);

            Apply(
                ctx =>
                {
                    xs = ctx.GetObservable<string, string, string, string, string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observable.XS))("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7", "observable_parameter_8", "observable_parameter_9", "observable_parameter_10", "observable_parameter_11", "observable_parameter_12", "observable_parameter_13", "observable_parameter_14");
                    ys = ctx.GetObservable<int, int, int, int, int, int, int, int, int, int, int, int, int, int, string>(new Uri(Constants.Observable.YS));

                    var zs = from x in xs
                             from y in ys(x, x, x, x, x, x, x, x, x, x, x, x, x, x)
                             select x + y.Length;

                    var ob = ctx.GetObserver<int>(new Uri(Constants.Observer.OB));
                    zs.SubscribeAsync(ob, new Uri(Constants.Subscription.SUB), state: null).Wait();
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Parameter(typeof(int), "x").Let(x =>
                        Expression.Parameter(typeof(string), "y").Let(y =>
                            Expression.Invoke(
                                Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, IAsyncReactiveQbserver<int>, Task<IAsyncReactiveQubscription>>), Constants.SubscribeUri),
                                Expression.Invoke(
                                    Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, Expression<Func<int, IAsyncReactiveQbservable<string>>>, Expression<Func<int, string, int>>, IAsyncReactiveQbservable<int>>), Constants.Observable.SelectMany),
                                    Expression.Invoke(
                                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS),
                                        Expression.Constant("observable_parameter_1"),
                                        Expression.Constant("observable_parameter_2"),
                                        Expression.Constant("observable_parameter_3"),
                                        Expression.Constant("observable_parameter_4"),
                                        Expression.Constant("observable_parameter_5"),
                                        Expression.Constant("observable_parameter_6"),
                                        Expression.Constant("observable_parameter_7"),
                                        Expression.Constant("observable_parameter_8"),
                                        Expression.Constant("observable_parameter_9"),
                                        Expression.Constant("observable_parameter_10"),
                                        Expression.Constant("observable_parameter_11"),
                                        Expression.Constant("observable_parameter_12"),
                                        Expression.Constant("observable_parameter_13"),
                                        Expression.Constant("observable_parameter_14")
                                    ),
                                    Expression.Lambda(
                                        Expression.Invoke(
                                            Expression.Parameter(typeof(Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, IAsyncReactiveQbservable<string>>), Constants.Observable.YS),
                                            x, x, x, x, x, x, x, x, x, x, x, x, x, x
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
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_Parameterized_SelectMany2_Closure_15P()
        {
            var xs = default(IAsyncReactiveQbservable<int>);
            var ys = default(Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, IAsyncReactiveQbservable<string>>);

            Apply(
                ctx =>
                {
                    xs = ctx.GetObservable<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observable.XS))("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7", "observable_parameter_8", "observable_parameter_9", "observable_parameter_10", "observable_parameter_11", "observable_parameter_12", "observable_parameter_13", "observable_parameter_14", "observable_parameter_15");
                    ys = ctx.GetObservable<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, string>(new Uri(Constants.Observable.YS));

                    var zs = from x in xs
                             from y in ys(x, x, x, x, x, x, x, x, x, x, x, x, x, x, x)
                             select x + y.Length;

                    var ob = ctx.GetObserver<int>(new Uri(Constants.Observer.OB));
                    zs.SubscribeAsync(ob, new Uri(Constants.Subscription.SUB), state: null).Wait();
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Parameter(typeof(int), "x").Let(x =>
                        Expression.Parameter(typeof(string), "y").Let(y =>
                            Expression.Invoke(
                                Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, IAsyncReactiveQbserver<int>, Task<IAsyncReactiveQubscription>>), Constants.SubscribeUri),
                                Expression.Invoke(
                                    Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, Expression<Func<int, IAsyncReactiveQbservable<string>>>, Expression<Func<int, string, int>>, IAsyncReactiveQbservable<int>>), Constants.Observable.SelectMany),
                                    Expression.Invoke(
                                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS),
                                        Expression.Constant("observable_parameter_1"),
                                        Expression.Constant("observable_parameter_2"),
                                        Expression.Constant("observable_parameter_3"),
                                        Expression.Constant("observable_parameter_4"),
                                        Expression.Constant("observable_parameter_5"),
                                        Expression.Constant("observable_parameter_6"),
                                        Expression.Constant("observable_parameter_7"),
                                        Expression.Constant("observable_parameter_8"),
                                        Expression.Constant("observable_parameter_9"),
                                        Expression.Constant("observable_parameter_10"),
                                        Expression.Constant("observable_parameter_11"),
                                        Expression.Constant("observable_parameter_12"),
                                        Expression.Constant("observable_parameter_13"),
                                        Expression.Constant("observable_parameter_14"),
                                        Expression.Constant("observable_parameter_15")
                                    ),
                                    Expression.Lambda(
                                        Expression.Invoke(
                                            Expression.Parameter(typeof(Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, IAsyncReactiveQbservable<string>>), Constants.Observable.YS),
                                            x, x, x, x, x, x, x, x, x, x, x, x, x, x, x
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
                    state: null
                )
            );
        }


        #endregion

        #region "ReactiveClientContext_SubscribeAsync_Parameterized_SelectMany3_CustomContext"

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_Parameterized_SelectMany3_CustomContext_2P()
        {
            Apply(
                provider => new MyParameterizedContext(provider),
                ctx =>
                {
                    var zs = from x1 in ctx.Xs2p("observable_parameter_1", "observable_parameter_2")
                             from x2 in ctx.Xs2p("observable_parameter_3", "observable_parameter_4")
                             select x1 + x2;

                    var ob = ctx.GetObserver<int>(new Uri(Constants.Observer.OB));
                    zs.SubscribeAsync(ob, new Uri(Constants.Subscription.SUB), state: null).Wait();
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Parameter(typeof(int), "x1").Let(x1 =>
                        Expression.Parameter(typeof(int), "x2").Let(x2 =>
                            Expression.Invoke(
                                Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, IAsyncReactiveQbserver<int>, Task<IAsyncReactiveQubscription>>), Constants.SubscribeUri),
                                Expression.Invoke(
                                    Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, Expression<Func<int, IAsyncReactiveQbservable<int>>>, Expression<Func<int, int, int>>, IAsyncReactiveQbservable<int>>), Constants.Observable.SelectMany),
                                    Expression.Invoke(
                                        Expression.Parameter(typeof(Func<string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS + 2),
                                        Expression.Constant("observable_parameter_1"),
                                        Expression.Constant("observable_parameter_2")
                                    ),
                                    Expression.Lambda(
                                        Expression.Invoke(
                                            Expression.Parameter(typeof(Func<string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS + 2),
                                            Expression.Constant("observable_parameter_3"),
                                            Expression.Constant("observable_parameter_4")
                                        ),
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
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_Parameterized_SelectMany3_CustomContext_3P()
        {
            Apply(
                provider => new MyParameterizedContext(provider),
                ctx =>
                {
                    var zs = from x1 in ctx.Xs3p("observable_parameter_1", "observable_parameter_2", "observable_parameter_3")
                             from x2 in ctx.Xs3p("observable_parameter_4", "observable_parameter_5", "observable_parameter_6")
                             select x1 + x2;

                    var ob = ctx.GetObserver<int>(new Uri(Constants.Observer.OB));
                    zs.SubscribeAsync(ob, new Uri(Constants.Subscription.SUB), state: null).Wait();
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Parameter(typeof(int), "x1").Let(x1 =>
                        Expression.Parameter(typeof(int), "x2").Let(x2 =>
                            Expression.Invoke(
                                Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, IAsyncReactiveQbserver<int>, Task<IAsyncReactiveQubscription>>), Constants.SubscribeUri),
                                Expression.Invoke(
                                    Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, Expression<Func<int, IAsyncReactiveQbservable<int>>>, Expression<Func<int, int, int>>, IAsyncReactiveQbservable<int>>), Constants.Observable.SelectMany),
                                    Expression.Invoke(
                                        Expression.Parameter(typeof(Func<string, string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS + 3),
                                        Expression.Constant("observable_parameter_1"),
                                        Expression.Constant("observable_parameter_2"),
                                        Expression.Constant("observable_parameter_3")
                                    ),
                                    Expression.Lambda(
                                        Expression.Invoke(
                                            Expression.Parameter(typeof(Func<string, string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS + 3),
                                            Expression.Constant("observable_parameter_4"),
                                            Expression.Constant("observable_parameter_5"),
                                            Expression.Constant("observable_parameter_6")
                                        ),
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
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_Parameterized_SelectMany3_CustomContext_4P()
        {
            Apply(
                provider => new MyParameterizedContext(provider),
                ctx =>
                {
                    var zs = from x1 in ctx.Xs4p("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4")
                             from x2 in ctx.Xs4p("observable_parameter_5", "observable_parameter_6", "observable_parameter_7", "observable_parameter_8")
                             select x1 + x2;

                    var ob = ctx.GetObserver<int>(new Uri(Constants.Observer.OB));
                    zs.SubscribeAsync(ob, new Uri(Constants.Subscription.SUB), state: null).Wait();
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Parameter(typeof(int), "x1").Let(x1 =>
                        Expression.Parameter(typeof(int), "x2").Let(x2 =>
                            Expression.Invoke(
                                Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, IAsyncReactiveQbserver<int>, Task<IAsyncReactiveQubscription>>), Constants.SubscribeUri),
                                Expression.Invoke(
                                    Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, Expression<Func<int, IAsyncReactiveQbservable<int>>>, Expression<Func<int, int, int>>, IAsyncReactiveQbservable<int>>), Constants.Observable.SelectMany),
                                    Expression.Invoke(
                                        Expression.Parameter(typeof(Func<string, string, string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS + 4),
                                        Expression.Constant("observable_parameter_1"),
                                        Expression.Constant("observable_parameter_2"),
                                        Expression.Constant("observable_parameter_3"),
                                        Expression.Constant("observable_parameter_4")
                                    ),
                                    Expression.Lambda(
                                        Expression.Invoke(
                                            Expression.Parameter(typeof(Func<string, string, string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS + 4),
                                            Expression.Constant("observable_parameter_5"),
                                            Expression.Constant("observable_parameter_6"),
                                            Expression.Constant("observable_parameter_7"),
                                            Expression.Constant("observable_parameter_8")
                                        ),
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
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_Parameterized_SelectMany3_CustomContext_5P()
        {
            Apply(
                provider => new MyParameterizedContext(provider),
                ctx =>
                {
                    var zs = from x1 in ctx.Xs5p("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5")
                             from x2 in ctx.Xs5p("observable_parameter_6", "observable_parameter_7", "observable_parameter_8", "observable_parameter_9", "observable_parameter_10")
                             select x1 + x2;

                    var ob = ctx.GetObserver<int>(new Uri(Constants.Observer.OB));
                    zs.SubscribeAsync(ob, new Uri(Constants.Subscription.SUB), state: null).Wait();
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Parameter(typeof(int), "x1").Let(x1 =>
                        Expression.Parameter(typeof(int), "x2").Let(x2 =>
                            Expression.Invoke(
                                Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, IAsyncReactiveQbserver<int>, Task<IAsyncReactiveQubscription>>), Constants.SubscribeUri),
                                Expression.Invoke(
                                    Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, Expression<Func<int, IAsyncReactiveQbservable<int>>>, Expression<Func<int, int, int>>, IAsyncReactiveQbservable<int>>), Constants.Observable.SelectMany),
                                    Expression.Invoke(
                                        Expression.Parameter(typeof(Func<string, string, string, string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS + 5),
                                        Expression.Constant("observable_parameter_1"),
                                        Expression.Constant("observable_parameter_2"),
                                        Expression.Constant("observable_parameter_3"),
                                        Expression.Constant("observable_parameter_4"),
                                        Expression.Constant("observable_parameter_5")
                                    ),
                                    Expression.Lambda(
                                        Expression.Invoke(
                                            Expression.Parameter(typeof(Func<string, string, string, string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS + 5),
                                            Expression.Constant("observable_parameter_6"),
                                            Expression.Constant("observable_parameter_7"),
                                            Expression.Constant("observable_parameter_8"),
                                            Expression.Constant("observable_parameter_9"),
                                            Expression.Constant("observable_parameter_10")
                                        ),
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
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_Parameterized_SelectMany3_CustomContext_6P()
        {
            Apply(
                provider => new MyParameterizedContext(provider),
                ctx =>
                {
                    var zs = from x1 in ctx.Xs6p("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6")
                             from x2 in ctx.Xs6p("observable_parameter_7", "observable_parameter_8", "observable_parameter_9", "observable_parameter_10", "observable_parameter_11", "observable_parameter_12")
                             select x1 + x2;

                    var ob = ctx.GetObserver<int>(new Uri(Constants.Observer.OB));
                    zs.SubscribeAsync(ob, new Uri(Constants.Subscription.SUB), state: null).Wait();
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Parameter(typeof(int), "x1").Let(x1 =>
                        Expression.Parameter(typeof(int), "x2").Let(x2 =>
                            Expression.Invoke(
                                Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, IAsyncReactiveQbserver<int>, Task<IAsyncReactiveQubscription>>), Constants.SubscribeUri),
                                Expression.Invoke(
                                    Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, Expression<Func<int, IAsyncReactiveQbservable<int>>>, Expression<Func<int, int, int>>, IAsyncReactiveQbservable<int>>), Constants.Observable.SelectMany),
                                    Expression.Invoke(
                                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS + 6),
                                        Expression.Constant("observable_parameter_1"),
                                        Expression.Constant("observable_parameter_2"),
                                        Expression.Constant("observable_parameter_3"),
                                        Expression.Constant("observable_parameter_4"),
                                        Expression.Constant("observable_parameter_5"),
                                        Expression.Constant("observable_parameter_6")
                                    ),
                                    Expression.Lambda(
                                        Expression.Invoke(
                                            Expression.Parameter(typeof(Func<string, string, string, string, string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS + 6),
                                            Expression.Constant("observable_parameter_7"),
                                            Expression.Constant("observable_parameter_8"),
                                            Expression.Constant("observable_parameter_9"),
                                            Expression.Constant("observable_parameter_10"),
                                            Expression.Constant("observable_parameter_11"),
                                            Expression.Constant("observable_parameter_12")
                                        ),
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
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_Parameterized_SelectMany3_CustomContext_7P()
        {
            Apply(
                provider => new MyParameterizedContext(provider),
                ctx =>
                {
                    var zs = from x1 in ctx.Xs7p("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7")
                             from x2 in ctx.Xs7p("observable_parameter_8", "observable_parameter_9", "observable_parameter_10", "observable_parameter_11", "observable_parameter_12", "observable_parameter_13", "observable_parameter_14")
                             select x1 + x2;

                    var ob = ctx.GetObserver<int>(new Uri(Constants.Observer.OB));
                    zs.SubscribeAsync(ob, new Uri(Constants.Subscription.SUB), state: null).Wait();
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Parameter(typeof(int), "x1").Let(x1 =>
                        Expression.Parameter(typeof(int), "x2").Let(x2 =>
                            Expression.Invoke(
                                Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, IAsyncReactiveQbserver<int>, Task<IAsyncReactiveQubscription>>), Constants.SubscribeUri),
                                Expression.Invoke(
                                    Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, Expression<Func<int, IAsyncReactiveQbservable<int>>>, Expression<Func<int, int, int>>, IAsyncReactiveQbservable<int>>), Constants.Observable.SelectMany),
                                    Expression.Invoke(
                                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS + 7),
                                        Expression.Constant("observable_parameter_1"),
                                        Expression.Constant("observable_parameter_2"),
                                        Expression.Constant("observable_parameter_3"),
                                        Expression.Constant("observable_parameter_4"),
                                        Expression.Constant("observable_parameter_5"),
                                        Expression.Constant("observable_parameter_6"),
                                        Expression.Constant("observable_parameter_7")
                                    ),
                                    Expression.Lambda(
                                        Expression.Invoke(
                                            Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS + 7),
                                            Expression.Constant("observable_parameter_8"),
                                            Expression.Constant("observable_parameter_9"),
                                            Expression.Constant("observable_parameter_10"),
                                            Expression.Constant("observable_parameter_11"),
                                            Expression.Constant("observable_parameter_12"),
                                            Expression.Constant("observable_parameter_13"),
                                            Expression.Constant("observable_parameter_14")
                                        ),
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
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_Parameterized_SelectMany3_CustomContext_8P()
        {
            Apply(
                provider => new MyParameterizedContext(provider),
                ctx =>
                {
                    var zs = from x1 in ctx.Xs8p("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7", "observable_parameter_8")
                             from x2 in ctx.Xs8p("observable_parameter_9", "observable_parameter_10", "observable_parameter_11", "observable_parameter_12", "observable_parameter_13", "observable_parameter_14", "observable_parameter_15", "observable_parameter_16")
                             select x1 + x2;

                    var ob = ctx.GetObserver<int>(new Uri(Constants.Observer.OB));
                    zs.SubscribeAsync(ob, new Uri(Constants.Subscription.SUB), state: null).Wait();
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Parameter(typeof(int), "x1").Let(x1 =>
                        Expression.Parameter(typeof(int), "x2").Let(x2 =>
                            Expression.Invoke(
                                Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, IAsyncReactiveQbserver<int>, Task<IAsyncReactiveQubscription>>), Constants.SubscribeUri),
                                Expression.Invoke(
                                    Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, Expression<Func<int, IAsyncReactiveQbservable<int>>>, Expression<Func<int, int, int>>, IAsyncReactiveQbservable<int>>), Constants.Observable.SelectMany),
                                    Expression.Invoke(
                                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS + 8),
                                        Expression.Constant("observable_parameter_1"),
                                        Expression.Constant("observable_parameter_2"),
                                        Expression.Constant("observable_parameter_3"),
                                        Expression.Constant("observable_parameter_4"),
                                        Expression.Constant("observable_parameter_5"),
                                        Expression.Constant("observable_parameter_6"),
                                        Expression.Constant("observable_parameter_7"),
                                        Expression.Constant("observable_parameter_8")
                                    ),
                                    Expression.Lambda(
                                        Expression.Invoke(
                                            Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS + 8),
                                            Expression.Constant("observable_parameter_9"),
                                            Expression.Constant("observable_parameter_10"),
                                            Expression.Constant("observable_parameter_11"),
                                            Expression.Constant("observable_parameter_12"),
                                            Expression.Constant("observable_parameter_13"),
                                            Expression.Constant("observable_parameter_14"),
                                            Expression.Constant("observable_parameter_15"),
                                            Expression.Constant("observable_parameter_16")
                                        ),
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
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_Parameterized_SelectMany3_CustomContext_9P()
        {
            Apply(
                provider => new MyParameterizedContext(provider),
                ctx =>
                {
                    var zs = from x1 in ctx.Xs9p("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7", "observable_parameter_8", "observable_parameter_9")
                             from x2 in ctx.Xs9p("observable_parameter_10", "observable_parameter_11", "observable_parameter_12", "observable_parameter_13", "observable_parameter_14", "observable_parameter_15", "observable_parameter_16", "observable_parameter_17", "observable_parameter_18")
                             select x1 + x2;

                    var ob = ctx.GetObserver<int>(new Uri(Constants.Observer.OB));
                    zs.SubscribeAsync(ob, new Uri(Constants.Subscription.SUB), state: null).Wait();
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Parameter(typeof(int), "x1").Let(x1 =>
                        Expression.Parameter(typeof(int), "x2").Let(x2 =>
                            Expression.Invoke(
                                Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, IAsyncReactiveQbserver<int>, Task<IAsyncReactiveQubscription>>), Constants.SubscribeUri),
                                Expression.Invoke(
                                    Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, Expression<Func<int, IAsyncReactiveQbservable<int>>>, Expression<Func<int, int, int>>, IAsyncReactiveQbservable<int>>), Constants.Observable.SelectMany),
                                    Expression.Invoke(
                                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS + 9),
                                        Expression.Constant("observable_parameter_1"),
                                        Expression.Constant("observable_parameter_2"),
                                        Expression.Constant("observable_parameter_3"),
                                        Expression.Constant("observable_parameter_4"),
                                        Expression.Constant("observable_parameter_5"),
                                        Expression.Constant("observable_parameter_6"),
                                        Expression.Constant("observable_parameter_7"),
                                        Expression.Constant("observable_parameter_8"),
                                        Expression.Constant("observable_parameter_9")
                                    ),
                                    Expression.Lambda(
                                        Expression.Invoke(
                                            Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS + 9),
                                            Expression.Constant("observable_parameter_10"),
                                            Expression.Constant("observable_parameter_11"),
                                            Expression.Constant("observable_parameter_12"),
                                            Expression.Constant("observable_parameter_13"),
                                            Expression.Constant("observable_parameter_14"),
                                            Expression.Constant("observable_parameter_15"),
                                            Expression.Constant("observable_parameter_16"),
                                            Expression.Constant("observable_parameter_17"),
                                            Expression.Constant("observable_parameter_18")
                                        ),
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
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_Parameterized_SelectMany3_CustomContext_10P()
        {
            Apply(
                provider => new MyParameterizedContext(provider),
                ctx =>
                {
                    var zs = from x1 in ctx.Xs10p("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7", "observable_parameter_8", "observable_parameter_9", "observable_parameter_10")
                             from x2 in ctx.Xs10p("observable_parameter_11", "observable_parameter_12", "observable_parameter_13", "observable_parameter_14", "observable_parameter_15", "observable_parameter_16", "observable_parameter_17", "observable_parameter_18", "observable_parameter_19", "observable_parameter_20")
                             select x1 + x2;

                    var ob = ctx.GetObserver<int>(new Uri(Constants.Observer.OB));
                    zs.SubscribeAsync(ob, new Uri(Constants.Subscription.SUB), state: null).Wait();
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Parameter(typeof(int), "x1").Let(x1 =>
                        Expression.Parameter(typeof(int), "x2").Let(x2 =>
                            Expression.Invoke(
                                Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, IAsyncReactiveQbserver<int>, Task<IAsyncReactiveQubscription>>), Constants.SubscribeUri),
                                Expression.Invoke(
                                    Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, Expression<Func<int, IAsyncReactiveQbservable<int>>>, Expression<Func<int, int, int>>, IAsyncReactiveQbservable<int>>), Constants.Observable.SelectMany),
                                    Expression.Invoke(
                                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS + 10),
                                        Expression.Constant("observable_parameter_1"),
                                        Expression.Constant("observable_parameter_2"),
                                        Expression.Constant("observable_parameter_3"),
                                        Expression.Constant("observable_parameter_4"),
                                        Expression.Constant("observable_parameter_5"),
                                        Expression.Constant("observable_parameter_6"),
                                        Expression.Constant("observable_parameter_7"),
                                        Expression.Constant("observable_parameter_8"),
                                        Expression.Constant("observable_parameter_9"),
                                        Expression.Constant("observable_parameter_10")
                                    ),
                                    Expression.Lambda(
                                        Expression.Invoke(
                                            Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS + 10),
                                            Expression.Constant("observable_parameter_11"),
                                            Expression.Constant("observable_parameter_12"),
                                            Expression.Constant("observable_parameter_13"),
                                            Expression.Constant("observable_parameter_14"),
                                            Expression.Constant("observable_parameter_15"),
                                            Expression.Constant("observable_parameter_16"),
                                            Expression.Constant("observable_parameter_17"),
                                            Expression.Constant("observable_parameter_18"),
                                            Expression.Constant("observable_parameter_19"),
                                            Expression.Constant("observable_parameter_20")
                                        ),
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
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_Parameterized_SelectMany3_CustomContext_11P()
        {
            Apply(
                provider => new MyParameterizedContext(provider),
                ctx =>
                {
                    var zs = from x1 in ctx.Xs11p("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7", "observable_parameter_8", "observable_parameter_9", "observable_parameter_10", "observable_parameter_11")
                             from x2 in ctx.Xs11p("observable_parameter_12", "observable_parameter_13", "observable_parameter_14", "observable_parameter_15", "observable_parameter_16", "observable_parameter_17", "observable_parameter_18", "observable_parameter_19", "observable_parameter_20", "observable_parameter_21", "observable_parameter_22")
                             select x1 + x2;

                    var ob = ctx.GetObserver<int>(new Uri(Constants.Observer.OB));
                    zs.SubscribeAsync(ob, new Uri(Constants.Subscription.SUB), state: null).Wait();
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Parameter(typeof(int), "x1").Let(x1 =>
                        Expression.Parameter(typeof(int), "x2").Let(x2 =>
                            Expression.Invoke(
                                Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, IAsyncReactiveQbserver<int>, Task<IAsyncReactiveQubscription>>), Constants.SubscribeUri),
                                Expression.Invoke(
                                    Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, Expression<Func<int, IAsyncReactiveQbservable<int>>>, Expression<Func<int, int, int>>, IAsyncReactiveQbservable<int>>), Constants.Observable.SelectMany),
                                    Expression.Invoke(
                                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS + 11),
                                        Expression.Constant("observable_parameter_1"),
                                        Expression.Constant("observable_parameter_2"),
                                        Expression.Constant("observable_parameter_3"),
                                        Expression.Constant("observable_parameter_4"),
                                        Expression.Constant("observable_parameter_5"),
                                        Expression.Constant("observable_parameter_6"),
                                        Expression.Constant("observable_parameter_7"),
                                        Expression.Constant("observable_parameter_8"),
                                        Expression.Constant("observable_parameter_9"),
                                        Expression.Constant("observable_parameter_10"),
                                        Expression.Constant("observable_parameter_11")
                                    ),
                                    Expression.Lambda(
                                        Expression.Invoke(
                                            Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS + 11),
                                            Expression.Constant("observable_parameter_12"),
                                            Expression.Constant("observable_parameter_13"),
                                            Expression.Constant("observable_parameter_14"),
                                            Expression.Constant("observable_parameter_15"),
                                            Expression.Constant("observable_parameter_16"),
                                            Expression.Constant("observable_parameter_17"),
                                            Expression.Constant("observable_parameter_18"),
                                            Expression.Constant("observable_parameter_19"),
                                            Expression.Constant("observable_parameter_20"),
                                            Expression.Constant("observable_parameter_21"),
                                            Expression.Constant("observable_parameter_22")
                                        ),
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
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_Parameterized_SelectMany3_CustomContext_12P()
        {
            Apply(
                provider => new MyParameterizedContext(provider),
                ctx =>
                {
                    var zs = from x1 in ctx.Xs12p("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7", "observable_parameter_8", "observable_parameter_9", "observable_parameter_10", "observable_parameter_11", "observable_parameter_12")
                             from x2 in ctx.Xs12p("observable_parameter_13", "observable_parameter_14", "observable_parameter_15", "observable_parameter_16", "observable_parameter_17", "observable_parameter_18", "observable_parameter_19", "observable_parameter_20", "observable_parameter_21", "observable_parameter_22", "observable_parameter_23", "observable_parameter_24")
                             select x1 + x2;

                    var ob = ctx.GetObserver<int>(new Uri(Constants.Observer.OB));
                    zs.SubscribeAsync(ob, new Uri(Constants.Subscription.SUB), state: null).Wait();
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Parameter(typeof(int), "x1").Let(x1 =>
                        Expression.Parameter(typeof(int), "x2").Let(x2 =>
                            Expression.Invoke(
                                Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, IAsyncReactiveQbserver<int>, Task<IAsyncReactiveQubscription>>), Constants.SubscribeUri),
                                Expression.Invoke(
                                    Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, Expression<Func<int, IAsyncReactiveQbservable<int>>>, Expression<Func<int, int, int>>, IAsyncReactiveQbservable<int>>), Constants.Observable.SelectMany),
                                    Expression.Invoke(
                                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS + 12),
                                        Expression.Constant("observable_parameter_1"),
                                        Expression.Constant("observable_parameter_2"),
                                        Expression.Constant("observable_parameter_3"),
                                        Expression.Constant("observable_parameter_4"),
                                        Expression.Constant("observable_parameter_5"),
                                        Expression.Constant("observable_parameter_6"),
                                        Expression.Constant("observable_parameter_7"),
                                        Expression.Constant("observable_parameter_8"),
                                        Expression.Constant("observable_parameter_9"),
                                        Expression.Constant("observable_parameter_10"),
                                        Expression.Constant("observable_parameter_11"),
                                        Expression.Constant("observable_parameter_12")
                                    ),
                                    Expression.Lambda(
                                        Expression.Invoke(
                                            Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS + 12),
                                            Expression.Constant("observable_parameter_13"),
                                            Expression.Constant("observable_parameter_14"),
                                            Expression.Constant("observable_parameter_15"),
                                            Expression.Constant("observable_parameter_16"),
                                            Expression.Constant("observable_parameter_17"),
                                            Expression.Constant("observable_parameter_18"),
                                            Expression.Constant("observable_parameter_19"),
                                            Expression.Constant("observable_parameter_20"),
                                            Expression.Constant("observable_parameter_21"),
                                            Expression.Constant("observable_parameter_22"),
                                            Expression.Constant("observable_parameter_23"),
                                            Expression.Constant("observable_parameter_24")
                                        ),
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
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_Parameterized_SelectMany3_CustomContext_13P()
        {
            Apply(
                provider => new MyParameterizedContext(provider),
                ctx =>
                {
                    var zs = from x1 in ctx.Xs13p("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7", "observable_parameter_8", "observable_parameter_9", "observable_parameter_10", "observable_parameter_11", "observable_parameter_12", "observable_parameter_13")
                             from x2 in ctx.Xs13p("observable_parameter_14", "observable_parameter_15", "observable_parameter_16", "observable_parameter_17", "observable_parameter_18", "observable_parameter_19", "observable_parameter_20", "observable_parameter_21", "observable_parameter_22", "observable_parameter_23", "observable_parameter_24", "observable_parameter_25", "observable_parameter_26")
                             select x1 + x2;

                    var ob = ctx.GetObserver<int>(new Uri(Constants.Observer.OB));
                    zs.SubscribeAsync(ob, new Uri(Constants.Subscription.SUB), state: null).Wait();
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Parameter(typeof(int), "x1").Let(x1 =>
                        Expression.Parameter(typeof(int), "x2").Let(x2 =>
                            Expression.Invoke(
                                Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, IAsyncReactiveQbserver<int>, Task<IAsyncReactiveQubscription>>), Constants.SubscribeUri),
                                Expression.Invoke(
                                    Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, Expression<Func<int, IAsyncReactiveQbservable<int>>>, Expression<Func<int, int, int>>, IAsyncReactiveQbservable<int>>), Constants.Observable.SelectMany),
                                    Expression.Invoke(
                                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS + 13),
                                        Expression.Constant("observable_parameter_1"),
                                        Expression.Constant("observable_parameter_2"),
                                        Expression.Constant("observable_parameter_3"),
                                        Expression.Constant("observable_parameter_4"),
                                        Expression.Constant("observable_parameter_5"),
                                        Expression.Constant("observable_parameter_6"),
                                        Expression.Constant("observable_parameter_7"),
                                        Expression.Constant("observable_parameter_8"),
                                        Expression.Constant("observable_parameter_9"),
                                        Expression.Constant("observable_parameter_10"),
                                        Expression.Constant("observable_parameter_11"),
                                        Expression.Constant("observable_parameter_12"),
                                        Expression.Constant("observable_parameter_13")
                                    ),
                                    Expression.Lambda(
                                        Expression.Invoke(
                                            Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS + 13),
                                            Expression.Constant("observable_parameter_14"),
                                            Expression.Constant("observable_parameter_15"),
                                            Expression.Constant("observable_parameter_16"),
                                            Expression.Constant("observable_parameter_17"),
                                            Expression.Constant("observable_parameter_18"),
                                            Expression.Constant("observable_parameter_19"),
                                            Expression.Constant("observable_parameter_20"),
                                            Expression.Constant("observable_parameter_21"),
                                            Expression.Constant("observable_parameter_22"),
                                            Expression.Constant("observable_parameter_23"),
                                            Expression.Constant("observable_parameter_24"),
                                            Expression.Constant("observable_parameter_25"),
                                            Expression.Constant("observable_parameter_26")
                                        ),
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
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_Parameterized_SelectMany3_CustomContext_14P()
        {
            Apply(
                provider => new MyParameterizedContext(provider),
                ctx =>
                {
                    var zs = from x1 in ctx.Xs14p("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7", "observable_parameter_8", "observable_parameter_9", "observable_parameter_10", "observable_parameter_11", "observable_parameter_12", "observable_parameter_13", "observable_parameter_14")
                             from x2 in ctx.Xs14p("observable_parameter_15", "observable_parameter_16", "observable_parameter_17", "observable_parameter_18", "observable_parameter_19", "observable_parameter_20", "observable_parameter_21", "observable_parameter_22", "observable_parameter_23", "observable_parameter_24", "observable_parameter_25", "observable_parameter_26", "observable_parameter_27", "observable_parameter_28")
                             select x1 + x2;

                    var ob = ctx.GetObserver<int>(new Uri(Constants.Observer.OB));
                    zs.SubscribeAsync(ob, new Uri(Constants.Subscription.SUB), state: null).Wait();
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Parameter(typeof(int), "x1").Let(x1 =>
                        Expression.Parameter(typeof(int), "x2").Let(x2 =>
                            Expression.Invoke(
                                Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, IAsyncReactiveQbserver<int>, Task<IAsyncReactiveQubscription>>), Constants.SubscribeUri),
                                Expression.Invoke(
                                    Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, Expression<Func<int, IAsyncReactiveQbservable<int>>>, Expression<Func<int, int, int>>, IAsyncReactiveQbservable<int>>), Constants.Observable.SelectMany),
                                    Expression.Invoke(
                                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS + 14),
                                        Expression.Constant("observable_parameter_1"),
                                        Expression.Constant("observable_parameter_2"),
                                        Expression.Constant("observable_parameter_3"),
                                        Expression.Constant("observable_parameter_4"),
                                        Expression.Constant("observable_parameter_5"),
                                        Expression.Constant("observable_parameter_6"),
                                        Expression.Constant("observable_parameter_7"),
                                        Expression.Constant("observable_parameter_8"),
                                        Expression.Constant("observable_parameter_9"),
                                        Expression.Constant("observable_parameter_10"),
                                        Expression.Constant("observable_parameter_11"),
                                        Expression.Constant("observable_parameter_12"),
                                        Expression.Constant("observable_parameter_13"),
                                        Expression.Constant("observable_parameter_14")
                                    ),
                                    Expression.Lambda(
                                        Expression.Invoke(
                                            Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS + 14),
                                            Expression.Constant("observable_parameter_15"),
                                            Expression.Constant("observable_parameter_16"),
                                            Expression.Constant("observable_parameter_17"),
                                            Expression.Constant("observable_parameter_18"),
                                            Expression.Constant("observable_parameter_19"),
                                            Expression.Constant("observable_parameter_20"),
                                            Expression.Constant("observable_parameter_21"),
                                            Expression.Constant("observable_parameter_22"),
                                            Expression.Constant("observable_parameter_23"),
                                            Expression.Constant("observable_parameter_24"),
                                            Expression.Constant("observable_parameter_25"),
                                            Expression.Constant("observable_parameter_26"),
                                            Expression.Constant("observable_parameter_27"),
                                            Expression.Constant("observable_parameter_28")
                                        ),
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
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_Parameterized_SelectMany3_CustomContext_15P()
        {
            Apply(
                provider => new MyParameterizedContext(provider),
                ctx =>
                {
                    var zs = from x1 in ctx.Xs15p("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7", "observable_parameter_8", "observable_parameter_9", "observable_parameter_10", "observable_parameter_11", "observable_parameter_12", "observable_parameter_13", "observable_parameter_14", "observable_parameter_15")
                             from x2 in ctx.Xs15p("observable_parameter_16", "observable_parameter_17", "observable_parameter_18", "observable_parameter_19", "observable_parameter_20", "observable_parameter_21", "observable_parameter_22", "observable_parameter_23", "observable_parameter_24", "observable_parameter_25", "observable_parameter_26", "observable_parameter_27", "observable_parameter_28", "observable_parameter_29", "observable_parameter_30")
                             select x1 + x2;

                    var ob = ctx.GetObserver<int>(new Uri(Constants.Observer.OB));
                    zs.SubscribeAsync(ob, new Uri(Constants.Subscription.SUB), state: null).Wait();
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Parameter(typeof(int), "x1").Let(x1 =>
                        Expression.Parameter(typeof(int), "x2").Let(x2 =>
                            Expression.Invoke(
                                Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, IAsyncReactiveQbserver<int>, Task<IAsyncReactiveQubscription>>), Constants.SubscribeUri),
                                Expression.Invoke(
                                    Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, Expression<Func<int, IAsyncReactiveQbservable<int>>>, Expression<Func<int, int, int>>, IAsyncReactiveQbservable<int>>), Constants.Observable.SelectMany),
                                    Expression.Invoke(
                                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS + 15),
                                        Expression.Constant("observable_parameter_1"),
                                        Expression.Constant("observable_parameter_2"),
                                        Expression.Constant("observable_parameter_3"),
                                        Expression.Constant("observable_parameter_4"),
                                        Expression.Constant("observable_parameter_5"),
                                        Expression.Constant("observable_parameter_6"),
                                        Expression.Constant("observable_parameter_7"),
                                        Expression.Constant("observable_parameter_8"),
                                        Expression.Constant("observable_parameter_9"),
                                        Expression.Constant("observable_parameter_10"),
                                        Expression.Constant("observable_parameter_11"),
                                        Expression.Constant("observable_parameter_12"),
                                        Expression.Constant("observable_parameter_13"),
                                        Expression.Constant("observable_parameter_14"),
                                        Expression.Constant("observable_parameter_15")
                                    ),
                                    Expression.Lambda(
                                        Expression.Invoke(
                                            Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS + 15),
                                            Expression.Constant("observable_parameter_16"),
                                            Expression.Constant("observable_parameter_17"),
                                            Expression.Constant("observable_parameter_18"),
                                            Expression.Constant("observable_parameter_19"),
                                            Expression.Constant("observable_parameter_20"),
                                            Expression.Constant("observable_parameter_21"),
                                            Expression.Constant("observable_parameter_22"),
                                            Expression.Constant("observable_parameter_23"),
                                            Expression.Constant("observable_parameter_24"),
                                            Expression.Constant("observable_parameter_25"),
                                            Expression.Constant("observable_parameter_26"),
                                            Expression.Constant("observable_parameter_27"),
                                            Expression.Constant("observable_parameter_28"),
                                            Expression.Constant("observable_parameter_29"),
                                            Expression.Constant("observable_parameter_30")
                                        ),
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
                    state: null
                )
            );
        }


        #endregion

        #region "ReactiveClientContext_SubscribeAsync_Parameterized_Inlined"

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_Parameterized_Inlined_2P()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<string, string, int>(new Uri(Constants.Observable.XS))("observable_parameter_1", "observable_parameter_2");

                    var zs = from x in xs
                             from y in ctx.GetObservable<int, int, string>(new Uri(Constants.Observable.YS))(x, x)
                             select x + y.Length;

                    var ob = ctx.GetObserver<int>(new Uri(Constants.Observer.OB));
                    zs.SubscribeAsync(ob, new Uri(Constants.Subscription.SUB), state: null).Wait();
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Parameter(typeof(int), "x").Let(x =>
                        Expression.Parameter(typeof(string), "y").Let(y =>
                            Expression.Invoke(
                                Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, IAsyncReactiveQbserver<int>, Task<IAsyncReactiveQubscription>>), Constants.SubscribeUri),
                                Expression.Invoke(
                                    Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, Expression<Func<int, IAsyncReactiveQbservable<string>>>, Expression<Func<int, string, int>>, IAsyncReactiveQbservable<int>>), Constants.Observable.SelectMany),
                                    Expression.Invoke(
                                        Expression.Parameter(typeof(Func<string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS),
                                        Expression.Constant("observable_parameter_1"),
                                        Expression.Constant("observable_parameter_2")
                                    ),
                                    Expression.Lambda(
                                        Expression.Invoke(
                                            Expression.Call(
                                                Expression.Parameter(typeof(TestClientContext), "rx://builtin/this"),
                                                "GetObservable",
                                                new Type[] { typeof(int), typeof(int), typeof(string) },
                                                new Expression[] {
                                                    Expression.New(typeof(Uri).GetConstructor(new[] { typeof(string) }), Expression.Constant(Constants.Observable.YS))
                                                }
                                            ),
                                            x, x
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
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_Parameterized_Inlined_3P()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<string, string, string, int>(new Uri(Constants.Observable.XS))("observable_parameter_1", "observable_parameter_2", "observable_parameter_3");

                    var zs = from x in xs
                             from y in ctx.GetObservable<int, int, int, string>(new Uri(Constants.Observable.YS))(x, x, x)
                             select x + y.Length;

                    var ob = ctx.GetObserver<int>(new Uri(Constants.Observer.OB));
                    zs.SubscribeAsync(ob, new Uri(Constants.Subscription.SUB), state: null).Wait();
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Parameter(typeof(int), "x").Let(x =>
                        Expression.Parameter(typeof(string), "y").Let(y =>
                            Expression.Invoke(
                                Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, IAsyncReactiveQbserver<int>, Task<IAsyncReactiveQubscription>>), Constants.SubscribeUri),
                                Expression.Invoke(
                                    Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, Expression<Func<int, IAsyncReactiveQbservable<string>>>, Expression<Func<int, string, int>>, IAsyncReactiveQbservable<int>>), Constants.Observable.SelectMany),
                                    Expression.Invoke(
                                        Expression.Parameter(typeof(Func<string, string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS),
                                        Expression.Constant("observable_parameter_1"),
                                        Expression.Constant("observable_parameter_2"),
                                        Expression.Constant("observable_parameter_3")
                                    ),
                                    Expression.Lambda(
                                        Expression.Invoke(
                                            Expression.Call(
                                                Expression.Parameter(typeof(TestClientContext), "rx://builtin/this"),
                                                "GetObservable",
                                                new Type[] { typeof(int), typeof(int), typeof(int), typeof(string) },
                                                new Expression[] {
                                                    Expression.New(typeof(Uri).GetConstructor(new[] { typeof(string) }), Expression.Constant(Constants.Observable.YS))
                                                }
                                            ),
                                            x, x, x
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
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_Parameterized_Inlined_4P()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<string, string, string, string, int>(new Uri(Constants.Observable.XS))("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4");

                    var zs = from x in xs
                             from y in ctx.GetObservable<int, int, int, int, string>(new Uri(Constants.Observable.YS))(x, x, x, x)
                             select x + y.Length;

                    var ob = ctx.GetObserver<int>(new Uri(Constants.Observer.OB));
                    zs.SubscribeAsync(ob, new Uri(Constants.Subscription.SUB), state: null).Wait();
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Parameter(typeof(int), "x").Let(x =>
                        Expression.Parameter(typeof(string), "y").Let(y =>
                            Expression.Invoke(
                                Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, IAsyncReactiveQbserver<int>, Task<IAsyncReactiveQubscription>>), Constants.SubscribeUri),
                                Expression.Invoke(
                                    Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, Expression<Func<int, IAsyncReactiveQbservable<string>>>, Expression<Func<int, string, int>>, IAsyncReactiveQbservable<int>>), Constants.Observable.SelectMany),
                                    Expression.Invoke(
                                        Expression.Parameter(typeof(Func<string, string, string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS),
                                        Expression.Constant("observable_parameter_1"),
                                        Expression.Constant("observable_parameter_2"),
                                        Expression.Constant("observable_parameter_3"),
                                        Expression.Constant("observable_parameter_4")
                                    ),
                                    Expression.Lambda(
                                        Expression.Invoke(
                                            Expression.Call(
                                                Expression.Parameter(typeof(TestClientContext), "rx://builtin/this"),
                                                "GetObservable",
                                                new Type[] { typeof(int), typeof(int), typeof(int), typeof(int), typeof(string) },
                                                new Expression[] {
                                                    Expression.New(typeof(Uri).GetConstructor(new[] { typeof(string) }), Expression.Constant(Constants.Observable.YS))
                                                }
                                            ),
                                            x, x, x, x
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
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_Parameterized_Inlined_5P()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<string, string, string, string, string, int>(new Uri(Constants.Observable.XS))("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5");

                    var zs = from x in xs
                             from y in ctx.GetObservable<int, int, int, int, int, string>(new Uri(Constants.Observable.YS))(x, x, x, x, x)
                             select x + y.Length;

                    var ob = ctx.GetObserver<int>(new Uri(Constants.Observer.OB));
                    zs.SubscribeAsync(ob, new Uri(Constants.Subscription.SUB), state: null).Wait();
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Parameter(typeof(int), "x").Let(x =>
                        Expression.Parameter(typeof(string), "y").Let(y =>
                            Expression.Invoke(
                                Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, IAsyncReactiveQbserver<int>, Task<IAsyncReactiveQubscription>>), Constants.SubscribeUri),
                                Expression.Invoke(
                                    Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, Expression<Func<int, IAsyncReactiveQbservable<string>>>, Expression<Func<int, string, int>>, IAsyncReactiveQbservable<int>>), Constants.Observable.SelectMany),
                                    Expression.Invoke(
                                        Expression.Parameter(typeof(Func<string, string, string, string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS),
                                        Expression.Constant("observable_parameter_1"),
                                        Expression.Constant("observable_parameter_2"),
                                        Expression.Constant("observable_parameter_3"),
                                        Expression.Constant("observable_parameter_4"),
                                        Expression.Constant("observable_parameter_5")
                                    ),
                                    Expression.Lambda(
                                        Expression.Invoke(
                                            Expression.Call(
                                                Expression.Parameter(typeof(TestClientContext), "rx://builtin/this"),
                                                "GetObservable",
                                                new Type[] { typeof(int), typeof(int), typeof(int), typeof(int), typeof(int), typeof(string) },
                                                new Expression[] {
                                                    Expression.New(typeof(Uri).GetConstructor(new[] { typeof(string) }), Expression.Constant(Constants.Observable.YS))
                                                }
                                            ),
                                            x, x, x, x, x
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
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_Parameterized_Inlined_6P()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<string, string, string, string, string, string, int>(new Uri(Constants.Observable.XS))("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6");

                    var zs = from x in xs
                             from y in ctx.GetObservable<int, int, int, int, int, int, string>(new Uri(Constants.Observable.YS))(x, x, x, x, x, x)
                             select x + y.Length;

                    var ob = ctx.GetObserver<int>(new Uri(Constants.Observer.OB));
                    zs.SubscribeAsync(ob, new Uri(Constants.Subscription.SUB), state: null).Wait();
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Parameter(typeof(int), "x").Let(x =>
                        Expression.Parameter(typeof(string), "y").Let(y =>
                            Expression.Invoke(
                                Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, IAsyncReactiveQbserver<int>, Task<IAsyncReactiveQubscription>>), Constants.SubscribeUri),
                                Expression.Invoke(
                                    Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, Expression<Func<int, IAsyncReactiveQbservable<string>>>, Expression<Func<int, string, int>>, IAsyncReactiveQbservable<int>>), Constants.Observable.SelectMany),
                                    Expression.Invoke(
                                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS),
                                        Expression.Constant("observable_parameter_1"),
                                        Expression.Constant("observable_parameter_2"),
                                        Expression.Constant("observable_parameter_3"),
                                        Expression.Constant("observable_parameter_4"),
                                        Expression.Constant("observable_parameter_5"),
                                        Expression.Constant("observable_parameter_6")
                                    ),
                                    Expression.Lambda(
                                        Expression.Invoke(
                                            Expression.Call(
                                                Expression.Parameter(typeof(TestClientContext), "rx://builtin/this"),
                                                "GetObservable",
                                                new Type[] { typeof(int), typeof(int), typeof(int), typeof(int), typeof(int), typeof(int), typeof(string) },
                                                new Expression[] {
                                                    Expression.New(typeof(Uri).GetConstructor(new[] { typeof(string) }), Expression.Constant(Constants.Observable.YS))
                                                }
                                            ),
                                            x, x, x, x, x, x
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
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_Parameterized_Inlined_7P()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<string, string, string, string, string, string, string, int>(new Uri(Constants.Observable.XS))("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7");

                    var zs = from x in xs
                             from y in ctx.GetObservable<int, int, int, int, int, int, int, string>(new Uri(Constants.Observable.YS))(x, x, x, x, x, x, x)
                             select x + y.Length;

                    var ob = ctx.GetObserver<int>(new Uri(Constants.Observer.OB));
                    zs.SubscribeAsync(ob, new Uri(Constants.Subscription.SUB), state: null).Wait();
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Parameter(typeof(int), "x").Let(x =>
                        Expression.Parameter(typeof(string), "y").Let(y =>
                            Expression.Invoke(
                                Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, IAsyncReactiveQbserver<int>, Task<IAsyncReactiveQubscription>>), Constants.SubscribeUri),
                                Expression.Invoke(
                                    Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, Expression<Func<int, IAsyncReactiveQbservable<string>>>, Expression<Func<int, string, int>>, IAsyncReactiveQbservable<int>>), Constants.Observable.SelectMany),
                                    Expression.Invoke(
                                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS),
                                        Expression.Constant("observable_parameter_1"),
                                        Expression.Constant("observable_parameter_2"),
                                        Expression.Constant("observable_parameter_3"),
                                        Expression.Constant("observable_parameter_4"),
                                        Expression.Constant("observable_parameter_5"),
                                        Expression.Constant("observable_parameter_6"),
                                        Expression.Constant("observable_parameter_7")
                                    ),
                                    Expression.Lambda(
                                        Expression.Invoke(
                                            Expression.Call(
                                                Expression.Parameter(typeof(TestClientContext), "rx://builtin/this"),
                                                "GetObservable",
                                                new Type[] { typeof(int), typeof(int), typeof(int), typeof(int), typeof(int), typeof(int), typeof(int), typeof(string) },
                                                new Expression[] {
                                                    Expression.New(typeof(Uri).GetConstructor(new[] { typeof(string) }), Expression.Constant(Constants.Observable.YS))
                                                }
                                            ),
                                            x, x, x, x, x, x, x
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
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_Parameterized_Inlined_8P()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observable.XS))("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7", "observable_parameter_8");

                    var zs = from x in xs
                             from y in ctx.GetObservable<int, int, int, int, int, int, int, int, string>(new Uri(Constants.Observable.YS))(x, x, x, x, x, x, x, x)
                             select x + y.Length;

                    var ob = ctx.GetObserver<int>(new Uri(Constants.Observer.OB));
                    zs.SubscribeAsync(ob, new Uri(Constants.Subscription.SUB), state: null).Wait();
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Parameter(typeof(int), "x").Let(x =>
                        Expression.Parameter(typeof(string), "y").Let(y =>
                            Expression.Invoke(
                                Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, IAsyncReactiveQbserver<int>, Task<IAsyncReactiveQubscription>>), Constants.SubscribeUri),
                                Expression.Invoke(
                                    Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, Expression<Func<int, IAsyncReactiveQbservable<string>>>, Expression<Func<int, string, int>>, IAsyncReactiveQbservable<int>>), Constants.Observable.SelectMany),
                                    Expression.Invoke(
                                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS),
                                        Expression.Constant("observable_parameter_1"),
                                        Expression.Constant("observable_parameter_2"),
                                        Expression.Constant("observable_parameter_3"),
                                        Expression.Constant("observable_parameter_4"),
                                        Expression.Constant("observable_parameter_5"),
                                        Expression.Constant("observable_parameter_6"),
                                        Expression.Constant("observable_parameter_7"),
                                        Expression.Constant("observable_parameter_8")
                                    ),
                                    Expression.Lambda(
                                        Expression.Invoke(
                                            Expression.Call(
                                                Expression.Parameter(typeof(TestClientContext), "rx://builtin/this"),
                                                "GetObservable",
                                                new Type[] { typeof(int), typeof(int), typeof(int), typeof(int), typeof(int), typeof(int), typeof(int), typeof(int), typeof(string) },
                                                new Expression[] {
                                                    Expression.New(typeof(Uri).GetConstructor(new[] { typeof(string) }), Expression.Constant(Constants.Observable.YS))
                                                }
                                            ),
                                            x, x, x, x, x, x, x, x
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
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_Parameterized_Inlined_9P()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observable.XS))("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7", "observable_parameter_8", "observable_parameter_9");

                    var zs = from x in xs
                             from y in ctx.GetObservable<int, int, int, int, int, int, int, int, int, string>(new Uri(Constants.Observable.YS))(x, x, x, x, x, x, x, x, x)
                             select x + y.Length;

                    var ob = ctx.GetObserver<int>(new Uri(Constants.Observer.OB));
                    zs.SubscribeAsync(ob, new Uri(Constants.Subscription.SUB), state: null).Wait();
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Parameter(typeof(int), "x").Let(x =>
                        Expression.Parameter(typeof(string), "y").Let(y =>
                            Expression.Invoke(
                                Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, IAsyncReactiveQbserver<int>, Task<IAsyncReactiveQubscription>>), Constants.SubscribeUri),
                                Expression.Invoke(
                                    Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, Expression<Func<int, IAsyncReactiveQbservable<string>>>, Expression<Func<int, string, int>>, IAsyncReactiveQbservable<int>>), Constants.Observable.SelectMany),
                                    Expression.Invoke(
                                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS),
                                        Expression.Constant("observable_parameter_1"),
                                        Expression.Constant("observable_parameter_2"),
                                        Expression.Constant("observable_parameter_3"),
                                        Expression.Constant("observable_parameter_4"),
                                        Expression.Constant("observable_parameter_5"),
                                        Expression.Constant("observable_parameter_6"),
                                        Expression.Constant("observable_parameter_7"),
                                        Expression.Constant("observable_parameter_8"),
                                        Expression.Constant("observable_parameter_9")
                                    ),
                                    Expression.Lambda(
                                        Expression.Invoke(
                                            Expression.Call(
                                                Expression.Parameter(typeof(TestClientContext), "rx://builtin/this"),
                                                "GetObservable",
                                                new Type[] { typeof(int), typeof(int), typeof(int), typeof(int), typeof(int), typeof(int), typeof(int), typeof(int), typeof(int), typeof(string) },
                                                new Expression[] {
                                                    Expression.New(typeof(Uri).GetConstructor(new[] { typeof(string) }), Expression.Constant(Constants.Observable.YS))
                                                }
                                            ),
                                            x, x, x, x, x, x, x, x, x
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
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_Parameterized_Inlined_10P()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observable.XS))("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7", "observable_parameter_8", "observable_parameter_9", "observable_parameter_10");

                    var zs = from x in xs
                             from y in ctx.GetObservable<int, int, int, int, int, int, int, int, int, int, string>(new Uri(Constants.Observable.YS))(x, x, x, x, x, x, x, x, x, x)
                             select x + y.Length;

                    var ob = ctx.GetObserver<int>(new Uri(Constants.Observer.OB));
                    zs.SubscribeAsync(ob, new Uri(Constants.Subscription.SUB), state: null).Wait();
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Parameter(typeof(int), "x").Let(x =>
                        Expression.Parameter(typeof(string), "y").Let(y =>
                            Expression.Invoke(
                                Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, IAsyncReactiveQbserver<int>, Task<IAsyncReactiveQubscription>>), Constants.SubscribeUri),
                                Expression.Invoke(
                                    Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, Expression<Func<int, IAsyncReactiveQbservable<string>>>, Expression<Func<int, string, int>>, IAsyncReactiveQbservable<int>>), Constants.Observable.SelectMany),
                                    Expression.Invoke(
                                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS),
                                        Expression.Constant("observable_parameter_1"),
                                        Expression.Constant("observable_parameter_2"),
                                        Expression.Constant("observable_parameter_3"),
                                        Expression.Constant("observable_parameter_4"),
                                        Expression.Constant("observable_parameter_5"),
                                        Expression.Constant("observable_parameter_6"),
                                        Expression.Constant("observable_parameter_7"),
                                        Expression.Constant("observable_parameter_8"),
                                        Expression.Constant("observable_parameter_9"),
                                        Expression.Constant("observable_parameter_10")
                                    ),
                                    Expression.Lambda(
                                        Expression.Invoke(
                                            Expression.Call(
                                                Expression.Parameter(typeof(TestClientContext), "rx://builtin/this"),
                                                "GetObservable",
                                                new Type[] { typeof(int), typeof(int), typeof(int), typeof(int), typeof(int), typeof(int), typeof(int), typeof(int), typeof(int), typeof(int), typeof(string) },
                                                new Expression[] {
                                                    Expression.New(typeof(Uri).GetConstructor(new[] { typeof(string) }), Expression.Constant(Constants.Observable.YS))
                                                }
                                            ),
                                            x, x, x, x, x, x, x, x, x, x
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
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_Parameterized_Inlined_11P()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<string, string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observable.XS))("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7", "observable_parameter_8", "observable_parameter_9", "observable_parameter_10", "observable_parameter_11");

                    var zs = from x in xs
                             from y in ctx.GetObservable<int, int, int, int, int, int, int, int, int, int, int, string>(new Uri(Constants.Observable.YS))(x, x, x, x, x, x, x, x, x, x, x)
                             select x + y.Length;

                    var ob = ctx.GetObserver<int>(new Uri(Constants.Observer.OB));
                    zs.SubscribeAsync(ob, new Uri(Constants.Subscription.SUB), state: null).Wait();
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Parameter(typeof(int), "x").Let(x =>
                        Expression.Parameter(typeof(string), "y").Let(y =>
                            Expression.Invoke(
                                Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, IAsyncReactiveQbserver<int>, Task<IAsyncReactiveQubscription>>), Constants.SubscribeUri),
                                Expression.Invoke(
                                    Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, Expression<Func<int, IAsyncReactiveQbservable<string>>>, Expression<Func<int, string, int>>, IAsyncReactiveQbservable<int>>), Constants.Observable.SelectMany),
                                    Expression.Invoke(
                                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS),
                                        Expression.Constant("observable_parameter_1"),
                                        Expression.Constant("observable_parameter_2"),
                                        Expression.Constant("observable_parameter_3"),
                                        Expression.Constant("observable_parameter_4"),
                                        Expression.Constant("observable_parameter_5"),
                                        Expression.Constant("observable_parameter_6"),
                                        Expression.Constant("observable_parameter_7"),
                                        Expression.Constant("observable_parameter_8"),
                                        Expression.Constant("observable_parameter_9"),
                                        Expression.Constant("observable_parameter_10"),
                                        Expression.Constant("observable_parameter_11")
                                    ),
                                    Expression.Lambda(
                                        Expression.Invoke(
                                            Expression.Call(
                                                Expression.Parameter(typeof(TestClientContext), "rx://builtin/this"),
                                                "GetObservable",
                                                new Type[] { typeof(int), typeof(int), typeof(int), typeof(int), typeof(int), typeof(int), typeof(int), typeof(int), typeof(int), typeof(int), typeof(int), typeof(string) },
                                                new Expression[] {
                                                    Expression.New(typeof(Uri).GetConstructor(new[] { typeof(string) }), Expression.Constant(Constants.Observable.YS))
                                                }
                                            ),
                                            x, x, x, x, x, x, x, x, x, x, x
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
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_Parameterized_Inlined_12P()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<string, string, string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observable.XS))("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7", "observable_parameter_8", "observable_parameter_9", "observable_parameter_10", "observable_parameter_11", "observable_parameter_12");

                    var zs = from x in xs
                             from y in ctx.GetObservable<int, int, int, int, int, int, int, int, int, int, int, int, string>(new Uri(Constants.Observable.YS))(x, x, x, x, x, x, x, x, x, x, x, x)
                             select x + y.Length;

                    var ob = ctx.GetObserver<int>(new Uri(Constants.Observer.OB));
                    zs.SubscribeAsync(ob, new Uri(Constants.Subscription.SUB), state: null).Wait();
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Parameter(typeof(int), "x").Let(x =>
                        Expression.Parameter(typeof(string), "y").Let(y =>
                            Expression.Invoke(
                                Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, IAsyncReactiveQbserver<int>, Task<IAsyncReactiveQubscription>>), Constants.SubscribeUri),
                                Expression.Invoke(
                                    Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, Expression<Func<int, IAsyncReactiveQbservable<string>>>, Expression<Func<int, string, int>>, IAsyncReactiveQbservable<int>>), Constants.Observable.SelectMany),
                                    Expression.Invoke(
                                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS),
                                        Expression.Constant("observable_parameter_1"),
                                        Expression.Constant("observable_parameter_2"),
                                        Expression.Constant("observable_parameter_3"),
                                        Expression.Constant("observable_parameter_4"),
                                        Expression.Constant("observable_parameter_5"),
                                        Expression.Constant("observable_parameter_6"),
                                        Expression.Constant("observable_parameter_7"),
                                        Expression.Constant("observable_parameter_8"),
                                        Expression.Constant("observable_parameter_9"),
                                        Expression.Constant("observable_parameter_10"),
                                        Expression.Constant("observable_parameter_11"),
                                        Expression.Constant("observable_parameter_12")
                                    ),
                                    Expression.Lambda(
                                        Expression.Invoke(
                                            Expression.Call(
                                                Expression.Parameter(typeof(TestClientContext), "rx://builtin/this"),
                                                "GetObservable",
                                                new Type[] { typeof(int), typeof(int), typeof(int), typeof(int), typeof(int), typeof(int), typeof(int), typeof(int), typeof(int), typeof(int), typeof(int), typeof(int), typeof(string) },
                                                new Expression[] {
                                                    Expression.New(typeof(Uri).GetConstructor(new[] { typeof(string) }), Expression.Constant(Constants.Observable.YS))
                                                }
                                            ),
                                            x, x, x, x, x, x, x, x, x, x, x, x
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
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_Parameterized_Inlined_13P()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<string, string, string, string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observable.XS))("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7", "observable_parameter_8", "observable_parameter_9", "observable_parameter_10", "observable_parameter_11", "observable_parameter_12", "observable_parameter_13");

                    var zs = from x in xs
                             from y in ctx.GetObservable<int, int, int, int, int, int, int, int, int, int, int, int, int, string>(new Uri(Constants.Observable.YS))(x, x, x, x, x, x, x, x, x, x, x, x, x)
                             select x + y.Length;

                    var ob = ctx.GetObserver<int>(new Uri(Constants.Observer.OB));
                    zs.SubscribeAsync(ob, new Uri(Constants.Subscription.SUB), state: null).Wait();
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Parameter(typeof(int), "x").Let(x =>
                        Expression.Parameter(typeof(string), "y").Let(y =>
                            Expression.Invoke(
                                Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, IAsyncReactiveQbserver<int>, Task<IAsyncReactiveQubscription>>), Constants.SubscribeUri),
                                Expression.Invoke(
                                    Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, Expression<Func<int, IAsyncReactiveQbservable<string>>>, Expression<Func<int, string, int>>, IAsyncReactiveQbservable<int>>), Constants.Observable.SelectMany),
                                    Expression.Invoke(
                                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS),
                                        Expression.Constant("observable_parameter_1"),
                                        Expression.Constant("observable_parameter_2"),
                                        Expression.Constant("observable_parameter_3"),
                                        Expression.Constant("observable_parameter_4"),
                                        Expression.Constant("observable_parameter_5"),
                                        Expression.Constant("observable_parameter_6"),
                                        Expression.Constant("observable_parameter_7"),
                                        Expression.Constant("observable_parameter_8"),
                                        Expression.Constant("observable_parameter_9"),
                                        Expression.Constant("observable_parameter_10"),
                                        Expression.Constant("observable_parameter_11"),
                                        Expression.Constant("observable_parameter_12"),
                                        Expression.Constant("observable_parameter_13")
                                    ),
                                    Expression.Lambda(
                                        Expression.Invoke(
                                            Expression.Call(
                                                Expression.Parameter(typeof(TestClientContext), "rx://builtin/this"),
                                                "GetObservable",
                                                new Type[] { typeof(int), typeof(int), typeof(int), typeof(int), typeof(int), typeof(int), typeof(int), typeof(int), typeof(int), typeof(int), typeof(int), typeof(int), typeof(int), typeof(string) },
                                                new Expression[] {
                                                    Expression.New(typeof(Uri).GetConstructor(new[] { typeof(string) }), Expression.Constant(Constants.Observable.YS))
                                                }
                                            ),
                                            x, x, x, x, x, x, x, x, x, x, x, x, x
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
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_Parameterized_Inlined_14P()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<string, string, string, string, string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observable.XS))("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7", "observable_parameter_8", "observable_parameter_9", "observable_parameter_10", "observable_parameter_11", "observable_parameter_12", "observable_parameter_13", "observable_parameter_14");

                    var zs = from x in xs
                             from y in ctx.GetObservable<int, int, int, int, int, int, int, int, int, int, int, int, int, int, string>(new Uri(Constants.Observable.YS))(x, x, x, x, x, x, x, x, x, x, x, x, x, x)
                             select x + y.Length;

                    var ob = ctx.GetObserver<int>(new Uri(Constants.Observer.OB));
                    zs.SubscribeAsync(ob, new Uri(Constants.Subscription.SUB), state: null).Wait();
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Parameter(typeof(int), "x").Let(x =>
                        Expression.Parameter(typeof(string), "y").Let(y =>
                            Expression.Invoke(
                                Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, IAsyncReactiveQbserver<int>, Task<IAsyncReactiveQubscription>>), Constants.SubscribeUri),
                                Expression.Invoke(
                                    Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, Expression<Func<int, IAsyncReactiveQbservable<string>>>, Expression<Func<int, string, int>>, IAsyncReactiveQbservable<int>>), Constants.Observable.SelectMany),
                                    Expression.Invoke(
                                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS),
                                        Expression.Constant("observable_parameter_1"),
                                        Expression.Constant("observable_parameter_2"),
                                        Expression.Constant("observable_parameter_3"),
                                        Expression.Constant("observable_parameter_4"),
                                        Expression.Constant("observable_parameter_5"),
                                        Expression.Constant("observable_parameter_6"),
                                        Expression.Constant("observable_parameter_7"),
                                        Expression.Constant("observable_parameter_8"),
                                        Expression.Constant("observable_parameter_9"),
                                        Expression.Constant("observable_parameter_10"),
                                        Expression.Constant("observable_parameter_11"),
                                        Expression.Constant("observable_parameter_12"),
                                        Expression.Constant("observable_parameter_13"),
                                        Expression.Constant("observable_parameter_14")
                                    ),
                                    Expression.Lambda(
                                        Expression.Invoke(
                                            Expression.Call(
                                                Expression.Parameter(typeof(TestClientContext), "rx://builtin/this"),
                                                "GetObservable",
                                                new Type[] { typeof(int), typeof(int), typeof(int), typeof(int), typeof(int), typeof(int), typeof(int), typeof(int), typeof(int), typeof(int), typeof(int), typeof(int), typeof(int), typeof(int), typeof(string) },
                                                new Expression[] {
                                                    Expression.New(typeof(Uri).GetConstructor(new[] { typeof(string) }), Expression.Constant(Constants.Observable.YS))
                                                }
                                            ),
                                            x, x, x, x, x, x, x, x, x, x, x, x, x, x
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
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_SubscribeAsync_Parameterized_Inlined_15P()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observable.XS))("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7", "observable_parameter_8", "observable_parameter_9", "observable_parameter_10", "observable_parameter_11", "observable_parameter_12", "observable_parameter_13", "observable_parameter_14", "observable_parameter_15");

                    var zs = from x in xs
                             from y in ctx.GetObservable<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, string>(new Uri(Constants.Observable.YS))(x, x, x, x, x, x, x, x, x, x, x, x, x, x, x)
                             select x + y.Length;

                    var ob = ctx.GetObserver<int>(new Uri(Constants.Observer.OB));
                    zs.SubscribeAsync(ob, new Uri(Constants.Subscription.SUB), state: null).Wait();
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Parameter(typeof(int), "x").Let(x =>
                        Expression.Parameter(typeof(string), "y").Let(y =>
                            Expression.Invoke(
                                Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, IAsyncReactiveQbserver<int>, Task<IAsyncReactiveQubscription>>), Constants.SubscribeUri),
                                Expression.Invoke(
                                    Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, Expression<Func<int, IAsyncReactiveQbservable<string>>>, Expression<Func<int, string, int>>, IAsyncReactiveQbservable<int>>), Constants.Observable.SelectMany),
                                    Expression.Invoke(
                                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQbservable<int>>), Constants.Observable.XS),
                                        Expression.Constant("observable_parameter_1"),
                                        Expression.Constant("observable_parameter_2"),
                                        Expression.Constant("observable_parameter_3"),
                                        Expression.Constant("observable_parameter_4"),
                                        Expression.Constant("observable_parameter_5"),
                                        Expression.Constant("observable_parameter_6"),
                                        Expression.Constant("observable_parameter_7"),
                                        Expression.Constant("observable_parameter_8"),
                                        Expression.Constant("observable_parameter_9"),
                                        Expression.Constant("observable_parameter_10"),
                                        Expression.Constant("observable_parameter_11"),
                                        Expression.Constant("observable_parameter_12"),
                                        Expression.Constant("observable_parameter_13"),
                                        Expression.Constant("observable_parameter_14"),
                                        Expression.Constant("observable_parameter_15")
                                    ),
                                    Expression.Lambda(
                                        Expression.Invoke(
                                            Expression.Call(
                                                Expression.Parameter(typeof(TestClientContext), "rx://builtin/this"),
                                                "GetObservable",
                                                new Type[] { typeof(int), typeof(int), typeof(int), typeof(int), typeof(int), typeof(int), typeof(int), typeof(int), typeof(int), typeof(int), typeof(int), typeof(int), typeof(int), typeof(int), typeof(int), typeof(string) },
                                                new Expression[] {
                                                    Expression.New(typeof(Uri).GetConstructor(new[] { typeof(string) }), Expression.Constant(Constants.Observable.YS))
                                                }
                                            ),
                                            x, x, x, x, x, x, x, x, x, x, x, x, x, x, x
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
                    state: null
                )
            );
        }


        #endregion

        #region "ReactiveClientContext_GetStreamFactory"

        [TestMethod]
        public void ReactiveClientContext_GetStreamFactory_2P()
        {
            Apply(
                ctx =>
                {
                    var factory = ctx.GetStreamFactory<string, string, int, int>(new Uri(Constants.StreamFactory.SF));
                    var stream = factory.CreateAsync(new Uri(Constants.Stream.FOO), "factory_parameter_1", "factory_parameter_2", state: null, CancellationToken.None).Result;

                    factory.CreateAsync(new Uri(Constants.Stream.BAR), "factory_parameter_1", "factory_parameter_2", state: null);
                    ((IAsyncReactiveSubjectFactory<int, int, string, string>)factory).CreateAsync(new Uri(Constants.Stream.QUX), "factory_parameter_1", "factory_parameter_2", state: null, CancellationToken.None).Wait();

                    Assert.ThrowsException<ArgumentNullException>(() => factory.CreateAsync(streamUri: null, "factory_parameter_1", "factory_parameter_2", state: null).Wait());
                    Assert.ThrowsException<ArgumentNullException>(() => factory.CreateAsync(streamUri: null, "factory_parameter_1", "factory_parameter_2", state: null, CancellationToken.None).Wait());
                    Assert.ThrowsException<ArgumentNullException>(() => ((IAsyncReactiveSubjectFactory<int, int, string, string>)factory).CreateAsync(null, "factory_parameter_1", "factory_parameter_2", state: null).Wait());
                    Assert.ThrowsException<ArgumentNullException>(() => ((IAsyncReactiveSubjectFactory<int, int, string, string>)factory).CreateAsync(null, "factory_parameter_1", "factory_parameter_2", state: null, CancellationToken.None).Wait());

                    var observer = ctx.GetObserver<int>(new Uri(Constants.Observer.OB));
                    stream.SubscribeAsync(observer, new Uri(Constants.Subscription.SUB), state: null, CancellationToken.None).Wait();
                },
                new CreateStream(
                    new Uri(Constants.Stream.FOO),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<string, string, IAsyncReactiveQubject<int, int>>), Constants.StreamFactory.SF),
                        Expression.Constant("factory_parameter_1"),
                        Expression.Constant("factory_parameter_2")
                    ),
                    state: null
                ),
                new CreateStream(
                    new Uri(Constants.Stream.BAR),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<string, string, IAsyncReactiveQubject<int, int>>), Constants.StreamFactory.SF),
                        Expression.Constant("factory_parameter_1"),
                        Expression.Constant("factory_parameter_2")
                    ),
                    state: null
                ),
                new CreateStream(
                    new Uri(Constants.Stream.QUX),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<string, string, IAsyncReactiveQubject<int, int>>), Constants.StreamFactory.SF),
                        Expression.Constant("factory_parameter_1"),
                        Expression.Constant("factory_parameter_2")
                    ),
                    state: null
                ),
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, IAsyncReactiveQbserver<int>, Task<IAsyncReactiveQubscription>>), Constants.SubscribeUri),
                        Expression.Parameter(typeof(IAsyncReactiveQbservable<int>), Constants.Stream.FOO),
                        Expression.Parameter(typeof(IAsyncReactiveQbserver<int>), Constants.Observer.OB)
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_GetStreamFactory_3P()
        {
            Apply(
                ctx =>
                {
                    var factory = ctx.GetStreamFactory<string, string, string, int, int>(new Uri(Constants.StreamFactory.SF));
                    var stream = factory.CreateAsync(new Uri(Constants.Stream.FOO), "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", state: null, CancellationToken.None).Result;

                    factory.CreateAsync(new Uri(Constants.Stream.BAR), "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", state: null);
                    ((IAsyncReactiveSubjectFactory<int, int, string, string, string>)factory).CreateAsync(new Uri(Constants.Stream.QUX), "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", state: null, CancellationToken.None).Wait();

                    Assert.ThrowsException<ArgumentNullException>(() => factory.CreateAsync(streamUri: null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", state: null).Wait());
                    Assert.ThrowsException<ArgumentNullException>(() => factory.CreateAsync(streamUri: null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", state: null, CancellationToken.None).Wait());
                    Assert.ThrowsException<ArgumentNullException>(() => ((IAsyncReactiveSubjectFactory<int, int, string, string, string>)factory).CreateAsync(null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", state: null).Wait());
                    Assert.ThrowsException<ArgumentNullException>(() => ((IAsyncReactiveSubjectFactory<int, int, string, string, string>)factory).CreateAsync(null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", state: null, CancellationToken.None).Wait());

                    var observer = ctx.GetObserver<int>(new Uri(Constants.Observer.OB));
                    stream.SubscribeAsync(observer, new Uri(Constants.Subscription.SUB), state: null, CancellationToken.None).Wait();
                },
                new CreateStream(
                    new Uri(Constants.Stream.FOO),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<string, string, string, IAsyncReactiveQubject<int, int>>), Constants.StreamFactory.SF),
                        Expression.Constant("factory_parameter_1"),
                        Expression.Constant("factory_parameter_2"),
                        Expression.Constant("factory_parameter_3")
                    ),
                    state: null
                ),
                new CreateStream(
                    new Uri(Constants.Stream.BAR),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<string, string, string, IAsyncReactiveQubject<int, int>>), Constants.StreamFactory.SF),
                        Expression.Constant("factory_parameter_1"),
                        Expression.Constant("factory_parameter_2"),
                        Expression.Constant("factory_parameter_3")
                    ),
                    state: null
                ),
                new CreateStream(
                    new Uri(Constants.Stream.QUX),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<string, string, string, IAsyncReactiveQubject<int, int>>), Constants.StreamFactory.SF),
                        Expression.Constant("factory_parameter_1"),
                        Expression.Constant("factory_parameter_2"),
                        Expression.Constant("factory_parameter_3")
                    ),
                    state: null
                ),
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, IAsyncReactiveQbserver<int>, Task<IAsyncReactiveQubscription>>), Constants.SubscribeUri),
                        Expression.Parameter(typeof(IAsyncReactiveQbservable<int>), Constants.Stream.FOO),
                        Expression.Parameter(typeof(IAsyncReactiveQbserver<int>), Constants.Observer.OB)
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_GetStreamFactory_4P()
        {
            Apply(
                ctx =>
                {
                    var factory = ctx.GetStreamFactory<string, string, string, string, int, int>(new Uri(Constants.StreamFactory.SF));
                    var stream = factory.CreateAsync(new Uri(Constants.Stream.FOO), "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", state: null, CancellationToken.None).Result;

                    factory.CreateAsync(new Uri(Constants.Stream.BAR), "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", state: null);
                    ((IAsyncReactiveSubjectFactory<int, int, string, string, string, string>)factory).CreateAsync(new Uri(Constants.Stream.QUX), "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", state: null, CancellationToken.None).Wait();

                    Assert.ThrowsException<ArgumentNullException>(() => factory.CreateAsync(streamUri: null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", state: null).Wait());
                    Assert.ThrowsException<ArgumentNullException>(() => factory.CreateAsync(streamUri: null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", state: null, CancellationToken.None).Wait());
                    Assert.ThrowsException<ArgumentNullException>(() => ((IAsyncReactiveSubjectFactory<int, int, string, string, string, string>)factory).CreateAsync(null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", state: null).Wait());
                    Assert.ThrowsException<ArgumentNullException>(() => ((IAsyncReactiveSubjectFactory<int, int, string, string, string, string>)factory).CreateAsync(null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", state: null, CancellationToken.None).Wait());

                    var observer = ctx.GetObserver<int>(new Uri(Constants.Observer.OB));
                    stream.SubscribeAsync(observer, new Uri(Constants.Subscription.SUB), state: null, CancellationToken.None).Wait();
                },
                new CreateStream(
                    new Uri(Constants.Stream.FOO),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<string, string, string, string, IAsyncReactiveQubject<int, int>>), Constants.StreamFactory.SF),
                        Expression.Constant("factory_parameter_1"),
                        Expression.Constant("factory_parameter_2"),
                        Expression.Constant("factory_parameter_3"),
                        Expression.Constant("factory_parameter_4")
                    ),
                    state: null
                ),
                new CreateStream(
                    new Uri(Constants.Stream.BAR),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<string, string, string, string, IAsyncReactiveQubject<int, int>>), Constants.StreamFactory.SF),
                        Expression.Constant("factory_parameter_1"),
                        Expression.Constant("factory_parameter_2"),
                        Expression.Constant("factory_parameter_3"),
                        Expression.Constant("factory_parameter_4")
                    ),
                    state: null
                ),
                new CreateStream(
                    new Uri(Constants.Stream.QUX),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<string, string, string, string, IAsyncReactiveQubject<int, int>>), Constants.StreamFactory.SF),
                        Expression.Constant("factory_parameter_1"),
                        Expression.Constant("factory_parameter_2"),
                        Expression.Constant("factory_parameter_3"),
                        Expression.Constant("factory_parameter_4")
                    ),
                    state: null
                ),
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, IAsyncReactiveQbserver<int>, Task<IAsyncReactiveQubscription>>), Constants.SubscribeUri),
                        Expression.Parameter(typeof(IAsyncReactiveQbservable<int>), Constants.Stream.FOO),
                        Expression.Parameter(typeof(IAsyncReactiveQbserver<int>), Constants.Observer.OB)
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_GetStreamFactory_5P()
        {
            Apply(
                ctx =>
                {
                    var factory = ctx.GetStreamFactory<string, string, string, string, string, int, int>(new Uri(Constants.StreamFactory.SF));
                    var stream = factory.CreateAsync(new Uri(Constants.Stream.FOO), "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", state: null, CancellationToken.None).Result;

                    factory.CreateAsync(new Uri(Constants.Stream.BAR), "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", state: null);
                    ((IAsyncReactiveSubjectFactory<int, int, string, string, string, string, string>)factory).CreateAsync(new Uri(Constants.Stream.QUX), "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", state: null, CancellationToken.None).Wait();

                    Assert.ThrowsException<ArgumentNullException>(() => factory.CreateAsync(streamUri: null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", state: null).Wait());
                    Assert.ThrowsException<ArgumentNullException>(() => factory.CreateAsync(streamUri: null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", state: null, CancellationToken.None).Wait());
                    Assert.ThrowsException<ArgumentNullException>(() => ((IAsyncReactiveSubjectFactory<int, int, string, string, string, string, string>)factory).CreateAsync(null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", state: null).Wait());
                    Assert.ThrowsException<ArgumentNullException>(() => ((IAsyncReactiveSubjectFactory<int, int, string, string, string, string, string>)factory).CreateAsync(null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", state: null, CancellationToken.None).Wait());

                    var observer = ctx.GetObserver<int>(new Uri(Constants.Observer.OB));
                    stream.SubscribeAsync(observer, new Uri(Constants.Subscription.SUB), state: null, CancellationToken.None).Wait();
                },
                new CreateStream(
                    new Uri(Constants.Stream.FOO),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<string, string, string, string, string, IAsyncReactiveQubject<int, int>>), Constants.StreamFactory.SF),
                        Expression.Constant("factory_parameter_1"),
                        Expression.Constant("factory_parameter_2"),
                        Expression.Constant("factory_parameter_3"),
                        Expression.Constant("factory_parameter_4"),
                        Expression.Constant("factory_parameter_5")
                    ),
                    state: null
                ),
                new CreateStream(
                    new Uri(Constants.Stream.BAR),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<string, string, string, string, string, IAsyncReactiveQubject<int, int>>), Constants.StreamFactory.SF),
                        Expression.Constant("factory_parameter_1"),
                        Expression.Constant("factory_parameter_2"),
                        Expression.Constant("factory_parameter_3"),
                        Expression.Constant("factory_parameter_4"),
                        Expression.Constant("factory_parameter_5")
                    ),
                    state: null
                ),
                new CreateStream(
                    new Uri(Constants.Stream.QUX),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<string, string, string, string, string, IAsyncReactiveQubject<int, int>>), Constants.StreamFactory.SF),
                        Expression.Constant("factory_parameter_1"),
                        Expression.Constant("factory_parameter_2"),
                        Expression.Constant("factory_parameter_3"),
                        Expression.Constant("factory_parameter_4"),
                        Expression.Constant("factory_parameter_5")
                    ),
                    state: null
                ),
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, IAsyncReactiveQbserver<int>, Task<IAsyncReactiveQubscription>>), Constants.SubscribeUri),
                        Expression.Parameter(typeof(IAsyncReactiveQbservable<int>), Constants.Stream.FOO),
                        Expression.Parameter(typeof(IAsyncReactiveQbserver<int>), Constants.Observer.OB)
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_GetStreamFactory_6P()
        {
            Apply(
                ctx =>
                {
                    var factory = ctx.GetStreamFactory<string, string, string, string, string, string, int, int>(new Uri(Constants.StreamFactory.SF));
                    var stream = factory.CreateAsync(new Uri(Constants.Stream.FOO), "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", state: null, CancellationToken.None).Result;

                    factory.CreateAsync(new Uri(Constants.Stream.BAR), "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", state: null);
                    ((IAsyncReactiveSubjectFactory<int, int, string, string, string, string, string, string>)factory).CreateAsync(new Uri(Constants.Stream.QUX), "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", state: null, CancellationToken.None).Wait();

                    Assert.ThrowsException<ArgumentNullException>(() => factory.CreateAsync(streamUri: null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", state: null).Wait());
                    Assert.ThrowsException<ArgumentNullException>(() => factory.CreateAsync(streamUri: null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", state: null, CancellationToken.None).Wait());
                    Assert.ThrowsException<ArgumentNullException>(() => ((IAsyncReactiveSubjectFactory<int, int, string, string, string, string, string, string>)factory).CreateAsync(null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", state: null).Wait());
                    Assert.ThrowsException<ArgumentNullException>(() => ((IAsyncReactiveSubjectFactory<int, int, string, string, string, string, string, string>)factory).CreateAsync(null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", state: null, CancellationToken.None).Wait());

                    var observer = ctx.GetObserver<int>(new Uri(Constants.Observer.OB));
                    stream.SubscribeAsync(observer, new Uri(Constants.Subscription.SUB), state: null, CancellationToken.None).Wait();
                },
                new CreateStream(
                    new Uri(Constants.Stream.FOO),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, IAsyncReactiveQubject<int, int>>), Constants.StreamFactory.SF),
                        Expression.Constant("factory_parameter_1"),
                        Expression.Constant("factory_parameter_2"),
                        Expression.Constant("factory_parameter_3"),
                        Expression.Constant("factory_parameter_4"),
                        Expression.Constant("factory_parameter_5"),
                        Expression.Constant("factory_parameter_6")
                    ),
                    state: null
                ),
                new CreateStream(
                    new Uri(Constants.Stream.BAR),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, IAsyncReactiveQubject<int, int>>), Constants.StreamFactory.SF),
                        Expression.Constant("factory_parameter_1"),
                        Expression.Constant("factory_parameter_2"),
                        Expression.Constant("factory_parameter_3"),
                        Expression.Constant("factory_parameter_4"),
                        Expression.Constant("factory_parameter_5"),
                        Expression.Constant("factory_parameter_6")
                    ),
                    state: null
                ),
                new CreateStream(
                    new Uri(Constants.Stream.QUX),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, IAsyncReactiveQubject<int, int>>), Constants.StreamFactory.SF),
                        Expression.Constant("factory_parameter_1"),
                        Expression.Constant("factory_parameter_2"),
                        Expression.Constant("factory_parameter_3"),
                        Expression.Constant("factory_parameter_4"),
                        Expression.Constant("factory_parameter_5"),
                        Expression.Constant("factory_parameter_6")
                    ),
                    state: null
                ),
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, IAsyncReactiveQbserver<int>, Task<IAsyncReactiveQubscription>>), Constants.SubscribeUri),
                        Expression.Parameter(typeof(IAsyncReactiveQbservable<int>), Constants.Stream.FOO),
                        Expression.Parameter(typeof(IAsyncReactiveQbserver<int>), Constants.Observer.OB)
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_GetStreamFactory_7P()
        {
            Apply(
                ctx =>
                {
                    var factory = ctx.GetStreamFactory<string, string, string, string, string, string, string, int, int>(new Uri(Constants.StreamFactory.SF));
                    var stream = factory.CreateAsync(new Uri(Constants.Stream.FOO), "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", state: null, CancellationToken.None).Result;

                    factory.CreateAsync(new Uri(Constants.Stream.BAR), "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", state: null);
                    ((IAsyncReactiveSubjectFactory<int, int, string, string, string, string, string, string, string>)factory).CreateAsync(new Uri(Constants.Stream.QUX), "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", state: null, CancellationToken.None).Wait();

                    Assert.ThrowsException<ArgumentNullException>(() => factory.CreateAsync(streamUri: null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", state: null).Wait());
                    Assert.ThrowsException<ArgumentNullException>(() => factory.CreateAsync(streamUri: null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", state: null, CancellationToken.None).Wait());
                    Assert.ThrowsException<ArgumentNullException>(() => ((IAsyncReactiveSubjectFactory<int, int, string, string, string, string, string, string, string>)factory).CreateAsync(null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", state: null).Wait());
                    Assert.ThrowsException<ArgumentNullException>(() => ((IAsyncReactiveSubjectFactory<int, int, string, string, string, string, string, string, string>)factory).CreateAsync(null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", state: null, CancellationToken.None).Wait());

                    var observer = ctx.GetObserver<int>(new Uri(Constants.Observer.OB));
                    stream.SubscribeAsync(observer, new Uri(Constants.Subscription.SUB), state: null, CancellationToken.None).Wait();
                },
                new CreateStream(
                    new Uri(Constants.Stream.FOO),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, IAsyncReactiveQubject<int, int>>), Constants.StreamFactory.SF),
                        Expression.Constant("factory_parameter_1"),
                        Expression.Constant("factory_parameter_2"),
                        Expression.Constant("factory_parameter_3"),
                        Expression.Constant("factory_parameter_4"),
                        Expression.Constant("factory_parameter_5"),
                        Expression.Constant("factory_parameter_6"),
                        Expression.Constant("factory_parameter_7")
                    ),
                    state: null
                ),
                new CreateStream(
                    new Uri(Constants.Stream.BAR),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, IAsyncReactiveQubject<int, int>>), Constants.StreamFactory.SF),
                        Expression.Constant("factory_parameter_1"),
                        Expression.Constant("factory_parameter_2"),
                        Expression.Constant("factory_parameter_3"),
                        Expression.Constant("factory_parameter_4"),
                        Expression.Constant("factory_parameter_5"),
                        Expression.Constant("factory_parameter_6"),
                        Expression.Constant("factory_parameter_7")
                    ),
                    state: null
                ),
                new CreateStream(
                    new Uri(Constants.Stream.QUX),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, IAsyncReactiveQubject<int, int>>), Constants.StreamFactory.SF),
                        Expression.Constant("factory_parameter_1"),
                        Expression.Constant("factory_parameter_2"),
                        Expression.Constant("factory_parameter_3"),
                        Expression.Constant("factory_parameter_4"),
                        Expression.Constant("factory_parameter_5"),
                        Expression.Constant("factory_parameter_6"),
                        Expression.Constant("factory_parameter_7")
                    ),
                    state: null
                ),
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, IAsyncReactiveQbserver<int>, Task<IAsyncReactiveQubscription>>), Constants.SubscribeUri),
                        Expression.Parameter(typeof(IAsyncReactiveQbservable<int>), Constants.Stream.FOO),
                        Expression.Parameter(typeof(IAsyncReactiveQbserver<int>), Constants.Observer.OB)
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_GetStreamFactory_8P()
        {
            Apply(
                ctx =>
                {
                    var factory = ctx.GetStreamFactory<string, string, string, string, string, string, string, string, int, int>(new Uri(Constants.StreamFactory.SF));
                    var stream = factory.CreateAsync(new Uri(Constants.Stream.FOO), "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", state: null, CancellationToken.None).Result;

                    factory.CreateAsync(new Uri(Constants.Stream.BAR), "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", state: null);
                    ((IAsyncReactiveSubjectFactory<int, int, string, string, string, string, string, string, string, string>)factory).CreateAsync(new Uri(Constants.Stream.QUX), "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", state: null, CancellationToken.None).Wait();

                    Assert.ThrowsException<ArgumentNullException>(() => factory.CreateAsync(streamUri: null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", state: null).Wait());
                    Assert.ThrowsException<ArgumentNullException>(() => factory.CreateAsync(streamUri: null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", state: null, CancellationToken.None).Wait());
                    Assert.ThrowsException<ArgumentNullException>(() => ((IAsyncReactiveSubjectFactory<int, int, string, string, string, string, string, string, string, string>)factory).CreateAsync(null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", state: null).Wait());
                    Assert.ThrowsException<ArgumentNullException>(() => ((IAsyncReactiveSubjectFactory<int, int, string, string, string, string, string, string, string, string>)factory).CreateAsync(null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", state: null, CancellationToken.None).Wait());

                    var observer = ctx.GetObserver<int>(new Uri(Constants.Observer.OB));
                    stream.SubscribeAsync(observer, new Uri(Constants.Subscription.SUB), state: null, CancellationToken.None).Wait();
                },
                new CreateStream(
                    new Uri(Constants.Stream.FOO),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, IAsyncReactiveQubject<int, int>>), Constants.StreamFactory.SF),
                        Expression.Constant("factory_parameter_1"),
                        Expression.Constant("factory_parameter_2"),
                        Expression.Constant("factory_parameter_3"),
                        Expression.Constant("factory_parameter_4"),
                        Expression.Constant("factory_parameter_5"),
                        Expression.Constant("factory_parameter_6"),
                        Expression.Constant("factory_parameter_7"),
                        Expression.Constant("factory_parameter_8")
                    ),
                    state: null
                ),
                new CreateStream(
                    new Uri(Constants.Stream.BAR),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, IAsyncReactiveQubject<int, int>>), Constants.StreamFactory.SF),
                        Expression.Constant("factory_parameter_1"),
                        Expression.Constant("factory_parameter_2"),
                        Expression.Constant("factory_parameter_3"),
                        Expression.Constant("factory_parameter_4"),
                        Expression.Constant("factory_parameter_5"),
                        Expression.Constant("factory_parameter_6"),
                        Expression.Constant("factory_parameter_7"),
                        Expression.Constant("factory_parameter_8")
                    ),
                    state: null
                ),
                new CreateStream(
                    new Uri(Constants.Stream.QUX),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, IAsyncReactiveQubject<int, int>>), Constants.StreamFactory.SF),
                        Expression.Constant("factory_parameter_1"),
                        Expression.Constant("factory_parameter_2"),
                        Expression.Constant("factory_parameter_3"),
                        Expression.Constant("factory_parameter_4"),
                        Expression.Constant("factory_parameter_5"),
                        Expression.Constant("factory_parameter_6"),
                        Expression.Constant("factory_parameter_7"),
                        Expression.Constant("factory_parameter_8")
                    ),
                    state: null
                ),
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, IAsyncReactiveQbserver<int>, Task<IAsyncReactiveQubscription>>), Constants.SubscribeUri),
                        Expression.Parameter(typeof(IAsyncReactiveQbservable<int>), Constants.Stream.FOO),
                        Expression.Parameter(typeof(IAsyncReactiveQbserver<int>), Constants.Observer.OB)
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_GetStreamFactory_9P()
        {
            Apply(
                ctx =>
                {
                    var factory = ctx.GetStreamFactory<string, string, string, string, string, string, string, string, string, int, int>(new Uri(Constants.StreamFactory.SF));
                    var stream = factory.CreateAsync(new Uri(Constants.Stream.FOO), "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", state: null, CancellationToken.None).Result;

                    factory.CreateAsync(new Uri(Constants.Stream.BAR), "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", state: null);
                    ((IAsyncReactiveSubjectFactory<int, int, string, string, string, string, string, string, string, string, string>)factory).CreateAsync(new Uri(Constants.Stream.QUX), "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", state: null, CancellationToken.None).Wait();

                    Assert.ThrowsException<ArgumentNullException>(() => factory.CreateAsync(streamUri: null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", state: null).Wait());
                    Assert.ThrowsException<ArgumentNullException>(() => factory.CreateAsync(streamUri: null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", state: null, CancellationToken.None).Wait());
                    Assert.ThrowsException<ArgumentNullException>(() => ((IAsyncReactiveSubjectFactory<int, int, string, string, string, string, string, string, string, string, string>)factory).CreateAsync(null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", state: null).Wait());
                    Assert.ThrowsException<ArgumentNullException>(() => ((IAsyncReactiveSubjectFactory<int, int, string, string, string, string, string, string, string, string, string>)factory).CreateAsync(null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", state: null, CancellationToken.None).Wait());

                    var observer = ctx.GetObserver<int>(new Uri(Constants.Observer.OB));
                    stream.SubscribeAsync(observer, new Uri(Constants.Subscription.SUB), state: null, CancellationToken.None).Wait();
                },
                new CreateStream(
                    new Uri(Constants.Stream.FOO),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, IAsyncReactiveQubject<int, int>>), Constants.StreamFactory.SF),
                        Expression.Constant("factory_parameter_1"),
                        Expression.Constant("factory_parameter_2"),
                        Expression.Constant("factory_parameter_3"),
                        Expression.Constant("factory_parameter_4"),
                        Expression.Constant("factory_parameter_5"),
                        Expression.Constant("factory_parameter_6"),
                        Expression.Constant("factory_parameter_7"),
                        Expression.Constant("factory_parameter_8"),
                        Expression.Constant("factory_parameter_9")
                    ),
                    state: null
                ),
                new CreateStream(
                    new Uri(Constants.Stream.BAR),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, IAsyncReactiveQubject<int, int>>), Constants.StreamFactory.SF),
                        Expression.Constant("factory_parameter_1"),
                        Expression.Constant("factory_parameter_2"),
                        Expression.Constant("factory_parameter_3"),
                        Expression.Constant("factory_parameter_4"),
                        Expression.Constant("factory_parameter_5"),
                        Expression.Constant("factory_parameter_6"),
                        Expression.Constant("factory_parameter_7"),
                        Expression.Constant("factory_parameter_8"),
                        Expression.Constant("factory_parameter_9")
                    ),
                    state: null
                ),
                new CreateStream(
                    new Uri(Constants.Stream.QUX),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, IAsyncReactiveQubject<int, int>>), Constants.StreamFactory.SF),
                        Expression.Constant("factory_parameter_1"),
                        Expression.Constant("factory_parameter_2"),
                        Expression.Constant("factory_parameter_3"),
                        Expression.Constant("factory_parameter_4"),
                        Expression.Constant("factory_parameter_5"),
                        Expression.Constant("factory_parameter_6"),
                        Expression.Constant("factory_parameter_7"),
                        Expression.Constant("factory_parameter_8"),
                        Expression.Constant("factory_parameter_9")
                    ),
                    state: null
                ),
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, IAsyncReactiveQbserver<int>, Task<IAsyncReactiveQubscription>>), Constants.SubscribeUri),
                        Expression.Parameter(typeof(IAsyncReactiveQbservable<int>), Constants.Stream.FOO),
                        Expression.Parameter(typeof(IAsyncReactiveQbserver<int>), Constants.Observer.OB)
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_GetStreamFactory_10P()
        {
            Apply(
                ctx =>
                {
                    var factory = ctx.GetStreamFactory<string, string, string, string, string, string, string, string, string, string, int, int>(new Uri(Constants.StreamFactory.SF));
                    var stream = factory.CreateAsync(new Uri(Constants.Stream.FOO), "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", state: null, CancellationToken.None).Result;

                    factory.CreateAsync(new Uri(Constants.Stream.BAR), "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", state: null);
                    ((IAsyncReactiveSubjectFactory<int, int, string, string, string, string, string, string, string, string, string, string>)factory).CreateAsync(new Uri(Constants.Stream.QUX), "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", state: null, CancellationToken.None).Wait();

                    Assert.ThrowsException<ArgumentNullException>(() => factory.CreateAsync(streamUri: null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", state: null).Wait());
                    Assert.ThrowsException<ArgumentNullException>(() => factory.CreateAsync(streamUri: null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", state: null, CancellationToken.None).Wait());
                    Assert.ThrowsException<ArgumentNullException>(() => ((IAsyncReactiveSubjectFactory<int, int, string, string, string, string, string, string, string, string, string, string>)factory).CreateAsync(null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", state: null).Wait());
                    Assert.ThrowsException<ArgumentNullException>(() => ((IAsyncReactiveSubjectFactory<int, int, string, string, string, string, string, string, string, string, string, string>)factory).CreateAsync(null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", state: null, CancellationToken.None).Wait());

                    var observer = ctx.GetObserver<int>(new Uri(Constants.Observer.OB));
                    stream.SubscribeAsync(observer, new Uri(Constants.Subscription.SUB), state: null, CancellationToken.None).Wait();
                },
                new CreateStream(
                    new Uri(Constants.Stream.FOO),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQubject<int, int>>), Constants.StreamFactory.SF),
                        Expression.Constant("factory_parameter_1"),
                        Expression.Constant("factory_parameter_2"),
                        Expression.Constant("factory_parameter_3"),
                        Expression.Constant("factory_parameter_4"),
                        Expression.Constant("factory_parameter_5"),
                        Expression.Constant("factory_parameter_6"),
                        Expression.Constant("factory_parameter_7"),
                        Expression.Constant("factory_parameter_8"),
                        Expression.Constant("factory_parameter_9"),
                        Expression.Constant("factory_parameter_10")
                    ),
                    state: null
                ),
                new CreateStream(
                    new Uri(Constants.Stream.BAR),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQubject<int, int>>), Constants.StreamFactory.SF),
                        Expression.Constant("factory_parameter_1"),
                        Expression.Constant("factory_parameter_2"),
                        Expression.Constant("factory_parameter_3"),
                        Expression.Constant("factory_parameter_4"),
                        Expression.Constant("factory_parameter_5"),
                        Expression.Constant("factory_parameter_6"),
                        Expression.Constant("factory_parameter_7"),
                        Expression.Constant("factory_parameter_8"),
                        Expression.Constant("factory_parameter_9"),
                        Expression.Constant("factory_parameter_10")
                    ),
                    state: null
                ),
                new CreateStream(
                    new Uri(Constants.Stream.QUX),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQubject<int, int>>), Constants.StreamFactory.SF),
                        Expression.Constant("factory_parameter_1"),
                        Expression.Constant("factory_parameter_2"),
                        Expression.Constant("factory_parameter_3"),
                        Expression.Constant("factory_parameter_4"),
                        Expression.Constant("factory_parameter_5"),
                        Expression.Constant("factory_parameter_6"),
                        Expression.Constant("factory_parameter_7"),
                        Expression.Constant("factory_parameter_8"),
                        Expression.Constant("factory_parameter_9"),
                        Expression.Constant("factory_parameter_10")
                    ),
                    state: null
                ),
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, IAsyncReactiveQbserver<int>, Task<IAsyncReactiveQubscription>>), Constants.SubscribeUri),
                        Expression.Parameter(typeof(IAsyncReactiveQbservable<int>), Constants.Stream.FOO),
                        Expression.Parameter(typeof(IAsyncReactiveQbserver<int>), Constants.Observer.OB)
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_GetStreamFactory_11P()
        {
            Apply(
                ctx =>
                {
                    var factory = ctx.GetStreamFactory<string, string, string, string, string, string, string, string, string, string, string, int, int>(new Uri(Constants.StreamFactory.SF));
                    var stream = factory.CreateAsync(new Uri(Constants.Stream.FOO), "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", "factory_parameter_11", state: null, CancellationToken.None).Result;

                    factory.CreateAsync(new Uri(Constants.Stream.BAR), "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", "factory_parameter_11", state: null);
                    ((IAsyncReactiveSubjectFactory<int, int, string, string, string, string, string, string, string, string, string, string, string>)factory).CreateAsync(new Uri(Constants.Stream.QUX), "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", "factory_parameter_11", state: null, CancellationToken.None).Wait();

                    Assert.ThrowsException<ArgumentNullException>(() => factory.CreateAsync(streamUri: null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", "factory_parameter_11", state: null).Wait());
                    Assert.ThrowsException<ArgumentNullException>(() => factory.CreateAsync(streamUri: null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", "factory_parameter_11", state: null, CancellationToken.None).Wait());
                    Assert.ThrowsException<ArgumentNullException>(() => ((IAsyncReactiveSubjectFactory<int, int, string, string, string, string, string, string, string, string, string, string, string>)factory).CreateAsync(null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", "factory_parameter_11", state: null).Wait());
                    Assert.ThrowsException<ArgumentNullException>(() => ((IAsyncReactiveSubjectFactory<int, int, string, string, string, string, string, string, string, string, string, string, string>)factory).CreateAsync(null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", "factory_parameter_11", state: null, CancellationToken.None).Wait());

                    var observer = ctx.GetObserver<int>(new Uri(Constants.Observer.OB));
                    stream.SubscribeAsync(observer, new Uri(Constants.Subscription.SUB), state: null, CancellationToken.None).Wait();
                },
                new CreateStream(
                    new Uri(Constants.Stream.FOO),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQubject<int, int>>), Constants.StreamFactory.SF),
                        Expression.Constant("factory_parameter_1"),
                        Expression.Constant("factory_parameter_2"),
                        Expression.Constant("factory_parameter_3"),
                        Expression.Constant("factory_parameter_4"),
                        Expression.Constant("factory_parameter_5"),
                        Expression.Constant("factory_parameter_6"),
                        Expression.Constant("factory_parameter_7"),
                        Expression.Constant("factory_parameter_8"),
                        Expression.Constant("factory_parameter_9"),
                        Expression.Constant("factory_parameter_10"),
                        Expression.Constant("factory_parameter_11")
                    ),
                    state: null
                ),
                new CreateStream(
                    new Uri(Constants.Stream.BAR),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQubject<int, int>>), Constants.StreamFactory.SF),
                        Expression.Constant("factory_parameter_1"),
                        Expression.Constant("factory_parameter_2"),
                        Expression.Constant("factory_parameter_3"),
                        Expression.Constant("factory_parameter_4"),
                        Expression.Constant("factory_parameter_5"),
                        Expression.Constant("factory_parameter_6"),
                        Expression.Constant("factory_parameter_7"),
                        Expression.Constant("factory_parameter_8"),
                        Expression.Constant("factory_parameter_9"),
                        Expression.Constant("factory_parameter_10"),
                        Expression.Constant("factory_parameter_11")
                    ),
                    state: null
                ),
                new CreateStream(
                    new Uri(Constants.Stream.QUX),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQubject<int, int>>), Constants.StreamFactory.SF),
                        Expression.Constant("factory_parameter_1"),
                        Expression.Constant("factory_parameter_2"),
                        Expression.Constant("factory_parameter_3"),
                        Expression.Constant("factory_parameter_4"),
                        Expression.Constant("factory_parameter_5"),
                        Expression.Constant("factory_parameter_6"),
                        Expression.Constant("factory_parameter_7"),
                        Expression.Constant("factory_parameter_8"),
                        Expression.Constant("factory_parameter_9"),
                        Expression.Constant("factory_parameter_10"),
                        Expression.Constant("factory_parameter_11")
                    ),
                    state: null
                ),
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, IAsyncReactiveQbserver<int>, Task<IAsyncReactiveQubscription>>), Constants.SubscribeUri),
                        Expression.Parameter(typeof(IAsyncReactiveQbservable<int>), Constants.Stream.FOO),
                        Expression.Parameter(typeof(IAsyncReactiveQbserver<int>), Constants.Observer.OB)
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_GetStreamFactory_12P()
        {
            Apply(
                ctx =>
                {
                    var factory = ctx.GetStreamFactory<string, string, string, string, string, string, string, string, string, string, string, string, int, int>(new Uri(Constants.StreamFactory.SF));
                    var stream = factory.CreateAsync(new Uri(Constants.Stream.FOO), "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", "factory_parameter_11", "factory_parameter_12", state: null, CancellationToken.None).Result;

                    factory.CreateAsync(new Uri(Constants.Stream.BAR), "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", "factory_parameter_11", "factory_parameter_12", state: null);
                    ((IAsyncReactiveSubjectFactory<int, int, string, string, string, string, string, string, string, string, string, string, string, string>)factory).CreateAsync(new Uri(Constants.Stream.QUX), "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", "factory_parameter_11", "factory_parameter_12", state: null, CancellationToken.None).Wait();

                    Assert.ThrowsException<ArgumentNullException>(() => factory.CreateAsync(streamUri: null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", "factory_parameter_11", "factory_parameter_12", state: null).Wait());
                    Assert.ThrowsException<ArgumentNullException>(() => factory.CreateAsync(streamUri: null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", "factory_parameter_11", "factory_parameter_12", state: null, CancellationToken.None).Wait());
                    Assert.ThrowsException<ArgumentNullException>(() => ((IAsyncReactiveSubjectFactory<int, int, string, string, string, string, string, string, string, string, string, string, string, string>)factory).CreateAsync(null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", "factory_parameter_11", "factory_parameter_12", state: null).Wait());
                    Assert.ThrowsException<ArgumentNullException>(() => ((IAsyncReactiveSubjectFactory<int, int, string, string, string, string, string, string, string, string, string, string, string, string>)factory).CreateAsync(null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", "factory_parameter_11", "factory_parameter_12", state: null, CancellationToken.None).Wait());

                    var observer = ctx.GetObserver<int>(new Uri(Constants.Observer.OB));
                    stream.SubscribeAsync(observer, new Uri(Constants.Subscription.SUB), state: null, CancellationToken.None).Wait();
                },
                new CreateStream(
                    new Uri(Constants.Stream.FOO),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQubject<int, int>>), Constants.StreamFactory.SF),
                        Expression.Constant("factory_parameter_1"),
                        Expression.Constant("factory_parameter_2"),
                        Expression.Constant("factory_parameter_3"),
                        Expression.Constant("factory_parameter_4"),
                        Expression.Constant("factory_parameter_5"),
                        Expression.Constant("factory_parameter_6"),
                        Expression.Constant("factory_parameter_7"),
                        Expression.Constant("factory_parameter_8"),
                        Expression.Constant("factory_parameter_9"),
                        Expression.Constant("factory_parameter_10"),
                        Expression.Constant("factory_parameter_11"),
                        Expression.Constant("factory_parameter_12")
                    ),
                    state: null
                ),
                new CreateStream(
                    new Uri(Constants.Stream.BAR),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQubject<int, int>>), Constants.StreamFactory.SF),
                        Expression.Constant("factory_parameter_1"),
                        Expression.Constant("factory_parameter_2"),
                        Expression.Constant("factory_parameter_3"),
                        Expression.Constant("factory_parameter_4"),
                        Expression.Constant("factory_parameter_5"),
                        Expression.Constant("factory_parameter_6"),
                        Expression.Constant("factory_parameter_7"),
                        Expression.Constant("factory_parameter_8"),
                        Expression.Constant("factory_parameter_9"),
                        Expression.Constant("factory_parameter_10"),
                        Expression.Constant("factory_parameter_11"),
                        Expression.Constant("factory_parameter_12")
                    ),
                    state: null
                ),
                new CreateStream(
                    new Uri(Constants.Stream.QUX),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQubject<int, int>>), Constants.StreamFactory.SF),
                        Expression.Constant("factory_parameter_1"),
                        Expression.Constant("factory_parameter_2"),
                        Expression.Constant("factory_parameter_3"),
                        Expression.Constant("factory_parameter_4"),
                        Expression.Constant("factory_parameter_5"),
                        Expression.Constant("factory_parameter_6"),
                        Expression.Constant("factory_parameter_7"),
                        Expression.Constant("factory_parameter_8"),
                        Expression.Constant("factory_parameter_9"),
                        Expression.Constant("factory_parameter_10"),
                        Expression.Constant("factory_parameter_11"),
                        Expression.Constant("factory_parameter_12")
                    ),
                    state: null
                ),
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, IAsyncReactiveQbserver<int>, Task<IAsyncReactiveQubscription>>), Constants.SubscribeUri),
                        Expression.Parameter(typeof(IAsyncReactiveQbservable<int>), Constants.Stream.FOO),
                        Expression.Parameter(typeof(IAsyncReactiveQbserver<int>), Constants.Observer.OB)
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_GetStreamFactory_13P()
        {
            Apply(
                ctx =>
                {
                    var factory = ctx.GetStreamFactory<string, string, string, string, string, string, string, string, string, string, string, string, string, int, int>(new Uri(Constants.StreamFactory.SF));
                    var stream = factory.CreateAsync(new Uri(Constants.Stream.FOO), "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", "factory_parameter_11", "factory_parameter_12", "factory_parameter_13", state: null, CancellationToken.None).Result;

                    factory.CreateAsync(new Uri(Constants.Stream.BAR), "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", "factory_parameter_11", "factory_parameter_12", "factory_parameter_13", state: null);
                    ((IAsyncReactiveSubjectFactory<int, int, string, string, string, string, string, string, string, string, string, string, string, string, string>)factory).CreateAsync(new Uri(Constants.Stream.QUX), "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", "factory_parameter_11", "factory_parameter_12", "factory_parameter_13", state: null, CancellationToken.None).Wait();

                    Assert.ThrowsException<ArgumentNullException>(() => factory.CreateAsync(streamUri: null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", "factory_parameter_11", "factory_parameter_12", "factory_parameter_13", state: null).Wait());
                    Assert.ThrowsException<ArgumentNullException>(() => factory.CreateAsync(streamUri: null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", "factory_parameter_11", "factory_parameter_12", "factory_parameter_13", state: null, CancellationToken.None).Wait());
                    Assert.ThrowsException<ArgumentNullException>(() => ((IAsyncReactiveSubjectFactory<int, int, string, string, string, string, string, string, string, string, string, string, string, string, string>)factory).CreateAsync(null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", "factory_parameter_11", "factory_parameter_12", "factory_parameter_13", state: null).Wait());
                    Assert.ThrowsException<ArgumentNullException>(() => ((IAsyncReactiveSubjectFactory<int, int, string, string, string, string, string, string, string, string, string, string, string, string, string>)factory).CreateAsync(null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", "factory_parameter_11", "factory_parameter_12", "factory_parameter_13", state: null, CancellationToken.None).Wait());

                    var observer = ctx.GetObserver<int>(new Uri(Constants.Observer.OB));
                    stream.SubscribeAsync(observer, new Uri(Constants.Subscription.SUB), state: null, CancellationToken.None).Wait();
                },
                new CreateStream(
                    new Uri(Constants.Stream.FOO),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQubject<int, int>>), Constants.StreamFactory.SF),
                        Expression.Constant("factory_parameter_1"),
                        Expression.Constant("factory_parameter_2"),
                        Expression.Constant("factory_parameter_3"),
                        Expression.Constant("factory_parameter_4"),
                        Expression.Constant("factory_parameter_5"),
                        Expression.Constant("factory_parameter_6"),
                        Expression.Constant("factory_parameter_7"),
                        Expression.Constant("factory_parameter_8"),
                        Expression.Constant("factory_parameter_9"),
                        Expression.Constant("factory_parameter_10"),
                        Expression.Constant("factory_parameter_11"),
                        Expression.Constant("factory_parameter_12"),
                        Expression.Constant("factory_parameter_13")
                    ),
                    state: null
                ),
                new CreateStream(
                    new Uri(Constants.Stream.BAR),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQubject<int, int>>), Constants.StreamFactory.SF),
                        Expression.Constant("factory_parameter_1"),
                        Expression.Constant("factory_parameter_2"),
                        Expression.Constant("factory_parameter_3"),
                        Expression.Constant("factory_parameter_4"),
                        Expression.Constant("factory_parameter_5"),
                        Expression.Constant("factory_parameter_6"),
                        Expression.Constant("factory_parameter_7"),
                        Expression.Constant("factory_parameter_8"),
                        Expression.Constant("factory_parameter_9"),
                        Expression.Constant("factory_parameter_10"),
                        Expression.Constant("factory_parameter_11"),
                        Expression.Constant("factory_parameter_12"),
                        Expression.Constant("factory_parameter_13")
                    ),
                    state: null
                ),
                new CreateStream(
                    new Uri(Constants.Stream.QUX),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQubject<int, int>>), Constants.StreamFactory.SF),
                        Expression.Constant("factory_parameter_1"),
                        Expression.Constant("factory_parameter_2"),
                        Expression.Constant("factory_parameter_3"),
                        Expression.Constant("factory_parameter_4"),
                        Expression.Constant("factory_parameter_5"),
                        Expression.Constant("factory_parameter_6"),
                        Expression.Constant("factory_parameter_7"),
                        Expression.Constant("factory_parameter_8"),
                        Expression.Constant("factory_parameter_9"),
                        Expression.Constant("factory_parameter_10"),
                        Expression.Constant("factory_parameter_11"),
                        Expression.Constant("factory_parameter_12"),
                        Expression.Constant("factory_parameter_13")
                    ),
                    state: null
                ),
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, IAsyncReactiveQbserver<int>, Task<IAsyncReactiveQubscription>>), Constants.SubscribeUri),
                        Expression.Parameter(typeof(IAsyncReactiveQbservable<int>), Constants.Stream.FOO),
                        Expression.Parameter(typeof(IAsyncReactiveQbserver<int>), Constants.Observer.OB)
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_GetStreamFactory_14P()
        {
            Apply(
                ctx =>
                {
                    var factory = ctx.GetStreamFactory<string, string, string, string, string, string, string, string, string, string, string, string, string, string, int, int>(new Uri(Constants.StreamFactory.SF));
                    var stream = factory.CreateAsync(new Uri(Constants.Stream.FOO), "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", "factory_parameter_11", "factory_parameter_12", "factory_parameter_13", "factory_parameter_14", state: null, CancellationToken.None).Result;

                    factory.CreateAsync(new Uri(Constants.Stream.BAR), "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", "factory_parameter_11", "factory_parameter_12", "factory_parameter_13", "factory_parameter_14", state: null);
                    ((IAsyncReactiveSubjectFactory<int, int, string, string, string, string, string, string, string, string, string, string, string, string, string, string>)factory).CreateAsync(new Uri(Constants.Stream.QUX), "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", "factory_parameter_11", "factory_parameter_12", "factory_parameter_13", "factory_parameter_14", state: null, CancellationToken.None).Wait();

                    Assert.ThrowsException<ArgumentNullException>(() => factory.CreateAsync(streamUri: null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", "factory_parameter_11", "factory_parameter_12", "factory_parameter_13", "factory_parameter_14", state: null).Wait());
                    Assert.ThrowsException<ArgumentNullException>(() => factory.CreateAsync(streamUri: null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", "factory_parameter_11", "factory_parameter_12", "factory_parameter_13", "factory_parameter_14", state: null, CancellationToken.None).Wait());
                    Assert.ThrowsException<ArgumentNullException>(() => ((IAsyncReactiveSubjectFactory<int, int, string, string, string, string, string, string, string, string, string, string, string, string, string, string>)factory).CreateAsync(null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", "factory_parameter_11", "factory_parameter_12", "factory_parameter_13", "factory_parameter_14", state: null).Wait());
                    Assert.ThrowsException<ArgumentNullException>(() => ((IAsyncReactiveSubjectFactory<int, int, string, string, string, string, string, string, string, string, string, string, string, string, string, string>)factory).CreateAsync(null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", "factory_parameter_11", "factory_parameter_12", "factory_parameter_13", "factory_parameter_14", state: null, CancellationToken.None).Wait());

                    var observer = ctx.GetObserver<int>(new Uri(Constants.Observer.OB));
                    stream.SubscribeAsync(observer, new Uri(Constants.Subscription.SUB), state: null, CancellationToken.None).Wait();
                },
                new CreateStream(
                    new Uri(Constants.Stream.FOO),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQubject<int, int>>), Constants.StreamFactory.SF),
                        Expression.Constant("factory_parameter_1"),
                        Expression.Constant("factory_parameter_2"),
                        Expression.Constant("factory_parameter_3"),
                        Expression.Constant("factory_parameter_4"),
                        Expression.Constant("factory_parameter_5"),
                        Expression.Constant("factory_parameter_6"),
                        Expression.Constant("factory_parameter_7"),
                        Expression.Constant("factory_parameter_8"),
                        Expression.Constant("factory_parameter_9"),
                        Expression.Constant("factory_parameter_10"),
                        Expression.Constant("factory_parameter_11"),
                        Expression.Constant("factory_parameter_12"),
                        Expression.Constant("factory_parameter_13"),
                        Expression.Constant("factory_parameter_14")
                    ),
                    state: null
                ),
                new CreateStream(
                    new Uri(Constants.Stream.BAR),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQubject<int, int>>), Constants.StreamFactory.SF),
                        Expression.Constant("factory_parameter_1"),
                        Expression.Constant("factory_parameter_2"),
                        Expression.Constant("factory_parameter_3"),
                        Expression.Constant("factory_parameter_4"),
                        Expression.Constant("factory_parameter_5"),
                        Expression.Constant("factory_parameter_6"),
                        Expression.Constant("factory_parameter_7"),
                        Expression.Constant("factory_parameter_8"),
                        Expression.Constant("factory_parameter_9"),
                        Expression.Constant("factory_parameter_10"),
                        Expression.Constant("factory_parameter_11"),
                        Expression.Constant("factory_parameter_12"),
                        Expression.Constant("factory_parameter_13"),
                        Expression.Constant("factory_parameter_14")
                    ),
                    state: null
                ),
                new CreateStream(
                    new Uri(Constants.Stream.QUX),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQubject<int, int>>), Constants.StreamFactory.SF),
                        Expression.Constant("factory_parameter_1"),
                        Expression.Constant("factory_parameter_2"),
                        Expression.Constant("factory_parameter_3"),
                        Expression.Constant("factory_parameter_4"),
                        Expression.Constant("factory_parameter_5"),
                        Expression.Constant("factory_parameter_6"),
                        Expression.Constant("factory_parameter_7"),
                        Expression.Constant("factory_parameter_8"),
                        Expression.Constant("factory_parameter_9"),
                        Expression.Constant("factory_parameter_10"),
                        Expression.Constant("factory_parameter_11"),
                        Expression.Constant("factory_parameter_12"),
                        Expression.Constant("factory_parameter_13"),
                        Expression.Constant("factory_parameter_14")
                    ),
                    state: null
                ),
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, IAsyncReactiveQbserver<int>, Task<IAsyncReactiveQubscription>>), Constants.SubscribeUri),
                        Expression.Parameter(typeof(IAsyncReactiveQbservable<int>), Constants.Stream.FOO),
                        Expression.Parameter(typeof(IAsyncReactiveQbserver<int>), Constants.Observer.OB)
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_GetStreamFactory_15P()
        {
            Apply(
                ctx =>
                {
                    var factory = ctx.GetStreamFactory<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, int, int>(new Uri(Constants.StreamFactory.SF));
                    var stream = factory.CreateAsync(new Uri(Constants.Stream.FOO), "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", "factory_parameter_11", "factory_parameter_12", "factory_parameter_13", "factory_parameter_14", "factory_parameter_15", state: null, CancellationToken.None).Result;

                    factory.CreateAsync(new Uri(Constants.Stream.BAR), "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", "factory_parameter_11", "factory_parameter_12", "factory_parameter_13", "factory_parameter_14", "factory_parameter_15", state: null);
                    ((IAsyncReactiveSubjectFactory<int, int, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string>)factory).CreateAsync(new Uri(Constants.Stream.QUX), "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", "factory_parameter_11", "factory_parameter_12", "factory_parameter_13", "factory_parameter_14", "factory_parameter_15", state: null, CancellationToken.None).Wait();

                    Assert.ThrowsException<ArgumentNullException>(() => factory.CreateAsync(streamUri: null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", "factory_parameter_11", "factory_parameter_12", "factory_parameter_13", "factory_parameter_14", "factory_parameter_15", state: null).Wait());
                    Assert.ThrowsException<ArgumentNullException>(() => factory.CreateAsync(streamUri: null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", "factory_parameter_11", "factory_parameter_12", "factory_parameter_13", "factory_parameter_14", "factory_parameter_15", state: null, CancellationToken.None).Wait());
                    Assert.ThrowsException<ArgumentNullException>(() => ((IAsyncReactiveSubjectFactory<int, int, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string>)factory).CreateAsync(null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", "factory_parameter_11", "factory_parameter_12", "factory_parameter_13", "factory_parameter_14", "factory_parameter_15", state: null).Wait());
                    Assert.ThrowsException<ArgumentNullException>(() => ((IAsyncReactiveSubjectFactory<int, int, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string>)factory).CreateAsync(null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", "factory_parameter_11", "factory_parameter_12", "factory_parameter_13", "factory_parameter_14", "factory_parameter_15", state: null, CancellationToken.None).Wait());

                    var observer = ctx.GetObserver<int>(new Uri(Constants.Observer.OB));
                    stream.SubscribeAsync(observer, new Uri(Constants.Subscription.SUB), state: null, CancellationToken.None).Wait();
                },
                new CreateStream(
                    new Uri(Constants.Stream.FOO),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQubject<int, int>>), Constants.StreamFactory.SF),
                        Expression.Constant("factory_parameter_1"),
                        Expression.Constant("factory_parameter_2"),
                        Expression.Constant("factory_parameter_3"),
                        Expression.Constant("factory_parameter_4"),
                        Expression.Constant("factory_parameter_5"),
                        Expression.Constant("factory_parameter_6"),
                        Expression.Constant("factory_parameter_7"),
                        Expression.Constant("factory_parameter_8"),
                        Expression.Constant("factory_parameter_9"),
                        Expression.Constant("factory_parameter_10"),
                        Expression.Constant("factory_parameter_11"),
                        Expression.Constant("factory_parameter_12"),
                        Expression.Constant("factory_parameter_13"),
                        Expression.Constant("factory_parameter_14"),
                        Expression.Constant("factory_parameter_15")
                    ),
                    state: null
                ),
                new CreateStream(
                    new Uri(Constants.Stream.BAR),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQubject<int, int>>), Constants.StreamFactory.SF),
                        Expression.Constant("factory_parameter_1"),
                        Expression.Constant("factory_parameter_2"),
                        Expression.Constant("factory_parameter_3"),
                        Expression.Constant("factory_parameter_4"),
                        Expression.Constant("factory_parameter_5"),
                        Expression.Constant("factory_parameter_6"),
                        Expression.Constant("factory_parameter_7"),
                        Expression.Constant("factory_parameter_8"),
                        Expression.Constant("factory_parameter_9"),
                        Expression.Constant("factory_parameter_10"),
                        Expression.Constant("factory_parameter_11"),
                        Expression.Constant("factory_parameter_12"),
                        Expression.Constant("factory_parameter_13"),
                        Expression.Constant("factory_parameter_14"),
                        Expression.Constant("factory_parameter_15")
                    ),
                    state: null
                ),
                new CreateStream(
                    new Uri(Constants.Stream.QUX),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQubject<int, int>>), Constants.StreamFactory.SF),
                        Expression.Constant("factory_parameter_1"),
                        Expression.Constant("factory_parameter_2"),
                        Expression.Constant("factory_parameter_3"),
                        Expression.Constant("factory_parameter_4"),
                        Expression.Constant("factory_parameter_5"),
                        Expression.Constant("factory_parameter_6"),
                        Expression.Constant("factory_parameter_7"),
                        Expression.Constant("factory_parameter_8"),
                        Expression.Constant("factory_parameter_9"),
                        Expression.Constant("factory_parameter_10"),
                        Expression.Constant("factory_parameter_11"),
                        Expression.Constant("factory_parameter_12"),
                        Expression.Constant("factory_parameter_13"),
                        Expression.Constant("factory_parameter_14"),
                        Expression.Constant("factory_parameter_15")
                    ),
                    state: null
                ),
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<IAsyncReactiveQbservable<int>, IAsyncReactiveQbserver<int>, Task<IAsyncReactiveQubscription>>), Constants.SubscribeUri),
                        Expression.Parameter(typeof(IAsyncReactiveQbservable<int>), Constants.Stream.FOO),
                        Expression.Parameter(typeof(IAsyncReactiveQbserver<int>), Constants.Observer.OB)
                    ),
                    state: null
                )
            );
        }

        #endregion

        #region "ReactiveClientContext_GetSubscriptionFactory"

        [TestMethod]
        public void ReactiveClientContext_GetSubscriptionFactory_2P()
        {
            Apply(
                ctx =>
                {
                    var factory = ctx.GetSubscriptionFactory<string, string>(new Uri(Constants.SubscriptionFactory.SF));
                    factory.CreateAsync(new Uri(Constants.Subscription.SUB1), "factory_parameter_1", "factory_parameter_2", state: null, CancellationToken.None).Wait();

                    Assert.ThrowsException<ArgumentNullException>(() => factory.CreateAsync(subscriptionUri: null, "factory_parameter_1", "factory_parameter_2", state: null).Wait());
                    Assert.ThrowsException<ArgumentNullException>(() => factory.CreateAsync(subscriptionUri: null, "factory_parameter_1", "factory_parameter_2", state: null, CancellationToken.None).Wait());
                    Assert.ThrowsException<ArgumentNullException>(() => ((IAsyncReactiveSubscriptionFactory<string, string>)factory).CreateAsync(subscriptionUri: null, "factory_parameter_1", "factory_parameter_2", state: null).Wait());
                    Assert.ThrowsException<ArgumentNullException>(() => ((IAsyncReactiveSubscriptionFactory<string, string>)factory).CreateAsync(subscriptionUri: null, "factory_parameter_1", "factory_parameter_2", state: null, CancellationToken.None).Wait());
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB1),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<string, string, IAsyncReactiveQubscription>), Constants.SubscriptionFactory.SF),
                        Expression.Constant("factory_parameter_1"),
                        Expression.Constant("factory_parameter_2")
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_GetSubscriptionFactory_3P()
        {
            Apply(
                ctx =>
                {
                    var factory = ctx.GetSubscriptionFactory<string, string, string>(new Uri(Constants.SubscriptionFactory.SF));
                    factory.CreateAsync(new Uri(Constants.Subscription.SUB1), "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", state: null, CancellationToken.None).Wait();

                    Assert.ThrowsException<ArgumentNullException>(() => factory.CreateAsync(subscriptionUri: null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", state: null).Wait());
                    Assert.ThrowsException<ArgumentNullException>(() => factory.CreateAsync(subscriptionUri: null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", state: null, CancellationToken.None).Wait());
                    Assert.ThrowsException<ArgumentNullException>(() => ((IAsyncReactiveSubscriptionFactory<string, string, string>)factory).CreateAsync(subscriptionUri: null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", state: null).Wait());
                    Assert.ThrowsException<ArgumentNullException>(() => ((IAsyncReactiveSubscriptionFactory<string, string, string>)factory).CreateAsync(subscriptionUri: null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", state: null, CancellationToken.None).Wait());
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB1),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<string, string, string, IAsyncReactiveQubscription>), Constants.SubscriptionFactory.SF),
                        Expression.Constant("factory_parameter_1"),
                        Expression.Constant("factory_parameter_2"),
                        Expression.Constant("factory_parameter_3")
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_GetSubscriptionFactory_4P()
        {
            Apply(
                ctx =>
                {
                    var factory = ctx.GetSubscriptionFactory<string, string, string, string>(new Uri(Constants.SubscriptionFactory.SF));
                    factory.CreateAsync(new Uri(Constants.Subscription.SUB1), "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", state: null, CancellationToken.None).Wait();

                    Assert.ThrowsException<ArgumentNullException>(() => factory.CreateAsync(subscriptionUri: null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", state: null).Wait());
                    Assert.ThrowsException<ArgumentNullException>(() => factory.CreateAsync(subscriptionUri: null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", state: null, CancellationToken.None).Wait());
                    Assert.ThrowsException<ArgumentNullException>(() => ((IAsyncReactiveSubscriptionFactory<string, string, string, string>)factory).CreateAsync(subscriptionUri: null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", state: null).Wait());
                    Assert.ThrowsException<ArgumentNullException>(() => ((IAsyncReactiveSubscriptionFactory<string, string, string, string>)factory).CreateAsync(subscriptionUri: null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", state: null, CancellationToken.None).Wait());
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB1),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<string, string, string, string, IAsyncReactiveQubscription>), Constants.SubscriptionFactory.SF),
                        Expression.Constant("factory_parameter_1"),
                        Expression.Constant("factory_parameter_2"),
                        Expression.Constant("factory_parameter_3"),
                        Expression.Constant("factory_parameter_4")
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_GetSubscriptionFactory_5P()
        {
            Apply(
                ctx =>
                {
                    var factory = ctx.GetSubscriptionFactory<string, string, string, string, string>(new Uri(Constants.SubscriptionFactory.SF));
                    factory.CreateAsync(new Uri(Constants.Subscription.SUB1), "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", state: null, CancellationToken.None).Wait();

                    Assert.ThrowsException<ArgumentNullException>(() => factory.CreateAsync(subscriptionUri: null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", state: null).Wait());
                    Assert.ThrowsException<ArgumentNullException>(() => factory.CreateAsync(subscriptionUri: null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", state: null, CancellationToken.None).Wait());
                    Assert.ThrowsException<ArgumentNullException>(() => ((IAsyncReactiveSubscriptionFactory<string, string, string, string, string>)factory).CreateAsync(subscriptionUri: null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", state: null).Wait());
                    Assert.ThrowsException<ArgumentNullException>(() => ((IAsyncReactiveSubscriptionFactory<string, string, string, string, string>)factory).CreateAsync(subscriptionUri: null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", state: null, CancellationToken.None).Wait());
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB1),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<string, string, string, string, string, IAsyncReactiveQubscription>), Constants.SubscriptionFactory.SF),
                        Expression.Constant("factory_parameter_1"),
                        Expression.Constant("factory_parameter_2"),
                        Expression.Constant("factory_parameter_3"),
                        Expression.Constant("factory_parameter_4"),
                        Expression.Constant("factory_parameter_5")
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_GetSubscriptionFactory_6P()
        {
            Apply(
                ctx =>
                {
                    var factory = ctx.GetSubscriptionFactory<string, string, string, string, string, string>(new Uri(Constants.SubscriptionFactory.SF));
                    factory.CreateAsync(new Uri(Constants.Subscription.SUB1), "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", state: null, CancellationToken.None).Wait();

                    Assert.ThrowsException<ArgumentNullException>(() => factory.CreateAsync(subscriptionUri: null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", state: null).Wait());
                    Assert.ThrowsException<ArgumentNullException>(() => factory.CreateAsync(subscriptionUri: null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", state: null, CancellationToken.None).Wait());
                    Assert.ThrowsException<ArgumentNullException>(() => ((IAsyncReactiveSubscriptionFactory<string, string, string, string, string, string>)factory).CreateAsync(subscriptionUri: null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", state: null).Wait());
                    Assert.ThrowsException<ArgumentNullException>(() => ((IAsyncReactiveSubscriptionFactory<string, string, string, string, string, string>)factory).CreateAsync(subscriptionUri: null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", state: null, CancellationToken.None).Wait());
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB1),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, IAsyncReactiveQubscription>), Constants.SubscriptionFactory.SF),
                        Expression.Constant("factory_parameter_1"),
                        Expression.Constant("factory_parameter_2"),
                        Expression.Constant("factory_parameter_3"),
                        Expression.Constant("factory_parameter_4"),
                        Expression.Constant("factory_parameter_5"),
                        Expression.Constant("factory_parameter_6")
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_GetSubscriptionFactory_7P()
        {
            Apply(
                ctx =>
                {
                    var factory = ctx.GetSubscriptionFactory<string, string, string, string, string, string, string>(new Uri(Constants.SubscriptionFactory.SF));
                    factory.CreateAsync(new Uri(Constants.Subscription.SUB1), "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", state: null, CancellationToken.None).Wait();

                    Assert.ThrowsException<ArgumentNullException>(() => factory.CreateAsync(subscriptionUri: null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", state: null).Wait());
                    Assert.ThrowsException<ArgumentNullException>(() => factory.CreateAsync(subscriptionUri: null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", state: null, CancellationToken.None).Wait());
                    Assert.ThrowsException<ArgumentNullException>(() => ((IAsyncReactiveSubscriptionFactory<string, string, string, string, string, string, string>)factory).CreateAsync(subscriptionUri: null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", state: null).Wait());
                    Assert.ThrowsException<ArgumentNullException>(() => ((IAsyncReactiveSubscriptionFactory<string, string, string, string, string, string, string>)factory).CreateAsync(subscriptionUri: null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", state: null, CancellationToken.None).Wait());
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB1),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, IAsyncReactiveQubscription>), Constants.SubscriptionFactory.SF),
                        Expression.Constant("factory_parameter_1"),
                        Expression.Constant("factory_parameter_2"),
                        Expression.Constant("factory_parameter_3"),
                        Expression.Constant("factory_parameter_4"),
                        Expression.Constant("factory_parameter_5"),
                        Expression.Constant("factory_parameter_6"),
                        Expression.Constant("factory_parameter_7")
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_GetSubscriptionFactory_8P()
        {
            Apply(
                ctx =>
                {
                    var factory = ctx.GetSubscriptionFactory<string, string, string, string, string, string, string, string>(new Uri(Constants.SubscriptionFactory.SF));
                    factory.CreateAsync(new Uri(Constants.Subscription.SUB1), "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", state: null, CancellationToken.None).Wait();

                    Assert.ThrowsException<ArgumentNullException>(() => factory.CreateAsync(subscriptionUri: null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", state: null).Wait());
                    Assert.ThrowsException<ArgumentNullException>(() => factory.CreateAsync(subscriptionUri: null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", state: null, CancellationToken.None).Wait());
                    Assert.ThrowsException<ArgumentNullException>(() => ((IAsyncReactiveSubscriptionFactory<string, string, string, string, string, string, string, string>)factory).CreateAsync(subscriptionUri: null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", state: null).Wait());
                    Assert.ThrowsException<ArgumentNullException>(() => ((IAsyncReactiveSubscriptionFactory<string, string, string, string, string, string, string, string>)factory).CreateAsync(subscriptionUri: null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", state: null, CancellationToken.None).Wait());
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB1),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, IAsyncReactiveQubscription>), Constants.SubscriptionFactory.SF),
                        Expression.Constant("factory_parameter_1"),
                        Expression.Constant("factory_parameter_2"),
                        Expression.Constant("factory_parameter_3"),
                        Expression.Constant("factory_parameter_4"),
                        Expression.Constant("factory_parameter_5"),
                        Expression.Constant("factory_parameter_6"),
                        Expression.Constant("factory_parameter_7"),
                        Expression.Constant("factory_parameter_8")
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_GetSubscriptionFactory_9P()
        {
            Apply(
                ctx =>
                {
                    var factory = ctx.GetSubscriptionFactory<string, string, string, string, string, string, string, string, string>(new Uri(Constants.SubscriptionFactory.SF));
                    factory.CreateAsync(new Uri(Constants.Subscription.SUB1), "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", state: null, CancellationToken.None).Wait();

                    Assert.ThrowsException<ArgumentNullException>(() => factory.CreateAsync(subscriptionUri: null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", state: null).Wait());
                    Assert.ThrowsException<ArgumentNullException>(() => factory.CreateAsync(subscriptionUri: null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", state: null, CancellationToken.None).Wait());
                    Assert.ThrowsException<ArgumentNullException>(() => ((IAsyncReactiveSubscriptionFactory<string, string, string, string, string, string, string, string, string>)factory).CreateAsync(subscriptionUri: null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", state: null).Wait());
                    Assert.ThrowsException<ArgumentNullException>(() => ((IAsyncReactiveSubscriptionFactory<string, string, string, string, string, string, string, string, string>)factory).CreateAsync(subscriptionUri: null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", state: null, CancellationToken.None).Wait());
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB1),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, IAsyncReactiveQubscription>), Constants.SubscriptionFactory.SF),
                        Expression.Constant("factory_parameter_1"),
                        Expression.Constant("factory_parameter_2"),
                        Expression.Constant("factory_parameter_3"),
                        Expression.Constant("factory_parameter_4"),
                        Expression.Constant("factory_parameter_5"),
                        Expression.Constant("factory_parameter_6"),
                        Expression.Constant("factory_parameter_7"),
                        Expression.Constant("factory_parameter_8"),
                        Expression.Constant("factory_parameter_9")
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_GetSubscriptionFactory_10P()
        {
            Apply(
                ctx =>
                {
                    var factory = ctx.GetSubscriptionFactory<string, string, string, string, string, string, string, string, string, string>(new Uri(Constants.SubscriptionFactory.SF));
                    factory.CreateAsync(new Uri(Constants.Subscription.SUB1), "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", state: null, CancellationToken.None).Wait();

                    Assert.ThrowsException<ArgumentNullException>(() => factory.CreateAsync(subscriptionUri: null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", state: null).Wait());
                    Assert.ThrowsException<ArgumentNullException>(() => factory.CreateAsync(subscriptionUri: null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", state: null, CancellationToken.None).Wait());
                    Assert.ThrowsException<ArgumentNullException>(() => ((IAsyncReactiveSubscriptionFactory<string, string, string, string, string, string, string, string, string, string>)factory).CreateAsync(subscriptionUri: null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", state: null).Wait());
                    Assert.ThrowsException<ArgumentNullException>(() => ((IAsyncReactiveSubscriptionFactory<string, string, string, string, string, string, string, string, string, string>)factory).CreateAsync(subscriptionUri: null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", state: null, CancellationToken.None).Wait());
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB1),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQubscription>), Constants.SubscriptionFactory.SF),
                        Expression.Constant("factory_parameter_1"),
                        Expression.Constant("factory_parameter_2"),
                        Expression.Constant("factory_parameter_3"),
                        Expression.Constant("factory_parameter_4"),
                        Expression.Constant("factory_parameter_5"),
                        Expression.Constant("factory_parameter_6"),
                        Expression.Constant("factory_parameter_7"),
                        Expression.Constant("factory_parameter_8"),
                        Expression.Constant("factory_parameter_9"),
                        Expression.Constant("factory_parameter_10")
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_GetSubscriptionFactory_11P()
        {
            Apply(
                ctx =>
                {
                    var factory = ctx.GetSubscriptionFactory<string, string, string, string, string, string, string, string, string, string, string>(new Uri(Constants.SubscriptionFactory.SF));
                    factory.CreateAsync(new Uri(Constants.Subscription.SUB1), "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", "factory_parameter_11", state: null, CancellationToken.None).Wait();

                    Assert.ThrowsException<ArgumentNullException>(() => factory.CreateAsync(subscriptionUri: null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", "factory_parameter_11", state: null).Wait());
                    Assert.ThrowsException<ArgumentNullException>(() => factory.CreateAsync(subscriptionUri: null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", "factory_parameter_11", state: null, CancellationToken.None).Wait());
                    Assert.ThrowsException<ArgumentNullException>(() => ((IAsyncReactiveSubscriptionFactory<string, string, string, string, string, string, string, string, string, string, string>)factory).CreateAsync(subscriptionUri: null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", "factory_parameter_11", state: null).Wait());
                    Assert.ThrowsException<ArgumentNullException>(() => ((IAsyncReactiveSubscriptionFactory<string, string, string, string, string, string, string, string, string, string, string>)factory).CreateAsync(subscriptionUri: null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", "factory_parameter_11", state: null, CancellationToken.None).Wait());
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB1),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQubscription>), Constants.SubscriptionFactory.SF),
                        Expression.Constant("factory_parameter_1"),
                        Expression.Constant("factory_parameter_2"),
                        Expression.Constant("factory_parameter_3"),
                        Expression.Constant("factory_parameter_4"),
                        Expression.Constant("factory_parameter_5"),
                        Expression.Constant("factory_parameter_6"),
                        Expression.Constant("factory_parameter_7"),
                        Expression.Constant("factory_parameter_8"),
                        Expression.Constant("factory_parameter_9"),
                        Expression.Constant("factory_parameter_10"),
                        Expression.Constant("factory_parameter_11")
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_GetSubscriptionFactory_12P()
        {
            Apply(
                ctx =>
                {
                    var factory = ctx.GetSubscriptionFactory<string, string, string, string, string, string, string, string, string, string, string, string>(new Uri(Constants.SubscriptionFactory.SF));
                    factory.CreateAsync(new Uri(Constants.Subscription.SUB1), "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", "factory_parameter_11", "factory_parameter_12", state: null, CancellationToken.None).Wait();

                    Assert.ThrowsException<ArgumentNullException>(() => factory.CreateAsync(subscriptionUri: null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", "factory_parameter_11", "factory_parameter_12", state: null).Wait());
                    Assert.ThrowsException<ArgumentNullException>(() => factory.CreateAsync(subscriptionUri: null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", "factory_parameter_11", "factory_parameter_12", state: null, CancellationToken.None).Wait());
                    Assert.ThrowsException<ArgumentNullException>(() => ((IAsyncReactiveSubscriptionFactory<string, string, string, string, string, string, string, string, string, string, string, string>)factory).CreateAsync(subscriptionUri: null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", "factory_parameter_11", "factory_parameter_12", state: null).Wait());
                    Assert.ThrowsException<ArgumentNullException>(() => ((IAsyncReactiveSubscriptionFactory<string, string, string, string, string, string, string, string, string, string, string, string>)factory).CreateAsync(subscriptionUri: null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", "factory_parameter_11", "factory_parameter_12", state: null, CancellationToken.None).Wait());
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB1),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQubscription>), Constants.SubscriptionFactory.SF),
                        Expression.Constant("factory_parameter_1"),
                        Expression.Constant("factory_parameter_2"),
                        Expression.Constant("factory_parameter_3"),
                        Expression.Constant("factory_parameter_4"),
                        Expression.Constant("factory_parameter_5"),
                        Expression.Constant("factory_parameter_6"),
                        Expression.Constant("factory_parameter_7"),
                        Expression.Constant("factory_parameter_8"),
                        Expression.Constant("factory_parameter_9"),
                        Expression.Constant("factory_parameter_10"),
                        Expression.Constant("factory_parameter_11"),
                        Expression.Constant("factory_parameter_12")
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_GetSubscriptionFactory_13P()
        {
            Apply(
                ctx =>
                {
                    var factory = ctx.GetSubscriptionFactory<string, string, string, string, string, string, string, string, string, string, string, string, string>(new Uri(Constants.SubscriptionFactory.SF));
                    factory.CreateAsync(new Uri(Constants.Subscription.SUB1), "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", "factory_parameter_11", "factory_parameter_12", "factory_parameter_13", state: null, CancellationToken.None).Wait();

                    Assert.ThrowsException<ArgumentNullException>(() => factory.CreateAsync(subscriptionUri: null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", "factory_parameter_11", "factory_parameter_12", "factory_parameter_13", state: null).Wait());
                    Assert.ThrowsException<ArgumentNullException>(() => factory.CreateAsync(subscriptionUri: null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", "factory_parameter_11", "factory_parameter_12", "factory_parameter_13", state: null, CancellationToken.None).Wait());
                    Assert.ThrowsException<ArgumentNullException>(() => ((IAsyncReactiveSubscriptionFactory<string, string, string, string, string, string, string, string, string, string, string, string, string>)factory).CreateAsync(subscriptionUri: null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", "factory_parameter_11", "factory_parameter_12", "factory_parameter_13", state: null).Wait());
                    Assert.ThrowsException<ArgumentNullException>(() => ((IAsyncReactiveSubscriptionFactory<string, string, string, string, string, string, string, string, string, string, string, string, string>)factory).CreateAsync(subscriptionUri: null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", "factory_parameter_11", "factory_parameter_12", "factory_parameter_13", state: null, CancellationToken.None).Wait());
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB1),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQubscription>), Constants.SubscriptionFactory.SF),
                        Expression.Constant("factory_parameter_1"),
                        Expression.Constant("factory_parameter_2"),
                        Expression.Constant("factory_parameter_3"),
                        Expression.Constant("factory_parameter_4"),
                        Expression.Constant("factory_parameter_5"),
                        Expression.Constant("factory_parameter_6"),
                        Expression.Constant("factory_parameter_7"),
                        Expression.Constant("factory_parameter_8"),
                        Expression.Constant("factory_parameter_9"),
                        Expression.Constant("factory_parameter_10"),
                        Expression.Constant("factory_parameter_11"),
                        Expression.Constant("factory_parameter_12"),
                        Expression.Constant("factory_parameter_13")
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_GetSubscriptionFactory_14P()
        {
            Apply(
                ctx =>
                {
                    var factory = ctx.GetSubscriptionFactory<string, string, string, string, string, string, string, string, string, string, string, string, string, string>(new Uri(Constants.SubscriptionFactory.SF));
                    factory.CreateAsync(new Uri(Constants.Subscription.SUB1), "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", "factory_parameter_11", "factory_parameter_12", "factory_parameter_13", "factory_parameter_14", state: null, CancellationToken.None).Wait();

                    Assert.ThrowsException<ArgumentNullException>(() => factory.CreateAsync(subscriptionUri: null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", "factory_parameter_11", "factory_parameter_12", "factory_parameter_13", "factory_parameter_14", state: null).Wait());
                    Assert.ThrowsException<ArgumentNullException>(() => factory.CreateAsync(subscriptionUri: null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", "factory_parameter_11", "factory_parameter_12", "factory_parameter_13", "factory_parameter_14", state: null, CancellationToken.None).Wait());
                    Assert.ThrowsException<ArgumentNullException>(() => ((IAsyncReactiveSubscriptionFactory<string, string, string, string, string, string, string, string, string, string, string, string, string, string>)factory).CreateAsync(subscriptionUri: null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", "factory_parameter_11", "factory_parameter_12", "factory_parameter_13", "factory_parameter_14", state: null).Wait());
                    Assert.ThrowsException<ArgumentNullException>(() => ((IAsyncReactiveSubscriptionFactory<string, string, string, string, string, string, string, string, string, string, string, string, string, string>)factory).CreateAsync(subscriptionUri: null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", "factory_parameter_11", "factory_parameter_12", "factory_parameter_13", "factory_parameter_14", state: null, CancellationToken.None).Wait());
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB1),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQubscription>), Constants.SubscriptionFactory.SF),
                        Expression.Constant("factory_parameter_1"),
                        Expression.Constant("factory_parameter_2"),
                        Expression.Constant("factory_parameter_3"),
                        Expression.Constant("factory_parameter_4"),
                        Expression.Constant("factory_parameter_5"),
                        Expression.Constant("factory_parameter_6"),
                        Expression.Constant("factory_parameter_7"),
                        Expression.Constant("factory_parameter_8"),
                        Expression.Constant("factory_parameter_9"),
                        Expression.Constant("factory_parameter_10"),
                        Expression.Constant("factory_parameter_11"),
                        Expression.Constant("factory_parameter_12"),
                        Expression.Constant("factory_parameter_13"),
                        Expression.Constant("factory_parameter_14")
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_GetSubscriptionFactory_15P()
        {
            Apply(
                ctx =>
                {
                    var factory = ctx.GetSubscriptionFactory<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string>(new Uri(Constants.SubscriptionFactory.SF));
                    factory.CreateAsync(new Uri(Constants.Subscription.SUB1), "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", "factory_parameter_11", "factory_parameter_12", "factory_parameter_13", "factory_parameter_14", "factory_parameter_15", state: null, CancellationToken.None).Wait();

                    Assert.ThrowsException<ArgumentNullException>(() => factory.CreateAsync(subscriptionUri: null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", "factory_parameter_11", "factory_parameter_12", "factory_parameter_13", "factory_parameter_14", "factory_parameter_15", state: null).Wait());
                    Assert.ThrowsException<ArgumentNullException>(() => factory.CreateAsync(subscriptionUri: null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", "factory_parameter_11", "factory_parameter_12", "factory_parameter_13", "factory_parameter_14", "factory_parameter_15", state: null, CancellationToken.None).Wait());
                    Assert.ThrowsException<ArgumentNullException>(() => ((IAsyncReactiveSubscriptionFactory<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string>)factory).CreateAsync(subscriptionUri: null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", "factory_parameter_11", "factory_parameter_12", "factory_parameter_13", "factory_parameter_14", "factory_parameter_15", state: null).Wait());
                    Assert.ThrowsException<ArgumentNullException>(() => ((IAsyncReactiveSubscriptionFactory<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string>)factory).CreateAsync(subscriptionUri: null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", "factory_parameter_11", "factory_parameter_12", "factory_parameter_13", "factory_parameter_14", "factory_parameter_15", state: null, CancellationToken.None).Wait());
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB1),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQubscription>), Constants.SubscriptionFactory.SF),
                        Expression.Constant("factory_parameter_1"),
                        Expression.Constant("factory_parameter_2"),
                        Expression.Constant("factory_parameter_3"),
                        Expression.Constant("factory_parameter_4"),
                        Expression.Constant("factory_parameter_5"),
                        Expression.Constant("factory_parameter_6"),
                        Expression.Constant("factory_parameter_7"),
                        Expression.Constant("factory_parameter_8"),
                        Expression.Constant("factory_parameter_9"),
                        Expression.Constant("factory_parameter_10"),
                        Expression.Constant("factory_parameter_11"),
                        Expression.Constant("factory_parameter_12"),
                        Expression.Constant("factory_parameter_13"),
                        Expression.Constant("factory_parameter_14"),
                        Expression.Constant("factory_parameter_15")
                    ),
                    state: null
                )
            );
        }

        #endregion

        #region "ReactiveClientContext_DefineObservableAsync"

        [TestMethod]
        public void ReactiveClientContext_DefineObservableAsync_2P()
        {
            var lambdaParameters = new[] {
                Expression.Parameter(typeof(string), "parameter_1"),
                Expression.Parameter(typeof(string), "parameter_2")
            };

            Apply(
                ctx =>
                {
                    var obs = ctx.GetObservable<string, string, int>(new Uri(Constants.Observable.ZS));
                    ctx.DefineObservableAsync<string, string, int>(
                        new Uri(Constants.StreamFactory.SG),
                        (parameter_1, parameter_2) => obs(parameter_1, parameter_2),
                        state: null,
                        CancellationToken.None
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineObservableAsync<string, string, int>(
                            uri: null,
                            (parameter_1, parameter_2) => obs(parameter_1, parameter_2),
                            state: null,
                            CancellationToken.None
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineObservableAsync<string, string, int>(
                            new Uri(Constants.StreamFactory.SG),
                            observable: null,
                            state: null,
                            CancellationToken.None
                        )
                    );
                },
                new DefineObservable(
                    new Uri(Constants.StreamFactory.SG),
                    Expression.Lambda(
                        Expression.Invoke(
                            Expression.Parameter(
                                typeof(Func<string, string, IAsyncReactiveQbservable<int>>),
                                Constants.Observable.ZS
                            ),
                            lambdaParameters[0],
                            lambdaParameters[1]
                        ),
                        lambdaParameters[0],
                        lambdaParameters[1]
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_DefineObservableAsync_3P()
        {
            var lambdaParameters = new[] {
                Expression.Parameter(typeof(string), "parameter_1"),
                Expression.Parameter(typeof(string), "parameter_2"),
                Expression.Parameter(typeof(string), "parameter_3")
            };

            Apply(
                ctx =>
                {
                    var obs = ctx.GetObservable<string, string, string, int>(new Uri(Constants.Observable.ZS));
                    ctx.DefineObservableAsync<string, string, string, int>(
                        new Uri(Constants.StreamFactory.SG),
                        (parameter_1, parameter_2, parameter_3) => obs(parameter_1, parameter_2, parameter_3),
                        state: null,
                        CancellationToken.None
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineObservableAsync<string, string, string, int>(
                            uri: null,
                            (parameter_1, parameter_2, parameter_3) => obs(parameter_1, parameter_2, parameter_3),
                            state: null,
                            CancellationToken.None
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineObservableAsync<string, string, string, int>(
                            new Uri(Constants.StreamFactory.SG),
                            observable: null,
                            state: null,
                            CancellationToken.None
                        )
                    );
                },
                new DefineObservable(
                    new Uri(Constants.StreamFactory.SG),
                    Expression.Lambda(
                        Expression.Invoke(
                            Expression.Parameter(
                                typeof(Func<string, string, string, IAsyncReactiveQbservable<int>>),
                                Constants.Observable.ZS
                            ),
                            lambdaParameters[0],
                            lambdaParameters[1],
                            lambdaParameters[2]
                        ),
                        lambdaParameters[0],
                        lambdaParameters[1],
                        lambdaParameters[2]
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_DefineObservableAsync_4P()
        {
            var lambdaParameters = new[] {
                Expression.Parameter(typeof(string), "parameter_1"),
                Expression.Parameter(typeof(string), "parameter_2"),
                Expression.Parameter(typeof(string), "parameter_3"),
                Expression.Parameter(typeof(string), "parameter_4")
            };

            Apply(
                ctx =>
                {
                    var obs = ctx.GetObservable<string, string, string, string, int>(new Uri(Constants.Observable.ZS));
                    ctx.DefineObservableAsync<string, string, string, string, int>(
                        new Uri(Constants.StreamFactory.SG),
                        (parameter_1, parameter_2, parameter_3, parameter_4) => obs(parameter_1, parameter_2, parameter_3, parameter_4),
                        state: null,
                        CancellationToken.None
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineObservableAsync<string, string, string, string, int>(
                            uri: null,
                            (parameter_1, parameter_2, parameter_3, parameter_4) => obs(parameter_1, parameter_2, parameter_3, parameter_4),
                            state: null,
                            CancellationToken.None
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineObservableAsync<string, string, string, string, int>(
                            new Uri(Constants.StreamFactory.SG),
                            observable: null,
                            state: null,
                            CancellationToken.None
                        )
                    );
                },
                new DefineObservable(
                    new Uri(Constants.StreamFactory.SG),
                    Expression.Lambda(
                        Expression.Invoke(
                            Expression.Parameter(
                                typeof(Func<string, string, string, string, IAsyncReactiveQbservable<int>>),
                                Constants.Observable.ZS
                            ),
                            lambdaParameters[0],
                            lambdaParameters[1],
                            lambdaParameters[2],
                            lambdaParameters[3]
                        ),
                        lambdaParameters[0],
                        lambdaParameters[1],
                        lambdaParameters[2],
                        lambdaParameters[3]
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_DefineObservableAsync_5P()
        {
            var lambdaParameters = new[] {
                Expression.Parameter(typeof(string), "parameter_1"),
                Expression.Parameter(typeof(string), "parameter_2"),
                Expression.Parameter(typeof(string), "parameter_3"),
                Expression.Parameter(typeof(string), "parameter_4"),
                Expression.Parameter(typeof(string), "parameter_5")
            };

            Apply(
                ctx =>
                {
                    var obs = ctx.GetObservable<string, string, string, string, string, int>(new Uri(Constants.Observable.ZS));
                    ctx.DefineObservableAsync<string, string, string, string, string, int>(
                        new Uri(Constants.StreamFactory.SG),
                        (parameter_1, parameter_2, parameter_3, parameter_4, parameter_5) => obs(parameter_1, parameter_2, parameter_3, parameter_4, parameter_5),
                        state: null,
                        CancellationToken.None
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineObservableAsync<string, string, string, string, string, int>(
                            uri: null,
                            (parameter_1, parameter_2, parameter_3, parameter_4, parameter_5) => obs(parameter_1, parameter_2, parameter_3, parameter_4, parameter_5),
                            state: null,
                            CancellationToken.None
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineObservableAsync<string, string, string, string, string, int>(
                            new Uri(Constants.StreamFactory.SG),
                            observable: null,
                            state: null,
                            CancellationToken.None
                        )
                    );
                },
                new DefineObservable(
                    new Uri(Constants.StreamFactory.SG),
                    Expression.Lambda(
                        Expression.Invoke(
                            Expression.Parameter(
                                typeof(Func<string, string, string, string, string, IAsyncReactiveQbservable<int>>),
                                Constants.Observable.ZS
                            ),
                            lambdaParameters[0],
                            lambdaParameters[1],
                            lambdaParameters[2],
                            lambdaParameters[3],
                            lambdaParameters[4]
                        ),
                        lambdaParameters[0],
                        lambdaParameters[1],
                        lambdaParameters[2],
                        lambdaParameters[3],
                        lambdaParameters[4]
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_DefineObservableAsync_6P()
        {
            var lambdaParameters = new[] {
                Expression.Parameter(typeof(string), "parameter_1"),
                Expression.Parameter(typeof(string), "parameter_2"),
                Expression.Parameter(typeof(string), "parameter_3"),
                Expression.Parameter(typeof(string), "parameter_4"),
                Expression.Parameter(typeof(string), "parameter_5"),
                Expression.Parameter(typeof(string), "parameter_6")
            };

            Apply(
                ctx =>
                {
                    var obs = ctx.GetObservable<string, string, string, string, string, string, int>(new Uri(Constants.Observable.ZS));
                    ctx.DefineObservableAsync<string, string, string, string, string, string, int>(
                        new Uri(Constants.StreamFactory.SG),
                        (parameter_1, parameter_2, parameter_3, parameter_4, parameter_5, parameter_6) => obs(parameter_1, parameter_2, parameter_3, parameter_4, parameter_5, parameter_6),
                        state: null,
                        CancellationToken.None
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineObservableAsync<string, string, string, string, string, string, int>(
                            uri: null,
                            (parameter_1, parameter_2, parameter_3, parameter_4, parameter_5, parameter_6) => obs(parameter_1, parameter_2, parameter_3, parameter_4, parameter_5, parameter_6),
                            state: null,
                            CancellationToken.None
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineObservableAsync<string, string, string, string, string, string, int>(
                            new Uri(Constants.StreamFactory.SG),
                            observable: null,
                            state: null,
                            CancellationToken.None
                        )
                    );
                },
                new DefineObservable(
                    new Uri(Constants.StreamFactory.SG),
                    Expression.Lambda(
                        Expression.Invoke(
                            Expression.Parameter(
                                typeof(Func<string, string, string, string, string, string, IAsyncReactiveQbservable<int>>),
                                Constants.Observable.ZS
                            ),
                            lambdaParameters[0],
                            lambdaParameters[1],
                            lambdaParameters[2],
                            lambdaParameters[3],
                            lambdaParameters[4],
                            lambdaParameters[5]
                        ),
                        lambdaParameters[0],
                        lambdaParameters[1],
                        lambdaParameters[2],
                        lambdaParameters[3],
                        lambdaParameters[4],
                        lambdaParameters[5]
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_DefineObservableAsync_7P()
        {
            var lambdaParameters = new[] {
                Expression.Parameter(typeof(string), "parameter_1"),
                Expression.Parameter(typeof(string), "parameter_2"),
                Expression.Parameter(typeof(string), "parameter_3"),
                Expression.Parameter(typeof(string), "parameter_4"),
                Expression.Parameter(typeof(string), "parameter_5"),
                Expression.Parameter(typeof(string), "parameter_6"),
                Expression.Parameter(typeof(string), "parameter_7")
            };

            Apply(
                ctx =>
                {
                    var obs = ctx.GetObservable<string, string, string, string, string, string, string, int>(new Uri(Constants.Observable.ZS));
                    ctx.DefineObservableAsync<string, string, string, string, string, string, string, int>(
                        new Uri(Constants.StreamFactory.SG),
                        (parameter_1, parameter_2, parameter_3, parameter_4, parameter_5, parameter_6, parameter_7) => obs(parameter_1, parameter_2, parameter_3, parameter_4, parameter_5, parameter_6, parameter_7),
                        state: null,
                        CancellationToken.None
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineObservableAsync<string, string, string, string, string, string, string, int>(
                            uri: null,
                            (parameter_1, parameter_2, parameter_3, parameter_4, parameter_5, parameter_6, parameter_7) => obs(parameter_1, parameter_2, parameter_3, parameter_4, parameter_5, parameter_6, parameter_7),
                            state: null,
                            CancellationToken.None
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineObservableAsync<string, string, string, string, string, string, string, int>(
                            new Uri(Constants.StreamFactory.SG),
                            observable: null,
                            state: null,
                            CancellationToken.None
                        )
                    );
                },
                new DefineObservable(
                    new Uri(Constants.StreamFactory.SG),
                    Expression.Lambda(
                        Expression.Invoke(
                            Expression.Parameter(
                                typeof(Func<string, string, string, string, string, string, string, IAsyncReactiveQbservable<int>>),
                                Constants.Observable.ZS
                            ),
                            lambdaParameters[0],
                            lambdaParameters[1],
                            lambdaParameters[2],
                            lambdaParameters[3],
                            lambdaParameters[4],
                            lambdaParameters[5],
                            lambdaParameters[6]
                        ),
                        lambdaParameters[0],
                        lambdaParameters[1],
                        lambdaParameters[2],
                        lambdaParameters[3],
                        lambdaParameters[4],
                        lambdaParameters[5],
                        lambdaParameters[6]
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_DefineObservableAsync_8P()
        {
            var lambdaParameters = new[] {
                Expression.Parameter(typeof(string), "parameter_1"),
                Expression.Parameter(typeof(string), "parameter_2"),
                Expression.Parameter(typeof(string), "parameter_3"),
                Expression.Parameter(typeof(string), "parameter_4"),
                Expression.Parameter(typeof(string), "parameter_5"),
                Expression.Parameter(typeof(string), "parameter_6"),
                Expression.Parameter(typeof(string), "parameter_7"),
                Expression.Parameter(typeof(string), "parameter_8")
            };

            Apply(
                ctx =>
                {
                    var obs = ctx.GetObservable<string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observable.ZS));
                    ctx.DefineObservableAsync<string, string, string, string, string, string, string, string, int>(
                        new Uri(Constants.StreamFactory.SG),
                        (parameter_1, parameter_2, parameter_3, parameter_4, parameter_5, parameter_6, parameter_7, parameter_8) => obs(parameter_1, parameter_2, parameter_3, parameter_4, parameter_5, parameter_6, parameter_7, parameter_8),
                        state: null,
                        CancellationToken.None
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineObservableAsync<string, string, string, string, string, string, string, string, int>(
                            uri: null,
                            (parameter_1, parameter_2, parameter_3, parameter_4, parameter_5, parameter_6, parameter_7, parameter_8) => obs(parameter_1, parameter_2, parameter_3, parameter_4, parameter_5, parameter_6, parameter_7, parameter_8),
                            state: null,
                            CancellationToken.None
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineObservableAsync<string, string, string, string, string, string, string, string, int>(
                            new Uri(Constants.StreamFactory.SG),
                            observable: null,
                            state: null,
                            CancellationToken.None
                        )
                    );
                },
                new DefineObservable(
                    new Uri(Constants.StreamFactory.SG),
                    Expression.Lambda(
                        Expression.Invoke(
                            Expression.Parameter(
                                typeof(Func<string, string, string, string, string, string, string, string, IAsyncReactiveQbservable<int>>),
                                Constants.Observable.ZS
                            ),
                            lambdaParameters[0],
                            lambdaParameters[1],
                            lambdaParameters[2],
                            lambdaParameters[3],
                            lambdaParameters[4],
                            lambdaParameters[5],
                            lambdaParameters[6],
                            lambdaParameters[7]
                        ),
                        lambdaParameters[0],
                        lambdaParameters[1],
                        lambdaParameters[2],
                        lambdaParameters[3],
                        lambdaParameters[4],
                        lambdaParameters[5],
                        lambdaParameters[6],
                        lambdaParameters[7]
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_DefineObservableAsync_9P()
        {
            var lambdaParameters = new[] {
                Expression.Parameter(typeof(string), "parameter_1"),
                Expression.Parameter(typeof(string), "parameter_2"),
                Expression.Parameter(typeof(string), "parameter_3"),
                Expression.Parameter(typeof(string), "parameter_4"),
                Expression.Parameter(typeof(string), "parameter_5"),
                Expression.Parameter(typeof(string), "parameter_6"),
                Expression.Parameter(typeof(string), "parameter_7"),
                Expression.Parameter(typeof(string), "parameter_8"),
                Expression.Parameter(typeof(string), "parameter_9")
            };

            Apply(
                ctx =>
                {
                    var obs = ctx.GetObservable<string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observable.ZS));
                    ctx.DefineObservableAsync<string, string, string, string, string, string, string, string, string, int>(
                        new Uri(Constants.StreamFactory.SG),
                        (parameter_1, parameter_2, parameter_3, parameter_4, parameter_5, parameter_6, parameter_7, parameter_8, parameter_9) => obs(parameter_1, parameter_2, parameter_3, parameter_4, parameter_5, parameter_6, parameter_7, parameter_8, parameter_9),
                        state: null,
                        CancellationToken.None
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineObservableAsync<string, string, string, string, string, string, string, string, string, int>(
                            uri: null,
                            (parameter_1, parameter_2, parameter_3, parameter_4, parameter_5, parameter_6, parameter_7, parameter_8, parameter_9) => obs(parameter_1, parameter_2, parameter_3, parameter_4, parameter_5, parameter_6, parameter_7, parameter_8, parameter_9),
                            state: null,
                            CancellationToken.None
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineObservableAsync<string, string, string, string, string, string, string, string, string, int>(
                            new Uri(Constants.StreamFactory.SG),
                            observable: null,
                            state: null,
                            CancellationToken.None
                        )
                    );
                },
                new DefineObservable(
                    new Uri(Constants.StreamFactory.SG),
                    Expression.Lambda(
                        Expression.Invoke(
                            Expression.Parameter(
                                typeof(Func<string, string, string, string, string, string, string, string, string, IAsyncReactiveQbservable<int>>),
                                Constants.Observable.ZS
                            ),
                            lambdaParameters[0],
                            lambdaParameters[1],
                            lambdaParameters[2],
                            lambdaParameters[3],
                            lambdaParameters[4],
                            lambdaParameters[5],
                            lambdaParameters[6],
                            lambdaParameters[7],
                            lambdaParameters[8]
                        ),
                        lambdaParameters[0],
                        lambdaParameters[1],
                        lambdaParameters[2],
                        lambdaParameters[3],
                        lambdaParameters[4],
                        lambdaParameters[5],
                        lambdaParameters[6],
                        lambdaParameters[7],
                        lambdaParameters[8]
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_DefineObservableAsync_10P()
        {
            var lambdaParameters = new[] {
                Expression.Parameter(typeof(string), "parameter_1"),
                Expression.Parameter(typeof(string), "parameter_2"),
                Expression.Parameter(typeof(string), "parameter_3"),
                Expression.Parameter(typeof(string), "parameter_4"),
                Expression.Parameter(typeof(string), "parameter_5"),
                Expression.Parameter(typeof(string), "parameter_6"),
                Expression.Parameter(typeof(string), "parameter_7"),
                Expression.Parameter(typeof(string), "parameter_8"),
                Expression.Parameter(typeof(string), "parameter_9"),
                Expression.Parameter(typeof(string), "parameter_10")
            };

            Apply(
                ctx =>
                {
                    var obs = ctx.GetObservable<string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observable.ZS));
                    ctx.DefineObservableAsync<string, string, string, string, string, string, string, string, string, string, int>(
                        new Uri(Constants.StreamFactory.SG),
                        (parameter_1, parameter_2, parameter_3, parameter_4, parameter_5, parameter_6, parameter_7, parameter_8, parameter_9, parameter_10) => obs(parameter_1, parameter_2, parameter_3, parameter_4, parameter_5, parameter_6, parameter_7, parameter_8, parameter_9, parameter_10),
                        state: null,
                        CancellationToken.None
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineObservableAsync<string, string, string, string, string, string, string, string, string, string, int>(
                            uri: null,
                            (parameter_1, parameter_2, parameter_3, parameter_4, parameter_5, parameter_6, parameter_7, parameter_8, parameter_9, parameter_10) => obs(parameter_1, parameter_2, parameter_3, parameter_4, parameter_5, parameter_6, parameter_7, parameter_8, parameter_9, parameter_10),
                            state: null,
                            CancellationToken.None
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineObservableAsync<string, string, string, string, string, string, string, string, string, string, int>(
                            new Uri(Constants.StreamFactory.SG),
                            observable: null,
                            state: null,
                            CancellationToken.None
                        )
                    );
                },
                new DefineObservable(
                    new Uri(Constants.StreamFactory.SG),
                    Expression.Lambda(
                        Expression.Invoke(
                            Expression.Parameter(
                                typeof(Func<string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQbservable<int>>),
                                Constants.Observable.ZS
                            ),
                            lambdaParameters[0],
                            lambdaParameters[1],
                            lambdaParameters[2],
                            lambdaParameters[3],
                            lambdaParameters[4],
                            lambdaParameters[5],
                            lambdaParameters[6],
                            lambdaParameters[7],
                            lambdaParameters[8],
                            lambdaParameters[9]
                        ),
                        lambdaParameters[0],
                        lambdaParameters[1],
                        lambdaParameters[2],
                        lambdaParameters[3],
                        lambdaParameters[4],
                        lambdaParameters[5],
                        lambdaParameters[6],
                        lambdaParameters[7],
                        lambdaParameters[8],
                        lambdaParameters[9]
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_DefineObservableAsync_11P()
        {
            var lambdaParameters = new[] {
                Expression.Parameter(typeof(string), "parameter_1"),
                Expression.Parameter(typeof(string), "parameter_2"),
                Expression.Parameter(typeof(string), "parameter_3"),
                Expression.Parameter(typeof(string), "parameter_4"),
                Expression.Parameter(typeof(string), "parameter_5"),
                Expression.Parameter(typeof(string), "parameter_6"),
                Expression.Parameter(typeof(string), "parameter_7"),
                Expression.Parameter(typeof(string), "parameter_8"),
                Expression.Parameter(typeof(string), "parameter_9"),
                Expression.Parameter(typeof(string), "parameter_10"),
                Expression.Parameter(typeof(string), "parameter_11")
            };

            Apply(
                ctx =>
                {
                    var obs = ctx.GetObservable<string, string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observable.ZS));
                    ctx.DefineObservableAsync<string, string, string, string, string, string, string, string, string, string, string, int>(
                        new Uri(Constants.StreamFactory.SG),
                        (parameter_1, parameter_2, parameter_3, parameter_4, parameter_5, parameter_6, parameter_7, parameter_8, parameter_9, parameter_10, parameter_11) => obs(parameter_1, parameter_2, parameter_3, parameter_4, parameter_5, parameter_6, parameter_7, parameter_8, parameter_9, parameter_10, parameter_11),
                        state: null,
                        CancellationToken.None
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineObservableAsync<string, string, string, string, string, string, string, string, string, string, string, int>(
                            uri: null,
                            (parameter_1, parameter_2, parameter_3, parameter_4, parameter_5, parameter_6, parameter_7, parameter_8, parameter_9, parameter_10, parameter_11) => obs(parameter_1, parameter_2, parameter_3, parameter_4, parameter_5, parameter_6, parameter_7, parameter_8, parameter_9, parameter_10, parameter_11),
                            state: null,
                            CancellationToken.None
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineObservableAsync<string, string, string, string, string, string, string, string, string, string, string, int>(
                            new Uri(Constants.StreamFactory.SG),
                            observable: null,
                            state: null,
                            CancellationToken.None
                        )
                    );
                },
                new DefineObservable(
                    new Uri(Constants.StreamFactory.SG),
                    Expression.Lambda(
                        Expression.Invoke(
                            Expression.Parameter(
                                typeof(Func<string, string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQbservable<int>>),
                                Constants.Observable.ZS
                            ),
                            lambdaParameters[0],
                            lambdaParameters[1],
                            lambdaParameters[2],
                            lambdaParameters[3],
                            lambdaParameters[4],
                            lambdaParameters[5],
                            lambdaParameters[6],
                            lambdaParameters[7],
                            lambdaParameters[8],
                            lambdaParameters[9],
                            lambdaParameters[10]
                        ),
                        lambdaParameters[0],
                        lambdaParameters[1],
                        lambdaParameters[2],
                        lambdaParameters[3],
                        lambdaParameters[4],
                        lambdaParameters[5],
                        lambdaParameters[6],
                        lambdaParameters[7],
                        lambdaParameters[8],
                        lambdaParameters[9],
                        lambdaParameters[10]
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_DefineObservableAsync_12P()
        {
            var lambdaParameters = new[] {
                Expression.Parameter(typeof(string), "parameter_1"),
                Expression.Parameter(typeof(string), "parameter_2"),
                Expression.Parameter(typeof(string), "parameter_3"),
                Expression.Parameter(typeof(string), "parameter_4"),
                Expression.Parameter(typeof(string), "parameter_5"),
                Expression.Parameter(typeof(string), "parameter_6"),
                Expression.Parameter(typeof(string), "parameter_7"),
                Expression.Parameter(typeof(string), "parameter_8"),
                Expression.Parameter(typeof(string), "parameter_9"),
                Expression.Parameter(typeof(string), "parameter_10"),
                Expression.Parameter(typeof(string), "parameter_11"),
                Expression.Parameter(typeof(string), "parameter_12")
            };

            Apply(
                ctx =>
                {
                    var obs = ctx.GetObservable<string, string, string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observable.ZS));
                    ctx.DefineObservableAsync<string, string, string, string, string, string, string, string, string, string, string, string, int>(
                        new Uri(Constants.StreamFactory.SG),
                        (parameter_1, parameter_2, parameter_3, parameter_4, parameter_5, parameter_6, parameter_7, parameter_8, parameter_9, parameter_10, parameter_11, parameter_12) => obs(parameter_1, parameter_2, parameter_3, parameter_4, parameter_5, parameter_6, parameter_7, parameter_8, parameter_9, parameter_10, parameter_11, parameter_12),
                        state: null,
                        CancellationToken.None
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineObservableAsync<string, string, string, string, string, string, string, string, string, string, string, string, int>(
                            uri: null,
                            (parameter_1, parameter_2, parameter_3, parameter_4, parameter_5, parameter_6, parameter_7, parameter_8, parameter_9, parameter_10, parameter_11, parameter_12) => obs(parameter_1, parameter_2, parameter_3, parameter_4, parameter_5, parameter_6, parameter_7, parameter_8, parameter_9, parameter_10, parameter_11, parameter_12),
                            state: null,
                            CancellationToken.None
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineObservableAsync<string, string, string, string, string, string, string, string, string, string, string, string, int>(
                            new Uri(Constants.StreamFactory.SG),
                            observable: null,
                            state: null,
                            CancellationToken.None
                        )
                    );
                },
                new DefineObservable(
                    new Uri(Constants.StreamFactory.SG),
                    Expression.Lambda(
                        Expression.Invoke(
                            Expression.Parameter(
                                typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQbservable<int>>),
                                Constants.Observable.ZS
                            ),
                            lambdaParameters[0],
                            lambdaParameters[1],
                            lambdaParameters[2],
                            lambdaParameters[3],
                            lambdaParameters[4],
                            lambdaParameters[5],
                            lambdaParameters[6],
                            lambdaParameters[7],
                            lambdaParameters[8],
                            lambdaParameters[9],
                            lambdaParameters[10],
                            lambdaParameters[11]
                        ),
                        lambdaParameters[0],
                        lambdaParameters[1],
                        lambdaParameters[2],
                        lambdaParameters[3],
                        lambdaParameters[4],
                        lambdaParameters[5],
                        lambdaParameters[6],
                        lambdaParameters[7],
                        lambdaParameters[8],
                        lambdaParameters[9],
                        lambdaParameters[10],
                        lambdaParameters[11]
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_DefineObservableAsync_13P()
        {
            var lambdaParameters = new[] {
                Expression.Parameter(typeof(string), "parameter_1"),
                Expression.Parameter(typeof(string), "parameter_2"),
                Expression.Parameter(typeof(string), "parameter_3"),
                Expression.Parameter(typeof(string), "parameter_4"),
                Expression.Parameter(typeof(string), "parameter_5"),
                Expression.Parameter(typeof(string), "parameter_6"),
                Expression.Parameter(typeof(string), "parameter_7"),
                Expression.Parameter(typeof(string), "parameter_8"),
                Expression.Parameter(typeof(string), "parameter_9"),
                Expression.Parameter(typeof(string), "parameter_10"),
                Expression.Parameter(typeof(string), "parameter_11"),
                Expression.Parameter(typeof(string), "parameter_12"),
                Expression.Parameter(typeof(string), "parameter_13")
            };

            Apply(
                ctx =>
                {
                    var obs = ctx.GetObservable<string, string, string, string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observable.ZS));
                    ctx.DefineObservableAsync<string, string, string, string, string, string, string, string, string, string, string, string, string, int>(
                        new Uri(Constants.StreamFactory.SG),
                        (parameter_1, parameter_2, parameter_3, parameter_4, parameter_5, parameter_6, parameter_7, parameter_8, parameter_9, parameter_10, parameter_11, parameter_12, parameter_13) => obs(parameter_1, parameter_2, parameter_3, parameter_4, parameter_5, parameter_6, parameter_7, parameter_8, parameter_9, parameter_10, parameter_11, parameter_12, parameter_13),
                        state: null,
                        CancellationToken.None
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineObservableAsync<string, string, string, string, string, string, string, string, string, string, string, string, string, int>(
                            uri: null,
                            (parameter_1, parameter_2, parameter_3, parameter_4, parameter_5, parameter_6, parameter_7, parameter_8, parameter_9, parameter_10, parameter_11, parameter_12, parameter_13) => obs(parameter_1, parameter_2, parameter_3, parameter_4, parameter_5, parameter_6, parameter_7, parameter_8, parameter_9, parameter_10, parameter_11, parameter_12, parameter_13),
                            state: null,
                            CancellationToken.None
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineObservableAsync<string, string, string, string, string, string, string, string, string, string, string, string, string, int>(
                            new Uri(Constants.StreamFactory.SG),
                            observable: null,
                            state: null,
                            CancellationToken.None
                        )
                    );
                },
                new DefineObservable(
                    new Uri(Constants.StreamFactory.SG),
                    Expression.Lambda(
                        Expression.Invoke(
                            Expression.Parameter(
                                typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQbservable<int>>),
                                Constants.Observable.ZS
                            ),
                            lambdaParameters[0],
                            lambdaParameters[1],
                            lambdaParameters[2],
                            lambdaParameters[3],
                            lambdaParameters[4],
                            lambdaParameters[5],
                            lambdaParameters[6],
                            lambdaParameters[7],
                            lambdaParameters[8],
                            lambdaParameters[9],
                            lambdaParameters[10],
                            lambdaParameters[11],
                            lambdaParameters[12]
                        ),
                        lambdaParameters[0],
                        lambdaParameters[1],
                        lambdaParameters[2],
                        lambdaParameters[3],
                        lambdaParameters[4],
                        lambdaParameters[5],
                        lambdaParameters[6],
                        lambdaParameters[7],
                        lambdaParameters[8],
                        lambdaParameters[9],
                        lambdaParameters[10],
                        lambdaParameters[11],
                        lambdaParameters[12]
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_DefineObservableAsync_14P()
        {
            var lambdaParameters = new[] {
                Expression.Parameter(typeof(string), "parameter_1"),
                Expression.Parameter(typeof(string), "parameter_2"),
                Expression.Parameter(typeof(string), "parameter_3"),
                Expression.Parameter(typeof(string), "parameter_4"),
                Expression.Parameter(typeof(string), "parameter_5"),
                Expression.Parameter(typeof(string), "parameter_6"),
                Expression.Parameter(typeof(string), "parameter_7"),
                Expression.Parameter(typeof(string), "parameter_8"),
                Expression.Parameter(typeof(string), "parameter_9"),
                Expression.Parameter(typeof(string), "parameter_10"),
                Expression.Parameter(typeof(string), "parameter_11"),
                Expression.Parameter(typeof(string), "parameter_12"),
                Expression.Parameter(typeof(string), "parameter_13"),
                Expression.Parameter(typeof(string), "parameter_14")
            };

            Apply(
                ctx =>
                {
                    var obs = ctx.GetObservable<string, string, string, string, string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observable.ZS));
                    ctx.DefineObservableAsync<string, string, string, string, string, string, string, string, string, string, string, string, string, string, int>(
                        new Uri(Constants.StreamFactory.SG),
                        (parameter_1, parameter_2, parameter_3, parameter_4, parameter_5, parameter_6, parameter_7, parameter_8, parameter_9, parameter_10, parameter_11, parameter_12, parameter_13, parameter_14) => obs(parameter_1, parameter_2, parameter_3, parameter_4, parameter_5, parameter_6, parameter_7, parameter_8, parameter_9, parameter_10, parameter_11, parameter_12, parameter_13, parameter_14),
                        state: null,
                        CancellationToken.None
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineObservableAsync<string, string, string, string, string, string, string, string, string, string, string, string, string, string, int>(
                            uri: null,
                            (parameter_1, parameter_2, parameter_3, parameter_4, parameter_5, parameter_6, parameter_7, parameter_8, parameter_9, parameter_10, parameter_11, parameter_12, parameter_13, parameter_14) => obs(parameter_1, parameter_2, parameter_3, parameter_4, parameter_5, parameter_6, parameter_7, parameter_8, parameter_9, parameter_10, parameter_11, parameter_12, parameter_13, parameter_14),
                            state: null,
                            CancellationToken.None
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineObservableAsync<string, string, string, string, string, string, string, string, string, string, string, string, string, string, int>(
                            new Uri(Constants.StreamFactory.SG),
                            observable: null,
                            state: null,
                            CancellationToken.None
                        )
                    );
                },
                new DefineObservable(
                    new Uri(Constants.StreamFactory.SG),
                    Expression.Lambda(
                        Expression.Invoke(
                            Expression.Parameter(
                                typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQbservable<int>>),
                                Constants.Observable.ZS
                            ),
                            lambdaParameters[0],
                            lambdaParameters[1],
                            lambdaParameters[2],
                            lambdaParameters[3],
                            lambdaParameters[4],
                            lambdaParameters[5],
                            lambdaParameters[6],
                            lambdaParameters[7],
                            lambdaParameters[8],
                            lambdaParameters[9],
                            lambdaParameters[10],
                            lambdaParameters[11],
                            lambdaParameters[12],
                            lambdaParameters[13]
                        ),
                        lambdaParameters[0],
                        lambdaParameters[1],
                        lambdaParameters[2],
                        lambdaParameters[3],
                        lambdaParameters[4],
                        lambdaParameters[5],
                        lambdaParameters[6],
                        lambdaParameters[7],
                        lambdaParameters[8],
                        lambdaParameters[9],
                        lambdaParameters[10],
                        lambdaParameters[11],
                        lambdaParameters[12],
                        lambdaParameters[13]
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_DefineObservableAsync_15P()
        {
            var lambdaParameters = new[] {
                Expression.Parameter(typeof(string), "parameter_1"),
                Expression.Parameter(typeof(string), "parameter_2"),
                Expression.Parameter(typeof(string), "parameter_3"),
                Expression.Parameter(typeof(string), "parameter_4"),
                Expression.Parameter(typeof(string), "parameter_5"),
                Expression.Parameter(typeof(string), "parameter_6"),
                Expression.Parameter(typeof(string), "parameter_7"),
                Expression.Parameter(typeof(string), "parameter_8"),
                Expression.Parameter(typeof(string), "parameter_9"),
                Expression.Parameter(typeof(string), "parameter_10"),
                Expression.Parameter(typeof(string), "parameter_11"),
                Expression.Parameter(typeof(string), "parameter_12"),
                Expression.Parameter(typeof(string), "parameter_13"),
                Expression.Parameter(typeof(string), "parameter_14"),
                Expression.Parameter(typeof(string), "parameter_15")
            };

            Apply(
                ctx =>
                {
                    var obs = ctx.GetObservable<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observable.ZS));
                    ctx.DefineObservableAsync<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, int>(
                        new Uri(Constants.StreamFactory.SG),
                        (parameter_1, parameter_2, parameter_3, parameter_4, parameter_5, parameter_6, parameter_7, parameter_8, parameter_9, parameter_10, parameter_11, parameter_12, parameter_13, parameter_14, parameter_15) => obs(parameter_1, parameter_2, parameter_3, parameter_4, parameter_5, parameter_6, parameter_7, parameter_8, parameter_9, parameter_10, parameter_11, parameter_12, parameter_13, parameter_14, parameter_15),
                        state: null,
                        CancellationToken.None
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineObservableAsync<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, int>(
                            uri: null,
                            (parameter_1, parameter_2, parameter_3, parameter_4, parameter_5, parameter_6, parameter_7, parameter_8, parameter_9, parameter_10, parameter_11, parameter_12, parameter_13, parameter_14, parameter_15) => obs(parameter_1, parameter_2, parameter_3, parameter_4, parameter_5, parameter_6, parameter_7, parameter_8, parameter_9, parameter_10, parameter_11, parameter_12, parameter_13, parameter_14, parameter_15),
                            state: null,
                            CancellationToken.None
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineObservableAsync<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, int>(
                            new Uri(Constants.StreamFactory.SG),
                            observable: null,
                            state: null,
                            CancellationToken.None
                        )
                    );
                },
                new DefineObservable(
                    new Uri(Constants.StreamFactory.SG),
                    Expression.Lambda(
                        Expression.Invoke(
                            Expression.Parameter(
                                typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQbservable<int>>),
                                Constants.Observable.ZS
                            ),
                            lambdaParameters[0],
                            lambdaParameters[1],
                            lambdaParameters[2],
                            lambdaParameters[3],
                            lambdaParameters[4],
                            lambdaParameters[5],
                            lambdaParameters[6],
                            lambdaParameters[7],
                            lambdaParameters[8],
                            lambdaParameters[9],
                            lambdaParameters[10],
                            lambdaParameters[11],
                            lambdaParameters[12],
                            lambdaParameters[13],
                            lambdaParameters[14]
                        ),
                        lambdaParameters[0],
                        lambdaParameters[1],
                        lambdaParameters[2],
                        lambdaParameters[3],
                        lambdaParameters[4],
                        lambdaParameters[5],
                        lambdaParameters[6],
                        lambdaParameters[7],
                        lambdaParameters[8],
                        lambdaParameters[9],
                        lambdaParameters[10],
                        lambdaParameters[11],
                        lambdaParameters[12],
                        lambdaParameters[13],
                        lambdaParameters[14]
                    ),
                    state: null
                )
            );
        }

        #endregion

        #region "ReactiveClientContext_DefineObserverAsync"

        [TestMethod]
        public void ReactiveClientContext_DefineObserverAsync_2P()
        {
            var lambdaParameters = new[] {
                Expression.Parameter(typeof(string), "parameter_1"),
                Expression.Parameter(typeof(string), "parameter_2")
            };

            Apply(
                ctx =>
                {
                    var obs = ctx.GetObserver<string, string, int>(new Uri(Constants.Observable.ZS));
                    var obsExpr = (Expression<Func<string, string, IAsyncReactiveQbserver<int>>>)((parameter_1, parameter_2) => obs(parameter_1, parameter_2));
                    ctx.DefineObserverAsync<string, string, int>(new Uri(Constants.StreamFactory.SG), obsExpr, state: null, CancellationToken.None);

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineObserverAsync<string, string, int>(
                            uri: null,
                            obsExpr,
                            state: null,
                            CancellationToken.None
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineObserverAsync<string, string, int>(
                            new Uri(Constants.StreamFactory.SG),
                            observer: null,
                            state: null,
                            CancellationToken.None
                        )
                    );
                },
                new DefineObserver(
                    new Uri(Constants.StreamFactory.SG),
                    Expression.Lambda(
                        Expression.Invoke(
                            Expression.Parameter(
                                typeof(Func<string, string, IAsyncReactiveQbserver<int>>),
                                Constants.Observable.ZS
                            ),
                            lambdaParameters[0],
                            lambdaParameters[1]
                    ),
                        lambdaParameters[0],
                        lambdaParameters[1]
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_DefineObserverAsync_3P()
        {
            var lambdaParameters = new[] {
                Expression.Parameter(typeof(string), "parameter_1"),
                Expression.Parameter(typeof(string), "parameter_2"),
                Expression.Parameter(typeof(string), "parameter_3")
            };

            Apply(
                ctx =>
                {
                    var obs = ctx.GetObserver<string, string, string, int>(new Uri(Constants.Observable.ZS));
                    var obsExpr = (Expression<Func<string, string, string, IAsyncReactiveQbserver<int>>>)((parameter_1, parameter_2, parameter_3) => obs(parameter_1, parameter_2, parameter_3));
                    ctx.DefineObserverAsync<string, string, string, int>(new Uri(Constants.StreamFactory.SG), obsExpr, state: null, CancellationToken.None);

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineObserverAsync<string, string, string, int>(
                            uri: null,
                            obsExpr,
                            state: null,
                            CancellationToken.None
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineObserverAsync<string, string, string, int>(
                            new Uri(Constants.StreamFactory.SG),
                            observer: null,
                            state: null,
                            CancellationToken.None
                        )
                    );
                },
                new DefineObserver(
                    new Uri(Constants.StreamFactory.SG),
                    Expression.Lambda(
                        Expression.Invoke(
                            Expression.Parameter(
                                typeof(Func<string, string, string, IAsyncReactiveQbserver<int>>),
                                Constants.Observable.ZS
                            ),
                            lambdaParameters[0],
                            lambdaParameters[1],
                            lambdaParameters[2]
                    ),
                        lambdaParameters[0],
                        lambdaParameters[1],
                        lambdaParameters[2]
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_DefineObserverAsync_4P()
        {
            var lambdaParameters = new[] {
                Expression.Parameter(typeof(string), "parameter_1"),
                Expression.Parameter(typeof(string), "parameter_2"),
                Expression.Parameter(typeof(string), "parameter_3"),
                Expression.Parameter(typeof(string), "parameter_4")
            };

            Apply(
                ctx =>
                {
                    var obs = ctx.GetObserver<string, string, string, string, int>(new Uri(Constants.Observable.ZS));
                    var obsExpr = (Expression<Func<string, string, string, string, IAsyncReactiveQbserver<int>>>)((parameter_1, parameter_2, parameter_3, parameter_4) => obs(parameter_1, parameter_2, parameter_3, parameter_4));
                    ctx.DefineObserverAsync<string, string, string, string, int>(new Uri(Constants.StreamFactory.SG), obsExpr, state: null, CancellationToken.None);

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineObserverAsync<string, string, string, string, int>(
                            uri: null,
                            obsExpr,
                            state: null,
                            CancellationToken.None
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineObserverAsync<string, string, string, string, int>(
                            new Uri(Constants.StreamFactory.SG),
                            observer: null,
                            state: null,
                            CancellationToken.None
                        )
                    );
                },
                new DefineObserver(
                    new Uri(Constants.StreamFactory.SG),
                    Expression.Lambda(
                        Expression.Invoke(
                            Expression.Parameter(
                                typeof(Func<string, string, string, string, IAsyncReactiveQbserver<int>>),
                                Constants.Observable.ZS
                            ),
                            lambdaParameters[0],
                            lambdaParameters[1],
                            lambdaParameters[2],
                            lambdaParameters[3]
                    ),
                        lambdaParameters[0],
                        lambdaParameters[1],
                        lambdaParameters[2],
                        lambdaParameters[3]
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_DefineObserverAsync_5P()
        {
            var lambdaParameters = new[] {
                Expression.Parameter(typeof(string), "parameter_1"),
                Expression.Parameter(typeof(string), "parameter_2"),
                Expression.Parameter(typeof(string), "parameter_3"),
                Expression.Parameter(typeof(string), "parameter_4"),
                Expression.Parameter(typeof(string), "parameter_5")
            };

            Apply(
                ctx =>
                {
                    var obs = ctx.GetObserver<string, string, string, string, string, int>(new Uri(Constants.Observable.ZS));
                    var obsExpr = (Expression<Func<string, string, string, string, string, IAsyncReactiveQbserver<int>>>)((parameter_1, parameter_2, parameter_3, parameter_4, parameter_5) => obs(parameter_1, parameter_2, parameter_3, parameter_4, parameter_5));
                    ctx.DefineObserverAsync<string, string, string, string, string, int>(new Uri(Constants.StreamFactory.SG), obsExpr, state: null, CancellationToken.None);

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineObserverAsync<string, string, string, string, string, int>(
                            uri: null,
                            obsExpr,
                            state: null,
                            CancellationToken.None
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineObserverAsync<string, string, string, string, string, int>(
                            new Uri(Constants.StreamFactory.SG),
                            observer: null,
                            state: null,
                            CancellationToken.None
                        )
                    );
                },
                new DefineObserver(
                    new Uri(Constants.StreamFactory.SG),
                    Expression.Lambda(
                        Expression.Invoke(
                            Expression.Parameter(
                                typeof(Func<string, string, string, string, string, IAsyncReactiveQbserver<int>>),
                                Constants.Observable.ZS
                            ),
                            lambdaParameters[0],
                            lambdaParameters[1],
                            lambdaParameters[2],
                            lambdaParameters[3],
                            lambdaParameters[4]
                    ),
                        lambdaParameters[0],
                        lambdaParameters[1],
                        lambdaParameters[2],
                        lambdaParameters[3],
                        lambdaParameters[4]
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_DefineObserverAsync_6P()
        {
            var lambdaParameters = new[] {
                Expression.Parameter(typeof(string), "parameter_1"),
                Expression.Parameter(typeof(string), "parameter_2"),
                Expression.Parameter(typeof(string), "parameter_3"),
                Expression.Parameter(typeof(string), "parameter_4"),
                Expression.Parameter(typeof(string), "parameter_5"),
                Expression.Parameter(typeof(string), "parameter_6")
            };

            Apply(
                ctx =>
                {
                    var obs = ctx.GetObserver<string, string, string, string, string, string, int>(new Uri(Constants.Observable.ZS));
                    var obsExpr = (Expression<Func<string, string, string, string, string, string, IAsyncReactiveQbserver<int>>>)((parameter_1, parameter_2, parameter_3, parameter_4, parameter_5, parameter_6) => obs(parameter_1, parameter_2, parameter_3, parameter_4, parameter_5, parameter_6));
                    ctx.DefineObserverAsync<string, string, string, string, string, string, int>(new Uri(Constants.StreamFactory.SG), obsExpr, state: null, CancellationToken.None);

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineObserverAsync<string, string, string, string, string, string, int>(
                            uri: null,
                            obsExpr,
                            state: null,
                            CancellationToken.None
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineObserverAsync<string, string, string, string, string, string, int>(
                            new Uri(Constants.StreamFactory.SG),
                            observer: null,
                            state: null,
                            CancellationToken.None
                        )
                    );
                },
                new DefineObserver(
                    new Uri(Constants.StreamFactory.SG),
                    Expression.Lambda(
                        Expression.Invoke(
                            Expression.Parameter(
                                typeof(Func<string, string, string, string, string, string, IAsyncReactiveQbserver<int>>),
                                Constants.Observable.ZS
                            ),
                            lambdaParameters[0],
                            lambdaParameters[1],
                            lambdaParameters[2],
                            lambdaParameters[3],
                            lambdaParameters[4],
                            lambdaParameters[5]
                    ),
                        lambdaParameters[0],
                        lambdaParameters[1],
                        lambdaParameters[2],
                        lambdaParameters[3],
                        lambdaParameters[4],
                        lambdaParameters[5]
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_DefineObserverAsync_7P()
        {
            var lambdaParameters = new[] {
                Expression.Parameter(typeof(string), "parameter_1"),
                Expression.Parameter(typeof(string), "parameter_2"),
                Expression.Parameter(typeof(string), "parameter_3"),
                Expression.Parameter(typeof(string), "parameter_4"),
                Expression.Parameter(typeof(string), "parameter_5"),
                Expression.Parameter(typeof(string), "parameter_6"),
                Expression.Parameter(typeof(string), "parameter_7")
            };

            Apply(
                ctx =>
                {
                    var obs = ctx.GetObserver<string, string, string, string, string, string, string, int>(new Uri(Constants.Observable.ZS));
                    var obsExpr = (Expression<Func<string, string, string, string, string, string, string, IAsyncReactiveQbserver<int>>>)((parameter_1, parameter_2, parameter_3, parameter_4, parameter_5, parameter_6, parameter_7) => obs(parameter_1, parameter_2, parameter_3, parameter_4, parameter_5, parameter_6, parameter_7));
                    ctx.DefineObserverAsync<string, string, string, string, string, string, string, int>(new Uri(Constants.StreamFactory.SG), obsExpr, state: null, CancellationToken.None);

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineObserverAsync<string, string, string, string, string, string, string, int>(
                            uri: null,
                            obsExpr,
                            state: null,
                            CancellationToken.None
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineObserverAsync<string, string, string, string, string, string, string, int>(
                            new Uri(Constants.StreamFactory.SG),
                            observer: null,
                            state: null,
                            CancellationToken.None
                        )
                    );
                },
                new DefineObserver(
                    new Uri(Constants.StreamFactory.SG),
                    Expression.Lambda(
                        Expression.Invoke(
                            Expression.Parameter(
                                typeof(Func<string, string, string, string, string, string, string, IAsyncReactiveQbserver<int>>),
                                Constants.Observable.ZS
                            ),
                            lambdaParameters[0],
                            lambdaParameters[1],
                            lambdaParameters[2],
                            lambdaParameters[3],
                            lambdaParameters[4],
                            lambdaParameters[5],
                            lambdaParameters[6]
                    ),
                        lambdaParameters[0],
                        lambdaParameters[1],
                        lambdaParameters[2],
                        lambdaParameters[3],
                        lambdaParameters[4],
                        lambdaParameters[5],
                        lambdaParameters[6]
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_DefineObserverAsync_8P()
        {
            var lambdaParameters = new[] {
                Expression.Parameter(typeof(string), "parameter_1"),
                Expression.Parameter(typeof(string), "parameter_2"),
                Expression.Parameter(typeof(string), "parameter_3"),
                Expression.Parameter(typeof(string), "parameter_4"),
                Expression.Parameter(typeof(string), "parameter_5"),
                Expression.Parameter(typeof(string), "parameter_6"),
                Expression.Parameter(typeof(string), "parameter_7"),
                Expression.Parameter(typeof(string), "parameter_8")
            };

            Apply(
                ctx =>
                {
                    var obs = ctx.GetObserver<string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observable.ZS));
                    var obsExpr = (Expression<Func<string, string, string, string, string, string, string, string, IAsyncReactiveQbserver<int>>>)((parameter_1, parameter_2, parameter_3, parameter_4, parameter_5, parameter_6, parameter_7, parameter_8) => obs(parameter_1, parameter_2, parameter_3, parameter_4, parameter_5, parameter_6, parameter_7, parameter_8));
                    ctx.DefineObserverAsync<string, string, string, string, string, string, string, string, int>(new Uri(Constants.StreamFactory.SG), obsExpr, state: null, CancellationToken.None);

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineObserverAsync<string, string, string, string, string, string, string, string, int>(
                            uri: null,
                            obsExpr,
                            state: null,
                            CancellationToken.None
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineObserverAsync<string, string, string, string, string, string, string, string, int>(
                            new Uri(Constants.StreamFactory.SG),
                            observer: null,
                            state: null,
                            CancellationToken.None
                        )
                    );
                },
                new DefineObserver(
                    new Uri(Constants.StreamFactory.SG),
                    Expression.Lambda(
                        Expression.Invoke(
                            Expression.Parameter(
                                typeof(Func<string, string, string, string, string, string, string, string, IAsyncReactiveQbserver<int>>),
                                Constants.Observable.ZS
                            ),
                            lambdaParameters[0],
                            lambdaParameters[1],
                            lambdaParameters[2],
                            lambdaParameters[3],
                            lambdaParameters[4],
                            lambdaParameters[5],
                            lambdaParameters[6],
                            lambdaParameters[7]
                    ),
                        lambdaParameters[0],
                        lambdaParameters[1],
                        lambdaParameters[2],
                        lambdaParameters[3],
                        lambdaParameters[4],
                        lambdaParameters[5],
                        lambdaParameters[6],
                        lambdaParameters[7]
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_DefineObserverAsync_9P()
        {
            var lambdaParameters = new[] {
                Expression.Parameter(typeof(string), "parameter_1"),
                Expression.Parameter(typeof(string), "parameter_2"),
                Expression.Parameter(typeof(string), "parameter_3"),
                Expression.Parameter(typeof(string), "parameter_4"),
                Expression.Parameter(typeof(string), "parameter_5"),
                Expression.Parameter(typeof(string), "parameter_6"),
                Expression.Parameter(typeof(string), "parameter_7"),
                Expression.Parameter(typeof(string), "parameter_8"),
                Expression.Parameter(typeof(string), "parameter_9")
            };

            Apply(
                ctx =>
                {
                    var obs = ctx.GetObserver<string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observable.ZS));
                    var obsExpr = (Expression<Func<string, string, string, string, string, string, string, string, string, IAsyncReactiveQbserver<int>>>)((parameter_1, parameter_2, parameter_3, parameter_4, parameter_5, parameter_6, parameter_7, parameter_8, parameter_9) => obs(parameter_1, parameter_2, parameter_3, parameter_4, parameter_5, parameter_6, parameter_7, parameter_8, parameter_9));
                    ctx.DefineObserverAsync<string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.StreamFactory.SG), obsExpr, state: null, CancellationToken.None);

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineObserverAsync<string, string, string, string, string, string, string, string, string, int>(
                            uri: null,
                            obsExpr,
                            state: null,
                            CancellationToken.None
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineObserverAsync<string, string, string, string, string, string, string, string, string, int>(
                            new Uri(Constants.StreamFactory.SG),
                            observer: null,
                            state: null,
                            CancellationToken.None
                        )
                    );
                },
                new DefineObserver(
                    new Uri(Constants.StreamFactory.SG),
                    Expression.Lambda(
                        Expression.Invoke(
                            Expression.Parameter(
                                typeof(Func<string, string, string, string, string, string, string, string, string, IAsyncReactiveQbserver<int>>),
                                Constants.Observable.ZS
                            ),
                            lambdaParameters[0],
                            lambdaParameters[1],
                            lambdaParameters[2],
                            lambdaParameters[3],
                            lambdaParameters[4],
                            lambdaParameters[5],
                            lambdaParameters[6],
                            lambdaParameters[7],
                            lambdaParameters[8]
                    ),
                        lambdaParameters[0],
                        lambdaParameters[1],
                        lambdaParameters[2],
                        lambdaParameters[3],
                        lambdaParameters[4],
                        lambdaParameters[5],
                        lambdaParameters[6],
                        lambdaParameters[7],
                        lambdaParameters[8]
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_DefineObserverAsync_10P()
        {
            var lambdaParameters = new[] {
                Expression.Parameter(typeof(string), "parameter_1"),
                Expression.Parameter(typeof(string), "parameter_2"),
                Expression.Parameter(typeof(string), "parameter_3"),
                Expression.Parameter(typeof(string), "parameter_4"),
                Expression.Parameter(typeof(string), "parameter_5"),
                Expression.Parameter(typeof(string), "parameter_6"),
                Expression.Parameter(typeof(string), "parameter_7"),
                Expression.Parameter(typeof(string), "parameter_8"),
                Expression.Parameter(typeof(string), "parameter_9"),
                Expression.Parameter(typeof(string), "parameter_10")
            };

            Apply(
                ctx =>
                {
                    var obs = ctx.GetObserver<string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observable.ZS));
                    var obsExpr = (Expression<Func<string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQbserver<int>>>)((parameter_1, parameter_2, parameter_3, parameter_4, parameter_5, parameter_6, parameter_7, parameter_8, parameter_9, parameter_10) => obs(parameter_1, parameter_2, parameter_3, parameter_4, parameter_5, parameter_6, parameter_7, parameter_8, parameter_9, parameter_10));
                    ctx.DefineObserverAsync<string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.StreamFactory.SG), obsExpr, state: null, CancellationToken.None);

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineObserverAsync<string, string, string, string, string, string, string, string, string, string, int>(
                            uri: null,
                            obsExpr,
                            state: null,
                            CancellationToken.None
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineObserverAsync<string, string, string, string, string, string, string, string, string, string, int>(
                            new Uri(Constants.StreamFactory.SG),
                            observer: null,
                            state: null,
                            CancellationToken.None
                        )
                    );
                },
                new DefineObserver(
                    new Uri(Constants.StreamFactory.SG),
                    Expression.Lambda(
                        Expression.Invoke(
                            Expression.Parameter(
                                typeof(Func<string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQbserver<int>>),
                                Constants.Observable.ZS
                            ),
                            lambdaParameters[0],
                            lambdaParameters[1],
                            lambdaParameters[2],
                            lambdaParameters[3],
                            lambdaParameters[4],
                            lambdaParameters[5],
                            lambdaParameters[6],
                            lambdaParameters[7],
                            lambdaParameters[8],
                            lambdaParameters[9]
                    ),
                        lambdaParameters[0],
                        lambdaParameters[1],
                        lambdaParameters[2],
                        lambdaParameters[3],
                        lambdaParameters[4],
                        lambdaParameters[5],
                        lambdaParameters[6],
                        lambdaParameters[7],
                        lambdaParameters[8],
                        lambdaParameters[9]
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_DefineObserverAsync_11P()
        {
            var lambdaParameters = new[] {
                Expression.Parameter(typeof(string), "parameter_1"),
                Expression.Parameter(typeof(string), "parameter_2"),
                Expression.Parameter(typeof(string), "parameter_3"),
                Expression.Parameter(typeof(string), "parameter_4"),
                Expression.Parameter(typeof(string), "parameter_5"),
                Expression.Parameter(typeof(string), "parameter_6"),
                Expression.Parameter(typeof(string), "parameter_7"),
                Expression.Parameter(typeof(string), "parameter_8"),
                Expression.Parameter(typeof(string), "parameter_9"),
                Expression.Parameter(typeof(string), "parameter_10"),
                Expression.Parameter(typeof(string), "parameter_11")
            };

            Apply(
                ctx =>
                {
                    var obs = ctx.GetObserver<string, string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observable.ZS));
                    var obsExpr = (Expression<Func<string, string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQbserver<int>>>)((parameter_1, parameter_2, parameter_3, parameter_4, parameter_5, parameter_6, parameter_7, parameter_8, parameter_9, parameter_10, parameter_11) => obs(parameter_1, parameter_2, parameter_3, parameter_4, parameter_5, parameter_6, parameter_7, parameter_8, parameter_9, parameter_10, parameter_11));
                    ctx.DefineObserverAsync<string, string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.StreamFactory.SG), obsExpr, state: null, CancellationToken.None);

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineObserverAsync<string, string, string, string, string, string, string, string, string, string, string, int>(
                            uri: null,
                            obsExpr,
                            state: null,
                            CancellationToken.None
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineObserverAsync<string, string, string, string, string, string, string, string, string, string, string, int>(
                            new Uri(Constants.StreamFactory.SG),
                            observer: null,
                            state: null,
                            CancellationToken.None
                        )
                    );
                },
                new DefineObserver(
                    new Uri(Constants.StreamFactory.SG),
                    Expression.Lambda(
                        Expression.Invoke(
                            Expression.Parameter(
                                typeof(Func<string, string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQbserver<int>>),
                                Constants.Observable.ZS
                            ),
                            lambdaParameters[0],
                            lambdaParameters[1],
                            lambdaParameters[2],
                            lambdaParameters[3],
                            lambdaParameters[4],
                            lambdaParameters[5],
                            lambdaParameters[6],
                            lambdaParameters[7],
                            lambdaParameters[8],
                            lambdaParameters[9],
                            lambdaParameters[10]
                    ),
                        lambdaParameters[0],
                        lambdaParameters[1],
                        lambdaParameters[2],
                        lambdaParameters[3],
                        lambdaParameters[4],
                        lambdaParameters[5],
                        lambdaParameters[6],
                        lambdaParameters[7],
                        lambdaParameters[8],
                        lambdaParameters[9],
                        lambdaParameters[10]
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_DefineObserverAsync_12P()
        {
            var lambdaParameters = new[] {
                Expression.Parameter(typeof(string), "parameter_1"),
                Expression.Parameter(typeof(string), "parameter_2"),
                Expression.Parameter(typeof(string), "parameter_3"),
                Expression.Parameter(typeof(string), "parameter_4"),
                Expression.Parameter(typeof(string), "parameter_5"),
                Expression.Parameter(typeof(string), "parameter_6"),
                Expression.Parameter(typeof(string), "parameter_7"),
                Expression.Parameter(typeof(string), "parameter_8"),
                Expression.Parameter(typeof(string), "parameter_9"),
                Expression.Parameter(typeof(string), "parameter_10"),
                Expression.Parameter(typeof(string), "parameter_11"),
                Expression.Parameter(typeof(string), "parameter_12")
            };

            Apply(
                ctx =>
                {
                    var obs = ctx.GetObserver<string, string, string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observable.ZS));
                    var obsExpr = (Expression<Func<string, string, string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQbserver<int>>>)((parameter_1, parameter_2, parameter_3, parameter_4, parameter_5, parameter_6, parameter_7, parameter_8, parameter_9, parameter_10, parameter_11, parameter_12) => obs(parameter_1, parameter_2, parameter_3, parameter_4, parameter_5, parameter_6, parameter_7, parameter_8, parameter_9, parameter_10, parameter_11, parameter_12));
                    ctx.DefineObserverAsync<string, string, string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.StreamFactory.SG), obsExpr, state: null, CancellationToken.None);

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineObserverAsync<string, string, string, string, string, string, string, string, string, string, string, string, int>(
                            uri: null,
                            obsExpr,
                            state: null,
                            CancellationToken.None
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineObserverAsync<string, string, string, string, string, string, string, string, string, string, string, string, int>(
                            new Uri(Constants.StreamFactory.SG),
                            observer: null,
                            state: null,
                            CancellationToken.None
                        )
                    );
                },
                new DefineObserver(
                    new Uri(Constants.StreamFactory.SG),
                    Expression.Lambda(
                        Expression.Invoke(
                            Expression.Parameter(
                                typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQbserver<int>>),
                                Constants.Observable.ZS
                            ),
                            lambdaParameters[0],
                            lambdaParameters[1],
                            lambdaParameters[2],
                            lambdaParameters[3],
                            lambdaParameters[4],
                            lambdaParameters[5],
                            lambdaParameters[6],
                            lambdaParameters[7],
                            lambdaParameters[8],
                            lambdaParameters[9],
                            lambdaParameters[10],
                            lambdaParameters[11]
                    ),
                        lambdaParameters[0],
                        lambdaParameters[1],
                        lambdaParameters[2],
                        lambdaParameters[3],
                        lambdaParameters[4],
                        lambdaParameters[5],
                        lambdaParameters[6],
                        lambdaParameters[7],
                        lambdaParameters[8],
                        lambdaParameters[9],
                        lambdaParameters[10],
                        lambdaParameters[11]
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_DefineObserverAsync_13P()
        {
            var lambdaParameters = new[] {
                Expression.Parameter(typeof(string), "parameter_1"),
                Expression.Parameter(typeof(string), "parameter_2"),
                Expression.Parameter(typeof(string), "parameter_3"),
                Expression.Parameter(typeof(string), "parameter_4"),
                Expression.Parameter(typeof(string), "parameter_5"),
                Expression.Parameter(typeof(string), "parameter_6"),
                Expression.Parameter(typeof(string), "parameter_7"),
                Expression.Parameter(typeof(string), "parameter_8"),
                Expression.Parameter(typeof(string), "parameter_9"),
                Expression.Parameter(typeof(string), "parameter_10"),
                Expression.Parameter(typeof(string), "parameter_11"),
                Expression.Parameter(typeof(string), "parameter_12"),
                Expression.Parameter(typeof(string), "parameter_13")
            };

            Apply(
                ctx =>
                {
                    var obs = ctx.GetObserver<string, string, string, string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observable.ZS));
                    var obsExpr = (Expression<Func<string, string, string, string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQbserver<int>>>)((parameter_1, parameter_2, parameter_3, parameter_4, parameter_5, parameter_6, parameter_7, parameter_8, parameter_9, parameter_10, parameter_11, parameter_12, parameter_13) => obs(parameter_1, parameter_2, parameter_3, parameter_4, parameter_5, parameter_6, parameter_7, parameter_8, parameter_9, parameter_10, parameter_11, parameter_12, parameter_13));
                    ctx.DefineObserverAsync<string, string, string, string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.StreamFactory.SG), obsExpr, state: null, CancellationToken.None);

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineObserverAsync<string, string, string, string, string, string, string, string, string, string, string, string, string, int>(
                            uri: null,
                            obsExpr,
                            state: null,
                            CancellationToken.None
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineObserverAsync<string, string, string, string, string, string, string, string, string, string, string, string, string, int>(
                            new Uri(Constants.StreamFactory.SG),
                            observer: null,
                            state: null,
                            CancellationToken.None
                        )
                    );
                },
                new DefineObserver(
                    new Uri(Constants.StreamFactory.SG),
                    Expression.Lambda(
                        Expression.Invoke(
                            Expression.Parameter(
                                typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQbserver<int>>),
                                Constants.Observable.ZS
                            ),
                            lambdaParameters[0],
                            lambdaParameters[1],
                            lambdaParameters[2],
                            lambdaParameters[3],
                            lambdaParameters[4],
                            lambdaParameters[5],
                            lambdaParameters[6],
                            lambdaParameters[7],
                            lambdaParameters[8],
                            lambdaParameters[9],
                            lambdaParameters[10],
                            lambdaParameters[11],
                            lambdaParameters[12]
                    ),
                        lambdaParameters[0],
                        lambdaParameters[1],
                        lambdaParameters[2],
                        lambdaParameters[3],
                        lambdaParameters[4],
                        lambdaParameters[5],
                        lambdaParameters[6],
                        lambdaParameters[7],
                        lambdaParameters[8],
                        lambdaParameters[9],
                        lambdaParameters[10],
                        lambdaParameters[11],
                        lambdaParameters[12]
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_DefineObserverAsync_14P()
        {
            var lambdaParameters = new[] {
                Expression.Parameter(typeof(string), "parameter_1"),
                Expression.Parameter(typeof(string), "parameter_2"),
                Expression.Parameter(typeof(string), "parameter_3"),
                Expression.Parameter(typeof(string), "parameter_4"),
                Expression.Parameter(typeof(string), "parameter_5"),
                Expression.Parameter(typeof(string), "parameter_6"),
                Expression.Parameter(typeof(string), "parameter_7"),
                Expression.Parameter(typeof(string), "parameter_8"),
                Expression.Parameter(typeof(string), "parameter_9"),
                Expression.Parameter(typeof(string), "parameter_10"),
                Expression.Parameter(typeof(string), "parameter_11"),
                Expression.Parameter(typeof(string), "parameter_12"),
                Expression.Parameter(typeof(string), "parameter_13"),
                Expression.Parameter(typeof(string), "parameter_14")
            };

            Apply(
                ctx =>
                {
                    var obs = ctx.GetObserver<string, string, string, string, string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observable.ZS));
                    var obsExpr = (Expression<Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQbserver<int>>>)((parameter_1, parameter_2, parameter_3, parameter_4, parameter_5, parameter_6, parameter_7, parameter_8, parameter_9, parameter_10, parameter_11, parameter_12, parameter_13, parameter_14) => obs(parameter_1, parameter_2, parameter_3, parameter_4, parameter_5, parameter_6, parameter_7, parameter_8, parameter_9, parameter_10, parameter_11, parameter_12, parameter_13, parameter_14));
                    ctx.DefineObserverAsync<string, string, string, string, string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.StreamFactory.SG), obsExpr, state: null, CancellationToken.None);

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineObserverAsync<string, string, string, string, string, string, string, string, string, string, string, string, string, string, int>(
                            uri: null,
                            obsExpr,
                            state: null,
                            CancellationToken.None
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineObserverAsync<string, string, string, string, string, string, string, string, string, string, string, string, string, string, int>(
                            new Uri(Constants.StreamFactory.SG),
                            observer: null,
                            state: null,
                            CancellationToken.None
                        )
                    );
                },
                new DefineObserver(
                    new Uri(Constants.StreamFactory.SG),
                    Expression.Lambda(
                        Expression.Invoke(
                            Expression.Parameter(
                                typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQbserver<int>>),
                                Constants.Observable.ZS
                            ),
                            lambdaParameters[0],
                            lambdaParameters[1],
                            lambdaParameters[2],
                            lambdaParameters[3],
                            lambdaParameters[4],
                            lambdaParameters[5],
                            lambdaParameters[6],
                            lambdaParameters[7],
                            lambdaParameters[8],
                            lambdaParameters[9],
                            lambdaParameters[10],
                            lambdaParameters[11],
                            lambdaParameters[12],
                            lambdaParameters[13]
                    ),
                        lambdaParameters[0],
                        lambdaParameters[1],
                        lambdaParameters[2],
                        lambdaParameters[3],
                        lambdaParameters[4],
                        lambdaParameters[5],
                        lambdaParameters[6],
                        lambdaParameters[7],
                        lambdaParameters[8],
                        lambdaParameters[9],
                        lambdaParameters[10],
                        lambdaParameters[11],
                        lambdaParameters[12],
                        lambdaParameters[13]
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_DefineObserverAsync_15P()
        {
            var lambdaParameters = new[] {
                Expression.Parameter(typeof(string), "parameter_1"),
                Expression.Parameter(typeof(string), "parameter_2"),
                Expression.Parameter(typeof(string), "parameter_3"),
                Expression.Parameter(typeof(string), "parameter_4"),
                Expression.Parameter(typeof(string), "parameter_5"),
                Expression.Parameter(typeof(string), "parameter_6"),
                Expression.Parameter(typeof(string), "parameter_7"),
                Expression.Parameter(typeof(string), "parameter_8"),
                Expression.Parameter(typeof(string), "parameter_9"),
                Expression.Parameter(typeof(string), "parameter_10"),
                Expression.Parameter(typeof(string), "parameter_11"),
                Expression.Parameter(typeof(string), "parameter_12"),
                Expression.Parameter(typeof(string), "parameter_13"),
                Expression.Parameter(typeof(string), "parameter_14"),
                Expression.Parameter(typeof(string), "parameter_15")
            };

            Apply(
                ctx =>
                {
                    var obs = ctx.GetObserver<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observable.ZS));
                    var obsExpr = (Expression<Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQbserver<int>>>)((parameter_1, parameter_2, parameter_3, parameter_4, parameter_5, parameter_6, parameter_7, parameter_8, parameter_9, parameter_10, parameter_11, parameter_12, parameter_13, parameter_14, parameter_15) => obs(parameter_1, parameter_2, parameter_3, parameter_4, parameter_5, parameter_6, parameter_7, parameter_8, parameter_9, parameter_10, parameter_11, parameter_12, parameter_13, parameter_14, parameter_15));
                    ctx.DefineObserverAsync<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.StreamFactory.SG), obsExpr, state: null, CancellationToken.None);

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineObserverAsync<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, int>(
                            uri: null,
                            obsExpr,
                            state: null,
                            CancellationToken.None
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineObserverAsync<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, int>(
                            new Uri(Constants.StreamFactory.SG),
                            observer: null,
                            state: null,
                            CancellationToken.None
                        )
                    );
                },
                new DefineObserver(
                    new Uri(Constants.StreamFactory.SG),
                    Expression.Lambda(
                        Expression.Invoke(
                            Expression.Parameter(
                                typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQbserver<int>>),
                                Constants.Observable.ZS
                            ),
                            lambdaParameters[0],
                            lambdaParameters[1],
                            lambdaParameters[2],
                            lambdaParameters[3],
                            lambdaParameters[4],
                            lambdaParameters[5],
                            lambdaParameters[6],
                            lambdaParameters[7],
                            lambdaParameters[8],
                            lambdaParameters[9],
                            lambdaParameters[10],
                            lambdaParameters[11],
                            lambdaParameters[12],
                            lambdaParameters[13],
                            lambdaParameters[14]
                    ),
                        lambdaParameters[0],
                        lambdaParameters[1],
                        lambdaParameters[2],
                        lambdaParameters[3],
                        lambdaParameters[4],
                        lambdaParameters[5],
                        lambdaParameters[6],
                        lambdaParameters[7],
                        lambdaParameters[8],
                        lambdaParameters[9],
                        lambdaParameters[10],
                        lambdaParameters[11],
                        lambdaParameters[12],
                        lambdaParameters[13],
                        lambdaParameters[14]
                    ),
                    state: null
                )
            );
        }

        #endregion

        #region "ReactiveClientContext_DefineStreamFactoryAsync"

        [TestMethod]
        public void ReactiveClientContext_DefineStreamFactoryAsync_2P()
        {
            Apply(
                ctx =>
                {
                    var factory = ctx.GetStreamFactory<string, string, int, int>(new Uri(Constants.StreamFactory.SF));

                    Assert.AreEqual(new Uri(Constants.StreamFactory.SF), ((IKnownResource)factory).Uri);

                    ctx.DefineStreamFactoryAsync<string, string, int, int>(new Uri(Constants.StreamFactory.SG), factory, state: null, CancellationToken.None);

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineStreamFactoryAsync<string, string, int, int>(
                            uri: null,
                            factory,
                            state: null,
                            CancellationToken.None
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineStreamFactoryAsync<string, string, int, int>(
                            new Uri(Constants.StreamFactory.SF),
                            streamFactory: null,
                            state: null,
                            CancellationToken.None
                        )
                    );
                },
                new DefineStreamFactory(
                    new Uri(Constants.StreamFactory.SG),
                    Expression.Parameter(
                        typeof(Func<string, string, IAsyncReactiveQubject<int, int>>),
                        Constants.StreamFactory.SF
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_DefineStreamFactoryAsync_3P()
        {
            Apply(
                ctx =>
                {
                    var factory = ctx.GetStreamFactory<string, string, string, int, int>(new Uri(Constants.StreamFactory.SF));

                    Assert.AreEqual(new Uri(Constants.StreamFactory.SF), ((IKnownResource)factory).Uri);

                    ctx.DefineStreamFactoryAsync<string, string, string, int, int>(new Uri(Constants.StreamFactory.SG), factory, state: null, CancellationToken.None);

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineStreamFactoryAsync<string, string, string, int, int>(
                            uri: null,
                            factory,
                            state: null,
                            CancellationToken.None
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineStreamFactoryAsync<string, string, string, int, int>(
                            new Uri(Constants.StreamFactory.SF),
                            streamFactory: null,
                            state: null,
                            CancellationToken.None
                        )
                    );
                },
                new DefineStreamFactory(
                    new Uri(Constants.StreamFactory.SG),
                    Expression.Parameter(
                        typeof(Func<string, string, string, IAsyncReactiveQubject<int, int>>),
                        Constants.StreamFactory.SF
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_DefineStreamFactoryAsync_4P()
        {
            Apply(
                ctx =>
                {
                    var factory = ctx.GetStreamFactory<string, string, string, string, int, int>(new Uri(Constants.StreamFactory.SF));

                    Assert.AreEqual(new Uri(Constants.StreamFactory.SF), ((IKnownResource)factory).Uri);

                    ctx.DefineStreamFactoryAsync<string, string, string, string, int, int>(new Uri(Constants.StreamFactory.SG), factory, state: null, CancellationToken.None);

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineStreamFactoryAsync<string, string, string, string, int, int>(
                            uri: null,
                            factory,
                            state: null,
                            CancellationToken.None
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineStreamFactoryAsync<string, string, string, string, int, int>(
                            new Uri(Constants.StreamFactory.SF),
                            streamFactory: null,
                            state: null,
                            CancellationToken.None
                        )
                    );
                },
                new DefineStreamFactory(
                    new Uri(Constants.StreamFactory.SG),
                    Expression.Parameter(
                        typeof(Func<string, string, string, string, IAsyncReactiveQubject<int, int>>),
                        Constants.StreamFactory.SF
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_DefineStreamFactoryAsync_5P()
        {
            Apply(
                ctx =>
                {
                    var factory = ctx.GetStreamFactory<string, string, string, string, string, int, int>(new Uri(Constants.StreamFactory.SF));

                    Assert.AreEqual(new Uri(Constants.StreamFactory.SF), ((IKnownResource)factory).Uri);

                    ctx.DefineStreamFactoryAsync<string, string, string, string, string, int, int>(new Uri(Constants.StreamFactory.SG), factory, state: null, CancellationToken.None);

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineStreamFactoryAsync<string, string, string, string, string, int, int>(
                            uri: null,
                            factory,
                            state: null,
                            CancellationToken.None
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineStreamFactoryAsync<string, string, string, string, string, int, int>(
                            new Uri(Constants.StreamFactory.SF),
                            streamFactory: null,
                            state: null,
                            CancellationToken.None
                        )
                    );
                },
                new DefineStreamFactory(
                    new Uri(Constants.StreamFactory.SG),
                    Expression.Parameter(
                        typeof(Func<string, string, string, string, string, IAsyncReactiveQubject<int, int>>),
                        Constants.StreamFactory.SF
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_DefineStreamFactoryAsync_6P()
        {
            Apply(
                ctx =>
                {
                    var factory = ctx.GetStreamFactory<string, string, string, string, string, string, int, int>(new Uri(Constants.StreamFactory.SF));

                    Assert.AreEqual(new Uri(Constants.StreamFactory.SF), ((IKnownResource)factory).Uri);

                    ctx.DefineStreamFactoryAsync<string, string, string, string, string, string, int, int>(new Uri(Constants.StreamFactory.SG), factory, state: null, CancellationToken.None);

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineStreamFactoryAsync<string, string, string, string, string, string, int, int>(
                            uri: null,
                            factory,
                            state: null,
                            CancellationToken.None
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineStreamFactoryAsync<string, string, string, string, string, string, int, int>(
                            new Uri(Constants.StreamFactory.SF),
                            streamFactory: null,
                            state: null,
                            CancellationToken.None
                        )
                    );
                },
                new DefineStreamFactory(
                    new Uri(Constants.StreamFactory.SG),
                    Expression.Parameter(
                        typeof(Func<string, string, string, string, string, string, IAsyncReactiveQubject<int, int>>),
                        Constants.StreamFactory.SF
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_DefineStreamFactoryAsync_7P()
        {
            Apply(
                ctx =>
                {
                    var factory = ctx.GetStreamFactory<string, string, string, string, string, string, string, int, int>(new Uri(Constants.StreamFactory.SF));

                    Assert.AreEqual(new Uri(Constants.StreamFactory.SF), ((IKnownResource)factory).Uri);

                    ctx.DefineStreamFactoryAsync<string, string, string, string, string, string, string, int, int>(new Uri(Constants.StreamFactory.SG), factory, state: null, CancellationToken.None);

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineStreamFactoryAsync<string, string, string, string, string, string, string, int, int>(
                            uri: null,
                            factory,
                            state: null,
                            CancellationToken.None
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineStreamFactoryAsync<string, string, string, string, string, string, string, int, int>(
                            new Uri(Constants.StreamFactory.SF),
                            streamFactory: null,
                            state: null,
                            CancellationToken.None
                        )
                    );
                },
                new DefineStreamFactory(
                    new Uri(Constants.StreamFactory.SG),
                    Expression.Parameter(
                        typeof(Func<string, string, string, string, string, string, string, IAsyncReactiveQubject<int, int>>),
                        Constants.StreamFactory.SF
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_DefineStreamFactoryAsync_8P()
        {
            Apply(
                ctx =>
                {
                    var factory = ctx.GetStreamFactory<string, string, string, string, string, string, string, string, int, int>(new Uri(Constants.StreamFactory.SF));

                    Assert.AreEqual(new Uri(Constants.StreamFactory.SF), ((IKnownResource)factory).Uri);

                    ctx.DefineStreamFactoryAsync<string, string, string, string, string, string, string, string, int, int>(new Uri(Constants.StreamFactory.SG), factory, state: null, CancellationToken.None);

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineStreamFactoryAsync<string, string, string, string, string, string, string, string, int, int>(
                            uri: null,
                            factory,
                            state: null,
                            CancellationToken.None
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineStreamFactoryAsync<string, string, string, string, string, string, string, string, int, int>(
                            new Uri(Constants.StreamFactory.SF),
                            streamFactory: null,
                            state: null,
                            CancellationToken.None
                        )
                    );
                },
                new DefineStreamFactory(
                    new Uri(Constants.StreamFactory.SG),
                    Expression.Parameter(
                        typeof(Func<string, string, string, string, string, string, string, string, IAsyncReactiveQubject<int, int>>),
                        Constants.StreamFactory.SF
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_DefineStreamFactoryAsync_9P()
        {
            Apply(
                ctx =>
                {
                    var factory = ctx.GetStreamFactory<string, string, string, string, string, string, string, string, string, int, int>(new Uri(Constants.StreamFactory.SF));

                    Assert.AreEqual(new Uri(Constants.StreamFactory.SF), ((IKnownResource)factory).Uri);

                    ctx.DefineStreamFactoryAsync<string, string, string, string, string, string, string, string, string, int, int>(new Uri(Constants.StreamFactory.SG), factory, state: null, CancellationToken.None);

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineStreamFactoryAsync<string, string, string, string, string, string, string, string, string, int, int>(
                            uri: null,
                            factory,
                            state: null,
                            CancellationToken.None
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineStreamFactoryAsync<string, string, string, string, string, string, string, string, string, int, int>(
                            new Uri(Constants.StreamFactory.SF),
                            streamFactory: null,
                            state: null,
                            CancellationToken.None
                        )
                    );
                },
                new DefineStreamFactory(
                    new Uri(Constants.StreamFactory.SG),
                    Expression.Parameter(
                        typeof(Func<string, string, string, string, string, string, string, string, string, IAsyncReactiveQubject<int, int>>),
                        Constants.StreamFactory.SF
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_DefineStreamFactoryAsync_10P()
        {
            Apply(
                ctx =>
                {
                    var factory = ctx.GetStreamFactory<string, string, string, string, string, string, string, string, string, string, int, int>(new Uri(Constants.StreamFactory.SF));

                    Assert.AreEqual(new Uri(Constants.StreamFactory.SF), ((IKnownResource)factory).Uri);

                    ctx.DefineStreamFactoryAsync<string, string, string, string, string, string, string, string, string, string, int, int>(new Uri(Constants.StreamFactory.SG), factory, state: null, CancellationToken.None);

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineStreamFactoryAsync<string, string, string, string, string, string, string, string, string, string, int, int>(
                            uri: null,
                            factory,
                            state: null,
                            CancellationToken.None
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineStreamFactoryAsync<string, string, string, string, string, string, string, string, string, string, int, int>(
                            new Uri(Constants.StreamFactory.SF),
                            streamFactory: null,
                            state: null,
                            CancellationToken.None
                        )
                    );
                },
                new DefineStreamFactory(
                    new Uri(Constants.StreamFactory.SG),
                    Expression.Parameter(
                        typeof(Func<string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQubject<int, int>>),
                        Constants.StreamFactory.SF
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_DefineStreamFactoryAsync_11P()
        {
            Apply(
                ctx =>
                {
                    var factory = ctx.GetStreamFactory<string, string, string, string, string, string, string, string, string, string, string, int, int>(new Uri(Constants.StreamFactory.SF));

                    Assert.AreEqual(new Uri(Constants.StreamFactory.SF), ((IKnownResource)factory).Uri);

                    ctx.DefineStreamFactoryAsync<string, string, string, string, string, string, string, string, string, string, string, int, int>(new Uri(Constants.StreamFactory.SG), factory, state: null, CancellationToken.None);

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineStreamFactoryAsync<string, string, string, string, string, string, string, string, string, string, string, int, int>(
                            uri: null,
                            factory,
                            state: null,
                            CancellationToken.None
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineStreamFactoryAsync<string, string, string, string, string, string, string, string, string, string, string, int, int>(
                            new Uri(Constants.StreamFactory.SF),
                            streamFactory: null,
                            state: null,
                            CancellationToken.None
                        )
                    );
                },
                new DefineStreamFactory(
                    new Uri(Constants.StreamFactory.SG),
                    Expression.Parameter(
                        typeof(Func<string, string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQubject<int, int>>),
                        Constants.StreamFactory.SF
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_DefineStreamFactoryAsync_12P()
        {
            Apply(
                ctx =>
                {
                    var factory = ctx.GetStreamFactory<string, string, string, string, string, string, string, string, string, string, string, string, int, int>(new Uri(Constants.StreamFactory.SF));

                    Assert.AreEqual(new Uri(Constants.StreamFactory.SF), ((IKnownResource)factory).Uri);

                    ctx.DefineStreamFactoryAsync<string, string, string, string, string, string, string, string, string, string, string, string, int, int>(new Uri(Constants.StreamFactory.SG), factory, state: null, CancellationToken.None);

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineStreamFactoryAsync<string, string, string, string, string, string, string, string, string, string, string, string, int, int>(
                            uri: null,
                            factory,
                            state: null,
                            CancellationToken.None
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineStreamFactoryAsync<string, string, string, string, string, string, string, string, string, string, string, string, int, int>(
                            new Uri(Constants.StreamFactory.SF),
                            streamFactory: null,
                            state: null,
                            CancellationToken.None
                        )
                    );
                },
                new DefineStreamFactory(
                    new Uri(Constants.StreamFactory.SG),
                    Expression.Parameter(
                        typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQubject<int, int>>),
                        Constants.StreamFactory.SF
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_DefineStreamFactoryAsync_13P()
        {
            Apply(
                ctx =>
                {
                    var factory = ctx.GetStreamFactory<string, string, string, string, string, string, string, string, string, string, string, string, string, int, int>(new Uri(Constants.StreamFactory.SF));

                    Assert.AreEqual(new Uri(Constants.StreamFactory.SF), ((IKnownResource)factory).Uri);

                    ctx.DefineStreamFactoryAsync<string, string, string, string, string, string, string, string, string, string, string, string, string, int, int>(new Uri(Constants.StreamFactory.SG), factory, state: null, CancellationToken.None);

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineStreamFactoryAsync<string, string, string, string, string, string, string, string, string, string, string, string, string, int, int>(
                            uri: null,
                            factory,
                            state: null,
                            CancellationToken.None
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineStreamFactoryAsync<string, string, string, string, string, string, string, string, string, string, string, string, string, int, int>(
                            new Uri(Constants.StreamFactory.SF),
                            streamFactory: null,
                            state: null,
                            CancellationToken.None
                        )
                    );
                },
                new DefineStreamFactory(
                    new Uri(Constants.StreamFactory.SG),
                    Expression.Parameter(
                        typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQubject<int, int>>),
                        Constants.StreamFactory.SF
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_DefineStreamFactoryAsync_14P()
        {
            Apply(
                ctx =>
                {
                    var factory = ctx.GetStreamFactory<string, string, string, string, string, string, string, string, string, string, string, string, string, string, int, int>(new Uri(Constants.StreamFactory.SF));

                    Assert.AreEqual(new Uri(Constants.StreamFactory.SF), ((IKnownResource)factory).Uri);

                    ctx.DefineStreamFactoryAsync<string, string, string, string, string, string, string, string, string, string, string, string, string, string, int, int>(new Uri(Constants.StreamFactory.SG), factory, state: null, CancellationToken.None);

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineStreamFactoryAsync<string, string, string, string, string, string, string, string, string, string, string, string, string, string, int, int>(
                            uri: null,
                            factory,
                            state: null,
                            CancellationToken.None
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineStreamFactoryAsync<string, string, string, string, string, string, string, string, string, string, string, string, string, string, int, int>(
                            new Uri(Constants.StreamFactory.SF),
                            streamFactory: null,
                            state: null,
                            CancellationToken.None
                        )
                    );
                },
                new DefineStreamFactory(
                    new Uri(Constants.StreamFactory.SG),
                    Expression.Parameter(
                        typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQubject<int, int>>),
                        Constants.StreamFactory.SF
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_DefineStreamFactoryAsync_15P()
        {
            Apply(
                ctx =>
                {
                    var factory = ctx.GetStreamFactory<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, int, int>(new Uri(Constants.StreamFactory.SF));

                    Assert.AreEqual(new Uri(Constants.StreamFactory.SF), ((IKnownResource)factory).Uri);

                    ctx.DefineStreamFactoryAsync<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, int, int>(new Uri(Constants.StreamFactory.SG), factory, state: null, CancellationToken.None);

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineStreamFactoryAsync<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, int, int>(
                            uri: null,
                            factory,
                            state: null,
                            CancellationToken.None
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineStreamFactoryAsync<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, int, int>(
                            new Uri(Constants.StreamFactory.SF),
                            streamFactory: null,
                            state: null,
                            CancellationToken.None
                        )
                    );
                },
                new DefineStreamFactory(
                    new Uri(Constants.StreamFactory.SG),
                    Expression.Parameter(
                        typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQubject<int, int>>),
                        Constants.StreamFactory.SF
                    ),
                    state: null
                )
            );
        }

        #endregion

        #region "ReactiveClientContext_DefineSubscriptionFactoryAsync"

        [TestMethod]
        public void ReactiveClientContext_DefineSubscriptionFactoryAsync_2P()
        {
            Apply(
                ctx =>
                {
                    var factory = ctx.GetSubscriptionFactory<string, string>(new Uri(Constants.SubscriptionFactory.SF));

                    Assert.AreEqual(new Uri(Constants.SubscriptionFactory.SF), ((IKnownResource)factory).Uri);

                    ctx.DefineSubscriptionFactoryAsync<string, string>(new Uri(Constants.SubscriptionFactory.SG), factory, state: null, CancellationToken.None);

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineSubscriptionFactoryAsync<string, string>(
                            uri: null,
                            factory,
                            state: null,
                            CancellationToken.None
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineSubscriptionFactoryAsync<string, string>(
                            new Uri(Constants.SubscriptionFactory.SF),
                            default(IAsyncReactiveQubscriptionFactory<string, string>),
                            state: null,
                            CancellationToken.None
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineSubscriptionFactoryAsync<string, string>(
                            new Uri(Constants.SubscriptionFactory.SF),
                            default(Expression<Func<string, string, IAsyncReactiveQubscription>>),
                            state: null,
                            CancellationToken.None
                        )
                    );
                },
                new DefineSubscriptionFactory(
                    new Uri(Constants.SubscriptionFactory.SG),
                    Expression.Parameter(
                        typeof(Func<string, string, IAsyncReactiveQubscription>),
                        Constants.SubscriptionFactory.SF
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_DefineSubscriptionFactoryAsync_3P()
        {
            Apply(
                ctx =>
                {
                    var factory = ctx.GetSubscriptionFactory<string, string, string>(new Uri(Constants.SubscriptionFactory.SF));

                    Assert.AreEqual(new Uri(Constants.SubscriptionFactory.SF), ((IKnownResource)factory).Uri);

                    ctx.DefineSubscriptionFactoryAsync<string, string, string>(new Uri(Constants.SubscriptionFactory.SG), factory, state: null, CancellationToken.None);

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineSubscriptionFactoryAsync<string, string, string>(
                            uri: null,
                            factory,
                            state: null,
                            CancellationToken.None
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineSubscriptionFactoryAsync<string, string, string>(
                            new Uri(Constants.SubscriptionFactory.SF),
                            default(IAsyncReactiveQubscriptionFactory<string, string, string>),
                            state: null,
                            CancellationToken.None
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineSubscriptionFactoryAsync<string, string, string>(
                            new Uri(Constants.SubscriptionFactory.SF),
                            default(Expression<Func<string, string, string, IAsyncReactiveQubscription>>),
                            state: null,
                            CancellationToken.None
                        )
                    );
                },
                new DefineSubscriptionFactory(
                    new Uri(Constants.SubscriptionFactory.SG),
                    Expression.Parameter(
                        typeof(Func<string, string, string, IAsyncReactiveQubscription>),
                        Constants.SubscriptionFactory.SF
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_DefineSubscriptionFactoryAsync_4P()
        {
            Apply(
                ctx =>
                {
                    var factory = ctx.GetSubscriptionFactory<string, string, string, string>(new Uri(Constants.SubscriptionFactory.SF));

                    Assert.AreEqual(new Uri(Constants.SubscriptionFactory.SF), ((IKnownResource)factory).Uri);

                    ctx.DefineSubscriptionFactoryAsync<string, string, string, string>(new Uri(Constants.SubscriptionFactory.SG), factory, state: null, CancellationToken.None);

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineSubscriptionFactoryAsync<string, string, string, string>(
                            uri: null,
                            factory,
                            state: null,
                            CancellationToken.None
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineSubscriptionFactoryAsync<string, string, string, string>(
                            new Uri(Constants.SubscriptionFactory.SF),
                            default(IAsyncReactiveQubscriptionFactory<string, string, string, string>),
                            state: null,
                            CancellationToken.None
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineSubscriptionFactoryAsync<string, string, string, string>(
                            new Uri(Constants.SubscriptionFactory.SF),
                            default(Expression<Func<string, string, string, string, IAsyncReactiveQubscription>>),
                            state: null,
                            CancellationToken.None
                        )
                    );
                },
                new DefineSubscriptionFactory(
                    new Uri(Constants.SubscriptionFactory.SG),
                    Expression.Parameter(
                        typeof(Func<string, string, string, string, IAsyncReactiveQubscription>),
                        Constants.SubscriptionFactory.SF
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_DefineSubscriptionFactoryAsync_5P()
        {
            Apply(
                ctx =>
                {
                    var factory = ctx.GetSubscriptionFactory<string, string, string, string, string>(new Uri(Constants.SubscriptionFactory.SF));

                    Assert.AreEqual(new Uri(Constants.SubscriptionFactory.SF), ((IKnownResource)factory).Uri);

                    ctx.DefineSubscriptionFactoryAsync<string, string, string, string, string>(new Uri(Constants.SubscriptionFactory.SG), factory, state: null, CancellationToken.None);

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineSubscriptionFactoryAsync<string, string, string, string, string>(
                            uri: null,
                            factory,
                            state: null,
                            CancellationToken.None
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineSubscriptionFactoryAsync<string, string, string, string, string>(
                            new Uri(Constants.SubscriptionFactory.SF),
                            default(IAsyncReactiveQubscriptionFactory<string, string, string, string, string>),
                            state: null,
                            CancellationToken.None
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineSubscriptionFactoryAsync<string, string, string, string, string>(
                            new Uri(Constants.SubscriptionFactory.SF),
                            default(Expression<Func<string, string, string, string, string, IAsyncReactiveQubscription>>),
                            state: null,
                            CancellationToken.None
                        )
                    );
                },
                new DefineSubscriptionFactory(
                    new Uri(Constants.SubscriptionFactory.SG),
                    Expression.Parameter(
                        typeof(Func<string, string, string, string, string, IAsyncReactiveQubscription>),
                        Constants.SubscriptionFactory.SF
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_DefineSubscriptionFactoryAsync_6P()
        {
            Apply(
                ctx =>
                {
                    var factory = ctx.GetSubscriptionFactory<string, string, string, string, string, string>(new Uri(Constants.SubscriptionFactory.SF));

                    Assert.AreEqual(new Uri(Constants.SubscriptionFactory.SF), ((IKnownResource)factory).Uri);

                    ctx.DefineSubscriptionFactoryAsync<string, string, string, string, string, string>(new Uri(Constants.SubscriptionFactory.SG), factory, state: null, CancellationToken.None);

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineSubscriptionFactoryAsync<string, string, string, string, string, string>(
                            uri: null,
                            factory,
                            state: null,
                            CancellationToken.None
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineSubscriptionFactoryAsync<string, string, string, string, string, string>(
                            new Uri(Constants.SubscriptionFactory.SF),
                            default(IAsyncReactiveQubscriptionFactory<string, string, string, string, string, string>),
                            state: null,
                            CancellationToken.None
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineSubscriptionFactoryAsync<string, string, string, string, string, string>(
                            new Uri(Constants.SubscriptionFactory.SF),
                            default(Expression<Func<string, string, string, string, string, string, IAsyncReactiveQubscription>>),
                            state: null,
                            CancellationToken.None
                        )
                    );
                },
                new DefineSubscriptionFactory(
                    new Uri(Constants.SubscriptionFactory.SG),
                    Expression.Parameter(
                        typeof(Func<string, string, string, string, string, string, IAsyncReactiveQubscription>),
                        Constants.SubscriptionFactory.SF
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_DefineSubscriptionFactoryAsync_7P()
        {
            Apply(
                ctx =>
                {
                    var factory = ctx.GetSubscriptionFactory<string, string, string, string, string, string, string>(new Uri(Constants.SubscriptionFactory.SF));

                    Assert.AreEqual(new Uri(Constants.SubscriptionFactory.SF), ((IKnownResource)factory).Uri);

                    ctx.DefineSubscriptionFactoryAsync<string, string, string, string, string, string, string>(new Uri(Constants.SubscriptionFactory.SG), factory, state: null, CancellationToken.None);

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineSubscriptionFactoryAsync<string, string, string, string, string, string, string>(
                            uri: null,
                            factory,
                            state: null,
                            CancellationToken.None
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineSubscriptionFactoryAsync<string, string, string, string, string, string, string>(
                            new Uri(Constants.SubscriptionFactory.SF),
                            default(IAsyncReactiveQubscriptionFactory<string, string, string, string, string, string, string>),
                            state: null,
                            CancellationToken.None
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineSubscriptionFactoryAsync<string, string, string, string, string, string, string>(
                            new Uri(Constants.SubscriptionFactory.SF),
                            default(Expression<Func<string, string, string, string, string, string, string, IAsyncReactiveQubscription>>),
                            state: null,
                            CancellationToken.None
                        )
                    );
                },
                new DefineSubscriptionFactory(
                    new Uri(Constants.SubscriptionFactory.SG),
                    Expression.Parameter(
                        typeof(Func<string, string, string, string, string, string, string, IAsyncReactiveQubscription>),
                        Constants.SubscriptionFactory.SF
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_DefineSubscriptionFactoryAsync_8P()
        {
            Apply(
                ctx =>
                {
                    var factory = ctx.GetSubscriptionFactory<string, string, string, string, string, string, string, string>(new Uri(Constants.SubscriptionFactory.SF));

                    Assert.AreEqual(new Uri(Constants.SubscriptionFactory.SF), ((IKnownResource)factory).Uri);

                    ctx.DefineSubscriptionFactoryAsync<string, string, string, string, string, string, string, string>(new Uri(Constants.SubscriptionFactory.SG), factory, state: null, CancellationToken.None);

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineSubscriptionFactoryAsync<string, string, string, string, string, string, string, string>(
                            uri: null,
                            factory,
                            state: null,
                            CancellationToken.None
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineSubscriptionFactoryAsync<string, string, string, string, string, string, string, string>(
                            new Uri(Constants.SubscriptionFactory.SF),
                            default(IAsyncReactiveQubscriptionFactory<string, string, string, string, string, string, string, string>),
                            state: null,
                            CancellationToken.None
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineSubscriptionFactoryAsync<string, string, string, string, string, string, string, string>(
                            new Uri(Constants.SubscriptionFactory.SF),
                            default(Expression<Func<string, string, string, string, string, string, string, string, IAsyncReactiveQubscription>>),
                            state: null,
                            CancellationToken.None
                        )
                    );
                },
                new DefineSubscriptionFactory(
                    new Uri(Constants.SubscriptionFactory.SG),
                    Expression.Parameter(
                        typeof(Func<string, string, string, string, string, string, string, string, IAsyncReactiveQubscription>),
                        Constants.SubscriptionFactory.SF
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_DefineSubscriptionFactoryAsync_9P()
        {
            Apply(
                ctx =>
                {
                    var factory = ctx.GetSubscriptionFactory<string, string, string, string, string, string, string, string, string>(new Uri(Constants.SubscriptionFactory.SF));

                    Assert.AreEqual(new Uri(Constants.SubscriptionFactory.SF), ((IKnownResource)factory).Uri);

                    ctx.DefineSubscriptionFactoryAsync<string, string, string, string, string, string, string, string, string>(new Uri(Constants.SubscriptionFactory.SG), factory, state: null, CancellationToken.None);

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineSubscriptionFactoryAsync<string, string, string, string, string, string, string, string, string>(
                            uri: null,
                            factory,
                            state: null,
                            CancellationToken.None
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineSubscriptionFactoryAsync<string, string, string, string, string, string, string, string, string>(
                            new Uri(Constants.SubscriptionFactory.SF),
                            default(IAsyncReactiveQubscriptionFactory<string, string, string, string, string, string, string, string, string>),
                            state: null,
                            CancellationToken.None
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineSubscriptionFactoryAsync<string, string, string, string, string, string, string, string, string>(
                            new Uri(Constants.SubscriptionFactory.SF),
                            default(Expression<Func<string, string, string, string, string, string, string, string, string, IAsyncReactiveQubscription>>),
                            state: null,
                            CancellationToken.None
                        )
                    );
                },
                new DefineSubscriptionFactory(
                    new Uri(Constants.SubscriptionFactory.SG),
                    Expression.Parameter(
                        typeof(Func<string, string, string, string, string, string, string, string, string, IAsyncReactiveQubscription>),
                        Constants.SubscriptionFactory.SF
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_DefineSubscriptionFactoryAsync_10P()
        {
            Apply(
                ctx =>
                {
                    var factory = ctx.GetSubscriptionFactory<string, string, string, string, string, string, string, string, string, string>(new Uri(Constants.SubscriptionFactory.SF));

                    Assert.AreEqual(new Uri(Constants.SubscriptionFactory.SF), ((IKnownResource)factory).Uri);

                    ctx.DefineSubscriptionFactoryAsync<string, string, string, string, string, string, string, string, string, string>(new Uri(Constants.SubscriptionFactory.SG), factory, state: null, CancellationToken.None);

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineSubscriptionFactoryAsync<string, string, string, string, string, string, string, string, string, string>(
                            uri: null,
                            factory,
                            state: null,
                            CancellationToken.None
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineSubscriptionFactoryAsync<string, string, string, string, string, string, string, string, string, string>(
                            new Uri(Constants.SubscriptionFactory.SF),
                            default(IAsyncReactiveQubscriptionFactory<string, string, string, string, string, string, string, string, string, string>),
                            state: null,
                            CancellationToken.None
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineSubscriptionFactoryAsync<string, string, string, string, string, string, string, string, string, string>(
                            new Uri(Constants.SubscriptionFactory.SF),
                            default(Expression<Func<string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQubscription>>),
                            state: null,
                            CancellationToken.None
                        )
                    );
                },
                new DefineSubscriptionFactory(
                    new Uri(Constants.SubscriptionFactory.SG),
                    Expression.Parameter(
                        typeof(Func<string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQubscription>),
                        Constants.SubscriptionFactory.SF
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_DefineSubscriptionFactoryAsync_11P()
        {
            Apply(
                ctx =>
                {
                    var factory = ctx.GetSubscriptionFactory<string, string, string, string, string, string, string, string, string, string, string>(new Uri(Constants.SubscriptionFactory.SF));

                    Assert.AreEqual(new Uri(Constants.SubscriptionFactory.SF), ((IKnownResource)factory).Uri);

                    ctx.DefineSubscriptionFactoryAsync<string, string, string, string, string, string, string, string, string, string, string>(new Uri(Constants.SubscriptionFactory.SG), factory, state: null, CancellationToken.None);

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineSubscriptionFactoryAsync<string, string, string, string, string, string, string, string, string, string, string>(
                            uri: null,
                            factory,
                            state: null,
                            CancellationToken.None
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineSubscriptionFactoryAsync<string, string, string, string, string, string, string, string, string, string, string>(
                            new Uri(Constants.SubscriptionFactory.SF),
                            default(IAsyncReactiveQubscriptionFactory<string, string, string, string, string, string, string, string, string, string, string>),
                            state: null,
                            CancellationToken.None
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineSubscriptionFactoryAsync<string, string, string, string, string, string, string, string, string, string, string>(
                            new Uri(Constants.SubscriptionFactory.SF),
                            default(Expression<Func<string, string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQubscription>>),
                            state: null,
                            CancellationToken.None
                        )
                    );
                },
                new DefineSubscriptionFactory(
                    new Uri(Constants.SubscriptionFactory.SG),
                    Expression.Parameter(
                        typeof(Func<string, string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQubscription>),
                        Constants.SubscriptionFactory.SF
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_DefineSubscriptionFactoryAsync_12P()
        {
            Apply(
                ctx =>
                {
                    var factory = ctx.GetSubscriptionFactory<string, string, string, string, string, string, string, string, string, string, string, string>(new Uri(Constants.SubscriptionFactory.SF));

                    Assert.AreEqual(new Uri(Constants.SubscriptionFactory.SF), ((IKnownResource)factory).Uri);

                    ctx.DefineSubscriptionFactoryAsync<string, string, string, string, string, string, string, string, string, string, string, string>(new Uri(Constants.SubscriptionFactory.SG), factory, state: null, CancellationToken.None);

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineSubscriptionFactoryAsync<string, string, string, string, string, string, string, string, string, string, string, string>(
                            uri: null,
                            factory,
                            state: null,
                            CancellationToken.None
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineSubscriptionFactoryAsync<string, string, string, string, string, string, string, string, string, string, string, string>(
                            new Uri(Constants.SubscriptionFactory.SF),
                            default(IAsyncReactiveQubscriptionFactory<string, string, string, string, string, string, string, string, string, string, string, string>),
                            state: null,
                            CancellationToken.None
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineSubscriptionFactoryAsync<string, string, string, string, string, string, string, string, string, string, string, string>(
                            new Uri(Constants.SubscriptionFactory.SF),
                            default(Expression<Func<string, string, string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQubscription>>),
                            state: null,
                            CancellationToken.None
                        )
                    );
                },
                new DefineSubscriptionFactory(
                    new Uri(Constants.SubscriptionFactory.SG),
                    Expression.Parameter(
                        typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQubscription>),
                        Constants.SubscriptionFactory.SF
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_DefineSubscriptionFactoryAsync_13P()
        {
            Apply(
                ctx =>
                {
                    var factory = ctx.GetSubscriptionFactory<string, string, string, string, string, string, string, string, string, string, string, string, string>(new Uri(Constants.SubscriptionFactory.SF));

                    Assert.AreEqual(new Uri(Constants.SubscriptionFactory.SF), ((IKnownResource)factory).Uri);

                    ctx.DefineSubscriptionFactoryAsync<string, string, string, string, string, string, string, string, string, string, string, string, string>(new Uri(Constants.SubscriptionFactory.SG), factory, state: null, CancellationToken.None);

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineSubscriptionFactoryAsync<string, string, string, string, string, string, string, string, string, string, string, string, string>(
                            uri: null,
                            factory,
                            state: null,
                            CancellationToken.None
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineSubscriptionFactoryAsync<string, string, string, string, string, string, string, string, string, string, string, string, string>(
                            new Uri(Constants.SubscriptionFactory.SF),
                            default(IAsyncReactiveQubscriptionFactory<string, string, string, string, string, string, string, string, string, string, string, string, string>),
                            state: null,
                            CancellationToken.None
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineSubscriptionFactoryAsync<string, string, string, string, string, string, string, string, string, string, string, string, string>(
                            new Uri(Constants.SubscriptionFactory.SF),
                            default(Expression<Func<string, string, string, string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQubscription>>),
                            state: null,
                            CancellationToken.None
                        )
                    );
                },
                new DefineSubscriptionFactory(
                    new Uri(Constants.SubscriptionFactory.SG),
                    Expression.Parameter(
                        typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQubscription>),
                        Constants.SubscriptionFactory.SF
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_DefineSubscriptionFactoryAsync_14P()
        {
            Apply(
                ctx =>
                {
                    var factory = ctx.GetSubscriptionFactory<string, string, string, string, string, string, string, string, string, string, string, string, string, string>(new Uri(Constants.SubscriptionFactory.SF));

                    Assert.AreEqual(new Uri(Constants.SubscriptionFactory.SF), ((IKnownResource)factory).Uri);

                    ctx.DefineSubscriptionFactoryAsync<string, string, string, string, string, string, string, string, string, string, string, string, string, string>(new Uri(Constants.SubscriptionFactory.SG), factory, state: null, CancellationToken.None);

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineSubscriptionFactoryAsync<string, string, string, string, string, string, string, string, string, string, string, string, string, string>(
                            uri: null,
                            factory,
                            state: null,
                            CancellationToken.None
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineSubscriptionFactoryAsync<string, string, string, string, string, string, string, string, string, string, string, string, string, string>(
                            new Uri(Constants.SubscriptionFactory.SF),
                            default(IAsyncReactiveQubscriptionFactory<string, string, string, string, string, string, string, string, string, string, string, string, string, string>),
                            state: null,
                            CancellationToken.None
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineSubscriptionFactoryAsync<string, string, string, string, string, string, string, string, string, string, string, string, string, string>(
                            new Uri(Constants.SubscriptionFactory.SF),
                            default(Expression<Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQubscription>>),
                            state: null,
                            CancellationToken.None
                        )
                    );
                },
                new DefineSubscriptionFactory(
                    new Uri(Constants.SubscriptionFactory.SG),
                    Expression.Parameter(
                        typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQubscription>),
                        Constants.SubscriptionFactory.SF
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveClientContext_DefineSubscriptionFactoryAsync_15P()
        {
            Apply(
                ctx =>
                {
                    var factory = ctx.GetSubscriptionFactory<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string>(new Uri(Constants.SubscriptionFactory.SF));

                    Assert.AreEqual(new Uri(Constants.SubscriptionFactory.SF), ((IKnownResource)factory).Uri);

                    ctx.DefineSubscriptionFactoryAsync<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string>(new Uri(Constants.SubscriptionFactory.SG), factory, state: null, CancellationToken.None);

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineSubscriptionFactoryAsync<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string>(
                            uri: null,
                            factory,
                            state: null,
                            CancellationToken.None
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineSubscriptionFactoryAsync<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string>(
                            new Uri(Constants.SubscriptionFactory.SF),
                            default(IAsyncReactiveQubscriptionFactory<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string>),
                            state: null,
                            CancellationToken.None
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineSubscriptionFactoryAsync<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string>(
                            new Uri(Constants.SubscriptionFactory.SF),
                            default(Expression<Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQubscription>>),
                            state: null,
                            CancellationToken.None
                        )
                    );
                },
                new DefineSubscriptionFactory(
                    new Uri(Constants.SubscriptionFactory.SG),
                    Expression.Parameter(
                        typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, IAsyncReactiveQubscription>),
                        Constants.SubscriptionFactory.SF
                    ),
                    state: null
                )
            );
        }

        #endregion

        private sealed class MyParameterizedContext : MyContext
        {
            public MyParameterizedContext(IReactiveServiceProvider provider)
                : base(provider)
            {
            }

            [KnownResource(Constants.Observable.XS + "2")]
            public IAsyncReactiveQbservable<int> Xs2p(string p1, string p2)
            {
                return base.Provider.CreateQbservable<int>(
                    Expression.Call(
                        Expression.Constant(this),
                        (MethodInfo)MethodBase.GetCurrentMethod(),
                        Expression.Constant(p1, typeof(string)),
                        Expression.Constant(p2, typeof(string))
                    )
                );
            }

            [KnownResource(Constants.Observable.XS + "3")]
            public IAsyncReactiveQbservable<int> Xs3p(string p1, string p2, string p3)
            {
                return base.Provider.CreateQbservable<int>(
                    Expression.Call(
                        Expression.Constant(this),
                        (MethodInfo)MethodBase.GetCurrentMethod(),
                        Expression.Constant(p1, typeof(string)),
                        Expression.Constant(p2, typeof(string)),
                        Expression.Constant(p3, typeof(string))
                    )
                );
            }

            [KnownResource(Constants.Observable.XS + "4")]
            public IAsyncReactiveQbservable<int> Xs4p(string p1, string p2, string p3, string p4)
            {
                return base.Provider.CreateQbservable<int>(
                    Expression.Call(
                        Expression.Constant(this),
                        (MethodInfo)MethodBase.GetCurrentMethod(),
                        Expression.Constant(p1, typeof(string)),
                        Expression.Constant(p2, typeof(string)),
                        Expression.Constant(p3, typeof(string)),
                        Expression.Constant(p4, typeof(string))
                    )
                );
            }

            [KnownResource(Constants.Observable.XS + "5")]
            public IAsyncReactiveQbservable<int> Xs5p(string p1, string p2, string p3, string p4, string p5)
            {
                return base.Provider.CreateQbservable<int>(
                    Expression.Call(
                        Expression.Constant(this),
                        (MethodInfo)MethodBase.GetCurrentMethod(),
                        Expression.Constant(p1, typeof(string)),
                        Expression.Constant(p2, typeof(string)),
                        Expression.Constant(p3, typeof(string)),
                        Expression.Constant(p4, typeof(string)),
                        Expression.Constant(p5, typeof(string))
                    )
                );
            }

            [KnownResource(Constants.Observable.XS + "6")]
            public IAsyncReactiveQbservable<int> Xs6p(string p1, string p2, string p3, string p4, string p5, string p6)
            {
                return base.Provider.CreateQbservable<int>(
                    Expression.Call(
                        Expression.Constant(this),
                        (MethodInfo)MethodBase.GetCurrentMethod(),
                        Expression.Constant(p1, typeof(string)),
                        Expression.Constant(p2, typeof(string)),
                        Expression.Constant(p3, typeof(string)),
                        Expression.Constant(p4, typeof(string)),
                        Expression.Constant(p5, typeof(string)),
                        Expression.Constant(p6, typeof(string))
                    )
                );
            }

            [KnownResource(Constants.Observable.XS + "7")]
            public IAsyncReactiveQbservable<int> Xs7p(string p1, string p2, string p3, string p4, string p5, string p6, string p7)
            {
                return base.Provider.CreateQbservable<int>(
                    Expression.Call(
                        Expression.Constant(this),
                        (MethodInfo)MethodBase.GetCurrentMethod(),
                        Expression.Constant(p1, typeof(string)),
                        Expression.Constant(p2, typeof(string)),
                        Expression.Constant(p3, typeof(string)),
                        Expression.Constant(p4, typeof(string)),
                        Expression.Constant(p5, typeof(string)),
                        Expression.Constant(p6, typeof(string)),
                        Expression.Constant(p7, typeof(string))
                    )
                );
            }

            [KnownResource(Constants.Observable.XS + "8")]
            public IAsyncReactiveQbservable<int> Xs8p(string p1, string p2, string p3, string p4, string p5, string p6, string p7, string p8)
            {
                return base.Provider.CreateQbservable<int>(
                    Expression.Call(
                        Expression.Constant(this),
                        (MethodInfo)MethodBase.GetCurrentMethod(),
                        Expression.Constant(p1, typeof(string)),
                        Expression.Constant(p2, typeof(string)),
                        Expression.Constant(p3, typeof(string)),
                        Expression.Constant(p4, typeof(string)),
                        Expression.Constant(p5, typeof(string)),
                        Expression.Constant(p6, typeof(string)),
                        Expression.Constant(p7, typeof(string)),
                        Expression.Constant(p8, typeof(string))
                    )
                );
            }

            [KnownResource(Constants.Observable.XS + "9")]
            public IAsyncReactiveQbservable<int> Xs9p(string p1, string p2, string p3, string p4, string p5, string p6, string p7, string p8, string p9)
            {
                return base.Provider.CreateQbservable<int>(
                    Expression.Call(
                        Expression.Constant(this),
                        (MethodInfo)MethodBase.GetCurrentMethod(),
                        Expression.Constant(p1, typeof(string)),
                        Expression.Constant(p2, typeof(string)),
                        Expression.Constant(p3, typeof(string)),
                        Expression.Constant(p4, typeof(string)),
                        Expression.Constant(p5, typeof(string)),
                        Expression.Constant(p6, typeof(string)),
                        Expression.Constant(p7, typeof(string)),
                        Expression.Constant(p8, typeof(string)),
                        Expression.Constant(p9, typeof(string))
                    )
                );
            }

            [KnownResource(Constants.Observable.XS + "10")]
            public IAsyncReactiveQbservable<int> Xs10p(string p1, string p2, string p3, string p4, string p5, string p6, string p7, string p8, string p9, string p10)
            {
                return base.Provider.CreateQbservable<int>(
                    Expression.Call(
                        Expression.Constant(this),
                        (MethodInfo)MethodBase.GetCurrentMethod(),
                        Expression.Constant(p1, typeof(string)),
                        Expression.Constant(p2, typeof(string)),
                        Expression.Constant(p3, typeof(string)),
                        Expression.Constant(p4, typeof(string)),
                        Expression.Constant(p5, typeof(string)),
                        Expression.Constant(p6, typeof(string)),
                        Expression.Constant(p7, typeof(string)),
                        Expression.Constant(p8, typeof(string)),
                        Expression.Constant(p9, typeof(string)),
                        Expression.Constant(p10, typeof(string))
                    )
                );
            }

            [KnownResource(Constants.Observable.XS + "11")]
            public IAsyncReactiveQbservable<int> Xs11p(string p1, string p2, string p3, string p4, string p5, string p6, string p7, string p8, string p9, string p10, string p11)
            {
                return base.Provider.CreateQbservable<int>(
                    Expression.Call(
                        Expression.Constant(this),
                        (MethodInfo)MethodBase.GetCurrentMethod(),
                        Expression.Constant(p1, typeof(string)),
                        Expression.Constant(p2, typeof(string)),
                        Expression.Constant(p3, typeof(string)),
                        Expression.Constant(p4, typeof(string)),
                        Expression.Constant(p5, typeof(string)),
                        Expression.Constant(p6, typeof(string)),
                        Expression.Constant(p7, typeof(string)),
                        Expression.Constant(p8, typeof(string)),
                        Expression.Constant(p9, typeof(string)),
                        Expression.Constant(p10, typeof(string)),
                        Expression.Constant(p11, typeof(string))
                    )
                );
            }

            [KnownResource(Constants.Observable.XS + "12")]
            public IAsyncReactiveQbservable<int> Xs12p(string p1, string p2, string p3, string p4, string p5, string p6, string p7, string p8, string p9, string p10, string p11, string p12)
            {
                return base.Provider.CreateQbservable<int>(
                    Expression.Call(
                        Expression.Constant(this),
                        (MethodInfo)MethodBase.GetCurrentMethod(),
                        Expression.Constant(p1, typeof(string)),
                        Expression.Constant(p2, typeof(string)),
                        Expression.Constant(p3, typeof(string)),
                        Expression.Constant(p4, typeof(string)),
                        Expression.Constant(p5, typeof(string)),
                        Expression.Constant(p6, typeof(string)),
                        Expression.Constant(p7, typeof(string)),
                        Expression.Constant(p8, typeof(string)),
                        Expression.Constant(p9, typeof(string)),
                        Expression.Constant(p10, typeof(string)),
                        Expression.Constant(p11, typeof(string)),
                        Expression.Constant(p12, typeof(string))
                    )
                );
            }

            [KnownResource(Constants.Observable.XS + "13")]
            public IAsyncReactiveQbservable<int> Xs13p(string p1, string p2, string p3, string p4, string p5, string p6, string p7, string p8, string p9, string p10, string p11, string p12, string p13)
            {
                return base.Provider.CreateQbservable<int>(
                    Expression.Call(
                        Expression.Constant(this),
                        (MethodInfo)MethodBase.GetCurrentMethod(),
                        Expression.Constant(p1, typeof(string)),
                        Expression.Constant(p2, typeof(string)),
                        Expression.Constant(p3, typeof(string)),
                        Expression.Constant(p4, typeof(string)),
                        Expression.Constant(p5, typeof(string)),
                        Expression.Constant(p6, typeof(string)),
                        Expression.Constant(p7, typeof(string)),
                        Expression.Constant(p8, typeof(string)),
                        Expression.Constant(p9, typeof(string)),
                        Expression.Constant(p10, typeof(string)),
                        Expression.Constant(p11, typeof(string)),
                        Expression.Constant(p12, typeof(string)),
                        Expression.Constant(p13, typeof(string))
                    )
                );
            }

            [KnownResource(Constants.Observable.XS + "14")]
            public IAsyncReactiveQbservable<int> Xs14p(string p1, string p2, string p3, string p4, string p5, string p6, string p7, string p8, string p9, string p10, string p11, string p12, string p13, string p14)
            {
                return base.Provider.CreateQbservable<int>(
                    Expression.Call(
                        Expression.Constant(this),
                        (MethodInfo)MethodBase.GetCurrentMethod(),
                        Expression.Constant(p1, typeof(string)),
                        Expression.Constant(p2, typeof(string)),
                        Expression.Constant(p3, typeof(string)),
                        Expression.Constant(p4, typeof(string)),
                        Expression.Constant(p5, typeof(string)),
                        Expression.Constant(p6, typeof(string)),
                        Expression.Constant(p7, typeof(string)),
                        Expression.Constant(p8, typeof(string)),
                        Expression.Constant(p9, typeof(string)),
                        Expression.Constant(p10, typeof(string)),
                        Expression.Constant(p11, typeof(string)),
                        Expression.Constant(p12, typeof(string)),
                        Expression.Constant(p13, typeof(string)),
                        Expression.Constant(p14, typeof(string))
                    )
                );
            }

            [KnownResource(Constants.Observable.XS + "15")]
            public IAsyncReactiveQbservable<int> Xs15p(string p1, string p2, string p3, string p4, string p5, string p6, string p7, string p8, string p9, string p10, string p11, string p12, string p13, string p14, string p15)
            {
                return base.Provider.CreateQbservable<int>(
                    Expression.Call(
                        Expression.Constant(this),
                        (MethodInfo)MethodBase.GetCurrentMethod(),
                        Expression.Constant(p1, typeof(string)),
                        Expression.Constant(p2, typeof(string)),
                        Expression.Constant(p3, typeof(string)),
                        Expression.Constant(p4, typeof(string)),
                        Expression.Constant(p5, typeof(string)),
                        Expression.Constant(p6, typeof(string)),
                        Expression.Constant(p7, typeof(string)),
                        Expression.Constant(p8, typeof(string)),
                        Expression.Constant(p9, typeof(string)),
                        Expression.Constant(p10, typeof(string)),
                        Expression.Constant(p11, typeof(string)),
                        Expression.Constant(p12, typeof(string)),
                        Expression.Constant(p13, typeof(string)),
                        Expression.Constant(p14, typeof(string)),
                        Expression.Constant(p15, typeof(string))
                    )
                );
            }

        }
    }
}