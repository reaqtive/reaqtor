// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Concurrent;

using Reaqtive.Scheduler;

namespace Test.Reaqtive.Scheduler.Tasks
{
    internal sealed class MessageQueueBasedTask<T> : ISchedulerTask
    {
        private readonly ConcurrentQueue<int> _queue;
        private readonly int _batchSize;
        private readonly Func<IScheduler, ConcurrentQueue<int>, int, T, bool> _action;

        public MessageQueueBasedTask(ConcurrentQueue<int> queue, int batchSize, T state, Func<IScheduler, ConcurrentQueue<int>, int, T, bool> action)
        {
            _queue = queue;
            _batchSize = batchSize;
            _action = action;
            State = state;
        }

        public long Priority { get; private set; }

        public bool IsRunnable => !_queue.IsEmpty;

        public T State { get; private set; }

        public bool Execute(IScheduler scheduler) => _action(scheduler, _queue, _batchSize, State);

        public void RecalculatePriority()
        {
            var count = _queue.Count;

            Priority = (count == 0 ? long.MinValue : _batchSize / count);
        }
    }
}
