// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Reflection;

using Reaqtor;
using Reaqtor.Hosting.Shared.Tools;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Microsoft.Hosting.Shared.Tools
{
    [TestClass]
    public class ReactiveEntityTypeExtensionsTests
    {
        [TestMethod]
        public void ReactiveEntityTypeExtensions_FromTypeSlim()
        {
            AssertType(ReactiveEntityType.None, typeof(int));
            AssertType(ReactiveEntityType.None, typeof(Func<int, int>));
            AssertType(ReactiveEntityType.Observable, typeof(IAsyncReactiveQbservable<int>));
            AssertType(ReactiveEntityType.Observer, typeof(IAsyncReactiveQbserver<int>));
            AssertType(ReactiveEntityType.StreamFactory, typeof(IAsyncReactiveQubjectFactory<int, int>));
            AssertType(ReactiveEntityType.SubscriptionFactory, typeof(IAsyncReactiveQubscriptionFactory));
            AssertType(ReactiveEntityType.Stream, typeof(IAsyncReactiveQubject<int, int>));
            AssertType(ReactiveEntityType.Subscription, typeof(IAsyncReactiveQubscription));
            AssertType(ReactiveEntityType.Func | ReactiveEntityType.Observable, typeof(Func<int, IAsyncReactiveQbservable<int>>));
        }

        private static void AssertType(ReactiveEntityType reactiveType, Type type)
        {
            Assert.AreEqual(reactiveType, ReactiveEntityTypeExtensions.FromType(type));
            Assert.AreEqual(reactiveType, ReactiveEntityTypeExtensions.FromTypeSlim(type.ToTypeSlim()));
        }
    }
}
