// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - October 2013 - Created this file.
//

using System;
using System.Linq.Expressions;

namespace Reaqtor.TestingFramework
{
    [Serializable]
    public class InsertSubscriptionMetadata(Uri subscriptionUri, Expression expression, object state) : InsertMetadataOperation(ServiceOperationKind.InsertSubscriptionMetadata, subscriptionUri, expression, state)
    {
        public InsertSubscriptionMetadata(Uri subscriptionUri)
            : this(subscriptionUri, null, null)
        {
        }
    }
}
