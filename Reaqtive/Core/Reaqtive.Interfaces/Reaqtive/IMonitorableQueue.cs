// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1003 // Non-default events.
#pragma warning disable CA1711 // Name ends with Queue.

namespace Reaqtive
{
    /// <summary>
    /// A queue with events for enqueue and dequeue operations for monitoring.
    /// </summary>
    /// <typeparam name="T">The type of items in the queue.</typeparam>
    public interface IMonitorableQueue<out T>
    {
        /// <summary>
        /// Event raised before dequeueing. The argument is the enqueued item.
        /// </summary>
        event Action<T> Enqueueing;

        /// <summary>
        /// Event raised before dequeueing. The argument is the dequeued item.
        /// </summary>
        event Action<T> Dequeued;

        /// <summary>
        /// The size of the queue.
        /// </summary>
        int QueueSize { get; }
    }
}
