// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - May 2017 - Initial prototype of JIT.
//

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1711 // Remove Ex suffix. (Infrastructure only.)

using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq.Expressions.Jit;

namespace System.Runtime.CompilerServices
{
    public partial class RuntimeOpsEx
    {
        /// <summary>
        /// Quotes expressions at runtime by performing binding steps on unbound variables.
        /// </summary>
        private sealed class ExpressionQuoter : BetterExpressionVisitor
        {
            /// <summary>
            /// Stack of nested declaration scopes.
            /// </summary>
            private readonly Stack<HashSet<ParameterExpression>> _environment = new();

            /// <summary>
            /// The hoisted locals information gathered by the compiler at compile time.
            /// Used to traverse the parent chain and to resolve indices of variable slots.
            /// </summary>
            private readonly HoistedLocals _locals;

            /// <summary>
            /// The closure to access for the binding of hoisted variables.
            /// </summary>
            private readonly IRuntimeVariables _closure;

            /// <summary>
            /// Creates a new expression quoter.
            /// </summary>
            /// <param name="locals">The hoisted locals information gathered by the compiler at compile time.</param>
            /// <param name="closure">The closure to access for the binding of hoisted variables.</param>
            public ExpressionQuoter(HoistedLocals locals, IRuntimeVariables closure)
            {
                _locals = locals;
                _closure = closure;
            }

            /// <summary>
            /// Visit lambda expressions to perform scope tracking.
            /// </summary>
            /// <typeparam name="T">The type of the delegate represented by the lambda expression.</typeparam>
            /// <param name="node">The lambda expression to visit.</param>
            /// <returns>The result of visiting the lambda expression.</returns>
            protected override Expression VisitLambda<T>(Expression<T> node)
            {
                if (node.Parameters.Count > 0)
                {
                    _environment.Push(new HashSet<ParameterExpression>(node.Parameters));
                }

                var body = Visit(node.Body);

                if (node.Parameters.Count > 0)
                {
                    _environment.Pop();
                }

                if (body == node.Body)
                {
                    return node;
                }
                else
                {
                    return Expression.Lambda<T>(body, node.Name, node.TailCall, node.Parameters);
                }
            }

            /// <summary>
            /// Visit block expressions to perform scope tracking.
            /// </summary>
            /// <param name="node">The block expression to visit.</param>
            /// <returns>The result of visiting the block expression.</returns>
            protected override Expression VisitBlock(BlockExpression node)
            {
                if (node.Variables.Count > 0)
                {
                    _environment.Push(new HashSet<ParameterExpression>(node.Variables));
                }

                var expressions = Visit(node.Expressions);

                if (node.Variables.Count > 0)
                {
                    _environment.Pop();
                }

                if (expressions == node.Expressions)
                {
                    return node;
                }
                else
                {
                    return Expression.Block(node.Type, node.Variables, expressions);
                }
            }

            /// <summary>
            /// Visit catch block nodes to perform scope tracking.
            /// </summary>
            /// <param name="node">The catch block node to visit.</param>
            /// <returns>The result of visiting the catch block node.</returns>
            protected override CatchBlock VisitCatchBlock(CatchBlock node)
            {
                if (node.Variable != null)
                {
                    _environment.Push(new HashSet<ParameterExpression>(new[] { node.Variable }));
                }

                var filter = Visit(node.Filter);
                var body = Visit(node.Body);

                if (node.Variable != null)
                {
                    _environment.Pop();
                }

                if (filter == node.Filter && body == node.Body)
                {
                    return node;
                }
                else
                {
                    return Expression.MakeCatchBlock(node.Test, node.Variable, body, filter);
                }
            }

