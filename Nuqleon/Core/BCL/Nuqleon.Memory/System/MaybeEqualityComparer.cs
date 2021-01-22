// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 08/03/2015 - Added this type to make dictionaries with null entries easier to build.
//

using System.Collections.Generic;

namespace System
{
    internal class MaybeEqualityComparer<K> : IEqualityComparer<Maybe<K>>
    {
        private readonly IEqualityComparer<K> _comparer;

        public MaybeEqualityComparer(IEqualityComparer<K> comparer) => _comparer = comparer;

        public bool Equals(Maybe<K> x, Maybe<K> y) => _comparer.Equals(x.Value, y.Value);

        public int GetHashCode(Maybe<K> obj) => _comparer.GetHashCode(obj.Value);
    }
}
