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

using System.Collections.Generic;

namespace System.Linq.Expressions
{
    public partial class ExpressionOptimizer
    {
        /// <summary>
        /// Visits a block expression to perform optimization steps.
        /// </summary>
        /// <param name="node">The block expression to visit.</param>
        /// <returns>The result of optimizing the block expression.</returns>
        protected override Expression VisitBlock(BlockExpression node)
        {
            var res = (BlockExpression)base.VisitBlock(node);

            AssertTypes(node, res);

            var opt = OptimizeBlock(res);

            if (opt.NodeType != ExpressionType.Block)
            {
                return opt;
            }

            var optimizedBlock = (BlockExpression)opt;

            AssertTypes(node, optimizedBlock);

            //
            // Analyze the expressions in the block from top to bottom or left to right.
            // This will mimic sequential evaluation of the expressions and provide
            // opportunities to reduce the block to a single expression. Note that the
            // use of AnalyzeLeftToRight only considers pure and throw child expressions
            // so this is safe even without having control flow analysis because any such
            // expressions (label and goto in particular) are not considered pure and will
            // cause the analysis to bail out early.
            //
            // CONSIDER: If we do basic control flow analysis, we could also eliminate
            //           dead code blocks and reduce the expression more aggressively. For
            //           now, we'll ignore these cases.
            //

            var analysis = AnalyzeLeftToRight(first: null, optimizedBlock.Expressions);

            if (analysis.AllPure)
            {
                return UpdateBlock(optimizedBlock, optimizedBlock.Result);
            }
            else if (analysis.Throw != null)
            {
                return UpdateBlock(optimizedBlock, analysis.Throw);
            }

            return optimizedBlock;
        }

        /// <summary>
        /// Performs a series of optimizations to a block expression.
        /// </summary>
        /// <param name="node">The block expression to optimize.</param>
        /// <returns>The result of optimizing the specified block expression.</returns>
        private Expression OptimizeBlock(BlockExpression node)
        {
            //
            // First, remove pure statements that can be omitted. The most common case
            // of this is empty statements which may have been introduced by optimizing
            // conditional statements. By removing these first, we may end up having
            // more unused variables that can be removed.
            //
            var opt1 = RemovePureStatements(node);

            if (opt1.NodeType != ExpressionType.Block)
            {
                return opt1;
            }

            var opt1Block = (BlockExpression)opt1;

            //
            // Second, remove any unused variables that are declared by the block but
            // don't occur *at all* in the expressions of the block. This includes
            // both lval and rval use sites.
            //
            // Example: { int x, int y; x + 1; } -> { int x; x + 1; }
            //
            var opt2 = RemoveUnusedVariables(opt1Block);

            AssertTypes(node, opt2);

            //
            // Third, remove declared variables that are not assigned to by any child
            // expression by replacing them with default expressions.
            //
            // Example: { int x, int y; x = 1; x + y; } -> { int x; x = 1; x + default(int); }
            //
            // Note that the resulting expressions will be further optimized because
            // the use of a default value can lead to algebraic optimizations. In the
            // example above this will result in one more reduction:
            //
            // Example: { int x; x = 1; x + default(int); } -> { int x; x = 1; x; }
            //
            // CONSIDER: Note that, currently, we don't perform any control flow analysis
            //           so we can't optimize this further to simply return 1.
            //
            var opt3 = RemoveUnassignedVariables(opt2);

            AssertTypes(node, opt3);

            //
            // Fourth, flatten nested single blocks to reduce the number of intermediate
            // scopes that may exist in the expression.
            //
            // Example: { int x; { int y; x + y; } } -> { int x, int y; x + y; }
            //
            var opt4 = FlattenBlocks(opt3);

            AssertTypes(node, opt4);

            return opt4;
        }

