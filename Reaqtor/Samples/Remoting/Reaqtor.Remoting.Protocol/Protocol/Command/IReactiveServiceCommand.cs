// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Threading;
using System.Threading.Tasks;

namespace Reaqtor.Remoting.Protocol
{
    /// <summary>
    /// Represents a command executed against a reactive service.
    /// </summary>
    public interface IReactiveServiceCommand
    {
        /// <summary>
        /// Gets the IReactiveServiceConnection used by this instance of the IReactiveServiceCommand.
        /// </summary>
        IReactiveServiceConnection Connection { get; }

        /// <summary>
        /// Gets the command verb.
        /// </summary>
        CommandVerb Verb { get; }

        /// <summary>
        /// Gets the command noun.
        /// </summary>
        CommandNoun Noun { get; }

        /// <summary>
        /// Gets the text command to run against the reactive service.
        /// </summary>
        string CommandText { get; }

        /// <summary>
        /// Executes the command and obtains the result, if any.
        /// </summary>
        /// <param name="token">Token to observe cancellation requests.</param>
        /// <returns>Task representing the eventual completion of the command, or an exception.</returns>
        Task<string> ExecuteAsync(CancellationToken token);
    }
}
