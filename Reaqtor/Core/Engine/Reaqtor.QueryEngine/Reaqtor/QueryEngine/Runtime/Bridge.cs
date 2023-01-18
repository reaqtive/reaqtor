// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Memory;
using System.Runtime.ExceptionServices;
using System.Threading;

using Reaqtive;

using Reaqtor.Reactive;
using Reaqtor.Reliable;

namespace Reaqtor.QueryEngine
{
    //
    // This bridge does two things right now:
    // 1. Bridge across two subscriptions to decouple lifetimes (and recovery sequence)
    // 2. Manage the upstream subscription lifetime
    //
    // We should consider separating the functionality, although not immediately necessary.
    //

    /// <summary>
    /// Bridge subject between a subscription to an upstream observable and a downstream (single) observer.
    /// Used to decouple lifetimes and state management, e.g. in the case of higher-order operators.
    /// </summary>
    /// <typeparam name="T">The type of the elements flowing through the bridge.</typeparam>
    /// <remarks>
    /// Bridges are instantiated through a stream factory identified as <see cref="QueryEngineConstants.BridgeUri"/>. This identifier is
    /// known (by means of loose coupling) to the Reaqtor library, e.g. in its `HigherOrderStatefulOperator` base class, in order
    /// to create bridges for higher order operators. A typical example is `SelectMany`, for example:
    ///
    /// xs.SelectMany(x => ys(x))
    ///
    /// For each invocation of the selector function, a new `ys(a)` observable is created, where `a` is the value of `x`. The query operator
    /// will turn around an create a bridge which consists of:
    ///
    /// * a bridge subject (an instance of this type, where T is substituted for the element type of `ys(a)`);
    /// * an observable definition for `ys(a)` - the "upstream observable";
    /// * a subscription between the upstream observable and the bridge subject - the "upstream subscription".
    ///
    /// The `SelectMany` operator then remembers the identifier for the "upstream subscription"; in case of its own disposal, it can clean
    /// up all associated bridges, which in turn clean up the upstream disposable and upstream subscription.
    ///
    /// While the bridge, upstream observable, and upstream subscription are persisted, the downstream subscription (between the bridge and
    /// an observer, e.g. inside `SelectMany` to merge the events from all inner sequences) is volatile. Upon recovery, the URI of the
    /// bridge is used to re-retrieve the subject and to re-establish a volatile subscription that causes events to flow between the bridge
    /// and the downstream operator, through a volatile observer instance. As such, the link from the parent operator to the bridge is
    /// not part of the peristed operator graph; all that's persisted in the bridge's URI (a different kind of "reference"). Also note that
    /// during recovery, subjects (and hence bridges) are recovered before subscriptions, so the recovery of `SelectMany` will find the
    /// recovered bridges for the inner subscriptions.
    ///
    /// By having separate bridges, the lifecycle of inner subscriptions gets properly decoupled, and changes to the state of inner
    /// subscriptions don't cause the parent subscription to be dirty (which would inflate the differential checkpoint size). It also limits
    /// the state of the parent operator (e.g. `SelectMany`) to keeping track of the URIs identifying the bridges. (Note this can be quite
    /// big still, and possible improvements include the use of "operator local storage").
    ///
    /// Bridges have a non-persisted queue for events to deal with race conditions when an inner subscription starts producing events
    /// before the (single) downstream observer arrives. This is inherit to the design of the system because subscriptions can be started
    /// in any order and concurrently during recovery. As such, it's possible for an inner subscription to recover before its parent
    /// subscription (e.g. the one containing the `SelectMany` operator) recovers. The queue is used to buffer events during that window.
    ///
    /// It's worth noting that the ordering of events across inner subscriptions being merged by an operator like `SelectMany` is not
    /// guaranteed to be conistent across recoveries, unless an ingress manager in hooked up to the engine that performs a sort of events
    /// across all streams (subject to some ordering policy) that feed into the engine. Such an ingress manager would only "open the flood
    /// gates" after an engine's recovery is done, which is also after having seen all the low watermarks requested by the subjects (and
    /// variants thereof, such as edges) that have been recovered and instantiated within the engine. This design is intentional to separate
    /// the concerns of time/ordering and event processing in a way similar to the scheduler abstraction in Rx (see LINQ to Traces and Tx
    /// for an implementation of a "time source" that applies the same principles in classic Rx).
    ///
    /// Bridge have historically also been used for different configurations where events are received from a reliable source (with sequence
    /// IDs), e.g. when inner subscriptions cross engine boundaries. To support this, a low watermark is persisted, such that replay can be
    /// requested.
    /// </remarks>
    internal sealed class Bridge<T> : IReliableMultiSubject<T>, IMultiSubject<T>, IStatefulOperator, IDependencyOperator
    {
        // Only needed for upstream subscription lifetime management
        private readonly IDiscardable<Expression> _source;
        private Uri _upstreamObservableUri;
        private Uri _upstreamSubscriptionUri;

