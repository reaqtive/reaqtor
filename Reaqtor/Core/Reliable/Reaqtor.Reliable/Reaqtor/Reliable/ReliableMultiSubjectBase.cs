// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1062 // Omitted null checks; will become non-nullable references going forward.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

using Reaqtive;

namespace Reaqtor.Reliable
{
    public abstract class ReliableMultiSubjectBase<T> : IReliableMultiSubject<T>
    {
        private Subscription[] _subscriptions = Array.Empty<Subscription>();
        private readonly object _subscriptionsLock = new();

        private List<T> _queue = new();

        private Exception _error;
        private bool _done;
        private bool _completeNotified;
        private long _lowWatermark;
        private int _queueIdx = 0;
        private long _disposed = 0;

        protected ReliableMultiSubjectBase()
        {
        }

        public abstract Uri Id { get; }

        public bool StateChanged => true; // TODO: Make this properly fine-grained.

        protected int SubscriptionCount
        {
            get { lock (_subscriptionsLock) { return _subscriptions.Length; } }
        }

        protected int QueueSize
        {
            get
            {
                lock (_queue)
                {
                    return _queue.Count - _queueIdx;
                }
            }
        }

        protected long LowWatermark
        {
            get { lock (_queue) { return _lowWatermark; } }
            set { lock (_queue) { _lowWatermark = value; } }
        }

        public IReliableObserver<T> CreateObserver()
        {
            if (Interlocked.Read(ref _disposed) != 0)
            {
                throw new ObjectDisposedException(Id.AbsoluteUri);
            }

            return new Observer(this);
        }

        public IReliableSubscription Subscribe(IReliableObserver<T> observer)
        {
            if (Interlocked.Read(ref _disposed) != 0)
            {
                throw new ObjectDisposedException(Id.AbsoluteUri);
            }

            lock (_subscriptionsLock)
            {
                var subscription = CreateNewSubscription(observer);

                var newSubscriptions = new Subscription[_subscriptions.Length + 1];
                Array.Copy(_subscriptions, 0, newSubscriptions, 0, _subscriptions.Length);
                newSubscriptions[_subscriptions.Length] = subscription;

                _subscriptions = newSubscriptions;

                return subscription;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (Interlocked.CompareExchange(ref _disposed, 1, 0) != 0)
                {
                    return;
                }

                DisposeCore();
            }
        }

        protected abstract void DisposeCore();

        protected virtual Subscription CreateNewSubscription(IReliableObserver<T> observer)
        {
            return new Subscription(this, observer, _lowWatermark - 1);
        }

        public virtual void Start()
        {
        }

        public virtual void LoadState(IOperatorStateReader reader, Version version)
        {
            _done = reader.Read<bool>();
            _completeNotified = reader.Read<bool>();
            _lowWatermark = reader.Read<long>();
            _queueIdx = reader.Read<int>();
            _queue = reader.Read<List<T>>();

            // CONSIDER: Instead of storing the whole queue, integrate this with the work done on Operator Local Storage.

            // TODO: Handle _done == true and _completeNotified == true.
        }

        public virtual void SaveState(IOperatorStateWriter writer, Version version)
        {
            writer.Write(_done);
            writer.Write(_completeNotified);
            writer.Write(_lowWatermark);
            writer.Write(_queueIdx);
            writer.Write(_queue);
        }

        public virtual void OnStateSaved()
        {
            // TODO: Implement this when making StateChanged no longer return true unconditionally.
        }

        protected virtual void SubscriptionStart(long sequenceId, Subscription subscription)
        {
            //
            // Sequence IDs are inclusive and start from 0. Start replays from the
            // given sequence ID (included).
            //

            lock (_queue)
            {
                if (sequenceId == -1)
                {
                    sequenceId = _queueIdx;
                }

                int queueIdx = (int)(sequenceId - _lowWatermark);
                Debug.Assert(queueIdx >= 0);

                if (queueIdx > _queue.Count)
                {
                    queueIdx = _queue.Count;
                }

                T[] items = new T[_queue.Count - queueIdx];
                _queue.CopyTo(queueIdx, items, 0, items.Length);

                // First start the subscription to make sure it will get the replayed events.
                // This has to be done under the lock to make sure no new events get to 
                // the queue and to the subscription before the replayed events.
                subscription.StartCompleted();

                // TODO: Take this out of the lock - maybe keep a separate queueIdx (cursor) per subscription.
                NotifySubscription(subscription, items, _done, _error, _lowWatermark, queueIdx);
            }
        }

        protected virtual void SubscriptionAcknowledgeRange(long sequenceId, Subscription subscription)
        {
            long newLowWatermark;
            lock (_subscriptionsLock)
            {
                newLowWatermark = 1 + _subscriptions.Min(s => s.LastAck);
            }

            lock (_queue)
            {
                Debug.Assert(newLowWatermark >= _lowWatermark);
                if (newLowWatermark != _lowWatermark)
                {
                    int count = (int)(newLowWatermark - _lowWatermark);
                    _queue.RemoveRange(0, count);
                    _queueIdx -= count;
                    _lowWatermark = newLowWatermark;
                }
            }
        }

        protected virtual void SubscriptionDispose(Subscription subscription)
        {
            lock (_subscriptionsLock)
            {
                int idx;
                for (idx = 0; idx < _subscriptions.Length; ++idx)
                {
                    if (subscription == _subscriptions[idx])
                    {
                        break;
                    }
                }

                if (idx >= _subscriptions.Length)
                {
                    return;
                }

                var newSubscriptions = new Subscription[_subscriptions.Length - 1];

                if (idx > 0)
                {
                    Array.Copy(_subscriptions, 0, newSubscriptions, 0, idx);
                }

                if (idx < (_subscriptions.Length - 1))
                {
                    Array.Copy(_subscriptions, idx + 1, newSubscriptions, idx, _subscriptions.Length - idx - 1);
                }

                _subscriptions = newSubscriptions;
            }
        }

