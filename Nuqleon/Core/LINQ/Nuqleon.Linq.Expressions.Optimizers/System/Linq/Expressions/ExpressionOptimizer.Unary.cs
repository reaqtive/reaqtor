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

namespace System.Linq.Expressions
{
    public partial class ExpressionOptimizer
    {
        /// <summary>
        /// Cache of evaluators for unary expressions without a custom method.
        /// </summary>
        /// <remarks>
        /// The size of this cache has an upper bound determined by the supported operand types for unary operators.
        /// It merely acts to reduce code size of pre-compiled unary evaluators for the different primitive types,
        /// which may also be error-prone due to potential semantic differences between C# and the expression tree
        /// runtime compiler.
        /// </remarks>
        private static readonly ConcurrentDictionary<Unary, Func<object, object>> s_unaryEvaluators = new();

        /// <summary>
        /// Visits a unary expression to perform optimization steps.
        /// </summary>
        /// <param name="node">The unary expression to visit.</param>
        /// <param name="isAssignment">Indicates whether the node represents an assignment.</param>
        /// <returns>The result of optimizing the unary expression.</returns>
        protected override Expression VisitUnaryNonQuote(UnaryExpression node, bool isAssignment)
        {
            //
            // NB: We apply algebraic optimizations *twice*, once before visiting the child nodes, and once after
            //     visiting the child nodes. This enables reduction of the tree size before applying recursive
            //     optimizations and relies on OptimizeUnaryAlgebraic to reduce the size of the tree (progress
            //     property of the optimizer). An example where this pays off is when considering the following
            //     algebraic identities:
            //
            //       !(!x & !y) --> x | y  (De Morgan's rule)
            //       !(a == b)  --> a != b
            //
            //     Assume we have an expression containing both optimization opportunities:
            //
            //       !(!(a == b) & !c)
            //
            //     If we perform a depth-first optimization visit and apply algebraic optimizations only after
            //     visiting the children, the second rule will kick in first:
            //
            //       !((a != b) & !c)
            //
            //     at which point there's no further optimizations to be made. We've eliminated one node from
            //     the tree. On the other hand, if we also apply the optimization before recursing into child
            //     nodes, the first rule kicks in first:
            //
            //       (a == b) & c
            //
            //     at which point the second rule doesn't have to considered ever and we ended up eliminating a
            //     total of three nodes from the tree.
            //
            //     Note that similar observations hold for rules such as double-negation:
            //
            //       !!x       --> x
            //       !(a == b) --> a != b
            //       !(a != b) --> a == b
            //
            //     With an expression like this:
            //
            //       !!(a == b)
            //
            //     the end result is the same (`a == b`) but the extra optimization pass at the beginning does
            //     remove the need to apply the second and third rule ever.
            //

            var opt = OptimizeUnaryAlgebraic(node);

            AssertTypes(node, opt);

            if (opt != node)
            {
                return Visit(opt);
            }

            var res = (UnaryExpression)base.VisitUnaryNonQuote(node, isAssignment);

            AssertTypes(node, res);

            if (!isAssignment)
            {
                var operand = res.Operand;

                if (operand == null)
                {
                    Debug.Assert(res.NodeType == ExpressionType.Throw);
                    return res;
                }

                if (AlwaysThrows(operand))
                {
                    return ChangeType(operand, res.Type);
                }

                switch (res.NodeType)
                {
                    case ExpressionType.ArrayLength:
                        return OptimizeArrayLength(res);
                    case ExpressionType.Convert:
                    case ExpressionType.ConvertChecked:
                        return OptimizeConvert(res);
                    case ExpressionType.Throw: // CONSIDER: Could change to NullReferenceException if operand is known to be null.
                    case ExpressionType.Unbox: // CONSIDER: Could reduce to throw if operand is known to be null.
                        return res;
                    case ExpressionType.TypeAs:
                        return OptimizeTypeAs(res);
                }

                if (res.Method == null)
                {
                    // NB: This applies algebraic optimizations again at the end.
                    return OptimizeUnaryWithoutMethod(res);
                }
                else
                {
                    // NB: If we ever end up adding algebraic optimizations that apply to nodes with custom methods
                    //     specified, we could consider moving the call to OptimizeUnaryAlgebraic from WithoutMethod
                    //     to the bottom of this method (VisitUnaryNonQuote).
                    return OptimizeUnaryWithMethod(res);
                }
            }

            return res;
        }

