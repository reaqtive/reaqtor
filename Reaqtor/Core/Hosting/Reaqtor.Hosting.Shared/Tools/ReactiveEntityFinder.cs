// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq.CompilerServices.Bonsai;
using System.Linq.Expressions;

namespace Reaqtor.Hosting.Shared.Tools
{
    /// <summary>
    /// A utility to find and organize all the unbound parameters with Reactive entity types.
    /// </summary>
    public static class ReactiveEntityFinder
    {
        /// <summary>
        /// A utility method to find and organize all the unbound parameters with Reactive entity types.
        /// </summary>
        /// <param name="expression">The expression to search for Reactive entities.</param>
        /// <returns>The Reactive entities in the expression, organized by entity type.</returns>
        public static ReactiveEntities Find(ExpressionSlim expression)
        {
            var impl = new Impl();
            impl.Visit(expression);
            return impl.Entities;
        }

        /// <summary>
        /// A slim expression visitor to find Reactive entity types.
        /// </summary>
        private sealed class Impl : ScopedExpressionSlimVisitor<ParameterExpressionSlim>
        {
            /// <summary>
            /// Gets the list of entities found as a result of a call to the visit method.
            /// </summary>
            public ReactiveEntities Entities { get; } = new ReactiveEntities();

            /// <summary>
            /// Visits an invocation expression and adds an entry to the set of Reactive
            /// entities if it is an invocation of an unbound parameter with a Reactive
            /// entity type.
            /// </summary>
            /// <param name="node">The invocation expression to visit.</param>
            /// <returns>The result of visiting the invocation expression.</returns>
            protected override ExpressionSlim VisitInvocation(InvocationExpressionSlim node)
            {
                // If the expression is an invocation of an unbound parameter...
                if (node.Expression is ParameterExpressionSlim target && !TryLookup(target, out _))
                {
                    var reactiveEntityType = ReactiveEntityTypeExtensions.FromTypeSlim(target.Type);
                    reactiveEntityType &= ~ReactiveEntityType.Func;
                    AddToReactiveEntities(reactiveEntityType, target, node.Arguments);
                }

                return base.VisitInvocation(node);
            }

            /// <summary>
            /// Visits a parameter expression and adds an entry to the entities list if
            /// the parameter type is a Reactive entity type.
            /// </summary>
            /// <param name="node">The parameter expression.</param>
            /// <returns>The result of visiting the parameter expression.</returns>
            protected override ExpressionSlim VisitParameter(ParameterExpressionSlim node)
            {
                // If the expression is an unbound parameter...
                if (!TryLookup(node, out _))
                {
                    var reactiveEntityType = ReactiveEntityTypeExtensions.FromTypeSlim(node.Type);

                    // ... that is not a parameterized Reactive entity type...
                    if ((reactiveEntityType & ReactiveEntityType.Func) != ReactiveEntityType.Func)
                    {
                        AddToReactiveEntities(reactiveEntityType, node, Array.Empty<ExpressionSlim>());
                    }
                }

                return base.VisitParameter(node);
            }

            /// <summary>
            /// Gets a state object for a bound parameter expression. Useful for identifying
            /// unbound parameters in an expression.
            /// </summary>
            /// <param name="parameter">The parameter expression to get state for.</param>
            /// <returns>The state for the given parameter expression.</returns>
            protected override ParameterExpressionSlim GetState(ParameterExpressionSlim parameter) => parameter;

            /// <summary>
            /// Adds an occurrence of a Reactive entity to the results, iff it is an expected Reactive entity type.
            /// The expected Reactive entity types are those Reactive entity type enum values for which
            /// dictionaries have already been set up in the results.
            /// </summary>
            /// <param name="reactiveEntityType">The Reactive entity type of the given parameter expression.</param>
            /// <param name="parameter">The parameter expression being added.</param>
            /// <param name="arguments">The list of arguments this expression is invoked with.</param>
            private void AddToReactiveEntities(ReactiveEntityType reactiveEntityType, ParameterExpressionSlim parameter, IEnumerable<ExpressionSlim> arguments)
            {
                if (Entities.TryGetValue(reactiveEntityType, out var reactiveEntitySet))
                {
                    if (!reactiveEntitySet.TryGetValue(parameter.Name, out var occurrences))
                    {
                        occurrences = new HashSet<IEnumerable<ExpressionSlim>>();
                        reactiveEntitySet.Add(parameter.Name, occurrences);
                    }

                    occurrences.Add(arguments);
                }
            }
        }
    }
}
