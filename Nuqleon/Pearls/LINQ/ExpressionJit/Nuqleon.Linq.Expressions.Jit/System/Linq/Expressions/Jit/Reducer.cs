// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - May 2017 - Initial prototype of JIT.
//

using System.Diagnostics;
using static System.Linq.Expressions.Expression;

namespace System.Linq.Expressions.Jit
{
    using static TypeUtils;

    /// <summary>
    /// Utility to reduce expression trees to a representation that can be consumed by the subsequent
    /// expression tree JIT compilation stages. This includes reduction of extension nodes as well as
    /// reduction of certain constructs that cannot be rewritten safely to support JIT.
    /// </summary>
    internal static class Reducer
    {
        /// <summary>
        /// Singleton instance of the visitor implementation.
        /// </summary>
        private static readonly Impl s_instance = new();

        /// <summary>
        /// Reduces the specified expression to a form that can be rewritten by the JIT compiler.
        /// </summary>
        /// <typeparam name="T">The type of the expression to reduce.</typeparam>
        /// <param name="expression">The expression to reduce.</param>
        /// <returns>The reduced form of the expression.</returns>
        public static T Reduce<T>(T expression) where T : Expression
        {
            return s_instance.VisitAndConvert(expression, nameof(Reduce));
        }

        /// <summary>
        /// Implementation of the reducer as an expression visitor.
        /// </summary>
        /// <remarks>
        /// Instances of this class are stateless, so a singleton instance can be used.
        /// </remarks>
        private sealed class Impl : BetterExpressionVisitor
        {
            //
            // WARNING: If per-instance state is added here, undo instance sharing optimizations.
            //

            //
            // NB: We don't have to override VisitExtension. The base class definition has the desired
            //     behavior of reducing these nodes and throwing if they are not reducible.
            //

            //
            // NB: We don't have to worry about Quote nodes here. We can't prevent the need for further
            //     analysis of these nodes in subsequent stages of JIT compilation nor do we have a
            //     reduced form for them; hence we have to keep their lambda operands as-is and deal
            //     with them later during scope analysis and expression rewriting.
            //

            //
            // CONSIDER: Should we retain the inlined invocation optimization of the lambda compiler?
            //           Right now, we'll keep these lambda expressions as-is, causing them to get
            //           rewritten to CreateDelegate calls, thus preventing the inlining optimization
            //           from taking place in the LambdaCompiler. This gives us the benefit of having
            //           more lambdas that can be subject to JIT but also increases evaluation cost.
            //
            //           Note that some of the reductions below introduce Invoke/Lambda trees, so if
            //           we decide to apply inlining optimizations here, we should likely reduce these
            //           newly introduced nodes as well.
            //

            /// <summary>
            /// Reduce binary expressions with <see cref="BinaryExpression.Conversion"/> properties in
            /// order to eliminate occurrences of <see cref="LambdaExpression"/> nodes in positions
            /// where the JIT compiler can't rewrite them.
            /// </summary>
            /// <param name="node">The expression to reduce.</param>
            /// <returns>The reduced expression.</returns>
            protected override Expression VisitBinary(BinaryExpression node)
            {
                //
                // Occurrences of LambdaExpression nodes will get rewritten in subsequent compilation
                // steps to CreateDelegate calls on thunks that can be JITted. However, the rewrite of
                // a BinaryExpression.Conversion property requires to receive back a LambdaExpression
                // so we can't apply the rewrite to these nodes. Hence we reduce them early on.
                //
                if (node.Conversion != null)
                {
                    Debug.Assert(node.Conversion.Parameters.Count == 1, "Expected one conversion parameter.");

                    //
                    // This is the case for compound assignments with conversions. The Reduce method
                    // will turn them into Expression.Invoke(Conversion, ...) nodes which can be JIT
                    // compiled safely.
                    //
                    if (node.CanReduce)
                    {
                        return node.Reduce();
                    }

                    //
                    // The remaining case is for Coalesce nodes.
                    //
                    if (node.NodeType == ExpressionType.Coalesce)
                    {
                        //
                        // We can recurse first in order to rewrite the child nodes, allowing us to
                        // free the recursion burden from our specialized reduction methods.
                        //
                        var visited = (BinaryExpression)base.VisitBinary(node);

                        //
                        // Structure is analogous to the LambdaCompiler's Emit methods for Coalesce
                        // nodes here.
                        //
                        if (visited.Left.Type.IsNullableType())
                        {
                            return ReduceBinaryCoalesceNullable(visited);
                        }
                        else
                        {
                            return ReduceBinaryCoalesceReference(visited);
                        }
                    }

                    //
                    // There should be no other cases of BinaryExpression having a non-null Conversion.
                    //
                    throw Invariant.Unreachable;
                }

                return base.VisitBinary(node);
            }

