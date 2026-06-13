// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - October 2013 - Created this file.
//

using System.Threading;
using System.Threading.Tasks;

namespace Reaqtor.Remoting.Platform
{
    /// <summary>
    /// The reactive service interface.
    /// </summary>
    public interface IReactiveService
    {
        /// <summary>
        /// Gets the type of Reactive service.
        /// </summary>
        ReactiveServiceType ServiceType { get; }

        /// <summary>
        /// Gets the runnable host for the service.
        /// </summary>
        IRunnable Runnable { get; }

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
        /// Registers a service with the current service.
        /// </summary>
        /// <param name="service">The service to register with this service.</param>
        void Register(IReactiveService service);

        /// <summary>
        /// Gets a transparent proxy to the remote instance.
        /// </summary>
        /// <typeparam name="T">The interface for the remote instance.</typeparam>
        /// <returns>The transparent proxy.</returns>
        TInstance GetInstance<TInstance>();
    }
}
