// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 07/16/2014 - Created this type.
//

using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace System.Collections.Generic
{
    //====================================================\\
    //   _____            _          _ _      _     _     \\
    //  |  __ \          | |        | | |    (_)   | |    \\
    //  | |__) |__   ___ | | ___  __| | |     _ ___| |_   \\
    //  |  ___/ _ \ / _ \| |/ _ \/ _` | |    | / __| __|  \\
    //  | |  | (_) | (_) | |  __/ (_| | |____| \__ \ |_   \\
    //  |_|   \___/ \___/|_|\___|\__,_|______|_|___/\__|  \\
    //                                                    \\
    //====================================================\\

    [SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Justification = "Standard pattern of derivation.")]
    public partial class PooledList<T>
    {
        /// <summary>
        /// Singleton instance of a default pool for lists.
        /// </summary>
        /// <remarks>
        /// * The default capacity for this pool was picked based on measurements of typical usage patterns
        ///   in the context of System.Linq.CompilerServices but seems like a good overall value.
        /// * Notice this capacity holds for every T generic type parameter.
        /// * Also keep in mind that the capacity is the maximum number of instances that can be retained;
        ///   if not or rarely used, the number of instances held by the pool will remain low.
        /// </remarks>
        private static readonly ListPool<T> PoolInstance = ListPool<T>.Create(128);

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1000 // Do not declare static members on generic types

        /// <summary>
        /// Gets a pooled <see cref="List{T}"/> instance, from the global pool, with RAII capabilities to return it to the pool.
        /// </summary>
        /// <returns>Pooled list instance.</returns>
        public static PooledListHolder<T> New() => PoolInstance.New();

#pragma warning disable CA1062 // Check for null. (Omitted by design; see remarks.)

        /// <summary>
        /// Gets a pooled <see cref="List{T}"/> instance, from the specified pool, with RAII capabilities to return it to the pool.
        /// </summary>
        /// <param name="pool">Pool to allocate from.</param>
        /// <returns>Pooled list instance.</returns>
        /// <exception cref="NullReferenceException">Thrown if <paramref name="pool"/> is null.</exception>
        public static PooledListHolder<T> New(ListPool<T> pool)
        {
            // This method is performance-critical and acts as a replacement for "newobj".
            // As such, we don't perform argument checks and rely on correct use patterns.
            // Worst case, the pool passed in is null and a NullReferenceException occurs.

            return pool.New();
        }

#pragma warning restore CA1062

        /// <summary>
        /// Gets a <see cref="List{T}"/> instance from the global list pool.
        /// </summary>
        /// <returns>A <see cref="List{T}"/> instance obtained from the global pool.</returns>
        public static PooledList<T> GetInstance()
        {
            var instance = PoolInstance.Allocate();
            Debug.Assert(instance.Count == 0);
            return instance;
        }

#pragma warning restore CA1000
#pragma warning restore IDE0079
    }

    public partial class PooledList<T>
    {
        /// <summary>
        /// Returns a read-only wrapper for the current collection.
        /// </summary>
        /// <returns>A <see cref="ReadOnlyCollection{T}"/> that acts as a read-only wrapper around the current collection.</returns>
        [Obsolete("Use of AsReadOnly is dangerous. The resulting collection should not be shared because it aliases the underlying collection. Use AsReadOnlyCopy if a copy should be made.")]
        public new ReadOnlyCollection<T> AsReadOnly()
        {
            return base.AsReadOnly();
        }

        /// <summary>
        /// Returns a read-only copy of the current collection.
        /// </summary>
        /// <returns>A <see cref="ReadOnlyCollection{T}"/> that contains a copy of the elements in the current collection.</returns>
        public ReadOnlyCollection<T> AsReadOnlyCopy()
        {
            var res = new List<T>(Count);
            res.AddRange(this);

            return new ReadOnlyCollection<T>(res);
        }

        /// <summary>
        /// Returns a read-only copy of the current collection and returns the list to the pool.
        /// </summary>
        /// <returns>A <see cref="ReadOnlyCollection{T}"/> that contains a copy of the elements in the current collection.</returns>
        public ReadOnlyCollection<T> AsReadOnlyCopyAndFree()
        {
            var res = AsReadOnlyCopy();
            Free();

            return res;
        }
    }


    //=========================================\\
    //   _      _     _   _____            _   \\
    //  | |    (_)   | | |  __ \          | |  \\
    //  | |     _ ___| |_| |__) |__   ___ | |  \\
    //  | |    | / __| __|  ___/ _ \ / _ \| |  \\
    //  | |____| \__ \ |_| |  | (_) | (_) | |  \\
    //  |______|_|___/\__|_|   \___/ \___/|_|  \\
    //                                         \\
    //=========================================\\

    public partial class ListPool<T>
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
