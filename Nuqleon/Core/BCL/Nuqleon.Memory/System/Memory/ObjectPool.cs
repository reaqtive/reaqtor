// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 07/02/2014 - Created this type.
//

namespace System.Memory
{
    //=====================================================\\
    //    ____  _     _           _   _____            _   \\
    //   / __ \| |   (_)         | | |  __ \          | |  \\
    //  | |  | | |__  _  ___  ___| |_| |__) |__   ___ | |  \\
    //  | |  | | '_ \| |/ _ \/ __| __|  ___/ _ \ / _ \| |  \\
    //  | |__| | |_) | |  __/ (__| |_| |  | (_) | (_) | |  \\
    //   \____/|_.__/| |\___|\___|\__|_|   \___/ \___/|_|  \\
    //              _/ |                                   \\
    //             |__/                                    \\
    //=====================================================\\

    /// <summary>
    /// Generic implementation of object pooling pattern with predefined pool size limit. The main
    /// purpose is that a limited number of frequently used objects can be kept in the pool for
    /// further recycling.
    /// </summary>
    /// <typeparam name="T">Type of the elements stored in the pool.</typeparam>
    /// <remarks>
    /// 1) It is not the goal to keep all returned objects. The pool is not meant for storage. If there
    ///    is no space in the pool, extra returned objects will be dropped.
    /// 2) It is implied that if object was obtained from a pool, the caller will return it back in a
    ///    relatively short time. Keeping objects checked out for long durations is ok, but reduces
    ///    usefulness of pooling. Just new up your own if that's your intended object usage.
    /// 3) Not returning objects to the pool in not detrimental to the pool's work, but is a bad practice.
    /// 
    /// Rationale:
    ///    If there is no intent for reusing the object, do not use pool - just use "new".
    /// </remarks>
    public class ObjectPool<T> : ObjectPoolBase<T>
        where T : class
    {
        /// <summary>
        /// Factory delegate used to create new object instances.
        /// </summary>
        private readonly Func<T> _factory;

        /// <summary>
        /// Creates a new object pool using the specified factory to create new object instances.
        /// </summary>
        /// <param name="factory">Factory delegate used to create new object instances.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="factory"/> is null.</exception>
        public ObjectPool(Func<T> factory)
            : this(factory, Environment.ProcessorCount * 2)
        {
        }

        /// <summary>
        /// Creates a new object pool using the specified factory to create new object instances
        /// and using the specified pool size.
        /// </summary>
        /// <param name="factory">Factory delegate used to create new object instances.</param>
        /// <param name="size">Number of object instances to keep in the pool.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="factory"/> is null.</exception>
        public ObjectPool(Func<T> factory, int size)
            : base(size)
        {
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }

        /// <summary>
        /// Creates an instance of a pooled object.
        /// </summary>
        /// <returns>Newly created pooled object instance.</returns>
        protected override T CreateInstance()
        {
            var inst = _factory();
            return inst;
        }
    }
}
