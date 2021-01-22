// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2018
//

namespace Reaqtive.Storage
{
    /// <summary>
    /// Interface representing a persisted object.
    /// </summary>
    public interface IPersisted
    {
        /// <summary>
        /// Gets the unique identifier of the object.
        /// </summary>
        string Id { get; }
    }
}
