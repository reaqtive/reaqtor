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
    internal sealed class KnownQubjectFactory<TInput, TOutput>(Expression expression, Uri streamFactoryUri, IAsyncReactiveQueryProvider provider) : QubjectFactory<TInput, TOutput>(expression, provider), IKnownResource
    {
        public Uri Uri { get; } = streamFactoryUri;
    }

    internal sealed class KnownQubjectFactory<TInput, TOutput, TArg>(Expression expression, Uri streamFactoryUri, IAsyncReactiveQueryProvider provider) : QubjectFactory<TInput, TOutput, TArg>(expression, provider), IKnownResource
    {
        public Uri Uri { get; } = streamFactoryUri;
    }
}
