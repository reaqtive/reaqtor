// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Threading;

namespace Reaqtive.Scheduler
{
    /// <summary>
    /// Auto reset event implemented using monitors.
    /// </summary>
    /// <remarks>
    /// In general this works faster than native events but it does not have the same degree
    /// of flexibility. For example, operations such as <c>WaitAll</c> and <c>WaitAny</c> are
    /// not supported.
    /// </remarks>
    internal sealed class MonitorAutoResetEvent
    {
        /// <summary>
        /// The thread safety gate.
        /// </summary>
        private readonly object _lock = new();

        /// <summary>
        /// Flag indicating whether the monitor is currently set.
        /// </summary>
        private bool _isSet;

        /// <summary>
        /// Initializes a new instance of the <see cref="MonitorAutoResetEvent" /> class.
        /// </summary>
        /// <param name="isSet">A flag indicating whether the new instance should have the monitor initially set.</param>
        public MonitorAutoResetEvent(bool isSet = false)
        {
            _isSet = isSet;
        }

        /// <summary>
        /// Sets the event.
        /// </summary>
        public void Set()
        {
            lock (_lock)
            {
                _isSet = true;
                Monitor.Pulse(_lock);
            }
        }

        /// <summary>
        /// Waits on the event.
        /// </summary>
        public void Wait()
        {
            lock (_lock)
            {
                while (!_isSet)
                {
                    Monitor.Wait(_lock);
                }

                _isSet = false;
            }
        }
    }
}
