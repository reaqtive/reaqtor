// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Threading;

using Reaqtive;

using Reaqtor.Reactive;
using Reaqtor.Reliable;

namespace Reaqtor.QueryEngine
{
    /// <summary>
    /// Subject that supports sealing through <see cref="ISealable"/>. Used by operators like GroupBy and Window.
    /// </summary>
    /// <typeparam name="T">Type of the events flowing through the subject.</typeparam>
    /// <remarks>
    /// Sealing is used to limit the number of subscriptions on a subject after its creation. It effectively ensures that
    /// the subject is surrounded by a static graph, which simplifies checkpointing.
    ///
    /// For example, an operator like Window does seal the subject after it emits it via OnNext. This requires the downstream
    /// consumer to immediately subscribe to it (e.g. after applying some more operators on it, feeding it into SelectMany to
    /// flatten the query). Once sealed, we can simply count the number of subscriptions and decide to delete the subject when
    /// the count drops to 0. This is done in a self-destructive manner, see <see cref="DeleteIfEmpty"/>.
    /// </remarks>
    internal class InnerSubject<T> : IReliableMultiSubject<T>, IMultiSubject<T>, IStatefulOperator, ISealable
    {
        private static readonly IObserver<T> s_empty = new NopObserver<T>(); // Note: don't use NopObserver<T>.Instance which is public!
        private static readonly IObserver<T> s_disposed = new NopObserver<T>(); // Note: don't use NopObserver<T>.Instance which is public!

        private IHostedOperatorContext _context;
        private bool _sealed;

        private int _subscriptionCount;
        private int _remainingSubscriptionsToRecover;
        private bool _wasSealed;

        private readonly object _gate = new();
        private IObserver<T> _observer = s_empty;

        public InnerSubject()
        {
        }

        protected bool Recovered { get; private set; }

        public IObserver<T> CreateObserver()
        {
            // No sealing here; the stream-producing higher-order operator needs to
            // be able to create an observer upon recovery.

            // TODO: specialized derived class to enforce single observer behavior for windows etc.
            return new Observer(this);
        }

        private sealed class Observer : IObserver<T>
        {
            private InnerSubject<T> _subject;

            public Observer(InnerSubject<T> subject) => _subject = subject;

            public void OnCompleted()
            {
                var subject = Interlocked.Exchange(ref _subject, null);
                if (subject != null)
                {
                    lock (subject._gate)
                    {
                        subject._observer.OnCompleted();
                    }
                }
            }

            public void OnError(Exception error)
            {
                var subject = Interlocked.Exchange(ref _subject, null);
                if (subject != null)
                {
                    lock (subject._gate)
                    {
                        subject._observer.OnError(error);
                    }
                }
            }

            public void OnNext(T value)
            {
                var subject = Volatile.Read(ref _subject);
                if (subject != null)
                {
                    lock (subject._gate)
                    {
                        subject._observer.OnNext(value);
                    }
                }
            }
        }

        public ISubscription Subscribe(IObserver<T> observer)
        {
            CheckSealed();

            return new Subscription(this, observer);
        }

        protected virtual void OnSubscriptionCreated()
        {
            // Operators like StartWith can leave a time gap due to internal
            // scheduling. Let's be conservative and throw even in these cases.
            // We need this anyway to be able to capture the refcount by means
            // of the ISealable technique; we can't tolerate later arrivals to
            // the party.
            CheckSealed();

            // During recovery only...
            if (Recovered)
            {
                // ...if we have more to recover...
                if (_remainingSubscriptionsToRecover > 0)
                {
                    // ...subtract the remaining number...
                    var rem = --_remainingSubscriptionsToRecover;

                    // ...keep count of the number of subscriptions but don't touch the dirty state...
                    _subscriptionCount++;

                    // ...if we were checkpointed in a sealed state and this was the last one to recover...
                    if (_wasSealed && rem == 0)
                    {
                        // ...then seal again.
                        _sealed = true;
                    }

                    return;
                }
            }

            _subscriptionCount++;
            StateChanged = true;
        }

        protected virtual void OnSubscriptionDisposed()
        {
            _subscriptionCount--;
            StateChanged = true;
        }

        IDisposable IObservable<T>.Subscribe(IObserver<T> observer) => Subscribe(observer);

        private void SubscribeCore(IObserver<T> observer)
        {
            lock (_gate)
            {
                OnSubscriptionCreated();

                if (_observer == s_disposed)
                {
                    throw new ObjectDisposedException("this");
                }
                else if (_observer == s_empty)
                {
                    _observer = observer;
                }
                else
                {
                    if (_observer is MultiObserver<T> m)
                    {
                        var observers = new IObserver<T>[m._observers.Length + 1];
                        Array.Copy(m._observers, observers, m._observers.Length);
                        observers[m._observers.Length] = observer;
                        _observer = new MultiObserver<T>(observers);
                    }
                    else
                    {
                        var observers = new IObserver<T>[] { _observer, observer };
                        _observer = new MultiObserver<T>(observers);
                    }
                }
            }
        }

