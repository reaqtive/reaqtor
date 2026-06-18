// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
//

namespace System.Reflection
{
    /// <summary>
    /// Lightweight representation of a simple type, i.e. not constructed from other types.
    /// </summary>
    /// <remarks>
    /// Creates a new generic type definition representation object.
    /// </remarks>
    /// <param name="assembly">Assembly defining the type.</param>
    /// <param name="name">Name of the type.</param>
    public sealed class SimpleTypeSlim(AssemblySlim assembly, string name) : SimpleTypeSlimBase(assembly, name)
    {

        /// <summary>
        /// Gets the kind of the type.
        /// </summary>
        public override TypeSlimKind Kind => TypeSlimKind.Simple;
    }

    public partial class TypeSlim
    {
        /// <summary>
        /// Creates a new lightweight representation of a simple type, i.e. not constructed from other types.
        /// </summary>
        /// <param name="assembly">Assembly defining the type.</param>
        /// <param name="name">Name of the type.</param>
        /// <returns>A new lightweight representation of a simple type, i.e. not constructed from other types.</returns>
        public static SimpleTypeSlim Simple(AssemblySlim assembly, string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Type name can not be null or empty.", nameof(name));

            return new SimpleTypeSlim(assembly, name);
        }
    }
}
