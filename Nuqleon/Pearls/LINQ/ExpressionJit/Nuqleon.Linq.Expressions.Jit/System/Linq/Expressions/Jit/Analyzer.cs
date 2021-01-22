// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - May 2017 - Initial prototype of JIT.
//

using System.Collections.Generic;
using System.Diagnostics;

namespace System.Linq.Expressions.Jit
{
    /// <summary>
    /// Analyzer to gather scope information about nodes in expression trees.
    /// Scope information includes a mapping for all variables onto their required storage kind.
    /// </summary>
    internal static class Analyzer
    {
        /// <summary>
        /// Analyzes the specified lambda expression and its children to produce scope information.
        /// </summary>
        /// <param name="expression">The lambda expression to perform scope analysis on.</param>
        /// <param name="methodTable">The variable representing the method table passed to the top-level lambda.</param>
        /// <returns>A dictionary mapping each node introducing a scope to the gathered scope information.</returns>
        /// <remarks>Input is assumed to be reduced. An exception will be thrown if an unreduced node is encountered.</remarks>
        public static Dictionary<object, Scope> Analyze(LambdaExpression expression, ParameterExpression methodTable)
        {
            Debug.Assert(expression != null);

            //
            // Our visitor is stateful, so create a new instance each time.
            //
            var impl = new Impl(methodTable);

            //
            // The visitor accesses the tree in a read-only manner, so we can ignore the result.
            //
            var res = impl.Visit(expression);
            Debug.Assert(ReferenceEquals(res, expression), "Expected expression tree analysis to access the tree in a read-only manner.");

            //
            // Return the analysis that was built up by the visitor.
            //
            return impl.Analysis;
        }

        /// <summary>
        /// Internal implementation of the scope analyzer.
        /// </summary>
        /// <remarks>
        /// Instances of this type are stateful and should not be reused.
        /// </remarks>
        private sealed class Impl : BetterExpressionVisitor
        {
            /// <summary>
            /// Stores the result of scope analysis for each node that introduces a scope. These nodes include:
            /// * LambdaExpression through the Parameters collection
            /// * BlockExpression through the Variables collection
            /// * CatchBlock through the Variable property
            /// </summary>
            /// <remarks>
            /// The expression nodes that introduce a scope don't share a common base type, hence the use of System.Object for the key.
            /// </remarks>
            internal readonly Dictionary<object, Scope> Analysis = new();

            /// <summary>
            /// Stack containing the nested scopes. The top of the stack contains the closest scope to the node currently being visted.
            /// </summary>
            /// <remarks>
            /// Artificial nodes are kept for Quote nodes in order to detect variable uses in Quote nodes.
            /// </remarks>
            private readonly Stack<Scope> _environment = new();

            /// <summary>
            /// The variable representing the top-level method table.
            /// </summary>
            private readonly ParameterExpression _methodTable;

            /// <summary>
            /// Creates a new visitor for analysis of scoping.
            /// </summary>
            /// <param name="methodTable">The variable representing the top-level method table.</param>
            public Impl(ParameterExpression methodTable)
            {
                _methodTable = methodTable;

                //
                // If a method table variable is supplied, create a fake top-level scope that's not backed by any node
                // but simply contains the method table variable. When we're looking up the variable from VisitLambda
                // in order to get access to the method table (to retrieve a thunk in the final rewritten tree), we'll
                // find it in this top-level scope, thus recording NeedsClosure information on the way.
                //
                if (_methodTable != null)
                {
                    var topScope = new Scope(node: null, parent: null, variables: new[] { _methodTable });
                    _environment.Push(topScope);
                }
            }

            /// <summary>
            /// Gets the nearest enclosing scope, or null.
            /// </summary>
            /// <returns>The scope object representing the nearest enclosing scope, or null if we're in the global scope.</returns>
            private Scope NearestEnclosingScope
            {
                get
                {
                    foreach (var scope in _environment)
                    {
                        //
                        // Skip artificial scopes introduced to keep track of Quote nodes.
                        //
                        if (scope.Node is not UnaryExpression)
                        {
                            return scope;
                        }
                    }

                    return null;
                }
            }

