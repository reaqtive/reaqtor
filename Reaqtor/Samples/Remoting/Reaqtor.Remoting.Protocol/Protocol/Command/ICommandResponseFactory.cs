// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Threading;
using System.Threading.Tasks;

namespace Reaqtor.Remoting.Protocol
{
    /// <summary>
    /// An interface for creating responses for the execution of commands.
    /// </summary>
    public interface ICommandResponseFactory
    {
        /// <summary>
        /// Creates a response for a given command.
        /// </summary>
        /// <param name="command">The command that is executing.</param>
        /// <param name="task">A task to await the completion of the command.</param>
        /// <param name="token">A cancellation token.</param>
        /// <returns>A task to await the response from the command.</returns>
        Task<string> CreateResponseAsync(IReactiveServiceCommand command, Task task, CancellationToken token);

        /// <summary>
        /// Creates a response for a given command.
        /// </summary>
        /// <typeparam name="T">The type of value that will be sent in the response.</typeparam>
        /// <param name="command">The command that is executing.</param>
        /// <param name="task">A task to await the completion of the command.</param>
        /// <param name="token">A cancellation token.</param>
        /// <returns>A task to await the response from the command.</returns>
        Task<string> CreateResponseAsync<T>(IReactiveServiceCommand command, Task<T> task, CancellationToken token);
    }
}
