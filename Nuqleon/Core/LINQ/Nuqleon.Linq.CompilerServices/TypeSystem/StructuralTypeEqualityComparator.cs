// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - October 2013 - Created this file.
//

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1062 // Omitted null checks similar to expression tree visitors.

using System.Collections.Generic;
using System.Reflection;

namespace System.Linq.CompilerServices
{
    /// <summary>
    /// Equality comparer for structural types.
    /// </summary>
    public class StructuralTypeEqualityComparator : TypeEqualityComparer
    {
        // TODO: Implement GetHashCode.

        /// <summary>
        /// Instantiates an equality comparer for structural types, using the default structural member info comparer.
        /// </summary>
        public StructuralTypeEqualityComparator()
        {
            TypeMap = new Dictionary<Type, Type>();
            MemberComparer = new StructuralMemberInfoEqualityComparator(this);
        }

        /// <summary>
        /// Instantiates an equality comparer for structural types, using the given structural member info comparer.
        /// </summary>
        /// <param name="memberComparer">The member comparer.</param>
        public StructuralTypeEqualityComparator(MemberInfoEqualityComparer memberComparer)
        {
            TypeMap = new Dictionary<Type, Type>();
            MemberComparer = memberComparer;
        }

        /// <summary>
        /// Update the equality comparer used to compare type members.
        /// </summary>
        public MemberInfoEqualityComparer MemberComparer { get; }

        /// <summary>
        /// The map of types determined to be equal.
        /// </summary>
        protected IDictionary<Type, Type> TypeMap { get; }

        /// <summary>
        /// Checks whether two given types are equal.
        /// </summary>
        /// <param name="x">The left type.</param>
        /// <param name="y">The right type.</param>
        /// <returns>true if the given types are equal, false otherwise.</returns>
        public override bool Equals(Type x, Type y)
        {
            if (x == null && y == null)
            {
                return true;
            }

            if (x == null || y == null)
            {
                return false;
            }

            if (AreStructurallyComparable(x, y))
            {
                return EqualsStructural(x, y);
            }

            return base.Equals(x, y);
        }

        /// <summary>
        /// Checks whether two given structural types are equal.
        /// </summary>
        /// <param name="x">The left type.</param>
        /// <param name="y">The right type.</param>
        /// <returns>true if the given types are equal, false otherwise.</returns>
        protected virtual bool EqualsStructural(Type x, Type y)
        {
            if (TypeMap.TryGetValue(x, out Type res))
            {
                return res.Equals(y);
            }
            else
            {
                TypeMap[x] = y;
            }

            var xMembers = x.GetMembers();
            var yMembers = y.GetMembers();

            if (xMembers.Length != yMembers.Length)
            {
                TypeMap.Remove(x);
                return false;
            }

            foreach (var xMember in xMembers)
            {
                if (MemberComparer.ResolveMember(y, xMember) == null)
                {
                    TypeMap.Remove(x);
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Checks if two given types can be compared as structural types.
        /// </summary>
        /// <param name="x">The left type.</param>
        /// <param name="y">The right type.</param>
        /// <returns>true if the given types are structurally comparable, false otherwise.</returns>
        protected virtual bool AreStructurallyComparable(Type x, Type y) => (x.IsRecordType() && y.IsRecordType()) || (x.IsAnonymousType() && y.IsAnonymousType());
    }
}
