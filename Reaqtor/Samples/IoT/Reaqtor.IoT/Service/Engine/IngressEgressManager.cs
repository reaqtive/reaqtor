// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using Reaqtive;

using Reaqtor.Reliable;

namespace Reaqtor.IoT
{
    //
    // Mimics the world outside the query engine with event streams that can be consumed from or produced to.
    // In reality, this is either some environment receiving events, or an external system such as EventHub.
    //
    // The main thing to notice here is that we speak IReliable* interfaces at this boundary. These interfaces
    // differ from the corresponding I* interfaces in the presence of sequence IDs to refer to events, e.g.
    //
    //   OnNext(long sequenceId, T value);  // receive an event with the specified sequenceId
    //   Start(long sequenceId);            // replay events > sequenceId
    //   AcknowledgeRange(long sequenceId); // acknowledge having processed events <= sequenceId
    //
    // Proxies are installed in the query engine to communicate with this ingress/egress manager in order to
    // receive/send events (see IngressObservable<T> and EgressObserver<T>).
    //

    public sealed class IngressEgressManager
    {
        private readonly Dictionary<string, object> _subjects = new();

        public ReliableSubject<T> CreateSubject<T>(string name)
        {
            //
            // Used by the "outside world". Compare to creating a topic in EventHub.
            //

            var subject = new ReliableSubject<T>();

            lock (_subjects)
            {
                _subjects.Add(name, subject);
            }

            return subject;
        }

        internal IReliableObserver<T> GetObserver<T>(string name)
        {
            //
            // Used by EgressObserver<T> instances in the query engine.
            //

            lock (_subjects)
            {
                return new ReliableObserver<T>((ReliableSubject<T>)_subjects[name]);
            }
        }

        internal IReliableObservable<T> GetObservable<T>(string name)
        {
            //
            // Used by IngressObservable<T> instances in the query engine.
            //

            lock (_subjects)
            {
                return new ReliableObservable<T>((ReliableSubject<T>)_subjects[name]);
            }
        }

        private sealed class ReliableObserver<T> : IReliableObserver<T>
        {
            private readonly ReliableSubject<T> _subject;

            public ReliableObserver(ReliableSubject<T> subject) => _subject = subject;

            public Uri ResubscribeUri => throw new NotImplementedException();

            public void OnCompleted() => _subject.OnCompleted();

            public void OnError(Exception error) => _subject.OnError(error);

            public void OnNext(T item, long sequenceId) => _subject.OnNext((sequenceId, item));

            public void OnStarted() { }
        }

        private sealed class ReliableObservable<T> : IReliableObservable<T>
        {
            private readonly ReliableSubject<T> _subject;

            public ReliableObservable(ReliableSubject<T> subject) => _subject = subject;

            public IReliableSubscription Subscribe(IReliableObserver<T> observer) => new Subscription(_subject, observer);

            private sealed class Subscription : IReliableSubscription
            {
                private readonly ReliableSubject<T> _subject;
                private readonly IReliableObserver<T> _observer;
                private IDisposable _subscription;

                public Subscription(ReliableSubject<T> subject, IReliableObserver<T> observer)
                {
                    _subject = subject;
                    _observer = observer;
                }

                public Uri ResubscribeUri => throw new NotImplementedException("Used for engine-to-engine communication; N/A here.");

                public void Accept(ISubscriptionVisitor visitor) => throw new NotImplementedException("Used for engine-to-engine communication; N/A here.");

                public void AcknowledgeRange(long sequenceId)
                {
                    // NB: Could wire up to prune history.
                }

                public void Dispose() => _subscription?.Dispose();

                public void Start(long sequenceId) => _subscription = _subject.Subscribe(_observer, sequenceId);
            }
        }
    }

    public sealed class ReliableSubject<T> : IObservable<(long sequenceId, T item)>, IObserver<(long sequenceId, T item)>
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

                if (sequenceId != null)
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
