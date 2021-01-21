// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - May 2017 - Initial prototype of JIT.
//

using System.Collections.Generic;
using static System.Linq.Expressions.Expression;

namespace System.Linq.Expressions
{
    /// <summary>
    /// Optimizer for expression trees.
    /// </summary>
    internal static class Optimizer
    {
        /// <summary>
        /// Shared instance of the optimizer's implementation with all optimizations enabled.
        /// </summary>
        private static readonly Impl s_all = new(Optimizations.All);

        /// <summary>
        /// Optimizes the specified expression and its children using the specified optimizations.
        /// </summary>
        /// <param name="expression">The expression to optimize, or null.</param>
        /// <param name="optimizations">The optimizations to apply.</param>
        /// <returns>The result of optimizing the specified expression, or null if the original expression was null.</returns>
        public static Expression Optimize(Expression expression, Optimizations optimizations)
        {
            Impl impl;

            //
            // Find a shared instance, if available. Note that the optimizer is stateless and
            // thread-safe, allowing instances to be used in a shared manner.
            //
            if (optimizations == Optimizations.All)
            {
                impl = s_all;
            }
            else
            {
                impl = new Impl(optimizations);
            }

            //
            // Apply the visitor to perform optimizations.
            //
            return impl.Visit(expression);
        }

        /// <summary>
        /// Implementation of the optimizer as an expression visitor.
        /// </summary>
        /// <remarks>
        /// Modulo the optimization flags, instances of this type are stateless and may be reused.
        /// </remarks>
        private sealed class Impl : BetterExpressionVisitor
        {
            /// <summary>
            /// Flags indicating which optimizations are applied.
            /// </summary>
            private readonly Optimizations _optimizations;

            //
            // WARNING: If per-instance state is added here, undo instance sharing optimizations.
            //

            /// <summary>
            /// Creates a new optimizer instance with the given optimizations flags set.
            /// </summary>
            /// <param name="optimizations">The optimizations to apply.</param>
            public Impl(Optimizations optimizations)
            {
                _optimizations = optimizations;
            }

            /// <summary>
            /// Visits block expressions in order to apply the block flattening optimization, if enabled.
            /// </summary>
            /// <param name="node">The expression to optimize.</param>
            /// <returns>The result of optimizing the expression and its children.</returns>
            protected override Expression VisitBlock(BlockExpression node)
            {
                if ((_optimizations & Optimizations.BlockFlattening) != 0)
                {
                    //
                    // Nesting of blocks containing a single block expression can be flattened into
                    // a single block expression. This reduces the number of scopes emitted and can
                    // reduce closure traversals at runtime when variables get hoisted.
                    //
                    // CONSIDER: More aggressive merging of blocks is possible in some cases and
                    //           can be considered independent of our JIT work.
                    //
                    var expressions = node.Expressions;

                    //
                    // We'll merge the variables declared by the nested blocks. Note that shadowing
                    // is fine here, because there can't be any use sites of declared variables in
                    // between two shadowing declaration sites:
                    //
                    //   Block(
                    //     [x],      <---+
                    //     Block(        +-- Shadows itself
                    //       [x],    <---+
                    //       ...
                    //     )
                    //  )
                    //
                    var variables = new HashSet<ParameterExpression>(node.Variables);

                    Expression first;

                    while (expressions.Count == 1 && (first = expressions[0]).NodeType == ExpressionType.Block)
                    {
                        var inner = (BlockExpression)first;

                        var innerVariables = inner.Variables;

                        //
                        // Merge variables but check length first to avoid enumerator allocation.
                        //
                        if (innerVariables.Count > 0)
                        {
                            foreach (var variable in innerVariables)
                            {
                                variables.Add(variable);
                            }
                        }

                        //
                        // Move to the next block.
                        //
                        expressions = inner.Expressions;
                    }

                    //
                    // If the expressions collection we got back has changed, we performed block
                    // flattening. Update the expression using the expressions collection we got
                    // back and use the flattened list of variables. Also keep the original node
                    // type in order to retain the original static typing of the tree.
                    //
                    if (expressions != node.Expressions)
                    {
                        return Block(node.Type, variables, Visit(expressions));
                    }
                }

                //
                // No optimization carried out; do the regular visit.
                //
                return base.VisitBlock(node);
            }

