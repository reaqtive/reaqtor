// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2018
//

namespace Reaqtive.Storage
{
    /// <summary>
    /// Struct representing a value or the absence of a value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal readonly struct Maybe<T>
    {
        /// <summary>
        /// Creates a new instance of <see cref="Maybe{T}"/> with the specified value.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public Maybe(T value)
        {
            HasValue = true;
            Value = value;
        }

        /// <summary>
        /// Gets a value indicating whether this instance contains a value in <see cref="Value"/>.
        /// </summary>
        public bool HasValue { get; }

        /// <summary>
        /// Gets the value contained by this instance in case <see cref="HasValue"/> is set to <c>true</c>; otherwise, returns the default value.
        /// </summary>
        public T Value { get; }
    }
}
