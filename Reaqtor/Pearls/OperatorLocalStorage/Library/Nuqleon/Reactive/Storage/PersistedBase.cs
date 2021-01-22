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
    /// Base class for persisted objects.
    /// </summary>
    public class PersistedBase : IPersisted
    {
        /// <summary>
        /// Creates a new instance of <see cref="PersistedBase"/> using the specified unique <paramref name="id"/> for the object.
        /// </summary>
        /// <param name="id">The unique identifier of the object.</param>
        protected PersistedBase(string id)
        {
            Id = id;
        }

        /// <summary>
        /// Gets the unique identifier of the object.
        /// </summary>
        public string Id { get; }
    }
}
