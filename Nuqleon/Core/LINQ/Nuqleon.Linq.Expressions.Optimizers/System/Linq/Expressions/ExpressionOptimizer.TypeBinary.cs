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
        /// Visits a type binary expression to perform optimization steps.
        /// </summary>
        /// <param name="node">The type binary expression to visit.</param>
        /// <returns>The result of optimizing the type binary expression.</returns>
        protected override Expression VisitTypeBinary(TypeBinaryExpression node)
        {
            var res = (TypeBinaryExpression)base.VisitTypeBinary(node);

            AssertTypes(node, res);

            var expression = res.Expression;

            if (AlwaysThrows(expression))
            {
                return ChangeType(expression, res.Type);
            }

            var typeCheck = AnalyzeTypeCheckResult.Unknown;

            switch (node.NodeType)
            {
                case ExpressionType.TypeIs:
                    typeCheck = AnalyzeTypeIs(res);
                    break;
                case ExpressionType.TypeEqual:
                    typeCheck = AnalyzeTypeEqual(res);
                    break;
            }

            return typeCheck switch
            {
                AnalyzeTypeCheckResult.KnownTrue => Constant(node, value: true),
                AnalyzeTypeCheckResult.KnownFalse => Constant(node, value: false),
                _ => res,
            };
        }

        /// <summary>
        /// Tries to predict the outcome of an `isinst` instruction at compile time.
        /// </summary>
        /// <param name="node">The expression to check.</param>
        /// <returns>The result of analyzing the type checking behavior of the specified expression.</returns>
        private AnalyzeTypeCheckResult AnalyzeTypeIs(TypeBinaryExpression node)
        {
            return AnalyzeTypeIs(node.Expression, node.TypeOperand);
        }

        /// <summary>
        /// Tries to predict the outcome of an `isinst` instruction at compile time.
        /// </summary>
        /// <param name="operand">The operand to check.</param>
        /// <param name="typeOperand">The type to check the operand against.</param>
        /// <returns>The result of analyzing the type checking behavior of the specified expression.</returns>
        private AnalyzeTypeCheckResult AnalyzeTypeIs(Expression operand, Type typeOperand)
        {
            if (!IsPure(operand))
            {
                return AnalyzeTypeCheckResult.Unknown;
            }

            var operandType = operand.Type;

            //
            // NB: This is LINQ v1 behavior and is also present in the lambda compiler.
            //
            if (operandType == typeof(void))
            {
                return AnalyzeTypeCheckResult.KnownFalse;
            }

            //
            // NB: Type comparisons treat nullable types as if they were the underlying type. The
            //     reason is when you box a nullable it becomes a boxed value of the underlying type,
            //     or null.
            //
            var nonNullOperandType = operandType.GetNonNullableType();
            var nonNullTypeOperand = typeOperand.GetNonNullableType();

            if (nonNullTypeOperand.IsAssignableFrom(nonNullOperandType))
            {
                if (operandType.IsValueType && !operandType.IsNullableType())
                {
                    return AnalyzeTypeCheckResult.KnownTrue;
                }

                if (IsAlwaysNull(operand))
                {
                    return AnalyzeTypeCheckResult.KnownFalse;
                }
                else if (IsNeverNull(operand))
                {
                    //
                    // NB: Note that we checked higher up that the operand is pure, so we don't
                    //     have to worry about the operand being a non-null node that may throw
                    //     an exception.
                    //
                    return AnalyzeTypeCheckResult.KnownTrue;
                }

                //
                // NB: Needs a null-check at runtime.
                //
                return AnalyzeTypeCheckResult.KnownAssignable;
            }
            else
            {
                //
                // NB: Avoid issues with variance checks here. Only if we know that a type is
                //     sealed, we can rest assured that the operand can not implement some
                //     interface that the code is checking for at runtime.
                //
                // CONSIDER: This may be overly conservative, so more cases could be added where
                //           we can predict the outcome statically.
                //
                if (IsSealedNonGeneric(nonNullOperandType) && !nonNullTypeOperand.IsGenericType)
                {
                    return AnalyzeTypeCheckResult.KnownFalse;
                }
            }

            return AnalyzeTypeCheckResult.Unknown;
        }

        /// <summary>
        /// Tries to predict the outcome of an exact type check at compile time.
        /// </summary>
        /// <param name="node">The expression to check.</param>
        /// <returns>The result of analyzing the type checking behavior of the specified expression.</returns>
        private AnalyzeTypeCheckResult AnalyzeTypeEqual(TypeBinaryExpression node)
        {
            var operand = node.Expression;

            if (IsAlwaysNull(operand))
            {
                return AnalyzeTypeCheckResult.KnownFalse;
            }

            if (!IsPure(operand))
            {
                return AnalyzeTypeCheckResult.Unknown;
            }

            var operandType = operand.Type;
            var typeOperand = node.TypeOperand;

            if (operandType.IsValueType && !operandType.IsNullableType())
            {
                if (operandType == typeOperand.GetNonNullableType())
                {
                    return AnalyzeTypeCheckResult.KnownTrue;
                }
                else
                {
                    return AnalyzeTypeCheckResult.KnownFalse;
                }
            }

            if (!HasConstantValue(operand))
            {
                return AnalyzeTypeCheckResult.Unknown;
            }

            var value = GetConstantValue(operand);

            Debug.Assert(value != null, "IsNull check should have caught this earlier.");

            if (typeOperand.GetNonNullableType() == value.GetType())
            {
                return AnalyzeTypeCheckResult.KnownTrue;
            }
            else
            {
                return AnalyzeTypeCheckResult.KnownFalse;
            }
        }

        /// <summary>
        /// Checks if the specified <paramref name="type"/> is a non-generic sealed type.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns><c>true</c> if the specified <paramref name="type"/> is a non-generic sealed type; otherwise, false.</returns>
        private static bool IsSealedNonGeneric(Type type) => !type.IsGenericType && type.IsSealed;

        /// <summary>
        /// The result of a compile-time static type analysis.
        /// </summary>
        private enum AnalyzeTypeCheckResult
        {
            /// <summary>
            /// The result of the type check is unknown at compile time.
            /// </summary>
            Unknown,

            /// <summary>
            /// The result of the type check is known to be always <c>false</c>.
            /// </summary>
            KnownFalse,

            /// <summary>
            /// The result of the type check is known to be always <c>true</c>.
            /// </summary>
            KnownTrue,

            /// <summary>
            /// The result of the type check depends on whether the operand is <c>null</c>.
            /// </summary>
            /// <remarks>
            /// In case this value is returned we could reduce a type check expression to a null-check.
            /// However, such a tree is strictly larger so we treat this outcome the same as an unknown
            /// outcome and perform the operation at runtime instead. Note that the lambda compiler can
            /// and will still optimize these cases using null-checking IL instructions.
            /// </remarks>
            KnownAssignable,
        }
    }
}
