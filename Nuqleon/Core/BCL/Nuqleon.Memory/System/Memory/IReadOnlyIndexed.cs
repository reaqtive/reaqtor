// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 10/27/2014 - Created bundle functionality.
//

namespace System.Memory
{
    /// <summary>
    /// Interface for indexable objects.
    /// </summary>
    public interface IReadOnlyIndexed
    {
        /// <summary>
        /// Gets the element at the specified index.
        /// </summary>
        /// <param name="index">Index of the element to retrieve.</param>
        /// <returns>The element at the specified index.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the specified index falls out of the boundaries of the object.</exception>
        object this[int index] { get; }
    }
}
