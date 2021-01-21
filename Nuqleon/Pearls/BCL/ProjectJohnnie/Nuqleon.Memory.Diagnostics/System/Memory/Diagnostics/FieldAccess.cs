// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 08/05/2017 - Created this type.
//

using System.Linq.Expressions;
using System.Reflection;

namespace System.Memory.Diagnostics
{
    /// <summary>
    /// Represents accessing a field.
    /// </summary>
    public sealed class FieldAccess : Access
    {
        /// <summary>
        /// Creates a new field access for the specified <paramref name="field"/>.
        /// </summary>
        /// <param name="field">The field to access.</param>
        internal FieldAccess(FieldInfo field)
        {
            Field = field;
        }

        /// <summary>
        /// Gets the access type, always returning <see cref="AccessType.Field"/>.
        /// </summary>
        public override AccessType AccessType => AccessType.Field;

        /// <summary>
        /// Gets the field being accessed.
        /// </summary>
        public new FieldInfo Field { get; }

        /// <summary>
        /// Applies the field access to the specified object <paramref name="obj"/>.
        /// </summary>
        /// <param name="obj">The object to apply the field access to.</param>
        /// <returns>The result of applying the field access to the specified object.</returns>
        public override object Apply(object obj) => Field.GetValue(obj);

        /// <summary>
        /// Converts the field access to an <see cref="MemberExpression"/> applied to the specified expression representing an object instance.
        /// </summary>
        /// <param name="obj">The expression representing an object instance to apply the field access to.</param>
        /// <returns>An expression that represents accessing the field on the specified object instance.</returns>
        public override Expression ToExpression(Expression obj) => Expression.Field(obj, Field);

        /// <summary>
        /// Gets a friendly string representation of the field access.
        /// </summary>
        /// <returns>A friendly string representation of the field access.</returns>
        public override string ToString() => "." + Field.Name;
    }
}
