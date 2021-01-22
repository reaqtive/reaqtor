// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace Reaqtive.Expressions
{
    /// <summary>
    /// Extensions methods for the expression options interface.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class ExpressionEvaluationPolicyExtensions
    {
        /// <summary>
        /// Evaluates the expression using the given expression policy.
        /// </summary>
        /// <typeparam name="T">The expected result type.</typeparam>
        /// <param name="policy">The expression policy.</param>
        /// <param name="expression">The expression.</param>
        /// <returns>The result of evaluating the expression with the given policy.</returns>
        public static T Evaluate<T>(this IExpressionEvaluationPolicy policy, Expression expression)
        {
            if (policy == null)
            {
                throw new ArgumentNullException(nameof(policy));
            }

            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            var delegateCache = policy.DelegateCache;
            if (delegateCache == null)
            {
                throw new ArgumentException("Expected non-null compiled delegate cache.", nameof(policy));
            }

            var constantHoister = policy.ConstantHoister;
            if (constantHoister == null)
            {
                throw new ArgumentException("Expected non-null constant hoister.", nameof(policy));
            }

            var lambda = Expression.Lambda<Func<T>>(expression);
            return lambda.Compile(delegateCache, policy.OutlineCompilation, constantHoister)();
        }
    }
}
