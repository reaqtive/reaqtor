// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;

using Reaqtive;

namespace Reaqtor.QueryEngine.Mocks
{
    /// <summary>
    /// Provides a set of methods to manage <see cref="MockObservable{T}"/> instances.
    /// </summary>
    public static class MockObservable
    {
        private static readonly Dictionary<string, object> _observables = new();

        /// <summary>
        /// Gets or creates an observable with the specified identifier.
        /// </summary>
        /// <typeparam name="T">The type of the elements.</typeparam>
        /// <param name="id">Identifier for the observable.</param>
        /// <returns>An existing observable instance if an entry with the specified identifier already exists; otherwise, a new observable instance.</returns>
        public static IObservable<T> CreateObservable<T>(string id)
        {
            lock (_observables)
            {
                if (_observables.ContainsKey(id))
                {
                    return _observables[id] as IObservable<T>;
                }

                var obs = new MockObservable<T>(id);
                _observables.Add(id, obs);
                return obs;
            }
        }

        /// <summary>
        /// Clears all of the observable instances.
        /// </summary>
        public static void Clear()
        {
            lock (_observables)
            {
                _observables.Clear();
            }
        }

        /// <summary>
        /// Gets an observable with the specified identifier, if it exists.
        /// </summary>
        /// <typeparam name="T">The type of the elements.</typeparam>
        /// <param name="id">The identifier of the observable to retrieve.</param>
        /// <returns>An observable instance, if found; otherwise, null.</returns>
        public static MockObservable<T> Get<T>(string id)
        {
            lock (_observables)
            {
                if (!_observables.ContainsKey(id))
                {
                    return null;
                }

                return (MockObservable<T>)_observables[id];
            }
        }
    }

    public class MockObservable<T> : IObservable<T>, ISubscribable<T>, IObserver<T>
    {
        private readonly List<Subscription> _subscriptions = new();
        private readonly object _lock = new();

        public MockObservable(string id) => Id = id;

        public string Id { get; }

        public IEnumerable<IKnownResource> Subscriptions => _subscriptions;

        IDisposable IObservable<T>.Subscribe(IObserver<T> observer) => Subscribe(observer);

        public ISubscription Subscribe(IObserver<T> observer)
        {
            var s = new Subscription(this, observer);

            lock (_lock)
            {
                _subscriptions.Add(s);
            }

            return s;
        }

        public int SubscriptionCount
        {
            get { lock (_lock) { return _subscriptions.Count; } }
        }

        public void DropAllSubscriptions()
        {
            lock (_lock)
            {
                _subscriptions.Clear();
            }
        }

        public void OnCompleted()
        {
            List<Subscription> subs;

            lock (_lock)
            {
                subs = new List<Subscription>(_subscriptions);
            }

            subs.ForEach(s => s.OnCompleted());
        }

        public void OnError(Exception error)
        {
            List<Subscription> subs;

            lock (_lock)
            {
                subs = new List<Subscription>(_subscriptions);
            }

            subs.ForEach(s => s.OnError(error));
        }

        public void OnNext(T value)
        {
            List<Subscription> subs;

            lock (_lock)
            {
                subs = new List<Subscription>(_subscriptions);
            }

            subs.ForEach(s => s.OnNext(value));
        }

        private class Subscription : ISubscription, IObserver<T>, IOperator, IKnownResource
        {
            private readonly MockObservable<T> _parent;
            private readonly IObserver<T> _sink;

            public Subscription(MockObservable<T> obs, IObserver<T> sink)
            {
                _parent = obs;
                _sink = sink;
            }

            public Uri Uri { get; private set; }

            public void Accept(ISubscriptionVisitor visitor) => visitor.Visit(this);

            public void Dispose()
            {
                lock (_parent._lock)
                {
                    _parent._subscriptions.Remove(this);
                }
            }

            public void OnCompleted() => _sink.OnCompleted();

            public void OnError(Exception error) => _sink.OnError(error);

            public void OnNext(T value) => _sink.OnNext(value);

            public IEnumerable<ISubscription> Inputs
            {
                get { yield break; }
            }

            public void Subscribe() { }

            public void SetContext(IOperatorContext context)
            {
                Uri = context.InstanceId;
            }

            public void Start() { }
        }
    }
}
