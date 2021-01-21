// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Threading;
using System.Threading.Tasks;

using Reaqtor.Remoting.KeyValueStore;

namespace Reaqtor.Remoting.Platform
{
    internal sealed class InMemoryKeyValueStoreService : ReactiveServiceBase, IKeyValueStoreService
    {
        public InMemoryKeyValueStoreService()
            : base(new InMemoryRunnable(CreateKeyValueStoreService), ReactiveServiceType.KeyValueStoreService)
        {
        }

        private static Task<object> CreateKeyValueStoreService(CancellationToken token)
        {
            return Task.FromResult<object>(new KeyValueStoreConnection());
        }
    }
}
