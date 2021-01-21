// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

namespace Reaqtive
{
    /// <summary>
    /// Represents an operator state reader.
    /// </summary>
    public interface IOperatorStateReader : IDisposable
    {
        /// <summary>
        /// Reads a value of the provided type.
        /// </summary>
        /// <typeparam name="T">The expected type of the element to retrieve.</typeparam>
        /// <returns>The value of the element.</returns>
        /// <remarks>
        /// Note that reading must be done in the same sequential order as the writing.
        /// </remarks>
        T Read<T>();

        /// <summary>
        /// Try to read the next value of the provided type.
        /// </summary>
        /// <typeparam name="T">The expected type of the element to retrieve.</typeparam>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if a value is read; otherwise, <c>false</c>.</returns>
        bool TryRead<T>(out T value);

        /// <summary>
        /// Reset the operator state reader.
        /// </summary>
        void Reset();

        /// <summary>
        /// Creates a child state reader.
        /// </summary>
        /// <returns>Child state reader.</returns>
        IOperatorStateReader CreateChild();
    }
}
