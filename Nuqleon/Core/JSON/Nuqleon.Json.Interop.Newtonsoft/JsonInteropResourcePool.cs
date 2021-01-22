// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2017 - Created this file.
//

using System;
using System.Memory;

namespace Nuqleon.Json.Interop.Newtonsoft
{
    // REVIEW: Consider adding support to specify some maximum capacity for the objects
    //         held in the pool. Unfortunately, the implementation detail of a TokenStack
    //         is hidden from the user, so the number they'd pass would have relatively
    //         little meaning (though we could label it as "maximum token stack depth").

    /// <summary>
    /// Provides a pool of resources used by <see cref="JsonExpressionReader"/> and <see cref="JsonExpressionWriter"/>.
    /// </summary>
    public sealed class JsonInteropResourcePool
    {
        internal readonly ObjectPool<TokenStack> Pool;

        /// <summary>
        /// Creates a new resource pool with the default capacity.
        /// </summary>
        public JsonInteropResourcePool()
            : this(Environment.ProcessorCount)
        {
        }

        /// <summary>
        /// Creates a new resource pool with the specified capacity.
        /// </summary>
        /// <param name="capacity">The capacity of the resource pool.</param>
        public JsonInteropResourcePool(int capacity) => Pool = new ObjectPool<TokenStack>(() => new TokenStack(), capacity);
    }
}
