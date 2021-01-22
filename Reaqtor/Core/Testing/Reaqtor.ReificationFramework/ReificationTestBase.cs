// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using Reaqtor.TestingFramework;
using System;
using System.Threading.Tasks;

namespace Reaqtor.ReificationFramework
{
    /// <summary>
    /// A base class for declaring reified operations.
    /// </summary>
    public abstract class ReificationTestBase
    {
        /// <summary>
        /// Produces a set of reified operations.
        /// </summary>
        /// <param name="operation">The operation to perform.</param>
        /// <returns>The set of reified operations.</returns>
        protected ServiceOperation[] Run(Func<ReificationClientContext, Task> operation)
        {
            if (operation == null)
                throw new ArgumentNullException(nameof(operation));

            var expressionServices = GetExpressionServices();
            var serviceProvider = new ReificationServiceProvider();
            var ctx = new ReificationClientContext(expressionServices, serviceProvider);
            operation(ctx).Wait();
            return serviceProvider.Operations;
        }

        /// <summary>
        /// Gets the expression services used to construct a <see cref="ReificationClientContext"/>.
        /// </summary>
        /// <returns>An expression services implementation.</returns>
        protected abstract IReactiveExpressionServices GetExpressionServices(); // E.g. "return new TupletizingExpressionServices(typeof(IReactiveClientProxy))".
    }
}
