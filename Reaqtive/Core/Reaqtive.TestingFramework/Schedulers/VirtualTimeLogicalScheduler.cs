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
    /// Virtual time scheduler implementation of <see cref="IScheduler"/>.
    /// </summary>
    /// <typeparam name="TAbsolute">Absolute time representation type.</typeparam>
    /// <typeparam name="TRelative">Relative time representation type.</typeparam>
    public abstract class VirtualTimeLogicalScheduler<TAbsolute, TRelative> : IScheduler, ISchedulerExceptionHandler
        where TAbsolute : IComparable<TAbsolute>
    {
        private readonly VirtualTimeLogicalScheduler<TAbsolute, TRelative> _parent;
        private readonly List<VirtualTimeLogicalScheduler<TAbsolute, TRelative>> _children;
        private readonly List<IWorkItem<TAbsolute>> _tasks;
        private readonly object _gate = new();

        protected VirtualTimeLogicalScheduler(
            VirtualTimePhysicalSchedulerBase<TAbsolute, TRelative> physical,
            VirtualTimeLogicalScheduler<TAbsolute, TRelative> parent)
        {
            Physical = physical;
            _parent = parent;
            _tasks = new List<IWorkItem<TAbsolute>>();
            _children = new List<VirtualTimeLogicalScheduler<TAbsolute, TRelative>>();
        }

        /// <summary>
        /// Gets the current time. Currently, only physical time. Can be virtual in the future.
        /// </summary>
        public DateTimeOffset Now => Physical.Now;

        /// <summary>
        /// Gets the scheduler's absolute time clock value.
        /// </summary>
        public TAbsolute Clock => Physical.Clock;

        /// <summary>
        /// Starts execution of the scheduled actions.
        /// </summary>
        public void Start()
        {
            Physical.Start();
        }

        /// <summary>
        /// Advances the scheduler to the specified absolute time.
        /// </summary>
        /// <param name="time">The time to advance to.</param>
        public void AdvanceTo(TAbsolute time)
        {
            Physical.AdvanceTo(time);
        }

        /// <summary>
        /// Creates a child scheduler.
        /// </summary>
        /// <returns>A child scheduler.</returns>
        public abstract IScheduler CreateChildScheduler();

        /// <summary>
        /// Schedules a task to be executed at the specified absolute due time.
        /// </summary>
        /// <param name="dueTime">Absolute time at which to execute the task.</param>
        /// <param name="task">The task to execute.</param>
        public virtual void ScheduleAbsolute(TAbsolute dueTime, ISchedulerTask task)
        {
            if (task == null)
                throw new ArgumentNullException(nameof(task));

            lock (_gate)
            {
                var item = Physical.ScheduleAbsolute(dueTime, task, this);

                _tasks.Add(item);
            }
        }

        /// <summary>
        /// Schedules a task to be executed at the specified absolute due time.
        /// </summary>
        /// <param name="dueTime">Relative time at which to execute the task.</param>
        /// <param name="task">The task to execute.</param>
        public void ScheduleRelative(TRelative dueTime, ISchedulerTask task)
        {
            if (task == null)
                throw new ArgumentNullException(nameof(task));

            lock (_gate)
            {
                var item = Physical.ScheduleRelative(dueTime, task, this);

                _tasks.Add(item);
            }
        }

        /// <summary>
        /// Schedules the specified task.
        /// </summary>
        /// <param name="task">The task to execute.</param>
        public void Schedule(ISchedulerTask task)
        {
            if (task == null)
                throw new ArgumentNullException(nameof(task));

            ScheduleAbsolute(Physical.Clock, task);
        }

        /// <summary>
        /// Schedules a task to be executed at the specified absolute due time.
        /// </summary>
        /// <param name="dueTime">Relative time at which to execute the task.</param>
        /// <param name="task">The task to execute.</param>
        public void Schedule(TimeSpan dueTime, ISchedulerTask task)
        {
            if (task == null)
                throw new ArgumentNullException(nameof(task));

            ScheduleRelative(Physical.ToRelative(dueTime), task);
        }

        /// <summary>
        /// Schedules a task to be executed at the specified absolute due time.
        /// </summary>
        /// <param name="dueTime">Absolute time at which to execute the task.</param>
        /// <param name="task">The task to execute.</param>
        public void Schedule(DateTimeOffset dueTime, ISchedulerTask task)
        {
            if (task == null)
                throw new ArgumentNullException(nameof(task));

            ScheduleRelative(Physical.ToRelative(dueTime - Now), task);
        }

        /// <summary>
        /// Asynchronously pauses all tasks on the scheduler and any of the child schedulers.
        /// </summary>
        /// <returns>A task representing the eventual completion of pausing all tasks on the scheduler.</returns>
        public Task PauseAsync()
        {
            var pause = new List<Task>();
            _tasks.ForEach(t => pause.Add(Physical.PauseAsync(t)));
            _children.ForEach(s => pause.Add(s.PauseAsync()));
            return Task.WhenAll(pause);
        }

        /// <summary>
        /// Continues all tasks on the scheduler and any of the child schedulers.
        /// </summary>
        public void Continue()
        {
            _tasks.ForEach(t => Physical.Continue(t));
            _children.ForEach(s => s.Continue());
        }

        /// <summary>
        /// Recalculates the priority of all tasks in the scheduler.
        /// As the result of recalculation some tasks can become runnable/change priority.
        /// </summary>
        public void RecalculatePriority()
        {
            _tasks.ForEach(t => Physical.RecalculatePriority(t));
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes the object.
        /// </summary>
        /// <param name="disposing">true if called from <see cref="IDisposable.Dispose"/>; otherwise, false.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _children.ForEach(s => s.Dispose());
                _children.Clear();
                _tasks.ForEach(t => Physical.Cancel(t));
                _tasks.Clear();

                _parent?._children.Remove(this);
            }
        }

        internal void Remove(IWorkItem<TAbsolute> item)
        {
            _tasks.Remove(item);
        }

        /// <summary>
        /// Gets the underlying physical scheduler.
        /// </summary>
        protected VirtualTimePhysicalSchedulerBase<TAbsolute, TRelative> Physical { get; private set; }

        /// <summary>
        /// Determines whether the calling thread has access to the scheduler.
        /// </summary>
        /// <exception cref="InvalidOperationException">The calling thread does not have access to this scheduler.</exception>
        public void VerifyAccess()
        {
        }

        /// <summary>
        /// Determines whether the calling thread has access to the scheduler.
        /// </summary>
        /// <returns>true if the calling thread has access to the scheduler; otherwise, false.</returns>
        public bool CheckAccess()
        {
            return true;
        }

        /// <summary>
        /// Event to observe unhandled exceptions and optionally mark them as handled.
        /// </summary>
        public event EventHandler<SchedulerUnhandledExceptionEventArgs> UnhandledException;

        /// <summary>
        /// Tries to handle an exception that was thrown by a work item running on the scheduler.
        /// </summary>
        /// <param name="exception">Exception to handle.</param>
        /// <param name="task">Task that threw the exception.</param>
        /// <returns>true if the exception was handled; otherwise, false.</returns>
        public bool TryCatch(Exception exception, IWorkItem task)
        {
            if (task == null)
                throw new ArgumentNullException(nameof(task));

            var handler = UnhandledException;
            if (handler != null)
            {
                var e = new SchedulerUnhandledExceptionEventArgs(task.Scheduler, exception);
                handler(this, e);
                return e.Handled;
            }

            return false;
        }
    }
}
