// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - December 2012
//

using System.Globalization;

namespace Nuqleon.Linq.Expressions.Serialization.TypeSystem
{
    using Json = Nuqleon.Json.Expressions;

    /// <summary>
    /// Represents a reference to a type definition.
    /// </summary>
    internal sealed class TypeRef
    {
        #region Constructors

        /// <summary>
        /// Creates a new type reference with the given ordinal.
        /// </summary>
        /// <param name="ordinal">Ordinal number to identify the type reference.</param>
        public TypeRef(int ordinal)
        {
            Ordinal = ordinal;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the ordinal number to identify the type reference. Used for equality comparison.
        /// </summary>
        public int Ordinal { get; }

        /// <summary>
        /// Gets or sets the definition associated with this type reference.
        /// </summary>
        public TypeDef Definition { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the JSON representation of the type reference.
        /// </summary>
        /// <returns>JSON representation of the type reference.</returns>
        public Json.Expression ToJson()
        {
            return Json.Expression.Number(Ordinal.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Obtains a type reference object from a JSON representation.
        /// </summary>
        /// <param name="json">JSON representation of the type reference.</param>
        /// <returns>Type reference for the given JSON representation.</returns>
        public static TypeRef FromJson(Json.Expression json)
        {
            var ordinal = int.Parse((string)((Json.ConstantExpression)json).Value, CultureInfo.InvariantCulture);
            return new TypeRef(ordinal);
        }

        /// <summary>
        /// Gets a hash code value for the current type reference instance.
        /// </summary>
        /// <returns>Hash code value for the current type reference instance.</returns>
        public override int GetHashCode()
        {
            return Ordinal.GetHashCode();
        }

        /// <summary>
        /// Checks whether the current instance is equal to the given object.
        /// </summary>
        /// <param name="obj">Object to check for equality.</param>
        /// <returns>true if the given object is a type reference with the same ordinal; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            return obj is TypeRef other && Ordinal == other.Ordinal;
        }

        #endregion
    }
}
