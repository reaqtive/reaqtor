// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

using Reaqtor;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class ReactiveSubscriptionBaseTests
    {
        [TestMethod]
        public void ReactiveSubscriptionBase_Dispose()
        {
            var s = new MyReactiveSubscription();

            var disposed = false;
            s.DisposeImpl = () => { disposed = true; };

            s.Dispose();

            Assert.IsTrue(disposed);
        }

        private sealed class MyReactiveSubscription : ReactiveSubscriptionBase
        {
            public Action DisposeImpl;

            protected override void DisposeCore()
            {
                DisposeImpl();
            }
        }
    }
}
