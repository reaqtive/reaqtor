// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

namespace Reaqtor.Remoting.Protocol
{
    /// <summary>
    /// Represents a connection to a reactive service.
    /// </summary>
    public interface IReactiveServiceConnection
    {
        /// <summary>
        /// Creates and returns an IReactiveServiceCommand object associated with the connection.
        /// </summary>
        /// <param name="verb">Command verb.</param>
        /// <param name="noun">Command noun.</param>
        /// <param name="commandText">Text command to run against the reactive service.</param>
        /// <returns>Command object associated with the connection.</returns>
        IReactiveServiceCommand CreateCommand(CommandVerb verb, CommandNoun noun, string commandText);
    }
}
