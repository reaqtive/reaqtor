// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace Reaqtive.Scheduler
{
    using Command = KeyValuePair<Action<object>, object>;

    /// <summary>
    /// Represents a worker.
    /// </summary>
    internal sealed class Worker : IDisposable
    {
        /// <summary>
        /// Physical scheduler.
        /// </summary>
        private readonly PhysicalScheduler _scheduler;

        /// <summary>
        /// Worker name.
        /// </summary>
        private readonly string _name;

        /// <summary>
        /// Working thread.
        /// </summary>
        private readonly WorkerThread _thread;

        /// <summary>
        /// The commands.
        /// </summary>
        private readonly SnapshotCollection<List<Command>, Command> _commands;

        /// <summary>
        /// The priority recalculation set.
        /// </summary>
        private readonly SnapshotCollection<HashSet<WorkItem>, WorkItem> _priorityRecalculations;

        /// <summary>
        /// The ready tasks.
        /// </summary>
        private readonly HeapBasedPriorityQueue<WorkItem> _readyTasks;

        /// <summary>
        /// Not ready tasks.
        /// </summary>
        private readonly HashSet<WorkItem> _notReadyTasks;

        /// <summary>
        /// The time scheduled items.
        /// </summary>
        private readonly HeapBasedPriorityQueue<WorkItem> _timeScheduledItems;

        /// <summary>
        /// Cached delegate for the <see cref="AddItem(object)"/> method.
        /// </summary>
        private readonly Action<object> _addItem;

        /// <summary>
        /// Cached delegate for the <see cref="RemoveItem(object)"/> method.
        /// </summary>
        private readonly Action<object> _removeItem;

        /// <summary>
        /// Cached delegate for the <see cref="RecalculatePriority(object)"/> method.
        /// </summary>
        private readonly Action<object> _recalculatePriority;

        /// <summary>
        /// The active task count.
        /// </summary>
        private volatile int _activeTaskCount;

        /// <summary>
        /// Timer to the next item in the queue.
        /// </summary>
        private Timer _timer;

        /// <summary>
        /// Flag showing if the timer is running.
        /// </summary>
        private int _isTimerRunning;

        /// <summary>
        /// The next scheduled item to run.
        /// </summary>
        private WorkItem _nextItemToRun;

        /// <summary>
        /// The last exception that occurred on the scheduler, if any.
        /// </summary>
        private Exception _lastError;

        /// <summary>
        /// Initializes a new instance of the <see cref="Worker" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="scheduler">Physical scheduler the worker is associated with.</param>
        /// <param name="priority">The thread priority for the worker thread.</param>
        public Worker(string name, PhysicalScheduler scheduler, ThreadPriority priority)
        {
            _name = name;
            _scheduler = scheduler;

            _thread = new WorkerThread(WorkLoop, priority, name);
            _commands = new SnapshotCollection<List<Command>, Command>();
            _priorityRecalculations = new SnapshotCollection<HashSet<WorkItem>, WorkItem>();
            _readyTasks = new HeapBasedPriorityQueue<WorkItem>(Comparer<WorkItem>.Create((a, b) => a.Priority.CompareTo(b.Priority)));
            _notReadyTasks = new HashSet<WorkItem>();
            _timeScheduledItems = new HeapBasedPriorityQueue<WorkItem>(Comparer<WorkItem>.Create((a, b) => a.CompareTo(b)));

            _addItem = AddItem;
            _removeItem = RemoveItem;
            _recalculatePriority = RecalculatePriority;

            _timer = new Timer(Tick, state: null, Timeout.Infinite, Timeout.Infinite);
            _isTimerRunning = 0;
            _nextItemToRun = null;
        }

        /// <summary>
        /// Gets the active task count.
        /// </summary>
        public int ActiveTaskCount => _activeTaskCount;

        /// <summary>
        /// Handler for timer ticks.
        /// </summary>
        /// <param name="state">Ignored.</param>
        private void Tick(object state)
        {
            IsTimerRunning = false;
            _thread.WorkExists();
        }

        /// <summary>
        /// Gets or sets a value indicating whether the timer is running.
        /// </summary>
        /// <value>
        /// <c>true</c> if this the timer is running; otherwise, <c>false</c>.
        /// </value>
        private bool IsTimerRunning
        {
            get => Interlocked.CompareExchange(ref _isTimerRunning, 1, 1) == 1;

            set => Interlocked.Exchange(ref _isTimerRunning, value ? 1 : 0);
        }

        /// <summary>
        /// Adds the specified item to the worker to run.
        /// </summary>
        /// <param name="item">The item.</param>
        public void Add(WorkItem item)
        {
            Debug.Assert(item != null, "Work item is not allowed to be null.");

            EnqueueCommand(_addItem, item);
        }

        /// <summary>
        /// Implementation of the logic to process an <see cref="Add(WorkItem)"/> call.
        /// </summary>
        /// <param name="state">The state passed to the command, of type <see cref="WorkItem"/>.</param>
        private void AddItem(object state)
        {
            var item = (WorkItem)state;

            if (item.DueTime <= PhysicalScheduler.Now)
            {
                _readyTasks.Enqueue(item);
            }
            else
            {
                _timeScheduledItems.Enqueue(item);
            }
        }

        /// <summary>
        /// Removes the specified item from the worker.
        /// </summary>
        /// <param name="item">The item.</param>
        public void Remove(WorkItem item)
        {
            Debug.Assert(item != null, "Work item is not allowed to be null.");

            EnqueueCommand(_removeItem, item);
        }

        /// <summary>
        /// Implementation of the logic to process an <see cref="Remove(WorkItem)"/> call.
        /// </summary>
        /// <param name="state">The state passed to the command, of type <see cref="WorkItem"/>.</param>
        private void RemoveItem(object state)
        {
            var item = (WorkItem)state;

            _readyTasks.Remove(item);
            _notReadyTasks.Remove(item);

            // even if it is not a timer, we do not really care.
            _timeScheduledItems.Remove(item);
        }

        /// <summary>
        /// Recalculates the item's priority.
        /// </summary>
        /// <param name="item">The item.</param>
        public void RecalculatePriority(WorkItem item)
        {
            Debug.Assert(item != null, "Work item is not allowed to be null.");

            bool first = _priorityRecalculations.Add(item);
            if (first)
            {
                _thread.WorkExists();
            }
        }

        /// <summary>
        /// Recalculates the item's priority and calls the callback.
        /// Main usage is for continue/pause.
        /// </summary>
        /// <param name="items">The item.</param>
        /// <param name="callback">The callback.</param>
        public void RecalculatePriority(List<WorkItem> items, Action callback)
        {
            Debug.Assert(items != null);
            Debug.Assert(callback != null);

            EnqueueCommand(_recalculatePriority, Tuple.Create(items, callback));
        }

        /// <summary>
        /// Implementation of the logic to process an <see cref="RecalculatePriority(List{WorkItem}, Action)"/> call.
        /// </summary>
        /// <param name="state">The state passed to the command, of type <c>Tuple{List{WorkItem}, Action}</c>.</param>
        private void RecalculatePriority(object state)
        {
            var innerTuple = (Tuple<List<WorkItem>, Action>)state;

            var items = innerTuple.Item1;
            var callback = innerTuple.Item2;

            foreach (var workItem in items)
            {
                RecalculatePriorityFor(workItem);
            }

            callback();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            //
            // NB: The ordering of the using statements should not be changed without consideration.
            //
            //     Disposal of `_thread` must occur before disposal of `_timer`. In order to achieve
            //     this, the using statement for `_thread` must come after the using statement of `_timer`.
            //

            using (_timer)
            using (_thread)
            {
            }

            _timer = null;
        }

        /// <summary>
        /// Main work loop. Is called by work thread as soon as there exists some work.
        /// </summary>
        private void WorkLoop()
        {
            do
            {
                ProcessCommands();
                RecalculatePriorities();
                ProcessTimers();
                _activeTaskCount = _readyTasks.Count;

                if (_readyTasks.Count == 0)
                {
                    break;
                }

                WorkItem task = _readyTasks.Dequeue();
                bool isCompleted = false;
                var yieldToken = default(YieldToken);

                if (task.IsRunnable)
                {
                    //
                    // CONSIDER: The concept of yield tokens could be used to implement cooperative
                    //           scheduling where a time-based quantum can trigger the yield token
                    //           to be set. We can consider this as a complement to the count-based
                    //           approach in ItemProcessingTask.
                    //

                    if (task.Scheduler is IYieldTokenSource yieldTokenSource)
                    {
                        yieldToken = yieldTokenSource.Token;
                    }

                    try
                    {
                        isCompleted = task.Invoke(yieldToken);
                    }
                    catch (Exception e)
                    {
                        if (e is OutOfMemoryException or StackOverflowException or ThreadAbortException)
                        {
                            Environment.FailFast("Unexpected thread termination.");
                        }

                        if (!OnUnhandledException(e, task))
                        {
                            throw;
                        }

                        isCompleted = true;
                    }
                }

                if (!isCompleted)
                {
                    task.RecalculatePriority();
                    if (task.IsRunnable && !yieldToken.IsYieldRequested)
                    {
                        _scheduler.Schedule(task);
                    }
                    else
                    {
                        _notReadyTasks.Add(task);
                    }
                }
            }
            while (!_thread.ShouldStop);
        }

        /// <summary>
        /// Enqueues a command constructed from the specified <paramref name="action"/> and <paramref name="state"/>.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        /// <param name="state">The state to pass to the action.</param>
        private void EnqueueCommand(Action<object> action, object state)
        {
            EnqueueCommand(new Command(action, state));
        }

        /// <summary>
        /// Enqueues the specified <paramref name="command"/>.
        /// </summary>
        /// <param name="command">The command to enqueue.</param>
        private void EnqueueCommand(Command command)
        {
            bool firstElement = _commands.Add(command);
            if (firstElement)
            {
                _thread.WorkExists();
            }
        }

        /// <summary>
        /// Processes the commands.
        /// </summary>
        private void ProcessCommands()
        {
            var snapshot = _commands.Snapshot();

            foreach (var command in snapshot)
            {
                command.Key(command.Value);
            }

            //
            // NB: We need to clear this snapshot collection, otherwise its items will be kept in memory
            //     until the next round of processing the tasks comes in.
            //

            snapshot.Clear();
        }

        /// <summary>
        /// Recalculates the priorities.
        /// </summary>
        private void RecalculatePriorities()
        {
            foreach (var item in _priorityRecalculations.Snapshot())
            {
                RecalculatePriorityFor(item);
            }
        }

        /// <summary>
        /// Recalculates the priority for the task.
        /// </summary>
        /// <param name="task">The task.</param>
        private void RecalculatePriorityFor(WorkItem task)
        {
            Debug.Assert(task != null, "Task is not allowed to be null.");

            if (_readyTasks.Contains(task))
            {
                _readyTasks.Remove(task);
                task.RecalculatePriority();

                if (task.IsRunnable)
                {
                    _readyTasks.Enqueue(task);
                }
                else
                {
                    _notReadyTasks.Add(task);
                }
            }
            else if (_notReadyTasks.Contains(task))
            {
                task.RecalculatePriority();

                if (task.IsRunnable)
                {
                    _notReadyTasks.Remove(task);
                    _readyTasks.Enqueue(task);
                }
            }
        }

        /// <summary>
        /// Processes the timer queue: moves expired items to the active queue.
        /// </summary>
        private void ProcessTimers()
        {
            while (_timeScheduledItems.Count > 0 &&
                   _timeScheduledItems.Peek().DueTime <= PhysicalScheduler.Now)
            {
                var item = _timeScheduledItems.Dequeue();
                item.TimerReady();
                _readyTasks.Enqueue(item);
            }

            if (_timeScheduledItems.Count == 0)
            {
                return;
            }

            var first = _timeScheduledItems.Peek();
            if (_nextItemToRun != first || !IsTimerRunning)
            {
                _nextItemToRun = first;
                var due = _nextItemToRun.DueTime - PhysicalScheduler.Now;
                var normalizedDue = NormalizeForTimer(due);

                IsTimerRunning = true;
                _timer.Change(normalizedDue, TimeSpan.FromMilliseconds(-1)); // no periods

                _scheduler.TraceSource.Worker_NextTimeScheduledItem(_name, PhysicalScheduler.Now + normalizedDue);
            }
        }

        /// <summary>
        /// Normalizes the given TimeSpan from use by a System.Threading.Timer such that it falls in the acceptable range.
        /// The resulting value may be rounded up to TimeSpan.Zero if it was negative, and may be rounded down to an upper
        /// limit if it's too high. The caller should not use the normalized timer to run work that's due at the specified
        /// time, but rather use it to re-evaluate the work and reschedule if needed.
        /// </summary>
        /// <param name="due">TimeSpan to normalize for use by a System.Threading.Timer.</param>
        /// <returns>Normalized TimeSpan safe for use in System.Threading.Timer methods.</returns>
        private static TimeSpan NormalizeForTimer(TimeSpan due)
        {
            if (due.Ticks < 0)
            {
                return TimeSpan.Zero;
            }

            // From System.Threading.Timer's code in ndp\clr\src\bcl\mscorlib. This is the maximum supported timeout, in ms
            // for timers as exposed by the BCL. The value is 49.17:02:47.2940000 in TimeSpan.ToString form.
            const uint MAX_SUPPORTED_TIMEOUT = 0xfffffffe;

            if ((long)due.TotalMilliseconds > MAX_SUPPORTED_TIMEOUT)
            {
                return TimeSpan.FromMilliseconds(MAX_SUPPORTED_TIMEOUT);
            }

            return due;
        }

        /// <summary>
        /// Determines whether the calling thread is the thread associated with this worker.
        /// </summary>
        /// <returns><c>true</c> if the calling thread is the thread associated with this worker; otherwise, <c>false</c>.</returns>
        public bool CheckAccess() => _thread.CheckAccess();

        /// <summary>
        /// Heartbeat processing method. Reevaluates the work queue.
        /// </summary>
        internal void Heartbeat()
        {
            // TODO: it might be useful to collect more metrics about the state of the queues,
            // e.g., using an enumerator over the queue that we can use to count the number of
            // priority 1 tasks vs. priority 2 tasks, etc.
            _scheduler.TraceSource.Worker_Heartbeat(_name, _readyTasks.Count, _notReadyTasks.Count, _timeScheduledItems.Count);

            _thread.WorkExists();
        }

        private bool OnUnhandledException(Exception exception, WorkItem task)
        {
            //
            // Keeping the last error around for post-mortem dump debugging.
            //
            // NB: Calling GC.KeepAlive to trick FxCop to not complain about the variable not beign used.
            //     GC.KeepAlive is an empty method; calling it doesn't have any effect other than creating
            //     a use site for its argument, so the variable looks like it's being "used".
            //

            _lastError = exception;
            GC.KeepAlive(_lastError);

            if (task.Scheduler is ISchedulerExceptionHandler handler)
            {
                return handler.TryCatch(exception, task);
            }

            return false;
        }
    }
}
