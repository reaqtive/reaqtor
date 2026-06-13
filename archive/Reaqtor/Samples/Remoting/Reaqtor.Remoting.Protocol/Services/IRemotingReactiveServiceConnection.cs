// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - March 2015 - Created this file.
//

using System;

namespace Reaqtor.Remoting.Protocol
{
    public interface IRemotingReactiveServiceConnection : IReactiveConnection, IDisposable
    {
        void Configure(IReactivePlatformConfiguration configuration);

        IReactiveServiceCommandRemoting CreateCommand(CommandVerb verb, CommandNoun noun, string commandText);

        void Start();
    }
}
