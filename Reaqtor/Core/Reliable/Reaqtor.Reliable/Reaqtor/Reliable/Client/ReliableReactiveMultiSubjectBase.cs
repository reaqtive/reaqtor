// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

namespace Reaqtor.Reliable.Client
{
    public abstract class ReliableReactiveMultiSubjectBase<TInput, TOutput> : IReliableReactiveMultiSubject<TInput, TOutput>
    {
        public IReliableReactiveObserver<TInput> CreateObserver()
        {
            return CreateObserverCore();
        }

        protected abstract IReliableReactiveObserver<TInput> CreateObserverCore();

        public IReliableReactiveSubscription Subscribe(IReliableReactiveObserver<TOutput> observer, Uri subscriptionUri, object state = null)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (subscriptionUri == null)
                throw new ArgumentNullException(nameof(subscriptionUri));

            return SubscribeCore(observer, subscriptionUri, state);
        }

        protected abstract IReliableReactiveSubscription SubscribeCore(IReliableReactiveObserver<TOutput> observer, Uri subscriptionUri, object state);

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                DisposeCore();
            }
        }

        protected abstract void DisposeCore();
    }
}
