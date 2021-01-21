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
using System.Collections.Generic;
using System.Reactive.Disposables;

namespace PartitionedSubject
{
    internal class PartitionedSubject<T, K> : IObserver<T>
    {
        private readonly ConcurrentDictionary<K, IObserver<T>> _observers;
        private readonly Func<T, K> _keySelector;

        public PartitionedSubject(Func<T, K> keySelector, IEqualityComparer<K> comparer)
        {
            _keySelector = keySelector;
            _observers = new ConcurrentDictionary<K, IObserver<T>>(comparer);
        }

        public void OnCompleted()
        {
            foreach (var observer in _observers.Values)
            {
                observer.OnCompleted();
            }
        }

        public void OnError(Exception error)
        {
            foreach (var observer in _observers.Values)
            {
                observer.OnError(error);
            }
        }

        public void OnNext(T value)
        {
            var key = _keySelector(value); // TODO: error strategy

            if (_observers.TryGetValue(key, out var observer))
            {
                observer.OnNext(value);
            }
        }

        public IDisposable Subscribe(K key, IObserver<T> observer) // TODO: [Get|Create]Observable strategy for binding
        {
            while (true)
            {
                if (_observers.TryGetValue(key, out var oldEntry))
                {
                    var newEntry = default(IObserver<T>);

                    if (oldEntry is ListObserver<T> lst)
                    {
                        newEntry = lst.Add(observer);
                    }
                    else
                    {
                        if (oldEntry == Sentinels<T>.Empty)
                        {
                            newEntry = observer;
                        }
                        else
                        {
                            newEntry = new ListObserver<T>(oldEntry, observer);
                        }
                    }

                    if (_observers.TryUpdate(key, newEntry, oldEntry))
                    {
                        break;
                    }
                }
                else
                {
                    if (_observers.TryAdd(key, observer))
                    {
                        break;
                    }
                }
            }

            return Disposable.Create(() =>
            {
                while (true)
                {
                    if (_observers.TryGetValue(key, out var oldEntry))
                    {
                        var newEntry = default(IObserver<T>);

                        if (oldEntry is ListObserver<T> lst)
                        {
                            newEntry = lst.Remove(observer);
                        }
                        else
                        {
                            if (oldEntry == observer)
                            {
                                newEntry = Sentinels<T>.Empty;
                            }
                            else
                            {
                                break;
                            }
                        }

                        if (_observers.TryUpdate(key, newEntry, oldEntry))
                        {
                            break;
                        }
                    }
                }
            });
        }
    }
}