        private void UnsubscribeCore(IObserver<T> observer)
        {
            lock (_gate)
            {
                if (_observer != s_empty)
                {
                    if (_observer is MultiObserver<T> m)
                    {
                        var n = m._observers.Length;

                        var i = Array.LastIndexOf(m._observers, observer);
                        if (i >= 0)
                        {
                            if (n == 2)
                            {
                                _observer = m._observers[1 ^ i]; // apparently the ^ operator is deemed esoteric, so this comment says it's not: 1 -> 0, 0 -> 1
                            }
                            else
                            {
                                var observers = new IObserver<T>[n - 1];

                                if (i > 0)
                                {
                                    Array.Copy(m._observers, 0, observers, 0, i);
                                }

                                if (i + 1 < m._observers.Length)
                                {
                                    Array.Copy(m._observers, i + 1, observers, i, n - i - 1);
                                }

                                _observer = new MultiObserver<T>(observers);
                            }

                            OnSubscriptionDisposed();
                        }
                    }
                    else
                    {
                        if (_observer == observer)
                        {
                            _observer = s_empty;

                            OnSubscriptionDisposed();
                        }
                    }
                }

                DeleteIfEmpty();
            }
        }

        private sealed class Subscription : ISubscription, IOperator
        {
            private InnerSubject<T> _subject;
            private IObserver<T> _observer;
            private int _started;

            public Subscription(InnerSubject<T> subject, IObserver<T> observer)
            {
                _subject = subject;
                _observer = observer;
            }

            public void Accept(ISubscriptionVisitor visitor) => visitor.Visit(this);

            public IEnumerable<ISubscription> Inputs => Array.Empty<ISubscription>();

            public void Subscribe() { }

            public void SetContext(IOperatorContext context)
            {
            }

            public void Start()
            {
                var observer = _observer;
                if (observer != null && Interlocked.CompareExchange(ref _started, 1, 0) == 0)
                {
                    _subject.SubscribeCore(observer);
                }
            }

            public void Dispose()
            {
                var observer = Interlocked.Exchange(ref _observer, null);

                if (observer != null)
                {
                    _subject.UnsubscribeCore(observer);
                    _subject = null;
                }
            }
        }

        public void Dispose()
        {
            lock (_gate)
            {
                _observer = s_disposed;
            }
        }

        public void Seal()
        {
            lock (_gate)
            {
                if (!_sealed)
                {
                    _sealed = true;
                    StateChanged = true;

                    DeleteIfEmpty();
                }
            }
        }

        private void CheckSealed()
        {
            if (Volatile.Read(ref _sealed))
            {
                throw new InvalidOperationException("The subject is sealed and cannot accept new subscriptions. This limitation may be lifted in a future release.");
            }
        }

        private void DeleteIfEmpty()
        {
            if (_subscriptionCount == 0 && _sealed)
            {
                _context.ReactiveService.GetStream<T, T>(_context.InstanceId).Dispose();
                OnDeleted();
            }
        }

        protected virtual void OnDeleted()
        {
        }

        public IEnumerable<ISubscription> Inputs => Array.Empty<ISubscription>();

        public virtual void Subscribe() { }

        public virtual void SetContext(IOperatorContext context) => _context = (IHostedOperatorContext)context;

        public virtual void Start() { }

        #region State

        private StateChangedManager _stateful;

        public void LoadState(IOperatorStateReader reader, Version version)
        {
            _wasSealed = reader.Read<bool>();
            _remainingSubscriptionsToRecover = reader.Read<int>();

            if (_wasSealed && _remainingSubscriptionsToRecover == 0)
            {
                _sealed = true;
            }

            Recovered = true;

            _stateful.LoadState();
        }

        public void SaveState(IOperatorStateWriter writer, Version version)
        {
            writer.Write<bool>(_wasSealed | _sealed);
            writer.Write<int>(_subscriptionCount);

            _stateful.SaveState();
        }

        public void OnStateSaved() => _stateful.OnStateSaved();

        public bool StateChanged
        {
            get => _stateful.StateChanged;

            protected set => _stateful.StateChanged = value;
        }

        public string Name => "rce:Subject+Inner";

        public Version Version => Versioning.v1;

        #endregion

        IReliableObserver<T> IReliableMultiSubject<T, T>.CreateObserver()
        {
            // Needed to pass binder scrutiny; to be revisited later.
            throw new NotSupportedException();
        }

        public IReliableSubscription Subscribe(IReliableObserver<T> observer)
        {
            // Needed to pass binder scrutiny; to be revisited later.
            throw new NotSupportedException();
        }
    }
}
