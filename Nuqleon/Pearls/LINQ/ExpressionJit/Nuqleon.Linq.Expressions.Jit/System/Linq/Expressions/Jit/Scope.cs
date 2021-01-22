// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - May 2017 - Initial prototype of JIT.
//

using System.Collections.Generic;

namespace System.Linq.Expressions.Jit
{
    /// <summary>
    /// Contains scoping information about a node. Instances of this type are produced by <see cref="Analyzer"/>. 
    /// </summary>
    internal sealed class Scope
    {
        /// <summary>
        /// Gets the node to which this scoping information applies.
        /// </summary>
        public readonly object Node;

        /// <summary>
        /// Gets the parent scope, or null if this is the top-level scope.
        /// </summary>
        public readonly Scope Parent;

        /// <summary>
        /// Gets a dictionary mapping all variables introduced by this scope onto their required storage kind.
        /// </summary>
        public readonly Dictionary<ParameterExpression, StorageKind> Locals;

        /// <summary>
        /// Creates a new instance holding scope information.
        /// </summary>
        /// <param name="node">The node to which the scoping information applies.</param>
        /// <param name="parent">The parent scope, or null if this is the top-level scope.</param>
        /// <param name="variables">A dictionary mapping all variables introduced by this scope onto their required storage kind.</param>
        public Scope(object node, Scope parent, IList<ParameterExpression> variables)
        {
            Node = node;
            Parent = parent;
            Locals = new Dictionary<ParameterExpression, StorageKind>(variables.Count);

            //
            // All variables start off as requiring local storage. Calls to the Hoist method made
            // by the Analyzer can add additional requirements to storage locations.
            //
            foreach (var var in variables)
            {
                Locals[var] = StorageKind.Local;
            }
        }

        /// <summary>
        /// Gets a Boolean indicating whether this scope introduces any variables that require hoisting into a closure.
        /// </summary>
        public bool HasHoistedLocals { get; private set; }

        /// <summary>
        /// Gets a Boolean indicating whether this scope needs a closure in order for children to traverse to storage locations of variables declared in parent scopes.
        /// </summary>
        public bool NeedsClosure { get; private set; }

        /// <summary>
        /// Changes the storage requirements of the specified variable.
        /// </summary>
        /// <param name="variable">The variable to change the storage requirements for.</param>
        /// <param name="kind">The storage kind required for this variable.</param>
        /// <remarks>
        /// Storage requirements are OR-ed together; different use sites of variables can have different storage requirements.
        /// </remarks>
        public void Bind(ParameterExpression variable, StorageKind kind)
        {
            //
            // NB: We can't hoist or box variables of ByRef types. This case can't get detected
            //     at expression creation time, so we have to guard against it here.
            //
            if (kind >= StorageKind.Boxed && variable.IsByRef)
            {
                throw new InvalidOperationException("ByRef variables cannot be referenced from nested lambdas.");
            }

            Locals[variable] |= kind;

            if ((kind & StorageKind.Hoisted) != 0)
            {
                HasHoistedLocals = true;
            }
        }

        /// <summary>
        /// Toggles the <see cref="NeedsClosure"/> flag to record the closure requirement.
        /// </summary>
        public void RequireClosure()
        {
            NeedsClosure = true;
        }
    }
}
