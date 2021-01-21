// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 08/03/2015 - Created ValueOrError functionality.
//

namespace System.Memory
{
    /// <summary>
    /// Interface for objects that represent a value or an error.
    /// </summary>
    /// <typeparam name="T">Type of the value.</typeparam>
    public interface IValueOrError<out T>
    {
        /// <summary>
        /// Gets a value indicating whether this object represents a value or an error.
        /// </summary>
        ValueOrErrorKind Kind { get; }

        /// <summary>
        /// Gets the value represented by the object. If this object represents an error, the original error is thrown.
        /// </summary>
        T Value { get; }

        /// <summary>
        /// Gets the error represented by the object. If this object represents a value, an error is thrown.
        /// </summary>
        Exception Exception { get; }
    }
}
