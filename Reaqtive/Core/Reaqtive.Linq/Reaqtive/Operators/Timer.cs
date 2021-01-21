// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using Reaqtive.Scheduler;

namespace Reaqtive.Operators
{
    /// <summary>
    /// Timer operator.
    /// </summary>
    internal sealed class Timer : SubscribableBase<long>
    {
        /// <summary>
        /// The absolute due time.
        /// </summary>
        private readonly DateTimeOffset? _absoluteDueTime;

        /// <summary>
        /// The relative due time.
        /// </summary>
        private readonly TimeSpan? _relativeDueTime;

        /// <summary>
        /// The period.
        /// </summary>
        private readonly TimeSpan? _period;

        /// <summary>
        /// Initializes a new instance of the <see cref="Timer"/> class.
        /// </summary>
        /// <param name="dueTime">The due time.</param>
        /// <param name="period">The period.</param>
        public Timer(DateTimeOffset dueTime, TimeSpan? period)
        {
            _absoluteDueTime = dueTime;
            _period = period;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Timer"/> class.
        /// </summary>
        /// <param name="dueTime">The due time.</param>
        /// <param name="period">The period.</param>
        public Timer(TimeSpan dueTime, TimeSpan? period)
        {
            _relativeDueTime = dueTime;
            _period = period;
        }

        /// <summary>
        /// Subscribes the specified observer to a timer.
        /// </summary>
        /// <param name="observer">The observer.</param>
        /// <returns>A newly created subscription.</returns>
        protected override ISubscription SubscribeCore(IObserver<long> observer)
        {
            return _period.HasValue
                ? new Periodic(this, observer)
                : new Single(this, observer);
        }

        /// <summary>
        /// Gets a friendly string representation of the operator.
        /// </summary>
        /// <returns>Friendly string representation of the operator.</returns>
        public override string ToString()
        {
            var start = _absoluteDueTime != null ? _absoluteDueTime.Value.ToString(CultureInfo.InvariantCulture) : _relativeDueTime.Value.ToString();

            if (_period != null)
            {
                return "Timer(" + start + ", " + _period.Value.ToString() + ")";
            }
            else
            {
                return "Timer(" + start + ")";
            }
        }

        /// <summary>
        /// Implementation of a scheduler task for timers.
        /// </summary>
        /// <typeparam name="TParam">Type of the parameters passed to the observer.</typeparam>
        private abstract class TimerTask<TParam> : StatefulUnaryOperator<TParam, long>, ISchedulerTask
        {
            /// <summary>
            /// Creates a new timer task instance using the given parameters and the
            /// observer to report timer events to.
            /// </summary>
            /// <param name="parent">Parameters used by the timer.</param>
            /// <param name="observer">Observer receiving the timer's output.</param>
            protected TimerTask(TParam parent, IObserver<long> observer)
                : base(parent, observer)
            {
            }

            /// <summary>
            /// Gets task priority.
            /// </summary>
            public long Priority => 1L;

            /// <summary>
            /// Gets a value indicating whether the task is runnable.
            /// </summary>
            /// <value>
            /// <c>true</c> if this instance is runnable; otherwise, <c>false</c>.
            /// </value>
            public bool IsRunnable => true;

            /// <summary>
            /// Executes the task.
            /// </summary>
            /// <param name="scheduler">The scheduler.</param>
            /// <returns>True if the task has been completed.</returns>
            public bool Execute(IScheduler scheduler)
            {
                Invoke();
                return true;
            }

            /// <summary>
            /// Recalculates the priority of the task. The task can become runnable
            /// as the result of this operation.
            /// </summary>
            public void RecalculatePriority()
            {
            }

            /// <summary>
            /// Invokes the observer when the timer fires.
            /// </summary>
            protected abstract void Invoke();

            protected static DateTimeOffset? AddSafe(DateTimeOffset? left, TimeSpan? right)
            {
                if (left.HasValue && right.HasValue)
                {
                    var l = left.Value;
                    var r = right.Value;

                    if (r > DateTimeOffset.MaxValue - l)
                    {
                        return DateTimeOffset.MaxValue;
                    }
                    else if (r < DateTimeOffset.MinValue - l)
                    {
                        return DateTimeOffset.MinValue;
                    }

                    return l + r;
                }

                return null;
            }
        }

        /// <summary>
        /// Base class for timers.
        /// </summary>
        private abstract class TimerBase : TimerTask<Timer>
        {
            /// <summary>
            /// Counter used to assign unique instance identifiers, in a way similar to Task.Id.
            /// </summary>
            private static int s_count;

            /// <summary>
            /// A unique identifier for the timer.
            /// </summary>
            protected readonly int _id;

            /// <summary>
            /// Initializes a new instance of the <see cref="TimerBase"/> class.
            /// </summary>
            /// <param name="parent">The parent.</param>
            /// <param name="observer">The observer.</param>
            public TimerBase(Timer parent, IObserver<long> observer)
                : base(parent, observer)
            {
                _id = Interlocked.Increment(ref s_count);
            }

            /// <summary>
            /// Gets a friendly string representation of the operator.
            /// </summary>
            /// <returns>Friendly string representation of the operator.</returns>
            public override string ToString()
            {
                return Params.ToString() + " [" + _id + "]";
            }
        }

        /// <summary>
        /// Implementation of a single timer subscription.
        /// </summary>
        private sealed class Single : TimerBase
        {
            /// <summary>
            /// The next absolute time that the timer should fire.
            /// </summary>
            private DateTimeOffset? _nextAbsoluteTime;

            /// <summary>
            /// The context.
            /// </summary>
            private IOperatorContext _context;

            /// <summary>
            /// The timer has fired. Will be used for checkpointing.
            /// </summary>
            private bool _hasFired;

            /// <summary>
            /// Initializes a new instance of the <see cref="Single"/> class.
            /// </summary>
            /// <param name="parent">The parent.</param>
            /// <param name="observer">The observer.</param>
            public Single(Timer parent, IObserver<long> observer)
                : base(parent, observer)
            {
            }

            public override string Name => "rc:Timer+Single";

            public override Version Version => Versioning.v1;

            /// <summary>
            /// Sets the context.
            /// </summary>
            /// <param name="context">The context.</param>
            public override void SetContext(IOperatorContext context)
            {
                Debug.Assert(context != null, "Context should not be null.");

                base.SetContext(context);

                _context = context;

                if (Params._absoluteDueTime != null)
                {
                    _context.TraceSource.Timer_Single_Absolute_Created(_id, _context.InstanceId, Params._absoluteDueTime.Value);
                }
                else
                {
                    _context.TraceSource.Timer_Single_Relative_Created(_id, _context.InstanceId, Params._relativeDueTime.Value);
                }
            }

            /// <summary>
            /// Called on start of the operator.
            /// </summary>
            protected override void OnStart()
            {
                Debug.Assert(_context != null, "Context should have already been set.");

                if (_hasFired)
                {
                    Dispose();
                    return;
                }

                if (_nextAbsoluteTime == null)
                {
                    if (Params._absoluteDueTime != null)
                    {
                        _nextAbsoluteTime = Params._absoluteDueTime.Value;
                    }
                    else
                    {
                        _nextAbsoluteTime = AddSafe(_context.Scheduler.Now, Params._relativeDueTime.Value);
                    }
                }

                var due = _nextAbsoluteTime.Value.ToUniversalTime();
                _context.TraceSource.Timer_ScheduledToFire(_id, _context.InstanceId, 0L, due);

                _context.Scheduler.Schedule(_nextAbsoluteTime.Value, this);
            }

            /// <summary>
            /// Called when the operator is disposed.
            /// </summary>
            protected override void OnDispose()
            {
                base.OnDispose();

                if (_context != null && _context.TraceSource != null)
                {
                    _context.TraceSource.Timer_Disposed(_id, _context.InstanceId);
                }
            }

            /// <summary>
            /// Invokes the observer when the timer fires.
            /// </summary>
            protected sealed override void Invoke()
            {
                var trace = _context.TraceSource;

                var now = _context.Scheduler.Now;
                var due = _nextAbsoluteTime.Value.ToUniversalTime();
                var delta = now - due;

                if (IsDisposed)
                {
                    trace.Timer_Muted(_id, _context.InstanceId, 0L, now, due, delta);
                    return;
                }

                trace.Timer_Fired(_id, _context.InstanceId, 0L, now, due, delta);

                Output.OnNext(0);
                Output.OnCompleted();

                _hasFired = true;

                StateChanged = true;

                Dispose();
            }

            /// <summary>
            /// Restores the state whether the timer had already fired before the checkpoint.
            /// </summary>
            /// <param name="reader">The reader.</param>
            protected override void LoadStateCore(IOperatorStateReader reader)
            {
                base.LoadStateCore(reader);

                _hasFired = reader.Read<bool>();
                _nextAbsoluteTime = reader.Read<DateTimeOffset?>();
            }

            /// <summary>
            /// Saves the state whether the timer had fired.
            /// </summary>
            /// <param name="writer">The writer.</param>
            protected override void SaveStateCore(IOperatorStateWriter writer)
            {
                base.SaveStateCore(writer);

                writer.Write(_hasFired);
                writer.Write(_nextAbsoluteTime);
            }
        }

        /// <summary>
        /// Implementation of a periodic timer subscription.
        /// </summary>
        private sealed class Periodic : TimerBase
        {
            /// <summary>
            /// The next absolute time that the timer should fire.
            /// </summary>
            private DateTimeOffset? _nextAbsoluteTime;

            /// <summary>
            /// The context.
            /// </summary>
            private IOperatorContext _context;

            /// <summary>
            /// The counter.
            /// </summary>
            private long _counter;

            /// <summary>
            /// Initializes a new instance of the <see cref="Periodic"/> class.
            /// </summary>
            /// <param name="parent">The parent.</param>
            /// <param name="observer">The observer.</param>
            public Periodic(Timer parent, IObserver<long> observer)
                : base(parent, observer)
            {
            }

            public override string Name => "rc:Timer+Period";

            public override Version Version => Versioning.v1;

            /// <summary>
            /// Sets the context.
            /// </summary>
            /// <param name="context">The context.</param>
            public override void SetContext(IOperatorContext context)
            {
                Debug.Assert(context != null, "Context should not be null.");

                base.SetContext(context);

                _context = context;

                if (Params._absoluteDueTime != null)
                {
                    _context.TraceSource.Timer_Periodic_Absolute_Created(_id, _context.InstanceId, Params._absoluteDueTime.Value, Params._period.Value);
                }
                else
                {
                    _context.TraceSource.Timer_Periodic_Relative_Created(_id, _context.InstanceId, Params._relativeDueTime.Value, Params._period.Value);
                }
            }

            /// <summary>
            /// Called on start of the operator.
            /// </summary>
            protected override void OnStart()
            {
                Debug.Assert(_context != null, "Context should have already been set.");

                if (_nextAbsoluteTime == null)
                {
                    if (Params._absoluteDueTime != null)
                    {
                        var start = Params._absoluteDueTime.Value;
                        var period = Params._period.Value;

                        var firstDue = CatchUp(start, period);

                        _nextAbsoluteTime = firstDue;
                    }
                    else
                    {
                        _nextAbsoluteTime = AddSafe(_context.Scheduler.Now, Params._relativeDueTime.Value);
                    }
                }

                var due = _nextAbsoluteTime.Value.ToUniversalTime();
                _context.TraceSource.Timer_ScheduledToFire(_id, _context.InstanceId, _counter, due);

                _context.Scheduler.Schedule(_nextAbsoluteTime.Value, this);
            }

            /// <summary>
            /// Helper function to catch up a timer to the latest past occurrence, if its start time was in the past.
            /// This is done to assist in stateless migration where the repeated firing of past due ticks would lead
            /// to a large amount of events being sent, potentially causing a DoS attack.
            /// </summary>
            /// <param name="start">Timer start time.</param>
            /// <param name="period">Timer period.</param>
            /// <returns>Start time being offset by an integer number of periods such that it won't fire more than one past due tick.</returns>
            private DateTimeOffset CatchUp(DateTimeOffset start, TimeSpan period)
            {
                var result = start;

                var now = _context.Scheduler.Now;

                var diff = now - start;

                if (diff >= period)
                {
                    var catchUp = TimeSpan.Zero;

                    // Periodic timers with a period of 00:00:00 are allowed and will run a recurring timer that
                    // fires repeatedly, as quickly as possible. In terms of catching up, we will just let such a
                    // timer start immediately, without any catch-up ticks.
                    if (period > TimeSpan.Zero)
                    {
                        // This will have the effect of rounding down, so we will keep at least one past event.
                        var delta = diff.Ticks / period.Ticks;

                        catchUp = TimeSpan.FromTicks(delta * period.Ticks);
                    }

                    result += catchUp;

                    _context.TraceSource.Timer_CatchUp(_id, _context.InstanceId, result);
                }

                return result;
            }

            /// <summary>
            /// Called when the operator is disposed.
            /// </summary>
            protected override void OnDispose()
            {
                base.OnDispose();

                _context?.TraceSource.Timer_Disposed(_id, _context.InstanceId);
            }

            /// <summary>
            /// Starts periodic timer.
            /// Currently implemented using recursive scheduling and absolute time.
            /// Has some problems with the time drift, please see comments in Rx code of the timer (Linq/Timer.cs).
            /// </summary>
            protected sealed override void Invoke()
            {
                var trace = _context.TraceSource;

                var now = _context.Scheduler.Now;
                var due = _nextAbsoluteTime.Value.ToUniversalTime();
                var delta = now - due;

                if (IsDisposed)
                {
                    trace.Timer_Muted(_id, _context.InstanceId, _counter, now, due, delta);
                    return;
                }

                trace.Timer_Fired(_id, _context.InstanceId, _counter, now, due, delta);

                Output.OnNext(_counter);

                _counter = unchecked(_counter + 1);
                _nextAbsoluteTime = AddSafe(_nextAbsoluteTime, Params._period.Value);

                StateChanged = true;

                trace.Timer_ScheduledToFire(_id, _context.InstanceId, _counter, _nextAbsoluteTime.Value.ToUniversalTime());

                _context.Scheduler.Schedule(_nextAbsoluteTime.Value, this);
            }

            /// <summary>
            /// Restores the counter.
            /// </summary>
            /// <param name="reader">The reader.</param>
            protected override void LoadStateCore(IOperatorStateReader reader)
            {
                base.LoadStateCore(reader);

                _counter = reader.Read<long>();
                _nextAbsoluteTime = reader.Read<DateTimeOffset?>();
            }

            /// <summary>
            /// Saves the counter.
            /// </summary>
            /// <param name="writer">The writer.</param>
            protected override void SaveStateCore(IOperatorStateWriter writer)
            {
                base.SaveStateCore(writer);

                writer.Write(_counter);
                writer.Write(_nextAbsoluteTime);
            }
        }
    }
}
