// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
//

using System.Linq;

namespace Reaqtor
{
    /// <summary>
    /// Interface for metadata discovery operations of a reactive processing service.
    /// </summary>
    public interface IReactiveMetadataServiceProvider
    {
        /// <summary>
        /// Gets the query provider that will be used to execute metadata queries.
        /// </summary>
        IQueryProvider Provider
        {
            get;
        }
    }
}
