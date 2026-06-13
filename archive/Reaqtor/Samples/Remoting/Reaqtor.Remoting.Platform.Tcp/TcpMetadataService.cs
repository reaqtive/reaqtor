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

using Reaqtor.Remoting.Protocol;

namespace Reaqtor.Remoting.Platform
{
    internal sealed class TcpMetadataService : ReactiveServiceBase, IReactiveMetadataService
    {
        public TcpMetadataService(string executablePath, int port, string uri)
#pragma warning disable CA2000 // Dispose objects before losing scope. (Ownership transfer.)
            : this(new TcpRunnable<IReactiveStorageConnection>(executablePath, port, uri))
#pragma warning restore CA2000
        {
        }

        public TcpMetadataService(IRunnable runnable)
            : base(runnable, ReactiveServiceType.MetadataService)
        {
        }

        public override Task StartAsync(CancellationToken token)
        {
            Runnable.RunAsync(token);
            return Task.FromResult(true);
        }
    }
}
