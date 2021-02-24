// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.CompilerServices;
using System.Linq.Expressions;

namespace Reaqtor.Expressions
{
    /// <summary>
    /// Provides various helpers to work with expression trees.
    /// </summary>
    internal static class ExpressionHelpers
    {
        /// <summary>
        /// Gets a collection of free variables in the specified expression.
        /// </summary>
        /// <param name="expression">The expression to scan for free variables.</param>
        /// <returns>A collection of free variables that occur in the expression.</returns>
        public static ICollection<ParameterExpression> FindFreeVariables(Expression expression)
        {
            var freeVariables = FreeVariableScanner.Scan(expression);

            //
            // PERF: The current implementation of Scan returns either a HashSet<T> or an empty T[]
            //       singleton for the free variables sequence. Both are convertible to ICollection<T>,
            //       so the call to AsCollection below will return the original object and not create
            //       a copy. This is cheaper than calling HasFreeVariables followed by a Scan in case
            //       an expression has free variables, because we'll visit the expression twice (with
            //       early bail-out for HasFreeVariables but still a lot of virtual dispatching). By
            //       returning a collection, we can also avoid dynamically growing array allocations
            //       when mapping variables using a .Select(...).ToArray() approach.
            //

            Debug.Assert(freeVariables is ICollection<ParameterExpression>, "Potential performance hazard.");

            return freeVariables.AsCollection();
        }
    }
}
