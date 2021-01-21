// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Threading;

using Reaqtive;

namespace Reaqtor.Reactive
{
    public partial class HigherOrderExecutionEnvironment
    {
        private static readonly Version v1 = new(1, 0, 0, 0);

        private sealed class PersistableSubscription<TInner> : IPersistableSubscription, IStatefulOperator, IDependencyOperator
        {
            private readonly SingleAssignmentSubscription _subscription = new();
            private readonly IObserver<TInner> _observer;
            private Uri _bridgeUri;
            private IOperatorContext _context;

            private StateChangedManager _stateful;

            //
            // NB: See remarks in Bridge.cs on the state transitions enforced here.
            //

            private const int Created = 0;
            private const int Starting = 1;
            private const int Started = 2;
            private const int StartFailed = 3;
            private const int DisposeRequested = 4;
            private const int Disposed = 5;

            private int _state = Created;

            public PersistableSubscription(Uri bridgeUri, IObserver<TInner> observer)
            {
                Debug.Assert(observer != null);

                _bridgeUri = bridgeUri;
                _observer = observer;
            }

            public void Subscribe() { }

            public void SetContext(IOperatorContext context) => _context = context;

            public string Name => "rc:HigherOrder/InnerSub";

            public Version Version => v1;

            public IEnumerable<Uri> Dependencies
            {
                get
                {
                    if (_bridgeUri != null)
                    {
                        yield return _bridgeUri;
                    }
                }
            }

            public void Load(IOperatorStateReader reader)
            {
                using var child = reader.CreateChild();

                StateManager.LoadVersionInfo(child, this); // result ignored for the time being

                _bridgeUri = child.Read<Uri>();
            }

            public void Save(IOperatorStateWriter writer)
            {
                using var child = writer.CreateChild();

                StateManager.SaveVersionInfo(child, this);

                child.Write<Uri>(_bridgeUri);
            }

            public void LoadState(IOperatorStateReader reader, Version version)
            {
                if (_observer is IStatefulOperator so)
                {
                    StateManager.LoadState(reader, so);
                }

                _stateful.LoadState();
            }

            public void SaveState(IOperatorStateWriter writer, Version version)
            {
                if (_observer is IStatefulOperator so)
                {
                    StateManager.SaveState(writer, so);
                }

                _stateful.SaveState();
            }

            public void Start()
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
                        StartCore();

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

            private void StartCore()
            {
                if (_bridgeUri != null)
                {
                    var bridge = _context.ExecutionEnvironment.GetSubject<TInner, TInner>(_bridgeUri);

                    try
                    {
                        _context.TraceSource.HigherOrderOperator_StartingInnerSubscription(_bridgeUri, _context.InstanceId);

                        // The bridge subscription should be subscribed to exactly once. However, there is a race
                        // condition (particularly with the SelectMany operator) that occurs when an inner
                        // subscription is created during the start sequence of the outer subscription. E.g., in
                        // the following query:
                        //
                        // xs.SelectMany(_ => ys)
                        //
                        // In the start sequence, `xs` may begin firing events as soon as its subscribed to (while
                        // the SelectMany operator is still starting). After `xs` fires its event, an inner
                        // subscription to `ys` will be created and appended to the children of the SelectMany
                        // operator.  Meanwhile, as the SelectMany operator continues its start sequence, it may
                        // discover that `ys` is included in its list of child nodes after the inner subscription
                        // to `ys` has already been set up and started. This leads to the double start race
                        // condition we are protecting against with the lock below. In the ideal case, SelectMany
                        // would "freeze" its set of children as soon as it enters the start routine, and only
                        // start those children. Based on the way the current visitor pattern is designed, such
                        // logic would be non-trivial to implement.
                        lock (_subscription)
                        {
                            if (_subscription.Subscription != null)
                            {
                                return;
                            }

                            var subscription = bridge.Subscribe(_observer);

                            SubscriptionInitializeVisitor.Subscribe(subscription);
                            SubscriptionInitializeVisitor.SetContext(subscription, _context); // Start of inner subscription will be taken care of by the Accept call

                            _subscription.Subscription = subscription;
                        }
                    }
                    catch (Exception ex)
                    {
#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1031 // Do not catch general exception types. (By design; gets wrapped.)
                        try
                        {
                            bridge.Dispose();
                        }
                        catch (Exception inner)
                        {
                            ex = new AggregateException(ex, inner);
                        }
#pragma warning restore CA1031
#pragma warning restore IDE0079

                        var msg = string.Format(CultureInfo.InvariantCulture, "Higher order operator '{0}' failed to set up a subscription to a received inner sequence using bridge URI '{1}'. Please refer to the inner exception for more information.", GetType().FullName, _bridgeUri);

                        _bridgeUri = null;

                        throw new HigherOrderSubscriptionFailedException(msg, ex);
                    }
                }
            }

            public void Accept(ISubscriptionVisitor visitor) => visitor.Visit(this);

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
                if (_bridgeUri != null)
                {
                    var streamUri = _bridgeUri;
                    _bridgeUri = null;
                    StateChanged = true;

                    var err = default(Exception);

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1031 // Do not catch general exception types. (By design; gets wrapped or rethrown.)
                    try
                    {
                        _context.TraceSource.HigherOrderOperator_DisposingInnerSubscription(streamUri, _context.InstanceId);

                        // This disposal will trigger the following:
                        // 1. Dispose on the intrinsic subscription between the Bridge and the parent observer
                        // 2. A second call to Dispose() on the Bridge (note, the Bridge.Dispose() method is idempotent).
                        _subscription.Dispose();
                    }
                    catch (Exception ex)
                    {
                        err = ex;
                    }

                    try
                    {
                        _context.TraceSource.HigherOrderOperator_DeletingBridge(streamUri, _context.InstanceId);

                        // This disposal will trigger the following:
                        // 1. Stream entity will be removed from query engine registry
                        // 2. Dispose will be triggered on the Bridge instance
                        // 3. Upstream subscription will be disposed and removed from the query engine registry
                        // 4. Upstream observable will be undefined and removed from the query engine registry
                        ((IHigherOrderExecutionEnvironment)_context.ExecutionEnvironment).DeleteSubject<TInner>(streamUri, _context);
                    }
                    catch (Exception ex)
                    {
                        err = (err != null) ? new AggregateException(ex, err) : ex;
                    }

                    if (err != null)
                    {
                        throw err;
                    }
#pragma warning restore CA1031
#pragma warning restore IDE0079
                }
            }

            public void OnStateSaved() => _stateful.OnStateSaved();

            public bool StateChanged
            {
                get => _stateful.StateChanged;

                private set => _stateful.StateChanged = value;
            }

            public IEnumerable<ISubscription> Inputs => new[] { _subscription };
        }
    }
}
