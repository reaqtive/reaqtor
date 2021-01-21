// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq.CompilerServices;
using System.Linq.Expressions;

namespace Reaqtor.QueryEngine
{
    /// <summary>
    /// Equality comparer to look up definition entities by their value (i.e. the underlying expression tree).
    /// </summary>
    internal sealed class InvertedDefinitionEntityComparer : IEqualityComparer<DefinitionEntity>
    {
        private const int Prime = 17;

        private readonly ExpressionEqualityComparer _comparer = new(() => new Comparator());

        private InvertedDefinitionEntityComparer() { }

        public static InvertedDefinitionEntityComparer Default { get; } = new InvertedDefinitionEntityComparer();

        public bool Equals(DefinitionEntity x, DefinitionEntity y)
        {
            if (ReferenceEquals(x, y))
            {
                return true;
            }
            else if (x == null || y == null)
            {
                return false;
            }

            return _comparer.Equals(x.Expression, y.Expression);
        }

        public int GetHashCode(DefinitionEntity obj)
        {
            return obj != null ? _comparer.GetHashCode(obj.Expression) : Prime;
        }

        private sealed class Comparator : ExpressionEqualityComparator
        {
            protected override bool EqualsGlobalParameter(ParameterExpression x, ParameterExpression y)
            {
                return x.Name == y.Name && Equals(x.Type, y.Type);
            }

            protected override int GetHashCodeGlobalParameter(ParameterExpression obj)
            {
                var hash = obj.Name != null
#if NET5_0
                    ? obj.Name.GetHashCode(System.StringComparison.Ordinal)
#else
                    ? obj.Name.GetHashCode()
#endif
                    : Prime;

                return hash * Prime + GetHashCode(obj.Type);
            }
        }
    }

}