        private IHostedOperatorContext _context;

        private Subscription _subscription;

        private List<T> _queue = new();
        private Exception _error;
        private bool _done;

        private bool _completeNotified;
        private long _lowWatermark = 0;

        private readonly object _lock = new();
        private int _disposed = 0;
        private StateChangedManager _stateful;

        public Bridge(IDiscardable<Expression> source)
        {
            _source = source;
        }

        public Uri Id => _context.InstanceId;

        public IEnumerable<Uri> Dependencies
        {
            get
            {
                if (Volatile.Read(ref _disposed) == 0)
                {
                    var upstreamObservableUri = _upstreamObservableUri;
                    if (upstreamObservableUri != null)
                    {
                        yield return upstreamObservableUri;
                    }

                    var upstreamSubscriptionUri = _upstreamSubscriptionUri;
                    if (upstreamSubscriptionUri != null)
                    {
                        yield return upstreamSubscriptionUri;
                    }
                }
            }
        }

        #region Observer

        public IReliableObserver<T> CreateObserver()
        {
            if (Volatile.Read(ref _disposed) != 0)
            {
                return ReliableSentinels<T>.Disposed;
            }

            return new Observer(this);
        }

        IObserver<T> IMultiSubject<T, T>.CreateObserver()
        {
            // This shouldn't be called anyways but implemented for completeness.
            return (IObserver<T>)CreateObserver();
        }

        private void OnNext(T item, long sequenceId)
        {
            _ = sequenceId;

            lock (_lock)
            {
                if (_queue == null)
                {
                    var subscription = Volatile.Read(ref _subscription);

                    subscription?.OnNext(item, _lowWatermark);

                    // Since the bridge has a single subscription and they are both checkpointed together
                    // we assume each OnNext is followed by Ack.
                    ++_lowWatermark;

                    StateChanged = true;
                }
                else
                {
                    // The queue is only here to allow producers to start before consumers upon recovery.
                    // It is not used after the subscription is created and doesn't have to be persisted during checkpointing.
                    _queue.Add(item);
                }
            }
        }

        private void OnError(Exception error)
        {
            lock (_lock)
            {
                _error = error;

                var subscription = Volatile.Read(ref _subscription);

                if (_queue == null && subscription != null)
                {
                    subscription.OnError(error);
                    _completeNotified = true;
                    StateChanged = true;
                }
            }
        }

        private void OnCompleted()
        {
            lock (_lock)
            {
                _done = true;

                var subscription = Volatile.Read(ref _subscription);

                if (_queue == null && subscription != null)
                {
                    subscription.OnCompleted();
                    _completeNotified = true;
                    StateChanged = true;
                }
            }
        }

        private sealed class Observer : IReliableObserver<T>, IObserver<T>
        {
            private readonly Bridge<T> _parent;
            private long _lastSequenceId = -1;

            public Observer(Bridge<T> parent)
            {
                _parent = parent;
            }

            public Uri ResubscribeUri => _parent.Id;

