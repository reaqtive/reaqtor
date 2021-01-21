// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using Reaqtive.Scheduler;
using Reaqtive.Tasks;

namespace Reaqtive
{
    /// <summary>
    /// Base class for operators that need a context switch between some arbitrary thread and a logical scheduler.
    /// </summary>
    /// <typeparam name="TParam">Type of parameter.</typeparam>
    /// <typeparam name="TResult">Type of elements produced by the context switch operator.</typeparam>
    public abstract class ContextSwitchOperator<TParam, TResult> : StatefulOperator<TParam, TResult>, IObserver<TResult>
    {
        private readonly _ _itemProcessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContextSwitchOperator{TParam, TResult}"/> class.
        /// </summary>
        /// <param name="parent">Parameters passed to the context switch operator.</param>
        /// <param name="observer">Observer to publish elements into.</param>
        protected ContextSwitchOperator(TParam parent, IObserver<TResult> observer)
            : this(parent, new _(observer))
        {
        }

        private ContextSwitchOperator(TParam parent, _ itemProcessor)
            : base(parent, itemProcessor)
        {
            _itemProcessor = itemProcessor;
        }

        /// <summary>
        /// Subscribes to the operator's input.
        /// </summary>
        /// <returns>Sequence of operator inputs.</returns>
        protected override IEnumerable<ISubscription> OnSubscribe()
        {
            return new[] { (ISubscription)Output };
        }

        /// <summary>
        /// Gets the queue maintained by the context switch operator.
        /// </summary>
        public IMonitorableQueue<TResult> MonitorableQueue => _itemProcessor;

        #region IObserver

        /// <summary>
        /// Publishes a value into the context switch operator.
        /// </summary>
        /// <param name="value">Value to publish.</param>
        public virtual void OnNext(TResult value)
        {
            Output.OnNext(value);
        }

#pragma warning disable CA1716 // Identifiers should not match keywords. (Using error from `IObserver<T>.OnError(Exception error)`.)

        /// <summary>
        /// Publishes an error into the context switch operator.
        /// </summary>
        /// <param name="error">Error to publish.</param>
        public virtual void OnError(Exception error)
        {
            Output.OnError(error);
        }

#pragma warning restore CA1716

        /// <summary>
        /// Publishes a completion notification into the context switch operator.
        /// </summary>
        public virtual void OnCompleted()
        {
            Output.OnCompleted();
        }

        #endregion

        private sealed class _ : StatefulObserver<TResult>, IMonitorableQueue<TResult>, IYieldableItemProcessor
        {
            #region Constructors & Fields

            private readonly IObserver<TResult> _observer;
            private readonly List<TResult> _queue;
            private readonly ISchedulerTask _task;

            private Exception _error;
            private bool _completed;

            private IOperatorContext _context;
            private IScheduler _childScheduler;

            private Exception _terminalError;

            private bool _itemProcessed = false;
            private int _initialQueueCount = 0;

            /// <summary>
            /// Event buffer, not thread safe, should be accessed only from QE scheduler.
            /// </summary>
            private TResult[] _eventBuffer;

            public _(IObserver<TResult> observer)
            {
                _observer = observer;
                _queue = new List<TResult>();
                _task = new YieldableItemProcessingTask(this);
                _eventBuffer = new TResult[16];
            }

            #endregion

            #region IVersioned

            public override string Name => "rc:ContextSwitch/v";

            public override Version Version => Versioning.v1;

            #endregion

            #region Operator

            public override void SetContext(IOperatorContext context)
            {
                Debug.Assert(context != null, "Context should not be null.");
                _context = context;
                _childScheduler = context.Scheduler.CreateChildScheduler();
            }

            protected override void OnStart()
            {
                // If the the operator was disposed, we should not schedule anything.
                if (!IsDisposed)
                {
                    _childScheduler.Schedule(_task);
                }
            }

            #endregion

            #region Stateful operator

            protected override void LoadStateCore(IOperatorStateReader reader)
            {
                base.LoadStateCore(reader);

                lock (_queue)
                {
                    _completed = reader.Read<bool>();

                    var length = reader.Read<int>();
                    _queue.Capacity = length;
                    _initialQueueCount = length;

                    for (int i = 0; i < length; ++i)
                    {
                        var element = reader.Read<TResult>();
                        _queue.Add(element);
                    }
                }
            }

            protected override void SaveStateCore(IOperatorStateWriter writer)
            {
                base.SaveStateCore(writer);

                lock (_queue)
                {
                    writer.Write(_completed);
                    writer.Write(_queue.Count);

                    _queue.ForEach(writer.Write);

                    _initialQueueCount = _queue.Count;
                }
            }

            public override bool StateChanged
            {
                get
                {
                    // There are two cases to consider for how the state of a context switch
                    // operator can change after being started or having state saved.
                    //
                    // 1) The operator starts (or continues after a save) with an empty item
                    //    queue, so the state is considered dirty if the queue is not empty.
                    //
                    // 2) The operator starts (or continues after a save) with a non-empty
                    //    item queue, so the state is considered dirty so long as an item
                    //    from the queue is processed, or the queue has grown.
                    //
                    // In both case 1 and 2, if the operator or a derived class explicitly
                    // sets the dirty flag, then the operator is considered dirty.
                    if (base.StateChanged)
                    {
                        return true;
                    }
                    else if (_initialQueueCount == 0)
                    {
                        return _queue.Count != 0;
                    }
                    else
                    {
                        return _itemProcessed || _initialQueueCount != _queue.Count;
                    }
                }

                protected set => base.StateChanged = value;
            }

