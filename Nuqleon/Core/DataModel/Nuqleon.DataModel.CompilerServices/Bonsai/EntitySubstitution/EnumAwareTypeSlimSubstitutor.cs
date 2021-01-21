// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD, ER - November 2013 - Created this file.
//

using System.Collections.Generic;
using System.Reflection;

namespace Nuqleon.DataModel.CompilerServices.Bonsai
{
    internal sealed class EnumAwareTypeSlimSubstitutor : TypeSlimVisitor
    {
        private readonly IDictionary<TypeSlim, TypeSlim> _typeMap;

        public EnumAwareTypeSlimSubstitutor(IDictionary<TypeSlim, TypeSlim> typeMap) => _typeMap = typeMap;

        public TypeSlim Rewrite(TypeSlim type) => Visit(type);

        public override TypeSlim Visit(TypeSlim type)
        {
            if (type == null)
            {
                return null;
            }

            if (_typeMap.TryGetValue(type, out var res))
            {
                return res;
            }

            return base.Visit(type);
        }

        protected override TypeSlim VisitSimple(SimpleTypeSlim type)
        {
            if (_typeMap.TryGetValue(type, out var res) && IsValueType(res))
            {
                return res;
            }

            return base.VisitSimple(type);
        }

        private static bool IsValueType(TypeSlim type)
        {
            if (type.TryGetCarriedType(out var carriedType))
            {
                return carriedType.IsValueType;
            }

            return false;
        }
    }
}
