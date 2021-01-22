// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2018
//

using System;

namespace Reaqtive.Storage
{
    /// <summary>
    /// Interface representing a persisted value.
    /// </summary>
    /// <typeparam name="T">The type of the persisted value.</typeparam>
    public interface IPersistedValue<T> : IValue<T>, IPersisted
    {
    }
}
