// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using Reaqtive;

using Reaqtor.Reliable;

namespace Reaqtor.Shebang.App
{
    internal sealed class ReliableSubject<T> : IReliableSubject<T>
    {
        private readonly object _gate = new();
        private readonly SortedDictionary<long, T> _values = new();
        private readonly List<IObserver<(long sequenceId, T item)>> _observers = new();
        private Exception _error;
        private bool _done;

        public void OnCompleted()
        {
            lock (_gate)
            {
                _done = true;

                foreach (var observer in _observers)
                {
                    observer.OnCompleted();
                }
            }
        }

        public void OnError(Exception error)
        {
            lock (_gate)
            {
                _error = error;

                foreach (var observer in _observers)
                {
                    observer.OnError(error);
                }
            }
        }

        public void OnNext((long sequenceId, T item) value)
        {
            if (value.sequenceId < 0)
                throw new ArgumentOutOfRangeException(nameof(value));

            lock (_gate)
            {
                // NB: Deduplication of single producer

                if (!_values.ContainsKey(value.sequenceId))
                {
                    _values.Add(value.sequenceId, value.item);

                    foreach (var observer in _observers)
                    {
                        observer.OnNext(value);
                    }
                }
            }
        }

        public IDisposable Subscribe(IObserver<(long sequenceId, T item)> observer) => Subscribe(observer, null);

        internal IDisposable Subscribe(IReliableObserver<T> observer, long sequenceId) => Subscribe(new Observer(observer), sequenceId);

        private IDisposable Subscribe(IObserver<(long sequenceId, T item)> observer, long? sequenceId)
        {
            lock (_gate)
            {
                if (_done)
                {
                    observer.OnCompleted();
                    return new Subscription(this, null);
                }

                if (_error != null)
                {
                    observer.OnError(_error);
                    return new Subscription(this, null);
                }

                if (sequenceId >= 0)
                {
                    foreach (var item in _values.SkipWhile(x => x.Key < sequenceId))
                    {
                        observer.OnNext((item.Key, item.Value));
                    }
                }

                _observers.Add(observer);
                return new Subscription(this, observer);
            }
        }

        private sealed class Observer : IObserver<(long sequenceId, T item)>
        {
            private readonly IReliableObserver<T> _observer;

            public Observer(IReliableObserver<T> observer) => _observer = observer;

            public void OnCompleted() => _observer.OnCompleted();

            public void OnError(Exception error) => _observer.OnError(error);

            public void OnNext((long sequenceId, T item) value) => _observer.OnNext(value.item, value.sequenceId);
        }

        private sealed class Subscription : IDisposable
        {
            private readonly ReliableSubject<T> _parent;
            private IObserver<(long sequenceId, T item)> _observer;

            public Subscription(ReliableSubject<T> parent, IObserver<(long sequenceId, T item)> observer)
            {
                _parent = parent;
                _observer = observer;
            }

            public void Dispose()
            {
                var observer = Interlocked.Exchange(ref _observer, null);

                if (observer != null)
                {
                    _parent.Unsubscribe(observer);
                }
            }
        }

        private void Unsubscribe(IObserver<(long sequenceId, T item)> observer)
        {
            lock (_gate)
            {
                _observers.Remove(observer);
            }
        }
    }
}
