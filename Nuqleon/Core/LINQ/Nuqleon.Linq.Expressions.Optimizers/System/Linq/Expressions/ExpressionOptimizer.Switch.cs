// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2017 - Created this file.
//

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1062 // Omitted null checks similar to expression tree visitors.

using System.Diagnostics;

namespace System.Linq.Expressions
{
    public partial class ExpressionOptimizer
    {
        /// <summary>
        /// Visits a switch expression to perform optimization steps.
        /// </summary>
        /// <param name="node">The switch expression to visit.</param>
        /// <returns>The result of optimizing the switch expression.</returns>
        protected override Expression VisitSwitch(SwitchExpression node)
        {
            // REVIEW: Should we visit some nodes as `const`?

            var res = (SwitchExpression)base.VisitSwitch(node);

            AssertTypes(node, res);

            var switchValue = node.SwitchValue;

            if (AlwaysThrows(switchValue))
            {
                return ChangeType(switchValue, res.Type);
            }

            if (HasConstantValue(switchValue))
            {
                //
                // NB: Evaluation of tests in a switch expression happens either using a switch table
                //     if all test expressions are constant, or through a sequential if-then-goto
                //     series of checks. As such, we can scan the cases left-to-right to cover both
                //     variants. Once we find a test value that's neither a throw nor a constant, we
                //     can simply bail out. Otherwise, we keep performing equality checks.
                //

                var comparison = GetComparison(res);

                if (comparison != null)
                {
                    var switchValueValue = GetConstantValue(switchValue);

                    var cases = res.Cases;

                    for (int i = 0, n = cases.Count; i < n; i++)
                    {
                        var @case = cases[i];

                        var testValues = @case.TestValues;

                        for (int j = 0, m = testValues.Count; j < m; j++)
                        {
                            var testValue = testValues[j];

                            if (AlwaysThrows(testValue))
                            {
                                return ChangeType(testValue, res.Type);
                            }

                            if (!HasConstantValue(testValue))
                            {
                                return res;
                            }

                            var testValueValue = GetConstantValue(testValue);

                            var isEqual = false;

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1031 // Catch more specific exception type. (By design.)

                            try
                            {
                                isEqual = comparison(switchValueValue, testValueValue);
                            }
                            catch (Exception ex)
                            {
                                return Throw(node, ex, res.Type);
                            }

#pragma warning restore CA1031
#pragma warning restore IDE0079

                            if (isEqual)
                            {
                                return ChangeType(@case.Body, res.Type);
                            }
                        }
                    }

                    return ChangeType(res.DefaultBody, res.Type);
                }
            }

            return res;
        }

        /// <summary>
        /// Gets a comparison delegate used to evaluate the tests of the specified switch expression.
        /// </summary>
        /// <param name="node">The switch expression for which to get a comparison delegate.</param>
        /// <returns>
        /// A comparison delegate taking the switch value and test value to compare, returning a Boolean
        /// value indicating value equality. If the comparison method is not considered pure, this method
        /// returns <c>null</c>.
        /// </returns>
        private Func<object, object, bool> GetComparison(SwitchExpression node)
        {
            Debug.Assert(node != null);

            if (node.Comparison == null)
            {
                return Equals;
            }

            if (IsPure(node.Comparison))
            {
                var eval = (Func<object, object, object>)GetEvaluator(node.Comparison);
                return (l, r) => (bool)eval(l, r);
            }

            return null;
        }
    }
}
