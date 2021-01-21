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

namespace System.Runtime.CompilerServices
{
    /// <summary>
    /// Base class for thunk factories.
    /// </summary>
    internal abstract class ThunkFactoryBase : IThunkFactory
    {
        /// <summary>
        /// Cache of runtime generated thunk types.
        /// </summary>
        /// <remarks>
        /// Access to this dictionary should be synchronized using a lock on the collection itself.
        /// </remarks>
        private readonly Dictionary<Type, Type> _dynamicThunks = new();

        /// <summary>
        /// Gets the thunk type for the specified delegate type. If the delegate type is
        /// generic, it should be a closed generic type. The specified closure type will
        /// be used for the closure type parameter of the thunk type.
        /// </summary>
        /// <param name="delegateType">The delegate type to get a thunk type for.</param>
        /// <param name="closureType">The closure type to parameterize the thunk type on.</param>
        /// <returns>The thunk type, closed over the specified closure type.</returns>
        public Type GetThunkType(Type delegateType, Type closureType)
        {
            Type thunkType;

            if (delegateType.IsGenericType)
            {
                Debug.Assert(delegateType.IsConstructedGenericType, "Expected constructed generic type.");

                var def = delegateType.GetGenericTypeDefinition();

                //
                // Try to find a built-in thunk type and fall back to generating a custom
                // thunk type at runtime if no built-in one is found.
                //
                // NB: Construction of runtime thunk types requires the use of the open
                //     generic delegate type. We'll close over the generic arguments further
                //     down.
                //
                if (!TryGetPreCompiledThunk(def, out thunkType))
                {
                    thunkType = GetRuntimeThunkType(def);
                }

                //
                // For generic types, we should parameterize the thunk type on the closure
                // type, followed by the original generic arguments.
                //
                var args = delegateType.GetGenericArguments();

                var newArgs = new Type[args.Length + 1];
                newArgs[0] = closureType;
                Array.Copy(args, 0, newArgs, 1, args.Length);

                return thunkType.MakeGenericType(newArgs);
            }
            else
            {
                //
                // Try to find a built-in thunk type and fall back to generating a custom
                // thunk type at runtime if no built-in one is found.
                //
                if (!TryGetPreCompiledThunk(delegateType, out thunkType))
                {
                    thunkType = GetRuntimeThunkType(delegateType);
                }

                //
                // For non-generic types, the thunk type will have a single generic type
                // parameter which should be set to the closure type.
                //
                return thunkType.MakeGenericType(closureType);
            }
        }

        /// <summary>
        /// Tries to find a pre-compiled thunk type for the specified delegate type.
        /// </summary>
        /// <param name="delegateType">The delegate type to find a thunk type for.</param>
        /// <param name="thunkType">A thunk type for the specified delegate type.</param>
        /// <returns>true if a thunk type was found; otherwise, false.</returns>
        protected abstract bool TryGetPreCompiledThunk(Type delegateType, out Type thunkType);

        /// <summary>
        /// Gets a runtime generated thunk type for the specified delegate type.
        /// </summary>
        /// <param name="delegateType">The delegate type to generate a thunk type for.</param>
        /// <returns>A thunk type for the specified delegate type.</returns>
        private Type GetRuntimeThunkType(Type delegateType)
        {
            var thunkType = default(Type);

            //
            // Use a simple locking strategy to access the cache of thunk types. Lookup
            // should be fast enough and its cost marginal compared to the rest of an
            // expression tree compilation process.
            //
            lock (_dynamicThunks)
            {
                if (!_dynamicThunks.TryGetValue(delegateType, out thunkType))
                {
                    //
                    // NB: Running this code under the lock is by design. We want to avoid
                    //     generating the same thunk type twice. Hitting this code path
                    //     should also be very rare: the delegate type needs to be a non-
                    //     standard one (cf. Thunks.TypeMap) and it should not yet have a
                    //     custom thunk type generated for it.
                    //
                    thunkType = CreateThunkType(delegateType);
                    _dynamicThunks[delegateType] = thunkType;
                }
            }

            return thunkType;
        }

        /// <summary>
        /// Creates a new thunk type (and related dispatcher and inner delegate types) for the specified delegate type.
        /// </summary>
        /// <param name="delegateType">The delegate for which create a thunk type.</param>
        /// <returns>A newly created thunk type.</returns>
        protected abstract Type CreateThunkType(Type delegateType);
    }
}
