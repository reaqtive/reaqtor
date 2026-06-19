// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Reaqtive.Scheduler;

namespace Reaqtive.TestingFramework
{
    /// <summary>
    /// Base class for virtual time schedulers.
    /// </summary>
    /// <typeparam name="TAbsolute">Absolute time representation type.</typeparam>
    /// <typeparam name="TRelative">Relative time representation type.</typeparam>
    public abstract class VirtualTimePhysicalSchedulerBase<TAbsolute, TRelative> where TAbsolute : IComparable<TAbsolute>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VirtualTimePhysicalSchedulerBase{TAbsolute, TRelative}"/> class.
        /// </summary>
        protected VirtualTimePhysicalSchedulerBase() : this(Comparer<TAbsolute>.Default, default)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VirtualTimePhysicalSchedulerBase{TAbsolute, TRelative}"/> class.
        /// </summary>
        /// <param name="comparer">The comparer.</param>
        /// <param name="initialClock">The initial clock.</param>
        protected VirtualTimePhysicalSchedulerBase(IComparer<TAbsolute> comparer, TAbsolute initialClock)
        {
            Clock = initialClock;
            Comparer = comparer ?? throw new ArgumentNullException(nameof(comparer));
        }

        /// <summary>
        /// Gets the scheduler's absolute time clock value.
        /// </summary>
        public TAbsolute Clock
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets the comparer used to compare absolute time values.
        /// </summary>
        protected IComparer<TAbsolute> Comparer
        {
            get;
            private set;
        }

        /// <summary>
        /// Starts the scheduler.
        /// </summary>
        public void Start()
        {
            var maxTime = Add(absolute: default, relative: ToRelative(TimeSpan.MaxValue));
            AdvanceTo(maxTime);
        }

        /// <summary>
        /// Advances the scheduler's clock to the specified time, running all work till that point.
        /// </summary>
        /// <param name="time">Absolute time to advance the scheduler's clock to.</param>
        public void AdvanceTo(TAbsolute time)
        {
            var dueToClock = Comparer.Compare(time, Clock);
            if (dueToClock < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(time));
            }

            if (dueToClock == 0)
            {
                return;
            }

            do
            {
                var next = DequeueItem();
                if (next == null)
                {
                    break;
                }

                IScheduler logicalScheduler = next.Scheduler;
                if (Comparer.Compare(next.DueTime, time) > 0)
                {
                    // Should not run, schedule it again.
                    logicalScheduler.Schedule(ToDateTimeOffset(next.DueTime) - Now, next.Task);
                    break;
                }

                if (Comparer.Compare(next.DueTime, Clock) > 0)
                {
                    // Advance the clock.
                    Clock = next.DueTime;
                }

                bool completed = next.Invoke();

                if (logicalScheduler is VirtualTimeLogicalScheduler<TAbsolute, TRelative> child)
                {
                    child.Remove(next);
                }

                if (!completed)
                {
                    // not completed, should put on exectution again.
                    logicalScheduler.Schedule(Now, next.Task);
                }
            }
            while (true);
        }

        /// <summary>
        /// Advances the scheduler's clock by the specified relative time.
        /// </summary>
        /// <param name="time">Relative time to advance the scheduler's clock by.</param>
        public void Sleep(TRelative time)
        {
            var dt = Add(Clock, time);

            var dueToClock = Comparer.Compare(dt, Clock);
            if (dueToClock < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(time));
            }

            Clock = dt;
        }

        /// <summary>
        /// Schedules a task to be executed after the specified dueTime.
        /// </summary>
        /// <param name="dueTime">Relative time at which to execute the task.</param>
        /// <param name="task">The task.</param>
        /// <param name="scheduler">The scheduler.</param>
        /// <returns>A scheduled work item.</returns>
        public IWorkItem<TAbsolute> ScheduleRelative(TRelative dueTime, ISchedulerTask task, IScheduler scheduler)
        {
            var runAt = Add(Clock, dueTime);
            return ScheduleAbsolute(runAt, task, scheduler);
        }

        #region Abstract methods

        /// <summary>
        /// Adds a relative time value to an absolute time value.
        /// </summary>
        /// <param name="absolute">Absolute time value.</param>
        /// <param name="relative">Relative time value to add.</param>
        /// <returns>The resulting absolute time sum value.</returns>
        public abstract TAbsolute Add(TAbsolute absolute, TRelative relative);

        /// <summary>
        /// Converts the absolute time value to a DateTimeOffset value.
        /// </summary>
        /// <param name="absolute">Absolute time value to convert.</param>
        /// <returns>The corresponding DateTimeOffset value.</returns>
        public abstract DateTimeOffset ToDateTimeOffset(TAbsolute absolute);

        /// <summary>
        /// Converts the TimeSpan value to a relative time value.
        /// </summary>
        /// <param name="timeSpan">TimeSpan value to convert.</param>
        /// <returns>The corresponding relative time value.</returns>
        public abstract TRelative ToRelative(TimeSpan timeSpan);

        /// <summary>
        /// Dequeues next item to execute.
        /// </summary>
        /// <returns>Work item.</returns>
        protected abstract IWorkItem<TAbsolute> DequeueItem();

        /// <summary>
        /// Schedules a task to be executed at dueTime.
        /// </summary>
        /// <param name="dueTime">Absolute time at which to execute the task.</param>
        /// <param name="task">The task.</param>
        /// <param name="scheduler">The scheduler.</param>
        /// <returns>A scheduled work item.</returns>
        public abstract IWorkItem<TAbsolute> ScheduleAbsolute(TAbsolute dueTime, ISchedulerTask task, IScheduler scheduler);

        /// <summary>
        /// Cancels the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        public abstract void Cancel(IWorkItem<TAbsolute> item);

        /// <summary>
        /// Pauses the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>A pause task.</returns>
        public abstract Task PauseAsync(IWorkItem<TAbsolute> item);

#pragma warning disable CA1716 // Identifiers should not match keywords. (Backwards compatibility. Resume isn't any better.)

        /// <summary>
        /// Continues execution of the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        public abstract void Continue(IWorkItem<TAbsolute> item);

#pragma warning restore CA1716

        /// <summary>
        /// Recalculates the priority of the item.
        /// </summary>
        /// <param name="item">The item.</param>
        public abstract void RecalculatePriority(IWorkItem<TAbsolute> item);

        #endregion

        /// <summary>
        /// Gets current time.
        /// </summary>
        public DateTimeOffset Now => ToDateTimeOffset(Clock);
    }
}