            public override void OnStateSaved()
            {
                base.OnStateSaved();
                _itemProcessed = false;
            }

            #endregion

            #region IObserver

            protected override void OnCompletedCore()
            {
                lock (_queue)
                {
                    if (IsDisposed)
                    {
                        return;
                    }

                    _completed = true;
                    _childScheduler.RecalculatePriority();
                }
            }

            protected override void OnErrorCore(Exception error)
            {
                lock (_queue)
                {
                    if (IsDisposed)
                    {
                        return;
                    }

                    _error = error;
                    _childScheduler.RecalculatePriority();
                }
            }

            protected override void OnNextCore(TResult item)
            {
                lock (_queue)
                {
                    if (IsDisposed)
                    {
                        return;
                    }

                    OnEnqueueing(item);
                    _queue.Add(item);
                    _childScheduler.RecalculatePriority();
                }
            }

            #endregion

            #region IMonitorableQueue

            public event Action<TResult> Enqueueing;

            public event Action<TResult> Dequeued;

            public int QueueSize => _queue.Count;

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1031 // Do not catch general exception types. (Hardening against the events throwing.)

            private void OnEnqueueing(TResult item)
            {
                try
                {
                    Enqueueing?.Invoke(item);
                }
                catch (Exception ex)
                {
                    _context.TraceSource.ContextSwitch_OnEnqueueing_Error(ex);
                }
            }

            private void OnDequeued(TResult item)
            {
                try
                {
                    Dequeued?.Invoke(item);
                }
                catch (Exception ex)
                {
                    _context.TraceSource.ContextSwitch_Dequeued_Error(ex);
                }
            }

#pragma warning restore CA1031
#pragma warning restore IDE0079

            #endregion

            #region IItemProcessor

            public int ItemCount
            {
                get
                {
                    lock (_queue)
                    {
                        // If the operator is disposed, there should be no items to process.
                        if (IsDisposed)
                        {
                            return 0;
                        }
                        // Otherwise, return the number of items in the queue, plus one
                        // additional item if there is a completion or error message to
                        // process.
                        else
                        {
                            return _queue.Count + (_completed || _error != null ? 1 : 0);
                        }
                    }
                }
            }

            public void Process(int batchSize) => Process(batchSize, token: default);

            public void Process(int batchSize, YieldToken token)
            {
                if (token.IsYieldRequested)
                    return;

                bool completed = default;
                Exception error = default;
                int eventCount;

                _itemProcessed = true;

                if (_eventBuffer.Length < batchSize)
                {
                    _eventBuffer = new TResult[batchSize];
                }

                lock (_queue)
                {
                    eventCount = Math.Min(_queue.Count, batchSize);
                    _queue.CopyTo(0, _eventBuffer, 0, eventCount);
                    _queue.RemoveRange(0, eventCount);

                    if (_queue.Count == 0)
                    {
                        completed = _completed;
                        error = _error;
                    }
                }

                int i;

                for (i = 0; i < eventCount && !token.IsYieldRequested; ++i)
                {
                    var evt = _eventBuffer[i];

                    var success = false;
                    try
                    {
                        _observer.OnNext(evt);
                        success = true;
                        OnDequeued(evt);
                    }
                    catch (Exception ex)
                    {
                        //
                        // Failure to deliver one message is final. We can't retry without triggering duplication of
                        // side-effects on the downstream observers, nor can we skip messages because it'd break the
                        // sequential nature of observable sequences. As such, we can do nothing but terminate.
                        //
                        TraceObserverError("OnNext", ex, evt);
                        throw;
                    }
                    finally
                    {
                        if (!success)
                        {
                            Dispose();
                        }
                    }
                }

                if (i != eventCount)
                {
                    lock (_queue)
                    {
                        _queue.InsertRange(0, _eventBuffer.Take(eventCount).Skip(i));
                    }

                    return;
                }

                if (token.IsYieldRequested)
                    return;

                if (error != null)
                {
                    try
                    {
                        _observer.OnError(error);
                    }
                    catch (Exception ex)
                    {
                        TraceObserverError("OnError", ex, error);
                        throw;
                    }
                    finally
                    {
                        Dispose();
                    }
                }

                if (completed)
                {
                    try
                    {
                        _observer.OnCompleted();
                    }
                    catch (Exception ex)
                    {
                        TraceObserverError("OnCompleted", ex);
                        throw;
                    }
                    finally
                    {
                        Dispose();
                    }
                }
            }

            private void TraceObserverError(string call, Exception error, object arg = null)
            {
                //
                // Keeping this exception around for post-mortem dump debugging.
                //
                _terminalError = error;

                _context.TraceSource.ContextSwitch_Observer_Error(_context.InstanceId, call, arg, _terminalError);
            }

            #endregion

            #region IDisposable

            protected override void OnDispose()
            {
                lock (_queue)
                {
                    if (_childScheduler != null)
                    {
                        _childScheduler.Dispose();
                        _childScheduler = null;
                    }

                    _queue.Clear();
                }

                base.OnDispose();
            }

            #endregion
        }
    }
}
