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
    /// Interface representing a linked list.
    /// </summary>
    /// <typeparam name="T">The type of the elements stored in the linked list.</typeparam>
    public interface ILinkedList<T> : IReadOnlyLinkedList<T>, ICollection<T>
    {
        /// <summary>
        /// Gets the number of elements in the linked list.
        /// </summary>
        new int Count { get; }

        /// <summary>
        /// Gets the first node of the linked list, or <c>null</c> if the linked list is empty.
        /// </summary>
        new ILinkedListNode<T> First { get; }

        /// <summary>
        /// Gets the last node of the linked list, or <c>null</c> if the linked list is empty.
        /// </summary>
        new ILinkedListNode<T> Last { get; }

        /// <summary>
        /// Adds a new node containing the specified <paramref name="value"/> after the specified existing <paramref name="node"/> in the linked list.
        /// </summary>
        /// <param name="node">The node after which to insert a new node containing <paramref name="value"/>.</param>
        /// <param name="value">The value to add to the linked list.</param>
        /// <returns>The new node containing <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="node"/> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="node"/> is not in the current linked list.</exception>
        ILinkedListNode<T> AddAfter(ILinkedListNode<T> node, T value);

        /// <summary>
        /// Adds the specified new node after the specified existing <paramref name="node"/> in the linked list.
        /// </summary>
        /// <param name="node">The node after which to insert <paramref name="newNode"/>.</param>
        /// <param name="newNode">The new node to add to the linked list.</param>
        /// <exception cref="ArgumentNullException"><paramref name="node"/> is <c>null</c>, or <paramref name="newNode"/> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="node"/> is not in the current linked list,  or <paramref name="newNode"/> belongs to another linked list.</exception>
        void AddAfter(ILinkedListNode<T> node, ILinkedListNode<T> newNode);

        /// <summary>
        /// Adds a new node containing the specified <paramref name="value"/> before the specified existing <paramref name="node"/> in the linked list.
        /// </summary>
        /// <param name="node">The node before which to insert a new node containing <paramref name="value"/>.</param>
        /// <param name="value">The value to add to the linked list.</param>
        /// <returns>The new node containing <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="node"/> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="node"/> is not in the current linked list.</exception>
        ILinkedListNode<T> AddBefore(ILinkedListNode<T> node, T value);

        /// <summary>
        /// Adds the specified new node before the specified existing <paramref name="node"/> in the linked list.
        /// </summary>
        /// <param name="node">The node before which to insert <paramref name="newNode"/>.</param>
        /// <param name="newNode">The new node to add to the linked list.</param>
        /// <exception cref="ArgumentNullException"><paramref name="node"/> is <c>null</c>, or <paramref name="newNode"/> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="node"/> is not in the current linked list,  or <paramref name="newNode"/> belongs to another linked list.</exception>
        void AddBefore(ILinkedListNode<T> node, ILinkedListNode<T> newNode);

        /// <summary>
        /// Adds a new node containing the specified <paramref name="value"/> at the start of the linked list.
        /// </summary>
        /// <param name="value">The value to add at the start of the linked list.</param>
        /// <returns>The new node containing the <paramref name="value"/>.</returns>
        ILinkedListNode<T> AddFirst(T value);

        /// <summary>
        /// Adds the specified new <paramref name="node"/> at the start of the linked list.
        /// </summary>
        /// <param name="node">The new node to add at the start of the linked list.</param>
        /// <exception cref="ArgumentNullException"><paramref name="node"/> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="node"/> belongs to another linked list.</exception>
        void AddFirst(ILinkedListNode<T> node);

        /// <summary>
        /// Adds a new node containing the specified <paramref name="value"/> at the end of the linked list.
        /// </summary>
        /// <param name="value">The value to add at the end of the linked list.</param>
        /// <returns>The new node containing the <paramref name="value"/>.</returns>
        ILinkedListNode<T> AddLast(T value);

        /// <summary>
        /// Adds the specified new <paramref name="node"/> at the end of the linked list.
        /// </summary>
        /// <param name="node">The new node to add at the end of the linked list.</param>
        /// <exception cref="ArgumentNullException"><paramref name="node"/> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="node"/> belongs to another linked list.</exception>
        void AddLast(ILinkedListNode<T> node);

        //
        // REVIEW: It's unfortunate that Find methods can't be defined on the read-only collection type due to the covariance requirement.
        //         Should we have extension methods to provide these on the read-only collection type as well?
        //

        /// <summary>
        /// Finds the first node that contains the specified <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The value to locate in the linked list.</param>
        /// <returns>The first node that contains the specified <paramref name="value"/>, if found; otherwise, <c>null</c>.</returns>
        ILinkedListNode<T> Find(T value);

        /// <summary>
        /// Finds the last node that contains the specified <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The value to locate in the linked list.</param>
        /// <returns>The last node that contains the specified <paramref name="value"/>, if found; otherwise, <c>null</c>.</returns>
        ILinkedListNode<T> FindLast(T value);

        /// <summary>
        /// Removes the specified <paramref name="node"/> from the linked list.
        /// </summary>
        /// <param name="node">The node to remove from the linked list.</param>
        /// <exception cref="ArgumentNullException"><paramref name="node"/> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="node"/> is not in the current list.</exception>
        void Remove(ILinkedListNode<T> node);

        /// <summary>
        /// Removes the node at the start of the linked list.
        /// </summary>
        /// <exception cref="InvalidOperationException">The linked list is empty.</exception>
        void RemoveFirst();

        /// <summary>
        /// Removes the node at the end of the linked list.
        /// </summary>
        /// <exception cref="InvalidOperationException">The linked list is empty.</exception>
        void RemoveLast();
    }
}
