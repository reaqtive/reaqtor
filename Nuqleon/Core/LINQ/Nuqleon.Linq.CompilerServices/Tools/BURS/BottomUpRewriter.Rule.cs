// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - May 2013 - Created this file.
//

using System.Diagnostics;
using System.Globalization;
using System.Linq.Expressions;

namespace System.Linq.CompilerServices
{
    /// <summary>
    /// Represents a rewrite rule.
    /// </summary>
    /// <typeparam name="TSource">Type of the source tree nodes.</typeparam>
    /// <typeparam name="TTarget">Type of the target tree nodes.</typeparam>
    public sealed class Rule<TSource, TTarget> : RuleBase<TSource, TTarget>
    {
        internal Rule(LambdaExpression pattern, Func<TSource[], TSource> getPattern, LambdaExpression goal, Func<TTarget[], TTarget> invokeGoal, int cost)
            : base(cost)
        {
            Debug.Assert(pattern.Parameters.Count == goal.Parameters.Count, "Wildcard count mismatch between pattern and goal.");

            Pattern = pattern;
            GetPattern = getPattern;
            Goal = goal;
            InvokeGoal = invokeGoal;
        }

        /// <summary>
        /// Gets the lambda expression representing the pattern matched by the rule.
        /// </summary>
        public LambdaExpression Pattern { get; }

        /// <summary>
        /// Gets the lambda expression representing the rewrite goal applied by the rule.
        /// </summary>
        public LambdaExpression Goal { get; }

        internal Func<TSource[], TSource> GetPattern { get; }

        internal Func<TTarget[], TTarget> InvokeGoal { get; }

        /// <summary>
        /// Gets a diagnostic string representation of the rewrite rule.
        /// </summary>
        /// <returns>String representation of the rewrite rule, used for diagnostic purposes.</returns>
        public override string ToString() => string.Format(CultureInfo.InvariantCulture, "{0} --({1}$)--> {2}", Pattern, Cost, Goal);
    }
}
