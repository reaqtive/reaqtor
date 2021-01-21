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
    /// Represents an access composed of one or more <see cref="Access"/> instances.
    /// </summary>
    public sealed class CompositeAccess : Access
    {
        /// <summary>
        /// Creates a new composite access using the specified <paramref name="accesses"/>.
        /// </summary>
        /// <param name="accesses">The accesses to apply left-to-right.</param>
        internal CompositeAccess(Access[] accesses)
        {
            Accesses = accesses;
        }

        /// <summary>
        /// Gets the access type, always returning <see cref="AccessType.Composite"/>.
        /// </summary>
        public override AccessType AccessType => AccessType.Composite;

        /// <summary>
        /// Gets the list of accesses being applied left-to-right.
        /// </summary>
        public IReadOnlyList<Access> Accesses { get; }

        /// <summary>
        /// Applies the composite access to the specified object <paramref name="obj"/>.
        /// </summary>
        /// <param name="obj">The object to apply the composite access to.</param>
        /// <returns>The result of applying the composite access to the specified object.</returns>
        public override object Apply(object obj) => Accesses.Aggregate(obj, (o, a) => a.Apply(o));

        /// <summary>
        /// Converts the composite access to an <see cref="Expression"/> applied to the specified expression representing an object instance.
        /// </summary>
        /// <param name="obj">The expression representing an object instance to apply the accesses to.</param>
        /// <returns>An expression that represents performing the accesses left-to-right on the specified object instance.</returns>
        public override Expression ToExpression(Expression obj) => Accesses.Aggregate(obj, (o, a) => a.ToExpression(o));

        /// <summary>
        /// Gets a friendly string representation of the composite access.
        /// </summary>
        /// <returns>A friendly string representation of the composite access.</returns>
        public override string ToString() => string.Join("", Accesses);
    }
}
