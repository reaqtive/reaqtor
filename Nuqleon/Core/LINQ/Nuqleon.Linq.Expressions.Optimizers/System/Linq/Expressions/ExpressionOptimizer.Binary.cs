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

using System.Collections.Concurrent;
using System.Diagnostics;
using System.Reflection;

namespace System.Linq.Expressions
{
    public partial class ExpressionOptimizer
    {
        /// <summary>
        /// Cache of evaluators for binary expressions without a custom method.
        /// </summary>
        /// <remarks>
        /// The size of this cache has an upper bound determined by the supported operand types for binary operators.
        /// It merely acts to reduce code size of pre-compiled binary evaluators for the different primitive types,
        /// which may also be error-prone due to potential semantic differences between C# and the expression tree
        /// runtime compiler.
        /// </remarks>
        private static readonly ConcurrentDictionary<Binary, Func<object, object, object>> s_binaryEvaluators = new();

        /// <summary>
        /// Visits a binary expression with a <see cref="BinaryExpression.Conversion"/> specified to perform optimization steps.
        /// </summary>
        /// <param name="node">The binary expression to visit.</param>
        /// <param name="isAssignment">Indicates whether the node represents an assignment.</param>
        /// <returns>The result of optimizing the binary expression.</returns>
        protected override Expression VisitBinaryWithConversion(BinaryExpression node, bool isAssignment)
        {
            var res = (BinaryExpression)base.VisitBinaryWithConversion(node, isAssignment);

            AssertTypes(node, res);

            if (!isAssignment)
            {
                Debug.Assert(node.NodeType == ExpressionType.Coalesce);

                return OptimizeBinary(res);
            }

            return res;
        }

        /// <summary>
        /// Visits a binary expression with no <see cref="BinaryExpression.Conversion"/> specified to perform optimization steps.
        /// </summary>
        /// <param name="node">The binary expression to visit.</param>
        /// <param name="isAssignment">Indicates whether the node represents an assignment.</param>
        /// <returns>The result of optimizing the binary expression.</returns>
        protected override Expression VisitBinaryWithoutConversion(BinaryExpression node, bool isAssignment)
        {
            var res = (BinaryExpression)base.VisitBinaryWithoutConversion(node, isAssignment);

            AssertTypes(node, res);

            if (!isAssignment)
            {
                return OptimizeBinary(res);
            }

            return res;
        }

        /// <summary>
        /// Visits a binary expression to perform optimization steps.
        /// </summary>
        /// <param name="node">The binary expression to visit.</param>
        /// <returns>The result of optimizing the binary expression.</returns>
        private Expression OptimizeBinary(BinaryExpression node)
        {
            var left = node.Left;

            if (AlwaysThrows(left))
            {
                return ChangeType(left, node.Type);
            }

            return node.NodeType switch
            {
                ExpressionType.ArrayIndex => OptimizeArrayIndex(node),
                ExpressionType.Coalesce => OptimizeCoalesce(node),
                ExpressionType.AndAlso or ExpressionType.OrElse => OptimizeAndAlsoOrElse(node),
                _ => OptimizeBinaryOther(node),
            };
        }

        /// <summary>
        /// Optimizes an <see cref="ExpressionType.ArrayIndex"/> expression.
        /// </summary>
        /// <param name="node">The expression to optimize.</param>
        /// <returns>The result of optimizing the expression.</returns>
        private Expression OptimizeArrayIndex(BinaryExpression node)
        {
            // CONSIDER: We could support `new[] { x0, x1, ..., xn }[i] -> xi` and `(new T[n])[i] -> default(T)`.

            if (IsPure(node.Left))
            {
                if (AlwaysThrows(node.Right))
                {
                    return ChangeType(node.Right, node.Type);
                }

                if (IsPure(node.Right))
                {
                    if (IsAlwaysNull(node.Left))
                    {
                        return Throw(node, NullReferenceException, node.Type);
                    }

                    // REVIEW: Omitting a check for constant folding here; the node returns an existing
                    //         element from an array, so this node can't introduce a new mutable value.

                    if (/* CanConstantFold(node) && */ HasConstantValue(node.Left) && HasConstantValue(node.Right))
                    {
                        return EvaluateArrayIndex(node);
                    }
                }
            }

            return node;
        }

