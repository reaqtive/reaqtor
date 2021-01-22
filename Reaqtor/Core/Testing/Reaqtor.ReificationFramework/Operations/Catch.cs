// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

#pragma warning disable CA1716 // Identifiers should not match keywords. (Catch is the name of the operator.)

using System;

namespace Reaqtor.ReificationFramework
{
    /// <summary>
    /// Operation to catch exceptions thrown by the inner operation.
    /// </summary>
    public class Catch<T> : OperationBase
        where T : Exception
    {
        internal Catch(ReifiedOperation operation, Action<T> handler)
            : base(ReifiedOperationKind.Catch, operation)
        {
            Handler = handler ?? throw new ArgumentNullException(nameof(handler));
        }

        /// <summary>
        /// Callback when exceptions are thrown.
        /// </summary>
        public Action<T> Handler { get; }
    }
}