        /// <summary>
        /// Removes pure statements from the specified <paramref name="block"/> expression.
        /// </summary>
        /// <param name="block">The block to remove pure statements from.</param>
        /// <returns>The result of removing pure statements.</returns>
        /// <example>
        ///   <code>{ ; x; F(); 42; G(); }</code>
        ///   becomes
        ///   <code>{ F(); G(); }</code>
        /// </example>
        private Expression RemovePureStatements(BlockExpression block)
        {
            var oldExpressions = block.Expressions;
            var newExpressions = default(List<Expression>);

            var count = oldExpressions.Count;

            for (int i = 0, n = count - 1; i < n; i++)
            {
                var expr = block.Expressions[i];

                if (IsPure(expr))
                {
                    if (newExpressions == null)
                    {
                        newExpressions = new List<Expression>(n);

                        for (var j = 0; j < i; j++)
                        {
                            newExpressions.Add(oldExpressions[j]);
                        }
                    }
                }
                else if (newExpressions != null)
                {
                    newExpressions.Add(expr);
                }
            }

            if (newExpressions == null)
            {
                return block;
            }
            else if (newExpressions.Count == 0)
            {
                return ChangeType(oldExpressions[count - 1], block.Type);
            }
            else
            {
                newExpressions.Add(oldExpressions[count - 1]);

                return block.Update(block.Variables, newExpressions);
            }
        }

        /// <summary>
        /// Removes unused declared variables from the specified block.
        /// </summary>
        /// <param name="block">The block to remove unused declared variables from.</param>
        /// <returns>The result of removing unused variables.</returns>
        /// <example>
        ///   <code>{ int x, int y; x + 1; }</code>
        ///   becomes
        ///   <code>{ int x; x + 1; }</code>
        /// </example>
        private static BlockExpression RemoveUnusedVariables(BlockExpression block)
        {
            var variables = block.Variables;

            var n = variables.Count;

            if (n > 0)
            {
                // REVIEW: This does a lot of repeated scanning of the same child nodes.
                //         We could consider storing these results for later reuse.

                var finder = new FreeVariableFinder();

                finder.Visit(block.Expressions);

                var freeVariables = finder.FreeVariables;

                var remainingVariables = default(List<ParameterExpression>);

                for (var i = 0; i < n; i++)
                {
                    var variable = variables[i];

                    if (!freeVariables.Contains(variable))
                    {
                        if (remainingVariables == null)
                        {
                            remainingVariables = new List<ParameterExpression>(n);

                            for (var j = 0; j < i; j++)
                            {
                                remainingVariables.Add(variables[j]);
                            }
                        }
                    }
                    else
                    {
                        remainingVariables?.Add(variable);
                    }
                }

                if (remainingVariables != null)
                {
                    return block.Update(remainingVariables, block.Expressions);
                }
            }

            return block;
        }

        /// <summary>
        /// Removes unassigned declared variables and replaces use sites by default expressions.
        /// </summary>
        /// <param name="block">The block to remove unassigned variables from.</param>
        /// <returns>The result of removing unassigned variables.</returns>
        /// <example>
        ///   <code>{ int x, int y; x = 1; x + y; }</code>
        ///   becomes
        ///   <code>{ int x; x = 1; x + default(int); }</code>
        /// </example>
        private BlockExpression RemoveUnassignedVariables(BlockExpression block)
        {
            var oldVariables = block.Variables;

            // REVIEW: This does a lot of repeated scanning of the same child nodes.
            //         We could consider storing these results for later reuse.

            if (oldVariables.Count > 0)
            {
                var analyzer = new AssignmentAnalyzer(oldVariables);

                analyzer.Visit(block.Expressions);

                var unassigned = analyzer.Unassigned;

                if (unassigned.Count > 0)
                {
                    var subst = new VariableDefaultValueSubstitutor(unassigned);

                    var newExpressions = subst.Visit(block.Expressions);

                    //
                    // NB: The introduction of default values can yield more optimization
                    //     opportunities, so we visit the child expressions again.
                    //
                    var optExpressions = Visit(newExpressions);

                    if (oldVariables.Count == unassigned.Count)
                    {
                        return block.Update(variables: null, optExpressions);
                    }
                    else
                    {
                        var newVariables = new ParameterExpression[oldVariables.Count - unassigned.Count];

                        for (int i = 0, j = 0, n = oldVariables.Count; i < n; i++)
                        {
                            var variable = oldVariables[i];

                            if (!unassigned.Contains(variable))
                            {
                                newVariables[j++] = variable;
                            }
                        }

                        return block.Update(newVariables, optExpressions);
                    }
                }
            }

            return block;
        }

