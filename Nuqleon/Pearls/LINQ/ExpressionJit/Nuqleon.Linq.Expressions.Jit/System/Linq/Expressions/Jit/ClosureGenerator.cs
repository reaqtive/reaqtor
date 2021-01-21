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
using System.Runtime.CompilerServices;

namespace System.Linq.Expressions.Jit
{
    /// <summary>
    /// Utility to create closures for hoisted local storage and to emit storage operation expressions.
    /// </summary>
    internal static class ClosureGenerator
    {
        /// <summary>
        /// Prepares a closure for the the specified variables.
        /// </summary>
        /// <param name="variables">The variables with their required storage kind in the resulting closure.</param>
        /// <returns>An object containing information about the closure, usable to emit variable storage operations.</returns>
        public static ClosureInfo Create(IList<KeyValuePair<ParameterExpression, StorageKind>> variables)
        {
            var count = variables.Count;

            //
            // First, create an array of types for the locals that need to be stored in
            // the closure. Boxed locals will be wrapped in a StrongBox<T>.
            //
            var types = new Type[count];

            for (var i = 0; i < count; i++)
            {
                var variable = variables[i];
                var type = variable.Key.Type;

                if ((variable.Value & StorageKind.Boxed) != 0)
                {
                    types[i] = typeof(StrongBox<>).MakeGenericType(type);
                }
                else
                {
                    Debug.Assert(variable.Value != StorageKind.Local, "Only expected hoisted locals.");
                    types[i] = type;
                }
            }

            //
            // Next, create a map to associate the original hoisted locals to their
            // closure field storage.
            //
            var closureType = ClosureFactory.GetClosureType(types);
            var fieldMap = new Dictionary<ParameterExpression, ClosureFieldInfo>(count);

            for (var i = 0; i < count; i++)
            {
                var variable = variables[i];

                var field = closureType.GetField("Item" + (i + 1));

                var info = new ClosureFieldInfo
                {
                    Field = field,
                    Kind = variable.Value,
                };

                fieldMap.Add(variable.Key, info);
            }

            //
            // Return an object holding the gathered information, providing means to
            // emit variable storage operations for the hoisted locals provided.
            //
            return new ClosureInfo(closureType, fieldMap);
        }
    }
}
