// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2018
//

using System.Collections.Generic;

namespace Reaqtive.Storage
{
    /// <summary>
    /// Interface representing a persisted array.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the persisted array.</typeparam>
    public interface IPersistedArray<T> : IArray<T>, IPersisted
    {
    }
}
