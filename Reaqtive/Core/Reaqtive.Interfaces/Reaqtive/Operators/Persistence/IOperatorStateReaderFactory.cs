// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

namespace Reaqtive
{
    /// <summary>
    /// Represents a factory for operator state readers.
    /// </summary>
    public interface IOperatorStateReaderFactory : IDisposable
    {
        /// <summary>
        /// Creates a new operator state reader for the specified operator.
        /// </summary>
        /// <param name="node">Operator whose state will be read by the created reader.</param>
        /// <returns>Operator state reader for the specified operator.</returns>
        IOperatorStateReader Create(IStatefulOperator node);
    }
}
