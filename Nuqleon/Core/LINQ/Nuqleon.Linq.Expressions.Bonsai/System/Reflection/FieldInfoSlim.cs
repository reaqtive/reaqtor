// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
// ER - July 2013 - Small tweaks.
//

using System.Diagnostics;

namespace System.Reflection
{
    /// <summary>
    /// Lightweight representation of a field.
    /// </summary>
    public sealed class FieldInfoSlim : MemberInfoSlim
    {
        /// <summary>
        /// Creates a new field representation object.
        /// </summary>
        /// <param name="declaringType">Type declaring the member.</param>
        /// <param name="name">Name of the field.</param>
        /// <param name="fieldType">Type of the field.</param>
        internal FieldInfoSlim(TypeSlim declaringType, string name, TypeSlim fieldType)
            : base(declaringType)
        {
            Debug.Assert(!string.IsNullOrEmpty(name));

            Name = name;
            FieldType = fieldType;
        }

        /// <summary>
        /// Gets the member type of the member.
        /// </summary>
        public sealed override MemberTypes MemberType => MemberTypes.Field;

        /// <summary>
        /// Gets the name of the field.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the type of the field.
        /// </summary>
        public TypeSlim FieldType { get; }
    }
}
