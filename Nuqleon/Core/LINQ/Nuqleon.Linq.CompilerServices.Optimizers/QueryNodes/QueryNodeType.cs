// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// PS - February 2015 - Created this file.
//

namespace System.Linq.CompilerServices.Optimizers
{
    /// <summary>
    /// Describes the node types for the nodes of an expression tree.
    /// </summary>
    public enum QueryNodeType
    {
        /// <summary>
        /// Unknown node type.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// A node that is a wrapper around a query node, allowing it to be treated as a monad member.
        /// </summary>
        MonadAbstraction = 1,

        /// <summary>
        /// A node that represents an operation on the monad.
        /// </summary>
        Operator = 2,

        /// <summary>
        /// A node that abstracts over unknown query expressions.
        /// </summary>
        Lambda = 3,
    }
}