        /// <summary>
        /// Evaluates an <see cref="ExpressionType.ArrayIndex"/> expression with constant operands.
        /// </summary>
        /// <param name="node">The expression to evaluate.</param>
        /// <returns>The result of evaluating the expression.</returns>
        private Expression EvaluateArrayIndex(BinaryExpression node)
        {
            var array = (Array)GetConstantValue(node.Left);
            var index = (int)GetConstantValue(node.Right);

            object value;

            try
            {
                value = array.GetValue(index);
            }
            catch (IndexOutOfRangeException)
            {
                return Throw(node, IndexOutOfRangeException, node.Type);
            }

            return Constant(node, value, node.Type);
        }

        /// <summary>
        /// Optimizes an <see cref="ExpressionType.Coalesce"/> expression.
        /// </summary>
        /// <param name="node">The expression to optimize.</param>
        /// <returns>The result of optimizing the expression.</returns>
        private Expression OptimizeCoalesce(BinaryExpression node)
        {
            if (IsNeverNull(node.Left))
            {
                var conversion = node.Conversion;

                if (conversion != null)
                {
                    var p = conversion.Parameters[0];
                    var a = ChangeType(node.Left, p.Type);

                    //
                    // NB: This will cause further beta reduction in VisitLambdaInvocation. For example:
                    //
                    //       f() ?? g() --> (x => (T)x)(f()) --> (T)f()
                    //
                    var convert = Expression.Invoke(conversion, a);
                    return Visit(convert);
                }
                else
                {
                    return ChangeType(node.Left, node.Type);
                }
            }

            if (IsAlwaysNull(node.Left) && IsPure(node.Left))
            {
                return ChangeType(node.Right, node.Type);
            }

            return node;
        }

        /// <summary>
        /// Optimizes an <see cref="ExpressionType.AndAlso"/> expression or <see cref="ExpressionType.OrElse"/> expression.
        /// </summary>
        /// <param name="node">The expression to optimize.</param>
        /// <returns>The result of optimizing the expression.</returns>
        private Expression OptimizeAndAlsoOrElse(BinaryExpression node)
        {
            var isLiftedLogical = IsLiftedLogical(node);

            if (node.Method != null && !isLiftedLogical)
            {
                return OptimizeMethodAndAlsoOrElse(node);
            }
            else if (node.Left.Type == typeof(bool?))
            {
                return OptimizeLiftedAndAlsoOrElse(node);
            }
            else if (isLiftedLogical)
            {
                return OptimizeUserDefinedLiftedAndAlsoOrElse(node);
            }
            else
            {
                return OptimizeUnliftedAndAlsoOrElse(node);
            }
        }

        /// <summary>
        /// Checks if the specified binary expression represented a lifted logical operator, i.e. an expression of
        /// type <see cref="ExpressionType.AndAlso"/> or <see cref="ExpressionType.OrElse"/> applied to nullable
        /// operands of types which have overloaded logical operators.
        /// </summary>
        /// <param name="node">The expression to check.</param>
        /// <returns><c>true</c> if the expression represents a lifted logical operation; otherwise, <c>false</c>.</returns>
        private static bool IsLiftedLogical(BinaryExpression node)
        {
            Debug.Assert(node.NodeType is ExpressionType.AndAlso or ExpressionType.OrElse);

            var left = node.Left.Type;
            var right = node.Right.Type;
            var method = node.Method;

            return TypeUtils.AreEquivalent(left, right)
                && left.IsNullableType()
                && method != null
                && TypeUtils.AreEquivalent(method.ReturnType, left.GetNonNullableType());
        }

        /// <summary>
        /// Searches for an operator method on the type. The method must have the specified signature,
        /// no generic arguments, and have the SpecialName bit set. Also searches inherited operator methods.
        /// </summary>
        /// <param name="type">The type for which to get the operator.</param>
        /// <param name="name">The name of the operator to get.</param>
        /// <returns>The method implementing the operator, if found; otherwise, false.</returns>
        /// <remarks>
        /// This was designed to satisfy the needs of op_True and op_False, because we have to do runtime lookup
        /// for those. It may not work right for unary operators in general.
        /// </remarks>
        internal static MethodInfo GetBooleanOperator(Type type, string name)
        {
            do
            {
                var result = type.GetMethod(name, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, binder: null, new[] { type }, modifiers: null);

                if (result != null && result.IsSpecialName && !result.ContainsGenericParameters)
                {
                    return result;
                }

                type = type.BaseType;
            } while (type != null);

            return null;
        }

