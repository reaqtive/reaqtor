// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - July 2013 - Created this file.
//

using System;
using System.Linq.Expressions;

namespace Reaqtor.TestingFramework
{
    [Serializable]
    public class CreateSubscription : CreateServiceOperation
    {
        public CreateSubscription(Uri subscriptionUri)
            : this(subscriptionUri, subscription: null, state: null)
        {
        }

        public CreateSubscription(Uri subscriptionUri, Expression subscription, object state)
            : base(ServiceOperationKind.CreateSubscription, subscriptionUri, subscription, state)
        {
        }
    }
}
