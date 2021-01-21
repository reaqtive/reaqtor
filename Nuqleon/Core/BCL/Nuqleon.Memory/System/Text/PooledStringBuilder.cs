// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 07/02/2014 - Created this type.
//

using System.Diagnostics;

namespace System.Text
{
    //=================================================================================================\\
    //   _____            _          _  _____ _        _             ____        _ _     _             \\
    //  |  __ \          | |        | |/ ____| |      (_)           |  _ \      (_) |   | |            \\
    //  | |__) |__   ___ | | ___  __| | (___ | |_ _ __ _ _ __   __ _| |_) |_   _ _| | __| | ___ _ __   \\
    //  |  ___/ _ \ / _ \| |/ _ \/ _` |\___ \| __| '__| | '_ \ / _` |  _ <| | | | | |/ _` |/ _ \ '__|  \\
    //  | |  | (_) | (_) | |  __/ (_| |____) | |_| |  | | | | | (_| | |_) | |_| | | | (_| |  __/ |     \\
    //  |_|   \___/ \___/|_|\___|\__,_|_____/ \__|_|  |_|_| |_|\__, |____/ \__,_|_|_|\__,_|\___|_|     \\
    //                                                          __/ |                                  \\
    //                                                         |___/                                   \\
    //=================================================================================================\\

    public partial class PooledStringBuilder
    {
        /// <summary>
        /// Singleton instance of a default pool for string builders.
        /// </summary>
        /// <remarks>
        /// * The default capacity for this pool was picked based on measurements of typical usage patterns
        ///   in the context of System.Linq.CompilerServices but seems like a good overall value.
        /// * Keep in mind that the capacity is the maximum number of instances that can be retained; if
        ///   not or rarely used, the number of instances held by the pool will remain low.
        /// </remarks>
        private static readonly StringBuilderPool PoolInstance = StringBuilderPool.Create(32);

        /// <summary>
        /// Gets a pooled <see cref="StringBuilder"/> instance, from the global pool, with RAII capabilities to return it to the pool.
        /// </summary>
        /// <returns>Pooled string builder instance.</returns>
        public static PooledStringBuilderHolder New() => PoolInstance.New();

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1062 // Check for null. (Omitted by design; see remarks.)

        /// <summary>
        /// Gets a pooled <see cref="StringBuilder"/> instance, from the specified pool, with RAII capabilities to return it to the pool.
        /// </summary>
        /// <param name="pool">Pool to allocate from.</param>
        /// <returns>Pooled string builder instance.</returns>
        /// <exception cref="NullReferenceException">Thrown if <paramref name="pool"/> is null.</exception>
        public static PooledStringBuilderHolder New(StringBuilderPool pool)
        {
            // This method is performance-critical and acts as a replacement for "newobj".
            // As such, we don't perform argument checks and rely on correct use patterns.
            // Worst case, the pool passed in is null and a NullReferenceException occurs.

            return pool.New();
        }

#pragma warning restore CA1062
#pragma warning restore IDE0079

        /// <summary>
        /// Gets a <see cref="StringBuilder"/> instance from the global pool.
        /// </summary>
        /// <returns>A <see cref="StringBuilder"/> instance obtained from the global pool.</returns>
        public static PooledStringBuilder GetInstance()
        {
            var builder = PoolInstance.Allocate();
            Debug.Assert(builder.StringBuilder.Length == 0);
            return builder;
        }
    }

    public partial class PooledStringBuilder
    {
        /// <summary>
        /// Gets the string built by the string builder.
        /// </summary>
        /// <returns>String built by the string builder.</returns>
        [Obsolete("Consider calling ToStringAndFree instead.")]
        public new string ToString() => StringBuilder.ToString();

        /// <summary>
        /// Gets the string built by the string builder and returns the builder back to the pool.
        /// </summary>
        /// <returns>String built by the string builder.</returns>
        public string ToStringAndFree()
        {
            var result = StringBuilder.ToString();
            Free();

            return result;
        }

        /// <summary>
        /// Converts a pooled string builder to its underlying StringBuilder instance.
        /// </summary>
        /// <param name="obj">Pooled string builder to convert.</param>
        /// <returns>The underlying StringBuilder instance.</returns>
        public static implicit operator StringBuilder(PooledStringBuilder obj) => obj?.StringBuilder;

        /// <summary>
        /// Obtains the underlying StringBuilder instance.
        /// </summary>
        /// <returns>The underlying StringBuilder instance.</returns>
        public StringBuilder ToStringBuilder() => StringBuilder;
    }


    //======================================================================================\\
    //    _____ _        _             ____        _ _     _           _____            _   \\
    //   / ____| |      (_)           |  _ \      (_) |   | |         |  __ \          | |  \\
    //  | (___ | |_ _ __ _ _ __   __ _| |_) |_   _ _| | __| | ___ _ __| |__) |__   ___ | |  \\
    //   \___ \| __| '__| | '_ \ / _` |  _ <| | | | | |/ _` |/ _ \ '__|  ___/ _ \ / _ \| |  \\
    //   ____) | |_| |  | | | | | (_| | |_) | |_| | | | (_| |  __/ |  | |  | (_) | (_) | |  \\
    //  |_____/ \__|_|  |_|_| |_|\__, |____/ \__,_|_|_|\__,_|\___|_|  |_|   \___/ \___/|_|  \\
    //                            __/ |                                                     \\
    //                           |___/                                                      \\
    //======================================================================================\\

    public partial class StringBuilderPool
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