        protected void DropAllSubscriptions()
        {
            lock (_subscriptionsLock)
            {
                _subscriptions = Array.Empty<Subscription>();

                // TODO: Old subscriptions must be deleted from the QE *after* they are dropped from the EdgeOutput.
            }
        }

        protected virtual void OnNext(T item, long sequenceId)
        {
            lock (_queue)
            {
                _queue.Add(item);
            }
        }

#pragma warning disable CA1716 // Identifiers should not match keywords. (Using error from `IObserver<T>.OnError(Exception error)`.)

        protected virtual void OnError(Exception error)
        {
            lock (_queue)
            {
                _error = error;
            }
        }

#pragma warning restore CA1716

        protected virtual void OnCompleted()
        {
            lock (_queue)
            {
                _done = true;
            }
        }

        protected void NotifySubscriptions(int batchSize = 0)
        {
            long lowWatermark;
            bool done;
            int queueIdx;
            Exception error;

            T[] items;

            lock (_queue)
            {
                if (_queueIdx == _queue.Count && (_completeNotified || (!_done && _error == null)))
                {
                    return;
                }

                lowWatermark = _lowWatermark;
                done = _done;
                error = _error;

                queueIdx = _queueIdx;

                int count = _queue.Count - queueIdx;
                if (batchSize > 0 && batchSize < count)
                {
                    count = batchSize;
                    done = false;
                    error = null;
                }

                if (done || error != null)
                {
                    _completeNotified = true;
                }

                if (ShouldBufferedEventsBeDropped())
                {
                    // Dropping all events.
                    _queueIdx = 0;
                    _lowWatermark += _queue.Count;
                    _queue.Clear();
                    return;
                }

                items = new T[count];
                _queue.CopyTo(queueIdx, items, 0, count);
                _queueIdx += count;
            }

            Subscription[] subscriptions = _subscriptions;

            foreach (var subscription in subscriptions)
            {
                NotifySubscription(subscription, items, done, error, lowWatermark, queueIdx);
            }
        }

        protected virtual bool ShouldBufferedEventsBeDropped() => false;

        private static void NotifySubscription(Subscription subscription, T[] items, bool done, Exception error, long lowWatermark, int queueIdx)
        {
            for (int i = 0; i < items.Length; ++i)
            {
                long id = lowWatermark + queueIdx + i;
                subscription.OnNext(items[i], id);
            }

            if (error != null)
            {
                subscription.OnError(error);
            }
            else if (done)
            {
                subscription.OnCompleted();
            }
        }

        private class Observer : IReliableObserver<T>
        {
            private readonly ReliableMultiSubjectBase<T> _parent;
            private long _lastSequenceId = -1;

            public Observer(ReliableMultiSubjectBase<T> parent)
            {
                _parent = parent;
            }

            public Uri ResubscribeUri => _parent.Id;

            public void OnNext(T item, long sequenceId)
            {
                Debug.Assert(Interlocked.Exchange(ref _lastSequenceId, sequenceId) <= sequenceId);
                _parent.OnNext(item, sequenceId);
            }

            public void OnStarted()
            {
                throw new NotImplementedException();
            }

            public void OnError(Exception error) => _parent.OnError(error);

            public void OnCompleted() => _parent.OnCompleted();
        }

        protected sealed class Subscription : IReliableSubscription
        {
            private readonly ReliableMultiSubjectBase<T> _parent;
            private readonly IReliableObserver<T> _observer;
            private long _lastAck;
            private long _disposed = 1;

            public Subscription(ReliableMultiSubjectBase<T> parent, IReliableObserver<T> observer, long lastAck)
            {
                _parent = parent;
                _observer = observer;
                _lastAck = lastAck;
            }

            public long LastAck => Interlocked.Read(ref _lastAck);

            public Uri ResubscribeUri => _parent.Id;

            public void Start(long sequenceId)
            {
                //
                // Sequence IDs are inclusive and start from 0. Start replays from the
                // given sequence ID (included).
                //

                Debug.Assert(sequenceId == -1 || sequenceId > LastAck);

                _parent.SubscriptionStart(sequenceId, this);
            }

            public void StartCompleted() => Interlocked.Exchange(ref _disposed, 0);

            public void AcknowledgeRange(long sequenceId)
            {
                if (Interlocked.Read(ref _disposed) != 0)
                {
                    return;
                }

                long lastAck = Interlocked.Exchange(ref _lastAck, sequenceId);

                // Allowing for double ACK of the same sequence ID.
                Debug.Assert(lastAck <= sequenceId);

                _parent.SubscriptionAcknowledgeRange(sequenceId, this);
            }

            public void Dispose()
            {
                if (Interlocked.CompareExchange(ref _disposed, 1, 0) != 0)
                {
                    return;
                }

                _parent.SubscriptionDispose(this);
            }

            public void OnNext(T item, long sequenceId)
            {
                if (Interlocked.Read(ref _disposed) != 0)
                {
                    return;
                }

                _observer.OnNext(item, sequenceId);
            }

            public void OnError(Exception error)
            {
                if (Interlocked.Read(ref _disposed) != 0)
                {
                    return;
                }

                _observer.OnError(error);
                Dispose();
            }

            public void OnCompleted()
            {
                if (Interlocked.Read(ref _disposed) != 0)
                {
                    return;
                }

                _observer.OnCompleted();
                Dispose();
            }

            public void Accept(ISubscriptionVisitor visitor)
            {
            }
        }
    }
}
