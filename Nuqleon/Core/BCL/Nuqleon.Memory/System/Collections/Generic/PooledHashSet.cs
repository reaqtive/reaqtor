// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 07/02/2014 - Created this type.
//

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace System.Collections.Generic
{
    //==========================================================================\\
    //   _____            _          _ _    _           _      _____      _     \\
    //  |  __ \          | |        | | |  | |         | |    / ____|    | |    \\
    //  | |__) |__   ___ | | ___  __| | |__| | __ _ ___| |__ | (___   ___| |_   \\
    //  |  ___/ _ \ / _ \| |/ _ \/ _` |  __  |/ _` / __| '_ \ \___ \ / _ \ __|  \\
    //  | |  | (_) | (_) | |  __/ (_| | |  | | (_| \__ \ | | |____) |  __/ |_   \\
    //  |_|   \___/ \___/|_|\___|\__,_|_|  |_|\__,_|___/_| |_|_____/ \___|\__|  \\
    //                                                                          \\
    //==========================================================================\\

    [SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Justification = "Standard pattern of derivation.")]
    public partial class PooledHashSet<T>
    {
        /// <summary>
        /// Singleton instance of a default pool for hash sets.
        /// </summary>
        /// <remarks>
        /// * The default capacity for this pool was picked based on measurements of typical usage patterns
        ///   in the context of System.Linq.CompilerServices but seems like a good overall value.
        /// * Notice this capacity holds for every T generic type parameter.
        /// * Also keep in mind that the capacity is the maximum number of instances that can be retained;
        ///   if not or rarely used, the number of instances held by the pool will remain low.
        /// </remarks>
        private static readonly HashSetPool<T> PoolInstance = HashSetPool<T>.Create(128);

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1000 // Do not declare static members on generic types

        /// <summary>
        /// Gets a pooled <see cref="HashSet{T}"/> instance, from the global pool, with RAII capabilities to return it to the pool.
        /// </summary>
        /// <returns>Pooled hash set instance.</returns>
        public static PooledHashSetHolder<T> New() => PoolInstance.New();

#pragma warning disable CA1062 // Check for null. (Omitted by design; see remarks.)

        /// <summary>
        /// Gets a pooled <see cref="HashSet{T}"/> instance, from the specified pool, with RAII capabilities to return it to the pool.
        /// </summary>
        /// <param name="pool">Pool to allocate from.</param>
        /// <returns>Pooled hash set instance.</returns>
        /// <exception cref="NullReferenceException">Thrown if <paramref name="pool"/> is null.</exception>
        public static PooledHashSetHolder<T> New(HashSetPool<T> pool)
        {
            // This method is performance-critical and acts as a replacement for "newobj".
            // As such, we don't perform argument checks and rely on correct use patterns.
            // Worst case, the pool passed in is null and a NullReferenceException occurs.

            return pool.New();
        }

#pragma warning restore CA1062
#pragma warning restore IDE0079

        /// <summary>
        /// Gets a <see cref="HashSet{T}"/> instance from the global hash set pool.
        /// </summary>
        /// <returns>A <see cref="HashSet{T}"/> instance obtained from the global pool.</returns>
        public static PooledHashSet<T> GetInstance()
        {
            var instance = PoolInstance.Allocate();
            Debug.Assert(instance.Count == 0);
            return instance;
        }

#pragma warning restore CA1000
    }


    //===============================================================\\
    //   _    _           _      _____      _   _____            _   \\
    //  | |  | |         | |    / ____|    | | |  __ \          | |  \\
    //  | |__| | __ _ ___| |__ | (___   ___| |_| |__) |__   ___ | |  \\
    //  |  __  |/ _` / __| '_ \ \___ \ / _ \ __|  ___/ _ \ / _ \| |  \\
    //  | |  | | (_| \__ \ | | |____) |  __/ |_| |  | (_) | (_) | |  \\
    //  |_|  |_|\__,_|___/_| |_|_____/ \___|\__|_|   \___/ \___/|_|  \\
    //                                                               \\
    //===============================================================\\

    public partial class HashSetPool<T>
    {
        static partial void CheckArguments(IEqualityComparer<T> comparer)
        {
            Errors.ThrowArgumentNull(comparer, nameof(comparer));
        }

        static partial void CheckArguments(IEqualityComparer<T> comparer, int maxCapacity)
        {
            Errors.ThrowArgumentNull(comparer, nameof(comparer));
            Errors.ThrowArgumentOutOfRangeIf(maxCapacity < 0, nameof(maxCapacity));
        }
    }
}
