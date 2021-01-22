// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

namespace Reaqtor.ReificationFramework
{
    /// <summary>
    /// Enumeration of reified operation kinds.
    /// </summary>
    public enum ReifiedOperationKind
    {
        /// <summary>
        /// Operation to start an operation on a thread pool thread.
        /// </summary>
        Async,

        /// <summary>
        /// Operation to catch exceptions thrown by the inner operation. 
        /// </summary>
        Catch,

        /// <summary>
        /// Operation to combine operations into a sequence.
        /// </summary>
        Chain,

        /// <summary>
        /// Operation to inject callbacks before and after an operation is performed.
        /// </summary>
        Instrument,

        /// <summary>
        /// Operation to lift the wildcards in a reified operation.
        /// </summary>
        LiftWildcards,

        /// <summary>
        /// Operation applied to a query engine.
        /// </summary>
        QueryEngineOperation,

        /// <summary>
        /// Operation to repeat an operation a given number of times.
        /// </summary>
        Repeat,

        /// <summary>
        /// Operation to repeat an operation until a cancellation is requested.
        /// </summary>
        RepeatUntil,

        /// <summary>
        /// Operation applied to a reactive service.
        /// </summary>
        ServiceOperation,
    }
}
