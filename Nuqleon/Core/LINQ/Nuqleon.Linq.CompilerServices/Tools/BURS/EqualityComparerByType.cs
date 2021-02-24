// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2011 - Created this file.
//

using System.Collections.Generic;

namespace System.Linq.CompilerServices
{
    internal sealed class EqualityComparerByType<TSource> : IEqualityComparer<TSource>
    {
        public bool Equals(TSource x, TSource y)
        {
            var res = false;

            if (x is ITyped xTyped && y is ITyped yTyped)
            {
                var xType = xTyped.GetType();
                var yType = yTyped.GetType();
                res = EqualityComparer<IType>.Default.Equals(xType, yType);
            }

            return res;
        }

        public int GetHashCode(TSource obj)
        {
            if (obj is ITyped typed)
            {
                var type = typed.GetType();
                return EqualityComparer<IType>.Default.GetHashCode(type);
            }

            return EqualityComparer<object>.Default.GetHashCode(obj);
        }
    }
}
