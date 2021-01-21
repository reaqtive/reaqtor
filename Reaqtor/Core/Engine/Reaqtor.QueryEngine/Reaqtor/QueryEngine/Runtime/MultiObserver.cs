// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

namespace Reaqtor.QueryEngine
{
    /// <summary>
    /// Observer backed by a static list of underlying observers that get notified sequentially,
    /// analogous to a multicast delegate.
    /// </summary>
    /// <typeparam name="T">The type of the events processed by the observer.</typeparam>
    internal sealed class MultiObserver<T> : IObserver<T>
    {
        public readonly IObserver<T>[] _observers;

        public MultiObserver(IObserver<T>[] observers)
        {
            _observers = observers;
        }

        public void OnCompleted()
        {
            foreach (var observer in _observers)
            {
                observer.OnCompleted();
            }
        }

        public void OnError(Exception error)
        {
            foreach (var observer in _observers)
            {
                observer.OnError(error);
            }
        }

        public void OnNext(T value)
        {
            foreach (var observer in _observers)
            {
                observer.OnNext(value);
            }
        }
    }
}
