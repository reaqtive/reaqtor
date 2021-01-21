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
    /// Interface representing a fixed-size array.
    /// </summary>
    /// <typeparam name="T">The type of the elements stored in the array.</typeparam>
    public interface IArray<T> : IReadOnlyList<T>
    {
        /// <summary>
        /// Gets the length of the array.
        /// </summary>
        int Length { get; }

        /// <summary>
        /// Gets or sets the element at the specified <paramref name="index"/> in the array.
        /// </summary>
        /// <param name="index">The index in the array.</param>
        /// <returns>The element at the specified <paramref name="index"/>.</returns>
        /// <exception cref="IndexOutOfRangeException">The specified <paramref name="index"/> is outside the bounds of the array.</exception>
        new T this[int index] { get; set; }
    }
}