            /// <summary>
            /// Reduces a binary expression of type Coalesce applied to a nullable left operand.
            /// </summary>
            /// <param name="node">The expression to reduce.</param>
            /// <returns>The reduced expression.</returns>
            private static Expression ReduceBinaryCoalesceNullable(BinaryExpression node)
            {
                Debug.Assert(node.Conversion != null, "Expected a conversion lambda.");

                var leftType = node.Left.Type;
                var leftTemp = Parameter(leftType);

                //
                // Using Nullable<T>.HasValue to perform the null check.
                //
                var leftNotNull = Property(leftTemp, nameof(Nullable<int>.HasValue));

                //
                // If not null, we need to invoke the Conversion lambda but we may need to convert the
                // left operand to its non-null value first.
                //
                var conversionArgument = default(Expression);

                var conversionParameterType = node.Conversion.Parameters[0].Type;

                Debug.Assert(
                    conversionParameterType.IsAssignableFrom(leftType) ||
                    conversionParameterType.IsAssignableFrom(leftType.GetNonNullableType()),
                    "Expected assignment compatibility of the left operand and the conversion lambda parameter."
                );

                if (!conversionParameterType.IsAssignableFrom(leftType))
                {
                    //
                    // NB: The use of GetValueOrDefault in lieu of Value avoids a null check and conditional
                    //     log to throw InvalidOperationException in case the nullable has no value.
                    //
                    var getValueOrDefault = leftType.GetMethod(nameof(Nullable<int>.GetValueOrDefault), Type.EmptyTypes);
                    conversionArgument = Call(leftTemp, getValueOrDefault);
                }
                else
                {
                    conversionArgument = leftTemp;
                }

                //
                // Use an InvocationExpression node to invoke the conversion. This pushes down the
                // conversion lambda to a place where the JIT compiler won't have to worry about rewriting
                // the expression to have CreateDelegate method calls.
                //
                var invokeConversion = Invoke(node.Conversion, conversionArgument);

                //
                // If null, we need to return the right operand. In case of a Coalesce node with a conversion
                // lambda specified, there should be assignment compatibility (see Expression.Coalesce).
                //
                var right = node.Right;
                Debug.Assert(
                    AreEquivalent(right.Type, invokeConversion.Type),
                    "Expected assignment compatibility of the right operand and the conversion result."
                );

                //
                // Construct and return the reduced form.
                //
                var res =
                    Block(
                        new[] { leftTemp },
                        Assign(leftTemp, node.Left),
                        Condition(
                            leftNotNull,
                            invokeConversion,
                            right
                        )
                    );

                return res;
            }

            /// <summary>
            /// Reduces a binary expression of type Coalesce applied to a left operand of a reference type.
            /// </summary>
            /// <param name="node">The expression to reduce.</param>
            /// <returns>The reduced expression.</returns>
            private static Expression ReduceBinaryCoalesceReference(BinaryExpression node)
            {
                Debug.Assert(node.Conversion != null, "Expected a conversion lambda.");

                var leftType = node.Left.Type;
                var leftTemp = Parameter(leftType);

                //
                // NB: Not using Default(leftType) because of ConstantCheck in the LambdaCompiler
                //     providing optimizations only for null literals right now.
                //
                //     See https://github.com/dotnet/corefx/pull/11136 for an improvement to this.
                //
                var leftNotNull = ReferenceNotEqual(leftTemp, Constant(value: null, leftType));

                //
                // If not null, we need to invoke the Conversion lambda. 
                //
                // Use an InvocationExpression node to invoke the conversion. This pushes down the
                // conversion lambda to a place where the JIT compiler won't have to worry about rewriting
                // the expression to have CreateDelegate method calls.
                //
                var invokeConversion = Invoke(node.Conversion, leftTemp);

                //
                // Construct and return the reduced form.
                //
                var res =
                    Block(
                        new[] { leftTemp },
                        Assign(leftTemp, node.Left),
                        Condition(
                            leftNotNull,
                            invokeConversion,
                            node.Right
                        )
                    );

                return res;
            }
        }
    }
}