            /// <summary>
            /// Analyzes scoping of a lambda expression.
            /// </summary>
            /// <typeparam name="T">The type of the delegate represented by the lambda expression.</typeparam>
            /// <param name="node">The lambda expression to perform analysis on.</param>
            /// <returns>The original expression passed in <paramref name="node"/>.</returns>
            protected override Expression VisitLambda<T>(Expression<T> node)
            {
                //
                // NB: Expression trees nodes can be reused due to their immutable nature. The analysis of scoping of
                //     variables declared within a node is a property that solely depends on the node's descendants,
                //     thus allowing us to reuse the result of a prior analysis.
                //
                if (!Analysis.TryGetValue(node, out _))
                {
                    //
                    // First, visit the method table which will be required in the final rewritting expression with
                    // JIT compilation support. This will ensure that the analysis contains valid NeedsClosure info
                    // used to determine whether a rewritten scope needs to set up a closure.
                    //
                    if (_methodTable != null)
                    {
                        Visit(_methodTable);
                    }

                    var scope = new Scope(node, NearestEnclosingScope, node.Parameters);

                    _environment.Push(scope);

                    Visit(node.Body);

                    _environment.Pop();

                    Debug.Assert(!Analysis.ContainsKey(node), "Unexpected recursive tree definition.");
                    Analysis[node] = scope;
                }

                return node;
            }

            /// <summary>
            /// Analyzes scoping of a block expression.
            /// </summary>
            /// <param name="node">The block expression to perform analysis on.</param>
            /// <returns>The original expression passed in <paramref name="node"/>.</returns>
            protected override Expression VisitBlock(BlockExpression node)
            {
                //
                // NB: Expression trees nodes can be reused due to their immutable nature. The analysis of scoping of
                //     variables declared within a node is a property that solely depends on the node's descendants,
                //     thus allowing us to reuse the result of a prior analysis.
                //
                if (!Analysis.TryGetValue(node, out _))
                {
                    var scope = new Scope(node, NearestEnclosingScope, node.Variables);

                    _environment.Push(scope);

                    Visit(node.Expressions);

                    _environment.Pop();

                    Debug.Assert(!Analysis.ContainsKey(node), "Unexpected recursive tree definition.");
                    Analysis[node] = scope;
                }

                return node;
            }

            /// <summary>
            /// Analyzes scoping of a catch block.
            /// </summary>
            /// <param name="node">The catch block to perform analysis on.</param>
            /// <returns>The original node passed in <paramref name="node"/>.</returns>
            protected override CatchBlock VisitCatchBlock(CatchBlock node)
            {
                //
                // NB: Expression trees nodes can be reused due to their immutable nature. The analysis of scoping of
                //     variables declared within a node is a property that solely depends on the node's descendants,
                //     thus allowing us to reuse the result of a prior analysis.
                //
                if (!Analysis.TryGetValue(node, out _))
                {
                    var scope = new Scope(node, NearestEnclosingScope, node.Variable == null ? Array.Empty<ParameterExpression>() : new[] { node.Variable });

                    _environment.Push(scope);

                    //
                    // CONSIDER: The analysis of Filter and Body could be separated in order to avoid the creation
                    //           of closures in the different subexpressions of the CatchBlock.
                    //
                    Visit(node.Filter);
                    Visit(node.Body);

                    _environment.Pop();

                    Debug.Assert(!Analysis.ContainsKey(node), "Unexpected recursive tree definition.");
                    Analysis[node] = scope;
                }

                return node;
            }

            /// <summary>
            /// Ensures that variables referenced in runtime variables expressions are hoisted.
            /// </summary>
            /// <param name="node">The runtime variables expression to perform analysis on.</param>
            /// <returns>The original node passed in <paramref name="node"/>.</returns>
            protected override Expression VisitRuntimeVariables(RuntimeVariablesExpression node)
            {
                //
                // NB: Variables accessed through IRuntimeVariables need strong boxes around them in order to have
                //     reference capturing semantics.
                //
                foreach (var var in node.Variables)
                {
                    VisitParameter(var, StorageKind.Hoisted | StorageKind.Boxed);
                }

                return node;
            }

            /// <summary>
            /// Ensures that variables referenced from quote expressions are hoisted.
            /// </summary>
            /// <param name="node">The unary expression to perform analysis on.</param>
            /// <returns>The original node passed in <paramref name="node"/>.</returns>
            protected override Expression VisitUnary(UnaryExpression node)
            {
                //
                // NB: Variables accessed in a Quote need strong boxes around them in order for the runtime rewrite
                //     of the Quote to emit an accessor to their storage location, through StrongBox<T>.Value.
                //
                if (node.NodeType == ExpressionType.Quote)
                {
                    var scope = new Scope(node, NearestEnclosingScope, Array.Empty<ParameterExpression>());

                    _environment.Push(scope);

                    Visit(node.Operand);

                    _environment.Pop();

                    return node;
                }

                return base.VisitUnary(node);
            }

