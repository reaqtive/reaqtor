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
    /// Describes the node types for the nodes of a query expression tree.
    /// </summary>
    public enum OperatorType
    {
        /// <summary>
        /// An unknown query node.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// A node that represents a where operator.
        /// </summary>
        Where = 1,

        /// <summary>
        /// A node that represents a select operator.
        /// </summary>
        Select = 2,

        /// <summary>
        /// A node that represents a take operator.
        /// </summary>
        Take = 3,

        /// <summary>
        /// A node that represents a first operator.
        /// </summary>
        First = 4,

        /// <summary>
        /// A node that represents a first operator with a predicate.
        /// </summary>
        FirstPredicate = 5,
    }
}
