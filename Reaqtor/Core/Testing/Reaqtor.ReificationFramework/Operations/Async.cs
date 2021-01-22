// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Reaqtor.ReificationFramework
{
    /// <summary>
    /// Operation to start an operation on a thread pool thread.
    /// </summary>
    public sealed class Async : OperationBase
    {
        internal Async(ReifiedOperation operation, Action<Task> onStart, CancellationToken token)
            : base(ReifiedOperationKind.Async, operation)
        {
            Token = token;
            OnStart = onStart;
        }

        /// <summary>
        /// A cancellation token to give to the task factory.
        /// </summary>
        public CancellationToken Token { get; }

        /// <summary>
        /// A callback to return the task after the operation has been started.
        /// </summary>
        public Action<Task> OnStart { get; }
    }
}
