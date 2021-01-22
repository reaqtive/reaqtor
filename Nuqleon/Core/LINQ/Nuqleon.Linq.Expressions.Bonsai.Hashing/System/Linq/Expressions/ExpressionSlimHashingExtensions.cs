// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2017 - Created this file.
//

namespace System.Linq.Expressions
{
    /// <summary>
    /// Provides a set of extension methods to perform stable hashing of expression trees.
    /// </summary>
    public static class ExpressionSlimHashingExtensions
    {
        private static readonly StableExpressionSlimHasher s_none = new(StableExpressionSlimHashingOptions.None);
        private static readonly StableExpressionSlimHasher s_all = new(StableExpressionSlimHashingOptions.All);

        /// <summary>
        /// Gets a stable hash code for the specified <paramref name="expression"/> using the specified <paramref name="options"/>.
        /// </summary>
        /// <param name="expression">The expression tree to compute a stable hash code for.</param>
        /// <param name="options">Options to influence the behavior of stable hashing of expression trees.</param>
        /// <returns>A stable hash code for the specified <paramref name="expression"/>.</returns>
        public static int GetStableHashCode(this ExpressionSlim expression, StableExpressionSlimHashingOptions options)
        {
            var instance = options switch
            {
                StableExpressionSlimHashingOptions.None => s_none,
                StableExpressionSlimHashingOptions.All => s_all,
                _ => new StableExpressionSlimHasher(options),
            };

            return instance.GetHashCode(expression);
        }
    }
}
