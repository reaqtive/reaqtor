// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace Reaqtive.Scheduler
{
    /// <summary>
    /// Physical scheduler managing pool of workers.
    /// </summary>
    public sealed partial class PhysicalScheduler : IDisposable, ISchedulerExceptionHandler
    {
        /// <summary>
        /// The workers.
        /// </summary>
        private readonly List<Worker> _workers;

        /// <summary>
        /// The lazy worker index.
        /// </summary>
        private int _currentWorker;

        /// <summary>
        /// Flag to keep track of the disposed state of the scheduler.
        /// </summary>
        private int _isDisposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="PhysicalScheduler"/> class.
        /// </summary>
        /// <param name="numberOfWorkers">The number of workers.</param>
        /// <param name="priority">The thread priority for the worker threads.</param>
        private PhysicalScheduler(int numberOfWorkers, ThreadPriority priority)
        {
            Debug.Assert(numberOfWorkers > 0, "Number of workers should be greater than zero.");

            _workers = new List<Worker>(numberOfWorkers);
            _currentWorker = 0;

            for (int i = 0; i < numberOfWorkers; ++i)
            {
                _workers.Add(new Worker("Reaqtive.Scheduler.Worker" + i, this, priority));
            }

            StartHeartbeat();
        }

        /// <summary>
        /// Gets the current time.
        /// </summary>
        internal static DateTimeOffset Now => DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the trace source used to log scheduler events.
        /// </summary>
        public TraceSource TraceSource { get; set; }

        /// <summary>
        /// Creates an instance of the scheduler.
        /// </summary>
        /// <returns>A scheduler.</returns>
        public static PhysicalScheduler Create() => Create(Environment.ProcessorCount);

        /// <summary>
        /// Creates a physical scheduler with the specified number of workers.
        /// </summary>
        /// <param name="numberOfWorkers">The number of workers.</param>
        /// <param name="priority">The thread priority for the worker threads.</param>
        /// <returns>A scheduler.</returns>
        public static PhysicalScheduler Create(int numberOfWorkers, ThreadPriority priority = ThreadPriority.Normal)
        {
            if (numberOfWorkers <= 0)
                throw new ArgumentOutOfRangeException(nameof(numberOfWorkers));

            return new PhysicalScheduler(numberOfWorkers, priority);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (Interlocked.Exchange(ref _isDisposed, 1) == 0)
            {
                StopHeartbeat();

                _workers.ForEach(w => w.Dispose());
                _workers.Clear();

#if !NO_HEARTBEAT
                _heartbeatEvent.Dispose();
#endif
            }
        }

        /// <summary>
        /// Starts the heartbeat of the workers.
        /// </summary>
        partial void StartHeartbeat();

        /// <summary>
        /// Stops the heartbeat of the workers.
        /// </summary>
        partial void StopHeartbeat();

        /// <summary>
        /// Schedules the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        internal void Schedule(WorkItem item)
        {
            Debug.Assert(item != null, "Work item is not allowed to be null.");

            Worker lazy = FindLaziestWorker();
            item.Worker = lazy;
            lazy.Add(item);
        }

        /// <summary>
        /// Removes the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        internal static void Remove(WorkItem item)
        {
            Debug.Assert(item != null, "Work item is not allowed to be null.");

            if (item.Worker == null)
            {
                throw new ArgumentException("The worker should be set.", nameof(item));
            }

            Worker worker = item.Worker;
            worker.Remove(item);
        }

        /// <summary>
        /// Recalculates the priority.
        /// </summary>
        /// <param name="item">The item.</param>
        internal static void RecalculatePriority(WorkItem item)
        {
            Debug.Assert(item != null, "Work item is not allowed to be null.");

            if (item.Worker == null)
            {
                throw new ArgumentException("The worker should be set.", nameof(item));
            }

            Worker worker = item.Worker;
            worker.RecalculatePriority(item);
        }

        /// <summary>
        /// Finds a potentially laziest worker.
        /// </summary>
        /// <returns>Laziest worker.</returns>
        private Worker FindLaziestWorker()
        {
            // AcitveTaskCount can change even during iteration, but we do not care about this too much.
            // We are just trying to avoid rebalance if possible.
            int nextWorkerIndex = Interlocked.Increment(ref _currentWorker) % _workers.Count;

            Worker lazy = _workers[nextWorkerIndex];
            foreach (var worker in _workers)
            {
                if (worker.ActiveTaskCount < lazy.ActiveTaskCount)
                {
                    lazy = worker;
                }
            }

            return lazy;
        }

        /// <summary>
        /// Determines whether the calling thread is one the threads associated with this scheduler.
        /// </summary>
        /// <returns><c>true</c> if the calling thread is one the threads associated with this scheduler; otherwise, <c>false</c>.</returns>
        public bool CheckAccess() => _workers.Any(w => w.CheckAccess());

        /// <summary>
        /// Determines whether the calling thread has access to the scheduler.
        /// </summary>
        /// <exception cref="InvalidOperationException">The calling thread does not have access to this scheduler.</exception>
        public void VerifyAccess()
        {
            if (!CheckAccess())
            {
                throw new InvalidOperationException("The calling thread does not have access to this scheduler.");
            }
        }

        /// <summary>
        /// Event to observe unhandled exceptions and optionally mark them as handled.
        /// </summary>
        public event EventHandler<SchedulerUnhandledExceptionEventArgs> UnhandledException;

        /// <summary>
        /// Tries to handle an exception that was thrown by a work item running on the scheduler.
        /// </summary>
        /// <param name="error">Exception to handle.</param>
        /// <param name="task">Task that threw the exception.</param>
        /// <returns><c>true</c> if the exception was handled; otherwise, <c>false</c>.</returns>
        bool ISchedulerExceptionHandler.TryCatch(Exception error, IWorkItem task)
        {
            var handler = UnhandledException;
            if (handler != null)
            {
                var e = new SchedulerUnhandledExceptionEventArgs(task?.Scheduler, error);
                handler(this, e);
                return e.Handled;
            }

            return false;
        }
    }

#if !NO_HEARTBEAT
    public partial class PhysicalScheduler
    {
        /// <summary>
        /// Heartbeat thread.
        /// </summary>
        private Thread _heartbeat;

        /// <summary>
        /// Event to signal heartbeat thread.
        /// </summary>
        private ManualResetEvent _heartbeatEvent;

        /// <summary>
        /// Sets up a heartbeat for the workers.
        /// </summary>
        partial void StartHeartbeat()
        {
            const int HEARTBEAT = 30000;

            _heartbeatEvent = new ManualResetEvent(false);

            _heartbeat = new Thread(() =>
            {
                var numberOfWorkers = _workers.Count;

                while (true)
                {
                    for (int i = 0; i < numberOfWorkers; ++i)
                    {
                        var worker = _workers[i];

                        _heartbeatEvent.WaitOne(HEARTBEAT / numberOfWorkers);

                        if (Volatile.Read(ref _isDisposed) == 1)
                            return;

                        worker.Heartbeat();
                    }
                }
            })
            { Priority = ThreadPriority.Highest };

            _heartbeat.Start();
        }

        /// <summary>
        /// Stops the heartbeat of the workers.
        /// </summary>
        partial void StopHeartbeat()
        {
            _heartbeatEvent.Set();
            _heartbeat.Join();
        }
    }
#endif
}
