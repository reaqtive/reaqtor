// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - July 2013 - Created this file.
//

using System.Collections.Generic;

namespace System.Linq
{
    internal static class EnumerableEx
    {
        public static T[] AsArray<T>(this IEnumerable<T> source) => source is T[] res ? res : source.ToArray();
    }
}
