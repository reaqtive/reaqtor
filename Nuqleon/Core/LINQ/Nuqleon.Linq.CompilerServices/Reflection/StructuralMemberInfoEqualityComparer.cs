// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - September 2013 - Created this file.
//

using System.Collections.Generic;

namespace System.Reflection
{
    /// <summary>
    /// MemberInfo comparer for structural types.
    /// </summary>
    /// <remarks>
    /// Instantiates a MemberInfo comparer for structural type members, using the given comparator factory.
    /// </remarks>
    /// <param name="comparatorFactory">Factory to produce comparator instances for performing equals and hash code operations.</param>
    public class StructuralMemberInfoEqualityComparer(Func<StructuralMemberInfoEqualityComparator> comparatorFactory) : IEqualityComparer<MemberInfo>
    {
        private readonly Func<StructuralMemberInfoEqualityComparator> _comparatorFactory = comparatorFactory;

        /// <summary>
        /// Instantiates a MemberInfo comparer for structural type members, using a default comparator factory.
        /// </summary>
        public StructuralMemberInfoEqualityComparer()
            : this(() => new StructuralMemberInfoEqualityComparator())
        {
        }

        /// <summary>
        /// A default instance of the equality comparer.
        /// </summary>
        public static StructuralMemberInfoEqualityComparer Default
        {
            get
            {
                field ??= new StructuralMemberInfoEqualityComparer();
                return field;
            }
        }

        /// <summary>
        /// Checks whether two given members are equal.
        /// </summary>
        /// <param name="x">The left member.</param>
        /// <param name="y">The right member.</param>
        /// <returns>true if the given members are equal, false otherwise.</returns>
        public bool Equals(MemberInfo x, MemberInfo y) => _comparatorFactory().Equals(x, y);

        /// <summary>
        /// Returns a hash code for the specified member.
        /// </summary>
        /// <param name="obj">The member for which a hash code is to be returned.</param>
        /// <returns>The hash code of the member.</returns>
        public int GetHashCode(MemberInfo obj) => _comparatorFactory().GetHashCode(obj);
    }
}