        /// <summary>
        /// Optimizes an <see cref="ExpressionType.ArrayLength"/> expression.
        /// </summary>
        /// <param name="node">The expression to optimize.</param>
        /// <returns>The result of optimizing the expression.</returns>
        private Expression OptimizeArrayLength(UnaryExpression node)
        {
            if (IsAlwaysNull(node.Operand))
            {
                return Throw(node, NullReferenceException, node.Type);
            }

            if (CanConstantFold(node) && HasConstantValue(node.Operand))
            {
                var array = (Array)GetConstantValue(node.Operand);

                return Constant(node, array.Length);
            }

            if (node.Operand is NewArrayExpression newArray)
            {
                switch (newArray.NodeType)
                {
                    case ExpressionType.NewArrayBounds:
                        Debug.Assert(newArray.Expressions.Count == 1);
                        if (CanConstantFold(node) && HasConstantValue(newArray.Expressions[0]))
                        {
                            var lengthValue = (int)GetConstantValue(newArray.Expressions[0]);
                            return Constant(node, lengthValue);
                        }
                        break;
                    case ExpressionType.NewArrayInit:
                        if (AnalyzeLeftToRight(first: null, newArray.Expressions).AllPure)
                        {
                            return Constant(node, newArray.Expressions.Count);
                        }
                        break;
                }
            }

            return node;
        }

        /// <summary>
        /// Optimizes an <see cref="ExpressionType.Convert"/> or <see cref="ExpressionType.ConvertChecked"/> expression.
        /// </summary>
        /// <param name="node">The expression to optimize.</param>
        /// <returns>The result of optimizing the expression.</returns>
        private Expression OptimizeConvert(UnaryExpression node)
        {
            if (node.Method == null)
            {
                return OptimizeConvertWithoutMethod(node);
            }
            else
            {
                return OptimizeConvertWithMethod(node);
            }
        }

        /// <summary>
        /// Optimizes an <see cref="ExpressionType.Convert"/> or <see cref="ExpressionType.ConvertChecked"/> expression
        /// that has no <see cref="UnaryExpression.Method"/> specified.
        /// </summary>
        /// <param name="node">The expression to optimize.</param>
        /// <returns>The result of optimizing the expression.</returns>
        private Expression OptimizeConvertWithoutMethod(UnaryExpression node)
        {
            Debug.Assert(node.Method == null);

            var operand = node.Operand;

            var operandType = operand.Type;
            var resultType = node.Type;

            if (TypeUtils.AreEquivalent(operandType, resultType))
            {
                return operand;
            }
            /*
             * NB: The expression lambda compiler has this case, but it seems impossible to construct a Convert node
             *     with a void destination type.
             *
            else if (resultType == typeof(void))
            {
                if (IsPure(operand))
                {
                    return Expression.Empty();
                }
            }
             */
            else if (TypeUtils.HasPrimitiveConversion(operandType, resultType))
            {
                if (CanConstantFold(node) && HasConstantValue(operand))
                {
                    if (operandType.IsEnum || resultType.IsEnum)
                    {
                        var unary = node.Update(Expression.Parameter(operandType));
                        var evaluator = GetEvaluator(unary);

                        return EvaluateUnary(node, evaluator);
                    }
                    else
                    {
                        return EvaluateUnary(node);
                    }
                }
            }
            else
            {
                if (CanConstantFold(node) && HasConstantValue(operand))
                {
                    var operandTypeIsNullable = operandType.IsNullableType();
                    var resultTypeIsNullable = resultType.IsNullableType();

                    if (operandTypeIsNullable || resultTypeIsNullable)
                    {
                        var operandValue = GetConstantValue(operand);

                        // NB: We can't deal with nullable-to-non-nullable conversion where the operand is null because it
                        //     requires throwing an InvalidOperationException with a specific message (cf. Nullable<T>.Value)
                        //     and we don't really want to reuse exception objects in general.

                        if (operandValue != null)
                        {
                            return Constant(node, operandValue, resultType);
                        }
                    }
                }
            }

            // REVIEW: Do we need any more cases for nullable-to-nullable conversion here, which could be
            //         optimized in case the operand is null. It seems these should not be reachable if we
            //         check for this *after* checking for a primitive conversion. Is it worth checking for
            //         these special cases first in order to avoid running an evaluator?

            // NB: We don't deal with reference conversions for the time being. This needs further analysis to
            //     make sure we don't take away conversions that are needed for the static typing of the tree.

            return node;
        }

