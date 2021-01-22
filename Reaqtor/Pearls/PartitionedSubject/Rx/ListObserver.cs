// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - October 2014
//

using System;

namespace PartitionedSubject
{
    internal class ListObserver<T> : IObserver<T>
    {
        private readonly IObserver<T>[] _observers;

        public ListObserver(params IObserver<T>[] observers)
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

        public IObserver<T> Add(IObserver<T> observer)
        {
            var observers = new IObserver<T>[_observers.Length + 1];
            Array.Copy(_observers, observers, _observers.Length);
            observers[_observers.Length] = observer;
            return new ListObserver<T>(observers);
        }

        public IObserver<T> Remove(IObserver<T> observer)
        {
            var i = Array.IndexOf(_observers, observer);
            if (i >= 0)
            {
                if (_observers.Length == 1)
                {
                    return Sentinels<T>.Empty;
                }
                else if (_observers.Length == 2)
                {
                    var r = 1 ^ i;
                    return _observers[r];
                }
                else
                {
                    var observers = new IObserver<T>[_observers.Length - 1];

                    if (i > 0)
                    {
                        Array.Copy(_observers, observers, i);
                    }

                    if (i + 1 < _observers.Length)
                    {
                        Array.Copy(_observers, i + 1, observers, i, _observers.Length - i - 1);
                    }

                    return new ListObserver<T>(observers);
                }
            }

            return this;
        }
    }
}
