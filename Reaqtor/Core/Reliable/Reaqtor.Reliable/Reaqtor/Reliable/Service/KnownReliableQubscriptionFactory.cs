// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq.Expressions;

using Reaqtor.Reliable.Expressions;

namespace Reaqtor.Reliable.Service
{
    public class KnownReliableQubscriptionFactory(Expression expression, Uri uri, IReliableQueryProvider provider) : ReliableQubscriptionFactory(expression, provider), IKnownResource
    {
        public Uri Uri { get; } = uri;
    }

    public class KnownReliableQubscriptionFactory<TArg>(Expression expression, Uri uri, IReliableQueryProvider provider) : ReliableQubscriptionFactory<TArg>(expression, provider), IKnownResource
    {
        public Uri Uri { get; } = uri;
    }
}
