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
    public abstract class RemotingReactiveServiceConnectionBase : ReactiveConnectionBase, IRemotingReactiveServiceConnection
    {
        public IReactiveServiceConnection Connection
        {
            get;
            set;
        }

        public abstract void Configure(IReactivePlatformConfiguration configuration);

        public IReactiveServiceCommandRemoting CreateCommand(CommandVerb verb, CommandNoun noun, string commandText)
        {
            var connection = Connection;
            if (connection == null)
            {
                throw new InvalidOperationException("Inner connection is not set. Did you configure and start the service?");
            }

            return new ReactiveServiceCommandService(connection.CreateCommand(verb, noun, commandText));
        }

        public abstract void Start();

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                DisposeCore();
            }
        }

        protected abstract void DisposeCore();
    }
}
