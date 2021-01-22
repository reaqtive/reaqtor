// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
//

using System;
using System.Linq.Expressions;

namespace Reaqtor
{
    internal sealed class KnownQubscription : Qubscription, IKnownResource
    {
        public KnownQubscription(Expression expression, Uri subscriptionUri, IAsyncReactiveQueryProvider provider)
            : base(expression, provider)
        {
            Uri = subscriptionUri;
        }

        public Uri Uri { get; }
    }
}
