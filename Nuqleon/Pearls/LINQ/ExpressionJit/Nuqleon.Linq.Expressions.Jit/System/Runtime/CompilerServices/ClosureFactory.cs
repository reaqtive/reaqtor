// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - May 2017 - Initial prototype of JIT.
//

using System.Collections.Generic;

namespace System.Runtime.CompilerServices
{
    /// <summary>
    /// Factory for closure types.
    /// </summary>
    internal static class ClosureFactory
    {
        /// <summary>
        /// Cache of runtime generated closure types. The key represents the generic arity.
        /// </summary>
        /// <remarks>
        /// Access to this dictionary should be synchronized using a lock on the collection itself.
        /// </remarks>
        private static readonly Dictionary<int, Type> s_dynamicClosures = new();

        /// <summary>
        /// Gets a closure type with storage locations of the specified types. These storage
        /// locations will be available through fields named <c>Item1</c> through <c>ItemN</c>
        /// and through the <see cref="IRuntimeVariables"/> indexer. 
        /// </summary>
        /// <param name="types">The types of the storage locations in the closure.</param>
        /// <returns>A closure type with fields of the specified types.</returns>
        public static Type GetClosureType(params Type[] types)
        {
            Type closureType;

            var arity = types.Length;

            //
            // Try to find a built-in closure type and fall back to generating a custom
            // thunk type at runtime if no built-in one is found.
            //
            if (arity < Closures.Types.Count)
            {
                closureType = Closures.Types[arity];
            }
            else
            {
                closureType = GetRuntimeClosureType(arity);
            }

            //
            // NB: In case we reach here for a closure containing no fields, we simply
            //     return the entry which is supposed to be a type with no generic
            //     parameters (currently `object`). This simplifies code-gen.
            //
            if (arity > 0)
            {
                closureType = closureType.MakeGenericType(types);
            }

            return closureType;
        }

        /// <summary>
        /// Gets a runtime generated open generic closure type with the specified arity.
        /// </summary>
        /// <param name="arity">The number of slots in the closure.</param>
        /// <returns>An open generic closure type with the specified arity.</returns>
        private static Type GetRuntimeClosureType(int arity)
        {
            var closureType = default(Type);

            //
            // Use a simple locking strategy to access the cache of closure types. Lookup
            // should be fast enough and its cost marginal compared to the rest of an
            // expression tree compilation process.
            //
            lock (s_dynamicClosures)
            {
                if (!s_dynamicClosures.TryGetValue(arity, out closureType))
                {
                    //
                    // NB: Running this code under the lock is by design. We want to avoid
                    //     generating the same closure type twice. Hitting this code path
                    //     should also be very rare: at most once per arity beyond the number
                    //     of built-in closure types (currently 16).
                    //
                    closureType = RuntimeTypeFactory.CreateClosureType(arity);
                    s_dynamicClosures[arity] = closureType;
                }
            }

            return closureType;
        }
    }
}
