// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using Reaqtor.Hosting.Shared.Tools;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Microsoft.Hosting.Shared.Tools
{
    [TestClass]
    public class ReactiveEntitiesTests
    {
        [TestMethod]
        public void ReactiveEntities_AssertTables()
        {
            var entities = new ReactiveEntities();
            Assert.IsNotNull(entities.Observables);
            Assert.IsNotNull(entities.Observers);
            Assert.IsNotNull(entities.StreamFactories);
            Assert.IsNotNull(entities.SubscriptionFactories);
            Assert.IsNotNull(entities.Streams);
            Assert.IsNotNull(entities.Subscriptions);
        }
    }
}
