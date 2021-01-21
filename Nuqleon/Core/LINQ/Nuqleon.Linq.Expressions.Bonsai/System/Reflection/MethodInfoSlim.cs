// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
// ER - October 2013 - Created this file.
//

using System.Collections.ObjectModel;

namespace System.Reflection
{
    /// <summary>
    /// Lightweight representation of a method.
    /// </summary>
    public abstract class MethodInfoSlim : MemberInfoSlim
    {
        /// <summary>
        /// Creates a new method representation object.
        /// </summary>
        /// <param name="declaringType">Type declaring the method.</param>
        protected MethodInfoSlim(TypeSlim declaringType)
            : base(declaringType)
        {
        }

        /// <summary>
        /// Gets the member type of the member.
        /// </summary>
        public override MemberTypes MemberType => MemberTypes.Method;

        /// <summary>
        /// Gets the kind of the method.
        /// </summary>
        public abstract MethodInfoSlimKind Kind { get; }

        /// <summary>
        /// Checks if the method is generic.
        /// </summary>
        public abstract bool IsGenericMethod { get; }

        /// <summary>
        /// Gets the parameters types of the method.
        /// </summary>
        public abstract ReadOnlyCollection<TypeSlim> ParameterTypes { get; }

        /// <summary>
        /// Gets the return type of the method.
        /// </summary>
        public abstract TypeSlim ReturnType { get; }
    }
}
