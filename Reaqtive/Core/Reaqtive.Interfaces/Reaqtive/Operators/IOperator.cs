// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;

namespace Reaqtive
{
    /// <summary>
    /// Represents an operator used for event processing.
    /// </summary>
    public interface IOperator : IDisposable
    {
        /// <summary>
        /// Gets the operator's inputs.
        /// </summary>
        IEnumerable<ISubscription> Inputs { get; }

        /// <summary>
        /// Initializes <see cref="Inputs"/>.
        /// </summary>
        void Subscribe();

        /// <summary>
        /// Sets the operator context on the operator.
        /// </summary>
        /// <param name="context">Operator context to set on the operator.</param>
        void SetContext(IOperatorContext context);

        /// <summary>
        /// Starts the operator.
        /// </summary>
        void Start();
    }
}
