// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - December 2014 - Created this file.
//

using System.Linq.Expressions;

namespace System.Linq.CompilerServices
{
    /// <summary>
    /// Interface for facilities that hoist constants out of an expression tree into an environment.
    /// </summary>
    public interface IConstantHoister
    {
        /// <summary>
        /// Hoists constants in the specified expression and returns an environment.
        /// </summary>
        /// <param name="expression">Expression to hoist constants in.</param>
        /// <returns>Expression bound by an environment consisting of the hoisted constants.</returns>
        IExpressionWithEnvironment Hoist(Expression expression);
    }
}
