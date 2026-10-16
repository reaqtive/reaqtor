// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Threading;

using Reaqtor.Remoting.Protocol;

namespace Reaqtor.Remoting.Platform;

//
// ADAPTATION (plan §3.6): the archived MessageRouter resolved its IReactiveMessagingConnection via
// ServiceInstanceHelpers.GetMessagingServiceInstance(), an AppDomain-keyed data slot populated by
// ReactiveEnvironmentBase.SetMessagingServiceInstance(AppDomain.CurrentDomain, ...). There is no AppDomain
// (nor any cross-domain data slot) on net10.0. The messaging connection is now supplied purely by
// constructor/DI injection. The static Instance singleton is retained for source compatibility with the
// single-argument FirehoseObserver(Uri) ctor, but its MessagingConnection getter now fails fast with a clear
// error when no connection was injected instead of silently reading an AppDomain slot. The MarshalByRefObject
// Receiver inner class is dropped (no remoting); Receive is invoked directly.
//
// A deeper redesign (eliminating the parameterless/Instance path entirely in favour of always-injected
// wiring, per the single-shared-MessagingConnection invariant in §3.6) is a follow-up handled when the
// InMemory.Core oracle and the gRPC hosts are wired up (steps 0a.4+).
//
public class MessageRouter
{
    public const string ContextHandle = "MessageRouter";

    private static readonly Lock s_instanceLock = new();
    private readonly Lock _gate = new();
    private readonly IDictionary<Uri, Subscription> _routes;

    public MessageRouter()
    {
        _routes = new Dictionary<Uri, Subscription>();
    }

    public MessageRouter(IReactiveMessagingConnection messaging)
    {
        MessagingConnection = messaging ?? throw new ArgumentNullException(nameof(messaging));
        _routes = new Dictionary<Uri, Subscription>();
    }

    public IReactiveMessagingConnection MessagingConnection
    {
        get
        {
            var messaging = field;
            if (messaging == null)
            {
                throw new InvalidOperationException("Could not connect to messaging service. A MessageRouter that is used to publish or subscribe must be constructed with an IReactiveMessagingConnection (it is now DI-injected; the AppDomain-keyed lookup was removed on net10.0 - see plan §3.6).");
            }
            return messaging;
        }
    }

    //
    // §3.6 resolution: the engine-side FirehoseObserver(Uri) (defined by CoreDeployable) resolves its router via
    // this process-wide Instance. The composition root (the gRPC EngineHost, or the InMemory oracle) calls
    // Initialize with the single shared IReactiveMessagingConnection BEFORE any firehose is instantiated, so the
    // engine publishes results to the real broker (which the gRPC Messaging service / in-proc subscribers read).
    // Without Initialize, Instance is a connection-less router whose Publish fails fast (no silent drop).
    //
    /// <summary>
    /// Initializes the process-wide <see cref="Instance"/> to publish to the given broker connection.
    /// <para>
    /// CONTRACT: this sets PROCESS-WIDE mutable state and must be called once at the composition root BEFORE any
    /// firehose is instantiated (a <see cref="Firehose.FirehoseObserver"/> captures <see cref="Instance"/> at
    /// construction). The design assumes ONE platform per process; the only sanctioned second call is the QE host's
    /// deliberate broker-redirect (QueryEvaluatorHost/Program.cs). In-process tests that construct a platform and
    /// rely on this static must run SEQUENTIALLY — the sample test assemblies are marked
    /// <c>[assembly: DoNotParallelize]</c> — so a later <see cref="Initialize"/> cannot clobber the broker a
    /// still-live firehose captured. A fuller fix injects the router via the two-arg <c>FirehoseObserver</c> ctor
    /// on every engine path (follow-up).
    /// </para>
    /// </summary>
    public static void Initialize(IReactiveMessagingConnection messaging)
    {
        ArgumentNullException.ThrowIfNull(messaging);

        lock (s_instanceLock)
        {
            Instance = new MessageRouter(messaging);
        }
    }

    /// <summary>The process-wide router used by the engine-side firehose (see <see cref="Initialize"/>).</summary>
    public static MessageRouter Instance
    {
        get
        {
            lock (s_instanceLock)
            {
                field ??= new MessageRouter();
                return field;
            }
        }

        private set;
    }

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
            _observers = [];
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
                unsubscribe = _router.MessagingConnection.Subscribe(_topic, Receive);
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
