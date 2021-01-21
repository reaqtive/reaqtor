// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

namespace Reaqtor.QueryEngine
{
    /// <summary>
    /// Flags representing the status of a query engine.
    /// </summary>
    [Flags]
    public enum QueryEngineStatus
    {
        /// <summary>
        /// Unknown state.
        /// </summary>
        None = 0,

        /// <summary>
        /// The engine has been created but is not yet running.
        /// </summary>
        /// <remarks>This state is reserved for future use.</remarks>
        Created = 1,

        /// <summary>
        /// The engine is recovering. During this state:
        /// - Event processing is paused and events get queued up.
        /// - Incoming requests are accepted but their effects may be deferred.
        /// </summary>
        Recovering = 2,

        /// <summary>
        /// The engine is running and processing events.
        /// </summary>
        Running = 4,

        /// <summary>
        /// The engine is checkpointing. During this state:
        /// - Event processing is paused and events get queued up.
        /// - Incoming requests are accepted but their effects may be deferred.
        /// </summary>
        Checkpointing = 8,

        /// <summary>
        /// The engine is unloading and will transition to the Unloaded state. During this state:
        /// - Event processing will be stopped.
        /// - Incoming requests will be rejected.
        /// </summary>
        Unloading = 16,

        /// <summary>
        /// The engine has been unloaded and will no longer process events or serve requests. This is a terminal state.
        /// </summary>
        Unloaded = 32,

        /// <summary>
        /// Transient state that can be combined with Recovering and Checkpointing states.
        /// When this flag is set, pending operations will be cancelled and the engine will transition to the Unloading state next.
        /// </summary>
        UnloadRequested = 64,

        /// <summary>
        /// The engine attempted an unload but the operation failed and can be retried.
        /// </summary>
        UnloadFailed = 128,

        /// <summary>
        /// The engine has encountered a critical error. This is a terminal state.
        /// </summary>
        Faulted = 1024,
    }
}
