// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

namespace Reaqtor.Reliable.Client;

public abstract class ReliableReactiveObservableBase<T> : IReliableReactiveObservable<T>
{
    public IReliableReactiveSubscription Subscribe(IReliableReactiveObserver<T> observer, Uri subscriptionUri, object state = null)
    {
        ArgumentNullException.ThrowIfNull(observer);
        ArgumentNullException.ThrowIfNull(subscriptionUri);

        return SubscribeCore(observer, subscriptionUri, state);
    }

    protected abstract IReliableReactiveSubscription SubscribeCore(IReliableReactiveObserver<T> observer, Uri subscriptionUri, object state);
}
