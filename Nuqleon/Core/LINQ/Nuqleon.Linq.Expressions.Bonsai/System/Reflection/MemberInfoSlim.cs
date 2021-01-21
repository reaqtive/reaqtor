// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
//

using System.Diagnostics;

namespace System.Reflection
{
    /// <summary>
    /// Lightweight representation of a type member.
    /// </summary>
    public abstract class MemberInfoSlim : IEquatable<MemberInfoSlim>
    {
        private int _hashCode;

        /// <summary>
        /// Creates a new member of the specified member type.
        /// </summary>
        /// <param name="declaringType">Type declaring the member.</param>
        protected MemberInfoSlim(TypeSlim declaringType)
        {
            Debug.Assert(declaringType != null);

            DeclaringType = declaringType;
        }

        /// <summary>
        /// Gets the member type of the member.
        /// </summary>
        public abstract MemberTypes MemberType { get; }

        /// <summary>
        /// Gets the type declaring the member.
        /// </summary>
        public TypeSlim DeclaringType { get; }

        /// <summary>
        /// The equals method for comparing against other class instances.
        /// </summary>
        /// <param name="other">The other instance.</param>
        /// <returns><b>true</b> if the given instance equals this instance, <b>false</b> otherwise.</returns>
        public bool Equals(MemberInfoSlim other)
        {
            return MemberInfoSlimEqualityComparer.Default.Equals(this, other);
        }

        /// <summary>
        /// Returns a friendly string representation of the member.
        /// </summary>
        /// <returns>Friendly string representation of the member.</returns>
        public override string ToString()
        {
            return new MemberInfoSlimPrettyPrinter().Visit(this);
        }

        /// <summary>
        /// Returns a hash code for this object.
        /// </summary>
        /// <returns>The hash code.</returns>
        public override int GetHashCode()
        {
            if (_hashCode == 0)
            {
                _hashCode = MemberInfoSlimEqualityComparer.Default.GetHashCode(this);
            }

            return _hashCode;
        }

        /// <summary>
        /// The default equals method.
        /// </summary>
        /// <param name="obj">The object to compare against.</param>
        /// <returns><b>true</b> if the given object equals this object, <b>false</b> otherwise.</returns>
        public override bool Equals(object obj) => obj is MemberInfoSlim other && Equals(other);
    }
}
