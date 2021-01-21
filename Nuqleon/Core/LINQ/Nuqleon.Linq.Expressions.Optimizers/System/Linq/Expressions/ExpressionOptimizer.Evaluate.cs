// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2017 - Created this file.
//

using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reflection;

namespace System.Linq.Expressions
{
    public partial class ExpressionOptimizer
    {
        /// <summary>
        /// Gets an evaluator delegate for the specified (pure) <paramref name="member"/>.
        /// </summary>
        /// <param name="member">The member to get an evaluator delegate for.</param>
        /// <returns>A delegate that takes zero or more <see cref="object"/> parameters and returns either <see cref="object"/> or <see cref="void"/>.</returns>
        protected virtual Delegate GetEvaluator(MemberInfo member) => EvaluatorFactory.GetEvaluator(member);

        /// <summary>
        /// Gets an evaluator delegate for the specified <paramref name="expression"/> that's free of parameters.
        /// </summary>
        /// <param name="expression">The expression to get an evaluator delegate for.</param>
        /// <returns>A delegate that returns an <see cref="object"/> with the result of evaluating the expression.</returns>
        protected virtual Func<object> GetEvaluator(Expression expression) => EvaluatorFactory.GetEvaluator(expression);

        /// <summary>
        /// Gets an evaluator delegate for the specified unary <paramref name="expression"/>.
        /// </summary>
        /// <param name="expression">The unary expression to get an evaluator delegate for.</param>
        /// <returns>A delegate that takes an <see cref="object"/> operand and returns an <see cref="object"/> result.</returns>
        protected virtual Func<object, object> GetEvaluator(UnaryExpression expression) => EvaluatorFactory.GetEvaluator(expression);

        /// <summary>
        /// Evaluates the specified <paramref name="expression"/> which has constant value operands using the specified
        /// <paramref name="evaluator"/> delegate. The operands of the expression are passed in <paramref name="instance"/>
        /// and <paramref name="arguments"/>, both of which may be <c>null</c>.
        /// </summary>
        /// <param name="expression">The expression to evaluate.</param>
        /// <param name="evaluator">The evaluator delegate used to evaluate the expression.</param>
        /// <param name="instance">The instance of the expression (e.g. for a <see cref="ExpressionType.Call"/>, or <c>null</c>.</param>
        /// <param name="arguments">The arguments of the expression (e.g. for a <see cref="ExpressionType.Call"/>, or <c>null</c>.</param>
        /// <returns>
        /// An expression representing the result of evaluating the expression. Note that this expression may represent a
        /// <see cref="ExpressionType.Throw"/> if the expression evaluation threw an exception.
        /// </returns>
        private Expression Evaluate(Expression expression, Delegate evaluator, Expression instance, ReadOnlyCollection<Expression> arguments)
        {
            Debug.Assert(evaluator != null); // REVIEW: Should we allow a derived class to return null?

            var n = (instance != null ? 1 : 0) + (arguments?.Count ?? 0);

            var values = new object[n];

            var i = 0;

            if (instance != null)
            {
                values[i++] = GetConstantValue(instance);
            }

            if (arguments != null)
            {
                for (int j = 0, m = arguments.Count; j < m; j++)
                {
                    values[i++] = GetConstantValue(arguments[j]);
                }
            }

            return Evaluate(expression, evaluator, values);
        }

        /// <summary>
        /// Evaluates the specified <paramref name="expression"/> using the specified <paramref name="evaluator"/>
        /// delegate and the specified argument values.
        /// </summary>
        /// <param name="expression">The expression to evaluate.</param>
        /// <param name="evaluator">The evaluator delegate used to evaluate the expression.</param>
        /// <param name="argValues">The argument values to pass to the evaluator delegate.</param>
        /// <returns>
        /// An expression representing the result of evaluating the expression. Note that this expression may represent a
        /// <see cref="ExpressionType.Throw"/> if the expression evaluation threw an exception.
        /// </returns>
        private Expression Evaluate(Expression expression, Delegate evaluator, object[] argValues)
        {
            Debug.Assert(evaluator != null); // REVIEW: Should we allow a derived class to return null?

            object res;

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1031 // Catch more specific exception type. (By design.)

            try
            {
                res = evaluator.DynamicInvoke(argValues);
            }
            catch (Exception ex)
            {
                if (ex is TargetInvocationException tie)
                {
                    ex = tie.InnerException;
                }

                return Throw(expression, ex, expression.Type);
            }

#pragma warning restore CA1031
#pragma warning restore IDE0079

            return Constant(expression, res, expression.Type);
        }
    }
}
