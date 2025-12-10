// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - February 2016 - Created this file.
// IG - 2025/12       - Remove CLR serialization support.
//

using System;

namespace Reaqtor.TestingFramework
{
    public class LookupSubscriptionFactoryMetadata : LookupMetadataOperation
    {
        public LookupSubscriptionFactoryMetadata(Uri subscriptionFactoryUri)
            : base(ServiceOperationKind.LookupSubscriptionFactoryMetadata, subscriptionFactoryUri)
        {
        }
    }
}
