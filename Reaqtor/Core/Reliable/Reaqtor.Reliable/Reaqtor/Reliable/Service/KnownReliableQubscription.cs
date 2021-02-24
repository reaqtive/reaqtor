﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq.Expressions;

using Reaqtor.Reliable.Expressions;

namespace Reaqtor.Reliable.Service
{
    public class KnownReliableQubscription : ReliableQubscription, IKnownResource
    {
        public KnownReliableQubscription(Expression expression, Uri uri, IReliableQueryProvider provider)
            : base(expression, provider)
        {
            Uri = uri;
        }

        public Uri Uri { get; }
    }
}
