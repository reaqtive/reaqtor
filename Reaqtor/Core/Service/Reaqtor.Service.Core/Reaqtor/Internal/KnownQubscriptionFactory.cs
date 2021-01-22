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

namespace Reaqtor
{
    internal sealed class KnownQubscriptionFactory : QubscriptionFactory, IKnownResource
    {
        public KnownQubscriptionFactory(Expression expression, Uri subscriptionFactoryUri, IReactiveQueryProvider provider)
            : base(expression, provider)
        {
            Uri = subscriptionFactoryUri;
        }

        public Uri Uri { get; }
    }

    internal sealed class KnownQubscriptionFactory<TArg> : QubscriptionFactory<TArg>, IKnownResource
    {
        public KnownQubscriptionFactory(Expression expression, Uri subscriptionFactoryUri, IReactiveQueryProvider provider)
            : base(expression, provider)
        {
            Uri = subscriptionFactoryUri;
        }

        public Uri Uri { get; }
    }
}
