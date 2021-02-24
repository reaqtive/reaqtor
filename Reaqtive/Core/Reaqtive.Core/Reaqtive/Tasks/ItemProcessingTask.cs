// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;

using Reaqtive.Scheduler;

namespace Reaqtive.Tasks
{
    /// <summary>
    /// Task that can process items using an <see cref="IItemProcessor"/>.
    /// </summary>
    public sealed class ItemProcessingTask : ISchedulerTask
    {
        private const int TaskPriority = 2;
        private const int BatchSize = 128;

        private readonly IItemProcessor _processor;

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemProcessingTask"/> class.
        /// </summary>
        /// <param name="processor">The processor.</param>
        public ItemProcessingTask(IItemProcessor processor)
        {
            _processor = processor ?? throw new ArgumentNullException(nameof(processor));
        }

        /// <summary>
        /// Gets task priority.
        /// </summary>
        public long Priority => TaskPriority;

        /// <summary>
        /// Gets a value indicating whether the task is runnable.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is runnable; otherwise, <c>false</c>.
        /// </value>
        public bool IsRunnable => _processor.ItemCount != 0;

        /// <summary>
        /// Executes the task.
        /// </summary>
        /// <param name="scheduler">The scheduler.</param>
        /// <returns><c>true</c> if the task has been completed; otherwise, <c>false</c>.</returns>
        public bool Execute(IScheduler scheduler)
        {
            Debug.Assert(scheduler != null, "Scheduler should not be null.");

            _processor.Process(BatchSize);

            return false;
        }

        /// <summary>
        /// Recalculates the priority of the task. The task can become runnable
        /// as the result of this operation.
        /// </summary>
        public void RecalculatePriority()
        {
        }
    }
}
