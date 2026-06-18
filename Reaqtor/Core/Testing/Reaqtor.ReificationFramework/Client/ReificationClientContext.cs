// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

namespace Reaqtor.ReificationFramework
{
    /// <summary>
    /// Client context for reifications.
    /// </summary>
    /// <remarks>
    /// Instantiates the client context.
    /// </remarks>
    /// <param name="expressionServices">The expression rewrite services.</param>
    /// <param name="serviceProvider">The service provider.</param>
    public class ReificationClientContext(IReactiveExpressionServices expressionServices, IReactiveServiceProvider serviceProvider) : ReactiveClientContext(expressionServices, serviceProvider)
    {
    }
}
