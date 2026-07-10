// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - August 2014 - Created this code in the Remoting sample's client library.
// HvR - July 2026 - Factored out of the archived Remoting sample (see #158).
//

using System.Linq.Expressions;

namespace Reaqtor;

/// <summary>
/// Expression tree rewriting services for reactive service clients, which, after normalization,
/// convert all invocation expressions and the root level lambda expression from a form using unary
/// lambdas with tuple arguments to n-ary lambdas with the tuple arguments unpacked.
/// </summary>
/// <remarks>
/// Unlike <see cref="TupletizingReactiveExpressionServices"/>, this type derives from
/// <see cref="ReactiveExpressionServices"/> rather than <see cref="CheckedReactiveExpressionServices"/>.
/// Detupletization takes place on the receiving side of an expression exchange, where allow list
/// scanning and local evaluation of rejected subexpressions - which are submission concerns - do not
/// apply.
/// </remarks>
public class DetupletizingReactiveExpressionServices : ReactiveExpressionServices
{
    /// <summary>
    /// Creates a new detupletizing expression service provider instance.
    /// </summary>
    /// <param name="reactiveClientInterfaceType">Interface type of the IReactiveClient variant used to obtain reactive resources.</param>
    public DetupletizingReactiveExpressionServices(Type reactiveClientInterfaceType)
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
        return ExpressionTupletization.Detupletize(normalized);
    }
}
