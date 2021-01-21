// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 07/02/2014 - Set up test project.
//

using System.Collections.Generic;
using System.Linq;

namespace Tests
{
    internal static class TestHelpers
    {
        public static IEnumerable<T> Trim<T>(this IEnumerable<T> xs)
        {
            return xs
#if DEBUG
                .Take(1) // can be used to speed up tests during development
#endif
                ;
        }
    }
}