        /// <summary>
        /// Optimizes an <see cref="ExpressionType.Convert"/> or <see cref="ExpressionType.ConvertChecked"/> expression
        /// that has a custom <see cref="UnaryExpression.Method"/> specified.
        /// </summary>
        /// <param name="node">The expression to optimize.</param>
        /// <returns>The result of optimizing the expression.</returns>
        private Expression OptimizeConvertWithMethod(UnaryExpression node)
        {
            if (node.IsLifted && !(node.Type.IsValueType && node.Operand.Type.IsValueType))
            {
                // See remarks in LambdaCompiler.EmitConvert for this specific case, which is a bit tricky.

                return EvaluateUnaryMethod(node);
            }
            else
            {
                return OptimizeUnaryWithMethod(node);
            }
        }

        /// <summary>
        /// Optimizes an <see cref="ExpressionType.TypeAs"/> expression.
        /// </summary>
        /// <param name="node">The expression to optimize.</param>
        /// <returns>The result of optimizing the expression.</returns>
        private Expression OptimizeTypeAs(UnaryExpression node)
        {
            var operand = node.Operand;

            var operandType = operand.Type;
            var resultType = node.Type;

            if (TypeUtils.AreEquivalent(operandType, resultType))
            {
                return operand;
            }

            if (IsAlwaysNull(operand))
            {
                return Null(node, resultType);
            }

            if (AnalyzeTypeIs(operand, resultType) == AnalyzeTypeCheckResult.KnownFalse)
            {
                return Null(node, resultType);
            }

            // REVIEW: Omitting a check for constant folding here; the node either returns null or
            //         the original object, so this node can't introduce a new mutable value.

            if (/* CanConstantFold(node) && */ HasConstantValue(operand))
            {
                var evaluator = GetEvaluator(node);

                return EvaluateUnary(node, evaluator);
            }

            return node;
        }

        /// <summary>
        /// Optimizes a unary expression that has no <see cref="UnaryExpression.Method"/> specified. The unary expression
        /// is assumed to be neither of <see cref="ExpressionType.ArrayLength"/>, <see cref="ExpressionType.Quote"/>,
        /// <see cref="ExpressionType.Convert"/>, <see cref="ExpressionType.ConvertChecked"/>, <see cref="ExpressionType.TypeAs"/>,
        /// <see cref="ExpressionType.Throw"/>, or <see cref="ExpressionType.Unbox"/>.
        /// </summary>
        /// <param name="node">The expression to optimize.</param>
        /// <returns>The result of optimizing the expression.</returns>
        private Expression OptimizeUnaryWithoutMethod(UnaryExpression node)
        {
            Debug.Assert(node.Method == null);

            Debug.Assert(
                  node.NodeType != ExpressionType.ArrayLength
                & node.NodeType != ExpressionType.Quote
                & node.NodeType != ExpressionType.Convert
                & node.NodeType != ExpressionType.ConvertChecked
                & node.NodeType != ExpressionType.TypeAs
                & node.NodeType != ExpressionType.Throw
                & node.NodeType != ExpressionType.Unbox);

            var operand = node.Operand;

            if (operand.Type.IsNullableType() && IsAlwaysNull(operand))
            {
                return Null(node, node.Type);
            }

            if (CanConstantFold(node) && HasConstantValue(operand))
            {
                return EvaluateUnary(node);
            }

            return OptimizeUnaryAlgebraic(node);
        }

