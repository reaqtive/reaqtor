// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Tests.Reaqtor.Remoting.Glitching
{
    internal static class TestHelpers
    {
        public static IEnumerable<IEnumerable<T>> Powerset<T>(this IEnumerable<T> set)
        {
            if (set == null)
            {
                throw new ArgumentNullException(nameof(set));
            }

            return set.ToList().Powerset(0);
        }

        private static IEnumerable<IEnumerable<T>> Powerset<T>(this List<T> set, int start)
        {
            if (start == set.Count)
            {
                return new List<IEnumerable<T>> { Array.Empty<T>() };
            }
            else
            {
                var first = set[start];
                var subPowerset = set.Powerset(start + 1);
                return subPowerset.Concat(subPowerset.Select(ss => new[] { first }.Concat(ss)));
            }
        }

        public static IEnumerable<IEnumerable<T>> Choose<T>(this IEnumerable<T> source, int k)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            var list = source.ToList();
            if (k > list.Count || k < 0)
                throw new ArgumentOutOfRangeException(nameof(k));

            if (k == 0)
            {
                return new List<IEnumerable<T>> { Array.Empty<T>() };
            }

            var first = list[0];
            var result = Choose(list.Skip(1), k - 1).Select(l => l.Prepend(first));
            if (list.Count > k)
            {
                result = result.Concat(Choose(list.Skip(1), k));
            }
            return result;
        }

        private static IEnumerable<T> Prepend<T>(this IEnumerable<T> source, T item)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            yield return item;

            foreach (var element in source)
                yield return element;
        }
    }
}
