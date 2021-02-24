﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - October 2013 - Created this file.
//

using System.Threading;
using System.Threading.Tasks;

using Reaqtor.Remoting.Metadata;

namespace Reaqtor.Remoting.Platform
{
    internal sealed class InMemoryMetadataService : ReactiveServiceBase, IReactiveMetadataService
    {
        public InMemoryMetadataService()
            : base(new InMemoryRunnable(CreateMetadataService), ReactiveServiceType.MetadataService)
        {
        }

        private static Task<object> CreateMetadataService(CancellationToken token)
        {
            return Task.FromResult<object>(new StorageConnection());
        }
    }
}
