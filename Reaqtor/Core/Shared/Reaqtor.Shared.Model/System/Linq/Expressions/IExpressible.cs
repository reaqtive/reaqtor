// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
//

namespace System.Linq.Expressions
{
    /// <summary>
    /// Interface for objects with an expression tree representation.
    /// </summary>
    public interface IExpressible
    {
        /// <summary>
        /// Gets the expression tree representation of the object.
        /// </summary>
        Expression Expression { get; }
    }
}
