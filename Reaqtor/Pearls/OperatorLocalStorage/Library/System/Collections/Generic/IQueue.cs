// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2018
//

namespace System.Collections.Generic
{
    /// <summary>
    /// Interface representing a queue.
    /// </summary>
    /// <typeparam name="T">The type of the elements stored in the queue.</typeparam>
    public interface IQueue<T> : IReadOnlyCollection<T>
    {
        /// <summary>
        /// Removes and returns the object at the beginning of the queue.
        /// </summary>
        /// <returns>The object that is removed from the beginning of the queue.</returns>
        /// <exception cref="InvalidOperationException">The queue is empty.</exception>
        T Dequeue();

        /// <summary>
        /// Adds an object to the end of the queue.
        /// </summary>
        /// <param name="item">The object to add to the queue.</param>
        void Enqueue(T item);

        /// <summary>
        /// Returns the object at the beginning of the queue without removing it.
        /// </summary>
        /// <returns>The object at the beginning of the queue.</returns>
        /// <exception cref="InvalidOperationException">The queue is empty.</exception>
        T Peek();

        // REVIEW: Should we have a Clear operation?
        // NB: ICollection<T> is undesirable because of Contains and Remove.
    }
}
