// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - February 2017 - Created this file.
//

using System.Reflection;

namespace System.Linq.Expressions
{
    /// <summary>
    /// Interface representing a factory for evaluators of members and expressions.
    /// </summary>
    public interface IEvaluatorFactory
    {
        /// <summary>
        /// Gets an evaluator delegate for the specified <paramref name="member"/>.
        /// </summary>
        /// <param name="member">The member to get an evaluator delegate for.</param>
        /// <returns>An evaluator delegate for the specified <paramref name="member"/>.</returns>
        Delegate GetEvaluator(MemberInfo member);

        /// <summary>
        /// Gets an evaluator delegate for the specified <paramref name="expression"/> that's free of parameters.
        /// </summary>
        /// <param name="expression">The expression to get an evaluator delegate for.</param>
        /// <returns>A delegate that returns an <see cref="object"/> with the result of evaluating the expression.</returns>
        Func<object> GetEvaluator(Expression expression);

        /// <summary>
        /// Get an evaluator delegate for the specified unary expression template. The operand of the template expression
        /// will be substituted for the parameter passed in to the evaluator.
        /// </summary>
        /// <param name="node">The unary expression template to build an evaluator for.</param>
        /// <returns>An evaluator delegate to evaluate the specified parameterized unary expression.</returns>
        Func<object, object> GetEvaluator(UnaryExpression node);

        /// <summary>
        /// Get an evaluator delegate for the specified binary expression template. The operands of the template expression
        /// will be substituted for the parameters passed in to the evaluator.
        /// </summary>
        /// <param name="node">The binary expression template to build an evaluator for.</param>
        /// <returns>An evaluator delegate to evaluate the specified parameterized binary expression.</returns>
        Func<object, object, object> GetEvaluator(BinaryExpression node);
    }
}