            /// <summary>
            /// Visit parameter expression to perform binding of closed over variables if needed.
            /// </summary>
            /// <param name="node">The parameter expression to bind.</param>
            /// <returns>The result of binding the parameter expression.</returns>
            protected override Expression VisitParameter(ParameterExpression node)
            {
                //
                // Scan the environment for the parameter and return a box if the parameter is not
                // bound within the quoted expression.
                //
                var box = GetBox(node);

                //
                // The variable is bound within the quoted expression; return it.
                //
                if (box == null)
                {
                    return node;
                }

                //
                // Access the value of the box.
                //
                // NB: This approach is taken to ensure compatibility with LINQ ETs.
                //
                // NB: Note that we don't pass a type to the Constant factory; we want the runtime
                //     type of the box in order to resolve the Value field and retain strong typing.
                //
                return Expression.Field(Expression.Constant(box), "Value");
            }

            /// <summary>
            /// Visit runtime variables expressions to perform binding of closed over variables if needed.
            /// </summary>
            /// <param name="node">The runtime variables expression containing parameters to bind.</param>
            /// <returns>The result of binding the runtime variables expression.</returns>
            protected override Expression VisitRuntimeVariables(RuntimeVariablesExpression node)
            {
                var count = node.Variables.Count;

                var hoistedVariables = new List<IStrongBox>();
                var localVariables = new List<ParameterExpression>();

                var indexes = new int[count];
                for (int i = 0; i < count; i++)
                {
                    //
                    // Try to bind variables to locals defined in the quoted expression.
                    //
                    var box = GetBox(node.Variables[i]);
                    if (box == null)
                    {
                        //
                        // If no hoisted variable is found, keep track of the variable as a local and
                        // use a positive index to encode the local slot.
                        //
                        indexes[i] = localVariables.Count;
                        localVariables.Add(node.Variables[i]);
                    }
                    else
                    {
                        //
                        // If no local is found, store the box that provides access to the variable and
                        // use a negative index to encode the hoisted slot.
                        //
                        indexes[i] = -1 - hoistedVariables.Count;
                        hoistedVariables.Add(box);
                    }
                }

                //
                // No hoisted variables found, thus no merging at runtime needed. We won't use the positive
                // indices we computed earlier.
                //
                if (hoistedVariables.Count == 0)
                {
                    return node;
                }

                //
                // Convert the list of hoisted variables to an array (to prune excess storage slots) and
                // store the result in a constant expression of type IRuntimeVariables.
                //
                var hoistedRuntimeVariables = Expression.Constant(new RuntimeVariables(hoistedVariables.ToArray()), typeof(IRuntimeVariables));

                //
                // No local variables found; we can simply return the hoisted variables. We won't use the
                // negative indices we computed earlier.
                //
                if (localVariables.Count == 0)
                {
                    return hoistedRuntimeVariables;
                }

                //
                // We found both locals and hoisted variables, so we need to perform merging of the variables
                // at runtime. Emit a MergeRuntimeVariables call using our positive and negative indices to
                // indicate which IRuntimeVariables object to index into.
                //
                return Expression.Call(typeof(RuntimeOps).GetMethod(nameof(RuntimeOps.MergeRuntimeVariables)), Expression.RuntimeVariables(localVariables.ToArray()), hoistedRuntimeVariables, Expression.Constant(indexes));
            }

            /// <summary>
            /// Gets the strong box storage location of the specified variable used for runtime access.
            /// </summary>
            /// <param name="variable">The variable whose storage location to retrieve.</param>
            /// <returns>The strong box storage location of the specified variable.</returns>
            private IStrongBox GetBox(ParameterExpression variable)
            {
                //
                // First try to bind the variable to a local defined within the quoted expression.
                //
                foreach (var frame in _environment)
                {
                    if (frame.Contains(variable))
                    {
                        return null;
                    }
                }

                //
                // Traverse the parent chain to the closure containing the local, and obtain its slot.
                //
                var hoistedLocals = _locals;
                var closure = _closure;

                int slot;

                while (!hoistedLocals.Indexes.TryGetValue(variable, out slot))
                {
                    hoistedLocals = hoistedLocals.Parent;

                    Invariant.Assert(hoistedLocals != null);

                    closure = (IRuntimeVariables)closure[0];
                }

                //
                // Return the storage location of the variable.
                //
                return (IStrongBox)closure[slot];
            }
        }
    }
}
