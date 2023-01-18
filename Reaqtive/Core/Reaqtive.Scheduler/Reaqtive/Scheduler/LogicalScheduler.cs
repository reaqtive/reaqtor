// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Reaqtive.Scheduler
{
    /// <summary>
    /// Class representing a logical scheduler.
    /// </summary>
    public sealed class LogicalScheduler : ISchedulerStatus, ISchedulerExceptionHandler, ISchedulerPerformanceCountersProvider, IAccountable, IYieldTokenSource
    {
        /// <summary>
        /// The physical scheduler.
        /// </summary>
        private readonly PhysicalScheduler _scheduler;

        /// <summary>
        /// The child schedulers.
        /// </summary>
        private readonly List<LogicalScheduler> _children;

        /// <summary>
        /// Tasks scheduled through this scheduler.
        /// </summary>
        private readonly HashSet<WorkItem> _tasks;

        /// <summary>
        /// Synchronization gate.
        /// </summary>
        private readonly object _lock = new();

        /// <summary>
        /// The parent scheduler.
        /// </summary>
        private readonly LogicalScheduler _parent;

        /// <summary>
        /// The last exception that occurred on the scheduler, if any.
        /// </summary>
        private Exception _lastError;

        /// <summary>
        /// The scheduler status.
        /// </summary>
        private volatile SchedulerStatus _status;

        /// <summary>
        /// The performance counters tracking utility, exposed via <see cref="ISchedulerPerformanceCountersProvider"/>.
        /// </summary>
        private readonly PerformanceCounters _counters = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicalScheduler"/> class.
        /// </summary>
        /// <param name="scheduler">The scheduler.</param>
        public LogicalScheduler(PhysicalScheduler scheduler) : this(scheduler, parent: null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicalScheduler" /> class.
        /// </summary>
        /// <param name="scheduler">The physical scheduler.</param>
        /// <param name="parent">The parent.</param>
        private LogicalScheduler(PhysicalScheduler scheduler, LogicalScheduler parent)
        {
            _scheduler = scheduler ?? throw new ArgumentNullException(nameof(scheduler));
            _tasks = new HashSet<WorkItem>();
            _status = SchedulerStatus.Running;
            _children = new List<LogicalScheduler>();
            _parent = parent;
        }

        /// <summary>
        /// Gets the current time.
        /// </summary>
        public DateTimeOffset Now => PhysicalScheduler.Now;

        /// <summary>
        /// Gets the scheduler status.
        /// </summary>
        public SchedulerStatus Status => _status;

        /// <summary>
        /// Gets the trace source of the physical scheduler.
        /// </summary>
        internal TraceSource TraceSource => _scheduler.TraceSource;

        /// <summary>
        /// Gets the yield token used for cooperative pausing.
        /// </summary>
        YieldToken IYieldTokenSource.Token => new(this);

        /// <summary>
        /// Gets a value indicating whether a yield has been requested.
        /// </summary>
        bool IYieldTokenSource.IsYieldRequested => _status >= SchedulerStatus.Pausing;

        /// <summary>
        /// Creates a new child scheduler.
        /// </summary>
        /// <returns>A new child scheduler instance.</returns>
        public IScheduler CreateChildScheduler()
        {
            using (_counters.AccountKernel())
            {
                lock (_lock)
                {
                    if (_status == SchedulerStatus.Disposed)
                    {
                        //
                        // NB: To be safe we need to return a scheduler that will discard all of its requests and
                        //     will not post back to the physical scheduler.
                        //

                        return NopScheduler.Instance;
                    }

                    var child = new LogicalScheduler(_scheduler, this)
                    {
                        _status = _status
                    };

                    _children.Add(child);

                    return child;
                }
            }
        }

        /// <summary>
        /// Schedules the specified task.
        /// </summary>
        /// <param name="task">The task to schedule.</param>
        public void Schedule(ISchedulerTask task)
        {
            if (task == null)
                throw new ArgumentNullException(nameof(task));

            Schedule(CreateWorkItem(task, TimeSpan.Zero));
        }

        /// <summary>
        /// Schedules the specified task at the specified relative due time.
        /// </summary>
        /// <param name="dueTime">The due time.</param>
        /// <param name="task">The task to schedule.</param>
        public void Schedule(TimeSpan dueTime, ISchedulerTask task)
        {
            if (task == null)
                throw new ArgumentNullException(nameof(task));

            Schedule(CreateWorkItem(task, dueTime));
        }

        /// <summary>
        /// Schedules the specified task at the specified absolute due time.
        /// </summary>
        /// <param name="dueTime">The due time.</param>
        /// <param name="task">The task to schedule.</param>
        public void Schedule(DateTimeOffset dueTime, ISchedulerTask task)
        {
            if (task == null)
                throw new ArgumentNullException(nameof(task));

            Schedule(CreateWorkItem(task, dueTime.ToUniversalTime()));
        }

        /// <summary>
        /// Asynchronously pauses all tasks on the scheduler and any of the child schedulers.
        /// </summary>
        /// <returns>A task representing the eventual completion of pausing all tasks on the scheduler.</returns>
        public Task PauseAsync()
        {
            // REVIEW: Is a call to PauseAsync possible from a state other than Running?

            var tasksToPause = GetTasksToPause();

            if (tasksToPause.Count == 0)
            {
                _status = SchedulerStatus.Paused;
                return Task.FromResult(true);
            }

            return PauseAsync(tasksToPause);
        }

        /// <summary>
        /// Pauses the current scheduler and any of its child schedulers, returning a map of workers to
        /// lists of running work items that need to be paused. After this call returns, freshly scheduled
        /// work on the current scheduler, any of its child schedulers, or new child schedulers, will be
        /// put in the paused state immediately. The work items returned in the map may still be running.
        /// </summary>
        /// <returns>A map of workers to lists of running work items that need to be paused.</returns>
        private Dictionary<Worker, List<WorkItem>> GetTasksToPause()
        {
            var currentTasks = new List<WorkItem>();

            PauseAndCollectRunningTasks(currentTasks);

            using (_counters.AccountKernel())
            {
                var tasksToPause = new Dictionary<Worker, List<WorkItem>>();

                foreach (var task in currentTasks)
                {
                    var worker = task.Worker;

                    if (!tasksToPause.TryGetValue(worker, out var items))
                    {
                        items = new List<WorkItem>();
                        tasksToPause.Add(worker, items);
                    }

                    items.Add(task);
                }

                return tasksToPause;
            }
        }

        /// <summary>
        /// Pauses the current scheduler and any of its child schedulers, and appends any running work
        /// items to the specified <paramref name="tasks"/> list. After this call returns, freshly scheduled
        /// work on the current scheduler, any of its child schedulers, or new child schedulers, will be
        /// put in the paused state immediately.
        /// </summary>
        /// <param name="tasks">The list of work items to append to.</param>
        private void PauseAndCollectRunningTasks(List<WorkItem> tasks)
        {
            List<LogicalScheduler> currentChildren;

            using (_counters.AccountKernel())
            {
                //
                // NB: We record the time of pausing the scheduler right before we start to mark incoming work
                //     as paused. Because this method is called recursively on child schedulers, each scheduler
                //     will have its own pause duration recorded. When reporting pause time through performance
                //     counters, a parent scheduler pause time *includes* all of the pause times of the children
                //     because of this recursion, so we should not sum it up at reporting time.
                //

                _counters.MarkPauseBegin();

                lock (_lock)
                {
                    _status = SchedulerStatus.Pausing;
                    tasks.AddRange(_tasks);
                    currentChildren = new List<LogicalScheduler>(_children);
                }
            }

            //
            // NB: Kernel time accounting does not include the recursion. We want to charge the recursive work
            //     to the children so we avoid double-counting when reporting performance counters.
            //

            foreach (var child in currentChildren)
            {
                child.PauseAndCollectRunningTasks(tasks);
            }
        }

        /// <summary>
        /// Triggers pausing of the work items in the specified map and signals completion when all work
        /// items have been successfully paused. After the task returned from this method signals completion,
        /// all work items are guaranteed to have stopped running.
        /// </summary>
        /// <param name="tasksToPause">The map of workers to list work items.</param>
        /// <returns>A task representing the eventual completion of pausing all work items.</returns>
        private Task PauseAsync(Dictionary<Worker, List<WorkItem>> tasksToPause)
        {
            Debug.Assert(tasksToPause.Count > 0);

            //
            // NB: Kernel accounting does *not* include the time it takes for the pause to be completed.
            //     During this time, user work running on schedulers is trying to bail out and the scheduler
            //     infrastructure is not involved. The only time that goes unaccounted for is the atomic
            //     decrement logic in the callback delegate used to determine when all tasks have completed;
            //     this time is negligible and not worth hiring an accountant for.
            //

            using (_counters.AccountKernel())
            {
                var source = new TaskCompletionSource<bool>();

                var stillRunning = tasksToPause.Count;

                void Finish()
                {
                    if (Interlocked.Decrement(ref stillRunning) == 0)
                    {
                        //
                        // NB: We don't want to signal the task right here, because this callback comes on a
                        //     scheduler thread, and we don't want any continuation code to start running here,
                        //     causing the scheduler thread to be hijacked. Instead, we signal completion on
                        //     the regular .NET thread pool.
                        //

                        ThreadPool.QueueUserWorkItem(state =>
                        {
                            _status = SchedulerStatus.Paused;
                            source.SetResult(true);
                        });
                    }
                }

                foreach (var tasks in tasksToPause)
                {
                    var items = tasks.Value;

                    foreach (var item in items)
                    {
                        item.IsPaused = true;
                    }

                    tasks.Key.RecalculatePriority(items, Finish);
                }

                return source.Task;
            }
        }

        /// <summary>
        /// Continues all tasks on the scheduler and any of the child schedulers.
        /// </summary>
        public void Continue()
        {
            // REVIEW: Is a call to Continue possible from a state other than Paused?

            List<WorkItem> pausedTasks;
            List<LogicalScheduler> currentChildren;

            using (_counters.AccountKernel())
            {
                lock (_lock)
                {
                    _status = SchedulerStatus.Running;
                    pausedTasks = new List<WorkItem>(_tasks);
                    currentChildren = new List<LogicalScheduler>(_children);
                }

                foreach (var runningTask in pausedTasks)
                {
                    runningTask.IsPaused = false;
                    PhysicalScheduler.RecalculatePriority(runningTask);
                }

                //
                // NB: We record the time of continuing the scheduler before children have been continued; the
                //     tasks owned by the current scheduler instance are already marked runnable at this time.
                //

                _counters.MarkContinueEnd();
            }

            //
            // NB: Kernel time accounting does not include the recursion. We want to charge the recursive work
            //     to the children so we avoid double-counting when reporting performance counters.
            //

            foreach (var child in currentChildren)
            {
                child.Continue();
            }
        }

        /// <summary>
        /// Recalculates the priority of all tasks in the scheduler.
        /// </summary>
        public void RecalculatePriority()
        {
            using (_counters.AccountKernel())
            {
                List<WorkItem> currentTasks;

                lock (_lock)
                {
                    currentTasks = new List<WorkItem>(_tasks);
                }

                foreach (var task in currentTasks)
                {
                    PhysicalScheduler.RecalculatePriority(task);
                }
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            //
            // NB: No kernel accounting here; it's relatively mood to do so at the point of disposal anyway,
            //     and it brings complexity with double-counting due to recursion into children.
            //

            List<WorkItem> currentTasks;
            List<IScheduler> currentChildren;

            lock (_lock)
            {
                // REVIEW: Is a call to Dispose possible from a state other than Running?

                if (_status == SchedulerStatus.Disposed)
                {
                    return;
                }

                currentTasks = new List<WorkItem>(_tasks);
                currentChildren = new List<IScheduler>(_children);

                _tasks.Clear();
                _children.Clear();

                _status = SchedulerStatus.Disposed;
            }

            foreach (var task in currentTasks)
            {
                PhysicalScheduler.Remove(task);
            }

            foreach (var child in currentChildren)
            {
                child.Dispose();
            }

            _parent?.Remove(this);

            _counters.Dispose();
        }

        /// <summary>
        /// Removes the specified child.
        /// </summary>
        /// <param name="child">The child.</param>
        private void Remove(LogicalScheduler child)
        {
            Debug.Assert(child != null, "Child is not allowed to be null.");

            lock (_lock)
            {
                _children.Remove(child);
            }
        }

        /// <summary>
        /// Schedules the specified item. Made generic because it has to call different overloads
        /// on the physical scheduler and is not allowed to "forget" the type.
        /// </summary>
        /// <param name="item">The item.</param>
        private void Schedule(WorkItem item)
        {
            Debug.Assert(item != null, "Work item is not allowed to be null.");

            using (_counters.AccountKernel())
            {
                lock (_lock)
                {
                    if (_status == SchedulerStatus.Disposed)
                    {
                        return;
                    }

                    item.IsPaused = _status >= SchedulerStatus.Pausing;
                    _tasks.Add(item);
                    _scheduler.Schedule(item);
                }
            }
        }

        /// <summary>
        /// Creates a new work item.
        /// </summary>
        /// <param name="task">The task.</param>
        /// <param name="dueTime">The due time.</param>
        /// <returns>A new work item.</returns>
        private WorkItem CreateWorkItem(ISchedulerTask task, TimeSpan dueTime)
        {
            var due = Now + dueTime;
            return CreateWorkItem(task, due);
        }

        /// <summary>
        /// Creates a new work item.
        /// </summary>
        /// <param name="task">The task.</param>
        /// <param name="dueTime">The due time.</param>
        /// <returns>A new work item.</returns>
        private WorkItem CreateWorkItem(ISchedulerTask task, DateTimeOffset dueTime)
        {
            return new WorkItem(this, task, dueTime);
        }

        /// <summary>
        /// Removes the work item from the logical scheduler.
        /// </summary>
        /// <param name="item">The work item.</param>
        internal void Remove(WorkItem item)
        {
            using (_counters.AccountKernel())
            {
                lock (_lock)
                {
                    _tasks.Remove(item);
                }
            }
        }

        /// <summary>
        /// Determines whether the calling thread has access to the scheduler.
        /// </summary>
        /// <returns><c>true</c> if the calling thread has access to the scheduler; otherwise, <c>false</c>.</returns>
        public bool CheckAccess() => _scheduler.CheckAccess();

        /// <summary>
        /// Determines whether the calling thread has access to the scheduler.
        /// </summary>
        /// <exception cref="InvalidOperationException">The calling thread does not have access to this scheduler.</exception>
        public void VerifyAccess() => _scheduler.VerifyAccess();

        /// <summary>
        /// Event to observe unhandled exceptions and optionally mark them as handled.
        /// </summary>
        public event EventHandler<SchedulerUnhandledExceptionEventArgs> UnhandledException;

        /// <summary>
        /// Tries to handle an exception that was thrown by a work item running on the scheduler.
        /// </summary>
        /// <param name="error">Exception to handle.</param>
        /// <param name="task">Task that threw the exception.</param>
        /// <returns>true if the exception was handled; otherwise, false.</returns>
        bool ISchedulerExceptionHandler.TryCatch(Exception error, IWorkItem task)
        {
            //
            // Keeping the last error around for post-mortem dump debugging.
            //
            // NB: Calling GC.KeepAlive to trick FxCop to not complain about the variable not being used.
            //     GC.KeepAlive is an empty method; calling it doesn't have any effect other than creating
            //     a use site for its argument, so the variable looks like it's being "used".
            //

            _lastError = error;
            GC.KeepAlive(_lastError);

            var unhandledException = UnhandledException;
            if (unhandledException != null)
            {
                var e = new SchedulerUnhandledExceptionEventArgs(task?.Scheduler, error);

                unhandledException(this, e);

                if (e.Handled)
                {
                    return true;
                }
            }

            var parent = _parent as ISchedulerExceptionHandler ?? _scheduler;
            return parent.TryCatch(error, task);
        }

        /// <summary>
        /// Queries the current value of the performance counters.
        /// </summary>
        /// <param name="includeChildren">
        /// <c>true</c> to aggregate counters of the current object and all children;
        /// <c>false</c> to just return the counters for the current object.
        /// </param>
        /// <returns>The current value of the performance counters.</returns>
        public SchedulerPerformanceCounters QueryPerformanceCounters(bool includeChildren) => QueryPerformanceCounters(includeChildren, isTopLevel: true);

        /// <summary>
        /// Queries the current value of the performance counters.
        /// </summary>
        /// <param name="includeChildren">
        /// <c>true</c> to aggregate counters of the current object and all children;
        /// <c>false</c> to just return the counters for the current object.
        /// </param>
        /// <param name="isTopLevel">
        /// <c>true</c> when this call is made for the top-levle root scheduler;
        /// <c>false</c> when this call is made for a child scheduler.
        /// </param>
        /// <returns>The current value of the performance counters.</returns>
        private SchedulerPerformanceCounters QueryPerformanceCounters(bool includeChildren, bool isTopLevel)
        {
            var res = _counters.GetCounters(isTopLevel);

            if (includeChildren)
            {
                foreach (var child in _children)
                {
                    res += child.QueryPerformanceCounters(includeChildren, isTopLevel: false);
                }
            }

            return res;
        }

        /// <summary>
        /// Charges the execution of a task.
        /// </summary>
        /// <param name="cycles">The number of thread cycles to charge.</param>
        /// <param name="ticks">The number of <see cref="Stopwatch"/> ticks to charge.</param>
        void IAccountable.ChargeTaskExecution(ulong cycles, long ticks) => _counters.ChargeTaskExecution(cycles, ticks);

        /// <summary>
        /// Charges a single timer tick upon a timer task being transferred to the ready queue.
        /// </summary>
        void IAccountable.ChargeTimerTick() => _counters.ChargeTimerTick();

        /// <summary>
        /// This scheduler is being used at the moment to prevent us from running into exceptions in case we have race conditions between Dispose and CreateChildScheduler paths.
        /// We might need to revisit this in the future after we have understood all the scenarios where races might occur.
        /// </summary>
        private sealed class NopScheduler : IScheduler
        {
            private NopScheduler() { }

            public static NopScheduler Instance { get; } = new NopScheduler();

            public DateTimeOffset Now => DateTimeOffset.UtcNow;

            public IScheduler CreateChildScheduler() => this;

            public void Schedule(ISchedulerTask task) { }

            public void Schedule(TimeSpan dueTime, ISchedulerTask task) { }

            public void Schedule(DateTimeOffset dueTime, ISchedulerTask task) { }

            public Task PauseAsync() => Task.FromResult(0);

            public void Continue() { }

            public void RecalculatePriority() { }

            public bool CheckAccess() => true;

            public void VerifyAccess() { }

            public event EventHandler<SchedulerUnhandledExceptionEventArgs> UnhandledException
            {
                add { }
                remove { }
            }

            public void Dispose() { }
        }
    }
}
