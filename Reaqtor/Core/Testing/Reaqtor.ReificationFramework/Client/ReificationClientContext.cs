// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

namespace Reaqtor.ReificationFramework
{
    /// <summary>
    /// Client context for reifications.
    /// </summary>
    public class ReificationClientContext : ReactiveClientContext
    {
        /// <summary>
        /// Instantiates the client context.
        /// </summary>
        /// <param name="expressionServices">The expression rewrite services.</param>
        /// <param name="serviceProvider">The service provider.</param>
        public ReificationClientContext(IReactiveExpressionServices expressionServices, IReactiveServiceProvider serviceProvider)
            : base(expressionServices, serviceProvider)
        {
            // NB: base used to accept new TupletizingExpressionServices(typeof(IReactiveClientProxy)), which has dependencies on DataModel etc.
        }
    }
}
