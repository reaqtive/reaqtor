// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2017 - Created this file.
//

using System.Reflection;

namespace System.Linq.Expressions
{
    /// <summary>
    /// Utility to create stable hash codes for expression trees.
    /// </summary>
    public class StableExpressionSlimHasher : ExpressionSlimHasher
    {
        private readonly StableExpressionSlimHashingOptions _options;

        /// <summary>
        /// Creates a new stable expression hasher with the specified <paramref name="options"/>.
        /// </summary>
        /// <param name="options">Options to influence the behavior of stable hashing of expression trees.</param>
        public StableExpressionSlimHasher(StableExpressionSlimHashingOptions options) => _options = options;

        /// <summary>
        /// Gets the hash code for the specified object <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The object value to get the hash code for.</param>
        /// <returns>A hash code for the specified object <paramref name="value"/>.</returns>
        protected override int GetHashCode(ObjectSlim value) => (_options & StableExpressionSlimHashingOptions.IgnoreConstants) != 0 ? 0 : base.GetHashCode(value);

        /// <summary>
        /// Gets the hash code for the specified string <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The string value to get the hash code for.</param>
        /// <returns>A hash code for the specified string <paramref name="value"/>.</returns>
        protected override int GetHashCode(string value) => value.GetMarvin32Hash(0L);

        /// <summary>
        /// Gets the hash code for the specified <paramref name="assembly"/>.
        /// </summary>
        /// <param name="assembly">The assembly to get the hash code for.</param>
        /// <returns>A hash code for the specified <paramref name="assembly"/>.</returns>
        protected override int GetHashCode(AssemblySlim assembly)
        {
            var name = assembly?.Name;

            if (name != null && (_options & StableExpressionSlimHashingOptions.UseAssemblySimpleName) != 0)
            {
#if NET6_0 || NETSTANDARD2_1
                var comma = name.IndexOf(',', StringComparison.Ordinal);
#else
                var comma = name.IndexOf(',');
#endif
                if (comma >= 0)
                {
                    name = name.Substring(0, comma);
                }
            }

            return GetHashCode(name);
        }
    }
}
