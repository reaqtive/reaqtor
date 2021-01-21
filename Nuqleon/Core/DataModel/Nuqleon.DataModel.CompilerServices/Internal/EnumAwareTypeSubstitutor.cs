// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - July 2013 - Created this file.
//

using System;
using System.Collections.Generic;
using System.Linq.CompilerServices;

namespace Nuqleon.DataModel.CompilerServices
{
    internal sealed class EnumAwareTypeSubstitutor : TypeVisitor
    {
        private readonly IDictionary<Type, Type> _typeMap;

        public EnumAwareTypeSubstitutor(IDictionary<Type, Type> typeMap) => _typeMap = typeMap;

        public Type Rewrite(Type type) => Visit(type);

        public override Type Visit(Type type)
        {
            if (_typeMap.TryGetValue(type, out var res))
            {
                return res;
            }

            return base.Visit(type);
        }

        protected override Type VisitGenericClosed(Type type)
        {
            var valueType = Nullable.GetUnderlyingType(type);

            if (valueType != null)
            {
                if (_typeMap.TryGetValue(valueType, out var res) && !res.IsValueType)
                {
                    return res;
                }
            }

            return base.VisitGenericClosed(type);
        }
    }
}