        /// <summary>
        /// Optimizes unary expressions that satisfy some algebraic law.
        /// </summary>
        /// <param name="node">The expression to optimize.</param>
        /// <returns>The result of optimizing the expression.</returns>
        private Expression OptimizeUnaryAlgebraic(UnaryExpression node)
        {
            if (node.Method == null)
            {
                var operand = node.Operand;

                switch (node.NodeType)
                {
                    case ExpressionType.IsFalse:
                        // NB: This doesn't optimize the expression per-se, but enables optimizations such
                        //     as double-negation to pick up on it.
                        Debug.Assert(operand.Type.GetNonNullableType() == typeof(bool));
                        Debug.Assert(operand.Type == node.Type);
                        return OptimizeUnaryAlgebraic(Expression.Not(operand));
                    case ExpressionType.IsTrue:
                        Debug.Assert(operand.Type.GetNonNullableType() == typeof(bool));
                        return operand;
                    // NB: No optimizations for NegateChecked to avoid eliminating overflow exceptions.
                    case ExpressionType.Negate:
                        // NB: This is safe to do for float and double where `neg` just flips the sign bit.
                        return OptimizeUnaryAlgebraicNop(node);
                    case ExpressionType.Not:
                        if (IsUnaryBooleanNot(node))
                        {
                            return OptimizeUnaryBooleanNot(node);
                        }
                        else
                        {
                            // NB: This doesn't optimize the expression per-se, but enables optimizations such
                            //     as double-negation to pick up on it. It also makes the expression more natural
                            //     to read by using the explicit OnesComplement node type instead of Not for the
                            //     integer cases.
                            return OptimizeUnaryAlgebraic(Expression.OnesComplement(operand));
                        }
                    case ExpressionType.OnesComplement:
                        return OptimizeUnaryAlgebraicNop(node);
                    case ExpressionType.UnaryPlus:
                        // NB: Unlike C, where unary plus can cause widening, ETs implement this operator as
                        //     a nop instruction. It only has a use with custom methods.
                        return operand;
                }
            }

            return node;
        }

        /// <summary>
        /// Optimizes a unary <see cref="ExpressionType.Not"/> expression with a <see cref="bool"/> or nullable
        /// <see cref="bool"/> type.
        /// </summary>
        /// <param name="node">The expression to optimize.</param>
        /// <returns>The result of optimizing the expression.</returns>
        private Expression OptimizeUnaryBooleanNot(UnaryExpression node)
        {
            Debug.Assert(IsUnaryBooleanNot(node));

            var opt = OptimizeUnaryDeMorgan(node);

            if (opt != node)
            {
                return opt;
            }

            opt = OptimizeUnaryNotOfComparison(node);

            if (opt != node)
            {
                return opt;
            }

            return OptimizeUnaryAlgebraicNop(node);
        }

        /// <summary>
        /// Optimizes a unary expression which represents a reducible form of De Morgan's law, i.e.
        /// <c>!(!x &amp; !y) -> x | y</c> -or-
        /// <c>!(!x | !y) -> x &amp; y</c>
        /// </summary>
        /// <param name="node">The expression to optimize.</param>
        /// <returns>The result of optimizing the expression.</returns>
        private static Expression OptimizeUnaryDeMorgan(UnaryExpression node)
        {
            Debug.Assert(IsUnaryBooleanNot(node));

            var operand = node.Operand;

            if (operand.Type == node.Type)
            {
                if (operand is BinaryExpression binary && binary.Method == null && IsUnaryBooleanNot(binary.Left) && IsUnaryBooleanNot(binary.Right))
                {
                    var leftOperand = ((UnaryExpression)binary.Left).Operand;
                    var rightOperand = ((UnaryExpression)binary.Right).Operand;

                    switch (binary.NodeType)
                    {
                        case ExpressionType.And:
                            return Expression.Or(leftOperand, rightOperand);
                        case ExpressionType.AndAlso:
                            return Expression.OrElse(leftOperand, rightOperand);
                        case ExpressionType.Or:
                            return Expression.And(leftOperand, rightOperand);
                        case ExpressionType.OrElse:
                            return Expression.AndAlso(leftOperand, rightOperand);
                    }
                }
            }

            return node;
        }