            public void OnNext(T item)
            {
                // This shouldn't be used for now.
                throw new NotImplementedException();
            }

            public void OnNext(T item, long sequenceId)
            {
                Debug.Assert(Interlocked.Exchange(ref _lastSequenceId, sequenceId) <= sequenceId);
                _parent.OnNext(item, sequenceId);
            }

            public void OnStarted() => throw new NotImplementedException();

            public void OnError(Exception error) => _parent.OnError(error);

            public void OnCompleted() => _parent.OnCompleted();
        }

        #endregion

        #region Subscribable

        public IReliableSubscription Subscribe(IReliableObserver<T> observer)
        {
            if (Volatile.Read(ref _disposed) != 0)
            {
                throw new ObjectDisposedException(Id.ToCanonicalString());
            }

            var subscription = new Subscription(this, observer, _lowWatermark - 1);

            if (Interlocked.CompareExchange(ref _subscription, subscription, null) != null)
            {
                throw new InvalidOperationException("Bridge subject allows only one subscription.");
            }

            return subscription;
        }

        ISubscription ISubscribable<T>.Subscribe(IObserver<T> observer) => new SingleAssignmentSubscription
        {
            //
            // NB: Use of SingleAssignmentSubscription hardens against the case where the subscription is disposed,
            //     avoiding subsequent visitor calls from making further calls (e.g. Start).
            //
            Subscription = Subscribe(new ObserverWrapper(observer))
        };

        IDisposable IObservable<T>.Subscribe(IObserver<T> observer) => ((IMultiSubject<T>)this).Subscribe(observer);

        private void SubscriptionStart(long sequenceId, Subscription subscription)
        {
            lock (_lock)
            {
                if (_queue != null)
                {
                    // Replay and dispose of the queue

                    Debug.Assert(subscription == _subscription);

                    if (sequenceId == -1)
                    {
                        sequenceId = 0;
                    }

                    int queueIdx = (int)(sequenceId - _lowWatermark);
                    Debug.Assert(queueIdx >= 0);

                    if (queueIdx > _queue.Count)
                    {
                        queueIdx = _queue.Count;
                    }

                    for (int i = queueIdx; i < _queue.Count; ++i)
                    {
                        subscription.OnNext(_queue[i], _lowWatermark + i);
                    }

                    // Since the bridge has a single subscription and they are both checkpointed together
                    // we assume each OnNext is followed by Ack.
                    _lowWatermark += (_queue.Count - queueIdx);

                    _queue = null;

                    if (_error != null)
                    {
                        subscription.OnError(_error);
                        _completeNotified = true;
                    }
                    else if (_done)
                    {
                        subscription.OnCompleted();
                        _completeNotified = true;
                    }

                    StateChanged = true;
                }

                // Eventually we can tease things apart but for now have the bridge manage the upstream subscription, too
                if (_upstreamSubscriptionUri == null)
                {
                    StartUpstreamSubscription(subscription.Context);
                }
            }
        }

        private void SubscriptionAcknowledgeRange(long sequenceId, Subscription subscription)
        {
            // Since the bridge has a single subscription and they are both checkpointed together
            // we assume each OnNext is followed by Ack.
            _ = this; // NB: Suppress CA1822
            _ = sequenceId;
            _ = subscription;
        }

        private void SubscriptionDispose(Subscription subscription)
        {
            var oldSubscription = Interlocked.CompareExchange(ref _subscription, null, subscription);

            if (oldSubscription == null)
            {
                return;
            }

            Debug.Assert(subscription == oldSubscription);

            Dispose();
        }

