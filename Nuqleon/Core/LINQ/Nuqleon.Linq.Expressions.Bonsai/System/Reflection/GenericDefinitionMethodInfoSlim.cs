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
    /// Lightweight representation of an open generic method definition.
    /// </summary>
    public sealed class GenericDefinitionMethodInfoSlim : SimpleMethodInfoSlimBase
    {
        /// <summary>
        /// Creates a new closed generic method representation object.
        /// </summary>
        /// <param name="declaringType">Type declaring the method.</param>
        /// <param name="name">Name of the method.</param>
        /// <param name="genericParameterTypes">Generic parameter types.</param>
        /// <param name="parameterTypes">Type of the method parameters.</param>
        /// <param name="returnType">Return type of the method.</param>
        internal GenericDefinitionMethodInfoSlim(TypeSlim declaringType, string name, ReadOnlyCollection<TypeSlim> genericParameterTypes, ReadOnlyCollection<TypeSlim> parameterTypes, TypeSlim returnType)
            : base(declaringType, name, parameterTypes, returnType)
        {
            Debug.Assert(genericParameterTypes != null);
            Debug.Assert(genericParameterTypes.Count >= 1);

            GenericParameterTypes = genericParameterTypes;
        }

        /// <summary>
        /// Gets the kind of the method.
        /// </summary>
        public sealed override MethodInfoSlimKind Kind => MethodInfoSlimKind.GenericDefinition;

        /// <summary>
        /// Checks if the method is generic.
        /// </summary>
        public sealed override bool IsGenericMethod => true;

        /// <summary>
        /// Gets the generic parameter types.
        /// </summary>
        public ReadOnlyCollection<TypeSlim> GenericParameterTypes { get; }
    }
}
