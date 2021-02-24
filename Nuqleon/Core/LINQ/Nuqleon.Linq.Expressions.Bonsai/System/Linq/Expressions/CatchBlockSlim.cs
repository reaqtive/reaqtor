// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// PS - December 2014 - Created this file.
//

using System.Reflection;

namespace System.Linq.Expressions
{
    // PERF: Consider introducing optimized layouts for cases such as the absence of a Filter, or the absence of a Variable.

    /// <summary>
    /// Lightweight representation of a catch statement in a try block.
    /// </summary>
    public sealed class CatchBlockSlim
    {
        internal CatchBlockSlim(TypeSlim test, ParameterExpressionSlim variable, ExpressionSlim body, ExpressionSlim filter)
        {
            Test = test;
            Variable = variable;
            Body = body;
            Filter = filter;
        }

        /// <summary>
        /// Gets a reference to the <see cref="Exception"/> object caught by this handler.
        /// </summary>
        public ParameterExpressionSlim Variable { get; }

        /// <summary>
        /// Gets the type of <see cref="Exception"/> this handler catches.
        /// </summary>
        public TypeSlim Test { get; }

        /// <summary>
        /// Gets the body of the catch block.
        /// </summary>
        public ExpressionSlim Body { get; }

        /// <summary>
        /// Gets the body of the <see cref="CatchBlockSlim"/>'s filter.
        /// </summary>
        public ExpressionSlim Filter { get; }

        /// <summary>
        /// Creates a new expression that is like this one, but using the supplied children. If all of the children are the same, it will return this expression.
        /// </summary>
        /// <param name="variable">The <see cref="Variable" /> property of the result.</param>
        /// <param name="filter">The <see cref="Filter" /> property of the result.</param>
        /// <param name="body">The <see cref="Body" /> property of the result.</param>
        /// <returns>This expression if no children changed, or an expression with the updated children.</returns>
        public CatchBlockSlim Update(ParameterExpressionSlim variable, ExpressionSlim filter, ExpressionSlim body)
        {
            if (variable == Variable && filter == Filter && body == Body)
            {
                return this;
            }

            return new CatchBlockSlim(Test, variable, body, filter);
        }
    }
}
