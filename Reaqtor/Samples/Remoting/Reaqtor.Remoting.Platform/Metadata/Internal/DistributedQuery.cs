// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Linq.Expressions;

namespace Reaqtor.Remoting.Metadata
{
    /// <summary>
    /// Representation of a distributed query that has a remote part and a local part.
    /// </summary>
    internal class DistributedQuery
    {
        /// <summary>
        /// Creates a new distributed query representation.
        /// </summary>
        /// <param name="remote">Remote part of the query.</param>
        /// <param name="local">Local part of the query that will get applied to the results of the remote part.</param>
        public DistributedQuery(Expression remote, LambdaExpression local)
        {
            Remote = remote;
            Local = local;
        }

        /// <summary>
        /// Gets the remote part of the query.
        /// </summary>
        public Expression Remote { get; }

        /// <summary>
        /// Gets the local part of the query that will get applied to the results of the remote part.
        /// </summary>
        public LambdaExpression Local { get; }
    }
}
