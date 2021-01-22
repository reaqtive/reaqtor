// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;
using System.Threading;

namespace Reaqtive.Scheduler
{
    /// <summary>
    /// Represents a single scheduler worker thread.
    /// </summary>
    internal sealed class WorkerThread : IDisposable
    {
        /// <summary>
        /// The worker thread.
        /// </summary>
        private readonly Thread _workerThread;

        /// <summary>
        /// Event indicating whether there is some work available - command or recalculation.
        /// </summary>
        private readonly MonitorAutoResetEvent _workExists;

        /// <summary>
        /// The work processing function.
        /// </summary>
        private readonly Action _workProcessingFunction;

        /// <summary>
        /// Flag indicating whether the thread should run or quit.
        /// </summary>
        private volatile bool _shouldStop;

        /// <summary>
        /// Thread-local canary to reveal the thread as a worker.
        /// </summary>
        private readonly ThreadLocal<bool> _canary;

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkerThread" /> class.
        /// </summary>
        /// <param name="workProcessingFunction">The work processing function.</param>
        /// <param name="name">The name.</param>
        public WorkerThread(Action workProcessingFunction, string name)
        {
            Debug.Assert(workProcessingFunction != null, "Worker thread should have a worker delegate.");
            Debug.Assert(!string.IsNullOrEmpty(name), "Worker thread should have a name.");

            _shouldStop = false;
            _canary = new ThreadLocal<bool>();
            _workProcessingFunction = workProcessingFunction;
            _workerThread = new Thread(WorkLoop) { Name = name };
            _workExists = new MonitorAutoResetEvent();
            _workerThread.Start();
        }

        /// <summary>
        /// Gets a value indicating whether the worker thread should terminate.
        /// </summary>
        public bool ShouldStop => _shouldStop;

        /// <summary>
        /// Can be used to initiate new iteration of work processing.
        /// </summary>
        public void WorkExists() => _workExists.Set();

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _shouldStop = true;
            WorkExists();
            _workerThread.Join();
            _canary.Dispose();
        }

        /// <summary>
        /// Main work loop.
        /// </summary>
        private void WorkLoop()
        {
            _canary.Value = true;

            while (!_shouldStop)
            {
                _workExists.Wait();
                _workProcessingFunction();
            }
        }

        /// <summary>
        /// Determines whether the calling thread is the thread associated with this worker thread.
        /// </summary>
        /// <returns><c>true</c> if the calling thread is the thread associated with this worker thread; otherwise, <c>false</c>.</returns>
        public bool CheckAccess() => _canary.Value;
    }
}