        /// <summary>
        /// Flattens nested blocks to reduce the nesting level.
        /// </summary>
        /// <param name="block">The block to flatten.</param>
        /// <returns>The result of flattening nested blocks.</returns>
        /// <example>
        ///   <code>{ int x; { int y; x + y; } }</code>
        ///   becomes
        ///   <code>{ int x, int y; x + y; }</code>
        /// </example>
        private Expression FlattenBlocks(BlockExpression block)
        {
            var allVariables = default(HashSet<ParameterExpression>);
            var variableList = default(List<ParameterExpression>);

            var current = block;

            //
            // NB: This optimization is only safe for blocks with single expressions in
            //     them because we're flattening declaration scopes. In a block has more
            //     than one expression, this could result in changing bindings:
            //
            //     { int x; { int y; x + y; } y; } -> { int x, int y; x + y; y }
            //
            //     In this example, the binding of expression `y` in the outer block has
            //     changed due to scope merging. If only a single child block expression
            //     exists, this can't occur.
            //
            // CONSIDER: There are more cases where flattening blocks is safe to do, but
            //           these may require more analysis steps. Nested blocks without any
            //           variables or local branches could be merged into the parent.
            //

            while (current.Expressions.Count == 1 && current.Expressions[0].NodeType == ExpressionType.Block)
            {
                var variables = current.Variables;

                if (variables.Count > 0)
                {
                    if (allVariables == null)
                    {
                        allVariables = new HashSet<ParameterExpression>();
                        variableList = new List<ParameterExpression>();
                    }

                    foreach (var variable in variables)
                    {
                        if (allVariables.Add(variable))
                        {
                            variableList.Add(variable);
                        }
                    }
                }

                current = (BlockExpression)current.Expressions[0];
            }

            if (current != block)
            {
                IEnumerable<ParameterExpression> variables;

                if (variableList == null)
                {
                    variables = current.Variables;
                }
                else
                {
                    if (current.Variables.Count > 0)
                    {
                        foreach (var variable in current.Variables)
                        {
                            if (allVariables.Add(variable))
                            {
                                variableList.Add(variable);
                            }
                        }
                    }

                    variables = variableList;
                }

                var expressions = current.Expressions;

                block = block.Update(variables, expressions);
            }

            if (block.Variables.Count == 0 && block.Expressions.Count == 1)
            {
                return ChangeType(block.Expressions[0], block.Type);
            }

            return block;
        }

        /// <summary>
        /// Updates the block expression to have a single child expression.
        /// </summary>
        /// <param name="node">The block expression to rewrite.</param>
        /// <param name="expression">The single child expression to keep in the resulting block.</param>
        /// <returns>The result of rewriting the original block expression.</returns>
        private Expression UpdateBlock(BlockExpression node, Expression expression)
        {
            var body = ChangeType(expression, node.Type);

            if (node.Variables.Count != 0)
            {
                return body;
            }
            else
            {
                //
                // NB: Declared variables may be needed by the expression that's returned
                //     so we have to keep them. However, some variables may only be used by
                //     expressions following the node which we're keeping, so they may
                //     have become unused. Therefore, we optimize the generated block again.
                //
                var bodyWithVariables = Expression.Block(node.Type, node.Variables, body);
                return OptimizeBlock(bodyWithVariables);
            }
        }
    }
}
