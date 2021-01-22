// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD, ER - July 2013 - Created this file.
//

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1062 // Omitted null checks similar to expression tree visitors.

using System;
using System.Collections.Generic;
using System.Memory;

namespace Nuqleon.DataModel.TypeSystem
{
    /// <summary>
    /// Equality comparer for objects with data model-compliant types.
    /// </summary>
#if SUPPORT_SUBSET_TYPES
    /// <remarks>
    /// The left object (i.e., the expected object) may contain a subset
    /// of the properties available to the right object, so long as these
    /// remaining properties have default values.
    /// </remarks>
#endif
    [KnownType]
    public class DataTypeObjectEqualityComparer : IEqualityComparer<object>
    {
        private static readonly ObjectPool<DataTypeObjectEqualityComparator> s_pool = new(() => new DataTypeObjectEqualityComparator());

        private readonly Func<DataTypeObjectEqualityComparator> _comparatorFactory;
        private readonly bool _isPooled;

        /// <summary>
        /// Instantiates the comparer.
        /// </summary>
        public DataTypeObjectEqualityComparer()
            : this(s_pool.Allocate)
        {
            _isPooled = true;
        }

        /// <summary>
        /// Instantiates the comparer with a custom comparator factory.
        /// </summary>
        /// <param name="comparatorFactory">Produces comparator instances to compute equivalence and hash codes.</param>
        public DataTypeObjectEqualityComparer(Func<DataTypeObjectEqualityComparator> comparatorFactory)
        {
            _comparatorFactory = comparatorFactory;
        }

        /// <summary>
        /// Gets the default instance of the comparer.
        /// </summary>
        public static DataTypeObjectEqualityComparer Default { get; } = new DataTypeObjectEqualityComparer();

        /// <summary>
        /// Checks for value equality of two objects with data model-compliant types.
        /// </summary>
        /// <param name="x">The first object.</param>
        /// <param name="y">The second object.</param>
        /// <returns>true if the objects are equal, false otherwise.</returns>
        public new bool Equals(object x, object y)
        {
            var comparator = _comparatorFactory();

            try
            {
                return comparator.Equals(x, y);
            }
            finally
            {
                if (_isPooled && comparator.ShouldReturnToPool)
                {
                    s_pool.Free(comparator);
                }
            }
        }

        /// <summary>
        /// Gets the hash code of an object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>The hash code of the object.</returns>
        public int GetHashCode(object obj)
        {
            var comparator = _comparatorFactory();

            try
            {
                return comparator.GetHashCode(obj);
            }
            finally
            {
                if (_isPooled && comparator.ShouldReturnToPool)
                {
                    s_pool.Free(comparator);
                }
            }
        }
    }
}