        /// <summary>
        /// Optimizes a unary <see cref="ExpressionType.Not"/> expression applied to a binary expression operand
        /// representing a comparison.
        /// </summary>
        /// <param name="node">The expression to optimize.</param>
        /// <returns>The result of optimizing the expression.</returns>
        private Expression OptimizeUnaryNotOfComparison(UnaryExpression node)
        {
            Debug.Assert(IsUnaryBooleanNot(node));

            var operand = node.Operand;

            if (operand.Type == node.Type)
            {
                if (operand is BinaryExpression binary && binary.Method == null && binary.Left.Type == binary.Right.Type)
                {
                    switch (binary.NodeType)
                    {
                        case ExpressionType.GreaterThan:
                        case ExpressionType.GreaterThanOrEqual:
                        case ExpressionType.LessThan:
                        case ExpressionType.LessThanOrEqual:
                            return OptimizeUnaryNotOfComparison(node, binary);
                        case ExpressionType.Equal:
                        case ExpressionType.NotEqual:
                            return OptimizeUnaryNotOfEquality(node, binary);
                    }
                }
            }

            return node;
        }

        /// <summary>
        /// Optimizes a unary <see cref="ExpressionType.Not"/> expression applied to a binary expression operand
        /// representing a comparison.
        /// </summary>
        /// <param name="node">The expression to optimize.</param>
        /// <param name="operand">The binary operand passed to the unary operator.</param>
        /// <returns>The result of optimizing the expression.</returns>
        private Expression OptimizeUnaryNotOfComparison(UnaryExpression node, BinaryExpression operand)
        {
            Debug.Assert(node.NodeType == ExpressionType.Not);
            Debug.Assert(operand.Method == null);
            Debug.Assert(operand.Left.Type == operand.Right.Type);

            //
            // NB: None of these optimizations hold for floating point numbers due to NaN being not
            //     comparable:
            //
            //        0.0 <  NaN  ==  false
            //        0.0 >= NaN  ==  false
            //
            //     thus
            //
            //      !(0.0 <  NaN)  ==  true     --X-->     0.0 >= NaN  ==  false
            //
            if (TypeUtils.IsInteger(operand.Left.Type))
            {
                var isNotLifted = !operand.IsLifted;
                var isLiftedToNull = operand.IsLiftedToNull;

                //
                // NB: If the operator is not lifted to null but all operands are guaranteed to be non-null,
                //     the algebraic law holds as well. In case either operands is null, a non-lifted comparison
                //     returns false, for which we can't undo the Boolean negation.
                //
                var isNotLiftedToNullWithNonNullOperands = !isLiftedToNull && IsNeverNull(operand.Left) && IsNeverNull(operand.Right);

                if (isNotLifted || isLiftedToNull || isNotLiftedToNullWithNonNullOperands)
                {
                    //
                    // NB: Turning Not([Greater|Less]Than) into [Less|Greater]ThanOrEqual increases generated
                    //     IL code size and is something the C# compiler doesn't do either. On the flip side,
                    //     it would lead to smaller expression trees here, which is useful for serialization
                    //     cases. For now, we'll err on the safe side and only apply the optimization if we
                    //     know we're not regressing generated IL code size. We could consider knobs to turn
                    //     by derived classes to "optimize for speed" versus "optimize for size".
                    //

                    switch (operand.NodeType)
                    {
                        case ExpressionType.LessThanOrEqual:
                            return Expression.GreaterThan(operand.Left, operand.Right, isLiftedToNull, method: null);
                        case ExpressionType.GreaterThanOrEqual:
                            return Expression.LessThan(operand.Left, operand.Right, isLiftedToNull, method: null);
                    }
                }
            }

            return node;
        }

