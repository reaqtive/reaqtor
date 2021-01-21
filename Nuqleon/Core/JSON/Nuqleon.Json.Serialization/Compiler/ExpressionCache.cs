// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 07/04/2017 - Added caching of expression nodes.
//

using System.Linq.Expressions;

namespace Nuqleon.Json.Serialization
{
    /// <summary>
    /// Provides access to some cached expression tree nodes.
    /// </summary>
    internal static class ExpressionUtils
    {
        /// <summary>
        /// Gets a <see cref="ConstantExpression"/> instance with value <c>false</c>.
        /// </summary>
        public static readonly Expression ConstantFalse = Expression.Constant(false);

        /// <summary>
        /// Gets a <see cref="ConstantExpression"/> instance with value <c>true</c>.
        /// </summary>
        public static readonly Expression ConstantTrue = Expression.Constant(true);

        /// <summary>
        /// Gets a <see cref="DefaultExpression"/> instance of type <see cref="void"/>.
        /// </summary>
        public static readonly Expression Empty = Expression.Empty();
    }
}
