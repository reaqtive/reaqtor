// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Threading;
using System.Threading.Tasks;

using Reaqtor.Remoting.Protocol;

namespace Reaqtor.Remoting.Platform
{
    internal sealed class TcpKeyValueStoreService : ReactiveServiceBase, IKeyValueStoreService
    {
        public TcpKeyValueStoreService(string executablePath, int port, string uri)
            : this(new TcpRunnable<ITransactionalKeyValueStoreConnection>(executablePath, port, uri))
        {
        }

        public TcpKeyValueStoreService(IRunnable runnable)
            : base(runnable, ReactiveServiceType.KeyValueStoreService)
        {
        }

        public override Task StartAsync(CancellationToken token)
        {
            Runnable.RunAsync(token);
            return Task.FromResult(true);
        }
    }
}
