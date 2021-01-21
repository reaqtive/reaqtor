// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
//

using System.Collections.ObjectModel;

namespace System.Reflection
{
    /// <summary>
    /// Lightweight representation of a simple method, i.e. not generic.
    /// </summary>
    public class SimpleMethodInfoSlim : SimpleMethodInfoSlimBase
    {
        /// <summary>
        /// Creates a new simple method representation object.
        /// </summary>
        /// <param name="declaringType">Type declaring the method.</param>
        /// <param name="name">Name of the method.</param>
        /// <param name="parameterTypes">Type of the method parameters.</param>
        /// <param name="returnType">Return type of the method.</param>
        internal SimpleMethodInfoSlim(TypeSlim declaringType, string name, ReadOnlyCollection<TypeSlim> parameterTypes, TypeSlim returnType)
            : base(declaringType, name, parameterTypes, returnType)
        {
        }

        /// <summary>
        /// Gets the kind of the method.
        /// </summary>
        public override MethodInfoSlimKind Kind => MethodInfoSlimKind.Simple;

        /// <summary>
        /// Checks if the method is generic.
        /// </summary>
        public override bool IsGenericMethod => false;
    }
}
