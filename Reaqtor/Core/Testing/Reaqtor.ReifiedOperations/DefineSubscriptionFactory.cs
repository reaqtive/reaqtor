// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - February 2016 - Created this file.
//

using System;
using System.Linq.Expressions;

namespace Reaqtor.TestingFramework
{
    [Serializable]
    public class DefineSubscriptionFactory : DefineServiceOperation
    {
        public DefineSubscriptionFactory(Uri subscriptionFactoryUri)
            : this(subscriptionFactoryUri, subscriptionFactory: null, state: null)
        {
        }

        public DefineSubscriptionFactory(Uri subscriptionFactoryUri, Expression subscriptionFactory, object state)
            : base(ServiceOperationKind.DefineSubscriptionFactory, subscriptionFactoryUri, subscriptionFactory, state)
        {
        }
    }
}
