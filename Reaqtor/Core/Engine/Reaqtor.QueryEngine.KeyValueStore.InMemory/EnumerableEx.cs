// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

namespace System.Linq
{
    internal static class EnumerableEx
    {
        public static IEnumerable<T[]> Buffer<T>(this IEnumerable<T> source, int count)
        {
            var cur = new List<T>();

            var i = 0;

            foreach (var item in source)
            {
                cur.Add(item);

                if (++i == count)
                {
                    yield return cur.ToArray();
                    cur.Clear();
                    i = 0;
                }
            }

            yield return cur.ToArray();
        }
    }
}
