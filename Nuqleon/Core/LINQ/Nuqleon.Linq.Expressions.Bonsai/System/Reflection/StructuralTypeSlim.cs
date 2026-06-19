// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - July 2013 - Created this file.
//

using System.Collections.ObjectModel;

namespace System.Reflection
{
    /// <summary>
    /// Base type for lightweight representations of structural types.
    /// </summary>
    public abstract class StructuralTypeSlim : TypeSlim
    {
        #region Properties

        /// <summary>
        /// Gets the kind of the type.
        /// </summary>
        public sealed override TypeSlimKind Kind => TypeSlimKind.Structural;

        /// <summary>
        /// true if type uses value equality semantics, false otherwise.
        /// </summary>
        public abstract bool HasValueEqualitySemantics { get; }

        /// <summary>
        /// Gets the members of the structural type.
        /// </summary>
        public abstract ReadOnlyCollection<PropertyInfoSlim> Properties { get; }

        /// <summary>
        /// Gets the kind of structural type.
        /// </summary>
        public abstract StructuralTypeSlimKind StructuralKind { get; }

        #endregion
    }

    /// <summary>
    /// Lightweight representation of a structural type.
    /// </summary>
    internal sealed class ReadOnlyStructuralTypeSlim : StructuralTypeSlim
    {
        // PERF: Consider creating specialized layouts for this type. Note this is not too pressing
        //       because the Structural factory method is rarely used. Instead, these types are often
        //       built using the StructuralTypeSlimReference.Create factory methods using a builder
        //       pattern.

        #region Constructors

        /// <summary>
        /// Creates a new structural type representation object.
        /// </summary>
        /// <param name="properties">The set of properties for the structural type.</param>
        /// <param name="hasValueEqualitySemantics">true if type uses value equality semantics, false otherwise.</param>
        /// <param name="kind">The kind of structural type to create/serialize.</param>
        public ReadOnlyStructuralTypeSlim(ReadOnlyCollection<PropertyInfoSlim> properties, bool hasValueEqualitySemantics, StructuralTypeSlimKind kind)
        {
            Properties = properties;
            StructuralKind = kind;
            HasValueEqualitySemantics = hasValueEqualitySemantics;
        }

        #endregion

        #region Properties

        /// <summary>
        /// true if type uses value equality semantics, false otherwise.
        /// </summary>
        public override bool HasValueEqualitySemantics { get; }

        /// <summary>
        /// Gets the members of the structural type.
        /// </summary>
        public override ReadOnlyCollection<PropertyInfoSlim> Properties { get; }

        /// <summary>
        /// Gets the kind of structural type.
        /// </summary>
        public override StructuralTypeSlimKind StructuralKind { get; }

        #endregion
    }

    public partial class TypeSlim
    {
        /// <summary>
        /// Creates a new lightweight representation of a structural type with the specified properties.
        /// </summary>
        /// <param name="properties">The properties of the structural type.</param>
        /// <param name="hasValueEqualitySemantics">true if the structural type has value equality semantics, false otherwise.</param>
        /// <param name="kind">The kind of structural type.</param>
        /// <returns>A new lightweight representation of a structural type with the specified properties.</returns>
        public static StructuralTypeSlim Structural(ReadOnlyCollection<PropertyInfoSlim> properties, bool hasValueEqualitySemantics, StructuralTypeSlimKind kind)
        {
            RequireNotNull(properties, nameof(properties));

            return new ReadOnlyStructuralTypeSlim(properties, hasValueEqualitySemantics, kind);
        }
    }
}
