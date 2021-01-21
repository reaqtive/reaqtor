// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Threading;

using Reaqtor.Remoting.Protocol;

namespace Reaqtor.Remoting.Platform
{
    public class MessageRouter
    {
        public const string ContextHandle = "MessageRouter";

        private static readonly Lazy<MessageRouter> s_instance = new(() => new MessageRouter(), LazyThreadSafetyMode.ExecutionAndPublication);

        private readonly object _gate = new();
        private readonly IDictionary<Uri, Subscription> _routes;
        private readonly IReactiveMessagingConnection _messaging;

        public MessageRouter()
        {
            _routes = new Dictionary<Uri, Subscription>();
        }

        public MessageRouter(IReactiveMessagingConnection messaging)
        {
            _messaging = messaging ?? throw new ArgumentNullException(nameof(messaging));
            _routes = new Dictionary<Uri, Subscription>();
        }

        public IReactiveMessagingConnection MessagingConnection
        {
            get
            {
                var messaging = _messaging ?? ServiceInstanceHelpers.GetMessagingServiceInstance();
                if (messaging == null)
                {
                    throw new InvalidOperationException("Could not connect to messaging service.");
                }
                return messaging;
            }
        }

        public static MessageRouter Instance => s_instance.Value;

        public IDisposable Subscribe(Uri topic, IObserver<byte[]> observer)
        {
            lock (_gate)
            {
                if (!_routes.TryGetValue(topic, out var topicSubscription))
                {
                    topicSubscription = new Subscription(this, topic);
                    _routes[topic] = topicSubscription;
                }

                topicSubscription.Add(observer);
            }

            return new Disposable(() =>
            {
                lock (_gate)
                {
                    var topicSubscription = _routes[topic];
                    if (topicSubscription.Remove(observer))
                    {
                        _routes.Remove(topic);
                    }
                }
            });
        }

        public void Publish(Uri topic, INotification<byte[]> data)
        {
            MessagingConnection.Publish(topic.ToCanonicalString(), data);
        }

        private sealed class Subscription
        {
            private readonly MessageRouter _router;
            private readonly string _topic;
            private readonly IList<IObserver<byte[]>> _observers;
            private IDisposable unsubscribe;

            public Subscription(MessageRouter router, Uri topic)
            {
                _router = router;
                _topic = topic.ToCanonicalString();
                _observers = new List<IObserver<byte[]>>();
            }

            public void Add(IObserver<byte[]> observer)
            {
                var started = false;

                lock (_observers)
                {
                    _observers.Add(observer);

                    started = _observers.Count == 1;
                }

                if (started)
                {
                    unsubscribe = _router.MessagingConnection.Subscribe(_topic, new Receiver(this).Receive);
                }
            }

            public bool Remove(IObserver<byte[]> observer)
            {
                var stopped = false;

                lock (_observers)
                {
                    _observers.Remove(observer);

                    stopped = _observers.Count == 0;
                }

                if (stopped && unsubscribe != null)
                {
                    unsubscribe.Dispose();
                    unsubscribe = null;
                }

                return stopped;
            }

            private void Receive(INotification<byte[]> data)
            {
                switch (data.Kind)
                {
                    case NotificationKind.OnCompleted:
                        lock (_observers)
                        {
                            foreach (var observer in _observers)
                            {
                                observer.OnCompleted();
                            }
                        }
                        break;
                    case NotificationKind.OnError:
                        lock (_observers)
                        {
                            foreach (var observer in _observers)
                            {
                                observer.OnError(data.Exception);
                            }
                        }
                        break;
                    case NotificationKind.OnNext:
                        lock (_observers)
                        {
                            foreach (var observer in _observers)
                            {
                                observer.OnNext(data.Value);
                            }
                        }
                        break;
                    default:
                        throw new InvalidOperationException("Invalid data received.");
                }
            }

            private sealed class Receiver : MarshalByRefObject
            {
                private readonly Subscription _parent;

                public Receiver(Subscription parent)
                {
                    _parent = parent;
                }

                public void Receive(INotification<byte[]> data)
                {
                    _parent.Receive(data);
                }

                public override object InitializeLifetimeService() => null;
            }
        }

        private sealed class Disposable : IDisposable
        {
            private Action _action;

            public Disposable(Action action)
            {
                _action = action;
            }

            public void Dispose()
            {
                Interlocked.Exchange(ref _action, new Action(() => { }))();
            }
        }
    }
}
