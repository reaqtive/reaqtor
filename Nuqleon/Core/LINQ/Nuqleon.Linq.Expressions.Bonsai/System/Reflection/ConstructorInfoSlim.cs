// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
//

using System.Collections.ObjectModel;
using System.Diagnostics;

namespace System.Reflection
{
    /// <summary>
    /// Lightweight representation of a constructor.
    /// </summary>
    public class ConstructorInfoSlim : MemberInfoSlim
    {
        /// <summary>
        /// Creates a new constructor representation object.
        /// </summary>
        /// <param name="declaringType">Type declaring the member.</param>
        /// <param name="parameterTypes">Types of the parameters.</param>
        protected internal ConstructorInfoSlim(TypeSlim declaringType, ReadOnlyCollection<TypeSlim> parameterTypes)
            : base(declaringType)
        {
            Debug.Assert(parameterTypes != null);

            ParameterTypes = parameterTypes;
        }

        /// <summary>
        /// Gets the member type of the member.
        /// </summary>
        public sealed override MemberTypes MemberType => MemberTypes.Constructor;

        /// <summary>
        /// Gets the types of the parameters.
        /// </summary>
        public ReadOnlyCollection<TypeSlim> ParameterTypes { get; }
    }
}
