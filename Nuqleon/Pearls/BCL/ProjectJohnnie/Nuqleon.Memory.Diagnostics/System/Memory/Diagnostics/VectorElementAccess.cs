// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 08/05/2017 - Created this type.
//

using System.Linq.Expressions;

namespace System.Memory.Diagnostics
{
    /// <summary>
    /// Represents accessing an element in a single-dimensional array.
    /// </summary>
    public sealed class VectorElementAccess : Access
    {
        /// <summary>
        /// Creates a new element access for an element at the specified <paramref name="index"/>.
        /// </summary>
        /// <param name="index">The index of the element to access.</param>
        internal VectorElementAccess(int index)
        {
            Index = index;
        }

        /// <summary>
        /// Gets the access type, always returning <see cref="AccessType.VectorElement"/>.
        /// </summary>
        public override AccessType AccessType => AccessType.VectorElement;

        /// <summary>
        /// Gets the index of the element to access.
        /// </summary>
        public int Index { get; }

        /// <summary>
        /// Applies the element access to the specified object <paramref name="obj"/>.
        /// </summary>
        /// <param name="obj">The object to apply the element access to.</param>
        /// <returns>The result of applying the element access to the specified object.</returns>
        public override object Apply(object obj) => ((Array)obj).GetValue(Index);

        /// <summary>
        /// Converts the element access to an <see cref="IndexExpression"/> applied to the specified expression representing an array instance.
        /// </summary>
        /// <param name="obj">The expression representing an array instance to apply the element access to.</param>
        /// <returns>An expression that represents accessing the element on the specified array instance.</returns>
        public override Expression ToExpression(Expression obj) => Expression.ArrayAccess(obj, Expression.Constant(Index));

        /// <summary>
        /// Gets a friendly string representation of the element access.
        /// </summary>
        /// <returns>A friendly string representation of the element access.</returns>
        public override string ToString() => "[" + Index + "]";
    }
}
