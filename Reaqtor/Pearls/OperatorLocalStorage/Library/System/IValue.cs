// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2018
//

namespace System
{
    /// <summary>
    /// Interface representing a value.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    public interface IValue<T> : IReadOnlyValue<T>
    {
        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        new T Value { get; set; }
    }
}
