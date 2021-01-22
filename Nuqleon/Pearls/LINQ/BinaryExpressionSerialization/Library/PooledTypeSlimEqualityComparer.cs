// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - December 2014 - Created this file.
//

#if USE_SLIM

using System.Collections.Generic;
using System.Memory;
using System.Reflection;

namespace System.Linq.Expressions.Bonsai.Serialization.Binary
{
    internal sealed class PooledTypeSlimEqualityComparer : IEqualityComparer<TypeSlim>
    {
        private readonly ObjectPool<TypeSlimEqualityComparator> _comparatorPool = new(() => new TypeSlimEqualityComparator());

        private PooledTypeSlimEqualityComparer()
        {
        }

        public static PooledTypeSlimEqualityComparer Instance { get; } = new PooledTypeSlimEqualityComparer();

        public bool Equals(TypeSlim x, TypeSlim y)
        {
            using var o = _comparatorPool.New();

            return o.Object.Equals(x, y);
        }

        public int GetHashCode(TypeSlim obj)
        {
            using var o = _comparatorPool.New();

            return o.Object.GetHashCode(obj);
        }
    }
}

#endif
