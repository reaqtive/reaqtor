// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

using Reaqtive;
using Reaqtive.Testing;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.Reaqtive.Operators
{
    [TestClass]
    public partial class DelaySubscription : OperatorTestBase
    {
        [TestMethod]
        public void DelaySubscription_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.DelaySubscription(default(ISubscribable<int>), DateTimeOffset.Now));

            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.DelaySubscription(default(ISubscribable<int>), TimeSpan.Zero));

            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Subscribable.DelaySubscription(DummySubscribable<int>.Instance, TimeSpan.FromSeconds(-1)));
        }
    }
}