        /// <summary>
        /// Optimizes an <see cref="ExpressionType.AndAlso"/> expression or <see cref="ExpressionType.OrElse"/> expression
        /// with a custom <see cref="BinaryExpression.Method"/> specified and not lifted.
        /// </summary>
        /// <param name="node">The expression to optimize.</param>
        /// <returns>The result of optimizing the expression.</returns>
        private Expression OptimizeMethodAndAlsoOrElse(BinaryExpression node)
        {
            if (HasConstantValue(node.Left))
            {
                var truthCheck = TryEvalTruthCheck(node, node.Left);

                if (truthCheck != null)
                {
                    if (AlwaysThrows(truthCheck))
                    {
                        return ChangeType(truthCheck, node.Type);
                    }

                    if (HasConstantValue(truthCheck))
                    {
                        if ((bool)GetConstantValue(truthCheck))
                        {
                            return ChangeType(node.Left, node.Type);
                        }
                        else
                        {
                            if (AlwaysThrows(node.Right))
                            {
                                return ChangeType(node.Right, node.Type);
                            }

                            var method = node.Method;

                            if (CanConstantFold(node) && HasConstantValue(node.Right) && IsPure(method))
                            {
                                var leftValue = GetConstantValue(node.Left);
                                var rightValue = GetConstantValue(node.Right);

                                var evaluator = GetEvaluator(method);
                                return Evaluate(node, evaluator, new[] { leftValue, rightValue });
                            }
                            else
                            {
                                return Expression.MakeBinary(node.NodeType == ExpressionType.AndAlso ? ExpressionType.And : ExpressionType.Or, node.Left, node.Right, liftToNull: false, node.Method);
                            }
                        }
                    }
                }
            }

            return node;
        }

        /// <summary>
        /// Tries to evaluate the Boolean truth check associated with the specified <paramref name="node"/>.
        /// </summary>
        /// <param name="node">The node for which to evaluate the truth check.</param>
        /// <param name="left">The non-null left operand of the binary expression.</param>
        /// <returns>The result of evaluating the truth check, if possible based on purity analysis; otherwise, <c>null</c>.</returns>
        private Expression TryEvalTruthCheck(BinaryExpression node, Expression left)
        {
            var method = node.Method;

            var truthCheckMethodName = node.NodeType == ExpressionType.AndAlso ? "op_False" : "op_True";
            var truthCheckMethod = GetBooleanOperator(method.DeclaringType, truthCheckMethodName);

            // NB: Boolean is always immutable, so no need for constant folding checks here.

            if (IsPure(truthCheckMethod))
            {
                var leftTruthCheck = Expression.Call(instance: null, truthCheckMethod, left);

                return EvaluateMethodCall(leftTruthCheck);
            }

            return null;
        }

        /// <summary>
        /// Optimizes an <see cref="ExpressionType.AndAlso"/> expression or <see cref="ExpressionType.OrElse"/> expression
        /// with lifted operands.
        /// </summary>
        /// <param name="node">The expression to optimize.</param>
        /// <returns>The result of optimizing the expression.</returns>
        private Expression OptimizeLiftedAndAlsoOrElse(BinaryExpression node)
        {
            var result = default(Expression);

            if (IsFalse(node.Left))
            {
                result = node.NodeType == ExpressionType.AndAlso ? Constant(node, s_false, typeof(bool?)) : node.Right;
            }
            else if (IsTrue(node.Left))
            {
                result = node.NodeType == ExpressionType.OrElse ? Constant(node, s_true, typeof(bool?)) : node.Right;
            }
            else if (IsAlwaysNull(node.Left))
            {
                if (AlwaysThrows(node.Right))
                {
                    result = node.Right;
                }
                else if (IsFalse(node.Right))
                {
                    result = node.NodeType == ExpressionType.AndAlso ? Constant(node, s_false, typeof(bool?)) : Null(node, typeof(bool?));
                }
                else if (IsTrue(node.Right))
                {
                    result = node.NodeType == ExpressionType.OrElse ? Constant(node, s_true, typeof(bool?)) : Null(node, typeof(bool?));
                }
                else if (IsAlwaysNull(node.Right))
                {
                    result = Null(node, typeof(bool?));
                }
            }

            if (result != null)
            {
                return ChangeType(result, node.Type);
            }

            return node;
        }

