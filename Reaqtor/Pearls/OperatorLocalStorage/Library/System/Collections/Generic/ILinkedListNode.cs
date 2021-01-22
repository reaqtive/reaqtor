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
    /// Interface representing a node in a linked list.
    /// </summary>
    /// <typeparam name="T">The type of the value stored in the linked list node.</typeparam>
    public interface ILinkedListNode<T> : IReadOnlyLinkedListNode<T>
    {
        /// <summary>
        /// Gets or sets the value contained in the node.
        /// </summary>
        new T Value { get; set; }

        /// <summary>
        /// Gets the linked list the node belongs to.
        /// </summary>
        new ILinkedList<T> List { get; }

        /// <summary>
        /// Gets the previous node in the linked list.
        /// </summary>
        new ILinkedListNode<T> Previous { get; }

        /// <summary>
        /// Gets the next node in the linked list.
        /// </summary>
        new ILinkedListNode<T> Next { get; }
    }
}
