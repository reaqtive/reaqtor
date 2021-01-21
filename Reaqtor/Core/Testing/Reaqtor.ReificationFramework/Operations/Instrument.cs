// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

namespace Reaqtor.ReificationFramework
{
    /// <summary>
    /// Operation to inject callbacks before and after an operation is performed.
    /// </summary>
    public sealed class Instrument : OperationBase
    {
        internal Instrument(ReifiedOperation operation, Action onEnter, Action onExit)
            : base(ReifiedOperationKind.Instrument, operation)
        {
            OnEnter = onEnter ?? throw new ArgumentNullException(nameof(onEnter));
            OnExit = onExit ?? throw new ArgumentNullException(nameof(onExit));
        }

        /// <summary>
        /// Callback to invoke prior to evaluating the operation.
        /// </summary>
        public Action OnEnter { get; }

        /// <summary>
        /// Callback to invoke after evaluating the operation.
        /// </summary>
        public Action OnExit { get; }
    }
}
