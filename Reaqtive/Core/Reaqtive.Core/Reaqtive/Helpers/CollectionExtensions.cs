// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2014 - Created this file.
//

using System.Linq;

namespace System.Collections.Generic
{
    /// <summary>
    /// Provides extension methods for collections.
    /// </summary>
    internal static class CollectionExtensions
    {
        /// <summary>
        /// Converts the specified sequence to a collection.
        /// If the specified sequence is null, an empty array is returned.
        /// If the specified sequence is already a collection, it gets returned after type conversion. Otherwise, a collection is created with the sequence's elements.
        /// </summary>
        /// <typeparam name="T">Type of the elements in the sequence.</typeparam>
        /// <param name="enumerable">Sequence to get a collection representation for.</param>
        /// <returns>Collection containing the elements of the specified sequence.</returns>
        public static ICollection<T> AsCollection<T>(this IEnumerable<T> enumerable)
        {
            return enumerable switch
            {
                null => Array.Empty<T>(),
                ICollection<T> collection => collection,
                _ => enumerable.ToList(),
            };
        }
    }
}
