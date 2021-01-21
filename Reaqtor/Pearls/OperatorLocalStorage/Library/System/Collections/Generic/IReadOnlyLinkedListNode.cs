// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - February 2018
//

namespace System.Collections.Generic
{
    /// <summary>
    /// Interface representing a read-only node in a linked list.
    /// </summary>
    /// <typeparam name="T">The type of the value stored in the linked list node.</typeparam>
    public interface IReadOnlyLinkedListNode<out T>
    {
        /// <summary>
        /// Gets the value contained in the node.
        /// </summary>
        T Value { get; }

        /// <summary>
        /// Gets the linked list the node belongs to.
        /// </summary>
        IReadOnlyLinkedList<T> List { get; }

        /// <summary>
        /// Gets the previous node in the linked list.
        /// </summary>
        IReadOnlyLinkedListNode<T> Previous { get; }

        /// <summary>
        /// Gets the next node in the linked list.
        /// </summary>
        IReadOnlyLinkedListNode<T> Next { get; }
    }
}
