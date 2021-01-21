// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - May 2017 - Initial prototype of JIT.
//

using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace System.Linq.Expressions.Jit
{
    /// <summary>
    /// A closure information object providing facilities to emit variable storage operations for hoisted locals.
    /// </summary>
    internal sealed class ClosureInfo
    {
        /// <summary>
        /// The closure type providing hoisted variable storage.
        /// </summary>
        public readonly Type ClosureType;

        /// <summary>
        /// The closure type providing hoisted variable storage.
        /// </summary>
        public readonly Dictionary<ParameterExpression, ClosureFieldInfo> FieldMap;

        /// <summary>
        /// Creates a new closure information object.
        /// </summary>
        /// <param name="closureType">The closure type providing hoisted variable storage.</param>
        /// <param name="fieldMap">The closure type providing hoisted variable storage.</param>
        public ClosureInfo(Type closureType, Dictionary<ParameterExpression, ClosureFieldInfo> fieldMap)
        {
            ClosureType = closureType;
            FieldMap = fieldMap;
        }

        /// <summary>
        /// Gets an expression tree to access the specified hoisted variable from the specified closure.
        /// The resulting expression can be used to load from or store to the variable.
        /// </summary>
        /// <param name="closure">The closure containing storage for the hoisted variable. This expression should have no side-effects.</param>
        /// <param name="variable">The hoisted variable to access.</param>
        /// <returns>An expression tree to access the hoisted variable.</returns>
        public Expression Access(Expression closure, ParameterExpression variable)
        {
            //
            // Get the variable from the map. It's the caller's responsibility to ensure that
            // accesses are only performed for hoisted variables that exist in the closure.
            //
            var field = FieldMap[variable];

            //
            // Simply perform a field load.
            //
            var res = Expression.Field(closure, field.Field);

            //
            // In case the storage is boxed, we need to traverse into the StrongBox<T>.Value
            // field as well. Note this field is writeable.
            //
            if ((field.Kind & StorageKind.Boxed) != 0)
            {
                res = Expression.Field(res, "Value");
            }

            //
            // Return the expression which can be used to perform loads and stores.
            //
            return res;
        }

        /// <summary>
        /// Gets an expression tree to assign the specified value to the specified hoisted variable.
        /// The resulting expression should only be used to initialize variable storage when initializing the closure.
        /// </summary>
        /// <param name="closure">The closure containing storage for the hoisted variable. This expression should have no side-effects.</param>
        /// <param name="variable">The hoisted variable to assign to.</param>
        /// <param name="value">The value to assign to the hoisted variable.</param>
        /// <returns>An expression tree to asign to the hoisted variable.</returns>
        public Expression Assign(Expression closure, ParameterExpression variable, Expression value)
        {
            //
            // Get the variable from the map. It's the caller's responsibility to ensure that
            // assignments are only performed for hoisted variables that exist in the closure.
            //
            var field = FieldMap[variable];

            //
            // Prepare a left and right hand side for the assignment.
            //
            var lhs = Expression.Field(closure, field.Field);
            var rhs = value;

            //
            // In case the storage is boxed, we can't assign the Value field during the closure
            // initialization phase. Instead, we have to invoke the StrongBox<T> constructor
            // and assign to the closure field.
            //
            if ((field.Kind & StorageKind.Boxed) != 0)
            {
                rhs = Expression.New(typeof(StrongBox<>).MakeGenericType(variable.Type).GetConstructor(new[] { variable.Type }), value);
            }

            //
            // Return the expression to perform assignment to the variable storage during
            // closure initialization.
            //
            return Expression.Assign(lhs, rhs);
        }
    }
}
