// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
// ER - June 2013 - Created this file.
//

namespace System.Reflection
{
    /// <summary>
    /// Lightweight representation of a generic parameter type.
    /// </summary>
    public sealed class GenericParameterTypeSlim : TypeSlim
    {
        /// <summary>
        /// The hash code to use for unbound generic parameters. We intentionally
        /// cause all unbound generic parameters to return the same hash code in
        /// order to leave the equality decision making to the context in which
        /// the type occurs. By default, reference equality will be used, but
        /// other facilities such as <see cref="MemberInfoSlimEqualityComparer"/>
        /// can override this behavior and take generic parameter bindings into
        /// consideration.
        /// </summary>
        internal const int UnboundHashCode = -1;

        /// <summary>
        /// Creates a new generic parameter type representation object.
        /// </summary>
        /// <param name="name">Name of the generic parameter type (for diagnostic purposes only).</param>
        internal GenericParameterTypeSlim(string name)
        {
            Name = name;
            _hashCode = UnboundHashCode;
        }

        /// <summary>
        /// Gets the kind of the type.
        /// </summary>
        public sealed override TypeSlimKind Kind => TypeSlimKind.GenericParameter;

        /// <summary>
        /// Gets the name of the generic parameter type (for diagnostic purposes only).
        /// </summary>
        public string Name { get; }
    }

    public partial class TypeSlim
    {
        /// <summary>
        /// Creates a new lightweight representation of a generic parameter type.
        /// </summary>
        /// <param name="name">Name of the generic parameter type (for diagnostic purposes only).</param>
        /// <returns>A new lightweight representation of a generic parameter type.</returns>
        public static GenericParameterTypeSlim GenericParameter(string name)
        {
            // CONSIDER: Allow null and use a specialized layout to conserve memory.

            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Type name can not be null or empty.", nameof(name));

            return new GenericParameterTypeSlim(name);
        }
    }
}
