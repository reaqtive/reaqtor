// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;

namespace Reaqtor.Remoting.Deployable.Streams
{
    internal static class ReadOnlyListExtensions
    {
        public static IReadOnlyList<T> Sublist<T>(this IReadOnlyList<T> list, int offset, int count)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));
            if (offset < 0)
                throw new ArgumentOutOfRangeException(nameof(offset));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            if (list.Count - offset < count)
                throw new ArgumentException("There are not enough elements starting from the given offset.");

            if (list is ReadOnlyListSegment<T> segment)
            {
                return new ReadOnlyListSegment<T>(segment.List, segment.Offset + offset, count);
            }

            return new ReadOnlyListSegment<T>(list, offset, count);
        }

        public static IReadOnlyList<T> Sublist<T>(this IReadOnlyList<T> list, int offset)
        {
            return Sublist(list, offset, list.Count - offset);
        }

        public readonly struct ReadOnlyListSegment<T> : IReadOnlyList<T>, IEquatable<ReadOnlyListSegment<T>>
        {
            public ReadOnlyListSegment(IReadOnlyList<T> list)
            {
                List = list ?? throw new ArgumentNullException(nameof(list));
                Offset = 0;
                Count = list.Count;
            }

            public ReadOnlyListSegment(IReadOnlyList<T> list, int offset, int count)
            {
                if (list == null)
                    throw new ArgumentNullException(nameof(list));
                if (offset < 0)
                    throw new ArgumentOutOfRangeException(nameof(offset));
                if (count < 0)
                    throw new ArgumentOutOfRangeException(nameof(count));
                if (list.Count - offset < count)
                    throw new ArgumentException("There are not enough elements starting from the given offset.");

                List = list;
                Offset = offset;
                Count = count;
            }

            public IReadOnlyList<T> List { get; }

            public int Offset { get; }

            public int Count { get; }

            public override int GetHashCode() => null == List ? 0 : List.GetHashCode() ^ Offset ^ Count;

            public override bool Equals(object obj) => obj is ReadOnlyListSegment<T> segment && Equals(segment);

            public bool Equals(ReadOnlyListSegment<T> obj) => obj.List == List && obj.Offset == Offset && obj.Count == Count;

            public static bool operator ==(ReadOnlyListSegment<T> a, ReadOnlyListSegment<T> b) => a.Equals(b);

            public static bool operator !=(ReadOnlyListSegment<T> a, ReadOnlyListSegment<T> b) => !(a == b);

            public T this[int index]
            {
                get
                {
                    if (index < 0 || index >= Count)
                        throw new ArgumentOutOfRangeException(nameof(index));

                    return List[Offset + index];
                }
            }

            public IEnumerator<T> GetEnumerator()
            {
                var end = Offset + Count;
                for (var i = Offset; i < end; i++)
                {
                    yield return List[i];
                }
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
        }
    }
}
