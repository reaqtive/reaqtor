// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Threading;
using System.Threading.Tasks;

using Reaqtor.Remoting.StateStore;

namespace Reaqtor.Remoting.Platform
{
    internal sealed class InMemoryStateStoreService : ReactiveServiceBase, IReactiveStateStoreService
    {
        public InMemoryStateStoreService()
#pragma warning disable CA2000 // Dispose objects before losing scope. (Ownership transfer.)
            : base(new InMemoryRunnable(CreateStateStoreService), ReactiveServiceType.StateStoreService)
#pragma warning restore CA2000
        {
        }

        private static Task<object> CreateStateStoreService(CancellationToken token)
        {
            return Task.FromResult<object>(new StateStoreConnection());
        }
    }
}
