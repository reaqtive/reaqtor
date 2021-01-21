// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

namespace Reaqtor.Reliable.Client
{
    public abstract class ReliableReactiveObserverBase<T> : IReliableReactiveObserver<T>
    {
        public abstract Uri ResubscribeUri { get; }

        public void OnNext(T item, long sequenceId) => OnNextCore(item, sequenceId);

        protected abstract void OnNextCore(T item, long sequenceId);

        public void OnStarted() => OnStartedCore();

        protected abstract void OnStartedCore();

        public void OnError(Exception error)
        {
            if (error == null)
                throw new ArgumentNullException(nameof(error));

            OnErrorCore(error);
        }

#pragma warning disable CA1716 // Identifiers should not match keywords. (Using error from `IObserver<T>.OnError(Exception error)`.)

        protected abstract void OnErrorCore(Exception error);

#pragma warning restore CA1716

        public void OnCompleted() => OnCompletedCore();

        protected abstract void OnCompletedCore();
    }
}
