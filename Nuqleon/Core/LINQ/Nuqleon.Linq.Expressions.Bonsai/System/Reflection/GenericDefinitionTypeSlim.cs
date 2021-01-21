// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
// ER - July 2013 - Small tweaks.
//

namespace System.Reflection
{
    /// <summary>
    /// Lightweight representation of an open generic type.
    /// </summary>
    public sealed class GenericDefinitionTypeSlim : SimpleTypeSlimBase
    {
        // CONSIDER: In this version, the representation of an open generic type is
        //           is basically equivalent to the representation of a simple type.
        //
        //           In the future, we should consider adding properties for the
        //           generic parameters to represent generic parameter attributes,
        //           such as covariance and contravariance.

        /// <summary>
        /// Creates a new open generic type definition representation object.
        /// </summary>
        /// <param name="assembly">Assembly defining the type.</param>
        /// <param name="name">Name of the type.</param>
        internal GenericDefinitionTypeSlim(AssemblySlim assembly, string name)
            : base(assembly, name)
        {
        }

        /// <summary>
        /// Gets the kind of the type.
        /// </summary>
        public sealed override TypeSlimKind Kind => TypeSlimKind.GenericDefinition;
    }

    public partial class TypeSlim
    {
        /// <summary>
        /// Creates a new lightweight representation of an open generic type.
        /// </summary>
        /// <param name="assembly">Assembly defining the type.</param>
        /// <param name="name">Name of the type.</param>
        /// <returns>A new lightweight representation of an open generic type.</returns>
        public static GenericDefinitionTypeSlim GenericDefinition(AssemblySlim assembly, string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Type name can not be null or empty.", nameof(name));

            return new GenericDefinitionTypeSlim(assembly, name);
        }
    }
}
