// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Threading;

namespace Reaqtor.ReificationFramework
{
    /// <summary>
    /// Operation to repeat an operation until a cancellation is requested.
    /// </summary>
    public class RepeatUntil : OperationBase
    {
        internal RepeatUntil(ReifiedOperation operation, CancellationToken token)
            : base(ReifiedOperationKind.RepeatUntil, operation)
        {
            Token = token;
        }

        /// <summary>
        /// The cancellation token.
        /// </summary>
        public CancellationToken Token { get; }
    }
}
