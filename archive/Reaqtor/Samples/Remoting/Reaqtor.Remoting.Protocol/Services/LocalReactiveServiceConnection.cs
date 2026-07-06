// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - March 2015 - Created this file.
//

namespace Reaqtor.Remoting.Protocol
{
    public class LocalReactiveServiceConnection : IReactiveServiceConnection
    {
        private readonly IRemotingReactiveServiceConnection _connection;

        public LocalReactiveServiceConnection(IRemotingReactiveServiceConnection connection)
        {
            _connection = connection;
        }

        public IReactiveServiceCommand CreateCommand(CommandVerb verb, CommandNoun noun, string commandText)
        {
            return new ReactiveServiceCommandProxy(_connection.CreateCommand(verb, noun, commandText));
        }
    }
}