        /// <summary>
        /// Optimizes an <see cref="ExpressionType.AndAlso"/> expression or <see cref="ExpressionType.OrElse"/> expression
        /// with a custom <see cref="BinaryExpression.Method"/> specified and lifted.
        /// </summary>
        /// <param name="node">The expression to optimize.</param>
        /// <returns>The result of optimizing the expression.</returns>
        private Expression OptimizeUserDefinedLiftedAndAlsoOrElse(BinaryExpression node)
        {
            if (IsAlwaysNull(node.Left))
            {
                return Null(node, node.Type);
            }

            if (HasConstantValue(node.Left))
            {
                var leftValue = GetConstantValue(node.Left);

                if (leftValue != null) // NB: Should never be null if IsAlwaysNull is correctly overridden.
                {
                    var leftNonNullValue = Utils.Constant(leftValue, node.Left.Type.GetNonNullableType());

                    var truthCheck = TryEvalTruthCheck(node, leftNonNullValue);

                    if (truthCheck != null)
                    {
                        if (AlwaysThrows(truthCheck))
                        {
                            return ChangeType(truthCheck, node.Type);
                        }

                        if (HasConstantValue(truthCheck))
                        {
                            if ((bool)GetConstantValue(truthCheck))
                            {
                                return ChangeType(node.Left, node.Type);
                            }
                            else
                            {
                                if (AlwaysThrows(node.Right))
                                {
                                    return ChangeType(node.Right, node.Type);
                                }

                                if (IsAlwaysNull(node.Right))
                                {
                                    return Null(node, node.Type);
                                }

                                var method = node.Method;

                                if (CanConstantFold(node) && HasConstantValue(node.Right) && IsPure(method))
                                {
                                    var rightValue = GetConstantValue(node.Right);

                                    if (rightValue != null) // NB: Should never be null if IsAlwaysNull is correctly overridden.
                                    {
                                        var evaluator = GetEvaluator(method);
                                        return Evaluate(node, evaluator, new[] { leftValue, rightValue });
                                    }
                                }
                                else
                                {
                                    return Expression.MakeBinary(node.NodeType == ExpressionType.AndAlso ? ExpressionType.And : ExpressionType.Or, node.Left, node.Right, liftToNull: false, node.Method);
                                }
                            }
                        }
                    }
                }
            }

            return node;
        }

        /// <summary>
        /// Optimizes an <see cref="ExpressionType.AndAlso"/> expression or <see cref="ExpressionType.OrElse"/> expression
        /// with unlifted operands.
        /// </summary>
        /// <param name="node">The expression to optimize.</param>
        /// <returns>The result of optimizing the expression.</returns>
        private Expression OptimizeUnliftedAndAlsoOrElse(BinaryExpression node)
        {
            var result = default(Expression);

            if (IsFalse(node.Left))
            {
                result = node.NodeType == ExpressionType.AndAlso ? Constant(node, value: false) : node.Right;
            }
            else if (IsTrue(node.Left))
            {
                result = node.NodeType == ExpressionType.AndAlso ? node.Right : Constant(node, value: true);
            }

            if (result != null)
            {
                return ChangeType(result, node.Type);
            }

            return node;
        }

        /// <summary>
        /// Optimizes a binary expression that is not an <see cref="ExpressionType.AndAlso"/> expression,
        /// an <see cref="ExpressionType.OrElse"/> expression, or a <see cref="ExpressionType.Coalesce"/> expression.
        /// </summary>
        /// <param name="node">The expression to optimize.</param>
        /// <returns>The result of optimizing the expression.</returns>
        private Expression OptimizeBinaryOther(BinaryExpression node)
        {
            if (IsPure(node.Left) && AlwaysThrows(node.Right))
            {
                return ChangeType(node.Right, node.Type);
            }

            if (node.Method != null)
            {
                return OptimizeBinaryOtherWithMethod(node);
            }
            else
            {
                return OptimizeBinaryOtherWithoutMethod(node);
            }
        }

        /// <summary>
        /// Optimizes a binary expression that is not an <see cref="ExpressionType.AndAlso"/> expression,
        /// an <see cref="ExpressionType.OrElse"/> expression, or a <see cref="ExpressionType.Coalesce"/> expression
        /// and has no custom <see cref="BinaryExpression.Method"/> specified.
        /// </summary>
        /// <param name="node">The expression to optimize.</param>
        /// <returns>The result of optimizing the expression.</returns>
        private Expression OptimizeBinaryOtherWithoutMethod(BinaryExpression node)
        {
            Debug.Assert(node.Method == null);

            if (node.IsLifted)
            {
                var liftedResult = TryEvaluateLiftedBinary(node);
                if (liftedResult != null)
                {
                    return liftedResult;
                }
            }

            if (CanConstantFold(node) && HasConstantValue(node.Left) && HasConstantValue(node.Right))
            {
                return EvaluateBinary(node);
            }

            return OptimizeBinaryAlgebraic(node);
        }

