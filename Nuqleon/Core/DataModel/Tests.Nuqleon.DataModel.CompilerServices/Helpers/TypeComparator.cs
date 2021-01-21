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
using System.Linq.CompilerServices;
using System.Reflection;

using Nuqleon.DataModel.TypeSystem;

namespace Tests.Nuqleon.DataModel.CompilerServices
{
    internal class TypeComparator : StructuralTypeEqualityComparator
    {
        private readonly IDictionary<Type, Type> _structuralMap = new Dictionary<Type, Type>();

        public TypeComparator()
            : this(new MemberComparator())
        {
        }

        public TypeComparator(StructuralMemberInfoEqualityComparator memberComparator)
            : base(memberComparator)
        {
            memberComparator.TypeComparer = this;
        }

        protected override bool EqualsSimple(Type x, Type y)
        {
            var result = base.EqualsSimple(x, y);

            if (!result)
            {
                if (x.IsEnum && !y.IsEnum)
                {
                    return Equals(x.GetEnumUnderlyingType(), y);
                }
                else if (!x.IsEnum && y.IsEnum)
                {
                    return Equals(x, y.GetEnumUnderlyingType());
                }
            }

            return result;
        }

        protected override bool EqualsStructural(Type x, Type y)
        {
            if (_structuralMap.TryGetValue(x, out var mappedY))
            {
                return y == mappedY;
            }
            else
            {
                _structuralMap.Add(x, y);
            }

            try
            {
                var xDataType = (StructuralDataType)DataType.FromType(x, allowCycles: true);
                var yDataType = (StructuralDataType)DataType.FromType(y, allowCycles: true);

                if (xDataType.Properties.Count != yDataType.Properties.Count)
                {
                    return false;
                }

                var xProperties = new Dictionary<string, DataProperty>(xDataType.Properties.Count);
                for (var i = 0; i < xDataType.Properties.Count; ++i)
                {
                    var property = xDataType.Properties[i];
                    xProperties[property.Name] = property;
                }

                for (var i = 0; i < yDataType.Properties.Count; ++i)
                {
                    var yProperty = yDataType.Properties[i];
                    if (!xProperties.TryGetValue(yProperty.Name, out var xProperty) || !MemberComparer.Equals(xProperty.Property, yProperty.Property))
                    {
                        return false;
                    }
                }

                return true;
            }
            finally
            {
                _structuralMap.Remove(x);
            }
        }

        protected override bool AreStructurallyComparable(Type x, Type y)
        {
            if (!DataType.IsStructuralEntityDataType(x) && (!DataType.TryFromType(x, allowCycles: true, out var dataType) || !(dataType is StructuralDataType)))
            {
                return false;
            }

            return DataType.IsStructuralEntityDataType(y) || (DataType.TryFromType(y, allowCycles: true, out dataType) && dataType is StructuralDataType);
        }
    }
}
