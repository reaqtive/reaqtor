// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Reaqtive.Disposables;
using Reaqtive.Scheduler;

namespace Reaqtive.TestingFramework
{
    /// <summary>
    /// Virtual time scheduler based on priority queue.
    /// </summary>
    /// <typeparam name="TAbsolute">Absolute time representation type.</typeparam>
    /// <typeparam name="TRelative">Relative time representation type.</typeparam>
    public abstract class VirtualTimePhysicalScheduler<TAbsolute, TRelative> : VirtualTimePhysicalSchedulerBase<TAbsolute, TRelative>
        where TAbsolute : IComparable<TAbsolute>
    {
        private readonly HeapBasedPriorityQueue<IWorkItem<TAbsolute>> _ready;
        private readonly List<IWorkItem<TAbsolute>> _notReady;

        /// <summary>
        /// Initializes a new instance of the <see cref="VirtualTimePhysicalScheduler{TAbsolute, TRelative}"/> class.
        /// </summary>
        protected VirtualTimePhysicalScheduler() : this(Comparer<TAbsolute>.Default, default)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VirtualTimePhysicalScheduler{TAbsolute, TRelative}"/> class.
        /// </summary>
        /// <param name="comparer">The comparer used to sort work items.</param>
        /// <param name="initialClock">The initial clock value.</param>
        protected VirtualTimePhysicalScheduler(IComparer<TAbsolute> comparer, TAbsolute initialClock)
            : base(comparer, initialClock)
        {
            _ready = new HeapBasedPriorityQueue<IWorkItem<TAbsolute>>(Comparer<IWorkItem<TAbsolute>>.Create(Compare));
            _notReady = new List<IWorkItem<TAbsolute>>();
        }

        /// <summary>
        /// Schedules a task to be executed at the specified due time.
        /// </summary>
        /// <param name="dueTime">Absolute time at which to execute the task.</param>
        /// <param name="task">The task to execute.</param>
        /// <param name="scheduler">The scheduler to execute the task on.</param>
        /// <returns>A scheduled work item.</returns>
        public override IWorkItem<TAbsolute> ScheduleAbsolute(TAbsolute dueTime, ISchedulerTask task, IScheduler scheduler)
        {
            if (task == null)
                throw new ArgumentNullException(nameof(task));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            var item = new WorkItemBase<TAbsolute>(scheduler, task, dueTime, Disposable.Empty);

            item.RecalculatePriority();

            if (item.IsRunnable)
            {
                _ready.Enqueue(item);
            }
            else
            {
                _notReady.Add(item);
            }

            return item;
        }

        /// <summary>
        /// Pauses the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>A pause task.</returns>
        public override Task PauseAsync(IWorkItem<TAbsolute> item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            item.IsPaused = true;

            if (_ready.Contains(item))
            {
                _ready.Remove(item);
                _notReady.Add(item);
            }

            return Task.FromResult(true);
        }

        /// <summary>
        /// Continues execution of the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        public override void Continue(IWorkItem<TAbsolute> item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            item.IsPaused = false;

            if (_notReady.Contains(item))
            {
                _notReady.Remove(item);
                _ready.Enqueue(item);
            }
        }

        /// <summary>
        /// Cancels the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        public override void Cancel(IWorkItem<TAbsolute> item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            _ready.Remove(item);
            _notReady.Remove(item);
        }

        /// <summary>
        /// Recalculates the priority of the item.
        /// </summary>
        /// <param name="item">The item.</param>
        public override void RecalculatePriority(IWorkItem<TAbsolute> item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            item.RecalculatePriority();

            if (_notReady.Contains(item))
            {
                _notReady.Remove(item);
                _ready.Enqueue(item);
            }
            else if (_ready.Contains(item))
            {
                // Need to change the places of the item according to the priority
                _ready.Remove(item);
                _ready.Enqueue(item);
            }
        }

        /// <summary>
        /// Dequeues next item to execute.
        /// </summary>
        /// <returns>Work item.</returns>
        protected override IWorkItem<TAbsolute> DequeueItem()
        {
            IWorkItem<TAbsolute> item = null;

            while (_ready.Count != 0)
            {
                item = _ready.Dequeue();
                if (!item.IsRunnable)
                {
                    _notReady.Add(item);
                }
                else
                {
                    break;
                }
            }

            return item;
        }

        private static int Compare(IWorkItem<TAbsolute> a, IWorkItem<TAbsolute> b)
        {
            int c = a.DueTime.CompareTo(b.DueTime);

            if (c != 0)
            {
                return c;
            }

            return a.Priority.CompareTo(b.Priority);
        }
    }
}
