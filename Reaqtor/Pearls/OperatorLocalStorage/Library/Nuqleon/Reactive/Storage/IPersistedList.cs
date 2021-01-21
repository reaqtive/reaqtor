// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2018
//

using System; // NB: Used for XML doc comments.
using System.Collections.Generic;

namespace Reaqtive.Storage
{
    /// <summary>
    /// Interface representing a persisted list.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the persisted list.</typeparam>
    public interface IPersistedList<T> : IList<T>, IReadOnlyList<T>, IPersisted
    {
        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element to get or set.</param>
        /// <returns>The element at the specified index.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index in the list.</exception>
        new T this[int index] { get; set; }

        /// <summary>
        /// Gets the number of elements contained in the list.
        /// </summary>
        new int Count { get; }
    }
}
