// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - October 2013 - Created this file.
//

using System.Collections.Generic;

namespace Reaqtor.Remoting.Protocol
{
    public interface IReactiveStorageConnection : IReactiveConnection
    {
        void AddEntity(string collection, string key, StorageEntity entity);

        void DeleteEntity(string collection, string key);

        bool TryGetEntity(string collection, string key, out StorageEntity entity);

        List<StorageEntity> GetEntities(string collection);
    }
}
