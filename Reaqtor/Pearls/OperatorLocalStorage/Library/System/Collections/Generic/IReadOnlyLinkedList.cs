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
    /// Interface representing a read-only linked list.
    /// </summary>
    /// <typeparam name="T">The type of the elements stored in the linked list.</typeparam>
#pragma warning disable CA1710 // Identifiers should have correct suffix. List is an appropriate suffix.
    public interface IReadOnlyLinkedList<out T> : IReadOnlyCollection<T>
#pragma warning restore CA1710
    {
        /// <summary>
        /// Gets the first node of the linked list, or <c>null</c> if the linked list is empty.
        /// </summary>
        IReadOnlyLinkedListNode<T> First { get; }

        /// <summary>
        /// Gets the last node of the linked list, or <c>null</c> if the linked list is empty.
        /// </summary>
        IReadOnlyLinkedListNode<T> Last { get; }
    }
}
