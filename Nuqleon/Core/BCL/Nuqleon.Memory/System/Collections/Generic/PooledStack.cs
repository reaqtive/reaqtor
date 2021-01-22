// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 07/16/2014 - Created this type.
//

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1010 // Type inherits ICollection without implementing ICollection<T>. (Just like Stack<T>; nothing more, nothing less.)

using System.Diagnostics;

namespace System.Collections.Generic
{
    //=============================================================\\
    //   _____            _          _  _____ _             _      \\
    //  |  __ \          | |        | |/ ____| |           | |     \\
    //  | |__) |__   ___ | | ___  __| | (___ | |_ __ _  ___| | __  \\
    //  |  ___/ _ \ / _ \| |/ _ \/ _` |\___ \| __/ _` |/ __| |/ /  \\
    //  | |  | (_) | (_) | |  __/ (_| |____) | || (_| | (__|   <   \\
    //  |_|   \___/ \___/|_|\___|\__,_|_____/ \__\__,_|\___|_|\_\  \\
    //                                                             \\
    //=============================================================\\

    public partial class PooledStack<T>
    {
        /// <summary>
        /// Singleton instance of a default pool for stacks.
        /// </summary>
        /// <remarks>
        /// * The default capacity for this pool was picked based on measurements of typical usage patterns
        ///   in the context of System.Linq.CompilerServices but seems like a good overall value.
        /// * Notice this capacity holds for every T generic type parameter.
        /// * Also keep in mind that the capacity is the maximum number of instances that can be retained;
        ///   if not or rarely used, the number of instances held by the pool will remain low.
        /// </remarks>
        private static readonly StackPool<T> PoolInstance = StackPool<T>.Create(128);

#pragma warning disable CA1000 // Do not declare static members on generic types

        /// <summary>
        /// Gets a pooled <see cref="Stack{T}"/> instance, from the global pool, with RAII capabilities to return it to the pool.
        /// </summary>
        /// <returns>Pooled stack instance.</returns>
        public static PooledStackHolder<T> New() => PoolInstance.New();

#pragma warning disable CA1062 // Check for null. (Omitted by design; see remarks.)

        /// <summary>
        /// Gets a pooled <see cref="Stack{T}"/> instance, from the specified pool, with RAII capabilities to return it to the pool.
        /// </summary>
        /// <param name="pool">Pool to allocate from.</param>
        /// <returns>Pooled stack instance.</returns>
        /// <exception cref="NullReferenceException">Thrown if <paramref name="pool"/> is null.</exception>
        public static PooledStackHolder<T> New(StackPool<T> pool)
        {
            // This method is performance-critical and acts as a replacement for "newobj".
            // As such, we don't perform argument checks and rely on correct use patterns.
            // Worst case, the pool passed in is null and a NullReferenceException occurs.

            return pool.New();
        }

#pragma warning restore CA1062

        /// <summary>
        /// Gets a <see cref="Stack{T}"/> instance from the global stack pool.
        /// </summary>
        /// <returns>A <see cref="Stack{T}"/> instance obtained from the global pool.</returns>
        public static PooledStack<T> GetInstance()
        {
            var instance = PoolInstance.Allocate();
            Debug.Assert(instance.Count == 0);
            return instance;
        }

#pragma warning restore CA1000
    }


    //==================================================\\
    //    _____ _             _    _____            _   \\
    //   / ____| |           | |  |  __ \          | |  \\
    //  | (___ | |_ __ _  ___| | _| |__) |__   ___ | |  \\
    //   \___ \| __/ _` |/ __| |/ /  ___/ _ \ / _ \| |  \\
    //   ____) | || (_| | (__|   <| |  | (_) | (_) | |  \\
    //  |_____/ \__\__,_|\___|_|\_\_|   \___/ \___/|_|  \\
    //                                                  \\
    //==================================================\\

    public partial class StackPool<T>
    {
        static partial void CheckArguments(int capacity)
        {
            Errors.ThrowArgumentOutOfRangeIf(capacity < 0, nameof(capacity));
        }

        static partial void CheckArguments(int capacity, int maxCapacity)
        {
            Errors.ThrowArgumentOutOfRangeIf(capacity < 0, nameof(capacity));
            Errors.ThrowArgumentOutOfRangeIf(maxCapacity < 0, nameof(maxCapacity));
        }
    }
}
