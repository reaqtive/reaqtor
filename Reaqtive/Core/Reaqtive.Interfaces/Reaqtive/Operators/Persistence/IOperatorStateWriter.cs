// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

namespace Reaqtive
{
    /// <summary>
    /// Represents an operator state writer.
    /// </summary>
    public interface IOperatorStateWriter : IDisposable
    {
        /// <summary>
        /// Stores the provided value.
        /// </summary>
        /// <typeparam name="T">The type of the value to store.</typeparam>
        /// <param name="value">The value to store.</param>
        void Write<T>(T value);

        /// <summary>
        /// Creates a child state writer.
        /// </summary>
        /// <returns>Child state writer.</returns>
        IOperatorStateWriter CreateChild();
    }
}
