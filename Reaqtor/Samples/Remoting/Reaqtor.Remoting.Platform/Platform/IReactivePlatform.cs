// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - October 2013 - Created this file.
//

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Reaqtor.Remoting.Protocol;

namespace Reaqtor.Remoting.Platform
{
    /// <summary>
    /// An interface to organize the services that make up a reactive platform.
    /// </summary>
    /// <typeparam name="T">The type of runnables used for the platform services.</typeparam>
    public interface IReactivePlatform : IDisposable
    {
        /// <summary>
        /// Gets the platform query coordinator.
        /// </summary>
        IReactiveQueryCoordinator QueryCoordinator { get; }

        /// <summary>
        /// Gets the platform query evaluators.
        /// </summary>
        IEnumerable<IReactiveQueryEvaluator> QueryEvaluators { get; }

        /// <summary>
        /// Gets the platform environment, which includes the services for
        /// state storage, metadata storage, and messaging.
        /// </summary>
        IReactiveEnvironment Environment { get; }

        /// <summary>
        /// Gets the platform configuration.
        /// </summary>
        IReactivePlatformConfiguration Configuration { get; }

        /// <summary>
        /// Starts the service.
        /// </summary>
        /// <param name="token">A token to cancel the operation.</param>
        /// <returns>A task to await the operation.</returns>
        Task StartAsync(CancellationToken token);

        /// <summary>
        /// Stops the service.
        /// </summary>
        /// <param name="token">A token to cancel the operation.</param>
        /// <returns>A task to await the operation.</returns>
        Task StopAsync(CancellationToken token);

        /// <summary>
        /// Creates a client for the platform.
        /// </summary>
        /// <returns>A client for the platform.</returns>
        IReactivePlatformClient CreateClient();
    }
}
