// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
// ER - July 2013 - Small tweaks.
//

using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq.CompilerServices;

namespace System.Reflection
{
    /// <summary>
    /// Lightweight representation of a property.
    /// </summary>
    public class PropertyInfoSlim : MemberInfoSlim
    {
        #region Constructors

        /// <summary>
        /// Creates a new property representation object.
        /// </summary>
        /// <param name="declaringType">Type declaring the member.</param>
        /// <param name="name">Name of the property.</param>
        /// <param name="propertyType">Type of the property.</param>
        /// <param name="canWrite"><b>true</b> if property is writable, <b>false</b> otherwise.</param>
        internal PropertyInfoSlim(TypeSlim declaringType, string name, TypeSlim propertyType, bool canWrite)
            : base(declaringType)
        {
            Debug.Assert(!string.IsNullOrEmpty(name));

            Name = name;
            PropertyType = propertyType;
            CanWrite = canWrite;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the member type of the member.
        /// </summary>
        public sealed override MemberTypes MemberType => MemberTypes.Property;

        /// <summary>
        /// true if property is writable, false otherwise.
        /// </summary>
        public bool CanWrite { get; }

        /// <summary>
        /// Gets the name of the property.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the type of the property.
        /// </summary>
        public TypeSlim PropertyType { get; }

        /// <summary>
        /// Gets the types of the indexer parameters, if the property is indexed. Otherwise, an empty collection.
        /// </summary>
        public virtual ReadOnlyCollection<TypeSlim> IndexParameterTypes => EmptyReadOnlyCollection<TypeSlim>.Instance;

        #endregion

        #region Methods

        /// <summary>
        /// Creates a new property representation object.
        /// </summary>
        /// <param name="declaringType">Type declaring the member.</param>
        /// <param name="name">Name of the property.</param>
        /// <param name="propertyType">Type of the property.</param>
        /// <param name="canWrite"><b>true</b> if property is writable, <b>false</b> otherwise.</param>
        /// <param name="indexParameterTypes">Types of the indexer parameters, if the property is indexed. Otherwise, an empty collection.</param>
        /// <returns>A new property representation object.</returns>
        internal static PropertyInfoSlim Make(TypeSlim declaringType, string name, TypeSlim propertyType, ReadOnlyCollection<TypeSlim> indexParameterTypes, bool canWrite)
        {
            Debug.Assert(indexParameterTypes != null);

            if (indexParameterTypes.Count == 0)
            {
                return new PropertyInfoSlim(declaringType, name, propertyType, canWrite);
            }
            else
            {
                return new IndexedPropertyInfoSlim(declaringType, name, propertyType, canWrite, indexParameterTypes);
            }
        }

        #endregion
    }

    /// <summary>
    /// Lightweight representation of an indexed property.
    /// </summary>
    internal sealed class IndexedPropertyInfoSlim : PropertyInfoSlim
    {
        #region Constructors

        /// <summary>
        /// Creates a new indexed property representation object.
        /// </summary>
        /// <param name="declaringType">Type declaring the member.</param>
        /// <param name="name">Name of the property.</param>
        /// <param name="propertyType">Type of the property.</param>
        /// <param name="canWrite"><b>true</b> if property is writable, <b>false</b> otherwise.</param>
        /// <param name="indexParameterTypes">Types of the indexer parameters.</param>
        internal IndexedPropertyInfoSlim(TypeSlim declaringType, string name, TypeSlim propertyType, bool canWrite, ReadOnlyCollection<TypeSlim> indexParameterTypes)
            : base(declaringType, name, propertyType, canWrite)
        {
            IndexParameterTypes = indexParameterTypes;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the types of the indexer parameters.
        /// </summary>
        public override ReadOnlyCollection<TypeSlim> IndexParameterTypes { get; }

        #endregion
    }
}
