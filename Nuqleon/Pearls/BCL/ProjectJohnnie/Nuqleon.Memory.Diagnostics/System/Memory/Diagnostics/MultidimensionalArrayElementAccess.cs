// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 08/05/2017 - Created this type.
//

using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace System.Memory.Diagnostics
{
    /// <summary>
    /// Represents accessing an element in a multi-dimensional array.
    /// </summary>
    public sealed class MultidimensionalArrayElementAccess : Access
    {
        /// <summary>
        /// Creates a new element access for an element at the specified <paramref name="indexes"/>.
        /// </summary>
        /// <param name="indexes">The indexes of the element to access.</param>
        internal MultidimensionalArrayElementAccess(int[] indexes)
        {
            Indexes = indexes;
        }

        /// <summary>
        /// Gets the access type, always returning <see cref="AccessType.MultidimensionalArrayElement"/>.
        /// </summary>
        public override AccessType AccessType => AccessType.MultidimensionalArrayElement;

        /// <summary>
        /// Gets the indexes of the element to access.
        /// </summary>
        public IReadOnlyList<int> Indexes { get; }

        /// <summary>
        /// Applies the element access to the specified object <paramref name="obj"/>.
        /// </summary>
        /// <param name="obj">The object to apply the element access to.</param>
        /// <returns>The result of applying the element access to the specified object.</returns>
        public override object Apply(object obj) => ((Array)obj).GetValue(Indexes.ToArray());

        /// <summary>
        /// Converts the element access to an <see cref="IndexExpression"/> applied to the specified expression representing an array instance.
        /// </summary>
        /// <param name="obj">The expression representing an array instance to apply the element access to.</param>
        /// <returns>An expression that represents accessing the element on the specified array instance.</returns>
        public override Expression ToExpression(Expression obj) => Expression.ArrayAccess(obj, Indexes.Select(i => Expression.Constant(i)));

        /// <summary>
        /// Gets a friendly string representation of the element access.
        /// </summary>
        /// <returns>A friendly string representation of the element access.</returns>
        public override string ToString() => "[" + string.Join(", ", Indexes) + "]";
    }
}
