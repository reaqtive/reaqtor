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

namespace System.Linq.Expressions
{
    public partial class ExpressionOptimizer
    {
        /// <summary>
        /// An expression to create a new instance of <see cref="NullReferenceException"/>.
        /// </summary>
        private static readonly NewExpression NullReferenceException = Expression.New(typeof(NullReferenceException).GetConstructor(Type.EmptyTypes));

        /// <summary>
        /// An expression to create a new instance of <see cref="IndexOutOfRangeException"/>.
        /// </summary>
        private static readonly NewExpression IndexOutOfRangeException = Expression.New(typeof(IndexOutOfRangeException).GetConstructor(Type.EmptyTypes));

        /// <summary>
        /// Asserts that the typing of an expression didn't change due to an optimization.
        /// </summary>
        /// <param name="original">The original expression tree.</param>
        /// <param name="replacement">The result of optimizing the expression tree.</param>
        [Conditional("DEBUG")]
        private static void AssertTypes(Expression original, Expression replacement)
        {
            Debug.Assert(original == replacement || original.Type == replacement.Type);
        }

        /// <summary>
        /// Analyzes the left-to-right evaluation of the specified <paramref name="first"/> expression
        /// (if any) and the specified <paramref name="expressions"/>.
        /// </summary>
        /// <param name="first">The first expression to evaluate.</param>
        /// <param name="expressions">The subsequent expressions to evaluate.</param>
        /// <returns>The result of analyzing left-to-right evaluation of the specified expressions.</returns>
        private EvaluationAnalysis AnalyzeLeftToRight(Expression first, ReadOnlyCollection<Expression> expressions)
        {
            var allPure = true;
            var allConst = true;

            if (first != null)
            {
                if (AlwaysThrows(first))
                {
                    return new EvaluationAnalysis { Throw = first };
                }

                allPure &= IsPure(first);
                allConst &= HasConstantValue(first);
            }

            for (int i = 0, n = expressions.Count; i < n && allPure; i++)
            {
                var arg = expressions[i];

                if (AlwaysThrows(arg))
                {
                    return new EvaluationAnalysis { Throw = arg };
                }

                allPure &= IsPure(arg);
                allConst &= HasConstantValue(arg);
            }

            return new EvaluationAnalysis { AllConstant = allConst, AllPure = allPure };
        }

        /// <summary>
        /// Evaluation analysis returned by <see cref="EvaluationAnalysis"/>. This value can be
        /// used to decide on optimization steps.
        /// </summary>
        private struct EvaluationAnalysis
        {
            /// <summary>
            /// Indicates that all analyzed expressions represent a constant.
            /// </summary>
            /// <example>
            /// <code>f(1, 2, 3)</code>
            /// </example>
            public bool AllConstant;

            /// <summary>
            /// Indicates that all analyzed expressions are pure. i.e. don't have side-effects.
            /// </summary>
            /// <example>
            /// <code>f(x, 2, z)</code>
            /// </example>
            public bool AllPure;

            /// <summary>
            /// Indicates that evaluation of the analyzed expressions will cause an unconditional
            /// exception without any other side-effects.
            /// </summary>
            /// <example>
            /// <code>f(1, y, throw e)</code>
            /// </example>
            public Expression Throw;
        }
    }
}
