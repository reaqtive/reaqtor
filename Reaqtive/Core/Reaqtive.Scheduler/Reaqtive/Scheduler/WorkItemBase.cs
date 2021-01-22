// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading;

namespace Reaqtive.Scheduler
{
    /// <summary>
    /// Represents a work item using <typeparamref name="TTime"/> to represent due time.
    /// </summary>
    /// <typeparam name="TTime">The type used to represent due time. This type should implement <see cref="IComparable{TTime}"/>.</typeparam>
    public class WorkItemBase<TTime> : IComparable<WorkItemBase<TTime>>, IWorkItem<TTime>
        where TTime : IComparable<TTime>
    {
        /// <summary>
        /// Atomic flag indicating whether the work item is paused.
        /// </summary>
        private int _isPaused;

        /// <summary>
        /// Should be called when the work item has been completed.
        /// </summary>
        private readonly IDisposable _onCompleted;

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkItemBase{TTime}" /> class.
        /// </summary>
        /// <param name="scheduler">The logical scheduler that owns the work item.</param>
        /// <param name="task">The task to execute.</param>
        /// <param name="dueTime">The due time at which to execute the task.</param>
        /// <param name="onCompleted">Resource to dispose when the task has finished executing.</param>
        public WorkItemBase(IScheduler scheduler, ISchedulerTask task, TTime dueTime, IDisposable onCompleted)
        {
            Scheduler = scheduler ?? throw new ArgumentNullException(nameof(scheduler));
            Task = task ?? throw new ArgumentNullException(nameof(task));
            DueTime = dueTime;
            _onCompleted = onCompleted ?? throw new ArgumentNullException(nameof(onCompleted));
        }

        /// <summary>
        /// Gets the task to execute.
        /// </summary>
        public ISchedulerTask Task { get; }

        /// <summary>
        /// Gets the logical scheduler that owns the work item.
        /// </summary>
        public IScheduler Scheduler { get; }

        /// <summary>
        /// Gets or sets the due time at which to execute the task.
        /// </summary>
        public TTime DueTime { get; set; }

        /// <summary>
        /// Gets the priority at which the task should be scheduled to run.
        /// </summary>
        public long Priority => Task.Priority;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is paused.
        /// </summary>
        public bool IsPaused
        {
            get => Interlocked.CompareExchange(ref _isPaused, 1, 1) == 1;
            set => Interlocked.Exchange(ref _isPaused, value ? 1 : 0);
        }

        /// <summary>
        /// Gets a value indicating whether this instance is runnable.
        /// </summary>
        public bool IsRunnable => !IsPaused && Task.IsRunnable;

        /// <summary>
        /// Compares the current object with another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has the following meanings:
        /// - Less than zero. This object is less than <paramref name="other" />.
        /// - Zero. This object is equal to <paramref name="other" />.
        /// - Greater than zero. This object is greater than <paramref name="other" />.
        /// </returns>
        public int CompareTo(WorkItemBase<TTime> other)
        {
            if (other == null)
            {
                return 1;
            }

            int c = DueTime.CompareTo(other.DueTime);
            if (c != 0)
            {
                return c;
            }

            return Priority.CompareTo(other.Priority);
        }

        /// <summary>
        /// Recalculates the priority.
        /// </summary>
        public void RecalculatePriority() => Task.RecalculatePriority();

        /// <summary>
        /// Runs this instance through the logical scheduler.
        /// </summary>
        /// <returns>Flag indicating whether the item has completed.</returns>
        public bool Invoke() => Invoke(yieldToken: default);

        /// <summary>
        /// Runs this instance through the logical scheduler.
        /// </summary>
        /// <param name="yieldToken">Token to observe for yield requests.</param>
        /// <returns>Flag indicating whether the item has completed.</returns>
        public bool Invoke(YieldToken yieldToken)
        {
            using (Scheduler.Account())
            using (Task.Account())
            {
                //
                // Termination and disposal of the task upon a failure is by design. When the
                // task cannot complete its invocation at time t0, we should not run it at t1
                // where it may have broken invariants. If the task wants to survive failures
                // it should guard against exceptional cases on its own.
                //
                bool isCompleted = true;
                try
                {
                    if (Task is IYieldableSchedulerTask yieldableTask)
                    {
                        isCompleted = yieldableTask.Execute(Scheduler, yieldToken);
                    }
                    else
                    {
                        isCompleted = Task.Execute(Scheduler);
                    }
                }
                catch (Exception ex) when (Scheduler is LogicalScheduler logicalScheduler)
                {
                    logicalScheduler.TraceSource.WorkItemBase_ExecutionException(ex);
                    throw;
                }
                finally
                {
                    if (isCompleted)
                    {
                        OnCompleted();
                    }
                }

                return isCompleted;
            }
        }

        /// <summary>
        /// Called when the work item completes.
        /// </summary>
        protected virtual void OnCompleted() => _onCompleted.Dispose();

        /// <summary>
        /// Signals that the work item representing a timer operation is made ready by the
        /// parent worker, enabling accounting operations.
        /// </summary>
        internal void TimerReady() => Scheduler.ChargeTimerTick();
    }
}