        /// <summary>
        /// Optimizes binary expressions that satisfy some algebraic law.
        /// </summary>
        /// <param name="node">The expression to optimize.</param>
        /// <returns>The result of optimizing the expression.</returns>
        private Expression OptimizeBinaryAlgebraic(BinaryExpression node)
        {
            Debug.Assert(node.Method == null);

            switch (node.NodeType)
            {
                case ExpressionType.Add:
                case ExpressionType.AddChecked:
                    // CONSIDER: Check if there are caveats to enable this for floats as well.
                    if (TypeUtils.IsInteger(node.Type))
                    {
                        if (IsZero(node.Right))
                        {
                            return node.Left;
                        }
                        else if (IsZero(node.Left))
                        {
                            return node.Right;
                        }
                    }
                    break;
                case ExpressionType.Subtract:
                case ExpressionType.SubtractChecked:
                    // CONSIDER: Check if there are caveats to enable this for floats as well.
                    if (TypeUtils.IsInteger(node.Type))
                    {
                        if (IsZero(node.Right))
                        {
                            return node.Left;
                        }
                    }
                    break;
                case ExpressionType.Multiply:
                case ExpressionType.MultiplyChecked:
                    // NB: Not doing this for floats where we have to deal with NaN.
                    if (TypeUtils.IsInteger(node.Type))
                    {
                        if (IsOne(node.Left))
                        {
                            return node.Right;
                        }
                        else if (IsOne(node.Right))
                        {
                            return node.Left;
                        }
                        else if (IsZero(node.Left) && IsPureNeverNull(node.Right))
                        {
                            return node.Left;
                        }
                        else if (IsZero(node.Right) && IsPureNeverNull(node.Left))
                        {
                            return node.Right;
                        }
                    }
                    break;
                case ExpressionType.Divide:
                    // CONSIDER: Check if there are caveats to enable this for floats as well.
                    // CONSIDER: Add compile-time evaluation of DivideByZeroException (also for Modulo).
                    if (TypeUtils.IsInteger(node.Type))
                    {
                        if (IsOne(node.Right))
                        {
                            return node.Left;
                        }
                    }
                    break;
                case ExpressionType.LeftShift:
                case ExpressionType.RightShift:
                    if (IsZero(node.Right))
                    {
                        return node.Left;
                    }
                    break;
                case ExpressionType.And:
                    if (AllBitsOne(node.Left))
                    {
                        return node.Right;
                    }
                    else if (AllBitsOne(node.Right))
                    {
                        return node.Left;
                    }
                    else if (AllBitsZero(node.Left) && IsPureNeverNull(node.Right))
                    {
                        return node.Left;
                    }
                    else if (AllBitsZero(node.Right) && IsPureNeverNull(node.Left))
                    {
                        return node.Right;
                    }
                    break;
                case ExpressionType.Or:
                    if (AllBitsZero(node.Left))
                    {
                        return node.Right;
                    }
                    else if (AllBitsZero(node.Right))
                    {
                        return node.Left;
                    }
                    else if (AllBitsOne(node.Left) && IsPureNeverNull(node.Right))
                    {
                        return node.Left;
                    }
                    else if (AllBitsOne(node.Right) && IsPureNeverNull(node.Left))
                    {
                        return node.Right;
                    }
                    break;
                case ExpressionType.ExclusiveOr:
                    if (IsBitwiseComplement(node.Left) && IsBitwiseComplement(node.Right))
                    {
                        var l = (UnaryExpression)node.Left;
                        var r = (UnaryExpression)node.Right;
                        return OptimizeBinaryAlgebraic(node.Update(l.Operand, conversion: null, r.Operand));
                    }

                    if (AllBitsZero(node.Left))
                    {
                        return node.Right;
                    }
                    else if (AllBitsZero(node.Right))
                    {
                        return node.Left;
                    }
                    else if (AllBitsOne(node.Left) && IsPureNeverNull(node.Right))
                    {
                        if (node.Type.GetNonNullableType() == typeof(bool))
                        {
                            return Expression.Not(node.Right);
                        }
                        else
                        {
                            return Expression.OnesComplement(node.Right);
                        }
                    }
                    else if (AllBitsOne(node.Right) && IsPureNeverNull(node.Left))
                    {
                        if (node.Type.GetNonNullableType() == typeof(bool))
                        {
                            return Expression.Not(node.Left);
                        }
                        else
                        {
                            return Expression.OnesComplement(node.Left);
                        }
                    }
                    break;
                case ExpressionType.LessThan:
                    // NB: Not doing this for floats where we have to worry about special values.
                    if (TypeUtils.IsInteger(node.Left.Type))
                    {
                        // NB: Comparison against null is fine if the node is not lifted to null, which
                        //     results in the comparison returning false.
                        if (IsPureNeverNull(node.Left, node.IsLiftedToNull) && IsMinValue(node.Right) ||
                            IsMaxValue(node.Left) && IsPureNeverNull(node.Right, node.IsLiftedToNull))
                        {
                            return Constant(node, s_false, node.Type);
                        }
                    }
                    break;
                case ExpressionType.LessThanOrEqual:
                    // NB: Not doing this for floats where we have to worry about special values.
                    if (TypeUtils.IsInteger(node.Left.Type))
                    {
                        // NB: We don't use whether or not the node is lifted to null to relax conditions
                        //     (unlike for LessThan) because comparison against null can never return true
                        //     so we can't optimize those cases.
                        if (IsPureNeverNull(node.Left) && IsMaxValue(node.Right) ||
                            IsMinValue(node.Left) && IsPureNeverNull(node.Right))
                        {
                            return Constant(node, s_true, node.Type);
                        }
                    }
                    break;
                case ExpressionType.GreaterThan:
                    // NB: Not doing this for floats where we have to worry about special values.
                    if (TypeUtils.IsInteger(node.Left.Type))
                    {
                        // NB: Comparison against null is fine if the node is not lifted to null, which
                        //     results in the comparison returning false.
                        if (IsPureNeverNull(node.Left, node.IsLiftedToNull) && IsMaxValue(node.Right) ||
                            IsMinValue(node.Left) && IsPureNeverNull(node.Right, node.IsLiftedToNull))
                        {
                            return Constant(node, s_false, node.Type);
                        }
                    }
                    break;
                case ExpressionType.GreaterThanOrEqual:
                    // NB: Not doing this for floats where we have to worry about special values.
                    if (TypeUtils.IsInteger(node.Left.Type))
                    {
                        // NB: We don't use whether or not the node is lifted to null to relax conditions
                        //     (unlike for GreaterThan) because comparison against null can never return true
                        //     so we can't optimize those cases.
                        if (IsPureNeverNull(node.Left) && IsMinValue(node.Right) ||
                            IsMaxValue(node.Left) && IsPureNeverNull(node.Right))
                        {
                            return Constant(node, s_true, node.Type);
                        }
                    }
                    break;
            }

            return node;
        }