        /// <summary>
        /// Optimizes a unary <see cref="ExpressionType.Not"/> expression applied to a binary expression operand
        /// representing an equality.
        /// </summary>
        /// <param name="node">The expression to optimize.</param>
        /// <param name="operand">The binary operand passed to the unary operator.</param>
        /// <returns>The result of optimizing the expression.</returns>
        private static Expression OptimizeUnaryNotOfEquality(UnaryExpression node, BinaryExpression operand)
        {
            Debug.Assert(node.NodeType == ExpressionType.Not);
            Debug.Assert(operand.Method == null);
            Debug.Assert(operand.Left.Type == operand.Right.Type);

            //
            // NB: There are other constraints for these cases. The algebraic laws hold regardless of the type
            //     (including floating point numbers) and nullable lifting, as illustrated below.
            //
            //     If lifted to null:
            //
            //             == | null  | non-null               != | null  | non-null
            //       ---------+-------+-------------     ---------+-------+-------------
            //           null | true  | null                 null | false | null
            //       non-null | null  | [true|false]     non-null | null  | [true|false]
            //
            //     and considering null-lifted Boolean negation, then the following hold:
            //
            //      !(x == y) --> (x != y)
            //      !(x != y) --> (x == y)
            //
            //     If not lifted to null:
            //
            //             == | null  | non-null               != | null  | non-null
            //       ---------+-------+-------------     ---------+-------+-------------
            //           null | true  | false                null | false | true
            //       non-null | false | [true|false]     non-null | true  | [true|false]
            //
            //     the same laws hold using reglar Boolean negation.
            //

            if (operand.NodeType == ExpressionType.Equal)
            {
                //
                // NB: IL generation for NotEqual and Not(Equal) is the same for non-nullable operands (with
                //     the --- indicating assumed operands above and emitted instructions below):
                //
                //       NotEqual     Equal      Not
                //       ========     =====      ===
                //       x            x          o
                //       y            y          --------
                //       --------     -------    ldc.i4.0
                //       ceq          ceq        ceq
                //       ldc.i4.0
                //       ceq
                //
                //     As such, we're not growing the code size here (and reduce the tree by one node). Similar
                //     observations apply for nullable variants.
                //
                return Expression.NotEqual(operand.Left, operand.Right, operand.IsLiftedToNull, method: null);
            }
            else
            {
                Debug.Assert(operand.NodeType == ExpressionType.NotEqual);
                return Expression.Equal(operand.Left, operand.Right, operand.IsLiftedToNull, method: null);
            }
        }

        /// <summary>
        /// Checks if the specified expression is a unary <see cref="ExpressionType.Not"/> expression with a <see cref="bool"/>
        /// or nullable <see cref="bool"/> operand.
        /// </summary>
        /// <param name="node">The node to check.</param>
        /// <returns><c>true</c> if the specified expression is a (nullable) <see cref="bool"/> <see cref="ExpressionType.Not"/> expression; otherwise, <c>false</c>.</returns>
        private static bool IsUnaryBooleanNot(Expression node)
        {
            if (node.NodeType == ExpressionType.Not)
            {
                var unary = (UnaryExpression)node;
                if (unary.Method == null && unary.Type == unary.Operand.Type)
                {
                    return unary.Type.GetNonNullableType() == typeof(bool);
                }
            }

            return false;
        }

        /// <summary>
        /// Optimizes unary expressions that satisfy an <c>op(op(x)) == x</c> algebraic law.
        /// </summary>
        /// <param name="node">The expression to optimize.</param>
        /// <returns>The result of optimizing the expression.</returns>
        private static Expression OptimizeUnaryAlgebraicNop(UnaryExpression node)
        {
            var operand = node.Operand;

            if (operand.NodeType == node.NodeType && operand.Type == node.Type)
            {
                var unaryOperand = (UnaryExpression)operand;
                if (unaryOperand.Method == null && unaryOperand.Operand.Type == unaryOperand.Type)
                {
                    return unaryOperand.Operand;
                }
            }

            return node;
        }

