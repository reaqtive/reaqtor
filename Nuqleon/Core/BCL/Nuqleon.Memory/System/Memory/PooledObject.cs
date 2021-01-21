// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 07/02/2014 - Created this type.
//

using System.Collections.Generic;
using System.Diagnostics;

namespace System.Memory
{
    //================================================================\\
    //   _____            _          _  ____  _     _           _     \\
    //  |  __ \          | |        | |/ __ \| |   (_)         | |    \\
    //  | |__) |__   ___ | | ___  __| | |  | | |__  _  ___  ___| |_   \\
    //  |  ___/ _ \ / _ \| |/ _ \/ _` | |  | | '_ \| |/ _ \/ __| __|  \\
    //  | |  | (_) | (_) | |  __/ (_| | |__| | |_) | |  __/ (__| |_   \\
    //  |_|   \___/ \___/|_|\___|\__,_|\____/|_.__/| |\___|\___|\__|  \\
    //                                            _/ |                \\
    //                                           |__/                 \\
    //================================================================\\

    /// <summary>
    /// Struct holding a pooled object, exposing RAII capabilities to return the object to the pool upon disposal.
    /// </summary>
    /// <typeparam name="T">Type of the object.</typeparam>
    public struct PooledObject<T> : IDisposable, IEquatable<PooledObject<T>>
        where T : class
    {
        /// <summary>
        /// Pool to which the pooled object belongs.
        /// </summary>
        private readonly ObjectPoolBase<T> _pool;

        /// <summary>
        /// Action used to release the instance of the pooled object from the pool.
        /// </summary>
        private readonly Action<ObjectPoolBase<T>, T> _releaser;

        /// <summary>
        /// Allocates a new object from the pool and stores it in the wrapper instance.
        /// </summary>
        /// <param name="pool">Pool to obtain an object from.</param>
        public PooledObject(ObjectPoolBase<T> pool)
            : this()
        {
            Debug.Assert(pool != null);

            _pool = pool;
            Object = pool.Allocate();
        }

        /// <summary>
        /// Allocates a new object from the pool and stores it in the wrapper instance.
        /// </summary>
        /// <param name="pool">Pool to obtain an object from.</param>
        /// <param name="allocator">Custom allocator function.</param>
        /// <param name="releaser">Custom releaser function.</param>
        public PooledObject(ObjectPoolBase<T> pool, Func<ObjectPoolBase<T>, T> allocator, Action<ObjectPoolBase<T>, T> releaser)
            : this()
        {
            Debug.Assert(pool != null);
            Debug.Assert(allocator != null);
            Debug.Assert(releaser != null);

            _pool = pool;
            _releaser = releaser;
            Object = allocator(pool);
        }


#pragma warning disable CA1720 // Identifier contains type name. (By design; PooledXYZ exposes an XYZ property.)

        /// <summary>
        /// Gets the pooled object instance.
        /// </summary>
        /// <returns>Pooled object instance, or null if the object has been freed.</returns>
        public T Object { get; private set; }

#pragma warning restore CA1720

        /// <summary>
        /// Checks if the current object is equal to the specified object.
        /// </summary>
        /// <param name="other">The object to compare to.</param>
        /// <returns>true if the objects are equal; otherwise, false.</returns>
        public bool Equals(PooledObject<T> other) =>
            _pool == other._pool &&
            _releaser == other._releaser &&
            EqualityComparer<T>.Default.Equals(Object, other.Object);

        /// <summary>
        /// Checks if the current object is equal to the specified object.
        /// </summary>
        /// <param name="obj">The object to compare to.</param>
        /// <returns>true if the objects are equal; otherwise, false.</returns>
        public override bool Equals(object obj) => obj is PooledObject<T> o && Equals(o);

        /// <summary>
        /// Gets a hash code for the current object.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode() =>
            HashHelpers.Combine(
                _pool?.GetHashCode() ?? 0,
                _releaser?.GetHashCode() ?? 0,
                EqualityComparer<T>.Default.GetHashCode(Object)
            );

        /// <summary>
        /// Checks if the two objects are equal.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns>true if both objects are equal; otherwise, false.</returns>
        public static bool operator ==(PooledObject<T> left, PooledObject<T> right) => left.Equals(right);

        /// <summary>
        /// Checks if the two objects are not equal.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns>true if both objects are not equal; otherwise, false.</returns>
        public static bool operator !=(PooledObject<T> left, PooledObject<T> right) => !(left == right);

        /// <summary>
        /// Disposes the pooled object holder, returning the pooled object to its pool.
        /// </summary>
        public void Dispose()
        {
            if (Object != null)
            {
                if (_releaser != null)
                {
                    _releaser(_pool, Object);
                }
                else
                {
                    if (Object is IFreeable freeable)
                    {
                        freeable.Free();
                    }
                    else
                    {
                        _pool.Free(Object);
                    }
                }

                Object = null;
            }
        }
    }
}
