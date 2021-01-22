// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq.Expressions;

using Nuqleon.DataModel;

namespace Reaqtor.QueryEngine
{
    internal class EdgeDescription
    {
        [Mapping("nucleus://engine/edge_description/expression")]
        public Expression Expression { get; set; }

        [Mapping("nucleus://engine/edge_description/internal_uri")]
        public Uri InternalUri { get; set; }

        [Mapping("nucleus://engine/edge_description/internal_subscription_uri")]
        public Uri InternalSubscriptionUri { get; set; }

        [Mapping("nucleus://engine/edge_description/external_uri")]
        public Uri ExternalUri { get; set; }

        [Mapping("nucleus://engine/edge_description/external_subscription_uri")]
        public Uri ExternalSubscriptionUri { get; set; }
    }
}
