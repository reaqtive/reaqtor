// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - October 2013 - Created this code in the Remoting sample's client library.
// HvR - July 2026 - Factored out of the archived Remoting sample (see #158).
//

using System.Linq.Expressions;

namespace Reaqtor;

/// <summary>
/// Expression tree rewriting services for reactive service clients, which, after normalization,
/// convert all invocation expressions and the root level lambda expression to a form using unary
/// lambdas by tupletizing the arguments.
/// </summary>
public class TupletizingReactiveExpressionServices : CheckedReactiveExpressionServices
{
    /// <summary>
    /// Creates a new tupletizing expression service provider instance.
    /// </summary>
    /// <param name="reactiveClientInterfaceType">Interface type of the IReactiveClient variant used to obtain reactive resources.</param>
    public TupletizingReactiveExpressionServices(Type reactiveClientInterfaceType)
        : base(reactiveClientInterfaceType)
    {
    }

    /// <summary>
    /// Normalizes the specified expression prior to submission to the service.
    /// </summary>
    /// <param name="expression">The expression to normalize.</param>
    /// <returns>The normalized expression.</returns>
    public override Expression Normalize(Expression expression)
    {
        var normalized = base.Normalize(expression);
        return ExpressionTupletization.Tupletize(normalized);
    }
}
