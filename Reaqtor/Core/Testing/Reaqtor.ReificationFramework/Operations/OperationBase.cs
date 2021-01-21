// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

namespace Reaqtor.ReificationFramework
{
    /// <summary>
    /// Base class for reified operations.
    /// </summary>
    public abstract class OperationBase : ReifiedOperation
    {
        internal OperationBase(ReifiedOperationKind kind, ReifiedOperation operation)
            : base(kind)
        {
            Operation = operation ?? throw new ArgumentNullException(nameof(operation));
        }

        /// <summary>
        /// The inner operation.
        /// </summary>
        public ReifiedOperation Operation { get; }
    }
}
