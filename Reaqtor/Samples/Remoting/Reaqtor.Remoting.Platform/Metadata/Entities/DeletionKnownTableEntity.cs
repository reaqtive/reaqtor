// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

namespace Reaqtor.Remoting.Metadata
{
    /// <summary>
    /// Table entity derived class to use for deletion operations from URI only.
    /// </summary>
    public class DeletionKnownTableEntity : KnownTableEntity
    {
        /// <summary>
        /// Instantiates a table entity for deletion operations.
        /// </summary>
        /// <param name="uri">The URI of the entity to delete.</param>
        public DeletionKnownTableEntity(Uri uri)
            : base(uri)
        {
            ETag = "*";
        }
    }
}
