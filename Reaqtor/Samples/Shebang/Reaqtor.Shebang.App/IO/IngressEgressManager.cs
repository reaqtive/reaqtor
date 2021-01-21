// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;

using Reaqtive;

using Reaqtor.Reliable;
using Reaqtor.Shebang.Service;

namespace Reaqtor.Shebang.App
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

    public sealed class IngressEgressManager : IIngressEgressManager
    {
        private readonly Dictionary<string, object> _subjects = new();

        public IReliableSubject<T> CreateSubject<T>(string name)
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

        public IReliableSubject<T> GetSubject<T>(string name)
        {
            lock (_subjects)
            {
                return (ReliableSubject<T>)_subjects[name];
            }
        }

        IReliableObserver<T> IIngressEgressManager.GetObserver<T>(string name)
        {
            //
            // Used by EgressObserver<T> instances in the query engine.
            //

            lock (_subjects)
            {
                return new ReliableObserver<T>((ReliableSubject<T>)_subjects[name]);
            }
        }

        IReliableObservable<T> IIngressEgressManager.GetObservable<T>(string name)
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
}
