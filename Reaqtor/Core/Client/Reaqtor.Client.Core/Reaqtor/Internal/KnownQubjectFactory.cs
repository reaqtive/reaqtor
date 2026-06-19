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
    internal sealed class KnownQubjectFactory<TInput, TOutput> : QubjectFactory<TInput, TOutput>, IKnownResource
    {
        public KnownQubjectFactory(Expression expression, Uri streamFactoryUri, IAsyncReactiveQueryProvider provider)
            : base(expression, provider)
        {
            Uri = streamFactoryUri;
        }

        public Uri Uri { get; }
    }

    internal sealed class KnownQubjectFactory<TInput, TOutput, TArg> : QubjectFactory<TInput, TOutput, TArg>, IKnownResource
    {
        public KnownQubjectFactory(Expression expression, Uri streamFactoryUri, IAsyncReactiveQueryProvider provider)
            : base(expression, provider)
        {
            Uri = streamFactoryUri;
        }

        public Uri Uri { get; }
    }
}
