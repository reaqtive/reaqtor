// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - February 2016 - Created this file.
//

using System;

namespace Reaqtor.TestingFramework
{
    [Serializable]
    public class UndefineSubscriptionFactory : UndefineServiceOperation
    {
        public UndefineSubscriptionFactory(Uri subscriptionFactoryUri)
            : base(ServiceOperationKind.UndefineSubscriptionFactory, subscriptionFactoryUri)
        {
        }
    }
}
