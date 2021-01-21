// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD, ER - July 2013 - Created this file.
//

using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace System.Linq.CompilerServices
{
    /// <summary>
    /// Container for structural type property information.
    /// </summary>
    public class StructuralFieldDeclaration
    {
        #region Constructors

        /// <summary>
        /// Instantiates a container for structural type property information.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <param name="type">The type of the property.</param>
        /// <param name="customAttributes">The set of custom attributes to declare on the property.</param>
        public StructuralFieldDeclaration(string name, Type type, params CustomAttributeDeclaration[] customAttributes)
            : this(name, type, customAttributes.ToReadOnly())
        {
        }

        /// <summary>
        /// Instantiates a container for structural type property information.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <param name="type">The type of the property.</param>
        /// <param name="customAttributes">The set of custom attributes to declare on the property.</param>
        public StructuralFieldDeclaration(string name, Type type, IEnumerable<CustomAttributeDeclaration> customAttributes)
            : this(name, type, customAttributes.ToReadOnly())
        {
        }

        /// <summary>
        /// Instantiates a container for structural type property information.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <param name="type">The type of the property.</param>
        /// <param name="customAttributes">The set of custom attributes to declare on the property.</param>
        public StructuralFieldDeclaration(string name, Type type, ReadOnlyCollection<CustomAttributeDeclaration> customAttributes)
        {
            Name = name;
            PropertyType = type;
            CustomAttributes = customAttributes;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The name of the property.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The type of the property.
        /// </summary>
        public Type PropertyType { get; }

        /// <summary>
        /// The set of custom attributes to declare on the property.
        /// </summary>
        public ReadOnlyCollection<CustomAttributeDeclaration> CustomAttributes { get; }

        #endregion
    }
}
