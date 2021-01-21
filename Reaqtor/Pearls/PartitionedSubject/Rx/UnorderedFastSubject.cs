// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - October 2014
//

using System;
using System.Collections.Concurrent;
using System.Reactive.Disposables;
using System.Reactive.Subjects;

namespace PartitionedSubject
{
    internal class UnorderedFastSubject<T> : ISubject<T>
    {
        private readonly ConcurrentDictionary<IObserver<T>, object> _observers = new();

        public void OnCompleted()
        {
            foreach (var observer in _observers.Keys)
            {
                observer.OnCompleted();
            }
        }

        public void OnError(Exception error)
        {
            foreach (var observer in _observers.Keys)
            {
                observer.OnError(error);
            }
        }

        public void OnNext(T value)
        {
            foreach (var observer in _observers.Keys)
            {
                observer.OnNext(value);
            }
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            if (!_observers.TryAdd(observer, null))
                throw new InvalidOperationException("This subject requires unique observers.");

            return Disposable.Create(() =>
            {
                _observers.TryRemove(observer, out var ignored);
            });
        }
    }
}