        private void StartUpstreamSubscription(IOperatorContext context)
        {
            // 1. Define observable from the _source observable expression
            // 2. Subscribe the bridge as observer

            _upstreamSubscriptionUri = new Uri(Id.ToCanonicalString() + "/upstream-subscription");

            StateChanged = true;

            var source = _source.Value;
            var traceSource = _context.TraceSource;

            var qbservableSource = default(IReactiveQbservable<T>);
            if (Version == Versioning.v1)
            {
                _upstreamObservableUri = new Uri(Id.ToCanonicalString() + "/upstream-observable");

                traceSource.Bridge_CreatingUpstreamObservable(_upstreamObservableUri, Id, source, e => e.ToTraceString());

                _context.ReactiveService.DefineObservable<T>(
                    _upstreamObservableUri,
                    _context.ReactiveService.Provider.CreateQbservable<T>(source),
                    null);

                qbservableSource = _context.ReactiveService.GetObservable<T>(_upstreamObservableUri);
            }
            else
            {
                qbservableSource = _context.ReactiveService.Provider.CreateQbservable<T>(
                    Expression.Invoke(Expression.Parameter(typeof(Func<ISubscribable<T>, IReactiveQbservable<T>>), Constants.IdentityFunctionUri), source));
            }

            // If user-supplied state is supported later, we should add it to the dictionary.
            var state = new Dictionary<string, object>(1)
            {
                { QueryEngineConstants.ParentUri, context.InstanceId.OriginalString },
            };

            traceSource.Bridge_CreatingUpstreamSubscription(_upstreamSubscriptionUri, Id);

            qbservableSource.Subscribe(_context.ReactiveService.GetObserver<T>(Id), _upstreamSubscriptionUri, state);
        }

        private sealed class Subscription : IReliableSubscription, ISubscription, IOperator, IDependencyOperator
        {
            private readonly Bridge<T> _parent;
            private readonly IReliableObserver<T> _observer;
            private long _lastAck;

            //
            // NB: We harden the code against invalid state transitions which can happen in concurrent scenarios. For example,
            //     consider a query like this:
            //
            //       Return<int>(42).SelectMany(x => f(x))
            //
            //     First, note that Return<T> uses the scheduler to emit an OnNext call, which can happen any time after
            //     the operator gets a call to Start.
            //
            //     Second, consider the ISubscription structure returned by Inputs for SelectMany. This contains two objects,
            //     one to represent the source subscription, and a CompositeSubscription for the inner subscriptions.
            //
            //     When SelectMany gets Start'ed, it first starts itself (which is a no-op), and then its inputs. The source
            //     subscription to Return<T> is first, and we end up with concurrency:
            //
            //       1. The current thread that's working on Start'ing the subscription.
            //       2. A scheduler thread that can will invoke OnNext, causing SelectMany to make an inner subscription.
            //
            //     In case 2 happens first, SelectMany's OnNext ends up creating an inner subscription consisting of a bridge
            //     and using a persisted subscription, which it will then also initialize and start, after publishing the
            //     newly created inner subscription into SelectMany's CompositeSubscription.
            //
            //     When 1 runs later, it also sees the newly created inner subscription, and it goes ahead calling Start on
            //     that one as well, so we end up with potential double-start.
            //
            //     In between these two steps, it's even possible for the bridge to get disposed, e.g. if Return also emits
            //     the OnCompleted notification, causing the bridge to get dropped.
            //
            //     To guard against invalid state transitions, we harden the bridge and persisted subscriptions to have a
            //     little state machine, shown below, guaranteeing that Start and Dispose cna only be called once.
            //

            private const int Created = 0;
            private const int Starting = 1;
            private const int Started = 2;
            private const int StartFailed = 3;
            private const int DisposeRequested = 4;
            private const int Disposed = 5;

            private int _state = Created;

            public Subscription(Bridge<T> parent, IReliableObserver<T> observer, long lastAck)
            {
                _parent = parent;
                _observer = observer;
                _lastAck = lastAck;
            }

            private bool IsStarted => Volatile.Read(ref _state) is Starting or Started;

            public IOperatorContext Context { get; private set; }

            public long LastAck => Interlocked.Read(ref _lastAck);

            public Uri ResubscribeUri => _parent.Id;

            public IEnumerable<Uri> Dependencies
            {
                get
                {
                    yield return _parent.Id;
                }
            }

