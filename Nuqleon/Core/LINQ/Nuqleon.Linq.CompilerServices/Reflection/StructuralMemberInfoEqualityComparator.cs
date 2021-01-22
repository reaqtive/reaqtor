// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - September 2013 - Created this file.
//

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1062 // Omitted null checks similar to expression tree visitors.

using System.Collections.Generic;
using System.Linq.CompilerServices;

namespace System.Reflection
{
    /// <summary>
    /// MemberInfo comparer for structural types.
    /// </summary>
    public class StructuralMemberInfoEqualityComparator : MemberInfoEqualityComparer
    {
        /// <summary>
        /// Instantiates a MemberInfo comparer for structural type members, using the default structural type comparer.
        /// </summary>
        public StructuralMemberInfoEqualityComparator() => TypeComparer = new StructuralTypeEqualityComparator(this);

        /// <summary>
        /// Instantiates a MemberInfo comparer for structural type members, using the given type comparer.
        /// </summary>
        /// <param name="typeComparer"></param>
        public StructuralMemberInfoEqualityComparator(IEqualityComparer<Type> typeComparer) => TypeComparer = typeComparer;

        /// <summary>
        /// The type comparer to use when testing member return types, etc.
        /// </summary>
        public IEqualityComparer<Type> TypeComparer
        {
            get;
            set;
        }

        /// <summary>
        /// Checks for structural equality of constructors.
        /// </summary>
        /// <param name="x">The left constructor.</param>
        /// <param name="y">The right constructor.</param>
        /// <returns>true if the constructors are structurally equivalent, false otherwise.</returns>
        protected override bool EqualsConstructor(ConstructorInfo x, ConstructorInfo y) => EqualsParameters(x.GetParameters(), y.GetParameters());

        /// <summary>
        /// Checks for structural equality of fields.
        /// </summary>
        /// <param name="x">The left field.</param>
        /// <param name="y">The right field.</param>
        /// <returns>true if the fields are structurally equivalent, false otherwise.</returns>
        protected override bool EqualsField(FieldInfo x, FieldInfo y) => x.Name == y.Name && TypeComparer.Equals(x.FieldType, y.FieldType);

        /// <summary>
        /// Checks for structural equality of methods.
        /// </summary>
        /// <param name="x">The left method.</param>
        /// <param name="y">The right method.</param>
        /// <returns>true if the methods are structurally equivalent, false otherwise.</returns>
        protected override bool EqualsMethod(MethodInfo x, MethodInfo y) => x.Name == y.Name && TypeComparer.Equals(x.ReturnType, y.ReturnType) && EqualsParameters(x.GetParameters(), y.GetParameters());

        /// <summary>
        /// Checks for structural equality of properties.
        /// </summary>
        /// <param name="x">The left property.</param>
        /// <param name="y">The right property.</param>
        /// <returns>true if the properties are structurally equivalent, false otherwise.</returns>
        protected override bool EqualsProperty(PropertyInfo x, PropertyInfo y) => x.Name == y.Name && TypeComparer.Equals(x.PropertyType, y.PropertyType) && EqualsParameters(x.GetIndexParameters(), y.GetIndexParameters());

        private bool EqualsParameters(ParameterInfo[] x, ParameterInfo[] y)
        {
            if (x.Length != y.Length)
            {
                return false;
            }

            for (var i = 0; i < x.Length; ++i)
            {
                if (x[i].Position != y[i].Position || !TypeComparer.Equals(x[i].ParameterType, y[i].ParameterType))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
