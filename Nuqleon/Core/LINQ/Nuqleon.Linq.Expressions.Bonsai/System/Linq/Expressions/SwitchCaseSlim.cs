// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// PS - December 2014 - Created this file.
//

using System.Collections.ObjectModel;

namespace System.Linq.Expressions
{
    // PERF: Consider introducing a specialization for a switch case with only one test value.

    /// <summary>
    /// Lightweight representation of one case of a <see cref="SwitchExpressionSlim" />.
    /// </summary>
    public sealed class SwitchCaseSlim
    {
        internal SwitchCaseSlim(ExpressionSlim body, ReadOnlyCollection<ExpressionSlim> testValues)
        {
            Body = body;
            TestValues = testValues;
        }

        /// <summary>
        /// Gets the values of this case. This case is selected for execution when the <see cref="SwitchExpressionSlim.SwitchValue"/> matches any of these values.
        /// </summary>
        public ReadOnlyCollection<ExpressionSlim> TestValues { get; }

        /// <summary>
        /// Gets the body of this case.
        /// </summary>
        public ExpressionSlim Body { get; }

        /// <summary>
        /// Creates a new expression that is like this one, but using the supplied children. If all of the children are the same, it will return this expression.
        /// </summary>
        /// <param name="testValues">The <see cref="TestValues" /> property of the result.</param>
        /// <param name="body">The <see cref="Body" /> property of the result.</param>
        /// <returns>This expression if no children changed, or an expression with the updated children.</returns>
        public SwitchCaseSlim Update(ReadOnlyCollection<ExpressionSlim> testValues, ExpressionSlim body)
        {
            if (testValues == TestValues && body == Body)
            {
                return this;
            }

            return new SwitchCaseSlim(body, testValues);
        }
    }
}