            public void Subscribe() { }

            public void Start() => Start(LastAck + 1);

            public void Start(long sequenceId)
            {
                try { } // NB: Ensure no ThreadAbortException can leave a thread spinning in its check for Starting.
                finally
                {
                    Core();
                }

                void Core()
                {
                    var oldState = Interlocked.CompareExchange(ref _state, Starting, Created);

                    if (oldState == Starting)
                    {
                        SpinWait.SpinUntil(() => Volatile.Read(ref _state) != Starting);
                        return;
                    }

                    if (oldState != Created)
                    {
                        return;
                    }

                    var success = false;

                    try
                    {
                        StartCore(sequenceId);

                        success = true;
                    }
                    finally
                    {
                        oldState = Interlocked.CompareExchange(ref _state, success ? Started : StartFailed, Starting);

                        if (oldState == DisposeRequested)
                        {
                            Volatile.Write(ref _state, Disposed);

                            if (success)
                            {
                                DisposeCore();
                            }
                        }
                        else
                        {
                            Debug.Assert(oldState == Starting);
                        }
                    }
                }
            }

            private void StartCore(long sequenceId)
            {
                //
                // Sequence IDs are inclusive and start from 0. Start replays from the
                // given sequence ID (included).
                //

                Debug.Assert(sequenceId == -1 || sequenceId > LastAck);

                _parent.SubscriptionStart(sequenceId, this);
            }

            public void AcknowledgeRange(long sequenceId)
            {
                if (!IsStarted)
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
                while (true)
                {
                    var state = Volatile.Read(ref _state);

                    if (state is Disposed or DisposeRequested)
                    {
                        // Already disposed; no-op.
                        return;
                    }

                    if (state == Starting)
                    {
                        // Let the starting thread take care of Dispose, so we don't spin here.
                        if (Interlocked.CompareExchange(ref _state, DisposeRequested, state) == state)
                        {
                            return;
                        }

                        continue;
                    }

                    if (Interlocked.CompareExchange(ref _state, Disposed, state) == state)
                    {
                        // We need to dispose the subscription only if original state was started.
                        if (state == Started)
                        {
                            DisposeCore();
                        }

                        return;
                    }
                }
            }

            private void DisposeCore()
            {
                _parent.SubscriptionDispose(this);
            }

            public void OnNext(T item, long sequenceId)
            {
                if (!IsStarted)
                {
                    return;
                }

                _observer.OnNext(item, sequenceId);
            }

            public void OnError(Exception error)
            {
                if (!IsStarted)
                {
                    return;
                }

                _observer.OnError(error);
                Dispose();
            }

            public void OnCompleted()
            {
                if (!IsStarted)
                {
                    return;
                }

                _observer.OnCompleted();
                Dispose();
            }

            public void Accept(ISubscriptionVisitor visitor) => visitor.Visit(this);

            public IEnumerable<ISubscription> Inputs => Enumerable.Empty<ISubscription>();

            public void SetContext(IOperatorContext context) => Context = (IHostedOperatorContext)context;
        }

        // Wraps an non-reliable observer into a reliable one by dropping the sequence numbers in OnNext.
        private sealed class ObserverWrapper : IReliableObserver<T>
        {
            private readonly IObserver<T> _observer;

            public ObserverWrapper(IObserver<T> observer)
            {
                Debug.Assert(observer != null);
                _observer = observer;
            }

            public Uri ResubscribeUri => throw new NotImplementedException();

            public void OnNext(T item, long sequenceId) => _observer.OnNext(item);

            public void OnStarted()
            {
                // Nothing?
            }

            public void OnError(Exception error) => _observer.OnError(error);

            public void OnCompleted() => _observer.OnCompleted();
        }

        #endregion

        #region Dispose