        /// <summary>
        /// Checks whether the specified <paramref name="expression"/> represents a bitwise complement defined on
        /// primitive types.
        /// </summary>
        /// <param name="expression">The expression to check.</param>
        /// <returns><c>true</c> if the specified <paramref name="expression"/> represents a bitwise complement; otherwise, <c>false</c>.</returns>
        private static bool IsBitwiseComplement(Expression expression)
        {
            if (expression.NodeType is ExpressionType.Not or ExpressionType.OnesComplement)
            {
                var u = (UnaryExpression)expression;
                return u.Method == null;
            }

            return false;
        }

        /// <summary>
        /// Optimizes a binary expression that is not an <see cref="ExpressionType.AndAlso"/> expression,
        /// an <see cref="ExpressionType.OrElse"/> expression, or a <see cref="ExpressionType.Coalesce"/> expression
        /// and has a custom <see cref="BinaryExpression.Method"/> specified.
        /// </summary>
        /// <param name="node">The expression to optimize.</param>
        /// <returns>The result of optimizing the expression.</returns>
        private Expression OptimizeBinaryOtherWithMethod(BinaryExpression node)
        {
            Debug.Assert(node.Method != null);

            if (node.IsLifted)
            {
                return OptimizeBinaryOtherWithMethodLifted(node);
            }
            else
            {
                return EvaluateBinaryMethod(node);
            }
        }

