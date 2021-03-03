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

using Reaqtor.Remoting.Messaging;

namespace Reaqtor.Remoting.Platform
{
    internal sealed class InMemoryMessagingService : ReactiveServiceBase, IReactiveMessagingService
    {
        public InMemoryMessagingService()
#pragma warning disable CA2000 // Dispose objects before losing scope. (Ownership transfer.)
            : base(new InMemoryRunnable(CreateMessagingService), ReactiveServiceType.MessagingService)
#pragma warning restore CA2000
        {
        }

        private static Task<object> CreateMessagingService(CancellationToken token)
        {
            return Task.FromResult<object>(new MessagingConnection());
        }
    }
}