        /// <summary>
        /// Optimizes a unary expression that has a custom <see cref="UnaryExpression.Method"/> specified. The unary expression
        /// is assumed to be neither of <see cref="ExpressionType.ArrayLength"/>, <see cref="ExpressionType.Quote"/>,
        /// <see cref="ExpressionType.TypeAs"/>, <see cref="ExpressionType.Throw"/>, or <see cref="ExpressionType.Unbox"/>.
        /// </summary>
        /// <param name="node">The expression to optimize.</param>
        /// <returns>The result of optimizing the expression.</returns>
        private Expression OptimizeUnaryWithMethod(UnaryExpression node)
        {
            Debug.Assert(
                  node.NodeType != ExpressionType.ArrayLength
                & node.NodeType != ExpressionType.Quote
                & node.NodeType != ExpressionType.TypeAs
                & node.NodeType != ExpressionType.Throw
                & node.NodeType != ExpressionType.Unbox);

            if (node.IsLiftedToNull && IsAlwaysNull(node.Operand))
            {
                return Null(node, node.Type);
            }

            return EvaluateUnaryMethod(node);
        }

        /// <summary>
        /// Evaluates a unary expression that has a custom <see cref="UnaryExpression.Method"/> specified.
        /// </summary>
        /// <param name="node">The unary expression to evaluate.</param>
        /// <returns>An expression representing the result of evaluating the unary expression at compile time.</returns>
        private Expression EvaluateUnaryMethod(UnaryExpression node)
        {
            if (CanConstantFold(node) && HasConstantValue(node.Operand) && IsPure(node.Method))
            {
                var parameter = node.Method.GetParameters()[0];
                var parameterType = parameter.ParameterType;

                // NB: This conversion is needed to deal with T? -> T conversion which shall not cause a null reference
                //     to be fed to the evaluator delegate. This could change nullable unwrapping (through .Value in the
                //     compiled expression tree code) to be hidden by a conversion of object to a value type. The former
                //     would throw InvalidOperationException, while the latter would call NullReferenceException.
                //
                //     Example:
                //
                //        Expression `Convert(Constant(null, typeof(DateTime?)), typeof(DateTimeOffset))`
                //        uses method `DateTimeOffset op_Implicit(DateTime)` for the conversion using lifting.
                //
                //        An evaluator would look like `(object o) => op_Implicit((DateTime)o)`. In case `null` is passed
                //        to the parameter `o`, the conversion fails with `NullReferenceException`. However, the original
                //        convert node would get implemented as `op_Implicit(((DateTime?)o).Value)` where the `Value`
                //        property causes `InvalidOperationException` when applied to a `null` operand.

                var convertedOperand = Visit(Utils.ConvertTo(node.Operand, parameterType));

                if (CanConstantFold(node) && HasConstantValue(convertedOperand))
                {
                    var operandValue = GetConstantValue(convertedOperand);

                    var evaluator = GetEvaluator(node.Method);
                    var convertedResult = Evaluate(node, evaluator, new[] { operandValue });

                    return convertedResult;
                }

                // CONSIDER: If we support reducing an expression to `InvalidOperationException`, we could add a check
                //           for `IsThrow` here to propagate the exception. For now, we simply return the original node
                //           without applying any optimizations.
            }

            return node;
        }

        /// <summary>
        /// Evaluates a unary expression that's deemed primitive.
        /// </summary>
        /// <param name="node">The unary expression to evaluate.</param>
        /// <returns>An expression representing the result of evaluating the unary expression at compile time.</returns>
        /// <remarks>
        /// This method should only be called if the unary expression is part of a fixed set of built-in unary
        /// operations because it will populate a static cache that's meant to have an upper size. For example,
        /// arbitrary conversions involving enum types should *not* obtain their evaluator through this code
        /// path but go through <see cref="GetEvaluator(UnaryExpression)"/> instead, so a derived class can
        /// make caching decisions.
        /// </remarks>
        private Expression EvaluateUnary(UnaryExpression node)
        {
            var evaluator = GetUnaryPrimitiveEvaluator(node);

            return EvaluateUnary(node, evaluator);
        }