        /// <summary>
        /// Optimizes a lifted binary expression that is not an <see cref="ExpressionType.AndAlso"/> expression,
        /// an <see cref="ExpressionType.OrElse"/> expression, or a <see cref="ExpressionType.Coalesce"/> expression
        /// and has a custom <see cref="BinaryExpression.Method"/> specified.
        /// </summary>
        /// <param name="node">The expression to optimize.</param>
        /// <returns>The result of optimizing the expression.</returns>
        private Expression OptimizeBinaryOtherWithMethodLifted(BinaryExpression node)
        {
            Debug.Assert(node.Method != null);
            Debug.Assert(node.IsLifted);

            var liftedResult = TryEvaluateLiftedBinary(node);
            if (liftedResult != null)
            {
                return liftedResult;
            }

            if (CanConstantFold(node) && HasConstantValue(node.Left) && HasConstantValue(node.Right) && IsPure(node.Method))
            {
                var leftValue = GetConstantValue(node.Left);
                var rightValue = GetConstantValue(node.Right);

                if (leftValue != null && rightValue != null)
                {
                    var evaluator = GetEvaluator(node.Method);
                    var result = Evaluate(node, evaluator, new[] { leftValue, rightValue });

                    return result;
                }
            }

            return node;
        }

        /// <summary>
        /// Tries to evaluate a lifted binary expression by checking for <c>null</c> operands and propagating
        /// <c>null</c> values when safe to do so with regards to preservation of side-effects.
        /// </summary>
        /// <param name="node">The expression to try to evaluate.</param>
        /// <returns>The result of evaluating the binary expression at compile time, if possible; otherwise, <c>null</c>.</returns>
        private Expression TryEvaluateLiftedBinary(BinaryExpression node)
        {
            Debug.Assert(node.IsLifted);

            var leftIsNull = IsAlwaysNull(node.Left);
            var rightIsNull = IsAlwaysNull(node.Right);

            if ((leftIsNull && IsPure(node.Right)) || (IsPure(node.Left) && rightIsNull))
            {
                if (node.Type.IsNullableType())
                {
                    return Null(node, node.Type);
                }

                if (leftIsNull || rightIsNull)
                {
                    switch (node.NodeType)
                    {
                        case ExpressionType.LessThan:
                        case ExpressionType.LessThanOrEqual:
                        case ExpressionType.GreaterThan:
                        case ExpressionType.GreaterThanOrEqual:
                            return Constant(node, value: false);
                    }
                }

                if (leftIsNull && rightIsNull)
                {
                    switch (node.NodeType)
                    {
                        case ExpressionType.Equal:
                            return Constant(node, value: true);
                        case ExpressionType.NotEqual:
                            return Constant(node, value: false);
                    }
                }
                else
                {
                    var leftIsNeverNull = IsNeverNull(node.Left);
                    var rightIsNeverNull = IsNeverNull(node.Right);

                    if ((leftIsNull && rightIsNeverNull) || (rightIsNull && leftIsNeverNull))
                    {
                        switch (node.NodeType)
                        {
                            case ExpressionType.Equal:
                                return Constant(node, value: false);
                            case ExpressionType.NotEqual:
                                return Constant(node, value: true);
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Evaluates a binary expression that has a custom <see cref="BinaryExpression.Method"/> specified.
        /// </summary>
        /// <param name="node">The binary expression to evaluate.</param>
        /// <returns>An expression representing the result of evaluating the binary expression at compile time.</returns>
        private Expression EvaluateBinaryMethod(BinaryExpression node)
        {
            if (CanConstantFold(node) && HasConstantValue(node.Left) && HasConstantValue(node.Right) && IsPure(node.Method))
            {
                var leftValue = GetConstantValue(node.Left);
                var rightValue = GetConstantValue(node.Right);

                var evaluator = GetEvaluator(node.Method);
                var result = Evaluate(node, evaluator, new[] { leftValue, rightValue });

                return result;
            }

            return node;
        }

        /// <summary>
        /// Evaluates a binary expression that's deemed primitive.
        /// </summary>
        /// <param name="node">The binary expression to evaluate.</param>
        /// <returns>An expression representing the result of evaluating the binary expression at compile time.</returns>
        /// <remarks>
        /// This method should only be called if the binary expression is part of a fixed set of built-in binary
        /// operations because it will populate a static cache that's meant to have an upper size.
        /// </remarks>
        private Expression EvaluateBinary(BinaryExpression node)
        {
            var evaluator = GetBinaryPrimitiveEvaluator(node);

            return EvaluateBinary(node, evaluator);
        }

        /// <summary>
        /// Evaluates a binary expression using the specified <paramref name="evaluator"/> by getting the constant
        /// value of the operand.
        /// </summary>
        /// <param name="node">The binary expression to evaluate.</param>
        /// <param name="evaluator">The evaluator to use.</param>
        /// <returns>An expression representing the result of evaluating the binary expression at compile time.</returns>
        private Expression EvaluateBinary(BinaryExpression node, Func<object, object, object> evaluator)
        {
            return EvaluateBinary(node, evaluator, GetConstantValue(node.Left), GetConstantValue(node.Right));
        }

        /// <summary>
        /// Evaluates a binary expression using the specified <paramref name="evaluator"/> and the specified
        /// constant operand value.
        /// </summary>
        /// <param name="node">The binary expression to evaluate.</param>
        /// <param name="evaluator">The evaluator to use.</param>
        /// <param name="leftValue">The constant left operand value to pass to the evaluator.</param>
        /// <param name="rightValue">The constant right operand value to pass to the evaluator.</param>
        /// <returns>An expression representing the result of evaluating the binary expression at compile time.</returns>
        private Expression EvaluateBinary(BinaryExpression node, Func<object, object, object> evaluator, object leftValue, object rightValue)
        {
            Debug.Assert(evaluator != null); // REVIEW: Should we allow a derived class to return null?

            object resultValue;

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1031 // Catch more specific exception type. (By design.)

            try
            {
                resultValue = evaluator(leftValue, rightValue);
            }
            catch (Exception ex)
            {
                return Throw(node, ex, node.Type);
            }

#pragma warning restore CA1031
#pragma warning restore IDE0079

            return Constant(node, resultValue, node.Type);
        }

        /// <summary>
        /// Gets an evaluator delegate for a binary expression that's deemed primitive.
        /// </summary>
        /// <param name="node">The binary expression to get an evaluator for.</param>
        /// <returns>An evaluator delegate for the binary expression.</returns>
        /// <remarks>
        /// This method should only be called if the binary expression is part of a fixed set of built-in unary
        /// operations because it will populate a static cache that's meant to have an upper size.
        /// </remarks>
        private Func<object, object, object> GetBinaryPrimitiveEvaluator(BinaryExpression node)
        {
            Debug.Assert(node.Method == null);
            Debug.Assert(node.Conversion == null);

            return s_binaryEvaluators.GetOrAdd(new Binary { ExpressionType = node.NodeType, LeftType = node.Left.Type, RightType = node.Right.Type, LiftToNull = node.IsLiftedToNull }, b =>
            {
                var binary = Expression.MakeBinary(b.ExpressionType, Expression.Default(b.LeftType), Expression.Default(b.RightType), b.LiftToNull, method: null, conversion: null);
                return EvaluatorFactory.GetEvaluator(binary);
            });
        }

        /// <summary>
        /// Struct for the key of the binary evaluator cache.
        /// </summary>
        private struct Binary : IEquatable<Binary>
        {
            /// <summary>
            /// The expression type of the binary node.
            /// </summary>
            public ExpressionType ExpressionType;

            /// <summary>
            /// The left operand type of the binary node.
            /// </summary>
            public Type LeftType;

            /// <summary>
            /// The right operand type of the binary node.
            /// </summary>
            public Type RightType;

            /// <summary>
            /// Indicates whether the binary node has a result lifted to null.
            /// </summary>
            public bool LiftToNull;

            /// <summary>
            /// Checks whether the specified <paramref name="other"/> value is equal to the current value.
            /// </summary>
            /// <param name="other">The value to compare the current value to.</param>
            /// <returns><c>true</c> if the current value equals the <paramref name="other"/> value; otherwise, <c>false</c>.</returns>
            public bool Equals(Binary other) => ExpressionType == other.ExpressionType && LeftType == other.LeftType && RightType == other.RightType && LiftToNull == other.LiftToNull;

            /// <summary>
            /// Checks whether the specified object is equal to the current value.
            /// </summary>
            /// <param name="obj">The object to compare the current value to.</param>
            /// <returns><c>true</c> if the current value equals the specified object; otherwise, <c>false</c>.</returns>
            public override bool Equals(object obj) => obj is Binary binary && Equals(binary);

            /// <summary>
            /// Gets a hash code for the current value.
            /// </summary>
            /// <returns>A hash code for the current value.</returns>
            public override int GetHashCode() => HashHelpers.Combine((int)ExpressionType, LeftType.GetHashCode(), RightType.GetHashCode(), LiftToNull ? 1 : 0);
        }
    }
}