            /// <summary>
            /// Visits invocation expressions in order to apply the invocation inlining optimization, if enabled.
            /// </summary>
            /// <param name="node">The expression to optimize.</param>
            /// <returns>The result of optimizing the expression and its children.</returns>
            protected override Expression VisitInvocation(InvocationExpression node)
            {
                if ((_optimizations & Optimizations.InvocationInlining) != 0)
                {
                    //
                    // Try to inline an invocation applied to a lambda expression.
                    //
                    if (node.Expression is LambdaExpression lambda)
                    {
                        var parameters = lambda.Parameters;

                        //
                        // If any parameter is declared as ByRef, we can't apply the optimization
                        // because we can't use the parameter as a variable in a block expression
                        // which does not support ByRef locals.
                        //
                        var hasByRef = false;

                        foreach (var parameter in parameters)
                        {
                            if (parameter.IsByRef)
                            {
                                hasByRef = true;
                                break;
                            }
                        }

                        if (!hasByRef)
                        {
                            //
                            // We found no contraindications preventing us from inlining the lambda
                            // expression's body. We won't change our mind anymore, so we can visit
                            // the invocation arguments straight away. To keep tree traversal order
                            // consistent across visitors, we'll first visit the lambda body.
                            //
                            var body = Visit(lambda.Body);
                            var arguments = Visit(node.Arguments);

                            //
                            // However, we still need to make sure we can safely evaluate arguments
                            // in a block where we use the lambda parameters as locals. E.g.
                            //
                            //   Invoke((a, b, c) => a + b * c, 1 + a, 2 * b, x - 3)
                            //                                      ^      ^
                            //
                            // If we were to rewrite this as follows:
                            //
                            //   Block(
                            //     [a, b, c],
                            //     a = 1 + a,
                            //     b = 2 * b,
                            //     c = x - 3,
                            //     a + b * c
                            //   )
                            //
                            // we'd end up binding the use site of `a` and `b` in the arguments to
                            // the lambda's own parameters, rather than referring to the enclosing
                            // scope. If this situation arises, we need to introduce another block
                            // to use unique temporary variables.
                            //
                            // We keep a list of parameters (`locals`) to assign the arguments to,
                            // in left-to-right evaluation order. An entry can contain:
                            //
                            // * the original corresponding lambda parameter, if safe to do so
                            // * a temporary variable
                            //
                            // In case any temporary variable is introduced, a companion `dealias`
                            // map is created containing the temporary-to-parameter assignments to
                            // perform prior to evaluating the lambda body, e.g.:
                            //
                            //   Block(
                            //     [t1, t2, c],   // <-- `locals`
                            //     t1 = 1 + a,
                            //     t2 = 2 * b,
                            //     c = x - 3,     // safe to eval; x is bound in enclosing scope
                            //     Block(
                            //       [a, b],
                            //       a = t1,      // <-- entries in the `dealias` map
                            //       b = t2,      //
                            //       a + b * c
                            //     )
                            //   )
                            //
                            var locals = parameters.ToArray();
                            var dealias = default(Dictionary<ParameterExpression, ParameterExpression>);

                            if (parameters.Count > 0)
                            {
                                //
                                // Find any unbound variable in any visited invocation argument.
                                //
                                var findUnboundVariables = new UnboundVariableScanner();

                                findUnboundVariables.Visit(arguments);

                                var unboundVariables = findUnboundVariables.UnboundVariables;

                                //
                                // Now that we've found the unbound variables that occur in any of
                                // the invocation arguments, find those that intersect with lambda
                                // parameters.
                                //
                                var conflicts = new HashSet<ParameterExpression>(parameters);

                                conflicts.IntersectWith(unboundVariables);

                                //
                                // If we found any conflicts, it's time to build the dealias map,
                                // and patch up the locals list. Note we don't have to worry about
                                // order of entries in the dealias map because variable-to-variable
                                // assignment is free of side-effects.
                                //
                                // CONSIDER: To have deterministic code generation, we could use an
                                //           ordered data structure for `dealias`.
                                //
                                if (conflicts.Count > 0)
                                {
                                    dealias = new Dictionary<ParameterExpression, ParameterExpression>(conflicts.Count);

                                    for (var i = 0; i < locals.Length; i++)
                                    {
                                        var local = locals[i];
                                        if (conflicts.Contains(local))
                                        {
                                            var temporary = Parameter(local.Type, local.Name);
                                            locals[i] = temporary;
                                            dealias.Add(local, temporary);
                                        }
                                    }
                                }
                            }

                            //
                            // First, build the outer block by assigning the arguments to the
                            // locals we gathered. We'll need one assignment for each local, and
                            // one more expression for the result.
                            //
                            var outerBlock = new Expression[locals.Length + 1];
                            for (var i = 0; i < locals.Length; i++)
                            {
                                outerBlock[i] = Assign(locals[i], arguments[i]);
                            }

                            //
                            // Next, prepare the result expression which may need to be wrapped
                            // with an inner block.
                            //
                            var result = body;

                            if (dealias != null)
                            {
                                //
                                // We need another block to assign the temporaries to the lambda
                                // parameters which are referenced in the lambda body.
                                //
                                var innerBlock = new Expression[dealias.Count + 1];

                                var innerIndex = 0;
                                foreach (var entry in dealias)
                                {
                                    innerBlock[innerIndex++] = Assign(entry.Key, entry.Value);
                                }

                                //
                                // Store the original result (i.e. the body) in the last slot
                                // and replace the result expression with the inner block.
                                //
                                innerBlock[innerIndex] = result;

                                result =
                                    Block(
                                        node.Type,     // See remark below on explicit typing.
                                        dealias.Keys,
                                        innerBlock
                                    );
                            }

                            //
                            // Stuff the result in the last expression slot.
                            //
#if NET5_0
                            outerBlock[^1] = result;
#else
                            outerBlock[outerBlock.Length - 1] = result;
#endif

                            //
                            // Create the final block to return. Note we set the type explicitly
                            // in order to ensure we keep the static typing of the tree identical
                            // to the original. A difference would come up if the lambda body was
                            // using a covariant return type compared to its delegate type.
                            //
                            var optimized =
                                Block(
                                    node.Type,
                                    locals,
                                    outerBlock
                                );

                            return optimized;
                        }
                    }
                }

                //
                // No optimization carried out; do the regular visit.
                //
                return base.VisitInvocation(node);
            }
        }
    }
}
