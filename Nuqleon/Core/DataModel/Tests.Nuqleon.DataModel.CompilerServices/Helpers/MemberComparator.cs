// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER, BD - July 2013 - Created this file.
//

using System;
using System.Collections.Generic;
using System.Reflection;

using Nuqleon.DataModel;

namespace Tests.Nuqleon.DataModel.CompilerServices
{
    internal class MemberComparator : StructuralMemberInfoEqualityComparator
    {
        public MemberComparator() { }

        public MemberComparator(IEqualityComparer<Type> typeComparer)
            : base(typeComparer)
        {
        }

        public override bool Equals(MemberInfo x, MemberInfo y)
        {
            if (x == null && y == null)
            {
                return true;
            }
            if (x == null || y == null)
            {
                return false;
            }

            if (x.MemberType != y.MemberType)
            {
                if (x.MemberType == MemberTypes.Property && y.MemberType == MemberTypes.Field)
                {
                    return EqualsPropertyToField((PropertyInfo)x, (FieldInfo)y);
                }
                if (y.MemberType == MemberTypes.Property && x.MemberType == MemberTypes.Field)
                {
                    return EqualsPropertyToField((PropertyInfo)y, (FieldInfo)x);
                }
            }

            return base.Equals(x, y);
        }

        protected override bool EqualsProperty(PropertyInfo x, PropertyInfo y)
        {
            if (!base.EqualsProperty(x, y))
            {
                var xMapping = x.GetCustomAttribute<MappingAttribute>(inherit: false);
                var yMapping = y.GetCustomAttribute<MappingAttribute>(inherit: false);

                var xName = xMapping != null ? xMapping.Uri : x.Name;
                var yName = yMapping != null ? yMapping.Uri : y.Name;

                return xName == yName && TypeComparer.Equals(x.PropertyType, y.PropertyType);
            }

            return true;
        }

        private bool EqualsPropertyToField(PropertyInfo x, FieldInfo y)
        {
            var xMapping = x.GetCustomAttribute<MappingAttribute>(inherit: false);
            var yMapping = y.GetCustomAttribute<MappingAttribute>(inherit: false);

            var xName = xMapping != null ? xMapping.Uri : x.Name;
            var yName = yMapping != null ? yMapping.Uri : y.Name;

            return xName == yName && TypeComparer.Equals(x.PropertyType, y.FieldType) && x.GetIndexParameters().Length == 0;
        }
    }
}
