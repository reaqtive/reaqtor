// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
//

using System.Linq.Expressions;

namespace Reaqtor
{
    /// <summary>
    /// Represents a reactive resource with an expression tree representation.
    /// </summary>
    public interface IAsyncReactiveExpressible : IExpressible
    {
        /// <summary>
        /// Gets the query provider that is associated with the object.
        /// </summary>
        IAsyncReactiveQueryProvider Provider { get; }
    }
}
