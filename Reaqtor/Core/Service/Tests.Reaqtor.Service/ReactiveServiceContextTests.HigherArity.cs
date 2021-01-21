// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// RB, ER - October 2013 - Created this file.
// Auto-generated file, changes to source may be lost
//

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using Reaqtor;
using Reaqtor.TestingFramework;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Reaqtor.Service
{
    public partial class ReactiveServiceContextTests
    {
        #region "ReactiveServiceContext_ReactiveQueryProvider_ArgumentChecking_HigherArity"

        [TestMethod]
        public void ReactiveServiceContext_ReactiveQueryProvider_ArgumentChecking_HigherArity_CreateQbservable()
        {
            Apply(
                ctx =>
                {
                    var provider = (ReactiveQueryProvider)ctx.Provider;
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
        public void ReactiveServiceContext_ReactiveQueryProvider_ArgumentChecking_HigherArity_CreateQbserver()
        {
            Apply(
                ctx =>
                {
                    var provider = (ReactiveQueryProvider)ctx.Provider;
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
        public void ReactiveServiceContext_ReactiveQueryProvider_ArgumentChecking_HigherArity_CreateQubjectFactory()
        {
            Apply(
                ctx =>
                {
                    var provider = (ReactiveQueryProvider)ctx.Provider;
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
        public void ReactiveServiceContext_ReactiveQueryProvider_CreateQubjectFactory_HigherArity_UnknownArtifact()
        {
            Apply(
                ctx =>
                {
                    var provider = (ReactiveQueryProvider)ctx.Provider;
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
        public void ReactiveServiceContext_ReactiveQueryProvider_ArgumentChecking_HigherArity_CreateQubscriptionFactory()
        {
            Apply(
                ctx =>
                {
                    var provider = (ReactiveQueryProvider)ctx.Provider;
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
        public void ReactiveServiceContext_ReactiveQueryProvider_CreateQubscriptionFactory_HigherArity_UnknownArtifact()
        {
            Apply(
                ctx =>
                {
                    var provider = (ReactiveQueryProvider)ctx.Provider;
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

        #region "ReactiveServiceContext_ReactiveClient_ArgumentChecking_HigherArity"

        [TestMethod]
        public void ReactiveServiceContext_ReactiveClient_ArgumentChecking_HigherArity_GetObservable()
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
        public void ReactiveServiceContext_ReactiveClient_ArgumentChecking_HigherArity_GetObserver()
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
        public void ReactiveServiceContext_ReactiveClient_ArgumentChecking_HigherArity_GetStreamFactory()
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
        public void ReactiveServiceContext_ReactiveClient_ArgumentChecking_HigherArity_GetSubscriptionFactory()
        {
            Apply(
                ctx =>
                {
                    Assert.ThrowsException<ArgumentNullException>(() => ctx.GetSubscriptionFactory<int>(null));
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

        #region "ReactiveServiceContext_Subscribe_Parameterized_Simple1"

        [TestMethod]
        public void ReactiveServiceContext_Subscribe_Parameterized_Simple1_2P()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<string, string, int>(new Uri(Constants.Observable.XS));
                    var ob = ctx.GetObserver<string, string, int>(new Uri(Constants.Observer.OB));
                    xs("observable_parameter_1", "observable_parameter_2").Subscribe(ob("observer_parameter_1", "observer_parameter_2"), new Uri(Constants.Subscription.SUB), null);
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<IReactiveQbservable<int>, IReactiveQbserver<int>, IReactiveQubscription>), Constants.SubscribeUri),
                        Expression.Invoke(
                            Expression.Parameter(typeof(Func<string, string, IReactiveQbservable<int>>), Constants.Observable.XS),
                            Expression.Constant("observable_parameter_1"),
                            Expression.Constant("observable_parameter_2")
                        ),
                        Expression.Invoke(
                            Expression.Parameter(typeof(Func<string, string, IReactiveQbserver<int>>), Constants.Observer.OB),
                            Expression.Constant("observer_parameter_1"),
                            Expression.Constant("observer_parameter_2")
                        )
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_Subscribe_Parameterized_Simple1_3P()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<string, string, string, int>(new Uri(Constants.Observable.XS));
                    var ob = ctx.GetObserver<string, string, string, int>(new Uri(Constants.Observer.OB));
                    xs("observable_parameter_1", "observable_parameter_2", "observable_parameter_3").Subscribe(ob("observer_parameter_1", "observer_parameter_2", "observer_parameter_3"), new Uri(Constants.Subscription.SUB), null);
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<IReactiveQbservable<int>, IReactiveQbserver<int>, IReactiveQubscription>), Constants.SubscribeUri),
                        Expression.Invoke(
                            Expression.Parameter(typeof(Func<string, string, string, IReactiveQbservable<int>>), Constants.Observable.XS),
                            Expression.Constant("observable_parameter_1"),
                            Expression.Constant("observable_parameter_2"),
                            Expression.Constant("observable_parameter_3")
                        ),
                        Expression.Invoke(
                            Expression.Parameter(typeof(Func<string, string, string, IReactiveQbserver<int>>), Constants.Observer.OB),
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
        public void ReactiveServiceContext_Subscribe_Parameterized_Simple1_4P()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<string, string, string, string, int>(new Uri(Constants.Observable.XS));
                    var ob = ctx.GetObserver<string, string, string, string, int>(new Uri(Constants.Observer.OB));
                    xs("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4").Subscribe(ob("observer_parameter_1", "observer_parameter_2", "observer_parameter_3", "observer_parameter_4"), new Uri(Constants.Subscription.SUB), null);
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<IReactiveQbservable<int>, IReactiveQbserver<int>, IReactiveQubscription>), Constants.SubscribeUri),
                        Expression.Invoke(
                            Expression.Parameter(typeof(Func<string, string, string, string, IReactiveQbservable<int>>), Constants.Observable.XS),
                            Expression.Constant("observable_parameter_1"),
                            Expression.Constant("observable_parameter_2"),
                            Expression.Constant("observable_parameter_3"),
                            Expression.Constant("observable_parameter_4")
                        ),
                        Expression.Invoke(
                            Expression.Parameter(typeof(Func<string, string, string, string, IReactiveQbserver<int>>), Constants.Observer.OB),
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
        public void ReactiveServiceContext_Subscribe_Parameterized_Simple1_5P()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<string, string, string, string, string, int>(new Uri(Constants.Observable.XS));
                    var ob = ctx.GetObserver<string, string, string, string, string, int>(new Uri(Constants.Observer.OB));
                    xs("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5").Subscribe(ob("observer_parameter_1", "observer_parameter_2", "observer_parameter_3", "observer_parameter_4", "observer_parameter_5"), new Uri(Constants.Subscription.SUB), null);
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<IReactiveQbservable<int>, IReactiveQbserver<int>, IReactiveQubscription>), Constants.SubscribeUri),
                        Expression.Invoke(
                            Expression.Parameter(typeof(Func<string, string, string, string, string, IReactiveQbservable<int>>), Constants.Observable.XS),
                            Expression.Constant("observable_parameter_1"),
                            Expression.Constant("observable_parameter_2"),
                            Expression.Constant("observable_parameter_3"),
                            Expression.Constant("observable_parameter_4"),
                            Expression.Constant("observable_parameter_5")
                        ),
                        Expression.Invoke(
                            Expression.Parameter(typeof(Func<string, string, string, string, string, IReactiveQbserver<int>>), Constants.Observer.OB),
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
        public void ReactiveServiceContext_Subscribe_Parameterized_Simple1_6P()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<string, string, string, string, string, string, int>(new Uri(Constants.Observable.XS));
                    var ob = ctx.GetObserver<string, string, string, string, string, string, int>(new Uri(Constants.Observer.OB));
                    xs("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6").Subscribe(ob("observer_parameter_1", "observer_parameter_2", "observer_parameter_3", "observer_parameter_4", "observer_parameter_5", "observer_parameter_6"), new Uri(Constants.Subscription.SUB), null);
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<IReactiveQbservable<int>, IReactiveQbserver<int>, IReactiveQubscription>), Constants.SubscribeUri),
                        Expression.Invoke(
                            Expression.Parameter(typeof(Func<string, string, string, string, string, string, IReactiveQbservable<int>>), Constants.Observable.XS),
                            Expression.Constant("observable_parameter_1"),
                            Expression.Constant("observable_parameter_2"),
                            Expression.Constant("observable_parameter_3"),
                            Expression.Constant("observable_parameter_4"),
                            Expression.Constant("observable_parameter_5"),
                            Expression.Constant("observable_parameter_6")
                        ),
                        Expression.Invoke(
                            Expression.Parameter(typeof(Func<string, string, string, string, string, string, IReactiveQbserver<int>>), Constants.Observer.OB),
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
        public void ReactiveServiceContext_Subscribe_Parameterized_Simple1_7P()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<string, string, string, string, string, string, string, int>(new Uri(Constants.Observable.XS));
                    var ob = ctx.GetObserver<string, string, string, string, string, string, string, int>(new Uri(Constants.Observer.OB));
                    xs("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7").Subscribe(ob("observer_parameter_1", "observer_parameter_2", "observer_parameter_3", "observer_parameter_4", "observer_parameter_5", "observer_parameter_6", "observer_parameter_7"), new Uri(Constants.Subscription.SUB), null);
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<IReactiveQbservable<int>, IReactiveQbserver<int>, IReactiveQubscription>), Constants.SubscribeUri),
                        Expression.Invoke(
                            Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, IReactiveQbservable<int>>), Constants.Observable.XS),
                            Expression.Constant("observable_parameter_1"),
                            Expression.Constant("observable_parameter_2"),
                            Expression.Constant("observable_parameter_3"),
                            Expression.Constant("observable_parameter_4"),
                            Expression.Constant("observable_parameter_5"),
                            Expression.Constant("observable_parameter_6"),
                            Expression.Constant("observable_parameter_7")
                        ),
                        Expression.Invoke(
                            Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, IReactiveQbserver<int>>), Constants.Observer.OB),
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
        public void ReactiveServiceContext_Subscribe_Parameterized_Simple1_8P()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observable.XS));
                    var ob = ctx.GetObserver<string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observer.OB));
                    xs("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7", "observable_parameter_8").Subscribe(ob("observer_parameter_1", "observer_parameter_2", "observer_parameter_3", "observer_parameter_4", "observer_parameter_5", "observer_parameter_6", "observer_parameter_7", "observer_parameter_8"), new Uri(Constants.Subscription.SUB), null);
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<IReactiveQbservable<int>, IReactiveQbserver<int>, IReactiveQubscription>), Constants.SubscribeUri),
                        Expression.Invoke(
                            Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, IReactiveQbservable<int>>), Constants.Observable.XS),
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
                            Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, IReactiveQbserver<int>>), Constants.Observer.OB),
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
        public void ReactiveServiceContext_Subscribe_Parameterized_Simple1_9P()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observable.XS));
                    var ob = ctx.GetObserver<string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observer.OB));
                    xs("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7", "observable_parameter_8", "observable_parameter_9").Subscribe(ob("observer_parameter_1", "observer_parameter_2", "observer_parameter_3", "observer_parameter_4", "observer_parameter_5", "observer_parameter_6", "observer_parameter_7", "observer_parameter_8", "observer_parameter_9"), new Uri(Constants.Subscription.SUB), null);
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<IReactiveQbservable<int>, IReactiveQbserver<int>, IReactiveQubscription>), Constants.SubscribeUri),
                        Expression.Invoke(
                            Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, IReactiveQbservable<int>>), Constants.Observable.XS),
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
                            Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, IReactiveQbserver<int>>), Constants.Observer.OB),
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
        public void ReactiveServiceContext_Subscribe_Parameterized_Simple1_10P()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observable.XS));
                    var ob = ctx.GetObserver<string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observer.OB));
                    xs("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7", "observable_parameter_8", "observable_parameter_9", "observable_parameter_10").Subscribe(ob("observer_parameter_1", "observer_parameter_2", "observer_parameter_3", "observer_parameter_4", "observer_parameter_5", "observer_parameter_6", "observer_parameter_7", "observer_parameter_8", "observer_parameter_9", "observer_parameter_10"), new Uri(Constants.Subscription.SUB), null);
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<IReactiveQbservable<int>, IReactiveQbserver<int>, IReactiveQubscription>), Constants.SubscribeUri),
                        Expression.Invoke(
                            Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, IReactiveQbservable<int>>), Constants.Observable.XS),
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
                            Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, IReactiveQbserver<int>>), Constants.Observer.OB),
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
        public void ReactiveServiceContext_Subscribe_Parameterized_Simple1_11P()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<string, string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observable.XS));
                    var ob = ctx.GetObserver<string, string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observer.OB));
                    xs("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7", "observable_parameter_8", "observable_parameter_9", "observable_parameter_10", "observable_parameter_11").Subscribe(ob("observer_parameter_1", "observer_parameter_2", "observer_parameter_3", "observer_parameter_4", "observer_parameter_5", "observer_parameter_6", "observer_parameter_7", "observer_parameter_8", "observer_parameter_9", "observer_parameter_10", "observer_parameter_11"), new Uri(Constants.Subscription.SUB), null);
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<IReactiveQbservable<int>, IReactiveQbserver<int>, IReactiveQubscription>), Constants.SubscribeUri),
                        Expression.Invoke(
                            Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, IReactiveQbservable<int>>), Constants.Observable.XS),
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
                            Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, IReactiveQbserver<int>>), Constants.Observer.OB),
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
        public void ReactiveServiceContext_Subscribe_Parameterized_Simple1_12P()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<string, string, string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observable.XS));
                    var ob = ctx.GetObserver<string, string, string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observer.OB));
                    xs("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7", "observable_parameter_8", "observable_parameter_9", "observable_parameter_10", "observable_parameter_11", "observable_parameter_12").Subscribe(ob("observer_parameter_1", "observer_parameter_2", "observer_parameter_3", "observer_parameter_4", "observer_parameter_5", "observer_parameter_6", "observer_parameter_7", "observer_parameter_8", "observer_parameter_9", "observer_parameter_10", "observer_parameter_11", "observer_parameter_12"), new Uri(Constants.Subscription.SUB), null);
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<IReactiveQbservable<int>, IReactiveQbserver<int>, IReactiveQubscription>), Constants.SubscribeUri),
                        Expression.Invoke(
                            Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, IReactiveQbservable<int>>), Constants.Observable.XS),
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
                            Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, IReactiveQbserver<int>>), Constants.Observer.OB),
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
        public void ReactiveServiceContext_Subscribe_Parameterized_Simple1_13P()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<string, string, string, string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observable.XS));
                    var ob = ctx.GetObserver<string, string, string, string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observer.OB));
                    xs("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7", "observable_parameter_8", "observable_parameter_9", "observable_parameter_10", "observable_parameter_11", "observable_parameter_12", "observable_parameter_13").Subscribe(ob("observer_parameter_1", "observer_parameter_2", "observer_parameter_3", "observer_parameter_4", "observer_parameter_5", "observer_parameter_6", "observer_parameter_7", "observer_parameter_8", "observer_parameter_9", "observer_parameter_10", "observer_parameter_11", "observer_parameter_12", "observer_parameter_13"), new Uri(Constants.Subscription.SUB), null);
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<IReactiveQbservable<int>, IReactiveQbserver<int>, IReactiveQubscription>), Constants.SubscribeUri),
                        Expression.Invoke(
                            Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, IReactiveQbservable<int>>), Constants.Observable.XS),
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
                            Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, IReactiveQbserver<int>>), Constants.Observer.OB),
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
        public void ReactiveServiceContext_Subscribe_Parameterized_Simple1_14P()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<string, string, string, string, string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observable.XS));
                    var ob = ctx.GetObserver<string, string, string, string, string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observer.OB));
                    xs("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7", "observable_parameter_8", "observable_parameter_9", "observable_parameter_10", "observable_parameter_11", "observable_parameter_12", "observable_parameter_13", "observable_parameter_14").Subscribe(ob("observer_parameter_1", "observer_parameter_2", "observer_parameter_3", "observer_parameter_4", "observer_parameter_5", "observer_parameter_6", "observer_parameter_7", "observer_parameter_8", "observer_parameter_9", "observer_parameter_10", "observer_parameter_11", "observer_parameter_12", "observer_parameter_13", "observer_parameter_14"), new Uri(Constants.Subscription.SUB), null);
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<IReactiveQbservable<int>, IReactiveQbserver<int>, IReactiveQubscription>), Constants.SubscribeUri),
                        Expression.Invoke(
                            Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, IReactiveQbservable<int>>), Constants.Observable.XS),
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
                            Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, IReactiveQbserver<int>>), Constants.Observer.OB),
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
        public void ReactiveServiceContext_Subscribe_Parameterized_Simple1_15P()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observable.XS));
                    var ob = ctx.GetObserver<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observer.OB));
                    xs("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7", "observable_parameter_8", "observable_parameter_9", "observable_parameter_10", "observable_parameter_11", "observable_parameter_12", "observable_parameter_13", "observable_parameter_14", "observable_parameter_15").Subscribe(ob("observer_parameter_1", "observer_parameter_2", "observer_parameter_3", "observer_parameter_4", "observer_parameter_5", "observer_parameter_6", "observer_parameter_7", "observer_parameter_8", "observer_parameter_9", "observer_parameter_10", "observer_parameter_11", "observer_parameter_12", "observer_parameter_13", "observer_parameter_14", "observer_parameter_15"), new Uri(Constants.Subscription.SUB), null);
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<IReactiveQbservable<int>, IReactiveQbserver<int>, IReactiveQubscription>), Constants.SubscribeUri),
                        Expression.Invoke(
                            Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, IReactiveQbservable<int>>), Constants.Observable.XS),
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
                            Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, IReactiveQbserver<int>>), Constants.Observer.OB),
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

        #region "ReactiveServiceContext_Subscribe_Parameterized_Simple2"

        [TestMethod]
        public void ReactiveServiceContext_Subscribe_Parameterized_Simple2_2P()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<string, string, int>(new Uri(Constants.Observable.XS));

                    var ys = from x in xs("observable_parameter_1", "observable_parameter_2")
                             where x > 0
                             select x * x;

                    var ob = ctx.GetObserver<string, string, int>(new Uri(Constants.Observer.OB));
                    ys.Subscribe(ob("observer_parameter_1", "observer_parameter_2"), new Uri(Constants.Subscription.SUB), null);
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
                                        Expression.Parameter(typeof(Func<string, string, IReactiveQbservable<int>>), Constants.Observable.XS),
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
                                Expression.Parameter(typeof(Func<string, string, IReactiveQbserver<int>>), Constants.Observer.OB),
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
        public void ReactiveServiceContext_Subscribe_Parameterized_Simple2_3P()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<string, string, string, int>(new Uri(Constants.Observable.XS));

                    var ys = from x in xs("observable_parameter_1", "observable_parameter_2", "observable_parameter_3")
                             where x > 0
                             select x * x;

                    var ob = ctx.GetObserver<string, string, string, int>(new Uri(Constants.Observer.OB));
                    ys.Subscribe(ob("observer_parameter_1", "observer_parameter_2", "observer_parameter_3"), new Uri(Constants.Subscription.SUB), null);
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
                                        Expression.Parameter(typeof(Func<string, string, string, IReactiveQbservable<int>>), Constants.Observable.XS),
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
                                Expression.Parameter(typeof(Func<string, string, string, IReactiveQbserver<int>>), Constants.Observer.OB),
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
        public void ReactiveServiceContext_Subscribe_Parameterized_Simple2_4P()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<string, string, string, string, int>(new Uri(Constants.Observable.XS));

                    var ys = from x in xs("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4")
                             where x > 0
                             select x * x;

                    var ob = ctx.GetObserver<string, string, string, string, int>(new Uri(Constants.Observer.OB));
                    ys.Subscribe(ob("observer_parameter_1", "observer_parameter_2", "observer_parameter_3", "observer_parameter_4"), new Uri(Constants.Subscription.SUB), null);
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
                                        Expression.Parameter(typeof(Func<string, string, string, string, IReactiveQbservable<int>>), Constants.Observable.XS),
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
                                Expression.Parameter(typeof(Func<string, string, string, string, IReactiveQbserver<int>>), Constants.Observer.OB),
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
        public void ReactiveServiceContext_Subscribe_Parameterized_Simple2_5P()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<string, string, string, string, string, int>(new Uri(Constants.Observable.XS));

                    var ys = from x in xs("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5")
                             where x > 0
                             select x * x;

                    var ob = ctx.GetObserver<string, string, string, string, string, int>(new Uri(Constants.Observer.OB));
                    ys.Subscribe(ob("observer_parameter_1", "observer_parameter_2", "observer_parameter_3", "observer_parameter_4", "observer_parameter_5"), new Uri(Constants.Subscription.SUB), null);
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
                                        Expression.Parameter(typeof(Func<string, string, string, string, string, IReactiveQbservable<int>>), Constants.Observable.XS),
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
                                Expression.Parameter(typeof(Func<string, string, string, string, string, IReactiveQbserver<int>>), Constants.Observer.OB),
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
        public void ReactiveServiceContext_Subscribe_Parameterized_Simple2_6P()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<string, string, string, string, string, string, int>(new Uri(Constants.Observable.XS));

                    var ys = from x in xs("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6")
                             where x > 0
                             select x * x;

                    var ob = ctx.GetObserver<string, string, string, string, string, string, int>(new Uri(Constants.Observer.OB));
                    ys.Subscribe(ob("observer_parameter_1", "observer_parameter_2", "observer_parameter_3", "observer_parameter_4", "observer_parameter_5", "observer_parameter_6"), new Uri(Constants.Subscription.SUB), null);
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
                                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, IReactiveQbservable<int>>), Constants.Observable.XS),
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
                                Expression.Parameter(typeof(Func<string, string, string, string, string, string, IReactiveQbserver<int>>), Constants.Observer.OB),
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
        public void ReactiveServiceContext_Subscribe_Parameterized_Simple2_7P()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<string, string, string, string, string, string, string, int>(new Uri(Constants.Observable.XS));

                    var ys = from x in xs("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7")
                             where x > 0
                             select x * x;

                    var ob = ctx.GetObserver<string, string, string, string, string, string, string, int>(new Uri(Constants.Observer.OB));
                    ys.Subscribe(ob("observer_parameter_1", "observer_parameter_2", "observer_parameter_3", "observer_parameter_4", "observer_parameter_5", "observer_parameter_6", "observer_parameter_7"), new Uri(Constants.Subscription.SUB), null);
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
                                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, IReactiveQbservable<int>>), Constants.Observable.XS),
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
                                Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, IReactiveQbserver<int>>), Constants.Observer.OB),
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
        public void ReactiveServiceContext_Subscribe_Parameterized_Simple2_8P()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observable.XS));

                    var ys = from x in xs("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7", "observable_parameter_8")
                             where x > 0
                             select x * x;

                    var ob = ctx.GetObserver<string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observer.OB));
                    ys.Subscribe(ob("observer_parameter_1", "observer_parameter_2", "observer_parameter_3", "observer_parameter_4", "observer_parameter_5", "observer_parameter_6", "observer_parameter_7", "observer_parameter_8"), new Uri(Constants.Subscription.SUB), null);
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
                                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, IReactiveQbservable<int>>), Constants.Observable.XS),
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
                                Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, IReactiveQbserver<int>>), Constants.Observer.OB),
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
        public void ReactiveServiceContext_Subscribe_Parameterized_Simple2_9P()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observable.XS));

                    var ys = from x in xs("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7", "observable_parameter_8", "observable_parameter_9")
                             where x > 0
                             select x * x;

                    var ob = ctx.GetObserver<string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observer.OB));
                    ys.Subscribe(ob("observer_parameter_1", "observer_parameter_2", "observer_parameter_3", "observer_parameter_4", "observer_parameter_5", "observer_parameter_6", "observer_parameter_7", "observer_parameter_8", "observer_parameter_9"), new Uri(Constants.Subscription.SUB), null);
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
                                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, IReactiveQbservable<int>>), Constants.Observable.XS),
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
                                Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, IReactiveQbserver<int>>), Constants.Observer.OB),
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
        public void ReactiveServiceContext_Subscribe_Parameterized_Simple2_10P()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observable.XS));

                    var ys = from x in xs("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7", "observable_parameter_8", "observable_parameter_9", "observable_parameter_10")
                             where x > 0
                             select x * x;

                    var ob = ctx.GetObserver<string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observer.OB));
                    ys.Subscribe(ob("observer_parameter_1", "observer_parameter_2", "observer_parameter_3", "observer_parameter_4", "observer_parameter_5", "observer_parameter_6", "observer_parameter_7", "observer_parameter_8", "observer_parameter_9", "observer_parameter_10"), new Uri(Constants.Subscription.SUB), null);
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
                                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, IReactiveQbservable<int>>), Constants.Observable.XS),
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
                                Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, IReactiveQbserver<int>>), Constants.Observer.OB),
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
        public void ReactiveServiceContext_Subscribe_Parameterized_Simple2_11P()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<string, string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observable.XS));

                    var ys = from x in xs("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7", "observable_parameter_8", "observable_parameter_9", "observable_parameter_10", "observable_parameter_11")
                             where x > 0
                             select x * x;

                    var ob = ctx.GetObserver<string, string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observer.OB));
                    ys.Subscribe(ob("observer_parameter_1", "observer_parameter_2", "observer_parameter_3", "observer_parameter_4", "observer_parameter_5", "observer_parameter_6", "observer_parameter_7", "observer_parameter_8", "observer_parameter_9", "observer_parameter_10", "observer_parameter_11"), new Uri(Constants.Subscription.SUB), null);
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
                                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, IReactiveQbservable<int>>), Constants.Observable.XS),
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
                                Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, IReactiveQbserver<int>>), Constants.Observer.OB),
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
        public void ReactiveServiceContext_Subscribe_Parameterized_Simple2_12P()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<string, string, string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observable.XS));

                    var ys = from x in xs("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7", "observable_parameter_8", "observable_parameter_9", "observable_parameter_10", "observable_parameter_11", "observable_parameter_12")
                             where x > 0
                             select x * x;

                    var ob = ctx.GetObserver<string, string, string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observer.OB));
                    ys.Subscribe(ob("observer_parameter_1", "observer_parameter_2", "observer_parameter_3", "observer_parameter_4", "observer_parameter_5", "observer_parameter_6", "observer_parameter_7", "observer_parameter_8", "observer_parameter_9", "observer_parameter_10", "observer_parameter_11", "observer_parameter_12"), new Uri(Constants.Subscription.SUB), null);
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
                                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, IReactiveQbservable<int>>), Constants.Observable.XS),
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
                                Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, IReactiveQbserver<int>>), Constants.Observer.OB),
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
        public void ReactiveServiceContext_Subscribe_Parameterized_Simple2_13P()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<string, string, string, string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observable.XS));

                    var ys = from x in xs("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7", "observable_parameter_8", "observable_parameter_9", "observable_parameter_10", "observable_parameter_11", "observable_parameter_12", "observable_parameter_13")
                             where x > 0
                             select x * x;

                    var ob = ctx.GetObserver<string, string, string, string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observer.OB));
                    ys.Subscribe(ob("observer_parameter_1", "observer_parameter_2", "observer_parameter_3", "observer_parameter_4", "observer_parameter_5", "observer_parameter_6", "observer_parameter_7", "observer_parameter_8", "observer_parameter_9", "observer_parameter_10", "observer_parameter_11", "observer_parameter_12", "observer_parameter_13"), new Uri(Constants.Subscription.SUB), null);
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
                                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, IReactiveQbservable<int>>), Constants.Observable.XS),
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
                                Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, IReactiveQbserver<int>>), Constants.Observer.OB),
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
        public void ReactiveServiceContext_Subscribe_Parameterized_Simple2_14P()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<string, string, string, string, string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observable.XS));

                    var ys = from x in xs("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7", "observable_parameter_8", "observable_parameter_9", "observable_parameter_10", "observable_parameter_11", "observable_parameter_12", "observable_parameter_13", "observable_parameter_14")
                             where x > 0
                             select x * x;

                    var ob = ctx.GetObserver<string, string, string, string, string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observer.OB));
                    ys.Subscribe(ob("observer_parameter_1", "observer_parameter_2", "observer_parameter_3", "observer_parameter_4", "observer_parameter_5", "observer_parameter_6", "observer_parameter_7", "observer_parameter_8", "observer_parameter_9", "observer_parameter_10", "observer_parameter_11", "observer_parameter_12", "observer_parameter_13", "observer_parameter_14"), new Uri(Constants.Subscription.SUB), null);
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
                                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, IReactiveQbservable<int>>), Constants.Observable.XS),
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
                                Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, IReactiveQbserver<int>>), Constants.Observer.OB),
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
        public void ReactiveServiceContext_Subscribe_Parameterized_Simple2_15P()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observable.XS));

                    var ys = from x in xs("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7", "observable_parameter_8", "observable_parameter_9", "observable_parameter_10", "observable_parameter_11", "observable_parameter_12", "observable_parameter_13", "observable_parameter_14", "observable_parameter_15")
                             where x > 0
                             select x * x;

                    var ob = ctx.GetObserver<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observer.OB));
                    ys.Subscribe(ob("observer_parameter_1", "observer_parameter_2", "observer_parameter_3", "observer_parameter_4", "observer_parameter_5", "observer_parameter_6", "observer_parameter_7", "observer_parameter_8", "observer_parameter_9", "observer_parameter_10", "observer_parameter_11", "observer_parameter_12", "observer_parameter_13", "observer_parameter_14", "observer_parameter_15"), new Uri(Constants.Subscription.SUB), null);
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
                                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, IReactiveQbservable<int>>), Constants.Observable.XS),
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
                                Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, IReactiveQbserver<int>>), Constants.Observer.OB),
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

        #region "ReactiveServiceContext_Subscribe_Parameterized_Simple3"

        [TestMethod]
        public void ReactiveServiceContext_Subscribe_Parameterized_Simple3_2P()
        {
            Apply(
                provider => new MyParameterizedContext(provider),
                ctx =>
                {
                    var xs = ctx.Xs2p("observable_parameter_1", "observable_parameter_2");
                    var ob = ctx.GetObserver<string, string, int>(new Uri(Constants.Observer.OB));
                    xs.Subscribe(ob("observer_parameter_1", "observer_parameter_2"), new Uri(Constants.Subscription.SUB), null);
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<IReactiveQbservable<int>, IReactiveQbserver<int>, IReactiveQubscription>), Constants.SubscribeUri),
                        Expression.Invoke(
                            Expression.Parameter(typeof(Func<string, string, IReactiveQbservable<int>>), Constants.Observable.XS + 2),
                            Expression.Constant("observable_parameter_1"),
                            Expression.Constant("observable_parameter_2")
                        ),
                        Expression.Invoke(
                            Expression.Parameter(typeof(Func<string, string, IReactiveQbserver<int>>), Constants.Observer.OB),
                            Expression.Constant("observer_parameter_1"),
                            Expression.Constant("observer_parameter_2")
                        )
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_Subscribe_Parameterized_Simple3_3P()
        {
            Apply(
                provider => new MyParameterizedContext(provider),
                ctx =>
                {
                    var xs = ctx.Xs3p("observable_parameter_1", "observable_parameter_2", "observable_parameter_3");
                    var ob = ctx.GetObserver<string, string, string, int>(new Uri(Constants.Observer.OB));
                    xs.Subscribe(ob("observer_parameter_1", "observer_parameter_2", "observer_parameter_3"), new Uri(Constants.Subscription.SUB), null);
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<IReactiveQbservable<int>, IReactiveQbserver<int>, IReactiveQubscription>), Constants.SubscribeUri),
                        Expression.Invoke(
                            Expression.Parameter(typeof(Func<string, string, string, IReactiveQbservable<int>>), Constants.Observable.XS + 3),
                            Expression.Constant("observable_parameter_1"),
                            Expression.Constant("observable_parameter_2"),
                            Expression.Constant("observable_parameter_3")
                        ),
                        Expression.Invoke(
                            Expression.Parameter(typeof(Func<string, string, string, IReactiveQbserver<int>>), Constants.Observer.OB),
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
        public void ReactiveServiceContext_Subscribe_Parameterized_Simple3_4P()
        {
            Apply(
                provider => new MyParameterizedContext(provider),
                ctx =>
                {
                    var xs = ctx.Xs4p("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4");
                    var ob = ctx.GetObserver<string, string, string, string, int>(new Uri(Constants.Observer.OB));
                    xs.Subscribe(ob("observer_parameter_1", "observer_parameter_2", "observer_parameter_3", "observer_parameter_4"), new Uri(Constants.Subscription.SUB), null);
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<IReactiveQbservable<int>, IReactiveQbserver<int>, IReactiveQubscription>), Constants.SubscribeUri),
                        Expression.Invoke(
                            Expression.Parameter(typeof(Func<string, string, string, string, IReactiveQbservable<int>>), Constants.Observable.XS + 4),
                            Expression.Constant("observable_parameter_1"),
                            Expression.Constant("observable_parameter_2"),
                            Expression.Constant("observable_parameter_3"),
                            Expression.Constant("observable_parameter_4")
                        ),
                        Expression.Invoke(
                            Expression.Parameter(typeof(Func<string, string, string, string, IReactiveQbserver<int>>), Constants.Observer.OB),
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
        public void ReactiveServiceContext_Subscribe_Parameterized_Simple3_5P()
        {
            Apply(
                provider => new MyParameterizedContext(provider),
                ctx =>
                {
                    var xs = ctx.Xs5p("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5");
                    var ob = ctx.GetObserver<string, string, string, string, string, int>(new Uri(Constants.Observer.OB));
                    xs.Subscribe(ob("observer_parameter_1", "observer_parameter_2", "observer_parameter_3", "observer_parameter_4", "observer_parameter_5"), new Uri(Constants.Subscription.SUB), null);
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<IReactiveQbservable<int>, IReactiveQbserver<int>, IReactiveQubscription>), Constants.SubscribeUri),
                        Expression.Invoke(
                            Expression.Parameter(typeof(Func<string, string, string, string, string, IReactiveQbservable<int>>), Constants.Observable.XS + 5),
                            Expression.Constant("observable_parameter_1"),
                            Expression.Constant("observable_parameter_2"),
                            Expression.Constant("observable_parameter_3"),
                            Expression.Constant("observable_parameter_4"),
                            Expression.Constant("observable_parameter_5")
                        ),
                        Expression.Invoke(
                            Expression.Parameter(typeof(Func<string, string, string, string, string, IReactiveQbserver<int>>), Constants.Observer.OB),
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
        public void ReactiveServiceContext_Subscribe_Parameterized_Simple3_6P()
        {
            Apply(
                provider => new MyParameterizedContext(provider),
                ctx =>
                {
                    var xs = ctx.Xs6p("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6");
                    var ob = ctx.GetObserver<string, string, string, string, string, string, int>(new Uri(Constants.Observer.OB));
                    xs.Subscribe(ob("observer_parameter_1", "observer_parameter_2", "observer_parameter_3", "observer_parameter_4", "observer_parameter_5", "observer_parameter_6"), new Uri(Constants.Subscription.SUB), null);
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<IReactiveQbservable<int>, IReactiveQbserver<int>, IReactiveQubscription>), Constants.SubscribeUri),
                        Expression.Invoke(
                            Expression.Parameter(typeof(Func<string, string, string, string, string, string, IReactiveQbservable<int>>), Constants.Observable.XS + 6),
                            Expression.Constant("observable_parameter_1"),
                            Expression.Constant("observable_parameter_2"),
                            Expression.Constant("observable_parameter_3"),
                            Expression.Constant("observable_parameter_4"),
                            Expression.Constant("observable_parameter_5"),
                            Expression.Constant("observable_parameter_6")
                        ),
                        Expression.Invoke(
                            Expression.Parameter(typeof(Func<string, string, string, string, string, string, IReactiveQbserver<int>>), Constants.Observer.OB),
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
        public void ReactiveServiceContext_Subscribe_Parameterized_Simple3_7P()
        {
            Apply(
                provider => new MyParameterizedContext(provider),
                ctx =>
                {
                    var xs = ctx.Xs7p("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7");
                    var ob = ctx.GetObserver<string, string, string, string, string, string, string, int>(new Uri(Constants.Observer.OB));
                    xs.Subscribe(ob("observer_parameter_1", "observer_parameter_2", "observer_parameter_3", "observer_parameter_4", "observer_parameter_5", "observer_parameter_6", "observer_parameter_7"), new Uri(Constants.Subscription.SUB), null);
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<IReactiveQbservable<int>, IReactiveQbserver<int>, IReactiveQubscription>), Constants.SubscribeUri),
                        Expression.Invoke(
                            Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, IReactiveQbservable<int>>), Constants.Observable.XS + 7),
                            Expression.Constant("observable_parameter_1"),
                            Expression.Constant("observable_parameter_2"),
                            Expression.Constant("observable_parameter_3"),
                            Expression.Constant("observable_parameter_4"),
                            Expression.Constant("observable_parameter_5"),
                            Expression.Constant("observable_parameter_6"),
                            Expression.Constant("observable_parameter_7")
                        ),
                        Expression.Invoke(
                            Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, IReactiveQbserver<int>>), Constants.Observer.OB),
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
        public void ReactiveServiceContext_Subscribe_Parameterized_Simple3_8P()
        {
            Apply(
                provider => new MyParameterizedContext(provider),
                ctx =>
                {
                    var xs = ctx.Xs8p("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7", "observable_parameter_8");
                    var ob = ctx.GetObserver<string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observer.OB));
                    xs.Subscribe(ob("observer_parameter_1", "observer_parameter_2", "observer_parameter_3", "observer_parameter_4", "observer_parameter_5", "observer_parameter_6", "observer_parameter_7", "observer_parameter_8"), new Uri(Constants.Subscription.SUB), null);
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<IReactiveQbservable<int>, IReactiveQbserver<int>, IReactiveQubscription>), Constants.SubscribeUri),
                        Expression.Invoke(
                            Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, IReactiveQbservable<int>>), Constants.Observable.XS + 8),
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
                            Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, IReactiveQbserver<int>>), Constants.Observer.OB),
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
        public void ReactiveServiceContext_Subscribe_Parameterized_Simple3_9P()
        {
            Apply(
                provider => new MyParameterizedContext(provider),
                ctx =>
                {
                    var xs = ctx.Xs9p("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7", "observable_parameter_8", "observable_parameter_9");
                    var ob = ctx.GetObserver<string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observer.OB));
                    xs.Subscribe(ob("observer_parameter_1", "observer_parameter_2", "observer_parameter_3", "observer_parameter_4", "observer_parameter_5", "observer_parameter_6", "observer_parameter_7", "observer_parameter_8", "observer_parameter_9"), new Uri(Constants.Subscription.SUB), null);
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<IReactiveQbservable<int>, IReactiveQbserver<int>, IReactiveQubscription>), Constants.SubscribeUri),
                        Expression.Invoke(
                            Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, IReactiveQbservable<int>>), Constants.Observable.XS + 9),
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
                            Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, IReactiveQbserver<int>>), Constants.Observer.OB),
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
        public void ReactiveServiceContext_Subscribe_Parameterized_Simple3_10P()
        {
            Apply(
                provider => new MyParameterizedContext(provider),
                ctx =>
                {
                    var xs = ctx.Xs10p("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7", "observable_parameter_8", "observable_parameter_9", "observable_parameter_10");
                    var ob = ctx.GetObserver<string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observer.OB));
                    xs.Subscribe(ob("observer_parameter_1", "observer_parameter_2", "observer_parameter_3", "observer_parameter_4", "observer_parameter_5", "observer_parameter_6", "observer_parameter_7", "observer_parameter_8", "observer_parameter_9", "observer_parameter_10"), new Uri(Constants.Subscription.SUB), null);
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<IReactiveQbservable<int>, IReactiveQbserver<int>, IReactiveQubscription>), Constants.SubscribeUri),
                        Expression.Invoke(
                            Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, IReactiveQbservable<int>>), Constants.Observable.XS + 10),
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
                            Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, IReactiveQbserver<int>>), Constants.Observer.OB),
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
        public void ReactiveServiceContext_Subscribe_Parameterized_Simple3_11P()
        {
            Apply(
                provider => new MyParameterizedContext(provider),
                ctx =>
                {
                    var xs = ctx.Xs11p("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7", "observable_parameter_8", "observable_parameter_9", "observable_parameter_10", "observable_parameter_11");
                    var ob = ctx.GetObserver<string, string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observer.OB));
                    xs.Subscribe(ob("observer_parameter_1", "observer_parameter_2", "observer_parameter_3", "observer_parameter_4", "observer_parameter_5", "observer_parameter_6", "observer_parameter_7", "observer_parameter_8", "observer_parameter_9", "observer_parameter_10", "observer_parameter_11"), new Uri(Constants.Subscription.SUB), null);
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<IReactiveQbservable<int>, IReactiveQbserver<int>, IReactiveQubscription>), Constants.SubscribeUri),
                        Expression.Invoke(
                            Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, IReactiveQbservable<int>>), Constants.Observable.XS + 11),
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
                            Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, IReactiveQbserver<int>>), Constants.Observer.OB),
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
        public void ReactiveServiceContext_Subscribe_Parameterized_Simple3_12P()
        {
            Apply(
                provider => new MyParameterizedContext(provider),
                ctx =>
                {
                    var xs = ctx.Xs12p("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7", "observable_parameter_8", "observable_parameter_9", "observable_parameter_10", "observable_parameter_11", "observable_parameter_12");
                    var ob = ctx.GetObserver<string, string, string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observer.OB));
                    xs.Subscribe(ob("observer_parameter_1", "observer_parameter_2", "observer_parameter_3", "observer_parameter_4", "observer_parameter_5", "observer_parameter_6", "observer_parameter_7", "observer_parameter_8", "observer_parameter_9", "observer_parameter_10", "observer_parameter_11", "observer_parameter_12"), new Uri(Constants.Subscription.SUB), null);
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<IReactiveQbservable<int>, IReactiveQbserver<int>, IReactiveQubscription>), Constants.SubscribeUri),
                        Expression.Invoke(
                            Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, IReactiveQbservable<int>>), Constants.Observable.XS + 12),
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
                            Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, IReactiveQbserver<int>>), Constants.Observer.OB),
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
        public void ReactiveServiceContext_Subscribe_Parameterized_Simple3_13P()
        {
            Apply(
                provider => new MyParameterizedContext(provider),
                ctx =>
                {
                    var xs = ctx.Xs13p("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7", "observable_parameter_8", "observable_parameter_9", "observable_parameter_10", "observable_parameter_11", "observable_parameter_12", "observable_parameter_13");
                    var ob = ctx.GetObserver<string, string, string, string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observer.OB));
                    xs.Subscribe(ob("observer_parameter_1", "observer_parameter_2", "observer_parameter_3", "observer_parameter_4", "observer_parameter_5", "observer_parameter_6", "observer_parameter_7", "observer_parameter_8", "observer_parameter_9", "observer_parameter_10", "observer_parameter_11", "observer_parameter_12", "observer_parameter_13"), new Uri(Constants.Subscription.SUB), null);
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<IReactiveQbservable<int>, IReactiveQbserver<int>, IReactiveQubscription>), Constants.SubscribeUri),
                        Expression.Invoke(
                            Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, IReactiveQbservable<int>>), Constants.Observable.XS + 13),
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
                            Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, IReactiveQbserver<int>>), Constants.Observer.OB),
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
        public void ReactiveServiceContext_Subscribe_Parameterized_Simple3_14P()
        {
            Apply(
                provider => new MyParameterizedContext(provider),
                ctx =>
                {
                    var xs = ctx.Xs14p("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7", "observable_parameter_8", "observable_parameter_9", "observable_parameter_10", "observable_parameter_11", "observable_parameter_12", "observable_parameter_13", "observable_parameter_14");
                    var ob = ctx.GetObserver<string, string, string, string, string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observer.OB));
                    xs.Subscribe(ob("observer_parameter_1", "observer_parameter_2", "observer_parameter_3", "observer_parameter_4", "observer_parameter_5", "observer_parameter_6", "observer_parameter_7", "observer_parameter_8", "observer_parameter_9", "observer_parameter_10", "observer_parameter_11", "observer_parameter_12", "observer_parameter_13", "observer_parameter_14"), new Uri(Constants.Subscription.SUB), null);
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<IReactiveQbservable<int>, IReactiveQbserver<int>, IReactiveQubscription>), Constants.SubscribeUri),
                        Expression.Invoke(
                            Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, IReactiveQbservable<int>>), Constants.Observable.XS + 14),
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
                            Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, IReactiveQbserver<int>>), Constants.Observer.OB),
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
        public void ReactiveServiceContext_Subscribe_Parameterized_Simple3_15P()
        {
            Apply(
                provider => new MyParameterizedContext(provider),
                ctx =>
                {
                    var xs = ctx.Xs15p("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7", "observable_parameter_8", "observable_parameter_9", "observable_parameter_10", "observable_parameter_11", "observable_parameter_12", "observable_parameter_13", "observable_parameter_14", "observable_parameter_15");
                    var ob = ctx.GetObserver<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observer.OB));
                    xs.Subscribe(ob("observer_parameter_1", "observer_parameter_2", "observer_parameter_3", "observer_parameter_4", "observer_parameter_5", "observer_parameter_6", "observer_parameter_7", "observer_parameter_8", "observer_parameter_9", "observer_parameter_10", "observer_parameter_11", "observer_parameter_12", "observer_parameter_13", "observer_parameter_14", "observer_parameter_15"), new Uri(Constants.Subscription.SUB), null);
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<IReactiveQbservable<int>, IReactiveQbserver<int>, IReactiveQubscription>), Constants.SubscribeUri),
                        Expression.Invoke(
                            Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, IReactiveQbservable<int>>), Constants.Observable.XS + 15),
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
                            Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, IReactiveQbserver<int>>), Constants.Observer.OB),
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

        #region "ReactiveServiceContext_Subscribe_Parameterized_Closure1"

        [TestMethod]
        public void ReactiveServiceContext_Subscribe_Parameterized_Closure1_2P()
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
                        ys.Subscribe(ob("observer_parameter_1", "observer_parameter_2"), new Uri(Constants.Subscription.SUB), null);
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
                                        Expression.Invoke(
                                            Expression.Parameter(typeof(Func<string, string, IReactiveQbservable<int>>), Constants.Observable.XS),
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
                                    Expression.Parameter(typeof(Func<string, string, IReactiveQbserver<int>>), Constants.Observer.OB),
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
        public void ReactiveServiceContext_Subscribe_Parameterized_Closure1_3P()
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
                        ys.Subscribe(ob("observer_parameter_1", "observer_parameter_2", "observer_parameter_3"), new Uri(Constants.Subscription.SUB), null);
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
                                        Expression.Invoke(
                                            Expression.Parameter(typeof(Func<string, string, string, IReactiveQbservable<int>>), Constants.Observable.XS),
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
                                    Expression.Parameter(typeof(Func<string, string, string, IReactiveQbserver<int>>), Constants.Observer.OB),
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
        public void ReactiveServiceContext_Subscribe_Parameterized_Closure1_4P()
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
                        ys.Subscribe(ob("observer_parameter_1", "observer_parameter_2", "observer_parameter_3", "observer_parameter_4"), new Uri(Constants.Subscription.SUB), null);
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
                                        Expression.Invoke(
                                            Expression.Parameter(typeof(Func<string, string, string, string, IReactiveQbservable<int>>), Constants.Observable.XS),
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
                                    Expression.Parameter(typeof(Func<string, string, string, string, IReactiveQbserver<int>>), Constants.Observer.OB),
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
        public void ReactiveServiceContext_Subscribe_Parameterized_Closure1_5P()
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
                        ys.Subscribe(ob("observer_parameter_1", "observer_parameter_2", "observer_parameter_3", "observer_parameter_4", "observer_parameter_5"), new Uri(Constants.Subscription.SUB), null);
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
                                        Expression.Invoke(
                                            Expression.Parameter(typeof(Func<string, string, string, string, string, IReactiveQbservable<int>>), Constants.Observable.XS),
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
                                    Expression.Parameter(typeof(Func<string, string, string, string, string, IReactiveQbserver<int>>), Constants.Observer.OB),
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
        public void ReactiveServiceContext_Subscribe_Parameterized_Closure1_6P()
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
                        ys.Subscribe(ob("observer_parameter_1", "observer_parameter_2", "observer_parameter_3", "observer_parameter_4", "observer_parameter_5", "observer_parameter_6"), new Uri(Constants.Subscription.SUB), null);
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
                                        Expression.Invoke(
                                            Expression.Parameter(typeof(Func<string, string, string, string, string, string, IReactiveQbservable<int>>), Constants.Observable.XS),
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
                                    Expression.Parameter(typeof(Func<string, string, string, string, string, string, IReactiveQbserver<int>>), Constants.Observer.OB),
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
        public void ReactiveServiceContext_Subscribe_Parameterized_Closure1_7P()
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
                        ys.Subscribe(ob("observer_parameter_1", "observer_parameter_2", "observer_parameter_3", "observer_parameter_4", "observer_parameter_5", "observer_parameter_6", "observer_parameter_7"), new Uri(Constants.Subscription.SUB), null);
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
                                        Expression.Invoke(
                                            Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, IReactiveQbservable<int>>), Constants.Observable.XS),
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
                                    Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, IReactiveQbserver<int>>), Constants.Observer.OB),
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
        public void ReactiveServiceContext_Subscribe_Parameterized_Closure1_8P()
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
                        ys.Subscribe(ob("observer_parameter_1", "observer_parameter_2", "observer_parameter_3", "observer_parameter_4", "observer_parameter_5", "observer_parameter_6", "observer_parameter_7", "observer_parameter_8"), new Uri(Constants.Subscription.SUB), null);
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
                                        Expression.Invoke(
                                            Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, IReactiveQbservable<int>>), Constants.Observable.XS),
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
                                    Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, IReactiveQbserver<int>>), Constants.Observer.OB),
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
        public void ReactiveServiceContext_Subscribe_Parameterized_Closure1_9P()
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
                        ys.Subscribe(ob("observer_parameter_1", "observer_parameter_2", "observer_parameter_3", "observer_parameter_4", "observer_parameter_5", "observer_parameter_6", "observer_parameter_7", "observer_parameter_8", "observer_parameter_9"), new Uri(Constants.Subscription.SUB), null);
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
                                        Expression.Invoke(
                                            Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, IReactiveQbservable<int>>), Constants.Observable.XS),
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
                                    Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, IReactiveQbserver<int>>), Constants.Observer.OB),
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
        public void ReactiveServiceContext_Subscribe_Parameterized_Closure1_10P()
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
                        ys.Subscribe(ob("observer_parameter_1", "observer_parameter_2", "observer_parameter_3", "observer_parameter_4", "observer_parameter_5", "observer_parameter_6", "observer_parameter_7", "observer_parameter_8", "observer_parameter_9", "observer_parameter_10"), new Uri(Constants.Subscription.SUB), null);
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
                                        Expression.Invoke(
                                            Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, IReactiveQbservable<int>>), Constants.Observable.XS),
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
                                    Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, IReactiveQbserver<int>>), Constants.Observer.OB),
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
        public void ReactiveServiceContext_Subscribe_Parameterized_Closure1_11P()
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
                        ys.Subscribe(ob("observer_parameter_1", "observer_parameter_2", "observer_parameter_3", "observer_parameter_4", "observer_parameter_5", "observer_parameter_6", "observer_parameter_7", "observer_parameter_8", "observer_parameter_9", "observer_parameter_10", "observer_parameter_11"), new Uri(Constants.Subscription.SUB), null);
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
                                        Expression.Invoke(
                                            Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, IReactiveQbservable<int>>), Constants.Observable.XS),
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
                                    Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, IReactiveQbserver<int>>), Constants.Observer.OB),
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
        public void ReactiveServiceContext_Subscribe_Parameterized_Closure1_12P()
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
                        ys.Subscribe(ob("observer_parameter_1", "observer_parameter_2", "observer_parameter_3", "observer_parameter_4", "observer_parameter_5", "observer_parameter_6", "observer_parameter_7", "observer_parameter_8", "observer_parameter_9", "observer_parameter_10", "observer_parameter_11", "observer_parameter_12"), new Uri(Constants.Subscription.SUB), null);
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
                                        Expression.Invoke(
                                            Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, IReactiveQbservable<int>>), Constants.Observable.XS),
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
                                    Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, IReactiveQbserver<int>>), Constants.Observer.OB),
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
        public void ReactiveServiceContext_Subscribe_Parameterized_Closure1_13P()
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
                        ys.Subscribe(ob("observer_parameter_1", "observer_parameter_2", "observer_parameter_3", "observer_parameter_4", "observer_parameter_5", "observer_parameter_6", "observer_parameter_7", "observer_parameter_8", "observer_parameter_9", "observer_parameter_10", "observer_parameter_11", "observer_parameter_12", "observer_parameter_13"), new Uri(Constants.Subscription.SUB), null);
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
                                        Expression.Invoke(
                                            Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, IReactiveQbservable<int>>), Constants.Observable.XS),
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
                                    Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, IReactiveQbserver<int>>), Constants.Observer.OB),
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
        public void ReactiveServiceContext_Subscribe_Parameterized_Closure1_14P()
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
                        ys.Subscribe(ob("observer_parameter_1", "observer_parameter_2", "observer_parameter_3", "observer_parameter_4", "observer_parameter_5", "observer_parameter_6", "observer_parameter_7", "observer_parameter_8", "observer_parameter_9", "observer_parameter_10", "observer_parameter_11", "observer_parameter_12", "observer_parameter_13", "observer_parameter_14"), new Uri(Constants.Subscription.SUB), null);
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
                                        Expression.Invoke(
                                            Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, IReactiveQbservable<int>>), Constants.Observable.XS),
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
                                    Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, IReactiveQbserver<int>>), Constants.Observer.OB),
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
        public void ReactiveServiceContext_Subscribe_Parameterized_Closure1_15P()
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
                        ys.Subscribe(ob("observer_parameter_1", "observer_parameter_2", "observer_parameter_3", "observer_parameter_4", "observer_parameter_5", "observer_parameter_6", "observer_parameter_7", "observer_parameter_8", "observer_parameter_9", "observer_parameter_10", "observer_parameter_11", "observer_parameter_12", "observer_parameter_13", "observer_parameter_14", "observer_parameter_15"), new Uri(Constants.Subscription.SUB), null);
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
                                        Expression.Invoke(
                                            Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, IReactiveQbservable<int>>), Constants.Observable.XS),
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
                                    Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, IReactiveQbserver<int>>), Constants.Observer.OB),
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

        #region "ReactiveServiceContext_Subscribe_Parameterized_SelectMany1_Simple"

        [TestMethod]
        public void ReactiveServiceContext_Subscribe_Parameterized_SelectMany1_Simple_2P()
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
                                    Expression.Invoke(
                                        Expression.Parameter(typeof(Func<string, string, IReactiveQbservable<int>>), Constants.Observable.XS),
                                        Expression.Constant("observable_parameter_1"),
                                        Expression.Constant("observable_parameter_2")
                                    ),
                                    Expression.Lambda(
                                        Expression.Invoke(
                                        Expression.Parameter(typeof(Func<int, int, IReactiveQbservable<string>>), Constants.Observable.YS),
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
                                Expression.Parameter(typeof(IReactiveQbserver<int>), Constants.Observer.OB)
                            )
                        )
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_Subscribe_Parameterized_SelectMany1_Simple_3P()
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
                                    Expression.Invoke(
                                        Expression.Parameter(typeof(Func<string, string, string, IReactiveQbservable<int>>), Constants.Observable.XS),
                                        Expression.Constant("observable_parameter_1"),
                                        Expression.Constant("observable_parameter_2"),
                                        Expression.Constant("observable_parameter_3")
                                    ),
                                    Expression.Lambda(
                                        Expression.Invoke(
                                        Expression.Parameter(typeof(Func<int, int, int, IReactiveQbservable<string>>), Constants.Observable.YS),
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
                                Expression.Parameter(typeof(IReactiveQbserver<int>), Constants.Observer.OB)
                            )
                        )
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_Subscribe_Parameterized_SelectMany1_Simple_4P()
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
                                    Expression.Invoke(
                                        Expression.Parameter(typeof(Func<string, string, string, string, IReactiveQbservable<int>>), Constants.Observable.XS),
                                        Expression.Constant("observable_parameter_1"),
                                        Expression.Constant("observable_parameter_2"),
                                        Expression.Constant("observable_parameter_3"),
                                        Expression.Constant("observable_parameter_4")
                                    ),
                                    Expression.Lambda(
                                        Expression.Invoke(
                                        Expression.Parameter(typeof(Func<int, int, int, int, IReactiveQbservable<string>>), Constants.Observable.YS),
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
                                Expression.Parameter(typeof(IReactiveQbserver<int>), Constants.Observer.OB)
                            )
                        )
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_Subscribe_Parameterized_SelectMany1_Simple_5P()
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
                                    Expression.Invoke(
                                        Expression.Parameter(typeof(Func<string, string, string, string, string, IReactiveQbservable<int>>), Constants.Observable.XS),
                                        Expression.Constant("observable_parameter_1"),
                                        Expression.Constant("observable_parameter_2"),
                                        Expression.Constant("observable_parameter_3"),
                                        Expression.Constant("observable_parameter_4"),
                                        Expression.Constant("observable_parameter_5")
                                    ),
                                    Expression.Lambda(
                                        Expression.Invoke(
                                        Expression.Parameter(typeof(Func<int, int, int, int, int, IReactiveQbservable<string>>), Constants.Observable.YS),
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
                                Expression.Parameter(typeof(IReactiveQbserver<int>), Constants.Observer.OB)
                            )
                        )
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_Subscribe_Parameterized_SelectMany1_Simple_6P()
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
                                    Expression.Invoke(
                                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, IReactiveQbservable<int>>), Constants.Observable.XS),
                                        Expression.Constant("observable_parameter_1"),
                                        Expression.Constant("observable_parameter_2"),
                                        Expression.Constant("observable_parameter_3"),
                                        Expression.Constant("observable_parameter_4"),
                                        Expression.Constant("observable_parameter_5"),
                                        Expression.Constant("observable_parameter_6")
                                    ),
                                    Expression.Lambda(
                                        Expression.Invoke(
                                        Expression.Parameter(typeof(Func<int, int, int, int, int, int, IReactiveQbservable<string>>), Constants.Observable.YS),
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
                                Expression.Parameter(typeof(IReactiveQbserver<int>), Constants.Observer.OB)
                            )
                        )
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_Subscribe_Parameterized_SelectMany1_Simple_7P()
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
                                    Expression.Invoke(
                                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, IReactiveQbservable<int>>), Constants.Observable.XS),
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
                                        Expression.Parameter(typeof(Func<int, int, int, int, int, int, int, IReactiveQbservable<string>>), Constants.Observable.YS),
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
                                Expression.Parameter(typeof(IReactiveQbserver<int>), Constants.Observer.OB)
                            )
                        )
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_Subscribe_Parameterized_SelectMany1_Simple_8P()
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
                                    Expression.Invoke(
                                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, IReactiveQbservable<int>>), Constants.Observable.XS),
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
                                        Expression.Parameter(typeof(Func<int, int, int, int, int, int, int, int, IReactiveQbservable<string>>), Constants.Observable.YS),
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
                                Expression.Parameter(typeof(IReactiveQbserver<int>), Constants.Observer.OB)
                            )
                        )
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_Subscribe_Parameterized_SelectMany1_Simple_9P()
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
                                    Expression.Invoke(
                                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, IReactiveQbservable<int>>), Constants.Observable.XS),
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
                                        Expression.Parameter(typeof(Func<int, int, int, int, int, int, int, int, int, IReactiveQbservable<string>>), Constants.Observable.YS),
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
                                Expression.Parameter(typeof(IReactiveQbserver<int>), Constants.Observer.OB)
                            )
                        )
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_Subscribe_Parameterized_SelectMany1_Simple_10P()
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
                                    Expression.Invoke(
                                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, IReactiveQbservable<int>>), Constants.Observable.XS),
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
                                        Expression.Parameter(typeof(Func<int, int, int, int, int, int, int, int, int, int, IReactiveQbservable<string>>), Constants.Observable.YS),
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
                                Expression.Parameter(typeof(IReactiveQbserver<int>), Constants.Observer.OB)
                            )
                        )
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_Subscribe_Parameterized_SelectMany1_Simple_11P()
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
                                    Expression.Invoke(
                                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, IReactiveQbservable<int>>), Constants.Observable.XS),
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
                                        Expression.Parameter(typeof(Func<int, int, int, int, int, int, int, int, int, int, int, IReactiveQbservable<string>>), Constants.Observable.YS),
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
                                Expression.Parameter(typeof(IReactiveQbserver<int>), Constants.Observer.OB)
                            )
                        )
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_Subscribe_Parameterized_SelectMany1_Simple_12P()
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
                                    Expression.Invoke(
                                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, IReactiveQbservable<int>>), Constants.Observable.XS),
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
                                        Expression.Parameter(typeof(Func<int, int, int, int, int, int, int, int, int, int, int, int, IReactiveQbservable<string>>), Constants.Observable.YS),
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
                                Expression.Parameter(typeof(IReactiveQbserver<int>), Constants.Observer.OB)
                            )
                        )
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_Subscribe_Parameterized_SelectMany1_Simple_13P()
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
                                    Expression.Invoke(
                                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, IReactiveQbservable<int>>), Constants.Observable.XS),
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
                                        Expression.Parameter(typeof(Func<int, int, int, int, int, int, int, int, int, int, int, int, int, IReactiveQbservable<string>>), Constants.Observable.YS),
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
                                Expression.Parameter(typeof(IReactiveQbserver<int>), Constants.Observer.OB)
                            )
                        )
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_Subscribe_Parameterized_SelectMany1_Simple_14P()
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
                                    Expression.Invoke(
                                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, IReactiveQbservable<int>>), Constants.Observable.XS),
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
                                        Expression.Parameter(typeof(Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, IReactiveQbservable<string>>), Constants.Observable.YS),
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
                                Expression.Parameter(typeof(IReactiveQbserver<int>), Constants.Observer.OB)
                            )
                        )
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_Subscribe_Parameterized_SelectMany1_Simple_15P()
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
                                    Expression.Invoke(
                                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, IReactiveQbservable<int>>), Constants.Observable.XS),
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
                                        Expression.Parameter(typeof(Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, IReactiveQbservable<string>>), Constants.Observable.YS),
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
                                Expression.Parameter(typeof(IReactiveQbserver<int>), Constants.Observer.OB)
                            )
                        )
                    ),
                    state: null
                )
            );
        }


        #endregion

        #region "ReactiveServiceContext_Subscribe_Parameterized_SelectMany2_Closure"

        [TestMethod]
        public void ReactiveServiceContext_Subscribe_Parameterized_SelectMany2_Closure_2P()
        {
            var xs = default(IReactiveQbservable<int>);
            var ys = default(Func<int, int, IReactiveQbservable<string>>);

            Apply(
                ctx =>
                {
                    xs = ctx.GetObservable<string, string, int>(new Uri(Constants.Observable.XS))("observable_parameter_1", "observable_parameter_2");
                    ys = ctx.GetObservable<int, int, string>(new Uri(Constants.Observable.YS));

                    var zs = from x in xs
                             from y in ys(x, x)
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
                                    Expression.Invoke(
                                        Expression.Parameter(typeof(Func<string, string, IReactiveQbservable<int>>), Constants.Observable.XS),
                                        Expression.Constant("observable_parameter_1"),
                                        Expression.Constant("observable_parameter_2")
                                    ),
                                    Expression.Lambda(
                                        Expression.Invoke(
                                            Expression.Parameter(typeof(Func<int, int, IReactiveQbservable<string>>), Constants.Observable.YS),
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
                                Expression.Parameter(typeof(IReactiveQbserver<int>), Constants.Observer.OB)
                            )
                        )
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_Subscribe_Parameterized_SelectMany2_Closure_3P()
        {
            var xs = default(IReactiveQbservable<int>);
            var ys = default(Func<int, int, int, IReactiveQbservable<string>>);

            Apply(
                ctx =>
                {
                    xs = ctx.GetObservable<string, string, string, int>(new Uri(Constants.Observable.XS))("observable_parameter_1", "observable_parameter_2", "observable_parameter_3");
                    ys = ctx.GetObservable<int, int, int, string>(new Uri(Constants.Observable.YS));

                    var zs = from x in xs
                             from y in ys(x, x, x)
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
                                    Expression.Invoke(
                                        Expression.Parameter(typeof(Func<string, string, string, IReactiveQbservable<int>>), Constants.Observable.XS),
                                        Expression.Constant("observable_parameter_1"),
                                        Expression.Constant("observable_parameter_2"),
                                        Expression.Constant("observable_parameter_3")
                                    ),
                                    Expression.Lambda(
                                        Expression.Invoke(
                                            Expression.Parameter(typeof(Func<int, int, int, IReactiveQbservable<string>>), Constants.Observable.YS),
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
                                Expression.Parameter(typeof(IReactiveQbserver<int>), Constants.Observer.OB)
                            )
                        )
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_Subscribe_Parameterized_SelectMany2_Closure_4P()
        {
            var xs = default(IReactiveQbservable<int>);
            var ys = default(Func<int, int, int, int, IReactiveQbservable<string>>);

            Apply(
                ctx =>
                {
                    xs = ctx.GetObservable<string, string, string, string, int>(new Uri(Constants.Observable.XS))("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4");
                    ys = ctx.GetObservable<int, int, int, int, string>(new Uri(Constants.Observable.YS));

                    var zs = from x in xs
                             from y in ys(x, x, x, x)
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
                                    Expression.Invoke(
                                        Expression.Parameter(typeof(Func<string, string, string, string, IReactiveQbservable<int>>), Constants.Observable.XS),
                                        Expression.Constant("observable_parameter_1"),
                                        Expression.Constant("observable_parameter_2"),
                                        Expression.Constant("observable_parameter_3"),
                                        Expression.Constant("observable_parameter_4")
                                    ),
                                    Expression.Lambda(
                                        Expression.Invoke(
                                            Expression.Parameter(typeof(Func<int, int, int, int, IReactiveQbservable<string>>), Constants.Observable.YS),
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
                                Expression.Parameter(typeof(IReactiveQbserver<int>), Constants.Observer.OB)
                            )
                        )
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_Subscribe_Parameterized_SelectMany2_Closure_5P()
        {
            var xs = default(IReactiveQbservable<int>);
            var ys = default(Func<int, int, int, int, int, IReactiveQbservable<string>>);

            Apply(
                ctx =>
                {
                    xs = ctx.GetObservable<string, string, string, string, string, int>(new Uri(Constants.Observable.XS))("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5");
                    ys = ctx.GetObservable<int, int, int, int, int, string>(new Uri(Constants.Observable.YS));

                    var zs = from x in xs
                             from y in ys(x, x, x, x, x)
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
                                    Expression.Invoke(
                                        Expression.Parameter(typeof(Func<string, string, string, string, string, IReactiveQbservable<int>>), Constants.Observable.XS),
                                        Expression.Constant("observable_parameter_1"),
                                        Expression.Constant("observable_parameter_2"),
                                        Expression.Constant("observable_parameter_3"),
                                        Expression.Constant("observable_parameter_4"),
                                        Expression.Constant("observable_parameter_5")
                                    ),
                                    Expression.Lambda(
                                        Expression.Invoke(
                                            Expression.Parameter(typeof(Func<int, int, int, int, int, IReactiveQbservable<string>>), Constants.Observable.YS),
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
                                Expression.Parameter(typeof(IReactiveQbserver<int>), Constants.Observer.OB)
                            )
                        )
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_Subscribe_Parameterized_SelectMany2_Closure_6P()
        {
            var xs = default(IReactiveQbservable<int>);
            var ys = default(Func<int, int, int, int, int, int, IReactiveQbservable<string>>);

            Apply(
                ctx =>
                {
                    xs = ctx.GetObservable<string, string, string, string, string, string, int>(new Uri(Constants.Observable.XS))("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6");
                    ys = ctx.GetObservable<int, int, int, int, int, int, string>(new Uri(Constants.Observable.YS));

                    var zs = from x in xs
                             from y in ys(x, x, x, x, x, x)
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
                                    Expression.Invoke(
                                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, IReactiveQbservable<int>>), Constants.Observable.XS),
                                        Expression.Constant("observable_parameter_1"),
                                        Expression.Constant("observable_parameter_2"),
                                        Expression.Constant("observable_parameter_3"),
                                        Expression.Constant("observable_parameter_4"),
                                        Expression.Constant("observable_parameter_5"),
                                        Expression.Constant("observable_parameter_6")
                                    ),
                                    Expression.Lambda(
                                        Expression.Invoke(
                                            Expression.Parameter(typeof(Func<int, int, int, int, int, int, IReactiveQbservable<string>>), Constants.Observable.YS),
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
                                Expression.Parameter(typeof(IReactiveQbserver<int>), Constants.Observer.OB)
                            )
                        )
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_Subscribe_Parameterized_SelectMany2_Closure_7P()
        {
            var xs = default(IReactiveQbservable<int>);
            var ys = default(Func<int, int, int, int, int, int, int, IReactiveQbservable<string>>);

            Apply(
                ctx =>
                {
                    xs = ctx.GetObservable<string, string, string, string, string, string, string, int>(new Uri(Constants.Observable.XS))("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7");
                    ys = ctx.GetObservable<int, int, int, int, int, int, int, string>(new Uri(Constants.Observable.YS));

                    var zs = from x in xs
                             from y in ys(x, x, x, x, x, x, x)
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
                                    Expression.Invoke(
                                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, IReactiveQbservable<int>>), Constants.Observable.XS),
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
                                            Expression.Parameter(typeof(Func<int, int, int, int, int, int, int, IReactiveQbservable<string>>), Constants.Observable.YS),
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
                                Expression.Parameter(typeof(IReactiveQbserver<int>), Constants.Observer.OB)
                            )
                        )
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_Subscribe_Parameterized_SelectMany2_Closure_8P()
        {
            var xs = default(IReactiveQbservable<int>);
            var ys = default(Func<int, int, int, int, int, int, int, int, IReactiveQbservable<string>>);

            Apply(
                ctx =>
                {
                    xs = ctx.GetObservable<string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observable.XS))("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7", "observable_parameter_8");
                    ys = ctx.GetObservable<int, int, int, int, int, int, int, int, string>(new Uri(Constants.Observable.YS));

                    var zs = from x in xs
                             from y in ys(x, x, x, x, x, x, x, x)
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
                                    Expression.Invoke(
                                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, IReactiveQbservable<int>>), Constants.Observable.XS),
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
                                            Expression.Parameter(typeof(Func<int, int, int, int, int, int, int, int, IReactiveQbservable<string>>), Constants.Observable.YS),
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
                                Expression.Parameter(typeof(IReactiveQbserver<int>), Constants.Observer.OB)
                            )
                        )
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_Subscribe_Parameterized_SelectMany2_Closure_9P()
        {
            var xs = default(IReactiveQbservable<int>);
            var ys = default(Func<int, int, int, int, int, int, int, int, int, IReactiveQbservable<string>>);

            Apply(
                ctx =>
                {
                    xs = ctx.GetObservable<string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observable.XS))("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7", "observable_parameter_8", "observable_parameter_9");
                    ys = ctx.GetObservable<int, int, int, int, int, int, int, int, int, string>(new Uri(Constants.Observable.YS));

                    var zs = from x in xs
                             from y in ys(x, x, x, x, x, x, x, x, x)
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
                                    Expression.Invoke(
                                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, IReactiveQbservable<int>>), Constants.Observable.XS),
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
                                            Expression.Parameter(typeof(Func<int, int, int, int, int, int, int, int, int, IReactiveQbservable<string>>), Constants.Observable.YS),
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
                                Expression.Parameter(typeof(IReactiveQbserver<int>), Constants.Observer.OB)
                            )
                        )
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_Subscribe_Parameterized_SelectMany2_Closure_10P()
        {
            var xs = default(IReactiveQbservable<int>);
            var ys = default(Func<int, int, int, int, int, int, int, int, int, int, IReactiveQbservable<string>>);

            Apply(
                ctx =>
                {
                    xs = ctx.GetObservable<string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observable.XS))("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7", "observable_parameter_8", "observable_parameter_9", "observable_parameter_10");
                    ys = ctx.GetObservable<int, int, int, int, int, int, int, int, int, int, string>(new Uri(Constants.Observable.YS));

                    var zs = from x in xs
                             from y in ys(x, x, x, x, x, x, x, x, x, x)
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
                                    Expression.Invoke(
                                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, IReactiveQbservable<int>>), Constants.Observable.XS),
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
                                            Expression.Parameter(typeof(Func<int, int, int, int, int, int, int, int, int, int, IReactiveQbservable<string>>), Constants.Observable.YS),
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
                                Expression.Parameter(typeof(IReactiveQbserver<int>), Constants.Observer.OB)
                            )
                        )
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_Subscribe_Parameterized_SelectMany2_Closure_11P()
        {
            var xs = default(IReactiveQbservable<int>);
            var ys = default(Func<int, int, int, int, int, int, int, int, int, int, int, IReactiveQbservable<string>>);

            Apply(
                ctx =>
                {
                    xs = ctx.GetObservable<string, string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observable.XS))("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7", "observable_parameter_8", "observable_parameter_9", "observable_parameter_10", "observable_parameter_11");
                    ys = ctx.GetObservable<int, int, int, int, int, int, int, int, int, int, int, string>(new Uri(Constants.Observable.YS));

                    var zs = from x in xs
                             from y in ys(x, x, x, x, x, x, x, x, x, x, x)
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
                                    Expression.Invoke(
                                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, IReactiveQbservable<int>>), Constants.Observable.XS),
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
                                            Expression.Parameter(typeof(Func<int, int, int, int, int, int, int, int, int, int, int, IReactiveQbservable<string>>), Constants.Observable.YS),
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
                                Expression.Parameter(typeof(IReactiveQbserver<int>), Constants.Observer.OB)
                            )
                        )
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_Subscribe_Parameterized_SelectMany2_Closure_12P()
        {
            var xs = default(IReactiveQbservable<int>);
            var ys = default(Func<int, int, int, int, int, int, int, int, int, int, int, int, IReactiveQbservable<string>>);

            Apply(
                ctx =>
                {
                    xs = ctx.GetObservable<string, string, string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observable.XS))("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7", "observable_parameter_8", "observable_parameter_9", "observable_parameter_10", "observable_parameter_11", "observable_parameter_12");
                    ys = ctx.GetObservable<int, int, int, int, int, int, int, int, int, int, int, int, string>(new Uri(Constants.Observable.YS));

                    var zs = from x in xs
                             from y in ys(x, x, x, x, x, x, x, x, x, x, x, x)
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
                                    Expression.Invoke(
                                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, IReactiveQbservable<int>>), Constants.Observable.XS),
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
                                            Expression.Parameter(typeof(Func<int, int, int, int, int, int, int, int, int, int, int, int, IReactiveQbservable<string>>), Constants.Observable.YS),
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
                                Expression.Parameter(typeof(IReactiveQbserver<int>), Constants.Observer.OB)
                            )
                        )
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_Subscribe_Parameterized_SelectMany2_Closure_13P()
        {
            var xs = default(IReactiveQbservable<int>);
            var ys = default(Func<int, int, int, int, int, int, int, int, int, int, int, int, int, IReactiveQbservable<string>>);

            Apply(
                ctx =>
                {
                    xs = ctx.GetObservable<string, string, string, string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observable.XS))("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7", "observable_parameter_8", "observable_parameter_9", "observable_parameter_10", "observable_parameter_11", "observable_parameter_12", "observable_parameter_13");
                    ys = ctx.GetObservable<int, int, int, int, int, int, int, int, int, int, int, int, int, string>(new Uri(Constants.Observable.YS));

                    var zs = from x in xs
                             from y in ys(x, x, x, x, x, x, x, x, x, x, x, x, x)
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
                                    Expression.Invoke(
                                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, IReactiveQbservable<int>>), Constants.Observable.XS),
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
                                            Expression.Parameter(typeof(Func<int, int, int, int, int, int, int, int, int, int, int, int, int, IReactiveQbservable<string>>), Constants.Observable.YS),
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
                                Expression.Parameter(typeof(IReactiveQbserver<int>), Constants.Observer.OB)
                            )
                        )
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_Subscribe_Parameterized_SelectMany2_Closure_14P()
        {
            var xs = default(IReactiveQbservable<int>);
            var ys = default(Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, IReactiveQbservable<string>>);

            Apply(
                ctx =>
                {
                    xs = ctx.GetObservable<string, string, string, string, string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observable.XS))("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7", "observable_parameter_8", "observable_parameter_9", "observable_parameter_10", "observable_parameter_11", "observable_parameter_12", "observable_parameter_13", "observable_parameter_14");
                    ys = ctx.GetObservable<int, int, int, int, int, int, int, int, int, int, int, int, int, int, string>(new Uri(Constants.Observable.YS));

                    var zs = from x in xs
                             from y in ys(x, x, x, x, x, x, x, x, x, x, x, x, x, x)
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
                                    Expression.Invoke(
                                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, IReactiveQbservable<int>>), Constants.Observable.XS),
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
                                            Expression.Parameter(typeof(Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, IReactiveQbservable<string>>), Constants.Observable.YS),
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
                                Expression.Parameter(typeof(IReactiveQbserver<int>), Constants.Observer.OB)
                            )
                        )
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_Subscribe_Parameterized_SelectMany2_Closure_15P()
        {
            var xs = default(IReactiveQbservable<int>);
            var ys = default(Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, IReactiveQbservable<string>>);

            Apply(
                ctx =>
                {
                    xs = ctx.GetObservable<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observable.XS))("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7", "observable_parameter_8", "observable_parameter_9", "observable_parameter_10", "observable_parameter_11", "observable_parameter_12", "observable_parameter_13", "observable_parameter_14", "observable_parameter_15");
                    ys = ctx.GetObservable<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, string>(new Uri(Constants.Observable.YS));

                    var zs = from x in xs
                             from y in ys(x, x, x, x, x, x, x, x, x, x, x, x, x, x, x)
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
                                    Expression.Invoke(
                                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, IReactiveQbservable<int>>), Constants.Observable.XS),
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
                                            Expression.Parameter(typeof(Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, IReactiveQbservable<string>>), Constants.Observable.YS),
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
                                Expression.Parameter(typeof(IReactiveQbserver<int>), Constants.Observer.OB)
                            )
                        )
                    ),
                    state: null
                )
            );
        }


        #endregion

        #region "ReactiveServiceContext_Subscribe_Parameterized_SelectMany3_CustomContext"

        [TestMethod]
        public void ReactiveServiceContext_Subscribe_Parameterized_SelectMany3_CustomContext_2P()
        {
            Apply(
                provider => new MyParameterizedContext(provider),
                ctx =>
                {
                    var zs = from x1 in ctx.Xs2p("observable_parameter_1", "observable_parameter_2")
                             from x2 in ctx.Xs2p("observable_parameter_3", "observable_parameter_4")
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
                                    Expression.Invoke(
                                        Expression.Parameter(typeof(Func<string, string, IReactiveQbservable<int>>), Constants.Observable.XS + 2),
                                        Expression.Constant("observable_parameter_1"),
                                        Expression.Constant("observable_parameter_2")
                                    ),
                                    Expression.Lambda(
                                        Expression.Invoke(
                                            Expression.Parameter(typeof(Func<string, string, IReactiveQbservable<int>>), Constants.Observable.XS + 2),
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
                                Expression.Parameter(typeof(IReactiveQbserver<int>), Constants.Observer.OB)
                            )
                        )
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_Subscribe_Parameterized_SelectMany3_CustomContext_3P()
        {
            Apply(
                provider => new MyParameterizedContext(provider),
                ctx =>
                {
                    var zs = from x1 in ctx.Xs3p("observable_parameter_1", "observable_parameter_2", "observable_parameter_3")
                             from x2 in ctx.Xs3p("observable_parameter_4", "observable_parameter_5", "observable_parameter_6")
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
                                    Expression.Invoke(
                                        Expression.Parameter(typeof(Func<string, string, string, IReactiveQbservable<int>>), Constants.Observable.XS + 3),
                                        Expression.Constant("observable_parameter_1"),
                                        Expression.Constant("observable_parameter_2"),
                                        Expression.Constant("observable_parameter_3")
                                    ),
                                    Expression.Lambda(
                                        Expression.Invoke(
                                            Expression.Parameter(typeof(Func<string, string, string, IReactiveQbservable<int>>), Constants.Observable.XS + 3),
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
                                Expression.Parameter(typeof(IReactiveQbserver<int>), Constants.Observer.OB)
                            )
                        )
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_Subscribe_Parameterized_SelectMany3_CustomContext_4P()
        {
            Apply(
                provider => new MyParameterizedContext(provider),
                ctx =>
                {
                    var zs = from x1 in ctx.Xs4p("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4")
                             from x2 in ctx.Xs4p("observable_parameter_5", "observable_parameter_6", "observable_parameter_7", "observable_parameter_8")
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
                                    Expression.Invoke(
                                        Expression.Parameter(typeof(Func<string, string, string, string, IReactiveQbservable<int>>), Constants.Observable.XS + 4),
                                        Expression.Constant("observable_parameter_1"),
                                        Expression.Constant("observable_parameter_2"),
                                        Expression.Constant("observable_parameter_3"),
                                        Expression.Constant("observable_parameter_4")
                                    ),
                                    Expression.Lambda(
                                        Expression.Invoke(
                                            Expression.Parameter(typeof(Func<string, string, string, string, IReactiveQbservable<int>>), Constants.Observable.XS + 4),
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
                                Expression.Parameter(typeof(IReactiveQbserver<int>), Constants.Observer.OB)
                            )
                        )
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_Subscribe_Parameterized_SelectMany3_CustomContext_5P()
        {
            Apply(
                provider => new MyParameterizedContext(provider),
                ctx =>
                {
                    var zs = from x1 in ctx.Xs5p("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5")
                             from x2 in ctx.Xs5p("observable_parameter_6", "observable_parameter_7", "observable_parameter_8", "observable_parameter_9", "observable_parameter_10")
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
                                    Expression.Invoke(
                                        Expression.Parameter(typeof(Func<string, string, string, string, string, IReactiveQbservable<int>>), Constants.Observable.XS + 5),
                                        Expression.Constant("observable_parameter_1"),
                                        Expression.Constant("observable_parameter_2"),
                                        Expression.Constant("observable_parameter_3"),
                                        Expression.Constant("observable_parameter_4"),
                                        Expression.Constant("observable_parameter_5")
                                    ),
                                    Expression.Lambda(
                                        Expression.Invoke(
                                            Expression.Parameter(typeof(Func<string, string, string, string, string, IReactiveQbservable<int>>), Constants.Observable.XS + 5),
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
                                Expression.Parameter(typeof(IReactiveQbserver<int>), Constants.Observer.OB)
                            )
                        )
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_Subscribe_Parameterized_SelectMany3_CustomContext_6P()
        {
            Apply(
                provider => new MyParameterizedContext(provider),
                ctx =>
                {
                    var zs = from x1 in ctx.Xs6p("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6")
                             from x2 in ctx.Xs6p("observable_parameter_7", "observable_parameter_8", "observable_parameter_9", "observable_parameter_10", "observable_parameter_11", "observable_parameter_12")
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
                                    Expression.Invoke(
                                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, IReactiveQbservable<int>>), Constants.Observable.XS + 6),
                                        Expression.Constant("observable_parameter_1"),
                                        Expression.Constant("observable_parameter_2"),
                                        Expression.Constant("observable_parameter_3"),
                                        Expression.Constant("observable_parameter_4"),
                                        Expression.Constant("observable_parameter_5"),
                                        Expression.Constant("observable_parameter_6")
                                    ),
                                    Expression.Lambda(
                                        Expression.Invoke(
                                            Expression.Parameter(typeof(Func<string, string, string, string, string, string, IReactiveQbservable<int>>), Constants.Observable.XS + 6),
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
                                Expression.Parameter(typeof(IReactiveQbserver<int>), Constants.Observer.OB)
                            )
                        )
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_Subscribe_Parameterized_SelectMany3_CustomContext_7P()
        {
            Apply(
                provider => new MyParameterizedContext(provider),
                ctx =>
                {
                    var zs = from x1 in ctx.Xs7p("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7")
                             from x2 in ctx.Xs7p("observable_parameter_8", "observable_parameter_9", "observable_parameter_10", "observable_parameter_11", "observable_parameter_12", "observable_parameter_13", "observable_parameter_14")
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
                                    Expression.Invoke(
                                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, IReactiveQbservable<int>>), Constants.Observable.XS + 7),
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
                                            Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, IReactiveQbservable<int>>), Constants.Observable.XS + 7),
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
                                Expression.Parameter(typeof(IReactiveQbserver<int>), Constants.Observer.OB)
                            )
                        )
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_Subscribe_Parameterized_SelectMany3_CustomContext_8P()
        {
            Apply(
                provider => new MyParameterizedContext(provider),
                ctx =>
                {
                    var zs = from x1 in ctx.Xs8p("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7", "observable_parameter_8")
                             from x2 in ctx.Xs8p("observable_parameter_9", "observable_parameter_10", "observable_parameter_11", "observable_parameter_12", "observable_parameter_13", "observable_parameter_14", "observable_parameter_15", "observable_parameter_16")
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
                                    Expression.Invoke(
                                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, IReactiveQbservable<int>>), Constants.Observable.XS + 8),
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
                                            Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, IReactiveQbservable<int>>), Constants.Observable.XS + 8),
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
                                Expression.Parameter(typeof(IReactiveQbserver<int>), Constants.Observer.OB)
                            )
                        )
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_Subscribe_Parameterized_SelectMany3_CustomContext_9P()
        {
            Apply(
                provider => new MyParameterizedContext(provider),
                ctx =>
                {
                    var zs = from x1 in ctx.Xs9p("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7", "observable_parameter_8", "observable_parameter_9")
                             from x2 in ctx.Xs9p("observable_parameter_10", "observable_parameter_11", "observable_parameter_12", "observable_parameter_13", "observable_parameter_14", "observable_parameter_15", "observable_parameter_16", "observable_parameter_17", "observable_parameter_18")
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
                                    Expression.Invoke(
                                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, IReactiveQbservable<int>>), Constants.Observable.XS + 9),
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
                                            Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, IReactiveQbservable<int>>), Constants.Observable.XS + 9),
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
                                Expression.Parameter(typeof(IReactiveQbserver<int>), Constants.Observer.OB)
                            )
                        )
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_Subscribe_Parameterized_SelectMany3_CustomContext_10P()
        {
            Apply(
                provider => new MyParameterizedContext(provider),
                ctx =>
                {
                    var zs = from x1 in ctx.Xs10p("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7", "observable_parameter_8", "observable_parameter_9", "observable_parameter_10")
                             from x2 in ctx.Xs10p("observable_parameter_11", "observable_parameter_12", "observable_parameter_13", "observable_parameter_14", "observable_parameter_15", "observable_parameter_16", "observable_parameter_17", "observable_parameter_18", "observable_parameter_19", "observable_parameter_20")
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
                                    Expression.Invoke(
                                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, IReactiveQbservable<int>>), Constants.Observable.XS + 10),
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
                                            Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, IReactiveQbservable<int>>), Constants.Observable.XS + 10),
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
                                Expression.Parameter(typeof(IReactiveQbserver<int>), Constants.Observer.OB)
                            )
                        )
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_Subscribe_Parameterized_SelectMany3_CustomContext_11P()
        {
            Apply(
                provider => new MyParameterizedContext(provider),
                ctx =>
                {
                    var zs = from x1 in ctx.Xs11p("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7", "observable_parameter_8", "observable_parameter_9", "observable_parameter_10", "observable_parameter_11")
                             from x2 in ctx.Xs11p("observable_parameter_12", "observable_parameter_13", "observable_parameter_14", "observable_parameter_15", "observable_parameter_16", "observable_parameter_17", "observable_parameter_18", "observable_parameter_19", "observable_parameter_20", "observable_parameter_21", "observable_parameter_22")
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
                                    Expression.Invoke(
                                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, IReactiveQbservable<int>>), Constants.Observable.XS + 11),
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
                                            Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, IReactiveQbservable<int>>), Constants.Observable.XS + 11),
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
                                Expression.Parameter(typeof(IReactiveQbserver<int>), Constants.Observer.OB)
                            )
                        )
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_Subscribe_Parameterized_SelectMany3_CustomContext_12P()
        {
            Apply(
                provider => new MyParameterizedContext(provider),
                ctx =>
                {
                    var zs = from x1 in ctx.Xs12p("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7", "observable_parameter_8", "observable_parameter_9", "observable_parameter_10", "observable_parameter_11", "observable_parameter_12")
                             from x2 in ctx.Xs12p("observable_parameter_13", "observable_parameter_14", "observable_parameter_15", "observable_parameter_16", "observable_parameter_17", "observable_parameter_18", "observable_parameter_19", "observable_parameter_20", "observable_parameter_21", "observable_parameter_22", "observable_parameter_23", "observable_parameter_24")
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
                                    Expression.Invoke(
                                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, IReactiveQbservable<int>>), Constants.Observable.XS + 12),
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
                                            Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, IReactiveQbservable<int>>), Constants.Observable.XS + 12),
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
                                Expression.Parameter(typeof(IReactiveQbserver<int>), Constants.Observer.OB)
                            )
                        )
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_Subscribe_Parameterized_SelectMany3_CustomContext_13P()
        {
            Apply(
                provider => new MyParameterizedContext(provider),
                ctx =>
                {
                    var zs = from x1 in ctx.Xs13p("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7", "observable_parameter_8", "observable_parameter_9", "observable_parameter_10", "observable_parameter_11", "observable_parameter_12", "observable_parameter_13")
                             from x2 in ctx.Xs13p("observable_parameter_14", "observable_parameter_15", "observable_parameter_16", "observable_parameter_17", "observable_parameter_18", "observable_parameter_19", "observable_parameter_20", "observable_parameter_21", "observable_parameter_22", "observable_parameter_23", "observable_parameter_24", "observable_parameter_25", "observable_parameter_26")
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
                                    Expression.Invoke(
                                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, IReactiveQbservable<int>>), Constants.Observable.XS + 13),
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
                                            Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, IReactiveQbservable<int>>), Constants.Observable.XS + 13),
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
                                Expression.Parameter(typeof(IReactiveQbserver<int>), Constants.Observer.OB)
                            )
                        )
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_Subscribe_Parameterized_SelectMany3_CustomContext_14P()
        {
            Apply(
                provider => new MyParameterizedContext(provider),
                ctx =>
                {
                    var zs = from x1 in ctx.Xs14p("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7", "observable_parameter_8", "observable_parameter_9", "observable_parameter_10", "observable_parameter_11", "observable_parameter_12", "observable_parameter_13", "observable_parameter_14")
                             from x2 in ctx.Xs14p("observable_parameter_15", "observable_parameter_16", "observable_parameter_17", "observable_parameter_18", "observable_parameter_19", "observable_parameter_20", "observable_parameter_21", "observable_parameter_22", "observable_parameter_23", "observable_parameter_24", "observable_parameter_25", "observable_parameter_26", "observable_parameter_27", "observable_parameter_28")
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
                                    Expression.Invoke(
                                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, IReactiveQbservable<int>>), Constants.Observable.XS + 14),
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
                                            Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, IReactiveQbservable<int>>), Constants.Observable.XS + 14),
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
                                Expression.Parameter(typeof(IReactiveQbserver<int>), Constants.Observer.OB)
                            )
                        )
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_Subscribe_Parameterized_SelectMany3_CustomContext_15P()
        {
            Apply(
                provider => new MyParameterizedContext(provider),
                ctx =>
                {
                    var zs = from x1 in ctx.Xs15p("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7", "observable_parameter_8", "observable_parameter_9", "observable_parameter_10", "observable_parameter_11", "observable_parameter_12", "observable_parameter_13", "observable_parameter_14", "observable_parameter_15")
                             from x2 in ctx.Xs15p("observable_parameter_16", "observable_parameter_17", "observable_parameter_18", "observable_parameter_19", "observable_parameter_20", "observable_parameter_21", "observable_parameter_22", "observable_parameter_23", "observable_parameter_24", "observable_parameter_25", "observable_parameter_26", "observable_parameter_27", "observable_parameter_28", "observable_parameter_29", "observable_parameter_30")
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
                                    Expression.Invoke(
                                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, IReactiveQbservable<int>>), Constants.Observable.XS + 15),
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
                                            Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, IReactiveQbservable<int>>), Constants.Observable.XS + 15),
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
                                Expression.Parameter(typeof(IReactiveQbserver<int>), Constants.Observer.OB)
                            )
                        )
                    ),
                    state: null
                )
            );
        }


        #endregion

        #region "ReactiveServiceContext_Subscribe_Parameterized_Inlined"

        [TestMethod]
        public void ReactiveServiceContext_Subscribe_Parameterized_Inlined_2P()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<string, string, int>(new Uri(Constants.Observable.XS))("observable_parameter_1", "observable_parameter_2");

                    var zs = from x in xs
                             from y in ctx.GetObservable<int, int, string>(new Uri(Constants.Observable.YS))(x, x)
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
                                    Expression.Invoke(
                                        Expression.Parameter(typeof(Func<string, string, IReactiveQbservable<int>>), Constants.Observable.XS),
                                        Expression.Constant("observable_parameter_1"),
                                        Expression.Constant("observable_parameter_2")
                                    ),
                                    Expression.Lambda(
                                        Expression.Invoke(
                                            Expression.Call(
                                                Expression.Parameter(typeof(TestServiceContext), "rx://builtin/this"),
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
                                Expression.Parameter(typeof(IReactiveQbserver<int>), Constants.Observer.OB)
                            )
                        )
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_Subscribe_Parameterized_Inlined_3P()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<string, string, string, int>(new Uri(Constants.Observable.XS))("observable_parameter_1", "observable_parameter_2", "observable_parameter_3");

                    var zs = from x in xs
                             from y in ctx.GetObservable<int, int, int, string>(new Uri(Constants.Observable.YS))(x, x, x)
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
                                    Expression.Invoke(
                                        Expression.Parameter(typeof(Func<string, string, string, IReactiveQbservable<int>>), Constants.Observable.XS),
                                        Expression.Constant("observable_parameter_1"),
                                        Expression.Constant("observable_parameter_2"),
                                        Expression.Constant("observable_parameter_3")
                                    ),
                                    Expression.Lambda(
                                        Expression.Invoke(
                                            Expression.Call(
                                                Expression.Parameter(typeof(TestServiceContext), "rx://builtin/this"),
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
                                Expression.Parameter(typeof(IReactiveQbserver<int>), Constants.Observer.OB)
                            )
                        )
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_Subscribe_Parameterized_Inlined_4P()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<string, string, string, string, int>(new Uri(Constants.Observable.XS))("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4");

                    var zs = from x in xs
                             from y in ctx.GetObservable<int, int, int, int, string>(new Uri(Constants.Observable.YS))(x, x, x, x)
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
                                    Expression.Invoke(
                                        Expression.Parameter(typeof(Func<string, string, string, string, IReactiveQbservable<int>>), Constants.Observable.XS),
                                        Expression.Constant("observable_parameter_1"),
                                        Expression.Constant("observable_parameter_2"),
                                        Expression.Constant("observable_parameter_3"),
                                        Expression.Constant("observable_parameter_4")
                                    ),
                                    Expression.Lambda(
                                        Expression.Invoke(
                                            Expression.Call(
                                                Expression.Parameter(typeof(TestServiceContext), "rx://builtin/this"),
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
                                Expression.Parameter(typeof(IReactiveQbserver<int>), Constants.Observer.OB)
                            )
                        )
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_Subscribe_Parameterized_Inlined_5P()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<string, string, string, string, string, int>(new Uri(Constants.Observable.XS))("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5");

                    var zs = from x in xs
                             from y in ctx.GetObservable<int, int, int, int, int, string>(new Uri(Constants.Observable.YS))(x, x, x, x, x)
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
                                    Expression.Invoke(
                                        Expression.Parameter(typeof(Func<string, string, string, string, string, IReactiveQbservable<int>>), Constants.Observable.XS),
                                        Expression.Constant("observable_parameter_1"),
                                        Expression.Constant("observable_parameter_2"),
                                        Expression.Constant("observable_parameter_3"),
                                        Expression.Constant("observable_parameter_4"),
                                        Expression.Constant("observable_parameter_5")
                                    ),
                                    Expression.Lambda(
                                        Expression.Invoke(
                                            Expression.Call(
                                                Expression.Parameter(typeof(TestServiceContext), "rx://builtin/this"),
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
                                Expression.Parameter(typeof(IReactiveQbserver<int>), Constants.Observer.OB)
                            )
                        )
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_Subscribe_Parameterized_Inlined_6P()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<string, string, string, string, string, string, int>(new Uri(Constants.Observable.XS))("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6");

                    var zs = from x in xs
                             from y in ctx.GetObservable<int, int, int, int, int, int, string>(new Uri(Constants.Observable.YS))(x, x, x, x, x, x)
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
                                    Expression.Invoke(
                                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, IReactiveQbservable<int>>), Constants.Observable.XS),
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
                                                Expression.Parameter(typeof(TestServiceContext), "rx://builtin/this"),
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
                                Expression.Parameter(typeof(IReactiveQbserver<int>), Constants.Observer.OB)
                            )
                        )
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_Subscribe_Parameterized_Inlined_7P()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<string, string, string, string, string, string, string, int>(new Uri(Constants.Observable.XS))("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7");

                    var zs = from x in xs
                             from y in ctx.GetObservable<int, int, int, int, int, int, int, string>(new Uri(Constants.Observable.YS))(x, x, x, x, x, x, x)
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
                                    Expression.Invoke(
                                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, IReactiveQbservable<int>>), Constants.Observable.XS),
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
                                                Expression.Parameter(typeof(TestServiceContext), "rx://builtin/this"),
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
                                Expression.Parameter(typeof(IReactiveQbserver<int>), Constants.Observer.OB)
                            )
                        )
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_Subscribe_Parameterized_Inlined_8P()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observable.XS))("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7", "observable_parameter_8");

                    var zs = from x in xs
                             from y in ctx.GetObservable<int, int, int, int, int, int, int, int, string>(new Uri(Constants.Observable.YS))(x, x, x, x, x, x, x, x)
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
                                    Expression.Invoke(
                                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, IReactiveQbservable<int>>), Constants.Observable.XS),
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
                                                Expression.Parameter(typeof(TestServiceContext), "rx://builtin/this"),
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
                                Expression.Parameter(typeof(IReactiveQbserver<int>), Constants.Observer.OB)
                            )
                        )
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_Subscribe_Parameterized_Inlined_9P()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observable.XS))("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7", "observable_parameter_8", "observable_parameter_9");

                    var zs = from x in xs
                             from y in ctx.GetObservable<int, int, int, int, int, int, int, int, int, string>(new Uri(Constants.Observable.YS))(x, x, x, x, x, x, x, x, x)
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
                                    Expression.Invoke(
                                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, IReactiveQbservable<int>>), Constants.Observable.XS),
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
                                                Expression.Parameter(typeof(TestServiceContext), "rx://builtin/this"),
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
                                Expression.Parameter(typeof(IReactiveQbserver<int>), Constants.Observer.OB)
                            )
                        )
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_Subscribe_Parameterized_Inlined_10P()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observable.XS))("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7", "observable_parameter_8", "observable_parameter_9", "observable_parameter_10");

                    var zs = from x in xs
                             from y in ctx.GetObservable<int, int, int, int, int, int, int, int, int, int, string>(new Uri(Constants.Observable.YS))(x, x, x, x, x, x, x, x, x, x)
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
                                    Expression.Invoke(
                                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, IReactiveQbservable<int>>), Constants.Observable.XS),
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
                                                Expression.Parameter(typeof(TestServiceContext), "rx://builtin/this"),
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
                                Expression.Parameter(typeof(IReactiveQbserver<int>), Constants.Observer.OB)
                            )
                        )
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_Subscribe_Parameterized_Inlined_11P()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<string, string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observable.XS))("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7", "observable_parameter_8", "observable_parameter_9", "observable_parameter_10", "observable_parameter_11");

                    var zs = from x in xs
                             from y in ctx.GetObservable<int, int, int, int, int, int, int, int, int, int, int, string>(new Uri(Constants.Observable.YS))(x, x, x, x, x, x, x, x, x, x, x)
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
                                    Expression.Invoke(
                                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, IReactiveQbservable<int>>), Constants.Observable.XS),
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
                                                Expression.Parameter(typeof(TestServiceContext), "rx://builtin/this"),
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
                                Expression.Parameter(typeof(IReactiveQbserver<int>), Constants.Observer.OB)
                            )
                        )
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_Subscribe_Parameterized_Inlined_12P()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<string, string, string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observable.XS))("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7", "observable_parameter_8", "observable_parameter_9", "observable_parameter_10", "observable_parameter_11", "observable_parameter_12");

                    var zs = from x in xs
                             from y in ctx.GetObservable<int, int, int, int, int, int, int, int, int, int, int, int, string>(new Uri(Constants.Observable.YS))(x, x, x, x, x, x, x, x, x, x, x, x)
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
                                    Expression.Invoke(
                                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, IReactiveQbservable<int>>), Constants.Observable.XS),
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
                                                Expression.Parameter(typeof(TestServiceContext), "rx://builtin/this"),
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
                                Expression.Parameter(typeof(IReactiveQbserver<int>), Constants.Observer.OB)
                            )
                        )
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_Subscribe_Parameterized_Inlined_13P()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<string, string, string, string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observable.XS))("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7", "observable_parameter_8", "observable_parameter_9", "observable_parameter_10", "observable_parameter_11", "observable_parameter_12", "observable_parameter_13");

                    var zs = from x in xs
                             from y in ctx.GetObservable<int, int, int, int, int, int, int, int, int, int, int, int, int, string>(new Uri(Constants.Observable.YS))(x, x, x, x, x, x, x, x, x, x, x, x, x)
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
                                    Expression.Invoke(
                                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, IReactiveQbservable<int>>), Constants.Observable.XS),
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
                                                Expression.Parameter(typeof(TestServiceContext), "rx://builtin/this"),
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
                                Expression.Parameter(typeof(IReactiveQbserver<int>), Constants.Observer.OB)
                            )
                        )
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_Subscribe_Parameterized_Inlined_14P()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<string, string, string, string, string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observable.XS))("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7", "observable_parameter_8", "observable_parameter_9", "observable_parameter_10", "observable_parameter_11", "observable_parameter_12", "observable_parameter_13", "observable_parameter_14");

                    var zs = from x in xs
                             from y in ctx.GetObservable<int, int, int, int, int, int, int, int, int, int, int, int, int, int, string>(new Uri(Constants.Observable.YS))(x, x, x, x, x, x, x, x, x, x, x, x, x, x)
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
                                    Expression.Invoke(
                                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, IReactiveQbservable<int>>), Constants.Observable.XS),
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
                                                Expression.Parameter(typeof(TestServiceContext), "rx://builtin/this"),
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
                                Expression.Parameter(typeof(IReactiveQbserver<int>), Constants.Observer.OB)
                            )
                        )
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_Subscribe_Parameterized_Inlined_15P()
        {
            Apply(
                ctx =>
                {
                    var xs = ctx.GetObservable<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observable.XS))("observable_parameter_1", "observable_parameter_2", "observable_parameter_3", "observable_parameter_4", "observable_parameter_5", "observable_parameter_6", "observable_parameter_7", "observable_parameter_8", "observable_parameter_9", "observable_parameter_10", "observable_parameter_11", "observable_parameter_12", "observable_parameter_13", "observable_parameter_14", "observable_parameter_15");

                    var zs = from x in xs
                             from y in ctx.GetObservable<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, string>(new Uri(Constants.Observable.YS))(x, x, x, x, x, x, x, x, x, x, x, x, x, x, x)
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
                                    Expression.Invoke(
                                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, IReactiveQbservable<int>>), Constants.Observable.XS),
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
                                                Expression.Parameter(typeof(TestServiceContext), "rx://builtin/this"),
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
                                Expression.Parameter(typeof(IReactiveQbserver<int>), Constants.Observer.OB)
                            )
                        )
                    ),
                    state: null
                )
            );
        }


        #endregion

        #region "ReactiveServiceContext_GetStreamFactory"

        [TestMethod]
        public void ReactiveServiceContext_GetStreamFactory_2P()
        {
            Apply(
                ctx =>
                {
                    var factoryUri = new Uri(Constants.StreamFactory.SF);
                    var factory = ctx.GetStreamFactory<string, string, int, int>(factoryUri);
                    Assert.AreEqual(factoryUri, ((IKnownResource)factory).Uri);

                    var stream = factory.Create(new Uri(Constants.Stream.BAR), "factory_parameter_1", "factory_parameter_2", null);

                    factory.Create(new Uri(Constants.Stream.FOO), "factory_parameter_1", "factory_parameter_2", null);
                    ((IReactiveSubjectFactory<int, int, string, string>)factory).Create(new Uri(Constants.Stream.QUX), "factory_parameter_1", "factory_parameter_2", null);

                    Assert.ThrowsException<ArgumentNullException>(() => factory.Create(null, "factory_parameter_1", "factory_parameter_2", null));
                    Assert.ThrowsException<ArgumentNullException>(() => ((IReactiveSubjectFactory<int, int, string, string>)factory).Create(null, "factory_parameter_1", "factory_parameter_2", null));

                    var observer = ctx.GetObserver<int>(new Uri(Constants.Observer.OB));
                    stream.Subscribe(observer, new Uri(Constants.Subscription.SUB), null);
                },
                new CreateStream(
                    new Uri(Constants.Stream.BAR),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<string, string, IReactiveQubject<int, int>>), Constants.StreamFactory.SF),
                        Expression.Constant("factory_parameter_1"),
                        Expression.Constant("factory_parameter_2")
                    ),
                    state: null
                ),
                new CreateStream(
                    new Uri(Constants.Stream.FOO),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<string, string, IReactiveQubject<int, int>>), Constants.StreamFactory.SF),
                        Expression.Constant("factory_parameter_1"),
                        Expression.Constant("factory_parameter_2")
                    ),
                    state: null
                ),
                new CreateStream(
                    new Uri(Constants.Stream.QUX),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<string, string, IReactiveQubject<int, int>>), Constants.StreamFactory.SF),
                        Expression.Constant("factory_parameter_1"),
                        Expression.Constant("factory_parameter_2")
                    ),
                    state: null
                ),
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<IReactiveQbservable<int>, IReactiveQbserver<int>, IReactiveQubscription>), Constants.SubscribeUri),
                        Expression.Parameter(typeof(IReactiveQbservable<int>), Constants.Stream.BAR),
                        Expression.Parameter(typeof(IReactiveQbserver<int>), Constants.Observer.OB)
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_GetStreamFactory_3P()
        {
            Apply(
                ctx =>
                {
                    var factoryUri = new Uri(Constants.StreamFactory.SF);
                    var factory = ctx.GetStreamFactory<string, string, string, int, int>(factoryUri);
                    Assert.AreEqual(factoryUri, ((IKnownResource)factory).Uri);

                    var stream = factory.Create(new Uri(Constants.Stream.BAR), "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", null);

                    factory.Create(new Uri(Constants.Stream.FOO), "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", null);
                    ((IReactiveSubjectFactory<int, int, string, string, string>)factory).Create(new Uri(Constants.Stream.QUX), "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", null);

                    Assert.ThrowsException<ArgumentNullException>(() => factory.Create(null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", null));
                    Assert.ThrowsException<ArgumentNullException>(() => ((IReactiveSubjectFactory<int, int, string, string, string>)factory).Create(null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", null));

                    var observer = ctx.GetObserver<int>(new Uri(Constants.Observer.OB));
                    stream.Subscribe(observer, new Uri(Constants.Subscription.SUB), null);
                },
                new CreateStream(
                    new Uri(Constants.Stream.BAR),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<string, string, string, IReactiveQubject<int, int>>), Constants.StreamFactory.SF),
                        Expression.Constant("factory_parameter_1"),
                        Expression.Constant("factory_parameter_2"),
                        Expression.Constant("factory_parameter_3")
                    ),
                    state: null
                ),
                new CreateStream(
                    new Uri(Constants.Stream.FOO),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<string, string, string, IReactiveQubject<int, int>>), Constants.StreamFactory.SF),
                        Expression.Constant("factory_parameter_1"),
                        Expression.Constant("factory_parameter_2"),
                        Expression.Constant("factory_parameter_3")
                    ),
                    state: null
                ),
                new CreateStream(
                    new Uri(Constants.Stream.QUX),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<string, string, string, IReactiveQubject<int, int>>), Constants.StreamFactory.SF),
                        Expression.Constant("factory_parameter_1"),
                        Expression.Constant("factory_parameter_2"),
                        Expression.Constant("factory_parameter_3")
                    ),
                    state: null
                ),
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<IReactiveQbservable<int>, IReactiveQbserver<int>, IReactiveQubscription>), Constants.SubscribeUri),
                        Expression.Parameter(typeof(IReactiveQbservable<int>), Constants.Stream.BAR),
                        Expression.Parameter(typeof(IReactiveQbserver<int>), Constants.Observer.OB)
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_GetStreamFactory_4P()
        {
            Apply(
                ctx =>
                {
                    var factoryUri = new Uri(Constants.StreamFactory.SF);
                    var factory = ctx.GetStreamFactory<string, string, string, string, int, int>(factoryUri);
                    Assert.AreEqual(factoryUri, ((IKnownResource)factory).Uri);

                    var stream = factory.Create(new Uri(Constants.Stream.BAR), "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", null);

                    factory.Create(new Uri(Constants.Stream.FOO), "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", null);
                    ((IReactiveSubjectFactory<int, int, string, string, string, string>)factory).Create(new Uri(Constants.Stream.QUX), "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", null);

                    Assert.ThrowsException<ArgumentNullException>(() => factory.Create(null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", null));
                    Assert.ThrowsException<ArgumentNullException>(() => ((IReactiveSubjectFactory<int, int, string, string, string, string>)factory).Create(null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", null));

                    var observer = ctx.GetObserver<int>(new Uri(Constants.Observer.OB));
                    stream.Subscribe(observer, new Uri(Constants.Subscription.SUB), null);
                },
                new CreateStream(
                    new Uri(Constants.Stream.BAR),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<string, string, string, string, IReactiveQubject<int, int>>), Constants.StreamFactory.SF),
                        Expression.Constant("factory_parameter_1"),
                        Expression.Constant("factory_parameter_2"),
                        Expression.Constant("factory_parameter_3"),
                        Expression.Constant("factory_parameter_4")
                    ),
                    state: null
                ),
                new CreateStream(
                    new Uri(Constants.Stream.FOO),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<string, string, string, string, IReactiveQubject<int, int>>), Constants.StreamFactory.SF),
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
                        Expression.Parameter(typeof(Func<string, string, string, string, IReactiveQubject<int, int>>), Constants.StreamFactory.SF),
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
                        Expression.Parameter(typeof(Func<IReactiveQbservable<int>, IReactiveQbserver<int>, IReactiveQubscription>), Constants.SubscribeUri),
                        Expression.Parameter(typeof(IReactiveQbservable<int>), Constants.Stream.BAR),
                        Expression.Parameter(typeof(IReactiveQbserver<int>), Constants.Observer.OB)
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_GetStreamFactory_5P()
        {
            Apply(
                ctx =>
                {
                    var factoryUri = new Uri(Constants.StreamFactory.SF);
                    var factory = ctx.GetStreamFactory<string, string, string, string, string, int, int>(factoryUri);
                    Assert.AreEqual(factoryUri, ((IKnownResource)factory).Uri);

                    var stream = factory.Create(new Uri(Constants.Stream.BAR), "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", null);

                    factory.Create(new Uri(Constants.Stream.FOO), "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", null);
                    ((IReactiveSubjectFactory<int, int, string, string, string, string, string>)factory).Create(new Uri(Constants.Stream.QUX), "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", null);

                    Assert.ThrowsException<ArgumentNullException>(() => factory.Create(null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", null));
                    Assert.ThrowsException<ArgumentNullException>(() => ((IReactiveSubjectFactory<int, int, string, string, string, string, string>)factory).Create(null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", null));

                    var observer = ctx.GetObserver<int>(new Uri(Constants.Observer.OB));
                    stream.Subscribe(observer, new Uri(Constants.Subscription.SUB), null);
                },
                new CreateStream(
                    new Uri(Constants.Stream.BAR),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<string, string, string, string, string, IReactiveQubject<int, int>>), Constants.StreamFactory.SF),
                        Expression.Constant("factory_parameter_1"),
                        Expression.Constant("factory_parameter_2"),
                        Expression.Constant("factory_parameter_3"),
                        Expression.Constant("factory_parameter_4"),
                        Expression.Constant("factory_parameter_5")
                    ),
                    state: null
                ),
                new CreateStream(
                    new Uri(Constants.Stream.FOO),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<string, string, string, string, string, IReactiveQubject<int, int>>), Constants.StreamFactory.SF),
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
                        Expression.Parameter(typeof(Func<string, string, string, string, string, IReactiveQubject<int, int>>), Constants.StreamFactory.SF),
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
                        Expression.Parameter(typeof(Func<IReactiveQbservable<int>, IReactiveQbserver<int>, IReactiveQubscription>), Constants.SubscribeUri),
                        Expression.Parameter(typeof(IReactiveQbservable<int>), Constants.Stream.BAR),
                        Expression.Parameter(typeof(IReactiveQbserver<int>), Constants.Observer.OB)
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_GetStreamFactory_6P()
        {
            Apply(
                ctx =>
                {
                    var factoryUri = new Uri(Constants.StreamFactory.SF);
                    var factory = ctx.GetStreamFactory<string, string, string, string, string, string, int, int>(factoryUri);
                    Assert.AreEqual(factoryUri, ((IKnownResource)factory).Uri);

                    var stream = factory.Create(new Uri(Constants.Stream.BAR), "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", null);

                    factory.Create(new Uri(Constants.Stream.FOO), "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", null);
                    ((IReactiveSubjectFactory<int, int, string, string, string, string, string, string>)factory).Create(new Uri(Constants.Stream.QUX), "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", null);

                    Assert.ThrowsException<ArgumentNullException>(() => factory.Create(null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", null));
                    Assert.ThrowsException<ArgumentNullException>(() => ((IReactiveSubjectFactory<int, int, string, string, string, string, string, string>)factory).Create(null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", null));

                    var observer = ctx.GetObserver<int>(new Uri(Constants.Observer.OB));
                    stream.Subscribe(observer, new Uri(Constants.Subscription.SUB), null);
                },
                new CreateStream(
                    new Uri(Constants.Stream.BAR),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, IReactiveQubject<int, int>>), Constants.StreamFactory.SF),
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
                    new Uri(Constants.Stream.FOO),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, IReactiveQubject<int, int>>), Constants.StreamFactory.SF),
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
                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, IReactiveQubject<int, int>>), Constants.StreamFactory.SF),
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
                        Expression.Parameter(typeof(Func<IReactiveQbservable<int>, IReactiveQbserver<int>, IReactiveQubscription>), Constants.SubscribeUri),
                        Expression.Parameter(typeof(IReactiveQbservable<int>), Constants.Stream.BAR),
                        Expression.Parameter(typeof(IReactiveQbserver<int>), Constants.Observer.OB)
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_GetStreamFactory_7P()
        {
            Apply(
                ctx =>
                {
                    var factoryUri = new Uri(Constants.StreamFactory.SF);
                    var factory = ctx.GetStreamFactory<string, string, string, string, string, string, string, int, int>(factoryUri);
                    Assert.AreEqual(factoryUri, ((IKnownResource)factory).Uri);

                    var stream = factory.Create(new Uri(Constants.Stream.BAR), "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", null);

                    factory.Create(new Uri(Constants.Stream.FOO), "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", null);
                    ((IReactiveSubjectFactory<int, int, string, string, string, string, string, string, string>)factory).Create(new Uri(Constants.Stream.QUX), "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", null);

                    Assert.ThrowsException<ArgumentNullException>(() => factory.Create(null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", null));
                    Assert.ThrowsException<ArgumentNullException>(() => ((IReactiveSubjectFactory<int, int, string, string, string, string, string, string, string>)factory).Create(null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", null));

                    var observer = ctx.GetObserver<int>(new Uri(Constants.Observer.OB));
                    stream.Subscribe(observer, new Uri(Constants.Subscription.SUB), null);
                },
                new CreateStream(
                    new Uri(Constants.Stream.BAR),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, IReactiveQubject<int, int>>), Constants.StreamFactory.SF),
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
                    new Uri(Constants.Stream.FOO),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, IReactiveQubject<int, int>>), Constants.StreamFactory.SF),
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
                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, IReactiveQubject<int, int>>), Constants.StreamFactory.SF),
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
                        Expression.Parameter(typeof(Func<IReactiveQbservable<int>, IReactiveQbserver<int>, IReactiveQubscription>), Constants.SubscribeUri),
                        Expression.Parameter(typeof(IReactiveQbservable<int>), Constants.Stream.BAR),
                        Expression.Parameter(typeof(IReactiveQbserver<int>), Constants.Observer.OB)
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_GetStreamFactory_8P()
        {
            Apply(
                ctx =>
                {
                    var factoryUri = new Uri(Constants.StreamFactory.SF);
                    var factory = ctx.GetStreamFactory<string, string, string, string, string, string, string, string, int, int>(factoryUri);
                    Assert.AreEqual(factoryUri, ((IKnownResource)factory).Uri);

                    var stream = factory.Create(new Uri(Constants.Stream.BAR), "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", null);

                    factory.Create(new Uri(Constants.Stream.FOO), "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", null);
                    ((IReactiveSubjectFactory<int, int, string, string, string, string, string, string, string, string>)factory).Create(new Uri(Constants.Stream.QUX), "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", null);

                    Assert.ThrowsException<ArgumentNullException>(() => factory.Create(null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", null));
                    Assert.ThrowsException<ArgumentNullException>(() => ((IReactiveSubjectFactory<int, int, string, string, string, string, string, string, string, string>)factory).Create(null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", null));

                    var observer = ctx.GetObserver<int>(new Uri(Constants.Observer.OB));
                    stream.Subscribe(observer, new Uri(Constants.Subscription.SUB), null);
                },
                new CreateStream(
                    new Uri(Constants.Stream.BAR),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, IReactiveQubject<int, int>>), Constants.StreamFactory.SF),
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
                    new Uri(Constants.Stream.FOO),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, IReactiveQubject<int, int>>), Constants.StreamFactory.SF),
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
                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, IReactiveQubject<int, int>>), Constants.StreamFactory.SF),
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
                        Expression.Parameter(typeof(Func<IReactiveQbservable<int>, IReactiveQbserver<int>, IReactiveQubscription>), Constants.SubscribeUri),
                        Expression.Parameter(typeof(IReactiveQbservable<int>), Constants.Stream.BAR),
                        Expression.Parameter(typeof(IReactiveQbserver<int>), Constants.Observer.OB)
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_GetStreamFactory_9P()
        {
            Apply(
                ctx =>
                {
                    var factoryUri = new Uri(Constants.StreamFactory.SF);
                    var factory = ctx.GetStreamFactory<string, string, string, string, string, string, string, string, string, int, int>(factoryUri);
                    Assert.AreEqual(factoryUri, ((IKnownResource)factory).Uri);

                    var stream = factory.Create(new Uri(Constants.Stream.BAR), "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", null);

                    factory.Create(new Uri(Constants.Stream.FOO), "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", null);
                    ((IReactiveSubjectFactory<int, int, string, string, string, string, string, string, string, string, string>)factory).Create(new Uri(Constants.Stream.QUX), "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", null);

                    Assert.ThrowsException<ArgumentNullException>(() => factory.Create(null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", null));
                    Assert.ThrowsException<ArgumentNullException>(() => ((IReactiveSubjectFactory<int, int, string, string, string, string, string, string, string, string, string>)factory).Create(null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", null));

                    var observer = ctx.GetObserver<int>(new Uri(Constants.Observer.OB));
                    stream.Subscribe(observer, new Uri(Constants.Subscription.SUB), null);
                },
                new CreateStream(
                    new Uri(Constants.Stream.BAR),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, IReactiveQubject<int, int>>), Constants.StreamFactory.SF),
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
                    new Uri(Constants.Stream.FOO),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, IReactiveQubject<int, int>>), Constants.StreamFactory.SF),
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
                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, IReactiveQubject<int, int>>), Constants.StreamFactory.SF),
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
                        Expression.Parameter(typeof(Func<IReactiveQbservable<int>, IReactiveQbserver<int>, IReactiveQubscription>), Constants.SubscribeUri),
                        Expression.Parameter(typeof(IReactiveQbservable<int>), Constants.Stream.BAR),
                        Expression.Parameter(typeof(IReactiveQbserver<int>), Constants.Observer.OB)
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_GetStreamFactory_10P()
        {
            Apply(
                ctx =>
                {
                    var factoryUri = new Uri(Constants.StreamFactory.SF);
                    var factory = ctx.GetStreamFactory<string, string, string, string, string, string, string, string, string, string, int, int>(factoryUri);
                    Assert.AreEqual(factoryUri, ((IKnownResource)factory).Uri);

                    var stream = factory.Create(new Uri(Constants.Stream.BAR), "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", null);

                    factory.Create(new Uri(Constants.Stream.FOO), "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", null);
                    ((IReactiveSubjectFactory<int, int, string, string, string, string, string, string, string, string, string, string>)factory).Create(new Uri(Constants.Stream.QUX), "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", null);

                    Assert.ThrowsException<ArgumentNullException>(() => factory.Create(null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", null));
                    Assert.ThrowsException<ArgumentNullException>(() => ((IReactiveSubjectFactory<int, int, string, string, string, string, string, string, string, string, string, string>)factory).Create(null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", null));

                    var observer = ctx.GetObserver<int>(new Uri(Constants.Observer.OB));
                    stream.Subscribe(observer, new Uri(Constants.Subscription.SUB), null);
                },
                new CreateStream(
                    new Uri(Constants.Stream.BAR),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, IReactiveQubject<int, int>>), Constants.StreamFactory.SF),
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
                    new Uri(Constants.Stream.FOO),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, IReactiveQubject<int, int>>), Constants.StreamFactory.SF),
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
                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, IReactiveQubject<int, int>>), Constants.StreamFactory.SF),
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
                        Expression.Parameter(typeof(Func<IReactiveQbservable<int>, IReactiveQbserver<int>, IReactiveQubscription>), Constants.SubscribeUri),
                        Expression.Parameter(typeof(IReactiveQbservable<int>), Constants.Stream.BAR),
                        Expression.Parameter(typeof(IReactiveQbserver<int>), Constants.Observer.OB)
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_GetStreamFactory_11P()
        {
            Apply(
                ctx =>
                {
                    var factoryUri = new Uri(Constants.StreamFactory.SF);
                    var factory = ctx.GetStreamFactory<string, string, string, string, string, string, string, string, string, string, string, int, int>(factoryUri);
                    Assert.AreEqual(factoryUri, ((IKnownResource)factory).Uri);

                    var stream = factory.Create(new Uri(Constants.Stream.BAR), "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", "factory_parameter_11", null);

                    factory.Create(new Uri(Constants.Stream.FOO), "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", "factory_parameter_11", null);
                    ((IReactiveSubjectFactory<int, int, string, string, string, string, string, string, string, string, string, string, string>)factory).Create(new Uri(Constants.Stream.QUX), "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", "factory_parameter_11", null);

                    Assert.ThrowsException<ArgumentNullException>(() => factory.Create(null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", "factory_parameter_11", null));
                    Assert.ThrowsException<ArgumentNullException>(() => ((IReactiveSubjectFactory<int, int, string, string, string, string, string, string, string, string, string, string, string>)factory).Create(null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", "factory_parameter_11", null));

                    var observer = ctx.GetObserver<int>(new Uri(Constants.Observer.OB));
                    stream.Subscribe(observer, new Uri(Constants.Subscription.SUB), null);
                },
                new CreateStream(
                    new Uri(Constants.Stream.BAR),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, IReactiveQubject<int, int>>), Constants.StreamFactory.SF),
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
                    new Uri(Constants.Stream.FOO),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, IReactiveQubject<int, int>>), Constants.StreamFactory.SF),
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
                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, IReactiveQubject<int, int>>), Constants.StreamFactory.SF),
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
                        Expression.Parameter(typeof(Func<IReactiveQbservable<int>, IReactiveQbserver<int>, IReactiveQubscription>), Constants.SubscribeUri),
                        Expression.Parameter(typeof(IReactiveQbservable<int>), Constants.Stream.BAR),
                        Expression.Parameter(typeof(IReactiveQbserver<int>), Constants.Observer.OB)
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_GetStreamFactory_12P()
        {
            Apply(
                ctx =>
                {
                    var factoryUri = new Uri(Constants.StreamFactory.SF);
                    var factory = ctx.GetStreamFactory<string, string, string, string, string, string, string, string, string, string, string, string, int, int>(factoryUri);
                    Assert.AreEqual(factoryUri, ((IKnownResource)factory).Uri);

                    var stream = factory.Create(new Uri(Constants.Stream.BAR), "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", "factory_parameter_11", "factory_parameter_12", null);

                    factory.Create(new Uri(Constants.Stream.FOO), "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", "factory_parameter_11", "factory_parameter_12", null);
                    ((IReactiveSubjectFactory<int, int, string, string, string, string, string, string, string, string, string, string, string, string>)factory).Create(new Uri(Constants.Stream.QUX), "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", "factory_parameter_11", "factory_parameter_12", null);

                    Assert.ThrowsException<ArgumentNullException>(() => factory.Create(null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", "factory_parameter_11", "factory_parameter_12", null));
                    Assert.ThrowsException<ArgumentNullException>(() => ((IReactiveSubjectFactory<int, int, string, string, string, string, string, string, string, string, string, string, string, string>)factory).Create(null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", "factory_parameter_11", "factory_parameter_12", null));

                    var observer = ctx.GetObserver<int>(new Uri(Constants.Observer.OB));
                    stream.Subscribe(observer, new Uri(Constants.Subscription.SUB), null);
                },
                new CreateStream(
                    new Uri(Constants.Stream.BAR),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, IReactiveQubject<int, int>>), Constants.StreamFactory.SF),
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
                    new Uri(Constants.Stream.FOO),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, IReactiveQubject<int, int>>), Constants.StreamFactory.SF),
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
                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, IReactiveQubject<int, int>>), Constants.StreamFactory.SF),
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
                        Expression.Parameter(typeof(Func<IReactiveQbservable<int>, IReactiveQbserver<int>, IReactiveQubscription>), Constants.SubscribeUri),
                        Expression.Parameter(typeof(IReactiveQbservable<int>), Constants.Stream.BAR),
                        Expression.Parameter(typeof(IReactiveQbserver<int>), Constants.Observer.OB)
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_GetStreamFactory_13P()
        {
            Apply(
                ctx =>
                {
                    var factoryUri = new Uri(Constants.StreamFactory.SF);
                    var factory = ctx.GetStreamFactory<string, string, string, string, string, string, string, string, string, string, string, string, string, int, int>(factoryUri);
                    Assert.AreEqual(factoryUri, ((IKnownResource)factory).Uri);

                    var stream = factory.Create(new Uri(Constants.Stream.BAR), "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", "factory_parameter_11", "factory_parameter_12", "factory_parameter_13", null);

                    factory.Create(new Uri(Constants.Stream.FOO), "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", "factory_parameter_11", "factory_parameter_12", "factory_parameter_13", null);
                    ((IReactiveSubjectFactory<int, int, string, string, string, string, string, string, string, string, string, string, string, string, string>)factory).Create(new Uri(Constants.Stream.QUX), "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", "factory_parameter_11", "factory_parameter_12", "factory_parameter_13", null);

                    Assert.ThrowsException<ArgumentNullException>(() => factory.Create(null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", "factory_parameter_11", "factory_parameter_12", "factory_parameter_13", null));
                    Assert.ThrowsException<ArgumentNullException>(() => ((IReactiveSubjectFactory<int, int, string, string, string, string, string, string, string, string, string, string, string, string, string>)factory).Create(null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", "factory_parameter_11", "factory_parameter_12", "factory_parameter_13", null));

                    var observer = ctx.GetObserver<int>(new Uri(Constants.Observer.OB));
                    stream.Subscribe(observer, new Uri(Constants.Subscription.SUB), null);
                },
                new CreateStream(
                    new Uri(Constants.Stream.BAR),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, IReactiveQubject<int, int>>), Constants.StreamFactory.SF),
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
                    new Uri(Constants.Stream.FOO),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, IReactiveQubject<int, int>>), Constants.StreamFactory.SF),
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
                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, IReactiveQubject<int, int>>), Constants.StreamFactory.SF),
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
                        Expression.Parameter(typeof(Func<IReactiveQbservable<int>, IReactiveQbserver<int>, IReactiveQubscription>), Constants.SubscribeUri),
                        Expression.Parameter(typeof(IReactiveQbservable<int>), Constants.Stream.BAR),
                        Expression.Parameter(typeof(IReactiveQbserver<int>), Constants.Observer.OB)
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_GetStreamFactory_14P()
        {
            Apply(
                ctx =>
                {
                    var factoryUri = new Uri(Constants.StreamFactory.SF);
                    var factory = ctx.GetStreamFactory<string, string, string, string, string, string, string, string, string, string, string, string, string, string, int, int>(factoryUri);
                    Assert.AreEqual(factoryUri, ((IKnownResource)factory).Uri);

                    var stream = factory.Create(new Uri(Constants.Stream.BAR), "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", "factory_parameter_11", "factory_parameter_12", "factory_parameter_13", "factory_parameter_14", null);

                    factory.Create(new Uri(Constants.Stream.FOO), "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", "factory_parameter_11", "factory_parameter_12", "factory_parameter_13", "factory_parameter_14", null);
                    ((IReactiveSubjectFactory<int, int, string, string, string, string, string, string, string, string, string, string, string, string, string, string>)factory).Create(new Uri(Constants.Stream.QUX), "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", "factory_parameter_11", "factory_parameter_12", "factory_parameter_13", "factory_parameter_14", null);

                    Assert.ThrowsException<ArgumentNullException>(() => factory.Create(null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", "factory_parameter_11", "factory_parameter_12", "factory_parameter_13", "factory_parameter_14", null));
                    Assert.ThrowsException<ArgumentNullException>(() => ((IReactiveSubjectFactory<int, int, string, string, string, string, string, string, string, string, string, string, string, string, string, string>)factory).Create(null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", "factory_parameter_11", "factory_parameter_12", "factory_parameter_13", "factory_parameter_14", null));

                    var observer = ctx.GetObserver<int>(new Uri(Constants.Observer.OB));
                    stream.Subscribe(observer, new Uri(Constants.Subscription.SUB), null);
                },
                new CreateStream(
                    new Uri(Constants.Stream.BAR),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, IReactiveQubject<int, int>>), Constants.StreamFactory.SF),
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
                    new Uri(Constants.Stream.FOO),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, IReactiveQubject<int, int>>), Constants.StreamFactory.SF),
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
                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, IReactiveQubject<int, int>>), Constants.StreamFactory.SF),
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
                        Expression.Parameter(typeof(Func<IReactiveQbservable<int>, IReactiveQbserver<int>, IReactiveQubscription>), Constants.SubscribeUri),
                        Expression.Parameter(typeof(IReactiveQbservable<int>), Constants.Stream.BAR),
                        Expression.Parameter(typeof(IReactiveQbserver<int>), Constants.Observer.OB)
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_GetStreamFactory_15P()
        {
            Apply(
                ctx =>
                {
                    var factoryUri = new Uri(Constants.StreamFactory.SF);
                    var factory = ctx.GetStreamFactory<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, int, int>(factoryUri);
                    Assert.AreEqual(factoryUri, ((IKnownResource)factory).Uri);

                    var stream = factory.Create(new Uri(Constants.Stream.BAR), "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", "factory_parameter_11", "factory_parameter_12", "factory_parameter_13", "factory_parameter_14", "factory_parameter_15", null);

                    factory.Create(new Uri(Constants.Stream.FOO), "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", "factory_parameter_11", "factory_parameter_12", "factory_parameter_13", "factory_parameter_14", "factory_parameter_15", null);
                    ((IReactiveSubjectFactory<int, int, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string>)factory).Create(new Uri(Constants.Stream.QUX), "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", "factory_parameter_11", "factory_parameter_12", "factory_parameter_13", "factory_parameter_14", "factory_parameter_15", null);

                    Assert.ThrowsException<ArgumentNullException>(() => factory.Create(null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", "factory_parameter_11", "factory_parameter_12", "factory_parameter_13", "factory_parameter_14", "factory_parameter_15", null));
                    Assert.ThrowsException<ArgumentNullException>(() => ((IReactiveSubjectFactory<int, int, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string>)factory).Create(null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", "factory_parameter_11", "factory_parameter_12", "factory_parameter_13", "factory_parameter_14", "factory_parameter_15", null));

                    var observer = ctx.GetObserver<int>(new Uri(Constants.Observer.OB));
                    stream.Subscribe(observer, new Uri(Constants.Subscription.SUB), null);
                },
                new CreateStream(
                    new Uri(Constants.Stream.BAR),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, IReactiveQubject<int, int>>), Constants.StreamFactory.SF),
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
                    new Uri(Constants.Stream.FOO),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, IReactiveQubject<int, int>>), Constants.StreamFactory.SF),
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
                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, IReactiveQubject<int, int>>), Constants.StreamFactory.SF),
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
                        Expression.Parameter(typeof(Func<IReactiveQbservable<int>, IReactiveQbserver<int>, IReactiveQubscription>), Constants.SubscribeUri),
                        Expression.Parameter(typeof(IReactiveQbservable<int>), Constants.Stream.BAR),
                        Expression.Parameter(typeof(IReactiveQbserver<int>), Constants.Observer.OB)
                    ),
                    state: null
                )
            );
        }

        #endregion

        #region "ReactiveServiceContext_GetSubscriptionFactory"

        [TestMethod]
        public void ReactiveServiceContext_GetSubscriptionFactory_2P()
        {
            Apply(
                ctx =>
                {
                    var factoryUri = new Uri(Constants.SubscriptionFactory.SF);
                    var factory = ctx.GetSubscriptionFactory<string, string>(factoryUri);
                    Assert.AreEqual(factoryUri, ((IKnownResource)factory).Uri);

                    var sub1 = factory.Create(new Uri(Constants.Subscription.SUB1), "factory_parameter_1", "factory_parameter_2", null);
                    var sub2 = ((IReactiveSubscriptionFactory<string, string>)factory).Create(new Uri(Constants.Subscription.SUB2), "factory_parameter_1", "factory_parameter_2", null);

                    Assert.ThrowsException<ArgumentNullException>(() => factory.Create(null, "factory_parameter_1", "factory_parameter_2", null));
                    Assert.ThrowsException<ArgumentNullException>(() => ((IReactiveSubscriptionFactory<string, string>)factory).Create(null, "factory_parameter_1", "factory_parameter_2", null));

                    sub1.Dispose();
                    sub2.Dispose();
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB1),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<string, string, IReactiveQubscription>), Constants.SubscriptionFactory.SF),
                        Expression.Constant("factory_parameter_1"),
                        Expression.Constant("factory_parameter_2")
                    ),
                    state: null
                ),
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB2),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<string, string, IReactiveQubscription>), Constants.SubscriptionFactory.SF),
                        Expression.Constant("factory_parameter_1"),
                        Expression.Constant("factory_parameter_2")
                    ),
                    state: null
                ),
                new DeleteSubscription(
                    new Uri(Constants.Subscription.SUB1)
                ),
                new DeleteSubscription(
                    new Uri(Constants.Subscription.SUB2)
                )
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_GetSubscriptionFactory_3P()
        {
            Apply(
                ctx =>
                {
                    var factoryUri = new Uri(Constants.SubscriptionFactory.SF);
                    var factory = ctx.GetSubscriptionFactory<string, string, string>(factoryUri);
                    Assert.AreEqual(factoryUri, ((IKnownResource)factory).Uri);

                    var sub1 = factory.Create(new Uri(Constants.Subscription.SUB1), "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", null);
                    var sub2 = ((IReactiveSubscriptionFactory<string, string, string>)factory).Create(new Uri(Constants.Subscription.SUB2), "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", null);

                    Assert.ThrowsException<ArgumentNullException>(() => factory.Create(null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", null));
                    Assert.ThrowsException<ArgumentNullException>(() => ((IReactiveSubscriptionFactory<string, string, string>)factory).Create(null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", null));

                    sub1.Dispose();
                    sub2.Dispose();
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB1),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<string, string, string, IReactiveQubscription>), Constants.SubscriptionFactory.SF),
                        Expression.Constant("factory_parameter_1"),
                        Expression.Constant("factory_parameter_2"),
                        Expression.Constant("factory_parameter_3")
                    ),
                    state: null
                ),
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB2),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<string, string, string, IReactiveQubscription>), Constants.SubscriptionFactory.SF),
                        Expression.Constant("factory_parameter_1"),
                        Expression.Constant("factory_parameter_2"),
                        Expression.Constant("factory_parameter_3")
                    ),
                    state: null
                ),
                new DeleteSubscription(
                    new Uri(Constants.Subscription.SUB1)
                ),
                new DeleteSubscription(
                    new Uri(Constants.Subscription.SUB2)
                )
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_GetSubscriptionFactory_4P()
        {
            Apply(
                ctx =>
                {
                    var factoryUri = new Uri(Constants.SubscriptionFactory.SF);
                    var factory = ctx.GetSubscriptionFactory<string, string, string, string>(factoryUri);
                    Assert.AreEqual(factoryUri, ((IKnownResource)factory).Uri);

                    var sub1 = factory.Create(new Uri(Constants.Subscription.SUB1), "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", null);
                    var sub2 = ((IReactiveSubscriptionFactory<string, string, string, string>)factory).Create(new Uri(Constants.Subscription.SUB2), "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", null);

                    Assert.ThrowsException<ArgumentNullException>(() => factory.Create(null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", null));
                    Assert.ThrowsException<ArgumentNullException>(() => ((IReactiveSubscriptionFactory<string, string, string, string>)factory).Create(null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", null));

                    sub1.Dispose();
                    sub2.Dispose();
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB1),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<string, string, string, string, IReactiveQubscription>), Constants.SubscriptionFactory.SF),
                        Expression.Constant("factory_parameter_1"),
                        Expression.Constant("factory_parameter_2"),
                        Expression.Constant("factory_parameter_3"),
                        Expression.Constant("factory_parameter_4")
                    ),
                    state: null
                ),
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB2),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<string, string, string, string, IReactiveQubscription>), Constants.SubscriptionFactory.SF),
                        Expression.Constant("factory_parameter_1"),
                        Expression.Constant("factory_parameter_2"),
                        Expression.Constant("factory_parameter_3"),
                        Expression.Constant("factory_parameter_4")
                    ),
                    state: null
                ),
                new DeleteSubscription(
                    new Uri(Constants.Subscription.SUB1)
                ),
                new DeleteSubscription(
                    new Uri(Constants.Subscription.SUB2)
                )
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_GetSubscriptionFactory_5P()
        {
            Apply(
                ctx =>
                {
                    var factoryUri = new Uri(Constants.SubscriptionFactory.SF);
                    var factory = ctx.GetSubscriptionFactory<string, string, string, string, string>(factoryUri);
                    Assert.AreEqual(factoryUri, ((IKnownResource)factory).Uri);

                    var sub1 = factory.Create(new Uri(Constants.Subscription.SUB1), "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", null);
                    var sub2 = ((IReactiveSubscriptionFactory<string, string, string, string, string>)factory).Create(new Uri(Constants.Subscription.SUB2), "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", null);

                    Assert.ThrowsException<ArgumentNullException>(() => factory.Create(null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", null));
                    Assert.ThrowsException<ArgumentNullException>(() => ((IReactiveSubscriptionFactory<string, string, string, string, string>)factory).Create(null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", null));

                    sub1.Dispose();
                    sub2.Dispose();
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB1),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<string, string, string, string, string, IReactiveQubscription>), Constants.SubscriptionFactory.SF),
                        Expression.Constant("factory_parameter_1"),
                        Expression.Constant("factory_parameter_2"),
                        Expression.Constant("factory_parameter_3"),
                        Expression.Constant("factory_parameter_4"),
                        Expression.Constant("factory_parameter_5")
                    ),
                    state: null
                ),
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB2),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<string, string, string, string, string, IReactiveQubscription>), Constants.SubscriptionFactory.SF),
                        Expression.Constant("factory_parameter_1"),
                        Expression.Constant("factory_parameter_2"),
                        Expression.Constant("factory_parameter_3"),
                        Expression.Constant("factory_parameter_4"),
                        Expression.Constant("factory_parameter_5")
                    ),
                    state: null
                ),
                new DeleteSubscription(
                    new Uri(Constants.Subscription.SUB1)
                ),
                new DeleteSubscription(
                    new Uri(Constants.Subscription.SUB2)
                )
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_GetSubscriptionFactory_6P()
        {
            Apply(
                ctx =>
                {
                    var factoryUri = new Uri(Constants.SubscriptionFactory.SF);
                    var factory = ctx.GetSubscriptionFactory<string, string, string, string, string, string>(factoryUri);
                    Assert.AreEqual(factoryUri, ((IKnownResource)factory).Uri);

                    var sub1 = factory.Create(new Uri(Constants.Subscription.SUB1), "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", null);
                    var sub2 = ((IReactiveSubscriptionFactory<string, string, string, string, string, string>)factory).Create(new Uri(Constants.Subscription.SUB2), "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", null);

                    Assert.ThrowsException<ArgumentNullException>(() => factory.Create(null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", null));
                    Assert.ThrowsException<ArgumentNullException>(() => ((IReactiveSubscriptionFactory<string, string, string, string, string, string>)factory).Create(null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", null));

                    sub1.Dispose();
                    sub2.Dispose();
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB1),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, IReactiveQubscription>), Constants.SubscriptionFactory.SF),
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
                    new Uri(Constants.Subscription.SUB2),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, IReactiveQubscription>), Constants.SubscriptionFactory.SF),
                        Expression.Constant("factory_parameter_1"),
                        Expression.Constant("factory_parameter_2"),
                        Expression.Constant("factory_parameter_3"),
                        Expression.Constant("factory_parameter_4"),
                        Expression.Constant("factory_parameter_5"),
                        Expression.Constant("factory_parameter_6")
                    ),
                    state: null
                ),
                new DeleteSubscription(
                    new Uri(Constants.Subscription.SUB1)
                ),
                new DeleteSubscription(
                    new Uri(Constants.Subscription.SUB2)
                )
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_GetSubscriptionFactory_7P()
        {
            Apply(
                ctx =>
                {
                    var factoryUri = new Uri(Constants.SubscriptionFactory.SF);
                    var factory = ctx.GetSubscriptionFactory<string, string, string, string, string, string, string>(factoryUri);
                    Assert.AreEqual(factoryUri, ((IKnownResource)factory).Uri);

                    var sub1 = factory.Create(new Uri(Constants.Subscription.SUB1), "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", null);
                    var sub2 = ((IReactiveSubscriptionFactory<string, string, string, string, string, string, string>)factory).Create(new Uri(Constants.Subscription.SUB2), "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", null);

                    Assert.ThrowsException<ArgumentNullException>(() => factory.Create(null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", null));
                    Assert.ThrowsException<ArgumentNullException>(() => ((IReactiveSubscriptionFactory<string, string, string, string, string, string, string>)factory).Create(null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", null));

                    sub1.Dispose();
                    sub2.Dispose();
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB1),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, IReactiveQubscription>), Constants.SubscriptionFactory.SF),
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
                    new Uri(Constants.Subscription.SUB2),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, IReactiveQubscription>), Constants.SubscriptionFactory.SF),
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
                new DeleteSubscription(
                    new Uri(Constants.Subscription.SUB1)
                ),
                new DeleteSubscription(
                    new Uri(Constants.Subscription.SUB2)
                )
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_GetSubscriptionFactory_8P()
        {
            Apply(
                ctx =>
                {
                    var factoryUri = new Uri(Constants.SubscriptionFactory.SF);
                    var factory = ctx.GetSubscriptionFactory<string, string, string, string, string, string, string, string>(factoryUri);
                    Assert.AreEqual(factoryUri, ((IKnownResource)factory).Uri);

                    var sub1 = factory.Create(new Uri(Constants.Subscription.SUB1), "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", null);
                    var sub2 = ((IReactiveSubscriptionFactory<string, string, string, string, string, string, string, string>)factory).Create(new Uri(Constants.Subscription.SUB2), "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", null);

                    Assert.ThrowsException<ArgumentNullException>(() => factory.Create(null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", null));
                    Assert.ThrowsException<ArgumentNullException>(() => ((IReactiveSubscriptionFactory<string, string, string, string, string, string, string, string>)factory).Create(null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", null));

                    sub1.Dispose();
                    sub2.Dispose();
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB1),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, IReactiveQubscription>), Constants.SubscriptionFactory.SF),
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
                    new Uri(Constants.Subscription.SUB2),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, IReactiveQubscription>), Constants.SubscriptionFactory.SF),
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
                new DeleteSubscription(
                    new Uri(Constants.Subscription.SUB1)
                ),
                new DeleteSubscription(
                    new Uri(Constants.Subscription.SUB2)
                )
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_GetSubscriptionFactory_9P()
        {
            Apply(
                ctx =>
                {
                    var factoryUri = new Uri(Constants.SubscriptionFactory.SF);
                    var factory = ctx.GetSubscriptionFactory<string, string, string, string, string, string, string, string, string>(factoryUri);
                    Assert.AreEqual(factoryUri, ((IKnownResource)factory).Uri);

                    var sub1 = factory.Create(new Uri(Constants.Subscription.SUB1), "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", null);
                    var sub2 = ((IReactiveSubscriptionFactory<string, string, string, string, string, string, string, string, string>)factory).Create(new Uri(Constants.Subscription.SUB2), "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", null);

                    Assert.ThrowsException<ArgumentNullException>(() => factory.Create(null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", null));
                    Assert.ThrowsException<ArgumentNullException>(() => ((IReactiveSubscriptionFactory<string, string, string, string, string, string, string, string, string>)factory).Create(null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", null));

                    sub1.Dispose();
                    sub2.Dispose();
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB1),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, IReactiveQubscription>), Constants.SubscriptionFactory.SF),
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
                    new Uri(Constants.Subscription.SUB2),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, IReactiveQubscription>), Constants.SubscriptionFactory.SF),
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
                new DeleteSubscription(
                    new Uri(Constants.Subscription.SUB1)
                ),
                new DeleteSubscription(
                    new Uri(Constants.Subscription.SUB2)
                )
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_GetSubscriptionFactory_10P()
        {
            Apply(
                ctx =>
                {
                    var factoryUri = new Uri(Constants.SubscriptionFactory.SF);
                    var factory = ctx.GetSubscriptionFactory<string, string, string, string, string, string, string, string, string, string>(factoryUri);
                    Assert.AreEqual(factoryUri, ((IKnownResource)factory).Uri);

                    var sub1 = factory.Create(new Uri(Constants.Subscription.SUB1), "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", null);
                    var sub2 = ((IReactiveSubscriptionFactory<string, string, string, string, string, string, string, string, string, string>)factory).Create(new Uri(Constants.Subscription.SUB2), "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", null);

                    Assert.ThrowsException<ArgumentNullException>(() => factory.Create(null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", null));
                    Assert.ThrowsException<ArgumentNullException>(() => ((IReactiveSubscriptionFactory<string, string, string, string, string, string, string, string, string, string>)factory).Create(null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", null));

                    sub1.Dispose();
                    sub2.Dispose();
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB1),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, IReactiveQubscription>), Constants.SubscriptionFactory.SF),
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
                    new Uri(Constants.Subscription.SUB2),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, IReactiveQubscription>), Constants.SubscriptionFactory.SF),
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
                new DeleteSubscription(
                    new Uri(Constants.Subscription.SUB1)
                ),
                new DeleteSubscription(
                    new Uri(Constants.Subscription.SUB2)
                )
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_GetSubscriptionFactory_11P()
        {
            Apply(
                ctx =>
                {
                    var factoryUri = new Uri(Constants.SubscriptionFactory.SF);
                    var factory = ctx.GetSubscriptionFactory<string, string, string, string, string, string, string, string, string, string, string>(factoryUri);
                    Assert.AreEqual(factoryUri, ((IKnownResource)factory).Uri);

                    var sub1 = factory.Create(new Uri(Constants.Subscription.SUB1), "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", "factory_parameter_11", null);
                    var sub2 = ((IReactiveSubscriptionFactory<string, string, string, string, string, string, string, string, string, string, string>)factory).Create(new Uri(Constants.Subscription.SUB2), "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", "factory_parameter_11", null);

                    Assert.ThrowsException<ArgumentNullException>(() => factory.Create(null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", "factory_parameter_11", null));
                    Assert.ThrowsException<ArgumentNullException>(() => ((IReactiveSubscriptionFactory<string, string, string, string, string, string, string, string, string, string, string>)factory).Create(null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", "factory_parameter_11", null));

                    sub1.Dispose();
                    sub2.Dispose();
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB1),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, IReactiveQubscription>), Constants.SubscriptionFactory.SF),
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
                    new Uri(Constants.Subscription.SUB2),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, IReactiveQubscription>), Constants.SubscriptionFactory.SF),
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
                new DeleteSubscription(
                    new Uri(Constants.Subscription.SUB1)
                ),
                new DeleteSubscription(
                    new Uri(Constants.Subscription.SUB2)
                )
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_GetSubscriptionFactory_12P()
        {
            Apply(
                ctx =>
                {
                    var factoryUri = new Uri(Constants.SubscriptionFactory.SF);
                    var factory = ctx.GetSubscriptionFactory<string, string, string, string, string, string, string, string, string, string, string, string>(factoryUri);
                    Assert.AreEqual(factoryUri, ((IKnownResource)factory).Uri);

                    var sub1 = factory.Create(new Uri(Constants.Subscription.SUB1), "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", "factory_parameter_11", "factory_parameter_12", null);
                    var sub2 = ((IReactiveSubscriptionFactory<string, string, string, string, string, string, string, string, string, string, string, string>)factory).Create(new Uri(Constants.Subscription.SUB2), "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", "factory_parameter_11", "factory_parameter_12", null);

                    Assert.ThrowsException<ArgumentNullException>(() => factory.Create(null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", "factory_parameter_11", "factory_parameter_12", null));
                    Assert.ThrowsException<ArgumentNullException>(() => ((IReactiveSubscriptionFactory<string, string, string, string, string, string, string, string, string, string, string, string>)factory).Create(null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", "factory_parameter_11", "factory_parameter_12", null));

                    sub1.Dispose();
                    sub2.Dispose();
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB1),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, IReactiveQubscription>), Constants.SubscriptionFactory.SF),
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
                    new Uri(Constants.Subscription.SUB2),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, IReactiveQubscription>), Constants.SubscriptionFactory.SF),
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
                new DeleteSubscription(
                    new Uri(Constants.Subscription.SUB1)
                ),
                new DeleteSubscription(
                    new Uri(Constants.Subscription.SUB2)
                )
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_GetSubscriptionFactory_13P()
        {
            Apply(
                ctx =>
                {
                    var factoryUri = new Uri(Constants.SubscriptionFactory.SF);
                    var factory = ctx.GetSubscriptionFactory<string, string, string, string, string, string, string, string, string, string, string, string, string>(factoryUri);
                    Assert.AreEqual(factoryUri, ((IKnownResource)factory).Uri);

                    var sub1 = factory.Create(new Uri(Constants.Subscription.SUB1), "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", "factory_parameter_11", "factory_parameter_12", "factory_parameter_13", null);
                    var sub2 = ((IReactiveSubscriptionFactory<string, string, string, string, string, string, string, string, string, string, string, string, string>)factory).Create(new Uri(Constants.Subscription.SUB2), "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", "factory_parameter_11", "factory_parameter_12", "factory_parameter_13", null);

                    Assert.ThrowsException<ArgumentNullException>(() => factory.Create(null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", "factory_parameter_11", "factory_parameter_12", "factory_parameter_13", null));
                    Assert.ThrowsException<ArgumentNullException>(() => ((IReactiveSubscriptionFactory<string, string, string, string, string, string, string, string, string, string, string, string, string>)factory).Create(null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", "factory_parameter_11", "factory_parameter_12", "factory_parameter_13", null));

                    sub1.Dispose();
                    sub2.Dispose();
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB1),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, IReactiveQubscription>), Constants.SubscriptionFactory.SF),
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
                    new Uri(Constants.Subscription.SUB2),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, IReactiveQubscription>), Constants.SubscriptionFactory.SF),
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
                new DeleteSubscription(
                    new Uri(Constants.Subscription.SUB1)
                ),
                new DeleteSubscription(
                    new Uri(Constants.Subscription.SUB2)
                )
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_GetSubscriptionFactory_14P()
        {
            Apply(
                ctx =>
                {
                    var factoryUri = new Uri(Constants.SubscriptionFactory.SF);
                    var factory = ctx.GetSubscriptionFactory<string, string, string, string, string, string, string, string, string, string, string, string, string, string>(factoryUri);
                    Assert.AreEqual(factoryUri, ((IKnownResource)factory).Uri);

                    var sub1 = factory.Create(new Uri(Constants.Subscription.SUB1), "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", "factory_parameter_11", "factory_parameter_12", "factory_parameter_13", "factory_parameter_14", null);
                    var sub2 = ((IReactiveSubscriptionFactory<string, string, string, string, string, string, string, string, string, string, string, string, string, string>)factory).Create(new Uri(Constants.Subscription.SUB2), "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", "factory_parameter_11", "factory_parameter_12", "factory_parameter_13", "factory_parameter_14", null);

                    Assert.ThrowsException<ArgumentNullException>(() => factory.Create(null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", "factory_parameter_11", "factory_parameter_12", "factory_parameter_13", "factory_parameter_14", null));
                    Assert.ThrowsException<ArgumentNullException>(() => ((IReactiveSubscriptionFactory<string, string, string, string, string, string, string, string, string, string, string, string, string, string>)factory).Create(null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", "factory_parameter_11", "factory_parameter_12", "factory_parameter_13", "factory_parameter_14", null));

                    sub1.Dispose();
                    sub2.Dispose();
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB1),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, IReactiveQubscription>), Constants.SubscriptionFactory.SF),
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
                    new Uri(Constants.Subscription.SUB2),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, IReactiveQubscription>), Constants.SubscriptionFactory.SF),
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
                new DeleteSubscription(
                    new Uri(Constants.Subscription.SUB1)
                ),
                new DeleteSubscription(
                    new Uri(Constants.Subscription.SUB2)
                )
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_GetSubscriptionFactory_15P()
        {
            Apply(
                ctx =>
                {
                    var factoryUri = new Uri(Constants.SubscriptionFactory.SF);
                    var factory = ctx.GetSubscriptionFactory<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string>(factoryUri);
                    Assert.AreEqual(factoryUri, ((IKnownResource)factory).Uri);

                    var sub1 = factory.Create(new Uri(Constants.Subscription.SUB1), "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", "factory_parameter_11", "factory_parameter_12", "factory_parameter_13", "factory_parameter_14", "factory_parameter_15", null);
                    var sub2 = ((IReactiveSubscriptionFactory<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string>)factory).Create(new Uri(Constants.Subscription.SUB2), "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", "factory_parameter_11", "factory_parameter_12", "factory_parameter_13", "factory_parameter_14", "factory_parameter_15", null);

                    Assert.ThrowsException<ArgumentNullException>(() => factory.Create(null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", "factory_parameter_11", "factory_parameter_12", "factory_parameter_13", "factory_parameter_14", "factory_parameter_15", null));
                    Assert.ThrowsException<ArgumentNullException>(() => ((IReactiveSubscriptionFactory<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string>)factory).Create(null, "factory_parameter_1", "factory_parameter_2", "factory_parameter_3", "factory_parameter_4", "factory_parameter_5", "factory_parameter_6", "factory_parameter_7", "factory_parameter_8", "factory_parameter_9", "factory_parameter_10", "factory_parameter_11", "factory_parameter_12", "factory_parameter_13", "factory_parameter_14", "factory_parameter_15", null));

                    sub1.Dispose();
                    sub2.Dispose();
                },
                new CreateSubscription(
                    new Uri(Constants.Subscription.SUB1),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, IReactiveQubscription>), Constants.SubscriptionFactory.SF),
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
                    new Uri(Constants.Subscription.SUB2),
                    Expression.Invoke(
                        Expression.Parameter(typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, IReactiveQubscription>), Constants.SubscriptionFactory.SF),
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
                new DeleteSubscription(
                    new Uri(Constants.Subscription.SUB1)
                ),
                new DeleteSubscription(
                    new Uri(Constants.Subscription.SUB2)
                )
            );
        }

        #endregion

        #region "ReactiveServiceContext_DefineObservable"

        [TestMethod]
        public void ReactiveServiceContext_DefineObservable_2P()
        {
            var lambdaParameters = new[] {
                Expression.Parameter(typeof(string), "parameter_1"),
                Expression.Parameter(typeof(string), "parameter_2")
            };

            Apply(
                ctx =>
                {
                    var obs = ctx.GetObservable<string, string, int>(new Uri(Constants.Observable.XS));
                    ctx.DefineObservable<string, string, int>(
                        new Uri(Constants.Observable.YS),
                        (parameter_1, parameter_2) => obs(parameter_1, parameter_2),
                        state: null
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineObservable<string, string, int>(
                            uri: null,
                            (parameter_1, parameter_2) => obs(parameter_1, parameter_2),
                            state: null
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineObservable<string, string, int>(
                            new Uri(Constants.Observable.YS),
                            observable: null,
                            state: null
                        )
                    );
                },
                new DefineObservable(
                    new Uri(Constants.Observable.YS),
                    Expression.Lambda(
                        Expression.Invoke(
                            Expression.Parameter(
                                typeof(Func<string, string, IReactiveQbservable<int>>),
                                Constants.Observable.XS
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
        public void ReactiveServiceContext_DefineObservable_3P()
        {
            var lambdaParameters = new[] {
                Expression.Parameter(typeof(string), "parameter_1"),
                Expression.Parameter(typeof(string), "parameter_2"),
                Expression.Parameter(typeof(string), "parameter_3")
            };

            Apply(
                ctx =>
                {
                    var obs = ctx.GetObservable<string, string, string, int>(new Uri(Constants.Observable.XS));
                    ctx.DefineObservable<string, string, string, int>(
                        new Uri(Constants.Observable.YS),
                        (parameter_1, parameter_2, parameter_3) => obs(parameter_1, parameter_2, parameter_3),
                        state: null
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineObservable<string, string, string, int>(
                            uri: null,
                            (parameter_1, parameter_2, parameter_3) => obs(parameter_1, parameter_2, parameter_3),
                            state: null
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineObservable<string, string, string, int>(
                            new Uri(Constants.Observable.YS),
                            observable: null,
                            state: null
                        )
                    );
                },
                new DefineObservable(
                    new Uri(Constants.Observable.YS),
                    Expression.Lambda(
                        Expression.Invoke(
                            Expression.Parameter(
                                typeof(Func<string, string, string, IReactiveQbservable<int>>),
                                Constants.Observable.XS
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
        public void ReactiveServiceContext_DefineObservable_4P()
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
                    var obs = ctx.GetObservable<string, string, string, string, int>(new Uri(Constants.Observable.XS));
                    ctx.DefineObservable<string, string, string, string, int>(
                        new Uri(Constants.Observable.YS),
                        (parameter_1, parameter_2, parameter_3, parameter_4) => obs(parameter_1, parameter_2, parameter_3, parameter_4),
                        state: null
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineObservable<string, string, string, string, int>(
                            uri: null,
                            (parameter_1, parameter_2, parameter_3, parameter_4) => obs(parameter_1, parameter_2, parameter_3, parameter_4),
                            state: null
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineObservable<string, string, string, string, int>(
                            new Uri(Constants.Observable.YS),
                            observable: null,
                            state: null
                        )
                    );
                },
                new DefineObservable(
                    new Uri(Constants.Observable.YS),
                    Expression.Lambda(
                        Expression.Invoke(
                            Expression.Parameter(
                                typeof(Func<string, string, string, string, IReactiveQbservable<int>>),
                                Constants.Observable.XS
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
        public void ReactiveServiceContext_DefineObservable_5P()
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
                    var obs = ctx.GetObservable<string, string, string, string, string, int>(new Uri(Constants.Observable.XS));
                    ctx.DefineObservable<string, string, string, string, string, int>(
                        new Uri(Constants.Observable.YS),
                        (parameter_1, parameter_2, parameter_3, parameter_4, parameter_5) => obs(parameter_1, parameter_2, parameter_3, parameter_4, parameter_5),
                        state: null
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineObservable<string, string, string, string, string, int>(
                            uri: null,
                            (parameter_1, parameter_2, parameter_3, parameter_4, parameter_5) => obs(parameter_1, parameter_2, parameter_3, parameter_4, parameter_5),
                            state: null
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineObservable<string, string, string, string, string, int>(
                            new Uri(Constants.Observable.YS),
                            observable: null,
                            state: null
                        )
                    );
                },
                new DefineObservable(
                    new Uri(Constants.Observable.YS),
                    Expression.Lambda(
                        Expression.Invoke(
                            Expression.Parameter(
                                typeof(Func<string, string, string, string, string, IReactiveQbservable<int>>),
                                Constants.Observable.XS
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
        public void ReactiveServiceContext_DefineObservable_6P()
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
                    var obs = ctx.GetObservable<string, string, string, string, string, string, int>(new Uri(Constants.Observable.XS));
                    ctx.DefineObservable<string, string, string, string, string, string, int>(
                        new Uri(Constants.Observable.YS),
                        (parameter_1, parameter_2, parameter_3, parameter_4, parameter_5, parameter_6) => obs(parameter_1, parameter_2, parameter_3, parameter_4, parameter_5, parameter_6),
                        state: null
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineObservable<string, string, string, string, string, string, int>(
                            uri: null,
                            (parameter_1, parameter_2, parameter_3, parameter_4, parameter_5, parameter_6) => obs(parameter_1, parameter_2, parameter_3, parameter_4, parameter_5, parameter_6),
                            state: null
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineObservable<string, string, string, string, string, string, int>(
                            new Uri(Constants.Observable.YS),
                            observable: null,
                            state: null
                        )
                    );
                },
                new DefineObservable(
                    new Uri(Constants.Observable.YS),
                    Expression.Lambda(
                        Expression.Invoke(
                            Expression.Parameter(
                                typeof(Func<string, string, string, string, string, string, IReactiveQbservable<int>>),
                                Constants.Observable.XS
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
        public void ReactiveServiceContext_DefineObservable_7P()
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
                    var obs = ctx.GetObservable<string, string, string, string, string, string, string, int>(new Uri(Constants.Observable.XS));
                    ctx.DefineObservable<string, string, string, string, string, string, string, int>(
                        new Uri(Constants.Observable.YS),
                        (parameter_1, parameter_2, parameter_3, parameter_4, parameter_5, parameter_6, parameter_7) => obs(parameter_1, parameter_2, parameter_3, parameter_4, parameter_5, parameter_6, parameter_7),
                        state: null
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineObservable<string, string, string, string, string, string, string, int>(
                            uri: null,
                            (parameter_1, parameter_2, parameter_3, parameter_4, parameter_5, parameter_6, parameter_7) => obs(parameter_1, parameter_2, parameter_3, parameter_4, parameter_5, parameter_6, parameter_7),
                            state: null
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineObservable<string, string, string, string, string, string, string, int>(
                            new Uri(Constants.Observable.YS),
                            observable: null,
                            state: null
                        )
                    );
                },
                new DefineObservable(
                    new Uri(Constants.Observable.YS),
                    Expression.Lambda(
                        Expression.Invoke(
                            Expression.Parameter(
                                typeof(Func<string, string, string, string, string, string, string, IReactiveQbservable<int>>),
                                Constants.Observable.XS
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
        public void ReactiveServiceContext_DefineObservable_8P()
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
                    var obs = ctx.GetObservable<string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observable.XS));
                    ctx.DefineObservable<string, string, string, string, string, string, string, string, int>(
                        new Uri(Constants.Observable.YS),
                        (parameter_1, parameter_2, parameter_3, parameter_4, parameter_5, parameter_6, parameter_7, parameter_8) => obs(parameter_1, parameter_2, parameter_3, parameter_4, parameter_5, parameter_6, parameter_7, parameter_8),
                        state: null
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineObservable<string, string, string, string, string, string, string, string, int>(
                            uri: null,
                            (parameter_1, parameter_2, parameter_3, parameter_4, parameter_5, parameter_6, parameter_7, parameter_8) => obs(parameter_1, parameter_2, parameter_3, parameter_4, parameter_5, parameter_6, parameter_7, parameter_8),
                            state: null
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineObservable<string, string, string, string, string, string, string, string, int>(
                            new Uri(Constants.Observable.YS),
                            observable: null,
                            state: null
                        )
                    );
                },
                new DefineObservable(
                    new Uri(Constants.Observable.YS),
                    Expression.Lambda(
                        Expression.Invoke(
                            Expression.Parameter(
                                typeof(Func<string, string, string, string, string, string, string, string, IReactiveQbservable<int>>),
                                Constants.Observable.XS
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
        public void ReactiveServiceContext_DefineObservable_9P()
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
                    var obs = ctx.GetObservable<string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observable.XS));
                    ctx.DefineObservable<string, string, string, string, string, string, string, string, string, int>(
                        new Uri(Constants.Observable.YS),
                        (parameter_1, parameter_2, parameter_3, parameter_4, parameter_5, parameter_6, parameter_7, parameter_8, parameter_9) => obs(parameter_1, parameter_2, parameter_3, parameter_4, parameter_5, parameter_6, parameter_7, parameter_8, parameter_9),
                        state: null
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineObservable<string, string, string, string, string, string, string, string, string, int>(
                            uri: null,
                            (parameter_1, parameter_2, parameter_3, parameter_4, parameter_5, parameter_6, parameter_7, parameter_8, parameter_9) => obs(parameter_1, parameter_2, parameter_3, parameter_4, parameter_5, parameter_6, parameter_7, parameter_8, parameter_9),
                            state: null
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineObservable<string, string, string, string, string, string, string, string, string, int>(
                            new Uri(Constants.Observable.YS),
                            observable: null,
                            state: null
                        )
                    );
                },
                new DefineObservable(
                    new Uri(Constants.Observable.YS),
                    Expression.Lambda(
                        Expression.Invoke(
                            Expression.Parameter(
                                typeof(Func<string, string, string, string, string, string, string, string, string, IReactiveQbservable<int>>),
                                Constants.Observable.XS
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
        public void ReactiveServiceContext_DefineObservable_10P()
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
                    var obs = ctx.GetObservable<string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observable.XS));
                    ctx.DefineObservable<string, string, string, string, string, string, string, string, string, string, int>(
                        new Uri(Constants.Observable.YS),
                        (parameter_1, parameter_2, parameter_3, parameter_4, parameter_5, parameter_6, parameter_7, parameter_8, parameter_9, parameter_10) => obs(parameter_1, parameter_2, parameter_3, parameter_4, parameter_5, parameter_6, parameter_7, parameter_8, parameter_9, parameter_10),
                        state: null
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineObservable<string, string, string, string, string, string, string, string, string, string, int>(
                            uri: null,
                            (parameter_1, parameter_2, parameter_3, parameter_4, parameter_5, parameter_6, parameter_7, parameter_8, parameter_9, parameter_10) => obs(parameter_1, parameter_2, parameter_3, parameter_4, parameter_5, parameter_6, parameter_7, parameter_8, parameter_9, parameter_10),
                            state: null
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineObservable<string, string, string, string, string, string, string, string, string, string, int>(
                            new Uri(Constants.Observable.YS),
                            observable: null,
                            state: null
                        )
                    );
                },
                new DefineObservable(
                    new Uri(Constants.Observable.YS),
                    Expression.Lambda(
                        Expression.Invoke(
                            Expression.Parameter(
                                typeof(Func<string, string, string, string, string, string, string, string, string, string, IReactiveQbservable<int>>),
                                Constants.Observable.XS
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
        public void ReactiveServiceContext_DefineObservable_11P()
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
                    var obs = ctx.GetObservable<string, string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observable.XS));
                    ctx.DefineObservable<string, string, string, string, string, string, string, string, string, string, string, int>(
                        new Uri(Constants.Observable.YS),
                        (parameter_1, parameter_2, parameter_3, parameter_4, parameter_5, parameter_6, parameter_7, parameter_8, parameter_9, parameter_10, parameter_11) => obs(parameter_1, parameter_2, parameter_3, parameter_4, parameter_5, parameter_6, parameter_7, parameter_8, parameter_9, parameter_10, parameter_11),
                        state: null
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineObservable<string, string, string, string, string, string, string, string, string, string, string, int>(
                            uri: null,
                            (parameter_1, parameter_2, parameter_3, parameter_4, parameter_5, parameter_6, parameter_7, parameter_8, parameter_9, parameter_10, parameter_11) => obs(parameter_1, parameter_2, parameter_3, parameter_4, parameter_5, parameter_6, parameter_7, parameter_8, parameter_9, parameter_10, parameter_11),
                            state: null
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineObservable<string, string, string, string, string, string, string, string, string, string, string, int>(
                            new Uri(Constants.Observable.YS),
                            observable: null,
                            state: null
                        )
                    );
                },
                new DefineObservable(
                    new Uri(Constants.Observable.YS),
                    Expression.Lambda(
                        Expression.Invoke(
                            Expression.Parameter(
                                typeof(Func<string, string, string, string, string, string, string, string, string, string, string, IReactiveQbservable<int>>),
                                Constants.Observable.XS
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
        public void ReactiveServiceContext_DefineObservable_12P()
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
                    var obs = ctx.GetObservable<string, string, string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observable.XS));
                    ctx.DefineObservable<string, string, string, string, string, string, string, string, string, string, string, string, int>(
                        new Uri(Constants.Observable.YS),
                        (parameter_1, parameter_2, parameter_3, parameter_4, parameter_5, parameter_6, parameter_7, parameter_8, parameter_9, parameter_10, parameter_11, parameter_12) => obs(parameter_1, parameter_2, parameter_3, parameter_4, parameter_5, parameter_6, parameter_7, parameter_8, parameter_9, parameter_10, parameter_11, parameter_12),
                        state: null
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineObservable<string, string, string, string, string, string, string, string, string, string, string, string, int>(
                            uri: null,
                            (parameter_1, parameter_2, parameter_3, parameter_4, parameter_5, parameter_6, parameter_7, parameter_8, parameter_9, parameter_10, parameter_11, parameter_12) => obs(parameter_1, parameter_2, parameter_3, parameter_4, parameter_5, parameter_6, parameter_7, parameter_8, parameter_9, parameter_10, parameter_11, parameter_12),
                            state: null
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineObservable<string, string, string, string, string, string, string, string, string, string, string, string, int>(
                            new Uri(Constants.Observable.YS),
                            observable: null,
                            state: null
                        )
                    );
                },
                new DefineObservable(
                    new Uri(Constants.Observable.YS),
                    Expression.Lambda(
                        Expression.Invoke(
                            Expression.Parameter(
                                typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, IReactiveQbservable<int>>),
                                Constants.Observable.XS
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
        public void ReactiveServiceContext_DefineObservable_13P()
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
                    var obs = ctx.GetObservable<string, string, string, string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observable.XS));
                    ctx.DefineObservable<string, string, string, string, string, string, string, string, string, string, string, string, string, int>(
                        new Uri(Constants.Observable.YS),
                        (parameter_1, parameter_2, parameter_3, parameter_4, parameter_5, parameter_6, parameter_7, parameter_8, parameter_9, parameter_10, parameter_11, parameter_12, parameter_13) => obs(parameter_1, parameter_2, parameter_3, parameter_4, parameter_5, parameter_6, parameter_7, parameter_8, parameter_9, parameter_10, parameter_11, parameter_12, parameter_13),
                        state: null
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineObservable<string, string, string, string, string, string, string, string, string, string, string, string, string, int>(
                            uri: null,
                            (parameter_1, parameter_2, parameter_3, parameter_4, parameter_5, parameter_6, parameter_7, parameter_8, parameter_9, parameter_10, parameter_11, parameter_12, parameter_13) => obs(parameter_1, parameter_2, parameter_3, parameter_4, parameter_5, parameter_6, parameter_7, parameter_8, parameter_9, parameter_10, parameter_11, parameter_12, parameter_13),
                            state: null
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineObservable<string, string, string, string, string, string, string, string, string, string, string, string, string, int>(
                            new Uri(Constants.Observable.YS),
                            observable: null,
                            state: null
                        )
                    );
                },
                new DefineObservable(
                    new Uri(Constants.Observable.YS),
                    Expression.Lambda(
                        Expression.Invoke(
                            Expression.Parameter(
                                typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, IReactiveQbservable<int>>),
                                Constants.Observable.XS
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
        public void ReactiveServiceContext_DefineObservable_14P()
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
                    var obs = ctx.GetObservable<string, string, string, string, string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observable.XS));
                    ctx.DefineObservable<string, string, string, string, string, string, string, string, string, string, string, string, string, string, int>(
                        new Uri(Constants.Observable.YS),
                        (parameter_1, parameter_2, parameter_3, parameter_4, parameter_5, parameter_6, parameter_7, parameter_8, parameter_9, parameter_10, parameter_11, parameter_12, parameter_13, parameter_14) => obs(parameter_1, parameter_2, parameter_3, parameter_4, parameter_5, parameter_6, parameter_7, parameter_8, parameter_9, parameter_10, parameter_11, parameter_12, parameter_13, parameter_14),
                        state: null
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineObservable<string, string, string, string, string, string, string, string, string, string, string, string, string, string, int>(
                            uri: null,
                            (parameter_1, parameter_2, parameter_3, parameter_4, parameter_5, parameter_6, parameter_7, parameter_8, parameter_9, parameter_10, parameter_11, parameter_12, parameter_13, parameter_14) => obs(parameter_1, parameter_2, parameter_3, parameter_4, parameter_5, parameter_6, parameter_7, parameter_8, parameter_9, parameter_10, parameter_11, parameter_12, parameter_13, parameter_14),
                            state: null
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineObservable<string, string, string, string, string, string, string, string, string, string, string, string, string, string, int>(
                            new Uri(Constants.Observable.YS),
                            observable: null,
                            state: null
                        )
                    );
                },
                new DefineObservable(
                    new Uri(Constants.Observable.YS),
                    Expression.Lambda(
                        Expression.Invoke(
                            Expression.Parameter(
                                typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, IReactiveQbservable<int>>),
                                Constants.Observable.XS
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
        public void ReactiveServiceContext_DefineObservable_15P()
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
                    var obs = ctx.GetObservable<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observable.XS));
                    ctx.DefineObservable<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, int>(
                        new Uri(Constants.Observable.YS),
                        (parameter_1, parameter_2, parameter_3, parameter_4, parameter_5, parameter_6, parameter_7, parameter_8, parameter_9, parameter_10, parameter_11, parameter_12, parameter_13, parameter_14, parameter_15) => obs(parameter_1, parameter_2, parameter_3, parameter_4, parameter_5, parameter_6, parameter_7, parameter_8, parameter_9, parameter_10, parameter_11, parameter_12, parameter_13, parameter_14, parameter_15),
                        state: null
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineObservable<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, int>(
                            uri: null,
                            (parameter_1, parameter_2, parameter_3, parameter_4, parameter_5, parameter_6, parameter_7, parameter_8, parameter_9, parameter_10, parameter_11, parameter_12, parameter_13, parameter_14, parameter_15) => obs(parameter_1, parameter_2, parameter_3, parameter_4, parameter_5, parameter_6, parameter_7, parameter_8, parameter_9, parameter_10, parameter_11, parameter_12, parameter_13, parameter_14, parameter_15),
                            state: null
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineObservable<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, int>(
                            new Uri(Constants.Observable.YS),
                            observable: null,
                            state: null
                        )
                    );
                },
                new DefineObservable(
                    new Uri(Constants.Observable.YS),
                    Expression.Lambda(
                        Expression.Invoke(
                            Expression.Parameter(
                                typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, IReactiveQbservable<int>>),
                                Constants.Observable.XS
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

        #region "ReactiveServiceContext_DefineObserver"

        [TestMethod]
        public void ReactiveServiceContext_DefineObserver_2P()
        {
            var lambdaParameters = new[] {
                Expression.Parameter(typeof(string), "parameter_1"),
                Expression.Parameter(typeof(string), "parameter_2")
            };

            Apply(
                ctx =>
                {
                    var obs = ctx.GetObserver<string, string, int>(new Uri(Constants.Observer.XS));
                    var obsExpr = (Expression<Func<string, string, IReactiveQbserver<int>>>)((parameter_1, parameter_2) => obs(parameter_1, parameter_2));
                    ctx.DefineObserver<string, string, int>(new Uri(Constants.Observer.YS), obsExpr, null);

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineObserver<string, string, int>(
                            uri: null,
                            obsExpr,
                            state: null
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineObserver<string, string, int>(
                            new Uri(Constants.Observer.YS),
                            observer: null,
                            state: null
                        )
                    );
                },
                new DefineObserver(
                    new Uri(Constants.Observer.YS),
                    Expression.Lambda(
                        Expression.Invoke(
                            Expression.Parameter(
                                typeof(Func<string, string, IReactiveQbserver<int>>),
                                Constants.Observer.XS
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
        public void ReactiveServiceContext_DefineObserver_3P()
        {
            var lambdaParameters = new[] {
                Expression.Parameter(typeof(string), "parameter_1"),
                Expression.Parameter(typeof(string), "parameter_2"),
                Expression.Parameter(typeof(string), "parameter_3")
            };

            Apply(
                ctx =>
                {
                    var obs = ctx.GetObserver<string, string, string, int>(new Uri(Constants.Observer.XS));
                    var obsExpr = (Expression<Func<string, string, string, IReactiveQbserver<int>>>)((parameter_1, parameter_2, parameter_3) => obs(parameter_1, parameter_2, parameter_3));
                    ctx.DefineObserver<string, string, string, int>(new Uri(Constants.Observer.YS), obsExpr, null);

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineObserver<string, string, string, int>(
                            uri: null,
                            obsExpr,
                            state: null
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineObserver<string, string, string, int>(
                            new Uri(Constants.Observer.YS),
                            observer: null,
                            state: null
                        )
                    );
                },
                new DefineObserver(
                    new Uri(Constants.Observer.YS),
                    Expression.Lambda(
                        Expression.Invoke(
                            Expression.Parameter(
                                typeof(Func<string, string, string, IReactiveQbserver<int>>),
                                Constants.Observer.XS
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
        public void ReactiveServiceContext_DefineObserver_4P()
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
                    var obs = ctx.GetObserver<string, string, string, string, int>(new Uri(Constants.Observer.XS));
                    var obsExpr = (Expression<Func<string, string, string, string, IReactiveQbserver<int>>>)((parameter_1, parameter_2, parameter_3, parameter_4) => obs(parameter_1, parameter_2, parameter_3, parameter_4));
                    ctx.DefineObserver<string, string, string, string, int>(new Uri(Constants.Observer.YS), obsExpr, null);

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineObserver<string, string, string, string, int>(
                            uri: null,
                            obsExpr,
                            state: null
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineObserver<string, string, string, string, int>(
                            new Uri(Constants.Observer.YS),
                            observer: null,
                            state: null
                        )
                    );
                },
                new DefineObserver(
                    new Uri(Constants.Observer.YS),
                    Expression.Lambda(
                        Expression.Invoke(
                            Expression.Parameter(
                                typeof(Func<string, string, string, string, IReactiveQbserver<int>>),
                                Constants.Observer.XS
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
        public void ReactiveServiceContext_DefineObserver_5P()
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
                    var obs = ctx.GetObserver<string, string, string, string, string, int>(new Uri(Constants.Observer.XS));
                    var obsExpr = (Expression<Func<string, string, string, string, string, IReactiveQbserver<int>>>)((parameter_1, parameter_2, parameter_3, parameter_4, parameter_5) => obs(parameter_1, parameter_2, parameter_3, parameter_4, parameter_5));
                    ctx.DefineObserver<string, string, string, string, string, int>(new Uri(Constants.Observer.YS), obsExpr, null);

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineObserver<string, string, string, string, string, int>(
                            uri: null,
                            obsExpr,
                            state: null
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineObserver<string, string, string, string, string, int>(
                            new Uri(Constants.Observer.YS),
                            observer: null,
                            state: null
                        )
                    );
                },
                new DefineObserver(
                    new Uri(Constants.Observer.YS),
                    Expression.Lambda(
                        Expression.Invoke(
                            Expression.Parameter(
                                typeof(Func<string, string, string, string, string, IReactiveQbserver<int>>),
                                Constants.Observer.XS
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
        public void ReactiveServiceContext_DefineObserver_6P()
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
                    var obs = ctx.GetObserver<string, string, string, string, string, string, int>(new Uri(Constants.Observer.XS));
                    var obsExpr = (Expression<Func<string, string, string, string, string, string, IReactiveQbserver<int>>>)((parameter_1, parameter_2, parameter_3, parameter_4, parameter_5, parameter_6) => obs(parameter_1, parameter_2, parameter_3, parameter_4, parameter_5, parameter_6));
                    ctx.DefineObserver<string, string, string, string, string, string, int>(new Uri(Constants.Observer.YS), obsExpr, null);

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineObserver<string, string, string, string, string, string, int>(
                            uri: null,
                            obsExpr,
                            state: null
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineObserver<string, string, string, string, string, string, int>(
                            new Uri(Constants.Observer.YS),
                            observer: null,
                            state: null
                        )
                    );
                },
                new DefineObserver(
                    new Uri(Constants.Observer.YS),
                    Expression.Lambda(
                        Expression.Invoke(
                            Expression.Parameter(
                                typeof(Func<string, string, string, string, string, string, IReactiveQbserver<int>>),
                                Constants.Observer.XS
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
        public void ReactiveServiceContext_DefineObserver_7P()
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
                    var obs = ctx.GetObserver<string, string, string, string, string, string, string, int>(new Uri(Constants.Observer.XS));
                    var obsExpr = (Expression<Func<string, string, string, string, string, string, string, IReactiveQbserver<int>>>)((parameter_1, parameter_2, parameter_3, parameter_4, parameter_5, parameter_6, parameter_7) => obs(parameter_1, parameter_2, parameter_3, parameter_4, parameter_5, parameter_6, parameter_7));
                    ctx.DefineObserver<string, string, string, string, string, string, string, int>(new Uri(Constants.Observer.YS), obsExpr, null);

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineObserver<string, string, string, string, string, string, string, int>(
                            uri: null,
                            obsExpr,
                            state: null
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineObserver<string, string, string, string, string, string, string, int>(
                            new Uri(Constants.Observer.YS),
                            observer: null,
                            state: null
                        )
                    );
                },
                new DefineObserver(
                    new Uri(Constants.Observer.YS),
                    Expression.Lambda(
                        Expression.Invoke(
                            Expression.Parameter(
                                typeof(Func<string, string, string, string, string, string, string, IReactiveQbserver<int>>),
                                Constants.Observer.XS
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
        public void ReactiveServiceContext_DefineObserver_8P()
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
                    var obs = ctx.GetObserver<string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observer.XS));
                    var obsExpr = (Expression<Func<string, string, string, string, string, string, string, string, IReactiveQbserver<int>>>)((parameter_1, parameter_2, parameter_3, parameter_4, parameter_5, parameter_6, parameter_7, parameter_8) => obs(parameter_1, parameter_2, parameter_3, parameter_4, parameter_5, parameter_6, parameter_7, parameter_8));
                    ctx.DefineObserver<string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observer.YS), obsExpr, null);

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineObserver<string, string, string, string, string, string, string, string, int>(
                            uri: null,
                            obsExpr,
                            state: null
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineObserver<string, string, string, string, string, string, string, string, int>(
                            new Uri(Constants.Observer.YS),
                            observer: null,
                            state: null
                        )
                    );
                },
                new DefineObserver(
                    new Uri(Constants.Observer.YS),
                    Expression.Lambda(
                        Expression.Invoke(
                            Expression.Parameter(
                                typeof(Func<string, string, string, string, string, string, string, string, IReactiveQbserver<int>>),
                                Constants.Observer.XS
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
        public void ReactiveServiceContext_DefineObserver_9P()
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
                    var obs = ctx.GetObserver<string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observer.XS));
                    var obsExpr = (Expression<Func<string, string, string, string, string, string, string, string, string, IReactiveQbserver<int>>>)((parameter_1, parameter_2, parameter_3, parameter_4, parameter_5, parameter_6, parameter_7, parameter_8, parameter_9) => obs(parameter_1, parameter_2, parameter_3, parameter_4, parameter_5, parameter_6, parameter_7, parameter_8, parameter_9));
                    ctx.DefineObserver<string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observer.YS), obsExpr, null);

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineObserver<string, string, string, string, string, string, string, string, string, int>(
                            uri: null,
                            obsExpr,
                            state: null
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineObserver<string, string, string, string, string, string, string, string, string, int>(
                            new Uri(Constants.Observer.YS),
                            observer: null,
                            state: null
                        )
                    );
                },
                new DefineObserver(
                    new Uri(Constants.Observer.YS),
                    Expression.Lambda(
                        Expression.Invoke(
                            Expression.Parameter(
                                typeof(Func<string, string, string, string, string, string, string, string, string, IReactiveQbserver<int>>),
                                Constants.Observer.XS
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
        public void ReactiveServiceContext_DefineObserver_10P()
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
                    var obs = ctx.GetObserver<string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observer.XS));
                    var obsExpr = (Expression<Func<string, string, string, string, string, string, string, string, string, string, IReactiveQbserver<int>>>)((parameter_1, parameter_2, parameter_3, parameter_4, parameter_5, parameter_6, parameter_7, parameter_8, parameter_9, parameter_10) => obs(parameter_1, parameter_2, parameter_3, parameter_4, parameter_5, parameter_6, parameter_7, parameter_8, parameter_9, parameter_10));
                    ctx.DefineObserver<string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observer.YS), obsExpr, null);

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineObserver<string, string, string, string, string, string, string, string, string, string, int>(
                            uri: null,
                            obsExpr,
                            state: null
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineObserver<string, string, string, string, string, string, string, string, string, string, int>(
                            new Uri(Constants.Observer.YS),
                            observer: null,
                            state: null
                        )
                    );
                },
                new DefineObserver(
                    new Uri(Constants.Observer.YS),
                    Expression.Lambda(
                        Expression.Invoke(
                            Expression.Parameter(
                                typeof(Func<string, string, string, string, string, string, string, string, string, string, IReactiveQbserver<int>>),
                                Constants.Observer.XS
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
        public void ReactiveServiceContext_DefineObserver_11P()
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
                    var obs = ctx.GetObserver<string, string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observer.XS));
                    var obsExpr = (Expression<Func<string, string, string, string, string, string, string, string, string, string, string, IReactiveQbserver<int>>>)((parameter_1, parameter_2, parameter_3, parameter_4, parameter_5, parameter_6, parameter_7, parameter_8, parameter_9, parameter_10, parameter_11) => obs(parameter_1, parameter_2, parameter_3, parameter_4, parameter_5, parameter_6, parameter_7, parameter_8, parameter_9, parameter_10, parameter_11));
                    ctx.DefineObserver<string, string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observer.YS), obsExpr, null);

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineObserver<string, string, string, string, string, string, string, string, string, string, string, int>(
                            uri: null,
                            obsExpr,
                            state: null
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineObserver<string, string, string, string, string, string, string, string, string, string, string, int>(
                            new Uri(Constants.Observer.YS),
                            observer: null,
                            state: null
                        )
                    );
                },
                new DefineObserver(
                    new Uri(Constants.Observer.YS),
                    Expression.Lambda(
                        Expression.Invoke(
                            Expression.Parameter(
                                typeof(Func<string, string, string, string, string, string, string, string, string, string, string, IReactiveQbserver<int>>),
                                Constants.Observer.XS
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
        public void ReactiveServiceContext_DefineObserver_12P()
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
                    var obs = ctx.GetObserver<string, string, string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observer.XS));
                    var obsExpr = (Expression<Func<string, string, string, string, string, string, string, string, string, string, string, string, IReactiveQbserver<int>>>)((parameter_1, parameter_2, parameter_3, parameter_4, parameter_5, parameter_6, parameter_7, parameter_8, parameter_9, parameter_10, parameter_11, parameter_12) => obs(parameter_1, parameter_2, parameter_3, parameter_4, parameter_5, parameter_6, parameter_7, parameter_8, parameter_9, parameter_10, parameter_11, parameter_12));
                    ctx.DefineObserver<string, string, string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observer.YS), obsExpr, null);

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineObserver<string, string, string, string, string, string, string, string, string, string, string, string, int>(
                            uri: null,
                            obsExpr,
                            state: null
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineObserver<string, string, string, string, string, string, string, string, string, string, string, string, int>(
                            new Uri(Constants.Observer.YS),
                            observer: null,
                            state: null
                        )
                    );
                },
                new DefineObserver(
                    new Uri(Constants.Observer.YS),
                    Expression.Lambda(
                        Expression.Invoke(
                            Expression.Parameter(
                                typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, IReactiveQbserver<int>>),
                                Constants.Observer.XS
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
        public void ReactiveServiceContext_DefineObserver_13P()
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
                    var obs = ctx.GetObserver<string, string, string, string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observer.XS));
                    var obsExpr = (Expression<Func<string, string, string, string, string, string, string, string, string, string, string, string, string, IReactiveQbserver<int>>>)((parameter_1, parameter_2, parameter_3, parameter_4, parameter_5, parameter_6, parameter_7, parameter_8, parameter_9, parameter_10, parameter_11, parameter_12, parameter_13) => obs(parameter_1, parameter_2, parameter_3, parameter_4, parameter_5, parameter_6, parameter_7, parameter_8, parameter_9, parameter_10, parameter_11, parameter_12, parameter_13));
                    ctx.DefineObserver<string, string, string, string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observer.YS), obsExpr, null);

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineObserver<string, string, string, string, string, string, string, string, string, string, string, string, string, int>(
                            uri: null,
                            obsExpr,
                            state: null
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineObserver<string, string, string, string, string, string, string, string, string, string, string, string, string, int>(
                            new Uri(Constants.Observer.YS),
                            observer: null,
                            state: null
                        )
                    );
                },
                new DefineObserver(
                    new Uri(Constants.Observer.YS),
                    Expression.Lambda(
                        Expression.Invoke(
                            Expression.Parameter(
                                typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, IReactiveQbserver<int>>),
                                Constants.Observer.XS
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
        public void ReactiveServiceContext_DefineObserver_14P()
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
                    var obs = ctx.GetObserver<string, string, string, string, string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observer.XS));
                    var obsExpr = (Expression<Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, IReactiveQbserver<int>>>)((parameter_1, parameter_2, parameter_3, parameter_4, parameter_5, parameter_6, parameter_7, parameter_8, parameter_9, parameter_10, parameter_11, parameter_12, parameter_13, parameter_14) => obs(parameter_1, parameter_2, parameter_3, parameter_4, parameter_5, parameter_6, parameter_7, parameter_8, parameter_9, parameter_10, parameter_11, parameter_12, parameter_13, parameter_14));
                    ctx.DefineObserver<string, string, string, string, string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observer.YS), obsExpr, null);

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineObserver<string, string, string, string, string, string, string, string, string, string, string, string, string, string, int>(
                            uri: null,
                            obsExpr,
                            state: null
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineObserver<string, string, string, string, string, string, string, string, string, string, string, string, string, string, int>(
                            new Uri(Constants.Observer.YS),
                            observer: null,
                            state: null
                        )
                    );
                },
                new DefineObserver(
                    new Uri(Constants.Observer.YS),
                    Expression.Lambda(
                        Expression.Invoke(
                            Expression.Parameter(
                                typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, IReactiveQbserver<int>>),
                                Constants.Observer.XS
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
        public void ReactiveServiceContext_DefineObserver_15P()
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
                    var obs = ctx.GetObserver<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observer.XS));
                    var obsExpr = (Expression<Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, IReactiveQbserver<int>>>)((parameter_1, parameter_2, parameter_3, parameter_4, parameter_5, parameter_6, parameter_7, parameter_8, parameter_9, parameter_10, parameter_11, parameter_12, parameter_13, parameter_14, parameter_15) => obs(parameter_1, parameter_2, parameter_3, parameter_4, parameter_5, parameter_6, parameter_7, parameter_8, parameter_9, parameter_10, parameter_11, parameter_12, parameter_13, parameter_14, parameter_15));
                    ctx.DefineObserver<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, int>(new Uri(Constants.Observer.YS), obsExpr, null);

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineObserver<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, int>(
                            uri: null,
                            obsExpr,
                            state: null
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineObserver<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, int>(
                            new Uri(Constants.Observer.YS),
                            observer: null,
                            state: null
                        )
                    );
                },
                new DefineObserver(
                    new Uri(Constants.Observer.YS),
                    Expression.Lambda(
                        Expression.Invoke(
                            Expression.Parameter(
                                typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, IReactiveQbserver<int>>),
                                Constants.Observer.XS
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

        #region "ReactiveServiceContext_DefineStreamFactory"

        [TestMethod]
        public void ReactiveServiceContext_DefineStreamFactory_2P()
        {
            Apply(
                ctx =>
                {
                    var factory = ctx.GetStreamFactory<string, string, int, int>(new Uri(Constants.StreamFactory.SF));
                    ctx.DefineStreamFactory<string, string, int, int>(new Uri(Constants.StreamFactory.SG), factory, null);

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineStreamFactory<string, string, int, int>(
                            uri: null,
                            factory,
                            state: null
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineStreamFactory<string, string, int, int>(
                            new Uri(Constants.StreamFactory.SG),
                            streamFactory: null,
                            state: null
                        )
                    );
                },
                new DefineStreamFactory(
                    new Uri(Constants.StreamFactory.SG),
                    Expression.Parameter(
                        typeof(Func<string, string, IReactiveQubject<int, int>>),
                        Constants.StreamFactory.SF
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_DefineStreamFactory_3P()
        {
            Apply(
                ctx =>
                {
                    var factory = ctx.GetStreamFactory<string, string, string, int, int>(new Uri(Constants.StreamFactory.SF));
                    ctx.DefineStreamFactory<string, string, string, int, int>(new Uri(Constants.StreamFactory.SG), factory, null);

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineStreamFactory<string, string, string, int, int>(
                            uri: null,
                            factory,
                            state: null
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineStreamFactory<string, string, string, int, int>(
                            new Uri(Constants.StreamFactory.SG),
                            streamFactory: null,
                            state: null
                        )
                    );
                },
                new DefineStreamFactory(
                    new Uri(Constants.StreamFactory.SG),
                    Expression.Parameter(
                        typeof(Func<string, string, string, IReactiveQubject<int, int>>),
                        Constants.StreamFactory.SF
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_DefineStreamFactory_4P()
        {
            Apply(
                ctx =>
                {
                    var factory = ctx.GetStreamFactory<string, string, string, string, int, int>(new Uri(Constants.StreamFactory.SF));
                    ctx.DefineStreamFactory<string, string, string, string, int, int>(new Uri(Constants.StreamFactory.SG), factory, null);

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineStreamFactory<string, string, string, string, int, int>(
                            uri: null,
                            factory,
                            state: null
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineStreamFactory<string, string, string, string, int, int>(
                            new Uri(Constants.StreamFactory.SG),
                            streamFactory: null,
                            state: null
                        )
                    );
                },
                new DefineStreamFactory(
                    new Uri(Constants.StreamFactory.SG),
                    Expression.Parameter(
                        typeof(Func<string, string, string, string, IReactiveQubject<int, int>>),
                        Constants.StreamFactory.SF
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_DefineStreamFactory_5P()
        {
            Apply(
                ctx =>
                {
                    var factory = ctx.GetStreamFactory<string, string, string, string, string, int, int>(new Uri(Constants.StreamFactory.SF));
                    ctx.DefineStreamFactory<string, string, string, string, string, int, int>(new Uri(Constants.StreamFactory.SG), factory, null);

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineStreamFactory<string, string, string, string, string, int, int>(
                            uri: null,
                            factory,
                            state: null
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineStreamFactory<string, string, string, string, string, int, int>(
                            new Uri(Constants.StreamFactory.SG),
                            streamFactory: null,
                            state: null
                        )
                    );
                },
                new DefineStreamFactory(
                    new Uri(Constants.StreamFactory.SG),
                    Expression.Parameter(
                        typeof(Func<string, string, string, string, string, IReactiveQubject<int, int>>),
                        Constants.StreamFactory.SF
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_DefineStreamFactory_6P()
        {
            Apply(
                ctx =>
                {
                    var factory = ctx.GetStreamFactory<string, string, string, string, string, string, int, int>(new Uri(Constants.StreamFactory.SF));
                    ctx.DefineStreamFactory<string, string, string, string, string, string, int, int>(new Uri(Constants.StreamFactory.SG), factory, null);

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineStreamFactory<string, string, string, string, string, string, int, int>(
                            uri: null,
                            factory,
                            state: null
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineStreamFactory<string, string, string, string, string, string, int, int>(
                            new Uri(Constants.StreamFactory.SG),
                            streamFactory: null,
                            state: null
                        )
                    );
                },
                new DefineStreamFactory(
                    new Uri(Constants.StreamFactory.SG),
                    Expression.Parameter(
                        typeof(Func<string, string, string, string, string, string, IReactiveQubject<int, int>>),
                        Constants.StreamFactory.SF
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_DefineStreamFactory_7P()
        {
            Apply(
                ctx =>
                {
                    var factory = ctx.GetStreamFactory<string, string, string, string, string, string, string, int, int>(new Uri(Constants.StreamFactory.SF));
                    ctx.DefineStreamFactory<string, string, string, string, string, string, string, int, int>(new Uri(Constants.StreamFactory.SG), factory, null);

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineStreamFactory<string, string, string, string, string, string, string, int, int>(
                            uri: null,
                            factory,
                            state: null
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineStreamFactory<string, string, string, string, string, string, string, int, int>(
                            new Uri(Constants.StreamFactory.SG),
                            streamFactory: null,
                            state: null
                        )
                    );
                },
                new DefineStreamFactory(
                    new Uri(Constants.StreamFactory.SG),
                    Expression.Parameter(
                        typeof(Func<string, string, string, string, string, string, string, IReactiveQubject<int, int>>),
                        Constants.StreamFactory.SF
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_DefineStreamFactory_8P()
        {
            Apply(
                ctx =>
                {
                    var factory = ctx.GetStreamFactory<string, string, string, string, string, string, string, string, int, int>(new Uri(Constants.StreamFactory.SF));
                    ctx.DefineStreamFactory<string, string, string, string, string, string, string, string, int, int>(new Uri(Constants.StreamFactory.SG), factory, null);

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineStreamFactory<string, string, string, string, string, string, string, string, int, int>(
                            uri: null,
                            factory,
                            state: null
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineStreamFactory<string, string, string, string, string, string, string, string, int, int>(
                            new Uri(Constants.StreamFactory.SG),
                            streamFactory: null,
                            state: null
                        )
                    );
                },
                new DefineStreamFactory(
                    new Uri(Constants.StreamFactory.SG),
                    Expression.Parameter(
                        typeof(Func<string, string, string, string, string, string, string, string, IReactiveQubject<int, int>>),
                        Constants.StreamFactory.SF
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_DefineStreamFactory_9P()
        {
            Apply(
                ctx =>
                {
                    var factory = ctx.GetStreamFactory<string, string, string, string, string, string, string, string, string, int, int>(new Uri(Constants.StreamFactory.SF));
                    ctx.DefineStreamFactory<string, string, string, string, string, string, string, string, string, int, int>(new Uri(Constants.StreamFactory.SG), factory, null);

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineStreamFactory<string, string, string, string, string, string, string, string, string, int, int>(
                            uri: null,
                            factory,
                            state: null
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineStreamFactory<string, string, string, string, string, string, string, string, string, int, int>(
                            new Uri(Constants.StreamFactory.SG),
                            streamFactory: null,
                            state: null
                        )
                    );
                },
                new DefineStreamFactory(
                    new Uri(Constants.StreamFactory.SG),
                    Expression.Parameter(
                        typeof(Func<string, string, string, string, string, string, string, string, string, IReactiveQubject<int, int>>),
                        Constants.StreamFactory.SF
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_DefineStreamFactory_10P()
        {
            Apply(
                ctx =>
                {
                    var factory = ctx.GetStreamFactory<string, string, string, string, string, string, string, string, string, string, int, int>(new Uri(Constants.StreamFactory.SF));
                    ctx.DefineStreamFactory<string, string, string, string, string, string, string, string, string, string, int, int>(new Uri(Constants.StreamFactory.SG), factory, null);

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineStreamFactory<string, string, string, string, string, string, string, string, string, string, int, int>(
                            uri: null,
                            factory,
                            state: null
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineStreamFactory<string, string, string, string, string, string, string, string, string, string, int, int>(
                            new Uri(Constants.StreamFactory.SG),
                            streamFactory: null,
                            state: null
                        )
                    );
                },
                new DefineStreamFactory(
                    new Uri(Constants.StreamFactory.SG),
                    Expression.Parameter(
                        typeof(Func<string, string, string, string, string, string, string, string, string, string, IReactiveQubject<int, int>>),
                        Constants.StreamFactory.SF
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_DefineStreamFactory_11P()
        {
            Apply(
                ctx =>
                {
                    var factory = ctx.GetStreamFactory<string, string, string, string, string, string, string, string, string, string, string, int, int>(new Uri(Constants.StreamFactory.SF));
                    ctx.DefineStreamFactory<string, string, string, string, string, string, string, string, string, string, string, int, int>(new Uri(Constants.StreamFactory.SG), factory, null);

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineStreamFactory<string, string, string, string, string, string, string, string, string, string, string, int, int>(
                            uri: null,
                            factory,
                            state: null
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineStreamFactory<string, string, string, string, string, string, string, string, string, string, string, int, int>(
                            new Uri(Constants.StreamFactory.SG),
                            streamFactory: null,
                            state: null
                        )
                    );
                },
                new DefineStreamFactory(
                    new Uri(Constants.StreamFactory.SG),
                    Expression.Parameter(
                        typeof(Func<string, string, string, string, string, string, string, string, string, string, string, IReactiveQubject<int, int>>),
                        Constants.StreamFactory.SF
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_DefineStreamFactory_12P()
        {
            Apply(
                ctx =>
                {
                    var factory = ctx.GetStreamFactory<string, string, string, string, string, string, string, string, string, string, string, string, int, int>(new Uri(Constants.StreamFactory.SF));
                    ctx.DefineStreamFactory<string, string, string, string, string, string, string, string, string, string, string, string, int, int>(new Uri(Constants.StreamFactory.SG), factory, null);

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineStreamFactory<string, string, string, string, string, string, string, string, string, string, string, string, int, int>(
                            uri: null,
                            factory,
                            state: null
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineStreamFactory<string, string, string, string, string, string, string, string, string, string, string, string, int, int>(
                            new Uri(Constants.StreamFactory.SG),
                            streamFactory: null,
                            state: null
                        )
                    );
                },
                new DefineStreamFactory(
                    new Uri(Constants.StreamFactory.SG),
                    Expression.Parameter(
                        typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, IReactiveQubject<int, int>>),
                        Constants.StreamFactory.SF
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_DefineStreamFactory_13P()
        {
            Apply(
                ctx =>
                {
                    var factory = ctx.GetStreamFactory<string, string, string, string, string, string, string, string, string, string, string, string, string, int, int>(new Uri(Constants.StreamFactory.SF));
                    ctx.DefineStreamFactory<string, string, string, string, string, string, string, string, string, string, string, string, string, int, int>(new Uri(Constants.StreamFactory.SG), factory, null);

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineStreamFactory<string, string, string, string, string, string, string, string, string, string, string, string, string, int, int>(
                            uri: null,
                            factory,
                            state: null
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineStreamFactory<string, string, string, string, string, string, string, string, string, string, string, string, string, int, int>(
                            new Uri(Constants.StreamFactory.SG),
                            streamFactory: null,
                            state: null
                        )
                    );
                },
                new DefineStreamFactory(
                    new Uri(Constants.StreamFactory.SG),
                    Expression.Parameter(
                        typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, IReactiveQubject<int, int>>),
                        Constants.StreamFactory.SF
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_DefineStreamFactory_14P()
        {
            Apply(
                ctx =>
                {
                    var factory = ctx.GetStreamFactory<string, string, string, string, string, string, string, string, string, string, string, string, string, string, int, int>(new Uri(Constants.StreamFactory.SF));
                    ctx.DefineStreamFactory<string, string, string, string, string, string, string, string, string, string, string, string, string, string, int, int>(new Uri(Constants.StreamFactory.SG), factory, null);

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineStreamFactory<string, string, string, string, string, string, string, string, string, string, string, string, string, string, int, int>(
                            uri: null,
                            factory,
                            state: null
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineStreamFactory<string, string, string, string, string, string, string, string, string, string, string, string, string, string, int, int>(
                            new Uri(Constants.StreamFactory.SG),
                            streamFactory: null,
                            state: null
                        )
                    );
                },
                new DefineStreamFactory(
                    new Uri(Constants.StreamFactory.SG),
                    Expression.Parameter(
                        typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, IReactiveQubject<int, int>>),
                        Constants.StreamFactory.SF
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_DefineStreamFactory_15P()
        {
            Apply(
                ctx =>
                {
                    var factory = ctx.GetStreamFactory<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, int, int>(new Uri(Constants.StreamFactory.SF));
                    ctx.DefineStreamFactory<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, int, int>(new Uri(Constants.StreamFactory.SG), factory, null);

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineStreamFactory<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, int, int>(
                            uri: null,
                            factory,
                            state: null
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineStreamFactory<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, int, int>(
                            new Uri(Constants.StreamFactory.SG),
                            streamFactory: null,
                            state: null
                        )
                    );
                },
                new DefineStreamFactory(
                    new Uri(Constants.StreamFactory.SG),
                    Expression.Parameter(
                        typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, IReactiveQubject<int, int>>),
                        Constants.StreamFactory.SF
                    ),
                    state: null
                )
            );
        }

        #endregion

        #region "ReactiveServiceContext_DefineSubscriptionFactory"

        [TestMethod]
        public void ReactiveServiceContext_DefineSubscriptionFactory_2P()
        {
            Apply(
                ctx =>
                {
                    var factory = ctx.GetSubscriptionFactory<string, string>(new Uri(Constants.SubscriptionFactory.SF));
                    ctx.DefineSubscriptionFactory<string, string>(new Uri(Constants.SubscriptionFactory.SG), factory, null);

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineSubscriptionFactory<string, string>(
                            uri: null,
                            factory,
                            state: null
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineSubscriptionFactory<string, string>(
                            new Uri(Constants.SubscriptionFactory.SG),
                            subscriptionFactory: null,
                            state: null
                        )
                    );
                },
                new DefineSubscriptionFactory(
                    new Uri(Constants.SubscriptionFactory.SG),
                    Expression.Parameter(
                        typeof(Func<string, string, IReactiveQubscription>),
                        Constants.SubscriptionFactory.SF
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_DefineSubscriptionFactory_3P()
        {
            Apply(
                ctx =>
                {
                    var factory = ctx.GetSubscriptionFactory<string, string, string>(new Uri(Constants.SubscriptionFactory.SF));
                    ctx.DefineSubscriptionFactory<string, string, string>(new Uri(Constants.SubscriptionFactory.SG), factory, null);

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineSubscriptionFactory<string, string, string>(
                            uri: null,
                            factory,
                            state: null
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineSubscriptionFactory<string, string, string>(
                            new Uri(Constants.SubscriptionFactory.SG),
                            subscriptionFactory: null,
                            state: null
                        )
                    );
                },
                new DefineSubscriptionFactory(
                    new Uri(Constants.SubscriptionFactory.SG),
                    Expression.Parameter(
                        typeof(Func<string, string, string, IReactiveQubscription>),
                        Constants.SubscriptionFactory.SF
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_DefineSubscriptionFactory_4P()
        {
            Apply(
                ctx =>
                {
                    var factory = ctx.GetSubscriptionFactory<string, string, string, string>(new Uri(Constants.SubscriptionFactory.SF));
                    ctx.DefineSubscriptionFactory<string, string, string, string>(new Uri(Constants.SubscriptionFactory.SG), factory, null);

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineSubscriptionFactory<string, string, string, string>(
                            uri: null,
                            factory,
                            state: null
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineSubscriptionFactory<string, string, string, string>(
                            new Uri(Constants.SubscriptionFactory.SG),
                            subscriptionFactory: null,
                            state: null
                        )
                    );
                },
                new DefineSubscriptionFactory(
                    new Uri(Constants.SubscriptionFactory.SG),
                    Expression.Parameter(
                        typeof(Func<string, string, string, string, IReactiveQubscription>),
                        Constants.SubscriptionFactory.SF
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_DefineSubscriptionFactory_5P()
        {
            Apply(
                ctx =>
                {
                    var factory = ctx.GetSubscriptionFactory<string, string, string, string, string>(new Uri(Constants.SubscriptionFactory.SF));
                    ctx.DefineSubscriptionFactory<string, string, string, string, string>(new Uri(Constants.SubscriptionFactory.SG), factory, null);

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineSubscriptionFactory<string, string, string, string, string>(
                            uri: null,
                            factory,
                            state: null
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineSubscriptionFactory<string, string, string, string, string>(
                            new Uri(Constants.SubscriptionFactory.SG),
                            subscriptionFactory: null,
                            state: null
                        )
                    );
                },
                new DefineSubscriptionFactory(
                    new Uri(Constants.SubscriptionFactory.SG),
                    Expression.Parameter(
                        typeof(Func<string, string, string, string, string, IReactiveQubscription>),
                        Constants.SubscriptionFactory.SF
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_DefineSubscriptionFactory_6P()
        {
            Apply(
                ctx =>
                {
                    var factory = ctx.GetSubscriptionFactory<string, string, string, string, string, string>(new Uri(Constants.SubscriptionFactory.SF));
                    ctx.DefineSubscriptionFactory<string, string, string, string, string, string>(new Uri(Constants.SubscriptionFactory.SG), factory, null);

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineSubscriptionFactory<string, string, string, string, string, string>(
                            uri: null,
                            factory,
                            state: null
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineSubscriptionFactory<string, string, string, string, string, string>(
                            new Uri(Constants.SubscriptionFactory.SG),
                            subscriptionFactory: null,
                            state: null
                        )
                    );
                },
                new DefineSubscriptionFactory(
                    new Uri(Constants.SubscriptionFactory.SG),
                    Expression.Parameter(
                        typeof(Func<string, string, string, string, string, string, IReactiveQubscription>),
                        Constants.SubscriptionFactory.SF
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_DefineSubscriptionFactory_7P()
        {
            Apply(
                ctx =>
                {
                    var factory = ctx.GetSubscriptionFactory<string, string, string, string, string, string, string>(new Uri(Constants.SubscriptionFactory.SF));
                    ctx.DefineSubscriptionFactory<string, string, string, string, string, string, string>(new Uri(Constants.SubscriptionFactory.SG), factory, null);

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineSubscriptionFactory<string, string, string, string, string, string, string>(
                            uri: null,
                            factory,
                            state: null
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineSubscriptionFactory<string, string, string, string, string, string, string>(
                            new Uri(Constants.SubscriptionFactory.SG),
                            subscriptionFactory: null,
                            state: null
                        )
                    );
                },
                new DefineSubscriptionFactory(
                    new Uri(Constants.SubscriptionFactory.SG),
                    Expression.Parameter(
                        typeof(Func<string, string, string, string, string, string, string, IReactiveQubscription>),
                        Constants.SubscriptionFactory.SF
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_DefineSubscriptionFactory_8P()
        {
            Apply(
                ctx =>
                {
                    var factory = ctx.GetSubscriptionFactory<string, string, string, string, string, string, string, string>(new Uri(Constants.SubscriptionFactory.SF));
                    ctx.DefineSubscriptionFactory<string, string, string, string, string, string, string, string>(new Uri(Constants.SubscriptionFactory.SG), factory, null);

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineSubscriptionFactory<string, string, string, string, string, string, string, string>(
                            uri: null,
                            factory,
                            state: null
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineSubscriptionFactory<string, string, string, string, string, string, string, string>(
                            new Uri(Constants.SubscriptionFactory.SG),
                            subscriptionFactory: null,
                            state: null
                        )
                    );
                },
                new DefineSubscriptionFactory(
                    new Uri(Constants.SubscriptionFactory.SG),
                    Expression.Parameter(
                        typeof(Func<string, string, string, string, string, string, string, string, IReactiveQubscription>),
                        Constants.SubscriptionFactory.SF
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_DefineSubscriptionFactory_9P()
        {
            Apply(
                ctx =>
                {
                    var factory = ctx.GetSubscriptionFactory<string, string, string, string, string, string, string, string, string>(new Uri(Constants.SubscriptionFactory.SF));
                    ctx.DefineSubscriptionFactory<string, string, string, string, string, string, string, string, string>(new Uri(Constants.SubscriptionFactory.SG), factory, null);

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineSubscriptionFactory<string, string, string, string, string, string, string, string, string>(
                            uri: null,
                            factory,
                            state: null
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineSubscriptionFactory<string, string, string, string, string, string, string, string, string>(
                            new Uri(Constants.SubscriptionFactory.SG),
                            subscriptionFactory: null,
                            state: null
                        )
                    );
                },
                new DefineSubscriptionFactory(
                    new Uri(Constants.SubscriptionFactory.SG),
                    Expression.Parameter(
                        typeof(Func<string, string, string, string, string, string, string, string, string, IReactiveQubscription>),
                        Constants.SubscriptionFactory.SF
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_DefineSubscriptionFactory_10P()
        {
            Apply(
                ctx =>
                {
                    var factory = ctx.GetSubscriptionFactory<string, string, string, string, string, string, string, string, string, string>(new Uri(Constants.SubscriptionFactory.SF));
                    ctx.DefineSubscriptionFactory<string, string, string, string, string, string, string, string, string, string>(new Uri(Constants.SubscriptionFactory.SG), factory, null);

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineSubscriptionFactory<string, string, string, string, string, string, string, string, string, string>(
                            uri: null,
                            factory,
                            state: null
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineSubscriptionFactory<string, string, string, string, string, string, string, string, string, string>(
                            new Uri(Constants.SubscriptionFactory.SG),
                            subscriptionFactory: null,
                            state: null
                        )
                    );
                },
                new DefineSubscriptionFactory(
                    new Uri(Constants.SubscriptionFactory.SG),
                    Expression.Parameter(
                        typeof(Func<string, string, string, string, string, string, string, string, string, string, IReactiveQubscription>),
                        Constants.SubscriptionFactory.SF
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_DefineSubscriptionFactory_11P()
        {
            Apply(
                ctx =>
                {
                    var factory = ctx.GetSubscriptionFactory<string, string, string, string, string, string, string, string, string, string, string>(new Uri(Constants.SubscriptionFactory.SF));
                    ctx.DefineSubscriptionFactory<string, string, string, string, string, string, string, string, string, string, string>(new Uri(Constants.SubscriptionFactory.SG), factory, null);

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineSubscriptionFactory<string, string, string, string, string, string, string, string, string, string, string>(
                            uri: null,
                            factory,
                            state: null
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineSubscriptionFactory<string, string, string, string, string, string, string, string, string, string, string>(
                            new Uri(Constants.SubscriptionFactory.SG),
                            subscriptionFactory: null,
                            state: null
                        )
                    );
                },
                new DefineSubscriptionFactory(
                    new Uri(Constants.SubscriptionFactory.SG),
                    Expression.Parameter(
                        typeof(Func<string, string, string, string, string, string, string, string, string, string, string, IReactiveQubscription>),
                        Constants.SubscriptionFactory.SF
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_DefineSubscriptionFactory_12P()
        {
            Apply(
                ctx =>
                {
                    var factory = ctx.GetSubscriptionFactory<string, string, string, string, string, string, string, string, string, string, string, string>(new Uri(Constants.SubscriptionFactory.SF));
                    ctx.DefineSubscriptionFactory<string, string, string, string, string, string, string, string, string, string, string, string>(new Uri(Constants.SubscriptionFactory.SG), factory, null);

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineSubscriptionFactory<string, string, string, string, string, string, string, string, string, string, string, string>(
                            uri: null,
                            factory,
                            state: null
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineSubscriptionFactory<string, string, string, string, string, string, string, string, string, string, string, string>(
                            new Uri(Constants.SubscriptionFactory.SG),
                            subscriptionFactory: null,
                            state: null
                        )
                    );
                },
                new DefineSubscriptionFactory(
                    new Uri(Constants.SubscriptionFactory.SG),
                    Expression.Parameter(
                        typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, IReactiveQubscription>),
                        Constants.SubscriptionFactory.SF
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_DefineSubscriptionFactory_13P()
        {
            Apply(
                ctx =>
                {
                    var factory = ctx.GetSubscriptionFactory<string, string, string, string, string, string, string, string, string, string, string, string, string>(new Uri(Constants.SubscriptionFactory.SF));
                    ctx.DefineSubscriptionFactory<string, string, string, string, string, string, string, string, string, string, string, string, string>(new Uri(Constants.SubscriptionFactory.SG), factory, null);

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineSubscriptionFactory<string, string, string, string, string, string, string, string, string, string, string, string, string>(
                            uri: null,
                            factory,
                            state: null
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineSubscriptionFactory<string, string, string, string, string, string, string, string, string, string, string, string, string>(
                            new Uri(Constants.SubscriptionFactory.SG),
                            subscriptionFactory: null,
                            state: null
                        )
                    );
                },
                new DefineSubscriptionFactory(
                    new Uri(Constants.SubscriptionFactory.SG),
                    Expression.Parameter(
                        typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, IReactiveQubscription>),
                        Constants.SubscriptionFactory.SF
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_DefineSubscriptionFactory_14P()
        {
            Apply(
                ctx =>
                {
                    var factory = ctx.GetSubscriptionFactory<string, string, string, string, string, string, string, string, string, string, string, string, string, string>(new Uri(Constants.SubscriptionFactory.SF));
                    ctx.DefineSubscriptionFactory<string, string, string, string, string, string, string, string, string, string, string, string, string, string>(new Uri(Constants.SubscriptionFactory.SG), factory, null);

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineSubscriptionFactory<string, string, string, string, string, string, string, string, string, string, string, string, string, string>(
                            uri: null,
                            factory,
                            state: null
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineSubscriptionFactory<string, string, string, string, string, string, string, string, string, string, string, string, string, string>(
                            new Uri(Constants.SubscriptionFactory.SG),
                            subscriptionFactory: null,
                            state: null
                        )
                    );
                },
                new DefineSubscriptionFactory(
                    new Uri(Constants.SubscriptionFactory.SG),
                    Expression.Parameter(
                        typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, IReactiveQubscription>),
                        Constants.SubscriptionFactory.SF
                    ),
                    state: null
                )
            );
        }

        [TestMethod]
        public void ReactiveServiceContext_DefineSubscriptionFactory_15P()
        {
            Apply(
                ctx =>
                {
                    var factory = ctx.GetSubscriptionFactory<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string>(new Uri(Constants.SubscriptionFactory.SF));
                    ctx.DefineSubscriptionFactory<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string>(new Uri(Constants.SubscriptionFactory.SG), factory, null);

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineSubscriptionFactory<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string>(
                            uri: null,
                            factory,
                            state: null
                        )
                    );

                    Assert.ThrowsException<ArgumentNullException>(() =>
                        ctx.DefineSubscriptionFactory<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string>(
                            new Uri(Constants.SubscriptionFactory.SG),
                            subscriptionFactory: null,
                            state: null
                        )
                    );
                },
                new DefineSubscriptionFactory(
                    new Uri(Constants.SubscriptionFactory.SG),
                    Expression.Parameter(
                        typeof(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, IReactiveQubscription>),
                        Constants.SubscriptionFactory.SF
                    ),
                    state: null
                )
            );
        }

        #endregion

        private sealed class MyParameterizedContext : MyContext
        {
            public MyParameterizedContext(IReactiveEngineProvider provider)
                : base(provider)
            {
            }

            [KnownResource(Constants.Observable.XS + "2")]
            public IReactiveQbservable<int> Xs2p(string p1, string p2)
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
            public IReactiveQbservable<int> Xs3p(string p1, string p2, string p3)
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
            public IReactiveQbservable<int> Xs4p(string p1, string p2, string p3, string p4)
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
            public IReactiveQbservable<int> Xs5p(string p1, string p2, string p3, string p4, string p5)
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
            public IReactiveQbservable<int> Xs6p(string p1, string p2, string p3, string p4, string p5, string p6)
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
            public IReactiveQbservable<int> Xs7p(string p1, string p2, string p3, string p4, string p5, string p6, string p7)
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
            public IReactiveQbservable<int> Xs8p(string p1, string p2, string p3, string p4, string p5, string p6, string p7, string p8)
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
            public IReactiveQbservable<int> Xs9p(string p1, string p2, string p3, string p4, string p5, string p6, string p7, string p8, string p9)
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
            public IReactiveQbservable<int> Xs10p(string p1, string p2, string p3, string p4, string p5, string p6, string p7, string p8, string p9, string p10)
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
            public IReactiveQbservable<int> Xs11p(string p1, string p2, string p3, string p4, string p5, string p6, string p7, string p8, string p9, string p10, string p11)
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
            public IReactiveQbservable<int> Xs12p(string p1, string p2, string p3, string p4, string p5, string p6, string p7, string p8, string p9, string p10, string p11, string p12)
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
            public IReactiveQbservable<int> Xs13p(string p1, string p2, string p3, string p4, string p5, string p6, string p7, string p8, string p9, string p10, string p11, string p12, string p13)
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
            public IReactiveQbservable<int> Xs14p(string p1, string p2, string p3, string p4, string p5, string p6, string p7, string p8, string p9, string p10, string p11, string p12, string p13, string p14)
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
            public IReactiveQbservable<int> Xs15p(string p1, string p2, string p3, string p4, string p5, string p6, string p7, string p8, string p9, string p10, string p11, string p12, string p13, string p14, string p15)
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