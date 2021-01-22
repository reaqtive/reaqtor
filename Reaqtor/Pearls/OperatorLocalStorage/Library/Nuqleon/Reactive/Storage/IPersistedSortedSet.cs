// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2018
//

using System.Collections.Generic;

namespace Reaqtive.Storage
{
    /// <summary>
    /// Interface representing a persisted sorted set.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the persisted sorted set.</typeparam>
    /// <remarks>This interface type does not inherit from <see cref="IPersistedSet{T}"/> in order to guarantee type uniqueness per persisted artifact.</remarks>
    public interface IPersistedSortedSet<T> : ISortedSet<T>, IReadOnlyCollection<T>, IPersisted
    {
        /// <summary>
        /// Gets the number of elements in the set.
        /// </summary>
        new int Count { get; }
    }
}
