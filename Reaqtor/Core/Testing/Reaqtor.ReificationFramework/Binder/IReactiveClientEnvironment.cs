// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

namespace Reaqtor.ReificationFramework
{
    /// <summary>
    /// An interface to support reification for a client Reactive environment.
    /// </summary>
    public interface IReactiveClientEnvironment : IDisposable
    {
        /// <summary>
        /// Gets the client context for the environment.
        /// </summary>
        IReactiveProxy Context { get; }

        /// <summary>
        /// Triggers a differential checkpoint on the environment.
        /// </summary>
        /// <param name="uri">The URI of the engine to checkpoint.</param>
        void DifferentialCheckpoint(Uri uri);

        /// <summary>
        /// Triggers a full checkpoint on the environment.
        /// </summary>
        /// <param name="uri">The URI of the engine to checkpoint.</param>
        void FullCheckpoint(Uri uri);

        /// <summary>
        /// Triggers a recovery on the environment.
        /// /// </summary>
        /// <param name="uri">The URI of the engine to recover.</param>
        void Recover(Uri uri);
    }
}
