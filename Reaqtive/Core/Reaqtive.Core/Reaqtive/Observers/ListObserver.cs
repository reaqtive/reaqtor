// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;

namespace Reaqtive
{
    internal class ListObserver<TSource> : IObserver<TSource>
    {
        private readonly IObserver<TSource>[] _observers;

        public ListObserver(params IObserver<TSource>[] observers)
        {
            Debug.Assert(observers != null);

            _observers = observers;
        }

        public IObserver<TSource> Add(IObserver<TSource> observer)
        {
            var observers = new IObserver<TSource>[_observers.Length + 1];

            Array.Copy(_observers, observers, _observers.Length);
            observers[_observers.Length] = observer ?? throw new ArgumentNullException(nameof(observer));

            return new ListObserver<TSource>(observers);
        }

        public IObserver<TSource> Remove(IObserver<TSource> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            var index = -1;

            for (var i = _observers.Length - 1; i >= 0; i--)
            {
                if (_observers[i] == observer)
                {
                    index = i;
                    break;
                }
            }

            if (index == -1)
            {
                throw new InvalidOperationException("Observer not found.");
            }

            if (_observers.Length == 2)
            {
                var remaining = 1 ^ index;
                return _observers[remaining];
            }

            var observers = new IObserver<TSource>[_observers.Length - 1];

            if (index > 0)
            {
                Array.Copy(_observers, 0, observers, 0, index);
            }

            var next = index + 1;

            if (next < _observers.Length)
            {
                Array.Copy(_observers, next, observers, index, _observers.Length - next);
            }

            return new ListObserver<TSource>(observers);
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

        public void OnNext(TSource value)
        {
            foreach (var observer in _observers)
            {
                observer.OnNext(value);
            }
        }
    }
}
