// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Linq.Expressions;

namespace Nuqleon.DataModel.Serialization.Binary
{
    internal static class ExpressionHelpers
    {
        public static Expression For(Expression initialize, Expression conditional, Expression postIncrement, Expression action)
        {
            var label = Expression.Label(typeof(void));
            return Expression.Block(
                initialize,
                Expression.Loop(
                    Expression.IfThenElse(
                        conditional,
                        Expression.Block(action, postIncrement),
                        Expression.Break(label)
                    ),
                    label
                )
            );
        }
    }
}
