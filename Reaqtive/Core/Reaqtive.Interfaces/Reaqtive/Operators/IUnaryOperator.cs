// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

namespace Reaqtive
{
    /// <summary>
    /// Represents an operator with at most one child used for event processing.
    /// </summary>
    public interface IUnaryOperator : IOperator
    {
        /// <summary>
        /// Gets the single input to the operator.
        /// </summary>
        ISubscription Input { get; }
    }
}
