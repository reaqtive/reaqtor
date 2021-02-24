// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

using Reaqtor.Reliable.Client;

namespace Reaqtor.Reliable.Expressions
{
    public interface IReliableQubscriptionFactory : IReliableReactiveSubscriptionFactory, IReliableReactiveExpressible
    {
        new IReliableQubscription Create(Uri subscriptionUri, object state = null);
    }

    public interface IReliableQubscriptionFactory<TArgs> : IReliableReactiveSubscriptionFactory<TArgs>, IReliableReactiveExpressible
    {
        new IReliableQubscription Create(Uri subscriptionUri, TArgs argument, object state = null);
    }
}