        public void Dispose()
        {
            if (Interlocked.CompareExchange(ref _disposed, 1, 0) != 0)
            {
                return;
            }

            var errors = new List<Exception>();

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1031 // Do not catch general exception types. (Errors are reported through AggregateException.)

            if (_upstreamSubscriptionUri != null)
            {
                _context.TraceSource.Bridge_DisposingUpstreamSubscription(_upstreamSubscriptionUri, Id);

                try
                {
                    _context.ReactiveService.GetSubscription(_upstreamSubscriptionUri).Dispose();
                }
                catch (Exception ex)
                {
                    errors.Add(new BridgeCleanupException(string.Format(CultureInfo.InvariantCulture, "Failed to delete upstream subscription '{0}'. Please refer to the inner exception for more information.", _upstreamSubscriptionUri.ToCanonicalString()), ex));
                }
                finally
                {
                    _upstreamSubscriptionUri = null;
                }
            }

            if (_upstreamObservableUri != null)
            {
                _context.TraceSource.Bridge_DisposingUpstreamObservable(_upstreamObservableUri, Id);

                try
                {
                    _context.ReactiveService.UndefineObservable(_upstreamObservableUri);
                }
                catch (Exception ex)
                {
                    errors.Add(new BridgeCleanupException(string.Format(CultureInfo.InvariantCulture, "Failed to undefine upstream observable '{0}'. Please refer to the inner exception for more information.", _upstreamObservableUri), ex));
                }
                finally
                {
                    _upstreamObservableUri = null;
                }
            }

#pragma warning restore CA1031
#pragma warning restore IDE0079

            StateChanged = true;

            if (errors.Count == 1)
            {
                ExceptionDispatchInfo.Capture(errors[0]).Throw();
            }
            else if (errors.Count > 1)
            {
                throw new AggregateException(errors);
            }
        }

        #endregion

        #region State

        public IEnumerable<ISubscription> Inputs => Enumerable.Empty<ISubscription>();

        public string Name => "rce:Bridge";

        public Version Version { get; private set; } = BridgeVersion.Version;

        public bool StateChanged
        {
            get => _stateful.StateChanged;

            private set => _stateful.StateChanged = value;
        }

        public void Subscribe() { }

        public void SetContext(IOperatorContext context)
        {
            Debug.Assert(context is IHostedOperatorContext, "Context is not allowed to be null.");
            _context = (IHostedOperatorContext)context;
        }

        public void SaveState(IOperatorStateWriter writer, Version version)
        {
            if (version == Versioning.v1)
            {
                writer.Write(_upstreamSubscriptionUri);
                writer.Write(_upstreamObservableUri);
                writer.Write(_completeNotified);
                writer.Write(_lowWatermark);

                _stateful.SaveState();
            }
            else if (version == Versioning.v2)
            {
                writer.Write(_upstreamSubscriptionUri);
                writer.Write(_completeNotified);
                writer.Write(_lowWatermark);

                _stateful.SaveState();
            }
            else
            {
                throw new NotSupportedException(
                    string.Format(CultureInfo.InvariantCulture, "This implementation of Bridge does not support operator state version '{0}'.", version));
            }
        }

        public void LoadState(IOperatorStateReader reader, Version version)
        {
            if (version == Versioning.v1)
            {
                _upstreamSubscriptionUri = reader.Read<Uri>();
                _upstreamObservableUri = reader.Read<Uri>();
                _completeNotified = reader.Read<bool>();
                _lowWatermark = reader.Read<long>();

                // TODO: Handle _completeNotified == true

                Version = version;
                _stateful.LoadState();
            }
            else if (version == Versioning.v2)
            {
                _upstreamSubscriptionUri = reader.Read<Uri>();
                _completeNotified = reader.Read<bool>();
                _lowWatermark = reader.Read<long>();

                // TODO: Handle _completeNotified == true

                Version = version;
                _stateful.LoadState();
            }
            else
            {
                throw new NotSupportedException(
                    string.Format(CultureInfo.InvariantCulture, "This implementation of Bridge does not support operator state version '{0}'.", version));
            }
        }

        public void OnStateSaved() => _stateful.OnStateSaved();

        public void Start() { }

        #endregion
    }
}
