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
    /// Interface representing a stack.
    /// </summary>
    /// <typeparam name="T">The type of the elements stored in the queue.</typeparam>
    public interface IStack<T> : IReadOnlyCollection<T>
    {
        /// <summary>
        /// Removes and returns the object at the top of the stack.
        /// </summary>
        /// <returns>The object removed from the top of the stack.</returns>
        /// <exception cref="InvalidOperationException">The stack is empty.</exception>
        T Pop();

        /// <summary>
        /// Inserts an object at the top of the stack.
        /// </summary>
        /// <param name="value">The object to push onto the stack.</param>
        void Push(T value);

        /// <summary>
        /// Returns the object at the top of the stack without removing it.
        /// </summary>
        /// <returns>The object at the top of the stack.</returns>
        /// <exception cref="InvalidOperationException">The stack is empty.</exception>
        T Peek();

        // REVIEW: Should we have a Clear operation?
        // NB: ICollection<T> is undesirable because of Contains and Remove.
    }
}
