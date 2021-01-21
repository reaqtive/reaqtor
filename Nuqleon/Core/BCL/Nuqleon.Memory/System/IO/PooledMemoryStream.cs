// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 07/02/2014 - Created this type.
//

using System.Diagnostics;

namespace System.IO
{
    //==============================================================================================================\\
    //   _____            _          _ __  __                                  _____ _                              \\
    //  |  __ \          | |        | |  \/  |                                / ____| |                             \\
    //  | |__) |__   ___ | | ___  __| | \  / | ___ _ __ ___   ___  _ __ _   _| (___ | |_ _ __ ___  __ _ _ __ ___    \\
    //  |  ___/ _ \ / _ \| |/ _ \/ _` | |\/| |/ _ \ '_ ` _ \ / _ \| '__| | | |\___ \| __| '__/ _ \/ _` | '_ ` _ \   \\
    //  | |  | (_) | (_) | |  __/ (_| | |  | |  __/ | | | | | (_) | |  | |_| |____) | |_| | |  __/ (_| | | | | | |  \\
    //  |_|   \___/ \___/|_|\___|\__,_|_|  |_|\___|_| |_| |_|\___/|_|   \__, |_____/ \__|_|  \___|\__,_|_| |_| |_|  \\
    //                                                                   __/ |                                      \\
    //                                                                  |___/                                       \\
    //==============================================================================================================\\

    public partial class PooledMemoryStream
    {
        partial void ClearCore()
        {
            SetLength(0);
        }
    }

    public partial class PooledMemoryStream
    {
        /// <summary>
        /// Singleton instance of a default pool for memory streams.
        /// </summary>
        /// <remarks>
        /// * The default capacity for this pool was picked based on measurements of typical usage patterns
        ///   in the context of System.Linq.CompilerServices but seems like a good overall value.
        /// * Also keep in mind that the capacity is the maximum number of instances that can be retained;
        ///   if not or rarely used, the number of instances held by the pool will remain low.
        /// </remarks>
        private static readonly MemoryStreamPool PoolInstance = MemoryStreamPool.Create(128);

        /// <summary>
        /// Gets a pooled <see cref="MemoryStream"/> instance, from the global pool, with RAII capabilities to return it to the pool.
        /// </summary>
        /// <returns>Pooled memory stream instance.</returns>
        public static PooledMemoryStreamHolder New() => PoolInstance.New();

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1062 // Check for null. (Omitted by design; see remarks.)

        /// <summary>
        /// Gets a pooled <see cref="MemoryStream"/> instance, from the specified pool, with RAII capabilities to return it to the pool.
        /// </summary>
        /// <param name="pool">Pool to allocate from.</param>
        /// <returns>Pooled memory stream instance.</returns>
        /// <exception cref="NullReferenceException">Thrown if <paramref name="pool"/> is null.</exception>
        public static PooledMemoryStreamHolder New(MemoryStreamPool pool)
        {
            // This method is performance-critical and acts as a replacement for "newobj".
            // As such, we don't perform argument checks and rely on correct use patterns.
            // Worst case, the pool passed in is null and a NullReferenceException occurs.

            return pool.New();
        }

#pragma warning restore CA1062
#pragma warning restore IDE0079

        /// <summary>
        /// Gets a <see cref="MemoryStream"/> instance from the global memory stream pool.
        /// </summary>
        /// <returns>A <see cref="MemoryStream"/> instance obtained from the global pool.</returns>
        public static PooledMemoryStream GetInstance()
        {
            var stream = PoolInstance.Allocate();
            Debug.Assert(stream.Length == 0);
            return stream;
        }
    }

    public partial class PooledMemoryStream
    {
        /// <summary>
        /// Gets a copy of the contents of the memory stream. Note that this method does not return the memory stream back to the pool. Consider
        /// using <see cref="ToArrayAndFree"/> to copy the contents of the memory stream to an array and return the memory stream to the pool.
        /// </summary>
        /// <returns>Byte array with a copy of the contents of the memory stream.</returns>
        public new byte[] ToArray() => base.ToArray();

        /// <summary>
        /// Gets a copy of the contents of the memory stream and returns the memory stream back to the pool.
        /// </summary>
        /// <returns>Byte array with a copy of the contents of the memory stream.</returns>
        public byte[] ToArrayAndFree()
        {
            var result = base.ToArray();
            Free();

            return result;
        }

        /// <summary>
        /// Gets the buffer with the contents of the memory stream. Users should be careful not to use the returned buffer beyond the point of
        /// returning the pooled memory stream back to the pool.
        /// </summary>
        /// <returns>Byte array with the contents of the memory stream.</returns>
        public new byte[] GetBuffer() => base.GetBuffer();

        /// <summary>
        /// Exposes the buffer with the contents of the memory stream and returns the memory stream back to the pool.
        /// </summary>
        /// <param name="processBuffer">Delegate to process the buffer. Once the delegate invocation returns, the buffer should no longer be accessed.</param>
        public void GetBufferAndFree(Action<byte[]> processBuffer)
        {
            if (processBuffer == null)
                throw new ArgumentNullException(nameof(processBuffer));

            processBuffer(base.GetBuffer());
            Free();
        }
    }


    //====================================================================================================\\
    //   __  __                                  _____ _                            _____            _    \\
    //  |  \/  |                                / ____| |                          |  __ \          | |   \\
    //  | \  / | ___ _ __ ___   ___  _ __ _   _| (___ | |_ _ __ ___  __ _ _ __ ___ | |__) |__   ___ | |   \\
    //  | |\/| |/ _ \ '_ ` _ \ / _ \| '__| | | |\___ \| __| '__/ _ \/ _` | '_ ` _ \|  ___/ _ \ / _ \| |   \\
    //  | |  | |  __/ | | | | | (_) | |  | |_| |____) | |_| | |  __/ (_| | | | | | | |  | (_) | (_) | |   \\
    //  |_|  |_|\___|_| |_| |_|\___/|_|   \__, |_____/ \__|_|  \___|\__,_|_| |_| |_|_|   \___/ \___/|_|   \\
    //                                     __/ |                                                          \\
    //                                    |___/                                                           \\
    //====================================================================================================\\

    public partial class MemoryStreamPool
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
