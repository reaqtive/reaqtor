// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

namespace Reaqtor.Reliable.Client
{
    public interface IReliableReactiveObservable<out T>
    {
        IReliableReactiveSubscription Subscribe(IReliableReactiveObserver<T> observer, Uri subscriptionUri, object state = null);
    }
}
