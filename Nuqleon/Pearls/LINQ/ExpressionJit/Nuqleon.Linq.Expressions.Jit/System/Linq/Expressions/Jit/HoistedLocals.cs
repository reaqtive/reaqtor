// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - May 2017 - Initial prototype of JIT.
//

using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace System.Linq.Expressions.Jit
{
    /// <summary>
    /// Provides information about hoisted locals at runtime.
    /// </summary>
    internal sealed class HoistedLocals
    {
        /// <summary>
        /// Reference to the parent scope of hoisted locals, if any.
        /// </summary>
        public readonly HoistedLocals Parent;

        /// <summary>
        /// Variables representing the hoisted locals. If a parent exists, its self variable is in the first slot.
        /// </summary>
        public readonly ReadOnlyCollection<ParameterExpression> Variables;

        /// <summary>
        /// Maps of the hoisted locals to required storage kinds.
        /// </summary>
        public readonly Dictionary<ParameterExpression, StorageKind> Definitions;

        /// <summary>
        /// The variable representing the current scope of hoisted locals.
        /// </summary>
        public readonly ParameterExpression SelfVariable;

        /// <summary>
        /// Information about the closure containing the hoisted locals.
        /// </summary>
        public readonly ClosureInfo Closure;

        /// <summary>
        /// Map of the hoisted locals to storage slots in the closure.
        /// </summary>
        public readonly ReadOnlyDictionary<ParameterExpression, int> Indexes;

        /// <summary>
        /// Creates a new object representing the top-level closure.
        /// </summary>
        /// <param name="selfVariable">The variable representing the closure.</param>
        /// <remarks>
        /// This constructor is used to create the top-level lambda's closure using the method
        /// table as the first parameter.
        /// </remarks>
        public HoistedLocals(ParameterExpression selfVariable)
        {
            SelfVariable = selfVariable;

            // REVIEW: Do we need to instantiate other fields?
        }

        /// <summary>
        /// Creates a new object representing information about hoisted locals.
        /// </summary>
        /// <param name="parent">The parent scope.</param>
        /// <param name="variables">The hoisted locals in the current scope.</param>
        /// <param name="definitions">The storage kinds of the variables.</param>
        public HoistedLocals(HoistedLocals parent, ReadOnlyCollection<ParameterExpression> variables, Dictionary<ParameterExpression, StorageKind> definitions)
        {
            Parent = parent;
            Definitions = definitions;

            //
            // By convention, we'll store the parent's self-variable in the first slot of the closure.
            //
            // Note that the self-variable is statically typed, so the whole closure and all its storage
            // slots are statically typed too, no matter how far we have to ascend through the parent
            // chain. This differs from LINQ ETs at the point of writing (10/10/16).
            //
            if (parent != null)
            {
                variables = variables.AddFirst(parent.SelfVariable);
            }

            Variables = variables;

            //
            // Next, build the index map and gather all the storage kinds for the hoisted locals in
            // order to create a statically typed closure. The index map will be used for runtime
            // variable access; the closure will be used to emit code to access the locals in the
            // compiled expression tree.
            //
            var count = variables.Count;
            var indexes = new Dictionary<ParameterExpression, int>(count);
            var fields = new List<KeyValuePair<ParameterExpression, StorageKind>>(count);

            for (var i = 0; i < count; i++)
            {
                var variable = variables[i];
                var storageKind = variable == parent?.SelfVariable ? StorageKind.Hoisted : definitions[variable];

                indexes.Add(variable, i);
                fields.Add(new KeyValuePair<ParameterExpression, StorageKind>(variable, storageKind));
            }

            //
            // Build the closure and store the closure information. The static type of the closure
            // is used for the self-variable, resulting in static typing across the closure and its
            // parent chain.
            //
            Closure = ClosureGenerator.Create(fields);
            SelfVariable = Expression.Variable(Closure.ClosureType, name: null);
            Indexes = new ReadOnlyDictionary<ParameterExpression, int>(indexes);
        }
    }
}
