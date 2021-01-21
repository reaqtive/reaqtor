// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2014 - Created this file.
//

using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace System.Linq.CompilerServices
{
    /// <summary>
    /// Provides extension methods for collections.
    /// </summary>
    public static class CollectionExtensions
    {
        /// <summary>
        /// Converts the specified sequence to an array.
        /// If the specified sequence is null, an empty array is returned.
        /// If the specified sequence is already an array, it gets returned after type conversion. Otherwise, an array is created with the sequence's elements.
        /// </summary>
        /// <typeparam name="T">Type of the elements in the sequence.</typeparam>
        /// <param name="enumerable">Sequence to get an array representation for.</param>
        /// <returns>Array containing the elements of the specified sequence.</returns>
        public static T[] AsArray<T>(this IEnumerable<T> enumerable)
        {
            return enumerable switch
            {
                null => Array.Empty<T>(),
                T[] array => array,
                _ => enumerable.ToArray(),
            };
        }

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

        /// <summary>
        /// Gets a read-only collection containing the elements of the specified sequence.
        /// If the specified sequence is null, an empty collection is returned.
        /// </summary>
        /// <typeparam name="T">Type of the elements in the sequence.</typeparam>
        /// <param name="enumerable">Sequence to get a read-only collection for.</param>
        /// <returns>Read-only collection containing the elements of the specified sequence.</returns>
        public static ReadOnlyCollection<T> ToReadOnly<T>(this IEnumerable<T> enumerable)
        {
            switch (enumerable)
            {
                case null:
                    return EmptyReadOnlyCollection<T>.Instance;
                case TrueReadOnlyCollection<T> readOnly:
                    return readOnly;
                case ICollection<T> collection:
                    {
                        var n = collection.Count;
                        if (n == 0)
                        {
                            return EmptyReadOnlyCollection<T>.Instance;
                        }
                        else
                        {
                            var res = new T[n];
                            collection.CopyTo(res, 0);
                            return new TrueReadOnlyCollection<T>(res);
                        }
                    }
                default:
                    return new TrueReadOnlyCollection<T>(enumerable.ToArray());
            }
        }

        /// <summary>
        /// Converts the specified sequence to an IList&lt;T&gt;.
        /// </summary>
        /// <typeparam name="T">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Sequence to convert to a list.</param>
        /// <returns>The source sequence object if it implements IList&lt;T&gt;, otherwise a copy of the source sequence into a list.</returns>
        /// <remarks>Notice this method does not guarantee that a copy of the source sequence will be made; therefore, aliasing can occur and the ownership of the source sequence needs to be approved for transfer.</remarks>
        internal static IList<T> ToIListUnsafe<T>(this IEnumerable<T> source) => source is IList<T> list ? list : new List<T>(source);

        /// <summary>
        /// Converts the specified sequence to an IReadOnlyList&lt;T&gt;.
        /// </summary>
        /// <typeparam name="T">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Sequence to convert to a list.</param>
        /// <returns>The source sequence object if it implements IReadOnlyList&lt;T&gt;, otherwise a copy of the source sequence into a list.</returns>
        /// <remarks>Notice this method does not guarantee that a copy of the source sequence will be made; therefore, aliasing can occur and the ownership of the source sequence needs to be approved for transfer.</remarks>
        internal static IReadOnlyList<T> ToIReadOnlyListUnsafe<T>(this IEnumerable<T> source) => source is IReadOnlyList<T> list ? list : new List<T>(source);

        /// <summary>
        /// Converts the specified sequence to a ReadOnlyCollection&lt;T&gt;.
        /// </summary>
        /// <typeparam name="T">Type of the elements in the source sequence.</typeparam>
        /// <param name="source">Sequence to convert to a read-only collection.</param>
        /// <returns>A read-only collection instance providing access to the elements in the source sequence.</returns>
        /// <remarks>Notice this method does not guarantee that a copy of the source sequence will be made; therefore, aliasing can occur and the ownership of the source sequence needs to be approved for transfer.</remarks>
        internal static ReadOnlyCollection<T> ToReadOnlyUnsafe<T>(this IEnumerable<T> source)
        {
            if (source is ReadOnlyCollection<T> res)
            {
                return res;
            }

            if (source is not IList<T> list)
            {
                list = new List<T>(source);
            }

            return list.Count == 0 ? EmptyReadOnlyCollection<T>.Instance : new ReadOnlyCollection<T>(list);
        }

        /// <summary>
        /// Gets an array with the elements from the source. If the source is backed by an array, it doesn't create a copy.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the source.</typeparam>
        /// <param name="source">The source collection to get an elements array from.</param>
        /// <returns>An array containing the elements from the source.</returns>
        internal static T[] ToArrayUnsafe<T>(this ReadOnlyCollection<T> source) => source is TrueReadOnlyCollection<T> troc && troc.List is T[] array ? array : Enumerable.ToArray(source);
    }

    /// <summary>
    /// Read-only collection whose inner collection is unreachable, so it cannot be mutated.
    /// Upon creating an instance of this type, the ownership of the specified collection is transferred.
    /// </summary>
    /// <typeparam name="T">Type of the elements in the collection.</typeparam>
    internal class TrueReadOnlyCollection<T> : ReadOnlyCollection<T>
    {
        /// <summary>
        /// Creates a new read-only wrapper around the specified inner collection.
        /// </summary>
        /// <param name="list">Inner collection to wrap in a read-only container. Ownership of this collection should be transferred upon calling this constructor.</param>
        public TrueReadOnlyCollection(IList<T> list)
            : base(list)
        {
        }

        /// <summary>
        /// Gets the underlying list.
        /// </summary>
        internal IList<T> List => Items;
    }

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1711 // Do not use Collection suffix. (Analogous to System.Dynamic.Utils.)

    /// <summary>
    /// Factory for empty ReadOnlyCollection objects.
    /// </summary>
    /// <typeparam name="T">Element type.</typeparam>
    public static class EmptyReadOnlyCollection<T>
    {
#pragma warning disable CA1000 // Do not declare static members on generic types.

        /// <summary>
        /// Singleton instance of an empty ReadOnlyCollection with the specified element type.
        /// </summary>
        public static ReadOnlyCollection<T> Instance { get; } = new TrueReadOnlyCollection<T>(Array.Empty<T>());

#pragma warning restore CA1000
    }

#pragma warning restore CA1711
#pragma warning restore IDE0079
}
