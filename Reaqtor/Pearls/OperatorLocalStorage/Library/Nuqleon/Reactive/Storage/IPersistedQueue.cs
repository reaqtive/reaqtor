// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2018
//

#pragma warning disable CA1711 // Identifiers should not have incorrect suffix. (By design for collection interfaces.)

using System.Collections.Generic;

namespace Reaqtive.Storage
{
    /// <summary>
    /// Interface representing a persisted queue.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the persisted queue.</typeparam>
    public interface IPersistedQueue<T> : IQueue<T>, IPersisted
    {
    }
}
