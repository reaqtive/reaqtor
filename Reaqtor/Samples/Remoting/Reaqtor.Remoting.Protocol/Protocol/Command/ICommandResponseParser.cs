// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Threading;
using System.Threading.Tasks;

namespace Reaqtor.Remoting.Protocol
{
    /// <summary>
    /// An interface for parsing the response from executing a command.
    /// </summary>
    public interface ICommandResponseParser
    {
        /// <summary>
        /// Parses the response from a command.
        /// </summary>
        /// <param name="command">The command that was executed.</param>
        /// <param name="request">A task to await the execution of the command.</param>
        /// <param name="token">A cancellation token.</param>
        /// <returns>A task to await the parsed response.</returns>
        Task ParseResponseAsync(IReactiveServiceCommand command, Task<string> request, CancellationToken token);

        /// <summary>
        /// Parses the response from a command.
        /// </summary>
        /// <typeparam name="T">The expected response type.</typeparam>
        /// <param name="command">The command that was executed.</param>
        /// <param name="request">A task to await the execution of the command.</param>
        /// <param name="token">A cancellation token.</param>
        /// <returns>A task to await the parsed response.</returns>
        Task<T> ParseResponseAsync<T>(IReactiveServiceCommand command, Task<string> request, CancellationToken token);
    }
}