        /// <summary>
        /// Evaluates a unary expression using the specified <paramref name="evaluator"/> by getting the constant
        /// value of the operand.
        /// </summary>
        /// <param name="node">The unary expression to evaluate.</param>
        /// <param name="evaluator">The evaluator to use.</param>
        /// <returns>An expression representing the result of evaluating the unary expression at compile time.</returns>
        private Expression EvaluateUnary(UnaryExpression node, Func<object, object> evaluator)
        {
            return EvaluateUnary(node, evaluator, GetConstantValue(node.Operand));
        }

        /// <summary>
        /// Evaluates a unary expression using the specified <paramref name="evaluator"/> and the specified
        /// constant operand value.
        /// </summary>
        /// <param name="node">The unary expression to evaluate.</param>
        /// <param name="evaluator">The evaluator to use.</param>
        /// <param name="operandValue">The constant operand value to pass to the evaluator.</param>
        /// <returns>An expression representing the result of evaluating the unary expression at compile time.</returns>
        private Expression EvaluateUnary(UnaryExpression node, Func<object, object> evaluator, object operandValue)
        {
            Debug.Assert(evaluator != null); // REVIEW: Should we allow a derived class to return null?

            object resultValue;

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1031 // Catch more specific exception type. (By design.)

            try
            {
                resultValue = evaluator(operandValue);
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
        /// Gets an evaluator delegate for a unary expression that's deemed primitive.
        /// </summary>
        /// <param name="node">The unary expression to get an evaluator for.</param>
        /// <returns>An evaluator delegate for the unary expression.</returns>
        /// <remarks>
        /// This method should only be called if the unary expression is part of a fixed set of built-in unary
        /// operations because it will populate a static cache that's meant to have an upper size. For example,
        /// arbitrary conversions involving enum types should *not* obtain their evaluator through this code
        /// path but go through <see cref="GetEvaluator(UnaryExpression)"/> instead, so a derived class can
        /// make caching decisions.
        /// </remarks>
        private Func<object, object> GetUnaryPrimitiveEvaluator(UnaryExpression node)
        {
            Debug.Assert(node.Method == null);

            return s_unaryEvaluators.GetOrAdd(new Unary { ExpressionType = node.NodeType, OperandType = node.Operand.Type, ResultType = node.Type }, u =>
            {
                var unary = Expression.MakeUnary(u.ExpressionType, Expression.Default(u.OperandType), u.ResultType);
                return EvaluatorFactory.GetEvaluator(unary);
            });
        }

        /// <summary>
        /// Struct for the key of the unary evaluator cache.
        /// </summary>
        private struct Unary : IEquatable<Unary>
        {
            /// <summary>
            /// The expression type of the unary node.
            /// </summary>
            public ExpressionType ExpressionType;

            /// <summary>
            /// The operand type of the unary node.
            /// </summary>
            public Type OperandType;

            /// <summary>
            /// The result type of the unary node.
            /// </summary>
            public Type ResultType;

            /// <summary>
            /// Checks whether the specified <paramref name="other"/> value is equal to the current value.
            /// </summary>
            /// <param name="other">The value to compare the current value to.</param>
            /// <returns><c>true</c> if the current value equals the <paramref name="other"/> value; otherwise, <c>false</c>.</returns>
            public bool Equals(Unary other) => ExpressionType == other.ExpressionType && OperandType == other.OperandType && ResultType == other.ResultType;

            /// <summary>
            /// Checks whether the specified object is equal to the current value.
            /// </summary>
            /// <param name="obj">The object to compare the current value to.</param>
            /// <returns><c>true</c> if the current value equals the specified object; otherwise, <c>false</c>.</returns>
            public override bool Equals(object obj) => obj is Unary unary && Equals(unary);

            /// <summary>
            /// Gets a hash code for the current value.
            /// </summary>
            /// <returns>A hash code for the current value.</returns>
            public override int GetHashCode() => HashHelpers.Combine((int)ExpressionType, OperandType.GetHashCode(), ResultType.GetHashCode());
        }
    }
}