            /// <summary>
            /// Binds the use site of the specified variable to the closest enclosing declaration site and records
            /// storage requirements for the variable at the declaration site based on the nesting of scopes
            /// between the use site and the declaration site.
            /// </summary>
            /// <param name="node">The variable to bind.</param>
            /// <returns>The original node passed in <paramref name="node"/>.</returns>
            /// <exception cref="InvalidOperationException">
            /// Thrown when the variable cannot be bound to an enclosing scope's declaration site.
            /// </exception>
            protected override Expression VisitParameter(ParameterExpression node)
            {
                return VisitParameter(node, StorageKind.Local);
            }

            /// <summary>
            /// Binds the use site of the specified variable to the closest enclosing declaration site and records
            /// the specified storage requirements for the variable at the declaration site based on the nesting
            /// of scopes between the use site and the declaration site.
            /// </summary>
            /// <param name="node">The variable to bind.</param>
            /// <param name="storage">The minimal storage requirements for the variable. By default, a value of <see cref="StorageKind.Local"/> is used.</param>
            /// <returns>The original node passed in <paramref name="node"/>.</returns>
            /// <exception cref="InvalidOperationException">
            /// Thrown when the variable cannot be bound to an enclosing scope's declaration site.
            /// </exception>
            private Expression VisitParameter(ParameterExpression node, StorageKind storage)
            {
                //
                // Iterate over the enclosing scopes from the nearest scope to the top-level scope.
                //
                foreach (var scope in _environment)
                {
                    //
                    // When found, bind the variable using the storage requirements found.
                    //
                    if (scope.Locals.ContainsKey(node))
                    {
                        scope.Bind(node, storage);
                        return node;
                    }

                    //
                    // When we traverse a Lambda boundary, the variable needs to be hoisted into a
                    // closure in order for it to be referenced by the nested lambda.
                    //
                    if (scope.Node is LambdaExpression)
                    {
                        storage |= StorageKind.Hoisted;
                    }

                    //
                    // Variables referenced from Quote nodes need to get boxed explicitly in order
                    // to have correct reference capturing semantics.
                    //
                    if (scope.Node is UnaryExpression)
                    {
                        Debug.Assert(((Expression)scope.Node).NodeType == ExpressionType.Quote);
                        storage |= StorageKind.Boxed;
                    }

                    //
                    // In order to access the variable's declaration site, all intermediate scopes
                    // that get traversed need to set up a closure so we can traverse the chain.
                    //
                    scope.RequireClosure();
                }

                //
                // NB: Throws an exception of the same type as VariableBinder in System.Linq.Expressions
                //     in order to ensure compatibility.
                //
                throw new InvalidOperationException($"Parameter '{node}' is unbound.");
            }

            /// <summary>
            /// Ensures that no extension nodes occur in the expression tree.
            /// </summary>
            /// <param name="node">The extension node to visit.</param>
            /// <returns>Always throws <see cref="InvalidOperationException"/>.</returns>
            /// <exception cref="InvalidOperationException">
            /// Thrown to indicate that extension nodes should have been fully reduced prior to invoking the analyzer.
            /// </exception>
            protected override Expression VisitExtension(Expression node)
            {
                throw new InvalidOperationException($"Unexpected extension node '{node}'.");
            }

#if TODO
            private void MergeScopes(LambdaExpression node)
            {
                var block = node.Body as BlockExpression;
                if (block != null)
                {
                    MergeScopes(block);
                }
                else
                {
                    Visit(node.Body);
                }
            }

            private ReadOnlyCollection<Expression> MergeScopes(BlockExpression node)
            {
                var scope = NearestEnclosingScope;
                Debug.Assert(scope != null);

                var expressions = node.Expressions;
                var first = default(Expression);

                while (expressions.Count == 1 && (first = expressions[0]).NodeType == ExpressionType.Block)
                {
                    var block = (BlockExpression)first;

                    if (block.Variables.Count > 0)
                    {
                        foreach (var v in block.Variables)
                        {
                            if (scope.Locals.ContainsKey(v))
                            {
                                return expressions;
                            }
                        }
                    }

                    expressions = block.Expressions;
                }

                return expressions;
            }
#endif
        }
    }
}
