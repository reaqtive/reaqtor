// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 07/02/2014 - Created this type.
//

using System.Diagnostics;

namespace System.Collections.Generic
{
    //====================================================================================\\
    //   _____            _          _ _____  _      _   _                                \\
    //  |  __ \          | |        | |  __ \(_)    | | (_)                               \\
    //  | |__) |__   ___ | | ___  __| | |  | |_  ___| |_ _  ___  _ __   __ _ _ __ _   _   \\
    //  |  ___/ _ \ / _ \| |/ _ \/ _` | |  | | |/ __| __| |/ _ \| '_ \ / _` | '__| | | |  \\
    //  | |  | (_) | (_) | |  __/ (_| | |__| | | (__| |_| | (_) | | | | (_| | |  | |_| |  \\
    //  |_|   \___/ \___/|_|\___|\__,_|_____/|_|\___|\__|_|\___/|_| |_|\__,_|_|   \__, |  \\
    //                                                                             __/ |  \\
    //                                                                            |___/   \\
    //====================================================================================\\

    public partial class PooledDictionary<TKey, TValue>
    {
        /// <summary>
        /// Singleton instance of a default pool for dictionaries.
        /// </summary>
        /// <remarks>
        /// * The default capacity for this pool was picked based on measurements of typical usage patterns
        ///   in the context of System.Linq.CompilerServices but seems like a good overall value.
        /// * Notice this capacity holds for every combination of TKey and TValue generic type parameters.
        /// * Also keep in mind that the capacity is the maximum number of instances that can be retained;
        ///   if not or rarely used, the number of instances held by the pool will remain low.
        /// </remarks>
        private static readonly DictionaryPool<TKey, TValue> PoolInstance = DictionaryPool<TKey, TValue>.Create(128);

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1000 // Do not declare static members on generic types

        /// <summary>
        /// Gets a pooled <see cref="Dictionary{TKey, TValue}"/> instance, from the global pool, with RAII capabilities to return it to the pool.
        /// </summary>
        /// <returns>Pooled dictionary instance.</returns>
        public static PooledDictionaryHolder<TKey, TValue> New() => PoolInstance.New();

#pragma warning disable CA1062 // Check for null. (Omitted by design; see remarks.)

        /// <summary>
        /// Gets a pooled <see cref="Dictionary{TKey, TValue}"/> instance, from the specified pool, with RAII capabilities to return it to the pool.
        /// </summary>
        /// <param name="pool">Pool to allocate from.</param>
        /// <returns>Pooled dictionary instance.</returns>
        /// <exception cref="NullReferenceException">Thrown if <paramref name="pool"/> is null.</exception>
        public static PooledDictionaryHolder<TKey, TValue> New(DictionaryPool<TKey, TValue> pool)
        {
            // This method is performance-critical and acts as a replacement for "newobj".
            // As such, we don't perform argument checks and rely on correct use patterns.
            // Worst case, the pool passed in is null and a NullReferenceException occurs.

            return pool.New();
        }

#pragma warning restore CA1062

        /// <summary>
        /// Gets a <see cref="Dictionary{TKey, TValue}"/> instance from the global dictionary pool.
        /// </summary>
        /// <returns>A <see cref="Dictionary{TKey, TValue}"/> instance obtained from the global pool.</returns>
        public static PooledDictionary<TKey, TValue> GetInstance()
        {
            var instance = PoolInstance.Allocate();
            Debug.Assert(instance.Count == 0);
            return instance;
        }

#pragma warning restore CA1000
#pragma warning restore IDE0079
    }


    //=========================================================================\\
    //   _____  _      _   _                              _____            _   \\
    //  |  __ \(_)    | | (_)                            |  __ \          | |  \\
    //  | |  | |_  ___| |_ _  ___  _ __   __ _ _ __ _   _| |__) |__   ___ | |  \\
    //  | |  | | |/ __| __| |/ _ \| '_ \ / _` | '__| | | |  ___/ _ \ / _ \| |  \\
    //  | |__| | | (__| |_| | (_) | | | | (_| | |  | |_| | |  | (_) | (_) | |  \\
    //  |_____/|_|\___|\__|_|\___/|_| |_|\__,_|_|   \__, |_|   \___/ \___/|_|  \\
    //                                               __/ |                     \\
    //                                              |___/                      \\
    //=========================================================================\\

    public partial class DictionaryPool<TKey, TValue>
    {
        static partial void CheckArguments(int capacity)
        {
            Errors.ThrowArgumentOutOfRangeIf(capacity < 0, nameof(capacity));
        }

        static partial void CheckArguments(IEqualityComparer<TKey> comparer)
        {
            Errors.ThrowArgumentNull(comparer, nameof(comparer));
        }

        static partial void CheckArguments(int capacity, IEqualityComparer<TKey> comparer)
        {
            Errors.ThrowArgumentOutOfRangeIf(capacity < 0, nameof(capacity));
            Errors.ThrowArgumentNull(comparer, nameof(comparer));
        }

        static partial void CheckArguments(int capacity, IEqualityComparer<TKey> comparer, int maxCapacity)
        {
            Errors.ThrowArgumentOutOfRangeIf(capacity < 0, nameof(capacity));
            Errors.ThrowArgumentNull(comparer, nameof(comparer));
            Errors.ThrowArgumentOutOfRangeIf(maxCapacity < 0, nameof(maxCapacity));
        }
    }
}
