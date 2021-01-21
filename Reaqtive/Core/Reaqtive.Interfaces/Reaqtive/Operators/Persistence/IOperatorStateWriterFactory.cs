// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

namespace Reaqtive
{
    /// <summary>
    /// Represents a factory for operator state writers.
    /// </summary>
    public interface IOperatorStateWriterFactory : IDisposable
    {
        /// <summary>
        /// Creates a new operator state writer for the specified operator.
        /// </summary>
        /// <param name="node">Operator whose state will be written by the created reader.</param>
        /// <returns>Operator state writer for the specified operator.</returns>
        IOperatorStateWriter Create(IStatefulOperator node);
    }
}
