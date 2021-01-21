// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;
using System.Linq.CompilerServices;
using System.Linq.Expressions;

namespace Reaqtor.ReificationFramework
{
    internal static class ExpressionHelpers
    {
        public static Expression BetaReduce(this Expression expression)
        {
            return BetaReducer.ReduceEager(
                expression,
                BetaReductionNodeTypes.Unrestricted,
                BetaReductionRestrictions.None,
                true);
        }

        public static Expression<T> BetaReduce<T>(this Expression<T> expression)
        {
            return (Expression<T>)BetaReducer.ReduceEager(
                expression,
                BetaReductionNodeTypes.Unrestricted,
                BetaReductionRestrictions.None,
                true);
        }

        public static IEnumerable<T> StartWith<T>(this IEnumerable<T> rest, params T[] items)
        {
            return items.Concat(rest);
        }

        public static IEnumerable<T> EndWith<T>(this IEnumerable<T> items, params T[] rest)
        {
            return items.Concat(rest);
        }
    }
}
