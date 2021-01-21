// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - December 2014 - Created this file.
//

using System.Collections.Generic;
using System.Linq.Expressions;

namespace System.Linq.CompilerServices
{
    /// <summary>
    /// Interface for the representation of an expression bound by an environment.
    /// </summary>
    public interface IExpressionWithEnvironment
    {
        /// <summary>
        /// Gets the expression bound to the environment.
        /// </summary>
        Expression Expression { get; }

        /// <summary>
        /// Gets the environment containing bindings of parameters to values in the expression.
        /// </summary>
        IReadOnlyList<Binding> Bindings { get; }
    }
}
