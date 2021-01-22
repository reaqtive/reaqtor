// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel;

using Microsoft.Azure.Cosmos.Table;

namespace Reaqtor.Remoting.Metadata
{
    /// <summary>
    /// Base class for table entities representing known resources.
    /// </summary>
    public abstract class KnownTableEntity : TableEntity, IKnownResource
    {
        /// <summary>
        /// Default constructor, required by the Azure Table query provider.
        /// </summary>
        protected KnownTableEntity()
        {
        }

        /// <summary>
        /// Creates a new table entity representing a known resource with the specified URI.
        /// </summary>
        /// <param name="uri">URI identifying the resource represented by the table entity.</param>
        protected KnownTableEntity(Uri uri)
        {
            Contract.RequiresNotNull(uri);

            RowKey = HashingHelper.ComputeHash(uri.ToCanonicalString());
            Id = uri.ToCanonicalString();
        }

        /// <summary>
        /// Gets the URI identifying the resource represented by the current table entity.
        /// </summary>
        public Uri Uri => new(Id);

        /// <summary>
        /// (Infrastructure) Gets or sets the serialized form of the URI identifier.
        /// This property is used by the framework infrastructure and is not intended to be used directly.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string Id
        {
            get;
            set;
        }
    }
}
